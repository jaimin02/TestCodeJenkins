Imports Microsoft.Win32
Imports System.Web.Services
Imports Newtonsoft.Json
Imports Microsoft
Imports Microsoft.Office.Interop
Imports System.IO
Imports System.Drawing

Partial Class frmActivityGroupMst
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Private Const VS_Choice As String = "Choice"
    'Private Const VS_SAVE As String = "Save"
    Private Const VS_DtActivityGroupMst As String = "DtActivityGroupMst"
    'Private Const VS_ActivityGroupId As String = "ActivityGroupId"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_ProjectTypeName As Integer = 1
    Private Const GVC_ActivityGroupId As Integer = 2
    Private Const GVC_ActivityGroupName As Integer = 3
    Private Const GVC_Edit As Integer = 4

#End Region

#Region "Load Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...Page_Load")
        End Try
        Try
            If GV_ActivityGroup.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGV_ActivityGroup", "UIGV_ActivityGroup(); ", True)
            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region " GenCall() "
    Private Function GenCall() As Boolean
        Dim ds As New DataSet

        Try
            Me.ViewState(VS_Choice) = Me.Request.QueryString("Mode")
            '            If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
            'Me.ViewState(VS_ActivityGroupId) = Me.Request.QueryString("Value").ToString
            'End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Return False
            End If

            Me.ViewState(VS_DtActivityGroupMst) = ds.Tables("ActivityGroupMst")   ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Return False
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideActivityGroupDetails", "HideActivityGroupDetails(); ", True)
            End If
            GenCall = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall")
            Return False
        End Try
    End Function
#End Region

#Region " GenCall_Data "
    Private Function GenCall_Data(ByRef ds_Retu As DataSet) As Boolean
        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Val As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try

            If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Val = Me.Request.QueryString("Value").ToString 'Me.ViewState(VS_ActivityGroupId) 'Value of where condition
            End If
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vActivityGroupId=" + Val.ToString
            End If

            If Not objHelp.getActivityGroupMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Retu, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Return False
            End If

            If ds_Retu Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            If ds_Retu.Tables(0).Rows.Count <= 0 And _
             Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found for Selected Operation")
            End If

            'ds_Retu = ds
            GenCall_Data = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_Data")
            Return False
        End Try
    End Function
#End Region

#Region " GenCall_showUI() "
    Private Function GenCall_ShowUI() As Boolean
        Dim dt_OpMst As DataTable = Nothing

        Try

            Page.Title = " :: Activity Group Master :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Activity Group Master"
            dt_OpMst = Me.ViewState(VS_DtActivityGroupMst)

            If Not FillDropDown() Then
                Return False
            End If

            If Not FillGrid() Then
                Return False
            End If

            If Me.ViewState("Choice") <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.txtActivityGroupName.Text = dt_OpMst.Rows(0).Item("vActivityGroupName")
                Me.ddlProjectType.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_OpMst.Rows(0)("vProjectTypeCode"), dt_OpMst.Rows(0)("vProjectTypeCode").GetType)
                Me.txtRemark.Text = ""
                Me.BtnSave.Text = "Update"
                Me.BtnSave.ToolTip = "Update"
            End If

            GenCall_ShowUI = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
            Return False
        End Try
    End Function
#End Region

#Region "FillDropDown"

    Private Function FillDropDown() As Boolean
        Dim ds_ProjectType As New Data.DataSet
        Dim dv_ProjectType As New Data.DataView
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty
        Try
            'To Get Where condition of ScopeVales( Project Type )
            If Not ObjCommon.GetScopeValueWithCondition(Wstr) Then
                Return False
            End If

            If Not objHelp.GetviewProjectTypeMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                ds_ProjectType, estr) Then
                Me.ShowErrorMessage(estr, "...FillDropDown")
                Return False
            End If

            dv_ProjectType = ds_ProjectType.Tables(0).DefaultView.ToTable(True, "vProjectTypeCode,vProjectTypeName".Split(",")).DefaultView
            dv_ProjectType.Sort = "vProjectTypeName"
            Me.ddlProjectType.DataSource = dv_ProjectType
            Me.ddlProjectType.DataValueField = "vProjectTypeCode"
            Me.ddlProjectType.DataTextField = "vProjectTypeName"
            Me.ddlProjectType.DataBind()
            Me.ddlProjectType.Items.Insert(0, New ListItem("Select Project Type", ""))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillDropDown")
            Return False
        End Try
    End Function

#End Region

#Region "FillGrid"

    Private Function FillGrid() As Boolean
        Dim ds_View As New Data.DataSet
        Dim dv_View As New Data.DataView
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty
        Try
            'To Get Where condition of ScopeVales( Project Type )
            If Not ObjCommon.GetScopeValueWithCondition(Wstr) Then
                Return False
            End If

            If Not objHelp.GetviewActivityGroupMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_View, estr) Then
                Return False
            End If

            dv_View = ds_View.Tables(0).Copy.DefaultView()
            dv_View.Sort = "vProjectTypeName,vActivityGroupName"

            Me.GV_ActivityGroup.DataSource = dv_View
            Me.GV_ActivityGroup.DataBind()

            If GV_ActivityGroup.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGV_ActivityGroup", "UIGV_ActivityGroup(); ", True)
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillGrid")
            Return False
        End Try
    End Function

#End Region

#Region "Button Click"

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim ds_Save As New DataSet
        Dim dt_Save As New DataTable
        Dim estr As String = String.Empty
        Dim message As String = String.Empty

        Try
            If Not AssignValues() Then
                Exit Sub
            End If

            ds_Save = New DataSet
            dt_Save = CType(Me.ViewState(VS_DtActivityGroupMst), DataTable)
            dt_Save.TableName = "ActivityGroupMst"
            ds_Save.Tables.Add(dt_Save)

            If Not objLambda.Save_InsertActivityGroupMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_ActivityGroupMst, ds_Save, Me.Session(S_UserID), estr) Then
                ObjCommon.ShowAlert("Error while saving ActivityGroupMst", Me.Page)
                Exit Sub
            End If

            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "Activity Group Details Saved Successfully !", "Activity Group Details Updated Successfully!")

            Me.txtActivityGroupName.Text = ""
            Me.ViewState(VS_DtActivityGroupMst) = Nothing
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('" + message + "')", True)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...BtnSave_Click")
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.txtActivityGroupName.Text = ""
        Me.ViewState(VS_DtActivityGroupMst) = Nothing
        Me.Response.Redirect("frmActivityGroupMst.aspx?mode=1", False)
    End Sub

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Me.Response.Redirect("frmMainPage.aspx", False)
    End Sub

#End Region

#Region "Assign values"
    Private Function AssignValues() As Boolean
        Dim dr As DataRow
        Dim dt_User As New DataTable
        Dim ds_Valid As New DataSet
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty

        Try
            dt_User = CType(Me.ViewState(VS_DtActivityGroupMst), DataTable)

            'For validating Duplication 
            Wstr = "vProjectTypeCode='" & Me.ddlProjectType.SelectedValue.Trim & "' And vActivityGroupName='" & Me.txtActivityGroupName.Text.Trim() & "'"

            Me.objHelp.getActivityGroupMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            ds_Valid, estr)

            If (Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add And ds_Valid.Tables(0).Rows.Count > 0) Or _
                (Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit And ds_Valid.Tables(0).Rows.Count > 1) Then

                ObjCommon.ShowAlert("This Activity Group Is Already Exist For Selected Project Type !", Me.Page)

                If GV_ActivityGroup.Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGV_ActivityGroup", "UIGV_ActivityGroup(); ", True)
                End If
                Exit Function

                Return False
            End If
            '**************************

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                dt_User.Clear()
                dr = dt_User.NewRow()
                dr("vActivityGroupId") = "0"
                dr("vProjectTypeCode") = Me.ddlProjectType.SelectedValue.Trim
                dr("vActivityGroupName") = Me.txtActivityGroupName.Text.Trim()
                dr("vRemark") = Me.txtRemark.Text.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)
                dt_User.Rows.Add(dr)

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                For Each dr In dt_User.Rows
                    dr("vProjectTypeCode") = Me.ddlProjectType.SelectedValue.Trim
                    dr("vActivityGroupName") = Me.txtActivityGroupName.Text.Trim()
                    dr("vRemark") = Me.txtRemark.Text.Trim()
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = Status_Edit
                Next

            End If
            dt_User.AcceptChanges()
            Me.ViewState(VS_DtActivityGroupMst) = dt_User

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignValues")
            Return False
        End Try
    End Function
#End Region

#Region "Reset Page"

    'Private Sub ResetPage()
    '    Me.txtActivityGroupName.Text = ""
    '    Me.txtRemark.text="" 
    '    Me.ViewState(VS_DtActivityGroupMst) = Nothing
    '    Me.Response.Redirect("frmActivityGroupMst.aspx?mode=1", False)
    'End Sub

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

#Region "GRID EVENTS"

    Protected Sub GV_ActivityGroup_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim i As Integer = e.CommandArgument

        If e.CommandName.ToUpper = "EDIT" Then
            Me.Response.Redirect("frmActivityGroupMst.aspx?mode=2&value=" & Me.GV_ActivityGroup.Rows(i).Cells(GVC_ActivityGroupId).Text.Trim(), True)
        End If
    End Sub

    Protected Sub GV_ActivityGroup_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Try
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Or e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(GVC_ActivityGroupId).Visible = False

                If e.Row.RowType = DataControlRowType.DataRow Then
                    e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (Me.GV_ActivityGroup.PageSize * Me.GV_ActivityGroup.PageIndex) + 1
                    e.Row.Cells(GVC_SrNo).HorizontalAlign = HorizontalAlign.Center
                    e.Row.Cells(GVC_Edit).HorizontalAlign = HorizontalAlign.Center
                    CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandArgument = e.Row.RowIndex
                    CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandName = "Edit"
                    CType(e.Row.FindControl("lnkAudit"), ImageButton).Attributes.Add("vActivityGroupId", e.Row.Cells(GVC_ActivityGroupId).Text)
                End If
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GV_ActivityGroup_RowDataBound")
        End Try
    End Sub

    Protected Sub GV_ActivityGroup_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GV_ActivityGroup.PageIndex = e.NewPageIndex
        FillGrid()
    End Sub

#End Region

#Region "Web Method"

    <WebMethod> _
    Public Shared Function AuditTrail(ByVal vActivityGroupId As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_ActivityGroupMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim dtActivityGroupMstHistory As New DataTable
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

            vTableName = "ActivityGroupMstHistory"
            vIdName = "vProjectTypeCode"
            AuditFieldName = "vActivityGroupId"
            AuditFieldValue = vActivityGroupId
            vOtherTableName = "ProjectTypeMst"
            vOtherIdName = "vProjectTypeCode"


            Dim Param As String = ""


            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName

            If Not objHelp.Proc_GetAuditTrail(wStr, ds_ActivityGroupMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtActivityGroupMstHistory Is Nothing Then
                dtActivityGroupMstHistory.Columns.Add("SrNo")
                dtActivityGroupMstHistory.Columns.Add("ProjectTypeName")
                dtActivityGroupMstHistory.Columns.Add("ActivityGroupName")
                dtActivityGroupMstHistory.Columns.Add("Remark")
                dtActivityGroupMstHistory.Columns.Add("ModifyBy")
                dtActivityGroupMstHistory.Columns.Add("ModifyOn")
            End If
            dt = ds_ActivityGroupMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtActivityGroupMstHistory.NewRow()
                drAuditTrail("SrNo") = i
                drAuditTrail("ProjectTypeName") = dr("vProjectTypeName").ToString()
                drAuditTrail("ActivityGroupName") = dr("vActivityGroupName").ToString()
                drAuditTrail("Remark") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtActivityGroupMstHistory.Rows.Add(drAuditTrail)
                dtActivityGroupMstHistory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtActivityGroupMstHistory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Protected Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_ActivityGroupMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtActivityGroupMstHistory As New DataTable
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

            vTableName = "ActivityGroupMstHistory"
            vIdName = "vProjectTypeCode"
            AuditFieldName = "vActivityGroupId"
            AuditFieldValue = hdnActivityGroupId.Value
            vOtherTableName = "ProjectTypeMst"
            vOtherIdName = "vProjectTypeCode"


            Dim Param As String = ""

            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName

            If Not objHelp.Proc_GetAuditTrail(wStr, ds_ActivityGroupMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtActivityGroupMstHistory Is Nothing Then
                dtActivityGroupMstHistory.Columns.Add("Sr. No")
                dtActivityGroupMstHistory.Columns.Add("Project Type")
                dtActivityGroupMstHistory.Columns.Add("Activity Group Name")
                dtActivityGroupMstHistory.Columns.Add("Remarks")
                dtActivityGroupMstHistory.Columns.Add("Modify By")
                dtActivityGroupMstHistory.Columns.Add("Modify On")
            End If

            dtActivityGroupMstHistory.AcceptChanges()
            dt = ds_ActivityGroupMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtActivityGroupMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Project Type") = dr("vProjectTypeName").ToString()
                drAuditTrail("Activity Group Name") = dr("vActivityGroupName").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("Modify By") = dr("vModifyBy").ToString()
                drAuditTrail("Modify On") = Convert.ToString(dr("ModifyOn"))
                dtActivityGroupMstHistory.Rows.Add(drAuditTrail)
                dtActivityGroupMstHistory.AcceptChanges()
                i += 1
            Next

            gvExport.DataSource = dtActivityGroupMstHistory
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

                filename = "Audit Trail_" + hdnActivityGroupId.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

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
                strMessage.Append("Activity Group Master-AuditTrail")
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

                filename = "Audit Trail_" + hdnActivityGroupId.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"


                drAuditTrail = dtActivityGroupMstHistory.NewRow()

                drAuditTrail("Sr. No") = ""
                drAuditTrail("Project Type") = ""
                drAuditTrail("Activity Group Name") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("Modify By") = ""
                drAuditTrail("Modify On") = ""
                dtActivityGroupMstHistory.Rows.Add(drAuditTrail)
                dtActivityGroupMstHistory.AcceptChanges()
                gvExport.DataSource = dtActivityGroupMstHistory
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
                strMessage.Append("Activity Group Master-AuditTrail")
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
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_ActivityGroupMst As New DataSet
        Dim estr As String = String.Empty
        Dim dtActivityGroupMstHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Dim ActivityGroupId As String = String.Empty
        Dim filename As String = String.Empty
        Dim strMessage As New StringBuilder

        Try
            'wStr = "" + "##"
            If Not ObjCommon.GetScopeValueWithCondition(wStr) Then
                Me.ObjCommon.ShowAlert("Error While Exporting Data To Excel : " + estr, Me.Page)
            End If

            If Not objHelp.GetviewActivityGroupMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_ActivityGroupMst, estr) Then
                Me.ObjCommon.ShowAlert("Error While Exporting Data To Excel : " + estr, Me.Page)
            End If
            'wStr = "" + "##" + wStr
            'ds_ActivityGroupMst = objHelp.ProcedureExecute("Proc_ActivityGroupMaster ", wStr)
            wStr = String.Empty
            If Not dtActivityGroupMstHistory Is Nothing Then
                dtActivityGroupMstHistory.Columns.Add("Sr. No")
                dtActivityGroupMstHistory.Columns.Add("Project Type")
                dtActivityGroupMstHistory.Columns.Add("Activity Group Name")
                dtActivityGroupMstHistory.Columns.Add("Remarks")
                dtActivityGroupMstHistory.Columns.Add("Modify By")
                dtActivityGroupMstHistory.Columns.Add("Modify On")
            End If

            dtActivityGroupMstHistory.AcceptChanges()
            dt = ds_ActivityGroupMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtActivityGroupMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Project Type") = dr("vProjectTypeName").ToString()
                drAuditTrail("Activity Group Name") = dr("vActivityGroupName").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("Modify By") = dr("iModifyByNew").ToString()
                drAuditTrail("Modify On") = Convert.ToString(dr("dModifyOnNew"))
                dtActivityGroupMstHistory.Rows.Add(drAuditTrail)
                dtActivityGroupMstHistory.AcceptChanges()
                i += 1
            Next

            gvExportToExcel.DataSource = dtActivityGroupMstHistory
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

                filename = "ActivityGroup Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

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
                strMessage.Append("Activity Group Master")
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

                filename = "ActivityGroup Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                drAuditTrail = dtActivityGroupMstHistory.NewRow()
                drAuditTrail("Sr. No") = ""
                drAuditTrail("Project Type") = ""
                drAuditTrail("Activity Group Name") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("Modify By") = ""
                drAuditTrail("Modify On") = ""
                dtActivityGroupMstHistory.Rows.Add(drAuditTrail)
                dtActivityGroupMstHistory.AcceptChanges()
                gvExportToExcel.DataSource = dtActivityGroupMstHistory
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
                strMessage.Append("Activity Group Master")
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
End Class
