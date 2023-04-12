Imports java.net

Partial Class frmEditChecksExecution_New
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

    Private Const vSrNo As Integer = 0
    Private Const vSubjectId As Integer = 1
    Private Const vProjectNo As Integer = 2
    Private Const vMySubjectNo As Integer = 3
    Private Const vRandomizationNo As Integer = 4
    Private Const iRepeatNo As Integer = 5
    Private Const ParentActivity As Integer = 6
    Private Const vNodeDisplayName As Integer = 7
    Private Const vWorkspaceId As Integer = 8
    Private Const iNodeId As Integer = 9
    Private Const vActivityId As Integer = 10
    Private Const iPeriod As Integer = 11
    Private Const vQueryMessage As Integer = 12
    Private Const vErrorMessage As Integer = 13
    Private Const cEditCheckType As Integer = 14
    Private Const dFiredDate As Integer = 15
    Private Const vFiredOn As Integer = 16
    Private Const cIsQuery As Integer = 17
    Private Const nCRFDtlNo As Integer = 18
    Private Const vResolveRemark As Integer = 21
    Private Const nEditChecksDtlNo As Integer = 22
    Private Const iMySubjectNo As Integer = 23
    Private Const vProjectTypeCode As Integer = 24


    Private Const CTMProject As String = "0014"
    Private Const IsCTMProject As String = "N"
    Private Const Vs_WorkspaceEditChecksHdrDTL As String = "WorkspaceEditChecksHdrDTL"

    Private Const Vs_isLocked As Boolean = False
#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.Request.QueryString("Mode") Is Nothing Then
            btnExecute.Visible = False
        End If
        If Not Page.IsPostBack Then
            Me.Page.Title = ":: Edit Checks Execution ::" + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Edit Checks Execution"
            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtProject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
            End If
        End If
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "bindgrid", "BindDatatable();", True)
        Me.Session(S_SelectedRepeatation) = ""
    End Sub

#End Region

#Region "Bind acitivity"

    Private Function BindActivity(ByVal cEditCheckType As Char) As Boolean
        Dim ds_Acivity As DataSet = Nothing
        Dim wstr As String = String.Empty
        Dim lItem As ListItem
        Dim WorkSpaceId As String = Me.HProjectId.Value
        Dim EditCheckType As String = ""
        Dim eStr As String = ""
        Try

            If cEditCheckType = "W" Then
                rblEditCheckType.Items(0).Selected = True
            Else
                rblEditCheckType.Items(1).Selected = True
                EditCheckType = "C"
            End If

            objHelp.Timeout = -1

            If Not objHelp.Proc_GetEditCheckActivity(WorkSpaceId, EditCheckType, ds_Acivity, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_Acivity.Tables(0) Is Nothing AndAlso ds_Acivity.Tables(0).Rows.Count > 0 Then

                Dim Dv_Acivity As New DataView
                Me.chkLstActivity.Items.Clear()

                For Each dr As DataRow In ds_Acivity.Tables(0).Rows

                    lItem = New ListItem
                    lItem.Text = Convert.ToString(dr("ActivityWithParent"))
                    lItem.Value = dr("iNodeId")
                    Me.chkLstActivity.Items.Add(lItem)

                Next dr

            End If

            btnExecute.Visible = True
            If rblEditCheckType.SelectedValue = "P" Or ViewState(Vs_isLocked) = True Or Not Me.Request.QueryString("Mode") Is Nothing Then
                btnExecute.Visible = False
            End If

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "Bind Grid"

    Private Function BindGrid() As Boolean
        Dim ds_WorkspaceEditChecksHdrDTL As DataSet = Nothing
        Dim estr As String = String.Empty
        Dim strActivityList As String = String.Empty
        Dim Param As String = String.Empty

        Dim strSubjectId As String = String.Empty
        Dim strParentID As String = String.Empty
        Dim strActivityId As String = String.Empty

        Try

            For Each item In Me.chkLstActivity.Items
                If item.Selected Then
                    strActivityList += item.Value.Trim() + ","
                End If
            Next item
            If strActivityList <> "" Then
                strActivityList = strActivityList.Substring(0, strActivityList.LastIndexOf(","))
            End If

            '' Add by Ketan
            If tvSubject.Nodes.Count = 0 Then
                objCommon.ShowAlert("No Subject Found For This Project!", Me.Page)
                Exit Function
            End If
            If tvSubject.Nodes(0).Checked = False Then
                For index = 0 To tvSubject.Nodes(0).ChildNodes.Count - 1
                    If tvSubject.Nodes(0).ChildNodes(index).Checked = True Then
                        strSubjectId = strSubjectId + tvSubject.Nodes(0).ChildNodes(index).Value + ","
                    End If
                Next 'Next Index
                If strSubjectId <> "" Then
                    strSubjectId.Remove(strSubjectId.Length - 1)
                Else
                    objCommon.ShowAlert("Please Select Subject !", Me.Page)
                    Exit Function
                End If

            End If

            If tvActivity.Nodes(0).Checked = False Then
                For index = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                    If tvActivity.Nodes(0).ChildNodes(index).Checked = True Then
                        strParentID = strParentID + tvActivity.Nodes(0).ChildNodes(index).Value + ","
                    End If
                Next
            End If
            If strParentID <> "" Then
                strParentID.Remove(strParentID.Length - 1)
                strParentID = strParentID + "'"
            End If
            If tvActivity.Nodes(0).Checked = False Then
                For iParent = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                    For iChild = 0 To tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes.Count - 1
                        If tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Checked = True Then
                            strActivityId = strActivityId + tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Value + ","
                        End If
                    Next 'Next iChild
                Next 'Next iParent
            End If

            If strActivityId <> "" Then
                strActivityId.Remove(strActivityId.Length - 1)
                strActivityId = strActivityId + strParentID + "'"
            Else
                If strParentID <> "" Then
                    strActivityId = strParentID + "'"
                Else
                    If tvActivity.Nodes(0).Checked = False Then
                        objCommon.ShowAlert("Please Select Activity !", Me.Page)
                        Exit Function
                    End If
                End If
            End If

            strActivityList = strActivityId

            If strActivityList <> "" Then
                strActivityList = strActivityList.Substring(0, strActivityList.LastIndexOf(","))
            End If

            If strSubjectId <> "" Then
                strSubjectId = strSubjectId.Substring(0, strSubjectId.LastIndexOf(","))
            End If


            Me.PnlDetail.Style.Add("display", "")
            Me.pnlDetailGrid.Style.Add("display", "")
            Param = Convert.ToString(Me.HProjectId.Value) + "##" + Convert.ToString(strActivityList) + "####" + Convert.ToString(strSubjectId) + "####" + Convert.ToString(rblEditCheckType.SelectedValue)
            ds_WorkspaceEditChecksHdrDTL = objHelp.ProcedureExecute("PROC_Workspaceeditcheckshdrdtl", Param)
            If ds_WorkspaceEditChecksHdrDTL.Tables(0).Rows.Count > 0 Then
                Me.ViewState(Vs_WorkspaceEditChecksHdrDTL) = ds_WorkspaceEditChecksHdrDTL.Tables(0)
                ds_WorkspaceEditChecksHdrDTL.Tables(0).Columns.Add("vFiredDate")
                ds_WorkspaceEditChecksHdrDTL.Tables(0).Columns.Add("vResolvedOn")
                If ds_WorkspaceEditChecksHdrDTL.Tables(0).Rows(0)("vProjectTypeCode") = CTMProject Then
                    Me.ViewState(IsCTMProject) = "Y"
                Else
                    Me.ViewState(IsCTMProject) = "N"
                End If

                For Each dr In ds_WorkspaceEditChecksHdrDTL.Tables(0).Rows
                    If Convert.ToString(dr("dFiredDate")) <> "" Then
                        dr("vFiredDate") = Convert.ToString(CDate(dr("dFiredDate")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                    End If
                    If Convert.ToString(dr("dResolvedOn")) <> "" Then
                        dr("vResolvedOn") = Convert.ToString(CDate(dr("dResolvedOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                    End If

                Next
                Me.Grid.DataSource = ds_WorkspaceEditChecksHdrDTL.Tables(0)
                Me.Grid.DataBind()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "bindgrid", "BindDatatable();", True)
            Else
                Me.btnResolveAll.Visible = False
                Me.Grid.EmptyDataText = Nothing
                Me.Grid.DataBind()
                objCommon.ShowAlert("Data Not Found !", Me.Page)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "bindgrid", "BindDatatable();", True)
            End If
            Return True

        Catch ex As Exception
            Return False
            objCommon.ShowAlert(ex.Message, Me.Page)
        End Try
    End Function

#End Region

#Region "Execute EditChecks"

    Private Function ExecuteEditCheck() As Boolean
        Dim Param As String = String.Empty
        Dim ds_ExecuteEditChecks As DataSet = Nothing
        Dim estr As String = String.Empty
        Dim strActivityList = String.Empty

        Dim strSubjectId As String = String.Empty
        Dim strParentID As String = String.Empty
        Dim strActivityId As String = String.Empty

        Try

            '' Add by Ketan
            If tvSubject.Nodes.Count = 0 Then
                objCommon.ShowAlert("No Subject Found For This Project!", Me.Page)
                Exit Function
            End If
            If tvSubject.Nodes(0).Checked = False Then
                For index = 0 To tvSubject.Nodes(0).ChildNodes.Count - 1
                    If tvSubject.Nodes(0).ChildNodes(index).Checked = True Then
                        strSubjectId = strSubjectId + tvSubject.Nodes(0).ChildNodes(index).Value + ","
                    End If
                Next 'Next Index
                If strSubjectId <> "" Then
                    strSubjectId.Remove(strSubjectId.Length - 1)
                Else
                    objCommon.ShowAlert("Please Select Subject !", Me.Page)
                    Exit Function
                End If

            End If

            If tvActivity.Nodes(0).Checked = False Then
                For index = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                    If tvActivity.Nodes(0).ChildNodes(index).Checked = True Then
                        strParentID = strParentID + tvActivity.Nodes(0).ChildNodes(index).Value + ","
                    End If
                Next
            End If
            If strParentID <> "" Then
                strParentID.Remove(strParentID.Length - 1)
                strParentID = strParentID + "'"
            End If
            If tvActivity.Nodes(0).Checked = False Then
                For iParent = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                    For iChild = 0 To tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes.Count - 1
                        If tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Checked = True Then
                            strActivityId = strActivityId + tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Value + ","
                        End If
                    Next 'Next iChild
                Next 'Next iParent
            End If

            If strActivityId <> "" Then
                strActivityId.Remove(strActivityId.Length - 1)
                strActivityId = strActivityId + strParentID + "'"
            Else
                If strParentID <> "" Then
                    strActivityId = strParentID + "'"
                Else
                    If tvActivity.Nodes(0).Checked = False Then
                        objCommon.ShowAlert("Please Select Activity  !", Me.Page)
                        Exit Function
                    End If
                End If
            End If

            strActivityList = strActivityId

            If strActivityList <> "" Then
                strActivityList = strActivityList.Substring(0, strActivityList.LastIndexOf(","))
            End If

            If strSubjectId <> "" Then
                strSubjectId = strSubjectId.Substring(0, strSubjectId.LastIndexOf(","))
            End If

            Param = Me.HProjectId.Value + "##" + Session(S_UserID).ToString + "##" + strActivityList + "##" + "" + "##" + rblEditCheckType.SelectedValue + "##" + strSubjectId
            objHelp.Timeout = -1
            If Not objHelp.Proc_ExecuteEditChecks(Param, ds_ExecuteEditChecks, estr) Then
                Throw New Exception("Error While Executing EditChecks..." + estr.ToString)
            End If
            Return True
        Catch ex As Exception
            Me.objCommon.ShowAlert(ex.ToString, Me.Page)
            Return False
        End Try
    End Function

#End Region

#Region "Save Remarks"

    Private Function funSaveRemark() As Boolean
        Dim ds_Remark As DataSet = Nothing
        Dim strEditCheckNo As String = String.Empty
        Dim estr As String = String.Empty
        Dim strRemarks As String = String.Empty
        Dim dt As DataTable = New DataTable
        Try

            ''Check For All Pages 
            Dim ChkBoxHeader As CheckBox = DirectCast(Grid.HeaderRow.FindControl("chkAllPages"), CheckBox)
            If ChkBoxHeader.Checked = True Then
                dt = CType(ViewState(Vs_WorkspaceEditChecksHdrDTL), DataTable)
                dt.DefaultView.RowFilter = "cResolvedFlag = 'N'"
                For Each row As DataRow In dt.Rows
                    If Convert.ToString(row(14)) = "N" Then
                        strEditCheckNo += Convert.ToString(row(16)) + ","
                    End If
                Next
            Else
                For Each grdRow As GridViewRow In Me.Grid.Rows
                    If CType(grdRow.Cells(vResolveRemark).FindControl("chkResolvedRemarks"), CheckBox).Checked Then
                        If Replace(Convert.ToString(grdRow.Cells(20).Text), "&nbsp;", "") = "" Then
                            strEditCheckNo += grdRow.Cells(nEditChecksDtlNo).Text + ","
                        End If
                    End If
                Next
            End If

            strEditCheckNo = strEditCheckNo.Substring(0, strEditCheckNo.LastIndexOf(","))

            If Not objHelp.GetData("WorkspaceEditChecksDtl", "*", " nEditChecksDtlNo in (" + strEditCheckNo + ")", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Remark, estr) Then
                Throw New Exception("Error While Getting Blank Structure Of WorkspaceEditChecksDtl")
            End If

            If Me.ddlRemarks.SelectedIndex <> 0 Then
                strRemarks = Me.ddlRemarks.SelectedItem.Text
            Else
                strRemarks = txtRemark.Text.Trim()
            End If

            If Not ds_Remark Is Nothing Then
                If ds_Remark.Tables(0).Rows.Count > 0 Then
                    For Each dr As DataRow In ds_Remark.Tables(0).Rows
                        dr("cResolvedFlag") = "Y"
                        dr("vResolvedRemark") = strRemarks
                        dr("iResolvedBy") = Session(S_UserID)
                    Next
                End If
            End If

            ds_Remark.AcceptChanges()

            If Not objLambda.insert_WorkspaceEditChecksDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_Remark, estr) Then
                Throw New Exception("Error While Saving Remarks Of Resolved Editchecks")
            End If
            Return True
        Catch ex As Exception
            Return False
            objCommon.ShowAlert(ex.ToString, Me.Page)
        End Try

    End Function

#End Region

#Region "Grid Events "

    Protected Sub Grid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles Grid.PageIndexChanging
        Try

            Dim dt As DataTable = Nothing
            Grid.PageIndex = e.NewPageIndex
            dt = CType(ViewState(Vs_WorkspaceEditChecksHdrDTL), DataTable)
            Me.Grid.DataSource = dt
            Me.Grid.DataBind()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "bindgrid", "BindDatatable();", True)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub Grid_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Grid.RowDataBound
        Dim strCellValue As String = String.Empty
        Try
            If e.Row.RowType = DataControlRowType.Header Or _
                       e.Row.RowType = DataControlRowType.DataRow Or _
                       e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(vWorkspaceId).Visible = False
                e.Row.Cells(iNodeId).Visible = False
                e.Row.Cells(vActivityId).Visible = False
                'e.Row.Cells(cEditCheckType).Visible = False
                e.Row.Cells(cIsQuery).Visible = False
                e.Row.Cells(nCRFDtlNo).Visible = False
                e.Row.Cells(nEditChecksDtlNo).Visible = False
                e.Row.Cells(iMySubjectNo).Visible = False
                e.Row.Cells(vSubjectId).Visible = False
                If Me.ViewState(IsCTMProject) = "Y" Then
                    e.Row.Cells(iPeriod).Visible = False
                    e.Row.Cells(vProjectTypeCode).Visible = False
                Else
                    e.Row.Cells(vProjectTypeCode).Visible = False
                End If
            End If
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(vSrNo).Text = e.Row.RowIndex + (Grid.PageSize * Grid.PageIndex) + 1

                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                    CType(e.Row.Cells(vNodeDisplayName).FindControl("lbtnActvityName"), LinkButton).OnClientClick = "funOPEN('frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + e.Row.Cells(vWorkspaceId).Text.ToString + _
                                                                                                                            "&ActivityId=" + e.Row.Cells(vActivityId).Text.ToString + "&NodeId=" + e.Row.Cells(iNodeId).Text.ToString + "&PeriodId=" + _
                                                                                                                            e.Row.Cells(iPeriod).Text.ToString + "&SubjectId=" + e.Row.Cells(vSubjectId).Text.ToString + "&MySubjectNo=" + e.Row.Cells(iMySubjectNo).Text.ToString + "&Activityname=" + URLEncoder.encode(e.Row.Cells(ParentActivity).Text.ToString) + "&ScreenNo=" + e.Row.Cells(vMySubjectNo).Text.ToString + "&mode=')"
                Else
                    CType(e.Row.Cells(vNodeDisplayName).FindControl("lbtnActvityName"), LinkButton).OnClientClick = "funOPEN('frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + e.Row.Cells(vWorkspaceId).Text.ToString + _
                                                                                                                             "&ActivityId=" + e.Row.Cells(vActivityId).Text.ToString + "&NodeId=" + e.Row.Cells(iNodeId).Text.ToString + "&PeriodId=" + _
                                                                                                                              e.Row.Cells(iPeriod).Text.ToString + "&SubjectId=" + e.Row.Cells(vSubjectId).Text.ToString + "&Activityname=" + URLEncoder.encode(e.Row.Cells(ParentActivity).Text.ToString) + "&MySubjectNo=" + e.Row.Cells(iMySubjectNo).Text.ToString + "&mode=&Type=BA-BE')"
                End If


                If (CType(e.Row.Cells(vResolveRemark).FindControl("lblResolveRemark"), Label).Text.Trim <> "") Then
                    CType(e.Row.Cells(vResolveRemark).FindControl("chkResolvedRemarks"), CheckBox).Visible = False
                Else
                    e.Row.Cells(vResolveRemark).VerticalAlign = VerticalAlign.Middle
                    e.Row.Cells(vResolveRemark).HorizontalAlign = HorizontalAlign.Center
                    'CType(e.Row.Cells(vResolveRemark).FindControl("chkResolvedRemarks"), CheckBox).Attributes.Add("onClick", "displayBackGround(" + e.Row.Cells(nEditChecksDtlNo).Text.ToString + ")")
                    CType(e.Row.Cells(vResolveRemark).FindControl("lblResolveRemark"), Label).Visible = False
                End If

                strCellValue = HttpUtility.HtmlDecode(e.Row.Cells(vQueryMessage).Text)   '' For Speical Character
                If strCellValue <> "" Then
                    If strCellValue.Length > 20 Then
                        e.Row.Cells(vQueryMessage).Attributes.Add("title", strCellValue)
                        e.Row.Cells(vQueryMessage).Text = strCellValue.Substring(0, 20) + "..."
                    Else
                        e.Row.Cells(vQueryMessage).Text = strCellValue
                    End If
                End If

                strCellValue = HttpUtility.HtmlDecode(e.Row.Cells(vErrorMessage).Text)
                If strCellValue <> "" Then
                    If strCellValue.Length > 20 Then
                        e.Row.Cells(vErrorMessage).Attributes.Add("title", strCellValue)
                        e.Row.Cells(vErrorMessage).Text = strCellValue.Substring(0, 20) + "..."
                    Else
                        e.Row.Cells(vErrorMessage).Text = strCellValue
                    End If
                End If

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "bindgrid", "BindDatatable();", True)

            End If
        Catch ex As Exception
            Me.objCommon.ShowAlert(ex.ToString, Me.Page)
        End Try
    End Sub

    Protected Sub Grid_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles Grid.RowCommand
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "bindgrid", "BindDatatable();", True)
    End Sub


    Protected Sub chkAllResolvedRemarks_CheckedChanged(sender As Object, e As EventArgs)
        Dim ChkBoxHeader As CheckBox = DirectCast(Grid.HeaderRow.FindControl("chkAllResolvedRemarks"), CheckBox)
        Dim chkAllPages As CheckBox = DirectCast(Grid.HeaderRow.FindControl("chkAllPages"), CheckBox)

        If chkAllPages.Checked = True Then
            ChkBoxHeader.Checked = True
        End If

        For Each row As GridViewRow In Grid.Rows
            Dim ChkBoxRows As CheckBox = DirectCast(row.FindControl("chkResolvedRemarks"), CheckBox)
            If ChkBoxHeader.Checked = True Then
                ChkBoxRows.Checked = True
            Else
                ChkBoxRows.Checked = False
            End If
        Next
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "bindgrid", "BindDatatable();", True)
    End Sub

#End Region

#Region "Button Click Events"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim ds_CrfLockDetail As DataSet = Nothing
        Dim wstr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Try

            Me.divSubject.Style.Add("display", "none")
            Me.divActivity.Style.Add("display", "none")
            Me.tvActivity.Nodes.Clear()
            Me.tvSubject.Nodes.Clear()



            ViewState(Vs_isLocked) = False
            wstr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'"

            If Not objHelp.GetCRFLockDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CrfLockDetail, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If
            If ds_CrfLockDetail.Tables(0).Rows.Count > 0 Then
                ds_CrfLockDetail.Tables(0).DefaultView.RowFilter = " iTranNo = Max(iTranNo)"
                If ds_CrfLockDetail.Tables(0).DefaultView(0)("cLockFlag") = "L" Then
                    objCommon.ShowAlert("Project is Locked.", Me.Page)
                    btnExecute.Visible = False
                    ViewState(Vs_isLocked) = True
                    Me.PnlDetail.Style.Add("display", "none")
                    Me.pnlDetailGrid.Style.Add("display", "none")
                    Me.rblEditCheckType.Items(1).Selected = False
                    Me.rblEditCheckType.Items(0).Selected = False
                    Me.chkLstActivity.Items.Clear()
                    Me.chkAll.Checked = False
                    Exit Sub
                End If
            End If
            'Me.fldGrid.Style.Add("display", "none")
            Me.PnlDetail.Style.Add("display", "none")
            Me.pnlDetailGrid.Style.Add("display", "none")
            Me.rblEditCheckType.Items(1).Selected = False
            Me.rblEditCheckType.Items(0).Selected = False
            Me.chkLstActivity.Items.Clear()
            Me.chkAll.Checked = False

            If Not BindSubjectTree(eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            If Not BindActivityTree(eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            Me.divSubject.Style.Add("display", "block")
            Me.divActivity.Style.Add("display", "block")


            ResetSelection()

        Catch ex As Exception
            objCommon.ShowAlert(ex.Message, Me.Page)
        End Try
    End Sub

    Protected Sub btnExecute_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExecute.Click
        Try
            If Not ExecuteEditCheck() Then
                Throw New Exception("Error While Executing EditChecks...ExecuteEditCheck()")
            End If
        Catch ex As Exception
            Me.objCommon.ShowAlert(ex.ToString, Me.Page)
        End Try
    End Sub

    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnView.Click
        Try
            If Not BindGrid() Then
                Throw New Exception("Error While Binding Grid...BindGrid")
            End If
            If Me.Grid.Rows.Count > 0 Then
                Me.btnExport.Visible = True
                Me.btnResolveAll.Visible = True
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Display", "DisplayFromCodeBehind('ctl00_CPHLAMBDA_img2','divEditCheckCriteria');", True)
        Catch ex As Exception
            objCommon.ShowAlert(ex.ToString, Me.Page)
        End Try
    End Sub

    Protected Sub btnSaveRemark_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveRemark.Click
        Try
            If Not funSaveRemark() Then
                Throw New Exception("Error While Saving Remarks Of Resolved Editchecks...funSaveRemarks()")
            End If
            If Not BindGrid() Then
                Throw New Exception("Error While BindGrid ...BindGrid()")
            End If
            Me.txtRemark.Text = ""
            Me.ddlRemarks.SelectedIndex = -1
        Catch ex As Exception
            objCommon.ShowAlert(ex.ToString, Me.Page)
        End Try
    End Sub

    Protected Sub rblEditCheckOperation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblEditCheckOperation.SelectedIndexChanged
        Dim eStr_Retu As String = String.Empty
        Try
            btnExecute.Visible = True
            If rblEditCheckOperation.SelectedValue = "V" Or ViewState(Vs_isLocked) = True Or Not Me.Request.QueryString("Mode") Is Nothing Then
                btnExecute.Visible = False
            End If

            If rblEditCheckOperation.SelectedValue = "E" Then
                rblEditCheckType.Items(0).Enabled = True
                rblEditCheckType.Items(1).Enabled = True
                rblEditCheckType.Items(2).Enabled = True

                rblEditCheckType.Items(0).Selected = False
                rblEditCheckType.Items(1).Selected = True
                rblEditCheckType.Items(2).Selected = False

                rblEditCheckType.Items(0).Enabled = False
                rblEditCheckType.Items(2).Enabled = False

            ElseIf rblEditCheckOperation.SelectedValue = "V" Then

                rblEditCheckType.Items(0).Enabled = True
                rblEditCheckType.Items(1).Enabled = True
                rblEditCheckType.Items(2).Enabled = True
            End If
            'chkAll.Checked = False
            Me.chkAll.Checked = False
            Me.btnResolveAll.Visible = False
            Me.Grid.EmptyDataText = Nothing
            Me.Grid.DataBind()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "bindgrid", "BindDatatable();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clear_Subject_ACtivity", "clear_Subject_ACtivity();", True)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "Change Operation event...")
        End Try

    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim eStr As String = String.Empty

        Try
            If Me.Grid.Rows.Count < 1 Then
                Me.objCommon.ShowAlert("No data to Export", Me.Page)
                Exit Sub
            End If
            If Not ExportToExcel(CType(Me.ViewState(Vs_WorkspaceEditChecksHdrDTL), DataTable), "WorkspaceEditChecksHdrDtl") Then
                Throw New Exception("Problem while Exporting data")
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Exit Sub
        End Try
    End Sub

    Protected Sub btnResolveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResolveAll.Click
        Dim boolFlag As Boolean = False
        Try
            For Each grdRow As GridViewRow In Me.Grid.Rows
                If CType(grdRow.Cells(vResolveRemark).FindControl("chkResolvedRemarks"), CheckBox).Checked Then
                    boolFlag = True
                    Exit For
                End If
            Next
            If boolFlag Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "strTemp", "displayBackGround();", True)
            Else
                objCommon.ShowAlert("Please select atleast one edit checks to resolve.", Me)
            End If

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "bindgrid", "BindDatatable();", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "ResolveAll...")
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try

            Me.txtProject.Text = ""
            Me.HProjectId.Value = ""
            Me.chkAll.Checked = False
            Me.Grid.DataSource = Nothing
            Me.Grid.DataBind()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "bindgrid", "BindDatatable();", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

#End Region

#Region "Export To Excell"

    Private Function ExportToExcel(ByVal Dt As DataTable, ByVal dtString As String) As Boolean
        Dim fileName As String = ""
        Dim eStr As String = String.Empty
        Try
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            fileName = "Executed Edit Checks"
            fileName = fileName & ".xls"
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            Context.Response.Write(ConvertDtTO(Dt, dtString))

            Context.Response.Flush()
            'Context.Response.Close()    ' Don't Open comment it create a problem in Export to excel
            Context.Response.End()
            HttpContext.Current.ApplicationInstance.CompleteRequest()

            System.IO.File.Delete(fileName)
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Return False
            Exit Function
        End Try

    End Function

    Private Function ConvertDtTO(ByVal dt As DataTable, ByVal dtString As String) As String
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer
        Dim SrNo As Integer = 1
        Dim dtConvert As New DataTable
        Try

            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""16""><center><strong><font color=""black"" size=""9px"" face=""Verdana"">")
            strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""16""><center><strong><font color=""black"" size=""8px"" face=""Verdana"">")
            strMessage.Append("Edit Check Execution Details")
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")
            strMessage.Append("<td><strong><font color=""black"" size=""9px"" face=""Verdana"">")
            strMessage.Append("Project/Site")
            strMessage.Append("</font></strong></td>")
            strMessage.Append("<td colspan=""15""><strong><font color=""black"" size=""9px"" face=""Verdana"">")
            strMessage.Append(Me.txtProject.Text.Trim().Substring(1, Me.txtProject.Text.Trim().IndexOf("]") - 1))
            strMessage.Append("</font></strong></td>")
            strMessage.Append("</tr>")

            strMessage.Append("<tr style=""background-color: #e2e2e2;"">")


            dt.Columns.Add("vSrNo")
            For Each dr In dt.Rows
                dr("vSrNo") = SrNo
                SrNo += 1
            Next


            dtConvert = dt.DefaultView.ToTable(True, "vSrNo,vProjectNo,vMySubjectNo,vRandomizationNo,iRepeatNo,ParentvNodeDisplayName,vNodeDisplayName,iPeriod,vQueryMessage,vErrorMessage,cEditCheckTypeDisplay,vFiredBy,vFiredDate,vResolvedBy,vResolvedOn,vResolvedRemark".Split(","))
            dtConvert.AcceptChanges()

            dtConvert.Columns(0).ColumnName = "SrNo"
            dtConvert.Columns(1).ColumnName = "Project No"
            dtConvert.Columns(2).ColumnName = "Subject No"
            dtConvert.Columns(3).ColumnName = "Randomization No"
            dtConvert.Columns(4).ColumnName = "Repeat No"
            dtConvert.Columns(5).ColumnName = "Parent Activity"
            dtConvert.Columns(6).ColumnName = "Activity"
            dtConvert.Columns(7).ColumnName = "Period"
            dtConvert.Columns(8).ColumnName = "Edit check formula"
            dtConvert.Columns(9).ColumnName = "Discrepancy Message"
            dtConvert.Columns(10).ColumnName = "Edit Check Type"
            dtConvert.Columns(11).ColumnName = "Executed By"
            dtConvert.Columns(12).ColumnName = "Executed On"
            dtConvert.Columns(13).ColumnName = "Resolved By"
            dtConvert.Columns(14).ColumnName = "Resolved On"
            dtConvert.Columns(15).ColumnName = "Reason/Remarks"

            dtConvert.AcceptChanges()

            For iCol = 0 To dtConvert.Columns.Count - 1

                strMessage.Append("<td style=""text-align:center;""><strong><font color=""Black"" size=""9px"" face=""Verdana"">")
                strMessage.Append(Convert.ToString(dtConvert.Columns(iCol)).Trim())
                strMessage.Append("</font></strong></td>")

            Next
            strMessage.Append("</tr>")

            For j = 0 To dtConvert.Rows.Count - 1
                strMessage.Append("<tr>")
                For i = 0 To dtConvert.Columns.Count - 1
                    If i = 0 Then
                        strMessage.Append("<td style=""text-align:center;vertical-align:top;""><font color=""black"" size=""2"" face=""Verdana"">")
                    Else
                        strMessage.Append("<td style=""text-align:left;vertical-align:top;""><font color=""black"" size=""2"" face=""Verdana"">")
                    End If
                    strMessage.Append(Convert.ToString(dtConvert.Rows(j).Item(i)).Trim())
                    strMessage.Append("</font></td>")

                Next
                strMessage.Append("</tr>")
            Next
            strMessage.Append("</table>")

            Return strMessage.ToString

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
            Return ex.Message
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

#Region "Function For Bind Activity and Subject"
    Protected Function BindSubjectTree(ByRef eStr As String) As Boolean
        Dim strqry As String = String.Empty
        Dim whrCon As String = String.Empty
        Dim dsSubject As DataSet = New DataSet
        Dim dtSubject As New DataTable
        Dim period As String = "1"

        Try
            whrCon = " vWorkspaceId='" + Me.HProjectId.Value + "'" _
                    + "AND cStatusIndi <> 'D'  and  iPeriod=" + CInt(period).ToString + " Order by iMySubjectNo"

            Me.divSubject.Style.Add("display", "none")
            Me.tvSubject.Style.Add("Height", "0px")

            If Not objHelp.GetWorkspaceSubjectMst(whrCon, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsSubject, eStr) Then
                Throw New Exception(eStr)
            End If
            If Not dsSubject Is Nothing Then
                If dsSubject.Tables(0).Rows.Count > 0 Then
                    dtSubject = dsSubject.Tables(0)
                    Dim nodeAll As New TreeNode()
                    nodeAll.Text = "All Subject\ScreenNo*"
                    nodeAll.Value = "All Subject\ScreenNo"

                    If (Request.QueryString("ProjectName") <> "" And Request.QueryString("SubjectId") = "") Then
                        nodeAll.Checked = True
                    End If
                    Me.tvSubject.Nodes.Add(nodeAll)
                    For index = 0 To dtSubject.Rows.Count - 1
                        Dim nodeSubject As New TreeNode()
                        nodeSubject.Text = Convert.ToString(dtSubject.Rows(index).Item("vMySubjectNo"))
                        If Convert.ToString(dtSubject.Rows(index).Item("cRejectionFlag")) = "Y" Then
                            nodeSubject.Text = "<font color = red>" + Convert.ToString(dtSubject.Rows(index).Item("vMySubjectNo")) + "</font>"
                        End If
                        nodeSubject.ToolTip = Convert.ToString(dtSubject.Rows(index).Item("vMySubjectNo")) + "|" + Convert.ToString(dtSubject.Rows(index).Item("vSubjectId"))
                        nodeSubject.Value = Convert.ToString(dtSubject.Rows(index).Item("vSubjectId"))
                        nodeSubject.SelectAction = TreeNodeSelectAction.None
                        If (Request.QueryString("ProjectName") <> "" And Request.QueryString("SubjectId") <> "" And Request.QueryString("SubjectId") = Convert.ToString(dtSubject.Rows(index).Item("vSubjectId"))) Then
                            nodeSubject.Checked = True
                        ElseIf (Request.QueryString("ProjectName") <> "" And Request.QueryString("SubjectId") = "") Then
                            nodeSubject.Checked = True
                        End If
                        nodeSubject.ChildNodes.Add(nodeSubject)
                        Me.tvSubject.Nodes(0).ChildNodes.Add(nodeSubject)
                    Next ' Next Index
                    Me.tvSubject.Nodes(0).ExpandAll()
                    Me.tvSubject.Nodes(0).SelectAction = TreeNodeSelectAction.None

                    Me.divSubject.Style.Add("display", "block")
                    Me.tvSubject.Style.Add("Height", "100px")
                Else
                    Me.divSubject.Style.Add("display", "none")
                    objCommon.ShowAlert("No Subject Found For This Project!", Me.Page)
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
        Try

            iPeriod = ""
            Subject_Specific = "Y"

            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                If Not objHelp.Proc_ActivityTreeCTM(Me.HProjectId.Value, iPeriod, Subject_Specific, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, dsActivity, eStr) Then
                    Throw New Exception(eStr)
                End If
            Else
                If Not objHelp.Proc_ActivityTreeBABE(Me.HProjectId.Value, iPeriod, Subject_Specific, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, dsActivity, eStr) Then
                    Throw New Exception(eStr)
                End If
            End If
            dtActivity = dsActivity.Tables(0)
            dvActivity = New DataView(dtActivity)
            dvActivity.RowFilter = "TreeLevel=0"
            ActNodeAll.Text = "All Activity*"
            ActNodeAll.Value = "All Activity"
            Me.tvActivity.Nodes.Add(ActNodeAll)
            If (Request.QueryString("ProjectName") <> "") Then
                ActNodeAll.Checked = True
            End If
            For ParentNode = 0 To dvActivity.Count - 1
                Dim nodeActivity As New TreeNode()
                nodeActivity.Text = Convert.ToString(dvActivity(ParentNode).Item("Name"))
                nodeActivity.ToolTip = Convert.ToString(dvActivity(ParentNode).Item("Name"))
                nodeActivity.Value = Convert.ToString(dvActivity(ParentNode).Item("Id"))
                If (Request.QueryString("ProjectName") <> "") Then
                    nodeActivity.Checked = True
                End If
                nodeActivity.SelectAction = TreeNodeSelectAction.None
                nodeActivity.ChildNodes.Add(nodeActivity)
                dvChild = New DataView(dtActivity)
                dvChild.RowFilter = "Treelevel=1 AND ParentId=" + Convert.ToString(dvActivity(ParentNode).Item("Id"))
                dvChild.Sort = "iNodeNo"
                For ChildNode = 0 To dvChild.Count - 1
                    Dim nodeChild As New TreeNode()
                    nodeChild.Text = Convert.ToString(dvChild(ChildNode)("Name"))
                    nodeChild.ToolTip = Convert.ToString(dvChild(ChildNode)("Name"))
                    nodeChild.Value = Convert.ToString(dvChild(ChildNode)("Id"))
                    If (Request.QueryString("ProjectName") <> "") Then
                        nodeChild.Checked = True
                    End If
                    nodeChild.SelectAction = TreeNodeSelectAction.None
                    nodeActivity.ChildNodes.Add(nodeChild)
                Next 'Next Child Node
                Me.tvActivity.Nodes(0).ChildNodes.Add(nodeActivity)
            Next 'Newxt Parent Node
            Me.tvActivity.Nodes(0).Expand()
            Me.tvActivity.Nodes(0).SelectAction = TreeNodeSelectAction.None
            Return True

        Catch ex As Exception
            eStr = ex.Message
            Return False
        Finally
            dsActivity.Dispose()
            dtActivity.Dispose()
        End Try
    End Function

    Protected Function ResetSelection() As Boolean

        rblEditCheckType.Items(0).Enabled = True
        rblEditCheckType.Items(1).Enabled = True
        rblEditCheckType.Items(2).Enabled = True

        rblEditCheckType.Items(0).Selected = True
        rblEditCheckType.Items(1).Selected = False
        rblEditCheckType.Items(2).Selected = False

        rblEditCheckOperation.Items(0).Enabled = True
        rblEditCheckOperation.Items(1).Enabled = True

        rblEditCheckOperation.Items(0).Selected = True
        rblEditCheckOperation.Items(1).Selected = False

        Me.btnExecute.Visible = False

        Return True
    End Function
#End Region


End Class
