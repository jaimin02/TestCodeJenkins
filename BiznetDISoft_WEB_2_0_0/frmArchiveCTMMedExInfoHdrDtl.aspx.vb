
Partial Class frmArchiveCTMMedExInfoHdrDtl
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()


    Private Const VS_dtMedEx_Fill As String = "dt_MedEx_Fill"
    Private Const VS_DtCRFHdr As String = "DtCRFHdr"
    Private Const VS_DtCRFDtl As String = "DtCRFDtl"
    Private Const VS_DtCRFSubDtl As String = "DtCRFSubDtl"
    'Private Const VS_Choice As String = "Choice"
    Private Const VS_DtDCF As String = "DtDCF"
    Private Const VS_DCFNo As String = "DCFNo"
    Private Const VS_DataStatus As String = "DataStatus"
    Private Const VS_ReviewFlag As String = "ReviewFlag"

    Private Const GVCDCF_nDCFNo As Integer = 0
    Private Const GVCDCF_nCRFDtlNo As Integer = 1
    Private Const GVCDCF_iSrNo As Integer = 2
    Private Const GVCDCF_vMedExCode As Integer = 3
    Private Const GVCDCF_cDCFType As Integer = 4
    Private Const GVCDCF_iDCFBy As Integer = 5
    Private Const GVCDCF_vCreatedBy As Integer = 6
    Private Const GVCDCF_dDCFDate As Integer = 7
    Private Const GVCDCF_vDiscrepancy As Integer = 8
    Private Const GVCDCF_vSourceResponse As Integer = 9
    Private Const GVCDCF_cDCFStatus As Integer = 10
    Private Const GVCDCF_vUpdatedBy As Integer = 11
    Private Const GVCDCF_dStatusChangedOn As Integer = 12
    Private Const GVCDCF_UserType As Integer = 14
    Private Const GVCDCF_IntrenallyResolved As Integer = 15

    Private Is_ComboGlobalDictionary As Boolean = False
    Private Is_FormulaEnabled As Boolean = False

    Private Const ActId_DataEdition_CTM As String = "0006"
    Private Const ActId_DataEdition_BABE As String = "0007"

    Public Const Type_BABE As String = "BA-BE"

#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.HFWorkspaceId.Value = Me.Request.QueryString("WorkSpaceId").ToString
            Me.HFParentNodeId.Value = Me.Request.QueryString("NodeId").ToString
            Me.HFParentActivityId.Value = Me.Request.QueryString("ActivityId").ToString
            Me.HFPeriodId.Value = Me.Request.QueryString("PeriodId").ToString
            Me.HFSubjectId.Value = Me.Request.QueryString("SubjectId").ToString
            Me.HFMySubjectNo.Value = Me.Request.QueryString("MySubjectNo").ToString
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                Me.HFScreenNo.Value = Me.Request.QueryString("ScreenNo").ToString
            Else
                Me.HFScreenNo.Value = Me.Request.QueryString("SubjectId").ToString
            End If
            Me.HFSchemaId.Value = Me.Request.QueryString("SchemaId").ToString

            Me.lblUserName.Text = Session(S_FirstName).ToString.Trim() + " " + Session(S_LastName).ToString.Trim()
            'Me.lblTime.Text = Session(S_Time).ToString
            Me.lblTime.Text = Convert.ToString(CDate(Session(S_Time)).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)

            'Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            'If Not IsNothing(Me.Request.QueryString("Mode")) Then
            '    If Convert.ToString(Me.Request.QueryString("Mode")).Trim() <> "" Then
            '        Me.ViewState(VS_Choice) = Me.Request.QueryString("Mode").ToString
            '    End If
            'End If

            If Not IsNothing(Me.Request.QueryString("Type")) Then
                If Convert.ToString(Me.Request.QueryString("Type")).Trim() <> "" Then
                    Me.HFType.Value = Me.Request.QueryString("Type").ToString
                End If
            End If

            Me.trimageinformation.Visible = False
            Me.trReviewCompleted.Style.Add("display", "none")

            If Not IsPostBack() Then

                Me.HFActivityId.Value = Me.Request.QueryString("ActivityId").ToString
                Me.HFNodeId.Value = Me.Request.QueryString("NodeId").ToString

                If Not DisplayHeader() Then
                    Exit Sub
                End If

                Me.trimageinformation.Visible = False
                Me.trReviewCompleted.Style.Add("display", "none")
                'Me.BtnSave.Visible = False
                'Me.btnSaveAndContinue.Visible = False
                Me.BtnNext.Visible = False
                Me.BtnPrevious.Visible = False

                If Not FillActivities() Then
                    Exit Sub
                End If

                'If HFSubjectId.Value <> "0000" Then
                '    If Me.ViewState(VS_DataStatus) = CRF_DataEntryPending AndAlso _
                '        Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then

                '        If Session(S_WorkFlowStageId) = WorkFlowStageId_DataEntry AndAlso Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                '            If Not CheckPendingActivity() Then
                '                Throw New Exception
                '            End If
                '        End If
                '        Exit Sub
                '    End If
                'End If

            End If

            If Not GenCall() Then
                Exit Sub
            End If

            If Page.IsPostBack Then
                btnContinueWorking_Click(Nothing, Nothing)
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean

        ' Me.ViewState(VS_DataStatus) = CRF_DataEntryCompleted

        If Me.HFSubjectId.Value.Trim() = "" Then
            Me.ddlRepeatNo.Items.Clear()
        End If

        If Not GenCall_Data() Then
            Exit Function
        End If

        If Not GenCall_ShowUI() Then
            Exit Function
        End If

        Return True

    End Function

#End Region

#Region "GenCall_Data "

    Private Function GenCall_Data() As Boolean
        Dim ds As New DataSet
        Dim ds_CRFSubjectHdrDtl As New DataSet
        Dim dv_CRFSubjectHdrDtl As DataView
        Dim estr_retu As String = String.Empty
        Dim Wstr As String = String.Empty

        Dim ds_CRFHdrDtlSubDtl As New DataSet
        Dim dv As DataView

        Try
            'If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

            '    If Not Me.objHelp.GetDCFMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
            '                ds, estr_retu) Then
            '        Throw New Exception(estr_retu)
            '    End If
            '    Me.ViewState(VS_DtDCF) = ds.Tables(0).Copy()

            '    ds = Nothing
            '    ds = New DataSet

            '    If Not objHelp.GetCRFHdr("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds, estr_retu) Then
            '        Throw New Exception(estr_retu)
            '    End If
            '    Me.ViewState(VS_DtCRFHdr) = ds.Tables(0).Copy()

            '    ds = Nothing
            '    ds = New DataSet

            '    If Not objHelp.GetCRFDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds, estr_retu) Then
            '        Throw New Exception(estr_retu)
            '    End If
            '    Me.ViewState(VS_DtCRFDtl) = ds.Tables(0).Copy()

            '    ds = Nothing
            '    ds = New DataSet

            '    If Not objHelp.GetCRFSubDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds, estr_retu) Then
            '        Throw New Exception(estr_retu)
            '    End If
            '    Me.ViewState(VS_DtCRFSubDtl) = ds.Tables(0).Copy()

            'End If

            If Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso _
                        (Me.Request.QueryString("SubjectId").ToString.Trim() <> "") Then

                If Me.ddlRepeatNo.Items.Count <= 0 Then
                    fillRepeatation()
                End If

            End If

            If Me.ddlRepeatNo.Items.Count > 0 AndAlso Me.ddlRepeatNo.SelectedValue.ToUpper.Trim() <> "N" Then 'For Old

                Wstr = Me.HFSchemaId.Value.Trim() + "##" + Me.HFWorkspaceId.Value.Trim() + "##" + Me.HFSubjectId.Value.Trim() + "##" + Me.HFNodeId.Value.Trim() + "##" + Me.HFPeriodId.Value.Trim()

                If Not Me.objHelp.Proc_CRFHdrDtlSubDtl_Edit(Wstr, ds_CRFSubjectHdrDtl, estr_retu) Then
                    Throw New Exception(estr_retu)
                End If

                Me.ViewState(VS_dtMedEx_Fill) = ds_CRFSubjectHdrDtl.Tables(0)

                If ds_CRFSubjectHdrDtl.Tables(0).Rows.Count > 0 Then

                    Me.HFMedexInfoDtlTranNo.Value = ds_CRFSubjectHdrDtl.Tables(0).Rows(0).Item("iTranNo")
                    Me.HFCRFHdrNo.Value = ds_CRFSubjectHdrDtl.Tables(0).Rows(0).Item("nCRFHdrNo")
                    Me.HFCRFDtlNo.Value = ds_CRFSubjectHdrDtl.Tables(0).Rows(0).Item("nCRFDtlNo")
                    Me.HFCRFDtlLockStatus.Value = ds_CRFSubjectHdrDtl.Tables(0).Rows(0).Item("cCRFDtlLockStatus")
                    Me.ViewState(VS_DataStatus) = ds_CRFSubjectHdrDtl.Tables(0).Rows(0).Item("cCRFDtlDataStatus")
                    'If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    '    Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                    'End If
                    dv = ds_CRFSubjectHdrDtl.Tables(0).DefaultView
                    dv.RowFilter = "(cDCFStatus = '" + Discrepancy_Generated + "' Or cDCFStatus = '" + Discrepancy_Answered _
                                        + "') And ((( vDCFByProfile = '" + Me.Session(S_UserType) + _
                                        "' Or DCFByWorkFlowStageId < " + Me.Session(S_WorkFlowStageId) _
                                        + ") And cDCFType = 'M') Or (cDCFType = 'S')) And DCFByWorkFlowStageId <> " & WorkFlowStageId_OnlyView
                    Me.HFDCFCount.Value = dv.ToTable.Rows.Count

                    'For showing data entry completed data
                    dv_CRFSubjectHdrDtl = ds_CRFSubjectHdrDtl.Tables(0).DefaultView
                    dv_CRFSubjectHdrDtl.RowFilter = "cCRFDtlDataStatus <> '" + CRF_DataEntry + "'"
                    Me.ViewState(VS_dtMedEx_Fill) = dv_CRFSubjectHdrDtl.ToTable()
                    '*************

                    'For showing data entry continue data
                    dv_CRFSubjectHdrDtl = ds_CRFSubjectHdrDtl.Tables(0).DefaultView
                    dv_CRFSubjectHdrDtl.RowFilter = "iRepeatNo = " + Me.ddlRepeatNo.SelectedItem.Value.Trim()
                    If dv_CRFSubjectHdrDtl.ToTable().Rows.Count > 0 Then

                        If dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("cCRFDtlDataStatus") = CRF_DataEntry Then
                            Me.ViewState(VS_dtMedEx_Fill) = dv_CRFSubjectHdrDtl.ToTable()
                            Me.HFMedexInfoDtlTranNo.Value = dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("iTranNo")
                            Me.HFCRFHdrNo.Value = dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("nCRFHdrNo")
                            Me.HFCRFDtlNo.Value = dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("nCRFDtlNo")
                            Me.HFCRFDtlLockStatus.Value = dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("cCRFDtlLockStatus")
                            Me.ViewState(VS_DataStatus) = dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("cCRFDtlDataStatus")
                            'If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                            '    Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                            'End If
                            dv = dv_CRFSubjectHdrDtl.ToTable.DefaultView
                            dv.RowFilter = "(cDCFStatus = '" + Discrepancy_Generated + "' Or cDCFStatus = '" + Discrepancy_Answered _
                                        + "') And ((( vDCFByProfile = '" + Me.Session(S_UserType) + _
                                        "' Or DCFByWorkFlowStageId < " + Me.Session(S_WorkFlowStageId) _
                                        + ") And cDCFType = 'M') Or (cDCFType = 'S')) And DCFByWorkFlowStageId <> " & WorkFlowStageId_OnlyView
                            Me.HFDCFCount.Value = dv.ToTable.Rows.Count

                        Else

                            Me.HFMedexInfoDtlTranNo.Value = dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("iTranNo")
                            Me.HFCRFHdrNo.Value = dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("nCRFHdrNo")
                            Me.HFCRFDtlNo.Value = dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("nCRFDtlNo")
                            Me.HFCRFDtlLockStatus.Value = dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("cCRFDtlLockStatus")
                            Me.ViewState(VS_DataStatus) = dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("cCRFDtlDataStatus")
                            'If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                            '    Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                            'End If
                            dv = dv_CRFSubjectHdrDtl.ToTable.DefaultView
                            dv.RowFilter = "(cDCFStatus = '" + Discrepancy_Generated + "' Or cDCFStatus = '" + Discrepancy_Answered _
                                        + "') And ((( vDCFByProfile = '" + Me.Session(S_UserType) + _
                                        "' Or DCFByWorkFlowStageId < " + Me.Session(S_WorkFlowStageId) _
                                        + ") And cDCFType = 'M') Or (cDCFType = 'S')) And DCFByWorkFlowStageId <> " & WorkFlowStageId_OnlyView
                            Me.HFDCFCount.Value = dv.ToTable.Rows.Count

                        End If
                    End If
                    '****************
                    Me.btnReviewHistory.Visible = True
                    Me.btnReviewHistory.OnClientClick = "return show_popup('frmCRFReviewHistory.aspx?CRFDtlNo=" + Me.HFCRFDtlNo.Value.Trim() + "')"

                End If

                'Me.BtnSave.Visible = True
                'Me.btnSaveAndContinue.Visible = True

                'If CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_Review Then
                '    'Me.BtnSave.Visible = False
                '    'Me.btnSaveAndContinue.Visible = False
                '    'Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View
                '    If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                '        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                '    End If
                'End If

                Me.trReviewCompleted.Visible = True
                Me.trReviewCompleted.Disabled = False
                Me.btnOk.Visible = True
                Me.chkReviewCompleted.Checked = False
                '*************Checking for Lock Status of CRFDtl************
                If Not Me.HFCRFDtlLockStatus.Value Is Nothing AndAlso _
                       Convert.ToString(Me.HFCRFDtlLockStatus.Value).Trim.ToUpper() = CRFHdr_Locked Then
                    Me.chkReviewCompleted.Checked = True
                    Me.trReviewCompleted.Disabled = True
                    Me.trReviewCompleted.Visible = False
                    Me.btnOk.Visible = False
                End If
                '*************************************

                '************Showing Edit Checks Queries*******************
                'If Not Me.fillQueryGrid() Then
                '    Throw New Exception("Error While Filling Query Grid.")
                'End If
                '**********************************************************

            ElseIf Me.ddlRepeatNo.Items.Count > 0 AndAlso Me.ddlRepeatNo.SelectedValue.ToUpper.Trim() = "N" Then 'For New

                Wstr = "cActiveFlag<>'N' and vWorkSpaceId='" & Me.HFWorkspaceId.Value.Trim() & "'" & _
                        " and vActivityId='" & Me.HFActivityId.Value.Trim() & "'" & _
                        " And iNodeId=" & _
                        Me.HFNodeId.Value.Trim() & " Order by iSeqNo"

                If Not Me.objHelp.GetViewMedExWorkSpaceDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr_retu) Then
                    Throw New Exception(estr_retu)
                End If
                Me.ViewState(VS_dtMedEx_Fill) = ds.Tables(0)
                'Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

                Me.chkReviewCompleted.Checked = False
                Me.btnOk.Visible = False
                Me.trReviewCompleted.Disabled = True
                Me.trReviewCompleted.Visible = False

                'Changed as when data entry is pending then GetViewMedExWorkSpaceDtl is used for crf
                ' And so status should be CRF_DataEntryPending and not CRF_DataEntry. -Pratiksha
                'Me.ViewState(VS_DataStatus) = CRF_DataEntry
                Me.ViewState(VS_DataStatus) = CRF_DataEntryPending
                'Me.BtnSave.Visible = True
                'Me.btnSaveAndContinue.Visible = True

                'If Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_DataEntry Or Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                '    Me.BtnSave.Visible = False
                '    Me.btnSaveAndContinue.Visible = False
                'End If

            End If

            If Not Me.IsPostBack() Then

                If Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_MedicalCoding Then

                    'If Me.Session(S_WorkFlowStageId) > WorkFlowStageId_DataEntry AndAlso (Me.ViewState(VS_DataStatus) = CRF_DataEntry Or Me.ViewState(VS_DataStatus) = CRF_DataEntryPending) Then
                    '    If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    '        'Me.BtnSave.Visible = False
                    '        'Me.btnSaveAndContinue.Visible = False
                    '        Me.trReviewCompleted.Visible = False
                    '        If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    '            Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View
                    '            'Me.objCommon.ShowAlert("Data Entry Or Review Is Not Done By Lower Stage For Selected Activity.", Me.Page)
                    '            'Exit Function
                    '        End If
                    '    End If

                ElseIf Not Me.HFReviewedWorkFlowId.Value Is Nothing AndAlso Convert.ToString(Me.HFReviewedWorkFlowId.Value).Trim() <> "" Then

                    ' Condition changed for view mode -- Pratiksha
                    If Convert.ToString(Me.Session(S_WorkFlowStageId)) <> WorkFlowStageId_OnlyView Then 'Or WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View <> Me.ViewState(VS_Choice) Then

                        If Convert.ToString(Me.HFReviewedWorkFlowId.Value) <> WorkFlowStageId_DataEntry Then

                            If Convert.ToInt32(Me.HFReviewedWorkFlowId.Value) = Convert.ToInt32(Me.Session(S_WorkFlowStageId)) Or _
                                Convert.ToInt32(Me.HFReviewedWorkFlowId.Value) > Convert.ToInt32(Me.Session(S_WorkFlowStageId)) Then
                                'Me.objCommon.ShowAlert("Review Already Done For Selected Activity.", Me.Page)
                                Me.ViewState(VS_ReviewFlag) = "YES"
                                'Me.BtnSave.Visible = False
                                'Me.btnSaveAndContinue.Visible = False
                                Return True
                            End If

                        End If

                        If Convert.ToString(Me.HFReviewedWorkFlowId.Value) = WorkFlowStageId_DataEntry AndAlso _
                            Me.ddlRepeatNo.SelectedValue.Trim.ToUpper() <> "N" Then
                            Me.ViewState(VS_ReviewFlag) = "NO"

                            If CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_DataEntry AndAlso _
                                Me.Session(S_WorkFlowStageId) = WorkFlowStageId_DataEntry Then
                                'Me.BtnSave.Visible = True
                                'Me.btnSaveAndContinue.Visible = True
                                Me.trReviewCompleted.Visible = False
                            Else
                                If Convert.ToInt32(Me.HFReviewedWorkFlowId.Value) <= (Convert.ToInt32(Me.Session(S_WorkFlowStageId)) - 10) Then
                                    'Me.BtnSave.Visible = False
                                    'Me.btnSaveAndContinue.Visible = False
                                    If CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_DataEntry Then
                                        Me.trReviewCompleted.Visible = False
                                    End If
                                End If
                            End If
                        End If

                        ' Condition changed for view mode -- pratiksha
                        If Convert.ToInt32(Me.HFReviewedWorkFlowId.Value) < (Convert.ToInt32(Me.Session(S_WorkFlowStageId)) - 10) Then ' And Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

                            If Convert.ToInt32(Me.HFImportedDataWorkFlowId.Value) = (Convert.ToInt32(Me.Session(S_WorkFlowStageId))) Then
                                'This is done only for activity of lab data import, because from import lab data page, only entries are being made and no review process is done.
                                Return True
                            End If

                            'Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View
                            'Me.objCommon.ShowAlert("Data Entry Or Review Is Not Done By Lower Stage For Selected Activity.", Me.Page)
                            'Exit Function

                        End If

                    Else
                        Me.trReviewCompleted.Visible = False
                    End If

                End If

            ElseIf Me.Session(S_WorkFlowStageId) = WorkFlowStageId_MedicalCoding Then

                Me.trReviewCompleted.Visible = False

            End If

            'End If

            'edit by vishal
            Dim ds_Check As New DataSet

            'If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
            Wstr = "vWorkspaceId = '" + Me.HFWorkspaceId.Value + "' And cStatusIndi <> 'D' Order by iTranNo desc"

            If Not Me.objHelp.GetCRFLockDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_Check, estr_retu) Then
                Throw New Exception(estr_retu)
            End If
            If Not ds_Check Is Nothing Then

                ' edited by vishal for lock/unlock site
                'If ds_Check.Tables(0).Rows.Count > 0 Then

                '    If Convert.ToString(ds_Check.Tables(0).Rows(0)("cLockFlag")).Trim.ToUpper() = CRFHdr_Locked Then
                '        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View
                '    End If

                'End If

            End If

            'End If

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message.ToString, estr_retu)
        End Try
    End Function

#End Region

#Region "DisplayHeader"

    Private Function DisplayHeader() As Boolean
        Dim Wstr As String = ""
        Dim ds_Heading As New DataSet
        Dim estr As String = ""
        Dim ds_WorkspaceNodeDetail As New DataSet
        Dim dt_heading As New DataTable
        Dim vMysubjectId As String = String.Empty

        Dim ds_WorkspaceSubjectMst As New DataSet
        Try

            Wstr = "vWorkspaceId='" & Me.HFWorkspaceId.Value.Trim() & "' and cStatusIndi <> 'D'"

            If Not Me.objHelp.GetWorkspaceProtocolDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition _
                                    , ds_Heading, estr) Then
                Throw New Exception("Error while getting Header information. " + estr)
            End If
            dt_heading = ds_Heading.Tables(0)

            '********Getting Activity name for display
            Wstr += " And vActivityId = '" + Me.HFActivityId.Value.Trim() + "' And iNodeId = " + Me.HFNodeId.Value.Trim()
            If Not Me.objHelp.getWorkSpaceNodeDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition _
                                                , ds_WorkspaceNodeDetail, estr) Then
                Throw New Exception("Error while getting Header information. " + estr)
            End If
            '*********************************

            '***********Getting Randomization No or vMySubjectNo for display
            Wstr = "vWorkspaceId='" & Me.HFWorkspaceId.Value.Trim() & "' and cStatusIndi <> 'D' And "
            Wstr += " iMySubjectNo = " + Me.HFMySubjectNo.Value.Trim() & " and iperiod = " & Convert.ToString(Me.HFPeriodId.Value).Trim()
            If Not Me.objHelp.GetWorkspaceSubjectMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                     ds_WorkspaceSubjectMst, estr) Then
                Throw New Exception("Error while getting Header information. " + estr)
            End If
            '*************************************************

            If dt_heading.Rows.Count > 0 Then

                Me.lblHeader.Text = "Project No: " + dt_heading.Rows(0).Item("vProjectNo").ToString.Trim() + _
                                        ",       Subject Id : " + Me.HFSubjectId.Value.Trim() + _
                                        ",       MySubject No : 0" + _
                                        ",       Activity: " + _
                                        ",       Period: " + Convert.ToString(Me.HFPeriodId.Value).Trim()

                If ds_WorkspaceNodeDetail.Tables(0).Rows.Count > 0 Then

                    Me.lblHeader.Text = "Project No: " + dt_heading.Rows(0).Item("vProjectNo").ToString.Trim() + _
                                        ",       Subject Id : " + Me.HFSubjectId.Value.Trim() + _
                                        ",       MySubject No : 0" + _
                                        ",       Activity: " + Convert.ToString(ds_WorkspaceNodeDetail.Tables(0).Rows(0)("vNodeDisplayName")).Trim() + _
                                        ",       Period: " + Convert.ToString(Me.HFPeriodId.Value).Trim()

                    If ds_WorkspaceSubjectMst.Tables(0).Rows.Count > 0 Then

                        Me.lblHeader.Text = "Project No: " + dt_heading.Rows(0).Item("vProjectNo").ToString.Trim() + _
                                        ",       Subject Id : " + Me.HFSubjectId.Value.Trim() + _
                                        ",       MySubject No : " + Convert.ToString(ds_WorkspaceSubjectMst.Tables(0).Rows(0)("vMySubjectNo")).Trim() + _
                                        ",       Activity: " + Convert.ToString(ds_WorkspaceNodeDetail.Tables(0).Rows(0)("vNodeDisplayName")).Trim() + _
                                        ",       Period: " + Convert.ToString(Me.HFPeriodId.Value).Trim()

                    End If

                End If

            End If

            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then


                If dt_heading.Rows.Count > 0 Then

                    Me.lblHeader.Text = "Site No: " + dt_heading.Rows(0).Item("vProjectNo").ToString.Trim() + _
                                ",       Screen No : " + Me.HFScreenNo.Value.Trim()

                    If ds_WorkspaceSubjectMst.Tables(0).Rows.Count > 0 Then
                        If Convert.ToString(ds_WorkspaceSubjectMst.Tables(0).Rows(0)("vRandomizationNo")).Trim() <> "" Then
                            Me.lblHeader.Text += ",       Patient/Randomization No : " + Convert.ToString(ds_WorkspaceSubjectMst.Tables(0).Rows(0)("vRandomizationNo")).Trim()
                        End If
                    End If

                    If ds_WorkspaceNodeDetail.Tables(0).Rows.Count > 0 Then
                        Me.lblHeader.Text += ",       Visit: " + ds_WorkspaceNodeDetail.Tables(0).Rows(0)("vNodeDisplayName").ToString()
                    End If

                    If Convert.ToString(dt_heading.Rows(0).Item("iNoOfPeriods")).Trim() <> "1" Then
                        Me.lblHeader.Text += ",       Period: " + Convert.ToString(Me.HFPeriodId.Value).Trim()
                    End If

                End If

            End If

            dt_heading = Nothing

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "Fill Edit Reason DropDown"

    Private Function FillEditDropDown() As Boolean
        Dim estr As String = String.Empty
        Dim Ds_Reason As New DataSet
        Dim wStr As String = String.Empty
        Dim item As New ListItem

        Try

            wStr = " vActivityId ='" & ActId_DataEdition_BABE & "' and cStatusIndi <> 'D'"
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                wStr = " vActivityId ='" & ActId_DataEdition_CTM & "' and cStatusIndi <> 'D'"
            End If

            If Not Me.objHelp.View_ReasonMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                        Ds_Reason, estr) Then
                Throw New Exception(estr)
            End If

            Me.DdlEditRemarks.DataSource = Ds_Reason.Tables(0)
            Me.DdlEditRemarks.DataValueField = "nReasonNo"
            Me.DdlEditRemarks.DataTextField = "vReasonDesc"
            Me.DdlEditRemarks.DataBind()

            item.Text = "Please Select a Reason"
            item.Value = "0"
            Me.DdlEditRemarks.Items.Insert(0, item)
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Filling Reasons ,", ex.Message)
            Return False
        End Try

    End Function

#End Region

#Region "GenCall_ShowUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim dt As New DataTable
        Dim dv As New DataView
        Dim dt_MedExMst As New DataTable
        Dim dr As DataRow
        Dim drGroup As DataRow
        Dim objelement As Object
        Dim StrValidation() As String
        Dim StrGroup(1) As String
        Dim StrGroupCodes As String = ""
        Dim StrGroupDesc As String = ""
        Dim dt_heading As New DataTable
        Dim dv_MedexGroup As DataView
        Dim dt_MedexGroup As New DataTable
        Dim i As Integer = 0
        Dim CntSubGroup As Integer = 0
        Dim PrevSubGroupCodes As String = ""
        Dim Wstr As String = ""
        Dim ds_Heading As New DataSet
        Dim estr As String = ""
        Dim ds_WorkspaceNodeDetail As New DataSet
        Dim StrMedExCodes As String = String.Empty
        Dim Repeatation As Integer = 1
        Dim PreviousRepeatation As Integer = 1
        Dim Copyright As String = String.Empty
        Dim clientname As String = String.Empty

        Try
            Copyright = System.Configuration.ConfigurationManager.AppSettings("CopyRight")

            Page.Title = " :: Subject Attributes Detail :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            Me.CompanyName.Value = Copyright



            PlaceMedEx.Controls.Clear()

            If CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows.Count < 1 Then
                'Me.BtnSave.Visible = False
                'Me.btnSaveAndContinue.Visible = False
                Me.BtnNext.Visible = False
                Me.BtnPrevious.Visible = False
                If Not IsPostBack Then
                    Me.objCommon.ShowAlert("No Attribute is Attached with This Activity. So, please Attach Attribute to this Activity.", Me.Page)
                End If
                Exit Function
            End If

            dt_MedexGroup = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable)

            Dim ds As New DataSet
            ds.Tables.Add(dt_MedexGroup.Copy())
            dv_MedexGroup = ds.Tables(0).DefaultView

            StrGroup(0) = "vMedExGroupCode"
            StrGroup(1) = "vMedExGroupDesc"
            dt_MedexGroup = dv_MedexGroup.ToTable(True, StrGroup)
            If dt_MedexGroup.Rows.Count > 1 Then
                Me.BtnNext.Visible = True
                Me.BtnPrevious.Visible = True
            End If

            PlaceMedEx.Controls.Add(New LiteralControl("<Table  style=""width: 980px"">"))
            'For Tab Buttons
            For Each drGroup In dt_MedexGroup.Rows

                If StrGroupCodes <> "" Then
                    StrGroupCodes += ",Div" + drGroup("vMedExGroupCode")
                    StrGroupDesc += "," + drGroup("vMedExGroupDesc")
                Else
                    StrGroupCodes = "Div" + drGroup("vMedExGroupCode")
                    StrGroupDesc = "" + drGroup("vMedExGroupDesc")
                End If

            Next

            PlaceMedEx.Controls.Add(New LiteralControl("<tr>"))
            PlaceMedEx.Controls.Add(New LiteralControl("<Td style="" vertical-align:top"">"))
            PlaceMedEx.Controls.Add(New LiteralControl("<div style=""display: block; min-width: 100%; width: auto !important; width: 100%;" + _
                    " align=""center"" id=""divMedExGroups"" runat=""server"">"))

            PlaceMedEx.Controls.Add(New LiteralControl("<Table  style=""width: 100%"">"))
            PlaceMedEx.Controls.Add(New LiteralControl("<tr>"))

            PlaceMedEx.Controls.Add(New LiteralControl("<Td style="" width:980px; vertical-align:top"">"))
            Me.GetButtons(StrGroupDesc, StrGroupCodes)

            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
            PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
            PlaceMedEx.Controls.Add(New LiteralControl("</Table>"))
            PlaceMedEx.Controls.Add(New LiteralControl("</div>"))

            Me.BtnNext.OnClientClick = "return Next('" & StrGroupCodes & "');"
            Me.BtnPrevious.OnClientClick = "return Previous('" & StrGroupCodes & "');"

            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
            PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

            '**********For user's readibility************************
            PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" height=""2px"" ALIGN=LEFT style=""BACKGROUND-COLOR: #FFFFFF"">"))
            PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""white-space: nowrap; vertical-align:middle"">"))
            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
            PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
            '*************************

            PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT >"))
            PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""vertical-align:top"">"))

            For Each drGroup In dt_MedexGroup.Rows

                dt = New DataTable
                dt_MedExMst = New DataTable
                dt = Me.ViewState(VS_dtMedEx_Fill)
                dv = New DataView
                dv = dt.DefaultView
                dv.RowFilter = "vMedExGroupCode='" & drGroup("vMedExGroupCode").ToString.Trim() & "'"
                dt_MedExMst = dv.ToTable()
                If Me.Session(S_SelectedTab) = Nothing Then
                    Me.Session(S_SelectedTab) = ""
                End If

                If HFActivateTab.Value <> "" Then
                    Me.Session(S_SelectedTab) = HFActivateTab.Value
                Else
                    Me.HFActivateTab.Value = Me.Session(S_SelectedTab).ToString()
                End If
                If Me.Session(S_SelectedTab) = "" Then
                    If i = 0 Then
                        PlaceMedEx.Controls.Add(New LiteralControl("<Div id='Div" & drGroup("vMedExGroupCode").ToString.Trim() & "' style=""display:block""" + _
                         " >")) 'Added for QC Comments on 22-May-2009

                        'Added on 27-Jul-2009
                        'HFActivateTab.Value = "Div" + drGroup("vMedExGroupCode")
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "CurrentTab", "currTab='Div" & drGroup("vMedExGroupCode").ToString.Trim() & "';", True)
                        '***********************
                    Else
                        PlaceMedEx.Controls.Add(New LiteralControl("<Div id='Div" & drGroup("vMedExGroupCode").ToString.Trim() & "' style=""display:none""" + _
                         " >")) 'Added for QC Comments on 22-May-2009
                    End If
                Else
                    If Me.Session(S_SelectedTab).ToString() = "Div" + drGroup("vMedExGroupCode") Then
                        PlaceMedEx.Controls.Add(New LiteralControl("<Div id='Div" & drGroup("vMedExGroupCode").ToString.Trim() & "' style=""display:block""" + _
                         " >")) 'Added for QC Comments on 22-May-2009

                        'Added on 27-Jul-2009

                        'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "ShowHideDiv", "ShowHideDiv('divMedExGroups','imgMedExGroup')", True)
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "CurrentTab", "currTab='" & Me.Session(S_SelectedTab).ToString() & "';", True)

                    Else
                        PlaceMedEx.Controls.Add(New LiteralControl("<Div id='Div" & drGroup("vMedExGroupCode").ToString.Trim() & "' style=""display:none""" + _
                        " >")) 'Added for QC Comments on 22-May-2009
                    End If
                End If
                Me.Session(S_SelectedTab) = Nothing

                'PlaceMedEx.Controls.Add(New LiteralControl("<Table width=""100%"" style=""border: solid 1px gray"">")) ' border=""1""
                PlaceMedEx.Controls.Add(New LiteralControl("<Table width=""970px"" style=""border: solid 1px gray"">")) 'Added on 30-01-2010 to fix the size of display

                For Each dr In dt_MedExMst.Rows

                    If StrMedExCodes <> String.Empty Then
                        StrMedExCodes += ","
                    End If
                    StrMedExCodes += Convert.ToString(dr("vMedExCode")).Trim()

                    'If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                    If Me.ViewState(VS_DataStatus) <> CRF_DataEntryPending Then
                        Repeatation = dr("iRepeatNo")
                    End If

                    If Me.ViewState(VS_DataStatus) <> CRF_DataEntry AndAlso Me.ViewState(VS_DataStatus) <> CRF_DataEntryPending Then

                        'If dr("iWorkflowStageId") = (Convert.ToInt32(Me.Session(S_WorkFlowStageId)) - 10) Then
                        '    Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                        'End If

                        If PreviousRepeatation <> Repeatation Then
                            PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" height=""1px"" ALIGN=LEFT style=""BACKGROUND-COLOR: #FFC300"">"))
                            'PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" ALIGN=LEFT style=""BACKGROUND-COLOR: #ffcc66"">")) '008ecd
                            PlaceMedEx.Controls.Add(New LiteralControl("<Td colspan=""3"" style=""white-space: nowrap; vertical-align:middle; font-weight: bold; font-size: 12px; float: left; "">"))
                            PlaceMedEx.Controls.Add(New LiteralControl("Repetition " + Convert.ToString(Repeatation)))
                            'PlaceMedEx.Controls.Add(Getlable("Repeatation " + Convert.ToString(dr("iRepeatNo")), "Repeatation" + Convert.ToString(dr("iRepeatNo"))))
                            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                            PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
                        End If
                    End If
                    If Me.ViewState(VS_DataStatus) <> CRF_DataEntryPending Then
                        PreviousRepeatation = dr("iRepeatNo")
                    End If
                    'End If

                    If PrevSubGroupCodes = "" Or PrevSubGroupCodes <> dr("vMedExSubGroupCode") Then

                        If Convert.ToString(dr("vMedExSubGroupCode")).Trim() <> "0000" Then 'Added for default SubGroup

                            PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" ALIGN=LEFT style=""BACKGROUND-COLOR: #008ecd"">")) 'ffcc66
                            PlaceMedEx.Controls.Add(New LiteralControl("<Td colspan=""3"" style=""white-space: nowrap; vertical-align:middle"">"))
                            PlaceMedEx.Controls.Add(Getlable(dr("vMedExSubGroupDesc").ToString.Trim(), dr("vMedExSubGroupCode") + CntSubGroup.ToString.Trim() + "R" + Convert.ToString(Repeatation)))
                            CntSubGroup += 1
                            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                            PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                        End If

                    End If

                    PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT>"))


                    If Convert.ToString(dr("vMedExType")).Trim.ToUpper() <> "LABEL" Then
                        PlaceMedEx.Controls.Add(New LiteralControl("<Td style="" width:500px; vertical-align:middle"" ALIGN=LEFT>"))

                        PrevSubGroupCodes = dr("vMedExSubGroupCode")
                        PlaceMedEx.Controls.Add(GetlableWithHistory(dr("vMedExDesc") & ": ", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), Repeatation))
                        PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                        PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""white-space: nowrap; vertical-align:middle"" ALIGN=LEFT>"))
                    Else
                        PlaceMedEx.Controls.Add(New LiteralControl("<Td colspan=""2"" style="" vertical-align:middle"" ALIGN=LEFT>"))
                        PrevSubGroupCodes = dr("vMedExSubGroupCode")
                        'PlaceMedEx.Controls.Add(GetlableWithHistory(dr("vMedExDesc") & ": ", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode")))
                    End If

                    If Not (dr("vValidationType") Is System.DBNull.Value) Then
                        If dr("vValidationType") <> "" And dr("vValidationType") <> "NA" Then
                            StrValidation = dr("vValidationType").ToString.Trim().Split(",")
                            If StrValidation.Length > 1 Then
                                objelement = GetObject(dr("vMedExType"), dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("vMedExValues"), IIf(dr("vDefaultValue") Is System.DBNull.Value, "", dr("vDefaultValue")), _
                                                        StrValidation(0).ToString.Trim(), StrValidation(1).ToString.Trim(), IIf(dr("vAlertonvalue") Is System.DBNull.Value, "", dr("vAlertonvalue")), _
                                                        IIf(dr("vAlertMessage") Is System.DBNull.Value, "", dr("vAlertMessage")), IIf(dr("vHighRange") Is System.DBNull.Value, "0", dr("vHighRange")), _
                                                        IIf(dr("vLowRange") Is System.DBNull.Value, "0", dr("vLowRange")), _
                                                        IIf(dr("vRefTable") Is System.DBNull.Value, "", dr("vRefTable")), _
                                                        IIf(dr("vRefColumn") Is System.DBNull.Value, "", dr("vRefColumn")), IIf(dr("cAlertType") Is System.DBNull.Value, "", dr("cAlertType")))
                            Else
                                objelement = GetObject(dr("vMedExType"), dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("vMedExValues"), IIf(dr("vDefaultValue") Is System.DBNull.Value, "", dr("vDefaultValue")), _
                                                        StrValidation(0).ToString.Trim(), "", IIf(dr("vAlertonvalue") Is System.DBNull.Value, "", dr("vAlertonvalue")), _
                                                        IIf(dr("vAlertMessage") Is System.DBNull.Value, "", dr("vAlertMessage")), IIf(dr("vHighRange") Is System.DBNull.Value, "0", dr("vHighRange")), _
                                                        IIf(dr("vLowRange") Is System.DBNull.Value, "0", dr("vLowRange")), _
                                                        IIf(dr("vRefTable") Is System.DBNull.Value, "", dr("vRefTable")), _
                                                        IIf(dr("vRefColumn") Is System.DBNull.Value, "", dr("vRefColumn")), IIf(dr("cAlertType") Is System.DBNull.Value, "", dr("cAlertType")))
                            End If

                        Else
                            objelement = GetObject(dr("vMedExType"), dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("vMedExValues"), IIf(dr("vDefaultValue") Is System.DBNull.Value, "", dr("vDefaultValue")), _
                                                     "", "", IIf(dr("vAlertonvalue") Is System.DBNull.Value, "", dr("vAlertonvalue")), _
                                                        IIf(dr("vAlertMessage") Is System.DBNull.Value, "", dr("vAlertMessage")), IIf(dr("vHighRange") Is System.DBNull.Value, "0", dr("vHighRange")), _
                                                        IIf(dr("vLowRange") Is System.DBNull.Value, "0", dr("vLowRange")), _
                                                        IIf(dr("vRefTable") Is System.DBNull.Value, "", dr("vRefTable")), _
                                                        IIf(dr("vRefColumn") Is System.DBNull.Value, "", dr("vRefColumn")), IIf(dr("cAlertType") Is System.DBNull.Value, "", dr("cAlertType")))
                        End If
                    Else
                        objelement = GetObject(dr("vMedExType"), dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("vMedExValues"), IIf(dr("vDefaultValue") Is System.DBNull.Value, "", dr("vDefaultValue")), _
                                                    "", "", IIf(dr("vAlertonvalue") Is System.DBNull.Value, "", dr("vAlertonvalue")), _
                                                        IIf(dr("vAlertMessage") Is System.DBNull.Value, "", dr("vAlertMessage")), IIf(dr("vHighRange") Is System.DBNull.Value, "0", dr("vHighRange")), _
                                                        IIf(dr("vLowRange") Is System.DBNull.Value, "0", dr("vLowRange")), _
                                                        IIf(dr("vRefTable") Is System.DBNull.Value, "", dr("vRefTable")), _
                                                        IIf(dr("vRefColumn") Is System.DBNull.Value, "", dr("vRefColumn")), IIf(dr("cAlertType") Is System.DBNull.Value, "", dr("cAlertType")))
                    End If

                    PlaceMedEx.Controls.Add(objelement)

                    ''For File Type*************
                    'If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                    If dr("vMedExType").ToString.ToUpper.Trim() = "FILE" AndAlso dr("vDefaultValue").ToString.Trim() <> "" Then
                        Dim HLnkFile As New HyperLink

                        HLnkFile.ID = "hlnk" + dr("vMedExCode").ToString.Trim()
                        HLnkFile.NavigateUrl = dr("vDefaultValue").ToString.Trim()
                        HLnkFile.Text = Path.GetFileName(dr("vDefaultValue").ToString.Trim())
                        HLnkFile.Target = "_blank"
                        PlaceMedEx.Controls.Add(HLnkFile)
                    End If

                    'End If

                    'For Print UOM 
                    If Not dr("vUOM") Is System.DBNull.Value AndAlso dr("vUOM").ToString.Trim() <> "" Then
                        PlaceMedEx.Controls.Add(Getlable(dr("vUOM"), "UOM" + dr("vMedExCode") + "R" + Convert.ToString(Repeatation)))
                    End If
                    '*********************************

                    'If Is_ComboGlobalDictionary Then
                    '    If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_MedicalCoding Then
                    '        PlaceMedEx.Controls.Add(GetBrowseButton("Browse", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation)))
                    '        Is_ComboGlobalDictionary = False
                    '    End If
                    'End If

                    If Is_FormulaEnabled Then
                        If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_DataEntry Or _
                                    Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_FirstReview Then
                            'PlaceMedEx.Controls.Add(GetAutoCalculateButton("Calc", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("vMedExFormula")))
                            Is_FormulaEnabled = False
                        End If
                    End If

                    'Added By Pratiksha To have date or time buttons //Enhancement in EDC
                    If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_DataEntry Then _
                        'AndAlso Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View _

                        If Me.Session(S_EDCUser).ToString = EDCUser Then
                            If Not dt_MedExMst.Columns.Contains("iTranNo") Then
                                If Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "DATETIME" Then
                                    PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                                    PlaceMedEx.Controls.Add(GetAutoDateButton(dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), Repeatation))
                                    PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                                    PlaceMedEx.Controls.Add(GetClearButton(dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), Repeatation))
                                ElseIf Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "TIME" Then
                                    PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                                    PlaceMedEx.Controls.Add(GetAutoTimeButton(dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), Repeatation))
                                    PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                                    PlaceMedEx.Controls.Add(GetClearButton(dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), Repeatation))
                                End If
                            ElseIf CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_DataEntry Then
                                If Convert.ToString(dr("vDefaultValue")).Trim() = "" Or dr("iTranNo") < 1 Then
                                    If Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "DATETIME" Then
                                        PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                                        PlaceMedEx.Controls.Add(GetAutoDateButton(dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), Repeatation))
                                        PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                                        PlaceMedEx.Controls.Add(GetClearButton(dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), Repeatation))
                                    ElseIf Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "TIME" Then
                                        PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                                        PlaceMedEx.Controls.Add(GetAutoTimeButton(dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), Repeatation))
                                        PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                                        PlaceMedEx.Controls.Add(GetClearButton(dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), Repeatation))
                                    End If
                                End If
                            End If
                        End If
                    End If
                    ''''''''''''''''''''''''''''''''''''''''''''''''

                    PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                    'PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""white-space: nowrap; vertical-align:middle"" ALIGN=LEFT>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""white-space: nowrap "" ALIGN=""LEFT"">"))

                    objelement.Attributes.Remove("disabled")

                    If (Me.ddlRepeatNo.SelectedIndex = 0 AndAlso _
                            (Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_DataEntry AndAlso _
                             Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_FirstReview)) Then ' Or Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                        objelement.Attributes.Add("disabled", "true")
                    End If

                    If Me.ddlRepeatNo.SelectedIndex <> 0 AndAlso _
                            (CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_Review Or _
                                CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_ReviewCompleted Or _
                                CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_Locked) Then

                        If Convert.ToString(dr("vMedExType")).Trim.ToUpper() <> "LABEL" Then

                            objelement.Attributes.Add("disabled", "true")

                            'Condition changed to solve problem of view mode -- Pratiksha
                            'If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

                            '    If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_OnlyView Then

                            If Not Me.HFCRFDtlLockStatus.Value Is Nothing AndAlso _
                                   Not Convert.ToString(Me.HFCRFDtlLockStatus.Value).Trim.ToUpper() = CRFHdr_Locked Then

                                'Checking if Discrepancy is there or not in field
                                If Convert.ToString(dr("nDCFNo")).Trim() <> "" AndAlso _
                                        Convert.ToString(dr("cDCFStatus")).Trim.ToUpper() <> Discrepancy_Answered AndAlso _
                                        Convert.ToString(dr("cDCFStatus")).Trim.ToUpper() <> Discrepancy_Resolved AndAlso _
                                        Convert.ToString(dr("cDCFStatus")).Trim.ToUpper() <> Discrepancy_InternallyResolved AndAlso _
                                        Convert.ToString(dr("cDCFStatus")).Trim.ToUpper() <> Discrepancy_AutoResolved Then

                                    'AndAlso CType(Me.Session(S_UserID), Integer) = CType(Convert.ToString(dr("iDCFBy")).Trim(), Integer) Then

                                    If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_DataEntry Then
                                        If (Me.Session(S_EDCUser).ToString() = EDCUser And _
                                            (Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "DATETIME" Or Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "TIME")) Then
                                            'Else
                                            '    PlaceMedEx.Controls.Add(GetEditButton("Edit", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo")))
                                            '    PlaceMedEx.Controls.Add(GetUpdateButton("Update", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo")))
                                        End If
                                    End If

                                    objelement.Attributes.Add("title", Convert.ToString(dr("vSourceResponse")).Trim())

                                End If

                                'If Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "COMBOGLOBALDICTIONARY" AndAlso _
                                '                        Me.Session(S_WorkFlowStageId) = WorkFlowStageId_MedicalCoding Then
                                '    PlaceMedEx.Controls.Add(GetEditButton("Edit", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo")))
                                '    PlaceMedEx.Controls.Add(GetUpdateButton("Update", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo")))

                                'End If

                            End If

                            If dt.Columns.Contains("nCRFDtlNo") Then

                                'Condition changed for view mode -pratiksha
                                'If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                                'If Convert.ToString(Me.ViewState(VS_ReviewFlag)).ToUpper() <> "YES" Then
                                If Convert.ToString(dr("nDCFNo")).Trim() <> "" AndAlso _
                                      Convert.ToString(dr("cDCFStatus")).Trim.ToUpper() <> Discrepancy_Answered AndAlso _
                                      Convert.ToString(dr("cDCFStatus")).Trim.ToUpper() <> Discrepancy_Resolved AndAlso _
                                       Convert.ToString(dr("cDCFStatus")).Trim.ToUpper() <> Discrepancy_InternallyResolved AndAlso _
                                      Convert.ToString(dr("cDCFStatus")).Trim.ToUpper() <> Discrepancy_AutoResolved Then
                                    'Changed for view mode --Pratiksha
                                    'If Me.Session(S_WorkFlowStageId) >= Me.HFReviewedWorkFlowId.Value.Trim() Then
                                    'If Me.Session(S_WorkFlowStageId) >= dr("iWorkFlowstageId").ToString() And Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_OnlyView AndAlso _
                                    '    dr("iWorkFlowstageId") = Me.Session(S_WorkFlowStageId) - 10 Then
                                    '    PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))
                                    'ElseIf Me.Session(S_WorkFlowStageId) = WorkFlowStageId_DataEntry Then
                                    '    PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))
                                    'End If

                                    'ElseIf Me.Session(S_WorkFlowStageId) = WorkFlowStageId_DataValidator Then
                                    '    PlaceMedEx.Controls.Add(GetEditButton("Edit", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo")))
                                    '    PlaceMedEx.Controls.Add(GetUpdateButton("Update", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo")))

                                    'ElseIf Convert.ToString(Me.ViewState(VS_ReviewFlag)).ToUpper() <> "YES" AndAlso _
                                    '    Me.Session(S_WorkFlowStageId) >= WorkFlowStageId_FirstReview Then
                                    '    PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))
                                    '            ElseIf Not dr("iWorkFlowstageId") Is Nothing AndAlso _
                                    '                dr("iWorkFlowstageId").ToString() < Me.Session(S_WorkFlowStageId) AndAlso _
                                    '                dr("iWorkFlowstageId") = Me.Session(S_WorkFlowStageId) - 10 Then
                                    '                PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))

                                    '                'ElseIf Me.Session(S_WorkFlowStageId) = WorkFlowStageId_FinalReviewAndLock AndAlso _
                                    '                '    Convert.ToString(Me.HFCRFDtlLockStatus.Value).Trim.ToUpper() <> CRFHdr_Locked Then
                                    '                '    PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))

                                    '            ElseIf Me.Session(S_WorkFlowStageId) = WorkFlowStageId_DataEntry AndAlso _
                                    '                Convert.ToString(Me.ViewState(VS_ReviewFlag)).ToUpper() <> "YES" AndAlso _
                                    '                dr("iWorkFlowstageId").ToString() < Me.Session(S_WorkFlowStageId) Then
                                    '                PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))

                                    '            ElseIf Me.Session(S_WorkFlowStageId) >= dr("iWorkFlowstageId").ToString() AndAlso _
                                    '            Me.Session(S_WorkFlowStageId) = WorkFlowStageId_DataEntry AndAlso _
                                    '                    dr("cCRFDtlDatastatus").ToString() <> CRF_DataEntryPending AndAlso _
                                    '                    dr("cCRFDtlDatastatus").ToString() <> CRF_DataEntry Then
                                    '                PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))
                                    'End If
                                    'End If

                                End If

                            End If

                            'ElseIf Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_OnlyView Then
                            '    PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))
                            'End If
                            '*****************
                        End If

                        'PlaceMedEx.Controls.Add(GetAudittrailButton("AuditTrail", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("iTranNo"), dr("nCRFDtlNo")))
                        'Showing image information for identifying them
                        Me.trimageinformation.Visible = True
                        '***************************

                    End If

                    'Else before it elseif -Megha
                    If Me.ddlRepeatNo.SelectedIndex <> 0 AndAlso _
                            (CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_DataEntry) Then

                        If Convert.ToString(dr("vMedExType")).Trim.ToUpper() <> "LABEL" Then
                            'Condition changed to solve problem of view mode -- Pratiksha
                            'If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_OnlyView Or dataMode_String <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                            'If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

                            If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_OnlyView Then

                                If Not Me.HFCRFDtlLockStatus.Value Is Nothing AndAlso _
                                       Not Convert.ToString(Me.HFCRFDtlLockStatus.Value).Trim.ToUpper() = CRFHdr_Locked Then

                                    If Convert.ToString(dr("vDefaultValue")).Trim() <> "" Or dr("iTranNo") > 1 Then

                                        If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_DataEntry Then

                                            objelement.Attributes.Add("disabled", "true")
                                            If (Me.Session(S_EDCUser).ToString() = EDCUser And _
                                                (Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "DATETIME" Or Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "TIME")) Then
                                                'Else
                                                '    PlaceMedEx.Controls.Add(GetEditButton("Edit", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo")))
                                                '    PlaceMedEx.Controls.Add(GetUpdateButton("Update", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo")))
                                            End If
                                        End If

                                    End If
                                End If
                            End If
                        End If
                    End If
                    'End If

                    If Convert.ToString(dr("vMedExType")).Trim.ToUpper() <> "LABEL" Then
                        If dt.Columns.Contains("nCRFDtlNo") Then
                            PlaceMedEx.Controls.Add(GetAudittrailButton("AuditTrail", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("iTranNo"), dr("nCRFDtlNo"), dr("vModificationRemark")))
                        End If
                        If Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "COMBOGLOBALDICTIONARY" Then
                            ' Added to have comboglobal available for medical coder
                            objelement.Attributes.Add("disabled", "")
                            If Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_MedicalCoding Then
                                objelement.Attributes.Add("disabled", "true")
                            End If
                        End If
                    End If

                    PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                    '**********For User's readibility**************
                    PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" height=""2px"" ALIGN=LEFT style=""BACKGROUND-COLOR: #FFFFFF"">"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<Td colspan=""3"" style=""white-space: nowrap; vertical-align:middle"">"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                    PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" height=""1px"" ALIGN=LEFT style=""BACKGROUND-COLOR: #1560a1"">")) '#FFC300
                    PlaceMedEx.Controls.Add(New LiteralControl("<Td colspan=""3"" style=""white-space: nowrap; vertical-align:middle"">"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                    PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" height=""2px"" ALIGN=LEFT style=""BACKGROUND-COLOR: #FFFFFF"">"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<Td colspan=""3"" style=""white-space: nowrap; vertical-align:middle"">"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
                    '****************************************

                Next dr

                i += 1
                PlaceMedEx.Controls.Add(New LiteralControl("</Table>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</Div>"))

            Next drGroup

            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
            PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
            PlaceMedEx.Controls.Add(New LiteralControl("</Table>"))
            Me.Session("PlaceMedEx") = PlaceMedEx.Controls

            dt_MedExMst = New DataTable
            dt_MedExMst = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable)

            Me.trReviewCompleted.Style.Add("display", "block")
            If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_DataEntry AndAlso _
                Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_FirstReview Then
                'Me.BtnSave.Visible = False
                'Me.btnSaveAndContinue.Visible = False
            End If
            If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_DataEntry Or _
                               Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_OnlyView Then 'Or Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_DataValidator Then
                Me.trReviewCompleted.Style.Add("display", "none")
            End If

            If Convert.ToString(Me.ViewState(VS_ReviewFlag)).Trim.ToUpper() = "YES" Then 'Or Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.trReviewCompleted.Style.Add("display", "none")
            End If

            Me.imgShow.Attributes.Add("onmouseover", "$('#" + Me.canal.ClientID + "').toggle('medium');")
            Me.canal.Attributes.Add("onmouseleave", "$('#" + Me.canal.ClientID + "').toggle('medium');")

            'Me.imgActivityLegends.Attributes.Add("onmouseover", "$('#" + Me.divActivityLegends.ClientID + "').toggle('medium');")
            'Me.divActivityLegends.Attributes.Add("onmouseleave", "$('#" + Me.divActivityLegends.ClientID + "').toggle('medium');")

            If Not FillEditDropDown() Then
                Me.objCommon.ShowAlert("Error While Filling Edit Drop Down", Me.Page)
                Exit Function
            End If

            '*************Electronic Signature***************************

            If Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_FinalReviewAndLock AndAlso Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                Me.trName.Visible = False
                Me.trDesignation.Visible = False
                Me.trRemarks.Visible = False
                Me.divAuthentication.Style.Add("height", "150px")
            ElseIf Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_SecondReview AndAlso Me.Session(S_ScopeNo) = Scope_BABE Then
                Me.trName.Visible = False
                Me.trDesignation.Visible = False
                Me.trRemarks.Visible = False
                Me.divAuthentication.Style.Add("height", "150px")
            End If

            Me.lblSignername.Text = Me.Session(S_FirstName).ToString() + " " + Me.Session(S_LastName).ToString()
            Dim dt_Profiles As New DataTable
            dt_Profiles = CType(Me.Session(S_Profiles), DataTable)
            Dim dv_Profiles As DataView
            dv_Profiles = dt_Profiles.DefaultView
            dv_Profiles.RowFilter = "vUserTypeCode = '" + Me.Session(S_UserType).ToString() + "'"
            Me.lblSignerDesignation.Text = dv_Profiles.ToTable.Rows(0)("vUserTypeName").ToString()
            'Me.lblSignDateTime.Text = Me.objHelp.GetServerDateTime().ToString("dd-MMM-yyyy hh:mm:ss")
            Me.lblSignRemarks.Text = "I attest to the accuracy and integrity of the data being reviewed."

            '*************Electronic Signature***************************

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message.ToString, "")
        End Try
    End Function

#End Region

#Region "Getlable"

    Private Function Getlable(ByVal vlabelName As String, ByVal Id As String, Optional ByVal vFieldType As String = "") As Label
        Dim lab As Label
        lab = New Label
        lab.ID = "Lab" & Id
        lab.Text = vlabelName.Trim()
        lab.SkinID = "lblDisplay"
        lab.ForeColor = System.Drawing.Color.FromName("Navy")
        If vFieldType.ToUpper.Trim() = "IMPORT" Then
            lab.Visible = False
        End If
        Getlable = lab
    End Function

    Private Function GetlableWithHistory(ByVal vlabelName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal Repeatition As Integer) As Label
        Dim lnk As Label
        lnk = New Label
        lnk.ID = "Lnk" + MedExCode
        lnk.Text = vlabelName.Trim()
        lnk.SkinID = "LabelDisplay"
        lnk.CssClass = "Label"
        lnk.Style.Add("word-wrap", "break-word")
        lnk.Style.Add("white-space", "")
        'Commented because applicable when displaying attributes in table format
        'If Repeatition <> 1 Then
        '    lnk.Style.Add("display", "none")
        'End If
        GetlableWithHistory = lnk
    End Function

#End Region

#Region "GetButtons"

    Private Function GetButtons(ByVal BtnName As String, ByVal Id As String) As Boolean
        Dim Btn As Button
        Dim index As Integer
        Dim StrGroupCode_arry() As String
        Dim StrGroupDesc_arry() As String
        Try
            StrGroupCode_arry = Id.Split(",")
            StrGroupDesc_arry = BtnName.Split(",")
            For index = 0 To StrGroupCode_arry.Length - 1
                Btn = New Button
                Btn.ID = "Btn" & StrGroupCode_arry(index)
                Btn.Text = " " & StrGroupDesc_arry(index).Trim() & " "

                If StrGroupDesc_arry(index).Trim().Length > 30 Then
                    Btn.Text = " " & StrGroupDesc_arry(index).Substring(0, 30).Trim() & "... "
                End If

                Btn.ToolTip = StrGroupDesc_arry(index).Trim()
                Btn.CssClass = "buttonForTabActive"

                If Convert.ToString(Me.HFActivateTab.Value).Trim() <> "" Or _
                        Convert.ToString(Me.Session(S_SelectedTab)).Trim() <> "" Then

                    If Convert.ToString(Me.HFActivateTab.Value).Trim() = StrGroupCode_arry(index) Then
                        Btn.CssClass = "buttonForTabInActive"
                    ElseIf Convert.ToString(Me.Session(S_SelectedTab)).Trim() = StrGroupCode_arry(index) Then
                        Btn.CssClass = "buttonForTabInActive"
                    End If

                Else
                    If index = 0 Then
                        Btn.CssClass = "buttonForTabInActive"
                    End If
                End If

                Btn.OnClientClick = "return DisplayDiv('" & StrGroupCode_arry(index) & "','" & Id & "');"
                PlaceMedEx.Controls.Add(Btn)
                PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
            Next
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Function

#End Region

#Region "GetObject"

    Private Function GetObject(ByVal vFieldType As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal Id As String, ByVal vMedExValues As String, _
                                ByVal dtValues As String, Optional ByVal Validation As String = "", _
                                Optional ByVal length As String = "", Optional ByVal AlertonValue As String = "", _
                                Optional ByVal AlertMsg As String = "", _
                                Optional ByVal HighRange As String = "0", _
                                Optional ByVal LowRange As String = "0", _
                                Optional ByVal RefTable As String = "", _
                                Optional ByVal RefColumn As String = "", _
                                Optional ByVal IsNotNull As String = "") As Object
        Dim txt As TextBox
        Dim ddl As DropDownList
        Dim FileBro As FileUpload
        Dim RBL As RadioButtonList
        Dim CBL As CheckBoxList
        Dim lbl As Label

        Select Case vFieldType.ToUpper.Trim()
            Case "TEXTBOX"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                If IsNotNull.ToUpper() = "Y" Then
                    txt.CssClass = "Required textBox"
                End If

                txt.Text = dtValues
                txt.Attributes.Add("title", dtValues)
                txt.Enabled = False

                'HighRange = IIf(HighRange = "", "0", HighRange)
                'LowRange = IIf(LowRange = "", "0", LowRange)

                'Dim checktype As String = ""
                'Dim msg As String = ""

                'If Validation = "" Or Validation = "NA" Then
                '    checktype = "0"
                '    msg = "No Validation"
                'ElseIf Validation = GeneralModule.Val_AN Then
                '    checktype = GeneralModule.Validation_Alphanumeric.ToString()
                '    msg = "Please Enter AlphaNumeric Value"
                'ElseIf Validation = GeneralModule.Val_NU Then
                '    checktype = GeneralModule.Validation_Numeric.ToString()
                '    msg = "Please Enter Numeric or Blank Value"
                'ElseIf Validation = GeneralModule.Val_IN Then
                '    checktype = GeneralModule.Validation_Integer.ToString()
                '    msg = "Please Enter Integer or Blank Value"
                'ElseIf Validation = GeneralModule.Val_AL Then
                '    checktype = GeneralModule.Validation_Alphabet.ToString()
                '    msg = "Please Enter Alphabets only"
                'ElseIf Validation = GeneralModule.Val_NNI Then
                '    checktype = GeneralModule.Validation_NotNullInteger.ToString()
                '    msg = "Please Enter Integer Value"
                'ElseIf Validation = GeneralModule.Val_NNU Then
                '    checktype = GeneralModule.Validation_NotNullNumeric.ToString()
                '    msg = "Please Enter Numeric Value"
                'End If

                'If AlertonValue.Trim() <> "" OrElse length.Trim() <> "" Then
                '    checktype = "0"
                'End If

                'txt.Attributes.Add("onblur", "ValidateTextbox('" & checktype & "',this,'" & msg & "'," & _
                '                           HighRange & "," & LowRange & ",'" & AlertonValue & "','" & AlertMsg.Replace("'", "\'") & "','" & length & "','" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")


                'If Id = GeneralModule.Medex_Oral_Body_Temperature_F Then

                '    txt.Attributes.Add("onblur", "F2C('" & GeneralModule.Medex_Oral_Body_Temperature_F.Trim() & "','" & GeneralModule.Medex_Oral_Body_Temperature_C.Trim() & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")

                'End If
                ''************************************
                ''added on 24-Apr-2010 by Deepak Singh
                'If Id = GeneralModule.medex_DosingLabel Then
                '    txt.Attributes.Add("onchange", "SetDosingTime('00650','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "')")
                'End If

                'txt.Attributes.Add("onfocus", "SetValue(this.id,this.value);")
                GetObject = txt


            Case "COMBOBOX"
                Dim Arrvalue() As String = Nothing
                Dim i As Integer
                Dim ds_ddl As New DataSet
                Dim estr As String = ""

                ddl = New DropDownList
                ddl.ID = Id
                ddl.CssClass = "dropDownList"
                ddl.Enabled = False

                If IsNotNull.ToUpper() = "Y" Then
                    ddl.CssClass = "Required dropDownList"
                End If

                If RefTable.Trim() <> "" Then

                    If Not Me.objHelp.GetFieldsOfTable(RefTable.Trim(), RefColumn.Trim(), "", ds_ddl, estr) Then
                        Me.objCommon.ShowAlert(estr, Me.Page())
                        Return Nothing
                    End If
                    ds_ddl.Tables(0).DefaultView.Sort = RefColumn
                    ddl.DataSource = ds_ddl.Tables(0).DefaultView.ToTable()
                    ddl.DataTextField = RefColumn
                    ddl.DataValueField = RefColumn
                    ddl.DataBind()

                Else

                    Arrvalue = Split(vMedExValues, ",")
                    For i = 0 To Arrvalue.Length - 1
                        ddl.Items.Add(New ListItem(Arrvalue(i).Trim(), Arrvalue(i).Trim()))
                        ddl.Items(i).Attributes.Add("title", Arrvalue(i).Trim())
                    Next

                    If Not dtValues = "" Then
                        ddl.SelectedValue = dtValues.Trim()
                        ddl.Attributes.Add("title", dtValues)
                    End If

                End If

                'ddl.Attributes.Add("onblur", "checkddlNotNull('" & ddl.ClientID & "','" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")

                ''If AlertonValue.Trim() <> "" Then
                'ddl.Attributes.Add("onchange", "ddlAlerton('" & ddl.ClientID & "','" & AlertonValue & "','" & AlertMsg.Replace("'", "\'") & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")
                ''End If

                'ddl.Attributes.Add("onfocus", "SetValue('" & ddl.ClientID & "','" + Convert.ToString(dtValues).Trim() + "');")
                GetObject = ddl

            Case "RADIO"
                Dim Arrvalue() As String = Nothing
                Dim i As Integer
                Dim ds_Radio As New DataSet
                Dim estr As String = ""

                RBL = New RadioButtonList
                RBL.ID = Id
                RBL.EnableViewState = True
                RBL.Enabled = False

                If IsNotNull.ToUpper() = "Y" Then
                    RBL.CssClass = "Required"
                End If

                If RefTable.Trim() <> "" Then

                    If Not Me.objHelp.GetFieldsOfTable(RefTable.Trim(), RefColumn.Trim(), "", ds_Radio, estr) Then
                        Me.objCommon.ShowAlert(estr, Me.Page())
                        Return Nothing
                    End If

                    ds_Radio.Tables(0).DefaultView.Sort = RefColumn
                    RBL.DataSource = ds_Radio.Tables(0).DefaultView.ToTable()
                    RBL.DataTextField = RefColumn
                    RBL.DataValueField = RefColumn
                    RBL.DataBind()

                Else
                    Arrvalue = Split(vMedExValues, ",")
                    For i = 0 To Arrvalue.Length - 1
                        RBL.Items.Add(New ListItem(Arrvalue(i).Trim(), Arrvalue(i).Trim().ToUpper()))
                        'RBL.Items(i).Attributes.Add("onblur", "checkRBLNotNull('" & RBL.ClientID & "','" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")
                        'RBL.Items(i).Attributes.Add("onfocus", "SetValue('" & RBL.ClientID & "','" + Convert.ToString(dtValues).Trim() + "');")
                        'RBL.Items(i).Attributes.Add("onclick", "Alerton('" & RBL.ClientID & "','" & AlertonValue & "','" & AlertMsg.Replace("'", "\'") & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "','" + Convert.ToString(dtValues).Trim() + "');")
                    Next
                    If Not dtValues = "" Then
                        RBL.SelectedValue = dtValues.Trim().ToUpper()
                    End If

                End If

                RBL.RepeatDirection = RepeatDirection.Horizontal
                RBL.RepeatColumns = 3

                'RBL.Attributes.Add("onblur", "checkRBLNotNull('" & RBL.ClientID & "','" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")

                ''If AlertonValue.Trim() <> "" Then
                ''    RBL.Attributes.Add("onclick", "Alerton('" & RBL.ClientID & "','" & AlertonValue & "','" & AlertMsg.Replace("'", "\'") & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")
                ''End If

                'RBL.Attributes.Add("ondblclick", "RemoveSelection('" & RBL.ClientID & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")
                'RBL.Attributes.Add("onfocus", "SetValue('" & RBL.ClientID & "','" + Convert.ToString(dtValues).Trim() + "');")

                GetObject = RBL

            Case "CHECKBOX"
                Dim Arrvalue() As String = Nothing
                Dim Defvalue() As String = Nothing
                Dim i As Integer
                Dim ds_Check As New DataSet
                Dim estr As String = ""

                CBL = New CheckBoxList
                CBL.ID = Id
                CBL.EnableViewState = True
                CBL.Enabled = False

                If IsNotNull.ToUpper() = "Y" Then
                    CBL.CssClass = "Required"
                End If

                If RefTable.Trim() <> "" Then

                    If Not Me.objHelp.GetFieldsOfTable(RefTable.Trim(), RefColumn.Trim(), "", ds_Check, estr) Then
                        Me.objCommon.ShowAlert(estr, Me.Page())
                        Return Nothing
                    End If

                    ds_Check.Tables(0).DefaultView.Sort = RefColumn
                    CBL.DataSource = ds_Check.Tables(0).DefaultView.ToTable()
                    CBL.DataTextField = RefColumn
                    CBL.DataValueField = RefColumn
                    CBL.DataBind()

                Else

                    Arrvalue = Split(vMedExValues, ",")

                    For i = 0 To Arrvalue.Length - 1
                        CBL.Items.Add(New ListItem(Arrvalue(i).Trim(), Arrvalue(i).Trim()))
                    Next

                End If

                CBL.ClearSelection()
                If Not dtValues = "" Then
                    Defvalue = Split(dtValues, ",")
                    Dim SetValue As String = ""

                    For i = 0 To Defvalue.Length - 1
                        For Each item As ListItem In CBL.Items
                            If item.Value.Trim().ToUpper() = Defvalue(i).Trim().ToUpper() Then
                                item.Selected = True
                                If SetValue.Trim() <> "" Then
                                    SetValue += "##"
                                End If
                                SetValue += Defvalue(i).Trim()
                                'Exit For
                            End If
                            'item.Attributes.Add("onfocus", "SetValue('" & CBL.ClientID & "','" + SetValue + "');")
                            'item.Attributes.Add("onblur", "checkCBLNotNull('" & CBL.ClientID & "','" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")
                            'item.Attributes.Add("onclick", "AlertonCheckBox(this,'" & AlertonValue & "','" & AlertMsg.Replace("'", "\'") & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "','" + SetValue + "');")
                        Next item
                    Next i

                    'Else
                    '    For Each item As ListItem In CBL.Items
                    '        'item.Attributes.Add("onfocus", "SetValue('" & CBL.ClientID & "','');")
                    '        'item.Attributes.Add("onblur", "checkCBLNotNull('" & CBL.ClientID & "','" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")
                    '        'item.Attributes.Add("onclick", "AlertonCheckBox(this,'" & AlertonValue & "','" & AlertMsg.Replace("'", "\'") & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "','');")
                    '    Next item

                End If

                CBL.RepeatDirection = RepeatDirection.Horizontal
                CBL.RepeatColumns = 3

                '  CBL.Attributes.Add("onblur", "checkCBLNotNull('" & CBL.ClientID & "','" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")

                'If AlertonValue.Trim() <> "" Then
                '    CBL.Attributes.Add("onclick", "AlertonCheckBox(this,'" & AlertonValue & "','" & AlertMsg.Replace("'", "\'") & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")
                'End If

                GetObject = CBL

            Case "FILE"
                FileBro = New FileUpload
                FileBro.ID = "FU" + Id
                FileBro.EnableViewState = True
                FileBro.CssClass = "textBox"
                FileBro.Enabled = False

                If IsNotNull.ToUpper() = "Y" Then
                    FileBro.CssClass = "Required textBox"
                End If

                GetObject = FileBro

            Case "TEXTAREA"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                If IsNotNull.ToUpper() = "Y" Then
                    txt.CssClass = "Required textBox"
                End If

                txt.Attributes.Add("title", dtValues)
                txt.Text = dtValues
                txt.TextMode = TextBoxMode.MultiLine
                txt.Width = 275
                txt.Height = 70
                txt.Enabled = False
                'HighRange = IIf(HighRange = "", "0", HighRange)
                'LowRange = IIf(LowRange = "", "0", LowRange)

                'Dim checktype As String = ""
                'Dim msg As String = ""

                'If Validation = "" Or Validation = "NA" Then
                '    checktype = "0"
                '    msg = "No Validation"
                'ElseIf Validation = GeneralModule.Val_AN Then
                '    checktype = GeneralModule.Validation_Alphanumeric.ToString()
                '    msg = "Please Enter AlphaNumeric Value"
                'ElseIf Validation = GeneralModule.Val_NU Then
                '    checktype = GeneralModule.Validation_Numeric.ToString()
                '    msg = "Please Enter Numeric or Blank Value"
                'ElseIf Validation = GeneralModule.Val_IN Then
                '    checktype = GeneralModule.Validation_Integer.ToString()
                '    msg = "Please Enter Integer or Blank Value"
                'ElseIf Validation = GeneralModule.Val_AL Then
                '    checktype = GeneralModule.Validation_Alphabet.ToString()
                '    msg = "Please Enter Alphabets only"
                'ElseIf Validation = GeneralModule.Val_NNI Then
                '    checktype = GeneralModule.Validation_NotNullInteger.ToString()
                '    msg = "Please Enter Integer Value"
                'ElseIf Validation = GeneralModule.Val_NNU Then
                '    checktype = GeneralModule.Validation_NotNullNumeric.ToString()
                '    msg = "Please Enter Numeric Value"
                'End If

                'If AlertonValue.Trim() <> "" OrElse length.Trim() <> "" Then
                '    checktype = "0"
                'End If

                'txt.Attributes.Add("onblur", "ValidateTextbox('" & checktype & "',this,'" & msg & "'," & _
                '                           HighRange & "," & LowRange & ",'" & AlertonValue & "','" & AlertMsg.Replace("'", "\'") & "','" & length & "','" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")
                'txt.Attributes.Add("onfocus", "SetValue(this.id,this.value);")

                GetObject = txt

            Case "DATETIME"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.Enabled = False
                ''Enhancement in EDC
                'If Me.Session(S_EDCUser) = EDCUser Then
                '    'txt.Enabled = False
                '    'txt.ReadOnly = True
                '    txt.Attributes.Add("ReadOnly", "true")
                'End If
                '''''''''''''''''''

                If IsNotNull.ToUpper() = "Y" Then
                    txt.CssClass = "Required textBox"
                End If

                txt.Text = IIf(dtValues = "", "", dtValues)

                'txt.Attributes.Add("onblur", "DateValidationForCTM(this.value,this,'" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "')")

                If Me.Session(S_ScopeNo) = Scope_BABE Then
                    'If (Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add) Then
                    txt.Text = IIf(dtValues = "", System.DateTime.Now.ToString("dd-MMM-yyyy"), dtValues)
                    'End If
                    If dtValues = "" AndAlso Me.HFSubjectId.Value.Trim() = "0000" Then
                        txt.Text = ""
                    End If
                    'txt.Attributes.Add("onblur", "DateValidation(this.value,this,'" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "')")
                End If

                txt.Attributes.Add("title", txt.Text)
                'txt.Attributes.Add("onfocus", "SetValue(this.id,this.value);")
                GetObject = txt

            Case "TIME"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.Enabled = False
                ''Enhancement in EDC
                'If Me.Session(S_EDCUser) = EDCUser Then
                '    'txt.Enabled = False
                '    'txt.ReadOnly = True
                '    txt.Attributes.Add("ReadOnly", "true")
                'End If
                ''''''''''''''''''''''''''''''
                If IsNotNull.ToUpper() = "Y" Then
                    txt.CssClass = "Required textBox"
                End If

                txt.Text = dtValues
                txt.Attributes.Add("title", dtValues)

                'txt.Attributes.Add("onblur", "AutoTimeConvert(this.value,this,'" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "')")
                'txt.Attributes.Add("onfocus", "SetValue(this.id,this.value);")

                GetObject = txt

                ''''''''''''''''''''''''''''''''''
            Case "ASYNCDATETIME"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.Enabled = False

                If IsNotNull.ToUpper() = "Y" Then
                    txt.CssClass = "Required textBox"
                End If

                txt.Text = IIf(dtValues = "", "", dtValues)

                'txt.Attributes.Add("onblur", "DateValidationForCTM(this.value,this,'" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "')")

                If Me.Session(S_ScopeNo) = Scope_BABE Then
                    ' If (Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add) Then
                    txt.Text = IIf(dtValues = "", System.DateTime.Now.ToString("dd-MMM-yyyy"), dtValues)
                    'End If
                    If dtValues = "" AndAlso Me.HFSubjectId.Value.Trim() = "0000" Then
                        txt.Text = ""
                    End If
                    'txt.Attributes.Add("onblur", "DateValidation(this.value,this,'" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "')")
                End If

                txt.Attributes.Add("title", txt.Text)
                'txt.Attributes.Add("onfocus", "SetValue(this.id,this.value);")
                GetObject = txt

            Case "ASYNCTIME"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.Enabled = False

                If IsNotNull.ToUpper() = "Y" Then
                    txt.CssClass = "Required textBox"
                End If

                txt.Text = dtValues
                txt.Attributes.Add("title", dtValues)

                'txt.Attributes.Add("onblur", "AutoTimeConvert(this.value,this,'" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "')")
                'txt.Attributes.Add("onfocus", "SetValue(this.id,this.value);")

                GetObject = txt

                ''''''''''''''''''''''''''''''''''

            Case "IMPORT"
                'Hf = New HiddenField
                'Hf.ID = Id
                'Hf.Value = dtValues.Trim()

                'GetObject = Hf
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.Enabled = False
                If IsNotNull.ToUpper() = "Y" Then
                    txt.CssClass = "Required textBox"
                End If

                txt.Text = dtValues
                txt.Attributes.Add("title", dtValues)
                GetObject = txt

            Case "COMBOGLOBALDICTIONARY"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.Enabled = False
                If IsNotNull.ToUpper() = "Y" Then
                    txt.CssClass = "Required textBox"
                End If

                txt.Text = dtValues
                txt.Attributes.Add("title", dtValues)

                Is_ComboGlobalDictionary = True

                'HighRange = IIf(HighRange = "", "0", HighRange)
                'LowRange = IIf(LowRange = "", "0", LowRange)

                'Dim checktype As String = ""
                'Dim msg As String = ""

                'If Validation = "" Or Validation = "NA" Then
                '    checktype = "0"
                '    msg = "No Validation"
                'ElseIf Validation = GeneralModule.Val_AN Then
                '    checktype = GeneralModule.Validation_Alphanumeric.ToString()
                '    msg = "Please Enter AlphaNumeric Value"
                'ElseIf Validation = GeneralModule.Val_NU Then
                '    checktype = GeneralModule.Validation_Numeric.ToString()
                '    msg = "Please Enter Numeric or Blank Value"
                'ElseIf Validation = GeneralModule.Val_IN Then
                '    checktype = GeneralModule.Validation_Integer.ToString()
                '    msg = "Please Enter Integer or Blank Value"
                'ElseIf Validation = GeneralModule.Val_AL Then
                '    checktype = GeneralModule.Validation_Alphabet.ToString()
                '    msg = "Please Enter Alphabets only"
                'ElseIf Validation = GeneralModule.Val_NNI Then
                '    checktype = GeneralModule.Validation_NotNullInteger.ToString()
                '    msg = "Please Enter Integer Value"
                'ElseIf Validation = GeneralModule.Val_NNU Then
                '    checktype = GeneralModule.Validation_NotNullNumeric.ToString()
                '    msg = "Please Enter Numeric Value"
                'End If

                'If AlertonValue.Trim() <> "" OrElse length.Trim() <> "" Then
                '    checktype = "0"
                'End If

                'txt.Attributes.Add("onblur", "ValidateTextbox('" & checktype & "',this,'" & msg & "'," & _
                '                           HighRange & "," & LowRange & ",'" & AlertonValue & "','" & AlertMsg.Replace("'", "\'") & "','" & length & "','" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")

                'txt.Attributes.Add("onfocus", "SetValue(this.id,this.value);")

                GetObject = txt

            Case "FORMULA"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.Enabled = False
                If IsNotNull.ToUpper() = "Y" Then
                    txt.CssClass = "Required textBox"
                End If

                txt.Text = dtValues
                txt.Attributes.Add("title", dtValues)

                'HighRange = IIf(HighRange = "", "0", HighRange)
                'LowRange = IIf(LowRange = "", "0", LowRange)

                'Dim checktype As String = ""
                'Dim msg As String = ""

                'If Validation = "" Or Validation = "NA" Then
                '    checktype = "0"
                '    msg = "No Validation"
                'ElseIf Validation = GeneralModule.Val_AN Then
                '    checktype = GeneralModule.Validation_Alphanumeric.ToString()
                '    msg = "Please Enter AlphaNumeric Value"
                'ElseIf Validation = GeneralModule.Val_NU Then
                '    checktype = GeneralModule.Validation_Numeric.ToString()
                '    msg = "Please Enter Numeric or Blank Value"
                'ElseIf Validation = GeneralModule.Val_IN Then
                '    checktype = GeneralModule.Validation_Integer.ToString()
                '    msg = "Please Enter Integer or Blank Value"
                'ElseIf Validation = GeneralModule.Val_AL Then
                '    checktype = GeneralModule.Validation_Alphabet.ToString()
                '    msg = "Please Enter Alphabets only"
                'ElseIf Validation = GeneralModule.Val_NNI Then
                '    checktype = GeneralModule.Validation_NotNullInteger.ToString()
                '    msg = "Please Enter Integer Value"
                'ElseIf Validation = GeneralModule.Val_NNU Then
                '    checktype = GeneralModule.Validation_NotNullNumeric.ToString()
                '    msg = "Please Enter Numeric Value"
                'End If

                'If AlertonValue.Trim() <> "" OrElse length.Trim() <> "" Then
                '    checktype = "0"
                'End If

                'txt.Attributes.Add("onblur", "ValidateTextbox('" & checktype & "',this,'" & msg & "'," & _
                '                           HighRange & "," & LowRange & ",'" & AlertonValue & "','" & AlertMsg.Replace("'", "\'") & "','" & length & "','" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")

                'If Id = GeneralModule.Medex_Oral_Body_Temperature_F Then

                '    txt.Attributes.Add("onblur", "F2C('" & GeneralModule.Medex_Oral_Body_Temperature_F.Trim() & "','" & GeneralModule.Medex_Oral_Body_Temperature_C.Trim() & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")

                'End If
                'txt.Attributes.Add("onfocus", "SetValue(this.id,this.value);")

                Is_FormulaEnabled = True
                GetObject = txt

            Case "LABEL"
                lbl = New Label
                lbl.ID = "lbl" + Id
                lbl.EnableViewState = True
                lbl.CssClass = "Label"
                'lbl.Width = "500"
                lbl.Style.Add("word-wrap", "break-word")
                lbl.Style.Add("white-space", "none")
                lbl.Text = vMedExValues.Trim()
                lbl.ToolTip = dtValues.Trim()
                GetObject = lbl

            Case Else
                Return Nothing
        End Select
    End Function

#End Region

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        objCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub

#End Region

    '#Region "AssignValues"

    '    Private Function AssignValues(ByVal SubjectId As String, ByVal WorkspaceId As String, _
    '                                ByVal ActivityId As String, ByVal NodeId As String, _
    '                                ByVal PeriodId As String, _
    '                                ByVal MySubjectNo As String, ByRef DsSave As DataSet) As Boolean
    '        Dim DsCRFHdr As New DataSet
    '        Dim DsCRFDtl As New DataSet
    '        Dim DtCRFHdr As New DataTable
    '        Dim DtCRFDtl As New DataTable
    '        Dim DtCRFSubDtl As New DataTable

    '        Dim objCollection As ControlCollection
    '        Dim objControl As Control
    '        Dim ObjId As String = ""
    '        Dim dr As DataRow
    '        Dim TranNo As Integer = 0
    '        Dim Wstr As String = ""
    '        Dim estr As String = ""
    '        Dim dsNodeInfo As New DataSet
    '        Dim NodeIndex As String = ""
    '        Dim ds_DCF As New DataSet
    '        Dim dt_DCF As New DataTable

    '        Dim ControlDesc As String = String.Empty
    '        Dim ControlId As String = String.Empty
    '        Try

    '            Wstr = "vWorkSpaceId='" & WorkspaceId & "' and iNodeId='" & NodeId & "'" & _
    '                    " and vActivityId='" & ActivityId & "'"

    '            If Not Me.objHelp.getWorkSpaceNodeDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                            dsNodeInfo, estr) Then
    '                Me.objCommon.ShowAlert("Error while getting NodeIndex", Me)
    '                Return False
    '            End If
    '            NodeIndex = dsNodeInfo.Tables(0).Rows(0)("iNodeIndex").ToString()

    '            objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection) 'Me.PlaceMedEx.Controls 

    '            DtCRFHdr = CType(Me.ViewState(VS_DtCRFHdr), DataTable)
    '            DtCRFDtl = CType(Me.ViewState(VS_DtCRFDtl), DataTable)
    '            DtCRFSubDtl = CType(Me.ViewState(VS_DtCRFSubDtl), DataTable)

    '            '*********Checking MedEx values on 25-Dec-2009******************
    '            If Not Me.objHelp.GetDCFMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
    '                                    ds_DCF, estr) Then
    '                Exit Function
    '            End If
    '            Me.ViewState(VS_DtDCF) = ds_DCF.Tables(0).Copy()
    '            '*************************************

    '            If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
    '                If Not Me.objHelp.GetCRFHdr("nCRFHdrNo=" & CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows(0)("nCRFHdrNo"), _
    '                                                    WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                            DsCRFHdr, estr) Then
    '                    Exit Function
    '                End If
    '                DtCRFHdr = DsCRFHdr.Tables(0)

    '            Else

    '                DtCRFHdr.Clear()
    '                dr = DtCRFHdr.NewRow
    '                'nCRFHdrNo, vWorkSpaceId,dStartDate,iPeriod,iNodeId,iNodeIndex,vActivityId,cLockStatus
    '                dr("nCRFHdrNo") = 1
    '                If DtCRFHdr.Rows.Count > 0 Then
    '                    dr("nCRFHdrNo") = DtCRFHdr.Rows(0)("nCRFHdrNo")
    '                End If

    '                dr("vWorkSpaceId") = WorkspaceId
    '                dr("dStartDate") = System.DateTime.Now
    '                dr("iPeriod") = PeriodId
    '                dr("iNodeId") = NodeId
    '                dr("iNodeIndex") = NodeIndex
    '                dr("vActivityId") = ActivityId
    '                dr("cLockStatus") = "U" 'cLockStatus
    '                dr("iModifyBy") = Me.Session(S_UserID)
    '                dr("cStatusIndi") = "N"
    '                'dr.AcceptChanges()
    '                DtCRFHdr.Rows.Add(dr)
    '                DtCRFHdr.TableName = "CRFHdr"
    '                DtCRFHdr.AcceptChanges()
    '            End If

    '            If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
    '                If Not Me.objHelp.GetCRFDtl("nCRFDtlNo=" & CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows(0)("nCRFDtlNo"), _
    '                                                    WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                            DsCRFDtl, estr) Then
    '                    Exit Function
    '                End If
    '                DtCRFDtl = DsCRFDtl.Tables(0)
    '                For Each dr In DtCRFDtl.Rows
    '                    dr("cDataStatus") = CRF_DataEntryCompleted
    '                Next
    '                DtCRFDtl.AcceptChanges()

    '                If CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_DataEntry Then
    '                    For Each dr In DtCRFDtl.Rows
    '                        dr("cDataStatus") = CRF_DataEntry
    '                    Next
    '                    DtCRFDtl.AcceptChanges()
    '                End If

    '            Else

    '                DtCRFDtl.Clear()
    '                dr = DtCRFDtl.NewRow
    '                'nCRFDtlNo,nCRFHdrNo,iRepeatNo,dRepeatationDate,vSubjectId,iMySubjectNo,cLockStatus,iWorkFlowstageId
    '                dr("nCRFDtlNo") = 1
    '                If DtCRFDtl.Rows.Count > 0 Then
    '                    dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
    '                End If
    '                dr("nCRFHdrNo") = DtCRFHdr.Rows(0)("nCRFHdrNo").ToString.Trim()
    '                dr("iRepeatNo") = 1 'iRepeatNo
    '                dr("dRepeatationDate") = System.DateTime.Now
    '                dr("vSubjectId") = SubjectId
    '                dr("iMySubjectNo") = MySubjectNo
    '                dr("cLockStatus") = "U" 'cLockStatus
    '                dr("iWorkFlowstageId") = Me.Session(S_WorkFlowStageId)
    '                dr("iModifyBy") = Me.Session(S_UserID)
    '                dr("cStatusIndi") = "N"

    '                dr("cDataStatus") = CRF_DataEntryCompleted
    '                If CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_DataEntry Then
    '                    dr("cDataStatus") = CRF_DataEntry
    '                End If

    '                'dr.AcceptChanges()
    '                DtCRFDtl.Rows.Add(dr)
    '                DtCRFDtl.TableName = "CRFDtl"
    '                DtCRFDtl.AcceptChanges()
    '            End If

    '            DtCRFSubDtl.Clear()
    '            'For Detail Table

    '            For Each objControl In objCollection
    '                'nCRFSubDtlNo,nCRFDtlNo,iTranNo,vMedExCode,dMedExDatetime,vMedExResult,vModificationRemark

    '                If Not objControl.ID Is Nothing Then 'Commented for Tabulor Format'AndAlso Convert.ToString(objControl.ID).Trim().Contains("R1000")

    '                    If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.Label" AndAlso _
    '                               objControl.ID.ToString.Trim().Contains("Lnk") Then
    '                        Dim objLbl As Label = CType(objControl, Label)
    '                        ControlId = objControl.ID.ToString.Replace("Lnk", "")
    '                        ControlDesc = objLbl.Text
    '                    End If

    '                    If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.TextBox" Then
    '                        TranNo += 1
    '                        If objControl.ID.ToString.Contains("txt") Then
    '                            ObjId = objControl.ID.ToString.Replace("txt", "")
    '                        Else
    '                            ObjId = objControl.ID.ToString.Trim()
    '                        End If

    '                        dr = DtCRFSubDtl.NewRow
    '                        dr("nCRFSubDtlNo") = TranNo
    '                        dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
    '                        dr("iTranNo") = TranNo
    '                        dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
    '                        'If ControlId.ToUpper() = ObjId.ToUpper() AndAlso ControlDesc.Trim() <> "" Then
    '                        dr("vMedExDesc") = ControlDesc.Trim.Substring(0, ControlDesc.Trim.Length - 1)
    '                        'End If
    '                        dr("dMedExDateTime") = System.DateTime.Now
    '                        dr("vMedexResult") = Request.Form(objControl.ID) 'CType(objControl, TextBox).Text CType(Me.FindControl(obj.GetId), TextBox).Text
    '                        dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
    '                        dr("iModifyBy") = Me.Session(S_UserID)
    '                        dr("cStatusIndi") = "N"

    '                        If Me.CheckDiscrepancy(objControl, ObjId, Request.Form(objControl.ID), "S") Then
    '                            dr("cStatusIndi") = "A"
    '                        End If
    '                        DtCRFSubDtl.Rows.Add(dr)
    '                        DtCRFSubDtl.AcceptChanges()


    '                    ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" Then
    '                        TranNo += 1
    '                        ObjId = objControl.ID.ToString.Trim()
    '                        dr = DtCRFSubDtl.NewRow

    '                        dr = DtCRFSubDtl.NewRow
    '                        dr("nCRFSubDtlNo") = TranNo
    '                        dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
    '                        dr("iTranNo") = TranNo
    '                        dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
    '                        dr("vMedExDesc") = ControlDesc.Trim.Substring(0, ControlDesc.Trim.Length - 1)
    '                        dr("dMedExDateTime") = System.DateTime.Now
    '                        dr("vMedexResult") = Request.Form(objControl.ID) 'CType(objControl, TextBox).Text 'CType(Me.FindControl(obj.GetId), TextBox).Text
    '                        dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
    '                        dr("iModifyBy") = Me.Session(S_UserID)
    '                        dr("cStatusIndi") = "N"
    '                        If Me.CheckDiscrepancy(objControl, ObjId, Request.Form(objControl.ID), "S") Then
    '                            dr("cStatusIndi") = "A"
    '                        End If
    '                        DtCRFSubDtl.Rows.Add(dr)
    '                        DtCRFSubDtl.AcceptChanges()

    '                    ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.FileUpload" Then
    '                        Dim filename As String = ""

    '                        TranNo += 1
    '                        If objControl.ID.ToString.Contains("FU") Then
    '                            ObjId = objControl.ID.ToString.Replace("FU", "")
    '                        Else
    '                            ObjId = objControl.ID.ToString.Trim()
    '                        End If


    '                        If Request.Files(objControl.ID).FileName = "" And _
    '                            Not IsNothing(CType(FindControl("hlnk" + ObjId), HyperLink)) AndAlso _
    '                            CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl <> "" Then

    '                            filename = CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl

    '                        ElseIf Request.Files(objControl.ID).FileName <> "" Then

    '                            filename = "~/" + System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail") + _
    '                                        WorkspaceId + "/" + NodeId + "/" + SubjectId + "/" + Path.GetFileName(Request.Files(objControl.ID).FileName.ToString.Trim())
    '                        End If

    '                        dr = DtCRFSubDtl.NewRow
    '                        dr("nCRFSubDtlNo") = TranNo
    '                        dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
    '                        dr("iTranNo") = TranNo
    '                        dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
    '                        dr("vMedExDesc") = ControlDesc.Trim.Substring(0, ControlDesc.Trim.Length - 1)
    '                        dr("dMedExDateTime") = System.DateTime.Now
    '                        dr("vMedexResult") = filename
    '                        dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
    '                        dr("iModifyBy") = Me.Session(S_UserID)
    '                        dr("cStatusIndi") = "N"
    '                        DtCRFSubDtl.Rows.Add(dr)
    '                        DtCRFSubDtl.AcceptChanges()

    '                    ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.RadioButtonList" Then
    '                        TranNo += 1
    '                        ObjId = objControl.ID.ToString.Trim()
    '                        dr = DtCRFSubDtl.NewRow
    '                        dr("nCRFSubDtlNo") = TranNo
    '                        dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
    '                        dr("iTranNo") = TranNo
    '                        dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
    '                        dr("vMedExDesc") = ControlDesc.Trim.Substring(0, ControlDesc.Trim.Length - 1)
    '                        dr("dMedExDateTime") = System.DateTime.Now
    '                        dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
    '                        dr("iModifyBy") = Me.Session(S_UserID)
    '                        dr("cStatusIndi") = "N"

    '                        'Dim rbl As RadioButtonList = CType(FindControl(objControl.ID), RadioButtonList)
    '                        Dim rbl As RadioButtonList = CType(objControl, RadioButtonList)
    '                        Dim StrChk As String = ""

    '                        For index As Integer = 0 To rbl.Items.Count - 1
    '                            StrChk += IIf(rbl.Items(index).Selected, rbl.Items(index).Value.ToString.Trim() + ",", "")
    '                        Next index

    '                        If StrChk.Trim() <> "" Then
    '                            StrChk = StrChk.Substring(0, StrChk.LastIndexOf(","))
    '                        End If
    '                        dr("vMedexResult") = StrChk
    '                        If Me.CheckDiscrepancy(objControl, ObjId, StrChk, "S") Then
    '                            dr("cStatusIndi") = "A"
    '                        End If
    '                        DtCRFSubDtl.Rows.Add(dr)
    '                        DtCRFSubDtl.AcceptChanges()

    '                    ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.CheckBoxList" Then
    '                        TranNo += 1
    '                        ObjId = objControl.ID.ToString.Trim()
    '                        dr = DtCRFSubDtl.NewRow
    '                        dr("nCRFSubDtlNo") = TranNo
    '                        dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
    '                        dr("iTranNo") = TranNo
    '                        dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
    '                        dr("vMedExDesc") = ControlDesc.Trim.Substring(0, ControlDesc.Trim.Length - 1)
    '                        dr("dMedExDateTime") = System.DateTime.Now
    '                        dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
    '                        dr("iModifyBy") = Me.Session(S_UserID)
    '                        dr("cStatusIndi") = "N"

    '                        Dim chkl As CheckBoxList = CType(objControl, CheckBoxList)
    '                        Dim StrChk As String = ""

    '                        For index As Integer = 0 To chkl.Items.Count - 1
    '                            StrChk += IIf(chkl.Items(index).Selected, chkl.Items(index).Value.ToString.Trim() + ",", "")
    '                        Next index

    '                        If StrChk.Trim() <> "" Then
    '                            StrChk = StrChk.Substring(0, StrChk.LastIndexOf(","))
    '                        End If
    '                        dr("vMedexResult") = StrChk
    '                        If Me.CheckDiscrepancy(objControl, ObjId, StrChk, "S") Then
    '                            dr("cStatusIndi") = "A"
    '                        End If
    '                        DtCRFSubDtl.Rows.Add(dr)
    '                        DtCRFSubDtl.AcceptChanges()

    '                    ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.HiddenField" Then
    '                        TranNo += 1
    '                        ObjId = objControl.ID.ToString.Trim()
    '                        dr = DtCRFSubDtl.NewRow

    '                        '******Adding Header & footer to the document**********************

    '                        Dim ds_WorkSpaceNodeHistory As New DataSet
    '                        Dim filename As String = ""
    '                        Dim versionNo As String = ""
    '                        Wstr = "vWorkSpaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And iNodeId=" + NodeId.Trim() + " And iStageId =" & GeneralModule.Stage_Authorized

    '                        If Not objHelp.GetViewWorkSpaceNodeHistory(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                                                ds_WorkSpaceNodeHistory, estr) Then
    '                            Me.ShowErrorMessage("Error While Getting Data From View_WorkSpaceNodeHistory", estr)
    '                        End If

    '                        If Not ds_WorkSpaceNodeHistory.Tables(0).Rows.Count < 1 Then

    '                            filename = ds_WorkSpaceNodeHistory.Tables(0).Rows(0).Item("vDocPath").ToString()
    '                            versionNo = ds_WorkSpaceNodeHistory.Tables(0).Rows(0).Item("vUSerVersion").ToString()
    '                        End If

    '                        '*****************************************
    '                        ObjId = ObjId.Substring(0, ObjId.IndexOf("R")).Trim()
    '                        If ObjId = GeneralModule.Medex_FilePath.Trim() Then
    '                            dr("vMedexResult") = filename  'File Name from WorkspaceNodeHistory
    '                        ElseIf ObjId = GeneralModule.Medex_DownloadedBy.Trim() Then
    '                            dr("vMedexResult") = Me.Session(S_UserID)
    '                        ElseIf ObjId = GeneralModule.Medex_VersionNo.Trim() Then
    '                            dr("vMedexResult") = versionNo 'Version No from WorkspaceNodeHistory
    '                        ElseIf ObjId = GeneralModule.Medex_ReleaseDate.Trim() Then
    '                            dr("vMedexResult") = Date.Now.Date.ToString("dd-MMM-yyyy") 'Version No from WorkspaceNodeHistory
    '                        Else
    '                            dr("vMedexResult") = Request.Form(objControl.ID) 'CType(objControl, TextBox).Text 'CType(Me.FindControl(obj.GetId), TextBox).Text
    '                        End If

    '                        dr("nCRFSubDtlNo") = TranNo
    '                        dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
    '                        dr("iTranNo") = TranNo
    '                        dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
    '                        dr("vMedExDesc") = ControlDesc.Trim.Substring(0, ControlDesc.Trim.Length - 1)
    '                        dr("dMedExDateTime") = System.DateTime.Now
    '                        dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
    '                        dr("iModifyBy") = Me.Session(S_UserID)
    '                        dr("cStatusIndi") = "N"
    '                        DtCRFSubDtl.Rows.Add(dr)
    '                        DtCRFSubDtl.AcceptChanges()

    '                    ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.Label" AndAlso _
    '                               objControl.ID.ToString.Trim().Contains("lbl") Then

    '                        Dim objLbl As Label = CType(objControl, Label)
    '                        ControlId = objControl.ID.ToString.Replace("Lnk", "")
    '                        ControlDesc = objLbl.Text

    '                        TranNo += 1
    '                        ObjId = objControl.ID.ToString.Replace("lbl", "")
    '                        dr = DtCRFSubDtl.NewRow
    '                        dr("nCRFSubDtlNo") = TranNo
    '                        dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
    '                        dr("iTranNo") = TranNo
    '                        dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
    '                        dr("vMedExDesc") = ""
    '                        dr("dMedExDateTime") = System.DateTime.Now
    '                        dr("vMedexResult") = ControlDesc.Trim()
    '                        dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
    '                        dr("iModifyBy") = Me.Session(S_UserID)
    '                        dr("cStatusIndi") = "N"
    '                        DtCRFSubDtl.Rows.Add(dr)
    '                        DtCRFSubDtl.AcceptChanges()

    '                    End If

    '                End If

    '            Next objControl
    '            '****************************************

    '            DtCRFSubDtl.TableName = "CRFSubDtl"
    '            DtCRFSubDtl.AcceptChanges()

    '            DsSave = Nothing
    '            DsSave = New DataSet
    '            DsSave.Tables.Add(DtCRFHdr.Copy())
    '            DsSave.Tables.Add(DtCRFDtl.Copy())
    '            DsSave.Tables.Add(DtCRFSubDtl.Copy())
    '            DsSave.AcceptChanges()

    '            dt_DCF = CType(Me.ViewState(VS_DtDCF), DataTable).Copy()
    '            dt_DCF.TableName = "DCFMst"
    '            dt_DCF.AcceptChanges()
    '            DsSave.Tables.Add(dt_DCF.Copy())
    '            DsSave.AcceptChanges()

    '            Return True
    '        Catch ex As Exception
    '            Me.ShowErrorMessage(ex.Message, "")
    '            Return False
    '        End Try

    '    End Function

    '#End Region

    '#Region "Edit Checks"

    '    Private Sub EditChecks()
    '        Dim ds_MedExEditChecks As New DataSet
    '        Dim ds_EditChecksHdr As New DataSet
    '        Dim ds_EditChecksDtl As New DataSet
    '        Dim ds_CRFDetail As New DataSet
    '        Dim ds_ParentWorkspace As New DataSet
    '        Dim wStr As String = String.Empty
    '        Dim eStr As String = String.Empty
    '        Dim ParentWorkspaceId As String = String.Empty

    '        Try

    '            wStr = "vWorkspaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And cStatusIndi <> 'D'"

    '            If Not Me.objHelp.getworkspacemst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                    ds_ParentWorkspace, eStr) Then
    '                Throw New Exception(eStr)
    '            End If
    '            If ds_ParentWorkspace Is Nothing Then
    '                Throw New Exception("Error While Getting Edit Parent Project Detail")
    '            End If
    '            If ds_ParentWorkspace.Tables(0).Rows.Count > 0 Then
    '                If Convert.ToString(ds_ParentWorkspace.Tables(0).Rows(0)("vParentWorkspaceId")).Trim() <> "" Then
    '                    ParentWorkspaceId = Convert.ToString(ds_ParentWorkspace.Tables(0).Rows(0)("vParentWorkspaceId")).Trim()
    '                End If
    '            End If

    '            wStr = "vWorkspaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And cStatusIndi <> 'D'"
    '            If ParentWorkspaceId.Trim() <> "" Then
    '                wStr = "vWorkspaceId = '" + ParentWorkspaceId + "' And cStatusIndi <> 'D'"
    '            End If


    '            ''Commented as the query was creating problem when opend from report and 
    '            ''               dataentry is changed(AS parentid is the nodeid stored from query string.)-Pratiksha
    '            'If Me.Session(S_ScopeNo) = GeneralModule.Scope_ClinicalTrial Then
    '            '    wStr += " And iParentNodeId = " + Me.HFParentNodeId.Value.Trim() + " "
    '            'End If
    '            wStr += " And iSourceNodeId_If = " + Me.HFNodeId.Value.Trim() + " "
    '            wStr += " And (iTargetNodeId_If = 0 Or iTargetNodeId_If = " + Me.HFNodeId.Value.Trim() + ")"
    '            wStr += " And (iSourceNodeId_Then = 0 Or iSourceNodeId_Then = " + Me.HFNodeId.Value.Trim() + ")"
    '            wStr += " And (iTargetNodeId_Then = 0 Or iTargetNodeId_Then = " + Me.HFNodeId.Value.Trim() + ")"

    '            If Not Me.objHelp.GetMedExEditChecks(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition _
    '                                    , ds_MedExEditChecks, eStr) Then
    '                Throw New Exception(eStr)
    '            End If
    '            If ds_MedExEditChecks Is Nothing Then
    '                Throw New Exception("Error While Getting Edit Checks Detail")
    '            End If

    '            If ds_MedExEditChecks.Tables(0).Rows.Count < 1 Then
    '                Exit Sub
    '            End If

    '            wStr = "vWorkspaceId = '" & Me.HFWorkspaceId.Value.Trim() & "' And cStatusIndi <> 'D'"
    '            wStr += " And iNodeId = " & Me.HFNodeId.Value.Trim() & " And vSubjectId = '" & Me.HFSubjectId.Value.Trim() & "'"
    '            ''''''''''''''''''''''''''''''''''''''''''''''''''''

    '            If Not Me.objHelp.View_CRFHdrDtlSubDtl_Edit(wStr, "*", ds_CRFDetail, eStr) Then
    '                Throw New Exception(eStr)
    '            End If
    '            If ds_CRFDetail Is Nothing Then
    '                Throw New Exception("Error While Getting CRF Detail")
    '            End If

    '            If Not Me.objHelp.GetEditChecksHdr("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
    '                                                         ds_EditChecksHdr, eStr) Then
    '                Throw New Exception(eStr)
    '            End If
    '            If ds_EditChecksHdr Is Nothing Then
    '                Throw New Exception(eStr)
    '            End If

    '            If Not Me.objHelp.GetEditChecksDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
    '                                                         ds_EditChecksDtl, eStr) Then
    '                Throw New Exception(eStr)
    '            End If
    '            If ds_EditChecksDtl Is Nothing Then
    '                Throw New Exception(eStr)
    '            End If

    '            Me.FireEditChecks(ds_MedExEditChecks, ds_CRFDetail, ds_EditChecksHdr, ds_EditChecksDtl)

    '        Catch ex As Exception
    '            Me.ShowErrorMessage("Error While Executing Edit Checks", ex.Message)
    '        End Try
    '    End Sub

    '    Private Sub FireEditChecks(ByVal ds_MedExEditChecks As DataSet, ByVal ds_CRFDetail As DataSet, _
    '                                ByVal ds_EditChecksHdr As DataSet, ByVal ds_EditChecksDtl As DataSet)
    '        Dim dt_EditChecksHdr As New DataTable
    '        Dim dr_EditChecksHdr As DataRow
    '        Dim dt_EditChecksDtl As New DataTable
    '        Dim dr_EditChecksDtl As DataRow

    '        Dim dv_EditChecksHdr As New DataView
    '        Dim dv_EditChecksDtl As New DataView

    '        Dim ds_Save As New DataSet
    '        Dim ds_EditChecksHdrDtl As New DataSet

    '        Dim dt_MedExEditChecks As New DataTable
    '        Dim dt_CRFDetail As New DataTable
    '        Dim dv_CRFDetail As New DataView

    '        Dim Source_MedExValue_If As String = String.Empty
    '        Dim Source_MedExType_If As String = String.Empty
    '        Dim Target_MedExValue_If As String = String.Empty
    '        Dim Target_MedExType_If As String = String.Empty

    '        Dim Source_MedExValue_Then As String = String.Empty
    '        Dim Source_MedExType_Then As String = String.Empty
    '        Dim Target_MedExValue_Then As String = String.Empty
    '        Dim Target_MedExType_Then As String = String.Empty

    '        Dim filter As String = String.Empty
    '        Dim Op_If As String = String.Empty
    '        Dim Op_Then As String = String.Empty

    '        Dim counter As Integer = 0
    '        Dim Is_Query As Boolean = False

    '        Dim wStr As String = String.Empty
    '        Dim eStr As String = String.Empty
    '        Dim Subject_Count As Integer = 0

    '        Dim dr_Detail() As DataRow
    '        Dim dr_DetailThen() As DataRow

    '        Try

    '            dt_EditChecksHdr = ds_EditChecksHdr.Tables(0).Copy()
    '            dt_EditChecksDtl = ds_EditChecksDtl.Tables(0).Copy()
    '            dt_MedExEditChecks = ds_MedExEditChecks.Tables(0).Copy()
    '            dt_CRFDetail = ds_CRFDetail.Tables(0).Copy()

    '            dv_CRFDetail = dt_CRFDetail.DefaultView.ToTable(True, "vSubjectId".Split(",")).DefaultView

    '            For Each drSubject As DataRow In dv_CRFDetail.ToTable().Rows

    '                Subject_Count = Subject_Count + 1

    '                dr_EditChecksHdr = dt_EditChecksHdr.NewRow()
    '                dr_EditChecksHdr("nEditChecksHdrNo") = Subject_Count
    '                dr_EditChecksHdr("vWorkspaceId") = Me.HFWorkspaceId.Value.Trim()
    '                dr_EditChecksHdr("iPeriod") = 1
    '                dr_EditChecksHdr("iNodeId") = Me.HFNodeId.Value.Trim()
    '                dr_EditChecksHdr("vActivityId") = Me.HFActivityId.Value.Trim()
    '                dr_EditChecksHdr("vSubjectId") = drSubject("vSubjectId")
    '                'dr_EditChecksHdr("dFiredDate") =
    '                dr_EditChecksHdr("iTranNo") = 1
    '                dr_EditChecksHdr("iModifyBy") = Me.Session(S_UserID)
    '                dt_EditChecksHdr.Rows.Add(dr_EditChecksHdr)
    '                dt_EditChecksHdr.AcceptChanges()

    '                For Each dr As DataRow In dt_MedExEditChecks.Rows

    '                    counter += 1

    '                    filter = "iNodeId = " + Convert.ToString(dr("iSourceNodeId_If")) + " And vMedExCode = '"
    '                    filter += Convert.ToString(dr("vSourceMedExCode_If")) + "'"
    '                    filter += " And vSubjectId = '" + drSubject("vSubjectId") + "'"
    '                    dr_Detail = dt_CRFDetail.Select(filter)

    '                    For index As Integer = 0 To dr_Detail.Length - 1

    '                        Dim count As Integer = dr_Detail.Length

    '                        Source_MedExValue_If = String.Empty
    '                        Source_MedExType_If = String.Empty
    '                        Target_MedExValue_If = String.Empty

    '                        '**************If Condition
    '                        Source_MedExValue_If = Convert.ToString(dr_Detail(index)("vDefaultValue")).Trim()
    '                        Source_MedExType_If = Convert.ToString(dr_Detail(index)("vMedExType")).Trim()
    '                        Target_MedExValue_If = Convert.ToString(dr("vTargetValue_If")).Trim()
    '                        If Convert.ToString(dr("vTargetMedExCode_If")).Trim() <> "" Then
    '                            Target_MedExValue_If = String.Empty
    '                            Target_MedExType_If = String.Empty

    '                            Dim dr_TargetCRFDetail() As DataRow
    '                            filter = "iNodeId = " + Convert.ToString(dr("iTargetNodeId_If")) + " And vMedExCode = '"
    '                            filter += dr("vTargetMedExCode_If") + "'"
    '                            filter += " And vSubjectId = '" + drSubject("vSubjectId") + "'"
    '                            dr_TargetCRFDetail = dt_CRFDetail.Select(filter)


    '                            If dr_TargetCRFDetail.Length > 0 Then
    '                                Target_MedExValue_If = Convert.ToString(dr_TargetCRFDetail(index)("vDefaultValue"))
    '                                Target_MedExType_If = Convert.ToString(dr_TargetCRFDetail(index)("vMedexType"))
    '                            End If

    '                        End If
    '                        Op_If = Convert.ToString(dr("vOperator_If")).Trim()
    '                        '***********************


    '                        '*************Then Condition
    '                        Source_MedExValue_Then = ""
    '                        Source_MedExType_Then = ""
    '                        Target_MedExValue_Then = ""
    '                        Op_Then = ""

    '                        If Convert.ToString(dr("iSourceNodeId_Then")).Trim() <> "" AndAlso _
    '                                                        Convert.ToString(dr("iSourceNodeId_Then")).Trim() <> "0" Then

    '                            filter = "iNodeId = " + Convert.ToString(dr("iSourceNodeId_Then")) + " And vMedExCode = '"
    '                            filter += Convert.ToString(dr("vSourceMedExCode_Then")) + "'"
    '                            filter += " And vSubjectId = '" + drSubject("vSubjectId") + "'"
    '                            dr_DetailThen = dt_CRFDetail.Select(filter)

    '                            If dr_DetailThen.Length > 0 Then

    '                                Source_MedExValue_Then = Convert.ToString(dr_DetailThen(index)("vDefaultValue")).Trim()
    '                                Source_MedExType_Then = Convert.ToString(dr_DetailThen(index)("vMedExType")).Trim()
    '                                Target_MedExValue_Then = Convert.ToString(dr("vTargetValue_Then")).Trim()

    '                                If Convert.ToString(dr("vTargetMedExCode_Then")).Trim() <> "" Then
    '                                    Dim dr_TargetCRFDetail() As DataRow
    '                                    filter = "iNodeId = " + Convert.ToString(dr("iTargetNodeId_Then")) + " And vMedExCode = '"
    '                                    filter += dr("vTargetMedExCode_Then") + "'"
    '                                    filter += " And vSubjectId = '" + drSubject("vSubjectId") + "'"
    '                                    filter += " And nCRFDtlNo = " + Convert.ToString(dr_Detail(index)("nCRFDtlNo"))
    '                                    filter += " And iRepeatNo = " + Convert.ToString(dr_Detail(index)("iRepeatNo"))
    '                                    dr_TargetCRFDetail = dt_CRFDetail.Select(filter)

    '                                    If dr_TargetCRFDetail.Length > 0 Then
    '                                        Dim dr_TargetDetail() As DataRow
    '                                        filter = "iNodeId = " + Convert.ToString(dr("iTargetNodeId_Then")) + " And vMedExCode = '"
    '                                        filter += dr("vTargetMedExCode_Then") + "' And nCRFDtlNo = " + Convert.ToString(dr_TargetCRFDetail(index)("nCRFDtlNo"))
    '                                        filter += " And iRepeatNo = " + Convert.ToString(dr_TargetCRFDetail(index)("iRepeatNo"))
    '                                        filter += " And vSubjectId = '" + drSubject("vSubjectId") + "'"
    '                                        dr_TargetDetail = dt_CRFDetail.Select(filter)
    '                                        If dr_TargetDetail.Length > 0 Then
    '                                            Target_MedExValue_Then = Convert.ToString(dr_TargetDetail(index)("vDefaultValue"))
    '                                            Target_MedExType_Then = Convert.ToString(dr_TargetDetail(index)("vMedexType"))
    '                                        End If
    '                                    End If
    '                                End If
    '                                Op_Then = Convert.ToString(dr("vOperator_Then")).Trim()
    '                            End If

    '                        End If
    '                        '***********************

    '                        dr_EditChecksDtl = dt_EditChecksDtl.NewRow()
    '                        dr_EditChecksDtl("bIsQuery") = 0
    '                        Is_Query = False

    '                        Me.GetResult(Source_MedExValue_If, Source_MedExType_If, _
    '                                     Target_MedExValue_If, Target_MedExType_If, _
    '                                     Op_If.ToUpper(), _
    '                                     Source_MedExValue_Then, Source_MedExType_Then, _
    '                                     Target_MedExValue_Then, Target_MedExType_Then, _
    '                                     Op_Then.ToUpper(), Is_Query)

    '                        If Is_Query Then
    '                            dr_EditChecksDtl("bIsQuery") = 1
    '                        End If
    '                        dr_EditChecksDtl("nEditChecksHdrNo") = Subject_Count
    '                        dr_EditChecksDtl("nEditChecksDtlNo") = counter
    '                        dr_EditChecksDtl("nMedExEditCheckNo") = Convert.ToString(dr("nMedExEditCheckNo"))
    '                        dr_EditChecksDtl("nCRFDtlNo") = Convert.ToString(dr_Detail(index)("nCRFDtlNo"))
    '                        dr_EditChecksDtl("vQueryValue") = Convert.ToString(dr("vCondition")).Trim()
    '                        dr_EditChecksDtl("cQueryStatus") = Query_Generated
    '                        dr_EditChecksDtl("vRemarks") = IIf(Convert.ToString(dr_Detail(index)("iRepeatNo")).Trim() > 0 And count > 1, "[" & Convert.ToString(dr_Detail(index)("vNodeDisplayName")).Trim() & " Repetition : " & Convert.ToString(dr_Detail(index)("iRepeatNo")).Trim() & "]", "") & Convert.ToString(dr("vRemarks")).Trim()
    '                        dr_EditChecksDtl("cStatusIndi") = "N"
    '                        dr_EditChecksDtl("cGenerateFlag") = "Y"
    '                        dr_EditChecksDtl("vGenerateRemark") = "On Submit"
    '                        dr_EditChecksDtl("iModifyBy") = Me.Session(S_UserID)

    '                        dt_EditChecksDtl.Rows.Add(dr_EditChecksDtl)
    '                        dt_EditChecksDtl.AcceptChanges()

    '                        Source_MedExValue_If = String.Empty
    '                        Source_MedExType_If = String.Empty
    '                        Target_MedExValue_If = String.Empty
    '                        Target_MedExType_If = String.Empty
    '                        Source_MedExValue_Then = String.Empty
    '                        Source_MedExType_Then = String.Empty
    '                        Target_MedExValue_Then = String.Empty
    '                        Target_MedExType_Then = String.Empty
    '                        filter = String.Empty
    '                        Op_If = String.Empty
    '                        Op_Then = String.Empty

    '                    Next index

    '                Next dr

    '            Next drSubject

    '            dv_EditChecksHdr = dt_EditChecksHdr.DefaultView
    '            dt_EditChecksHdr = Nothing
    '            dt_EditChecksHdr = New DataTable()
    '            dt_EditChecksHdr = dv_EditChecksHdr.ToTable().Copy()

    '            For Each dr As DataRow In dt_EditChecksHdr.Rows

    '                dv_EditChecksHdr = dt_EditChecksHdr.DefaultView
    '                dv_EditChecksHdr.RowFilter = "nEditChecksHdrNo = " + Convert.ToString(dr("nEditChecksHdrNo")).Trim()

    '                dv_EditChecksDtl = dt_EditChecksDtl.DefaultView
    '                dv_EditChecksDtl.RowFilter = "nEditChecksHdrNo = " + Convert.ToString(dr("nEditChecksHdrNo")).Trim()

    '                ds_Save = Nothing
    '                ds_Save = New DataSet()
    '                ds_Save.Tables.Add(dv_EditChecksHdr.ToTable().Copy())
    '                ds_Save.AcceptChanges()
    '                ds_Save.Tables.Add(dv_EditChecksDtl.ToTable().Copy())
    '                ds_Save.AcceptChanges()

    '                If Not Me.objLambda.Save_EditChecksHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
    '                                        ds_Save, Me.Session(S_UserID), eStr) Then
    '                    Throw New Exception(eStr)
    '                End If

    '            Next

    '        Catch ex As Exception
    '            Me.ShowErrorMessage("Error While Processing Edit Checks : " + ex.Message, "")
    '        End Try

    '    End Sub

    '    Private Sub GetDateTimeResult_If(ByVal Op As String, ByVal SourceValue As String, ByVal TargetValue As String, ByRef Is_Query As Boolean)
    '        If IsDate(TargetValue) Then
    '            If Op = ">" Then

    '                If CDate(SourceValue) > CDate(TargetValue) Then
    '                    Is_Query = True
    '                End If

    '            ElseIf Op = ">=" Then

    '                If CDate(SourceValue) >= CDate(TargetValue) Then
    '                    Is_Query = True
    '                End If

    '            ElseIf Op = "<" Then

    '                If CDate(SourceValue) < CDate(TargetValue) Then
    '                    Is_Query = True
    '                End If

    '            ElseIf Op = "<=" Then

    '                If CDate(SourceValue) <= CDate(TargetValue) Then
    '                    Is_Query = True
    '                End If

    '            ElseIf Op = "=" Then

    '                If CDate(SourceValue) = CDate(TargetValue) Then
    '                    Is_Query = True
    '                End If

    '            ElseIf Op = "<>" Then

    '                If CDate(SourceValue) <> CDate(TargetValue) Then
    '                    Is_Query = True
    '                End If
    '            End If
    '        ElseIf TargetValue = "NULL" Then
    '            If Op = "=" Then

    '                If IsDBNull(SourceValue) Then
    '                    Is_Query = True
    '                End If

    '            ElseIf Op = "<>" Then

    '                If Not IsDBNull(SourceValue) Then
    '                    Is_Query = True
    '                End If
    '            End If
    '        End If

    '    End Sub

    '    Private Sub GetTextValueResult_If(ByVal Op As String, ByVal SourceValue As String, ByVal TargetValue As String, ByRef Is_Query As Boolean)

    '        If Op = ">" Then

    '            If Val(SourceValue) > Val(TargetValue) Then
    '                Is_Query = True
    '            End If

    '        ElseIf Op = ">=" Then

    '            If Val(SourceValue) >= Val(TargetValue) Then
    '                Is_Query = True
    '            End If

    '        ElseIf Op = "<" Then

    '            If Val(SourceValue) < Val(TargetValue) Then
    '                Is_Query = True
    '            End If

    '        ElseIf Op = "<=" Then

    '            If Val(SourceValue) <= Val(TargetValue) Then
    '                Is_Query = True
    '            End If

    '        ElseIf Op = "=" Then

    '            If SourceValue.ToUpper = TargetValue.ToUpper Then
    '                Is_Query = True
    '            End If

    '        ElseIf Op = "<>" Then

    '            If SourceValue.ToUpper <> TargetValue.ToUpper Then
    '                Is_Query = True
    '            End If

    '        End If

    '    End Sub

    '    Private Sub GetDateTimeResult_Then(ByVal Op As String, ByVal SourceValue As String, ByVal TargetValue As String, ByRef Is_Query As Boolean)
    '        If IsDate(TargetValue) Then
    '            If Op = ">" Then

    '                If CDate(SourceValue) > CDate(TargetValue) Then
    '                    Is_Query = True
    '                End If

    '            ElseIf Op = ">=" Then

    '                If CDate(SourceValue) >= CDate(TargetValue) Then
    '                    Is_Query = True
    '                End If

    '            ElseIf Op = "<" Then

    '                If CDate(SourceValue) < CDate(TargetValue) Then
    '                    Is_Query = True
    '                End If

    '            ElseIf Op = "<=" Then

    '                If CDate(SourceValue) <= CDate(TargetValue) Then
    '                    Is_Query = True
    '                End If

    '            ElseIf Op = "=" Then

    '                If CDate(SourceValue) = CDate(TargetValue) Then
    '                    Is_Query = True
    '                End If

    '            ElseIf Op = "<>" Then

    '                If CDate(SourceValue) <> CDate(TargetValue) Then
    '                    Is_Query = True
    '                End If

    '            End If
    '        ElseIf TargetValue = "NULL" Then
    '            If Op = "=" Then

    '                If IsDBNull(SourceValue) Then
    '                    Is_Query = True
    '                End If

    '            ElseIf Op = "<>" Then

    '                If Not IsDBNull(SourceValue) Then
    '                    Is_Query = True
    '                End If
    '            End If
    '        End If
    '    End Sub

    '    Private Sub GetTextValueResult_Then(ByVal Op As String, ByVal SourceValue As String, ByVal TargetValue As String, ByRef Is_Query As Boolean)

    '        If Op = ">" Then

    '            If Val(SourceValue) > Val(TargetValue) Then
    '                Is_Query = True
    '            End If

    '        ElseIf Op = ">=" Then

    '            If Val(SourceValue) >= Val(TargetValue) Then
    '                Is_Query = True
    '            End If

    '        ElseIf Op = "<" Then

    '            If Val(SourceValue) < Val(TargetValue) Then
    '                Is_Query = True
    '            End If

    '        ElseIf Op = "<=" Then

    '            If Val(SourceValue) <= Val(TargetValue) Then
    '                Is_Query = True
    '            End If

    '        ElseIf Op = "=" Then

    '            If SourceValue.ToUpper = TargetValue.ToUpper Then
    '                Is_Query = True
    '            End If

    '        ElseIf Op = "<>" Then

    '            If SourceValue.ToUpper <> TargetValue.ToUpper Then
    '                Is_Query = True
    '            End If

    '        End If

    '    End Sub

    '    'Private Sub GetResult(ByVal SourceValue_If As String, ByVal SourceType_If As String, _
    '    '                        ByVal TargetValue_If As String, ByVal TargetType_If As String, _
    '    '                        ByVal Op_If As String, _
    '    '                        ByVal SourceValue_Then As String, ByVal SourceType_Then As String, _
    '    '                        ByVal TargetValue_Then As String, ByVal TargetType_Then As String, _
    '    '                        ByVal Op_Then As String, ByRef Is_Query As Boolean)

    '    '    Try


    '    '        ''''''''''''''check if date is not in proper dateformate editchecks will fire without query:Start
    '    '        ' Addecd to check if date contains any other values then editchecks not need to fire.
    '    '        If SourceType_If = "DateTime" Or SourceType_If = "AsyncDateTime" Then
    '    '            If SourceValue_If <> DBNull.Value.ToString Or SourceValue_If <> "" Then
    '    '                If IsDate(SourceValue_If) = False Then
    '    '                    Is_Query = 0
    '    '                    Exit Sub
    '    '                End If
    '    '            End If
    '    '        End If
    '    '        ' Addecd to check if date contains any other values then editchecks not need to fire.
    '    '        If SourceType_Then = "DateTime" Or SourceType_Then = "AsyncDateTime" Then
    '    '            If SourceValue_Then <> DBNull.Value.ToString Or SourceValue_Then <> "" Then
    '    '                If IsDate(SourceValue_Then) = False Then
    '    '                    Is_Query = 0
    '    '                    Exit Sub
    '    '                End If
    '    '            End If
    '    '        End If
    '    '        If TargetType_If = "DateTime" Or TargetType_If = "AsyncDateTime" Then
    '    '            If TargetValue_If <> DBNull.Value.ToString Or TargetValue_If <> "" Then
    '    '                If IsDate(TargetValue_If) = False Then
    '    '                    Is_Query = 0
    '    '                    Exit Sub
    '    '                End If
    '    '            End If
    '    '            ' Addecd to check if date contains any other values then editchecks not need to fire.
    '    '        End If
    '    '        If TargetType_Then = "DateTime" Or TargetType_Then = "AsyncDateTime" Then
    '    '            If TargetValue_Then <> DBNull.Value.ToString Or TargetValue_Then <> "" Then
    '    '                If IsDate(TargetValue_Then) = False Then
    '    '                    Is_Query = 0
    '    '                    Exit Sub
    '    '                End If
    '    '            End If
    '    '        End If ''''''''''''''check if date is not in proper dateformate editchecks will fire without query:End
    '    '        If (SourceType_If = "DateTime" Or SourceType_If = "AsyncDateTime") Or (SourceType_If = "Time" Or SourceType_If = "AsyncTime") Then

    '    '            If SourceValue_If <> "" AndAlso TargetValue_If <> "" Then
    '    '                GetDateTimeResult_If(Op_If, SourceValue_If, TargetValue_If, Is_Query)
    '    '            End If

    '    '            If SourceValue_If = "" AndAlso TargetValue_If = "NULL" Then
    '    '                TargetValue_If = ""
    '    '                GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
    '    '            End If

    '    '            If (SourceValue_Then <> "" Or Op_Then <> "") AndAlso Is_Query = True Then

    '    '                Is_Query = False
    '    '                If TargetValue_Then.ToUpper() = "NULL" And SourceValue_Then.ToUpper() <> "NULL" Then
    '    '                    TargetValue_Then = ""
    '    '                    GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)

    '    '                ElseIf TargetValue_Then.ToUpper() <> "NULL" And TargetValue_Then.ToUpper() <> "" Then
    '    '                    If (SourceType_Then = "DateTime" Or SourceType_Then = "AsyncDateTime") Or (SourceType_Then = "Time" Or SourceType_Then = "AsyncTime") Then
    '    '                        GetDateTimeResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
    '    '                    Else
    '    '                        GetTextValueResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
    '    '                    End If
    '    '                End If

    '    '            ElseIf TargetValue_Then = "NULL" AndAlso Is_Query = True Then

    '    '                Is_Query = False
    '    '                If TargetValue_Then.ToUpper() = "NULL" And SourceValue_Then.ToUpper() <> "NULL" Then
    '    '                    TargetValue_Then = ""
    '    '                    GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
    '    '                End If

    '    '            End If

    '    '        Else 'If SourceType_If = "TextBox" Then

    '    '            If SourceValue_If <> "" AndAlso TargetValue_If <> "" Then
    '    '                GetTextValueResult_If(Op_If, SourceValue_If, TargetValue_If, Is_Query)
    '    '            End If

    '    '            If SourceValue_If = "" AndAlso TargetValue_If = "NULL" Then
    '    '                TargetValue_If = ""
    '    '                GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
    '    '            End If

    '    '            If (SourceValue_Then <> "" Or Op_Then <> "") AndAlso Is_Query = True Then

    '    '                Is_Query = False
    '    '                If TargetValue_Then.ToUpper() = "NULL" And SourceValue_Then.ToUpper() <> "NULL" Then
    '    '                    TargetValue_Then = ""
    '    '                    GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)

    '    '                ElseIf TargetValue_Then.ToUpper() <> "NULL" And TargetValue_Then.ToUpper() <> "" Then
    '    '                    If (SourceType_Then = "DateTime" Or SourceType_Then = "AsyncDateTime") Or (SourceType_Then = "Time" Or SourceType_Then = "AsyncTime") Then
    '    '                        GetDateTimeResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
    '    '                    Else
    '    '                        GetTextValueResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
    '    '                    End If
    '    '                End If

    '    '            ElseIf TargetValue_Then = "NULL" AndAlso Is_Query = True Then

    '    '                Is_Query = False
    '    '                If TargetValue_Then.ToUpper() = "NULL" And SourceValue_Then.ToUpper() <> "NULL" Then
    '    '                    TargetValue_Then = ""
    '    '                    GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
    '    '                End If
    '    '            ElseIf Op_Then <> "" Then
    '    '                Is_Query = False
    '    '                If (TargetValue_If.ToUpper() = "NULL" AndAlso Op_Then <> "") AndAlso SourceValue_If.ToUpper() <> "NULL" Then
    '    '                    TargetValue_Then = ""
    '    '                    GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
    '    '                ElseIf TargetValue_Then.ToUpper() <> "NULL" And TargetValue_Then.ToUpper() <> "" Then
    '    '                    If (SourceType_Then = "DateTime" Or SourceType_Then = "AsyncDateTime") Or (SourceType_Then = "Time" Or SourceType_Then = "AsyncTime") Then
    '    '                        GetDateTimeResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
    '    '                    Else
    '    '                        GetTextValueResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
    '    '                    End If
    '    '                ElseIf TargetValue_Then = "NULL" Then
    '    '                    TargetValue_Then = ""
    '    '                    GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
    '    '                End If

    '    '            End If

    '    '        End If

    '    '    Catch ex As Exception
    '    '        Me.ShowErrorMessage("Error While Processing Edit Checks ", ex.Message)
    '    '    End Try

    '    'End Sub 
    '    ' Commented old one On 17-may-2012

    '    Private Sub GetResult(ByVal SourceValue_If As String, ByVal SourceType_If As String, _
    '                    ByVal TargetValue_If As String, ByVal TargetType_If As String, _
    '                    ByVal Op_If As String, _
    '                    ByVal SourceValue_Then As String, ByVal SourceType_Then As String, _
    '                    ByVal TargetValue_Then As String, ByVal TargetType_Then As String, _
    '                    ByVal Op_Then As String, _
    '                    ByRef Is_Query As Boolean)

    '        Try

    '            ''''''''''''''check if date is not in proper dateformate editchecks will fire without query:Start
    '            ' Addecd to check if date contains any other values then editchecks not need to fire.
    '            If SourceType_If = "DateTime" Or SourceType_If = "AsyncDateTime" Then
    '                If SourceValue_If <> DBNull.Value.ToString Or SourceValue_If <> "" Then
    '                    If IsDate(SourceValue_If) = False Then
    '                        Is_Query = 0
    '                        Exit Sub
    '                    End If
    '                End If
    '            End If
    '            ' Addecd to check if date contains any other values then editchecks not need to fire.
    '            If SourceType_Then = "DateTime" Or SourceType_Then = "AsyncDateTime" Then
    '                If SourceValue_Then <> DBNull.Value.ToString Or SourceValue_Then <> "" Then
    '                    If IsDate(SourceValue_Then) = False Then
    '                        Is_Query = 0
    '                        Exit Sub
    '                    End If
    '                End If
    '            End If
    '            If TargetType_If = "DateTime" Or TargetType_If = "AsyncDateTime" Then
    '                If TargetValue_If <> DBNull.Value.ToString Or TargetValue_If <> "" Then
    '                    If IsDate(TargetValue_If) = False Then
    '                        Is_Query = 0
    '                        Exit Sub
    '                    End If
    '                End If
    '                ' Addecd to check if date contains any other values then editchecks not need to fire.
    '            End If
    '            If TargetType_Then = "DateTime" Or TargetType_Then = "AsyncDateTime" Then
    '                If TargetValue_Then <> DBNull.Value.ToString Or TargetValue_Then <> "" Then
    '                    If IsDate(TargetValue_Then) = False Then
    '                        Is_Query = 0
    '                        Exit Sub
    '                    End If
    '                End If
    '            End If ''''''''''''''check if date is not in proper dateformate editchecks will fire without query:End

    '            If (SourceType_If = "DateTime" Or SourceType_If = "AsyncDateTime") Or (SourceType_If = "Time" Or SourceType_If = "AsyncTime") Then

    '                If SourceValue_If <> "" AndAlso TargetValue_If <> "" Then
    '                    GetDateTimeResult_If(Op_If, SourceValue_If, TargetValue_If, Is_Query)
    '                End If

    '                If SourceValue_If = "" AndAlso (TargetValue_If = "NULL" Or TargetValue_If = "") Then
    '                    TargetValue_If = ""
    '                    GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
    '                End If

    '                If SourceValue_If <> "" AndAlso TargetValue_If = "" Then
    '                    GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
    '                End If

    '                If SourceValue_If = "" AndAlso TargetValue_If <> "" Then
    '                    GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
    '                End If

    '                If Op_Then <> "" AndAlso Is_Query = True Then
    '                    If SourceValue_Then <> "" Then
    '                        Is_Query = False
    '                        If (TargetValue_If.ToUpper() = "NULL" Or TargetValue_If.ToUpper() = "") AndAlso SourceValue_If.ToUpper() <> "NULL" Then
    '                            TargetValue_Then = ""
    '                            GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
    '                        ElseIf TargetValue_Then.ToUpper() <> "NULL" And TargetValue_Then.ToUpper() <> "" Then
    '                            If (SourceType_Then = "DateTime" Or SourceType_Then = "AsyncDateTime") Or (SourceType_Then = "Time" Or SourceType_Then = "AsyncTime") Then
    '                                GetDateTimeResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
    '                            Else
    '                                GetTextValueResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
    '                            End If
    '                        End If
    '                    ElseIf TargetValue_Then = "NULL" Or TargetValue_Then = "" Then

    '                        Is_Query = False
    '                        If TargetValue_Then.ToUpper() = "NULL" And SourceValue_Then.ToUpper() <> "NULL" Then
    '                            TargetValue_Then = ""
    '                            GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
    '                        End If

    '                    ElseIf SourceValue_Then = "" AndAlso (TargetValue_Then = "NULL" Or TargetValue_Then = "") Then
    '                        GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
    '                    ElseIf SourceValue_Then = "" AndAlso (TargetValue_Then <> "NULL" And TargetValue_Then <> "") Then
    '                        GetDateTimeResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
    '                    Else
    '                        Is_Query = False
    '                        If (SourceType_Then = "DateTime" Or SourceType_Then = "AsyncDateTime") Or (SourceType_Then = "Time" Or SourceType_Then = "AsyncTime") Then
    '                            GetDateTimeResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
    '                        Else
    '                            GetTextValueResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
    '                        End If

    '                    End If
    '                End If


    '            Else 'If SourceType_If = "TextBox" Then

    '                If SourceValue_If <> "" AndAlso TargetValue_If <> "" Then
    '                    GetTextValueResult_If(Op_If, SourceValue_If, TargetValue_If, Is_Query)
    '                End If

    '                If SourceValue_If = "" AndAlso (TargetValue_If = "NULL" Or TargetValue_If = "") Then
    '                    TargetValue_If = ""
    '                    GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
    '                End If
    '                If SourceValue_If <> "" AndAlso (TargetValue_If = "NULL" Or TargetValue_If = "") Then
    '                    TargetValue_If = ""
    '                    GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
    '                End If
    '                If SourceValue_If = "" AndAlso (TargetValue_If <> "NULL" And TargetValue_If <> "") Then
    '                    TargetValue_If = ""
    '                    GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
    '                End If

    '                If Op_Then <> "" AndAlso Is_Query = True Then
    '                    If SourceValue_Then <> "" Then
    '                        Is_Query = False
    '                        If TargetValue_Then.ToUpper() = "NULL" Or TargetValue_Then.ToUpper() = "" Then
    '                            TargetValue_Then = ""
    '                            GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
    '                        ElseIf TargetValue_Then.ToUpper() <> "NULL" And TargetValue_Then.ToUpper() <> "" Then
    '                            If SourceType_Then = "DateTime" Or SourceType_Then = "AsyncDateTime" Then
    '                                GetDateTimeResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
    '                            Else
    '                                GetTextValueResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
    '                            End If
    '                        End If
    '                    ElseIf (TargetValue_Then = "NULL" Or TargetValue_Then = "") Then

    '                        Is_Query = False
    '                        If (SourceValue_Then.ToUpper() <> "NULL" Or SourceValue_Then.ToUpper() <> "") Then
    '                            TargetValue_Then = ""
    '                            GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
    '                        ElseIf (SourceValue_Then.ToUpper() = "NULL" Or SourceValue_Then.ToUpper() = "") Then
    '                            TargetValue_Then = ""
    '                            GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
    '                        End If

    '                    Else
    '                        Is_Query = False
    '                        If (SourceType_Then = "DateTime" Or SourceType_Then = "AsyncDateTime") Or (SourceType_Then = "Time" Or SourceType_Then = "AsyncTime") Then
    '                            GetDateTimeResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
    '                        Else
    '                            GetTextValueResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
    '                        End If

    '                    End If

    '                End If


    '            End If


    '        Catch ex As Exception
    '            Me.ShowErrorMessage("Error While Processing Edit Checks ", ex.Message)
    '        End Try

    '    End Sub


    '    Private Sub GetNullValueChecked(ByVal Op As String, ByVal SourceValue As String, ByVal TargetValue As String, ByRef Is_Query As Boolean)

    '        If Op = "=" Then

    '            If SourceValue = TargetValue Then
    '                Is_Query = True
    '            End If

    '        ElseIf Op = "<>" Then

    '            If SourceValue <> TargetValue Then
    '                Is_Query = True
    '            End If

    '        End If

    '    End Sub

    '#End Region

    '#Region "Checking & Assigning DCF Values"

    '    Private Function CheckDiscrepancy(ByVal objControl As Control, ByVal objId As String, ByVal objValue As String, ByVal DCFType As Char) As Boolean
    '        Dim dt_CheckValue As New DataTable
    '        Dim dv_CheckValue As New DataView
    '        Dim LowRange As String = ""
    '        Dim HighRange As String = ""
    '        Dim ValidationType() As String
    '        Dim Alertonvalue As String = ""
    '        Dim AlertMessage As String = ""
    '        Dim AlertType As String = ""
    '        Dim Is_Discrepancy As Boolean = False
    '        CheckDiscrepancy = False
    '        Dim SourceResponse As String = ""
    '        Try
    '            dt_CheckValue = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable)
    '            dv_CheckValue = dt_CheckValue.DefaultView()
    '            dv_CheckValue.RowFilter = "vMedExCode = '" + objId.Substring(0, objId.IndexOf("R")).Trim() + "'"

    '            LowRange = dv_CheckValue.ToTable().Rows(0)("vLowRange").ToString.Trim()
    '            HighRange = dv_CheckValue.ToTable().Rows(0)("vHighRange").ToString.Trim()
    '            ValidationType = dv_CheckValue.ToTable().Rows(0)("vValidationType").ToString.Trim.Split(",")

    '            Alertonvalue = dv_CheckValue.ToTable().Rows(0)("vAlertonvalue").ToString.Trim()
    '            AlertMessage = dv_CheckValue.ToTable().Rows(0)("vAlertMessage").ToString.Trim()
    '            AlertType = dv_CheckValue.ToTable().Rows(0)("cAlertType").ToString.Trim()


    '            If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.TextBox" Then

    '                Me.CheckIsNotNull(objValue, AlertType, Is_Discrepancy)
    '                If Is_Discrepancy Then
    '                    If Not Me.AssignDCFValues(objValue, "Please specify value", DCFType, objId) Then
    '                        Exit Function
    '                    End If
    '                    CheckDiscrepancy = True
    '                    Exit Function
    '                End If

    '                If Convert.ToString(objValue).Trim() <> "" Then
    '                    Me.CheckValidationType(ValidationType(0).Trim(), objValue, Is_Discrepancy)
    '                    If Is_Discrepancy Then
    '                        If Not Me.AssignDCFValues(objValue, "Validation Type Mismatch", DCFType, objId) Then
    '                            Exit Function
    '                        End If
    '                        CheckDiscrepancy = True
    '                        Exit Function
    '                    End If
    '                End If

    '                Me.CheckLength(ValidationType(1).Trim(), objValue, Is_Discrepancy)
    '                If Is_Discrepancy Then
    '                    If Not Me.AssignDCFValues(objValue, "Lengh Exceeded", DCFType, objId) Then
    '                        Exit Function
    '                    End If
    '                    CheckDiscrepancy = True
    '                    Exit Function
    '                End If

    '                'Commented the code because alert is used only for the alerting the user. values can be exceed.
    '                'Me.CheckLowHighRange(LowRange, HighRange, objValue, Is_Discrepancy)
    '                'If Is_Discrepancy Then
    '                '    If Not Me.AssignDCFValues(objValue, "Value must be between " + LowRange.Trim() + " and " + HighRange.Trim(), DCFType, objId) Then
    '                '        Exit Function
    '                '    End If
    '                '    CheckDiscrepancy = True
    '                '    Exit Function
    '                'End If

    '                'Commented the code because alert is used only for the alerting the user.
    '                'Me.CheckAlertType(Alertonvalue, AlertType, objValue, Is_Discrepancy)
    '                'If Is_Discrepancy Then
    '                '    If Not Me.AssignDCFValues(objValue, AlertMessage, DCFType, objId) Then
    '                '        Exit Function
    '                '    End If
    '                '    CheckDiscrepancy = True
    '                '    Exit Function
    '                'End If


    '            ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" Then

    '                Me.CheckIsNotNull(objValue, AlertType, Is_Discrepancy)
    '                If Is_Discrepancy Then
    '                    If Not Me.AssignDCFValues(objValue, "Please specify value", DCFType, objId) Then
    '                        Exit Function
    '                    End If
    '                    CheckDiscrepancy = True
    '                    Exit Function
    '                End If

    '                'Commented the code because alert is used only for the alerting the user.
    '                'Me.CheckAlertType(Alertonvalue, AlertType, objValue, Is_Discrepancy)
    '                'If Is_Discrepancy Then
    '                '    If Not Me.AssignDCFValues(objValue, AlertMessage, DCFType, objId) Then
    '                '        Exit Function
    '                '    End If
    '                '    CheckDiscrepancy = True
    '                '    Exit Function
    '                'End If

    '            ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.RadioButtonList" Then

    '                Me.CheckIsNotNull(objValue, AlertType, Is_Discrepancy)
    '                If Is_Discrepancy Then
    '                    If Not Me.AssignDCFValues(objValue, "Please specify value", DCFType, objId) Then
    '                        Exit Function
    '                    End If
    '                    CheckDiscrepancy = True
    '                    Exit Function
    '                End If

    '                'Commented the code because alert is used only for the alerting the user.
    '                'Me.CheckAlertType(Alertonvalue, AlertType, objValue, Is_Discrepancy)
    '                'If Is_Discrepancy Then
    '                '    If Not Me.AssignDCFValues(objValue, AlertMessage, DCFType, objId) Then
    '                '        Exit Function
    '                '    End If
    '                '    CheckDiscrepancy = True
    '                '    Exit Function
    '                'End If

    '            ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.CheckBoxList" Then

    '                Me.CheckIsNotNull(objValue, AlertType, Is_Discrepancy)
    '                If Is_Discrepancy Then
    '                    If Not Me.AssignDCFValues(objValue, "Please specify value", DCFType, objId) Then
    '                        Exit Function
    '                    End If
    '                    CheckDiscrepancy = True
    '                    Exit Function
    '                End If

    '                'Commented the code because alert is used only for the alerting the user.
    '                'Me.CheckAlertType(Alertonvalue, AlertType, objValue, Is_Discrepancy)
    '                'If Is_Discrepancy Then
    '                '    If Not Me.AssignDCFValues(objValue, AlertMessage, DCFType, objId) Then
    '                '        Exit Function
    '                '    End If
    '                '    CheckDiscrepancy = True
    '                '    Exit Function
    '                'End If

    '            End If

    '            Return False
    '        Catch ex As Exception
    '            Me.ShowErrorMessage("Problem While Checking Discrepancy : ", ex.Message)
    '            Return True
    '        End Try

    '    End Function

    '    Private Sub CheckIsNotNull(ByVal objValue As String, ByVal IsNotNull As String, ByRef Is_Discrepancy As Boolean)

    '        If IsNotNull.Trim.ToUpper() = "Y" AndAlso objValue.Trim() = "" Then
    '            Is_Discrepancy = True
    '        End If

    '    End Sub

    '    'Private Sub CheckLowHighRange(ByVal LowRange As String, ByVal HighRange As String, ByVal objValue As String, ByRef Is_Discrepancy As Boolean)
    '    '    Dim lr As Integer = 0
    '    '    Dim hr As Integer = 0
    '    '    Dim value As Integer = 0

    '    '    If LowRange <> "" Then
    '    '        If HighRange <> "" Then
    '    '            If Not Integer.TryParse(LowRange, lr) Then
    '    '                Is_Discrepancy = True
    '    '            End If
    '    '            If Not Integer.TryParse(HighRange, hr) Then
    '    '                Is_Discrepancy = True
    '    '            End If
    '    '            If Not Integer.TryParse(objValue, value) Then
    '    '                Is_Discrepancy = True
    '    '            End If

    '    '            If value < lr OrElse value > hr Then
    '    '                Is_Discrepancy = True
    '    '            End If
    '    '        End If
    '    '    Else
    '    '        If HighRange <> "" Then
    '    '            If Not Integer.TryParse(HighRange, hr) Then
    '    '                Is_Discrepancy = True
    '    '            End If

    '    '            If Not Integer.TryParse(objValue, value) Then
    '    '                Is_Discrepancy = True
    '    '            End If

    '    '            If value > hr Then
    '    '                Is_Discrepancy = True
    '    '            End If

    '    '        End If
    '    '    End If
    '    'End Sub

    '    Private Sub CheckValidationType(ByVal ValidationType As String, ByVal objValue As String, ByRef Is_Discrepancy As Boolean)

    '        If ValidationType <> "" Then

    '            If ValidationType.ToUpper() = Val_IN Then
    '                Dim result As Integer = 0
    '                If Not Integer.TryParse(objValue, result) Then
    '                    Is_Discrepancy = True
    '                End If
    '            ElseIf ValidationType.ToUpper() = Val_NU Then
    '                Dim result As Integer = 0
    '                If Not Decimal.TryParse(objValue, result) Then
    '                    Is_Discrepancy = True
    '                End If
    '            ElseIf ValidationType.ToUpper() = Val_AL Then
    '                Dim result As Char
    '                Dim str() As String
    '                str = objValue.Split("")
    '                For index As Integer = 0 To str.Length - 1
    '                    If Not Char.TryParse(str(index), result) Then
    '                        Is_Discrepancy = True
    '                        Exit Sub
    '                    End If
    '                Next

    '            ElseIf ValidationType.ToUpper() = Val_NNI OrElse ValidationType.ToUpper() = Val_NNU Then
    '                Dim result As Integer = 0
    '                If objValue.Trim() = "" AndAlso Not Integer.TryParse(objValue, result) Then
    '                    Is_Discrepancy = True
    '                End If

    '            ElseIf ValidationType.ToUpper() = Val_AN Then

    '                Dim resultchar As Char
    '                Dim resultinteger As Integer = 0
    '                Dim str() As String
    '                str = objValue.Split("")
    '                For index As Integer = 0 To str.Length - 1
    '                    If Not Char.TryParse(str(index), resultchar) OrElse _
    '                                            Not Integer.TryParse(str(index), resultinteger) Then
    '                        Is_Discrepancy = True
    '                        Exit Sub
    '                    End If
    '                Next

    '            End If

    '        End If

    '    End Sub

    '    Private Sub CheckLength(ByVal length As String, ByVal objValue As String, ByRef Is_Discrepancy As Boolean)

    '        If length <> "" AndAlso length <> "0" Then

    '            Dim result As Integer = 0
    '            If Not Integer.TryParse(length, result) Then
    '                Is_Discrepancy = True
    '            End If

    '            If objValue.Length > result Then
    '                Is_Discrepancy = True
    '            End If

    '        End If

    '    End Sub

    '    'Private Sub CheckAlertType(ByVal Alertonvalue As String, ByVal AlertType As String, ByVal objValue As String, ByRef Is_Discrepancy As Boolean)
    '    '    Dim values() As String
    '    '    Dim count As Integer = 0
    '    '    values = objValue.Split(",")
    '    '    For count = 0 To values.Length - 1

    '    '        If Alertonvalue <> "" Then
    '    '            If Alertonvalue.ToUpper() = values(count).ToUpper() Then
    '    '                Is_Discrepancy = True
    '    '                Exit For
    '    '            End If
    '    '        End If

    '    '    Next count
    '    'End Sub

    '    Private Function AssignDCFValues(ByVal dicrepancy As String, ByVal dicrepancyResponse As String, ByVal DCFType As Char, ByVal MedExCode As String) As Boolean
    '        Dim dt_DCF As New DataTable
    '        Dim drDCF As DataRow
    '        Try
    '            dt_DCF = CType(Me.ViewState(VS_DtDCF), DataTable).Copy()

    '            'nDCFNo,nCRFSubDtlNo,cDCFType,iDCFBy,dDCFDate,vDiscrepancy,vSourceResponse
    '            'cDCFStatus,iStatusChangedBy,dStatusChangedOn,iModifyBy,

    '            drDCF = dt_DCF.NewRow()
    '            drDCF("nDCFNo") = 0
    '            drDCF("nCRFDtlNo") = 0
    '            If Me.HFCRFDtlNo.Value.Trim() <> "" Then
    '                drDCF("nCRFDtlNo") = CType(Me.HFCRFDtlNo.Value.Trim(), Integer)
    '            End If

    '            drDCF("iSrNo") = 0
    '            drDCF("vMedExcode") = MedExCode
    '            drDCF("cDCFType") = DCFType
    '            drDCF("iDCFBy") = Me.Session(S_UserID)
    '            drDCF("dDCFDate") = System.DateTime.Now()
    '            drDCF("vDiscrepancy") = dicrepancy
    '            drDCF("vSourceResponse") = dicrepancyResponse
    '            drDCF("cDCFStatus") = "N"
    '            drDCF("iModifyBy") = Me.Session(S_UserID)
    '            dt_DCF.Rows.Add(drDCF)
    '            dt_DCF.AcceptChanges()

    '            Me.ViewState(VS_DtDCF) = dt_DCF.Copy()

    '            Return True
    '        Catch ex As Exception
    '            Me.ShowErrorMessage("Problem While Assigning DCF Values : ", ex.Message)
    '            Return False
    '        End Try

    '    End Function

    '#End Region

    '#Region "Review Completed"

    '    Private Sub ReviewAllActivities()

    '        Dim ds_CRFWorkFlowDtl As New DataSet
    '        Dim ds_CRFDtl As New DataSet
    '        Dim ds_CRFHdr As New DataSet
    '        Dim dv_CRFDtl As DataView
    '        Dim ds_DCF As New DataSet
    '        Dim dr_DCF() As DataRow
    '        Dim dr As DataRow
    '        Dim dr_WorkFlowDtl As DataRow
    '        Dim wStr As String = String.Empty
    '        Dim eStr As String = String.Empty

    '        Try
    '            ' Change to review all activities of the parent activity --Pratiksha
    '            wStr = "vWorkSpaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And iPeriod = " + Me.HFPeriodId.Value.Trim() + " And "
    '            wStr += "iNodeId in (Select iNodeId From View_WorkSpaceNodeDetail Where vWorkSpaceId = '" + Me.HFWorkspaceId.Value.Trim() + "'"
    '            If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
    '                If Me.HFSubjectId.Value.Trim.ToString() = "0000" Then
    '                    wStr += " And cSubjectWiseFlag = 'N' "
    '                Else
    '                    wStr += " And cSubjectWiseFlag = 'Y' "
    '                End If
    '            End If
    '            wStr += " And iPeriod = " + Me.HFPeriodId.Value.Trim() + " And (iParentNodeId = " + Me.HFParentNodeId.Value.Trim()
    '            If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
    '                wStr += "  Or iNodeId =  " + Me.HFParentNodeId.Value.Trim()
    '            End If
    '            wStr += "  ) )"

    '            'wStr = "vWorkSpaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And iPeriod = " + Me.HFPeriodId.Value.Trim() + " And "
    '            'wStr += "iNodeId in (Select iNodeId From WorkspaceNodeDetail Where vWorkSpaceId = '" + Me.HFWorkspaceId.Value.Trim()
    '            'wStr += "' And iPeriod = " + Me.HFPeriodId.Value.Trim() + " And iParentNodeId = " + Me.HFParentNodeId.Value.Trim() + ")"
    '            '--Pratiksha

    '            If Not Me.objHelp.GetCRFHdr(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                ds_CRFHdr, eStr) Then
    '                Throw New Exception(eStr)
    '            End If

    '            wStr = "vSubjectId = '" + Me.HFSubjectId.Value.Trim() + "' And nCRFHdrNo in("

    '            If Not ds_CRFHdr Is Nothing Then
    '                For Each dr In ds_CRFHdr.Tables(0).Rows
    '                    wStr += Convert.ToString(dr("nCRFHdrNo")).Trim() + ","
    '                Next dr
    '            End If
    '            wStr = wStr.Substring(0, wStr.LastIndexOf(","))
    '            wStr += ") And iWorkFlowStageId = " + (Convert.ToInt32(Me.Session(S_WorkFlowStageId)) - 10).ToString()
    '            wStr += " And cDataStatus <> '" + CRF_DataEntry + "'"

    '            If Not Me.objHelp.GetCRFDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                            ds_CRFDtl, eStr) Then
    '                Throw New Exception(eStr)
    '            End If

    '            ''wStr = "iDCFBy = " + Convert.ToString(Me.Session(S_UserID)).Trim() + " And (cDCFStatus = '"
    '            'wStr = " (cDCFStatus = '"
    '            'wStr += Discrepancy_Generated + "' Or cDCFStatus = '" + Discrepancy_Answered + "') And nCRFDtlNo in("
    '            'If Not ds_CRFDtl Is Nothing Then
    '            '    For Each dr In ds_CRFDtl.Tables(0).Rows
    '            '        wStr += Convert.ToString(dr("nCRFDtlNo")).Trim() + ","
    '            '    Next dr
    '            'End If
    '            'wStr = wStr.Substring(0, wStr.LastIndexOf(",")) + ")"

    '            'If Not Me.objHelp.GetDCFMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '            '                            ds_DCF, eStr) Then
    '            '    Throw New Exception(eStr)
    '            'End If
    '            wStr = " (cDCFStatus = '" + Discrepancy_Generated + "' Or cDCFStatus = '" + Discrepancy_Answered + "')"
    '            wStr += " And (((vUserTypeCode = '" + Convert.ToString(Me.Session(S_UserType)).Trim() _
    '                    + "' Or CRFWorkflowBy < " + Me.Session(S_WorkFlowStageId).ToString() + ") And cDCFType = 'M') "
    '            wStr += " Or cDCFType = 'S') And CRFWorkflowBy <> " & WorkFlowStageId_OnlyView
    '            wStr += " And nCRFDtlNo in("
    '            If Not ds_CRFDtl Is Nothing Then
    '                For Each dr In ds_CRFDtl.Tables(0).Rows
    '                    wStr += Convert.ToString(dr("nCRFDtlNo")).Trim() + ","
    '                Next dr
    '            End If
    '            wStr = wStr.Substring(0, wStr.LastIndexOf(",")) + ")"

    '            If Not Me.objHelp.View_DCFMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                        ds_DCF, eStr) Then
    '                Throw New Exception(eStr)
    '            End If
    '            If Not ds_CRFDtl Is Nothing AndAlso Not ds_DCF Is Nothing Then
    '                For Each dr In ds_CRFDtl.Tables(0).Rows
    '                    dr_DCF = ds_DCF.Tables(0).Select("nCRFDtlNo = " + Convert.ToString(dr("nCRFDtlNo")).Trim())
    '                    If dr_DCF.Length > 0 Then
    '                        dr("cStatusIndi") = "D"
    '                        ds_CRFDtl.Tables(0).AcceptChanges()
    '                    End If
    '                Next dr
    '            End If
    '            dv_CRFDtl = ds_CRFDtl.Tables(0).Copy.DefaultView
    '            dv_CRFDtl.RowFilter = "cStatusIndi <> 'D'"
    '            ds_CRFDtl = Nothing
    '            ds_CRFDtl = New DataSet
    '            ds_CRFDtl.Tables.Add(dv_CRFDtl.ToTable().Copy())

    '            If Not Me.objHelp.GetCRFWorkFlowDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
    '                                ds_CRFWorkFlowDtl, eStr) Then
    '                Throw New Exception(eStr)
    '            End If

    '            For Index As Integer = 0 To ds_CRFDtl.Tables(0).Rows.Count - 1

    '                If Convert.ToString(ds_CRFDtl.Tables(0).Rows(Index)("iWorkFlowStageId")).Trim.ToUpper() = _
    '                                            CType(Me.Session(S_WorkFlowStageId), String).ToUpper() Then
    '                    ds_CRFDtl.Tables(0).Rows(Index).Delete()
    '                    Continue For
    '                End If

    '                ds_CRFDtl.Tables(0).Rows(Index)("iWorkFlowStageId") = Me.Session(S_WorkFlowStageId)
    '                ds_CRFDtl.Tables(0).Rows(Index)("cDataStatus") = CRF_ReviewCompleted
    '                If CType(Me.Session(S_WorkFlowStageId), String).ToUpper() = WorkFlowStageId_FinalReviewAndLock Then
    '                    ds_CRFDtl.Tables(0).Rows(Index)("cDataStatus") = CRF_Locked
    '                End If

    '                dr_WorkFlowDtl = ds_CRFWorkFlowDtl.Tables(0).NewRow()
    '                dr_WorkFlowDtl("nCRFWorkFlowNo") = 0
    '                dr_WorkFlowDtl("nCRFDtlNo") = Convert.ToString(ds_CRFDtl.Tables(0).Rows(Index)("nCRFDtlNo")).Trim()
    '                dr_WorkFlowDtl("iTranNo") = 0
    '                dr_WorkFlowDtl("iWorkFlowStageId") = Me.Session(S_WorkFlowStageId)
    '                dr_WorkFlowDtl("iModifyBy") = Me.Session(S_UserID)
    '                dr_WorkFlowDtl("cStatusIndi") = "N"
    '                dr_WorkFlowDtl("cReviewFlag") = "A"
    '                ds_CRFWorkFlowDtl.Tables(0).Rows.Add(dr_WorkFlowDtl)
    '                ds_CRFWorkFlowDtl.AcceptChanges()

    '            Next Index

    '            ds_CRFDtl.AcceptChanges()
    '            If ds_CRFDtl.Tables(0).Rows.Count > 0 Then
    '                If Not Me.objLambda.Save_CRFDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
    '                                ds_CRFDtl, Me.Session(S_UserID), eStr) Then
    '                    Throw New Exception(eStr)
    '                End If
    '            End If

    '            If ds_CRFWorkFlowDtl.Tables(0).Rows.Count > 0 Then
    '                If Not Me.objLambda.Save_CRFWorkFlowDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
    '                               ds_CRFWorkFlowDtl, Me.Session(S_UserID), eStr) Then
    '                    Throw New Exception(eStr)
    '                End If
    '            End If

    '            Me.objCommon.ShowAlert("Review All Completed Successfully.", Me.Page)
    '            'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "DivAuthenticationHideShow('H');", True)
    '            Me.MpeAuthentication.Hide()

    '        Catch ex As Exception
    '            Me.ShowErrorMessage("Problem While Reviewing All Activities. ", eStr)
    '        End Try

    '    End Sub

    '    Protected Sub btnAuthenticate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAuthenticate.Click
    '        Dim wStr As String = ""
    '        Dim eStr As String = ""
    '        Dim ds_CRFWorkFlowDtl As New DataSet
    '        Dim dr As DataRow
    '        Dim ds_CRFDtl As New DataSet
    '        Dim Pwd As String = String.Empty

    '        Try

    '            Pwd = Me.txtPassword.Text.Trim()
    '            Pwd = objHelp.EncryptPassword(Pwd)

    '            If Pwd.ToUpper() <> CType(Me.Session(S_Password), String).ToUpper() Then
    '                Me.txtPassword.Text = ""
    '                objCommon.ShowAlert("Password Authentication Fails.", Me.Page)
    '                'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "DivAuthenticationHideShow('S');", True)
    '                Me.MpeAuthentication.Show()
    '                Me.txtPassword.Focus()
    '                Exit Sub
    '            End If

    '            If Me.chkReviewAll.Checked Then

    '                Me.ReviewAllActivities()
    '                Me.HFSessionFlg.Value = "1"
    '                Me.ddlActivities_SelectedIndexChanged(sender, e)
    '                Exit Sub

    '            End If

    '            If Me.HFCRFDtlNo.Value.Trim() = "" OrElse Me.Session(S_WorkFlowStageId) Is Nothing OrElse _
    '                                    Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = "" Then
    '                Me.objCommon.ShowAlert("CRFDtl No./WorkFlowStage Id Not Found.", Me.Page)
    '                Exit Sub
    '            End If
    '            '**************Checking if Review already done ******************

    '            'wStr = "nCRFDtlNo = " + Me.HFCRFDtlNo.Value.Trim() + " And iWorkFlowStageId = " + _
    '            '                    Me.Session(S_WorkFlowStageId) + " And cStatusIndi <> 'D'"

    '            'If Not Me.objHelp.GetCRFWorkFlowDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '            '                    ds_CRFWorkFlowDtl, eStr) Then
    '            '    Throw New Exception(eStr)
    '            'End If

    '            'If ds_CRFWorkFlowDtl.Tables(0) Is Nothing Then
    '            '    Throw New Exception("Review Details Not Found.")
    '            'End If

    '            'If ds_CRFWorkFlowDtl.Tables(0).Rows.Count > 0 Then
    '            '    Me.objCommon.ShowAlert("Review Already Done For Current User Type.", Me.Page)
    '            '    'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "DivAuthenticationHideShow('H');", True)
    '            '    Me.MpeAuthentication.Hide()
    '            '    Exit Sub
    '            'End If

    '            '*******************************************
    '            '*****Added for Re-review***********
    '            If Not Me.objHelp.GetCRFWorkFlowDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
    '                                ds_CRFWorkFlowDtl, eStr) Then
    '                Throw New Exception(eStr)
    '            End If
    '            '***********************************
    '            ds_CRFWorkFlowDtl.Tables(0).Clear()
    '            ds_CRFWorkFlowDtl.AcceptChanges()

    '            dr = ds_CRFWorkFlowDtl.Tables(0).NewRow()
    '            'nCRFWorkFlowNo,nCRFDtlNo,iTranNo,iWorkFlowStageId,dModifyOn,iModifyBy,cStatusIndi
    '            dr("nCRFWorkFlowNo") = 0
    '            dr("nCRFDtlNo") = Me.HFCRFDtlNo.Value.Trim()
    '            dr("iTranNo") = 0
    '            dr("iWorkFlowStageId") = Me.Session(S_WorkFlowStageId)
    '            dr("iModifyBy") = Me.Session(S_UserID)
    '            dr("cStatusIndi") = "N"
    '            dr("cReviewFlag") = "M"
    '            ds_CRFWorkFlowDtl.Tables(0).Rows.Add(dr)
    '            ds_CRFWorkFlowDtl.AcceptChanges()

    '            If Not Me.objLambda.Save_CRFWorkFlowDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
    '                    ds_CRFWorkFlowDtl, Me.Session(S_UserID), eStr) Then
    '                Throw New Exception(eStr)
    '            End If

    '            '*********Updating datastatus flag for review
    '            wStr = "nCRFDtlNo = " + Me.HFCRFDtlNo.Value.Trim() + " And cStatusIndi <> 'D'"

    '            If Not Me.objHelp.GetCRFDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                ds_CRFDtl, eStr) Then
    '                Throw New Exception(eStr)
    '            End If

    '            For Each dr In ds_CRFDtl.Tables(0).Rows
    '                dr("cDataStatus") = CRF_ReviewCompleted
    '                dr("iWorkFlowStageId") = Me.Session(S_WorkFlowStageId)
    '                If CType(Me.Session(S_WorkFlowStageId), String).ToUpper() = WorkFlowStageId_FinalReviewAndLock Then
    '                    dr("cDataStatus") = CRF_Locked
    '                End If
    '            Next
    '            ds_CRFDtl.AcceptChanges()
    '            If Not Me.objLambda.Save_CRFDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
    '                            ds_CRFDtl, Me.Session(S_UserID), eStr) Then
    '                Throw New Exception(eStr)
    '            End If
    '            '**********************************

    '            Me.objCommon.ShowAlert("Review Completed Successfully.", Me.Page)

    '            'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "DivAuthenticationHideShow('H');", True)
    '            Me.MpeAuthentication.Hide()
    '            Me.HFSessionFlg.Value = "1"
    '            Me.ddlActivities_SelectedIndexChanged(sender, e)

    '        Catch ex As Exception
    '            Me.ShowErrorMessage("Problem While Review Completed : ", ex.Message)
    '        End Try

    '    End Sub

    '#End Region

#Region "Repeatation Related"

    Private Function fillRepeatation() As Boolean
        Dim ds_AuditTrail As New DataSet
        Dim dv_AuditTrail As New DataView
        Dim Wstr As String = ""
        Dim estr As String = ""

        Try

            Wstr = Me.HFSchemaId.Value.Trim() + "##" + Me.HFWorkspaceId.Value.Trim() + "##" + _
                   Me.HFSubjectId.Value.Trim() + "##" + Me.HFNodeId.Value.Trim() + "##" + Me.HFPeriodId.Value.Trim() + "##" + "0" + "##" + "" + "##" + "0"


            If Not Me.objHelp.Proc_CRFHdrDtlSubDtl_Archive(Wstr, ds_AuditTrail, estr) Then
                Exit Function
            End If

            dv_AuditTrail = ds_AuditTrail.Tables(0).DefaultView().ToTable(True, "iRepeatNo,dRepeatationDate,CRFDtlChangedBy".Split(",")).DefaultView()
            dv_AuditTrail.Sort = "iRepeatNo desc"

            Me.ddlRepeatNo.Items.Clear()

            For Each dr As DataRow In dv_AuditTrail.ToTable().Rows

                Me.ddlRepeatNo.Items.Add(New ListItem(dr("iRepeatNo").ToString() + "-" + _
                                        IIf(dr("CRFDtlChangedBy").ToString.Trim() = "", "", "" + _
                                        dr("CRFDtlChangedBy").ToString() + " On: " + CType(dr("dRepeatationDate"), Date).ToString("dd-MMM-yyyy-hh:mm tt")), _
                                        dr("iRepeatNo").ToString()))
            Next dr

            'If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

            Me.ddlRepeatNo.Items.Insert(0, New ListItem("New Repeatation", "N"))
            Me.ddlRepeatNo.SelectedIndex = 0

            If Me.ddlRepeatNo.Items.Count > 1 Then

                'If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_DataEntry Then

                Me.ddlRepeatNo.SelectedItem.Enabled = False
                Me.ddlRepeatNo.SelectedIndex = 1

                'End If
                Me.ddlRepeatNo.SelectedIndex = 1

            End If

            'End If

            If Not Me.Session(S_SelectedRepeatation) Is Nothing Then
                If Convert.ToString(Me.Session(S_SelectedRepeatation)).ToUpper() = "N" Then
                    Me.ddlRepeatNo.SelectedIndex = 0
                Else
                    Me.ddlRepeatNo.SelectedIndex = Me.ddlRepeatNo.Items.IndexOf(Me.ddlRepeatNo.Items.FindByValue(CType(Me.Session(S_SelectedRepeatation), Integer).ToString()))
                End If
            End If
            Me.Session(S_SelectedRepeatation) = Nothing

            If Me.ddlRepeatNo.SelectedValue.Trim.ToUpper() = "N" Then
                Me.HFReviewedWorkFlowId.Value = ""
                Me.HFImportedDataWorkFlowId.Value = ""
                Me.lblLastReviewedBy.Text = ""
            End If

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

    Protected Sub ddlRepeatNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRepeatNo.SelectedIndexChanged
        Me.Session(S_SelectedRepeatation) = Me.ddlRepeatNo.SelectedValue.Trim()
        Me.HFSessionFlg.Value = "1"
        Me.ddlActivities_SelectedIndexChanged(sender, e)
    End Sub

#End Region

#Region "GetAuditButtons"

    'Private Function GetBrowseButton(ByVal vButtonName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String) As Button
    '    Dim btn As Button
    '    btn = New Button
    '    btn.ID = "btnBrowse" & MedExGroupCode + MedExSubGroupCode + MedExCode
    '    btn.Text = vButtonName.Trim()
    '    btn.CssClass = "button"

    '    btn.Style.Add("display", "block")

    '    'Condition changed for view mode -- Pratiksha
    '    If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_OnlyView Or Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
    '        btn.Style.Add("display", "none")
    '    End If

    '    If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_MedicalCoding Then
    '        btn.Attributes.Add("disabled", "true")
    '    End If

    '    btn.OnClientClick = "return MeddraBrowser('" + MedExCode + "');"
    '    GetBrowseButton = btn
    'End Function

    'Private Function GetAutoCalculateButton(ByVal vButtonName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal MedExFormula As String) As Button
    '    Dim btn As Button
    '    btn = New Button
    '    btn.ID = "btnAutoCalculate" & MedExGroupCode + MedExSubGroupCode + MedExCode
    '    btn.Text = vButtonName.Trim()
    '    btn.CssClass = "button"
    '    btn.OnClientClick = "return MedExFormula('" + MedExCode + "','" + MedExFormula + "');"
    '    GetAutoCalculateButton = btn
    'End Function

    'Private Function GetEditButton(ByVal vButtonName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal CRFDtlNo As String) As ImageButton
    '    Dim btn As ImageButton
    '    btn = New ImageButton
    '    btn.ID = "btnEdit" & MedExGroupCode + MedExSubGroupCode + MedExCode
    '    btn.ToolTip = vButtonName.Trim()

    '    If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_FirstReview AndAlso _
    '                Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_DataEntry AndAlso _
    '                Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_MedicalCoding AndAlso _
    '                Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_DataValidator Then
    '        btn.Attributes.Add("disabled", "true")
    '    End If

    '    btn.OnClientClick = "return AuditDivShowHide('E','" + MedExCode + "','" + MedExGroupCode + MedExSubGroupCode + MedExCode + "','" + CRFDtlNo + "');"
    '    btn.SkinID = "ImgBtnEdit"

    '    GetEditButton = btn
    'End Function

    'Private Function GetUpdateButton(ByVal vButtonName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal CRFDtlNo As String) As ImageButton
    '    Dim btn As ImageButton
    '    btn = New ImageButton
    '    btn.ID = "btnUpdate" & MedExGroupCode + MedExSubGroupCode + MedExCode
    '    btn.ToolTip = vButtonName.Trim()
    '    btn.Attributes.Add("disabled", "true")
    '    'btn.Style.Add("display", "none")
    '    btn.OnClientClick = "return AuditDivShowHide('U','" + MedExCode + "','" + MedExGroupCode + MedExSubGroupCode + MedExCode + "','" + CRFDtlNo + "');"
    '    btn.SkinID = "ImgBtnSave"

    '    GetUpdateButton = btn
    'End Function

    'Private Function GetDCFButton(ByVal vButtonName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal CRFDtlNo As String, ByVal TranNo As Integer, ByVal DCFStatus As String) As ImageButton
    '    If Not Me.HFCRFDtlLockStatus.Value Is Nothing AndAlso _
    '                   Convert.ToString(Me.HFCRFDtlLockStatus.Value).Trim.ToUpper() <> CRFHdr_Locked Then
    '        Dim btn As ImageButton
    '        btn = New ImageButton
    '        btn.ID = "btnForDCF" & MedExGroupCode + MedExSubGroupCode + MedExCode
    '        btn.ToolTip = vButtonName.Trim()
    '        btn.OnClientClick = "return AuditDivShowHide('D','" + MedExCode + "','','" + CRFDtlNo + "');"
    '        btn.SkinID = "ImgBtnDCF"

    '        If TranNo > 0 AndAlso (DCFStatus = Discrepancy_Generated Or DCFStatus = Discrepancy_Answered) Then
    '            btn.SkinID = "ImgBtnDCFUpdated"
    '        End If
    '        GetDCFButton = btn
    '    End If
    'End Function

    'Private Function GetSaveRunTimeButton(ByVal vButtonName As String) As Button
    '    Dim btn As Button
    '    btn = New Button
    '    btn.ID = "btnSaveRunTime" & vButtonName.ToString()
    '    btn.ToolTip = "Save"
    '    btn.Text = "Save"
    '    btn.OnClientClick = "return  ('S','','','0');"
    '    btn.CssClass = "button"
    '    GetSaveRunTimeButton = btn
    'End Function

    Private Function GetAudittrailButton(ByVal vButtonName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal TranNo As Integer, ByVal CRFDtlNo As String, ByVal Remarks As String) As ImageButton
        Dim btn As ImageButton
        btn = New ImageButton
        btn.ID = "btnAudittrail" & MedExGroupCode + MedExSubGroupCode + MedExCode
        btn.ToolTip = vButtonName.Trim()

        btn.OnClientClick = "return HistoryDivShowHide('A','" + MedExCode + "','','','" + CRFDtlNo + "');"
        btn.SkinID = "ImgBtnAuditTrail"

        If TranNo > 1 AndAlso Remarks.Length > 0 Then
            btn.SkinID = "ImgBtnAuditTrailUpdated"
        End If
        GetAudittrailButton = btn
    End Function
    ''Enhancement in EDC
    Private Function GetAutoDateButton(ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal Repeatation As Integer) As Button
        Dim btn As Button
        btn = New Button
        btn.ID = "btnCurrDate" & MedExGroupCode + MedExSubGroupCode + MedExCode
        btn.ToolTip = "Current Date"
        btn.CssClass = "button"
        btn.Width = "45"

        btn.OnClientClick = "return GetCurrentDate('" & MedExCode + "R" + Convert.ToString(Repeatation) & "');" '('E','" + MedExCode + "','" + MedExGroupCode + MedExSubGroupCode + MedExCode + "','" + CRFDtlNo + "');"
        'btn.SkinID = "ImgBtnEdit"
        btn.Text = "Auto"

        GetAutoDateButton = btn
    End Function

    Private Function GetAutoTimeButton(ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal Repeatation As Integer) As Button
        Dim btn As Button
        btn = New Button
        btn.ID = "btnCurrTime" & MedExGroupCode + MedExSubGroupCode + MedExCode
        btn.ToolTip = "Current Time"
        btn.CssClass = "button"
        btn.Width = "45"

        btn.OnClientClick = "return GetCurrentTime('" & MedExCode + "R" + Convert.ToString(Repeatation) & "');" '('E','" + MedExCode + "','" + MedExGroupCode + MedExSubGroupCode + MedExCode + "','" + CRFDtlNo + "');"
        'btn.SkinID = "ImgBtnEdit"
        btn.Text = "Auto"

        GetAutoTimeButton = btn
    End Function

    Private Function GetClearButton(ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal Repeatation As Integer) As Button
        Dim btn As Button
        btn = New Button
        btn.ID = "btnClear" & MedExGroupCode + MedExSubGroupCode + MedExCode
        btn.ToolTip = "Clear"
        btn.CssClass = "button"
        btn.Width = "45"

        btn.OnClientClick = "return ClearVal('" & MedExCode + "R" + Convert.ToString(Repeatation) & "');" '('E','" + MedExCode + "','" + MedExGroupCode + MedExSubGroupCode + MedExCode + "','" + CRFDtlNo + "');"
        'btn.SkinID = "ImgBtnEdit"
        btn.Text = "Clear"

        GetClearButton = btn
    End Function
    '''''''''''''''''''''''''''''''

#End Region

#Region "Buttons Click Events"

    'Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
    '    Dim estr As String = ""
    '    Dim SubjectId As String = ""
    '    Dim WorkspaceId As String = ""
    '    Dim ActivityId As String = ""
    '    Dim NodeId As String = ""
    '    Dim PeriodId As String = ""
    '    Dim MySubjectNo As String = ""
    '    Dim DsSave As New DataSet

    '    Dim Dir As DirectoryInfo
    '    Dim Flinfo As FileInfo
    '    Dim TranNo_Retu As String = ""
    '    Dim FolderPath As String = ""

    '    Dim objCollection As ControlCollection
    '    Dim objControl As Control
    '    Dim ObjId As String = ""

    '    Try

    '        Me.ViewState(VS_DataStatus) = CRF_DataEntryCompleted

    '        SubjectId = Me.HFSubjectId.Value.Trim()
    '        WorkspaceId = Me.HFWorkspaceId.Value.Trim()
    '        ActivityId = Me.HFActivityId.Value.Trim()
    '        NodeId = Me.HFNodeId.Value.Trim()
    '        PeriodId = Me.HFPeriodId.Value.Trim()
    '        MySubjectNo = Me.HFMySubjectNo.Value.Trim()


    '        'If Me.Session(S_ScopeNo).ToString <> Scope_ClinicalTrial AndAlso _
    '        '    Me.txtContent.Text.Trim.ToString <> "" Then

    '        '    If Not AssignValuesForSequenceDeviation(DsSave) Then
    '        '        Me.objCommon.ShowAlert("Error While Assigning Data.", Me.Page)
    '        '        Exit Sub
    '        '    End If

    '        '    If Not objLambda.Insert_WorkspaceActivitySequenceDeviation(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
    '        '                                                           DsSave, Me.Session(S_UserID), estr) Then
    '        '        Me.objCommon.ShowAlert("Error While inserting Data.", Me.Page)
    '        '        Exit Sub
    '        '    End If
    '        'End If

    '        If Not AssignValues(SubjectId, WorkspaceId, ActivityId, NodeId, PeriodId, MySubjectNo, DsSave) Then
    '            Me.objCommon.ShowAlert("Error While Assigning Data.", Me.Page)
    '            Exit Sub
    '        End If

    '        If Not Me.objLambda.Save_CRFHdrDtlSubDtl(Me.ViewState(VS_Choice), DsSave, False, Me.Session(S_UserID), estr) Then
    '            Me.objCommon.ShowAlert(estr, Me.Page)
    '            Exit Sub
    '        Else

    '            objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection) 'Me.PlaceMedEx.Controls 
    '            For Each objControl In objCollection
    '                If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.FileUpload" Then
    '                    Dim filename As String = ""

    '                    If objControl.ID.ToString.Contains("FU") Then
    '                        ObjId = objControl.ID.ToString.Replace("FU", "")
    '                    Else
    '                        ObjId = objControl.ID.ToString.Trim()
    '                    End If
    '                    If Request.Files(objControl.ID).FileName = "" And _
    '                         Not IsNothing(CType(FindControl("hlnk" + ObjId), HyperLink)) AndAlso _
    '                         CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl <> "" Then

    '                        filename = Server.MapPath(CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl)

    '                        FolderPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail"))
    '                        FolderPath += WorkspaceId + "/" + NodeId + "/" + SubjectId + "/" + TranNo_Retu + "/"

    '                        Dir = New DirectoryInfo(FolderPath)
    '                        If Not Dir.Exists() Then
    '                            Dir.Create()
    '                        End If

    '                        Flinfo = New FileInfo(filename.Trim())
    '                        Flinfo.CopyTo(FolderPath + Flinfo.Name)

    '                    ElseIf Request.Files(objControl.ID).FileName <> "" Then
    '                        FolderPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail"))
    '                        FolderPath += WorkspaceId + "/" + NodeId + "/" + SubjectId + "/" + TranNo_Retu + "/"

    '                        Dir = New DirectoryInfo(FolderPath)
    '                        If Not Dir.Exists() Then
    '                            Dir.Create()
    '                        End If

    '                        filename = FolderPath + Path.GetFileName(Request.Files(objControl.ID).FileName.ToString.Trim())
    '                        Request.Files(objControl.ID).SaveAs(filename)

    '                    End If

    '                End If
    '            Next objControl

    '        End If

    '        Me.EditChecks()

    '        Me.objCommon.ShowAlert("Attributes Saved Successfully.", Me.Page)

    '        Me.Session.Remove("PlaceMedEx")
    '        Me.HFSessionFlg.Value = "1"
    '        Me.ddlActivities_SelectedIndexChanged(sender, e)

    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.ToString, "")
    '    End Try
    'End Sub

    'Protected Sub btnSaveAndContinue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAndContinue.Click

    '    Dim estr As String = ""
    '    Dim SubjectId As String = ""
    '    Dim RedirectStr As String = ""
    '    Dim WorkspaceId As String = ""
    '    Dim ActivityId As String = ""
    '    Dim NodeId As String = ""
    '    Dim PeriodId As String = ""
    '    Dim MySubjectNo As String = ""
    '    Dim DsSave As New DataSet

    '    Dim Dir As DirectoryInfo
    '    Dim Flinfo As FileInfo
    '    Dim TranNo_Retu As String = ""
    '    Dim FolderPath As String = ""

    '    Dim objCollection As ControlCollection
    '    Dim objControl As Control
    '    Dim ObjId As String = ""
    '    Dim StrRedirect As String = ""

    '    Try

    '        Me.ViewState(VS_DataStatus) = CRF_DataEntry

    '        SubjectId = Me.HFSubjectId.Value.Trim()
    '        WorkspaceId = Me.HFWorkspaceId.Value.Trim()
    '        ActivityId = Me.HFActivityId.Value.Trim()
    '        NodeId = Me.HFNodeId.Value.Trim()
    '        PeriodId = Me.HFPeriodId.Value.Trim()
    '        MySubjectNo = Me.HFMySubjectNo.Value.Trim()

    '        If Not AssignValuesForSaveAndContinue(SubjectId, WorkspaceId, ActivityId, NodeId, PeriodId, MySubjectNo, DsSave) Then
    '            Me.objCommon.ShowAlert("Error While Assigning Data.", Me.Page)
    '            Exit Sub
    '        End If

    '        If Not Me.objLambda.Save_CRFHdrDtlSubDtl(Me.ViewState(VS_Choice), DsSave, False, Me.Session(S_UserID), estr) Then
    '            Me.objCommon.ShowAlert(estr, Me.Page)
    '            Exit Sub
    '        Else

    '            objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection) 'Me.PlaceMedEx.Controls 
    '            For Each objControl In objCollection
    '                If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.FileUpload" Then
    '                    Dim filename As String = ""

    '                    If objControl.ID.ToString.Contains("FU") Then
    '                        ObjId = objControl.ID.ToString.Replace("FU", "")
    '                    Else
    '                        ObjId = objControl.ID.ToString.Trim()
    '                    End If
    '                    If Request.Files(objControl.ID).FileName = "" And _
    '                         Not IsNothing(CType(FindControl("hlnk" + ObjId), HyperLink)) AndAlso _
    '                         CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl <> "" Then

    '                        filename = Server.MapPath(CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl)

    '                        FolderPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail"))
    '                        FolderPath += WorkspaceId + "/" + NodeId + "/" + SubjectId + "/" + TranNo_Retu + "/"

    '                        Dir = New DirectoryInfo(FolderPath)
    '                        If Not Dir.Exists() Then
    '                            Dir.Create()
    '                        End If

    '                        Flinfo = New FileInfo(filename.Trim())
    '                        Flinfo.CopyTo(FolderPath + Flinfo.Name)

    '                    ElseIf Request.Files(objControl.ID).FileName <> "" Then
    '                        FolderPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail"))
    '                        FolderPath += WorkspaceId + "/" + NodeId + "/" + SubjectId + "/" + TranNo_Retu + "/"

    '                        Dir = New DirectoryInfo(FolderPath)
    '                        If Not Dir.Exists() Then
    '                            Dir.Create()
    '                        End If

    '                        filename = FolderPath + Path.GetFileName(Request.Files(objControl.ID).FileName.ToString.Trim())
    '                        Request.Files(objControl.ID).SaveAs(filename)

    '                    End If

    '                End If
    '            Next objControl

    '        End If

    '        Me.objCommon.ShowAlert("Attributes Saved Successfully.", Me.Page)

    '        Me.Session.Remove("PlaceMedEx")
    '        Me.HFSessionFlg.Value = "1"
    '        Me.ddlActivities_SelectedIndexChanged(sender, e)

    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.ToString, "")
    '    End Try
    'End Sub

    'Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click
    '    'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "AnyDivShowHide('S');", True)
    '    Me.MpeEditAttribute.Show()
    'End Sub

    'Protected Sub btnDCF_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDCF.Click
    '    If Not Me.fillDCFGrid() Then
    '        Exit Sub
    '    End If
    '    'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "AnyDivShowHide('DCFSHOW');", True)
    '    'Me.MpeDCF.Show()
    'End Sub

    'Protected Sub btnSaveDiscrepancy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDiscrepancy.Click
    '    Dim ds_DCF As New DataSet
    '    Dim drDCF As DataRow
    '    Dim controlid As String = ""
    '    Dim ObjControl As Control
    '    Dim discrepancy As String = ""
    '    Dim eStr As String = ""
    '    Dim ds_CRFDtl As New DataSet
    '    Dim wStr As String = String.Empty

    '    Try

    '        ds_DCF.Tables.Add(CType(Me.ViewState(VS_DtDCF), DataTable).Copy())

    '        'nDCFNo,nCRFSubDtlNo,cDCFType,iDCFBy,dDCFDate,vDiscrepancy,vSourceResponse
    '        'cDCFStatus,iStatusChangedBy,dStatusChangedOn,iModifyBy,

    '        drDCF = ds_DCF.Tables(0).NewRow()
    '        controlid = Me.hfMedexCode.Value.Trim()

    '        If Not Me.GetControlValue(discrepancy, controlid, ObjControl) Then
    '            Me.objCommon.ShowAlert("Problem While Editing Attribute Detail", Me.Page)
    '            Exit Sub
    '        End If

    '        If Me.HFCRFHdrNo.Value.Trim() = "" OrElse Me.ddlRepeatNo.SelectedItem.Value.Trim.ToUpper() = "N" Then
    '            Me.objCommon.ShowAlert("CRFHdrNo Not Available", Me.Page())
    '            Exit Sub
    '        End If

    '        wStr = "nCRFHdrNo = " + Me.HFCRFHdrNo.Value.Trim() + " And vSubjectId = '" + Me.HFSubjectId.Value.Trim() + _
    '                "' And iRepeatNo = " + Me.hfMedexCode.Value.Substring(Me.hfMedexCode.Value.IndexOf("R") + 1) + " And cStatusIndi <> 'D'"

    '        If Not Me.objHelp.GetCRFDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                    ds_CRFDtl, eStr) Then
    '            Throw New Exception("Error While Getting Data From CRFDtl. " + eStr)
    '        End If

    '        If ds_CRFDtl Is Nothing Then
    '            Throw New Exception("CRFDtl No. Not found.")
    '        End If

    '        If ds_CRFDtl.Tables(0).Rows.Count < 0 Then
    '            Throw New Exception("CRFDtl No. Not Available.")
    '        End If

    '        drDCF("nDCFNo") = 0
    '        drDCF("nCRFDtlNo") = ds_CRFDtl.Tables(0).Rows(0).Item("nCRFDtlNo")
    '        drDCF("iSrNo") = 0
    '        drDCF("vMedExcode") = Me.hfMedexCode.Value.Substring(0, Me.hfMedexCode.Value.IndexOf("R"))
    '        drDCF("cDCFType") = "M"
    '        drDCF("iDCFBy") = Me.Session(S_UserID)
    '        drDCF("dDCFDate") = System.DateTime.Now()
    '        drDCF("vDiscrepancy") = discrepancy
    '        drDCF("vSourceResponse") = Me.txtDiscrepancyRemarks.Text.Trim()
    '        drDCF("cDCFStatus") = Me.ddlDiscrepancyStatus.SelectedItem.Value.Trim()
    '        drDCF("iModifyBy") = Me.Session(S_UserID)
    '        ds_DCF.Tables(0).Rows.Add(drDCF)
    '        ds_DCF.AcceptChanges()

    '        If Not Me.objLambda.Save_DCFMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
    '                                ds_DCF, Me.Session(S_UserID), eStr) Then
    '            Throw New Exception(eStr)
    '        End If

    '        Me.objCommon.ShowAlert("Discrepancy Added Successfully.", Me.Page)
    '        'Me.txtDiscrepancyRemarks.Text = ""
    '        If Not Me.fillDCFGrid() Then
    '            Exit Sub
    '        End If
    '        'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "AnyDivShowHide('DCFSHOW');", True)
    '        'Me.MpeDCF.Show()
    '        Me.HFSessionFlg.Value = "1"
    '        Me.ddlActivities_SelectedIndexChanged(sender, e)

    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.Message, "")
    '    End Try
    'End Sub

    'Protected Sub btnSaveRemarksForAttribute_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveRemarksForAttribute.Click
    '    Dim wStr As String = ""
    '    Dim eStr As String = ""
    '    Dim ds_CRFSubDtl As New DataSet
    '    Dim dr As DataRow
    '    Dim ControlId As String
    '    Dim ObjControl As Control
    '    Dim dt_DCF As New DataTable
    '    Dim result_Retu As String = ""
    '    Dim ControlDesc As String = ""
    '    Dim ds_CRFDtl As New DataSet

    '    Try
    '        If Not Me.objHelp.GetCRFSubDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
    '                    ds_CRFSubDtl, eStr) Then
    '            Throw New Exception(eStr)
    '        End If

    '        dr = ds_CRFSubDtl.Tables(0).NewRow()
    '        ControlId = Me.hfMedexCode.Value.Trim()

    '        If Not Me.GetControlValue(result_Retu, ControlId, ObjControl) Then
    '            Me.objCommon.ShowAlert("Problem While Editing Attribute Detail", Me.Page)
    '            Exit Sub
    '        End If

    '        If Me.HFCRFHdrNo.Value.Trim() = "" OrElse Me.ddlRepeatNo.SelectedItem.Value.Trim.ToUpper() = "N" Then
    '            Me.objCommon.ShowAlert("CRFHdrNo Not Available", Me.Page())
    '            Exit Sub
    '        End If

    '        wStr = "nCRFHdrNo = " + Me.HFCRFHdrNo.Value.Trim() + " And vSubjectId = '" + Me.HFSubjectId.Value.Trim() + _
    '                "' And iRepeatNo = " + Me.hfMedexCode.Value.Substring(Me.hfMedexCode.Value.IndexOf("R") + 1) + " And cStatusIndi <> 'D'"

    '        If Not Me.objHelp.GetCRFDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                    ds_CRFDtl, eStr) Then
    '            Throw New Exception("Error While Getting Data From CRFDtl. " + eStr)
    '        End If

    '        If ds_CRFDtl Is Nothing Then
    '            Throw New Exception("CRFDtl No. Not found.")
    '        End If

    '        If ds_CRFDtl.Tables(0).Rows.Count < 0 Then
    '            Throw New Exception("CRFDtl No. Not Available.")
    '        End If

    '        dr("nCRFSubDtlNo") = 0
    '        dr("nCRFDtlNo") = ds_CRFDtl.Tables(0).Rows(0).Item("nCRFDtlNo")
    '        dr("iTranNo") = Me.HFMedexInfoDtlTranNo.Value.Trim()
    '        dr("vMedExCode") = Me.hfMedexCode.Value.Substring(0, Me.hfMedexCode.Value.IndexOf("R"))
    '        'dr("vMedExDesc") = No need to pass, because procedure is taking care of this
    '        dr("dMedExDateTime") = System.DateTime.Now
    '        dr("vMedexResult") = result_Retu
    '        dr("vModificationRemark") = Me.HdReasonDesc.Value.ToString()
    '        dr("iModifyBy") = Me.Session(S_UserID)
    '        dr("cStatusIndi") = "N"

    '        If CheckDiscrepancy(ObjControl, Me.hfMedexCode.Value.Trim(), dr("vMedexResult"), "S") Then
    '            dr("cStatusIndi") = "D"
    '            dt_DCF = CType(Me.ViewState(VS_DtDCF), DataTable)
    '            dt_DCF.TableName = "DCFMst"
    '            ds_CRFSubDtl.Tables.Add(dt_DCF.Copy())
    '            ds_CRFSubDtl.AcceptChanges()
    '        End If

    '        ds_CRFSubDtl.Tables(0).Rows.Add(dr)
    '        ds_CRFSubDtl.Tables(0).AcceptChanges()

    '        If Not Me.objLambda.Save_CRFSubDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
    '                    ds_CRFSubDtl, Me.Session(S_UserID), eStr) Then
    '            Throw New Exception(eStr)
    '        End If

    '        If ds_CRFDtl.Tables(0).Rows(0).Item("cDataStatus") = CRF_Review Then
    '            EditChecks()
    '        End If

    '        Me.objCommon.ShowAlert("Attribute Updated Successfully.", Me.Page)
    '        Me.txtRemarkForAttributeEdit.Text = ""
    '        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "AnyDivShowHide('H');", True)

    '        'If Not GenCall() Then
    '        '    Exit Sub
    '        'End If
    '        Me.HFSessionFlg.Value = "1"
    '        Me.ddlActivities_SelectedIndexChanged(sender, e)

    '    Catch ex As Exception
    '        Me.ShowErrorMessage("Error While Saving Remarks : ", ex.Message)
    '        Me.objCommon.ShowAlert("Error While Saving Remarks : " + ex.Message, Me.Page)
    '    End Try
    'End Sub

    Protected Sub btnAudittrail_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim MedexGroupCode As String = ""
        If Not fillGrid(MedexGroupCode) Then
            Exit Sub
        End If
        Me.MpeAuditTrail.Show()
    End Sub

    'Protected Sub btnUpdateDiscrepancy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateDiscrepancy.Click
    '    Dim wStr As String = ""
    '    Dim eStr As String = ""
    '    Dim dsDCF As New DataSet
    '    Dim dr As DataRow
    '    Dim controlId As String = ""
    '    Dim ObjControl As Control
    '    Dim discrepancy As String = ""

    '    Try
    '        If Me.ViewState(VS_DCFNo) Is Nothing OrElse CType(Me.ViewState(VS_DCFNo), String).Trim() = "" Then
    '            Me.objCommon.ShowAlert("DCF No. Not Found", Me.Page)
    '            Exit Sub
    '        End If

    '        wStr = "nDCFNo = " + CType(Me.ViewState(VS_DCFNo), String).Trim() + " And cStatusIndi <> 'D'"
    '        If Not Me.objHelp.GetDCFMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                    dsDCF, eStr) Then
    '            Throw New Exception(eStr)
    '        End If

    '        If dsDCF.Tables(0) Is Nothing OrElse dsDCF.Tables(0).Rows.Count < 1 Then
    '            Me.objCommon.ShowAlert("DCF No. Not Found", Me.Page)
    '            Exit Sub
    '        End If

    '        For Each dr In dsDCF.Tables(0).Rows
    '            controlId = Me.hfMedexCode.Value.Trim()

    '            If Not Me.GetControlValue(discrepancy, controlId, ObjControl) Then
    '                Me.objCommon.ShowAlert("Problem While Editing Attribute Detail", Me.Page)
    '                Exit Sub
    '            End If
    '            dr("vSourceResponse") = Me.txtDiscrepancyRemarks.Text.Trim()
    '            dr("cDCFStatus") = Me.ddlDiscrepancyStatus.SelectedItem.Value.Trim()
    '            dr("iStatusChangedBy") = Me.Session(S_UserID)
    '            dr("iModifyBy") = Me.Session(S_UserID)
    '        Next
    '        dsDCF.AcceptChanges()

    '        If Not Me.objLambda.Save_DCFMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
    '                                dsDCF, Me.Session(S_UserID), eStr) Then
    '            Throw New Exception(eStr)
    '        End If

    '        Me.objCommon.ShowAlert("Discrepancy Updated Successfully.", Me.Page)
    '        'Me.txtDiscrepancyRemarks.Text = ""
    '        If Not Me.fillDCFGrid() Then
    '            Exit Sub
    '        End If
    '        'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "AnyDivShowHide('DCFSHOW');", True)
    '        'Me.MpeDCF.Show()
    '        Me.HFSessionFlg.Value = "1"
    '        Me.ddlActivities_SelectedIndexChanged(sender, e)

    '    Catch ex As Exception
    '        Me.ShowErrorMessage("Error While Updating DCF. ", ex.Message)
    '    End Try
    'End Sub

    'Protected Sub btnMeddraBrowse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMeddraBrowse.Click
    '    Dim RedirectStr As String = ""
    '    Dim MedExCode As String = ""
    '    MedExCode = Me.hfMedexCode.Value.Trim()
    '    MedExCode = MedExCode.Substring(0, MedExCode.IndexOf("R"))
    '    RedirectStr = "window.open(""" + "frmCTMMeddraBrowse.aspx?MedExCode=" & MedExCode & """)"

    '    ScriptManager.RegisterClientScriptBlock(Me.Page(), Me.GetType(), "OpenWindow", RedirectStr, True)

    'End Sub

    'Protected Sub btnAutoCalculate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim formula As String = ""
    '    Dim MedExes() As String
    '    Dim index As Integer = 0
    '    Dim result As String = ""
    '    Dim objControl As Control
    '    Dim evaluator As New Evaluator
    '    Dim FinalResult As Double = 0
    '    Try

    '        formula = Me.HFMedExFormula.Value.Trim()
    '        MedExes = formula.Split("?")
    '        formula = formula.Replace("?", "")

    '        For index = 0 To MedExes.Length - 1
    '            If MedExes(index).Length = 5 Then


    '                If Not Me.GetControlValue(result, MedExes(index).Trim(), objControl) Then
    '                    Exit Sub
    '                End If
    '                formula = formula.Replace(MedExes(index).Trim(), result)

    '            End If
    '        Next index

    '        FinalResult = evaluator.Eval(formula)
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetResult", "SetFormulaResult('" + FinalResult.ToString() + "');", True)

    '    Catch ex As Exception
    '        Me.ShowErrorMessage("Error While Auto Calculation. ", ex.Message)
    '    End Try
    'End Sub

    Protected Sub BtnDosingTime_click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim objControl As Control
        Dim MedexId As String = "00650"
        Dim result As String = String.Empty

        If Not Me.GetControlValue(result, MedexId, objControl) Then
            Exit Sub
        End If
    End Sub

#End Region

    '#Region "AssignValuesForSequenceDeviation"

    '    'Private Function AssignValuesForSequenceDeviation(ByRef DsSave As DataSet) As Boolean
    '    '    Dim DsCRFDtl As New DataSet
    '    '    Dim DtSequenceDeviation As New DataTable

    '    '    Dim dr As DataRow

    '    '    Dim estr As String = String.Empty
    '    '    Dim ControlId As String = String.Empty


    '    '    Try
    '    '        If Not Me.objHelp.View_WorkspaceActivitySequenceDeviation("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
    '    '                                        DsSave, estr) Then
    '    '            Me.objCommon.ShowAlert("Error while getting WorkspaceActivitySequenceDeviation", Me)
    '    '            Return False
    '    '        End If

    '    '        DtSequenceDeviation = DsSave.Tables(0)
    '    '        dr = DtSequenceDeviation.NewRow
    '    '        dr("iSequenceDeviation") = 1
    '    '        dr("vWorkspaceId") = Me.HFWorkspaceId.Value
    '    '        dr("iPeriod") = Me.HFPeriodId.Value
    '    '        dr("vSubjectId") = Me.HFSubjectId.Value
    '    '        dr("iNodeId") = Me.HFNodeId.Value
    '    '        dr("vPendingNodes") = Me.HFPendingNode.Value
    '    '        dr("vRemarks") = Me.HFRemarks.Value
    '    '        dr("iUserId") = Me.Session(S_UserID).ToString()
    '    '        DtSequenceDeviation.Rows.Add(dr)
    '    '        DtSequenceDeviation.AcceptChanges()

    '    '        DsSave = Nothing
    '    '        DsSave = New DataSet
    '    '        DsSave.Tables.Add(DtSequenceDeviation.Copy())
    '    '        DsSave.AcceptChanges()

    '    '        Return True
    '    '    Catch ex As Exception
    '    '        Me.ShowErrorMessage(ex.Message, "")
    '    '        Return False
    '    '    End Try

    '    'End Function

    '#End Region

#Region "Fill And Helper Functions"

    Private Function GetControlValue(ByRef result As String, ByVal ControlId As String, ByRef objControl_Retu As Control) As Boolean
        Dim objCollection As ControlCollection
        Dim objControl As Control
        Dim ObjId As String = ""
        Dim Wstr As String = ""
        Dim estr As String = ""

        Try

            objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection)

            For Each objControl In objCollection

                If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.TextBox" Then
                    If objControl.ID.Trim.ToUpper() = ControlId.Trim.ToUpper() Then
                        If objControl.ID.ToString.Contains("txt") Then
                            ObjId = objControl.ID.ToString.Replace("txt", "")
                        Else
                            ObjId = objControl.ID.ToString.Trim()
                        End If
                        ObjId = ObjId.Substring(0, ObjId.IndexOf("R"))
                        result = Request.Form(objControl.ID)
                        objControl_Retu = objControl
                        Return True
                    End If

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" Then
                    If objControl.ID.Trim.ToUpper() = ControlId.Trim.ToUpper() Then
                        result = Request.Form(objControl.ID)
                        objControl_Retu = objControl
                        Return True
                    End If

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.FileUpload" Then
                    If objControl.ID.Trim.ToUpper() = ControlId.Trim.ToUpper() Then
                        Dim filename As String = ""
                        If objControl.ID.ToString.Contains("FU") Then
                            ObjId = objControl.ID.ToString.Replace("FU", "")
                        Else
                            ObjId = objControl.ID.ToString.Trim()
                        End If
                        ObjId = ObjId.Substring(0, ObjId.IndexOf("R"))

                        If Request.Files(objControl.ID).FileName = "" And _
                            Not IsNothing(CType(FindControl("hlnk" + ObjId), HyperLink)) AndAlso _
                            CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl <> "" Then

                            filename = CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl

                        ElseIf Request.Files(objControl.ID).FileName <> "" Then

                            filename = "~/" + System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail") + _
                                        Me.HFWorkspaceId.Value.Trim() + "/" + Me.HFNodeId.Value.Trim() + "/" + Me.HFSubjectId.Value.Trim() + "/" + Path.GetFileName(Request.Files(objControl.ID).FileName.ToString.Trim())
                        End If
                        result = filename
                        objControl_Retu = objControl
                        Return True
                    End If

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.RadioButtonList" Then
                    If objControl.ID.Trim.ToUpper() = ControlId.Trim.ToUpper() Then
                        Dim rbl As RadioButtonList = CType(FindControl(objControl.ID), RadioButtonList)
                        Dim StrChk As String = ""

                        For index As Integer = 0 To rbl.Items.Count - 1
                            StrChk += IIf(rbl.Items(index).Selected, rbl.Items(index).Value.ToString.Trim() + ",", "")
                        Next index

                        If StrChk.Trim() <> "" Then
                            StrChk = StrChk.Substring(0, StrChk.LastIndexOf(","))
                        End If
                        result = StrChk

                        If Convert.ToString(Me.HFRadioButtonValue.Value).Trim() = "NULL" Then
                            result = ""
                        End If
                        Me.HFRadioButtonValue.Value = ""
                        objControl_Retu = objControl
                        Return True
                    End If

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.CheckBoxList" Then
                    If objControl.ID.Trim.ToUpper() = ControlId.Trim.ToUpper() Then
                        Dim chkl As CheckBoxList = CType(FindControl(objControl.ID), CheckBoxList)
                        Dim StrChk As String = ""

                        For index As Integer = 0 To chkl.Items.Count - 1
                            StrChk += IIf(chkl.Items(index).Selected, chkl.Items(index).Value.ToString.Trim() + ",", "")
                        Next index

                        If StrChk.Trim() <> "" Then
                            StrChk = StrChk.Substring(0, StrChk.LastIndexOf(","))
                        End If
                        result = StrChk
                        objControl_Retu = objControl
                        Return True
                    End If

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.HiddenField" Then
                    If objControl.ID.Trim.ToUpper() = ControlId.Trim.ToUpper() Then
                        '******Adding Header & footer to the document**********************
                        Dim ds_WorkSpaceNodeHistory As New DataSet
                        Dim filename As String = ""
                        Dim versionNo As String = ""
                        Wstr = "vWorkSpaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And iNodeId=" + Me.HFNodeId.Value.Trim() + " And iStageId =" & GeneralModule.Stage_Authorized

                        If Not objHelp.GetViewWorkSpaceNodeHistory(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                ds_WorkSpaceNodeHistory, estr) Then
                            Me.ShowErrorMessage("Error While Getting Data From View_WorkSpaceNodeHistory", estr)
                        End If

                        If Not ds_WorkSpaceNodeHistory.Tables(0).Rows.Count < 1 Then

                            filename = ds_WorkSpaceNodeHistory.Tables(0).Rows(0).Item("vDocPath").ToString()
                            versionNo = ds_WorkSpaceNodeHistory.Tables(0).Rows(0).Item("vUSerVersion").ToString()
                        End If
                        '*****************************************
                        ObjId = ObjId.Substring(0, ObjId.IndexOf("R"))

                        If ObjId = GeneralModule.Medex_FilePath.Trim() Then
                            result = filename  'File Name from WorkspaceNodeHistory
                        ElseIf ObjId = GeneralModule.Medex_DownloadedBy.Trim() Then
                            result = Me.Session(S_UserID)
                        ElseIf ObjId = GeneralModule.Medex_VersionNo.Trim() Then
                            result = versionNo 'Version No from WorkspaceNodeHistory
                        ElseIf ObjId = GeneralModule.Medex_ReleaseDate.Trim() Then
                            result = Date.Now.Date.ToString("dd-MMM-yyyy") 'Version No from WorkspaceNodeHistory
                        Else
                            result = Request.Form(objControl.ID) 'CType(objControl, TextBox).Text 'CType(Me.FindControl(obj.GetId), TextBox).Text
                        End If
                        objControl_Retu = objControl
                        Return True
                    End If

                End If

            Next objControl

        Catch ex As Exception
            Return False
        End Try

    End Function

    Private Function fillGrid(ByRef MedexGroupCode As String) As Boolean
        Dim ds_AuditTrail As New DataSet
        Dim dv_AuditTrail As New DataView
        Dim Wstr As String = ""
        Dim estr As String = ""
        Dim dc_Audit As DataColumn

        Try

            If Me.HFCRFHdrNo.Value.Trim() = "" OrElse Me.ddlRepeatNo.SelectedItem.Value.Trim.ToUpper() = "N" Then
                Me.objCommon.ShowAlert("No History Available.", Me.Page())
                Exit Function
            End If

            'Wstr = "vSubjectId='" & Me.HFSubjectId.Value.Trim() & "' And vMedexCode='" & Me.hfMedexCode.Value.Substring(0, Me.hfMedexCode.Value.IndexOf("R")) & "' And " & _
            '        " nCRFHdrNo=" & Me.HFCRFHdrNo.Value.Trim() & " And iNodeId=" & Me.HFNodeId.Value.Trim() & _
            '        " And nCRFDtlNo=" & Me.HFCRFDtlNo.Value.Trim()


            'Wstr = "dbo" + "##" + Me.HFWorkspaceId.Value.Trim() + "##" + _
            '       Me.HFSubjectId.Value.Trim() + "##" + Me.HFNodeId.Value.Trim() + "##" + Me.HFPeriodId.Value.Trim()

            'Wstr = "nCRFHdrNo=" & Me.HFCRFHdrNo.Value.Trim() & " And vSubjectID = '" + Me.HFSubjectId.Value.Trim() + _
            '        "' And vMedExCode = '" + Me.hfMedexCode.Value.Substring(0, Me.hfMedexCode.Value.IndexOf("R")) + _
            '        "' And iNodeId=" + Me.HFNodeId.Value.Trim() + " And iRepeatNo = " + Me.hfMedexCode.Value.Substring(Me.hfMedexCode.Value.IndexOf("R") + 1)

            Wstr = Me.HFSchemaId.Value.Trim() + "##" + Me.HFWorkspaceId.Value.Trim() + "##" + _
                     Me.HFSubjectId.Value.Trim() + "##" + Me.HFNodeId.Value.Trim() + "##" + Me.HFPeriodId.Value.Trim() + "##" + _
                     Me.HFCRFHdrNo.Value.Trim() + "##" + Me.hfMedexCode.Value.Substring(0, Me.hfMedexCode.Value.IndexOf("R")) + "##" + _
                     Me.hfMedexCode.Value.Substring(Me.hfMedexCode.Value.IndexOf("R") + 1)

            If Not Me.objHelp.Proc_CRFHdrDtlSubDtl_Archive(Wstr, ds_AuditTrail, estr) Then
                Me.objCommon.ShowAlert("Problem While Getting Data Of Audit Trail. " + estr, Me.Page())
                Exit Function
            End If

            Me.lblMedexDescription.Text = ""

            If ds_AuditTrail.Tables(0).Rows.Count < 1 Then
                Exit Function
            End If

            Me.lblMedexDescription.EnableViewState = True
            Me.lblMedexDescription.Text = ds_AuditTrail.Tables(0).Rows(0).Item("vMedExDesc").ToString.Trim()
            Me.lblMedexDescription.CssClass = "Label"
            Me.lblMedexDescription.Width = "400"
            Me.lblMedexDescription.Style.Add("word-wrap", "break-word")
            Me.lblMedexDescription.Style.Add("white-space", "none")

            dc_Audit = New DataColumn("dModifyOnSubDtl_IST", System.Type.GetType("System.String"))
            ds_AuditTrail.Tables(0).Columns.Add("dModifyOnSubDtl_IST")
            ds_AuditTrail.AcceptChanges()
            For Each dr_dModifyOn In ds_AuditTrail.Tables(0).Rows
                dr_dModifyOn("dModifyOnSubDtl_IST") = Convert.ToString(dr_dModifyOn("dModifyOnSubDtl") + strServerOffset)

            Next
            ds_AuditTrail.AcceptChanges()

            dv_AuditTrail = ds_AuditTrail.Tables(0).DefaultView.ToTable(True, "iTranNo,vMedExResult,vModificationRemark,CRFSubDtlChangedBy,dModifyOnSubDtl,dModifyOnSubDtl_IST".Split(",")).DefaultView 'vSubjectId,iMySubjectNo,vMedExDesc
            'dv_AuditTrail.Sort = "iRepeatNo desc"

            Me.GVHistoryDtl.DataSource = dv_AuditTrail.ToTable()
            Me.GVHistoryDtl.DataBind()

            MedexGroupCode = ds_AuditTrail.Tables(0).Rows(0).Item("vMedExGroupCode").ToString.Trim()

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

    'Private Function fillDCFGrid() As Boolean
    '    Dim ds_DCF As New DataSet
    '    Dim Wstr As String = ""
    '    Dim estr As String = ""

    '    Try

    '        'If Me.HFCRFDtlNo.Value.Trim() = "" OrElse Me.ddlRepeatNo.SelectedItem.Value.Trim.ToUpper() = "N" Then
    '        '    Me.objCommon.ShowAlert("CRFDtlNo Not Available", Me.Page())
    '        '    Exit Function
    '        'End If

    '        'Wstr = "nCRFDtlNo=" & Me.HFCRFDtlNo.Value.Trim() & " And vMedExCode = '" + _
    '        '                        Me.hfMedexCode.Value.Substring(0, Me.hfMedexCode.Value.IndexOf("R")) + "' And cStatusIndi <> 'D'" '+ _
    '        ''" And iDCFBy = " + Me.ySession(S_UserID)

    '        If Me.HFCRFHdrNo.Value.Trim() = "" OrElse Me.ddlRepeatNo.SelectedItem.Value.Trim.ToUpper() = "N" Then
    '            Me.objCommon.ShowAlert("CRFHdrNo Not Available", Me.Page())
    '            Exit Function
    '        End If

    '        Wstr = "nCRFHdrNo=" & Me.HFCRFHdrNo.Value.Trim() & " And vSubjectID = '" + Me.HFSubjectId.Value.Trim() + _
    '                "' And vMedExCode = '" + Me.hfMedexCode.Value.Substring(0, Me.hfMedexCode.Value.IndexOf("R")) + _
    '                "' And iRepeatNo = " + Me.hfMedexCode.Value.Substring(Me.hfMedexCode.Value.IndexOf("R") + 1) + " And cStatusIndi <> 'D'"

    '        If Not Me.objHelp.View_DCFMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                         ds_DCF, estr) Then
    '            Throw New Exception(estr)
    '        End If
    '        Me.lblAttributeDCF.Text = ""
    '        Me.lblAttributeDCF.CssClass = "Label"
    '        Me.lblAttributeDCF.Width = "400"
    '        Me.lblAttributeDCF.Style.Add("word-wrap", "break-word")
    '        Me.lblAttributeDCF.Style.Add("white-space", "none")

    '        If ds_DCF.Tables(0).Rows.Count > 0 Then
    '            Me.lblAttributeDCF.Text = ds_DCF.Tables(0).Rows(0).Item("vMedExDesc").ToString.Trim()
    '        End If

    '        Me.GVWDCF.DataSource = ds_DCF
    '        Me.GVWDCF.DataBind()

    '        Me.txtDiscrepancyRemarks.Text = ""
    '        Me.ddlDiscrepancyStatus.SelectedIndex = -1
    '        Me.ddlDiscrepancyStatus.Enabled = False

    '        Me.txtDiscrepancyRemarks.Enabled = True
    '        Me.btnUpdateDiscrepancy.Visible = False
    '        Me.btnSaveDiscrepancy.Visible = True

    '        If Me.ViewState(VS_ReviewFlag) = "YES" AndAlso Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_OnlyView Then
    '            Me.btnSaveDiscrepancy.Enabled = False
    '        End If

    '        Return True

    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.Message, "")
    '        Return False
    '    End Try

    'End Function

    'Private Function fillQueryGrid() As Boolean
    '    Dim ds_EditChecksDtl As New DataSet
    '    Dim Wstr As String = ""
    '    Dim estr As String = ""

    '    Try

    '        Wstr = "vWorkspaceId = '" + Me.HFWorkspaceId.Value.Trim() + "'" & _
    '                " and vSubjectId='" & Me.HFSubjectId.Value.Trim() & "' And bIsQuery = 1" & _
    '                " And iNodeId=" & Me.HFNodeId.Value.Trim()
    '        '''''Commented by pratiksha to solve the problem of show query as edit check operation page stores only period 1 for every editchecks
    '        '" and iPeriod=" & Me.HFPeriodId.Value.Trim() & 

    '        '" and nCRFDtlNo = " + Me.HFCRFDtlNo.Value.Trim() + "" & _
    '        If Not Me.objHelp.View_EditChecksDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                        ds_EditChecksDtl, estr) Then
    '            Throw New Exception(estr)
    '        End If

    '        Me.gvwQueries.DataSource = Nothing
    '        If Not ds_EditChecksDtl Is Nothing Then
    '            Me.gvwQueries.DataSource = ds_EditChecksDtl.Tables(0)
    '        End If
    '        Me.gvwQueries.DataBind()

    '        If Me.gvwQueries.Rows.Count > 0 Then
    '            Me.btnShowquery.Visible = True
    '            Me.btnShowquery.ToolTip = "No of queries on this page : " + Me.gvwQueries.Rows.Count.ToString()
    '            Me.btnShowquery.Text = "Show Query (" + Me.gvwQueries.Rows.Count.ToString() + ")"
    '            Me.btnShowquery.Style.Add("color", "Red")

    '            'To solve problem of query disply wrong number of queries
    '            'Me.btnShowquery.Attributes.Add("text", Me.gvwQueries.Rows.Count.ToString())
    '        End If

    '        Return True

    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.Message, "")
    '        Return False
    '    End Try

    'End Function

#End Region

    '#Region "DCFGrid Events"

    '    Protected Sub GVWDCF_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GVWDCF.RowCommand
    '        Dim rindex As Integer = e.CommandArgument
    '        Try
    '            If e.CommandName.ToUpper() = "UPDATE" Then

    '                Me.ViewState(VS_DCFNo) = Me.GVWDCF.Rows(rindex).Cells(GVCDCF_nDCFNo).Text.Trim()
    '                Me.txtDiscrepancyRemarks.Text = Me.GVWDCF.Rows(rindex).Cells(GVCDCF_vSourceResponse).Text.Trim()
    '                Me.txtDiscrepancyRemarks.Enabled = True
    '                Me.ddlDiscrepancyStatus.Enabled = True

    '                If Me.GVWDCF.Rows(rindex).Cells(GVCDCF_UserType).Text.Trim() = Me.Session(S_UserType) Then

    '                    Me.ddlDiscrepancyStatus.SelectedIndex = 2
    '                    If Me.GVWDCF.Rows(rindex).Cells(GVCDCF_cDCFType).Text.Trim().ToUpper() = "SYSTEM" Then
    '                        Me.ddlDiscrepancyStatus.SelectedIndex = 3
    '                    End If

    '                Else
    '                    Me.ddlDiscrepancyStatus.SelectedIndex = 1
    '                    If Me.GVWDCF.Rows(rindex).Cells(GVCDCF_cDCFType).Text.Trim().ToUpper() = "SYSTEM" Then
    '                        Me.ddlDiscrepancyStatus.SelectedIndex = 3
    '                    End If
    '                    If Me.Session(S_WorkFlowStageId) = WorkFlowStageId_DataEntry Then
    '                        Me.ddlDiscrepancyStatus.SelectedValue = "O"
    '                    End If
    '                    If Me.GVWDCF.Rows(rindex).Cells(GVCDCF_cDCFType).Text.Trim().ToUpper() = "SYSTEM" AndAlso _
    '                            Me.Session(S_WorkFlowStageId) = WorkFlowStageId_DataEntry Then
    '                        Me.ddlDiscrepancyStatus.SelectedIndex = 3
    '                    End If
    '                End If

    '                Me.ddlDiscrepancyStatus.Enabled = False
    '                Me.btnUpdateDiscrepancy.Visible = True
    '                Me.btnSaveDiscrepancy.Visible = False


    '            End If

    '            'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "AnyDivShowHide('DCFSHOW');", True)
    '            Me.MpeDCF.Show()

    '        Catch ex As Exception
    '            Me.ShowErrorMessage("Error While Updating DCF. ", ex.Message)
    '        End Try
    '    End Sub

    '    Protected Sub GVWDCF_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVWDCF.RowCreated
    '        If e.Row.RowType = DataControlRowType.Header Or _
    '                e.Row.RowType = DataControlRowType.DataRow Or _
    '                e.Row.RowType = DataControlRowType.Footer Then
    '            e.Row.Cells(GVCDCF_nDCFNo).Visible = False
    '            e.Row.Cells(GVCDCF_nCRFDtlNo).Visible = False
    '            e.Row.Cells(GVCDCF_vMedExCode).Visible = False
    '            e.Row.Cells(GVCDCF_iDCFBy).Visible = False
    '            e.Row.Cells(GVCDCF_UserType).Visible = False
    '        End If
    '    End Sub

    '    Protected Sub GVWDCF_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVWDCF.RowDataBound
    '        If e.Row.RowType = DataControlRowType.DataRow Then
    '            CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).CommandArgument = e.Row.RowIndex
    '            CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).CommandName = "UPDATE"


    '            ' '' Added on 12-May-2011 beacuse of aadding functionallity of Internally Resolved Functionallity ''
    '            'CType(e.Row.FindControl("LnkBtnInternallyResolved"), LinkButton).CommandArgument = e.Row.RowIndex
    '            'CType(e.Row.FindControl("LnkBtnInternallyResolved"), LinkButton).CommandName = "IR"
    '            ' '' ********************************************************************************************* ''


    '            e.Row.Cells(GVCDCF_iSrNo).Text = (e.Row.RowIndex + 1).ToString()

    '            If e.Row.Cells(GVCDCF_UserType).Text.Trim() = Me.Session(S_UserType).ToString() Then
    '                CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).Visible = True
    '            End If

    '            If e.Row.Cells(GVCDCF_cDCFStatus).Text.Trim.ToUpper() = Discrepancy_Answered And _
    '                (e.Row.Cells(GVCDCF_iDCFBy).Text.Trim.ToUpper() <> Convert.ToString(Me.Session(S_UserID)).Trim.ToUpper() And _
    '                 e.Row.Cells(GVCDCF_UserType).Text.Trim() <> Me.Session(S_UserType).ToString()) Then
    '                e.Row.Cells(GVCDCF_cDCFStatus).Text = "Answered"
    '                CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).Visible = False
    '            End If

    '            If e.Row.Cells(GVCDCF_cDCFStatus).Text.Trim.ToUpper() = Discrepancy_Generated Then
    '                e.Row.Cells(GVCDCF_cDCFStatus).Text = "Generated"

    '            ElseIf e.Row.Cells(GVCDCF_cDCFStatus).Text.Trim.ToUpper() = Discrepancy_Answered Then
    '                e.Row.Cells(GVCDCF_cDCFStatus).Text = "Answered"

    '            ElseIf e.Row.Cells(GVCDCF_cDCFStatus).Text.Trim.ToUpper() = Discrepancy_AutoResolved Then
    '                e.Row.Cells(GVCDCF_cDCFStatus).Text = "Auto Resolved"
    '                'CType(e.Row.FindControl("LnkBtnInternallyResolved"), LinkButton).Visible = False
    '                CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).Visible = False

    '            ElseIf e.Row.Cells(GVCDCF_cDCFStatus).Text.Trim.ToUpper() = Discrepancy_Resolved Then
    '                e.Row.Cells(GVCDCF_cDCFStatus).Text = "Resolved"
    '                CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).Visible = False

    '            ElseIf e.Row.Cells(GVCDCF_cDCFStatus).Text.Trim.ToUpper() = Discrepancy_InternallyResolved Then
    '                e.Row.Cells(GVCDCF_cDCFStatus).Text = " Internally Resolved"
    '                CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).Visible = False

    '            End If

    '            If e.Row.Cells(GVCDCF_UserType).Text.Trim() <> Me.Session(S_UserType).ToString() AndAlso _
    '                Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_DataEntry Then
    '                CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).Visible = False
    '            End If

    '            If e.Row.Cells(GVCDCF_cDCFType).Text.Trim.ToUpper() = "S" Then
    '                e.Row.Cells(GVCDCF_cDCFType).Text = "System"
    '                e.Row.Cells(GVCDCF_vCreatedBy).Text = "System"
    '                If e.Row.Cells(GVCDCF_cDCFStatus).Text.Trim.ToUpper() = Discrepancy_AutoResolved Then
    '                    e.Row.Cells(GVCDCF_vUpdatedBy).Text = "System"
    '                End If
    '                'CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).Visible = False

    '            ElseIf e.Row.Cells(GVCDCF_cDCFType).Text.Trim.ToUpper() = "M" Then
    '                e.Row.Cells(GVCDCF_cDCFType).Text = "Manual"

    '            End If

    '        End If
    '    End Sub

    '    Protected Sub GVWDCF_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GVWDCF.RowUpdating

    '    End Sub

    '#End Region

    '#Region "AssignValuesForSaveAndContinue"

    '    Private Function AssignValuesForSaveAndContinue(ByVal SubjectId As String, ByVal WorkspaceId As String, _
    '                                ByVal ActivityId As String, ByVal NodeId As String, _
    '                                ByVal PeriodId As String, _
    '                                ByVal MySubjectNo As String, ByRef DsSave As DataSet) As Boolean
    '        Dim DsCRFHdr As New DataSet
    '        Dim DsCRFDtl As New DataSet
    '        Dim DtCRFHdr As New DataTable
    '        Dim DtCRFDtl As New DataTable
    '        Dim DtCRFSubDtl As New DataTable

    '        Dim objCollection As ControlCollection
    '        Dim objControl As Control
    '        Dim ObjId As String = ""
    '        Dim dr As DataRow
    '        Dim TranNo As Integer = 0
    '        Dim Wstr As String = ""
    '        Dim estr As String = ""
    '        Dim dsNodeInfo As New DataSet
    '        Dim NodeIndex As String = ""
    '        Dim ds_DCF As New DataSet
    '        Dim dt_DCF As New DataTable

    '        Dim ControlDesc As String = String.Empty
    '        Dim ControlId As String = String.Empty

    '        Try

    '            Wstr = "vWorkSpaceId='" & WorkspaceId & "' and iNodeId='" & NodeId & "'" & _
    '                    " and vActivityId='" & ActivityId & "'"

    '            If Not Me.objHelp.getWorkSpaceNodeDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                            dsNodeInfo, estr) Then
    '                Me.objCommon.ShowAlert("Error while getting NodeIndex", Me)
    '                Return False
    '            End If
    '            NodeIndex = dsNodeInfo.Tables(0).Rows(0)("iNodeIndex").ToString()

    '            objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection) 'Me.PlaceMedEx.Controls 

    '            DtCRFHdr = CType(Me.ViewState(VS_DtCRFHdr), DataTable)
    '            DtCRFDtl = CType(Me.ViewState(VS_DtCRFDtl), DataTable)
    '            DtCRFSubDtl = CType(Me.ViewState(VS_DtCRFSubDtl), DataTable)

    '            '*********Checking MedEx values on 25-Dec-2009******************
    '            If Not Me.objHelp.GetDCFMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
    '                                    ds_DCF, estr) Then
    '                Exit Function
    '            End If
    '            Me.ViewState(VS_DtDCF) = ds_DCF.Tables(0).Copy()
    '            '*************************************

    '            If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
    '                If Not Me.objHelp.GetCRFHdr("nCRFHdrNo=" & CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows(0)("nCRFHdrNo"), _
    '                                                    WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                            DsCRFHdr, estr) Then
    '                    Exit Function
    '                End If
    '                DtCRFHdr = DsCRFHdr.Tables(0)

    '            Else

    '                DtCRFHdr.Clear()
    '                dr = DtCRFHdr.NewRow
    '                'nCRFHdrNo, vWorkSpaceId,dStartDate,iPeriod,iNodeId,iNodeIndex,vActivityId,cLockStatus
    '                dr("nCRFHdrNo") = 1
    '                If DtCRFHdr.Rows.Count > 0 Then
    '                    dr("nCRFHdrNo") = DtCRFHdr.Rows(0)("nCRFHdrNo")
    '                End If

    '                dr("vWorkSpaceId") = WorkspaceId
    '                dr("dStartDate") = System.DateTime.Now
    '                dr("iPeriod") = PeriodId
    '                dr("iNodeId") = NodeId
    '                dr("iNodeIndex") = NodeIndex
    '                dr("vActivityId") = ActivityId
    '                dr("cLockStatus") = "U" 'cLockStatus
    '                dr("iModifyBy") = Me.Session(S_UserID)
    '                dr("cStatusIndi") = "N"
    '                'dr.AcceptChanges()
    '                DtCRFHdr.Rows.Add(dr)
    '                DtCRFHdr.TableName = "CRFHdr"
    '                DtCRFHdr.AcceptChanges()
    '            End If

    '            If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
    '                If Not Me.objHelp.GetCRFDtl("nCRFDtlNo=" & CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows(0)("nCRFDtlNo"), _
    '                                                    WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                            DsCRFDtl, estr) Then
    '                    Exit Function
    '                End If
    '                DtCRFDtl = DsCRFDtl.Tables(0)

    '            Else

    '                DtCRFDtl.Clear()
    '                dr = DtCRFDtl.NewRow
    '                'nCRFDtlNo,nCRFHdrNo,iRepeatNo,dRepeatationDate,vSubjectId,iMySubjectNo,cLockStatus,iWorkFlowstageId
    '                dr("nCRFDtlNo") = 1
    '                If DtCRFDtl.Rows.Count > 0 Then
    '                    dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
    '                End If
    '                dr("nCRFHdrNo") = DtCRFHdr.Rows(0)("nCRFHdrNo").ToString.Trim()
    '                dr("iRepeatNo") = 1 'iRepeatNo
    '                dr("dRepeatationDate") = System.DateTime.Now
    '                dr("vSubjectId") = SubjectId
    '                dr("iMySubjectNo") = MySubjectNo
    '                dr("cLockStatus") = "U" 'cLockStatus
    '                dr("iWorkFlowstageId") = Me.Session(S_WorkFlowStageId)
    '                dr("iModifyBy") = Me.Session(S_UserID)
    '                dr("cStatusIndi") = "N"

    '                dr("cDataStatus") = CRF_DataEntryCompleted
    '                If CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_DataEntry Then
    '                    dr("cDataStatus") = CRF_DataEntry
    '                End If

    '                'dr.AcceptChanges()
    '                DtCRFDtl.Rows.Add(dr)
    '                DtCRFDtl.TableName = "CRFDtl"
    '                DtCRFDtl.AcceptChanges()
    '            End If

    '            DtCRFSubDtl.Clear()
    '            'For Detail Table
    '            For Each objControl In objCollection
    '                'nCRFSubDtlNo,nCRFDtlNo,iTranNo,vMedExCode,dMedExDatetime,vMedExResult,vModificationRemark

    '                If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.Label" AndAlso _
    '                           objControl.ID.ToString.Trim().Contains("Lnk") Then
    '                    Dim objLbl As Label = CType(objControl, Label)
    '                    ControlId = objControl.ID.ToString.Replace("Lnk", "")
    '                    ControlDesc = objLbl.Text
    '                End If

    '                If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.TextBox" Then
    '                    TranNo += 1
    '                    If objControl.ID.ToString.Contains("txt") Then
    '                        ObjId = objControl.ID.ToString.Replace("txt", "")
    '                    Else
    '                        ObjId = objControl.ID.ToString.Trim()
    '                    End If

    '                    dr = DtCRFSubDtl.NewRow
    '                    dr("nCRFSubDtlNo") = TranNo
    '                    dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
    '                    dr("iTranNo") = TranNo
    '                    dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
    '                    dr("vMedExDesc") = ControlDesc.Trim.Substring(0, ControlDesc.Trim.Length - 1)
    '                    dr("dMedExDateTime") = System.DateTime.Now
    '                    dr("vMedexResult") = Request.Form(objControl.ID) 'CType(objControl, TextBox).Text CType(Me.FindControl(obj.GetId), TextBox).Text
    '                    dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
    '                    dr("iModifyBy") = Me.Session(S_UserID)
    '                    dr("cStatusIndi") = "N"

    '                    'If Me.CheckDiscrepancy(objControl, ObjId, Request.Form(objControl.ID), "S") Then
    '                    '    dr("cStatusIndi") = "A"
    '                    'End If
    '                    DtCRFSubDtl.Rows.Add(dr)
    '                    DtCRFSubDtl.AcceptChanges()


    '                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" Then
    '                    TranNo += 1
    '                    ObjId = objControl.ID.ToString.Trim()
    '                    dr = DtCRFSubDtl.NewRow
    '                    dr("nCRFSubDtlNo") = TranNo
    '                    dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
    '                    dr("iTranNo") = TranNo
    '                    dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
    '                    dr("vMedExDesc") = ControlDesc.Trim.Substring(0, ControlDesc.Trim.Length - 1)
    '                    dr("dMedExDateTime") = System.DateTime.Now
    '                    dr("vMedexResult") = Request.Form(objControl.ID) 'CType(objControl, TextBox).Text 'CType(Me.FindControl(obj.GetId), TextBox).Text
    '                    dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
    '                    dr("iModifyBy") = Me.Session(S_UserID)
    '                    dr("cStatusIndi") = "N"
    '                    'If Me.CheckDiscrepancy(objControl, ObjId, Request.Form(objControl.ID), "S") Then
    '                    '    dr("cStatusIndi") = "A"
    '                    'End If
    '                    DtCRFSubDtl.Rows.Add(dr)
    '                    DtCRFSubDtl.AcceptChanges()

    '                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.FileUpload" Then
    '                    Dim filename As String = ""

    '                    TranNo += 1
    '                    If objControl.ID.ToString.Contains("FU") Then
    '                        ObjId = objControl.ID.ToString.Replace("FU", "")
    '                    Else
    '                        ObjId = objControl.ID.ToString.Trim()
    '                    End If


    '                    If Request.Files(objControl.ID).FileName = "" And _
    '                        Not IsNothing(CType(FindControl("hlnk" + ObjId), HyperLink)) AndAlso _
    '                        CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl <> "" Then

    '                        filename = CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl

    '                    ElseIf Request.Files(objControl.ID).FileName <> "" Then

    '                        filename = "~/" + System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail") + _
    '                                    WorkspaceId + "/" + NodeId + "/" + SubjectId + "/" + Path.GetFileName(Request.Files(objControl.ID).FileName.ToString.Trim())
    '                    End If

    '                    dr = DtCRFSubDtl.NewRow
    '                    dr("nCRFSubDtlNo") = TranNo
    '                    dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
    '                    dr("iTranNo") = TranNo
    '                    dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
    '                    dr("vMedExDesc") = ControlDesc.Trim.Substring(0, ControlDesc.Trim.Length - 1)
    '                    dr("dMedExDateTime") = System.DateTime.Now
    '                    dr("vMedexResult") = filename
    '                    dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
    '                    dr("iModifyBy") = Me.Session(S_UserID)
    '                    dr("cStatusIndi") = "N"
    '                    DtCRFSubDtl.Rows.Add(dr)
    '                    DtCRFSubDtl.AcceptChanges()

    '                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.RadioButtonList" Then
    '                    TranNo += 1
    '                    ObjId = objControl.ID.ToString.Trim()
    '                    dr = DtCRFSubDtl.NewRow
    '                    dr("nCRFSubDtlNo") = TranNo
    '                    dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
    '                    dr("iTranNo") = TranNo
    '                    dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
    '                    dr("vMedExDesc") = ControlDesc.Trim.Substring(0, ControlDesc.Trim.Length - 1)
    '                    dr("dMedExDateTime") = System.DateTime.Now
    '                    dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
    '                    dr("iModifyBy") = Me.Session(S_UserID)
    '                    dr("cStatusIndi") = "N"

    '                    Dim rbl As RadioButtonList = CType(objControl, RadioButtonList)
    '                    Dim StrChk As String = ""

    '                    For index As Integer = 0 To rbl.Items.Count - 1
    '                        StrChk += IIf(rbl.Items(index).Selected, rbl.Items(index).Value.ToString.Trim() + ",", "")
    '                    Next index

    '                    If StrChk.Trim() <> "" Then
    '                        StrChk = StrChk.Substring(0, StrChk.LastIndexOf(","))
    '                    End If
    '                    dr("vMedexResult") = StrChk
    '                    'If Me.CheckDiscrepancy(objControl, ObjId, StrChk, "S") Then
    '                    '    dr("cStatusIndi") = "A"
    '                    'End If
    '                    DtCRFSubDtl.Rows.Add(dr)
    '                    DtCRFSubDtl.AcceptChanges()

    '                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.CheckBoxList" Then
    '                    TranNo += 1
    '                    ObjId = objControl.ID.ToString.Trim()
    '                    dr = DtCRFSubDtl.NewRow
    '                    dr("nCRFSubDtlNo") = TranNo
    '                    dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
    '                    dr("iTranNo") = TranNo
    '                    dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
    '                    dr("vMedExDesc") = ControlDesc.Trim.Substring(0, ControlDesc.Trim.Length - 1)
    '                    dr("dMedExDateTime") = System.DateTime.Now
    '                    dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
    '                    dr("iModifyBy") = Me.Session(S_UserID)
    '                    dr("cStatusIndi") = "N"

    '                    Dim chkl As CheckBoxList = CType(objControl, CheckBoxList)
    '                    Dim StrChk As String = ""

    '                    For index As Integer = 0 To chkl.Items.Count - 1
    '                        StrChk += IIf(chkl.Items(index).Selected, chkl.Items(index).Value.ToString.Trim() + ",", "")
    '                    Next index

    '                    If StrChk.Trim() <> "" Then
    '                        StrChk = StrChk.Substring(0, StrChk.LastIndexOf(","))
    '                    End If
    '                    dr("vMedexResult") = StrChk
    '                    'If Me.CheckDiscrepancy(objControl, ObjId, StrChk, "S") Then
    '                    '    dr("cStatusIndi") = "A"
    '                    'End If
    '                    DtCRFSubDtl.Rows.Add(dr)
    '                    DtCRFSubDtl.AcceptChanges()

    '                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.HiddenField" Then
    '                    TranNo += 1
    '                    ObjId = objControl.ID.ToString.Trim()
    '                    dr = DtCRFSubDtl.NewRow

    '                    '******Adding Header & footer to the document**********************

    '                    Dim ds_WorkSpaceNodeHistory As New DataSet
    '                    Dim filename As String = ""
    '                    Dim versionNo As String = ""
    '                    Wstr = "vWorkSpaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And iNodeId=" + NodeId.Trim() + " And iStageId =" & GeneralModule.Stage_Authorized

    '                    If Not objHelp.GetViewWorkSpaceNodeHistory(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                                            ds_WorkSpaceNodeHistory, estr) Then
    '                        Me.ShowErrorMessage("Error While Getting Data From View_WorkSpaceNodeHistory", estr)
    '                    End If

    '                    If Not ds_WorkSpaceNodeHistory.Tables(0).Rows.Count < 1 Then

    '                        filename = ds_WorkSpaceNodeHistory.Tables(0).Rows(0).Item("vDocPath").ToString()
    '                        versionNo = ds_WorkSpaceNodeHistory.Tables(0).Rows(0).Item("vUSerVersion").ToString()
    '                    End If

    '                    '*****************************************
    '                    ObjId = ObjId.Substring(0, ObjId.IndexOf("R")).Trim()
    '                    If ObjId = GeneralModule.Medex_FilePath.Trim() Then
    '                        dr("vMedexResult") = filename  'File Name from WorkspaceNodeHistory
    '                    ElseIf ObjId = GeneralModule.Medex_DownloadedBy.Trim() Then
    '                        dr("vMedexResult") = Me.Session(S_UserID)
    '                    ElseIf ObjId = GeneralModule.Medex_VersionNo.Trim() Then
    '                        dr("vMedexResult") = versionNo 'Version No from WorkspaceNodeHistory
    '                    ElseIf ObjId = GeneralModule.Medex_ReleaseDate.Trim() Then
    '                        dr("vMedexResult") = Date.Now.Date.ToString("dd-MMM-yyyy") 'Version No from WorkspaceNodeHistory
    '                    Else
    '                        dr("vMedexResult") = Request.Form(objControl.ID) 'CType(objControl, TextBox).Text 'CType(Me.FindControl(obj.GetId), TextBox).Text
    '                    End If

    '                    dr("nCRFSubDtlNo") = TranNo
    '                    dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
    '                    dr("iTranNo") = TranNo
    '                    dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
    '                    dr("vMedExDesc") = ControlDesc.Trim.Substring(0, ControlDesc.Trim.Length - 1)
    '                    dr("dMedExDateTime") = System.DateTime.Now
    '                    dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
    '                    dr("iModifyBy") = Me.Session(S_UserID)
    '                    dr("cStatusIndi") = "N"
    '                    DtCRFSubDtl.Rows.Add(dr)
    '                    DtCRFSubDtl.AcceptChanges()

    '                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.Label" AndAlso _
    '                       objControl.ID.ToString.Trim().Contains("lbl") Then

    '                    Dim objLbl As Label = CType(objControl, Label)
    '                    ControlId = objControl.ID.ToString.Replace("Lnk", "")
    '                    ControlDesc = objLbl.Text

    '                    TranNo += 1
    '                    ObjId = objControl.ID.ToString.Replace("lbl", "")
    '                    dr = DtCRFSubDtl.NewRow
    '                    dr("nCRFSubDtlNo") = TranNo
    '                    dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
    '                    dr("iTranNo") = TranNo
    '                    dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
    '                    dr("vMedExDesc") = ""
    '                    dr("dMedExDateTime") = System.DateTime.Now
    '                    dr("vMedexResult") = ControlDesc.Trim()
    '                    dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
    '                    dr("iModifyBy") = Me.Session(S_UserID)
    '                    dr("cStatusIndi") = "N"
    '                    DtCRFSubDtl.Rows.Add(dr)
    '                    DtCRFSubDtl.AcceptChanges()

    '                End If

    '            Next objControl
    '            '****************************************

    '            DtCRFSubDtl.TableName = "CRFSubDtl"
    '            DtCRFSubDtl.AcceptChanges()

    '            DsSave = Nothing
    '            DsSave = New DataSet
    '            DsSave.Tables.Add(DtCRFHdr.Copy())
    '            DsSave.Tables.Add(DtCRFDtl.Copy())
    '            DsSave.Tables.Add(DtCRFSubDtl.Copy())
    '            DsSave.AcceptChanges()

    '            dt_DCF = CType(Me.ViewState(VS_DtDCF), DataTable).Copy()
    '            dt_DCF.TableName = "DCFMst"
    '            dt_DCF.AcceptChanges()
    '            DsSave.Tables.Add(dt_DCF.Copy())
    '            DsSave.AcceptChanges()

    '            Return True
    '        Catch ex As Exception
    '            Me.ShowErrorMessage(ex.Message, "")
    '            Return False
    '        End Try

    '    End Function

    '#End Region

#Region "Activity DropDown Related"

    Private Function FillActivities() As Boolean
        Dim ds As New DataSet
        Dim dv As New DataView
        Dim ds_ActivityStatus As New DataSet
        Dim dv_ActivityStatus As New DataView
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim clr As String = Drawing.ColorTranslator.ToHtml(Drawing.Color.Red)

        Try

            'added by deepak Singh to merge with BA-BE

            '''''''''''Scop filter is removed as BABE projects also can see child activity in dropdown
            wStr = " vWorkspaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And (iParentNodeId = " + Me.HFParentNodeId.Value.Trim() + " Or iNodeId = " + Me.HFNodeId.Value.Trim() + ")"
            If Me.HFType.Value = Type_BABE Then
                wStr = " vWorkspaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And iNodeId = " + Me.HFNodeId.Value.Trim()
            End If

            'If Me.Session(S_ScopeNo) = Scope_BABE Then
            '    wStr = " vWorkspaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And iNodeId = " + Me.HFNodeId.Value.Trim()
            'ElseIf Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
            '    wStr = " vWorkspaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And (iParentNodeId = " + Me.HFParentNodeId.Value.Trim() + " Or iNodeId = " + Me.HFNodeId.Value.Trim() + ")"
            'End If
            wStr += " AND cStatusindi <> 'D'"
            ''''''''''''''''''''''

            'Condition Added to identify subject specific study --Pratiksha
            If Me.HFSubjectId.Value.Trim() = "0000" AndAlso Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                wStr += " AND cSubjectWiseFlag = 'N'"
            ElseIf Me.HFSubjectId.Value.Trim() <> "0000" AndAlso Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                wStr += " AND cSubjectWiseFlag = 'Y'"
            End If
            '--Pratiksha

            wStr += " Order by iNodeNo "

            If Not Me.objHelp.GetViewWorkSpaceNodeDetail(wStr, ds, eStr) Then
                Throw New Exception(eStr)
            End If

            ds.Tables(0).Columns.Add("ActivityNodeId")
            ds.AcceptChanges()

            For Each dr As DataRow In ds.Tables(0).Rows
                dr("ActivityNodeId") = dr("vActivityId").ToString() + "#" + dr("iNodeId").ToString()
            Next
            ds.AcceptChanges()

            dv = ds.Tables(0).DefaultView

            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                dv.RowFilter = "iParentNodeId = " + Me.HFParentNodeId.Value.Trim()
                If dv.ToTable().Rows.Count < 1 Then
                    dv = ds.Tables(0).DefaultView
                    dv.RowFilter = "iNodeId = " + Me.HFNodeId.Value.Trim()
                End If
            End If

            Me.ddlActivities.DataSource = dv.ToTable()
            Me.ddlActivities.DataTextField = "vNodeDisplayName"
            Me.ddlActivities.DataValueField = "ActivityNodeId"
            Me.ddlActivities.DataBind()

            For Count As Integer = 0 To Me.ddlActivities.Items.Count - 1
                Me.ddlActivities.Items(Count).Attributes.Add("title", Me.ddlActivities.Items(Count).Text.Trim())
            Next Count

            If Not Me.Session(S_SelectedActivity) Is Nothing Then
                Me.ddlActivities.SelectedIndex = CType(Me.Session(S_SelectedActivity), Integer)
            End If

            Me.HFActivityId.Value = Me.ddlActivities.SelectedItem.Value.Substring(0, 4)
            Me.HFNodeId.Value = Me.ddlActivities.SelectedItem.Value.Substring(5)

            Me.Session(S_SelectedActivity) = Nothing

            '************Repeatation Show/Hode logic******************
            dv = ds.Tables(0).DefaultView
            dv.RowFilter = "vActivityId = '" + Me.HFActivityId.Value.Trim() + "' And iNodeId = " + Me.HFNodeId.Value.Trim()
            Me.ddlRepeatNo.Style.Add("display", "none")
            If dv.ToTable().Rows.Count > 0 Then
                If Convert.ToString(dv.ToTable().Rows(0)("cIsRepeatable")).Trim.ToUpper() = "Y" Then
                    Me.ddlRepeatNo.Style.Add("display", "block")
                End If
            End If
            '******************************

            ''****************Setting colors to activities for displaying their status**************
            ''added by deepak singh to merge in BA-BE

            'wStr = "vWorkspaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And iPeriod = " + Me.HFPeriodId.Value.Trim()
            'wStr += " And (iParentNodeId = " + Me.HFParentNodeId.Value.Trim() + " Or iNodeId = " + Me.HFNodeId.Value.Trim() + ") And vSubjectId = '" + Me.HFSubjectId.Value.Trim() + "'"

            'If Me.Session(S_ScopeNo) = Scope_BABE Then
            '    wStr = "vWorkspaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And iPeriod = " + Me.HFPeriodId.Value.Trim() + " And iNodeid = " + Me.HFNodeId.Value.Trim() + " And vSubjectId = '" + Me.HFSubjectId.Value.Trim() + "'"
            'End If

            'If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
            '    wStr = "vWorkspaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And iPeriod = " + Me.HFPeriodId.Value.Trim()
            '    wStr += " And (iParentNodeId = " + Me.HFParentNodeId.Value.Trim() + " Or iNodeId = " + Me.HFParentNodeId.Value.Trim() + ") And vSubjectId = '" + Me.HFSubjectId.Value.Trim() + "'"
            'End If
            ''=================================

            'If Not Me.objHelp.View_CRFActivityStatus(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '                    ds_ActivityStatus, eStr) Then
            '    Throw New Exception(eStr)
            'End If

            'Me.HFReviewedWorkFlowId.Value = ""
            'Me.HFImportedDataWorkFlowId.Value = ""
            'Me.lblLastReviewedBy.Text = ""

            'Me.ddlActivities.ForeColor = Drawing.Color.Red

            'If Not ds_ActivityStatus Is Nothing Then

            '    dv_ActivityStatus = ds_ActivityStatus.Tables(0).DefaultView

            '    If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
            '        dv_ActivityStatus.RowFilter = "iParentNodeId = " + Me.HFParentNodeId.Value.Trim()
            '        If dv_ActivityStatus.ToTable().Rows.Count < 1 Then
            '            dv_ActivityStatus = ds_ActivityStatus.Tables(0).DefaultView
            '            dv_ActivityStatus.RowFilter = "iNodeId = " + Me.HFNodeId.Value.Trim()
            '        End If
            '    End If

            '    For Each dr As DataRow In dv_ActivityStatus.ToTable().Rows

            '        'If Convert.ToString(dr("cCRFDtlDataStatus")).Trim.ToUpper() = CRF_DataEntryPending Then
            '        clr = Drawing.ColorTranslator.ToHtml(Drawing.Color.Red)
            '        'Else
            '        If Convert.ToString(dr("cCRFDtlDataStatus")).Trim.ToUpper() = CRF_DataEntry Then
            '            clr = Drawing.ColorTranslator.ToHtml(Drawing.Color.Orange)
            '            'ElseIf Convert.ToString(dr("cCRFDtlDataStatus")).Trim.ToUpper() = CRF_DataEntryCompleted Then

            '        ElseIf Convert.ToString(dr("cCRFDtlDataStatus")).Trim.ToUpper() = CRF_Review Then
            '            clr = Drawing.ColorTranslator.ToHtml(Drawing.Color.Blue)
            '        ElseIf Convert.ToString(dr("cCRFDtlDataStatus")).Trim.ToUpper() = CRF_ReviewCompleted Then
            '            If Convert.ToString(dr("iWorkFlowStageId")).Trim() = WorkFlowStageId_FirstReview Then
            '                clr = "#50C000"
            '            ElseIf Convert.ToString(dr("iWorkFlowStageId")).Trim() = WorkFlowStageId_SecondReview Then
            '                clr = "#006000"
            '            End If
            '        ElseIf Convert.ToString(dr("cCRFDtlDataStatus")).Trim.ToUpper() = CRF_Locked Then
            '            clr = Drawing.ColorTranslator.ToHtml(Drawing.Color.Gray)
            '        End If

            '        If Not Me.ddlActivities.Items.FindByValue(dr("vActivityId").ToString() + "#" + dr("iNodeId").ToString()) Is Nothing Then
            '            Me.ddlActivities.Items.FindByValue(dr("vActivityId").ToString() + "#" + dr("iNodeId").ToString()).Attributes.Add("style", "color:" + clr)
            '        End If

            '        '***********Reviewed By Displaying Logic********

            '        'Commented because color codes are displayed for indication
            '        'If Convert.ToString(dr("CRFReviewedBy")).Trim() <> "" Then

            '        '    Me.ddlActivities.Items.FindByValue(dr("vActivityId").ToString() + "#" + dr("iNodeId").ToString()).Attributes.Add("title", "Last Review Done By : " + Convert.ToString(dr("CRFReviewedBy")).Trim() + " - " + Convert.ToString(dr("CRFReviewedByUserType")).Trim())

            '        'End If

            '        '***********************************************

            '    Next

            '    Me.ViewState(VS_DataStatus) = CRF_DataEntryPending

            '    dv_ActivityStatus.RowFilter = "iNodeId = " + Me.HFNodeId.Value.Trim()
            '    If dv_ActivityStatus.ToTable().Rows.Count > 0 Then

            '        If Not Me.Session(S_SelectedRepeatation) Is Nothing AndAlso Convert.ToString(Me.Session(S_SelectedRepeatation)).ToUpper() <> "N" Then
            '            dv_ActivityStatus.RowFilter = "iNodeId = " + Me.HFNodeId.Value.Trim() + " And iRepeatNo = " + CType(Me.Session(S_SelectedRepeatation), Integer).ToString()
            '            If dv_ActivityStatus.ToTable().Rows.Count > 0 Then
            '                Me.HFReviewedWorkFlowId.Value = dv_ActivityStatus.ToTable().Rows(0)("iReviewedWorkFlowStageId")
            '                Me.HFImportedDataWorkFlowId.Value = dv_ActivityStatus.ToTable().Rows(0)("iWorkFlowStageId")
            '                Me.ViewState(VS_DataStatus) = dv_ActivityStatus.ToTable().Rows(0)("cCRFDtlDataStatus")
            '                If Convert.ToString(dv_ActivityStatus.ToTable().Rows(0)("CRFReviewedBy")).Trim() <> "" AndAlso Convert.ToString(dv_ActivityStatus.ToTable().Rows(0)("cCRFDtlDataStatus")).Trim() <> CRF_Review Then
            '                    Me.lblLastReviewedBy.Text = "Last Review Done By : " + Convert.ToString(dv_ActivityStatus.ToTable().Rows(0)("CRFReviewedBy")).Trim() + " - " + Convert.ToString(dv_ActivityStatus.ToTable().Rows(0)("CRFReviewedByUserType")).Trim()
            '                End If
            '            End If
            '        Else
            '            'dv_ActivityStatus.Sort = "iRepeatNo asc"
            '            'Me.HFReviewedWorkFlowId.Value = dv_ActivityStatus.ToTable().Rows(0)("iReviewedWorkFlowStageId")
            '            dv_ActivityStatus.Sort = "iRepeatNo Desc"
            '            Me.HFReviewedWorkFlowId.Value = dv_ActivityStatus.ToTable().Rows(0)("iReviewedWorkFlowStageId")
            '            Me.HFImportedDataWorkFlowId.Value = dv_ActivityStatus.ToTable().Rows(0)("iWorkFlowStageId")
            '            Me.ViewState(VS_DataStatus) = dv_ActivityStatus.ToTable().Rows(0)("cCRFDtlDataStatus")
            '            If Convert.ToString(dv_ActivityStatus.ToTable().Rows(0)("CRFReviewedBy")).Trim() <> "" AndAlso Convert.ToString(dv_ActivityStatus.ToTable().Rows(0)("cCRFDtlDataStatus")).Trim() <> CRF_Review Then
            '                Me.lblLastReviewedBy.Text = "Last Review Done By : " + Convert.ToString(dv_ActivityStatus.ToTable().Rows(0)("CRFReviewedBy")).Trim() + " - " + Convert.ToString(dv_ActivityStatus.ToTable().Rows(0)("CRFReviewedByUserType")).Trim()
            '            End If
            '        End If

            '    End If

            'End If
            ''*****************end of setting Activity status****************

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

    Protected Sub ddlActivities_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlActivities.SelectedIndexChanged

        Dim StrRedirect As String = String.Empty
        Dim choice As String = ""

        'If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
        '    choice = "1"
        'ElseIf CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
        '    choice = "2"
        'ElseIf CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
        '    choice = "4"
        'End If

        If Me.HFType.Value = Type_BABE Then
            'If Me.Session(S_ScopeNo) = Scope_BABE Then
            StrRedirect = "frmArchiveCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + Me.HFWorkspaceId.Value.Trim() + _
                              "&ActivityId=" + Me.HFParentActivityId.Value.Trim() + "&NodeId=" + Me.HFParentNodeId.Value.Trim() + _
                              "&PeriodId=" + Me.HFPeriodId.Value.Trim() + "&Type=" + Type_BABE + "&SubjectId=" + Me.HFSubjectId.Value.Trim() + _
                              "&MySubjectNo=" + Me.HFMySubjectNo.Value.Trim() + "&ScreenNo=" + Me.HFScreenNo.Value.Trim() + "&SchemaId=" + Me.HFSchemaId.Value.Trim()
            'If Not IsNothing(Me.Request.QueryString("Mode")) Then
            '    If Convert.ToString(Me.Request.QueryString("Mode")).Trim() <> "" Then
            '        StrRedirect += "&Mode=" + Me.Request.QueryString("Mode")
            '    End If
            'End If
        Else

            StrRedirect = "frmArchiveCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + Me.HFWorkspaceId.Value.Trim() + _
                              "&ActivityId=" + Me.HFParentActivityId.Value.Trim() + "&NodeId=" + Me.HFParentNodeId.Value.Trim() + _
                              "&PeriodId=" + Me.HFPeriodId.Value.Trim() + "&SubjectId=" + Me.HFSubjectId.Value.Trim() + _
                              "&MySubjectNo=" + Me.HFMySubjectNo.Value.Trim() + "&ScreenNo=" + Me.HFScreenNo.Value.Trim() + "&SchemaId=" + Me.HFSchemaId.Value.Trim()
            'If Not IsNothing(Me.Request.QueryString("Mode")) Then
            '    If Convert.ToString(Me.Request.QueryString("Mode")).Trim() <> "" Then
            '        StrRedirect += "&Mode=" + Me.Request.QueryString("Mode")
            '    End If
            'End If
        End If

        Me.Session(S_SelectedActivity) = Me.ddlActivities.SelectedIndex.ToString()

        Me.Session(S_SelectedTab) = Me.HFActivateTab.Value
        If Me.HFSessionFlg.Value = "" Then
            Me.Session(S_SelectedTab) = Nothing
        End If
        Me.HFActivateTab.Value = ""
        Me.Response.Redirect(StrRedirect)

    End Sub

#End Region

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Me.MpeAuthentication.Show()
    End Sub

    'Protected Sub btnSaveRunTime_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveRunTime.Click
    '    Dim estr As String = ""
    '    Dim SubjectId As String = ""
    '    Dim WorkspaceId As String = ""
    '    Dim ActivityId As String = ""
    '    Dim NodeId As String = ""
    '    Dim PeriodId As String = ""
    '    Dim MySubjectNo As String = ""
    '    Dim DsSave As New DataSet

    '    Dim Dir As DirectoryInfo
    '    Dim Flinfo As FileInfo
    '    Dim TranNo_Retu As String = ""
    '    Dim FolderPath As String = ""

    '    Dim objCollection As ControlCollection
    '    Dim objControl As Control
    '    Dim ObjId As String = ""

    '    Try

    '        Me.ViewState(VS_DataStatus) = CRF_DataEntryCompleted
    '        Me.ViewState(VS_Choice) = WS_HelpDbTable.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

    '        SubjectId = Me.HFSubjectId.Value.Trim()
    '        WorkspaceId = Me.HFWorkspaceId.Value.Trim()
    '        ActivityId = Me.HFActivityId.Value.Trim()
    '        NodeId = Me.HFNodeId.Value.Trim()
    '        PeriodId = Me.HFPeriodId.Value.Trim()
    '        MySubjectNo = Me.HFMySubjectNo.Value.Trim()

    '        'If Not AssignValues(SubjectId, WorkspaceId, ActivityId, NodeId, PeriodId, MySubjectNo, DsSave) Then
    '        '    Me.objCommon.ShowAlert("Error While Assigning Data.", Me.Page)
    '        '    Exit Sub
    '        'End If

    '        'If Not Me.objLambda.Save_CRFHdrDtlSubDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, DsSave, False, Me.Session(S_UserID), estr) Then
    '        '    Me.objCommon.ShowAlert(estr, Me.Page)
    '        '    Exit Sub
    '        'Else

    '        objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection) 'Me.PlaceMedEx.Controls 
    '        For Each objControl In objCollection
    '            If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.FileUpload" Then
    '                Dim filename As String = ""

    '                If objControl.ID.ToString.Contains("FU") Then
    '                    ObjId = objControl.ID.ToString.Replace("FU", "")
    '                Else
    '                    ObjId = objControl.ID.ToString.Trim()
    '                End If
    '                If Request.Files(objControl.ID).FileName = "" And _
    '                     Not IsNothing(CType(FindControl("hlnk" + ObjId), HyperLink)) AndAlso _
    '                     CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl <> "" Then

    '                    filename = Server.MapPath(CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl)

    '                    FolderPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail"))
    '                    FolderPath += WorkspaceId + "/" + NodeId + "/" + SubjectId + "/" + TranNo_Retu + "/"

    '                    Dir = New DirectoryInfo(FolderPath)
    '                    If Not Dir.Exists() Then
    '                        Dir.Create()
    '                    End If

    '                    Flinfo = New FileInfo(filename.Trim())
    '                    Flinfo.CopyTo(FolderPath + Flinfo.Name)

    '                ElseIf Request.Files(objControl.ID).FileName <> "" Then
    '                    FolderPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail"))
    '                    FolderPath += WorkspaceId + "/" + NodeId + "/" + SubjectId + "/" + TranNo_Retu + "/"

    '                    Dir = New DirectoryInfo(FolderPath)
    '                    If Not Dir.Exists() Then
    '                        Dir.Create()
    '                    End If

    '                    filename = FolderPath + Path.GetFileName(Request.Files(objControl.ID).FileName.ToString.Trim())
    '                    Request.Files(objControl.ID).SaveAs(filename)

    '                End If

    '            End If
    '        Next objControl

    '        ' End If
    '        'Check data status before executing editchecks
    '        Me.EditChecks()

    '        Me.objCommon.ShowAlert("Attributes Saved Successfully.", Me.Page)

    '        Me.Session.Remove("PlaceMedEx")
    '        Me.HFSessionFlg.Value = "1"
    '        Me.ddlActivities_SelectedIndexChanged(sender, e)

    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.ToString, "")
    '    End Try
    'End Sub

    '#Region "Check For Pending Activity"
    '    'Added on 1-3-2012 by Megha shah----
    '    'Private Function CheckPendingActivity() As Boolean
    '    '    Dim eStr As String = String.Empty
    '    '    Dim Param As String = String.Empty
    '    '    Dim Ds_Structure As DataSet = Nothing
    '    '    Dim SeqNo As Integer
    '    '    Dim Str As String = ""
    '    '    Dim count As Integer = 0
    '    '    Dim PendingNode As String = String.Empty

    '    '    Param = Me.HFWorkspaceId.Value.Trim() + "##" + Me.HFSubjectId.Value.Trim() + "##"

    '    '    If Not objHelp.Proc_GetStructure(Param, Ds_Structure, eStr) Then
    '    '        Return False
    '    '    End If

    '    '    If Ds_Structure.Tables(0).Rows.Count > 0 Then

    '    '        SeqNo = Ds_Structure.Tables(0).Select(" iNodeId = " & Me.HFNodeId.Value)(0).Item("SeqNo")
    '    '        Ds_Structure.Tables(0).DefaultView.RowFilter = " SeqNo < " & SeqNo.ToString & " And ActivityStatus = '" & CRF_DataEntryPending & "'"
    '    '        If Ds_Structure.Tables(0).DefaultView.ToTable.Rows.Count > 0 Then
    '    '            For Each dr As DataRow In Ds_Structure.Tables(0).DefaultView.ToTable.Rows
    '    '                count += 1
    '    '                If PendingNode.ToString.Trim = "" Then
    '    '                    PendingNode = dr.Item("iNodeId").ToString()
    '    '                Else
    '    '                    PendingNode = PendingNode.ToString() + "," + dr.Item("iNodeId").ToString()
    '    '                End If
    '    '                Str += count.ToString & ". " & dr.Item("vNodeDisplayName").ToString().ToUpper() & "<br/>"
    '    '            Next dr
    '    '        End If
    '    '    End If

    '    '    ' --------added by Megha shah for calling GenCall for those activity which are pending and not having other pending activity
    '    '    If Ds_Structure.Tables(0).DefaultView.ToTable.Rows.Count <= 0 Then
    '    '        If Not GenCall() Then
    '    '            CheckPendingActivity = False
    '    '        End If
    '    '        CheckPendingActivity = True
    '    '        Exit Function
    '    '    End If
    '    '    '-------------------------------------------------------

    '    '    If count > 0 Then
    '    '        Str += " activity/s pending For subject " & Ds_Structure.Tables(0).DefaultView.ToTable().Rows(0)("vMySubjectNo")
    '    '    End If
    '    '    If Not Str.ToString.Trim() = "" Then
    '    '        Me.lblContent.Text = Str.ToString()
    '    '        Me.HFPendingNode.Value = PendingNode.ToString()
    '    '        Me.MPEActivitySequence.Show()
    '    '    End If
    '    '    Return True

    '    'End Function
    '#End Region

    'Protected Sub btnOk_MPEID_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk_MPEID.Click
    '    'This event is created to show the attribute template
    '    ' Here no need to call gencall as the pageload event will automaticaly handel this
    '    Me.HFRemarks.Value = Me.txtContent.Text
    '    Me.MPEActivitySequence.Hide()
    'End Sub

    Protected Sub btnContinueWorking_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinueWorking.Click
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ResetSessionTimer", "btnContinueWorking_Click();", True)
    End Sub
End Class
