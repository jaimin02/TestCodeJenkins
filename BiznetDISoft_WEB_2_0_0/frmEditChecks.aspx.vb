
Partial Class frmEditChecks
    Inherits System.Web.UI.Page

#Region "Variable Declaration "

    Private objCommon As New clsCommon
    Private objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

    Private VS_Choice As String = "Choice"
    Private VS_DtEditChecks As String = "DtEditChecks"
    Private VS_DtGrid As String = "DtGrid"

    Private VS_DtTree As String = "DtTree"
    Private VS_MedExEditCheckNo As String = "MedExEditCheckNo"
    Private VS_DtWorkspaceNodeDetail As String = "DtWorkspaceNodeDetail"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_MedExEditCheckNo As Integer = 1
    Private Const GVC_WorkspaceId As Integer = 2
    Private Const GVC_ParentNodeId As Integer = 3
    Private Const GVC_SourceNodeId_If As Integer = 4
    Private Const GVC_SourceMedExCode_If As Integer = 5
    Private Const GVC_Operator_If As Integer = 6
    Private Const GVC_TargetNodeId_If As Integer = 7
    Private Const GVC_TargetMedExCode_If As Integer = 8
    Private Const GVC_TargetValue_If As Integer = 9
    Private Const GVC_SourceNodeId_Then As Integer = 10
    Private Const GVC_SourceMedExCode_Then As Integer = 11
    Private Const GVC_Operator_Then As Integer = 12
    Private Const GVC_TargetNodeId_Then As Integer = 13
    Private Const GVC_TargetMedExCode_Then As Integer = 14
    Private Const GVC_TargetValue_Then As Integer = 15
    Private Const GVC_Condition As Integer = 16
    Private Const GVC_Remarks As Integer = 17
    Private Const GVC_Edit As Integer = 18
    Private Const GVC_Delete As Integer = 19
    Private Const GVC_StatusIndi As Integer = 20

    Private Const Scope_ClinicalTrial As Integer = 1

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Me.Page.Title = ":: Edit Checks ::" + System.Configuration.ConfigurationManager.AppSettings("Client")
            If Not GenCall_Data() Then
                Exit Sub
            End If
        End If
    End Sub

#End Region

#Region "GENCALL_DATA "

    Private Function GenCall_Data() As Boolean
        Dim ds_EditChecks As DataSet = Nothing
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim ds_UserMst As DataSet = Nothing
        Dim sender As New Object
        Dim e As New EventArgs
        Try

            Me.pnlGrid.Visible = False
            Me.btnSave.Visible = False
            Me.btnAdd.Text = "Add"

            Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            Me.ViewState(VS_Choice) = Choice

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Edit Checks"

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""


            If Not Me.objhelpDb.GetMedExEditChecks("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                                         ds_EditChecks, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_EditChecks Is Nothing Then
                Throw New Exception(eStr)
            End If

            Me.ViewState(VS_DtEditChecks) = ds_EditChecks.Tables(0)


            'Added on 09-Apr-10 to show the set Project on page load
            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
            End If

            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
            '========================================================================

            If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                Me.trActivity.Attributes.Add("style", "display:")
                Me.trPeriod.Attributes.Add("style", "display:")
            End If

            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)
            Return False
        End Try
    End Function

#End Region

#Region "Filling Tree Structure"

    Private Sub fillTreeTable()
        Dim Ds_Tree As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim parentNode As New TreeNode

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            wStr = "cStatusIndi <> 'D'"
            wStr += " And vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
            wStr += " And isnull(vTemplateId,'') <> '0001' And isnull(vTemplateId,'') <> '0040' "
            wStr += " And iParentNodeId = 1 Order by iNodeId,iNodeNo"

            If Not Me.objhelpDb.getWorkSpaceNodeDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                Ds_Tree, eStr) Then
                Throw New Exception(eStr)
            End If

            Me.trvwStructure.Nodes.Clear()
            'done on 24-MAy-2010
            Me.AddParent("1", New TreeNode(), Ds_Tree.Tables(0))
            'parentNode = New TreeNode
            'parentNode.Text = "Project"
            'parentNode.Value = "1"
            'trvwStructure.Nodes.Add(parentNode)
            ''''''

            wStr = "cStatusIndi <> 'D'"
            wStr += " And vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' order by iNodeNo,vActivityId,iSeqNo"
            If Not Me.objhelpDb.VIEW_MedExActivityVise(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                Ds_Tree, eStr) Then
                Throw New Exception(eStr)
            End If

            Me.ViewState(VS_DtTree) = Ds_Tree.Tables(0)

            Me.TestChild("1", New TreeNode())

            Me.trvwStructure.Attributes.Add("OnClick", "return NodeChecked('" + trvwStructure.ClientID + "');")
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Private Sub fillTreeTableforBABE()
        Dim Ds_Tree As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim parentNode As New TreeNode

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            wStr = "cStatusIndi <> 'D'"
            wStr += " And vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
            wStr += " And isnull(vTemplateId,'') <> '0001' And isnull(vTemplateId,'') <> '0040' "
            wStr += " AND iNodeId = " + Me.ddlActivity.SelectedValue.Trim()
            'wStr += " And (iParentNodeId = " + Me.ddlActivity.SelectedValue.Trim()
            'wStr += " OR iNodeId = " + Me.ddlActivity.SelectedValue.Trim() + ")"
            ''wStr += " And iPeriod = 1"
            wStr += " Order by iNodeId,iNodeNo"

            If Not Me.objhelpDb.getWorkSpaceNodeDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                Ds_Tree, eStr) Then
                Throw New Exception(eStr)
            End If

            Me.trvwStructure.Nodes.Clear()
            'done on 24-MAy-2010
            Me.AddParent("1", New TreeNode(), Ds_Tree.Tables(0))
            'parentNode = New TreeNode
            'parentNode.Text = "Project"
            'parentNode.Value = "1"
            'trvwStructure.Nodes.Add(parentNode)
            ''''''


            wStr = "cStatusIndi <> 'D'"
            wStr += " And vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' "
            wStr += " AND iNodeId = " + Me.ddlActivity.SelectedValue.Trim()
            'wStr += " And (iParentNodeId = " + Me.ddlActivity.SelectedValue.Trim()
            'wStr += " OR iNodeId = " + Me.ddlActivity.SelectedValue.Trim() + ")"
            wStr += " order by iNodeNo,vActivityId,iSeqNo"

            If Not Me.objhelpDb.VIEW_MedExActivityVise(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                Ds_Tree, eStr) Then
                Throw New Exception(eStr)
            End If

            Me.ViewState(VS_DtTree) = Ds_Tree.Tables(0)


            Me.TestChildforBaBE("1", New TreeNode())
            '====
            Me.trvwStructure.Attributes.Add("OnClick", "return NodeChecked('" + trvwStructure.ClientID + "');")
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Private Sub AddParent(ByVal parentId As String, ByVal tn As TreeNode, ByVal dt_new As DataTable)
        Dim parentNode As New TreeNode

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            For Each dr As DataRow In dt_new.Rows

                parentNode = New TreeNode
                parentNode.Text = dr("vNodeDisplayName").ToString
                parentNode.Value = dr("vActivityId").ToString & "(" & dr("iNodeId") & ")" & "(" & dr("iParentNodeId") & ")"
                trvwStructure.Nodes.Add(parentNode)

            Next dr

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Private Sub TestChild(ByVal parentId As String, ByVal tn As TreeNode)
        Dim dr() As DataRow
        Dim dt_new As DataTable
        Dim dv_new As New DataView
        Dim parentNode As New TreeNode
        Dim childNode As New TreeNode

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            For index As Integer = 0 To Me.trvwStructure.Nodes.Count - 1

                tn = Me.trvwStructure.Nodes(index)
                dt_new = CType(Me.ViewState(VS_DtTree), DataTable).Copy()

                'dr = dt_new.Select("iSeqNo = " & parentId & " And iParentNodeId = " & tn.Value.Substring(tn.Value.IndexOf("(") + 1, tn.Value.IndexOf(")") - (tn.Value.IndexOf("(") + 1)))
                'dr = dt_new.Select("iSeqNo = " & tn.Value.Substring(tn.Value.IndexOf("(") + 1, tn.Value.IndexOf(")") - (tn.Value.IndexOf("(") + 1)))

                dv_new = dt_new.DefaultView.ToTable(True, "vActivityId,vNodedisplayName,iNodeId,iParentNodeId,iNodeNo".Split(",")).DefaultView
                dt_new = Nothing
                dt_new = dv_new.ToTable()
                'added on 24-May-2010
                dr = dt_new.Select("iParentNodeId = " & tn.Value.Substring(tn.Value.IndexOf("(") + 1, tn.Value.IndexOf(")") - (tn.Value.IndexOf("(") + 1)))
                'dr = dt_new.Select("iParentNodeId =1")

                For indexparent As Integer = 0 To dr.Length - 1

                    parentNode = New TreeNode
                    parentNode.Text = dr(indexparent)("vNodeDisplayName").ToString
                    parentNode.Value = dr(indexparent)("vActivityId").ToString & "(" & dr(indexparent)("iNodeId") & ")" & "(" & dr(indexparent)("iParentNodeId") & ")"
                    trvwStructure.Nodes.Add(parentNode)
                    tn.ChildNodes.Add(parentNode)
                    '01-june
                    Dim dt_Child As New DataTable
                    dt_Child = CType(Me.ViewState(VS_DtTree), DataTable).Copy()

                    Dim dr_Child() As DataRow
                    dr_Child = dt_Child.Select("iNodeId = " & parentNode.Value.Substring(parentNode.Value.IndexOf("(") + 1, parentNode.Value.IndexOf(")") - (parentNode.Value.IndexOf("(") + 1)))

                    For indexChild As Integer = 0 To dr_Child.Length - 1

                        childNode = New TreeNode
                        childNode.Text = dr_Child(indexChild)("vMedExDesc").ToString + "(" + Convert.ToString(dr_Child(indexChild)("vMedExSubGroupDesc")).Trim() + ")" + "(" + Convert.ToString(dr_Child(indexChild)("vMedExGroupDesc")).Trim() + ")"
                        childNode.Value = "M-" & dr_Child(indexChild)("vMedExCode").ToString & "N-" & dr_Child(indexChild)("iNodeId") & "P-" & dr_Child(indexChild)("iParentNodeId") & "V-" & Convert.ToString(dr_Child(indexChild)("vMedExValues")).Trim()
                        trvwStructure.Nodes.Add(childNode)
                        parentNode.ChildNodes.Add(childNode)
                        'childNode.NavigateUrl = "javascript:void(0);"

                    Next

                Next indexparent

                '********************For Activities Which Does Not Have Any Child Activity********************
                If dr.Length = 0 Then

                    Dim dt_ChildMedEx As New DataTable
                    dt_ChildMedEx = CType(Me.ViewState(VS_DtTree), DataTable).Copy()

                    Dim dr_ChildMedEx() As DataRow
                    dr_ChildMedEx = dt_ChildMedEx.Select("iNodeId = " & tn.Value.Substring(tn.Value.IndexOf("(") + 1, tn.Value.IndexOf(")") - (tn.Value.IndexOf("(") + 1)))

                    For indexChild As Integer = 0 To dr_ChildMedEx.Length - 1

                        childNode = New TreeNode
                        childNode.Text = dr_ChildMedEx(indexChild)("vMedExDesc").ToString + "(" + Convert.ToString(dr_ChildMedEx(indexChild)("vMedExSubGroupDesc")).Trim() + ")" + "(" + Convert.ToString(dr_ChildMedEx(indexChild)("vMedExGroupDesc")).Trim() + ")"
                        childNode.Value = "M-" & dr_ChildMedEx(indexChild)("vMedExCode").ToString & "N-" & dr_ChildMedEx(indexChild)("iNodeId") & "P-" & dr_ChildMedEx(indexChild)("iParentNodeId") & "V-" & Convert.ToString(dr_ChildMedEx(indexChild)("vMedExValues")).Trim()
                        trvwStructure.Nodes.Add(childNode)
                        tn.ChildNodes.Add(childNode)

                    Next

                End If
                '**************************

            Next index

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Private Sub TestChildforBaBE(ByVal parentId As String, ByVal tn As TreeNode)
        Dim parentNode As New TreeNode
        Dim childNode As New TreeNode

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            For index As Integer = 0 To Me.trvwStructure.Nodes.Count - 1
                parentNode = Me.trvwStructure.Nodes(index)
                Dim dt_Child As New DataTable
                dt_Child = CType(Me.ViewState(VS_DtTree), DataTable).Copy()

                Dim dr_Child() As DataRow
                dr_Child = dt_Child.Select("iNodeId = " & parentNode.Value.Substring(parentNode.Value.IndexOf("(") + 1, parentNode.Value.IndexOf(")") - (parentNode.Value.IndexOf("(") + 1)))

                For indexChild As Integer = 0 To dr_Child.Length - 1

                    childNode = New TreeNode
                    childNode.Text = dr_Child(indexChild)("vMedExDesc").ToString + "(" + Convert.ToString(dr_Child(indexChild)("vMedExSubGroupDesc")).Trim() + ")" + "(" + Convert.ToString(dr_Child(indexChild)("vMedExGroupDesc")).Trim() + ")"
                    childNode.Value = "M-" & dr_Child(indexChild)("vMedExCode").ToString & "N-" & dr_Child(indexChild)("iNodeId") & "P-" & dr_Child(indexChild)("iParentNodeId") & "V-" & Convert.ToString(dr_Child(indexChild)("vMedExValues")).Trim()
                    'dr_Child(indexChild)("vMedExCode").ToString & "(" & dr_Child(indexChild)("iNodeId") & ")" & "(" & dr_Child(indexChild)("iParentNodeId") & ")"
                    trvwStructure.Nodes.Add(childNode)
                    parentNode.ChildNodes.Add(childNode)

                Next
            Next index

        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)
        End Try


    End Sub

#End Region

#Region "FillGrid "

    Private Function FillGrid() As Boolean
        Dim ds_Grid As DataSet = Nothing
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty

        Try

            If Me.HFParentNodeId.Value.Trim() = "" Then
                Return True
            End If

            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
            wStr += " And (iParentNodeId = " + Me.HFSourceNodeId.Value.Trim()
            wStr += " Or  iSourceNodeId_If = " + Me.HFSourceNodeId.Value.Trim()
            wStr += ") And cStatusIndi <> 'D'"




            If Not Me.objhelpDb.GetMedExEditChecks(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                         ds_Grid, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_Grid Is Nothing Then
                Throw New Exception(eStr)
            End If

            Me.ViewState(VS_DtGrid) = ds_Grid.Tables(0)
            'Me.ViewState(VS_DtEditChecks) = ds_Grid.Tables(0)

            BindGrid(ds_Grid.Tables(0))
            'Me.GVEditChecks.DataSource = ds_Grid.Tables(0)
            'Me.GVEditChecks.DataBind()

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)
            Return False
        End Try
    End Function

    Private Sub BindGrid(ByVal dt As DataTable)
        Dim dv As New DataView
        dv = dt.DefaultView
        dv.RowFilter = "cStatusIndi <> 'D'"
        Me.GVEditChecks.DataSource = dv.ToTable()
        Me.GVEditChecks.DataBind()
        Me.pnlGrid.Visible = False
        Me.btnSave.Visible = False
        If Me.GVEditChecks.Rows.Count > 0 Then
            Me.pnlGrid.Visible = True
            'Me.btnSave.Visible = True
        End If
    End Sub

#End Region

#Region "Fill Functions"

    Private Function FillddlPeriod() As Boolean
        Dim ds_WorkSpaceNodeDetail As New Data.DataSet
        Dim View_WorkSpaceNodeDetail As New DataView
        Dim estr As String = ""
        Dim wstr As String = "1=1"

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            Me.trvwStructure.Nodes.Clear()
            Me.ddlActivity.Items.Clear()
            Me.ddlActivity.Items.Insert(0, New ListItem("Select Activity", 0))

            wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' "
            ds_WorkSpaceNodeDetail = Nothing
            If Not objhelpDb.GetViewWorkSpaceNodeDetail(wstr, ds_WorkSpaceNodeDetail, estr) Then
                Me.objCommon.ShowAlert("Error While Getting Data from WorkSpaceModeDetail:" + estr, Me.Page)
                Return False
            End If

            View_WorkSpaceNodeDetail.Table = ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable()
            View_WorkSpaceNodeDetail.Sort = "iPeriod"

            Me.ddlPeriod.DataSource = ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable(True, "iPeriod".ToString())
            Me.ddlPeriod.DataValueField = "iPeriod"
            Me.ddlPeriod.DataTextField = "iPeriod"
            Me.ddlPeriod.DataBind()
            Me.ddlPeriod.Items.Insert(0, New ListItem("--Select Period--", 0))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function FillActivity() As Boolean
        Dim wstr As String = ""
        Dim Ds_FillActivity As New DataSet
        Dim eStr_Retu As String = ""
        Try
            wstr = " vWorkspaceId = '" & Me.HProjectId.Value.Trim()
            wstr += "' And iPeriod = " & Me.ddlPeriod.SelectedValue.Trim()
            wstr += " And (vActivityId is Not NULL or vActivityId<>'') and cstatusindi<>'D' order by vActivityName"

            If Not objhelpDb.GetViewWorkSpaceNodeDetail(wstr, Ds_FillActivity, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If
            If Ds_FillActivity.Tables(0).Rows.Count <= 0 Then
                objCommon.ShowAlert("No Record Found", Me)
                Me.ddlActivity.Items.Clear()
                Me.ddlActivity.Items.Insert(0, New ListItem("Select Activity", 0))
                Return True
                Exit Function
            End If

            Me.ddlActivity.DataSource = Ds_FillActivity
            Me.ddlActivity.DataTextField = "ActivityDisplayName"
            Me.ddlActivity.DataValueField = "iNodeid"
            Me.ddlActivity.DataBind()

            Me.ddlActivity.Items.Insert(0, New ListItem("Select Activity", 0))
            Me.trvwStructure.Nodes.Clear()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "FillNodeDetails "

    Private Function FillNodeDetails() As Boolean
        Dim ds_WorkspaceNodeDetail As DataSet = Nothing
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"
            If Not Me.objhelpDb.getWorkSpaceNodeDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_WorkspaceNodeDetail, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_WorkspaceNodeDetail Is Nothing Then
                Throw New Exception(eStr)
            End If

            Me.ViewState(VS_DtWorkspaceNodeDetail) = ds_WorkspaceNodeDetail.Tables(0)

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)
            Return False
        End Try
    End Function

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

#Region "Button Events"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_Check As New DataSet
        Dim dv_Check As New DataView
        Try

            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"

            If Not Me.objhelpDb.GetCRFLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_Check, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_Check Is Nothing Then

                dv_Check = ds_Check.Tables(0).DefaultView
                dv_Check.Sort = "iTranNo desc"

                If dv_Check.ToTable().Rows.Count > 0 Then

                    If Convert.ToString(dv_Check.ToTable().Rows(0)("cLockFlag")).Trim.ToUpper() = "L" Then
                        Me.objCommon.ShowAlert("Site is Locked.", Me.Page)
                        Me.txtproject.Text = ""
                        Me.HProjectId.Value = ""
                        Exit Sub
                    End If

                End If

            End If

            If Me.Session(S_ScopeNo) = 1 Then
                Me.fillTreeTable()
            Else
                Me.FillddlPeriod()
            End If

            If Not FillNodeDetails() Then
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Getting CRF Lock Details ", ex.Message)
        End Try

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim dt_Grid As New DataTable
        Dim dv_Grid As New DataView
        Dim dr As DataRow
        Dim Condition As String = String.Empty
        Dim ParentNodeId As Integer = 0
        Dim ds_Save As New DataSet
        Dim eStr As String = String.Empty

        Dim MedExValue_If As String = String.Empty
        Dim MedExValue_Then As String = String.Empty
        Dim index As Integer = 0

        Try

            dt_Grid = CType(Me.ViewState(VS_DtEditChecks), DataTable)

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                dt_Grid.Rows.Clear()
                dt_Grid.AcceptChanges()
                dr = dt_Grid.NewRow()
                dr("nMedExEditCheckNo") = 0
                dr("vWorkspaceId") = Me.HProjectId.Value.Trim()
                dr("iSourceNodeId_If") = CType(Me.HFSourceNodeId.Value, Integer)
                dr("vSourceMedExCode_If") = Me.HFSourceMedExCode.Value.Trim()
                dr("vOperator_If") = Me.ddlOperator.SelectedItem.Text.Trim()

                Condition = "If (" + Me.txtSourceMedEx.Text.Trim() + ")" + Me.ddlOperator.SelectedItem.Text.Trim()

                If ParentNodeId < CType(Me.HFSourceParentNodeId.Value, Integer) Then
                    ParentNodeId = CType(Me.HFSourceParentNodeId.Value, Integer)
                End If

                Condition += "("

                For index = 0 To Me.rbtnlstMedExValues_If.Items.Count - 1
                    If Me.rbtnlstMedExValues_If.Items(index).Selected Then
                        MedExValue_If = Me.rbtnlstMedExValues_If.Items(index).Text
                    End If
                Next index

                If MedExValue_If.Trim() <> "" Then
                    Condition += MedExValue_If
                    dr("vTargetValue_If") = MedExValue_If
                Else
                    If Me.chkNull.Checked = True Then
                        Condition += "NULL"
                        dr("vTargetValue_If") = "NULL"
                    Else
                        Condition += Me.txtTargetValue.Text.Trim()
                        dr("vTargetValue_If") = Me.txtTargetValue.Text.Trim()
                    End If
                End If

                If Me.txtTargetValue.Text.Trim() = "" AndAlso MedExValue_If.Trim() = "" Then
                    dr("iTargetNodeId_If") = CType(Me.HFTargetNodeId.Value, Integer)
                    dr("vTargetMedExCode_If") = Me.HFTargetMedExCode.Value.Trim()
                    If ParentNodeId < CType(Me.HFTargetParentNodeId.Value, Integer) Then
                        ParentNodeId = CType(Me.HFTargetParentNodeId.Value, Integer)
                    End If
                    Condition += Me.txtTargetMedEx.Text.Trim()
                End If

                Condition += ")"

                If Me.chkThen.Checked And Me.txtThenSourceMedEx.Text.Trim() <> "" Then

                    Condition += " And "
                    Condition += "(" + Me.txtThenSourceMedEx.Text.Trim() + ")" + Me.ddlThenOperator.SelectedItem.Text.Trim()

                    dr("iSourceNodeId_Then") = CType(Me.HFThenSourceNodeId.Value, Integer)
                    dr("vSourceMedExCode_Then") = Me.HFThenSourceMedExCode.Value.Trim()
                    dr("vOperator_Then") = Me.ddlThenOperator.SelectedItem.Text.Trim()

                    If ParentNodeId < CType(Me.HFThenSourceParentNodeId.Value, Integer) Then
                        ParentNodeId = CType(Me.HFThenSourceParentNodeId.Value, Integer)
                    End If

                    Condition += "("

                    For index = 0 To Me.rbtnlstMedExValues_Then.Items.Count - 1
                        If Me.rbtnlstMedExValues_Then.Items(index).Selected Then
                            MedExValue_Then = Me.rbtnlstMedExValues_Then.Items(index).Text
                        End If
                    Next index

                    If MedExValue_Then.Trim() <> "" Then
                        Condition += MedExValue_Then
                        dr("vTargetValue_Then") = MedExValue_Then
                    Else
                        If Me.chkThenNull.Checked = True Then
                            Condition += "Null"
                            dr("vTargetValue_Then") = "Null"
                        Else
                            Condition += Me.txtThenTargetValue.Text.Trim()
                            dr("vTargetValue_Then") = Me.txtThenTargetValue.Text.Trim()
                        End If
                    End If

                    If Me.txtThenTargetValue.Text = "" AndAlso MedExValue_Then.Trim() = "" Then
                        dr("iTargetNodeId_Then") = CType(Me.HFThenTargetNodeId.Value, Integer)
                        dr("vTargetMedExCode_Then") = Me.HFThenTargetMedExCode.Value.Trim()
                        If ParentNodeId < CType(Me.HFThenTargetParentNodeId.Value, Integer) Then
                            ParentNodeId = CType(Me.HFThenTargetParentNodeId.Value, Integer)
                        End If
                        Condition += Me.txtThenTargetMedEx.Text.Trim()
                    End If

                    Condition += ")"
                End If

                dr("iParentNodeId") = ParentNodeId
                dr("vRemarks") = Me.txtRemarks.Text.Trim()
                dr("vCondition") = Condition

                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dt_Grid.Rows.Add(dr)
                dt_Grid.AcceptChanges()

            Else

                For Each dr In dt_Grid.Rows

                    dr("vWorkspaceId") = Me.HProjectId.Value.Trim()
                    dr("iSourceNodeId_If") = CType(Me.HFSourceNodeId.Value, Integer)
                    dr("vSourceMedExCode_If") = Me.HFSourceMedExCode.Value.Trim()
                    dr("vOperator_If") = Me.ddlOperator.SelectedItem.Text.Trim()
                    Condition = "If (" + Me.txtSourceMedEx.Text.Trim() + ")" + Me.ddlOperator.SelectedItem.Text.Trim()
                    If ParentNodeId < CType(Me.HFSourceParentNodeId.Value, Integer) Then
                        ParentNodeId = CType(Me.HFSourceParentNodeId.Value, Integer)
                    End If
                    Condition += "("

                    For index = 0 To Me.rbtnlstMedExValues_If.Items.Count - 1
                        If Me.rbtnlstMedExValues_If.Items(index).Selected Then
                            MedExValue_If = Me.rbtnlstMedExValues_If.Items(index).Text
                        End If
                    Next index

                    If MedExValue_If.Trim() <> "" Then
                        Condition += MedExValue_If
                        dr("vTargetValue_If") = MedExValue_If
                    Else
                        If Me.chkNull.Checked = True Then
                            Condition += "NULL"
                            dr("vTargetValue_If") = "NULL"
                        Else
                            Condition += Me.txtTargetValue.Text.Trim()
                            dr("vTargetValue_If") = Me.txtTargetValue.Text.Trim()
                        End If
                    End If

                    If Me.txtTargetValue.Text.Trim() = "" AndAlso MedExValue_If.Trim() = "" Then
                        dr("iTargetNodeId_If") = CType(Me.HFTargetNodeId.Value, Integer)
                        dr("vTargetMedExCode_If") = Me.HFTargetMedExCode.Value.Trim()
                        If ParentNodeId < CType(Me.HFTargetParentNodeId.Value, Integer) Then
                            ParentNodeId = CType(Me.HFTargetParentNodeId.Value, Integer)
                        End If
                        Condition += Me.txtTargetMedEx.Text.Trim()
                    End If

                    Condition += ")"

                    If Me.chkThen.Checked Then

                        Condition += " And "
                        Condition += "(" + Me.txtThenSourceMedEx.Text.Trim() + ")" + Me.ddlThenOperator.SelectedItem.Text.Trim()
                        dr("iSourceNodeId_Then") = CType(Me.HFThenSourceNodeId.Value, Integer)
                        dr("vSourceMedExCode_Then") = Me.HFThenSourceMedExCode.Value.Trim()
                        dr("vOperator_Then") = Me.ddlThenOperator.SelectedItem.Text.Trim()
                        If ParentNodeId < CType(Me.HFThenSourceParentNodeId.Value, Integer) Then
                            ParentNodeId = CType(Me.HFThenSourceParentNodeId.Value, Integer)
                        End If
                        Condition += "("

                        For index = 0 To Me.rbtnlstMedExValues_Then.Items.Count - 1
                            If Me.rbtnlstMedExValues_Then.Items(index).Selected Then
                                MedExValue_Then = Me.rbtnlstMedExValues_Then.Items(index).Text
                            End If
                        Next index

                        If MedExValue_Then.Trim() <> "" Then
                            Condition += MedExValue_Then
                            dr("vTargetValue_Then") = MedExValue_Then
                        Else
                            If Me.chkThenNull.Checked = True Then
                                Condition += "NULL"
                                dr("vTargetValue_Then") = "NULL"
                            Else
                                Condition += Me.txtThenTargetValue.Text.Trim()
                                dr("vTargetValue_Then") = Me.txtThenTargetValue.Text.Trim()
                            End If
                        End If

                        If Me.txtThenTargetValue.Text = "" AndAlso MedExValue_Then.Trim() = "" Then
                            dr("iTargetNodeId_Then") = CType(Me.HFThenTargetNodeId.Value, Integer)
                            dr("vTargetMedExCode_Then") = Me.HFThenTargetMedExCode.Value.Trim()
                            If ParentNodeId < CType(Me.HFThenTargetParentNodeId.Value, Integer) Then
                                ParentNodeId = CType(Me.HFThenTargetParentNodeId.Value, Integer)
                            End If
                            Condition += Me.txtThenTargetMedEx.Text.Trim()
                        End If

                        Condition += ")"

                    End If

                    dr("iParentNodeId") = ParentNodeId
                    dr("vRemarks") = Me.txtRemarks.Text.Trim()
                    dr("vCondition") = Condition

                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                Next
                dt_Grid.AcceptChanges()

            End If

            ds_Save.Tables.Add(dt_Grid.Copy())
            ds_Save.AcceptChanges()

            If Not Me.objLambda.Save_MedExEditChecks(Me.ViewState(VS_Choice), ds_Save, _
                            Me.Session(S_UserID), eStr) Then
                Throw New Exception(eStr)
            End If

            Me.objCommon.ShowAlert("Edit Check Saved Successfully", Me.Page)

            If Not Me.FillGrid() Then
                Exit Sub
            End If

            ResetPage()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim ds_MedExCrossChecks As New DataSet
        Dim dv_MedExCrossChecks As New DataView
        Dim eStr As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try

            ds_MedExCrossChecks.Tables.Add(CType(Me.ViewState(VS_DtEditChecks), DataTable))
            ds_MedExCrossChecks.AcceptChanges()

            dv_MedExCrossChecks = ds_MedExCrossChecks.Tables(0).DefaultView
            dv_MedExCrossChecks.RowFilter = "nMedExEditCheckNo = 0 Or cStatusIndi <> 'N'"
            ds_MedExCrossChecks = Nothing
            ds_MedExCrossChecks = New DataSet
            ds_MedExCrossChecks.Tables.Add(dv_MedExCrossChecks.ToTable())
            ds_MedExCrossChecks.AcceptChanges()

            Choice = Me.ViewState(VS_Choice)
            If Not Me.objLambda.Save_MedExEditChecks(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_MedExCrossChecks, _
                            Me.Session(S_UserID), eStr) Then
                Throw New Exception(eStr)
            End If

            Me.objCommon.ShowAlert("Edit Check Saved Successfully", Me.Page)

            Me.ViewState(VS_DtEditChecks) = Nothing
            Me.GVEditChecks.DataSource = Nothing

            If Not GenCall_Data() Then
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnexit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnexit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ResetPage()
        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
        Me.btnAdd.Text = "Add"
    End Sub

#End Region

#Region "TreeView Events"

    Protected Sub trvwStructure_TreeNodeCheckChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.TreeNodeEventArgs)

        Dim MedExDesc As String = String.Empty
        Dim MedExCode As String = String.Empty
        Dim NodeId As String = String.Empty
        Dim ParentNodeId As String = String.Empty

        Dim dt_MedExDtl As New DataTable
        Dim dv_MedExDtl As New DataView
        Dim str_MedExValues() As String

        Dim MedExValue_If As String = String.Empty
        Dim MedExValue_Then As String = String.Empty
        Dim index As Integer = 0

        Try

            If e.Node.Checked Then

                MedExDesc = e.Node.Text.Substring(0, e.Node.Text.IndexOf("("))
                MedExCode = e.Node.Value.Substring(e.Node.Value.IndexOf("M-") + 2, (e.Node.Value.IndexOf("N-") - (e.Node.Value.IndexOf("M-") + 2)))
                NodeId = e.Node.Value.Substring(e.Node.Value.IndexOf("N-") + 2, e.Node.Value.IndexOf("P-") - (e.Node.Value.IndexOf("N-") + 2))
                ParentNodeId = e.Node.Value.Substring(e.Node.Value.LastIndexOf("P-") + 2, e.Node.Value.LastIndexOf("V-") - (e.Node.Value.LastIndexOf("P-") + 2))

                For index = 0 To Me.rbtnlstMedExValues_If.Items.Count - 1
                    If Me.rbtnlstMedExValues_If.Items(index).Selected Then
                        MedExValue_If = Me.rbtnlstMedExValues_If.Items(index).Text
                    End If
                Next index

                For index = 0 To Me.rbtnlstMedExValues_Then.Items.Count - 1
                    If Me.rbtnlstMedExValues_Then.Items(index).Selected Then
                        MedExValue_Then = Me.rbtnlstMedExValues_Then.Items(index).Text
                    End If
                Next index

                If Me.txtSourceMedEx.Text.Trim() = "" Then
                    Me.txtSourceMedEx.Text = MedExDesc
                    Me.HFSourceMedExCode.Value = MedExCode
                    Me.HFSourceNodeId.Value = NodeId
                    Me.HFSourceParentNodeId.Value = ParentNodeId

                    dt_MedExDtl = CType(Me.ViewState(VS_DtTree), DataTable)
                    dv_MedExDtl = dt_MedExDtl.DefaultView
                    dv_MedExDtl.RowFilter = "vMedExCode = '" + MedExCode + "' And " + _
                                    "iNodeId = " + NodeId + " And " + _
                                    "iParentNodeId = " + ParentNodeId
                    str_MedExValues = dv_MedExDtl.ToTable().Rows(0)("vMedExValues").ToString.Trim.Split(",")

                    Me.rbtnlstMedExValues_If.Items.Clear()
                    If str_MedExValues.Length > 1 Then
                        For index = 0 To str_MedExValues.Length - 1
                            Me.rbtnlstMedExValues_If.Items.Add(str_MedExValues(index).Trim())
                        Next index
                    End If

                ElseIf (Me.txtTargetMedEx.Text.Trim() = "" And Me.chkNull.Checked = False) AndAlso (Me.txtTargetValue.Text.Trim() = "" And MedExValue_If.Trim() = "") Then
                    Me.txtTargetMedEx.Text = MedExDesc
                    Me.HFTargetMedExCode.Value = MedExCode
                    Me.HFTargetNodeId.Value = NodeId
                    Me.HFTargetParentNodeId.Value = ParentNodeId

                ElseIf Me.txtThenSourceMedEx.Text.Trim() = "" Then
                    Me.txtThenSourceMedEx.Text = MedExDesc
                    Me.HFThenSourceMedExCode.Value = MedExCode
                    Me.HFThenSourceNodeId.Value = NodeId
                    Me.HFThenSourceParentNodeId.Value = ParentNodeId

                    dt_MedExDtl = CType(Me.ViewState(VS_DtTree), DataTable)
                    dv_MedExDtl = dt_MedExDtl.DefaultView
                    dv_MedExDtl.RowFilter = "vMedExCode = '" + MedExCode + "' And " + _
                                        "iNodeId = " + NodeId + " And " + _
                                        "iParentNodeId = " + ParentNodeId
                    str_MedExValues = dv_MedExDtl.ToTable().Rows(0)("vMedExValues").ToString.Trim.Split(",")

                    Me.rbtnlstMedExValues_Then.Items.Clear()
                    If str_MedExValues.Length > 1 Then
                        For index = 0 To str_MedExValues.Length - 1
                            Me.rbtnlstMedExValues_Then.Items.Add(str_MedExValues(index).Trim())
                        Next index
                    End If

                ElseIf (Me.txtThenTargetMedEx.Text.Trim() = "" And Me.chkThenNull.Checked = False) AndAlso (Me.txtThenTargetValue.Text.Trim() = "" And MedExValue_Then.Trim() = "") Then
                    Me.txtThenSourceMedEx.Text = MedExDesc
                    Me.HFThenTargetMedExCode.Value = MedExCode
                    Me.HFThenTargetNodeId.Value = NodeId
                    Me.HFThenTargetParentNodeId.Value = ParentNodeId

                End If

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

    Protected Sub trvwStructure_SelectedNodeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles trvwStructure.SelectedNodeChanged
        Try

            If Me.trvwStructure.SelectedNode.ChildNodes.Count > 0 Then
                Dim selectednode As TreeNode
                selectednode = Me.trvwStructure.SelectedNode

                If Me.HFParentNodeId.Value.Trim.ToUpper() <> selectednode.Value.Substring(selectednode.Value.LastIndexOf("(") + 1, selectednode.Value.LastIndexOf(")") - (selectednode.Value.LastIndexOf("(") + 1)) _
                Or Me.HFSourceNodeId.Value.Trim.ToUpper() <> selectednode.Value.Substring(selectednode.Value.IndexOf("(") + 1, selectednode.Value.Substring(selectednode.Value.IndexOf("(") + 1).IndexOf(")")) Then

                    Me.HFParentNodeId.Value = selectednode.Value.Substring(selectednode.Value.LastIndexOf("(") + 1, selectednode.Value.LastIndexOf(")") - (selectednode.Value.LastIndexOf("(") + 1))
                    Me.HFSourceNodeId.Value = selectednode.Value.Substring(selectednode.Value.IndexOf("(") + 1, selectednode.Value.Substring(selectednode.Value.IndexOf("(") + 1).IndexOf(")"))
                    If Not FillGrid() Then
                        Exit Sub
                    End If

                End If

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "ResetPage"

    Private Sub ResetPage()
        Me.HFSourceMedExCode.Value = ""
        Me.HFTargetMedExCode.Value = ""
        Me.HFThenSourceMedExCode.Value = ""
        Me.HFThenTargetMedExCode.Value = ""
        Me.HFSourceNodeId.Value = ""
        Me.HFSourceParentNodeId.Value = ""
        Me.HFTargetNodeId.Value = ""
        Me.HFTargetParentNodeId.Value = ""
        Me.HFThenSourceNodeId.Value = ""
        Me.HFThenSourceParentNodeId.Value = ""
        Me.HFThenTargetNodeId.Value = ""
        Me.HFThenTargetParentNodeId.Value = ""

        Me.txtSourceMedEx.Text = ""
        Me.txtTargetValue.Text = ""
        Me.txtTargetMedEx.Text = ""
        Me.txtThenSourceMedEx.Text = ""
        Me.txtThenTargetValue.Text = ""
        Me.txtThenTargetMedEx.Text = ""
        Me.chkThen.Checked = False
        Me.txtRemarks.Text = ""
        Me.chkNull.Checked = False
        Me.chkThenNull.Checked = False

        Me.btnAdd.Text = "Add"
        Me.rbtnlstMedExValues_If.Items.Clear()
        Me.rbtnlstMedExValues_Then.Items.Clear()
    End Sub

#End Region

#Region "Grid Events"

    Protected Sub GVEditChecks_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.Header Or _
                    e.Row.RowType = DataControlRowType.DataRow Or _
                    e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GVC_MedExEditCheckNo).Visible = False
            e.Row.Cells(GVC_WorkspaceId).Visible = False
            e.Row.Cells(GVC_ParentNodeId).Visible = False
            e.Row.Cells(GVC_SourceNodeId_If).Visible = False
            e.Row.Cells(GVC_SourceMedExCode_If).Visible = False
            e.Row.Cells(GVC_Operator_If).Visible = False
            e.Row.Cells(GVC_TargetNodeId_If).Visible = False
            e.Row.Cells(GVC_TargetMedExCode_If).Visible = False
            e.Row.Cells(GVC_TargetValue_If).Visible = False
            e.Row.Cells(GVC_SourceNodeId_Then).Visible = False
            e.Row.Cells(GVC_SourceMedExCode_Then).Visible = False
            e.Row.Cells(GVC_Operator_Then).Visible = False
            e.Row.Cells(GVC_TargetNodeId_Then).Visible = False
            e.Row.Cells(GVC_TargetMedExCode_Then).Visible = False
            e.Row.Cells(GVC_TargetValue_Then).Visible = False
            e.Row.Cells(GVC_Edit).Visible = False
            e.Row.Cells(GVC_StatusIndi).Visible = False
        End If
    End Sub

    Protected Sub GVEditChecks_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Dim index As Integer = e.Row.RowIndex

        If e.Row.RowType = DataControlRowType.DataRow Then

            e.Row.Cells(GVC_SrNo).Text = (index + 1).ToString()
            CType(e.Row.Cells(GVC_Delete).FindControl("imgBtnDelete"), ImageButton).CommandArgument = index
            CType(e.Row.Cells(GVC_Delete).FindControl("imgBtnDelete"), ImageButton).CommandName = "DELETE"

            CType(e.Row.Cells(GVC_Delete).FindControl("imgBtnEdit"), ImageButton).CommandArgument = index
            CType(e.Row.Cells(GVC_Delete).FindControl("imgBtnEdit"), ImageButton).CommandName = "EDIT"

            If e.Row.Cells(GVC_StatusIndi).Text.Trim.ToUpper() = "D" Then
                e.Row.Visible = False
            End If

        End If

    End Sub

    Protected Sub GVEditChecks_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim index As Integer = e.CommandArgument
        Dim dt_Grid As New DataTable
        Dim dv_Grid As New DataView
        Dim NodeValue As String = String.Empty
        Dim dr As DataRow
        Dim dt_WorkspaceNodeDetail As New DataTable
        Dim dv_WorkspaceNodeDetail As New DataView
        Dim ds_Save As New DataSet
        Dim eStr As String = String.Empty

        Try

            'dt_Grid = CType(Me.ViewState(VS_DtEditChecks), DataTable)

            dt_Grid = CType(Me.ViewState(VS_DtGrid), DataTable)

            If e.CommandName.ToUpper() = "DELETE" Then

                dv_Grid = dt_Grid.DefaultView
                dv_Grid.RowFilter = "nMedExEditCheckNo = " + Me.GVEditChecks.Rows(index).Cells(GVC_MedExEditCheckNo).Text.Trim()
                dt_Grid = Nothing
                dt_Grid = New DataTable
                dt_Grid = dv_Grid.ToTable()

                'Me.ViewState(VS_MedExEditCheckNo) = Me.GVEditChecks.Rows(index).Cells(GVC_MedExEditCheckNo).Text.Trim()
                For Each dr In dt_Grid.Rows

                    dr("cStatusIndi") = "D"
                    dt_Grid.AcceptChanges()
                    'If dr(GVC_MedExEditCheckNo) = CType(Me.ViewState(VS_MedExEditCheckNo), Integer) Then
                    'dt_Grid.Rows(index)("cStatusIndi") = "D"
                    'If dt_Grid.Rows(index)(GVC_MedExEditCheckNo) = 0 Then
                    '    dt_Grid.Rows(index).Delete()
                    'End If
                    'dt_Grid.AcceptChanges()
                    'End If

                Next dr

                ds_Save.Tables.Add(dt_Grid.Copy())
                ds_Save.AcceptChanges()

                If Not Me.objLambda.Save_MedExEditChecks(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, ds_Save, _
                            Me.Session(S_UserID), eStr) Then
                    Throw New Exception(eStr)
                End If

                Me.objCommon.ShowAlert("Edit Check Deleted Successfully", Me.Page)
                'Me.ResetPage() : To resolve the problem of fill grid after deleting
                If Not Me.FillGrid() Then
                    Exit Sub
                End If
                'Me.ViewState(VS_DtEditChecks) = dt_Grid
                'BindGrid(dt_Grid)
                'Me.GVEditChecks.DataSource = dt_Grid
                'Me.GVEditChecks.DataBind()

            ElseIf e.CommandName.ToUpper() = "EDIT" Then

                ResetPage()
                Me.btnAdd.Text = "Update"
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

                Me.ViewState(VS_MedExEditCheckNo) = Me.GVEditChecks.Rows(index).Cells(GVC_MedExEditCheckNo).Text.Trim()

                dv_Grid = dt_Grid.DefaultView
                dv_Grid.RowFilter = "nMedExEditCheckNo = " + CType(Me.ViewState(VS_MedExEditCheckNo), String).Trim()
                dt_Grid = Nothing
                dt_Grid = New DataTable
                dt_Grid = dv_Grid.ToTable()
                index = 0
                Me.HFParentNodeId.Value = dt_Grid.Rows(index)(GVC_ParentNodeId).ToString()
                Me.HFSourceNodeId.Value = dt_Grid.Rows(index)(GVC_SourceNodeId_If).ToString()
                Me.HFSourceMedExCode.Value = dt_Grid.Rows(index)(GVC_SourceMedExCode_If).ToString()
                'Me.ddlOperator.Items.FindByText(dt_Grid.Rows(index)(GVC_Operator_If).ToString()).Selected = True
                Me.ddlOperator.SelectedItem.Text = dt_Grid.Rows(index)(GVC_Operator_If).ToString()

                Me.HFTargetNodeId.Value = dt_Grid.Rows(index)(GVC_TargetNodeId_If).ToString()
                Me.HFTargetMedExCode.Value = dt_Grid.Rows(index)(GVC_TargetMedExCode_If).ToString()
                Me.txtTargetValue.Text = dt_Grid.Rows(index)(GVC_TargetValue_If).ToString()

                Me.HFThenSourceNodeId.Value = dt_Grid.Rows(index)(GVC_SourceNodeId_Then).ToString()
                Me.HFThenSourceMedExCode.Value = dt_Grid.Rows(index)(GVC_SourceMedExCode_Then).ToString()
                'Me.ddlThenOperator.Items.FindByText(dt_Grid.Rows(index)(GVC_Operator_Then).ToString()).Selected = True
                Me.ddlThenOperator.SelectedItem.Text = dt_Grid.Rows(index)(GVC_Operator_Then).ToString()

                Me.HFThenTargetNodeId.Value = dt_Grid.Rows(index)(GVC_TargetNodeId_Then).ToString()
                Me.HFThenTargetMedExCode.Value = dt_Grid.Rows(index)(GVC_TargetMedExCode_Then).ToString()
                Me.txtThenTargetValue.Text = dt_Grid.Rows(index)(GVC_TargetValue_Then).ToString()

                dt_WorkspaceNodeDetail = CType(Me.ViewState(VS_DtWorkspaceNodeDetail), DataTable)
                dv_WorkspaceNodeDetail = New DataView
                dv_WorkspaceNodeDetail = dt_WorkspaceNodeDetail.DefaultView
                dv_WorkspaceNodeDetail.RowFilter = "iNodeId = " + HFSourceNodeId.Value
                Me.HFSourceParentNodeId.Value = 0
                If dv_WorkspaceNodeDetail.ToTable().Rows.Count > 0 Then
                    Me.HFSourceParentNodeId.Value = dv_WorkspaceNodeDetail.ToTable().Rows(0)("iParentNodeId").ToString()
                End If

                dv_WorkspaceNodeDetail = dt_WorkspaceNodeDetail.DefaultView
                dv_WorkspaceNodeDetail.RowFilter = "iNodeId = " + HFTargetNodeId.Value
                Me.HFTargetParentNodeId.Value = 0
                If dv_WorkspaceNodeDetail.ToTable().Rows.Count > 0 Then
                    Me.HFTargetParentNodeId.Value = dv_WorkspaceNodeDetail.ToTable().Rows(0)("iParentNodeId").ToString()
                End If

                dv_WorkspaceNodeDetail = dt_WorkspaceNodeDetail.DefaultView
                dv_WorkspaceNodeDetail.RowFilter = "iNodeId = " + HFThenSourceNodeId.Value
                Me.HFThenSourceParentNodeId.Value = 0
                If dv_WorkspaceNodeDetail.ToTable().Rows.Count > 0 Then
                    Me.HFThenSourceParentNodeId.Value = dv_WorkspaceNodeDetail.ToTable().Rows(0)("iParentNodeId").ToString()
                End If

                dv_WorkspaceNodeDetail = dt_WorkspaceNodeDetail.DefaultView
                dv_WorkspaceNodeDetail.RowFilter = "iNodeId = " + HFThenTargetNodeId.Value
                Me.HFThenTargetParentNodeId.Value = 0
                If dv_WorkspaceNodeDetail.ToTable().Rows.Count > 0 Then
                    Me.HFThenTargetParentNodeId.Value = dv_WorkspaceNodeDetail.ToTable().Rows(0)("iParentNodeId").ToString()
                End If

                'Me.HFSourceParentNodeId.Value = ""
                'Me.HFTargetParentNodeId.Value = ""
                'Me.HFThenSourceParentNodeId.Value = ""
                'Me.HFThenTargetParentNodeId.Value = ""

                NodeValue = Me.HFSourceMedExCode.Value.Trim() + "(" + Me.HFSourceNodeId.Value.Trim()
                NodeValue += ")(" + Me.HFParentNodeId.Value.Trim() + ")"

                Me.txtSourceMedEx.Text = SearchNodeName(NodeValue)

                If Me.HFTargetMedExCode.Value.Trim() <> "" Then
                    NodeValue = Me.HFTargetMedExCode.Value.Trim() + "(" + Me.HFTargetNodeId.Value.Trim()
                    NodeValue += ")(" + Me.HFParentNodeId.Value.Trim() + ")"
                    Me.txtTargetMedEx.Text = SearchNodeName(NodeValue) 'Me.trvwStructure.FindNode(NodeValue).Text
                End If

                Me.chkThen.Checked = False
                If Me.HFThenSourceMedExCode.Value.Trim() <> "" Then
                    Me.chkThen.Checked = True
                    NodeValue = Me.HFThenSourceMedExCode.Value.Trim() + "(" + Me.HFThenSourceNodeId.Value.Trim()
                    NodeValue += ")(" + Me.HFParentNodeId.Value.Trim() + ")"
                    Me.txtThenSourceMedEx.Text = SearchNodeName(NodeValue) 'Me.trvwStructure.FindNode(NodeValue).Text
                    If Me.HFThenTargetMedExCode.Value.Trim() <> "" Then
                        NodeValue = Me.HFThenTargetMedExCode.Value.Trim() + "(" + Me.HFThenTargetNodeId.Value.Trim()
                        NodeValue += ")(" + Me.HFParentNodeId.Value.Trim() + ")"
                        Me.txtThenTargetMedEx.Text = SearchNodeName(NodeValue) 'Me.trvwStructure.FindNode(NodeValue).Text
                    End If
                End If

                Me.txtRemarks.Text = dt_Grid.Rows(index)(GVC_Remarks).ToString()

                Me.ViewState(VS_DtEditChecks) = dt_Grid.Copy()
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

    Protected Sub GVEditChecks_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)

    End Sub

    Protected Sub GVEditChecks_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GVEditChecks.RowEditing
        e.Cancel = True
    End Sub

#End Region

#Region "SearchNodeName"

    Private Function SearchNodeName(ByVal NodeValue As String) As String
        For Each node As TreeNode In Me.trvwStructure.Nodes
            If node.ChildNodes.Count > 0 Then
                For Each childNode As TreeNode In node.ChildNodes

                    If childNode.ChildNodes.Count > 0 Then

                        For Each LastNode As TreeNode In childNode.ChildNodes

                            If LastNode.Value = NodeValue Then
                                Return LastNode.Text.Substring(0, LastNode.Text.IndexOf("("))
                            End If

                        Next LastNode

                    End If

                    'If childNode.Value = NodeValue Then
                    '    Return childNode.Text.Trim()
                    'End If
                Next childNode
            End If
        Next node
        Return ""
    End Function

#End Region

#Region "Drop Down List Events"

    Protected Sub ddlActivity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlActivity.SelectedIndexChanged
        Try
            fillTreeTableforBABE()

        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)
        End Try
    End Sub

    Protected Sub ddlPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPeriod.SelectedIndexChanged
        FillActivity()
    End Sub

#End Region

End Class
