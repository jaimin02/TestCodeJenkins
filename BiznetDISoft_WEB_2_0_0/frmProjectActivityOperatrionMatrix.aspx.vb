Imports System.Web.Services
Imports Newtonsoft.Json

Partial Class frmProjectActivityOperatrionMatrix
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Private objCommon As New clsCommon
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
    Private objhelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private eStr_Retu As String = String.Empty
    Dim ds_UserType As New Data.DataSet
    Dim dt_ProjectActivityOpertationMatrix As New DataTable
    Dim ds_save As New DataSet
#End Region


    Protected Sub Page_Load() Handles Me.Load
        Try
            If Not IsPostBack Then
                AutoCompleteExtender1.ContextKey = "isnull(vParentWorkspaceId,'') = ''"
                GenCall()
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....Page_Load")
        Finally
        End Try
    End Sub

    Private Function GenCall() As Boolean
        Try
            If Not GenCall_ShowUI() Then 'For Displaying Data
                Return False
            End If
            GenCall = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall")
            Return False
        Finally
        End Try
    End Function

    Private Function GenCall_ShowUI() As Boolean
        Try
            Page.Title = " :: Project Activity Operation Matrix ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Project Activity Operation Rights"
            Me.hdnsession.Value = Session(S_UserID)
            GenCall_ShowUI = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_showUI")
            Return False
        Finally
        End Try

    End Function

    Protected Sub btnSetProject_Click(ByVal Sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        If Not FillData() Then
            Exit Sub
        End If
    End Sub

    Private Function FillData() As Boolean
        Dim Wstr As String = String.Empty
        Dim ds_WorkspaceNodeDetail As DataSet = Nothing
        Dim dt_WorkSpaceActivity As New DataTable
        Dim ActivityparamArry(1) As String
        Dim Periods As Integer = 1
        Dim eStr As String = String.Empty
        Try

            If Not FillDropDownListPeriods() Then
                Exit Function
            End If

            ddlPeriods_SelectedIndexChanged(Me, EventArgs.Empty)

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillParentActicity()")
            Return False
        Finally
        End Try
    End Function

#Region "FillDropDownList Periods"

    Private Function FillDropDownListPeriods() As Boolean
        Dim ds_Periods As New DataSet
        Dim wStr As String = String.Empty
        Dim Periods As Integer = 1
        Dim eStr As String = String.Empty

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            Me.ddlPeriods.Items.Clear()

            wStr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "' and cStatusIndi<>'D'"

            If Not objhelp.GetWorkspaceProtocolDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        ds_Periods, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_Periods Is Nothing Then
                Throw New Exception("Problem while getting data")
            End If

            If Not ds_Periods.Tables(0).Rows(0)("iNoOfPeriods") Is Nothing Then
                If IsNumeric(ds_Periods.Tables(0).Rows(0)("iNoOfPeriods")) Then
                    Periods = ds_Periods.Tables(0).Rows(0)("iNoOfPeriods")
                    For count As Integer = 0 To Periods - 1
                        Me.ddlPeriods.Items.Add((count + 1).ToString)
                    Next count

                    Me.ddlPeriods.Items.Insert(0, "ALL")

                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Legend", "legendUI();", True)
                    Return True
                Else
                    objCommon.ShowAlert("Record Not Found.!", Me.Page)
                    Return False
                End If
            End If
        Catch ex As Exception
            ShowErrorMessage("Problem While Filling Periods. ", ex.Message)
            eStr = ex.Message
            Return False
        Finally
            ds_Periods.Dispose()
        End Try

    End Function

#End Region

    Protected Function BindUserTypeTree(ByRef eStr As String) As Boolean
        Dim strqry As String = String.Empty
        Dim whrCon As String = String.Empty
        Dim dsSubject As DataSet = New DataSet
        Dim dtUserType As New DataTable
        Dim ds_UserType As New Data.DataSet
        Dim Wstr As String = String.Empty

        Try

            Me.tdHRUpper.Style.Add("display", "none")
            Me.tdHRLower.Style.Add("display", "none")
            Me.divUserType.Style.Add("display", "none")
            Me.tvUserType.Style.Add("Height", "0px")

            If Not objhelp.getUserTypeMst("cStatusIndi <> '" + Status_Delete + "' Order By vUserTypeName", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_UserType, estr) Then
                Return False
            End If

            If Not ds_UserType Is Nothing Then
                If ds_UserType.Tables(0).Rows.Count > 0 Then
                    dtUserType = ds_UserType.Tables(0)
                    tvUserType.Nodes.Clear()
                    Dim nodeAll As New TreeNode()
                    nodeAll.Text = "User Profiles*"
                    nodeAll.Value = "User Profiles"

                    Me.tvUserType.Nodes.Add(nodeAll)
                    For index = 0 To dtUserType.Rows.Count - 1
                        Dim nodeUserType As New TreeNode()
                        nodeUserType.Text = dtUserType.Rows(index).Item("vUserTypeName").ToString()
                        nodeUserType.ToolTip = dtUserType.Rows(index).Item("vUserTypeName").ToString()
                        nodeUserType.Value = dtUserType.Rows(index).Item("vUserTypeCode").ToString()
                        nodeUserType.SelectAction = TreeNodeSelectAction.None
                        nodeUserType.ChildNodes.Add(nodeUserType)
                        Me.tvUserType.Nodes(0).ChildNodes.Add(nodeUserType)
                    Next ' Next Index
                    Me.tvUserType.Nodes(0).ExpandAll()
                    Me.tvUserType.Nodes(0).SelectAction = TreeNodeSelectAction.None
                    Me.tdHRUpper.Style.Add("display", "")
                    Me.tdHRLower.Style.Add("display", "")
                    Me.divUserType.Style.Add("display", "block")
                    Me.tvUserType.Style.Add("Height", "100px")
                Else
                    objCommon.ShowAlert("No User Type Found!", Me.Page)
                End If
            End If


            Return True
        Catch ex As Exception
            estr = ex.Message
            Return False
        Finally
            dsSubject.Dispose()
            dtUserType.Dispose()
        End Try
    End Function

    Protected Function BindActivityTree(ByRef eStr As String) As Boolean
        Dim strqry As String = String.Empty
        Dim iPeriod As String = String.Empty
        Dim whrCon As String = String.Empty
        Dim dtActivity As New DataTable
        Dim dsActivity As DataSet = New DataSet
        Dim dvActivity As DataView
        Dim dvChild As DataView
        Dim Subject_Specific As String = String.Empty
        Dim ActNodeAll As New TreeNode()
        Dim Wstr As String = String.Empty
        Dim ds_WorkspaceNodeDetail As New DataSet
        Dim Periods As Integer = 1

        Try

            If Me.ddlPeriods.SelectedValue <> "ALL" Then
                iPeriod = Me.ddlPeriods.SelectedValue.ToString()
            Else
                iPeriod = Periods
            End If

            Wstr = "vWorkSpaceId = '" & Me.HProjectId.Value.Trim() & "' AND cStatusIndi<>'D' And iPeriod = '" + iPeriod + "' And isnull(vTemplateId,0)<>'0001' Order By iNodeNo,iNodeid"

            If Not objhelp.GetViewWorkSpaceNodeDetail(Wstr, dsActivity, eStr_Retu) Then
                Me.objCommon.ShowAlert("Error while getting Information: " & eStr_Retu, Me.Page)
                Exit Function
            End If

            dtActivity = dsActivity.Tables(0)
            dvActivity = New DataView(dtActivity)
            dvActivity.RowFilter = "iParentNodeId = 1"
            tvActivity.Nodes.Clear()
            ActNodeAll.Text = "All Activity*"
            ActNodeAll.Value = "All Activity"
            Me.tvActivity.Nodes.Add(ActNodeAll)
            For ParentNode = 0 To dvActivity.Count - 1
                Dim nodeActivity As New TreeNode()

                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                    nodeActivity.Text = dvActivity(ParentNode).Item("vNodeDisplayName").ToString()
                    nodeActivity.ToolTip = dvActivity(ParentNode).Item("vNodeDisplayName").ToString()
                Else
                    nodeActivity.Text = dvActivity(ParentNode).Item("vActivityName").ToString()
                    nodeActivity.ToolTip = dvActivity(ParentNode).Item("vActivityName").ToString()
                End If
                nodeActivity.Value = dvActivity(ParentNode).Item("iParentNodeId").ToString() + "#" + dvActivity(ParentNode).Item("iNodeNo").ToString() + "#" + dvActivity(ParentNode).Item("iNodeId").ToString() + "#" + dvActivity(ParentNode).Item("vActivityId").ToString()

                nodeActivity.SelectAction = TreeNodeSelectAction.None
                nodeActivity.ChildNodes.Add(nodeActivity)
                dvChild = New DataView(dtActivity)

                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                    dvChild.RowFilter = "iParentNodeId=" + dvActivity(ParentNode).Item("iNodeId").ToString()
                    dvChild.Sort = "iNodeId"
                Else
                    dvChild.RowFilter = "iParentNodeId=" + dvActivity(ParentNode).Item("vActivityId").ToString()
                    dvChild.Sort = "vActivityId"
                End If

                For ChildNode = 0 To dvChild.Count - 1
                    Dim nodeChild As New TreeNode()
                    If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                        nodeChild.Text = dvChild(ChildNode).Item("vNodeDisplayName").ToString()
                        nodeChild.ToolTip = dvChild(ChildNode).Item("vNodeDisplayName").ToString()
                    Else
                        nodeChild.Text = dvChild(ChildNode).Item("vActivityName").ToString()
                        nodeChild.ToolTip = dvChild(ChildNode).Item("vActivityName").ToString()
                    End If
                    nodeChild.Value = dvChild(ChildNode).Item("iParentNodeId").ToString() + "#" + dvChild(ChildNode).Item("iNodeNo").ToString() + "#" + dvChild(ChildNode).Item("iNodeId").ToString() + "#" + dvChild(ChildNode).Item("vActivityId").ToString()

                    nodeChild.SelectAction = TreeNodeSelectAction.None
                    nodeActivity.ChildNodes.Add(nodeChild)
                Next 'Next Child Node
                Me.tvActivity.Nodes(0).ChildNodes.Add(nodeActivity)
            Next 'Newxt Parent Node
            Me.tvActivity.Nodes(0).Expand()
            Me.tvActivity.Nodes(0).SelectAction = TreeNodeSelectAction.None
            Me.divActivity.Style.Add("display", "block")
            Return True

        Catch ex As Exception
            eStr = ex.Message
            Return False
        Finally
            dsActivity.Dispose()
            dtActivity.Dispose()
        End Try
    End Function

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR>" + eStr
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR>" + eStr)
    End Sub

#End Region


#Region "Reset Page"

    Private Sub ResetPage()
        Try
            Me.txtproject.Text = ""
            Me.ddlPeriods.Items.Clear()
            Me.tvUserType.Nodes.Clear()
            Me.tvActivity.Nodes.Clear()
            Me.tdHRUpper.Style.Add("display", "none")
            Me.tdHRLower.Style.Add("display", "none")
            Me.divUserType.Style.Add("display", "none")
            Me.tvUserType.Style.Add("Height", "0px")
            Me.divActivity.Style.Add("display", "none")
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "clear", "clear();", True)

            'If Not Me.GenCall() Then
            '    Exit Sub
            'End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...ResetPage")
        End Try
    End Sub

#End Region

    Protected Sub BtnExit_Click(sender As Object, e As EventArgs) Handles BtnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.ResetPage()
    End Sub

#Region "Assign values"
    Private Function AssignValues() As Boolean
        Dim activity As Integer
        Dim subActivity As Integer
        Dim userType As Integer
        Dim count As Integer
        Dim dr As DataRow
        Dim iPeriod As String = String.Empty
        Dim Periods As Integer = 1

        Try

            If Me.ddlPeriods.SelectedValue <> "ALL" Then
                iPeriod = Me.ddlPeriods.SelectedValue.ToString()
            Else
                iPeriod = Periods
            End If

            dt_ProjectActivityOpertationMatrix.Clear()
            dt_ProjectActivityOpertationMatrix.Columns.Clear()
            dt_ProjectActivityOpertationMatrix.Rows.Clear()

            dt_ProjectActivityOpertationMatrix.Columns.Add("vProjectActivityOperationId")
            dt_ProjectActivityOpertationMatrix.Columns.Add("vWorkSpaceId")
            dt_ProjectActivityOpertationMatrix.Columns.Add("iPeriod")
            dt_ProjectActivityOpertationMatrix.Columns.Add("iParentNodeId")
            dt_ProjectActivityOpertationMatrix.Columns.Add("iNodeNo")
            dt_ProjectActivityOpertationMatrix.Columns.Add("iNodeId")
            dt_ProjectActivityOpertationMatrix.Columns.Add("vActivityId")
            dt_ProjectActivityOpertationMatrix.Columns.Add("vUserTypeCode")
            dt_ProjectActivityOpertationMatrix.Columns.Add("vRemark")
            dt_ProjectActivityOpertationMatrix.Columns.Add("cStatusIndi")
            dt_ProjectActivityOpertationMatrix.Columns.Add("iModifyBy")
            dt_ProjectActivityOpertationMatrix.Columns.Add("DATAOPMODE")

            For userType = 0 To tvUserType.Nodes(0).ChildNodes.Count - 1
                If tvUserType.Nodes(0).ChildNodes(userType).Checked = True Then
                    For activity = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                        If tvActivity.Nodes(0).ChildNodes(activity).Checked = True Then
                            Dim userTypeVal As String = String.Empty
                            Dim arrayActivityval() As String
                            arrayActivityval = tvActivity.Nodes(0).ChildNodes(activity).Value.Split("#")
                            dr = dt_ProjectActivityOpertationMatrix.NewRow()
                            dr("vWorkSpaceId") = Me.HProjectId.Value.ToString().Trim()
                            dr("iPeriod") = iPeriod.ToString().Trim()
                            dr("iParentNodeId") = arrayActivityval(0).ToString().Trim()
                            dr("iNodeNo") = arrayActivityval(1).ToString().Trim()
                            dr("iNodeId") = arrayActivityval(2).ToString().Trim()
                            dr("vActivityId") = arrayActivityval(3).ToString().Trim()
                            dr("vUserTypeCode") = tvUserType.Nodes(0).ChildNodes(userType).Value.ToString().Trim()
                            dr("cStatusIndi") = Status_New
                            dr("iModifyBy") = Me.Session(S_UserID)
                            dr("DATAOPMODE") = 1
                            dt_ProjectActivityOpertationMatrix.Rows.Add(dr)
                        End If
                    Next

                    For activity = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                        For subActivity = 0 To tvActivity.Nodes(0).ChildNodes(activity).ChildNodes.Count - 1
                            If tvActivity.Nodes(0).ChildNodes(activity).ChildNodes(subActivity).Checked = True Then

                                Dim arrayActivityval() As String
                                arrayActivityval = tvActivity.Nodes(0).ChildNodes(activity).ChildNodes(subActivity).Value.Split("#")

                                dr = dt_ProjectActivityOpertationMatrix.NewRow()
                                dr("vWorkSpaceId") = Me.HProjectId.Value.ToString().Trim()
                                dr("iPeriod") = iPeriod.ToString().Trim()
                                dr("iParentNodeId") = arrayActivityval(0).ToString().Trim()
                                dr("iNodeNo") = arrayActivityval(1).ToString().Trim()
                                dr("iNodeId") = arrayActivityval(2).ToString().Trim()
                                dr("vActivityId") = arrayActivityval(3).ToString().Trim()
                                dr("vUserTypeCode") = tvUserType.Nodes(0).ChildNodes(userType).Value.ToString().Trim()
                                dr("cStatusIndi") = Status_New
                                dr("iModifyBy") = Me.Session(S_UserID)
                                dr("DATAOPMODE") = 1
                                dt_ProjectActivityOpertationMatrix.Rows.Add(dr)
                            End If
                        Next
                    Next
                End If
            Next

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignValues")
            Return False
        Finally

        End Try
    End Function
#End Region

    Protected Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        Dim estr As String = String.Empty
        Dim tvUserTypeCount As Integer
        Dim tvActivityCount As Integer
        Try
            If Not AssignValues() Then
                Exit Sub
            End If
            ds_save = New DataSet
            dt_ProjectActivityOpertationMatrix.TableName = "ProjectActivityOperationMatrix"
            ds_save.Tables.Add(dt_ProjectActivityOpertationMatrix)

            If ds_save.Tables(0).Rows.Count > 0 Then
                If Not objLambda.Save_ProjectActivityOperationMatrix(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_save, Me.Session(S_UserID), estr) Then
                    objCommon.ShowAlert("Error while saving activityOperationMatrix", Me.Page)
                    Exit Sub
                End If
            End If

            objCommon.ShowAlert("Record Saved SuccessFully.!", Me.Page)

            tvUserTypeCount = tvUserType.CheckedNodes.Count
            If tvUserTypeCount > 0 Then
                For index = 0 To tvUserTypeCount - 1
                    Dim node As New TreeNode
                    node = tvUserType.CheckedNodes(0)
                    node.Checked = False
                Next
            End If

            tvActivityCount = tvActivity.CheckedNodes.Count
            If tvActivityCount > 0 Then
                For index = 0 To tvActivityCount - 1
                    Dim node As New TreeNode
                    node = tvActivity.CheckedNodes(0)
                    node.Checked = False
                Next
            End If

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ProjectActivityOperationDetails", "projectActivityOperationDetails();", True)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...BtnSave_Click")
        Finally
        End Try
    End Sub

    Protected Sub ddlPeriods_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPeriods.SelectedIndexChanged
        Dim eStr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim returnData As String = String.Empty
        Dim dsProjectActivityOperationDetails As New DataSet
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Try
            If tvUserType.Nodes.Count = 0 Then
                If Not BindUserTypeTree(eStr) Then
                    Throw New Exception(eStr)
                    'Exit Sub
                End If
            End If

            If Not BindActivityTree(eStr) Then
                Throw New Exception(eStr)
            End If

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ProjectActivityOperationDetails", "projectActivityOperationDetails();", True)

        Catch ex As Exception
        Finally
        End Try
    End Sub
 
    <WebMethod> _
    Public Shared Function getprojectActivityOperationDetails(ByVal vWorkSpaceId As String, ByVal iPeriod As Integer) As String
        Dim wstr As String = String.Empty
        Dim returnData As String = String.Empty
        Dim dsProjectActivityOperationDetails As New DataSet
        Dim objcommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
        Dim iMode As String = "1"
        Try
            wstr = vWorkSpaceId.ToString() + "##" + iPeriod.ToString() + "##" + iMode.ToString()
            dsProjectActivityOperationDetails = objHelp.ProcedureExecute("dbo.Get_ProjectActivityOperationMatrix", wstr)
            returnData = JsonConvert.SerializeObject(dsProjectActivityOperationDetails)
            Return returnData
        Catch ex As Exception
            Return ex.ToString()
        Finally
        End Try
    End Function

    <WebMethod> _
    Public Shared Function manageProjectActivityOperationDetails(ByVal mode As Integer, ByVal vProjectActivityOperationId As Integer, ByVal session As Integer, ByVal vRemark As String) As String
        Dim wstr As String = String.Empty
        Dim returnData As String = String.Empty
        Dim ds_ProjectActivityOperationDetails As New DataSet
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim ds_save As DataSet
        Dim dt_ProjectActivityOperationDetails As New DataTable
        Dim dr As DataRow
        Dim estr As String = String.Empty
        Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()


        Try
            dt_ProjectActivityOperationDetails.Clear()

            dt_ProjectActivityOperationDetails.Columns.Add("vProjectActivityOperationId")
            dt_ProjectActivityOperationDetails.Columns.Add("vWorkSpaceId")
            dt_ProjectActivityOperationDetails.Columns.Add("iPeriod")
            dt_ProjectActivityOperationDetails.Columns.Add("iParentNodeId")
            dt_ProjectActivityOperationDetails.Columns.Add("iNodeNo")
            dt_ProjectActivityOperationDetails.Columns.Add("iNodeId")
            dt_ProjectActivityOperationDetails.Columns.Add("vActivityId")
            dt_ProjectActivityOperationDetails.Columns.Add("vUserTypeCode")
            dt_ProjectActivityOperationDetails.Columns.Add("vRemark")
            dt_ProjectActivityOperationDetails.Columns.Add("iModifyBy")
            dt_ProjectActivityOperationDetails.Columns.Add("cStatusIndi")
            dt_ProjectActivityOperationDetails.Columns.Add("DATAOPMODE")

            dr = dt_ProjectActivityOperationDetails.NewRow()

            dr("vProjectActivityOperationId") = vProjectActivityOperationId
            dr("vRemark") = vRemark
            dr("iModifyBy") = session
            dr("cStatusIndi") = "D"
            dr("DATAOPMODE") = mode

            dt_ProjectActivityOperationDetails.Rows.Add(dr)
            dt_ProjectActivityOperationDetails.AcceptChanges()

            ds_save = New DataSet
            dt_ProjectActivityOperationDetails.TableName = "ProjectActivityOperationMatrix"
            ds_save.Tables.Add(dt_ProjectActivityOperationDetails)

            If ds_save.Tables(0).Rows.Count > 0 Then
                If Not objLambda.Save_ProjectActivityOperationMatrix(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_save, session, estr) Then
                    Return returnData = JsonConvert.SerializeObject("ERROR")
                    ''Exit Function
                End If
            End If
            returnData = JsonConvert.SerializeObject("SUCCESS")
            Return returnData
        Catch ex As Exception
        Finally

        End Try
    End Function

    <WebMethod>
    Public Shared Function AuditTrail(ByVal vWorkSpaceId As String, ByVal iPeriod As Integer) As String
        Dim objcommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim returnData As String = String.Empty
        Dim ds_ProjectActivityOperationMatrixAuditTrail As New DataSet
        Dim iMode As String = "2"
        Try
            wStr = vWorkSpaceId.ToString() + "##" + iPeriod.ToString() + "##" + iMode.ToString()
            ds_ProjectActivityOperationMatrixAuditTrail = objHelp.ProcedureExecute("dbo.Get_ProjectActivityOperationMatrix", wStr)
            returnData = JsonConvert.SerializeObject(ds_ProjectActivityOperationMatrixAuditTrail)
            Return returnData
        Catch ex As Exception
            Return ex.ToString()
        Finally
        End Try

    End Function

End Class
