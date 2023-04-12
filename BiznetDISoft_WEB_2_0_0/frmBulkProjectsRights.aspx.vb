
Imports System.Web.Services
Imports Newtonsoft.Json

Partial Class frmBulkProjectsRights
    Inherits System.Web.UI.Page


#Region "VARIABLE DECLARATIONS"

    Private objCommon As New clsCommon
    Private objHelpDBTable As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

    Private Const VS_ProjectListProjectNo As String = "ProjectListProjectNo"
    Private Const VS_ProjectListRequestId As String = "ProjectListRequestId"
    Private ds_ProjectNo As New DataSet
    Private ds_UserList As New DataSet
    Private eStr_Retu As String = String.Empty
    Dim ds_ProjectList As New DataSet
    Dim ds_UserLst As New DataSet
    Dim wstr_location As String = String.Empty
    Dim dataRetriveMode As WS_HelpDbTable.DataRetrievalModeEnum

#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Page.Title = " :: Bulk Project Rights ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
        CType(Me.Master.FindControl("lblHeading"), Label).Style.Add("display", "block")
        'CType(Me.Master.FindControl("lblHeading"), Label).Text = "Set Projects User Rights"
        CType(Me.Master.FindControl("lblHeading"), Label).Text = "Bulk Project Rights"

        If Not Page.IsPostBack Then
            BindProjectNo("ProjectNo")
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " + Me.Session(S_UserID)
        End If
    End Sub

#End Region

#Region "BindProject No"

    Protected Sub BindProjectNo(ByVal Selection As String)
        Dim Wstr As String = String.Empty
        Dim ds_ProjectNo As New DataSet
        Dim estr As String = String.Empty
        Dim Index As Integer
        Dim Dv_DeptList As DataView
        Dim Dv_UserList As DataView
        Dim ds_UserList As New DataSet
        Dim Ds_RequestId As New DataSet
        ChkBoxLstProjectNo.Items.Clear()

        TV_UserLst.Nodes.Clear()

        'If Session(S_UserID) = SuperUserId Then
        'If Session(S_UserType) = SuperUserType Then
        '    wstr_location = ""
        '    'dataRetriveMode = WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords
        'Else
        '    wstr_location = "vLocationCode='" + Me.Session(S_LocationCode) + "' AND "
        '    'dataRetriveMode = WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition
        'End If

        If Selection = "ProjectNo" Then
            Wstr = "cStatusIndi <> 'D'order by vProjectNo "
            If Not objHelpDBTable.GetViewgetWorkspaceDetailForHdr(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ProjectNo, estr) Then
                Me.objCommon.ShowAlert("Error while getting data from workspaceprotocoldetail", Me.Page)
                Exit Sub
            End If

            For Index = 0 To ds_ProjectNo.Tables(0).Rows.Count - 1 Step 1
                If Convert.ToString(ds_ProjectNo.Tables(0).Rows(Index).Item("vprojectNo")).Trim() <> "" Then
                    ChkBoxLstProjectNo.Items.Add(New ListItem(Convert.ToString(ds_ProjectNo.Tables(0).Rows(Index).Item("vProjectNo")).Trim(), Convert.ToString(ds_ProjectNo.Tables(0).Rows(Index).Item("vWorkSpaceId")).Trim()))
                End If
            Next
            LblUserProfile.Text = "UserName (Profile)"
            LblUserProfile.Visible = True
            LblProjectNo.Text = "Project No / Request Id"
            LblProjectNo.Visible = True
            PnlProjectNo.Visible = True
            ChkBoxLstProjectNo.Visible = True
            Rdbtnlst.Items(1).Selected = True

            Wstr = "cStatusIndi <> 'D' order by vDeptName"
            If Not objHelpDBTable.GetViewUserMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_UserList, estr) Then
                Me.objCommon.ShowAlert("Error while getting data from UserMst", Me.Page)
                Exit Sub
            End If
        ElseIf Selection = "RequestId" Then
            ChkBoxLstProjectNo.Items.Clear()
            Wstr = "cStatusIndi <> 'D'order by vProjectNo "
            If Not objHelpDBTable.GetViewgetWorkspaceDetailForHdr(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ProjectNo, estr) Then
                Me.objCommon.ShowAlert("Error while getting data from workspaceprotocoldetail", Me.Page)
                Exit Sub
            End If

            For Index = 0 To ds_ProjectNo.Tables(0).Rows.Count - 1 Step 1
                If Convert.ToString(ds_ProjectNo.Tables(0).Rows(Index).Item("vprojectNo")).Trim() = "" Then
                    If ds_ProjectNo.Tables(0).Rows(Index).Item("vRequestId").ToString().Trim() = "0000000000" Then
                    Else
                        ChkBoxLstProjectNo.Items.Add(New ListItem(Convert.ToString(ds_ProjectNo.Tables(0).Rows(Index).Item("vRequestId")).Trim(), Convert.ToString(ds_ProjectNo.Tables(0).Rows(Index).Item("vWorkSpaceId")).Trim()))
                    End If

                End If

            Next
            LblUserProfile.Text = "UserName (Profile)"
            LblUserProfile.Visible = True
            LblProjectNo.Text = "Project No / Request Id"
            LblProjectNo.Visible = True
            PnlProjectNo.Visible = True
            ChkBoxLstProjectNo.Visible = True
            Rdbtnlst.Items(0).Selected = True

            Wstr = "cStatusIndi<>'D' order by vDeptName"
            If Not objHelpDBTable.GetViewUserMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_UserList, estr) Then
                Me.objCommon.ShowAlert("Error while getting data from UserMst", Me.Page)
                Exit Sub
            End If

        End If

        Dv_DeptList = ds_UserList.Tables(0).DefaultView.ToTable(True, "vDeptCode,vDeptName".Split(",")).DefaultView
        'Dv_UserList = ds_UserList.Tables(0).DefaultView.ToTable(True).DefaultView
        Dv_UserList = ds_UserList.Tables(0).DefaultView.ToTable(True, "iUserId,vUserName,vUserTypeName,vDeptCode,vDeptName".Split(",")).DefaultView






        PnlProjectNo.Visible = True
        ChkBoxLstProjectNo.Visible = True
        PnlUserLst.Visible = True
        TV_UserLst.Visible = True
        BtnSetRights.Visible = True
        AddParentNode(Dv_DeptList.ToTable())
        AddChildNode(Dv_UserList.ToTable())

        'AddChildNode(ds_UserList.Tables(0).AsEnumerable().Distinct(System.Data.DataRowComparer.Default).ToList().CopyToDataTable())

        '
        'If Not objHelpDBTable.GetViewgetWorkspaceDetailForHdr(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ProjectNo, estr) Then
        '    Me.objCommon.ShowAlert("Error while getting data from workspaceprotocoldetail", Me.Page)
        '    Exit Sub
        'End If
        ''ViewState("VS_ProjectList") = ds_ProjectNo.Tables(0)
        'countForProjectNo = ds_ProjectNo.Tables(0).Rows.Count - 1
        'For Index = 0 To countForProjectNo
        '    If Convert.ToString(ds_ProjectNo.Tables(0).Rows(Index).Item("vprojectNo")).Trim() = "" Then
        '        'ChkBoxLstProjectNo.Items.Insert(Index, Convert.ToString(ds_ProjectNo.Tables(0).Rows(Index).Item("vRequestId")).Trim())
        '        ChkBoxLstProjectNo.Items.Add(New ListItem(Convert.ToString(ds_ProjectNo.Tables(0).Rows(Index).Item("vRequestId")).Trim(), Convert.ToString(ds_ProjectNo.Tables(0).Rows(Index).Item("vWorkSpaceId")).Trim()))
        '    Else
        '        'ChkBoxLstProjectNo.Items.Insert(Index, Convert.ToString(ds_ProjectNo.Tables(0).Rows(Index).Item("vProjectNo")).Trim())
        '        ChkBoxLstProjectNo.Items.Add(New ListItem(Convert.ToString(ds_ProjectNo.Tables(0).Rows(Index).Item("vProjectNo")).Trim(), Convert.ToString(ds_ProjectNo.Tables(0).Rows(Index).Item("vWorkSpaceId")).Trim()))
        '    End If
        'Next
    End Sub

#End Region

#Region "Button Events"

    Protected Sub BtnSetRights_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSetRights.Click
        Dim ds_WorkspaceWorkFlowUserDtl As New DataSet
        Dim Wstr As String = "1=2"
        Dim SelectedProjectNo As String = String.Empty
        Dim estr As String = String.Empty
        Dim IndexForProjectNo As Integer
        Dim IndexForUserNo As Integer
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim IndexForWorkFlowUserId As Integer = 0
        Try
            If Not objHelpDBTable.GetWorkspaceDefaultWorkFlowUserDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_WorkspaceWorkFlowUserDtl, estr) Then
                Exit Sub
            End If

            For IndexForProjectNo = 0 To ChkBoxLstProjectNo.Items.Count - 1
                If ChkBoxLstProjectNo.Items.Item(IndexForProjectNo).Selected = True Then
                    For IndexForUserNo = 0 To TV_UserLst.Nodes.Count - 1 Step 1
                        For Each ChildNodes As TreeNode In TV_UserLst.Nodes(IndexForUserNo).ChildNodes
                            If ChildNodes.Checked Then
                                dr = ds_WorkspaceWorkFlowUserDtl.Tables(0).NewRow
                                dr.Item("nWorkspaceDefaultWorkflowUserId") = IndexForWorkFlowUserId
                                dr.Item("vWorkSpaceId") = Convert.ToString(ChkBoxLstProjectNo.Items(IndexForProjectNo).Value).Trim()
                                dr.Item("iUserId") = ChildNodes.Value.Trim()
                                dr.Item("iStageId") = 12
                                dr.Item("iModifyBy") = CInt(Me.Session(S_UserID))
                                dr.Item("cStatusIndi") = "N"
                                ds_WorkspaceWorkFlowUserDtl.Tables(0).Rows.Add(dr)
                                ds_WorkspaceWorkFlowUserDtl.AcceptChanges()
                                IndexForWorkFlowUserId += 1
                            End If
                        Next ChildNodes
                    Next IndexForUserNo
                End If
            Next IndexForProjectNo

            ds_WorkspaceWorkFlowUserDtl.Tables(0).TableName = "VIEW_WORKSPACEDEFAULTWORKFLOWUSERDTL"
            ds_WorkspaceWorkFlowUserDtl.AcceptChanges()

            If Not objLambda.Save_WorkspaceDefaultWorkFlowUserDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_WorkspaceWorkFlowUserDtl, Me.Session(S_UserID), estr) Then
                Throw New Exception(estr)
            End If

            Me.objCommon.ShowAlert("Record Saved Successfully", Me.Page)
            BindProjectNo("ProjectNo")
            txtproject.Text = ""

        Catch ex As Exception
        End Try
    End Sub

    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGo.Click
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim Ds_ProjectNo As New DataSet
        Dim Ds_RequestId As New DataSet
        Dim ds_UserList As New DataSet
        Dim Dv_DeptList As DataView
        Dim Dv_UserList As DataView

        Dim Index As Integer
        TV_UserLst.Nodes.Clear()
        Wstr = "cStatusIndi<>'D' order by vDeptName"
        If Not objHelpDBTable.GetViewUserMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_UserList, estr) Then
            Me.objCommon.ShowAlert("Error while getting data from UserMst", Me.Page)
            Exit Sub
        End If

        Dv_DeptList = ds_UserList.Tables(0).DefaultView.ToTable(True, "vDeptCode,vDeptName".Split(",")).DefaultView
        Dv_UserList = ds_UserList.Tables(0).DefaultView.ToTable(True, "iUserId,vUserName,vUserTypeName,vDeptCode,vDeptName".Split(",")).DefaultView

        PnlUserLst.Visible = True
        TV_UserLst.Visible = True

        AddParentNode(Dv_DeptList.ToTable())
        AddChildNode(Dv_UserList.ToTable())
        'AddChildNode(ds_UserList.Tables(0))


        If ChkBoxAll.Checked = True Then
            If Rdbtnlst.Items(0).Selected = True Then
                BindProjectNo("RequestId")
            Else
                BindProjectNo("ProjectNo")
            End If

        ElseIf Rdbtnlst.SelectedValue.ToString().Trim() = "requestid" Then

            ChkBoxLstProjectNo.Items.Clear()

            Wstr = "cStatusIndi<>'D' And dCreatedOn between '" & TxtFromDate.Text & "' And '" & TxtToDate.Text & "'order by vRequestId"
            If Not objHelpDBTable.GetViewgetWorkspaceDetailForHdr(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_RequestId, estr) Then
                Me.objCommon.ShowAlert("Error While Getting Data From Workspaceprotocoldetail", Me.Page)
                Exit Sub
            End If
            ViewState("VS_ProjectListRequestId") = Ds_RequestId.Tables(0)

            For Index = 0 To Ds_RequestId.Tables(0).Rows.Count - 1 Step 1
                If Convert.ToString(Ds_RequestId.Tables(0).Rows(Index).Item("vprojectNo")).Trim() = "" Then
                    If Ds_RequestId.Tables(0).Rows(Index).Item("vRequestId").ToString().Trim() = "" Then
                    Else
                        ChkBoxLstProjectNo.Items.Add(New ListItem(Convert.ToString(Ds_RequestId.Tables(0).Rows(Index).Item("vRequestId")).Trim(), Convert.ToString(Ds_RequestId.Tables(0).Rows(Index).Item("vWorkSpaceId")).Trim()))
                    End If
                End If
            Next
            LblUserProfile.Text = "UserName (Profile)"
            LblUserProfile.Visible = True
            LblProjectNo.Text = "Project No / Request Id"
            LblProjectNo.Visible = True
            PnlProjectNo.Visible = True
            ChkBoxLstProjectNo.Visible = True
            Rdbtnlst.Items(0).Selected = True

        ElseIf Rdbtnlst.SelectedValue.ToString().Trim() = "projectno" Then
            ChkBoxLstProjectNo.Items.Clear()

            Wstr = "cStatusIndi<>'D' And dCreatedOn between '" & TxtFromDate.Text & "' And '" & TxtToDate.Text & "'order by vProjectNo"
            If Not objHelpDBTable.GetViewgetWorkspaceDetailForHdr(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_ProjectNo, estr) Then
                Me.objCommon.ShowAlert("Error While Getting Data From Workspaceprotocoldetail", Me.Page)
                Exit Sub
            End If


            For Index = 0 To Ds_ProjectNo.Tables(0).Rows.Count - 1 Step 1
                If Convert.ToString(Ds_ProjectNo.Tables(0).Rows(Index).Item("vprojectNo")).Trim() <> "" Then
                    ChkBoxLstProjectNo.Items.Add(New ListItem(Convert.ToString(Ds_ProjectNo.Tables(0).Rows(Index).Item("vProjectNo")).Trim(), Convert.ToString(Ds_ProjectNo.Tables(0).Rows(Index).Item("vWorkSpaceId")).Trim()))
                End If
            Next

            LblUserProfile.Text = "UserName (Profile)"
            LblUserProfile.Visible = True
            LblProjectNo.Text = "Project No / Request Id"
            LblProjectNo.Visible = True
            PnlProjectNo.Visible = True
            ChkBoxLstProjectNo.Visible = True
        Else
            Me.objCommon.ShowAlert("Select Any Option From RequestId Or ProjectNo", Me.Page)
        End If
    End Sub

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        TxtFromDate.Text = String.Empty
        TxtToDate.Text = String.Empty
        txtproject.Text = ""
        BindProjectNo("ProjectNo")
    End Sub

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim Index As Integer
        Dim Ds_RequestId As New DataSet
        ChkBoxLstProjectNo.Items.Clear()
        Wstr = "vWorkSpaceId='" & HProjectId.Value.ToString().Trim() & "' And cStatusIndi<>'D'"
        If Not objHelpDBTable.GetViewgetWorkspaceDetailForHdr(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_RequestId, estr) Then
            Me.objCommon.ShowAlert("Error While Getting Data From Workspaceprotocoldetail", Me.Page)
            Exit Sub
        End If

        For Index = 0 To Ds_RequestId.Tables(0).Rows.Count - 1 Step 1
            If Convert.ToString(Ds_RequestId.Tables(0).Rows(Index).Item("vprojectNo")).Trim() <> "" Then
                ChkBoxLstProjectNo.Items.Add(New ListItem(Convert.ToString(Ds_RequestId.Tables(0).Rows(Index).Item("vProjectNo")).Trim(), Convert.ToString(Ds_RequestId.Tables(0).Rows(Index).Item("vWorkSpaceId")).Trim()))
            Else
                ChkBoxLstProjectNo.Items.Add(New ListItem(Convert.ToString(Ds_RequestId.Tables(0).Rows(Index).Item("vRequestId")).Trim(), Convert.ToString(Ds_RequestId.Tables(0).Rows(Index).Item("vWorkSpaceId")).Trim()))
            End If
        Next
    End Sub
#End Region

#Region "Functions"

    Private Sub AddParentNode(ByVal dt As DataTable)
        Dim PNode As TreeNode
        Dim dr As DataRow

        For Each dr In dt.Rows

            PNode = New TreeNode
            PNode.Text = Convert.ToString(dr("vDeptName")).Trim()
            PNode.Value = Convert.ToString(dr("vDeptCode")).Trim()
            PNode.NavigateUrl = "javascript:void(0);"
            Me.TV_UserLst.Nodes.Add(PNode)

        Next

    End Sub

    Private Sub AddChildNode(ByVal dt As DataTable)
        Dim PNode As TreeNode
        Dim CNode As TreeNode
        Dim dv As New DataView
        Dim dr As DataRow
        Dim Index As Integer = 0

        For Index = 0 To Me.TV_UserLst.Nodes.Count - 1

            dt.DefaultView.Sort = "vUserName"
            dv = dt.DefaultView
            dv.RowFilter = "vDeptCode = '" + Me.TV_UserLst.Nodes(Index).Value.Trim() + "'"

            For Each dr In dv.ToTable().Rows

                PNode = Me.TV_UserLst.Nodes(Index)
                CNode = New TreeNode()

                CNode.Text = Convert.ToString(dr("vUserName")).Trim() + " ( " + Convert.ToString(dr("vUserTypeName")).Trim() + " )"
                CNode.Value = Convert.ToString(dr("iUserId")).Trim()
                CNode.NavigateUrl = "javascript:void(0);"

                Me.TV_UserLst.Nodes.Add(CNode)
                PNode.ChildNodes.Add(CNode)


            Next

        Next


    End Sub

#End Region

#Region "Selected index change"

    Protected Sub Rdbtnlst_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Rdbtnlst.Items(0).Selected = True Then
            BindProjectNo("RequestId")
        Else
            BindProjectNo("ProjectNo")
        End If
    End Sub

#End Region

    <WebMethod>
    Public Shared Function AuditTrail(ByVal vWorkSpaceId As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim estr As String = String.Empty
        Dim returnData As String = String.Empty
        Dim ds_Audit As New DataSet
        Dim iMode As String = "2"
        Try

            wStr = " vWorkSpaceId = '" + vWorkSpaceId + " ' AND cStatusIndi <> 'D'"
            If Not objHelp.View_WorkspaceDefaultWorkflowUserDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                    ds_Audit, estr) Then
                Throw New Exception(estr)
            End If
            returnData = JsonConvert.SerializeObject(ds_Audit)
            Return returnData

        Catch ex As Exception
            Return ex.ToString()
        Finally
        End Try

    End Function
End Class
