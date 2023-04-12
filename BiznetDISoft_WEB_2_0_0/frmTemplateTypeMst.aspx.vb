Imports Microsoft.Win32
Imports System.Web.Services
Imports Newtonsoft.Json
Imports Microsoft
Imports Microsoft.Office.Interop
Imports System.IO
Imports System.Drawing

Partial Class frmTemplateTypeMst
    Inherits System.Web.UI.Page

#Region "Variable Declaration "

    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_dtTemplateTypeMst As String = "dtTemplateTypeMst"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_TemplateTypeCode As Integer = 1
    Private Const GVC_TemplateTypeName As Integer = 2
    Private Const GVC_Remarks As Integer = 3
    Private Const GVC_ModifyOn As Integer = 4

#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GenCall()
            Exit Sub
        End If
    End Sub
#End Region

#Region "GenCall "

    Private Function GenCall() As Boolean
        Dim dt_TemplateTypeMst As DataTable = Nothing

        Try


            Me.ViewState(VS_Choice) = Me.Request.QueryString("Mode")
            If Not GenCall_Data() Then ' For Data Retrieval
                Exit Function
            End If
            If Not GenCall_ShowUI() Then 'For Displaying Data 
                Exit Function
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideTempTypeDetails", "HideTempTypeDetails(); ", True)
            End If
            GenCall = True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....GenCall")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_Data"


    Private Function GenCall_Data() As Boolean

        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_TemplateTypeMst As DataSet = Nothing
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Try


            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vTemplateTypeCode=" + Me.Request.QueryString("Value").ToString() 'Value of where condition
            End If
            wStr += " And cStatusIndi <> 'D'"

            If Not objHelp.getTemplateTypeMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_TemplateTypeMst, eStr) Then

                Throw New Exception(eStr)
            End If

            If ds_TemplateTypeMst Is Nothing Then
                Throw New Exception(eStr)
            End If

            If ds_TemplateTypeMst.Tables(0).Rows.Count <= 0 And _
               Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Throw New Exception("No Records Found For Selected Role")

            End If
           
            Me.ViewState(VS_dtTemplateTypeMst) = ds_TemplateTypeMst.Tables(0)
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......GenCall_Data")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_ShowUI"

    Private Function GenCall_ShowUI() As Boolean

        Dim dt_TemplateTypeMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Try

            Page.Title = ":: Template Type Master  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")




            CType(Master.FindControl("lblHeading"), Label).Text = "Template Type Master"

            dt_TemplateTypeMst = Me.ViewState(VS_dtTemplateTypeMst)

            Choice = Me.ViewState("Choice")

            BindGrid()


            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.txtTemplateTypeName.Text = ConvertDbNullToDbTypeDefaultValue(dt_TemplateTypeMst.Rows(0)("vTemplateTypeName"), dt_TemplateTypeMst.Rows(0)("vTemplateTypeName").GetType)
                Me.txtRemark.Text = ""
                Me.btnSave.Text = "Update"
                Me.btnSave.ToolTip = "UPdate"
            End If



            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....GenCallShowUI")
        End Try
    End Function

#End Region

#Region "BindGrid"

    Private Sub BindGrid()
        Dim Ds_templatetypemst As New DataSet
        Dim eStr As String = String.Empty
        Try



            If objHelp.getTemplateTypeMst("cStatusIndi <> 'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        Ds_templatetypemst, eStr) Then

                gvtemplatetypemst.ShowFooter = False
                Ds_templatetypemst.Tables(0).DefaultView.Sort = "vTemplateTypeName"
                gvtemplatetypemst.DataSource = Ds_templatetypemst.Tables(0).DefaultView.ToTable
                gvtemplatetypemst.DataBind()
                Ds_templatetypemst.Dispose()

            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, " .....BindGrid")
        End Try
    End Sub

#End Region

#Region "Reset Page"

    Private Sub ResetPage()
        Me.txtTemplateTypeName.Text = ""
        Me.txtRemark.Text = ""
        Me.ViewState(VS_dtTemplateTypeMst) = Nothing
    End Sub

#End Region

#Region "Save"

    Private Function AssignUpdatedValues() As Boolean
        Dim dtOld As New DataTable
        Dim dr As DataRow
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_Check As New DataSet
        Try


            dtOld = Me.ViewState(VS_dtTemplateTypeMst)

            wStr = "cStatusIndi <> 'D'"
            If Not objHelp.getTemplateTypeMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Check, eStr) Then

                objcommon.ShowAlert("Error While Getting Data From TemplateTypeMaster", Me.Page)
            End If

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                For Each dr In ds_Check.Tables(0).Rows
                    If dr("vTemplateTypeName").ToString().ToUpper().Trim() = Me.txtTemplateTypeName.Text.ToUpper().Trim() Then
                        objcommon.ShowAlert("TemplateType Name Already Exists !", Me.Page)
                        Return False
                    End If
                Next

                dtOld.Clear()
                dr = dtOld.NewRow
                dr("vTemplateTypeName") = Me.txtTemplateTypeName.Text.Trim
                dr("vRemarks") = Me.txtRemark.Text.Trim
                dr("vTemplateTypeCode") = "0000"
                dr("iModifyBy") = Session(S_UserID)
                dtOld.Rows.Add(dr)

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                For Each dr In ds_Check.Tables(0).Rows

                    If dr("vTemplateTypeCode").ToString().Trim() <> dtOld.Rows(0).Item("vTemplateTypeCode").ToString().Trim() _
                                        And dr("vTemplateTypeName").ToString().ToUpper().Trim() = Me.txtTemplateTypeName.Text.ToUpper().Trim() Then
                        objcommon.ShowAlert("TemplateType Name Already Exists", Me.Page)
                        Return False
                    End If
                Next

                For Each dr In dtOld.Rows

                    dr("vTemplateTypeName") = Me.txtTemplateTypeName.Text.Trim
                    dr("vRemarks") = Me.txtRemark.Text.Trim
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr.AcceptChanges()

                Next
                dtOld.AcceptChanges()

            End If

            Me.ViewState(VS_dtTemplateTypeMst) = dtOld
            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...AssignValues")
            Return False
        End Try
    End Function

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim Dt As New DataTable
        Dim ds_TemplateTypeMst As DataSet
        Dim Ds_templatetypemstgrid As New DataSet
        Dim eStr As String = String.Empty
        Dim message As String = String.Empty

        Try



            If Not AssignUpdatedValues() Then
                Exit Sub
            End If

            ds_TemplateTypeMst = New DataSet
            ds_TemplateTypeMst.Tables.Add(CType(Me.ViewState(VS_dtTemplateTypeMst), Data.DataTable).Copy())
            ds_TemplateTypeMst.Tables(0).TableName = "templatetypemst"   ' New Values on the form to be updated

            If Not objLambda.Save_TemplateTypeMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_TemplateTypeMst, ds_TemplateTypeMst, "1", eStr) Then
                objcommon.ShowAlert("Error While Saving TemplateTypeMaster", Me.Page)

                Exit Sub

            End If

            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "Template Type Saved Successfully !", "Template Type Updated Successfully !")
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('" + message + "')", True)
            ResetPage()

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "......BtnSave_Click")
        End Try
    End Sub

#End Region

#Region "Cancel & Close "

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ResetPage()
        Me.Response.Redirect("frmTemplateTypeMst.aspx?mode=1")
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

#Region "Grid Events"

    Protected Sub gvtemplatetypemst_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim index As Integer = e.CommandArgument
        If e.CommandName.ToUpper = "EDIT" Then
            Me.Response.Redirect("frmTemplateTypeMst.aspx?mode=2&value=" & _
         Me.gvtemplatetypemst.Rows(index).Cells(GVC_TemplateTypeCode).Text.Trim())
        End If
    End Sub

    Protected Sub gvtemplatetypemst_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.Header Or _
                            e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GVC_TemplateTypeCode).Visible = False
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (gvtemplatetypemst.PageSize * gvtemplatetypemst.PageIndex) + 1
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "Edit"
            CType(e.Row.FindControl("lnkAudit"), ImageButton).Attributes.Add("vTemplateTypeCode", e.Row.Cells(GVC_TemplateTypeCode).Text)
            e.Row.Cells(GVC_TemplateTypeCode).Visible = False

        End If
    End Sub

    Protected Sub gvtemplatetypemst_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)

        gvtemplatetypemst.PageIndex = e.NewPageIndex
        BindGrid()

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
    Public Shared Function AuditTrail(ByVal vTemplateTypeCode As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_TemplateTypeMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim dtTemplateTypeMstHistory As New DataTable
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

            vTableName = "TemplateTypeMstHistory"
            vIdName = ""
            AuditFieldName = "vTemplateTypeCode"
            AuditFieldValue = vTemplateTypeCode
            vOtherTableName = ""
            vOtherIdName = ""


            Dim Param As String = ""


            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName

            If Not objHelp.Proc_GetAuditTrail(wStr, ds_TemplateTypeMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtTemplateTypeMstHistory Is Nothing Then
                dtTemplateTypeMstHistory.Columns.Add("SrNo")
                dtTemplateTypeMstHistory.Columns.Add("TemplateTypeName")
                dtTemplateTypeMstHistory.Columns.Add("Remarks")
                dtTemplateTypeMstHistory.Columns.Add("ModifyBy")
                dtTemplateTypeMstHistory.Columns.Add("ModifyOn")
            End If

            dt = ds_TemplateTypeMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtTemplateTypeMstHistory.NewRow()
                drAuditTrail("SrNo") = i
                drAuditTrail("TemplateTypeName") = dr("vTemplateTypeName").ToString()
                drAuditTrail("Remarks") = dr("vRemarks").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtTemplateTypeMstHistory.Rows.Add(drAuditTrail)
                dtTemplateTypeMstHistory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtTemplateTypeMstHistory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Protected Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_TemplateTypeMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtTemplateTypeMstHistory As New DataTable
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

            vTableName = "TemplateTypeMstHistory"
            vIdName = ""
            AuditFieldName = "vTemplateTypeCode"
            AuditFieldValue = hdnTemplateTypeCode.Value
            vOtherTableName = ""
            vOtherIdName = ""


            Dim Param As String = ""

            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName

            If Not objHelp.Proc_GetAuditTrail(wStr, ds_TemplateTypeMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtTemplateTypeMstHistory Is Nothing Then
                dtTemplateTypeMstHistory.Columns.Add("Sr. No")
                dtTemplateTypeMstHistory.Columns.Add("Template Type Name")
                dtTemplateTypeMstHistory.Columns.Add("Remarks")
                dtTemplateTypeMstHistory.Columns.Add("Modify By")
                dtTemplateTypeMstHistory.Columns.Add("Modify On")
            End If

            dtTemplateTypeMstHistory.AcceptChanges()
            dt = ds_TemplateTypeMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtTemplateTypeMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Template Type Name") = dr("vTemplateTypeName").ToString()
                drAuditTrail("Remarks") = dr("vRemarks").ToString()
                drAuditTrail("Modify By") = dr("vModifyBy").ToString()
                drAuditTrail("Modify On") = Convert.ToString(dr("ModifyOn"))
                dtTemplateTypeMstHistory.Rows.Add(drAuditTrail)
                dtTemplateTypeMstHistory.AcceptChanges()
                i += 1
            Next

            gvExport.DataSource = dtTemplateTypeMstHistory
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

                filename = "Audit Trail_" + hdnTemplateTypeCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

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
                strMessage.Append("Template Type Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")


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

                filename = "Audit Trail_" + hdnTemplateTypeCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"


                drAuditTrail = dtTemplateTypeMstHistory.NewRow()

                drAuditTrail("Sr. No") = i
                drAuditTrail("Template Type Name") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("Modify By") = ""
                drAuditTrail("Modify On") = ""
                dtTemplateTypeMstHistory.Rows.Add(drAuditTrail)
                dtTemplateTypeMstHistory.AcceptChanges()
                gvExport.DataSource = dtTemplateTypeMstHistory
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
                strMessage.Append("Template Type Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td align=""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")


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
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_TemplateTypeMst As New DataSet
        Dim estr As String = String.Empty
        Dim dtTemplateTypeMstHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Dim TemplateTypeCode As String = String.Empty
        Dim filename As String = String.Empty
        Dim strMessage As New StringBuilder

        Try
            wStr = "" + "##"
            ds_TemplateTypeMst = objHelp.ProcedureExecute("Proc_TemplateTypeMst ", wStr)
            wStr = String.Empty

            If Not dtTemplateTypeMstHistory Is Nothing Then
               dtTemplateTypeMstHistory.Columns.Add("Sr. No")
                dtTemplateTypeMstHistory.Columns.Add("Template Type Name")
                dtTemplateTypeMstHistory.Columns.Add("Remarks")
                dtTemplateTypeMstHistory.Columns.Add("Modify By")
                dtTemplateTypeMstHistory.Columns.Add("Modify On")
            End If

            dtTemplateTypeMstHistory.AcceptChanges()
            dt = ds_TemplateTypeMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtTemplateTypeMstHistory.NewRow()
                 drAuditTrail("Sr. No") = i
                drAuditTrail("Template Type Name") = dr("vTemplateTypeName").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("Modify By") = dr("iModifyBy").ToString()
                drAuditTrail("Modify On") = Convert.ToString(dr("dModifyOn"))
                dtTemplateTypeMstHistory.Rows.Add(drAuditTrail)
                dtTemplateTypeMstHistory.AcceptChanges()
                i += 1
            Next

            gvExportToExcel.DataSource = dtTemplateTypeMstHistory
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

                filename = "TemplateType Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

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
                strMessage.Append("Template Type Master")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")

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

                filename = "TemplateType Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                drAuditTrail = dtTemplateTypeMstHistory.NewRow()

                 drAuditTrail("Sr. No") = i
                drAuditTrail("Template Type Name") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("Modify By") = ""
                drAuditTrail("Modify On") = ""
                dtTemplateTypeMstHistory.Rows.Add(drAuditTrail)
                dtTemplateTypeMstHistory.AcceptChanges()
                gvExportToExcel.DataSource = dtTemplateTypeMstHistory
                gvExportToExcel.DataBind()

                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                gvExportToExcel.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""7""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""7""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Template Type Master")
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
