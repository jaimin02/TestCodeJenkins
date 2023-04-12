
Partial Class frmMedExDependency
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objCommon As New clsCommon
    Private objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()


    Private Const VS_Choice As String = "Choice"
    Private Const VS_DsDependency As String = "DsDependency"
    Private Const VS_DsActDependency As String = "DsActDependency"
    Private Const VS_DsAttDependency As String = "DsAttDependency"
    Private Const VS_Medex As String = "Medex"

    Private Const GVCDependency_SrNo As Integer = 0
    Private Const GVCDependency_nMedExDependcyNo As Integer = 1
    Private Const GVCDependency_SourceMedexDesc As Integer = 2
    Private Const GVCDependency_TargetMedexDesc As Integer = 3
    Private Const GVCDependency_vMedExValue As Integer = 4
    Private Const GVCDependency_SourceActivityDesc As Integer = 5
    Private Const GVCDependency_TargetActivityDesc As Integer = 6
    Private Const GVCDependency_cDependencyType As Integer = 7
    Private Const GVCDependency_DependencyType As Integer = 8
    Private Const GVCDependency_iModifyBy As Integer = 9
    Private Const GVCDependency_dModifyOn As Integer = 10
    Private Const GVCDependency_Delete As Integer = 11

#End Region

#Region "Page Load"
 
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            GenCall()
            Exit Sub

        End If
    End Sub

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean

        Try

            Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            If Not GenCall_ShowUI() Then
                Exit Function
            End If
            GenCall = True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...GenCall")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_ShowUI"

    Private Function GenCall_ShowUI() As Boolean
        Dim sender As Object
        Dim e As EventArgs


        Try
            CType(Master.FindControl("lblHeading"), Label).Text = "Attribute/Activity Dependency"

            Page.Title = ":: Attribute/Activity Dependency :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
            End If
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID) & " And cWorkspaceType='P'"

            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                Me.ddlVisit.Style.Add("display", "")
                Me.lblPeriod.Text = "Select Visit* :"

            ElseIf Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                Me.ddlPeriod.Style.Add("display", "")
                Me.lblPeriod.Text = "Select Period* :"

            End If
            GenCall_ShowUI = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
            Return False
        End Try
    End Function

#End Region

#Region "Fill Dropdown"

    Private Function FillddlPeriod() As Boolean
        Dim ds_Period As DataSet = Nothing
        Dim eStr As String = String.Empty

        Try
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                Me.ddlPeriod.Items.Insert(0, "1")
                ds_Period = Me.objhelpDb.GetResultSet("select iNodeId,vNodeDisplayName from WorkspaceNodeDetail where vWorkspaceId='" + Me.HProjectId.Value + _
                                                      "' And iParentNodeId = 1 And cStatusIndi<>'D' AND  ISNULL(vTemplateId,'') <> '0001' ORDER BY iNodeno asc", "WorkspaceNodeDetail")
                Me.ddlVisit.DataSource = ds_Period.Tables(0)
                Me.ddlVisit.DataValueField = "iNodeId"
                Me.ddlVisit.DataTextField = "vNodeDisplayName"
                Me.ddlVisit.DataBind()
                Me.ddlTargetVisit.DataSource = ds_Period.Tables(0)
                Me.ddlTargetVisit.DataValueField = "iNodeId"
                Me.ddlTargetVisit.DataTextField = "vNodeDisplayName"
                Me.ddlTargetVisit.DataBind()


                For iMedexGroup As Integer = 0 To ddlVisit.Items.Count - 1
                    ddlVisit.Items(iMedexGroup).Attributes.Add("title", ds_Period.Tables(0).Rows(iMedexGroup)("vNodeDisplayName"))
                    ddlVisit.Items(iMedexGroup).Attributes.Add("data", ds_Period.Tables(0).Rows(iMedexGroup)("vNodeDisplayName"))
                    ddlTargetVisit.Items(iMedexGroup).Attributes.Add("title", ds_Period.Tables(0).Rows(iMedexGroup)("vNodeDisplayName"))
                    ddlTargetVisit.Items(iMedexGroup).Attributes.Add("data", ds_Period.Tables(0).Rows(iMedexGroup)("vNodeDisplayName"))
                Next iMedexGroup

                Me.ddlVisit.Items.Insert(0, New ListItem("Select Visit/Parent Activity", 0))
                Me.ddlTargetVisit.Items.Insert(0, New ListItem("Select Visit/Parent Activity", 0))
            ElseIf Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                ds_Period = Me.objhelpDb.GetResultSet("select distinct iPeriod from WorkspaceNodeDetail where vWorkspaceId='" + Me.HProjectId.Value + "'And cStatusIndi<>'D'  AND  ISNULL(vTemplateId,'') <> '0001' ", "WorkspaceNodeDetail")
                Me.ddlPeriod.DataSource = ds_Period.Tables(0)
                Me.ddlPeriod.DataValueField = "iPeriod"
                Me.ddlPeriod.DataTextField = "iPeriod"
                Me.ddlPeriod.DataBind()

                For iMedexGroup As Integer = 0 To ddlPeriod.Items.Count - 1
                    ddlPeriod.Items(iMedexGroup).Attributes.Add("title", ds_Period.Tables(0).Rows(iMedexGroup)("iPeriod"))
                Next iMedexGroup
                Me.ddlPeriod.Items.Insert(0, New ListItem("Select Period", 0))
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillddlPeriod")
            Return False
        End Try
    End Function

    Private Function FillddlActivity() As Boolean
        Dim ds_WorkSpaceNodeDetail As New DataSet
        Dim dt_FilterNodename As New DataTable
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        'Dim dr_Row As DataRow

        Try
            Me.ddlActivity.Items.Clear()
            wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' And cStatusIndi <> 'D'"
            If Me.Session(S_ScopeNo).ToString <> Scope_ClinicalTrial Then
                wstr += " And iPeriod='" & Convert.ToString(ddlPeriod.SelectedValue.Trim) & "' "
            ElseIf Me.Session(S_ScopeNo).ToString = Scope_ClinicalTrial Then
                wstr += " And iParentNodeId='" & Convert.ToString(ddlVisit.SelectedValue.Trim) & "' "
            End If
            wstr += " order by  iParentNodeId,iNodeNo "

            If Not objhelpDb.GetViewWorkSpaceNodeDetail(wstr, ds_WorkSpaceNodeDetail, estr) Then
                Me.objCommon.ShowAlert("Error While Getting Data From View_WorkSpaceNodeDetail:" + estr, Me.Page)
                Return False
            End If

            If Me.Session(S_ScopeNo).ToString <> Scope_ClinicalTrial Then
                ds_WorkSpaceNodeDetail.Tables(0).Columns.Add("vNodeDetails", System.Type.GetType("System.String"))
                For Each dr As DataRow In ds_WorkSpaceNodeDetail.Tables(0).Rows
                    dr("vNodeDetails") = dr("iNodeId").ToString() + "##" + dr("vActivityId").ToString()
                    ds_WorkSpaceNodeDetail.Tables(0).AcceptChanges()
                Next
                Me.ddlActivity.DataSource = ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable()
                Me.ddlActivity.DataValueField = "vNodeDetails"
                Me.ddlActivity.DataTextField = "vNodeDisplayName"
            Else
                ds_WorkSpaceNodeDetail.Tables(0).Columns.Add("vNodeDetails", System.Type.GetType("System.String"))
                For Each dr As DataRow In ds_WorkSpaceNodeDetail.Tables(0).Rows
                    dr("vNodeDetails") = dr("iNodeId").ToString() + "##" + dr("vActivityId").ToString()
                    ds_WorkSpaceNodeDetail.Tables(0).AcceptChanges()
                Next
                Me.ddlActivity.DataSource = ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable()
                Me.ddlActivity.DataValueField = "vNodeDetails"
                Me.ddlActivity.DataTextField = "vNodeDisplayName"
            End If

            Me.ddlActivity.DataBind()

            For iMedexGroup As Integer = 0 To ddlActivity.Items.Count - 1
                ddlActivity.Items(iMedexGroup).Attributes.Add("title", ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable().Rows(iMedexGroup)("ActivityDisplayName") + "(" + ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable().Rows(iMedexGroup)("vActivityId") + ")")
            Next iMedexGroup

            Me.ddlActivity.Items.Insert(0, New ListItem("Select Activity", 0))
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......FillddlActivity")
            Return False
        End Try

    End Function

    'Private Function FillddlActActivty() As Boolean
    '    Dim ds_WorkSpaceNodeDetail As New DataSet
    '    Dim dt_FilterNodename As New DataTable
    '    Dim estr As String = String.Empty
    '    Dim wstr As String = String.Empty

    '    Try
    '        Me.ddlActActivty.Items.Clear()
    '        wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' And cStatusIndi <> 'D'"
    '        If Me.Session(S_ScopeNo).ToString <> Scope_ClinicalTrial Then
    '            wstr += " And iPeriod='" & Convert.ToString(ddlPeriod.SelectedValue.Trim) & "' "
    '        ElseIf Me.Session(S_ScopeNo).ToString = Scope_ClinicalTrial Then
    '            wstr += " And iParentNodeId='" & Convert.ToString(ddlVisit.SelectedValue.Trim) & "' "
    '        End If
    '        wstr += " order by  iParentNodeId,iNodeNo "

    '        If Not objhelpDb.GetViewWorkSpaceNodeDetail(wstr, ds_WorkSpaceNodeDetail, estr) Then
    '            Me.objCommon.ShowAlert("Error While Getting Data From View_WorkSpaceNodeDetail:" + estr, Me.Page)
    '            Return False
    '        End If

    '        'If Me.Session(S_ScopeNo).ToString <> Scope_ClinicalTrial Then
    '        ds_WorkSpaceNodeDetail.Tables(0).Columns.Add("vNodeDetails", System.Type.GetType("System.String"))
    '        For Each dr As DataRow In ds_WorkSpaceNodeDetail.Tables(0).Rows
    '            dr("vNodeDetails") = dr("iNodeId").ToString() + "##" + dr("vActivityId").ToString()
    '            ds_WorkSpaceNodeDetail.Tables(0).AcceptChanges()
    '        Next
    '        ds_WorkSpaceNodeDetail.Tables(0).DefaultView.RowFilter = "iNodeId <> '" + Me.ddlActivity.SelectedValue.Split("##")(0).Trim.ToString() + "'"
    '        dt_FilterNodename = ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable()
    '        dt_FilterNodename.DefaultView.Sort = "vNodeDisplayName"
    '        Me.ddlActActivty.DataSource = dt_FilterNodename.DefaultView.ToTable()
    '        Me.ddlActActivty.DataValueField = "vNodeDetails"
    '        Me.ddlActActivty.DataTextField = "vNodeDisplayName"
    '        'Else
    '        '    dt_FilterNodename = ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable(True, "vNodeName", "vActivityId")
    '        '    dt_FilterNodename.DefaultView.Sort = "vNodeName"
    '        '    Me.ddlActivity.DataSource = dt_FilterNodename.DefaultView.ToTable()
    '        '    Me.ddlActivity.DataValueField = "vActivityId"
    '        '    Me.ddlActivity.DataTextField = "vNodeName"
    '        'End If

    '        Me.ddlActActivty.DataBind()

    '        For iMedexGroup As Integer = 0 To ddlActActivty.Items.Count - 1
    '            ddlActActivty.Items(iMedexGroup).Attributes.Add("title", dt_FilterNodename.DefaultView.ToTable().Rows(iMedexGroup)("ActivityDisplayName") + "(" + dt_FilterNodename.DefaultView.ToTable().Rows(iMedexGroup)("vActivityId") + ")")
    '        Next iMedexGroup
    '        Me.ddlActActivty.Items.Insert(0, New ListItem("Select Activity", 0))


    '        Return True
    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.Message, ".....FillddlActActivty")
    '        Return False
    '    End Try
    'End Function

    Private Function FillddlActAttribute() As Boolean
        Dim ds_Attribute As DataSet = Nothing
        Dim Wstr As String = String.Empty

        Try
            Me.ddlActAttribute.Items.Clear()

            Wstr = "SELECT vMedexCode,vMedexDesc,iNodeid,vMedexType,iNodeNo,iSeqNo,vmedexsubGroupDesc,vmedexgroupDesc FROM VIEW_MedExWorkspaceHdr WHERE vWorkspaceid ='" + Me.HProjectId.Value.Trim() + "'AND iPeriod ='" + Me.ddlPeriod.SelectedValue.Trim.ToString() + "' AND iNodeid = '" + Me.ddlActivity.SelectedValue.Split("##")(0).Trim.ToString() + "' AND vMedexType <> 'File' AND vMedexType <> 'Import' AND cStatusIndi <> 'D' AND cActiveFlag <> 'N' order by iNodeNo,iSeqNo"

            ds_Attribute = objhelpDb.GetResultSet(Wstr, "VIEW_MedExWorkspaceHdr")

            If Not ds_Attribute Is Nothing Then
                If ds_Attribute.Tables(0).Rows.Count > 0 Then
                    'ds_Attribute.Tables(0).DefaultView.Sort = "vMedexDesc"
                    Me.ddlActAttribute.DataSource = ds_Attribute.Tables(0).DefaultView.ToTable()
                    Me.ddlActAttribute.DataValueField = "vMedexCode"
                    Me.ddlActAttribute.DataTextField = "vMedexDesc"
                    Me.ddlActAttribute.DataBind()

                    For iMedexGroup As Integer = 0 To ddlActAttribute.Items.Count - 1
                        ddlActAttribute.Items(iMedexGroup).Attributes.Add("title", ds_Attribute.Tables(0).Rows(iMedexGroup)("vMedexDesc") + "(" + ds_Attribute.Tables(0).Rows(iMedexGroup)("vmedexgroupDesc") + ")" + "[" + ds_Attribute.Tables(0).Rows(iMedexGroup)("vmedexsubGroupDesc") + "]")
                    Next iMedexGroup

                    Me.ddlActAttribute.Items.Insert(0, New ListItem("Select Attribute", 0))
                End If
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillddlActAttribute")
            Return False
        End Try
    End Function

    Private Function FillddlAttAttribute() As Boolean
        Dim ds_Attribute As DataSet = Nothing
        Dim Wstr As String = String.Empty

        Try
            Me.ddlAttAttribute.Items.Clear()

            Wstr = "SELECT vMedexCode,vMedexDesc,iNodeid,vMedexType,iNodeNo,iSeqNo,vmedexsubGroupDesc,vmedexgroupDesc FROM VIEW_MedExWorkspaceHdr WHERE vWorkspaceid ='" + Me.HProjectId.Value.Trim() + "'AND iPeriod ='" + Me.ddlPeriod.SelectedValue.Trim.ToString() + "' AND iNodeid = '" + Me.ddlActivity.SelectedValue.Split("##")(0).Trim.ToString() + "' AND vMedexType <> 'File' AND vMedexType <> 'Import' AND vMedexType <> 'Label' AND cStatusIndi <> 'D' AND cActiveFlag <> 'N' order by iNodeNo,iSeqNo"

            ds_Attribute = objhelpDb.GetResultSet(Wstr, "VIEW_MedExWorkspaceHdr")

            If Not ds_Attribute Is Nothing Then
                If ds_Attribute.Tables(0).Rows.Count > 0 Then
                    'ds_Attribute.Tables(0).DefaultView.Sort = "vMedexDesc"
                    Me.ddlAttAttribute.DataSource = ds_Attribute.Tables(0).DefaultView.ToTable()
                    Me.ddlAttAttribute.DataValueField = "vMedexCode"
                    Me.ddlAttAttribute.DataTextField = "vMedexDesc"
                    Me.ddlAttAttribute.DataBind()

                    For iMedexGroup As Integer = 0 To ddlAttAttribute.Items.Count - 1
                        ddlAttAttribute.Items(iMedexGroup).Attributes.Add("title", ds_Attribute.Tables(0).Rows(iMedexGroup)("vMedexDesc") + "(" + ds_Attribute.Tables(0).Rows(iMedexGroup)("vmedexgroupDesc") + ")" + "[" + ds_Attribute.Tables(0).Rows(iMedexGroup)("vmedexsubGroupDesc") + "]")
                    Next iMedexGroup

                    Me.ddlAttAttribute.Items.Insert(0, New ListItem("Select Attribute", 0))
                End If
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillddlAttAttribute")
            Return False
        End Try
    End Function

    Private Function FillChkAttTrgAttribute() As Boolean
        Dim ds_Attribute As DataSet = Nothing
        Dim dt_SortAttribute As New DataTable
        Dim Wstr As String = String.Empty

        Try
            Me.ChkAttTrgAttribute.Items.Clear()

            Wstr = "SELECT vMedexCode,vMedexDesc,iNodeid,vMedexType,iNodeNo,iSeqNo,vmedexsubGroupDesc,vmedexgroupDesc FROM VIEW_MedExWorkspaceHdr WHERE vWorkspaceid ='" + Me.HProjectId.Value.Trim() + "'AND iPeriod ='" + Me.ddlPeriod.SelectedValue.Trim.ToString() + "' AND iNodeid = '" + Me.ddlTargetActivity.SelectedValue.Split("##")(0).Trim.ToString() + "' AND vMedexType <> 'File' AND vMedexType <> 'Import' AND vMedexType <> 'Label' AND cStatusIndi <> 'D' AND cActiveFlag <> 'N' order by iNodeNo,iSeqNo"

            ds_Attribute = objhelpDb.GetResultSet(Wstr, "VIEW_MedExWorkspaceHdr")

            If Not ds_Attribute Is Nothing Then
                If ds_Attribute.Tables(0).Rows.Count > 0 Then
                    ds_Attribute.Tables(0).DefaultView.RowFilter = "vMedexCode <> '" + Me.ddlAttAttribute.SelectedValue.Trim.ToString() + "'"
                    dt_SortAttribute = ds_Attribute.Tables(0).DefaultView.ToTable()
                    'dt_SortAttribute.DefaultView.Sort = "vMedexDesc"
                    Me.ChkAttTrgAttribute.DataSource = dt_SortAttribute.DefaultView.ToTable()
                    Me.ChkAttTrgAttribute.DataValueField = "vMedexCode"
                    Me.ChkAttTrgAttribute.DataTextField = "vMedexDesc"
                    Me.ChkAttTrgAttribute.DataBind()
                    For iMedexGroup As Integer = 0 To ChkAttTrgAttribute.Items.Count - 1
                        ChkAttTrgAttribute.Items(iMedexGroup).Attributes.Add("title", dt_SortAttribute.Rows(iMedexGroup)("vMedexDesc") + "(" + dt_SortAttribute.Rows(iMedexGroup)("vmedexgroupDesc") + ")" + "[" + dt_SortAttribute.Rows(iMedexGroup)("vmedexsubGroupDesc") + "]")
                    Next iMedexGroup
                    Me.FldTrgAttribute.Style.Add("display", "")
                    Me.TrTrgAttribute.Style.Add("display", "")
                End If
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillChkAttTrgAttribute")
            Return False
        End Try
    End Function


    Private Function FillddlTargetActivity() As Boolean
        Dim ds_WorkSpaceNodeDetail As New DataSet
        Dim dt_FilterNodename As New DataTable
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        'Dim dr_Row As DataRow

        Try
            Me.ddlTargetActivity.Items.Clear()
            wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' And cStatusIndi <> 'D'"
            If Me.Session(S_ScopeNo).ToString <> Scope_ClinicalTrial Then
                wstr += " And iPeriod='" & Convert.ToString(ddlPeriod.SelectedValue.Trim) & "' "
            ElseIf Me.Session(S_ScopeNo).ToString = Scope_ClinicalTrial Then
                wstr += " And iParentNodeId='" & Convert.ToString(ddlTargetVisit.SelectedValue.Trim) & "' "
            End If

            'wstr += " And vActivityId <> '" & Convert.ToString(ddlActivity.SelectedValue.Trim()).Split("##")(2) & "' "

            wstr += " order by  iParentNodeId,iNodeNo "


            If Not objhelpDb.GetViewWorkSpaceNodeDetail(wstr, ds_WorkSpaceNodeDetail, estr) Then
                Me.objCommon.ShowAlert("Error While Getting Data From View_WorkSpaceNodeDetail:" + estr, Me.Page)
                Return False
            End If

            If Me.Session(S_ScopeNo).ToString <> Scope_ClinicalTrial Then
                ds_WorkSpaceNodeDetail.Tables(0).Columns.Add("vNodeDetails", System.Type.GetType("System.String"))
                For Each dr As DataRow In ds_WorkSpaceNodeDetail.Tables(0).Rows
                    dr("vNodeDetails") = dr("iNodeId").ToString() + "##" + dr("vActivityId").ToString()
                    ds_WorkSpaceNodeDetail.Tables(0).AcceptChanges()
                Next
                Me.ddlTargetActivity.DataSource = ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable()
                Me.ddlTargetActivity.DataValueField = "vNodeDetails"
                Me.ddlTargetActivity.DataTextField = "vNodeDisplayName"
            Else
                ds_WorkSpaceNodeDetail.Tables(0).Columns.Add("vNodeDetails", System.Type.GetType("System.String"))
                For Each dr As DataRow In ds_WorkSpaceNodeDetail.Tables(0).Rows
                    dr("vNodeDetails") = dr("iNodeId").ToString() + "##" + dr("vActivityId").ToString()
                    ds_WorkSpaceNodeDetail.Tables(0).AcceptChanges()
                Next
                Me.ddlTargetActivity.DataSource = ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable()
                Me.ddlTargetActivity.DataValueField = "vNodeDetails"
                Me.ddlTargetActivity.DataTextField = "vNodeDisplayName"
            End If

            Me.ddlTargetActivity.DataBind()

            For iMedexGroup As Integer = 0 To ddlTargetActivity.Items.Count - 1
                ddlTargetActivity.Items(iMedexGroup).Attributes.Add("title", ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable().Rows(iMedexGroup)("ActivityDisplayName") + "(" + ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable().Rows(iMedexGroup)("vActivityId") + ")")
            Next iMedexGroup

            Me.ddlTargetActivity.Items.Insert(0, New ListItem("Select Activity", 0))
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......FillddlTargetActivity")
            Return False
        End Try

    End Function



#End Region

#Region "Rbl Index Change"

    Protected Sub rblDependency_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblDependency.SelectedIndexChanged

        Try
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                ddlTargetVisit.SelectedIndex = 0
            End If
            If Convert.ToString(ddlActAttribute.SelectedItem) = Convert.ToString(ddlTargetActivity.SelectedItem) Then
                'fldAttachedActivities.Style.Add("display", "")
            Else
                fldAttachedActivities.Style.Add("display", "none")
            End If
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                If Me.ddlVisit.SelectedValue = 0 Then
                    Me.rblDependency.ClearSelection()
                    objCommon.ShowAlert("Please Select Visit/Parent Activity", Me.Page)
                    Exit Sub
                End If
            ElseIf Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                If Me.ddlPeriod.SelectedValue = 0 Then
                    Me.rblDependency.ClearSelection()
                    objCommon.ShowAlert("Please Select Period", Me.Page)
                    Exit Sub
                End If
            End If
            If Me.ddlActivity.SelectedValue.Trim.ToString() = "0" Then
                Me.rblDependency.ClearSelection()
                objCommon.ShowAlert("Please Select Activity", Me.Page)
                Exit Sub
            End If

            If rblDependency.SelectedValue = "A" Then
                Me.fldsActivity.Style.Add("display", "")
                Me.fldsAttribute.Style.Add("display", "none")
                Me.fldsTargetActivity.Style.Add("display", "")

                If Not FillddlActAttribute() Then
                    Exit Sub
                End If

                'If Not FillddlActActivty() Then
                '    Exit Sub
                'End If
                Me.trAct.Visible = False
                Me.txtAct.Visible = False
                Me.ChkAtt.Visible = False
                Me.FldAtt.Style.Add("display", "none")
            ElseIf rblDependency.SelectedValue = "F" Then
                Me.fldsAttribute.Style.Add("display", "")
                Me.fldsActivity.Style.Add("display", "none")
                Me.fldsTargetActivity.Style.Add("display", "")
                If Not FillddlAttAttribute() Then
                    Exit Sub
                End If

                Me.trAtt.Visible = False
                Me.txtAtt.Visible = False
                Me.ChkAtt.Visible = False
                Me.FldAtt.Style.Add("display", "none")
            End If
            ddlTargetActivity.Items.Clear()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..rblDependency_SelectedIndexChanged")
        End Try
    End Sub

#End Region

#Region "DropDown IndexChange"

    Protected Sub ddlVisit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlVisit.SelectedIndexChanged

        Me.rblDependency.ClearSelection()
        Me.ddlActAttribute.SelectedIndex = -1
        Me.ChkAttTrgAttribute.Items.Clear()
        Me.FldTrgAttribute.Style.Add("display", "none")
        Me.TrTrgAttribute.Style.Add("display", "none")

        Me.fldsActivity.Style.Add("display", "none")
        Me.fldsAttribute.Style.Add("display", "none")

        Me.fldsTargetActivity.Style.Add("display", "none")
        Me.FldTrgAttribute.Style.Add("display", "none")


        If ddlVisit.SelectedValue <> 0 Then
            If Not FillddlActivity() Then
                Exit Sub
            End If
            '' Add by shivani pandya for lock impact
            If hndLockStatus.Value.Trim() = "Lock" Then
                Me.trRbl.Style.Add("display", "none")
            Else
                Me.trRbl.Style.Add("display", "")
            End If
        Else
            Me.ddlActivity.Items.Clear()
            Me.ddlActivity.Items.Insert(0, New ListItem("Select Activity", 0))
            Me.rblDependency.ClearSelection()
            Me.trRbl.Style.Add("display", "none")
        End If
    End Sub

    Protected Sub ddlPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPeriod.SelectedIndexChanged
        If Me.ddlPeriod.SelectedValue <> 0 Then
            If Not FillddlActivity() Then
                Exit Sub
            End If
            '' Add by shivani pandya for lock impact
            If hndLockStatus.Value.Trim() = "Lock" Then
                Me.trRbl.Style.Add("display", "none")
            Else
                Me.trRbl.Style.Add("display", "")
            End If
        Else
            Me.ddlActivity.Items.Clear()
            Me.ddlActivity.Items.Insert(0, New ListItem("Select Activity", 0))
            Me.rblDependency.ClearSelection()
            Me.trRbl.Style.Add("display", "none")
        End If
    End Sub

    Protected Sub ddlActivity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlActivity.SelectedIndexChanged

        Me.rblDependency.ClearSelection()
        Me.ddlActAttribute.SelectedIndex = -1
        Me.ChkAttTrgAttribute.Items.Clear()
        Me.FldTrgAttribute.Style.Add("display", "none")
        Me.TrTrgAttribute.Style.Add("display", "none")

        Me.fldsActivity.Style.Add("display", "none")
        Me.fldsAttribute.Style.Add("display", "none")

        Me.fldsTargetActivity.Style.Add("display", "none")
        Me.FldTrgAttribute.Style.Add("display", "none")

        If Convert.ToString(ddlActivity.SelectedItem) = Convert.ToString(ddlTargetActivity.SelectedItem) Then
            fldAttachedActivities.Style.Add("display", "")
        Else
            fldAttachedActivities.Style.Add("display", "none")

        End If

        If ddlActivity.SelectedValue = "0" Then
            Me.rblDependency.ClearSelection()
            Me.trRbl.Style.Add("display", "none")

        Else
            If Me.rblDependency.SelectedValue <> "" Then
                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                    If ddlVisit.SelectedValue <> 0 Then
                        If Me.rblDependency.SelectedValue = "A" Then
                            If Not FillddlActAttribute() Then
                                Exit Sub
                            End If
                            'If Not FillddlActActivty() Then
                            '    Exit Sub
                            'End If

                        ElseIf Me.rblDependency.SelectedValue = "F" Then
                            If Not FillddlAttAttribute() Then
                                Exit Sub
                            End If

                            If Not FillChkAttTrgAttribute() Then
                                Exit Sub
                            End If
                        End If
                    End If
                ElseIf Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                    If Me.ddlPeriod.SelectedValue <> 0 Then
                        If Me.rblDependency.SelectedValue = "A" Then
                            If Not FillddlActAttribute() Then
                                Exit Sub
                            End If
                            'If Not FillddlActActivty() Then
                            '    Exit Sub
                            'End If
                        ElseIf Me.rblDependency.SelectedValue = "F" Then
                            If Not FillddlAttAttribute() Then
                                Exit Sub
                            End If

                            If Not FillChkAttTrgAttribute() Then
                                Exit Sub
                            End If
                        End If
                    End If
                End If
            Else
                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                    If Me.ddlVisit.SelectedValue <> 0 Then
                        '' Add by shivani pandya for lock impact
                        If hndLockStatus.Value.Trim() = "Lock" Then
                            Me.trRbl.Style.Add("display", "none")
                        Else
                            Me.trRbl.Style.Add("display", "")
                        End If
                        ''   Me.trRbl.Style.Add("display", "")
                    End If
                ElseIf Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                    If Me.ddlPeriod.SelectedValue <> 0 Then
                        '' Add by shivani pandya for lock impact
                        If hndLockStatus.Value.Trim() = "Lock" Then
                            Me.trRbl.Style.Add("display", "none")
                        Else
                            Me.trRbl.Style.Add("display", "")
                        End If
                        ''Me.trRbl.Style.Add("display", "")
                    End If
                End If
            End If
        End If
    End Sub

    Protected Sub ddlActAttribute_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlActAttribute.SelectedIndexChanged
        Dim ds_Medex As New DataSet
        Dim Wstr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim MedexValues() As String
        Dim dt_MedExDependency As New DataTable
        Dim dr As DataRow
        Try

            If ddlActAttribute.SelectedValue <> 0 Then
                Wstr = "SELECT * FROM MedexWorkSPaceDtl WHERE nMedexWorkSPaceHdrNo IN (SELECT nMedexWorkSPaceHdrNo FROM MedexWorkSPaceHdr WHERE vWorkSpaceID = '" + Me.HProjectId.Value().ToString() + "' AND iNodeId = " + Me.ddlActivity.SelectedValue.Split("##")(0).Trim.ToString() + "  AND vMedexCode = '" + Me.ddlActAttribute.SelectedValue.Trim.ToString() + "')"
                ' Wstr = "SELECT * FROM View_MedExmst where vMedexCode = '" + Me.ddlActAttribute.SelectedValue.Trim.ToString() + "'"

                ds_Medex = objhelpDb.GetResultSet(Wstr, "MedexWorkSPaceDtl")
                If Not ds_Medex Is Nothing Then
                    If ds_Medex.Tables(0).Rows.Count > 0 Then
                        Me.ViewState(VS_Medex) = ds_Medex
                        If ds_Medex.Tables(0).Rows(0)("vMedexType") = "Formula" Or ds_Medex.Tables(0).Rows(0)("vMedexType") = "CrfTerm" _
                        Or ds_Medex.Tables(0).Rows(0)("vMedexType") = "Time" Or ds_Medex.Tables(0).Rows(0)("vMedexType") = "ComboGlobalDictionary" _
                        Or ds_Medex.Tables(0).Rows(0)("vMedexType") = "TextBox" Or ds_Medex.Tables(0).Rows(0)("vMedexType") = "Label" _
                        Or ds_Medex.Tables(0).Rows(0)("vMedexType") = "AsyncTime" Or ds_Medex.Tables(0).Rows(0)("vMedexType") = "TextArea" _
                        Or ds_Medex.Tables(0).Rows(0)("vMedexType") = "AsyncDateTime" Then


                            Me.txtAct.Visible = True

                            Me.ChkAct.Visible = False
                            Me.fldAct.Style.Add("display", "none")
                        ElseIf ds_Medex.Tables(0).Rows(0)("vMedexType") = "CheckBox" Or ds_Medex.Tables(0).Rows(0)("vMedexType") = "ComboBox" _
                        Or ds_Medex.Tables(0).Rows(0)("vMedexType") = "Radio" Then

                            MedexValues = ds_Medex.Tables(0).Rows(0)("vMedexValues").ToString().Split(",")
                            dt_MedExDependency.Columns.Add("ChkText", System.Type.GetType("System.String"))
                            dt_MedExDependency.Columns.Add("ChkValue", System.Type.GetType("System.String"))

                            For Each Item As String In MedexValues
                                'If Item <> "" Then
                                dr = dt_MedExDependency.NewRow()
                                dr("ChkText") = Item
                                dr("ChkValue") = Item
                                dt_MedExDependency.Rows.Add(dr)
                                dt_MedExDependency.AcceptChanges()
                                'End If
                            Next

                            Me.ChkAct.DataSource = dt_MedExDependency
                            Me.ChkAct.DataTextField = "ChkText"
                            Me.ChkAct.DataValueField = "ChkValue"
                            Me.ChkAct.DataBind()
                            Me.txtAct.Visible = False
                            Me.ChkAct.Visible = True
                            Me.fldAct.Style.Add("display", "")
                        ElseIf ds_Medex.Tables(0).Rows(0)("vMedexType") = "DateTime" Then
                            Dim CalendarExtender As AjaxControlToolkit.CalendarExtender = New AjaxControlToolkit.CalendarExtender()
                            Dim imgTo As New System.Web.UI.HtmlControls.HtmlImage()
                            imgTo.ID = "img" & CStr(ds_Medex.Tables(0).Rows(0)("vMedexType"))
                            CalendarExtender.TargetControlID = Me.txtAct.ID
                            CalendarExtender.PopupButtonID = imgTo.ID
                            CalendarExtender.Format = "dd-MMM-yyyy"
                            Me.txtAct.Visible = True

                            Me.ChkAct.Visible = False
                            Me.fldAct.Style.Add("display", "none")
                        End If


                        Me.trAct.Visible = True
                    End If
                End If
            Else
                Me.txtAct.Visible = False
                Me.ChkAct.Visible = False
                Me.fldAct.Style.Add("display", "none")
                objCommon.ShowAlert("Please Select Attribute", Me.Page)
                Exit Sub
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub ddlAttAttribute_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAttAttribute.SelectedIndexChanged
        Dim ds_Medex As New DataSet
        Dim Wstr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim MedexValues() As String
        Dim dt_MedExDependency As New DataTable
        Dim dr As DataRow
        Dim dsDate As New DataSet
        Try

            If ddlAttAttribute.SelectedValue <> 0 Then
                Wstr = "SELECT * FROM MedexWorkSPaceDtl WHERE nMedexWorkSPaceHdrNo IN (SELECT nMedexWorkSPaceHdrNo FROM MedexWorkSPaceHdr WHERE vWorkSpaceID = '" + Me.HProjectId.Value().ToString() + "' AND iNodeId = " + Me.ddlActivity.SelectedValue.Split("##")(0).Trim.ToString() + "  AND vMedexCode = '" + Me.ddlAttAttribute.SelectedValue.Trim.ToString() + "')"
                'Wstr = "SELECT * FROM View_MedExmst where vMedexCode = '" + Me.ddlAttAttribute.SelectedValue.Trim.ToString() + "'"
                ds_Medex = objhelpDb.GetResultSet(Wstr, "MedexWorkSPaceDtl")
                If Not ds_Medex Is Nothing Then
                    If ds_Medex.Tables(0).Rows.Count > 0 Then
                        Me.ViewState(VS_Medex) = ds_Medex
                        If ds_Medex.Tables(0).Rows(0)("vMedexType") = "Formula" Or ds_Medex.Tables(0).Rows(0)("vMedexType") = "CrfTerm" _
                        Or ds_Medex.Tables(0).Rows(0)("vMedexType") = "Time" Or ds_Medex.Tables(0).Rows(0)("vMedexType") = "ComboGlobalDictionary" _
                        Or ds_Medex.Tables(0).Rows(0)("vMedexType") = "TextBox" Or ds_Medex.Tables(0).Rows(0)("vMedexType") = "Label" _
                        Or ds_Medex.Tables(0).Rows(0)("vMedexType") = "AsyncTime" Or ds_Medex.Tables(0).Rows(0)("vMedexType") = "TextArea" _
                        Or ds_Medex.Tables(0).Rows(0)("vMedexType") = "DateTime" Or ds_Medex.Tables(0).Rows(0)("vMedexType") = "AsyncDateTime" Then

                            Me.txtAtt.Visible = True
                            Me.ChkAtt.Visible = False
                            Me.FldAtt.Style.Add("display", "none")
                            Me.ddlDate.Style.Add("display", "none")
                            Me.ddlMonth.Style.Add("display", "none")
                            Me.ddlYear.Style.Add("display", "none")
                        ElseIf ds_Medex.Tables(0).Rows(0)("vMedexType") = "CheckBox" Or ds_Medex.Tables(0).Rows(0)("vMedexType") = "ComboBox" _
                        Or ds_Medex.Tables(0).Rows(0)("vMedexType") = "Radio" Then

                            MedexValues = ds_Medex.Tables(0).Rows(0)("vMedexValues").ToString().Split(",")
                            dt_MedExDependency.Columns.Add("ChkText", System.Type.GetType("System.String"))
                            dt_MedExDependency.Columns.Add("ChkValue", System.Type.GetType("System.String"))

                            For Each Item As String In MedexValues
                                If Item <> "" Then
                                    dr = dt_MedExDependency.NewRow()
                                    dr("ChkText") = Item
                                    dr("ChkValue") = Item
                                    dt_MedExDependency.Rows.Add(dr)
                                    dt_MedExDependency.AcceptChanges()
                                End If
                            Next

                            Me.ChkAtt.DataSource = dt_MedExDependency
                            Me.ChkAtt.DataTextField = "ChkText"
                            Me.ChkAtt.DataValueField = "ChkValue"
                            Me.ChkAtt.DataBind()

                            Me.txtAtt.Visible = False
                            Me.ChkAtt.Visible = True
                            Me.FldAtt.Style.Add("display", "")
                            Me.ddlDate.Style.Add("display", "none")
                            Me.ddlMonth.Style.Add("display", "none")
                            Me.ddlYear.Style.Add("display", "none")
                        ElseIf ds_Medex.Tables(0).Rows(0)("vMedexType") = "DateTime" Then
                            Dim CalendarExtender As AjaxControlToolkit.CalendarExtender = New AjaxControlToolkit.CalendarExtender()
                            Dim imgTo As New System.Web.UI.HtmlControls.HtmlImage()
                            imgTo.ID = "img" & CStr(ds_Medex.Tables(0).Rows(0)("vMedexType"))
                            CalendarExtender.TargetControlID = Me.txtAct.ID
                            CalendarExtender.PopupButtonID = imgTo.ID
                            CalendarExtender.Format = "dd-MMM-yyyy"
                            Me.txtAtt.Visible = True
                            Me.ChkAtt.Visible = False
                            Me.FldAtt.Style.Add("display", "none")
                            Me.ddlDate.Style.Add("display", "none")
                            Me.ddlMonth.Style.Add("display", "none")
                            Me.ddlYear.Style.Add("display", "none")
                        ElseIf ds_Medex.Tables(0).Rows(0)("vMedexType").ToString.ToUpper = "STANDARDDATE" Then
                            Me.txtAtt.Visible = False
                            Me.ChkAtt.Visible = False
                            Me.FldAtt.Style.Add("display", "none")
                            Me.ddlDate.Style.Add("display", "")
                            Me.ddlMonth.Style.Add("display", "")
                            Me.ddlYear.Style.Add("display", "")

                            If Not objhelpDb.GetDatesMonthsAndYears("PROC_GetDatesMonthsAndYears", dsDate, eStr) Then

                            End If

                            ddlDate.DataSource = dsDate.Tables(0)
                            ddlDate.DataTextField = "Dates"
                            ddlDate.DataValueField = "Dates"
                            ddlDate.DataBind()
                            ddlDate.Items.Insert(0, New ListItem("dd", ""))
                            ddlDate.Items.Insert(1, New ListItem("UK", "00"))

                            ddlMonth.DataSource = dsDate.Tables(1)
                            ddlMonth.DataTextField = "Months"
                            ddlMonth.DataValueField = "Val"
                            ddlMonth.DataBind()
                            ddlMonth.Items.Insert(0, New ListItem("MMM", ""))
                            ddlMonth.Items.Insert(1, New ListItem("UNK", "00"))

                            ddlYear.DataSource = dsDate.Tables(2)
                            ddlYear.DataTextField = "Years"
                            ddlYear.DataValueField = "Years"
                            ddlYear.DataBind()
                            ddlYear.Items.Insert(0, New ListItem("yyyy", ""))
                            ddlYear.Items.Insert(1, New ListItem("UKUK", "0000"))
                        End If
                        Me.trAtt.Visible = True

                        If Not FillChkAttTrgAttribute() Then
                            Exit Sub
                        End If
                    End If
                End If
            Else
                Me.txtAtt.Visible = False
                Me.ChkAtt.Visible = False
                Me.FldAtt.Visible = False
                Me.FldAtt.Style.Add("display", "none")
                Me.ddlDate.Style.Add("display", "none")
                Me.ddlMonth.Style.Add("display", "none")
                Me.ddlYear.Style.Add("display", "none")
                objCommon.ShowAlert("Please Select Source Attribute", Me.Page)
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....ddlAttAttribute_SelectedIndexChanged")
        End Try
    End Sub


    Protected Sub ddlTargetVisit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTargetVisit.SelectedIndexChanged
        If ddlTargetVisit.SelectedValue <> 0 Then
            If Not FillddlTargetActivity() Then
                Exit Sub
            End If
            Me.ChkAttTrgAttribute.Items.Clear()

            '' Add by shivani pandya for lock impact
            If hndLockStatus.Value.Trim() = "Lock" Then
                Me.trRbl.Style.Add("display", "none")
            Else
                Me.trRbl.Style.Add("display", "")
            End If
        Else
            Me.ddlTargetActivity.Items.Clear()
            Me.ddlTargetActivity.Items.Insert(0, New ListItem("Select Activity", 0))
            FillChkAttTrgAttribute()
            Me.ChkAttTrgAttribute.Items.Clear()

            'Me.rblDependency.ClearSelection()
            'Me.trRbl.Style.Add("display", "none")
        End If

        
    End Sub

    Protected Sub ddlTargetActivity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTargetActivity.SelectedIndexChanged
        Try

            If ddlVisit.SelectedValue = ddlTargetVisit.SelectedValue And ddlActivity.SelectedValue = ddlTargetActivity.SelectedValue Then
                RadioButtonList1.SelectedIndex = 0
            Else
                RadioButtonList1.SelectedIndex = 1
            End If
            If rblDependency.SelectedIndex = 1 Then
                If Not FillChkAttTrgAttribute() Then
                    Exit Sub
                End If
            End If

            If Convert.ToString(ddlActivity.SelectedItem) = Convert.ToString(ddlTargetActivity.SelectedItem) And rblDependency.SelectedIndex <> 1 Then
                fldAttachedActivities.Style.Add("display", "")
            Else
                fldAttachedActivities.Style.Add("display", "none")
            End If

        Catch ex As Exception

        End Try
        
    End Sub

    Protected Sub chkAllActivity_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAllActivity.CheckedChanged


        Dim tbl As New HtmlTable
        Dim row As New HtmlTableRow
        Dim cell As HtmlTableCell
        Dim div As New HtmlGenericControl("div")
        Dim dsPeriod As New DataSet
        Dim dtPeriod As New DataTable
        Dim chkPeriod As New CheckBoxList
        Dim lstItem As New ListItem
        Dim Count As Integer = 0
        Dim dvPeriod As DataView

        Try


            If chkAllActivity.Checked Then
                If Me.HProjectId.Value.Trim() = "" Then
                    objCommon.ShowAlert("Please Enter Project.", Me)
                    Me.chkAllActivity.Checked = False
                    Exit Sub
                ElseIf Me.ddlPeriod.SelectedItem.Text.ToLower = "select period" Then
                    objCommon.ShowAlert("Please Select Period.", Me)
                    Me.chkAllActivity.Checked = False
                    Exit Sub
                ElseIf Me.ddlActivity.SelectedItem.Text.ToLower = "select activity" Then
                    objCommon.ShowAlert("Please Select Activity.", Me)
                    Me.chkAllActivity.Checked = False
                    Exit Sub
                End If
            End If

            If Me.chkAllActivity.Checked Then
                If Me.ddlActivity.SelectedValue.ToString.Trim() <> "" And _
                    Me.ddlActivity.SelectedValue.ToString.Trim().ToLower() <> "select activity" Then


                    Dim pnl As Panel
                    dsPeriod = Me.objhelpDb.GetResultSet("select iNodeID,vNodeDisplayName,iPeriod,ActivityWithParent,vActivityId " + _
                                                         " from View_WorkspaceNodeDetail where vWorkspaceId='" + Me.HProjectId.Value + "'" + _
                                                         " And cStatusIndi<>'D' AND vActivityId = '" + ddlActivity.SelectedValue.Split("##")(2).ToString + "'" + _
                                                         " Order by nRefTime,iNodeID", "PeriodDetails")

                    If Not dsPeriod Is Nothing Then

                        dtPeriod = dsPeriod.Tables(0).DefaultView.ToTable(True, "iPeriod")
                        dtPeriod.DefaultView.Sort = "iPeriod ASC"

                        For Each dr As DataRow In dtPeriod.DefaultView.ToTable().Rows

                            dvPeriod = dsPeriod.Tables(0).DefaultView
                            dvPeriod.RowFilter = "iPeriod = " + dr("iPeriod").ToString

                            Count += 1

                            cell = New HtmlTableCell
                            chkPeriod = New CheckBoxList

                            chkPeriod.ID = "Period_" + dr("iPeriod").ToString

                            For Each drItem As DataRow In dvPeriod.ToTable().Rows
                                lstItem = New ListItem
                                lstItem.Text = drItem("vNodeDisplayName")
                                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                                    lstItem.Text = drItem("ActivityWithParent")
                                End If
                                lstItem.Value = drItem("iNodeID")
                                lstItem.Attributes.Add("NodeID", drItem("iNodeID").ToString() + "#" + dr("iPeriod").ToString())
                                lstItem.Attributes.Add("title", "(" + drItem("vActivityId").ToString() + ")")
                                chkPeriod.Items.Add(lstItem)
                            Next
                            'chkPeriod.DataSource = dvPeriod.ToTable()
                            'chkPeriod.DataTextField = "vNodeDisplayName"
                            'chkPeriod.DataValueField = "iNodeID"

                            chkPeriod.DataBind()
                            chkPeriod.Items.Insert(0, "Select All")
                            chkPeriod.Items(0).Attributes.Add("onclick", "fnSelectAllPeriod(this);")

                            chkPeriod.Style.Add("text-align", "left")
                            chkPeriod.Style.Add("margin", "15px 10px 10px 10px")
                            chkPeriod.Style.Add("font-size", "8pt")

                            pnl = New Panel
                            pnl.GroupingText = "<span class='Label'>Period " + dr("iPeriod").ToString + "</span>"
                            pnl.Style.Add("width", "100%")

                            pnl.Controls.Add(chkPeriod)

                            If ((Count Mod 2) <> 0) Then
                                row = New HtmlTableRow
                            End If

                            cell.Controls.Add(pnl)
                            cell.Style.Add("width", "50%")
                            cell.Attributes.Add("vAlign", "top")
                            row.Cells.Add(cell)
                            tbl.Rows.Add(row)

                        Next
                        tbl.Attributes.Add("width", "100%")
                        Me.tdAllActivity.Controls.Add(tbl)

                    End If
                End If
            End If
        Catch ex As Exception

            Me.ShowErrorMessage(ex.ToString, "")

        End Try

    End Sub
#End Region

#Region "Button Events"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim mode As String = String.Empty
        Try
            ResetPage()
            If Me.HProjectId.Value.Trim() <> "" Then
                If Not FillddlPeriod() Then
                    Exit Sub
                End If
                'mode = "1"
                'If Not FillGrid(mode) Then
                '    Exit Sub
                'End If
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "Error in BtnSetProject_Click")
        End Try
    End Sub

    Protected Sub btnActSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnActSave.Click
        Dim ds_Save As New DataSet
        Dim eStr As String = String.Empty

        Try
            If Me.HProjectId.Value <> "" Then
                If Not AssignValuesActivity(ds_Save) Then
                    Exit Sub
                End If

                If Not objLambda.Save_MedExDependency(Me.ViewState(VS_Choice), ds_Save, Me.Session(S_UserID), eStr) Then
                    objCommon.ShowAlert("Error While Saving MedExDependency", Me.Page)
                    Exit Sub
                End If

                If Not FillGrid("2") Then
                    Exit Sub
                End If
                'Me.ddlActActivty.SelectedIndex = -1
                Me.ddlActAttribute.SelectedIndex = -1
                Me.txtAct.Text = ""
                Me.txtAct.Visible = False
                Me.ChkAct.Visible = False
                Me.fldAct.Style.Add("display", "none")
                Me.ChkAct.ClearSelection()
                Me.trAct.Visible = False

                fldAttachedActivities.Style.Add("display", "none")

                objCommon.ShowAlert("Activity Dependency Saved Sucessfully !", Me.Page)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......btnActSave_Click")
        End Try
    End Sub

    Protected Sub btnAttSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAttSave.Click
        Dim ds_Save As New DataSet
        Dim eStr As String = String.Empty
        Try
            If Me.HProjectId.Value <> "" Then
                If Me.HProjectId.Value <> "" Then
                    If Not AssignValuesAttribute(ds_Save) Then
                        Exit Sub
                    End If

                    If Not objLambda.Save_MedExDependency(Me.ViewState(VS_Choice), ds_Save, Me.Session(S_UserID), eStr) Then
                        objCommon.ShowAlert("Error While Saving MedExDependency", Me.Page)
                        Exit Sub
                    End If

                    If Not FillGrid("2") Then
                        Exit Sub
                    End If

                    Me.ddlAttAttribute.SelectedIndex = -1
                    For Each item As ListItem In Me.ChkAttTrgAttribute.Items
                        item.Selected = False
                    Next
                    Me.txtAtt.Text = ""
                    Me.txtAtt.Visible = False
                    Me.ChkAtt.Visible = False
                    Me.ChkAtt.ClearSelection()
                    Me.FldAtt.Style.Add("display", "none")
                    Me.trAtt.Visible = False
                    objCommon.ShowAlert("Attribute Dependency Saved Sucessfully !", Me.Page)
                End If
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......btnAttSave_Click")
        End Try
    End Sub

    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
        Dim mode As String = String.Empty

        Try
            mode = "2"
            If Not FillGrid(mode) Then
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnGo_click")
        End Try

    End Sub

    'Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
    '    Dim MedexDependency As String = String.Empty
    '    Dim chk As CheckBox
    '    Dim dt_medexDependency As New DataTable
    '    Dim ds_medex As New DataSet
    '    Dim Wstr As String = String.Empty
    '    Dim eStr As String = String.Empty
    '    Try
    '        dt_medexDependency = CType(Me.ViewState(VS_DsDependency), DataTable)
    '        For index As Integer = 0 To gvwDependency.Rows.Count - 1
    '            chk = CType(Me.gvwDependency.Rows(index).Cells(GVCDependency_Select).FindControl("ChkMove"), CheckBox)
    '            If chk.Checked Then
    '                MedexDependency += Me.gvwDependency.Rows(index).Cells(GVCDependency_nMedExDependcyNo).Text.ToString.Trim() + ","
    '            End If
    '        Next

    '        If MedexDependency <> "" Then
    '            MedexDependency = MedexDependency.Substring(0, MedexDependency.LastIndexOf(","))
    '        End If

    '        Wstr = "SELECT * FROM  MedexDependency WHERE nMedExDependcyNo IN (" + MedexDependency + ") "
    '        ds_medex = objhelpDb.GetResultSet(Wstr, "MedexDependency")

    '        For Each dr As DataRow In ds_medex.Tables(0).Rows
    '            dr("cStatusIndi") = "D"
    '            dr("iModifyBy") = Session(S_UserID)
    '            ds_medex.Tables(0).AcceptChanges()
    '        Next

    '        If Not objLambda.Save_MedExDependency(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_medex, Me.Session(S_UserID), eStr) Then
    '            objCommon.ShowAlert("Error While Saving MedExDependency", Me.Page)
    '            Exit Sub
    '        End If

    '        objCommon.ShowAlert("Record Deleted Sucessfully", Me.Page)

    '        If Not FillGrid("1") Then
    '            Exit Sub
    '        End If

    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.Message, "....btndelete_click")
    '    End Try
    'End Sub

    Protected Sub btnActCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnActCancel.Click
        'Me.ddlActActivty.SelectedIndex = -1
        Me.ddlActAttribute.SelectedIndex = -1
        Me.ChkAct.Items.Clear()
        Me.txtAct.Text = ""
        Me.trAct.Visible = False
    End Sub

    Protected Sub btnAttCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAttCancel.Click
        Me.ddlAttAttribute.SelectedIndex = -1
        Me.txtAtt.Text = ""
        Me.ChkAtt.Items.Clear()
        Me.trAtt.Visible = False
        For Each item As ListItem In Me.ChkAttTrgAttribute.Items
            item.Selected = False
        Next
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim fileName As String = String.Empty
        Dim ds As New DataSet
        Try

            If Me.gvwDependency.Rows.Count < 1 Then
                Me.objCommon.ShowAlert("No Data To Export", Me.Page)
                Exit Sub
            End If
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            fileName = "Attribute Dependency Report"
            fileName = fileName & ".xls"
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)


            ds.Tables.Add(CType(Me.ViewState(VS_DsDependency), DataSet).Tables(0).Copy())
            ds.AcceptChanges()

            Context.Response.Write(ConvertDsuserTO(ds))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception
            'Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

    Private Function ConvertDsuserTO(ByVal ds As DataSet) As String
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer
        Dim dsConvert As New DataSet
        Dim SrNo As Integer = 1
        Try
            'If Me.rblstatus.SelectedValue = 0 Then
            '    Status = "Screening Location : " + Me.ddlLoction.SelectedItem.Text.Trim() + " Master List For Project No : " + Me.txtprojectForsubject.Text.Trim()
            'ElseIf Me.rblstatus.SelectedValue = 1 Then
            '    Status = "Screening Location : " + Me.ddlLoction.SelectedItem.Text.Trim() + "                Screening Date : " + Me.txtdate.Text.Trim()
            'End If

            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""9""><center><strong><font color=""black"" size=""9px"" face=""Calibri"">")
            strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""9""><center><strong><font color=""black"" size=""8px"" face=""Calibri"">")
            strMessage.Append("Dependency Details")
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")
            strMessage.Append("<td><strong><font color=""black"" size=""9px"" face=""Calibri"">")
            strMessage.Append("Project/Site")
            strMessage.Append("</font></strong></td>")
            strMessage.Append("<td colspan=""8""><strong><font color=""black"" size=""9px"" face=""Calibri"">")
            strMessage.Append(Me.txtproject.Text.Trim().Substring(1, Me.txtproject.Text.Trim().IndexOf("]") - 1))
            strMessage.Append("</font></strong></td>")
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")


            ds.Tables(0).Columns.Add("vSrNo")
            ds.Tables(0).Columns.Add("vModifyWithIST")
            For Each dr In ds.Tables(0).Rows
                dr("vSrNo") = SrNo
                dr("vModifyWithIST") = CDate(dr("dModifyOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset
                SrNo += 1
            Next
            dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vSrNo,SourceMedexDesc,TargetMedexDesc,vMedExValue,SourceActivityDesc,TargetActivityDesc,cDependencyType,iModifyBy,vModifyWithIST".Split(",")).DefaultView.ToTable())
            dsConvert.AcceptChanges()
            dsConvert.Tables(0).Columns(0).ColumnName = "Sr.No."
            dsConvert.Tables(0).Columns(1).ColumnName = "Source Attribute"
            dsConvert.Tables(0).Columns(2).ColumnName = "Target Attribute"
            dsConvert.Tables(0).Columns(3).ColumnName = "Attribute Value"
            dsConvert.Tables(0).Columns(4).ColumnName = "Source Activity"
            dsConvert.Tables(0).Columns(5).ColumnName = "Target Activity"
            dsConvert.Tables(0).Columns(6).ColumnName = "Dependency Type"
            dsConvert.Tables(0).Columns(7).ColumnName = "Modify By"
            dsConvert.Tables(0).Columns(8).ColumnName = "Modify On"

            dsConvert.AcceptChanges()

            For iCol = 0 To dsConvert.Tables(0).Columns.Count - 1

                strMessage.Append("<td style=""text-align:center;""><strong><font color=""Black"" size=""9px"" face=""Calibri"">")
                strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).Trim())
                strMessage.Append("</font></strong></td>")

            Next
            strMessage.Append("</tr>")

            For j = 0 To dsConvert.Tables(0).Rows.Count - 1
                strMessage.Append("<tr>")
                For i = 0 To dsConvert.Tables(0).Columns.Count - 1
                    If i = 0 Then
                        strMessage.Append("<td style=""text-align:center;vertical-align:top;""><font color=""black"" size=""2"" face=""Calibri"">")
                    Else
                        strMessage.Append("<td style=""text-align:left;vertical-align:top;""><font color=""black"" size=""2"" face=""Calibri"">")
                    End If
                    strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim())
                    strMessage.Append("</font></td>")

                Next
                strMessage.Append("</tr>")
            Next
            strMessage.Append("</table>")

            Return strMessage.ToString

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....ConvertDsuserTO")
            Return ""
        End Try
    End Function

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ResetPage()
        Me.ddlActivity.Items.Clear()
        Me.ddlVisit.Items.Clear()
        Me.ddlPeriod.Items.Clear()
        Me.txtproject.Text = ""
        Me.HProjectId.Value = Nothing
        For Each rblitem As ListItem In Me.rblDependency.Items
            rblitem.Selected = False
        Next
    End Sub
#End Region

#Region "Assignvalues"

    Private Function AssignValuesAttribute(ByRef ds_Save As DataSet) As Boolean
        Dim dtOld As New DataTable
        Dim dr As DataRow
        Dim ds_MedExDependency As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty

        Try


            'If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
            '    ds_MedExDependency = Me.ViewState(VS_DTimeZoneMst)

            'Else
            wStr = "1=2"

            If Not objhelpDb.GetMedexDependency(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_MedExDependency, eStr) Then
                Me.ShowErrorMessage("", eStr)
                Exit Function
            End If

            ' End If

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                For Each chkItem As ListItem In Me.ChkAttTrgAttribute.Items
                    If chkItem.Selected = True Then
                        Dim SelectedVal As String = String.Empty
                        dr = ds_MedExDependency.Tables(0).NewRow()
                        dr("vWorkspaceId") = Me.HProjectId.Value.Trim.ToString()
                        dr("vSourceMedExCode") = Me.ddlAttAttribute.SelectedValue.Trim()
                        dr("vTargetMedExCode") = chkItem.Value.ToString()
                        If Me.txtAtt.Visible = True Then
                            dr("vMedExValue") = Me.txtAtt.Text.Trim()
                        ElseIf Me.ChkAtt.Visible = True Then
                            For Each Item As ListItem In Me.ChkAtt.Items
                                If Item.Selected = True Then
                                    SelectedVal += Item.Text + ","
                                End If
                            Next
                            If SelectedVal <> "" Then
                                SelectedVal = SelectedVal.Substring(0, SelectedVal.LastIndexOf(","))
                            End If
                            dr("vMedExValue") = SelectedVal
                        ElseIf Me.ddlDate.Style("display") <> "none" Then
                            dr("vMedExValue") = Me.ddlDate.SelectedValue.ToString + Me.ddlMonth.SelectedValue.ToString + Me.ddlYear.SelectedValue.ToString
                        End If
                        dr("vSourceActivityId") = Me.ddlActivity.SelectedValue.Split("##")(2).Trim.ToString()
                        dr("vTargetActivityId") = Me.ddlTargetActivity.SelectedValue.Split("##")(2).Trim.ToString()
                        dr("iSourceNodeId") = Me.ddlActivity.SelectedValue.Split("##")(0).Trim.ToString()
                        dr("iTargetNodeId") = Me.ddlTargetActivity.SelectedValue.Split("##")(0).Trim.ToString()
                        dr("cDependencyType") = "F"
                        dr("cActionType") = "E"
                        If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                            dr("iParentNodeid") = Me.ddlVisit.SelectedValue.ToString()
                            dr("iPeriod") = System.DBNull.Value
                        Else
                            dr("iParentNodeid") = System.DBNull.Value
                            dr("iPeriod") = Me.ddlPeriod.SelectedValue.ToString()
                        End If
                        dr("cStatusIndi") = "N"
                        dr("iModifyBy") = Session(S_UserID)

                        ds_MedExDependency.Tables(0).Rows.Add(dr)
                        ds_MedExDependency.Tables(0).AcceptChanges()
                    End If
                Next

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                'For Each dr In ds_MedExDependency.Tables(0).Rows
                '    dr("vTimeZoneName") = Me.ddlTimezone.SelectedValue.Trim()
                '    dr("vCountryName") = Me.ddlCountry.SelectedValue.Trim()
                '    dr("vTimeZoneOffset") = Me.txtoffset.Text.Trim()
                '    dr("iModifyBy") = Session(S_UserID)
                '    dr("cStatusIndi") = "E"
                '    dr.AcceptChanges()
                'Next dr
                'ds_MedExDependency.Tables(0).AcceptChanges()
            End If

            ds_Save = ds_MedExDependency
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...AssignValuesAttribute")
            Return False
        End Try
    End Function

    Private Function AssignValuesActivity(ByRef ds_Save As DataSet) As Boolean
        Dim dtOld As New DataTable
        Dim dr As DataRow
        Dim ds_MedExDependency As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim SelectedVal As String = String.Empty
        Dim strNode As String = String.Empty

        Try

            wStr = "1=2"

            If Not objhelpDb.GetMedexDependency(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_MedExDependency, eStr) Then
                Me.ShowErrorMessage("", eStr)
                Exit Function
            End If

            strNode = Me.hdnSaveNode.Value

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                If strNode <> "" Then
                    For Each Str As String In strNode.Split(",")
                        If Str <> "" Then


                            SelectedVal = ""
                            dr = ds_MedExDependency.Tables(0).NewRow()
                            dr("vWorkspaceId") = Me.HProjectId.Value.Trim.ToString()
                            dr("vSourceMedExCode") = Me.ddlActAttribute.SelectedValue.Trim()
                            dr("vTargetMedExCode") = System.DBNull.Value
                            If Me.txtAct.Visible = True Then
                                dr("vMedExValue") = Me.txtAct.Text.Trim()
                            ElseIf Me.ChkAct.Visible = True Then
                                For Each Item As ListItem In Me.ChkAct.Items
                                    If Item.Selected = True Then
                                        SelectedVal += Item.Text + ","
                                    End If
                                Next
                                If SelectedVal <> "" Then
                                    SelectedVal = SelectedVal.Substring(0, SelectedVal.LastIndexOf(","))
                                End If
                                dr("vMedExValue") = SelectedVal
                            End If
                            dr("vSourceActivityId") = Me.ddlActivity.SelectedValue.Split("##")(2).Trim.ToString()
                            dr("vTargetActivityId") = Me.ddlTargetActivity.SelectedValue.Split("#")(2).Trim.ToString()
                            dr("iSourceNodeId") = Me.ddlActivity.SelectedValue.Split("##")(0).Trim.ToString()
                            dr("iTargetNodeId") = Convert.ToInt64(Str.Split("#")(0).ToString())
                            dr("cDependencyType") = "A"
                            dr("cActionType") = "E"
                            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                                dr("iParentNodeid") = Me.ddlVisit.SelectedValue.ToString()
                                dr("iPeriod") = System.DBNull.Value
                            Else
                                dr("iParentNodeid") = System.DBNull.Value
                                dr("iPeriod") = Me.ddlPeriod.SelectedValue.ToString()
                            End If
                            dr("cStatusIndi") = "N"
                            dr("iModifyBy") = Session(S_UserID)
                            ds_MedExDependency.Tables(0).Rows.Add(dr)
                        End If
                    Next


                    'ElseIf fldAttachedActivities.Style("display") = "none" Then
                Else

                    dr = ds_MedExDependency.Tables(0).NewRow()
                    dr("vWorkspaceId") = Me.HProjectId.Value.Trim.ToString()
                    dr("vSourceMedExCode") = Me.ddlActAttribute.SelectedValue.Trim()
                    dr("vTargetMedExCode") = System.DBNull.Value
                    If Me.txtAct.Visible = True Then
                        dr("vMedExValue") = Me.txtAct.Text.Trim()
                    ElseIf Me.ChkAct.Visible = True Then
                        For Each Item As ListItem In Me.ChkAct.Items
                            If Item.Selected = True Then
                                SelectedVal += Item.Text + ","
                            End If
                        Next
                        If SelectedVal <> "" Then
                            SelectedVal = SelectedVal.Substring(0, SelectedVal.LastIndexOf(","))
                        End If
                        dr("vMedExValue") = SelectedVal
                    End If
                    dr("vSourceActivityId") = Me.ddlActivity.SelectedValue.Split("##")(2).Trim.ToString()
                    dr("vTargetActivityId") = Me.ddlTargetActivity.SelectedValue.Split("#")(2).Trim.ToString()
                    dr("iSourceNodeId") = Me.ddlActivity.SelectedValue.Split("##")(0).Trim.ToString()
                    dr("iTargetNodeId") = ddlTargetActivity.SelectedValue.Split("##")(0).Trim.ToString()
                    dr("cDependencyType") = "A"
                    dr("cActionType") = "E"
                    If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                        dr("iParentNodeid") = Me.ddlVisit.SelectedValue.ToString()
                        dr("iPeriod") = System.DBNull.Value
                    Else
                        dr("iParentNodeid") = System.DBNull.Value
                        dr("iPeriod") = Me.ddlPeriod.SelectedValue.ToString()
                    End If
                    dr("cStatusIndi") = "N"
                    dr("iModifyBy") = Session(S_UserID)

                    ds_MedExDependency.Tables(0).Rows.Add(dr)

                End If
                hdnSaveNode.Value = ""



            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                'For Each dr In ds_MedExDependency.Tables(0).Rows
                '    dr("vTimeZoneName") = Me.ddlTimezone.SelectedValue.Trim()
                '    dr("vCountryName") = Me.ddlCountry.SelectedValue.Trim()
                '    dr("vTimeZoneOffset") = Me.txtoffset.Text.Trim()
                '    dr("iModifyBy") = Session(S_UserID)
                '    dr("cStatusIndi") = "E"
                '    dr.AcceptChanges()
                'Next dr
                'ds_MedExDependency.Tables(0).AcceptChanges()
            End If

            ds_Save = ds_MedExDependency
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...AssignValuesActivity")
            Return False
        End Try
    End Function

#End Region

#Region "FillGrid"

    Private Function FillGrid(ByVal mode As String) As Boolean
        Dim ds_MedexDependency As DataSet = Nothing
        Dim eStr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim flag As Integer = 1
        Try
            Me.gvwDependency.DataSource = Nothing
            Me.gvwDependency.DataBind()
            wstr = "cStatusIndi <> 'D' And vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
            If mode = "2" Then
                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                    If ddlVisit.SelectedIndex <> 0 Then
                        flag = 6
                    End If
                Else
                    If ddlPeriod.SelectedIndex <> 0 Then
                        flag = 5
                    End If
                End If
                If Me.ddlActivity.SelectedValue <> "0" And Me.ddlActivity.SelectedValue <> "" Then
                    flag = 2
                    wstr += "AND vSourceActivityId ='" + Me.ddlActivity.SelectedValue.Split("##")(2).Trim.ToString() + "'"
                    wstr += "AND iSourceNodeId ='" + Me.ddlActivity.SelectedValue.Split("##")(0).Trim.ToString() + "'"
                Else

                End If
                If Me.rblDependency.SelectedValue <> "" Then
                    If Me.rblDependency.SelectedValue.Trim.ToString() = "F" Then
                        flag = 3
                        wstr += "AND cDependencyType ='Attribute Dependency'"
                    ElseIf Me.rblDependency.SelectedValue.Trim.ToString() = "A" Then
                        flag = 4
                        wstr += "AND cDependencyType ='Activity Dependency'"
                    End If
                End If
            End If
            If mode = "3" Then
                ds_MedexDependency = Me.ViewState(VS_DsDependency)
                Me.gvwDependency.DataSource = ds_MedexDependency.Tables(0)
                Me.gvwDependency.DataBind()
                Me.fldGrid.Style.Add("display", "")
                Return True
            End If


            If Not objhelpDb.GetviewMedexDependency(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_MedexDependency, eStr) Then
                Me.objCommon.ShowAlert("Error While Getting Data From View_MedexDependency" + eStr, Me.Page)
                Return False
            End If

            If Not ds_MedexDependency Is Nothing Then
                If ds_MedexDependency.Tables(0).Rows.Count > 0 Then
                    Me.ViewState(VS_DsDependency) = ds_MedexDependency
                    Me.gvwDependency.DataSource = ds_MedexDependency.Tables(0)
                    Me.gvwDependency.DataBind()
                    Me.fldGrid.Style.Add("display", "")
                    Me.btnExport.Visible = True
                Else
                    If flag = 1 Then
                        objCommon.ShowAlert("No Dependency Found For This Project", Me.Page)
                    ElseIf flag = 2 Then
                        objCommon.ShowAlert("No Dependency Found For This Activity", Me.Page)
                    ElseIf flag = 3 Then
                        objCommon.ShowAlert("No Dependency Found For Attribute Dependency", Me.Page)
                    ElseIf flag = 4 Then
                        objCommon.ShowAlert("No Dependency Found For Activity Dependency", Me.Page)
                    ElseIf flag = 5 Then
                        objCommon.ShowAlert("No Dependency Found,Please Select Activity", Me.Page)
                    ElseIf flag = 6 Then
                        objCommon.ShowAlert("No Dependency Found,Please Select Activity", Me.Page)
                    End If

                    Me.ViewState(VS_DsDependency) = Nothing
                    Me.gvwDependency.DataSource = Nothing
                    Me.gvwDependency.DataBind()
                    Me.fldGrid.Style.Add("display", "none")
                    Me.btnExport.Visible = False
                End If
            Else
                If flag = 1 Then
                    objCommon.ShowAlert("No Dependency Found For This Project", Me.Page)
                ElseIf flag = 2 Then
                    objCommon.ShowAlert("No Dependency Found For This Activity", Me.Page)
                ElseIf flag = 3 Then
                    objCommon.ShowAlert("No Dependency Found For Attribute Dependency", Me.Page)
                ElseIf flag = 4 Then
                    objCommon.ShowAlert("No Dependency Found For Activity Dependency", Me.Page)
                ElseIf flag = 5 Then
                    objCommon.ShowAlert("No Dependency Found,Please Select Activity", Me.Page)
                ElseIf flag = 6 Then
                    objCommon.ShowAlert("No Dependency Found,Please Select Activity", Me.Page)
                End If
                Me.ViewState(VS_DsDependency) = Nothing
                Me.gvwDependency.DataSource = Nothing
                Me.gvwDependency.DataBind()
                Me.fldGrid.Style.Add("display", "none")
                Me.btnExport.Visible = False
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Binding Grid", "....FillGrid")
            Return False
        End Try
    End Function

#End Region

#Region "Grid Events"

    Protected Sub gvwDependency_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvwDependency.PageIndexChanging
        Try
            gvwDependency.PageIndex = e.NewPageIndex
            If Not FillGrid("3") Then
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....gvwDependency_PageIndexChanging")
        End Try
    End Sub

    Protected Sub gvwDependency_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwDependency.RowCommand
        Dim index As Integer = e.CommandArgument
        Dim Wstr As String = String.Empty
        Dim ds_medex As New DataSet
        Dim eStr As String = String.Empty
        Try

            If e.CommandName.ToUpper = "DELETE" Then
                Wstr = "SELECT * FROM  MedexDependency WHERE nMedExDependcyNo ='" + Me.gvwDependency.Rows(index).Cells(GVCDependency_nMedExDependcyNo).Text.ToString.Trim() + "'"
                ds_medex = objhelpDb.GetResultSet(Wstr, "MedexDependency")


                For Each dr As DataRow In ds_medex.Tables(0).Rows
                    dr("cStatusIndi") = "D"
                    dr("iModifyBy") = Session(S_UserID)
                    ds_medex.Tables(0).AcceptChanges()
                Next

                If Not objLambda.Save_MedExDependency(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_medex, Me.Session(S_UserID), eStr) Then
                    objCommon.ShowAlert("Error While Saving MedExDependency", Me.Page)
                    Exit Sub
                End If



                If Not FillGrid("2") Then
                    Exit Sub
                End If
                objCommon.ShowAlert("Record Deleted Sucessfully !", Me.Page)
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".......gvwDependency_RowCommand")
        End Try


    End Sub

    Protected Sub gvwDependency_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwDependency.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Or e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(GVCDependency_nMedExDependcyNo).Style.Add("display", "none")
        End If

    End Sub

    Protected Sub gvwDependency_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwDependency.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVCDependency_SrNo).Text = e.Row.RowIndex + (gvwDependency.PageSize * gvwDependency.PageIndex) + 1

            If Not e.Row.Cells.Item(GVCDependency_dModifyOn).Text.ToString = "" Then
                e.Row.Cells.Item(GVCDependency_dModifyOn).Text = CType(Replace(e.Row.Cells(GVCDependency_dModifyOn).Text.Trim(), "&nbsp;", ""), Date).ToString("dd-MMM-yyyy HH:mm").Trim() + strServerOffset.ToString.Replace("IST ", "")
            End If

            If Me.hndLockStatus.Value.Trim() = "Lock" Then
                CType(e.Row.FindControl("lnkDelete"), ImageButton).Attributes.Add("Disabled", "Disabled")
            End If

            CType(e.Row.FindControl("lnkDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkDelete"), ImageButton).CommandName = "DELETE"
        End If

    End Sub

    Protected Sub gvwDependency_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvwDependency.RowDeleting

    End Sub

#End Region

#Region "Reset Page"

    Private Sub ResetPage()
        'Me.ddlActActivty.SelectedIndex = -1
        Me.ddlActAttribute.SelectedIndex = -1
        Me.ddlActivity.SelectedIndex = -1
        Me.ddlAttAttribute.SelectedIndex = -1
        Me.ChkAttTrgAttribute.Items.Clear()
        Me.FldTrgAttribute.Style.Add("display", "none")
        Me.TrTrgAttribute.Style.Add("display", "none")
        Me.ddlPeriod.SelectedIndex = -1
        Me.ddlVisit.SelectedIndex = -1
        Me.fldsActivity.Style.Add("display", "none")
        Me.fldsAttribute.Style.Add("display", "none")

        Me.fldsTargetActivity.Style.Add("display", "none")
        Me.FldTrgAttribute.Style.Add("display", "none")

        Me.fldGrid.Style.Add("display", "none")
        Me.gvwDependency.DataSource = Nothing
        Me.gvwDependency.DataBind()

        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
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
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()

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
