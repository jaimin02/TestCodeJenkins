Imports Microsoft.Win32
Imports System.Web.Services
Imports Newtonsoft.Json
Imports Microsoft
Imports Microsoft.Office.Interop
Imports System.IO
Imports System.Drawing

Partial Class frmDeptMst
    Inherits System.Web.UI.Page

#Region " VARIABLE DECLARATION "

    Dim ObjCommon As New clsCommon
    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtDeptMst As String = "DtDeptMst"
    Private Const VS_DeptCode As String = "DeptCode"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_DeptCode As Integer = 1
    Private Const GVC_DeptName As Integer = 2
    Private Const GVC_Remark As Integer = 3
    Private Const GVC_Edit As Integer = 4

#End Region

#Region "Load Event"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
            End If
            If gvwDeptMst.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvwDeptMst", "UIgvwDeptMst(); ", True)
            End If
        Catch ex As Exception

        End Try
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
                Me.ViewState(VS_DeptCode) = Me.Request.QueryString("Value").ToString
            End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtDeptMst) = ds.Tables("DeptMst")   ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideDeptDetails", "HideDeptDetails(); ", True)
            End If
            GenCall = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......GenCall")
        Finally
        End Try
    End Function
#End Region

#Region "GenCall_Data "
    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean

        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing

        Try

            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vDeptCode=" + Me.ViewState(VS_DeptCode).ToString()
            End If
            wStr += " And cStatusIndi <> 'D'"

            If Not objHelp.GetDeptMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
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
            Me.ShowErrorMessage(ex.Message, "....GenCallData")
            Return False
        End Try
    End Function
#End Region

#Region "GenCall_showUI "
    Private Function GenCall_ShowUI() As Boolean
        Dim dt_DeptMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = " :: Department Master ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Department Master"
            dt_DeptMst = Me.ViewState(VS_DtDeptMst)

            If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.txtDeptName.Text = dt_DeptMst.Rows(0).Item("vDeptName")
                Me.txtRemarks.Text = ""
                Me.btnSave.Text = "Update"
                Me.btnSave.ToolTip = "Update"

            End If

            If Not FillGrid() Then
                Return False
            End If

            GenCall_ShowUI = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_ShowUI")
            GenCall_ShowUI = False
        Finally
        End Try
    End Function
#End Region

#Region "FillGrid"
    Private Function FillGrid() As Boolean
        Dim ds_DeptMst As New Data.DataSet
        Dim estr As String = String.Empty
        Try

            
            If Not objHelp.GetDeptMst("cStatusIndi <> 'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_DeptMst, estr) Then
                Return False
            End If

            ds_DeptMst.Tables(0).DefaultView.Sort = "vDeptName"
            Me.gvwDeptMst.DataSource = ds_DeptMst.Tables(0).DefaultView.ToTable()
            Me.gvwDeptMst.DataBind()
            If gvwDeptMst.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvwDeptMst", "UIgvwDeptMst(); ", True)
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillGrid")
            Return False
        End Try
    End Function

#End Region

#Region "Button Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim ds_DeptMst As New DataSet
        Dim dt_DeptMst As New DataTable
        Dim estr As String = String.Empty
        Dim message As String = String.Empty
        Try
            
            If Not AssignValues() Then
                Exit Sub
            End If

            ds_DeptMst = New DataSet
            dt_DeptMst = CType(Me.ViewState(VS_DtDeptMst), DataTable)
            dt_DeptMst.TableName = "DeptMst"
            ds_DeptMst.Tables.Add(dt_DeptMst)

            If Not objLambda.Save_InsertUserMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_DeptMst, ds_DeptMst, Me.Session(S_UserID), estr) Then

                ObjCommon.ShowAlert("Error While Saving DeptMst", Me.Page)
                Exit Sub

            End If
            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "Department Details Saved Successfully !", "Department Group Details Updated Successfully !")

            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('" + message + "')", True)

            ResetPage()


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...Save")
        End Try

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ResetPage()

        Response.Redirect("frmDeptMst.aspx?mode=1")
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("frmMainPage.aspx")
    End Sub
#End Region

#Region "Assign Values "
    Private Function AssignValues() As Boolean
        Dim dr As DataRow
        Dim dt_Dept As New DataTable
        Dim ds_Check As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Try


            dt_Dept = CType(Me.ViewState(VS_DtDeptMst), DataTable)

            'For validating Duplication
            wStr = "cStatusIndi <> 'D' And vDeptName='" & Me.txtDeptName.Text.Trim() & "'"

            If Me.Request.QueryString("mode") = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                wStr += " And vDeptCode <> '" + Me.Request.QueryString("Value").ToString + "'"
            End If

            If Not objHelp.GetDeptMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Check, eStr) Then
                Me.ShowErrorMessage("Error While Getting Data From DeptMst", eStr)
                Return False
            End If

            If ds_Check.Tables(0).Rows.Count > 0 Then
                ObjCommon.ShowAlert("Department Name Already Exists !", Me.Page)
                Return False
            End If
            '*************************************

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                dt_Dept.Clear()
                dr = dt_Dept.NewRow()
                dr("vDeptCode") = "0000"
                dr("vDeptName") = Me.txtDeptName.Text.Trim
                dr("vRemark") = Me.txtRemarks.Text.Trim
                dr("iModifyBy") = Me.Session(S_UserID)
                dt_Dept.Rows.Add(dr)

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                For Each dr In dt_Dept.Rows
                    dr("vDeptName") = Me.txtDeptName.Text.Trim
                    dr("vRemark") = Me.txtRemarks.Text.Trim
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr.AcceptChanges()
                Next
                dt_Dept.AcceptChanges()
            End If

            Me.ViewState(VS_DtDeptMst) = dt_Dept
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......AssignValues")
            Return False
        End Try
    End Function
#End Region

#Region "ResetPage"

    Private Sub ResetPage()
        Me.txtDeptName.Text = ""
        Me.txtRemarks.Text = ""
        Me.ViewState(VS_DtDeptMst) = Nothing
        Me.ViewState(VS_DeptCode) = Nothing
        Me.ViewState(VS_Choice) = Nothing

    End Sub

#End Region

#Region "GRID EVENTS "

    Protected Sub gvwDeptMst_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        If Not e.Row.RowType = DataControlRowType.Pager Then
            e.Row.Cells(GVC_DeptCode).Visible = False
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (Me.gvwDeptMst.PageSize * Me.gvwDeptMst.PageIndex) + 1
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "Edit"
            CType(e.Row.FindControl("lnkAudit"), ImageButton).Attributes.Add("vDeptCode", e.Row.Cells(GVC_DeptCode).Text)

        End If

    End Sub

    Protected Sub gvwDeptMst_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim i As Integer = e.CommandArgument

        If e.CommandName.ToUpper = "EDIT" Then

            Me.Response.Redirect("frmDeptMst.aspx?mode=2&value=" & Me.gvwDeptMst.Rows(i).Cells(GVC_DeptCode).Text.Trim())

        End If

    End Sub

    Protected Sub gvwDeptMst_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        gvwDeptMst.PageIndex = e.NewPageIndex
        If Not FillGrid() Then
            Exit Sub
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
    Public Shared Function AuditTrail(ByVal vDeptCode As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_DeptMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim dtDeptMstHistory As New DataTable
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

            vTableName = "DeptMstHistory"
            vIdName = ""
            AuditFieldName = "vDeptCode"
            AuditFieldValue = vDeptCode
            vOtherTableName = ""
            vOtherIdName = ""


            Dim Param As String = ""


            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName

            If Not objHelp.Proc_GetAuditTrail(wStr, ds_DeptMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtDeptMstHistory Is Nothing Then
                dtDeptMstHistory.Columns.Add("SrNo")
                dtDeptMstHistory.Columns.Add("DeptName")
                dtDeptMstHistory.Columns.Add("Remark")
                dtDeptMstHistory.Columns.Add("ModifyBy")
                dtDeptMstHistory.Columns.Add("ModifyOn")
            End If

            dt = ds_DeptMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtDeptMstHistory.NewRow()
                drAuditTrail("SrNo") = i
                drAuditTrail("DeptName") = dr("vDeptName").ToString()
                drAuditTrail("Remark") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtDeptMstHistory.Rows.Add(drAuditTrail)
                dtDeptMstHistory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtDeptMstHistory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Protected Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_DeptMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtDeptMstHistory As New DataTable
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

            vTableName = "DeptMstHistory"
            vIdName = ""
            AuditFieldName = "vDeptCode"
            AuditFieldValue = hdnDeptCode.Value
            vOtherTableName = ""
            vOtherIdName = ""


            Dim Param As String = ""

            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName

            If Not objHelp.Proc_GetAuditTrail(wStr, ds_DeptMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtDeptMstHistory Is Nothing Then
                dtDeptMstHistory.Columns.Add("Sr. No")
                dtDeptMstHistory.Columns.Add("Department")
                dtDeptMstHistory.Columns.Add("Remarks")
                dtDeptMstHistory.Columns.Add("ModifyBy")
                dtDeptMstHistory.Columns.Add("ModifyOn")
            End If

            dtDeptMstHistory.AcceptChanges()
            dt = ds_DeptMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtDeptMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Department") = dr("vDeptName").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtDeptMstHistory.Rows.Add(drAuditTrail)
                dtDeptMstHistory.AcceptChanges()
                i += 1
            Next

            gvExport.DataSource = dtDeptMstHistory
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

                filename = "Audit Trail_" + hdnDeptCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

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
                strMessage.Append("Department Master-AuditTrail")
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

                filename = "Audit Trail_" + hdnDeptCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"


                drAuditTrail = dtDeptMstHistory.NewRow()

                drAuditTrail("Sr. No") = ""
                drAuditTrail("Department") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("ModifyBy") = ""
                drAuditTrail("ModifyOn") = ""
                dtDeptMstHistory.Rows.Add(drAuditTrail)
                dtDeptMstHistory.AcceptChanges()
                gvExport.DataSource = dtDeptMstHistory
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
                strMessage.Append("Department Master-AuditTrail")
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
            Me.ObjCommon.ShowAlert("Error While Exporting Data To Excel : " + estr, Me.Page)

            Exit Sub
        End Try
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub

#End Region

    Protected Sub btnExportToExcelGrid_Click(sender As Object, e As EventArgs) Handles btnExportToExcelGrid.Click
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Dim wStr As String = String.Empty
        Dim ds_DeptMst As New DataSet
        Dim estr As String = String.Empty
        Dim dtDeptMstHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Dim DeptCode As String = ""
        Dim filename As String = String.Empty
        Dim strMessage As New StringBuilder

        Try
            wStr = "" + "##"
            ds_DeptMst = objHelp.ProcedureExecute("Proc_DepartmentMst ", wStr)
            wStr = String.Empty
            If Not dtDeptMstHistory Is Nothing Then
                dtDeptMstHistory.Columns.Add("Sr. No")
                dtDeptMstHistory.Columns.Add("Department")
                dtDeptMstHistory.Columns.Add("Remarks")
                dtDeptMstHistory.Columns.Add("ModifyBy")
                dtDeptMstHistory.Columns.Add("ModifyOn")
            End If

            dtDeptMstHistory.AcceptChanges()
            dt = ds_DeptMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtDeptMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Department") = dr("vDeptName").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("iModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("dModifyOn"))
                dtDeptMstHistory.Rows.Add(drAuditTrail)
                dtDeptMstHistory.AcceptChanges()
                i += 1
            Next

            gvExport.DataSource = dtDeptMstHistory
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

                filename = "Department Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

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
                strMessage.Append("Department Master")
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

                filename = "Department Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"


                drAuditTrail = dtDeptMstHistory.NewRow()

                drAuditTrail("Sr. No") = ""
                drAuditTrail("Department") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("ModifyBy") = ""
                drAuditTrail("ModifyOn") = ""
                dtDeptMstHistory.Rows.Add(drAuditTrail)
                dtDeptMstHistory.AcceptChanges()
                gvExportToExcel.DataSource = dtDeptMstHistory
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
                strMessage.Append("Department Master")
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
            Me.ObjCommon.ShowAlert("Error While Exporting Data To Excel : " + estr, Me.Page)

            Exit Sub
        End Try
    End Sub

End Class
