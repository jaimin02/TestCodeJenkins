
Partial Class frmDWR
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Private objCommon As New clsCommon
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
    Private objHelpdb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()

    Private Const VS_DtForCalender As String = "Dt_ForCalender"
    Private Const VS_dtDWRDetail As String = "dt_DWRDetail"
    Private Const VS_dtDWRHdr As String = "dt_DWRHdr"
    Private Const VS_dtDWRDetailGV As String = "dt_DWRDetailGV"
    Private Const VS_dtDWRFinalGV As String = "dt_DWRFinalGV"

    'For GV_Planned Grid

    Private Const GVC_Planned_MtpNo As Integer = 0
    Private Const GVC_Planned_StpNo As Integer = 1
    Private Const GVC_Planned_CityNo As Integer = 2
    Private Const GVC_Planned_ActId As Integer = 3
    Private Const GVC_Planned_Project As Integer = 4
    Private Const GVC_Planned_Site As Integer = 5
    Private Const GVC_Planned_CityName As Integer = 6
    Private Const GVC_Planned_ActDesc As Integer = 7
    Private Const GVC_Planned_Remark As Integer = 8

    'For GV_DWRDetail Final Grid
    Private Const GVC_DWRDetail_DWRHdrNo As Integer = 0
    Private Const GVC_DWRDetail_DWRDtlNo As Integer = 1
    Private Const GVC_DWRDetail_ActivityId As Integer = 2
    Private Const GVC_DWRDetail_Project As Integer = 3
    Private Const GVC_DWRDetail_Site As Integer = 4
    Private Const GVC_DWRDetail_CityName As Integer = 5
    Private Const GVC_DWRDetail_Work As Integer = 6
    Private Const GVC_DWRDetail_ActDesc As Integer = 7
    Private Const GVC_DWRDetail_FromTime As Integer = 8
    Private Const GVC_DWRDetail_ToTime As Integer = 9
    Private Const GVC_DWRDetail_Reason As Integer = 10
    Private Const GVC_DWRDetail_Remark As Integer = 11

#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim estr As String = ""
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not IsPostBack Then

                Me.GenCall()

            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "", ex)
        End Try
    End Sub

#End Region

#Region "Calender Events"

    Protected Sub EventCalendar_DayRender(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DayRenderEventArgs) Handles EventCalendar.DayRender
        Dim CurrMonth As Integer = Month(Today.Date)
        Dim DtForcheck As New DataTable
        Dim dr As DataRow

        Try
            If e.Day.Date.DayOfWeek = DayOfWeek.Sunday Then
                e.Cell.ForeColor = Drawing.Color.Red
            End If

            DtForcheck = CType(Me.ViewState(VS_DtForCalender), DataTable)

            For Each dr In DtForcheck.Rows

                If Format(CDate(e.Day.Date), "dd-MMM-yyyy") = Format(CDate(dr("ddate")), "dd-MMM-yyyy") Then

                    If dr("dHolidayDate").ToString.Trim() <> "" Then
                        e.Cell.ForeColor = System.Drawing.Color.Red
                        e.Cell.Text = e.Day.DayNumberText.ToString.Trim() & "(" & dr("vHolidayDescription").ToString.Trim() & ")"
                        e.Day.IsSelectable = False
                    End If

                    If dr("dLeaveDate").ToString.Trim() <> "" Then
                        e.Cell.ForeColor = System.Drawing.Color.DarkRed
                        e.Cell.Text = e.Day.DayNumberText.ToString.Trim() & "(Leave)"
                        e.Day.IsSelectable = False
                    End If

                    If dr("dReportDate").ToString.Trim() <> "" Then

                        e.Cell.ForeColor = System.Drawing.Color.DarkMagenta
                        e.Cell.ToolTip = dr("vSiteName").ToString.ToUpper() & "," & _
                                         dr("vWorkSpaceDesc").ToString.ToUpper() & "," & _
                                         dr("vActivityName").ToString.ToUpper()
                        e.Cell.Text = "<b>" & e.Day.DayNumberText.ToString.Trim() & "(Done)</b>"
                        e.Day.IsSelectable = False
                    End If

                End If

            Next

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, ex.ToString())
        End Try
    End Sub

    Protected Sub EventCalendar_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles EventCalendar.SelectionChanged

        Me.txtRepDate.Text = EventCalendar.SelectedDate.Date.ToString("dd-MMM-yyyy")
        Me.btnGO_Click(sender, e)

    End Sub

    Protected Sub EventCalendar_VisibleMonthChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MonthChangedEventArgs)
        FillCalander(e.NewDate)
    End Sub

    Private Sub FillCalander(ByVal Ddate As Date)
        Dim estr As String = ""
        Dim ds_Calender As New DataSet

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not Me.objHelpdb.Proc_DWRLeaveHoliPerMonth(Me.Session(S_UserID), Me.Session(S_LocationCode), _
                                                                           Ddate.ToString("dd-MMM-yyyy"), ds_Calender, estr) Then

                Me.objCommon.ShowAlert("Error while Getting Data from Proc_DWRLeaveHoliPerMonth:" + estr, Me.Page)
                Exit Sub

            End If

            Me.ViewState(VS_DtForCalender) = ds_Calender.Tables(0)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString(), ex.ToString())
        End Try
    End Sub

#End Region

#Region "GenCall "

    Private Function GenCall() As Boolean
        Dim ds_DWR As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add ' CType(Me.Request.QueryString("Mode").ToString, WS_Lambda.DataObjOpenSaveModeEnum)
            Me.ViewState("Choice") = Choice   'To use it while saving

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState("nDWRHdrNo") = Me.Request.QueryString("Value").ToString
            End If

            ''''Check for Valid User''''''''''''''

            If Not GenCall_Data(ds_DWR) Then ' For Data Retrieval
                Exit Function
            End If

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")

        Finally

        End Try

    End Function

#End Region

#Region "GenCall_Data "

    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean

        Dim wStr As String = ""
        Dim eStr_Retu As String = ""
        Dim Val As String = ""
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing
        'Dim objLambda As New WS_Lambda.WS_Lambda

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Val = Me.ViewState("nDWRHdrNo") 'Value of where condition
            Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "nDWRHdrNo=" + Val.ToString
            End If

            'For DWRHdr Blank Dataset
            If Not Me.objHelpdb.GetDWRHdr(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            Me.ViewState(VS_dtDWRHdr) = ds.Tables(0)


            'For DWRDetail Blank Dataset
            ds = Nothing
            ds = New DataSet
            If Not Me.objHelpdb.GetDWRDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            Me.ViewState(VS_dtDWRDetail) = ds.Tables(0)

            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            If ds.Tables(0).Rows.Count <= 0 And _
             Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found for Selected Operation")
            End If

            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")

        Finally

        End Try
    End Function

#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim ds_DWR As DataSet = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim estr As String = ""
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Daily Work Report"
            Page.Title = " :: Daily Work Report ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            ds_DWR = Me.ViewState("dsDWR")

            If Not FillSTP() Then
                Exit Function
            End If

            If Not FillActivityGroup() Then
                Exit Function
            End If

            If Not FillReason() Then
                Exit Function
            End If

            If Not IsNothing(Me.Request.QueryString("RepDate")) AndAlso Me.Request.QueryString("RepDate") <> "" Then
                Me.txtRepDate.Text = DatePart(DateInterval.Day, CDate(Me.Request.QueryString("RepDate"))) & "-" & MonthName(DatePart(DateInterval.Month, CDate(Me.Request.QueryString("RepDate"))), True) & "-" & DatePart(DateInterval.Year, CDate(Me.Request.QueryString("RepDate")))
            Else
                Me.txtRepDate.Text = DatePart(DateInterval.Day, Date.Now) & "-" & MonthName(DatePart(DateInterval.Month, Date.Now), True) & "-" & DatePart(DateInterval.Year, Date.Now)
            End If

            FillCalander(Today.Now.Date.ToString("dd-MMM-yyyy"))

            Me.btnSave.Enabled = False
            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")

        Finally

        End Try
    End Function

#End Region

#Region "Fill Drop down"

    Private Function FillReason() As Boolean
        Dim ds As New DataSet
        Dim estr As String = ""
        Dim dv As New DataView
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Me.objHelpdb.GetReasonMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds, estr)

            If Not IsNothing(ds) Then
                If ds.Tables(0).Rows.Count > 0 Then
                    Me.ddlReason.Items.Clear()
                    dv = ds.Tables(0).DefaultView
                    dv.Sort = "vReasonDesc"
                    Me.ddlReason.DataSource = dv
                    Me.ddlReason.DataTextField = "vReasonDesc"
                    Me.ddlReason.DataValueField = "nReasonNo"
                    Me.ddlReason.DataBind()
                    Me.ddlReason.Items.Insert(0, New ListItem("Please Select Reason", ""))
                    Me.ddlReason.SelectedIndex = 0
                End If

            End If
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
            Return False
        End Try
    End Function

    Private Function FillSTP() As Boolean
        Dim ds_STP As New DataSet
        Dim Wstr As String = ""
        Dim estr As String = ""
        Dim dv_STP As New DataView
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not Me.objHelpdb.GetViewSTPUserWise(Me.Session(S_UserID), ds_STP, estr) Then
                Me.objCommon.ShowAlert("Error While Gwtting STP UserWise", Me.Page)
                Return False
            End If
            If Not IsNothing(ds_STP) Then

                If ds_STP.Tables(0).Rows.Count > 0 Then

                    dv_STP = ds_STP.Tables(0).DefaultView
                    dv_STP.Sort = "vSiteName"
                    Me.ddlSTP.DataSource = dv_STP
                    Me.ddlSTP.DataTextField = "vSiteName"
                    Me.ddlSTP.DataValueField = "nStpNo"
                    Me.ddlSTP.DataBind()
                    Me.ddlSTP.Items.Insert(0, New ListItem("Please Select Site", "0"))

                End If

            End If

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
            Return False

        Finally
            If Not IsNothing(ds_STP) Then
                ds_STP = Nothing
            End If
        End Try

    End Function

    Private Function FillActivityGroup() As Boolean

        Dim dsActivityGroup As New DataSet
        Dim eStr As String = ""
        Dim Wstr_Scope As String = ""

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            'To Get Where condition of ScopeVales( Project Type )
            If Not objCommon.GetScopeValueWithCondition(Wstr_Scope) Then
                Exit Function
            End If

            If Not objHelpdb.GetviewActivityGroupMst(Wstr_Scope, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    dsActivityGroup, eStr) Then
                Me.objCommon.ShowAlert(eStr + vbCrLf + "Error occured while retrieving Activity Group", Me)
                Exit Function
            End If

            If Not IsNothing(dsActivityGroup) Then

                If dsActivityGroup.Tables(0).Rows.Count > 0 Then

                    dsActivityGroup.Tables(0).DefaultView.Sort = "vActivityGroupName"
                    Me.ddlActivityGroup.DataSource = dsActivityGroup.Tables(0).DefaultView
                    Me.ddlActivityGroup.DataTextField = "vActivityGroupName"
                    Me.ddlActivityGroup.DataValueField = "vActivityGroupId"
                    Me.ddlActivityGroup.DataBind()
                    Me.ddlActivityGroup.Items.Insert(0, New ListItem("Please select ActivityGroup", "0"))

                End If

            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

#End Region

#Region "Grid Events"

    Protected Sub GV_DWRDetail_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GV_DWRDetail.RowDeleting
        Dim dtDWRDetailGV As DataTable
        Dim dtDWRDetail As DataTable
        Dim dtDWRHdr As DataTable
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim Wstr As String = ""
        Dim estr As String = ""
        Dim dsDWRHdr As New DataSet
        Dim dsDWRDtl As New DataSet
        Dim dsDWRDtlDelete As New DataSet
        Dim DWRHdrNo As Integer = 0
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            'Deleting From All Tables
            dtDWRHdr = CType(Me.ViewState(VS_dtDWRHdr), DataTable)
            dtDWRDetail = CType(Me.ViewState(VS_dtDWRDetail), DataTable)
            dtDWRDetailGV = CType(Me.ViewState(VS_dtDWRDetailGV), DataTable)

            'Deleteing from Grid Data table dtDWRDetailGV
            dtDWRDetailGV.Rows(e.RowIndex).Delete()
            dtDWRDetailGV.AcceptChanges()

            'For deleting Existing Data

            If Me.GV_DWRDetail.Rows(e.RowIndex).Cells(GVC_DWRDetail_DWRHdrNo).Text.Trim() > 0 AndAlso Me.GV_DWRDetail.Rows(e.RowIndex).Cells(GVC_DWRDetail_DWRDtlNo).Text.Trim() > 0 Then

                Wstr = "nDWRHdrNo=" & Me.GV_DWRDetail.Rows(e.RowIndex).Cells(GVC_DWRDetail_DWRHdrNo).Text.Trim()
                Wstr += " And cStatusIndi <> 'D'"
                If Not Me.objHelpdb.GetDWRHdr(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                dsDWRHdr, estr) Then

                    Me.objCommon.ShowAlert("Error while Getting Data from DWRHdr", Me.Page())
                    Exit Sub

                End If

                'Wstr += " AND nOtherExpDtlNo=" & Me.gvwExpense.Rows(e.RowIndex).Cells(GVC_DtlNo).Text.Trim()
                If Not Me.objHelpdb.GetDWRDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                dsDWRDtl, estr) Then

                    Me.objCommon.ShowAlert("Error while Getting Data from DWRDtl", Me.Page())
                    Exit Sub

                End If

                dsDWRDtl.Tables(0).DefaultView.RowFilter = "nDWRDtlNo=" & Me.GV_DWRDetail.Rows(e.RowIndex).Cells(GVC_DWRDetail_DWRDtlNo).Text.Trim()
                dsDWRDtlDelete.Tables.Add(dsDWRDtl.Tables(0).DefaultView.ToTable().Copy())

                dsDWRDtlDelete.Tables.Add(dsDWRHdr.Tables(0).Copy())

                If Me.objLambda.Save_DWRHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, _
                             dsDWRDtlDelete, Me.Session(S_UserID), DWRHdrNo, estr) Then

                    Me.objCommon.ShowAlert("DWR Successfuly Deleted", Me.Page())

                End If


            Else

                'Deleteing from DWRDetail Data table dtDWRDetail
                For index As Integer = dtDWRDetail.Rows.Count - 1 To 0 Step -1
                    If Me.GV_DWRDetail.Rows(e.RowIndex).Cells(GVC_DWRDetail_DWRDtlNo).Text.Trim() = dtDWRDetail.Rows(index).Item("nDWRDtlNo").ToString.Trim() Then

                        dtDWRDetail.Rows(index).Delete()
                        dtDWRDetail.AcceptChanges()
                        Exit For

                    End If

                Next index

                'Deleteing from DWRDetail Data table dtDWRHdr
                If dtDWRDetail.Rows.Count < 1 Then

                    dtDWRHdr.Rows(0).Delete()
                    dtDWRHdr.AcceptChanges()

                    Me.LblFinal.Visible = False
                    Me.btnSave.Enabled = False

                End If

            End If


            Me.GV_DWRDetail.DataSource = dtDWRDetailGV
            Me.GV_DWRDetail.DataBind()

            Me.ViewState(VS_dtDWRHdr) = dtDWRHdr
            Me.ViewState(VS_dtDWRDetail) = dtDWRDetail
            Me.ViewState(VS_dtDWRDetailGV) = dtDWRDetailGV

            If dtDWRDetail.Rows.Count < 1 Then
                Me.btnSave.Enabled = False
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
            dtDWRHdr = Nothing
            dtDWRDetail = Nothing
            dtDWRDetailGV = Nothing
        End Try
    End Sub

    Protected Sub GV_DWRDetail_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        e.Row.Cells(GVC_DWRDetail_ActivityId).Visible = False
        e.Row.Cells(GVC_DWRDetail_DWRDtlNo).Visible = False
        e.Row.Cells(GVC_DWRDetail_DWRHdrNo).Visible = False
        e.Row.Cells(GVC_DWRDetail_CityName).Visible = False

    End Sub

    Protected Sub GV_Planned_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or _
                e.Row.RowType = DataControlRowType.Header Or _
                e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GVC_Planned_ActId).Visible = False
            e.Row.Cells(GVC_Planned_CityNo).Visible = False
            e.Row.Cells(GVC_Planned_MtpNo).Visible = False
            e.Row.Cells(GVC_Planned_StpNo).Visible = False
        End If
    End Sub

    Protected Sub GV_Planned_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Dim DcrDate_1 As DateTime
        GV_Planned.PageIndex = e.NewPageIndex
        DcrDate_1 = Me.txtRepDate.Text
        If Not FillPlannedGrid(DcrDate_1) Then
            Exit Sub
        End If
    End Sub

#End Region

#Region "Clear And Exit"

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            If Not ResetPage() Then
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

#Region "GO CLICK"

    Protected Sub btnGO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGO.Click
        Dim eStr As String = ""
        Dim Choice1 As WS_Lambda.DataObjOpenSaveModeEnum
        Dim wStr As String = ""

        Dim dt_DWRDetail As New DataTable
        Dim dt_DWRhdr As New DataTable
        Dim dt_DWRFinalGV As New DataTable

        Dim dsEmp As New DataSet
        Dim DcrDate_1 As DateTime

        Dim dsParam As New DataSet
        Dim peraDays As Integer = 0
        Dim IsEditing As Boolean

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            'Added for Checking 
            DcrDate_1 = Me.txtRepDate.Text

            Choice1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

            If objHelpdb.IsDcrLock(Me.Session(S_UserID), Me.Session(S_LocationCode), Choice1, _
                                                    DcrDate_1, IsEditing, eStr) Then

                If Not IsEditing AndAlso (eStr.Trim() <> "") Then
                    Me.ResetPage()
                    Me.objCommon.ShowAlert(eStr, Me)
                    Exit Sub
                End If

            End If

            '-----------Checking that user can edit data or not for the given date-----------------

            peraDays = ClsParameterList.ParameterNoEnum.DWREditDays
            wStr = "vDeptCode = " & Me.Session(S_DeptCode) & " And nParameterNo = " & peraDays

            If Not objHelpdb.GetParameterDeptMatrix(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                    dsParam, eStr) Then

                Me.objCommon.ShowAlert(eStr, Me)
                Exit Sub

            End If

            If CType(Me.txtRepDate.Text.Trim(), Date) < (Date.Now.AddDays(-(dsParam.Tables(0).Rows(0).Item("vParameterValue").ToString()))) Then

                Me.objCommon.ShowAlert("You Can Not Edit Data Of More Than " & _
                        dsParam.Tables(0).Rows(0).Item("vParameterValue").ToString() & " Previous Days", Me)
                Exit Sub

            End If

            '-------------------------------------------------------------

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not Me.ResetPage() Then
                Exit Sub
            End If
            Me.btnExit.Enabled = True

            wStr = "cDtlStatusIndi <> 'D' And dReportDate = '" & Me.txtRepDate.Text.Trim() & "' And iUserId = " & Me.Session(S_UserID)

            If Not objHelpdb.GetViewUserWiseDWR(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsEmp, eStr) Then

                Me.objCommon.ShowAlert(eStr, Me)
                Exit Sub

            End If

            dt_DWRFinalGV = dsEmp.Tables(0)

            Me.EventCalendar.Visible = False

            If Not FillPlannedGrid(DcrDate_1) Then
                Exit Sub
            End If

            Me.ViewState(VS_dtDWRFinalGV) = dt_DWRFinalGV
            If Not CreateTableGV() Then
                Exit Sub
            End If

            If Not FillDWRFinalGV() Then
                Me.ShowErrorMessage("Error While Filling Grid", "")
                Exit Sub
            End If

            Me.PanelUnMslDtl.Visible = True

            If Not dt_DWRFinalGV.Rows.Count = 0 Then
                wStr = "dReportDate = '" & DcrDate_1 & "' And iUserId = " & Me.Session(S_UserID) & " And cStatusIndi <> 'D' "

                If Not objHelpdb.GetDWRHdr(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsEmp, eStr) Then
                    Me.objCommon.ShowAlert(eStr, Me)
                    Exit Sub
                End If

                dt_DWRhdr = dsEmp.Tables(0)

                Me.ViewState(VS_dtDWRHdr) = dt_DWRhdr

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try
    End Sub

#End Region

#Region "Planned Dtl"
    Private Function FillPlannedGrid(ByVal DcrDate_1 As Date) As Boolean
        Dim ds_MTP As New DataSet
        Dim dv As New DataView
        Dim eStr_Retu As String = ""
        Dim wStr As String = ""

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            wStr = "iUserId = " + Me.Session(S_UserID).ToString() + " AND " + _
                  " dMTPDate='" + DcrDate_1.ToString("dd-MMM-yyyy") + "'" '+" AND " + " nYear = " + DcrDate_1.Year.ToString()

            If Not objHelpdb.GetViewMTPInfo(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            ds_MTP, eStr_Retu) Then

                Exit Function

            End If


            dv = ds_MTP.Tables(0).DefaultView

            Me.GV_Planned.DataSource = Nothing
            Me.GV_Planned.DataBind()

            If dv.ToTable.Rows.Count > 0 Then

                Me.GV_Planned.DataSource = dv.ToTable
                Me.GV_Planned.DataBind()

            End If

            Me.ViewState(VS_dtDWRDetailGV) = ds_MTP.Tables(0)

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
            Return False
        End Try
    End Function
#End Region

#Region "Create Table for Grid"
    Private Function CreateTableGV() As Boolean
        Dim dtGV As New DataTable
        Dim dc As DataColumn

        dc = New DataColumn
        dc.ColumnName = "nDWRHdrNo"
        dc.DataType = GetType(Integer)
        dtGV.Columns.Add(dc)

        dc = New DataColumn
        dc.ColumnName = "nDWRDtlNo"
        dc.DataType = GetType(Integer)
        dtGV.Columns.Add(dc)

        dc = New DataColumn
        dc.ColumnName = "vActivityId"
        dc.DataType = GetType(String)
        dtGV.Columns.Add(dc)

        dc = New DataColumn
        dc.ColumnName = "vWorkspaceDesc"
        dc.DataType = GetType(String)
        dtGV.Columns.Add(dc)

        dc = New DataColumn
        dc.ColumnName = "vSiteName"
        dc.DataType = GetType(String)
        dtGV.Columns.Add(dc)

        dc = New DataColumn
        dc.ColumnName = "vCityName"
        dc.DataType = GetType(String)
        dtGV.Columns.Add(dc)

        dc = New DataColumn
        dc.ColumnName = "vWorkType"
        dc.DataType = GetType(String)
        dtGV.Columns.Add(dc)

        dc = New DataColumn
        dc.ColumnName = "vActivityDesc"
        dc.DataType = GetType(String)
        dtGV.Columns.Add(dc)


        dc = New DataColumn
        dc.ColumnName = "dFromTime"
        dc.DataType = GetType(String)
        dtGV.Columns.Add(dc)

        dc = New DataColumn
        dc.ColumnName = "dToTime"
        dc.DataType = GetType(String)
        dtGV.Columns.Add(dc)

        dc = New DataColumn
        dc.ColumnName = "vReasonDesc"
        dc.DataType = GetType(String)
        dtGV.Columns.Add(dc)

        dc = New DataColumn
        dc.ColumnName = "vRemark"
        dc.DataType = GetType(String)
        dtGV.Columns.Add(dc)

        Me.ViewState(VS_dtDWRDetailGV) = dtGV

        Return True
    End Function
#End Region

#Region "Selection Change"

    Protected Sub ddlActivityGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim estr As String = ""
        Dim wstr As String = ""
        Dim ds As New Data.DataSet
        Dim ds_type As New Data.DataSet
        Dim dv_Activity As New DataView
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            wstr = "vActivityGroupId='" & Me.ddlActivityGroup.SelectedValue.Trim() & "'"

            If Not objHelpdb.getActivityMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            ds_type, estr) Then

                ShowErrorMessage(estr, estr)
                Exit Sub

            End If

            dv_Activity = ds_type.Tables(0).DefaultView
            dv_Activity.Sort = "vActivityName"

            Me.ddlActivity.DataSource = dv_Activity.ToTable()
            Me.ddlActivity.DataValueField = "vActivityId"
            Me.ddlActivity.DataTextField = "vActivityName"
            Me.ddlActivity.DataBind()
            Me.ddlActivity.Items.Insert(0, New ListItem("Please select Activity", "0"))
            CType(Me.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(Me.ddlActivity)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "Button CLICK"

    Protected Sub btnOtherSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOtherSave.Click
        Dim dt_DWRDetail As New DataTable
        Dim dt_DWRhdr As New DataTable
        Dim dr As DataRow
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            dt_DWRDetail = Me.ViewState(VS_dtDWRDetailGV)

            For Each dr In dt_DWRDetail.Rows

                If (CType(Me.txtfromTime_OW.Text.ToString(), Date) >= CType(dr("dFromTime"), Date) And _
                        (CType(Me.txtfromTime_OW.Text.ToString(), Date) <= CType(dr("dToTime"), Date)) Or _
                        CType(Me.txtToTime_OW.Text.ToString(), Date) >= CType(dr("dFromTime"), Date) And _
                        CType(Me.txtToTime_OW.Text.ToString(), Date) <= CType(dr("dToTime"), Date)) Then

                    Me.objCommon.ShowAlert("You Can Not Enter Record For The Same Time ", Me)
                    Exit Sub

                End If
            Next

            FillDWRHdr(dt_DWRhdr)
            FillDWRDetail(2, dt_DWRDetail)

            Me.LblFinal.Visible = True

            If Me.ddlReason.Items.Count > 1 Then
                Me.ddlReason.SelectedIndex = 0
            End If

            Me.TxtOtherRemarks.Text = ""

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try
    End Sub

    Protected Sub BtnProjectSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dt_DWRDetail As New DataTable
        Dim dt_DWRhdr As New DataTable
        Dim dr As DataRow
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            dt_DWRDetail = Me.ViewState(VS_dtDWRDetailGV)

            For Each dr In dt_DWRDetail.Rows

                If (CType(Me.txtfromTime_Un.Text.ToString(), Date) >= CType(dr("dFromTime"), Date) And _
                        (CType(Me.txtfromTime_Un.Text.ToString(), Date) <= CType(dr("dToTime"), Date)) _
                Or CType(Me.txtToTime_Un.Text.ToString(), Date) >= CType(dr("dFromTime"), Date) And _
                        CType(Me.txtToTime_Un.Text.ToString(), Date) <= CType(dr("dToTime"), Date)) Then

                    Me.objCommon.ShowAlert("You Can Not Enter Record For The Same Time ", Me)
                    Exit Sub

                End If
            Next

            If Not FillDWRHdr(dt_DWRhdr) Then
                Exit Sub
            End If

            If Not FillDWRDetail(1, dt_DWRDetail) Then
                Exit Sub
            End If

            Me.LblFinal.Visible = True

            If Me.ddlActivity.Items.Count > 1 Then
                Me.ddlActivity.SelectedIndex = 0
            End If
            Me.txtRemark.Value = ""

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try
    End Sub

#End Region

#Region "DWR Hdr/Dtl Tables Functions "

    Private Function FillDWRHdr(ByRef dt_DWRhdr As DataTable) As Boolean
        Dim newHdr As DataRow
        Dim strNullValue As String = ""

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            'nDwrHdrNo,dReportDate,dDwrSubmitDate,iUserId,iModifyBY,dModifyOn,cReplicaFlag
            dt_DWRhdr = CType(Me.ViewState(VS_dtDWRHdr), DataTable)

            If dt_DWRhdr.Rows.Count >= 1 Then
                Return True
            End If

            newHdr = dt_DWRhdr.NewRow()
            newHdr("nDwrHdrNo") = 0
            newHdr("dReportDate") = Me.txtRepDate.Text
            newHdr("dDwrSubmitDate") = DatePart(DateInterval.Day, Date.Now) & "-" & MonthName(DatePart(DateInterval.Month, Date.Now), True) & "-" & DatePart(DateInterval.Year, Date.Now)
            newHdr("iUserId") = Me.Session(S_UserID)
            newHdr("iModifyBY") = Me.Session(S_UserID)
            newHdr("dModifyOn") = DatePart(DateInterval.Day, Date.Now) & "-" & MonthName(DatePart(DateInterval.Month, Date.Now), True) & "-" & DatePart(DateInterval.Year, Date.Now)
            newHdr("cStatusIndi") = "N"

            dt_DWRhdr.Rows.Add(newHdr)
            dt_DWRhdr.AcceptChanges()

            Me.ViewState(VS_dtDWRHdr) = dt_DWRhdr

            Return True

        Catch ex As System.Threading.ThreadAbortException

        Catch ex As Exception
            ShowErrorMessage(ex.ToString, "")
            Return False
        End Try

    End Function

    Private Function FillDWRDetail(ByVal Worktype As Integer, ByRef dt_DWRDetail As DataTable) As Boolean
        Dim DWRDtlNo As Integer
        Dim newHdr As DataRow
        Dim dt_DWRDetailGV As New DataTable
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            dt_DWRDetail = Me.ViewState(VS_dtDWRDetail)
            DWRDtlNo = dt_DWRDetail.Rows.Count + 1
            'nDwrDtlNo,nDwrHdrNo,cPlanned,nWorkTypeNo,vActivityId,nVisitedSTPNo,nVisitedCityNo,nWorkWithNo,
            'cTime,dFromTime,dToTime, cMissed, tMissedRemark, nReasonNo, vMeetPerson, vRemark, iModifyBY, dModifyOn, cReplicaFlag
            newHdr = dt_DWRDetail.NewRow()

            newHdr("nDwrDtlNo") = dt_DWRDetail.Rows.Count - 999
            If dt_DWRDetail.Rows.Count > 1 Then
                newHdr("nDwrDtlNo") = dt_DWRDetail.Compute("Max(nDwrDtlNo)", "1=1") + 1
            End If

            newHdr("nDwrHdrNo") = "0"
            newHdr("cPlanned") = "P"
            newHdr("nVisitedSTPNo") = Me.ddlSTP.SelectedValue.Trim()

            If Worktype = 1 Then
                newHdr("dFromTime") = Me.txtRepDate.Text & " " & Me.txtfromTime_Un.Text.Trim()
                newHdr("dToTime") = Me.txtRepDate.Text & " " & Me.txtToTime_Un.Text.Trim()

                newHdr("nWorkTypeNo") = Me.rblWorktype.SelectedValue.Trim()
                newHdr("vActivityId") = Me.ddlActivity.SelectedValue.Trim()
                newHdr("cMissed") = "N"
                newHdr("tMissedRemark") = "N"
                newHdr("vRemark") = Me.txtRemark.Value.Trim()
                newHdr("nReasonNo") = 0
                newHdr("vRemark") = ""
            ElseIf Worktype = 2 Then
                newHdr("dFromTime") = Me.txtRepDate.Text & " " & Me.txtfromTime_OW.Text.Trim()
                newHdr("dToTime") = Me.txtRepDate.Text & " " & Me.txtToTime_OW.Text.Trim()

                newHdr("nWorkTypeNo") = 2
                newHdr("vActivityId") = ""
                newHdr("cMissed") = "N"
                newHdr("tMissedRemark") = "N"
                newHdr("nReasonNo") = Me.ddlReason.SelectedValue.Trim()
                newHdr("vRemark") = Me.TxtOtherRemarks.Text.Trim()
            End If
            newHdr("iModifyBY") = Me.Session(S_UserID)
            newHdr("cStatusIndi") = "N"

            dt_DWRDetail.Rows.Add(newHdr)
            dt_DWRDetail.AcceptChanges()

            Me.ViewState(VS_dtDWRDetail) = dt_DWRDetail

            If Not FillDWRDetailGV(Worktype, DWRDtlNo, dt_DWRDetailGV) Then
                Exit Function
            End If

            Return True

        Catch ex As ConstraintException
            Return True

        Catch ex As System.Threading.ThreadAbortException

        Catch ex As Exception
            ShowErrorMessage(ex.ToString, "")
            Return False
        End Try

    End Function

    ''Only For Display Purpose
    Private Function FillDWRDetailGV(ByVal Worktype As Integer, ByVal DWRDtlNo As Integer, ByRef dt_DWRDetailGV As DataTable) As Boolean
        Dim newHdr As DataRow
        Dim DsSite As New DataSet
        Dim wstr As String = ""
        Dim estr As String = ""
        Dim dt_DWRDetail As DataTable
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            dt_DWRDetailGV = CType(Me.ViewState(VS_dtDWRDetailGV), DataTable)

            wstr = "nStpNo=" & Me.ddlSTP.SelectedValue.Trim()
            If Not Me.objHelpdb.GetViewSTPWithScope(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, DsSite, estr) Then
                Me.objCommon.ShowAlert("Error While Getting Information of Site", Me.Page)
                Return False
            End If

            dt_DWRDetail = Me.ViewState(VS_dtDWRDetail)

            newHdr = dt_DWRDetailGV.NewRow()

            newHdr("nDwrHdrNo") = "0"

            newHdr("nDwrDtlNo") = (dt_DWRDetail.Rows.Count - 999) - 1
            If dt_DWRDetail.Rows.Count > 1 Then
                newHdr("nDwrDtlNo") = dt_DWRDetail.Compute("Max(nDwrDtlNo)", "1=1") + 1
            End If

            'newHdr("nDwrDtlNo") = DWRDtlNo

            newHdr("vCityName") = DsSite.Tables(0).Rows(0).Item("vWorkspaceDesc")
            newHdr("vSiteName") = Me.ddlSTP.SelectedItem.Text.Trim()
            newHdr("vWorkspaceDesc") = DsSite.Tables(0).Rows(0).Item("vWorkspaceDesc")
            newHdr("vWorkType") = IIf(Worktype = 1, Me.rblWorktype.SelectedItem.Text.Trim() & " Work", "Other Work")
            newHdr("vRemark") = Me.txtRemark.Value.Trim()

            If Worktype = 1 Then
                newHdr("dFromTime") = Me.txtfromTime_Un.Text.Trim()
                newHdr("dToTime") = Me.txtToTime_Un.Text.Trim()

                newHdr("vActivityId") = Me.ddlActivity.SelectedValue.Trim()
                newHdr("vActivityDesc") = Me.ddlActivity.SelectedItem.Text.Trim()
                newHdr("vReasonDesc") = ""
            ElseIf Worktype = 2 Then
                newHdr("dFromTime") = Me.txtfromTime_OW.Text.Trim()
                newHdr("dToTime") = Me.txtToTime_OW.Text.Trim()

                newHdr("vActivityId") = ""
                newHdr("vActivityDesc") = ""
                newHdr("vReasonDesc") = Me.ddlReason.SelectedItem.Text.Trim()
                newHdr("vRemark") = Me.TxtOtherRemarks.Text.Trim()
            End If

            dt_DWRDetailGV.Rows.Add(newHdr)
            dt_DWRDetailGV.AcceptChanges()

            Me.ViewState(VS_dtDWRDetailGV) = dt_DWRDetailGV

            Me.GV_DWRDetail.DataSource = Nothing
            Me.GV_DWRDetail.DataBind()

            Me.GV_DWRDetail.DataSource = dt_DWRDetailGV
            Me.GV_DWRDetail.DataBind()

            Me.btnSave.Enabled = True
            If Me.GV_DWRDetail.Rows.Count < 1 Then
                Me.btnSave.Enabled = False
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function FillDWRFinalGV() As Boolean
        Dim dr As DataRow
        Dim newHdr As DataRow
        Dim dt_DWRFinalGV As DataTable
        Dim dt_DWRDetailGV As DataTable
        Dim wstr As String = ""
        Dim estr As String = ""

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            dt_DWRDetailGV = CType(Me.ViewState(VS_dtDWRDetailGV), DataTable)
            dt_DWRFinalGV = CType(Me.ViewState(VS_dtDWRFinalGV), DataTable)

            For Each dr In dt_DWRFinalGV.Rows

                newHdr = dt_DWRDetailGV.NewRow()

                newHdr("nDwrHdrNo") = dr("nDwrHdrNo").ToString()
                newHdr("nDwrDtlNo") = dr("nDWRDtlNo").ToString()
                newHdr("vCityName") = dr("vCityName").ToString()
                newHdr("vSiteName") = dr("vSiteName").ToString()
                newHdr("vWorkspaceDesc") = dr("vWorkSpaceDesc").ToString()
                newHdr("vWorkType") = dr("vWorkTypeDesc").ToString()
                newHdr("vRemark") = dr("vRemark").ToString()
                newHdr("dFromTime") = dr("dFromTime").ToString()
                newHdr("dToTime") = dr("dToTime").ToString()
                newHdr("vActivityId") = dr("vActivityId").ToString()
                newHdr("vActivityDesc") = dr("vActivityName").ToString()
                newHdr("vReasonDesc") = dr("vReasonDesc").ToString()

                dt_DWRDetailGV.Rows.Add(newHdr)
                dt_DWRDetailGV.AcceptChanges()

            Next

            Me.ViewState(VS_dtDWRDetailGV) = dt_DWRDetailGV

            Me.GV_DWRDetail.DataSource = Nothing
            Me.GV_DWRDetail.DataBind()

            Me.GV_DWRDetail.DataSource = dt_DWRDetailGV
            Me.GV_DWRDetail.DataBind()

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "ResetPanels"

    Private Function ResetPage() As Boolean
        Dim ds As New DataSet
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Me.ddlReason.SelectedIndex = 0

            Me.PanelUnMslDtl.Visible = False
            Me.btnSave.Enabled = False

            Me.EventCalendar.Visible = True

            FillCalander(CDate(Me.txtRepDate.Text))

            Me.ViewState(VS_dtDWRDetailGV) = Nothing
            Me.ViewState(VS_dtDWRHdr) = Nothing
            Me.ViewState(VS_dtDWRDetail) = Nothing

            Me.LblFinal.Visible = False
            Me.GV_DWRDetail.DataSource = Nothing
            Me.GV_DWRDetail.DataBind()

            Me.GV_Planned.DataSource = Nothing
            Me.GV_Planned.DataBind()

            If Not GenCall_Data(ds) Then
                Exit Function
            End If
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
            Return False
        End Try
    End Function

#End Region

#Region "Save"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim DocumentNo As String = ""
        Dim ds_DWR As New DataSet
        Dim dt_DWRHdr As New DataTable
        Dim dt_DWRDetail As New DataTable

        Dim estr_retu As String = ""
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If IsNothing(Me.ViewState(VS_dtDWRHdr)) Then

                objCommon.ShowAlert(" No Records in DWR Hdr ", Me)
                Exit Sub

            End If

            ds_DWR = New DataSet
            dt_DWRHdr = CType(Me.ViewState(VS_dtDWRHdr), DataTable)
            dt_DWRHdr.TableName = "DWRHDR"
            ds_DWR.Tables.Add(dt_DWRHdr.Copy)

            dt_DWRDetail = CType(Me.ViewState(VS_dtDWRDetail), DataTable)
            dt_DWRDetail.TableName = "DWRDetail"
            ds_DWR.Tables.Add(dt_DWRDetail.Copy)

            If Not objLambda.Save_DWRHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                            ds_DWR, Me.Session(S_UserID), DocumentNo, estr_retu) Then

                objCommon.ShowAlert("Error While Saving", Me)
                Exit Sub

            End If

            Me.objCommon.ShowAlert(DocumentNo, Me)
            Me.ResetPage()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try


    End Sub

#End Region

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        If exMessage <> "" And eStr <> "" Then
            objCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
        End If

    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        If exMessage <> "" And eStr <> "" Then
            objCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
        End If
    End Sub
#End Region

End Class
