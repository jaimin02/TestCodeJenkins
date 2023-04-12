
Partial Class frmEditWorkspaceNodeDetail
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
    Private eStr_Retu As String

    Private dt As DataTable = Nothing
    Private Const VS_DtWSSub As String = "DtWSSub"

    Private ActivityState As String = "ADD"

#End Region

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            GenCall()
        Else
            CreateWorkspaceNodeTable()
        End If

        Exit Sub
    End Sub
#End Region

#Region "GenCall"
    Private Function GenCall() As Boolean
        Dim dt_WorkspaceNodeDetail As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim wStr As String = "1=2"
        Dim ds_WorkspacenodeDetail As New Data.DataSet
        Try

            Choice = CType("1", WS_Lambda.DataObjOpenSaveModeEnum)

            Me.ViewState("Choice") = Choice   'To use it while saving

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState("vActivityId") = Me.Request.QueryString("Value").ToString

            End If

            ''Check for Valid User''
            If Not GenCall_Data(Choice, dt_WorkspaceNodeDetail) Then ' For Data Retrieval
                Exit Function
            End If

            wStr = " vWorkspaceid = '" + Me.HProjectId.Value + "'"

            If Not objHelp.getWorkSpaceNodeDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_WorkspacenodeDetail, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            Me.ViewState("dtWorkspaceNodeDetail") = dt_WorkspaceNodeDetail ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI(Choice, dt_WorkspaceNodeDetail) Then 'For Displaying Data 
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...GenCall")
        Finally
        End Try
    End Function
#End Region

#Region "GenCall_Data"
    Private Function GenCall_Data(ByVal Choice As WS_Lambda.DataObjOpenSaveModeEnum, _
                            ByRef dt_Dist_Retu As DataTable) As Boolean

        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds As DataSet = Nothing
        Dim i As Integer

        Try

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vActivityId=" + Me.ViewState("vActivityId").ToString() 'Value of where condition
                wStr = "vWorkspaceId='" & Me.TVWorkspace.CheckedNodes(i).Value & "' and iNodeId=" & Me.TVWorkspace.CheckedNodes(i).ImageToolTip
            End If

            If Not objHelp.getWorkSpaceNodeDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Throw New Exception(eStr)
                Exit Function
            End If

            If ds Is Nothing Then
                Throw New Exception(eStr)
            End If

            If ds.Tables(0).Rows.Count <= 0 And _
               Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Throw New Exception("No Records Found For Selected Role")
            End If

            dt_Dist_Retu = ds.Tables(0)
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_Data")
        Finally
        End Try
    End Function
#End Region

#Region "GenCall_ShowUI"
    Private Function GenCall_ShowUI(ByVal Choice As WS_Lambda.DataObjOpenSaveModeEnum, _
                                      ByVal dt_Dist As DataTable) As Boolean
        Dim dt_treenodedetail As DataTable = Nothing
        Dim Sender As Object
        Dim e As EventArgs

        CType(Master.FindControl("lblHeading"), Label).Text = "Project Structure Management"

        Page.Title = " ::  Project Structure Management  ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
        '==================
        'Me.btnAddLast.OnClientClick = "return Validation(this)"
        'Me.btnAddBefore.OnClientClick = "return Validation(this)"
        'Me.btnAddAfter.OnClientClick = "return Validation(this)"
        '===========================
        If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

            Me.ddlAddActivity.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_treenodedetail.Rows(0)("vActivityId"), dt_treenodedetail.Rows(0)("vActivityId").GetType)
            Me.ddlAct.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_treenodedetail.Rows(0)("vActivityId"), dt_treenodedetail.Rows(0)("vActivityId").GetType)

        End If

        Me.BtnBack.Visible = False
        'Added on 10-Jul-2009
        If Not IsNothing(Me.Request.QueryString("WorkspaceId")) AndAlso Me.Request.QueryString("WorkspaceId") <> "" Then
            tblProjectName.Attributes.Add("style", "display:none")
            If Not IsNothing(Me.Request.QueryString("WorkspaceName")) AndAlso Me.Request.QueryString("WorkspaceName") <> "" Then
            End If
            Me.HProjectId.Value = Me.Request.QueryString("WorkspaceId")
            BindTree()
            Me.BtnBack.Visible = True
        End If
        '**************************************

        If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
            Me.txtproject.Text = Session(S_ProjectName)
            Me.HProjectId.Value = Session(S_ProjectId)
            btnSetProject_Click(sender, e)
            GenCall_ShowUI = True
            Exit Function
        End If



        '==added on 10-sep-2011 by deepak singh to show project according to user
        'Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)

        ''== Added by Arpit on 14/Nov/2017 To Show Only Parent Project
        Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID) & " And cWorkspaceType = 'P'"
        '========


        If Not FillActivityGroup() Then
            Return False
        End If

        Return True
    End Function
#End Region

#Region "Bindtree"

    Private Sub BindTree()

        Dim Ds_WorkSpaceNode As New DataSet
        Dim dsVacantPosition As New DataSet
        Dim dsWorkspace As New DataSet
        Dim dt_Workspace As New DataTable
        Dim CurrentNode As New TreeNode
        Dim WorkspaceId As String = String.Empty
        Dim ParentNode As New TreeNode
        Dim bln As Boolean = False
        Dim cnt As Integer = 0
        Dim eStr As String = String.Empty

        Try



            Me.TVWorkspace.Nodes.Clear()
            WorkspaceId = Me.HProjectId.Value.Trim()
            Ds_WorkSpaceNode = Me.objHelp.GetProc_TreeViewOfNodes(WorkspaceId)

            dt = Ds_WorkSpaceNode.Tables(0)
            Me.ViewState("TreeNodes") = dt
            TestLast("0", New TreeNode())

            'TVWorkspace.ExpandAll()
            'TVWorkspace.CollapseAll()


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....BindTree")
        End Try
    End Sub

    Private Sub TestLast(ByVal parentId As String, ByVal tn As TreeNode)
        Dim dr() As DataRow = dt.Select("iParentNodeId=" & parentId)
        Dim parentNode As New TreeNode

        Try

            For index As Integer = 0 To dr.Length - 1

                parentNode = New TreeNode

                'parentNode.Text = Replace(dr(index)("vNodeDisplayName").ToString, "*", "") & "(" & dr(index)("vNodeName") & ")" '& "(" & dr(index)("iNodeId") & ")" & "(" & dr(index)("iNodeId") & ")"
                parentNode.Text = "<a href=""frmPreviewattributesForm.aspx?WorkspaceId=" & dr(index)("vworkspaceid").ToString.Trim() & "&ActivityId=" & dr(index)("vActivityId").ToString.Trim() & "&iNodeId=" & dr(index)("iNodeId") & """ target='_blank' title = " & dr(index)("vActivityId") & " >" & Replace(dr(index)("vNodeDisplayName").ToString, "*", "") & "(" & dr(index)("vNodeName") & ")" & "</a>"
                parentNode.Value = Replace(dr(index)("vNodeDisplayName").ToString, "*", "")
                parentNode.ImageToolTip = dr(index)("iNodeId")
                parentNode.ToolTip = dr(index)("vActivityId").ToString.Trim()
                'parentNode.NavigateUrl = "javascript:ShowTemplate('frmPreviewattributesForm.aspx?WorkspaceId=" & dr(index)("vworkspaceid").ToString.Trim() & "&ActivityId=" & dr(index)("vActivityId").ToString.Trim() & "&iNodeId=" & dr(index)("iNodeId") & "');"

                If parentId = "0" Then
                    TVWorkspace.Nodes.Add(parentNode)
                    'Added by Parth Modi on dated 12-Dec-2014 - Parent Node Expand
                    parentNode.Expand()
                    'END
                    parentNode.ShowCheckBox = False
                Else
                    tn.ChildNodes.Add(parentNode)
                    'Added by Parth Modi on dated 12-Dec-2014 - Child Node Collpase
                    parentNode.Collapse()
                    'END
                End If

                TestLast(dr(index)("iNodeId"), parentNode)

            Next index

            Me.TVWorkspace.Attributes.Add("OnClick", "return NodeChecked('" + TVWorkspace.ClientID + "');")

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....TestLast")
        End Try
    End Sub

#End Region

#Region "Fill ActivityGroup"

    Private Function FillActivityGroup() As Boolean
        Dim estr As String = String.Empty
        Dim Wstr_Scope As String = String.Empty
        Dim ds_Group As New Data.DataSet
        Dim dv_Activity As New DataView
        Try


            'To Get Where condition of ScopeVales( Project Type )
            If Not objCommon.GetScopeValueWithCondition(Wstr_Scope) Then
                FillActivityGroup = False
                Exit Function
            End If
            Wstr_Scope += " And cStatusIndi <> 'D'"
            If Not objHelp.GetviewActivityGroupMst(Wstr_Scope, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Group, estr) Then

                Me.objCommon.ShowAlert(estr, Me.Page())
                FillActivityGroup = False
                Exit Function

            End If

            dv_Activity = ds_Group.Tables(0).DefaultView
            dv_Activity.Sort = "vActivityGroupName"
            Me.ddlActivityGroup2.DataSource = dv_Activity.ToTable()
            Me.ddlActivityGroup2.DataTextField = "vActivityGroupName"
            Me.ddlActivityGroup2.DataValueField = "vActivityGroupId"
            Me.ddlActivityGroup2.DataBind()
            Me.ddlActivityGroup2.Items.Insert(0, New ListItem("--Select Activity Group--", "0"))

            Me.ddlActivityGroup.DataSource = dv_Activity.ToTable()
            Me.ddlActivityGroup.DataTextField = "vActivityGroupName"
            Me.ddlActivityGroup.DataValueField = "vActivityGroupId"
            Me.ddlActivityGroup.DataBind()
            Me.ddlActivityGroup.Items.Insert(0, New ListItem("--Select Activity Group--", "0"))
            FillActivityGroup = True

        Catch ex As Exception
            FillActivityGroup = False
            Me.ShowErrorMessage(ex.Message, estr)
        End Try
    End Function

    Protected Sub ddlActivityGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        FillActivity("DDLACTIVITYGROUP")
        mpeDialogAddActivity.Show()
    End Sub

    Protected Sub ddlActivityGroup2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlActivityGroup2.SelectedIndexChanged

        FillActivity("DDLACTIVITYGROUP2")
        mpeDialogEditActivity.Show()
    End Sub

#End Region

#Region "Fill Activity"

    Private Function FillActivity(ByVal FromDropdown As String) As Boolean
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim ds_type As New Data.DataSet
        Dim dv_Activity As New DataView
        Try



            If FromDropdown.ToUpper = "DDLACTIVITYGROUP2" Then

                wstr = "vActivityGroupId='" & Me.ddlActivityGroup2.SelectedValue.Trim() & "'"

            ElseIf FromDropdown.ToUpper = "DDLACTIVITYGROUP" Then

                wstr = "vActivityGroupId='" & Me.ddlActivityGroup.SelectedValue.Trim() & "'"

            End If

            wstr += " And cStatusIndi <> 'D'"
            If Not objHelp.getActivityMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_type, estr) Then

                Me.objCommon.ShowAlert(estr, Me.Page())
                Exit Function

            End If

            dv_Activity = ds_type.Tables(0).DefaultView
            dv_Activity.Sort = "vActivityName"

            If FromDropdown.ToUpper = "DDLACTIVITYGROUP" Then

                Me.ddlAddActivity.DataSource = dv_Activity.ToTable()
                Me.ddlAddActivity.DataValueField = "vActivityId"
                Me.ddlAddActivity.DataTextField = "vActivityName"
                Me.ddlAddActivity.DataBind()
                Me.ddlAddActivity.Items.Insert(0, New ListItem(" Select Activity", "0"))
                ' tooltip
                For iMedexGroup As Integer = 0 To ddlAddActivity.Items.Count - 1
                    ddlAddActivity.Items(iMedexGroup).Attributes.Add("title", ddlAddActivity.Items(iMedexGroup).Text)
                Next iMedexGroup
                '=========

            ElseIf FromDropdown.ToUpper = "DDLACTIVITYGROUP2" Then

                Me.ddlAct.DataSource = dv_Activity.ToTable()
                Me.ddlAct.DataValueField = "vActivityId"
                Me.ddlAct.DataTextField = "vActivityName"
                Me.ddlAct.DataBind()
                Me.ddlAct.Items.Insert(0, New ListItem(" Select Activity", "0"))
                ' tooltip
                For iMedexGroup As Integer = 0 To ddlAct.Items.Count - 1
                    ddlAct.Items(iMedexGroup).Attributes.Add("title", ddlAct.Items(iMedexGroup).Text)
                Next iMedexGroup
                '=========

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, estr)
        End Try
    End Function

    Protected Sub ddlAddActivity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAddActivity.SelectedIndexChanged
        Dim estr As String = String.Empty
        Dim ds As New Data.DataSet
        Try


            If Not objHelp.GetActivityStageDetailByActivityId(Me.ddlAddActivity.SelectedValue, ds, estr) Then

                Me.objCommon.ShowAlert(estr, Me.Page())
                Exit Sub

            End If

            Me.GVActivityName.DataSource = ds
            Me.GVActivityName.DataBind()

            mpeDialogAddActivity.Show()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, estr)
        End Try
    End Sub
#End Region

#Region "Fill Period"
    Private Function FillPeriod(ByVal VWorkspaceId As String, ByVal dropToFill As DropDownList, ByVal ActivityState As String) As Boolean

        If VWorkspaceId = "" Then
            Me.ShowErrorMessage("Problem While Getting vWorkspaceId", "")
            FillPeriod = False
            Exit Function
        End If

        Dim wStr As String = ""
        Dim dsPeriod As New DataSet
        Dim iPeriodNumbers As Integer = 0
        Dim dt As DataTable
        Dim dr() As DataRow

        wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
        If Not objHelp.GetWorkspaceProtocolDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                        dsPeriod, eStr_Retu) Then
            Me.ShowErrorMessage(eStr_Retu, "")
            Exit Function
        End If

        dropToFill.Items.Clear()
        If dsPeriod.Tables(0).Rows.Count > 0 AndAlso dsPeriod.Tables(0).Rows(0).Item("iNoOfPeriods").ToString() <> System.DBNull.Value.ToString() Then
            iPeriodNumbers = dsPeriod.Tables(0).Rows(0).Item("iNoOfPeriods")
        End If
        If iPeriodNumbers > 0 Then
            For count As Integer = 1 To dsPeriod.Tables(0).Rows(0).Item("iNoOfPeriods")
                dropToFill.Items.Add(count.ToString())
            Next
        End If

        If dropToFill.Items.Count <= 0 Then
            dropToFill.Items.Add(0)
        End If

        dt = CType(Me.ViewState("TreeNodes"), DataTable)
        If ActivityState = "Edit" Then
            dt = CType(Me.ViewState("dtWorkspaceNodeDetail1"), DataTable)
        End If
        dr = dt.Select(" vWorkspaceId='" & Me.HProjectId.Value & "' and iNodeId=" & Me.TVWorkspace.CheckedNodes(0).ImageToolTip)
        Me.ViewState("iPeriodOfParent") = 0
        If dr(0).Item("iPeriod").ToString() <> System.DBNull.Value.ToString() Then
            Me.ViewState("iPeriodOfParent") = dr(0).Item("iPeriod").ToString()
        End If
        If iPeriodNumbers > 0 Then
            If Me.ViewState("iPeriodOfParent") <> System.DBNull.Value.ToString() AndAlso Me.ViewState("iPeriodOfParent") <> 0 Then
                dropToFill.SelectedValue = Me.ViewState("iPeriodOfParent").ToString()
            End If
        End If

        FillPeriod = True
    End Function
#End Region

    '#Region "Add Last"
    '    Protected Sub btnAddLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddLast.Click

    '        If Me.ddlAddActivity.SelectedIndex <= 0 And Me.ddlActivityGroup.SelectedIndex <= 0 And Me.txtActDispName.Text.Length <= 0 Then
    '            objCommon.ShowAlert("Please Fill All The Information", Me.Page())
    '            Exit Sub
    '        End If

    '        Dim ds_addlst As New DataSet
    '        Dim ds_CanStart As New DataSet
    '        Dim ds_CanStartActvity As New DataSet
    '        Dim eStr As String = String.Empty
    '        Dim wStr As String = String.Empty
    '        Dim CanStartActivities As String = String.Empty

    '        Try

    '            'Start To check Canstart after activity on 19-Nov-2008
    '            wStr = " cStatusindi<>'D' and vWorkSpaceId='" + Me.HProjectId.Value + "'" + _
    '                    " and Charindex(vActivityId,case (select VcanStartAfter from ActivityMst where vActivityId='" + _
    '                    Me.ddlAddActivity.SelectedValue.Trim() + "') " + " when '0000' then vActivityId else " + _
    '                    " (select VcanStartAfter from ActivityMst where vActivityId='" + Me.ddlAddActivity.SelectedValue.Trim() + _
    '                    "') end)>0 "

    '            If Not Me.objHelp.getWorkSpaceNodeDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                                    ds_CanStart, eStr) Then
    '                Me.objCommon.ShowAlert(eStr, Me.Page())
    '                Exit Sub

    '            End If

    '            If ds_CanStart.Tables(0).Rows.Count <= 0 Then
    '                wStr = " Charindex(vActivityId,case (select VcanStartAfter from ActivityMst where vActivityId='" + _
    '                                    Me.ddlAddActivity.SelectedValue.Trim() + "') " + " when '0000' then vActivityId else " + _
    '                                    " (select VcanStartAfter from ActivityMst where vActivityId='" + Me.ddlAddActivity.SelectedValue.Trim() + _
    '                                    "') end)>0 "

    '                If Not Me.objHelp.getActivityMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                                        ds_CanStartActvity, eStr) Then

    '                    Me.objCommon.ShowAlert(eStr, Me.Page())
    '                    Exit Sub

    '                End If

    '                For index As Integer = 0 To ds_CanStartActvity.Tables(0).Rows.Count - 1
    '                    CanStartActivities += IIf(CanStartActivities = "", "", ", ") + _
    '                                        "'" + ds_CanStartActvity.Tables(0).Rows(index)("vActivityName") + "'"
    '                Next index

    '                Me.objCommon.ShowAlert("Please, First Add: " + CanStartActivities + " Activities Then Only You Can Add This Activity.", Me.Page)
    '                Exit Sub
    '            End If

    '            '***********End Check

    '            AssignValue()
    '            ds_addlst = New DataSet
    '            ds_addlst.Tables.Add(Me.ViewState("dtWorkspaceNodeDetail"))
    '            ds_addlst.Tables(0).TableName = "WorkspaceNodeDetail"   ' New Values on the form to be updated

    '            If Not objLambda.Save_InsertWorkspaceLeafNode(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_addlst, Me.Session("UserCode"), eStr) Then
    '                Me.objCommon.ShowAlert(eStr, Me.Page())
    '                Exit Sub
    '            End If


    '            Me.TVWorkspace.Enabled = True
    '            BindTree()
    '            'TVWorkspace.ExpandAll()
    '            'TVWorkspace.CollapseAll()

    '            Me.mpeDialogAddActivity.Hide()

    '        Catch ex As Exception
    '            Me.ShowErrorMessage(ex.Message, "...btnAddLast_Click", ex)
    '        End Try
    '    End Sub
    '#End Region

    '#Region "Add Before"
    '    Protected Sub btnAddBefore_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddBefore.Click

    '        If Me.ddlAddActivity.SelectedIndex <= 0 And Me.ddlActivityGroup.SelectedIndex <= 0 And Me.txtActDispName.Text.Length <= 0 Then
    '            objCommon.ShowAlert("Please Fill All The Information", Me.Page())
    '            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + Me.divTV.ClientID.ToString.Trim() + _
    '                                                    "');", True)
    '            Exit Sub
    '        End If

    '        Dim ds_addbfr As DataSet

    '        Dim ds_CanStart As New DataSet
    '        Dim ds_CanStartActvity As New DataSet
    '        Dim eStr As String = String.Empty
    '        Dim wStr As String = String.Empty
    '        Dim CanStartActivities As String = String.Empty

    '        Try


    '            'Start To check Canstart after activity on 19-Nov-2008
    '            wStr = " cStatusindi<>'D' and vWorkSpaceId='" + Me.HProjectId.Value + "'" + _
    '                    " and Charindex(vActivityId,case (select VcanStartAfter from ActivityMst where vActivityId='" + _
    '                    Me.ddlAddActivity.SelectedValue.Trim() + "') " + " when '0000' then vActivityId else " + _
    '                    " (select VcanStartAfter from ActivityMst where vActivityId='" + Me.ddlAddActivity.SelectedValue.Trim() + _
    '                    "') end)>0 "

    '            If Not Me.objHelp.getWorkSpaceNodeDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                                    ds_CanStart, eStr) Then

    '                Me.objCommon.ShowAlert(eStr, Me.Page())
    '                Exit Sub

    '            End If

    '            If ds_CanStart.Tables(0).Rows.Count <= 0 Then
    '                wStr = " Charindex(vActivityId,case (select VcanStartAfter from ActivityMst where vActivityId='" + _
    '                                    Me.ddlAddActivity.SelectedValue.Trim() + "') " + " when '0000' then vActivityId else " + _
    '                                    " (select VcanStartAfter from ActivityMst where vActivityId='" + Me.ddlAddActivity.SelectedValue.Trim() + _
    '                                    "') end)>0 "

    '                If Not Me.objHelp.getActivityMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                                        ds_CanStartActvity, eStr) Then

    '                    Me.objCommon.ShowAlert(eStr, Me.Page())
    '                    Exit Sub

    '                End If

    '                For index As Integer = 0 To ds_CanStartActvity.Tables(0).Rows.Count - 1
    '                    CanStartActivities += IIf(CanStartActivities = "", "", ", ") + _
    '                                        "'" + ds_CanStartActvity.Tables(0).Rows(index)("vActivityName") + "'"
    '                Next index

    '                Me.objCommon.ShowAlert("Please, First Add: " + CanStartActivities + " Activities Then Only You Can Add This Activity.", Me.Page)
    '                Exit Sub

    '            End If

    '            '***********End Check

    '            AssignValue()
    '            ds_addbfr = New DataSet
    '            ds_addbfr.Tables.Add(Me.ViewState("dtWorkspaceNodeDetail"))
    '            ds_addbfr.Tables(0).TableName = "WorkspaceNodeDetail"   ' New Values on the form to be updated

    '            If Not objLambda.Save_InsertWorkspaceNodeBefore(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_addbfr, Me.Session(S_UserID), eStr) Then

    '                Me.objCommon.ShowAlert(eStr, Me.Page())
    '                Exit Sub

    '            End If

    '            'To not allow to insert same activity on refresh 
    '            ds_addbfr.Clear()
    '            Me.TVWorkspace.Enabled = True
    '            BindTree()
    '            'TVWorkspace.ExpandAll()
    '            'TVWorkspace.CollapseAll()

    '            Me.mpeDialogAddActivity.Hide()

    '        Catch ex As System.Threading.ThreadAbortException
    '        Catch ex As Exception
    '            Me.ShowErrorMessage(ex.Message, "...btnAddBefore_Click", ex)
    '        End Try
    '    End Sub
    '#End Region

    '#Region "Add After"
    '    Protected Sub btnAddAfter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddAfter.Click

    '        If Me.ddlAddActivity.SelectedIndex <= 0 And Me.ddlActivityGroup.SelectedIndex <= 0 And Me.txtActDispName.Text.Length <= 0 Then
    '            objCommon.ShowAlert("Please Fill All The Information", Me.Page())
    '            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + Me.divTV.ClientID.ToString.Trim() + _
    '                                                    "');", True)
    '            Exit Sub
    '        End If

    '        Dim ds_addaft As DataSet
    '        Dim ds_CanStart As New DataSet
    '        Dim ds_CanStartActvity As New DataSet
    '        Dim eStr As String = String.Empty
    '        Dim wStr As String = String.Empty
    '        Dim CanStartActivities As String = String.Empty

    '        Try



    '            'Start To check Canstart after activity on 19-Nov-2008
    '            wStr = " cStatusindi<>'D' and vWorkSpaceId='" + Me.HProjectId.Value + "'" + _
    '                    " and Charindex(vActivityId,case (select VcanStartAfter from ActivityMst where vActivityId='" + _
    '                    Me.ddlAddActivity.SelectedValue.Trim() + "') " + " when '0000' then vActivityId else " + _
    '                    " (select VcanStartAfter from ActivityMst where vActivityId='" + Me.ddlAddActivity.SelectedValue.Trim() + _
    '                    "') end)>0 "

    '            If Not Me.objHelp.getWorkSpaceNodeDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                                    ds_CanStart, eStr) Then

    '                Me.objCommon.ShowAlert(eStr, Me.Page())
    '                Exit Sub

    '            End If

    '            If ds_CanStart.Tables(0).Rows.Count <= 0 Then
    '                wStr = " Charindex(vActivityId,case (select VcanStartAfter from ActivityMst where vActivityId='" + _
    '                                    Me.ddlAddActivity.SelectedValue.Trim() + "') " + " when '0000' then vActivityId else " + _
    '                                    " (select VcanStartAfter from ActivityMst where vActivityId='" + Me.ddlAddActivity.SelectedValue.Trim() + _
    '                                    "') end)>0 "

    '                If Not Me.objHelp.getActivityMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                                        ds_CanStartActvity, eStr) Then

    '                    Me.objCommon.ShowAlert(eStr, Me.Page())
    '                    Exit Sub

    '                End If

    '                For index As Integer = 0 To ds_CanStartActvity.Tables(0).Rows.Count - 1
    '                    CanStartActivities += IIf(CanStartActivities = "", "", ", ") + _
    '                                        "'" + ds_CanStartActvity.Tables(0).Rows(index)("vActivityName") + "'"
    '                Next index

    '                Me.objCommon.ShowAlert("Please, First Add: " + CanStartActivities + " Activities Then Only You Can Add This Activity.", Me.Page)
    '                Exit Sub
    '            End If

    '            '***********End Check

    '            AssignValue()
    '            ds_addaft = New DataSet
    '            ds_addaft.Tables.Add(Me.ViewState("dtWorkspaceNodeDetail"))
    '            ds_addaft.Tables(0).TableName = "WorkspaceNodeDetail"   ' New Values on the form to be updated

    '            If Not objLambda.Save_InsertWorkspaceNodeAfter(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_addaft, Me.Session(S_UserID), eStr) Then

    '                Me.objCommon.ShowAlert(eStr, Me.Page())
    '                Exit Sub

    '            End If
    '            'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + Me.DivAllChilds.ClientID.ToString.Trim() + _
    '            '                                        "');", True)
    '            'To not allow to insert same activity on refresh 
    '            ds_addaft.Clear()

    '            Me.TVWorkspace.Enabled = True
    '            BindTree()
    '            'TVWorkspace.ExpandAll()
    '            'TVWorkspace.CollapseAll()

    '            Me.mpeDialogAddActivity.Hide()
    '            'Me.HActivityDone.Value = "AddAfter"
    '            'Me.mpeApplyToAllChilds.Show()
    '        Catch ex As System.Threading.ThreadAbortException
    '        Catch ex As Exception
    '            Me.ShowErrorMessage(ex.Message, "...btnAddAfter_Click", ex)
    '        End Try
    '    End Sub
    '#End Region

#Region "AssingValues"
    Private Sub AssignValue()
        Dim dtWorkspaceNodeDetail As New DataTable
        Dim dr As DataRow
        Dim index As Integer
        Dim WorkspaceId As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds_ActivityMst As New DataSet
        Dim ds_WorkspacenodeDetail As New Data.DataSet
        Dim Wstr As String = "vActivityId='" + ddlAddActivity.SelectedValue + "'"

        Try


            dtWorkspaceNodeDetail = Me.ViewState("dtWorkspaceNodeDetail")
            dtWorkspaceNodeDetail.Rows.Clear()
            WorkspaceId = Me.HProjectId.Value
            dr = dtWorkspaceNodeDetail.NewRow

            dr("iNodeId") = Me.ViewState("iNodeId") 'Me.TVWorkspace.CheckedNodes(i).ImageToolTip
            dr("vWorkspaceId") = WorkspaceId
            dr("vNodeName") = Me.ddlAddActivity.SelectedItem.Text.Trim ' Me.ViewState("vnodename")
            dr("vActivityId") = Me.ddlAddActivity.SelectedValue
            dr("iPeriod") = Me.ddlPeriod.SelectedValue

            'Get Period Specific and milestone from activity master
            If ((Me.ddlAddActivity.SelectedIndex > 0)) Then

                If Not objHelp.getActivityMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ActivityMst, estr) Then

                    Me.objCommon.ShowAlert(estr, Me.Page())
                    Exit Sub

                End If

                If (ds_ActivityMst.Tables(0).Rows(0)("nMilestone").ToString() <> "") Then

                    dr("nMilestone") = ds_ActivityMst.Tables(0).Rows(0)("nMilestone").ToString()

                End If
            End If

            dr("vNodeDisplayName") = Me.txtActDispName.Text.Trim()
            dr("vFolderName") = Me.ddlAddActivity.SelectedItem.Text.Trim
            dr("vRemark") = ""
            dr("iModifyBy") = Me.Session(S_UserID)

            For index = 0 To Me.TVWorkspace.CheckedNodes.Count - 1

                Me.ViewState("vWorkspaceid") = Me.TVWorkspace.CheckedNodes(index).Value
                Me.ViewState("iNodeId") = Me.TVWorkspace.CheckedNodes(index).ImageToolTip

            Next index

            dtWorkspaceNodeDetail.Rows.Add(dr)
            Me.ViewState("dtWorkspaceNodeDetail") = dtWorkspaceNodeDetail

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....AssignValue", ex)
        End Try
    End Sub
#End Region

#Region "Edit Activity"
    Private Function AssignValue2() As Boolean
        Dim dtWorkspaceNodeDetail As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim Workspacetype As String = String.Empty
        Dim ds_WorkspacenodeDetail As New Data.DataSet
        Dim dtOld As DataTable
        Try

            dtOld = Me.ViewState("dtWorkspaceNodeDetail1")

            If Me.ViewState("Choice") = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                dtOld.Rows.Clear()
                dr = dtOld.NewRow
                dtOld.Rows.Add(dr)
            Else
                dr = dtOld.Rows(0)
            End If

            dr("iNodeId") = Me.TVWorkspace.CheckedNodes(i).ImageToolTip 'Me.ViewState("iNodeId")
            dr("vNodeName") = Me.ddlAct.SelectedItem.Text.Trim ' Me.ViewState("vnodename")
            dr("vNodeDisplayName") = Me.txtDisplayName.Text.Trim()
            dr("vActivityId") = Me.ddlAct.SelectedValue
            dr("iPeriod") = Me.ddlPeriodEdit.SelectedValue
            dr("cStatusIndi") = "E"
            dr.AcceptChanges()

            Me.ViewState("dtWorkspaceNodeDetail1") = dtOld
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..AssignValue2", ex)
        End Try
    End Function
#End Region

#Region "attach Activity"
    Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click

        If Me.ddlActivityGroup2.SelectedIndex <= 0 And Me.ddlAct.SelectedIndex <= 0 And Me.txtDisplayName.Text.Length <= 0 Then
            objCommon.ShowAlert("Please Fill All The Information", Me.Page())
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + Me.divact.ClientID.ToString.Trim() + _
                                                    "');", True)
            Exit Sub
        End If

        Dim ds As DataSet
        Dim ds_CanStart As New DataSet
        Dim ds_CanStartActvity As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim CanStartActivities As String = String.Empty
        Try


            'Start To check Canstart after activity on 19-Nov-2008
            wStr = " cStatusindi<>'D' and vWorkSpaceId='" + Me.HProjectId.Value + "'" + _
                    " and Charindex(vActivityId,case (select VcanStartAfter from ActivityMst where vActivityId='" + _
                    Me.ddlAct.SelectedValue.Trim() + "') " + " when '0000' then vActivityId else " + _
                    " (select VcanStartAfter from ActivityMst where vActivityId='" + Me.ddlAct.SelectedValue.Trim() + _
                    "') end)>0 "

            If Not Me.objHelp.getWorkSpaceNodeDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                    ds_CanStart, eStr) Then

                Me.objCommon.ShowAlert(eStr, Me.Page())
                Exit Sub

            End If

            If ds_CanStart.Tables(0).Rows.Count <= 0 Then
                wStr = " Charindex(vActivityId,case (select VcanStartAfter from ActivityMst where vActivityId='" + _
                                    Me.ddlAct.SelectedValue.Trim() + "') " + " when '0000' then vActivityId else " + _
                                    " (select VcanStartAfter from ActivityMst where vActivityId='" + Me.ddlAct.SelectedValue.Trim() + _
                                    "') end)>0 "

                If Not Me.objHelp.getActivityMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                        ds_CanStartActvity, eStr) Then

                    Me.objCommon.ShowAlert(eStr, Me.Page())
                    Exit Sub

                End If

                For index As Integer = 0 To ds_CanStartActvity.Tables(0).Rows.Count - 1
                    CanStartActivities += IIf(CanStartActivities = "", "", ", ") + _
                                        "'" + ds_CanStartActvity.Tables(0).Rows(index)("vActivityName") + "'"
                Next

                Me.objCommon.ShowAlert("Please, First Add: " + CanStartActivities + " Activities Then Only You Can Add This Activity.", Me.Page)
                Exit Sub
            End If

            '***********End Check
            AssignValue2()
            ds = New DataSet
            ds.Tables.Add(Me.ViewState("dtWorkspaceNodeDetail1"))
            ds.Tables(0).TableName = "WorkspaceNodeDetail"   ' New Values on the form to be updated
            If Not objLambda.Save_WorkSpaceNodeDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds, Me.Session("UserCode"), eStr) Then

                Me.objCommon.ShowAlert(eStr, Me.Page())
                Exit Sub

            End If

            BindTree()
            'TVWorkspace.ExpandAll()
            'TVWorkspace.CollapseAll()

            Me.TVWorkspace.Enabled = True


        Catch ex As System.Threading.ThreadAbortException
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....btnOK_Click", ex)
        End Try
    End Sub
#End Region

#Region "Create Table"
    Private Sub CreateWorkspaceNodeTable()
        Dim dtinactWorkspacenode As New DataTable
        Dim dc As DataColumn
        Try
            dc = New DataColumn
            dc.ColumnName = "inodeId"
            dc.DataType = GetType(String)
            dtinactWorkspacenode.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "vWorkspaceId"
            dc.DataType = GetType(String)
            dtinactWorkspacenode.Columns.Add(dc)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "...CreateWorkspaceNodeTable")

        End Try

        Me.ViewState("dtinactWorkspacenode") = dtinactWorkspacenode
    End Sub
#End Region

#Region "Button Click Event"

    'Protected Sub btneditact_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btneditact.Click
    '    Dim Wstr As String = String.Empty
    '    Dim eStr As String = String.Empty
    '    Dim dsWorkspacenodedetail As New DataSet
    '    Dim i As Integer
    '    Dim dsActivity As New DataSet

    '    Me.divact.Visible = True

    '    Me.ddlActivityGroup2.SelectedIndex = 0
    '    Me.ddlAct.SelectedIndex = 0


    '    If Me.TVWorkspace.CheckedNodes.Count = 1 Then

    '        Wstr = " vActivityId=" + Me.ddlAct.SelectedValue() 'Value of where condition
    '        Wstr = " vWorkspaceId='" & Me.HProjectId.Value & "' and iNodeId=" & Me.TVWorkspace.CheckedNodes(i).ImageToolTip '& " and vNodeName='" & Me.ddlAct.SelectedItem.Text.Trim & "'"

    '        If Not objHelp.getWorkSpaceNodeDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsWorkspacenodedetail, eStr_Retu) Then
    '            Throw New Exception(eStr)
    '        End If

    '        Me.ViewState("dtWorkspaceNodeDetail1") = dsWorkspacenodedetail.Tables(0)

    '        If Not objHelp.getActivityMst(" vActivityId = '" + Me.TVWorkspace.CheckedNodes(i).ToolTip.ToString + "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsActivity, eStr) Then
    '            Throw New Exception(eStr)
    '        End If

    '        Me.ddlActivityGroup2.SelectedValue = dsActivity.Tables(0).Rows(0)("vActivityGroupId")

    '        FillActivity("DDLACTIVITYGROUP2")
    '        Me.ddlAct.SelectedValue = Me.TVWorkspace.CheckedNodes(i).ToolTip.ToString


    '    End If
    '    ActivityState = "Edit"
    '    If Not FillPeriod(Me.HProjectId.Value, Me.ddlPeriodEdit, ActivityState) Then
    '        Me.ShowErrorMessage("Problem While Getting Periods For This Project", "")
    '    End If
    '    mpeDialogEditActivity.Show()
    'End Sub

    'Protected Sub BtnAddActivity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAddActivity.Click
    '    Dim index As Integer
    '    Me.divTV.Visible = True

    '    For index = 0 To Me.TVWorkspace.CheckedNodes.Count - 1
    '        Me.ViewState("vWorkspaceid") = Me.TVWorkspace.CheckedNodes(index).Value
    '        Me.ViewState("iNodeId") = Me.TVWorkspace.CheckedNodes(index).ImageToolTip
    '    Next
    '    ActivityState = "Add"
    '    If Not FillPeriod(Me.HProjectId.Value, Me.ddlPeriod, ActivityState) Then
    '        Me.ShowErrorMessage("Problem While getting periods for this project", "")
    '    End If
    '    mpeDialogAddActivity.Show()
    'End Sub

    'Protected Sub btnDeleteActivity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteActivity.Click

    '    Dim dsUpdate As New DataSet
    '    Dim dtupdatenode As New DataTable
    '    Dim dr As DataRow
    '    Dim ds_updatenode As New DataSet
    '    Dim i As Integer
    '    Dim eStr As String = String.Empty
    '    Dim wStr As String = String.Empty
    '    Dim wDelteStr As String = String.Empty
    '    Dim NodeIds As String = String.Empty
    '    Dim CanStartActivities As String = String.Empty
    '    Dim ds_CanStartActvity As New DataSet
    '    Dim ds_CanStart As New DataSet

    '    Try


    '        dtupdatenode = Me.ViewState("dtinactWorkspacenode")
    '        wDelteStr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "'"

    '        For i = 0 To Me.TVWorkspace.CheckedNodes.Count - 1


    '            'Start To check Canstart after activity on 19-Nov-2008
    '            wStr = " cStatusindi<>'D' and vWorkSpaceId='" + Me.HProjectId.Value + "'" + _
    '                    " and vActivityId in (select vActivityId from ActivityMst where vcanstartAfter like '%" + _
    '                    Me.TVWorkspace.CheckedNodes(i).ToolTip + "%')"

    '            If Not Me.objHelp.getWorkSpaceNodeDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                                    ds_CanStart, eStr) Then

    '                Me.objCommon.ShowAlert(eStr, Me.Page())
    '                Exit Sub

    '            End If
    '            If ds_CanStart.Tables(0).Rows.Count > 0 Then
    '                wStr = "  vcanstartAfter like '%" + _
    '                    Me.TVWorkspace.CheckedNodes(i).ToolTip + "%'"

    '                If Not Me.objHelp.getActivityMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                                        ds_CanStartActvity, eStr) Then

    '                    Me.objCommon.ShowAlert(eStr, Me.Page())
    '                    Exit Sub
    '                End If

    '                For index As Integer = 0 To ds_CanStartActvity.Tables(0).Rows.Count - 1
    '                    CanStartActivities += IIf(CanStartActivities = "", "", ",") + _
    '                                        ds_CanStartActvity.Tables(0).Rows(index)("vActivityName")
    '                Next

    '                Me.objCommon.ShowAlert("Please First Delete: " + CanStartActivities + " Activities Then Only You Can Delete This Activity.", Me.Page)
    '                Exit Sub
    '            End If

    '            '***********End Check


    '            If NodeIds = "" Then
    '                NodeIds += Me.TVWorkspace.CheckedNodes(i).ImageToolTip.ToString.Trim()
    '            Else
    '                NodeIds += "," + Me.TVWorkspace.CheckedNodes(i).ImageToolTip.ToString.Trim()
    '            End If

    '        Next

    '        wDelteStr += " and inodeId in (" & NodeIds & ")"

    '        If Not Me.objHelp.getWorkSpaceNodeDetail(wDelteStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsUpdate, eStr) Then
    '            Me.objCommon.ShowAlert("Error While Getting Node For Deletion:" + eStr, Me.Page)
    '            Exit Sub
    '        End If

    '        dtupdatenode = dsUpdate.Tables(0)

    '        For Each dr In dtupdatenode.Rows
    '            dr("cStatusIndi") = "D"

    '        Next

    '        ds_updatenode.Tables.Add(dtupdatenode.Copy)
    '        ds_updatenode.Tables(0).TableName = "WorkspaceNodeDetail"   ' New Values on the form to be updated

    '        'On 15-Nov-2008
    '        If Not objLambda.Save_WorkSpaceNodeDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_updatenode, Me.Session("UserCode"), eStr) Then

    '            Me.objCommon.ShowAlert(eStr, Me.Page())
    '            Exit Sub

    '        End If

    '        BindTree()
    '        'TVWorkspace.ExpandAll()
    '        'TVWorkspace.CollapseAll()
    '        '******************
    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.Message, "......btnDeleteActivity_Click", ex)
    '    End Try
    'End Sub

    Protected Sub btnuserrights_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnuserrights.Click

        Response.Redirect("frmuserRights.aspx?mode=1&WorkspaceId=" & Me.HProjectId.Value & "&default=yes")

    End Sub

    Protected Sub BtnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnBack.Click
        'Added on 10-Jul-2009
        If Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso Me.Request.QueryString("SubjectId") <> "" Then
            If Not IsNothing(Me.Request.QueryString("WorkspaceId")) AndAlso Me.Request.QueryString("WorkspaceId") <> "" Then
                Response.Redirect("frmVisitDetail.aspx?Page2=frmSubjectEnrollment&SubjectId=" + Me.Request.QueryString("SubjectId").Trim() + "&WorkspaceId=" + Me.HProjectId.Value.Trim())
            End If
        Else
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Show", "CloseTab();", True)
        End If
        '**********************************
    End Sub

    'Protected Sub btnclose_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    '    divTV.Visible = False
    '    Me.TVWorkspace.Enabled = True
    '    BindTree()
    '    'TVWorkspace.ExpandAll()
    '    'TVWorkspace.CollapseAll()
    '    Me.mpeDialogAddActivity.Hide()

    'End Sub

    Protected Sub btnsetuserrights_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsetuserrights.Click
        Dim index As Integer

        For index = 0 To Me.TVWorkspace.CheckedNodes.Count - 1
            Me.ViewState("vWorkspaceid") = Me.TVWorkspace.CheckedNodes(index).Value
            Me.ViewState("iNodeId") = Me.TVWorkspace.CheckedNodes(index).ImageToolTip
        Next

        Response.Redirect("frmEditUserRights.aspx?mode=1&WorkspaceId=" & Me.HProjectId.Value & "&NodeId=" & Me.ViewState("iNodeId") & "&Page2=frmtreenodeMst")
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click

        Me.divact.Visible = False
        Me.TVWorkspace.Enabled = True
        Me.mpeDialogEditActivity.Hide()

    End Sub

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim wstr As String = String.Empty
        Dim ds_Check As New DataSet
        Dim estr As String = String.Empty
        Dim ds_CrfVersionDetail As DataSet = Nothing
        Dim VersionNo As Integer = 0
        Dim VersionDate As Date

        'Me.BtnAddActivity.Style.Add("display", "")
        'Me.btneditact.Style.Add("display", "")
        'Me.btnDeleteActivity.Style.Add("display", "")

        '=========Added BY:vikas(TO Show crfversion detail and not allow to change structure in freeze state====================
        wstr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'"

        If Not objHelp.GetData("View_CRFVersionForDataEntryControl", "*", wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CrfVersionDetail, eStr_Retu) Then
            Throw New Exception(eStr_Retu)
        End If
        Me.Hdn_FreezeStatus.Value = ""
        If ds_CrfVersionDetail.Tables(0).Rows.Count > 0 Then
            Hdn_FreezeStatus.Value = ds_CrfVersionDetail.Tables(0).Rows(0)("cFreezeStatus").ToString.ToUpper.Trim
            VersionNo = ds_CrfVersionDetail.Tables(0).Rows(0)("nVersionNo")
            VersionDate = ds_CrfVersionDetail.Tables(0).Rows(0)("dVersiondate")
            Me.VersionNo.Text = VersionNo.ToString
            Me.VersionDate.Text = VersionDate.ToString("dd-MMM-yyyy")

            Me.VersionDtl.Attributes.Add("style", "display:block;")

            If ds_CrfVersionDetail.Tables(0).Rows(0)("cFreezeStatus").ToString.Trim.ToUpper = "U" Then
                ImageLockUnlock.Attributes.Add("src", "images/UnFreeze.jpg")
            End If
        End If
        '======================================================================================
        wstr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D' Order by iTranNo desc"

        If Not Me.objHelp.GetData("WorkspaceMst", "*", "vWorkspaceId= '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D' And cWorkspaceType='P'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Check, estr) Then
            Throw New Exception(estr)
        End If
        'If ds_Check.Tables(0).Rows.Count <= 0 Then
        '    Me.BtnAddActivity.Style.Add("display", "none")
        '    Me.btneditact.Style.Add("display", "none")
        '    Me.btnDeleteActivity.Style.Add("display", "none")
        'End If

        If Not Me.objHelp.GetCRFLockDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_Check, estr) Then
            Throw New Exception(estr)
        End If

        If ds_Check.Tables(0).Rows.Count > 0 Then

            If Convert.ToString(ds_Check.Tables(0).Rows(0)("cLockFlag")).Trim.ToUpper() = "L" Then
                Me.objCommon.ShowAlert("Project Is Locked.", Me.Page)
                'btnDeleteActivity.Visible = False
                'BtnAddActivity.Visible = False
                'btneditact.Visible = False
            Else
                'btnDeleteActivity.Visible = True
                'BtnAddActivity.Visible = True
                'btneditact.Visible = True
            End If
            BindTree()
        Else
            BindTree()
            'btnDeleteActivity.Visible = True
            'BtnAddActivity.Visible = True
            'btneditact.Visible = True
        End If
    End Sub

    Protected Sub btnExitPage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExitPage.Click

        'Added on 10-Jul-2009
        If Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso Me.Request.QueryString("SubjectId") <> "" Then
            If Not IsNothing(Me.Request.QueryString("WorkspaceId")) AndAlso Me.Request.QueryString("WorkspaceId") <> "" Then
                Response.Redirect("frmVisitDetail.aspx?Page2=frmSubjectEnrollment&SubjectId=" + Me.Request.QueryString("SubjectId").Trim() + "&WorkspaceId=" + Me.HProjectId.Value.Trim())
            End If
        End If
        '**********************************

        Me.Response.Redirect("frmMainPage.aspx")

    End Sub

#End Region

#Region "Error Handler"
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub
#End Region

#Region "ActivityMedExTemplate"

    Protected Function ActivityMedExTemplate() As Boolean
        Dim ds_ActivityMedExTemplateDtl As New DataSet
        Dim ds_MedExWorkSpaceHdrDtl As New DataSet
        Dim dt_MedExWorkSpaceHdr As New DataTable
        Dim dt_MedExWorkSpaceDtl As New DataTable
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim dr As DataRow
        Dim drHdr As DataRow
        Dim drDtl As DataRow

        Try


            wStr = "vActivityId ='" + ddlAddActivity.SelectedValue + "'"

            If Not Me.objHelp.VIEW_ActivityMedExTemplateDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_ActivityMedExTemplateDtl, eStr) Then

                Me.objCommon.ShowAlert("Error While Getting Data From View_MedExTemplateDtl :" + eStr, Me.Page)
                Exit Function

            End If

            If ds_ActivityMedExTemplateDtl.Tables(0).Rows.Count > 0 Then

                If Not Me.objHelp.GetMedExWorkSpaceHdr("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                            ds_MedExWorkSpaceHdrDtl, eStr) Then
                    Me.objCommon.ShowAlert("Error While Getting Data From MedExWorkSpaceHdr :" + eStr, Me.Page)
                    Exit Function
                End If

                dt_MedExWorkSpaceHdr = ds_MedExWorkSpaceHdrDtl.Tables(0).Copy()
                ds_MedExWorkSpaceHdrDtl = Nothing
                ds_MedExWorkSpaceHdrDtl = New DataSet

                If Not Me.objHelp.GetMedExWorkSpaceDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                                            ds_MedExWorkSpaceHdrDtl, eStr) Then
                    Me.objCommon.ShowAlert("Error While Getting Data From MedExWorkSpaceDtl :" + eStr, Me.Page)
                    Exit Function
                End If
                dt_MedExWorkSpaceDtl = ds_MedExWorkSpaceHdrDtl.Tables(0).Copy()
                ds_MedExWorkSpaceHdrDtl = Nothing

                drHdr = dt_MedExWorkSpaceHdr.NewRow()
                drHdr("nMedExWorkSpaceHdrNo") = 0
                drHdr("vActivityId") = Me.ddlAddActivity.SelectedValue
                drHdr("vWorkspaceId") = Me.HProjectId.Value
                drHdr("iModifyBy") = Me.Session(S_UserID)
                drHdr("cStatusIndi") = "N"
                dt_MedExWorkSpaceHdr.Rows.Add(drHdr)
                dt_MedExWorkSpaceHdr.AcceptChanges()

                For Each dr In ds_ActivityMedExTemplateDtl.Tables(0).Rows

                    drDtl = dt_MedExWorkSpaceDtl.NewRow()
                    drDtl("nMedExWorkSpaceDtlNo") = dt_MedExWorkSpaceDtl.Rows.Count - 999
                    drDtl("nMedExWorkSpaceHdrNo") = 0
                    drDtl("iSeqNo") = dr("iSeqNo")
                    drDtl("vMedExCode") = dr("vMedExCode")
                    drDtl("vMedExDesc") = dr("vMedExDesc")
                    drDtl("vMedExDefaultValue") = dr("vDefaultValue")
                    drDtl("vLowRange") = dr("vLowRange")
                    drDtl("vHighRange") = dr("vHighRange")
                    drDtl("vWarningOnValue") = dr("vAlertonvalue")
                    drDtl("vWarningMsg") = dr("vAlertMessage")
                    drDtl("cActiveFlag") = dr("cActiveFlag")
                    drDtl("iModifyBy") = Me.Session(S_UserID)
                    drDtl("cStatusIndi") = "N"
                    dt_MedExWorkSpaceDtl.Rows.Add(drDtl)

                Next dr

                dt_MedExWorkSpaceDtl.AcceptChanges()

                ds_MedExWorkSpaceHdrDtl = New DataSet
                ds_MedExWorkSpaceHdrDtl.Tables.Add(dt_MedExWorkSpaceHdr.Copy())
                ds_MedExWorkSpaceHdrDtl.Tables(0).TableName = "MedExWorkSpaceHdr"
                ds_MedExWorkSpaceHdrDtl.Tables.Add(dt_MedExWorkSpaceDtl.Copy())
                ds_MedExWorkSpaceHdrDtl.Tables(1).TableName = "MedExWorkSpaceDtl"

                If Not Me.objLambda.Save_MedExWorkspaceDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                     WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExWorkspaceDtl, _
                                     ds_MedExWorkSpaceHdrDtl, Me.Session(S_UserID), eStr) Then

                    Me.objCommon.ShowAlert("Error While Saving Record :" + eStr, Me.Page)
                    Exit Function

                End If

            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....ActivityMedExTemplate")
            Return False
        End Try

    End Function

#End Region


End Class
