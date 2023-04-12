
Partial Class frmOldNewProjectSearch
    Inherits System.Web.UI.Page
#Region "Variable Declaration"
    Private ObjCommon As New clsCommon
    Private ObjHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()

    Private Const vProjectNo_New As Integer = 0
    Private Const vRequestId_New As Integer = 1
    Private Const vClientName_New As Integer = 2
    Private Const vDrugName_New As Integer = 3
    Private Const vBrandName_New As Integer = 4
    Private Const vProjectManager_New As Integer = 5
    Private Const vProjectCoordinator_New As Integer = 6
    Private Const iNoOfSubjects_New As Integer = 7
    Private Const nRetaintionPeriod_New As Integer = 8
    Private Const cFastingFed_New As Integer = 9
    Private Const vLocation_New As Integer = 10
    Private Const vRegionName_New As Integer = 11
    Private Const cProjectStatus_New As Integer = 12
    Private Const CheckIn1_New As Integer = 13
    Private Const IPAdministration1_New As Integer = 14
    Private Const CheckIn2_New As Integer = 15
    Private Const IPAdministration2_New As Integer = 16
    Private Const CheckIn3_New As Integer = 17
    Private Const IPAdministration3_New As Integer = 18
    Private Const CheckIn4_New As Integer = 19
    Private Const IPAdministration4_New As Integer = 20
    Private Const Initial As Integer = 21
    Private Const notrewarded As Integer = 22
    Private Const preclinical As Integer = 23
    Private Const study As Integer = 24
    Private Const analytical As Integer = 25
    Private Const document As Integer = 26
    Private Const compelited As Integer = 27
    Private Const terminated As Integer = 28
    Private Const archived As Integer = 29
    Private Const Details_New As Integer = 30
    Private Const vWorkSpaceId_New As Integer = 31

    Private Const vProjectNo_Old As Integer = 0
    Private Const vClientName_Old As Integer = 1
    Private Const vDrugName_Old As Integer = 2
    Private Const iNoOfSubjects_Old As Integer = 3
    Private Const Coordinator_Old As Integer = 4
    Private Const nRetaintionPeriod_Old As Integer = 5
    Private Const Location_Old As Integer = 6
    Private Const vRegionName_Old As Integer = 7
    Private Const cProjectStatus_Old As Integer = 8
    Private Const CheckIn1_Old As Integer = 9
    Private Const IPAdministration1_Old As Integer = 10
    Private Const CheckIn2_Old As Integer = 11
    Private Const IPAdministration2_Old As Integer = 12
    Private Const CheckIn3_Old As Integer = 13
    Private Const IPAdministration3_Old As Integer = 14
    Private Const CheckIn4_Old As Integer = 15
    Private Const IPAdministration4_Old As Integer = 16
    Private Const StudyResults_Old As Integer = 17

    Private estr As String

    Dim Ds_FillDropDown As New DataSet

#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CType(Me.Master.FindControl("lblHeading"), Label).Text = " Project Search "
        If Not IsPostBack Then
            If Not GenCall_ShowUI() Then
            End If
        End If
    End Sub
#End Region

#Region "Gencall show UI"
    Private Function GenCall_ShowUI() As Boolean
        Try
            Page.Title = ":: Project Search :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            If Not FillDropDowns() Then
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message.ToString(), "")
        End Try
    End Function
#End Region

#Region "FillDropDowns"
    Private Function FillDropDowns() As Boolean
        Dim Dt_ProjectType As New DataTable
        Dim Dt_ProjectStatus As New DataTable

        'Get Project type
        If Not ObjHelp.getprojectTypeMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, Ds_FillDropDown, estr) Then
            Me.ShowErrorMessage(estr, "")
            Exit Function
        End If
        Dt_ProjectType = Ds_FillDropDown.Tables("ProjectTypeMst")
        Me.DdlProjectType.DataSource = Dt_ProjectType
        Me.DdlProjectType.DataTextField = "vProjectTypeName"
        Me.DdlProjectType.DataValueField = "vProjectTypeCode"
        Me.DdlProjectType.DataBind()
        Me.DdlProjectType.Items.Insert(0, "All Project Type")

        'Get Project Status
        If Not ObjHelp.GetFieldsOfTable(" View_getWorkspaceDetailForHdr ", " distinct cProjectStatus,cProjectStatusDesc, Case cProjectStatus when 'I' then 0 when 'N' then 1 when 'P' then 2 when 's' then 3 when 'L' then 4 when 'D' then 5 when 'C' then 6 when 'T' then 7 when 'A' then 8 end AS StatusRank ", " cProjectStatus <> '' Order by StatusRank ", Ds_FillDropDown, estr) Then
            Me.ShowErrorMessage(estr, "")
            Exit Function
        End If
        Ds_FillDropDown.Tables(0).TableName = "View_getWorkspaceDetailForHdr"
        Dt_ProjectStatus = Ds_FillDropDown.Tables("View_getWorkspaceDetailForHdr")
        Me.DdlProjectStatus.DataSource = Dt_ProjectStatus
        Me.DdlProjectStatus.DataValueField = "cProjectStatus"
        Me.DdlProjectStatus.DataTextField = "cProjectStatusDesc"
        Me.DdlProjectStatus.DataBind()
        Me.DdlProjectStatus.Items.Insert(0, "All Status")
    End Function
#End Region

#Region "FillAdvDropDowns"
    Private Function FillAdvDropDowns() As Boolean
        Dim Dt_ProjectType As New DataTable
        Dim Dt_Sponser As New DataTable
        Dim Dt_Location As New DataTable
        Dim Dt_Study As New DataTable
        Dim Dt_Submission As New DataTable

        'Get drug dataset
        If Me.DDlDrugs.Items.Count <= 0 Then
            If Not ObjHelp.getdrugmst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, Ds_FillDropDown, estr) Then
                Me.ShowErrorMessage(estr, "")
                Exit Function
            End If
            Dt_ProjectType = Ds_FillDropDown.Tables("DRUGMST")
            Me.DDlDrugs.DataSource = Dt_ProjectType
            Me.DDlDrugs.DataValueField = "vDrugCode"
            Me.DDlDrugs.DataTextField = "vDrugName"
            Me.DDlDrugs.DataBind()
            Me.DDlDrugs.Items.Insert(0, "All Drugs")
        End If
        'Adding tooltip to drug
        For CountDrugs As Integer = 0 To Me.DDlDrugs.Items.Count - 1
            Me.DDlDrugs.Items(CountDrugs).Attributes.Add("Title", Me.DDlDrugs.Items(CountDrugs).Text.ToString())
        Next CountDrugs

        'Get Sponser
        If Me.DDlSponser.Items.Count <= 0 Then
            If Not ObjHelp.getclientmst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, Ds_FillDropDown, estr) Then
                Me.ShowErrorMessage(estr, "")
                Exit Function
            End If
            Dt_Sponser = Ds_FillDropDown.Tables("CLIENTMST")
            Me.DDlSponser.DataSource = Dt_Sponser
            Me.DDlSponser.DataValueField = "vClientCode"
            Me.DDlSponser.DataTextField = "vClientName"
            Me.DDlSponser.DataBind()
            Me.DDlSponser.Items.Insert(0, "All Sponsers")
        End If
        'Adding sponser tooltip
        For CountSponser As Integer = 0 To Me.DDlSponser.Items.Count - 1
            Me.DDlSponser.Items(CountSponser).Attributes.Add("Title", Me.DDlSponser.Items(CountSponser).Text.ToString())
        Next CountSponser

        'Get Location
        If Me.DdlLocation.Items.Count <= 0 Then
            If Not ObjHelp.getLocationMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, Ds_FillDropDown, estr) Then
                Me.ShowErrorMessage(estr, "")
                Exit Function
            End If
            Dt_Location = Ds_FillDropDown.Tables("LocationMst")
            Me.DdlLocation.DataSource = Dt_Location
            Me.DdlLocation.DataValueField = "vLocationCode"
            Me.DdlLocation.DataTextField = "vLocationName"
            Me.DdlLocation.DataBind()
            Me.DdlLocation.Items.Insert(0, "All Locations/Sites")
        End If

        'Get Submission
        If Me.DdlSubmission.Items.Count <= 0 Then
            If Not ObjHelp.getregionmst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, Ds_FillDropDown, estr) Then
                Me.ShowErrorMessage(estr, "")
                Exit Function
            End If
            Dt_Submission = Ds_FillDropDown.Tables("RegionMst")
            Me.DdlSubmission.DataSource = Dt_Submission
            Me.DdlSubmission.DataValueField = "vRegionCode"
            Me.DdlSubmission.DataTextField = "vRegionName"
            Me.DdlSubmission.DataBind()
            Me.DdlSubmission.Items.Insert(0, "All Submissions")
        End If

        'Get Study Type
        If Me.DdlStudyType.Items.Count <= 0 Then
            If Not ObjHelp.getWorkTypeMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, Ds_FillDropDown, estr) Then
                Me.ShowErrorMessage(estr, "")
                Exit Function
            End If
            Dt_Study = Ds_FillDropDown.Tables("WorkTypeMst")
            Me.DdlStudyType.DataSource = Dt_Study
            Me.DdlStudyType.DataValueField = "nWorkTypeNo"
            Me.DdlStudyType.DataTextField = "vWorkTypeDesc"
            Me.DdlStudyType.DataBind()
            Me.DdlStudyType.Items.Insert(0, "All Study")
        End If

    End Function
#End Region

#Region "FillGrid"
    Private Function FillGrid() As Boolean
        Dim WstrCommon As String = ""
        Dim WstrForNewProjects As String = ""
        Dim Ds As DataSet = Nothing
        Dim Dt_Old As DataTable
        Dim Dt_New As DataTable
        'Wstr = " 1=1 And "
        If Me.DdlProjectType.SelectedIndex > 0 Then
            WstrForNewProjects += " vProjectTypeCode = '" & Me.DdlProjectType.SelectedValue.ToString() & "' And "
        End If
        If Me.DdlProjectStatus.SelectedIndex > 0 Then
            WstrCommon += " cProjectStatus = '" & Me.DdlProjectStatus.SelectedValue.ToString() & "' And "
        End If
        If Me.ChkAllDate.Checked = False Then
            If Me.DdlProjectStatus.SelectedValue = "I" Then
                WstrForNewProjects += " InitialQuotationStart Between '" & Me.txtFrmDate.Text.ToString() & "' And '" & Me.txtToDate.Text.ToString() & "' And "
            ElseIf Me.DdlProjectStatus.SelectedValue = "N" Then
                WstrForNewProjects += " NotRewardedStart Between '" & Me.txtFrmDate.Text.ToString() & "' And '" & Me.txtToDate.Text.ToString() & "' And "
            ElseIf Me.DdlProjectStatus.SelectedValue = "P" Then
                WstrForNewProjects += " PreClinicalStart Between '" & Me.txtFrmDate.Text.ToString() & "' And '" & Me.txtToDate.Text.ToString() & "' And "
            ElseIf Me.DdlProjectStatus.SelectedValue = "S" Then
                WstrForNewProjects += " StudyStart Between '" & Me.txtFrmDate.Text.ToString() & "' And '" & Me.txtToDate.Text.ToString() & "' And "
            ElseIf Me.DdlProjectStatus.SelectedValue = "L" Then
                WstrForNewProjects += " AnalyticalStart Between '" & Me.txtFrmDate.Text.ToString() & "' And '" & Me.txtToDate.Text.ToString() & "' And "
            ElseIf Me.DdlProjectStatus.SelectedValue = "D" Then
                WstrForNewProjects += " DocumentStart Between '" & Me.txtFrmDate.Text.ToString() & "' And '" & Me.txtToDate.Text.ToString() & "' And "
            ElseIf Me.DdlProjectStatus.SelectedValue = "C" Then
                WstrForNewProjects += " ComplitedStart Between '" & Me.txtFrmDate.Text.ToString() & "' And '" & Me.txtToDate.Text.ToString() & "' And "
            ElseIf Me.DdlProjectStatus.SelectedValue = "A" Then
                WstrForNewProjects += " ArchivedDate Between '" & Me.txtFrmDate.Text.ToString() & "' And '" & Me.txtToDate.Text.ToString() & "' And "
            ElseIf Me.DdlProjectStatus.SelectedValue = "T" Then
                WstrForNewProjects += " TerminatedDate Between '" & Me.txtFrmDate.Text.ToString() & "' And '" & Me.txtToDate.Text.ToString() & "' And "
            End If
        End If
        If Me.DDlSponser.SelectedIndex > 0 Then
            WstrCommon += " (vClientCode = '" & Me.DDlSponser.SelectedValue.ToString() & "' Or vClientName Like '%" & Me.DDlSponser.SelectedItem.Text.ToString() & "%') And "
        End If
        If Me.DDlDrugs.SelectedIndex > 0 Then
            WstrCommon += " (vDrugCode = '" & Me.DDlDrugs.SelectedValue.ToString() & "' Or vDrugName Like '%" & Me.DDlDrugs.SelectedItem.Text.ToString() & "%') And "
        End If
        If Me.DdlLocation.SelectedIndex > 0 Then
            WstrCommon += " (vLocationCode = '" & Me.DdlLocation.SelectedValue.ToString() & "' Or vLocationName Like '%" & Me.DdlLocation.SelectedItem.Text.ToString() & "%') And "
        End If
        If Me.DdlStudyType.SelectedIndex > 0 Then
            WstrForNewProjects += " nWorkTypeNo = " & Me.DdlStudyType.SelectedValue.ToString() & " And "
        End If
        If Me.DdlSubmission.SelectedIndex > 0 Then
            WstrCommon += " vRegionName = '" & Me.DDlSponser.SelectedItem.Text.ToString() & "' And "
            WstrForNewProjects += " vRegionCode = '" & Me.DDlSponser.SelectedValue.ToString() & "' And "
        End If

        WstrForNewProjects += " vProjectTypeCode In (" & Me.Session(S_ScopeValue) & ") "
        WstrCommon += " 1=1"

        'wstrfornewprojects = replace(wstrfornewprojects, "'", "''")
        'wstrcommon = replace(wstrcommon, "'", "''")

        WstrForNewProjects = Replace(WstrCommon, "'", "''") + " And " + Replace(WstrForNewProjects, "'", "''")

        If Not ObjHelp.GetOldNewProjects(WstrCommon, WstrForNewProjects, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds, estr) Then
            Me.ShowErrorMessage("Problem while getting projects", estr)
            Exit Function
        End If

        Dt_New = Ds.Tables(0)
        GvNewProjects.DataSource = Dt_New
        GvNewProjects.DataBind()

        Dt_Old = Ds.Tables("View_OldProjects")
        GvOldProjects.DataSource = Dt_Old
        GvOldProjects.DataBind()
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "DisplayPanels", "DisplayPanels();", True)


        CType(Master.FindControl("lblerrormsg"), Label).Text = ""

    End Function
#End Region

#Region "Button Events"

    Protected Sub BtnPnlSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPnlSearch.Click
        If Not FillAdvDropDowns() Then
        End If
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "ShowDiv('" & Me.DvAdvSearch.ClientID & "','imgAdvSearch','" & Me.BtnPnlSearch.ClientID & "');", True)


    End Sub

    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click, BtnSearchProject.Click
        FillGrid()
    End Sub

#End Region

#Region "Selected index change"
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

#Region "Grid events"

#Region " New project Grid events"

    Protected Sub GvNewProjects_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvNewProjects.PageIndexChanging
        GvNewProjects.PageIndex = e.NewPageIndex
        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub

    Protected Sub GvNewProjects_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GvNewProjects.RowCommand
        'Dim Index As Integer = e.CommandArgument
        If e.CommandName.ToUpper = "DETAIL" Then
            Me.Response.Redirect("frmProjectDetailMst.aspx?WorkSpaceId=" & e.CommandArgument & _
                                  "&Page=frmOldNewProjectSearch&Type=ALL") 'GvNewProjects.Rows(Index).Cells(vRequestId_New).Text.Trim() & _
        End If
    End Sub

    Protected Sub GvNewProjects_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvNewProjects.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(vWorkSpaceId_New).Visible = False
            For FieldCount As Integer = 0 To Me.ChkListFields.Items.Count() - 1
                If Me.ChkListFields.Items(FieldCount).Selected = False Then
                    If Me.ChkListFields.Items(FieldCount).Value = "vProjectNo" Then
                        e.Row.Cells(vProjectNo_New).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "vRequestId" Then
                        e.Row.Cells(vRequestId_New).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "vClientName" Then
                        e.Row.Cells(vClientName_New).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "vDrugName" Then
                        e.Row.Cells(vDrugName_New).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "vBrandName" Then
                        e.Row.Cells(vBrandName_New).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "vProjectManager" Then
                        e.Row.Cells(vProjectManager_New).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "iNoOfSubjects" Then
                        e.Row.Cells(iNoOfSubjects_New).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "nRetaintionPeriod" Then
                        e.Row.Cells(nRetaintionPeriod_New).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "cFastingFed" Then
                        e.Row.Cells(cFastingFed_New).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "vRegionName" Then
                        e.Row.Cells(vRegionName_New).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "cProjectStatus" Then
                        e.Row.Cells(cProjectStatus_New).Visible = False
                        If Me.DdlProjectStatus.SelectedValue = "I" Then
                            e.Row.Cells(notrewarded).Visible = False
                            e.Row.Cells(preclinical).Visible = False
                            e.Row.Cells(study).Visible = False
                            e.Row.Cells(analytical).Visible = False
                            e.Row.Cells(document).Visible = False
                            e.Row.Cells(compelited).Visible = False
                            e.Row.Cells(terminated).Visible = False
                            e.Row.Cells(archived).Visible = False
                        ElseIf Me.DdlProjectStatus.SelectedValue = "N" Then
                            e.Row.Cells(Initial).Visible = False
                            e.Row.Cells(preclinical).Visible = False
                            e.Row.Cells(study).Visible = False
                            e.Row.Cells(analytical).Visible = False
                            e.Row.Cells(document).Visible = False
                            e.Row.Cells(compelited).Visible = False
                            e.Row.Cells(terminated).Visible = False
                            e.Row.Cells(archived).Visible = False
                        ElseIf Me.DdlProjectStatus.SelectedValue = "P" Then
                            e.Row.Cells(Initial).Visible = False
                            e.Row.Cells(notrewarded).Visible = False
                            e.Row.Cells(study).Visible = False
                            e.Row.Cells(analytical).Visible = False
                            e.Row.Cells(document).Visible = False
                            e.Row.Cells(compelited).Visible = False
                            e.Row.Cells(terminated).Visible = False
                            e.Row.Cells(archived).Visible = False
                        ElseIf Me.DdlProjectStatus.SelectedValue = "S" Then
                            e.Row.Cells(Initial).Visible = False
                            e.Row.Cells(notrewarded).Visible = False
                            e.Row.Cells(preclinical).Visible = False
                            e.Row.Cells(analytical).Visible = False
                            e.Row.Cells(document).Visible = False
                            e.Row.Cells(compelited).Visible = False
                            e.Row.Cells(terminated).Visible = False
                            e.Row.Cells(archived).Visible = False
                        ElseIf Me.DdlProjectStatus.SelectedValue = "L" Then
                            e.Row.Cells(Initial).Visible = False
                            e.Row.Cells(notrewarded).Visible = False
                            e.Row.Cells(preclinical).Visible = False
                            e.Row.Cells(study).Visible = False
                            e.Row.Cells(document).Visible = False
                            e.Row.Cells(compelited).Visible = False
                            e.Row.Cells(terminated).Visible = False
                            e.Row.Cells(archived).Visible = False
                        ElseIf Me.DdlProjectStatus.SelectedValue = "D" Then
                            e.Row.Cells(Initial).Visible = False
                            e.Row.Cells(notrewarded).Visible = False
                            e.Row.Cells(preclinical).Visible = False
                            e.Row.Cells(study).Visible = False
                            e.Row.Cells(analytical).Visible = False
                            e.Row.Cells(compelited).Visible = False
                            e.Row.Cells(terminated).Visible = False
                            e.Row.Cells(archived).Visible = False
                        ElseIf Me.DdlProjectStatus.SelectedValue = "C" Then
                            e.Row.Cells(Initial).Visible = False
                            e.Row.Cells(notrewarded).Visible = False
                            e.Row.Cells(preclinical).Visible = False
                            e.Row.Cells(study).Visible = False
                            e.Row.Cells(analytical).Visible = False
                            e.Row.Cells(document).Visible = False
                            e.Row.Cells(terminated).Visible = False
                            e.Row.Cells(archived).Visible = False
                        ElseIf Me.DdlProjectStatus.SelectedValue = "A" Then
                            e.Row.Cells(Initial).Visible = False
                            e.Row.Cells(notrewarded).Visible = False
                            e.Row.Cells(preclinical).Visible = False
                            e.Row.Cells(study).Visible = False
                            e.Row.Cells(analytical).Visible = False
                            e.Row.Cells(document).Visible = False
                            e.Row.Cells(compelited).Visible = False
                            e.Row.Cells(terminated).Visible = False
                        ElseIf Me.DdlProjectStatus.SelectedValue = "T" Then
                            e.Row.Cells(Initial).Visible = False
                            e.Row.Cells(notrewarded).Visible = False
                            e.Row.Cells(preclinical).Visible = False
                            e.Row.Cells(study).Visible = False
                            e.Row.Cells(analytical).Visible = False
                            e.Row.Cells(document).Visible = False
                            e.Row.Cells(compelited).Visible = False
                            e.Row.Cells(archived).Visible = False
                        Else
                            e.Row.Cells(Initial).Visible = False
                            e.Row.Cells(notrewarded).Visible = False
                            e.Row.Cells(preclinical).Visible = False
                            e.Row.Cells(study).Visible = False
                            e.Row.Cells(analytical).Visible = False
                            e.Row.Cells(document).Visible = False
                            e.Row.Cells(compelited).Visible = False
                            e.Row.Cells(archived).Visible = False
                            e.Row.Cells(terminated).Visible = False
                        End If
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "CheckIn1" Then
                        e.Row.Cells(CheckIn1_New).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "dosing1" Then
                        e.Row.Cells(IPAdministration1_New).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "CheckIn2" Then
                        e.Row.Cells(CheckIn2_New).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "Dosing2" Then
                        e.Row.Cells(IPAdministration2_New).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "checkin3" Then
                        e.Row.Cells(CheckIn3_New).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "dosing3" Then
                        e.Row.Cells(IPAdministration3_New).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "Details" Then
                        e.Row.Cells(Details_New).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "Location" Then
                        e.Row.Cells(vLocation_New).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "coordinator" Then
                        e.Row.Cells(vProjectCoordinator_New).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "dosing4" Then
                        e.Row.Cells(IPAdministration4_New).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "checkin4" Then
                        e.Row.Cells(CheckIn4_New).Visible = False
                    End If
                End If
            Next FieldCount
        End If
    End Sub

    Protected Sub GvNewProjects_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvNewProjects.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("LkbDetail"), LinkButton).CommandArgument = e.Row.Cells(vWorkSpaceId_New).Text.ToString()
        End If
    End Sub

#End Region

#Region " New project Grid events"

    Protected Sub GvOldProjects_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvOldProjects.PageIndexChanging
        GvOldProjects.PageIndex = e.NewPageIndex
        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub

    Protected Sub GvOldProjects_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvOldProjects.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Or e.Row.RowType = DataControlRowType.Header Then
            For FieldCount As Integer = 0 To Me.ChkListFields.Items.Count - 1
                If Me.ChkListFields.Items(FieldCount).Selected = False Then
                    If Me.ChkListFields.Items(FieldCount).Value = "vProjectNo" Then
                        e.Row.Cells(vProjectNo_Old).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "vClientName" Then
                        e.Row.Cells(vClientName_Old).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "vDrugName" Then
                        e.Row.Cells(vDrugName_Old).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "iNoOfSubjects" Then
                        e.Row.Cells(iNoOfSubjects_Old).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "nRetaintionPeriod" Then
                        e.Row.Cells(nRetaintionPeriod_Old).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "vRegionName" Then
                        e.Row.Cells(vRegionName_Old).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "dosing1" Then
                        e.Row.Cells(CheckIn1_Old).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "CheckIn1" Then
                        e.Row.Cells(IPAdministration1_Old).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "CheckIn2" Then
                        e.Row.Cells(CheckIn2_Old).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "Dosing2" Then
                        e.Row.Cells(IPAdministration2_Old).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "checkin3" Then
                        e.Row.Cells(CheckIn3_Old).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "dosing3" Then
                        e.Row.Cells(IPAdministration3_Old).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "StudyResult" Then
                        e.Row.Cells(StudyResults_Old).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "cProjectStatus" Then
                        e.Row.Cells(cProjectStatus_Old).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "Location" Then
                        e.Row.Cells(Location_Old).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "coordinator" Then
                        e.Row.Cells(Coordinator_Old).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "dosing4" Then
                        e.Row.Cells(IPAdministration4_Old).Visible = False
                    ElseIf Me.ChkListFields.Items(FieldCount).Value = "checkin4" Then
                        e.Row.Cells(CheckIn4_Old).Visible = False
                    End If
                End If
            Next FieldCount
        End If
    End Sub

#End Region
#End Region

End Class
