Imports Newtonsoft.Json
Imports System.Data.OleDb
Imports System.Data
Imports System.Web.Script.Serialization
Imports System.Collections.Generic
Partial Class frmECRFProtocol_New
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objCommon As New clsCommon
    Private objhelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

    Private Vs_Choice As String = "Choice"
    Private eStr_Retu As String = String.Empty
    Private Const VS_MedExWorkspaceId As String = "MedExWorkspaceId"
    Private Const VS_Dt_GVParent As String = "Dt_GVParent"
    Private Const VS_Dt_GVChild As String = "Dt_GVChild"
    Private Const Vs_DtWorkSpaceNodeDetail As String = "DtWorkSpaceNodeDetail"
    Private Const Normal As String = "00"
    Private Const Multidose As String = "01"
    Public IsProjectFreeze As Boolean = False
    Public IsProjectLock As Boolean = False
    Private Const GVP_Edit As Integer = 0

    Private Const GVP_SRNo As Integer = 1
    Private Const GVP_DisplayName As Integer = 2
    Private Const GVP_iNodeIndex As Integer = 3
    Private Const GVP_iNodeId As Integer = 4
    Private Const GVP_IsPreDose As Integer = 5
    Private Const GVP_RefTime As Integer = 6
    Private Const GVP_devTime As Integer = 7
    Private Const GVP_chkIsPreDose As Integer = 8
    Private Const GVP_Day As Integer = 9
    Private Const GVP_Dose As Integer = 10
    Private Const GVP_DomainName As Integer = 11
    Private Const GVP_Barcode As Integer = 12
    Private Const GVP_Rearrange As Integer = 13
    Private Const GVP_Delete As Integer = 14
    Private Const GVP_Status As Integer = 15
    Private Const GVP_Preview As Integer = 16
    Private Const GVP_ActivityId As Integer = 17
    Private Const GVP_Period As Integer = 18

    Private Const GVC_Edit As Integer = 0
    Private Const GVC_SRNo As Integer = 1
    Private Const GVC_DisplayName As Integer = 2
    Private Const GVC_iNodeIndex As Integer = 3
    Private Const GVC_iNodeId As Integer = 4
    Private Const GVC_IsPreDose As Integer = 5
    Private Const GVC_RefTime As Integer = 6
    Private Const GVC_devTime As Integer = 7
    Private Const GVC_chkIsPreDose As Integer = 8
    Private Const GVC_Day As Integer = 9
    Private Const GVC_Dose As Integer = 10
    Private Const GVC_DomainName As Integer = 11
    Private Const GVC_Barcode As Integer = 12
    Private Const GVC_delete As Integer = 13
    Private Const GVC_Rearrange As Integer = 14
    Private Const GVC_Status As Integer = 15
    Private Const GVC_Preview As Integer = 16
    Private Const GVC_ActivityId As Integer = 17
    Private Const GVC_Period As Integer = 18

    '========================For reviewer=======================
    Private Const VS_UserTypeMst As String = "UserTypeProfileMst"
    Private Const gvc_chkSelectreviewer As Integer = 0
    Private Const gvc_reviewerlevel As Integer = 1
    Private Const gvc_profile As Integer = 2
    Private Const gvc_profilecode As Integer = 3
    Private Const gvc_authenticationcheckbox As Integer = 4
    Private Const gvc_authentication As Integer = 5
    Private Const gvc_ReviwWorkflowstageid As Integer = 6
    Private Const gvc_ActualWorkflowstageid As Integer = 7
    Private Const Vs_AllowRemarks As String = "AllowRemarks"

#End Region

#Region "Page_Load"

    Protected Sub Page_Load() Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
            End If
            'add by shivani pandya for latest repeatition
            Me.Session(S_SelectedRepeatation) = ""
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....Page_Load")
        End Try
    End Sub

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Try
            If Not GenCall_Data(ds) Then
                Me.objCommon.ShowAlert("Error While Getting Data from WorkSpaceNodeDetail.", Me.Page)
            End If

            If Not GenCall_ShowUI() Then
                Exit Function
            End If
            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall")
        End Try
    End Function

#End Region

#Region "GenCall Data"

    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean
        Dim Val As String = String.Empty
        Dim wStr As String = String.Empty
        Dim DS_WorkSpaceNodeDetail As New DataSet
        Try
            Val = Me.ViewState(VS_MedExWorkspaceId)

            If objhelp.GetViewWorkSpaceNodeDetail("1=2", DS_WorkSpaceNodeDetail, eStr_Retu) Then
                Me.ViewState(Vs_DtWorkSpaceNodeDetail) = DS_WorkSpaceNodeDetail.Tables(0)
                Me.ViewState(VS_Dt_GVParent) = DS_WorkSpaceNodeDetail.Tables(0)
                Me.ViewState(VS_Dt_GVChild) = DS_WorkSpaceNodeDetail.Tables(0)
            Else
                Me.ShowErrorMessage("Error While Getting Blank Structure WorkSpaceNodeDetail", ".......GenCall_Data")
            End If

            If DS_WorkSpaceNodeDetail Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If
            ds_DWR_Retu = DS_WorkSpaceNodeDetail
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_Data")
        End Try
    End Function

#End Region

#Region "GenCall_ShowUI"

    Private Function GenCall_ShowUI() As Boolean
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim sender As New Object
        Dim e As EventArgs
        Try
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "CRF Activity Management"
            Page.Title = " :: CRF Activity Management ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            Choice = Me.ViewState(Vs_Choice)
            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtProject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
            End If

            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                DisableControlforCT()
            End If

            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)

            If Not FillActivityGroup() Then
                Exit Function
            End If
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
            End If
            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...............GenCall_ShowUI")
        Finally
        End Try
    End Function

#End Region

#Region "Drop-Down Events"

    Private Function FillActivityGroup() As Boolean
        Dim wStr As String = String.Empty
        Dim ds_ActivityGroup As DataSet = Nothing
        Dim dv_ActivityGroup As New DataView
        Try
            If Not objCommon.GetScopeValueWithCondition(wStr) Then
                Exit Function
            End If
            wStr += " And cStatusIndi <> 'D'"
            If Not objhelp.GetviewActivityGroupMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ActivityGroup, eStr_Retu) Then
                ShowErrorMessage("", eStr_Retu)
                Return False
            End If
            dv_ActivityGroup = ds_ActivityGroup.Tables(0).DefaultView
            dv_ActivityGroup.Sort = "vActivityGroupName"
            Me.ddlActivityGroup.DataSource = dv_ActivityGroup.ToTable()
            Me.ddlActivityGroup.DataTextField = "vActivityGroupName"
            Me.ddlActivityGroup.DataValueField = "vActivityGroupId"
            Me.ddlActivityGroup.DataBind()
            Me.ddlActivityGroup.Items.Insert(0, New ListItem("Please Select Activity Group", "1"))
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...FillActivityGroup")
            Return False
        End Try
    End Function

    Private Function FillActivity() As Boolean
        Dim wStr As String = String.Empty
        Dim ds_Type As DataSet = Nothing
        Dim dv_Activity As DataView = Nothing
        Try
            wStr = "vActivityGroupId='" & Me.ddlActivityGroup.SelectedValue.Trim() & "' And cStatusIndi <> 'D'"
            If Not objhelp.getActivityMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Type, eStr_Retu) Then
                ShowErrorMessage("", eStr_Retu)
            End If
            dv_Activity = ds_Type.Tables(0).DefaultView
            dv_Activity.Sort = "vActivityName"
            Me.ddlActivity.DataSource = dv_Activity.ToTable()
            Me.ddlActivity.DataValueField = "vActivityId"
            Me.ddlActivity.DataTextField = "vActivityName"
            Me.ddlActivity.DataBind()
            Me.ddlActivity.Items.Insert(0, New ListItem("Please Select Activity", "1"))

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......FillActivity")
        End Try

    End Function

    Private Function FillPeriod(ByVal VWorkspaceId As String, ByVal dropToFill As DropDownList) As Boolean
        Dim wStr As String = ""
        Dim dsPeriod As New DataSet
        Dim iPeriodNumbers As Integer = 0
        Try
            If VWorkspaceId = "" Then
                Me.ShowErrorMessage("Problem While Getting vWorkspaceId", "")
                FillPeriod = False
                Exit Function
            End If

            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
            If Not objhelp.GetWorkspaceProtocolDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            dsPeriod, eStr_Retu) Then
                Me.ShowErrorMessage(eStr_Retu, "")
                Exit Function
            End If

            dropToFill.Items.Clear()
            If dsPeriod.Tables(0).Rows.Count > 0 AndAlso dsPeriod.Tables(0).Rows(0).Item("iNoOfPeriods").ToString() <> System.DBNull.Value.ToString() Then
                iPeriodNumbers = dsPeriod.Tables(0).Rows(0).Item("iNoOfPeriods")
            End If

            If iPeriodNumbers > 0 Then
                For count As Integer = 1 To dsPeriod.Tables(0).Rows(0).Item("iNoOfPeriods")
                    dropToFill.Items.Add(count.ToString())
                Next
            End If

            If dropToFill.Items.Count <= 0 Then
                dropToFill.Items.Add(0)
            End If

            If chkMultiDose.Checked = False Then
                If Me.ddlPeriod.Items.Count > 1 Then
                    Me.ddlPeriod.Items.Insert(0, "All")
                End If
            End If

            FillPeriod = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillPeriod()")
        End Try

    End Function

    Private Function FillParentActicity() As Boolean

        Dim ActivityparamArry(1) As String
        Dim Wstr As String = String.Empty
        Dim ds_WorkspaceNodeDetail As DataSet = Nothing
        Dim dt_WorkSpaceActivity As New DataTable
        Dim dt_WorkSpaceNodes As New DataTable

        Try
            Wstr = "vWorkSpaceId = '" & Me.HProjectId.Value.Trim() & "' And iParentNodeId = " & 1 & " AND cStatusIndi<>'D' "
            If Me.ddlPeriod.SelectedValue.ToString.ToUpper <> "ALL" Then
                Wstr += " AND iPeriod = " + Me.ddlPeriod.SelectedValue.ToString
            End If

            Wstr += " Order by iNodeNo"
            If Not objhelp.GetViewWorkSpaceNodeDetail(Wstr, ds_WorkspaceNodeDetail, eStr_Retu) Then
                Me.objCommon.ShowAlert("Error while getting Information: " & eStr_Retu, Me.Page)
                Exit Function
            End If

            If chkMultiDose.Checked = True Or Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                ActivityparamArry(0) = "vNodeDisplayName"
                ActivityparamArry(1) = "iNodeId"
                dt_WorkSpaceActivity = ds_WorkspaceNodeDetail.Tables(0).DefaultView.ToTable("Activity", True, ActivityparamArry)
                Me.ddlParentAct.DataSource = dt_WorkSpaceActivity.DefaultView.ToTable()
                Me.ddlParentAct.DataTextField = "vNodeDisplayName"
                Me.ddlParentAct.DataValueField = "iNodeId"
                Me.ddlParentAct.DataBind()
                Me.ddlParentAct.Items.Insert(0, New ListItem("Please Select Parent Activity", "0"))
            Else
                ActivityparamArry(0) = "vActivityName"
                ActivityparamArry(1) = "vActivityId"
                dt_WorkSpaceActivity = ds_WorkspaceNodeDetail.Tables(0).DefaultView.ToTable("Activity", True, ActivityparamArry)
                dt_WorkSpaceActivity.DefaultView.Sort = "vActivityName"
                Me.ddlParentAct.DataSource = dt_WorkSpaceActivity.DefaultView.ToTable()
                Me.ddlParentAct.DataTextField = "vActivityName"
                Me.ddlParentAct.DataValueField = "vActivityId"
                Me.ddlParentAct.DataBind()
                Me.ddlParentAct.Items.Insert(0, New ListItem("Please Select Parent Activity", "0"))
            End If
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillParentActicity()")
            Return False
        End Try
    End Function

    Private Function FillReferenceActicity() As Boolean
        Dim ActivityparamArry(1) As String
        Dim Wstr As String = String.Empty
        Dim ds_WorkspaceNodeDetail As DataSet = Nothing
        Dim dt_WorkSpaceActivity As New DataTable
        Dim dt_WorkSpaceNodes As New DataTable

        Try
            Wstr = "vWorkSpaceId = '" & Me.HProjectId.Value.Trim() & "' AND cStatusIndi<>'D' "
            If Me.ddlPeriod.SelectedValue.ToString.ToUpper <> "ALL" Then
                Wstr += " AND iPeriod = " + Me.ddlPeriod.SelectedValue.ToString
            End If

            Wstr += " Order by iNodeNo"
            If Not objhelp.GetViewWorkSpaceNodeDetail(Wstr, ds_WorkspaceNodeDetail, eStr_Retu) Then

                Me.objCommon.ShowAlert("Error while getting Information: " & eStr_Retu, Me.Page)
                Exit Function
            End If

            If chkMultiDose.Checked = True Then
                ActivityparamArry(0) = "vNodeDisplayName"
                ActivityparamArry(1) = "iNodeId"
                dt_WorkSpaceActivity = ds_WorkspaceNodeDetail.Tables(0).DefaultView.ToTable("Activity", True, ActivityparamArry)
                dt_WorkSpaceActivity.DefaultView.Sort = "vNodeDisplayName"
                Me.ddlRefAct.DataSource = dt_WorkSpaceActivity.DefaultView.ToTable()
                Me.ddlRefAct.DataTextField = "vNodeDisplayName"
                Me.ddlRefAct.DataValueField = "iNodeId"
                Me.ddlRefAct.DataBind()
                Me.ddlRefAct.Items.Insert(0, New ListItem("Please Select Reference Activity", "0"))
            Else
                ActivityparamArry(0) = "vActivityName"
                ActivityparamArry(1) = "vActivityId"
                dt_WorkSpaceActivity = ds_WorkspaceNodeDetail.Tables(0).DefaultView.ToTable("Activity", True, ActivityparamArry)
                dt_WorkSpaceActivity.DefaultView.Sort = "vActivityName"
                Me.ddlRefAct.DataSource = dt_WorkSpaceActivity.DefaultView.ToTable()
                Me.ddlRefAct.DataTextField = "vActivityName"
                Me.ddlRefAct.DataValueField = "vActivityId"
                Me.ddlRefAct.DataBind()
                Me.ddlRefAct.Items.Insert(0, New ListItem("Please Select Reference Activity", "0"))
            End If
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillReferenceActicity()")
            Return False
        End Try
    End Function

#End Region

#Region "Drop-Down Selected index changed event"

    Protected Sub ddlParentAct_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CType(Me.ViewState(VS_Dt_GVChild), DataTable).Rows.Clear()
        Me.ddlActivityGroup.SelectedIndex = 0
        Me.ddlActivity.Items.Clear()
        Me.divParent.Style.Add("Display", "none")
        Me.divChild.Style.Add("Display", "none")

    End Sub

    Protected Sub ddlActivityGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlActivityGroup.SelectedIndexChanged
        FillActivity()
    End Sub

    Protected Sub ddlRefAct_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRefAct.SelectedIndexChanged
        Me.divParent.Style.Add("Display", "none")
        Me.divChild.Style.Add("Display", "none")
    End Sub

    Protected Sub ddlPeriod_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPeriod.SelectedIndexChanged
        Try
            Me.divChild.Style.Add("Display", "none")
            Me.divParent.Style.Add("Display", "none")
            If Not FillGridAndActivity() Then
                Me.ShowErrorMessage("Problem While getting Parent Activities for this project", "")
            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "Radio-Buttom And Checkbox event"

    Protected Sub rbtSelection_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtSelection.SelectedIndexChanged

        If Not resetpage() Then
            Exit Sub
        End If
        Me.trPeriod.Style.Add("Display", "")
        Me.divActivityGroup.Style.Add("Display", "")
        If rbtSelection.SelectedValue() = "ParentActivity" Then
            Me.divParentActivity.Style.Add("Display", "none")

            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                DisableControlforCT()
            Else
                Me.trmultidose.Style.Add("Display", "")
                Me.trDeviationTime.Style.Add("Display", "none")
                Me.trreferencetime.Style.Add("Display", "none")
            End If
        ElseIf rbtSelection.SelectedValue() = "ChildActivity" Then
            Me.divParentActivity.Style.Add("Display", "")
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                DisableControlforCT()
            Else
                Me.trDeviationTime.Style.Add("Display", "")
                Me.trreferencetime.Style.Add("Display", "")
            End If
        ElseIf rbtSelection.SelectedValue() = "DefineWorkflow" Then
            Me.trmultidose.Style.Add("Display", "none")
            Me.trDeviationTime.Style.Add("Display", "none")
            Me.trreferencetime.Style.Add("Display", "none")
            Me.divActivityGroup.Style.Add("Display", "none")
            Me.divParentActivity.Style.Add("Display", "none")
            Me.trPeriod.Style.Add("Display", "none")
        End If

    End Sub

    Protected Sub chkMultiDose_CheckedChanged(sender As Object, e As EventArgs) Handles chkMultiDose.CheckedChanged

        If Not resetpage() Then
            Exit Sub
        End If

    End Sub

#End Region

#Region "Button Event"

    Protected Sub btnSetProject_Click(ByVal Sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim Wstr As String = String.Empty
        Dim ActivityparamArry(1) As String
        Dim dt_WorkSpaceActivity As New DataTable
        Dim dt_WorkSpaceNodes As New DataTable
        Dim dt_WorkSpaceNodeDetail As New DataTable
        Dim ds_CRFVersionDetail As DataSet = Nothing
        Dim ds_ParentWorkspace As DataSet = Nothing
        Dim ds_Editchecks As DataSet = Nothing
        Dim ds_CrfHdr As DataSet = Nothing
        Dim ds_Check As DataSet = Nothing
        Dim ds_WorkspaceNodeDetail As DataSet = Nothing
        Dim dv_ParentGrid As New DataView

        Try
            Me.ViewState(IsProjectFreeze) = False
            Me.ViewState(IsProjectLock) = False
            Me.GV_ChildActivity.DataSource = Nothing
            Me.GV_ChildActivity.DataBind()
            Me.Divlnk.Style.Add("Display", "inline")
            Me.btnaudittrail.Style.Add("Display", "none")
            Me.ViewState(Vs_AllowRemarks) = "Y"

            'to check Project is Locked or not
            Wstr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D' Order by iTranNo desc"
            If Not Me.objhelp.GetCRFLockDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                           ds_Check, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If
            If ds_Check.Tables(0).Rows.Count > 0 Then
                If Convert.ToString(ds_Check.Tables(0).Rows(0)("cLockFlag")).Trim.ToUpper() = "L" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ProjectLocked", "$(document).ready(function(){pageLoad(); msgalert('Project Is Locked')});", True)
                    Me.ViewState(IsProjectLock) = True
                Else
                    Me.ViewState(IsProjectLock) = False
                End If
            End If
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Me.ViewState(IsProjectLock) = False Then
                Wstr = "vWorkspaceId='" + Me.HProjectId.Value.ToString.Trim() + "'"
                If Not objhelp.GetData("View_CRFVersionForDataEntryControl", "*", Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CRFVersionDetail, eStr_Retu) Then  'To get Project Details
                    Throw New Exception(eStr_Retu)
                End If

                If ds_CRFVersionDetail.Tables(0).Rows.Count > 0 Then
                    Me.VersionNo.Text = ds_CRFVersionDetail.Tables(0).Rows(0)("nVersionNo").ToString
                    Me.VersionDate.Text = CType(ds_CRFVersionDetail.Tables(0).Rows(0)("dVersiondate"), Date).ToString("dd-MMM-yyyy")
                    Me.VersionDtl.Style.Add("display", "")
                    If ds_CRFVersionDetail.Tables(0).Rows(0)("cFreezeStatus").ToString.Trim.ToUpper = "F" Then
                        ImageLockUnlock.Attributes.Add("src", "images/Freeze.jpg")
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ProjectFreeze", "$(document).ready(function(){pageLoad(); msgalert('Project Is In Freeze State, First UnFreeze It Then Proceed')});", True)
                        Me.ViewState(IsProjectFreeze) = True
                        'Add by shivani pandya for independent profile issue
                        Me.btnSave.Style.Add("Display", "none")
                    Else
                        Me.btnSave.Style.Add("Display", "")
                        Me.ViewState(IsProjectFreeze) = False
                        ImageLockUnlock.Attributes.Add("src", "images/UnFreeze.jpg")
                    End If
                Else
                    Me.VersionDtl.Attributes.Add("style", "display:none;")
                End If
            End If
            If rbtSelection.SelectedValue() = "DefineWorkflow" Then
                If Not Me.FillReviewerLevel() Then
                    Exit Sub
                End If
            Else
                If Me.ViewState(IsProjectFreeze) = True Or Me.ViewState(IsProjectLock) = True Then
                    If Not Me.DisableControl() Then
                        Throw New Exception("Error In Disable Control")
                    End If
                Else
                    If Not Me.EnableControl() Then
                        Throw New Exception("Error In Enable Control")
                    End If
                End If

                'only view the Data
                If Me.ViewState(IsProjectLock) = False Or Me.ViewState(IsProjectFreeze) = False Then
                    If Convert.ToString(Me.Session(S_WorkFlowStageId)) = WorkFlowStageId_OnlyView AndAlso Not Convert.ToString(ConfigurationManager.AppSettings("CRFActivityMgmtForMI")).Trim().Contains(Me.Session(S_UserType)) Then
                        'If Convert.ToString(Me.Session(S_WorkFlowStageId)) = WorkFlowStageId_OnlyView AndAlso Me.Session(S_UserType) <> 35 Then
                        If Not Me.DisableControl() Then
                            Throw New Exception("Error In Disable Control")
                        End If
                    Else
                        If Not Me.EnableControl() Then
                            Throw New Exception("Error In Enable Control")
                        End If

                    End If

                End If
                    Me.trrepetitions.Style.Add("Display", "")
                    If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial AndAlso Me.rbtSelection.SelectedValue.ToString.ToUpper = "PARENTACTIVITY" AndAlso Me.chkMultiDose.Checked = False Then
                        Me.trrepetitions.Style.Add("Display", "none")
                    End If

                    'For bind Period Drop-Down'
                    Me.divParent.Style.Add("Display", "")
                    If Not FillPeriod(Me.HProjectId.Value, Me.ddlPeriod) Then
                        Me.ShowErrorMessage("Problem While getting periods for this project", "")
                    End If

                    'For Activity Grid and Activity Dropdown fill
                    If Not FillGridAndActivity() Then
                        Me.ShowErrorMessage("Problem While getting Parent Activities for this project", "")
                    End If

                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "ApplyDtAddActivity", "fnApplyDtAddParentActivity();", True)
                    Me.divChild.Style.Add("Dsiplay", "none")
                End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....btnSetProject")
        End Try

    End Sub

    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim wStr As String = String.Empty
        Dim ds_WorkspaceNodeDetail As DataSet = Nothing
        Dim dv_Grid As New DataView
        Try
            If chkMultiDose.Checked = True Or Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                wStr = "vWorkSpaceId = '" & Me.HProjectId.Value.Trim() & "' And iParentNodeId = " & _
                  Me.ddlParentAct.SelectedValue.Trim() & "  and cStatusIndi<>'D' Order by iNodeNo "
            Else
                wStr = "vWorkSpaceId = '" & Me.HProjectId.Value.Trim() & "' And vParentActivityId = '" & _
               Me.ddlParentAct.SelectedValue.Trim() & "' and cStatusIndi<>'D' "
                If Me.ddlPeriod.SelectedValue.ToString.ToUpper <> "ALL" Then
                    wStr += " and iperiod = " + Me.ddlPeriod.SelectedValue.ToString
                End If
                wStr += " order by iNodeNo"
            End If

            If Not objhelp.GetViewWorkSpaceNodeDetail(wStr, ds_WorkspaceNodeDetail, eStr_Retu) Then
                Me.objCommon.ShowAlert("Error while getting Information: " & eStr_Retu, Me.Page)
                Exit Sub
            End If

            If Not ds_WorkspaceNodeDetail Is Nothing AndAlso ds_WorkspaceNodeDetail.Tables(0).Rows.Count > 0 Then
                ds_WorkspaceNodeDetail.Tables(0).Columns("cStatusIndi").Expression = "'E'"
                dv_Grid = ds_WorkspaceNodeDetail.Tables(0).DefaultView()
                CType(Me.ViewState(Vs_DtWorkSpaceNodeDetail), DataTable).Rows.Clear()
                Me.ViewState(Vs_DtWorkSpaceNodeDetail) = dv_Grid.ToTable

                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                    dv_Grid = New DataView
                    dv_Grid = ds_WorkspaceNodeDetail.Tables(0).DefaultView()
                    CType(Me.ViewState(VS_Dt_GVChild), DataTable).Rows.Clear()
                    Me.ViewState(VS_Dt_GVChild) = dv_Grid.ToTable()
                    Me.GV_ChildActivity.DataSource = dv_Grid
                    Me.GV_ChildActivity.DataBind()
                    Me.divParent.Style.Add("Display", "none")
                    Me.divChild.Style.Add("Display", "")
                    Me.btnSchedule.Style.Add("Display", "none")
                Else
                    If Me.ddlPeriod.SelectedValue = "All" Then
                        dv_Grid = New DataView
                        dv_Grid = ds_WorkspaceNodeDetail.Tables(0).DefaultView()
                        dv_Grid.RowFilter = "iPeriod = 1"

                        If dv_Grid.ToTable.Rows.Count <= 0 Then
                            dv_Grid = New DataView
                            dv_Grid = ds_WorkspaceNodeDetail.Tables(0).DefaultView()
                            dv_Grid.RowFilter = "iPeriod = 2"
                        End If
                    Else
                        dv_Grid = New DataView
                        dv_Grid = ds_WorkspaceNodeDetail.Tables(0).DefaultView()
                        dv_Grid.RowFilter = "iPeriod =" & Me.ddlPeriod.SelectedValue.Trim()
                    End If
                    CType(Me.ViewState(VS_Dt_GVChild), DataTable).Rows.Clear()
                    Me.ViewState(VS_Dt_GVChild) = dv_Grid.ToTable()
                    Me.GV_ChildActivity.DataSource = dv_Grid
                    Me.GV_ChildActivity.DataBind()
                    Me.GV_ChildActivity.Columns(GVC_delete).Visible = False
                    Me.divParent.Style.Add("Display", "none")
                    Me.divChild.Style.Add("Display", "")
                    Me.btnSchedule.Style.Add("Display", "")
                End If

                Me.GV_ChildActivity.Columns(GVC_delete).Visible = False
                Me.GV_ChildActivity.Columns(GVC_Edit).Visible = True
                Me.GV_ChildActivity.Columns(GVC_Rearrange).Visible = True

                If Me.ViewState(IsProjectFreeze) Or Me.ViewState(IsProjectLock) Then
                    Me.GV_ChildActivity.Columns(GVC_Edit).Visible = False
                    Me.GV_ChildActivity.Columns(GVC_Rearrange).Visible = False
                End If
            Else
                CType(Me.ViewState(VS_Dt_GVChild), DataTable).Rows.Clear()
                Me.objCommon.ShowAlert("No Child Activity Found!!!", Me.Page)
                Me.divChild.Style.Add("Display", "none")
                Me.divParent.Style.Add("Display", "none")
                Exit Sub
            End If

            If Me.ViewState(IsProjectFreeze) = True Or Me.ViewState(IsProjectLock) = True Then
                Me.btnSchedule.Style.Add("Display", "none")
            End If

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "ApplyDtAddActivity", "fnApplyDtAddChildActivity();", True)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....btnSearch")
        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim DtWorkSpaceNodeDetail As New DataTable
        Dim Dr As DataRow
        Dim irep As Integer
        Dim INodeId As Integer
        Dim ds_Parent As New DataSet
        Dim ds_RefAct As New DataSet
        Dim wstr As String = String.Empty
        Try
            Me.btnSave.Style.Add("Display", "")
            DtWorkSpaceNodeDetail = IIf(Me.ddlParentAct.SelectedIndex = 0, CType(Me.ViewState(VS_Dt_GVParent), DataTable), CType(Me.ViewState(VS_Dt_GVChild), DataTable))
            DtWorkSpaceNodeDetail.DefaultView.RowFilter = "cStatusIndi = 'N'"
            INodeId = DtWorkSpaceNodeDetail.DefaultView.ToTable.Rows.Count + 1
            DtWorkSpaceNodeDetail.DefaultView.RowFilter = ""
            DtWorkSpaceNodeDetail.DefaultView.ToTable.AcceptChanges()
            If chkMultiDose.Checked = False And Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                If Me.ddlPeriod.SelectedValue.ToString.ToUpper = "ALL" Then
                    INodeId = DtWorkSpaceNodeDetail.Rows.Count + 1
                End If
            End If
            For irep = 1 To Val(Me.txtRepeatation.Text)
                Dr = DtWorkSpaceNodeDetail.NewRow()
                Dr("vWorkSpaceId") = Me.HProjectId.Value.Trim()
                Dr("iNodeIndex") = 0
                Dr("iNodeId") = INodeId
                Dr("iNodeNo") = INodeId
                Dr("vNodeName") = Me.ddlActivity.SelectedItem.Text.Trim()
                Dr("vNodeDisplayName") = Me.ddlActivity.SelectedItem.Text.Trim() + "-" + irep.ToString.Trim()
                Dr("cNodeTypeIndi") = "N"
                Dr("iParentNodeId") = IIf(Me.ddlParentAct.SelectedIndex = 0, 1, Me.ddlParentAct.SelectedValue.ToString).ToString
                Dr("iperiod") = IIf(Me.ddlPeriod.SelectedValue.ToString.ToUpper = "ALL", 0, Me.ddlPeriod.SelectedValue.ToString)
                Dr("vActivityId") = Me.ddlActivity.SelectedValue.Trim()

                If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial AndAlso Me.ddlParentAct.SelectedIndex <> 0 Then
                    If Me.txtRefTimeInterval.Text.Trim = "" Then
                        Dr("nRefTime") = Nothing
                    Else
                        Dr("nRefTime") = Format(Val(Me.txtRefTimeInterval.Text.Trim()), "00.00")
                    End If
                    If irep > 1 Then
                        If Me.txtRefTimeInterval.Text.Trim = "" Then
                            Dr("nRefTime") = Nothing
                        Else
                            Dr("nRefTime") = Format(Val(TimeInterval(DtWorkSpaceNodeDetail.Rows(DtWorkSpaceNodeDetail.Rows.Count - 1).Item("nRefTime") + _
                                                    Val(Me.txtRefTimeInterval.Text.Trim()))), "00.00")
                        End If
                    End If
                End If

                If Me.ddlRefAct.SelectedIndex = 0 Then
                    If Me.ddlParentAct.SelectedIndex = 0 Then
                        Dr("iRefNodeId") = 1
                    Else
                        Dr("iRefNodeId") = Me.ddlParentAct.SelectedValue.ToString
                    End If
                Else
                    Dr("iRefNodeId") = Me.ddlRefAct.SelectedValue.ToString
                End If

                Dr("cCloneFlag") = IIf(Me.ddlParentAct.SelectedIndex = 0, "N", "H").ToString
                Dr("cRequiredFlag") = "Y"
                Dr("cPublishFlag") = "N"
                Dr("iModifyBy") = Me.Session(S_UserID)
                Dr("dModifyOn") = DateAndTime.Now
                Dr("cStatusIndi") = "N"

                If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial AndAlso Me.ddlParentAct.SelectedIndex <> 0 Then
                    If Me.txtDevTime.Text.Trim = "" Then
                        Dr("nDeviationTime") = Nothing
                    Else
                        Dr("nDeviationTime") = Format(Val(Me.txtDevTime.Text.Trim()), "00.00")
                    End If
                End If
                Dr("iTranNo") = 1
                If Me.chkMultiDose.Checked = False Then
                    Dr("iDayNo") = 1
                    Dr("iDoseNo") = 1
                Else
                    Dr("iDayNo") = 0
                    Dr("iDoseNo") = 0
                End If
                Dr("vDomainName") = DBNull.Value
                DtWorkSpaceNodeDetail.Rows.Add(Dr)
                DtWorkSpaceNodeDetail.AcceptChanges()
                INodeId += 1
            Next irep

            If ddlParentAct.SelectedIndex = 0 Then
                Me.ViewState(VS_Dt_GVParent) = DtWorkSpaceNodeDetail
                Me.GV_ParentActivity.DataSource = DtWorkSpaceNodeDetail
                Me.GV_ParentActivity.DataBind()
                GV_ParentActivity.Columns(GVP_Edit).Visible = False
                GV_ParentActivity.Columns(GVP_Rearrange).Visible = False
                GV_ParentActivity.Columns(GVP_Delete).Visible = True
            Else
                Me.ViewState(VS_Dt_GVChild) = DtWorkSpaceNodeDetail
                Me.GV_ChildActivity.DataSource = DtWorkSpaceNodeDetail
                Me.GV_ChildActivity.DataBind()
                GV_ChildActivity.Columns(GVC_Edit).Visible = False
                GV_ChildActivity.Columns(GVC_Rearrange).Visible = False
                GV_ChildActivity.Columns(GVC_delete).Visible = True
                Me.divParent.Style.Add("Display", "none")
                Me.divChild.Style.Add("Display", "")
                Me.btnSchedule.Style.Add("Display", "none")
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "ApplyDtAddActivity", "fnApplyDtAddParentActivity();", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "ApplyDtAddActivity", "fnApplyDtAddChildActivity();", True)
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....btnAdd_Click")
        End Try
    End Sub

    Protected Sub btnSave_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim DtWorkSpaceNodeDetail As New DataTable
        Dim DtWorkSpaceNodeDetail_Edit As New DataTable
        Dim choice As Integer
        Dim drSelect As DataRow()
        Dim inodeid As String = String.Empty
        Dim wstr As String = String.Empty
        Dim ds_Parent As New DataSet
        Dim ds_RefAct As New DataSet
        Dim ds_projectreviewer As New DataSet
        Dim dt_projectreviewer As New DataTable
        Dim dv_reviewer As DataView
        Dim DtWorkSpaceNodeDetail_Edit_New As New DataTable
        Dim Nodeid As Integer = 0
        Dim ParentNodeId As Integer
        Dim strindependentprofile As String = ""
        Dim dr As DataRow
        Try
            choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            If rbtSelection.SelectedValue.Trim = "DefineWorkflow" Then
                If Me.ViewState(Vs_AllowRemarks) = "Y" Then
                    MPERemarks.Show()
                    Me.txtRemarks.Text = ""
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "CheckBoxChecked", "CheckBoxChecked();", True)

                    Exit Sub
                End If

                wstr = "vParentWorkspaceid = '" + Me.HProjectId.Value.Trim() + "'and iActualWorkflowStageId <> 5 and cstatusindi <> 'D'"
                If Not objhelp.GetProjectReviewerMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_projectreviewer, eStr_Retu) Then
                    Throw New Exception(eStr_Retu)
                End If
                dt_projectreviewer = ds_projectreviewer.Tables(0).Clone()
                UPGrid.Update()
                Dim j As Integer = -1
                For Each row As GridViewRow In gvreview.Rows
                    dv_reviewer = ds_projectreviewer.Tables(0).Copy().DefaultView
                    Dim lblactualreviewer As Label = CType(row.FindControl("lbliActualWorkflowstageid"), Label)
                    dv_reviewer.RowFilter = "iActualWorkflowStageId = " + Convert.ToString(lblactualreviewer.Text.Trim)
                    Dim chkauthentication As CheckBox = CType(row.FindControl("chkauthentication"), CheckBox)
                    Dim ddlprofile As DropDownList = CType(row.FindControl("ddlgvprofile"), DropDownList)
                    Dim lblReviewedworkflowstageid As TextBox = CType(row.FindControl("lbliReviewWorkflowStageId"), TextBox)
                    For Each rows As DataRow In dv_reviewer.ToTable.Rows
                        j = j + 1
                        dr = dt_projectreviewer.NewRow()
                        dr("vParentWorkspaceid") = rows("vParentWorkspaceid")
                        dr("vReviewerlevel") = rows("vReviewerlevel")
                        dr("iActualWorkflowStageId") = rows("iActualWorkflowStageId")
                        dr("iReviewWorkflowStageId") = lblReviewedworkflowstageid.Text.ToString.Trim
                        dr("vUserTypeCode") = ddlprofile.SelectedValue.Trim()
                        dr("vRemarks") = Convert.ToString(txtRemarks.Text)
                        dr("cAuthenticationDialog") = "N"
                        If ddlprofile.SelectedValue.Trim() = "0000" Then
                            dr("cAuthenticationDialog") = "N"
                        Else
                            Dim var = hdnCheckBox.Value.Split(",")
                            Try
                                dr("cAuthenticationDialog") = var(j)
                            Catch ex As Exception
                                dr("cAuthenticationDialog") = IIf(chkauthentication.Checked = True, "Y", "N")
                            End Try

                        End If
                        dr("vStatus") = rows("vStatus")
                        dr("itranno") = 0
                        dr("iModifyBy") = Convert.ToString(Me.Session(S_UserID))
                        dr("cStatusindi") = "E"
                        dr("vColorCodeForDynamic") = rows("vColorCodeForDynamic")
                        dt_projectreviewer.Rows.Add(dr)
                        dt_projectreviewer.AcceptChanges()
                    Next
                Next
                dv_reviewer = dt_projectreviewer.Copy().DefaultView
                dv_reviewer.RowFilter = "iReviewWorkflowStageId <> 0"

                For i As Integer = 0 To dt_projectreviewer.Rows.Count - 1
                    If dv_reviewer.ToTable.Rows.Count = 1 Then
                        If dt_projectreviewer.Rows(i)("iActualWorkflowStageId") = "10" Then
                            dt_projectreviewer.Rows(i)("vStatus") = "L"
                        Else
                            dt_projectreviewer.Rows(i)("vStatus") = ""
                        End If
                    ElseIf dv_reviewer.ToTable.Rows.Count = 2 Then
                        If dt_projectreviewer.Rows(i)("iActualWorkflowStageId") = "10" Then
                            dt_projectreviewer.Rows(i)("vStatus") = "SRP"
                        ElseIf dt_projectreviewer.Rows(i)("iActualWorkflowStageId") = "20" Then
                            dt_projectreviewer.Rows(i)("vStatus") = "L"
                        Else
                            dt_projectreviewer.Rows(i)("vStatus") = ""
                        End If
                    ElseIf dv_reviewer.ToTable.Rows.Count = 3 Then
                        If dt_projectreviewer.Rows(i)("iActualWorkflowStageId") = "10" Then
                            dt_projectreviewer.Rows(i)("vStatus") = "SRP"
                        ElseIf dt_projectreviewer.Rows(i)("iActualWorkflowStageId") = "20" Then
                            dt_projectreviewer.Rows(i)("vStatus") = "FNLRP"
                        ElseIf dt_projectreviewer.Rows(i)("iActualWorkflowStageId") = "30" Then
                            dt_projectreviewer.Rows(i)("vStatus") = "L"
                        End If
                    End If
                    dt_projectreviewer.AcceptChanges()
                Next

                wstr = "vParentWorkspaceid = '" + Me.HProjectId.Value.Trim() + "'and iActualWorkflowStageId = 5 and cstatusindi <> 'D'"
                If Not objhelp.GetProjectReviewerMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_projectreviewer, eStr_Retu) Then
                    Throw New Exception(eStr_Retu)
                End If
                strindependentprofile = Me.hindependentprofile.Value.Trim()
                strindependentprofile = strindependentprofile.Replace("'", "")

                If ds_projectreviewer.Tables(0).Rows(0)("vUserTypeCode").ToString.Trim() <> strindependentprofile.Trim() Then
                    For i As Integer = 0 To ds_projectreviewer.Tables(0).Rows.Count - 1
                        dr = dt_projectreviewer.NewRow()
                        dr("vParentWorkspaceid") = ds_projectreviewer.Tables(0).Rows(i)("vParentWorkspaceid")
                        dr("vReviewerlevel") = ds_projectreviewer.Tables(0).Rows(i)("vReviewerlevel")
                        dr("iActualWorkflowStageId") = ds_projectreviewer.Tables(0).Rows(i)("iActualWorkflowStageId")
                        dr("iReviewWorkflowStageId") = ds_projectreviewer.Tables(0).Rows(i)("iReviewWorkflowStageId")
                        dr("vUserTypeCode") = strindependentprofile.Trim()
                        dr("cAuthenticationDialog") = ds_projectreviewer.Tables(0).Rows(i)("cAuthenticationDialog")
                        dr("vStatus") = ds_projectreviewer.Tables(0).Rows(i)("vStatus")
                        dr("itranno") = 0
                        dr("iModifyBy") = Convert.ToString(Me.Session(S_UserID))
                        dr("cStatusindi") = "E"
                        dr("vRemarks") = Convert.ToString(txtRemarks.Text)
                        dt_projectreviewer.Rows.Add(dr)
                        dt_projectreviewer.AcceptChanges()
                    Next
                End If

                ds_projectreviewer = New DataSet
                ds_projectreviewer.Tables.Add(dt_projectreviewer)

                If Not objLambda.Save_ProjectReviewerMst(choice, ds_projectreviewer, Convert.ToString(Me.Session(S_UserID)), eStr_Retu) Then
                    Throw New Exception(eStr_Retu)
                End If

                If Not Me.FillReviewerLevel() Then
                    Exit Sub
                End If
            Else
                ParentNodeId = IIf(Me.ddlParentAct.SelectedIndex = 0, 1, Me.ddlParentAct.SelectedValue.ToString).ToString()
                If ((Me.ddlParentAct.SelectedIndex = 0 AndAlso GV_ParentActivity.Columns(GVP_Edit).Visible = True) Or (Me.ddlParentAct.SelectedIndex <> 0 AndAlso GV_ChildActivity.Columns(GVC_Edit).Visible = True)) Then
                    choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                End If

                If (choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add) Then
                    DtWorkSpaceNodeDetail = IIf(Me.ddlParentAct.SelectedIndex = 0, Me.ViewState(VS_Dt_GVParent), Me.ViewState(VS_Dt_GVChild))
                    DtWorkSpaceNodeDetail.DefaultView.RowFilter = "cStatusIndi=" + "'N'"
                    DtWorkSpaceNodeDetail_Edit = DtWorkSpaceNodeDetail.Clone
                    If Me.ddlParentAct.SelectedIndex = 0 Then
                        For Each row As GridViewRow In GV_ParentActivity.Rows

                            If row.Cells(GVP_Status).Text = "N" Then
                                drSelect = DtWorkSpaceNodeDetail.DefaultView.ToTable.Select("iNodeID= " + row.Cells(GVP_iNodeId).Text.ToString())
                                DtWorkSpaceNodeDetail_Edit.ImportRow(drSelect(0))
                                DtWorkSpaceNodeDetail_Edit.AcceptChanges()
                                drSelect = DtWorkSpaceNodeDetail_Edit.Select("iNodeID= " + row.Cells(GVP_iNodeId).Text.ToString())
                                drSelect(0)("vNodeDisplayName") = CType(row.FindControl("txtDesc"), TextBox).Text.Trim
                                drSelect(0)("iDayNo") = IIf((CType(row.FindControl("txtDay"), TextBox).Text) = "", 1, CType(row.FindControl("txtDay"), TextBox).Text.Trim)
                                drSelect(0)("iDoseNo") = IIf((CType(row.FindControl("txtDose"), TextBox).Text) = "", 1, CType(row.FindControl("txtDose"), TextBox).Text.Trim)
                                drSelect(0)("cISPredose") = IIf((CType(row.FindControl("chkIsPredose"), HtmlInputCheckBox).Checked) = True, "Y", "N")
                                drSelect(0)("vDomainName") = CType(row.FindControl("txtDomainName"), TextBox).Text.Trim
                                DtWorkSpaceNodeDetail_Edit.AcceptChanges()
                            End If
                        Next

                        If chkMultiDose.Checked = False And Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                            If Me.ddlPeriod.SelectedValue.ToString.ToUpper = "ALL" Then
                                DtWorkSpaceNodeDetail_Edit_New = DtWorkSpaceNodeDetail_Edit.Clone()
                                For TotalAct As Integer = 0 To DtWorkSpaceNodeDetail_Edit.Rows.Count - 1
                                    For iPeriod As Integer = 1 To Me.ddlPeriod.Items.Count - 1
                                        Nodeid += 1
                                        DtWorkSpaceNodeDetail_Edit_New.ImportRow(DtWorkSpaceNodeDetail_Edit.Rows(TotalAct))
                                        DtWorkSpaceNodeDetail_Edit_New.AcceptChanges()
                                        DtWorkSpaceNodeDetail_Edit_New.Rows(DtWorkSpaceNodeDetail_Edit_New.Rows.Count - 1)("iPeriod") = iPeriod.ToString
                                        DtWorkSpaceNodeDetail_Edit_New.Rows(DtWorkSpaceNodeDetail_Edit_New.Rows.Count - 1)("vNodeDisplayName") = DtWorkSpaceNodeDetail_Edit_New.Rows(DtWorkSpaceNodeDetail_Edit_New.Rows.Count - 1)("vNodeDisplayName").ToString + " (Period - " + iPeriod.ToString + " )"
                                        DtWorkSpaceNodeDetail_Edit_New.Rows(DtWorkSpaceNodeDetail_Edit_New.Rows.Count - 1)("iNodeId") = Nodeid.ToString
                                        DtWorkSpaceNodeDetail_Edit_New.Rows(DtWorkSpaceNodeDetail_Edit_New.Rows.Count - 1)("iParentNodeId") = 1
                                        DtWorkSpaceNodeDetail_Edit_New.Rows(DtWorkSpaceNodeDetail_Edit_New.Rows.Count - 1)("iRefNodeId") = 1
                                    Next
                                Next
                                DtWorkSpaceNodeDetail_Edit.Rows.Clear()
                                DtWorkSpaceNodeDetail_Edit = DtWorkSpaceNodeDetail_Edit_New.Copy
                                DtWorkSpaceNodeDetail_Edit.AcceptChanges()
                            End If
                            ParentNodeId = 1
                        End If

                    Else
                        For Each row As GridViewRow In GV_ChildActivity.Rows
                            If row.Cells(GVC_Status).Text = "N" Then
                                drSelect = DtWorkSpaceNodeDetail.Select("iNodeID= " + row.Cells(GVC_iNodeId).Text.ToString())
                                DtWorkSpaceNodeDetail_Edit.ImportRow(drSelect(0))
                                DtWorkSpaceNodeDetail_Edit.AcceptChanges()
                                drSelect = DtWorkSpaceNodeDetail_Edit.Select("iNodeID= " + row.Cells(GVC_iNodeId).Text.ToString())
                                drSelect(0)("vNodeDisplayName") = CType(row.FindControl("txtDesc"), TextBox).Text.Trim
                                drSelect(0)("nRefTime") = IIf(Me.Session(S_ScopeNo) <> Scope_ClinicalTrial, CType(row.FindControl("txtRefTime"), TextBox).Text.Trim, DBNull.Value)
                                drSelect(0)("nDeviationTime") = IIf(Me.Session(S_ScopeNo) <> Scope_ClinicalTrial, CType(row.FindControl("txtDevTime"), TextBox).Text.Trim, DBNull.Value)
                                drSelect(0)("iDayNo") = IIf((CType(row.FindControl("txtDay"), TextBox).Text) = "", 1, CType(row.FindControl("txtDay"), TextBox).Text.Trim)
                                drSelect(0)("iDoseNo") = IIf((CType(row.FindControl("txtDose"), TextBox).Text) = "", 1, CType(row.FindControl("txtDose"), TextBox).Text.Trim)
                                drSelect(0)("cISPredose") = IIf((CType(row.FindControl("chkIsPredose"), HtmlInputCheckBox).Checked) = True, "Y", "N")
                                drSelect(0)("vDomainName") = CType(row.FindControl("txtDomainName"), TextBox).Text.Trim
                                DtWorkSpaceNodeDetail_Edit.AcceptChanges()
                            End If
                        Next
                        If chkMultiDose.Checked = False And Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then

                            If Me.ddlPeriod.SelectedValue.ToString.ToUpper = "ALL" Then
                                wstr = " vWorkspaceid = '" + Me.HProjectId.Value.Trim() + "' and vActivityId = '" + Me.ddlParentAct.SelectedValue.ToString + "' and cstatusindi <> 'D'"

                                If Not objhelp.getWorkSpaceNodeDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Parent, eStr_Retu) Then
                                    objCommon.ShowAlert("Error occurred while getting data from workspacenodedetail", Me.Page)
                                    Exit Sub
                                End If

                                If Me.ddlRefAct.SelectedIndex <> 0 Then
                                    wstr = ""
                                    wstr = " vWorkspaceid = '" + Me.HProjectId.Value.Trim() + "' and vActivityId = '" + Me.ddlRefAct.SelectedValue.ToString + "' and cstatusindi <> 'D'"

                                    If Not objhelp.getWorkSpaceNodeDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_RefAct, eStr_Retu) Then
                                        objCommon.ShowAlert("Error occurred while getting data from workspacenodedetail", Me.Page)
                                        Exit Sub
                                    End If
                                End If

                                DtWorkSpaceNodeDetail_Edit_New = DtWorkSpaceNodeDetail_Edit.Clone()
                                For TotalAct As Integer = 0 To DtWorkSpaceNodeDetail_Edit.Rows.Count - 1
                                    For iPeriod As Integer = 0 To ds_Parent.Tables(0).Rows.Count - 1
                                        Nodeid += 1
                                        DtWorkSpaceNodeDetail_Edit_New.ImportRow(DtWorkSpaceNodeDetail_Edit.Rows(TotalAct))
                                        DtWorkSpaceNodeDetail_Edit_New.AcceptChanges()
                                        DtWorkSpaceNodeDetail_Edit_New.Rows(DtWorkSpaceNodeDetail_Edit_New.Rows.Count - 1)("iPeriod") = ds_Parent.Tables(0).Rows(iPeriod)("iPeriod").ToString
                                        DtWorkSpaceNodeDetail_Edit_New.Rows(DtWorkSpaceNodeDetail_Edit_New.Rows.Count - 1)("iNodeId") = Nodeid.ToString
                                        DtWorkSpaceNodeDetail_Edit_New.Rows(DtWorkSpaceNodeDetail_Edit_New.Rows.Count - 1)("iParentNodeId") = IIf(Me.ddlParentAct.SelectedIndex = 0, 1, ds_Parent.Tables(0).Rows(iPeriod)("iNodeId").ToString)
                                        If Me.ddlRefAct.SelectedIndex <> 0 Then
                                            ds_RefAct.Tables(0).DefaultView.RowFilter = "iPeriod = " + ds_Parent.Tables(0).Rows(iPeriod)("iPeriod").ToString
                                            DtWorkSpaceNodeDetail_Edit_New.Rows(DtWorkSpaceNodeDetail_Edit_New.Rows.Count - 1)("iRefNodeId") = ds_RefAct.Tables(0).DefaultView.ToTable.Rows(0)("iNodeId").ToString()
                                        Else
                                            DtWorkSpaceNodeDetail_Edit_New.Rows(DtWorkSpaceNodeDetail_Edit_New.Rows.Count - 1)("iRefNodeId") = ds_Parent.Tables(0).Rows(iPeriod)("iNodeId").ToString
                                        End If
                                    Next
                                Next
                                DtWorkSpaceNodeDetail_Edit.Rows.Clear()
                                DtWorkSpaceNodeDetail_Edit = DtWorkSpaceNodeDetail_Edit_New.Copy
                                DtWorkSpaceNodeDetail_Edit.AcceptChanges()
                                ParentNodeId = -1
                            Else
                                wstr = " vWorkspaceid = '" + Me.HProjectId.Value.Trim() + "' and vActivityId = '" + Me.ddlParentAct.SelectedValue.ToString + "' and cstatusindi <> 'D' and iPeriod = " + Me.ddlPeriod.SelectedValue.ToString
                                If Not objhelp.getWorkSpaceNodeDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Parent, eStr_Retu) Then
                                    objCommon.ShowAlert("Error occurred while getting data from workspacenodedetail", Me.Page)
                                    Exit Sub
                                End If
                                ParentNodeId = ds_Parent.Tables(0).Rows(0)("iNodeId").ToString
                                DtWorkSpaceNodeDetail_Edit.Columns("iParentNodeId").Expression = ds_Parent.Tables(0).Rows(0)("iNodeId").ToString
                                DtWorkSpaceNodeDetail_Edit.Columns("iRefNodeId").Expression = ds_Parent.Tables(0).Rows(0)("iNodeId").ToString

                                If Me.ddlRefAct.SelectedIndex > 0 Then
                                    wstr = " vWorkspaceid = '" + Me.HProjectId.Value.Trim() + "' and vActivityId = '" + Me.ddlRefAct.SelectedValue.ToString + "' and cstatusindi <> 'D' and iPeriod = " + Me.ddlPeriod.SelectedValue.ToString

                                    If Not objhelp.getWorkSpaceNodeDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Parent, eStr_Retu) Then
                                        objCommon.ShowAlert("Error occurred while getting data from workspacenodedetail", Me.Page)
                                        Exit Sub
                                    End If
                                    DtWorkSpaceNodeDetail_Edit.Columns("iRefNodeId").Expression = ds_Parent.Tables(0).Rows(0)("iNodeId").ToString
                                End If
                            End If
                        End If
                    End If

                    If Not objhelp.ProcedureExecute_WorkspaceNodeDetail("Insert_Workspacenodedetail_New", DtWorkSpaceNodeDetail_Edit, Me.HProjectId.Value.ToString.Trim(), ParentNodeId, choice.ToString) Then
                        Me.objCommon.ShowAlert("ERROR WHILE SAVING VALUES IN WORKSPACENODEDETAIL ... " + eStr_Retu.ToString(), Me.Page)
                        Exit Sub
                    End If
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ActivitySaved", "$(document).ready(function(){pageLoad(); msgalert('Activity Saved Successfully')});", True)
                Else
                    If Me.ddlParentAct.SelectedIndex = 0 Then
                        DtWorkSpaceNodeDetail = Me.ViewState(VS_Dt_GVParent)
                        DtWorkSpaceNodeDetail_Edit = DtWorkSpaceNodeDetail.Clone
                        For Each row As GridViewRow In GV_ParentActivity.Rows

                            If CType(row.FindControl("chkEdit"), HtmlInputCheckBox).Checked Then
                                drSelect = DtWorkSpaceNodeDetail.Select("iNodeID= " + row.Cells(GVP_iNodeId).Text.ToString())
                                DtWorkSpaceNodeDetail_Edit.ImportRow(drSelect(0))
                                DtWorkSpaceNodeDetail_Edit.AcceptChanges()
                                drSelect = DtWorkSpaceNodeDetail_Edit.Select("iNodeID= " + row.Cells(GVP_iNodeId).Text.ToString())
                                drSelect(0)("vNodeDisplayName") = CType(row.FindControl("txtDesc"), TextBox).Text.Trim
                                drSelect(0)("iDayNo") = IIf((CType(row.FindControl("txtDay"), TextBox).Text) = "", 1, CType(row.FindControl("txtDay"), TextBox).Text.Trim)
                                drSelect(0)("iDoseNo") = IIf((CType(row.FindControl("txtDose"), TextBox).Text) = "", 1, CType(row.FindControl("txtDose"), TextBox).Text.Trim)
                                drSelect(0)("cISPredose") = IIf((CType(row.FindControl("chkIsPredose"), HtmlInputCheckBox).Checked) = True, "Y", "N")
                                drSelect(0)("vDomainName") = CType(row.FindControl("txtDomainName"), TextBox).Text.Trim
                                DtWorkSpaceNodeDetail_Edit.AcceptChanges()
                            End If
                        Next
                    Else
                        DtWorkSpaceNodeDetail = Me.ViewState(VS_Dt_GVChild)
                        DtWorkSpaceNodeDetail_Edit = DtWorkSpaceNodeDetail.Clone
                        For Each row As GridViewRow In GV_ChildActivity.Rows
                            If CType(row.FindControl("chkChildEdit"), HtmlInputCheckBox).Checked Then
                                drSelect = DtWorkSpaceNodeDetail.Select("iNodeID= " + row.Cells(GVC_iNodeId).Text.ToString())
                                DtWorkSpaceNodeDetail_Edit.ImportRow(drSelect(0))
                                DtWorkSpaceNodeDetail_Edit.AcceptChanges()
                                drSelect = DtWorkSpaceNodeDetail_Edit.Select("iNodeID= " + row.Cells(GVC_iNodeId).Text.ToString())
                                drSelect(0)("vNodeDisplayName") = CType(row.FindControl("txtDesc"), TextBox).Text.Trim
                                drSelect(0)("nRefTime") = IIf(Me.Session(S_ScopeNo) <> Scope_ClinicalTrial, CType(row.FindControl("txtRefTime"), TextBox).Text.Trim, DBNull.Value)
                                drSelect(0)("nDeviationTime") = IIf(Me.Session(S_ScopeNo) <> Scope_ClinicalTrial, CType(row.FindControl("txtDevTime"), TextBox).Text.Trim, DBNull.Value)
                                drSelect(0)("iDayNo") = IIf((CType(row.FindControl("txtDay"), TextBox).Text) = "", 1, CType(row.FindControl("txtDay"), TextBox).Text.Trim)
                                drSelect(0)("iDoseNo") = IIf((CType(row.FindControl("txtDose"), TextBox).Text) = "", 1, CType(row.FindControl("txtDose"), TextBox).Text.Trim)
                                drSelect(0)("cISPredose") = IIf((CType(row.FindControl("chkIsPredose"), HtmlInputCheckBox).Checked) = True, "Y", "N")
                                drSelect(0)("vDomainName") = CType(row.FindControl("txtDomainName"), TextBox).Text.Trim
                                DtWorkSpaceNodeDetail_Edit.AcceptChanges()
                            End If
                        Next

                        If chkMultiDose.Checked = False And Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                            If Me.ddlPeriod.SelectedValue.ToString.ToUpper = "ALL" Then
                                DtWorkSpaceNodeDetail_Edit_New = DtWorkSpaceNodeDetail_Edit.Clone
                                For TotalAct As Integer = 0 To DtWorkSpaceNodeDetail_Edit.Rows.Count - 1
                                    wstr = " vWorkspaceid = '" + Me.HProjectId.Value.Trim() + "' and cstatusindi <> 'D' and vActivityId = '" + DtWorkSpaceNodeDetail_Edit.Rows(TotalAct)("vActivityId").ToString + "' and iNodeNo = " + DtWorkSpaceNodeDetail_Edit.Rows(TotalAct)("iNodeNo").ToString
                                    If Not objhelp.getWorkSpaceNodeDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Parent, eStr_Retu) Then
                                        objCommon.ShowAlert("Error occurred while getting data from workspacenodedetail", Me.Page)
                                        Exit Sub
                                    End If
                                    For Act As Integer = 0 To ds_Parent.Tables(0).Rows.Count - 1
                                        DtWorkSpaceNodeDetail_Edit_New.ImportRow(DtWorkSpaceNodeDetail_Edit.Rows(TotalAct))
                                        DtWorkSpaceNodeDetail_Edit_New.AcceptChanges()
                                        DtWorkSpaceNodeDetail_Edit_New.Rows(DtWorkSpaceNodeDetail_Edit_New.Rows.Count - 1)("iNodeId") = ds_Parent.Tables(0).Rows(Act)("iNodeId").ToString
                                        DtWorkSpaceNodeDetail_Edit_New.AcceptChanges()
                                    Next
                                Next
                                DtWorkSpaceNodeDetail_Edit.Rows.Clear()
                                DtWorkSpaceNodeDetail_Edit = DtWorkSpaceNodeDetail_Edit_New.Copy
                                DtWorkSpaceNodeDetail_Edit.AcceptChanges()
                            End If
                        End If
                    End If

                    If Not objhelp.ProcedureExecute_WorkspaceNodeDetail("Insert_Workspacenodedetail_new", DtWorkSpaceNodeDetail_Edit, Me.HProjectId.Value.ToString.Trim(), IIf(Me.ddlParentAct.SelectedIndex = 0, 1, Me.ddlParentAct.SelectedValue.ToString).ToString, choice.ToString) Then
                        Me.objCommon.ShowAlert("ERROR WHILE SAVING VALUES IN WORKSPACENODEDETAIL ... " + eStr_Retu.ToString(), Me.Page)
                        Exit Sub
                    End If
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ActivityEdited", "$(document).ready(function(){pageLoad(); msgalert('Activity Edited Successfully')});", True)

                End If
                If Not FillGrid(sender, e) Then
                    Exit Sub
                End If

                If Not FillReferenceActicity() Then
                    Exit Sub
                End If

                If Not clearControl() Then
                    Exit Sub
                End If
            End If            
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....btnSave_Click")
        End Try
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim DtWorkSpaceNodeDetail As New DataTable
        Dim DtWorkSpaceNodeDetail_Edit As New DataTable
        Dim choice As Integer
        Dim drSelect As DataRow()
        Dim DtWorkSpaceNodeDetail_Edit_New As New DataTable
        Dim wstr As String = String.Empty
        Dim ds_Parent As New DataSet

        Try
            choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete

            If Me.ddlParentAct.SelectedIndex = 0 Then
                DtWorkSpaceNodeDetail = Me.ViewState(VS_Dt_GVParent)
                DtWorkSpaceNodeDetail_Edit = DtWorkSpaceNodeDetail.Clone
                For Each row As GridViewRow In GV_ParentActivity.Rows

                    If CType(row.FindControl("chkEdit"), HtmlInputCheckBox).Checked Then
                        drSelect = DtWorkSpaceNodeDetail.Select("iNodeID= " + row.Cells(GVP_iNodeId).Text.ToString())
                        DtWorkSpaceNodeDetail_Edit.ImportRow(drSelect(0))
                        DtWorkSpaceNodeDetail_Edit.AcceptChanges()
                    End If
                Next
            Else
                DtWorkSpaceNodeDetail = Me.ViewState(VS_Dt_GVChild)
                DtWorkSpaceNodeDetail_Edit = DtWorkSpaceNodeDetail.Clone
                For Each row As GridViewRow In GV_ChildActivity.Rows

                    If CType(row.FindControl("chkChildEdit"), HtmlInputCheckBox).Checked Then
                        drSelect = DtWorkSpaceNodeDetail.Select("iNodeID= " + row.Cells(GVC_iNodeId).Text.ToString())
                        DtWorkSpaceNodeDetail_Edit.ImportRow(drSelect(0))
                        DtWorkSpaceNodeDetail_Edit.AcceptChanges()
                    End If
                Next

                If chkMultiDose.Checked = False And Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                    If Me.ddlPeriod.SelectedValue.ToString.ToUpper = "ALL" Then
                        DtWorkSpaceNodeDetail_Edit_New = DtWorkSpaceNodeDetail_Edit.Clone
                        For TotalAct As Integer = 0 To DtWorkSpaceNodeDetail_Edit.Rows.Count - 1
                            wstr = " vWorkspaceid = '" + Me.HProjectId.Value.Trim() + "' and cstatusindi <> 'D' and vActivityId = '" + DtWorkSpaceNodeDetail_Edit.Rows(TotalAct)("vActivityId").ToString + "' and iNodeNo = " + DtWorkSpaceNodeDetail_Edit.Rows(TotalAct)("iNodeNo").ToString

                            If Not objhelp.getWorkSpaceNodeDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Parent, eStr_Retu) Then
                                objCommon.ShowAlert("Error occurred while getting data from workspacenodedetail", Me.Page)
                                Exit Sub
                            End If

                            For Act As Integer = 0 To ds_Parent.Tables(0).Rows.Count - 1
                                DtWorkSpaceNodeDetail_Edit_New.ImportRow(DtWorkSpaceNodeDetail_Edit.Rows(TotalAct))
                                DtWorkSpaceNodeDetail_Edit_New.AcceptChanges()
                                DtWorkSpaceNodeDetail_Edit_New.Rows(DtWorkSpaceNodeDetail_Edit_New.Rows.Count - 1)("iNodeId") = ds_Parent.Tables(0).Rows(Act)("iNodeId").ToString
                                DtWorkSpaceNodeDetail_Edit_New.AcceptChanges()
                            Next
                        Next
                        DtWorkSpaceNodeDetail_Edit.Rows.Clear()
                        DtWorkSpaceNodeDetail_Edit = DtWorkSpaceNodeDetail_Edit_New.Copy
                        DtWorkSpaceNodeDetail_Edit.AcceptChanges()
                    End If
                End If
            End If

            If Not objhelp.ProcedureExecute_WorkspaceNodeDetail("Insert_Workspacenodedetail_new", DtWorkSpaceNodeDetail_Edit, Me.HProjectId.Value.ToString.Trim(), IIf(Me.ddlParentAct.SelectedIndex = 0, 1, Me.ddlParentAct.SelectedValue.ToString).ToString, choice.ToString) Then
                Me.objCommon.ShowAlert("ERROR WHILE DELETING VALUES IN WORKSPACENODEDETAIL ... " + eStr_Retu.ToString(), Me.Page)
                Exit Sub
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ActivityDeleted", "$(document).ready(function(){pageLoad(); msgalert('Activity Deleted Successfully')});", True)

            If Not FillGrid(sender, e) Then
                Exit Sub
            End If

            If Not FillReferenceActicity() Then
                Exit Sub
            End If

            clearControl()

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....btnDelete_Click")
        End Try
    End Sub

    Protected Sub btnRearrange_Click(sender As Object, e As EventArgs) Handles btnRearrange.Click
        Dim DtWorkSpaceNodeDetail As New DataTable
        Dim wstr As String = String.Empty
        Dim ds_AllParentAct As New DataSet
        Dim cIsChanged As Boolean = False
        Try

            If Not SequenceActivity() Then
                Me.ShowErrorMessage("Error Whlie Rearranging", "")
                Exit Try
            End If
            DtWorkSpaceNodeDetail = IIf(Me.ddlParentAct.SelectedIndex = 0, CType(Me.ViewState(VS_Dt_GVParent), DataTable), CType(Me.ViewState(VS_Dt_GVChild), DataTable))

            If Not RearrangeAct(DtWorkSpaceNodeDetail) Then
                Me.ShowErrorMessage("Error Whlie Rearranging", "")
                Exit Try
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ActivityRearranged", "$(document).ready(function(){pageLoad(); msgalert('Activity Rearranged Successfully')});", True)

            If Not FillGrid(sender, e) Then
                Exit Sub
            End If

            If Not FillReferenceActicity() Then
                Exit Sub
            End If

            clearControl()

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....btnRearrange_Click")
        End Try
    End Sub

    Protected Sub btnSchedule_Click(sender As Object, e As EventArgs) Handles btnSchedule.Click
        Dim DtWorkSpaceNodeDetail As New DataTable
        Dim NodeNo As Integer = 1
        Try
            DtWorkSpaceNodeDetail = IIf(Me.ddlParentAct.SelectedIndex = 0, CType(Me.ViewState(VS_Dt_GVParent), DataTable), CType(Me.ViewState(VS_Dt_GVChild), DataTable))
            DtWorkSpaceNodeDetail.DefaultView.Sort = "nRefTime"
            DtWorkSpaceNodeDetail.DefaultView.ToTable()
            DtWorkSpaceNodeDetail = DtWorkSpaceNodeDetail.DefaultView.ToTable

            For Act As Integer = 0 To DtWorkSpaceNodeDetail.Rows.Count - 1
                DtWorkSpaceNodeDetail.Rows(Act)("iNodeNo") = NodeNo
                NodeNo += 1
            Next
            DtWorkSpaceNodeDetail.AcceptChanges()

            If Not RearrangeAct(DtWorkSpaceNodeDetail) Then
                Me.ShowErrorMessage("Error Whlie Rearranging.", "")
                Exit Try
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ActivityScheduled", "$(document).ready(function(){pageLoad(); msgalert('Activity Scheduled Successfully.')});", True)

            If Not FillGrid(sender, e) Then
                Exit Sub
            End If
            If Not FillReferenceActicity() Then
                Exit Sub
            End If
            clearControl()

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....btnSchedule_Click")
        End Try
    End Sub

#End Region

#Region "Fill Grid"

    Private Function FillGrid(sender As Object, e As EventArgs) As Boolean
        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim ds_WorkspaceNodeDetail As New DataSet
        Try
            If Me.ddlParentAct.SelectedIndex = 0 Then
                wStr = "vWorkSpaceId = '" & Me.HProjectId.Value.Trim() & "' And iParentNodeId = " & IIf(Me.ddlParentAct.SelectedIndex = 0, 1, Me.ddlParentAct.SelectedValue.ToString).ToString & " AND cStatusIndi<>'D'"
                If Me.ddlPeriod.SelectedValue.ToString.ToUpper <> "ALL" Then
                    If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Or Me.ddlParentAct.SelectedIndex <> 0 Then
                        wStr += " AND iPeriod = " + Me.ddlPeriod.SelectedValue.ToString
                    End If
                End If

                wStr += " Order by iNodeNo"
                If Not objhelp.GetViewWorkSpaceNodeDetail(wStr, ds_WorkspaceNodeDetail, eStr_Retu) Then
                    Me.objCommon.ShowAlert("Error while getting Information: " & eStr_Retu, Me.Page)
                    Exit Function
                End If

                ds_WorkspaceNodeDetail.Tables(0).AcceptChanges()
                ds_WorkspaceNodeDetail.Tables(0).Columns("cStatusIndi").Expression = "'E'"
                CType(Me.ViewState(VS_Dt_GVParent), DataTable).Rows.Clear()
                Me.ViewState(VS_Dt_GVParent) = ds_WorkspaceNodeDetail.Tables(0).DefaultView.ToTable
                Me.GV_ParentActivity.DataSource = ds_WorkspaceNodeDetail.Tables(0).DefaultView.ToTable
                Me.GV_ParentActivity.DataBind()
                GV_ParentActivity.Columns(GVP_Edit).Visible = True
                GV_ParentActivity.Columns(GVP_Rearrange).Visible = True
                GV_ParentActivity.Columns(GVP_Delete).Visible = False
            Else
                BtnSearch_Click(sender, e)
            End If

            Me.btnSave.Style.Add("Display", "none")

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "ApplyDtAddActivity", "fnApplyDtAddChildActivity();", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "ApplyDtAddActivity", "fnApplyDtAddParentActivity();", True)
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillGrid()")
            Return False
        End Try
    End Function

    Private Function FillGridAndActivity() As Boolean
        Dim Wstr As String = String.Empty
        Dim dt_WorkSpaceNodeDetail As New DataTable
        Dim ds_WorkspaceNodeDetail As DataSet = Nothing
        Dim dv_ParentGrid As New DataView

        Try
            GV_ChildActivity.DataSource = Nothing
            GV_ChildActivity.DataBind()
            GV_ParentActivity.DataSource = Nothing
            GV_ParentActivity.DataBind()
            Me.divParent.Style.Add("Display", "")
            Me.divChild.Style.Add("Display", "")

            If Not FillParentActicity() Then
                Exit Function
            End If

            Wstr = String.Empty
            Wstr = "vWorkSpaceId = '" & Me.HProjectId.Value.Trim() & "' And iParentNodeId = " & 1 & " AND cStatusIndi<>'D' "
            If Me.ddlPeriod.SelectedValue.ToString.ToUpper <> "ALL" Then

                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Or Me.ddlParentAct.SelectedIndex <> 0 Then

                    Wstr += " AND iPeriod = " + Me.ddlPeriod.SelectedValue.ToString
                End If
            End If

            Wstr += " Order by iNodeNo"
            If Not objhelp.GetViewWorkSpaceNodeDetail(Wstr, ds_WorkspaceNodeDetail, eStr_Retu) Then
                Me.objCommon.ShowAlert("Error while getting Information: " & eStr_Retu, Me.Page)
                Exit Function
            End If

            ds_WorkspaceNodeDetail.Tables(0).TableName = "ParentWorkSpaceNodeDetail"
            ds_WorkspaceNodeDetail.Tables(0).AcceptChanges()
            ds_WorkspaceNodeDetail.Tables(0).Columns("cStatusIndi").Expression = "'E'"
            dv_ParentGrid = ds_WorkspaceNodeDetail.Tables(0).DefaultView()
            Me.ViewState(VS_Dt_GVParent) = dv_ParentGrid.ToTable()
            Me.GV_ParentActivity.DataSource = dv_ParentGrid.ToTable()
            Me.GV_ParentActivity.DataBind()
            Me.GV_ParentActivity.Columns(GVP_Delete).Visible = False

            If dv_ParentGrid.ToTable().Rows.Count <= 0 Then
                Me.objCommon.ShowAlert("No Parent Activity Found! " & eStr_Retu, Me.Page)
            End If

            If Not FillReferenceActicity() Then
                Exit Function
            End If

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "Grid Events"

#Region "Parent Grid Events"

    Protected Sub GV_ParentActivity_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles GV_ParentActivity.RowCreated
        e.Row.Cells(GVP_Period).Style.Add("Display", "none")
        e.Row.Cells(GVP_SRNo).Style.Add("Display", "none")
        e.Row.Cells(GVP_iNodeId).Style.Add("Display", "none")
        e.Row.Cells(GVP_iNodeIndex).Style.Add("Display", "none")
        e.Row.Cells(GVP_Status).Style.Add("Display", "none")
        e.Row.Cells(GVP_ActivityId).Style.Add("Display", "none")
        e.Row.Cells(GVP_IsPreDose).Style.Add("Display", "none")
        e.Row.Cells(GVP_devTime).Style.Add("Display", "none")
        e.Row.Cells(GVP_RefTime).Style.Add("Display", "none")
        e.Row.Cells(GVP_Day).Style.Add("Display", "none")
        e.Row.Cells(GVP_Dose).Style.Add("Display", "none")

        If Me.chkMultiDose.Checked = True Then
            e.Row.Cells(GVP_Day).Style.Add("Display", "")
            e.Row.Cells(GVP_Dose).Style.Add("Display", "")
        End If

        If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
            e.Row.Cells(GVP_chkIsPreDose).Style.Add("Display", "none")
            e.Row.Cells(GVP_Barcode).Style.Add("Display", "none")
        End If

    End Sub

    Protected Sub GV_ParentActivity_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GV_ParentActivity.RowCommand
        Dim str As String
        Dim index As Integer
        Dim DtParentWorkSpaceNodeDetail As New DataTable

        If e.CommandName.ToUpper.Trim() = "DELETE" Then
            index = e.CommandArgument
            DtParentWorkSpaceNodeDetail = CType(Me.ViewState(VS_Dt_GVParent), DataTable)
            DtParentWorkSpaceNodeDetail.Rows(index).Delete()
            DtParentWorkSpaceNodeDetail.AcceptChanges()
            Me.ViewState(VS_Dt_GVParent) = DtParentWorkSpaceNodeDetail
            Me.GV_ParentActivity.DataSource = Nothing
            Me.GV_ParentActivity.DataSource = DtParentWorkSpaceNodeDetail
            Me.GV_ParentActivity.DataBind()
        End If

        If e.CommandName.ToUpper.Trim() = "PREVIEW" Then
            index = e.CommandArgument
            DtParentWorkSpaceNodeDetail = CType(Me.ViewState(VS_Dt_GVParent), DataTable)
            str = "frmPreviewattributesForm.aspx?WorkspaceId=" & Me.HProjectId.Value.Trim() & "&ActivityId=" & DtParentWorkSpaceNodeDetail.Rows(index).Item("vActivityId") & "&iNodeId=" & DtParentWorkSpaceNodeDetail.Rows(index).Item("iNodeId")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenWinodw", "OpenWindow('" + str + "');", True)
        End If
    End Sub

    Protected Sub GV_ParentActivity_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GV_ParentActivity.RowDataBound
        Dim Barcode As String = String.Empty

        If e.Row.RowType = DataControlRowType.DataRow Then

            e.Row.CssClass = "SaveParent"
            e.Row.Cells(GVP_SRNo).Text = e.Row.RowIndex + 1
            CType(e.Row.FindControl("imgbtnDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("imgbtnDelete"), ImageButton).CommandName = "DELETE"
            CType(e.Row.FindControl("imgBtnPreview"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("imgBtnPreview"), ImageButton).CommandName = "PREVIEW"
           
            If (CType(e.Row.FindControl("lblActivity"), Label).Text) = "1153" Or (CType(e.Row.FindControl("lblActivity"), Label).Text) = "2053" Or (CType(e.Row.FindControl("lblActivity"), Label).Text) = "2483" Then
                If GenerateBarcode(CType(e.Row.FindControl("txtDesc"), TextBox).Text, Barcode) Then
                    CType(e.Row.FindControl("lblBarcode"), Label).Text = Barcode
                End If
            End If

            If (e.Row.Cells(GVP_Status).Text) = "N" Then
                e.Row.FindControl("imgBtnPreview").Visible = False
                e.Row.FindControl("imgbtnDelete").Visible = True
                CType(e.Row.FindControl("imgbtnDelete"), ImageButton).OnClientClick = "return confirm('Are you sure you want to delete?');"
                CType(e.Row.FindControl("txtDesc"), TextBox).Enabled = True
                CType(e.Row.FindControl("chkIsPredose"), HtmlInputCheckBox).Disabled = False
                CType(e.Row.FindControl("txtDay"), TextBox).Enabled = True
                CType(e.Row.FindControl("txtDose"), TextBox).Enabled = True
                CType(e.Row.FindControl("txtDomainName"), TextBox).Enabled = True
            Else
                e.Row.FindControl("imgBtnPreview").Visible = True
                e.Row.FindControl("imgbtnDelete").Visible = False
                CType(e.Row.FindControl("chkIsPredose"), HtmlInputCheckBox).Disabled = True
            End If

            If e.Row.Cells(GVP_IsPreDose).Text = "Y" Then
                'CType(e.Row.FindControl("chkIsPredose"), HtmlInputCheckBox).Checked = True
                CType(e.Row.FindControl("chkIsPredose"), HtmlInputCheckBox).Attributes.Add("checked", True)
                CType(e.Row.FindControl("chkIsPredose"), HtmlInputCheckBox).Attributes.Add("onClick", "CheckUnCheck(this)")
            End If

            If Me.ddlParentAct.SelectedIndex = 0 AndAlso Me.Session(S_ScopeNo) <> Scope_ClinicalTrial AndAlso (CType(e.Row.FindControl("lblPeriod"), Label).Text) <> Me.ddlPeriod.SelectedValue AndAlso Me.chkMultiDose.Checked = True Then
                e.Row.Style.Add("Display", "none")
            End If

        End If
    End Sub

    Protected Sub GV_ParentActivity_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GV_ParentActivity.RowDeleting

    End Sub

#End Region

#Region "Child Grid Events"

    Protected Sub GV_ChildActivity_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GV_ChildActivity.RowCommand
        Dim index As Integer
        Dim str As String
        Dim DtWorkSpaceNodeDetail As New DataTable
        If e.CommandName.ToUpper.Trim() = "DELETE" Then
            index = e.CommandArgument
            DtWorkSpaceNodeDetail = CType(Me.ViewState(VS_Dt_GVChild), DataTable)
            DtWorkSpaceNodeDetail.Rows(index).Delete()
            DtWorkSpaceNodeDetail.AcceptChanges()
            Me.ViewState(VS_Dt_GVChild) = DtWorkSpaceNodeDetail
            Me.GV_ChildActivity.DataSource = Nothing
            Me.GV_ChildActivity.DataSource = DtWorkSpaceNodeDetail
            Me.GV_ChildActivity.DataBind()
        End If

        If e.CommandName.ToUpper.Trim() = "PREVIEW" Then
            index = e.CommandArgument
            DtWorkSpaceNodeDetail = CType(Me.ViewState(VS_Dt_GVChild), DataTable)
            str = "frmPreviewattributesForm.aspx?WorkspaceId=" & Me.HProjectId.Value.Trim() & "&ActivityId=" & DtWorkSpaceNodeDetail.Rows(index).Item("vActivityId") & "&iNodeId=" & DtWorkSpaceNodeDetail.Rows(index).Item("iNodeId")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenWinodw", "OpenWindow('" + str + "');", True)
        End If

    End Sub

    Protected Sub GV_ChildActivity_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles GV_ChildActivity.RowCreated
        e.Row.Cells(GVP_Period).Style.Add("Display", "none")
        e.Row.Cells(GVC_SRNo).Style.Add("Display", "none")
        e.Row.Cells(GVC_iNodeId).Style.Add("Display", "none")
        e.Row.Cells(GVC_iNodeIndex).Style.Add("Display", "none")
        e.Row.Cells(GVC_Status).Style.Add("Display", "none")
        e.Row.Cells(GVC_ActivityId).Style.Add("Display", "none")
        e.Row.Cells(GVC_IsPreDose).Style.Add("Display", "none")
        e.Row.Cells(GVC_Day).Style.Add("Display", "none")
        e.Row.Cells(GVC_Dose).Style.Add("Display", "none")

        If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
            e.Row.Cells(GVC_chkIsPreDose).Style.Add("Display", "none")
            e.Row.Cells(GVC_Barcode).Style.Add("Display", "none")
            e.Row.Cells(GVC_devTime).Style.Add("Display", "none")
            e.Row.Cells(GVC_RefTime).Style.Add("Display", "none")
        End If

    End Sub

    Protected Sub GV_ChildActivity_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GV_ChildActivity.RowDataBound
        Dim Barcode As String = String.Empty

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.CssClass = "SaveChild"
            e.Row.Cells(GVC_SRNo).Text = e.Row.RowIndex + 1
            CType(e.Row.FindControl("imgbtnDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("imgbtnDelete"), ImageButton).CommandName = "DELETE"
            CType(e.Row.FindControl("imgBtnPreview"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("imgBtnPreview"), ImageButton).CommandName = "PREVIEW"

            If (CType(e.Row.FindControl("lblActivity"), Label).Text) = "1153" Or (CType(e.Row.FindControl("lblActivity"), Label).Text) = "2053" Or (CType(e.Row.FindControl("lblActivity"), Label).Text) = "2483" Then
                If GenerateBarcode(CType(e.Row.FindControl("txtDesc"), TextBox).Text, Barcode) Then
                    CType(e.Row.FindControl("lblBarcode"), Label).Text = Barcode
                End If
            End If

            If (e.Row.Cells(GVC_Status).Text) = "N" Then
                e.Row.FindControl("imgbtnDelete").Visible = True
                e.Row.FindControl("imgBtnPreview").Visible = False
                CType(e.Row.FindControl("imgbtnDelete"), ImageButton).OnClientClick = "return confirm('Are you sure you want to delete?');"
                CType(e.Row.FindControl("txtDesc"), TextBox).Enabled = True
                CType(e.Row.FindControl("txtRefTime"), TextBox).Enabled = True
                CType(e.Row.FindControl("txtDevTime"), TextBox).Enabled = True
                CType(e.Row.FindControl("chkIsPredose"), HtmlInputCheckBox).Disabled = False
                CType(e.Row.FindControl("txtDomainName"), TextBox).Enabled = True
            Else
                e.Row.FindControl("imgbtnDelete").Visible = False
                e.Row.FindControl("imgBtnPreview").Visible = True
                CType(e.Row.FindControl("chkIsPredose"), HtmlInputCheckBox).Disabled = True
            End If

            If e.Row.Cells(GVC_IsPreDose).Text = "Y" Then
                CType(e.Row.FindControl("chkIsPredose"), HtmlInputCheckBox).Attributes.Add("checked", True)
                CType(e.Row.FindControl("chkIsPredose"), HtmlInputCheckBox).Attributes.Add("onClick", "CheckUnCheck(this)")
            End If
        End If
    End Sub

    Protected Sub GV_ChildActivity_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GV_ChildActivity.RowDeleting

    End Sub

#End Region

#End Region

#Region "Helper Function to validate timeInterval"

    Private Function TimeInterval(ByVal CrTime As Decimal) As String
        Dim tottime As Decimal
        Dim arrtime() As String
        Dim hh As Integer, MM As Integer
        Dim StrMM As String = String.Empty
        Dim Newtime As Decimal
        Try
            Newtime = CrTime
            If CrTime.ToString.Trim().IndexOf(".") > -1 Then
                arrtime = Split(CrTime.ToString.Trim(), ".")
                tottime = arrtime(0) * 60

                If arrtime(1).Length < 2 Then
                    arrtime(1) = arrtime(1).Trim() + "0"
                End If

                If arrtime(1) > 59 Then
                    tottime = tottime + arrtime(1)
                    hh = CType(Math.Floor(tottime / 60), Integer)
                    MM = CType(Math.Floor(tottime Mod 60), Integer)
                    StrMM = MM

                    If MM.ToString.Length < 2 Then
                        StrMM = "0" + MM.ToString.Trim()
                    End If
                Else
                    hh = arrtime(0)
                    StrMM = arrtime(1)
                End If

                If arrtime(0).ToString() <> "-0".ToString.Trim() Then
                    Newtime = hh.ToString.Trim() + "." + StrMM.Trim()
                Else
                    Newtime = "-0".ToString.Trim() + "." + StrMM.Trim()
                End If
            End If
            Return Newtime
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....TimeInterval()")
            Return ""
        End Try
    End Function

#End Region

#Region "Enable/Disable Control"

    Public Function DisableControlforCT() As Boolean
        Try
            Me.btnSchedule.Style.Add("Display", "none")
            Me.trPeriod.Style.Add("Display", "none")
            Me.trReferenceActivity.Style.Add("Display", "none")
            Me.trreferencetime.Style.Add("Display", "none")
            Me.trDeviationTime.Style.Add("Display", "none")
            Me.trmultidose.Style.Add("Display", "none")
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...DisableControlforCT()")
            Return False
        End Try
    End Function

    Public Function DisableControl() As Boolean
        Try
            Me.divActivityGroup.Style.Add("Display", "none")
            Me.GV_ParentActivity.Columns(GVP_Edit).Visible = False
            Me.GV_ParentActivity.Columns(GVP_Rearrange).Visible = False
            Me.btnSchedule.Style.Add("Display", "none")
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...DisableControl()")
            Return False
        End Try
    End Function

    Public Function EnableControl() As Boolean
        Try
            Me.divActivityGroup.Style.Add("Display", "")
            Me.GV_ParentActivity.Columns(GVP_Edit).Visible = True
            Me.GV_ParentActivity.Columns(GVP_Rearrange).Visible = True
            Me.btnSchedule.Style.Add("Display", "")
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...EnableControl()")
            Return False
        End Try
    End Function

#End Region

#Region "Other Functions"

    Private Function RearrangeAct(ByVal DtWorkSpaceNodeDetail As DataTable) As Boolean
        Dim DtWorkSpaceNodeDetail_Edit_New As New DataTable
        Dim wstr As String = String.Empty
        Dim ds_Parent As New DataSet
        Dim choice As Integer
        Try
            choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Rearrange
            If chkMultiDose.Checked = False And Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then

                If Me.ddlPeriod.SelectedValue.ToString.ToUpper = "ALL" Then
                    DtWorkSpaceNodeDetail_Edit_New = DtWorkSpaceNodeDetail.DefaultView.ToTable.Clone
                    For TotalAct As Integer = 0 To DtWorkSpaceNodeDetail.Rows.Count - 1
                        wstr = " vWorkspaceid = '" + Me.HProjectId.Value.Trim() + "' and cstatusindi <> 'D' and vActivityId = '" + DtWorkSpaceNodeDetail.Rows(TotalAct)("vActivityId").ToString + "' and iNodeId = " + DtWorkSpaceNodeDetail.Rows(TotalAct)("iNodeId").ToString

                        If Not objhelp.getWorkSpaceNodeDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Parent, eStr_Retu) Then
                            objCommon.ShowAlert("Error occurred while getting data from workspacenodedetail", Me.Page)
                            Exit Function
                        End If

                        wstr = " vWorkspaceid = '" + Me.HProjectId.Value.Trim() + "' and cstatusindi <> 'D' and vActivityId = '" + ds_Parent.Tables(0).Rows(0)("vActivityId").ToString + "' and iNodeNo = " + ds_Parent.Tables(0).Rows(0)("iNodeNo").ToString
                        If Not objhelp.GetViewWorkSpaceNodeDetail(wstr, ds_Parent, eStr_Retu) Then
                            objCommon.ShowAlert("Error occurred while getting data from workspacenodedetail", Me.Page)
                            Exit Function
                        End If
                        For Act As Integer = 0 To ds_Parent.Tables(0).Rows.Count - 1
                            DtWorkSpaceNodeDetail_Edit_New.ImportRow(ds_Parent.Tables(0).Rows(Act))
                            DtWorkSpaceNodeDetail_Edit_New.AcceptChanges()
                            DtWorkSpaceNodeDetail_Edit_New.Rows(DtWorkSpaceNodeDetail_Edit_New.Rows.Count - 1)("iNodeNo") = DtWorkSpaceNodeDetail.Rows(TotalAct)("iNodeNo").ToString
                            DtWorkSpaceNodeDetail_Edit_New.AcceptChanges()
                        Next
                    Next
                    DtWorkSpaceNodeDetail.Rows.Clear()
                    DtWorkSpaceNodeDetail = DtWorkSpaceNodeDetail_Edit_New.Copy
                    DtWorkSpaceNodeDetail.AcceptChanges()
                End If
            End If

            If Not objhelp.ProcedureExecute_WorkspaceNodeDetail("Insert_Workspacenodedetail_New", DtWorkSpaceNodeDetail.DefaultView.ToTable, Me.HProjectId.Value.ToString.Trim(), IIf(Me.ddlParentAct.SelectedIndex = 0, 1, Me.ddlParentAct.SelectedValue.ToString).ToString, choice.ToString) Then
                Me.objCommon.ShowAlert("ERROR WHILE RE-ARRANGING VALUES IN WORKSPACENODEDETAIL ... " + eStr_Retu.ToString(), Me.Page)
                Exit Function
            End If

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function GenerateBarcode(ByVal vNodeDisplayName As String, ByRef BarcodeName As String) As Boolean
        Dim TimePoint As String = String.Empty
        Dim Barcode As String = String.Empty
        Try
            TimePoint = vNodeDisplayName
            Dim vnodename As String = TimePoint.Split(" ")(0).ToUpper().Replace("HR", "")
            TimePoint = TimePoint.ToUpper().Replace(vnodename, "")

            If vnodename.ToLower() = "pre" Then
                Dim nodearray As String() = TimePoint.Split("-")

                If nodearray.Length > 1 Then

                    If Not nodearray(nodearray.Length - 1).ToString().Contains(")") Then
                        Barcode = vnodename + " DOSE " + nodearray(nodearray.Length - 1)
                    Else
                        Barcode = vnodename + " DOSE"
                    End If
                Else
                    Barcode = vnodename + " DOSE"
                End If
            Else
                Dim nodearray As String() = TimePoint.Split("-")

                If nodearray.Length > 1 Then

                    If Not nodearray(nodearray.Length - 1).ToString().Contains(")") Then
                        Barcode = vnodename + " HR " + nodearray(nodearray.Length - 1)
                    Else
                        Barcode = vnodename + " HR"
                    End If
                Else
                    Barcode = vnodename + " HR"
                End If
            End If
            BarcodeName = Barcode + "(xxxx)"

            If BarcodeName.Length > 18 Then
                BarcodeName = BarcodeName.Substring(0, 17)
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenerateBarcode()")
            Return False
        End Try

    End Function

    Private Function SequenceActivity() As Boolean
        Dim ds_Activity As New DataSet, ds_SequenceSample As New DataSet
        Dim dt_WorkspaceNodeDetail As New DataTable
        Dim drActivity As DataRow, drSeq As DataRow
        Dim wstr As String = String.Empty
        Dim ds_ParentAct As New DataSet
        Dim iNodeNo As Integer = 2
        Try
            If Me.HFGV_Sequence.Value.ToString() <> "" Then
                ds_Activity.Tables.Add(JsonConvert.DeserializeObject(Me.HFGV_Sequence.Value.Trim(), GetType(System.Data.DataTable)))
                dt_WorkspaceNodeDetail = IIf(Me.ddlParentAct.SelectedIndex = 0, CType(Me.ViewState(VS_Dt_GVParent), DataTable), CType(Me.ViewState(VS_Dt_GVChild), DataTable))
                If Not dt_WorkspaceNodeDetail Is Nothing AndAlso Not ds_Activity Is Nothing Then
                    For Each drActivity In dt_WorkspaceNodeDetail.Rows
                        For Each drSeq In ds_Activity.Tables(0).Rows
                            If drActivity("iNodeId") = drSeq("iNodeId") Then
                                drActivity("iNodeNo") = drSeq("iNodeNo")
                                Exit For
                            End If
                        Next
                        dt_WorkspaceNodeDetail.AcceptChanges()
                    Next
                    dt_WorkspaceNodeDetail.DefaultView.Sort = "iNodeNo ASC"

                End If
                
                If Me.ddlParentAct.SelectedIndex = 0 Then
                    Me.ViewState(VS_Dt_GVParent) = dt_WorkspaceNodeDetail.DefaultView.ToTable()
                Else
                    Me.ViewState(VS_Dt_GVChild) = dt_WorkspaceNodeDetail.DefaultView.ToTable()
                End If

            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...SequenceActivity()")
            Return False
        End Try
    End Function

#End Region

#Region "Reset Page"

    Public Function clearControl() As Boolean
        Try
            Me.ddlActivityGroup.SelectedIndex = 0
            Me.ddlActivity.Items.Clear()
            Me.txtDevTime.Text = 1
            Me.txtRepeatation.Text = 1
            Me.txtRefTimeInterval.Text = 1
            Me.ViewState(Vs_AllowRemarks) = "Y"
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...clearControl()")
            Return False
        End Try
    End Function

    Public Function resetpage() As Boolean
        Try
            Me.HFGV_NodeId.Value = ""
            Me.ViewState(Vs_AllowRemarks) = "Y"
            Me.HfPeriod.Value = ""
            Me.HProjectId.Value = ""
            Me.txtProject.Text = ""
            Me.hdataentry.Value = ""
            Me.hindependentprofile.Value = ""
            Me.ddlActivity.DataSource = Nothing
            Me.ddlActivity.DataBind()
            Me.ddlActivity.Items.Clear()
            Me.ViewState(Vs_AllowRemarks) = "N"
            Me.divreviewer.Style.Add("Display", "none")
            Me.Divlnk.Style.Add("Display", "none")
            Me.VersionDtl.Style.Add("Display", "none")
            Me.divParent.Style.Add("Display", "none")
            Me.divChild.Style.Add("Display", "none")
            Me.btnaudittrail.Style.Add("Display", "none")
            Me.ddlActivityGroup.DataSource = Nothing
            Me.ddlActivityGroup.DataBind()
            Me.ddlActivityGroup.ClearSelection()
            Me.ddlPeriod.Items.Clear()
            Me.ddlParentAct.Items.Clear()
            Me.ddlRefAct.Items.Clear()
            Me.ddlindependentprofile.Items.Clear()
            Me.GV_ParentActivity.DataSource = Nothing
            Me.GV_ParentActivity.DataBind()
            Me.GV_ChildActivity.DataSource = Nothing
            Me.GV_ChildActivity.DataBind()
            Me.gvreview.DataSource = Nothing
            Me.gvreview.DataBind()
            Me.ViewState.Clear()
            GenCall()
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...resetpage()")
            Return False
        End Try

    End Function

#End Region

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR>" + eStr
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR>" + eStr)
    End Sub

#End Region
#Region "FillReviewerLevel"

    Private Function FillReviewerLevel() As Boolean
        Dim ds_reviewer As New DataSet
        Dim ds_usertypeMst As New DataSet
        Dim ds_profile As New DataSet
        Dim ds_crfhdr As New DataSet
        Dim dr As DataRow
        Dim dv_reviewer As DataView
        Dim estr As String = ""
        Dim wstr As String = ""
        Dim strjson As String = ""

        Try
            Me.ViewState(Vs_AllowRemarks) = "Y"

            Me.divreviewer.Style.Add("Display", "none")
            'Me.btnSave.Style.Add("Display", "none")
            Me.btnaudittrail.Style.Add("Display", "none")
            Me.hdataentry.Value = ""
            Me.hindependentprofile.Value = ""
            wstr = "vParentWorkspaceId = '" + Me.HProjectId.Value.ToString.Trim() + "'"
            If Not Me.objhelp.GetCRFHdr(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_crfhdr, estr) Then
                Throw New Exception("Error while getting data from usertypemst " + estr.Trim)
            End If

            'commented by shivani pandya for Independent profile change druing project is frezz
            If ds_crfhdr.Tables(0).Rows.Count <> 0 Then
                Me.hdataentry.Value = "dataentry"             
            End If

            'wstr = " iWorkflowStageId > 9 and cStatusIndi <> 'D'"
            'If Not Me.objhelp.GetData("UserTypeMst", "vUserTypeCode,vUserTypeName,iWorkflowStageId", wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_usertypeMst, estr) Then
            '    Throw New Exception("Error while getting data from usertypemst " + estr.Trim)
            'End If

            If Not Me.objhelp.PROC_GETPROFILELIST(Me.Session(S_ScopeNo), "10,20,30", ds_usertypeMst, estr) Then
                Throw New Exception("Error while getting data from proc_getprofilelist " + estr.Trim)
            End If

            If ds_usertypeMst Is Nothing Or ds_usertypeMst.Tables.Count = 0 Or ds_usertypeMst.Tables(0).Rows.Count = 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Nodata", "$(document).ready(function(){pageLoad(); msgalert('No data found')});", True)
                Me.btnSave.Style.Add("Display", "none")
                Return False
            End If
            dr = ds_usertypeMst.Tables(0).NewRow()
            dr("vUserTypeName") = "Select Profile"
            dr("vUserTypeCode") = "0000"
            dr("iWorkflowStageId") = "0"
            ds_usertypeMst.Tables(0).Rows.InsertAt(dr, 0)
            ds_usertypeMst.AcceptChanges()
            Me.Session(VS_UserTypeMst) = ds_usertypeMst.Tables(0)

            If Not Me.GetJson(ds_usertypeMst.Tables(0), strjson) Then
                Return False
            End If

            Me.Hprofilelist.Value = strjson

            'wstr = " iWorkflowStageId = 5 and cStatusIndi <> 'D'"
            'If Not Me.objhelp.GetData("UserTypeMst", "vUserTypeCode,vUserTypeName,iWorkflowStageId", wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_profile, estr) Then
            '    Throw New Exception("Error while getting data from usertypemst " + estr.Trim)
            'End If

            If Not Me.objhelp.PROC_GETPROFILELIST(Me.Session(S_ScopeNo), "5", ds_profile, estr) Then
                Throw New Exception("Error while getting data from proc_getprofilelist " + estr.Trim)
            End If

            Me.ddlindependentprofile.DataValueField = "vUserTypeCode"
            Me.ddlindependentprofile.DataTextField = "vUserTypeName"
            Me.ddlindependentprofile.DataSource = ds_profile.Tables(0).Copy()
            Me.ddlindependentprofile.DataBind()

            If Not Me.objhelp.Proc_GetProjectReviewerLevel(Me.HProjectId.Value.ToString.Trim(), ds_reviewer, estr) Then
                Throw New Exception("Error while getting data from Proc_GetProjectReviewerLevel " + estr.Trim)
            End If

            If ds_reviewer Is Nothing Or ds_reviewer.Tables.Count = 0 Or ds_reviewer.Tables(0).Rows.Count = 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Nodata", "$(document).ready(function(){pageLoad(); msgalert('No data found')});", True)
                Me.btnSave.Style.Add("Display", "none")
                Return False
            End If

            dv_reviewer = ds_reviewer.Tables(0).Copy.DefaultView
            dv_reviewer.RowFilter = "iActualWorkflowStageId <> 5"
            Me.gvreview.DataSource = dv_reviewer.ToTable
            Me.gvreview.DataBind()
            dv_reviewer = ds_reviewer.Tables(0).Copy.DefaultView
            dv_reviewer.RowFilter = "iActualWorkflowStageId = 5"
            Me.hindependentprofile.Value = dv_reviewer.ToTable.Rows(0)("vUsertypeCode")

            Me.divreviewer.Style.Add("Display", "")
            Me.btnaudittrail.Style.Add("Display", "")
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......FillReviewerLevel")
            Return False
        End Try
    End Function

    Protected Sub gvreview_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvreview.RowDataBound
        Dim dt_profile As New DataTable
        Dim eStr As String = String.Empty

        Dim dv As DataView
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim profilecode As Label = CType(e.Row.FindControl("lblusertypecode"), Label)
                Dim ddlprofile As DropDownList = CType(e.Row.FindControl("ddlgvprofile"), DropDownList)
                Dim chkauthentication As CheckBox = CType(e.Row.FindControl("chkauthentication"), CheckBox)
                Dim chkEdit As CheckBox = CType(e.Row.FindControl("chkEdit"), CheckBox)
                'Dim txtremarks As TextBox = CType(e.Row.FindControl("txtremarks"), TextBox)

                dt_profile = CType(Me.Session(VS_UserTypeMst), DataTable)
                dv = dt_profile.Copy().DefaultView
                ddlprofile.DataSource = dt_profile
                ddlprofile.DataValueField = "vUserTypeCode"
                ddlprofile.DataTextField = "vUserTypeName"
                ddlprofile.DataBind()
                If profilecode.Text.Trim() <> "" Then
                    dv.RowFilter = " vUserTypeCode = '" + profilecode.Text + "'"
                    If dv.ToTable.Rows.Count > 0 Then
                        ddlprofile.SelectedValue = dv.ToTable.Copy().Rows(0)("vUserTypeCode")
                        ddlprofile.ToolTip = dv.ToTable.Copy().Rows(0)("vUserTypeName")
                        ddlprofile.Attributes.Add("tag", dv.ToTable.Copy().Rows(0)("vUserTypeCode"))
                    End If
                    If DirectCast(e.Row.FindControl("lblauthentication"), Label).Text.ToUpper() = "YES" Then
                        chkauthentication.Checked = True
                    End If
                Else
                    ddlprofile.SelectedValue = "0000"
                    ddlprofile.ToolTip = "Select Profile"
                    ddlprofile.Attributes.Add("tag", "0000")
                End If
                ddlprofile.Attributes.Add("disabled", "true")
                'txtremarks.Attributes.Add("disabled", "true")
                chkauthentication.Enabled = False

                If Me.ViewState(IsProjectFreeze) = True Or Me.hdataentry.Value <> "" Then
                    chkEdit.Enabled = False
                End If

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......gvreview_RowDataBound")
        End Try
    End Sub

    Protected Sub gvreview_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvreview.RowCreated
        Try
            e.Row.Cells(gvc_profilecode).Style.Add("Display", "none")
            e.Row.Cells(gvc_authentication).Style.Add("Display", "none")
            e.Row.Cells(gvc_ReviwWorkflowstageid).Style.Add("Display", "none")
            e.Row.Cells(gvc_ActualWorkflowstageid).Style.Add("Display", "none")
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......gvreview_RowDataBound")
        End Try
    End Sub

    Private Function GetJson(ByVal dt As DataTable, ByRef strjson As String) As Boolean
        Try
            Dim JSSerializer As New JavaScriptSerializer()
            Dim DtRows As New List(Of Dictionary(Of String, Object))()

            Dim newrow As Dictionary(Of String, Object) = Nothing

            For Each drow As DataRow In dt.Rows
                newrow = New Dictionary(Of String, Object)()
                For Each col As DataColumn In dt.Columns
                    newrow.Add(col.ColumnName.Trim(), drow(col))
                Next
                DtRows.Add(newrow)
            Next

            strjson = JSSerializer.Serialize(DtRows)
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......GetJson")
            strjson = ""
            Return False
        End Try
    End Function

#Region "Web Method"

    <System.Web.Services.WebMethod()> _
    Public Shared Function AddReviewer(ByVal strWorkspaceid As String, ByVal iActualWorkflowstageid As String, ByVal iReviewworkflowstageid As String, ByVal vusertypecode As String, ByVal cAuthenticationdialog As String, ByVal strUserId As String, ByVal strremarks As String) As String
        Dim objCommon As New clsCommon
        Dim ObjHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim ObjLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
        Dim ds_projectreviewer As New DataSet
        Dim dv_reviewer As DataView
        Dim wStr As String = ""
        Dim eStr As String = ""
        Try
            wStr = "vParentWorkspaceid = '" + strWorkspaceid + "' and cstatusindi <> 'D'"
            If Not ObjHelp.GetProjectReviewerMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_projectreviewer, eStr) Then
                Throw New Exception(eStr)
            End If
            dv_reviewer = ds_projectreviewer.Tables(0).Copy().DefaultView
            dv_reviewer.RowFilter = "iActualWorkflowStageId = " + iActualWorkflowstageid

            If iActualWorkflowstageid = "10" Then

            ElseIf iActualWorkflowstageid = "20" Then

            ElseIf iActualWorkflowstageid = "30" Then

            End If

            If ds_projectreviewer.Tables(0).Rows.Count <> 0 Then
                ds_projectreviewer.Tables(0).Rows(0)("iReviewworkflowstageid") = iReviewworkflowstageid
                ds_projectreviewer.Tables(0).Rows(0)("vUserTypeCode") = vusertypecode
                ds_projectreviewer.Tables(0).Rows(0)("cAuthenticationdialog") = cAuthenticationdialog
                If vusertypecode = "0000" Then
                    ds_projectreviewer.Tables(0).Rows(0)("vUserTypeCode") = ""
                    ds_projectreviewer.Tables(0).Rows(0)("iReviewworkflowstageid") = "0"
                    ds_projectreviewer.Tables(0).Rows(0)("cAuthenticationdialog") = "N"
                    ds_projectreviewer.Tables(0).Rows(0)("vStatus") = ""
                End If

                ds_projectreviewer.Tables(0).Rows(0)("vRemarks") = strremarks.ToString()
                ds_projectreviewer.Tables(0).Rows(0)("cStatusIndi") = "E"
                ds_projectreviewer.Tables(0).Rows(0)("iModifyBy") = strUserId
                ds_projectreviewer.AcceptChanges()

            End If

            If Not ObjLambda.Save_ProjectReviewerMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_projectreviewer, strUserId, eStr) Then
                Throw New Exception(eStr)
            End If

            Return "Success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

#End Region

    Protected Sub btnreviewer_Click(ByVal Sender As Object, ByVal e As System.EventArgs) Handles btnreviewer.Click
        Try
            If Not Me.FillReviewerLevel() Then
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......btnreviewer_Click")
        End Try
    End Sub

    Protected Sub btnSaveRemarks_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveRemarks.Click
        Try
            Me.ViewState(Vs_AllowRemarks) = "N"
            MPERemarks.Hide()
            btnSave_click(Me.btnSave, New EventArgs())
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......btnSaveRemarks_Click")
        End Try
    End Sub

    Protected Sub btnaudittrail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaudittrail.Click
        Dim ds_auditreviewer As New DataSet
        Dim estr As String = ""
        Dim dc_Audit As DataColumn
        Try
            If Not Me.objhelp.Proc_GetAuditProjectReviewerLevel(Me.HProjectId.Value.ToString.Trim(), ds_auditreviewer, estr) Then
                Throw New Exception("Error while getting data from Proc_GetProjectReviewerLevel " + estr.Trim)
            End If

            dc_Audit = New DataColumn("dModifyOn_IST", System.Type.GetType("System.String"))
            ds_auditreviewer.Tables(0).Columns.Add("dModifyOn_IST")
            ds_auditreviewer.AcceptChanges()
            For Each dr_Audit In ds_auditreviewer.Tables(0).Rows
                dr_Audit("dModifyOn_IST") = Convert.ToString(CDate(dr_Audit("dModifyOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
            Next
            ds_auditreviewer.AcceptChanges()
            GV_Audit.DataSource = ds_auditreviewer
            GV_Audit.DataBind()
            MPEaudit.Show()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......btnaudittrail_Click")
        End Try
    End Sub

#End Region

End Class

