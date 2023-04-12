Imports Microsoft.Win32
Imports System.Web.Services
Imports Newtonsoft.Json
Imports Microsoft
Imports Microsoft.Office.Interop
Imports System.IO
Imports System.Drawing


Partial Class frmSubjectLanguageMst
    Inherits System.Web.UI.Page

#Region "Variable Declaration "

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtSubjectLanguageMst As String = "DtSubjectLanguageMst"
    Private Const VS_LanguageId As String = "LanguageId"

    Private Const GVC_LanguageId As Integer = 1
    Private Const GVC_LanguageName As Integer = 2
    Private Const GVC_Active As Integer = 3
    Private Const GVC_Edit As Integer = 4


#End Region

#Region "Page Load "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GenCall()
        End If
        If gvwSubjectLanguageMst.Rows.Count > 0 Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvwSubjectLanguageMst", "UIgvwSubjectLanguageMst(); ", True)
        End If
    End Sub

#End Region

#Region "GenCall "
    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try


            Choice = Me.Request.QueryString("Mode")
            Me.ViewState(VS_Choice) = Choice   'To use it while saving
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_LanguageId) = Me.Request.QueryString("Value").ToString
            End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtSubjectLanguageMst) = ds.Tables("SubjectLanguageMst")   ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideSponsorDetails", "HideSponsorDetails(); ", True)
            End If
            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "........GenCall")

        Finally

        End Try

    End Function
#End Region

#Region "GenCall_Data "

    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean

        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Val As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing

        Try



            Val = Me.ViewState(VS_LanguageId) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vLanguageId=" + Val.ToString
            End If


            If Not objHelp.getSubjectLanguageMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            If ds.Tables(0).Rows.Count <= 0 And _
             Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found for Selected Operation")
            End If

            ds_DWR_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..........GenCallData")

        Finally

        End Try
    End Function

#End Region

#Region "GenCall_ShowUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim dt_SubjectLanguageMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try

            Page.Title = ":: Language Master   :: " + System.Configuration.ConfigurationManager.AppSettings("Client")


            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Language Master"

            dt_SubjectLanguageMst = Me.ViewState(VS_DtSubjectLanguageMst)

            If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.txtLanguage.Text = dt_SubjectLanguageMst.Rows(0).Item("vLanguageName")
                Me.ChkActive.Checked = (dt_SubjectLanguageMst.Rows(0).Item("cActiveFlag").ToString.ToUpper.Trim() = "Y")
                btnSave.Text = "Update"
                btnSave.ToolTip = "Update"
            End If

            'btnSave.Attributes.Add("onclick", "return Validation();")

            If Not FillGrid() Then
                Exit Function
            End If
            GenCall_ShowUI = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCal_ShowUI")
        Finally
        End Try
    End Function

#End Region

#Region "FillGrid"

    Private Function FillGrid() As Boolean
        Dim ds_SubjectLanguageMst As New Data.DataSet
        Dim dv_SubjectLanguageMst As New DataView
        Dim estr As String = String.Empty
        Try



            If Not objHelp.getSubjectLanguageMst("cStatusIndi <> 'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_SubjectLanguageMst, estr) Then
                Return False
            End If
            dv_SubjectLanguageMst = ds_SubjectLanguageMst.Tables(0).Copy.DefaultView()
            dv_SubjectLanguageMst.Sort = "vLanguageName"

            Me.gvwSubjectLanguageMst.DataSource = dv_SubjectLanguageMst
            Me.gvwSubjectLanguageMst.DataBind()
            If gvwSubjectLanguageMst.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvwSubjectLanguageMst", "UIgvwSubjectLanguageMst(); ", True)
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "........FillGrid")
            Return False
        End Try
    End Function
#End Region

#Region "Button Click"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim ds_SubjectLanguageMst As New DataSet
        Dim dt_SubjectLanguageMst As New DataTable
        Dim estr As String = String.Empty
        Dim message As String = String.Empty
        Try
            If Not AssignValues() Then
                Exit Sub
            End If

            ds_SubjectLanguageMst = New DataSet
            dt_SubjectLanguageMst = CType(Me.ViewState(VS_DtSubjectLanguageMst), DataTable)
            dt_SubjectLanguageMst.TableName = "SubjectLanguageMst"
            ds_SubjectLanguageMst.Tables.Add(dt_SubjectLanguageMst)

            If Not objLambda.Save_SubjectLanguageMst(Me.ViewState(VS_Choice), ds_SubjectLanguageMst, Me.Session(S_UserID), estr) Then

                ObjCommon.ShowAlert("Error While Saving SubjectLanguageMst", Me)
                Exit Sub
            End If
            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "Subject Language Details Saved Successfully !", "Subject Language Details Updated Successfully !")
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('" + message + "')", True)
            ResetPage()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnSave_Click")
        Finally
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ResetPage()
        Response.Redirect("frmSubjectLanguageMst.aspx?mode=1")
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("frmMainPage.aspx")
    End Sub
#End Region

#Region "Assign Values "

    Private Function AssignValues() As Boolean
        Dim dr As DataRow
        Dim dt_Language As New DataTable
        Dim ds_Check As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Try


            dt_Language = CType(Me.ViewState(VS_DtSubjectLanguageMst), DataTable)
            'for Validation of duplicate data 
            wStr = "cStatusIndi <> 'D' And vLanguageName='" & Me.txtLanguage.Text.Trim() & "'"
            If Me.Request.QueryString("mode") = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                wStr += " And vLanguageId <> '" + Me.Request.QueryString("Value").ToString + "'"
            End If

            If Not objHelp.getSubjectLanguageMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Check, eStr) Then
                Me.ShowErrorMessage("Error While Getting Data From DeptMst", eStr)
                Return False
            End If

            If ds_Check.Tables(0).Rows.Count > 0 Then
                ObjCommon.ShowAlert(" Subject Language Already Exists !", Me.Page)
                Return False
            End If
            '**************************
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                dt_Language.Clear()
                dr = dt_Language.NewRow()
                dr("vLanguageId") = "0000"
                dr("vLanguageName") = Me.txtLanguage.Text.Trim
                dr("vRemark") = Me.txtRemark.Text.Trim
                dr("cActiveFlag") = IIf(Me.ChkActive.Checked, "Y", "N")
                dr("iModifyBy") = Me.Session(S_UserID)
                dt_Language.Rows.Add(dr)

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                For Each dr In dt_Language.Rows
                    dr("vLanguageName") = Me.txtLanguage.Text.Trim
                    dr("vRemark") = Me.txtRemark.Text.Trim
                    dr("cActiveFlag") = IIf(Me.ChkActive.Checked, "Y", "N")
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr.AcceptChanges()

                Next dr
                dt_Language.AcceptChanges()
            End If
            Me.ViewState(VS_DtSubjectLanguageMst) = dt_Language
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignValues")
            Return False
        End Try

    End Function

#End Region

#Region "ResetPage"

    Private Sub ResetPage()

        Me.txtLanguage.Text = ""
        Me.ViewState(VS_DtSubjectLanguageMst) = Nothing
        Me.ViewState(VS_LanguageId) = Nothing
        Me.ViewState(VS_Choice) = Nothing

    End Sub

#End Region

#Region "Grid Events "

    Protected Sub gvwSubjectLanguageMst_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then

            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "Edit"
            CType(e.Row.FindControl("lnkAudit"), ImageButton).Attributes.Add("vLanguageId", e.Row.Cells(GVC_LanguageId).Text)
            e.Row.Cells(GVC_Active).Text = IIf(e.Row.Cells(GVC_Active).Text = "Y", "Yes", "No")
            i = (gvwSubjectLanguageMst.PageIndex * 10)
            e.Row.Cells(0).Text = i + e.Row.RowIndex + 1

        End If

    End Sub

    Protected Sub gvwSubjectLanguageMst_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or _
        e.Row.RowType = DataControlRowType.Header Or _
        e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(1).Visible = False
        End If
    End Sub

    Protected Sub gvwSubjectLanguageMst_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        gvwSubjectLanguageMst.PageIndex = e.NewPageIndex
        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub

    Protected Sub gvwSubjectLanguageMst_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim index As Integer = e.CommandArgument

        If e.CommandName.ToUpper = "EDIT" Then
            Me.Response.Redirect("frmSubjectLanguageMst.aspx?mode=2&value=" & Me.gvwSubjectLanguageMst.Rows(index).Cells(GVC_LanguageId).Text.Trim())
        End If

    End Sub

#End Region

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        ObjCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        ObjCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub
#End Region

#Region "Web Method"

    <WebMethod> _
    Public Shared Function AuditTrail(ByVal vLanguageId As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_SubjectLanguageMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim dtSubjectLanguageMstHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable

        Dim vTableName As String = String.Empty
        Dim vIdName As String = String.Empty
        Dim AuditFieldName As String = String.Empty
        Dim AuditFieldValue As String = String.Empty
        Dim vOtherTableName As String = String.Empty
        Dim vOtherIdName As String = String.Empty

        Try

            vTableName = "SubjectLanguageMstHistory"
            vIdName = ""
            AuditFieldName = "vLanguageId"
            AuditFieldValue = vLanguageId
            vOtherTableName = ""
            vOtherIdName = ""


            Dim Param As String = ""


            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName

            If Not objHelp.Proc_GetAuditTrail(wStr, ds_SubjectLanguageMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtSubjectLanguageMstHistory Is Nothing Then
                dtSubjectLanguageMstHistory.Columns.Add("SrNo")
                dtSubjectLanguageMstHistory.Columns.Add("LanguageName")
                dtSubjectLanguageMstHistory.Columns.Add("ActiveFlag")
                dtSubjectLanguageMstHistory.Columns.Add("Remark")
                dtSubjectLanguageMstHistory.Columns.Add("ModifyBy")
                dtSubjectLanguageMstHistory.Columns.Add("ModifyOn")
            End If

            dt = ds_SubjectLanguageMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtSubjectLanguageMstHistory.NewRow()
                drAuditTrail("SrNo") = i
                drAuditTrail("LanguageName") = dr("vLanguageName").ToString()
                If dr("cActiveFlag").ToString() = "Y" Then
                    drAuditTrail("ActiveFlag") = "Yes"
                ElseIf dr("cActiveFlag").ToString() = "N" Then
                    drAuditTrail("ActiveFlag") = "No"
                End If
                'drAuditTrail("ActiveFlag") = dr("cActiveFlag").ToString()
                drAuditTrail("Remark") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtSubjectLanguageMstHistory.Rows.Add(drAuditTrail)
                dtSubjectLanguageMstHistory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtSubjectLanguageMstHistory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Protected Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_SubjectLanguageMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtSubjectLanguageMstHistory As New DataTable
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

            vTableName = "SubjectLanguageMstHistory"
            vIdName = ""
            AuditFieldName = "vLanguageId"
            AuditFieldValue = hdnLanguageId.Value
            vOtherTableName = ""
            vOtherIdName = ""


            Dim Param As String = ""

            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName

            If Not objHelp.Proc_GetAuditTrail(wStr, ds_SubjectLanguageMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtSubjectLanguageMstHistory Is Nothing Then
                dtSubjectLanguageMstHistory.Columns.Add("Sr. No")
                dtSubjectLanguageMstHistory.Columns.Add("Language Name")
                dtSubjectLanguageMstHistory.Columns.Add("Active")
                dtSubjectLanguageMstHistory.Columns.Add("Remarks")
                dtSubjectLanguageMstHistory.Columns.Add("Modify By")
                dtSubjectLanguageMstHistory.Columns.Add("Modify On")
            End If

            dtSubjectLanguageMstHistory.AcceptChanges()
            dt = ds_SubjectLanguageMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtSubjectLanguageMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Language Name") = dr("vLanguageName").ToString()
                If dr("cActiveFlag").ToString() = "Y" Then
                    drAuditTrail("Active") = "Yes"
                ElseIf dr("cActiveFlag").ToString() = "N" Then
                    drAuditTrail("Active") = "No"
                End If
                'drAuditTrail("Active Flag") = dr("cActiveFlag").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("Modify By") = dr("vModifyBy").ToString()
                drAuditTrail("Modify On") = Convert.ToString(dr("ModifyOn"))
                dtSubjectLanguageMstHistory.Rows.Add(drAuditTrail)
                dtSubjectLanguageMstHistory.AcceptChanges()
                i += 1
            Next

            gvExport.DataSource = dtSubjectLanguageMstHistory
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

                filename = "Audit Trail_" + hdnLanguageId.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

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
                strMessage.Append("Subject Language Master-AuditTrail")
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

                filename = "Audit Trail_" + hdnLanguageId.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"


                drAuditTrail = dtSubjectLanguageMstHistory.NewRow()

                drAuditTrail("Sr. No") = i
                drAuditTrail("Language Name") = ""
                drAuditTrail("Active") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("Modify By") = ""
                drAuditTrail("Modify On") = ""
                dtSubjectLanguageMstHistory.Rows.Add(drAuditTrail)
                dtSubjectLanguageMstHistory.AcceptChanges()
                gvExport.DataSource = dtSubjectLanguageMstHistory
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
                strMessage.Append("Subject Language Master-AuditTrail")
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
            Me.ObjCommon.ShowAlert("Error While Exporting Data To Excel : " + estr, Me.Page)

            Exit Sub
        End Try
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub

#End Region

    Protected Sub btnExportToExcelGrid_Click(sender As Object, e As EventArgs) Handles btnExportToExcelGrid.Click

        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_SubjectLanguageMst As New DataSet
        Dim estr As String = String.Empty
        Dim dtSubjectLanguageMstHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Dim vLanguageId As String = String.Empty
        Dim filename As String = String.Empty
        Dim strMessage As New StringBuilder

        Try
            ds_SubjectLanguageMst = objHelp.ProcedureExecute("Proc_SubjectLanguageMst ", wStr)
            If Not dtSubjectLanguageMstHistory Is Nothing Then
                dtSubjectLanguageMstHistory.Columns.Add("Sr. No")
                dtSubjectLanguageMstHistory.Columns.Add("Language Name")
                dtSubjectLanguageMstHistory.Columns.Add("Active")
                dtSubjectLanguageMstHistory.Columns.Add("Remarks")
                dtSubjectLanguageMstHistory.Columns.Add("Modify By")
                dtSubjectLanguageMstHistory.Columns.Add("Modify On")
            End If

            dtSubjectLanguageMstHistory.AcceptChanges()
            dt = ds_SubjectLanguageMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtSubjectLanguageMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Language Name") = dr("vLanguageName").ToString()
                If dr("cActiveFlag").ToString() = "Y" Then
                    drAuditTrail("Active") = "Yes"
                ElseIf dr("cActiveFlag").ToString() = "N" Then
                    drAuditTrail("Active") = "No"
                End If
                'drAuditTrail("Active Flag") = dr("cActiveFlag").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("Modify By") = dr("iModifyBy").ToString()
                drAuditTrail("Modify On") = Convert.ToString(dr("dModifyOn"))
                dtSubjectLanguageMstHistory.Rows.Add(drAuditTrail)
                dtSubjectLanguageMstHistory.AcceptChanges()
                i += 1
            Next

            gvExportToExcel.DataSource = dtSubjectLanguageMstHistory
            gvExportToExcel.DataBind()
            If gvExportToExcel.Rows.Count > 0 Then

                gvExportToExcel.HeaderRow.BackColor = Color.White
                gvExportToExcel.FooterRow.BackColor = Color.White
                For Each cell As TableCell In gvExportToExcel.HeaderRow.Cells
                    cell.BackColor = gvExportToExcel.HeaderStyle.BackColor
                Next
                For Each row As GridViewRow In gvExportToExcel.Rows
                    row.BackColor = Color.White
                    row.Height = 20
                    For Each cell As TableCell In row.Cells
                        If row.RowIndex Mod 2 = 0 Then
                            cell.BackColor = gvExportToExcel.AlternatingRowStyle.BackColor
                            cell.ForeColor = gvExportToExcel.RowStyle.ForeColor
                        Else
                            cell.BackColor = gvExportToExcel.RowStyle.BackColor
                            cell.ForeColor = gvExportToExcel.RowStyle.ForeColor
                        End If
                        cell.CssClass = "textmode"
                    Next
                Next

                'style to format numbers to string
                Dim style As String = "<style> .textmode { } </style>"
                Response.Write(style)



                Dim info As String = String.Empty
                Dim gridviewHtml As String = String.Empty

                filename = "SubjectLanguage Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                Dim stringWriter As New System.IO.StringWriter()

                Dim writer As New HtmlTextWriter(stringWriter)
                gvExportToExcel.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Subject Language Master")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")


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

                filename = "SubjectLanguage Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                drAuditTrail = dtSubjectLanguageMstHistory.NewRow()

                drAuditTrail("Sr. No") = i
                drAuditTrail("Language Name") = ""
                drAuditTrail("Active") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("Modify By") = ""
                drAuditTrail("Modify On") = ""
                dtSubjectLanguageMstHistory.Rows.Add(drAuditTrail)
                dtSubjectLanguageMstHistory.AcceptChanges()
                gvExportToExcel.DataSource = dtSubjectLanguageMstHistory
                gvExportToExcel.DataBind()

                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                gvExportToExcel.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Subject Language Master")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td align=""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")

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
            Me.ObjCommon.ShowAlert("Error While Exporting Data To Excel : " + estr, Me.Page)

            Exit Sub
        End Try
    End Sub
End Class
