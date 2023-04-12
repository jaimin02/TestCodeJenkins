
Partial Class frmViewDWR
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objCommon As New clsCommon
    Private ObjLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
    Private ObjHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private Const VS_dtDWRDetailGV As String = "dt_DWRDetailGV"

    'For GV_DWRDetail Final Grid
    Private Const GVC_DWRDetail_DWRHdrNo As Integer = 0
    Private Const GVC_DWRDetail_DWRDtlNo As Integer = 1
    Private Const GVC_DWRDetail_ActivityId As Integer = 2
    Private Const GVC_DWRDetail_Date As Integer = 3
    Private Const GVC_DWRDetail_Day As Integer = 4
    Private Const GVC_DWRDetail_Project As Integer = 5
    Private Const GVC_DWRDetail_Site As Integer = 6
    Private Const GVC_DWRDetail_CityName As Integer = 7
    Private Const GVC_DWRDetail_Work As Integer = 8
    Private Const GVC_DWRDetail_ActDesc As Integer = 9
    Private Const GVC_DWRDetail_FromTime As Integer = 10
    Private Const GVC_DWRDetail_ToTime As Integer = 11
    Private Const GVC_DWRDetail_Reason As Integer = 12
    Private Const GVC_DWRDetail_Remark As Integer = 13
    Private Const GVC_DWRDetail_Delete As Integer = 14

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

            Page.Title = ":: View DWR :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "View DWR Detail Report"

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

#Region "Fill User"

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
                    Me.objCommon.ShowAlert("No Record Found", Me.Page)
                    Return False
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
        Dim dS_DWRDetailGV As New DataSet
        Dim dt_DWRDetailGV As New DataTable

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Me.GV_DWRDetail.DataSource = Nothing
            Me.GV_DWRDetail.DataBind()

            Me.HFromDate.Value = Me.txtFromDate.Text
            Me.HToDate.Value = Me.txtToDate.Text

            Wstr = "iUserId=" & Me.Session(S_UserID).ToString.Trim()

            If Me.ddlUser.Visible = True Then
                Wstr = "iUserId=" & Me.ddlUser.SelectedValue.Trim()
            End If

            Wstr += " AND dReportDate>='" & Me.HFromDate.Value.Trim() & _
                    "' AND dReportDate<='" & Me.HToDate.Value.Trim() & "'"

            If Not Me.ObjHelp.GetViewUserWiseDWR(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                dS_DWRDetailGV, Estr) Then

                Me.objCommon.ShowAlert("Error while getting Information from View_UserWiseDWR", Me.Page)
                Return False

            End If

            If dS_DWRDetailGV.Tables(0).Rows.Count < 1 Then

                Me.objCommon.ShowAlert("No Record Found", Me.Page)
                Me.BtnExport.Visible = False
                Return True

            End If

            Me.BtnExport.Visible = True

            dS_DWRDetailGV.Tables(0).DefaultView.Sort = "dReportDate"

            Me.ViewState(VS_dtDWRDetailGV) = dS_DWRDetailGV.Tables(0).DefaultView.ToTable()

            Me.GV_DWRDetail.DataSource = dS_DWRDetailGV.Tables(0).DefaultView
            Me.GV_DWRDetail.DataBind()
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

            Dt_Export = CType(Me.ViewState(VS_dtDWRDetailGV), DataTable)

            If Not Dt_Export Is Nothing Then
                Dt_Export.Columns.Remove("nDwrHdrNo")
                Dt_Export.Columns.Remove("vActivityId")
                Dt_Export.Columns.Remove("nVisitedSTPNo")
                Dt_Export.Columns.Remove("nVisitedCityNo")
                Dt_Export.Columns.Remove("vWorkspaceId")
                Dt_Export.Columns.Remove("nDWRDtlNo")
                Dt_Export.Columns.Remove("nWorkTypeNo")
                Dt_Export.Columns.Remove("nWorkWithNo")
                Dt_Export.Columns.Remove("cTime")
                Dt_Export.Columns.Remove("cMissed")
                Dt_Export.Columns.Remove("tMissedRemark")
                Dt_Export.Columns.Remove("nReasonNo")
                Dt_Export.Columns.Remove("vMeetPerson")
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

                Dt_Export.Columns("dReportDate").ColumnName = "Report Date"
                Dt_Export.Columns("dDwrSubmitDate").ColumnName = "Submit Date"
                Dt_Export.Columns("vDay").ColumnName = "Day"
                Dt_Export.Columns("vWorkspaceDesc").ColumnName = "Project"
                Dt_Export.Columns("vSiteName").ColumnName = "Site"
                Dt_Export.Columns("vCityName").ColumnName = "City"
                Dt_Export.Columns("vWorkTypeDesc").ColumnName = "Work"
                Dt_Export.Columns("vActivityName").ColumnName = "Activity"
                Dt_Export.Columns("dFromTime").ColumnName = "From Time"
                Dt_Export.Columns("dToTime").ColumnName = "To Time"
                Dt_Export.Columns("vReasonDesc").ColumnName = "Reason"
                Dt_Export.Columns("vRemark").ColumnName = "Remark"
                Dt_Export.AcceptChanges()
                Ds_Export.Tables.Add(Dt_Export.Copy())

                ReportHeader = "<table style=""border:solid 1px black;font-name:Arial; font-size:10pt; color:navy; font-weight:bold;"" " + _
                               " cellspacing=""0px"" cellpadding=""5px"" >"
                ReportHeader += "<tbody><tr><td colspan=""13"" align=center >View Daily Work Reprort <br/>" + _
                                "User: " + Me.Session(S_UserName).ToString.Trim() + "<br/>" + _
                                "from Date: " + Me.HFromDate.Value.Trim() + " To " + Me.HToDate.Value.Trim() + _
                                "</td></tr></tbody>" + _
                                "</table>"

                Me.objCommon.ExportToExcel("ViewDailyWorkReport", Ds_Export, ReportHeader)

                Me.txtFromDate.Text = Me.HFromDate.Value
                Me.txtToDate.Text = Me.HToDate.Value

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "", ex)
        End Try

    End Sub

#End Region

#Region "Grid Events"

    Protected Sub GV_DWRDetail_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Or _
        e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(GVC_DWRDetail_ActivityId).Visible = False
            e.Row.Cells(GVC_DWRDetail_DWRDtlNo).Visible = False
            e.Row.Cells(GVC_DWRDetail_DWRHdrNo).Visible = False
        End If

    End Sub

    Protected Sub GV_DWRDetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_DWRDetail.RowDataBound

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If e.Row.RowType <> DataControlRowType.DataRow Then
                Exit Sub
            End If

            CType(e.Row.FindControl("lnkDelete"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkDelete"), LinkButton).CommandName = "DELETE"
            CType(e.Row.FindControl("lnkDelete"), LinkButton).OnClientClick = "return confirm('Are you sure you want to delete DWR of " + e.Row.Cells(GVC_DWRDetail_Date).Text.Trim() + "?');"

            If e.Row.RowIndex <= 0 Then
                e.Row.BackColor = Drawing.ColorTranslator.FromHtml("#ffdead")
                e.Row.Cells(GVC_DWRDetail_Date).Font.Bold = True
                e.Row.Cells(GVC_DWRDetail_Day).Font.Bold = True
                CType(e.Row.FindControl("lnkDelete"), LinkButton).Font.Bold = True
                Exit Sub
            End If

            If e.Row.Cells(GVC_DWRDetail_DWRHdrNo).Text.Trim() = Me.GV_DWRDetail.Rows(e.Row.RowIndex - 1).Cells(GVC_DWRDetail_DWRHdrNo).Text.Trim() Then

                e.Row.Cells(GVC_DWRDetail_Date).Text = ""
                e.Row.Cells(GVC_DWRDetail_Day).Text = ""
                e.Row.BackColor = Drawing.Color.White
                CType(e.Row.FindControl("lnkDelete"), LinkButton).Visible = False

            Else 'If Different Date than Previos Date

                e.Row.BackColor = Drawing.ColorTranslator.FromHtml("#ffdead")
                e.Row.Cells(GVC_DWRDetail_Date).Font.Bold = True
                e.Row.Cells(GVC_DWRDetail_Day).Font.Bold = True
                CType(e.Row.FindControl("lnkDelete"), LinkButton).Font.Bold = True

            End If

        Catch ex As Exception

            Me.ShowErrorMessage(ex.Message, "", ex)

        End Try

    End Sub

    Protected Sub GV_DWRDetail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GV_DWRDetail.RowCommand

        Dim Index As Integer = CInt(e.CommandArgument)
        Dim dsDWRHdr As New DataSet
        Dim dsDWRDetail As New DataSet
        Dim DS_DWR As New DataSet
        Dim WStr As String = ""
        Dim DWRNo As Integer
        Dim eStr As String = ""

        Dim dsParam As New DataSet
        Dim peraDays As Integer
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            '-----------Checking that user can edit data or not for the given date-----------------

            peraDays = ClsParameterList.ParameterNoEnum.DWREditDays
            WStr = "vDeptCode = " & Me.Session(S_DeptCode) & " And nParameterNo = " & peraDays

            If Not ObjHelp.GetParameterDeptMatrix(WStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsParam, eStr) Then

                Me.objCommon.ShowAlert(eStr, Me)
                Exit Sub

            End If

            'If Not dsEmp.Tables(0).Rows.Count < 1 Then
            If CType(GV_DWRDetail.Rows(Index).Cells(GVC_DWRDetail_Date).Text.Trim(), Date) < _
                    (Date.Now.AddDays(-(dsParam.Tables(0).Rows(0).Item("vParameterValue").ToString()))) Then

                Me.objCommon.ShowAlert("You can not edit data of more than " & _
                        dsParam.Tables(0).Rows(0).Item("vParameterValue").ToString() & " previous days", Me)
                Exit Sub
            End If

            'End If
            '-------------------------------------------------------------


            If e.CommandName.ToUpper.Trim() = "DELETE" Then

                WStr = "nDWRHdrNo=" & Me.GV_DWRDetail.Rows(Index).Cells(GVC_DWRDetail_DWRHdrNo).Text.Trim()

                If Not Me.ObjHelp.GetDWRHdr(WStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            dsDWRHdr, eStr) Then

                    Me.objCommon.ShowAlert("Error while getting DWRHdr Data:" + eStr, Me.Page)
                    Exit Sub

                End If

                If Not Me.ObjHelp.GetDWRDetail(WStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            dsDWRDetail, eStr) Then

                    Me.objCommon.ShowAlert("Error while getting DWRDetail Data:" + eStr, Me.Page)
                    Exit Sub

                End If

                DS_DWR.Tables.Add(dsDWRHdr.Tables(0).Copy())
                DS_DWR.AcceptChanges()
                DS_DWR.Tables.Add(dsDWRDetail.Tables(0).Copy())
                DS_DWR.AcceptChanges()

                If Not Me.ObjLambda.Save_DWRHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, DS_DWR, _
                                                    Me.Session(S_UserID), DWRNo, eStr) Then

                    Me.objCommon.ShowAlert("Error while Deleting DWR:" + eStr, Me.Page)
                    Exit Sub

                End If

                Me.objCommon.ShowAlert("DWR Deleted Successfully." + eStr, Me.Page)

                If Not FillGrid() Then
                    Exit Sub
                End If

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "", ex)
        End Try

    End Sub

    Protected Sub GV_DWRDetail_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)

    End Sub

    Protected Sub GV_DWRDetail_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        GV_DWRDetail.PageIndex = e.NewPageIndex
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

