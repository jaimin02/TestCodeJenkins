Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Configuration
Imports System.Collections
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.DataVisualization.Charting
Imports System.Web.UI.MobileControls
Imports System.Web.Services
Imports System.Drawing.Image
Imports Newtonsoft.Json
Imports System.Net
Imports System.Threading.Tasks
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf

Partial Class frmMainPage
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION"
    Private objCommon As New clsCommon
    Private objHelpDbTable As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
    Private Const VS_Choice As String = "Choice"
    Private rPage As RepoPage

    Private VS_grvSubjectInfo As String = "SubjectInfo"
    Private VS_GvwPnl00001 As String = "vs_Gvwpnl0001"
    Private VS_GvwPnl00003 As String = "vs_Gvwpnl0003"
    Private VS_GvwPnl00004 As String = "vs_GvwPnl00004"
    Private VS_gvwpnl_Analysis As String = "vs_gvwpnl_Analysis"
    Private VS_ProjectType As String = "ProjectType"
    Private Vs_CommonForPageIndexChangingForClientRequest As String = "Vs_CommonForPageIndexChanging"
    Private VS_GvwPnlProjectStudyWorkSummary_PageIndexChanging As String = "VS_GvwpnlProjectStudyWorkSummary"
    Private Vs_CommonForPageIndexChangingForClinicalPhase As String = "Vs_CommonForPageIndexChangingForClinicalPhase"
    Private Vs_CommonForPageIndexChangingForAnalyticalPhase As String = "Vs_CommonForPageIndexChangingForAnalyticalPhase"
    Private Vs_CommonForPageIndexChangingForDocumentPhase As String = "Vs_CommonForPageIndexChangingForDocumentPhase"

    Private eStr_Retu As String = ""
    Private dsDisplayDetails As DataSet = Nothing

    Private Const gvwpnl0003_Dateils As Integer = 0
    Dim gvwpnl0003_workspaceid As Integer = 1
    Private Const gvwpnl0003_NoOfTimePoints As Integer = 6
    'Private Const gvwpnl0003_NoOfTimePoints As Integer = 5
    Private BtnGoClickCheck As Boolean = False
    Private BtnGoAdjuClickCheck As Boolean = False
    Private BtnGoQueryClickCheck As Boolean = False
    Private BtnGoCAClickCheck As Boolean = False 'Added by Bhargav Thaker
    Dim ds As DataSet = Nothing
    Dim gvwpnl0001_Details As Integer = 0
    Dim gvwpnl0001_workspaceid As Integer = 1
    Dim gvwpnl0013_Details As Integer = 0
    Dim gvwpnl0013_workspaceid As Integer = 1
    Dim gvwpnl0004_Details As Integer = 0
    Dim gvwpnl0004_workspaceid As Integer = 1
    Dim gvwpnl_Analysis_Details As Integer = 0
    Dim gvwpnl_Analysis_workspaceid As Integer = 1
    Dim gvwpnl_Analysis_status As Integer = 7
    'Dim gvwpnl_Analysis_status As Integer = 6
    Dim gvw_workspaceid As Integer = 1
    'Dim gvwScreeningAanlytics_ScreeningOpen As Integer = 6
    'Dim gvwScreeningAanlytics_ScreeningLapsed As Integer = 4

    Dim gvwpnlOperationalKpi_Details As Integer = 0
    'Dim gvw_OperationalKpiCheckInDate As Integer = 4
    'Dim gvw_OperationalKpiCheckOutDate As Integer = 5
    Dim gvw_OperationalKpiiPeriod As Integer = 4
    Dim gvw_OperationalKpiInoOfSubjects As Integer = 5
    'Dim gvwpnlOperationalKpi_NoOfDosedSubjects As Integer = 8
    Dim gvwpnlOperationalKpi_workspaceid As Integer = 8
    Dim gvwpnlOperationalKpi_NoOfSamples As Integer = 9
    Dim gvwpnlOperationalKpi_NoOfIxSamples As Integer = 10
    Dim gvwpnlOperationalKpi_TotalDosed As Integer = 11
    Dim gvwpnlOperationalKpi_BedNights As Integer = 12
    Dim gvwpnWorkSummary_Details As Integer = 0
    Dim gvwpnWorkSummary_workspaceid As Integer = 1

    Dim DelayedProjectListOfES As String = String.Empty
    Dim DelayedProjectListOfSA As String = String.Empty
    Dim OnTimeProjectListOfES As String = String.Empty
    Dim OnTimeProjectListOfSA As String = String.Empty

    '' For My Calendar
    Public RedColor As Integer = 100
    Public BlueColor As Integer = 50
    Public GreenColor As Integer = 10
    Public Brown As Integer = 20

    Dim Gvc_SrnO As Integer = 0
    Dim GVC_SchStart As Integer = 2
    Dim GVC_ActStart As Integer = 3
    Private Const VS_QSLocation As String = "QueryStringLocation"
    Private Const VS_Mode As String = "Mode"
    Private Const SubjectsTotalDosed As String = "SubjectsTotalDosed"
    Private Const ProjectsReleased As String = "ProjectsReleased"

    Private isDataEntered As Boolean
    Private ActualDateVisible As Boolean = True
    Private ActualDateVisible1 As Boolean = True
    Private Const GVC_ProjectNo As Integer = 0
    Private Const GVC_Randomization As Integer = 1
    Private Const GVC_SubjectNo As Integer = 2
    Private Const GVC_vMySubjectNo As Integer = 3
    Private Const GVC_iMySubjectNo As Integer = 4
    Private Const GVC_cScreenFailure As Integer = 5
    Private Const GVC_cDisContinue As Integer = 6
    Private Const GVC_FirStVisit As Integer = 7

    Private ActId As New ArrayList
    Private NodeId As New ArrayList

    Private VS_Scheduler As String = "VisitScheduler"
    Private VS_SchedulerTracker As String = "VisitSchedulerTracker"
    Private VS_Actual As String = "VisitActual"
    Private Const VS_SiteWiseSubjectInformation As String = "Panel1"
    Private Const VS_SiteWiseCA As String = "Panel2"
    Private Const VS_SiteWiseGR As String = "Panel3"
    Private Const VS_SiteWiseAR As String = "Panel4"
    Private Const VS_Activity As String = "Activity"
    Dim UserId As String

    Private Const AdjucatorConstant As String = "Dashboard - Adjucator"
    Private Const VisitConstant As String = "Dashboard - Visit"
    Private Const CAConstant As String = "frmActivitySelectionforBunch.aspx" 'Added By Bhargav Thaker
#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim LblMandaTory As System.Web.UI.WebControls.Label
        Dim clientname As String = String.Empty
        Dim ds_location As DataSet = Nothing
        Dim ds_ProjectDetail As DataSet = Nothing
        Dim wstr As String = String.Empty
        clsCommon.ClearCache(Me) 'for removing page from cache
        Dim ds_ProjectMngr As New DataSet()
        Dim dsMenu As New DataSet
        Dim currenturl As String = Request.Url.ToString()
        Try
            Page.Title = " :: Home Page :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            If Not Page.IsPostBack Then
                wstr = "cLocationType='L' and cStatusIndi <>'D' "
                If Not objHelpDbTable.getLocationMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_location, eStr_Retu) Then
                    Throw New Exception(eStr_Retu)
                End If
                ds_ProjectMngr = Me.objHelpDbTable.GetResultSet("Select iUserId,ProjectManagerWithProfile From View_ProjectManager order by ProjectManager", "View_ProjectManager")

                If ds_location.Tables(0).Rows.Count > 0 Then
                    Me.ddlLocation.DataSource = ds_location
                    Me.ddlLocation.DataTextField = "vLocationName"
                    Me.ddlLocation.DataValueField = "vLocationCode"
                    Me.ddlLocation.DataBind()
                End If
                If ds_ProjectMngr.Tables(0).Rows.Count > 0 Then
                    Me.ddlProjectManager.DataSource = ds_ProjectMngr
                    Me.ddlProjectManager.DataTextField = "ProjectManagerWithProfile"
                    Me.ddlProjectManager.DataValueField = "iUserId"
                    Me.ddlProjectManager.DataBind()
                    Me.ddlProjectManager.Items.Insert(0, "All")
                End If

                If Not displayNotice() Then
                    Exit Sub
                End If

                If Not GetDiplayDetails() Then
                    Exit Sub
                End If

                Me.AutoCompleteExtenderForClientRequest.ContextKey = "cProjectStatus = 'I' And vProjectTypeCode <> '0016'"
                Me.AutoCompleteExtenderForClinicalPhase.ContextKey = "(cProjectStatus = 'S' or cProjectStatus = 'O') And vProjectTypeCode <> '0016'"
                Me.AutoCompleteExtenderForAnalyticalPhase.ContextKey = "(cProjectStatus = 'L' or cProjectStatus = 'Y') And vProjectTypeCode <> '0016'"
                Me.AutoCompleteExtenderForDocumentPhase.ContextKey = "cProjectStatus = 'D' And vProjectTypeCode <> '0016'"
                Me.AutoCompleteExtenderForProjectPreClinical.ContextKey = "cProjectStatus = 'P' And vProjectTypeCode <> '0016'"
                Me.AutoCompleteExtenderadd.ContextKey = "iUserId = " & Me.Session(S_UserID) + " AND vProjectTypeCode = '0014'"
                Me.AutoCompleteExtenderForVisitScheduler.ContextKey = "iUserId = " & Me.Session(S_UserID)
                Me.AutoCompleteExtenderForVisitScheduler.ServiceMethod = "GetMyProjectCompletionList"

                ''Added By Vivek Patel
                Me.AutoCompleteExtenderforsubjectInfo.ContextKey = "cIsTestSite <> 'Y' AND iUserId = " & Me.Session(S_UserID)
                Me.AutoCompleteExtenderforsiteInfo.ContextKey = "cIsTestSite <> 'Y' AND iUserId = " & Me.Session(S_UserID)
                Me.AutoCompleteExtenderforcrf1.ContextKey = "cIsTestSite <> 'Y' AND iUserId = " & Me.Session(S_UserID)
                Me.AutoCompleteExtenderforaese.ContextKey = "cIsTestSite <> 'Y' AND iUserId = " & Me.Session(S_UserID)
                Me.AutoCompleteExtenderForCRFDataStatus.ContextKey = "cIsTestSite <> 'Y' AND iUserId = " & Me.Session(S_UserID)
                Me.AutoCompleteExtenderforsiteData.ContextKey = "cIsTestSite <> 'Y' AND iUserId = " & Me.Session(S_UserID)
                Me.AutoCompleteExtenderforDCFManage.ContextKey = "cIsTestSite <> 'Y' AND iUserId = " & Me.Session(S_UserID)
                'Me.AutoCompleteExtender1.ContextKey = "cIsTestSite <> 'Y' AND iUserId = " & Me.Session(S_UserID) 'Comment By Bhargav Thaker
                Me.AutoCompleteExtender1.ContextKey = "cWorkspaceType ='C' AND iUserId = " & Me.Session(S_UserID) 'Added By Bhargav Thaker
                'Me.AutoCompleteExtender3.ContextKey = "cIsTestSite <> 'Y' AND iUserId = " & Me.Session(S_UserID) 'Added By Bhargav Thaker
                Me.AutoCompleteExtender2.ContextKey = "cWorkspaceType ='C' AND iUserId = " & Me.Session(S_UserID) 'Added By Bhargav Thaker 23March2023

                If (ddlProjectManager.SelectedIndex > 0) Then
                    Me.AutoCompleteExtenderForTracking.ContextKey = "iProjectManagerId = " + ddlProjectManager.SelectedValue
                End If

                GetCounts()

                'Removing master page control
                CType(Me.Master.FindControl("lblMandatory"), System.Web.UI.WebControls.Label).Style.Add("display", "none")
                CType(Me.Master.FindControl("lblMode"), System.Web.UI.WebControls.Label).Style.Add("display", "none")
                CType(Me.Master.FindControl("lblShowMode"), System.Web.UI.WebControls.Label).Style.Add("display", "none")
                CType(Me.Master.FindControl("lblHeading"), System.Web.UI.WebControls.Label).Style.Add("display", "none")
                CType(Me.Master.FindControl("lblerrormsg"), System.Web.UI.WebControls.Label).Style.Add("display", "none")
                '**************************

                Me.TrProjectTrack.Attributes.Add("onclick", "togglePanel($get('imgTrackProject'), 'divProjectTrack','medium');")
                Me.TrClientRequest.Attributes.Add("onclick", "ShowHideDiv('divClientRequest','imgClientRequest','ClientRequestProjects');")
                Me.TrPreClinical.Attributes.Add("onclick", "ShowHideDiv('divProjectPreClinical','imgClientRequest','PreClinicalProjects');")
                Me.TrClinicalPhase.Attributes.Add("onclick", "ShowHideDiv('divClinicalPhase','imgClinicalPhase','ClinicalPhaseProjects');")
                Me.TrAnalytical.Attributes.Add("onclick", "ShowHideDiv('divAnalyticalphase','imgAnalyticalphase','AnalyticalProjects');")
                Me.TrDocument.Attributes.Add("onclick", "ShowHideDiv('divDocumentPhase','imgDocumentPhase','DocumentProjects');")
                Me.TrScreeningAnalytic.Attributes.Add("onclick", "ShowHideDiv('divScreeningAnalytic','imgScreeningAnalytic','ScreeningAnalytic');")
                Me.TrProjectStudyWorkSummary.Attributes.Add("onclick", "ShowHideDiv('divProjectStudyWorkSummary','imgProjectStudyWorkSummary','ProjectStudyWorkSummary');")
                Me.TrOperationalKpi.Attributes.Add("onclick", "ShowHideDiv('divOperationalKpi','imgOperationalKpi','OperationalKpi');")
                'Me.TrMyCalendar.Attributes.Add("onclick", "ShowHideDiv('divMyCalendar','imgMyCalendar','MyCalendar');")
                Me.TrVisitTracker.Attributes.Add("onclick", "ShowHideDiv('divVisitTracker','imgMyCalendar','VisitTracker');")
                Me.TrVisitScheduler.Attributes.Add("onclick", "ShowHideDiv('divVisitScheduler','imgMyCalendar','VisitScheduler');")
                Me.TrSiteWiseSubjectDetail.Attributes.Add("onclick", "ShowHideDiv('divSiteWiseSubjectDetail','imgSiteWiseSubjectDetail','SiteWiseSubjectDetail');")
                Me.TrSiteInformation.Attributes.Add("onclick", "ShowHideDiv('divSiteInformation','imgSiteInformation','SiteInformation');")
                Me.TrCRFStatus.Attributes.Add("onclick", "ShowHideDiv('divCRFStatus','imgSiteimgCRFStatusWiseSubjectDetail','CRFStatus');")
                Me.TrAESAE.Attributes.Add("onclick", "ShowHideDiv('divAESAE','imgAESAE','AESAE');")
                Me.TrDemo.Attributes.Add("onclick", "ShowHideDiv('divDemo','imgDemo','NewDemo');")
                Me.TrDemo1.Attributes.Add("onclick", "ShowHideDiv('divDemo1','imgDemo1','NewDemo1');")
                Me.TrDCFManage.Attributes.Add("onclick", "ShowHideDiv('divDcfmanage','imgdcfmg','DcfManagement');")
                ''Completed By Vivek Patel

                LblMandaTory = CType(Me.Master.FindControl("lblMandatory"), System.Web.UI.WebControls.Label)
                LblMandaTory.Visible = False

                wstr = "iUserId='" & Me.Session(S_UserID) & "' and cStatusIndi <> 'D'"
                'Session(S_ProjectId) = Nothing
                If Session(S_ProjectId) Is Nothing Then
                    If Not objHelpDbTable.GetProjectDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ProjectDetail, eStr_Retu) Then
                        Me.objCommon.ShowAlert("Error WhileGetting Data From View_MyProjectsForSetProject", Me.Page)
                        Exit Sub
                    End If

                    If Not ds_ProjectDetail Is Nothing AndAlso ds_ProjectDetail.Tables(0).Rows.Count > 0 Then
                        Session(S_ProjectId) = ds_ProjectDetail.Tables(0).Rows(0)("vWorkSpaceId")
                        Session(S_ProjectName) = "[" + ds_ProjectDetail.Tables(0).Rows(0)("vProjectNo") + "]" + ds_ProjectDetail.Tables(0).Rows(0)("vRequestId") + ""
                    End If
                End If

                If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                    'Me.txtprojectForDI.Text = Session(S_ProjectName) 'Comment By Bhargav Thaker
                    Me.HProjectId.Value = Session(S_ProjectId)
                    'btnSetProject_Click(sender, e) 'Comment By Bhargav Thaker
                End If

                Dim currentDay As DayOfWeek = DateTime.Now.DayOfWeek
                Dim daysTillCurrentDay As Integer = currentDay - DayOfWeek.Monday
                'Dim currentWeekStartDate As DateTime = DateTime.Now.AddDays(-7) 'Comment By Bhargav Thaker
                'txtVisitFromDate.Text = currentWeekStartDate.ToString("dd-MMM-yyyy") 'Comment By Bhargav Thaker
                'txtVisitToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy") 'Comment By Bhargav Thaker
            End If

            If (ddlProjectManager.SelectedIndex > 0) Then
                Me.AutoCompleteExtenderForTracking.ContextKey = "iProjectManagerId = " + ddlProjectManager.SelectedValue
                Me.AutoCompleteExtenderForClinicalPhase.ContextKey = "iProjectManagerId = " + ddlProjectManager.SelectedValue
                Me.AutoCompleteExtenderForAnalyticalPhase.ContextKey = "iProjectManagerId = " + ddlProjectManager.SelectedValue
                Me.AutoCompleteExtenderForDocumentPhase.ContextKey = "iProjectManagerId = " + ddlProjectManager.SelectedValue
            End If

            ' Stauts dropdown bind at runtime (Shyam Kamdar - 31/03/2021)
            'If Convert.ToString(ConfigurationManager.AppSettings("ImageUploaderUserCode")).Contains(Me.Session(S_UserType)) Then
            '    ddlVisitStatus.SelectedValue = ("ImageUploader")
            'ElseIf Convert.ToString(ConfigurationManager.AppSettings("QCUserCode")).Contains(Me.Session(S_UserType)) Then
            '    ddlVisitStatus.SelectedValue = ("QC1")
            'Else
            '    ddlVisitStatus.Items.Add("All")
            'End If
            'End
            dsMenu = GetMenuDataDashboard(Session(S_UserType), AdjucatorConstant)
            dvVisitReview.Visible = False
            dvAdjucatorReview.Visible = False
            'dvCaseAssign.Visible = False 'Added by Bhargav Thaker
            If dsMenu.Tables(0).Rows.Count > 0 Then
                For i = 0 To dsMenu.Tables(0).Rows.Count - 1
                    If (dsMenu.Tables(0).Rows(i)("MenuText").ToString() = VisitConstant) Then
                        dvVisitReview.Visible = True
                    End If

                    If (dsMenu.Tables(0).Rows(i)("MenuText").ToString() = AdjucatorConstant) Then
                        dvAdjucatorReview.Visible = True
                    End If

                    'Added by Bhargav Thaker
                    'If (dsMenu.Tables(0).Rows(i)("MenuURL").ToString() = CAConstant) Then
                    '    dvCaseAssign.Visible = True
                    'End If
                Next

                'If dsMenu.Tables(0).Rows.Count > 0 Then
                '    dvAdjucatorReview.Visible = True
                '    dvVisitReview.Visible = True
                '    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UICertificateDatatable", "UICertificateDatatable();", True)
                'Else
                '    dvAdjucatorReview.Visible = False
                '    dvVisitReview.Visible = False
                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UICertificateDatatable", "UICertificateDatatable();", False)

                FillQueryDetails()
            End If

            Me.FillDropDown() 'Added by Bhargav Thaker

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "...PageLoad")
        End Try
    End Sub

#End Region

#Region "FillDropDown"
    'Added by Bhargav Thaker
    Private Function FillDropDown() As Boolean
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim dv_Study As New DataView
        Dim eStr As String = String.Empty
        Dim ds_Project As DataSet
        Dim whereCondition As String = "cWorkspaceType = 'P' AND cIsTestSite <> 'Y' AND iUserId = " & Me.Session(S_UserID)

        If txtprojectForDI.Items.Count = 0 Then
            objHelp.GetFieldsOfTable("View_Myprojects", " *,'['+vProjectNo+'] '+vRequestId As vStudyNo ", whereCondition, ds_Project, eStr)

            dv_Study = ds_Project.Tables(0).DefaultView
            Me.txtprojectForDI.DataSource = dv_Study
            Me.txtprojectForDI.DataValueField = "vWorkspaceid"
            Me.txtprojectForDI.DataTextField = "vStudyNo"
            Me.txtprojectForDI.DataBind()
            Me.txtprojectForDI.Items.Insert(0, New System.Web.UI.WebControls.ListItem("Select Study", ""))

            For itxtprojectForDI As Integer = 0 To txtprojectForDI.Items.Count - 1
                txtprojectForDI.Items(itxtprojectForDI).Attributes.Add("title", txtprojectForDI.Items(itxtprojectForDI).Text)
            Next
        End If

        Dim Usertype As String = Session(S_UserType)
        Me.ddlVisitStatus.DataSource = Nothing
        Me.ddlVisitStatus.DataBind()

        If ddlVisitStatus.Items.Count = 0 Then
            Me.ddlVisitStatus.Items.Insert(0, New System.Web.UI.WebControls.ListItem("All", "All"))
            Me.ddlVisitStatus.Items.Insert(1, New System.Web.UI.WebControls.ListItem("Pending", "Pending"))
            If Usertype = "0122" Then
                Me.ddlVisitStatus.Items.Insert(2, New System.Web.UI.WebControls.ListItem("Image Uploader", "ImageUploader"))
                Me.ddlVisitStatus.Items.Insert(3, New System.Web.UI.WebControls.ListItem("QC1", "QC1"))
                Me.ddlVisitStatus.Items.Insert(4, New System.Web.UI.WebControls.ListItem("QC2", "QC2"))
                Me.ddlVisitStatus.Items.Insert(5, New System.Web.UI.WebControls.ListItem("Radiologist", "R12"))
            ElseIf Usertype = "0124" Then
                Me.ddlVisitStatus.Items.Insert(2, New System.Web.UI.WebControls.ListItem("QC1", "QC1"))
                Me.ddlVisitStatus.Items.Insert(3, New System.Web.UI.WebControls.ListItem("QC2", "QC2"))
                Me.ddlVisitStatus.Items.Insert(4, New System.Web.UI.WebControls.ListItem("CA1", "CA"))
                Me.ddlVisitStatus.Items.Insert(5, New System.Web.UI.WebControls.ListItem("Radiologist", "R12"))
            ElseIf Usertype = "0120" Then
                Me.ddlVisitStatus.Items.Insert(2, New System.Web.UI.WebControls.ListItem("Image Uploader", "ImageUploader"))
                Me.ddlVisitStatus.Items.Insert(3, New System.Web.UI.WebControls.ListItem("QC2", "QC2"))
                Me.ddlVisitStatus.Items.Insert(4, New System.Web.UI.WebControls.ListItem("CA1", "CA"))
                Me.ddlVisitStatus.Items.Insert(5, New System.Web.UI.WebControls.ListItem("Radiologist", "R12"))
            ElseIf Usertype = "0121" Then
                Me.ddlVisitStatus.Items.Insert(2, New System.Web.UI.WebControls.ListItem("Image Uploader", "ImageUploader"))
                Me.ddlVisitStatus.Items.Insert(3, New System.Web.UI.WebControls.ListItem("QC1", "QC1"))
                Me.ddlVisitStatus.Items.Insert(4, New System.Web.UI.WebControls.ListItem("CA1", "CA"))
                Me.ddlVisitStatus.Items.Insert(5, New System.Web.UI.WebControls.ListItem("Radiologist", "R12"))
            ElseIf Usertype = "0118" Or Usertype = "0119" Then
                Me.ddlVisitStatus.Items.Insert(2, New System.Web.UI.WebControls.ListItem("Image Uploader", "ImageUploader"))
                Me.ddlVisitStatus.Items.Insert(3, New System.Web.UI.WebControls.ListItem("QC1", "QC1"))
                Me.ddlVisitStatus.Items.Insert(4, New System.Web.UI.WebControls.ListItem("QC2", "QC2"))
                Me.ddlVisitStatus.Items.Insert(5, New System.Web.UI.WebControls.ListItem("CA1", "CA"))
            Else
                Me.ddlVisitStatus.Items.Insert(2, New System.Web.UI.WebControls.ListItem("Image Uploader", "ImageUploader"))
                Me.ddlVisitStatus.Items.Insert(3, New System.Web.UI.WebControls.ListItem("QC1", "QC1"))
                Me.ddlVisitStatus.Items.Insert(4, New System.Web.UI.WebControls.ListItem("QC2", "QC2"))
                Me.ddlVisitStatus.Items.Insert(5, New System.Web.UI.WebControls.ListItem("CA1", "CA"))
                Me.ddlVisitStatus.Items.Insert(6, New System.Web.UI.WebControls.ListItem("Radiologist", "R12"))
            End If
            Me.ddlVisitStatus.SelectedIndex = 1
        End If
        Return True
    End Function
#End Region

#Region "My Calendar"
#Region "fillActivityName"
    Public Sub fillActivityName()
        Dim Wstr As String = String.Empty
        Dim Estr As String = String.Empty
        Dim Dv_ActivityName As New DataView
        Dim Ds_ActivityName As New DataSet

        If Not Me.objHelpDbTable.Proc_GetActivityForDashBoard(3, IIf(DdllistForDepartment.SelectedIndex = 0, "", Session(S_DeptCode)), Ds_ActivityName, Estr) Then
            Me.objCommon.ShowAlert("Error While Getting Activity", Me.Page)
            Exit Sub
        End If

        If Not Ds_ActivityName Is Nothing AndAlso Ds_ActivityName.Tables(0).Rows.Count > 0 Then
            Dv_ActivityName = Ds_ActivityName.Tables(0).DefaultView().ToTable(True, "vActivityName,vActivityId".Split(", ")).DefaultView()
            Dv_ActivityName.Sort = "vActivityName"
            DdllistActivityName.DataSource = Dv_ActivityName.ToTable()
            DdllistActivityName.DataTextField = "vActivityName"
            DdllistActivityName.DataValueField = "vActivityId"
            DdllistActivityName.DataBind()
            DdllistActivityName.Width = 200
            DdllistActivityName.Items.Insert(0, "All Activities")
        End If
    End Sub
#End Region

#Region "set drop down month with current month"
    Public Sub setDropdownmonthwithcurrentmonthandyear()
        Dim currentmonth = DateTime.Now.Month.ToString()
        DdlListMonthForMyCalendar.SelectedIndex = currentmonth
        DdlListYearForMyCalendar.SelectedIndex = 2
    End Sub
#End Region

#Region "setIntialCalendar"
    Public Sub setIntialCalendar()
        Dim ActivityGroupLoop As Integer = 0
        Dim Wstr As String = String.Empty
        Dim Estr As String = String.Empty
        Dim ds_Details As New DataSet
        Dim FirstDate As String
        Dim SecondDate As String
        Dim CurrentMonthName As String = String.Empty
        Dim LastDateInSelectedMonth As String = ""
        Dim GroupOfActivityId As String = String.Empty

        CurrentMonthName = Get_MonthName(DateTime.Now.Month)
        FirstDate = "01" + "-" + CurrentMonthName + "-" + DateTime.Now.Year.ToString()
        LastDateInSelectedMonth = DateTime.DaysInMonth(CInt(DateTime.Now.Year), CInt(DateTime.Now.Month))
        SecondDate = LastDateInSelectedMonth & "-" & CurrentMonthName & "-" & DateTime.Now.Year.ToString()

        ViewState("selectedmonth") = CurrentMonthName
        ViewState("selectedyear") = DateTime.Now.Year.ToString()
        ViewState("lastdateofmonth") = LastDateInSelectedMonth

        'For ActivityGroupLoop = 0 To DdllistActivityName.Items.Count - 1
        '    If ActivityGroupLoop = DdllistActivityName.Items.Count - 1 Then
        '        GroupOfActivityId += "'" & DdllistActivityName.SelectedValue(ActivityGroupLoop).ToString() & "'"
        '    Else
        '        GroupOfActivityId += "'" & DdllistActivityName.SelectedValue(.ToString() & "'" & ","
        '    End If

        'Next

        For li As Integer = 1 To Me.DdllistActivityName.Items.Count - 1
            GroupOfActivityId += "'" + Me.DdllistActivityName.Items(li).Value.ToString + "'" + ","
        Next li
        If GroupOfActivityId = "" Then
            Wstr = "vDeptCode='" & Session(S_DeptCode).ToString() & "'"
        Else
            GroupOfActivityId = GroupOfActivityId.Substring(0, GroupOfActivityId.Length - 1)
            Wstr = "vDeptCode='" & Session(S_DeptCode).ToString() & "'And vActivityId in(" & GroupOfActivityId & ")"
        End If

        If ddlProjectManager.SelectedIndex > 0 Then
            Wstr += " AND iProjectManagerId ='" & ddlProjectManager.SelectedValue + "'"
        End If

        'Wstr = "vDeptCode='" & DdllistForDepartment.SelectedValue.ToString() & "' and cast(convert(varchar(11),SchStart ,113) as smalldatetime)>= cast(convert(varchar(11),cast('" & FirstDate & "'as datetime),113)as smalldatetime)And cast(convert(varchar(11),SchStart,113)as smalldatetime)<= cast(convert(varchar(11),cast('" & SecondDate & "'as datetime),113)as smalldatetime)"

        If Not objHelpDbTable.View_ActivityStartEndDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Details, Estr) Then
            Me.objCommon.ShowAlert("Error WhileGetting Data From View_ActivityStartEndDtl", Me.Page)
            Exit Sub
        End If

        ViewState("DetailsOfSelectedMonth") = ds_Details
        'DdllistForDepartment.SelectedValue = Session(S_DeptCode).ToString()
        CldDatePicker.Visible = True
        CldDatePicker.VisibleDate = FirstDate
        PnlLegendsForMyCalendar.Visible = True
    End Sub
#End Region

#Region "Get_Month_Name"
    Public Function Get_MonthName(ByVal MonthNumber As Integer) As String
        If MonthNumber = 1 Then
            Return "Jan"
        ElseIf MonthNumber = 2 Then
            Return "Feb"
        ElseIf MonthNumber = 3 Then
            Return "Mar"
        ElseIf MonthNumber = 4 Then
            Return "Apr"
        ElseIf MonthNumber = 5 Then
            Return "May"
        ElseIf MonthNumber = 6 Then
            Return "Jun"
        ElseIf MonthNumber = 7 Then
            Return "Jul"
        ElseIf MonthNumber = 8 Then
            Return "Aug"
        ElseIf MonthNumber = 9 Then
            Return "Sep"
        ElseIf MonthNumber = 10 Then
            Return "Oct"
        ElseIf MonthNumber = 11 Then
            Return "Nov"
        ElseIf MonthNumber = 12 Then
            Return "Dec"
        End If
    End Function
#End Region

#Region "Fill Drop Down Year"
    Public Sub fillDdlYear()
        Dim currentyear As Integer
        currentyear = DateTime.Now.Year
        DdlListYearForMyCalendar.Items.Clear()

        DdlListYearForMyCalendar.Items.Insert(0, "Select...")
        Dim iForLoop As Integer = 2

        For iForLoop = 2 To 12
            If iForLoop = 2 Then
                DdlListYearForMyCalendar.Items.Insert(iForLoop - 1, currentyear + 1)
            Else
                DdlListYearForMyCalendar.Items.Insert(iForLoop - 1, (currentyear + 1) - (iForLoop - 2))
            End If
        Next

    End Sub
#End Region

#Region "Fill DropDown Months"

    Public Sub fillDropDownMonth()
        DdlListMonthForMyCalendar.Items.Clear()
        DdlListMonthForMyCalendar.Items.Insert(0, "Select...")
        DdlListMonthForMyCalendar.Items.Insert(1, "January")
        DdlListMonthForMyCalendar.Items.Insert(2, "February")
        DdlListMonthForMyCalendar.Items.Insert(3, "March")
        DdlListMonthForMyCalendar.Items.Insert(4, "April")
        DdlListMonthForMyCalendar.Items.Insert(5, "May")
        DdlListMonthForMyCalendar.Items.Insert(6, "June")
        DdlListMonthForMyCalendar.Items.Insert(7, "July")
        DdlListMonthForMyCalendar.Items.Insert(8, "August")
        DdlListMonthForMyCalendar.Items.Insert(9, "September")
        DdlListMonthForMyCalendar.Items.Insert(10, "October")
        DdlListMonthForMyCalendar.Items.Insert(11, "November")
        DdlListMonthForMyCalendar.Items.Insert(12, "December")
    End Sub

#End Region

#Region "Fill Dept Drop Down"

    Public Sub fillDeptDropDown()
        Dim wstr As String = "1=1"
        Dim ds_Dept As New DataSet
        Dim estr As String = String.Empty
        Dim dv_dept As DataView
        If Not objHelpDbTable.GetDeptMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Dept, estr) Then
            Me.objCommon.ShowAlert("Error While Getting Data From DeptMst", Me.Page)
            Exit Sub
        End If
        dv_dept = ds_Dept.Tables(0).DefaultView()
        dv_dept.ToTable().AcceptChanges()

        dv_dept = ds_Dept.Tables(0).DefaultView.ToTable(True, "vDeptName").DefaultView()
        dv_dept.ToTable().AcceptChanges()

        DdllistForDepartment.DataSource = ds_Dept
        DdllistForDepartment.DataTextField = "vDeptName"
        DdllistForDepartment.DataValueField = "vDeptCode"
        DdllistForDepartment.DataBind()
        DdllistForDepartment.Items.Insert(0, "All Department")

    End Sub

#End Region

#Region "Calendar Events"

    Protected Sub CldDatePicker_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CldDatePicker.SelectionChanged

        'Me.DivCalendar.Style(HtmlTextWriterStyle.Display) = "block"
    End Sub

    Protected Sub CldDatePicker_DayRender(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DayRenderEventArgs) Handles CldDatePicker.DayRender
        'If ViewState(VS_Mode) = "N" Then
        'Else
        ViewState(VS_Mode) = "Y"
        Dim jForLoop As Integer
        Dim kForLoop As Integer
        Dim Mark As Integer = 0
        Dim ds_DetailsOfSelectedMonth As New DataSet
        Dim dv_DetailOfSleectedMonth As New DataView
        Dim ProjectList(2) As String
        Dim selectedmonth As String = String.Empty
        Dim selectedyear As String = String.Empty
        Dim LastDateOfSelectedMonth As String = String.Empty
        Dim dv_datewise As New DataView
        Dim lstitem As New ListItemCollection
        Dim Dv_TotalDetailWithDept As New DataView
        Dim dv_UniqueWorkSpaceId As New DataView
        Dim dt_Details As New DataTable
        Dim dv_WorkSpaceIdWise As New DataView
        Dim dv_FillEachDateWise As New DataView
        Dim firstdate As String
        Dim lastdate As String
        Dim LstProjectNoColor As New System.Web.UI.WebControls.ListItem
        Dim lstItemCollection As New ListItemCollection
        Dim dt_DistinctProjerctNo As New DataTable
        Dim dv_DsitinctWorkSpaceiD As New DataView
        Dim div As New HtmlGenericControl("div")
        'Dim ImgForSelection As New HtmlImage

        div.ID = "div" + e.Day.DayNumberText.ToString
        'div.Style("clear") = "both"
        'div.Style("float") = "right"
        div.Style("top") = "0"
        'div.Style("display") = "table-cell"
        div.Style("font-weight") = "20"
        'div.Style("margin-right") = "15px"
        'div.Style("border") = "outset 1px black"
        div.Style("width") = "80px"


        'ImgForSelection.Src = "~/images/collapse.jpg"
        'ImgForSelection.Style("float") = "right"
        selectedmonth = ViewState("selectedmonth").ToString()
        selectedyear = ViewState("selectedyear").ToString()
        LastDateOfSelectedMonth = ViewState("lastdateofmonth").ToString()
        firstdate = "01" & "-" & ViewState("selectedmonth").ToString() & "-" & ViewState("selectedyear").ToString()
        lastdate = LastDateOfSelectedMonth & "-" & ViewState("selectedmonth").ToString() & "-" & ViewState("selectedyear").ToString()

        ds_DetailsOfSelectedMonth = CType(ViewState("DetailsOfSelectedMonth"), DataSet)

        dv_DetailOfSleectedMonth = ds_DetailsOfSelectedMonth.Tables(0).DefaultView()
        dv_datewise = ds_DetailsOfSelectedMonth.Tables(0).DefaultView()
        dv_datewise.RowFilter = "SchStart >= #" & firstdate & "# And SchStart <= #" & lastdate & "#"
        dv_datewise.ToTable().AcceptChanges()
        dv_UniqueWorkSpaceId = dv_datewise.ToTable(True, "vWorkSpaceId").DefaultView()
        dv_UniqueWorkSpaceId.ToTable.AcceptChanges()

        dt_DistinctProjerctNo.Clear()
        Dim col1 As New DataColumn

        If Not e.Day.IsOtherMonth Then

            If e.Day.Date = CDate(e.Day.DayNumberText & "-" & ViewState("selectedmonth").ToString() & "-" & ViewState("selectedyear").ToString()) Then
                If e.Day.Date.DayOfWeek = DayOfWeek.Sunday Then
                    'e.Cell.Style("background") = "url(images/holyday_bg.jpg) #ffda18"
                    'e.Cell.Style("background-repeat") = "repeat-x"

                    e.Cell.Style("background") = "url(images/holyday_bg2.png)"
                    e.Cell.Style("background-repeat") = "repeat"

                    'e.Cell.Attributes.Add("OnMouseOver", "SetColorBlue(this);")
                    'e.Cell.Attributes.Add("OnMouseOut", "SetColorRed(this);")
                Else
                    'e.Cell.Style("background") = "url(images/date_bg.jpg) #c0e8f8"
                    'e.Cell.Style("background-repeat") = "repeat-x"

                    e.Cell.Style("background") = "url(images/whitebg.jpg)"
                    e.Cell.Style("background-repeat") = "repeat-x"

                    e.Cell.Attributes.Add("OnMouseOver", "SetColorOfficeDaysHover(this);")
                    e.Cell.Attributes.Add("OnMouseOut", "SetColorOfficeDaysOut(this);")
                End If
                dv_FillEachDateWise = dv_datewise.ToTable().Copy().DefaultView()
                dv_FillEachDateWise.RowFilter = "SchStart=#" & e.Day.DayNumberText & "-" & ViewState("selectedmonth").ToString() & "-" & ViewState("selectedyear").ToString() & "#"


                dv_DsitinctWorkSpaceiD = dv_FillEachDateWise.ToTable(True, "vprojectNo,TotalSubjects".Split(",")).Copy.DefaultView()

                For jForLoop = 0 To dv_DsitinctWorkSpaceiD.ToTable().Rows.Count - 1
                    Mark = 0
                    Dim dv_SubDtlWise As New DataView
                    dv_SubDtlWise = dv_FillEachDateWise.ToTable().Copy.DefaultView()
                    dv_SubDtlWise.RowFilter = "vProjectNo='" & dv_DsitinctWorkSpaceiD.ToTable().Rows(jForLoop).Item("vProjectNo").ToString() & "'"

                    For kForLoop = 0 To dv_SubDtlWise.ToTable().Rows.Count - 1
                        '               e.Cell.Controls.Add(ImgForSelection)

                        If dv_SubDtlWise.ToTable().Rows(kForLoop).Item("ActStart") Is DBNull.Value Then
                            Mark = Brown
                            Exit For
                        ElseIf CDate(dv_SubDtlWise.ToTable().Rows(kForLoop).Item("ActStart").ToString()) > CDate(dv_SubDtlWise.ToTable().Rows(kForLoop).Item("SchStart").ToString()) Then
                            Mark = RedColor
                            Exit For

                        ElseIf CDate(dv_SubDtlWise.ToTable().Rows(kForLoop).Item("ActStart").ToString()) = CDate(dv_SubDtlWise.ToTable().Rows(kForLoop).Item("SchStart").ToString()) Then

                            If Not Mark > BlueColor Then
                                Mark = BlueColor
                            End If

                        ElseIf CDate(dv_SubDtlWise.ToTable().Rows(kForLoop).Item("ActStart").ToString()) < CDate(dv_SubDtlWise.ToTable().Rows(kForLoop).Item("SchStart").ToString()) Then

                            If Not Mark > GreenColor Then
                                Mark = GreenColor
                            End If

                        End If

                    Next kForLoop

                    e.Cell.Width = 150

                    'div.Controls.Add(New LiteralControl("<br/>"))
                    'div.Style(HtmlTextWriterStyle.Height) = "50px"
                    'div.Attributes.Add("OnMouseOver", "Call(this);")
                    'div.Attributes.Add("OnMouseOut", "CallOut(this);")
                    Dim LnkBtn As LinkButton
                    LnkBtn = New LinkButton
                    'div.Controls.Add(New LiteralControl("<br/>"))
                    LnkBtn.Text = dv_DsitinctWorkSpaceiD.ToTable().Rows(jForLoop).Item("vProjectNo").ToString() & "(" & dv_DsitinctWorkSpaceiD.ToTable().Rows(jForLoop).Item("TotalSubjects").ToString() & ")"
                    'LnkBtn.ID = dv_DsitinctWorkSpaceiD.ToTable().Rows(jForLoop).Item("vWorkSpaceId").ToString()
                    LnkBtn.ForeColor = Drawing.Color.Red
                    LnkBtn.Width = 80%
                    If Mark = GreenColor Then
                        LnkBtn.ForeColor = Drawing.Color.Green
                    ElseIf Mark = BlueColor Then
                        LnkBtn.ForeColor = Drawing.Color.Blue
                    ElseIf Mark = Brown Then
                        LnkBtn.ForeColor = Drawing.Color.Brown
                    End If
                    LnkBtn.Font.Size = 8
                    LnkBtn.PostBackUrl = ""
                    LnkBtn.CssClass = "MyLinkButtons"
                    LnkBtn.OnClientClick = "ShowActivity(this, '" + e.Day.DayNumberText.ToString + "','" + dv_DsitinctWorkSpaceiD.ToTable().Rows(jForLoop).Item("vProjectNo").ToString() + "')"

                    div.Controls.Add(LnkBtn)

                Next jForLoop
                '     ImgForSelection.Attributes("onclick") = "jQuery('#" + div.ClientID + "').toggle('medium');"
            End If

            e.Cell.Controls.Add(div)
        End If


        ' End If

    End Sub

#End Region

#Region "Button Events"

    'Protected Sub BtnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClose.Click
    '    '  Me.DivCalendar.Style(HtmlTextWriterStyle.Display) = "none"
    'End Sub

    Protected Sub BtnGOForCalendar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGOForCalendar.Click
        Dim Wstr As String = String.Empty
        Dim Estr As String = String.Empty
        Dim ds_Details As New DataSet
        Dim FirstDate As String
        Dim SecondDate As String
        Dim LastDateInSelectedMonth As String = ""
        Dim GroupOfActivityId As String = String.Empty
        If DdllistActivityName.Items.Count = 0 Then
            Me.objCommon.ShowAlert("There is no any Activity Exist to Filter", Me.Page)
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divMyCalendar');", True)
            Exit Sub
        End If

        FirstDate = "01" + "-" + DdlListMonthForMyCalendar.SelectedItem.ToString().Trim() + "-" + DdlListYearForMyCalendar.SelectedItem.ToString()
        LastDateInSelectedMonth = DateTime.DaysInMonth(CInt(DdlListYearForMyCalendar.Text.ToString()), CInt(DdlListMonthForMyCalendar.SelectedIndex.ToString()))
        SecondDate = LastDateInSelectedMonth & "-" & DdlListMonthForMyCalendar.Text.ToString() & "-" & DdlListYearForMyCalendar.Text.ToString()

        ViewState("selectedmonth") = DdlListMonthForMyCalendar.Text.ToString().Substring(0, 3)
        ViewState("selectedyear") = DdlListYearForMyCalendar.Text.ToString()
        ViewState("lastdateofmonth") = LastDateInSelectedMonth
        If DdllistActivityName.SelectedItem.Text = "All Activities" Then

            For li As Integer = 1 To Me.DdllistActivityName.Items.Count - 1
                GroupOfActivityId += "'" + Me.DdllistActivityName.Items(li).Value.ToString + "'" + ","
            Next li
            If GroupOfActivityId = "" Then

            Else
                GroupOfActivityId = GroupOfActivityId.Substring(0, GroupOfActivityId.Length - 1)
                Wstr = "vActivityId in(" & GroupOfActivityId & ")"
            End If

        Else
            Wstr = "vActivityId= '" & DdllistActivityName.SelectedValue.ToString() & "'"
        End If


        If DdllistForDepartment.SelectedItem.Text = "All Department" Then
            Dim GroupOfDeptCodeId As String = String.Empty


            For li As Integer = 1 To Me.DdllistForDepartment.Items.Count - 1
                GroupOfDeptCodeId += "'" + Me.DdllistForDepartment.Items(li).Value.ToString + "'" + ","
            Next li
            If GroupOfDeptCodeId = "" Then
                If Wstr = "" Then
                    Wstr += " vDeptCode='" & DdllistForDepartment.SelectedValue.ToString() & "'"
                Else
                    Wstr += "AND vDeptCode='" & DdllistForDepartment.SelectedValue.ToString() & "'"
                End If




            Else
                GroupOfDeptCodeId = GroupOfDeptCodeId.Substring(0, GroupOfDeptCodeId.Length - 1)
                If Wstr = "" Then
                    Wstr += "  vDeptCode in(" & GroupOfDeptCodeId & ")"
                Else
                    Wstr += " And vDeptCode in(" & GroupOfDeptCodeId & ")"
                End If

            End If

        Else
            Wstr += " AND vDeptCode='" & DdllistForDepartment.SelectedValue.ToString() + "'"
        End If
        If ddlProjectManager.SelectedIndex > 0 Then
            Wstr += " AND iProjectManagerId ='" & ddlProjectManager.SelectedValue + "'"
        End If

        If Not objHelpDbTable.View_ActivityStartEndDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Details, Estr) Then
            Me.objCommon.ShowAlert("Error WhileGetting Data From View_ActivityStartEndDtl", Me.Page)
            Exit Sub
        End If

        ViewState("DetailsOfSelectedMonth") = ds_Details
        CldDatePicker.Visible = True


        CldDatePicker.VisibleDate = FirstDate
        PnlLegendsForMyCalendar.Visible = True
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divMyCalendar');", True)

    End Sub

    Protected Sub BtnGetActivityDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGetActivityDetails.Click
        Dim WorkSpaceId As String = HdDFieldWorkSapceId.Value.ToString()
        Dim selecteddate As String = HdFieldDate.Value.ToString()
        Dim ds_ActivityDetails As New DataSet
        Dim dv_SelectedDetails As New DataView
        ds_ActivityDetails = CType(ViewState("DetailsOfSelectedMonth"), DataSet)
        dv_SelectedDetails = ds_ActivityDetails.Tables(0).DefaultView()
        dv_SelectedDetails.ToTable().AcceptChanges()
        dv_SelectedDetails.RowFilter = "SchStart= #" & selecteddate & "-" & ViewState("selectedmonth").ToString() & "-" & ViewState("selectedyear").ToString() & "# and vProjectNo='" & HdFieldProjectNo.Value.ToString() & "'"
        dv_SelectedDetails.ToTable().AcceptChanges()
        ViewState("SelectedDateList") = dv_SelectedDetails.ToTable()
        GvwPnlPnlActivityDetails.DataSource = dv_SelectedDetails.ToTable()
        GvwPnlPnlActivityDetails.DataBind()
        LblPopUpTitle.Text = "Activity Details For Project No : " & HdFieldProjectNo.Value.ToString() & ""
        LblPopUpTitle.Visible = True
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divMyCalendar');", True)
        Mpedialog3.Show()

    End Sub

    Protected Sub btnSetForVisitTracker_Click(sender As Object, e As EventArgs) Handles btnSetForVisitTracker.Click

        If Not FillGrid() Then
            ShowErrorMessage("Error While FillGrid", "")
        End If
    End Sub

    Protected Sub btnSetProjectforVisitScheduler_Click(sender As Object, e As EventArgs) Handles btnSetProjectforVisitScheduler.Click

        If Not FillGridForVisitScheduler() Then
            ShowErrorMessage("Error While FillGrid", "")
        End If
    End Sub


#End Region

#Region "Grid Events"


    Protected Sub GvwPnlPnlActivityDetails_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GvwPnlPnlActivityDetails.RowCommand

    End Sub

    Protected Sub GvwPnlPnlActivityDetails_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvwPnlPnlActivityDetails.RowCreated

    End Sub

    Protected Sub GvwPnlPnlActivityDetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvwPnlPnlActivityDetails.RowDataBound
        Dim dt_SelectedDateDtl As New DataTable

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(Gvc_SrnO).Text = e.Row.RowIndex + 1
            If e.Row.Cells(GVC_ActStart).Text.ToString() = "&nbsp;" Then
                CType(e.Row.FindControl("ImgIndicator"), System.Web.UI.WebControls.Image).ImageUrl = "~/images/themeBrown.png"
                Exit Sub
            ElseIf CDate(e.Row.Cells(GVC_ActStart).Text.ToString()) > CDate(e.Row.Cells(GVC_SchStart).Text.ToString()) Then
                CType(e.Row.FindControl("ImgIndicator"), System.Web.UI.WebControls.Image).ImageUrl = "~/images/themeRed.png"
                Exit Sub
            ElseIf CDate(e.Row.Cells(GVC_ActStart).Text.ToString()) = CDate(e.Row.Cells(GVC_SchStart).Text.ToString()) Then
                CType(e.Row.FindControl("ImgIndicator"), System.Web.UI.WebControls.Image).ImageUrl = "~/images/themeBlue.png"
                Exit Sub
            ElseIf CDate(e.Row.Cells(GVC_ActStart).Text.ToString()) < CDate(e.Row.Cells(GVC_SchStart).Text.ToString()) Then
                CType(e.Row.FindControl("ImgIndicator"), System.Web.UI.WebControls.Image).ImageUrl = "~/images/themeGreen.png"
                Exit Sub
            End If

        End If
    End Sub


#End Region

#Region "Calendar Events "

    Protected Sub CldDatePicker_VisibleMonthChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MonthChangedEventArgs) Handles CldDatePicker.VisibleMonthChanged

        Dim Wstr As String = String.Empty
        Dim ds_Details As New DataSet
        Dim Estr As String = String.Empty
        Dim FirstDate As String = String.Empty
        Dim LastDateInSelectedMonth As String = String.Empty
        Dim Month As String = String.Empty
        'CldDatePicker.VisibleDate.Month
        Dim Year As String = String.Empty
        Dim GroupOfActivityId As String = String.Empty
        Dim MonthAsInteger As Integer = 0
        Dim YearAsInteger As Integer = 0
        'DdllistMonth.Items.FindByValue(CldDatePicker.VisibleDate.Month.ToString()).Selected = True
        Month = Get_MonthName(CldDatePicker.VisibleDate.Month)
        Year = CldDatePicker.VisibleDate.Year.ToString()

        FirstDate = "01" & "-" & Month & "-" & Year
        CldDatePicker.Visible = True
        LastDateInSelectedMonth = DateTime.DaysInMonth(CldDatePicker.VisibleDate.Year, CldDatePicker.VisibleDate.Month)
        ViewState("selectedmonth") = Month
        ViewState("lastdateofmonth") = LastDateInSelectedMonth
        ViewState("selectedyear") = Year
        CldDatePicker.VisibleDate = FirstDate
        If DdllistActivityName.SelectedItem.Text.ToString() = "All Activities" Then
            For li As Integer = 1 To Me.DdllistActivityName.Items.Count - 1
                GroupOfActivityId += "'" + Me.DdllistActivityName.Items(li).Value.ToString + "'" + ","
            Next li
            If GroupOfActivityId = "" Then
                Wstr = "vDeptCode='" & DdllistForDepartment.SelectedValue.ToString() & "'"
            Else
                GroupOfActivityId = GroupOfActivityId.Substring(0, GroupOfActivityId.Length - 1)
                Wstr = "vDeptCode='" & DdllistForDepartment.SelectedValue.ToString() & "'And vActivityId in(" & GroupOfActivityId & ")"
            End If

        Else
            Wstr = "vDeptCode='" & DdllistForDepartment.SelectedValue.ToString() & "'And vActivityId='" & DdllistActivityName.SelectedValue.ToString() & "'"
        End If

        If ddlProjectManager.SelectedIndex > 0 Then
            Wstr += " AND iProjectManagerId ='" & ddlProjectManager.SelectedValue + "'"
        End If

        If Not objHelpDbTable.View_ActivityStartEndDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Details, Estr) Then
            Me.objCommon.ShowAlert("Error WhileGetting Data From View_ActivityStartEndDtl", Me.Page)
            Exit Sub
        End If
        YearAsInteger = CldDatePicker.VisibleDate.Year
        DdlListYearForMyCalendar.SelectedIndex = DdlListYearForMyCalendar.Items.IndexOf(DdlListYearForMyCalendar.Items.FindByText(YearAsInteger))
        MonthAsInteger = CldDatePicker.VisibleDate.Month
        DdlListMonthForMyCalendar.SelectedIndex = MonthAsInteger
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divMyCalendar');", True)
        ViewState("DetailsOfSelectedMonth") = ds_Details
    End Sub

#End Region


#End Region

#Region "Fill Operational Kpi Controls"

    Public Sub FillOperationalKpiControls()
        Me.DdllistForOperationalKpi.Items.Clear()
        Me.DdllistForOperationalKpi.Items.Add("Current Month")
        Me.DdllistForOperationalKpi.Items.Add("Last Month")
        Me.DdllistForOperationalKpi.Items.Add("Last Quater")
        Me.DdllistForOperationalKpi.Items.Add("Current Calendar Year")
        Me.DdllistForOperationalKpi.Items.Add("Last Calendar Year")
        Me.DdllistForOperationalKpi.Items.Add("Current Financial Year")
        Me.DdllistForOperationalKpi.Items.Add("Last Financial Year")
    End Sub

    Public Sub FillKpiWithCurrentDate()
        Dim FromDateOfOperationalKpi As String = String.Empty
        Dim LastdayOfPpreviousMonth As String = String.Empty
        Dim ToDateOfOperationalKpi As String = String.Empty
        FromDateOfOperationalKpi = "01" & "-" & MonthName(DateTime.Now.Month, True) & "-" & DateTime.Now.Year.ToString()
        LastdayOfPpreviousMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)
        ToDateOfOperationalKpi = LastdayOfPpreviousMonth & "-" & MonthName(DateTime.Now.Month, True) & "-" & DateTime.Now.Year
        TxtFromDateOfOperationalKpi.Text = FromDateOfOperationalKpi
        TxtToDateOfOperationalKpi.Text = ToDateOfOperationalKpi
        TxtFromDateOfOperationalKpi.Enabled = True
        TxtToDateOfOperationalKpi.Enabled = True
        LblFromDateForOperationalKpi.Enabled = True
        LblToDateForOperationalKpi.Enabled = True
        BtnGoForOperationalKpi.Enabled = True
    End Sub

    Public Sub getIntialDataForOperationalKpi()
        'BtnGoForOperationalKpi_Click("", Nothing)
        GetKPIData()
    End Sub

#End Region

#Region "Fill Other Controls"

    Public Sub FillOtherControls()
        BindDropDownOfStusyWorkSummary()
    End Sub

#End Region

#Region "Get Counts"

    Private Sub GetCounts()
        Dim ds As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            If Not objCommon.GetScopeValueWithCondition(wStr) Then
                Exit Sub
            End If
            wStr = wStr.Substring(wStr.IndexOf("(") + 1, (wStr.LastIndexOf(")") + 1) - (wStr.IndexOf("(") + 2))
            wStr = wStr.Replace("'", "") + "##" + Me.ddlLocation.SelectedValue() + "##" + IIf(ddlProjectManager.SelectedIndex = 0, "", ddlProjectManager.SelectedValue())

            If Not Me.objHelpDbTable.Proc_GetProjectStatusCount(wStr, ds, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds Is Nothing Then

                If ds.Tables(0).Rows.Count > 0 Then

                    Me.LblClientRequestProjects.Text = Convert.ToString(ds.Tables(0).Rows(0)("NoOfInitialPhaseProjects")).Trim()
                    Me.LblProjectPreClinical.Text = Convert.ToString(ds.Tables(0).Rows(0)("NoOfPreClinicalPhaseProjects")).Trim()
                    Me.LblProjectsClinincalPhase.Text = Convert.ToString(ds.Tables(0).Rows(0)("NoOfClinicalPhaseProjects")).Trim()
                    'Me.LblProjectsClinincalPhaseCompleted.Text = " (" + Convert.ToString(ds.Tables(0).Rows(0)("NoOfClinicalPhaseCompletedProjects")).Trim() + ")"
                    Me.LblAnalyticalPhase.Text = Convert.ToString(ds.Tables(0).Rows(0)("NoOfAnalyticalPhaseProjects")).Trim()
                    'Me.LblAnalyticalPhaseCompleted.Text = " (" + Convert.ToString(ds.Tables(0).Rows(0)("NoOfAnalyticalPhaseCompletedProjects")).Trim() + ")"
                    Me.LblDocumentPhase.Text = Convert.ToString(ds.Tables(0).Rows(0)("NoOfDocumentPhaseProjects")).Trim()

                End If

            End If

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Getting Counts Of Projects. ", ex.Message)
        End Try

    End Sub

#End Region

#Region "GetDiplayDetails"

    Private Function GetDiplayDetails() As Boolean

        dsDisplayDetails = New DataSet
        Dim dsTemp As New DataSet

        Dim dtDisplayDetails As New DataTable
        Dim errStr As String = ""
        Dim result As Boolean = False

        If Not objHelpDbTable.GetPanelDisplayDetailByUserType(" vUserTypeCode = " & Session(S_UserType) + " AND cActiveFlag ='Y' ",
                                                         WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                                         dsDisplayDetails, eStr_Retu) Then
            GetDiplayDetails = False
            Exit Function
        End If

        dtDisplayDetails = dsDisplayDetails.Tables(0)
        Dim obj(2) As Object
        obj(0) = dsTemp
        obj(1) = errStr

        If dtDisplayDetails.Rows.Count <= 0 Then
            GetDiplayDetails = True
            Exit Function
        End If
        For Each dr As DataRow In dtDisplayDetails.Rows

            If dr("vPanelId").ToString = "0001" Then
                Me.TrClientRequest.Style.Add("display", "block")
            End If

            If dr("vPanelId").ToString = "0013" Then
                Me.TrPreClinical.Style.Add("display", "block")
            End If

            If dr("vPanelId").ToString = "0003" Then
                Me.TrClinicalPhase.Style.Add("display", "block")
            End If

            If dr("vPanelId").ToString = "0004" Then
                Me.TrDocument.Style.Add("display", "block")
            End If

            If dr("vPanelId").ToString = "0007" Then
                Me.TrAnalytical.Style.Add("display", "block")
            End If

            If dr("vPanelId").ToString = "0008" Then
                Me.TrProjectTrack.Style.Add("display", "block")
            End If

            If dr("vPanelId").ToString = "0009" Then
                Me.TrScreeningAnalytic.Style.Add("display", "block")
            End If

            If dr("vPanelId").ToString = "0010" Then
                Me.TrProjectStudyWorkSummary.Style.Add("display", "block")
                'FillOtherControls()
            End If
            If dr("vPanelId").ToString = "0011" Then
                Me.TrOperationalKpi.Style.Add("display", "block")
                'FillOperationalKpiControls()
                'FillKpiWithCurrentDate()
                'getIntialDataForOperationalKpi()
            End If
            If dr("vPanelId").ToString = "0012" Then
                Me.TrMyCalendar.Style.Add("display", "block")
                Me.TrMyCalendar.Attributes.Add("onclick", "ShowHideDiv('divMyCalendar','imgMyCalendar','MyCalendar');")
                'fillDeptDropDown()
                'fillActivityName()
                'fillDdlYear()
                'fillDropDownMonth()
                'setDropdownmonthwithcurrentmonthandyear()
                'setIntialCalendar()
            End If
            If dr("vPanelId").ToString = "0014" Then
                Me.TrVisitTracker.Style.Add("display", "block")
                'fillDeptDropDown()
                'fillActivityName()
                'fillDdlYear()
                'fillDropDownMonth()
                'setDropdownmonthwithcurrentmonthandyear()
                'setIntialCalendar()
            End If
            If dr("vPanelId").ToString = "0014" Then
                Me.TrVisitScheduler.Style.Add("display", "block")
                'fillDeptDropDown()
                'fillActivityName()
                'fillDdlYear()
                'fillDropDownMonth()
                'setDropdownmonthwithcurrentmonthandyear()
                'setIntialCalendar()
            End If

            '' Added by vivekpatel
            If dr("vPanelId").ToString = "0016" Then
                Me.TrSiteWiseSubjectDetail.Style.Add("display", "block")
            End If
            If dr("vPanelId").ToString = "0017" Then
                Me.TrSiteInformation.Style.Add("display", "block")
            End If
            If dr("vPanelId").ToString = "0018" Then
                Me.TrCRFStatus.Style.Add("display", "block")
            End If
            If dr("vPanelId").ToString = "0019" Then
                Me.TrAESAE.Style.Add("display", "block")
            End If

            If dr("vPanelId").ToString = "0020" Then
                Me.TrDemo.Style.Add("display", "block")
            End If
            If dr("vPanelId").ToString = "0021" Then
                Me.TrDemo1.Style.Add("display", "block")
            End If
            If dr("vPanelId").ToString = "0023" Then
                Me.TrDCFManage.Style.Add("display", "block")
            End If
            '' Completed by vivekpatel
        Next dr

        GetDiplayDetails = True

    End Function

#End Region

#Region "Bind Grid"

    Private Sub BindGrid(ByVal grdName As String)
        Dim dsTemp As New DataSet
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim errStr As String = String.Empty
        Dim WStr_Scope As String = String.Empty
        Dim dt_analyticalphase As New DataTable

        Select Case grdName

            Case "gvwpnl0001" ' Client Request Phase
                Dim dv_ClientNameforClientRequest As DataView
                'To Get Where condition of ScopeVales( Project Type )
                If Not objCommon.GetScopeValueWithCondition(WStr_Scope) Then
                    Exit Sub
                End If
                WStr_Scope = WStr_Scope & " AND cProjectStatus = 'I' And vProjectTypeCode not in ('0016','0014') And cWorkspaceType = 'P' and vLocationCode='" + Me.ddlLocation.SelectedValue() + "' and vProjectNo Not like 'MV%' and vProjectNo Not like 'MD%' and dCreatedOn >= '01-Jan-2016'"

                If ddlProjectManager.SelectedIndex > 0 Then ''added by prayag
                    WStr_Scope += " AND iProjectManagerId =" + ddlProjectManager.SelectedValue
                End If

                WStr_Scope += "order by dCreatedOn desc"



                If objHelpDbTable.GetViewgetWorkspaceDetailForHdr(WStr_Scope, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsTemp, errStr) Then

                    gvwpnl0001.ShowFooter = False
                    'LblClientRequestProjects.Text = " " & "(" & dsTemp.Tables(0).Rows.Count & ")" & " "
                    dv_ClientNameforClientRequest = dsTemp.Tables(0).DefaultView.ToTable(True, "vClientName").DefaultView()
                    dv_ClientNameforClientRequest.Sort = "vClientName"
                    DdlClientnameforClientRequest.DataSource = dv_ClientNameforClientRequest.ToTable()
                    DdlClientnameforClientRequest.DataTextField = "vClientName"
                    DdlClientnameforClientRequest.DataBind()
                    DdlClientnameforClientRequest.Items.Insert(0, "Select Sponsor...")
                    'Added By Naimesh for showing Header only
                    If dsTemp.Tables(0).Rows.Count < 1 Then
                        gvwpnl0001.DataSource = Nothing
                        gvwpnl0001.EmptyDataText = "No Data Found"
                        gvwpnl0001.DataBind()
                    Else
                        gvwpnl0001.DataSource = dsTemp
                        gvwpnl0001.DataBind()
                    End If

                    '****************************************

                    ViewState("VS_GvwPnl00001") = dsTemp.Tables(0)
                    ViewState("Vs_CommonForPageIndexChangingForClientRequest") = dsTemp.Tables(0)
                Else
                    objCommon.ShowAlert("Cannot fetch data for Panel 1", Me)
                End If
                'End If
            Case "gvwpnl0013" ' Pre-clinical Phase
                Me.TxtProjectRequestIdProjectPreClinical.Text = ""
                Dim dv_ClientNameforClientRequest As DataView
                'To Get Where condition of ScopeVales( Project Type )
                If Not objCommon.GetScopeValueWithCondition(WStr_Scope) Then
                    Exit Sub
                End If
                WStr_Scope = WStr_Scope & " AND cProjectStatus = 'P'  And vProjectTypeCode not in ('0016','0014') And cWorkspaceType = 'P'  and vLocationCode='" + Me.ddlLocation.SelectedValue() + "'"
                WStr_Scope += "order by dCreatedOn desc"

                If objHelpDbTable.GetViewgetWorkspaceDetailForHdr(WStr_Scope, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsTemp, errStr) Then

                    gvwpnl0013.ShowFooter = False
                    dv_ClientNameforClientRequest = dsTemp.Tables(0).DefaultView.ToTable(True, "vClientName").DefaultView()
                    dv_ClientNameforClientRequest.Sort = "vClientName"
                    DdlClientnameforProjectPreClinical.DataSource = dv_ClientNameforClientRequest.ToTable()
                    DdlClientnameforProjectPreClinical.DataTextField = "vClientName"
                    DdlClientnameforProjectPreClinical.DataBind()
                    DdlClientnameforProjectPreClinical.Items.Insert(0, "Select Sponsor")

                    If dsTemp.Tables(0).Rows.Count < 1 Then
                        gvwpnl0013.DataSource = Nothing
                        gvwpnl0013.EmptyDataText = "No Data Found"
                        gvwpnl0013.DataBind()
                    Else
                        gvwpnl0013.DataSource = dsTemp
                        gvwpnl0013.DataBind()
                    End If
                    ViewState("VS_GvwPnl00013") = dsTemp.Tables(0)
                    ViewState("Vs_CommonForPageIndexChangingForPreClinical") = dsTemp.Tables(0)
                Else
                    objCommon.ShowAlert("Cannot fetch data for Preclinical projects", Me)
                End If
                'End If
            Case "gvwpnl0003" 'Clinical Phase

                Dim dv_ClientNameforClinicalphase As DataView
                dsTemp = New DataSet

                'To Get Where condition of ScopeVales( Project Type )
                If Not objCommon.GetScopeValueWithCondition(WStr_Scope) Then
                    Exit Sub
                End If

                'WStr_Scope = WStr_Scope & " AND cProjectStatus = 'S'  And vProjectTypeCode <> '0016' "   commented By Mrunal Parekh for study completed phase
                WStr_Scope = WStr_Scope & " AND cProjectStatus = 'S' And vProjectTypeCode not in ('0016','0014') And cWorkspaceType = 'P' and vLocationCode='" + Me.ddlLocation.SelectedValue() + "' "
                If ddlProjectManager.SelectedIndex > 0 Then
                    WStr_Scope += " AND iProjectManagerId =" + ddlProjectManager.SelectedValue
                End If
                WStr_Scope += "order by dCreatedOn desc"

                If objHelpDbTable.View_getClinicalPhaseProjects(WStr_Scope, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsTemp, errStr) Then
                    gvwpnl0003.ShowFooter = False
                    dv_ClientNameforClinicalphase = dsTemp.Tables(0).DefaultView.ToTable(True, "vClientName").DefaultView()
                    dv_ClientNameforClinicalphase.Sort = "vClientName"
                    DdlCLientNameForCliniclaphase.DataSource = dv_ClientNameforClinicalphase.ToTable()
                    DdlCLientNameForCliniclaphase.DataTextField = "vClientName"
                    DdlCLientNameForCliniclaphase.DataBind()
                    DdlCLientNameForCliniclaphase.Items.Insert(0, "Select Sponsor...")
                    If dsTemp.Tables(0).Rows.Count < 1 Then
                        gvwpnl0003.DataSource = Nothing
                        gvwpnl0003.EmptyDataText = "No Data Found"
                        gvwpnl0003.DataBind()
                    Else
                        gvwpnl0003.DataSource = dsTemp.Tables(0)
                        gvwpnl0003.DataBind()
                    End If
                    ViewState("VS_GvwPnl00003") = dsTemp.Tables(0)
                    ViewState("Vs_CommonForPageIndexChangingForClinicalPhase") = dsTemp.Tables(0)

                Else
                    objCommon.ShowAlert("Cannot fetch data for Panel 3", Me)
                End If


            Case "gvwpnl0004"

                dsTemp = New DataSet ' Document Phase
                Dim dv_ClientNameforDocumentphase As DataView

                'To Get Where condition of ScopeVales( Project Type )
                If Not objCommon.GetScopeValueWithCondition(WStr_Scope) Then
                    Exit Sub
                End If

                WStr_Scope = WStr_Scope & " AND cProjectStatus = 'D'  And vProjectTypeCode not in ('0016','0014') And cWorkspaceType = 'P' and vLocationCode='" + Me.ddlLocation.SelectedValue() + " '"
                If (ddlProjectManager.SelectedIndex > 0) Then
                    WStr_Scope += " AND iProjectManagerId =" + ddlProjectManager.SelectedValue
                End If
                WStr_Scope += " order by dCreatedOn desc"

                If objHelpDbTable.GetViewgetWorkspaceDetailForHdr(WStr_Scope, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsTemp, errStr) Then

                    gvwpnl0004.ShowFooter = False
                    'LblDocumentsPhase.Text = " " & "(" & dsTemp.Tables(0).Rows.Count & ")" & " "
                    dv_ClientNameforDocumentphase = dsTemp.Tables(0).DefaultView.ToTable(True, "vClientName").DefaultView()
                    dv_ClientNameforDocumentphase.Sort = "vClientName"
                    DdllstForDocumentPhase.DataSource = dv_ClientNameforDocumentphase.ToTable()
                    DdllstForDocumentPhase.DataTextField = "vClientName"
                    DdllstForDocumentPhase.DataBind()
                    DdllstForDocumentPhase.Items.Insert(0, "Select Sponsor...")
                    'Added By Naimesh for showing Header only
                    If dsTemp.Tables(0).Rows.Count < 1 Then
                        gvwpnl0004.DataSource = Nothing
                        gvwpnl0004.EmptyDataText = "No Data Found"
                        gvwpnl0004.DataBind()
                    Else
                        gvwpnl0004.DataSource = dsTemp
                        gvwpnl0004.DataBind()
                    End If

                    '****************************************

                    ViewState("VS_GvwPnl00004") = dsTemp.Tables(0)
                    ViewState("Vs_CommonForPageIndexChangingForDocumentPhase") = dsTemp.Tables(0)
                    'dsTemp.Dispose()

                Else
                    objCommon.ShowAlert("Cannot fetch data for Panel ", Me)
                End If

                'End If

            Case "gvwpnl_Analysis"

                Dim dv_ClientNameforAnalyticalPhase As DataView
                dsTemp = New DataSet
                'Wstr = "(cProjectStatus = 'L' or cProjectStatus = 'Y')  And vProjectTypeCode <> '0016' order by dCreatedOn desc"
                Wstr = "cProjectStatus = 'L' And vProjectTypeCode not in ('0016','0014') And cWorkspaceType = 'P'  and vLocationCode='" + Me.ddlLocation.SelectedValue() + "'"
                If ddlProjectManager.SelectedIndex > 0 Then
                    Wstr += " AND iProjectManagerId =" + ddlProjectManager.SelectedValue
                End If
                Wstr += " order by dCreatedOn desc"
                If Not objHelpDbTable.view_getanalyticalprojects(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsTemp, eStr_Retu) Then
                    Me.objCommon.ShowAlert("Error While Getting Data Of analytical phase", Me.Page)
                End If

                If DdlCLientNameForAnalyticalPhase.SelectedIndex > 0 Then
                    dv_ClientNameforAnalyticalPhase = dsTemp.Tables(0).DefaultView
                    dv_ClientNameforAnalyticalPhase.RowFilter = "vClientName = '" + DdlCLientNameForAnalyticalPhase.SelectedItem.ToString.Trim() + "'"
                    gvwpnl_Analysis.DataSource = dv_ClientNameforAnalyticalPhase.ToTable
                    gvwpnl_Analysis.DataBind()
                    ViewState("VS_gvwpnl_Analysis") = dsTemp.Tables(0)

                Else
                    'LblAnalyticalPhase.Text = " " & "(" & dsTemp.Tables(0).Rows.Count & ")" & " "
                    Me.gvwpnl_Analysis.ShowFooter = False
                    Me.gvwpnl_Analysis.DataSource = dsTemp.Tables(0)
                    Me.gvwpnl_Analysis.DataBind()
                    ViewState("VS_gvwpnl_Analysis") = dsTemp.Tables(0)
                    ViewState("Vs_CommonForPageIndexChangingForAnalyticalPhase") = dsTemp.Tables(0)
                    'Ddlstatusforanlyticalphase.SelectedIndex = 0

                    dv_ClientNameforAnalyticalPhase = dsTemp.Tables(0).DefaultView.ToTable(True, "vClientName").DefaultView()
                    dv_ClientNameforAnalyticalPhase.Sort = "vClientName"
                    DdlCLientNameForAnalyticalPhase.DataSource = dv_ClientNameforAnalyticalPhase.ToTable()
                    DdlCLientNameForAnalyticalPhase.DataTextField = "vClientName"
                    DdlCLientNameForAnalyticalPhase.DataBind()
                    DdlCLientNameForAnalyticalPhase.Items.Insert(0, "Select Sponsor")
                End If

            Case Else
                'objCommon.ShowAlert("Invalid Panel Name", Me)
        End Select

    End Sub

#End Region

#Region "Export Button"
    Protected Sub btnExportToExcelKPI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportToExcelKPI.Click

        Dim fileName As String = ""
        Dim isReportComplete As Boolean = False
        Dim style As String = "<style>.text{mso-number-format:\@;}</style>"
        Try
            If GrdvgiewOfOperationalKpi.Rows.Count > 0 Then
                Dim Info As String = String.Empty
                Dim gridViewhtml As String = String.Empty
                fileName = dialogModalTitle.Text + Date.Today.ToString("dd-MMM-yyyy") + ".xls"
                isReportComplete = True

                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                GrdvgiewOfOperationalKpi.Columns(0).Visible = False
                GrdvgiewOfOperationalKpi.ShowFooter = False
                GrdvgiewOfOperationalKpi.RenderControl(writer)
                gridViewhtml = stringWriter.ToString()
                gridViewhtml = "<table><tr><td align = ""center"" colspan=""6"" ><font color=""#000099"" size=""4"" face=""Verdana""><b>" + System.Configuration.ConfigurationManager.AppSettings("Client").ToString() + "<br/></b></td></font></tr></table> <table><tr><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Date.Today.ToString("dd-MMM-yyyy") + "</b></font></td><td></td><td></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr></table>" + gridViewhtml

                Context.Response.Buffer = True
                Context.Response.ClearContent()
                Context.Response.ClearHeaders()

                Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
                Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)
                Context.Response.AddHeader("Content-Length", gridViewhtml.Length)

                Context.Response.Write(style)
                Context.Response.Write(gridViewhtml)
                Context.Response.Flush()
                Context.Response.End()

                System.IO.File.Delete(fileName)
                GrdvgiewOfOperationalKpi.Columns(0).Visible = True
                GrdvgiewOfOperationalKpi.ShowFooter = True
            Else
                Exit Sub
            End If

        Catch Threadex As Threading.ThreadAbortException
        Catch ex As Exception
            isReportComplete = False
        Finally
            If Not rPage Is Nothing Then
                rPage.CloseReport()
                rPage = Nothing
            End If
        End Try

    End Sub

    Protected Sub BtnExportToExcelForClientRequest_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds_Field As New DataSet
        Dim fileName As String = String.Empty
        Dim dt_Final As New DataTable
        Try
            dt_Final = CType(ViewState("VS_GvwPnl00001"), DataTable)
            ViewState("IdOfBtn") = "BtnExportToExcelForClientRequest"
            fileName = "Client Request Project Details-" + Date.Today.ToString("dd-MMM-yyyy") + ".xls"
            ds_Field.Tables.Add(dt_Final)
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
            Context.Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName)

            Context.Response.Write(ConvertDsuserTO(ds_Field))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub BtnExportToExcelForProjectPreClinical_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportToExcelForProjectPreClinical.Click
        Dim ds_Field As New DataSet
        Dim fileName As String = String.Empty
        Dim dt_Final As New DataTable
        Try
            ViewState("IdOfBtn") = "BtnExportToExcelForProjectPreClinical"
            fileName = "Pre-Clinical Project Details-" + Date.Today.ToString("dd-MMM-yyyy") + ".xls"
            dt_Final = CType(ViewState("VS_GvwPnl00013"), DataTable)
            ds_Field.Tables.Add(dt_Final)
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            Context.Response.Write(ConvertDsuserTO(ds_Field))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception

        End Try

    End Sub

    Protected Sub BtnExportToExcelForClinicalPhase_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds_Field As New DataSet
        Dim fileName As String = String.Empty
        Dim dt_Final As New DataTable
        Try
            ViewState("IdOfBtn") = "BtnExportToExcelForClinicalPhase"
            fileName = "Clinical Phase Project Details-" + Date.Today.ToString("dd-MMM-yyyy") + ".xls"
            dt_Final = CType(ViewState("VS_GvwPnl00003"), DataTable)
            ds_Field.Tables.Add(dt_Final)
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            Context.Response.Write(ConvertDsuserTO(ds_Field))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub BtnExportToExcelForAnalyticalPhase_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportToExcelForAnalyticalPhase.Click
        Dim ds_Field As New DataSet
        Dim fileName As String = String.Empty
        Dim dt_Final As New DataTable
        Try
            ViewState("IdOfBtn") = "BtnExportToExcelForAnalyticalPhase"
            fileName = "Analytical Phase Project Details-" + Date.Today.ToString("dd-MMM-yyyy") + ".xls"
            dt_Final = CType(ViewState("VS_gvwpnl_Analysis"), DataTable)
            ds_Field.Tables.Add(dt_Final)
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            Context.Response.Write(ConvertDsuserTO(ds_Field))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception

        End Try

    End Sub

    Protected Sub BtnExportToExcelForDocumentPhase_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportToExcelForDocumentPhase.Click
        Dim ds_Field As New DataSet
        Dim fileName As String = String.Empty
        Dim dt_Final As New DataTable
        Try
            ViewState("IdOfBtn") = "BtnExportToExcelForDocumentPhase"
            fileName = "Document Phase Project Details-" + Date.Today.ToString("dd-MMM-yyyy") + ".xls"
            dt_Final = CType(ViewState("VS_GvwPnl00004"), DataTable)
            ds_Field.Tables.Add(dt_Final)
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            Context.Response.Write(ConvertDsuserTO(ds_Field))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub BtnExportToExcelForProjectStudyWorkSummary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportToExcelForProjectStudyWorkSummary.Click

        Dim ds_Field As New DataSet
        Dim fileName As String = String.Empty
        Dim dt_Final As New DataTable
        Try
            ViewState("IdOfBtn") = "BtnExportToExcelForProjectStudyWorkSummary"
            fileName = "Project Study Work Summary Details-" + Date.Today.ToString("dd-MMM-yyyy") + ".xls"
            dt_Final = CType(ViewState("VS_GvwPnlProjectStudyWorkSummary_PageIndexChanging"), DataTable)
            ds_Field.Tables.Add(dt_Final)
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            Context.Response.Write(ConvertDsuserTO(ds_Field))
            pnlChartForWorkSummary1.Visible = False
            Context.Response.Flush()
            Context.Response.End()
            System.IO.File.Delete(fileName)




        Catch ex As Exception

        End Try

    End Sub

    Protected Sub btnExportDataStatus_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportDataStatus.Click
        Dim ds_Field As New DataSet
        Dim fileName As String = String.Empty
        Dim dt_Final As New DataTable
        Dim vProjectWorkSpaceId As String
        Dim isChild As String = "N"
        Try
            ViewState("IdOfBtn") = "btnExportDataStatus"
            fileName = "Data Status-" + Date.Today.ToString("dd-MMM-yyyy") + ".xls"

            If chkCRFDataStatusParentProject.Checked = True Then
                isChild = "Y"
            End If

            vProjectWorkSpaceId = ProjectWorkSpaceIdDemo.Value.ToString().Trim()

            If Not objHelpDbTable.Proc_GetActivityStatusCountRecords(vProjectWorkSpaceId, "", isChild, 2, "N", ds_Field, eStr_Retu) Then
                Throw New Exception("Error while getting data from Proc_GetActivityStatusCountRecords " + eStr_Retu.Trim)
                Exit Sub
            End If







            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)
            Context.Response.Write(ConvertDsuserTO(ds_Field))
            Context.Response.Flush()
            Context.Response.End()
            System.IO.File.Delete(fileName)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Protected Sub btnExportQueryMgt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportQueryMgt.Click
        Dim ds_Field As New DataSet
        Dim fileName As String = String.Empty
        Dim dt_Final As New DataTable
        Dim vProjectWorkSpaceId As String
        Dim isChild As String = "N"
        Try
            ViewState("IdOfBtn") = "btnExportQueryMgt"
            fileName = "" + ddlColumnList3.SelectedValue + "-" + Date.Today.ToString("dd-MMM-yyyy") + ".xls"

            If chkCRFDataStatusParentProject.Checked = True Then
                isChild = "Y"
            End If

            vProjectWorkSpaceId = ProjectWorkSpaceIdDCFManage.Value.ToString().Trim()

            If Not objHelpDbTable.Proc_DCFTrackingReport(vProjectWorkSpaceId, ddlColumnList3.SelectedValue, ds_Field, eStr_Retu) Then
                Throw New Exception("Error while getting data from Proc_GetActivityStatusCountRecords " + eStr_Retu.Trim)
                Exit Sub
            End If

            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)
            Context.Response.Write(ConvertDsuserTO(ds_Field))
            Context.Response.Flush()
            Context.Response.End()
            System.IO.File.Delete(fileName)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub
    '' Added By dipen Shah.
    Protected Sub btnExportToExcelForScreeningSubjectInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcelForScreeningSubjectInfo.Click
        Dim ds_Field As New DataSet
        Dim fileName As String = String.Empty
        Dim dt_Final As New DataTable
        Try
            ViewState("IdOfBtn") = "btnExportToExcelForScreeningSubjectInfo"
            fileName = "Subject Screening Details-" + Date.Today.ToString("dd-MMM-yyyy") + ".xls"
            dt_Final = CType(ViewState("VS_grvSubjectInfo"), DataTable)
            ds_Field.Tables.Add(dt_Final)
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)
            Context.Response.Write(ConvertDsuserTO(ds_Field))
            Context.Response.Flush()
            Context.Response.End()
            System.IO.File.Delete(fileName)

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnExportSiteWiseData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportSiteWiseData.Click
        Dim ds_Field As New DataSet
        Dim fileName As String = String.Empty
        Dim dt_Final As New DataTable
        Dim vProjectWorkSpaceId As String
        Dim isChild As String = "N"
        Try
            ViewState("IdOfBtn") = "btnExportSiteWiseData"
            fileName = "SiteWiseDataStatus-" + Date.Today.ToString("dd-MMM-yyyy") + ".xls"

            If chkSiteWiseParentProject.Checked = True Then
                isChild = "Y"
            End If

            vProjectWorkSpaceId = ProjectWorkSpaceIdDemo1.Value.ToString().Trim()

            If Not objHelpDbTable.Proc_GetActivityStatusCountRecords(vProjectWorkSpaceId, "", isChild, 2, "N", ds_Field, eStr_Retu) Then
                Throw New Exception("Error while getting data from Proc_GetActivityStatusCountRecords " + eStr_Retu.Trim)
                Exit Sub
            End If


            ds_Field.Tables(0).Columns.Add("Total Expected Data", GetType(Integer), "[Data Entry Pending]+[Data Entry Continue]+[Ready For Review]+[First Review Done]+[Second Review Done]")
            ds_Field.Tables(0).Columns.Add("Available Data", GetType(Integer), " [Data Entry Continue]+[Ready For Review]+[First Review Done]+[Second Review Done]")
            ds_Field.Tables(0).Columns.Add("SDV Data", GetType(Integer), "[First Review Done]+[Second Review Done]")   '' AS discessed with Lambda Team Remove Ready For Review Sum
            ds_Field.Tables(0).Columns.Add("DM Reviwed", GetType(Integer), "[Second Review Done]")

            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)
            Context.Response.Write(ConvertDsuserTO(ds_Field))
            Context.Response.Flush()
            Context.Response.End()
            System.IO.File.Delete(fileName)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub
#End Region

#Region "Export to excel"

    Private Function ConvertDsuserTO(ByVal ds As DataSet) As String
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer
        Dim dsConvert As New DataSet
        Dim selecetedmonth As String

        Try
            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            Select Case ViewState("IdOfBtn")
                Case "BtnExportToExcelForClientRequest"
                    strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<tr><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Date.Today.ToString("dd-MMM-yyyy") + "</b></font></td><td></td><td></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")
                Case "BtnExportToExcelForProjectPreClinical"
                    strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<tr><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Date.Today.ToString("dd-MMM-yyyy") + "</b></font></td><td></td><td></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")
                Case "BtnExportToExcelForClinicalPhase"
                    strMessage.Append("<td colspan=""3""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<tr><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Print Date:-</b></font><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Date.Today.ToString("dd-MMM-yyyy") + "</b></font></td><td></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Printed By:-</b></font><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")
                Case "BtnExportToExcelForAnalyticalPhase"
                    strMessage.Append("<td colspan=""5""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<tr><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Date.Today.ToString("dd-MMM-yyyy") + "</b></font></td><td></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")
                Case "BtnExportToExcelForDocumentPhase"
                    strMessage.Append("<td colspan=""4""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<tr><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Date.Today.ToString("dd-MMM-yyyy") + "</b></font></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")
                Case "BtnExportToExcelForProjectStudyWorkSummary"
                    strMessage.Append("<td colspan=""12""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<tr><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Date.Today.ToString("dd-MMM-yyyy") + "</b></font></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")
                Case "btnExportToExcelForScreeningSubjectInfo"
                    strMessage.Append("<td colspan=""4""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")

                Case "btnExportDataStatus"
                    strMessage.Append("<td colspan=""11""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")
                Case "btnExportSiteWiseData"
                    strMessage.Append("<td colspan=""5""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<tr><td align = ""right""><font color=""#000099"" size=""3"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Date.Today.ToString("dd-MMM-yyyy") + "</b></font></td><td></td><td align = ""right""><font color=""#000099"" size=""3"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")
                Case "btnExportQueryMgt"

                    If (ddlColumnList3.SelectedValue = "DCF STATUS" Or ddlColumnList3.SelectedValue = "TYPES OF DCF" Or ddlColumnList3.SelectedValue = "System Generated" Or ddlColumnList3.SelectedValue = "Manually Generated") Then
                        strMessage.Append("<td colspan=""3""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                        strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                        strMessage.Append("</font></strong><center></b></td>")
                        strMessage.Append("</tr>")
                        strMessage.Append("<tr><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Print Date:- <b>" + Date.Today.ToString("dd-MMM-yyyy") + "</b></font></td><td></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Printed By:-" + Session(S_LoginName) + "</b></font></td></tr>")
                    ElseIf (ddlColumnList3.SelectedValue = "ACTIVITY WISE" Or ddlColumnList3.SelectedValue = "Query By Site" Or ddlColumnList3.SelectedValue = "Query Per Subject") Then
                        strMessage.Append("<td colspan=""2""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                        strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                        strMessage.Append("</font></strong><center></b></td>")
                        strMessage.Append("</tr>")
                        strMessage.Append("<tr><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Print Date:- <b>" + Date.Today.ToString("dd-MMM-yyyy") + "</b></font></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Printed By:-" + Session(S_LoginName) + "</b></font></td></tr>")
                    ElseIf (ddlColumnList3.SelectedValue = "FULL CHART") Then
                        strMessage.Append("<td colspan=""5""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                        strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                        strMessage.Append("</font></strong><center></b></td>")
                        strMessage.Append("</tr>")
                        strMessage.Append("<tr><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Print Date:- <b>" + Date.Today.ToString("dd-MMM-yyyy") + "</b></font></td><td></td><td></td><td></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Printed By:-" + Session(S_LoginName) + "</b></font></td></tr>")
                    End If




            End Select




            If ViewState("IdOfBtn") = "btnExportToExcelForScreeningSubjectInfo" Then
                strMessage.Append("<tr>")
                strMessage.Append("<td align=""left"" colspan=""2""><center><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                'strMessage.Append(DdllistMonth.SelectedItem + DdllistYear.SelectedValue.ToString)
                selecetedmonth = DdllistMonth.SelectedItem.ToString().Trim() & "-" & DdllistYear.SelectedItem.ToString().Trim()
                strMessage.Append(selecetedmonth.ToString())
                strMessage.Append("</font></strong><center></td>")
                strMessage.Append("<td align=""left"" colspan=""2""><center><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Generic Screening Subject Information")
                strMessage.Append("</font></strong><center></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""4""><center><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(lbltask.Text.ToString())
                strMessage.Append("</font></strong><center></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Date.Today.ToString("dd-MMM-yyyy") + "</b></font></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")
                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""4""><center><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("</font></strong><center></td>")
                strMessage.Append("</tr>")
            Else
                strMessage.Append("<tr>")
                strMessage.Append("</tr>")
            End If
            strMessage.Append("<tr>")
            If ViewState("IdOfBtn") = "BtnExportToExcelForClientRequest" Or ViewState("IdOfBtn") = "BtnExportToExcelForProjectPreClinical" Then
                dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vRequestId,vClientName,vDrugName,vRegionName,dCreatedOn,vProjectManager".Split(",")).DefaultView.ToTable())
                dsConvert.AcceptChanges()
                dsConvert.Tables(0).Columns(0).ColumnName = "Request Id"
                dsConvert.Tables(0).Columns(1).ColumnName = "Sponsor Name"
                dsConvert.Tables(0).Columns(2).ColumnName = "Drug Name"
                dsConvert.Tables(0).Columns(3).ColumnName = "Submission"
                dsConvert.Tables(0).Columns(4).ColumnName = "Request Date"
                dsConvert.Tables(0).Columns(5).ColumnName = "Project Manager"
                dsConvert.AcceptChanges()
            ElseIf ViewState("IdOfBtn") = "BtnExportToExcelForClinicalPhase" Then
                dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vProjectNo,vDrugName,iNoOfSubjects".Split(",")).DefaultView.ToTable())
                dsConvert.AcceptChanges()
                dsConvert.Tables(0).Columns(0).ColumnName = "Project No"
                dsConvert.Tables(0).Columns(1).ColumnName = "Drug Name"
                dsConvert.Tables(0).Columns(2).ColumnName = "No Of Subjects"
                dsConvert.AcceptChanges()
            ElseIf ViewState("IdOfBtn") = "BtnExportToExcelForAnalyticalPhase" Then
                'dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vProjectNo,vDrugName,iNoOfSubjects,NoOfTimePoints".Split(",")).DefaultView.ToTable())
                dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vProjectNo,vDrugName,iNoOfSubjects,vClientName,NoOfTimePoints".Split(",")).DefaultView.ToTable()) '' vClientName added by prayag
                dsConvert.AcceptChanges()
                dsConvert.Tables(0).Columns(0).ColumnName = "Project No"
                dsConvert.Tables(0).Columns(1).ColumnName = "Drug Name"
                dsConvert.Tables(0).Columns(2).ColumnName = "No Of Subjects"
                dsConvert.Tables(0).Columns(3).ColumnName = "Sponsor Name" '' added by prayag
                dsConvert.Tables(0).Columns(4).ColumnName = "No Of  Time Points"
                dsConvert.AcceptChanges()
            ElseIf ViewState("IdOfBtn") = "BtnExportToExcelForDocumentPhase" Then
                dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vProjectNo,vDrugName,vClientName,iNoOfSubjects".Split(",")).DefaultView.ToTable())  '' vClientName added by prayag
                dsConvert.AcceptChanges()
                dsConvert.Tables(0).Columns(0).ColumnName = "Project No"
                dsConvert.Tables(0).Columns(1).ColumnName = "Drug Name"
                dsConvert.Tables(0).Columns(2).ColumnName = "Sponsor Name" '' added by prayag
                dsConvert.Tables(0).Columns(3).ColumnName = "No Of Subjects"
                dsConvert.AcceptChanges()
            ElseIf ViewState("IdOfBtn") = "BtnExportToExcelForProjectStudyWorkSummary" Then
                dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vProjectNo,vClientName,vRegionName,vProjectTypeName,iNoOfSubjects,cPermissionRequired,ClinicEndDate,SampleAnalysisEndDate,SponsorReportDate,ReportDispatchDate,vDrugName,vProjectManager".Split(",")).DefaultView.ToTable()) '' added vDrugName,vProjectManager
                dsConvert.AcceptChanges()
                dsConvert.Tables(0).Columns(0).ColumnName = "Project No"
                dsConvert.Tables(0).Columns(1).ColumnName = "Sponsor"
                dsConvert.Tables(0).Columns(2).ColumnName = "Regulatory Submission"
                dsConvert.Tables(0).Columns(3).ColumnName = "Project Type"
                dsConvert.Tables(0).Columns(4).ColumnName = "No Of Subjects"
                dsConvert.Tables(0).Columns(5).ColumnName = "DCGI Approval"
                dsConvert.Tables(0).Columns(6).ColumnName = "Clinical End Date"
                dsConvert.Tables(0).Columns(7).ColumnName = "Sample Analysis End Date"
                dsConvert.Tables(0).Columns(8).ColumnName = "Report Sent To Sponsor Date"
                dsConvert.Tables(0).Columns(9).ColumnName = "Report Dispatch End Date"
                dsConvert.Tables(0).Columns(10).ColumnName = "Drug Name"
                dsConvert.Tables(0).Columns(11).ColumnName = "Project Manager"
                dsConvert.AcceptChanges()
                '' Added By Dipen Shah To view subject information from ScreeningAnalytics
            ElseIf ViewState("IdOfBtn") = "btnExportToExcelForScreeningSubjectInfo" Then
                If ds.Tables(0).Columns.Contains("vProjectNo") Then  '' added by prayag
                    dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vProjectNo,vSubjectId,vInitials,vSubjectName,dScreenDate".Split(",")).DefaultView.ToTable())
                    dsConvert.AcceptChanges()
                    dsConvert.Tables(0).Columns(0).ColumnName = "Project No"
                    dsConvert.Tables(0).Columns(1).ColumnName = "Subject Id"
                    dsConvert.Tables(0).Columns(2).ColumnName = "Initials"
                    dsConvert.Tables(0).Columns(3).ColumnName = "Subject Name"
                    dsConvert.Tables(0).Columns(4).ColumnName = "Screening Date"
                Else
                    dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vSubjectId,vInitials,vSubjectName,dScreenDate".Split(",")).DefaultView.ToTable())
                    dsConvert.AcceptChanges()
                    dsConvert.Tables(0).Columns(0).ColumnName = "Subject Id"
                    dsConvert.Tables(0).Columns(1).ColumnName = "Initials"
                    dsConvert.Tables(0).Columns(2).ColumnName = "Subject Name"
                    dsConvert.Tables(0).Columns(3).ColumnName = "Screening Date"
                End If
            ElseIf ViewState("IdOfBtn") = "btnExportDataStatus" Then
                dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "Project/Site Id.,No. Of Subject,Data Entry Pending,Data Entry Continue,Ready For Review,First Review Done,Second Review Done,Final Reviewed & Freeze,Generated DCF,Answered DCF,Total DCF".Split(",")).DefaultView.ToTable())
                dsConvert.AcceptChanges()
                dsConvert.Tables(0).Columns(0).ColumnName = "Project/Site Id."
                dsConvert.Tables(0).Columns(1).ColumnName = "No. Of Subject"
                dsConvert.Tables(0).Columns(2).ColumnName = "Data Entry Pending"
                dsConvert.Tables(0).Columns(3).ColumnName = "Data Entry Continue"
                dsConvert.Tables(0).Columns(4).ColumnName = "Ready For Review"
                dsConvert.Tables(0).Columns(5).ColumnName = "First Review Done"
                dsConvert.Tables(0).Columns(6).ColumnName = "Second Review Done"
                dsConvert.Tables(0).Columns(7).ColumnName = "Final Reviewed & Freeze"
                dsConvert.Tables(0).Columns(8).ColumnName = "Generated DCF"
                dsConvert.Tables(0).Columns(9).ColumnName = "Answered DCF"
                dsConvert.Tables(0).Columns(10).ColumnName = "Total DCF"
                dsConvert.AcceptChanges()
            ElseIf ViewState("IdOfBtn") = "btnExportSiteWiseData" Then
                dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "Project/Site Id.,Total Expected Data,Available Data,SDV Data,Second Review Done".Split(",")).DefaultView.ToTable())
                dsConvert.AcceptChanges()
                dsConvert.Tables(0).Columns(0).ColumnName = "Project/Site Id."
                dsConvert.Tables(0).Columns(1).ColumnName = "Total Expected Data"
                dsConvert.Tables(0).Columns(2).ColumnName = "Available Data"
                dsConvert.Tables(0).Columns(3).ColumnName = "SDV Data"
                dsConvert.Tables(0).Columns(4).ColumnName = "Second Review Done"
                dsConvert.AcceptChanges()
            ElseIf ViewState("IdOfBtn") = "btnExportQueryMgt" Then

                If (ddlColumnList3.SelectedValue = "DCF STATUS") Then
                    dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "ProjectNo,DCFCount,cDCFStatus".Split(",")).DefaultView.ToTable())
                    dsConvert.AcceptChanges()
                    dsConvert.Tables(0).Columns(0).ColumnName = "Project"
                    dsConvert.Tables(0).Columns(1).ColumnName = "Total Count"
                    dsConvert.Tables(0).Columns(2).ColumnName = "DCF Status"

                ElseIf (ddlColumnList3.SelectedValue = "TYPES OF DCF") Then
                    dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "ProjectNo,DCFCount,DCFType".Split(",")).DefaultView.ToTable())
                    dsConvert.AcceptChanges()
                    dsConvert.Tables(0).Columns(0).ColumnName = "Project No"
                    dsConvert.Tables(0).Columns(1).ColumnName = "Query Count"
                    dsConvert.Tables(0).Columns(2).ColumnName = "DCF Type"

                ElseIf (ddlColumnList3.SelectedValue = "ACTIVITY WISE") Then
                    dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vActivityName,DCFCount".Split(",")).DefaultView.ToTable())
                    dsConvert.AcceptChanges()
                    dsConvert.Tables(0).Columns(0).ColumnName = "Activity Name"
                    dsConvert.Tables(0).Columns(1).ColumnName = "Total Count"

                ElseIf (ddlColumnList3.SelectedValue = "System Generated" Or ddlColumnList3.SelectedValue = "Manually Generated") Then

                    dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vProjectNo,Type,QueryCount".Split(",")).DefaultView.ToTable())
                    dsConvert.AcceptChanges()
                    dsConvert.Tables(0).Columns(0).ColumnName = "Project"
                    dsConvert.Tables(0).Columns(1).ColumnName = "Type"
                    dsConvert.Tables(0).Columns(2).ColumnName = "Total Count"

                ElseIf (ddlColumnList3.SelectedValue = "FULL CHART") Then
                    dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "Site,Days,QUERY GENRATED TO ANSWER,QUERY ANSWRED TO RESOLVED,QUERY GENRATED TO RESOLVED".Split(",")).DefaultView.ToTable())
                    dsConvert.AcceptChanges()
                    dsConvert.Tables(0).Columns(0).ColumnName = "Project"

                ElseIf (ddlColumnList3.SelectedValue.ToUpper() = "QUERY BY SITE") Then
                    dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vProjectNo,QueryCOUNT".Split(",")).DefaultView.ToTable())
                    dsConvert.AcceptChanges()
                    dsConvert.Tables(0).Columns(0).ColumnName = "Project"
                    dsConvert.Tables(0).Columns(1).ColumnName = "Query COUNT"

                ElseIf (ddlColumnList3.SelectedValue.ToUpper() = "QUERY PER SUBJECT") Then
                    dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vProjectNo,QueryCOUNT".Split(",")).DefaultView.ToTable())
                    dsConvert.AcceptChanges()
                    dsConvert.Tables(0).Columns(0).ColumnName = "Project"
                    dsConvert.Tables(0).Columns(1).ColumnName = "Count Per Subject"


                End If
                dsConvert.AcceptChanges()
            End If

            If ViewState("IdOfBtn") = "btnExportToExcelForScreeningSubjectInfo" Then
                For iCol = 0 To dsConvert.Tables(0).Columns.Count - 1

                    If dsConvert.Tables(0).Columns(0).ColumnName = "Subject Name" Then
                        strMessage.Append("<td bgcolor=""#1560a1""><strong><font color=""#FFFFFF"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                        strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).Trim())
                        strMessage.Append("</font></strong></td>")
                    Else
                        strMessage.Append("<td bgcolor=""#1560a1"" align=""left""><strong><font color=""#FFFFFF"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                        strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).Trim())
                        strMessage.Append("</font></strong></td>")
                    End If
                Next
            Else
                For iCol = 0 To dsConvert.Tables(0).Columns.Count - 1

                    strMessage.Append("<td bgcolor=""#1560a1""><strong><font color=""#FFFFFF"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).Trim())
                    strMessage.Append("</font></strong></td>")

                Next
            End If
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")

            For j = 0 To dsConvert.Tables(0).Rows.Count - 1
                strMessage.Append("<tr>")
                For i = 0 To dsConvert.Tables(0).Columns.Count - 1

                    If j Mod 2 = 0 Then
                        strMessage.Append("<td align=""left"" bgcolor=""84c8e6""><font color=""#191970"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                        strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim())
                        strMessage.Append("</font></td>")
                    Else
                        strMessage.Append("<td align=""left"" bgcolor=""#""><font color=""#191970"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                        strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim())
                        strMessage.Append("</font></td>")
                    End If


                Next
                strMessage.Append("</tr>")
            Next
            strMessage.Append("</table>")

            Return strMessage.ToString

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....ConvertDsuserTO")
            Return ""
        End Try
    End Function

#End Region

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), System.Web.UI.WebControls.Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), System.Web.UI.WebControls.Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub

#End Region

#Region "grid events"

#Region "gvwpnl0001"

    Protected Sub gvwpnl0001_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwpnl0001.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lnkBtnDetails"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkBtnDetails"), LinkButton).CommandName = "ProDtlForClientRequest"
            e.Row.Cells(gvwpnl0001_Details).HorizontalAlign = HorizontalAlign.Center
        End If
    End Sub

    Protected Sub gvwpnl0001_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwpnl0001.RowCommand
        Dim SelectedRowIndex As Integer
        Dim RedirectStr As String = ""
        Dim workSpaceId As String = ""
        SelectedRowIndex = e.CommandArgument
        If e.CommandName.ToUpper = "PRODTLFORCLIENTREQUEST" Then
            workSpaceId = Me.gvwpnl0001.Rows(SelectedRowIndex).Cells(gvwpnl0001_workspaceid).Text.ToString().Trim()
            RedirectStr = "window.open(""" + "frmProjectDetailMst.aspx?WorkSpaceId=" & workSpaceId & "&Page=frmMyProject&Type=MONITORING&ParentName=frmMainPage" + """)"
            ScriptManager.RegisterClientScriptBlock(Me.Page(), Me.GetType(), "OpenWindow", RedirectStr, True)
        End If

    End Sub

    Protected Sub gvwpnl0001_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwpnl0001.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or
            e.Row.RowType = DataControlRowType.Header Or
            e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(gvwpnl0001_workspaceid).Visible = False
        End If
    End Sub

    Protected Sub gvwpnl0001_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvwpnl0001.PageIndexChanging
        gvwpnl0001.PageIndex = e.NewPageIndex
        'BindGrid("gvwpnl0001")
        Dim dtTemp As New DataTable
        dtTemp = CType(Me.ViewState("Vs_CommonForPageIndexChangingForClientRequest"), DataTable)
        gvwpnl0001.DataSource = dtTemp
        gvwpnl0001.DataBind()
    End Sub

#End Region

#Region "gvwpnl0013"

    Protected Sub gvwpnl0013_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvwpnl0013.PageIndexChanging
        gvwpnl0013.PageIndex = e.NewPageIndex
        Dim dtTemp As New DataTable
        dtTemp = CType(Me.ViewState("Vs_CommonForPageIndexChangingForPreClinical"), DataTable)
        gvwpnl0013.DataSource = dtTemp
        gvwpnl0013.DataBind()
    End Sub
    Protected Sub gvwpnl0013_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwpnl0013.RowCommand
        Dim SelectedRowIndex As Integer
        Dim RedirectStr As String = ""
        Dim workSpaceId As String = ""
        SelectedRowIndex = e.CommandArgument
        If e.CommandName.ToUpper = "PRODTLFORPRECLINICAL" Then
            workSpaceId = Me.gvwpnl0013.Rows(SelectedRowIndex).Cells(gvwpnl0013_workspaceid).Text.ToString().Trim()

            RedirectStr = "window.open(""" + "frmProjectDetailMst.aspx?WorkSpaceId=" & workSpaceId & "&Page=frmMyProject&Type=MONITORING&ParentName=frmMainPage" + """)"

            ScriptManager.RegisterClientScriptBlock(Me.Page(), Me.GetType(), "OpenWindow", RedirectStr, True)
        End If
    End Sub

    Protected Sub gvwpnl0013_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwpnl0013.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or
        e.Row.RowType = DataControlRowType.Header Or
        e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(gvwpnl0013_workspaceid).Visible = False
        End If
    End Sub

    Protected Sub gvwpnl0013_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwpnl0013.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lnkBtnDetailsProjectPreClinical"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkBtnDetailsProjectPreClinical"), LinkButton).CommandName = "ProDtlForPreClinical"
            e.Row.Cells(gvwpnl0013_Details).HorizontalAlign = HorizontalAlign.Center
        End If
    End Sub

#End Region

#Region "gvwpnl0003"
    Protected Sub gvwpnl0003_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwpnl0003.RowCommand
        Dim SelectedRowIndex As Integer
        Dim RedirectStr As String = ""
        Dim workSpaceId As String = ""
        SelectedRowIndex = e.CommandArgument
        If e.CommandName.ToUpper = "PRODTLFORCLINICALPHASE" Then
            workSpaceId = Me.gvwpnl0003.Rows(SelectedRowIndex).Cells(gvwpnl0003_workspaceid).Text.ToString().Trim()
            RedirectStr = "window.open(""" + "frmProjectDetailMst.aspx?WorkSpaceId=" & workSpaceId & "&Page=frmMyProject&Type=MONITORING&ParentName=frmMainPage" + """)"
            ScriptManager.RegisterClientScriptBlock(Me.Page(), Me.GetType(), "OpenWindow", RedirectStr, True)
        End If
    End Sub

    Protected Sub gvwpnl0003_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwpnl0003.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or
              e.Row.RowType = DataControlRowType.Header Or
              e.Row.RowType = DataControlRowType.Footer Then
            'e.Row.Cells(gvwpnl0003_NoOfTimePoints).Visible = False ''commented by prayag for dashboard changes (show no of subject)
            e.Row.Cells(gvwpnl0003_workspaceid).Visible = False
            'e.Row.Cells(gvwpnl0003_NoOfTimePoints).Visible = False commented by prayag for dashboard changes
        End If
    End Sub

    Protected Sub gvwpnl0003_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwpnl0003.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            CType(e.Row.FindControl("lnkBtnDetailsForClinicalPhase"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkBtnDetailsForClinicalPhase"), LinkButton).CommandName = "ProDtlForClinicalPhase"
            e.Row.Cells(gvwpnl0003_Dateils).HorizontalAlign = HorizontalAlign.Center
        End If

    End Sub

    Protected Sub gvwpnl0003_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvwpnl0003.PageIndexChanging
        gvwpnl0003.PageIndex = e.NewPageIndex

        'BindGrid("gvwpnl0003")
        Dim dtTemp As New DataTable
        dtTemp = CType(Me.ViewState("Vs_CommonForPageIndexChangingForClinicalPhase"), DataTable)
        'dtTemp.DefaultView.Sort = 
        gvwpnl0003.DataSource = dtTemp
        gvwpnl0003.DataBind()
    End Sub

#End Region

#Region "gvwpnl_Analysis"

    Protected Sub gvwpnl_Analysis_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        gvwpnl_Analysis.PageIndex = e.NewPageIndex
        'BindGrid("gvwpnl_Analysis")
        Dim dtTemp As New DataTable
        dtTemp = CType(Me.ViewState("Vs_CommonForPageIndexChangingForAnalyticalPhase"), DataTable)
        gvwpnl_Analysis.DataSource = dtTemp
        gvwpnl_Analysis.DataBind()
    End Sub
    Protected Sub gvwpnl_Analysis_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwpnl_Analysis.RowCommand
        Dim SelectedRowIndex As Integer
        Dim RedirectStr As String = ""
        Dim workSpaceId As String = ""
        SelectedRowIndex = e.CommandArgument
        If e.CommandName.ToUpper = "PRODTLFORANALYTICALPHASE" Then
            workSpaceId = Me.gvwpnl_Analysis.Rows(SelectedRowIndex).Cells(gvwpnl_Analysis_workspaceid).Text.ToString().Trim()

            RedirectStr = "window.open(""" + "frmProjectDetailMst.aspx?WorkSpaceId=" & workSpaceId & "&Page=frmMyProject&Type=MONITORING&ParentName=frmMainPage" + """)"

            ScriptManager.RegisterClientScriptBlock(Me.Page(), Me.GetType(), "OpenWindow", RedirectStr, True)
        End If
    End Sub

    Protected Sub gvwpnl_Analysis_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwpnl_Analysis.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or
                      e.Row.RowType = DataControlRowType.Header Or
                      e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(gvwpnl_Analysis_workspaceid).Visible = False
            e.Row.Cells(gvwpnl0003_NoOfTimePoints).Visible = False
            e.Row.Cells(gvwpnl_Analysis_status).Style.Add("display", "none")
        End If
    End Sub

    Protected Sub gvwpnl_Analysis_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwpnl_Analysis.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            CType(e.Row.FindControl("lnkBtnDetailsForAnalyticalphase"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkBtnDetailsForAnalyticalphase"), LinkButton).CommandName = "ProDtlForAnalyticalPhase"
            e.Row.Cells(gvwpnl_Analysis_Details).HorizontalAlign = HorizontalAlign.Center
            ''===Added by Mrunal Parekh on 27-Jan-2012
            'If e.Row.Cells(gvwpnl_Analysis_status).Text = "Y" Then
            '    e.Row.BackColor = Color.LightGray
            'End If


        End If
    End Sub

#End Region

#Region "gvwpnl0004"

    Protected Sub gvwpnl0004_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvwpnl0004.PageIndexChanging
        gvwpnl0004.PageIndex = e.NewPageIndex
        'BindGrid("gvwpnl0004")
        Dim dtTemp As New DataTable
        dtTemp = CType(Me.ViewState("Vs_CommonForPageIndexChangingForDocumentPhase"), DataTable)
        gvwpnl0004.DataSource = dtTemp
        gvwpnl0004.DataBind()
    End Sub
    Protected Sub gvwpnl0004_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwpnl0004.RowCommand
        Dim SelectedRowIndex As Integer
        Dim RedirectStr As String = ""
        Dim workSpaceId As String = ""
        SelectedRowIndex = e.CommandArgument
        If e.CommandName.ToUpper = "PRODTLFORDOCUMENTPHASE" Then
            workSpaceId = Me.gvwpnl0004.Rows(SelectedRowIndex).Cells(gvwpnl0004_workspaceid).Text.ToString().Trim()

            RedirectStr = "window.open(""" + "frmProjectDetailMst.aspx?WorkSpaceId=" & workSpaceId & "&Page=frmMyProject&Type=MONITORING&ParentName=frmMainPage" + """)"

            ScriptManager.RegisterClientScriptBlock(Me.Page(), Me.GetType(), "OpenWindow", RedirectStr, True)
        End If
    End Sub

    Protected Sub gvwpnl0004_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwpnl0004.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or
                             e.Row.RowType = DataControlRowType.Header Or
                             e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(gvw_workspaceid).Visible = False
        End If
    End Sub

    Protected Sub gvwpnl0004_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwpnl0004.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lnkBtnDetailsForDocumentphase"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkBtnDetailsForDocumentphase"), LinkButton).CommandName = "ProDtlForDocumentPhase"
            e.Row.Cells(gvwpnl0004_Details).HorizontalAlign = HorizontalAlign.Center
        End If
    End Sub

#End Region

#Region "GvwPnlProjectStudyWorkSummary"

    Protected Sub GvwPnlProjectStudyWorkSummary_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GvwPnlProjectStudyWorkSummary.RowCommand
        Dim SelectedRowIndex As Integer
        Dim RedirectStr As String = ""
        Dim workSpaceId As String = ""
        SelectedRowIndex = e.CommandArgument
        If e.CommandName.ToUpper = "PRODTLFORWORKSUMMARY" Then
            workSpaceId = Me.GvwPnlProjectStudyWorkSummary.Rows(SelectedRowIndex).Cells(gvwpnWorkSummary_workspaceid).Text.ToString().Trim()

            RedirectStr = "window.open(""" + "frmProjectDetailMst.aspx?WorkSpaceId=" & workSpaceId & "&Page=frmMyProject&Type=MONITORING&ParentName=frmMainPage" + """)"

            ScriptManager.RegisterClientScriptBlock(Me.Page(), Me.GetType(), "OpenWindow", RedirectStr, True)
        End If
    End Sub

    Protected Sub GvwPnlProjectStudyWorkSummary_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvwPnlProjectStudyWorkSummary.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or
                           e.Row.RowType = DataControlRowType.Header Or
                           e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(gvwpnWorkSummary_workspaceid).Visible = False
        End If
    End Sub

    Protected Sub GvwPnlProjectStudyWorkSummary_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvwPnlProjectStudyWorkSummary.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lnkBtnDetailsForWorkSummary"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkBtnDetailsForWorkSummary"), LinkButton).CommandName = "ProDtlForWorkSummary"
            e.Row.Cells(gvwpnWorkSummary_Details).HorizontalAlign = HorizontalAlign.Center
        End If
    End Sub

    Protected Sub GvwPnlProjectStudyWorkSummary_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvwPnlProjectStudyWorkSummary.PageIndexChanging
        GvwPnlProjectStudyWorkSummary.PageIndex = e.NewPageIndex
        'BindGrid("gvwpnl0004")
        Dim dtTemp As New DataTable
        dtTemp = CType(Me.ViewState("VS_GvwPnlProjectStudyWorkSummary_PageIndexChanging"), DataTable)
        GvwPnlProjectStudyWorkSummary.DataSource = dtTemp
        GvwPnlProjectStudyWorkSummary.DataBind()
    End Sub

#End Region

#Region "GrdvgiewOfOperationalKpi"

    'Protected Sub GrdvgiewOfOperationalKpi_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GrdvgiewOfOperationalKpi.PageIndexChanging
    '    GrdvgiewOfOperationalKpi.PageIndex = e.NewPageIndex
    '    Dim dtOperationalKpi As New DataTable
    '    dtOperationalKpi = ViewState("CommonForAllOperationalKpi")
    '    GrdvgiewOfOperationalKpi.DataSource = dtOperationalKpi
    '    GrdvgiewOfOperationalKpi.DataBind()
    '    mpeDialog.Show()
    'End Sub

    Protected Sub GrdvgiewOfOperationalKpi_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GrdvgiewOfOperationalKpi.RowCommand
        Dim SelectedRowIndex As Integer
        Dim RedirectStr As String = ""
        Dim workSpaceId As String = ""
        SelectedRowIndex = e.CommandArgument

        If e.CommandName.ToUpper = "PRODTLFOROPERATIONALKPI" Then
            workSpaceId = Me.GrdvgiewOfOperationalKpi.Rows(SelectedRowIndex).Cells(gvwpnlOperationalKpi_workspaceid).Text.ToString().Trim()

            RedirectStr = "window.open(""" + "frmProjectDetailMst.aspx?WorkSpaceId=" & workSpaceId & "&Page=frmMyProject&Type=MONITORING&ParentName=frmMainPage" + """)"

            ScriptManager.RegisterClientScriptBlock(Me.Page(), Me.GetType(), "OpenWindow", RedirectStr, True)
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
        End If
    End Sub

    Protected Sub GrdvgiewOfOperationalKpi_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GrdvgiewOfOperationalKpi.RowCreated
        If ViewState("FlagChk") = "BedNights" Then

            If e.Row.RowType = DataControlRowType.DataRow Or
                           e.Row.RowType = DataControlRowType.Header Or
                           e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(gvwpnlOperationalKpi_workspaceid).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_NoOfSamples).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_NoOfIxSamples).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_TotalDosed).Visible = False

            End If

        ElseIf ViewState("FlagChk") = "ClinicDate" Then

            If e.Row.RowType = DataControlRowType.DataRow Or
                           e.Row.RowType = DataControlRowType.Header Or
                           e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(gvw_OperationalKpiiPeriod).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_workspaceid).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_NoOfSamples).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_NoOfIxSamples).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_TotalDosed).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_BedNights).Visible = False

            End If

        ElseIf ViewState("FlagChk") = "DosedSubjects" Then
            If e.Row.RowType = DataControlRowType.DataRow Or
                       e.Row.RowType = DataControlRowType.Header Or
                       e.Row.RowType = DataControlRowType.Footer Then

                e.Row.Cells(gvw_OperationalKpiiPeriod).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_workspaceid).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_NoOfSamples).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_NoOfIxSamples).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_TotalDosed).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_BedNights).Visible = False

            End If


        ElseIf ViewState("FlagChk") = "SampleAnalysis" Then
            If e.Row.RowType = DataControlRowType.DataRow Or
                       e.Row.RowType = DataControlRowType.Header Or
                       e.Row.RowType = DataControlRowType.Footer Then

                e.Row.Cells(gvw_OperationalKpiiPeriod).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_workspaceid).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_TotalDosed).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_BedNights).Visible = False


            End If

        ElseIf ViewState("FlagChk") = "SponsorReport" Then
            If e.Row.RowType = DataControlRowType.DataRow Or
                       e.Row.RowType = DataControlRowType.Header Or
                       e.Row.RowType = DataControlRowType.Footer Then

                e.Row.Cells(gvw_OperationalKpiiPeriod).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_workspaceid).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_NoOfSamples).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_NoOfIxSamples).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_TotalDosed).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_BedNights).Visible = False

            End If


        ElseIf ViewState("FlagChk") = "Report" Then
            If e.Row.RowType = DataControlRowType.DataRow Or
                       e.Row.RowType = DataControlRowType.Header Or
                       e.Row.RowType = DataControlRowType.Footer Then

                e.Row.Cells(gvw_OperationalKpiiPeriod).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_workspaceid).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_NoOfSamples).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_NoOfIxSamples).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_TotalDosed).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_BedNights).Visible = False
            End If





        ElseIf ViewState("FlagChk") = "SampleAnalyst" Then
            If e.Row.RowType = DataControlRowType.DataRow Or
                       e.Row.RowType = DataControlRowType.Header Or
                       e.Row.RowType = DataControlRowType.Footer Then

                e.Row.Cells(gvw_OperationalKpiiPeriod).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_workspaceid).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_TotalDosed).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_BedNights).Visible = False


            End If
        ElseIf ViewState("FlagChk") = "TotalDosed" Then
            If e.Row.RowType = DataControlRowType.DataRow Or
                       e.Row.RowType = DataControlRowType.Header Or
                       e.Row.RowType = DataControlRowType.Footer Then


                e.Row.Cells(gvwpnlOperationalKpi_workspaceid).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_NoOfSamples).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_NoOfIxSamples).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_BedNights).Visible = False
            End If

        ElseIf ViewState("FlagChk") = "ProjectsReleased" Then
            If e.Row.RowType = DataControlRowType.DataRow Or
                      e.Row.RowType = DataControlRowType.Header Or
                      e.Row.RowType = DataControlRowType.Footer Then

                e.Row.Cells(gvw_OperationalKpiiPeriod).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_workspaceid).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_NoOfSamples).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_NoOfIxSamples).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_TotalDosed).Visible = False
                e.Row.Cells(gvwpnlOperationalKpi_BedNights).Visible = False

            End If

        End If




    End Sub

    Protected Sub GrdvgiewOfOperationalKpi_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GrdvgiewOfOperationalKpi.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lnkBtnDetailsForOperationalKpi"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkBtnDetailsForOperationalKpi"), LinkButton).CommandName = "ProDtlForOperationalKpi"
            e.Row.Cells(gvwpnlOperationalKpi_Details).HorizontalAlign = HorizontalAlign.Center
        End If
    End Sub
#End Region

#Region "grdSubjectInfo"
    Protected Sub grdSubjectInfo_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdSubjectInfo.PageIndexChanging
        grdSubjectInfo.PageIndex = e.NewPageIndex
        ''BindGrid("gvwpnl0004")
        Dim dtTemp As New DataTable
        dtTemp = CType(Me.ViewState("VS_grvSubjectInfo"), DataTable)
        grdSubjectInfo.DataSource = dtTemp
        grdSubjectInfo.DataBind()
        grdSubjectInfo.FooterRow.Visible = False
        mdpSubjectInfo.Show()
    End Sub
#End Region

    Protected Sub gvScheduler_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvScheduler.RowDataBound
        Dim HdrStr As String = String.Empty
        Dim strHeader As String = String.Empty
        Dim ds_Scheduler As DataSet

        ActualDateVisible1 = True
        If e.Row.RowType = DataControlRowType.Header Then

            For HdrIndex As Integer = 0 To e.Row.Cells.Count - 1

                ActId.Add("0")
                NodeId.Add("0")

                If e.Row.Cells(HdrIndex).Text.Contains("#") Then
                    HdrStr = Context.Server.HtmlDecode(e.Row.Cells(HdrIndex).Text.Trim())
                    ActId.Insert(HdrIndex, HdrStr.Substring(HdrStr.IndexOf("#") + 1, HdrStr.LastIndexOf("#") - (HdrStr.IndexOf("#") + 1)))
                    NodeId.Insert(HdrIndex, HdrStr.Substring(HdrStr.LastIndexOf("#") + 1).Trim())
                    strHeader = HdrStr.Substring(0, HdrStr.IndexOf("#"))
                    e.Row.Cells(HdrIndex).Attributes.Add("title", strHeader)

                    e.Row.Cells(HdrIndex).Text = strHeader
                End If
            Next HdrIndex
        End If




        Dim ds_Schedule As DataSet = CType(ViewState(VS_Scheduler), DataSet)
        Dim ds_Actual As DataSet = CType(ViewState(VS_Actual), DataSet)

        If e.Row.RowType = DataControlRowType.DataRow Then
            If isDataEntered = False Then
                'lblSiteNo.Text = Convert.ToString(e.Row.Cells(0).Text)
                For cIndex As Integer = 1 To e.Row.Cells.Count - 1
                    Dim txt As LinkButton = New LinkButton()
                    Dim txt1 As LinkButton = New LinkButton()
                    Dim HesderText = Server.HtmlDecode(gvScheduler.HeaderRow.Cells(cIndex).Text.ToString())
                    Dim id As String = "Scheduled_" + HesderText.Trim() + "_" + e.Row.Cells(1).Text
                    Dim id1 As String = "Actual_" + HesderText.Trim() + "_" + e.Row.Cells(1).Text
                    txt.ID = id
                    txt1.ID = id1
                    txt.CssClass = "Scheduled-" + HesderText
                    txt1.CssClass = "Actual-" + HesderText
                    txt.Text = "ScheduledDate " + "</br>"
                    txt1.Text = "</br> ActualDate"
                    e.Row.Cells(cIndex).Controls.Add(txt)
                    e.Row.Cells(cIndex).Controls.Add(txt1)

                    If cIndex <> GVC_FirStVisit Then
                        e.Row.Cells(cIndex).Enabled = False
                    Else
                    End If
                    CType(e.Row.FindControl(id), LinkButton).CommandArgument = e.Row.RowIndex
                    CType(e.Row.FindControl(id), LinkButton).CommandName = "Scheduler"
                    CType(e.Row.FindControl(id1), LinkButton).CommandArgument = e.Row.RowIndex
                    CType(e.Row.FindControl(id1), LinkButton).CommandName = "Actual"

                Next
            Else

                For cIndex As Integer = GVC_FirStVisit To e.Row.Cells.Count - 1
                    Dim txt As LinkButton = New LinkButton()
                    Dim txt1 As LinkButton = New LinkButton()

                    Dim HesderText = Server.HtmlDecode(gvScheduler.HeaderRow.Cells(cIndex).Text.ToString())
                    Dim id As String = "Scheduled_" + HesderText.Trim() + "_" + e.Row.Cells(1).Text + "_" + e.Row.Cells(2).Text + "_" + ActId(cIndex)
                    Dim id1 As String = "Actual_" + HesderText.Trim() + "_" + e.Row.Cells(1).Text + "_" + e.Row.Cells(2).Text + "_" + ActId(cIndex)
                    txt1.ID = id1
                    txt.ID = id
                    txt1.CssClass = "Actual-" + HesderText
                    txt.CssClass = "Scheduled-" + HesderText
                    txt.Text = "ScheduledDate "
                    txt1.Text = "</br> ActualDate"
                    Dim lblSchedule As Label = New Label()
                    Dim lblActual As Label = New Label()
                    lblSchedule.Text = Convert.ToString(ds_Schedule.Tables(0).Rows(e.Row.RowIndex)(cIndex))
                    lblActual.Text = Convert.ToString(ds_Actual.Tables(0).Rows(e.Row.RowIndex)(cIndex))

                    If lblSchedule.Text = "" Then
                        'e.Row.Cells(cIndex).Controls.Add(txt)
                    Else
                        e.Row.Cells(cIndex).Controls.Add(lblSchedule)
                        lblSchedule.ToolTip = "Scheduled Date"
                    End If

                    If lblActual.Text = "" Then
                        ActualDateVisible = False
                        'e.Row.Cells(cIndex).Controls.Add(txt1)
                    Else
                        lblActual.Text = "</br>" + Convert.ToString(ds_Actual.Tables(0).Rows(e.Row.RowIndex)(cIndex))
                        e.Row.Cells(cIndex).Controls.Add(lblActual)
                        lblActual.ToolTip = "Actual Date"
                    End If


                    If cIndex <> GVC_FirStVisit Then
                        If ActualDateVisible = False And ActualDateVisible1 = True AndAlso Not (ds_Schedule.Tables(0).Rows(e.Row.RowIndex)(GVC_cDisContinue) = "Y" Or ds_Schedule.Tables(0).Rows(e.Row.RowIndex)(GVC_cScreenFailure) = "Y") Then
                            ActualDateVisible1 = False
                            txt.Enabled = False
                        Else
                            If Not (ds_Schedule.Tables(0).Rows(e.Row.RowIndex)(GVC_cDisContinue) = "Y" Or ds_Schedule.Tables(0).Rows(e.Row.RowIndex)(GVC_cScreenFailure) = "Y") Then
                                txt1.Enabled = True
                                txt.Enabled = True
                            Else
                                e.Row.Cells(cIndex).Enabled = True
                                txt.Enabled = True
                            End If

                        End If

                    Else
                        If (ds_Schedule.Tables(0).Rows(e.Row.RowIndex)(GVC_cDisContinue) = "Y" Or ds_Schedule.Tables(0).Rows(e.Row.RowIndex)(GVC_cScreenFailure) = "Y") Then
                            txt.Enabled = False
                            txt1.Enabled = False
                        Else
                        End If
                        ActualDateVisible = True
                        If lblActual.Text = "" Then
                            ActualDateVisible1 = False
                        End If
                    End If
                    txt.Enabled = False
                    txt1.Enabled = False
                Next

            End If


        End If

    End Sub

    Protected Sub gvScheduler_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvScheduler.RowCreated
        If e.Row.RowType = DataControlRowType.Header Or
                   e.Row.RowType = DataControlRowType.DataRow Or
                   e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVC_iMySubjectNo).Visible = False
            e.Row.Cells(GVC_cScreenFailure).Visible = False
            e.Row.Cells(GVC_cDisContinue).Visible = False
            e.Row.Cells(GVC_SubjectNo).Visible = False
        End If
    End Sub



#End Region

#Region "Button SetProjects"

    Protected Sub btnSetProjectforclientrequest_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim wStr As String = ""
        Dim dtTemp As New DataTable
        Dim dvTemp As New DataView

        wStr = " vWorkSpaceId='" & HProjectIdForClientRequest.Value.ToString().Trim() & "'"
        dtTemp = CType(Me.ViewState("VS_GvwPnl00001"), DataTable)

        If Not dtTemp Is Nothing Then
            dvTemp = dtTemp.DefaultView
            dvTemp.RowFilter = wStr

            gvwpnl0001.DataSource = dvTemp.ToTable()
            gvwpnl0001.DataBind()
            ViewState("Vs_CommonForPageIndexChangingForClientRequest") = dvTemp.ToTable()
            DdlClientnameforClientRequest.SelectedIndex = 0
        End If


        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divClientRequest');", True)

    End Sub

    Protected Sub btnSetProjectForClinicalPhase_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim wStr As String = ""
        Dim dtTemp As New DataTable
        Dim dvTemp As New DataView

        wStr = "vworkspaceid='" + HProjectIdForClinicalPhase.Value.ToString().Trim() + "'"
        dtTemp = CType(Me.ViewState("VS_GvwPnl00003"), DataTable)

        If Not dtTemp Is Nothing Then
            dvTemp = dtTemp.DefaultView
            dvTemp.RowFilter = wStr
            gvwpnl0003.DataSource = dvTemp.ToTable()
            Me.ViewState("Vs_CommonForPageIndexChangingForClinicalPhase") = dvTemp.ToTable() 'Added By Mrunal Parekh on 27-Jan-2012
            gvwpnl0003.DataBind()
            DdlCLientNameForCliniclaphase.SelectedIndex = 0
        End If
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divClinicalPhase');", True)

    End Sub

    '' Added by DhruviShah
    Protected Sub btnCRFDataStatus_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim wStr As String = ""
        Dim dtTemp As New DataTable
        Dim dvTemp As New DataView

        wStr = "vworkspaceid='" + ProjectWorkSpaceIdDemo.Value.ToString().Trim() + "'"
        ddlColumnList.SelectedIndex = 0
        dd1_chart.SelectedIndex = 0
        dtTemp = CType(Me.ViewState("VS_GvwPnl00003"), DataTable)

        If Not dtTemp Is Nothing Then
            dvTemp = dtTemp.DefaultView
            dvTemp.RowFilter = wStr
            gvwpnl0003.DataSource = dvTemp.ToTable()
            Me.ViewState("Vs_CommonForPageIndexChangingForClinicalPhase") = dvTemp.ToTable()
            gvwpnl0003.DataBind()
            DdlCLientNameForCliniclaphase.SelectedIndex = 0
        End If
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divDemo');", True)

    End Sub

    Protected Sub btnSiteDataStatus_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim wStr As String = ""
        Dim dtTemp As New DataTable
        Dim dvTemp As New DataView

        wStr = "vworkspaceid='" + ProjectWorkSpaceIdDemo.Value.ToString().Trim() + "'"

        ddlColumnList1.SelectedIndex = 0
        dd2_chart.SelectedIndex = 0
        ddlsite.SelectedIndex = 0
        dtTemp = CType(Me.ViewState("VS_GvwPnl00003"), DataTable)

        If Not dtTemp Is Nothing Then
            dvTemp = dtTemp.DefaultView
            dvTemp.RowFilter = wStr
            gvwpnl0003.DataSource = dvTemp.ToTable()
            Me.ViewState("Vs_CommonForPageIndexChangingForClinicalPhase") = dvTemp.ToTable()
            gvwpnl0003.DataBind()
            DdlCLientNameForCliniclaphase.SelectedIndex = 0
        End If
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divDemo1');", True)
    End Sub
    Protected Sub btndcfmanage_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim wStr As String = ""
        Dim dtTemp As New DataTable
        Dim dvTemp As New DataView

        wStr = "vworkspaceid='" + ProjectWorkSpaceIdDCFManage.Value.ToString().Trim() + "'"
        ddlColumnList3.SelectedIndex = 0
        dd3_chart.SelectedIndex = 0
        dtTemp = CType(Me.ViewState("VS_GvwPnl00003"), DataTable)

        If Not dtTemp Is Nothing Then
            dvTemp = dtTemp.DefaultView
            dvTemp.RowFilter = wStr
            gvwpnl0003.DataSource = dvTemp.ToTable()
            Me.ViewState("Vs_CommonForPageIndexChangingForClinicalPhase") = dvTemp.ToTable()
            gvwpnl0003.DataBind()
            DdlCLientNameForCliniclaphase.SelectedIndex = 0
        End If

        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divDcfmanage');", True)


    End Sub
    Protected Sub btnAESE_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim wStr As String = ""
        Dim dtTemp As New DataTable
        Dim dvTemp As New DataView

        wStr = "vworkspaceid='" + hdnAESAEProjectWorkSpaceID.Value.ToString().Trim() + "'"
        dtTemp = CType(Me.ViewState("VS_GvwPnl00003"), DataTable)

        If Not dtTemp Is Nothing Then
            dvTemp = dtTemp.DefaultView
            dvTemp.RowFilter = wStr
            gvwpnl0003.DataSource = dvTemp.ToTable()
            Me.ViewState("Vs_CommonForPageIndexChangingForClinicalPhase") = dvTemp.ToTable()
            gvwpnl0003.DataBind()
            DdlCLientNameForCliniclaphase.SelectedIndex = 0
        End If
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divAESAE');", True)


    End Sub

    Protected Sub btncrf1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim wStr As String = ""
        Dim dtTemp As New DataTable
        Dim dvTemp As New DataView

        wStr = "vworkspaceid='" + hdnCRFStatusProjectWorkSpaceID.Value.ToString().Trim() + "'"
        dtTemp = CType(Me.ViewState("VS_GvwPnl00003"), DataTable)

        If Not dtTemp Is Nothing Then
            dvTemp = dtTemp.DefaultView
            dvTemp.RowFilter = wStr
            gvwpnl0003.DataSource = dvTemp.ToTable()
            Me.ViewState("Vs_CommonForPageIndexChangingForClinicalPhase") = dvTemp.ToTable()
            gvwpnl0003.DataBind()
            DdlCLientNameForCliniclaphase.SelectedIndex = 0
        End If
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divCRFStatus');", True)


    End Sub
    Protected Sub btnSiteInf1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim wStr As String = ""
        Dim dtTemp As New DataTable
        Dim dvTemp As New DataView

        wStr = "vworkspaceid='" + hdnSiteInformationProjectWorkSpaceID.Value.ToString().Trim() + "'"
        dtTemp = CType(Me.ViewState("VS_GvwPnl00003"), DataTable)

        If Not dtTemp Is Nothing Then
            dvTemp = dtTemp.DefaultView
            dvTemp.RowFilter = wStr
            gvwpnl0003.DataSource = dvTemp.ToTable()
            Me.ViewState("Vs_CommonForPageIndexChangingForClinicalPhase") = dvTemp.ToTable()
            gvwpnl0003.DataBind()
            DdlCLientNameForCliniclaphase.SelectedIndex = 0
        End If
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divSiteInformation');", True)


    End Sub

    ''completed by Dhruvi Shah
    Protected Sub btnSubjectinfo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim wStr As String = ""
        Dim dtTemp As New DataTable
        Dim dvTemp As New DataView

        wStr = "vworkspaceid='" + ProjectWorkSpaceID.Value.ToString().Trim() + "'"
        dtTemp = CType(Me.ViewState("VS_GvwPnl00003"), DataTable)

        If Not dtTemp Is Nothing Then
            dvTemp = dtTemp.DefaultView
            dvTemp.RowFilter = wStr
            gvwpnl0003.DataSource = dvTemp.ToTable()
            Me.ViewState("Vs_CommonForPageIndexChangingForClinicalPhase") = dvTemp.ToTable()
            gvwpnl0003.DataBind()
            DdlCLientNameForCliniclaphase.SelectedIndex = 0
        End If
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divSiteWiseSubjectDetail');", True)
    End Sub

    '' Completed by DhruviShah

    Protected Sub btnSetProjectforAnalyticalPhase_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim wStr As String = ""
        Dim dtTemp As New DataTable
        Dim dvTemp As New DataView

        wStr = "vWorkSpaceId='" & HProjectIdforAnalyticalPhase.Value.ToString().Trim() & "'"
        dtTemp = CType(Me.ViewState("VS_gvwpnl_Analysis"), DataTable)

        If Not dtTemp Is Nothing Then
            dvTemp = dtTemp.DefaultView
            dvTemp.RowFilter = wStr
            Me.ViewState("Vs_CommonForPageIndexChangingForAnalyticalPhase") = dvTemp.ToTable()
            gvwpnl_Analysis.DataSource = dvTemp.ToTable()
            gvwpnl_Analysis.DataBind()
            DdlCLientNameForAnalyticalPhase.SelectedIndex = 0
        End If
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divAnalyticalphase');", True)

    End Sub

    Protected Sub BtnSetProjectForDocumentPhase_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSetProjectForDocumentPhase.Click
        Dim wStr As String = ""
        Dim dtTemp As New DataTable
        Dim dvTemp As New DataView

        wStr = "vWorkSpaceId='" & HProjectIdforDocumentPhase.Value.ToString().Trim() & "'"
        dtTemp = CType(Me.ViewState("VS_GvwPnl00004"), DataTable)

        If Not dtTemp Is Nothing Then
            dvTemp = dtTemp.DefaultView
            dvTemp.RowFilter = wStr
            gvwpnl0004.DataSource = dvTemp.ToTable()
            gvwpnl0004.DataBind()
            Me.ViewState("Vs_CommonForPageIndexChangingForDocumentPhase") = dvTemp.ToTable()
            DdllstForDocumentPhase.SelectedIndex = 0
        End If
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divDocumentPhase');", True)
    End Sub

    Protected Sub btnSetProjectforTracking_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProjectforTracking.Click
        'Me.divProjectTrack.Style.Add("display", "block")
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectTrack');", True)
        'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "CheckBoxChange", "chkChange()", True)
    End Sub

    Protected Sub btnSetProjectProjectPreClinical_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProjectProjectPreClinical.Click
        Dim wStr As String = ""
        Dim dtTemp As New DataTable
        Dim dvTemp As New DataView

        wStr = " vWorkSpaceId='" & HFRequestIdProjectPreClinical.Value.ToString().Trim() & "'"

        dtTemp = CType(Me.ViewState("VS_GvwPnl00013"), DataTable)

        If Not dtTemp Is Nothing Then
            dvTemp = dtTemp.DefaultView
            dvTemp.RowFilter = wStr

            gvwpnl0013.DataSource = dvTemp.ToTable()
            gvwpnl0013.DataBind()
            ViewState("Vs_CommonForPageIndexChangingForPreClinical") = dvTemp.ToTable()
            dtTemp = dvTemp.ToTable()
            DdlClientnameforProjectPreClinical.SelectedIndex = 0
            If dtTemp.Rows.Count > 0 Then
                DdlClientnameforProjectPreClinical.SelectedValue = dtTemp.Rows(0).Item("vClientName").ToString()
            End If
        End If

        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectPreClinical');", True)
    End Sub

    ''Comment by Bhargav Thaker
    'Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
    '    Dim Parameters As String = ""
    '    Dim wstr As String = ""
    '    Dim dtTemp As New DataTable
    '    Dim dvTemp As New DataView
    '    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    '    Dim strReturn As String = String.Empty
    '    Dim Sql_DataSet As Data.DataSet = Nothing
    '    Dim Sql_DataSetForActivity As Data.DataSet = Nothing
    '    Dim eStr_Retu As String = String.Empty

    '    Parameters = HProjectId.Value.ToString().Trim() + "##" + "" + "##" + "1"

    '    If Not objHelp.Proc_SiteWiseSubjectInformationForDashboard(Parameters, Sql_DataSet, eStr_Retu) Then

    '        Throw New Exception("Error while getting data from Proc_SiteWiseSubjectInformationForDashboard " + eStr_Retu.Trim)
    '    End If

    '    wstr = "iParentNodeId = 1 and vWorkSpaceId = " + HProjectId.Value.ToString().Trim() + " and vNodeDisplayName NOT LIKE '%ADJUDICATOR%' and vNodeDisplayName NOT LIKE '%ELIGIBILITY-REVIEW%' order by iNodeNo"
    '    If Not objHelp.GetViewWorkSpaceNodeDetail(wstr, Sql_DataSetForActivity, eStr_Retu) Then

    '        Throw New Exception("Error while getting data from GetViewWorkSpaceNodeDetail " + eStr_Retu.Trim)
    '    End If

    '    Sql_DataSet.Tables(0).TableName = "Table"
    '    'Sql_DataSetForActivity.Tables(0).TableName = "Activity"
    '    ViewState(VS_SiteWiseSubjectInformation) = JsonConvert.SerializeObject(Sql_DataSet)
    '    'ViewState(VS_SiteWiseCA) = JsonConvert.SerializeObject(Sql_DataSet)
    '    'ViewState(VS_SiteWiseGR) = JsonConvert.SerializeObject(Sql_DataSet)
    '    'ViewState(VS_SiteWiseAR) = JsonConvert.SerializeObject(Sql_DataSet)
    '    ViewState(VS_Activity) = JsonConvert.SerializeObject(Sql_DataSetForActivity)

    '    'Dim currentWeekStartDate As DateTime = DateTime.Now.AddDays(-7)
    '    'txtVisitFromDate.Text = currentWeekStartDate.ToString("dd-MMM-yyyy")
    '    'txtVisitToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy")
    '    txtVisitFromDate.Text = ""
    '    txtVisitToDate.Text = ""
    '    gvVisitReviewStatus.DataSource = Nothing
    '    gvVisitReviewStatus.DataBind()

    '    Dim eStr As String = String.Empty
    '    Dim ds_Subject As New DataSet
    '    Dim Usertype As String = Session(S_UserType)

    '    If Usertype = "0123" Then
    '        FillAdjucatorviewStatusGrid()
    '    Else
    '        hdniUserId1.Value = Session(S_UserID)
    '        DISoftURL1.Value = Convert.ToString(ConfigurationManager.AppSettings.Item("DISoftURL").Trim())
    '        hdnWorkFlowStageId1.Value = Session(S_WorkFlowStageId)

    '        If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
    '            wstr = String.Empty
    '            wstr = "(isnull(vWorkspaceId,'') ='" + Me.HProjectId.Value.Trim() + " ' OR '" + Me.HProjectId.Value.Trim() + "' ='') " + vbCrLf +
    '                    "AND iUserId = " + Me.Session(S_UserID) + " Order by iImgTransmittalDtlId desc"
    '            objHelp.GetFieldsOfTable("View_GetVisitStatus", " * ", wstr, ds_Subject, eStr)

    '            If ds_Subject Is Nothing Then
    '                gvVisitReviewStatus.DataSource = Nothing
    '                gvVisitReviewStatus.DataBind()
    '                If gvVisitReviewStatus.Rows.Count > 0 Then
    '                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIVisitDatatable", "UIVisitDatatable();", True)
    '                End If
    '            ElseIf ds_Subject.Tables.Count = 0 Then
    '                gvVisitReviewStatus.DataSource = Nothing
    '                gvVisitReviewStatus.DataBind()
    '                If gvVisitReviewStatus.Rows.Count > 0 Then
    '                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIVisitDatatable", "UIVisitDatatable();", True)
    '                End If
    '            Else
    '                Me.ViewState("getData") = ds_Subject.Tables(0)
    '                gvVisitReviewStatus.DataSource = ds_Subject.Tables(0)
    '                gvVisitReviewStatus.DataBind()

    '                If Session(S_WorkFlowStageId) = WorkFlowStageId_OnlyView Then
    '                    gvVisitReviewStatus.Columns(11).Visible = True
    '                    gvVisitReviewStatus.Columns(12).Visible = True
    '                Else
    '                    gvVisitReviewStatus.Columns(11).Visible = False
    '                    gvVisitReviewStatus.Columns(12).Visible = False
    '                End If

    '                If gvVisitReviewStatus.Rows.Count > 0 Then
    '                    Dim json As String = " var pageJson = " + JsonConvert.SerializeObject(ds_Subject.Tables(0)) + ";"
    '                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIVisitDatatable", json + " UIVisitDatatable();", True)
    '                End If
    '            End If
    '        End If

    '        If Not ds_Subject Is Nothing Then
    '            ds_Subject.Dispose()
    '        End If
    '    End If

    '    'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen(strReturn);", True)
    'End Sub

    '' Added By Bhargav Thaker
    Protected Sub btnSetSite_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetSite.Click
        Dim eStr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim WBString As String = String.Empty 'Added By Bhargav Thaker 06Mar2023
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim ds_Subject As New DataSet
        Dim Usertype As String = Session(S_UserType)
        Dim dv As New DataView 'Added By Bhargav Thaker 06Mar2023
        Dim dtData As DataTable = New DataTable() 'Added By Bhargav Thaker 06Mar2023
        Dim dv_Subject As New DataView 'Added By Bhargav Thaker 06Mar2023

        gvVisitReviewStatus.DataSource = Nothing
        gvVisitReviewStatus.DataBind()

        If Usertype = "0123" Then
            FillAdjucatorviewStatusGrid()
        Else
            hdniUserId1.Value = Session(S_UserID)
            DISoftURL1.Value = Convert.ToString(ConfigurationManager.AppSettings.Item("DISoftURL").Trim())
            hdnWorkFlowStageId1.Value = Session(S_WorkFlowStageId)

            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                wstr = String.Empty
                If Me.txtSite.Text <> String.Empty Then
                    Me.txtprojectForDI.SelectedIndex = 0
                End If

                wstr = "(isnull(vWorkspaceId,'') ='" + Me.HProjectId.Value.Trim() + " ' OR '" + Me.HProjectId.Value.Trim() + "' ='') " + vbCrLf +
                        "AND iUserId = " + Me.Session(S_UserID) + " Order by iImgTransmittalDtlId desc"
                objHelp.GetFieldsOfTable("View_GetVisitStatus", " * ", wstr, ds_Subject, eStr)

                'Added by Bhargav Thaker 06Mar2023
                If ddlVisitStatus.SelectedValue <> "All" Then
                    If ddlVisitStatus.SelectedValue = "ImageUploader" Then
                        WBString = "IN ('Uploaded', 'Rejected')"
                    ElseIf ddlVisitStatus.SelectedValue = "QC1" Then
                        WBString = "IN ('Uploaded', 'Reuploaded')"
                    ElseIf ddlVisitStatus.SelectedValue = "QC2" Then
                        WBString = "IN ('QC1 Review Complete')"
                    ElseIf ddlVisitStatus.SelectedValue = "CA" Then
                        WBString = "IN ('QC2 Review Complete')"
                    ElseIf ddlVisitStatus.SelectedValue = "R12" Then
                        WBString = "IN ('CA1 Review Complete')"
                    ElseIf ddlVisitStatus.SelectedValue = "Pending" AndAlso Convert.ToString(ConfigurationManager.AppSettings("ImageUploaderUserCode")).Contains(Me.Session(S_UserType)) Then
                        WBString = "IN ('Uploaded', 'Rejected')"
                    ElseIf ddlVisitStatus.SelectedValue = "Pending" AndAlso Convert.ToString(ConfigurationManager.AppSettings("QCUserCode")).Contains(Me.Session(S_UserType)) Then
                        WBString = "IN ('Uploaded', 'Reuploaded')"
                    ElseIf ddlVisitStatus.SelectedValue = "Pending" AndAlso Convert.ToString(ConfigurationManager.AppSettings("QC2UserCode")).Contains(Me.Session(S_UserType)) Then
                        WBString = "IN ('QC1 Review Complete')"
                    ElseIf ddlVisitStatus.SelectedValue = "Pending" AndAlso Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(Me.Session(S_UserType)) Then
                        WBString = "IN ('QC2 Review Complete')"
                    ElseIf ddlVisitStatus.SelectedValue = "Pending" AndAlso (Me.Session(S_UserType) = "0118" Or Me.Session(S_UserType) = "0119") Then
                        WBString = "IN ('R1 Complete - R2-Pending', 'R2 Complete - R1-Pending')"
                    Else
                        WBString = " = Status"
                    End If
                Else
                    WBString = " = Status"
                End If

                dv = ds_Subject.Tables(0).DefaultView
                dv.RowFilter = "Status  " + WBString
                If dv.ToTable().Rows.Count < 0 Then
                    dv = ds.Tables(0).DefaultView
                End If

                gvVisitReviewStatus.DataSource = Nothing
                dv_Subject = dv

                If ds_Subject Is Nothing Then
                    emptyTable.Visible = True
                    gvVisitReviewStatus.DataSource = Nothing
                    gvVisitReviewStatus.DataBind()
                    If gvVisitReviewStatus.Rows.Count > 0 Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIVisitDatatable", "UIVisitDatatable();", True)
                    End If
                ElseIf ds_Subject.Tables.Count = 0 Then
                    emptyTable.Visible = True
                    gvVisitReviewStatus.DataSource = Nothing
                    gvVisitReviewStatus.DataBind()
                    If gvVisitReviewStatus.Rows.Count > 0 Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIVisitDatatable", "UIVisitDatatable();", True)
                    End If
                Else
                    'if else condition Added by Bhargav Thaker 06Mar2023
                    If dv_Subject.ToTable.Rows.Count > 0 Then
                        emptyTable.Visible = False
                        dtData = dv_Subject.ToTable()
                        Me.ViewState("getData") = dtData
                        gvVisitReviewStatus.DataSource = dtData
                        gvVisitReviewStatus.DataBind()
                    Else
                        emptyTable.Visible = True
                    End If

                    If Session(S_WorkFlowStageId) = WorkFlowStageId_OnlyView Then
                        gvVisitReviewStatus.Columns(11).Visible = True
                        gvVisitReviewStatus.Columns(12).Visible = True
                    Else
                        gvVisitReviewStatus.Columns(11).Visible = False
                        gvVisitReviewStatus.Columns(12).Visible = False
                    End If

                    If gvVisitReviewStatus.Rows.Count > 0 Then
                        Dim json As String = " var pageJson = " + JsonConvert.SerializeObject(ds_Subject.Tables(0)) + ";"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIVisitDatatable", json + " UIVisitDatatable();", True)
                    End If
                End If
            End If

            If Not ds_Subject Is Nothing Then
                ds_Subject.Dispose()
            End If
        End If
    End Sub
#End Region

#Region "DropDown Events"
    'Added by Bhargav Thaker
    Protected Sub txtprojectForDI_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim eStr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim WBString As String = String.Empty 'Added By Bhargav Thaker 06Mar2023
        Dim Usertype As String = Session(S_UserType)
        Dim ds_Subject As New DataSet
        Dim dtData As DataTable = New DataTable() 'Added By Bhargav Thaker 06Mar2023
        Dim dv As New DataView 'Added By Bhargav Thaker 06Mar2023
        Dim dv_Subject As New DataView 'Added By Bhargav Thaker 06Mar2023
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Try
            Me.HProjectId.Value = txtprojectForDI.SelectedValue
            Me.txtSite.Text = ""
            If Me.txtprojectForDI.SelectedIndex <> 0 Then
                If Usertype = "0123" Then
                    FillAdjucatorviewStatusGrid()
                Else
                    hdniUserId1.Value = Session(S_UserID)
                    DISoftURL1.Value = Convert.ToString(ConfigurationManager.AppSettings.Item("DISoftURL").Trim())
                    hdnWorkFlowStageId1.Value = Session(S_WorkFlowStageId)

                    If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                        wstr = String.Empty
                        'wstr = "(isnull(vWorkspaceId,'') ='" + Me.HProjectId.Value.Trim() + "' OR isnull(vparentWorkspaceId,'') ='" + Me.HProjectId.Value.Trim() + "' OR '" + Me.HProjectId.Value.Trim() + "' ='') " + vbCrLf +
                        '        "AND iUserId = " + Me.Session(S_UserID) + " Order by iImgTransmittalDtlId desc"

                        wstr = "vWorkspaceId IN (SELECT vWorkspaceId FROM dbo.View_MyProjects WHERE iUserId=" + Me.Session(S_UserID) + " AND cWorkspaceType='C' AND ParentWorkspaceId IN (SELECT ParentWorkspaceId FROM dbo.View_MyProjects WHERE iUserId=" + Me.Session(S_UserID) + " AND isnull(ParentWorkspaceId,'')='" + Me.HProjectId.Value.Trim() + "' AND cWorkspaceType='P')) " + vbCrLf +
                            "AND iUserId = " + Me.Session(S_UserID) + " Order by iImgTransmittalDtlId desc"
                        objHelp.GetFieldsOfTable("View_GetVisitStatus", " * ", wstr, ds_Subject, eStr)

                        'Added by Bhargav Thaker 06Mar2023
                        If ddlVisitStatus.SelectedValue <> "All" Then
                            If ddlVisitStatus.SelectedValue = "ImageUploader" Then
                                WBString = "IN ('Uploaded', 'Rejected')"
                            ElseIf ddlVisitStatus.SelectedValue = "QC1" Then
                                WBString = "IN ('Uploaded', 'Reuploaded')"
                            ElseIf ddlVisitStatus.SelectedValue = "QC2" Then
                                WBString = "IN ('QC1 Review Complete')"
                            ElseIf ddlVisitStatus.SelectedValue = "CA" Then
                                WBString = "IN ('QC2 Review Complete')"
                            ElseIf ddlVisitStatus.SelectedValue = "R12" Then
                                WBString = "IN ('CA1 Review Complete')"
                            ElseIf ddlVisitStatus.SelectedValue = "Pending" AndAlso Convert.ToString(ConfigurationManager.AppSettings("ImageUploaderUserCode")).Contains(Me.Session(S_UserType)) Then
                                WBString = "IN ('Uploaded', 'Rejected')"
                            ElseIf ddlVisitStatus.SelectedValue = "Pending" AndAlso Convert.ToString(ConfigurationManager.AppSettings("QCUserCode")).Contains(Me.Session(S_UserType)) Then
                                WBString = "IN ('Uploaded', 'Reuploaded')"
                            ElseIf ddlVisitStatus.SelectedValue = "Pending" AndAlso Convert.ToString(ConfigurationManager.AppSettings("QC2UserCode")).Contains(Me.Session(S_UserType)) Then
                                WBString = "IN ('QC1 Review Complete')"
                            ElseIf ddlVisitStatus.SelectedValue = "Pending" AndAlso Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(Me.Session(S_UserType)) Then
                                WBString = "IN ('QC2 Review Complete')"
                            ElseIf ddlVisitStatus.SelectedValue = "Pending" AndAlso (Me.Session(S_UserType) = "0118" Or Me.Session(S_UserType) = "0119") Then
                                WBString = "IN ('R1 Complete - R2-Pending', 'R2 Complete - R1-Pending')"
                            Else
                                WBString = " = Status"
                            End If
                        Else
                            WBString = " = Status"
                        End If

                        dv = ds_Subject.Tables(0).DefaultView
                        dv.RowFilter = "Status  " + WBString
                        If dv.ToTable().Rows.Count < 1 Then
                            dv = ds.Tables(0).DefaultView
                        End If

                        gvVisitReviewStatus.DataSource = Nothing
                        dv_Subject = dv

                        If ds_Subject Is Nothing Then
                            gvVisitReviewStatus.DataSource = Nothing
                            gvVisitReviewStatus.DataBind()
                            emptyTable.Visible = True
                            If gvVisitReviewStatus.Rows.Count > 0 Then
                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIVisitDatatable", "UIVisitDatatable();", True)
                            End If
                        ElseIf ds_Subject.Tables.Count = 0 Then
                            gvVisitReviewStatus.DataSource = Nothing
                            gvVisitReviewStatus.DataBind()
                            emptyTable.Visible = True
                            If gvVisitReviewStatus.Rows.Count > 0 Then
                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIVisitDatatable", "UIVisitDatatable();", True)
                            End If
                        Else
                            'if else condition Added by Bhargav Thaker 06Mar2023
                            If dv_Subject.ToTable.Rows.Count > 0 Then
                                emptyTable.Visible = False
                                dtData = dv_Subject.ToTable()
                                Me.ViewState("getData") = dtData
                                gvVisitReviewStatus.DataSource = dtData
                                gvVisitReviewStatus.DataBind()
                            Else
                                emptyTable.Visible = True
                            End If

                            If Session(S_WorkFlowStageId) = WorkFlowStageId_OnlyView Then
                                gvVisitReviewStatus.Columns(11).Visible = True
                                gvVisitReviewStatus.Columns(12).Visible = True
                            Else
                                gvVisitReviewStatus.Columns(11).Visible = False
                                gvVisitReviewStatus.Columns(12).Visible = False
                            End If

                            If gvVisitReviewStatus.Rows.Count > 0 Then
                                emptyTable.Visible = False
                                Dim json As String = " var pageJson = " + JsonConvert.SerializeObject(ds_Subject.Tables(0)) + ";"
                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIVisitDatatable", json + " UIVisitDatatable();", True)
                            End If
                        End If
                    End If

                    If Not ds_Subject Is Nothing Then
                        ds_Subject.Dispose()
                    End If
                End If
            Else
                objCommon.ShowAlert("Select Study.", Me.Page)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".........Error While txtprojectForDI_SelectedIndexChanged()")
        End Try
    End Sub

    Protected Sub DdllstForDocumentPhase_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdllstForDocumentPhase.SelectedIndexChanged
        Dim WStr_Scope As String = String.Empty
        Dim dt_DocumentClientName As New DataTable
        Dim dv_DocumentClientName As DataView
        If DdllstForDocumentPhase.SelectedIndex = 0 Then
            BindGrid("gvwpnl0004")
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divDocumentPhase');", True)
            Exit Sub
        End If
        WStr_Scope += "vClientName='" & DdllstForDocumentPhase.SelectedItem.ToString().Trim() & "'"
        dt_DocumentClientName = CType(ViewState("VS_GvwPnl00004"), DataTable)
        dv_DocumentClientName = dt_DocumentClientName.DefaultView()
        dv_DocumentClientName.RowFilter = WStr_Scope
        dv_DocumentClientName.ToTable.AcceptChanges()
        dv_DocumentClientName.Sort = "vClientName"
        dv_DocumentClientName.ToTable().AcceptChanges()
        Me.gvwpnl0004.DataSource = dv_DocumentClientName.ToTable()
        Me.gvwpnl0004.DataBind()
        Me.ViewState("Vs_CommonForPageIndexChangingForDocumentPhase") = dv_DocumentClientName.ToTable()

        If TxtProjectNoForDocumentPhase.Text <> "" Then
            TxtProjectNoForDocumentPhase.Text = ""
        End If
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divDocumentPhase');", True)
    End Sub

    Protected Sub DdlCLientNameForAnalyticalPhase_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim WStr_Scope As String = String.Empty
        Dim dt_AnalyticalClientName As New DataTable
        Dim dv_AnalyticalClientName As DataView
        If DdlCLientNameForAnalyticalPhase.SelectedIndex = 0 Then
            BindGrid("gvwpnl_Analysis")
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divAnalyticalphase');", True)
            Exit Sub
        End If

        WStr_Scope += "vClientName='" & DdlCLientNameForAnalyticalPhase.SelectedItem.ToString().Trim() & "'"
        dt_AnalyticalClientName = CType(ViewState("VS_gvwpnl_Analysis"), DataTable)
        dv_AnalyticalClientName = dt_AnalyticalClientName.DefaultView()
        dv_AnalyticalClientName.RowFilter = WStr_Scope
        dv_AnalyticalClientName.ToTable.AcceptChanges()
        dv_AnalyticalClientName.Sort = "vClientName"
        dv_AnalyticalClientName.ToTable().AcceptChanges()
        Me.gvwpnl_Analysis.DataSource = dv_AnalyticalClientName.ToTable()
        Me.gvwpnl_Analysis.DataBind()
        'Ddlstatusforanlyticalphase.SelectedIndex = 0
        Me.ViewState("Vs_CommonForPageIndexChangingForAnalyticalPhase") = dv_AnalyticalClientName.ToTable()

        If TxtProjectNoForAnalyticalPhase.Text <> "" Then
            TxtProjectNoForAnalyticalPhase.Text = ""
        End If
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divAnalyticalphase');", True)
    End Sub

    Protected Sub DdlClientnameforClientRequest_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlClientnameforClientRequest.SelectedIndexChanged
        Dim WStr_Scope As String = String.Empty
        Dim dv_ClientRequestClientName As DataView
        Dim dt_ClientRequestClientName As New DataTable
        If DdlClientnameforClientRequest.SelectedIndex = 0 Then
            BindGrid("gvwpnl0001")
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divClientRequest');", True)
            Exit Sub
        End If

        WStr_Scope += "vClientName='" & DdlClientnameforClientRequest.SelectedItem.ToString().Trim() & "'"
        dt_ClientRequestClientName = CType(ViewState("VS_GvwPnl00001"), DataTable)
        dv_ClientRequestClientName = dt_ClientRequestClientName.DefaultView()
        dv_ClientRequestClientName.RowFilter = WStr_Scope
        dv_ClientRequestClientName.ToTable.AcceptChanges()
        dv_ClientRequestClientName.Sort = "vClientName"
        dv_ClientRequestClientName.ToTable().AcceptChanges()
        Me.gvwpnl0001.DataSource = dv_ClientRequestClientName.ToTable()
        Me.gvwpnl0001.DataBind()
        Me.ViewState("Vs_CommonForPageIndexChangingForClientRequest") = dv_ClientRequestClientName.ToTable()
        If TxtRequestIdClientRequest.Text <> "" Then
            TxtRequestIdClientRequest.Text = ""
        End If
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divClientRequest');", True)
    End Sub

    Protected Sub DdlCLientNameForCliniclaphase_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dv_ClinicalPhaseClientName As DataView
        Dim dt_ClinicalPhaseClientName As New DataTable
        Dim WStr_Scope As String = String.Empty
        If DdlCLientNameForCliniclaphase.SelectedIndex = 0 Then
            BindGrid("gvwpnl0003")
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divClinicalPhase');", True)
            Exit Sub
        End If

        WStr_Scope += "vClientName='" & DdlCLientNameForCliniclaphase.SelectedItem.ToString().Trim() & "'"
        dt_ClinicalPhaseClientName = CType(ViewState("VS_GvwPnl00003"), DataTable)
        dv_ClinicalPhaseClientName = dt_ClinicalPhaseClientName.DefaultView()
        dv_ClinicalPhaseClientName.RowFilter = WStr_Scope
        dv_ClinicalPhaseClientName.ToTable.AcceptChanges()
        dv_ClinicalPhaseClientName.Sort = "vClientName"
        dv_ClinicalPhaseClientName.ToTable().AcceptChanges()
        Me.gvwpnl0003.DataSource = dv_ClinicalPhaseClientName.ToTable()
        Me.gvwpnl0003.DataBind()
        Me.ViewState("Vs_CommonForPageIndexChangingForClinicalPhase") = dv_ClinicalPhaseClientName.ToTable()

        If TxtProjectForClinicalPhase.Text <> "" Then
            TxtProjectForClinicalPhase.Text = ""
        End If
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divClinicalPhase');", True)
    End Sub

    Protected Sub DdllistForDepartment_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdllistForDepartment.SelectedIndexChanged
        Dim Wstr As String = String.Empty
        Dim Estr As String = String.Empty
        Dim ds_Details As New DataSet
        Dim Ds_ActivityName As New DataSet
        Dim Dv_ActivityName As New DataView

        Try
            If Not Me.objHelpDbTable.Proc_GetActivityForDashBoard(3, IIf(DdllistForDepartment.SelectedIndex = 0, "", Session(S_DeptCode)), Ds_ActivityName, Estr) Then
                Me.objCommon.ShowAlert("Error While Getting Activity", Me.Page)
                Exit Sub
            End If

            Dv_ActivityName = Ds_ActivityName.Tables(0).DefaultView().ToTable(True, "vActivityName,vActivityId".Split(", ")).DefaultView()
            Dv_ActivityName.Sort = "vActivityName"
            DdllistActivityName.DataSource = Dv_ActivityName.ToTable()
            DdllistActivityName.DataTextField = "vActivityName"
            DdllistActivityName.DataValueField = "vActivityId"
            DdllistActivityName.DataBind()
            DdllistActivityName.Width = 200
            DdllistActivityName.Items.Insert(0, "All Activities")

            ''For Getting Data Of Calendar
            Wstr = "vDeptCode='" & DdllistForDepartment.SelectedValue.ToString() & "'"

            If ddlProjectManager.SelectedIndex > 0 Then
                Wstr += " AND iProjectManagerId ='" & ddlProjectManager.SelectedValue + "'"
            End If

            If Not objHelpDbTable.View_ActivityStartEndDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Details, Estr) Then
                Me.objCommon.ShowAlert("Error While Getting Data From View_ActivityStartEndDtl", Me.Page)
                Exit Sub
            End If
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divMyCalendar');", True)
            ViewState("DetailsOfSelectedMonth") = ds_Details
            CldDatePicker.Visible = True
        Catch ex As Exception
            Me.objCommon.ShowAlert("Error While Getting Calendar Details.", Me.Page)
            Me.ShowErrorMessage("Error While Getting Calendar Details.", ex.Message)
        Finally
            If Not IsNothing(Ds_ActivityName) Then
                Ds_ActivityName.Dispose()
            End If
        End Try
    End Sub

    Protected Sub DdlClientnameforProjectPreClinical_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlClientnameforProjectPreClinical.SelectedIndexChanged
        Dim WStr_Scope As String = String.Empty
        Dim dv_PrecliinicalClientName As DataView
        Dim dt_PrecliinicalClientName As New DataTable
        If DdlClientnameforProjectPreClinical.SelectedIndex = 0 Then
            BindGrid("gvwpnl0013")
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectPreClinical');", True)
            Exit Sub
        End If

        WStr_Scope += "vClientName='" & DdlClientnameforProjectPreClinical.SelectedItem.ToString().Trim() & "'"
        dt_PrecliinicalClientName = CType(ViewState("VS_GvwPnl00013"), DataTable)
        dv_PrecliinicalClientName = dt_PrecliinicalClientName.DefaultView()
        dv_PrecliinicalClientName.RowFilter = WStr_Scope
        dv_PrecliinicalClientName.ToTable.AcceptChanges()
        dv_PrecliinicalClientName.Sort = "vClientName"
        dv_PrecliinicalClientName.ToTable().AcceptChanges()
        Me.gvwpnl0013.DataSource = dv_PrecliinicalClientName.ToTable()
        Me.gvwpnl0013.DataBind()
        Me.ViewState("Vs_CommonForPageIndexChangingForPreClinical") = dv_PrecliinicalClientName.ToTable()
        If TxtProjectRequestIdProjectPreClinical.Text <> "" Then
            TxtProjectRequestIdProjectPreClinical.Text = ""
        End If
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectPreClinical');", True)
    End Sub

    Protected Sub DdllistForOperationalKpi_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdllistForOperationalKpi.SelectedIndexChanged
        Dim LastMonth As String = String.Empty
        Dim LastdayOfPpreviousMonth As String = String.Empty
        Dim FromDateOfOperationalKpi As String = String.Empty
        Dim ToDateOfOperationalKpi As String = String.Empty

        If Convert.ToString(DdllistForOperationalKpi.SelectedItem).Trim() = "Last Month" Then
            FromDateOfOperationalKpi = "01" & "-" & MonthName(DateTime.Now.AddMonths(-1).Month, True) & "-" & DateTime.Now.AddMonths(-1).Year.ToString()
            LastdayOfPpreviousMonth = DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year.ToString(), DateTime.Now.AddMonths(-1).Month)
            ToDateOfOperationalKpi = LastdayOfPpreviousMonth & "-" & MonthName(DateTime.Now.AddMonths(-1).Month, True) & "-" & DateTime.Now.AddMonths(-1).Year.ToString()
            TxtFromDateOfOperationalKpi.Text = FromDateOfOperationalKpi
            TxtToDateOfOperationalKpi.Text = ToDateOfOperationalKpi
            TxtFromDateOfOperationalKpi.Enabled = True
            TxtToDateOfOperationalKpi.Enabled = True
            LblFromDateForOperationalKpi.Enabled = True
            BtnGoForOperationalKpi.Enabled = True
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)

        ElseIf Convert.ToString(DdllistReportFor.SelectedItem).Trim() = "Current Month" Then
            FromDateOfOperationalKpi = "01" & "-" & MonthName(DateTime.Now.Month, True) & "-" & DateTime.Now.Year.ToString()
            LastdayOfPpreviousMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)
            ToDateOfOperationalKpi = LastdayOfPpreviousMonth & "-" & MonthName(DateTime.Now.Month, True) & "-" & DateTime.Now.Year
            TxtFromDateOfOperationalKpi.Text = FromDateOfOperationalKpi
            TxtToDateOfOperationalKpi.Text = ToDateOfOperationalKpi
            TxtFromDateOfOperationalKpi.Enabled = True
            TxtToDateOfOperationalKpi.Enabled = True
            LblFromDateForOperationalKpi.Enabled = True
            BtnGoForOperationalKpi.Enabled = True
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)

        ElseIf Convert.ToString(DdllistForOperationalKpi.SelectedItem).Trim() = "Current Month" Then
            FromDateOfOperationalKpi = "01" & "-" & MonthName(DateTime.Now.Month, True) & "-" & DateTime.Now.Year.ToString()
            LastdayOfPpreviousMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)
            ToDateOfOperationalKpi = LastdayOfPpreviousMonth & "-" & MonthName(DateTime.Now.Month, True) & "-" & DateTime.Now.Year
            TxtFromDateOfOperationalKpi.Text = FromDateOfOperationalKpi
            TxtToDateOfOperationalKpi.Text = ToDateOfOperationalKpi
            TxtFromDateOfOperationalKpi.Enabled = True
            TxtToDateOfOperationalKpi.Enabled = True
            LblFromDateForOperationalKpi.Enabled = True
            BtnGoForOperationalKpi.Enabled = True
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)

        ElseIf Convert.ToString(DdllistForOperationalKpi.SelectedItem).Trim() = "Last Quater" Then
            FromDateOfOperationalKpi = "01" & "-" & MonthName(DateTime.Now.AddMonths(-2).Month, True) & "-" & DateTime.Now.AddMonths(-2).Year.ToString()
            LastdayOfPpreviousMonth = DateTime.DaysInMonth(DateTime.Now.Year.ToString(), DateTime.Now.Month)
            ToDateOfOperationalKpi = LastdayOfPpreviousMonth & "-" & MonthName(DateTime.Now.Month, True) & "-" & DateTime.Now.Year.ToString()
            TxtFromDateOfOperationalKpi.Text = FromDateOfOperationalKpi
            TxtToDateOfOperationalKpi.Text = ToDateOfOperationalKpi
            TxtFromDateOfOperationalKpi.Enabled = True
            TxtToDateOfOperationalKpi.Enabled = True
            LblFromDateForOperationalKpi.Enabled = True
            BtnGoForOperationalKpi.Enabled = True
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)

        ElseIf Convert.ToString(DdllistForOperationalKpi.SelectedItem).Trim() = "Current Calendar Year" Then
            FromDateOfOperationalKpi = "01" & "-" & "Jan" & "-" & DateTime.Now.Year.ToString()
            'LastdayOfPpreviousMonth = DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year.ToString(), DateTime.Now.AddMonths(-1).Month)
            ToDateOfOperationalKpi = "31" & "-" & "Dec" & "-" & DateTime.Now.Year.ToString()
            TxtFromDateOfOperationalKpi.Text = FromDateOfOperationalKpi
            TxtToDateOfOperationalKpi.Text = ToDateOfOperationalKpi
            TxtFromDateOfOperationalKpi.Enabled = True
            TxtToDateOfOperationalKpi.Enabled = True
            LblFromDateForOperationalKpi.Enabled = True
            BtnGoForOperationalKpi.Enabled = True
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)

        ElseIf Convert.ToString(DdllistForOperationalKpi.SelectedItem).Trim() = "Last Calendar Year" Then
            FromDateOfOperationalKpi = "01" & "-" & "Jan" & "-" & DateTime.Now.AddYears(-1).Year.ToString()
            'LastdayOfPpreviousMonth = DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year.ToString(), DateTime.Now.AddMonths(-1).Month)
            ToDateOfOperationalKpi = "31" & "-" & "Dec" & "-" & DateTime.Now.AddYears(-1).Year.ToString()
            TxtFromDateOfOperationalKpi.Text = FromDateOfOperationalKpi
            TxtToDateOfOperationalKpi.Text = ToDateOfOperationalKpi
            TxtFromDateOfOperationalKpi.Enabled = True
            TxtToDateOfOperationalKpi.Enabled = True
            LblFromDateForOperationalKpi.Enabled = True
            BtnGoForOperationalKpi.Enabled = True
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)

        ElseIf Convert.ToString(DdllistForOperationalKpi.SelectedItem).Trim() = "Current Financial Year" Then
            FromDateOfOperationalKpi = "01" & "-" & "Apr" & "-" & DateTime.Now.Year.ToString()

            ToDateOfOperationalKpi = "31" & "-" & "Mar" & "-" & DateTime.Now.AddYears(1).Year.ToString()
            TxtFromDateOfOperationalKpi.Text = FromDateOfOperationalKpi
            TxtToDateOfOperationalKpi.Text = ToDateOfOperationalKpi
            TxtFromDateOfOperationalKpi.Enabled = True
            TxtToDateOfOperationalKpi.Enabled = True
            LblFromDateForOperationalKpi.Enabled = True
            BtnGoForOperationalKpi.Enabled = True
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)

        ElseIf Convert.ToString(DdllistForOperationalKpi.SelectedItem).Trim() = "Last Financial Year" Then
            FromDateOfOperationalKpi = "01" & "-" & "Apr" & "-" & DateTime.Now.AddYears(-1).Year.ToString()
            ToDateOfOperationalKpi = "31" & "-" & "Mar" & "-" & DateTime.Now.Year.ToString()
            TxtFromDateOfOperationalKpi.Text = FromDateOfOperationalKpi
            TxtToDateOfOperationalKpi.Text = ToDateOfOperationalKpi
            TxtFromDateOfOperationalKpi.Enabled = True
            TxtToDateOfOperationalKpi.Enabled = True
            LblFromDateForOperationalKpi.Enabled = True
            BtnGoForOperationalKpi.Enabled = True
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)

        End If
    End Sub
#End Region

#Region "Button Events"

    Protected Sub btnProjectforTrackingGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProjectforTrackingGo.Click

        Dim RedirectStr As String = ""
        RedirectStr = "window.open(""" + "frmProjectDetailMst.aspx?WorkSpaceId=" & Me.HProjectIdForTracking.Value.Trim() & "&Page=frmMyProject&Type=MONITORING&ParentName=frmMainPage" + """)"
        ScriptManager.RegisterClientScriptBlock(Me.Page(), Me.GetType(), "OpenWindow", RedirectStr, True)
        Me.TxtProjectNoPlainForTrackProjectStatus.Text = ""
    End Sub

    Protected Sub btnMediator_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMediator.Click
        Try

            If Not Me.HfProjectType.Value Is Nothing AndAlso
                   Convert.ToString(Me.HfProjectType.Value).Trim() <> "" Then

                If Convert.ToString(Me.HfProjectType.Value).Trim.ToUpper() = "CLIENTREQUESTPROJECTS" Then
                    BindGrid("gvwpnl0001")
                    Me.ViewState(VS_ProjectType) = "CLIENTREQUESTPROJECTS"
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divClientRequest');", True)

                ElseIf Convert.ToString(Me.HfProjectType.Value).Trim.ToUpper() = "PRECLINICALPROJECTS" Then
                    BindGrid("gvwpnl0013")
                    Me.ViewState(VS_ProjectType) = "PreClinicalProjects"
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectPreClinical');", True)
                ElseIf Convert.ToString(Me.HfProjectType.Value).Trim.ToUpper() = "CLINICALPHASEPROJECTS" Then
                    BindGrid("gvwpnl0003")
                    Me.ViewState(VS_ProjectType) = "CLINICALPHASEPROJECTS"
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divClinicalPhase');", True)
                ElseIf Convert.ToString(Me.HfProjectType.Value).Trim.ToUpper() = "ANALYTICALPROJECTS" Then
                    BindGrid("gvwpnl_Analysis")
                    Me.ViewState(VS_ProjectType) = "ANALYTICALPROJECTS"
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divAnalyticalphase');", True)
                    ''Added by DhruviShah
                ElseIf Convert.ToString(Me.HfProjectType.Value).Trim.ToUpper() = "NEWDEMO" Then
                    BindGrid("gvwpnl0003")
                    Me.ViewState(VS_ProjectType) = "NEWDEMO"
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divDemo');", True)

                ElseIf Convert.ToString(Me.HfProjectType.Value).Trim.ToUpper() = "NEWDEMO1" Then
                    BindGrid("gvwpnl0003")
                    Me.ViewState(VS_ProjectType) = "NEWDEMO1"
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divDemo1');", True)

                ElseIf Convert.ToString(Me.HfProjectType.Value).Trim.ToUpper() = "DCFMANAGEMENT" Then
                    BindGrid("gvwpnl0003")
                    Me.ViewState(VS_ProjectType) = "DCFMANAGEMENT"
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divDcfmanage');", True)
                ElseIf Convert.ToString(Me.HfProjectType.Value).Trim.ToUpper() = "DOCUMENTPROJECTS" Then
                    BindGrid("gvwpnl0004")
                    Me.ViewState(VS_ProjectType) = "DOCUMENTPROJECTS"
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divDocumentPhase');", True)
                ElseIf Convert.ToString(Me.HfProjectType.Value).Trim.ToUpper() = "SCREENINGANALYTIC" Then
                    'BindGrid("gvwpnl0004")
                    pnl0005.Visible = False
                    gvwpnlScreeningAnalytic.DataSource = Nothing
                    gvwpnlScreeningAnalytic.DataBind()

                    FillDropDownLists()
                    Me.ViewState(VS_ProjectType) = "SCREENINGANALYTIC"
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divScreeningAnalytic');", True)
                ElseIf Convert.ToString(Me.HfProjectType.Value).Trim.ToUpper() = "PROJECTSTUDYWORKSUMMARY" Then
                    BindDropDownOfStusyWorkSummary()
                    Me.ViewState(VS_ProjectType) = "PROJECTSTUDYWORKSUMMARY"
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectStudyWorkSummary');", True)

                ElseIf Convert.ToString(Me.HfProjectType.Value).Trim.ToUpper() = "OPERATIONALKPI" Then
                    FillOperationalKpiControls()
                    FillKpiWithCurrentDate()
                    getIntialDataForOperationalKpi()
                    Me.ViewState(VS_ProjectType) = "OPERATIONALKPI"
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)

                ElseIf Convert.ToString(Me.HfProjectType.Value).Trim.ToUpper() = "MYCALENDAR" Then
                    fillDeptDropDown()
                    fillActivityName()
                    fillDdlYear()
                    fillDropDownMonth()
                    setDropdownmonthwithcurrentmonthandyear()
                    setIntialCalendar()
                    'getIntialDataForOperationalKpi()
                    Me.ViewState(VS_ProjectType) = "MYCALENDAR"
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divMyCalendar');", True)

                ElseIf Convert.ToString(Me.HfProjectType.Value).Trim.ToUpper() = "VISITTRACKER" Then

                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divVisitTracker');", True)
                ElseIf Convert.ToString(Me.HfProjectType.Value).Trim.ToUpper() = "VISITSCHEDULER" Then

                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divVisitScheduler');", True)

                    ''Added By Vivek Patel
                ElseIf Convert.ToString(Me.HfProjectType.Value).Trim.ToUpper() = "SITEWISESUBJECTDETAIL" Then
                    BindGrid("gvwpnl0003")
                    Me.ViewState(VS_ProjectType) = "SiteWiseSubjectDetail"
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divSiteWiseSubjectDetail');", True)

                ElseIf Convert.ToString(Me.HfProjectType.Value).Trim.ToUpper() = "SITEINFORMATION" Then
                    BindGrid("gvwpnl0003")
                    Me.ViewState(VS_ProjectType) = "SiteInformation"
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divSiteInformation');", True)

                ElseIf Convert.ToString(Me.HfProjectType.Value).Trim.ToUpper() = "CRFSTATUS" Then
                    BindGrid("gvwpnl0003")
                    Me.ViewState(VS_ProjectType) = "CRFStatus"
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divCRFStatus');", True)

                ElseIf Convert.ToString(Me.HfProjectType.Value).Trim.ToUpper() = "AESAE" Then
                    BindGrid("gvwpnl0003")
                    Me.ViewState(VS_ProjectType) = "AESAE"
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divAESAE');", True)
                End If
                ''Completed By Vivek Patel

            End If


        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)
        End Try
    End Sub

    Protected Sub BtnGoFOrScreeningAnalytic_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim FirstDateOfCurrMonth As Date
        Dim CurrMonthLastDate As Integer
        Dim LastDateOfCurrMonth As Date
        Dim PrevMonthLastDate As Integer
        Dim FirstDateOfPrevMonth As Date
        Dim LastDateOfPrevMonth As Date
        Dim LastMonth As String = String.Empty
        Dim LastYear As String = String.Empty
        Dim ds As DataSet = Nothing
        Dim Parameters As String = String.Empty
        Dim eStr_Retu As String = String.Empty

        Try

            If DdllistMonth.SelectedItem.ToString = "Select..." Then
                objCommon.ShowAlert("Enter the medatory field", Me.Page())
                DdllistMonth.Focus()
            End If

            If DdllistYear.SelectedItem.ToString = "select..." Then
                objCommon.ShowAlert("Enter the medatory field", Me.Page())
                DdllistYear.Focus()
            End If
            FirstDateOfCurrMonth = CDate("01" & "-" & DdllistMonth.SelectedItem.ToString().Trim() & "-" & DdllistYear.SelectedItem.ToString())
            CurrMonthLastDate = CInt(DateTime.DaysInMonth(DdllistYear.SelectedItem.ToString(), CInt(DdllistMonth.SelectedValue)))
            LastDateOfCurrMonth = CDate(CurrMonthLastDate & "-" & DdllistMonth.SelectedItem.ToString().Trim() & "-" & DdllistYear.SelectedItem.ToString())
            LastMonth = MonthName(CInt(CDate(FirstDateOfCurrMonth).AddMonths(-1).Month))
            LastYear = CInt(CDate(FirstDateOfCurrMonth).AddMonths(-1).Year.ToString())
            FirstDateOfPrevMonth = CDate("01" & "-" & LastMonth.ToString.Trim() & "-" & LastYear.ToString().Trim())
            PrevMonthLastDate = CInt(DateTime.DaysInMonth(CInt(LastYear), CInt(CDate(FirstDateOfPrevMonth).Month)))
            LastDateOfPrevMonth = CDate(PrevMonthLastDate & "-" & LastMonth.ToString.Trim() & "-" & LastYear.ToString.Trim())

            Parameters = Format(FirstDateOfCurrMonth, "dd/MMM/yyyy") + "##" + Format(LastDateOfCurrMonth, "dd/MMM/yyyy") + "##" + Format(FirstDateOfPrevMonth, "dd/MMM/yyyy") + "##" + Format(LastDateOfPrevMonth, "dd/MMM/yyyy") + "##" + ddlLocation.SelectedValue()

            If Not Me.objHelpDbTable.Proc_ScreeningAnalyticRatio(Parameters, ds, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If
            pnl0005.Visible = True
            If ds.Tables(0).Rows.Count > 0 Then

                gvwpnlScreeningAnalytic.DataSource = ds
                gvwpnlScreeningAnalytic.DataBind()

            End If

            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divScreeningAnalytic');", True)
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Processing Screening Analytics. ", ex.Message)
        End Try

    End Sub

    Protected Sub BtnGoForStudyWorkSummary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGoForStudyWorkSummary.Click
        Dim Estr As String = String.Empty
        Dim SelectedActivityId As String = String.Empty
        Dim Month As String = String.Empty
        Dim LastMonth As String = String.Empty
        Dim LastdayOfPpreviousMonth As String = String.Empty
        Dim FromDate As String = String.Empty
        Dim ToDate As String = String.Empty
        Dim dv_ClientNameforWorkSummary As DataView
        Dim dv_ProjectTypeforWorkSummary As DataView
        Dim ds_WorkSummary As New DataSet

        Dim TotalProjForES As Integer = 0
        Dim TotalProjForSA As Integer = 0
        Dim AvgTimeForES As String = String.Empty
        Dim AvgTimeForSA As String = String.Empty
        Dim OnTimeForES As Integer = 0
        Dim OnTimeForSA As Integer = 0
        Dim DelayedForES As Integer = 0
        Dim DelayedForSA As Integer = 0
        Dim iForLoop As Integer
        Dim StdTimeForES As Integer = 28
        Dim StdTimeForSA As Integer = 14
        Dim datedifference As Integer
        Dim dateifferenceforSA As Integer
        Dim datedifferenceforES As Integer
        Dim yValues(1) As Double
        Dim xValues(1) As String
        Dim xValuesForSA(1) As String
        Dim yValuesForSA(1) As Double
        Dim StdDaysFromES As Integer
        Dim StdDaysFromESReportDispatch As Integer
        Dim StdDaysFromSA As Integer
        Dim ProjectListForClinicDateNotEntered As String = String.Empty
        Dim ProjectListForSampleAnalysisNotEntered As String = String.Empty
        Dim TotalTimeForES As Integer
        Dim TotalTimeForSA As Integer
        Dim totalCountForNotEnteredClinicDateES As Integer
        Dim totalCountForNotEnteredClinicDatesa As Integer
        Dim CountForAvgForES As Integer
        Dim CountForAvgForSA As Integer

        Try

            StdDaysFromES = CInt(TxtStdTimeForES.Text)
            StdDaysFromSA = CInt(TxtStdTimeForSA.Text)
            StdDaysFromESReportDispatch = CInt(TxtStdForESinReportDispatch.Text)

            If DdllistFilterOn.SelectedItem.ToString() = "Clinic End Date" And RdbtnListForProjectStudyWorkSummary.SelectedIndex = 1 Then
                Me.objCommon.ShowAlert("Data InSufficient For getting Summary Of Clinic End Date", Me.Page)
                pnlChartForWorkSummary1.Visible = False
                DdllistFilterOn.SelectedIndex = 0
                DdllistReportFor.SelectedIndex = 0
                TxtFromDate.Text = String.Empty
                TxtToDate.Text = String.Empty
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectStudyWorkSummary');", True)
                Exit Sub
            End If

            If DdllistFilterOn.SelectedItem.ToString() = "Select..." And DdllistReportFor.SelectedItem.ToString() = "Select..." Then
                Me.objCommon.ShowAlert("Select Filter On And Report For Option", Me.Page)
                TxtFromDate.Text = String.Empty
                TxtToDate.Text = String.Empty
                pnlChartForWorkSummary1.Visible = False
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectStudyWorkSummary');", True)
                Exit Sub
            ElseIf DdllistFilterOn.SelectedItem.ToString() = "Select..." And DdllistReportFor.SelectedItem.ToString() <> "Select..." Then
                Me.objCommon.ShowAlert("Select Report For Option", Me.Page)
                pnlChartForWorkSummary1.Visible = False
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectStudyWorkSummary');", True)
                Exit Sub
            End If

            If DdllistFilterOn.SelectedItem.ToString() = "Clinic End Date" Then
                SelectedActivityId = "1090"
            ElseIf DdllistFilterOn.SelectedItem.ToString() = "Sample Analysis End Date" Then
                SelectedActivityId = "1076"
            ElseIf DdllistFilterOn.SelectedItem.ToString() = "Report Sent To Sponsor Date" Then
                SelectedActivityId = "1417"
            ElseIf DdllistFilterOn.SelectedItem.ToString() = "Report Dispatch Date" Then
                SelectedActivityId = "1418"
            End If

            FromDate = TxtFromDate.Text
            ToDate = TxtToDate.Text


            If Not objHelpDbTable.Proc_GetProjectStudyWorkSummaryDetails(SelectedActivityId, FromDate, ToDate, ds_WorkSummary, Estr) Then
                Me.objCommon.ShowAlert("Error While Getting Data for ProjectStudyWorkSummary", Me.Page)
                Exit Sub

            End If
            If ddlProjectManager.SelectedIndex > 0 Then
                Dim dv As DataView
                dv = ds_WorkSummary.Tables(0).DefaultView
                dv.RowFilter = "iProjectManagerId =" + ddlProjectManager.SelectedValue
                ds_WorkSummary = Nothing
                ds_WorkSummary = New DataSet()
                ds_WorkSummary.Tables().Add(dv.ToTable.DefaultView.Table())
            End If
            If RdbtnListForProjectStudyWorkSummary.SelectedValue = "" Then
                Me.objCommon.ShowAlert("Select either Details Or Summary", Me.Page)
                DdllistFilterOn.SelectedIndex = 0
                DdllistReportFor.SelectedIndex = 0
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectStudyWorkSummary');", True)
                Exit Sub

            ElseIf RdbtnListForProjectStudyWorkSummary.SelectedValue = "0" Then
                If Not ds_WorkSummary Is Nothing Then

                    If ds_WorkSummary.Tables(0).Rows.Count < 1 Then
                        pnl0006.Visible = True
                        GvwPnlProjectStudyWorkSummary.Visible = False
                        BtnExportToExcelForProjectStudyWorkSummary.Visible = False
                        LblNoRecordFound.Visible = True
                        LblNoRecordFound.Text = "No Record Found."
                        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectStudyWorkSummary');", True)
                        Exit Sub
                    End If

                End If

                If ds_WorkSummary Is Nothing Then
                    Me.objCommon.ShowAlert("Data Not Available For Project Study/Work Summary.", Me.Page)
                    Exit Sub
                End If

                LblNoRecordFound.Visible = False
                pnl0006.Visible = True
                pnlChartForWorkSummary1.Visible = False
                DdllistAllSponsor.Visible = True
                DdllistAllProjectType.Visible = True
                DdllistPilotPivotal.Visible = True
                ViewState("VS_GvwPnlProjectStudyWorkSummary_PageIndexChanging") = ds_WorkSummary.Tables(0)
                BtnExportToExcelForProjectStudyWorkSummary.Visible = True
                Me.GvwPnlProjectStudyWorkSummary.DataSource = ds_WorkSummary.Tables(0)
                Me.GvwPnlProjectStudyWorkSummary.DataBind()
                Me.GvwPnlProjectStudyWorkSummary.Visible = True

                dv_ClientNameforWorkSummary = ds_WorkSummary.Tables(0).DefaultView.ToTable(True, "vClientCode", "vClientName").DefaultView()
                dv_ClientNameforWorkSummary.Sort = "vClientName"
                DdllistAllSponsor.DataSource = dv_ClientNameforWorkSummary.ToTable()
                DdllistAllSponsor.DataTextField = "vClientName"
                DdllistAllSponsor.DataValueField = "vClientCode"
                DdllistAllSponsor.DataBind()
                DdllistAllSponsor.Items.Insert(0, "All Sponsor")

                dv_ProjectTypeforWorkSummary = ds_WorkSummary.Tables(0).DefaultView.ToTable(True, "vProjectTypeName").DefaultView()
                dv_ProjectTypeforWorkSummary.Sort = "vProjectTypeName"
                DdllistAllProjectType.DataSource = dv_ProjectTypeforWorkSummary
                DdllistAllProjectType.DataTextField = "vProjectTypeName"
                DdllistAllProjectType.DataBind()
                DdllistAllProjectType.Items.Insert(0, "All Project Type")
                DdllistAllSponsor.Enabled = True
                DdllistAllProjectType.Enabled = True
                DdllistPilotPivotal.Enabled = True
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectStudyWorkSummary');", True)
                pnl0006.Visible = True
                pnlChartForWorkSummary1.Visible = False

            ElseIf RdbtnListForProjectStudyWorkSummary.SelectedValue = "1" Then
                '' for summary work


                If DdllistFilterOn.SelectedItem.ToString() = "Sample Analysis End Date" Then
                    OnTimeProjectListOfES = ""
                    OnTimeProjectListOfSA = ""
                    DelayedProjectListOfES = ""
                    DelayedProjectListOfSA = ""
                    LblProjectNoPopUpWorkSummary.Text = ""
                    TotalProjForES = ds_WorkSummary.Tables(0).Rows.Count
                    For iForLoop = 0 To ds_WorkSummary.Tables(0).Rows.Count - 1
                        If ds_WorkSummary.Tables(0).Rows(iForLoop).Item("ClinicEndDate").ToString() <> "" Then

                            LblProjectNoPopUpWorkSummary.Text += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine
                            datedifference = DateDiff(DateInterval.Day, Convert.ToDateTime(ds_WorkSummary.Tables(0).Rows(iForLoop).Item("ClinicEndDate").ToString()), Convert.ToDateTime(ds_WorkSummary.Tables(0).Rows(iForLoop).Item("SampleAnalysisEndDate").ToString()))
                            TotalTimeForES += datedifference
                            If datedifference <= StdDaysFromES Then
                                OnTimeForES += 1
                                OnTimeProjectListOfES += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine
                            Else
                                DelayedForES += 1
                                DelayedProjectListOfES += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine
                            End If

                        Else
                            If iForLoop <> ds_WorkSummary.Tables(0).Rows.Count - 1 Then

                                totalCountForNotEnteredClinicDateES += 1
                                ProjectListForClinicDateNotEntered += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo") & "," & vbNewLine
                            Else
                                totalCountForNotEnteredClinicDateES += 1
                                ProjectListForClinicDateNotEntered += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo")
                            End If

                        End If


                    Next
                    ViewState("OnTimeForESValue") = ""
                    ViewState("OnTimeForSAValue") = ""
                    ViewState("DelayedForESValue") = ""
                    ViewState("DelayedForSAValue") = ""

                    ViewState("OnTimeForESValue") = OnTimeForES
                    ViewState("OnTimeForSAValue") = OnTimeForSA
                    ViewState("DelayedForESValue") = DelayedForES
                    ViewState("DelayedForSAValue") = DelayedForSA

                    xValues(0) = "On Time :" & OnTimeForES
                    xValues(1) = "Delayed :" & DelayedForES
                    yValues(0) = OnTimeForES
                    yValues(1) = DelayedForES

                    Chart1.Series("Default").Points.DataBindXY(xValues, yValues)
                    Chart1.Series("Default").ChartType = DataVisualization.Charting.SeriesChartType.Doughnut
                    Chart1.Series("Default")("PieLabelStyle") = "OutSide"
                    Chart1.BackGradientStyle = GradientStyle.HorizontalCenter
                    Chart1.BackImageTransparentColor = Color.Blue
                    Chart1.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
                    Chart1.Titles("Title1").Text = "From End Study"

                    Chart1.Visible = True
                    Chart1.Width = "250"
                    Chart1.Height = "170"

                    Chart2.Visible = False
                    ViewState("TotalProjectList") = LblProjectNoPopUpWorkSummary.Text
                    ViewState("DelayedProjectListOfES") = DelayedProjectListOfES
                    ViewState("OnTimeProjectListOfES") = OnTimeProjectListOfES
                    ViewState("SelectedActivity") = ""
                    ViewState("SelectedActivity") = "SA"



                ElseIf DdllistFilterOn.SelectedItem.ToString() = "Report Sent To Sponsor Date" Then
                    OnTimeProjectListOfES = ""
                    OnTimeProjectListOfSA = ""
                    DelayedProjectListOfES = ""
                    DelayedProjectListOfSA = ""
                    LblProjectNoPopUpWorkSummary.Text = ""
                    TotalProjForSA = ds_WorkSummary.Tables(0).Rows.Count
                    For iForLoop = 0 To ds_WorkSummary.Tables(0).Rows.Count - 1

                        If ds_WorkSummary.Tables(0).Rows(iForLoop).Item("ClinicEndDate").ToString() <> "" And ds_WorkSummary.Tables(0).Rows(iForLoop).Item("SampleAnalysisEndDate").ToString() <> "" And ds_WorkSummary.Tables(0).Rows(iForLoop).Item("SponsorReportDate").ToString() <> "" Then
                            LblProjectNoPopUpWorkSummary.Text += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine
                            datedifferenceforES = DateDiff(DateInterval.Day, Convert.ToDateTime(ds_WorkSummary.Tables(0).Rows(iForLoop).Item("ClinicEndDate").ToString()), Convert.ToDateTime(ds_WorkSummary.Tables(0).Rows(iForLoop).Item("SponsorReportDate").ToString()))
                            TotalTimeForES += datedifferenceforES
                            If datedifferenceforES <= StdDaysFromESReportDispatch Then
                                OnTimeForES += 1
                                OnTimeProjectListOfES += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine
                            Else
                                DelayedForES += 1
                                DelayedProjectListOfES += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine
                            End If

                            dateifferenceforSA = DateDiff(DateInterval.Day, Convert.ToDateTime(ds_WorkSummary.Tables(0).Rows(iForLoop).Item("SampleAnalysisEndDate").ToString()), Convert.ToDateTime(ds_WorkSummary.Tables(0).Rows(iForLoop).Item("SponsorReportDate").ToString()))
                            TotalTimeForSA += dateifferenceforSA
                            If dateifferenceforSA <= StdDaysFromSA Then
                                OnTimeForSA += 1
                                OnTimeProjectListOfSA += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine
                            Else
                                DelayedForSA += 1
                                DelayedProjectListOfSA += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine
                            End If


                        ElseIf ds_WorkSummary.Tables(0).Rows(iForLoop).Item("ClinicEndDate").ToString() = "" And ds_WorkSummary.Tables(0).Rows(iForLoop).Item("SampleAnalysisEndDate").ToString() <> "" And ds_WorkSummary.Tables(0).Rows(iForLoop).Item("SponsorReportDate").ToString() <> "" Then
                            LblProjectNoPopUpWorkSummary.Text += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine
                            ProjectListForClinicDateNotEntered += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString() & "," & vbNewLine
                            totalCountForNotEnteredClinicDateES += 1
                            dateifferenceforSA = DateDiff(DateInterval.Day, Convert.ToDateTime(ds_WorkSummary.Tables(0).Rows(iForLoop).Item("SampleAnalysisEndDate").ToString()), Convert.ToDateTime(ds_WorkSummary.Tables(0).Rows(iForLoop).Item("SponsorReportDate").ToString()))
                            TotalTimeForSA += dateifferenceforSA
                            If dateifferenceforSA <= StdDaysFromSA Then
                                OnTimeForSA += 1
                                OnTimeProjectListOfSA += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine
                            Else
                                DelayedForSA += 1
                                DelayedProjectListOfSA += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine

                            End If


                        ElseIf ds_WorkSummary.Tables(0).Rows(iForLoop).Item("ClinicEndDate").ToString() <> "" And ds_WorkSummary.Tables(0).Rows(iForLoop).Item("SampleAnalysisEndDate").ToString() = "" And ds_WorkSummary.Tables(0).Rows(iForLoop).Item("SponsorReportDate").ToString() <> "" Then
                            LblProjectNoPopUpWorkSummary.Text += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine
                            datedifferenceforES = DateDiff(DateInterval.Day, Convert.ToDateTime(ds_WorkSummary.Tables(0).Rows(iForLoop).Item("ClinicEndDate").ToString()), Convert.ToDateTime(ds_WorkSummary.Tables(0).Rows(iForLoop).Item("SponsorReportDate").ToString()))
                            TotalTimeForES += datedifferenceforES
                            If datedifferenceforES <= StdDaysFromESReportDispatch Then
                                OnTimeForES += 1
                                OnTimeProjectListOfES += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine
                            Else
                                DelayedForES += 1
                                DelayedProjectListOfES += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine
                            End If
                            totalCountForNotEnteredClinicDatesa += 1
                            ProjectListForSampleAnalysisNotEntered += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString() & "," & vbNewLine


                        ElseIf ds_WorkSummary.Tables(0).Rows(iForLoop).Item("ClinicEndDate").ToString() = "" And ds_WorkSummary.Tables(0).Rows(iForLoop).Item("SampleAnalysisEndDate").ToString() = "" Then
                            LblProjectNoPopUpWorkSummary.Text += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine
                            totalCountForNotEnteredClinicDateES += 1
                            totalCountForNotEnteredClinicDatesa += 1

                            ProjectListForSampleAnalysisNotEntered += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString() & "," & vbNewLine
                            ProjectListForClinicDateNotEntered += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString() & "," & vbNewLine

                        End If
                    Next
                    ViewState("OnTimeForESValue") = ""
                    ViewState("OnTimeForSAValue") = ""
                    ViewState("DelayedForESValue") = ""
                    ViewState("DelayedForSAValue") = ""

                    ViewState("OnTimeForESValue") = OnTimeForES
                    ViewState("OnTimeForSAValue") = OnTimeForSA
                    ViewState("DelayedForESValue") = DelayedForES
                    ViewState("DelayedForSAValue") = DelayedForSA

                    xValues(0) = "On Time :" & OnTimeForES
                    xValues(1) = "Delayed :" & DelayedForES
                    xValuesForSA(0) = "On Time :" & OnTimeForSA
                    xValuesForSA(1) = "Delayed :" & DelayedForSA
                    yValues(0) = OnTimeForES
                    yValues(1) = DelayedForES

                    yValuesForSA(0) = OnTimeForSA
                    yValuesForSA(1) = DelayedForSA

                    Chart1.Series("Default").Points.DataBindXY(xValues, yValues)
                    Chart1.Series("Default").ChartType = DataVisualization.Charting.SeriesChartType.Doughnut
                    Chart1.Series("Default")("PieLabelStyle") = "OutSide"
                    Chart1.BackGradientStyle = GradientStyle.HorizontalCenter
                    Chart1.BackImageTransparentColor = Color.Blue
                    Chart1.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
                    Chart1.Titles("Title1").Text = "From End Study"
                    Chart1.Visible = True
                    Chart1.Width = "250"
                    Chart1.Height = "170"

                    Chart2.Series("Default").Points.DataBindXY(xValuesForSA, yValuesForSA)
                    Chart2.Series("Default").ChartType = DataVisualization.Charting.SeriesChartType.Doughnut
                    Chart2.Series("Default")("PieLabelStyle") = "OutSide"

                    Chart2.BackGradientStyle = GradientStyle.HorizontalCenter
                    Chart2.BackImageTransparentColor = Color.Blue
                    Chart2.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
                    Chart2.Titles("Title1").Text = "From Sample Analysis"
                    Chart2.Visible = True
                    Chart2.Width = "250"
                    Chart2.Height = "170"
                    ViewState("TotalProjectList") = LblProjectNoPopUpWorkSummary.Text
                    ViewState("DelayedProjectListOfES") = DelayedProjectListOfES
                    ViewState("OnTimeProjectListOfES") = OnTimeProjectListOfES
                    ViewState("DelayedProjectListOfSA") = DelayedProjectListOfSA
                    ViewState("OnTimeProjectListOfSA") = OnTimeProjectListOfSA
                    ViewState("SelectedActivity") = ""
                    ViewState("SelectedActivity") = "RP"
                ElseIf DdllistFilterOn.SelectedItem.ToString() = "Report Dispatch Date" Then
                    OnTimeProjectListOfES = ""
                    OnTimeProjectListOfSA = ""
                    DelayedProjectListOfES = ""
                    DelayedProjectListOfSA = ""
                    LblProjectNoPopUpWorkSummary.Text = ""
                    TotalProjForSA = ds_WorkSummary.Tables(0).Rows.Count
                    For iForLoop = 0 To ds_WorkSummary.Tables(0).Rows.Count - 1

                        If ds_WorkSummary.Tables(0).Rows(iForLoop).Item("ClinicEndDate").ToString() <> "" And ds_WorkSummary.Tables(0).Rows(iForLoop).Item("SampleAnalysisEndDate").ToString() <> "" And ds_WorkSummary.Tables(0).Rows(iForLoop).Item("ReportDispatchDate").ToString() <> "" Then
                            LblProjectNoPopUpWorkSummary.Text += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine
                            datedifferenceforES = DateDiff(DateInterval.Day, Convert.ToDateTime(ds_WorkSummary.Tables(0).Rows(iForLoop).Item("ClinicEndDate").ToString()), Convert.ToDateTime(ds_WorkSummary.Tables(0).Rows(iForLoop).Item("ReportDispatchDate").ToString()))
                            TotalTimeForES += datedifferenceforES
                            If datedifferenceforES <= StdDaysFromESReportDispatch Then
                                OnTimeForES += 1
                                OnTimeProjectListOfES += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine
                            Else
                                DelayedForES += 1
                                DelayedProjectListOfES += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine
                            End If

                            dateifferenceforSA = DateDiff(DateInterval.Day, Convert.ToDateTime(ds_WorkSummary.Tables(0).Rows(iForLoop).Item("SampleAnalysisEndDate").ToString()), Convert.ToDateTime(ds_WorkSummary.Tables(0).Rows(iForLoop).Item("ReportDispatchDate").ToString()))
                            TotalTimeForSA += dateifferenceforSA
                            If dateifferenceforSA <= StdDaysFromSA Then
                                OnTimeForSA += 1
                                OnTimeProjectListOfSA += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine
                            Else
                                DelayedForSA += 1
                                DelayedProjectListOfSA += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine
                            End If


                        ElseIf ds_WorkSummary.Tables(0).Rows(iForLoop).Item("ClinicEndDate").ToString() = "" And ds_WorkSummary.Tables(0).Rows(iForLoop).Item("SampleAnalysisEndDate").ToString() <> "" And ds_WorkSummary.Tables(0).Rows(iForLoop).Item("ReportDispatchDate").ToString() <> "" Then
                            LblProjectNoPopUpWorkSummary.Text += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine
                            ProjectListForClinicDateNotEntered += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString() & "," & vbNewLine
                            totalCountForNotEnteredClinicDateES += 1
                            dateifferenceforSA = DateDiff(DateInterval.Day, Convert.ToDateTime(ds_WorkSummary.Tables(0).Rows(iForLoop).Item("SampleAnalysisEndDate").ToString()), Convert.ToDateTime(ds_WorkSummary.Tables(0).Rows(iForLoop).Item("ReportDispatchDate").ToString()))
                            TotalTimeForSA += dateifferenceforSA
                            If dateifferenceforSA <= StdDaysFromSA Then
                                OnTimeForSA += 1
                                OnTimeProjectListOfSA += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine
                            Else
                                DelayedForSA += 1
                                DelayedProjectListOfSA += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine

                            End If


                        ElseIf ds_WorkSummary.Tables(0).Rows(iForLoop).Item("ClinicEndDate").ToString() <> "" And ds_WorkSummary.Tables(0).Rows(iForLoop).Item("SampleAnalysisEndDate").ToString() = "" And ds_WorkSummary.Tables(0).Rows(iForLoop).Item("ReportDispatchDate").ToString() <> "" Then
                            LblProjectNoPopUpWorkSummary.Text += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine
                            datedifferenceforES = DateDiff(DateInterval.Day, Convert.ToDateTime(ds_WorkSummary.Tables(0).Rows(iForLoop).Item("ClinicEndDate").ToString()), Convert.ToDateTime(ds_WorkSummary.Tables(0).Rows(iForLoop).Item("ReportDispatchDate").ToString()))
                            TotalTimeForES += datedifferenceforES
                            If datedifferenceforES <= StdDaysFromESReportDispatch Then
                                OnTimeForES += 1
                                OnTimeProjectListOfES += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine
                            Else
                                DelayedForES += 1
                                DelayedProjectListOfES += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine
                            End If
                            totalCountForNotEnteredClinicDatesa += 1
                            ProjectListForSampleAnalysisNotEntered += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString() & "," & vbNewLine


                        ElseIf ds_WorkSummary.Tables(0).Rows(iForLoop).Item("ClinicEndDate").ToString() = "" And ds_WorkSummary.Tables(0).Rows(iForLoop).Item("SampleAnalysisEndDate").ToString() = "" Then
                            LblProjectNoPopUpWorkSummary.Text += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString().Trim() + "," + vbNewLine
                            totalCountForNotEnteredClinicDateES += 1
                            totalCountForNotEnteredClinicDatesa += 1

                            ProjectListForSampleAnalysisNotEntered += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString() & "," & vbNewLine
                            ProjectListForClinicDateNotEntered += ds_WorkSummary.Tables(0).Rows(iForLoop).Item("vProjectNo").ToString() & "," & vbNewLine

                        End If
                    Next
                    ViewState("OnTimeForESValue") = ""
                    ViewState("OnTimeForSAValue") = ""
                    ViewState("DelayedForESValue") = ""
                    ViewState("DelayedForSAValue") = ""

                    ViewState("OnTimeForESValue") = OnTimeForES
                    ViewState("OnTimeForSAValue") = OnTimeForSA
                    ViewState("DelayedForESValue") = DelayedForES
                    ViewState("DelayedForSAValue") = DelayedForSA

                    xValues(0) = "On Time :" & OnTimeForES
                    xValues(1) = "Delayed :" & DelayedForES
                    xValuesForSA(0) = "On Time :" & OnTimeForSA
                    xValuesForSA(1) = "Delayed :" & DelayedForSA
                    yValues(0) = OnTimeForES
                    yValues(1) = DelayedForES

                    yValuesForSA(0) = OnTimeForSA
                    yValuesForSA(1) = DelayedForSA

                    Chart1.Series("Default").Points.DataBindXY(xValues, yValues)
                    Chart1.Series("Default").ChartType = DataVisualization.Charting.SeriesChartType.Doughnut
                    Chart1.Series("Default")("PieLabelStyle") = "OutSide"
                    Chart1.BackGradientStyle = GradientStyle.HorizontalCenter
                    Chart1.BackImageTransparentColor = Color.Blue
                    Chart1.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
                    Chart1.Titles("Title1").Text = "From End Study"
                    Chart1.Visible = True
                    Chart1.Width = "250"
                    Chart1.Height = "170"

                    Chart2.Series("Default").Points.DataBindXY(xValuesForSA, yValuesForSA)
                    Chart2.Series("Default").ChartType = DataVisualization.Charting.SeriesChartType.Doughnut
                    Chart2.Series("Default")("PieLabelStyle") = "OutSide"

                    Chart2.BackGradientStyle = GradientStyle.HorizontalCenter
                    Chart2.BackImageTransparentColor = Color.Blue
                    Chart2.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
                    Chart2.Titles("Title1").Text = "From Sample Analysis"
                    Chart2.Visible = True
                    Chart2.Width = "250"
                    Chart2.Height = "170"
                    ViewState("TotalProjectList") = LblProjectNoPopUpWorkSummary.Text
                    ViewState("DelayedProjectListOfES") = DelayedProjectListOfES
                    ViewState("OnTimeProjectListOfES") = OnTimeProjectListOfES
                    ViewState("DelayedProjectListOfSA") = DelayedProjectListOfSA
                    ViewState("OnTimeProjectListOfSA") = OnTimeProjectListOfSA
                    ViewState("SelectedActivity") = ""
                    ViewState("SelectedActivity") = "RP"
                End If

                If DdllistFilterOn.SelectedItem.ToString() = "Sample Analysis End Date" Then


                    CountForAvgForES = TotalProjForES - totalCountForNotEnteredClinicDateES
                    LblForStdTimeOfESHeadingInWorkSummary.Text = "Standard Time From End Study"
                    LblForStdTimeOfESHeadingInWorkSummary.Visible = True
                    TxtStdTimeForES.Visible = True
                    LblForDaysESinWorkSummary.Text = "Days"
                    LblForDaysESinWorkSummary.Visible = True
                    BtnChangeForESInWorkSummary.Visible = True
                    LblProjects.Text = "# Projects : "
                    LblProjects.Visible = True
                    LnkBtnProjectsCountsForES.Text = TotalProjForES
                    LnkBtnProjectsCountsForES.Visible = True

                    LblAverageTimeFromES.Text = "# Average Time From End Study :"
                    LblAverageTimeFromES.Visible = True
                    LblAverageTimeCountsForES.Text = (CInt(TotalTimeForES) / CInt(CountForAvgForES)).ToString()
                    If LblAverageTimeCountsForES.Text.Length > 5 Then
                        LblAverageTimeCountsForES.Text = LblAverageTimeCountsForES.Text.ToString.Substring(0, 4)
                    End If
                    LblAverageTimeCountsForES.Visible = True
                    LblAverageTimeFromSA.Visible = False
                    LblAverageTimeCountsForSA.Visible = False
                    LblOnTimeFromES.Text = "# On Time From End Study : "
                    LblOnTimeFromES.Visible = True

                    LnkBtnOnTimeCountsForES.Text = OnTimeForES
                    LnkBtnOnTimeCountsForES.Visible = True

                    LblDelayedFromES.Text = "# Delayed From End Study:"
                    LblDelayedFromES.Visible = True

                    LnkBtnDelayedCountsForES.Text = DelayedForES
                    LnkBtnDelayedCountsForES.Visible = True

                    LblOnTimeFromSA.Visible = False
                    LnkBtnOnTimeCountsForSA.Visible = False
                    LblDelayedFromSA.Visible = False
                    LnkBtnDelayedCountsForSA.Visible = False
                    LblListOfProjectForSA.Visible = False
                    LblListOfProjectNoForSA.Visible = False
                    LblListOfProjectForES.Text = " * Project List Whose Clinic End Date Is Not Ended : "
                    LblListOfProjectForES.Visible = True
                    LblListOfProjectNoForES.Text = ProjectListForClinicDateNotEntered
                    LblListOfProjectNoForES.Visible = True
                    If Not LblListOfProjectNoForES.Text = String.Empty Then
                        If LblListOfProjectNoForES.Text.Substring(LblListOfProjectNoForES.Text.Length - 3) = "," & vbCrLf Then
                            LblListOfProjectNoForES.Text = LblListOfProjectNoForES.Text.Substring(0, LblListOfProjectNoForES.Text.Length - 3).ToString()
                            ImageForES.Visible = True
                        End If
                    Else
                        LblListOfProjectNoForES.Text = ""
                        LblListOfProjectNoForES.Visible = False
                        ImageForES.Visible = False
                    End If

                    TxtStdForESinReportDispatch.Visible = False
                    TxtStdTimeForES.Visible = True
                    LblForStdTimeOfSAHeadingInWorkSummary.Visible = False
                    LblForDaysSAinWorkSummary.Visible = False
                    TxtStdTimeForSA.Visible = False
                    ImageForSA.Visible = False
                    pnlChartForWorkSummary1.Visible = True
                    pnl0006.Visible = False



                ElseIf DdllistFilterOn.SelectedItem.ToString() = "Report Sent To Sponsor Date" Then
                    LblForStdTimeOfESHeadingInWorkSummary.Text = "Standard Time From End Study"
                    LblForStdTimeOfESHeadingInWorkSummary.Visible = True
                    TxtStdTimeForES.Visible = True
                    LblForDaysESinWorkSummary.Text = "Days"
                    LblForDaysESinWorkSummary.Visible = True
                    BtnChangeForESInWorkSummary.Visible = True
                    LblForStdTimeOfSAHeadingInWorkSummary.Text = "Standard Time From Sample Analysis"
                    LblForStdTimeOfSAHeadingInWorkSummary.Visible = True
                    TxtStdTimeForSA.Visible = True
                    LblForDaysSAinWorkSummary.Text = "Days"
                    LblForDaysSAinWorkSummary.Visible = True
                    LblProjects.Text = "# Projects : "
                    LblProjects.Visible = True
                    LnkBtnProjectsCountsForES.Text = TotalProjForSA
                    LnkBtnProjectsCountsForES.Visible = True

                    LblAverageTimeFromES.Text = "# Average Time From End Study :"
                    LblAverageTimeFromES.Visible = True

                    CountForAvgForES = TotalProjForSA - totalCountForNotEnteredClinicDateES
                    LblAverageTimeCountsForES.Text = (CInt(TotalTimeForES) / CInt(CountForAvgForES)).ToString()
                    If LblAverageTimeCountsForES.Text.Length > 5 Then
                        LblAverageTimeCountsForES.Text = LblAverageTimeCountsForES.Text.ToString().Substring(0, 4)

                    End If
                    LblAverageTimeCountsForES.Visible = True
                    LblAverageTimeFromSA.Text = "# Average Time From Sample Analysis"
                    LblAverageTimeFromSA.Visible = True
                    CountForAvgForSA = TotalProjForSA - totalCountForNotEnteredClinicDatesa
                    LblAverageTimeCountsForSA.Text = (CInt(TotalTimeForSA) / CInt(CountForAvgForSA)).ToString()
                    If LblAverageTimeCountsForSA.Text.Length > 5 Then
                        LblAverageTimeCountsForSA.Text = LblAverageTimeCountsForSA.Text.ToString().Substring(0, 4)
                    End If
                    LblAverageTimeCountsForSA.Visible = True

                    LblOnTimeFromES.Text = "# On Time From End Study: "
                    LblOnTimeFromES.Visible = True

                    LnkBtnOnTimeCountsForES.Text = OnTimeForES
                    LnkBtnOnTimeCountsForES.Visible = True

                    LblDelayedFromES.Text = "# Delayed From End Study:"
                    LblDelayedFromES.Visible = True

                    LnkBtnDelayedCountsForES.Text = DelayedForES
                    LnkBtnDelayedCountsForES.Visible = True

                    LblOnTimeFromSA.Text = "# On Time From Sample Analysis: "
                    LblOnTimeFromSA.Visible = True

                    LnkBtnOnTimeCountsForSA.Text = OnTimeForSA
                    LnkBtnOnTimeCountsForSA.Visible = True

                    LblDelayedFromSA.Text = "# Delayed From Sample Analysis:"
                    LblDelayedFromSA.Visible = True

                    LnkBtnDelayedCountsForSA.Text = DelayedForSA
                    LnkBtnDelayedCountsForSA.Visible = True

                    LblListOfProjectForES.Text = "    * Project List Whose Clinic End Date Is Not Ended : "
                    LblListOfProjectForES.Visible = True

                    LblListOfProjectNoForES.Text = ProjectListForClinicDateNotEntered
                    LblListOfProjectNoForES.Visible = True
                    If Not LblListOfProjectNoForES.Text = String.Empty Then
                        If LblListOfProjectNoForES.Text.Substring(LblListOfProjectNoForES.Text.Length - 3) = "," & vbCrLf Then
                            LblListOfProjectNoForES.Text = LblListOfProjectNoForES.Text.Substring(0, LblListOfProjectNoForES.Text.Length - 3).ToString()
                            LblListOfProjectNoForES.Visible = True
                            ImageForES.Visible = True
                        End If
                    Else
                        LblListOfProjectNoForES.Text = ""
                        LblListOfProjectForES.Visible = False
                        ImageForES.Visible = False
                    End If

                    LblListOfProjectForSA.Text = "    * Project List Whose Sample Aanlysis End Date Is Not Ended : "
                    LblListOfProjectForSA.Visible = True
                    LblListOfProjectNoForSA.Text = ProjectListForSampleAnalysisNotEntered
                    LblListOfProjectNoForSA.Visible = True
                    If Not LblListOfProjectNoForSA.Text = String.Empty Then
                        If LblListOfProjectNoForSA.Text.Substring(LblListOfProjectNoForSA.Text.Length - 3) = "," & vbCrLf Then
                            LblListOfProjectNoForSA.Text = LblListOfProjectNoForSA.Text.Substring(0, LblListOfProjectNoForSA.Text.Length - 3).ToString()
                            LblListOfProjectNoForSA.Visible = True
                            ImageForSA.Visible = True
                        End If
                    Else
                        LblListOfProjectNoForSA.Text = ""
                        LblListOfProjectForSA.Visible = False
                        ImageForSA.Visible = False
                    End If



                    TxtStdTimeForES.Visible = False
                    TxtStdForESinReportDispatch.Visible = True



                    DdllistAllSponsor.Visible = False
                    DdllistAllProjectType.Visible = False
                    DdllistPilotPivotal.Visible = False
                    pnl0006.Visible = False
                    pnlChartForWorkSummary1.Visible = True
                    pnl0006.Visible = False

                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectStudyWorkSummary');", True)


                ElseIf DdllistFilterOn.SelectedItem.ToString() = "Report Dispatch Date" Then
                    LblForStdTimeOfESHeadingInWorkSummary.Text = "Standard Time From End Study"
                    LblForStdTimeOfESHeadingInWorkSummary.Visible = True
                    TxtStdTimeForES.Visible = True
                    LblForDaysESinWorkSummary.Text = "Days"
                    LblForDaysESinWorkSummary.Visible = True
                    BtnChangeForESInWorkSummary.Visible = True
                    LblForStdTimeOfSAHeadingInWorkSummary.Text = "Standard Time From Sample Analysis"
                    LblForStdTimeOfSAHeadingInWorkSummary.Visible = True
                    TxtStdTimeForSA.Visible = True
                    LblForDaysSAinWorkSummary.Text = "Days"
                    LblForDaysSAinWorkSummary.Visible = True
                    'BtnChangeForSAInWorkSummary.Visible = True
                    LblProjects.Text = "# Projects : "
                    LblProjects.Visible = True
                    LnkBtnProjectsCountsForES.Text = TotalProjForSA
                    LnkBtnProjectsCountsForES.Visible = True

                    LblAverageTimeFromES.Text = "# Average Time From End Study :"
                    LblAverageTimeFromES.Visible = True

                    CountForAvgForES = TotalProjForSA - totalCountForNotEnteredClinicDateES
                    LblAverageTimeCountsForES.Text = (CInt(TotalTimeForES) / CInt(CountForAvgForES)).ToString()
                    If LblAverageTimeCountsForES.Text.Length > 5 Then
                        LblAverageTimeCountsForES.Text = LblAverageTimeCountsForES.Text.ToString().Substring(0, 4)

                    End If
                    LblAverageTimeCountsForES.Visible = True
                    LblAverageTimeFromSA.Text = "# Average Time From Sample Analysis"
                    LblAverageTimeFromSA.Visible = True
                    CountForAvgForSA = TotalProjForSA - totalCountForNotEnteredClinicDatesa
                    LblAverageTimeCountsForSA.Text = (CInt(TotalTimeForSA) / CInt(CountForAvgForSA)).ToString()
                    If LblAverageTimeCountsForSA.Text.Length > 5 Then
                        LblAverageTimeCountsForSA.Text = LblAverageTimeCountsForSA.Text.ToString().Substring(0, 4)
                    End If
                    LblAverageTimeCountsForSA.Visible = True

                    LblOnTimeFromES.Text = "# On Time From End Study: "
                    LblOnTimeFromES.Visible = True

                    LnkBtnOnTimeCountsForES.Text = OnTimeForES
                    LnkBtnOnTimeCountsForES.Visible = True

                    LblDelayedFromES.Text = "# Delayed From End Study:"
                    LblDelayedFromES.Visible = True

                    LnkBtnDelayedCountsForES.Text = DelayedForES
                    LnkBtnDelayedCountsForES.Visible = True

                    LblOnTimeFromSA.Text = "# On Time From Sample Analysis: "
                    LblOnTimeFromSA.Visible = True

                    LnkBtnOnTimeCountsForSA.Text = OnTimeForSA
                    LnkBtnOnTimeCountsForSA.Visible = True

                    LblDelayedFromSA.Text = "# Delayed From Sample Analysis:"
                    LblDelayedFromSA.Visible = True

                    LnkBtnDelayedCountsForSA.Text = DelayedForSA
                    LnkBtnDelayedCountsForSA.Visible = True

                    LblListOfProjectForES.Text = "    * Project List Whose Clinic End Date Is Not Ended : "
                    LblListOfProjectForES.Visible = True

                    LblListOfProjectNoForES.Text = ProjectListForClinicDateNotEntered
                    LblListOfProjectNoForES.Visible = True
                    If Not LblListOfProjectNoForES.Text = String.Empty Then
                        If LblListOfProjectNoForES.Text.Substring(LblListOfProjectNoForES.Text.Length - 3) = "," & vbCrLf Then
                            LblListOfProjectNoForES.Text = LblListOfProjectNoForES.Text.Substring(0, LblListOfProjectNoForES.Text.Length - 3).ToString()
                            LblListOfProjectNoForES.Visible = True
                            ImageForES.Visible = True
                        End If
                    Else
                        LblListOfProjectNoForES.Text = ""
                        LblListOfProjectForES.Visible = False
                        ImageForES.Visible = False
                    End If

                    LblListOfProjectForSA.Text = "    * Project List Whose Sample Aanlysis End Date Is Not Ended : "
                    LblListOfProjectForSA.Visible = True
                    LblListOfProjectNoForSA.Text = ProjectListForSampleAnalysisNotEntered
                    LblListOfProjectNoForSA.Visible = True
                    If Not LblListOfProjectNoForSA.Text = String.Empty Then
                        If LblListOfProjectNoForSA.Text.Substring(LblListOfProjectNoForSA.Text.Length - 3) = "," & vbCrLf Then
                            LblListOfProjectNoForSA.Text = LblListOfProjectNoForSA.Text.Substring(0, LblListOfProjectNoForSA.Text.Length - 3).ToString()
                            LblListOfProjectNoForSA.Visible = True
                            ImageForSA.Visible = True
                        End If
                    Else
                        LblListOfProjectNoForSA.Text = ""
                        LblListOfProjectForSA.Visible = False
                        ImageForSA.Visible = False
                    End If



                    TxtStdTimeForES.Visible = False
                    TxtStdForESinReportDispatch.Visible = True
                    pnlChartForWorkSummary1.Visible = True
                    pnl0006.Visible = False

                End If
            End If
            DdllistAllSponsor.Visible = False
            DdllistAllProjectType.Visible = False
            DdllistPilotPivotal.Visible = False

            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectStudyWorkSummary');", True)

        Catch ex As Exception
            Me.objCommon.ShowAlert(ex.ToString(), Me.Page)
        End Try
    End Sub

    Protected Sub BtnChangeForESInWorkSummary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnChangeForESInWorkSummary.Click
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectStudyWorkSummary');", True)
        BtnGoForStudyWorkSummary_Click(sender, e)

    End Sub

    Protected Sub BtnGoForOperationalKpi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGoForOperationalKpi.Click
        GetKPIData()
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)

    End Sub

#End Region

#Region "Screening Analytics Related"

    Private Sub FillDropDownLists()
        Dim Wstr As String = ""
        Dim estr As String = ""
        Dim ds_Year As New DataSet
        Dim ds_Location As New DataSet

        DdllistMonth.Items.Clear()
        Me.DdllistMonth.Items.Add("Select...")
        Me.DdllistMonth.Items(0).Value = 0
        Me.DdllistMonth.Items.Add("January")
        Me.DdllistMonth.Items(1).Value = 1
        Me.DdllistMonth.Items.Add("February")
        DdllistMonth.Items(2).Value = 2
        Me.DdllistMonth.Items.Add("March")
        DdllistMonth.Items(3).Value = 3
        Me.DdllistMonth.Items.Add("April")
        DdllistMonth.Items(4).Value = 4
        Me.DdllistMonth.Items.Add("May")
        DdllistMonth.Items(5).Value = 5
        Me.DdllistMonth.Items.Add("June")
        DdllistMonth.Items(6).Value = 6
        Me.DdllistMonth.Items.Add("July")
        DdllistMonth.Items(7).Value = 7
        Me.DdllistMonth.Items.Add("August")
        DdllistMonth.Items(8).Value = 8
        Me.DdllistMonth.Items.Add("September")
        DdllistMonth.Items(9).Value = 9
        Me.DdllistMonth.Items.Add("October")
        DdllistMonth.Items(10).Value = 10
        Me.DdllistMonth.Items.Add("November")
        DdllistMonth.Items(11).Value = 11
        Me.DdllistMonth.Items.Add("December")
        DdllistMonth.Items(12).Value = 12

        DdllistYear.Items.Clear()

        Wstr = "select distinct(YEAR(dScreenDate)) as Differentyear from medexscreeninghdr order by Differentyear"

        ds_Year = Me.objHelpDbTable.GetResultSet(Wstr, "MedExScreeningHdr")
        If ds_Year.Tables(0).Rows.Count = 0 Then
            Me.objCommon.ShowAlert("There Is No Entry For Year in Screening Detail.", Me.Page)
            Exit Sub
        End If

        'If Not objHelpDbTable.View_subjectLocation("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, _
        '                                                         ds_Location, estr) Then
        '    Throw New Exception("Error While Getting Data From MedExScreeninghdrDtl. " + estr)
        'End If
        DdllistYear.DataSource = ds_Year.Tables(0)
        DdllistYear.DataTextField = "Differentyear"
        DdllistYear.DataBind()
        Me.DdllistYear.Items.Insert(0, "Select...")

        'DdlLocation.DataSource = ds_Location.Tables(0)
        'DdlLocation.DataValueField = "vLocationCode"
        'DdlLocation.DataTextField = "vLocationName"
        'DdlLocation.DataBind()
    End Sub

#End Region

#Region "Project Work Summary"

    Private Sub BindDropDownOfStusyWorkSummary()
        Me.DdllistFilterOn.Items.Clear()
        Me.DdllistReportFor.Items.Clear()
        Me.DdllistAllSponsor.Items.Clear()
        Me.DdllistPilotPivotal.Items.Clear()

        Me.DdllistFilterOn.Items.Add("Select...")
        Me.DdllistFilterOn.Items.Add("Clinic End Date")
        Me.DdllistFilterOn.Items.Add("Sample Analysis End Date")
        Me.DdllistFilterOn.Items.Add("Report Sent To Sponsor Date")
        Me.DdllistFilterOn.Items.Add("Report Dispatch Date")

        Me.DdllistReportFor.Items.Add("Select...")
        Me.DdllistReportFor.Items.Add("Last Month")
        Me.DdllistReportFor.Items.Add("Current Month")
        Me.DdllistReportFor.Items.Add("Last Quater")
        Me.DdllistReportFor.Items.Add("Current Calendar Year")
        Me.DdllistReportFor.Items.Add("Last Calendar Year")
        Me.DdllistReportFor.Items.Add("Current Financial Year")
        Me.DdllistReportFor.Items.Add("Last Financial Year")


        Me.DdllistAllSponsor.Items.Add("All Sponsor")

        Me.DdllistAllProjectType.Items.Add("All Project Type")

        Me.DdllistPilotPivotal.Items.Add("Pilot And Pivotal")
        Me.DdllistPilotPivotal.Items.Add("Pilot")
        Me.DdllistPilotPivotal.Items.Add("Pivotal")
    End Sub

#End Region

#Region "Filterations"

    Protected Sub DdllistReportFor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdllistReportFor.SelectedIndexChanged
        Dim LastMonth As String = String.Empty
        Dim LastdayOfPpreviousMonth As String = String.Empty
        Dim FromDate As String = String.Empty
        Dim ToDate As String = String.Empty

        If DdllistReportFor.SelectedItem.ToString() = "Select..." Then
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectStudyWorkSummary');", True)
        ElseIf DdllistReportFor.SelectedItem.ToString() = "Last Month" Then
            FromDate = "01" & "-" & MonthName(DateTime.Now.AddMonths(-1).Month, True) & "-" & DateTime.Now.AddMonths(-1).Year.ToString()
            LastdayOfPpreviousMonth = DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year.ToString(), DateTime.Now.AddMonths(-1).Month)
            ToDate = LastdayOfPpreviousMonth & "-" & MonthName(DateTime.Now.AddMonths(-1).Month, True) & "-" & DateTime.Now.AddMonths(-1).Year.ToString()
            TxtFromDate.Text = FromDate
            TxtToDate.Text = ToDate
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectStudyWorkSummary');", True)
        ElseIf DdllistReportFor.SelectedItem.ToString() = "Current Month" Then
            FromDate = "01" & "-" & MonthName(DateTime.Now.Month, True) & "-" & DateTime.Now.Year.ToString()
            LastdayOfPpreviousMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)
            ToDate = LastdayOfPpreviousMonth & "-" & MonthName(DateTime.Now.Month, True) & "-" & DateTime.Now.Year
            TxtFromDate.Text = FromDate
            TxtToDate.Text = ToDate
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectStudyWorkSummary');", True)

        ElseIf DdllistReportFor.SelectedItem.ToString() = "Last Quater" Then
            FromDate = "01" & "-" & MonthName(DateTime.Now.AddMonths(-2).Month, True) & "-" & DateTime.Now.AddMonths(-2).Year.ToString()
            LastdayOfPpreviousMonth = DateTime.DaysInMonth(DateTime.Now.AddMonths(0).Year.ToString(), DateTime.Now.AddMonths(0).Month)
            ToDate = LastdayOfPpreviousMonth & "-" & MonthName(DateTime.Now.AddMonths(0).Month, True) & "-" & DateTime.Now.AddMonths(0).Year.ToString()
            TxtFromDate.Text = FromDate
            TxtToDate.Text = ToDate
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectStudyWorkSummary');", True)

        ElseIf DdllistReportFor.SelectedItem.ToString() = "Current Calendar Year" Then
            FromDate = "01" & "-" & "Jan" & "-" & DateTime.Now.Year.ToString()
            'LastdayOfPpreviousMonth = DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year.ToString(), DateTime.Now.AddMonths(-1).Month)
            ToDate = "31" & "-" & "Dec" & "-" & DateTime.Now.Year.ToString()
            TxtFromDate.Text = FromDate
            TxtToDate.Text = ToDate
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectStudyWorkSummary');", True)

        ElseIf DdllistReportFor.SelectedItem.ToString() = "Last Calendar Year" Then
            FromDate = "01" & "-" & "Jan" & "-" & DateTime.Now.AddYears(-1).Year.ToString()
            'LastdayOfPpreviousMonth = DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year.ToString(), DateTime.Now.AddMonths(-1).Month)
            ToDate = "31" & "-" & "Dec" & "-" & DateTime.Now.AddYears(-1).Year.ToString()
            TxtFromDate.Text = FromDate
            TxtToDate.Text = ToDate
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectStudyWorkSummary');", True)

        ElseIf DdllistReportFor.SelectedItem.ToString() = "Current Financial Year" Then
            FromDate = "01" & "-" & "Apr" & "-" & DateTime.Now.Year.ToString()

            ToDate = "31" & "-" & "Mar" & "-" & DateTime.Now.AddYears(1).Year.ToString()
            TxtFromDate.Text = FromDate
            TxtToDate.Text = ToDate
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectStudyWorkSummary');", True)

        ElseIf DdllistReportFor.SelectedItem.ToString() = "Last Financial Year" Then
            FromDate = "01" & "-" & "Apr" & "-" & DateTime.Now.AddYears(-1).Year.ToString()
            ToDate = "31" & "-" & "Mar" & "-" & DateTime.Now.Year.ToString()
            TxtFromDate.Text = FromDate
            TxtToDate.Text = ToDate
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectStudyWorkSummary');", True)

        End If
        pnlChartForWorkSummary1.Visible = False
    End Sub

    Protected Sub DdllistAllSponsor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdllistAllSponsor.SelectedIndexChanged
        Dim dv_SelectedSponsor As DataView
        Dim dt_SelectedSponsor As New DataTable
        Dim Filter As String = "1=1"

        dt_SelectedSponsor = CType(ViewState("VS_GvwPnlProjectStudyWorkSummary_PageIndexChanging"), DataTable)
        dv_SelectedSponsor = dt_SelectedSponsor.DefaultView()

        If DdllistAllSponsor.SelectedItem.ToString.ToUpper() <> "ALL SPONSOR" Then

            Filter = "vClientCode ='" & DdllistAllSponsor.SelectedValue.ToString() & "'"

            If DdllistAllProjectType.SelectedItem.ToString.ToUpper() <> "ALL PROJECT TYPE" Then
                Filter += " And vProjectTypeName ='" & DdllistAllProjectType.SelectedItem.ToString() & "'"
            End If

            If DdllistPilotPivotal.SelectedItem.ToString.ToUpper() <> "PILOT AND PIVOTAL" Then
                Filter += " And vWorkTypeDesc ='" & DdllistPilotPivotal.SelectedItem.ToString() & "'"
            End If

        Else

            If DdllistAllProjectType.SelectedItem.ToString.ToUpper() <> "ALL PROJECT TYPE" Then

                Filter = "vProjectTypeName ='" & DdllistAllProjectType.SelectedItem.ToString() & "'"

                If DdllistPilotPivotal.SelectedItem.ToString.ToUpper() <> "PILOT AND PIVOTAL" Then
                    Filter += " And vWorkTypeDesc ='" & DdllistPilotPivotal.SelectedItem.ToString() & "'"
                End If

            Else

                If DdllistPilotPivotal.SelectedItem.ToString.ToUpper() <> "PILOT AND PIVOTAL" Then
                    Filter = "vWorkTypeDesc ='" & DdllistPilotPivotal.SelectedItem.ToString() & "'"
                End If

            End If

        End If

        dv_SelectedSponsor.RowFilter = Filter

        LblNoRecordFound.Visible = True
        BtnExportToExcelForProjectStudyWorkSummary.Visible = False
        If dv_SelectedSponsor.ToTable().Rows.Count > 0 Then
            LblNoRecordFound.Visible = False
            BtnExportToExcelForProjectStudyWorkSummary.Visible = True
        End If

        GvwPnlProjectStudyWorkSummary.DataSource = dv_SelectedSponsor.ToTable()
        GvwPnlProjectStudyWorkSummary.DataBind()

        GvwPnlProjectStudyWorkSummary.Visible = True
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectStudyWorkSummary');", True)

    End Sub

    Protected Sub DdllistAllProjectType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdllistAllProjectType.SelectedIndexChanged
        Dim dv_SelectedProjectType As DataView
        Dim dt_SelectedProjectType As New DataTable
        Dim Filter As String = "1=1"

        dt_SelectedProjectType = CType(ViewState("VS_GvwPnlProjectStudyWorkSummary_PageIndexChanging"), DataTable)
        dv_SelectedProjectType = dt_SelectedProjectType.DefaultView()

        If DdllistAllProjectType.SelectedItem.ToString.ToUpper() <> "ALL PROJECT TYPE" Then

            Filter = "vProjectTypeName ='" & DdllistAllProjectType.SelectedItem.ToString() & "'"

            If DdllistAllSponsor.SelectedItem.ToString.ToUpper() <> "ALL SPONSOR" Then
                Filter += " And vClientCode ='" & DdllistAllSponsor.SelectedValue.ToString() & "'"
            End If

            If DdllistPilotPivotal.SelectedItem.ToString.ToUpper() <> "PILOT AND PIVOTAL" Then
                Filter += " And vWorkTypeDesc ='" & DdllistPilotPivotal.SelectedItem.ToString() & "'"
            End If

        Else

            If DdllistAllSponsor.SelectedItem.ToString.ToUpper() <> "ALL SPONSOR" Then

                Filter = "vClientCode ='" & DdllistAllSponsor.SelectedValue.ToString() & "'"

                If DdllistPilotPivotal.SelectedItem.ToString.ToUpper() <> "PILOT AND PIVOTAL" Then
                    Filter += " And vWorkTypeDesc ='" & DdllistPilotPivotal.SelectedItem.ToString() & "'"
                End If

            Else

                If DdllistPilotPivotal.SelectedItem.ToString.ToUpper() <> "PILOT AND PIVOTAL" Then
                    Filter = "vWorkTypeDesc ='" & DdllistPilotPivotal.SelectedItem.ToString() & "'"
                End If

            End If

        End If

        dv_SelectedProjectType.RowFilter = Filter

        LblNoRecordFound.Visible = True
        BtnExportToExcelForProjectStudyWorkSummary.Visible = False
        If dv_SelectedProjectType.ToTable().Rows.Count > 0 Then
            LblNoRecordFound.Visible = False
            BtnExportToExcelForProjectStudyWorkSummary.Visible = True
        End If

        GvwPnlProjectStudyWorkSummary.DataSource = dv_SelectedProjectType.ToTable()
        GvwPnlProjectStudyWorkSummary.DataBind()
        GvwPnlProjectStudyWorkSummary.Visible = True

        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectStudyWorkSummary');", True)
    End Sub

    Protected Sub DdllistPilotPivotal_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdllistPilotPivotal.SelectedIndexChanged
        Dim dv_SelectedPivotAndPivotal As DataView
        Dim dt_SelectedPivotAndPivotal As New DataTable
        Dim Filter As String = "1=1"

        dt_SelectedPivotAndPivotal = CType(ViewState("VS_GvwPnlProjectStudyWorkSummary_PageIndexChanging"), DataTable)
        dv_SelectedPivotAndPivotal = dt_SelectedPivotAndPivotal.DefaultView()

        If DdllistPilotPivotal.SelectedItem.ToString.ToUpper() <> "PILOT AND PIVOTAL" Then

            Filter = "vWorkTypeDesc ='" & DdllistPilotPivotal.SelectedItem.ToString() & "'"

            If DdllistAllSponsor.SelectedItem.ToString.ToUpper() <> "ALL SPONSOR" Then
                Filter += " And vClientCode ='" & DdllistAllSponsor.SelectedValue.ToString() & "'"
            End If

            If DdllistAllProjectType.SelectedItem.ToString.ToUpper() <> "ALL PROJECT TYPE" Then
                Filter += " And vProjectTypeName ='" & DdllistAllProjectType.SelectedItem.ToString() & "'"
            End If

        Else

            If DdllistAllSponsor.SelectedItem.ToString.ToUpper() <> "ALL SPONSOR" Then

                Filter = "vClientCode ='" & DdllistAllSponsor.SelectedValue.ToString() & "'"

                If DdllistAllProjectType.SelectedItem.ToString.ToUpper() <> "ALL PROJECT TYPE" Then
                    Filter += " And vProjectTypeName ='" & DdllistAllProjectType.SelectedItem.ToString() & "'"
                End If

            Else

                If DdllistAllProjectType.SelectedItem.ToString.ToUpper() <> "ALL PROJECT TYPE" Then
                    Filter = "vProjectTypeName ='" & DdllistAllProjectType.SelectedItem.ToString() & "'"
                End If

            End If

        End If

        dv_SelectedPivotAndPivotal.RowFilter = Filter

        LblNoRecordFound.Visible = True
        BtnExportToExcelForProjectStudyWorkSummary.Visible = False
        If dv_SelectedPivotAndPivotal.ToTable().Rows.Count > 0 Then
            LblNoRecordFound.Visible = False
            BtnExportToExcelForProjectStudyWorkSummary.Visible = True
        End If

        GvwPnlProjectStudyWorkSummary.DataSource = dv_SelectedPivotAndPivotal.ToTable()
        GvwPnlProjectStudyWorkSummary.DataBind()
        GvwPnlProjectStudyWorkSummary.Visible = True

        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divProjectStudyWorkSummary');", True)
    End Sub

#End Region

    Private Sub GetKPIData()
        Dim Ds_OperationalKpiBedNights As New DataSet
        Dim Ds_OperationalKpi As New DataSet
        Dim dt_OperationalBedNights As New DataTable
        Dim dt_OerationalBedNightsPilotCount As New DataTable
        Dim dt_OperationBedNightsPivotalCount As New DataTable
        Dim dt_SubjectsDosed As New DataTable
        Dim dt_SubjectsDosedPilotCount As New DataTable
        Dim dt_SubjectsDosedPivotalCount As New DataTable
        Dim dt_ClinicEndTotalCount As New DataTable
        Dim dt_ClinicFinalForPilotPivotal As New DataTable
        Dim dt_SampleEndTotalCount As New DataTable
        Dim dt_SampleFinalPilotPivotal As New DataTable
        Dim dt_ReportEndTotalCount As New DataTable
        Dim dt_SampleAnalystTotalCount As New DataTable
        Dim dt_SampleAnalystFinalPilotPivotal As New DataTable
        Dim dt_SampleAanlystFinalEndPivotal As DataTable
        Dim dt_SampleAnalystFinalEndPilot As DataTable
        Dim dt_ReportFinalPilotPivotal As New DataTable
        Dim dt_SponsorReportEndTotalCount As New DataTable
        Dim dt_SponsorReportFinalPilotPivotal As New DataTable

        Dim Dv_OperationalBedNightsPilotCount As DataView
        Dim Dv_OperationalBedNightsPivotalCount As DataView
        Dim Dv_SubjectsDosedPilotCount As DataView
        Dim Dv_SubjectsDosedPivotalCount As DataView
        Dim dv_ClinicTotal As DataView
        Dim Dv_ClinicEndPilotCount As DataView
        Dim Dv_ClinicEndPivotalCount As DataView
        Dim dv_SampleTotal As DataView
        Dim Dv_SampleEndPilotCount As DataView
        Dim Dv_SampleEndPivotalCount As DataView
        Dim dv_ReportTotal As DataView
        Dim dv_SponsorReportTotal As DataView

        Dim dv_SampleAnalystTotal As DataView
        Dim Dv_SampleAnalystEndPivotalCount As DataView
        Dim Dv_SampleAanlystEndPilotCount As DataView
        Dim Dv_SponsorSampleEndPilotCount As DataView
        Dim Dv_SponsorSampleEndPivotalCount As DataView

        Dim dv_ProjectsReleased As DataView
        Dim Dv_ProjectsReleasedPilotCount As DataView
        Dim Dv_ProjectsReleasedPivotalCount As DataView
        Dim dt_ProjectsReleased As DataTable

        Dim SumForTotalBedNightsCount As Double = 0
        Dim multiplyForTotalBedNightsCount As Double = 0
        Dim iForTotalBedNights As Integer = 0
        Dim SumForBedNightsPilotCount As Double = 0
        Dim multiplyForBedNightsPilotCount As Double = 0
        Dim iForBedNightsPilotCounts As Integer = 0
        Dim SumForBedNightsPivotalCount As Double = 0
        Dim multiplyForBedNightsPivotalCount As Double = 0
        Dim iForBedNightsPivotalCounts As Integer = 0
        Dim FirstNumForOperationalKpi As Double = 0
        Dim SecondNumForOperationkpi As Double = 0


        Dim estr As String = String.Empty
        Try


            If TxtFromDateOfOperationalKpi.Text = String.Empty And TxtToDateOfOperationalKpi.Text = String.Empty Then
                Me.objCommon.ShowAlert("Select From and To Date", Me.Page)
                Exit Sub
            ElseIf TxtFromDateOfOperationalKpi.Text = String.Empty And TxtToDateOfOperationalKpi.Text <> String.Empty Then
                Me.objCommon.ShowAlert("Select From Date", Me.Page)
                Exit Sub
            ElseIf TxtFromDateOfOperationalKpi.Text <> String.Empty And TxtToDateOfOperationalKpi.Text = String.Empty Then
                Me.objCommon.ShowAlert("Select To Date", Me.Page)
                Exit Sub
            Else
                If Not objHelpDbTable.Proc_Get_BedNights(TxtFromDateOfOperationalKpi.Text, TxtToDateOfOperationalKpi.Text, Me.ddlLocation.SelectedValue(), Ds_OperationalKpiBedNights, estr) Then
                    Me.objCommon.ShowAlert("Error While Getting Data For BedNights..." + estr, Me.Page)
                    Throw New Exception(estr)
                End If

                PnlTblForOperationalKpi.Visible = True
                dt_OperationalBedNights = CType(Ds_OperationalKpiBedNights.Tables(0), DataTable)
                dt_OperationalBedNights.Columns.Add("BedNightsTtl")
                dt_ClinicEndTotalCount.AcceptChanges()
                '' Counts Total No Of BedNights''
                For iForTotalBedNights = 0 To dt_OperationalBedNights.Rows.Count - 1
                    FirstNumForOperationalKpi = IIf(Convert.ToString(dt_OperationalBedNights.Rows(iForTotalBedNights).Item("BedNights")).Trim() = "", "0", Convert.ToDouble(dt_OperationalBedNights.Rows(iForTotalBedNights).Item("BedNights")))
                    SecondNumForOperationkpi = IIf(Convert.ToString(dt_OperationalBedNights.Rows(iForTotalBedNights).Item("DosedSubjects")).Trim() = "", "0", Convert.ToString(dt_OperationalBedNights.Rows(iForTotalBedNights).Item("DosedSubjects")).Trim())

                    multiplyForTotalBedNightsCount = FirstNumForOperationalKpi * SecondNumForOperationkpi
                    dt_OperationalBedNights.Rows(iForTotalBedNights).Item("BedNightsTtl") = multiplyForTotalBedNightsCount
                    'CDbl(dt_OperationalBedNights.Rows(iForTotalBedNights).Item("BedNights")) * CDbl(dt_OperationalBedNights.Rows(iForTotalBedNights).Item("DosedSubjects"))
                    SumForTotalBedNightsCount += multiplyForTotalBedNightsCount
                Next
                'LnkBtnBedNightsTotal.Text = IIf(Convert.ToString(dt_OperationalBedNights.Compute("Sum(BedNights*iNoOfSubjects)", True)).Trim() = "", "0", Convert.ToString(dt_OperationalBedNights.Compute("Sum(BedNights)", True)).Trim())

                LnkBtnBedNightsTotal.Text = SumForTotalBedNightsCount
                ViewState("BedNightsData") = dt_OperationalBedNights

                Dv_OperationalBedNightsPilotCount = dt_OperationalBedNights.DefaultView()
                Dv_OperationalBedNightsPilotCount.RowFilter = "vWorkTypeDesc='Pilot'"
                Dv_OperationalBedNightsPilotCount.ToTable().AcceptChanges()
                dt_OerationalBedNightsPilotCount = Dv_OperationalBedNightsPilotCount.ToTable()

                '' Counts Total No Of Pilot Bed Nights''
                For iForBedNightsPilotCounts = 0 To dt_OerationalBedNightsPilotCount.Rows.Count - 1
                    FirstNumForOperationalKpi = IIf(Convert.ToString(dt_OerationalBedNightsPilotCount.Rows(iForBedNightsPilotCounts).Item("BedNights")).Trim() = "", "0", Convert.ToDouble(dt_OerationalBedNightsPilotCount.Rows(iForBedNightsPilotCounts).Item("BedNights")))
                    SecondNumForOperationkpi = IIf(Convert.ToString(dt_OerationalBedNightsPilotCount.Rows(iForBedNightsPilotCounts).Item("DosedSubjects")).Trim() = "", "0", Convert.ToString(dt_OerationalBedNightsPilotCount.Rows(iForBedNightsPilotCounts).Item("DosedSubjects")).Trim())
                    multiplyForBedNightsPilotCount = FirstNumForOperationalKpi * SecondNumForOperationkpi
                    SumForBedNightsPilotCount += multiplyForBedNightsPilotCount
                Next
                'LnkBtnBedNightsPilot.Text = IIf(Convert.ToString(dt_OerationalBedNightsPilotCount.Compute("Sum(BedNights)", True)).Trim() = "", "0", Convert.ToString(dt_OerationalBedNightsPilotCount.Compute("Sum(BedNights)", True)).Trim())
                LnkBtnBedNightsPilot.Text = SumForBedNightsPilotCount

                Dv_OperationalBedNightsPivotalCount = dt_OperationalBedNights.DefaultView()
                Dv_OperationalBedNightsPivotalCount.RowFilter = "vWorkTypeDesc='Pivotal'"
                Dv_OperationalBedNightsPivotalCount.ToTable().AcceptChanges()
                dt_OperationBedNightsPivotalCount = Dv_OperationalBedNightsPivotalCount.ToTable()

                '' Counts For Total Of Pivotal Projects''

                For iForBedNightsPivotalCounts = 0 To dt_OperationBedNightsPivotalCount.Rows.Count - 1
                    FirstNumForOperationalKpi = IIf(Convert.ToString(dt_OperationBedNightsPivotalCount.Rows(iForBedNightsPivotalCounts).Item("BedNights")).Trim() = "", "0", Convert.ToDouble(dt_OperationBedNightsPivotalCount.Rows(iForBedNightsPivotalCounts).Item("BedNights")))
                    SecondNumForOperationkpi = IIf(Convert.ToString(dt_OperationBedNightsPivotalCount.Rows(iForBedNightsPivotalCounts).Item("DosedSubjects")).Trim() = "", "0", Convert.ToString(dt_OperationBedNightsPivotalCount.Rows(iForBedNightsPivotalCounts).Item("DosedSubjects")).Trim())
                    multiplyForBedNightsPivotalCount = FirstNumForOperationalKpi * SecondNumForOperationkpi
                    SumForBedNightsPivotalCount += multiplyForBedNightsPivotalCount
                Next

                'LnkBtnBedNightsPivotal.Text = IIf(Convert.ToString(dt_OperationBedNightsPivotalCount.Compute("Sum(BedNights)", True)).Trim() = "", "0", Convert.ToString(dt_OperationBedNightsPivotalCount.Compute("Sum(BedNights)", True)).Trim())
                LnkBtnBedNightsPivotal.Text = SumForBedNightsPivotalCount

                If Not objHelpDbTable.Proc_GetTotalDosedSubject(TxtFromDateOfOperationalKpi.Text + "##" + TxtToDateOfOperationalKpi.Text + "##" + Me.ddlLocation.SelectedValue(), Ds_OperationalKpi, estr) Then
                    Me.objCommon.ShowAlert("Error While Getting Data From Proc_GetTotalDosedSubject..." + estr.ToString.Trim(), Me.Page)
                    Throw New Exception(estr)
                End If
                ''' **** For No Of Subjects Total  Dosed   ***** '''''

                dt_SubjectsDosed = CType(Ds_OperationalKpi.Tables(0), DataTable)
                ViewState("SubjectsTotalDosed") = dt_SubjectsDosed
                LnkBtnSubjectsDosedTtl.Text = "0"
                For Each dr In dt_SubjectsDosed.Rows
                    LnkBtnSubjectsDosedTtl.Text = CInt(LnkBtnSubjectsDosedTtl.Text) + CInt(IIf(dr("TotalDosed").ToString.Trim() = "", 0, dr("TotalDosed").ToString.Trim()))
                Next


                Dv_SubjectsDosedPilotCount = dt_SubjectsDosed.DefaultView()
                Dv_SubjectsDosedPilotCount.RowFilter = "vWorkTypeDesc='Pilot'"
                Dv_SubjectsDosedPilotCount.ToTable().AcceptChanges()
                dt_SubjectsDosedPilotCount = Dv_SubjectsDosedPilotCount.ToTable()
                LnkBtnSubjectsDosedttlPilot.Text = IIf(Convert.ToString(dt_SubjectsDosedPilotCount.Compute("Sum(TotalDosed)", True)).Trim() = "", "0", Convert.ToString(dt_SubjectsDosedPilotCount.Compute("Sum(TotalDosed)", True)).Trim())

                Dv_SubjectsDosedPivotalCount = dt_SubjectsDosed.DefaultView()
                Dv_SubjectsDosedPivotalCount.RowFilter = "vWorkTypeDesc='Pivotal'"
                Dv_SubjectsDosedPivotalCount.ToTable().AcceptChanges()
                dt_SubjectsDosedPivotalCount = Dv_SubjectsDosedPivotalCount.ToTable()
                LnkBtnSubjectsDosedTtlPivotal.Text = IIf(Convert.ToString(dt_SubjectsDosedPivotalCount.Compute("Sum(TotalDosed)", True)).Trim() = "", "0", Convert.ToString(dt_SubjectsDosedPivotalCount.Compute("Sum(TotalDosed)", True)).Trim())


                'If Not objHelpDbTable.Proc_GetOperational_KPIs(TxtFromDateOfOperationalKpi.Text, TxtToDateOfOperationalKpi.Text, Ds_OperationalKpi, estr) Then
                '    Me.objCommon.ShowAlert("Error While Getting Data From Proc_GetOperationalKpis", Me.Page)
                '    Exit Sub
                'End If
                Ds_OperationalKpi = Nothing
                dt_SubjectsDosed = Nothing

                If Not objHelpDbTable.Proc_GetOperational_KPIs(TxtFromDateOfOperationalKpi.Text + "##" + TxtToDateOfOperationalKpi.Text + "##" + Me.ddlLocation.SelectedValue(), Ds_OperationalKpi, estr) Then
                    Me.objCommon.ShowAlert("Error While Getting Data From Proc_GetOperationalKpis..." + estr.ToString.Trim(), Me.Page)
                    Throw New Exception(estr)
                End If
                ''' **** For No Of Subjects Dosed   ***** '''''

                dt_SubjectsDosed = CType(Ds_OperationalKpi.Tables(0), DataTable)
                Dim dv_FinalSubjectsDosed As DataView = dt_SubjectsDosed.DefaultView()
                dv_FinalSubjectsDosed.RowFilter = "ProjectStatus='D'"
                'dv_FinalSubjectsDosed.ToTable().AcceptChanges()
                dt_SubjectsDosed = dv_FinalSubjectsDosed.ToTable()
                ViewState("SubjectsDosed") = dt_SubjectsDosed
                LnkBtnSubjectsDosedTotal.Text = "0"
                For Each dr In dt_SubjectsDosed.Rows
                    LnkBtnSubjectsDosedTotal.Text = CInt(LnkBtnSubjectsDosedTotal.Text) + CInt(IIf(dr("iNoOfSubjects").ToString.Trim() = "", 0, dr("iNoOfSubjects").ToString.Trim()))
                Next
                'LnkBtnSubjectsDosedTotal.Text = IIf(Convert.ToString(dt_SubjectsDosed.Compute("Sum(DosedSubjects)", True)).Trim() = "", "0", Convert.ToString(dt_SubjectsDosed.Compute("Sum(DosedSubjects)", True)).Trim())

                Dv_SubjectsDosedPilotCount = dt_SubjectsDosed.DefaultView()
                Dv_SubjectsDosedPilotCount.RowFilter = "vWorkTypeDesc='Pilot'"
                Dv_SubjectsDosedPilotCount.ToTable().AcceptChanges()
                dt_SubjectsDosedPilotCount = Dv_SubjectsDosedPilotCount.ToTable()
                LnkBtnSubjectsDosedPilot.Text = IIf(Convert.ToString(dt_SubjectsDosedPilotCount.Compute("Sum(iNoOfSubjects)", True)).Trim() = "", "0", Convert.ToString(dt_SubjectsDosedPilotCount.Compute("Sum(iNoOfSubjects)", True)).Trim())

                Dv_SubjectsDosedPivotalCount = dt_SubjectsDosed.DefaultView()
                Dv_SubjectsDosedPivotalCount.RowFilter = "vWorkTypeDesc='Pivotal'"
                Dv_SubjectsDosedPivotalCount.ToTable().AcceptChanges()
                dt_SubjectsDosedPivotalCount = Dv_SubjectsDosedPivotalCount.ToTable()
                LnkBtnSubjectsDosedPivotal.Text = IIf(Convert.ToString(dt_SubjectsDosedPivotalCount.Compute("Sum(iNoOfSubjects)", True)).Trim() = "", "0", Convert.ToString(dt_SubjectsDosedPivotalCount.Compute("Sum(iNoOfSubjects)", True)).Trim())

                '''**** for Clinical End ******''''

                dt_ClinicEndTotalCount = CType(Ds_OperationalKpi.Tables(0), DataTable)
                dv_ClinicTotal = dt_ClinicEndTotalCount.DefaultView()
                dv_ClinicTotal.RowFilter = "ProjectStatus='C'"
                dv_ClinicTotal.ToTable().AcceptChanges()
                ViewState("ClinicEndTotalData") = dv_ClinicTotal.ToTable()
                dt_ClinicFinalForPilotPivotal = dv_ClinicTotal.ToTable()


                LnkBtnClinicEndTotal.Text = dv_ClinicTotal.ToTable().Rows.Count

                Dv_ClinicEndPilotCount = dt_ClinicFinalForPilotPivotal.DefaultView()
                Dv_ClinicEndPilotCount.RowFilter = "vWorkTypeDesc='Pilot'"
                Dv_ClinicEndPilotCount.ToTable().AcceptChanges()
                LnkBtnClinicEndPilot.Text = Dv_ClinicEndPilotCount.ToTable().Rows.Count

                Dv_ClinicEndPivotalCount = dt_ClinicFinalForPilotPivotal.DefaultView()
                Dv_ClinicEndPivotalCount.RowFilter = "vWorkTypeDesc='Pivotal'"
                Dv_ClinicEndPivotalCount.ToTable().AcceptChanges()
                LnkBtnClinicEndPivotal.Text = Dv_ClinicEndPivotalCount.ToTable().Rows.Count

                ''' **** Sample Analysis End *******'''

                dt_SampleEndTotalCount = CType(Ds_OperationalKpi.Tables(0), DataTable)
                dv_SampleTotal = dt_SampleEndTotalCount.DefaultView()
                dv_SampleTotal.RowFilter = "ProjectStatus='S'"
                dv_SampleTotal.ToTable().AcceptChanges()
                ViewState("SampleEndTotalData") = dv_SampleTotal.ToTable()
                dt_SampleFinalPilotPivotal = dv_SampleTotal.ToTable()

                LnkBtnSampleEndTotal.Text = dv_SampleTotal.ToTable().Rows.Count

                Dv_SampleEndPilotCount = dt_SampleFinalPilotPivotal.DefaultView()
                Dv_SampleEndPilotCount.RowFilter = "vWorkTypeDesc='Pilot'"
                Dv_SampleEndPilotCount.ToTable().AcceptChanges()
                LnkBtnSampleEndPilot.Text = Dv_SampleEndPilotCount.ToTable().Rows.Count

                Dv_SampleEndPivotalCount = dt_SampleFinalPilotPivotal.DefaultView()
                Dv_SampleEndPivotalCount.RowFilter = "vWorkTypeDesc='Pivotal'"
                Dv_SampleEndPivotalCount.ToTable().AcceptChanges()
                LnkBtnSampleEndPivotal.Text = Dv_SampleEndPivotalCount.ToTable().Rows.Count

                ''' **** Report Dispatch  End ***** '''''
                dt_ReportEndTotalCount = CType(Ds_OperationalKpi.Tables(0), DataTable)
                dv_ReportTotal = dt_ReportEndTotalCount.DefaultView()
                dv_ReportTotal.RowFilter = "ProjectStatus='R'"
                dv_ReportTotal.ToTable().AcceptChanges()
                ViewState("ReportEndTotal") = dv_ReportTotal.ToTable()
                dt_ReportFinalPilotPivotal = dv_ReportTotal.ToTable()

                LnkBtnReportEndTotal.Text = dv_ReportTotal.ToTable().Rows.Count

                Dv_SampleEndPilotCount = dt_ReportFinalPilotPivotal.DefaultView()
                Dv_SampleEndPilotCount.RowFilter = "vWorkTypeDesc='Pilot'"
                Dv_SampleEndPilotCount.ToTable().AcceptChanges()
                LnkBtnReportEndPilot.Text = Dv_SampleEndPilotCount.ToTable().Rows.Count

                Dv_SampleEndPivotalCount = dt_ReportFinalPilotPivotal.DefaultView()
                Dv_SampleEndPivotalCount.RowFilter = "vWorkTypeDesc='Pivotal'"
                Dv_SampleEndPivotalCount.ToTable().AcceptChanges()
                LnkBtnReportEndPivotal.Text = Dv_SampleEndPivotalCount.ToTable().Rows.Count

                ''' *** Report Sent To Sponsor For Review *** '''

                dt_SponsorReportEndTotalCount = CType(Ds_OperationalKpi.Tables(0), DataTable)
                dv_SponsorReportTotal = dt_SponsorReportEndTotalCount.DefaultView()
                dv_SponsorReportTotal.RowFilter = "ProjectStatus='P'"
                dv_SponsorReportTotal.ToTable().AcceptChanges()
                ViewState("SponsorReportEndTotal") = dv_SponsorReportTotal.ToTable()
                dt_SponsorReportFinalPilotPivotal = dv_SponsorReportTotal.ToTable()

                LnkBtnReportToSponsorReviewEndTotal.Text = dv_SponsorReportTotal.ToTable().Rows.Count

                Dv_SponsorSampleEndPilotCount = dt_SponsorReportFinalPilotPivotal.DefaultView()
                Dv_SponsorSampleEndPilotCount.RowFilter = "vWorkTypeDesc='Pilot'"
                Dv_SponsorSampleEndPilotCount.ToTable().AcceptChanges()
                LnkBtnReportToSponsorReviewEndPilot.Text = Dv_SponsorSampleEndPilotCount.ToTable().Rows.Count

                Dv_SponsorSampleEndPivotalCount = dt_SponsorReportFinalPilotPivotal.DefaultView()
                Dv_SponsorSampleEndPivotalCount.RowFilter = "vWorkTypeDesc='Pivotal'"
                Dv_SponsorSampleEndPivotalCount.ToTable().AcceptChanges()
                LnkBtnReportToSponsorReviewEndPivotal.Text = Dv_SponsorSampleEndPivotalCount.ToTable().Rows.Count

                ''' **** Sample Analyst ***** '''''
                dt_SampleAnalystTotalCount = CType(Ds_OperationalKpi.Tables(0), DataTable)
                dv_SampleAnalystTotal = dt_SampleAnalystTotalCount.DefaultView()
                dv_SampleAnalystTotal.RowFilter = "ProjectStatus='S'"
                dv_SampleAnalystTotal.ToTable().AcceptChanges()
                ViewState("SampleAnalystTotal") = dv_SampleAnalystTotal.ToTable()
                dt_SampleAnalystFinalPilotPivotal = dv_SampleAnalystTotal.ToTable()
                LnkBtnSampleAnalystEndTotal.Text = IIf(Convert.ToString(dt_SampleAnalystFinalPilotPivotal.Compute("Sum(Samples)", True)).Trim() = "", "0", Convert.ToString(dt_SampleAnalystFinalPilotPivotal.Compute("Sum(Samples)", True)).Trim())

                Dv_SampleAnalystEndPivotalCount = dt_SampleAnalystFinalPilotPivotal.DefaultView()
                Dv_SampleAnalystEndPivotalCount.RowFilter = "vWorkTypeDesc='Pivotal'"
                Dv_SampleAnalystEndPivotalCount.ToTable().AcceptChanges()
                ViewState("SampleAnalystTotalPivotal") = Dv_SampleAnalystEndPivotalCount.ToTable()
                dt_SampleAanlystFinalEndPivotal = Dv_SampleAnalystEndPivotalCount.ToTable()
                LnkBtnSampleAnalystEndPivotal.Text = IIf(Convert.ToString(dt_SampleAanlystFinalEndPivotal.Compute("Sum(Samples)", True)).Trim() = "", "0", Convert.ToString(dt_SampleAanlystFinalEndPivotal.Compute("Sum(Samples)", True)).Trim())

                Dv_SampleAanlystEndPilotCount = dt_SampleAnalystFinalPilotPivotal.DefaultView()
                Dv_SampleAanlystEndPilotCount.RowFilter = "vWorkTypeDesc='Pilot'"
                Dv_SampleAanlystEndPilotCount.ToTable().AcceptChanges()
                ViewState("SampleAnalystTotalPilot") = Dv_SampleAanlystEndPilotCount.ToTable()
                dt_SampleAnalystFinalEndPilot = Dv_SampleAanlystEndPilotCount.ToTable()
                LnkBtnSampleAnalystEndPilot.Text = IIf(Convert.ToString(dt_SampleAnalystFinalEndPilot.Compute("Sum(Samples)", True)).Trim() = "", "0", Convert.ToString(dt_SampleAnalystFinalEndPilot.Compute("Sum(Samples)", True)).Trim())

                '''**** for Projects Report Released ******''''

                dt_ProjectsReleased = CType(Ds_OperationalKpi.Tables(0), DataTable)
                dv_ProjectsReleased = dt_ProjectsReleased.DefaultView()
                dv_ProjectsReleased.RowFilter = "ProjectStatus='T'"
                dv_ProjectsReleased.ToTable().AcceptChanges()
                ViewState("ProjectsReleased") = dv_ProjectsReleased.ToTable()
                dt_ClinicFinalForPilotPivotal = dv_ProjectsReleased.ToTable()


                LnkBtnProjectsReleased.Text = dv_ClinicTotal.ToTable().Rows.Count

                Dv_ProjectsReleasedPilotCount = dt_ClinicFinalForPilotPivotal.DefaultView()
                Dv_ProjectsReleasedPilotCount.RowFilter = "vWorkTypeDesc='Pilot'"
                Dv_ProjectsReleasedPilotCount.ToTable().AcceptChanges()
                LnkBtnProjectsReleasedPilot.Text = Dv_ProjectsReleasedPilotCount.ToTable().Rows.Count

                Dv_ProjectsReleasedPivotalCount = dt_ClinicFinalForPilotPivotal.DefaultView()
                Dv_ProjectsReleasedPivotalCount.RowFilter = "vWorkTypeDesc='Pivotal'"
                Dv_ProjectsReleasedPivotalCount.ToTable().AcceptChanges()
                LnkBtnProjectsReleasedPivotal.Text = Dv_ProjectsReleasedPivotalCount.ToTable().Rows.Count


            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "...GetKPI()")
        End Try
    End Sub

#Region "LinkButton For Operational Kpi"

    Protected Sub LnkBtnBedNightsTotal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnBedNightsTotal.Click
        Dim dt_BendNights As New DataTable
        Try
            dt_BendNights = CType(ViewState("BedNightsData"), DataTable)
            If dt_BendNights.Rows.Count = 0 Then
                dialogModalTitle.Text = "No Records Found"
                PanelForOperationalKpi.Visible = False
                GrdvgiewOfOperationalKpi.DataSource = Nothing
                GrdvgiewOfOperationalKpi.Visible = False
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
                mpeDialog.Show()
                Exit Sub
            End If
            PanelForOperationalKpi.Visible = True
            ViewState("FlagChk") = "BedNights"
            GrdvgiewOfOperationalKpi.DataSource = dt_BendNights
            ViewState("CommonForAllOperationalKpi") = dt_BendNights
            GrdvgiewOfOperationalKpi.DataBind()
            GrdvgiewOfOperationalKpi.Visible = True
            dialogModalTitle.Text = "Details Of No Of Bed Nights For Total Projects"
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "...LnkBtnBedNightsTotal.Click")
        End Try
    End Sub

    Protected Sub LnkBtnBedNightsPilot_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnBedNightsPilot.Click
        Try
            Dim dt_TotalBedNights As New DataTable
            Dim dv_GetPilotCountsData As DataView
            dt_TotalBedNights = CType(ViewState("BedNightsData"), DataTable)
            dv_GetPilotCountsData = dt_TotalBedNights.DefaultView()
            dv_GetPilotCountsData.RowFilter = "vWorkTypeDesc='Pilot'"
            dv_GetPilotCountsData.ToTable().AcceptChanges()
            If dv_GetPilotCountsData.ToTable().Rows.Count = 0 Then
                PanelForOperationalKpi.Visible = False
                dialogModalTitle.Text = "No Records Found"
                GrdvgiewOfOperationalKpi.DataSource = Nothing
                GrdvgiewOfOperationalKpi.Visible = False
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
                mpeDialog.Show()
                Exit Sub
            End If
            ViewState("FlagChk") = "BedNights"
            PanelForOperationalKpi.Visible = True
            GrdvgiewOfOperationalKpi.DataSource = dv_GetPilotCountsData.ToTable()
            ViewState("CommonForAllOperationalKpi") = dv_GetPilotCountsData.ToTable()
            GrdvgiewOfOperationalKpi.DataBind()
            GrdvgiewOfOperationalKpi.Visible = True
            dialogModalTitle.Text = "Details Of  No Of Bed Nights For Pilot Projects"
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "...LnkBtnBedNightsPilot.Click")
        End Try
    End Sub

    Protected Sub LnkBtnBedNightsPivotal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnBedNightsPivotal.Click
        Dim dt_TotalBedNights As New DataTable
        Dim dv_GetPivotalCountsData As DataView
        Try
            dt_TotalBedNights = CType(ViewState("BedNightsData"), DataTable)
            If dt_TotalBedNights.Rows.Count = 0 Then
                PanelForOperationalKpi.Visible = False
                dialogModalTitle.Text = "No Record Found"
                GrdvgiewOfOperationalKpi.DataSource = Nothing
                GrdvgiewOfOperationalKpi.Visible = False
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
                mpeDialog.Show()
                Exit Sub
            End If
            ViewState("FlagChk") = "BedNights"
            PanelForOperationalKpi.Visible = True
            dv_GetPivotalCountsData = dt_TotalBedNights.DefaultView()
            dv_GetPivotalCountsData.RowFilter = "vWorkTypeDesc='Pivotal'"
            dv_GetPivotalCountsData.ToTable().AcceptChanges()
            GrdvgiewOfOperationalKpi.DataSource = dv_GetPivotalCountsData.ToTable()
            ViewState("CommonForAllOperationalKpi") = dv_GetPivotalCountsData.ToTable()
            GrdvgiewOfOperationalKpi.DataBind()
            GrdvgiewOfOperationalKpi.Visible = True
            dialogModalTitle.Text = "Details Of  No Of Bed Nights For Pivotal Projects"
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "...LnkBtnBedNightsPivotal.Click")
        End Try
    End Sub

    Protected Sub LnkBtnClinicEndTotal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnClinicEndTotal.Click
        Dim dt_ClinicProjects As New DataTable
        dt_ClinicProjects = CType(ViewState("ClinicEndTotalData"), DataTable)
        If dt_ClinicProjects.Rows.Count = 0 Then
            PanelForOperationalKpi.Visible = False
            dialogModalTitle.Text = "No Records Found"
            GrdvgiewOfOperationalKpi.DataSource = Nothing
            GrdvgiewOfOperationalKpi.Visible = False
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
            Exit Sub
        End If
        ViewState("FlagChk") = "ClinicDate"
        PanelForOperationalKpi.Visible = True
        GrdvgiewOfOperationalKpi.DataSource = dt_ClinicProjects
        ViewState("CommonForAllOperationalKpi") = dt_ClinicProjects
        GrdvgiewOfOperationalKpi.DataBind()
        GrdvgiewOfOperationalKpi.Visible = True
        dialogModalTitle.Text = "Details Of No Of Projects Clinic End For Total Projects"
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
        mpeDialog.Show()
    End Sub

    Protected Sub LnkBtnClinicEndPilot_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnClinicEndPilot.Click
        Dim dt_ClinicEndTotalKpiData As New DataTable
        Dim dv_GetClinicPilotCountsData As DataView
        dt_ClinicEndTotalKpiData = CType(ViewState("ClinicEndTotalData"), DataTable)
        dv_GetClinicPilotCountsData = dt_ClinicEndTotalKpiData.DefaultView()
        dv_GetClinicPilotCountsData.RowFilter = "vWorkTypeDesc='Pilot'"
        dv_GetClinicPilotCountsData.ToTable().AcceptChanges()
        If dv_GetClinicPilotCountsData.ToTable().Rows.Count = 0 Then
            PanelForOperationalKpi.Visible = False
            dialogModalTitle.Text = "No Records Found"
            GrdvgiewOfOperationalKpi.DataSource = Nothing
            GrdvgiewOfOperationalKpi.Visible = False
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
            Exit Sub
        End If
        ViewState("FlagChk") = "ClinicDate"
        PanelForOperationalKpi.Visible = True
        GrdvgiewOfOperationalKpi.DataSource = dv_GetClinicPilotCountsData.ToTable()
        ViewState("CommonForAllOperationalKpi") = dv_GetClinicPilotCountsData.ToTable()
        GrdvgiewOfOperationalKpi.DataBind()
        GrdvgiewOfOperationalKpi.Visible = True
        dialogModalTitle.Text = "Details Of No Of Projects Clinic End For Pilot Projects"
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
        mpeDialog.Show()
    End Sub

    Protected Sub LnkBtnClinicEndPivotal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnClinicEndPivotal.Click
        Dim dt_ClinicEndTotalKpiData As New DataTable
        Dim dv_GetClinicPivotalCountsData As DataView
        dt_ClinicEndTotalKpiData = CType(ViewState("ClinicEndTotalData"), DataTable)
        dv_GetClinicPivotalCountsData = dt_ClinicEndTotalKpiData.DefaultView()
        dv_GetClinicPivotalCountsData.RowFilter = "vWorkTypeDesc='Pivotal'"
        dv_GetClinicPivotalCountsData.ToTable().AcceptChanges()
        If dv_GetClinicPivotalCountsData.ToTable().Rows.Count = 0 Then
            PanelForOperationalKpi.Visible = False
            dialogModalTitle.Text = "No Records Found"
            GrdvgiewOfOperationalKpi.DataSource = Nothing
            GrdvgiewOfOperationalKpi.Visible = False
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
            Exit Sub
        End If
        ViewState("FlagChk") = "ClinicDate"
        PanelForOperationalKpi.Visible = True
        GrdvgiewOfOperationalKpi.DataSource = dv_GetClinicPivotalCountsData.ToTable()
        ViewState("CommonForAllOperationalKpi") = dv_GetClinicPivotalCountsData.ToTable()
        GrdvgiewOfOperationalKpi.DataBind()
        GrdvgiewOfOperationalKpi.Visible = True
        dialogModalTitle.Text = "Details Of No Of Projects Clinic End For Pivotal Projects"
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
        mpeDialog.Show()
    End Sub

    Protected Sub LnkBtnSampleEndTotal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnSampleEndTotal.Click
        Dim dt_SampleProjects As New DataTable
        dt_SampleProjects = CType(ViewState("SampleEndTotalData"), DataTable)
        If dt_SampleProjects.Rows.Count = 0 Then
            dialogModalTitle.Text = "No Records Found"
            PanelForOperationalKpi.Visible = False
            GrdvgiewOfOperationalKpi.DataSource = Nothing
            GrdvgiewOfOperationalKpi.Visible = False
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
            Exit Sub
        End If
        ViewState("FlagChk") = "SampleAnalysis"
        PanelForOperationalKpi.Visible = True
        GrdvgiewOfOperationalKpi.DataSource = dt_SampleProjects
        ViewState("CommonForAllOperationalKpi") = dt_SampleProjects
        GrdvgiewOfOperationalKpi.DataBind()
        GrdvgiewOfOperationalKpi.Visible = True
        dialogModalTitle.Text = "Details Of No Of Projects Sample Analysis For Total Projects"
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
        mpeDialog.Show()

    End Sub

    Protected Sub LnkBtnSampleEndPilot_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnSampleEndPilot.Click
        Dim dt_SampleEndTotalKpiData As New DataTable
        Dim dv_GetSampleEndPilotCountsData As DataView
        dt_SampleEndTotalKpiData = CType(ViewState("SampleEndTotalData"), DataTable)
        dv_GetSampleEndPilotCountsData = dt_SampleEndTotalKpiData.DefaultView()
        dv_GetSampleEndPilotCountsData.RowFilter = "vWorkTypeDesc='Pilot'"
        dv_GetSampleEndPilotCountsData.ToTable().AcceptChanges()
        If dv_GetSampleEndPilotCountsData.ToTable().Rows.Count = 0 Then
            PanelForOperationalKpi.Visible = False
            dialogModalTitle.Text = "No Records Found"
            GrdvgiewOfOperationalKpi.DataSource = Nothing
            GrdvgiewOfOperationalKpi.Visible = False
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
            Exit Sub
        End If
        ViewState("FlagChk") = "SampleAnalysis"
        PanelForOperationalKpi.Visible = True
        GrdvgiewOfOperationalKpi.DataSource = dv_GetSampleEndPilotCountsData.ToTable()
        ViewState("CommonForAllOperationalKpi") = dv_GetSampleEndPilotCountsData.ToTable()
        GrdvgiewOfOperationalKpi.DataBind()
        GrdvgiewOfOperationalKpi.Visible = True
        dialogModalTitle.Text = "Details Of No Of Projects Sample Analysis For Pilot Projects"
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
        mpeDialog.Show()
    End Sub

    Protected Sub LnkBtnSampleEndPivotal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnSampleEndPivotal.Click
        Dim dt_SampleEndTotalKpiData As New DataTable
        Dim dv_GetSampleEndPivotalCountsData As DataView
        dt_SampleEndTotalKpiData = CType(ViewState("SampleEndTotalData"), DataTable)
        dv_GetSampleEndPivotalCountsData = dt_SampleEndTotalKpiData.DefaultView()
        dv_GetSampleEndPivotalCountsData.RowFilter = "vWorkTypeDesc='Pivotal'"
        dv_GetSampleEndPivotalCountsData.ToTable().AcceptChanges()
        If dv_GetSampleEndPivotalCountsData.ToTable().Rows.Count = 0 Then
            PanelForOperationalKpi.Visible = False
            dialogModalTitle.Text = "No Records Found"
            GrdvgiewOfOperationalKpi.DataSource = Nothing
            GrdvgiewOfOperationalKpi.Visible = False
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
            Exit Sub
        End If
        ViewState("FlagChk") = "SampleAnalysis"
        PanelForOperationalKpi.Visible = True
        GrdvgiewOfOperationalKpi.DataSource = dv_GetSampleEndPivotalCountsData.ToTable()
        ViewState("CommonForAllOperationalKpi") = dv_GetSampleEndPivotalCountsData.ToTable()
        GrdvgiewOfOperationalKpi.DataBind()
        GrdvgiewOfOperationalKpi.Visible = True
        dialogModalTitle.Text = "Details Of No Of Projects Sample Analysis For Pivotal Projects"
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
        mpeDialog.Show()
    End Sub

    Protected Sub LnkBtnReportEndTotal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnReportEndTotal.Click
        Dim dt_Projects As New DataTable
        dt_Projects = CType(ViewState("ReportEndTotal"), DataTable)
        If dt_Projects.Rows.Count = 0 Then
            PanelForOperationalKpi.Visible = False
            dialogModalTitle.Text = "No Records Found"
            GrdvgiewOfOperationalKpi.DataSource = Nothing
            GrdvgiewOfOperationalKpi.Visible = False
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
            Exit Sub
        End If
        ViewState("FlagChk") = "Report"
        PanelForOperationalKpi.Visible = True
        GrdvgiewOfOperationalKpi.DataSource = dt_Projects
        ViewState("CommonForAllOperationalKpi") = dt_Projects
        GrdvgiewOfOperationalKpi.DataBind()
        GrdvgiewOfOperationalKpi.Visible = True
        dialogModalTitle.Text = "Details Of No Of Projects Report Dispatch For Total Projects"
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
        mpeDialog.Show()
    End Sub

    Protected Sub LnkBtnReportEndPilot_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnReportEndPilot.Click
        Dim dt_ReportEndTotalKpiData As New DataTable
        Dim dv_GetReportEndPilotCountsData As DataView
        dt_ReportEndTotalKpiData = CType(ViewState("ReportEndTotal"), DataTable)
        If dt_ReportEndTotalKpiData Is Nothing Then
            PanelForOperationalKpi.Visible = False
            dialogModalTitle.Text = "No Records Found"
            GrdvgiewOfOperationalKpi.DataSource = Nothing
            GrdvgiewOfOperationalKpi.Visible = False
            Exit Sub
        End If
        ViewState("FlagChk") = "Report"
        PanelForOperationalKpi.Visible = True
        dv_GetReportEndPilotCountsData = dt_ReportEndTotalKpiData.DefaultView()
        dv_GetReportEndPilotCountsData.RowFilter = "vWorkTypeDesc='Pilot'"
        dv_GetReportEndPilotCountsData.ToTable().AcceptChanges()
        If dv_GetReportEndPilotCountsData.ToTable().Rows.Count = 0 Then
            PanelForOperationalKpi.Visible = False
            dialogModalTitle.Text = "No Records Found"
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
            Exit Sub
        End If
        GrdvgiewOfOperationalKpi.DataSource = dv_GetReportEndPilotCountsData.ToTable()
        PanelForOperationalKpi.Visible = True
        ViewState("CommonForAllOperationalKpi") = dv_GetReportEndPilotCountsData.ToTable()
        GrdvgiewOfOperationalKpi.DataBind()
        GrdvgiewOfOperationalKpi.Visible = True
        dialogModalTitle.Text = "Details Of No Of Projects Report Dispatch For Pilot Projects"
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
        mpeDialog.Show()
    End Sub

    Protected Sub LnkBtnReportEndPivotal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnReportEndPivotal.Click
        Dim dt_ReportEndTotalKpiData As New DataTable
        Dim dv_GetReportEndPivotalCountsData As DataView
        dt_ReportEndTotalKpiData = CType(ViewState("ReportEndTotal"), DataTable)
        dv_GetReportEndPivotalCountsData = dt_ReportEndTotalKpiData.DefaultView()
        dv_GetReportEndPivotalCountsData.RowFilter = "vWorkTypeDesc='Pivotal'"
        dv_GetReportEndPivotalCountsData.ToTable().AcceptChanges()
        If dv_GetReportEndPivotalCountsData.ToTable().Rows.Count = 0 Then
            PanelForOperationalKpi.Visible = False
            dialogModalTitle.Text = "No Records Found"
            GrdvgiewOfOperationalKpi.DataSource = Nothing
            GrdvgiewOfOperationalKpi.Visible = False
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
            Exit Sub
        End If
        ViewState("FlagChk") = "Report"
        PanelForOperationalKpi.Visible = True
        GrdvgiewOfOperationalKpi.DataSource = dv_GetReportEndPivotalCountsData.ToTable()
        ViewState("CommonForAllOperationalKpi") = dv_GetReportEndPivotalCountsData.ToTable()
        GrdvgiewOfOperationalKpi.DataBind()
        GrdvgiewOfOperationalKpi.Visible = True
        dialogModalTitle.Text = "Details Of No Of Projects Report Dispatch For Pivotal Projects"
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
        mpeDialog.Show()
    End Sub

    Protected Sub LnkBtnReportToSponsorReviewEndTotal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnReportToSponsorReviewEndTotal.Click
        Dim dt_SponsorProjects As New DataTable
        dt_SponsorProjects = CType(ViewState("SponsorReportEndTotal"), DataTable)
        If dt_SponsorProjects.Rows.Count = 0 Then
            PanelForOperationalKpi.Visible = False
            dialogModalTitle.Text = "No Records Found"
            GrdvgiewOfOperationalKpi.DataSource = Nothing
            GrdvgiewOfOperationalKpi.Visible = False
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
            Exit Sub
        End If
        ViewState("FlagChk") = "SponsorReport"
        PanelForOperationalKpi.Visible = True
        GrdvgiewOfOperationalKpi.DataSource = dt_SponsorProjects
        ViewState("CommonForAllOperationalKpi") = dt_SponsorProjects
        GrdvgiewOfOperationalKpi.DataBind()
        GrdvgiewOfOperationalKpi.Visible = True
        dialogModalTitle.Text = "Details Of No Of Projects Report To Sponsor For Total Projects"
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
        mpeDialog.Show()
    End Sub

    Protected Sub LnkBtnReportToSponsorReviewEndPivotal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnReportToSponsorReviewEndPivotal.Click
        Dim dt_SponsorReportEndTotalKpiData As New DataTable
        Dim dv_SponsorGetReportEndPivotalCountsData As DataView
        dt_SponsorReportEndTotalKpiData = CType(ViewState("SponsorReportEndTotal"), DataTable)
        dv_SponsorGetReportEndPivotalCountsData = dt_SponsorReportEndTotalKpiData.DefaultView()
        dv_SponsorGetReportEndPivotalCountsData.RowFilter = "vWorkTypeDesc='Pivotal'"
        dv_SponsorGetReportEndPivotalCountsData.ToTable().AcceptChanges()
        If dv_SponsorGetReportEndPivotalCountsData.ToTable().Rows.Count = 0 Then
            PanelForOperationalKpi.Visible = False
            dialogModalTitle.Text = "No Records Found"
            GrdvgiewOfOperationalKpi.DataSource = Nothing
            GrdvgiewOfOperationalKpi.Visible = False
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
            Exit Sub
        End If
        ViewState("FlagChk") = "SponsorReport"
        PanelForOperationalKpi.Visible = True
        GrdvgiewOfOperationalKpi.DataSource = dv_SponsorGetReportEndPivotalCountsData.ToTable()
        ViewState("CommonForAllOperationalKpi") = dv_SponsorGetReportEndPivotalCountsData.ToTable()
        GrdvgiewOfOperationalKpi.DataBind()
        GrdvgiewOfOperationalKpi.Visible = True
        dialogModalTitle.Text = "Details Of No Of Projects Report To Sponsor For Pivotal Projects"
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
        mpeDialog.Show()
    End Sub

    Protected Sub LnkBtnReportToSponsorReviewEndPilot_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnReportToSponsorReviewEndPilot.Click
        Dim dt_SponsorReportEndTotalKpiData As New DataTable
        Dim dv_SponsorGetReportEndPilotCountsData As DataView
        dt_SponsorReportEndTotalKpiData = CType(ViewState("SponsorReportEndTotal"), DataTable)
        If dt_SponsorReportEndTotalKpiData Is Nothing Then
            PanelForOperationalKpi.Visible = False
            dialogModalTitle.Text = "No Records Found"
            GrdvgiewOfOperationalKpi.DataSource = Nothing
            GrdvgiewOfOperationalKpi.Visible = False
            Exit Sub
        End If
        ViewState("FlagChk") = "SponsorReport"
        PanelForOperationalKpi.Visible = True
        dv_SponsorGetReportEndPilotCountsData = dt_SponsorReportEndTotalKpiData.DefaultView()
        dv_SponsorGetReportEndPilotCountsData.RowFilter = "vWorkTypeDesc='Pilot'"
        dv_SponsorGetReportEndPilotCountsData.ToTable().AcceptChanges()
        If dv_SponsorGetReportEndPilotCountsData.ToTable().Rows.Count = 0 Then
            PanelForOperationalKpi.Visible = False
            dialogModalTitle.Text = "No Records Found"
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
            Exit Sub
        End If
        GrdvgiewOfOperationalKpi.DataSource = dv_SponsorGetReportEndPilotCountsData.ToTable()
        PanelForOperationalKpi.Visible = True
        ViewState("CommonForAllOperationalKpi") = dv_SponsorGetReportEndPilotCountsData.ToTable()
        GrdvgiewOfOperationalKpi.DataBind()
        GrdvgiewOfOperationalKpi.Visible = True
        dialogModalTitle.Text = "Details Of No Of Projects Report To Sponsor For Pilot Projects"
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
        mpeDialog.Show()
    End Sub

    Protected Sub LnkBtnSubjectsDosedTtl_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LnkBtnSubjectsDosedTtl.Click
        Dim dt_SubjectsDosed As New DataTable
        dt_SubjectsDosed = CType(ViewState("SubjectsTotalDosed"), DataTable)
        If dt_SubjectsDosed.Rows.Count = 0 Then
            PanelForOperationalKpi.Visible = False
            dialogModalTitle.Text = "No Records Found"
            GrdvgiewOfOperationalKpi.DataSource = Nothing
            GrdvgiewOfOperationalKpi.Visible = False
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
            Exit Sub
        End If
        ViewState("FlagChk") = "TotalDosed"
        PanelForOperationalKpi.Visible = True
        GrdvgiewOfOperationalKpi.DataSource = dt_SubjectsDosed
        ViewState("CommonForAllOperationalKpi") = dt_SubjectsDosed
        GrdvgiewOfOperationalKpi.DataBind()
        GrdvgiewOfOperationalKpi.Visible = True
        dialogModalTitle.Text = "Details Of No Of Total Subjects Dosed "
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
        mpeDialog.Show()
    End Sub
    Protected Sub LnkBtnSubjectsDosedttlPilot_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LnkBtnSubjectsDosedttlPilot.Click
        Dim dt_SubjectsDosedTotalKpiData As New DataTable
        Dim dv_GetSubjectsDosedCountsData As DataView
        dt_SubjectsDosedTotalKpiData = CType(ViewState("SubjectsTotalDosed"), DataTable)
        dv_GetSubjectsDosedCountsData = dt_SubjectsDosedTotalKpiData.DefaultView()
        dv_GetSubjectsDosedCountsData.RowFilter = "vWorkTypeDesc='Pilot'"
        dv_GetSubjectsDosedCountsData.ToTable().AcceptChanges()
        If dv_GetSubjectsDosedCountsData.ToTable().Rows.Count = 0 Then
            PanelForOperationalKpi.Visible = False
            dialogModalTitle.Text = "No Records Found"
            GrdvgiewOfOperationalKpi.DataSource = Nothing
            GrdvgiewOfOperationalKpi.Visible = False
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
            Exit Sub
        End If
        ViewState("FlagChk") = "TotalDosed"
        PanelForOperationalKpi.Visible = True
        GrdvgiewOfOperationalKpi.DataSource = dv_GetSubjectsDosedCountsData.ToTable()
        ViewState("CommonForAllOperationalKpi") = dv_GetSubjectsDosedCountsData.ToTable()
        GrdvgiewOfOperationalKpi.DataBind()
        GrdvgiewOfOperationalKpi.Visible = True
        dialogModalTitle.Text = "Details Of No Of Total Subjects Dosed For Pilot Projects"
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
        mpeDialog.Show()
    End Sub

    Protected Sub LnkBtnSubjectsDosedTtlPivotal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LnkBtnSubjectsDosedTtlPivotal.Click
        Dim dt_SubjectsDosedTotalKpiData As New DataTable
        Dim dv_GetSubjectsDosedPivotalCountsData As DataView
        dt_SubjectsDosedTotalKpiData = CType(ViewState("SubjectsTotalDosed"), DataTable)
        dv_GetSubjectsDosedPivotalCountsData = dt_SubjectsDosedTotalKpiData.DefaultView()
        dv_GetSubjectsDosedPivotalCountsData.RowFilter = "vWorkTypeDesc='Pivotal'"
        dv_GetSubjectsDosedPivotalCountsData.ToTable().AcceptChanges()
        If dv_GetSubjectsDosedPivotalCountsData.ToTable().Rows.Count = 0 Then
            PanelForOperationalKpi.Visible = False
            dialogModalTitle.Text = "No Records Found"
            GrdvgiewOfOperationalKpi.DataSource = Nothing
            GrdvgiewOfOperationalKpi.Visible = False
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
            Exit Sub
        End If
        ViewState("FlagChk") = "TotalDosed"
        PanelForOperationalKpi.Visible = True
        GrdvgiewOfOperationalKpi.DataSource = dv_GetSubjectsDosedPivotalCountsData.ToTable()
        ViewState("CommonForAllOperationalKpi") = dv_GetSubjectsDosedPivotalCountsData.ToTable()
        GrdvgiewOfOperationalKpi.DataBind()
        GrdvgiewOfOperationalKpi.Visible = True
        dialogModalTitle.Text = "Details Of No Of Total Subjects Dosed For Pivotal Projects"
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
        mpeDialog.Show()
    End Sub

    Protected Sub LnkBtnSubjectsDosedTotal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnSubjectsDosedTotal.Click
        Dim dt_SubjectsDosed As New DataTable
        dt_SubjectsDosed = CType(ViewState("SubjectsDosed"), DataTable)
        If dt_SubjectsDosed.Rows.Count = 0 Then
            PanelForOperationalKpi.Visible = False
            dialogModalTitle.Text = "No Records Found"
            GrdvgiewOfOperationalKpi.DataSource = Nothing
            GrdvgiewOfOperationalKpi.Visible = False
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
            Exit Sub
        End If
        ViewState("FlagChk") = "DosedSubjects"
        PanelForOperationalKpi.Visible = True
        GrdvgiewOfOperationalKpi.DataSource = dt_SubjectsDosed
        ViewState("CommonForAllOperationalKpi") = dt_SubjectsDosed
        GrdvgiewOfOperationalKpi.DataBind()
        GrdvgiewOfOperationalKpi.Visible = True
        dialogModalTitle.Text = "Details Of No Of Subjects Dosed For Total Projects"
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
        mpeDialog.Show()
    End Sub

    Protected Sub LnkBtnSubjectsDosedPilot_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnSubjectsDosedPilot.Click
        Dim dt_SubjectsDosedTotalKpiData As New DataTable
        Dim dv_GetSubjectsDosedCountsData As DataView
        dt_SubjectsDosedTotalKpiData = CType(ViewState("SubjectsDosed"), DataTable)
        dv_GetSubjectsDosedCountsData = dt_SubjectsDosedTotalKpiData.DefaultView()
        dv_GetSubjectsDosedCountsData.RowFilter = "vWorkTypeDesc='Pilot'"
        dv_GetSubjectsDosedCountsData.ToTable().AcceptChanges()
        If dv_GetSubjectsDosedCountsData.ToTable().Rows.Count = 0 Then
            PanelForOperationalKpi.Visible = False
            dialogModalTitle.Text = "No Records Found"
            GrdvgiewOfOperationalKpi.DataSource = Nothing
            GrdvgiewOfOperationalKpi.Visible = False
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
            Exit Sub
        End If
        ViewState("FlagChk") = "DosedSubjects"
        PanelForOperationalKpi.Visible = True
        GrdvgiewOfOperationalKpi.DataSource = dv_GetSubjectsDosedCountsData.ToTable()
        ViewState("CommonForAllOperationalKpi") = dv_GetSubjectsDosedCountsData.ToTable()
        GrdvgiewOfOperationalKpi.DataBind()
        GrdvgiewOfOperationalKpi.Visible = True
        dialogModalTitle.Text = "Details Of No Of Subjects Dosed For Pilot Projects"
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
        mpeDialog.Show()
    End Sub

    Protected Sub LnkBtnSubjectsDosedPivotal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnSubjectsDosedPivotal.Click
        Dim dt_SubjectsDosedTotalKpiData As New DataTable
        Dim dv_GetSubjectsDosedPivotalCountsData As DataView
        dt_SubjectsDosedTotalKpiData = CType(ViewState("SubjectsDosed"), DataTable)
        dv_GetSubjectsDosedPivotalCountsData = dt_SubjectsDosedTotalKpiData.DefaultView()
        dv_GetSubjectsDosedPivotalCountsData.RowFilter = "vWorkTypeDesc='Pivotal'"
        dv_GetSubjectsDosedPivotalCountsData.ToTable().AcceptChanges()
        If dv_GetSubjectsDosedPivotalCountsData.ToTable().Rows.Count = 0 Then
            PanelForOperationalKpi.Visible = False
            dialogModalTitle.Text = "No Records Found"
            GrdvgiewOfOperationalKpi.DataSource = Nothing
            GrdvgiewOfOperationalKpi.Visible = False
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
            Exit Sub
        End If
        ViewState("FlagChk") = "DosedSubjects"
        PanelForOperationalKpi.Visible = True
        GrdvgiewOfOperationalKpi.DataSource = dv_GetSubjectsDosedPivotalCountsData.ToTable()
        ViewState("CommonForAllOperationalKpi") = dv_GetSubjectsDosedPivotalCountsData.ToTable()
        GrdvgiewOfOperationalKpi.DataBind()
        GrdvgiewOfOperationalKpi.Visible = True
        dialogModalTitle.Text = "Details Of No Of Subjects Dosed For Pivotal Projects"
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
        mpeDialog.Show()
    End Sub


    Protected Sub lnkbtnsampleanalystendpilot_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnSampleAnalystEndPilot.Click
        Dim dt_sampleanalystdataforpilot As New DataTable

        dt_sampleanalystdataforpilot = CType(ViewState("SampleAnalystTotalPilot"), DataTable)
        If dt_sampleanalystdataforpilot.Rows.Count = 0 Then
            PanelForOperationalKpi.Visible = False
            dialogModalTitle.Text = "no records found"
            GrdvgiewOfOperationalKpi.DataSource = Nothing
            GrdvgiewOfOperationalKpi.Visible = False
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
            Exit Sub
        End If
        ViewState("flagchk") = "SampleAnalyst"
        PanelForOperationalKpi.Visible = True
        GrdvgiewOfOperationalKpi.DataSource = dt_sampleanalystdataforpilot
        ViewState("commonforalloperationalkpi") = dt_sampleanalystdataforpilot
        GrdvgiewOfOperationalKpi.DataBind()
        GrdvgiewOfOperationalKpi.Visible = True
        dialogModalTitle.Text = "details of no of total sample analysis for pilot projects"
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
        mpeDialog.Show()
    End Sub

    Protected Sub LnkBtnSampleAnalystEndTotal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnSampleAnalystEndTotal.Click
        Dim dt_SampleanalystTotal As New DataTable
        dt_SampleanalystTotal = CType(ViewState("SampleAnalystTotal"), DataTable)
        If dt_SampleanalystTotal.Rows.Count = 0 Then
            PanelForOperationalKpi.Visible = False
            dialogModalTitle.Text = "No Records Found"
            GrdvgiewOfOperationalKpi.DataSource = Nothing
            GrdvgiewOfOperationalKpi.Visible = False
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
            Exit Sub
        End If
        ViewState("FlagChk") = "SampleAnalyst"
        PanelForOperationalKpi.Visible = True
        GrdvgiewOfOperationalKpi.DataSource = dt_SampleanalystTotal
        ViewState("CommonForAllOperationalKpi") = dt_SampleanalystTotal
        GrdvgiewOfOperationalKpi.DataBind()
        GrdvgiewOfOperationalKpi.Visible = True
        dialogModalTitle.Text = "Details Of No Of Total Sample Analysis"
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
        mpeDialog.Show()
    End Sub

    Protected Sub LnkBtnSampleAnalystEndPivotal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnSampleAnalystEndPivotal.Click
        Dim dt_SampleAnalystDataForPivotal As New DataTable

        dt_SampleAnalystDataForPivotal = CType(ViewState("SampleAnalystTotalPivotal"), DataTable)
        If dt_SampleAnalystDataForPivotal.Rows.Count = 0 Then
            PanelForOperationalKpi.Visible = False
            dialogModalTitle.Text = "No Records Found"
            GrdvgiewOfOperationalKpi.DataSource = Nothing
            GrdvgiewOfOperationalKpi.Visible = False
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
            Exit Sub
        End If
        ViewState("FlagChk") = "SampleAnalyst"
        PanelForOperationalKpi.Visible = True
        GrdvgiewOfOperationalKpi.DataSource = dt_SampleAnalystDataForPivotal
        ViewState("CommonForAllOperationalKpi") = dt_SampleAnalystDataForPivotal
        GrdvgiewOfOperationalKpi.DataBind()
        GrdvgiewOfOperationalKpi.Visible = True
        dialogModalTitle.Text = "Details Of No Of Total Sample Analysis For Pivotal Projects"
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
        mpeDialog.Show()
    End Sub

    Protected Sub LnkBtnProjectsReleased_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LnkBtnProjectsReleased.Click
        Dim dt As New DataTable
        dt = CType(ViewState("ProjectsReleased"), DataTable)
        If dt.Rows.Count = 0 Then
            PanelForOperationalKpi.Visible = False
            dialogModalTitle.Text = "No Records Found"
            GrdvgiewOfOperationalKpi.DataSource = Nothing
            GrdvgiewOfOperationalKpi.Visible = False
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
            Exit Sub
        End If
        ViewState("FlagChk") = "ProjectsReleased"
        PanelForOperationalKpi.Visible = True
        GrdvgiewOfOperationalKpi.DataSource = dt
        ViewState("CommonForAllOperationalKpi") = dt
        GrdvgiewOfOperationalKpi.DataBind()
        GrdvgiewOfOperationalKpi.Visible = True
        dialogModalTitle.Text = "Details Of No Of Total Subjects Dosed "
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
        mpeDialog.Show()
    End Sub

    Protected Sub LnkBtnProjectsReleasedPilot_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LnkBtnProjectsReleasedPilot.Click
        Dim dt As New DataTable
        Dim dv As DataView
        dt = CType(ViewState("ProjectsReleased"), DataTable)
        dv = dt.DefaultView()
        dv.RowFilter = "vWorkTypeDesc='Pilot'"
        dv.ToTable().AcceptChanges()
        If dv.ToTable().Rows.Count = 0 Then
            PanelForOperationalKpi.Visible = False
            dialogModalTitle.Text = "No Records Found"
            GrdvgiewOfOperationalKpi.DataSource = Nothing
            GrdvgiewOfOperationalKpi.Visible = False
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
            Exit Sub
        End If
        ViewState("FlagChk") = "ProjectsReleased"
        PanelForOperationalKpi.Visible = True
        GrdvgiewOfOperationalKpi.DataSource = dv.ToTable()
        ViewState("CommonForAllOperationalKpi") = dv.ToTable()
        GrdvgiewOfOperationalKpi.DataBind()
        GrdvgiewOfOperationalKpi.Visible = True
        dialogModalTitle.Text = "Details Of No Of Total Subjects Dosed For Pilot Projects"
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
        mpeDialog.Show()
    End Sub

    Protected Sub LnkBtnProjectsReleasedPivotal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LnkBtnProjectsReleasedPivotal.Click
        Dim dt As New DataTable
        Dim dv As DataView
        dt = CType(ViewState("ProjectsReleased"), DataTable)
        dv = dt.DefaultView()
        dv.RowFilter = "vWorkTypeDesc='Pivotal'"
        dv.ToTable().AcceptChanges()
        If dv.ToTable().Rows.Count = 0 Then
            PanelForOperationalKpi.Visible = False
            dialogModalTitle.Text = "No Records Found"
            GrdvgiewOfOperationalKpi.DataSource = Nothing
            GrdvgiewOfOperationalKpi.Visible = False
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
            mpeDialog.Show()
            Exit Sub
        End If
        ViewState("FlagChk") = "ProjectsReleased"
        PanelForOperationalKpi.Visible = True
        GrdvgiewOfOperationalKpi.DataSource = dv.ToTable()
        ViewState("CommonForAllOperationalKpi") = dv.ToTable()
        GrdvgiewOfOperationalKpi.DataBind()
        GrdvgiewOfOperationalKpi.Visible = True
        dialogModalTitle.Text = "Details Of No Of Total Subjects Dosed For Pivotal Projects"
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divOperationalKpi');", True)
        mpeDialog.Show()
    End Sub
#End Region

#Region "LinkButton For Summary"

    Protected Sub LnkBtnProjectsCountsForES_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnProjectsCountsForES.Click
        Dim yValues(1) As Double
        Dim xValues(1) As String
        Dim xValuesForSA(1) As String
        Dim yValuesForSA(1) As Double

        If ViewState("SelectedActivity") = "SA" Then
            xValues(0) = "On Time :" & ViewState("OnTimeForESValue").ToString()
            xValues(1) = "Delayed :" & ViewState("DelayedForESValue").ToString()
            yValues(0) = ViewState("OnTimeForESValue")
            yValues(1) = ViewState("DelayedForESValue")

            Chart1.Series("Default").Points.DataBindXY(xValues, yValues)
            Chart1.Series("Default").ChartType = DataVisualization.Charting.SeriesChartType.Doughnut
            Chart1.Series("Default")("PieLabelStyle") = "OutSide"
            Chart1.BackGradientStyle = GradientStyle.HorizontalCenter
            Chart1.BackImageTransparentColor = Color.Blue
            Chart1.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
            Chart1.Titles("Title1").Text = "From End Study"

            Chart1.Visible = True
            Chart1.Width = "250"
            Chart1.Height = "170"

            Chart2.Visible = False

        ElseIf ViewState("SelectedActivity") = "RP" Then
            xValues(0) = "On Time :" & ViewState("OnTimeForESValue").ToString()
            xValues(1) = "Delayed :" & ViewState("DelayedForESValue").ToString()
            xValuesForSA(0) = "On Time :" & ViewState("OnTimeForSAValue").ToString()
            xValuesForSA(1) = "Delayed :" & ViewState("DelayedForSAValue").ToString()
            yValues(0) = ViewState("OnTimeForESValue").ToString()
            yValues(1) = ViewState("DelayedForESValue").ToString()

            yValuesForSA(0) = ViewState("OnTimeForSAValue").ToString()
            yValuesForSA(1) = ViewState("DelayedForSAValue").ToString()

            Chart1.Series("Default").Points.DataBindXY(xValues, yValues)
            Chart1.Series("Default").ChartType = DataVisualization.Charting.SeriesChartType.Doughnut
            Chart1.Series("Default")("PieLabelStyle") = "OutSide"
            Chart1.BackGradientStyle = GradientStyle.HorizontalCenter
            Chart1.BackImageTransparentColor = Color.Blue
            Chart1.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
            Chart1.Titles("Title1").Text = "From End Study"
            Chart1.Visible = True
            Chart1.Width = "250"
            Chart1.Height = "170"

            Chart2.Series("Default").Points.DataBindXY(xValuesForSA, yValuesForSA)
            Chart2.Series("Default").ChartType = DataVisualization.Charting.SeriesChartType.Doughnut
            Chart2.Series("Default")("PieLabelStyle") = "OutSide"

            Chart2.BackGradientStyle = GradientStyle.HorizontalCenter
            Chart2.BackImageTransparentColor = Color.Blue
            Chart2.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
            Chart2.Titles("Title1").Text = "From Sample Analysis"
            Chart2.Visible = True
            Chart2.Width = "250"
            Chart2.Height = "170"
        End If

        LblProjectNoPopUpWorkSummary.Text = ViewState("TotalProjectList").ToString()

        If Not LblProjectNoPopUpWorkSummary.Text = String.Empty Then
            If LblProjectNoPopUpWorkSummary.Text.Substring(LblProjectNoPopUpWorkSummary.Text.Length - 3) = "," & vbCrLf Then
                LblProjectNoPopUpWorkSummary.Text = LblProjectNoPopUpWorkSummary.Text.Substring(0, LblProjectNoPopUpWorkSummary.Text.Length - 3).ToString()
                LblPopUpTitleWorkSummary.Text = "Total Project No List :"
                LblPopUpTitleWorkSummary.Visible = True
            End If
        Else
            LblPopUpTitleWorkSummary.Text = "There Is No Any Projects"
            LblPopUpTitleWorkSummary.Visible = True
        End If


        LblProjectNoPopUpWorkSummary.Visible = True
        Mpedialog2.Show()


    End Sub

    Protected Sub LnkBtnDelayedCountsForES_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnDelayedCountsForES.Click
        Dim yValues(1) As Double
        Dim xValues(1) As String
        Dim xValuesForSA(1) As String
        Dim yValuesForSA(1) As Double
        If ViewState("SelectedActivity") = "SA" Then
            xValues(0) = "On Time :" & ViewState("OnTimeForESValue").ToString()
            xValues(1) = "Delayed :" & ViewState("DelayedForESValue").ToString()
            yValues(0) = ViewState("OnTimeForESValue")
            yValues(1) = ViewState("DelayedForESValue")

            Chart1.Series("Default").Points.DataBindXY(xValues, yValues)
            Chart1.Series("Default").ChartType = DataVisualization.Charting.SeriesChartType.Doughnut
            Chart1.Series("Default")("PieLabelStyle") = "OutSide"
            Chart1.BackGradientStyle = GradientStyle.HorizontalCenter
            Chart1.BackImageTransparentColor = Color.Blue
            Chart1.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
            Chart1.Titles("Title1").Text = "From End Study"

            Chart1.Visible = True
            Chart1.Width = "250"
            Chart1.Height = "170"

            Chart2.Visible = False
        ElseIf ViewState("SelectedActivity") = "RP" Then
            xValues(0) = "On Time :" & ViewState("OnTimeForESValue").ToString()
            xValues(1) = "Delayed :" & ViewState("DelayedForESValue").ToString()
            xValuesForSA(0) = "On Time :" & ViewState("OnTimeForSAValue").ToString()
            xValuesForSA(1) = "Delayed :" & ViewState("DelayedForSAValue").ToString()
            yValues(0) = ViewState("OnTimeForESValue").ToString()
            yValues(1) = ViewState("DelayedForESValue").ToString()

            yValuesForSA(0) = ViewState("OnTimeForSAValue").ToString()
            yValuesForSA(1) = ViewState("DelayedForSAValue").ToString()

            Chart1.Series("Default").Points.DataBindXY(xValues, yValues)
            Chart1.Series("Default").ChartType = DataVisualization.Charting.SeriesChartType.Doughnut
            Chart1.Series("Default")("PieLabelStyle") = "OutSide"
            Chart1.BackGradientStyle = GradientStyle.HorizontalCenter
            Chart1.BackImageTransparentColor = Color.Blue
            Chart1.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
            Chart1.Titles("Title1").Text = "From End Study"
            Chart1.Visible = True
            Chart1.Width = "250"
            Chart1.Height = "170"

            Chart2.Series("Default").Points.DataBindXY(xValuesForSA, yValuesForSA)
            Chart2.Series("Default").ChartType = DataVisualization.Charting.SeriesChartType.Doughnut
            Chart2.Series("Default")("PieLabelStyle") = "OutSide"

            Chart2.BackGradientStyle = GradientStyle.HorizontalCenter
            Chart2.BackImageTransparentColor = Color.Blue
            Chart2.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
            Chart2.Titles("Title1").Text = "From Sample Analysis"
            Chart2.Visible = True
            Chart2.Width = "250"
            Chart2.Height = "170"
        End If
        LblProjectNoPopUpWorkSummary.Text = ViewState("DelayedProjectListOfES").ToString()
        If Not LblProjectNoPopUpWorkSummary.Text = String.Empty Then
            If LblProjectNoPopUpWorkSummary.Text.Substring(LblProjectNoPopUpWorkSummary.Text.Length - 3) = "," & vbCrLf Then
                LblProjectNoPopUpWorkSummary.Text = LblProjectNoPopUpWorkSummary.Text.Substring(0, LblProjectNoPopUpWorkSummary.Text.Length - 3).ToString()
                LblPopUpTitleWorkSummary.Text = "Project No List For Delyed From End Study :"
                LblPopUpTitleWorkSummary.Visible = True
            End If
        Else
            LblPopUpTitleWorkSummary.Text = "There Is No Any Projects"
            LblPopUpTitleWorkSummary.Visible = True
        End If
        LblProjectNoPopUpWorkSummary.Visible = True
        Mpedialog2.Show()
    End Sub

    Protected Sub LnkBtnDelayedCountsForSA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnDelayedCountsForSA.Click
        Dim yValues(1) As Double
        Dim xValues(1) As String
        Dim xValuesForSA(1) As String
        Dim yValuesForSA(1) As Double
        If ViewState("SelectedActivity") = "SA" Then
            xValues(0) = "On Time :" & ViewState("OnTimeForESValue").ToString()
            xValues(1) = "Delayed :" & ViewState("DelayedForESValue").ToString()
            yValues(0) = ViewState("OnTimeForESValue")
            yValues(1) = ViewState("DelayedForESValue")

            Chart1.Series("Default").Points.DataBindXY(xValues, yValues)
            Chart1.Series("Default").ChartType = DataVisualization.Charting.SeriesChartType.Doughnut
            Chart1.Series("Default")("PieLabelStyle") = "OutSide"
            Chart1.BackGradientStyle = GradientStyle.HorizontalCenter
            Chart1.BackImageTransparentColor = Color.Blue
            Chart1.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
            Chart1.Titles("Title1").Text = "From End Study"

            Chart1.Visible = True
            Chart1.Width = "250"
            Chart1.Height = "170"

            Chart2.Visible = False
        ElseIf ViewState("SelectedActivity") = "RP" Then
            xValues(0) = "On Time :" & ViewState("OnTimeForESValue").ToString()
            xValues(1) = "Delayed :" & ViewState("DelayedForESValue").ToString()
            xValuesForSA(0) = "On Time :" & ViewState("OnTimeForSAValue").ToString()
            xValuesForSA(1) = "Delayed :" & ViewState("DelayedForSAValue").ToString()
            yValues(0) = ViewState("OnTimeForESValue").ToString()
            yValues(1) = ViewState("DelayedForESValue").ToString()

            yValuesForSA(0) = ViewState("OnTimeForSAValue").ToString()
            yValuesForSA(1) = ViewState("DelayedForSAValue").ToString()

            Chart1.Series("Default").Points.DataBindXY(xValues, yValues)
            Chart1.Series("Default").ChartType = DataVisualization.Charting.SeriesChartType.Doughnut
            Chart1.Series("Default")("PieLabelStyle") = "OutSide"
            Chart1.BackGradientStyle = GradientStyle.HorizontalCenter
            Chart1.BackImageTransparentColor = Color.Blue
            Chart1.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
            Chart1.Titles("Title1").Text = "From End Study"
            Chart1.Visible = True
            Chart1.Width = "250"
            Chart1.Height = "170"

            Chart2.Series("Default").Points.DataBindXY(xValuesForSA, yValuesForSA)
            Chart2.Series("Default").ChartType = DataVisualization.Charting.SeriesChartType.Doughnut
            Chart2.Series("Default")("PieLabelStyle") = "OutSide"

            Chart2.BackGradientStyle = GradientStyle.HorizontalCenter
            Chart2.BackImageTransparentColor = Color.Blue
            Chart2.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
            Chart2.Titles("Title1").Text = "From Sample Analysis"
            Chart2.Visible = True
            Chart2.Width = "250"
            Chart2.Height = "170"
        End If


        LblProjectNoPopUpWorkSummary.Text = ViewState("DelayedProjectListOfSA").ToString()

        If Not LblProjectNoPopUpWorkSummary.Text = String.Empty Then
            If LblProjectNoPopUpWorkSummary.Text.Substring(LblProjectNoPopUpWorkSummary.Text.Length - 3) = "," & vbCrLf Then
                LblProjectNoPopUpWorkSummary.Text = LblProjectNoPopUpWorkSummary.Text.Substring(0, LblProjectNoPopUpWorkSummary.Text.Length - 3).ToString()
                LblPopUpTitleWorkSummary.Text = "Project No List For Delyed From Sample Analysis :"
                LblPopUpTitleWorkSummary.Visible = True
            End If
        Else
            LblPopUpTitleWorkSummary.Text = "There Is No Any Projects"
            LblPopUpTitleWorkSummary.Visible = True
        End If
        LblProjectNoPopUpWorkSummary.Visible = True
        Mpedialog2.Show()
    End Sub


    Protected Sub LnkBtnOnTimeCountsForES_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnOnTimeCountsForES.Click
        Dim yValues(1) As Double
        Dim xValues(1) As String
        Dim xValuesForSA(1) As String
        Dim yValuesForSA(1) As Double
        If ViewState("SelectedActivity") = "SA" Then
            xValues(0) = "On Time :" & ViewState("OnTimeForESValue").ToString()
            xValues(1) = "Delayed :" & ViewState("DelayedForESValue").ToString()
            yValues(0) = ViewState("OnTimeForESValue")
            yValues(1) = ViewState("DelayedForESValue")

            Chart1.Series("Default").Points.DataBindXY(xValues, yValues)
            Chart1.Series("Default").ChartType = DataVisualization.Charting.SeriesChartType.Doughnut
            Chart1.Series("Default")("PieLabelStyle") = "OutSide"
            Chart1.BackGradientStyle = GradientStyle.HorizontalCenter
            Chart1.BackImageTransparentColor = Color.Blue
            Chart1.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
            Chart1.Titles("Title1").Text = "From End Study"

            Chart1.Visible = True
            Chart1.Width = "250"
            Chart1.Height = "170"

            Chart2.Visible = False
        ElseIf ViewState("SelectedActivity") = "RP" Then
            xValues(0) = "On Time :" & ViewState("OnTimeForESValue").ToString()
            xValues(1) = "Delayed :" & ViewState("DelayedForESValue").ToString()
            xValuesForSA(0) = "On Time :" & ViewState("OnTimeForSAValue").ToString()
            xValuesForSA(1) = "Delayed :" & ViewState("DelayedForSAValue").ToString()
            yValues(0) = ViewState("OnTimeForESValue").ToString()
            yValues(1) = ViewState("DelayedForESValue").ToString()

            yValuesForSA(0) = ViewState("OnTimeForSAValue").ToString()
            yValuesForSA(1) = ViewState("DelayedForSAValue").ToString()

            Chart1.Series("Default").Points.DataBindXY(xValues, yValues)
            Chart1.Series("Default").ChartType = DataVisualization.Charting.SeriesChartType.Doughnut
            Chart1.Series("Default")("PieLabelStyle") = "OutSide"
            Chart1.BackGradientStyle = GradientStyle.HorizontalCenter
            Chart1.BackImageTransparentColor = Color.Blue
            Chart1.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
            Chart1.Titles("Title1").Text = "From End Study"
            Chart1.Visible = True
            Chart1.Width = "250"
            Chart1.Height = "170"

            Chart2.Series("Default").Points.DataBindXY(xValuesForSA, yValuesForSA)
            Chart2.Series("Default").ChartType = DataVisualization.Charting.SeriesChartType.Doughnut
            Chart2.Series("Default")("PieLabelStyle") = "OutSide"

            Chart2.BackGradientStyle = GradientStyle.HorizontalCenter
            Chart2.BackImageTransparentColor = Color.Blue
            Chart2.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
            Chart2.Titles("Title1").Text = "From Sample Analysis"
            Chart2.Visible = True
            Chart2.Width = "250"
            Chart2.Height = "170"
        End If


        LblProjectNoPopUpWorkSummary.Text = ViewState("OnTimeProjectListOfES").ToString()
        If Not LblProjectNoPopUpWorkSummary.Text = String.Empty Then
            If LblProjectNoPopUpWorkSummary.Text.Substring(LblProjectNoPopUpWorkSummary.Text.Length - 3) = "," & vbCrLf Then
                LblProjectNoPopUpWorkSummary.Text = LblProjectNoPopUpWorkSummary.Text.Substring(0, LblProjectNoPopUpWorkSummary.Text.Length - 3).ToString()

                LblPopUpTitleWorkSummary.Text = "Project No List For On Time From End Study :"
                LblPopUpTitleWorkSummary.Visible = True
            End If
        Else
            LblPopUpTitleWorkSummary.Text = "There Is No Any Projects"
            LblPopUpTitleWorkSummary.Visible = True
        End If

        LblProjectNoPopUpWorkSummary.Visible = True
        Mpedialog2.Show()

    End Sub

    Protected Sub LnkBtnOnTimeCountsForSA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnOnTimeCountsForSA.Click
        Dim yValues(1) As Double
        Dim xValues(1) As String
        Dim xValuesForSA(1) As String
        Dim yValuesForSA(1) As Double
        If ViewState("SelectedActivity") = "SA" Then
            xValues(0) = "On Time :" & ViewState("OnTimeForESValue").ToString()
            xValues(1) = "Delayed :" & ViewState("DelayedForESValue").ToString()
            yValues(0) = ViewState("OnTimeForESValue")
            yValues(1) = ViewState("DelayedForESValue")

            Chart1.Series("Default").Points.DataBindXY(xValues, yValues)
            Chart1.Series("Default").ChartType = DataVisualization.Charting.SeriesChartType.Doughnut
            Chart1.Series("Default")("PieLabelStyle") = "OutSide"
            Chart1.BackGradientStyle = GradientStyle.HorizontalCenter
            Chart1.BackImageTransparentColor = Color.Blue
            Chart1.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
            Chart1.Titles("Title1").Text = "From End Study"

            Chart1.Visible = True
            Chart1.Width = "250"
            Chart1.Height = "170"

            Chart2.Visible = False
        ElseIf ViewState("SelectedActivity") = "RP" Then
            xValues(0) = "On Time :" & ViewState("OnTimeForESValue").ToString()
            xValues(1) = "Delayed :" & ViewState("DelayedForESValue").ToString()
            xValuesForSA(0) = "On Time :" & ViewState("OnTimeForSAValue").ToString()
            xValuesForSA(1) = "Delayed :" & ViewState("DelayedForSAValue").ToString()
            yValues(0) = ViewState("OnTimeForESValue").ToString()
            yValues(1) = ViewState("DelayedForESValue").ToString()

            yValuesForSA(0) = ViewState("OnTimeForSAValue").ToString()
            yValuesForSA(1) = ViewState("DelayedForSAValue").ToString()

            Chart1.Series("Default").Points.DataBindXY(xValues, yValues)
            Chart1.Series("Default").ChartType = DataVisualization.Charting.SeriesChartType.Doughnut
            Chart1.Series("Default")("PieLabelStyle") = "OutSide"
            Chart1.BackGradientStyle = GradientStyle.HorizontalCenter
            Chart1.BackImageTransparentColor = Color.Blue
            Chart1.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
            Chart1.Titles("Title1").Text = "From End Study"
            Chart1.Visible = True
            Chart1.Width = "250"
            Chart1.Height = "170"

            Chart2.Series("Default").Points.DataBindXY(xValuesForSA, yValuesForSA)
            Chart2.Series("Default").ChartType = DataVisualization.Charting.SeriesChartType.Doughnut
            Chart2.Series("Default")("PieLabelStyle") = "OutSide"

            Chart2.BackGradientStyle = GradientStyle.HorizontalCenter
            Chart2.BackImageTransparentColor = Color.Blue
            Chart2.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
            Chart2.Titles("Title1").Text = "From Sample Analysis"
            Chart2.Visible = True
            Chart2.Width = "250"
            Chart2.Height = "170"
        End If


        LblProjectNoPopUpWorkSummary.Text = ViewState("OnTimeProjectListOfSA").ToString()

        If Not LblProjectNoPopUpWorkSummary.Text = String.Empty Then
            If LblProjectNoPopUpWorkSummary.Text.Substring(LblProjectNoPopUpWorkSummary.Text.Length - 3) = "," & vbCrLf Then
                LblProjectNoPopUpWorkSummary.Text = LblProjectNoPopUpWorkSummary.Text.Substring(0, LblProjectNoPopUpWorkSummary.Text.Length - 3).ToString()
                LblPopUpTitleWorkSummary.Text = "Project No List For On Time From Sample Analysis"
                LblPopUpTitleWorkSummary.Visible = True
            End If
        Else
            LblPopUpTitleWorkSummary.Text = "There Is No Any Projects"
            LblPopUpTitleWorkSummary.Visible = True
        End If


        LblProjectNoPopUpWorkSummary.Visible = True
        Mpedialog2.Show()

    End Sub

#End Region

    Protected Sub ddlLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlLocation.SelectedIndexChanged
        GetCounts()
    End Sub

    Protected Sub ddlProjectManager_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlProjectManager.SelectedIndexChanged '' added by prayag
        GetCounts()
    End Sub

#Region "lnkButton events"  '' Added by dipen shah For fill the grid.
    Protected Sub lnkGenericScreening_click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim FirstDateOfCurrMonth As Date
        Dim CurrMonthLastDate As Integer
        Dim LastDateOfCurrMonth As Date
        Dim PrevMonthLastDate As Integer
        Dim FirstDateOfPrevMonth As Date
        Dim IndexNo As Integer
        Dim task As String
        Dim Dt As DataTable
        Dim LastDateOfPrevMonth As Date
        Dim I As Int32
        Dim LastMonth As String = String.Empty
        Dim LastYear As String = String.Empty
        Dim ds As DataSet = Nothing
        Dim Parameters As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim HeaderText As String = String.Empty
        Dim gvRow As GridViewRow
        Dim type As String = "1"
        Try
            grdSubjectInfo.Dispose()

            FirstDateOfCurrMonth = CDate("01" & "-" & DdllistMonth.SelectedItem.ToString().Trim() & "-" & DdllistYear.SelectedItem.ToString())
            CurrMonthLastDate = CInt(DateTime.DaysInMonth(DdllistYear.SelectedItem.ToString(), CInt(DdllistMonth.SelectedValue)))
            LastDateOfCurrMonth = CDate(CurrMonthLastDate & "-" & DdllistMonth.SelectedItem.ToString().Trim() & "-" & DdllistYear.SelectedItem.ToString())
            LastMonth = MonthName(CInt(CDate(FirstDateOfCurrMonth).AddMonths(-1).Month))
            LastYear = CInt(CDate(FirstDateOfCurrMonth).AddMonths(-1).Year.ToString())
            FirstDateOfPrevMonth = CDate("01" & "-" & LastMonth.ToString.Trim() & "-" & LastYear.ToString().Trim())
            PrevMonthLastDate = CInt(DateTime.DaysInMonth(CInt(LastYear), CInt(CDate(FirstDateOfPrevMonth).Month)))
            LastDateOfPrevMonth = CDate(PrevMonthLastDate & "-" & LastMonth.ToString.Trim() & "-" & LastYear.ToString.Trim())
            gvRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            I = gvRow.RowIndex

            If I <= 9 Then
                task = Convert.ToString(gvwpnlScreeningAnalytic.Rows(I).Cells(0).Text)
                lbltask.Text = "Data For: " & task.ToString()
                IndexNo = I.ToString()
                Parameters = type + "##" + IndexNo.ToString + "##" + Format(FirstDateOfCurrMonth, "dd/MMM/yyyy") + "##" + Format(LastDateOfCurrMonth, "dd/MMM/yyyy") + "##" + Format(FirstDateOfPrevMonth, "dd/MMM/yyyy") + "##" + Format(LastDateOfPrevMonth, "dd/MMM/yyyy") + "##" + ddlLocation.SelectedValue()
            Else
                objCommon.ShowAlert("Data Not Found ", Me.Page)
            End If

            If Not Me.objHelpDbTable.Proc_SubjectScreeningInfo(Parameters, ds, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If
            Dt = ds.Tables(0)
            ViewState("VS_grvSubjectInfo") = Dt


            grdSubjectInfo.DataSource = Nothing
            grdSubjectInfo.DataBind()

            If grdSubjectInfo.Rows.Count > 0 Or Not grdSubjectInfo.DataSource Is Nothing Then
                grdSubjectInfo.UseAccessibleHeader = True
                grdSubjectInfo.HeaderRow.TableSection = TableRowSection.TableHeader
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gvtodt_grdSubjectInfo", "gvtodt(ctl00_CPHLAMBDA_grdSubjectInfo); ", True)
            End If

            If Convert.ToString(gvwpnlScreeningAnalytic.Rows(I).Cells(0).Text) = "Fresh Dosing" Then
                Me.grdSubjectInfo.Columns(0).Visible = True
            ElseIf Convert.ToString(gvwpnlScreeningAnalytic.Rows(I).Cells(0).Text) = "Opening Balance" Then
                Me.grdSubjectInfo.Columns(0).Visible = True
            ElseIf Convert.ToString(gvwpnlScreeningAnalytic.Rows(I).Cells(0).Text) = "Closing Balance" Then
                Me.grdSubjectInfo.Columns(0).Visible = True
            ElseIf Convert.ToString(gvwpnlScreeningAnalytic.Rows(I).Cells(0).Text) = "Total Screening during the month" Then
                Me.grdSubjectInfo.Columns(0).Visible = True
            ElseIf Convert.ToString(gvwpnlScreeningAnalytic.Rows(I).Cells(0).Text) = "Rejected" Then
                Me.grdSubjectInfo.Columns(0).Visible = True
            ElseIf Convert.ToString(gvwpnlScreeningAnalytic.Rows(I).Cells(0).Text) = "Successful Screening(In a month)" Then
                Me.grdSubjectInfo.Columns(0).Visible = True
            ElseIf Convert.ToString(gvwpnlScreeningAnalytic.Rows(I).Cells(0).Text) = "Total Successful Screening" Then
                Me.grdSubjectInfo.Columns(0).Visible = True
            ElseIf Convert.ToString(gvwpnlScreeningAnalytic.Rows(I).Cells(0).Text) = "Lapse during month" Then
                Me.grdSubjectInfo.Columns(0).Visible = True
            ElseIf Convert.ToString(gvwpnlScreeningAnalytic.Rows(I).Cells(0).Text) = "Eligibility not declared" Then
                Me.grdSubjectInfo.Columns(0).Visible = True
            ElseIf Convert.ToString(gvwpnlScreeningAnalytic.Rows(I).Cells(0).Text) = "Previous month eligibility not declared,Current month rejected" Then
                Me.grdSubjectInfo.Columns(0).Visible = True
            Else
                Me.grdSubjectInfo.Columns(0).Visible = False
            End If

            If ds.Tables(0).Rows.Count > 0 Then

                grdSubjectInfo.DataSource = ds
                grdSubjectInfo.DataBind()
                grdSubjectInfo.FooterRow.Visible = False
                mdpSubjectInfo.Show()

                If grdSubjectInfo.Rows.Count > 0 Or Not grdSubjectInfo.DataSource Is Nothing Then
                    grdSubjectInfo.UseAccessibleHeader = True
                    grdSubjectInfo.HeaderRow.TableSection = TableRowSection.TableHeader
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gvtodt_grdSubjectInfo", "gvtodt(ctl00_CPHLAMBDA_grdSubjectInfo); ", True)
                End If

            Else
                Me.objCommon.ShowAlert("There is no data found.", Me.Page)
                mdpSubjectInfo.Hide()
            End If

        Catch ex As Exception
            Me.objCommon.ShowAlert(ex.ToString(), Me.Page)
        End Try
    End Sub

    Protected Sub lnkProjectSpecifcScreening_click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim FirstDateOfCurrMonth As Date
        Dim CurrMonthLastDate As Integer
        Dim LastDateOfCurrMonth As Date
        Dim PrevMonthLastDate As Integer
        Dim FirstDateOfPrevMonth As Date
        Dim task As String
        Dim IndexNo As Integer
        Dim Dt As DataTable
        Dim LastDateOfPrevMonth As Date
        Dim LastMonth As String = String.Empty
        Dim LastYear As String = String.Empty
        Dim ds As DataSet = Nothing
        Dim Parameters As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim HeaderText As String = String.Empty
        Dim gvRow As GridViewRow
        Dim I As Integer
        Dim type As String = "2"
        Try
            grdSubjectInfo.Dispose()
            FirstDateOfCurrMonth = CDate("01" & "-" & DdllistMonth.SelectedItem.ToString().Trim() & "-" & DdllistYear.SelectedItem.ToString())
            CurrMonthLastDate = CInt(DateTime.DaysInMonth(DdllistYear.SelectedItem.ToString(), CInt(DdllistMonth.SelectedValue)))
            LastDateOfCurrMonth = CDate(CurrMonthLastDate & "-" & DdllistMonth.SelectedItem.ToString().Trim() & "-" & DdllistYear.SelectedItem.ToString())
            LastMonth = MonthName(CInt(CDate(FirstDateOfCurrMonth).AddMonths(-1).Month))
            LastYear = CInt(CDate(FirstDateOfCurrMonth).AddMonths(-1).Year.ToString())
            FirstDateOfPrevMonth = CDate("01" & "-" & LastMonth.ToString.Trim() & "-" & LastYear.ToString().Trim())
            PrevMonthLastDate = CInt(DateTime.DaysInMonth(CInt(LastYear), CInt(CDate(FirstDateOfPrevMonth).Month)))
            LastDateOfPrevMonth = CDate(PrevMonthLastDate & "-" & LastMonth.ToString.Trim() & "-" & LastYear.ToString.Trim())

            gvRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            I = gvRow.RowIndex

            If I <= 9 Then
                task = Convert.ToString(gvwpnlScreeningAnalytic.Rows(I).Cells(0).Text)
                lbltask.Text = "Data For: " & task.ToString()
                IndexNo = I.ToString()

                Parameters = type + "##" + IndexNo.ToString + "##" + Format(FirstDateOfCurrMonth, "dd/MMM/yyyy") + "##" + Format(LastDateOfCurrMonth, "dd/MMM/yyyy") + "##" + Format(FirstDateOfPrevMonth, "dd/MMM/yyyy") + "##" + Format(LastDateOfPrevMonth, "dd/MMM/yyyy") + "##" + ddlLocation.SelectedValue()
            Else
                objCommon.ShowAlert("Data not found", Me.Page)
            End If

            If Not Me.objHelpDbTable.Proc_SubjectScreeningInfo(Parameters, ds, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            grdSubjectInfo.DataSource = Nothing
            grdSubjectInfo.DataBind()

            If grdSubjectInfo.Rows.Count > 0 Or Not grdSubjectInfo.DataSource Is Nothing Then
                grdSubjectInfo.UseAccessibleHeader = True
                grdSubjectInfo.HeaderRow.TableSection = TableRowSection.TableHeader
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gvtodt_grdSubjectInfo", "gvtodt(ctl00_CPHLAMBDA_grdSubjectInfo); ", True)
            End If

            If Convert.ToString(gvwpnlScreeningAnalytic.Rows(I).Cells(0).Text) = "Opening Balance" Then
                Me.grdSubjectInfo.Columns(0).Visible = True
            ElseIf Convert.ToString(gvwpnlScreeningAnalytic.Rows(I).Cells(0).Text) = "Closing Balance" Then
                Me.grdSubjectInfo.Columns(0).Visible = True
            ElseIf Convert.ToString(gvwpnlScreeningAnalytic.Rows(I).Cells(0).Text) = "Total Screening during the month" Then
                Me.grdSubjectInfo.Columns(0).Visible = True
            ElseIf Convert.ToString(gvwpnlScreeningAnalytic.Rows(I).Cells(0).Text) = "Rejected" Then
                Me.grdSubjectInfo.Columns(0).Visible = True
            ElseIf Convert.ToString(gvwpnlScreeningAnalytic.Rows(I).Cells(0).Text) = "Successful Screening(In a month)" Then
                Me.grdSubjectInfo.Columns(0).Visible = True
            ElseIf Convert.ToString(gvwpnlScreeningAnalytic.Rows(I).Cells(0).Text) = "Lapse during month" Then
                Me.grdSubjectInfo.Columns(0).Visible = True
            ElseIf Convert.ToString(gvwpnlScreeningAnalytic.Rows(I).Cells(0).Text) = "Fresh Dosing" Then
                Me.grdSubjectInfo.Columns(0).Visible = True
            ElseIf Convert.ToString(gvwpnlScreeningAnalytic.Rows(I).Cells(0).Text) = "Eligibility not declared" Then
                Me.grdSubjectInfo.Columns(0).Visible = True
            ElseIf Convert.ToString(gvwpnlScreeningAnalytic.Rows(I).Cells(0).Text) = "Previous month eligibility not declared,Current month rejected" Then
                Me.grdSubjectInfo.Columns(0).Visible = True
            Else
                Me.grdSubjectInfo.Columns(0).Visible = False
            End If

            If ds.Tables(0).Rows.Count > 0 Then
                Dt = ds.Tables(0)
                ViewState("VS_grvSubjectInfo") = Dt
                If ds.Tables(0).Rows.Count > 0 Then
                    grdSubjectInfo.DataSource = ds
                    grdSubjectInfo.DataBind()
                    grdSubjectInfo.FooterRow.Visible = False
                    mdpSubjectInfo.Show()

                    If grdSubjectInfo.Rows.Count > 0 Or Not grdSubjectInfo.DataSource Is Nothing Then
                        grdSubjectInfo.UseAccessibleHeader = True
                        grdSubjectInfo.HeaderRow.TableSection = TableRowSection.TableHeader
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gvtodt_grdSubjectInfo", "gvtodt(ctl00_CPHLAMBDA_grdSubjectInfo); ", True)
                    End If

                End If
            Else
                Me.objCommon.ShowAlert("There is no data found.", Me.Page)
                mdpSubjectInfo.Hide()
            End If
        Catch ex As Exception
            Me.objCommon.ShowAlert(ex.ToString(), Me.Page)
        End Try
    End Sub

#End Region

#Region "Fill Function"
    Function FillGrid() As Boolean

        Dim ds_getvisittracker As DataSet
        Dim parameter As String
        Dim Note As String = String.Empty

        parameter = HProjectIdForVisitTracker.Value + "##" + Session(S_UserID).ToString() + "##"
        Try
            ds_getvisittracker = objHelpDbTable.ProcedureExecute("dbo.GetVisitTracker", parameter.ToString)

            gvVisitTracker.DataSource() = ds_getvisittracker.Tables(0)
            gvVisitTracker.DataBind()

            ViewState("DS_DeviationReport") = ds_getvisittracker


            upGvDeviation.Update()

            If Not ds_getvisittracker Is Nothing Then
                If ds_getvisittracker.Tables(0).Rows.Count > 0 Then
                    ds_getvisittracker.Tables(0).DefaultView.RowFilter = "iVisitNo = Max(iVisitNo)"
                    HfVisitNo.Value = Convert.ToInt32(ds_getvisittracker.Tables(0).DefaultView(0)("iVisitNo").ToString) + 1
                    Note = ds_getvisittracker.Tables(0).Rows(0).Item("cDateType").ToString()
                Else

                End If
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divVisitTracker');", True)
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    Function FillGridForVisitScheduler() As Boolean
        Dim estr As String = String.Empty
        Dim ds_ScheduledActivity As DataSet
        Dim ds_ActualActivity As DataSet = Nothing
        Dim ds_DisSubject As DataSet = Nothing
        Dim wStr As String = String.Empty

        isDataEntered = True
        ds_ScheduledActivity = objHelpDbTable.ProcedureExecute("dbo.PROC_VisitScheduler", HProjectIdForVisitScheduler.Value)
        ds_ActualActivity = objHelpDbTable.ProcedureExecute("dbo.PROC_VisitActual", HProjectIdForVisitScheduler.Value)
        ViewState(VS_Actual) = ds_ActualActivity
        ViewState(VS_Scheduler) = ds_ScheduledActivity

        Try
            If Not ds_ScheduledActivity Is Nothing AndAlso ds_ScheduledActivity.Tables(0).Rows.Count < 0 Then
                ds_ScheduledActivity = objHelpDbTable.ProcedureExecute("dbo.Proc_GETScheduledActivity", HProjectIdForVisitScheduler.Value)
                isDataEntered = False
            Else
                If ds_ScheduledActivity.Tables(0).Rows.Count = 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "nodate", "alert('No data found');", True)
                End If
                ViewState(VS_Scheduler) = ds_ScheduledActivity
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "nodate", "alert('No data found');", True)
            gvScheduler.DataSource = Nothing
            gvScheduler.DataBind()
            'upScheduler.Update()
            Exit Function
        End Try

        If Not ds_ScheduledActivity Is Nothing Then
            gvScheduler.DataSource = Nothing
            gvScheduler.DataBind()
            'upScheduler.Update()
            gvScheduler.DataSource = ds_ScheduledActivity.Tables(0)
            gvScheduler.DataBind()
            'upScheduler.Update()
        End If
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ModalOpen", "ModalOpen('divVisitScheduler');", True)
    End Function

    Function displayNotice() As Boolean
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_Notice As New DataSet
        Dim index As Integer = 0
        Dim dCurrentDate As DateTime


        Try
            dCurrentDate = DateTime.Parse(Date.Today).ToString("dd-MMM-yy")


            wStr = DateTime.Parse(Date.Today).ToString("dd-MMM-yy") & "#" & DateTime.Parse(Date.Today).ToString("dd-MMM-yy") & "#"
            wStr &= Convert.ToString(Me.Session(S_UserType)).Trim() & "#" & Convert.ToString(Me.Session(S_UserID))


            If Not Me.objHelpDbTable.Proc_GetNotice(DateTime.Parse(Date.Today).ToString("dd-MMM-yy"), DateTime.Parse(Date.Today).ToString("dd-MMM-yy"), Convert.ToString(Me.Session(S_UserType)).Trim(), Convert.ToString(Me.Session(S_UserID)), ds_Notice, eStr) Then
                Throw New Exception("Error while getting data from proc_getprofilelist " + eStr.Trim)
            End If

            If Not ds_Notice.Tables(0).Rows.Count = 0 Then

                PlaceAttach.Controls.Clear()
                PlaceAttach.Controls.Add(New LiteralControl("<Table align=left border=""0"" style=""width:100%; display:block;"">"))
                PlaceAttach.Controls.Add(New LiteralControl("<tbody style=""width:100%; display:block;"">"))
                PlaceAttach.Controls.Add(New LiteralControl("<Tr align=left style=""width:100%; height:auto;display:block"">"))
                Dim lblNotice As New Label
                lblNotice.ID = "lblNotice"
                lblNotice.Font.Bold = True
                lblNotice.ForeColor = Drawing.Color.WhiteSmoke
                lblNotice.Font.Size = 12
                PlaceAttach.Controls.Add(lblNotice)
                PlaceAttach.Controls.Add(New LiteralControl("<hr>"))
                PlaceAttach.Controls.Add(New LiteralControl("</Td>"))
                PlaceAttach.Controls.Add(New LiteralControl("</Tr>"))

                For index = 0 To ds_Notice.Tables(0).Rows.Count - 1

                    If Convert.ToString(ds_Notice.Tables(0).Rows(index)("vDescription")).Trim <> "" Or
                                Convert.ToString(ds_Notice.Tables(0).Rows(index)("vAttachment")).Trim <> "" Then

                        PlaceAttach.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT style=""width:100%;display:block"">"))
                        PlaceAttach.Controls.Add(New LiteralControl("<Td white-space: nowrap; style=""width:100%;display:block"">"))


                        If Convert.ToString(ds_Notice.Tables(0).Rows(index)("vSubject")).Trim <> "" Then
                            Dim lbl As New Label
                            lbl.ID = "lbl1" + index.ToString()
                            lbl.Text = ds_Notice.Tables(0).Rows(index)("vSubject").ToString
                            lbl.Text = " " + lbl.Text.Trim()
                            lbl.CssClass = ""
                            lbl.ForeColor = Color.MediumBlue
                            lbl.Font.Bold = True
                            PlaceAttach.Controls.Add(lbl)
                        End If

                        PlaceAttach.Controls.Add(New LiteralControl("</Td>"))
                        PlaceAttach.Controls.Add(New LiteralControl("</Tr>"))

                        PlaceAttach.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT style=""width:100%;display:block"">"))
                        PlaceAttach.Controls.Add(New LiteralControl("<Td white-space: nowrap; style=""width:100%;display:block"">"))


                        If Convert.ToString(ds_Notice.Tables(0).Rows(index)("vDescription")).Trim <> "" Then
                            Dim lbl As New Label
                            lbl.ID = "lbl" + index.ToString()
                            lbl.Text = ds_Notice.Tables(0).Rows(index)("vDescription").ToString
                            lbl.Text = " " + lbl.Text.Trim()
                            lbl.CssClass = ""
                            PlaceAttach.Controls.Add(lbl)
                        End If

                        If Convert.ToString(ds_Notice.Tables(0).Rows(index)("vAttachment")).Trim <> "" Then
                            PlaceAttach.Controls.Add(New LiteralControl("</br>"))
                            Dim hlnk As New HyperLink
                            hlnk.ID = "hlnk" + index.ToString()
                            hlnk.Target = "_blank"
                            hlnk.CssClass = ""
                            hlnk.Text = " " + ds_Notice.Tables(0).Rows(index)("vAttachment").ToString.Substring(ds_Notice.Tables(0).Rows(index)("vAttachment").ToString.Trim().LastIndexOf("/") + 1)
                            hlnk.NavigateUrl = System.Configuration.ConfigurationManager.AppSettings("DefaultGateway") + "/InformationManagement/" + hlnk.Text.Trim() + ""
                            PlaceAttach.Controls.Add(hlnk)
                        End If
                        'If Convert.ToString(ds_Notice.Tables(0).Rows(index)("vLoginName")).Trim <> "" Then
                        '    PlaceAttach.Controls.Add(New LiteralControl("</br>"))
                        '    'Dim lblMadeBy As New Label
                        '    'lblMadeBy.ID = "lblCreatedBy" + index.ToString()
                        '    'lblMadeBy.Text = "By: "
                        '    'lblMadeBy.Font.Bold = True
                        '    'lblMadeBy.ForeColor = Drawing.Color.Blue
                        '    'lblMadeBy.Font.Size = 8
                        '    'PlaceAttach.Controls.Add(lblMadeBy)

                        '    'Dim lblCreatedBy As New Label
                        '    'lblCreatedBy.ID = "lbl" + "_" + index.ToString()
                        '    'Dim strCreatedBy As String = ds_Notice.Tables(0).Rows(index)("vLoginName").ToString() _
                        '    '                            + "(" + ds_Notice.Tables(0).Rows(index)("vFirstName").ToString() _
                        '    '                            + " " + ds_Notice.Tables(0).Rows(index)("vLastName").ToString() + ")"
                        '    'lblCreatedBy.Text = " " + strCreatedBy
                        '    'PlaceAttach.Controls.Add(lblCreatedBy)
                        'End If
                        PlaceAttach.Controls.Add(New LiteralControl("<hr>"))
                        PlaceAttach.Controls.Add(New LiteralControl("</Td>"))
                        PlaceAttach.Controls.Add(New LiteralControl("</Tr>"))
                    End If
                Next
                PlaceAttach.Controls.Add(New LiteralControl("</tbody>"))
                PlaceAttach.Controls.Add(New LiteralControl("</Table>"))
            End If


            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function
#End Region

#Region " Web Method" ''Added By Vivek PAtel

    ''Added By Vivek Patel For DashBoard

    <WebMethod>
    Public Shared Function Proc_WorkSpaceProjectTotalSubjectCount(ByVal vWorkSpaceId As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wstr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim Sql_DataSet As Data.DataSet = Nothing
        Dim eStr_Retu As String = String.Empty

        Try
            wstr = vWorkSpaceId.ToString()
            If Not objHelp.Proc_WorkSpaceProjectTotalSubjectCount(wstr, Sql_DataSet, eStr_Retu) Then

                Throw New Exception("Error while getting data from Proc_WorkSpaceProjectTotalSubjectCount " + eStr_Retu.Trim)
            End If

            strReturn = JsonConvert.SerializeObject(Sql_DataSet)
            Return strReturn
        Catch ex As Exception
            Return strReturn
        End Try
    End Function

    <WebMethod>
    Public Shared Function Proc_SiteWiseSubjectInformation(ByVal vWorkSpaceId As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wstr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim Sql_DataSet As Data.DataSet = Nothing
        Dim eStr_Retu As String = String.Empty

        Try
            wstr = vWorkSpaceId.ToString()
            If Not objHelp.Proc_SiteWiseSubjectInformation(wstr, Sql_DataSet, eStr_Retu) Then
                Throw New Exception("Error while getting data from Proc_SiteWiseSubjectInformation " + eStr_Retu.Trim)
            End If

            strReturn = JsonConvert.SerializeObject(Sql_DataSet)
            Return strReturn
        Catch ex As Exception
            Return strReturn
        End Try
    End Function

    <WebMethod>
    Public Shared Function Proc_WorkSpaceAllSubjectCount(ByVal vWorkSpaceId As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wstr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim Sql_DataSet As Data.DataSet = Nothing
        Dim eStr_Retu As String = String.Empty

        Try
            wstr = vWorkSpaceId.ToString()
            If Not objHelp.Proc_WorkSpaceAllSubjectCount(wstr, Sql_DataSet, eStr_Retu) Then

                Throw New Exception("Error while getting data from Proc_WorkSpaceAllSubjectCount " + eStr_Retu.Trim)
            End If

            strReturn = JsonConvert.SerializeObject(Sql_DataSet)
            Return strReturn
        Catch ex As Exception
            Return strReturn
        End Try
    End Function

    <WebMethod>
    Public Shared Function Proc_WorkSpaceDeActivatedSubjectCount(ByVal vWorkSpaceId As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wstr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Sql_DataSet As Data.DataSet = Nothing
        Dim strReturn As String = String.Empty

        Try
            wstr = vWorkSpaceId.ToString()
            If Not objHelp.Proc_WorkSpaceDeActivatedSubjectCount(wstr, Sql_DataSet, eStr_Retu) Then

                Throw New Exception("Error while getting data from Proc_WorkSpaceDeActivatedSubjectCount " + eStr_Retu.Trim)
            End If

            strReturn = JsonConvert.SerializeObject(Sql_DataSet)
            Return strReturn
        Catch ex As Exception
            Return strReturn
        End Try
    End Function

    <WebMethod>
    Public Shared Function Proc_StudyDetail(ByVal vWorkSpaceId As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wstr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Sql_DataSet As Data.DataSet = Nothing
        Dim strReturn As String = String.Empty

        Try
            wstr = vWorkSpaceId.ToString()

            If Not objHelp.Proc_StudyDetail(wstr, Sql_DataSet, eStr_Retu) Then

                Throw New Exception("Error while getting data from Proc_StudyDetail " + eStr_Retu.Trim)
            End If

            strReturn = JsonConvert.SerializeObject(Sql_DataSet)
            Return strReturn
        Catch ex As Exception
            Return strReturn
        End Try

    End Function

    <WebMethod>
    Public Shared Function Proc_ProjectStudyDetail(ByVal vWorkspaceId As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wstr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Sql_DataSet As Data.DataSet = Nothing
        Dim strReturn As String = String.Empty

        Try
            wstr = vWorkspaceId.ToString()

            If Not objHelp.Proc_ProjectStudyDetail(wstr, Sql_DataSet, eStr_Retu) Then

                Throw New Exception("Error while getting data from Proc_ProjectStudyDetail " + eStr_Retu.Trim)
            End If

            strReturn = JsonConvert.SerializeObject(Sql_DataSet)
            Return strReturn
        Catch ex As Exception
            Return strReturn
        End Try

    End Function

    <WebMethod>
    Public Shared Function Proc_ProjectStudyDetailExportToExcel(ByVal vWorkspaceId As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wstr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Sql_DataSet As Data.DataSet = Nothing
        Dim strReturn As String = String.Empty

        Try
            wstr = vWorkspaceId.ToString()

            If Not objHelp.Proc_ProjectStudyDetail(wstr, Sql_DataSet, eStr_Retu) Then

                Throw New Exception("Error while getting data from Proc_ProjectStudyDetail " + eStr_Retu.Trim)
            End If

            strReturn = JsonConvert.SerializeObject(Sql_DataSet)
            Return strReturn
        Catch ex As Exception
            Return strReturn
        End Try

    End Function

    <WebMethod>
    Public Shared Function Proc_SiteInformation(ByVal vWorkSpaceId As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wstr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Sql_DataSet As Data.DataSet = Nothing
        Dim strReturn As String = String.Empty

        Try
            wstr = vWorkSpaceId.ToString()

            If Not objHelp.Proc_SiteInformation(wstr, Sql_DataSet, eStr_Retu) Then

                Throw New Exception("Error while getting data from Proc_SiteInformation " + eStr_Retu.Trim)
            End If

            strReturn = JsonConvert.SerializeObject(Sql_DataSet)
            Return strReturn
        Catch ex As Exception
            Return strReturn
        End Try

    End Function

    <WebMethod>
    Public Shared Function Proc_SubSiteInformation(ByVal vWorkSpaceId As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wstr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Sql_DataSet As Data.DataSet = Nothing
        Dim strReturn As String = String.Empty

        Try
            wstr = vWorkSpaceId.ToString()

            If Not objHelp.Proc_SubSiteInformation(wstr, Sql_DataSet, eStr_Retu) Then

                Throw New Exception("Error while getting data from Proc_SubSiteInformation " + eStr_Retu.Trim)
            End If

            strReturn = JsonConvert.SerializeObject(Sql_DataSet)
            Return strReturn
        Catch ex As Exception
            Return strReturn
        End Try

    End Function

    Private Sub Proc_GetParentProjectList()

        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds As New DataSet

        Try
            wStr = "cWorkspaceType = 'P'"

            If Not objHelp.GetData("View_getWorkspaceDetailForHdr", "vWorkspaceId,DisplayProject", wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If


            If ds.Tables(0).Rows.Count > 0 Then
                dd_project.DataSource = ds.Tables(0)
                dd_project.DataValueField = "vWorkspaceId"
                dd_project.DataTextField = "DisplayProject"
                dd_project.DataBind()
                dd_project.Items.Insert(0, New System.Web.UI.WebControls.ListItem(" --- Select Project ---", "0"))
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillDropDown")
        End Try

    End Sub

    <WebMethod>
    Public Shared Function Proc_GetProjectNo() As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wstr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Sql_DataSet As Data.DataSet = Nothing
        Dim strReturn As String = String.Empty

        Try
            wstr = "cWorkspaceType = 'P'"

            If Not objHelp.GetData("View_getWorkspaceDetailForHdr", "vWorkspaceId,vProjectNo,DisplayProject", wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Sql_DataSet, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            strReturn = JsonConvert.SerializeObject(Sql_DataSet)
            Return strReturn
        Catch ex As Exception
            Return strReturn
        End Try

    End Function

    <WebMethod>
    Public Shared Function Proc_GetActivityStatusCountRecords(ByVal vWorkSpaceId As String, ByVal vBoolId As Integer, ByVal isChild As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wstr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Sql_DataSet As Data.DataSet = Nothing
        Dim strReturn As String = String.Empty
        Dim vProjectWorkSpaceId As String = String.Empty

        Try
            vProjectWorkSpaceId = vWorkSpaceId.ToString()

            If Not objHelp.Proc_GetActivityStatusCountRecords(vProjectWorkSpaceId, "", isChild, 2, "N", Sql_DataSet, eStr_Retu) Then
                Throw New Exception("Error while getting data from Proc_GetActivityStatusCountRecords " + eStr_Retu.Trim)
                Exit Function
            End If
            If (vBoolId = 999) Then
                Sql_DataSet.Tables(0).Columns.Add("Total Expected Data", GetType(Integer), "[Data Entry Pending]+[Data Entry Continue]+[Ready For Review]+[First Review Done]+[Second Review Done]")
                Sql_DataSet.Tables(0).Columns.Add("Available Data", GetType(Integer), " [Data Entry Continue]+[Ready For Review]+[First Review Done]+[Second Review Done]")
                Sql_DataSet.Tables(0).Columns.Add("SDV Data", GetType(Integer), "[First Review Done]+[Second Review Done]")   '' AS discessed with Lambda Team Remove Ready For Review Sum
                Sql_DataSet.Tables(0).Columns.Add("DM Reviwed", GetType(Integer), "[Second Review Done]")
            End If

            strReturn = JsonConvert.SerializeObject(Sql_DataSet)

            Return strReturn
        Catch ex As Exception
            Return strReturn
        End Try

    End Function

    <WebMethod>
    Public Shared Function Proc_GetWorkSpaceProjectAESAE(ByVal vWorkSpaceId As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wstr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim Sql_DataSet As Data.DataSet = Nothing
        Dim eStr_Retu As String = String.Empty

        Try
            wstr = vWorkSpaceId.ToString()
            If Not objHelp.Proc_GetWorkSpaceProjectAESAE(wstr, Sql_DataSet, eStr_Retu) Then
                Throw New Exception("Error while getting data from Proc_GetWorkSpaceProjectAESAE " + eStr_Retu.Trim)
            End If

            strReturn = JsonConvert.SerializeObject(Sql_DataSet)
            Return strReturn
        Catch ex As Exception
            Return strReturn
        End Try
    End Function

    ''Ended By Vivek Patel



#End Region

    ''Added by Dhruvi Shah
    <WebMethod>
    Public Shared Function Proc_DCFTrackingReport(ByVal vWorkSpaceId As String, ByVal vType As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wstr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim Sql_DataSet As Data.DataSet = Nothing
        Dim eStr_Retu As String = String.Empty

        Try
            wstr = vWorkSpaceId.ToString()
            If Not objHelp.Proc_DCFTrackingReport(wstr, vType, Sql_DataSet, eStr_Retu) Then

                Throw New Exception("Error while getting data from Proc_DCFTrackingReport " + eStr_Retu.Trim)
            End If

            strReturn = JsonConvert.SerializeObject(Sql_DataSet)
            Return strReturn
        Catch ex As Exception
            Return strReturn
        End Try
    End Function
    ''End By Dhruvi Shah

    <WebMethod>
    Public Shared Function PanelRights() As String
        Dim ObjCommon As New clsCommon
        Dim objHelpDbTable As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wstr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Sql_DataSet As Data.DataSet = Nothing
        Dim strReturn As String = String.Empty
        Dim dsDisplayDetails As DataSet = Nothing

        Try
            If Not objHelpDbTable.GetPanelDisplayDetailByUserType(" vUserTypeCode = " & System.Web.HttpContext.Current.Session(S_UserType) + " AND cActiveFlag ='Y' ",
                                                         WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                                         dsDisplayDetails, eStr_Retu) Then
                Throw New Exception("Error while getting data from Proc_DCFTrackingReport " + eStr_Retu.Trim)
            End If

            strReturn = JsonConvert.SerializeObject(dsDisplayDetails)
            Return strReturn
        Catch ex As Exception
            Return strReturn
        End Try

    End Function

#Region "PreRender Event" ''Added By Vivek Patel

    ''PreRender Event Added By Vivek Patel

    Protected Sub gvwpnlScreeningAnalytic_PreRender(sender As Object, e As EventArgs) Handles gvwpnlScreeningAnalytic.PreRender
        If gvwpnlScreeningAnalytic.Rows.Count > 0 Or Not gvwpnlScreeningAnalytic.DataSource Is Nothing Then
            gvwpnlScreeningAnalytic.UseAccessibleHeader = True
            gvwpnlScreeningAnalytic.HeaderRow.TableSection = TableRowSection.TableHeader
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gvtodt_gvwpnlScreeningAnalytic", "gvtodt(ctl00_CPHLAMBDA_gvwpnlScreeningAnalytic); ", True)
        End If
    End Sub

    Protected Sub grdSubjectInfo_PreRender(sender As Object, e As EventArgs) Handles grdSubjectInfo.PreRender
        If grdSubjectInfo.Rows.Count > 0 Or Not grdSubjectInfo.DataSource Is Nothing Then
            grdSubjectInfo.UseAccessibleHeader = True
            grdSubjectInfo.HeaderRow.TableSection = TableRowSection.TableHeader
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gvtodt_grdSubjectInfo", "gvtodt(ctl00_CPHLAMBDA_grdSubjectInfo); ", True)
        End If
    End Sub

    Protected Sub PnlTblForOperationalKpi_PreRender(sender As Object, e As EventArgs) Handles PnlTblForOperationalKpi.PreRender
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gvtodt_PnlTblForOperationalKpi", "gvtodt(tblOperationalKpi); ", True)
    End Sub

    Protected Sub gvVisitTracker_PreRender(sender As Object, e As EventArgs) Handles gvVisitTracker.PreRender
        If gvVisitTracker.Rows.Count > 0 Or Not gvVisitTracker.DataSource Is Nothing Then
            gvVisitTracker.UseAccessibleHeader = True
            gvVisitTracker.HeaderRow.TableSection = TableRowSection.TableHeader
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gvtodt_gvVisitTracker", "gvtodt(ctl00_CPHLAMBDA_gvVisitTracker); ", True)
        End If
    End Sub

    Protected Sub GvwPnlPnlActivityDetails_PreRender(sender As Object, e As EventArgs) Handles GvwPnlPnlActivityDetails.PreRender
        If GvwPnlPnlActivityDetails.Rows.Count > 0 Or Not GvwPnlPnlActivityDetails.DataSource Is Nothing Then
            GvwPnlPnlActivityDetails.UseAccessibleHeader = True
            GvwPnlPnlActivityDetails.HeaderRow.TableSection = TableRowSection.TableHeader
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gvtodt_GvwPnlPnlActivityDetails", "gvtodt(ctl00_CPHLAMBDA_GvwPnlPnlActivityDetails); ", True)
        End If
    End Sub

    Protected Sub gvwpnl0001_PreRender(sender As Object, e As EventArgs) Handles gvwpnl0001.PreRender
        If gvwpnl0001.Rows.Count > 0 Or Not gvwpnl0001.DataSource Is Nothing Then
            gvwpnl0001.UseAccessibleHeader = True
            gvwpnl0001.HeaderRow.TableSection = TableRowSection.TableHeader
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gvtodt_gvwpnl0001", "gvtodt(ctl00_CPHLAMBDA_gvwpnl0001); ", True)
        End If
    End Sub

    Protected Sub gvwpnl0013_PreRender(sender As Object, e As EventArgs) Handles gvwpnl0013.PreRender
        If gvwpnl0013.Rows.Count > 0 Or Not gvwpnl0013.DataSource Is Nothing Then
            gvwpnl0013.UseAccessibleHeader = True
            gvwpnl0013.HeaderRow.TableSection = TableRowSection.TableHeader
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gvtodt_gvwpnl0013", "gvtodt(ctl00_CPHLAMBDA_gvwpnl0013); ", True)
        End If
    End Sub

    Protected Sub gvwpnl0003_PreRender(sender As Object, e As EventArgs) Handles gvwpnl0003.PreRender
        If gvwpnl0003.Rows.Count > 0 Or Not gvwpnl0003.DataSource Is Nothing Then
            gvwpnl0003.UseAccessibleHeader = True
            If gvwpnl0003.Rows.Count > 0 Or Not gvwpnl0003.DataSource Is Nothing Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gvtodt_gvwpnl0003", "gvtodt(ctl00_CPHLAMBDA_gvwpnl0003); ", True)
            End If
        End If
    End Sub

    Protected Sub gvwpnl_Analysis_PreRender(sender As Object, e As EventArgs) Handles gvwpnl_Analysis.PreRender
        If gvwpnl_Analysis.Rows.Count > 0 Or Not gvwpnl_Analysis.DataSource Is Nothing Then
            gvwpnl_Analysis.UseAccessibleHeader = True
            gvwpnl_Analysis.HeaderRow.TableSection = TableRowSection.TableHeader
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gvtodt_gvwpnl_Analysis", "gvtodt(ctl00_CPHLAMBDA_gvwpnl_Analysis); ", True)
        End If
    End Sub

    Protected Sub gvwpnl0004_PreRender(sender As Object, e As EventArgs) Handles gvwpnl0004.PreRender
        If gvwpnl0004.Rows.Count > 0 Or Not gvwpnl0004.DataSource Is Nothing Then
            gvwpnl0004.UseAccessibleHeader = True
            gvwpnl0004.HeaderRow.TableSection = TableRowSection.TableHeader
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gvtodt_gvwpnl0004", "gvtodt(ctl00_CPHLAMBDA_gvwpnl0004); ", True)
        End If
    End Sub

    Protected Sub GvwPnlProjectStudyWorkSummary_PreRender(sender As Object, e As EventArgs) Handles GvwPnlProjectStudyWorkSummary.PreRender
        If GvwPnlProjectStudyWorkSummary.Rows.Count > 0 Or Not GvwPnlProjectStudyWorkSummary.DataSource Is Nothing Then
            GvwPnlProjectStudyWorkSummary.UseAccessibleHeader = True
            GvwPnlProjectStudyWorkSummary.HeaderRow.TableSection = TableRowSection.TableHeader
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gvtodt_GvwPnlProjectStudyWorkSummary", "gvtodt(ctl00_CPHLAMBDA_GvwPnlProjectStudyWorkSummary); ", True)
        End If
    End Sub

    Protected Sub GrdvgiewOfOperationalKpi_PreRender(sender As Object, e As EventArgs) Handles GrdvgiewOfOperationalKpi.PreRender
        If GrdvgiewOfOperationalKpi.Rows.Count > 0 Or Not GrdvgiewOfOperationalKpi.DataSource Is Nothing Then
            GrdvgiewOfOperationalKpi.UseAccessibleHeader = True
            GrdvgiewOfOperationalKpi.HeaderRow.TableSection = TableRowSection.TableHeader
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gvtodt_GrdvgiewOfOperationalKpi", "gvtodt(ctl00_CPHLAMBDA_GrdvgiewOfOperationalKpi); ", True)
        End If
    End Sub

    Protected Sub gvScheduler_PreRender(sender As Object, e As EventArgs) Handles gvScheduler.PreRender
        If gvScheduler.Rows.Count > 0 Or Not gvScheduler.DataSource Is Nothing Then
            gvScheduler.UseAccessibleHeader = True
            gvScheduler.HeaderRow.TableSection = TableRowSection.TableHeader
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gvtodt_gvScheduler", "gvtodt(ctl00_CPHLAMBDA_gvScheduler); ", True)
        End If
    End Sub

    ''Ended By Vivek Patel

#End Region

#Region "DiSoft"
    '<WebMethod>
    'Public Shared Function SiteWiseSubjectInformation() As String

    '    Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
    '    Dim wstr As String = String.Empty
    '    Dim strReturn As String = String.Empty
    '    Dim Sql_DataSet As Data.DataSet = Nothing
    '    Dim eStr_Retu As String = String.Empty

    '    Try
    '        wstr = "1=1"
    '        If Not objHelp.GetViewSiteWiseSubjectInformation(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Sql_DataSet, eStr_Retu) Then

    '            Throw New Exception("Error while getting data from View_getSiteWiseSubjectInformation " + eStr_Retu.Trim)
    '        End If
    '        Sql_DataSet.Tables(0).TableName = "Table"
    '        strReturn = JsonConvert.SerializeObject(Sql_DataSet)
    '        Return strReturn
    '    Catch ex As Exception
    '        Return strReturn
    '    End Try
    'End Function

    <WebMethod>
    Public Shared Function SiteWiseCAInformation(ByVal vWorkSpaceId As String, ByVal iNodeId As String, ByVal MODE As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim Parameters As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim Sql_DataSet As Data.DataSet = Nothing
        Dim eStr_Retu As String = String.Empty

        Try
            Parameters = vWorkSpaceId.ToString().Trim() + "##" + iNodeId.ToString().Trim() + "##" + MODE
            If Not objHelp.Proc_SiteWiseSubjectInformationForDashboard(Parameters, Sql_DataSet, eStr_Retu) Then

                Throw New Exception("Error while getting data from Proc_SiteWiseSubjectInformationForDashboard " + eStr_Retu.Trim)
            End If
            Sql_DataSet.Tables(0).TableName = "Table"
            strReturn = JsonConvert.SerializeObject(Sql_DataSet)
            Return strReturn
        Catch ex As Exception
            Return strReturn
        End Try
    End Function
#End Region
    '"Added By rinkal"
    Protected Sub gvVisitReviewStatus_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        'Dim Button As LinkButton = CType(sender, LinkButton)

        ' Dim rowIndex As Integer = sender.Attributes("RowIndex")

        Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
        Dim row As GridViewRow = gvVisitReviewStatus.Rows(rowIndex)

        'Dim rowIndex As Integer = Button.CommandArgument
        'Dim row As GridViewRow = gvVisitReviewStatus.Rows(rowIndex)
        Dim WorkSpaceId As String = gvVisitReviewStatus.DataKeys(rowIndex).Values(0).ToString()
        Dim ActivityId As String = gvVisitReviewStatus.DataKeys(rowIndex).Values(1).ToString()
        Dim NodeId As String = gvVisitReviewStatus.DataKeys(rowIndex).Values(2).ToString()
        Dim PeriodId As String = gvVisitReviewStatus.DataKeys(rowIndex).Values(3).ToString()
        Dim SubjectId As String = gvVisitReviewStatus.DataKeys(rowIndex).Values(4).ToString()
        Dim MySubjectNo As String = gvVisitReviewStatus.DataKeys(rowIndex).Values(5).ToString()
        Dim ScreeningNo As String = gvVisitReviewStatus.DataKeys(rowIndex).Values(6).ToString()
        Dim QC1 As String = gvVisitReviewStatus.DataKeys(rowIndex).Values(7).ToString()
        Dim Grader As String = gvVisitReviewStatus.DataKeys(rowIndex).Values(8).ToString()
        Dim CA As String = gvVisitReviewStatus.DataKeys(rowIndex).Values(10).ToString()
        Dim QC2 As String = gvVisitReviewStatus.DataKeys(rowIndex).Values(11).ToString()
        Dim iImgTransmittalDtlId As String = gvVisitReviewStatus.DataKeys(rowIndex).Values(9).ToString()
        'Dim QCId As String = gvVisitReviewStatus.DataKeys(rowIndex).Values(9).ToString()
        'Dim GraderId As String = gvVisitReviewStatus.DataKeys(rowIndex).Values(10).ToString()
        Dim RedirectStr As String = String.Empty
        Dim querystring As String = String.Empty
        Dim QStr As String = String.Empty
        Dim Mode As String = "4"
        Dim result As Boolean = False
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim eStr As String = String.Empty
        Dim ds_Subject As New DataSet
        Dim extractPath As String = String.Empty
        Dim DicomPath As String = String.Empty, ResponseString As String = String.Empty
        Dim originalFiles As String()
        Dim request As WebRequest
        Dim FileCount As String = String.Empty
        Dim IsStatus As Boolean = False
        Dim url As String = ""
        Dim wstr As String = String.Empty
        Dim Project As String = String.Empty
        Dim dsDicom As New DataSet
        Dim iModalityNo As String = String.Empty
        Dim iAnatomyNo As String = String.Empty
        Dim iImgTransmittalHdrId As String = String.Empty
        Dim iImageStatus As String = String.Empty
        Dim ActivityName As String = String.Empty
        Dim RDicom As String = String.Empty

        If Convert.ToString(ConfigurationManager.AppSettings("QCUserCode")).Contains(Me.Session(S_UserType)) AndAlso e.CommandName = "QC1" Then
            Mode = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
        ElseIf Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(Me.Session(S_UserType)) AndAlso e.CommandName = "CA1" Then
            Mode = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
        ElseIf (Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(Me.Session(S_UserType)) AndAlso e.CommandName = "QC1") OrElse (Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(Me.Session(S_UserType)) AndAlso e.CommandName = "QC2") Then
            Mode = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View
        ElseIf Convert.ToString(ConfigurationManager.AppSettings("QC2UserCode")).Contains(Me.Session(S_UserType)) AndAlso e.CommandName = "QC2" Then
            Mode = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
        Else
            Mode = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View
        End If


        GetUserProfile(Me.Session(S_UserName), e.CommandName)

        If e.CommandName = "QC1" Then
            querystring = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" & WorkSpaceId &
                        "&ActivityId=" & ActivityId & "&NodeId=" & NodeId &
                        "&PeriodId=" & PeriodId & "&SubjectId=" & SubjectId &
                        "&MySubjectNo=" & MySubjectNo + "&ScreenNo=" & ScreeningNo + "&mode=" & Mode + "&UserTypeCode=" + QC1 + "&UserId=" + UserId

            RedirectStr = "window.open(""" + querystring + """)"
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WindowOpen", RedirectStr, True)

        ElseIf e.CommandName = "CA1" Then
            querystring = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" & WorkSpaceId &
                        "&ActivityId=" & ActivityId & "&NodeId=" & NodeId &
                        "&PeriodId=" & PeriodId & "&SubjectId=" & SubjectId &
                        "&MySubjectNo=" & MySubjectNo + "&ScreenNo=" & ScreeningNo + "&mode=" & Mode + "&UserTypeCode=" + CA + "&UserId=" + UserId

            RedirectStr = "window.open(""" + querystring + """)"
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WindowOpen", RedirectStr, True)

        ElseIf e.CommandName = "QC2" Then
            querystring = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" & WorkSpaceId &
                        "&ActivityId=" & ActivityId & "&NodeId=" & NodeId &
                        "&PeriodId=" & PeriodId & "&SubjectId=" & SubjectId &
                        "&MySubjectNo=" & MySubjectNo + "&ScreenNo=" & ScreeningNo + "&mode=" & Mode + "&UserTypeCode=" + QC2 + "&UserId=" + UserId

            RedirectStr = "window.open(""" + querystring + """)"
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WindowOpen", RedirectStr, True)

        ElseIf e.CommandName = "ViewR1" OrElse e.CommandName = "ViewR2" Then
            DISoftURL.Value = Convert.ToString(ConfigurationManager.AppSettings.Item("DISoftURL").Trim())
            wstr = "vWorkSpaceId = '" + WorkSpaceId + "' and iNodeId = '" + NodeId + "' and vSubjectId ='" + SubjectId + "'"

            If Not objHelp.GetData("View_GetSubjectStudyDetail", "*", wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsDicom, eStr_Retu) Then
                Throw New Exception("Error while getting status of Dicom..." + eStr_Retu)
            End If
            If dsDicom.Tables(0).Rows.Count > 0 Then
                Project = dsDicom.Tables(0).Rows(0)("vProjectNo").ToString()
                iModalityNo = dsDicom.Tables(0).Rows(0)("iModalityNo").ToString()
                iAnatomyNo = dsDicom.Tables(0).Rows(0)("iAnatomyNo").ToString()
                iImgTransmittalHdrId = dsDicom.Tables(0).Rows(0)("iImgTransmittalHdrId").ToString()
                iImageStatus = dsDicom.Tables(0).Rows(0)("iImageStatus").ToString()
                ActivityName = dsDicom.Tables(0).Rows(0)("vNodeDisplayName").ToString()
            End If

            If e.CommandName = "ViewR1" Then
                RDicom = "R1"
            ElseIf e.CommandName = "ViewR2" Then
                RDicom = "R2"
            End If

            querystring = DISoftURL.Value + "MIBizNETAdjudicatorResponse/MIBizNETAdjudicatorResponse?WId=" + WorkSpaceId + "&SId=" + SubjectId + "&PId=" + Project +
                          "&Uid=" + UserId + "&MId=" + iModalityNo + "&AId=" + iAnatomyNo + "&VId=" + NodeId + "&HdrId=" + iImgTransmittalHdrId + "&DtlId=" + iImgTransmittalDtlId +
                          "&iIS=" + iImageStatus + "&iMySubjectNo=" + MySubjectNo + "&ScreenNo=" + ScreeningNo + "&PeriodId=" + PeriodId + "&ActivityName=" + ActivityName +
                          "&parentActivityID=" + RDicom + "&AdjUserId=" + Me.Session(S_UserID) + "&UserTypeCode=" + Me.Session(S_UserType) + "&WorkFlowStageId=" + Session(S_WorkFlowStageId) +
                          "&RDicom=" + RDicom

            RedirectStr = "window.open(""" + querystring + """)"
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WindowOpen", RedirectStr, True)


        ElseIf e.CommandName = "DCM" Then

            QStr = WorkSpaceId.Trim.ToString() + "##" + SubjectId.Trim.ToString()
            ds_Subject = objHelp.ProcedureExecute("dbo.GetImageServerPath", QStr)

            If ds_Subject.Tables(0).Rows.Count > 0 Then
                extractPath = Path.GetDirectoryName(ds_Subject.Tables(0).Rows(0)("vServerPath"))
                DicomPath = ConfigurationManager.AppSettings("vUploadSourcePath").ToString() + extractPath

                url = ConfigurationManager.AppSettings("DISoftAPIURL") + "SetData/DownLoadFiles?iImgTransmittalDtlId=" + iImgTransmittalDtlId.ToString


            End If

            If url <> "" Then
                'Api Call the ImageTransfer
                Dim t As Task = Task.Run(Function()
                                             request = WebRequest.Create(url)
                                             request.Timeout = -1

                                             Using response As WebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
                                                 Dim reader As New StreamReader(response.GetResponseStream())
                                                 ResponseString = reader.ReadToEnd()
                                                 Dim Data As String = ResponseString.Replace("\", "")
                                                 Dim Data1 As String = Data.TrimStart("""")
                                                 Dim Data2 As String = Data1.TrimEnd("""")
                                                 Dim dt_res As New DataTable
                                                 dt_res = JsonConvert.DeserializeObject(Of DataTable)("[" + Data2 + "]") 'JsonConvert.des(ResponseString.Replace("\", ""))

                                                 If Not dt_res Is Nothing Then
                                                     If dt_res.Columns.Contains("Status") Then
                                                         If dt_res.Rows(0)("Status").ToString.ToUpper() = "1" Then
                                                             IsStatus = True
                                                             FileCount = dt_res.Rows(0)("Message").ToString
                                                             RedirectStr = "window.open(""" + FileCount + """)"
                                                         Else
                                                             IsStatus = False
                                                             Me.objCommon.ShowAlert(dt_res.Rows(0)("Message").ToString, Me.Page)
                                                             Return False
                                                             'Exit Sub
                                                         End If
                                                     End If
                                                 End If
                                                 Return True
                                             End Using
                                         End Function)
                t.Wait()
                If IsStatus Then
                    objCommon.ShowAlert("All Images have been Transferred successfully on below path: " + extractPath, Me.Page)
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WindowOpen", RedirectStr, True)

                Else
                    objCommon.ShowAlert("No Image found or Images not transferred successfully", Me.Page)
                    Exit Sub
                End If
            End If

        ElseIf e.CommandName = "FA" Then

            QStr = WorkSpaceId.Trim.ToString() + "##" + SubjectId.Trim.ToString() + "##" + e.CommandName.Trim.ToString()
            ds_Subject = objHelp.ProcedureExecute("dbo.GetImageServerPath", QStr)

            If ds_Subject.Tables(0).Rows.Count > 0 Then
                extractPath = Path.GetDirectoryName(ds_Subject.Tables(0).Rows(0)("vServerPath"))
                DicomPath = ConfigurationManager.AppSettings("vUploadSourcePath").ToString() + extractPath

                url = ConfigurationManager.AppSettings("DISoftAPIURL") + "SetData/DownLoadFiles?iImgTransmittalDtlId=" + iImgTransmittalDtlId.ToString


            End If

            If url <> "" Then
                'Api Call the ImageTransfer
                Dim t As Task = Task.Run(Function()
                                             request = WebRequest.Create(url)
                                             request.Timeout = -1

                                             Using response As WebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
                                                 Dim reader As New StreamReader(response.GetResponseStream())
                                                 ResponseString = reader.ReadToEnd()
                                                 Dim Data As String = ResponseString.Replace("\", "")
                                                 Dim Data1 As String = Data.TrimStart("""")
                                                 Dim Data2 As String = Data1.TrimEnd("""")
                                                 Dim dt_res As New DataTable
                                                 dt_res = JsonConvert.DeserializeObject(Of DataTable)("[" + Data2 + "]") 'JsonConvert.des(ResponseString.Replace("\", ""))

                                                 If Not dt_res Is Nothing Then
                                                     If dt_res.Columns.Contains("Status") Then
                                                         If dt_res.Rows(0)("Status").ToString.ToUpper() = "1" Then
                                                             IsStatus = True
                                                             FileCount = dt_res.Rows(0)("Message").ToString
                                                             RedirectStr = "window.open(""" + FileCount + """)"
                                                         Else
                                                             IsStatus = False
                                                             Me.objCommon.ShowAlert(dt_res.Rows(0)("Message").ToString, Me.Page)
                                                             Return False
                                                             'Exit Sub
                                                         End If
                                                     End If
                                                 End If
                                                 Return True
                                             End Using
                                         End Function)
                t.Wait()
                If IsStatus Then
                    objCommon.ShowAlert("All Images have been Transferred successfully on below path: ", Me.Page)
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WindowOpen", RedirectStr, True)

                Else
                    objCommon.ShowAlert("No Image found or Images not transferred successfully", Me.Page)
                    Exit Sub
                End If
            End If

        ElseIf e.CommandName = "OCT" Then

            QStr = WorkSpaceId.Trim.ToString() + "##" + SubjectId.Trim.ToString() + "##" + e.CommandName.Trim.ToString()
            ds_Subject = objHelp.ProcedureExecute("dbo.GetImageServerPath", QStr)

            If ds_Subject.Tables(0).Rows.Count > 0 Then
                extractPath = Path.GetDirectoryName(ds_Subject.Tables(0).Rows(0)("vServerPath"))
                DicomPath = ConfigurationManager.AppSettings("vUploadSourcePath").ToString() + extractPath

                url = ConfigurationManager.AppSettings("DISoftAPIURL") + "SetData/DownLoadFiles?iImgTransmittalDtlId=" + iImgTransmittalDtlId.ToString


            End If

            If url <> "" Then
                'Api Call the ImageTransfer
                Dim t As Task = Task.Run(Function()
                                             request = WebRequest.Create(url)
                                             request.Timeout = -1

                                             Using response As WebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
                                                 Dim reader As New StreamReader(response.GetResponseStream())
                                                 ResponseString = reader.ReadToEnd()
                                                 Dim Data As String = ResponseString.Replace("\", "")
                                                 Dim Data1 As String = Data.TrimStart("""")
                                                 Dim Data2 As String = Data1.TrimEnd("""")
                                                 Dim dt_res As New DataTable
                                                 dt_res = JsonConvert.DeserializeObject(Of DataTable)("[" + Data2 + "]") 'JsonConvert.des(ResponseString.Replace("\", ""))

                                                 If Not dt_res Is Nothing Then
                                                     If dt_res.Columns.Contains("Status") Then
                                                         If dt_res.Rows(0)("Status").ToString.ToUpper() = "1" Then
                                                             IsStatus = True
                                                             FileCount = dt_res.Rows(0)("Message").ToString
                                                             RedirectStr = "window.open(""" + FileCount + """)"
                                                         Else
                                                             IsStatus = False
                                                             Me.objCommon.ShowAlert(dt_res.Rows(0)("Message").ToString, Me.Page)
                                                             Return False
                                                             'Exit Sub
                                                         End If
                                                     End If
                                                 End If
                                                 Return True
                                             End Using
                                         End Function)
                t.Wait()
                If IsStatus Then
                    objCommon.ShowAlert("All Images have been Transferred successfully on below path: ", Me.Page)
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WindowOpen", RedirectStr, True)

                Else
                    objCommon.ShowAlert("No Image found or Images not transferred successfully", Me.Page)
                    Exit Sub
                End If
            End If
        End If
        If gvVisitReviewStatus.Rows.Count > 0 Then
            gvVisitReviewStatus.Visible = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIVisitDatatable", "UIVisitDatatable();", True)
        End If

    End Sub

    'Protected Sub gvQueryDetails_RowCommand(sender As Object, e As GridViewCommandEventArgs)
    '    'Dim Button As LinkButton = CType(sender, LinkButton)

    '    ' Dim rowIndex As Integer = sender.Attributes("RowIndex")

    '    Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
    '    Dim row As GridViewRow = gvQueryDetails.Rows(rowIndex)
    '    Dim QC1 As String = gvQueryDetails.DataKeys(rowIndex).Values(7).ToString()
    '    Dim CA As String = gvQueryDetails.DataKeys(rowIndex).Values(10).ToString()
    '    Dim QC2 As String = gvQueryDetails.DataKeys(rowIndex).Values(11).ToString()
    '    'Dim rowIndex As Integer = Button.CommandArgument
    '    'Dim row As GridViewRow = gvQueryDetails.Rows(rowIndex)
    '    Dim WorkSpaceId As String = gvQueryDetails.DataKeys(rowIndex).Values(0).ToString()
    '    Dim ActivityId As String = gvQueryDetails.DataKeys(rowIndex).Values(1).ToString()
    '    Dim NodeId As String = gvQueryDetails.DataKeys(rowIndex).Values(2).ToString()
    '    Dim SubjectId As String = gvQueryDetails.DataKeys(rowIndex).Values(4).ToString()
    '    Dim vMySubjectNo As String = gvQueryDetails.DataKeys(rowIndex).Values(5).ToString()
    '    Dim RedirectStr As String = String.Empty
    '    Dim querystring As String = String.Empty
    '    Dim QStr As String = String.Empty
    '    Dim Mode As String = "4"
    '    Dim result As Boolean = False
    '    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    '    Dim eStr As String = String.Empty
    '    Dim ds_Subject As New DataSet
    '    Dim extractPath As String = String.Empty
    '    Dim DicomPath As String = String.Empty, ResponseString As String = String.Empty
    '    Dim originalFiles As String()
    '    Dim request As WebRequest
    '    Dim FileCount As String = String.Empty
    '    Dim IsStatus As Boolean = False
    '    Dim url As String = ""

    '    If Convert.ToString(ConfigurationManager.AppSettings("QCUserCode")).Contains(Me.Session(S_UserType)) AndAlso e.CommandName = "QC1" Then
    '        Mode = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
    '    ElseIf Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(Me.Session(S_UserType)) AndAlso e.CommandName = "CA1" Then
    '        Mode = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
    '    ElseIf (Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(Me.Session(S_UserType)) AndAlso e.CommandName = "QC1") OrElse (Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(Me.Session(S_UserType)) AndAlso e.CommandName = "QC2") Then
    '        Mode = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View
    '    ElseIf Convert.ToString(ConfigurationManager.AppSettings("QC2UserCode")).Contains(Me.Session(S_UserType)) AndAlso e.CommandName = "QC2" Then
    '        Mode = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
    '    Else
    '        Mode = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View
    '    End If

    '    GetUserProfile(Me.Session(S_UserName), e.CommandName)

    '    If e.CommandName = "QC1" Then
    '        querystring = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" & WorkSpaceId &
    '                    "&ActivityId=" & ActivityId & "&NodeId=" & NodeId &
    '                     "&SubjectId=" & SubjectId & "&MySubjectNo=" & vMySubjectNo + "&mode=" & Mode + "&UserTypeCode=" + QC1

    '        RedirectStr = "window.open(""" + querystring + """)"
    '        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WindowOpen", RedirectStr, True)

    '    ElseIf e.CommandName = "CA1" Then
    '        querystring = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" & WorkSpaceId &
    '                    "&ActivityId=" & ActivityId & "&NodeId=" & NodeId &
    '                    "&SubjectId=" & SubjectId &
    '                    "&MySubjectNo=" & vMySubjectNo + "&mode=" & Mode + "&UserTypeCode=" + CA

    '        RedirectStr = "window.open(""" + querystring + """)"
    '        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WindowOpen", RedirectStr, True)

    '    ElseIf e.CommandName = "QC2" Then
    '        querystring = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" & WorkSpaceId &
    '                    "&ActivityId=" & ActivityId & "&NodeId=" & NodeId &
    '                    "&SubjectId=" & SubjectId &
    '                    "&MySubjectNo=" & vMySubjectNo + "&mode=" & Mode + "&UserTypeCode=" + QC2

    '        RedirectStr = "window.open(""" + querystring + """)"
    '        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WindowOpen", RedirectStr, True)

    '    End If

    '    If gvQueryDetails.Rows.Count > 0 Then
    '        gvQueryDetails.Visible = True
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIVisitDatatable", "UIVisitDatatable();", True)
    '    End If
    'End Sub
    Protected Sub btnGoVisit_Click(sender As Object, e As EventArgs) Handles btnGoVisit.Click
        BtnGoClickCheck = True
        If Not Me.FillVisiteviewStatusGrid() Then

            Exit Sub
        End If
    End Sub

    'Added By Bhargav Thaker Start
    'Protected Sub btnGoCA_Click(sender As Object, e As EventArgs) Handles btnGoCA.Click
    '    BtnGoCAClickCheck = True
    '    If Not Me.FillCAViewGrid() Then

    '    End If
    'End Sub
    'Added By Bhargav Thaker End

    Private Function FillVisiteviewStatusGrid() As Boolean
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim sSubjectIds As String = String.Empty
        Dim Parameters As String = String.Empty
        Dim ds_Subject As New DataSet
        Dim ds_DCFMst As New DataSet
        Dim dv_Subject As New DataView
        Dim Ds_WorkSpaceSubjectMst As New DataSet
        Dim dtData As New DataTable
        Dim strUserTypeCode As String = String.Empty
        Dim QStr As String = String.Empty
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim ProjectId As String = String.Empty
        Dim ScreeningNo As String = String.Empty
        Dim result As Boolean = False
        Dim whereCondition As String
        Dim WBString As String = String.Empty
        Dim dv As New DataView
        Try
            gvVisitReviewStatus.DataSource = Nothing

            'Parameters = Me.HProjectId.Value.Trim() + "##" + Me.txtScreeningForDI.Text
            hdniUserId1.Value = Session(S_UserID)
            DISoftURL1.Value = Convert.ToString(ConfigurationManager.AppSettings.Item("DISoftURL").Trim())
            hdnWorkFlowStageId1.Value = Session(S_WorkFlowStageId)
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                If txtSite.Text <> String.Empty Or txtprojectForDI.SelectedIndex = 0 Then
                    txtprojectForDI.SelectedIndex = 0
                    If Me.txtScreeningForDI.Text.Trim() <> String.Empty AndAlso Me.txtVisitFromDate.Text.Trim() <> String.Empty AndAlso Me.txtVisitToDate.Text.Trim() <> String.Empty Then
                        whereCondition = "(isnull(vWorkspaceId,'') ='" + Me.HProjectId.Value.Trim() + "' OR isnull(vparentWorkspaceId,'') ='" + Me.HProjectId.Value.Trim() + "' OR '" + Me.HProjectId.Value.Trim() + "' ='') " + vbCrLf +
                        "AND iUserId = " + Me.Session(S_UserID) + " AND dModifyOn >= CAST ( '" + Me.txtVisitFromDate.Text + "'AS DATE) AND dModifyOn <= CAST ('" + Me.txtVisitToDate.Text + "'AS DATE) AND (isnull(ScreeningNo,'') = '" + Me.txtScreeningForDI.Text + "' OR '" + Me.txtScreeningForDI.Text + "'='') Order by iImgTransmittalDtlId desc"
                    Else
                        If Me.txtScreeningForDI.Text.Trim() <> String.Empty AndAlso Me.txtVisitFromDate.Text.Trim() = String.Empty AndAlso Me.txtVisitToDate.Text.Trim() = String.Empty Then
                            whereCondition = "(isnull(vWorkspaceId,'') ='" + Me.HProjectId.Value.Trim() + "' OR isnull(vparentWorkspaceId,'') ='" + Me.HProjectId.Value.Trim() + "' OR '" + Me.HProjectId.Value.Trim() + "' ='') " + vbCrLf +
                            "AND iUserId = " + Me.Session(S_UserID) + " AND (isnull(ScreeningNo,'') = '" + Me.txtScreeningForDI.Text + "' OR '" + Me.txtScreeningForDI.Text + "'='') Order by iImgTransmittalDtlId desc"

                        ElseIf Me.txtScreeningForDI.Text.Trim() = String.Empty AndAlso Me.txtVisitFromDate.Text.Trim() <> String.Empty AndAlso Me.txtVisitToDate.Text.Trim() <> String.Empty Then
                            whereCondition = "(isnull(vWorkspaceId,'') ='" + Me.HProjectId.Value.Trim() + "' OR isnull(vparentWorkspaceId,'') ='" + Me.HProjectId.Value.Trim() + "' OR '" + Me.HProjectId.Value.Trim() + "' ='') " + vbCrLf +
                        "AND iUserId = " + Me.Session(S_UserID) + " AND dModifyOn >= CAST ( '" + Me.txtVisitFromDate.Text + "'AS DATE) AND dModifyOn <= CAST ('" + Me.txtVisitToDate.Text + "'AS DATE) Order by iImgTransmittalDtlId desc"

                        Else
                            whereCondition = "(isnull(vWorkspaceId,'') ='" + Me.HProjectId.Value.Trim() + "' OR isnull(vparentWorkspaceId,'') ='" + Me.HProjectId.Value.Trim() + "' OR '" + Me.HProjectId.Value.Trim() + "' ='') " + vbCrLf + "AND iUserId = " + Me.Session(S_UserID) + " Order by iImgTransmittalDtlId desc"
                        End If
                    End If
                Else
                    If Me.txtScreeningForDI.Text.Trim() <> String.Empty AndAlso Me.txtVisitFromDate.Text.Trim() <> String.Empty AndAlso Me.txtVisitToDate.Text.Trim() <> String.Empty Then
                        whereCondition = "vWorkspaceId IN (SELECT vWorkspaceId FROM dbo.View_MyProjects WHERE iUserId=" + Me.Session(S_UserID) + " AND cWorkspaceType='C' AND ParentWorkspaceId IN (SELECT ParentWorkspaceId FROM dbo.View_MyProjects WHERE iUserId=" + Me.Session(S_UserID) + " AND isnull(ParentWorkspaceId,'')='" + Me.HProjectId.Value.Trim() + "' AND cWorkspaceType='P')) " + vbCrLf +
                        "AND iUserId = " + Me.Session(S_UserID) + " AND dModifyOn >= CAST ( '" + Me.txtVisitFromDate.Text + "'AS DATE) AND dModifyOn <= CAST ('" + Me.txtVisitToDate.Text + "'AS DATE) AND (isnull(ScreeningNo,'') = '" + Me.txtScreeningForDI.Text + "' OR '" + Me.txtScreeningForDI.Text + "'='') Order by iImgTransmittalDtlId desc"
                    Else
                        If Me.txtScreeningForDI.Text.Trim() <> String.Empty AndAlso Me.txtVisitFromDate.Text.Trim() = String.Empty AndAlso Me.txtVisitToDate.Text.Trim() = String.Empty Then
                            whereCondition = "vWorkspaceId IN (SELECT vWorkspaceId FROM dbo.View_MyProjects WHERE iUserId=" + Me.Session(S_UserID) + " AND cWorkspaceType='C' AND ParentWorkspaceId IN (SELECT ParentWorkspaceId FROM dbo.View_MyProjects WHERE iUserId=" + Me.Session(S_UserID) + " AND isnull(ParentWorkspaceId,'')='" + Me.HProjectId.Value.Trim() + "' AND cWorkspaceType='P')) " + vbCrLf +
                            "AND iUserId = " + Me.Session(S_UserID) + " AND (isnull(ScreeningNo,'') = '" + Me.txtScreeningForDI.Text + "' OR '" + Me.txtScreeningForDI.Text + "'='') Order by iImgTransmittalDtlId desc"

                        ElseIf Me.txtScreeningForDI.Text.Trim() = String.Empty AndAlso Me.txtVisitFromDate.Text.Trim() <> String.Empty AndAlso Me.txtVisitToDate.Text.Trim() <> String.Empty Then
                            whereCondition = "vWorkspaceId IN (SELECT vWorkspaceId FROM dbo.View_MyProjects WHERE iUserId=" + Me.Session(S_UserID) + " AND cWorkspaceType='C' AND ParentWorkspaceId IN (SELECT ParentWorkspaceId FROM dbo.View_MyProjects WHERE iUserId=" + Me.Session(S_UserID) + " AND isnull(ParentWorkspaceId,'')='" + Me.HProjectId.Value.Trim() + "' AND cWorkspaceType='P')) " + vbCrLf +
                        "AND iUserId = " + Me.Session(S_UserID) + " AND dModifyOn >= CAST ( '" + Me.txtVisitFromDate.Text + "'AS DATE) AND dModifyOn <= CAST ('" + Me.txtVisitToDate.Text + "'AS DATE) Order by iImgTransmittalDtlId desc"

                        Else
                            whereCondition = "vWorkspaceId IN (SELECT vWorkspaceId FROM dbo.View_MyProjects WHERE iUserId=" + Me.Session(S_UserID) + " AND cWorkspaceType='C' AND ParentWorkspaceId IN (SELECT ParentWorkspaceId FROM dbo.View_MyProjects WHERE iUserId=" + Me.Session(S_UserID) + " AND isnull(ParentWorkspaceId,'')='" + Me.HProjectId.Value.Trim() + "' AND cWorkspaceType='P')) " + vbCrLf + "AND iUserId = " + Me.Session(S_UserID) + " Order by iImgTransmittalDtlId desc"
                        End If
                    End If
                End If

                'whereCondition = "(isnull(vWorkspaceId,'') ='" + Me.HProjectId.Value.Trim() + " ' OR '" + Me.HProjectId.Value.Trim() + "' ='') " + vbCrLf +
                '    "AND iUserId = " + Me.Session(S_UserID) + " AND dModifyOn >= CAST ( '" + Me.txtVisitFromDate.Text + "'AS DATE) AND dModifyOn <= CAST ('" + Me.txtVisitToDate.Text + "'AS DATE) AND (isnull(ScreeningNo,'') = '" + Me.txtScreeningForDI.Text + "' OR '" + Me.txtScreeningForDI.Text + "'='') Order by iImgTransmittalDtlId desc"
                result = DBHelp.GetFieldsOfTable("View_GetVisitStatus", " * ", whereCondition, ds_Subject, eStr)

                If ddlVisitStatus.SelectedValue <> "All" Then
                    If ddlVisitStatus.SelectedValue = "ImageUploader" Then
                        WBString = "IN ('Uploaded', 'Rejected')"
                    ElseIf ddlVisitStatus.SelectedValue = "QC1" Then
                        WBString = "IN ('Uploaded', 'Reuploaded')"
                    ElseIf ddlVisitStatus.SelectedValue = "QC2" Then
                        WBString = "IN ('QC1 Review Complete')"
                    ElseIf ddlVisitStatus.SelectedValue = "CA" Then
                        WBString = "IN ('QC2 Review Complete')"
                        'ElseIf ddlVisitStatus.SelectedValue = "Radiologist1" Then
                        'WBString = "IN ('CA Complete')"
                    ElseIf ddlVisitStatus.SelectedValue = "R12" Then
                        WBString = "IN ('CA1 Review Complete')"
                    ElseIf ddlVisitStatus.SelectedValue = "Pending" AndAlso Convert.ToString(ConfigurationManager.AppSettings("ImageUploaderUserCode")).Contains(Me.Session(S_UserType)) Then
                        WBString = "IN ('Uploaded', 'Rejected')"
                    ElseIf ddlVisitStatus.SelectedValue = "Pending" AndAlso Convert.ToString(ConfigurationManager.AppSettings("QCUserCode")).Contains(Me.Session(S_UserType)) Then
                        WBString = "IN ('Uploaded', 'Reuploaded')"
                    ElseIf ddlVisitStatus.SelectedValue = "Pending" AndAlso Convert.ToString(ConfigurationManager.AppSettings("QC2UserCode")).Contains(Me.Session(S_UserType)) Then
                        WBString = "IN ('QC1 Review Complete')"
                        'ElseIf ddlVisitStatus.SelectedValue = "Pending" AndAlso Convert.ToString(ConfigurationManager.AppSettings("QC2UserCode")).Contains(Me.Session(S_UserType)) Then
                        '    WBString = "IN ('Uploaded', 'QC1 Review Complete')"
                    ElseIf ddlVisitStatus.SelectedValue = "Pending" AndAlso Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(Me.Session(S_UserType)) Then
                        WBString = "IN ('QC2 Review Complete')"
                        'ElseIf ddlVisitStatus.SelectedValue = "Pending" AndAlso Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(Me.Session(S_UserType)) Then
                        '    WBString = "IN ('Uploaded', 'QC2 Review Complete')"
                        'Condition Added by Bhargav Thaker for Radiologist usertype
                    ElseIf ddlVisitStatus.SelectedValue = "Pending" AndAlso (Me.Session(S_UserType) = "0118" Or Me.Session(S_UserType) = "0119") Then
                        WBString = "IN ('R1 Complete - R2-Pending', 'R2 Complete - R1-Pending')"
                    Else
                        WBString = " = Status"
                    End If
                Else
                    WBString = " = Status"
                End If

                dv = ds_Subject.Tables(0).DefaultView
                dv.RowFilter = "Status  " + WBString
                If dv.ToTable().Rows.Count < 1 Then
                    dv = ds.Tables(0).DefaultView
                End If
            Else
            End If

            If ds_Subject Is Nothing Then
                gvVisitReviewStatus.DataSource = Nothing
                gvVisitReviewStatus.DataBind()
                emptyTable.Visible = True
                If gvVisitReviewStatus.Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIVisitDatatable", "UIVisitDatatable();", True)
                End If
                Return False
            ElseIf ds_Subject.Tables.Count = 0 Then
                gvVisitReviewStatus.DataSource = Nothing
                gvVisitReviewStatus.DataBind()
                emptyTable.Visible = True
                If gvVisitReviewStatus.Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIVisitDatatable", "UIVisitDatatable();", True)
                End If
                Return False
            End If

            If ds_Subject.Tables(0).Rows.Count = 0 Then
                If BtnGoClickCheck Then
                    gvVisitReviewStatus.DataSource = Nothing
                    gvVisitReviewStatus.DataBind()

                    If gvVisitReviewStatus.Rows.Count > 0 Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIVisitDatatable", "UIVisitDatatable();", True)
                    End If
                    Return False
                Else
                    emptyTable.Visible = True
                End If
            End If

            gvVisitReviewStatus.DataSource = Nothing
            dv_Subject = dv

            If dv_Subject.ToTable.Rows.Count > 0 Then
                emptyTable.Visible = False
                dtData = dv_Subject.ToTable()
                Me.ViewState("getData") = dtData
                gvVisitReviewStatus.DataSource = dtData
                If Session(S_WorkFlowStageId) = WorkFlowStageId_OnlyView Then
                    hdnAdjUserId1.Value = Me.Session(S_UserID)
                    hdnUserTypeCode1.Value = Me.Session(S_UserType)
                End If
            End If

            gvVisitReviewStatus.DataBind()
            'If Me.Session(S_UserNameWithProfile).ToString.Contains("QA Reviewer") Then
            If Session(S_WorkFlowStageId) = WorkFlowStageId_OnlyView Then
                gvVisitReviewStatus.Columns(11).Visible = True
                gvVisitReviewStatus.Columns(12).Visible = True
            Else
                gvVisitReviewStatus.Columns(11).Visible = False
                gvVisitReviewStatus.Columns(12).Visible = False
            End If

            If gvVisitReviewStatus.Rows.Count > 0 Then
                Dim json As String = " var pageJson = " + JsonConvert.SerializeObject(dtData) + ";"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIVisitDatatable", json + " UIVisitDatatable();", True)
            End If

            If String.IsNullOrEmpty(TryCast(gvVisitReviewStatus.Rows(0).FindControl("hfSiteNo"), HiddenField).Value) Then
                gvVisitReviewStatus.Rows(0).Visible = False
                emptyTable.Visible = True
            Else
                emptyTable.Visible = False
            End If

            Return True

            'End
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....FillVisiteviewStatusGrid")
            Return False
        Finally
            If Not ds_Subject Is Nothing Then
                ds_Subject.Dispose()
            End If
        End Try
    End Function

    'Added By Bhargav Thaker Start
    'Private Function FillCAViewGrid() As Boolean
    '    Dim eStr As String = String.Empty
    '    Dim ds_Subject As New DataSet
    '    Dim dv_Subject As New DataView
    '    Dim dtData As New DataTable
    '    Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    '    Dim result As Boolean = False
    '    Dim whereCondition As String

    '    Try
    '        gvCA.DataSource = Nothing

    '        whereCondition = "(isnull(vWorkspaceId,'') ='" + Me.HdnProjectId.Value.Trim() + " ' OR '" + Me.HdnProjectId.Value.Trim() + "' ='') " + vbCrLf +
    '                "AND iUserId = " + Me.Session(S_UserID) + " GROUP BY SiteNo,PatientInitial,ScreeningNo,vWorkspaceId,vSubjectId ORDER BY vWorkspaceId,vSubjectId"
    '        result = DBHelp.GetFieldsOfTable("View_GetVisitStatus", " SiteNo,PatientInitial,ScreeningNo,vWorkspaceId,vSubjectId ", whereCondition, ds_Subject, eStr)

    '        If ds_Subject Is Nothing Then
    '            gvCA.DataSource = Nothing
    '            gvCA.DataBind()
    '            If gvCA.Rows.Count > 0 Then
    '                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UICADatatable", "UICADatatable();", True)
    '            End If
    '            Return False
    '        ElseIf ds_Subject.Tables.Count = 0 Then
    '            gvCA.DataSource = Nothing
    '            gvCA.DataBind()
    '            If gvCA.Rows.Count > 0 Then
    '                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UICADatatable", "UICADatatable();", True)
    '            End If
    '            Return False
    '        End If

    '        If ds_Subject.Tables(0).Rows.Count = 0 Then
    '            If BtnGoCAClickCheck Then
    '                gvCA.DataSource = Nothing
    '                gvCA.DataBind()
    '                If gvCA.Rows.Count > 0 Then
    '                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UICADatatable", "UICADatatable();", True)
    '                End If
    '                Return False
    '            End If
    '        End If

    '        gvCA.DataSource = Nothing
    '        dv_Subject = ds_Subject.Tables(0).DefaultView

    '        If dv_Subject.ToTable.Rows.Count > 0 Then
    '            dtData = dv_Subject.ToTable()
    '            Me.ViewState("getData") = dtData
    '            gvCA.DataSource = dtData
    '        End If

    '        gvCA.DataBind()

    '        If gvCA.Rows.Count > 0 Then
    '            Dim json As String = " var pageJson = " + JsonConvert.SerializeObject(dtData) + ";"
    '            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UICADatatable", json + " UICADatatable();", True)
    '        End If

    '        If ds_Subject.Tables(0).Rows.Count = 0 Then
    '            CAEmptytbl.Visible = True
    '        Else
    '            CAEmptytbl.Visible = False
    '        End If

    '        Return True

    '    Catch ex As Exception
    '        ShowErrorMessage(ex.Message, ".....FillCAViewGrid")
    '        Return False
    '    Finally
    '        If Not ds_Subject Is Nothing Then
    '            ds_Subject.Dispose()
    '        End If
    '    End Try
    'End Function
    'Added By Bhargav Thaker End

    Protected Sub gvVisitReviewStatus_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvVisitReviewStatus.RowDataBound
        Dim dt_SelectedDateDtl As New DataTable

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(Gvc_SrnO).Text = e.Row.RowIndex + 1

            If Convert.ToString(ConfigurationManager.AppSettings("ImageUploaderUserCode")).Contains(Me.Session(S_UserType)) Then
                CType(e.Row.FindControl("btnImageType"), ImageButton).Visible = True
                CType(e.Row.FindControl("btnQC"), ImageButton).Visible = False
                CType(e.Row.FindControl("btnQC2"), ImageButton).Visible = False
                CType(e.Row.FindControl("btnGrader"), ImageButton).Visible = False
            ElseIf Convert.ToString(ConfigurationManager.AppSettings("QCUserCode")).Contains(Me.Session(S_UserType)) Then
                CType(e.Row.FindControl("btnImageType"), ImageButton).Visible = True
                CType(e.Row.FindControl("btnQC"), ImageButton).Visible = True
                CType(e.Row.FindControl("btnQC2"), ImageButton).Visible = False
                CType(e.Row.FindControl("btnGrader"), ImageButton).Visible = False
            ElseIf Convert.ToString(ConfigurationManager.AppSettings("QC2UserCode")).Contains(Me.Session(S_UserType)) Then
                CType(e.Row.FindControl("btnQC"), ImageButton).Visible = False
                CType(e.Row.FindControl("btnImageType"), ImageButton).Visible = True
                CType(e.Row.FindControl("btnQC2"), ImageButton).Visible = True
                CType(e.Row.FindControl("btnGrader"), ImageButton).Visible = False
            ElseIf Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(Me.Session(S_UserType)) Then
                CType(e.Row.FindControl("btnQC"), ImageButton).Visible = True
                CType(e.Row.FindControl("btnImageType"), ImageButton).Visible = True
                CType(e.Row.FindControl("btnQC2"), ImageButton).Visible = True
                CType(e.Row.FindControl("btnGrader"), ImageButton).Visible = True
            ElseIf Session(S_WorkFlowStageId) = WorkFlowStageId_OnlyView Then
                CType(e.Row.FindControl("btnQC"), ImageButton).Visible = True
                CType(e.Row.FindControl("btnImageType"), ImageButton).Visible = False
                CType(e.Row.FindControl("btnQC2"), ImageButton).Visible = True
                CType(e.Row.FindControl("btnGrader"), ImageButton).Visible = True
            Else
                CType(e.Row.FindControl("btnQC"), ImageButton).Visible = True
                CType(e.Row.FindControl("btnImageType"), ImageButton).Visible = True
                CType(e.Row.FindControl("btnQC2"), ImageButton).Visible = True
                CType(e.Row.FindControl("btnGrader"), ImageButton).Visible = True
            End If
        End If

    End Sub

    'Protected Sub btnGoQuery_Click(sender As Object, e As EventArgs) Handles btnGoQuery.Click
    '    BtnGoQueryClickCheck = True
    '    If Not Me.FillQueryDetailsGrid() Then

    '        Exit Sub
    '    End If
    'End Sub
    'Private Function FillQueryDetailsGrid() As Boolean
    '    Dim wStr As String = String.Empty
    '    Dim eStr As String = String.Empty
    '    Dim sSubjectIds As String = String.Empty
    '    Dim Parameters As String = String.Empty
    '    Dim ds_Subject As New DataSet
    '    Dim ds_DCFMst As New DataSet
    '    Dim dv_Subject As New DataView
    '    Dim Ds_WorkSpaceSubjectMst As New DataSet
    '    Dim dtData As New DataTable
    '    Dim strUserTypeCode As String = String.Empty
    '    Dim QStr As String = String.Empty
    '    Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    '    Dim ProjectId As String = String.Empty
    '    Dim ScreeningNo As String = String.Empty
    '    Dim result As Boolean = False
    '    Dim whereCondition As String
    '    Dim WBString As String = String.Empty
    '    Dim dv As New DataView
    '    Try

    '        gvQueryDetails.DataSource = Nothing
    '        whereCondition = "cStatusIndi <> 'D'"
    '        result = DBHelp.GetFieldsOfTable("View_GetQueryMaster", " * ", whereCondition, ds_Subject, eStr)

    '        If ddlQueryStatus.SelectedValue <> "All" Then
    '            If ddlQueryStatus.SelectedValue = "ImageUploader" Then
    '                WBString = "IN ('Uploaded', 'Rejected')"
    '            ElseIf ddlQueryStatus.SelectedValue = "QC1" Then
    '                WBString = "IN ('Uploaded', 'Reuploaded')"
    '            ElseIf ddlQueryStatus.SelectedValue = "QC2" Then
    '                WBString = "IN ('QC1 Review Complete')"
    '            ElseIf ddlQueryStatus.SelectedValue = "CA" Then
    '                WBString = "IN ('QC2 Review Complete')"
    '            ElseIf ddlVisitStatus.SelectedValue = "Pending" AndAlso Convert.ToString(ConfigurationManager.AppSettings("ImageUploaderUserCode")).Contains(Me.Session(S_UserType)) Then
    '                WBString = "IN ('Uploaded', 'Rejected')"
    '            ElseIf ddlVisitStatus.SelectedValue = "Pending" AndAlso Convert.ToString(ConfigurationManager.AppSettings("QCUserCode")).Contains(Me.Session(S_UserType)) Then
    '                WBString = "IN ('Uploaded', 'Reuploaded')"
    '            ElseIf ddlVisitStatus.SelectedValue = "Pending" AndAlso Convert.ToString(ConfigurationManager.AppSettings("QC2UserCode")).Contains(Me.Session(S_UserType)) Then
    '                WBString = "IN ('QC1 Review Complete')"
    '            ElseIf ddlVisitStatus.SelectedValue = "Pending" AndAlso Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(Me.Session(S_UserType)) Then
    '                WBString = "IN ('QC2 Review Complete')"
    '            Else
    '                WBString = " = Status"
    '            End If
    '        Else
    '            WBString = " = Status"
    '        End If


    '        dv = ds_Subject.Tables(0).DefaultView

    '        'dv.RowFilter = "Status  " + WBString
    '        'If dv.ToTable().Rows.Count < 1 Then
    '        '    dv = ds.Tables(0).DefaultView
    '        'End If




    '        If ds_Subject Is Nothing Then
    '            'objCommon.ShowAlert("No data found.", Me.Page)
    '            gvQueryDetails.DataSource = Nothing
    '            gvQueryDetails.DataBind()
    '            If gvQueryDetails.Rows.Count > 0 Then
    '                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIQueryDatatable", "UIQueryDatatable();", True)
    '            End If
    '            Return False
    '        ElseIf ds_Subject.Tables.Count = 0 Then
    '            'objCommon.ShowAlert("No data found.", Me.Page)
    '            gvQueryDetails.DataSource = Nothing
    '            gvQueryDetails.DataBind()
    '            If gvQueryDetails.Rows.Count > 0 Then
    '                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIQueryDatatable", "UIQueryDatatable();", True)
    '            End If
    '            Return False
    '        End If

    '        If ds_Subject.Tables(0).Rows.Count = 0 Then
    '            If BtnGoClickCheck Then
    '                'objCommon.ShowAlert("No data found.", Me.Page)
    '                gvQueryDetails.DataSource = Nothing
    '                gvQueryDetails.DataBind()

    '                If gvQueryDetails.Rows.Count > 0 Then
    '                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIQueryDatatable", "UIQueryDatatable();", True)
    '                End If
    '                Return False
    '            End If

    '        End If

    '        gvQueryDetails.DataSource = Nothing
    '        dv_Subject = dv

    '        If dv_Subject.ToTable.Rows.Count > 0 Then
    '            'Me.gvwCertificateReviewStatus.AutoGenerateColumns = True
    '            dtData = dv_Subject.ToTable()
    '            Me.ViewState("getData") = dtData
    '            gvQueryDetails.DataSource = dtData

    '        End If

    '        gvQueryDetails.DataBind()

    '        If gvQueryDetails.Rows.Count > 0 Then
    '            Dim json As String = " var pageJson = " + JsonConvert.SerializeObject(dtData) + ";"
    '            'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "pagejson", json, True)
    '            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIQueryDatatable", json + " UIQueryDatatable();", True)
    '        End If

    '        If String.IsNullOrEmpty(TryCast(gvQueryDetails.Rows(0).FindControl("hfSiteNo"), HiddenField).Value) Then
    '            gvQueryDetails.Rows(0).Visible = False
    '            emptyTable.Visible = True
    '        Else
    '            emptyTable.Visible = False
    '        End If

    '        Return True

    '        'End
    '    Catch ex As Exception
    '        ShowErrorMessage(ex.Message, ".....gvQueryDetails")
    '        Return False
    '    Finally
    '        If Not ds_Subject Is Nothing Then
    '            ds_Subject.Dispose()
    '        End If
    '        '=====================================================================
    '    End Try

    'End Function
    'Protected Sub gvQueryDetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvQueryDetails.RowDataBound
    '    Dim dt_SelectedDateDtl As New DataTable

    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        e.Row.Cells(Gvc_SrnO).Text = e.Row.RowIndex + 1

    '        If Convert.ToString(ConfigurationManager.AppSettings("ImageUploaderUserCode")).Contains(Me.Session(S_UserType)) Then
    '            CType(e.Row.FindControl("btnImageType"), ImageButton).Visible = True
    '            CType(e.Row.FindControl("btnQC"), ImageButton).Visible = False
    '            CType(e.Row.FindControl("btnQC2"), ImageButton).Visible = False
    '            CType(e.Row.FindControl("btnGrader"), ImageButton).Visible = False
    '        ElseIf Convert.ToString(ConfigurationManager.AppSettings("QCUserCode")).Contains(Me.Session(S_UserType)) Then
    '            CType(e.Row.FindControl("btnImageType"), ImageButton).Visible = True
    '            CType(e.Row.FindControl("btnQC"), ImageButton).Visible = False
    '            CType(e.Row.FindControl("btnQC2"), ImageButton).Visible = False
    '            CType(e.Row.FindControl("btnGrader"), ImageButton).Visible = False
    '        ElseIf Convert.ToString(ConfigurationManager.AppSettings("QC2UserCode")).Contains(Me.Session(S_UserType)) Then
    '            CType(e.Row.FindControl("btnQC"), ImageButton).Visible = False
    '            CType(e.Row.FindControl("btnImageType"), ImageButton).Visible = True
    '            CType(e.Row.FindControl("btnQC2"), ImageButton).Visible = False
    '            CType(e.Row.FindControl("btnGrader"), ImageButton).Visible = False
    '        ElseIf Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(Me.Session(S_UserType)) Then
    '            CType(e.Row.FindControl("btnQC"), ImageButton).Visible = False
    '            CType(e.Row.FindControl("btnImageType"), ImageButton).Visible = True
    '            CType(e.Row.FindControl("btnQC2"), ImageButton).Visible = False
    '            CType(e.Row.FindControl("btnGrader"), ImageButton).Visible = False
    '        Else
    '            CType(e.Row.FindControl("btnQC"), ImageButton).Visible = False
    '            'CType(e.Row.FindControl("btnImageType"), ImageButton).Visible = True
    '            CType(e.Row.FindControl("btnQC2"), ImageButton).Visible = False
    '            ' CType(e.Row.FindControl("btnGrader"), ImageButton).Visible = True
    '        End If
    '    End If

    'End Sub
    Private Function GetUserProfile(ByVal UserName As String, ByVal CommandName As String) As Boolean
        Dim eStr As String = String.Empty
        Dim WhereCondition As String
        Dim result As Boolean = False
        Dim ds_Subject As New DataSet
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim dv As New DataView
        Try
            WhereCondition = "vUserName = '" + UserName + "' AND vUserTypeName = '" + CommandName + "'"
            result = DBHelp.GetFieldsOfTable("View_UserMst", " iUserId, vUserTypeCode ", WhereCondition, ds_Subject, eStr)

            dv = ds_Subject.Tables(0).DefaultView

            UserId = dv.Item(0).Row("iUserId")

            If CommandName = "Adjudicator" Then
                hdnAdjUserId.Value = dv.Item(0).Row("iUserId")
                hdnUserTypeCode.Value = dv.Item(0).Row("vUserTypeCode")
            End If


        Catch ex As Exception
            ShowErrorMessage(ex.Message, "Error")
            Return False
        End Try
    End Function

    Protected Sub btnGoAdjucator_Click(sender As Object, e As EventArgs) Handles btnGoAdjucator.Click
        BtnGoAdjuClickCheck = True
        If Not Me.FillAdjucatorviewStatusGrid() Then

            Exit Sub
        End If
    End Sub

    Private Function FillAdjucatorviewStatusGrid() As Boolean
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim sSubjectIds As String = String.Empty
        Dim Parameters As String = String.Empty
        Dim ds_Subject As New DataSet
        Dim ds_DCFMst As New DataSet
        Dim dv_Subject As New DataView
        Dim Ds_WorkSpaceSubjectMst As New DataSet
        Dim dtData As New DataTable
        Dim strUserTypeCode As String = String.Empty
        Dim QStr As String = String.Empty
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim ProjectId As String = String.Empty
        Dim ScreeningNo As String = String.Empty
        Dim result As Boolean = False
        Dim whereCondition As String
        Dim WBString As String = String.Empty
        Dim dv As New DataView
        Try
            gvAdjucatorReviewStatus.DataSource = Nothing

            ' Parameters = Me.HProjectId.Value.Trim() + "##" + Me.txtScreeningForDI.Text
            hdniUserId.Value = Session(S_UserID)
            'hdniUserIP.Value = Request.ServerVariables("REMOTE_ADDR")
            DISoftURL.Value = Convert.ToString(ConfigurationManager.AppSettings.Item("DISoftURL").Trim())
            hdnWorkFlowStageId.Value = Session(S_WorkFlowStageId)
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                If Me.txtSite.Text <> String.Empty And txtprojectForDI.SelectedIndex = 0 Then
                    txtprojectForDI.SelectedIndex = 0
                    whereCondition = "R1vMedExResult <> R2vMedExResult AND (isnull(vWorkspaceId,'') ='" + Me.HProjectId.Value.Trim() + "' OR '" + Me.HProjectId.Value.Trim() + "' ='') "
                Else
                    whereCondition = "R1vMedExResult <> R2vMedExResult AND vWorkspaceId IN (SELECT vWorkspaceId FROM dbo.View_MyProjects WHERE iUserId=" + Me.Session(S_UserID) + " AND cWorkspaceType='C' AND ParentWorkspaceId IN (SELECT ParentWorkspaceId FROM dbo.View_MyProjects WHERE iUserId=" + Me.Session(S_UserID) + "AND (ISNULL(ParentWorkspaceId,'')='" + Me.HProjectId.Value.Trim() + "' OR '" + Me.HProjectId.Value.Trim() + "' ='') And cWorkspaceType='P'))"
                End If

                If ddlAdjudicator.SelectedValue <> "All" Then
                    whereCondition = whereCondition + "AND Status = '" + ddlAdjudicator.SelectedValue + "'"
                End If

                '"AND iUserId = " + Me.Session(S_UserID) + " AND (isnull(ScreeningNo,'') = '" + Me.txtScreeningForDI.Text + "' OR '" + Me.txtScreeningForDI.Text + "'='')"
                result = DBHelp.GetFieldsOfTable("View_GetAdjudicatorDashboard", " * ", whereCondition, ds_Subject, eStr)

                'If ddlVisitStatus.SelectedValue <> "All" Then
                '    If ddlVisitStatus.SelectedValue = "ImageUploader" Then
                '        WBString = "IN ('Uploaded', 'Rejected')"
                '    ElseIf ddlVisitStatus.SelectedValue = "QC1" Then
                '        WBString = "IN ('Uploaded', 'Reuploaded')"
                '    ElseIf ddlVisitStatus.SelectedValue = "Radiologist1" Then
                '        WBString = "IN ('QC Review Complete')"
                '    Else
                '        WBString = " = Status"
                '    End If
                'Else
                '   WBString = " = Status"
                'End If
                dv = ds_Subject.Tables(0).DefaultView
                'dv.RowFilter = "Status  " + WBString
                'If dv.ToTable().Rows.Count < 1 Then
                'dv = ds.Tables(0).DefaultView
                'End If
            Else
            End If

            If ds_Subject Is Nothing Then
                'objCommon.ShowAlert("No data found.", Me.Page)
                gvAdjucatorReviewStatus.DataSource = Nothing
                gvAdjucatorReviewStatus.DataBind()
                Table2.Visible = True
                If gvAdjucatorReviewStatus.Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIAdjucatorDatatable", "UIAdjucatorDatatable();", True)
                End If
                Return False
            ElseIf ds_Subject.Tables.Count = 0 Then
                gvAdjucatorReviewStatus.DataSource = Nothing
                gvAdjucatorReviewStatus.DataBind()
                Table2.Visible = True
                If gvAdjucatorReviewStatus.Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIAdjucatorDatatable", "UIAdjucatorDatatable();", True)
                End If
                Return False
            Else
                Table2.Visible = False
            End If

            If ds_Subject.Tables(0).Rows.Count = 0 Then
                If BtnGoAdjuClickCheck Then
                    gvAdjucatorReviewStatus.DataSource = Nothing
                    gvAdjucatorReviewStatus.DataBind()
                    If gvAdjucatorReviewStatus.Rows.Count > 0 Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIAdjucatorDatatable", "UIAdjucatorDatatable();", True)
                    End If
                    Return False
                Else
                    Table2.Visible = True
                End If
            End If

            gvAdjucatorReviewStatus.DataSource = Nothing
            dv_Subject = dv
            If dv_Subject.ToTable.Rows.Count > 0 Then
                'Me.gvwCertificateReviewStatus.AutoGenerateColumns = True
                Table2.Visible = False
                dtData = dv_Subject.ToTable()
                Me.ViewState("getData") = dtData
                gvAdjucatorReviewStatus.DataSource = dtData

                If (Me.Session(S_UserNameWithProfile).ToString.Contains("Adjudicator")) Then
                    GetUserProfile(Me.Session(S_UserName), "Adjudicator")
                ElseIf Session(S_WorkFlowStageId) = WorkFlowStageId_OnlyView Then   'If (Me.Session(S_UserNameWithProfile).ToString.Contains("QA Reviewer")) Then
                    hdnAdjUserId.Value = Me.Session(S_UserID)
                    hdnUserTypeCode.Value = Me.Session(S_UserType)
                    'GetUserProfile(Me.Session(S_UserName), Me.Session(S_UserNameWithProfile).Split("-")(1).Trim())
                End If
            End If

            gvAdjucatorReviewStatus.DataBind()

            If Me.Session(S_UserNameWithProfile).ToString.Contains("Adjudicator") Then
                gvAdjucatorReviewStatus.Columns(7).Visible = True
                gvAdjucatorReviewStatus.Columns(8).Visible = True
                gvAdjucatorReviewStatus.Columns(9).Visible = False
            ElseIf Session(S_WorkFlowStageId) = WorkFlowStageId_OnlyView Then 'If Me.Session(S_UserNameWithProfile).ToString.Contains("QA Reviewer") Then
                gvAdjucatorReviewStatus.Columns(7).Visible = False
                gvAdjucatorReviewStatus.Columns(8).Visible = False
                gvAdjucatorReviewStatus.Columns(9).Visible = True
            Else
                gvAdjucatorReviewStatus.Columns(7).Visible = False
                gvAdjucatorReviewStatus.Columns(8).Visible = False
                gvAdjucatorReviewStatus.Columns(9).Visible = False
            End If

            If gvAdjucatorReviewStatus.Rows.Count > 0 Then
                Dim json As String = " var pageJson = " + JsonConvert.SerializeObject(dtData) + ";"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIAdjucatorDatatable", json + " UIAdjucatorDatatable();", True)
            End If

            If String.IsNullOrEmpty(TryCast(gvAdjucatorReviewStatus.Rows(0).FindControl("hfSiteNo"), HiddenField).Value) Then
                gvAdjucatorReviewStatus.Rows(0).Visible = False
                emptyTable.Visible = True
            Else
                emptyTable.Visible = False
            End If

            Return True

            'End
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....FillAdjucatorviewStatusGrid")
            Return False
        Finally
            If Not ds_Subject Is Nothing Then
                ds_Subject.Dispose()
            End If
            '=====================================================================
        End Try

    End Function

    Protected Sub gvAdjucatorReviewStatus_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        'Dim Button As LinkButton = CType(sender, LinkButton)
        ' Dim rowIndex As Integer = sender.Attributes("RowIndex")

        Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
        Dim row As GridViewRow = gvAdjucatorReviewStatus.Rows(rowIndex)

        'Dim rowIndex As Integer = Button.CommandArgument
        'Dim row As GridViewRow = gvVisitReviewStatus.Rows(rowIndex)
        Dim WorkSpaceId As String = gvAdjucatorReviewStatus.DataKeys(rowIndex).Values(0).ToString()
        Dim ParentWorkSpaceId As String = gvAdjucatorReviewStatus.DataKeys(rowIndex).Values(1).ToString()
        Dim SubjectId As String = gvAdjucatorReviewStatus.DataKeys(rowIndex).Values(2).ToString()
        Dim MySubjectNo As String = gvAdjucatorReviewStatus.DataKeys(rowIndex).Values(3).ToString()
        Dim PeriodId As String = gvAdjucatorReviewStatus.DataKeys(rowIndex).Values(4).ToString()
        Dim ScreeningNo As String = gvAdjucatorReviewStatus.DataKeys(rowIndex).Values(6).ToString()
        'Dim QC1 As String = gvAdjucatorReviewStatus.DataKeys(rowIndex).Values(7).ToString()
        Dim iImgTransmittalDtlId As String = gvAdjucatorReviewStatus.DataKeys(rowIndex).Values(7).ToString()
        Dim Grader As String = gvAdjucatorReviewStatus.DataKeys(rowIndex).Values(8).ToString()
        Dim parentActivityID As String = gvAdjucatorReviewStatus.DataKeys(rowIndex).Values(9).ToString()
        Dim CA As String = gvAdjucatorReviewStatus.DataKeys(rowIndex).Values(10).ToString()
        Dim PerentNodeId As String = gvAdjucatorReviewStatus.DataKeys(rowIndex).Values(11).ToString()
        Dim ActivityId As String = gvAdjucatorReviewStatus.DataKeys(rowIndex).Values(13).ToString()
        Dim NodeId As String = gvAdjucatorReviewStatus.DataKeys(rowIndex).Values(15).ToString()
        Dim ActivityName As String = gvAdjucatorReviewStatus.DataKeys(rowIndex).Values(19).ToString()
        Dim SubActivityName As String = gvAdjucatorReviewStatus.DataKeys(rowIndex).Values(20).ToString()
        'Dim QCId As String = gvVisitReviewStatus.DataKeys(rowIndex).Values(9).ToString()
        'Dim GraderId As String = gvVisitReviewStatus.DataKeys(rowIndex).Values(10).ToString()
        Dim ActivityName1 As String = "ADJUDICATOR"
        Dim RedirectStr As String = String.Empty
        Dim querystring As String = String.Empty
        Dim QStr As String = String.Empty
        Dim Mode As String = "4"
        Dim result As Boolean = False
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim eStr As String = String.Empty
        Dim ds_Subject As New DataSet
        Dim extractPath As String = String.Empty
        Dim DicomPath As String = String.Empty, ResponseString As String = String.Empty
        Dim FileCount As String = String.Empty
        Dim IsStatus As Boolean = False
        Dim url As String = ""
        Dim Project As String = String.Empty
        Dim dsDicom As New DataSet
        Dim iModalityNo As String = String.Empty
        Dim iAnatomyNo As String = String.Empty
        Dim iImgTransmittalHdrId As String = String.Empty
        Dim iImageStatus As String = String.Empty
        'Dim ActivityName As String = String.Empty
        Dim RDicom As String = String.Empty
        Dim wstr As String = String.Empty
        Dim UserProfile As String = "ADJUDICATOR"
        'If Convert.ToString(ConfigurationManager.AppSettings("QCUserCode")).Contains(Me.Session(S_UserType)) AndAlso e.CommandName = "QC1" Then
        '    Mode = 1
        'ElseIf Convert.ToString(ConfigurationManager.AppSettings("QCUserCode")).Contains(Me.Session(S_UserType)) AndAlso e.CommandName = "QC1" Then
        '    Mode = 2
        'ElseIf Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(Me.Session(S_UserType)) AndAlso e.CommandName = "CA1" Then
        '    Mode = 1
        'ElseIf Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(Me.Session(S_UserType)) AndAlso e.CommandName = "CA1" Then
        '    Mode = 2
        'ElseIf Convert.ToString(ConfigurationManager.AppSettings("QC2UserCode")).Contains(Me.Session(S_UserType)) AndAlso e.CommandName = "QC2" Then
        '    Mode = 1
        'ElseIf Convert.ToString(ConfigurationManager.AppSettings("QC2UserCode")).Contains(Me.Session(S_UserType)) AndAlso e.CommandName = "QC2" Then
        '    Mode = 2
        'Else
        '    Mode = 4
        'End If

        GetUserProfile(Me.Session(S_UserName), e.CommandName)

        If e.CommandName = "QC1" Then
            querystring = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" & WorkSpaceId &
                        "&ActivityId=" & ActivityId & "&NodeId=" & NodeId &
                        "&PeriodId=" & PeriodId & "&SubjectId=" & SubjectId &
                        "&MySubjectNo=" & MySubjectNo + "&ScreenNo=" & ScreeningNo + "&mode=" & Mode + "&UserTypeCode=" + Me.Session(S_UserType) + "&UserId=" + UserId

            RedirectStr = "window.open(""" + querystring + """)"
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WindowOpen", RedirectStr, True)

        ElseIf e.CommandName = "CA1" Then
            querystring = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" & WorkSpaceId &
                        "&ActivityId=" & ActivityId & "&NodeId=" & NodeId &
                        "&PeriodId=" & PeriodId & "&SubjectId=" & SubjectId &
                        "&MySubjectNo=" & MySubjectNo + "&ScreenNo=" & ScreeningNo + "&mode=" & Mode + "&UserTypeCode=" + CA + "&UserId=" + UserId

            RedirectStr = "window.open(""" + querystring + """)"
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WindowOpen", RedirectStr, True)

        ElseIf e.CommandName = "QC2" Then
            querystring = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" & WorkSpaceId &
                        "&ActivityId=" & ActivityId & "&NodeId=" & NodeId &
                        "&PeriodId=" & PeriodId & "&SubjectId=" & SubjectId &
                        "&MySubjectNo=" & MySubjectNo + "&ScreenNo=" & ScreeningNo + "&mode=" & Mode + "&UserTypeCode=" + Me.Session(S_UserType) + "&UserId=" + UserId

            RedirectStr = "window.open(""" + querystring + """)"
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WindowOpen", RedirectStr, True)

        ElseIf e.CommandName = "AdjView" OrElse e.CommandName = "R1" OrElse e.CommandName = "R2" Then
            DISoftURL.Value = Convert.ToString(ConfigurationManager.AppSettings.Item("DISoftURL").Trim())
            wstr = "vWorkSpaceId = '" + WorkSpaceId + "' and iNodeId = '" + PerentNodeId + "' and vSubjectId ='" + SubjectId + "'"

            If Not objHelp.GetData("View_GetSubjectStudyDetail", "*", wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsDicom, eStr_Retu) Then
                Throw New Exception("Error while getting status of Dicom..." + eStr_Retu)
            End If
            If dsDicom.Tables(0).Rows.Count > 0 Then
                Project = dsDicom.Tables(0).Rows(0)("vProjectNo").ToString()
                iModalityNo = dsDicom.Tables(0).Rows(0)("iModalityNo").ToString()
                iAnatomyNo = dsDicom.Tables(0).Rows(0)("iAnatomyNo").ToString()
                iImgTransmittalHdrId = dsDicom.Tables(0).Rows(0)("iImgTransmittalHdrId").ToString()
                iImageStatus = dsDicom.Tables(0).Rows(0)("iImageStatus").ToString()
                ScreeningNo = dsDicom.Tables(0).Rows(0)("vMySubjectNo").ToString()
                'ActivityName = dsDicom.Tables(0).Rows(0)("vNodeDisplayName").ToString()
            End If

            If e.CommandName = "AdjView" Then
                RDicom = "R1"
                querystring = DISoftURL.Value + "MIBizNETAdjudicatorResponse/MIBizNETAdjudicatorResponse?WId=" + WorkSpaceId + "&SId=" + SubjectId + "&PId=" + Project +
                          "&Uid=" + Me.Session(S_UserID) + "&MId=" + iModalityNo + "&AId=" + iAnatomyNo + "&VId=" + PerentNodeId + "&HDrId=" + iImgTransmittalHdrId +
                          "&DtlId=" + iImgTransmittalDtlId + "&iIS=" + iImageStatus + "&iMySubjectNo=" + MySubjectNo + "&ScreenNo=" + ScreeningNo +
                          "&ParentWorkSpaceId=" + ParentWorkSpaceId + "&PeriodId=" + PeriodId + "&ActivityName=" + ActivityName1 + "&SubActivityName=" + ActivityName +
                          "&subinodeID=" + ActivityName + "&parentActivityID=" + parentActivityID + "&AdjUserId=" + Me.Session(S_UserID) + "&UserTypeCode=" + Me.Session(S_UserType) +
                          "&WorkFlowStageId=" + Session(S_WorkFlowStageId) + "&RDicom=" + RDicom
            ElseIf e.CommandName = "R1" OrElse e.CommandName = "R2" Then

                If e.CommandName = "R1" Then
                    RDicom = "R1"
                Else
                    RDicom = "R2"
                End If

                querystring = DISoftURL.Value + "MIBizNETAdjudicatorResponse/MIBizNETAdjudicatorResponse?WId=" + WorkSpaceId + "&SId=" + SubjectId + "&PId=" + Project +
                          "&Uid=" + Me.Session(S_UserID) + "&MId=" + iModalityNo + "&AId=" + iAnatomyNo + "&VId=" + PerentNodeId + "&HDrId=" + iImgTransmittalHdrId +
                          "&DtlId=" + iImgTransmittalDtlId + "&iIS=" + iImageStatus +
                          "&iMySubjectNo=" + MySubjectNo + "&ScreenNo=" + ScreeningNo + "&ParentWorkSpaceId=" + ParentWorkSpaceId + "&PeriodId=" + PeriodId +
                          "&ActivityName=" + ActivityName + "&SubActivityName=" + SubActivityName + "&subinodeID=" + NodeId + "&parentActivityID=" + parentActivityID +
                          "&AdjUserId=" + Me.Session(S_UserID) + "&UserTypeCode=" + Me.Session(S_UserType) + "&RDicom=" + RDicom
            End If

            RedirectStr = "window.open(""" + querystring + """)"
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WindowOpen", RedirectStr, True)

        End If

        If gvAdjucatorReviewStatus.Rows.Count > 0 Then
            gvAdjucatorReviewStatus.Visible = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIVisitDatatable", "UIVisitDatatable();", True)
        End If





    End Sub

    'Added By Bhargav Thaker Start
    'Protected Sub gvCA_RowCommand(sender As Object, e As GridViewCommandEventArgs)
    '    Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
    '    Dim row As GridViewRow = gvCA.Rows(rowIndex)
    '    Dim WorkSpaceId As String = gvCA.DataKeys(rowIndex).Values(0).ToString()
    '    Dim SubjectId As String = gvCA.DataKeys(rowIndex).Values(1).ToString()
    '    Dim RedirectStr As String = String.Empty
    '    Dim querystring As String = String.Empty

    '    GetUserProfile(Me.Session(S_UserName), e.CommandName)

    '    querystring = "frmActivitySelectionforBunch.aspx?WorkSpaceId=" & WorkSpaceId & "&SubjectId=" & SubjectId
    '    RedirectStr = "window.open(""" + querystring + """)"
    '    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WindowOpen", RedirectStr, True)

    '    If gvCA.Rows.Count > 0 Then
    '        gvCA.Visible = True
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UICADatatable", "UICADatatable();", True)
    '    End If
    'End Sub
    'Added By Bhargav Thaker End

    Protected Sub btnSaveActivity_Click(sender As Object, e As EventArgs) Handles btnSaveActivity.Click

        For Each row As GridViewRow In gvAdjucatorReviewStatus.Rows
            Dim isSelected As Boolean = (TryCast(row.FindControl("chkright"), CheckBox)).Checked

            If isSelected Then
                'For activity = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                '    If tvActivity.Nodes(0).ChildNodes(activity).Checked = True Then

                '        Dim userTypeVal As String = String.Empty
                '        Dim arrayActivityval() As String
                '        arrayActivityval = tvActivity.Nodes(0).ChildNodes(activity).Value.Split("#")
                '        dr = dt_ProjectActivityOpertationMatrix.NewRow()
                '        dr("vWorkSpaceId") = Me.HProjectId.Value.ToString().Trim()
                '        dr("iPeriod") = iPeriod.ToString().Trim()
                '        dr("iParentNodeId") = arrayActivityval(0).ToString().Trim()
                '        dr("iNodeNo") = arrayActivityval(1).ToString().Trim()
                '        dr("iNodeId") = arrayActivityval(2).ToString().Trim()
                '        dr("vActivityId") = arrayActivityval(3).ToString().Trim()
                '        dr("vMySubjectNo") = tvSubject.Nodes(0).ChildNodes(subject).Value.ToString().Trim()
                '        dr("vUserTypeCode") = tvUserType.Nodes(0).ChildNodes(userType).Value.ToString().Trim()
                '        dr("iUserId") = tvUserType.Nodes(0).ChildNodes(userType).ChildNodes(subuser).Value.ToString().Trim()
                '        dr("cStatusIndi") = Status_New
                '        dr("iModifyBy") = Me.Session(S_UserID)
                '        dr("DATAOPMODE") = 1
                '        dt_ProjectActivityOpertationMatrix.Rows.Add(dr)

                '    End If
                'Next
            End If
        Next
    End Sub

    Protected Sub btnClear_Click(sender As Object, e As EventArgs)
        Me.txtprojectForDI.Text = ""
        Me.txtprojectForDI.SelectedIndex = 0 'Added By Bhargav Thaker
        Me.txtSite.Text = "" 'Added By Bhargav Thaker
        Me.HProjectId.Value = ""
        Me.txtScreeningForDI.Text = "" 'Added By Bhargav Thaker
        Me.TxtFromDate.Text = "" 'Added By Bhargav Thaker
        Me.TxtToDate.Text = "" 'Added By Bhargav Thaker
        gvVisitReviewStatus.DataSource = Nothing
        gvVisitReviewStatus.DataBind()
    End Sub

    <Web.Services.WebMethod()>
    Public Shared Function checkDicom(ByVal WorkSpaceId As String, ByVal NodeId As String, ByVal SubjectId As String, ByVal iMySubjectNo As String) As String
        Dim vWorkSpaceId As String = String.Empty
        Dim iNodeId As String = String.Empty
        Dim vSubjectId As String = String.Empty
        Dim iSubjectNo As String = String.Empty
        Dim wstr As String = String.Empty
        Dim estr_retu As String = String.Empty
        Dim dsDicom As New DataSet
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()

        Try
            Dim returnData As String = String.Empty
            vWorkSpaceId = WorkSpaceId
            iNodeId = NodeId
            vSubjectId = SubjectId
            iSubjectNo = iMySubjectNo
            'Dim DIServer As String = Convert.ToString(ConfigurationManager.AppSettings.Item("DIServer").Trim())

            wstr = "vWorkSpaceId = '" + vWorkSpaceId + "' and iNodeId = '" + iNodeId + "' and vSubjectId ='" + vSubjectId + "'"
            'If Not objHelp.GetData(DIServer + "View_GetSubjectStudyDetail", "*", wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsDicom, estr_retu) Then
            '    Throw New Exception("Error while getting status of Dicom..." + estr_retu)
            'End If

            If Not objHelp.GetData("View_GetSubjectStudyDetail", "*", wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsDicom, estr_retu) Then
                Throw New Exception("Error while getting status of Dicom..." + estr_retu)
            End If

            returnData = JsonConvert.SerializeObject(dsDicom)
            Return returnData

        Catch ex As Exception
            Return ex.ToString()
        Finally

        End Try

    End Function

    Public Function GetMenuDataDashboard(ByVal UserTypeCode As String, ByVal AdjucatorConstant As String) As DataSet
        Try
            Return objHelpDbTable.GetMenuDashboard(UserTypeCode, AdjucatorConstant)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

#Region "bind query details"
    <WebMethod>
    Public Shared Function FillQueryDetails() As String
        Dim ds_querydetails As New Data.DataSet
        Dim dv_subject As New Data.DataView
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Try
            ds_querydetails = objHelp.GetResultSet("select * from View_GetQueryMaster WHERE cStatusIndi <> 'D'", "QueryMaster ")

            If ds_querydetails Is Nothing OrElse ds_querydetails.Tables.Count <= 0 OrElse ds_querydetails.Tables(0).Rows.Count <= 0 Then
                Exit Function
            End If
            If ds_querydetails.Tables(0).Rows.Count > 0 Then
                'gvQueryMaster.DataSource = ds_querydetails.Tables(0)
                'gvQueryMaster.DataBind()
                Return JsonConvert.SerializeObject(ds_querydetails.Tables(0))
            End If

            'If gvQueryMaster.Rows.Count > 0 Then
            '    'gvQueryMaster.Visible = True
            '    ' ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIquerydetailstable", "UIquerydetailstable();", True)
            'End If

        Catch ex As Exception
            Throw New Exception(ex.Message + vbCrLf + "Error in query details bind.")
        End Try

    End Function
#End Region

    <WebMethod>
    Public Shared Function AuditTrail(ByVal nQueryNo As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_Certificate As New DataSet
        Dim estr As String = String.Empty

        Try
            wStr = " nQueryNo = '" + nQueryNo + " ' AND cStatusIndi <> 'D'"
            If Not objHelp.GetData("View_GetQueryMasterAudit", "*", wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Certificate, estr) Then
                Throw New Exception(estr)
            End If
            Return JsonConvert.SerializeObject(ds_Certificate.Tables(0))
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

#Region "Export Audit Record PDF"
    <WebMethod>
    Public Shared Function exportToPDF(ByVal nQueryNo As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_Certificate As New DataSet
        Dim estr As String = String.Empty
        Dim dtAuditTrail As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Dim gvAuditTrial As New GridView
        Dim filename As String = String.Empty
        Dim vTableName As String = String.Empty
        Dim strMessage As New StringBuilder
        Dim targetDir As String = String.Empty
        Dim strModified As String = ""
        Try
            wStr = " nQueryNo = '" + nQueryNo + " ' AND cStatusIndi <> 'D'"
            If Not objHelp.GetData("View_GetQueryMasterAudit", "*", wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Certificate, estr) Then
                Throw New Exception(estr)
            End If
            If Not dtAuditTrail Is Nothing Then
                dtAuditTrail.Columns.Add("ProjectNo")
                dtAuditTrail.Columns.Add("MySubjectNo")
                dtAuditTrail.Columns.Add("Visit")
                dtAuditTrail.Columns.Add("Remarks")
                dtAuditTrail.Columns.Add("Change On(Uploader Date)")
            End If

            dtAuditTrail.AcceptChanges()
            dt = ds_Certificate.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtAuditTrail.NewRow()
                drAuditTrail("ProjectNo") = dr("vProjectNo").ToString()
                drAuditTrail("MySubjectNo") = dr("vMySubjectNo").ToString()
                drAuditTrail("Visit") = dr("vNodeDisplayName").ToString()
                drAuditTrail("Remarks") = Convert.ToString(dr("vRemarks"))
                drAuditTrail("Change On(Uploader Date)") = dr("dModifyOn").ToString()
                dtAuditTrail.Rows.Add(drAuditTrail)
                dtAuditTrail.AcceptChanges()
                i += 1
            Next
            gvAuditTrial.DataSource = dtAuditTrail
            gvAuditTrial.DataBind()
            If gvAuditTrial.Rows.Count > 0 Then
                Dim gridviewHtml As String = String.Empty
                filename = "Audit Trail_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf"
                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                gvAuditTrial.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()
                HttpContext.Current.Response.ContentType = "application/pdf"
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + filename)
                Dim HeaderDiv As String
                HeaderDiv = "<div>" +
                            "    <table cellspacing='0' cellpadding='0' rules='all'  style='border-collapse:collapse;width:100%;'>" +
                            "		<tr>" +
                            "			<th scope='col' colspan='4' align='center'><strong><b><font color='#000099' size='4' face='Verdana, Arial, Helvetica, sans-serif'>QueryDetails Audit Report</font></strong></b></th>" +
                            "		</tr>" +
                            "		<tr>" +
                            "			<th scope='col' colspan='4' align='center'><strong><b><font color='#000099' size='3' face='Verdana, Arial, Helvetica, sans-serif'>" + System.Configuration.ConfigurationManager.AppSettings("Client") + "</font></strong></b></th>" +
                            "		</tr>" +
                              "		<tr>" +
                            "			<th align = 'right'><font color='#000099' size='2' face='Verdana'><b>Print Date:-</b></font></th><th align = 'left'><font color='#000099' size='2' face='Verdana'><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></th>" +
                            " <th align='Right' ><font color='#000099' size='2' face='Verdana'><b>Printed By:-</b></font></th><th align = 'left'><font color='#000099' size='2' face='Verdana'><b>" + HttpContext.Current.Session(S_UserNameWithProfile) + "</b></font></th>" +
                            "		</tr>" +
                            "	</table>" +
                            "</div>"
                Dim sr As New StringReader(HeaderDiv + stringWriter.ToString())
                Dim pdfDoc As New Document(iTextSharp.text.PageSize.A4.Rotate(), 30.0F, 30.0F, 50.0F, 0.0F)
                Dim htmlparser As New HTMLWorker(pdfDoc)
                Dim msOutput As New MemoryStream()
                Dim writer1 As PdfWriter = PdfWriter.GetInstance(pdfDoc, msOutput)
                pdfDoc.Open()
                htmlparser.Parse(sr)
                'htmlparser.Parse(New StringReader(gridviewHtml.ToString()))
                pdfDoc.Close()
                Dim filebytes As Byte() = msOutput.ToArray()
                strModified = Convert.ToBase64String(filebytes, 0, filebytes.Length)
            End If
            Return strModified
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return strModified
    End Function
#End Region

End Class