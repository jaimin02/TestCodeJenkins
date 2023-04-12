Imports System.Web.Services
Imports Newtonsoft.Json

Partial Class frmCTMDashboard
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "

    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()

    Private Const VS_DtActivityStatus As String = "DtActivityStatus"
    Private Const VS_DtWorkspaceNodeDetail As String = "DtWorkspaceNodeDetail"
    Private Const VS_DtDCFMst As String = "DtDCFMst"
    Private Const VS_CurrentPage As String = "PageNo"
    Private Const VS_TotalRowCount As String = "TotalRowCount"
    Private Const VS_PagerStartPage As String = "PagerStartPage"
    Private Const VS_Mode As String = "mode"
    Private Const vs_gLock As String = "glock"
    Private Const Vs_DtWorkSpaceSubjectMst As String = "DtWorkSpaceSubjectMst"
    'Add by shivani pandya for dynamic review
    Private Const Vs_dsReviewerlevel As String = "dsReviewerlevel"

    ''Added by ketan
    Private Const VS_DtSubjectMst As String = "dtSubjectMst"
    Private Const VS_WorkspaceSubjectMst As String = "dtWorkspaceSubjectMst"
    Private Const VS_WorkspaceSubjectId As String = "WorkspaceSubjectId"
    Private Const SubjectType As String = "C"
    'Ended by ketan

    Private Const GVC_Review As Integer = 0
    Private Const GVC_RowNo As Integer = 1
    Private Const GVC_WorkspaceSubjectId As Integer = 2
    Private Const GVC_WorkSpaceId As Integer = 3
    Private Const GVC_Site As Integer = 4
    Private Const GVC_SubjectId As Integer = 5
    Private Const GVC_Initials As Integer = 6
    Private Const GVC_MySubjectNo As Integer = 7
    Private Const GVC_ScreenNo As Integer = 8
    Private Const GVC_RandomizationNo As Integer = 9
    Private Const GVC_SubjectName As Integer = 10
    Private Const GVC_Period As Integer = 11

    Private Const PAGESIZE As Integer = 5

    Private ActId As New ArrayList
    Private NodeId As New ArrayList

#End Region

#Region "Page Events"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.SetPaging()

        Me.Session(S_SelectedRepeatation) = Nothing
        Me.Session(S_GetLetestData) = Nothing
        If Not IsPostBack Then

            If Not GenCall() Then
                Exit Sub
            End If

            If Not Me.HProjectId.Value Is Nothing AndAlso Convert.ToString(Me.HProjectId.Value).Trim() <> "" Then
                If Not Me.FillDropDownListPeriods() Then
                    Exit Sub
                End If
            End If

        End If

    End Sub

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Try

            If Not GenCall_ShowUI() Then
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "......GenCall")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_Data"

    Private Function GenCall_Data() As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_ActivityStatus As New DataSet
        Dim ds_WorkspaceNodeDetail As New DataSet
        Dim ds_ParameterList As New DataSet

        Try

            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And iPeriod = " + Me.ddlPeriods.SelectedItem.Text.Trim()
            
            If Not objHelp.Proc_GetActivityStatusCount(Me.HProjectId.Value, Me.ddlPeriods.SelectedValue, ds_ActivityStatus, eStr) Then
                Throw New Exception(eStr)
            End If

            Me.Session(VS_DtActivityStatus) = ds_ActivityStatus.Tables(0)


            wStr += " And cStatusIndi <> 'D' OPTION (MAXDOP 1)"
            If Not Me.objHelp.GetViewWorkSpaceNodeDetail(wStr, ds_WorkspaceNodeDetail, eStr) Then
                Throw New Exception(eStr)
            End If
            Me.Session(VS_DtWorkspaceNodeDetail) = ds_WorkspaceNodeDetail.Tables(0)

            ''Added by Aaditya for Getting CompanySelection Parameter Value
            wStr = "vParameterName = 'cAllowEnrollmentPrefix' And cActiveFlag = 'Y' And cStatusIndi <> 'D'"
            If Not objHelp.GetParameterList(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ParameterList, eStr) Then
                MsgBox("Error while Getting ParameterList")
                Exit Function
            End If

            If Not IsNothing(ds_ParameterList.Tables(0)) AndAlso ds_ParameterList.Tables(0).Rows.Count > 0 Then
                If Convert.ToString(ds_ParameterList.Tables(0).Rows(0).Item("vParameterValue")).Trim() = "Y" Then
                    Me.Session("S_Prefix") = "Y"
                    Me.Session("S_ProjectNo") = Convert.ToString(Me.txtproject.Text).Trim()
                Else
                    Me.Session("S_Prefix") = "N"
                End If
            End If
            GenCall_Data = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "......GenCall_Data")
        Finally
            ds_ActivityStatus.Dispose()
            ds_WorkspaceNodeDetail.Dispose()
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
            Page.Title = " :: CRF Home ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Master.FindControl("lblHeading"), Label).Text = "CRF Home"
            Me.imgShow.Attributes.Add("onmouseover", "$('#" + Me.canal.ClientID + "').toggle('medium');")
            Me.canal.Attributes.Add("onmouseleave", "$('#" + Me.canal.ClientID + "').toggle('medium');")

            'added by Deepak Singh on 2-Mar-10 to show the set Project on page load
            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
            End If

            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
            '========================================================================

            Me.ddlPeriods.Style.Add("display", "")
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                'Me.lblPeriods.Style.Add("display", "block")
                'Me.ddlPeriod.Style.Add("display", "block")
                Me.lblPeriods.Visible = False
                Me.ddlPeriods.Visible = False
            End If

            'Commented by shivani pandya for dynamic review
            ''*************Electronic Signature***************************

            'If Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_FinalReviewAndLock AndAlso Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
            '    Me.trName.Visible = False
            '    Me.trDesignation.Visible = False
            '    Me.trRemarks.Visible = False
            '    Me.divAuthentication.Style.Add("height", "150px")
            'ElseIf Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_SecondReview AndAlso Me.Session(S_ScopeNo) = Scope_BABE Then
            '    Me.trName.Visible = False
            '    Me.trDesignation.Visible = False
            '    Me.trRemarks.Visible = False
            '    Me.divAuthentication.Style.Add("height", "150px")
            'End If

            'Me.lblSignername.Text = Me.Session(S_FirstName).ToString() + " " + Me.Session(S_LastName).ToString()
            'Dim dt_Profiles As New DataTable
            'dt_Profiles = CType(Me.Session(S_Profiles), DataTable)
            'Dim dv_Profiles As DataView
            'dv_Profiles = dt_Profiles.DefaultView
            'dv_Profiles.RowFilter = "vUserTypeCode = '" + Me.Session(S_UserType).ToString() + "'"
            'Me.lblSignerDesignation.Text = dv_Profiles.ToTable.Rows(0)("vUserTypeName").ToString()
            ''Me.lblSignDateTime.Text = Me.objHelp.GetServerDateTime().ToString("dd-MMM-yyyy hh:mm:ss")
            'Me.lblSignRemarks.Text = "I attest to the accuracy and integrity of the data being reviewed."

            ''*************Electronic Signature***************************

            'If (Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_FinalReviewAndLock AndAlso _
            '    Me.Session(S_ScopeNo) = Scope_ClinicalTrial) Or (Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_SecondReview AndAlso _
            '    Me.Session(S_ScopeNo) = Scope_BABE) Then
            '    Me.hdnDirectAuthentication.Value = "true"
            'Else
            '    Me.hdnDirectAuthentication.Value = "false"
            'End If


            GenCall_ShowUI = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....GenCall_ShowUI")
        Finally
            ds_Workspace.Dispose()
        End Try

    End Function

#End Region

#Region "FillGrid"

    Private Function FillGrid() As Boolean
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim sSubjectIds As String = String.Empty
        Dim Parameters As String = String.Empty
        Dim ds_Subject As New DataSet
        Dim ds_DCFMst As New DataSet
        Dim dv_Subject As New DataView
        Dim Ds_WorkSpaceSubjectMst As New DataSet
        Dim SHeader() As String
        Dim dtData As New DataTable
        Try
            TrLegends.Visible = False

            gvwSubjectSelectionForVisit.DataSource = Nothing

            If Me.HProjectId.Value.ToString.Trim() = "" Then
                Return True
            End If

            If Me.ddlPeriods.SelectedItem Is Nothing Then
                Return True
            End If

            Parameters = Me.HProjectId.Value.Trim() + "##" + Me.ddlPeriods.SelectedItem.Text.Trim() + "##" + _
                            Convert.ToString(Me.ViewState(VS_CurrentPage)).Trim() + "##" + PAGESIZE.ToString()

            If Me.HSubject.Value.ToString.Trim() <> "" Then
                Parameters += "##" + Me.HSubject.Value.ToString.Trim()
            Else
                Parameters += "##" + ""
            End If

            wStr = "vWorkSpaceId='" & HProjectId.Value.ToUpper() & "' And cStatusIndi <> 'D'"
            wStr += " And iPeriod = " + Me.ddlPeriods.SelectedItem.Text.Trim()

            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then

                Parameters += "##" + Session(S_UserType) + "##" + Session(S_UserID)

                If Not objHelp.Proc_WorkspaceActivitySubjectMatrix(Parameters, ds_Subject, eStr) Then
                    Throw New Exception(eStr)
                End If

                If Not objHelp.GetWorkspaceSubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_WorkSpaceSubjectMst, eStr) Then
                    Throw New Exception("Error While Getting Data From WorkSpaceSubjectMst")
                End If
                ''Me.ViewState(Vs_DtWorkSpaceSubjectMst) = Ds_WorkSpaceSubjectMst.Tables(0).Copy()
                Me.Session(Vs_DtWorkSpaceSubjectMst) = Ds_WorkSpaceSubjectMst.Tables(0).Copy()

            Else

                If Not objHelp.Proc_WorkspaceActivitySubjectMatrix_BABE(Parameters, ds_Subject, eStr) Then
                    Throw New Exception(eStr)
                End If

                If Not objHelp.GetWorkspaceSubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_WorkSpaceSubjectMst, eStr) Then
                    Throw New Exception("Error While Getting Data From WorkSpaceSubjectMst")
                End If
                ''Me.ViewState(Vs_DtWorkSpaceSubjectMst) = Ds_WorkSpaceSubjectMst.Tables(0).Copy()
                Me.Session(Vs_DtWorkSpaceSubjectMst) = Ds_WorkSpaceSubjectMst.Tables(0).Copy()

            End If

            If ds_Subject Is Nothing Then
                objcommon.ShowAlert("No data found.", Me.Page)
                gvwSubjectSelectionForVisit.DataSource = Nothing
                gvwSubjectSelectionForVisit.DataBind()
                'Add by shivani pandya for fix column hide issue
                If gvwSubjectSelectionForVisit.Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getDatatable", "getDatatable();", True)
                End If
                Return False    'Added by ketan
            ElseIf ds_Subject.Tables.Count = 0 Then
                objcommon.ShowAlert("No data found.", Me.Page)
                gvwSubjectSelectionForVisit.DataSource = Nothing
                gvwSubjectSelectionForVisit.DataBind()
                If gvwSubjectSelectionForVisit.Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getDatatable", "getDatatable();", True)
                End If
                Return False
            End If

            If ds_Subject.Tables(0).Rows.Count = 0 Then
                objcommon.ShowAlert("No data found.", Me.Page)
                gvwSubjectSelectionForVisit.DataSource = Nothing
                gvwSubjectSelectionForVisit.DataBind()
                'Add by shivani pandya for fix column hide issue
                If gvwSubjectSelectionForVisit.Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getDatatable", "getDatatable();", True)
                End If
                Return False   'Added by ketan
            End If

            For Each dr As DataRow In ds_Subject.Tables(0).Rows
                sSubjectIds += "'" + dr("vSubjectId").ToString() + "',"
            Next
            If sSubjectIds <> "" Then
                sSubjectIds = sSubjectIds.Substring(0, sSubjectIds.Length - 1)
                wStr += " AND vSubjectID IN (" + sSubjectIds + ")"
            End If

            If Not Me.objHelp.View_DCFMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                             ds_DCFMst, eStr) Then
                Throw New Exception(eStr)
            End If

            ''Me.ViewState(VS_DtDCFMst) = ds_DCFMst.Tables(0)
            Me.Session(VS_DtDCFMst) = ds_DCFMst.Tables(0)

            dv_Subject = ds_Subject.Tables(0).DefaultView

            If Me.chkAll.Checked = False Then
                dv_Subject.RowFilter = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
            End If

            If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                dv_Subject.RowFilter = "iPeriod = " + Me.ddlPeriods.SelectedItem.Text.Trim()
            End If

            If Me.HSubject.Value.ToString.Trim() <> "" Then
                dv_Subject.RowFilter = "vSubjectId = '" + Me.HSubject.Value.ToString.Trim() + "'"
            End If

            gvwSubjectSelectionForVisit.DataSource = Nothing

            If dv_Subject.ToTable.Rows.Count > 0 Then
                Dim ds = objHelp.GetMenu(CType(Session(S_UserType), Integer))
                Dim dt As DataTable = ds.Tables(0)
                dt.DefaultView.RowFilter = "MenuURL = 'frmCRFActivityStatusReport.aspx'"

                If dt.DefaultView.ToTable.Rows.Count > 0 Then
                    img7.Visible = True
                End If
                Dim dt1 As DataTable = ds.Tables(0)
                dt1.DefaultView.RowFilter = "MenuURL = 'frmCTMDiscrepancyStatusReport.aspx'"
                If dt1.DefaultView.ToTable.Rows.Count > 0 Then
                    img8.Visible = True
                End If

                Dim dt2 As DataTable = ds.Tables(0)
                dt2.DefaultView.RowFilter = "MenuURL = 'frmVisitScheduler.aspx'"
                If dt1.DefaultView.ToTable.Rows.Count > 0 Then
                    imgVisitScheduler.Visible = True
                End If



                Me.gvwSubjectSelectionForVisit.AutoGenerateColumns = True
                dtData = dv_Subject.ToTable()
                Me.ViewState("getData") = dtData

                'For Each drCol As DataColumn In dtData.Columns
                '    Dim str As String = String.Empty
                '    Dim strData As String = String.Empty
                '    If drCol.ToString().Length > 20 Then
                '        SHeader = Convert.ToString(drCol).Split(" ")
                '        For Each fern As String In SHeader                          
                '            strData = strData + fern.First
                '            If fern = "CHECK-IN" Then
                '                strData = strData + "_" + "IN" + "_"
                '            End If
                '            If fern = "CHECK-OUT" Then
                '                strData = strData + "_" + "OUT" + "_"
                '            End If
                '        Next
                '        dtData.Columns(Convert.ToString(drCol)).ColumnName = strData
                '        dtData.AcceptChanges()
                '    End If
                'Next
                gvwSubjectSelectionForVisit.DataSource = dtData

            End If

            gvwSubjectSelectionForVisit.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetPatient", "fsetPatient_Show(); ", True)
            upSubjectSelectionForVisit.Update()   ''ADDED BY KETAN FOR UPDATE UPDATEPANEL
            'Add by shivani pandya for fix column hide issue
            If gvwSubjectSelectionForVisit.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getDatatable", "getDatatable();", True)
            End If

            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....FillGrid")
            Return False
        Finally

            ' Changed By Nipun on 20-04-2015 To show No Data Found Message
            If Not ds_DCFMst Is Nothing Then
                ds_DCFMst.Dispose()
            End If
            If Not ds_Subject Is Nothing Then
                ds_Subject.Dispose()
            End If
            If Not Ds_WorkSpaceSubjectMst Is Nothing Then
                Ds_WorkSpaceSubjectMst.Dispose()
            End If
            If Not dv_Subject Is Nothing Then
                dv_Subject.Dispose()
            End If
            '=====================================================================
        End Try

    End Function
    Private Function getShortHeader() As Boolean
        Dim dt As New DataTable
        Dim dtHeader As New DataTable
        Dim dv_subject As New DataView
        Try
            dv_subject = Me.ViewState("getCurrentData")
           
        Catch ex As Exception
            Throw New Exception("Error while getShortHeader")
        End Try
    End Function

#End Region

#Region "FillDropDownList Periods"

    Private Function FillDropDownListPeriods() As Boolean
        Dim ds_Periods As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim Periods As Integer = 1

        Try

            Me.ddlPeriods.Items.Clear()

            wStr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'"

            If Not objHelp.GetViewWorkSpaceNodeDetail(wStr, ds_Periods, eStr) Then
                Throw New Exception(eStr)
            End If
            If ds_Periods.Tables(0).Rows.Count > 0 Then

                Periods = ds_Periods.Tables(0).DefaultView.ToTable(True, "iPeriod").Rows.Count
                For count As Integer = 0 To Periods - 1
                    Me.ddlPeriods.Items.Add((count + 1).ToString)
                Next count

            End If

            Return True

        Catch ex As Exception
            ShowErrorMessage("Problem While Filling Periods. ", ex.Message)
            Return False
        Finally
            ds_Periods.Dispose()
        End Try

    End Function

#End Region

#Region "Grid Events"

    Protected Sub gvwSubjectSelectionForVisit_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvwSubjectSelectionForVisit.DataBound
        Me.SetPaging()
    End Sub

    Protected Sub gvwSubjectSelectionForVisit_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwSubjectSelectionForVisit.RowCreated
        Dim dv As New DataView
        Dim dt As New DataTable
        'Add by shivani pandya for dynamic review
        Dim ds As New DataSet

        Try

            dt = CType(ViewState(vs_gLock), DataTable)
            ' dt = dv.ToTable()
            If e.Row.RowType = DataControlRowType.Header Or _
                    e.Row.RowType = DataControlRowType.DataRow Or _
                    e.Row.RowType = DataControlRowType.Footer Then

                e.Row.Cells(GVC_WorkspaceSubjectId).Visible = False
                e.Row.Cells(GVC_WorkSpaceId).Visible = False
                e.Row.Cells(GVC_SubjectId).Visible = False
                e.Row.Cells(GVC_SubjectName).Visible = False
                e.Row.Cells(GVC_MySubjectNo).Visible = False
                e.Row.Cells(GVC_Period).Visible = False
                e.Row.Cells(GVC_RowNo).Visible = False

                hdnIsReview.Value = "true"
                '' Commented By Dharmesh For Temporary on 26-May-2011
                If dt.Rows.Count > 0 Then
                    If dt.Rows(0).Item("cLockFlag") = "L" Then
                        'chkReview = CType(Me.gvwSubjectSelectionForVisit.Rows(Index).Cells(GVC_Review).FindControl("chkReview"), CheckBox)
                        e.Row.Cells(GVC_Review).Visible = False
                        hdnIsReview.Value = "false"
                    End If
                    '''' ************************************************************************* ''
                End If

                Dim ds_Review As New DataSet
                Dim estr As String = ""
                Dim lbl As New Label
                Dim dv_Review As DataView
                Try
                    If Not Me.objHelp.Proc_GetProjectReviewerLevel(Me.HProjectId.Value.ToString.Trim(), ds_Review, estr) Then
                        Throw New Exception("Error while getting data from Proc_GetProjectReviewerLevel " + estr.Trim)
                    End If

                    dv = ds_Review.Tables(0).Copy.DefaultView
                    dv.RowFilter = "vUsertypecode like '%" + Convert.ToString(Me.Session(S_UserType)) + "%'"

                    
                    If dv.ToTable.Rows.Count > 0 Then
                        If dv.ToTable().Rows(0).Item("vReviewerlevel") = "First Review" And Session(S_ScopeNo) = Scope_ClinicalTrial Then
                            e.Row.Cells(GVC_Review).Visible = False
                        End If
                    End If

                Catch ex As Exception

                End Try



                'Change by shivani pandya for dynamic review
                If Convert.ToInt16(Me.Session(S_WorkFlowStageId)) < 10 Then
                    e.Row.Cells(GVC_Review).Visible = False
                    hdnIsReview.Value = "false"
                Else
                    ds = CType(Me.Session(Vs_dsReviewerlevel), DataSet).Copy()
                    If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                        dv = ds.Tables(0).DefaultView
                        dv.RowFilter = "iReviewWorkflowStageId = " + Me.Session(S_WorkFlowStageId).ToString() + " AND vUserTypeCode = '" + Me.Session(S_UserType).ToString() + "'"
                        If dv.ToTable().Rows.Count = 0 Then
                            e.Row.Cells(GVC_Review).Visible = False
                            hdnIsReview.Value = "false"
                        End If
                    Else
                        e.Row.Cells(GVC_Review).Visible = False
                        hdnIsReview.Value = "false"
                    End If
                End If
                'End

                'Commented by shivani pandya for dynamic review
                'If Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_SecondReview AndAlso _
                '        'Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_FinalReviewAndLock AndAlso _
                '    'Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                '    e.Row.Cells(GVC_Review).Visible = False
                '    hdnIsReview.Value = "false"
                'End If

                'If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim.ToUpper() <> WorkFlowStageId_FinalReviewAndLock AndAlso _
                '        Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim.ToUpper() <> WorkFlowStageId_FirstReview AndAlso _
                '        Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim.ToUpper() <> WorkFlowStageId_SecondReview Then
                '    e.Row.Cells(GVC_Review).Visible = False
                '    hdnIsReview.Value = "false"
                'End If

            End If
            'Add by shivani pandya for datatable width at first time
            If e.Row.RowType = DataControlRowType.DataRow Then

                e.Row.Cells(GVC_Site).Style.Add("height", "28px")
                e.Row.Cells(GVC_Initials).Style.Add("height", "28px")
                e.Row.Cells(GVC_ScreenNo).Style.Add("height", "28px")

            End If


        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....gvwSubjectSelectionForVisit_RowCreated")
        Finally
            dt.Dispose()
            dv.Dispose()
        End Try

    End Sub

    Protected Sub gvwSubjectSelectionForVisit_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwSubjectSelectionForVisit.RowDataBound
        Dim rindex As Integer = e.Row.RowIndex
        Dim WorkspaceId As String = String.Empty
        Dim Period As String = String.Empty
        Dim SubjectId As String = String.Empty
        Dim MySubjectNo As String = String.Empty
        Dim ScreenNo As String = String.Empty
        Dim Initials As String = String.Empty
        Dim HdrStr As String = String.Empty
        Dim strHeader As String = String.Empty
        Dim strData() As String

        Dim DataEntry_Pending As Integer
        Dim DataEntry_Continue As Integer
        Dim DataEntry_Review As Integer
        Dim DataEntry_FirstReviewCompleted As Integer
        Dim DataEntry_SecondReviewCompleted As Integer
        Dim DataEntry_Locked As Integer
        Dim Total_Activities As Integer
        Dim maxStatus As Integer
        Dim minStatus As Integer

        Dim dt_ActivityStatus As New DataTable
        Dim dv_ActivityStatus As New DataView

        Dim dt_WorkspaceNodeDetail As New DataTable
        Dim dv_WorkspaceNodeDetail As New DataView

        Dim dt_Repeatation As New DataTable
        Dim dv_Repeatation As New DataView

        Dim dt_DCFMst As New DataTable
        Dim dv_DCFMst As New DataView

        Dim imgbtn As ImageButton
        Dim imgTempButton As New ImageButton

        Dim Pending As Integer
        Dim AutoResolved As Integer
        Dim Resolved As Integer
        Dim Answered As Integer
        Dim InternallyResolved As Integer
        Dim RedirectStr As String = String.Empty
        Dim str As String = String.Empty
        Dim RowFilterStr As String = String.Empty
        Dim mode As String = String.Empty
        Dim Ds_WorkSpaceSubjectMst As New DataSet
        Dim Dv_WorkSpaceSubjectMst As New DataView
        Dim Dt_WorkSpaceSubjectMst As New DataTable
        Dim dtData As New DataTable

        Try
            dtData = Me.ViewState("getData")
            If e.Row.RowType = DataControlRowType.Header Then

                For HdrIndex As Integer = 0 To e.Row.Cells.Count - 1

                    ActId.Add("0")
                    NodeId.Add("0")

                    If e.Row.Cells(HdrIndex).Text.Contains("#") Then


                        HdrStr = Context.Server.HtmlDecode(e.Row.Cells(HdrIndex).Text.Trim())
                        ActId.Insert(HdrIndex, HdrStr.Substring(HdrStr.IndexOf("#") + 1, HdrStr.LastIndexOf("#") - (HdrStr.IndexOf("#") + 1)))
                        NodeId.Insert(HdrIndex, HdrStr.Substring(HdrStr.LastIndexOf("#") + 1).Trim())

                        strHeader = HdrStr.Substring(0, HdrStr.IndexOf("#"))

                        'Add by shivani pandya for header wraper
                        e.Row.Cells(HdrIndex).Attributes.Add("title", strHeader)

                        If strHeader.Length > 10 Then
                            e.Row.Cells(HdrIndex).Text = strHeader.Split(" ")(0) + "..."
                        Else
                            e.Row.Cells(HdrIndex).Text = strHeader + "..."
                        End If

                    End If
                    'Add by shivani pandya for header wraper

                    If e.Row.Cells(HdrIndex).Text = "Patient/ Randomization No" Then
                        strHeader = "Patient/ Randomization No"
                        e.Row.Cells(HdrIndex).Attributes.Add("title", strHeader)
                        e.Row.Cells(HdrIndex).Text = strHeader.Split(" ")(0) + "..."
                        'e.Row.Cells(HdrIndex).Text = strHeader.Substring(0, 8)
                    End If

                Next HdrIndex


            ElseIf e.Row.RowType = DataControlRowType.DataRow Then

                '' Added by ketan for Add Link Button in Gridview
                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial AndAlso Me.Session(S_WorkFlowStageId) = "0" Then
                    Dim lbtnScreenNO As LinkButton = New LinkButton()
                    lbtnScreenNO.Text = Convert.ToString(e.Row.Cells(GVC_ScreenNo).Text)
                    lbtnScreenNO.ToolTip = "Add/Update Randomization No"
                    lbtnScreenNO.Attributes.Add("OnClick", "ShowConfirmPopup('" + e.Row.Cells(GVC_SubjectId).Text + "'); return false; ")
                    e.Row.Cells(GVC_ScreenNo).Controls.Add(lbtnScreenNO)
                End If
                '' Ended By ketan

                CType(e.Row.FindControl("chkReview"), CheckBox).Attributes.Add("OnClick", "CheckUncheckAll(this,'gvwSubjectSelectionForVisit')")

                For cIndex As Integer = 0 To e.Row.Cells.Count - 1

                    imgTempButton = New ImageButton
                    imgTempButton.ID = "imgbtnTempData" + rindex.ToString() + cIndex.ToString()
                    imgTempButton.SkinID = "ImgBtnDataEntryPending"
                    imgTempButton.Visible = True

                    'e.Row.Cells(cIndex).Controls.Add(New LiteralControl("&nbsp;&nbsp;"))

                    If e.Row.Cells(cIndex).Text.Trim.ToUpper() = CRF_DataEntryPending Then

                        ''dt_ActivityStatus = CType(Me.ViewState(VS_DtActivityStatus), DataTable)
                        ''dt_WorkspaceNodeDetail = CType(Me.ViewState(VS_DtWorkspaceNodeDetail), DataTable)
                        ''dt_DCFMst = CType(Me.ViewState(VS_DtDCFMst), DataTable)


                        dt_ActivityStatus = CType(Me.Session(VS_DtActivityStatus), DataTable)
                        dt_WorkspaceNodeDetail = CType(Me.Session(VS_DtWorkspaceNodeDetail), DataTable)
                        dt_DCFMst = CType(Me.Session(VS_DtDCFMst), DataTable)

                        DataEntry_Pending = 0
                        DataEntry_Continue = 0
                        DataEntry_Review = 0
                        DataEntry_FirstReviewCompleted = 0
                        DataEntry_SecondReviewCompleted = 0
                        DataEntry_Locked = 0
                        Total_Activities = 0
                        maxStatus = 0
                        minStatus = 0
                        Pending = 0
                        AutoResolved = 0
                        Resolved = 0
                        Answered = 0
                        InternallyResolved = 0

                        imgbtn = New ImageButton
                        imgbtn.ID = "imgbtnAssign" + rindex.ToString() + cIndex.ToString()
                        imgbtn.SkinID = "ImgBtnDataEntryPending"
                        imgbtn.Visible = True

                        WorkspaceId = Me.HProjectId.Value.Trim()
                        SubjectId = IIf(e.Row.Cells(GVC_SubjectId).Text.Trim() = "&nbsp;", "", e.Row.Cells(GVC_SubjectId).Text.Trim())
                        MySubjectNo = IIf(e.Row.Cells(GVC_MySubjectNo).Text.Trim() = "&nbsp;", "", e.Row.Cells(GVC_MySubjectNo).Text.Trim())
                        ScreenNo = IIf(e.Row.Cells(GVC_ScreenNo).Text.Trim() = "&nbsp;", "", e.Row.Cells(GVC_ScreenNo).Text.Trim())
                        Initials = IIf(e.Row.Cells(GVC_Initials).Text.Trim() = "&nbsp;", "", e.Row.Cells(GVC_Initials).Text.Trim())
                        Period = IIf(e.Row.Cells(GVC_Period).Text.Trim() = "&nbsp;", "", e.Row.Cells(GVC_Period).Text.Trim())

                        RedirectStr = ""
                        RedirectStr = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" & WorkspaceId & _
                                        "&ActivityId=" & ActId(cIndex) & "&NodeId=" & NodeId(cIndex) & _
                                        "&PeriodId=" & Period & "&SubjectId=" & SubjectId & _
                                        "&MySubjectNo=" & MySubjectNo + "&ScreenNo=" & ScreenNo + "&mode=" & ViewState(VS_Mode)

                        imgbtn.OnClientClick = "return OpenWindow('" + RedirectStr + "');"

                        imgbtn.ToolTip = "Data Entry Pending"

                        'Setting activity Status
                        dv_ActivityStatus = dt_ActivityStatus.Copy().DefaultView

                        dv_ActivityStatus.RowFilter = " (iParentNodeId = " + NodeId(cIndex).ToString() + " or iNodeID=" + NodeId(cIndex).ToString() + ")  And vSubjectId = '" + SubjectId + "'"

                        If dv_ActivityStatus.ToTable().Rows.Count > 1 Then
                            dv_ActivityStatus.RowFilter = " iParentNodeId=" + NodeId(cIndex).ToString() + "  And vSubjectId = '" + SubjectId + "'"
                        End If


                        dv_WorkspaceNodeDetail = dt_WorkspaceNodeDetail.Copy().DefaultView

                        '*******Row filter according to Scope********Not Calculating parent activities for Clinical Trails Scope**********
                        RowFilterStr = "iParentNodeId = " + NodeId(cIndex).ToString()
                        If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                            RowFilterStr = " cSubjectWiseFlag = 'Y' "
                            If SubjectId = "0000" Then
                                RowFilterStr = " cSubjectWiseFlag = 'N' "
                            End If
                            'RowFilterStr += " And (iParentNodeId = " + NodeId(cIndex).ToString() + " Or iNodeId = " + NodeId(cIndex).ToString() + ")"
                            RowFilterStr += " And (iParentNodeId = " + NodeId(cIndex).ToString() + " )"
                        End If
                        '*************************************************************

                        dv_WorkspaceNodeDetail.RowFilter = RowFilterStr


                        Total_Activities = dv_WorkspaceNodeDetail.ToTable().Rows.Count

                        '***For getting repeatations of activity for perfact status
                        dv_Repeatation = dt_ActivityStatus.Copy().DefaultView


                        dv_Repeatation.RowFilter = "iParentNodeId = " + NodeId(cIndex).ToString() + " And vSubjectId = '" + SubjectId + "' And iRepeatNo > 1"
                        If dv_Repeatation.ToTable().Rows.Count < 1 Then
                            dv_Repeatation.RowFilter = "iNodeId = " + NodeId(cIndex).ToString() + " And vSubjectId = '" + SubjectId + "' And iRepeatNo > 1"
                        End If
                        '--Pratiksha
                        Total_Activities += dv_Repeatation.ToTable().Rows.Count
                        '************************************

                        If dv_ActivityStatus.ToTable().Rows.Count = 0 Then
                            Total_Activities = 0
                        End If

                        imgbtn.ToolTip = "Data Entry Pending"
                        imgbtn.SkinID = "ImgBtnDataEntryPending"
                        If dv_ActivityStatus.Count > 0 Then


                            If dv_ActivityStatus(0)("Status").ToString() = "DEP" Then
                                imgbtn.ToolTip = "Data Entry Pending"
                                imgbtn.SkinID = "ImgBtnDataEntryPending"
                            ElseIf dv_ActivityStatus(0)("Status").ToString() = "DEC" Then
                                imgbtn.SkinID = "ImgBtnDataEntryContinue"
                                imgbtn.ToolTip = "Data Entry Continue"
                            ElseIf dv_ActivityStatus(0)("Status").ToString() = "RFR" Then
                                imgbtn.SkinID = "ImgBtnReviewing"
                                imgbtn.ToolTip = "Ready For Review"
                            ElseIf dv_ActivityStatus(0)("Status").ToString() = "FRD" Then
                                imgbtn.SkinID = "ImgBtnFirstReviewed"
                                imgbtn.ToolTip = "First Review Done"
                            ElseIf dv_ActivityStatus(0)("Status").ToString() = "SRD" Then
                                imgbtn.SkinID = "ImgBtnSecondReviewed"
                                imgbtn.ToolTip = "Second Review Done"
                            ElseIf dv_ActivityStatus(0)("Status").ToString() = "RL" Then
                                imgbtn.SkinID = "ImgBtnForLock"
                                imgbtn.ToolTip = "Reviewed & Freeze"
                            End If
                        End If

                        e.Row.Cells(cIndex).Controls.Add(imgbtn)

                        dv_DCFMst = dt_DCFMst.DefaultView
                        'change to have proper DCF numbers of amounts ' Pratiksha
                        'dv_DCFMst.RowFilter = "iPeriod = " + Period + " And iParentNodeId = " + NodeId(cIndex).ToString() _
                        '                                          + " And vSubjectId = '" + SubjectId + "'"
                        dv_DCFMst.RowFilter = "iPeriod = " + Period + " And (iParentNodeId = " + NodeId(cIndex).ToString() + " Or iNodeId = " + NodeId(cIndex).ToString() _
                                                                  + ") And vSubjectId = '" + SubjectId + "'"
                        dt_DCFMst = dv_DCFMst.ToTable()

                        dv_DCFMst = dt_DCFMst.DefaultView
                        dv_DCFMst.RowFilter = "cDCFStatus = '" + Discrepancy_Generated + "'"
                        Pending = dv_DCFMst.ToTable().Rows.Count

                        dv_DCFMst = dt_DCFMst.DefaultView
                        dv_DCFMst.RowFilter = "cDCFStatus = '" + Discrepancy_AutoResolved + "'"
                        AutoResolved = dv_DCFMst.ToTable().Rows.Count

                        dv_DCFMst = dt_DCFMst.DefaultView
                        dv_DCFMst.RowFilter = "cDCFStatus = '" + Discrepancy_Resolved + "'"
                        Resolved = dv_DCFMst.ToTable().Rows.Count

                        dv_DCFMst = dt_DCFMst.DefaultView
                        dv_DCFMst.RowFilter = "cDCFStatus = '" + Discrepancy_Answered + "'"
                        Answered = dv_DCFMst.ToTable().Rows.Count

                        dv_DCFMst = dt_DCFMst.DefaultView 'For tooltip of internaly resolved
                        dv_DCFMst.RowFilter = "cDCFStatus = '" + Discrepancy_InternallyResolved + "'"
                        InternallyResolved = dv_DCFMst.ToTable().Rows.Count

                        str = ""
                        str = "Pending:" + Pending.ToString()
                        str += ", Answered:" + Answered.ToString()
                        str += ", Resolved:" + Resolved.ToString()
                        str += ", Auto Resolved:" + AutoResolved.ToString()
                        str += ", Internally Resolved:" + InternallyResolved.ToString()

                        e.Row.Cells(cIndex).Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                        e.Row.Cells(cIndex).Font.Bold = True
                        e.Row.Cells(cIndex).ForeColor = Drawing.Color.Red
                        e.Row.Cells(cIndex).Controls.Add(New LiteralControl(Pending.ToString()))
                        e.Row.Cells(cIndex).ToolTip = str

                    End If

                Next cIndex

                ''Dv_WorkSpaceSubjectMst = CType(Me.ViewState(Vs_DtWorkSpaceSubjectMst), DataTable).Copy.DefaultView
                Dv_WorkSpaceSubjectMst = CType(Me.Session(Vs_DtWorkSpaceSubjectMst), DataTable).Copy.DefaultView
                Dv_WorkSpaceSubjectMst.RowFilter = "vSubjectId='" & e.Row.Cells(GVC_SubjectId).Text.ToString() & "'"
                Dt_WorkSpaceSubjectMst = Dv_WorkSpaceSubjectMst.ToTable()

                If Not Dt_WorkSpaceSubjectMst Is Nothing And Dt_WorkSpaceSubjectMst.Rows.Count > 0 Then

                    If Dt_WorkSpaceSubjectMst.Rows(0).Item("cRejectionFlag") = "Y" Then

                        e.Row.Cells(GVC_Initials).BackColor = Drawing.Color.Red
                        e.Row.Cells(GVC_Initials).ForeColor = Drawing.Color.White

                        e.Row.Cells(GVC_Site).BackColor = Drawing.Color.Red
                        e.Row.Cells(GVC_Site).ForeColor = Drawing.Color.White

                        e.Row.Cells(GVC_ScreenNo).BackColor = Drawing.Color.Red
                        e.Row.Cells(GVC_ScreenNo).ForeColor = Drawing.Color.White

                        e.Row.Cells(GVC_SubjectId).BackColor = Drawing.Color.Red
                        e.Row.Cells(GVC_SubjectId).ForeColor = Drawing.Color.White

                    End If

                End If

            End If
            TrLegends.Visible = True
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then  ''Added by Aaditya for not to show legend in CT Scope
                Me.TdRejectSubjects.Style.Add("display", "none")
            End If
          
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....gvwSubjectSelectionForVisit_RowDataBound")
        Finally
            dt_ActivityStatus.Dispose()
            dt_DCFMst.Dispose()
            dt_Repeatation.Dispose()
            dt_WorkspaceNodeDetail.Dispose()
            Dt_WorkSpaceSubjectMst.Dispose()

            dv_ActivityStatus.Dispose()
            dv_DCFMst.Dispose()
            dv_Repeatation.Dispose()
            dv_WorkspaceNodeDetail.Dispose()
            Dv_WorkSpaceSubjectMst.Dispose()

            Ds_WorkSpaceSubjectMst.Dispose()
        End Try


    End Sub

#End Region

#Region "Helper Functions/Subs"

    Private Function GetRecordsCounts() As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim dsCount As New DataSet
        Dim Param As String = String.Empty
        Dim ds_CRFVersion As New DataSet
        Dim VersionDate As New Date

        Try

            wStr = "vWorkSpaceId='" + Me.HProjectId.Value + "'"
            If Not objHelp.GetData("View_CRFVersionForDataEntryControl", "*", wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CRFVersion, eStr) Then
                Throw New Exception("Error While Gettin CRFVersion")
            End If
            If ds_CRFVersion.Tables(0).Rows.Count > 0 Then
                Me.trCRFVersion.Style.Add("display", "")
                Me.lblCRFVersion.Text = ds_CRFVersion.Tables(0).Rows(0)("nVersionNo").ToString()
                VersionDate = ds_CRFVersion.Tables(0).Rows(0)("dVersiondate")
                Me.lblCRFDate.Text = VersionDate.ToString("dd-MMM-yyyy")
                If ds_CRFVersion.Tables(0).Rows(0)("cFreezeStatus") = "U" Then
                    Me.imgCRFStatus.ImageUrl = "~/images/UnFreeze.jpg"
                End If
            Else
                Me.trCRFVersion.Style(HtmlTextWriterStyle.Display) = "none"
            End If


            If Me.HSubject.Value.ToString.Trim() <> "" Then
                Me.ViewState(VS_TotalRowCount) = "1"
                Return True
            End If

            If Me.HProjectId.Value.ToString.Trim() = "" Then
                Return True
            End If

            If Me.ddlPeriods.SelectedItem Is Nothing Then
                Return True
            End If

            Param = Me.HProjectId.Value.Trim() + "##" + Me.ddlPeriods.SelectedItem.Text.Trim() + "##" + Me.HProjectId.Value.Trim() + "##"

            If Not objHelp.Proc_WorkspaceActivitySubjectMatrix_Count(Param, dsCount, eStr) Then
                Throw New Exception("Error Retrieving Total Number of Cases")
            End If

            If dsCount.Tables.Count > 0 Then     ''Added by ketan for Condition check datatable
                Me.ViewState(VS_TotalRowCount) = Convert.ToString(dsCount.Tables(0).Rows(0)(0))
            End If

            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....GetRecordsCounts")
            Return False
        Finally
            dsCount.Dispose()
        End Try

    End Function

    Private Sub SetPaging()
        Dim totalPages As Integer = 0
        Dim startIndex As Integer = 1
        Dim count As Integer = 1
        Try

            Me.phTopPager.Controls.Clear()
            Me.phBottomPager.Controls.Clear()

            If Not Me.ViewState(VS_TotalRowCount) Is Nothing AndAlso Integer.Parse(Me.ViewState(VS_TotalRowCount)) > 0 Then
                totalPages = Math.Ceiling(Me.ViewState(VS_TotalRowCount) / PAGESIZE)
            End If
            startIndex = Me.ViewState(VS_PagerStartPage)
            If Me.ViewState(VS_PagerStartPage) Is Nothing Then
                startIndex = 1
                Me.ViewState(VS_PagerStartPage) = 1
            End If

            If totalPages > 1 Then

                For index As Integer = startIndex To totalPages
                    Me.phTopPager.Visible = True
                    Me.phBottomPager.Visible = True
                    Dim lnkButton As New LinkButton()
                    If startIndex > 1 And count = 1 Then
                        'This is for first Page <<
                        lnkButton = Me.AddLnkButton("BtnFirstPage", "BtnFirstPage", 1.ToString, "<<")

                        'This is for ellipse ...
                        lnkButton = Me.AddLnkButton("BtnEllipsePrev", "BtnEllipsePrev", (index - 1).ToString(), "...")
                    End If

                    'This is for Numeric Buttons
                    If index = Me.ViewState(VS_CurrentPage) Then
                        lnkButton = Me.AddLnkButton(index.ToString, "BtnPager", index, index, False)
                    Else
                        lnkButton = Me.AddLnkButton(index.ToString, "BtnPager", index, index)
                    End If

                    If count = 10 Then
                        If index < totalPages Then
                            'This is for ellipses ...
                            Me.AddLnkButton("BtnEllipseNext", "BtnEllipseNext", (index + 1).ToString, "...")

                            'This is for Last Page >>
                            Me.AddLnkButton("BtnLastPage", "BtnLastPage", totalPages.ToString, ">>")
                        End If
                        Exit For
                    End If

                    count = count + 1
                Next
            Else
                Me.phTopPager.Visible = False
                Me.phBottomPager.Visible = False
            End If
            'End If
        Catch ex As Exception

        End Try
    End Sub

    Private Function AddLnkButton(ByVal ID_1 As String, ByVal CommandName_1 As String, _
                                  ByVal CommandArg_1 As String, ByVal Text_1 As String, _
                                  Optional ByVal IsEnablePostBack As Boolean = True) As LinkButton
        Dim lnkButton As New LinkButton
        Dim lnkButtonBottom As New LinkButton
        Dim ltr As Literal
        Dim ltrBottom As Literal
        lnkButton = New LinkButton()
        ltr = New Literal()
        lnkButtonBottom = New LinkButton()
        ltrBottom = New Literal()
        lnkButton.ID = "Top" + ID_1
        lnkButton.CommandName = CommandName_1
        lnkButton.CommandArgument = CommandArg_1
        lnkButton.Text = Text_1
        lnkButton.CssClass = "PagerLinks"
        AddHandler lnkButton.Click, AddressOf PagerButton_Click
        If Not IsEnablePostBack Then
            lnkButton.OnClientClick = "return false;"
            lnkButton.Font.Underline = False
        End If

        lnkButtonBottom.ID = "Bottom" + ID_1
        lnkButtonBottom.CommandName = CommandName_1
        lnkButtonBottom.CommandArgument = CommandArg_1
        lnkButtonBottom.Text = Text_1
        lnkButtonBottom.CssClass = "PagerLinks"
        AddHandler lnkButtonBottom.Click, AddressOf PagerButton_Click
        If Not IsEnablePostBack Then
            lnkButtonBottom.OnClientClick = "return false;"
            lnkButtonBottom.Font.Underline = False
        End If

        Me.phTopPager.Controls.Add(lnkButton)
        Me.phBottomPager.Controls.Add(lnkButtonBottom)
        ltr.Text = "&nbsp;"
        ltrBottom.Text = "&nbsp;"
        Me.phTopPager.Controls.Add(ltr)
        Me.phBottomPager.Controls.Add(ltrBottom)
        Return lnkButton
    End Function

    Protected Sub PagerButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lnkButton As LinkButton
        Dim totalPages As Integer = 1
        totalPages = Me.GetTotalPages()
        lnkButton = CType(sender, LinkButton)
        Me.ViewState(VS_CurrentPage) = lnkButton.CommandArgument

        If lnkButton.CommandName.ToUpper = "BtnEllipseNext".ToUpper.ToString Then
            Me.ViewState(VS_PagerStartPage) = lnkButton.CommandArgument
            If (Integer.Parse(totalPages) - Integer.Parse(lnkButton.CommandArgument)) < 9 Then
                Me.ViewState(VS_PagerStartPage) = (Integer.Parse(totalPages) - 9)
            End If
        ElseIf lnkButton.CommandName.ToUpper = "BtnEllipsePrev".ToUpper.ToString Or _
                lnkButton.CommandName.ToUpper = "BtnLastPage".ToUpper.ToString Then
            Me.ViewState(VS_PagerStartPage) = Integer.Parse(lnkButton.CommandArgument) - 9
            If (Integer.Parse(lnkButton.CommandArgument) - 10) < 1 Then
                Me.ViewState(VS_PagerStartPage) = 1
            End If
        ElseIf lnkButton.CommandName.ToUpper = "BtnFirstPage".ToUpper.ToString Then
            Me.ViewState(VS_PagerStartPage) = 1
        End If

        If Not Me.FillGrid() Then
            Exit Sub
        End If

    End Sub

    Private Function GetTotalPages() As Integer
        GetTotalPages = 1
        If Not Me.ViewState(VS_TotalRowCount) Is Nothing Then
            GetTotalPages = Me.ViewState(VS_TotalRowCount)
        End If

        If GetTotalPages > PAGESIZE Then
            GetTotalPages = Math.Ceiling(Double.Parse(Me.ViewState(VS_TotalRowCount)) / PAGESIZE)
        End If
    End Function

    Private Sub ClearControls()
        gvwSubjectSelectionForVisit.DataSource = Nothing
        gvwSubjectSelectionForVisit.DataBind()
        'Add by shivani pandya for fix column hide issue
        If gvwSubjectSelectionForVisit.Rows.Count > 0 Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getDatatable", "getDatatable();", True)
        End If
        Me.ViewState(VS_PagerStartPage) = Nothing
        Me.ViewState(VS_CurrentPage) = Nothing
        Me.ViewState(VS_TotalRowCount) = Nothing
        Me.phBottomPager.Controls.Clear()
        Me.phTopPager.Controls.Clear()
        Me.HSubject.Value = ""
        Me.txtSubject.Text = ""
        Me.Session(VS_DtActivityStatus) = Nothing
        Me.Session(VS_DtDCFMst) = Nothing
        Me.Session(VS_DtWorkspaceNodeDetail) = Nothing
        Me.Session(Vs_DtWorkSpaceSubjectMst) = Nothing
        'add by shivani pandya for dynamic reivew
        Me.Session(Vs_dsReviewerlevel) = Nothing
    End Sub

    'Added by ketan 
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

            ElseIf CType(Me.ViewState(VS_Mode), String).ToUpper() = "EDIT" Then

                wStr = "vMySubjectNo = '" + Me.txtScreenNo.Text.Trim() + "'"
                wStr += " And vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
                wStr += " And vWorkspaceSubjectId <> '" + hdn_vWorkspaceSubjectId.Value.Trim() + "'"

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
                wStr += " And vWorkspaceSubjectId <> '" + hdn_vWorkspaceSubjectId.Value.Trim() + "'"

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
            If CType(Me.ViewState(VS_Mode), String).ToUpper() = "EDIT" Then
                Dt_WorkspaceSubjectMst = CType(Me.ViewState(VS_WorkspaceSubjectMst), DataTable)

                For Each dr In Dt_WorkspaceSubjectMst.Rows
                    dr("vInitials") = Me.txtInitial.Text.Trim.ToUpper()
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("vMySubjectNo") = Me.txtScreenNo.Text.Trim().ToUpper()
                    dr("iTranNo") = 0
                    dr("nWorkspaceSubjectHistoryId") = 0
                    dr("dICFDate") = System.DateTime.Now
                    dr("cRandomizationType") = Me.hdnRandomizationType.Value()
                    dr("iMySubjectNo") = HReplaceImySubjectNo.Value

                    If (Me.txtRandomizationRemarks.Text <> "") Then
                        dr("vRandomizationNo") = Me.txtPatientRandomizationNo.Text.Trim().ToUpper()
                        dr("cRandomizationType") = Me.hdnRandomizationType.Value()
                        dr("vRemarks") = IIf(Me.txtRandomizationRemarks.Text.Trim() <> "", Me.txtRandomizationRemarks.Text.Trim().ToUpper(), "")
                    ElseIf (Me.txtRemarks.Text <> "") Then
                        dr("vRandomizationNo") = Me.txtPatientRandomizationNo.Text.Trim().ToUpper()
                        dr("cRandomizationType") = ""
                        dr("vRemarks") = IIf(Me.txtRemarks.Text.Trim() <> "", Me.txtRemarks.Text.Trim().ToUpper(), "")
                    End If

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
    'REset page
    Private Sub ResetPage()
        Me.txtInitial.Text = ""
        Me.txtLastName.Text = ""
        Me.txtMiddleName.Text = ""
        Me.txtScreenNo.Text = ""
        Me.txtICFSignedDate.Text = ""
        Me.txtFirstName.Text = ""
        Me.txtRemarks.Text = ""
        '  Me.trRemarks.Style.Add("display", "none")
        Me.txtPatientRandomizationNo.Text = ""
        '  Me.trPatientRandomizationNo.Style.Add("display", "none")          'For


    End Sub

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

            End If

            dtOld.AcceptChanges()
            Me.ViewState(VS_DtSubjectMst) = dtOld

            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....AssignValues")
            Return False
        End Try
    End Function

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
    'Ended by ketan

#End Region

#Region "Button Events"

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.HProjectId.Value = ""
        Me.HSubject.Value = ""
        Me.txtproject.Text = ""
        Me.txtSubject.Text = ""
        Me.ddlPeriods.Items.Clear()
        Me.gvwSubjectSelectionForVisit.DataSource = Nothing
        Me.gvwSubjectSelectionForVisit.DataBind()
        'Add by shivani pandya for fix column hide issue
        If gvwSubjectSelectionForVisit.Rows.Count > 0 Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getDatatable", "getDatatable();", True)
        End If
        Me.ViewState(VS_PagerStartPage) = Nothing
        Me.ViewState(VS_CurrentPage) = Nothing
        Me.ViewState(VS_TotalRowCount) = Nothing
        Me.phBottomPager.Controls.Clear()
        Me.phTopPager.Controls.Clear()
        Me.trCRFVersion.Style(HtmlTextWriterStyle.Display) = "none"
        TrLegends.Visible = False

    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub


    ' ===== Added by Jeet Pate on 20-April-2015 to show CRF details ========
    Protected Sub btnSetSubject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetSubject.Click
        Dim wstr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_CRFVersion As Data.DataSet = Nothing
        Dim VersionDate As New Date

        Try
            wstr = "vWorkSpaceId='" + Me.HProjectId.Value + "'"
            If Not objHelp.GetData("View_CRFVersionForDataEntryControl", "*", wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CRFVersion, eStr) Then
                Throw New Exception("Error While Gettin CRFVersion")
            End If
            If ds_CRFVersion.Tables(0).Rows.Count > 0 Then
                Me.trCRFVersion.Style.Add("display", "")
                Me.lblCRFVersion.Text = ds_CRFVersion.Tables(0).Rows(0)("nVersionNo").ToString()
                VersionDate = ds_CRFVersion.Tables(0).Rows(0)("dVersiondate")
                Me.lblCRFDate.Text = VersionDate.ToString("dd-MMM-yyyy")
                If ds_CRFVersion.Tables(0).Rows(0)("cFreezeStatus") = "U" Then
                    Me.imgCRFStatus.ImageUrl = "~/images/UnFreeze.jpg"
                End If
            Else
                Me.trCRFVersion.Style(HtmlTextWriterStyle.Display) = "none"
            End If

        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)
        Finally
            ds_CRFVersion.Dispose()
        End Try

    End Sub
    '=======================================================================

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_CRFVersion As Data.DataSet = Nothing
        Dim ds_Check As New DataSet
        Dim dv_Check As New DataView
        Dim VersionDate As New Date

        Try

            Me.AutoCompleteExtenderSubject.ContextKey = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'"
            wStr = "vWorkSpaceId='" + Me.HProjectId.Value + "'"
            If Not objHelp.getworkspacemst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CRFVersion, eStr) Then
                Throw New Exception("Error While Gettin projectTypeMst")
            End If
            If Not ds_CRFVersion Is Nothing AndAlso Not ds_CRFVersion.Tables(0) Is Nothing AndAlso ds_CRFVersion.Tables(0).Rows.Count > 0 Then
                hfProjectTypeCode.Value = ds_CRFVersion.Tables(0).Rows(0)("vProjectTypeCode").ToString
            End If
            ds_CRFVersion = New DataSet
            ''==== added by Megha on 24-07-2012 CRFVersion========================================
            wStr = "vWorkSpaceId='" + Me.HProjectId.Value + "'"
            If Not objHelp.GetData("View_CRFVersionForDataEntryControl", "*", wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CRFVersion, eStr) Then
                Throw New Exception("Error While Gettin CRFVersion")
            End If
            ' ''==== added by Megha on 24-07-2012 CRFVersion========================================
            'wStr = "vWorkSpaceId='" + Me.HProjectId.Value + "'"
            'If Not objHelp.GetData("View_CRFVersionForDataEntryControl", "*", wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CRFVersion, eStr) Then
            '    Throw New Exception("Error While Gettin CRFVersion")
            'End If
            If ds_CRFVersion.Tables(0).Rows.Count > 0 Then
                Me.trCRFVersion.Style.Add("display", "")
                Me.lblCRFVersion.Text = ds_CRFVersion.Tables(0).Rows(0)("nVersionNo").ToString()
                VersionDate = ds_CRFVersion.Tables(0).Rows(0)("dVersiondate")
                Me.lblCRFDate.Text = VersionDate.ToString("dd-MMM-yyyy")
                If ds_CRFVersion.Tables(0).Rows(0)("cFreezeStatus") = "U" Then
                    Me.imgCRFStatus.ImageUrl = "~/images/UnFreeze.jpg"
                End If
            Else
                Me.trCRFVersion.Style(HtmlTextWriterStyle.Display) = "none"
            End If
            ''==========================================================
            wStr = String.Empty
            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"

            If Not Me.objHelp.GetCRFLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_Check, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_Check Is Nothing Then

                dv_Check = ds_Check.Tables(0).DefaultView
                dv_Check.Sort = "iTranNo desc"
                ViewState(vs_gLock) = dv_Check.ToTable()
                Me.ViewState(VS_Mode) = ""
                If dv_Check.ToTable().Rows.Count > 0 Then
                    'edited by vishal for site lock /unlock
                    If Convert.ToString(dv_Check.ToTable().Rows(0)("cLockFlag")).Trim.ToUpper() = "L" Then
                        Me.ViewState(VS_Mode) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View
                        Me.objcommon.ShowAlert("Project is Locked.", Me.Page())
                    End If
                End If
            End If

            If Not Me.FillDropDownListPeriods() Then
                Throw New Exception()
            End If

            SetLocation() ''Added by ketan
            Me.ClearControls()

        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)
        Finally
            ds_Check.Dispose()
            ds_CRFVersion.Dispose()
        End Try

    End Sub



    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
        Dim eStr As String = String.Empty

        If Me.txtproject.Text.Trim() = "" Then
            Me.objcommon.ShowAlert("Please Enter Project No.", Me.Page)
            Exit Sub
        End If


        If Not GenCall_Data() Then
            Exit Sub
        End If
        If Me.ViewState(VS_PagerStartPage) Is Nothing OrElse Me.ViewState(VS_CurrentPage) Is Nothing Then
            Me.ViewState(VS_PagerStartPage) = "1"
            Me.ViewState(VS_CurrentPage) = "1"
        End If

        If Not GetRecordsCounts() Then
            Exit Sub
        End If

        'Add by shivani pandya for dynamic reivew
        If Not GetLegends() Then
            Exit Sub
        End If

        If Not FillGrid() Then
            Exit Sub
        End If
        TrLegends.Visible = True
        If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then  ''Added by Aaditya for not to show legend in CT Scope
            Me.TdRejectSubjects.Style.Add("display", "none")
        End If

    End Sub

    Protected Sub btnAuthenticate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAuthenticate.Click, btnDirectAuthenticate.Click
        'Dim ds_Dcf As New DataSet
        Dim ds_CRFDtl As New DataSet
        Dim ds_CRFWorkFlowDtl As New DataSet
        Dim dr_CRFDtl As DataRow
        Dim dr_CRFWorkFlowDtl As DataRow
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim Pwd As String = String.Empty
        Dim ds_Dcf As New DataSet
        Dim SubjectIds As String = String.Empty
        Dim chkReview As CheckBox
        Dim Index As Integer = 0
        Dim dv_CRFDtl As DataView
        Dim ds_Save As New DataSet
        Dim wstr_dcf As String = ""
        Dim dtDCF As New DataTable
        Dim dvDCF As DataView
        'Add by shivani pandya for dynamic review
        Dim ds_reviewer As New DataSet
        Dim dv_reviewer As DataView

        Try
            wStr = " & HProjectId.Value.ToUpper() & """
            'wStr = Me.HFWorkspaceId.Value.Trim() + "##" + Me.HFActivityId.Value.Trim() + "##" + Me.HFSubjectId.Value.Trim() + "##" + Me.HFPeriodId.Value.Trim() + "##" + Me.HFNodeId.Value.Trim() + "##" + hndRepetitionNo.Value.Trim()
            'ds_CRFSubjectHdrDtl = objHelp.ProcedureExecute("Proc_SelectedRepeatition", wStr)

            If CType(sender, Button).ID = "btnAuthenticate" Then
                Pwd = Me.txtPassword.Text.Trim()
                Pwd = objHelp.EncryptPassword(Pwd)

                If Pwd.ToUpper() <> CType(Me.Session(S_Password), String).ToUpper() Then
                    ' objcommon.ShowAlert("Password Authentication Fails.", Me.Page)
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "OpenForAuthentication();", True)
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv1", "SelectAll(this, 'gvwSubjectSelectionForVisit')", True)
                    Me.txtPassword.Focus()
                    If gvwSubjectSelectionForVisit.Rows.Count > 0 Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getDatatable", "getDatatable();", True)
                    End If
                    Exit Sub
                End If
            End If

            'Add by shivani pandya for dynamic review
            ds_reviewer = CType(Me.Session(Vs_dsReviewerlevel), DataSet)

            For Index = 0 To Me.gvwSubjectSelectionForVisit.Rows.Count - 1

                'chkReview = CType(Me.gvwSubjectSelectionForVisit.Rows(Index).FindControl("chkReview" + Index.ToString()), CheckBox)
                chkReview = CType(Me.gvwSubjectSelectionForVisit.Rows(Index).Cells(GVC_Review).FindControl("chkReview"), CheckBox)

                If Not chkReview Is Nothing AndAlso chkReview.Checked Then
                    If hdnSub.Value.Contains(CType(Me.gvwSubjectSelectionForVisit.Rows(Index).Cells(GVC_Review).FindControl("chkReview"), CheckBox).ClientID.ToString().Split("_")(3).Substring(4, 1)) Then
                        SubjectIds += "'" + Me.gvwSubjectSelectionForVisit.Rows(Index).Cells(GVC_SubjectId).Text.Trim() + "',"
                    End If

                End If

            Next Index

            If Convert.ToString(SubjectIds).Trim() = "" Then
                Me.objcommon.ShowAlert("Please Select Atleast One Subject For Review.", Me.Page)
                Exit Sub
            End If

            SubjectIds = SubjectIds.Substring(0, SubjectIds.LastIndexOf(","))

            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"

            '==============Added by nipun khant for dynamic review======================
            dv_reviewer = ds_reviewer.Tables(0).Copy.DefaultView
            dv_reviewer.RowFilter = "iReviewWorkflowStageId = " + Me.Session(S_WorkFlowStageId) + " and vUserTypeCode = '" + Me.Session(S_UserType) + "'"

            If dv_reviewer.ToTable().Rows.Count > 0 Then
                If Convert.ToString(dv_reviewer.ToTable().Rows(0)("iActualWorkflowStageId")) = WorkFlowStageId_FirstReview Then
                    wStr += " And cCRFDtlDataStatus = 'D' "
                Else
                    wStr += " And cCRFDtlDataStatus = 'E' "
                End If
            End If
            '===========================================================================

            '==============Commented by nipun khant for dynamic review==================
            'If Me.Session(S_WorkFlowStageId) = WorkFlowStageId_FirstReview Then
            '    wStr += " And cCRFDtlDataStatus = 'D' "
            'Else
            '    wStr += " And cCRFDtlDataStatus = 'E' "
            'End If
            '===========================================================================
            wStr += " And iWorkFlowStageId = " + (CType(dv_reviewer.ToTable().Rows(0)("iActualWorkflowStageId"), Integer) - 10).ToString()
            wStr += " And vSubjectId in(" + SubjectIds + ") "
            wStr += " And iPeriod = " + Me.ddlPeriods.SelectedItem.Text.Trim()

            If Not Me.objHelp.View_CRFHdrDtlSubDtl_Review(wStr, " nCRFDtlNo", ds_CRFDtl, eStr) Then
                Throw New Exception(eStr)
            End If

            wStr = " nCRFDtlNo in ( 0,"
            For Index = 0 To ds_CRFDtl.Tables(0).Rows.Count - 1
                wStr += Convert.ToString(ds_CRFDtl.Tables(0).Rows(Index)("nCRFDtlNo")).Trim() + ","
            Next

            wStr = wStr.Substring(0, wStr.LastIndexOf(",")) + ") "
            wStr += "  And (cDCFStatus = '" + Discrepancy_Generated + "' Or cDCFStatus = '" + Discrepancy_Answered + "')"
            '==============Added by nipun khant for dynamic review======================
            If dv_reviewer.ToTable().Rows.Count > 0 Then
                wStr += " And (((vUserTypeCode = '" + Convert.ToString(Me.Session(S_UserType)).Trim() _
                       + "' Or CRFWorkflowBy < " + Convert.ToString(dv_reviewer.ToTable.Rows(0)("iActualWorkflowStageId")) + ") And cDCFType = 'M') "
            End If
            '===========================================================================
            '==============Commented by nipun khant for dynamic review==================
            'wStr += " And (((vUserTypeCode = '" + Convert.ToString(Me.Session(S_UserType)).Trim() _
            '       + "' Or CRFWorkflowBy < " + Me.Session(S_WorkFlowStageId).ToString() + ") And cDCFType = 'M') "
            '===========================================================================
            wStr += " Or cDCFType = 'S') And CRFWorkflowBy <> " & WorkFlowStageId_OnlyView
            wstr_dcf = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D' and iperiod = " + Me.ddlPeriods.SelectedItem.Text.Trim()
            wstr_dcf += " AND vSubjectID IN (" + SubjectIds + ") "

            If Not Me.objHelp.View_DCFMst(wstr_dcf, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Dcf, eStr) Then
                Throw New Exception(eStr)
            End If

            dtDCF = ds_Dcf.Tables(0)
            'dtDCF = CType(Me.Session(VS_DtDCFMst), DataTable)
            dvDCF = dtDCF.DefaultView
            dvDCF.RowFilter = wStr

            dtDCF = Nothing
            dtDCF = dvDCF.ToTable()

            dv_CRFDtl = ds_CRFDtl.Tables(0).DefaultView()
            If dtDCF.Rows.Count > 0 Then
                wStr = " nCRFDtlNo not in (0,"
                For Index = 0 To dtDCF.Rows.Count - 1
                    wStr += Convert.ToString(dtDCF.Rows(Index)("nCRFDtlNo")).Trim() + ","
                Next
                wStr = wStr.Substring(0, wStr.LastIndexOf(",")) + ") "

                dv_CRFDtl.RowFilter = wStr
            End If

            wStr = " nCRFDtlNo in ( 0,"
            For Index = 0 To dv_CRFDtl.ToTable.Rows.Count - 1
                wStr += Convert.ToString(dv_CRFDtl.ToTable.Rows(Index)("nCRFDtlNo")).Trim() + ","
            Next
            wStr = wStr.Substring(0, wStr.LastIndexOf(",")) + ") "

            If Not Me.objHelp.GetCRFDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_CRFDtl, eStr) Then
                Throw New Exception(eStr)
            End If

            ds_Save.Tables.Add(ds_CRFDtl.Tables(0).Copy())
            ds_Save.Tables(0).TableName = "CRFDtl"
            ds_Save.AcceptChanges()

            If Not Me.objHelp.GetCRFWorkFlowDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                ds_CRFWorkFlowDtl, eStr) Then
                Throw New Exception(eStr)
            End If

            'ds_Save.Tables(0).Columns("cStatusIndiDtl").ColumnName = "cStatusIndi"
            'ds_Save.Tables(0).Columns("cCRFDtlDataStatus").ColumnName = "cDataStatus"
            'ds_Save.Tables(0).Columns("iModifyByDtl").ColumnName = "iModifyBy"

            For Each dr_CRFDtl In ds_Save.Tables(0).Rows

                dr_CRFDtl("iWorkFlowStageId") = Me.Session(S_WorkFlowStageId)
                'dr_CRFDtl("iModifyBy") = Me.Session(S_UserID)
                'dr_CRFDtl("cStatusIndi") = "E"
                dr_CRFDtl("cDataStatus") = "E"
                dr_CRFDtl("dModifyon") = DateTime.Now()
                '==============Added by nipun khant for dynamic review==================
                If dv_reviewer.ToTable().Rows.Count > 0 Then
                    If Convert.ToString(dv_reviewer.ToTable.Rows(0)("vStatus")).ToUpper() = "L" Then
                        dr_CRFDtl("cDataStatus") = "F"
                    End If
                End If
                '===========================================================================
                '==============Commented by nipun khant for dynamic review==================
                'If Me.Session(S_WorkFlowStageId) = WorkFlowStageId_FinalReviewAndLock Then
                '    dr_CRFDtl("cDataStatus") = "F"
                'End If
                '===========================================================================
                dr_CRFWorkFlowDtl = ds_CRFWorkFlowDtl.Tables(0).NewRow()
                'nCRFWorkFlowNo,nCRFDtlNo,iTranNo,iWorkFlowStageId,dModifyOn,iModifyBy,cStatusIndi
                dr_CRFWorkFlowDtl("nCRFWorkFlowNo") = 0
                dr_CRFWorkFlowDtl("nCRFDtlNo") = dr_CRFDtl("nCRFDtlNo")
                dr_CRFWorkFlowDtl("iTranNo") = 0
                dr_CRFWorkFlowDtl("iWorkFlowStageId") = Me.Session(S_WorkFlowStageId)
                dr_CRFWorkFlowDtl("iModifyBy") = Me.Session(S_UserID)
                dr_CRFWorkFlowDtl("cStatusIndi") = "N"
                dr_CRFWorkFlowDtl("cReviewFlag") = "A"
                ds_CRFWorkFlowDtl.Tables(0).Rows.Add(dr_CRFWorkFlowDtl)
                ds_CRFWorkFlowDtl.AcceptChanges()

            Next dr_CRFDtl

            ds_Save.AcceptChanges()
            If ds_Save.Tables(0).Rows.Count > 0 Then

                If Not Me.objLambda.Save_CRFDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                                ds_Save, Me.Session(S_UserID), eStr) Then
                    Throw New Exception(eStr)
                End If

                If Not Me.objLambda.Save_CRFWorkFlowDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                            ds_CRFWorkFlowDtl, Me.Session(S_UserID), eStr) Then
                    Throw New Exception(eStr)
                End If

                Me.objcommon.ShowAlert("Review Completed Successfully.", Me.Page)

            Else
                Me.objcommon.ShowAlert("No Data Available For Review.", Me.Page)
            End If

            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "DivAuthenticationHideShow('H');", True)
            Me.btnGo_Click(sender, e)

        Catch ex As Exception
            Me.ShowErrorMessage("Problem While Review Completed : ", ex.Message)
        Finally
            ds_CRFDtl.Dispose()
            ds_CRFWorkFlowDtl.Dispose()
            dtDCF.Dispose()
            ds_Save.Dispose()
        End Try

    End Sub

    'Adde By ketan for Edit popup
    Protected Sub BtnSaveSubjectMst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSaveSubjectMst.Click
        Dim Ds_Subjectmst As New DataSet
        Dim eStr As String = String.Empty
        Dim SubjectId As String = String.Empty
        Dim mode = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
        Dim Msg As String = String.Empty
        Dim wStr As String = String.Empty

        Dim ds_WorkSpaceSubjectMst As New DataSet
        Me.ViewState(VS_Mode) = "EDIT"
        Try

            If Not Me.ViewState(VS_Mode) Is Nothing Then
                If CType(Me.ViewState(VS_Mode), String).ToUpper() = "EDIT" Then
                    wStr = "vSubjectId = '" + Me.hdnSubjectID.Value.Trim() + "' And vWorkspaceId = '"
                    wStr += Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"
                ElseIf CType(Me.ViewState(VS_Mode), String).ToUpper() = "ADDDEMOGRAPHIC" Then
                    wStr = "vSubjectId = '" + Me.hdnSubjectID.Value.Trim() + "' And vWorkspaceId = '"
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
                                                                Ds_Subjectmst, eStr) Then
                Throw New Exception(eStr)
            End If

            If Ds_Subjectmst Is Nothing Then
                Throw New Exception(eStr)
            End If
            Me.ViewState(VS_WorkspaceSubjectMst) = Ds_Subjectmst.Tables(0)



            If CType(Me.ViewState(VS_Mode), String).ToUpper() = "EDIT" Then
                mode = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
            End If

            If Not AssignValues() Then
                Me.objcommon.ShowAlert("Error While Assigning Data", Me.Page)
                Exit Sub
            End If


            wStr = String.Empty
            Dim Ds_Check As New DataSet
            If CType(Me.ViewState(VS_Mode), String).ToUpper() = "ADD" Then

                wStr = "vInitials = '" + Me.txtInitial.Text.Trim() + "'"
                wStr += " And vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"

            ElseIf CType(Me.ViewState(VS_Mode), String).ToUpper() = "EDIT" Then

                wStr = "vInitials = '" + Me.txtInitial.Text.Trim() + "'"
                wStr += " And vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
                wStr += " And vWorkspaceSubjectId <> '" + hdn_vWorkspaceSubjectId.Value.Trim() + "'"

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
                    Me.btnGo_Click(sender, e)
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
        Dim Msg As String = String.Empty
        Dim mode = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
        Dim RandomizationNo As String = String.Empty

        Try
            If CType(Me.ViewState(VS_Mode), String).ToUpper() = "EDIT" Then
                mode = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
            End If

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

            Me.objcommon.ShowAlert(Msg, Me.Page)

            Me.ResetPage()

            Me.MpeSubjectMst.Hide()

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "......Btnhidden_Click")
        End Try

    End Sub

    Protected Sub btnSaveRandomizationNoSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveRandomizationNoSave.Click
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Show", "validationforRandomizationNO('" + Convert.ToString(Me.Session(S_FirstName)) + "');", True)

    End Sub

    Protected Sub btnSignAuthOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSignAuthOK.Click
        Dim Ds_Subjectmst As New DataSet
        Dim eStr As String = String.Empty
        Dim SubjectId As String = String.Empty
        Dim mode = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
        Dim Msg As String = String.Empty
        Dim wStr As String = String.Empty

        Dim ds_WorkSpaceSubjectMst As New DataSet
        Me.ViewState(VS_Mode) = "EDIT"
        Try

            If Not Me.ViewState(VS_Mode) Is Nothing Then
                If CType(Me.ViewState(VS_Mode), String).ToUpper() = "EDIT" Then
                    wStr = "vSubjectId = '" + Me.hdnSubjectID.Value.Trim() + "' And vWorkspaceId = '"
                    wStr += Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"
                ElseIf CType(Me.ViewState(VS_Mode), String).ToUpper() = "ADDDEMOGRAPHIC" Then
                    wStr = "vSubjectId = '" + Me.hdnSubjectID.Value.Trim() + "' And vWorkspaceId = '"
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
                                                                Ds_Subjectmst, eStr) Then
                Throw New Exception(eStr)
            End If

            If Ds_Subjectmst Is Nothing Then
                Throw New Exception(eStr)
            End If
            Me.ViewState(VS_WorkspaceSubjectMst) = Ds_Subjectmst.Tables(0)



            If CType(Me.ViewState(VS_Mode), String).ToUpper() = "EDIT" Then
                mode = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
            End If

            If Not AssignValues() Then
                Me.objcommon.ShowAlert("Error While Assigning Data", Me.Page)
                Exit Sub
            End If


            wStr = String.Empty
            Dim Ds_Check As New DataSet
            If CType(Me.ViewState(VS_Mode), String).ToUpper() = "ADD" Then

                wStr = "vInitials = '" + Me.txtInitial.Text.Trim() + "'"
                wStr += " And vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"

            ElseIf CType(Me.ViewState(VS_Mode), String).ToUpper() = "EDIT" Then

                wStr = "vInitials = '" + Me.txtInitial.Text.Trim() + "'"
                wStr += " And vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
                wStr += " And vWorkspaceSubjectId <> '" + hdn_vWorkspaceSubjectId.Value.Trim() + "'"

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
                    Me.btnGo_Click(sender, e)
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Show", "RandomizationNumberGeneration('" + lblRandomizationno.Text + "');", True)
                End If
            End If


        Catch ex As Exception
            ShowErrorMessage(ex.Message, "..........BtnSaveSubjectMst_Click")
        End Try

    End Sub
    'Endded by ketan

#End Region

#Region "Check Box All Event"

    Protected Sub chkAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAll.CheckedChanged
        If Not FillGrid() Then
            Exit Sub
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
    Public Shared Function ViewDocument(ByVal WorkSpaceId As String, ByVal Period As String) As String
        Dim wstr As String = String.Empty
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim FilePath As String = String.Empty
        Dim dsChildGrid As New DataSet
        Try
            wstr = WorkSpaceId.ToString() + "##" + Period.ToString()
            dsChildGrid = objHelp.ProcedureExecute("dbo.Proc_GetProjectFilePath", wstr)

            If Not dsChildGrid Is Nothing AndAlso dsChildGrid.Tables(0).Rows.Count > 0 Then
                FilePath = System.Configuration.ConfigurationManager.AppSettings("CRFUploadFilePath") + dsChildGrid.Tables(0).Rows(0)("vFilePath").ToString()
            End If

            Return FilePath

        Catch ex As Exception
            Throw New Exception("Error while ViewDocument()")
        End Try
    End Function

    <WebMethod> _
    Public Shared Function FillSubjectDetails(ByVal WorkspaceID As String, ByVal vSubjectId As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim strReturn As String = String.Empty
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim WorkSpaceSubjectMst As New DataSet
        Dim ds_SubjectMst As New DataSet
        Dim dtData As New DataTable
        Dim dt_WorkSpaceSubjectMst As New DataTable
        Dim drProject As DataRow
        Dim i As Integer = 1
        Dim dt As DataTable = Nothing

        Try
            wStr = "vWorkspaceId = '" + WorkspaceID + "' AND vSubjectId = '" + vSubjectId + "' And cStatusIndi <> 'D'"

            If Not objHelp.GetView_SubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                ds_SubjectMst, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not objHelp.GetWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                               WorkSpaceSubjectMst, eStr) Then
                Throw New Exception(eStr)
            End If


            Dim dv As New DataView(ds_SubjectMst.Tables(0))
            If Not dt_WorkSpaceSubjectMst Is Nothing Then
                dt_WorkSpaceSubjectMst.Columns.Add("vFirstName")
                dt_WorkSpaceSubjectMst.Columns.Add("vMiddleName")
                dt_WorkSpaceSubjectMst.Columns.Add("vSurName")
                dt_WorkSpaceSubjectMst.Columns.Add("vInitials")


                'Field of Workspacesubjectmaster 
                dt_WorkSpaceSubjectMst.Columns.Add("vMySubjectNo")
                dt_WorkSpaceSubjectMst.Columns.Add("dICFDate")
                dt_WorkSpaceSubjectMst.Columns.Add("vRandomizationNo")
                dt_WorkSpaceSubjectMst.Columns.Add("vWorkspaceSubjectId")
                dt_WorkSpaceSubjectMst.Columns.Add("iMySubjectNo")
                dt_WorkSpaceSubjectMst.Columns.Add("cRandomizationType")


            End If
            If dv.ToTable.Rows.Count > 0 Then

                drProject = dt_WorkSpaceSubjectMst.NewRow()
                For Each dr In ds_SubjectMst.Tables(0).Rows
                    drProject("vFirstName") = dr("vFirstName").ToString()
                    drProject("vMiddleName") = dr("vMiddleName").ToString()
                    drProject("vSurName") = dr("vSurName").ToString()
                    drProject("vInitials") = dr("vInitials").ToString()
                Next

                For Each dr In WorkSpaceSubjectMst.Tables(0).Rows
                    drProject("vMySubjectNo") = dr("vMySubjectNo").ToString()
                    drProject("cRandomizationType") = dr("cRandomizationType").ToString()

                    If Convert.ToString(dr("vRandomizationNo")).Trim() <> "" Then
                        drProject("vRandomizationNo") = dr("vRandomizationNo").ToString()
                    ElseIf Convert.ToString(HttpContext.Current.Session("S_Prefix")).Trim() = "Y" Then
                        drProject("vRandomizationNo") = Convert.ToString(HttpContext.Current.Session("S_ProjectNo").Split("[")(1).Split("]")(0) + "R").Trim().ToUpper()
                    End If

                    drProject("vWorkspaceSubjectId") = dr("vWorkspaceSubjectId").ToString()

                    If Convert.ToString(dr("dICFDate")) <> "" Then
                        drProject("dICFDate") = CType(Convert.ToString(dr("dICFDate")).Trim(), Date).ToString("dd-MMM-yyyy")
                    End If
                    drProject("iMySubjectNo") = dr("iMySubjectNo").ToString()

                Next
                dt_WorkSpaceSubjectMst.Rows.Add(drProject)

                dt_WorkSpaceSubjectMst.AcceptChanges()
            End If

            strReturn = JsonConvert.SerializeObject(dt_WorkSpaceSubjectMst)
            Return strReturn
        Catch ex As Exception
            Throw New Exception("Error while FillActivityGrid")
        End Try
    End Function
#End Region

#Region "Dynamic review"
    Private Function GetLegends() As Boolean
        Dim ds As New DataSet
        Dim dv As DataView
        Dim estr As String = ""

        Try
            Me.PhlReviewer.Controls.Clear()
            If Not Me.objHelp.Proc_GetLegends(Me.HProjectId.Value.ToString(), ds, estr) Then
                Throw New Exception("Error While Gettin Proc_getLegends" + estr)
            End If

            If ds Is Nothing Or ds.Tables.Count = 0 Then
                Return False
            End If

            Me.Session(Vs_dsReviewerlevel) = ds.Copy()

            If ds.Tables(0).Rows.Count > 0 Then
                Me.PhlReviewer.Controls.Add(New LiteralControl("<img src=""images/new.png"" runat=""server"" id=""img1"" alt=""New"" enableviewstate=""false"" />-Data Entry Pending,&nbsp;"))
                Me.PhlReviewer.Controls.Add(New LiteralControl("<img src=""images/continue.png"" runat=""server"" id=""img2"" alt=""Continue"" enableviewstate=""false"" />-Data Entry Continue,&nbsp;"))
                Me.PhlReviewer.Controls.Add(New LiteralControl("<img src=""images/review.png"" runat=""server"" id=""img3"" alt=""Review"" enableviewstate=""false"" />-Ready For Review,&nbsp;"))
                For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                    If i = ds.Tables(0).Rows.Count - 1 Then
                        Me.PhlReviewer.Controls.Add(New LiteralControl("<img src=""" + ds.Tables(0).Rows(i)("vImagePath").ToString() + """ runat=""server"" id=""img" + ds.Tables(0).Rows(i)("iActualWorkflowStageId").ToString() + """ alt=""" + ds.Tables(0).Rows(i)("vReviewerLevel").ToString() + """ enableviewstate=""false"" />-" + ds.Tables(0).Rows(i)("vReviewerLevel").ToString()))
                    Else
                        Me.PhlReviewer.Controls.Add(New LiteralControl("<img src=""" + ds.Tables(0).Rows(i)("vImagePath").ToString() + """ runat=""server"" id=""img" + ds.Tables(0).Rows(i)("iActualWorkflowStageId").ToString() + """ alt=""" + ds.Tables(0).Rows(i)("vReviewerLevel").ToString() + """ enableviewstate=""false"" />-" + ds.Tables(0).Rows(i)("vReviewerLevel").ToString() + ",&nbsp;"))
                    End If
                Next

            End If

            dv = ds.Tables(0).Copy.DefaultView
            dv.RowFilter = "iReviewWorkflowStageId = " + Me.Session(S_WorkFlowStageId).ToString()
            Me.lblSignername.Text = Me.Session(S_FirstName).ToString() + " " + Me.Session(S_LastName).ToString()
            Dim dt_Profiles As New DataTable
            dt_Profiles = CType(Me.Session(S_Profiles), DataTable)
            Dim dv_Profiles As DataView
            dv_Profiles = dt_Profiles.DefaultView
            dv_Profiles.RowFilter = "vUserTypeCode = '" + Me.Session(S_UserType).ToString() + "'"
            Me.lblSignerDesignation.Text = dv_Profiles.ToTable.Rows(0)("vUserTypeName").ToString()
            Me.lblSignRemarks.Text = "I attest to the accuracy and integrity of the data being reviewed."
            If dv.ToTable().Rows.Count = 1 Then
                If dv.ToTable().Rows(0)("cAuthenticationDialog").ToString().ToUpper() = "N" Then
                    Me.trName.Visible = False
                    Me.trDesignation.Visible = False
                    Me.trRemarks.Visible = False
                    Me.divAuthentication.Style.Add("height", "150px")
                    Me.hdnDirectAuthentication.Value = "true"

                ElseIf dv.ToTable().Rows(0)("cAuthenticationDialog").ToString().ToUpper() = "Y" Then
                    Me.trName.Visible = True
                    Me.trDesignation.Visible = True
                    Me.trRemarks.Visible = True
                    Me.divAuthentication.Style.Add("height", "150px")
                    Me.hdnDirectAuthentication.Value = "false"
                End If
            End If

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....GetLegends")
            Return False
        End Try
    End Function

#End Region

#Region "Authenticate Password"
    Private Function Auntheticate() As Boolean
        Dim pwd As String = String.Empty
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        pwd = Me.txtPassword.Text
        pwd = objHelp.EncryptPassword(pwd)

        If Convert.ToString(Me.Session(S_Password)) <> pwd.ToString() Then
            objcommon.ShowAlert("Password Authentication Fails.", Me.Page)
            Me.txtPassword.Focus()
            Return False
        End If
        Return True
    End Function

#End Region

    'Protected Sub Page_Unload(sender As Object, e As EventArgs) Handles Me.Unload
    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "FixGrid", "FixGrid();", True)
    'End Sub

    <Web.Services.WebMethod()> _
    Public Shared Function CheckValidationForDataEntry(ByVal WorkSpaceId As String, ByVal iNodeId As String, ByVal vSubjectId As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
        Dim strReturn As String = String.Empty
        Dim ds_Scheduled As DataSet
        Dim wStr As String

        Dim message As String = "Success"

        wStr = WorkSpaceId + "##" + iNodeId + "##" + vSubjectId + "##"

        ds_Scheduled = objHelp.ProcedureExecute("dbo.Proc_GetDataVisitTracker", wStr)

        Try
            If Not ds_Scheduled Is Nothing AndAlso ds_Scheduled.Tables(0).Rows.Count > 0 Then
                Dim dv As DataView = ds_Scheduled.Tables(0).DefaultView
                dv.RowFilter = "dActualDate IS NOT NULL AND  vSubjectId  = '" + vSubjectId + "'"
                If dv.Table().DefaultView.ToTable().Rows.Count > 0 Then

                Else
                    message = "failure"
                End If
            End If
            Return message
        Catch ex As Exception
            message = "failure"
            Throw New Exception(ex.Message)
        End Try

    End Function

End Class