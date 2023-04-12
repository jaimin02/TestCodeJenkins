Imports Microsoft.Win32
Imports System.Web.Services
Imports Newtonsoft.Json
Imports Microsoft
Imports Microsoft.Office.Interop
Imports System.IO
Imports System.Drawing

Partial Class frmRegionMst
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATIONS"

    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtRegionMst As String = "DtRegionMst"
    Private Const VS_RegionCode As String = "vRegionCode"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_RegionName As Integer = 1
    Private Const GVC_ModifyOn As Integer = 2
    Private Const GVC_Edit As Integer = 3
    Private Const GVC_Code As Integer = 4

#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GenCall()
            Exit Sub
        End If
        If gvRegion.Rows.Count > 0 Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvRegion", "UIgvRegion(); ", True)
        End If
    End Sub

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Dim dt_RegionMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try



            Choice = Me.Request.QueryString("mode").ToString
            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_RegionCode) = Me.Request.QueryString("Value").ToString
            End If

            ''''Check for Valid User''''''''''''''
            If Not GenCall_Data(Choice, dt_RegionMst) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtRegionMst) = dt_RegionMst ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI(Choice, dt_RegionMst) Then 'For Displaying Data 
                Exit Function
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideRegionDetails", "HideRegionDetails(); ", True)
            End If
            GenCall = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...GenCall")
            Return False
        End Try
    End Function

#End Region

#Region "GenCall_Data"

    Private Function GenCall_Data(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                            ByRef dt_Dist_Retu As DataTable) As Boolean

        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_RegionMst As DataSet = Nothing
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Try



            If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vRegionCode=" + Me.ViewState(VS_RegionCode).ToString() 'Value of where condition
            End If
            wStr += " And cStatusIndi <> 'D'"
            If Not objHelp.getregionmst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_RegionMst, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_RegionMst Is Nothing Then
                Throw New Exception(eStr)
            End If
            If ds_RegionMst.Tables(0).Rows.Count <= 0 And _
               Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Throw New Exception("No Records Found for Selected role")

            End If
            dt_Dist_Retu = ds_RegionMst.Tables(0)
            GenCall_Data = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...Gencall_Data")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_ShowUI"
    Private Function GenCall_ShowUI(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                                      ByVal dt_Dist As DataTable) As Boolean
        Dim dt_RegionMst As DataTable = Nothing
        Try
            Page.Title = ":: Region Master :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Master.FindControl("lblHeading"), Label).Text = "Region/Country Master"
            If Not FillRegionMst() Then
                Return False
            End If
            dt_RegionMst = Me.ViewState(VS_DtRegionMst)
            If Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.txtRegionName.Text = ConvertDbNullToDbTypeDefaultValue(dt_RegionMst.Rows(0)("vRegionName"), dt_RegionMst.Rows(0)("vRegionName").GetType)
                Me.btnSave.Text = "Update"
                Me.btnSave.ToolTip = "Update"
            End If
            GenCall_ShowUI = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...Gencall_ShowUI")
            Return False
        End Try
    End Function
#End Region

#Region "Fill GridRegionMst"
    Private Function FillRegionMst() As Boolean
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_RegionMst As New DataSet
        Try

            wStr = "cStatusIndi <> 'D'"
            If Not objHelp.getregionmst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_RegionMst, eStr) Then
                Throw New Exception(eStr)
            End If
            ds_RegionMst.Tables(0).DefaultView.Sort = "vRegionName"
            Me.gvRegion.DataSource = ds_RegionMst.Tables(0).DefaultView.ToTable()
            Me.gvRegion.DataBind()
            If gvRegion.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvRegion", "UIgvRegion(); ", True)
            End If
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...FillGridRegionMst")
            Return False
        End Try
    End Function
#End Region

#Region "Reset Page"
    Private Sub ResetPage()
        Me.txtRegionName.Text = ""
        Me.txtRemark.Text = ""
        Me.ViewState(VS_Choice) = Nothing
        Me.ViewState(VS_DtRegionMst) = Nothing
        Me.ViewState(VS_RegionCode) = Nothing
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

            dtOld = Me.ViewState(VS_DtRegionMst)

            'For validating Duplication
            wStr = "cStatusIndi <> 'D' And vRegionName='" & Me.txtRegionName.Text.Trim & "'"

            If Me.Request.QueryString("mode") = 2 Then
                wStr += " And vRegionCode <> '" + Me.Request.QueryString("Value").ToString + "'"
            End If

            If Not objHelp.getregionmst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Check, eStr) Then
                Me.ShowErrorMessage("Error While Getting Data from RegionMst", eStr)
                Return False
            End If

            If ds_Check.Tables(0).Rows.Count > 0 Then
                objcommon.ShowAlert("Region Name Already Exists !", Me.Page)
                Return False
            End If
            '***********************************

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                dr = dtOld.NewRow
                dr("vRegionName") = Me.txtRegionName.Text.Trim
                dr("vRegionCode") = "0000"
                dr("iModifyBy") = Session(S_UserID)
                dr("vRemark") = txtRemark.Text.Trim()
                dtOld.Rows.Add(dr)
            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                For Each dr In dtOld.Rows
                    dr("vRegionName") = Me.txtRegionName.Text.Trim
                    dr("iModifyBy") = Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr("vRemark") = txtRemark.Text.Trim()
                    dr.AcceptChanges()
                Next dr
                dtOld.AcceptChanges()
            End If

            Me.ViewState(VS_DtRegionMst) = dtOld
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...AssignUpdateValues")
            Return False
        End Try
    End Function

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim Dt As New DataTable
        Dim ds_regionmst As DataSet
        Dim Ds_Regionmstgrid As New DataSet
        Dim eStr As String = String.Empty
        Dim message As String = String.Empty
        Try


            If Not AssignUpdatedValues() Then
                Exit Sub
            End If

            ds_regionmst = New DataSet
            ds_regionmst.Tables.Add(CType(Me.ViewState(VS_DtRegionMst), Data.DataTable).Copy())
            ds_regionmst.Tables(0).TableName = "regionmst"   ' New Values on the form to be updated

            If Not objLambda.Save_Insertregionmst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_regionmst, ds_regionmst, "1", eStr) Then
                objcommon.ShowAlert("Error While Saving RegionMst", Me.Page)
                Exit Sub
            End If

            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "Region Saved Successfully !", "Region  Updated Successfully !")
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('" + message + "')", True)
            ResetPage()
        Catch ex As Exception
            ShowErrorMessage(ex.Message, " ...btnSave_Click")
        End Try
    End Sub

#End Region

#Region "Cancel & Close"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Response.Redirect("frmRegionMst.aspx?mode=1")
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("frmMainPage.aspx")
    End Sub
#End Region

#Region "BindGrid"

    Private Sub BindGrid()
        Dim dsRegion As New DataSet
        Dim errStr As String = String.Empty

        If objHelp.getregionmst("cStatusIndi <> 'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsRegion, errStr) Then
            gvRegion.ShowFooter = True
            gvRegion.DataSource = dsRegion
            gvRegion.DataBind()
            gvRegion.Dispose()
        End If

    End Sub

#End Region

#Region "Grid Event"

    Protected Sub gvRegion_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        gvRegion.PageIndex = e.NewPageIndex
        FillRegionMst()
    End Sub

    Protected Sub gvRegion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        If Not e.Row.RowType = DataControlRowType.Pager Then
            e.Row.Cells(GVC_Code).Visible = False

            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (gvRegion.PageSize * gvRegion.PageIndex) + 1
                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "Edit"
                CType(e.Row.FindControl("lnkAudit"), ImageButton).Attributes.Add("vRegionCode", e.Row.Cells(GVC_Code).Text)
            End If
        End If
    End Sub

    Protected Sub gvRegion_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim index As Integer = e.CommandArgument

        If e.CommandName.ToUpper = "EDIT" Then
            Me.Response.Redirect("frmRegionMst.aspx?mode=2&value=" & Me.gvRegion.Rows(index).Cells(GVC_Code).Text.Trim())
        End If

    End Sub
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
    Public Shared Function AuditTrail(ByVal vRegionCode As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_RegionMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim dtRegionMstHistory As New DataTable
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

            vTableName = "RegionMstHistory"
            vIdName = ""
            AuditFieldName = "vRegionCode"
            AuditFieldValue = vRegionCode
            vOtherTableName = ""
            vOtherIdName = ""


            Dim Param As String = ""


            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName

            If Not objHelp.Proc_GetAuditTrail(wStr, ds_RegionMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtRegionMstHistory Is Nothing Then
                dtRegionMstHistory.Columns.Add("SrNo")
                dtRegionMstHistory.Columns.Add("RegionName")
                dtRegionMstHistory.Columns.Add("Remark")
                dtRegionMstHistory.Columns.Add("ModifyBy")
                dtRegionMstHistory.Columns.Add("ModifyOn")
            End If

            dt = ds_RegionMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtRegionMstHistory.NewRow()
                drAuditTrail("SrNo") = i
                drAuditTrail("RegionName") = dr("vRegionName").ToString()
                drAuditTrail("Remark") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtRegionMstHistory.Rows.Add(drAuditTrail)
                dtRegionMstHistory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtRegionMstHistory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Protected Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Dim wStr As String = String.Empty
        Dim ds_RegionMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtRegionMstHistory As New DataTable
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

            vTableName = "RegionMstHistory"
            vIdName = ""
            AuditFieldName = "vRegionCode"
            AuditFieldValue = hdnRegionCode.Value
            vOtherTableName = ""
            vOtherIdName = ""


            Dim Param As String = ""

            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName

            If Not objHelp.Proc_GetAuditTrail(wStr, ds_RegionMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtRegionMstHistory Is Nothing Then
                dtRegionMstHistory.Columns.Add("Sr. No")
                dtRegionMstHistory.Columns.Add("Region Name")
                dtRegionMstHistory.Columns.Add("Remarks")
                dtRegionMstHistory.Columns.Add("ModifyBy")
                dtRegionMstHistory.Columns.Add("ModifyOn")
            End If

            dtRegionMstHistory.AcceptChanges()
            dt = ds_RegionMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtRegionMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Region Name") = dr("vRegionName").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtRegionMstHistory.Rows.Add(drAuditTrail)
                dtRegionMstHistory.AcceptChanges()
                i += 1
            Next

            gvExport.DataSource = dtRegionMstHistory
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

                filename = "Audit Trail_" + hdnRegionCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                Dim stringWriter As New System.IO.StringWriter()

                Dim writer As New HtmlTextWriter(stringWriter)
                gvExport.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""5""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")





                strMessage.Append("<td colspan=""5""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Region Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")


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

                filename = "Audit Trail_" + hdnRegionCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"


                drAuditTrail = dtRegionMstHistory.NewRow()

                drAuditTrail("Sr. No") = ""
                drAuditTrail("Region Name") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("ModifyBy") = ""
                drAuditTrail("ModifyOn") = ""
                dtRegionMstHistory.Rows.Add(drAuditTrail)
                dtRegionMstHistory.AcceptChanges()
                gvExport.DataSource = dtRegionMstHistory
                gvExport.DataBind()


                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                gvExport.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()


                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""5""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""5""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Region Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td align=""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")


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

    Protected Sub btnExportToExcelGrid_Click(sender As Object, e As EventArgs) Handles btnExportToExcelGrid.Click
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Dim wStr As String = String.Empty
        Dim ds_RegionMst As New DataSet
        Dim estr As String = String.Empty
        Dim dtRegionMstHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Dim RegionCode As String = ""
        Dim filename As String = String.Empty
        Dim strMessage As New StringBuilder

        Try
            wStr = "" + "##"
            ds_RegionMst = objHelp.ProcedureExecute("Proc_RegionMst ", wStr)
            wStr = String.Empty

            If Not dtRegionMstHistory Is Nothing Then
                dtRegionMstHistory.Columns.Add("Sr. No")
                dtRegionMstHistory.Columns.Add("Region Name")
                dtRegionMstHistory.Columns.Add("Remarks")
                dtRegionMstHistory.Columns.Add("ModifyBy")
                dtRegionMstHistory.Columns.Add("ModifyOn")
            End If

            dtRegionMstHistory.AcceptChanges()
            dt = ds_RegionMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtRegionMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Region Name") = dr("vRegionName").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("iModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("dModifyOn"))
                dtRegionMstHistory.Rows.Add(drAuditTrail)
                dtRegionMstHistory.AcceptChanges()
                i += 1
            Next

            gvExportToExcel.DataSource = dtRegionMstHistory
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

                filename = "Region Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                Dim stringWriter As New System.IO.StringWriter()

                Dim writer As New HtmlTextWriter(stringWriter)
                gvExportToExcel.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""5""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""5""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Region Master")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")


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

                filename = "Region Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"


                drAuditTrail = dtRegionMstHistory.NewRow()

                drAuditTrail("Sr. No") = i
                drAuditTrail("Region Name") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("ModifyBy") = ""
                drAuditTrail("ModifyOn") = ""
                dtRegionMstHistory.Rows.Add(drAuditTrail)
                dtRegionMstHistory.AcceptChanges()
                gvExportToExcel.DataSource = dtRegionMstHistory
                gvExportToExcel.DataBind()


                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                gvExportToExcel.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()


                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""5""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""5""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Region Master")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td align=""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")


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
End Class
