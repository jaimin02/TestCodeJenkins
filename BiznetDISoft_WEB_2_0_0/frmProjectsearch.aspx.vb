
Partial Class frmProjectsearch
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATIONS"

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const VS_grid As String = "ds_Grid"
    Private Const VS_Report As String = "ds_Grid"
    Private rPage As RepoPage
    Private index1 As Integer = 0
    Private wstr As String = String.Empty

    Private Const GVC_ProjecttypeCode As Integer = 0
    Private Const GVC_ProjectNo As Integer = 1
    Private Const GVC_RequestId As Integer = 2
    Private Const GVC_Workspacedesc As Integer = 3
    Private Const GVC_ClientName As Integer = 4
    Private Const GVC_DrugName As Integer = 5
    Private Const GVC_BrandName As Integer = 6
    Private Const GVC_vProjectManager As Integer = 7
    Private Const GVC_vProjectCoordinator As Integer = 8
    Private Const GVC_NoOfSubjects As Integer = 9
    Private Const GVC_retentionperiod As Integer = 10
    Private Const GVC_FAstfed As Integer = 11
    Private Const GVC_vRegionName As Integer = 12
    Private Const GVC_ProjectStatus As Integer = 13

    Private Const GVC_Checkin1Start As Integer = 14
    Private Const GVC_IPAdministration1Start As Integer = 15
    Private Const GVC_Checkin2Start As Integer = 16
    Private Const GVC_IPAdministration2Start As Integer = 17
    Private Const GVC_Checkin3Start As Integer = 18
    Private Const GVC_IPAdministration3Start As Integer = 19

    Private Const GVC_Checkin4Start As Integer = 20
    Private Const GVC_IPAdministration4Start As Integer = 21
    Private Const GVC_Checkin5Start As Integer = 22
    Private Const GVC_IPAdministration5Start As Integer = 23
    Private Const GVC_Detail As Integer = 24
    Private Const GVC_WorkspaceId As Integer = 25

#End Region

#Region "Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Not IsPostBack Then

                GenCall_ShowUI()

            End If

        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim dt_OpMst As DataTable = Nothing
        Dim FromDate As String = String.Empty
        Dim TODate As String = String.Empty

        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = ":: Project Search  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")


            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Project Search"

            FillDropDown()

            Me.BtnGo1.Visible = True
            Me.DivSpDrug.Visible = False
            If Me.txtFromDate.Text.Trim() <> "" Then
                FromDate = DateTime.Parse(Me.txtFromDate.Text).ToString("dd-MMM-yyyy")
            End If

            If Me.txtToDate.Text.Trim() <> "" Then
                TODate = DateTime.Parse(Me.txtToDate.Text).ToString("dd-MMM-yyyy")
            End If

            txtFromDate.Text = FromDate
            txtToDate.Text = TODate



            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......GenCall_ShowUI")
        Finally
        End Try
    End Function

#End Region

#Region "FillDropDown"

    Private Function FillDropDown() As Boolean
        Dim ds_Proj As New Data.DataSet
        Dim estr As String = String.Empty
        Dim Wstr_Scope As String = String.Empty
        Dim dv_ProjectType As New DataView
        Try


            'To Get Where condition of ScopeVales( Project Type )
            If Not ObjCommon.GetScopeValueWithCondition(Wstr_Scope) Then
                Exit Function
            End If

            If Not objHelp.GetviewProjectTypeMst(Wstr_Scope, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Proj, estr) Then
                Me.ShowErrorMessage(estr, "")
                Return False
            End If

            dv_ProjectType = ds_Proj.Tables(0).DefaultView
            dv_ProjectType.Sort = "vProjectTypeName"
            Me.DDLProType.DataSource = dv_ProjectType
            Me.DDLProType.DataValueField = "vProjectTypeCode"
            Me.DDLProType.DataTextField = "vProjectTypeName"
            Me.DDLProType.DataBind()
            Me.DDLProType.Items.Insert(0, New ListItem("All Project Type", "ALL"))

            Me.DDLProStatus.Items.Insert(0, New ListItem("Select Project Status", 0))
            Me.DDLProStatus.Items.Insert(1, New ListItem("Not Rewarded", 1))
            Me.DDLProStatus.Items.Insert(2, New ListItem("All OnGoing", 2))
            Me.DDLProStatus.Items.Insert(3, New ListItem("All Completed", 3))
            Me.DDLProStatus.Items.Insert(4, New ListItem("All Archieved", 4))
            Me.DDLProStatus.Items.Insert(5, New ListItem("Client Request", 5))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......FillDropDown")
            Return False
        End Try
    End Function

    Private Function fillSpAndDrug() As Boolean
        Dim ds_SpAndDrug As New Data.DataSet
        Dim dt_Sponsor As New DataTable
        Dim dt_Drug As New DataTable
        Dim dt_SpAndDrug As New DataTable
        Dim estr As String = String.Empty
        Dim ProjType As String = String.Empty
        Dim Status As String = String.Empty
        Dim strDrug(1) As String
        Dim strSponsor(1) As String
        Dim Wstr_Scope As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim dv_Sponsor As New DataView
        Dim dv_Drug As New DataView
        Try



            'To Get Where condition of ScopeVales( Project Type )
            If Not ObjCommon.GetScopeValueWithCondition(Wstr_Scope) Then
                Exit Function
            End If

            Wstr = Wstr_Scope

            If Me.DDLProType.SelectedIndex <> 0 Then
                Wstr += " AND vProjectTypeCode='" & Me.DDLProType.SelectedValue.Trim() & "'"
            End If

            If Me.DDLProStatus.SelectedValue = 1 Then
                Status = "'N'"
            ElseIf Me.DDLProStatus.SelectedValue = 2 Then
                Status = "'S','D'"
            ElseIf Me.DDLProStatus.SelectedValue = 3 Then
                Status = "'C'"
            ElseIf Me.DDLProStatus.SelectedValue = 4 Then
                Status = "'A'"
            ElseIf Me.DDLProStatus.SelectedValue = 5 Then
                Status = "'I'"
            End If
            Wstr += " AND cProjectStatus in (" + Status + ")"

            If Not objHelp.GetViewgetWorkspaceDetailForHdr(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SpAndDrug, estr) Then
                Me.ShowErrorMessage(estr, "")
                Return False
            End If

            dt_SpAndDrug = ds_SpAndDrug.Tables(0)

            'Distinct vClientCode
            strSponsor(0) = "vClientCode"
            strSponsor(1) = "vClientName"
            dt_Sponsor = ObjCommon.SelectDistinct(dt_SpAndDrug, strSponsor)

            dv_Sponsor = dt_Sponsor.DefaultView
            dv_Sponsor.Sort = "vClientName"
            Me.DDLSponsor.DataSource = dv_Sponsor
            Me.DDLSponsor.DataValueField = "vClientCode"
            Me.DDLSponsor.DataTextField = "vClientName"
            Me.DDLSponsor.DataBind()
            Me.DDLSponsor.Items.Insert(0, New ListItem("All Sponsor", "ALL"))

            'Distinct vDrugCode
            strDrug(0) = "vDrugCode"
            strDrug(1) = "vDrugName"
            dt_Drug = ObjCommon.SelectDistinct(dt_SpAndDrug, strDrug)

            dv_Drug = dt_Drug.DefaultView
            dv_Drug.Sort = "vDrugName"
            Me.DDLDrug.DataSource = dv_Drug
            Me.DDLDrug.DataValueField = "vDrugCode"
            Me.DDLDrug.DataTextField = "vDrugName"
            Me.DDLDrug.DataBind()
            Me.DDLDrug.Items.Insert(0, New ListItem("All Drug", "ALL"))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....fillSpAndDrug")
            Return False
        End Try
    End Function

    Protected Sub DDLSegment_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLSegment.SelectedIndexChanged
        Dim wstr As String = String.Empty

        '==Added by: Vikas(TO fil location based on segment)As said by Deepak Bhai=====

        If DDLSegment.SelectedIndex = 1 Then
            wstr = " where cStatusindi <>'D' AND vRemark in ('IN','IND')"
        End If
        If DDLSegment.SelectedIndex = 2 Then
            wstr = " where cStatusIndi <>'D' And vRemark='PL'"
        End If
        If DDLSegment.SelectedIndex = 3 Then
            wstr = " where cStatusIndi <> 'D' And vRemark='CAN'"
        End If

        Dim estr_Retu As String = ""
        Dim ds_Location As New DataSet
        Try
            If Not objHelp.getLocationMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_Location, estr_Retu) Then
                ObjCommon.ShowAlert("Error whille Filling Location", Me.Page)
                Exit Sub
            End If

            DDLLocation.DataSource = ds_Location
            DDLLocation.DataTextField = "vLocationName"
            DDLLocation.DataValueField = "vLocationCode"
            DDLLocation.DataBind()
            Me.DDLLocation.Items.Insert(0, New ListItem("All Locations", 0))
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......DDLSegment_SelectedIndexChanged")
        Finally
        End Try


    End Sub

#End Region

#Region "Button Click"

    Protected Sub BtnGo1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGo1.Click

        If Not fillSpAndDrug() Then
            ObjCommon.ShowAlert("Error Whille Filling Sponsor And Drug", Me.Page)
            Exit Sub
        End If

        Me.DivSpDrug.Visible = True
        Me.BtnGo1.Visible = False

    End Sub

    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGo.Click
        '===Added By:vikas (Click on go button without selection of segment so i have added to remove that error)discussed with vishal=====

        If DDLSegment.SelectedIndex = 0 Then
            ObjCommon.ShowAlert("Please Select Segment !", Me.Page)
            Return
        End If
        If Not FillGrid() Then
            ObjCommon.ShowAlert("No Record Found !", Me.Page)
        Else
            Me.BtnExportExcel.Visible = True
        End If

    End Sub

    Protected Sub BtnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub BtnExportExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportExcel.Click
        Dim str As String = String.Empty
        Dim FileName As String = String.Empty
        Dim isReportComplete As Boolean = False
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim cnt As Integer = 0

        Try
            'divExportClient.Attributes.Add("style", "display:none")
            'dt = CType(ViewState(VS_grid), DataTable)
            'ds.Tables.Add(dt)
            'ds.AcceptChanges()
            ds = CType(ViewState(VS_grid), DataSet)
            Dim a As New ArrayList
            a.Add("sad")

            FileName = GetReportName() + ".xls"
            FileName = Server.MapPath("~/ExcelReportFile/" + FileName)

            OpenReport(FileName)

            ReportHeader()

            ReportDetail(ds)

            isReportComplete = True

        Catch ex As Exception
            isReportComplete = False
            Me.ShowErrorMessage(ex.Message, "......BtnExportExcel_Click")
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

    

#End Region

#Region "FillGrid"

    Private Function FillGrid() As Boolean
        Dim ds_Grid As New Data.DataSet
        Dim ProjType As String = String.Empty
        Dim Status As String = String.Empty
        Dim Sponsor As String = String.Empty
        Dim Drug As String = String.Empty
        Dim estr As String = String.Empty
        Dim Wstr_Scope As String = String.Empty
        Dim dt_grid As New DataTable
        'Dim Wstr As String = ""
        Try

            
            'For Poject Type
            ProjType = Me.DDLProType.SelectedValue.Trim()

            'For Project status
            If Me.DDLProStatus.SelectedValue = 0 Then
                Me.ObjCommon.ShowAlert("Please Select Project Status", Me.Page)
                Return False
            ElseIf Me.DDLProStatus.SelectedValue = 1 Then
                Status = "N"
            ElseIf Me.DDLProStatus.SelectedValue = 2 Then
                Status = "S,D"
            ElseIf Me.DDLProStatus.SelectedValue = 3 Then
                Status = "C"
            ElseIf Me.DDLProStatus.SelectedValue = 4 Then
                Status = "A"
            ElseIf Me.DDLProStatus.SelectedValue = 5 Then
                Status = "I"
            End If

            'For Sponsor
            Sponsor = Me.DDLSponsor.SelectedValue.Trim()

            'For Drug
            Drug = Me.DDLDrug.SelectedValue.Trim()

            'To Get Where condition of ScopeVales( Project Type )
            If Not ObjCommon.GetScopeValueWithCondition(Wstr_Scope) Then
                Exit Function
            End If



            If chkAllDate.Checked = True Then
                wstr = Wstr_Scope
            ElseIf chkAllDate.Checked = False Then
                wstr = Wstr_Scope
                wstr += " and cast(convert(varchar(11),dCreatedOn,113) as smalldatetime)>= cast(convert(varchar(11),cast('" & Me.txtFromDate.Text.Trim & "' as datetime),113)as smalldatetime) And cast(convert(varchar(11),dCreatedOn,113)as smalldatetime)<= cast(convert(varchar(11),cast('" & Me.txtToDate.Text.Trim & "' as datetime),113)as smalldatetime)"
            End If
            If ProjType.ToUpper <> "ALL" Then
                wstr += " And vProjectTypeCode='" & ProjType & "'"
            End If

            If Sponsor.ToUpper <> "ALL" Then
                wstr += " And vClientCode='" & Sponsor & "'"
            End If

            If Drug.ToUpper <> "ALL" Then
                wstr += " And vDrugCode='" & Drug & "'"
            End If
            '==added on 19-jan-2010 by deepak singh to enable user to select allLocation 
            If Me.DDLLocation.SelectedValue = 0 Then
                wstr += "AND charindex(cProjectStatus,'" & Status & "')>0  order By vWorkspaceID "
            ElseIf Me.DDLLocation.SelectedValue <> 0 Then

                wstr += "AND vLocationCode='" + Me.DDLLocation.SelectedValue + "' AND charindex(cProjectStatus,'" & Status & "')>0  order By vWorkspaceID "
            End If
            '============

            If Not objHelp.View_WorkspaceDtlForHdrwithCurrAttr(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Grid, estr) Then
                Me.ObjCommon.ShowAlert("Error while getting Data:" & estr, Me.Page)
                Return False
            End If

            '===added on 7-dec-2009==
            ViewState(VS_grid) = ds_Grid
            '====
            'dt_grid = formatDataset()

            If ds_Grid.Tables(0).Rows.Count <= 0 Then
                ObjCommon.ShowAlert("No Record Found", Me.Page)
                Return False
            End If

            Me.GV_Project.DataSource = ds_Grid
            Me.GV_Project.DataBind()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......FillGrid")
            Return False
        End Try
    End Function

#End Region

#Region "Selected Change"

    Protected Sub DDLProStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLProStatus.SelectedIndexChanged
        Reset()
    End Sub

    Protected Sub DDLProType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLProType.SelectedIndexChanged
        Reset()
    End Sub

    Private Sub Reset()
        Me.BtnGo1.Visible = True
        Me.DivSpDrug.Visible = False
        Me.GV_Project.DataSource = Nothing
        Me.GV_Project.DataBind()
    End Sub
#End Region

#Region "Grid Event"

    Protected Sub GV_Project_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim Index As Integer = e.CommandArgument
        If e.CommandName.ToUpper = "DETAIL" Then

            Me.Response.Redirect("frmProjectDetailMst.aspx?WorkSpaceId=" & GV_Project.Rows(Index).Cells(GVC_WorkspaceId).Text.Trim() & _
                                  "&Page=frmProjectSearch&Type=ALL")

        End If
    End Sub

    Protected Sub GV_Project_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then

            If e.Row.Cells(GVC_ProjectStatus).Text = "S" Then
                e.Row.Cells(GVC_ProjectStatus).Text = "Study"
            ElseIf e.Row.Cells(GVC_ProjectStatus).Text = "I" Then
                e.Row.Cells(GVC_ProjectStatus).Text = "Initial"
            ElseIf e.Row.Cells(GVC_ProjectStatus).Text = "C" Then
                e.Row.Cells(GVC_ProjectStatus).Text = "Completed"
            End If

            CType(e.Row.FindControl("LkbDetail"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("LkbDetail"), LinkButton).CommandName = "DETAIL"

            '===added on 13-jan-2010 by deepak to show projwct name in tooltip
            Dim str As String = Replace(CType(e.Row.FindControl("lblProjectName"), Label).Text, "*", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")
            e.Row.ToolTip = str.Replace("&nbsp;", "")
            '==============


        End If
    End Sub

    Protected Sub GV_Project_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Or _
                    e.Row.RowType = DataControlRowType.Header Or _
                    e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GVC_WorkspaceId).Visible = False
            e.Row.Cells(GVC_ProjecttypeCode).Visible = False
            '===added on 13-jan-2010 by deepak singh
            e.Row.Cells(GVC_Workspacedesc).Visible = False
            '===========
        End If

    End Sub

    Protected Sub GV_Project_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        GV_Project.PageIndex = e.NewPageIndex
        If Not FillGrid() Then
            Exit Sub
        End If
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
        rCell.NoofCellContain = 12
        rCell.FontColor = Drawing.Color.Maroon
        rCell.BackgroundColor = Drawing.Color.White
        rPage.Say(rRow)

        rRow = New RepoRow
        rCell = rRow.AddCell("ReportTitle")
        rCell.Value = "Project Detail"
        rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
        rCell.FontBold = True
        rCell.FontSize = 12
        rCell.FontColor = Drawing.Color.Maroon
        rCell.NoofCellContain = 12
        rPage.Say(rRow)


        rRow = New RepoRow
        rCell = rRow.AddCell("Location")
        'Change to drop down
        rCell.Value = "Location :: " & Me.DDLSegment.SelectedItem.Text.Trim() & ":" & Me.DDLLocation.SelectedItem.Text.Trim()
        rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
        rCell.FontBold = True
        rCell.FontSize = 12
        rCell.FontColor = Drawing.Color.Maroon
        rCell.NoofCellContain = 12
        rPage.Say(rRow)


        rPage.SayBlankRow()

    End Sub

    Private Function masterRow() As RepoRow
        Dim rRow As RepoRow
        Dim rCell As RepoCell
        Dim i As Integer

        rRow = New RepoRow


        rCell = New RepoCell("vProjectNo")
        rRow.AddCell(rCell)

        rCell = New RepoCell("vRequestId")
        rRow.AddCell(rCell)

        rCell = New RepoCell("vWorkspacedesc")
        rRow.AddCell(rCell)

        rCell = New RepoCell("vClientName")
        rRow.AddCell(rCell)



        rCell = New RepoCell("vDrugName")
        rRow.AddCell(rCell)

        rCell = New RepoCell("vBrandName")
        rRow.AddCell(rCell)

        rCell = New RepoCell("vProjectManager")
        rRow.AddCell(rCell)

        rCell = New RepoCell("vProjectCoordinator")
        rRow.AddCell(rCell)

        'rCell = New RepoCell("iNoOfSubjects")
        'rRow.AddCell(rCell)

        rCell = New RepoCell("nRetentionPeriod")
        rRow.AddCell(rCell)


        rCell = New RepoCell("cFastingfed")
        rRow.AddCell(rCell)


        rCell = New RepoCell("vRegionName")
        rRow.AddCell(rCell)


        rCell = New RepoCell("cProjectStatus")
        rRow.AddCell(rCell)
        'Check-In(1) Start,IP Administration(1) Start,,Check-In(2) Start,
        ',IP Administration(2) Start,,Check-In(3) Start,
        ',IP Administration(3) Start,,Check-In(4) Start,
        ',IP Administration(4) Start,,Check-In(5) Start,
        ',IP Administration(5) Start-

        rCell = New RepoCell("Check-In(1) Start")
        rRow.AddCell(rCell)


        rCell = New RepoCell("IP Administration(1) Start")
        rRow.AddCell(rCell)


        rCell = New RepoCell("Check-In(2) Start")
        rRow.AddCell(rCell)


        rCell = New RepoCell("IP Administration(2) Start")
        rRow.AddCell(rCell)


        rCell = New RepoCell("Check-In(3) Start")
        rRow.AddCell(rCell)


        rCell = New RepoCell("IP Administration(3) Start")
        rRow.AddCell(rCell)


        rCell = New RepoCell("Check-In(4) Start")
        rRow.AddCell(rCell)


        rCell = New RepoCell("IP Administration(4) Start")
        rRow.AddCell(rCell)

        rCell = New RepoCell("Check-In(5) Start")
        rRow.AddCell(rCell)


        rCell = New RepoCell("IP Administration(5) Start")
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

    Private Sub PrintHeader()
        Dim rRow As RepoRow
        Dim index As Integer

        rRow = New RepoRow

        rRow = masterRow()

        rRow.Cell("vProjectNo").Value = "Project No"
        rRow.Cell("vRequestId").Value = "Request ID"

        rRow.Cell("vWorkspacedesc").Value = "Project Name" 'Me.RblCommentsOn.SelectedItem.Text.Trim() + 
        rRow.Cell("vClientName").Value = "Client"
        rRow.Cell("vDrugName").Value = "Drug"
        rRow.Cell("vBrandName").Value = "Brand"
        rRow.Cell("vProjectManager").Value = "Project Manager"
        rRow.Cell("vProjectCoordinator").Value = "Project Coordinator"

        'rRow.Cell("iNoOfSubjects").Value = "No of Subject"
        rRow.Cell("nRetentionPeriod").Value = "RetentionPeriod"
        rRow.Cell("cFastingfed").Value = "Fastingfed"


        rRow.Cell("vRegionName").Value = "Submission"
        rRow.Cell("cProjectStatus").Value = "Status"

        'Check-In(1) Start,IP Administration(1) Start,,Check-In(2) Start,
        ',IP Administration(2) Start,,Check-In(3) Start,
        ',IP Administration(3) Start,,Check-In(4) Start,
        ',IP Administration(4) Start,,Check-In(5) Start,
        ',IP Administration(5) Start-

        rRow.Cell("Check-In(1) Start").Value = "Check-In(1) Start"
        rRow.Cell("IP Administration(1) Start").Value = "Dosing(1) Start"
        rRow.Cell("Check-In(2) Start").Value = "Check-In(2) Start"
        rRow.Cell("IP Administration(2) Start").Value = "Dosing(2) Start"
        rRow.Cell("Check-In(3) Start").Value = "Check-In(3) Start"

        rRow.Cell("IP Administration(3) Start").Value = "Dosing(3) Start"
        rRow.Cell("Check-In(4) Start").Value = "Check-In(4) Start"
        rRow.Cell("IP Administration(4) Start").Value = "Dosing(4) Start"
        rRow.Cell("Check-In(5) Start").Value = "Check-In(5) Start"
        rRow.Cell("IP Administration(5) Start").Value = "Dosing(5) Start"

        For index = 0 To rRow.CellCount - 1
            rRow.Cell(index).FontBold = True
            rRow.Cell(index).Alignment = RepoCell.AlignmentEnum.CenterTop
        Next

        rPage.Say(rRow)

    End Sub

    Private Sub ReportDetail(ByVal ds_grid As DataSet)
        Dim RowCnt As Integer
        Dim rRow As RepoRow
        'Dim dt_Report As New DataTable
        Dim PreviousCompany As String = String.Empty
        Dim PreviousProject As String = String.Empty
        Try

            rRow = masterRow()
            PrintHeader()



            RowCnt = 0

            Do While RowCnt <= ds_grid.Tables(0).Rows.Count - 1


                rRow.Cell("vProjectNo").Value = ds_grid.Tables(0).Rows(RowCnt)("vProjectNo").ToString()
                rRow.Cell("vRequestId").Value = ds_grid.Tables(0).Rows(RowCnt)("vRequestId").ToString()

                rRow.Cell("vWorkspacedesc").Value = ds_grid.Tables(0).Rows(RowCnt)("vWorkspacedesc").ToString()
                rRow.Cell("vClientName").Value = ds_grid.Tables(0).Rows(RowCnt)("vClientName").ToString()


                rRow.Cell("vDrugName").Value = ds_grid.Tables(0).Rows(RowCnt)("vDrugName").ToString()

                rRow.Cell("vBrandName").Value = ds_grid.Tables(0).Rows(RowCnt)("vBrandName").ToString()
                rRow.Cell("vProjectManager").Value = ds_grid.Tables(0).Rows(RowCnt)("vProjectManager").ToString()
                rRow.Cell("vProjectCoordinator").Value = ds_grid.Tables(0).Rows(RowCnt)("vProjectCoordinator").ToString()


                rRow.Cell("nRetentionPeriod").Value = ds_grid.Tables(0).Rows(RowCnt)("nRetaintionPeriod").ToString()
                rRow.Cell("cFastingfed").Value = ds_grid.Tables(0).Rows(RowCnt)("cFastingfed").ToString()




                rRow.Cell("vRegionName").Value = ds_grid.Tables(0).Rows(RowCnt)("vRegionName").ToString()
                rRow.Cell("cProjectStatus").Value = ds_grid.Tables(0).Rows(RowCnt)("cProjectStatus").ToString()

                'Check-In(1) Start,IP Administration(1) Start,,Check-In(2) Start,
                ',IP Administration(2) Start,,Check-In(3) Start,
                ',IP Administration(3) Start,,Check-In(4) Start,
                ',IP Administration(4) Start,,Check-In(5) Start,
                ',IP Administration(5) Start-

                rRow.Cell("Check-In(1) Start").Value = ds_grid.Tables(0).Rows(RowCnt)("Check-In(1) Start").ToString()
                rRow.Cell("IP Administration(1) Start").Value = ds_grid.Tables(0).Rows(RowCnt)("IP Administration(1) Start").ToString()
                rRow.Cell("Check-In(2) Start").Value = ds_grid.Tables(0).Rows(RowCnt)("Check-In(2) Start").ToString()
                rRow.Cell("IP Administration(2) Start").Value = ds_grid.Tables(0).Rows(RowCnt)("IP Administration(2) Start").ToString()
                rRow.Cell("Check-In(3) Start").Value = ds_grid.Tables(0).Rows(RowCnt)("Check-In(3) Start").ToString()
                rRow.Cell("IP Administration(3) Start").Value = ds_grid.Tables(0).Rows(RowCnt)("IP Administration(3) Start").ToString()
                rRow.Cell("Check-In(4) Start").Value = ds_grid.Tables(0).Rows(RowCnt)("Check-In(4) Start").ToString()
                rRow.Cell("IP Administration(4) Start").Value = ds_grid.Tables(0).Rows(RowCnt)("IP Administration(4) Start").ToString()
                rRow.Cell("Check-In(5) Start").Value = ds_grid.Tables(0).Rows(RowCnt)("Check-In(5) Start").ToString()
                rRow.Cell("IP Administration(5) Start").Value = ds_grid.Tables(0).Rows(RowCnt)("IP Administration(5) Start").ToString()

                rPage.Say(rRow)
                RowCnt = RowCnt + 1

            Loop ''detail loop ending

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message + "Error While Print Detail", "....ReportDetail")
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

    
End Class

