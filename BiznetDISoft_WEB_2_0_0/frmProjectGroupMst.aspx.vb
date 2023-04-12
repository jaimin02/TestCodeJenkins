Imports Microsoft.Win32
Imports System.Web.Services
Imports Newtonsoft.Json
Imports Microsoft
Imports Microsoft.Office.Interop
Imports System.IO
Imports System.Drawing

Partial Class frmProjectGroupMst
    Inherits System.Web.UI.Page
#Region "VARIABLE DECLARATIONS"

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"

    Private Const VS_SAVE As String = "Save"
    Private Const VS_DtProjectGroupMst As String = "DtProjectGroupMst"
    Private Const VS_ProjectGroupNo As String = "ProjectGroupNo"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_ProjectGroupNo As Integer = 1
    Private Const GVC_ProjectTypeCode As Integer = 2
    Private Const GVC_ProjectType As Integer = 3
    Private Const GVC_ProjectGroupDesc As Integer = 4
    Private Const GVC_Remark As Integer = 5
    Private Const GVC_Edit As Integer = 6

#End Region

#Region "Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Not IsPostBack Then
                GenCall()
            End If
        Catch ex As Exception

        End Try
        Try
            If GV_ProjectGroup.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGV_ProjectGroup", "UIGV_ProjectGroup(); ", True)
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
                Me.ViewState(VS_ProjectGroupNo) = Me.Request.QueryString("Value").ToString
            End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtProjectGroupMst) = ds.Tables("ProjectGroupMst")   ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideSponsorDetails", "HideSponsorDetails(); ", True)
            End If
            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall")

        Finally

        End Try

    End Function

#End Region

#Region " GenCall_Data "

    Private Function GenCall_Data(ByRef ds_Retu As DataSet) As Boolean

        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Val As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing
        'Dim objFFR As New FFReporting.FFReporting

        Try

           
            Val = Me.ViewState(VS_ProjectGroupNo) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "nProjectGroupNo=" + Val.ToString
            End If


            If Not objHelp.GetProjectgroupMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
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

            ds_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_Data")

        Finally

        End Try
    End Function

#End Region

#Region " GenCall_showUI() "

    Private Function GenCall_ShowUI() As Boolean
        Dim dt_ProTypeMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = ":: Project Group Master  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Project Group Master"

            dt_ProTypeMst = Me.ViewState(VS_DtProjectGroupMst)

            Choice = Me.ViewState("Choice")

            If Not FillDropDown() Then
                Exit Function
            End If

            If Not FillGrid() Then
                Exit Function
            End If

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Me.ddlProjectType.SelectedValue = dt_ProTypeMst.Rows(0).Item("vProjectTypeCode")
                Me.txtProjectGroupDesc.Text = dt_ProTypeMst.Rows(0).Item("vProjectGroupDesc")
                Me.txtRemark.Text = ""
                Me.BtnSave.Text = "Update"
                Me.BtnSave.ToolTip = "Update"

            End If

            'Me.BtnSave.Attributes.Add("OnClick", "return Validation();")

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
        Finally
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
            'Wstr = "nScopeNo=" & Me.Session(S_ScopeNo)

            If Not objHelp.GetviewProjectTypeMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                ds_ProjectType, estr) Then
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
        Dim estr As String = String.Empty
        Dim Wstr_Scope As String = String.Empty
        Dim Wstr As String = String.Empty
        Try

          
            ''To Get Where condition of ScopeVales( Project Type )
            'If Not ObjCommon.GetScopeValueWithCondition(Wstr_Scope) Then
            '    Return False
            'End If

            Wstr = "cStatusIndi<>'D'"
            If Not objHelp.View_ProjectgroupMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_View, estr) Then
                Return False
            End If
            ds_View.Tables(0).DefaultView.Sort = "vProjectGroupDesc"
            Me.GV_ProjectGroup.DataSource = ds_View.Tables(0).DefaultView.ToTable()
            Me.GV_ProjectGroup.DataBind()

            If GV_ProjectGroup.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGV_ProjectGroup", "UIGV_ProjectGroup(); ", True)
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..FillGrid")
            Return False
        End Try
    End Function

#End Region

#Region "Button Click"

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim ds_Save As New DataSet
        Dim dt_Save As New DataTable
        Dim estr As String = String.Empty
        Dim ProjectGroupNo As String = String.Empty
        Dim wStr As String = String.Empty
        Dim message As String = String.Empty
        'Dim dr As DataRow
        Try
           

            If Not AssignValues() Then
                Exit Sub
            End If

            ds_Save = New DataSet
            dt_Save = CType(Me.ViewState(VS_DtProjectGroupMst), DataTable)
            dt_Save.TableName = "ProjectGroupMst"
            ds_Save.Tables.Add(dt_Save)

            If Not objLambda.Save_ProjectgroupMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_ProjectGroupMst, _
                                                    ds_Save, Me.Session(S_UserID), estr) Then

                ObjCommon.ShowAlert("Error While Saving ProjectGroupMst", Me.Page)
                Exit Sub

            End If
            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "Project Details Saved Successfully !", "Project Details Updated Successfully !")
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('" + message + "')", True)

            ResetPage()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...BtnSave_Click")
        End Try
    End Sub

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click

        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.ResetPage()
        Me.Response.Redirect("frmProjectGroupMst.aspx?mode=1")
    End Sub

#End Region

#Region "Assign values"

    Private Function AssignValues() As Boolean
        Dim dr As DataRow
        Dim dt_Project As New DataTable
        Dim ds_Check As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim Wstr_Scope As String = String.Empty
        Try
           
            dt_Project = CType(Me.ViewState(VS_DtProjectGroupMst), DataTable)

            wStr = "vProjectGroupDesc='" + Me.txtProjectGroupDesc.Text.Trim() + "' And cStatusIndi<>'D' "

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                wStr += " And nProjectGroupNo <> '" + dt_Project.Rows(0).Item("nProjectGroupNo").ToString().Trim() + "'"
            End If

            If Not objHelp.View_ProjectgroupMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Check, eStr) Then
                Me.ShowErrorMessage("Error While Getting Data From ViewProjectGroupMst", eStr)
                Return False
            End If

            If ds_Check.Tables(0).Rows.Count > 0 Then
                ObjCommon.ShowAlert("Project Group Already Exists !", Me.Page)
                If GV_ProjectGroup.Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGV_ProjectGroup", "UIGV_ProjectGroup(); ", True)
                End If

                Exit Function
                Return False
            End If


            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                dt_Project.Clear()
                dr = dt_Project.NewRow()
                dr("nProjectGroupNo") = "0"
                dr("vProjectTypeCode") = Me.ddlProjectType.SelectedValue.Trim()
                dr("vProjectGroupDesc") = Me.txtProjectGroupDesc.Text.Trim()
                dr("vRemark") = Me.txtRemark.Text.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)
                dt_Project.Rows.Add(dr)
            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                For Each dr In dt_Project.Rows
                    dr("vProjectTypeCode") = Me.ddlProjectType.SelectedValue.Trim()
                    dr("vProjectGroupDesc") = Me.txtProjectGroupDesc.Text.Trim()
                    dr("vRemark") = Me.txtRemark.Text.Trim()
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                Next
            End If

            Me.ViewState(VS_DtProjectGroupMst) = dt_Project
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....AssignValues")
            Return False
        End Try
    End Function

#End Region

#Region "Reset Page"
    Private Sub ResetPage()
        Me.txtProjectGroupDesc.Text = ""
        Me.ViewState(VS_DtProjectGroupMst) = Nothing
        'Me.Response.Redirect("frmProjectGroupMst.aspx?mode=1")

    End Sub
#End Region

#Region "Grid Events"

    Protected Sub GV_ProjectGroup_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim Index As Integer = e.CommandArgument
        Dim dr As DataRow
        Dim dt_Project As New DataTable
        Dim ds_Save As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim Wstr_Scope As String = String.Empty


        If e.CommandName.ToUpper = "EDIT" Then

            Me.Response.Redirect("frmProjectGroupMst.aspx?mode=2&value=" & Me.GV_ProjectGroup.Rows(Index).Cells(GVC_ProjectGroupNo).Text.Trim())

        ElseIf e.CommandName.ToUpper = "DELETE" Then

            dt_Project = CType(Me.ViewState(VS_DtProjectGroupMst), DataTable)
            dt_Project.Clear()
            dr = dt_Project.NewRow()
            dr("nProjectGroupNo") = Me.GV_ProjectGroup.Rows(Index).Cells(GVC_ProjectGroupNo).Text.Trim()
            dr("vProjectTypeCode") = Me.GV_ProjectGroup.Rows(Index).Cells(GVC_ProjectTypeCode).Text.Trim()
            dr("vProjectGroupDesc") = Me.GV_ProjectGroup.Rows(Index).Cells(GVC_ProjectGroupDesc).Text.Trim()
            dr("vRemark") = Me.GV_ProjectGroup.Rows(Index).Cells(GVC_Remark).Text.Trim()
            dr("iModifyBy") = Me.Session(S_UserID)
            dr("cStatusIndi") = "D"
            dt_Project.Rows.Add(dr)


            dt_Project.TableName = "ProjectGroupMst"
            ds_Save.Tables.Add(dt_Project.Copy())

            If Not objLambda.Save_ProjectgroupMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_ProjectGroupMst, _
                                                    ds_Save, Me.Session(S_UserID), eStr) Then

                ObjCommon.ShowAlert("Error While Deleteing ProjectGroupMst", Me.Page)
                Exit Sub

            End If

            ObjCommon.ShowAlert("Record Deleted SuccessFully", Me.Page)

            If Not FillGrid() Then
                Exit Sub
            End If

        End If
    End Sub

    Protected Sub GV_ProjectGroup_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        'If e.Row.RowType = DataControlRowType.DataRow Or _
        '        e.Row.RowType = DataControlRowType.Header Or _
        '        e.Row.RowType = DataControlRowType.Footer Then

        '    e.Row.Cells(GVC_ProjectGroupNo).Visible = False
        '    e.Row.Cells(GVC_ProjectTypeCode).Visible = False

        'End If
    End Sub

    Protected Sub GV_ProjectGroup_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If Not e.Row.RowType = DataControlRowType.Pager Then
            e.Row.Cells(GVC_ProjectGroupNo).Visible = False
            e.Row.Cells(GVC_ProjectTypeCode).Visible = False


            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (Me.GV_ProjectGroup.PageIndex * Me.GV_ProjectGroup.PageSize) + 1
                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "Edit"

                CType(e.Row.FindControl("lnkDelete"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkDelete"), ImageButton).CommandName = "Delete"

                CType(e.Row.FindControl("lnkAudit"), ImageButton).Attributes.Add("nProjectGroupNo", e.Row.Cells(GVC_ProjectGroupNo).Text)

                'CType(e.Row.FindControl("lnkDelete"), LinkButton).OnClientClick = "return confirm('Are you sure you want to delete " + e.Row.Cells(GVC_ProjectGroupDesc).Text.Trim() + "?');"

            End If
        End If
    End Sub

    Protected Sub GV_ProjectGroup_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        GV_ProjectGroup.PageIndex = e.NewPageIndex
        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub

    Protected Sub GV_ProjectGroup_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)

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
    Public Shared Function AuditTrail(ByVal nProjectGroupNo As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_ProjectGroupMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim dtProjectGroupMstHistory As New DataTable
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

            vTableName = "ProjectGroupMstHistory"
            vIdName = "vProjectTypeCode"
            AuditFieldName = "nProjectGroupNo"
            AuditFieldValue = nProjectGroupNo
            vOtherTableName = "ProjectTypeMst"
            vOtherIdName = "vProjectTypeCode"


            Dim Param As String = ""


            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName

            If Not objHelp.Proc_GetAuditTrail(wStr, ds_ProjectGroupMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtProjectGroupMstHistory Is Nothing Then
                dtProjectGroupMstHistory.Columns.Add("SrNo")
                dtProjectGroupMstHistory.Columns.Add("ProjectTypeName")
                dtProjectGroupMstHistory.Columns.Add("ProjectGroupDesc")
                dtProjectGroupMstHistory.Columns.Add("Remark")
                dtProjectGroupMstHistory.Columns.Add("ModifyBy")
                dtProjectGroupMstHistory.Columns.Add("ModifyOn")
            End If
            dt = ds_ProjectGroupMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtProjectGroupMstHistory.NewRow()
                drAuditTrail("SrNo") = i
                drAuditTrail("ProjectTypeName") = dr("vProjectTypeName").ToString()
                drAuditTrail("ProjectGroupDesc") = dr("vProjectGroupDesc").ToString()
                drAuditTrail("Remark") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtProjectGroupMstHistory.Rows.Add(drAuditTrail)
                dtProjectGroupMstHistory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtProjectGroupMstHistory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Protected Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_ProjectGroupMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtProjectGroupMstHistory As New DataTable
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

            vTableName = "ProjectGroupMstHistory"
            vIdName = "vProjectTypeCode"
            AuditFieldName = "nProjectGroupNo"
            AuditFieldValue = hdnProjectGroupNo.Value
            vOtherTableName = "ProjectTypeMst"
            vOtherIdName = "vProjectTypeCode"


            Dim Param As String = ""

            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName

            If Not objHelp.Proc_GetAuditTrail(wStr, ds_ProjectGroupMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtProjectGroupMstHistory Is Nothing Then
                dtProjectGroupMstHistory.Columns.Add("Sr. No")
                dtProjectGroupMstHistory.Columns.Add("Project Type")
                dtProjectGroupMstHistory.Columns.Add("Project Group")
                dtProjectGroupMstHistory.Columns.Add("Remarks")
                dtProjectGroupMstHistory.Columns.Add("Modify By")
                dtProjectGroupMstHistory.Columns.Add("Modify On")
            End If

            dtProjectGroupMstHistory.AcceptChanges()
            dt = ds_ProjectGroupMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtProjectGroupMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Project Type") = dr("vProjectTypeName").ToString()
                drAuditTrail("Project Group") = dr("vProjectGroupDesc").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("Modify By") = dr("vModifyBy").ToString()
                drAuditTrail("Modify On") = Convert.ToString(dr("ModifyOn"))
                dtProjectGroupMstHistory.Rows.Add(drAuditTrail)
                dtProjectGroupMstHistory.AcceptChanges()
                i += 1
            Next

            gvExport.DataSource = dtProjectGroupMstHistory
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

                filename = "Audit Trail_" + hdnProjectGroupNo.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

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
                strMessage.Append("Project Group Master-AuditTrail")
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

                filename = "Audit Trail_" + hdnProjectGroupNo.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"


                drAuditTrail = dtProjectGroupMstHistory.NewRow()

                drAuditTrail("Sr. No") = ""
                drAuditTrail("Project Type") = ""
                drAuditTrail("Project Group") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("Modify By") = ""
                drAuditTrail("Modify On") = ""
                dtProjectGroupMstHistory.Rows.Add(drAuditTrail)
                dtProjectGroupMstHistory.AcceptChanges()
                gvExport.DataSource = dtProjectGroupMstHistory
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
                strMessage.Append("Project Group Master-AuditTrail")
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
        Dim ds_ProjectGroupMst As New DataSet
        Dim estr As String = String.Empty
        Dim dtProjectGroupMstHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Dim ProjectGroupNo As String = String.Empty
        Dim filename As String = String.Empty
        Dim strMessage As New StringBuilder
        Dim ds_View As New Data.DataSet

        Try
            wStr = "cStatusIndi<>'D'"
            If Not objHelp.View_ProjectgroupMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_View, estr) Then
                Me.ObjCommon.ShowAlert("Error While Exporting Data To Excel", Me.Page)
                Me.ShowErrorMessage(estr, "....btnExportToExcelGrid_Click")
                Exit Sub
            End If
            ds_View.Tables(0).DefaultView.Sort = "vProjectGroupDesc"
            dt = ds_View.Tables(0).DefaultView.ToTable()
            wStr = String.Empty

            If Not dtProjectGroupMstHistory Is Nothing Then
                dtProjectGroupMstHistory.Columns.Add("Sr. No")
                dtProjectGroupMstHistory.Columns.Add("Project Type")
                dtProjectGroupMstHistory.Columns.Add("Project Group")
                dtProjectGroupMstHistory.Columns.Add("Remarks")
                dtProjectGroupMstHistory.Columns.Add("Modify By")
                dtProjectGroupMstHistory.Columns.Add("Modify On")
            End If

            dtProjectGroupMstHistory.AcceptChanges()
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtProjectGroupMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Project Type") = dr("vProjectTypeName").ToString()
                drAuditTrail("Project Group") = dr("vProjectGroupDesc").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("Modify By") = dr("iModifyBy").ToString()
                drAuditTrail("Modify On") = Convert.ToString(dr("dModifyOn"))
                dtProjectGroupMstHistory.Rows.Add(drAuditTrail)
                dtProjectGroupMstHistory.AcceptChanges()
                i += 1
            Next

            gvExportToExcel.DataSource = dtProjectGroupMstHistory
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

                filename = "ProjectGroup Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

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
                strMessage.Append("Project Group Master")
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

                filename = "ProjectGroup Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                drAuditTrail = dtProjectGroupMstHistory.NewRow()
                 drAuditTrail("Sr. No") = ""
                drAuditTrail("Project Type") = ""
                drAuditTrail("Project Group") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("Modify By") = ""
                drAuditTrail("Modify On") = ""
                dtProjectGroupMstHistory.Rows.Add(drAuditTrail)
                dtProjectGroupMstHistory.AcceptChanges()
                gvExportToExcel.DataSource = dtProjectGroupMstHistory
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
                strMessage.Append("Project Group Master")
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
