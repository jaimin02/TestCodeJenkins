Imports System.Web.Services
Imports Newtonsoft.Json

Partial Class frmSubjectSelectionForVisit
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "

    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()

    Private Const SubjectType As String = "C"

    Private Const VS_DtSubjectMst As String = "dtSubjectMst"
    Private Const VS_WorkspaceSubjectMst As String = "dtWorkspaceSubjectMst"
    Private Const VS_Mode As String = "Mode"
    Private Const VS_RowIndex As String = "RowIndex"
    Private Const VS_WorkspaceSubjectId As String = "WorkspaceSubjectId"
    Private Const Vs_DeletedSubjectData As String = "Vs_DeletedSubjectData"
    Private Const VS_SubjectId As String = "SubjectId"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_SubjectId As Integer = 1
    Private Const GVC_Initials As Integer = 2
    Private Const GVC_WorkspaceSubjectId As Integer = 3
    Private Const GVC_Period As Integer = 4
    Private Const GVC_MySubjectNo As Integer = 5
    Private Const GVC_RandomizationNo As Integer = 6
    Private Const GVC_Attendance As Integer = 7
    Private Const GVC_ICFDate As Integer = 8
    Private Const GVC_RejectionFlag As Integer = 9
    Private Const GVC_Reject As Integer = 10
    Private Const GVC_Edit As Integer = 11
    Private Const GVC_Delete As Integer = 12
    Private Const GVC_KitAllocatin As Integer = 14
    Private Const GVC_ReverseRandomizatin As Integer = 15
    Private Const GVC_Audittrail As Integer = 16
    Private Const GVC_Deleted As Integer = 17
    Private Const GVC_AddDemographic As Integer = 18
    Private Const GVC_cRandomizationType As Integer = 19
    Private Const GVC_cScreenFailure As Integer = 20
    Private Const GVC_cDisContinue As Integer = 21
    Private GVC_ScreenNo As Integer = 6
    Private GVC_PatientRandomizationNoNo As Integer = 7

    Private Const ActId_SubjectRejection As String = "0005"
    Private Const Scope_ClinicalTrial As Integer = 1

    Private Const GVCellAuditTrail_ReportingDateTime As Integer = 7
    Private Const GVCellAuditTrail_ICFDateTime As Integer = 4
    Private Const GVSCreenFailureAudit_ReportingDateTime As Integer = 7

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not GenCall() Then
                Exit Sub
            End If
        End If
    End Sub

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Try


            If Not GenCall_Data() Then ' For Data Retrieval
                Return False
            End If

            If Not GenCall_ShowUI() Then 'For Displaying Data 
                Return False
            End If

            GenCall = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall")
            Return False
        End Try

    End Function

#End Region

#Region "GenCall_Data"

    Private Function GenCall_Data() As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = "1=2"
        Dim ds_WorkSpaceSubjectMst As DataSet = Nothing
        Dim ds_SubjectMst As DataSet = Nothing

        Try

            If Not Me.ViewState(VS_Mode) Is Nothing Then
                If CType(Me.ViewState(VS_Mode), String).ToUpper() = "EDIT" Or CType(Me.ViewState(VS_Mode), String).ToUpper() = "REVERSERANDOMIZATION" Then
                    wStr = "vSubjectId = '" + Me.HSubject.Value.Trim() + "' And vWorkspaceId = '"
                    wStr += Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"
                ElseIf CType(Me.ViewState(VS_Mode), String).ToUpper() = "ADDDEMOGRAPHIC" Then
                    wStr = "vSubjectId = '" + Me.HSubject.Value.Trim() + "' And vWorkspaceId = '"
                    wStr += Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"
                End If
            Else
                Me.ViewState(VS_Mode) = "ADD"
            End If

            If Not objHelp.GetView_SubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                    ds_WorkSpaceSubjectMst, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_WorkSpaceSubjectMst Is Nothing Then
                Throw New Exception(eStr)
            End If

            Me.ViewState(VS_DtSubjectMst) = ds_WorkSpaceSubjectMst.Tables(0)
            ds_WorkSpaceSubjectMst = Nothing
            ds_WorkSpaceSubjectMst = New DataSet

            If Not objHelp.GetWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                ds_SubjectMst, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_SubjectMst Is Nothing Then
                Throw New Exception(eStr)
            End If
            Me.ViewState(VS_WorkspaceSubjectMst) = ds_SubjectMst.Tables(0)

            GenCall_Data = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_Data")
            Return False
        End Try

    End Function

#End Region

#Region "GenCall_ShowUI"

    Private Function GenCall_ShowUI() As Boolean
        Dim eStr As String = String.Empty
        Dim ds_Workspace As New DataSet
        Dim sender As New Object
        Dim e As New EventArgs

        Try
            Page.Title = ":: Subject Enrollment :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Master.FindControl("lblHeading"), Label).Text = "Subject Enrollment"

            Me.BtnSaveSubjectMst.Attributes.Add("OnClick", "return Validation();")
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)

            Me.txtWeight.Attributes.Add("onblur", "FillBMIValue('" & Me.txtHeight.ClientID & "','" & Me.txtWeight.ClientID & "','" & Me.txtBMI.ClientID & "');")
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                lblPeriod.Visible = False
                ddlPeriod.Visible = False
            End If

            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                    btnSetProject_Click(sender, e)
                Else
                    btnSetProject_Click(sender, e)
                End If

            End If

            If Not FillReason() Then
                Throw New Exception("Error While Filling Reasons")
            End If

            GenCall_ShowUI = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
            Return False
        Finally
        End Try
    End Function

#End Region

#Region "FillGrid"

    Private Function FillGrid() As Boolean
        Dim eStr As String = String.Empty
        Dim ds_Subject As New DataSet
        Dim Dv_SubjectDataWithoutDeleted As New DataView
        Dim Dt_SubjectDataWithoutDeleted As New DataTable
        Dim Dt_OnlyDeletedSubjectData As New DataTable
        Dim Dv_OnlyDeletedSubjectData As New DataView
        Dim wStr As String = String.Empty

        Try

            gvwSubjectSelectionForVisit.DataSource = Nothing

            If Me.HProjectId.Value.ToString.Trim() = "" Then
                Return True
            End If
            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"

            If Me.HSubject.Value.Trim() <> "" And ddlPeriod.SelectedValue = 1 Then '' Enabling of filtering Subjects  in grid when period is one
                wStr += " And vSubjectId = '" + Me.HSubject.Value.Trim() + "'"
            End If

            ''Added 13-10-2011
            wStr += " And iPeriod=" + ddlPeriod.SelectedValue
            wStr += " Order by cast(iMySubjectNo as Numeric)"
            If Not objHelp.GetViewWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    ds_Subject, eStr) Then
                Throw New Exception(eStr)
            End If

            ''Getting Deleted Subject Data'
            Dv_OnlyDeletedSubjectData = ds_Subject.Tables(0).Copy().DefaultView()
            Dv_OnlyDeletedSubjectData.RowFilter = "cStatusIndi='D'"
            Dt_OnlyDeletedSubjectData = Dv_OnlyDeletedSubjectData.ToTable()
            Me.ViewState(Vs_DeletedSubjectData) = Dt_OnlyDeletedSubjectData
            If Dt_OnlyDeletedSubjectData.Rows.Count > 0 Then
                BtnDeletedSubjects.Visible = True
            Else
                BtnDeletedSubjects.Visible = False
            End If
            '' Getting Deleted Subject Data Completed ''
            '' ****************************************************************************** ''
            'Getting Data Of Subject Without Deleted ''
            Dv_SubjectDataWithoutDeleted = ds_Subject.Tables(0).Copy().DefaultView()
            Dv_SubjectDataWithoutDeleted.RowFilter = "cStatusIndi<>'D'"
            Dt_SubjectDataWithoutDeleted = Dv_SubjectDataWithoutDeleted.ToTable()
            ''gETTING Data Of Subject Withour Deleted Completed ''
            '' ********************************************************************************* ''
            gvwSubjectSelectionForVisit.DataSource = Dt_SubjectDataWithoutDeleted
            gvwSubjectSelectionForVisit.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetPatient", "fsetPatient_Show(); ", True)
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillGrid")
            Return False
        End Try

    End Function

    Private Function FillAudittrailGrid() As Boolean
        Dim eStr As String = String.Empty
        Dim ds_Subject As New DataSet
        Dim wStr As String = String.Empty
        'Dim dc_AuditTrail As DataColumn

        Try


            gvwSubjectSelectionForVisit.DataSource = Nothing

            If Me.HProjectId.Value.ToString.Trim() = "" Then
                Return True
            End If
            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"

            If Me.HSubject.Value.Trim() <> "" Then
                wStr += " And vSubjectId = '" + Me.HSubject.Value.Trim() + "'"
            End If
            wStr += " And vScreenFailureRemaks = '' "
            If Not objHelp.View_WorkSpaceSubjectMstHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    ds_Subject, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_Subject.Tables(0).Rows.Count = 0 Then
                Return False
            End If
            gvAudittrail.DataSource = ds_Subject.Tables(0)
            gvAudittrail.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvClient", "UIgvClient(); ", True)   ''Added by ketan
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillAudittrailGrid")
            Return False
        End Try

    End Function


    Private Function FillScreenFailureAudittrailGrid() As Boolean
        Dim eStr As String = String.Empty
        Dim ds_Subject As New DataSet
        Dim wStr As String = String.Empty
        'Dim dc_AuditTrail As DataColumn

        Try


            gvScreenFailureAuditTrail.DataSource = Nothing
            upModel.Update()
            If Me.HProjectId.Value.ToString.Trim() = "" Then
                Return True
            End If
            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"


            If Me.ViewState(VS_SubjectId).ToString().Trim() <> "" Then
                wStr += " And vSubjectId = '" + Me.ViewState(VS_SubjectId).ToString().Trim() + "'"
            End If
            wStr += " And vScreenFailureRemaks <> '' "
            If Not objHelp.View_WorkSpaceSubjectMstHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    ds_Subject, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_Subject.Tables(0).Rows.Count = 0 Then
                gvScreenFailureAuditTrail.DataSource = Nothing
                gvScreenFailureAuditTrail.DataBind()
                upModel.Update()
                Return False
            End If

            gvScreenFailureAuditTrail.DataSource = ds_Subject.Tables(0)
            gvScreenFailureAuditTrail.DataBind()
            upModel.Update()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ScreenFailureAudit", "ScreenFailureAudit(); ", True)
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillAudittrailGrid")
            Return False
        End Try

    End Function


    Private Function FillAudittrailGridForDeletedSubjects(ByVal SubjectNo As String) As Boolean
        Dim eStr As String = String.Empty
        Dim ds_Subject As New DataSet
        Dim wStr As String = String.Empty
        'Dim dc_AuditTrail As DataColumn

        Try

            gvwSubjectSelectionForVisit.DataSource = Nothing

            If Me.HProjectId.Value.ToString.Trim() = "" Then
                Return True
            End If
            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"

            If SubjectNo.Trim() <> "" Then
                wStr += " And vMySubjectNo= '" + SubjectNo + "'"
            End If

            If Not objHelp.View_WorkSpaceSubjectMstHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    ds_Subject, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_Subject.Tables(0).Rows.Count = 0 Then
                Return False
            End If
            gvAudittrail.DataSource = ds_Subject.Tables(0)
            gvAudittrail.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvClient", "UIgvClient(); ", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "DeleteSubjectAuditTrail_Datatable", "DeleteSubjectAuditTrail_Datatable(); ", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "AuditTrailSubjectWise", "AuditTrailSubjectWise(); ", True)
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillAudittrailGridForDeletedSubjects")
            Return False
        End Try

    End Function

#End Region

#Region "Grid Events"

#Region "gvwSubjectSelectionForVisit"

    Protected Sub gvwSubjectSelectionForVisit_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvwSubjectSelectionForVisit.PageIndexChanging
        gvwSubjectSelectionForVisit.PageIndex = e.NewPageIndex
        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub
    Protected Sub gvwSubjectSelectionForVisit_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwSubjectSelectionForVisit.RowCommand
        Dim index As Integer = e.CommandArgument
        Dim dtSubjectMaster As New DataTable
        Dim dtWorkspaceSubjectMst As New DataTable


        If e.CommandName.ToUpper = "ASSIGNMENT" Then

            If Not SubjectAssignment(index) Then
                Exit Sub
            End If

        ElseIf e.CommandName.ToUpper = "REJECT" Then

            Me.ViewState(VS_Mode) = "REJECT"
            Me.ViewState(VS_RowIndex) = index.ToString()
            Me.MpeRejectionOrDeletion.Show()

        ElseIf e.CommandName.ToUpper = "DELETE" Then

            Me.ViewState(VS_Mode) = "DELETE"
            Me.ViewState(VS_RowIndex) = index.ToString()
            Me.MpeRejectionOrDeletion.Show()


        ElseIf e.CommandName.ToUpper = "EDIT" Then

            Me.trRemarks.Style.Add("display", "''")
            Me.trPatientRandomizationNo.Style.Add("display", "''")


            Me.ViewState(VS_Mode) = "EDIT"
            Me.ViewState(VS_RowIndex) = index.ToString()

            Me.HSubject.Value = Convert.ToString(Me.gvwSubjectSelectionForVisit.Rows(index).Cells(GVC_SubjectId).Text).Trim()

            Me.hdnRandomizationType.Value = Convert.ToString(Me.gvwSubjectSelectionForVisit.Rows(index).Cells(GVC_cRandomizationType).Text).Trim()
            Me.hdncScreenFailure.Value = Convert.ToString(Me.gvwSubjectSelectionForVisit.Rows(index).Cells(GVC_cScreenFailure).Text).Trim()
            Me.hdncDisContinue.Value = Convert.ToString(Me.gvwSubjectSelectionForVisit.Rows(index).Cells(GVC_cDisContinue).Text).Trim()

            If Me.hdnRandomizationType.Value = "M" Then
                Me.txtPatientRandomizationNo.Enabled = True
            ElseIf Me.hdnRandomizationType.Value = "I" Then
                Me.txtPatientRandomizationNo.Enabled = False
            End If

            If Not Me.GenCall_Data() Then
                Exit Sub
            End If

            dtSubjectMaster = CType(Me.ViewState(VS_DtSubjectMst), DataTable)
            If dtSubjectMaster.Rows.Count > 0 Then

                Me.txtFirstName.Text = Convert.ToString(dtSubjectMaster.Rows(0)("vFirstName")).Trim().ToUpper()
                Me.txtMiddleName.Text = Convert.ToString(dtSubjectMaster.Rows(0)("vMiddleName")).Trim().ToUpper()
                Me.txtLastName.Text = Convert.ToString(dtSubjectMaster.Rows(0)("vSurName")).Trim().ToUpper()
                Me.txtInitial.Text = Convert.ToString(dtSubjectMaster.Rows(0)("vInitials")).Trim().ToUpper()
                'Me.txtICFSignedDate.Text = Convert.ToString(dtSubjectMaster.Rows(0)("dAllocationDate")).Trim()

            End If

            dtWorkspaceSubjectMst = CType(Me.ViewState(VS_WorkspaceSubjectMst), DataTable)
            If dtWorkspaceSubjectMst.Rows.Count > 0 Then

                Me.ViewState(VS_WorkspaceSubjectId) = Convert.ToString(dtWorkspaceSubjectMst.Rows(0)("vWorkspaceSubjectId")).Trim()
                Me.txtScreenNo.Text = Convert.ToString(dtWorkspaceSubjectMst.Rows(0)("vMySubjectNo")).Trim()
                If Convert.ToString(dtWorkspaceSubjectMst.Rows(0)("dICFDate")).Trim() <> "" Then
                    Me.txtICFSignedDate.Text = CType(Convert.ToString(dtWorkspaceSubjectMst.Rows(0)("dICFDate")).Trim(), Date).ToString("dd-MMM-yyyy")
                End If
                Me.txtPatientRandomizationNo.Text = Convert.ToString(dtWorkspaceSubjectMst.Rows(0)("vRandomizationNo")).Trim()
                Me.HReplaceImySubjectNo.Value = Convert.ToString(dtWorkspaceSubjectMst.Rows(0)("iMySubjectNo")).Trim()
                '===========================

            End If

            Me.BtnSaveSubjectMst.Attributes.Add("OnClick", "return ValidationForEdit();")
            Me.MpeSubjectMst.Show()

        ElseIf e.CommandName.ToUpper = "AUDITTRAIL" Then

            Me.HSubject.Value = Convert.ToString(Me.gvwSubjectSelectionForVisit.Rows(index).Cells(GVC_SubjectId).Text).Trim()
            If Not FillAudittrailGrid() Then
                Me.objcommon.ShowAlert("There Is No Any Audit Trail Exist For Selected Subject.", Me.Page)
                Exit Sub
            End If
            Me.MpeAudittrail.Show()
            Me.HSubject.Value = ""
        ElseIf e.CommandName.ToUpper = "ADDDEMOGRAPHIC" Then

            Me.ViewState(VS_Mode) = "ADDDEMOGRAPHIC"
            Me.ViewState(VS_RowIndex) = index.ToString()

            Me.HSubject.Value = Convert.ToString(Me.gvwSubjectSelectionForVisit.Rows(index).Cells(GVC_SubjectId).Text).Trim()

            If Not Me.GenCall_Data() Then
                Exit Sub
            End If
            dtSubjectMaster = CType(Me.ViewState(VS_DtSubjectMst), DataTable)

            Me.txtHeight.Text = ""
            Me.txtWeight.Text = ""
            Me.txtAGE.Text = ""
            Me.txtDOB.Text = ""
            Me.txtBMI.Text = ""
            Me.rblSex.Items.Item(0).Selected = False
            Me.rblSex.Items.Item(1).Selected = False

            If dtSubjectMaster.Rows.Count > 0 Then

                Me.txtAGE.Text = IIf(Convert.ToString(dtSubjectMaster.Rows(0)("Age")) = "0.00", "", Convert.ToString(dtSubjectMaster.Rows(0)("Age")).Trim())
                Me.txtDOB.Text = Convert.ToString(dtSubjectMaster.Rows(0)("dBirthDate")).Trim()
                Me.txtHeight.Text = IIf(Convert.ToString(dtSubjectMaster.Rows(0)("nHeight")) = "0.00", "", Convert.ToString(dtSubjectMaster.Rows(0)("nHeight")).Trim())
                Me.txtWeight.Text = IIf(Convert.ToString(dtSubjectMaster.Rows(0)("nWeight")) = "0.00", "", Convert.ToString(dtSubjectMaster.Rows(0)("nWeight")).Trim())
                Me.txtBMI.Text = IIf(Convert.ToString(dtSubjectMaster.Rows(0)("nBMI")) = "0.00", "", Convert.ToString(dtSubjectMaster.Rows(0)("nBMI")).Trim())

                If Convert.ToChar(dtSubjectMaster.Rows(0)("cSEx")) = "M" Then
                    Me.rblSex.Items.Item(0).Selected = True
                ElseIf Convert.ToChar(dtSubjectMaster.Rows(0)("cSEx")) = "F" Then
                    Me.rblSex.Items.Item(1).Selected = True
                Else
                    Me.rblSex.Items.Item(0).Selected = False
                    Me.rblSex.Items.Item(1).Selected = False
                End If
            End If

            Me.mpeDemographic.Show()
            Me.txtHeight.Focus()

        ElseIf e.CommandName.ToUpper = "SCREENFAILURE" Then
            Me.HSubject.Value = Convert.ToString(Me.gvwSubjectSelectionForVisit.Rows(index).Cells(GVC_SubjectId).Text).Trim()
            Me.ViewState(VS_SubjectId) = Convert.ToString(Me.gvwSubjectSelectionForVisit.Rows(index).Cells(GVC_SubjectId).Text).Trim()
            Me.ViewState(VS_WorkspaceSubjectId) = Convert.ToString(Me.gvwSubjectSelectionForVisit.Rows(index).Cells(GVC_WorkspaceSubjectId).Text).Trim()
            If Not FillScreenFailureAudittrailGrid() Then
            End If
            rbtScreen.ClearSelection()
            txtScreenFailureDate.Text = ""
            txtScreenFailureRemarks.Text = ""
            If Not FillDropDownListVisits() Then
            End If
            ModalScreenFailure.Show()
        ElseIf e.CommandName.ToUpper = "REVERSERANDOMIZATION" Then
            Me.trRemarks.Style.Add("display", "''")
            Me.trPatientRandomizationNo.Style.Add("display", "''")


            Me.ViewState(VS_Mode) = "REVERSERANDOMIZATION"
            Me.ViewState(VS_RowIndex) = index.ToString()

            Me.HSubject.Value = Convert.ToString(Me.gvwSubjectSelectionForVisit.Rows(index).Cells(GVC_SubjectId).Text).Trim()

            Me.hdnRandomizationType.Value = Convert.ToString(Me.gvwSubjectSelectionForVisit.Rows(index).Cells(GVC_cRandomizationType).Text).Trim()
            Me.hdncScreenFailure.Value = Convert.ToString(Me.gvwSubjectSelectionForVisit.Rows(index).Cells(GVC_cScreenFailure).Text).Trim()
            Me.hdncDisContinue.Value = Convert.ToString(Me.gvwSubjectSelectionForVisit.Rows(index).Cells(GVC_cDisContinue).Text).Trim()

            'HReplaceImySubjectNo.Value = dsSubjectNo.Tables(0).Rows(0)("iMySubjectNo")

            If Not Me.GenCall_Data() Then
                Exit Sub
            End If

            dtSubjectMaster = CType(Me.ViewState(VS_DtSubjectMst), DataTable)
            If dtSubjectMaster.Rows.Count > 0 Then
                Me.ViewState(VS_WorkspaceSubjectId) = Convert.ToString(Me.gvwSubjectSelectionForVisit.Rows(index).Cells(3).Text).Trim()
                Me.txtFirstName.Text = Convert.ToString(dtSubjectMaster.Rows(0)("vFirstName")).Trim().ToUpper()
                Me.txtMiddleName.Text = Convert.ToString(dtSubjectMaster.Rows(0)("vMiddleName")).Trim().ToUpper()
                Me.txtLastName.Text = Convert.ToString(dtSubjectMaster.Rows(0)("vSurName")).Trim().ToUpper()
                Me.txtInitial.Text = Convert.ToString(dtSubjectMaster.Rows(0)("vInitials")).Trim().ToUpper()
            End If

            dtWorkspaceSubjectMst = CType(Me.ViewState(VS_WorkspaceSubjectMst), DataTable)
            If dtWorkspaceSubjectMst.Rows.Count > 0 Then

                Me.ViewState(VS_WorkspaceSubjectId) = Convert.ToString(dtWorkspaceSubjectMst.Rows(0)("vWorkspaceSubjectId")).Trim()
                Me.txtScreenNo.Text = Convert.ToString(dtWorkspaceSubjectMst.Rows(0)("vMySubjectNo")).Trim()
                If Convert.ToString(dtWorkspaceSubjectMst.Rows(0)("dICFDate")).Trim() <> "" Then
                    Me.txtICFSignedDate.Text = CType(Convert.ToString(dtWorkspaceSubjectMst.Rows(0)("dICFDate")).Trim(), Date).ToString("dd-MMM-yyyy")
                End If
                Me.txtPatientRandomizationNo.Text = Convert.ToString(dtWorkspaceSubjectMst.Rows(0)("vRandomizationNo")).Trim()
                Me.HReplaceImySubjectNo.Value = Convert.ToString(dtWorkspaceSubjectMst.Rows(0)("iMySubjectNo")).Trim()
                '===========================

            End If

        End If

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetPatient", "fsetPatient_Show(); ", True)

    End Sub

    Protected Sub gvwSubjectSelectionForVisit_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwSubjectSelectionForVisit.RowCreated

        If e.Row.RowType = DataControlRowType.Header Or _
                e.Row.RowType = DataControlRowType.DataRow Or _
                e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVC_SubjectId).Visible = False
            e.Row.Cells(GVC_Period).Visible = False
            e.Row.Cells(GVC_Attendance).Visible = False
            e.Row.Cells(GVC_WorkspaceSubjectId).Visible = False
            e.Row.Cells(GVC_Reject).Visible = False
            e.Row.Cells(GVC_Delete).Visible = False
            e.Row.Cells(GVC_RejectionFlag).Visible = False
            e.Row.Cells(GVC_Edit).Visible = False
            e.Row.Cells(GVC_Edit).Visible = False

            e.Row.Cells(GVC_Deleted).Visible = False
            ''''
            e.Row.Cells(GVC_AddDemographic).Visible = True
            e.Row.Cells(GVC_cRandomizationType).Visible = False
            e.Row.Cells(GVC_cScreenFailure).Visible = False
            e.Row.Cells(GVC_cDisContinue).Visible = False

            If (((CType(Me.Session(S_WorkFlowStageId), String) = WorkFlowStageId_FinalReviewAndLock) Or (CType(Me.Session(S_WorkFlowStageId), String) = WorkFlowStageId_DataEntry)) And _
                CType(Me.Session(S_ScopeNo), String) = Scope_ClinicalTrial) Or _
               (CType(Me.Session(S_WorkFlowStageId), String) = WorkFlowStageId_DataEntry And _
                CType(Me.Session(S_ScopeNo), String) <> Scope_ClinicalTrial) Then 'Added By Mrunal On 20-Dec-2011 For giving edit rights whose workflowstageid = 0 and scope not equle to CTM
                e.Row.Cells(GVC_Reject).Visible = True
                e.Row.Cells(GVC_Delete).Visible = True
                e.Row.Cells(GVC_Edit).Visible = True
                e.Row.Cells(GVC_Deleted).Visible = False
            End If

        End If

    End Sub

    Protected Sub gvwSubjectSelectionForVisit_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwSubjectSelectionForVisit.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            TrLegend.Visible = True

            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (gvwSubjectSelectionForVisit.PageSize * gvwSubjectSelectionForVisit.PageIndex) + 1

            CType(e.Row.FindControl("imgBtnAssignment"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("imgBtnAssignment"), ImageButton).CommandName = "ASSIGNMENT"

            If e.Row.Cells(GVC_RandomizationNo).Text.Trim() = "&nbsp;" Then
                e.Row.Cells(GVC_RandomizationNo).Text = ""
            End If

            CType(e.Row.FindControl("txtRandomizationNo"), TextBox).Text = e.Row.Cells(GVC_RandomizationNo).Text.Trim()

            If CType(e.Row.FindControl("txtRandomizationNo"), TextBox).Text.Trim() <> "" Then
                CType(e.Row.FindControl("txtRandomizationNo"), TextBox).Enabled = False
                CType(e.Row.FindControl("imgBtnAssignment"), ImageButton).Visible = False
            End If

            If (((CType(Me.Session(S_WorkFlowStageId), String) = WorkFlowStageId_FinalReviewAndLock) Or (CType(Me.Session(S_WorkFlowStageId), String) = WorkFlowStageId_DataEntry)) And _
                            CType(Me.Session(S_ScopeNo), String) = Scope_ClinicalTrial) Or _
                           (CType(Me.Session(S_WorkFlowStageId), String) = WorkFlowStageId_DataEntry And _
                            CType(Me.Session(S_ScopeNo), String) <> Scope_ClinicalTrial) Then  'Added By Mrunal On 20-Dec-2011 For giving edit rights whose workflowstageid = 0 and scope not equle to CTM

                CType(e.Row.FindControl("lnkbtnReject"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkbtnReject"), ImageButton).CommandName = "REJECT"

                CType(e.Row.FindControl("imgbtnDelete"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("imgbtnDelete"), ImageButton).CommandName = "DELETE"

                CType(e.Row.FindControl("imgbtnEdit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("imgbtnEdit"), ImageButton).CommandName = "EDIT"

                CType(e.Row.FindControl("imgScreenFailure"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("imgScreenFailure"), ImageButton).CommandName = "ScreenFailure"

                CType(e.Row.FindControl("imgbtnAddDemographic"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("imgbtnAddDemographic"), ImageButton).CommandName = "ADDDEMOGRAPHIC"

                CType(e.Row.FindControl("imgKitAllocation"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("imgKitAllocation"), ImageButton).CommandName = "KITALLOCATION"

                CType(e.Row.FindControl("imgReverseRandomization"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("imgReverseRandomization"), ImageButton).CommandName = "REVERSERANDOMIZATION"

            ElseIf CType(Me.Session(S_WorkFlowStageId), String) = WorkFlowStageId_DataEntry Then

                CType(e.Row.FindControl("imgbtnAddDemographic"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("imgbtnAddDemographic"), ImageButton).CommandName = "ADDDEMOGRAPHIC"

            End If

            ''for making row red after
            If e.Row.Cells(GVC_RejectionFlag).Text.Trim.ToUpper() = "Y" Then
                'e.Row.Enabled = False
                e.Row.Cells(GVC_SrNo).Enabled = False
                e.Row.Cells(GVC_Initials).Enabled = False
                e.Row.Cells(GVC_ScreenNo).Enabled = False
                e.Row.Cells(GVC_PatientRandomizationNoNo).Enabled = False
                e.Row.Cells(GVC_ICFDate).Enabled = False
                e.Row.Cells(GVC_Delete).Enabled = False
                e.Row.Cells(GVC_Edit).Enabled = False
                e.Row.Cells(GVC_Reject).Enabled = False
                CType(e.Row.FindControl("imgbtnAudittrail"), ImageButton).Enabled = True

                e.Row.Cells(GVC_AddDemographic).Enabled = False
                e.Row.BackColor = Drawing.Color.Red

            End If
            If e.Row.Cells(GVC_Deleted).Text.Trim.ToUpper() = "D" Then
                e.Row.Cells(GVC_SrNo).Enabled = False
                e.Row.Cells(GVC_Initials).Enabled = False
                e.Row.Cells(GVC_ScreenNo).Enabled = False
                e.Row.Cells(GVC_PatientRandomizationNoNo).Enabled = False
                e.Row.Cells(GVC_ICFDate).Enabled = False
                e.Row.Cells(GVC_Delete).Enabled = False
                e.Row.Cells(GVC_Edit).Enabled = False
                e.Row.Cells(GVC_Reject).Enabled = False
                CType(e.Row.FindControl("imgbtnAudittrail"), ImageButton).Enabled = True
                e.Row.BackColor = Drawing.Color.Gray
            End If

            CType(e.Row.FindControl("imgbtnAudittrail"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("imgbtnAudittrail"), ImageButton).CommandName = "AUDITTRAIL"

            If e.Row.Cells(GVC_cScreenFailure).Text.Trim.ToUpper() = "Y" Or e.Row.Cells(GVC_cDisContinue).Text.Trim.ToUpper() = "Y" Then
                e.Row.BackColor = System.Drawing.Color.Cyan
            End If



        End If

    End Sub

#End Region

#Region "gvAudittrail"

    Protected Sub gvAudittrail_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAudittrail.PageIndexChanging

    End Sub

    Protected Sub gvAudittrail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAudittrail.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                If Not Convert.ToString(e.Row.Cells(GVSCreenFailureAudit_ReportingDateTime).Text).Trim = "" Then
                    If Convert.ToString(Replace(e.Row.Cells(GVSCreenFailureAudit_ReportingDateTime).Text.Trim(), "&nbsp;", "")) = "" Then
                        e.Row.Cells(GVSCreenFailureAudit_ReportingDateTime).Text = ""
                    Else
                        e.Row.Cells(GVSCreenFailureAudit_ReportingDateTime).Text = e.Row.Cells(GVSCreenFailureAudit_ReportingDateTime).Text.Trim()
                    End If
                    If Not Convert.ToString(e.Row.Cells(GVCellAuditTrail_ICFDateTime).Text).Trim = "" Then
                        If Convert.ToString(Replace(e.Row.Cells(GVCellAuditTrail_ICFDateTime).Text.Trim(), "&nbsp;", "")) = "" Then
                            e.Row.Cells(GVCellAuditTrail_ICFDateTime).Text = ""
                        Else
                            e.Row.Cells(GVCellAuditTrail_ICFDateTime).Text = CType(Replace(e.Row.Cells(GVCellAuditTrail_ICFDateTime).Text.Trim(), "&nbsp;", ""), Date).ToString("dd-MMM-yyyy").Trim()
                        End If
                    End If

                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...gvAudittrail_RowDataBound")
        End Try
    End Sub


    Protected Sub gvScreenFailureAuditTrail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvScreenFailureAuditTrail.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                If Not Convert.ToString(e.Row.Cells(GVCellAuditTrail_ReportingDateTime).Text).Trim = "" Then
                    If Convert.ToString(Replace(e.Row.Cells(GVCellAuditTrail_ReportingDateTime).Text.Trim(), "&nbsp;", "")) = "" Then
                        e.Row.Cells(GVCellAuditTrail_ReportingDateTime).Text = ""
                    Else
                        e.Row.Cells(GVCellAuditTrail_ReportingDateTime).Text = CType(Replace(e.Row.Cells(GVCellAuditTrail_ReportingDateTime).Text.Trim(), "&nbsp;", ""), Date).ToString("dd-MMM-yyyy HH:mm").Trim() + strServerOffset
                    End If

                End If

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...gvAudittrail_RowDataBound")
        End Try
    End Sub


#End Region

#Region "GvwDeletedSubjectAuditTrail"
    Protected Sub GvwDeletedSubjectAuditTrail_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvwDeletedSubjectAuditTrail.PageIndexChanging
        gvwSubjectSelectionForVisit.PageIndex = e.NewPageIndex
        If Not FillDeletedSubjectGrid() Then
            Exit Sub
        End If
        MpeDeletedSubjectPopUp.Show()
    End Sub

    Protected Sub GvwDeletedSubjectAuditTrail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GvwDeletedSubjectAuditTrail.RowCommand
        Dim Dt_DeletedSubjectsData As New DataTable
        If e.CommandName.ToUpper = "DETAILSOFDELETEDSUBJECTS" Then
            Dt_DeletedSubjectsData = CType(Me.ViewState(Vs_DeletedSubjectData), DataTable)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetPatient", "fsetPatient_Show(); ", True)

            If Not FillAudittrailGridForDeletedSubjects(Me.GvwDeletedSubjectAuditTrail.Rows(e.CommandArgument).Cells(2).Text.ToString()) Then
                Me.objcommon.ShowAlert("", Me.Page)
                Exit Sub

            End If
            MpeAudittrail.Show()

        End If
    End Sub

    Protected Sub GvwDeletedSubjectAuditTrail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvwDeletedSubjectAuditTrail.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("imgbtnAudittrailForDeletedSubjects"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("imgbtnAudittrailForDeletedSubjects"), ImageButton).CommandName = "DetailsOfDeletedSubjects"

            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (gvwSubjectSelectionForVisit.PageSize * gvwSubjectSelectionForVisit.PageIndex) + 1

        End If
    End Sub

#End Region

#End Region

#Region "Button Events"

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_Check As New DataSet
        Dim dv_Check As New DataView
        Try
            Me.HSubject.Value = ""

            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"

            Me.AutoCompleteExtenderSubject.ContextKey = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'"

            If Not Me.objHelp.GetCRFLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_Check, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_Check Is Nothing Then

                dv_Check = ds_Check.Tables(0).DefaultView
                dv_Check.Sort = "iTranNo desc"

                If dv_Check.ToTable().Rows.Count > 0 Then

                    If Convert.ToString(dv_Check.ToTable().Rows(0)("cLockFlag")).Trim.ToUpper() = "L" Then
                        Me.objcommon.ShowAlert("Site is Locked.", Me.Page)
                        Me.txtproject.Text = ""
                        Me.HProjectId.Value = ""
                        Exit Sub
                    End If
                End If

            End If
            FillPeriodDropDown()
            If Not FillGrid() Then
                Exit Sub
            End If
            SetLocation()

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Getting CRF Lock Details ", ex.Message)
        End Try

    End Sub

    Private Sub SetLocation()
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds As New DataSet

        Try
            wStr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"

            If Not Me.objHelp.getworkspacemst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                ds, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds Is Nothing Then

                If ds.Tables(0).Rows.Count > 0 Then
                    Me.HLocationCode.Value = ds.Tables(0).Rows(0).Item("vLocationCode").ToString()
                End If

            End If

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Getting Location Details ", ex.Message)
        End Try
    End Sub

    Protected Sub btnSetSubject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetSubject.Click
        If Not Me.FillGrid() Then
            Exit Sub
        End If
    End Sub

    Protected Sub BtnSaveSubjectMst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSaveSubjectMst.Click
        Dim Ds_Subjectmst As New DataSet
        Dim eStr As String = String.Empty
        Dim SubjectId As String = String.Empty
        Dim mode = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
        Dim Msg As String = String.Empty

        Try

            If CType(Me.ViewState(VS_Mode), String).ToUpper() = "REVERSERANDOMIZATION" Then
                mode = 4
            End If

            If Not AssignValues() Then
                Me.objcommon.ShowAlert("Error While Assigning Data", Me.Page)
                Exit Sub
            End If

            Dim wStr As String = String.Empty
            Dim Ds_Check As New DataSet
            If CType(Me.ViewState(VS_Mode), String).ToUpper() = "ADD" Then

                wStr = "vInitials = '" + Me.txtInitial.Text.Trim() + "'"
                wStr += " And vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"

            ElseIf CType(Me.ViewState(VS_Mode), String).ToUpper() = "EDIT" Then

                wStr = "vInitials = '" + Me.txtInitial.Text.Trim() + "'"
                wStr += " And vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
                wStr += " And vWorkspaceSubjectId <> '" + CType(Me.ViewState(VS_WorkspaceSubjectId), String).Trim() + "'"

            End If

            If Not Me.objHelp.GetWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                        Ds_Check, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not Ds_Check.Tables(0) Is Nothing Then
                If Ds_Check.Tables(0).Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Show", "ShowConfirmation();", True)
                Else
                    Btnhidden_Click(sender, e)
                End If
            End If


        Catch ex As Exception
            ShowErrorMessage(ex.Message, "..........BtnSaveSubjectMst_Click")
        End Try

    End Sub

    Protected Sub Btnhidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btnhidden.Click
        Dim Ds_Subjectmst As New DataSet
        Dim eStr As String = String.Empty
        Dim SubjectId As String = String.Empty
        Dim RandomizationNo As String = String.Empty
        Dim Msg As String = String.Empty
        Dim mode = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

        Try
            If CType(Me.ViewState(VS_Mode), String).ToUpper() = "EDIT" Then
                mode = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
            End If


            If CType(Me.ViewState(VS_Mode), String).ToUpper() = "REVERSERANDOMIZATION" Then
                mode = 4
            End If

            CType(Me.ViewState(VS_Mode), String).ToUpper()

            Ds_Subjectmst.Tables.Add(CType(Me.ViewState(VS_DtSubjectMst), Data.DataTable).Copy())
            Ds_Subjectmst.AcceptChanges()

            If Not Enrollment(SubjectId) Then
                Me.objcommon.ShowAlert("Error while Assigning Data", Me.Page)
                Exit Sub
            End If
            '=============Checking for Duplicate Initial END===============================================
            Ds_Subjectmst.Tables.Add(CType(Me.ViewState(VS_WorkspaceSubjectMst), DataTable).Copy())
            Ds_Subjectmst.AcceptChanges()

            If Not Me.objLambda.Save_SubjectMst(mode, Ds_Subjectmst, _
                                            Me.Session(S_UserID), SubjectId, eStr, RandomizationNo) Then
                Throw New Exception(eStr)
            End If


            lblRandomizationno.Text = RandomizationNo
            Msg = "Subject Enrolled Successfully."
            If CType(Me.ViewState(VS_Mode), String).ToUpper() = "EDIT" Then
                Msg = "Subject Details Updated Successfully."
            End If

            If CType(Me.ViewState(VS_Mode), String).ToUpper() = "REVERSERANDOMIZATION" Then
                Msg = "Reverse Randomization Successfully !"
            End If

            Me.objcommon.ShowAlert(Msg, Me.Page)

            Me.ResetPage()

            If Not Me.FillGrid() Then
                Exit Sub
            End If

            Me.MpeSubjectMst.Hide()

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "......Btnhidden_Click")
        End Try

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim mode As String = String.Empty
        Dim index As Integer

        Try

            mode = CType(Me.ViewState(VS_Mode), String)
            index = CType(Me.ViewState(VS_RowIndex), Integer)

            If mode.ToUpper() = "REJECT" Then

                If Not RejectOrDelete("REJECT", index) Then
                    Throw New Exception("Error While Rejecting Subject")
                End If
                Me.objcommon.ShowAlert("Subject Rejected Successfully", Me.Page)
                TrLegend.Visible = True

            ElseIf mode.ToUpper() = "DELETE" Then

                If Not RejectOrDelete("DELETE", index) Then
                    Throw New Exception("Error While Deleting Subject")
                End If
                Me.objcommon.ShowAlert("Subject Deleted Successfully", Me.Page)

            End If

            If Not Me.FillGrid() Then
                Throw New Exception("Error While Filling Grid")
            End If
            Me.TxtReason.Text = ""
            Me.ddlReason.SelectedIndex = 0
            Me.MpeRejectionOrDeletion.Hide()
            '.update()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetPatient", "fsetPatient_Show(); ", True)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetPatient", "fsetPatient_Show(); ", True)
            Me.ShowErrorMessage(ex.Message, ".....btnSave_Click")
        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            If Me.txtproject.Text.Length <= 0 Then
                objcommon.ShowAlert("Please Select the Project.", Me.Page())
                Exit Sub
            End If




            Me.ViewState(VS_Mode) = "ADD"

            If Not Me.GenCall_Data() Then
                Exit Sub
            End If

            If btnAdd.Text = "Add New Subject" Then ''now This button will work as subject registration & subject attendance
                Me.ResetPage()
                Me.MpeSubjectMst.Show()
            Else  ''(When drop down is selected other then 1 )Now this button will work for making attendance of available subjects
                SubjectAttendance()
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetPatient", "fsetPatient_Show(); ", True)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...........btnAdd_Click")
        End Try
    End Sub

    Protected Sub btnScreenSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnScreenSave.Click
        Try
            Dim wstr As String
            Dim estr As String
            Dim Ds_WorkSpaceSubvjectmst As New DataSet
            Dim ddlValue As String
            Dim ddlText As String
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetPatient", "fsetPatient_Show(); ", True)

            wstr = " vWorkspaceSubjectId = '" + CType(Me.ViewState(VS_WorkspaceSubjectId), String).Trim() + "'"

            If Not Me.objHelp.GetWorkspaceSubjectMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                        Ds_WorkSpaceSubvjectmst, estr) Then
                Throw New Exception(estr)
            End If

            ddlValue = Me.ddlVisits.SelectedItem.Value
            ddlText = Me.ddlVisits.SelectedItem.Text
            For Each dr In Ds_WorkSpaceSubvjectmst.Tables(0).Rows

                If rbtScreen.SelectedIndex = 0 Then
                    dr("cScreenFailure") = "Y"
                    dr("cDisContinue") = "N"
                ElseIf rbtScreen.SelectedIndex = 1 Then
                    dr("cScreenFailure") = "N"
                    dr("cDisContinue") = "Y"
                Else
                    dr("cScreenFailure") = "N"
                    dr("cDisContinue") = "N"
                End If
                dr("vScreenFailureRemaks") = IIf(Me.txtScreenFailureRemarks.Text.Trim() <> "", Me.txtScreenFailureRemarks.Text.Trim().ToUpper(), "")
                dr("dScreenFailureDate") = IIf(Me.txtScreenFailureDate.Text.Trim() <> "", Me.txtScreenFailureDate.Text.Trim().ToUpper(), DBNull.Value)
                dr("vActivityId") = ddlValue.Split("#")(0)
                dr("iNodeId") = ddlValue.Split("#")(1)
                dr("vNodeDisplayName") = ddlText.ToString().Trim()
                Ds_WorkSpaceSubvjectmst.AcceptChanges()
            Next

            If Not objLambda.Save_WorkSpaceSubjectMaster(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, Ds_WorkSpaceSubvjectmst, Session(S_UserID), estr) Then
                Throw New Exception(estr)
            End If

            FillGrid()

            rbtScreen.ClearSelection()
            Me.txtScreenFailureRemarks.Text = ""
            Me.txtScreenFailureDate.Text = ""
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetPatient", "fsetPatient_Show(); ", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...........btnScreenSave_Click")
        End Try
    End Sub

    Protected Sub btnSaveRandomizationNoSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveRandomizationNoSave.Click
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Show", "validationforRandomizationNO('" + Convert.ToString(Me.Session(S_FirstName)) + "');", True)
    End Sub

    Protected Sub btnScreenCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnScreenCancel.Click
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetPatient", "fsetPatient_Show(); ", True)
    End Sub

    Protected Sub btnSignAuthOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSignAuthOK.Click
        Dim Ds_Subjectmst As New DataSet
        Dim eStr As String = String.Empty
        Dim SubjectId As String = String.Empty
        Dim mode = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
        Dim Msg As String = String.Empty
        Dim RandomizationNo As String = String.Empty
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetPatient", "fsetPatient_Show(); ", True)
        Try
            If Auntheticate(Me.txtPassword.Text) Then
                If CType(Me.ViewState(VS_Mode), String).ToUpper() = "EDIT" Then
                    mode = 5
                End If

                If Not AssignValues() Then
                    Me.objcommon.ShowAlert("Error While Assigning Data", Me.Page)
                    Exit Sub
                End If

                Dim wStr As String = String.Empty
                Dim Ds_Check As New DataSet
                If CType(Me.ViewState(VS_Mode), String).ToUpper() = "ADD" Then

                    wStr = "vInitials = '" + Me.txtInitial.Text.Trim() + "'"
                    wStr += " And vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"

                ElseIf CType(Me.ViewState(VS_Mode), String).ToUpper() = "EDIT" Then

                    wStr = "vInitials = '" + Me.txtInitial.Text.Trim() + "'"
                    wStr += " And vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
                    wStr += " And vWorkspaceSubjectId <> '" + CType(Me.ViewState(VS_WorkspaceSubjectId), String).Trim() + "'"

                End If

                If Not Me.objHelp.GetWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                            Ds_Check, eStr) Then
                    Throw New Exception(eStr)
                End If

                If Not Ds_Check.Tables(0) Is Nothing Then
                    If Ds_Check.Tables(0).Rows.Count > 0 Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Show", "ShowConfirmation();", True)
                    Else
                        'Btnhidden_Click(sender, e)
                        Ds_Subjectmst.Tables.Add(CType(Me.ViewState(VS_DtSubjectMst), Data.DataTable).Copy())
                        Ds_Subjectmst.AcceptChanges()

                        If Not Enrollment(SubjectId) Then
                            Me.objcommon.ShowAlert("Error while Assigning Data", Me.Page)
                            Exit Sub
                        End If
                        '=============Checking for Duplicate Initial END===============================================
                        Ds_Subjectmst.Tables.Add(CType(Me.ViewState(VS_WorkspaceSubjectMst), DataTable).Copy())
                        Ds_Subjectmst.AcceptChanges()

                        If Not Me.objLambda.Save_SubjectMst(mode, Ds_Subjectmst, _
                                                        Me.Session(S_UserID), SubjectId, eStr, RandomizationNo) Then
                            Throw New Exception(eStr)
                        End If

                        lblRandomizationno.Text = RandomizationNo
                        Me.ResetPage()
                        If Not Me.FillGrid() Then
                            Exit Sub
                        End If

                        Me.MpeSubjectMst.Hide()
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Show", "RandomizationNumberGeneration('" + lblRandomizationno.Text + "');", True)
                    End If
                End If
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "..........BtnSaveSubjectMst_Click")
        End Try

    End Sub

    Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SignatureAuthforKitAllocation", "SignatureAuthforKitAllocation('" + Convert.ToString(Me.Session(S_FirstName)) + "');", True)

    End Sub

    Protected Sub btnOKSignAuth_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOKSignAuth.Click
        If Auntheticate(Me.txtPasswordForKit.Text) Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "KitDataSave", "KitDataSave('" + Me.Session(S_UserID) + "');", True)
        End If


    End Sub

    Protected Sub btnSaveReverseRandomization_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveReverseRandomization.Click
        Dim Ds_Subjectmst As New DataSet
        Dim eStr As String = String.Empty
        Dim SubjectId As String = String.Empty
        Dim mode = 4
        Dim Msg As String = String.Empty

        Try

            If Not AssignValues() Then
                Me.objcommon.ShowAlert("Error While Assigning Data", Me.Page)
                Exit Sub
            End If

            Dim wStr As String = String.Empty
            Dim Ds_Check As New DataSet
            If CType(Me.ViewState(VS_Mode), String).ToUpper() = "ADD" Then

                wStr = "vInitials = '" + Me.txtInitial.Text.Trim() + "'"
                wStr += " And vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"

            ElseIf CType(Me.ViewState(VS_Mode), String).ToUpper() = "EDIT" Or CType(Me.ViewState(VS_Mode), String).ToUpper() = "REVERSERANDOMIZATION" Then

                wStr = "vInitials = '" + Me.txtInitial.Text.Trim() + "'"
                wStr += " And vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
                wStr += " And vWorkspaceSubjectId <> '" + CType(Me.ViewState(VS_WorkspaceSubjectId), String).Trim() + "'"

            End If

            If Not Me.objHelp.GetWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                        Ds_Check, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not Ds_Check.Tables(0) Is Nothing Then
                If Ds_Check.Tables(0).Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Show", "ShowConfirmation();", True)
                Else
                    Btnhidden_Click(sender, e)
                End If
            End If

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SaveReverseRandomization", "SaveReverseRandomization();", True)


        Catch ex As Exception
            ShowErrorMessage(ex.Message, "..........btnSaveReverseRandomization_Click")
        End Try
    End Sub


#End Region

#Region "Subject Assignment"

    Private Function SubjectAssignment(ByVal RowIndex As Integer) As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim Ds_WorkspaceSubjectMst As New DataSet
        Dim dr As DataRow
        Dim Ds_Check As New DataSet
        Dim Dv_Check As New DataView

        Try

            If CType(Me.gvwSubjectSelectionForVisit.Rows(RowIndex).Cells(GVC_Attendance).FindControl("txtRandomizationNo"), TextBox).Text.Trim() = "" Then
                Me.objcommon.ShowAlert("Enter the Randomization No.", Me.Page)
                Exit Function
            End If

            '********Checking for duplicate randomzation No************
            wStr = "vRandomizationNo = '" + CType(Me.gvwSubjectSelectionForVisit.Rows(RowIndex).Cells(GVC_Attendance).FindControl("txtRandomizationNo"), TextBox).Text.Trim() + "'"
            wStr += " And vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"

            If Not Me.objHelp.GetWorkspaceSubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                        Ds_Check, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not Ds_Check.Tables(0) Is Nothing Then
                If Ds_Check.Tables(0).Rows.Count > 0 Then
                    objcommon.ShowAlert("Randomization No. Already Assigned To Another Subject.", Me.Page)
                    Return False
                End If
            End If
            '************************************************

            wStr = "vSubjectId = '" + Me.gvwSubjectSelectionForVisit.Rows(RowIndex).Cells(GVC_SubjectId).Text.Trim() + "'"

            If Not Me.objHelp.GetWorkspaceSubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            Ds_WorkspaceSubjectMst, eStr) Then
                Throw New Exception(eStr)
            End If

            If Ds_WorkspaceSubjectMst.Tables(0).Rows.Count < 1 Then
                objcommon.ShowAlert("First Enroll The Subject And Then Assign Patient/Randomization No.", Me.Page)
                Exit Function
            End If

            For Each dr In Ds_WorkspaceSubjectMst.Tables(0).Rows
                dr("vRandomizationNo") = CType(Me.gvwSubjectSelectionForVisit.Rows(RowIndex).Cells(GVC_Attendance).FindControl("txtRandomizationNo"), TextBox).Text.Trim()
                dr("cStatusIndi") = "E"
                dr("iModifyBy") = Me.Session(S_UserID)
                Ds_WorkspaceSubjectMst.AcceptChanges()
            Next

            Ds_WorkspaceSubjectMst.Tables(0).TableName = "View_WorkspaceSubjectMst"
            Ds_WorkspaceSubjectMst.AcceptChanges()

            If Not objLambda.Save_InsertWorkspaceSubjectMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                        WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_WorkspaceSubjectMst, Ds_WorkspaceSubjectMst, Me.Session(S_UserID), eStr) Then
                objcommon.ShowAlert("Error While Assigning Subject", Me.Page)
                Exit Function
            End If

            objcommon.ShowAlert("Randomization No. Assigned Successfully.", Me)

            ResetPage()

            If Not Me.FillGrid() Then
                Exit Function
            End If

        Catch ex As System.Threading.ThreadAbortException

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......SubjectAssignment")
            Return False
        End Try

    End Function

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

#Region "AssignValues"

    Private Function AssignValues() As Boolean
        Dim dtOld As New DataTable
        Dim dr As DataRow

        Try



            dtOld = Me.ViewState(VS_DtSubjectMst)

            If CType(Me.ViewState(VS_Mode), String).ToUpper() = "ADD" Then

                dtOld.Clear()
                dr = dtOld.NewRow
                dr("vLocationCode") = Me.HLocationCode.Value.Trim()
                dr("vSubjectID") = Pro_Screening
                dr("dEnrollmentDate") = Date.Now().ToString("dd-MMM-yyyy hh:mm")
                dr("vFirstName") = Me.txtFirstName.Text.Trim.ToUpper()
                dr("vSurName") = Me.txtLastName.Text.Trim.ToUpper()
                dr("vMiddleName") = Me.txtMiddleName.Text.Trim.ToUpper()
                dr("vInitials") = Me.txtInitial.Text.Trim.ToUpper()
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cSubjectType") = SubjectType

                'subjectallocationtable
                dr("nSubjectWorkSpaceNo") = 1
                dr("vWorkSpaceId") = Me.HProjectId.Value.Trim()
                dr("dAllocationDate") = ""
                'CDate(Me.txtICFSignedDate.Text.Trim()).ToString("dd-MMM-yyyy")

                dtOld.Rows.Add(dr)
                dtOld.TableName = "View_SubjectMaster"

            ElseIf CType(Me.ViewState(VS_Mode), String).ToUpper() = "EDIT" Then

                For Each dr In dtOld.Rows

                    dr("vLocationCode") = Me.HLocationCode.Value.Trim()
                    'dr("vSubjectID") = Pro_Screening
                    dr("dEnrollmentDate") = Date.Now()
                    dr("vFirstName") = Me.txtFirstName.Text.Trim.ToUpper()
                    dr("vSurName") = Me.txtLastName.Text.Trim.ToUpper()
                    dr("vMiddleName") = Me.txtMiddleName.Text.Trim.ToUpper()
                    dr("vInitials") = Me.txtInitial.Text.Trim.ToUpper()
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cSubjectType") = SubjectType

                    'subjectallocationtable
                    dr("nSubjectWorkSpaceNo") = 1
                    dr("vWorkSpaceId") = Me.HProjectId.Value.Trim()
                    dr("dAllocationDate") = ""
                    'Me.txtICFSignedDate.Text.Trim()
                    dtOld.AcceptChanges()

                Next

                dtOld.TableName = "VIEW_SUBJECTMASTER"
            ElseIf CType(Me.ViewState(VS_Mode), String).ToUpper() = "REVERSERANDOMIZATION" Then
                For Each dr In dtOld.Rows
                    dr("vRemarks") = Me.txtReverseRandomizationRemarks.Text
                    dtOld.AcceptChanges()
                Next
            End If

            dtOld.AcceptChanges()
            Me.ViewState(VS_DtSubjectMst) = dtOld

            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....AssignValues")
            Return False
        End Try
    End Function

    Private Function AssignValueDemographic() As Boolean
        Dim DTsubject As New DataTable
        Dim drsubject As DataRow

        DTsubject = Me.ViewState(VS_DtSubjectMst)
        Try
            If DTsubject.Rows.Count > 0 Then
                For Each drsubject In DTsubject.Rows

                    drsubject("dBirthDate") = Me.txtDOB.Text.Trim().ToUpper()
                    drsubject("Age") = Convert.ToSingle(Me.txtAGE.Text.Trim().ToUpper())
                    drsubject("nHeight") = Convert.ToSingle(Me.txtHeight.Text)
                    drsubject("nWeight") = Convert.ToSingle(Me.txtWeight.Text)
                    drsubject("nBMI") = Me.txtBMI.Text.Trim().ToUpper()
                    drsubject("iModifyBy") = Me.Session(S_UserID)

                    If Me.rblSex.Items.Item(0).Selected Then
                        drsubject("cSEx") = "M"
                    Else
                        drsubject("CSEx") = "F"
                    End If
                    DTsubject.AcceptChanges()
                Next

            Else 'for adding rows to blank data structure
            End If


            Me.ViewState(VS_DtSubjectMst) = DTsubject

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......AssignValueDemographic")
        End Try
    End Function

#End Region

#Region "Enrollment"

    Private Function Enrollment(ByVal SubjectId As String) As Boolean
        Dim estr As String = String.Empty
        Dim dr As DataRow
        Dim Dt_WorkspaceSubjectMst As New DataTable
        Dim dsSubjectNo As New DataSet
        Dim SubjectNo As Integer = 0
        Dim wStr As String = String.Empty
        Dim Ds_Check As New DataSet

        Try

            '********Checking for duplicate Screen No************

            If CType(Me.ViewState(VS_Mode), String).ToUpper() = "ADD" Then

                wStr = "vMySubjectNo = '" + Me.txtScreenNo.Text.Trim() + "'"
                wStr += " And vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"

            ElseIf CType(Me.ViewState(VS_Mode), String).ToUpper() = "EDIT" Or CType(Me.ViewState(VS_Mode), String).ToUpper() = "REVERSERANDOMIZATION" Then

                wStr = "vMySubjectNo = '" + Me.txtScreenNo.Text.Trim() + "'"
                wStr += " And vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
                wStr += " And vWorkspaceSubjectId <> '" + CType(Me.ViewState(VS_WorkspaceSubjectId), String).Trim() + "'"

            End If

            If Not Me.objHelp.GetWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                        Ds_Check, estr) Then
                Throw New Exception(estr)
            End If

            If Not Ds_Check.Tables(0) Is Nothing Then
                If Ds_Check.Tables(0).Rows.Count > 0 Then
                    Dim Dv_DuplicateSubjectNoWithDeleted As New DataView
                    Dim Dv_DuplicateRejectedNo As New DataView
                    Dv_DuplicateSubjectNoWithDeleted = Ds_Check.Tables(0).Copy.DefaultView()
                    Dv_DuplicateRejectedNo = Ds_Check.Tables(0).Copy.DefaultView()
                    Dv_DuplicateRejectedNo.RowFilter = "cRejectionFlag='Y'"
                    If Dv_DuplicateRejectedNo.ToTable().Rows.Count = 0 Then
                        Dv_DuplicateSubjectNoWithDeleted.RowFilter = "cStatusIndi='D'"
                        If Dv_DuplicateSubjectNoWithDeleted.ToTable().Rows.Count = 0 Then
                            objcommon.ShowAlert("Screen No. already assigned to another Subject.", Me.Page)
                            'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "SubjectMstDivShowHide('S');", True)
                            Me.MpeSubjectMst.Show()
                            Me.txtScreenNo.Focus()
                            Return False
                        Else

                        End If
                    Else
                        objcommon.ShowAlert("Screen No. already assigned to another Subject.", Me.Page)
                        Return False
                    End If
                End If
            End If
            '**********Checking for duplication Screen No ends**************************************

            '********Checking for duplicate Randomization No************

            If CType(Me.ViewState(VS_Mode), String).ToUpper() = "EDIT" AndAlso Me.txtPatientRandomizationNo.Text.Trim() <> "" Then

                wStr = "vRandomizationNo = '" + Me.txtPatientRandomizationNo.Text.Trim() + "'"
                wStr += " And vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
                wStr += " And vWorkspaceSubjectId <> '" + CType(Me.ViewState(VS_WorkspaceSubjectId), String).Trim() + "'"

                If Not Me.objHelp.GetWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                            Ds_Check, estr) Then
                    Throw New Exception(estr)
                End If

                If Not Ds_Check.Tables(0) Is Nothing Then
                    If Ds_Check.Tables(0).Rows.Count > 0 Then
                        objcommon.ShowAlert("Patient/Randomization No. already assigned to another Subject.", Me.Page)
                        Me.MpeSubjectMst.Show()
                        Me.txtPatientRandomizationNo.Focus()
                        Return False
                    End If
                End If

            End If

            '**********Checking for duplication Randomization No ends**************************************

            If CType(Me.ViewState(VS_Mode), String).ToUpper() = "ADD" Then

                'Getting Maximum SubjectNo set iMySubjectNo changed by Mani to get Minimun iMySubjectNo

                If Not Me.objHelp.GetFieldsOfTable("WorkspaceSubjectMst", "ISNULL(MAX(iMySubjectNo),0) as MaxSubNo", _
                                       "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'", dsSubjectNo, estr) Then
                    Throw New Exception(estr)
                End If

                If dsSubjectNo.Tables(0).Rows(0)("MaxSubNo") = 0 Then
                    SubjectNo = 1
                Else
                    SubjectNo = dsSubjectNo.Tables(0).Rows(0)("MaxSubNo") + 1
                End If
                '****************************

                Dt_WorkspaceSubjectMst = CType(Me.ViewState(VS_WorkspaceSubjectMst), DataTable)
                Dt_WorkspaceSubjectMst.Clear()
                Dt_WorkspaceSubjectMst.AcceptChanges()

                dr = Dt_WorkspaceSubjectMst.NewRow()
                dr("vWorkspaceSubjectId") = 0
                dr("vWorkspaceid") = Me.HProjectId.Value.Trim()
                dr("iMySubjectNo") = SubjectNo
                'dr("vRandomizationNo") = 0
                dr("vSubjectId") = SubjectId
                dr("vInitials") = Me.txtInitial.Text.Trim.ToUpper()

                dr("iPeriod") = ddlPeriod.SelectedValue
                'SD
                'dr("dReportingDate") = Now.Date.ToString()
                dr("dReportingDate") = objcommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName))  'Its getting saved from Insert Procedure
                dr("cRejectionFlag") = "N"
                'dr("nReasonNo") = ""
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("vMySubjectNo") = Me.txtScreenNo.Text.Trim().ToUpper()

                dr("iTranNo") = 0
                dr("nWorkspaceSubjectHistoryId") = 0
                dr("dICFDate") = System.DateTime.Now
                'Me.txtICFSignedDate.Text.Trim()

                Dt_WorkspaceSubjectMst.Rows.Add(dr)

            ElseIf CType(Me.ViewState(VS_Mode), String).ToUpper() = "EDIT" Or CType(Me.ViewState(VS_Mode), String).ToUpper() = "REVERSERANDOMIZATION" Then
                Dt_WorkspaceSubjectMst = CType(Me.ViewState(VS_WorkspaceSubjectMst), DataTable)

                For Each dr In Dt_WorkspaceSubjectMst.Rows
                    dr("vInitials") = Me.txtInitial.Text.Trim.ToUpper()
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("vMySubjectNo") = Me.txtScreenNo.Text.Trim().ToUpper()
                    dr("iTranNo") = 0
                    dr("nWorkspaceSubjectHistoryId") = 0
                    dr("dICFDate") = System.DateTime.Now
                    'Me.txtICFSignedDate.Text.Trim()
                    dr("vRandomizationNo") = Me.txtPatientRandomizationNo.Text.Trim().ToUpper()

                    If (Me.txtRandomizationRemarks.Text <> "") Then
                        dr("vRandomizationNo") = Me.txtPatientRandomizationNo.Text.Trim().ToUpper()
                        dr("cRandomizationType") = Me.hdnRandomizationType.Value()
                        dr("vRemarks") = IIf(Me.txtRandomizationRemarks.Text.Trim() <> "", Me.txtRandomizationRemarks.Text.Trim().ToUpper(), "")
                    ElseIf (Me.txtRemarks.Text <> "") Then
                        dr("vRandomizationNo") = Me.txtPatientRandomizationNo.Text.Trim().ToUpper()
                        dr("cRandomizationType") = ""
                        dr("vRemarks") = IIf(Me.txtRemarks.Text.Trim() <> "", Me.txtRemarks.Text.Trim().ToUpper(), "")
                    ElseIf (Me.txtReverseRandomizationRemarks.Text <> "") Then
                        dr("vRandomizationNo") = ""
                        dr("vRemarks") = IIf(Me.txtReverseRandomizationRemarks.Text.Trim() <> "", Me.txtReverseRandomizationRemarks.Text.Trim().ToUpper(), "")
                    End If

                    dr("iMySubjectNo") = HReplaceImySubjectNo.Value

                    dr("cScreenFailure") = ""
                    dr("cDisContinue") = ""
                    dr("vScreenFailureRemaks") = DBNull.Value
                    dr("dScreenFailureDate") = DBNull.Value
                    Dt_WorkspaceSubjectMst.AcceptChanges()
                Next
            End If

            Dt_WorkspaceSubjectMst.TableName = "VIEW_WORKSPACESUBJECTMST"
            Dt_WorkspaceSubjectMst.AcceptChanges()

            Me.ViewState(VS_WorkspaceSubjectMst) = Dt_WorkspaceSubjectMst
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...Enrollment")
            Return False
        End Try
    End Function

#End Region

#Region "ResetPage"

    Private Sub ResetPage()
        Me.txtInitial.Text = ""
        Me.txtLastName.Text = ""
        Me.txtMiddleName.Text = ""
        Me.txtScreenNo.Text = ""
        Me.txtICFSignedDate.Text = ""
        Me.txtFirstName.Text = ""
        Me.txtRemarks.Text = ""
        Me.trRemarks.Style.Add("display", "none")
        Me.txtPatientRandomizationNo.Text = ""
        Me.trPatientRandomizationNo.Style.Add("display", "none")          'For
        Me.HSubject.Value = ""
        Me.BtnSaveSubjectMst.Attributes.Add("OnClick", "return Validation();")
        ''''for reseting the subject demographic details Popup

        Me.txtRandomizationRemarks.Text = ""
        Me.txtReverseRandomizationRemarks.Text = ""


        Me.txtWeight.Text = ""
        Me.txtHeight.Text = ""
        Me.txtBMI.Text = ""
        Me.txtDOB.Text = ""

        Me.txtAGE.Text = ""


    End Sub

#End Region

#Region "Fill Reason DropDown"

    Private Function FillReason() As Boolean
        Dim estr As String = String.Empty
        Dim Ds_Reason As New DataSet
        Dim wStr As String = String.Empty
        Dim item As New ListItem

        Try

            wStr = " vActivityId = '" & ActId_SubjectRejection & "' and cStatusIndi <> 'D'  and vProjectTypeCode in(" & Session(S_ScopeValue) & ")"

            If Not Me.objHelp.View_ReasonMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                        Ds_Reason, estr) Then
                Throw New Exception(estr)
            End If

            Me.ddlReason.DataSource = Ds_Reason.Tables(0)
            Me.ddlReason.DataValueField = "nReasonNo"
            Me.ddlReason.DataTextField = "vReasonDesc"
            Me.ddlReason.DataBind()

            item.Text = "Please Select a Reason"
            item.Value = "0"
            Me.ddlReason.Items.Insert(0, item)
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Filling Reasons ,", ex.Message)
            Return False
        End Try

    End Function

    'Private Function FillActivity() As Boolean
    '    Dim wstr As String = ""
    '    Dim Ds_FillActivity As New DataSet
    '    Dim eStr_Retu As String = ""
    '    Try
    '        wstr = " vWorkspaceId = '" & Me.HProjectId.Value.Trim()
    '        wstr += "' And iPeriod = " & Me.ddlPeriod.SelectedValue.Trim()
    '        wstr += " And (vActivityId is Not NULL or vActivityId<>'') and cstatusindi<>'D' order by vActivityName"

    '        If Not objHelp.GetViewWorkSpaceNodeDetail(wstr, Ds_FillActivity, eStr_Retu) Then
    '            Throw New Exception(eStr_Retu)
    '        End If
    '        If Ds_FillActivity.Tables(0).Rows.Count <= 0 Then
    '            objCommon.ShowAlert("No Record Found", Me)
    '            'Me.ddlActivity.Items.Clear()
    '            'Me.ddlActivity.Items.Insert(0, New ListItem("Select Activity", 0))
    '            Return True
    '            Exit Function
    '        End If

    '        'Me.ddlActivity.DataSource = Ds_FillActivity
    '        'Me.ddlActivity.DataTextField = "ActivityDisplayName"
    '        'Me.ddlActivity.DataValueField = "iNodeid"
    '        'Me.ddlActivity.DataBind()

    '        'Me.ddlActivity.Items.Insert(0, New ListItem("Select Activity", 0))
    '        'Me.trvwStructure.Nodes.Clear()

    '        Return True
    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.Message, "")
    '        Return False
    '    End Try
    'End Function

#End Region

#Region "Reject Or Delete"

    Private Function RejectOrDelete(ByVal mode As String, ByVal RowIndex As Integer) As Boolean
        Dim estr As String = String.Empty
        Dim dr As DataRow
        Dim Ds_WorkspaceSubjectMst As New DataSet
        Dim wStr As String = String.Empty
        Dim Reason As String = String.Empty

        Try

            wStr = "vSubjectId = '" + Me.gvwSubjectSelectionForVisit.Rows(RowIndex).Cells(GVC_SubjectId).Text.Trim() + "'"
            wStr += " And vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"

            If Not Me.objHelp.GetWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                        Ds_WorkspaceSubjectMst, estr) Then
                Throw New Exception(estr)
            End If

            


            For Each dr In Ds_WorkspaceSubjectMst.Tables(0).Rows
                dr("cRejectionFlag") = "N"
                dr("cStatusIndi") = "D"

                If mode.ToUpper() = "REJECT" Then
                    dr("cRejectionFlag") = "Y"
                    dr("cStatusIndi") = "E"
                End If

                dr("nReasonNo") = CType(Me.ddlReason.SelectedItem.Value.Trim(), Integer)
                dr("vRemarks") = IIf(TxtReason.Text.ToString.Trim() <> "", TxtReason.Text.ToString.Trim().ToUpper(), "")
                dr("iModifyBy") = Me.Session(S_UserID)
            Next
            Ds_WorkspaceSubjectMst.AcceptChanges()

            If Ds_WorkspaceSubjectMst.Tables(0).Rows(0)("vRandomizationNo") <> "" Then
                Me.objcommon.ShowAlert("First Reverse Randomize This subject, After You Can Delete this Subject", Me.Page)
            Else
                If Not Me.objLambda.Save_InsertWorkspaceSubjectMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                    WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_WorkspaceSubjectMst, _
                    Ds_WorkspaceSubjectMst, Me.Session(S_UserID), estr) Then
                    Throw New Exception(estr)
                End If
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Rejecting or Deleting a Subject ,", ex.Message)
            Return False
        End Try
    End Function

#End Region

#Region "Deleted Subjects"
    Public Function FillDeletedSubjectGrid() As Boolean

        Dim Dt_SubjectMst As New DataTable
        Try
            Dt_SubjectMst = CType(Me.ViewState(Vs_DeletedSubjectData), DataTable)
            Me.GvwDeletedSubjectAuditTrail.DataSource = Dt_SubjectMst
            Me.GvwDeletedSubjectAuditTrail.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "DeleteSubjectAuditTrail_Datatable", "DeleteSubjectAuditTrail_Datatable(); ", True)
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    Protected Sub BtnDeletedSubjects_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnDeletedSubjects.Click
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetPatient", "fsetPatient_Show(); ", True)
        If Not FillDeletedSubjectGrid() Then
            Me.objcommon.ShowAlert("Error While Filling Deleted Subjects Detail", Me.Page)
            Exit Sub
        End If
        MpeDeletedSubjectPopUp.Show()

    End Sub
#End Region

#Region "Save Demographic Details"
    Protected Sub btnDemographicSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDemographicSubmit.Click

        Dim dsSubjectDemographic As New DataSet
        Dim estrDemographic As String = String.Empty
        Dim SubjectIDDemographic As String = String.Empty

        Try
            If Not AssignValueDemographic() Then
                Me.objcommon.ShowAlert("Error while Adding demographic Details Data", Me.Page)
                Exit Sub
            End If
            dsSubjectDemographic.Tables.Add(CType(ViewState(VS_DtSubjectMst), DataTable))

            If objLambda.Save_SubjectMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, dsSubjectDemographic, Me.Session(S_UserID), SubjectIDDemographic, estrDemographic, "") Then
                ResetPage()
                Me.objcommon.ShowAlert("Demographic details added sucessfully !", Me.Page)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....btnDemographicSubmit_Click")
        End Try

    End Sub

    Protected Sub gvwSubjectSelectionForVisit_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvwSubjectSelectionForVisit.RowDeleting

    End Sub

    Protected Sub gvwSubjectSelectionForVisit_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvwSubjectSelectionForVisit.RowEditing
        e.Cancel = True
    End Sub

#End Region

#Region "Period Dropdown(Fill & selectedIndexChanged)"
    Private Sub FillPeriodDropDown()
        Dim wStr As String = String.Empty
        Dim dsPeriod As New DataSet
        Dim eStr_Retu As String = String.Empty
        Try

            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
            If Not objHelp.GetViewWorkSpaceNodeDetail(wStr, dsPeriod, eStr_Retu) Then
                Me.ShowErrorMessage(eStr_Retu, "")
                Exit Sub
            End If

            Me.ddlPeriod.DataSource = dsPeriod.Tables(0).DefaultView.ToTable("WorkSpaceNodeDetail", True, "iPeriod")
            Me.ddlPeriod.DataTextField = "iPeriod"
            Me.ddlPeriod.DataValueField = "iPeriod"
            Me.ddlPeriod.DataBind()
            Me.ddlPeriod.Items.Insert(0, New ListItem("Select Period", "0"))
            Me.ddlPeriod.SelectedValue = 1
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "Error While Filling Period!")
        End Try
    End Sub

    Protected Sub ddlPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPeriod.SelectedIndexChanged
        Dim wstr As String = String.Empty
        Try

            If ddlPeriod.SelectedIndex = 1 Then
                Me.AutoCompleteExtenderSubject.ContextKey = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'"
                btnAdd.Text = "Add New Subject"
            Else ''Adding the filter on Available subjects for period other than one
                wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' and cRejectionFlag <> 'Y' and iPeriod = 1 and " & _
                            " vSubjectId not in (Select Distinct vSubjectId from WorkspaceSubjectMst where vWorkspaceId='" & Me.HProjectId.Value.Trim() & _
                            "' and iPeriod=" & ddlPeriod.SelectedValue & ")"
                Me.AutoCompleteExtenderSubject.ContextKey = wstr
                btnAdd.Text = "Enroll Subject"
            End If

            txtSubject.Text = ""
            HSubject.Value = ""
            If Not FillGrid() Then
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "Error on Period Selection!")
        End Try

    End Sub
#End Region

#Region "Subject Attendance"
    Private Sub SubjectAttendance()
        Try
            Dim estr As String = String.Empty
            Dim dsSubjectNo As New DataSet
            Dim SubjectNo As Integer = 0
            Dim dsSaveWorkspaceSubjectMst As New DataSet
            Dim dr As DataRow
            If Me.HSubject.Value.Trim() = "" Then
                objcommon.ShowAlert("Please Select Subject", Me.Page)
                Exit Sub
            End If
            If Not objHelp.GetWorkspaceSubjectMaster("vSubjectId='" + Me.HSubject.Value.Trim() + "' and vWorkspaceId='" & _
                                                             Me.HProjectId.Value.Trim() & "' And iPeriod=1 and cStatusindi<>'D' ", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                             dsSaveWorkspaceSubjectMst, estr) Then
                Throw New Exception(estr)
            End If

            For Each dr In dsSaveWorkspaceSubjectMst.Tables("WorkspaceSubjectMst").Rows
                If (Me.ddlPeriod.SelectedIndex <> 0) Then
                    dr("iPeriod") = Me.ddlPeriod.SelectedValue
                End If
                dr("dReportingDate") = objcommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName))  'Its getting saved from Insert Procedure
                dr("cRejectionFlag") = "N"
                dr("dModifyOn") = Today.Date.ToString("dd-MMM-yyyy")
                dr("cStatusIndi") = "N"
                dr("iModifyBy") = Me.Session(S_UserID)
                dr.AcceptChanges()
            Next
            dsSaveWorkspaceSubjectMst.Tables("WorkspaceSubjectMst").TableName = "VIEW_WORKSPACESUBJECTMST"
            dsSaveWorkspaceSubjectMst.AcceptChanges()

            If Not objLambda.Save_InsertWorkspaceSubjectMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_WorkspaceSubjectMst, dsSaveWorkspaceSubjectMst, Me.Session(S_UserID), estr) Then
                objcommon.ShowAlert("Error while Saving WorkspaceSubjectMst", Me.Page)
                Exit Sub
            End If
            If Not FillGrid() Then
                Exit Sub
            End If
            txtSubject.Text = ""
            HSubject.Value = ""
            objcommon.ShowAlert("Subject Added for Selected Period !", Me.Page)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "Error While Subject Attendance.")
        End Try

    End Sub
#End Region

#Region "rblPassFail"


#End Region

#Region "Give Screening Number"
    Private Function ScreeningDatail() As Boolean
        Dim dsSubjectNo As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim SubjectNo As String = String.Empty

        Try
            If Not Me.objHelp.GetFieldsOfTable("WorkspaceSubjectMst", "IsNull(max(iMySubjectNo),0) as MaxSubNo", _
                                       "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'", dsSubjectNo, eStr) Then
                Throw New Exception(eStr)
            End If

            If dsSubjectNo.Tables(0).Rows(0)("MaxSubNo") = 0 Then
                objcommon.ShowAlert("There is no subject Enrolled for this Subject", Me.Page)
                Exit Function
            Else
                SubjectNo = dsSubjectNo.Tables(0).Rows(0)("MaxSubNo")
            End If

            dsSubjectNo = New DataSet

            If Not Me.objHelp.GetFieldsOfTable("RandomizationDetail", "IsNull(max(iMySubjectNo),0) as MaxSubNo", _
                           "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' and iMySubjectNo=" + SubjectNo, dsSubjectNo, eStr) Then
                Throw New Exception(eStr)
            End If

            SubjectNo = ""
            If dsSubjectNo.Tables(0).Rows(0)("MaxSubNo") = 0 Then
                SubjectNo = "1001"
            Else
                SubjectNo = dsSubjectNo.Tables(0).Rows(0)("MaxSubNo") + 1
            End If

            dsSubjectNo = New DataSet
            If Not Me.objHelp.GetFieldsOfTable("RandomizationDetail", "*", _
               "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'and iPeriod=1 and iMySubjectNo=" + SubjectNo, dsSubjectNo, eStr) Then
                Throw New Exception(eStr)
            End If

            Me.txtPatientRandomizationNo.Text = dsSubjectNo.Tables(0).Rows(0)("iMySubjectNo").ToString()
            HReplaceImySubjectNo.Value = dsSubjectNo.Tables(0).Rows(0)("iMySubjectNo")


            Return True

        Catch ex As Exception
            Throw New Exception()
            Return False
        End Try
    End Function
#End Region

#Region "Authenticate Password"
    Private Function Auntheticate(ByVal Password As String) As Boolean
        Dim pwd As String = String.Empty
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        'pwd = Me.txtPassword.Text
        pwd = objHelp.EncryptPassword(Password)


        If (pwd.ToString() = "") Then
            Me.txtPassword.Focus()
            objcommon.ShowAlert("Please Enter Password !", Me.Page)
            Return False
        End If

        If Convert.ToString(Me.Session(S_Password)) <> pwd.ToString() Then
            objcommon.ShowAlert("Password Authentication Fails !", Me.Page)
            Me.txtPassword.Focus()
            Return False
        End If
        Return True
    End Function

#End Region

    <WebMethod> _
    Public Shared Function View_WorkSpaceNodeDetail(ByVal vWorkSpaceID As String) As String
        Dim ds_RoleOp As New Data.DataSet
        Dim estr As String = String.Empty
        Dim lblexceltitle As String = String.Empty
        Dim wstr As String = String.Empty
        Dim datamode As WS_HelpDbTable.DataRetrievalModeEnum
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim dtUserMstHistrory As New DataTable
        Dim strReturn As String = String.Empty
        Dim dt As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1

        Try

            wstr = "vWorkSpaceID = '" + vWorkSpaceID + "'"
            wstr += "and isNULL(vTemplateId,'') <> '0001' and iParentNodeId = '1' and cStatusIndi <> 'd'"

            datamode = WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition

            If Not objHelp.GetViewWorkSpaceNodeDetail(wstr, ds_RoleOp, estr) Then
                Return False
            End If


            dt = ds_RoleOp.Tables(0)

            strReturn = JsonConvert.SerializeObject(dt)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return strReturn
    End Function

    <WebMethod> _
    Public Shared Function Save_WorkSpaceSubjectMst(ByVal vWorkspaceSubjectId As String) As String
        Dim ds_RoleOp As New Data.DataSet
        Dim estr As String = String.Empty
        Dim lblexceltitle As String = String.Empty
        Dim wstr As String = String.Empty
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim Dt_WorkspaceSubjectMst As New DataTable
        Dim Ds_Check As New DataSet
        Dim strReturn As String = String.Empty
        Dim dt As New DataTable
        Dim i As Integer = 1
        Dim dr As DataRow
        Dim RandomizationNo As String = String.Empty
        Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
        Dim ds_WorkSpaceSubjectMst As New DataSet
        Dim dt_table As New DataTable

        Try

            Dim Ds_Subjectmst As New DataSet
            Dim mode = 4
            Dim Msg As String = String.Empty

            'mode = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

            wstr += "vWorkspaceSubjectId = '" + CType(vWorkspaceSubjectId, String).Trim() + "'"
            If Not objHelp.GetWorkspaceSubjectMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                            Ds_Check, estr) Then
                Throw New Exception(estr)
            End If

            Dt_WorkspaceSubjectMst = CType(Ds_Check.Tables(0), DataTable)

            For Each dr In Dt_WorkspaceSubjectMst.Rows
                dr("vRandomizationNo") = ""
                Dt_WorkspaceSubjectMst.AcceptChanges()
            Next

            Dt_WorkspaceSubjectMst.TableName = "VIEW_WORKSPACESUBJECTMST"
            Dt_WorkspaceSubjectMst.AcceptChanges()

            Ds_Subjectmst.Tables.Add(CType(Dt_WorkspaceSubjectMst, DataTable).Copy())
            Ds_Subjectmst.AcceptChanges()

            'wstr = ""
            ''wstr = "vSubjectId = '" + Me.HSubject.Value.Trim() + "' And vWorkspaceId = '"
            ''wstr += Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"

            'If Not objHelp.GetView_SubjectMaster(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '                                        ds_WorkSpaceSubjectMst, estr) Then
            '    Throw New Exception(estr)
            'End If

            'dt_table.TableName = "View_SubjectMaster"
            'dt_table = ds_WorkSpaceSubjectMst.Tables(0)


            'Ds_Subjectmst.Tables.Add(CType(ds_WorkSpaceSubjectMst.Tables(0), DataTable))

            If Not objLambda.Save_SubjectMst(mode, Ds_Subjectmst, _
                                            System.Web.HttpContext.Current.Session(S_UserID), vWorkspaceSubjectId, estr, RandomizationNo) Then
                Throw New Exception(estr)
            End If



            Msg = "Randomization Number Reverse Successully !"



            Return Msg
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return strReturn
    End Function


#Region "FillDropDownList Visits"

    Private Function FillDropDownListVisits() As Boolean
        Dim ds_Visits As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim Periods As Integer = 1
        Dim dtVisit As New DataTable
        Dim dsVisit As DataSet = New DataSet
        Dim dvVisit As DataView

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            Me.ddlVisits.Items.Clear()

            wStr = "vWorkSpaceId = '" & Me.HProjectId.Value.Trim() & "' AND cStatusIndi<>'D' And iPeriod = '1' And isnull(vTemplateId,0)<>'0001' Order By iNodeNo,iNodeid"

            If Not objHelp.GetViewWorkSpaceNodeDetail(wStr, ds_Visits, eStr) Then
                Me.objcommon.ShowAlert("Error while getting Information: " & eStr, Me.Page)
                Exit Function
            End If
            dtVisit = ds_Visits.Tables(0)
            dvVisit = New DataView(dtVisit)
            dvVisit.RowFilter = "iParentNodeId = 1"

            If dvVisit.ToTable().Rows.Count = 0 Then
                Throw New Exception("Problem while getting data")
            End If

            If Not dvVisit.ToTable().Rows.Count = 0 Then
                For count As Integer = 0 To dvVisit.ToTable().Rows.Count - 1
                    'Me.ddlVisits.Items.Add(dvVisit.ToTable().Rows(count)("vNodeDisplayName"))
                    Me.ddlVisits.Items.Add(New ListItem(dvVisit.ToTable().Rows(count)("vNodeDisplayName"), dvVisit.ToTable().Rows(count)("vActivityId").ToString() + "#" + dvVisit.ToTable().Rows(count)("iNodeId").ToString()))
                Next count
            End If


            ' dvActivity(ParentNode).Item("vNodeDisplayName").ToString()


            'wStr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "' and cStatusIndi<>'D'"

            'If Not objHelp.Proc_WorkspaceActivitySubjectMatrix(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '                            ds_Visits, eStr) Then
            '    Throw New Exception(eStr)
            'End If



            'If Not dvVisit.Tables(0).Rows(0)("vNodeDisplayName") Is Nothing Then
            '    Periods = ds_Visits.Tables(0).Rows(0)("vNodeDisplayName")
            '    For count As Integer = 0 To Periods - 1
            '        Me.ddlVisits.Items.Add((count + 1).ToString)
            '    Next count
            'End If

            Me.ddlVisits.Items.Insert(0, "Select Visit")

            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Legend", "legendUI();", True)
            Return True

        Catch ex As Exception
            ShowErrorMessage("Problem While Filling Visit. ", ex.Message)
            eStr = ex.Message
            Return False
        Finally
            ds_Visits.Dispose()
        End Try

    End Function

    Protected Sub ddlVisits_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlVisits.SelectedIndexChanged
        Dim eStr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim returnData As String = String.Empty
        Dim dsProjectActivityOperationDetails As New DataSet
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Try

            If Not FillScreenFailureAudittrailGrid() Then
                ModalScreenFailure.Show()
                FillGrid()
                Exit Sub
            End If
            ModalScreenFailure.Show()
            FillGrid()
        Catch ex As Exception
        Finally
        End Try
    End Sub

#End Region




End Class