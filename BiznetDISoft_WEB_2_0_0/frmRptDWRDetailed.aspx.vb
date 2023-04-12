
Partial Class frmRptDWRDetailed
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private rPage As RepoPage

    Private Const VS_dtUserWiseDWR As String = "UserWiseDWR"

#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "GenCall "
    Private Function GenCall() As Boolean
        Dim ds As New DataSet

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not GenCall_Data() Then ' For Data Retrieval
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
    Private Function GenCall_Data() As Boolean

        Dim eStr_Retu As String = ""
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")

        Finally

        End Try
    End Function
#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = "::DWR Detailed Report :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "DWR Detailed Report"
            Choice = Me.ViewState("Choice")

            If Not FillcblUser() Then
                Exit Function
            End If

            Me.txtDWRFromDate.Text = Today.Now.Date.ToString("dd-MMM-yyyy")
            Me.txtDWRToDate.Text = Today.Now.Date.ToString("dd-MMM-yyyy")

            chkSelectAll.Attributes.Add("onclick", "CheckBoxListSelection(" + Me.cblUser.ClientID + ",this);")

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function

#End Region

#Region "FillCheckBoxList"
    Private Function FillcblUser() As Boolean
        Dim ds_User As New Data.DataSet
        Dim estr As String = ""
        Dim wstr As String = ""
        Try

            wstr = "vLocationCode = " & Me.Session(S_LocationCode) & " AND vDeptCode=" & Me.Session(S_DeptCode) & " And cStatusindi<>'D'"

            If Not objHelp.GetViewUserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_User, estr) Then
                Me.ObjCommon.ShowAlert("Error while Getting Data from ViewUserMst", Me.Page())
                Return False
            End If

            Me.cblUser.DataSource = ds_User.Tables(0).DefaultView.ToTable(True, "iUserId,vUserName".Split(","))
            Me.cblUser.DataValueField = "iUserId"
            Me.cblUser.DataTextField = "vUserName"
            Me.cblUser.DataBind()

            If ds_User.Tables(0).Rows.Count < 1 Then
                Me.pnlUser.Visible = False
                Me.chkSelectAll.Visible = False
            Else
                Me.pnlUser.Visible = True
                Me.chkSelectAll.Visible = True
            End If

            For Each lstItem As ListItem In cblUser.Items

                lstItem.Attributes.Add("onclick", "SetCheckUncheckAll(document.getElementById('" + _
                                                Me.cblUser.ClientID + "'), document.getElementById('" + _
                                                Me.chkSelectAll.ClientID + "'));")

            Next lstItem

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function
#End Region

#Region "Button Events"

    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click

        Dim ds_UserWiseDwr As New Data.DataSet
        Dim estr As String = ""
        Dim wstr As String = ""

        Dim FileName As String = ""
        Dim isReportComplete As Boolean = False
        Dim CntUser As Integer = 0
        Dim strUser As String = ""
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            wstr = "(cast(dReportDate as Datetime) >='" + Me.txtDWRFromDate.Text.Trim() + "'"
            wstr += " And cast(dReportDate as Datetime) <='" + Me.txtDWRToDate.Text.Trim() + "')"

            Do While CntUser <= cblUser.Items.Count - 1

                If Me.cblUser.Items(CntUser).Selected = True Then
                    strUser += Me.cblUser.Items(CntUser).Value.Trim()
                    strUser += ","
                End If
                CntUser += 1

            Loop

            strUser = strUser.Substring(0, strUser.Length - 1)
            wstr += " AND iUserId in(" & strUser & ")"

            If Not objHelp.GetViewUserWiseDWR(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_UserWiseDwr, estr) Then
                Me.ObjCommon.ShowAlert("Error while Getting Data from ViewUserWiseDwr", Me.Page())
                Exit Sub
            End If

            If ds_UserWiseDwr.Tables(0).Rows.Count < 1 Then
                ObjCommon.ShowAlert("No Records For Selected Criteria", Me.Page)
                Exit Sub
            End If

            Me.ViewState(VS_dtUserWiseDWR) = ds_UserWiseDwr.Tables(0)

            FileName = GetReportName() + ".xls"
            FileName = Server.MapPath("~/ExcelReportFile/" + FileName)

            OpenReport(FileName)

            ReportHeader()
            ReportDetail()

            isReportComplete = True


        Catch ex As Exception
            isReportComplete = False
            Me.ShowErrorMessage(ex.Message, "")
        Finally
            If Not rPage Is Nothing Then
                rPage.CloseReport()
                rPage = Nothing
            End If
        End Try

        If isReportComplete = True Then
            ReportGeneralFunction.ExportFileAtWeb(ReportGeneralFunction.ExportFileAsEnum.ExportAsXLS, HttpContext.Current.Response, FileName)
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.txtDWRFromDate.Text = ""
        Me.txtDWRToDate.Text = ""
        Me.chkWithSummary.Checked = False
        Me.chkSelectAll.Checked = False
        Me.cblUser.ClearSelection()
        Me.ViewState(VS_dtUserWiseDWR) = Nothing
        GenCall()
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

#Region "Report Functions"

    Private Function OpenReport(ByVal ReportFileName_1 As String) As Boolean
        '' This Function open file on physical memory(In HardDist)          
        If File.Exists(ReportFileName_1) = True Then
            File.Delete(ReportFileName_1)
        End If
        rPage = New RepoPage(ReportFileName_1)
    End Function

    Private Sub ReportHeader()
        Dim rRow As RepoRow
        Dim rCell As RepoCell
        'Dim i As Integer

        rRow = New RepoRow
        rCell = rRow.AddCell("ReportTitle")
        rCell.Value = "DWR Detailed Report"
        rCell.FontBold = True
        rCell.FontSize = 12
        'rCell.FontColor = Drawing.Color.LightGray
        rCell.NoofCellContain = 9
        rCell.Alignment = RepoCell.AlignmentEnum.CenterTop
        rPage.Say(rRow)

        rRow = New RepoRow
        rCell = rRow.AddCell("FromToDate")
        rCell.Value = "From Date:" + Me.txtDWRFromDate.Text.Trim() + " To:" + Me.txtDWRToDate.Text.Trim()
        rCell.FontBold = False
        rCell.FontSize = 12
        rCell.NoofCellContain = 9
        rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
        'rCell.FontColor = Drawing.Color.LightGray
        rPage.Say(rRow)

        'rPage.SayBlankRow()
    End Sub

    Private Sub ReportDetail()
        Dim rMasterRow As RepoRow
        'Dim rMasterUserRow As RepoRow
        Dim RowCnt As Integer
        Dim dt_UserWiseDWR As DataTable

        Dim PreviousDate As String = ""
        Dim PreviousProject As String = ""
        Dim PreviousSite As String = ""
        Dim PreviousActivity As String = ""
        Dim PreviousWorkType As String = ""
        Dim PreviousUserId As Integer = 0

        Dim rMasterRowSummary As RepoRow

        Dim TotalNoOfWorkDays As Integer
        Dim TotalNoOfSundays As Integer
        Dim TotalNoOfHolidays As Integer
        Dim TotalNoOfOfficeWorkDays As Integer
        Dim TotalNoOfOnsiteWorkDays As Integer
        Dim TotalNoOfTravelledDays As Integer
        Dim TotalNoOfOtherWorkDays As Integer
        Dim index As Integer = 0
        Dim wstr As String = ""
        Dim estr As String = ""
        Dim ds_Holiday As New DataSet
        Dim FromDate As Date
        Dim Todate As Date
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            dt_UserWiseDWR = Me.ViewState(VS_dtUserWiseDWR)

            rMasterRow = MasterRowMain()

            'rMasterUserRow = MasterUserRow()

            rMasterRowSummary = MasterSummaryRow()

            RowCnt = 0


            Do While RowCnt <= dt_UserWiseDWR.Rows.Count - 1

                If PreviousUserId <> dt_UserWiseDWR.Rows(RowCnt)("iUserId").ToString() Then

                    '***************Master Row
                    rPage.SayBlankRow()
                    PrintMasterUserRow(dt_UserWiseDWR.Rows(RowCnt)("vUserName").ToString())
                    PrintMasterRowMain()
                    '************************************

                End If

                rMasterRow.Cell("Date").Value = dt_UserWiseDWR.Rows(RowCnt)("dReportDate").ToString()
                rMasterRow.Cell("Day").Value = dt_UserWiseDWR.Rows(RowCnt)("vDay").ToString()
                rMasterRow.Cell("Project").Value = dt_UserWiseDWR.Rows(RowCnt)("vWorkSpaceDesc").ToString()
                rMasterRow.Cell("Site").Value = dt_UserWiseDWR.Rows(RowCnt)("vSiteName").ToString()
                rMasterRow.Cell("Activity").Value = dt_UserWiseDWR.Rows(RowCnt)("vActivityName").ToString()
                rMasterRow.Cell("WorkType").Value = dt_UserWiseDWR.Rows(RowCnt)("vWorkTypeDesc").ToString()
                rMasterRow.Cell("TimePeriod").Value = dt_UserWiseDWR.Rows(RowCnt)("dFromTime").ToString() + " To " + dt_UserWiseDWR.Rows(RowCnt)("dToTime").ToString()
                rMasterRow.Cell("TotalTime").Value = dt_UserWiseDWR.Rows(RowCnt)("DiffHH_MM").ToString()
                rMasterRow.Cell("ReasonIfOtherWork").Value = dt_UserWiseDWR.Rows(RowCnt)("vReasonDesc").ToString()


                If PreviousDate = dt_UserWiseDWR.Rows(RowCnt)("dReportDate").ToString() Then

                    rMasterRow.Cell("Date").Value = ""
                    rMasterRow.Cell("Day").Value = ""

                    If PreviousProject = dt_UserWiseDWR.Rows(RowCnt)("vWorkSpaceDesc").ToString() Then

                        rMasterRow.Cell("Project").Value = ""
                        If PreviousSite = dt_UserWiseDWR.Rows(RowCnt)("vSiteName").ToString() Then

                            rMasterRow.Cell("Site").Value = ""
                            If PreviousActivity = dt_UserWiseDWR.Rows(RowCnt)("vActivityName").ToString() Then

                                rMasterRow.Cell("Activity").Value = ""
                                If PreviousWorkType = dt_UserWiseDWR.Rows(RowCnt)("vWorkTypeDesc").ToString() Then

                                    rMasterRow.Cell("WorkType").Value = ""

                                End If

                            End If

                        End If

                    End If

                End If

                PreviousUserId = dt_UserWiseDWR.Rows(RowCnt)("iUserId").ToString()
                PreviousDate = dt_UserWiseDWR.Rows(RowCnt)("dReportDate").ToString()
                PreviousProject = dt_UserWiseDWR.Rows(RowCnt)("vWorkSpaceDesc").ToString()
                PreviousSite = dt_UserWiseDWR.Rows(RowCnt)("vSiteName").ToString()
                PreviousActivity = dt_UserWiseDWR.Rows(RowCnt)("vActivityName").ToString()
                PreviousWorkType = dt_UserWiseDWR.Rows(RowCnt)("vWorkTypeDesc").ToString()

                'RowCnt = RowCnt + 1
                rPage.Say(rMasterRow)

                '**************Summary Row

                If dt_UserWiseDWR.Rows(RowCnt)("vWorkTypeDesc").ToString() = "OnSite" Then
                    TotalNoOfOnsiteWorkDays += 1
                End If
                If dt_UserWiseDWR.Rows(RowCnt)("vWorkTypeDesc").ToString() = "Office" Then
                    TotalNoOfOfficeWorkDays += 1
                End If
                If dt_UserWiseDWR.Rows(RowCnt)("vWorkTypeDesc").ToString() = "Travell" Then
                    TotalNoOfTravelledDays += 1
                End If
                If dt_UserWiseDWR.Rows(RowCnt)("vWorkTypeDesc").ToString() = "Other Work" Then
                    TotalNoOfOtherWorkDays += 1
                End If

                If Me.chkWithSummary.Checked Then

                    If RowCnt < dt_UserWiseDWR.Rows.Count - 1 Then
                        If PreviousUserId <> dt_UserWiseDWR.Rows(RowCnt + 1)("iUserId").ToString() Then

                            'To count holidays
                            wstr = "(cast(dHolidayDate as Datetime) >='" + Me.txtDWRFromDate.Text.Trim() + "'"
                            wstr += " And cast(dHolidayDate as Datetime) <='" + Me.txtDWRToDate.Text.Trim() + "')"
                            wstr += " And vLocationCode = " & Me.Session(S_LocationCode)

                            If Not objHelp.GetViewHolidayMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Holiday, estr) Then
                                Me.ObjCommon.ShowAlert("Error while Getting Data from ViewUserWiseDwr", Me.Page())
                                Exit Sub
                            End If
                            TotalNoOfHolidays = ds_Holiday.Tables(0).Rows.Count

                            FromDate = CType(Me.txtDWRFromDate.Text.Trim(), Date)
                            Todate = CType(Me.txtDWRToDate.Text.Trim(), Date)

                            Do While FromDate <= Todate

                                If FromDate.DayOfWeek = DayOfWeek.Sunday Then
                                    TotalNoOfSundays += 1
                                End If
                                FromDate = FromDate.AddDays(1)
                                TotalNoOfWorkDays += 1
                            Loop
                            TotalNoOfWorkDays = TotalNoOfWorkDays - (TotalNoOfHolidays + TotalNoOfSundays)

                            '*****************************************

                            'TotalNoOfWorkDays, TotalNoOfSundays, TotalNoOfHolidays, TotalNoOfOfficeWorkDays, TotalNoOfOnsiteWorkDays, TotalNoOfTravelledDays, TotalNoOfOtherWorkDays

                            rPage.SayBlankRow()

                            PrintMasterUserRowForSummary(dt_UserWiseDWR.Rows(RowCnt)("vUserName").ToString())
                            PrintMasterSummaryRow()

                            rMasterRowSummary.Cell("TotalNoOfWorkDays").Value = TotalNoOfWorkDays
                            rMasterRowSummary.Cell("TotalNoOfSundays").Value = TotalNoOfSundays
                            rMasterRowSummary.Cell("TotalNoOfHolidays").Value = TotalNoOfHolidays
                            rMasterRowSummary.Cell("TotalNoOfOfficeWorkDays").Value = TotalNoOfOfficeWorkDays
                            rMasterRowSummary.Cell("TotalNoOfOnsiteDays").Value = TotalNoOfOnsiteWorkDays
                            rMasterRowSummary.Cell("TotalNoOfTravelledDays").Value = TotalNoOfTravelledDays
                            rMasterRowSummary.Cell("TotalNoOfOtherWorkDays").Value = TotalNoOfOtherWorkDays
                            rPage.Say(rMasterRowSummary)

                            TotalNoOfWorkDays = 0
                            TotalNoOfSundays = 0
                            TotalNoOfHolidays = 0
                            TotalNoOfOfficeWorkDays = 0
                            TotalNoOfOnsiteWorkDays = 0
                            TotalNoOfTravelledDays = 0
                            TotalNoOfOtherWorkDays = 0

                            '*************************************
                        End If

                    End If

                    If RowCnt = dt_UserWiseDWR.Rows.Count - 1 Then

                        'To count holidays
                        wstr = "(cast(dHolidayDate as Datetime) >='" + Me.txtDWRFromDate.Text.Trim() + "'"
                        wstr += " And cast(dHolidayDate as Datetime) <='" + Me.txtDWRToDate.Text.Trim() + "')"
                        wstr += " And vLocationCode = " & Me.Session(S_LocationCode)

                        If Not objHelp.GetViewHolidayMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Holiday, estr) Then
                            Me.ObjCommon.ShowAlert("Error while Getting Data from ViewHolidayMst", Me.Page())
                            Exit Sub
                        End If
                        TotalNoOfHolidays = ds_Holiday.Tables(0).Rows.Count

                        FromDate = CType(Me.txtDWRFromDate.Text.Trim(), Date)
                        Todate = CType(Me.txtDWRToDate.Text.Trim(), Date)

                        Do While FromDate <= Todate

                            If FromDate.DayOfWeek = DayOfWeek.Sunday Then
                                TotalNoOfSundays += 1
                            End If
                            FromDate = FromDate.AddDays(1)
                            TotalNoOfWorkDays += 1
                        Loop
                        TotalNoOfWorkDays = TotalNoOfWorkDays - (TotalNoOfHolidays + TotalNoOfSundays)

                        '**************Summary Row
                        'TotalNoOfWorkDays, TotalNoOfSundays, TotalNoOfHolidays, TotalNoOfOfficeWorkDays, TotalNoOfOnsiteWorkDays, TotalNoOfTravelledDays, TotalNoOfOtherWorkDays

                        rPage.SayBlankRow()

                        PrintMasterUserRowForSummary(dt_UserWiseDWR.Rows(RowCnt)("vUserName").ToString())
                        PrintMasterSummaryRow()

                        rMasterRowSummary.Cell("TotalNoOfWorkDays").Value = TotalNoOfWorkDays
                        rMasterRowSummary.Cell("TotalNoOfSundays").Value = TotalNoOfSundays
                        rMasterRowSummary.Cell("TotalNoOfHolidays").Value = TotalNoOfHolidays
                        rMasterRowSummary.Cell("TotalNoOfOfficeWorkDays").Value = TotalNoOfOfficeWorkDays
                        rMasterRowSummary.Cell("TotalNoOfOnsiteDays").Value = TotalNoOfOnsiteWorkDays
                        rMasterRowSummary.Cell("TotalNoOfTravelledDays").Value = TotalNoOfTravelledDays
                        rMasterRowSummary.Cell("TotalNoOfOtherWorkDays").Value = TotalNoOfOtherWorkDays
                        rPage.Say(rMasterRowSummary)

                        TotalNoOfWorkDays = 0
                        TotalNoOfSundays = 0
                        TotalNoOfHolidays = 0
                        TotalNoOfOfficeWorkDays = 0
                        TotalNoOfOnsiteWorkDays = 0
                        TotalNoOfTravelledDays = 0
                        TotalNoOfOtherWorkDays = 0

                        '*************************************

                    End If
                End If
                RowCnt = RowCnt + 1
            Loop


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message + "Error While Print Detail", "")
        End Try

    End Sub


#End Region

#Region "Print MasterRow For Main Table"

    Private Function MasterRowMain() As RepoRow
        Dim rRow As RepoRow
        Dim rCell As RepoCell
        Dim i As Integer

        rRow = New RepoRow

        rCell = New RepoCell("Date")
        rRow.AddCell(rCell)

        rCell = New RepoCell("Day")
        rRow.AddCell(rCell)

        rCell = New RepoCell("Project")
        rRow.AddCell(rCell)

        rCell = New RepoCell("Site")
        rRow.AddCell(rCell)

        rCell = New RepoCell("Activity")
        rRow.AddCell(rCell)

        rCell = New RepoCell("WorkType")
        rRow.AddCell(rCell)

        rCell = New RepoCell("TimePeriod")
        rRow.AddCell(rCell)

        rCell = New RepoCell("TotalTime")
        rRow.AddCell(rCell)

        rCell = New RepoCell("ReasonIfOtherWork")
        rRow.AddCell(rCell)

        For i = 0 To rRow.CellCount - 1
            rRow.Cell(i).FontSize = 8
            rRow.Cell(i).LineBottom.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(i).LineTop.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(i).LineRight.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(i).LineLeft.LineType = RepoLine.LineTypeEnum.SingleLine
            'rRow.Cell(i).Width = 1.5
        Next i

        Return rRow

    End Function

    Private Sub PrintMasterRowMain()
        Dim rRow As RepoRow
        Dim i As Integer

        rRow = New RepoRow

        rRow = MasterRowMain()

        rRow.Cell("Date").Value = "Date"
        rRow.Cell("Day").Value = "Day"
        rRow.Cell("Project").Value = "Project"
        rRow.Cell("Site").Value = "Site"
        rRow.Cell("Activity").Value = "Activity"
        rRow.Cell("WorkType").Value = "Work Type"
        rRow.Cell("TimePeriod").Value = "Time Period"
        rRow.Cell("TotalTime").Value = "Total Time"
        rRow.Cell("ReasonIfOtherWork").Value = "Reason if Other Work"
        For i = 0 To rRow.CellCount - 1
            rRow.Cell(i).FontBold = True
            rRow.Cell(i).Alignment = RepoCell.AlignmentEnum.CenterTop
        Next

        rPage.Say(rRow)

    End Sub

#End Region

#Region "Print MasterRow For UserName"

    Private Function MasterUserRow() As RepoRow

        Dim rRow As RepoRow
        Dim rCell As RepoCell
        Dim i As Integer = 0

        rRow = New RepoRow

        rCell = New RepoCell("UserName")
        rRow.AddCell(rCell)
        rCell = New RepoCell("Value")
        rRow.AddCell(rCell)

        For i = 0 To rRow.CellCount - 1

            rRow.Cell(i).FontSize = 8
            rRow.Cell(i).LineBottom.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(i).LineTop.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(i).LineRight.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(i).LineLeft.LineType = RepoLine.LineTypeEnum.SingleLine

        Next i
        Return rRow

    End Function

    Private Sub PrintMasterUserRow(ByVal UserName As String)
        Dim rRow As RepoRow
        Dim i As Integer = 0

        rRow = New RepoRow

        rRow = MasterUserRow()

        rRow.Cell("UserName").Value = "User Name:"
        rRow.Cell("Value").Value = UserName

        For i = 0 To rRow.CellCount - 1

            rRow.Cell(i).FontBold = True
            rRow.Cell(i).Alignment = RepoCell.AlignmentEnum.CenterTop

        Next i

        rPage.Say(rRow)

    End Sub

#End Region

#Region "Print MasterRow For Summary Table"

    Private Function MasterSummaryRow() As RepoRow

        Dim rRow As RepoRow
        Dim rCell As RepoCell
        Dim i As Integer = 0

        rRow = New RepoRow

        rCell = New RepoCell("TotalNoOfWorkDays")
        rRow.AddCell(rCell)
        rCell = New RepoCell("TotalNoOfSundays")
        rRow.AddCell(rCell)
        rCell = New RepoCell("TotalNoOfHolidays")
        rRow.AddCell(rCell)
        rCell = New RepoCell("TotalNoOfOfficeWorkDays")
        rRow.AddCell(rCell)
        rCell = New RepoCell("TotalNoOfOnsiteDays")
        rRow.AddCell(rCell)
        rCell = New RepoCell("TotalNoOfTravelledDays")
        rRow.AddCell(rCell)
        rCell = New RepoCell("TotalNoOfOtherWorkDays")
        rRow.AddCell(rCell)

        For i = 0 To rRow.CellCount - 1

            rRow.Cell(i).FontSize = 8
            rRow.Cell(i).LineBottom.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(i).LineTop.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(i).LineRight.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(i).LineLeft.LineType = RepoLine.LineTypeEnum.SingleLine

        Next i
        Return rRow

    End Function
    'ByVal TotalNoOfWorkDays As Integer, ByVal TotalNoOfSundays As Integer, ByVal TotalNoOfHolidays As Integer, ByVal TotalNoOfOfficeWorkDays As Integer, ByVal TotalNoOfOnsiteWorkDays As Integer, ByVal TotalNoOfTravelledDays As Integer, ByVal TotalNoOfOtherWorkDays As Integer
    Private Sub PrintMasterSummaryRow()
        Dim rRow As RepoRow
        Dim i As Integer

        rRow = New RepoRow

        rRow = MasterSummaryRow()

        rRow.Cell("TotalNoOfWorkDays").Value = "Total WorkDays"
        rRow.Cell("TotalNoOfSundays").Value = "Total Sundays"
        rRow.Cell("TotalNoOfHolidays").Value = "Total Holidays"
        rRow.Cell("TotalNoOfOfficeWorkDays").Value = "Total Office WorkDays"
        rRow.Cell("TotalNoOfOnsiteDays").Value = "Total Onsite WorkDays"
        rRow.Cell("TotalNoOfTravelledDays").Value = "Total Travelled Days"
        rRow.Cell("TotalNoOfOtherWorkDays").Value = "Total Other WorkDays"

        For i = 0 To rRow.CellCount - 1
            rRow.Cell(i).FontBold = True
            rRow.Cell(i).Alignment = RepoCell.AlignmentEnum.CenterTop
        Next

        rPage.Say(rRow)

    End Sub

#End Region

#Region "Print MasterRow Of Summary Table For UserName"

    Private Function MasterUserRowForSummary() As RepoRow

        Dim rRow As RepoRow
        Dim rCell As RepoCell
        Dim i As Integer = 0

        rRow = New RepoRow
        'rCell = rRow.AddCell("Summary")
        'rCell.NoofCellContain = 7
        rCell = New RepoCell("Summary")
        rRow.AddCell(rCell)


        rCell = New RepoCell("UserName")
        'rCell = rRow.AddCell("UserName")
        'rCell.NoofCellContain = 7
        rRow.AddCell(rCell)

        For i = 0 To rRow.CellCount - 1

            rRow.Cell(i).FontSize = 8
            rRow.Cell(i).LineBottom.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(i).LineTop.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(i).LineRight.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(i).LineLeft.LineType = RepoLine.LineTypeEnum.SingleLine

        Next i
        Return rRow

    End Function

    Private Sub PrintMasterUserRowForSummary(ByVal UserName As String)
        Dim rRow As RepoRow
        Dim i As Integer = 0

        rRow = New RepoRow

        rRow = MasterUserRowForSummary()

        rRow.Cell("Summary").Value = "Summary:"
        rRow.Cell("UserName").Value = UserName

        For i = 0 To rRow.CellCount - 1

            rRow.Cell(i).FontBold = True
            rRow.Cell(i).Alignment = RepoCell.AlignmentEnum.CenterTop

        Next i

        rPage.Say(rRow)

    End Sub

#End Region

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        ObjCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        ObjCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub

#End Region

End Class
