
Partial Class frmSourceComments
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private rPage As RepoPage

    Private Const VS_dtQAComments As String = "dt_QAComments"
    Private Const VS_dtSubjects As String = "dt_Subjects"

#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not GenCallShowUI() Then
                Exit Sub
            End If
        End If
    End Sub

#End Region

#Region "GenCallShowUI "

    Private Function GenCallShowUI() As Boolean

        Try
            Page.Title = ":: QA Comments Report  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "QA Comments Report"
            Me.txtFromDate.Text = Date.Today.AddDays(-7).ToString("dd-MMM-yy")
            Me.txtToDate.Text = Date.Today.ToString("dd-MMM-yy")
            GenCallShowUI = True
        Catch ex As Exception
            GenCallShowUI = False
        End Try

    End Function

#End Region

#Region "Generate Report "

    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Dim str As String = ""
        Dim FileName As String = ""
        Dim isReportComplete As Boolean = False

        Try
            Dim a As New ArrayList
            a.Add("sad")

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
            'ReportGeneralFunction.ExportFileAtWeb(ReportGeneralFunction.ExportFileAsEnum.ExportAsXLS, HttpContext.Current.Response, FileName, Me.Server.MapPath(""))
            ReportGeneralFunction.ExportFileAtWeb(ReportGeneralFunction.ExportFileAsEnum.ExportAsXLS, HttpContext.Current.Response, FileName)
        End If

    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

#Region "Report Helper Functions"

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

        rRow = New RepoRow
        rCell = rRow.AddCell("CompanyTitle")
        rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
        rCell.FontBold = True
        rCell.FontSize = 14
        rCell.Value = System.Configuration.ConfigurationManager.AppSettings("Client")
        rCell.NoofCellContain = 10
        rCell.FontColor = Drawing.Color.Maroon
        rCell.BackgroundColor = Drawing.Color.White
        rPage.Say(rRow)

        rRow = New RepoRow
        rCell = rRow.AddCell("ReportTitle")
        rCell.Value = "QA Comments Report"
        rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
        rCell.FontBold = True
        rCell.FontSize = 12
        rCell.FontColor = Drawing.Color.Maroon
        rCell.NoofCellContain = 10
        rPage.Say(rRow)

        rRow = New RepoRow
        rCell = rRow.AddCell("ProjectName")
        rCell.Value = "Project: " & Me.txtproject.Text.Trim() & "( " & Me.HProjectId.Value & " )"
        rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
        rCell.FontBold = True
        rCell.FontSize = 12
        rCell.FontColor = Drawing.Color.Maroon
        rCell.NoofCellContain = 12
        rPage.Say(rRow)

        'rRow = New RepoRow
        'rCell = rRow.AddCell("ProjectName")
        'rCell.Value = "Project: " & Me.ddlProject.SelectedItem.Text.Trim()
        'rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
        'rCell.FontBold = True
        'rCell.FontSize = 12
        'rCell.FontColor = Drawing.Color.Maroon
        'rCell.NoofCellContain = 6
        'rPage.Say(rRow)

        rRow = New RepoRow
        rCell = rRow.AddCell("DatePeriod")
        rCell.Value = "Period: " & Me.txtFromDate.Text.Trim() & " To " & Me.txtToDate.Text.Trim()
        rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
        rCell.FontBold = True
        rCell.FontSize = 12
        rCell.FontColor = Drawing.Color.Maroon
        rCell.NoofCellContain = 10
        rPage.Say(rRow)

        rPage.SayBlankRow()

    End Sub

    Private Sub PrintHeader()
        Dim rRow As RepoRow
        Dim index As Integer

        rRow = New RepoRow

        rRow = masterRow()
        rRow.Cell("SubjectId").Value = "Subject Id" 'Me.RblCommentsOn.SelectedItem.Text.Trim() + 
        rRow.Cell("SubjectName").Value = "Subject Name"

        'If Me.RblCommentsOn.SelectedValue.ToUpper.Trim() = "PIF" Then
        '    rRow.Cell("Date").Value = "Enrollment Date"
        'ElseIf Me.RblCommentsOn.SelectedValue.ToUpper.Trim() = "MSR" Then
        '    rRow.Cell("Date").Value = "Screening Date"
        'End If

        rRow.Cell("SrNo").Value = "Sr No."

        rRow.Cell("QAComments").Value = "QC Comments"
        'rRow.Cell("QCFlag").Value = "QA Flag"
        rRow.Cell("QABy").Value = "QC By"
        rRow.Cell("QAOn").Value = "QC On"
        rRow.Cell("Response").Value = "Response"
        rRow.Cell("ResponseBy").Value = "Response By"
        rRow.Cell("ResponseOn").Value = "Response On"

        For index = 0 To rRow.CellCount - 1
            rRow.Cell(index).FontBold = True
            rRow.Cell(index).Alignment = RepoCell.AlignmentEnum.CenterTop
        Next

        rPage.Say(rRow)

    End Sub

    Private Sub ReportDetail()
        Dim RowCnt As Integer
        Dim rRow As RepoRow
        Dim dt_Report As New DataTable
        Dim PreviousCompany As String = ""
        Dim PreviousProject As String = ""
        Try

            rRow = masterRow()
            PrintHeader()

            If Not GetData(dt_Report) Then
                Exit Sub
            End If

            RowCnt = 0

            Do While RowCnt <= dt_Report.Rows.Count - 1

                rRow.Cell("SubjectId").Value = dt_Report.Rows(RowCnt)("vSubjectId").ToString()
                rRow.Cell("SubjectName").Value = dt_Report.Rows(RowCnt)("FullName").ToString()

                'If Me.RblCommentsOn.SelectedValue.ToUpper.Trim() = "PIF" Then
                '    rRow.Cell("Date").Value = CType(dt_Report.Rows(RowCnt)("dEnrollmentDate"), DateTime).ToString("dd-MMM-yyyy")
                'ElseIf Me.RblCommentsOn.SelectedValue.ToUpper.Trim() = "MSR" Then
                '    rRow.Cell("Date").Value = CType(dt_Report.Rows(RowCnt)("dScreenDate"), DateTime).ToString("dd-MMM-yyyy")
                'End If

                rRow.Cell("SrNo").Value = dt_Report.Rows(RowCnt)("iTranNo").ToString()

                rRow.Cell("QAComments").Value = dt_Report.Rows(RowCnt)("vQCComment").ToString()
                rRow.Cell("QABy").Value = dt_Report.Rows(RowCnt)("vQCGivenBy").ToString()
                rRow.Cell("QAOn").Value = ""

                If Not dt_Report.Rows(RowCnt)("dQCGivenOn") Is DBNull.Value Then
                    rRow.Cell("QAOn").Value = CType(dt_Report.Rows(RowCnt)("dQCGivenOn"), DateTime).ToString("dd-MMM-yyyy hh:mm")
                End If

                rRow.Cell("Response").Value = dt_Report.Rows(RowCnt)("vResponse").ToString()
                rRow.Cell("ResponseBy").Value = dt_Report.Rows(RowCnt)("vResponseGivenBy").ToString()
                rRow.Cell("ResponseOn").Value = ""

                If Not dt_Report.Rows(RowCnt)("dResponseGivenOn") Is DBNull.Value Then
                    rRow.Cell("ResponseOn").Value = CType(dt_Report.Rows(RowCnt)("dResponseGivenOn"), DateTime).ToString("dd-MMM-yyyy hh:mm")
                End If

                rPage.Say(rRow)
                RowCnt = RowCnt + 1

            Loop ''detail loop ending

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message + "Error While Print Detail", "")
        End Try
    End Sub

    Private Function masterRow() As RepoRow
        Dim rRow As RepoRow
        Dim rCell As RepoCell
        Dim i As Integer

        rRow = New RepoRow

        rCell = New RepoCell("SubjectId")
        rRow.AddCell(rCell)

        rCell = New RepoCell("SubjectName")
        rRow.AddCell(rCell)

        'rCell = New RepoCell("Date")
        'rRow.AddCell(rCell)

        rCell = New RepoCell("SrNo")
        rRow.AddCell(rCell)

        rCell = New RepoCell("QAComments")
        rRow.AddCell(rCell)

        rCell = New RepoCell("QABy")
        rRow.AddCell(rCell)

        rCell = New RepoCell("QAOn")
        rRow.AddCell(rCell)

        rCell = New RepoCell("Response")
        rRow.AddCell(rCell)


        rCell = New RepoCell("ResponseBy")
        rRow.AddCell(rCell)

        rCell = New RepoCell("ResponseOn")
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

#End Region

#Region " GetData "
    Private Function GetData(ByRef dt_Report_Retu As DataTable) As Boolean

        Dim wStr As String = ""
        Dim eStr_Retu As String = ""
        Dim Val As String = ""
        Dim ds As DataSet = Nothing
        Dim Ds_QADetail As New DataSet
        Dim Dv_QADetail As New DataView
        Dim FromDate As String = ""
        Dim ToDate As String = ""
        Dim dr As DataRow
        Dim Dt_Subject As New DataTable

        Dim Subjects As String = ""

        Try
            If Me.txtFromDate.Text.Trim() <> "" Then
                FromDate = DateTime.Parse(Me.txtFromDate.Text).ToString("dd-MMM-yyyy")
            End If

            If Me.txtToDate.Text.Trim() <> "" Then
                ToDate = DateTime.Parse(Me.txtToDate.Text).ToString("dd-MMM-yyyy")
            End If

            wStr = "cast(convert(varchar(11),dQCGivenOn,113)as smalldatetime)>=cast(convert(varchar(11),cast('" & FromDate & "' as datetime),113)as smalldatetime) And cast(convert(varchar(11),dQCGivenOn,113)as smalldatetime)<= cast(convert(varchar(11),cast('" & ToDate & "' as datetime),113)as smalldatetime)"
            '"replace(convert(varchar(11),dQCGivenOn,113),' ','-') >= '" + FromDate + "' And replace(convert(varchar(11),dQCGivenOn,113),' ','-') <= '" + ToDate + "'"


            If Me.chkAll.Checked = False Then

                Dt_Subject = CType(Me.ViewState(VS_dtSubjects), DataTable)

                If Dt_Subject.Rows.Count > 0 Then

                    For Each dr In Dt_Subject.Rows

                        If Subjects = "" Then
                            Subjects = "'" + dr("vSubjectId") + "'"
                            Continue For
                        End If

                        Subjects += ",'" + dr("vSubjectId") + "'"

                    Next dr

                    wStr += " And vSubjectId in (" & Subjects.Trim() & ")"
                End If

            End If

            wStr += ""




            If Not objHelp.View_SubjectMasterQC(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    Ds_QADetail, eStr_Retu) Then

                ShowErrorMessage(eStr_Retu, "")
                Exit Function

            End If

            'ElseIf Me.RblCommentsOn.SelectedValue.ToUpper.Trim() = "MSR" Then

            'If Not objHelp.View_MedexScreeningHdrQc(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '        Ds_QADetail, eStr_Retu) Then

            '    ShowErrorMessage(eStr_Retu, "")
            '    Exit Function

            'End If



            If Ds_QADetail.Tables(0).Rows.Count > 0 Then
                Dv_QADetail = Ds_QADetail.Tables(0).DefaultView
                Dv_QADetail.Sort = "FullName,iTranNo"
                dt_Report_Retu = Dv_QADetail.ToTable()
            Else
                ObjCommon.ShowAlert("No Records Found.", Me)
            End If

            GetData = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")

        Finally

        End Try
    End Function
#End Region

#Region "Set Project"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click

        Dim ds_Subject As New DataSet
        Dim estr As String = ""
        Dim wstr As String = ""

        wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "'"

        If Not Me.objHelp.GetViewWorkspaceSubjectMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                ds_Subject, estr) Then
            Exit Sub
        End If

        Me.ViewState(VS_dtSubjects) = ds_Subject.Tables(0).DefaultView.ToTable(True, "vSubjectId")

    End Sub

#End Region

#Region "Error Handler"
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objcommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objcommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub
#End Region


End Class
