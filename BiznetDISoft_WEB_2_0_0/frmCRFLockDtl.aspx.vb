Partial Class frmCRFLockDtl
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objCommon As New clsCommon
    Private objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

    Private VS_DtCRFLockDtl As String = "DtCRFLockDtl"
    Private VS_dsMaxScreeningLockDtl As String = "dsMaxScreeningLockDtl"
    Private VS_dsWorkspaceSubjectMst As String = "dsWorkspaceSubjectMst"
    Private VS_dsScreeningLockDtl As String = "dsScreeningLockDtl"
    Private VS_dsCRFLockDtl As String = "dsCRFLockDtl"
    Private VS_IsSubjectLock As String = "IsSubjectLock"
    Private VS_IsProjectBABE As String = "IsProjectBABE"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_CRFLockDtlNo As Integer = 1
    Private Const GVC_WorkspaceId As Integer = 2
    Private Const GVC_ProjectNo As Integer = 3
    Private Const GVC_TranNo As Integer = 4
    Private Const GVC_LockFlag As Integer = 5
    Private Const GVC_Remarks As Integer = 6
    Private Const GVC_LockChangedBy As Integer = 7
    Private Const GVC_LockChangedOn As Integer = 8


#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Me.IsPostBack() Then
            If Not Me.GENCALL_ShowUI() Then
                Exit Sub
            End If
        End If

    End Sub

#End Region

#Region "GENCALL_ShowUI"

    Private Function GENCALL_ShowUI() As Boolean
        Dim eStr As String = String.Empty
        Dim ds As New DataSet

        Try
            Page.Title = " :: Project/Site Lock Detail  ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), System.Web.UI.WebControls.Label).Text = "Project/Site Lock Detail"


            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                If Not Me.FillGrid() Then
                    Exit Function
                End If
            End If

            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)

            Me.rbtnlstLockUnlock.Items(0).Selected = True

            If Not Me.objhelpDb.GetCRFLockDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                           ds, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds Is Nothing Then
                Me.ViewState(VS_DtCRFLockDtl) = ds.Tables(0)
            End If

            GENCALL_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_ShowUI")
            Return False
        End Try

    End Function

#End Region

#Region "Reset Page"

    Private Sub ResetPage()

        Me.txtproject.Text = ""
        Me.HProjectId.Value = ""
        Me.txtRemarks.Text = ""
        Me.GVCRFLockDtl.DataSource = Nothing
        Me.GVCRFLockDtl.DataBind()
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ThemeSelection();", "ThemeSelection();", True)
    End Sub

#End Region

#Region "Fill Grid"

    Private Function FillGrid() As Boolean
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_CRFLockDtl As New DataSet
        Dim strModifyOn As String = String.Empty
        Dim str2 As String = String.Empty
        Dim str3 As String = String.Empty
        Dim ds_MaxScreeningLockDtl As DataSet = Nothing
        Dim ds_WorkspaceSubjectMst As DataSet = Nothing
        Dim dvScreeningDtl As DataView = Nothing
        Dim valueIwantToGet As String = String.Empty
        Dim IsSubjectLock As Boolean = False
        Dim IsProjectBABE As Boolean = False
        Dim dsProjects As New DataSet
        Try

            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D' order by iTranNo desc"

            If Not Me.objhelpDb.View_CRFLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition _
                                    , ds_CRFLockDtl, eStr) Then
                Throw New Exception(eStr)
            End If

            'Added By Parth Pandya on 17-Apr-2014 for Locking/Unlocking data to Subject MSR

            ViewState(VS_dsMaxScreeningLockDtl) = Nothing
            wStr = String.Empty
            wStr = "vWorkspaceid= '" + Me.HProjectId.Value.ToString.Trim + "'  and cStatusIndi<>'D'"

            If Not objhelpDb.View_MaxScreeningLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                 ds_MaxScreeningLockDtl, eStr) Then

                Throw New Exception(eStr)
            End If
            dvScreeningDtl = ds_MaxScreeningLockDtl.Tables(0).DefaultView


            wStr = "vWorkspaceid= '" + Me.HProjectId.Value.ToString.Trim + "' And iPeriod =1  And cStatusIndi<>'D'  And iMysubjectNo >0  order By dReportingDate"

            If Not objhelpDb.GetViewWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                 ds_WorkspaceSubjectMst, eStr) Then

                Throw New Exception(eStr)
            End If

            If Not Me.objhelpDb.getworkspacemst("vWorkspaceID = " & Me.HProjectId.Value.Trim() & "", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                     dsProjects, eStr) Then
                Me.objCommon.ShowAlert("Error while getting QC projects: " + eStr, Me.Page)
                Return False
            End If
            If dsProjects.Tables(0).Rows.Count > 0 Then
                If dsProjects.Tables(0).Rows(0)("vProjectTypeCode") = ProjectTypeCode_BABE Then
                    IsProjectBABE = True
                    ViewState(VS_IsProjectBABE) = IsProjectBABE
                End If
            End If
            'End
            ViewState(VS_dsCRFLockDtl) = ds_CRFLockDtl
            Me.rbtnlstLockUnlock.Items(1).Selected = False
            Me.rbtnlstLockUnlock.Items(0).Selected = True
            Me.txtRemarks.Text = ""

            If ds_CRFLockDtl.Tables(0).Rows.Count > 0 Then
                If ds_CRFLockDtl.Tables(0).Rows(0).Item("cLockFlag").ToString.ToUpper() = "LOCK" Then
                    Me.rbtnlstLockUnlock.Items(1).Selected = True
                    Me.rbtnlstLockUnlock.Items(0).Selected = False
                Else
                    Me.rbtnlstLockUnlock.Items(1).Selected = False
                    Me.rbtnlstLockUnlock.Items(0).Selected = True
                End If
            End If

            Me.GVCRFLockDtl.DataSource = ds_CRFLockDtl.Tables(0)
            Me.GVCRFLockDtl.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ThemeSelection();", "ThemeSelection();", True)
            If Me.rbtnlstLockUnlock.Text.ToString.ToUpper() = "L" Then
                IsSubjectLock = True
            Else
                IsSubjectLock = False
            End If

            ViewState(VS_dsWorkspaceSubjectMst) = ds_WorkspaceSubjectMst
            If ds_MaxScreeningLockDtl.Tables(0).Rows.Count > 0 Then
                ds_MaxScreeningLockDtl.Tables(0).DefaultView.RowFilter = "cLockUnlockFlag = 'U'"
                If ds_MaxScreeningLockDtl.Tables(0).DefaultView.ToTable.Rows.Count > 0 Then
                    IsSubjectLock = True
                End If
                ViewState(VS_dsMaxScreeningLockDtl) = ds_MaxScreeningLockDtl
            End If

            ViewState(VS_IsSubjectLock) = IsSubjectLock

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillGrid")
            Return False
        End Try

    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_CRFLockDtl As New DataSet
        Dim dr As DataRow
        Dim IsProjectBABE As Boolean = False
        Dim ds_CRFHdr As New DataSet
        Dim ds_Check As New DataSet
        Dim dv_Check As New DataView
        Dim ds_lockCheck As New DataSet
        Dim ds_CRFMSRLockDetail As New DataSet
        Dim IsSubjectLock As Boolean = False
        Dim wStrForLock As String = ""
        Dim ds_DCFScreeningForProject As DataSet

        Try
            wStrForLock = Me.HProjectId.Value.Trim()
            ds_DCFScreeningForProject = objhelpDb.ProcedureExecute("Proc_MSRLOCKVALIDATION", wStrForLock)

            If Not ds_DCFScreeningForProject Is Nothing AndAlso ds_DCFScreeningForProject.Tables(0).Rows.Count > 0 Then
                objCommon.ShowAlert("Discrepancy is Pending For This Project You can Not Lock Project!", Me.Page)
                Exit Sub
            ElseIf Not ds_DCFScreeningForProject Is Nothing AndAlso ds_DCFScreeningForProject.Tables(1).Rows.Count > 0 Then
                objCommon.ShowAlert("Re-Review of MSR pending!", Me.Page)
                Exit Sub
            End If

            ' added by vishal for checking Discrepancy
            If rbtnlstLockUnlock.SelectedValue = "L" Then
                wStr = "(vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cDCFStatus = '" + Discrepancy_Answered + "')"
                wStr += " or (vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cDCFStatus = '" + Discrepancy_Generated + "') and cstatusindi<>'D'"
                If Not Me.objhelpDb.View_DCFMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_lockCheck, eStr) Then
                    Throw New Exception(eStr)
                End If
                If ds_lockCheck.Tables(0).Rows.Count > 0 Then
                    Me.objCommon.ShowAlert("Project Can not Be Locked as Discrepancy is Not yet  Resolved !", Me.Page)
                    Exit Sub
                End If
            End If
            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"
            If Not Me.objhelpDb.GetCRFLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_Check, eStr) Then
                Throw New Exception(eStr)
            End If
            If Not ds_Check Is Nothing AndAlso ds_Check.Tables(0).Rows.Count > 0 Then

                dv_Check = ds_Check.Tables(0).DefaultView
                dv_Check.Sort = "iTranNo desc"

                If Me.rbtnlstLockUnlock.Items(0).Selected Then
                    If Convert.ToString(dv_Check.ToTable().Rows(0)("cLockFlag")).Trim.ToUpper() = "L" Then
                        Me.objCommon.ShowAlert("Project/Site is already Locked !", Me.Page)
                        Exit Sub
                    End If
                Else
                    If Convert.ToString(dv_Check.ToTable().Rows(0)("cLockFlag")).Trim.ToUpper() = "U" Then
                        Me.objCommon.ShowAlert("CRF is already Unlocked !", Me.Page)
                        Exit Sub
                    End If
                End If
            End If
            IsSubjectLock = ViewState(VS_IsSubjectLock)
            IsProjectBABE = CType(ViewState(VS_IsProjectBABE), Boolean)
            If IsSubjectLock = True And IsProjectBABE = True Then
                '' ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "raj", "confirmbox();", True)
            End If
            If Me.hdnSubjectlock.Value = "False" Then
                Exit Sub
            End If
            ds_CRFLockDtl.Tables.Add(CType(Me.ViewState(VS_DtCRFLockDtl), DataTable).Copy())
            ds_CRFLockDtl.Tables(0).Rows.Clear()
            dr = ds_CRFLockDtl.Tables(0).NewRow()
            dr("nCRFLockDtlNo") = 0
            dr("vWorkspaceId") = Me.HProjectId.Value.Trim()
            dr("iTranNo") = 1
            dr("cLockFlag") = "L"
            If Me.rbtnlstLockUnlock.Items(1).Selected Then
                dr("cLockFlag") = "U"
            End If
            dr("vRemarks") = Me.txtRemarks.Text.Trim()
            dr("iLockChangedBy") = Me.Session(S_UserID)
            dr("iModifyBy") = Me.Session(S_UserID)
            dr("cStatusIndi") = "N"
            ds_CRFLockDtl.Tables(0).Rows.Add(dr)
            ds_CRFLockDtl.AcceptChanges()
            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"
            If Not Me.objhelpDb.GetCRFHdr(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                        ds_CRFHdr, eStr) Then
                Throw New Exception(eStr)
            End If
            For Each drNew As DataRow In ds_CRFHdr.Tables(0).Rows

                drNew("cLockStatus") = "L"
                If Me.rbtnlstLockUnlock.Items(1).Selected Then
                    drNew("cLockStatus") = "U"
                End If
                drNew("iModifyBy") = Me.Session(S_UserID)
                drNew("cStatusIndi") = "E"
                drNew("dModifyOn") = DateTime.Now() ''Added by Rahul Rupareliya For Audit Trail Changes
                ds_CRFHdr.AcceptChanges()

            Next drNew

            ds_CRFLockDtl.Tables.Add(ds_CRFHdr.Tables(0).Copy())
            ds_CRFLockDtl.Tables(1).TableName = "CRFHdr"
            ds_CRFLockDtl.AcceptChanges()

            If Not Me.objLambda.Save_CRFLockDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                    ds_CRFLockDtl, Me.Session(S_UserID), eStr) Then
                objCommon.ShowAlert("Error While Saving CRFHdr", Me.Page)
            End If

            'Added by Parth Pandya as per discussion with bhumi vyas on 8-Oct-2014
            If IsProjectBABE = True Then
                wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D' order by iTranNo desc"
                If Not Me.objhelpDb.View_CRFLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition _
                                        , ds_CRFMSRLockDetail, eStr) Then
                    Throw New Exception(eStr)
                End If
                If ds_CRFMSRLockDetail.Tables(0).Rows.Count > 0 Then
                    If Not ds_CRFMSRLockDetail.Tables(0).Rows(0).Item("cLockFlag").ToString.ToUpper() = "LOCK" Then
                        If Not LockUnlockScreening("UNLOCK") Then
                            Me.objCommon.ShowAlert("Error while saving details to Subject MSR", Me)
                        End If
                    Else
                        If Not LockUnlockScreening("LOCK") Then
                            Me.objCommon.ShowAlert("Error while saving details to Subject MSR", Me)
                        End If
                    End If
                End If
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert(' Project Status and MSR Status Saved Sucessfully ! ')", True)
            Else
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert(' Project Status Saved Sucessfully ! ')", True)
            End If
            'End
            If Not Me.FillGrid() Then
                Exit Sub
            End If

            Me.ResetPage()

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Processing Locking/Unlocking : " + ex.Message, "...btnSave_Click")
        End Try

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.ResetPage()
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        If Not Me.FillGrid() Then
            Exit Sub
        End If
    End Sub

#End Region

#Region "Lock\Unlock User Detail"
    'Added By Parth Pandya for saving data in Subject MSR
    Private Function LockUnlockScreening(ByVal Type As String, Optional ByVal IslockAll As Boolean = True) As Boolean
        Dim ds_ScreeningLockDtl As New DataSet
        Dim ds_WorkspaceSubjectMst As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim dr_ScrLockDtl As DataRow
        Try
            If IslockAll = True Then

                If Not objhelpDb.GetScreeningLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                    ds_ScreeningLockDtl, eStr) Then

                    Throw New Exception(eStr)
                End If

                ViewState(VS_dsScreeningLockDtl) = ds_ScreeningLockDtl

                ds_ScreeningLockDtl = CType(ViewState(VS_dsScreeningLockDtl), DataSet)
                ds_WorkspaceSubjectMst = CType(ViewState(VS_dsWorkspaceSubjectMst), DataSet)
            End If


            For index As Integer = 0 To ds_WorkspaceSubjectMst.Tables(0).Rows.Count - 1
                dr_ScrLockDtl = ds_ScreeningLockDtl.Tables(0).NewRow
                dr_ScrLockDtl("nScreeningLockDtlNo") = 0
                dr_ScrLockDtl("vWorkspaceID") = Me.HProjectId.Value
                dr_ScrLockDtl("vSubjectId") = ds_WorkspaceSubjectMst.Tables(0).Rows(index)("vSubjectID")
                dr_ScrLockDtl("iTranNo") = 0
                dr_ScrLockDtl("nMedexScreeningHdrNo") = ds_WorkspaceSubjectMst.Tables(0).Rows(index)("nMedexScreeningHdrNo")
                dr_ScrLockDtl("iLockedBy") = Me.Session(S_UserID)
                dr_ScrLockDtl("cLockUnlockFlag") = "U"
                If Type.ToUpper.Trim() = "LOCK" Then
                    dr_ScrLockDtl("cLockUnlockFlag") = "L"
                End If
                dr_ScrLockDtl("iModifyBy") = Me.Session(S_UserID)
                dr_ScrLockDtl("cStatusindi") = "N"
                ds_ScreeningLockDtl.Tables(0).Rows.Add(dr_ScrLockDtl)
                ds_ScreeningLockDtl.AcceptChanges()

            Next

            If Not objLambda.Save_ScreeeningLockDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                   ds_ScreeningLockDtl, Me.Session(S_UserID), eStr) Then

                Throw New Exception(eStr)

            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

#End Region

#Region "Grid Events"

    Protected Sub GVCRFLockDtl_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVCRFLockDtl.RowCreated

        If e.Row.RowType = DataControlRowType.Header Or _
           e.Row.RowType = DataControlRowType.DataRow Or _
           e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVC_CRFLockDtlNo).Visible = False
            e.Row.Cells(GVC_WorkspaceId).Visible = False
            e.Row.Cells(GVC_CRFLockDtlNo).Visible = False
            e.Row.Cells(GVC_CRFLockDtlNo).Visible = False

        End If

    End Sub

    Protected Sub GVCRFLockDtl_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVCRFLockDtl.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVC_SrNo).Text = (Me.GVCRFLockDtl.PageIndex * Me.GVCRFLockDtl.PageSize) + e.Row.RowIndex + 1
            e.Row.Cells(GVC_LockChangedOn).Text = CDate(e.Row.Cells(GVC_LockChangedOn).Text).ToString("dd-MMM-yyyy HH:mm") + strServerOffset
        End If
    End Sub

#End Region

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), System.Web.UI.WebControls.Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), System.Web.UI.WebControls.Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub

#End Region

End Class