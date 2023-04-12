
Partial Class frmEditUserRights
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtTemplateWorkFlowUserDtl As String = "DtTemplateWorkFlowUserDtl"
    Private Const VS_TemplateWorkflowUserId As String = "TemplateWorkflowUserId"
    Private Const VS_DtActivityOperationMxt As String = "DtActivityOperationMxt"
    Private Const VS_TemplateId As String = "TemplateId"
    Private Const VS_NodeId As String = "NodeId"
    Private Const VS_dsSave As String = "dsSave"
    Private Const VS_DtGrid As String = "dt_Grid"
    Private Const VS_DtStage As String = "dt_Stage"

    Private Const GVC_US_Delete As Integer = 0
    Private Const GVC_US_TemplateId As Integer = 1
    Private Const GVC_US_TemplateDesc As Integer = 2
    Private Const GVC_US_NodeId As Integer = 3
    Private Const GVC_US_NodeDisplayName As Integer = 4
    Private Const GVC_US_UserId As Integer = 5
    Private Const GVC_US_UserName As Integer = 6
    Private Const GVC_US_StageId As Integer = 7
    Private Const GVC_US_StageDesc As Integer = 8

    Private Const GVC_USE_TemplateId As Integer = 0
    Private Const GVC_USE_TemplateDesc As Integer = 1
    Private Const GVC_USE_NodeId As Integer = 2
    Private Const GVC_USE_NodeDisplayName As Integer = 3
    Private Const GVC_USE_UserId As Integer = 4
    Private Const GVC_USE_UserName As Integer = 5
    Private Const GVC_USE_StageId As Integer = 6
    Private Const GVC_USE_StageDesc As Integer = 7

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

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Me.ViewState(VS_Choice) = Me.Request.QueryString("mode").ToString.Trim()
            Me.ViewState(VS_TemplateId) = Me.Request.QueryString("TemplateId").ToString.Trim()
            Me.ViewState(VS_NodeId) = Me.Request.QueryString("NodeId").ToString.Trim()


            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtTemplateWorkFlowUserDtl) = ds.Tables("TemplateWorkFlowUserDtl")   ' adding blank DataTable in viewstate

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
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vTemplateId='" & Me.ViewState(VS_TemplateId).ToString.Trim() & "' and iNodeId=" & Me.ViewState(VS_NodeId)
            End If


            If Not objHelp.getTemplateWorkflowUserDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
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
        Dim Ds_Stage As New DataSet
        Dim estr As String = ""
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = " :: Set User Rights  ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Set User Rights"

            dt_OpMst = Me.ViewState(VS_DtTemplateWorkFlowUserDtl)

            Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit 'Always in Edit Mode

            If Not CreateTable_Grid() Then
                Exit Function
            End If

            If Not FillDropDown() Then
                Exit Function
            End If

            'As per Suggested By Riddhi mam
            'Me.objHelp.GetNodeidwiseStage(Me.ViewState(VS_NodeId), Me.ViewState(VS_TemplateId), Ds_Stage, estr)
            If Not Me.objHelp.GetStageMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, Ds_Stage, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Stages", Me.Page)
                Exit Function
            End If
            Me.ViewState(VS_DtStage) = Ds_Stage.Tables(0)
            '*************************

            If Not FillGrid() Then
                Exit Function
            End If

            Me.btnSave.Enabled = False
            Me.BtnUpdate.Enabled = False

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
            dc.ColumnName = "nTemplateWorkflowUserId"
            dc.DataType = GetType(String)
            dt_Grid.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "vTemplateDesc"
            dc.DataType = GetType(String)
            dt_Grid.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "vTemplateId"
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

#Region "FillDropDown"

    Private Function FillDropDown() As Boolean
        Dim ds_UserGroup As New Data.DataSet
        Dim Ds_Template As New Data.DataSet
        Dim Ds_Node As New Data.DataSet
        Dim estr As String = ""
        Dim wstr As String = ""

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            wstr = "vTemplateId='" & Me.ViewState(VS_TemplateId).ToString.Trim() & "' and iNodeId=" & Me.ViewState(VS_NodeId)

            Me.objHelp.getuserGroupMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_UserGroup, estr)
            Me.objHelp.getTemplateMst("vTemplateId='" & Me.ViewState(VS_TemplateId).ToString.Trim() & "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_Template, estr)
            Me.objHelp.getTemplateNodeDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_Node, estr)

            Me.DDLUserGroup.DataSource = ds_UserGroup
            Me.DDLUserGroup.DataValueField = "iUserGroupCode"
            Me.DDLUserGroup.DataTextField = "vUserGroupName"
            Me.DDLUserGroup.DataBind()
            Me.DDLUserGroup.Items.Insert(0, New ListItem("select User Group", 0))

            Me.HdfTempName.Value = Ds_Template.Tables(0).Rows(0).Item("vTemplateDesc")
            Me.HdfNodeName.Value = Ds_Node.Tables(0).Rows(0).Item("vNodeDisplayName")

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function fillUserAndStage() As Boolean
        Dim Ds_User As New Data.DataSet
        Dim Dt_Stage As New Data.DataTable
        Dim Ds_Template As New Data.DataSet
        Dim Ds_Node As New Data.DataSet
        Dim estr As String = ""
        Dim wstr As String = ""
        Dim Wstr_Scope As String = ""
        Try

            If Me.DDLUserGroup.SelectedIndex = 0 Then
                Return False
            End If


            Me.objHelp.GetViewUserMst("iUserGroupCode= '" & Me.DDLUserGroup.SelectedValue.Trim() & "'" & _
                                        " And nScopeNo=" & Me.Session(S_ScopeNo) & " And cStatusindi<>'D'", _
                                        WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_User, estr)

            Dt_Stage = Me.ViewState(VS_DtStage)

            Me.chklstUser.DataSource = Ds_User
            Me.chklstUser.DataValueField = "iUserId"
            Me.chklstUser.DataTextField = "vUserName"
            Me.chklstUser.DataBind()

            Me.chklstStages.DataSource = Dt_Stage
            Me.chklstStages.DataValueField = "istageid"
            Me.chklstStages.DataTextField = "vStageDesc"
            Me.chklstStages.DataBind()

            Return True
        Catch ex As Exception
            Return True
        End Try
    End Function

    Private Function FillGrid() As Boolean
        Dim wStr As String = ""
        Dim eStr_Retu As String = ""
        Dim ds As New DataSet
        Dim ds_Dtl As New DataSet
        Dim dt_Grid As New DataTable
        Dim dt_NewGrid As New DataTable
        Dim dv_Grid As New DataView
        Dim dr1 As DataRow
        Dim Index As Integer
        Try

            dt_Grid = Me.ViewState(VS_DtGrid)
            wStr = "vTemplateId='" & Me.ViewState(VS_TemplateId).ToString.Trim() & "' and iNodeId=" & Me.ViewState(VS_NodeId)

            Me.objHelp.GetViewTemplateWorkflowUserDtl(wStr, ds, eStr_Retu)
            Me.objHelp.getTemplateWorkflowUserDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Dtl, eStr_Retu)
            Me.ViewState(VS_DtTemplateWorkFlowUserDtl) = ds_Dtl.Tables("TemplateWorkFlowUserDtl")

            For Index = 0 To ds.Tables(0).Rows.Count - 1

                dr1 = dt_Grid.NewRow()
                dr1("nTemplateWorkflowUserId") = ds.Tables(0).Rows(Index).Item("nTemplateWorkflowUserId")
                dr1("vTemplateId") = ds.Tables(0).Rows(Index).Item("vTemplateId")
                dr1("vTemplateDesc") = ds.Tables(0).Rows(Index).Item("vTemplateDesc")
                dr1("iNodeId") = ds.Tables(0).Rows(Index).Item("iNodeId")
                dr1("vNodeDisplayName") = ds.Tables(0).Rows(Index).Item("vNodeDisplayName")
                dr1("iUserId") = ds.Tables(0).Rows(Index).Item("iUserId")
                dr1("vUserName") = ds.Tables(0).Rows(Index).Item("vUserName")
                dr1("iStageId") = ds.Tables(0).Rows(Index).Item("iStageId")
                dr1("vStageDesc") = ds.Tables(0).Rows(Index).Item("vStageDesc")
                dr1("cStatusIndi") = ds.Tables(0).Rows(Index).Item("cStatusIndi")
                dt_Grid.Rows.Add(dr1)

            Next Index

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

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "SelectedIndexChanged"

    Protected Sub DDLUserGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLUserGroup.SelectedIndexChanged
        fillUserAndStage()
    End Sub

#End Region

#Region "Assign values"

    Private Sub AssignValues(ByVal Type As String)
        Dim dr As DataRow
        Dim dr1 As DataRow
        Dim dr2 As DataRow
        Dim Index1 As Integer
        Dim Index2 As Integer
        Dim Index3 As Integer

        Dim dt_UserRights As New DataTable
        Dim dt_NewGrid As New DataTable
        Dim dt_Grid As New DataTable
        Dim dt_ActOperationMxt As New DataTable
        Dim dv_Grid As New DataView
        Dim Wstr As String = ""
        Dim ds_Dtl As New DataSet
        Dim ds_UserMst As New DataSet
        Dim eStr_Retu As String = ""
        Dim ds_ChildActivity As New DataSet
        Dim cntChild As Integer

        'Retrive ActivityId from TemplateNodeDetail
        Wstr = "vTemplateId='" & Me.ViewState(VS_TemplateId).ToString.Trim() & "' and iNodeId=" & Me.ViewState(VS_NodeId)
        Me.objHelp.getTemplateNodeDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Dtl, eStr_Retu)

        dt_UserRights = CType(Me.ViewState(VS_DtTemplateWorkFlowUserDtl), DataTable)
        dt_Grid = CType(Me.ViewState(VS_DtGrid), DataTable)
        dt_ActOperationMxt = CType(Me.ViewState(VS_DtActivityOperationMxt), DataTable)

        If Type.ToUpper = "ADD" Then
            'dt_UserRights.Clear()
            For Index1 = 0 To Me.chklstUser.Items.Count - 1

                If Me.chklstUser.Items(Index1).Selected Then

                    For Index2 = 0 To Me.chklstStages.Items.Count - 1

                        If Me.chklstStages.Items(Index2).Selected Then

                            For Index3 = 0 To dt_UserRights.Rows.Count - 1

                                If dt_UserRights.Rows(Index3).Item("iUserId") = Me.chklstUser.Items(Index1).Value And dt_UserRights.Rows(Index3).Item("iStageId") = Me.chklstStages.Items(Index2).Value And dt_UserRights.Rows(Index3).Item("cStatusIndi") <> "D" Then
                                    Me.ObjCommon.ShowAlert("UserId ='" & Me.chklstUser.Items(Index1).Text & "(" & Me.chklstUser.Items(Index1).Value & ")' And StageId = '" & _
                                                         Me.chklstStages.Items(Index2).Text & "(" & Me.chklstStages.Items(Index2).Value & ")' Already Added", Me.Page)

                                    Exit Sub

                                End If

                            Next Index3

                            'For Grid
                            dr1 = dt_Grid.NewRow()
                            dr1("nTemplateWorkflowUserId") = "-" & Index1.ToString.Trim() & Index2.ToString.Trim() & Me.chklstUser.Items(Index1).Value.Trim() & Me.chklstStages.Items(Index2).Value.Trim() '"00"
                            dr1("vTemplateId") = Me.ViewState(VS_TemplateId)
                            dr1("vTemplateDesc") = Me.HdfTempName.Value.Trim()
                            dr1("iNodeId") = Me.ViewState(VS_NodeId)
                            dr1("vNodeDisplayName") = Me.HdfNodeName.Value.Trim()
                            dr1("iUserId") = Me.chklstUser.Items(Index1).Value
                            dr1("vUserName") = Me.chklstUser.Items(Index1).Text
                            dr1("iStageId") = Me.chklstStages.Items(Index2).Value
                            dr1("vStageDesc") = Me.chklstStages.Items(Index2).Text
                            dr1("cStatusIndi") = "N"
                            dt_Grid.Rows.Add(dr1)

                            dr = dt_UserRights.NewRow()
                            dr("nTemplateWorkflowUserId") = "-" & Index1.ToString.Trim() & Index2.ToString.Trim() & Me.chklstUser.Items(Index1).Value.Trim() & Me.chklstStages.Items(Index2).Value.Trim() '"00"
                            dr("vTemplateId") = Me.ViewState(VS_TemplateId)
                            dr("iNodeId") = Me.ViewState(VS_NodeId)
                            dr("iUserId") = Me.chklstUser.Items(Index1).Value
                            dr("iStageId") = Me.chklstStages.Items(Index2).Value
                            dr("cCanEdit") = "Y"
                            dr("cCanRead") = "Y"
                            dr("cCanDelete") = "Y"
                            dr("iModifyBy") = Me.Session(S_UserID)
                            dr("cStatusIndi") = "N"
                            dt_UserRights.Rows.Add(dr)

                            Me.btnSave.Enabled = True


                        End If
                    Next Index2

                    ds_ChildActivity = Me.objHelp.GetResultSet("select * from CreateTreeforTemplate('" & Me.ViewState(VS_TemplateId).ToString.Trim() & "'," & Me.ViewState(VS_NodeId).ToString.Trim() & ")", "ChildActivity")

                    For cntChild = 0 To ds_ChildActivity.Tables(0).Rows.Count() - 1
                        'For Save Activity Operation Matrix
                        'Retrive vuserTypeCode,vDeptCode,vLocationCode from UserMst   

                        Wstr = "iUserId='" & Me.chklstUser.Items(Index1).Value & "'"
                        Me.objHelp.getuserMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_UserMst, eStr_Retu)

                        dt_ActOperationMxt.DefaultView.RowFilter = "vActivityId='" & ds_ChildActivity.Tables(0).Rows(cntChild)("vActivityId").ToString() & "'" & _
                                                                    " and vUserTypeCode='" & ds_UserMst.Tables(0).Rows(0)("vUserTypeCode") & "'"

                        If dt_ActOperationMxt.DefaultView.ToTable().Rows.Count <= 0 Then
                            dr2 = dt_ActOperationMxt.NewRow()
                            dr2("vActivityRoleId") = cntChild.ToString.Trim() & Me.chklstUser.Items(Index1).Value.Trim() & ds_ChildActivity.Tables(0).Rows(cntChild)("vActivityId").ToString()
                            dr2("vActivityId") = ds_ChildActivity.Tables(0).Rows(cntChild)("vActivityId").ToString()
                            dr2("vUserTypeCode") = ds_UserMst.Tables(0).Rows(0)("vUserTypeCode")
                            dr2("vDeptCode") = ds_UserMst.Tables(0).Rows(0)("vDeptCode")
                            dr2("vLocationCode") = ds_UserMst.Tables(0).Rows(0)("vLocationCode")
                            dr2("vOperationCode") = GeneralModule.Op_Slot 'HardCoded for OperationCode Slot
                            dr2("iModifyBy") = Me.Session(S_UserID)
                            dr2("cStatusIndi") = "N"
                            dt_ActOperationMxt.Rows.Add(dr2)
                        End If

                    Next cntChild

                End If

            Next Index1

            Me.ViewState(VS_DtTemplateWorkFlowUserDtl) = dt_UserRights
            Me.ViewState(VS_DtGrid) = dt_Grid
            Me.ViewState(VS_DtActivityOperationMxt) = dt_ActOperationMxt

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

#Region "PapulateActOperationMxtData"

    Public Sub PapulateActOperationMxtData()
        Dim ds_Save As New DataSet
        Dim dt_Save As New DataTable
        Dim dv_Save As New DataView
        Dim estr As String = ""

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            dv_Save = CType(Me.ViewState(VS_DtActivityOperationMxt), DataTable).DefaultView
            dt_Save = dv_Save.ToTable
            If dt_Save.Rows.Count > 0 Then

                ds_Save = New DataSet
                dt_Save.TableName = "ActivityOperationMatrix"
                ds_Save.Tables.Add(dt_Save)

                If Not objLambda.Save_InsertActivityOperationMatrix(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Save, Me.Session(S_UserID), estr) Then

                    ObjCommon.ShowAlert("Error While Saving TemplateWorkflowUserDtl", Me.Page)
                    Exit Sub

                End If

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try


    End Sub

#End Region

#Region "Button Click"

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click
        Dim eStr_Retu As String = ""
        Dim wStr As String = "1=2"
        Dim ds As DataSet = Nothing

        If IsNothing(Me.ViewState(VS_DtActivityOperationMxt)) Then

            If Not objHelp.GetActivityOperationMatrix(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then

                Me.ObjCommon.ShowAlert(eStr_Retu, Me.Page())
                Exit Sub

            End If

            Me.ViewState(VS_DtActivityOperationMxt) = ds.Tables("ActivityOperationMatrix")   ' adding blank DataTable in viewstate
        End If

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

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            dv_Save = CType(Me.ViewState(VS_DtTemplateWorkFlowUserDtl), DataTable).DefaultView
            dv_Save.RowFilter = "nTemplateWorkflowUserId<0"
            dt_Save = dv_Save.ToTable
            If dt_Save.Rows.Count > 0 Then

                ds_Save = New DataSet
                dt_Save.TableName = "TemplateWorkFlowUserDtl"
                ds_Save.Tables.Add(dt_Save)
                If Not objLambda.Save_InsertTemplateWorkflowUserDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Save, "No", Me.Session(S_UserID), estr) Then
                    Me.ObjCommon.ShowAlert("Error While Saving TemplateWorkflowUserDtl", Me.Page)
                    Exit Sub

                End If

                'Save ActivityOperationMxt
                PapulateActOperationMxtData()
                Success = True

            End If

            'For Deleted
            dv_Delete = CType(Me.ViewState(VS_DtTemplateWorkFlowUserDtl), DataTable).DefaultView
            dv_Delete.RowFilter = "(nTemplateWorkflowUserId>=0 and cStatusIndi <> 'N')"
            dt_Delete = dv_Delete.ToTable
            If dt_Delete.Rows.Count > 0 Then

                ds_Delete = New DataSet
                dt_Delete.TableName = "TemplateWorkFlowUserDtl"
                ds_Delete.Tables.Add(dt_Delete)

                If Not objLambda.Save_InsertTemplateWorkflowUserDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_Delete, "No", Me.Session(S_UserID), estr) Then
                    ObjCommon.ShowAlert("Error While Saving TemplateWorkflowUserDtl", Me.Page)
                    Exit Sub
                End If

                Success = True

            End If

            If Success = True Then
                ObjCommon.ShowAlert("Record Saved SuccessFully", Me.Page)
                ResetPage()
                Me.Response.Redirect("frmtreenodeMst.aspx?vTemplateId=" & Me.ViewState(VS_TemplateId) & "&Rights=Yes")
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

        'Changed on 14-May-2009******************************

        Dim dt_Grid As New DataTable
        Dim dt_NewGrid As New DataTable
        Dim dv_Grid As New DataView
        Dim index3 As New Integer
        Dim dt_UserRights As New DataTable

        Dim index As New Integer

        dt_Grid = CType(Me.ViewState(VS_DtGrid), DataTable)
        dt_UserRights = CType(Me.ViewState(VS_DtTemplateWorkFlowUserDtl), DataTable)

        For index = 0 To GV_UserStage_Edit.Rows.Count - 1
            If CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).Enabled = True Then


                For index3 = 0 To dt_UserRights.Rows.Count - 1

                    If dt_UserRights.Rows(index3).Item("iUserId") = GV_UserStage_Edit.Rows(index).Cells(GVC_USE_UserId).Text And _
                        dt_UserRights.Rows(index3).Item("iStageId") = CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).SelectedValue.Trim() And _
                        dt_UserRights.Rows(index3).Item("cStatusIndi") <> "D" Then

                        Me.ObjCommon.ShowAlert("UserId ='" & Me.GV_UserStage_Edit.Rows(index).Cells(GVC_USE_UserName).Text & "(" & Me.GV_UserStage_Edit.Rows(index).Cells(GVC_USE_UserId).Text & ")' And StageId = '" & _
                                                CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).SelectedItem.Text.Trim() & "(" & CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).SelectedValue.Trim() & _
                                                ")' Already Added", Me.Page)

                        Exit Sub

                    End If

                    If dt_UserRights.Rows(index3).Item("iUserId") = GV_UserStage_Edit.Rows(index).Cells(GVC_USE_UserId).Text And _
                        dt_UserRights.Rows(index3).Item("iStageId") = GV_UserStage_Edit.Rows(index).Cells(GVC_USE_StageId).Text Then


                        If CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).Enabled = True Then
                            dt_UserRights.Rows(index3)("iStageId") = CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).SelectedValue.Trim()
                        End If

                        dt_UserRights.Rows(index3)("iModifyBy") = Me.Session(S_UserID)
                        dt_UserRights.Rows(index3)("cStatusIndi") = "E"
                        dt_UserRights.Rows(index3).AcceptChanges()
                        dt_UserRights.AcceptChanges()

                    End If

                    If dt_Grid.Rows(index3).Item("iUserId") = GV_UserStage_Edit.Rows(index).Cells(GVC_USE_UserId).Text And _
                        dt_Grid.Rows(index3).Item("iStageId") = GV_UserStage_Edit.Rows(index).Cells(GVC_USE_StageId).Text Then

                        If CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).Enabled = True Then
                            dt_Grid.Rows(index3)("iStageId") = CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).SelectedValue.Trim()
                            dt_Grid.Rows(index3)("vStageDesc") = CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).SelectedItem.Text.Trim()
                        End If

                        dt_Grid.Rows(index3)("cStatusIndi") = "E"
                        dt_Grid.Rows(index3).AcceptChanges()
                        dt_Grid.AcceptChanges()

                    End If

                    Me.ViewState(VS_DtGrid) = dt_Grid
                    Me.ViewState(VS_DtTemplateWorkFlowUserDtl) = dt_UserRights

                Next index3
            End If

        Next index

        '******************************

        dt_Update = CType(Me.ViewState(VS_DtTemplateWorkFlowUserDtl), DataTable)
        ds_Update = New DataSet
        dt_Update.TableName = "TemplateWorkFlowUserDtl"
        ds_Update.Tables.Add(dt_Update)

        If Not objLambda.Save_InsertTemplateWorkflowUserDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_Update, "No", Me.Session(S_UserID), estr) Then
            ObjCommon.ShowAlert("Error While Saving TemplateWorkflowUserDtl", Me.Page)
            Exit Sub

        End If

        'Save ActivityOperationMxt
        PapulateActOperationMxtData()

        ObjCommon.ShowAlert("Record Saved SuccessFully", Me.Page)
        ResetPage()
        Me.Response.Redirect("frmtreenodeMst.aspx?vTemplateId=" & Me.ViewState(VS_TemplateId) & "&Rights=Yes")

    End Sub

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        ResetPage()
        Me.Response.Redirect("frmtreenodeMst.aspx?vTemplateId=" & Me.ViewState(VS_TemplateId) & "&Rights=No")
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim index As New Integer
        Dim index3 As New Integer
        Dim dt_UserRights As New DataTable
        Dim dt_Grid As New DataTable
        Dim dt_NewGrid As New DataTable
        Dim dv_Grid As New DataView
        Dim chk As New CheckBox

        Dim ds_Delete As New DataSet
        Dim dt_Delete As New DataTable
        Dim dv_Delete As New DataView
        Dim estr As String = ""

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            dt_Grid = CType(Me.ViewState(VS_DtGrid), DataTable)
            dt_UserRights = CType(Me.ViewState(VS_DtTemplateWorkFlowUserDtl), DataTable)

            For index = 0 To Me.GV_UserStage.Rows.Count - 1

                chk = CType(Me.GV_UserStage.Rows(index).FindControl("CHKDelete"), CheckBox)
                If chk.Checked = True Then

                    For index3 = dt_UserRights.Rows.Count - 1 To 0 Step -1

                        If dt_UserRights.Rows(index3).Item("iUserId") = GV_UserStage.Rows(index).Cells(GVC_US_UserId).Text And dt_UserRights.Rows(index3).Item("iStageId") = GV_UserStage.Rows(index).Cells(GVC_US_StageId).Text Then

                            If dt_UserRights.Rows(index3)("nTemplateWorkflowUserId") < 0 Then

                                dt_UserRights.Rows(index3).Delete()

                            Else

                                dt_UserRights.Rows(index3)("iModifyBy") = Me.Session(S_UserID)
                                dt_UserRights.Rows(index3)("cStatusIndi") = "D"
                                dt_UserRights.Rows(index3).AcceptChanges()

                            End If

                            dt_UserRights.AcceptChanges()

                        End If

                    Next index3

                    For index3 = dt_Grid.Rows.Count - 1 To 0 Step -1

                        If dt_Grid.Rows(index3).Item("iUserId") = GV_UserStage.Rows(index).Cells(GVC_US_UserId).Text And dt_Grid.Rows(index3).Item("iStageId") = GV_UserStage.Rows(index).Cells(GVC_US_StageId).Text Then

                            If dt_Grid.Rows(index3)("nTemplateWorkflowUserId") < 0 Then

                                dt_Grid.Rows(index3).Delete()

                            Else

                                dt_Grid.Rows(index3)("cStatusIndi") = "D"
                                dt_Grid.Rows(index3).AcceptChanges()

                            End If

                            dt_Grid.AcceptChanges()

                        End If

                    Next index3

                End If

            Next index

            Me.ViewState(VS_DtTemplateWorkFlowUserDtl) = dt_UserRights
            Me.ViewState(VS_DtGrid) = dt_Grid

            dv_Grid = CType(Me.ViewState(VS_DtGrid), DataTable).DefaultView
            dv_Grid.RowFilter = "cStatusIndi <> 'D'"
            dt_NewGrid = dv_Grid.ToTable
            Me.GV_UserStage.DataSource = dt_NewGrid
            Me.GV_UserStage.DataBind()

            Me.GV_UserStage_Edit.DataSource = dt_NewGrid
            Me.GV_UserStage_Edit.DataBind()

            'For Deleted
            dv_Delete = CType(Me.ViewState(VS_DtTemplateWorkFlowUserDtl), DataTable).DefaultView
            dv_Delete.RowFilter = "(nTemplateWorkflowUserId>=0 and cStatusIndi <> 'N')"
            dt_Delete = dv_Delete.ToTable

            If dt_Delete.Rows.Count > 0 Then

                ds_Delete = New DataSet
                dt_Delete.TableName = "TemplateWorkFlowUserDtl"
                ds_Delete.Tables.Add(dt_Delete)

                If Not objLambda.Save_InsertTemplateWorkflowUserDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_Delete, "No", Me.Session(S_UserID), estr) Then
                    ObjCommon.ShowAlert("Error While Saving TemplateWorkflowUserDtl", Me.Page)
                    Exit Sub
                End If
                ObjCommon.ShowAlert("Record Deleted SuccessFully", Me.Page)

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

#End Region

#Region "Grid Event"

    Protected Sub GV_UserStage_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
    End Sub

    Protected Sub GV_UserStage_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
    End Sub

    Protected Sub GV_UserStage_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)

    End Sub

    Protected Sub GV_UserStage_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_UserStage.RowCreated

        e.Row.Cells(GVC_US_TemplateId).Visible = False
        e.Row.Cells(GVC_US_NodeId).Visible = False
        e.Row.Cells(GVC_US_UserId).Visible = False
        e.Row.Cells(GVC_US_StageId).Visible = False

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
        Dim index3 As New Integer

        If e.CommandName.ToUpper = "EDIT" Then

            Me.BtnUpdate.Enabled = True
            CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).Enabled = True
            CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).DataSource = CType(Me.ViewState(VS_DtStage), DataTable)
            CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).DataValueField = "istageid"
            CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).DataTextField = "vStageDesc"
            CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).DataBind()

        End If

    End Sub

    Protected Sub GV_UserStage_Edit_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs)
        e.Cancel = True
    End Sub

    Protected Sub GV_UserStage_Edit_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs)

    End Sub

    Protected Sub GV_UserStage_Edit_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or _
                e.Row.RowType = DataControlRowType.Header Then

            e.Row.Cells(GVC_USE_TemplateId).Visible = False
            e.Row.Cells(GVC_USE_NodeId).Visible = False
            e.Row.Cells(GVC_USE_UserId).Visible = False
            e.Row.Cells(GVC_USE_StageId).Visible = False

        End If
    End Sub

    Protected Sub GV_UserStage_Edit_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        GV_UserStage.PageIndex = e.NewPageIndex
        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub

#End Region

#Region "RadioButton Event"

    Protected Sub RbEdit_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbEdit.CheckedChanged
        If Me.RbEdit.Checked = True Then
            Me.DivAdd.Visible = False
            Me.DivEdit.Visible = True
            'Me.DivDelete.Visible = False

            'Me.GV_UserStage_Edit.DataSource = Me.ViewState(VS_DtGrid)
            'Me.GV_UserStage_Edit.DataBind()

        ElseIf Me.RbDelete.Checked = True Then
            Me.DivAdd.Visible = False
            Me.DivEdit.Visible = False
            'Me.DivDelete.Visible = True
            'FillDivEmp()
        ElseIf Me.RbAdd.Checked = True Then
            Me.DivAdd.Visible = True
            Me.DivEdit.Visible = False
            'Me.DivDelete.Visible = False
        End If
    End Sub

    Protected Sub RbDelete_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbDelete.CheckedChanged
        If Me.RbEdit.Checked = True Then
            Me.DivAdd.Visible = False
            Me.DivEdit.Visible = True
            'Me.DivDelete.Visible = False
        ElseIf Me.RbDelete.Checked = True Then
            Me.DivAdd.Visible = False
            Me.DivEdit.Visible = False
            'Me.DivDelete.Visible = True
            'FillDivEmp()
        ElseIf Me.RbAdd.Checked = True Then
            Me.DivAdd.Visible = True
            Me.DivEdit.Visible = False
            'Me.DivDelete.Visible = False
        End If

    End Sub

    Protected Sub RbAdd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbAdd.CheckedChanged
        If Me.RbEdit.Checked = True Then
            Me.DivAdd.Visible = False
            Me.DivEdit.Visible = True
            'Me.DivDelete.Visible = False
        ElseIf Me.RbDelete.Checked = True Then
            Me.DivAdd.Visible = False
            Me.DivEdit.Visible = False
            'Me.DivDelete.Visible = True
            'FillDivEmp()
        ElseIf Me.RbAdd.Checked = True Then
            Me.DivAdd.Visible = True
            Me.DivEdit.Visible = False
            'Me.DivDelete.Visible = False
        End If
    End Sub

#End Region

#Region "ResetPage"
    Private Sub ResetPage()
        Me.ViewState(VS_DtTemplateWorkFlowUserDtl) = Nothing
        Me.DDLUserGroup.SelectedIndex = 0
        Me.chklstUser.Items.Clear()
        Me.chklstStages.Items.Clear()
        Me.GV_UserStage.DataSource = Nothing
        Me.GV_UserStage.DataBind()
        Me.GV_UserStage_Edit.DataSource = Nothing
        Me.GV_UserStage_Edit.DataBind()
        Me.btnSave.Enabled = False
        Me.BtnUpdate.Enabled = True
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
