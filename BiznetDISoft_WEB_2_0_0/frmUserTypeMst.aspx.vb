Imports Microsoft.Win32
Imports System.Web.Services
Imports Newtonsoft.Json
Imports Microsoft
Imports Microsoft.Office.Interop
Imports System.IO
Imports System.Drawing

Partial Class frmUserTypeMst
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_SAVE As String = "Save"
    Private Const VS_DtUserTypeMst As String = "DtUserTypeMst"
    Private Const VS_UserTypeCode As String = "UserTypeCode"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_Code As Integer = 1
    Private Const GVC_UserTypeName As Integer = 2
    Private Const GVC_WorkFlowStageId As Integer = 3
    Private Const GVC_IsEDCUser As Integer = 4
    Private Const GVC_Edit As Integer = 5

#End Region

#Region "Load Event"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
            End If
            If GV_UserType.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGV_UserType", "UIGV_UserType(); ", True)
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
                Me.ViewState(VS_UserTypeCode) = Me.Request.QueryString("Value").ToString
            End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtUserTypeMst) = ds.Tables("UserTypeMst")   ' adding blank DataTable in viewstate

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

        Try

            Val = Me.ViewState(VS_UserTypeCode) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vUserTypeCode=" + Val.ToString
            End If
            wStr += " And cStatusIndi <> 'D'"

            If Not objHelp.getUserTypeMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            If ds.Tables(0).Rows.Count <= 0 And _
             Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found For Selected Operation")
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
        Dim dt_OpMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try

            Page.Title = ":: UserType Master  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "User Type Master"

            dt_OpMst = Me.ViewState(VS_DtUserTypeMst)

            Choice = Me.ViewState("Choice")

            If Not FillGrid() Then
                Exit Function
            End If

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.txtUserTypeName.Text = dt_OpMst.Rows(0).Item("vUserTypeName")
                Me.DdlWorkFlow.SelectedValue = dt_OpMst.Rows(0).Item("iWorkflowStageId")
                Me.rBtnEdcUser.SelectedValue = dt_OpMst.Rows(0).Item("cIsEDCUser")
                'Me.txtRemarks.Text = dt_OpMst.Rows(0).Item("vRemarks")
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

#Region "FillGrid"
    Private Function FillGrid() As Boolean
        Dim ds_View As New Data.DataSet
        Dim estr As String = String.Empty
        Try


            If Not objHelp.getUserTypeMst("cStatusIndi <> 'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_View, estr) Then
                Return False
            End If
            ds_View.Tables(0).DefaultView.Sort = "vUserTypeName"
            Me.GV_UserType.DataSource = ds_View.Tables(0).DefaultView.ToTable
            Me.GV_UserType.DataBind()
            If GV_UserType.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGV_UserType", "UIGV_UserType(); ", True)
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
            If Me.DdlWorkFlow.SelectedIndex.ToString.Trim() = "0" Then
                CType(Me.Master.FindControl("lblerrormsg"), Label).Text = "Please Select Work Stage"
                Exit Sub
            End If

            If Me.rBtnEdcUser.SelectedValue.ToString.Trim Is Nothing Or _
            Me.rBtnEdcUser.SelectedValue.ToString.Trim.Length < 1 Then
                CType(Me.Master.FindControl("lblerrormsg"), Label).Text = "Please Specify ""Is EDC User?"""
                Exit Sub
            End If

            If Not AssignValues() Then
                Exit Sub
            End If
            ds_Save = New DataSet
            dt_Save = CType(Me.ViewState(VS_DtUserTypeMst), DataTable)
            dt_Save.TableName = "UserTypeMst"
            ds_Save.Tables.Add(dt_Save)
            If Not objLambda.Save_InsertUserTypeMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_UserTypeMst, ds_Save, Me.Session(S_UserID), estr) Then
                ObjCommon.ShowAlert("Error While Saving UserTypeMst", Me.Page)
                Exit Sub
            Else

                message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "User Type Saved Successfully !", "User Type Updated Successfully !")
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('" + message + "')", True)
                'ObjCommon.ShowAlert("Record Saved SuccessFully", Me.Page)
                ResetPage()
            End If


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...BtnSave_Click")
        End Try
    End Sub

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.ResetPage()
        Me.Response.Redirect("frmUserTypeMst.aspx?mode=1")
    End Sub
#End Region

#Region "Assign values"
    Private Function AssignValues() As Boolean
        Dim dr As DataRow
        Dim dt_User As New DataTable
        Dim ds_Check As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Try
            dt_User = CType(Me.ViewState(VS_DtUserTypeMst), DataTable)
            wStr = "cStatusIndi <> 'D' And vUserTypeName = '" + Me.txtUserTypeName.Text.Trim() + "'"
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                wStr += " And vUserTypeCode <> '" + Me.ViewState(VS_UserTypeCode).ToString() + "'" 'dt_User.Rows(0).Item("vUserTypeCode").ToString().Trim() + "'"
            End If

            If Not objHelp.getUserTypeMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Check, eStr) Then
                Me.ShowErrorMessage("Error While Getting Data From UserTypeMst", eStr)
                Return False
            End If

            If ds_Check.Tables(0).Rows.Count > 0 Then
                ObjCommon.ShowAlert("User Type Name Already Exists !", Me.Page)

               
                Return False
            End If

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                dt_User.Clear()
                dr = dt_User.NewRow()
                'vOperationName,vOperationPath,vParentUserTypeCode,iSeqNo,iModifyBy
                dr("vUserTypeCode") = "0"
                dr("vUserTypeName") = Me.txtUserTypeName.Text.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("iWorkflowStageId") = Me.DdlWorkFlow.SelectedValue.ToString.Trim()
                dr("vRemarks") = Me.txtRemarks.Text.Trim()
                dr("cIsEDCUser") = Me.rBtnEdcUser.SelectedValue.ToUpper.Trim.ToString()
                dt_User.Rows.Add(dr)

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                dt_User.Rows(0).Item("vUserTypeName") = Me.txtUserTypeName.Text.Trim()
                dt_User.Rows(0).Item("iModifyBy") = Me.Session(S_UserID)
                dt_User.Rows(0).Item("cStatusIndi") = "E"
                dt_User.Rows(0).Item("iWorkflowStageId") = Me.DdlWorkFlow.SelectedValue.ToString.Trim()
                dt_User.Rows(0).Item("vRemarks") = Convert.ToString(Me.txtRemarks.Text.Trim())
                dt_User.Rows(0).Item("cIsEDCUser") = Me.rBtnEdcUser.SelectedValue.ToUpper.Trim.ToString()
                dt_User.AcceptChanges()

            End If

                Me.ViewState(VS_DtUserTypeMst) = dt_User
                Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignValues")
            Return False
        End Try
        
    End Function
#End Region

#Region "Reset Page"
    Private Sub ResetPage()
        Me.txtUserTypeName.Text = ""
        Me.DdlWorkFlow.SelectedIndex = -1
        Me.txtRemarks.Text = ""
        'Me.ViewState(VS_DtUserTypeMst) = Nothing
        'Me.Response.Redirect("frmUserTypeMst.aspx?mode=1")
        'Me.Response.Redirect("frmUserTypeMst.aspx?mode=1&Save=Y")
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

#Region "Grid Events"
    Protected Sub GV_UserType_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim i As Integer = e.CommandArgument
        If e.CommandName.ToUpper = "EDIT" Then
            Me.Response.Redirect("frmUserTypeMst.aspx?mode=2&value=" & Me.GV_UserType.Rows(i).Cells(GVC_Code).Text.Trim())

        End If
    End Sub

    Protected Sub GV_UserType_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        'If e.Row.RowType = DataControlRowType.DataRow Or _
        'e.Row.RowType = DataControlRowType.Header Or _
        'e.Row.RowType = DataControlRowType.Footer Then
        '    e.Row.Cells(GVC_UserTypeCode).Visible = False
        'End If
    End Sub

    Protected Sub GV_UserType_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If Not e.Row.RowType = DataControlRowType.Pager Then
            e.Row.Cells(GVC_Code).Visible = False

            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + 1
                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "Edit"
                '=============
                CType(e.Row.FindControl("lnkAudit"), ImageButton).Attributes.Add("vUserTypeCode", e.Row.Cells(GVC_Code).Text)
                '=============
                If e.Row.Cells(GVC_IsEDCUser).Text.ToString.ToUpper.Trim() = "Y" Then
                    e.Row.Cells(GVC_IsEDCUser).Text = "Yes"
                ElseIf e.Row.Cells(GVC_IsEDCUser).Text.ToString.ToUpper.Trim() = "N" Then
                    e.Row.Cells(GVC_IsEDCUser).Text = "No"
                End If
                If e.Row.Cells(GVC_WorkFlowStageId).Text.ToString.ToUpper.Trim() = WorkFlowStageId_DataEntry Then
                    e.Row.Cells(GVC_WorkFlowStageId).Text = "Data Entry"
                ElseIf e.Row.Cells(GVC_WorkFlowStageId).Text.ToString.ToUpper.Trim() = WorkFlowStageId_MedicalCoding Then
                    e.Row.Cells(GVC_WorkFlowStageId).Text = "Medical Coding"
                ElseIf e.Row.Cells(GVC_WorkFlowStageId).Text.ToString.ToUpper.Trim() = WorkFlowStageId_DataValidator Then
                    e.Row.Cells(GVC_WorkFlowStageId).Text = "Data Validator"
                ElseIf e.Row.Cells(GVC_WorkFlowStageId).Text.ToString.ToUpper.Trim() = WorkFlowStageId_OnlyView Then
                    e.Row.Cells(GVC_WorkFlowStageId).Text = "View Only"
                ElseIf e.Row.Cells(GVC_WorkFlowStageId).Text.ToString.ToUpper.Trim() = WorkFlowStageId_FirstReview Then
                    e.Row.Cells(GVC_WorkFlowStageId).Text = "First Review"
                ElseIf e.Row.Cells(GVC_WorkFlowStageId).Text.ToString.ToUpper.Trim() = WorkFlowStageId_SecondReview Then
                    e.Row.Cells(GVC_WorkFlowStageId).Text = "Second Review"
                ElseIf e.Row.Cells(GVC_WorkFlowStageId).Text.ToString.ToUpper.Trim() = WorkFlowStageId_FinalReviewAndLock Then
                    e.Row.Cells(GVC_WorkFlowStageId).Text = "Final Review"
                ElseIf e.Row.Cells(GVC_WorkFlowStageId).Text.ToString.ToUpper.Trim() = WorkFlowStageId_DeleteDataEntry Then
                    e.Row.Cells(GVC_WorkFlowStageId).Text = "Delete Data Entry"
                End If
            End If
        End If
    End Sub

    Protected Sub GV_UserType_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        GV_UserType.PageIndex = e.NewPageIndex
        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub
#End Region
    '=============
#Region "Web Method"

    <WebMethod> _
    Public Shared Function AuditTrail(ByVal vUserTypeCode As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_UserTypeMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim dtUserTypeMstHistory As New DataTable
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

            vTableName = "UserTypeMstHistory"
            vIdName = ""
            AuditFieldName = "vUserTypeCode"
            AuditFieldValue = vUserTypeCode
            vOtherTableName = ""
            vOtherIdName = ""


            Dim Param As String = ""


            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName
            'wStr = " vUserTypeCode = '" + vUserTypeCode + "' Order by nUserTypeMstHistoryNo DESC"
            If Not objHelp.Proc_GetAuditTrail(wStr, ds_UserTypeMst, Param) Then
                Throw New Exception(estr)
            End If
            If Not dtUserTypeMstHistory Is Nothing Then
                dtUserTypeMstHistory.Columns.Add("SrNo")
                dtUserTypeMstHistory.Columns.Add("UserTypeName")
                dtUserTypeMstHistory.Columns.Add("WorkflowStageId")
                dtUserTypeMstHistory.Columns.Add("IsEDCUser")
                dtUserTypeMstHistory.Columns.Add("Remarks")
                dtUserTypeMstHistory.Columns.Add("ModifyBy")
                dtUserTypeMstHistory.Columns.Add("ModifyOn")
            End If
            dt = ds_UserTypeMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtUserTypeMstHistory.NewRow()
                drAuditTrail("SrNo") = i
                drAuditTrail("UserTypeName") = dr("vUserTypeName").ToString()

                'drAuditTrail("Workflow StageId") = dr("iWorkflowStageId").ToString()
                If dr("iWorkflowStageId").ToString() = "0" Then
                    drAuditTrail("WorkflowStageId") = "Data Entry"
                ElseIf dr("iWorkflowStageId").ToString() = "1" Then
                    drAuditTrail("WorkflowStageId") = "Medical Coding "
                ElseIf dr("iWorkflowStageId").ToString() = "2" Then
                    drAuditTrail("WorkflowStageId") = "Data Validator"
                ElseIf dr("iWorkflowStageId").ToString() = "3" Then
                    drAuditTrail("WorkflowStageId") = "Delete Data Entry"
                ElseIf dr("iWorkflowStageId").ToString() = "5" Then
                    drAuditTrail("WorkflowStageId") = "View Only"
                ElseIf dr("iWorkflowStageId").ToString() = "10" Then
                    drAuditTrail("WorkflowStageId") = "First Review"
                ElseIf dr("iWorkflowStageId").ToString() = "20" Then
                    drAuditTrail("WorkflowStageId") = "Second Review"
                ElseIf dr("iWorkflowStageId").ToString() = "30" Then
                    drAuditTrail("WorkflowStageId") = "Final Review"
                End If

                'drAuditTrail("IsEDCUser") = dr("cIsEDCUser").ToString()
                If dr("cIsEDCUser").ToString() = "Y" Then
                    drAuditTrail("IsEDCUser") = "Yes"
                ElseIf dr("cIsEDCUser").ToString() = "N" Then
                    drAuditTrail("IsEDCUser") = "No"
                End If

                drAuditTrail("Remarks") = dr("vRemarks").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtUserTypeMstHistory.Rows.Add(drAuditTrail)
                dtUserTypeMstHistory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtUserTypeMstHistory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Protected Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_UserTypeMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtUserTypeMstHistory As New DataTable
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
            vTableName = "UserTypeMstHistory"
            vIdName = ""
            AuditFieldName = "vUserTypeCode"
            AuditFieldValue = hdnUserTypeCode.Value
            vOtherTableName = ""
            vOtherIdName = ""


            Dim Param As String = ""


            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName


            If Not objHelp.Proc_GetAuditTrail(wStr, ds_UserTypeMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtUserTypeMstHistory Is Nothing Then
                dtUserTypeMstHistory.Columns.Add("Sr. No")
                dtUserTypeMstHistory.Columns.Add("User Type Name")
                dtUserTypeMstHistory.Columns.Add("Work Stage")
                dtUserTypeMstHistory.Columns.Add("Is EDC User?")
                dtUserTypeMstHistory.Columns.Add("Remarks")
                dtUserTypeMstHistory.Columns.Add("ModifyBy")
                dtUserTypeMstHistory.Columns.Add("ModifyOn")
            End If

            dt = ds_UserTypeMst.Tables(0)
            Dim dv As New DataView(dt)

            For Each dr As DataRow In dv.ToTable.Rows

                drAuditTrail = dtUserTypeMstHistory.NewRow()

                drAuditTrail("Sr. No") = i
                drAuditTrail("User Type Name") = dr("vUserTypeName").ToString()
                If dr("iWorkflowStageId").ToString() = "0" Then
                    drAuditTrail("Work Stage") = "Data Entry"
                ElseIf dr("iWorkflowStageId").ToString() = "1" Then
                    drAuditTrail("Work Stage") = "Medical Coding "
                ElseIf dr("iWorkflowStageId").ToString() = "2" Then
                    drAuditTrail("Work Stage") = "Data Validator"
                ElseIf dr("iWorkflowStageId").ToString() = "3" Then
                    drAuditTrail("Work Stage") = "Delete Data Entry"
                ElseIf dr("iWorkflowStageId").ToString() = "5" Then
                    drAuditTrail("Work Stage") = "View Only"
                ElseIf dr("iWorkflowStageId").ToString() = "10" Then
                    drAuditTrail("Work Stage") = "First Review"
                ElseIf dr("iWorkflowStageId").ToString() = "20" Then
                    drAuditTrail("Work Stage") = "Second Review"
                ElseIf dr("iWorkflowStageId").ToString() = "30" Then
                    drAuditTrail("Work Stage") = "Final Review"
                End If

                'drAuditTrail("IsEDCUser") = dr("cIsEDCUser").ToString()
                If dr("cIsEDCUser").ToString() = "Y" Then
                    drAuditTrail("Is EDC User?") = "Yes"
                ElseIf dr("cIsEDCUser").ToString() = "N" Then
                    drAuditTrail("Is EDC User?") = "No"
                End If

                drAuditTrail("Remarks") = dr("vRemarks").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtUserTypeMstHistory.Rows.Add(drAuditTrail)
                dtUserTypeMstHistory.AcceptChanges()
                i += 1
            Next

            gvExport.DataSource = dtUserTypeMstHistory
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

                filename = "Audit Trail_" + hdnUserTypeCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                Dim stringWriter As New System.IO.StringWriter()

                Dim writer As New HtmlTextWriter(stringWriter)
                gvExport.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""7""><center><strong><b><font color=""#000099""  size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""7""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("User Type Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td align=""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")

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

                filename = "Audit Trail_" + hdnUserTypeCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                drAuditTrail = dtUserTypeMstHistory.NewRow()
                drAuditTrail("Sr. No") = ""
                drAuditTrail("User Type Name") = ""
                drAuditTrail("Work Stage") = ""
                drAuditTrail("Is EDC User?") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("ModifyBy") = ""
                drAuditTrail("ModifyOn") = ""

                dtUserTypeMstHistory.Rows.Add(drAuditTrail)
                dtUserTypeMstHistory.AcceptChanges()

                gvExport.DataSource = dtUserTypeMstHistory
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
                strMessage.Append("User Type Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")


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
    '=============

    Protected Sub btnExportToExcelGrid_Click(sender As Object, e As EventArgs) Handles btnExportToExcelGrid.Click
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_UserTypeMst As New DataSet
        Dim estr As String = String.Empty
        Dim dtUserTypeMstHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Dim vUserTypeCode As String = String.Empty
        Dim filename As String = String.Empty
        Dim strMessage As New StringBuilder

        Try
            wStr = "" + "##"
            ds_UserTypeMst = objHelp.ProcedureExecute("Proc_UserTypeMaster ", wStr)
            wStr = String.Empty
            If Not dtUserTypeMstHistory Is Nothing Then
                dtUserTypeMstHistory.Columns.Add("Sr. No")
                dtUserTypeMstHistory.Columns.Add("User Type Name")
                dtUserTypeMstHistory.Columns.Add("Work Stage")
                dtUserTypeMstHistory.Columns.Add("Is EDC User?")
                dtUserTypeMstHistory.Columns.Add("Remarks")
                dtUserTypeMstHistory.Columns.Add("ModifyBy")
                dtUserTypeMstHistory.Columns.Add("ModifyOn")
            End If

            dtUserTypeMstHistory.AcceptChanges()
            dt = ds_UserTypeMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtUserTypeMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("User Type Name") = dr("vUserTypeName").ToString()
                drAuditTrail("Work Stage") = dr("iWorkflowStageId").ToString()
                drAuditTrail("Is EDC User?") = dr("cIsEDCUser").ToString()
                drAuditTrail("Remarks") = dr("vRemarks").ToString()
                drAuditTrail("ModifyBy") = dr("iModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("dModifyOn"))
                dtUserTypeMstHistory.Rows.Add(drAuditTrail)
                dtUserTypeMstHistory.AcceptChanges()
                i += 1
            Next

            gvExportToExcel.DataSource = dtUserTypeMstHistory
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

                filename = "UserType Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

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
                strMessage.Append("User Type Master")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")


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

                filename = "UserType Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                drAuditTrail = dtUserTypeMstHistory.NewRow()
                drAuditTrail("Sr. No") = ""
                drAuditTrail("User Type Name") = ""
                drAuditTrail("Work Stage") = ""
                drAuditTrail("Is EDC User?") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("ModifyBy") = ""
                drAuditTrail("ModifyOn") = ""
                dtUserTypeMstHistory.Rows.Add(drAuditTrail)
                dtUserTypeMstHistory.AcceptChanges()
                gvExportToExcel.DataSource = dtUserTypeMstHistory
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
                strMessage.Append("User Type Master")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td align=""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")

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
