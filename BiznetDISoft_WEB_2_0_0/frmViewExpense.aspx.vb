
Partial Class frmViewExpense
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objCommon As New clsCommon
    Private ObjLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
    Private ObjHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private Const VS_dtOtherExpDetailGV As String = "dt_OtherExpDetailGV"

    'For GV_OtherExpDetail Final Grid
    Private Const GVC_OtherExpDetail_OtherExpHdrNo As Integer = 0
    Private Const GVC_OtherExpDetail_OtherExpDtlNo As Integer = 1
    Private Const GVC_OtherExpDetail_FromDate As Integer = 2
    Private Const GVC_OtherExpDetail_ToDate As Integer = 3
    Private Const GVC_OtherExpDetail_FromDay As Integer = 4
    Private Const GVC_OtherExpDetail_ToDay As Integer = 5
    Private Const GVC_OtherExpDetail_Project As Integer = 6
    Private Const GVC_OtherExpDetail_Site As Integer = 7
    Private Const GVC_OtherExpDetail_ExpType As Integer = 8
    Private Const GVC_OtherExpDetail_Amt As Integer = 9
    Private Const GVC_OtherExpDetail_Remarks As Integer = 10
    Private Const GVC_OtherExpDetail_Ref As Integer = 11
    Private Const GVC_OtherExpDetail_Attachment As Integer = 12
    Private Const GVC_OtherExpDetail_Total As Integer = 13
    Private Const GVC_OtherExpDetail_Delete As Integer = 14
    Private Const GVC_OtherExpDetail_Approve As Integer = 15
    Private Const GVC_OtherExpDetail_Reject As Integer = 16
    Private Const GVC_OtherExpDetail_Flag As Integer = 17
    Private Const GVC_OtherExpDetail_ApprovalUserName As Integer = 18

#End Region

#Region "Form Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
        If Not IsPostBack Then

            If Not GenCall_ShowUI() Then
                Me.objCommon.ShowAlert("Error While Getting Data Or Filling Data", Me.Page)
                Exit Sub
            End If

        End If
    End Sub

#End Region

#Region "GenCall_ShowUI"

    Private Function GenCall_ShowUI() As Boolean
        Try

            Page.Title = ":: View Expense  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")


            CType(Me.Master.FindControl("lblHeading"), Label).Text = "View Expense Detail Report"

            If Not fillUser() Then
                Return False
            End If

            Me.txtFromDate.Text = Today.Date.ToString("dd-MMM-yyyy")
            Me.txtToDate.Text = Today.Date.ToString("dd-MMM-yyyy")
            Me.BtnExport.Visible = False

            GenCall_ShowUI = True
        Catch ex As Exception
            GenCall_ShowUI = False
        End Try
    End Function

#End Region

#Region "FillDropDown"

    Private Function fillUser() As Boolean
        Dim DsUser As New DataSet
        Dim Wstr As String = ""
        Dim Estr As String = ""

        Try

            Me.ddlUser.Visible = False
            Me.lblUser.Visible = False

            If Me.Session(S_UserGroup) = 3 Or Me.Session(S_UserGroup) = 4 Then

                Wstr = "vUserTypeCode='" & Me.Session(S_UserType).ToString.Trim() & "'"

                If Not Me.ObjHelp.GetViewUserMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                DsUser, Estr) Then

                    Me.objCommon.ShowAlert("Error While Getting Data from View_UserMst", Me.Page)
                    Return False

                End If

                If DsUser.Tables(0).Rows.Count < 1 Then
                    objCommon.ShowAlert("No Record Found", Me.Page)
                    Return True
                End If

                Me.ddlUser.Visible = True
                Me.lblUser.Visible = True

                DsUser.Tables(0).DefaultView.Sort = "vUserName"
                Me.ddlUser.DataSource = DsUser.Tables(0).DefaultView.ToTable()
                Me.ddlUser.DataTextField = "vUserName"
                Me.ddlUser.DataValueField = "iUserId"
                Me.ddlUser.DataBind()

            End If

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "FillGrid"

    Protected Function FillGrid() As Boolean
        Dim Wstr As String
        Dim Estr As String = ""
        Dim dS_OtherExpDetailGV As New DataSet
        Dim dt_OtherExpDetailGV As New DataTable

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Me.HFromDate.Value = Me.txtFromDate.Text
            Me.HToDate.Value = Me.txtToDate.Text

            Wstr = "iUserId=" & Me.Session(S_UserID).ToString.Trim()

            If Me.ddlUser.Visible = True Then
                Wstr = "iUserId=" & Me.ddlUser.SelectedValue.Trim()
            End If

            Wstr += " AND (cast(dFromDate as datetime) >= '" & Me.HFromDate.Value.Trim() & "'"
            Wstr += " AND cast(dToDate as datetime) <= '" & Me.HToDate.Value.Trim() & ""
            Wstr += "') And cActiveflag = 'Y' and (cApprovalFlag is NULL OR cApprovalFlag<>'R')"

            If Not Me.ObjHelp.GetViewUserWiseExpense(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                dS_OtherExpDetailGV, Estr) Then

                Me.objCommon.ShowAlert("Error while getting Information from View_UserWiseOtherExp", Me.Page)
                Return False

            End If

            If dS_OtherExpDetailGV.Tables(0).Rows.Count < 1 Then

                Me.objCommon.ShowAlert("No Record Found", Me.Page)
                Me.BtnExport.Visible = False
                Return True

            End If

            Me.BtnExport.Visible = True

            dS_OtherExpDetailGV.Tables(0).DefaultView.Sort = "dFromDate"
            Me.ViewState(VS_dtOtherExpDetailGV) = dS_OtherExpDetailGV.Tables(0).DefaultView.ToTable()
            Me.GV_OtherExpDetail.DataSource = dS_OtherExpDetailGV.Tables(0).DefaultView
            Me.GV_OtherExpDetail.DataBind()

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "", ex)
        End Try
    End Function

#End Region

#Region "Button Events"

    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGo.Click
        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub

    Protected Sub BtnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ReportHeader As String = ""
        Dim Dt_Export As New DataTable
        Dim Ds_Export As New DataSet

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            Dt_Export = CType(Me.ViewState(VS_dtOtherExpDetailGV), DataTable)

            If Not Dt_Export Is Nothing Then
                Dt_Export.Columns.Remove("nOtherExpHdrNo")
                Dt_Export.Columns.Remove("nOtherExpDtlNo")
                Dt_Export.Columns.Remove("iSrNo")
                Dt_Export.Columns.Remove("nSTPNo")
                Dt_Export.Columns.Remove("nOtherExpMstNo")
                Dt_Export.Columns.Remove("vWorkspaceId")
                Dt_Export.Columns.Remove("cActiveflag")
                Dt_Export.Columns.Remove("iUserId")
                Dt_Export.Columns.Remove("iUserGroupCode")
                Dt_Export.Columns.Remove("nScopeNo")
                Dt_Export.Columns.Remove("vUserName")
                Dt_Export.Columns.Remove("vFirstName")
                Dt_Export.Columns.Remove("vLastName")
                Dt_Export.Columns.Remove("vLoginName")
                Dt_Export.Columns.Remove("vLoginPass")
                Dt_Export.Columns.Remove("vUserTypeCode")
                Dt_Export.Columns.Remove("vDeptCode")
                Dt_Export.Columns.Remove("vLocationCode")
                Dt_Export.Columns.Remove("vEmailId")
                Dt_Export.Columns.Remove("vPhoneNo")
                Dt_Export.Columns.Remove("vExtNo")

                Dt_Export.Columns("dFromDate").ColumnName = "FromDate"
                Dt_Export.Columns("vFromDay").ColumnName = "FromDay"
                Dt_Export.Columns("dToDate").ColumnName = "ToDate"
                Dt_Export.Columns("vToDay").ColumnName = "ToDay"

                Dt_Export.Columns("vWorkspaceDesc").ColumnName = "Project"
                Dt_Export.Columns("vSiteName").ColumnName = "Site"
                Dt_Export.Columns("vCityName").ColumnName = "City"
                Dt_Export.Columns("vOtherExpName").ColumnName = "Expense"
                Dt_Export.Columns("iExpAmt").ColumnName = "Amount"
                Dt_Export.Columns("vRemarks").ColumnName = "Remark"
                Dt_Export.Columns("vRefDetail").ColumnName = "Ref. Detail"
                Dt_Export.Columns("vAttachment").ColumnName = "Attachment"
                Dt_Export.Columns("iTotalExpAmt").ColumnName = "Total Expense"

                Dt_Export.AcceptChanges()
                Ds_Export.Tables.Add(Dt_Export.Copy())

                ReportHeader = "<table style=""border:solid 1px black;font-name:Arial; font-size:10pt; color:navy; font-weight:bold;"" " + _
                               " cellspacing=""0px"" cellpadding=""5px"" >"
                ReportHeader += "<tbody><tr><td colspan=""13"" align=center >View Expence Detail Report<br/>" + _
                                "User: " + Me.Session(S_UserName).ToString.Trim() + "<br/>" + _
                                "from Date: " + Me.HFromDate.Value.Trim() + " To " + Me.HToDate.Value.Trim() + _
                                "</td></tr></tbody>" + _
                                "</table>"

                Me.objCommon.ExportToExcel("ViewExpenseDetailReport", Ds_Export, ReportHeader)

                Me.txtFromDate.Text = Me.HFromDate.Value
                Me.txtToDate.Text = Me.HToDate.Value
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

#End Region

#Region "Grid Events"

    Protected Sub GV_OtherExpDetail_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Or _
        e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(GVC_OtherExpDetail_OtherExpDtlNo).Visible = False
            e.Row.Cells(GVC_OtherExpDetail_OtherExpHdrNo).Visible = False

            e.Row.Cells(GVC_OtherExpDetail_FromDay).Visible = False
            e.Row.Cells(GVC_OtherExpDetail_ToDay).Visible = False

            e.Row.Cells(GVC_OtherExpDetail_Delete).Visible = False
            e.Row.Cells(GVC_OtherExpDetail_Flag).Visible = False
            e.Row.Cells(GVC_OtherExpDetail_ApprovalUserName).Visible = False

            If Me.Session(S_UserGroup) <> 4 Then
                e.Row.Cells(GVC_OtherExpDetail_Approve).Visible = False
                e.Row.Cells(GVC_OtherExpDetail_Reject).Visible = False
                e.Row.Cells(GVC_OtherExpDetail_Amt).Enabled = False
            End If
        End If

    End Sub

    Protected Sub GV_OtherExpDetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_OtherExpDetail.RowDataBound

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If e.Row.RowType <> DataControlRowType.DataRow Then
                Exit Sub
            End If

            CType(e.Row.FindControl("lnkApprove"), LinkButton).Enabled = (e.Row.Cells(GVC_OtherExpDetail_Flag).Text.ToUpper.Trim() = "")
            CType(e.Row.FindControl("lnkReject"), LinkButton).Enabled = (e.Row.Cells(GVC_OtherExpDetail_Flag).Text.ToUpper.Trim() = "")
            'CType(e.Row.FindControl("txtExpAmt"), TextBox).Enabled = (e.Row.Cells(GVC_OtherExpDetail_Flag).Text.ToUpper.Trim() = "")
            CType(e.Row.FindControl("txtExpAmt"), TextBox).Enabled = False

            If e.Row.Cells(GVC_OtherExpDetail_Flag).Text.ToUpper.Trim() <> "" Then

                CType(e.Row.FindControl("lnkApprove"), LinkButton).Text = IIf(e.Row.Cells(GVC_OtherExpDetail_Flag).Text.ToUpper.Trim() = "A", _
                                                                             "Approved BY " & e.Row.Cells(GVC_OtherExpDetail_ApprovalUserName).Text.ToUpper.Trim(), _
                                                                             "")
                CType(e.Row.FindControl("lnkReject"), LinkButton).Text = IIf(e.Row.Cells(GVC_OtherExpDetail_Flag).Text.ToUpper.Trim() = "R", _
                                                                            "Rejected BY " & e.Row.Cells(GVC_OtherExpDetail_ApprovalUserName).Text.ToUpper.Trim(), _
                                                                             "")

            End If

            CType(e.Row.FindControl("lnkDelete"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkDelete"), LinkButton).CommandName = "DELETE"
            CType(e.Row.FindControl("lnkDelete"), LinkButton).OnClientClick = "return confirm('Are you sure you want to delete OtherExp of " + e.Row.Cells(GVC_OtherExpDetail_FromDate).Text.Trim() + "?');"

            CType(e.Row.FindControl("lnkApprove"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkApprove"), LinkButton).CommandName = "APPROVE"

            CType(e.Row.FindControl("lnkReject"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkReject"), LinkButton).CommandName = "REJECT"


            CType(e.Row.FindControl("hlnkFile"), HyperLink).NavigateUrl = CType(e.Row.FindControl("hlnkFile"), HyperLink).Text.Trim()
            CType(e.Row.FindControl("hlnkFile"), HyperLink).Text = Path.GetFileName(CType(e.Row.FindControl("hlnkFile"), HyperLink).Text.Trim())

            If e.Row.RowIndex <= 0 Then
                e.Row.BackColor = Drawing.ColorTranslator.FromHtml("#ffdead")
                e.Row.Cells(GVC_OtherExpDetail_FromDate).Font.Bold = True
                e.Row.Cells(GVC_OtherExpDetail_ToDate).Font.Bold = True
                e.Row.Cells(GVC_OtherExpDetail_FromDay).Font.Bold = True
                e.Row.Cells(GVC_OtherExpDetail_ToDay).Font.Bold = True
                e.Row.Cells(GVC_OtherExpDetail_Total).Font.Bold = True
                CType(e.Row.FindControl("lnkDelete"), LinkButton).Font.Bold = True
                CType(e.Row.FindControl("lnkApprove"), LinkButton).Font.Bold = True
                CType(e.Row.FindControl("lnkReject"), LinkButton).Font.Bold = True
                Exit Sub
            End If

            If e.Row.Cells(GVC_OtherExpDetail_OtherExpHdrNo).Text.Trim() = Me.GV_OtherExpDetail.Rows(e.Row.RowIndex - 1).Cells(GVC_OtherExpDetail_OtherExpHdrNo).Text.Trim() Then

                e.Row.Cells(GVC_OtherExpDetail_FromDate).Text = ""
                e.Row.Cells(GVC_OtherExpDetail_ToDate).Text = ""
                e.Row.Cells(GVC_OtherExpDetail_FromDay).Text = ""
                e.Row.Cells(GVC_OtherExpDetail_ToDay).Text = ""
                e.Row.Cells(GVC_OtherExpDetail_Total).Text = ""
                e.Row.BackColor = Drawing.Color.White
                CType(e.Row.FindControl("lnkDelete"), LinkButton).Visible = False
                CType(e.Row.FindControl("lnkApprove"), LinkButton).Visible = False
                CType(e.Row.FindControl("lnkReject"), LinkButton).Visible = False

            Else 'If Different Date than Previos Date

                e.Row.BackColor = Drawing.ColorTranslator.FromHtml("#ffdead")
                e.Row.Cells(GVC_OtherExpDetail_FromDate).Font.Bold = True
                e.Row.Cells(GVC_OtherExpDetail_ToDate).Font.Bold = True
                e.Row.Cells(GVC_OtherExpDetail_FromDay).Font.Bold = True
                e.Row.Cells(GVC_OtherExpDetail_ToDay).Font.Bold = True
                e.Row.Cells(GVC_OtherExpDetail_Total).Font.Bold = True
                CType(e.Row.FindControl("lnkDelete"), LinkButton).Font.Bold = True
                CType(e.Row.FindControl("lnkApprove"), LinkButton).Font.Bold = True
                CType(e.Row.FindControl("lnkReject"), LinkButton).Font.Bold = True

            End If

        Catch ex As Exception

            Me.ShowErrorMessage(ex.Message, "", ex)

        End Try

    End Sub

    Protected Sub GV_OtherExpDetail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GV_OtherExpDetail.RowCommand

        Dim Index As Integer = CInt(e.CommandArgument)
        Dim dsOtherExpHdr As New DataSet
        Dim dsOtherExpDetail As New DataSet
        Dim dtOtherExpDetail As New DataTable
        Dim DS_OtherExp As New DataSet
        Dim Choice As New WS_HelpDbTable.DataObjOpenSaveModeEnum
        Dim dr As DataRow
        Dim WStr As String = ""
        Dim TotalExp As Double = 0.0
        Dim eStr As String = ""

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            '*************Getting Data for Operation
            WStr = "nOtherExpHdrNo=" & Me.GV_OtherExpDetail.Rows(Index).Cells(GVC_OtherExpDetail_OtherExpHdrNo).Text.Trim()

            If Not Me.ObjHelp.GetOtherExpHdr(WStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        dsOtherExpHdr, eStr) Then

                Me.objCommon.ShowAlert("Error while getting OtherExpHdr Data:" + eStr, Me.Page)
                Exit Sub

            End If

            If Not Me.ObjHelp.GetOtherExpDtl(WStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        dsOtherExpDetail, eStr) Then

                Me.objCommon.ShowAlert("Error while getting OtherExpDetail Data:" + eStr, Me.Page)
                Exit Sub

            End If
            '********************End Getting

            If e.CommandName.ToUpper.Trim() = "DELETE" Then

                Choice = WS_HelpDbTable.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete

            ElseIf e.CommandName.ToUpper.Trim() = "APPROVE" Then

                '**********OtherExpDtl
                For Each dr In dsOtherExpDetail.Tables(0).Rows

                    For cnt As Integer = Index To Me.GV_OtherExpDetail.Rows.Count - 1

                        If dr("nOtherExpDtlNo") = Me.GV_OtherExpDetail.Rows(cnt).Cells(GVC_OtherExpDetail_OtherExpDtlNo).Text.Trim() Then
                            dr("iExpAmt") = CType(Me.GV_OtherExpDetail.Rows(cnt).FindControl("txtExpAmt"), TextBox).Text.Trim()
                            dr("iApprovalUserId") = Me.Session(S_UserID).ToString.Trim()
                            dr("cApprovalFlag") = "A"
                            dr("dApprovalDate") = Today.Now.ToString("dd-MMM-yyyy")

                            TotalExp += Val(CType(Me.GV_OtherExpDetail.Rows(cnt).FindControl("txtExpAmt"), TextBox).Text)
                        End If

                    Next cnt

                Next dr
                dsOtherExpDetail.Tables(0).AcceptChanges()

                '**********OtherExpHdr
                For Each dr In dsOtherExpHdr.Tables(0).Rows

                    dr("iTotalExpAmt") = TotalExp
                    dr("iApprovalUserId") = Me.Session(S_UserID).ToString.Trim()
                    dr("cApprovalFlag") = "A"
                    dr("dApprovalDate") = Today.Now.ToString("dd-MMM-yyyy")

                Next dr
                dsOtherExpHdr.Tables(0).AcceptChanges()

                Choice = WS_HelpDbTable.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

            ElseIf e.CommandName.ToUpper.Trim() = "REJECT" Then

                '**********OtherExpDtl
                For Each dr In dsOtherExpDetail.Tables(0).Rows

                    dr("iApprovalUserId") = Me.Session(S_UserID).ToString.Trim()
                    dr("cApprovalFlag") = "R"
                    dr("dApprovalDate") = Today.Now.ToString("dd-MMM-yyyy")

                Next dr
                dsOtherExpDetail.Tables(0).AcceptChanges()

                '**********OtherExpHdr
                For Each dr In dsOtherExpHdr.Tables(0).Rows

                    dr("iApprovalUserId") = Me.Session(S_UserID).ToString.Trim()
                    dr("cApprovalFlag") = "R"
                    dr("dApprovalDate") = Today.Now.ToString("dd-MMM-yyyy")

                Next dr
                dsOtherExpHdr.Tables(0).AcceptChanges()

                Choice = WS_HelpDbTable.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

                'End If
            End If

            DS_OtherExp.Tables.Add(dsOtherExpHdr.Tables(0).Copy())
            DS_OtherExp.AcceptChanges()
            DS_OtherExp.Tables.Add(dsOtherExpDetail.Tables(0).Copy())
            DS_OtherExp.AcceptChanges()

            If Not Me.ObjLambda.Save_OtherExpHdr(Choice, DS_OtherExp, Me.Session(S_UserID), eStr) Then

                Me.objCommon.ShowAlert("Error while Deleting OtherExp:" + eStr, Me.Page)
                Exit Sub

            End If

            Me.objCommon.ShowAlert("OtherExp Deleted Successfully." + eStr, Me.Page)
            If Not FillGrid() Then
                Exit Sub
            End If

        Catch ex As Exception

            Me.ShowErrorMessage(ex.Message, "", ex)

        End Try

    End Sub

    Protected Sub GV_OtherExpDetail_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)

    End Sub

    Protected Sub GV_OtherExpDetail_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        GV_OtherExpDetail.PageIndex = e.NewPageIndex
        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub

#End Region

#Region "ShowErrorMessage"

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