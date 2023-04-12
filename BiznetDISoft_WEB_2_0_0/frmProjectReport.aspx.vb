
Partial Class frmProjectReport
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private rPage As RepoPage

    Private Const VS_dtUserWiseDWR As String = "UserWiseDWR"
    Private Const VS_ProjectReportDetails As String = "ProjectReportDetails"

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
            'Me.ShowErrorMessage(ex.Message, "")
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
            'Me.ShowErrorMessage(ex.Message, "")

        Finally

        End Try
    End Function
#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Try
            Page.Title = "::Project Report :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Project Report"
            Choice = Me.ViewState("Choice")

            If Not FillDropDown() Then
                Return False
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "MultiselectRequired", "MultiselectRequired();", True)
            Dim firday = New DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
            Me.txtFromDate.Text = Format(firday, "dd-MMM-yyyy")
            Me.txtToDate.Text = Format(System.DateTime.Now, "dd-MMM-yyyy")
        Catch ex As Exception
            'Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function

#End Region

#Region "Grid View Event"
    Protected Sub gvProjectReportDetails_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Text = e.Row.RowIndex + (gvProjectReportDetails.PageSize * gvProjectReportDetails.PageIndex) + 1
        End If
    End Sub
#End Region

#Region "FillDropDown"
    Private Function FillDropDown() As Boolean
        Dim ds_Template As New Data.DataSet
        Dim ds_Project As New Data.DataSet
        Dim ds_PIUser As New Data.DataSet
        Dim ds_Client As New Data.DataSet
        Dim ds_User As New Data.DataSet
        Dim ds_Drug As New Data.DataSet
        Dim ds_region As New Data.DataSet
        Dim ds_Location As New Data.DataSet
        Dim ds_WorktypeMst As New Data.DataSet
        Dim dv_Template As New DataView
        Dim dt_Template As New DataTable
        Dim dt_PiUser As New DataTable
        Dim estr As String = String.Empty
        Dim Wstr_UserId As String = String.Empty
        Dim Wstr_Scope As String = String.Empty

        Dim dv_Project As New DataView
        Dim dv_Client As New DataView
        Dim dv_User As New DataView
        Dim dv_Drug As New DataView
        Dim dv_region As New DataView
        Dim dv_Location As New DataView
        Dim dv_WorktypeMSt As New DataView

        Dim Ds_UserCopy As New DataSet

        ''Added by ketan
        Dim ds_Service As New Data.DataSet
        Dim dv_Service As New DataView
        Dim ds_ProjectType As New Data.DataSet
        Dim ds_ProjectSubType As New DataSet
        Dim dsProjectMngr As New DataSet

        Try

            objHelp.getWorkTypeMst("cReplicaFlag<>'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    ds_WorktypeMst, estr)
            objHelp.getclientmst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_Client, estr)
            objHelp.getdrugmst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_Drug, estr)
            objHelp.getregionmst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_region, estr)
            objHelp.GetServiceDetail("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_Service, estr)  ''Added By Ketan

            ds_ProjectType = objHelp.ProcedureExecute("dbo.Proc_getProjectType", "")

            ds_ProjectSubType = objHelp.ProcedureExecute("dbo.Proc_getProjectSubType", "")

            objHelp.GetViewUserMst("nScopeNo=" & Me.Session(S_ScopeNo) & "", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_User, estr)
            dsProjectMngr = Me.objHelp.GetResultSet("Select iUserId,ProjectManagerWithProfile From View_ProjectManagerForProjectReport order by ProjectManager", "View_ProjectManagerForProjectReport")

            If Not ds_User.Tables(0).Rows.Count <= 0 Then

                Ds_UserCopy = ds_User

                For CntOfDs_User As Integer = 0 To Ds_UserCopy.Tables(0).Rows.Count - 1
                    Ds_UserCopy.Tables(0).Rows(CntOfDs_User).Item("vUserName") = Ds_UserCopy.Tables(0).Rows(CntOfDs_User).Item("vUserName").ToString() + "   " + "(" + Ds_UserCopy.Tables(0).Rows(CntOfDs_User).Item("vUserTypeName").ToString() + ")"
                Next CntOfDs_User

                Ds_UserCopy.AcceptChanges()


                dv_User = Ds_UserCopy.Tables(0).DefaultView

                dv_User.Sort = "vUserName"

                Me.ddlProjectManager.DataSource = dsProjectMngr
                Me.ddlProjectManager.DataValueField = "iUserId"
                Me.ddlProjectManager.DataTextField = "ProjectManagerWithProfile"
                Me.ddlProjectManager.DataBind()
                'Me.LbProjectManager.Items.Insert(0, New ListItem("Select Manager", "0"))

                'added for tooltip
                For iSlcManager As Integer = 0 To ddlProjectManager.Items.Count - 1
                    ddlProjectManager.Items(iSlcManager).Attributes.Add("title", ddlProjectManager.Items(iSlcManager).Text)
                Next
                '=========
            End If
            If Not ds_Drug.Tables(0).Rows.Count <= 0 Then
                dv_Drug = ds_Drug.Tables(0).DefaultView
                dv_Drug.Sort = "vDrugName"
                Me.ddlDrug.DataSource = dv_Drug
                Me.ddlDrug.DataValueField = "vDrugCode"
                Me.ddlDrug.DataTextField = "vDrugName"
                Me.ddlDrug.DataBind()
                'Me.LbDrug.Items.Insert(0, New ListItem("Select Drug", ""))

                'added for tooltip
                For iSlcDrug As Integer = 0 To ddlDrug.Items.Count - 1
                    ddlDrug.Items(iSlcDrug).Attributes.Add("title", ddlDrug.Items(iSlcDrug).Text)
                Next
                '=========

            End If

            If Not ds_Client.Tables(0).Rows.Count <= 0 Then
                dv_Client = ds_Client.Tables(0).DefaultView
                dv_Client.Sort = "vClientName"
                Me.ddlSponsor.DataSource = dv_Client
                Me.ddlSponsor.DataValueField = "vClientCode"
                Me.ddlSponsor.DataTextField = "vClientName"
                Me.ddlSponsor.DataBind()
                'Me.LbSponsor.Items.Insert(0, New ListItem("Select Sponsor", ""))

                'added on 26-feb-10 for tooltip
                For iSlcSponsor As Integer = 0 To ddlSponsor.Items.Count - 1
                    ddlSponsor.Items(iSlcSponsor).Attributes.Add("title", ddlSponsor.Items(iSlcSponsor).Text)
                Next
            End If

            If Not ds_region.Tables(0).Rows.Count <= 0 Then
                dv_region = ds_region.Tables(0).DefaultView
                dv_region.Sort = "vRegionName"
                Me.ddlSubmission.DataSource = dv_region
                Me.ddlSubmission.DataValueField = "vRegionCode"
                Me.ddlSubmission.DataTextField = "vRegionName"
                Me.ddlSubmission.DataBind()
                'Me.SlcSubmission.Value = "0000"
                'Me.LbSubmission.Items.Insert(0, New ListItem("Select Submission", ""))

                'added on 26-feb-10 for tooltip
                For iSlcSubmission As Integer = 0 To ddlSubmission.Items.Count - 1
                    ddlSubmission.Items(iSlcSubmission).Attributes.Add("title", ddlSubmission.Items(iSlcSubmission).Text)
                Next
            End If



            If Not ds_ProjectType.Tables(0).Rows.Count <= 0 Then
                Me.ddlProjectType.DataSource = ds_ProjectType
                Me.ddlProjectType.DataValueField = "vProjectTypeCode"
                Me.ddlProjectType.DataTextField = "vProjectTypeName"
                Me.ddlProjectType.DataBind()
            End If


            If Not ds_ProjectSubType.Tables(0).Rows.Count <= 0 Then
                Me.ddlProjectTypeDetail.DataSource = ds_ProjectSubType
                Me.ddlProjectTypeDetail.DataValueField = "vProjectSubTypeCode"
                Me.ddlProjectTypeDetail.DataTextField = "vProjectSubTypeName"
                Me.ddlProjectTypeDetail.DataBind()
            End If

            '========
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillDropDown")
            Return False
        End Try
    End Function

#End Region

#Region "Button Event"
    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncancel.Click
        'ddlProjectManager.SelectedIndex = 0
        'ddlDrug.SelectedIndex = 0
        'ddlSponsor.SelectedIndex = 0
        'ddlSubmission.SelectedIndex = 0
        'hdnProjectManager.Value = ""
        'hdnDrug.Value = ""
        'hdnSponsor.Value = ""
        'hdnSubmission.Value = ""
        'txtFromDate.Text = ""
        'txtToDate.Text = ""
        'ViewState(VS_ProjectReportDetails) = Nothing
        'Me.ClientScript.RegisterHiddenField("VS_ProjectReportDetails", "0")
    End Sub

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub
#End Region

#Region "Generate Report "
    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGo.Click
        Dim strProjectmanager As String = Me.hdnProjectManager.Value
        Dim strDrug As String = Me.hdnDrug.Value
        Dim strSponsor As String = Me.hdnSponsor.Value
        Dim strSubmission As String = Me.hdnSubmission.Value
        Dim toDate As String = String.Empty
        Dim fromDate As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim estr_retu As String = String.Empty
        Dim ds_ProjectDetails As DataSet = New DataSet
        Try
            If txtToDate.Text.Trim() <> "" Then
                toDate = CType(txtToDate.Text.Trim(), DateTime).ToString("dd-MMM-yyyy")
            End If
            If txtFromDate.Text.Trim() <> "" Then
                fromDate = CType(txtFromDate.Text.Trim(), DateTime).ToString("dd-MMM-yyyy")
            End If

            'Wstr = IIf(strProjectmanager = "", "", strProjectmanager) + "##" + IIf(strDrug.Trim() = "", "' '", strDrug.Trim()) + "##" + IIf(strSponsor.Trim() = "", "", strSponsor.Trim()) + "##" + IIf(strSubmission.Trim() = "", "' '", strSubmission.Trim()) + "##" + IIf(hdnProjectType.Value.Trim() = "", "' '", hdnProjectType.Value.Trim()) + "##" + IIf(hdnProjectSubtype.Value.Trim() = "", "' '", hdnProjectSubtype.Value.Trim()) + "##" + IIf(toDate.Trim() = "", "' '", toDate.Trim()) + "##" + IIf(fromDate.Trim() = "", "' '", fromDate.Trim())

            Wstr = strProjectmanager + "##" + strDrug.Trim() + "##" + strSponsor.Trim() + "##" + strSubmission.Trim() + "##" + hdnProjectType.Value.Trim() + "##" + hdnProjectSubtype.Value.Trim() + "##" + toDate.Trim() + "##" + fromDate.Trim()

            If Not Me.objHelp.Proc_GetProjectReport(Wstr, ds_ProjectDetails, estr_retu) Then
                Throw New Exception(estr_retu)
            End If

            ViewState(VS_ProjectReportDetails) = ds_ProjectDetails.Tables(0)
            If ds_ProjectDetails.Tables(0).Rows.Count > 0 Then
                Me.ClientScript.RegisterHiddenField("VS_ProjectReportDetails", "1")
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ALter", "msgalert('Data not found.');", True)   ''Added by ketan
            End If

            gvProjectReportDetails.DataSource = ds_ProjectDetails.Tables(0)
            gvProjectReportDetails.DataBind()
            If gvProjectReportDetails.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "BingGvProjectReportDetails", "BingGvProjectReportDetails(); ", True)   ''Added by ketan
            End If

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "MultiselectRequired", "MultiselectRequired(); ", True)   ''Added by ketan
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideSponsorDetails", "HideSponsorDetails(); ", True)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillGridView")
        End Try
    End Sub
    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Dim str As String = String.Empty
        Dim FileName As String = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
        Dim isReportComplete As Boolean = False
        FileName = (FileName.Replace("/", "").Replace(":", "").Replace(" ", "")).Trim()
        Dim value As String = String.Empty
        Try
            If ViewState(VS_ProjectReportDetails) Is Nothing Then
                ObjCommon.ShowAlert("Data not found.", Me)
                Exit Sub
            End If

            'FileName = GetReportName() + ".xls"
            FileName = FileName + ".xls"
            FileName = Server.MapPath("~/ExcelReportFile/" + FileName)
            Try
                OpenReport(FileName)
                ReportHeader()
                ReportDetail()
                isReportComplete = True
            Catch ex As Exception
                Me.ShowErrorMessage(ex.Message, "....Export TO Excel")
            End Try

        Catch ex As Exception
            isReportComplete = False
            Me.ShowErrorMessage(ex.Message, "....btnGenerate_Click")
        Finally
            If Not rPage Is Nothing Then
                rPage.CloseReport()
                rPage = Nothing
            End If
        End Try

        If isReportComplete = True Then
            'ReportGeneralFunction.ExportFileAtWeb(ReportGeneralFunction.ExportFileAsEnum.ExportAsXLS, HttpContext.Current.Response, FileName, Me.Server.MapPath(""))
            'ReportGeneralFunction.ExportFileAtWeb(ReportGeneralFunction.ExportFileAsEnum.ExportAsXLS, HttpContext.Current.Response, FileName)
            Dim curContext As System.Web.HttpContext = System.Web.HttpContext.Current

            curContext.Response.Clear()
            curContext.Response.Buffer = True

            curContext.Response.AddHeader("content-disposition", "attachment; filename=" + System.Web.HttpUtility.UrlEncode(Path.GetFileName(FileName), System.Text.Encoding.UTF8))
            curContext.Response.ContentType = "application/octet-stream"
            curContext.Response.WriteFile(FileName)
            curContext.Response.Flush()
            curContext.Response.End()
            HttpContext.Current.ApplicationInstance.CompleteRequest()
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

        ' rRow = New RepoRow
        ' rCell = rRow.AddCell("CompanyTitle")
        ' rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
        ' rCell.FontBold = True
        ' rCell.FontSize = 14
        ' rCell.Value = System.Configuration.ConfigurationManager.AppSettings("Client")
        ' rCell.NoofCellContain = 10
        ' rCell.FontColor = Drawing.Color.Maroon
        ' rCell.BackgroundColor = Drawing.Color.White
        ' rPage.Say(rRow)


        rRow = New RepoRow
        rCell = rRow.AddCell("ClientName")
        rCell.Value = System.Configuration.ConfigurationManager.AppSettings("Client").ToString()
        rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
        rCell.FontBold = True
        rCell.FontSize = 12
        rCell.FontColor = Drawing.Color.Maroon
        rCell.NoofCellContain = 18
        rPage.Say(rRow)



        rRow = New RepoRow
        rCell = rRow.AddCell("ReportTitle")
        rCell.Value = "Project Report"
        rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
        rCell.FontBold = True
        rCell.FontSize = 12
        rCell.FontColor = Drawing.Color.Maroon
        rCell.NoofCellContain = 18
        rPage.Say(rRow)

        '   rRow = New RepoRow
        '   rCell = rRow.AddCell("ClientName")
        '   'rCell.Value = "Comments Done on : " & Me.RblCommentsOn.SelectedItem.Text.Trim()
        '   rCell.Value = "Comments Done on : " & ""
        '   rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
        '   rCell.FontBold = True
        '   rCell.FontSize = 12
        '   rCell.FontColor = Drawing.Color.Maroon
        '   rCell.NoofCellContain = 10
        '   rPage.Say(rRow)

        rRow = New RepoRow
        rCell = rRow.AddCell("DatePeriod")
        rCell.Value = "Period: " & Me.txtFromDate.Text.Trim() & " To " & Me.txtToDate.Text.Trim()
        rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
        rCell.FontBold = True
        rCell.FontSize = 12
        rCell.FontColor = Drawing.Color.Maroon
        rCell.NoofCellContain = 18
        rPage.Say(rRow)

        rRow = New RepoRow
        rCell = New RepoCell("Printed Date:-")
        rCell.Value = "Printed Date: " & DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm")
        rCell.Alignment = RepoCell.AlignmentEnum.LeftTop
        rRow.AddCell(rCell)
        rCell.FontBold = True
        rCell.FontSize = 12
        rCell.FontColor = Drawing.Color.Maroon
        rCell.NoofCellContain = 3
        rCell = New RepoCell("Printed By:-")
        rCell.Value = "Printed By: " & Session(S_LoginName)
        rCell.Alignment = RepoCell.AlignmentEnum.RightTop
        rRow.AddCell(rCell)
        rCell.FontBold = True
        rCell.FontSize = 12
        rCell.FontColor = Drawing.Color.Maroon
        rCell.NoofCellContain = 15
        rPage.Say(rRow)

        ' rPage.SayBlankRow()

    End Sub

    Private Sub PrintHeader()
        Dim rRow As RepoRow
        Dim index As Integer

        rRow = New RepoRow

        rRow = masterRow()
        rRow.Cell("ProjectNo").Value = "Project No" 'Me.RblCommentsOn.SelectedItem.Text.Trim() + 
        rRow.Cell("Sponsorname").Value = "Sponsor Name"
        rRow.Cell("DrugName").Value = "Drug Name"
        rRow.Cell("FastFed").Value = "FastFed"
        rRow.Cell("ProjectTypeName").Value = "ProjectTypeName"
        rRow.Cell("ProjectSubTypeName").Value = "ProjectSubTypeName"
        rRow.Cell("StudyType").Value = "Study Type"
        rRow.Cell("Submission").Value = "Submission"
        rRow.Cell("ServiceName").Value = "ServiceName"
        rRow.Cell("Location").Value = "Location"
        rRow.Cell("iNoOfSubjects").Value = "No Of Subjects"
        rRow.Cell("ProjectManager").Value = "Project Manager"
        rRow.Cell("ClinicalStartDate").Value = "Clinical StartDate"
        rRow.Cell("ClinicalEndDate").Value = "Clinical EndDate"
        rRow.Cell("AnalysisStartDate").Value = "Analysis StartDate"
        rRow.Cell("ReportDisPatchDate").Value = "Report Dispatch Date"
        rRow.Cell("cProjectStatus").Value = "Project Status"
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
        Dim PreviousCompany As String = String.Empty
        Dim PreviousProject As String = String.Empty
        Try

            rRow = masterRow()
            PrintHeader()
            RowCnt = 0

            dt_Report = CType(ViewState(VS_ProjectReportDetails), DataTable)
            If dt_Report Is Nothing Then
                Exit Sub
            End If

            Do While RowCnt <= dt_Report.Rows.Count - 1

                rRow.Cell("ProjectNo").Value = dt_Report.Rows(RowCnt)("ProjectNo").ToString()
                rRow.Cell("Sponsorname").Value = dt_Report.Rows(RowCnt)("Sponsorname").ToString()
                rRow.Cell("DrugName").Value = dt_Report.Rows(RowCnt)("Drug").ToString()
                rRow.Cell("FastFed").Value = dt_Report.Rows(RowCnt)("cFastingFed").ToString()
                rRow.Cell("ProjectTypeName").Value = dt_Report.Rows(RowCnt)("vProjectTypeName").ToString()
                rRow.Cell("ProjectSubTypeName").Value = dt_Report.Rows(RowCnt)("vProjectSubTypeName").ToString()
                rRow.Cell("StudyType").Value = dt_Report.Rows(RowCnt)("StudyType").ToString()
                rRow.Cell("Submission").Value = dt_Report.Rows(RowCnt)("Submission").ToString()
                rRow.Cell("ServiceName").Value = dt_Report.Rows(RowCnt)("ServiceName").ToString()
                rRow.Cell("Location").Value = dt_Report.Rows(RowCnt)("Location").ToString()
                rRow.Cell("iNoOfSubjects").Value = dt_Report.Rows(RowCnt)("iNoOfSubjects").ToString()
                rRow.Cell("ProjectManager").Value = dt_Report.Rows(RowCnt)("ProjectManager").ToString()
                If Not dt_Report.Rows(RowCnt)("ClinicalStartDate") Is DBNull.Value And Convert.ToString(dt_Report.Rows(RowCnt)("ClinicalStartDate")) <> "" Then
                    rRow.Cell("ClinicalStartDate").Value = CType(dt_Report.Rows(RowCnt)("ClinicalStartDate"), DateTime).ToString("dd-MMM-yyyy hh:mm")
                End If

                If Not dt_Report.Rows(RowCnt)("ClinicalEndDate") Is DBNull.Value And Convert.ToString(dt_Report.Rows(RowCnt)("ClinicalEndDate")) <> "" Then
                    rRow.Cell("ClinicalEndDate").Value = CType(dt_Report.Rows(RowCnt)("ClinicalEndDate"), DateTime).ToString("dd-MMM-yyyy hh:mm")
                End If
                If Not dt_Report.Rows(RowCnt)("AnalysisStartDate") Is DBNull.Value And Convert.ToString(dt_Report.Rows(RowCnt)("AnalysisStartDate")) <> "" Then
                    rRow.Cell("AnalysisStartDate").Value = CType(dt_Report.Rows(RowCnt)("AnalysisStartDate"), DateTime).ToString("dd-MMM-yyyy hh:mm")
                End If

                If Not dt_Report.Rows(RowCnt)("AnalysisEndDate") Is DBNull.Value And Convert.ToString(dt_Report.Rows(RowCnt)("AnalysisEndDate")) <> "" Then
                    rRow.Cell("AnalysisEndDate").Value = CType(dt_Report.Rows(RowCnt)("AnalysisEndDate"), DateTime).ToString("dd-MMM-yyyy hh:mm")
                End If
                rRow.Cell("ReportDisPatchDate").Value = dt_Report.Rows(RowCnt)("ReportDisPatchDate").ToString()
                rRow.Cell("cProjectStatus").Value = dt_Report.Rows(RowCnt)("cProjectStatus").ToString()

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

        rCell = New RepoCell("ProjectNo")
        rRow.AddCell(rCell)

        rCell = New RepoCell("Sponsorname")
        rRow.AddCell(rCell)


        rCell = New RepoCell("DrugName")
        rRow.AddCell(rCell)

        rCell = New RepoCell("FastFed")
        rRow.AddCell(rCell)

        rCell = New RepoCell("ProjectTypeName")
        rRow.AddCell(rCell)

        rCell = New RepoCell("ProjectSubTypeName")
        rRow.AddCell(rCell)

        rCell = New RepoCell("StudyType")
        rRow.AddCell(rCell)

        rCell = New RepoCell("Submission")
        rRow.AddCell(rCell)


        rCell = New RepoCell("ServiceName")
        rRow.AddCell(rCell)


        rCell = New RepoCell("Location")
        rRow.AddCell(rCell)

        rCell = New RepoCell("iNoOfSubjects")
        rRow.AddCell(rCell)

        rCell = New RepoCell("ProjectManager")
        rRow.AddCell(rCell)

        rCell = New RepoCell("ClinicalStartDate")
        rRow.AddCell(rCell)

        rCell = New RepoCell("ClinicalEndDate")
        rRow.AddCell(rCell)

        rCell = New RepoCell("AnalysisStartDate")
        rRow.AddCell(rCell)


        rCell = New RepoCell("AnalysisEndDate")
        rRow.AddCell(rCell)

        rCell = New RepoCell("ReportDisPatchDate")
        rRow.AddCell(rCell)

        rCell = New RepoCell("cProjectStatus")
        rRow.AddCell(rCell)




        For i = 0 To rRow.CellCount - 1
            rRow.Cell(i).FontSize = 11
            rRow.Cell(i).LineBottom.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(i).LineTop.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(i).LineRight.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(i).LineLeft.LineType = RepoLine.LineTypeEnum.SingleLine
        Next i

        Return rRow

    End Function

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
