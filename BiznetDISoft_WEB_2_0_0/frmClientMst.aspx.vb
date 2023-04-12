Imports Microsoft.Win32
Imports System.Web.Services
Imports Newtonsoft.Json
Imports Microsoft
Imports Microsoft.Office.Interop
Imports System.IO
Imports System.Drawing

Partial Class frmClientMst
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "

    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtClientMst As String = "DtClientMst"
    Private Const VS_ClientCode As String = "vClientCode"
    Private Const VS_ClientMst As String = "ClientMst"
    Private rPage As RepoPage
    Private index1 As Integer = 0

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_Code As Integer = 1
    Private Const GVC_ClientName As Integer = 2
    Private Const GVC_ProjectManager As Integer = 3
    Private Const GVC_Remark As Integer = 4
    Private Const GVC_UserID As Integer = 5
    Private Const GVC_Edit As Integer = 6
    Private WithClientContact As Boolean = True

#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            GenCall()

        End If
        If gvclient.Rows.Count > 0 Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvClient", "UIgvClient(); ", True)   'Added by ketan
        End If
    End Sub
#End Region

#Region "GenCall"
    Private Function GenCall() As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim dt_ClientMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try



            Choice = Me.Request.QueryString("mode").ToString
            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_ClientCode) = Me.Request.QueryString("Value").ToString
            End If

            ''''Check for Valid User''''''''''''''
            If Not GenCall_Data(Choice, dt_ClientMst) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtClientMst) = dt_ClientMst ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI(Choice, dt_ClientMst) Then 'For Displaying Data 
                Exit Function
            End If

            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideSponsorDetails", "HideSponsorDetails(); ", True)
            End If
            GenCall = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "..GenCall")
        Finally
        End Try
    End Function
#End Region

#Region "GenCall_Data"
    Private Function GenCall_Data(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                            ByRef dt_Dist_Retu As DataTable) As Boolean

        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_Clientmst As DataSet = Nothing

        Try


            If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vClientCode=" + Me.ViewState(VS_ClientCode).ToString() 'Value of where condition
            End If

            wStr += " And cStatusIndi <> 'D'"

            If Not objHelp.get_Viewclientmst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Clientmst, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_Clientmst Is Nothing Then
                Throw New Exception(eStr)
            End If

            If ds_Clientmst.Tables(0).Rows.Count <= 0 And _
               Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Throw New Exception("No Records Found for Selected role")

            End If

            dt_Dist_Retu = ds_Clientmst.Tables(0)
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_Data")
        Finally
        End Try
    End Function
#End Region

#Region "GenCall_ShowUI"
    Private Function GenCall_ShowUI(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                                      ByVal dt_ClientMst As DataTable) As Boolean
        Try
            Page.Title = " :: Client Master ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            'CType(Master.FindControl("lblHeading"), Label).Text = "Sponsor Master"
            CType(Master.FindControl("lblHeading"), Label).Text = "Client Master"
            
            If Not FillGridClient() Then
                Return False
            End If
            If Not FillddlProjectManager() Then
                Return False
            End If

            If Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.txtClientName.Text = ConvertDbNullToDbTypeDefaultValue(dt_ClientMst.Rows(0)("vClientName"), dt_ClientMst.Rows(0)("vClientName").GetType)
                'Me.txtremark.Text = ConvertDbNullToDbTypeDefaultValue(dt_ClientMst.Rows(0)("vRemark"), dt_ClientMst.Rows(0)("vRemark").GetType)
                Me.txtremark.Text = ""

                If Convert.ToString(dt_ClientMst.Rows(0)("ProjectMangerWithProfile")).Trim() <> "" Then
                    ddlProjectMngr.SelectedValue = Convert.ToString(dt_ClientMst.Rows(0)("iUserId"))
                End If

                Me.btnSave.Text = "Update"
                Me.btnSave.ToolTip = "Update"
            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_ShowUI")
        Finally
        End Try

    End Function

#End Region

#Region "Fill GridClient"

    Private Function FillGridClient() As Boolean
        Dim eStr As String = String.Empty
        Dim ds_ClientMst As New DataSet
        Dim dt_client As New DataTable
        Try

            'Commented by ketan
            If Not objHelp.get_Viewclientmst("cStatusIndi <> 'D' order by vClientName", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ClientMst, eStr) Then
                Throw New Exception(eStr)
            End If

            '=========added on 5-dec-09=====
            dt_client = ds_ClientMst.Tables(0)
            ViewState(VS_ClientMst) = dt_client
            '==============================
            Me.gvclient.DataSource = ds_ClientMst
            Me.gvclient.DataBind()


            If gvclient.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvClient", "UIgvClient(); ", True)   ''Added by ketan
            End If
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....FillGridClient")
            Return False
        End Try

    End Function

#End Region

#Region "Fill ddlProject Manger"

    Private Function FillddlProjectManager() As Boolean
        Dim dsProjectMngr As New DataSet
        Dim wstr As String = String.Empty
        Try
            dsProjectMngr = Me.objHelp.GetResultSet("Select iUserId,ProjectManagerWithProfile From View_ProjectManager order by ProjectManager", "View_ProjectManager")
            If Not dsProjectMngr Is Nothing Then
                If dsProjectMngr.Tables.Count > 0 Then
                    If dsProjectMngr.Tables(0).Rows.Count > 0 Then
                        ddlProjectMngr.Items.Clear()
                        Me.ddlProjectMngr.DataSource = dsProjectMngr.Tables(0)
                        Me.ddlProjectMngr.DataValueField = "iUserId"
                        Me.ddlProjectMngr.DataTextField = "ProjectManagerWithProfile"
                        Me.ddlProjectMngr.DataBind()
                        Me.ddlProjectMngr.Items.Insert(0, "Select Project Manager")
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....FillddlProjectManager")
            Return False
        End Try

    End Function

#End Region

#Region "Reset Page"
    Private Sub ResetPage()
        Me.txtClientName.Text = ""
        Me.txtremark.Text = ""
        'Response.Redirect("frmClientMst.aspx?mode=1")
    End Sub
#End Region

#Region "Save"

    Private Function AssignUpdatedValues() As Boolean
        Dim dtOld As New DataTable
        Dim dr As DataRow
        Dim ds_Check As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Try

            dtOld = Me.ViewState(VS_DtClientMst)
            'For Validation of Duplication

            wStr = "cStatusIndi <> 'D' And  upper(vClientName ) ='" & Me.txtClientName.Text.Trim.Replace("'", "''").ToUpper() & "'"
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                wStr += " And vClientCode <> '" + Me.ViewState(VS_ClientCode).ToString() + "'"
            End If

            If Not objHelp.getclientmst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_Check, eStr) Then

                Me.ShowErrorMessage("Error While Getting Data From ClientMst !", eStr)
                Exit Function

            End If
            '====
            'ds_Check = Me.objHelp.GetResultSet("Select * from View_ClientMst order by vClientName ", "View_ClientMst") ''Added by ketan
            If ds_Check.Tables(0).Rows.Count > 0 Then

                objcommon.ShowAlert("Client Name Already Exists !", Me)
                If gvclient.Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvClient", "UIgvClient(); ", True)   ''Added by ketan
                End If
                Exit Function

            End If
            '************************

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                ds_Check.Clear()
                dr = ds_Check.Tables(0).NewRow
                dr("vClientName") = Me.txtClientName.Text.Trim
                dr("iPmId") = Me.ddlProjectMngr.SelectedValue  ''Added by ketan
                dr("vRemark") = Me.txtremark.Text.Trim
                dr("vClientCode") = "0000"
                dr("iModifyBy") = Session(S_UserID)
                ds_Check.Tables(0).Rows.Add(dr)

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                ds_Check.Clear()
                dr = ds_Check.Tables(0).NewRow
                dr("vClientCode") = Me.ViewState(VS_ClientCode).ToString()
                dr("vClientName") = Me.txtClientName.Text.Trim
                dr("iPmId") = Me.ddlProjectMngr.SelectedValue  '' Added by ketan
                dr("vRemark") = Me.txtremark.Text.Trim
                dr("iModifyBy") = Session(S_UserID)
                dr("cStatusIndi") = "E"
                ds_Check.Tables(0).Rows.Add(dr)
                ds_Check.AcceptChanges()

            End If

            Me.ViewState(VS_DtClientMst) = ds_Check.Tables(0)
            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...AssignUpdatedValues")
            Return False
        End Try
    End Function

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim Dt As New DataTable
        Dim Ds_stagemat As DataSet
        Dim ds_LocGrid As New DataSet
        Dim eStr As String = String.Empty
        Dim message As String = String.Empty

        Try

            If Not AssignUpdatedValues() Then
                Exit Sub
            End If

            Ds_stagemat = New DataSet
            Ds_stagemat.Tables.Add(CType(Me.ViewState(VS_DtClientMst), Data.DataTable).Copy())
            'Ds_stagemat.Tables(0).TableName = "ClientMst"   ' New Values on the form to be updated
            'Ds_stagemat.Tables(0).Columns.Remove("ProjectManger")
            'Ds_stagemat.Tables(0).Columns.Remove("iUserId")

            If Not objLambda.Save_InsertClientmst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_clientmst, Ds_stagemat, "1", eStr) Then

                objcommon.ShowAlert("Error While Saving ClientMst !", Me.Page)
                Exit Sub

            End If

            ResetPage()
            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "Client Details Saved Successfully !", "Client Details Updated Successfully !")
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('" + message + "')", True)
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".......btnSave_Click")

        End Try
    End Sub

#End Region

#Region "Cancel"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ResetPage()
        'btnSave.Attributes.Add("style", "display:block")
        Me.Response.Redirect("frmClientMst.aspx?mode=1")
    End Sub

#End Region

#Region "Close"
    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnClose.Click

        Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

#Region "BindGrid"
    Private Sub BindGrid()
        Dim dsClient As New DataSet
        Dim eStr As String = String.Empty

        If objHelp.get_Viewclientmst("cStatusIndi <> 'D' order by vClientName", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsClient, eStr) Then

            gvclient.DataSource = dsClient
            gvclient.DataBind()
            If gvclient.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvClient", "UIgvClient(); ", True)   ''Added by ketan
            End If
            dsClient.Dispose()

        End If

    End Sub
#End Region

#Region "Button Events"
    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        MPE_Export.Show()
        'divExportClient.Attributes.Add("style", "display:block")
    End Sub

    Protected Sub btnDivExp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDivExp.Click


        Dim wstr As String = String.Empty
        Dim errStr As String = String.Empty
        Dim dv_client As DataView
        Dim dsclient As New DataSet
        Dim str As String = String.Empty
        Dim dt As New DataTable
        Dim FileName As String = String.Empty
        Dim isReportComplete As Boolean = False
        Dim dtclient As New DataTable
        Dim cnt As Integer = 0


        Try
            If rblst.Items(0).Selected = True Then


                ' divExportClient.Attributes.Add("style", "display:none")
                wstr = " cStatusIndi <> 'D'"

                If Not objHelp.GetViewClientContactMatrix(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsclient, errStr) Then

                    Me.objcommon.ShowAlert("Error While Binding Grid:" & errStr, Me.Page())
                    'Return False

                End If

                dv_client = dsclient.Tables(0).DefaultView
                dv_client.Sort = "vClientName,vContactName"
                dtclient = dv_client.ToTable.Copy

                Dim a As New ArrayList
                a.Add("sad")

                FileName = GetReportName() + ".xls"
                FileName = Server.MapPath("~/ExcelReportFile/" + FileName)

                OpenReport(FileName)

                ReportHeader()

                ReportDetail(dtclient, True)

                isReportComplete = True

            ElseIf rblst.Items(1).Selected = True Then


                dt = CType(ViewState(VS_ClientMst), DataTable)
                Dim a As New ArrayList
                a.Add("sad")

                FileName = GetReportName() + ".xls"
                FileName = Server.MapPath("~/ExcelReportFile/" + FileName)

                OpenReport(FileName)

                ReportHeader()

                ReportDetail(dt, False)

                isReportComplete = True

            End If

        Catch ex As Exception
            isReportComplete = False
            Me.ShowErrorMessage(ex.Message, "...btnDivExp_Click")
        Finally
            If Not rPage Is Nothing Then
                rPage.CloseReport()
                rPage = Nothing
            End If
        End Try

        If isReportComplete = True Then

            ReportGeneralFunction.ExportFileAtWeb(ReportGeneralFunction.ExportFileAsEnum.ExportAsXLS, HttpContext.Current.Response, FileName)
        End If
    End Sub

    Protected Sub btnDivExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDivExit.Click
        'divExportClient.Attributes.Add("style", "display:none")
    End Sub

    Protected Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        'Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_ClientMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtClientMstHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable


        Dim filename As String = String.Empty
        Dim vTableName As String = String.Empty
        Dim vIdName As String = String.Empty
        Dim AuditFieldName As String = String.Empty
        Dim AuditFieldValue As String = String.Empty
        Dim vOtherTableName As String = String.Empty
        Dim vOtherIdName As String = String.Empty
        Dim strMessage As New StringBuilder

        Try

            'vTableName = "ClientMstHistory"
            'vIdName = ""
            'AuditFieldName = "vClientCode"
            'AuditFieldValue = hdnClientCode.Value
            'vOtherTableName = ""
            'vOtherIdName = ""

            wStr = " vClientCode = '" + hdnClientCode.Value + "' Order by nClientMstHistoryNo DESC"
            If Not objHelp.GetView_clientmstHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ClientMst, estr) Then
                Throw New Exception(estr)
            End If

            If Not dtClientMstHistory Is Nothing Then
                dtClientMstHistory.Columns.Add("Sr. No")
                dtClientMstHistory.Columns.Add("Client Name")
                dtClientMstHistory.Columns.Add("Project Manager")
                dtClientMstHistory.Columns.Add("Remarks")
                dtClientMstHistory.Columns.Add("ModifyBy")
                dtClientMstHistory.Columns.Add("ModifyOn")
            End If

            dtClientMstHistory.AcceptChanges()
            dt = ds_ClientMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtClientMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Client Name") = dr("vClientName").ToString()
                drAuditTrail("Project Manager") = dr("ProjectManager").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("dModifyOn"))
                dtClientMstHistory.Rows.Add(drAuditTrail)
                dtClientMstHistory.AcceptChanges()
                i += 1
            Next

            gvExport.DataSource = dtClientMstHistory
            gvExport.DataBind()
            If gvExport.Rows.Count > 0 Then

                gvExport.HeaderRow.BackColor = Color.White
                gvExport.FooterRow.BackColor = Color.White
                For Each cell As TableCell In gvExport.HeaderRow.Cells
                    cell.BackColor = gvExport.HeaderStyle.BackColor
                Next
                For Each row As GridViewRow In gvExport.Rows
                    row.BackColor = Color.White
                    row.Height = 20
                    For Each cell As TableCell In row.Cells
                        If row.RowIndex Mod 2 = 0 Then
                            cell.BackColor = gvExport.AlternatingRowStyle.BackColor
                            cell.ForeColor = gvExport.RowStyle.ForeColor
                        Else
                            cell.BackColor = gvExport.RowStyle.BackColor
                            cell.ForeColor = gvExport.RowStyle.ForeColor
                        End If
                        cell.CssClass = "textmode"
                    Next
                Next

                'style to format numbers to string
                Dim style As String = "<style> .textmode { } </style>"
                Response.Write(style)



                Dim info As String = String.Empty
                Dim gridviewHtml As String = String.Empty

                filename = "Audit Trail_" + hdnClientCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                Dim stringWriter As New System.IO.StringWriter()

                Dim writer As New HtmlTextWriter(stringWriter)
                gvExport.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")





                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Client Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")


                gridviewHtml = strMessage.ToString() + gridviewHtml
                Context.Response.Buffer = True
                Context.Response.ClearContent()
                Context.Response.ClearHeaders()

                Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
                Context.Response.AddHeader("content-Disposition", "attachment; filename=" + filename)
                Context.Response.AddHeader("Content-Length", gridviewHtml.Length)

                Context.Response.Write(gridviewHtml)
                Context.Response.Flush()
                Context.Response.End()

                System.IO.File.Delete(filename)
            Else

                Dim info As String = String.Empty
                Dim gridviewHtml As String = String.Empty

                filename = "Audit Trail_" + hdnClientCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"


                drAuditTrail = dtClientMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Client Name") = ""
                drAuditTrail("Project Manager") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("ModifyBy") = ""
                drAuditTrail("ModifyOn") = ""
                dtClientMstHistory.Rows.Add(drAuditTrail)
                dtClientMstHistory.AcceptChanges()
                gvExport.DataSource = dtClientMstHistory
                gvExport.DataBind()


                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                gvExport.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()


                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Client Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td align=""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")


                gridviewHtml = strMessage.ToString() + gridviewHtml
                Context.Response.Buffer = True
                Context.Response.ClearContent()
                Context.Response.ClearHeaders()

                Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
                Context.Response.AddHeader("content-Disposition", "attachment; filename=" + filename)
                Context.Response.AddHeader("Content-Length", gridviewHtml.Length)

                Context.Response.Write(gridviewHtml)
                Context.Response.Flush()
                Context.Response.End()

                System.IO.File.Delete(filename)
                Exit Sub
            End If
        Catch ex As Exception
            Me.objcommon.ShowAlert("Error While Exporting Data To Excel : " + estr, Me.Page)

            Exit Sub
        End Try
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub
#End Region

#Region "Grid Event"

    '  Protected Sub gvClient_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
    '      'If e.Row.RowType = DataControlRowType.DataRow Or _
    '      '      e.Row.RowType = DataControlRowType.Header Or _
    '      '      e.Row.RowType = DataControlRowType.Footer Then
    '
    '      '    e.Row.Cells(GVC_Code).Visible = False
    '      'End If
    '  End Sub

    ' Protected Sub gvClient_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
    '     gvclient.PageIndex = e.NewPageIndex
    '     BindGrid()
    ' End Sub

    Protected Sub gvClient_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        'Dim i As Integer = e.CommandArgument
        If e.CommandName.ToUpper = "EDIT" Then
            Me.Response.Redirect("frmClientMst.aspx?mode=2&value=" & Me.gvclient.Rows(e.CommandArgument).Cells(GVC_Code).Text.Trim())
        End If
        If e.CommandName.ToUpper = "ADD CONTACTS" Then
            Me.Response.Redirect("frmClientContactMatrix.aspx?mode=1&ClientCode=" & Me.gvclient.Rows(e.CommandArgument).Cells(GVC_Code).Text.Trim())
        End If
        If e.CommandName.Contains("ExportToExcel") Then
            hdnClientCode.Value = e.CommandName.Replace("ExportToExcel", "")
            btnExportToExcel_Click(Nothing, Nothing)
        End If
    End Sub

    Protected Sub gvClient_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Dim strCellValue As String = ""
        If Not e.Row.RowType = DataControlRowType.Pager Then
            e.Row.Cells(GVC_Code).Visible = False
            e.Row.Cells(GVC_UserID).Visible = False

            If e.Row.RowType = DataControlRowType.DataRow Then

                e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (gvclient.PageSize * gvclient.PageIndex) + 1
                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "Edit"
                CType(e.Row.FindControl("lnkAdd"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkAdd"), ImageButton).CommandName = "ADD CONTACTS"
                CType(e.Row.FindControl("lnkAudit"), ImageButton).Attributes.Add("vClientCode", e.Row.Cells(GVC_Code).Text)
                'CType(e.Row.FindControl("imgExport"), ImageButton).CommandArgument = e.Row.RowIndex
                'CType(e.Row.FindControl("imgExport"), ImageButton).CommandName = "ExportToExcel" + e.Row.Cells(GVC_Code).Text

                ''Added by ketan for tool tip
                'For DataRowIndex As Integer = 0 To e.Row.Cells.Count - 1
                '    strCellValue = e.Row.Cells(DataRowIndex).Text
                '    If strCellValue.Length > 40 Then
                '        e.Row.Cells(DataRowIndex).Attributes.Add("title", strCellValue)
                '        e.Row.Cells(DataRowIndex).Text = strCellValue.Substring(0, 20) + "..."
                '    End If
                'Next DataRowIndex

            End If
        End If
    End Sub

#End Region

#Region "Report Helper Function "
    Private Function OpenReport(ByVal ReportFileName_1 As String) As Boolean
        '' This Function open file on physical memory(In HardDist)          
        If File.Exists(ReportFileName_1) = True Then
            File.Delete(ReportFileName_1)
        End If
        rPage = New RepoPage(ReportFileName_1)
    End Function

    Private Sub ReportHeader()
        Dim rRow As RepoRow
        Dim rCell As RepoCell

        rRow = New RepoRow
        rCell = rRow.AddCell("CompanyTitle")
        rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
        rCell.FontBold = True
        rCell.FontSize = 14
        rCell.Value = System.Configuration.ConfigurationManager.AppSettings("Client")
        rCell.NoofCellContain = 10
        rCell.FontColor = Drawing.Color.Maroon
        rCell.BackgroundColor = Drawing.Color.White
        rPage.Say(rRow)

        rRow = New RepoRow
        rCell = rRow.AddCell("ReportTitle")
        rCell.Value = "Clients Detail"
        rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
        rCell.FontBold = True
        rCell.FontSize = 12
        rCell.FontColor = Drawing.Color.Maroon
        rCell.NoofCellContain = 10
        rPage.Say(rRow)


        rPage.SayBlankRow()

    End Sub

    Private Sub PrintHeader(ByVal WithClientContact As Boolean)
        Dim rRow As RepoRow
        Dim index As Integer = 0
        rRow = New RepoRow

        'rRow = masterRow()
        'rRow.Cell("Tree").Value = "Client Detail"
        If WithClientContact = False Then
            rRow = masterRow(False)
            rRow.Cell("Client").Value = "Client Detail"
            rRow.Cell("ProjectManager").Value = "Project Manager"
        ElseIf WithClientContact = True Then
            rRow = masterRow(True)
            rRow.Cell("Client").Value = "Client"
            rRow.Cell("Contact").Value = "Contact"
            'vAddress1,vAddress2,vAddress3,vDesignation,vTelephoneNo,vExtNo,vFaxNo,vEmailId,
            rRow.Cell("vAddress1").Value = "Address1"
            rRow.Cell("vAddress2").Value = "Address2"
            rRow.Cell("vAddress3").Value = "Address3"
            rRow.Cell("vDesignation").Value = "Designation"
            rRow.Cell("vTelephoneNo").Value = "TelephoneNo"
            rRow.Cell("vExtNo").Value = "ExtNo"
            rRow.Cell("vFaxNo").Value = "FaxNo"
            rRow.Cell("vEmailId").Value = "EmailId"
            rRow.Cell("ProjectManager").Value = "ProjectManager"
        End If


        For index = 0 To rRow.CellCount - 1
            rRow.Cell(0).FontBold = True
            rRow.Cell(0).Alignment = RepoCell.AlignmentEnum.CenterTop
        Next
        rPage.Say(rRow)


    End Sub

    Private Sub ReportDetail(ByVal dt_client As DataTable, ByVal WithClientContact As Boolean)

        Dim rRow As RepoRow
        Dim dt_Report As New DataTable
        Dim PreviousCompany As String = String.Empty
        Dim PreviousProject As String = String.Empty
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dv As New DataView
        Dim dr1 As DataRow
        Dim i As Integer = 0
        Dim PreviousClient As String = String.Empty

        Try
            If WithClientContact = False Then
                rRow = masterRow(False)
                PrintHeader(False)
                For Each dr As DataRow In dt_client.Rows
                    rRow.Cell("Client").Value = dr("vclientName").ToString
                    rRow.Cell("ProjectManager").Value = Convert.ToString(dr("ProjectMangerWithProfile"))
                    rPage.Say(rRow)
                    'rPage.Say(rRow)
                Next
            ElseIf WithClientContact = True Then
                'vClientName,ClientRemark,vContactName,vAddress1,vAddress2,vAddress3,vDesignation,vTelephoneNo,vExtNo,vFaxNo,vEmailId,
                rRow = masterRow(True)
                PrintHeader(True)
                For indexClient As Integer = 0 To dt_client.Rows.Count
                    dr1 = dt_client.Rows(indexClient)
                    If indexClient = 0 Then
                        PreviousClient = ""
                    ElseIf indexClient <> 0 Then
                        PreviousClient = dt_client.Rows(indexClient - 1).Item("vclientName")
                    End If

                    If PreviousClient.ToString.ToUpper = dr1("vclientname").ToString.ToUpper Then
                        rRow.Cell("Client").Value = ""
                    ElseIf PreviousClient.ToString.ToUpper <> dr1("vclientname").ToString.ToUpper Then
                        rRow.Cell("Client").Value = dr1("vclientName").ToString
                    End If
                    rRow.Cell("Contact").Value = dr1("vContactName").ToString


                    rRow.Cell("vAddress1").Value = dr1("vAddress1").ToString
                    rRow.Cell("vAddress2").Value = dr1("vAddress3").ToString
                    rRow.Cell("vDesignation").Value = dr1("vDesignation").ToString
                    rRow.Cell("vTelephoneNo").Value = dr1("vTelephoneNo").ToString
                    rRow.Cell("vExtNo").Value = dr1("vExtNo").ToString
                    rRow.Cell("vFaxNo").Value = dr1("vFaxNo").ToString
                    rRow.Cell("vEmailId").Value = dr1("vEmailId").ToString
                    rRow.Cell("ProjectManager").Value = dr1("ProjectManager").ToString
                    rPage.Say(rRow)

                Next

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message + "Error While Print Detail", "")
        End Try
    End Sub

    Private Function masterRow(ByVal WithClientContact As Boolean) As RepoRow
        Dim rRow As RepoRow
        Dim rCell As RepoCell
        Dim i As Integer

        rRow = New RepoRow

        rCell = New RepoCell("Client")
        rRow.AddCell(rCell)

       


        If (WithClientContact = True) Then
            rCell = New RepoCell("Contact")
            rRow.AddCell(rCell)
            'vAddress1,vAddress2,vAddress3,vDesignation,vTelephoneNo,vExtNo,vFaxNo,vEmailId,
            rCell = New RepoCell("vAddress1")
            rRow.AddCell(rCell)

            rCell = New RepoCell("vAddress2")
            rRow.AddCell(rCell)

            rCell = New RepoCell("vAddress3")
            rRow.AddCell(rCell)

            rCell = New RepoCell("vDesignation")
            rRow.AddCell(rCell)

            rCell = New RepoCell("vTelephoneNo")
            rRow.AddCell(rCell)

            rCell = New RepoCell("vExtNo")
            rRow.AddCell(rCell)

            rCell = New RepoCell("vFaxNo")
            rRow.AddCell(rCell)

            rCell = New RepoCell("vEmailId")
            rRow.AddCell(rCell)

            'rCell = New RepoCell("ProjectManager")
            'rRow.AddCell(rCell)

           
        End If
        rCell = New RepoCell("ProjectManager")
        rRow.AddCell(rCell)
        For i = 0 To rRow.CellCount - 1
            rRow.Cell(i).FontSize = 11
            rRow.Cell(i).LineBottom.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(i).LineTop.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(i).LineRight.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(i).LineLeft.LineType = RepoLine.LineTypeEnum.SingleLine
        Next i
        Return rRow

    End Function
#End Region

#Region "Error Handler"
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objcommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objcommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub
#End Region

#Region "Web Method"

    <WebMethod> _
    Public Shared Function AuditTrail(ByVal vclientcode As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_Clientmst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtClientMstHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Try


            wStr = " vClientCode = '" + vclientcode + "' Order by nClientMstHistoryNo DESC"
            If Not objHelp.GetView_clientmstHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Clientmst, estr) Then
                Throw New Exception(estr)
            End If

            If Not dtClientMstHistory Is Nothing Then
                dtClientMstHistory.Columns.Add("SrNo")
                dtClientMstHistory.Columns.Add("ClientName")
                dtClientMstHistory.Columns.Add("ProjectManager")
                dtClientMstHistory.Columns.Add("Remarks")
                dtClientMstHistory.Columns.Add("ModifyBy")
                dtClientMstHistory.Columns.Add("ModifyOn")
            End If
            dt = ds_Clientmst.Tables(0)
            Dim dv As New DataView(dt)

            For Each dr As DataRow In dv.ToTable.Rows

                drAuditTrail = dtClientMstHistory.NewRow()

                drAuditTrail("SrNo") = i
                drAuditTrail("ClientName") = dr("vClientName").ToString()
                drAuditTrail("ProjectManager") = dr("ProjectManager").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("dModifyOn"))
                dtClientMstHistory.Rows.Add(drAuditTrail)
                dtClientMstHistory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtClientMstHistory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

#End Region

End Class
