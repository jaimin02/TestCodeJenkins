
Partial Class frmRptProjectWiseSiteResourceUtilization
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objcommon As New clsCommon
    Private objHelpDbTable As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()

    Private rPage As RepoPage
    Private Const VS_Choice As String = "Choice"
    Private Const VS_dtResourceUtilizationExpense As String = "Resource Utilization and Expense"

#End Region

#Region "Form Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall_ShowUI()
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......Page_Load")
        End Try
    End Sub

#End Region

#Region "GenCall_ShowUI"

    Private Function GenCall_ShowUI() As Boolean
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = ":: ProjectWiseSiteResourceUtilization Report :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Project Wise Site Resource Utilization Report"
            Choice = Me.ViewState("Choice")
            Me.pnlSTP.Visible = False
            Me.chkSelectAll.Visible = False

            Me.txtFromDate.Text = Date.Now.Date.ToString("dd-MMM-yyyy")
            Me.txtToDate.Text = Date.Now.Date.ToString("dd-MMM-yyyy")

            chkSelectAll.Attributes.Add("onclick", "CheckBoxListSelection(" + Me.chklstSTP.ClientID + ",this);")

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_ShowUI")
        Finally
        End Try
    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        fillchklstSite()
    End Sub

    Protected Sub BtnAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAll.Click
        Me.txtProject.Text = ""
        fillchklstSite(True)
    End Sub

    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGo.Click
        Dim ds_ResourceUtilizationExpense As New Data.DataSet
        Dim estr As String = String.Empty
        Dim Qstr As String = String.Empty

        Dim FileName As String = String.Empty
        Dim isReportComplete As Boolean = False
        Dim CntUser As Integer = 0
        Dim strSTP As String = String.Empty
        Try
            Do While CntUser <= chklstSTP.Items.Count - 1

                If Me.chklstSTP.Items(CntUser).Selected = True Then
                    strSTP += Me.chklstSTP.Items(CntUser).Value.Trim()
                    strSTP += ","
                End If
                CntUser += 1

            Loop
            strSTP = strSTP.Substring(0, strSTP.Length - 1)
            Qstr = "select Distinct View_STP.vWorkspaceId,View_STP.vWorkSpaceDesc,View_STP.nSTPNo,View_STP.vSiteName," + _
                    " DWTDtl.TotalUsers, DWTDtl.TotalHr, ExpDtl.totalStpExp" + _
                    " from View_STP" + _
                    " Left Join (" + _
                    " select nVisitedSTPNo,count(Distinct iUserId)as TotalUsers,Sum(cast(diffHH_MM as Numeric(18,2))) as TotalHr " + _
                    " from View_UserWiseDWR where cast(dReportDate as datetime)>= '" + Me.txtFromDate.Text.Trim() + "' and cast(dReportDate as datetime)<= '" + Me.txtToDate.Text.Trim() + "'" + _
                    " group by nVisitedSTPNo" + _
                    " ) as DWTDtl" + _
                    " on View_STP.nSTPNo=DWTDtl.nVisitedSTPNo" + _
                    " left join " + _
                    " (select nSTPNo,sum(iExpAmt) as totalStpExp from View_UserWiseExpense " + _
                    " where cast(dfromDate as datetime)>= '" + Me.txtFromDate.Text.Trim() + "' and cast(dToDate as datetime)<= '" + Me.txtToDate.Text.Trim() + "'" + _
                    " group by nSTPNo)as ExpDtl" + _
                    " on View_STP.nSTPNo = ExpDtl.nSTPNo" + _
                    " Where " + _
                    " View_STP.nSTPNo in (" + strSTP + ")"
          
            ds_ResourceUtilizationExpense = Me.objHelpDbTable.GetResultSet(Qstr, "Ds_ResourceUtilization")

            If ds_ResourceUtilizationExpense.Tables(0).Rows.Count < 1 Then
                objcommon.ShowAlert("No Records For Selected Criteria", Me.Page)
                Exit Sub
            End If

            Me.ViewState(VS_dtResourceUtilizationExpense) = ds_ResourceUtilizationExpense.Tables(0)

            FileName = GetReportName() + ".xls"
            FileName = Server.MapPath("~/ExcelReportFile/" + FileName)

            OpenReport(FileName)

            ReportHeader()
            ReportDetail()

            isReportComplete = True


        Catch ex As Exception
            isReportComplete = False
            Me.ShowErrorMessage(ex.Message, "....BtnGo_Click")
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
        Me.resetPage()
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

#Region "FillCheckBoxList"

    Private Sub fillchklstSite(Optional ByVal All As Boolean = False)
        Dim ds_Site As New DataSet
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Try
            'To Get Where condition of ScopeVales( Project Type )
            If Not objcommon.GetScopeValueWithCondition(Wstr) Then
                Exit Sub
            End If

            If Not All Then
                Wstr += " And vWorkspaceId='" & Me.HProjectId.Value.ToString.Trim() & "'"
            End If

            If Not Me.objHelpDbTable.GetViewSTPWithScope(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                ds_Site, estr) Then
                Me.objcommon.ShowAlert("Error While Getting Site UserWise", Me.Page)
                Exit Sub
            End If

            If ds_Site.Tables(0).Rows.Count < 1 Then
                Me.pnlSTP.Visible = False
                Me.chkSelectAll.Visible = False
                Exit Sub
            End If

            Me.chklstSTP.DataSource = ds_Site.Tables(0)
            Me.chklstSTP.DataValueField = "nSTPNo"
            Me.chklstSTP.DataTextField = "vSiteName"
            Me.chklstSTP.DataBind()
            Me.pnlSTP.Visible = True
            Me.chkSelectAll.Visible = True

            For Each lstItem As ListItem In chklstSTP.Items

                lstItem.Attributes.Add("onclick", "SetCheckUncheckAll(document.getElementById('" + _
                                                Me.chklstSTP.ClientID + "'), document.getElementById('" + _
                                                Me.chkSelectAll.ClientID + "'));")

            Next lstItem

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...fillchklstSite")
        End Try
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

#Region "Printing Report"

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
        rCell = rRow.AddCell("ReportTitle")
        rCell.Value = "Project Wise Site Wise Resource Utilization And Expense Report"
        rCell.FontBold = True
        rCell.FontSize = 12
        'rCell.FontColor = Drawing.Color.LightGray
        rCell.NoofCellContain = 5
        rCell.Alignment = RepoCell.AlignmentEnum.CenterTop
        rPage.Say(rRow)

        rRow = New RepoRow
        rCell = rRow.AddCell("FromToDate")
        rCell.Value = "From Date:" + Me.txtFromDate.Text.Trim() + " To:" + Me.txtToDate.Text.Trim()
        rCell.FontBold = False
        rCell.FontSize = 12
        rCell.NoofCellContain = 5
        rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
        'rCell.FontColor = Drawing.Color.LightGray
        rPage.Say(rRow)

        'rPage.SayBlankRow()
    End Sub

    Private Sub ReportDetail()
        Dim rMasterRow As RepoRow
        Dim RowCnt As Integer
        Dim dt_ResourceUtilizationExpense As DataTable

        Dim PreviousProject As String = String.Empty
        Dim PreviousActivity As String = String.Empty
        Try
            dt_ResourceUtilizationExpense = Me.ViewState(VS_dtResourceUtilizationExpense)

            rMasterRow = MasterRow()
            rPage.SayBlankRow()
            PrintMasterRow()

            RowCnt = 0

            Do While RowCnt <= dt_ResourceUtilizationExpense.Rows.Count - 1

                rMasterRow.Cell("Project").Value = dt_ResourceUtilizationExpense.Rows(RowCnt)("vWorkSpaceDesc").ToString()
                rMasterRow.Cell("Site").Value = dt_ResourceUtilizationExpense.Rows(RowCnt)("vSiteName").ToString()
                rMasterRow.Cell("Total No. of Users").Value = dt_ResourceUtilizationExpense.Rows(RowCnt)("TotalUsers").ToString()
                rMasterRow.Cell("Total No. of Hours").Value = dt_ResourceUtilizationExpense.Rows(RowCnt)("TotalHr").ToString()
                rMasterRow.Cell("Total Expense").Value = dt_ResourceUtilizationExpense.Rows(RowCnt)("totalStpExp").ToString()


                If PreviousProject = dt_ResourceUtilizationExpense.Rows(RowCnt)("vWorkSpaceDesc").ToString() Then

                    rMasterRow.Cell("Project").Value = ""

                    If PreviousActivity = dt_ResourceUtilizationExpense.Rows(RowCnt)("vSiteName").ToString() Then
                        rMasterRow.Cell("Site").Value = ""
                    End If

                End If

                PreviousProject = dt_ResourceUtilizationExpense.Rows(RowCnt)("vWorkSpaceDesc").ToString()
                PreviousActivity = dt_ResourceUtilizationExpense.Rows(RowCnt)("vSiteName").ToString()

                rPage.Say(rMasterRow)
                RowCnt = RowCnt + 1
            Loop

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message + "Error While Print Detail", "")
        End Try

    End Sub

    Private Function MasterRow() As RepoRow
        Dim rRow As RepoRow
        Dim rCell As RepoCell
        Dim index As Integer

        rRow = New RepoRow

        rCell = New RepoCell("Project")
        rRow.AddCell(rCell)

        rCell = New RepoCell("Site")
        rRow.AddCell(rCell)

        rCell = New RepoCell("Total No. of Users")
        rRow.AddCell(rCell)

        rCell = New RepoCell("Total No. of Hours")
        rRow.AddCell(rCell)

        rCell = New RepoCell("Total Expense")
        rRow.AddCell(rCell)

        For index = 0 To rRow.CellCount - 1
            rRow.Cell(index).FontSize = 8
            rRow.Cell(index).LineBottom.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(index).LineTop.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(index).LineRight.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(index).LineLeft.LineType = RepoLine.LineTypeEnum.SingleLine
            'rRow.Cell(index).Width = 1.5
        Next index

        Return rRow

    End Function

    Private Sub PrintMasterRow()
        Dim rRow As RepoRow
        Dim index As Integer

        rRow = New RepoRow

        rRow = MasterRow()

        rRow.Cell("Project").Value = "Project"
        rRow.Cell("Site").Value = "Site"
        rRow.Cell("Total No. of Users").Value = "Total No. of Users"
        rRow.Cell("Total No. of Hours").Value = "Total No. of Hours"
        rRow.Cell("Total Expense").Value = "Total Expense"

        For index = 0 To rRow.CellCount - 1
            rRow.Cell(index).FontBold = True
            rRow.Cell(index).Alignment = RepoCell.AlignmentEnum.CenterTop
        Next

        rPage.Say(rRow)

    End Sub

#End Region

#Region "Reset Page"

    Protected Sub resetPage()

        Me.HProjectId.Value = ""
        Me.txtproject.Text = ""
        Me.ViewState(VS_dtResourceUtilizationExpense) = Nothing
        Me.txtFromDate.Text = ""
        Me.txtToDate.Text = ""
        Me.chkSelectAll.Checked = False
        Me.chklstSTP.ClearSelection()

        If Not GenCall_ShowUI() Then
            Exit Sub
        End If

    End Sub

#End Region

End Class
