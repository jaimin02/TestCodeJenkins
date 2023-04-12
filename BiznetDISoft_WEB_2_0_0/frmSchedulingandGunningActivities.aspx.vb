Imports System.Linq
Partial Class frmSchedulingandGunningActivities
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION"
    Private ObjCommon As New clsCommon
    Private ObjHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private ObjLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const VS_dtViewPKSampleDetail As String = "View_PKSampleDetail"
    Private Const VS_DtMedexInfoHdr As String = "DtMedexInfoHdr"
    Private Const VS_DtMedexInfoHdr1 As String = "DtMedexInfoHdr1"
    Private Const VS_DtViewWorkSpaceSubjectMst As String = "View_WorkSpaceSubjectMst"
    Private Const VS_DtView_MedExWorkSpaceDtl As String = "View_MedExWorkSpaceDtl"
    Private Const VS_DtView_WorkSpaceNodeDetail As String = "View_WorkSpaceNodeDetail"
    Private Const VS_DtCRFHdr As String = "DtCRFHdr"
    Private Const VS_DtCRFDtl As String = "DtCRFDtl"
    Private Const VS_DtCRFSubDtl As String = "DtCRFSubDtl"
    Private Const Vs_RefTime As String = "nRefTime"
    Private Const Vs_RefNodeId As String = "iRefNodeId"
    Private Const Vs_CurTime As String = "CurTime"
    Private Const VS_MySubjectNo As String = "MySubjectNo"
    Private Const VS_SubjectId As String = "SubjectId"
    Private Const VS_SubjectIdGunned As String = "SubjectIdGunned"
    Private Const vs_gLock As String = "glock"
    Private Const GVCSub_Select As Integer = 0
    Private Const GVCSub_WorkspaceId As Integer = 1
    Private Const GVCSub_MySubjectNo As Integer = 2
    Private Const GVCSub_vMySubjectNo As Integer = 3
    Private Const GVCSub_SubjectID As Integer = 4
    Private Const GVCSub_FullName As Integer = 5

    'Private Const GVC_nSampleId As Integer = 0
    'Private Const GVC_vSampleId As Integer = 1
    'Private Const GVC_MySubjectNo As Integer = 2
    'Private Const GVC_vMySubjectNo As Integer = 3
    'Private Const GVC_vSubjectId As Integer = 4
    'Private Const GVC_SubjectID As Integer = 5
    'Private Const GVC_FullName As Integer = 6
    'Private Const GVC_RefNodeId As Integer = 7
    'Private Const GVC_RefTime As Integer = 8
    'Private Const GVC_CollectionDateTime As Integer = 9
    'Private Const GVC_iCollectionBy As Integer = 10
    'Private Const GVC_vCollectionBy As Integer = 11
    'Private Const GVC_DosingTime As Integer = 12
    'Private Const GVC_Remarks As Integer = 13
    'Private Const GVC_AttendanceMysubNo As Integer = 14


    Private Const GVC_MySubjectNo As Integer = 0
    Private Const GVC_vMySubjectNo As Integer = 1

    Private Const GVC_SubjectID As Integer = 2

    Private Const GVC_RefNodeId As Integer = 3
    Private Const GVC_RefTime As Integer = 4
    Private Const GVC_SubjectID1 As Integer = 5
    Private Const GVC_IsDataEntryDone As Integer = 6
    Private Const GVC_DataStatus As Integer = 7
    Private Const GVC_ReplaceSubject As Integer = 8

    'Private Const GVC_AttendanceMysubNo As Integer = 5
#End Region

#Region "Page Events"
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsNothing(Session(S_UserID)) Then
                Response.Redirect("~/Default.aspx?SessionExpire=true", True)
            End If
            CType(Me.Master.FindControl("form1"), HtmlForm).DefaultButton = Me.btndefault.UniqueID
            Page.Title = ":: Scheduling and Gunning of Activities :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Scheduling and Gunning of Activities"

            If Not IsPostBack Then
                ViewState(VS_SubjectIdGunned) = ""
                If Not GenCall() Then
                    Exit Sub
                End If
                'Session(S_DynamicPage_URL) = Nothing
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

    Private Function GenCall() As Boolean
        Try



            If Not GenCall_Data() Then ' For Data Retrieval
                Exit Function
            End If

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall")
        End Try
    End Function

#Region "GenCall_Data"

    Private Function GenCall_Data() As Boolean

        Dim Ds As New DataSet
        Dim wStr As String = String.Empty
        Dim estr As String = String.Empty

        Try


            If Not Me.ObjHelp.GetCRFHdr("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, Ds, estr) Then
                Throw New Exception(estr)
            End If
            Me.ViewState(VS_DtCRFHdr) = Nothing
            If Not Ds Is Nothing Then
                Me.ViewState(VS_DtCRFHdr) = Ds.Tables(0)
            End If

            Ds = Nothing
            Ds = New DataSet
            If Not Me.ObjHelp.GetCRFDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, Ds, estr) Then
                Throw New Exception(estr)
            End If
            Me.ViewState(VS_DtCRFDtl) = Nothing
            If Not Ds Is Nothing Then
                Me.ViewState(VS_DtCRFDtl) = Ds.Tables(0)
            End If

            Ds = Nothing
            Ds = New DataSet
            If Not Me.ObjHelp.GetCRFSubDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, Ds, estr) Then
                Throw New Exception(estr)
            End If
            Me.ViewState(VS_DtCRFSubDtl) = Nothing
            If Not Ds Is Nothing Then
                Me.ViewState(VS_DtCRFSubDtl) = Ds.Tables(0)
            End If

            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_Data")
        End Try

    End Function

#End Region

#Region "GenCall_ShowUI()"

    Private Function GenCall_ShowUI() As Boolean

        Dim dt_MedExInfo As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
        Dim sender As New Object
        Dim e As EventArgs
        Try


            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
                GenCall_ShowUI = True

            End If
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)


            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_ShowUI")
        End Try

    End Function

#End Region
#Region "Fill Function"

    'Private Function FillGrid() As Boolean
    '    Dim ds_ViewPKSampleDetail As New Data.DataSet
    '    Dim estr As String = String.Empty
    '    Dim Wstr As String = String.Empty
    '    Dim Subjects As String = String.Empty
    '    Dim dtUserSubject As New Data.DataTable
    '    Dim dvUserSubject As New Data.DataView

    '    Try

    '        If IsNothing(Me.Session("UserSubjectDtl")) Then

    '            Me.ObjCommon.ShowAlert("Please, First Do Subject Management", Me.Page())
    '            Exit Function

    '        Else

    '            dvUserSubject = CType(Me.Session("UserSubjectDtl"), DataTable).Copy().DefaultView()
    '            dvUserSubject.RowFilter = "vWorkspaceId = '" + Me.HProjectId.Value + "' And iUserId=" + _
    '                                                 Me.Session(S_UserID)
    '            dtUserSubject = dvUserSubject.ToTable()

    '            If dtUserSubject.Rows.Count <= 0 Then
    '                Me.ObjCommon.ShowAlert("Please, First Do Subject Management", Me.Page())
    '                Exit Function
    '            End If

    '        End If

    '        Wstr = "vWorkspaceId = '" + Me.HProjectId.Value + "' And cStatusIndi <> 'D'"
    '        Wstr += " And nRefTime=" + Me.ViewState(Vs_RefTime).ToString
    '        Wstr += " And iRefNodeId = " + Me.ViewState(Vs_RefNodeId).ToString
    '        Wstr += " And iPeriod = " + _
    '                    Me.ddlPeriod.SelectedValue.Trim().ToString()

    '        dtUserSubject = CType(Me.Session("UserSubjectDtl"), DataTable)
    '        dvUserSubject = dtUserSubject.DefaultView
    '        dvUserSubject.RowFilter = "vWorkspaceId = '" + Me.HProjectId.Value + "'"
    '        dtUserSubject = dvUserSubject.ToTable()

    '        For Each dr As DataRow In dtUserSubject.Rows()
    '            Subjects += IIf(Subjects = "", "'" & dr("iMySubjectNo"), "','" & dr("iMySubjectNo"))
    '        Next

    '        Subjects += "'"

    '        Wstr += " And iMySubjectNo in (" & Subjects & ") order by iMySubjectNo,iNodeId"

    '        If Not ObjHelp.View_PKSampleDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                        ds_ViewPKSampleDetail, estr) Then
    '            Me.ShowErrorMessage("Error While Getting Data From View_PKSampleDetail : ", estr)
    '            Exit Function
    '        End If

    '        Me.ViewState(VS_dtViewPKSampleDetail) = ds_ViewPKSampleDetail.Tables(0)

    '        If ds_ViewPKSampleDetail.Tables(0).Rows.Count > 0 Then

    '            Me.txtScan.Enabled = True

    '            fillViewState()

    '            Me.gvwSubjectSample.DataSource = ds_ViewPKSampleDetail.Tables(0)
    '            Me.gvwSubjectSample.DataBind()

    '            Return True

    '        End If

    '        Me.gvwSubjectSample.DataSource = Nothing
    '        Me.gvwSubjectSample.DataBind()
    '        Me.ObjCommon.ShowAlert("Records Not Found.", Me.Page)

    '        Return True

    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.Message, "....FillGrid")
    '        Return False
    '    End Try
    'End Function

    Private Function fillViewState(ByVal sSubjectIds As String) As Boolean
        Dim Param As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_RefTime As New DataSet
        Dim strDate As String = String.Empty
        Dim Columns As String = String.Empty
        Dim dsWorkspaceSubjectMst As New DataSet
        Try

            'wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
            'wStr += " And iPeriod = " + Me.ddlPeriod.SelectedItem.Value.Trim()
            'wStr += " And vActivityId = " + Me.ddlActivity.SelectedValue.Split("##")(0)
            'wStr += " And iNodeId = " + Me.ddlActivity.SelectedValue.Split("##")(1)

            'Columns = "vWorkSpaceId,iPeriod,vActivityId,iNodeId,vSubjectId,vSubjectName,iMySubjectNo,nRefTIme,vMedExCode,vDefaultValue"

            'Param = Me.HProjectId.Value.Trim() + "##" + Me.ddlPeriod.SelectedItem.Value.Trim() + "##" + Me.ddlActivity.SelectedValue.Split("##")(0) + "##" + Me.ddlActivity.SelectedValue.Split("##")(1)
            If Not ObjHelp.Proc_GetDataForScheduling(Me.HProjectId.Value.Trim(), Me.ddlPeriod.SelectedItem.Value.Trim(), Me.ddlActivity.SelectedValue.Split("##")(0), Me.ddlActivity.SelectedValue.Split("##")(1), dsWorkspaceSubjectMst, eStr) Then
                Return False
            End If
            If dsWorkspaceSubjectMst.Tables(0).Rows.Count > 0 Then
                dsWorkspaceSubjectMst.Tables(0).DefaultView.RowFilter = "vSubjectID IN (" + sSubjectIds.Substring(0, sSubjectIds.Length - 1) + ")"
                Me.ViewState(VS_DtMedexInfoHdr) = dsWorkspaceSubjectMst.Tables(0).DefaultView.ToTable().Copy()
                dsWorkspaceSubjectMst.Tables(0).DefaultView.RowFilter = "vSubjectID IN (" + sSubjectIds.Substring(0, sSubjectIds.Length - 1) + ") and vMEdextype IN ('AsyncDateTime','AsyncTime')"
                If dsWorkspaceSubjectMst.Tables(0).DefaultView.Count > 0 Then
                    Me.ViewState(VS_DtMedexInfoHdr) = dsWorkspaceSubjectMst.Tables(0).DefaultView.ToTable.Copy()
                End If

                If Not ObjHelp.Proc_GetDataForScheduling(Me.HProjectId.Value.Trim(), Me.ddlPeriod.SelectedItem.Value.Trim(), CType(Me.ViewState(VS_DtMedexInfoHdr), DataTable).Rows(0)("Reference Activity"), CType(Me.ViewState(VS_DtMedexInfoHdr), DataTable).Rows(0)("iRefNodeId"), dsWorkspaceSubjectMst, eStr) Then
                    Return False
                End If
                dsWorkspaceSubjectMst.Tables(0).DefaultView.RowFilter = "vSubjectID IN (" + sSubjectIds.Substring(0, sSubjectIds.Length - 1) + ")"
                dsWorkspaceSubjectMst.Tables(0).DefaultView.Sort = "iSeqNo"
                Me.ViewState(VS_DtMedexInfoHdr1) = dsWorkspaceSubjectMst.Tables(0).DefaultView.ToTable().Copy()
                dsWorkspaceSubjectMst.Tables(0).DefaultView.RowFilter = "vSubjectID IN (" + sSubjectIds.Substring(0, sSubjectIds.Length - 1) + ") and vMEdextype IN ('AsyncDateTime','AsyncTime')" ' and iTranNo = MAX(iTranNo)"
                dsWorkspaceSubjectMst.Tables(0).DefaultView.Sort = "iSeqNo"


                If dsWorkspaceSubjectMst.Tables(0).DefaultView.Count > 0 Then
                    Me.ViewState(VS_DtMedexInfoHdr1) = dsWorkspaceSubjectMst.Tables(0).DefaultView.ToTable.Copy()
                End If
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...fillVie wState")
            Return False
        End Try

    End Function

    Private Function CreateSubjectTable(ByRef Dt As DataTable) As Boolean

        Dt = Nothing
        Dt = New DataTable()

        Dt.Columns.Add(New DataColumn("iUserId", GetType(Integer)))
        Dt.Columns.Add(New DataColumn("vWorkspaceId", GetType(String)))
        Dt.Columns.Add(New DataColumn("vSubjectId", GetType(String)))
        Dt.Columns.Add(New DataColumn("iMySubjectNo", GetType(String)))
        Dt.Columns.Add(New DataColumn("vMySubjectNo", GetType(String)))
        Dt.AcceptChanges()
        Return True

    End Function

    Private Function FillddlPeriod() As Boolean
        Dim ds_WorkSpaceNodeDetail As New Data.DataSet
        Dim dv_WorkSpaceNodeDetail As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim Param As String = String.Empty
        Try

            'wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' And cStatusIndi <> 'D' and nRefTime is not null"

            'If Not ObjHelp.GetViewWorkSpaceNodeDetail(wstr, ds_WorkSpaceNodeDetail, estr) Then
            '    Throw New Exception("Error While Getting Data from WorkSpaceNodeDetail : " + estr)
            'End If   Commented By Dipen Shah on 19-Feb-2015



            If Not ObjHelp.Proc_SchedulingGunningActivities(Me.HProjectId.Value.ToString.Trim(), Session(S_LocationCode), Session(S_DeptCode), Session(S_UserType), ds_WorkSpaceNodeDetail, estr) Then '' Added by Dipen Shah on 19 feb 2015 to get selected Acitivities.
                Throw New Exception(estr + "Error While Proc_SchedulingGunningActivities()")
            End If
            If Not ds_WorkSpaceNodeDetail Is Nothing AndAlso ds_WorkSpaceNodeDetail.Tables(0).Rows.Count > 0 Then
                Me.ViewState(VS_DtView_WorkSpaceNodeDetail) = ds_WorkSpaceNodeDetail.Tables(0).Copy()
            Else
                Me.ShowErrorMessage("No Data Found .", "")
                Return False
            End If


            dv_WorkSpaceNodeDetail = ds_WorkSpaceNodeDetail.Tables(0).DefaultView
            dv_WorkSpaceNodeDetail.Sort = "iPeriod"

            Me.ddlPeriod.DataSource = dv_WorkSpaceNodeDetail.ToTable().DefaultView.ToTable(True, "iPeriod".ToString())
            Me.ddlPeriod.DataValueField = "iPeriod"
            Me.ddlPeriod.DataTextField = "iPeriod"
            Me.ddlPeriod.DataBind()
            Me.ddlPeriod.Items.Insert(0, New ListItem("--Select Period--", 0))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillddlPeriod")
            Return False
        End Try
    End Function

    Private Function FillddlActivity() As Boolean
        Dim dt_Activity As New DataTable
        Dim dv_Activity As New DataView
        Dim estr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_WorkSpaceNodeDetail As New Data.DataSet
        Dim param As String = String.Empty
        Try



            dv_Activity = CType(Me.ViewState(VS_DtView_WorkSpaceNodeDetail), DataTable).Copy().DefaultView
            dv_Activity.RowFilter = "iPeriod = " + Me.ddlPeriod.SelectedItem.Value.Trim() + " and cSubjectWiseFlag = 'Y'"
            dv_Activity.Sort = "iNodeID,iNodeNo"
            dt_Activity = dv_Activity.ToTable()

            Me.ddlActivity.DataSource = dt_Activity.DefaultView.ToTable(True, "ActivityDisplayId,vNodeDisplayName".Split(","))
            Me.ddlActivity.DataValueField = "ActivityDisplayId"
            Me.ddlActivity.DataTextField = "vNodeDisplayName"
            Me.ddlActivity.DataBind()
            Me.ddlActivity.Items.Insert(0, New ListItem("--Select Activity--", 0))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillddlActivity")
            Return False
        End Try

    End Function

    Private Function FillSubjectGrid() As Boolean
        Dim ds_Subjects As New Data.DataSet
        Dim dtOldSubjects As New Data.DataTable
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty

        Try


            Wstr = "vWorkspaceId = '" + Me.HProjectId.Value + "' And cStatusIndi <> 'D' "
            Wstr += " And iPeriod = " + Me.ddlPeriod.SelectedItem.Value.Trim() + " AND ((cRejectionFlag <> 'Y') OR (cRejectionFlag = 'Y' AND left(iMySubjectNo,1) = 1))"
            Wstr += " order by vMySubjectNo"

            If Not ObjHelp.GetViewWorkspaceSubjectMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                       ds_Subjects, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From ViewWorkspaceSubjectMst : ", estr)
                Exit Function
            End If

            If ds_Subjects.Tables(0).Rows.Count > 0 Then

                Me.ViewState(VS_DtViewWorkSpaceSubjectMst) = ds_Subjects.Tables(0)
                Me.gvwSubjects.DataSource = ds_Subjects.Tables(0)
                Me.gvwSubjects.DataBind()

                If Not IsNothing(Me.Session("UserSubjectDtl")) Then

                    dtOldSubjects = CType(Me.Session("UserSubjectDtl"), DataTable)

                    For index As Integer = 0 To Me.gvwSubjects.Rows.Count - 1

                        For Each DrSubjects As DataRow In dtOldSubjects.Rows

                            If DrSubjects("iUserId") = Me.Session(S_UserID) AndAlso _
                                DrSubjects("vWorkspaceId") = Me.gvwSubjects.Rows(index).Cells(GVCSub_WorkspaceId).Text AndAlso _
                                DrSubjects("vSubjectId") = Me.gvwSubjects.Rows(index).Cells(GVCSub_SubjectID).Text Then

                                CType(Me.gvwSubjects.Rows(index).FindControl("ChkMove"), CheckBox).Checked = True

                                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "CheckSelected", "CheckSelected();", True)

                            End If

                        Next DrSubjects

                    Next index

                End If

                Return True
            End If

            Me.gvwSubjects.DataSource = Nothing
            Me.gvwSubjects.DataBind()
            Me.ObjCommon.ShowAlert("Records Not Found.", Me.Page)

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillSubjectGrid")
            Return False
        End Try
    End Function
#End Region

#Region "Button Events"
    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim ds_CrfVersionDetail As DataSet = Nothing
        Dim VersionNo As Integer = 0
        Dim VersionDate As Date
        Dim wstr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_Check As New DataSet
        Dim dv_Check As New DataView

        Dim eStr_Retu As String = String.Empty
        Try

            ''====== CRFVersion Control==================================

            wstr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'"

            If Not ObjHelp.GetData("View_CRFVersionForDataEntryControl", "*", wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CrfVersionDetail, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            If Not ds_CrfVersionDetail Is Nothing AndAlso ds_CrfVersionDetail.Tables(0).Rows.Count > 0 Then
                VersionNo = ds_CrfVersionDetail.Tables(0).Rows(0)("nVersionNo")
                VersionDate = ds_CrfVersionDetail.Tables(0).Rows(0)("dVersiondate").ToString

                If ds_CrfVersionDetail.Tables(0).Rows(0)("cFreezeStatus").ToString.Trim.ToUpper = "U" Then
                    ObjCommon.ShowAlert("Project Structure Is Not Freeze,To Do Data Entry Freeze It ", Me.Page)
                    Exit Sub
                End If
            End If
            wstr = String.Empty
            wstr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"

            If Not Me.ObjHelp.GetCRFLockDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_Check, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_Check Is Nothing Then

                dv_Check = ds_Check.Tables(0).DefaultView
                dv_Check.Sort = "iTranNo desc"
                ViewState(vs_gLock) = dv_Check.ToTable()
                If dv_Check.ToTable().Rows.Count > 0 Then
                    'edited by vishal for site lock /unlock
                    If Convert.ToString(dv_Check.ToTable().Rows(0)("cLockFlag")).Trim.ToUpper() = "L" Then
                        ObjCommon.ShowAlert("Project is Locked", Me.Page)
                        Exit Sub

                    End If

                End If

            End If

            If Not FillddlPeriod() Then
                Exit Sub
            End If
            btnSubjectMgmt.Enabled = False
            btnSearch.Enabled = False
            If Not ClearControls() Then
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnSubjectMgmt_Click(sender As Object, e As EventArgs) Handles btnSubjectMgmt.Click
        Try
            If Me.hfTextChnaged.Value.ToUpper.Trim() = "BARCODE" Then
                Me.hfTextChnaged.Value = ""
                Exit Sub
            End If
            iFrmDynamicPage.Visible = False
            If Not FillSubjectGrid() Then
                Exit Sub
            End If
            Me.MPESubjectManagement.Show()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....btnSubjectMgmt_Click")
        End Try
    End Sub

    Protected Sub btnSaveSubject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveSubject.Click
        Try
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                Me.btnOk_Click(sender, e)
                Exit Sub
            End If
            If Not ClearControls() Then
                Exit Sub
            End If
            If Not CheckSequence(Me.HProjectId.Value.Trim(), Me.ddlActivity.SelectedValue.Split("##")(1), Me.HsubjectId.Value, Me.ddlPeriod.SelectedValue) Then
                Me.MPEActivitySequence.Hide()
                Exit Sub
            End If
            If Me.HPendingNode.Value = "" Then
                Me.btnOk_Click(sender, e)
                Exit Sub
            End If
            Me.txtContent.Text = ""
            Me.MPEActivitySequence.Show()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......btnSaveSubject_Click")
        End Try
    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click, btnSearch.Click
        Dim DtSubjects As New DataTable
        Dim DrSubjects As DataRow
        Dim index As Integer = 0
        Dim sSubjectIds As String = String.Empty
        Try
            If IsNothing(Me.Session("UserSubjectDtl")) Then

                If Not CreateSubjectTable(DtSubjects) Then
                    Exit Sub
                End If

            Else

                DtSubjects = CType(Me.Session("UserSubjectDtl"), DataTable)
                If Not DtSubjects.Columns.Contains("vMySubjectNo") Then
                    DtSubjects.Columns.Add(New DataColumn("vMySubjectNo", GetType(String)))
                End If


            End If

            DtSubjects.Clear()

            For index = 0 To Me.gvwSubjects.Rows.Count - 1

                If CType(Me.gvwSubjects.Rows(index).FindControl("ChkMove"), CheckBox).Checked = True Then

                    DrSubjects = DtSubjects.NewRow
                    DrSubjects("iUserId") = Me.Session(S_UserID)
                    DrSubjects("vWorkspaceId") = Me.gvwSubjects.Rows(index).Cells(GVCSub_WorkspaceId).Text
                    DrSubjects("vSubjectId") = Me.gvwSubjects.Rows(index).Cells(GVCSub_SubjectID).Text
                    sSubjectIds += "'" + Me.gvwSubjects.Rows(index).Cells(GVCSub_SubjectID).Text + "',"
                    '' ******************************Changed On 21-May-2011 (Dharmesh Salla) ******************'
                    '=========================================================================================='

                    If Me.gvwSubjects.Rows(index).Cells(GVCSub_MySubjectNo).Text.Length > 4 Then
                        DrSubjects("iMySubjectNo") = Me.gvwSubjects.Rows(index).Cells(GVCSub_MySubjectNo).Text.ToString().Remove(4, (Me.gvwSubjects.Rows(index).Cells(GVCSub_MySubjectNo).Text.ToString().Length) - 4)
                    Else
                        DrSubjects("iMySubjectNo") = Me.gvwSubjects.Rows(index).Cells(GVCSub_MySubjectNo).Text.ToString()
                    End If
                    '=========================================================================================='
                    '' ***************************************************************************************** ''


                    DrSubjects("vMySubjectNo") = Me.gvwSubjects.Rows(index).Cells(GVCSub_vMySubjectNo).Text
                    DtSubjects.Rows.Add(DrSubjects)
                    DtSubjects.AcceptChanges()

                End If

            Next index


            If Not DtSubjects Is Nothing AndAlso DtSubjects.Rows.Count > 0 Then
                Me.txtScan.Enabled = True
                fillViewState(sSubjectIds)
                ViewState(VS_SubjectIdGunned) = ""
                Me.gvwSubjectSample.DataSource = CType(Me.ViewState(VS_DtMedexInfoHdr), DataTable).DefaultView.ToTable(True, "iMySubjectNo", "vMySubjectNo", "iRefNodeId", "Reference Time", "vSubjectId", "IsDataEntryDone", "DataStatus", "ReplaceSubject")
                'Me.gvwSubjectSample.DataSource = DtSubjects.Copy()
                Me.gvwSubjectSample.DataBind()
                canal.Visible = True
            End If


            If IsNothing(Me.Session("UserSubjectDtl")) Then
                Me.Session.Add("UserSubjectDtl", DtSubjects)
            Else
                Me.Session("UserSubjectDtl") = DtSubjects
            End If


            btnSearch_Click(sender, e)
            'Me.UpControls.Update()
            'Me.UpGridSubjectSample.Update()
            gvwSubjectSample.Visible = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....btnOk_Click")
        End Try

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        If Me.hfTextChnaged.Value.ToUpper.Trim() = "BARCODE" Then
            Me.hfTextChnaged.Value = ""
            Exit Sub
        End If

        If Not GenCall_Data() Then
            Exit Sub
        End If
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "SearchClearCookie({hCookie:'" + HdnCookie.Value.ToString + "'});", True)
        iFrmDynamicPage.Visible = False
        UpdatePanel1.Update()
        'If Not FillGrid() Then
        '    Exit Sub
        'End If

        Me.txtScan.Focus()
    End Sub
#End Region

#Region "Method For Check Activity Deviation"
    Private Function CheckSequence(ByVal vWorkspaceId As String, _
                                         ByVal NodeId As Integer, _
                                         ByVal SubjectId As String, _
                                         ByVal Period As Integer) As Boolean

        Dim eStr As String = String.Empty
        Dim Param As String = String.Empty
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim Ds_Structure As DataSet = Nothing
        Dim SeqNo As Integer
        Dim Str As String = String.Empty

        Dim PendingNode As String = String.Empty
        Dim i As Integer
        Dim temp As Array

        Dim lbl As Label
        Dim gridView As GridView



        Try
            Param = vWorkspaceId + "##" + SubjectId.ToString.ToUpper.Trim() + "##" + Period.ToString()
            If Not objHelp.Proc_GetStructure(Param, Ds_Structure, eStr) Then
                Return False
            End If

            If Ds_Structure.Tables(0).Rows.Count > 0 Then

                temp = Regex.Split(SubjectId.ToString.ToUpper.Trim(), ",")
                SeqNo = Ds_Structure.Tables(0).Select(" iNodeId = " & NodeId.ToString)(0).Item("SeqNo")

                For i = 0 To temp.Length - 1
                    If Ds_Structure.Tables(0).Select(" iNodeId = " & NodeId.ToString & " And vSubjectID = '" & temp(i).ToString & "'")(0).Item("ActivityStatus") <> CRF_DataEntryPending Then
                        Continue For
                    End If

                    Ds_Structure.Tables(0).DefaultView.RowFilter = " SeqNo < " & SeqNo.ToString & " And ActivityStatus = '" & CRF_DataEntryPending & "'" & " And vSubjectID = '" & temp(i).ToString & "'"

                    If Ds_Structure.Tables(0).DefaultView.ToTable.Rows.Count > 0 Then

                        For Each dr As DataRow In Ds_Structure.Tables(0).DefaultView.ToTable.Rows
                            If PendingNode.ToString = "" Then
                                PendingNode = dr.Item("iNodeId").ToString()
                            Else
                                PendingNode = PendingNode.ToString().Trim + "," + dr.Item("iNodeId").ToString()
                            End If

                        Next dr

                        Str += temp(i).ToString() + "@@" + PendingNode.ToString() + "##"
                        PendingNode = String.Empty
                        lbl = New Label
                        lbl.ID = "lblDeviation" & i
                        lbl.CssClass = "Label"
                        lbl.Text = Ds_Structure.Tables(0).DefaultView.ToTable().Rows(0)("vMySubjectNo")

                        gridView = New GridView
                        gridView.ID = "GvDeviation" & i
                        gridView.AutoGenerateColumns = False
                        gridView.SkinID = "grdViewSmlAutoSize"
                        gridView.Attributes.Add("style", "width:100%")


                        Dim bf As BoundField = New BoundField
                        Dim dc As DataColumn
                        dc = New DataColumn(Ds_Structure.Tables(0).DefaultView.ToTable().Columns("vNodeDisplayName").ColumnName)

                        bf.DataField = dc.ColumnName
                        bf.HeaderText = "Pending Activity"
                        gridView.Columns.Add(bf)
                        gridView.DataSource = Ds_Structure.Tables(0).DefaultView.ToTable()
                        gridView.DataBind()

                        Me.PlaceDeviation.Controls.Add(lbl)
                        Me.PlaceDeviation.Controls.Add(gridView)

                        hdnFrom.Value = "SCH"
                    End If

                Next i
                Me.HPendingNode.Value = Str.ToString()
            Else
                Me.HPendingNode.Value = ""
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, ".......CheckSequence")
            Return False
        End Try
    End Function
#End Region

#Region "Grid Events"

    Protected Sub gvwSubjects_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwSubjects.RowCreated
        e.Row.Cells(GVCSub_WorkspaceId).Visible = False
        e.Row.Cells(GVCSub_MySubjectNo).Visible = False
    End Sub

    Protected Sub gvwSubjects_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwSubjects.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.Cells(GVCSub_Select).FindControl("ChkMove"), CheckBox).Attributes.Add("Onclick", "CheckSelected();")
        End If
    End Sub

    Protected Sub gvwSubjectSample_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwSubjectSample.RowCreated
        If e.Row.RowType = DataControlRowType.Header Or _
                    e.Row.RowType = DataControlRowType.DataRow Or _
                    e.Row.RowType = DataControlRowType.Footer Then
            'e.Row.Cells(GVC_nSampleId).Visible = False
            'e.Row.Cells(GVC_vSampleId).Visible = False
            'e.Row.Cells(GVC_FullName).Visible = False
            e.Row.Cells(GVC_RefNodeId).Visible = False
            'e.Row.Cells(GVC_CollectionDateTime).Visible = False
            'e.Row.Cells(GVC_iCollectionBy).Visible = False
            'e.Row.Cells(GVC_vCollectionBy).Visible = False
            'e.Row.Cells(GVC_DosingTime).Visible = False
            'e.Row.Cells(GVC_Remarks).Visible = False
            e.Row.Cells(GVC_MySubjectNo).Visible = False
            e.Row.Cells(GVC_SubjectID1).Visible = False
            e.Row.Cells(GVC_IsDataEntryDone).Visible = False
            e.Row.Cells(GVC_DataStatus).Visible = False
            e.Row.Cells(GVC_ReplaceSubject).Visible = False
            'e.Row.Cells(GVC_vSubjectId).Visible = False
            'e.Row.Cells(GVC_AttendanceMysubNo).Visible = False


        End If
    End Sub

    Protected Sub gvwSubjectSample_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwSubjectSample.RowDataBound
        Dim index As Integer = e.Row.RowIndex
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim dt_RefTime As New DataTable
        Dim dt As New DataTable
        Dim dv_RefTime As New DataView
        Dim refTime As Date
        Dim time1() As String
        Dim time As String = String.Empty
        Dim strDate As String = String.Empty
        Dim dt_WorkspaceSubjectMst As New DataTable
        Dim dv_WorkspaceSubjectMst As New DataView

        Dim dv_ViewSampleDetail As New DataView
        Dim dt_ViewSampleDetail As New DataView
        Dim SchedulingCode As IEnumerable
        Dim clr As String = Drawing.ColorTranslator.ToHtml(Drawing.Color.Red)
        Dim lnkbtn As LinkButton
        Try

            If e.Row.RowType = DataControlRowType.DataRow Then
                lnkbtn = CType(e.Row.FindControl("lnkSubjectId"), LinkButton)
                lnkbtn.CommandArgument = e.Row.RowIndex
                lnkbtn.CommandName = "OPEN"



                'e.Row.Cells(GVC_CollectionDateTime).Text = Replace(e.Row.Cells(GVC_CollectionDateTime).Text.Trim(), "&nbsp;", "")

                'If Not Convert.ToString(e.Row.Cells(GVC_CollectionDateTime).Text).Trim = "" Then
                '    'e.Row.Cells(GVC_CollectionDateTime).Text = CType(e.Row.Cells(GVC_CollectionDateTime).Text.Trim(), Date).GetDateTimeFormats()(90).Trim()
                '    e.Row.Cells(GVC_CollectionDateTime).Text = CType(e.Row.Cells(GVC_CollectionDateTime).Text.Trim(), Date).ToString("dd-MMM-yyyy HH:mm ")
                'End If

                dv_RefTime = Nothing
                dv_RefTime = New DataView
                dv_RefTime = CType(Me.ViewState(VS_DtView_WorkSpaceNodeDetail), DataTable).Copy().DefaultView()

                dv_RefTime.RowFilter = "iNodeId = " + Me.ddlActivity.SelectedValue.Split("##")(1) + " And cISPredose <> 'Y'"
                dt_RefTime = dv_RefTime.ToTable()

                If dt_RefTime.Rows.Count > 0 Then
                    dv_RefTime = Nothing
                    dv_RefTime = New DataView
                    dv_RefTime = CType(Me.ViewState(VS_DtMedexInfoHdr1), DataTable).Copy().DefaultView()

                    'dv_RefTime.RowFilter = "[Scheduling Node] = " + e.Row.Cells(GVC_RefNodeId).Text.Trim() + _
                    '                       " And vMySubjectNo = '" + e.Row.Cells(GVC_vMySubjectNo).Text.Trim() + "'"
                    dv_RefTime.RowFilter = "[Scheduling Node] = " + e.Row.Cells(GVC_RefNodeId).Text.Trim() + _
                                           " And vSubjectId = '" + e.Row.Cells(GVC_SubjectID1).Text.Trim() + "'"
                    dt_RefTime = dv_RefTime.ToTable().DefaultView.ToTable()
                    dt = dt_RefTime.Clone()

                    SchedulingCode = From ele In dt_RefTime Select ele.Field(Of String)("Scheduling Code") Distinct

                    For Each Ans In SchedulingCode
                        dt_RefTime.DefaultView.RowFilter = "[Scheduling Code] = '" + Ans.ToString + "'"
                        dt_RefTime.DefaultView.ToTable().DefaultView.RowFilter = "iTranNo = MAX(iTranNo)"
                        'dt.Rows.Add(dt_RefTime.DefaultView(0))
                        dt.Merge(dt_RefTime.DefaultView.ToTable().DefaultView.ToTable())
                    Next
                    'dt_RefTime.DefaultView.RowFilter = "iTranNo = MAX(iTranNo)"
                    dt_RefTime = dt
                    If dt_RefTime.Rows.Count > 1 Then

                        strDate = dt_RefTime.Rows(0).Item("Actual Value").ToString.Trim() + " "
                        strDate += dt_RefTime.Rows(1).Item("Actual Value").ToString.Trim()

                        dt_RefTime.DefaultView.RowFilter = "vMEdextype ='AsyncDateTime'"
                        If dt_RefTime.DefaultView.Count > 0 Then
                            strDate = dt_RefTime.DefaultView(0).Item("Actual Value").ToString.Trim() + " "
                            dt_RefTime.DefaultView.RowFilter = "vMEdextype ='AsyncTime'"
                            If dt_RefTime.DefaultView.Count > 0 Then
                                strDate += dt_RefTime.DefaultView(0).Item("Actual Value").ToString.Trim()
                            End If
                        End If



                        If strDate.Trim() <> "" Then
                            refTime = CType(strDate.Trim(), Date)
                            'e.Row.Cells(GVC_DosingTime).Text = refTime.GetDateTimeFormats()(23).Trim() 'Added by Parth Pandya for date format like 01 September, 2003 12:00 AM
                            'e.Row.Cells(GVC_DosingTime).Text = refTime.GetDateTimeFormats()(41).Trim()

                            time = CType(e.Row.Cells(GVC_RefTime).Text.Trim(), String)
                            time1 = time.Split(".")

                            If Not time1(1) = 0 Then
                                refTime = refTime.AddHours(((time1(0) * 60) + time1(1)) / 60)
                            Else
                                refTime = refTime.AddHours(time1(0))
                            End If

                            e.Row.Cells(GVC_RefTime).Text = refTime.ToString("dd-MMM-yyyy HH:mm ")  'GetDateTimeFormats()(25).Trim()
                            'e.Row.Cells(GVC_RefTime).Text = refTime.GetDateTimeFormats()(41).Trim()
                        Else
                            e.Row.Cells(GVC_RefTime).Text = ""
                        End If

                        'lnkbtn.Text = dt_RefTime.Rows(0).Item("vSubjectId").ToString.Trim()

                        ' e.Row.Cells(GVC_FullName).Text = dt_RefTime.Rows(0).Item("vSubjectName").ToString.Trim()

                    Else
                        e.Row.Cells(GVC_RefTime).Text = ""
                        'e.Row.Cells(GVC_DosingTime).Text = ""
                    End If

                Else
                    e.Row.Cells(GVC_RefTime).Text = ""
                    'e.Row.Cells(GVC_DosingTime).Text = ""
                End If

                'If e.Row.Cells(GVC_IsDataEntryDone).Text = "Y" Then
                '    e.Row.BackColor = Drawing.Color.ForestGreen
                '    lnkbtn.ForeColor = Drawing.Color.White
                '    e.Row.ForeColor = Drawing.Color.White
                'End If

                If e.Row.Cells(GVC_DataStatus).Text = CRF_DataEntry Or e.Row.Cells(GVC_DataStatus).Text = CRF_DataEntryCompleted Then
                    e.Row.BackColor = Drawing.Color.Orange
                    lnkbtn.ForeColor = Drawing.Color.White
                    e.Row.ForeColor = Drawing.Color.White
                ElseIf e.Row.Cells(GVC_DataStatus).Text = CRF_Review Then
                    e.Row.BackColor = Drawing.Color.Blue
                    lnkbtn.ForeColor = Drawing.Color.White
                    e.Row.ForeColor = Drawing.Color.White
                End If

                If e.Row.Cells(GVC_ReplaceSubject).Text = "Y" Then
                    e.Row.ForeColor = Drawing.Color.Red
                    lnkbtn.ForeColor = Drawing.Color.Red
                End If

                dv_WorkspaceSubjectMst = Nothing
                dv_WorkspaceSubjectMst = New DataView
                dv_WorkspaceSubjectMst = CType(Me.ViewState(VS_DtViewWorkSpaceSubjectMst), DataTable).Copy().DefaultView()

                dv_WorkspaceSubjectMst.RowFilter = "iMySubjectNoNew = " + e.Row.Cells(GVC_vMySubjectNo).Text.Trim().Replace("X", "").Replace("x", "")
                dt_WorkspaceSubjectMst = dv_WorkspaceSubjectMst.ToTable()

                If dt_WorkspaceSubjectMst.Rows.Count > 0 Then
                    'lnkbtn.Text = dt_WorkspaceSubjectMst.Rows(0).Item("vSubjectId").ToString.Trim()
                    'e.Row.Cells(GVC_FullName).Text = dt_WorkspaceSubjectMst.Rows(0).Item("vInitials").ToString.Trim()
                End If

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....gvwSubjectSample_RowDataBound")
        End Try
    End Sub

    Protected Sub gvwSubjectSample_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwSubjectSample.RowCommand
        Dim index As Integer = CType(e.CommandArgument, Integer)
        Dim WorkspaceId As String = String.Empty
        Dim ActivityId As String = String.Empty
        Dim SubjectId As String = String.Empty
        Dim Period As String = String.Empty
        Dim NodeId As String = String.Empty
        Dim iMySubjectNo As String = String.Empty
        Dim MySubjectNo As String = String.Empty
        Dim wstr, estr As String
        Dim ds_DataEntryControl As New DataSet
        Dim dr As DataRow
        Dim Return_Val As Char

        Try

            If e.CommandName.ToUpper.Trim() = "OPEN" Then
                'Session(S_DynamicPage_URL) = Nothing

                WorkspaceId = HProjectId.Value.Trim()
                ActivityId = Me.ddlActivity.SelectedValue.Split("##")(0)
                NodeId = Me.ddlActivity.SelectedValue.Split("##")(1)
                Period = ddlPeriod.SelectedValue.ToString()
                'SubjectId = Me.gvwSubjectSample.Rows(index).Cells(GVC_vSubjectId).Text
                SubjectId = CType(Me.gvwSubjectSample.Rows(index).FindControl("lnkSubjectId"), LinkButton).Text.Trim()
                ViewState(VS_SubjectId) = SubjectId.ToString.ToUpper.Trim()
                iMySubjectNo = Me.gvwSubjectSample.Rows(index).Cells(GVC_MySubjectNo).Text
                MySubjectNo = Me.gvwSubjectSample.Rows(index).Cells(GVC_vMySubjectNo).Text.Replace("X", "").Replace("x", "")
                ViewState(VS_MySubjectNo) = MySubjectNo
                'If dtActivity.DefaultView.Count > 0 Then
                'iFrmDynamicPage.Visible = True
                'iFrmDynamicPage.Attributes("src") = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + WorkspaceId + _
                '         "&ActivityId=" + ActivityId.ToString() + _
                '         "&NodeId=" + NodeId + "&PeriodId=" + Period + "&SubjectId=" + SubjectId + "&Type=BA-BE" + _
                '          "&ScreenNo=" + SubjectId + "&MySubjectNo=" + MySubjectNo + "&SubNo="
                If iFrmDynamicPage.Visible = False Then
                    iFrmDynamicPage.Visible = True
                    Session(S_DynamicPage_URL) = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + WorkspaceId + _
                        "&ActivityId=" + ActivityId.ToString() + _
                        "&NodeId=" + NodeId + "&PeriodId=" + Period + "&SubjectId=" + SubjectId.ToString.ToUpper.Trim() + "&Type=BA-BE" + _
                         "&ScreenNo=" + iMySubjectNo + "&MySubjectNo=" + MySubjectNo + "&From=SCH"
                    hdnDynamicURL.Value = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + WorkspaceId + _
                         "&ActivityId=" + ActivityId.ToString() + _
                         "&NodeId=" + NodeId + "&PeriodId=" + Period + "&SubjectId=" + SubjectId.ToString.ToUpper.Trim() + "&Type=BA-BE" + _
                          "&ScreenNo=" + iMySubjectNo + "&MySubjectNo=" + MySubjectNo + "&From=SCH"

                    wstr = "select DataEntryControl.vWorkspaceId,DataEntryControl.iNodeId,DataEntryControl.vSubjectId,DataEntryControl.imodifyBy," + _
                            "iWorkFlowStageId,UserMST.vFirstName  + ' '  +UserMST.vLastName as vUserName from DataEntryControl " + _
                            "inner join UserMst on (DataEntryControl.imodifyBy=UserMst.iUserId)   " + _
                            "Where vSubjectId='" & SubjectId.ToString.ToUpper.Trim() & "' And vWorkspaceId='" & WorkspaceId & "' And iNodeId=" & NodeId + " and iWorkFlowStageId = " + Session(S_WorkFlowStageId)

                    ds_DataEntryControl = ObjHelp.GetResultSet(wstr, "DataEntryControl")

                    If ds_DataEntryControl.Tables(0).Rows.Count > 0 Then


                        If ds_DataEntryControl.Tables(0).Rows(0).Item("iModifyBy") <> Session(S_UserID) AndAlso ds_DataEntryControl.Tables(0).Rows(0).Item("iWorkFlowStageId") = Session(S_WorkFlowStageId) Then
                            Me.lblDataEntrycontroller.Text = ds_DataEntryControl.Tables(0).Rows(0).Item("vUserName").ToString
                            Me.MpeDataentryControl.Show()
                            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "Open({flag:'LOAD',wId:'" + WorkspaceId.ToString + "',nId:'" + NodeId.ToString + "',pId:'" + Period.ToString + "',sId:'" + SubjectId.ToString + "',mNo:'" + MySubjectNo.ToString + "'});", True)
                            Exit Sub
                        Else
                            ''ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "Open({flag:'LOAD',wId:'" + WorkspaceId.ToString + "',nId:'" + NodeId.ToString + "',pId:'" + Period.ToString + "',sId:'" + SubjectId.ToString + "',mNo:'" + MySubjectNo.ToString + "'});", True)
                            'If HdnCookie.Value.ToString <> "" Then
                            '    If HdnCookie.Value = "" + WorkspaceId.ToString + ":" + NodeId.ToString + ":" + Period.ToString + ":" + SubjectId.ToString + ":" + MySubjectNo.ToString + "" Then
                            '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "RemoveCookie({hCookie:'" + HdnCookie.Value.ToString + "'});Open({flag:'LOAD',wId:'" + WorkspaceId.ToString + "',nId:'" + NodeId.ToString + "',pId:'" + Period.ToString + "',sId:'" + SubjectId.ToString + "',mNo:'" + MySubjectNo.ToString + "'});", True)
                            '        'Remove cookie and then add cookie
                            '    ElseIf HdnCookie.Value <> "" + WorkspaceId.ToString + ":" + NodeId.ToString + ":" + Period.ToString + ":" + SubjectId.ToString + ":" + MySubjectNo.ToString + "" Then
                            '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "OpenAlready();", True)
                            '    End If
                            'Else
                            '    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "RemoveCookie({hCookie:'" + WorkspaceId.ToString + ":" + NodeId.ToString + ":" + Period.ToString + ":" + SubjectId.ToString + ":" + MySubjectNo.ToString + "'});Open({flag:'LOAD',wId:'" + WorkspaceId.ToString + "',nId:'" + NodeId.ToString + "',pId:'" + Period.ToString + "',sId:'" + SubjectId.ToString + "',mNo:'" + MySubjectNo.ToString + "'});", True)
                            '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "CompareCookie({hCookie:'" + WorkspaceId.ToString + ":" + NodeId.ToString + ":" + Period.ToString + ":" + SubjectId.ToString + ":" + MySubjectNo.ToString + "'});Open({flag:'LOAD',wId:'" + WorkspaceId.ToString + "',nId:'" + NodeId.ToString + "',pId:'" + Period.ToString + "',sId:'" + SubjectId.ToString + "',mNo:'" + MySubjectNo.ToString + "'});", True)
                            'End If

                            If HdnCookie.Value.ToString <> "" Then
                                If HdnCookie.Value <> "" + WorkspaceId.ToString + ":" + NodeId.ToString + ":" + Period.ToString + ":" + SubjectId.ToString.ToUpper.Trim() + "" Then
                                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "OpenAlready();", True)
                                    iFrmDynamicPage.Visible = False
                                End If
                            Else
                                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "RemoveCookie({hCookie:'" + WorkspaceId.ToString + ":" + NodeId.ToString + ":" + Period.ToString + ":" + SubjectId.ToString + ":" + MySubjectNo.ToString + "'});Open({flag:'LOAD',wId:'" + WorkspaceId.ToString + "',nId:'" + NodeId.ToString + "',pId:'" + Period.ToString + "',sId:'" + SubjectId.ToString + "',mNo:'" + MySubjectNo.ToString + "'});", True)
                                'criptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "CompareCookie({hCookie:'" + WorkspaceId.ToString + ":" + NodeId.ToString + ":" + Period.ToString + ":" + SubjectId.ToString + ":" + MySubjectNo.ToString + "'});Open({flag:'LOAD',wId:'" + WorkspaceId.ToString + "',nId:'" + NodeId.ToString + "',pId:'" + Period.ToString + "',sId:'" + SubjectId.ToString + "',mNo:'" + MySubjectNo.ToString + "'});", True)
                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "OpenAlready();", True)
                                iFrmDynamicPage.Visible = False
                            End If

                        End If



                    Else
                        dr = ds_DataEntryControl.Tables(0).NewRow()
                        dr("vWorkspaceId") = WorkspaceId
                        dr("iNodeId") = NodeId
                        dr("vSubjectId") = SubjectId.ToString.ToUpper.Trim()
                        dr("iModifyBy") = Session(S_UserID)
                        dr("iWorkFlowStageId") = Session(S_WorkFlowStageId)

                        ds_DataEntryControl.Tables(0).Rows.Add(dr)
                        ds_DataEntryControl.Tables(0).AcceptChanges()

                        If Not ObjLambda.insert_DataEntryControl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_DataEntryControl, estr, Return_Val) Then
                            Throw New Exception("Error While saving information of Data Entry Control...PageLoad()..")
                        End If
                        If Return_Val = "Y" Then
                            Me.lblDataEntrycontroller.Text = ds_DataEntryControl.Tables(0).Rows(0).Item("vUserName").ToString
                            Me.MpeDataentryControl.Show()
                            'Me.txtScan.Text = ""
                            'Me.txtScan.Focus()
                            Exit Sub
                        End If

                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "RemoveCookie({hCookie:'" + HdnCookie.Value.ToString + "','2'});Open({flag:'LOAD',wId:'" + WorkspaceId.ToString + "',nId:'" + NodeId.ToString + "',pId:'" + Period.ToString + "',sId:'" + SubjectId.ToString + "'});", True)
                        'Remove cookie and then add cookie

                        'Open cookie
                        HdnCookie.Value = WorkspaceId.ToString + ":" + NodeId.ToString + ":" + Period.ToString + ":" + SubjectId.ToString.ToUpper.Trim()
                        iFrmDynamicPage.Attributes("src") = hdnDynamicURL.Value

                        UpdatePanel1.Update()
                    End If

                Else
                    Session(S_DynamicPage_URL) = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + WorkspaceId + _
                             "&ActivityId=" + ActivityId.ToString() + _
                             "&NodeId=" + NodeId + "&PeriodId=" + Period + "&SubjectId=" + SubjectId.ToString.ToUpper.Trim() + "&Type=BA-BE" + _
                              "&ScreenNo=" + SubjectId.ToString.ToUpper.Trim() + "&MySubjectNo=" + MySubjectNo + "&&From=SCH"
                    hdnDynamicURL.Value = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + WorkspaceId + _
                             "&ActivityId=" + ActivityId.ToString() + _
                             "&NodeId=" + NodeId + "&PeriodId=" + Period + "&SubjectId=" + SubjectId.ToString.ToUpper.Trim() + "&Type=BA-BE" + _
                              "&ScreenNo=" + SubjectId.ToString.ToUpper.Trim() + "&MySubjectNo=" + MySubjectNo + "&&From=SCH"

                    wstr = "select DataEntryControl.vWorkspaceId,DataEntryControl.iNodeId,DataEntryControl.vSubjectId,DataEntryControl.imodifyBy," + _
                           "iWorkFlowStageId,UserMST.vFirstName  + ' '  +UserMST.vLastName as vUserName from DataEntryControl " + _
                           "inner join UserMst on (DataEntryControl.imodifyBy=UserMst.iUserId)   " + _
                           "Where vSubjectId='" & SubjectId.ToString.ToUpper.Trim() & "' And vWorkspaceId='" & WorkspaceId & "' And iNodeId=" & NodeId + " and iWorkFlowStageId = " + Session(S_WorkFlowStageId)

                    ds_DataEntryControl = ObjHelp.GetResultSet(wstr, "DataEntryControl")

                    If ds_DataEntryControl.Tables(0).Rows.Count > 0 Then


                        If ds_DataEntryControl.Tables(0).Rows(0).Item("iModifyBy") <> Session(S_UserID) AndAlso ds_DataEntryControl.Tables(0).Rows(0).Item("iWorkFlowStageId") = Session(S_WorkFlowStageId) Then
                            Me.lblDataEntrycontroller.Text = ds_DataEntryControl.Tables(0).Rows(0).Item("vUserName").ToString
                            Me.MpeDataentryControl.Show()
                            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "Open({flag:'LOAD',wId:'" + WorkspaceId.ToString + "',nId:'" + NodeId.ToString + "',pId:'" + Period.ToString + "',sId:'" + SubjectId.ToString + "',mNo:'" + MySubjectNo.ToString + "'});", True)
                            Exit Sub
                        Else
                            ''ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "Open({flag:'LOAD',wId:'" + WorkspaceId.ToString + "',nId:'" + NodeId.ToString + "',pId:'" + Period.ToString + "',sId:'" + SubjectId.ToString + "',mNo:'" + MySubjectNo.ToString + "'});", True)
                            'If HdnCookie.Value.ToString <> "" Then
                            '    If HdnCookie.Value = "" + WorkspaceId.ToString + ":" + NodeId.ToString + ":" + Period.ToString + ":" + SubjectId.ToString + ":" + MySubjectNo.ToString + "" Then
                            '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "RemoveCookie({hCookie:'" + HdnCookie.Value.ToString + "'});Open({flag:'LOAD',wId:'" + WorkspaceId.ToString + "',nId:'" + NodeId.ToString + "',pId:'" + Period.ToString + "',sId:'" + SubjectId.ToString + "',mNo:'" + MySubjectNo.ToString + "'});", True)
                            '        'Remove cookie and then add cookie
                            '    ElseIf HdnCookie.Value <> "" + WorkspaceId.ToString + ":" + NodeId.ToString + ":" + Period.ToString + ":" + SubjectId.ToString + ":" + MySubjectNo.ToString + "" Then
                            '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "OpenAlready();", True)
                            '    End If
                            'Else
                            '    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "RemoveCookie({hCookie:'" + WorkspaceId.ToString + ":" + NodeId.ToString + ":" + Period.ToString + ":" + SubjectId.ToString + ":" + MySubjectNo.ToString + "'});Open({flag:'LOAD',wId:'" + WorkspaceId.ToString + "',nId:'" + NodeId.ToString + "',pId:'" + Period.ToString + "',sId:'" + SubjectId.ToString + "',mNo:'" + MySubjectNo.ToString + "'});", True)
                            '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "CompareCookie({hCookie:'" + WorkspaceId.ToString + ":" + NodeId.ToString + ":" + Period.ToString + ":" + SubjectId.ToString + ":" + MySubjectNo.ToString + "'});Open({flag:'LOAD',wId:'" + WorkspaceId.ToString + "',nId:'" + NodeId.ToString + "',pId:'" + Period.ToString + "',sId:'" + SubjectId.ToString + "',mNo:'" + MySubjectNo.ToString + "'});", True)
                            'End If

                            If HdnCookie.Value.ToString <> "" Then
                                If HdnCookie.Value <> "" + WorkspaceId.ToString + ":" + NodeId.ToString + ":" + Period.ToString + ":" + SubjectId.ToString.ToUpper.Trim() + "" Then
                                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "OpenAlready();", True)
                                Else
                                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "click", "iFramebtnClicked('grid');", True)
                                End If
                            Else
                                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "RemoveCookie({hCookie:'" + WorkspaceId.ToString + ":" + NodeId.ToString + ":" + Period.ToString + ":" + SubjectId.ToString + ":" + MySubjectNo.ToString + "'});Open({flag:'LOAD',wId:'" + WorkspaceId.ToString + "',nId:'" + NodeId.ToString + "',pId:'" + Period.ToString + "',sId:'" + SubjectId.ToString + "',mNo:'" + MySubjectNo.ToString + "'});", True)
                                'criptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "CompareCookie({hCookie:'" + WorkspaceId.ToString + ":" + NodeId.ToString + ":" + Period.ToString + ":" + SubjectId.ToString + ":" + MySubjectNo.ToString + "'});Open({flag:'LOAD',wId:'" + WorkspaceId.ToString + "',nId:'" + NodeId.ToString + "',pId:'" + Period.ToString + "',sId:'" + SubjectId.ToString + "',mNo:'" + MySubjectNo.ToString + "'});", True)
                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "OpenAlready();", True)
                            End If

                        End If



                    Else
                        dr = ds_DataEntryControl.Tables(0).NewRow()
                        dr("vWorkspaceId") = WorkspaceId
                        dr("iNodeId") = NodeId
                        dr("vSubjectId") = SubjectId.ToString.ToUpper.Trim()
                        dr("iModifyBy") = Session(S_UserID)
                        dr("iWorkFlowStageId") = Session(S_WorkFlowStageId)

                        ds_DataEntryControl.Tables(0).Rows.Add(dr)
                        ds_DataEntryControl.Tables(0).AcceptChanges()
                        If Not ObjLambda.insert_DataEntryControl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_DataEntryControl, estr, Return_Val) Then
                            Throw New Exception("Error While saving information of Data Entry Control...PageLoad()..")
                        End If
                        If Return_Val = "Y" Then
                            Me.lblDataEntrycontroller.Text = ds_DataEntryControl.Tables(0).Rows(0).Item("vUserName").ToString
                            Me.MpeDataentryControl.Show()
                            'Me.txtScan.Text = ""
                            'Me.txtScan.Focus()
                            Exit Sub
                        End If

                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "RemoveCookie({hCookie:'" + HdnCookie.Value.ToString + "','2'});Open({flag:'LOAD',wId:'" + WorkspaceId.ToString + "',nId:'" + NodeId.ToString + "',pId:'" + Period.ToString + "',sId:'" + SubjectId.ToString + "'});", True)
                        'Remove cookie and then add cookie

                        'Open cookie
                        HdnCookie.Value = WorkspaceId.ToString + ":" + NodeId.ToString + ":" + Period.ToString + ":" + SubjectId.ToString.ToUpper.Trim()
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "click", "iFramebtnClicked('grid');", True)
                    End If


                    'UpdatePanel1.Update()
                End If
                'End If

            End If
            txtScan.Text = ""
            txtScan.Focus()
        Catch ex As Exception
            Me.txtScan.Text = ""
            Me.txtScan.Focus()
            Me.ShowErrorMessage("Error While gvwSubejctSample_RowCommand.", ex.Message)

        End Try


    End Sub

#End Region

#Region "Drop down Events"

    Protected Sub ddlPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPeriod.SelectedIndexChanged
        Try
            If Not FillddlActivity() Then
                Exit Sub
            End If
            If ddlPeriod.SelectedValue = "0" Then
                btnSubjectMgmt.Enabled = False
                btnSearch.Enabled = False
            End If
            If Not ClearControls() Then
                Exit Sub
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub ddlActivity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlActivity.SelectedIndexChanged

        Dim dt_Activity As New DataTable
        Dim dv_Activity As New DataView

        Try
            ViewState(VS_SubjectIdGunned) = ""
            canal.Visible = False
            Me.gvwSubjectSample.DataSource = Nothing
            Me.gvwSubjectSample.DataBind()

            dv_Activity = CType(Me.ViewState(VS_DtView_WorkSpaceNodeDetail), DataTable).Copy().DefaultView

            btnSubjectMgmt.Enabled = True
            btnSearch.Enabled = True
            If ddlActivity.SelectedValue = "0" Then
                btnSubjectMgmt.Enabled = False
                btnSearch.Enabled = False
                Exit Sub
            End If
            dv_Activity.RowFilter = "iNodeID = " + Me.ddlActivity.SelectedValue.Split("##")(1)



            If Not dv_Activity.ToTable() Is Nothing Then
                If dv_Activity.ToTable().Rows.Count > 0 Then
                    If Not (dv_Activity.ToTable().Rows(0)("nRefTime") Is System.DBNull.Value) Then
                        Me.ViewState(Vs_RefTime) = dv_Activity.ToTable().Rows(0).Item("nRefTime").ToString
                        Me.ViewState(Vs_RefNodeId) = dv_Activity.ToTable().Rows(0).Item("iRefNodeId").ToString
                        dv_Activity.RowFilter = " nRefTime=" + Me.ViewState(Vs_RefTime).ToString + " and iPeriod=" + ddlPeriod.SelectedValue.Trim + " and iRefNodeId=" + Me.ViewState(Vs_RefNodeId).ToString

                        If Not (dv_Activity.ToTable().Rows(0)("nDeviationTime") Is System.DBNull.Value) Then


                        End If
                    Else
                        ObjCommon.ShowAlert("Please add refrence time for selected activity.", Me.Page)
                    End If
                End If
            End If

            If Not ClearControls() Then
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....ddlActivity_SelectedIndexChanged")

        End Try

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

    Protected Sub txtScan_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtScan.TextChanged
        Dim wstr, estr As String
        Dim ds_DataEntryControl As New DataSet
        Dim dr As DataRow
        Dim ValidSubject As Boolean = False
        Dim WorkspaceId, Period, ActivityId, NodeId, SubjectId, MySubjectNo, iMySubjectNo As String
        Dim Return_Val As Char
        Try
            'add by shivani pandya for latest repeatition
            Me.Session(S_SelectedRepeatation) = ""

            ' Me.txtScan.Enabled = False
            Me.Session(S_DynamicPage_URL) = Nothing
            Dim CurDate As DateTime = CDate(ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName)).DateTime).GetDateTimeFormats()(23) 'for date format like  01 September, 2003 12:00 AM
            ViewState(Vs_CurTime) = CurDate
            SubjectId = txtScan.Text.Trim()
            ViewState(VS_SubjectId) = SubjectId.ToString.ToUpper.Trim()

            'MySubjectNo = "1002"
            WorkspaceId = HProjectId.Value.Trim()
            Period = ddlPeriod.SelectedValue.Trim()
            ActivityId = Me.ddlActivity.SelectedValue.Split("##")(0)
            NodeId = Me.ddlActivity.SelectedValue.Split("##")(1)
            Dim cnt As Integer = 0
            For index As Integer = 0 To Me.gvwSubjectSample.Rows.Count - 1
                If Me.gvwSubjectSample.Rows(index).Cells(GVC_IsDataEntryDone).Text.ToUpper.Trim() = "N" Then
                    cnt = cnt + 1
                End If
            Next
            hdnDataEntryPendingCount.Value = cnt
            For index As Integer = 0 To Me.gvwSubjectSample.Rows.Count - 1

                ValidSubject = False

                If CType(Me.gvwSubjectSample.Rows(index).FindControl("lnkSubjectId"), LinkButton).Text.ToUpper = SubjectId.ToString().ToUpper.Trim() Then
                    MySubjectNo = Me.gvwSubjectSample.Rows(index).Cells(GVC_vMySubjectNo).Text.ToUpper.Trim().Replace("X", "").Replace("x", "")
                    iMySubjectNo = Me.gvwSubjectSample.Rows(index).Cells(GVC_MySubjectNo).Text.ToUpper.Trim()
                    ViewState(VS_MySubjectNo) = MySubjectNo
                    ValidSubject = True
                    'If gvwSubjectSample.Rows(index).Cells(GVC_IsDataEntryDone).Text.ToUpper.Trim() = "Y" Then
                    '    'ObjCommon.ShowAlert("Date and Time data Entry done for Subject (" + SubjectId.ToString().ToUpper.Trim() + ").", Me.Page)
                    '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "Open({flag:'LOAD',wId:'" + WorkspaceId.ToString + "',nId:'" + NodeId.ToString + "',pId:'" + Period.ToString + "',sId:'" + SubjectId.ToString + "'});", True)
                    '    'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "click", "iFramebtnClicked1();", True)
                    '    'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "click", "iFramebtnClicked1();", True)
                    '    Me.txtScan.Text = ""
                    '    Me.txtScan.Focus()
                    '    Exit Sub
                    'End If
                    Exit For
                End If

            Next

            If ValidSubject = False Then
                ObjCommon.ShowAlert("Subject (" + SubjectId.ToString().ToUpper.Trim() + ") Is Not Valid For You", Me.Page)
                Me.txtScan.Text = ""
                Me.txtScan.Focus()
                Exit Sub
            End If
            If ViewState(VS_SubjectIdGunned).ToString.ToUpper.Trim() = SubjectId.ToString.ToUpper.Trim() AndAlso CInt(hdnDataEntryPendingCount.Value) > 1 Then
                ObjCommon.ShowAlert("You can not gun the same subject (" + SubjectId.ToString().ToUpper.Trim() + ") again.", Me.Page)
                Me.txtScan.Text = ""
                Me.txtScan.Focus()
                Exit Sub
            End If
            ViewState(VS_SubjectIdGunned) = SubjectId.ToString.ToUpper.Trim()
            wstr = "select DataEntryControl.vWorkspaceId,DataEntryControl.iNodeId,DataEntryControl.vSubjectId,DataEntryControl.imodifyBy," + _
                            "iWorkFlowStageId,UserMST.vFirstName  + ' '  +UserMST.vLastName as vUserName from DataEntryControl " + _
                            "inner join UserMst on (DataEntryControl.imodifyBy=UserMst.iUserId)   " + _
                            "Where vSubjectId='" & SubjectId.ToString.ToUpper.Trim() & "' And vWorkspaceId='" & WorkspaceId & "' And iNodeId=" & NodeId + " and iWorkFlowStageId = " + Session(S_WorkFlowStageId)

            ds_DataEntryControl = ObjHelp.GetResultSet(wstr, "DataEntryControl")

            If ds_DataEntryControl.Tables(0).Rows.Count > 0 Then

                If ds_DataEntryControl.Tables(0).Rows(0).Item("iModifyBy") <> Session(S_UserID) AndAlso ds_DataEntryControl.Tables(0).Rows(0).Item("iWorkFlowStageId") = Session(S_WorkFlowStageId) Then
                    Me.lblDataEntrycontroller.Text = ds_DataEntryControl.Tables(0).Rows(0).Item("vUserName").ToString
                    Me.MpeDataentryControl.Show()
                    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "Open({flag:'LOAD',wId:'" + WorkspaceId.ToString + "',nId:'" + NodeId.ToString + "',pId:'" + Period.ToString + "',sId:'" + SubjectId.ToString + "',mNo:'" + MySubjectNo.ToString + "'});", True)
                    Me.txtScan.Text = ""
                    Me.txtScan.Focus()
                    Exit Sub
                Else
                    ''ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "Open({flag:'LOAD',wId:'" + WorkspaceId.ToString + "',nId:'" + NodeId.ToString + "',pId:'" + Period.ToString + "',sId:'" + SubjectId.ToString + "',mNo:'" + MySubjectNo.ToString + "'});", True)
                    'If HdnCookie.Value.ToString <> "" Then
                    '    If HdnCookie.Value = "" + WorkspaceId.ToString + ":" + NodeId.ToString + ":" + Period.ToString + ":" + SubjectId.ToString + ":" + MySubjectNo.ToString + "" Then
                    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "RemoveCookie({hCookie:'" + HdnCookie.Value.ToString + "'});Open({flag:'LOAD',wId:'" + WorkspaceId.ToString + "',nId:'" + NodeId.ToString + "',pId:'" + Period.ToString + "',sId:'" + SubjectId.ToString + "',mNo:'" + MySubjectNo.ToString + "'});", True)
                    '        'Remove cookie and then add cookie
                    '    ElseIf HdnCookie.Value <> "" + WorkspaceId.ToString + ":" + NodeId.ToString + ":" + Period.ToString + ":" + SubjectId.ToString + ":" + MySubjectNo.ToString + "" Then
                    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "OpenAlready();", True)
                    '    End If
                    'Else
                    '    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "RemoveCookie({hCookie:'" + WorkspaceId.ToString + ":" + NodeId.ToString + ":" + Period.ToString + ":" + SubjectId.ToString + ":" + MySubjectNo.ToString + "'});Open({flag:'LOAD',wId:'" + WorkspaceId.ToString + "',nId:'" + NodeId.ToString + "',pId:'" + Period.ToString + "',sId:'" + SubjectId.ToString + "',mNo:'" + MySubjectNo.ToString + "'});", True)
                    '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "CompareCookie({hCookie:'" + WorkspaceId.ToString + ":" + NodeId.ToString + ":" + Period.ToString + ":" + SubjectId.ToString + ":" + MySubjectNo.ToString + "'});Open({flag:'LOAD',wId:'" + WorkspaceId.ToString + "',nId:'" + NodeId.ToString + "',pId:'" + Period.ToString + "',sId:'" + SubjectId.ToString + "',mNo:'" + MySubjectNo.ToString + "'});", True)
                    'End If

                    If HdnCookie.Value.ToString <> "" Then
                        If HdnCookie.Value <> "" + WorkspaceId.ToString + ":" + NodeId.ToString + ":" + Period.ToString + ":" + SubjectId.ToString.ToUpper.Trim() + "" Then
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "OpenAlready();", True)
                        Else
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "click", "iFramebtnClicked1();", True)
                        End If
                    Else
                        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "RemoveCookie({hCookie:'" + WorkspaceId.ToString + ":" + NodeId.ToString + ":" + Period.ToString + ":" + SubjectId.ToString + ":" + MySubjectNo.ToString + "'});Open({flag:'LOAD',wId:'" + WorkspaceId.ToString + "',nId:'" + NodeId.ToString + "',pId:'" + Period.ToString + "',sId:'" + SubjectId.ToString + "',mNo:'" + MySubjectNo.ToString + "'});", True)
                        'criptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "CompareCookie({hCookie:'" + WorkspaceId.ToString + ":" + NodeId.ToString + ":" + Period.ToString + ":" + SubjectId.ToString + ":" + MySubjectNo.ToString + "'});Open({flag:'LOAD',wId:'" + WorkspaceId.ToString + "',nId:'" + NodeId.ToString + "',pId:'" + Period.ToString + "',sId:'" + SubjectId.ToString + "',mNo:'" + MySubjectNo.ToString + "'});", True)
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "OpenAlready();", True)
                    End If

                End If



            Else
                dr = ds_DataEntryControl.Tables(0).NewRow()
                dr("vWorkspaceId") = WorkspaceId
                dr("iNodeId") = NodeId
                dr("vSubjectId") = SubjectId.ToString.ToUpper.Trim()
                dr("iModifyBy") = Session(S_UserID)
                dr("iWorkFlowStageId") = Session(S_WorkFlowStageId)

                ds_DataEntryControl.Tables(0).Rows.Add(dr)
                ds_DataEntryControl.Tables(0).AcceptChanges()
                If Not ObjLambda.insert_DataEntryControl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_DataEntryControl, estr, Return_Val) Then
                    Throw New Exception("Error While saving information of Data Entry Control...PageLoad()..")
                End If

                If Return_Val = "Y" Then
                    Me.lblDataEntrycontroller.Text = ds_DataEntryControl.Tables(0).Rows(0).Item("vUserName").ToString
                    Me.MpeDataentryControl.Show()
                    Me.txtScan.Text = ""
                    Me.txtScan.Focus()
                    Exit Sub
                End If



                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "RemoveCookie({hCookie:'" + HdnCookie.Value.ToString + "',Frame123:'1'});Open({flag:'LOAD',wId:'" + WorkspaceId.ToString + "',nId:'" + NodeId.ToString + "',pId:'" + Period.ToString + "',sId:'" + SubjectId.ToString.ToUpper.Trim() + "'});", True)
                'Remove cookie and then add cookie

                'Open cookie
                HdnCookie.Value = WorkspaceId.ToString + ":" + NodeId.ToString + ":" + Period.ToString + ":" + SubjectId.ToString.ToUpper.Trim()
                ''ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "click", "iFramebtnClicked1();", True)
            End If
            'HdnCookie.Value = WorkspaceId.ToString + ":" + NodeId.ToString + ":" + Period.ToString + ":" + SubjectId.ToString + ":" + MySubjectNo.ToString





        Catch ex As Exception
            Me.txtScan.Text = ""
            Me.txtScan.Focus()
            Me.ShowErrorMessage("Error While txtScan_TextChanged.", ex.Message)
        End Try

    End Sub

    Protected Sub btnSaveandredirect_Click(sender As Object, e As EventArgs) Handles btnSaveandredirect.Click
        Dim wstr, estr, strCRFDtlNo As String
        Dim ds_DataEntryControl As New DataSet
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim dtTemp As New DataTable
        Dim dsCRFHdr As New DataSet
        Dim dsCRFDtl As New DataSet
        Dim dsCRFSubDtl As New DataSet
        Dim dsNodeInfo As New DataSet

        Dim DtCRFHdr As New DataTable
        Dim DtCRFDtl As New DataTable
        Dim DtCRFSubDtl As New DataTable
        Dim ValidSubject As Boolean = False
        Dim WorkspaceId, Period, ActivityId, NodeId, NodeIndex, SubjectId, MySubjectNo, Choice As String
        Dim isAsyncDate, isAsyncTime As Boolean
        Dim sSubjectIds As String = String.Empty
        Try
            Dim CurDate As DateTime = ViewState(Vs_CurTime)

            'Dim request38 As System.Net.HttpWebRequest = System.Net.WebRequest.Create("http://10.1.10.39")
            'Dim response38 As System.Net.HttpWebResponse = request38.GetResponse()
            'Dim Date38 As DateTime = DateTime.Parse(response38.Headers("Date"))
            strCRFDtlNo = ""
            SubjectId = txtScan.Text.Trim()
            'MySubjectNo = "1002"
            WorkspaceId = HProjectId.Value.Trim()
            Period = ddlPeriod.SelectedValue.Trim()
            ActivityId = Me.ddlActivity.SelectedValue.Split("##")(0)
            NodeId = Me.ddlActivity.SelectedValue.Split("##")(1)
            For index As Integer = 0 To Me.gvwSubjectSample.Rows.Count - 1

                ValidSubject = False
                If CType(Me.gvwSubjectSample.Rows(index).FindControl("lnkSubjectId"), LinkButton).Text.ToUpper = SubjectId.ToString().ToUpper.Trim() Then
                    MySubjectNo = Me.gvwSubjectSample.Rows(index).Cells(GVC_MySubjectNo).Text.ToUpper.Trim().Replace("X", "").Replace("x", "")
                    ValidSubject = True
                    Exit For
                End If

            Next

            If ValidSubject = False Then
                'ObjCommon.ShowAlert("Subject Is Not Valid For You", Me.Page)
                MySubjectNo = ViewState(VS_MySubjectNo)
                SubjectId = ViewState(VS_SubjectId)
                hdnDynamicURL.Value = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + WorkspaceId + _
                            "&ActivityId=" + ActivityId.ToString() + _
                            "&NodeId=" + NodeId + "&PeriodId=" + Period + "&SubjectId=" + SubjectId.ToString.ToUpper.Trim() + "&Type=BA-BE" + _
                             "&ScreenNo=" + SubjectId.ToString.ToUpper.Trim() + "&MySubjectNo=" + MySubjectNo + "&&From=SCH"
                iFrmDynamicPage.Attributes("src") = hdnDynamicURL.Value
                UpdatePanel1.Update()
                Exit Sub
            End If



            'Dim CurDate As DateTime = CDate(ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName)).DateTime).GetDateTimeFormats()(23) 'for date format like  01 September, 2003 12:00 AM
            'Dim localZone As TimeZone = TimeZone.CurrentTimeZone
            'Dim currentDate As DateTime = DateTime.Now
            'Dim currentYear As Integer = currentDate.Year

            'Dim request70 As System.Net.HttpWebRequest = System.Net.WebRequest.Create("http://10.1.10.70")
            'Dim response70 As System.Net.HttpWebResponse = request70.GetResponse()
            'Dim Date70 As DateTime = DateTime.Parse(response70.Headers("Date"))

            'Dim request38 As System.Net.HttpWebRequest = System.Net.WebRequest.Create("http://10.1.10.38")
            'Dim response38 As System.Net.HttpWebResponse = request38.GetResponse()
            'Dim Date38 As DateTime = DateTime.Parse(response38.Headers("Date"))


            'wstr = ""
            'Const dataFmt As String = "{0,-30}{1}"


            DtCRFHdr = CType(Me.ViewState(VS_DtCRFHdr), DataTable)
            DtCRFDtl = CType(Me.ViewState(VS_DtCRFDtl), DataTable)
            DtCRFSubDtl = CType(Me.ViewState(VS_DtCRFSubDtl), DataTable)
            DtCRFSubDtl.Clear()
            wstr = "cActiveFlag<>'N' and vWorkSpaceId='" + WorkspaceId + "'" + _
                            " and vSubjectId='" & _
                            SubjectId.ToString.ToUpper.Trim() & "'" & _
                            " and iPeriod=" & Period & " And iNodeId=" & _
                           NodeId
            'If ddlRepeatNo.SelectedIndex > 2 Then
            '    Wstr += " And iRepeatNo= " & ddlRepeatNo.SelectedValue & ""
            'End If
            wstr += "  Order by iRepeatNo,iSeqNo OPTION (MAXDOP 1)"
            If Not ObjHelp.Proc_GetDataForScheduling(Me.HProjectId.Value.Trim(), Me.ddlPeriod.SelectedItem.Value.Trim(), Me.ddlActivity.SelectedValue.Split("##")(0), Me.ddlActivity.SelectedValue.Split("##")(1), ds, estr) Then
                Throw New Exception(estr)
            End If
            ds.Tables(0).DefaultView.RowFilter = "vSubjectID = '" + SubjectId.ToString.ToUpper.Trim() + "'"
            ds.Tables(0).DefaultView.Sort = "iRepeatNo,iSeqNo ASC"
            dtTemp = ds.Tables(0).DefaultView.ToTable().Copy()
            'If Not Me.ObjHelp.View_CRFHdrDtlSubDtl_Edit(wstr, "*", ds, estr) Then
            '    Throw New Exception(estr)
            'End If
            'If ds.Tables(0).Rows.Count = 0 Then
            '    wstr = "cActiveFlag<>'N' and vWorkSpaceId='" & WorkspaceId & "'" & _
            '            " And iNodeId=" & _
            '            NodeId & " Order by iSeqNo"

            '    If Not Me.ObjHelp.GetViewMedExWorkSpaceDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr) Then
            '        Throw New Exception(estr)
            '    End If
            'End If
            'ds.Tables.Clear()
            'ds.Tables.Add(Me.ViewState(VS_DtMedexInfoHdr))


            If ds.Tables(0).DefaultView.Count > 0 Then
                wstr = "vWorkSpaceId='" & WorkspaceId & "' and iNodeId='" & NodeId & "'" & _
                   " and vActivityId='" & ActivityId & "'"

                If Not Me.ObjHelp.GetViewWorkSpaceNodeDetail(wstr, dsNodeInfo, estr) Then
                    Me.ObjCommon.ShowAlert("Error while getting NodeIndex", Me)

                End If
                NodeIndex = dsNodeInfo.Tables(0).Rows(0)("iNodeIndex").ToString()
                wstr = "vWorkSpaceId='" & WorkspaceId & "' and iNodeId='" & NodeId & "'"
                If Not Me.ObjHelp.GetCRFHdr(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                dsCRFHdr, estr) Then
                    Throw New Exception(estr)
                End If

                If dsCRFHdr.Tables(0).Rows.Count > 0 Then
                    Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                    DtCRFHdr = dsCRFHdr.Tables(0)
                    wstr = "nCRFHdrNo = " + dsCRFHdr.Tables(0).Rows(0)("nCRFHdrNo").ToString() + " AND vSubjectId = '" + SubjectId.ToString.ToUpper.Trim() + "'"
                    If Not Me.ObjHelp.GetCRFDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            dsCRFDtl, estr) Then
                        Throw New Exception(estr)
                    End If

                Else
                    Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
                    'dsCRFHdr.Tables(0).Clear()
                    dr = DtCRFHdr.NewRow
                    'nCRFHdrNo, vWorkSpaceId,dStartDate,iPeriod,iNodeId,iNodeIndex,vActivityId,cLockStatus
                    dr("nCRFHdrNo") = 1
                    If dsCRFHdr.Tables(0).Rows.Count > 0 Then
                        dr("nCRFHdrNo") = dsCRFHdr.Tables(0).Rows(0)("nCRFHdrNo")
                    End If

                    dr("vWorkSpaceId") = HProjectId.Value.Trim()
                    'dr("dStartDate") = System.DateTime.Now
                    dr("dStartDate") = CDate(ObjCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                    dr("iPeriod") = Period
                    dr("iNodeId") = NodeId
                    dr("iNodeIndex") = NodeIndex
                    dr("vActivityId") = ActivityId
                    dr("cLockStatus") = "U" 'cLockStatus
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "N"
                    dr("dModifyon") = DateTime.Now()   '' Added by Rahul Rupareliya For Audit Trial Changes
                    'dr.AcceptChanges()
                    DtCRFHdr.Rows.Add(dr)
                    DtCRFHdr.TableName = "CRFHdr"
                    DtCRFHdr.AcceptChanges()
                End If

                If Not dsCRFDtl Is Nothing AndAlso dsCRFDtl.Tables.Count > 0 AndAlso dsCRFDtl.Tables(0).Rows.Count > 0 Then
                    DtCRFDtl = dsCRFDtl.Tables(0)

                Else
                    DtCRFDtl.Clear()
                    dr = DtCRFDtl.NewRow
                    'nCRFDtlNo,nCRFHdrNo,iRepeatNo,dRepeatationDate,vSubjectId,iMySubjectNo,cLockStatus,iWorkFlowstageId
                    dr("nCRFDtlNo") = 1
                    If DtCRFDtl.Rows.Count > 0 Then
                        dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
                    End If
                    dr("nCRFHdrNo") = DtCRFHdr.Rows(0)("nCRFHdrNo").ToString.Trim()
                    dr("iRepeatNo") = 1 'iRepeatNo
                    dr("dRepeatationDate") = CDate(ObjCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                    dr("vSubjectId") = SubjectId.ToString.ToUpper.Trim()
                    dr("iMySubjectNo") = MySubjectNo
                    dr("cLockStatus") = "U" 'cLockStatus
                    dr("iWorkFlowstageId") = Me.Session(S_WorkFlowStageId)
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "N"
                    dr("cDataStatus") = CRF_DataEntry
                    dr("dModifyon") = DateTime.Now()   '' Added by Rahul Rupareliya For Audit Trial Changes
                    'dr.AcceptChanges()
                    DtCRFDtl.Rows.Add(dr)
                    DtCRFDtl.TableName = "CRFDtl"
                    DtCRFDtl.AcceptChanges()
                End If

                'ds.Tables(0).DefaultView.RowFilter = "vSubjectID IN ('" + SubjectId + "') "
                'AND vMEDEXTYPE IN ('AsyncDateTime','AsyncTime')"
                isAsyncDate = False
                isAsyncTime = False
                For Each drRep As DataRow In DtCRFDtl.Rows
                    If dtTemp.Rows(0)("iRepeatNo").ToString <> "" Then
                        dtTemp.DefaultView.RowFilter = "iRepeatNo = " + drRep("iRepeatNo").ToString.Trim()
                    End If
                    dt = dtTemp.DefaultView.ToTable().Copy()
                    If drRep("cDataStatus") = CRF_DataEntry Then
                        For Each drAttr As DataRow In dt.Rows
                            If drAttr("vMedExType").ToString.ToUpper = "ASYNCDATETIME" AndAlso drAttr("Actual Value") = "" AndAlso isAsyncDate <> True Then
                                'If drAttr("vDefaultValue") = "" Then
                                'drAttr("")
                                'CurDate.ToString("dd/MMM/yyyy")
                                'CurDate.ToString("HH:mm")
                                dr = DtCRFSubDtl.NewRow
                                dr("nCRFSubDtlNo") = "1"
                                dr("nCRFDtlNo") = drRep("nCRFDtlNo")
                                dr("iTranNo") = "1"
                                dr("vMedExCode") = drAttr("Scheduling Code")
                                dr("vMedExDesc") = drAttr("vMedExDesc")
                                'SDNidhi
                                'dr("dMedExDateTime") = System.DateTime.Now
                                dr("dMedExDateTime") = CDate(ObjCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                                dr("vMedexResult") = CurDate.ToString("dd-MMM-yyyy")
                                dr("iModifyBy") = Me.Session(S_UserID)
                                dr("cStatusIndi") = "N"
                                dr("dModifyOn") = DateTime.Now() '' Added by Rahul Rupareliya For Audit Trial Changes

                                'If Me.CheckDiscrepancy(objControl, ObjId, Request.Form(objControl.ID), "S") Then
                                '    dr("cStatusIndi") = "A"
                                'End If
                                DtCRFSubDtl.Rows.Add(dr)
                                DtCRFSubDtl.AcceptChanges()
                                strCRFDtlNo = drRep("nCRFDtlNo").ToString.Trim()
                                isAsyncDate = True
                                'End If
                            ElseIf drAttr("vMedExType").ToString.ToUpper = "ASYNCTIME" AndAlso drAttr("Actual Value") = "" AndAlso isAsyncTime <> True Then
                                'If drAttr("vDefaultValue") = "" Then
                                'drAttr("")
                                'CurDate.ToString("dd/MMM/yyyy")
                                'CurDate.ToString("HH: mm")
                                dr = DtCRFSubDtl.NewRow
                                dr("nCRFSubDtlNo") = "1"
                                dr("nCRFDtlNo") = drRep("nCRFDtlNo")
                                dr("iTranNo") = "1"
                                dr("vMedExCode") = drAttr("Scheduling Code")
                                dr("vMedExDesc") = drAttr("vMedExDesc")
                                'SDNidhi
                                'dr("dMedExDateTime") = System.DateTime.Now
                                dr("dMedExDateTime") = CDate(ObjCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                                dr("vMedexResult") = CurDate.ToString("HH:mm")
                                dr("iModifyBy") = Me.Session(S_UserID)
                                dr("cStatusIndi") = "N"
                                dr("dModifyOn") = DateTime.Now() '' Added by Rahul Rupareliya For Audit Trial Changes

                                'If Me.CheckDiscrepancy(objControl, ObjId, Request.Form(objControl.ID), "S") Then
                                '    dr("cStatusIndi") = "A"
                                'End If
                                DtCRFSubDtl.Rows.Add(dr)
                                DtCRFSubDtl.AcceptChanges()
                                isAsyncTime = True

                                'End If
                            Else
                                dr = DtCRFSubDtl.NewRow
                                dr("nCRFSubDtlNo") = "1"
                                dr("nCRFDtlNo") = drRep("nCRFDtlNo")
                                dr("iTranNo") = "1"
                                dr("vMedexCode") = drAttr("Scheduling Code")
                                dr("vMedExDesc") = drAttr("vMedExDesc")
                                'SDNidhi
                                'dr("dMedExDateTime") = System.DateTime.Now
                                dr("dMedExDateTime") = CDate(ObjCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                                dr("vMedexResult") = drAttr("Actual Value") 'CurDate.ToString("HH:mm")
                                dr("iModifyBy") = Me.Session(S_UserID)
                                dr("cStatusIndi") = "N"
                                dr("dModifyOn") = DateTime.Now() '' Added by Rahul Rupareliya For Audit Trial Changes

                                'If Me.CheckDiscrepancy(objControl, ObjId, Request.Form(objControl.ID), "S") Then
                                '    dr("cStatusIndi") = "A"
                                'End If
                                DtCRFSubDtl.Rows.Add(dr)
                                DtCRFSubDtl.AcceptChanges()
                            End If
                        Next
                    Else
                        If iFrmDynamicPage.Visible = False Then
                            iFrmDynamicPage.Visible = True

                            'Session(S_DynamicPage_URL) = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + WorkspaceId + _
                            '     "&ActivityId=" + ActivityId.ToString() + _
                            '     "&NodeId=" + NodeId + "&PeriodId=" + Period + "&SubjectId=" + SubjectId + "&Type=BA-BE" + _
                            '      "&ScreenNo=" + SubjectId + "&MySubjectNo=" + MySubjectNo + "&From=SCH"
                            hdnDynamicURL.Value = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + WorkspaceId + _
                                 "&ActivityId=" + ActivityId.ToString() + _
                                 "&NodeId=" + NodeId + "&PeriodId=" + Period + "&SubjectId=" + SubjectId.ToString.ToUpper.Trim() + "&Type=BA-BE" + _
                                  "&ScreenNo=" + SubjectId.ToString.ToUpper.Trim() + "&MySubjectNo=" + MySubjectNo + "&From=SCH"
                            iFrmDynamicPage.Attributes("src") = hdnDynamicURL.Value

                            UpdatePanel1.Update()

                        Else
                            'Session(S_DynamicPage_URL) = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + WorkspaceId + _
                            '         "&ActivityId=" + ActivityId.ToString() + _
                            '         "&NodeId=" + NodeId + "&PeriodId=" + Period + "&SubjectId=" + SubjectId + "&Type=BA-BE" + _
                            '          "&ScreenNo=" + SubjectId + "&MySubjectNo=" + MySubjectNo + "&&From=SCH"
                            hdnDynamicURL.Value = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + WorkspaceId + _
                                     "&ActivityId=" + ActivityId.ToString() + _
                                     "&NodeId=" + NodeId + "&PeriodId=" + Period + "&SubjectId=" + SubjectId.ToString.ToUpper.Trim() + "&Type=BA-BE" + _
                                      "&ScreenNo=" + SubjectId.ToString.ToUpper.Trim() + "&MySubjectNo=" + MySubjectNo + "&&From=SCH"
                            iFrmDynamicPage.Attributes("src") = hdnDynamicURL.Value
                            UpdatePanel1.Update()
                            'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "click", "iFramebtnClicked('textbox');", True)
                        End If
                        txtScan.Text = ""
                        txtScan.Focus()
                        Exit Sub
                    End If

                Next
                If strCRFDtlNo.ToString.Trim() <> "" Then
                    DtCRFDtl.DefaultView.RowFilter = "nCRFDtlNo = " + strCRFDtlNo.ToString.Trim()
                    DtCRFDtl = DtCRFDtl.DefaultView.ToTable().Copy()
                    DtCRFSubDtl.DefaultView.RowFilter = "nCRFDtlNo = " + strCRFDtlNo.ToString.Trim()
                    DtCRFSubDtl = DtCRFSubDtl.DefaultView.ToTable().Copy()
                End If
                ds = Nothing
                ds = New DataSet
                ds.Tables.Add(DtCRFHdr.Copy())
                ds.Tables.Add(DtCRFDtl.Copy())
                ds.Tables.Add(DtCRFSubDtl.Copy())
                ds.AcceptChanges()
                If Not Me.ObjLambda.Save_CRFHdrDtlSubDtl(Choice, ds, False, Me.Session(S_UserID), estr) Then
                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Exit Sub
                End If

                If iFrmDynamicPage.Visible = False Then
                    iFrmDynamicPage.Visible = True

                    'Session(S_DynamicPage_URL) = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + WorkspaceId + _
                    '     "&ActivityId=" + ActivityId.ToString() + _
                    '     "&NodeId=" + NodeId + "&PeriodId=" + Period + "&SubjectId=" + SubjectId + "&Type=BA-BE" + _
                    '      "&ScreenNo=" + SubjectId + "&MySubjectNo=" + MySubjectNo + "&From=SCH"
                    hdnDynamicURL.Value = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + WorkspaceId + _
                         "&ActivityId=" + ActivityId.ToString() + _
                         "&NodeId=" + NodeId + "&PeriodId=" + Period + "&SubjectId=" + SubjectId.ToString.ToUpper.Trim() + "&Type=BA-BE" + _
                          "&ScreenNo=" + SubjectId.ToString.ToUpper.Trim() + "&MySubjectNo=" + MySubjectNo + "&From=SCH"
                    iFrmDynamicPage.Attributes("src") = hdnDynamicURL.Value

                    UpdatePanel1.Update()

                Else
                    'Session(S_DynamicPage_URL) = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + WorkspaceId + _
                    '         "&ActivityId=" + ActivityId.ToString() + _
                    '         "&NodeId=" + NodeId + "&PeriodId=" + Period + "&SubjectId=" + SubjectId + "&Type=BA-BE" + _
                    '          "&ScreenNo=" + SubjectId + "&MySubjectNo=" + MySubjectNo + "&&From=SCH"
                    hdnDynamicURL.Value = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + WorkspaceId + _
                             "&ActivityId=" + ActivityId.ToString() + _
                             "&NodeId=" + NodeId + "&PeriodId=" + Period + "&SubjectId=" + SubjectId.ToString.ToUpper.Trim() + "&Type=BA-BE" + _
                              "&ScreenNo=" + SubjectId.ToString.ToUpper.Trim() + "&MySubjectNo=" + MySubjectNo + "&&From=SCH"
                    iFrmDynamicPage.Attributes("src") = hdnDynamicURL.Value
                    UpdatePanel1.Update()
                    'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "click", "iFramebtnClicked('textbox');", True)
                End If


                For index = 0 To Me.gvwSubjects.Rows.Count - 1

                    If CType(Me.gvwSubjects.Rows(index).FindControl("ChkMove"), CheckBox).Checked = True Then


                        sSubjectIds += "'" + Me.gvwSubjects.Rows(index).Cells(GVCSub_SubjectID).Text + "',"

                    End If

                Next index

                fillViewState(sSubjectIds)
                'ViewState(VS_SubjectIdGunned) = ""
                Me.gvwSubjectSample.DataSource = CType(Me.ViewState(VS_DtMedexInfoHdr), DataTable).DefaultView.ToTable(True, "iMySubjectNo", "vMySubjectNo", "iRefNodeId", "Reference Time", "vSubjectId", "IsDataEntryDone", "DataStatus", "ReplaceSubject")
                'Me.gvwSubjectSample.DataSource = DtSubjects.Copy()
                Me.gvwSubjectSample.DataBind()
                canal.Visible = True

            End If

            Me.txtScan.Text = ""
            Me.txtScan.Focus()
            '  Me.txtScan.Enabled = True

        Catch ex As Exception
            Me.txtScan.Text = ""
            Me.txtScan.Focus()
            Me.ShowErrorMessage("Error While btnSaveandRedirect_Click.", ex.Message)
        End Try

    End Sub


#Region "Other functions"
    Private Function ClearControls() As Boolean
        Try
            hdnFrom.Value = ""
            ViewState(VS_SubjectIdGunned) = ""
            canal.Visible = False
            gvwSubjectSample.DataSource = Nothing
            gvwSubjectSample.DataBind()
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....ClearControls")
            Return False
        End Try
    End Function
    Private Function ResetPage() As Boolean
        Try
            txtScan.Text = ""
            txtScan.Focus()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....ClearControls")
            Return False
        End Try
    End Function
#End Region


    Protected Sub btnForceKillPage_Click(sender As Object, e As EventArgs) Handles btnForceKillPage.Click
        Try
            txtScan.Text = ""
            txtScan.Focus()
        Catch ex As Exception

        End Try
    End Sub
End Class
