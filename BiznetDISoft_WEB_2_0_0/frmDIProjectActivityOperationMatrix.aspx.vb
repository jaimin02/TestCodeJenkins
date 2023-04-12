Imports System.Drawing
Imports System.Web.Services
Imports Aspose.Pdf.Facades
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf
Imports Newtonsoft.Json
'Imports Winnovative

Partial Class frmDIProjectActivityOperationMatrix
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
                Dim sender As New Object
                Dim e As New EventArgs
                'AutoCompleteExtender1.ContextKey = "isnull(vParentWorkspaceId,'') = ''"
                If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                    Me.txtproject.Text = Session(S_ProjectName)
                    Me.HProjectId.Value = Session(S_ProjectId)
                    btnSetProject_Click(sender, e)
                End If
                AutoCompleteExtender1.ContextKey = "iUserId = '" + Session(S_UserID) + "'"
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

            If Not objhelp.GetWorkspaceProtocolDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                        ds_Periods, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_Periods Is Nothing Then
                Throw New Exception("Problem while getting data")
            End If

            If Not ds_Periods.Tables(0).Rows(0)("iNoOfPeriods") Is Nothing Then
                Periods = ds_Periods.Tables(0).Rows(0)("iNoOfPeriods")
                For count As Integer = 0 To Periods - 1
                    Me.ddlPeriods.Items.Add((count + 1).ToString)
                Next count
            End If

            Me.ddlPeriods.Items.Insert(0, "ALL")

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Legend", "legendUI();", True)
            Return True

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
        Dim dtUserType As New DataTable
        Dim dsSubject As DataSet = New DataSet
        Dim dtTempUserDtl As New DataTable
        Dim dt_UserDtl As New DataTable
        Dim dvUserType As New DataView
        Dim dvUserChild As New DataView
        Dim ds_UserType As New Data.DataSet
        Dim ds_UserDtl As New DataSet
        Dim Wstr As String = String.Empty
        Dim nodeAll As New TreeNode()

        Try

            Me.tdHRUpper.Style.Add("display", "none")
            Me.tdHRLower.Style.Add("display", "none")
            Me.divUserType.Style.Add("display", "none")
            Me.tvUserType.Style.Add("Height", "0px")

            If Not objhelp.GetViewUserWiseProfile("cStatusIndi <> '" + Status_Delete + "' Order By vUserTypeName", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_UserDtl, eStr) Then
                Return False
            End If
            dtTempUserDtl.Columns.Add("vUserTypeCode", GetType(String))
            dtTempUserDtl.Columns.Add("vUserTypeName", GetType(String))

            Dim results = (From row In ds_UserDtl.Tables(0).AsEnumerable()
                           Select vUserTypeCode = row.Field(Of String)("vUserTypeCode"), vUserTypeName = row.Field(Of String)("vUserTypeName")
               ).Distinct().ToList()

            For Each dataRow In results
                Dim row As DataRow = dtTempUserDtl.NewRow()
                row("vUserTypeCode") = dataRow.vUserTypeCode
                row("vUserTypeName") = dataRow.vUserTypeName
                dtTempUserDtl.Rows.Add(row)
            Next

            If Not dtTempUserDtl Is Nothing Then
                If dtTempUserDtl.Rows.Count > 0 Then
                    dt_UserDtl = ds_UserDtl.Tables(0)
                    tvUserType.Nodes.Clear()
                    nodeAll.Text = "All User Details*"
                    nodeAll.Value = "All User Details"
                    Me.tvUserType.Nodes.Add(nodeAll)

                    For ParentNode = 0 To dtTempUserDtl.Rows.Count - 1
                        Dim nodeUserType As New TreeNode()

                        If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                            nodeUserType.Text = dtTempUserDtl(ParentNode).Item("vUserTypeName").ToString()
                            nodeUserType.ToolTip = dtTempUserDtl(ParentNode).Item("vUserTypeName").ToString()
                        Else
                            nodeUserType.Text = dtTempUserDtl(ParentNode).Item("vUserTypeName").ToString()
                            nodeUserType.ToolTip = dtTempUserDtl(ParentNode).Item("vUserTypeName").ToString()
                        End If
                        nodeUserType.Value = dtTempUserDtl(ParentNode).Item("vUserTypeCode").ToString()

                        nodeUserType.SelectAction = TreeNodeSelectAction.None
                        nodeUserType.ChildNodes.Add(nodeUserType)
                        dvUserChild = New DataView(dt_UserDtl)

                        For ChildNode = 0 To dvUserChild.Count - 1
                            Dim nodeChild As New TreeNode()
                            If dtTempUserDtl(ParentNode).Item("vUserTypeCode").ToString() = dvUserChild(ChildNode).Item("vUserTypeCode").ToString() Then
                                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                                    nodeChild.Text = dvUserChild(ChildNode).Item("vUserName").ToString()
                                    nodeChild.ToolTip = dvUserChild(ChildNode).Item("vUserName").ToString()
                                Else
                                    nodeChild.Text = dvUserChild(ChildNode).Item("vUserName").ToString()
                                    nodeChild.ToolTip = dvUserChild(ChildNode).Item("vUserName").ToString()
                                End If
                                nodeChild.Value = dvUserChild(ChildNode).Item("iUserId").ToString()

                                nodeChild.SelectAction = TreeNodeSelectAction.None
                                nodeUserType.ChildNodes.Add(nodeChild)
                            End If

                        Next 'Next Child Node
                        Me.tvUserType.Nodes(0).ChildNodes.Add(nodeUserType)
                    Next 'Newxt Parent Node
                    Me.tvUserType.Nodes(0).Expand()
                    Me.tvUserType.Nodes(0).SelectAction = TreeNodeSelectAction.None
                    Me.divUserType.Style.Add("display", "block")
                    Return True
                End If
            End If


            Return True
        Catch ex As Exception
            eStr = ex.Message
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

                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Or Me.Session(S_ScopeNo) = "6" Then ''For CT and ALL scope
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
            'Me.divSubject.Style.Add("display", "block")
            Return True

        Catch ex As Exception
            eStr = ex.Message
            Return False
        Finally
            dsActivity.Dispose()
            dtActivity.Dispose()
        End Try
    End Function

    Protected Function BindSubjectTree(ByRef eStr As String) As Boolean
        Dim strqry As String = String.Empty
        Dim whrCon As String = String.Empty
        Dim dsSubject As DataSet = New DataSet
        Dim dtSubject As New DataTable
        Dim ds_Subject As New Data.DataSet
        Dim Wstr As String = String.Empty
        Dim Periods As Integer = 1
        Dim iPeriod As String = String.Empty

        Try

            Me.tdHRUpper.Style.Add("display", "none")
            Me.tdHRLower.Style.Add("display", "none")
            Me.divSubject.Style.Add("display", "none")
            Me.tvSubject.Style.Add("Height", "0px")

            If Me.ddlPeriods.SelectedValue <> "ALL" Then
                iPeriod = Me.ddlPeriods.SelectedValue.ToString()
            Else
                iPeriod = Periods
            End If

            Wstr = "vWorkSpaceId = '" & Me.HProjectId.Value.Trim() & "' AND cStatusIndi<>'D' And iPeriod = '" + iPeriod + "'"
            If Not objhelp.GetWorkspaceSubjectMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Subject, eStr) Then
                Return False
            End If

            If Not ds_Subject Is Nothing Then
                If ds_Subject.Tables(0).Rows.Count > 0 Then
                    dtSubject = ds_Subject.Tables(0)
                    tvSubject.Nodes.Clear()
                    Dim nodeAll As New TreeNode()
                    nodeAll.Text = "Subject*"
                    nodeAll.Value = "Subject"

                    Me.tvSubject.Nodes.Add(nodeAll)
                    For index = 0 To dtSubject.Rows.Count - 1
                        Dim nodeSubject As New TreeNode()
                        nodeSubject.Text = dtSubject.Rows(index).Item("vMySubjectNo").ToString()
                        nodeSubject.ToolTip = dtSubject.Rows(index).Item("vMySubjectNo").ToString()
                        nodeSubject.Value = dtSubject.Rows(index).Item("vMySubjectNo").ToString()
                        nodeSubject.SelectAction = TreeNodeSelectAction.None
                        nodeSubject.ChildNodes.Add(nodeSubject)
                        Me.tvSubject.Nodes(0).ChildNodes.Add(nodeSubject)
                    Next ' Next Indexs
                    Me.tvSubject.Nodes(0).ExpandAll()
                    Me.tvSubject.Nodes(0).SelectAction = TreeNodeSelectAction.None
                    Me.tdHRUpper.Style.Add("display", "")
                    Me.tdHRLower.Style.Add("display", "")
                    Me.divSubject.Style.Add("display", "block")
                    Me.tvSubject.Style.Add("Height", "100px")
                Else
                    objCommon.ShowAlert("No Subject Found!", Me.Page)
                End If
            End If


            Return True
        Catch ex As Exception
            eStr = ex.Message
            Return False
        Finally
            dsSubject.Dispose()
            dtSubject.Dispose()
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
            Me.divSubject.Style.Add("display", "none")

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "clear", "clear();", True)

            If Not Me.GenCall() Then
                Exit Sub
            End If
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
        Dim subject As Integer
        Dim subActivity As Integer
        Dim userType As Integer
        Dim count As Integer
        Dim dr As DataRow
        Dim iPeriod As String = String.Empty
        Dim Periods As Integer = 1
        Dim checkedUserProfileFlag As Boolean = True
        Dim checkedUserFlag As Boolean = True

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
            dt_ProjectActivityOpertationMatrix.Columns.Add("vMySubjectNo")
            dt_ProjectActivityOpertationMatrix.Columns.Add("vUserTypeCode")
            dt_ProjectActivityOpertationMatrix.Columns.Add("iUserId")
            dt_ProjectActivityOpertationMatrix.Columns.Add("vRemark")
            dt_ProjectActivityOpertationMatrix.Columns.Add("cStatusIndi")
            dt_ProjectActivityOpertationMatrix.Columns.Add("iModifyBy")
            dt_ProjectActivityOpertationMatrix.Columns.Add("DATAOPMODE")

            For userType = 0 To tvUserType.Nodes(0).ChildNodes.Count - 1
                If tvUserType.Nodes(0).ChildNodes(userType).Checked = True Then
                    'If checkedUserProfileFlag = True Then
                    '    checkedUserProfileFlag = False
                    'End If
                    For subuser = 0 To tvUserType.Nodes(0).ChildNodes(userType).ChildNodes.Count - 1
                        If tvUserType.Nodes(0).ChildNodes(userType).ChildNodes(subuser).Checked = True Then
                            'If checkedUserFlag = True Then
                            '    checkedUserFlag = False
                            'End If
                            For activity = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                                If tvActivity.Nodes(0).ChildNodes(activity).Checked = True Then
                                    'For subject = 0 To tvSubject.Nodes(0).ChildNodes.Count - 1
                                    'If tvSubject.Nodes(0).ChildNodes(subject).Checked = True Then
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
                                    dr("vMySubjectNo") = "" 'tvSubject.Nodes(0).ChildNodes(subject).Value.ToString().Trim()
                                    dr("vUserTypeCode") = tvUserType.Nodes(0).ChildNodes(userType).Value.ToString().Trim()
                                    dr("iUserId") = tvUserType.Nodes(0).ChildNodes(userType).ChildNodes(subuser).Value.ToString().Trim()
                                    dr("cStatusIndi") = Status_New
                                    dr("iModifyBy") = Me.Session(S_UserID)
                                    dr("DATAOPMODE") = 1
                                    dt_ProjectActivityOpertationMatrix.Rows.Add(dr)
                                    'End If
                                    'Next
                                End If
                            Next

                            For activity = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                                For subActivity = 0 To tvActivity.Nodes(0).ChildNodes(activity).ChildNodes.Count - 1
                                    If tvActivity.Nodes(0).ChildNodes(activity).ChildNodes(subActivity).Checked = True Then
                                        'For subject = 0 To tvSubject.Nodes(0).ChildNodes.Count - 1
                                        'If tvSubject.Nodes(0).ChildNodes(subject).Checked = True Then
                                        Dim arrayActivityval() As String
                                        arrayActivityval = tvActivity.Nodes(0).ChildNodes(activity).ChildNodes(subActivity).Value.Split("#")

                                        dr = dt_ProjectActivityOpertationMatrix.NewRow()
                                        dr("vWorkSpaceId") = Me.HProjectId.Value.ToString().Trim()
                                        dr("iPeriod") = iPeriod.ToString().Trim()
                                        dr("iParentNodeId") = arrayActivityval(0).ToString().Trim()
                                        dr("iNodeNo") = arrayActivityval(1).ToString().Trim()
                                        dr("iNodeId") = arrayActivityval(2).ToString().Trim()
                                        dr("vActivityId") = arrayActivityval(3).ToString().Trim()
                                        dr("vMySubjectNo") = "" 'tvSubject.Nodes(0).ChildNodes(subject).Value.ToString().Trim()
                                        dr("vUserTypeCode") = tvUserType.Nodes(0).ChildNodes(userType).Value.ToString().Trim()
                                        dr("iUserId") = tvUserType.Nodes(0).ChildNodes(userType).ChildNodes(subuser).Value.ToString().Trim()
                                        dr("cStatusIndi") = Status_New
                                        dr("iModifyBy") = Me.Session(S_UserID)
                                        dr("DATAOPMODE") = 1
                                        dt_ProjectActivityOpertationMatrix.Rows.Add(dr)
                                        'End If
                                        'Next
                                    End If
                                Next
                            Next
                            'Else
                            '    checkedUserFlag = True
                        End If
                    Next
                    'Else
                    '    checkedUserProfileFlag = True

                End If
            Next

            'If checkedUserFlag = True Or checkedUserProfileFlag = True Then
            '    objCommon.ShowAlert("Please Select User with Profile", Me.Page)
            '    Return False
            'End If

            'If checkedUserProfileFlag = False Then
            '    objCommon.ShowAlert("Please Select UserProfile.!", Me.Page)
            '    Return False
            'End If

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
        Dim msgstr As String = String.Empty
        Dim tvUserTypeCount As Integer
        Dim tvActivityCount As Integer
        Dim tvSubjectCount As Integer
        Dim cnt As Integer
        Dim dsProjectActivityOperationDetails As New DataSet
        Dim dtActivity As New DataTable
        Try
            If Not AssignValues() Then
                Exit Sub
            End If
            ds_save = New DataSet
            dt_ProjectActivityOpertationMatrix.TableName = "ProjectActivityOperationMatrix"
            ds_save.Tables.Add(dt_ProjectActivityOpertationMatrix)

            dsProjectActivityOperationDetails = Session("activitydata")
            dtActivity = dsProjectActivityOperationDetails.Tables(0)
            'cnt = 0
            'For index = 1 To dtActivity.Rows.Count - 1
            '    For i = 0 To dt_ProjectActivityOpertationMatrix.Rows.Count - 1
            '        If (dtActivity.Rows(index)("iParentNodeId").ToString() <> "1" AndAlso dt_ProjectActivityOpertationMatrix.Rows(i)("iParentNodeId").ToString() <> "1") Then

            '            If (dtActivity.Rows(index)("iParentNodeId").ToString() = dt_ProjectActivityOpertationMatrix.Rows(i)("iParentNodeId").ToString() AndAlso
            '            dtActivity.Rows(index)("iNodeId").ToString() = dt_ProjectActivityOpertationMatrix.Rows(i)("iNodeId").ToString() AndAlso
            '            dtActivity.Rows(index)("vActivityId").ToString() = dt_ProjectActivityOpertationMatrix.Rows(i)("vActivityId").ToString()) Then
            '                cnt = cnt + 1
            '                msgstr += cnt.ToString() + ". " + dtActivity.Rows(index)("vUserTypeName").ToString() + " profile have already rights of activity " + dtActivity.Rows(index)("vNodeDisplayName").ToString() + " for user " + dtActivity.Rows(index)("vUserName").ToString() + ".\n"
            '            End If
            '        End If
            '    Next
            'Next
            'If Not String.IsNullOrEmpty(msgstr) Then
            '    objCommon.ShowAlert("Rights Already Exist.\n" + msgstr, Me.Page)
            '    Return
            'End If

            If ds_save.Tables(0).Rows.Count > 0 Then
                If Not objLambda.Save_DISoftProjectActivityOperationMatrix(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_save, Me.Session(S_UserID), estr) Then
                    objCommon.ShowAlert("Error while saving activityOperationMatrix", Me.Page)
                    Exit Sub
                End If
                objCommon.ShowAlert("Record Saved SuccessFully.!", Me.Page)
            Else
                objCommon.ShowAlert("Please Select User with Profile", Me.Page)
                Exit Sub
            End If


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

            tvSubjectCount = tvSubject.CheckedNodes.Count
            If tvSubjectCount > 0 Then
                For index = 0 To tvSubjectCount - 1
                    Dim node As New TreeNode
                    node = tvSubject.CheckedNodes(0)
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

            If Not BindSubjectTree(eStr) Then
                Throw New Exception(eStr)
            End If

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ProjectActivityOperationDetails", "projectActivityOperationDetails();", True)

        Catch ex As Exception
        Finally
        End Try
    End Sub

    <WebMethod>
    Public Shared Function getprojectActivityOperationDetails(ByVal vWorkSpaceId As String, ByVal iPeriod As Integer) As String
        Dim wstr As String = String.Empty
        Dim returnData As String = String.Empty
        Dim dsProjectActivityOperationDetails As New DataSet
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim iMode As String = "1"
        Try
            wstr = vWorkSpaceId.ToString() + "##" + iPeriod.ToString() + "##" + iMode.ToString()
            dsProjectActivityOperationDetails = objHelp.ProcedureExecute("dbo.Get_DISoftprojectactivityoperationmatrix", wstr)
            returnData = JsonConvert.SerializeObject(dsProjectActivityOperationDetails)
            HttpContext.Current.Session("activitydata") = dsProjectActivityOperationDetails
            Return returnData
        Catch ex As Exception
            Return ex.ToString()
        Finally
        End Try
    End Function

    <WebMethod>
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
            dt_ProjectActivityOperationDetails.Columns.Add("vMySubjectNo")
            'dt_ProjectActivityOperationDetails.Columns.Add("vSubjectId")
            dt_ProjectActivityOperationDetails.Columns.Add("vUserTypeCode")
            dt_ProjectActivityOperationDetails.Columns.Add("iUserId")
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
            dt_ProjectActivityOperationDetails.TableName = "DISoftProjectActivityOperationMatrix"
            ds_save.Tables.Add(dt_ProjectActivityOperationDetails)

            If ds_save.Tables(0).Rows.Count > 0 Then
                If Not objLambda.Save_DISoftProjectActivityOperationMatrix(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_save, session, estr) Then
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
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim returnData As String = String.Empty
        Dim ds_ProjectActivityOperationMatrixAuditTrail As New DataSet
        Dim iMode As String = "2"
        Try
            wStr = vWorkSpaceId.ToString() + "##" + iPeriod.ToString() + "##" + iMode.ToString()
            ds_ProjectActivityOperationMatrixAuditTrail = objHelp.ProcedureExecute("dbo.Get_DISoftprojectactivityoperationmatrix", wStr)
            returnData = JsonConvert.SerializeObject(ds_ProjectActivityOperationMatrixAuditTrail)
            Return returnData
        Catch ex As Exception
            Return ex.ToString()
        Finally
        End Try

    End Function

#Region "Export Audit Record PDF"
    <WebMethod>
    Public Shared Function exportToPDF(ByVal vWorkSpaceId As String, ByVal iPeriod As Integer,ByVal ProjectNo As String) As String
        Dim wstr = "", eStr As String = ""
        Dim ds_check As New DataSet
        Dim ds_MedexScreeningHdrDtl As New DataSet
        Dim ds_project As New DataSet
        Dim strpdf As String = ""
        Dim ds_exportpdf As New DataTable
        Dim headercontent As String = String.Empty
        Dim foldername As String = ""
        Dim fInfo As FileInfo = Nothing
        Dim Periods As Integer = 1
        Dim iMode As String = "1"
        Dim ds_ProjectActivityOperationMatrixAuditTrail As New DataSet
        Dim returnData As String = String.Empty
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim dtAuditTrail As New DataTable
        Dim dt As New DataTable
        Dim drAuditTrail As DataRow
        Dim gvAuditTrial As New GridView
        Dim i As Integer = 1
        Dim filename As String = String.Empty
        Dim strModified As String = ""
        Try
            'If Me.ddlPeriods.SelectedValue <> "ALL" Then
            '    iPeriod = Me.ddlPeriods.SelectedValue.ToString()
            'Else
            '    iPeriod = Periods
            'End If
            wstr = vWorkSpaceId.ToString() + "##" + iPeriod.ToString() + "##" + iMode.ToString()
            ds_ProjectActivityOperationMatrixAuditTrail = objhelp.ProcedureExecute("dbo.Get_DISoftprojectactivityoperationmatrix", wstr)

            If ds_ProjectActivityOperationMatrixAuditTrail Is Nothing Or ds_ProjectActivityOperationMatrixAuditTrail.Tables.Count = 0 Or ds_ProjectActivityOperationMatrixAuditTrail.Tables(0) Is Nothing Or ds_ProjectActivityOperationMatrixAuditTrail.Tables(0).Rows.Count = 0 Then
                Throw New Exception(eStr)
                Exit Function
            End If

            If Not dtAuditTrail Is Nothing Then
                dtAuditTrail.Columns.Add("UserProfiles")
                dtAuditTrail.Columns.Add("Username")
                dtAuditTrail.Columns.Add("ParentActivity")
                dtAuditTrail.Columns.Add("Activity")
                dtAuditTrail.Columns.Add("Modifyby")
                dtAuditTrail.Columns.Add("ModifyOn")
            End If

            dtAuditTrail.AcceptChanges()
            dt = ds_ProjectActivityOperationMatrixAuditTrail.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtAuditTrail.NewRow()
                drAuditTrail("UserProfiles") = dr("vUserTypeName").ToString()
                drAuditTrail("Username") = dr("vUserName").ToString()
                drAuditTrail("ParentActivity") = dr("vParentNodeDisplayName").ToString()
                drAuditTrail("Activity") = dr("vNodeDisplayName").ToString()
                drAuditTrail("Modifyby") = dr("iModifyBy").ToString()
                drAuditTrail("ModifyOn") = dr("dModifyOn").ToString()
                dtAuditTrail.Rows.Add(drAuditTrail)
                dtAuditTrail.AcceptChanges()
                i += 1
            Next
            gvAuditTrial.DataSource = dtAuditTrail
            gvAuditTrial.DataBind()
            If gvAuditTrial.Rows.Count > 0 Then
                Dim gridviewHtml As String = String.Empty
                filename = "Project Activity Operation rights Audit Trail_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf"
                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                gvAuditTrial.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()
                HttpContext.Current.Response.ContentType = "application/pdf"
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + filename)

                'ProjectNo added by Bhargav Thaker 01March2023
                Dim HeaderDiv As String
                HeaderDiv = "<div>" +
                            "  <table cellspacing='0' cellpadding='0' rules='all' style='border-collapse:collapse;width:100%;'>" +
                            "	<tr>" +
                            "	<th scope='col' colspan='4' align='center'><strong><b><font color='#000099' size='4' face='Verdana, Arial, Helvetica, sans-serif'>Project Activity Operation rights Audit Report</font></strong></b></th>" +
                            "	</tr>" +
                            "	<tr>" +
                            "	<th scope='col' colspan='4' align='center'><strong><b><font color='#000099' size='3' face='Verdana, Arial, Helvetica, sans-serif'>" + System.Configuration.ConfigurationManager.AppSettings("Client") + "</font></strong></b></th>" +
                            "	</tr>" +
                            "	<tr>" +
                            "	<th scope='col' colspan='4' align='center'><strong><b><font color='#000099' size='2' face='Verdana, Arial, Helvetica, sans-serif'><b>Project No:- </b>" + ProjectNo.Trim() + "</font></strong></b></th>" +
                            "	</tr>" +
                            "	<tr>" +
                            "	<th align='right'><font color='#000099' size='2' face='Verdana'><b>Print Date:-</b></font></th><th align = 'left'><font color='#000099' size='2' face='Verdana'><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></th>" +
                            "   <th align='Right'><font color='#000099' size='2' face='Verdana'><b>Printed By:-</b></font></th><th align = 'left'><font color='#000099' size='2' face='Verdana'><b>" + HttpContext.Current.Session(S_UserNameWithProfile) + "</b></font></th>" +
                            "	</tr>" +
                            "	</table>" +
                            "</div>"
                Dim sr As New StringReader(HeaderDiv + stringWriter.ToString())
                Dim pdfDoc As New Document(iTextSharp.text.PageSize.A4.Rotate(), 30.0F, 30.0F, 50.0F, 0.0F)
                Dim htmlparser As New HTMLWorker(pdfDoc)
                Dim msOutput As New MemoryStream()
                Dim writer1 As PdfWriter = PdfWriter.GetInstance(pdfDoc, msOutput)
                pdfDoc.Open()
                htmlparser.Parse(sr)
                'htmlparser.Parse(New StringReader(gridviewHtml.ToString()))
                pdfDoc.Close()
                Dim filebytes As Byte() = msOutput.ToArray()
                strModified = Convert.ToBase64String(filebytes, 0, filebytes.Length)
            End If

            Return strModified
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return strModified
    End Function
#End Region
End Class
