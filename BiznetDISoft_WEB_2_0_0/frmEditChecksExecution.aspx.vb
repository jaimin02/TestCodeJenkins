
Partial Class frmEditChecksExecution
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()


    Private Const vSrNo As Integer = 0
    Private Const vSubjectId As Integer = 1
    Private Const vInitials As Integer = 2
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
    Private Const cIsQuery As Integer = 16
    Private Const nCRFDtlNo As Integer = 17
    Private Const vResolveRemark As Integer = 20
    Private Const nEditChecksDtlNo As Integer = 21
    Private Const iMySubjectNo As Integer = 22
    Private Const vProjectTypeCode As Integer = 23


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
        'add by shivani pandya for latest repeatition
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

            ''Me.ddlActivity.Items.Clear()
            'wstr = "SELECT DISTINCT WorkSpaceNodeDetail.vNodeDisplayName, WorkSpaceNodeDetail.iNodeNo,WorkSpaceNodeDetail.iNodeId,WorkSpaceNodeDetail.vActivityId,WorkSpaceNodeDetail.iPeriod,WorkSpaceNodeDetail.iParentNodeId "
            'wstr += " FROM MedExWorkspaceEditChecks "
            'wstr += " INNER JOIN workspacemst" + _
            '        " ON((WorkspaceMst.vParentWorkspaceId = MedExWorkspaceEditChecks.vWorkspaceId" + _
            '        " OR WorkspaceMst.vWorkspaceId = MedExWorkspaceEditChecks.vWorkspaceId) " + _
            '        " AND WorkspaceMst.cStatusIndi <> 'D' ) "
            'wstr += " INNER JOIN WorkSpaceNodeDetail "
            'wstr += " ON(WorkSpaceNodeDetail.vWorkspaceId=MedExWorkspaceEditChecks.vWorkSpaceId"
            'wstr += " AND MedExWorkspaceEditChecks.iNodeId=WorkspaceNodeDetail.iNodeId "
            'wstr += " AND MedExWorkspaceEditChecks.iPeriod=WorkspaceNodeDetail.iPeriod "
            'wstr += " AND MedExWorkspaceEditChecks.cStatusIndi<>'D'"
            'wstr += " AND WorkSpaceNodeDetail.cStatusIndi<>'D' "


            ''If chkAll.Checked = True Then
            ''    wstr += " )"
            ''Else
            ''    wstr += " AND MedExWorkspaceEditChecks.cEditCheckType='C')"
            ''End If
            'If cEditCheckType = "W" Then
            '    rblEditCheckType.Items(0).Selected = True
            '    wstr += " )"
            'Else
            '    rblEditCheckType.Items(1).Selected = True
            '    wstr += " AND MedExWorkspaceEditChecks.cEditCheckType='C')"
            'End If

            'wstr += " WHERE workspacemst.vWorkspaceId='" + Me.HProjectId.Value + "'"
            'wstr += " AND WorkspaceNodeDetail.cStatusIndi<>'D'"
            'wstr += " ORDER BY iPeriod,iNodeNo,iNodeId "

            'ds_Acivity = objHelp.GetResultSet(wstr, "MedExWorkspaceEditChecks")

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



            'If Not ds_Acivity Is Nothing AndAlso ds_Acivity.Tables(0).Rows.Count > 0 Then
            '    Me.ddlActivity.DataSource = ds_Acivity.Tables(0)
            '    Me.ddlActivity.DataTextField = "vNodeDisplayName"
            '    Me.ddlActivity.DataValueField = "iNodeId"
            '    Me.ddlActivity.DataBind()
            '    Me.ddlActivity.Items.Insert(0, "Select Activity")
            'End If

            If Not ds_Acivity.Tables(0) Is Nothing AndAlso ds_Acivity.Tables(0).Rows.Count > 0 Then

                Dim Dv_Acivity As New DataView
                Me.chkLstActivity.Items.Clear()

                For Each dr As DataRow In ds_Acivity.Tables(0).Rows

                    lItem = New ListItem
                    lItem.Text = Convert.ToString(dr("ActivityWithParent"))
                    lItem.Value = dr("iNodeId")
                    Me.chkLstActivity.Items.Add(lItem)

                    'If dr("cRejectionFlag") = "Y" Then
                    '    Me.chkLstSubjects.Items(index - 1).Attributes.Add("Style", "Color:Red")
                    'End If

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
        Try

            For Each item In Me.chkLstActivity.Items
                If item.Selected Then
                    strActivityList += item.Value.Trim() + ","
                End If
            Next item
            If strActivityList <> "" Then
                strActivityList = strActivityList.Substring(0, strActivityList.LastIndexOf(","))
            End If
            'Me.fldGrid.Style.Add("display", "")
            Me.PnlDetail.Style.Add("display", "")
            Me.pnlDetailGrid.Style.Add("display", "")
            Param = Me.HProjectId.Value.ToString + "##" + strActivityList.ToString + "########" + rblEditCheckType.SelectedValue.ToString()
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
                    If dr("dFiredDate").ToString() <> "" Then
                        dr("vFiredDate") = Convert.ToString(CDate(dr("dFiredDate")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                    End If
                    If dr("dResolvedOn").ToString() <> "" Then
                        dr("vResolvedOn") = Convert.ToString(CDate(dr("dResolvedOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                    End If

                Next
                Me.Grid.DataSource = ds_WorkspaceEditChecksHdrDTL.Tables(0)
                Me.Grid.DataBind()
            Else
                Me.Grid.EmptyDataText = "No Data Found"
                Me.Grid.DataBind()
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
        Try
            For Each item In Me.chkLstActivity.Items
                If item.Selected Then
                    strActivityList += item.Value.Trim() + ","
                End If
            Next item
            If strActivityList <> "" Then
                strActivityList = strActivityList.Substring(0, strActivityList.LastIndexOf(","))
            End If
            Param = Me.HProjectId.Value + "##" + Session(S_UserID).ToString + "##" + strActivityList + "##" + "" + "##" + rblEditCheckType.SelectedValue
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
        Try


            For Each grdRow As GridViewRow In Me.Grid.Rows
                If CType(grdRow.Cells(vResolveRemark).FindControl("chkResolvedRemarks"), CheckBox).Checked Then
                    strEditCheckNo += grdRow.Cells(nEditChecksDtlNo).Text + ","
                End If
            Next

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
                        dr("iModifyBy") = Session(S_UserID)
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

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub Grid_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Grid.RowDataBound

        Try
            If e.Row.RowType = DataControlRowType.Header Or _
                       e.Row.RowType = DataControlRowType.DataRow Or _
                       e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(vWorkspaceId).Visible = False
                e.Row.Cells(iNodeId).Visible = False
                e.Row.Cells(vActivityId).Visible = False
                e.Row.Cells(cEditCheckType).Visible = False
                e.Row.Cells(cIsQuery).Visible = False
                e.Row.Cells(nCRFDtlNo).Visible = False
                e.Row.Cells(nEditChecksDtlNo).Visible = False
                e.Row.Cells(iMySubjectNo).Visible = False
                e.Row.Cells(vSubjectId).Visible = False
                If Me.ViewState(IsCTMProject) = "Y" Then
                    e.Row.Cells(iPeriod).Visible = False
                    e.Row.Cells(vProjectTypeCode).Visible = False
                End If
            End If
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(vSrNo).Text = e.Row.RowIndex + (Grid.PageSize * Grid.PageIndex) + 1
                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then                   
                    CType(e.Row.Cells(vNodeDisplayName).FindControl("lbtnActvityName"), LinkButton).OnClientClick = "funOPEN('frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + e.Row.Cells(vWorkspaceId).Text.ToString + _
                                                                                                                            "&ActivityId=" + e.Row.Cells(vActivityId).Text.ToString + "&NodeId=" + e.Row.Cells(iNodeId).Text.ToString + "&PeriodId=" + _
                                                                                                                            e.Row.Cells(iPeriod).Text.ToString + "&SubjectId=" + e.Row.Cells(vSubjectId).Text.ToString + "&MySubjectNo=" + e.Row.Cells(iMySubjectNo).Text.ToString + "&Activityname=" + e.Row.Cells(ParentActivity).Text.ToString + "&ScreenNo=" + e.Row.Cells(vMySubjectNo).Text.ToString + "&mode=')"
                Else
                    CType(e.Row.Cells(vNodeDisplayName).FindControl("lbtnActvityName"), LinkButton).OnClientClick = "funOPEN('frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + e.Row.Cells(vWorkspaceId).Text.ToString + _
                                                                                                                             "&ActivityId=" + e.Row.Cells(vActivityId).Text.ToString + "&NodeId=" + e.Row.Cells(iNodeId).Text.ToString + "&PeriodId=" + _
                                                                                                                              e.Row.Cells(iPeriod).Text.ToString + "&SubjectId=" + e.Row.Cells(vSubjectId).Text.ToString + "&Activityname=" + e.Row.Cells(ParentActivity).Text.ToString + "&MySubjectNo=" + e.Row.Cells(iMySubjectNo).Text.ToString + "&mode=&Type=BA-BE')"
                End If

                If (CType(e.Row.Cells(vResolveRemark).FindControl("lblResolveRemark"), Label).Text.Trim <> "") Then
                    CType(e.Row.Cells(vResolveRemark).FindControl("chkResolvedRemarks"), CheckBox).Visible = False
                Else
                    e.Row.Cells(vResolveRemark).VerticalAlign = VerticalAlign.Middle
                    e.Row.Cells(vResolveRemark).HorizontalAlign = HorizontalAlign.Center
                    'CType(e.Row.Cells(vResolveRemark).FindControl("chkResolvedRemarks"), CheckBox).Attributes.Add("onClick", "displayBackGround(" + e.Row.Cells(nEditChecksDtlNo).Text.ToString + ")")
                    CType(e.Row.Cells(vResolveRemark).FindControl("lblResolveRemark"), Label).Visible = False
                End If

            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "Button Click Events"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim ds_CrfLockDetail As DataSet = Nothing
        Dim wstr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Try
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
            If Not BindActivity("W") Then
                Throw New Exception("Error while executing BindActivity")
            End If
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

    'Protected Sub chkAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAll.CheckedChanged
    '    If Not BindActivity("C") Then

    '    End If
    'End Sub

    Protected Sub rblEditCheckType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblEditCheckType.SelectedIndexChanged
        'If ddlEditCheckType.SelectedValue <> "V" Then
        '    Me.btnExecute.Style.Add("display", "none")
        'Else
        '    Me.btnExecute.Style.Add("display", "")
        'End If
        Dim cEditCheckFlag As Char
        Try
            If rblEditCheckType.SelectedValue = "P" Then
                cEditCheckFlag = "W"
            Else
                cEditCheckFlag = "C"
            End If
            chkAll.Checked = False
            If Not BindActivity(cEditCheckFlag) Then
                Throw New Exception("Error while executing BindActivity")
            End If
        Catch ex As Exception

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

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "ResolveAll...")
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try

            Me.txtProject.Text = ""
            Me.HProjectId.Value = ""
            'Me.ddlEditCheckType.SelectedIndex = -1
            'Me.ddlActivity.Items.Clear()
            'Me.ddlActivity.Items.Insert(0, "Select Activity")
            Me.chkAll.Checked = False
            Me.Grid.DataSource = Nothing
            Me.Grid.DataBind()

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
            strMessage.Append("<td colspan=""14""><center><strong><font color=""black"" size=""9px"" face=""Calibri"">")
            strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""14""><center><strong><font color=""black"" size=""8px"" face=""Calibri"">")
            strMessage.Append("Edit Check Execution Details")
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")
            strMessage.Append("<td><strong><font color=""black"" size=""9px"" face=""Calibri"">")
            strMessage.Append("Project/Site")
            strMessage.Append("</font></strong></td>")
            strMessage.Append("<td colspan=""13""><strong><font color=""black"" size=""9px"" face=""Calibri"">")
            strMessage.Append(Me.txtProject.Text.Trim().Substring(1, Me.txtProject.Text.Trim().IndexOf("]") - 1))
            strMessage.Append("</font></strong></td>")
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")


            dt.Columns.Add("vSrNo")
            For Each dr In dt.Rows
                dr("vSrNo") = SrNo
                SrNo += 1
            Next


            dtConvert = dt.DefaultView.ToTable(True, "vSrNo,vInitials,vMySubjectNo,vRandomizationNo,iRepeatNo,vNodeDisplayName,iPeriod,vQueryMessage,vErrorMessage,cEditCheckTypeDisplay,vFiredDate,vResolvedRemark,vResolvedOn,vResolvedBy".Split(","))
            dtConvert.AcceptChanges()

            dtConvert.Columns(0).ColumnName = "#"
            dtConvert.Columns(1).ColumnName = "Initials"
            dtConvert.Columns(2).ColumnName = "Screen No"
            dtConvert.Columns(3).ColumnName = "Patient/Randomization No"
            dtConvert.Columns(4).ColumnName = "Repeat No"
            dtConvert.Columns(5).ColumnName = "Activity"
            dtConvert.Columns(6).ColumnName = "Period"
            dtConvert.Columns(7).ColumnName = "Edit Check Formula"
            dtConvert.Columns(8).ColumnName = "Discrepancy Message"
            dtConvert.Columns(9).ColumnName = "Type"
            dtConvert.Columns(10).ColumnName = "Executed On"
            dtConvert.Columns(11).ColumnName = "Reason/ Remark"
            dtConvert.Columns(12).ColumnName = "Resolved On"
            dtConvert.Columns(13).ColumnName = "Resolved By"

            dtConvert.AcceptChanges()

            For iCol = 0 To dtConvert.Columns.Count - 1

                strMessage.Append("<td style=""text-align:center;""><strong><font color=""Black"" size=""9px"" face=""Calibri"">")
                strMessage.Append(Convert.ToString(dtConvert.Columns(iCol)).Trim())
                strMessage.Append("</font></strong></td>")

            Next
            strMessage.Append("</tr>")

            For j = 0 To dtConvert.Rows.Count - 1
                strMessage.Append("<tr>")
                For i = 0 To dtConvert.Columns.Count - 1
                    If i = 0 Then
                        strMessage.Append("<td style=""text-align:center;vertical-align:top;""><font color=""black"" size=""2"" face=""Calibri"">")
                    Else
                        strMessage.Append("<td style=""text-align:left;vertical-align:top;""><font color=""black"" size=""2"" face=""Calibri"">")
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

End Class
