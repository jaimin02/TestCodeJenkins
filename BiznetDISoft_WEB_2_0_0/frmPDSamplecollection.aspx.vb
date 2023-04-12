
Partial Class frmPDSamplecollection
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION"

    Private ObjCommon As New clsCommon
    Private ObjHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private ObjLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const VS_dtViewPDSampleDetail As String = "View_PDSampleDetail"
    Private Const VS_DtSave As String = "Dt_Save"
    Private Const VS_DtMedexInfoHdr As String = "DtMedexInfoHdr"

    Private Const VS_DtViewWorkSpaceSubjectMst As String = "View_WorkSpaceSubjectMst"
    Private Const VS_DtView_WorkSpaceNodeDetail As String = "View_WorkSpaceNodeDetail"
    Private Const VS_DtCRFHdr As String = "DtCRFHdr"
    Private Const VS_DtCRFDtl As String = "DtCRFDtl"
    Private Const VS_DtCRFSubDtl As String = "DtCRFSubDtl"
    Private Const VS_CollectionTime As String = "CollectionTime"

    'nSampleId,vSampleId,vSubjectID,FullName,dCollectionDateTime,iCollectionBy,iMySubjectNo,iRefNodeId,nRefTime
    Private Const GVC_vSampleId As Integer = 0
    Private Const GVC_vSampleBarcode As Integer = 1
    Private Const GVC_MySubjectNo As Integer = 2
    Private Const GVC_vMySubjectNo As Integer = 3
    Private Const GVC_SubjectID As Integer = 4
    Private Const GVC_vInitials As Integer = 5
    Private Const GVC_RefNodeId As Integer = 6
    Private Const GVC_RefTime As Integer = 7
    Private Const GVC_CollectionDateTime As Integer = 8
    Private Const GVC_iCollectionBy As Integer = 9
    Private Const GVC_vCollectionBy As Integer = 10
    Private Const GVC_DosingTime As Integer = 11
    'Private Const GVC_Remarks As Integer = 12
    Private Const GVC_nSampleId As Integer = 12
    Private Const GVC_iMySubjectNoNew As Integer = 13


    Private Const GVCSub_Select As Integer = 0
    Private Const GVCSub_WorkspaceId As Integer = 1
    Private Const GVCSub_MySubjectNo As Integer = 2
    Private Const GVCSub_vMySubjectNo As Integer = 3
    Private Const GVCSub_SubjectID As Integer = 4
    Private Const GVCSub_FullName As Integer = 5

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Page.Title = Me.Session(S_UserName) + " :: PDSample Collection :: Lambda Therapeutic Research Ltd."

        If Not IsPostBack Then
            If Not GenCall() Then
                Exit Sub
            End If
        End If

    End Sub

#End Region

#Region "GenCall()"

    Private Function GenCall() As Boolean
        Try

            CType(Me.Master.FindControl("form1"), HtmlForm).DefaultButton = Me.btndefault.UniqueID

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

#End Region

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
            Me.ShowErrorMessage(ex.Message, "...GenCall_Data")
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

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "PD Sample Collection"
            Page.Title = ":: PD Sample Collection :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
                GenCall_ShowUI = True
                Exit Function
            End If
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)

            If Not FillddlUser() Then
                Exit Function
            End If


            Me.ddlUser.SelectedValue = Me.Session(S_UserID)

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_ShowUI")
        End Try

    End Function

#End Region

#Region "Fill Function"

    Private Function FillddlPeriod() As Boolean
        Dim ds_WorkSpaceNodeDetail As New Data.DataSet
        Dim dv_WorkSpaceNodeDetail As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty

        Try


            wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' And cStatusIndi <> 'D'"

            If Not ObjHelp.GetViewWorkSpaceNodeDetail(wstr, ds_WorkSpaceNodeDetail, estr) Then
                Throw New Exception("Error While Getting Data from WorkSpaceNodeDetail : " + estr)
            End If

            Me.ViewState(VS_DtView_WorkSpaceNodeDetail) = ds_WorkSpaceNodeDetail.Tables(0).Copy()

            dv_WorkSpaceNodeDetail = ds_WorkSpaceNodeDetail.Tables(0).DefaultView
            dv_WorkSpaceNodeDetail.Sort = "iPeriod"

            Me.ddlPeriod.DataSource = dv_WorkSpaceNodeDetail.ToTable().DefaultView.ToTable(True, "iPeriod".ToString())
            Me.ddlPeriod.DataValueField = "iPeriod"
            Me.ddlPeriod.DataTextField = "iPeriod"
            Me.ddlPeriod.DataBind()
            Me.ddlPeriod.Items.Insert(0, New ListItem("--Select Period--", 0))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillddlPeriod")
            Return False
        End Try
    End Function
    Private Function FillddlActivity() As Boolean
        Dim wstr As String = String.Empty
        Dim Ds_FillActivity As New DataSet
        Dim Dv_FillActivity As New DataView
        Dim Dt_FillActivity As New DataTable
        Dim eStr_Retu As String = String.Empty
        Try

            wstr = " vWorkspaceId = '" & Me.HProjectId.Value.Trim() & "'And iperiod=" + Me.ddlPeriod.SelectedItem.Text
            wstr += " And cStatusindi<>'D'"


            If Not ObjHelp.Get_ViewSampleDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                               Ds_FillActivity, eStr_Retu) Then

                Throw New Exception(eStr_Retu)

            End If
            Dv_FillActivity = Ds_FillActivity.Tables(0).DefaultView
            Dt_FillActivity = Dv_FillActivity.ToTable(True, "iNodeId,vNodeDisplayName".Split(","))

            If Dt_FillActivity.Rows.Count <= 0 Then
                ObjCommon.ShowAlert("No Safety Sample is created for selected Project.", Me)
                Return True
                Exit Function
            End If

            Me.ddlActivity.DataSource = Dt_FillActivity
            Me.ddlActivity.DataTextField = "vNodeDisplayName"
            Me.ddlActivity.DataValueField = "iNodeid"
            Me.ddlActivity.DataBind()


            Me.ddlActivity.Items.Insert(0, New ListItem("Select Activity", 0))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillddlActivity")
            Return False
        End Try
    End Function

#End Region

    Private Function FillddlUser() As Boolean
        Dim ds_User As New Data.DataSet
        Dim dt_User As New DataTable
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty

        Try
            'Commented and added by Parth Modi on dated 18-August-2014
            'Reason: Add Location filter on User list
            'wstr = "nScopeNo = " & Me.Session(S_ScopeNo) & " And cStatusIndi <> 'D' And vUserTypeCode = '" + Me.Session(S_UserType) + "'"
            wstr = "nScopeNo = " & Me.Session(S_ScopeNo) & " And cStatusIndi <> 'D' And vUserTypeCode = '" + Me.Session(S_UserType) + "' AND vLocationCode = '" + Me.Session(S_LocationCode).ToString + "'"
            'END
            wstr += " Order by vUserName"

            If Not ObjHelp.GetViewUserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        ds_User, estr) Then
                Throw New Exception("Error While Getting Data From View_UserMst:" + estr)
            End If

            For CntOfDs_User As Integer = 0 To ds_User.Tables(0).Rows.Count - 1
                ds_User.Tables(0).Rows(CntOfDs_User).Item("vUserName") = ds_User.Tables(0).Rows(CntOfDs_User).Item("vUserName").ToString() + "   " + "(" + ds_User.Tables(0).Rows(CntOfDs_User).Item("vUserTypeName").ToString() + ")"
            Next CntOfDs_User

            ds_User.Tables(0).AcceptChanges()
            dt_User = ds_User.Tables(0).DefaultView.ToTable(True, "iUserId,vUserName".Split(","))

            Me.ddlUser.DataSource = dt_User
            Me.ddlUser.DataValueField = "iUserId"
            Me.ddlUser.DataTextField = "vUserName"
            Me.ddlUser.DataBind()
            Me.ddlUser.Items.Insert(0, New ListItem("--Select User--", 0))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillddlUser")
            Return False
        End Try

    End Function

    Private Function FillGrid() As Boolean
        Dim ds_ViewPDSampleDetail As New Data.DataSet
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim Subjects As String = String.Empty
        Dim dtUserSubject As New Data.DataTable
        Dim dvUserSubject As New Data.DataView

        Try


            If IsNothing(Me.Session("UserSubjectDtl")) Then

                Me.ObjCommon.ShowAlert("Please, First Do Subject Management", Me.Page())
                Exit Function

            Else

                dvUserSubject = CType(Me.Session("UserSubjectDtl"), DataTable).Copy().DefaultView()
                dvUserSubject.RowFilter = "vWorkspaceId = '" + Me.HProjectId.Value + "' And iUserId=" + _
                                                     Me.Session(S_UserID)
                dtUserSubject = dvUserSubject.ToTable()

                If dtUserSubject.Rows.Count <= 0 Then
                    Me.ObjCommon.ShowAlert("Please, First Do Subject Management", Me.Page())
                    Exit Function
                End If

            End If

            Wstr = "vWorkspaceId = '" + Me.HProjectId.Value + "' And cStatusIndi <> 'D'"
            Wstr += " And iNodeId = " + Me.ddlActivity.SelectedValue.Trim.ToString() + " And iPeriod = " + _
                        Me.ddlPeriod.SelectedValue.Trim().ToString()

            dtUserSubject = CType(Me.Session("UserSubjectDtl"), DataTable)
            dvUserSubject = dtUserSubject.DefaultView
            dvUserSubject.RowFilter = "vWorkspaceId = '" + Me.HProjectId.Value + "'"
            dtUserSubject = dvUserSubject.ToTable()

            For Each dr As DataRow In dtUserSubject.Rows()
                Subjects += IIf(Subjects = "", "'" & dr("iMySubjectNo"), "','" & dr("iMySubjectNo"))
            Next

            Subjects += "'"

            Wstr += " And iMySubjectNo in (" & Subjects & ") order by iMySubjectNo"

            If Not ObjHelp.view_PDsampledetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_ViewPDSampleDetail, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From View_PDSampleDetail : ", estr)
                Exit Function
            End If

            Me.ViewState(VS_dtViewPDSampleDetail) = ds_ViewPDSampleDetail.Tables(0)

            If ds_ViewPDSampleDetail.Tables(0).Rows.Count > 0 Then

                Me.txtScan.Enabled = True

                fillViewState()
                '' ds_ViewPDSampleDetail.Tables(0).DefaultView().RowFilter = "cSampleTypeCode=''"

                Me.gvwSubjectSample.DataSource = ds_ViewPDSampleDetail.Tables(0).DefaultView.ToTable()
                Me.gvwSubjectSample.DataBind()
                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetPDSample", "fsetPDSample_Show(); ", True)
                Return True
            End If
            Me.gvwSubjectSample.EmptyDataText = "No Record Found"
            'Me.gvwSubjectSample.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetPDSample", "fsetPDSample_Show(); ", True)
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillGrid")
            Return False
        End Try
    End Function

    Private Function FillSubjectGrid() As Boolean
        Dim ds_Subjects As New Data.DataSet
        Dim dtOldSubjects As New Data.DataTable
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty

        Try
            Me.HsubjectId.Value = ""




            Wstr = "vWorkspaceId = '" + Me.HProjectId.Value + "' And cStatusIndi <> 'D' "
            Wstr += " And iPeriod = " + Me.ddlPeriod.SelectedItem.Value.Trim() + " And cRejectionFlag <> 'Y' And iMySubjectNo >0 "
            Wstr += " order by iMySubjectNo"

            If Not ObjHelp.GetViewWorkspaceSubjectMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        ds_Subjects, estr) Then
                Throw New Exception("Error While Getting Data From View_SampleDetail : " + estr)
            End If

            Me.ViewState(VS_DtViewWorkSpaceSubjectMst) = ds_Subjects.Tables(0).Copy()

            If ds_Subjects.Tables(0).Rows.Count > 0 Then
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

                                Me.HsubjectId.Value += Me.gvwSubjects.Rows(index).Cells(GVCSub_SubjectID).Text.ToString.Trim() + ","
                            End If

                        Next DrSubjects
                    Next index
                    If Me.HsubjectId.Value <> "" Then
                        Me.HsubjectId.Value = Me.HsubjectId.Value.Substring(0, Me.HsubjectId.Value.LastIndexOf(","))
                    End If

                End If

                Return True
            End If

            Me.gvwSubjects.DataSource = Nothing
            Me.gvwSubjects.DataBind()
            Me.ObjCommon.ShowAlert("Records Not Found.", Me.Page)

            Return False

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillSubjectGrid")
            Return False
        End Try
    End Function

    Private Function fillViewState() As Boolean
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_RefTime As New DataSet
        Dim strDate As String = String.Empty
        Dim Columns As String = String.Empty
        Dim ds_WorkspaceSubjectMst As New DataSet
        Try

            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
            wStr += " And iPeriod = " + Me.ddlPeriod.SelectedItem.Value.Trim()
            wStr += " And vMedExCode in('" + Medex_Date + "','" + Medex_Time + "')"

            Columns = "vWorkSpaceId,iPeriod,vActivityId,iNodeId,vSubjectId,vSubjectName,iMySubjectNo,nRefTIme,vMedExCode,vDefaultValue,iMySubjectNoForPdSample"

            If Not ObjHelp.View_CRFHdrDtlSubDtl_Edit(wStr, Columns, ds_RefTime, eStr) Then
                Throw New Exception("Error While Getting Data From View_CRFHdrDtlSubDtl_Edit : " + eStr)
            End If


            Me.ViewState(VS_DtMedexInfoHdr) = ds_RefTime.Tables(0).Copy()

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....fillViewState")
            Return False
        End Try

    End Function

    Private Function CheckSubject() As Boolean
        Dim ds_ViewCRFDtl As New DataSet
        Dim dt_User As New DataTable
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty

        Try
            wstr = " vWorkspaceID = '" + Me.HProjectId.Value + "' and iNodeId = " + Me.ddlActivity.SelectedValue + " and vSubjectId = '" + Me.lblSubject.Text.Trim + "' and cStatusIndiDtl <>'D'"
            If Not ObjHelp.View_CRFDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ViewCRFDtl, estr) Then
                Throw New Exception(estr)
            End If

            If ds_ViewCRFDtl.Tables(0).Rows.Count > 0 Then
                Return False
            Else
                Return True
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....CheckSubject")
            Return False
        End Try

    End Function

#Region "SELECTION CHANGE EVENT"

    Protected Sub ddlPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Not FillddlActivity() Then
            Exit Sub
        End If

    End Sub

#End Region

#Region "BUTTON EVENT"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim eStr_Retu As String = String.Empty
        Dim Ds_FillActivity As New DataSet
        Dim wstr As String = String.Empty
        Dim ds_CrfVersionDetail As DataSet = Nothing
        Dim VersionNo As Integer = 0
        Dim VersionDate As Date
        Try


            ''====== CRFVersion Control==================================
            wstr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'"

            If Not ObjHelp.GetData("View_CRFVersionForDataEntryControl", "*", wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CrfVersionDetail, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            If ds_CrfVersionDetail.Tables(0).Rows.Count > 0 Then
                VersionNo = ds_CrfVersionDetail.Tables(0).Rows(0)("nVersionNo")
                VersionDate = ds_CrfVersionDetail.Tables(0).Rows(0)("dVersiondate").ToString
                Me.VersionNo.Text = VersionNo.ToString
                Me.VersionDate.Text = VersionDate.ToString("dd-MMM-yyyy")
                Me.VersionDtl.Attributes.Add("style", "''")
                If ds_CrfVersionDetail.Tables(0).Rows(0)("cFreezeStatus").ToString.Trim.ToUpper = "U" Then
                    ImageLockUnlock.Attributes.Add("src", "images/UnFreeze.jpg")
                    ObjCommon.ShowAlert("Project Is In UnFreeze State, First Freeze It Then Proceed", Me.Page)
                    Exit Sub
                End If
            Else
                Me.VersionDtl.Attributes.Add("style", "display:none;")
            End If
            ''==========================================================

            If Not FillddlPeriod() Then
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnSetProject_Click")
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        If Not FillGrid() Then
            Exit Sub
        End If

        Me.UpControls.Update()
        Me.txtScan.Text = ""
        Me.lblSample.Text = ""
        Me.lblSubject.Text = ""
        Me.lblMySubject.Text = ""
        Me.txtScan.Focus()

    End Sub

    Protected Sub btnSubjectMgmt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubjectMgmt.Click

        If Not FilldllRemarks() Then
            Exit Sub
        End If

        If Me.hfTextChnaged.Value.ToUpper.Trim() = "BARCODE" Then
            Me.hfTextChnaged.Value = ""
            Exit Sub
        End If

        If Not FillSubjectGrid() Then
            Exit Sub
        End If

        Me.MPESubMgmt.Show()

    End Sub

    Protected Sub btnSaveSubject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveSubject.Click
        Try
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                Me.btn_ok_Click(sender, e)
                Exit Sub
            End If
            If Not CheckSequence(Me.HProjectId.Value.Trim(), Me.ddlActivity.SelectedValue, Me.HsubjectId.Value.Trim(), Me.ddlPeriod.SelectedValue) Then
                Me.MPEActivitySequence.Hide()
                Exit Sub
            End If

            If Me.HPendingNode.Value = "" Then
                Me.btn_ok_Click(sender, e)
                Exit Sub
            End If

            Me.txtContent.Text = ""
            Me.MPEActivitySequence.Show()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnSaveSubject_Click")
        End Try

    End Sub

    Protected Sub btn_ok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ok.Click
        Dim DtSubjects As New DataTable
        Dim DrSubjects As DataRow
        Dim index As Integer = 0

        Try

            If IsNothing(Me.Session("UserSubjectDtl")) Then

                If Not CreateSubjectTable(DtSubjects) Then
                    Exit Sub
                End If

            Else

                DtSubjects = CType(Me.Session("UserSubjectDtl"), DataTable)

            End If

            DtSubjects.Clear()

            For index = 0 To Me.gvwSubjects.Rows.Count - 1

                If CType(Me.gvwSubjects.Rows(index).FindControl("ChkMove"), CheckBox).Checked = True Then

                    DrSubjects = DtSubjects.NewRow
                    DrSubjects("iUserId") = Me.Session(S_UserID)
                    DrSubjects("vWorkspaceId") = Me.gvwSubjects.Rows(index).Cells(GVCSub_WorkspaceId).Text
                    DrSubjects("vSubjectId") = Me.gvwSubjects.Rows(index).Cells(GVCSub_SubjectID).Text

                    If Me.gvwSubjects.Rows(index).Cells(GVCSub_MySubjectNo).Text.Length > 4 Then
                        DrSubjects("iMySubjectNo") = Me.gvwSubjects.Rows(index).Cells(GVCSub_MySubjectNo).Text.ToString().Remove(4, (Me.gvwSubjects.Rows(index).Cells(GVCSub_MySubjectNo).Text.ToString().Length) - 4)
                    Else
                        DrSubjects("iMySubjectNo") = Me.gvwSubjects.Rows(index).Cells(GVCSub_MySubjectNo).Text.ToString()
                    End If
                    DtSubjects.Rows.Add(DrSubjects)
                    DtSubjects.AcceptChanges()

                End If

            Next index

            If IsNothing(Me.Session("UserSubjectDtl")) Then
                Me.Session.Add("UserSubjectDtl", DtSubjects)
            Else
                Me.Session("UserSubjectDtl") = DtSubjects
            End If

            btnSearch_Click(sender, e)

            Me.UpGridSubjectSample.Update()
            Me.UpOtherControls.Update()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Private Function CreateSubjectTable(ByRef Dt As DataTable) As Boolean
        Dt = Nothing
        Dt = New DataTable()

        Dt.Columns.Add(New DataColumn("iUserId", GetType(Integer)))
        Dt.Columns.Add(New DataColumn("vWorkspaceId", GetType(String)))
        Dt.Columns.Add(New DataColumn("vSubjectId", GetType(String)))
        Dt.Columns.Add(New DataColumn("iMySubjectNo", GetType(String)))
        Dt.AcceptChanges()
        Return True

    End Function

    Protected Sub btnReplace_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReplace.Click

    End Sub

    'Protected Sub btnReplaceCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReplaceCancel.Click
    '    Me.btnSubjectMgmt.Enabled = True
    'End Sub

    'Protected Sub btnReplaceOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReplaceOK.Click

    '    Dim ds_PKSample As New DataSet
    '    Dim wstr As String = ""
    '    Dim estr As String = ""

    '    Dim tempSubjectNo As String = ""

    '    Dim Retu_nPKSampleBarCode As String = ""
    '    Dim Retu_vPKSampleBarCode As String = ""
    '    Try

    '        wstr = "vPKSampleId in ('" & Me.lblReplaceCode.Text.Trim() & "','" & Me.lbReplaceWith.Text.Trim() & "')"

    '        If Not Me.ObjHelp.PKSampleDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                ds_PKSample, estr) Then

    '            Me.ObjCommon.ShowAlert("Error while getting Data", Me.Page())
    '            Exit Sub

    '        End If

    '        If ds_PKSample.Tables(0).Rows.Count > 1 Then

    '            tempSubjectNo = ds_PKSample.Tables(0).Rows(0)("iMySubjectNo").ToString.Trim()
    '            ds_PKSample.Tables(0).Rows(0)("iMySubjectNo") = ds_PKSample.Tables(0).Rows(1)("iMySubjectNo")
    '            ds_PKSample.Tables(0).Rows(1)("iMySubjectNo") = tempSubjectNo

    '            ds_PKSample.Tables(0).Rows(0)("vRemark") = Me.txtReplaceRemark.Text.Trim()
    '            ds_PKSample.Tables(0).Rows(1)("vRemark") = Me.txtReplaceRemark.Text.Trim()

    '            ds_PKSample.Tables(0).AcceptChanges()

    '            If Not ObjLambda.Save_PKSampleDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_PKSample, _
    '                                        Me.Session(S_UserID), Retu_nPKSampleBarCode, Retu_vPKSampleBarCode, estr) Then

    '                Me.ShowErrorMessage("Error While Saving Data in SampleDetail", estr)
    '                Me.resetpage()
    '                Exit Sub

    '            End If

    '        End If

    '        btnSearch_Click(sender, e)

    '        Me.btnSubjectMgmt.Enabled = True

    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.Message, "")
    '    End Try

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

#Region "Grid Events"

    Protected Sub gvwSubjectSample_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwSubjectSample.RowCreated
        If e.Row.RowType = DataControlRowType.Header Or _
                    e.Row.RowType = DataControlRowType.DataRow Or _
                    e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GVC_vSampleId).Visible = False
            e.Row.Cells(GVC_RefNodeId).Visible = False
            e.Row.Cells(GVC_iCollectionBy).Visible = False
            e.Row.Cells(GVC_DosingTime).Visible = False
            e.Row.Cells(GVC_MySubjectNo).Visible = False
            e.Row.Cells(GVC_MySubjectNo).Visible = False
            e.Row.Cells(GVC_iMySubjectNoNew).Visible = False

        End If
    End Sub

    Protected Sub gvwSubjectSample_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwSubjectSample.RowDataBound
        Dim index As Integer = e.Row.RowIndex
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim dt_RefTime As New DataTable
        Dim dv_RefTime As New DataView
        Dim refTime As Date
        Dim time1() As String
        Dim time As String = String.Empty
        Dim strDate As String = String.Empty
        Dim dt_WorkspaceSubjectMst As New DataTable
        Dim dv_WorkspaceSubjectMst As New DataView

        Dim dv_ViewSampleDetail As New DataView
        Dim dt_ViewSampleDetail As New DataView
        Dim CollectionDateTime As DateTime
        Try
            e.Row.Cells(GVC_nSampleId).Visible = False
            If e.Row.RowType = DataControlRowType.DataRow Then
                'CType(e.Row.FindControl("lnkReplace"), LinkButton).CommandArgument = e.Row.RowIndex
                'CType(e.Row.FindControl("lnkReplace"), LinkButton).CommandName = "REPLACE"

                e.Row.Cells(GVC_CollectionDateTime).Text = Replace(e.Row.Cells(GVC_CollectionDateTime).Text.Trim(), "&nbsp;", "")

                If Not Convert.ToString(e.Row.Cells(GVC_CollectionDateTime).Text).Trim = "" Then
                    CollectionDateTime = CType(e.Row.Cells(GVC_CollectionDateTime).Text.Trim, Date)
                    e.Row.Cells(GVC_CollectionDateTime).Text = CollectionDateTime.ToString("dd-MMM-yyyy HH:mm") + strServerOffset 'Added By Parth Pandya for Change in Time Format
                    'e.Row.Cells(GVC_CollectionDateTime).Text = CType(e.Row.Cells(GVC_CollectionDateTime).Text.Trim(), Date).GetDateTimeFormats()(25).Trim()
                End If

                dv_RefTime = Nothing
                dv_RefTime = New DataView
                dv_RefTime = CType(Me.ViewState(VS_DtView_WorkSpaceNodeDetail), DataTable).Copy().DefaultView()
                'Added by akhilesh for first adding the fiter of Working on Predose Activity 
                dv_RefTime.RowFilter = "iNodeId = " + Me.ddlActivity.SelectedValue.ToString.Trim() + " And cISPredose <> 'Y'"
                dt_RefTime = dv_RefTime.ToTable()

                If dt_RefTime.Rows.Count > 0 Then
                    dv_RefTime = Nothing
                    dv_RefTime = New DataView
                    dv_RefTime = CType(Me.ViewState(VS_DtMedexInfoHdr), DataTable).Copy().DefaultView()
                    'Added by akhilesh for first adding the fiter of Working on Predose Activity 
                    dv_RefTime.RowFilter = "iNodeId = " + e.Row.Cells(GVC_RefNodeId).Text.Trim() + _
                                           " And iMySubjectNo = " + e.Row.Cells(GVC_iMySubjectNoNew).Text.Trim()
                    dt_RefTime = dv_RefTime.ToTable()

                    If dt_RefTime.Rows.Count > 0 Then

                        strDate = dt_RefTime.Rows(0).Item("vDefaultValue").ToString.Trim() + " "
                        strDate += dt_RefTime.Rows(1).Item("vDefaultValue").ToString.Trim()

                        refTime = CType(strDate.Trim(), Date)
                        e.Row.Cells(GVC_DosingTime).Text = refTime.ToString("dd-MMM-yyyy HH:mm") + strServerOffset
                        time = CType(e.Row.Cells(GVC_RefTime).Text.Trim(), String)
                        time1 = time.Split(".")

                        If Not time1(1) = 0 Then
                            refTime = refTime.AddHours(((time1(0) * 60) + time1(1)) / 60)
                        Else
                            refTime = refTime.AddHours(time1(0))
                        End If
                        e.Row.Cells(GVC_RefTime).Text = refTime.ToString("dd-MMM-yyyy HH:mm") + strServerOffset
                        e.Row.Cells(GVC_SubjectID).Text = dt_RefTime.Rows(0).Item("vSubjectId").ToString.Trim()
                        e.Row.Cells(GVC_vInitials).Text = dt_RefTime.Rows(0).Item("vSubjectName").ToString.Trim()

                    Else
                        e.Row.Cells(GVC_RefTime).Text = ""
                        e.Row.Cells(GVC_DosingTime).Text = ""
                    End If

                    dv_WorkspaceSubjectMst = Nothing
                    dv_WorkspaceSubjectMst = New DataView
                    dv_WorkspaceSubjectMst = CType(Me.ViewState(VS_DtViewWorkSpaceSubjectMst), DataTable).Copy().DefaultView()

                    dv_WorkspaceSubjectMst.RowFilter = "iMySubjectNoNew = " + e.Row.Cells(GVC_MySubjectNo).Text.Trim()
                    dt_WorkspaceSubjectMst = dv_WorkspaceSubjectMst.ToTable()

                    If dt_WorkspaceSubjectMst.Rows.Count > 0 Then
                        e.Row.Cells(GVC_SubjectID).Text = dt_WorkspaceSubjectMst.Rows(0).Item("vSubjectId").ToString.Trim()
                        e.Row.Cells(GVC_vInitials).Text = dt_WorkspaceSubjectMst.Rows(0).Item("vInitials").ToString.Trim()
                    End If
                Else
                    e.Row.Cells(GVC_RefTime).Text = ""
                    e.Row.Cells(GVC_DosingTime).Text = ""
                End If
                If Me.hndLockStatus.Value.Trim() = "Lock" Then
                    Me.txtScan.Attributes.Add("Disabled", "Disabled")
                End If
                If Me.hndLockStatus.Value.Trim() = "UnLock" Then
                    Me.txtScan.Attributes.Remove("Disabled")
                End If

            End If


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....gvwSubjectSample_RowDataBound")
        End Try
    End Sub

    'Protected Sub gvwSubjectSample_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwSubjectSample.RowCommand

    '    Dim index As Integer = CType(e.CommandArgument, Integer)

    '    If e.CommandName.ToUpper.Trim() = "REPLACE" Then

    '        Me.lblReplaceCode.Text = Me.gvwSubjectSample.Rows(index).Cells(GVC_vSampleBarcode).Text.Trim()
    '        Me.txtReplaceRemark.Text = ""
    '        Me.lbReplaceWith.Text = ""
    '        Me.txtreplaceCode.Focus()
    '        Me.MPEReplacement.Show()
    '        Me.btnSubjectMgmt.Enabled = False

    '    End If

    'End Sub




#End Region

#Region "TextBox Events"
    Protected Sub txtScan_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtScan.TextChanged
        Dim ScanText As String = String.Empty
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds As New DataSet
        Dim ds_Save As New DataSet
        Dim SubjectID As String = String.Empty
        Dim ValidSubject As Boolean = False
        Dim time1() As String
        Dim time As String = String.Empty
        Dim ds_ViewCRFDtl As New DataSet
        'Variables Deviation checking
        Dim dt As New DataTable
        Dim dv As New DataView
        Dim dt_RefTime As New DataTable
        Dim dv_RefTime As New DataView
        Dim strDate As String = String.Empty
        Dim refTime As DateTime

        Try
            Me.hfTextChnaged.Value = "BARCODE"


            ScanText = Me.txtScan.Text.Trim

            If (ScanText.Contains("-")) Or (ScanText.Length < 8) Then

                Me.lblSubject.Text = ScanText.ToString()

                For index As Integer = 0 To Me.gvwSubjectSample.Rows.Count - 1

                    ValidSubject = False
                    If Me.gvwSubjectSample.Rows(index).Cells(GVC_SubjectID).Text.ToUpper.Trim() = ScanText.ToString().ToUpper.Trim() Then
                        ValidSubject = True
                        Exit For
                    End If

                Next

                If ValidSubject = False Then
                    ObjCommon.ShowAlert("Subject Is Not Valid For You", Me.Page)
                    Me.resetpage()
                    Exit Sub
                End If

                If Not Me.ViewState(VS_DtSave) Is Nothing Then

                    If ds_Save.Tables.Count < 1 Then
                        ds_Save.Tables.Add(CType(Me.ViewState(VS_DtSave), DataTable).Copy())
                        'Checking whether the subject belongs to same workspace or not
                    End If


                    'To Validate Subject
                    If Not IsNothing(ds_Save.Tables(0).Columns.Contains("vSubjectID")) AndAlso _
                        Not (ds_Save.Tables(0).Rows(0).Item("vSubjectID") Is System.DBNull.Value) AndAlso _
                        ds_Save.Tables(0).Rows(0).Item("vSubjectID").ToString.Trim() <> "" Then

                        SubjectID = ScanText.ToString()

                        'If SubjectID <> Convert.ToString(ds_Save.Tables(0).Rows(0)("vSubjectID")) Then
                        '    ObjCommon.ShowAlert("Subject Is Not Valid For This Sample. This Sample Is For " + _
                        '                        ds_Save.Tables(0).Rows(0)("vSubjectID").ToString.Trim(), Me.Page)
                        '    Me.resetpage()
                        '    Exit Sub
                        'End If

                    End If
                    '***********************************************

                    If ds_Save.Tables(0).Rows(0).Item("vWorkSpaceId").ToString = Pro_Screening Then
                        wStr = " vSubjectID = '" + Me.lblSubject.Text.Trim + "'"
                        wStr += " And cRejectionFlag <> 'Y'"

                        If Not ObjHelp.GetView_SubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr) Then
                            Me.ShowErrorMessage("Error While Getting Data From View_SubjectMaster", eStr)
                            Me.resetpage()
                            Exit Sub
                        End If

                    Else

                        wStr = "vWorkSpaceId = '" + ds_Save.Tables(0).Rows(0).Item("vWorkSpaceId").ToString + "' And"
                        wStr += " vSubjectID = '" + Me.lblSubject.Text.Trim + "'"
                        wStr += " And cRejectionFlag <> 'Y'"

                        ds.Tables.Clear()
                        ds.AcceptChanges()
                        dv = CType(Me.ViewState(VS_DtViewWorkSpaceSubjectMst), DataTable).Copy().DefaultView
                        dv.RowFilter = wStr
                        ds.Tables.Add(dv.ToTable())
                        ds.AcceptChanges()

                        If Not ds.Tables(0).Rows.Count < 1 Then
                            Me.lblMySubject.Text = ds.Tables(0).Rows(0)("vMySubjectNo").ToString()
                        End If

                    End If

                Else

                    '==added on 29-jan-10= by Deepak Singh to add a filter of SubjectID =
                    'Added For "MySubjectNo" on 27-Jun-2009
                    wStr = "vWorkSpaceId = '" + Me.HProjectId.Value.ToString + "' And"
                    wStr += " vSubjectID = '" + Me.lblSubject.Text.Trim + "'"
                    wStr += " And cRejectionFlag <> 'Y'"

                    ds.Tables.Clear()
                    ds.AcceptChanges()
                    dv = CType(Me.ViewState(VS_DtViewWorkSpaceSubjectMst), DataTable).Copy().DefaultView
                    dv.RowFilter = wStr
                    ds.Tables.Add(dv.ToTable())
                    ds.AcceptChanges()

                    If ds.Tables(0).Rows.Count > 0 Then

                        If Not ds.Tables(0).Rows(0).Item("vWorkSpaceId").ToString() Is Nothing Or _
                                    ds.Tables(0).Rows(0).Item("vWorkSpaceId").ToString() = "" Then

                            Me.lblMySubject.Text = ds.Tables(0).Rows(0)("vMySubjectNo").ToString()

                        End If

                    Else

                        wStr = " vSubjectID = '" + Me.lblSubject.Text.Trim + "'"
                        wStr += " And cRejectionFlag <> 'Y'"

                        If Not ObjHelp.GetView_SubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr) Then
                            Me.ShowErrorMessage("Error While Getting Data From View_SubjectMaster", eStr)
                            Me.resetpage()
                            Exit Sub
                        End If

                        If Not ds.Tables(0).Rows(0).Item("vWorkSpaceId").ToString() Is Nothing Or _
                                    ds.Tables(0).Rows(0).Item("vWorkSpaceId").ToString() = "" Then

                            Me.lblMySubject.Text = "Not Assigned"
                        End If

                    End If

                    '**************************************************
                End If

                If ds.Tables(0).Rows.Count < 1 Then
                    ObjCommon.ShowAlert("Subject Is Not Valid", Me.Page)
                    Me.resetpage()
                    Exit Sub
                End If
                ds = Nothing
                If Not CheckSubject() Then
                    ObjCommon.ShowAlert("Sample For Selected Subject Is Already Collected From eCRF Page.", Me.Page)
                    Me.resetpage()
                    Exit Sub
                End If
                'wStr = " vWorkspaceID = '" + Me.HProjectId.Value + "' and iNodeId = " + Me.ddlActivity.SelectedValue + " and vSubjectId = '" + Me.lblSubject.Text.Trim + "' and cStatusIndiDtl <>'D'"
                'If Not ObjHelp.View_CRFDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ViewCRFDtl, eStr) Then
                '    Throw New Exception(eStr)
                'End If

                'If ds_ViewCRFDtl.Tables(0).Rows.Count > 0 Then
                '    ObjCommon.ShowAlert("Sample For Selected Subject Is Already Collected From eCRF Page.", Me.Page)
                '    Me.resetpage()
                '    Exit Sub
                'End If
                Me.rblCollectionStatus.ClearSelection()
                Me.MPEFasting.Show()
            Else

                For index1 As Integer = 0 To Me.gvwSubjectSample.Rows.Count - 1

                    ValidSubject = False
                    If Me.gvwSubjectSample.Rows(index1).Cells(GVC_vSampleBarcode).Text.ToUpper.Trim() = ScanText.ToString().ToUpper.Trim() Then
                        Me.HFnSampleId.Value = Me.gvwSubjectSample.Rows(index1).Cells(GVC_nSampleId).Text.Trim()
                        ValidSubject = True
                        Exit For
                    End If

                Next

                If ValidSubject = False Then
                    ObjCommon.ShowAlert("Sample Is Not Valid For You", Me.Page)
                    Me.resetpage()
                    Exit Sub
                End If

                '' wStr = "vSampleID = '" + ScanText.Trim() + "' And cSampleTypeCode='' "

                wStr = "vSampleID = '" + ScanText.Trim() + "' "
                ds.Tables.Clear()
                ds.AcceptChanges()
                dv = CType(Me.ViewState(VS_dtViewPDSampleDetail), DataTable).Copy().DefaultView
                dv.RowFilter = wStr
                ds.Tables.Add(dv.ToTable())
                ds.AcceptChanges()

                If ds.Tables(0).Rows.Count < 1 Then
                    ObjCommon.ShowAlert("Sample Is Not Valid", Me.Page)
                    Me.resetpage()
                    Exit Sub
                End If

                If (Not ds.Tables(0).Rows(0)("vSubjectID") Is DBNull.Value AndAlso _
                        ds.Tables(0).Rows(0)("vSubjectID").ToString.Trim() <> "") And _
                    (Not ds.Tables(0).Rows(0)("dCollectionDateTime") Is DBNull.Value AndAlso _
                        ds.Tables(0).Rows(0)("dCollectionDateTime").ToString.Trim() <> "") Then

                    ObjCommon.ShowAlert("Sample is Already Assigned To The Subject", Me.Page)
                    Me.resetpage()
                    Exit Sub

                End If

                Me.ViewState(VS_DtSave) = ds.Tables(0)
                ds = Nothing
                Me.lblSample.Text = ScanText.Trim

            End If

            Me.txtScan.Text = ""
            Me.txtScan.Focus()

            If Me.lblSample.Text <> "" And Me.lblSubject.Text <> "" Then
                If Me.txtDeviation.Text.Trim = "" Then
                    ObjCommon.ShowAlert("Please Enter Deviation..", Me.Page)
                    resetpage()
                    Exit Sub
                End If
                For index1 As Integer = 0 To Me.gvwSubjectSample.Rows.Count - 1

                    ValidSubject = False
                    If Me.gvwSubjectSample.Rows(index1).Cells(GVC_vSampleBarcode).Text.ToUpper.Trim() = Me.lblSample.Text.ToUpper.Trim() And _
                       Me.gvwSubjectSample.Rows(index1).Cells(GVC_SubjectID).Text.ToUpper.Trim() = Me.lblSubject.Text.ToUpper.Trim() Then
                        ValidSubject = True
                        Exit For
                    End If

                Next

                If ValidSubject = False Then
                    ObjCommon.ShowAlert("Sample Is Not Valid For Selected Subject", Me.Page)
                    Me.resetpage()
                    Exit Sub
                End If

                dt = CType(Me.ViewState(VS_dtViewPDSampleDetail), DataTable)
                dv = dt.Copy().DefaultView()
                dv.RowFilter = "vSampleId = '" + Me.lblSample.Text.Trim() + "' And csampleTypeCode=''"
                dt = dv.ToTable()


                dv_RefTime = CType(Me.ViewState(VS_DtMedexInfoHdr), DataTable).Copy().DefaultView()
                dv_RefTime.RowFilter = "iNodeId = " + dt.Rows(0)("iRefNodeId").ToString.Trim() + _
                                       " And iMySubjectNoForPdSample =  " + dt.Rows(0)("iMySubjectNo").ToString.Trim()
                dt_RefTime = dv_RefTime.ToTable()
                'dt_RefTime = dv_RefTime.ToTable()
                If dt_RefTime.Rows.Count > 0 Then
                    strDate = dt_RefTime.Rows(0).Item("vDefaultValue").ToString.Trim() + " "
                    strDate += dt_RefTime.Rows(1).Item("vDefaultValue").ToString.Trim()
                    refTime = CType(strDate.Trim(), DateTime)

                    time = CType(dt.Rows(0)("nRefTime").ToString.Trim(), Decimal)
                    time1 = time.Split(".")

                    If time1.Length > 1 Then
                        refTime = refTime.AddHours(((time1(0) * 60) + time1(1)) / 60)
                    Else
                        refTime = refTime.AddHours(time1(0))
                    End If

                    refTime = refTime.GetDateTimeFormats()(81)

                    ''Copied  from frmSampleCollection by akhilesh
                    Dim CurDate As DateTime = CDate(ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName)).DateTime).GetDateTimeFormats()(81)

                    Dim CurDate1 As DateTime = CDate(ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName)).DateTime).GetDateTimeFormats()(50) 'done so that seconds should not come when taking Sample b4 Scheduled Time
                    ViewState(VS_CollectionTime) = CurDate
                    '-------------------------------------------------

                    If DateDiff(DateInterval.Minute, refTime, CurDate) > CType(Me.txtDeviation.Text.Trim(), Integer) Or _
                            DateDiff(DateInterval.Minute, CurDate1, refTime) > CType(Me.txtDeviation.Text.Trim(), Integer) Then
                        Me.MPEDeviation.Show()
                        Exit Sub
                    End If

                ElseIf Not (dt_RefTime.Rows.Count > 0) Then ' For Pre-Dose
                    ViewState(VS_CollectionTime) = CDate(ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName)).DateTime).GetDateTimeFormats()(81)
                End If

                If Not AssignValues1() Then
                    resetpage()
                    Exit Sub
                End If

            End If
            txtScan.Focus()
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Focus", "document.getElementById('" + txtScan.ClientID + "').focus();", True)
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Focus", " $find('MPEFasting').show(); ", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......txtScan_TextChanged")
        End Try

    End Sub
    'Protected Sub txtreplaceCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtreplaceCode.TextChanged
    '    Dim ScanText As String = ""
    '    Dim wStr As String = ""
    '    Dim eStr As String = ""
    '    Dim ds As New DataSet
    '    Dim ds_Save As New DataSet
    '    Dim ReplaceWith As String = ""
    '    Dim ValidSample As Boolean = False
    '    Dim ds_ReplacePKSampleDtl As New DataSet
    '    Dim dv As DataView
    '    Try

    '        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
    '        ScanText = Me.txtreplaceCode.Text.Trim

    '        If (ScanText.Contains("-")) Or (ScanText.Length < 8) Then

    '            ObjCommon.ShowAlert("Please Select Only Sample For Replacement", Me.Page)
    '            Me.resetpage()
    '            Exit Sub

    '        End If

    '        wStr = "vPkSampleId = '" + ScanText.Trim() + "'"

    '        If Not ObjHelp.view_PDsampledetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '            ds_ReplacePKSampleDtl, eStr) Then
    '            Me.ShowErrorMessage("Error While Getting Data From View_PDSampleDetail : ", eStr)
    '            Exit Sub
    '        End If


    '        If ds_ReplacePKSampleDtl.Tables(0).Rows.Count < 1 Then
    '            ObjCommon.ShowAlert("Sample Is Not Valid", Me.Page)
    '            Me.resetpage()
    '            Exit Sub
    '        End If

    '        If (Not ds_ReplacePKSampleDtl.Tables(0).Rows(0)("vSubjectID") Is DBNull.Value AndAlso _
    '                ds_ReplacePKSampleDtl.Tables(0).Rows(0)("vSubjectID").ToString.Trim() <> "") And _
    '            (Not ds_ReplacePKSampleDtl.Tables(0).Rows(0)("dCollectionDateTime") Is DBNull.Value AndAlso _
    '                ds_ReplacePKSampleDtl.Tables(0).Rows(0)("dCollectionDateTime").ToString.Trim() <> "") Then

    '            ObjCommon.ShowAlert("Sample is Already Assigned to the Subject", Me.Page)
    '            Me.resetpage()
    '            Exit Sub

    '        End If

    '        Me.ViewState(VS_DtSave) = ds_ReplacePKSampleDtl.Tables(0)
    '        ds_ReplacePKSampleDtl = Nothing
    '        Me.lbReplaceWith.Text = ScanText.Trim

    '        Me.txtreplaceCode.Text = ""
    '        Me.txtReplaceRemark.Focus()

    '        Me.MPEReplacement.Show()
    '        Me.btnSubjectMgmt.Enabled = False

    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.Message, "")
    '    End Try

    'End Sub

#End Region

#Region "AssignValues2"

    Private Function AssignValues2(ByVal ds_Save As DataSet, ByRef DtPDSampleDetail As DataTable) As Boolean
        Dim DsPDSampleDetail As New DataSet
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty
        Try

            Wstr = "nSampleId=" & Me.HFnSampleId.Value.ToString()

            If Not Me.ObjHelp.GetData("SampleDetail", "*", Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, DsPDSampleDetail, estr) Then
                Throw New Exception("Error While Getting Data From PDSampleDetail : " + estr)
            End If

            DtPDSampleDetail = DsPDSampleDetail.Tables(0).Copy()

            For Each dr As DataRow In DtPDSampleDetail.Rows

                dr("nSampleId") = Me.HFnSampleId.Value.Trim
                dr("iCollectionBy") = Me.ddlUser.SelectedItem.Value.Trim()
                dr("vRemark") = Me.txtRemarks.Text
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cFastingStatus") = "R"
                'dr("dCollectionDateTime") = CType(ViewState(VS_CollectionTime), DateTime)
                dr("dCollectionDateTime") = CType(CDate(ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName)).DateTime).GetDateTimeFormats()(81), DateTime)   '' Added by Rahul Rupareliya For Audit Trail
                'dr("dModifyOn") = CType(CDate(ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName)).DateTime).GetDateTimeFormats()(81), DateTime)           '' Added by Rahul Rupareliya For Audit Trail
                dr.AcceptChanges()
            Next dr
            DtPDSampleDetail.AcceptChanges()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "ResetPage"

    Protected Sub resetpage()
        Me.lblSample.Text = ""
        Me.lblSubject.Text = ""
        Me.txtScan.Text = ""
        Me.txtScan.Focus()
        Me.lblMySubject.Text = ""
        Me.ViewState(VS_DtSave) = Nothing
        Me.ddlRemarks.SelectedIndex = -1
        Me.txtRemarks.Text = ""
    End Sub

#End Region

#Region "DivGrid Events"

    Protected Sub gvwSubjects_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwSubjects.RowCreated
        e.Row.Cells(GVCSub_WorkspaceId).Visible = False
        e.Row.Cells(GVCSub_MySubjectNo).Visible = False
    End Sub
    Protected Sub gvwSubjects_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwSubjects.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.Cells(GVCSub_Select).FindControl("ChkMove"), CheckBox).Attributes.Add("Onclick", "CheckSelected();")
        End If
    End Sub

#End Region

#Region "AssignValues1"

    Private Function AssignValues1() As Boolean
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_ViewMedExWorkSpaceDtl As New DataSet
        Dim dr As DataRow
        Dim ds_CRFHdr As New DataSet
        Dim ds_CRFDtl As New DataSet
        Dim ds_ViewCRFDtl As New DataSet
        Dim ds_CRFSUBDTl As New DataSet
        Dim drHdr As DataRow
        Dim drDtl As DataRow
        Dim drSubDtl As DataRow
        Dim Retu_TranNo As Integer = 0
        Dim iMySubjectNo As String = "0"
        Dim iMySubjectNoNew As String = "0"
        Dim iNodeIndex As Integer = 0
        Dim SubjectID As String = ""
        Dim DtSampleTypeDetail As New DataTable
        Dim DtSampleDetail As New DataTable
        Dim ds As New DataSet
        Dim ds_Save As New DataSet
        Dim index As Integer = 0
        Dim dv As DataView
        Dim dv_PDSampleDetail As New DataView

        Dim pendingActivity As Array
        Dim str_PendingActivity As Array
        Dim ds_ActivityDeviation As DataSet = Nothing

        Try
            If Not CheckSubject() Then
                ObjCommon.ShowAlert("Sample For Selected Subject Is Already Collected From eCRF Page.", Me.Page)
                Me.resetpage()
                Return False
            End If
            If ds_Save.Tables.Count < 1 Then
                ds_Save.Tables.Add(CType(Me.ViewState(VS_DtSave), DataTable).Copy())
                'Checking whether the subject belongs to same workspace or not
            End If

            wStr = "vWorkSpaceId = '" + ds_Save.Tables(0).Rows(0).Item("vWorkSpaceId").ToString + "' And"
            wStr += " vSubjectID = '" + Me.lblSubject.Text.Trim + "'"
            wStr += " And cRejectionFlag <> 'Y'"

            dv = CType(Me.ViewState(VS_DtViewWorkSpaceSubjectMst), DataTable).Copy().DefaultView
            dv.RowFilter = wStr
            ds.Tables.Add(dv.ToTable())
            ds.AcceptChanges()

            If ds.Tables(0).Rows.Count < 1 Then
                ObjCommon.ShowAlert("Subject Is Not Valid For This Project", Me.Page)
                Me.resetpage()
                Exit Function
            End If

            'To Validate Subject
            If Not IsNothing(ds_Save.Tables(0).Columns.Contains("vSubjectID")) AndAlso _
             Not (ds_Save.Tables(0).Rows(0).Item("vSubjectID") Is System.DBNull.Value) AndAlso _
             ds_Save.Tables(0).Rows(0).Item("vSubjectID").ToString.Trim() <> "" Then

                SubjectID = Convert.ToString(ds.Tables(0).Rows(0)("vSubjectID"))

                'If SubjectID <> Convert.ToString(ds_Save.Tables(0).Rows(0)("vSubjectID")) Then
                '    ObjCommon.ShowAlert("Subject Is Not Valid For This Sample. This Sample Is For '" + _
                '                        ds_Save.Tables(0).Rows(0)("vSubjectID").ToString.Trim() + "'", Me.Page)
                '    Me.resetpage()
                '    Exit Function
                'End If

            End If
            '***********************************************
            If ds.Tables(0).Columns.Contains("iMySubjectNo") Then
                iMySubjectNo = Convert.ToString(ds.Tables(0).Rows(0)("iMySubjectNoNew"))
                iMySubjectNoNew = Convert.ToString(ds.Tables(0).Rows(0)("iMySubjectNo"))

                If iMySubjectNo <> Convert.ToString(ds_Save.Tables(0).Rows(0)("iMySubjectNo")) Then
                    ObjCommon.ShowAlert("SubjectNo Is Not Valid For This Sample", Me.Page)
                    Me.resetpage()
                    Exit Function
                End If

            End If

            For Each dr In ds_Save.Tables(0).Rows

                dr("vSubjectID") = Me.lblSubject.Text.Trim
                dr("vLocationCode") = Me.Session(S_LocationCode).ToString()
                'dr("dCollectionDateTime") = CType(ViewState(VS_CollectionTime), DateTime)
                dr("dCollectionDateTime") = CType(CDate(ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName)).DateTime).GetDateTimeFormats()(81), DateTime)    '' Added by Rahul Rupareliya For Audit Trail
                dr("iCollectionBy") = Me.ddlUser.SelectedItem.Value.Trim()
                dr("cStatusIndi") = "E"
                'dr("dModifyOn") = CType(CDate(ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName)).DateTime).GetDateTimeFormats()(81), DateTime)    '' Added by Rahul Rupareliya For Audit Trail
                dr.AcceptChanges()

            Next

            'Added Code For Making Entry In "CRFHdr" ,"CrfDTl" and "CRFSubdtl" Table.
            wStr = ""
            wStr = "vWorkSpaceId = '" + ds_Save.Tables(0).Rows(0)("vWorkSpaceId").ToString() + "'" _
                    + " And vActivityId = '" + ds_Save.Tables(0).Rows(0)("vActivityId").ToString() + "' AND iNodeID = " + Me.ddlActivity.SelectedValue.ToString.Trim() _
                    + " And vMedexType<> 'Import' And cStatusIndi <> 'D' "

            If Not Me.ObjHelp.GetViewMedExWorkSpaceDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_ViewMedExWorkSpaceDtl, eStr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From View_MedExWorkSpaceDtl : " + eStr, Me.Page)
                Exit Function
            End If

            If ds_ViewMedExWorkSpaceDtl.Tables(0).Rows.Count < 1 Then
                Me.ObjCommon.ShowAlert("No Records Found", Me.Page)
                Exit Function
            End If

            'For Getting iNodeIndex to fill in 

            wStr = "vWorkSpaceId = '" + ds_Save.Tables(0).Rows(0)("vWorkSpaceId").ToString() + "'" _
                    + " And iPeriod = " + ds_Save.Tables(0).Rows(0)("iPeriod").ToString() _
                    + " And vActivityId = '" + ds_Save.Tables(0).Rows(0)("vActivityId").ToString() + "'" _
                    + " And iNodeId = " + ds_Save.Tables(0).Rows(0)("iNodeId").ToString() + " And cStatusIndi <> 'D'"

            ds.Tables.Clear()
            ds.AcceptChanges()
            dv = CType(Me.ViewState(VS_DtView_WorkSpaceNodeDetail), DataTable).Copy().DefaultView
            dv.RowFilter = wStr
            ds.Tables.Add(dv.ToTable())
            ds.AcceptChanges()

            If ds.Tables(0).Rows.Count > 0 Then
                iNodeIndex = ds.Tables(0).Rows(0)("iNodeIndex")
            End If

            'for CRf HDR   
            ds_CRFHdr.Tables.Add(CType(Me.ViewState(VS_DtCRFHdr), DataTable).Copy())
            ds_CRFHdr.Tables(0).Rows.Clear()
            ds_CRFHdr.AcceptChanges()
            drHdr = ds_CRFHdr.Tables(0).NewRow()
            drHdr("nCRFHdrNo") = 1
            drHdr("vWorkSpaceId") = ds_Save.Tables(0).Rows(0)("vWorkSpaceId").ToString()
            'drHdr("dStartDate") = CType(ViewState(VS_CollectionTime), DateTime)
            drHdr("dStartDate") = CType(CDate(ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName)).DateTime).GetDateTimeFormats()(81), DateTime)         '' Added by Rahul Rupareliya
            drHdr("iPeriod") = ds_Save.Tables(0).Rows(0)("iPeriod").ToString()
            drHdr("iNodeId") = ds_Save.Tables(0).Rows(0)("iNodeId").ToString()
            drHdr("iNodeIndex") = iNodeIndex
            drHdr("vActivityId") = ds_Save.Tables(0).Rows(0)("vActivityId").ToString()
            drHdr("cLockStatus") = "U" 'cLockStatus
            drHdr("iModifyBy") = Me.Session(S_UserID).ToString()
            drHdr("cStatusIndi") = "N"
            drHdr("dModifyon") = DateTime.Now()  '' Added By Rahul Rupareliya For Audit Trial Changes
            ds_CRFHdr.Tables(0).Rows.Add(drHdr)
            ds_CRFHdr.Tables(0).AcceptChanges()

            'for CRFDtl
            ds_CRFDtl.Tables.Add(CType(Me.ViewState(VS_DtCRFDtl), DataTable).Copy())
            ds_CRFDtl.Tables(0).Rows.Clear()
            ds_CRFDtl.AcceptChanges()
            drDtl = ds_CRFDtl.Tables(0).NewRow()
            'drDtl("dRepeatationDate") = CType(ViewState(VS_CollectionTime), DateTime)
            drDtl("dRepeatationDate") = CType(CDate(ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName)).DateTime).GetDateTimeFormats()(81), DateTime)  '' Added by Rahul Rupareliya For Audit Trail
            drDtl("vSubjectId") = ds_Save.Tables(0).Rows(0)("vSubjectID").ToString()
            drDtl("iMySubjectNo") = iMySubjectNoNew
            drDtl("cLockStatus") = "U"
            drDtl("iWorkFlowstageId") = Me.Session(S_WorkFlowStageId)
            drDtl("iModifyBy") = Me.Session(S_UserID).ToString()
            drDtl("cStatusIndi") = "N"
            drDtl("cDataStatus") = CRF_Review
            drDtl("dModifyon") = DateTime.Now()   '' Added By Rahul Rupareliya For Audit Trial Changes
            ds_CRFDtl.Tables(0).Rows.Add(drDtl)
            ds_CRFDtl.Tables(0).AcceptChanges()

            'for CRfSubDtl
            dv_PDSampleDetail = CType(Me.ViewState(VS_dtViewPDSampleDetail), DataTable).Copy().DefaultView()
            ds_CRFSUBDTl.Tables.Add(CType(Me.ViewState(VS_DtCRFSubDtl), DataTable).Copy())
            ds_CRFSUBDTl.Tables(0).Rows.Clear()
            ds_CRFSUBDTl.AcceptChanges()
            For Each dr In ds_ViewMedExWorkSpaceDtl.Tables(0).Rows

                drSubDtl = ds_CRFSUBDTl.Tables(0).NewRow()
                drSubDtl("vMedexCode") = dr("vMedExCode").ToString()
                drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim()
                drSubDtl("vMedExResult") = dr("vMedExValues").ToString()
                drSubDtl("iModifyBy") = Me.Session(S_UserID).ToString()
                drSubDtl("cStatusIndi") = "N"
                drSubDtl("dModifyOn") = DateTime.Now()   '' Added By Rahul Rupareliya For Audit Trial Changes

                If dr("vMedExCode").ToString().Trim() = "15355" Then 'ActualTime Then

                    drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim()
                    'drSubDtl("vMedExResult") = CType(ViewState(VS_CollectionTime), DateTime).ToString("HH:mm").Trim 
                    drSubDtl("vMedExResult") = CType(CDate(ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName)).DateTime).GetDateTimeFormats()(81), DateTime).ToString("HH:mm").Trim    ''Added by Rahul  Rupareliya

                ElseIf dr("vMedExCode").ToString().Trim() = "15354" Then ' ScheduledTime Then

                    For index = 0 To Me.gvwSubjectSample.Rows.Count - 1
                        If Me.gvwSubjectSample.Rows(index).Cells(GVC_vSampleId).Text.Trim.ToUpper() = _
                                        ds_Save.Tables(0).Rows(0)("vSampleId").ToString.Trim.ToUpper() Then

                            drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim()
                            drSubDtl("vMedExResult") = Me.gvwSubjectSample.Rows(index).Cells(GVC_RefTime).Text.Trim()
                            Exit For
                        End If
                    Next

                ElseIf dr("vMedExcode").ToString().Trim() = "15352" Then 'DosingDate Then

                    For index = 0 To Me.gvwSubjectSample.Rows.Count - 1
                        If Me.gvwSubjectSample.Rows(index).Cells(GVC_vSampleId).Text.Trim.ToUpper() = _
                                        ds_Save.Tables(0).Rows(0)("vSampleId").ToString.Trim.ToUpper() Then

                            drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim()
                            drSubDtl("vMedExResult") = Me.gvwSubjectSample.Rows(index).Cells(GVC_DosingTime).Text.Trim()
                            Exit For
                        End If
                    Next

                ElseIf dr("vMedExcode").ToString().Trim() = "15353" Then 'SampleCollDate Then
                    drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim()
                    drSubDtl("vMedExResult") = CType(ViewState(VS_CollectionTime), DateTime).ToString("dd-MMM-yyyy")

                ElseIf dr("vMedExCode").ToString().Trim().ToUpper() = "15356" Then 'Remarks Then
                    drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim()
                    drSubDtl("vMedExResult") = Me.ddlRemarks.SelectedItem.Text.Trim()

                ElseIf dr("vMedExCode").ToString().Trim().ToUpper() = "15358" Then 'RemarksIfOther Then
                    drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim()
                    drSubDtl("vMedExResult") = Me.txtRemarks.Text.Trim()

                ElseIf dr("vMedExCode").ToString().Trim().ToUpper() = "15359" Then
                    drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim()
                    drSubDtl("vMedExResult") = Me.rblCollectionStatus.SelectedItem.ToString()

                ElseIf dr("vMedExCode").ToString().Trim().ToUpper() = "15357" Then 'LabId Then
                    drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim()
                    drSubDtl("vMedExResult") = Me.lblSample.Text

                ElseIf dr("vMedExCode").ToString().Trim().ToUpper() = "15361" Then 'EDTA Whole Blood
                    drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim()
                    dv_PDSampleDetail.RowFilter = "cSampleTypeCode='E' And vSampleId='" + Me.lblSample.Text + "'".ToString
                    drSubDtl("vMedExResult") = dv_PDSampleDetail.ToTable.Rows(0).Item("vSampleBarcode").ToString

                ElseIf dr("vMedExCode").ToString().Trim().ToUpper() = "15367" Then 'Floride
                    drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim()
                    dv_PDSampleDetail.RowFilter = "cSampleTypeCode='F' And vSampleId='" + Me.lblSample.Text + "'".ToString
                    drSubDtl("vMedExResult") = dv_PDSampleDetail.ToTable.Rows(0).Item("vSampleBarcode").ToString

                ElseIf dr("vMedExCode").ToString().Trim().ToUpper() = "15363" Then 'Plasma
                    drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim()
                    dv_PDSampleDetail.RowFilter = "cSampleTypeCode='P' And vSampleId='" + Me.lblSample.Text + "'".ToString
                    drSubDtl("vMedExResult") = dv_PDSampleDetail.ToTable.Rows(0).Item("vSampleBarcode").ToString

                ElseIf dr("vMedExCode").ToString().Trim().ToUpper() = "15365" Then 'Serum
                    drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim()
                    dv_PDSampleDetail.RowFilter = "cSampleTypeCode='S' And vSampleId='" + Me.lblSample.Text + "'".ToString
                    drSubDtl("vMedExResult") = dv_PDSampleDetail.ToTable.Rows(0).Item("vSampleBarcode").ToString

                ElseIf dr("vMedExCode").ToString().Trim().ToUpper() = "15364" Then 'Smear
                    drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim()
                    dv_PDSampleDetail.RowFilter = "cSampleTypeCode='M' And vSampleId='" + Me.lblSample.Text + "'".ToString
                    drSubDtl("vMedExResult") = dv_PDSampleDetail.ToTable.Rows(0).Item("vSampleBarcode").ToString

                ElseIf dr("vMedExCode").ToString().Trim().ToUpper() = "15368" Then 'Sodium Citrate 3.2%
                    drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim()
                    dv_PDSampleDetail.RowFilter = "cSampleTypeCode='C' And vSampleId='" + Me.lblSample.Text + "'".ToString
                    drSubDtl("vMedExResult") = dv_PDSampleDetail.ToTable.Rows(0).Item("vSampleBarcode").ToString

                ElseIf dr("vMedExCode").ToString().Trim().ToUpper() = "15366" Then 'Stool
                    drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim()
                    dv_PDSampleDetail.RowFilter = "cSampleTypeCode='T' And vSampleId='" + Me.lblSample.Text + "'".ToString
                    drSubDtl("vMedExResult") = dv_PDSampleDetail.ToTable.Rows(0).Item("vSampleBarcode").ToString

                ElseIf dr("vMedExCode").ToString().Trim().ToUpper() = "15362" Then 'Urine
                    drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim()
                    dv_PDSampleDetail.RowFilter = "cSampleTypeCode='U' And vSampleId='" + Me.lblSample.Text + "'".ToString
                    drSubDtl("vMedExResult") = dv_PDSampleDetail.ToTable.Rows(0).Item("vSampleBarcode").ToString

                End If


                ds_CRFSUBDTl.Tables(0).Rows.Add(drSubDtl)
                ds_CRFSUBDTl.Tables(0).AcceptChanges()

            Next dr

            If Not AssignValues2(ds_Save, DtSampleDetail) Then
                Me.ShowErrorMessage("Error While Assigning Data", eStr)
                Me.resetpage()
                Exit Function
            End If


            '---------For saving Remark subjectWise added by Megha Shah-----'
            If Me.HPendingNode.Value.Contains(Me.lblSubject.Text.Trim) Then
                str_PendingActivity = Regex.Split(Me.HPendingNode.Value, Me.lblSubject.Text.Trim + "@@")
                pendingActivity = Regex.Split(str_PendingActivity(1).ToString(), "##")

                If Not ObjHelp.GetData("WorkSpaceActivitySequenceDeviation", "*", "", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_ActivityDeviation, eStr) Then
                    Throw New Exception("Error While Saving Data For ActivitySeuenceDeviation")
                End If
                Dim drActivity As DataRow
                drActivity = ds_ActivityDeviation.Tables(0).NewRow()
                drActivity("vWorkspaceId") = Me.HProjectId.Value.Trim()
                drActivity("iPeriod") = Me.ddlPeriod.SelectedValue()
                drActivity("vSubjectId") = Me.lblSubject.Text.Trim
                drActivity("iNodeId") = Me.ddlActivity.SelectedValue.Trim()
                drActivity("vPendingNodes") = pendingActivity(0).ToString()
                drActivity("vRemarks") = Me.hremark.Value
                drActivity("iUserId") = Me.Session(S_UserID).ToString()
                'drActivity("dModifiedDate") = CType(ViewState(VS_CollectionTime), DateTime)
                drActivity("dModifiedDate") = CType(CDate(ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName)).DateTime).GetDateTimeFormats()(81), DateTime)
                ds_ActivityDeviation.Tables(0).Rows.Add(drActivity)
                ds_ActivityDeviation.Tables(0).AcceptChanges()

                If Not ObjLambda.Insert_WorkspaceActivitySequenceDeviation(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_ActivityDeviation, Me.Session(S_UserID).ToString(), eStr) Then
                    Throw New Exception("Error While Saving Data For ActivitySeuenceDeviation")
                End If

                '-------------------------'
            End If

            'For SampleDetail
            ds_Save = Nothing
            ds_Save = New DataSet
            ds_Save.Tables.Add(DtSampleDetail.Copy())
            ds_Save.Tables(0).TableName = "PDSample"
            ds_Save.AcceptChanges()

            ds_Save.Tables.Add(ds_CRFHdr.Tables(0).Copy())
            ds_Save.Tables(1).TableName = "CRFHdr"
            ds_Save.AcceptChanges()

            ds_Save.Tables.Add(ds_CRFDtl.Tables(0).Copy())
            ds_Save.Tables(2).TableName = "CRFDtl"
            ds_Save.AcceptChanges()

            ds_Save.Tables.Add(ds_CRFSUBDTl.Tables(0).Copy())
            ds_Save.Tables(3).TableName = "CRFSubDtl"
            ds_Save.AcceptChanges()

            If Not ObjLambda.Save_PDSampleDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_Save, Me.Session(S_UserID), eStr) Then
                Me.ShowErrorMessage("Error While Saving Data In SampleDetail", eStr)
                Me.resetpage()
                Exit Function
            End If

            Me.ViewState(VS_dtViewPDSampleDetail) = Nothing

            If Not FillGrid() Then
                ObjCommon.ShowAlert("Error While Filling Grid", Me.Page)
            End If

            Me.UpControls.Update()
            Me.UpGridSubjectSample.Update()
            Me.UpOtherControls.Update()

            Me.resetpage()
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignValues")
            Return False
        End Try

    End Function

#End Region

#Region "Deviation Related"

    Private Function FilldllRemarks() As Boolean
        Dim ds As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim item As String = String.Empty
        Dim str As String
        Try

            'Me.ddlRemarks.Items.Clear()

            ''wStr = "vMedExCode = '" + BleedSheet_Remarks + "'"
            ''If Not ObjHelp.GetMedExMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            ''                    ds, eStr) Then
            ''    Me.ShowErrorMessage(eStr, "")
            ''    Exit Function
            ''End If
            'wStr = " select MedExWorkSpaceHdr.vWorkspaceId,MedExWorkSpaceHdr.iNodeId,MedExWorkSpaceDtl.vMedExValues" + _
            '" from MedExWorkSpaceHdr " + _
            '" inner join MedexWorkspaceDtl " + _
            '" on(MedexWorkspaceHdr.nMedExWorkSpaceHdrNo=MedexWorkspaceDtl.nMedExWorkSpaceHdrNo" + _
            '" And MedExWorkSpaceDtl.cStatusIndi<>'D'" + _
            '" And MedexWorkspaceDtl.cActiveFlag <>'N'" + _
            '" And MedexWorkspaceDtl.vMedExCode = '" + PDSample_Remarks + "')" + _
            '" Where MedExWorkSpaceHdr.vWorkspaceId='" + Me.HProjectId.Value + "' " + _
            '" And  MedExWorkSpaceHdr.iNodeId=" + ddlActivity.SelectedValue

            ''wStr = "vMedExCode = '" + BleedSheet_Remarks + "'"
            'ds = ObjHelp.GetResultSet(wStr, "MedexWorkSpaceHdrDtl")
            Me.ddlRemarks.Items.Clear()

            wStr = "vMedExCode = '" + PDSample_Remarks + "' And vWorkspaceId='" + Me.HProjectId.Value + "' and iNodeId='" + ddlActivity.SelectedValue + "'"

            If Not Me.ObjHelp.GetData("VIEW_MedExWorkspaceHdr", "vWorkspaceId,iNodeId,vMedExValues", wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr) Then
                Throw New Exception("Error while get value for Remarks..." + eStr)
            End If
            If ds.Tables(0).Rows.Count > 0 Then
                str = ds.Tables(0).Rows(0)("vMedExValues").ToString.Trim()
                Do
                    item = str.Substring(0, str.IndexOf(","))
                    Me.ddlRemarks.Items.Add(item)
                    str = str.Substring(item.Length + 1)
                    If Not str.Contains(",") Then
                        item = str.Substring(0)
                        Me.ddlRemarks.Items.Add(item)
                        Exit Do
                    End If
                Loop
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FilldllRemarks")
            Return False
        End Try
    End Function

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try

            If Not AssignValues1() Then
                resetpage()
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnOk_Click")
        End Try
    End Sub

#End Region

    Protected Sub ddlActivity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlActivity.SelectedIndexChanged

        Dim dt_Activity As New DataTable
        Dim dv_Activity As New DataView

        Try


            dv_Activity = CType(Me.ViewState(VS_DtView_WorkSpaceNodeDetail), DataTable).Copy().DefaultView

            dv_Activity.RowFilter = "iNodeID = " + Me.ddlActivity.SelectedValue

            Me.txtDeviation.Text = "0"
            Me.txtDeviation.Enabled = True
            Me.txtDeviation.Attributes.Add("style", "color:Navy")
            If Not (dv_Activity.ToTable().Rows(0)("nDeviationTime") Is System.DBNull.Value) Then

                Me.txtDeviation.Text = dv_Activity.ToTable().Rows(0)("nDeviationTime")
                Me.txtDeviation.Enabled = False
                Me.txtDeviation.Attributes.Add("style", "color:gray")
            End If


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....ddlActivity_SelectedIndexChanged")

        End Try

    End Sub


#Region "Method For Check Activity Deviation"
    Private Function CheckSequence(ByVal vWorkspaceId As String, _
                                         ByVal NodeId As Integer, _
                                         ByVal SubjectId As String, _
                                         ByVal Period As Integer) As Boolean

        Dim eStr As String = String.Empty
        Dim Param As String = String.Empty
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Dim Ds_Structure As DataSet = Nothing
        Dim SeqNo As Integer
        Dim Str As String = String.Empty

        Dim PendingNode As String = String.Empty
        Dim i As Integer
        Dim temp As Array

        Dim lbl As Label
        Dim gridView As GridView



        Try
            Param = vWorkspaceId + "##" + SubjectId + "##" + Period.ToString()
            If Not objHelp.Proc_GetStructure(Param, Ds_Structure, eStr) Then
                Return False
            End If

            If Ds_Structure.Tables(0).Rows.Count > 0 Then

                If Ds_Structure.Tables(0).Select(" iNodeId = " & NodeId.ToString).Length > 0 Then

                    temp = Regex.Split(SubjectId, ",")

                    SeqNo = Convert.ToString(Ds_Structure.Tables(0).Select(" iNodeId = " & NodeId.ToString)(0).Item("SeqNo"))

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
                            'gridView.CellSpacing = "25"

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


                        End If

                    Next i
                    Me.HPendingNode.Value = Str.ToString()
                Else
                    Me.HPendingNode.Value = ""
                End If
            Else
                Me.HPendingNode.Value = ""
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "...CheckSequence")
            Return False
        End Try
    End Function
#End Region



    Protected Sub rblCollectionStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblCollectionStatus.SelectedIndexChanged
        Me.MPEFasting.Hide()
    End Sub
    'Add by shivani pandya for Project lock
#Region "LockImpact"
    <Services.WebMethod()> _
    Public Shared Function LockImpact(ByVal WorkspaceID As String) As String
        Dim ds_Check As DataSet = Nothing
        Dim dtProjectStatus As New DataTable
        Dim ProjectStatus As String = String.Empty
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim iTran As String = String.Empty
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()

        wStr = "vWorkspaceId = '" + WorkspaceID.ToString() + "' And cStatusIndi <> 'D'"

        If Not objHelp.GetCRFLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_Check, eStr) Then
            Throw New Exception(eStr)
        End If

        If ds_Check.Tables(0).Rows.Count > 0 Then

            dtProjectStatus = ds_Check.Tables(0)
            iTran = dtProjectStatus.Compute("Max(iTranNo)", String.Empty)
            dtProjectStatus.DefaultView.RowFilter = "iTranNo =" + iTran.ToString()

            If dtProjectStatus.DefaultView.ToTable.Rows.Count > 0 Then
                ProjectStatus = dtProjectStatus.DefaultView.ToTable().Rows(0)("cLockFlag").ToString()
            End If
        Else
            ProjectStatus = "U"
        End If

        Return ProjectStatus
    End Function
#End Region
End Class
