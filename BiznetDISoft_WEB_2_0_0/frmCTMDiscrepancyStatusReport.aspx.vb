Imports System.Linq

Partial Class frmCTMDiscrepancyStatusReport
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objHelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

    Private Const VS_DtGVWDCF As String = "GVWDCF"
    Private Const vs_gLock As String = "glock"
    Private Const VS_choice As String = "Choice"

    Private Const VS_Rereview As String = "VS_Rereview"
    Private Const VS_DCF As String = "VS_DCF"
    Private Const VS_DCFExporttoExcel As String = "VS_DCFExporttoExcel"
    Private Const VS_RereviewExporttoExcel As String = "VS_RereviewExporttoExcel"

    Private rPage As RepoPage
    'Const declaration for GVWDSR (Discrepancy Status Report)

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_nDCFNo As Integer = 1
    Private Const GVC_Site As Integer = 2
    Private Const GVC_Visit As Integer = 3
    Private Const GVC_Activity As Integer = 4
    Private Const GVC_Period As Integer = 5
    Private Const GVC_SubjectId As Integer = 6
    Private Const GVC_SubjectName As Integer = 7
    Private Const GVC_ScreenNo As Integer = 8
    Private Const GVC_RandomizationNo As Integer = 9
    Private Const GVC_Repeatation As Integer = 10
    Private Const GVC_Attribute As Integer = 11
    Private Const GVC_DiscrepancyValue As Integer = 12
    Private Const GVC_DCFType As Integer = 13
    Private Const GVC_SourceResponse As Integer = 14
    Private Const GVC_UpdatedValue As Integer = 15
    Private Const GVC_UpdationRemarks As Integer = 16
    Private Const GVC_UpdateRemarks As Integer = 17
    Private Const GVC_CreatedBy As Integer = 18
    Private Const GVC_CreatedDate As Integer = 19
    Private Const GVC_Status As Integer = 21
    Private Const GVC_UpdatedBy As Integer = 21
    Private Const GVC_UpdatedOn As Integer = 22
    Private Const GVC_DataEntryBy As Integer = 23
    Private Const GVC_DataEntryOn As Integer = 24
    Private Const GVC_Edit As Integer = 25
    Private Const GVC_ActivityId As Integer = 26
    Private Const GVC_NodeId As Integer = 27
    Private Const GVC_iMySubjectNo As Integer = 28
    Private Const GVC_Resolve As Integer = 29
    Private Const GVC_ReReview As Integer = 30
    Private Const GVC_vUserTypeCode As Integer = 31
    Private Const GVC_cDCFStatus As Integer = 32
    Private Const GVC_cDCFType As Integer = 33
    Private Const GVC_iWorkFlowStageId As Integer = 34
    Private Const GVC_nCRFDtlNo As Integer = 35


    'Const Declaration for GVWODMSR (ODM Status Report) (Added By Anuj for ODM Status Report on 14-05-2013)

    'Rahul RUpareliya

    Private Const GVC2_SrNo As Integer = 0
    Private Const GVC2_Activity As Integer = 2
    Private Const GVC2_Parent As Integer = 1
    Private Const GVC2_ScreenNo As Integer = 3
    Private Const GVC2_RandomizationNo As Integer = 4
    Private Const GVC2_SubjectId As Integer = 5
    Private Const GVC2_iMySubjectNo As Integer = 6
    Private Const GVC2_Repeatation As Integer = 7
    Private Const GVC2_Attribute As Integer = 8
    Private Const GVC2_Value As Integer = 9
    Private Const GVC2_UpdatedValue As Integer = 10
    Private Const GVC2_UpdationRemarks As Integer = 11
    Private Const GVC2_UpdateRemarks As Integer = 12
    Private Const GVC2_UpdatedBy As Integer = 13
    Private Const GVC2_UpdatedOn As Integer = 14
    Private Const GVC2_DataEntryBy As Integer = 15
    Private Const GVC2_DataEntryOn As Integer = 16
    Private Const GVC2_Period As Integer = 17
    Private Const GVC2_Edit As Integer = 18
    Private Const GVC2_ActivityId As Integer = 19
    Private Const GVC2_NodeId As Integer = 20
    Private Const GVC2_vProjectTypeCode As Integer = 21


    'Const Declaration for gvRereview (Rereview Report) (Added By Parth for Rereview Report on 26-Dec-2014)
    Private Const GVC3_SrNo As Integer = 0
    Private Const GVC3_Project As Integer = 1
    Private Const GVC3_SubjectId As Integer = 2
    Private Const GVC3_ScreenNo As Integer = 3
    Private Const GVC3_RandomizationNo As Integer = 4
    Private Const GVC3_Period As Integer = 5
    Private Const GVC3_Visit As Integer = 6
    Private Const GVC3_RepetitionNo As Integer = 7
    Private Const GVC3_Activity As Integer = 8
    Private Const GVC3_PendingRereviewLevel As Integer = 9
    Private Const GVC3_Edit As Integer = 10
    Private Const GVC3_WorkFloStageId As Integer = 11
    Private Const GVC3_ActivityId As Integer = 12
    Private Const GVC3_NodeId As Integer = 13
    Private Const GVC3_iMySubjectNo As Integer = 14
    Private Const GVC3_nCRFDtlNo As Integer = 15
    '' Private Const GVC3_UpdateRemarks As Integer = 16

    'Const declaration for gvwDCF (Discrepancy Tracking Report)

    Private Const GVC4_SrNo As Integer = 0
    Private Const GVC4_Visit As Integer = 1
    Private Const GVC4_Activity As Integer = 2
    Private Const GVC4_Period As Integer = 3
    Private Const GVC4_SubjectId As Integer = 4
    Private Const GVC4_SubjectNo As Integer = 5
    Private Const GVC4_Repeatation As Integer = 6
    Private Const GVC4_Attribute As Integer = 7
    Private Const GVC4_DCFType As Integer = 8
    Private Const GVC4_Generatedby As Integer = 9
    Private Const GVC4_GeneratedDate As Integer = 10
    Private Const GVC4_Status As Integer = 11
    Private Const GVC4_AnsweredBy As Integer = 12
    Private Const GVC4_AnsweredOn As Integer = 13
    Private Const GVC4_ResolvedBy As Integer = 14
    Private Const GVC4_ResolvedOn As Integer = 15
    Private Const GVC4_GeneratedtoAnswered As Integer = 16
    Private Const GVC4_AnsweredtoResolved As Integer = 17
    Private Const GVC4_GeneratedtoResolved As Integer = 18
    Private Const GVC4_ActivityId As Integer = 19
    Private Const GVC4_NodeId As Integer = 20
    Private Const GVC4_cDCFType As Integer = 21
    Private Const GVC4_UpdaterReamarks As Integer = 22
    'Private Const GVC4_SrNo As Integer = 0
    'Private Const GVC4_nDCFNo As Integer = 1
    'Private Const GVC4_Site As Integer = 2
    'Private Const GVC4_Visit As Integer = 3
    'Private Const GVC4_Activity As Integer = 4
    'Private Const GVC4_Period As Integer = 5
    'Private Const GVC4_SubjectId As Integer = 6
    'Private Const GVC4_SubjectName As Integer = 7
    'Private Const GVC4_ScreenNo As Integer = 8
    'Private Const GVC4_RandomizationNo As Integer = 9
    'Private Const GVC4_Repeatation As Integer = 10
    'Private Const GVC4_Attribute As Integer = 11
    'Private Const GVC4_DiscrepancyValue As Integer = 12
    'Private Const GVC4_DCFType As Integer = 13
    'Private Const GVC4_SourceResponse As Integer = 14
    'Private Const GVC4_UpdatedValue As Integer = 15
    'Private Const GVC4_UpdationRemarks As Integer = 16
    'Private Const GVC4_CreatedBy As Integer = 17
    'Private Const GVC4_CreatedDate As Integer = 18
    'Private Const GVC4_Status As Integer = 19
    'Private Const GVC4_UpdatedBy As Integer = 20
    'Private Const GVC4_UpdatedOn As Integer = 21
    'Private Const GVC4_DataEntryBy As Integer = 22
    'Private Const GVC4_DataEntryOn As Integer = 23
    'Private Const GVC4_Edit As Integer = 24
    'Private Const GVC4_ActivityId As Integer = 24
    'Private Const GVC4_NodeId As Integer = 25
    'Private Const GVC4_iMySubjectNo As Integer = 26
    'Private Const GVC4_Resolve As Integer = 28
    'Private Const GVC4_ReReview As Integer = 29
    'Private Const GVC4_vUserTypeCode As Integer = 27
    'Private Const GVC4_cDCFStatus As Integer = 28
    'Private Const GVC4_cDCFType As Integer = 29
    'Private Const GVC4_iWorkFlowStageId As Integer = 30
    'Private Const GVC4_nCRFDtlNo As Integer = 31
    Private Const Vs_dsReviewerlevel As String = "vs_ds_dcfreviewer"

    ' added by prayag
    Private Const All_Activity As Integer = 0
    Private Const Sub_Specific_Activity As Integer = 1
    Private Const Generic_Activity As Integer = 2


#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try
            'add by shivani pandya for latest repeatition
            Me.Session(S_SelectedRepeatation) = ""

            If Not Me.IsPostBack() Then

                If Me.Request.QueryString("mode") = "1" Then
                    Page.Title = " :: ODM Status Report ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
                    CType(Master.FindControl("lblHeading"), Label).Text = "ODM Status Report"
                    Me.ddlStatus.Visible = "False"
                    Me.ddlDiscrepancyType.Visible = "False"
                    Me.lblStatus.Visible = "False"
                    Me.lblType.Visible = "False"
                    Me.chkRereview.Visible = False
                    Me.chkDCF.Visible = False
                    Me.trresolved.Visible = False
                Else
                    Page.Title = " :: Discrepancy Management Report ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
                    CType(Master.FindControl("lblHeading"), Label).Text = "Discrepancy Management Report"
                End If

                Me.btnExportToExcel.Visible = False
                Me.btnReReview.Visible = False

                Me.lblSignername.Text = Me.Session(S_FirstName).ToString() + " " + Me.Session(S_LastName).ToString()
                Dim dt_Profiles As New DataTable
                dt_Profiles = CType(Me.Session(S_Profiles), DataTable)
                Dim dv_Profiles As DataView
                dv_Profiles = dt_Profiles.DefaultView
                dv_Profiles.RowFilter = "vUserTypeCode = '" + Me.Session(S_UserType).ToString() + "'"
                Me.lblSignerDesignation.Text = dv_Profiles.ToTable.Rows(0)("vUserTypeName").ToString()
                'Me.lblSignDateTime.Text = Me.objHelp.GetServerDateTime().ToString("dd-MMM-yyyy hh:mm:ss")
                Me.lblSignRemarks.Text = "I attest to the accuracy and integrity of the data being reviewed."

                Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
                If (Not Me.Request.QueryString("mode") Is Nothing) Then
                    Choice = Me.Request.QueryString("mode").ToString
                    Me.ViewState(VS_choice) = Choice
                End If

                If (Not Me.Request.QueryString("TrnType") Is Nothing) Then
                    Select Case Me.Request.QueryString("TrnType")
                        Case "N"
                            Page.Title = " :: Generated Discrepancy Report ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
                            CType(Master.FindControl("lblHeading"), Label).Text = "Generated Discrepancy Report"

                        Case "O"
                            Page.Title = " :: Answered Discrepancy Report ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
                            CType(Master.FindControl("lblHeading"), Label).Text = "Answered Discrepancy Report"
                        Case "R"
                            Page.Title = " :: Resolved Discrepancy Report ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
                            CType(Master.FindControl("lblHeading"), Label).Text = "Resolved Discrepancy Report"
                    End Select

                    Me.chkRereview.Visible = False

                    '' comment by prayag
                    'Me.trselectSubject.Style("display") = "none"
                    'Me.trSubject.Style("display") = "none"
                    'Me.tblSecond.Style("display") = "none"
                    'Me.btnGo.Style("display") = ""
                    '' up to here 

                    'Me.trselectSubject.Style("display") = "none"
                    Me.tvSubject.Style("display") = "none"
                    Me.tblSecond.Style("display") = "none"
                    Me.btnGo.Style("display") = ""


                    'If Not Me.Request.QueryString("TrnType").Contains("N,O,R") Then
                    '    objCommon.ShowAlert("This is not a valid URL.", Me)
                    '    Exit Sub
                    'End If
                End If

                If (Me.ViewState(VS_choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View) Then
                    If Not Me.Request.QueryString("vWorkSpaceId") Is Nothing Then

                        Me.txtproject.Text = Me.Request.QueryString("ProjectNo").ToString()
                        Me.txtproject.Enabled = False
                        Me.HProjectId.Value = Me.Request.QueryString("vWorkSpaceId").ToString()
                        Me.btnCancel.Visible = False
                        btnSetProject_Click(sender, e)
                        HideMenu()
                        Exit Sub
                    End If
                End If

                If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                    Me.txtproject.Text = Session(S_ProjectName)
                    Me.HProjectId.Value = Session(S_ProjectId)
                    Me.btnSetProject_Click(sender, e)
                    Exit Sub
                End If
                If (Request.QueryString("ProjectName") <> "" And chkRereview.Checked = False) Then
                    Try
                        'If (Request.QueryString("ProjectName") <> "" And Request.QueryString("SubjectId") = "") Then
                        '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SelectALLS", "SelectAllSubjects();", True)
                        'End If
                        Me.txtproject.Text = Request.QueryString("ProjectName").ToString().Trim
                        Me.HProjectId.Value = Request.QueryString("WorkSpaceId").ToString().Trim
                        btnSetProject_Click(Nothing, Nothing)
                        'If Not Fillsubjects() Then
                        '    'Me.ShowErrorMessage("Error While Page_Load. ", "")
                        'End If
                        DdlPeriod.SelectedValue = Request.QueryString("Period")
                        If Not FillGrid() Then
                            'Me.ShowErrorMessage("Error While Page_Load. ", "")
                        End If


                    Catch ex As Exception
                    Finally
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Ref", "ChangeUrl('Discrepancy and Re-review Status Report','frmCTMDiscrepancyStatusReport.aspx');", True)
                    End Try
                End If
            End If

            If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial And Me.Request.QueryString("TrnType") Is Nothing Then
                Me.DdlPeriod.Enabled = True
            End If

            If Me.Request.QueryString("TrnType") Is Nothing And chkRereview.Checked = False Then '' added by prayag
                Me.trPeriodnew.Visible = True
            End If

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Page_Load. ", ex.Message)
        End Try
    End Sub

#End Region

#Region "Fill functions"

    Private Function FillGrid() As Boolean
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds As New DataSet
        Dim flag_Selected As Boolean = False
        Dim ds_Grid As New Data.DataSet
        Dim workspaceid As String = String.Empty
        Dim cIsChkAll As Char = "Y"
        Dim dFromDate As String = String.Empty
        Dim dToDate As String = Today.Date.AddDays(1).ToString("dd-MMM-yyyy")
        Dim cStatus As String = String.Empty
        'Dim vSubjectId As String = String.Empty
        Dim cDCFType As String = String.Empty
        Dim vCreatedBy As String = String.Empty
        Dim vDataEntryBy As String = String.Empty
        Dim iPeriod As Integer = -1
        Dim str As String = String.Empty


        Dim strSubjectId As String = String.Empty
        Dim strParentID As String = String.Empty
        Dim strActivityId As String = String.Empty
        Dim Resolvedby As String = String.Empty
        Try

            Me.btnExportToExcel.Visible = False
            Me.btnReReview.Visible = False
            workspaceid = Me.HProjectId.Value.Trim()

            If Me.chkAllDates.Checked Then
            Else
                cIsChkAll = "N"
                dFromDate = Me.txtFromDate.Text
                dToDate = Me.txtToDate.Text
            End If

            If Me.ddlStatus.SelectedItem.Value.ToUpper() <> "ALL" Then
                cStatus = Me.ddlStatus.SelectedItem.Text.Trim().ToUpper
            End If

            'If Me.chkLstSubjects.Items.Count = 0 Then  '' comment by prayag
            '    objCommon.ShowAlert("No Subject Found.", Me)
            '    Exit Function
            'End If
            'wStr += " And SubjectId in("

            'If Me.chkGenericActivities.Checked Then  '' comment by prayag
            '    vSubjectId += ""
            '    flag_Selected = True
            'End If

            'If Me.chkGenericActivities.Checked Then  '' comment by prayag
            '    strSubjectId += ""
            '    flag_Selected = True
            'End If

            'For Each item In Me.chkLstSubjects.Items  '' comment by prayag
            '    If item.Selected Then
            '        flag_Selected = True
            '        vSubjectId += item.Value.Trim() + ","
            '    End If
            'Next item

            'If tvSubject.Nodes(0).ChildNodes.Count = 0 Then
            '    objCommon.ShowAlert("No Subject Found.", Me)
            '    Return False
            '    Exit Function
            'End If

            If Request.QueryString("SubjectId") <> "" Then
                For index = 0 To tvSubject.Nodes(0).ChildNodes.Count - 1
                    If tvSubject.Nodes(0).ChildNodes(index).Checked = True Then
                        strSubjectId = strSubjectId + tvSubject.Nodes(0).ChildNodes(index).Value + ","
                        flag_Selected = True
                    End If
                Next

            End If

            If Me.ddlType.SelectedValue = Sub_Specific_Activity Then

                For index = 0 To tvSubject.Nodes(0).ChildNodes.Count - 1
                    If tvSubject.Nodes(0).ChildNodes(index).Checked = True Then
                        strSubjectId = strSubjectId + tvSubject.Nodes(0).ChildNodes(index).Value + ","
                        flag_Selected = True
                    End If
                Next

            ElseIf Me.ddlType.SelectedValue = Generic_Activity Then
                strSubjectId += "0000,"
                flag_Selected = True
            Else
                strSubjectId += ""
                flag_Selected = True
            End If

            If flag_Selected = False Then
                Me.objCommon.ShowAlert("Please select Subject", Me.Page)
                Return False
                Exit Function
            End If

            If Me.ddlType.SelectedValue <> 0 Then
                If tvActivity.Nodes(0).Checked = False Then
                    For index = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                        If tvActivity.Nodes(0).ChildNodes(index).Checked = True Then
                            strActivityId = strActivityId + tvActivity.Nodes(0).ChildNodes(index).Value + ","
                        End If
                    Next
                End If
                If tvActivity.Nodes(0).Checked = False Then
                    For iParent = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                        For iChild = 0 To tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes.Count - 1
                            If tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Checked = True Then
                                strActivityId = strActivityId + tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Value + ","
                            End If
                        Next 'Next iChild
                    Next 'Next iPare
                End If
                If strActivityId <> "" Then
                    strActivityId = strActivityId.Remove(strActivityId.Length - 1)
                End If
            End If

            If Me.Request.QueryString("mode") <> "1" Then

                If Me.ddlDiscrepancyType.SelectedIndex <> 0 Then
                    cDCFType = Me.ddlDiscrepancyType.SelectedItem.Value.Trim()
                End If
            End If
            If (Me.Request.QueryString("TrnType") Is Nothing) Then
                If Me.ddlCreatedBy.SelectedIndex > 0 Then
                    vCreatedBy = Me.ddlCreatedBy.SelectedItem.Text.Trim()
                End If

                If Me.ddlDataEntryBy.SelectedIndex > 0 Then
                    vDataEntryBy = Me.ddlDataEntryBy.SelectedItem.Text.Trim()
                End If

                If Me.DdlPeriod.SelectedIndex > 0 Then
                    iPeriod = Me.DdlPeriod.SelectedValue.ToString()
                End If

                If Me.ddlResolvedBy.SelectedIndex > 0 Then
                    Resolvedby = Me.ddlResolvedBy.SelectedValue.ToString()
                End If
            End If

            If strSubjectId.ToString.Trim() <> "" Then
                strSubjectId = strSubjectId.Remove(strSubjectId.Length - 1, 1)
            End If

            If Me.Request.QueryString("mode") = "1" Then
                str = workspaceid.ToString + "##" + strSubjectId.ToString + "##" + cIsChkAll.ToString + "##" + dFromDate.ToString + "##" + dToDate.ToString + "##" + cStatus.ToString + "##" + cDCFType.ToString + "##" + vCreatedBy.ToString + "##" + vDataEntryBy.ToString + "##" + iPeriod.ToString
                ' parameter added by prayag
                str += "##" + strActivityId.ToString
                ds_Grid = objHelpDb.ProcedureExecute("Proc_ODMSTATUSREPORT", str)

            Else
                str = workspaceid.ToString + "##" + strSubjectId.ToString + "##" + cIsChkAll.ToString + "##" + dFromDate.ToString + "##" + dToDate.ToString + "##" + cStatus.ToString + "##" + cDCFType.ToString + "##" + vCreatedBy.ToString + "##" + vDataEntryBy.ToString + "##" + iPeriod.ToString
                ' parameter added by prayag
                str += "##" + strActivityId.ToString + "##" + Resolvedby.ToString
                ds_Grid = objHelpDb.ProcedureExecute("Proc_DiscrepancyStatusReport", str)
            End If

            Me.ViewState(VS_DtGVWDCF) = Nothing
            If ds_Grid.Tables(0).Rows.Count > 0 Then

                Me.btnExportToExcel.Visible = True
                'If ddlStatus.SelectedValue = "E" Then
                '    Me.btnReReview.Visible = True
                'End If
                Me.ViewState(VS_DtGVWDCF) = ds_Grid.Tables(0)

                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "grdshow", "DivfieldShowHide('R');", True) ' added by Prayag
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideEntryData", "displayCRFInfo(" + Me.imgfldgen.ClientID + ",'tblEntryData');", True) ' added by Prayag

            Else
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "fldgrdfalse", "DivfieldShowHide('Q');", True)  ' added by Prayag
                objCommon.ShowAlert("No Record Found.", Me)
                Me.GVWDSR.DataSource = Nothing
                Me.GVWDSR.DataBind()
                Me.GVWODMSR.DataSource = Nothing
                Me.GVWODMSR.DataBind()
                Me.lblCount.Text = ""
                Return False
                Exit Function
            End If
            If Me.Request.QueryString("mode") = "1" Then
                Me.GVWODMSR.DataSource = ds_Grid
                Me.GVWODMSR.DataBind()
            Else
                Me.GVWDSR.DataSource = ds_Grid
                Me.GVWDSR.DataBind()
                Me.lblCount.Text = "Total " + IIf(Me.ddlStatus.SelectedItem.Text.ToUpper <> "ALL", Me.ddlStatus.SelectedItem.Text, "") + " DCF: " + ds_Grid.Tables(0).Rows.Count.ToString
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Filling Grid. ", ex.Message)
            Return False
        End Try

    End Function

    Private Function FillDCFGrid() As Boolean

        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds As New DataSet
        Dim flag_Selected As Boolean = False
        Dim dsDCF_Grid As New Data.DataSet
        Dim workspaceid As String = String.Empty
        Dim cStatus As String = String.Empty
        Dim vSubjectId As String = String.Empty
        Dim cDCFType As String = String.Empty
        Dim vCreatedBy As String = String.Empty
        Dim iPeriod As Integer = -1
        Dim str As String = String.Empty

        Dim strActivityId As String = String.Empty
        Dim Resolvedby As String = String.Empty

        Try
            Me.btnDCFExportToExcel.Visible = False
            workspaceid = Me.HProjectId.Value.Trim()
            'If Me.chkLstSubjects.Items.Count = 0 Then  '' comment by prayag
            '    objCommon.ShowAlert("No Subject Found.", Me)
            '    Exit Function
            'End If

            'If tvSubject.Nodes(0).ChildNodes.Count = 0 Then
            '    objCommon.ShowAlert("No Subject Found!", Me)
            '    Return False
            '    Exit Function
            'End If



            'If Me.chkGenericActivities.Checked Then   '' comment by prayag
            '    vSubjectId += "0000,"
            '    flag_Selected = True
            'End If

            'For Each item In Me.chkLstSubjects.Items   '' comment by prayag
            '    If item.Selected Then
            '        flag_Selected = True
            '        vSubjectId += item.Value.Trim() + ","
            '    End If
            'Next item

            If Me.ddlType.SelectedValue = Sub_Specific_Activity Then

                For index = 0 To tvSubject.Nodes(0).ChildNodes.Count - 1
                    If tvSubject.Nodes(0).ChildNodes(index).Checked = True Then
                        vSubjectId = vSubjectId + tvSubject.Nodes(0).ChildNodes(index).Value + ","
                        flag_Selected = True
                    End If
                Next

            ElseIf Me.ddlType.SelectedValue = Generic_Activity Or Not Me.Request.QueryString("TrnType") Is Nothing Then
                vSubjectId += "0000,"
                flag_Selected = True
            Else
                vSubjectId += ""
                flag_Selected = True
            End If

            If flag_Selected = False Then
                Me.objCommon.ShowAlert("Please select Subject", Me.Page)
                Return False
                Exit Function
            End If


            If Me.ddlType.SelectedValue <> 0 Then
                If tvActivity.Nodes(0).Checked = False Then
                    For index = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                        If tvActivity.Nodes(0).ChildNodes(index).Checked = True Then
                            strActivityId = strActivityId + tvActivity.Nodes(0).ChildNodes(index).Value + ","
                        End If
                    Next
                End If
                If tvActivity.Nodes(0).Checked = False Then
                    For iParent = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                        For iChild = 0 To tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes.Count - 1
                            If tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Checked = True Then
                                strActivityId = strActivityId + tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Value + ","
                            End If
                        Next 'Next iChild
                    Next 'Next iPare
                End If
                If strActivityId <> "" Then
                    strActivityId = strActivityId.Remove(strActivityId.Length - 1)
                End If
            End If


            If Me.ddlDCFTypes.SelectedIndex <> 0 Then
                cDCFType = Me.ddlDCFTypes.SelectedItem.Value.Trim()
            End If

            If (Me.Request.QueryString("TrnType") Is Nothing) Then
                If Me.ddlGeneratedBy.SelectedIndex <> 0 Then
                    vCreatedBy = Me.ddlGeneratedBy.SelectedItem.Text.Trim()
                End If

                If Me.DdlPeriod.SelectedIndex > 0 Then
                    iPeriod = Me.DdlPeriod.SelectedValue.ToString()
                End If

            End If

            If Me.ddlStatus.SelectedItem.Value.ToUpper() <> "ALL" Then
                cStatus = Me.ddlStatus.SelectedItem.Text.Trim().ToUpper
            End If

            If vSubjectId.ToString.Trim() <> "" Then
                vSubjectId = vSubjectId.Remove(vSubjectId.Length - 1, 1)
            End If


            str = workspaceid.ToString + "##" + vSubjectId.ToString + "##" + cDCFType.ToString + "##" + vCreatedBy.ToString + "##" + iPeriod.ToString
            ' parameter added by prayag
            str += "##" + strActivityId.ToString

            dsDCF_Grid = objHelpDb.ProcedureExecute("Proc_DCFTracking1", str)

            Me.ViewState(VS_DtGVWDCF) = Nothing
            If dsDCF_Grid.Tables(0).Rows.Count > 0 Then
                Me.btnDCFExportToExcel.Visible = True
                Me.ViewState(VS_DCF) = dsDCF_Grid
            Else
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "fldgrdfalse", "DivfieldShowHide('Q');", True)  ' added by Prayag
                objCommon.ShowAlert("No Record Found.", Me)
                Me.gvwDCF.DataSource = Nothing
                Me.gvwDCF.DataBind()
                Me.lblCount.Text = ""
                Return False
                Exit Function
            End If

            Me.gvwDCF.DataSource = dsDCF_Grid
            Me.gvwDCF.DataBind()
            Me.lblCount.Text = "Total " + " DCF: " + dsDCF_Grid.Tables(0).Rows.Count.ToString
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideEntryData", "displayCRFInfo(" + Me.imgfldgen.ClientID + ",'tblEntryData');", True) ' added by Prayag
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "grdshow", " $('#ctl00_CPHLAMBDA_fldgrdParent').css({'display':'inline-block'}) ", True) ' added by Prayag

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Filling Grid. ", ex.Message)
            Return False
        End Try
    End Function

    Private Function FillRereviewGrid() As Boolean
        Dim vPeriod As String = String.Empty
        Dim vActivity As String = String.Empty
        Dim vNodeId As String = String.Empty
        Dim vSubject As String = String.Empty
        Dim eStr As String = String.Empty
        Dim dtRereview As New DataTable
        Dim paramActivity() As String = {"Site", "Period", "SubjectID", "ScreenNo", "RandomizationNo", "Visit", "Activity", "RepetitionNo", "PendingRereviewLevel"}
        Dim strRowFilter As String = String.Empty
        Try
            GVWDSR.DataSource = Nothing
            GVWDSR.DataBind()
            Me.btnRereviewExportToExcel.Visible = False

            'If ddlRereviewActivity.SelectedIndex > 1 Then
            '    vPeriod = DdlPeriod.SelectedValue.ToString().Trim()
            '    strRowFilter += " AND Period = " + vPeriod
            'End If
            'If ddlRereviewActivity.SelectedIndex > 1 Then
            '    'vActivity = Me.ddlActivity.SelectedValue.Split("##")(0)
            '    vNodeId = Me.ddlRereviewActivity.SelectedValue.ToString.Trim()
            '    strRowFilter += " AND [Scheduling Node] = " + vNodeId
            'End If
            'If ddlRereviewSubject.SelectedIndex > 1 Then
            '    vSubject = ddlRereviewSubject.SelectedValue.ToString().Trim()
            '    strRowFilter += " AND iMySubjectNo = " + vSubject
            'End If

            If hdnRereviewActivity.Value.Trim() <> "" Then
                strRowFilter = "iNodeId IN (" + hdnRereviewActivity.Value.ToString().Replace("'", "") + ")"
            End If
            If hdnRereviewSubject.Value.Trim() <> "" Then
                If strRowFilter.Trim() <> "" Then
                    strRowFilter += " AND SubjectID IN (" + hdnRereviewSubject.Value.ToString() + ")"
                Else
                    strRowFilter = "SubjectID IN (" + hdnRereviewSubject.Value.ToString() + ")"
                End If
            End If
            If ddlRereviewLevel.SelectedValue.ToUpper <> "ALL" And ddlRereviewLevel.SelectedValue.ToUpper <> "" Then
                If strRowFilter.ToString.Trim() = "" Then
                    strRowFilter = "iWorkflowStageId = " + (CType(ddlRereviewLevel.SelectedValue, Integer) - 10).ToString
                Else
                    strRowFilter += " AND iWorkflowStageId = " + (CType(ddlRereviewLevel.SelectedValue, Integer) - 10).ToString
                End If
            End If

            'If Not ObjHelp.Proc_GetDataForScheduling_Deviation(Me.HProjectId.Value.Trim(), vPeriod, vNodeId, dsDeviation, eStr) Then
            '    Throw New Exception(eStr)
            'End If
            dtRereview = CType(ViewState(VS_Rereview), DataTable).Copy()
            dtRereview.DefaultView.RowFilter = strRowFilter

            If dtRereview.DefaultView.Count = 0 Then
                Me.gvRereview.DataSource = Nothing
                Me.gvRereview.DataBind()
                Me.btnRereviewExportToExcel.Visible = False
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "fldgrdfalse", "DivfieldShowHide('Q');", True)  ' added by Prayag
                objCommon.ShowAlert("No Record found.", Me)
                Return False
                Exit Function
            End If
            Me.gvRereview.DataSource = dtRereview.DefaultView.ToTable().Copy()
            Me.ViewState(VS_RereviewExporttoExcel) = dtRereview.DefaultView.ToTable(True, paramActivity).Copy()
            Me.gvRereview.DataBind()
            Me.btnRereviewExportToExcel.Visible = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideEntryData", "displayCRFInfo(" + Me.imgfldgen.ClientID + ",'tblEntryData');", True) ' added by Prayag
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "grdshow", "DivfieldShowHide('R');", True) ' added by Prayag
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Filling Grid. ", ex.Message)
            Return False
        End Try
    End Function

    'Private Function Fillsubjects() As Boolean  comment by prayag
    '    Dim wStr As String = String.Empty
    '    Dim eStr As String = String.Empty
    '    Dim ds_Check As New DataSet
    '    Dim dv_Check As New DataView
    '    Dim ds_Subjects As New DataSet
    '    Dim ds_CreatedBy As New DataSet
    '    Dim dv_CreatedBy As New DataView
    '    Dim lItem As ListItem
    '    Dim workspaceid As String = String.Empty
    '    Dim cIsChkAll As Char = "Y"
    '    Dim dFromDate As String = String.Empty
    '    Dim dToDate As String = Today.Date.AddDays(1).ToString("dd-MMM-yyyy")
    '    Dim cStatus As String = String.Empty
    '    Dim vSubjectId As String = String.Empty
    '    Dim cDCFType As String = String.Empty
    '    Dim vCreatedBy As String = String.Empty
    '    Dim vDataEntryBy As String = String.Empty
    '    Dim iPeriod As Integer = -1
    '    Dim str As String = String.Empty
    '    Dim sender As Object
    '    Dim e As System.EventArgs

    '    Dim strActivityId As String = String.Empty

    '    Try

    '        If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
    '            wStr = " iPeriod = 1 And " 'As period 1 contains all the subjects
    '            If Me.DdlPeriod.SelectedIndex > 0 Then
    '                wStr = " iPeriod = " & Me.DdlPeriod.SelectedValue.Trim.ToString() + " And "
    '            End If
    '        End If
    '        ''Filter of iMySubjectNo>0 Removed for viewing all subjects-------22-Oct-2011
    '        wStr += " vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"

    '        wStr += " order by iMySubjectNo"
    '        If Not Me.objHelpDb.GetViewWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                ds_Subjects, eStr) Then
    '            Throw New Exception(eStr)
    '        End If

    '        If Not ds_Subjects.Tables(0) Is Nothing Then

    '            Dim Dv_Subjects As New DataView
    '            'Me.chkLstSubjects.Items.Clear()  comment by prayag


    '            For Each dr As DataRow In ds_Subjects.Tables(0).Rows

    '                lItem = New ListItem
    '                lItem.Text = Convert.ToString(dr("vInitials")).Trim() + "(" + Convert.ToString(dr("vMySubjectNo")).Trim() + ")(" + Convert.ToString(dr("vRandomizationNo")).Trim() + ")"
    '                lItem.Value = dr("vSubjectId")
    '                Try
    '                    If (Request.QueryString("ProjectName") <> "" And Request.QueryString("SubjectId") = "") Then
    '                        lItem.Selected = True
    '                    End If
    '                    If (Request.QueryString("ProjectName") <> "" And Request.QueryString("SubjectId") <> "" And Request.QueryString("SubjectId") = dr("vSubjectId")) Then
    '                        lItem.Selected = True
    '                    End If
    '                Catch ex As Exception

    '                End Try
    '                'Me.chkLstSubjects.Items.Add(lItem)


    '                'If dr("cRejectionFlag") = "Y" Then
    '                '    Me.chkLstSubjects.Items(index - 1).Attributes.Add("Style", "Color:Red")
    '                'End If

    '            Next dr

    '        End If
    '        '*********************
    '        If (Me.Request.QueryString("TrnType") Is Nothing) Then

    '            'Filling Created By list
    '            workspaceid = Me.HProjectId.Value.Trim()
    '            If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
    '                If Me.DdlPeriod.SelectedIndex > 0 Then
    '                    iPeriod = Me.DdlPeriod.SelectedValue.ToString()
    '                End If
    '            End If
    '            If Me.Request.QueryString("mode") = "1" Then
    '                str = workspaceid.ToString + "##" + vSubjectId.ToString + "##" + cIsChkAll.ToString + "##" + dFromDate.ToString + "##" + dToDate.ToString + "##" + cStatus.ToString + "##" + cDCFType.ToString + "##" + vCreatedBy.ToString + "##" + vDataEntryBy.ToString + "##" + iPeriod.ToString
    '                ds_CreatedBy = objHelpDb.ProcedureExecute("Proc_ODMSTATUSREPORT", str)
    '            ElseIf chkDCF.Checked Then
    '                str = workspaceid.ToString + "##" + vSubjectId.ToString + "##" + cDCFType.ToString + "##" + vCreatedBy.ToString + "##" + iPeriod.ToString
    '                ds_CreatedBy = objHelpDb.ProcedureExecute("Proc_DCFTracking1", str)
    '            Else
    '                str = workspaceid.ToString + "##" + vSubjectId.ToString + "##" + cIsChkAll.ToString + "##" + dFromDate.ToString + "##" + dToDate.ToString + "##" + cStatus.ToString + "##" + cDCFType.ToString + "##" + vCreatedBy.ToString + "##" + vDataEntryBy.ToString + "##" + iPeriod.ToString + "##" + strActivityId.ToString
    '                ds_CreatedBy = objHelpDb.ProcedureExecute("Proc_DiscrepancyStatusReport", str)
    '            End If

    '            Me.ddlCreatedBy.Items.Clear()
    '            Me.ddlGeneratedBy.Items.Clear()
    '            If Not ds_CreatedBy.Tables(0) Is Nothing Then

    '                dv_CreatedBy = ds_CreatedBy.Tables(0).DefaultView.ToTable(True, "CreatedBy".Split(",")).DefaultView
    '                dv_CreatedBy.Sort = "CreatedBy"

    '                For Each dr As DataRow In dv_CreatedBy.ToTable().Rows
    '                    Me.ddlCreatedBy.Items.Add(Convert.ToString(dr("CreatedBy")).Trim())
    '                    Me.ddlGeneratedBy.Items.Add(Convert.ToString(dr("CreatedBy")).Trim())
    '                Next
    '                Me.ddlCreatedBy.Items.Insert(0, "All")
    '                Me.ddlCreatedBy.SelectedIndex = 0
    '                Me.ddlGeneratedBy.Items.Insert(0, "All")
    '                Me.ddlGeneratedBy.SelectedIndex = 0

    '            End If
    '            '**********************

    '            Me.ddlDataEntryBy.Items.Clear()
    '            If Not ds_CreatedBy.Tables(0) Is Nothing Then

    '                dv_CreatedBy = ds_CreatedBy.Tables(0).DefaultView.ToTable(True, "DataEntryBy".Split(",")).DefaultView
    '                dv_CreatedBy.Sort = "DataEntryBy"

    '                For Each dr As DataRow In dv_CreatedBy.ToTable().Rows
    '                    Me.ddlDataEntryBy.Items.Add(Convert.ToString(dr("DataEntryBy")).Trim())
    '                Next
    '                Me.ddlDataEntryBy.Items.Insert(0, "All")
    '                Me.ddlDataEntryBy.SelectedIndex = 0
    '                'Me.chkSelectAll.Checked = False
    '                'Me.chkGenericActivities.Checked = False
    '            End If
    '            If Me.chkRereview.Checked Then
    '                chkRereview_CheckedChanged(sender, e)
    '            End If
    '        End If

    '        Fillsubjects = True
    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.Message(), eStr)
    '        Fillsubjects = False
    '    End Try
    'End Function

    Protected Function BindSubjectTree() As Boolean
        'added by prayag
        Dim strqry As String = String.Empty
        Dim whrCon As String = String.Empty
        Dim dsSubject As DataSet = New DataSet
        Dim dtSubject As New DataTable
        Dim period As String = "1"
        Dim eStr As String = String.Empty

        Dim wStr As String = String.Empty
        Dim ds_Subjects As New DataSet
        Dim ds_CreatedBy As New DataSet
        Dim dv_CreatedBy As New DataView
        Dim workspaceid As String = String.Empty
        Dim cIsChkAll As Char = "Y"
        Dim dFromDate As String = String.Empty
        Dim dToDate As String = Today.Date.AddDays(1).ToString("dd-MMM-yyyy")
        Dim cStatus As String = String.Empty
        Dim vSubjectId As String = String.Empty
        Dim cDCFType As String = String.Empty
        Dim vCreatedBy As String = String.Empty
        Dim vDataEntryBy As String = String.Empty
        Dim iPeriod As Integer = -1
        Dim str As String = String.Empty
        Dim sender As Object
        Dim e As System.EventArgs

        Dim strActivityId As String = String.Empty
        Dim Resolvedby As String = String.Empty

        'If Me.DdlPeriod.SelectedValue <> "All Period" Then   'Added on 2-Feb-2012 to avoid subject repetaion in subject tree view by Mrunal Parekh
        If Me.DdlPeriod.SelectedIndex > 0 Then
            period = Me.DdlPeriod.SelectedValue
        End If

        Try
            whrCon = " vWorkspaceId='" + Me.HProjectId.Value + "'" _
                    + " and  iPeriod=" + CInt(period).ToString + " And cStatusIndi <> 'D' Order by iMySubjectNo"
            'If Me.ddlPeriods.SelectedValue <> "All" Then
            '    whrCon += " and iPeriod=" + Me.ddlPeriods.SelectedValue
            'End If

            Me.divSubject.Style.Add("display", "none")
            Me.tvSubject.Style.Add("Height", "0px")

            If Not objHelp.GetWorkspaceSubjectMst(whrCon, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsSubject, eStr) Then
                Throw New Exception(eStr)
            End If

            Me.tvSubject.Nodes.Clear()

            If Not dsSubject Is Nothing Then
                If dsSubject.Tables(0).Rows.Count > 0 Then


                    dtSubject = dsSubject.Tables(0)
                    Dim nodeAll As New TreeNode()
                    nodeAll.Text = "All SubjectNo*"
                    nodeAll.Value = "All SubjectNo"

                    If (Request.QueryString("ProjectName") <> "" And Request.QueryString("SubjectId") = "") Then
                        nodeAll.Checked = True
                    End If

                    Me.tvSubject.Nodes.Add(nodeAll)
                    For index = 0 To dtSubject.Rows.Count - 1
                        Dim nodeSubject As New TreeNode()

                        If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                            nodeSubject.Text = dtSubject.Rows(index).Item("vMySubjectNo").ToString() + "(" + dtSubject.Rows(index).Item("vRandomizationNo").ToString() + ")"
                        Else
                            nodeSubject.Text = dtSubject.Rows(index).Item("vMySubjectNo").ToString() + "(" + dtSubject.Rows(index).Item("vSubjectId").ToString() + ")"
                        End If
                        'nodeSubject.Text = dtSubject.Rows(index).Item("vInitials").ToString() + "(" + dtSubject.Rows(index).Item("vMySubjectNo").ToString() + ")(" + dtSubject.Rows(index).Item("vRandomizationNo").ToString() + ")"

                        If dtSubject.Rows(index).Item("cRejectionFlag").ToString() = "Y" Then
                            nodeSubject.Text = "<font color = red>" + dtSubject.Rows(index).Item("vMySubjectNo").ToString() + "</font>"
                        End If

                        nodeSubject.ToolTip = dtSubject.Rows(index).Item("vMySubjectNo").ToString() + "|" + dtSubject.Rows(index).Item("vSubjectId").ToString()
                        nodeSubject.Value = dtSubject.Rows(index).Item("vSubjectId").ToString()
                        nodeSubject.SelectAction = TreeNodeSelectAction.None
                        If (Request.QueryString("ProjectName") <> "" And Request.QueryString("SubjectId") <> "" And Request.QueryString("SubjectId") = dtSubject.Rows(index).Item("vSubjectId").ToString()) Then
                            nodeSubject.Checked = True
                        ElseIf (Request.QueryString("ProjectName") <> "" And Request.QueryString("SubjectId") = "") Then
                            nodeSubject.Checked = True
                        End If
                        nodeSubject.ChildNodes.Add(nodeSubject)

                        Me.tvSubject.Nodes(0).ChildNodes.Add(nodeSubject)
                    Next ' Next Index
                    Me.tvSubject.Nodes(0).ExpandAll()
                    Me.tvSubject.Nodes(0).SelectAction = TreeNodeSelectAction.None

                    If Me.ddlType.SelectedValue = Sub_Specific_Activity Then
                        Me.divSubject.Style.Add("display", "block")
                        Me.tvSubject.Style.Add("Height", "100px")
                    End If

                Else
                    'objCommon.ShowAlert("No Subject Found!", Me)
                    'Return False
                    'Exit Function
                End If
            End If

            '*********************

            If (Me.Request.QueryString("TrnType") Is Nothing) Then

                'Filling Created By list
                workspaceid = Me.HProjectId.Value.Trim()
                If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                    If Me.DdlPeriod.SelectedIndex > 0 Then
                        iPeriod = Me.DdlPeriod.SelectedValue.ToString()
                    End If
                End If
                If Me.Request.QueryString("mode") = "1" Then
                    str = workspaceid.ToString + "##" + vSubjectId.ToString + "##" + cIsChkAll.ToString + "##" + dFromDate.ToString + "##" + dToDate.ToString + "##" + cStatus.ToString + "##" + cDCFType.ToString + "##" + vCreatedBy.ToString + "##" + vDataEntryBy.ToString + "##" + iPeriod.ToString
                    ' parameter added by prayag
                    str += "##" + strActivityId.ToString

                    ds_CreatedBy = objHelpDb.ProcedureExecute("Proc_ODMSTATUSREPORT", str)
                ElseIf chkDCF.Checked Then
                    str = workspaceid.ToString + "##" + vSubjectId.ToString + "##" + cDCFType.ToString + "##" + vCreatedBy.ToString + "##" + iPeriod.ToString
                    ' parameter added by prayag
                    str += "##" + strActivityId.ToString

                    ds_CreatedBy = objHelpDb.ProcedureExecute("Proc_DCFTracking1", str)
                Else
                    str = workspaceid.ToString + "##" + vSubjectId.ToString + "##" + cIsChkAll.ToString + "##" + dFromDate.ToString + "##" + dToDate.ToString + "##" + cStatus.ToString + "##" + cDCFType.ToString + "##" + vCreatedBy.ToString + "##" + vDataEntryBy.ToString + "##" + iPeriod.ToString
                    ' parameter added by prayag
                    str += "##" + strActivityId.ToString + "##" + Resolvedby.ToString
                    ds_CreatedBy = objHelpDb.ProcedureExecute("Proc_DiscrepancyStatusReport", str)
                End If

                Me.ddlCreatedBy.Items.Clear()
                Me.ddlGeneratedBy.Items.Clear()
                If Not ds_CreatedBy.Tables(0) Is Nothing Then

                    dv_CreatedBy = ds_CreatedBy.Tables(0).DefaultView.ToTable(True, "CreatedBy".Split(",")).DefaultView
                    dv_CreatedBy.Sort = "CreatedBy"

                    For Each dr As DataRow In dv_CreatedBy.ToTable().Rows
                        Me.ddlCreatedBy.Items.Add(Convert.ToString(dr("CreatedBy")).Trim())
                        Me.ddlGeneratedBy.Items.Add(Convert.ToString(dr("CreatedBy")).Trim())
                    Next
                    Me.ddlCreatedBy.Items.Insert(0, "All")
                    Me.ddlCreatedBy.SelectedIndex = 0
                    Me.ddlGeneratedBy.Items.Insert(0, "All")
                    Me.ddlGeneratedBy.SelectedIndex = 0

                End If
                '**********************
                Me.ddlDataEntryBy.Items.Clear()
                If Not ds_CreatedBy.Tables(0) Is Nothing Then

                    dv_CreatedBy = ds_CreatedBy.Tables(0).DefaultView.ToTable(True, "DataEntryBy".Split(",")).DefaultView
                    dv_CreatedBy.Sort = "DataEntryBy"

                    For Each dr As DataRow In dv_CreatedBy.ToTable().Rows
                        Me.ddlDataEntryBy.Items.Add(Convert.ToString(dr("DataEntryBy")).Trim())
                    Next
                    Me.ddlDataEntryBy.Items.Insert(0, "All")
                    Me.ddlDataEntryBy.SelectedIndex = 0
                    'Me.chkSelectAll.Checked = False
                    'Me.chkGenericActivities.Checked = False

                    If (Not chkDCF.Checked) And Me.Request.QueryString("TrnType") Is Nothing And Not Me.Request.QueryString("mode") = "1" Then
                        Me.ddlResolvedBy.Items.Clear()
                        dv_CreatedBy = ds_CreatedBy.Tables(0).DefaultView.ToTable(True, "vResolvedBy".Split(",")).DefaultView
                        dv_CreatedBy.Sort = "vResolvedBy"
                        dv_CreatedBy.RowFilter = "vResolvedBy <> ''"
                        dv_CreatedBy = dv_CreatedBy.ToTable().DefaultView

                        For Each dr As DataRow In dv_CreatedBy.ToTable.Rows
                            Me.ddlResolvedBy.Items.Add(Convert.ToString(dr("vResolvedBy")).Trim())
                        Next
                        Me.ddlResolvedBy.Items.Insert(0, "All")
                        Me.ddlResolvedBy.SelectedIndex = 0
                    End If
                End If
                If Me.chkRereview.Checked Then
                    chkRereview_CheckedChanged(sender, e)
                End If
            End If

            Return True
        Catch ex As Exception

            Return False
        Finally
            dsSubject.Dispose()
            dtSubject.Dispose()
        End Try
    End Function

    Protected Function BindActivityTree() As Boolean
        'added by prayag
        Dim eStr As String = String.Empty
        Dim strqry As String = String.Empty
        Dim iPeriod As String = String.Empty
        Dim whrCon As String = String.Empty
        Dim dtActivity As New DataTable
        Dim dsActivity As DataSet = New DataSet
        Dim dvActivity As DataView
        Dim dvChild As DataView
        Dim Subject_Specific As String = String.Empty
        Dim ActNodeAll As New TreeNode()
        Try
            iPeriod = "," + Me.DdlPeriod.SelectedValue.ToString() + ","
            If Me.DdlPeriod.SelectedValue = "All Period" Then
                iPeriod = ""
            End If
            Subject_Specific = "Y"
            If Me.ddlType.SelectedValue = Generic_Activity Then
                Subject_Specific = "N"
            End If

            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                If Not objHelp.Proc_ActivityTreeCTM(Me.HProjectId.Value, iPeriod, Subject_Specific, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, dsActivity, eStr) Then
                    Throw New Exception(eStr)
                End If
            Else
                If Not objHelp.Proc_ActivityTreeBABE(Me.HProjectId.Value, iPeriod, Subject_Specific, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, dsActivity, eStr) Then
                    Throw New Exception(eStr)
                End If
            End If
            dtActivity = dsActivity.Tables(0)
            dvActivity = New DataView(dtActivity)

            Me.tvActivity.Nodes.Clear()

            dvActivity.RowFilter = "TreeLevel=0"
            ActNodeAll.Text = "All Activity*"
            ActNodeAll.Value = "All Activity"
            Me.tvActivity.Nodes.Add(ActNodeAll)
            If (Request.QueryString("ProjectName") <> "") Then
                ActNodeAll.Checked = True
            End If



            For ParentNode = 0 To dvActivity.Count - 1
                Dim nodeActivity As New TreeNode()
                nodeActivity.Text = dvActivity(ParentNode).Item("Name").ToString()
                nodeActivity.ToolTip = dvActivity(ParentNode).Item("Name").ToString()
                nodeActivity.Value = dvActivity(ParentNode).Item("Id").ToString()
                If (Request.QueryString("ProjectName") <> "") Then
                    nodeActivity.Checked = True
                End If
                nodeActivity.SelectAction = TreeNodeSelectAction.None
                nodeActivity.ChildNodes.Add(nodeActivity)
                dvChild = New DataView(dtActivity)
                dvChild.RowFilter = "Treelevel=1 AND ParentId=" + dvActivity(ParentNode).Item("Id").ToString()
                dvChild.Sort = "iNodeNo"
                For ChildNode = 0 To dvChild.Count - 1
                    Dim nodeChild As New TreeNode()
                    nodeChild.Text = dvChild(ChildNode)("Name").ToString()
                    nodeChild.ToolTip = dvChild(ChildNode)("Name").ToString()
                    nodeChild.Value = dvChild(ChildNode)("Id").ToString()
                    If (Request.QueryString("ProjectName") <> "") Then
                        nodeChild.Checked = True
                    End If
                    nodeChild.SelectAction = TreeNodeSelectAction.None
                    nodeActivity.ChildNodes.Add(nodeChild)
                Next 'Next Child Node
                Me.tvActivity.Nodes(0).ChildNodes.Add(nodeActivity)
            Next 'Newxt Parent Node
            Me.tvActivity.Nodes(0).Expand()
            Me.tvActivity.Nodes(0).SelectAction = TreeNodeSelectAction.None
            Return True

        Catch ex As Exception
            eStr = ex.Message
            Return False
        Finally
            dsActivity.Dispose()
            dtActivity.Dispose()
        End Try
    End Function

    Private Function FillViewState() As Boolean
        Dim ds_Grid As New DataSet
        Dim eStr As String = String.Empty
        Dim workspaceid As String = String.Empty
        Dim cIsChkAll As Char = "Y"
        Dim dFromDate As String = String.Empty
        Dim dToDate As String = Today.Date.ToString("dd-MMM-yyyy")
        Dim cStatus As String = "Re-reviewed"
        Dim vSubjectId As String = String.Empty
        Dim cDCFType As String = String.Empty
        Dim vCreatedBy As String = String.Empty
        Dim vDataEntryBy As String = String.Empty
        Dim iPeriod As String = String.Empty
        Dim Str As String = String.Empty

        Dim strActivityId As String = String.Empty
        Dim Resolvedby As String = String.Empty
        Try
            workspaceid = HProjectId.Value.Trim().ToString
            iPeriod = DdlPeriod.SelectedValue.ToString().Trim()
            If DdlPeriod.SelectedValue.ToUpper = "All Period".ToUpper Then
                iPeriod = "-1"
            End If
            Str = workspaceid.ToString + "##" + vSubjectId.ToString + "##" + cIsChkAll.ToString + "##" + dFromDate.ToString + "##" + dToDate.ToString + "##" + cStatus.ToString + "##" + cDCFType.ToString + "##" + vCreatedBy.ToString + "##" + vDataEntryBy.ToString + "##" + iPeriod.ToString
            ' parameter added by prayag
            Str += "##" + strActivityId.ToString + "##" + Resolvedby.ToString
            ds_Grid = objHelpDb.ProcedureExecute("Proc_DiscrepancyStatusReport", Str)
            Me.ViewState(VS_Rereview) = ds_Grid.Tables(0).DefaultView.ToTable(True, "Site", "Period", "SubjectID", "ScreenNo", "RandomizationNo", "Visit", "Activity", "RepetitionNo", "PendingRereviewLevel", "iWorkFlowstageId", "vActivityId", "iNodeId", "iMySubjectNo", "nCRFDtlNo", "SubjectUniqueId", "DCFiWorkflowStageId").Copy()
            'Me.ViewState(VS_Rereview) = ds_Grid.Tables(0).DefaultView.ToTable().Copy()
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error While FillViewState. ", ex.Message)
            Return False
        End Try
    End Function

    Private Function FillDCFViewState() As Boolean
        Dim dsDCF_Grid As New DataSet
        Dim eStr As String = String.Empty
        Dim workspaceid As String = String.Empty
        Dim cIsChkAll As Char = "Y"
        Dim dFromDate As String = String.Empty
        Dim dToDate As String = Today.Date.ToString("dd-MMM-yyyy")
        Dim cStatus As String = String.Empty
        Dim vSubjectId As String = String.Empty
        Dim cDCFType As String = String.Empty
        Dim vCreatedBy As String = String.Empty
        Dim vDataEntryBy As String = String.Empty
        Dim iPeriod As String = String.Empty
        Dim Str As String = String.Empty

        Dim strActivityId As String = String.Empty
        Try
            workspaceid = HProjectId.Value.Trim().ToString
            iPeriod = DdlPeriod.SelectedValue.ToString().Trim()
            If DdlPeriod.SelectedValue.ToUpper = "All Period".ToUpper Then
                iPeriod = "-1"
            End If
            'Str = workspaceid.ToString + "##" + vSubjectId.ToString + "##" + cIsChkAll.ToString + "##" + dFromDate.ToString + "##" + dToDate.ToString + "##" + cStatus.ToString + "##" + cDCFType.ToString + "##" + vCreatedBy.ToString + "##" + vDataEntryBy.ToString + "##" + iPeriod.ToString
            Str = workspaceid.ToString + "##" + vSubjectId.ToString + "##" + cDCFType.ToString + "##" + vCreatedBy.ToString + "##" + iPeriod.ToString
            ' parameter added by prayag
            Str += "##" + strActivityId.ToString
            dsDCF_Grid = objHelpDb.ProcedureExecute("Proc_DCFTracking1", Str)
            'dsDCF_Grid = objHelpDb.ProcedureExecute("Proc_DiscrepancyStatusReport", Str)
            Me.ViewState(VS_DCF) = dsDCF_Grid.Tables(0).DefaultView.ToTable(True, "Period", "SubjectID", "Visit", "Activity", "RepetitionNo", "iWorkFlowstageId", "vActivityId", "iNodeId", "iMySubjectNo", "nCRFDtlNo", "SubjectUniqueId").Copy()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error While FillDCFViewState. ", ex.Message)
            Return False
        End Try
    End Function

    'Private Function FillDCFActivity() As Boolean
    '    Dim dt_Activity As New DataTable
    '    Dim dv_Activity As New DataView
    '    Dim estr As String = String.Empty
    '    Dim wStr As String = String.Empty
    '    Try


    '        dv_Activity = CType(Me.ViewState(VS_DCF), DataTable).Copy().DefaultView

    '        If DdlPeriod.SelectedValue.ToUpper <> "All Period".ToUpper Then
    '            dv_Activity.RowFilter = "Period = " + DdlPeriod.SelectedValue.Trim()
    '        End If
    '        dt_Activity = dv_Activity.ToTable()

    '        If Not dt_Activity Is Nothing AndAlso dt_Activity.Rows.Count > 0 Then
    '            Me.ddlDCFActivity.DataSource = dt_Activity.DefaultView.ToTable(True, "Activity", "iNodeId")
    '            Me.ddlDCFActivity.DataValueField = "iNodeId"
    '            Me.ddlDCFActivity.DataTextField = "Activity"
    '            Me.ddlDCFActivity.DataBind()

    '        Else
    '            objCommon.ShowAlert("No Activity found.", Me)
    '            'btnReReview.Visible = False
    '            'chkRereview.Checked = False
    '            Return False
    '        End If
    '        Return True
    '    Catch ex As Exception
    '        FillDCFActivity = False
    '        Me.ShowErrorMessage(ex.Message, "....FillddlActivity", ex)
    '    End Try
    'End Function

    Private Function FillActivity() As Boolean
        Dim dt_Activity As New DataTable
        Dim dv_Activity As New DataView
        Dim estr As String = String.Empty
        Dim wStr As String = String.Empty
        Try

            'dv_Activity = CType(Me.ViewState(VS_DtView_WorkSpaceNodeDetail), DataTable).Copy().DefaultView
            dv_Activity = CType(Me.ViewState(VS_Rereview), DataTable).Copy().DefaultView
            'dv_Activity.RowFilter = "iPeriod = " + Me.ddlPeriod.SelectedItem.Value.Trim()
            'dv_Activity.Sort = "iNodeID,iNodeNo"

            If DdlPeriod.SelectedValue.ToUpper <> "All Period".ToUpper Then
                dv_Activity.RowFilter = "Period = " + DdlPeriod.SelectedValue.Trim()
            End If
            dt_Activity = dv_Activity.ToTable()

            If Not dt_Activity Is Nothing AndAlso dt_Activity.Rows.Count > 0 Then
                Me.ddlRereviewActivity.DataSource = dt_Activity.DefaultView.ToTable(True, "Activity", "iNodeId")
                Me.ddlRereviewActivity.DataValueField = "iNodeId"
                Me.ddlRereviewActivity.DataTextField = "Activity"
                Me.ddlRereviewActivity.DataBind()
                'Me.ddlRereviewActivity.Items.Insert(0, New ListItem("All", 0))
                ' Me.ddlActivity.Items.Insert(0, New ListItem("--Select Activity--", 0))
            Else
                objCommon.ShowAlert("No Activity found.", Me)
                btnReReview.Visible = False
                'chkRereview.Checked = False
                Return False
            End If
            Return True
        Catch ex As Exception
            FillActivity = False
            Me.ShowErrorMessage(ex.Message, "....FillddlActivity", ex)
        End Try
    End Function

    

    'Private Function FillDCFSubject() As Boolean
    '    Dim dtSubjects As New DataTable
    '    Dim dvSubjects As New DataView
    '    Try
    '        dvSubjects = CType(Me.ViewState(VS_DCF), DataTable).Copy().DefaultView
    '        If DdlPeriod.SelectedValue.ToUpper <> "All Period".ToUpper Then
    '            dvSubjects.RowFilter = "Period = " + DdlPeriod.SelectedValue.Trim()
    '        End If
    '        dtSubjects = dvSubjects.ToTable()
    '        If Not dtSubjects Is Nothing AndAlso dtSubjects.Rows.Count > 0 Then
    '            Me.ddlDCFSubject.DataSource = dtSubjects.DefaultView.ToTable(True, "SubjectID", "SubjectUniqueId")
    '            Me.ddlDCFSubject.DataValueField = "SubjectID"
    '            Me.ddlDCFSubject.DataTextField = "SubjectUniqueId"
    '            Me.ddlDCFSubject.DataBind()
    '        End If
    '        FillDCFSubject = True
    '    Catch ex As Exception
    '        FillDCFSubject = False
    '        Me.ShowErrorMessage(ex.Message, "....FillddlSubject", ex)
    '    End Try
    'End Function

    Private Function FillRereviewSubject() As Boolean
        Dim dtSubjects As New DataTable
        Dim dvSubjects As New DataView
        Try
            'wStr = "vWorkspaceId = '" + Me.HProjectId.Value + "' And cStatusIndi <> 'D' "
            'wStr += " And iPeriod = " + Me.ddlPeriod.SelectedItem.Value.Trim() '+ " And cRejectionFlag<>'Y'"
            'wStr += " order by vMySubjectNo"

            'If Not ObjHelp.GetViewWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '                                           dsSubjects, eStr) Then
            '    Me.ShowErrorMessage("Error While Getting Data From ViewWorkspaceSubjectMst : ", eStr)
            '    Exit Function
            'End If
            dvSubjects = CType(Me.ViewState(VS_Rereview), DataTable).Copy().DefaultView

            If DdlPeriod.SelectedValue.ToUpper <> "All Period".ToUpper Then
                dvSubjects.RowFilter = "Period = " + DdlPeriod.SelectedValue.Trim()
            End If

            dtSubjects = dvSubjects.ToTable()
            If Not dtSubjects Is Nothing AndAlso dtSubjects.Rows.Count > 0 Then
                Me.ddlRereviewSubject.DataSource = dtSubjects.DefaultView.ToTable(True, "SubjectID", "SubjectUniqueId")
                Me.ddlRereviewSubject.DataValueField = "SubjectID"
                Me.ddlRereviewSubject.DataTextField = "SubjectUniqueId"
                Me.ddlRereviewSubject.DataBind()
                'Me.ddlRereviewSubject.Items.Insert(0, New ListItem("All", 0))
                ''Me.ddlSubject.Items.Insert(0, New ListItem("--Select Subject--", 0))
                'ddlSubject.SelectedIndex = 0
            End If
            FillRereviewSubject = True
        Catch ex As Exception
            FillRereviewSubject = False
            Me.ShowErrorMessage(ex.Message, "....FillddlSubject", ex)
        End Try
    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_Check As New DataSet
        Dim dv_Check As New DataView
        Dim ds_Periods As New DataSet
        Dim TotalPeriods As Integer


        Try
            Me.GVWDSR.DataSource = Nothing
            Me.GVWDSR.DataBind()
            Me.gvwDCF.DataSource = Nothing
            Me.gvwDCF.DataBind()
            Me.gvRereview.DataSource = Nothing
            Me.gvRereview.DataBind()
            Me.ViewState(VS_DCF) = Nothing
            Me.ViewState(VS_RereviewExporttoExcel) = Nothing
            Me.ViewState(VS_DtGVWDCF) = Nothing

            Me.lblCount.Text = ""
            'If Not Fillsubjects() Then
            '    Exit Sub
            'End If
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "fldgrdfalse", "DivfieldShowHide('P');", True)  ' added by Prayag
            If (Me.Request.QueryString("TrnType") Is Nothing) Then


                wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"

                If Not Me.objHelpDb.GetCRFLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    ds_Check, eStr) Then
                    Throw New Exception(eStr)
                End If

                If Not ds_Check Is Nothing AndAlso ds_Check.Tables(0).Rows.Count > 0 Then

                    dv_Check = ds_Check.Tables(0).DefaultView
                    dv_Check.Sort = "iTranNo desc"
                    ViewState(vs_gLock) = dv_Check.ToTable()
                End If

                Me.DdlPeriod.Items.Clear()
                Me.DdlPeriod.Items.Add("All Period")

                If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                    'Me.trPeriod.Visible = True
                    TotalPeriods = 0
                    If Not Me.objHelpDb.GetWorkspaceProtocolDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Periods, eStr) Then
                        Me.ShowErrorMessage(eStr.ToString(), eStr)
                    End If
                    If ds_Periods.Tables(0).Rows.Count > 0 Then

                        If ds_Periods.Tables(0).Rows(0).Item("iNoOfPeriods").ToString() = "" Then
                            Me.objCommon.ShowAlert("Period is not Defined!", Me.Page)
                            Exit Sub
                        End If
                        TotalPeriods = Int32.Parse(ds_Periods.Tables(0).Rows(0).Item("iNoOfPeriods").ToString())
                    End If
                    For Period As Integer = 1 To TotalPeriods
                        Me.DdlPeriod.Items.Add(Period)
                    Next Period
                    'wStr += " And iPeriod = " & Me.DdlPeriod.SelectedValue.ToString()
                End If

            Else
                Me.ddlStatus.SelectedValue = Me.Request.QueryString("TrnType").ToString
                '' comment by prayag
                'Me.chkGenericActivities.Checked = True
                'For Each item In Me.chkLstSubjects.Items
                '    item.Selected = True
                'Next item
                '' comment by prayag
                ' FillGrid()
            End If

            'If Not Fillsubjects() Then ''comment by prayag
            '    Exit Sub
            'End If

            If Not BindSubjectTree() Then
                Exit Sub
            End If

            If Not GetLegends() Then
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Getting CRF Lock Details ", ex.Message)
        End Try
    End Sub

    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
        If Not Me.FillGrid() Then
            Exit Sub
        End If
    End Sub

    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click, btnRereviewExportToExcel.Click
        Dim fileName As String = String.Empty
        Dim dsNew As New DataSet
        Try
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()
            If CType(sender, Button).ID = "btnRereviewExportToExcel" Then
                fileName = "Re-review Report"
                fileName = fileName & ".xls"

                Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)
                dsNew.Tables.Add(CType(Me.ViewState(VS_RereviewExporttoExcel), DataTable).Copy())
                'dsNew = CType(Me.ViewState(VS_RereviewExporttoExcel), DataSet).Copy()
                dsNew.AcceptChanges()

                Context.Response.Write(RereviewExportToExcel(dsNew))
            Else
                If Me.Request.QueryString("mode") = "1" Then
                    If Me.GVWODMSR.Rows.Count < 1 Then
                        Me.objCommon.ShowAlert("No Data To Export", Me.Page)
                        Exit Sub
                    End If
                    fileName = "ODM Status Report"
                    fileName = fileName & ".xls"
                Else
                    If Me.GVWDSR.Rows.Count < 1 Then
                        Me.objCommon.ShowAlert("No Data To Export", Me.Page)
                        Exit Sub
                    End If
                    fileName = "Discrepancy Status Report"
                    fileName = fileName & ".xls"
                End If
                Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
                Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

                dsNew.Tables.Add(CType(Me.ViewState(VS_DtGVWDCF), DataTable))
                dsNew.AcceptChanges()

                Context.Response.Write(ExportToExcel(dsNew))
            End If
            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....btnExportToExcel_Click")
        End Try
    End Sub

    Protected Sub btnDCFExportToExcel_Click(sender As Object, e As EventArgs) Handles btnDCFExportToExcel.Click
        Dim fileName As String = String.Empty
        Dim dsDCFNew As New DataSet
        Try
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            fileName = "DCF Tracking Report"
            fileName = fileName & ".xls"
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)
            dsDCFNew = CType(Me.ViewState(VS_DCF), DataSet).Copy()

            dsDCFNew.AcceptChanges()

            Context.Response.Write(DCFExportToExcel(dsDCFNew))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....btnExportToExcel_Click")
        End Try
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            Me.HProjectId.Value = ""
            Me.txtproject.Text = ""
            Me.chkAllDates.Checked = True
            'Me.chkLstSubjects.Items.Clear() '' comment by prayag
            'Me.chkSelectAll.Checked = False
            Me.txtFromDate.Text = ""
            Me.txtToDate.Text = ""
            Me.ddlCreatedBy.Items.Clear()
            If Me.Request.QueryString("mode") = "1" Then
                Me.GVWODMSR.DataSource = Nothing
                Me.GVWODMSR.DataBind()
                Me.ViewState(VS_DtGVWDCF) = Nothing
            Else
                Me.GVWDSR.DataSource = Nothing
                Me.GVWDSR.DataBind()
                Me.ViewState(VS_DtGVWDCF) = Nothing
            End If
            Me.lblCount.Text = ""
            Me.btnExportToExcel.Visible = False
            Me.btnDCFExportToExcel.Visible = False
            Me.btnRereviewExportToExcel.Visible = False
            Me.btnReReview.Visible = False
            If (Not Me.Request.QueryString("TrnType") Is Nothing) Then
                'Me.trselectSubject.Style("display") = "none"
                'Me.trSubject.Style("display") = "none"
                Me.tblSecond.Style("display") = "none"
            End If

            Me.gvwDCF.DataSource = Nothing
            Me.gvwDCF.DataBind()
            Me.ViewState(VS_DCF) = Nothing
            Me.ddlDCFTypes.SelectedIndex = 0
            Me.DdlPeriod.Items.Clear()
            Me.ddlDataEntryBy.Items.Clear()
            Me.ddlGeneratedBy.Items.Clear()
            Me.ddlResolvedBy.Items.Clear()
            Me.ddlType.Items.Clear()
            Me.gvRereview.DataSource = Nothing
            Me.gvRereview.DataBind()
            Me.ViewState(VS_Rereview) = Nothing
            Me.ViewState(VS_RereviewExporttoExcel) = Nothing
            'Me.chkRereview.Checked = False
            'Me.ddlRereviewLevel.SelectedIndex = 0
            Me.ddlStatus.SelectedIndex = 0
            Me.ddlDiscrepancyType.SelectedIndex = 0
            Me.ddlRereviewActivity.Items.Clear()
            Me.ddlRereviewSubject.Items.Clear()
            Dim PageName As Label = Master.FindControl("lblHeading")

            If (PageName.Text = "ODM Status Report") Then
                Response.Redirect("frmCTMDiscrepancyStatusReport.aspx?Mode=1", False)
            Else
                HttpContext.Current.Response.Redirect("frmCTMDiscrepancyStatusReport.aspx", False)
                'Response.Redirect("frmCTMDiscrepancyStatusReport.aspx", False)
            End If
        Catch ex As Exception
            Throw New Exception("Error while btnCancel_Click")
        End Try
    End Sub

    Protected Sub Btn_Resolve_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_Resolve.Click
        Dim nDcfNo As Integer = Me.HFnDCFNo.Value.Trim.ToString()
        'Dim objLambda = New WS_Lambda.WS_Lambda
        Dim eStr As String = Nothing
        Dim wStr As String = Nothing
        Dim ds_DCF As DataSet = Nothing

        Try

            'nDcfNo = Integer.Parse(e.CommandArgument.ToString())
            wStr = " nDCFNo = " & nDcfNo
            If Not Me.objHelpDb.GetDCFMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DCF, eStr) Then
                Me.ShowErrorMessage(eStr.ToString(), eStr)
            End If

            For Each dr As DataRow In ds_DCF.Tables(0).Rows
                dr("cDCFStatus") = Discrepancy_Resolved
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("iStatusChangedBy") = Me.Session(S_UserID)
            Next dr
            ds_DCF.AcceptChanges()

            If Not objLambda.Save_DCFMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                                    ds_DCF, Me.Session(S_UserID), eStr) Then
                Throw New Exception(eStr)
            End If

            Me.objCommon.ShowAlert("Discrepancy Resolved Successfully.", Me.Page)

            Me.HFnDCFNo.Value = ""

            If Not FillGrid() Then
                Throw New Exception
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnReReview_Click(sender As Object, e As EventArgs) Handles btnReReview.Click
        Dim strCRFDtl As String = String.Empty
        Dim chkbox As System.Web.UI.WebControls.CheckBox
        Dim ds_CRFWorkFlowDtl As DataSet = Nothing
        Dim ds_CRFWorkFlowDtl_Temp As DataSet = Nothing
        Dim ds_CRFDtl As DataSet = Nothing
        Dim ds_Grid As DataSet = Nothing
        Dim dr As DataRow
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim cIsChkAll As Char = "Y"
        Dim dFromDate As String = String.Empty
        Dim dToDate As String = Today.Date.AddDays(1).ToString("dd-MMM-yyyy")
        Dim cStatus As String = String.Empty
        Dim cDCFType As String = String.Empty
        Dim vCreatedBy As String = String.Empty
        Dim vDataEntryBy As String = String.Empty
        Dim iPeriod As Integer = -1
        Dim str As String = String.Empty
        Dim isExit As Boolean = False
        Dim isAnswered As Boolean = False
        Dim isNew As Boolean = False
        Dim ds_review As New DataSet
        Dim dv_review As New DataView
        Dim iworkflow As Integer = 0
        Dim ds_reviewer As New DataSet
        Dim dv_reviewer As DataView

        Dim strActivityId As String = String.Empty
        Dim Resolvedby As String = String.Empty
        Try

            If Not Me.objHelpDb.GetCRFWorkFlowDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                ds_CRFWorkFlowDtl_Temp, eStr) Then
                Throw New Exception(eStr)
            End If
            ds_CRFWorkFlowDtl_Temp.Tables(0).Clear()
            ds_CRFWorkFlowDtl_Temp.AcceptChanges()
            If Me.DdlPeriod.SelectedIndex > 0 Then
                iPeriod = Me.DdlPeriod.SelectedValue.ToString()
            End If
            ''Added By nipun khant for dynamic review
            ds_review = CType(Me.Session(Vs_dsReviewerlevel), DataSet).Copy()
            dv_review = ds_review.Tables(0).Copy.DefaultView
            dv_review.RowFilter = "iReviewWorkflowStageId = " + Session(S_WorkFlowStageId).ToString + " AND vUserTypeCode = '" + Session(S_UserType) + "'"
            If dv_review.ToTable.Rows.Count > 0 Then
                iworkflow = Convert.ToInt32(dv_review.ToTable.Rows(0)("iActualWorkflowStageId"))
            End If
            '==============================================================
            For iLoop = 0 To Me.gvRereview.Rows.Count - 1

                chkbox = CType(gvRereview.Rows(iLoop).Cells(GVC3_SrNo).FindControl("chkbxRRCell"), System.Web.UI.WebControls.CheckBox)

                If chkbox.Checked Then

                    wStr = "nCRfDtlNo = " + Me.gvRereview.Rows(iLoop).Cells(GVC3_nCRFDtlNo).Text
                    If Not Me.objHelpDb.GetCRFWorkFlowDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                           ds_CRFWorkFlowDtl, eStr) Then
                        Throw New Exception(eStr)
                    End If
                    ds_CRFWorkFlowDtl.Tables(0).DefaultView.RowFilter = "iTranNo = Max(iTranNo)"
                    If ds_CRFWorkFlowDtl.Tables(0).DefaultView(0)("iWorkFlowStageId").ToString <> Convert.ToString(iworkflow) Then
                        str = HProjectId.Value.ToString + "##" + gvRereview.Rows(iLoop).Cells(GVC3_SubjectId).Text.ToString + "##" + cIsChkAll.ToString + "##" + dFromDate.ToString + "##" + dToDate.ToString + "##" + cStatus.ToString + "##" + cDCFType.ToString + "##" + vCreatedBy.ToString + "##" + vDataEntryBy.ToString + "##" + iPeriod.ToString
                        ' parameter added by prayag
                        str += "##" + strActivityId.ToString + "##" + Resolvedby.ToString
                        ds_Grid = objHelpDb.ProcedureExecute("Proc_DiscrepancyStatusReport", str)
                        If ds_Grid.Tables(0).Rows.Count > 0 Then
                            ds_Grid.Tables(0).DefaultView.RowFilter = "nCRfDtlNo =" + Me.gvRereview.Rows(iLoop).Cells(GVC3_nCRFDtlNo).Text
                            isExit = False
                            For Each drDCF As DataRow In ds_Grid.Tables(0).DefaultView().ToTable().Rows
                                If drDCF("Status2").ToString.ToUpper = "ANSWERED" Then
                                    isAnswered = True
                                    isExit = True
                                    Exit For

                                ElseIf drDCF("cDCFStatus") = Discrepancy_Generated Or (drDCF("cDCFStatus") = Discrepancy_Answered AndAlso drDCF("iWorkFlowStageId") <> (iworkflow - 10)) Then
                                    isNew = True
                                    isExit = True
                                    Exit For
                                End If
                            Next
                            If isExit <> True Then

                                If strCRFDtl = "" Then
                                    strCRFDtl = Me.gvRereview.Rows(iLoop).Cells(GVC3_nCRFDtlNo).Text
                                Else
                                    strCRFDtl = strCRFDtl + "," + Me.gvRereview.Rows(iLoop).Cells(GVC3_nCRFDtlNo).Text
                                End If

                                dr = ds_CRFWorkFlowDtl_Temp.Tables(0).NewRow()
                                dr("nCRFWorkFlowNo") = 0
                                dr("nCRFDtlNo") = gvRereview.Rows(iLoop).Cells(GVC3_nCRFDtlNo).Text.ToString
                                dr("iTranNo") = 0
                                dr("iWorkFlowStageId") = Me.Session(S_WorkFlowStageId)
                                dr("iModifyBy") = Me.Session(S_UserID)
                                dr("cStatusIndi") = "N"
                                dr("cReviewFlag") = "M"
                                ds_CRFWorkFlowDtl_Temp.Tables(0).Rows.Add(dr)
                                ds_CRFWorkFlowDtl_Temp.AcceptChanges()
                            End If
                        End If
                    Else

                        Continue For
                    End If

                End If
            Next iLoop
            If isExit = True Then
                If ds_CRFWorkFlowDtl_Temp.Tables(0).Rows.Count = 0 Then
                    If isAnswered Then
                        objCommon.ShowAlert("Discrepancy pending, You can not re-review.", Me.Page)
                    Else
                        objCommon.ShowAlert("Discrepancy pending, You can not re-review.", Me.Page)
                    End If

                End If
            End If
            If ds_CRFWorkFlowDtl_Temp.Tables(0).Rows.Count > 0 Then
                If Not Me.objLambda.Save_CRFWorkFlowDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                    ds_CRFWorkFlowDtl_Temp, Me.Session(S_UserID), eStr) Then
                    Throw New Exception(eStr)
                End If

                If strCRFDtl <> "" Then
                    wStr = "nCRFDtlNo IN (" + strCRFDtl.ToString + ") And cStatusIndi <> 'D'"

                    If Not Me.objHelpDb.GetCRFDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        ds_CRFDtl, eStr) Then
                        Throw New Exception(eStr)
                    End If

                    If Not ds_CRFDtl Is Nothing AndAlso Not ds_CRFDtl.Tables(0) Is Nothing AndAlso ds_CRFDtl.Tables(0).Rows.Count > 0 Then
                        For index = 0 To Split(strCRFDtl, ",").Length - 1
                            ds_CRFDtl.Tables(0).DefaultView.RowFilter = "nCRFDtlNo = " + Split(strCRFDtl, ",")(index).ToString
                            If ds_CRFDtl.Tables(0).DefaultView.Count > 0 Then
                                ds_CRFDtl.Tables(0).DefaultView(0)("cDataStatus") = CRF_ReviewCompleted
                                ds_CRFDtl.Tables(0).DefaultView(0)("iWorkFlowStageId") = Me.Session(S_WorkFlowStageId)
                                ds_CRFDtl.Tables(0).DefaultView(0)("iModifyBy") = Me.Session(S_UserID)
                                ds_CRFDtl.Tables(0).DefaultView(0)("dModifyon") = DateTime.Now()             ''Added by Rahul Rupareliya  for Audit Trail Changes
                                ds_CRFDtl.Tables(0).DefaultView.ToTable().AcceptChanges()
                            End If
                        Next

                        If Not Me.objLambda.Save_CRFDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                                ds_CRFDtl, Me.Session(S_UserID), eStr) Then
                            Throw New Exception(eStr)
                        End If
                        objCommon.ShowAlert("Re-review Completed Successfully.", Me.Page)
                    End If
                End If
            End If


            If Not FillViewState() Then
                Exit Sub
            End If
            If Not FillActivity() Then
                Exit Sub
            End If
            If Not FillRereviewSubject() Then
                Exit Sub
            End If
            If Not FillRereviewGrid() Then
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....btnReReview_Click")
        End Try
    End Sub

    Protected Sub btnRereviewGo_Click(sender As Object, e As EventArgs) Handles btnRereviewGo.Click
        Try

            If Not Me.FillRereviewGrid() Then
                Exit Sub
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnDCFGo_Click(sender As Object, e As EventArgs) Handles btnDCFGo.Click
        Try
            If Not Me.FillDCFGrid() Then
                Exit Sub
            End If
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub btnRereviewExportToExcel_Click(sender As Object, e As EventArgs)
        Dim dt As DataTable = Nothing
        Dim FileName As String = String.Empty
        Dim style = "<style> .StyleAsText { mso-number-format:\@; } </style><style>.text{ mso-number-format:\@;}</style> "
        Dim SrNo As Integer = 1
        Try

            dt = CType(ViewState(VS_RereviewExporttoExcel), DataTable)
            dt.Columns.Add("SrNo")
            For Each dr In dt.Rows
                dr("SrNo") = SrNo
                SrNo += 1
            Next
            dt = dt.DefaultView.ToTable(True, "SrNo", "Site", "Period", "SubjectID", "ScreenNo", "RandomizationNo", "Visit", "Activity", "RepetitionNo", "PendingRereviewLevel")
            dt.Columns(0).ColumnName = "Sr No."
            dt.Columns(1).ColumnName = "Project"
            dt.Columns(2).ColumnName = "Period"
            dt.Columns(3).ColumnName = "Subejct Id"
            dt.Columns(4).ColumnName = "Screen No."
            dt.Columns(5).ColumnName = "Patient / Randomizationno."
            dt.Columns(6).ColumnName = "Parent Activity / Visit"
            dt.Columns(7).ColumnName = "Activity"
            dt.Columns(8).ColumnName = "Repetition No."
            dt.Columns(9).ColumnName = "Pending Re-review Level"
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then

                Context.Response.Clear()
                Context.Response.Buffer = True
                Context.Response.ClearContent()
                Context.Response.ClearHeaders()
                FileName = "Re-review Report"
                Context.Response.AppendHeader("Content-type", "application/vnd.ms-excel")
                Context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName + ".xls")
                Context.Response.Write(style)
                Context.Response.Write(ReportDetail(dt).ToString().Replace("<td>", "<td class='StyleAsText'>"))
                Context.Response.Flush()
                Context.Response.End()
            Else
                objCommon.ShowAlert("No Data Found.", Me.Page)
            End If
        Catch ex As Exception
            objCommon.ShowAlert(ex.Message.ToString() + "...btnExporttoExcel_Click", Me.Page)
        End Try
    End Sub

    Protected Sub btnAuthenticate_Click(sender As Object, e As EventArgs) Handles btnAuthenticate.Click
        Dim Pwd As String = String.Empty
        Try
            If CType(sender, Button).ID = "btnAuthenticate" Then
                Pwd = Me.hdnPassword.Value.Trim()
                Pwd = objHelpDb.EncryptPassword(Pwd)

                If Pwd.ToUpper() <> CType(Me.Session(S_Password), String).ToUpper() Then
                    objCommon.ShowAlert("Password Authentication Fails.", Me.Page)
                    'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "ValidationForAuthentication();", True)
                    Me.txtPassword.Text = ""
                    Me.txtPassword.Focus()
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "DivAuthenticationHideShow('S');", True)
                    Exit Sub
                End If
            End If
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "HideDiv", "DivAuthenticationHideShow('H');", True)
            btnReReview_Click(sender, e)
            If Not FillRereviewGrid() Then
                Exit Sub
            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "Selected index change"

    Protected Sub DdlPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlPeriod.SelectedIndexChanged
        Try
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "fldgrdfalse", "DivfieldShowHide('Q');", True)
            If chkDCF.Checked = True Then
                'Fillsubjects() '' comment by prayag

                If Not BindSubjectTree() Then ' added by prayag
                    Exit Sub
                End If

                If Not BindActivityTree() Then ' added by prayag
                    Exit Sub
                End If

                FillDCFViewState()
                gvwDCF.DataSource = Nothing
                gvwDCF.DataBind()
                Me.lblCount.Text = ""
                Exit Sub
            End If

            If chkRereview.Checked Then
                If Not FillViewState() Then
                    Exit Sub
                End If
                If Not FillActivity() Then
                    Exit Sub
                End If
                If Not FillRereviewSubject() Then
                    Exit Sub
                End If
            Else
                'Me.Fillsubjects() ''comment by prayag 
                If Not BindSubjectTree() Then ' added by prayag
                    Exit Sub
                End If
                If Not BindActivityTree() Then ' added by prayag
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub ddlStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlStatus.SelectedIndexChanged
        FillGrid()
    End Sub

    Protected Sub ddlDataEntryBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDataEntryBy.SelectedIndexChanged
        FillGrid()
    End Sub

    Protected Sub ddlCreatedBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCreatedBy.SelectedIndexChanged
        FillGrid()
    End Sub

    Protected Sub ddlDiscrepancyType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDiscrepancyType.SelectedIndexChanged
        FillGrid()
    End Sub

    Protected Sub ddlDCFTypes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDCFTypes.SelectedIndexChanged
        FillDCFGrid()
    End Sub

    Protected Sub ddlGeneratedBy_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlGeneratedBy.SelectedIndexChanged
        FillDCFGrid()
    End Sub

    Protected Sub ddlType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlType.SelectedIndexChanged
        ' develope by prayag
        Dim eStr As String = String.Empty
        Try
            If Not Me.HProjectId.Value.Trim.ToString = "" Then
                Me.tvSubject.Nodes.Clear()
                Me.tvActivity.Nodes.Clear()

                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "fldgrdfalse", "DivfieldShowHide('Q');", True)
                Me.fldgrdParent.Style.Add("display", "none")
                Me.divSubject.Style.Add("display", "none")
                Me.divActivity.Style.Add("display", "none")
                If Me.ddlType.SelectedIndex <> 0 Then
                    If Not BindSubjectTree() Then
                        Exit Sub
                    End If
                    If Not BindActivityTree() Then
                        Exit Sub
                    End If
                    'If Me.ddlType.SelectedIndex = 1 Then
                    '    Me.divSubject.Style.Add("display", "block")
                    'End If

                    Me.divActivity.Style.Add("display", "block")

                End If

            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".............ddlType_SelectedIndexChanged")
        End Try
    End Sub

    Protected Sub ddlResolvedBy_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlResolvedBy.SelectedIndexChanged ' added by prayag
        FillGrid()
    End Sub

#End Region

#Region "Grid Events"

    Protected Sub GVWDSR_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVWDSR.RowCreated
        'edited by vishal for lock/unlock to hide edit link
        Dim dv As New DataView
        dv.Table = ViewState(vs_gLock)

        If e.Row.RowType = DataControlRowType.DataRow Or _
                e.Row.RowType = DataControlRowType.Header Or _
                e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GVC_nDCFNo).Visible = False
            e.Row.Cells(GVC_SubjectName).Visible = False
            e.Row.Cells(GVC_Period).Visible = False
            e.Row.Cells(GVC_SubjectId).Visible = False
            e.Row.Cells(GVC_Site).Visible = False

            e.Row.Cells(GVC_ActivityId).Visible = False
            e.Row.Cells(GVC_NodeId).Visible = False
            e.Row.Cells(GVC_iMySubjectNo).Visible = False
            e.Row.Cells(GVC_vUserTypeCode).Visible = False
            e.Row.Cells(GVC_cDCFStatus).Visible = False
            e.Row.Cells(GVC_cDCFType).Visible = False
            e.Row.Cells(GVC_iWorkFlowStageId).Visible = False
            e.Row.Cells(GVC_nCRFDtlNo).Visible = False
            e.Row.Cells(GVC_ReReview).Visible = False

            If Not dv.Table Is Nothing AndAlso dv.ToTable().Rows.Count > 0 Then
                If Convert.ToString(dv.ToTable().Rows(0)("cLockFlag")).Trim.ToUpper() = "L" Then
                    e.Row.Cells(GVC_Edit).Visible = False
                    e.Row.Cells(GVC_Resolve).Visible = False
                End If

            End If

            If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then

                e.Row.Cells(GVC_Visit).Visible = False
                e.Row.Cells(GVC_Period).Visible = True
                e.Row.Cells(GVC_SubjectId).Visible = True
                e.Row.Cells(GVC_RandomizationNo).Visible = False

                If e.Row.RowType = DataControlRowType.Header Then
                    'e.Row.Cells(GVC_ScreenNo).Text = "My Subject No"
                    'e.Row.Cells(GVC_ScreenNo).Text = "SubejctNo"
                End If

            End If

        End If
    End Sub

    Protected Sub GVWDSR_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVWDSR.RowDataBound
        Dim chkBox As System.Web.UI.WebControls.CheckBox
        Dim ds_reviewer As New DataSet
        Dim dv_review As DataView
        Dim iworkflow As Integer = 0
        Dim dt_DCF As New DataTable
        Dim strCellValue As String = ""
        Try
            If (Me.ViewState(VS_choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View) Then
                If Not Me.Request.QueryString("vWorkSpaceId") Is Nothing Then
                    If e.Row.RowType <> DataControlRowType.Pager Then
                        e.Row.Cells(GVC_Edit).Visible = False
                        e.Row.Cells(GVC_Resolve).Visible = False
                    End If
                End If
            End If
            If e.Row.RowType = DataControlRowType.DataRow Then
                dt_DCF = DirectCast(Me.ViewState(VS_DtGVWDCF), DataTable)
                '                e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (GVWDSR.PageSize * GVWDSR.PageIndex) + 1
                e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + 1

                'Add by shivani pandya for Update remarks

                If dt_DCF.Rows(e.Row.RowIndex)("Status").ToString = "ANSWERED" Then
                    e.Row.Cells(GVC_UpdateRemarks).Text = ""
                End If

                If Not Convert.ToString(Replace(e.Row.Cells(GVC_DataEntryOn).Text.Trim(), "&nbsp;", "")) = "" Then
                    e.Row.Cells(GVC_DataEntryOn).Text = CDate(e.Row.Cells(GVC_DataEntryOn).Text).ToString("dd-MMM-yyyy HH:mm") + strServerOffset
                End If

                If Not Convert.ToString(Replace(e.Row.Cells(GVC_UpdatedOn).Text.Trim(), "&nbsp;", "")) = "" Then
                    e.Row.Cells(GVC_UpdatedOn).Text = CDate(e.Row.Cells(GVC_UpdatedOn).Text).ToString("dd-MMM-yyyy HH:mm") + strServerOffset
                End If

                If Not Convert.ToString(Replace(e.Row.Cells(GVC_CreatedDate).Text.Trim(), "&nbsp;", "")) = "" Then
                    e.Row.Cells(GVC_CreatedDate).Text = CDate(e.Row.Cells(GVC_CreatedDate).Text).ToString("dd-MMM-yyyy HH:mm") + strServerOffset
                End If

                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "EDIT"
                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex

                'Dim Visit As New StreamReader(e.Row.Cells(GVC_Visit).Text.Trim(), System.Text.Encoding.Default)
                Dim vist As String = HttpUtility.UrlEncode(e.Row.Cells(GVC_Visit).Text.Trim())

                Dim RedirectStr As String = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" & Me.HProjectId.Value.Trim() & _
                                        "&ActivityId=" & e.Row.Cells(GVC_ActivityId).Text.Trim() & _
                                        "&NodeId=" & e.Row.Cells(GVC_NodeId).Text.Trim() & _
                                        "&PeriodId=" & e.Row.Cells(GVC_Period).Text.Trim() & _
                                        "&SubjectId=" & e.Row.Cells(GVC_SubjectId).Text.Trim() & _
                                        "&MySubjectNo=" & e.Row.Cells(GVC_iMySubjectNo).Text.Trim() & _
                                        "&Activityname=" & Convert.ToString(vist) & _
                                        "&Repeatation=" & e.Row.Cells(GVC_Repeatation).Text.Trim() & _
                                        "&ScreenNo=" & e.Row.Cells(GVC_ScreenNo).Text.Trim()
                'Rahul RUpaeliya

                CType(e.Row.FindControl("lnkEdit"), ImageButton).OnClientClick = "return OpenWindow('" + RedirectStr + "');"

                e.Row.Cells(GVC_Resolve).FindControl("lnkResolve").Visible = False
                ''added by nipun khant for dynamic review
                ds_reviewer = CType(Me.Session(Vs_dsReviewerlevel), DataSet).Copy
                dv_review = ds_reviewer.Tables(0).Copy.DefaultView
                dv_review.RowFilter = "iReviewWorkflowStageId = " + Session(S_WorkFlowStageId).ToString + " AND vUserTypeCode = '" + Session(S_UserType) + "'"
                If dv_review.ToTable.Rows.Count > 0 Then
                    iworkflow = Convert.ToInt32(dv_review.ToTable.Rows(0)("iActualWorkflowStageId"))
                End If
                ''=============================================
                If e.Row.Cells(GVC_cDCFType).Text.Trim.ToUpper() = "M" AndAlso _
                            e.Row.Cells(GVC_vUserTypeCode).Text.Trim() = Me.Session(S_UserType) AndAlso _
                                ((e.Row.Cells(GVC_cDCFStatus).Text.Trim.ToUpper() = Discrepancy_Answered And e.Row.Cells(GVC_Status).Text.Trim.ToUpper() <> Discrepancy_ReReview.ToUpper()) Or _
                                 e.Row.Cells(GVC_cDCFStatus).Text.Trim.ToUpper() = Discrepancy_Generated) Then
                    e.Row.Cells(GVC_Resolve).FindControl("lnkResolve").Visible = True
                    'added by nipun khant for dynamic review
                    If e.Row.Cells(GVC_iWorkFlowStageId).Text < (iworkflow - 10).ToString() Then
                        e.Row.Cells(GVC_Resolve).FindControl("lnkResolve").Visible = False
                    End If
                    ''commented by nipun khant for dynamic review
                    'If e.Row.Cells(GVC_iWorkFlowStageId).Text < (Me.Session(S_WorkFlowStageId) - 10).ToString() Then
                    '    e.Row.Cells(GVC_Resolve).FindControl("lnkResolve").Visible = False
                    'End If
                End If

                CType(e.Row.Cells(GVC_Resolve).FindControl("lnkResolve"), LinkButton).OnClientClick = "return ShowConfirmation(" & e.Row.Cells(GVC_nDCFNo).Text.Trim.ToString() & ");"
                chkBox = CType(e.Row.Cells(GVC_ReReview).FindControl("chkbxRRCell"), System.Web.UI.WebControls.CheckBox)
                ''Added by nipun khant for dynamic review
                If e.Row.Cells(GVC_cDCFType).Text.Trim.ToUpper() = "M" AndAlso _
                            (Convert.ToInt32(e.Row.Cells(GVC_iWorkFlowStageId).Text) + 10) = Convert.ToString(iworkflow) AndAlso _
                            e.Row.Cells(GVC_Status).Text.Trim.ToUpper() = Discrepancy_ReReview.ToUpper() Then
                    chkBox.Visible = True
                Else
                    chkBox.Visible = False
                End If

                ''commented by nipun khant for dynamic review
                'If e.Row.Cells(GVC_cDCFType).Text.Trim.ToUpper() = "M" AndAlso _
                '            (Convert.ToInt32(e.Row.Cells(GVC_iWorkFlowStageId).Text) + 10) = Me.Session(S_WorkFlowStageId).ToString() AndAlso _
                '            e.Row.Cells(GVC_Status).Text.Trim.ToUpper() = Discrepancy_ReReview.ToUpper() Then
                '    chkBox.Visible = True
                'Else
                '    chkBox.Visible = False
                'End If
            End If

            '' Added By ketan
            If e.Row.RowType = DataControlRowType.DataRow Then
                For DataRowIndex As Integer = 0 To e.Row.Cells.Count - 1
                    strCellValue = e.Row.Cells(DataRowIndex).Text
                    If strCellValue <> "" Then
                        If strCellValue.Length > 20 Then
                            e.Row.Cells(DataRowIndex).Attributes.Add("title", strCellValue)
                            e.Row.Cells(DataRowIndex).Text = strCellValue.Substring(0, 20) + "..."
                        Else
                            e.Row.Cells(DataRowIndex).Text = strCellValue
                        End If
                    End If
                Next DataRowIndex
            End If
            ''Ended by ketan

            If e.Row.RowType = DataControlRowType.Header Or _
                e.Row.RowType = DataControlRowType.DataRow Or _
                e.Row.RowType = DataControlRowType.Footer Then
                'If ddlStatus.SelectedValue.ToString = "E" Then
                '    e.Row.Cells(GVC_ReReview).Visible = True
                'Else
                '    e.Row.Cells(GVC_ReReview).Visible = False
                'End If
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

    Protected Sub GVWDSR_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVWDSR.PageIndexChanging
        GVWDSR.PageIndex = e.NewPageIndex
        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub

    Protected Sub GVWODMSR_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVWODMSR.RowCreated
        Dim dv As New DataView
        dv.Table = ViewState(vs_gLock)
        If e.Row.RowType = DataControlRowType.DataRow Or _
                e.Row.RowType = DataControlRowType.Header Or _
                e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GVC2_SrNo).Visible = True
            e.Row.Cells(GVC2_Activity).Visible = True
            'e.Row.Cells(GVC2_ScreenNo).Visible = True
            'e.Row.Cells(GVC2_RandomizationNo).Visible = True
            e.Row.Cells(GVC2_Repeatation).Visible = True
            e.Row.Cells(GVC2_Attribute).Visible = True
            e.Row.Cells(GVC2_Value).Visible = True
            e.Row.Cells(GVC2_UpdatedValue).Visible = True
            e.Row.Cells(GVC2_UpdationRemarks).Visible = True
            e.Row.Cells(GVC2_UpdatedBy).Visible = True
            e.Row.Cells(GVC2_UpdatedOn).Visible = True
            e.Row.Cells(GVC2_DataEntryBy).Visible = True
            e.Row.Cells(GVC2_DataEntryOn).Visible = True
            e.Row.Cells(GVC2_Edit).Visible = True
            e.Row.Cells(GVC2_ActivityId).Visible = False
            e.Row.Cells(GVC2_NodeId).Visible = False
            e.Row.Cells(GVC2_iMySubjectNo).Visible = False
            e.Row.Cells(GVC2_Period).Visible = False
            'e.Row.Cells(GVC2_SubjectId).Visible = False
            e.Row.Cells(GVC2_vProjectTypeCode).Visible = False



            If Not dv.Table Is Nothing AndAlso dv.ToTable().Rows.Count > 0 Then
                If Convert.ToString(dv.ToTable().Rows(0)("cLockFlag")).Trim.ToUpper() = "L" Then
                    e.Row.Cells(GVC2_Edit).Visible = False
                End If

            End If

            If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then

                e.Row.Cells(GVC2_Period).Visible = True
                e.Row.Cells(GVC2_SubjectId).Visible = True
                'e.Row.Cells(GVC2_RandomizationNo).Visible = False

                'If e.Row.RowType = DataControlRowType.Header Then
                '    e.Row.Cells(GVC2_ScreenNo).Text = "My Subject No"
                'End If
            Else
                e.Row.Cells(GVC2_SubjectId).Visible = False
            End If

        End If
    End Sub

    Protected Sub GVWODMSR_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVWODMSR.RowDataBound
        Dim strCellValue As String = ""
        Try
            If (Me.ViewState(VS_choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View) Then
                If Not Me.Request.QueryString("vWorkSpaceId") Is Nothing Then
                    If e.Row.RowType <> DataControlRowType.Pager Then
                        e.Row.Cells(GVC2_Edit).Visible = False
                        'e.Row.Cells(GVC_Resolve).Visible = False
                    End If
                End If
            End If
            If e.Row.RowType = DataControlRowType.DataRow Then
                'e.Row.Cells(GVC2_SrNo).Text = e.Row.RowIndex + (GVWODMSR.PageSize * GVWODMSR.PageIndex) + 1
                e.Row.Cells(GVC2_SrNo).Text = e.Row.RowIndex + 1

                CType(e.Row.FindControl("lnkEdit2"), ImageButton).CommandName = "EDIT"
                CType(e.Row.FindControl("lnkEdit2"), ImageButton).CommandArgument = e.Row.RowIndex

                Dim vist As String = HttpUtility.UrlEncode(e.Row.Cells(GVC2_Parent).Text.Trim())

                Dim RedirectStr As String = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" & Me.HProjectId.Value.Trim() & _
                                        "&ActivityId=" & e.Row.Cells(GVC2_ActivityId).Text.Trim() & _
                                        "&NodeId=" & e.Row.Cells(GVC2_NodeId).Text.Trim() & _
                                        "&PeriodId=" & e.Row.Cells(GVC2_Period).Text.Trim() & _
                                        "&SubjectID=" & e.Row.Cells(GVC2_SubjectId).Text.Trim() & _
                                        "&MySubjectNo=" & e.Row.Cells(GVC2_iMySubjectNo).Text.Trim() & _
                                         "&Activityname=" & Convert.ToString(vist) & _
                                         "&Repeatation=" & e.Row.Cells(GVC2_Repeatation).Text.Trim() & _
                                        "&ScreenNo=" & e.Row.Cells(GVC2_ScreenNo).Text.Trim()

                'Rahul Rupaeliya
                CType(e.Row.FindControl("lnkEdit2"), ImageButton).OnClientClick = "return OpenWindow('" + RedirectStr + "');"

                'Added By Anuj to distinguish between CT and BA-BE projects (ProjectTypeMst)
                If e.Row.Cells(GVC2_vProjectTypeCode).Text.Trim.ToUpper() = "0002" Then     'For BA-BE Projects
                    'e.Row.Cells(GVC2_SubjectId).Visible = True
                    'e.Row.Cells(GVC2_iMySubjectNo).Visible = True
                    'e.Row.Cells(GVC2_ScreenNo).Visible = False
                    'e.Row.Cells(GVC2_RandomizationNo).Visible = False
                    GVWODMSR.Columns(GVC2_ScreenNo).Visible = False
                    GVWODMSR.Columns(GVC2_RandomizationNo).Visible = False
                    'GVWODMSR.Columns(GVC2_SubjectId).Visible = True
                    'GVWODMSR.Columns(5).Visible = True
                ElseIf e.Row.Cells(GVC2_vProjectTypeCode).Text.Trim.ToUpper() = "0014" Then    'For CT Projects
                    'e.Row.Cells(GVC2_SubjectId).Visible = False
                    'e.Row.Cells(GVC2_iMySubjectNo).Visible = False
                    'e.Row.Cells(GVC2_ScreenNo).Visible = True
                    'e.Row.Cells(GVC2_RandomizationNo).Visible = True
                    GVWODMSR.Columns(GVC2_ScreenNo).Visible = True
                    GVWODMSR.Columns(GVC2_RandomizationNo).Visible = True

                    'GVWODMSR.Columns(GVC2_SubjectId).Visible = False

                    ' GVWODMSR.Columns(5).Visible = False
                End If
                If Not Convert.ToString(e.Row.Cells(GVC2_UpdatedOn).Text).Trim = "" Then
                    e.Row.Cells(GVC2_UpdatedOn).Text = CType(Replace(e.Row.Cells(GVC2_UpdatedOn).Text.Trim(), "&nbsp;", ""), Date).ToString("dd-MMM-yyyy HH:mm").Trim() + strServerOffset
                    'GVWODMSR.Columns(GVC2_UpdatedOn).ItemStyle.Width = 350
                End If
                If Not Convert.ToString(e.Row.Cells(GVC2_DataEntryOn).Text).Trim = "" Then
                    e.Row.Cells(GVC2_DataEntryOn).Text = CType(Replace(e.Row.Cells(GVC2_DataEntryOn).Text.Trim(), "&nbsp;", ""), Date).ToString("dd-MMM-yyyy HH:mm").Trim() + strServerOffset
                    'GVWODMSR.Columns(GVC2_DataEntryOn).ItemStyle.Width = 350
                    'e.Row.Cells(GVC2_DataEntryOn).Width = "400px"
                End If

                '' Added By ketan
                If e.Row.RowType = DataControlRowType.DataRow Then
                    For DataRowIndex As Integer = 0 To e.Row.Cells.Count - 1
                        strCellValue = e.Row.Cells(DataRowIndex).Text
                        If strCellValue <> "" Then
                            If strCellValue.Length > 20 Then
                                e.Row.Cells(DataRowIndex).Attributes.Add("title", strCellValue)
                                e.Row.Cells(DataRowIndex).Text = strCellValue.Substring(0, 20) + "..."
                            Else
                                e.Row.Cells(DataRowIndex).Text = strCellValue
                            End If
                        End If
                    Next DataRowIndex
                End If
                ''Ended by ketan

            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

    Protected Sub GVWODMSR_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVWODMSR.PageIndexChanging
        GVWODMSR.PageIndex = e.NewPageIndex
        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub

    Protected Sub gvRereview_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvRereview.PageIndexChanging
        Try
            gvRereview.PageIndex = e.NewPageIndex
            If Not FillRereviewGrid() Then
                Exit Sub
            End If
        Catch ex As Exception
            objCommon.ShowAlert(ex.Message.ToString() + "...gvRereview_PageIndexChanging", Me.Page)
        End Try

    End Sub

    Protected Sub gvRereview_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvRereview.RowCreated
        Try
            If e.Row.RowType = DataControlRowType.DataRow Or _
                e.Row.RowType = DataControlRowType.Header Or _
                e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(GVC3_ActivityId).Visible = False
                e.Row.Cells(GVC3_NodeId).Visible = False
                e.Row.Cells(GVC3_iMySubjectNo).Visible = False
                e.Row.Cells(GVC3_WorkFloStageId).Visible = False
                e.Row.Cells(GVC3_nCRFDtlNo).Visible = False
                e.Row.Cells(16).Visible = False

            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvRereview_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvRereview.RowDataBound
        Dim ds_review As New DataSet
        Dim dv_review As DataView
        Dim iworkflow As Integer = 0
        Dim strCellValue As String = ""
        Try
            ''added by nipun khant for dynamic review
            ds_review = CType(Me.Session(Vs_dsReviewerlevel), DataSet).Copy
            dv_review = ds_review.Tables(0).Copy.DefaultView
            dv_review.RowFilter = "iReviewWorkflowStageId = " + Session(S_WorkFlowStageId).ToString + " AND vUserTypeCode = '" + Session(S_UserType) + "'"
            If dv_review.ToTable.Rows.Count > 0 Then
                iworkflow = Convert.ToInt32(dv_review.ToTable.Rows(0)("iActualWorkflowStageId"))
            End If
            ''=============================================
            If e.Row.RowType = DataControlRowType.DataRow Or _
                e.Row.RowType = DataControlRowType.Header Or _
                e.Row.RowType = DataControlRowType.Footer Then
                ''Added by nipun khant for dynamic review
                If e.Row.Cells(GVC3_WorkFloStageId).Text.ToString <> (iworkflow - 10).ToString Then
                    CType(e.Row.FindControl("chkbxRRCell"), CheckBox).Visible = False
                End If
                If e.Row.Cells(16).Text.ToString = iworkflow Then
                    CType(e.Row.FindControl("chkbxRRCell"), CheckBox).Visible = False
                End If
                ''commented by nipun khant for dynamic review
                'If e.Row.Cells(GVC3_WorkFloStageId).Text.ToString <> (Session(S_WorkFlowStageId) - 10).ToString Then
                '    CType(e.Row.FindControl("chkbxRRCell"), CheckBox).Visible = False
                'End If
            End If
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "EDIT"
                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex

                Dim RedirectStr As String = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" & Me.HProjectId.Value.Trim() & _
                                        "&ActivityId=" & e.Row.Cells(GVC3_ActivityId).Text.Trim() & _
                                        "&NodeId=" & e.Row.Cells(GVC3_NodeId).Text.Trim() & _
                                        "&PeriodId=" & e.Row.Cells(GVC3_Period).Text.Trim() & _
                                        "&SubjectId=" & e.Row.Cells(GVC3_SubjectId).Text.Trim() & _
                                        "&MySubjectNo=" & e.Row.Cells(GVC3_iMySubjectNo).Text.Trim() & _
                                        "&Repeatation=" & e.Row.Cells(GVC3_RepetitionNo).Text.Trim() & _
                                        "&ScreenNo=" & e.Row.Cells(GVC3_ScreenNo).Text.Trim()

                CType(e.Row.FindControl("lnkEdit"), ImageButton).OnClientClick = "return OpenWindow('" + RedirectStr + "');"
            End If

            '' Added By ketan
            If e.Row.RowType = DataControlRowType.DataRow Then
                For DataRowIndex As Integer = 0 To e.Row.Cells.Count - 1
                    strCellValue = e.Row.Cells(DataRowIndex).Text
                    If strCellValue <> "" Then
                        If strCellValue.Length > 20 Then
                            e.Row.Cells(DataRowIndex).Attributes.Add("title", strCellValue)
                            e.Row.Cells(DataRowIndex).Text = strCellValue.Substring(0, 20) + "..."

                        Else
                            e.Row.Cells(DataRowIndex).Text = strCellValue
                        End If
                    End If
                Next DataRowIndex
            End If
            ''Ended by ketan

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvwDCF_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvwDCF.RowCreated
        Try
            Dim dv As New DataView
            dv.Table = ViewState(vs_gLock)

            If e.Row.RowType = DataControlRowType.DataRow Or _
                    e.Row.RowType = DataControlRowType.Header Or _
                    e.Row.RowType = DataControlRowType.Footer Then
                'e.Row.Cells(GVC4_nDCFNo).Visible = False
                'e.Row.Cells(GVC4_SubjectName).Visible = False
                e.Row.Cells(GVC4_Period).Visible = False
                e.Row.Cells(GVC4_SubjectId).Visible = False
                'e.Row.Cells(GVC4_Site).Visible = False

                e.Row.Cells(GVC4_ActivityId).Visible = False
                e.Row.Cells(GVC4_NodeId).Visible = False
                e.Row.Cells(GVC4_cDCFType).Visible = False
                'e.Row.Cells(GVC4_iMySubjectNo).Visible = False
                'e.Row.Cells(GVC4_vUserTypeCode).Visible = False
                'e.Row.Cells(GVC4_cDCFStatus).Visible = False
                'e.Row.Cells(GVC4_cDCFType).Visible = False
                'e.Row.Cells(GVC4_iWorkFlowStageId).Visible = False
                'e.Row.Cells(GVC4_nCRFDtlNo).Visible = False

                'If Not dv.Table Is Nothing AndAlso dv.ToTable().Rows.Count > 0 Then
                '    If Convert.ToString(dv.ToTable().Rows(0)("cLockFlag")).Trim.ToUpper() = "L" Then
                '        e.Row.Cells(GVC4_Edit).Visible = False
                '        e.Row.Cells(GVC4_Resolve).Visible = False
                '    End If

                'End If

                If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then

                    'e.Row.Cells(GVC4_Visit).Visible = False
                    e.Row.Cells(GVC4_Period).Visible = True
                    e.Row.Cells(GVC4_SubjectId).Visible = True
                    'e.Row.Cells(GVC4_RandomizationNo).Visible = False

                    'If e.Row.RowType = DataControlRowType.Header Then
                    '    e.Row.Cells(GVC4_ScreenNo).Text = "My Subject No"
                    'End If

                End If

            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvwDCF_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvwDCF.RowDataBound
        Dim strCellValue As String = ""
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                'e.Row.Cells(GVC4_SrNo).Text = e.Row.RowIndex + (gvwDCF.PageSize * gvwDCF.PageIndex) + 1
                e.Row.Cells(GVC4_SrNo).Text = e.Row.RowIndex + 1

                If Not Convert.ToString(Replace(e.Row.Cells(GVC4_AnsweredOn).Text.Trim(), "&nbsp;", "")) = "" Then
                    e.Row.Cells(GVC4_AnsweredOn).Text = CDate(e.Row.Cells(GVC4_AnsweredOn).Text).ToString("dd-MMM-yyyy HH:mm") + strServerOffset
                End If

                If Not Convert.ToString(Replace(e.Row.Cells(GVC4_GeneratedDate).Text.Trim(), "&nbsp;", "")) = "" Then
                    e.Row.Cells(GVC4_GeneratedDate).Text = CDate(e.Row.Cells(GVC4_GeneratedDate).Text).ToString("dd-MMM-yyyy HH:mm") + strServerOffset
                End If

                If Not Convert.ToString(Replace(e.Row.Cells(GVC4_ResolvedOn).Text.Trim(), "&nbsp;", "")) = "" Then
                    e.Row.Cells(GVC4_ResolvedOn).Text = CDate(e.Row.Cells(GVC4_ResolvedOn).Text).ToString("dd-MMM-yyyy HH:mm") + strServerOffset
                End If

            End If
            '' Added By ketan
            If e.Row.RowType = DataControlRowType.DataRow Then
                For DataRowIndex As Integer = 0 To e.Row.Cells.Count - 1
                    strCellValue = e.Row.Cells(DataRowIndex).Text
                    If strCellValue <> "" Then
                        If strCellValue.Length > 20 Then
                            e.Row.Cells(DataRowIndex).Attributes.Add("title", strCellValue)
                            e.Row.Cells(DataRowIndex).Text = strCellValue.Substring(0, 20) + "..."

                        Else
                            e.Row.Cells(DataRowIndex).Text = strCellValue
                        End If
                    End If
                Next DataRowIndex
            End If
            ''Ended by ketan
            'If e.Row.RowType = DataControlRowType.Header Or _
            '    e.Row.RowType = DataControlRowType.DataRow Or _
            '    e.Row.RowType = DataControlRowType.Footer Then

            'End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub gvwDCF_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvwDCF.PageIndexChanging
        Try
            gvwDCF.PageIndex = e.NewPageIndex
            If Not FillDCFGrid() Then
                Exit Sub
            End If
        Catch ex As Exception
            objCommon.ShowAlert(ex.Message.ToString() + "...gvwDCF_PageIndexChanging", Me.Page)
        End Try

    End Sub

#End Region

#Region "HideMenu"

    Private Sub HideMenu()
        Dim tmpMenu As Menu
        Dim tmpddlprofile As DropDownList

        tmpMenu = CType(Me.Master.FindControl("menu1"), Menu)
        tmpddlprofile = CType(Me.Master.FindControl("ddlProfile"), DropDownList)

        If Not tmpMenu Is Nothing Then
            tmpMenu.Visible = False
        End If

        If Not tmpddlprofile Is Nothing Then
            tmpddlprofile.Enabled = False
        End If

    End Sub

#End Region

#Region "Export To Excel"

    Private Function ExportToExcel(ByVal ds As DataSet) As String
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer
        Dim SrNo As Integer = 1
        Dim dsConvert As New DataSet

        Try
            If Me.Request.QueryString("mode") = "1" Then
                strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")
                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""13""><center><strong><font color=""black"" size=""9px"" face=""Calibri"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""13""><center><strong><font color=""black"" size=""8px"" face=""Calibri"">")
                strMessage.Append("ODM Status Report")
                strMessage.Append("</font></strong><center></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<tr>")
                strMessage.Append("<td><strong><font color=""black"" size=""9px"" face=""Calibri"">")
                strMessage.Append("Project/Site")
                strMessage.Append("</font></strong></td>")
                strMessage.Append("<td colspan=""12""><strong><font color=""black"" size=""9px"" face=""Calibri"">")
                strMessage.Append(Me.txtproject.Text.Trim())
                strMessage.Append("</font></strong></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""13""><strong><font color=""black"" size=""9px"" face=""Calibri"">")
                strMessage.Append(Me.lblCount.Text.Trim())
                strMessage.Append("</font></strong></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<tr>")


                ds.Tables(0).Columns.Add("SrNo")
                ds.Tables(0).Columns.Add("vUpdatedDate")
                ds.Tables(0).Columns.Add("vDataEntryDate")
                For Each dr In ds.Tables(0).Rows
                    If dr("UpdatedDate").ToString() <> "" Then
                        dr("vUpdatedDate") = Convert.ToString(CDate(dr("UpdatedDate")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                    End If
                    If dr("DataEntryDate").ToString() <> "" Then
                        dr("vDataEntryDate") = Convert.ToString(CDate(dr("DataEntryDate")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                    End If
                    dr("SrNo") = SrNo
                    SrNo += 1
                Next

                If ds.Tables(0).Rows(0)("vProjectTypeCode") = "0002" Then
                    dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "SrNo,Activity,SubjectID,iMySubjectNo,Repeatation,Attribute,Value,LastUpdatedValue,vModificationRemark,UpdatedBy,vUpdatedDate,DataEntryBy,vDataEntryDate".Split(",")).DefaultView.ToTable())
                    dsConvert.AcceptChanges()
                    dsConvert.Tables(0).Columns(0).ColumnName = "Sr. No"
                    dsConvert.Tables(0).Columns(1).ColumnName = "Activity"
                    dsConvert.Tables(0).Columns(2).ColumnName = "Subject Id"
                    dsConvert.Tables(0).Columns(3).ColumnName = "My Subject No."
                    dsConvert.Tables(0).Columns(4).ColumnName = "Repetition No."
                    dsConvert.Tables(0).Columns(5).ColumnName = "Attribute"
                    dsConvert.Tables(0).Columns(6).ColumnName = "Old Value"
                    dsConvert.Tables(0).Columns(7).ColumnName = "Modified Value"
                    dsConvert.Tables(0).Columns(8).ColumnName = "Modification Remarks"
                    dsConvert.Tables(0).Columns(9).ColumnName = "Modified By"
                    dsConvert.Tables(0).Columns(10).ColumnName = "Modified Date"
                    dsConvert.Tables(0).Columns(11).ColumnName = "Data Entry By"
                    dsConvert.Tables(0).Columns(12).ColumnName = "Data Entry Date"

                    dsConvert.AcceptChanges()


                    For iCol = 0 To dsConvert.Tables(0).Columns.Count - 1

                        strMessage.Append("<td style=""text-align:center;""><strong><font color=""Black"" size=""9px"" face=""Calibri"">")
                        strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).Trim())
                        strMessage.Append("</font></strong></td>")

                    Next
                    strMessage.Append("</tr>")


                    For j = 0 To dsConvert.Tables(0).Rows.Count - 1
                        strMessage.Append("<tr>")

                        For i = 0 To dsConvert.Tables(0).Columns.Count - 1
                            If i = 0 Then
                                strMessage.Append("<td style=""text-align:center;vertical-align:top;""><font color=""black"" size=""2"" face=""Calibri"">")
                            Else
                                strMessage.Append("<td style=""text-align:left;vertical-align:top;""><font color=""black"" size=""2"" face=""Calibri"">")
                            End If
                            strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim())
                            strMessage.Append("</font></td>")

                        Next
                        strMessage.Append("</tr>")
                    Next
                    strMessage.Append("</table>")

                    Return strMessage.ToString

                ElseIf ds.Tables(0).Rows(0)("vProjectTypeCode") = "0014" Then



                    dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "SrNo,Activity,ScreenNo,RandomizationNo,RepetitionNo,Attribute,Value,LastUpdatedValue,vModificationRemark,UpdatedBy,vUpdatedDate,DataEntryBy,vDataEntryDate".Split(",")).DefaultView.ToTable())
                    dsConvert.AcceptChanges()
                    dsConvert.Tables(0).Columns(0).ColumnName = "Sr. No"
                    dsConvert.Tables(0).Columns(1).ColumnName = "Activity"
                    dsConvert.Tables(0).Columns(2).ColumnName = "Screen No."
                    dsConvert.Tables(0).Columns(3).ColumnName = "Patient/Randomization No."
                    dsConvert.Tables(0).Columns(4).ColumnName = "Repetition No."
                    dsConvert.Tables(0).Columns(5).ColumnName = "Attribute"
                    dsConvert.Tables(0).Columns(6).ColumnName = "Old Value"
                    dsConvert.Tables(0).Columns(7).ColumnName = "Modified Value"
                    dsConvert.Tables(0).Columns(8).ColumnName = "Modification Remarks"
                    dsConvert.Tables(0).Columns(9).ColumnName = "Modified By"
                    dsConvert.Tables(0).Columns(10).ColumnName = "Modified Date"
                    dsConvert.Tables(0).Columns(11).ColumnName = "Data Entry By"
                    dsConvert.Tables(0).Columns(12).ColumnName = "Data Entry Date"

                    dsConvert.AcceptChanges()

                    For iCol = 0 To dsConvert.Tables(0).Columns.Count - 1

                        strMessage.Append("<td style=""text-align:center;""><strong><font color=""Black"" size=""9px"" face=""Calibri"">")
                        strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).Trim())
                        strMessage.Append("</font></strong></td>")

                    Next
                    strMessage.Append("</tr>")


                    For j = 0 To dsConvert.Tables(0).Rows.Count - 1
                        strMessage.Append("<tr>")

                        For i = 0 To dsConvert.Tables(0).Columns.Count - 1

                            If i = 0 Then
                                strMessage.Append("<td style=""text-align:center;vertical-align:top;""><font color=""black"" size=""2"" face=""Calibri"">")
                            Else
                                strMessage.Append("<td style=""text-align:left;vertical-align:top;""><font color=""black"" size=""2"" face=""Calibri"">")
                            End If
                            strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim())
                            strMessage.Append("</font></td>")

                        Next
                        strMessage.Append("</tr>")
                    Next
                    strMessage.Append("</table>")

                    Return strMessage.ToString
                End If
            Else
                'Alter By Parth Pandya for Addidng Column Visit in Excel sheet on 23-04-2014
                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                    strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")
                    strMessage.Append("<tr>")
                    strMessage.Append("<td colspan=""20""><center><strong><font color=""black"" size=""9px"" face=""Calibri"">")
                    strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                    strMessage.Append("</font></strong><center></td>")
                    strMessage.Append("</tr>")

                    strMessage.Append("<tr>")
                    strMessage.Append("<td colspan=""20""><center><strong><font color=""black"" size=""8px"" face=""Calibri"">")
                    strMessage.Append("Discrepancy Status Report")
                    strMessage.Append("</font></strong><center></td>")
                    strMessage.Append("</tr>")

                    strMessage.Append("<tr>")
                    strMessage.Append("<td><strong><font color=""black"" size=""9px"" face=""Calibri"">")
                    strMessage.Append("Project/Site")
                    strMessage.Append("</font></strong></td>")
                    strMessage.Append("<td colspan=""19""><strong><font color=""black"" size=""9px"" face=""Calibri"">")
                    strMessage.Append(Me.txtproject.Text.Trim())
                    strMessage.Append("</font></strong></td>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<tr>")
                    strMessage.Append("<td colspan=""20""><strong><font color=""black"" size=""9px"" face=""Calibri"">")
                    strMessage.Append(Me.lblCount.Text.Trim())
                    strMessage.Append("</font></strong></td>")
                    strMessage.Append("</tr>")
                Else
                    strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")
                    strMessage.Append("<tr>")
                    strMessage.Append("<td colspan=""19""><center><strong><font color=""black"" size=""9px"" face=""Calibri"">")
                    strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                    strMessage.Append("</font></strong><center></td>")
                    strMessage.Append("</tr>")

                    strMessage.Append("<tr>")
                    strMessage.Append("<td colspan=""19""><center><strong><font color=""black"" size=""8px"" face=""Calibri"">")
                    strMessage.Append("Discrepancy Status Report")
                    strMessage.Append("</font></strong><center></td>")
                    strMessage.Append("</tr>")

                    strMessage.Append("<tr>")
                    strMessage.Append("<td><strong><font color=""black"" size=""9px"" face=""Calibri"">")
                    strMessage.Append("Project/Site")
                    strMessage.Append("</font></strong></td>")
                    strMessage.Append("<td colspan=""18""><strong><font color=""black"" size=""9px"" face=""Calibri"">")
                    strMessage.Append(Me.txtproject.Text.Trim())
                    strMessage.Append("</font></strong></td>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<tr>")
                    strMessage.Append("<td colspan=""19""><strong><font color=""black"" size=""9px"" face=""Calibri"">")
                    strMessage.Append(Me.lblCount.Text.Trim())
                    strMessage.Append("</font></strong></td>")
                    strMessage.Append("</tr>")
                End If


                strMessage.Append("<tr>")

                ds.Tables(0).Columns.Add("SrNo")
                ds.Tables(0).Columns.Add("vCreatedDate")
                ds.Tables(0).Columns.Add("vUpdatedDate")
                ds.Tables(0).Columns.Add("vDataEntryOn")
                For Each dr In ds.Tables(0).Rows
                    If dr("CreatedDate").ToString() <> "" Then
                        dr("vCreatedDate") = Convert.ToString(CDate(dr("CreatedDate")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                    End If
                    If dr("UpdatedDate").ToString() <> "" Then
                        dr("vUpdatedDate") = Convert.ToString(CDate(dr("UpdatedDate")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                    End If
                    If dr("DataEntryOn").ToString() <> "" Then
                        dr("vDataEntryOn") = Convert.ToString(CDate(dr("DataEntryOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                    End If
                    dr("SrNo") = SrNo
                    SrNo += 1
                Next

                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then

                    dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "SrNo,Visit,Activity,Period,SubjectId,ScreenNo,RepetitionNo,Attribute,DiscrepancyValue,DCFType,Response,vDefaultValue,vModificationRemark,UpdateRemarks,CreatedBy,vCreatedDate,Status,UpdatedBy,vUpdatedDate,DataEntryBy,vDataEntryOn".Split(",")).DefaultView.ToTable())
                    dsConvert.AcceptChanges()
                    dsConvert.Tables(0).Columns(0).ColumnName = "Sr. No"
                    dsConvert.Tables(0).Columns(1).ColumnName = "Visit"
                    dsConvert.Tables(0).Columns(2).ColumnName = "Activity"
                    dsConvert.Tables(0).Columns(3).ColumnName = "Period"
                    dsConvert.Tables(0).Columns(4).ColumnName = "Subject ID"
                    'dsConvert.Tables(0).Columns(5).ColumnName = "My Subject No." ' comment by prayag
                    dsConvert.Tables(0).Columns(5).ColumnName = "Subejct No."
                    dsConvert.Tables(0).Columns(6).ColumnName = "Repetition No."
                    dsConvert.Tables(0).Columns(7).ColumnName = "Attribute"
                    dsConvert.Tables(0).Columns(8).ColumnName = "Discrepancy On Value"
                    dsConvert.Tables(0).Columns(9).ColumnName = "DCF Type"
                    dsConvert.Tables(0).Columns(10).ColumnName = "Query"
                    dsConvert.Tables(0).Columns(11).ColumnName = "Modified Value"
                    dsConvert.Tables(0).Columns(12).ColumnName = "Modification Remarks"
                    dsConvert.Tables(0).Columns(13).ColumnName = "DCF Remarks"
                    dsConvert.Tables(0).Columns(14).ColumnName = "Created By"
                    dsConvert.Tables(0).Columns(15).ColumnName = "Created Date"
                    dsConvert.Tables(0).Columns(16).ColumnName = "Status"
                    dsConvert.Tables(0).Columns(17).ColumnName = "Modified By"
                    dsConvert.Tables(0).Columns(18).ColumnName = "Modified On"
                    dsConvert.Tables(0).Columns(19).ColumnName = "Data Entry By"
                    dsConvert.Tables(0).Columns(20).ColumnName = "Data Entry On"
                    dsConvert.AcceptChanges()

                Else

                    dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "SrNo,Activity,Period,SubjectId,ScreenNo,RepetitionNo,Attribute,DiscrepancyValue,DCFType,Response,vDefaultValue,vModificationRemark,UpdateRemarks,CreatedBy,vCreatedDate,Status,UpdatedBy,vUpdatedDate,DataEntryBy,vDataEntryOn".Split(",")).DefaultView.ToTable())
                    dsConvert.AcceptChanges()
                    dsConvert.Tables(0).Columns(0).ColumnName = "Sr. No"
                    dsConvert.Tables(0).Columns(1).ColumnName = "Activity"
                    dsConvert.Tables(0).Columns(2).ColumnName = "Period"
                    dsConvert.Tables(0).Columns(3).ColumnName = "Subject ID"
                    'dsConvert.Tables(0).Columns(4).ColumnName = "My Subject No." comment by prayag
                    dsConvert.Tables(0).Columns(4).ColumnName = "Subejct No."
                    dsConvert.Tables(0).Columns(5).ColumnName = "Repetition No."
                    dsConvert.Tables(0).Columns(6).ColumnName = "Attribute"
                    dsConvert.Tables(0).Columns(7).ColumnName = "Discrepancy On Value"
                    dsConvert.Tables(0).Columns(8).ColumnName = "DCF Type"
                    dsConvert.Tables(0).Columns(9).ColumnName = "Query"
                    dsConvert.Tables(0).Columns(10).ColumnName = "Modified Value"
                    dsConvert.Tables(0).Columns(11).ColumnName = "Modification Remarks"
                    dsConvert.Tables(0).Columns(12).ColumnName = "DCF Remarks"
                    dsConvert.Tables(0).Columns(13).ColumnName = "Created By"
                    dsConvert.Tables(0).Columns(14).ColumnName = "Created Date"
                    dsConvert.Tables(0).Columns(15).ColumnName = "Status"
                    dsConvert.Tables(0).Columns(16).ColumnName = "Modified By"
                    dsConvert.Tables(0).Columns(17).ColumnName = "Modified On"
                    dsConvert.Tables(0).Columns(18).ColumnName = "Data Entry By"
                    dsConvert.Tables(0).Columns(19).ColumnName = "Data Entry On"
                    dsConvert.AcceptChanges()

                End If
                'End Alter
                For iCol = 0 To dsConvert.Tables(0).Columns.Count - 1

                    strMessage.Append("<td style=""text-align:center;""><strong><font color=""Black"" size=""9px"" face=""Calibri"">")
                    strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).Trim())
                    strMessage.Append("</font></strong></td>")

                Next
                strMessage.Append("</tr>")

                For j = 0 To dsConvert.Tables(0).Rows.Count - 1
                    strMessage.Append("<tr>")
                    For i = 0 To dsConvert.Tables(0).Columns.Count - 1
                        If i = 0 Then
                            strMessage.Append("<td style=""text-align:center;vertical-align:top;""><font color=""black"" size=""2"" face=""Calibri"">")
                        Else
                            strMessage.Append("<td style=""text-align:left;vertical-align:top;""><font color=""black"" size=""2"" face=""Calibri"">")
                        End If
                        strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim())
                        strMessage.Append("</font></td>")

                    Next
                    strMessage.Append("</tr>")
                Next
                strMessage.Append("</table>")
                Return strMessage.ToString
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....Problem with ExportToExcel")
            Return ""
        End Try
    End Function

    Private Function DCFExportToExcel(ByVal ds As DataSet) As String
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer
        Dim SrNo As Integer = 1
        Dim dsConvert As New DataSet

        Try
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")
                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""20""><center><strong><font color=""black"" size=""9px"" face=""Calibri"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""20""><center><strong><font color=""black"" size=""8px"" face=""Calibri"">")
                strMessage.Append("DCF Tracking Report")
                strMessage.Append("</font></strong><center></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<tr>")
                strMessage.Append("<td><strong><font color=""black"" size=""9px"" face=""Calibri"">")
                strMessage.Append("Project/Site")
                strMessage.Append("</font></strong></td>")
                strMessage.Append("<td colspan=""19""><strong><font color=""black"" size=""9px"" face=""Calibri"">")
                strMessage.Append(Me.txtproject.Text.Trim())
                strMessage.Append("</font></strong></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""20""><strong><font color=""black"" size=""9px"" face=""Calibri"">")
                strMessage.Append(Me.lblCount.Text.Trim())
                strMessage.Append("</font></strong></td>")
                strMessage.Append("</tr>")
            Else
                strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")
                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""19""><center><strong><font color=""black"" size=""9px"" face=""Calibri"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""19""><center><strong><font color=""black"" size=""8px"" face=""Calibri"">")
                strMessage.Append("Discrepancy Status Report")
                strMessage.Append("</font></strong><center></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<tr>")
                strMessage.Append("<td><strong><font color=""black"" size=""9px"" face=""Calibri"">")
                strMessage.Append("Project/Site")
                strMessage.Append("</font></strong></td>")
                strMessage.Append("<td colspan=""18""><strong><font color=""black"" size=""9px"" face=""Calibri"">")
                strMessage.Append(Me.txtproject.Text.Trim())
                strMessage.Append("</font></strong></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""19""><strong><font color=""black"" size=""9px"" face=""Calibri"">")
                strMessage.Append(Me.lblCount.Text.Trim())
                strMessage.Append("</font></strong></td>")
                strMessage.Append("</tr>")
            End If


            strMessage.Append("<tr>")

            ds.Tables(0).Columns.Add("SrNo")

            For Each dr In ds.Tables(0).Rows
                dr("SrNo") = SrNo
                SrNo += 1
            Next

            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "SrNo,Visit,Activity,iMySubjectNo,RepetitionNo,Attribute,DCFType,CreatedBy,CreatedDate,Status,AnsweredBy,AnsweredDate,UpdatedBy,UpdatedDate,GeneratedtoAnswered,AnsweredtoResolved,GeneratedtoResolved".Split(",")).DefaultView.ToTable())
                dsConvert.AcceptChanges()
                dsConvert.Tables(0).Columns(0).ColumnName = "Sr. No"
                dsConvert.Tables(0).Columns(3).ColumnName = "Subject No."
                dsConvert.Tables(0).Columns(4).ColumnName = "Repetition No."
                dsConvert.Tables(0).Columns(6).ColumnName = "DCF Type"
                dsConvert.Tables(0).Columns(7).ColumnName = "Generated By"
                dsConvert.Tables(0).Columns(8).ColumnName = "Generated On"
                dsConvert.Tables(0).Columns(10).ColumnName = "Answered By"
                dsConvert.Tables(0).Columns(11).ColumnName = "Answered On"
                dsConvert.Tables(0).Columns(12).ColumnName = "Resolved By"
                dsConvert.Tables(0).Columns(13).ColumnName = "Resolved On"
                dsConvert.Tables(0).Columns(14).ColumnName = "Generated to Answered (Days)"
                dsConvert.Tables(0).Columns(15).ColumnName = "Answered to Resolved (Days)"
                dsConvert.Tables(0).Columns(16).ColumnName = "Generated to Resolved (Days)"

                dsConvert.AcceptChanges()
            Else
                dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "SrNo,Visit,Activity,Period,SubjectId,iMySubjectNo,RepetitionNo,Attribute,DCFType,CreatedBy,CreatedDate,Status,AnsweredBy,AnsweredDate,UpdatedBy,UpdatedDate,GeneratedtoAnswered,AnsweredtoResolved,GeneratedtoResolved".Split(",")).DefaultView.ToTable())
                dsConvert.AcceptChanges()
                dsConvert.Tables(0).Columns(0).ColumnName = "Sr. No"
                dsConvert.Tables(0).Columns(4).ColumnName = "Subject ID"
                'dsConvert.Tables(0).Columns(5).ColumnName = "My Subject No."
                dsConvert.Tables(0).Columns(5).ColumnName = "Subejct No."
                dsConvert.Tables(0).Columns(6).ColumnName = "Repetition No."
                dsConvert.Tables(0).Columns(8).ColumnName = "DCF Type"
                dsConvert.Tables(0).Columns(9).ColumnName = "Created By"
                dsConvert.Tables(0).Columns(10).ColumnName = "Created Date"
                dsConvert.Tables(0).Columns(12).ColumnName = "Answered By"
                dsConvert.Tables(0).Columns(13).ColumnName = "Answered Date"
                dsConvert.Tables(0).Columns(14).ColumnName = "Resolved By"
                dsConvert.Tables(0).Columns(15).ColumnName = "Resolved Date"
                dsConvert.Tables(0).Columns(16).ColumnName = "Generated to Answered (Days)"
                dsConvert.Tables(0).Columns(17).ColumnName = "Answered to Resolved (Days)"
                dsConvert.Tables(0).Columns(18).ColumnName = "Generated to Resolved (Days)"

                dsConvert.AcceptChanges()

            End If
            'End Alter
            For iCol = 0 To dsConvert.Tables(0).Columns.Count - 1

                strMessage.Append("<td style=""text-align:center;""><strong><font color=""Black"" size=""9px"" face=""Calibri"">")
                strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).Trim())
                strMessage.Append("</font></strong></td>")

            Next
            strMessage.Append("</tr>")

            For j = 0 To dsConvert.Tables(0).Rows.Count - 1
                strMessage.Append("<tr>")
                For i = 0 To dsConvert.Tables(0).Columns.Count - 1
                    If i = 0 Then
                        strMessage.Append("<td style=""text-align:center;vertical-align:top;""><font color=""black"" size=""2"" face=""Calibri"">")
                    Else
                        strMessage.Append("<td style=""text-align:left;vertical-align:top;""><font color=""black"" size=""2"" face=""Calibri"">")
                    End If
                    strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim())
                    strMessage.Append("</font></td>")

                Next
                strMessage.Append("</tr>")
            Next
            strMessage.Append("</table>")
            Return strMessage.ToString


        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....Problem with DCFExportToExcel")
            Return ""
        End Try
    End Function

    Private Function RereviewExportToExcel(ByVal ds As DataSet) As String
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer
        Dim SrNo As Integer = 1
        Dim dsConvert As New DataSet

        Try

            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")
            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""10""><center><strong><font color=""black"" size=""9px"" face=""Calibri"">")
            strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""10""><center><strong><font color=""black"" size=""8px"" face=""Calibri"">")
            strMessage.Append("Re-review Report")
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("<td><strong><font color=""black"" size=""9px"" face=""Calibri"">")
            strMessage.Append("Project/Site")
            strMessage.Append("</font></strong></td>")
            strMessage.Append("<td colspan=""9""><strong><font color=""black"" size=""9px"" face=""Calibri"">")
            strMessage.Append(Me.txtproject.Text.Trim())
            strMessage.Append("</font></strong></td>")
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")


            ds.Tables(0).Columns.Add("SrNo")

            For Each dr In ds.Tables(0).Rows

                dr("SrNo") = SrNo
                SrNo += 1
            Next


            dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "SrNo", "Site", "SubjectID", "ScreenNo", "RandomizationNo", "Period", "Visit", "RepetitionNo", "Activity", "PendingRereviewLevel").DefaultView.ToTable())
            dsConvert.AcceptChanges()

            dsConvert.Tables(0).Columns(0).ColumnName = "Sr. No"
            dsConvert.Tables(0).Columns(1).ColumnName = "Project / Site"
            dsConvert.Tables(0).Columns(2).ColumnName = "Subejct Id"
            dsConvert.Tables(0).Columns(3).ColumnName = "Subject No."
            dsConvert.Tables(0).Columns(4).ColumnName = "Patient / Randomization No"
            dsConvert.Tables(0).Columns(5).ColumnName = "Period"
            dsConvert.Tables(0).Columns(6).ColumnName = "Parent Activity / Visit"
            dsConvert.Tables(0).Columns(7).ColumnName = "Repetition No"
            dsConvert.Tables(0).Columns(8).ColumnName = "Activity"
            dsConvert.Tables(0).Columns(9).ColumnName = "Pending Re-review Level"


            dsConvert.AcceptChanges()


            For iCol = 0 To dsConvert.Tables(0).Columns.Count - 1

                strMessage.Append("<td style=""text-align:center;""><strong><font color=""Black"" size=""9px"" face=""Calibri"">")
                strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).Trim())
                strMessage.Append("</font></strong></td>")

            Next
            strMessage.Append("</tr>")

            For j = 0 To dsConvert.Tables(0).Rows.Count - 1
                strMessage.Append("<tr>")

                For i = 0 To dsConvert.Tables(0).Columns.Count - 1
                    If i = 0 Then
                        strMessage.Append("<td style=""text-align:center;vertical-align:top;""><font color=""black"" size=""2"" face=""Calibri"">")
                    Else
                        strMessage.Append("<td style=""text-align:left;vertical-align:top;""><font color=""black"" size=""2"" face=""Calibri"">")
                    End If
                    strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim())
                    strMessage.Append("</font></td>")

                Next
                strMessage.Append("</tr>")
            Next
            strMessage.Append("</table>")

            Return strMessage.ToString

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....Problem with RereviewExportToExcel")
            Return ""
        End Try
    End Function

#End Region

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub

#End Region

#Region "Checked Changed"

    Protected Sub chkRereview_CheckedChanged(sender As Object, e As EventArgs) Handles chkRereview.CheckedChanged
        Try

            If chkRereview.Checked Then
                If txtproject.Text.Trim() = "" Then
                    objCommon.ShowAlert("Please select project first.", Me.Page)
                    chkRereview.Checked = False
                    txtproject.Focus()
                    Exit Sub
                End If
                ShowRereviewControls()
                If Not Me.FillViewState() Then
                    Exit Sub
                End If
                If Not FillActivity() Then
                    Exit Sub
                End If
                If Not FillRereviewSubject() Then
                    Exit Sub
                End If
            Else
                If chkDCF.Checked = True Then
                    ShowDCFControls()
                Else
                    ShowDSRControls()
                End If

            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....chkRereview_CheckedChanged")
        End Try
    End Sub

    Protected Sub chkDCF_CheckedChanged(sender As Object, e As EventArgs) Handles chkDCF.CheckedChanged
        Try
            If chkDCF.Checked Then
                If txtproject.Text.Trim() = "" Then
                    objCommon.ShowAlert("Please select project first.", Me.Page)
                    chkDCF.Checked = False
                    txtproject.Focus()
                    Exit Sub
                End If
                ShowDCFControls()
                'If Not Me.FillDCFViewState() Then
                '    Exit Sub
                'End If

                Me.DdlPeriod_SelectedIndexChanged(sender, e)

            Else
                If Me.chkRereview.Checked Then
                    ShowRereviewControls()
                Else
                    ShowDSRControls()
                End If
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....chkDCF_CheckedChanged")
        End Try
    End Sub

#End Region

#Region "Display function"

    Private Sub ShowRereviewControls()
        Try
            tblRereview.Style.Add("Display", "")
            tblSecond.Style.Add("Display", "none")
            tblDCF.Style.Add("Display", "none")
            'trselectSubject.Style.Add("Display", "none") comment by prayag
            'trSubject.Style.Add("Display", "none")
            Me.GVWDSR.DataSource = Nothing
            Me.GVWDSR.DataBind()
            Me.GVWODMSR.DataSource = Nothing
            Me.GVWODMSR.DataBind()
            Me.gvwDCF.DataSource = Nothing
            Me.gvwDCF.DataBind()
            Me.lblCount.Text = ""
            btnReReview.Visible = True
            btnRereviewGo.Visible = True
            btnGo.Visible = False
            btnExportToExcel.Visible = False
            trPeriodnew.Visible = False


            If Me.Request.QueryString("TrnType") Is Nothing Then
                Me.ddlType.Enabled = True
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "divshownone", "DivfieldShowHide('P');", True) ' added by Prayag
            Else
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "divshownone", "DivfieldShowHide('Q');", True) ' added by Prayag
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....ShowRereviewControls()")
        End Try
    End Sub

    Private Sub ShowDCFControls()
        Try
            tblDCF.Style.Add("Display", "")
            tblSecond.Style.Add("Display", "none")
            tblRereview.Style.Add("Display", "none")
            Me.GVWDSR.DataSource = Nothing
            Me.GVWDSR.DataBind()
            Me.GVWODMSR.DataSource = Nothing
            Me.GVWODMSR.DataBind()
            Me.gvRereview.DataSource = Nothing
            Me.gvRereview.DataBind()
            Me.lblCount.Text = ""

            btnDCFGo.Visible = True
            btnReReview.Visible = False
            btnRereviewGo.Visible = False
            btnGo.Visible = False
            btnExportToExcel.Visible = False

            If Me.Request.QueryString("TrnType") Is Nothing Then
                Me.ddlType.Enabled = True
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "divshownone", "DivfieldShowHide('P');", True) ' added by Prayag
            Else
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "divshownone", "DivfieldShowHide('Q');", True) ' added by Prayag
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....ShowRereviewControls()")
        End Try
    End Sub

    Private Sub ShowDSRControls()
        Try
            tblRereview.Style.Add("Display", "none")
            tblSecond.Style.Add("Display", "")
            'trselectSubject.Style.Add("Display", "") ''comment by prayag
            'trSubject.Style.Add("Display", "")
            tblDCF.Style.Add("Display", "none")
            Me.lblCount.Text = ""
            btnDCFGo.Visible = False
            btnDCFExportToExcel.Visible = False
            btnReReview.Visible = False
            btnRereviewGo.Visible = False
            btnRereviewExportToExcel.Visible = False
            btnGo.Visible = True
            Me.gvRereview.DataSource = Nothing
            Me.gvRereview.DataBind()
            Me.gvwDCF.DataSource = Nothing
            Me.gvwDCF.DataBind()

            If Me.Request.QueryString("TrnType") Is Nothing Then
                Me.ddlType.Enabled = True
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "divshownone", "DivfieldShowHide('P');", True) ' added by Prayag
            Else
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "divshownone", "DivfieldShowHide('Q');", True) ' added by Prayag
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....ShowDSRControls()")
        End Try
    End Sub

#End Region

#Region "Other function"
    Private Function ReportDetail(ByVal dt_FileDetail As DataTable) As String
        Dim strMessage As New StringBuilder
        strMessage.Append("<table border=""1""><tr>")

        For Each dc As DataColumn In dt_FileDetail.Columns
            strMessage.Append("<td style='font-weight: bold;text-align:center;'>")
            strMessage.Append(dc.ColumnName)
            strMessage.Append("</td>")
        Next

        strMessage.Append("</tr>")
        For i As Integer = 0 To dt_FileDetail.Rows.Count - 1
            strMessage.Append("<tr>")
            For j As Integer = 0 To dt_FileDetail.Columns.Count - 1
                strMessage.Append("<td>")
                strMessage.Append(dt_FileDetail.Rows(i)(j).ToString)
                strMessage.Append("</td>")
            Next
            strMessage.Append("</tr>")
        Next
        strMessage.Append("</table>")
        Return strMessage.ToString
    End Function
#End Region

#Region "Dynamic review"

    Private Function GetLegends() As Boolean
        Dim ds As New DataSet
        Dim estr As String = ""
        Dim dv As DataView
        Dim dt As New DataTable
        Dim dr_review As DataRow
        Try
            ddlRereviewLevel.DataSource = Nothing

            If Not Me.objHelpDb.Proc_GetLegends(Me.HProjectId.Value.ToString(), ds, estr) Then
                Throw New Exception("Error While Gettin Proc_getLegends" + estr)
            End If

            If ds Is Nothing Or ds.Tables.Count = 0 Then
                Return False
            End If

            Me.Session(Vs_dsReviewerlevel) = ds.Copy()

            dv = ds.Tables(0).Copy.DefaultView
            dv.RowFilter = "iReviewWorkflowStageId = " + Me.Session(S_WorkFlowStageId).ToString() + " AND vUserTypeCode = '" + Me.Session(S_UserType).ToString() + "'"

            If dv.ToTable().Rows.Count = 1 Then
                If dv.ToTable().Rows(0)("cAuthenticationDialog").ToString().ToUpper() = "N" Then
                    Me.hdnDirectAuthentication.Value = "true"
                    Me.UpdatePanel1.Update()
                ElseIf dv.ToTable().Rows(0)("cAuthenticationDialog").ToString().ToUpper() = "Y" Then
                    Me.hdnDirectAuthentication.Value = "false"
                    Me.UpdatePanel1.Update()
                End If
            End If

            dt.Columns.Add(New DataColumn("iWorkflowstageid", GetType(String)))
            dt.Columns.Add(New DataColumn("vworkflowdesc", GetType(String)))
            dt.AcceptChanges()
            If ds.Tables(0).Rows.Count > 0 Then
                dr_review = dt.NewRow()
                dr_review("iWorkflowstageid") = "All"
                dr_review("vworkflowdesc") = "All"
                dt.Rows.Add(dr_review)
                dt.AcceptChanges()
            End If
            For Each dr As DataRow In ds.Tables(0).Rows
                dr_review = dt.NewRow()
                dr_review("iWorkflowstageid") = Convert.ToString(dr("iActualWorkflowStageId"))
                If dr("iActualWorkflowStageId") = "10" Then
                    dr_review("vworkflowdesc") = "First Re-Review"
                ElseIf dr("iActualWorkflowStageId") = "20" Then
                    dr_review("vworkflowdesc") = "Second Re-Review"
                ElseIf dr("iActualWorkflowStageId") = "30" Then
                    dr_review("vworkflowdesc") = "Third Re-Review"
                End If
                dt.Rows.Add(dr_review)
                dt.AcceptChanges()
            Next
            ddlRereviewLevel.DataValueField = "iWorkflowstageid"
            ddlRereviewLevel.DataTextField = "vworkflowdesc"
            ddlRereviewLevel.DataSource = dt
            ddlRereviewLevel.DataBind()
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....GetLegends")
            Return False
        End Try
    End Function

#End Region

End Class
