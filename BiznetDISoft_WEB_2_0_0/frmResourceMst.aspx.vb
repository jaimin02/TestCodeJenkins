Imports Microsoft.Win32
Imports System.Web.Services
Imports Newtonsoft.Json
Imports Microsoft
Imports Microsoft.Office.Interop
Imports System.IO
Imports System.Drawing

Partial Class frmResourceMst
    Inherits System.Web.UI.Page

#Region " VARIABLE DECLARATION "

    Dim ObjCommon As New clsCommon
    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtResourceMst As String = "DtResourceMst"
    Private Const VS_ResourceCode As String = "ResourceCode"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_Location As Integer = 1
    Private Const GVC_Department As Integer = 2
    Private Const GVC_Resource As Integer = 3
    Private Const GVC_UOM As Integer = 4
    Private Const GVC_Capacity As Integer = 5
    Private Const GVC_Edit As Integer = 6
    Private Const GVC_Code As Integer = 7

#End Region

#Region "Load Event"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region " GenCall() "
    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try
            Choice = Me.Request.QueryString("Mode")
            Me.ViewState(VS_Choice) = Choice   'To use it while saving
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_ResourceCode) = Me.Request.QueryString("Value").ToString
            End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtResourceMst) = ds.Tables("ResourceMst")   ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideResourceDetails", "HideResourceDetails(); ", True)
            End If
            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..GenCall")

        Finally

        End Try

    End Function
#End Region

#Region " GenCall_Data "
    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean

        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Val As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing


        Try

            Val = Me.ViewState(VS_ResourceCode) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vResourceCode=" + Val.ToString
            End If


            If Not objHelp.getresourcemst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
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
            Me.ShowErrorMessage(ex.Message, "....GenCall_Data")

        Finally

        End Try
    End Function
#End Region

#Region " GenCall_showUI() "
    Private Function GenCall_ShowUI() As Boolean
        Dim dt_ResourceMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = ":: Resource Master   :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Resource Master"
            Choice = Me.ViewState(VS_Choice)

            If Not FillGrid() Then
                Exit Function
            End If

            If Not FillDropDown() Then
                Exit Function
            End If

            dt_ResourceMst = Me.ViewState(VS_DtResourceMst)
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.txtResourceName.Text = ConvertDbNullToDbTypeDefaultValue(dt_ResourceMst.Rows(0)("vResourceName"), dt_ResourceMst.Rows(0)("vResourceName").GetType)
                Me.ddlLocation.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_ResourceMst.Rows(0)("vLocationCode"), dt_ResourceMst.Rows(0)("vLocationCode").GetType)
                Me.ddlDept.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_ResourceMst.Rows(0)("vDeptCode"), dt_ResourceMst.Rows(0)("vDeptCode").GetType)
                Me.TXTCapacity.Text = ConvertDbNullToDbTypeDefaultValue(dt_ResourceMst.Rows(0)("nResourceCapacity"), dt_ResourceMst.Rows(0)("nResourceCapacity").GetType)
                Me.TxtUOM.Text = ConvertDbNullToDbTypeDefaultValue(dt_ResourceMst.Rows(0)("vUOM"), dt_ResourceMst.Rows(0)("vUOM").GetType)
                btnSave.Text = "Update"
                btnSave.ToolTip = "Update"

            End If


            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, " ....GenCallShowUI")
        Finally
        End Try
    End Function
#End Region

#Region "FillGrid"

    Private Function FillGrid() As Boolean
        Dim ds_ResourceMst As New Data.DataSet
        Dim estr As String = String.Empty

        Try

            If Not objHelp.GetViewResourceMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_ResourceMst, estr) Then
                Return False
            End If

            ds_ResourceMst.Tables(0).DefaultView.Sort = "vLocationName,vDeptName,vUOM,vResourceName"
            Me.gvwResourceMst.DataSource = ds_ResourceMst.Tables(0).DefaultView.ToTable()
            Me.gvwResourceMst.DataBind()
            If gvwResourceMst.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvwResourceMst", "UIgvwResourceMst(); ", True)
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillGrid")
            Return False
        End Try
    End Function

#End Region

#Region "FillDropDown"

    Private Function FillDropDown() As Boolean

        Dim ds_ddlLocation As New DataSet
        Dim estr As String = String.Empty
        Dim ds_ddlDept As New DataSet

        Try

            If Not objHelp.FillDropDown("LocationMst", "vLocationCode", "vLocationName", "cLocationType = 'L'", ds_ddlLocation, estr) Then
                Return False
            Else
                ddlLocation.DataSource = ds_ddlLocation.Tables(0)
                ddlLocation.DataValueField = "vLocationCode"
                ddlLocation.DataTextField = "vLocationName"
                ddlLocation.DataBind()
                ddlLocation.Items.Insert(0, New ListItem("Select Location", ""))
            End If

            If Not objHelp.FillDropDown("DeptMst", "vDeptCode", "vDeptName", "", ds_ddlDept, estr) Then
                Return False
            Else
                ddlDept.DataSource = ds_ddlDept.Tables(0)
                ddlDept.DataValueField = "vDeptCode"
                ddlDept.DataTextField = "vDeptName"
                ddlDept.DataBind()
                ddlDept.Items.Insert(0, New ListItem("Select Deoartment", ""))
                Return True
            End If


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillDropDown")
            Return False
        End Try

        Return True
    End Function

#End Region

#Region "Button Events"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim ds_ResourceMst As New DataSet
        Dim dt_ResourceMst As New DataTable
        Dim estr As String = String.Empty
        Dim message As String = String.Empty
        Try
            AssignValues()
            ds_ResourceMst = New DataSet
            dt_ResourceMst = CType(Me.ViewState(VS_DtResourceMst), DataTable)
            dt_ResourceMst.TableName = "ResourceMst"
            ds_ResourceMst.Tables.Add(dt_ResourceMst)
            If Not objLambda.Save_InsertResourcemst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_resourcemst, ds_ResourceMst, Me.Session(S_UserID), estr) Then
                ObjCommon.ShowAlert("Error While Saving ResourceMst", Me)
                Exit Sub
            End If
            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "Resource Details Saved Successfully !", "Resource Details Updated Successfully !")
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('" + message + "')", True)
            ResetPage()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ResetPage()
        Response.Redirect("frmResourceMst.aspx?mode=1")
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

#Region "Assign Values "
    Private Sub AssignValues()
        Dim dr As DataRow
        Dim dt_Dept As New DataTable
        dt_Dept = CType(Me.ViewState(VS_DtResourceMst), DataTable)
        If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
            dt_Dept.Clear()
            dr = dt_Dept.NewRow()
            dr("vResourceCode") = "0000"
            dr("vLocationCode") = Me.ddlLocation.SelectedValue.Trim
            dr("vDeptCode") = Me.ddlDept.SelectedValue.Trim
            dr("vResourceName") = Me.txtResourceName.Text.Trim()
            'dr("nResourceCapacity") = Me.TXTCapacity.Text.Trim()
            dr("nResourceCapacity") = IIf(Me.TXTCapacity.Text.Trim = "", DBNull.Value, TXTCapacity.Text.Trim)
            dr("vRemark") = Me.txtRemark.Text.Trim()
            dr("vUOM") = Me.TxtUOM.Text.Trim()
            dr("iModifyBy") = Me.Session(S_UserID)
            dt_Dept.Rows.Add(dr)
        ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
            For Each dr In dt_Dept.Rows
                dr("vLocationCode") = Me.ddlLocation.SelectedValue.Trim
                dr("vDeptCode") = Me.ddlDept.SelectedValue.Trim
                dr("vResourceName") = Me.txtResourceName.Text.Trim
                dr("nResourceCapacity") = Me.TXTCapacity.Text.Trim()
                dr("vRemark") = Me.txtRemark.Text.Trim()
                dr("vUOM") = Me.TxtUOM.Text.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "E"
                dr.AcceptChanges()
            Next
            dt_Dept.AcceptChanges()
        End If
        Me.ViewState(VS_DtResourceMst) = dt_Dept
    End Sub
#End Region

#Region "reset Page"
    Private Function ResetPage() As Boolean
        ddlLocation.SelectedIndex = 0
        ddlDept.SelectedIndex = 0
        Me.txtResourceName.Text = ""
        Me.TxtUOM.Text = ""
        Me.TXTCapacity.Text = ""
        Me.txtRemark.Text = ""
       Return True
    End Function
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

#Region "Grid Events"

    Protected Sub gvwResourceMst_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or _
               e.Row.RowType = DataControlRowType.Header Or _
               e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GVC_Code).Visible = False
        End If
    End Sub

    Protected Sub gvwResourceMst_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (Me.gvwResourceMst.PageIndex * Me.gvwResourceMst.PageSize) + 1
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "Edit"
            CType(e.Row.FindControl("lnkAudit"), ImageButton).Attributes.Add("vResourceCode", e.Row.Cells(GVC_Code).Text)
        End If
    End Sub

    Protected Sub gvwResourceMst_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim i As Integer = e.CommandArgument
        If e.CommandName.ToUpper = "EDIT" Then
            Me.txtResourceName.Text = "A"
            Me.Response.Redirect("frmResourceMst.aspx?mode=2&value=" & Me.gvwResourceMst.Rows(i).Cells(GVC_Code).Text.Trim())
        End If
    End Sub

    Protected Sub gvwResourceMst_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        gvwResourceMst.PageIndex = e.NewPageIndex
        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub

#End Region

#Region "Web Method"

    <WebMethod> _
    Public Shared Function AuditTrail(ByVal vResourceCode As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim Str As String = String.Empty
        Dim ds_ResourceMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim dtResourceMstHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable

        Try

            
            Str = Convert.ToString(vResourceCode)
            ds_ResourceMst = objHelp.ProcedureExecute("Proc_GetResourceMstAuditTrail", Str)

            If Not dtResourceMstHistory Is Nothing Then
                dtResourceMstHistory.Columns.Add("SrNo")
                dtResourceMstHistory.Columns.Add("Location")
                dtResourceMstHistory.Columns.Add("DeptName")
                dtResourceMstHistory.Columns.Add("ResourceName")
                dtResourceMstHistory.Columns.Add("UOM")
                dtResourceMstHistory.Columns.Add("ResourceCapacity")
                dtResourceMstHistory.Columns.Add("Remark")
                dtResourceMstHistory.Columns.Add("ModifyBy")
                dtResourceMstHistory.Columns.Add("ModifyOn")
            End If

            dt = ds_ResourceMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtResourceMstHistory.NewRow()
                drAuditTrail("SrNo") = i
                drAuditTrail("Location") = dr("vLocationName").ToString()
                drAuditTrail("DeptName") = dr("vDeptName").ToString()
                drAuditTrail("ResourceName") = dr("vResourceName").ToString()
                drAuditTrail("UOM") = dr("vUOM").ToString()
                drAuditTrail("ResourceCapacity") = dr("nResourceCapacity").ToString()
                drAuditTrail("Remark") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("dModifyOn"))
                dtResourceMstHistory.Rows.Add(drAuditTrail)
                dtResourceMstHistory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtResourceMstHistory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Protected Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        'Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_ResourceMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtResourceMstHistory As New DataTable
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

            If Not dtResourceMstHistory Is Nothing Then
                dtResourceMstHistory.Columns.Add("Sr. No")
                dtResourceMstHistory.Columns.Add("Location")
                dtResourceMstHistory.Columns.Add("Department")
                dtResourceMstHistory.Columns.Add("Resource")
                dtResourceMstHistory.Columns.Add("Unit of Measurement")
                dtResourceMstHistory.Columns.Add("Capacity")
                dtResourceMstHistory.Columns.Add("Remarks")
                dtResourceMstHistory.Columns.Add("Modify By")
                dtResourceMstHistory.Columns.Add("Modify On")
            End If

            dtResourceMstHistory.AcceptChanges()

            wStr = hdnResourceCode.Value + "##"
            ds_ResourceMst = objHelp.ProcedureExecute("Proc_GetResourceMstAuditTrail", wStr)

            If Not ds_ResourceMst.Tables(0) Is Nothing Then
                dt = ds_ResourceMst.Tables(0)
                Dim dv As New DataView(dt)
                For Each dr As DataRow In dv.ToTable.Rows
                    drAuditTrail = dtResourceMstHistory.NewRow()
                    drAuditTrail("Sr. No") = i
                    drAuditTrail("Location") = dr("vLocationName").ToString()
                    drAuditTrail("Department") = dr("vDeptName").ToString()
                    drAuditTrail("Resource") = dr("vResourceName").ToString()
                    drAuditTrail("Unit of Measurement") = dr("vUOM").ToString()
                    drAuditTrail("Capacity") = dr("nResourceCapacity").ToString()
                    drAuditTrail("Remarks") = dr("vRemark").ToString()
                    drAuditTrail("Modify By") = dr("vModifyBy").ToString()
                    drAuditTrail("Modify On") = Convert.ToString(dr("dModifyOn"))
                    dtResourceMstHistory.Rows.Add(drAuditTrail)
                    dtResourceMstHistory.AcceptChanges()
                    i += 1
                Next

                gvExport.DataSource = dtResourceMstHistory
                gvExport.DataBind()
            End If


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

                filename = "Audit Trail_" + hdnResourceCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                Dim stringWriter As New System.IO.StringWriter()

                Dim writer As New HtmlTextWriter(stringWriter)
                gvExport.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""9""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")





                strMessage.Append("<td colspan=""9""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Resource Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")


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

                filename = "Audit Trail_" + hdnResourceCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"


                drAuditTrail = dtResourceMstHistory.NewRow()

                drAuditTrail("Sr. No") = i
                drAuditTrail("Location") = ""
                drAuditTrail("Department") = ""
                drAuditTrail("Resource") = ""
                drAuditTrail("Unit of Measurement") = ""
                drAuditTrail("Capacity") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("Modify By") = ""
                drAuditTrail("Modify On") = ""
                dtResourceMstHistory.Rows.Add(drAuditTrail)
                dtResourceMstHistory.AcceptChanges()
                gvExport.DataSource = dtResourceMstHistory
                gvExport.DataBind()


                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                gvExport.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()


                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""9""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""9""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Resource Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td></td><td></td><td align=""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")


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
        'Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_ResourceMst As New DataSet
        Dim estr As String = String.Empty
        Dim dtResourceMstHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Dim vDeptStageCode As String = String.Empty
        Dim filename As String = String.Empty
        Dim strMessage As New StringBuilder

        Try
            wStr = "" + "##"
            ds_ResourceMst = objHelp.ProcedureExecute("Proc_ResourceMst ", wStr)
            wStr = String.Empty
            If Not dtResourceMstHistory Is Nothing Then
                dtResourceMstHistory.Columns.Add("Sr. No")
                dtResourceMstHistory.Columns.Add("Location")
                dtResourceMstHistory.Columns.Add("Department")
                dtResourceMstHistory.Columns.Add("Resource")
                dtResourceMstHistory.Columns.Add("Unit of Measurement")
                dtResourceMstHistory.Columns.Add("Capacity")
                dtResourceMstHistory.Columns.Add("Remarks")
                dtResourceMstHistory.Columns.Add("Modify By")
                dtResourceMstHistory.Columns.Add("Modify On")
            End If

            dtResourceMstHistory.AcceptChanges()
            dt = ds_ResourceMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtResourceMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Location") = dr("vLocationName").ToString()
                drAuditTrail("Department") = dr("vDeptName").ToString()
                drAuditTrail("Resource") = dr("vResourceName").ToString()
                drAuditTrail("Unit of Measurement") = dr("vUOM").ToString()
                drAuditTrail("Capacity") = dr("nResourceCapacity").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("Modify By") = dr("iModifyBy").ToString()
                drAuditTrail("Modify On") = Convert.ToString(dr("dModifyOn"))
                dtResourceMstHistory.Rows.Add(drAuditTrail)
                dtResourceMstHistory.AcceptChanges()
                i += 1
            Next

            gvExportToExcel.DataSource = dtResourceMstHistory
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
                        cell.CssClass = " "
                    Next
                Next

                'style to format numbers to string
                Dim style As String = "<style> .textmode { } </style>"
                Response.Write(style)

                Dim info As String = String.Empty
                Dim gridviewHtml As String = String.Empty

                filename = "Resource Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                Dim stringWriter As New System.IO.StringWriter()

                Dim writer As New HtmlTextWriter(stringWriter)
                gvExportToExcel.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""9""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""9""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Resource Master")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")


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

                filename = "Resource Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                drAuditTrail = dtResourceMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Location") = ""
                drAuditTrail("Department") = ""
                drAuditTrail("Resource") = ""
                drAuditTrail("Unit of Measurement") = ""
                drAuditTrail("Capacity") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("Modify By") = ""
                drAuditTrail("Modify On") = ""
                dtResourceMstHistory.Rows.Add(drAuditTrail)
                dtResourceMstHistory.AcceptChanges()
                gvExportToExcel.DataSource = dtResourceMstHistory
                gvExportToExcel.DataBind()

                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                gvExportToExcel.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""9""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""9""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Resource Master")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td></td><td></td><td align=""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")

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
