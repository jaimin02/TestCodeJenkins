
Partial Class frmModifySamples
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtSubMst As String = "SubjectMst" 'gvwSubject
    Private Const VS_DtMedExMst As String = "MedExMst" 'ddlMedEx
    Private Const VS_DtViewSampleMedExDetail As String = "ViewSampleMedExWorkSpaceDetail" 'gvwMedEx
    Private Const VS_SampleId As String = "nSampleId"
    Private Const VS_DtWorkspaceSubjectMst As String = "WorkspaceSubjectMst"

    Private Const AddToGrid As String = "AddToGrid"
    Private Const AddToDatabase As String = "AddToDatabase"

    Private Const GvwSubject_nSampleId As Integer = 0
    Private Const GvwSubject_vSampleId As Integer = 1
    Private Const GvwSubject_SubjectID As Integer = 2
    Private Const GvwSubject_FullName As Integer = 3
    Private Const GvwSubject_CollectionDate As Integer = 4
    Private Const GvwSubject_MySubjectNo As Integer = 5
    Private Const GvwSubject_View As Integer = 6
    Private Const GvwSubject_Edit As Integer = 7

    Private Const GvwMedEx_MedExCode As Integer = 0
    Private Const GvwMedEx_MedExDesc As Integer = 1
    Private Const GvwMedEx_Delete As Integer = 2
    Dim UpdateNew As Boolean = True

#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
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
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim dt_SubMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = ":: Modify Samples :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            ' Me.btnSave.Enabled = True
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Modify Samples"
            dt_SubMst = Me.ViewState(VS_DtSubMst)
            Choice = Me.ViewState("Choice")

            If Me.rblSelection.Items(1).Selected Then 'Project Specific

                Me.pnlProjectSpecific.Visible = True
                Me.btnCancel.Visible = True
                Me.btnSearch.Visible = True

                If Not FillddlActivity() Then
                    Exit Function
                End If

                If Not FillddlPeriod() Then
                    Exit Function
                End If

                If Not FillddlNode() Then
                    Exit Function
                End If

            Else
                Me.rblSelection.Items(0).Selected = True 'Screening
                Me.pnlProjectSpecific.Visible = False
                Me.btnCancel.Visible = False
                Me.btnSearch.Visible = False
            End If

            If Not FillGridgvwSubject() Then
                Exit Function
            End If

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
            End If

            Me.btnSave.Attributes.Add("onClientClick", "return ValidationToSave('" & Me.gvwSubject.ClientID & "');")

            Me.divMedEx.Visible = False
            Me.pnlMedEx.Visible = False

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function

#End Region

#Region "FillDropDown"

    Private Function FillddlActivity() As Boolean
        Dim ds_MedexWorkspaceDtl As New Data.DataSet
        Dim estr As String = ""
        Dim wstr As String = "1=1"
        Dim dv_MedexWorkspaceDtl As New DataView
        Try

            wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "'"

            If Not objHelp.Get_ViewSampleDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_MedexWorkspaceDtl, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From View_SampleDetail", estr)
                Return False
            End If

            If ds_MedexWorkspaceDtl.Tables(0).Rows.Count > 0 Then

                dv_MedexWorkspaceDtl = ds_MedexWorkspaceDtl.Tables(0).DefaultView.ToTable(True, "vActivityId,vActivityName".Split(",")).DefaultView
                dv_MedexWorkspaceDtl.Sort = "vActivityName"
                Me.ddlActivity.DataSource = dv_MedexWorkspaceDtl
                Me.ddlActivity.DataValueField = "vActivityId"
                Me.ddlActivity.DataTextField = "vActivityName"
                Me.ddlActivity.DataBind()
                Me.ddlActivity.Items.Insert(0, New ListItem("--Select Activity--", 0))

            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function FillddlPeriod() As Boolean
        Dim ds_WorkspaceNodeDetail As New Data.DataSet
        Dim estr As String = ""
        Dim wstr As String = "1=1"
        Dim dv_WorkspaceNodeDetail As New DataView
        Try
            wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' And vActivityId='" & Me.ddlActivity.SelectedValue & "'"

            If Not objHelp.GetViewWorkSpaceNodeDetail(wstr, ds_WorkspaceNodeDetail, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From View_SampleDetail", estr)
                Return False
            End If

            If ds_WorkspaceNodeDetail.Tables(0).Rows.Count > 0 Then

                dv_WorkspaceNodeDetail = ds_WorkspaceNodeDetail.Tables(0).DefaultView.ToTable(True, "iPeriod".ToString()).DefaultView
                dv_WorkspaceNodeDetail.Sort = "iPeriod"
                Me.ddlPeriod.DataSource = dv_WorkspaceNodeDetail
                Me.ddlPeriod.DataValueField = "iPeriod"
                Me.ddlPeriod.DataTextField = "iPeriod"
                Me.ddlPeriod.DataBind()
                Me.ddlPeriod.Items.Insert(0, New ListItem("--Select Period--", 0))

            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function FillddlNode() As Boolean
        Dim ds_WorkspaceNodeDetail As New Data.DataSet
        Dim estr As String = ""
        Dim wstr As String = "1=1"
        Dim dv_WorkspaceNodeDetail As New DataView
        Try
            wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' And vActivityId='" & Me.ddlActivity.SelectedValue & "' And iPeriod='" & Me.ddlPeriod.SelectedValue & "'"

            If Not objHelp.GetViewWorkSpaceNodeDetail(wstr, ds_WorkspaceNodeDetail, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From View_SampleDetail", estr)
                Return False
            End If

            If ds_WorkspaceNodeDetail.Tables(0).Rows.Count > 0 Then

                dv_WorkspaceNodeDetail = ds_WorkspaceNodeDetail.Tables(0).DefaultView.ToTable(True, "iNodeId,vNodeDisplayName".Split(",")).DefaultView
                dv_WorkspaceNodeDetail.Sort = "vNodeDisplayName"
                Me.ddlNode.DataSource = dv_WorkspaceNodeDetail
                Me.ddlNode.DataValueField = "iNodeId"
                Me.ddlNode.DataTextField = "vNodeDisplayName"
                Me.ddlNode.DataBind()
                Me.ddlNode.Items.Insert(0, New ListItem("--Select Node--", 0))

            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function FillddlMedexGroup() As Boolean
        Dim ds_MedexGroup As New DataSet
        Dim estr As String = ""
        Dim wstr As String = ""
        Try

            If Not Me.ddlMedexGroup.DataSource Is Nothing Then
                Me.ddlMedexGroup.DataSource = Nothing
                Me.ddlMedexGroup.DataBind()
            End If

            'To Get Where condition of ScopeVales( Project Type )
            If Not ObjCommon.GetScopeValueWithCondition(wstr) Then
                Return False
            End If
            'Wstr = "nScopeNo=" & Me.Session(S_ScopeNo)

            wstr += " And vMedexType='Import' order by vMedExGroupDesc"

            If Not objHelp.GetMedExMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            ds_MedexGroup, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data from MedexGroupMst:" + estr, Me.Page)
                Return False
            End If

            If ds_MedexGroup.Tables(0).Rows.Count > 0 Then
                Me.ddlMedexGroup.DataSource = ds_MedexGroup.Tables(0).DefaultView.ToTable(True, "vMedExGroupCode,vMedExGroupDesc".Split(","))
                Me.ddlMedexGroup.DataValueField = "vMedExGroupCode"
                Me.ddlMedexGroup.DataTextField = "vMedExGroupDesc"
                Me.ddlMedexGroup.DataBind()
                Me.ddlMedexGroup.Items.Insert(0, New ListItem("--Select Attribute Group--", 0))
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function FillddlMedex() As Boolean
        Dim ds_MedexMst As New Data.DataSet
        Dim wstr As String = ""
        Dim estr As String = ""
        Try

            If Not Me.ddlMedexGroup.DataSource Is Nothing Then
                Me.ddlMedexGroup.DataSource = Nothing
                Me.ddlMedexGroup.DataBind()
            End If

            wstr = " vMedexGroupCode='" + Convert.ToString(Me.ddlMedexGroup.SelectedValue) + "'"
            wstr += " And  vMedexType='Import' order by vMedExDesc"

            If Not objHelp.GetMedExMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_MedexMst, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data from MedexMst:" + estr, Me.Page)
                Return False
            End If

            If ds_MedexMst.Tables(0).Rows.Count > 0 Then

                Me.ddlMedex.DataSource = ds_MedexMst
                Me.ddlMedex.DataValueField = "vMedExCode"
                Me.ddlMedex.DataTextField = "vMedExDesc"
                Me.ddlMedex.DataBind()
                Me.ddlMedex.Items.Insert(0, New ListItem("--Select Attribute--", 0))

                Me.ViewState(VS_DtMedExMst) = ds_MedexMst.Tables(0)

            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "FillGrid"

    Private Function FillGridgvwSubject() As Boolean
        Dim ds_gvwSubject As New Data.DataSet
        Dim dt_gvwSubject As New DataTable
        Dim estr As String = ""
        Dim wstr As String = ""
        Dim WStr_Scope As String = ""
        Dim ds_WorkspaceSubjectMst As New DataSet
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not Me.ViewState(VS_DtSubMst) Is Nothing Then

                dt_gvwSubject = CType(Me.ViewState(VS_DtSubMst), DataTable)
                ds_gvwSubject.Tables.Add(dt_gvwSubject)

            Else

                Me.gvwSubject.DataSource = Nothing
                Me.gvwSubject.DataBind()

                If Me.rblSelection.SelectedItem.Value = "00" Then 'If Screening
                    wstr = "vWorkspaceId= '0000000000'"

                Else 'If Project Specific

                    If Me.HProjectId.Value = "" Then
                        Me.gvwSubject.DataSource = Nothing
                        Me.gvwSubject.DataBind()
                        Me.ViewState(VS_DtSubMst) = Nothing
                        Return True
                    Else
                        wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' And vActivityId='" _
                            & Me.ddlActivity.SelectedValue & "' And iPeriod='" & Me.ddlPeriod.SelectedValue & "'" _
                            & " And iNodeId= " & Me.ddlNode.SelectedValue
                    End If

                End If

                'To Get Where condition of ScopeVales( Project Type )
                If Not ObjCommon.GetScopeValueWithCondition(WStr_Scope) Then
                    Exit Function
                End If

                wstr += " And cStatusIndi <> 'D'"
                wstr += " And " + WStr_Scope

                If Not objHelp.Get_ViewSampleDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_gvwSubject, estr) Then
                    Me.ShowErrorMessage("Error While Getting Data From View_SampleDetail", estr)
                    Return False
                End If

                If Me.rblSelection.SelectedItem.Value <> "00" Then

                    wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' And iPeriod='" & Me.ddlPeriod.SelectedValue & "'"
                    If Not objHelp.GetViewWorkspaceSubjectMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_WorkspaceSubjectMst, estr) Then
                        Me.ShowErrorMessage("Error While Getting Data From View_WorkspaceSubjectMst : ", estr)
                        Exit Function
                    End If

                    Me.ViewState(VS_DtWorkspaceSubjectMst) = ds_WorkspaceSubjectMst.Tables(0).Copy()
                End If

            End If

            Me.ViewState(VS_DtSubMst) = ds_gvwSubject.Tables(0) '.DefaultView.ToTable(True, "vSubjectId,FullName".Split(","))

            If ds_gvwSubject.Tables(0).Rows.Count < 1 Then
                ObjCommon.ShowAlert("No Records Found", Me.Page)
                Return True
            End If

            Me.gvwSubject.DataSource = ds_gvwSubject.Tables(0) '.DefaultView.ToTable(True, "vSubjectId,FullName".Split(","))
            Me.gvwSubject.DataBind()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function FillGridgvwMedEx() As Boolean
        Dim ds_gvwMedEx As New Data.DataSet
        Dim dt_gvwMedEx As New DataTable
        Dim estr As String = ""
        Dim wstr As String = ""
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not Me.ViewState(VS_DtViewSampleMedExDetail) Is Nothing Then

                dt_gvwMedEx = CType(Me.ViewState(VS_DtViewSampleMedExDetail), DataTable)
                ds_gvwMedEx.Tables.Add(dt_gvwMedEx)

            Else

                Me.gvwMedEx.DataSource = Nothing
                Me.gvwMedEx.DataBind()

                wstr = "nSampleId = " + CType(Me.ViewState(VS_SampleId), String) + " And cStatusIndi <> 'D' and iRepeatationNo=1 and itranNo=1"

                If Not objHelp.Get_ViewSampleMedExDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_gvwMedEx, estr) Then
                    Me.ShowErrorMessage("Error While Getting Data From View_SampleMedExDetail", estr)
                    Return False
                End If

            End If

            Me.gvwMedEx.DataSource = ds_gvwMedEx
            Me.gvwMedEx.DataBind()
            Me.ViewState(VS_DtViewSampleMedExDetail) = ds_gvwMedEx.Tables(0)

            If Me.gvwMedEx.Rows.Count < 1 Then

                Me.btnSave.Visible = False

            ElseIf Me.gvwMedEx.Rows.Count > 0 Then

                Me.btnSave.Visible = True

            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "RadioButton List Events"

    Protected Sub rblSelection_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Me.resetpage()

            If Me.rblSelection.Items(0).Selected = False Then
                Me.btnCancel.Visible = True
                Me.btnSearch.Visible = True

                If Not FillddlActivity() Then
                    Exit Sub
                End If

                If Not FillddlPeriod() Then
                    Exit Sub
                End If

                If Not FillddlNode() Then
                    Exit Sub
                End If

            Else
                Me.btnCancel.Visible = False
                Me.btnSearch.Visible = False

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "DropDown List Events"

    Protected Sub ddlMedexGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMedexGroup.SelectedIndexChanged
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not FillddlMedex() Then
                Exit Sub
            End If

            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + _
                                                       Me.divMedEx.ClientID.ToString.Trim() + "');", True)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub ddlActivity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not FillddlPeriod() Then
                Exit Sub
            End If

            If Not FillddlNode() Then
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub ddlPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not FillddlNode() Then
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "Button Events"

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Me.ViewState(VS_DtSubMst) = Nothing
            Me.ViewState(VS_DtViewSampleMedExDetail) = Nothing

            If Not FillGridgvwSubject() Then
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnAddMedEx_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dt_DtMedExDtl As New DataTable
        Dim estr As String = ""
        Dim wstr As String = ""
        Try
            'Me.btnSave.Enabled = True
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            AssignValues(AddToGrid)

            dt_DtMedExDtl = CType(Me.ViewState(VS_DtViewSampleMedExDetail), DataTable)
            If Not Me.FillEditGrid(dt_DtMedExDtl) Then
                Exit Sub
            End If

            Me.btnSave.Visible = False

            If Me.gvwMedEx.Rows.Count > 0 Then
                Me.btnSave.Visible = True
            End If

            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + _
                                                       Me.divMedEx.ClientID.ToString.Trim() + "');", True)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            If Not FillddlActivity() Then
                Exit Sub
            End If

            If Not FillddlPeriod() Then
                Exit Sub
            End If

            If Not FillddlNode() Then
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Me.ViewState(VS_DtViewSampleMedExDetail) = Nothing
        Me.ViewState(VS_SampleId) = Nothing
        Me.divMedEx.Visible = False
        Me.pnlMedEx.Visible = False

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim eStr_Retu As String = ""
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            'If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

            If Me.rblSelection.Items(0).Selected = False Then

                If Me.ddlNode.SelectedIndex = 0 Then
                    ObjCommon.ShowAlert("Select Node ", Me.Page)
                    Exit Sub
                End If

            End If
            'UpdateNew = True
            AssignValues(AddToDatabase)
            'CType(Me.ViewState("Dtsave"), DataTable).Clear()
            Me.ddlMedexGroup.Items.Clear()
            Me.ddlMedex.Items.Clear()
            Me.gvwMedEx.DataSource = Nothing
            Me.gvwMedEx.DataBind()

            Me.divMedEx.Visible = False
            Me.pnlMedEx.Visible = False
            'Me.resetpage()

            'End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr_Retu)
        End Try

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Me.resetpage()

    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

#Region "FillEditedGrid"

    Protected Function FillEditGrid(ByVal dt As DataTable) As Boolean
        Dim dv As New DataView

        dv = dt.DefaultView
        dv.RowFilter = "cStatusIndi <> 'D'"
        dt = CType(dv, Data.DataView).ToTable()
        Me.gvwMedEx.DataSource = dt
        Me.gvwMedEx.DataBind()

        Return True

    End Function

#End Region

#Region "Grid Events"

    Protected Sub gvwSubject_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or _
                    e.Row.RowType = DataControlRowType.Header Or _
                    e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GvwSubject_nSampleId).Visible = False
            e.Row.Cells(GvwSubject_MySubjectNo).Visible = False
        End If
    End Sub

    Protected Sub gvwsubject_pageindexchanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            gvwSubject.PageIndex = e.NewPageIndex
            If Not FillGridgvwSubject() Then
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub gvwSubject_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim index As Integer = e.CommandArgument

        If e.CommandName.ToUpper = "VIEW" Or e.CommandName.ToUpper = "EDIT" Then

            If e.CommandName.ToUpper = "VIEW" Then

                Me.ddlMedex.Enabled = False
                Me.ddlMedexGroup.Enabled = False
                Me.btnSave.Enabled = False
                Me.btnAddMedEx.Enabled = False
                Me.gvwMedEx.Columns(GvwMedEx_Delete).Visible = False

            ElseIf e.CommandName.ToUpper = "EDIT" Then
                Me.ddlMedex.Enabled = True
                Me.ddlMedexGroup.Enabled = True
                Me.btnSave.Enabled = True
                Me.btnAddMedEx.Enabled = True
                Me.gvwMedEx.Columns(GvwMedEx_Delete).Visible = True

                If Not FillddlMedexGroup() Then
                    Exit Sub
                End If

                If Not FillddlMedex() Then
                    Exit Sub
                End If
            End If

            Me.divMedEx.Visible = True
            Me.pnlMedEx.Visible = True

            Me.ViewState(VS_SampleId) = Me.gvwSubject.Rows(index).Cells(0).Text

            If Not FillGridgvwMedEx() Then
                Exit Sub
            End If

            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + _
                                                     Me.divMedEx.ClientID.ToString.Trim() + "');", True)

        End If

    End Sub

    Protected Sub gvwSubject_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Dim dt_WorkspaceSubjectMst As New DataTable
        Dim dv_WorkspaceSubjectMst As New DataView
        Try

            If e.Row.RowType = DataControlRowType.DataRow Then

                CType(e.Row.FindControl("lnkView"), LinkButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkView"), LinkButton).CommandName = "View"

                CType(e.Row.FindControl("lnkEdit"), LinkButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkEdit"), LinkButton).CommandName = "Edit"

                'Added on 15-Sep-2009 by Chandresh Vanker For getting SubjectName

                If Me.rblSelection.SelectedItem.Value <> "00" Then

                    dv_WorkspaceSubjectMst = Nothing
                    dv_WorkspaceSubjectMst = New DataView

                    dv_WorkspaceSubjectMst = CType(Me.ViewState(VS_DtWorkspaceSubjectMst), DataTable).Copy().DefaultView()

                    dv_WorkspaceSubjectMst.RowFilter = "iMySubjectNo = " + e.Row.Cells(GvwSubject_MySubjectNo).Text.Trim()

                    dt_WorkspaceSubjectMst = dv_WorkspaceSubjectMst.ToTable()

                    If dt_WorkspaceSubjectMst.Rows.Count > 0 Then
                        e.Row.Cells(GvwSubject_SubjectID).Text = dt_WorkspaceSubjectMst.Rows(0).Item("vSubjectId").ToString.Trim()
                        e.Row.Cells(GvwSubject_FullName).Text = dt_WorkspaceSubjectMst.Rows(0).Item("FullName").ToString.Trim()
                    End If

                End If

                e.Row.Cells(GvwSubject_CollectionDate).Text = e.Row.Cells(GvwSubject_CollectionDate).Text.Replace("&nbsp;", "")
                If e.Row.Cells(GvwSubject_CollectionDate).Text.Trim() <> "" Then
                    e.Row.Cells(GvwSubject_CollectionDate).Text = CType(e.Row.Cells(GvwSubject_CollectionDate).Text.Trim(), Date).GetDateTimeFormats()(23).Trim()
                End If
                '****************************************************************

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub gvwSubject_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs)
        e.Cancel = True
    End Sub

    Protected Sub gvwMedEx_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)

    End Sub

    Protected Sub gvwMedEx_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then

            CType(e.Row.FindControl("imgDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("imgDelete"), ImageButton).CommandName = "Delete"

        End If
    End Sub

    Protected Sub gvwMedEx_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim indexGV As Integer = e.CommandArgument
        'Dim indexSAve As Integer = 0
        Dim indexMedex As Integer = 0
        Dim dt_DtMedExDtl As New DataTable
        Dim gr As String = ""
        Dim dt_Save As New DataTable

        If e.CommandName.ToUpper = "DELETE" Then

            Try
                'Me.btnSave.Enabled = False
                ' dt_Save = CType(Me.ViewState("Dtsave"), DataTable)
                CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + _
                                                                     Me.divMedEx.ClientID.ToString.Trim() + "');", True)

                dt_DtMedExDtl = CType(Me.ViewState(VS_DtViewSampleMedExDetail), DataTable)

                'Deleteing from Grid Data table dt_DtMedExDtl
                'gr = dt_Save.Rows.Count - 1

      
                For indexMedex = 0 To dt_DtMedExDtl.Rows.Count - 1


                    If dt_DtMedExDtl.Rows(indexMedex).Item("vMedExCode").ToString.Trim() = Me.gvwMedEx.Rows(indexGV).Cells(GvwMedEx_MedExCode).Text.Trim() Then
                        Exit For
                        'indexSAve = indexMedex
                    End If

                Next indexMedex

                

                If dt_DtMedExDtl.Rows(indexMedex).Item("nSampleId") = 0 Then
                    dt_DtMedExDtl.Rows(indexMedex).Delete()
                    dt_DtMedExDtl.AcceptChanges()
                    'Me.FillEditGrid(dt_DtMedExDtl)
                    'Exit Sub
                Else
                    dt_DtMedExDtl.Rows(indexMedex).Item("cStatusIndi") = "D"
                    dt_DtMedExDtl.AcceptChanges()
                End If

                If Not Me.FillEditGrid(dt_DtMedExDtl) Then
                    Exit Sub
                End If

                Me.ViewState(VS_DtViewSampleMedExDetail) = dt_DtMedExDtl
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete
                AssignValues(AddToDatabase)

                '====================

            Catch ex As Exception
                Me.ShowErrorMessage(ex.Message, "")
            Finally
                dt_DtMedExDtl = Nothing
            End Try

        End If

    End Sub

    Protected Sub gvwMedEx_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        e.Row.Cells(GvwMedEx_MedExCode).Visible = False
    End Sub

#End Region

#Region "Assign Values"

    Private Sub AssignValues(ByVal mode As String)
        Dim dr As DataRow = Nothing
        Dim drMedEx As DataRow = Nothing
        Dim dt_MedEx As New DataTable
        Dim dt_DtMedExDtl As New DataTable
        Dim dv_DtMedExDtl As New DataView
        Dim ds As New DataSet
        Dim ds_save As New DataSet
        Dim dt_ViewSampleMedExDetail As New DataTable
        Dim estr_Retu As String = ""
        Dim wstr As String = ""
        Dim IndexSubject As Integer = 0
        Dim IndexMedEx As Integer = 0
        Dim Dt_SAve As New DataTable
        Dim dr1 As DataRow



        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            'Dt_SAve = CType(ViewState("Dtsave"), DataTable)
            If mode = AddToGrid Then

                dt_MedEx = CType(Me.ViewState(VS_DtMedExMst), DataTable)
                dt_DtMedExDtl = CType(Me.ViewState(VS_DtViewSampleMedExDetail), DataTable)

                If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                    For Each dr In dt_MedEx.Rows

                        If (dr("vMedExCode").ToString = Me.ddlMedex.SelectedValue.ToString) Then
                            drMedEx = dr
                            Exit For
                        End If

                    Next dr

                    '--------Checking for duplicate MedEx Entry
                    For Each dr In dt_DtMedExDtl.Rows

                        If (dr("vMedExCode") = Me.ddlMedex.SelectedValue.ToString) Then
                            ObjCommon.ShowAlert("Selected MedEx is already added ", Me.Page)
                            Exit Sub
                        End If

                    Next dr
                    '----------------
                    'added by Deepak Singh to define Dt_save to add to database on 23-11-09 after discussing with Naimesh Bhai
                    ' If Dt_SAve Is Nothing Then
                    'Dt_SAve = dt_DtMedExDtl.Copy()
                    'Dt_SAve.Rows.Clear()
                    'End If
                    '=============================
                    'dr = Dt_SAve.NewRow
                    dr = dt_DtMedExDtl.NewRow
                    dr("nSampleId") = 0
                    dr("vMedExCode") = drMedEx("vMedExCode")
                    dr("vMedExDesc") = drMedEx("vMedExDesc")
                    dr("vDefaultValue") = drMedEx("vDefaultValue")
                    dr("vMedExResult") = ""
                    dr("iRepeatationNo") = 1
                    dr("itranNo") = 1
                    dr("iModifyBy") = Me.Session(S_UserID).ToString()
                    dr("cStatusIndi") = "N"
                    'Dt_SAve.Rows.Add(dr)
                    'Dt_SAve.AcceptChanges()
                    'ViewState("Dtsave") = Dt_SAve
                    ' added by Deepak singh on 23-11=09
                    'For Each dr1 As Data.DataRow In Dt_SAve.Rows
                    'Dim indexrow As Integer = Dt_SAve.Rows.Count - 1
                    'dr1 = Dt_SAve.Rows(indexrow)
                    'dt_DtMedExDtl.ImportRow(dr1)
                    dt_DtMedExDtl.Rows.Add(dr)
                    dt_DtMedExDtl.AcceptChanges()

                End If
                Me.ViewState(VS_DtViewSampleMedExDetail) = dt_DtMedExDtl
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add


            ElseIf mode = AddToDatabase Then

                ds_save = New DataSet
                dt_DtMedExDtl = CType(Me.ViewState(VS_DtViewSampleMedExDetail), DataTable)
                '===added on 23-nov-09====
                If CType(Me.ViewState(VS_Choice), Integer) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                    dv_DtMedExDtl = dt_DtMedExDtl.DefaultView
                    dv_DtMedExDtl.RowFilter = "nSampleId=0"
                    dt_DtMedExDtl = dv_DtMedExDtl.ToTable.Copy

                End If
                If dt_DtMedExDtl.Rows.Count <= 0 Then
                    ObjCommon.ShowAlert("No New Item added ", Me.Page)
                    Exit Sub
                End If
                ds_save.Tables.Add(dt_DtMedExDtl.Copy())
                ds_save.Tables(0).TableName = "SampleMedExDetail"


                '========

                For Each dr In ds_save.Tables(0).Rows
                    dr("nSampleId") = Me.ViewState(VS_SampleId)
                Next

                If ds_save.Tables(0).Rows.Count > 0 Then
                    If Not objLambda.Save_SampleMedEXDetail(Me.ViewState(VS_Choice), ds_save, Me.Session(S_UserID), estr_Retu) Then
                        ObjCommon.ShowAlert("Error While Saving SampleDetail", Me.Page)
                        Me.resetpage()
                        Exit Sub
                    End If

                    ObjCommon.ShowAlert("SampleDetail Saved SuccessFully", Me.Page)

                End If
            End If


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "ResetPage"

    Protected Sub resetpage()

        Me.ViewState(VS_SampleId) = Nothing
        Me.txtproject.Text = ""
        Me.HProjectId.Value = ""

        Me.ddlActivity.Items.Clear()
        Me.ddlPeriod.Items.Clear()
        Me.ddlNode.Items.Clear()
        Me.ddlMedexGroup.Items.Clear()
        Me.ddlMedex.Items.Clear()

        Me.ViewState(VS_Choice) = Nothing
        Me.ViewState(VS_DtMedExMst) = Nothing
        Me.ViewState(VS_DtSubMst) = Nothing
        Me.ViewState(VS_DtViewSampleMedExDetail) = Nothing

        Me.gvwSubject.DataSource = Nothing
        Me.gvwSubject.DataBind()
        Me.gvwMedEx.DataSource = Nothing
        Me.gvwMedEx.DataBind()

        Me.divMedEx.Visible = False
        Me.pnlMedEx.Visible = False

        Me.ViewState(VS_DtWorkspaceSubjectMst) = Nothing
        GenCall()

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

End Class
