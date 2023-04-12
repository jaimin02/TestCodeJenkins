
Partial Class frmSubjectEnrollment
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "

    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()

    Private Const GVC_RendomizationNo As Integer = 0
    Private Const GVC_SubjectId As Integer = 1
    Private Const GVC_SubjectInitial As Integer = 2
    Private Const GVC_SubjectName As Integer = 3
    Private Const GVC_EnrollMentDate As Integer = 4
    Private Const GVC_LastVisit As Integer = 5
    Private Const GVC_WorkspaceId As Integer = 6
    Private Const GVC_Assign As Integer = 7
    Private Const GVC_MySubjectNo As Integer = 8

#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            GenCall()

        End If
    End Sub
#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not GenCall_ShowUI() Then 'For Displaying Data 
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_ShowUI"

    Private Function GenCall_ShowUI() As Boolean
        Dim eStr As String = ""
        Dim ds_Workspace As New DataSet
        Try

            Page.Title = Page.Title = ":: Subject Enrollment :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Master.FindControl("lblHeading"), Label).Text = "Subject Enrollment"
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)

            If Not IsNothing(Me.Request.QueryString("WorkspaceId")) AndAlso Me.Request.QueryString("WorkspaceId").Trim() <> "" Then

                Me.HProjectId.Value = Me.Request.QueryString("WorkspaceId").Trim()
                If Not Me.objHelp.getworkspacemst("vWorkspaceId='" + Me.Request.QueryString("WorkspaceId").Trim() + "' And cStatusIndi <> 'D'", _
                        WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Workspace, eStr) Then
                    Me.objcommon.ShowAlert("Error While Getting Data From WorkspaceMst:" + eStr, Me.Page)
                    Exit Function
                End If

                If ds_Workspace.Tables(0).Rows.Count > 0 Then
                    Me.txtproject.Text = ds_Workspace.Tables(0).Rows(0)("vWorkspaceDesc").ToString.Trim()
                End If

                If Not FillGridSubject() Then
                    Return False
                End If

            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        Finally
        End Try

    End Function

#End Region

#Region "FillGridSubject"

    Private Function FillGridSubject() As Boolean
        Dim eStr As String = ""
        Dim wStr As String = ""
        Dim ds_Subject As New DataSet
        Dim dv_Subject As New DataView
        Dim dt_Subject As New DataTable
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            'wStr = "cStatusIndi <> 'D' And vWorkspaceId = '" + Me.HProjectId.Value.ToString() + "'" 'And vLocationCode = '" + Me.Session(S_LocationCode) + "'"
            'wStr += " order by vRandomizationNo desc, cast(dEnrollmentDate as Datetime) desc"
            'If Not objHelp.GetView_SubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '                ds_Subject, eStr) Then
            '    Throw New Exception(eStr)
            'End If

            If Not objHelp.Proc_SubjectEnrollment(Me.HProjectId.Value.Trim(), Me.Session(S_LocationCode), _
                                    ds_Subject, eStr) Then
                Throw New Exception(eStr)
            End If

            dv_Subject = ds_Subject.Tables(0).DefaultView
            If Not IsNothing(Me.HSubjectId.Value) AndAlso Me.HSubjectId.Value <> "" Then
                dv_Subject.RowFilter = "vSubjectId = '" + Me.HSubjectId.Value.Trim() + "'"
            End If
            dt_Subject = dv_Subject.ToTable()

            BindGrid(dt_Subject)

            Me.HSubjectId.Value = ""
            Me.txtSubject.Text = ""
            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

    Private Sub BindGrid(ByVal dt As DataTable)
        Dim dvGrid As New DataView
        dvGrid = dt.DefaultView()
        dvGrid.Sort = "vRandomizationNo desc, dEnrollmentDate desc"
        gvSubject.DataSource = dvGrid.ToTable()
        gvSubject.DataBind()
        dt.Dispose()
    End Sub

#End Region

#Region "Button Click Events"

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Me.Response.Redirect("frmSubjectPIFMst.aspx?mode=1&Page2=frmSubjectEnrollment&WorkspaceId=" + Me.HProjectId.Value)
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        If Not FillGridSubject() Then
            Exit Sub
        End If
    End Sub

    Protected Sub btnSubject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubject.Click
        If Not FillGridSubject() Then
            Exit Sub
        End If
    End Sub

#End Region

#Region "Grid Event"

    Protected Sub gvSubject_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSubject.PageIndexChanging
        Dim dt_Subject As New DataTable
        gvSubject.PageIndex = e.NewPageIndex
        FillGridSubject()
    End Sub

    Protected Sub gvSubject_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSubject.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or _
                      e.Row.RowType = DataControlRowType.Header Or _
                      e.Row.RowType = DataControlRowType.Footer Then

            'e.Row.Cells(GVC_SubjectId).Visible = False
            e.Row.Cells(GVC_WorkspaceId).Visible = False
            e.Row.Cells(GVC_MySubjectNo).Visible = False
        End If
    End Sub

    Protected Sub gvSubject_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSubject.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            CType(e.Row.FindControl("lnkAssign"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkAssign"), LinkButton).CommandName = "ASSIGN"

            CType(e.Row.FindControl("hlnkFile"), HyperLink).NavigateUrl = "frmVisitDetail.aspx?Page2=frmSubjectEnrollment&SubjectId=" + _
                            e.Row.Cells(GVC_SubjectId).Text.Trim() + "&WorkspaceId=" + Me.HProjectId.Value.Trim() + _
                            "&MySubjectNo=" + e.Row.Cells(GVC_MySubjectNo).Text.Trim() + _
                            "&RandomizationNo=" + e.Row.Cells(GVC_RendomizationNo).Text.Trim()

            CType(e.Row.FindControl("lnkAssign"), LinkButton).Visible = False
            If e.Row.Cells(GVC_WorkspaceId).Text.Replace("&nbsp;", "").Trim() <> "" Then
                CType(e.Row.FindControl("lnkAssign"), LinkButton).Visible = True
            End If

        End If
    End Sub

    Protected Sub gvSubject_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim index As Integer = CType(e.CommandArgument, Integer)

        If e.CommandName.ToUpper.Trim() = "ASSIGN" Then

            Save(Me.gvSubject.Rows(index).Cells(GVC_SubjectId).Text.Trim())

        End If

    End Sub

#End Region

#Region "Save"

    Protected Sub Save(ByVal SubjectId As String)
        Dim Dt As New DataTable
        Dim Ds_stagemat As DataSet
        Dim Ds_Grid As New DataSet
        Dim Ds_WorkspaceSubjectMst As New DataSet
        Dim Dt_WorkspaceSubjectMst As New DataTable
        Dim eStr As String = ""
        Dim ds_type As New Data.DataSet
        Dim objOPws As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()
        Dim ds_MedExScreen As New Data.DataSet
        Try

            AssignUpdatedValues(SubjectId, WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                                Dt_WorkspaceSubjectMst)

            Ds_stagemat = New DataSet
            Ds_stagemat.Tables.Add(Dt_WorkspaceSubjectMst.Copy())
            Ds_stagemat.Tables(0).TableName = "VIEW_workspaceSubjectMst"   ' New Values on the form to be updated

            If Not objOPws.Save_InsertWorkspaceSubjectMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_WorkspaceSubjectMst, Ds_stagemat, Me.Session(S_UserID), eStr) Then
                objcommon.ShowAlert("Error while Assigning", Me.Page)
                Exit Sub
            End If

            objcommon.ShowAlert("Subject Assigned Successfully.", Me)
            FillGridSubject()

        Catch ex As System.Threading.ThreadAbortException
        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
        End Try

    End Sub

    Private Function AssignUpdatedValues(ByVal SubjectId As String, ByVal Choice As Integer, ByRef dtOld As DataTable) As Boolean
        Dim estr As String = ""
        Dim dsSubjectNo As New DataSet
        Dim SubjectNo As Integer = 0
        Dim dr As DataRow
        Dim CanStartAfterDetails As String = ""
        Dim dsSubjectMst As New DataSet
        Dim dsWorkspace As New DataSet
        Dim dsLocation As New DataSet
        Dim Wstr As String = "vSubjectId='" + SubjectId.Trim() + "'"
        Dim WorkspaceId As String = ""
        Dim LocationInitial As String = ""
        Dim RandomizationNo As String = ""

        WorkspaceId = Me.HProjectId.Value.Trim() 'Me.Request.QueryString("workspaceid").Trim()

        Try
            If Not Me.objHelp.GetViewgetWorkspaceDetailForHdr("vWorkspaceId = '" + WorkspaceId + "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                        dsWorkspace, estr) Then
                Me.objcommon.ShowAlert(estr, Me.Page())
                Return False
                Exit Function
            End If

            If Not Me.objHelp.getLocationMst("vLocationCode = '" + dsWorkspace.Tables(0).Rows(0)("vLocationCode").ToString.Trim() + "'", _
                    WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsLocation, estr) Then

                Me.objcommon.ShowAlert(estr, Me.Page())
                Return False
                Exit Function
            End If

            If dsLocation.Tables(0).Rows.Count < 1 Then
                Me.objcommon.ShowAlert("Location Not Found", Me.Page())
                Exit Function
            End If
            LocationInitial = dsLocation.Tables(0).Rows(0)("vLocationInitiate").ToString()

            'dtOld = Me.ViewState(VS_WorkspaceSubjectMst)
            If Not Me.objHelp.GetWorkspaceSubjectMaster(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            dsSubjectMst, estr) Then
                Me.objcommon.ShowAlert(estr, Me.Page())
                Return False
                Exit Function
            End If
            dtOld = dsSubjectMst.Tables(0)

            If Not Me.objHelp.GetFieldsOfTable("WorkspaceSubjectMst", "IsNull(max(iMySubjectNo),0) as MaxSubNo", _
                                   "vWorkspaceId = '" + WorkspaceId + "'", dsSubjectNo, estr) Then

                Exit Function
            End If

            If dsSubjectNo.Tables(0).Rows(0)("MaxSubNo") = 0 Then
                SubjectNo = 1
            Else
                SubjectNo = dsSubjectNo.Tables(0).Rows(0)("MaxSubNo") + 1
            End If

            RandomizationNo = LocationInitial + "-" + SubjectNo.ToString.Trim()

            'If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
            For Each dr In dtOld.Rows
                dr("vWorkspaceid") = WorkspaceId
                dr("iMySubjectNo") = SubjectNo
                dr("vRandomizationNo") = RandomizationNo

                'If Not IsNothing(Me.ViewState(VS_GVMySubjectNo)) AndAlso Me.ViewState(VS_GVMySubjectNo) <> 0 Then

                '    dr("iMySubjectNo") = CType(Me.ViewState(VS_GVMySubjectNo), Integer) + 1000

                'End If

                dr("iModifyBy") = Me.Session(S_UserID)
                dr.AcceptChanges()
            Next
            dtOld.AcceptChanges()
            'End If

            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
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

End Class
