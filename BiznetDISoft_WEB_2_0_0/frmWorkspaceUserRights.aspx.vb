
Partial Class frmWorkspaceUserRights
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtWorkSpaceWorkFlowUserDtl As String = "DtWorkSpaceWorkFlowUserDtl"
    Private Const VS_WorkSpaceWorkflowUserId As String = "WorkSpaceWorkflowUserId"
    Private Const VS_WorkspaceId As String = "WorkspaceId"
    Private Const VS_NodeId As String = "NodeId"
    Private Const VS_dsSave As String = "dsSave"
    Private Const VS_DtGrid As String = "dt_Grid"
    Private Const VS_DtStage As String = "dt_Stage"

    Private Const GVC_WorkspaceId As Integer = 0
    Private Const GVC_WorkspaceName As Integer = 1
    Private Const GVC_NodeId As Integer = 2
    Private Const GVC_NodeName As Integer = 3
    Private Const GVC_UserId As Integer = 4
    Private Const GVC_UserName As Integer = 5
    Private Const GVC_StageId As Integer = 6
    Private Const GVC_StageName As Integer = 7
    Private Const GVC_NewStage As Integer = 8
    Private Const GVC_Edit As Integer = 9

#End Region

#Region "Page Load "
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

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Me.ViewState(VS_WorkspaceId) = Me.Request.QueryString("workspaceId").ToString.Trim()
            Me.ViewState(VS_NodeId) = Me.Request.QueryString("NodeId").ToString.Trim()



            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtWorkSpaceWorkFlowUserDtl) = ds.Tables("WorkSpaceWorkFlowUserDtl")   ' adding blank DataTable in viewstate

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

#Region "GenCall_Data "
    Private Function GenCall_Data(ByRef ds_Retu As DataSet) As Boolean

        Dim wStr As String = ""
        Dim eStr_Retu As String = ""
        Dim Val As String = ""
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing
        Dim ds_Delete As New DataSet

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vWorkSpaceId='" & Me.ViewState(VS_WorkspaceId).ToString.Trim() & "' and iNodeId=" & Me.ViewState(VS_NodeId)
            End If

            wStr = "vWorkSpaceId='" & Me.ViewState(VS_WorkspaceId).ToString.Trim() & "' and iNodeId=" & Me.ViewState(VS_NodeId)
            If Not objHelp.getworkspaceWorkflowUserDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            If ds.Tables(0).Rows.Count <= 0 And _
             Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found for Selected Operation")
            End If

            ds_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")

        Finally

        End Try
    End Function

#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim dt_OpMst As DataTable = Nothing
        Dim estr As String = ""
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try


            Page.Title = ":: Project UserRights :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Set User Rights"

            dt_OpMst = Me.ViewState(VS_DtWorkSpaceWorkFlowUserDtl)

            Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit 'Always in Edit Mode

            If Not CreateTable_Grid() Then
                Exit Function
            End If

            If Not FillDropDown() Then
                Exit Function
            End If

            If Not FillGrid() Then
                Exit Function
            End If

            If Not fillStage() Then
                Exit Function
            End If

            Me.btnSave.Visible = False
            Me.BtnUpdate.Visible = False

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function

#End Region

#Region "CreateTable_Grid"

    Private Function CreateTable_Grid() As Boolean
        Dim dt_Grid As New DataTable
        Dim dc As DataColumn
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            dc = New DataColumn
            dc.ColumnName = "nWorkspaceWorkflowUserId"
            dc.DataType = GetType(String)
            dt_Grid.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "vWorkSpaceDesc"
            dc.DataType = GetType(String)
            dt_Grid.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "vWorkSpaceId"
            dc.DataType = GetType(String)
            dt_Grid.Columns.Add(dc)


            dc = New DataColumn
            dc.ColumnName = "iNodeId"
            dc.DataType = GetType(Integer)
            dt_Grid.Columns.Add(dc)


            dc = New DataColumn
            dc.ColumnName = "vNodeDisplayName"
            dc.DataType = GetType(String)
            dt_Grid.Columns.Add(dc)


            dc = New DataColumn
            dc.ColumnName = "iUserid"
            dc.DataType = GetType(Integer)
            dt_Grid.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "vUserName"
            dc.DataType = GetType(String)
            dt_Grid.Columns.Add(dc)
            dc = New DataColumn

            dc = New DataColumn
            dc.ColumnName = "iStageid"
            dc.DataType = GetType(Integer)
            dt_Grid.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "vStageDesc"
            dc.DataType = GetType(String)
            dt_Grid.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "cStatusIndi"
            dc.DataType = GetType(String)
            dt_Grid.Columns.Add(dc)

            Me.ViewState(VS_DtGrid) = dt_Grid
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try

    End Function

#End Region

#Region "Fill Functions"

    Private Function FillDropDown() As Boolean
        Dim ds_UserGroup As New Data.DataSet
        Dim Ds_WorkSpace As New Data.DataSet
        Dim Ds_Node As New Data.DataSet
        Dim estr As String = ""
        Dim wstr As String = ""

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            wstr = "vWorkSpaceId='" & Me.ViewState(VS_WorkspaceId).ToString.Trim() & "' and iNodeId=" & Me.ViewState(VS_NodeId)

            If Not Me.objHelp.getuserGroupMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_UserGroup, estr) Then
                Return False
            End If

            If Not Me.objHelp.getworkspacemst("vWorkSpaceId='" & Me.ViewState(VS_WorkspaceId).ToString.Trim() & "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_WorkSpace, estr) Then
                Return False
            End If

            If Not Me.objHelp.getWorkSpaceNodeDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_Node, estr) Then
                Return False
            End If

            Me.DDLUserGroup.DataSource = ds_UserGroup
            Me.DDLUserGroup.DataValueField = "iUserGroupCode"
            Me.DDLUserGroup.DataTextField = "vUserGroupName"
            Me.DDLUserGroup.DataBind()
            Me.DDLUserGroup.Items.Insert(0, New ListItem("select User Group", 0))

            Me.HdfTempName.Value = Ds_WorkSpace.Tables(0).Rows(0).Item("vWorkSpaceDesc")
            Me.HdfNodeName.Value = Ds_Node.Tables(0).Rows(0).Item("vNodeDisplayName")
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function fillStage() As Boolean
        Dim Ds_Stage As New DataSet
        Dim estr As String = ""

        If Not Me.objHelp.GetStageMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, _
                                            Ds_Stage, estr) Then
            Return False
        End If

        Me.ViewState(VS_DtStage) = Ds_Stage.Tables(0)

        Me.chklstStages.DataSource = Ds_Stage
        Me.chklstStages.DataValueField = "istageid"
        Me.chklstStages.DataTextField = "vStageDesc"
        Me.chklstStages.DataBind()

        Return True

    End Function

    Private Function fillUser() As Boolean
        Dim Ds_User As New Data.DataSet

        Dim Ds_WorkSpace As New Data.DataSet
        Dim Ds_Node As New Data.DataSet
        Dim estr As String = ""
        Dim wstr As String = ""
        Try

            If Me.DDLUserGroup.SelectedIndex = 0 Then
                Return False
            End If


            If Not Me.objHelp.getuserMst("iUserGroupCode= '" & Me.DDLUserGroup.SelectedValue.Trim() & "'", _
                                        WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        Ds_User, estr) Then

                Return False

            End If

            Me.chklstUser.DataSource = Ds_User
            Me.chklstUser.DataValueField = "iUserId"
            Me.chklstUser.DataTextField = "vUserName"
            Me.chklstUser.DataBind()

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function FillGrid() As Boolean
        Dim wStr As String = ""
        Dim eStr_Retu As String = ""
        Dim ds As New DataSet
        Dim dt_Grid As New DataTable
        Dim dt_NewGrid As New DataTable
        Dim dv_Grid As New DataView
        Dim dr1 As DataRow
        Dim index As Integer
        Try
            dt_Grid = Me.ViewState(VS_DtGrid)
            wStr = "vWorkSpaceId='" & Me.ViewState(VS_WorkspaceId).ToString.Trim() & "' and iNodeId=" & Me.ViewState(VS_NodeId)
            Me.objHelp.GetViewWorkspaceWorkflowUserDtl(wStr, ds, eStr_Retu)
            If ds.Tables(0).Rows.Count > 0 Then

                For index = 0 To ds.Tables(0).Rows.Count - 1
                    dr1 = dt_Grid.NewRow()
                    dr1("nWorkspaceWorkflowUserId") = ds.Tables(0).Rows(index).Item("nWorkspaceWorkflowUserId")
                    dr1("vWorkSpaceId") = ds.Tables(0).Rows(index).Item("vWorkSpaceId")
                    dr1("vWorkSpaceDesc") = ds.Tables(0).Rows(index).Item("vWorkSpaceDesc")
                    dr1("iNodeId") = ds.Tables(0).Rows(index).Item("iNodeId")
                    dr1("vNodeDisplayName") = ds.Tables(0).Rows(index).Item("vNodeDisplayName")
                    dr1("iUserId") = ds.Tables(0).Rows(index).Item("iUserId")
                    dr1("vUserName") = ds.Tables(0).Rows(index).Item("vUserName")
                    dr1("iStageId") = ds.Tables(0).Rows(index).Item("iStageId")
                    dr1("vStageDesc") = ds.Tables(0).Rows(index).Item("vStageDesc")
                    dr1("cStatusIndi") = ds.Tables(0).Rows(index).Item("cStatusIndi")
                    dt_Grid.Rows.Add(dr1)
                Next index

                If ds.Tables(0).Rows.Count > 0 Then
                    Me.ViewState(VS_DtGrid) = dt_Grid
                End If

                dv_Grid = CType(Me.ViewState(VS_DtGrid), DataTable).DefaultView
                dv_Grid.RowFilter = "cStatusIndi <> 'D'"
                dt_NewGrid = dv_Grid.ToTable
                Me.GV_UserStage.DataSource = dt_NewGrid
                Me.GV_UserStage.DataBind()

                Me.GV_UserStage_Edit.DataSource = dt_NewGrid
                Me.GV_UserStage_Edit.DataBind()
                Me.DivAdd.Visible = True
                Me.DivEdit.Visible = False
            End If

            Return True

        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "SelectedIndexChanged"
    Protected Sub DDLUserGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLUserGroup.SelectedIndexChanged
        If Not fillUser() Then
        End If
    End Sub
#End Region

#Region "Assign values"

    Private Sub AssignValues(ByVal Type As String)
        Dim dr As DataRow
        Dim dr1 As DataRow
        Dim index_User As Integer
        Dim index_Stage As Integer
        Dim Index_Row As Integer
        Dim dt_UserRights As New DataTable
        Dim dt_NewGrid As New DataTable
        Dim dt_Grid As New DataTable
        Dim dv_Grid As New DataView
        dt_UserRights = CType(Me.ViewState(VS_DtWorkSpaceWorkFlowUserDtl), DataTable)
        dt_Grid = CType(Me.ViewState(VS_DtGrid), DataTable)
        If Type.ToUpper = "ADD" Then

            For index_User = 0 To Me.chklstUser.Items.Count - 1

                If Me.chklstUser.Items(index_User).Selected Then

                    For index_Stage = 0 To Me.chklstStages.Items.Count - 1

                        If Me.chklstStages.Items(index_Stage).Selected Then

                            For Index_Row = 0 To dt_UserRights.Rows.Count - 1

                                If dt_UserRights.Rows(Index_Row).Item("iUserId") = Me.chklstUser.Items(index_User).Value And dt_UserRights.Rows(Index_Row).Item("iStageId") = Me.chklstStages.Items(index_Stage).Value And dt_UserRights.Rows(Index_Row).Item("cStatusIndi") <> "D" Then
                                    Me.ObjCommon.ShowAlert("UserId ='" & Me.chklstUser.Items(index_User).Text & "(" & Me.chklstUser.Items(index_User).Value & ")' And StageId = '" & _
                                                            Me.chklstStages.Items(index_Stage).Text & "(" & Me.chklstStages.Items(index_Stage).Value & ")' Already Added", Me.Page)
                                    Exit Sub
                                End If

                            Next Index_Row

                            'For Grid
                            dr1 = dt_Grid.NewRow()
                            dr1("nWorkspaceWorkflowUserId") = "-" & index_User.ToString.Trim() & index_Stage.ToString.Trim() & Me.chklstUser.Items(index_User).Value.Trim() & Me.chklstStages.Items(index_Stage).Value.Trim() '"00"
                            dr1("vWorkSpaceId") = Me.ViewState(VS_WorkspaceId)
                            dr1("vWorkSpaceDesc") = Me.HdfTempName.Value.Trim()
                            dr1("iNodeId") = Me.ViewState(VS_NodeId)
                            dr1("vNodeDisplayName") = Me.HdfNodeName.Value.Trim()
                            dr1("iUserId") = Me.chklstUser.Items(index_User).Value
                            dr1("vUserName") = Me.chklstUser.Items(index_User).Text
                            dr1("iStageId") = Me.chklstStages.Items(index_Stage).Value
                            dr1("vStageDesc") = Me.chklstStages.Items(index_Stage).Text
                            dr1("cStatusIndi") = "N"
                            dt_Grid.Rows.Add(dr1)

                            'For Save
                            dr = dt_UserRights.NewRow()
                            dr("nWorkspaceWorkflowUserId") = "-" & index_User.ToString.Trim() & index_Stage.ToString.Trim() & Me.chklstUser.Items(index_User).Value.Trim() & Me.chklstStages.Items(index_Stage).Value.Trim() '"00"
                            dr("vWorkSpaceId") = Me.ViewState(VS_WorkspaceId)
                            dr("iNodeId") = Me.ViewState(VS_NodeId)
                            dr("iUserId") = Me.chklstUser.Items(index_User).Value
                            dr("iStageId") = Me.chklstStages.Items(index_Stage).Value
                            dr("cCanEdit") = "Y"
                            dr("cCanRead") = "Y"
                            dr("cCanDelete") = "Y"
                            dr("iModifyBy") = Me.Session(S_UserID)
                            dr("cStatusIndi") = "N"
                            dt_UserRights.Rows.Add(dr)

                            Me.btnSave.Visible = True
                        End If

                    Next index_Stage

                End If

            Next index_User

            Me.ViewState(VS_DtWorkSpaceWorkFlowUserDtl) = dt_UserRights
            Me.ViewState(VS_DtGrid) = dt_Grid
            dv_Grid = CType(Me.ViewState(VS_DtGrid), DataTable).DefaultView
            dv_Grid.RowFilter = "cStatusIndi <> 'D'"
            dt_NewGrid = dv_Grid.ToTable
            Me.GV_UserStage.DataSource = dt_NewGrid
            Me.GV_UserStage.DataBind()

            Me.GV_UserStage_Edit.DataSource = dt_NewGrid
            Me.GV_UserStage_Edit.DataBind()

        End If

    End Sub

#End Region

#Region "Button Click"
    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click
        AssignValues("ADD")

    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim ds_Save As New DataSet
        Dim dt_Save As New DataTable
        Dim dv_Save As New DataView

        Dim ds_Delete As New DataSet
        Dim dt_Delete As New DataTable
        Dim dv_Delete As New DataView
        Dim Success As Boolean = False
        Dim estr As String = ""
        Try
            'AssignValues()
            dv_Save = CType(Me.ViewState(VS_DtWorkSpaceWorkFlowUserDtl), DataTable).DefaultView
            dv_Save.RowFilter = "nWorkspaceWorkflowUserId<0"
            dt_Save = dv_Save.ToTable
            If dt_Save.Rows.Count > 0 Then
                'dt_save.TableName=
                ds_Save = New DataSet
                dt_Save.TableName = "WorkSpaceWorkFlowUserDtl"
                ds_Save.Tables.Add(dt_Save)
                If Not objLambda.Save_InsertWorkspaceWorkflowUserDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Save, "No", Me.Session(S_UserID), estr) Then
                    ObjCommon.ShowAlert("Error While Saving WorkSpaceWorkflowUserDtl", Me.Page)
                    Exit Sub
                Else
                    Success = True
                    'ObjCommon.ShowAlert("Record Saved SuccessFuly", Me.Page)
                    'ResetPage()
                End If
            End If

            'For Deleted
            dv_Delete = CType(Me.ViewState(VS_DtWorkSpaceWorkFlowUserDtl), DataTable).DefaultView
            dv_Delete.RowFilter = "(nWorkspaceWorkflowUserId>=0 and cStatusIndi <> 'N')"
            dt_Delete = dv_Delete.ToTable
            If dt_Delete.Rows.Count > 0 Then
                ds_Delete = New DataSet
                dt_Delete.TableName = "WorkSpaceWorkFlowUserDtl"
                ds_Delete.Tables.Add(dt_Delete)
                If Not objLambda.Save_InsertWorkspaceWorkflowUserDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_Delete, "No", Me.Session(S_UserID), estr) Then
                    ObjCommon.ShowAlert("Error While Saving WorkSpaceWorkflowUserDtl", Me.Page)
                    Exit Sub
                Else
                    Success = True
                    'ObjCommon.ShowAlert("Record Saved SuccessFuly", Me.Page)
                    'ResetPage()
                End If
            End If

            If Success = True Then
                ObjCommon.ShowAlert("Record Saved SuccessFully", Me.Page)
                ResetPage()
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub BtnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds_Update As New DataSet
        Dim dt_Update As New DataTable
        Dim dv_Update As New DataView
        Dim Success As Boolean = False
        Dim estr As String = ""

        'For Updated
        'dv_Update = CType(Me.ViewState(VS_DtWorkSpaceWorkFlowUserDtl), DataTable).DefaultView
        'dv_Update.RowFilter = "nWorkspaceWorkflowUserId>=0"
        dt_Update = CType(Me.ViewState(VS_DtWorkSpaceWorkFlowUserDtl), DataTable)
        ds_Update = New DataSet
        dt_Update.TableName = "WorkSpaceWorkFlowUserDtl"
        ds_Update.Tables.Add(dt_Update)
        If Not objLambda.Save_InsertWorkspaceWorkflowUserDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_Update, "No", Me.Session(S_UserID), estr) Then
            ObjCommon.ShowAlert("Error While Saving WorkSpaceWorkflowUserDtl", Me.Page)
            Exit Sub
        Else
            ObjCommon.ShowAlert("Record Saved SuccessFully", Me.Page)
            ResetPage()

        End If
    End Sub

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Dim DMS As String = ""
        ResetPage()
        Me.Response.Redirect(Me.Request.QueryString("Page2").Trim() & ".aspx?WorkSpaceId=" & Me.ViewState(VS_WorkspaceId) & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Type=" & Me.Request.QueryString("Type") & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))

    End Sub
#End Region

#Region "Grid Event"

    Protected Sub GV_UserStage_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim Index As New Integer
        Dim Index_Row As New Integer
        Dim dt_UserRights As New DataTable
        Dim dt_Grid As New DataTable
        Dim dt_NewGrid As New DataTable
        Dim dv_Grid As New DataView

        dt_Grid = CType(Me.ViewState(VS_DtGrid), DataTable)
        dt_UserRights = CType(Me.ViewState(VS_DtWorkSpaceWorkFlowUserDtl), DataTable)

        Index = e.CommandArgument

        If e.CommandName.ToUpper = "DELETE" Then

            For Index_Row = 0 To dt_UserRights.Rows.Count - 1

                If dt_UserRights.Rows(Index_Row).Item("iUserId") = GV_UserStage.Rows(Index).Cells(GVC_UserId).Text And _
                    dt_UserRights.Rows(Index_Row).Item("iStageId") = GV_UserStage.Rows(Index).Cells(GVC_StageId).Text Then

                    If dt_UserRights.Rows(Index_Row)("nWorkspaceWorkflowUserId") < 0 Then
                        dt_UserRights.Rows(Index_Row).Delete()
                        dt_UserRights.AcceptChanges()
                    Else
                        dt_UserRights.Rows(Index_Row)("iModifyBy") = Me.Session(S_UserID)
                        dt_UserRights.Rows(Index_Row)("cStatusIndi") = "D"
                        dt_UserRights.Rows(Index_Row).AcceptChanges()
                        dt_UserRights.AcceptChanges()
                    End If

                End If

                If dt_Grid.Rows(Index_Row).Item("iUserId") = GV_UserStage.Rows(Index).Cells(GVC_UserId).Text And _
                    dt_Grid.Rows(Index_Row).Item("iStageId") = GV_UserStage.Rows(Index).Cells(GVC_StageId).Text Then

                    If dt_Grid.Rows(Index_Row)("nWorkspaceWorkflowUserId") < 0 Then
                        dt_Grid.Rows(Index_Row).Delete()
                        dt_Grid.AcceptChanges()
                    Else
                        dt_Grid.Rows(Index_Row)("cStatusIndi") = "D"
                        dt_Grid.Rows(Index_Row).AcceptChanges()
                        dt_Grid.AcceptChanges()
                    End If

                End If

            Next Index_Row

        End If

        Me.ViewState(VS_DtWorkSpaceWorkFlowUserDtl) = dt_UserRights
        Me.ViewState(VS_DtGrid) = dt_Grid

        dv_Grid = CType(Me.ViewState(VS_DtGrid), DataTable).DefaultView
        dv_Grid.RowFilter = "cStatusIndi <> 'D'"
        dt_NewGrid = dv_Grid.ToTable
        Me.GV_UserStage.DataSource = dt_NewGrid
        Me.GV_UserStage.DataBind()

        Me.GV_UserStage_Edit.DataSource = dt_NewGrid
        Me.GV_UserStage_Edit.DataBind()

        Me.btnSave.Visible = True

    End Sub

    Protected Sub GV_UserStage_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("ImbMDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImbMDelete"), ImageButton).CommandName = "Delete"
        End If
    End Sub

    Protected Sub GV_UserStage_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)

    End Sub

    Protected Sub GV_UserStage_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_UserStage.RowCreated
        e.Row.Cells(GVC_WorkspaceId).Visible = False
        e.Row.Cells(GVC_NodeId).Visible = False
        e.Row.Cells(GVC_UserId).Visible = False
        e.Row.Cells(GVC_StageId).Visible = False
    End Sub

    Protected Sub GV_UserStage_Edit_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("LnkEdit"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("LnkEdit"), LinkButton).CommandName = "Edit"

            CType(e.Row.FindControl("DDLStages"), DropDownList).Enabled = False

        End If

    End Sub

    Protected Sub GV_UserStage_Edit_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim index As Integer = e.CommandArgument
        Dim dt_UserRights As New DataTable
        Dim dt_Grid As New DataTable
        Dim dt_NewGrid As New DataTable
        Dim dv_Grid As New DataView
        Dim Index_Row As New Integer

        If e.CommandName.ToUpper = "EDIT" Then
            CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).Enabled = True
            CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).DataSource = CType(Me.ViewState(VS_DtStage), DataTable)
            CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).DataValueField = "istageid"
            CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).DataTextField = "vStageDesc"
            CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).DataBind()


            CType(GV_UserStage_Edit.Rows(index).FindControl("LnkEdit"), LinkButton).CommandName = "Update"
            CType(GV_UserStage_Edit.Rows(index).FindControl("LnkEdit"), LinkButton).Text = "Update"

        ElseIf e.CommandName.ToUpper = "UPDATE" Then

            dt_Grid = CType(Me.ViewState(VS_DtGrid), DataTable)
            dt_UserRights = CType(Me.ViewState(VS_DtWorkSpaceWorkFlowUserDtl), DataTable)

            For Index_Row = 0 To dt_UserRights.Rows.Count - 1

                If dt_UserRights.Rows(Index_Row).Item("iUserId") = GV_UserStage_Edit.Rows(index).Cells(GVC_UserId).Text And _
                    dt_UserRights.Rows(Index_Row).Item("iStageId") = CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).SelectedValue.Trim() And _
                    dt_UserRights.Rows(Index_Row).Item("cStatusIndi") <> "D" Then

                    Me.ObjCommon.ShowAlert("UserId ='" & Me.GV_UserStage_Edit.Rows(index).Cells(GVC_UserName).Text & "(" & _
                                            Me.GV_UserStage_Edit.Rows(index).Cells(GVC_UserId).Text & ")' And StageId = '" & _
                                            CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).SelectedItem.Text.Trim() & _
                                            "(" & CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).SelectedValue.Trim() & _
                                            ")' Already Added", Me.Page)
                    Exit Sub

                End If

                If dt_UserRights.Rows(Index_Row).Item("iUserId") = GV_UserStage_Edit.Rows(index).Cells(GVC_UserId).Text And _
                    dt_UserRights.Rows(Index_Row).Item("iStageId") = GV_UserStage_Edit.Rows(index).Cells(GVC_StageId).Text Then

                    dt_UserRights.Rows(Index_Row)("iStageId") = CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).SelectedValue.Trim()
                    dt_UserRights.Rows(Index_Row)("iModifyBy") = Me.Session(S_UserID)
                    dt_UserRights.Rows(Index_Row)("cStatusIndi") = "E"
                    dt_UserRights.Rows(Index_Row).AcceptChanges()
                    dt_UserRights.AcceptChanges()

                End If

                If dt_Grid.Rows(Index_Row).Item("iUserId") = GV_UserStage_Edit.Rows(index).Cells(GVC_UserId).Text And _
                    dt_Grid.Rows(Index_Row).Item("iStageId") = GV_UserStage_Edit.Rows(index).Cells(GVC_StageId).Text Then

                    dt_Grid.Rows(Index_Row)("iStageId") = CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).SelectedValue.Trim()
                    dt_Grid.Rows(Index_Row)("vStageDesc") = CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).SelectedItem.Text.Trim()
                    dt_Grid.Rows(Index_Row)("cStatusIndi") = "E"
                    dt_Grid.Rows(Index_Row).AcceptChanges()
                    dt_Grid.AcceptChanges()

                End If

                Me.ViewState(VS_DtGrid) = dt_Grid
                Me.ViewState(VS_DtWorkSpaceWorkFlowUserDtl) = dt_UserRights

            Next Index_Row

            dv_Grid = CType(Me.ViewState(VS_DtGrid), DataTable).DefaultView
            dv_Grid.RowFilter = "cStatusIndi <> 'D'"
            dt_NewGrid = dv_Grid.ToTable
            Me.GV_UserStage.DataSource = dt_NewGrid
            Me.GV_UserStage.DataBind()

            Me.GV_UserStage_Edit.DataSource = dt_NewGrid
            Me.GV_UserStage_Edit.DataBind()

            Me.BtnUpdate.Visible = True

            CType(GV_UserStage_Edit.Rows(index).FindControl("LnkEdit"), LinkButton).CommandName = "Edit"
            CType(GV_UserStage_Edit.Rows(index).FindControl("LnkEdit"), LinkButton).Text = "Edit"

        End If
    End Sub

    Protected Sub GV_UserStage_Edit_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs)
        e.Cancel = True
    End Sub

    Protected Sub GV_UserStage_Edit_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs)

    End Sub

    Protected Sub GV_UserStage_Edit_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        e.Row.Cells(GVC_WorkspaceId).Visible = False
        e.Row.Cells(GVC_NodeId).Visible = False
        e.Row.Cells(GVC_UserId).Visible = False
        e.Row.Cells(GVC_StageId).Visible = False
    End Sub

#End Region

#Region "RadioButton Event"

    Protected Sub RbEdit_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbEdit.CheckedChanged
        If Me.RbEdit.Checked = True Then
            Me.DivAdd.Visible = False
            Me.DivEdit.Visible = True

        ElseIf Me.RbDelete.Checked = True Then
            Me.DivAdd.Visible = False
            Me.DivEdit.Visible = False

        ElseIf Me.RbAdd.Checked = True Then
            Me.DivAdd.Visible = True
            Me.DivEdit.Visible = False

        End If
    End Sub

    Protected Sub RbDelete_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbDelete.CheckedChanged
        If Me.RbEdit.Checked = True Then
            Me.DivAdd.Visible = False
            Me.DivEdit.Visible = True

        ElseIf Me.RbDelete.Checked = True Then
            Me.DivAdd.Visible = False
            Me.DivEdit.Visible = False

        ElseIf Me.RbAdd.Checked = True Then
            Me.DivAdd.Visible = True
            Me.DivEdit.Visible = False

        End If

    End Sub

    Protected Sub RbAdd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbAdd.CheckedChanged
        If Me.RbEdit.Checked = True Then
            Me.DivAdd.Visible = False
            Me.DivEdit.Visible = True

        ElseIf Me.RbDelete.Checked = True Then
            Me.DivAdd.Visible = False
            Me.DivEdit.Visible = False

            'FillDivEmp()
        ElseIf Me.RbAdd.Checked = True Then
            Me.DivAdd.Visible = True
            Me.DivEdit.Visible = False

        End If
    End Sub

#End Region

#Region "ResetPage"

    Private Sub ResetPage()
        Me.ViewState(VS_DtWorkSpaceWorkFlowUserDtl) = Nothing
        Me.DDLUserGroup.SelectedIndex = 0
        Me.chklstUser.Items.Clear()
        Me.chklstStages.Items.Clear()
        Me.GV_UserStage.DataSource = Nothing
        Me.GV_UserStage.DataBind()
        Me.GV_UserStage_Edit.DataSource = Nothing
        Me.GV_UserStage_Edit.DataBind()
        Me.btnSave.Visible = False
        Me.BtnUpdate.Visible = True

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
