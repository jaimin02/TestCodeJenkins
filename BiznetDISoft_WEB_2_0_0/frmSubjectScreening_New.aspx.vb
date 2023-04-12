Imports System.Drawing
Imports System.Collections.Generic
Imports System.Collections
Imports System.Text
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports Winnovative
Imports Newtonsoft.Json
Imports System.Globalization

Partial Class frmSubjectScreening_New
    Inherits System.Web.UI.Page

#Region "Variable Declaration "

    Shared ObjCommon As New clsCommon
    Shared objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Shared objLmd As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private ObjCommonP As New clsCommon
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommonP.GetHelpDbLambdaRef()
    Private objHelpdb As WS_HelpDbTable.WS_HelpDbTable = ObjCommonP.GetHelpDbTableRef()

    Private Const dt_MedEx As String = "Dt_MedEx"
    Private Const VSDtSubMedExMst As String = "Dt_SubMedExMst"
    Private Const VS_Controls As String = "VsControls"
    Private Const VS_dsSubMedex As String = "ds_SubMedex"
    Private Const VS_Choice As String = "Choice"
    Private Const VS_dtMedEx_Fill As String = "dtMedEx_Fill"
    Private Const VS_MedexGroups As String = "MedexGroups"
    Private Const VS_ContinueMode As String = "ContinueMode"
    Private Const VS_ProjectNo As String = "ProjectNo"
    Private Const VS_SubjectID As String = "SubjectID"
    Private Const VS_SelectedScreening As String = "SelectedScreening"
    Private Const VS_ContinueSelected As String = "SelectedScreening"
    Private Const VS_SaveMode As String = "SaveMode"
    Private Const VS_CheckEvent As String = "CheckEvent"
    Private Const VS_Checkval As String = "Checkval"
    Private Const VS_Mode As String = "Mode"

    Private Const VS_PlaceMedEx As String = "VSPlaceMedEx"


    Private Const VS_DtQC As String = "dtQC"

    Private Const Val_AN As String = "AN" '"Alphanumeric"
    Private Const Val_AL As String = "AL" '"Alphabate"
    Private Const Val_NU As String = "NU" '"Numeric"
    Private Const Val_IN As String = "IN" '"Integer"
    Private Const Val_NNI As String = "NNI" '"NotNull Integer"
    Private Const Val_NNU As String = "NNU" '"NotNull Numeric"

    Private Const Valid_XRayMonth As Integer = 6

    Private arrylst As New ArrayList

    Private Const GVCQC_MedExScreeningHdrQCNo As Integer = 0
    Private Const GVCQC_SubjectId As Integer = 1
    Private Const GVCQC_SrNo As Integer = 2
    Private Const GVCQC_Subject As Integer = 3
    Private Const GVCQC_QCComment As Integer = 4
    Private Const GVCQC_QCFlag As Integer = 5
    Private Const GVCQC_QCBy As Integer = 6
    Private Const GVCQC_QCDate As Integer = 7
    Private Const GVCQC_Response As Integer = 8
    Private Const GVCQC_ResponseGivenBy As Integer = 9
    Private Const GVCQC_ResponseGivenOn As Integer = 10
    Private Const GVCQC_LnkResponse As Integer = 11

    Private Const GVAudit_SrNO As Integer = 0
    Private Const GVAudit_ScreenDate As Integer = 1
    Private Const GVAudit_Remark As Integer = 2
    Private Const GVAudit_ModifyBy As Integer = 3
    Private Const GVAudit_Modifyon As Integer = 4
    Private Const GVAudit_MedExScreeningHdrNo As Integer = 5


    Private Const GVCDCF_nDCFNo As Integer = 0
    Private Const GVCDCF_nCRFDtlNo As Integer = 1
    Private Const GVCDCF_iSrNo As Integer = 2
    Private Const GVCDCF_vMedExCode As Integer = 3
    Private Const GVCDCF_cDCFType As Integer = 4
    Private Const GVCDCF_iDCFBy As Integer = 5
    Private Const GVCDCF_vCreatedBy As Integer = 6
    Private Const GVCDCF_dDCFDate As Integer = 7
    Private Const GVCDCF_vDiscrepancy As Integer = 8
    Private Const GVCDCF_vSourceResponse As Integer = 9
    'Private Const GVCDCF_vUpdateRemarks As Integer = 10
    Private Const GVCDCF_cDCFStatus As Integer = 10
    'Private Const GVCDCF_vUpdatedBy As Integer = 12
    'Private Const GVCDCF_dStatusChangedOn As Integer = 13
    Private Const GVCDCF_UserType As Integer = 11
    'Private Const GVCDCF_IntrenallyResolved As Integer = 17


    Private isScreenDoneToday As Boolean = False
    Private Gender As String = String.Empty
    Private Is_FormulaEnabled As Boolean = False
    Private Is_ComboGlobalDictionary As Boolean = False
    Private isDate As String = String.Empty
    Dim ContinueMode As Integer = 0
    Dim SelectedProfile As String = String.Empty

    'variables for Replacing Session variables
    Dim L_FirstName As String = String.Empty
    Dim L_LastName As String = String.Empty
    Dim L_UserId As String = String.Empty
    Dim L_TimeZoneName As String = String.Empty
    Dim L_Password As String = String.Empty
    Dim L_UserType As String = String.Empty
    Dim L_WorkFlowStageId As String = String.Empty
    Dim L_LocationCode As String = String.Empty
    Dim L_UserTypeName As String = String.Empty

    Dim dsScreeningTime As DataSet
    Dim dtScreeningTime As DataTable

    Dim nMedexScreeningHdrNo As String
    Dim vWorkspaceId As String
    Dim vSubjectID As String
    Dim dNewScreeningDateTime As String = ""
    Dim dSavedScreeningDateTime As String = ""
    Dim dComparedScreeningDateTime As String = ""
    Dim dComparedTodayDateTime As String = ""
    Dim nMedExWorkSpaceScreeningHdrNo As String = ""
    Dim IsProfileChange As Boolean
    Dim vSex As String
    Dim profileChange As Boolean = False

    Dim GVC_ECGSrNo As Integer = 0
    Dim GVC_ECGView As Integer = 1
    Dim GVC_ECGWorkSpaceId As Integer = 2
    Dim GVC_ECGSubjectId As Integer = 3
    Dim GVC_ECGScreenDate As Integer = 4
    Dim GVC_ECGFile As Integer = 5
    Dim GVC_ECGPath As Integer = 6

#End Region

#Region "Page Load"

    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "assignCSS", "  assignCSS();", True)
        Catch ex As Exception
            Throw New Exception("Error while Page_Init()")
        End Try
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ''Change Logic By Vivek Patel
        If txtWristBand.Text <> "" Then
            HSubjectId.Value = Convert.ToString(txtWristBand.Text).Trim.Split(New String() {"--"}, StringSplitOptions.None)(0)
        End If

        If IsPostBack() Then
            If Convert.ToString(txtproject.Text).Trim() <> "" Then
                Session("hProjectIdWorkSpace") = HProjectId.Value
                Session("hProjectDescProject") = txtproject.Text
            End If
        End If

        If IsProfileChange = True Then
            IsProfileChange = False
            GroupValidation = False
            Response.Redirect("~/frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&Set=true")

        End If
        If GroupValidation = True Then
            GroupValidation = False
            ObjCommon.ShowAlert("Data entry on this group is going on.", Me.Page)
        End If
        Me.hdnIsSave.Value = IsSaved
        If hdnTranscribeDate.Value <> "" Then
            Session("dScreenDate") = hdnTranscribeDate.Value
            Session("TranscribeRemarks") = hdnTranscribeRemarks.Value
            Session("isTransCribe") = hdnIsTranscribeYes.Value
        End If

        If Request.QueryString("Group") <> "" Then
            If Convert.ToString(hdnSubGroup.Value).Replace("Div", "") <> Request.QueryString("Group") And Convert.ToString(hdnSubGroup.Value).Replace("Div", "") <> "" Then
                Session("ScreeningGroup") = Convert.ToString(hdnSubGroup.Value).Replace("Div", "")
                ddlGroup.SelectedValue = Session("ScreeningGroup").ToString()
            Else
                Try
                    Session("ScreeningGroup") = Request.QueryString("Group")
                    ddlGroup.SelectedValue = Session("ScreeningGroup").ToString()
                Catch ex As Exception

                End Try

            End If
        Else
            Session("ScreeningGroup") = Convert.ToString(hdnSubGroup.Value).Replace("Div", "")
        End If
        If Not Session("hProjectIdWorkSpace") Is Nothing AndAlso (Session("hProjectIdWorkSpace").ToString() <> "" Or Session("hProjectIdWorkSpace").ToString() <> "") Then
            HProjectId.Value = Session("hProjectIdWorkSpace").ToString()
            txtproject.Text = Session("hProjectDescProject").ToString()
        ElseIf Not Session("hProjectIdWorkSpace") Is Nothing AndAlso Session("hProjectIdWorkSpace").ToString() <> "" AndAlso Session("hProjectIdWorkSpace").ToString() <> "" Then
            HProjectId.Value = Session("hProjectIdWorkSpace").ToString()
            txtproject.Text = Session("hProjectDescProject")
        End If
        If Me.hfMedexCode.Value <> "" Then
            Session("DCFMedExCode") = hfMedexCode.Value
        End If
        If Me.hdnMedExScreeningDtlNo.Value <> "" Then
            Session("ScreeningDTLNo") = hdnMedExScreeningDtlNo.Value
        ElseIf Not Session("ScreeningDTLNo") Is Nothing AndAlso Session("ScreeningDTLNo").ToString() <> "" Then
            hdnMedExScreeningDtlNo.Value = Session("ScreeningDTLNo").ToString()
        End If

        If (Request.QueryString("ScrDt") <> "") Then
            hdnScreeningDate.Value = Request.QueryString("ScrDt")
        End If

        If Not GenCall() Then
            Exit Sub
        End If

        If Not FillReason() Then
            Exit Sub
        End If

        If Not IsPostBack() Then

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                If (Not Me.Request.QueryString("Type") Is Nothing) Then
                    Me.txtSubject.Visible = False
                    Me.BtnQC.Style.Add("display", "none")
                    Me.lblSuject.Style.Add("display", "none")
                    Me.rblScreeningDate.Items(0).Selected = True
                    Me.rblScreeningDate_SelectedIndexChanged(sender, e)
                End If
            End If
            hdnDCFGenerated.Value = "False"
            Session("DCFGenerated") = "False"
        Else
            'If hdnDCFGenerated.Value = "True" Or DCFGenerated = "True" Then
            '    If hdnDCFGenerated.Value = "False" Then
            '        DCFGenerated = "False"
            '        'Me.mpDCFGenerate.Hide()
            '        ShowDCFPopup("H")
            '        hdnDCFGenerated.Value = "False"
            '    Else
            '        DCFGenerated = "True"
            '        ShowDCFPopup("S")
            '        'Me.mpDCFGenerate.Show()
            '        hdnDCFGenerated.Value = "True"
            '        mpDCFGenerate.Show()
            '        'DCFPanel.Update()
            '    End If
            'ElseIf hdnDCFGenerated.Value = "False" Or DCFGenerated = "False" Then
            '    DCFGenerated = "False"
            '    Me.mpDCFGenerate.Hide()
            '    hdnDCFGenerated.Value = "False"
            '    'DCFPanel.Update()
            'End If
            'btnContinueWorking_Click(Nothing, Nothing)
        End If
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If txtSubject.Text = "" Then
            GroupValidation = False
            Session("IsProjectSpecificScreening") = False
        End If

    End Sub

#End Region

#Region "GenCall "

    Private Function GenCall() As Boolean
        Dim sender As Object = Nothing
        Dim e As EventArgs = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim FolderPath As String = String.Empty

        If ddlGroup.SelectedIndex > 0 Then
            ddlGroup.SelectedValue = hdnSubGroup.Value
        End If

        Try

            Choice = Me.Request.QueryString("mode").ToString
            Me.ViewState(VS_Choice) = Choice
            Me.ViewState(VS_Mode) = Choice
            dtScreeningTime = New DataTable("ScreeningTimeMst")
            dtScreeningTime.Columns.Add("nScreeningId")
            dtScreeningTime.Columns.Add("nMedexScreeningHdrNo")
            dtScreeningTime.Columns.Add("vWorkspaceId")
            dtScreeningTime.Columns.Add("vSubjectID")
            dtScreeningTime.Columns.Add("dNewScreeningDateTime")
            dtScreeningTime.Columns.Add("dSavedScreeningDateTime")
            dtScreeningTime.Columns.Add("dComparedScreeningDateTime")
            dtScreeningTime.Columns.Add("dComparedTodayDateTime")
            dtScreeningTime.Columns.Add("iModifyBy")

            If Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso _
                Me.Request.QueryString("SubjectId").ToString.Trim() <> "" Then
                Me.HSubjectId.Value = Convert.ToString(Me.Request.QueryString("SubjectId")).Trim()
            End If

            If Not IsNothing(Me.Request.QueryString("ScrHdrNo")) AndAlso _
             Me.Request.QueryString("ScrHdrNo").ToString.Trim() <> "" Then
                Me.HScrNo.Value = Convert.ToString(Me.Request.QueryString("ScrHdrNo")).Trim()
            End If
            If Not IsNothing(Me.Request.QueryString("workspace")) AndAlso _
                         Me.Request.QueryString("workspace").ToString.Trim() <> "0000000000" Then
                Me.HProjectId.Value = Convert.ToString(Me.Request.QueryString("workspace")).Trim()
            End If


            If Not GenCall_Data() Then
                GenCall = False
                Exit Function
            End If

            If Not GenCall_ShowUI() Then
                GenCall = False
                Exit Function
            End If

            If Not Request.QueryString("Save") Is Nothing AndAlso Request.QueryString("Save").ToUpper = "TRUE" AndAlso IsSaved = True Then
                ObjCommon.ShowAlert("Record Saved Sucessfully", Me.Page)
                IsSaved = False
            End If
            Me.BtnQCSaveSend.Visible = False

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.BtnQCSaveSend.Visible = True
            End If

            Dim ds_Validation As DataSet

            If Not IsNothing(Me.Request.QueryString("ScreeningToday")) AndAlso _
             Me.Request.QueryString("ScreeningToday").ToString.Trim() <> "" Then
                Dim wstr = Convert.ToString(HSubjectId.Value) + "##" + Me.txtTranscribeDate.Text
                Dim estrv As String = ""
                objHelp.Proc_ScreeningSameDayValidation(Convert.ToString(HSubjectId.Value), Me.txtTranscribeDate.Text, ds_Validation, estrv)

                If Not ds_Validation Is Nothing AndAlso ds_Validation.Tables(0).Rows.Count > 0 Then
                    Me.ObjCommon.ShowAlert("Screening For the Subject is done selected date,You can Edit that But cannot add a new Screening for same Date.", Me.Page)
                    Me.MpeTranscribeScreening.Show()
                End If
            Else
                Dim estrv As String = ""
                objHelp.Proc_ScreeningSameDayValidation(Convert.ToString(HSubjectId.Value), Me.txtTranscribeDate.Text, ds_Validation, estrv)

                If Not ds_Validation Is Nothing AndAlso ds_Validation.Tables(0).Rows.Count > 0 Then
                    Me.ObjCommon.ShowAlert("Screening For the Subject is done selected date,You can Edit that But cannot add a new Screening for same Date.", Me.Page)
                    Me.MpeTranscribeScreening.Show()
                End If
            End If

            FolderPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail"))
            FolderPath += GeneralModule.Pro_Screening + "/" + Me.HSubjectId.Value

            Me.HfFolderPath.Value = FolderPath
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall")
            Return False

        End Try
    End Function

#End Region

#Region "GenCall_Data "

    Private Function GenCall_Data() As Boolean
        Dim ds As New DataSet
        Dim ds_MedExSubjectHdrDtl As New DataSet
        Dim ds_SubMedExMst As New DataSet
        Dim ds_SubMedex As New DataSet
        Dim ds_SubDetail As New DataSet
        Dim estr_retu As String = String.Empty
        Dim strQuery As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim format As String = "dd-MMM-yyyy"
        Dim datetime As New DateTime
        Dim dsConvert As New DataSet
        Dim dt_GenderWise As New DataTable
        Dim dt_Fillter As New DataTable
        Dim ds_MedExSubject As New DataSet
        Dim removeFlag As Boolean = False
        Dim ds_MedExSubjectScreeningDtl As New DataSet
        Try

            If ddlProfile.Items.Count > 0 Then
                Me.Session(S_ScrProfileIndex) = Convert.ToString(Me.ddlProfile.SelectedValue)
                If Not FillLocalVariables() Then
                    Exit Try
                End If
            Else
                If Me.Request.QueryString("Set") = "true" Then
                    If Not FillLocalVariables() Then
                        Exit Try
                    End If
                Else
                    Me.Session(S_ScrProfileIndex) = Convert.ToString(Me.Session(S_UserType))
                    L_FirstName = Convert.ToString(Me.Session(S_FirstName))
                    L_LastName = Convert.ToString(Me.Session(S_LastName))
                    L_UserId = Convert.ToString(Me.Session(S_UserID))

                    L_TimeZoneName = Convert.ToString(Me.Session(S_TimeZoneName))
                    L_Password = Convert.ToString(Me.Session(S_Password))
                    L_UserType = Convert.ToString(Me.Session(S_UserType))
                    L_WorkFlowStageId = Convert.ToString(Me.Session(S_WorkFlowStageId))
                    L_LocationCode = Convert.ToString(Me.Session(S_LocationCode))
                End If
            End If
            If Me.ViewState(VS_ContinueMode) Is Nothing Then
                FillProfile()
            End If
            hdnUserId.Value = Convert.ToString(L_UserId)
            Me.HdnSUserid.Value = L_UserId
            HfUserName.Value = Me.Session(S_UserName).ToString()
            Me.HdnWorkflow.Value = L_WorkFlowStageId
            If Not IsNothing(Me.Request.QueryString("mode")) Then
                Me.ViewState(VS_Choice) = CType(Me.Request.QueryString("mode"), WS_Lambda.DataObjOpenSaveModeEnum)
            End If

            If Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso _
                        (Me.Request.QueryString("SubjectId").ToString.Trim() <> "") Then

                Wstr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "'"
                If Not objHelp.GetView_SubjectMaster(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                    ds_SubDetail, estr_retu) Then
                    Return False
                End If

                Me.txtSubject.Text = ds_SubDetail.Tables(0).Rows(0).Item("vInitials").ToString.Trim() + " (" + Me.HSubjectId.Value.Trim() + ")"
                If Me.Request.QueryString("mode") = 4 AndAlso Me.Request.QueryString("Workspace").ToString = "0000000000" Then
                    Me.txtSubject.Text = Me.HSubjectId.Value.Trim()
                End If

                If Me.rblScreeningDate.Items.Count <= 0 Then
                    If Not fillScreeningDates() Then
                        Throw New Exception("Error While Fill Screening Dates")
                    End If
                End If
            End If
            If Not IsNothing(Me.Request.QueryString("SubId")) AndAlso _
                        (Me.Request.QueryString("SubId").ToString.Trim() <> "") Then
                If Me.ViewState(VS_SaveMode) <> 1 Then
                    If Me.ViewState(VS_CheckEvent) <> 1 Then
                        Me.ViewState(VS_SaveMode) = Nothing
                        If Me.HSubjectId.Value.ToString = "" Then
                            Me.HSubjectId.Value = Me.Request.QueryString("SubId").ToString.Trim()
                        End If
                        Wstr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "'"
                        If Not objHelp.GetView_SubjectMaster(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                            ds_SubDetail, estr_retu) Then
                            Return False
                        End If
                        Me.txtSubject.Text = ds_SubDetail.Tables(0).Rows(0).Item("FullName").ToString.Trim() + " (" + Me.HSubjectId.Value.Trim() + ")"
                        If Me.Request.QueryString("mode") = 4 AndAlso Me.Request.QueryString("Workspace").ToString = "0000000000" Then
                            Me.txtSubject.Text = ds_SubDetail.Tables(0).Rows(0).Item("vInitials").ToString.Trim() + " (" + Me.HSubjectId.Value.Trim() + ")"
                        End If
                        If Me.HSubjectId.Value.ToString = Me.Request.QueryString("SubId").ToString.Trim() Then
                            If Me.rblScreeningDate.SelectedValue <> Me.Request.QueryString("ScrDt") Then
                                Me.ViewState(VS_SelectedScreening) = Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim()
                                If Me.Request.QueryString("SubId").ToString.Trim() <> "" Then
                                    If Me.rblScreeningDate.SelectedValue = "" Then
                                        'If Not ValidateUserWiseScreeningEntry("VALIDATE", Me.Request.QueryString("SubId").ToString.Trim(), Me.Request.QueryString("ScrDt")) Then
                                        '    ObjCommon.ShowAlert("Another User was working on this screening Date of this subjectid", Me.Page)
                                        '    For Each li As ListItem In rblScreeningDate.Items
                                        '        'li.Selected = False
                                        '    Next
                                        '    removeFlag = True
                                        '    ScreeningGroup = ""
                                        'End If
                                    Else
                                        If Not ValidateUserWiseScreeningEntry("REMOVEENTRY", Me.Request.QueryString("SubId").ToString.Trim(), Me.Request.QueryString("ScrDt")) Then
                                            ObjCommon.ShowAlert("Error While Removing Entry In Screening Control", Me.Page)
                                        End If
                                    End If
                                End If
                            End If
                            If Not fillScreeningDates() Then
                                Throw New Exception("Error While Fill Screening Dates")
                            End If
                            If removeFlag = False Then
                                If Me.ViewState(VS_SelectedScreening) <> "" Then
                                    Me.rblScreeningDate.SelectedValue = Me.ViewState(VS_SelectedScreening).ToString()
                                    Me.ddlScreeningDate.SelectedValue = Me.ViewState(VS_SelectedScreening).ToString()
                                Else
                                    If Not Me.Request.QueryString("ScrDt") Is Nothing Then
                                        Me.rblScreeningDate.SelectedValue = Me.Request.QueryString("ScrDt").ToString()
                                        Me.ddlScreeningDate.SelectedValue = Me.Request.QueryString("ScrDt").ToString()
                                    End If
                                End If
                            End If
                        Else
                            Me.btnReviewHistory.Style.Add("display", "none")
                            'Me.btnPdf.Style.Add("display", "none")
                        End If
                    End If
                End If
            End If

            If Not IsNothing(Me.Request.QueryString("Save")) AndAlso _
                        (Me.Request.QueryString("Save").ToString.Trim() <> "") Then
                If Me.ViewState(VS_ContinueMode) Is Nothing Then
                    If IsSaved = True Then
                        'ObjCommon.ShowAlert("Record Saved Sucessfully", Me.Page)
                        'IsSaved = False
                    End If
                Else
                    If Me.Request.QueryString("mode") = 4 Then
                        If Not IsNothing(Me.Request.QueryString("QC")) AndAlso _
                                    (Me.Request.QueryString("QC").ToString.Trim() = "true") Then

                            Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&QC=true&Set=true", False)
                        ElseIf Not IsNothing(Me.Request.QueryString("ScrHdrNo")) AndAlso _
                                    (Me.Request.QueryString("ScrHdrNo").ToString.Trim() <> "") Then
                            Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Set=true&Workspace=" + Me.HProjectId.Value + "&ScrHdrNo= " + Me.HScrNo.Value + "&SubId=" + Me.HSubjectId.Value.ToString() + "", False)
                        Else
                            Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Set=true&Workspace=0000000000", False)
                        End If
                    ElseIf Me.ViewState(VS_SaveMode) = 1 Then
                        Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&Set=true&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Me.HSubjectId.Value.ToString(), False)
                    Else
                        Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&Set=true&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Me.HSubjectId.Value.ToString(), False)
                    End If
                End If
            End If

            If Me.rblScreeningDate.Items.Count >= 0 AndAlso Me.rblScreeningDate.SelectedValue.ToUpper.Trim() <> "N" AndAlso _
                                        Me.rblScreeningDate.SelectedIndex <> -1 Then 'For Old

                If Not Me.rblScreeningDate.SelectedValue.Trim() = "" Then
                    If Me.Session("S_Temp") Is Nothing Then
                        If (Me.Request.QueryString("mode").ToString.Trim() = "4") Then
                            Wstr = IIf(Me.HSubjectId.Value.Trim() = "", "0000", Me.HSubjectId.Value.Trim()) + "##" + CDate(Me.rblScreeningDate.SelectedValue.Trim()).Date.ToString("dd-MMM-yyyy") + "##" + Session("ScreeningGroup").ToString().Replace("Div", "") + "##" + "0"
                        ElseIf (Not Request.QueryString("ViewAll") Is Nothing AndAlso Me.Request.QueryString("ViewAll").ToString.Trim() = True) Then
                            Wstr = IIf(Me.HSubjectId.Value.Trim() = "", "0000", Me.HSubjectId.Value.Trim()) + "##" + CDate(Me.rblScreeningDate.SelectedValue.Trim()).Date.ToString("dd-MMM-yyyy") + "##" + "" + "##" + L_UserType
                        Else
                            Wstr = IIf(Me.HSubjectId.Value.Trim() = "", "0000", Me.HSubjectId.Value.Trim()) + "##" + CDate(Me.rblScreeningDate.SelectedValue.Trim()).Date.ToString("dd-MMM-yyyy") + "##" + Session("ScreeningGroup").ToString().Replace("Div", "") + "##" + L_UserType
                        End If



                        ds_MedExSubjectScreeningDtl = Me.objHelpdb.ProcedureExecute("dbo.Proc_SCREENINGTEMPLATEHDRDTL", Wstr)
                        ds_MedExSubjectScreeningDtl.Tables(0).DefaultView.Sort = "cScreeningType,vWorkSpaceId,iSeqNo"
                        Dim primaryKey(1) As DataColumn
                        primaryKey(1) = ds_MedExSubjectScreeningDtl.Tables(0).Columns("nMedExScreeningDtl")
                        ds_MedExSubjectScreeningDtl.Tables(0).PrimaryKey = primaryKey
                        ds_MedExSubjectScreeningDtl.AcceptChanges()

                        Dim DV1 As DataView = ds_MedExSubjectScreeningDtl.Tables(0).DefaultView
                        DV1.RowFilter = "vMedExCode <> 'Eligble' AND vMedExCode <> 'FirstRemarks'  AND  vMedExCode <> 'FirstReview' AND vMedExCode <> 'SecondReview'  AND vMedExCode <> 'Remarks' "

                        Dim d1t As DataTable = DV1.ToTable()
                        If d1t.Rows.Count <= 0 Then
                            'If ds_MedExSubjectScreeningDtl.Tables(0).Rows.Count <= 3 And ds_MedExSubjectScreeningDtl.Tables(0).Columns.Contains("DefaultValue") Then

                            Wstr = "cActiveFlag <>'" + ActiveFlag_No + "' AND cStatusIndi <>'" + Status_Delete + "' AND cStatusIndi_Dtl <> '" + Status_Delete + "'  And vMedExType <> 'IMPORT'"
                            If chkScreeningType.Items(0).Selected Then
                                Wstr += " AND vWorkSpaceId='" & Me.Request.QueryString("Workspace") & "'" & _
                                    " AND vUserLocationCode like'%" + L_LocationCode + "%'"
                                Wstr += " AND vUserTypeCode = '" + L_UserType + "'"
                            End If

                            If chkScreeningType.Items(1).Selected Then
                                Wstr += IIf(chkScreeningType.Items(0).Selected, " %OR ", " AND ") + " vWorkSpaceId = '" + HProjectId.Value.ToString + "'"
                                Wstr += " AND vUserTypeCode = '" + L_UserType + "'"
                            Else
                                Wstr += " AND Getdate() BETWEEN ISNULL(dFromDate,GETDATE()) AND   ISNULL(dToDate,GETDATE())"
                                Wstr += " AND vUserTypeCode = '" + L_UserType + "'"

                            End If
                            If hdnSubGroup.Value <> "" Then
                                Wstr += " AND vMedExGroupCode = '" + Convert.ToString(hdnSubGroup.Value).Replace("Div", "") + "'"
                            ElseIf Request.QueryString("Group") <> "" Then
                                Wstr += " AND vMedExGroupCode = '" + Convert.ToString(Request.QueryString("Group")).Replace("Div", "") + "'"
                            End If


                            Wstr += " Order by vWorkSpaceId,iSeqNo"
                            If Not objHelp.GetViewScreeningTemplateDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_MedExSubjectScreeningDtl, estr_retu) Then
                                GenCall_Data = False
                                Exit Function
                            End If
                            hdnAlreadySaved.Value = "YES"
                            Dim dv As DataView = ds_MedExSubjectScreeningDtl.Tables(0).DefaultView()
                            If Not Session("ScreeningGroup") Is Nothing AndAlso Session("ScreeningGroup").ToString() <> "" Then
                                dv.RowFilter = "vMedExGroupCode =" + Session("ScreeningGroup").ToString()
                            End If
                            Dim dt As DataTable = dv.ToTable()
                            ds_MedExSubjectHdrDtl = New DataSet()
                            ds_MedExSubjectHdrDtl.Tables().Add(dt)


                        End If

                        Me.Session("S_Temp") = ds_MedExSubjectScreeningDtl.Tables(0).DefaultView.ToTable.Copy()
                        If Me.Request.QueryString("Workspace") <> "0000000000" Then
                            ds_MedExSubjectHdrDtl.Tables.Add(CType(Me.Session("S_Temp"), DataTable))
                            Me.Session("S_Temp").TableName = "VIEW_SCREENINGTEMPLATEHDRDTL"
                            Me.Session("S_Temp") = Nothing
                        ElseIf CType(Me.Session("S_Temp"), DataTable).Rows.Count > 0 Then
                            If Not ds_MedExSubjectHdrDtl.Tables.Contains("VIEW_SCREENINGTEMPLATEHDRDTL") Then

                                ds_MedExSubjectHdrDtl = Nothing
                                ds_MedExSubjectHdrDtl = New DataSet()
                                ds_MedExSubjectHdrDtl.Tables.Add(CType(Me.Session("S_Temp"), DataTable).Copy())
                                Me.Session("S_Temp").TableName = "VIEW_SCREENINGTEMPLATEHDRDTL"
                                'Me.Session("S_Temp") = Nothing
                            End If

                        End If
                    Else
                        If CType(Me.Session("S_Temp"), DataTable).Rows.Count <= 0 Then
                            If (Me.Request.QueryString("mode").ToString.Trim() = "4") Then
                                Wstr = IIf(Me.HSubjectId.Value.Trim() = "", "0000", Me.HSubjectId.Value.Trim()) + "##" + CDate(Me.rblScreeningDate.SelectedValue.Trim()).Date.ToString("dd-MMM-yyyy") + "##" + hdnSubGroup.Value.Replace("Div", "") + "##" + "0"
                            ElseIf (Not Request.QueryString("ViewAll") Is Nothing AndAlso Request.QueryString("ViewAll") = True) Then
                                Wstr = IIf(Me.HSubjectId.Value.Trim() = "", "0000", Me.HSubjectId.Value.Trim()) + "##" + CDate(Me.rblScreeningDate.SelectedValue.Trim()).Date.ToString("dd-MMM-yyyy") + "##" + hdnSubGroup.Value.Replace("Div", "") + "##" + L_UserType
                            Else
                                Wstr = IIf(Me.HSubjectId.Value.Trim() = "", "0000", Me.HSubjectId.Value.Trim()) + "##" + CDate(Me.rblScreeningDate.SelectedValue.Trim()).Date.ToString("dd-MMM-yyyy") + "##" + hdnSubGroup.Value.Replace("Div", "") + "##" + L_UserType
                            End If


                            ds_MedExSubjectScreeningDtl = Me.objHelpdb.ProcedureExecute("dbo.Proc_SCREENINGTEMPLATEHDRDTL", Wstr)
                            ds_MedExSubjectScreeningDtl.Tables(0).DefaultView.Sort = "cScreeningType,vWorkSpaceId,iSeqNo"
                            Me.Session("S_Temp") = ds_MedExSubjectScreeningDtl.Tables(0).DefaultView.ToTable.Copy()
                            Me.Session("S_Temp").TableName = "VIEW_SCREENINGTEMPLATEHDRDTL"
                        End If
                        If Not ds_MedExSubjectHdrDtl Is Nothing AndAlso ds_MedExSubjectHdrDtl.Tables.Count > 0 AndAlso ds_MedExSubjectHdrDtl.Tables(0).Rows.Count > 0 Then
                            If Not ds_MedExSubjectHdrDtl.Tables.Contains("VIEW_SCREENINGTEMPLATEHDRDTL") Then
                                ds_MedExSubjectHdrDtl.Tables.Add(CType(Me.Session("S_Temp"), DataTable).Copy())
                                Me.Session("S_Temp").TableName = "VIEW_SCREENINGTEMPLATEHDRDTL"
                            End If
                            Me.Session("S_Temp") = Nothing
                        Else


                            If (Not Request.QueryString("ViewAll") Is Nothing AndAlso Request.QueryString("ViewAll") = True) Then
                                Wstr = IIf(Me.HSubjectId.Value.Trim() = "", "0000", Me.HSubjectId.Value.Trim()) + "##" + CDate(Me.rblScreeningDate.SelectedValue.Trim()).Date.ToString("dd-MMM-yyyy") + "##" + "" + "##" + L_UserType
                            ElseIf Not Request.QueryString("mode") = "4" Then
                                Wstr = IIf(Me.HSubjectId.Value.Trim() = "", "0000", Me.HSubjectId.Value.Trim()) + "##" + CDate(Me.rblScreeningDate.SelectedValue.Trim()).Date.ToString("dd-MMM-yyyy") + "##" + Session("ScreeningGroup").ToString() + "##" + L_UserType
                            Else
                                Wstr = IIf(Me.HSubjectId.Value.Trim() = "", "0000", Me.HSubjectId.Value.Trim()) + "##" + CDate(Me.rblScreeningDate.SelectedValue.Trim()).Date.ToString("dd-MMM-yyyy") + "##" + Session("ScreeningGroup").ToString() + "##" + "0"
                            End If


                            ds_MedExSubjectHdrDtl = Me.objHelpdb.ProcedureExecute("dbo.Proc_SCREENINGTEMPLATEHDRDTL", Wstr)
                            ds_MedExSubjectHdrDtl.Tables(0).DefaultView.Sort = "cScreeningType,vWorkSpaceId,iSeqNo"

                            Dim DV1 As DataView = ds_MedExSubjectHdrDtl.Tables(0).DefaultView
                            DV1.RowFilter = "vMedExCode <> 'Eligble' AND vMedExCode <> 'FirstRemarks'  AND  vMedExCode <> 'FirstReview' AND vMedExCode <> 'SecondReview'  AND vMedExCode <> 'Remarks' "

                            Dim TemplateDTLno As String = ""

                            If Not ds_MedExSubjectHdrDtl Is Nothing AndAlso ds_MedExSubjectHdrDtl.Tables(0).Rows.Count > 0 AndAlso DV1.Table().DefaultView.ToTable().Rows.Count = 0 Then
                                Wstr = IIf(Me.HSubjectId.Value.Trim() = "", "0000", Me.HSubjectId.Value.Trim()) + "##" + CDate(Me.rblScreeningDate.SelectedValue.Trim()).Date.ToString("dd-MMM-yyyy") + "##" + "" + "##" + "0"
                                ds_MedExSubjectHdrDtl = Me.objHelpdb.ProcedureExecute("dbo.Proc_SCREENINGTEMPLATEHDRDTL", Wstr)
                                TemplateDTLno = ds_MedExSubjectHdrDtl.Tables(0).Rows(0)("nScreeningTemplateHdrNo")
                                Wstr = "cActiveFlag <>'" + ActiveFlag_No + "' AND cStatusIndi <>'" + Status_Delete + "' AND cStatusIndi_Dtl <> '" + Status_Delete + "'  And vMedExType <> 'IMPORT'"
                                If chkScreeningType.Items(0).Selected Then
                                    Wstr += " AND vWorkSpaceId='" & Me.Request.QueryString("Workspace") & "'" & _
                                        " AND vUserLocationCode like'%" + L_LocationCode + "%'"
                                End If

                                If chkScreeningType.Items(1).Selected Then
                                    Wstr += IIf(chkScreeningType.Items(0).Selected, " %OR ", " AND ") + " vWorkSpaceId = '" + HProjectId.Value.ToString + "'"
                                    Wstr += " AND vUserTypeCode = '" + L_UserType + "'"
                                Else
                                    Wstr += " AND nScreeningTemplateHdrNo = " + TemplateDTLno
                                    Wstr += " AND vUserTypeCode = '" + L_UserType + "'"
                                End If

                                Wstr += " Order by vWorkSpaceId,iSeqNo"
                                If Not objHelp.GetViewScreeningTemplateDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_MedExSubjectScreeningDtl, estr_retu) Then
                                    Exit Function
                                End If

                                hdnAlreadySaved.Value = "YES"
                                Dim dv As DataView = ds_MedExSubjectScreeningDtl.Tables(0).DefaultView()
                                If Not Session("ScreeningGroup") Is Nothing AndAlso Session("ScreeningGroup").ToString() <> "" Then
                                    dv.RowFilter = "vMedExGroupCode =" + Session("ScreeningGroup").ToString()
                                End If

                                Dim dt As DataTable = dv.ToTable()
                                ds_MedExSubjectHdrDtl = New DataSet()
                                ds_MedExSubjectHdrDtl.Tables().Add(dt)

                            Else
                                Dim d1t As DataTable = DV1.ToTable()
                                If d1t.Rows.Count <= 0 Then
                                    'If ds_MedExSubjectScreeningDtl.Tables(0).Rows.Count <= 3 And ds_MedExSubjectScreeningDtl.Tables(0).Columns.Contains("DefaultValue") Then

                                    Wstr = "cActiveFlag <>'" + ActiveFlag_No + "' AND cStatusIndi <>'" + Status_Delete + "' AND cStatusIndi_Dtl <> '" + Status_Delete + "'  And vMedExType <> 'IMPORT'"
                                    If chkScreeningType.Items(0).Selected Then
                                        Wstr += " AND vWorkSpaceId='" & Me.Request.QueryString("Workspace") & "'" & _
                                            " AND vUserLocationCode like'%" + L_LocationCode + "%'"
                                        Wstr += " AND vUserTypeCode = '" + L_UserType + "'"
                                    End If

                                    If chkScreeningType.Items(1).Selected Then
                                        Wstr += IIf(chkScreeningType.Items(0).Selected, " %OR ", " AND ") + " vWorkSpaceId = '" + HProjectId.Value.ToString + "'"
                                        Wstr += " AND vUserTypeCode = '" + L_UserType + "'"
                                    Else
                                        Wstr += " AND Getdate() BETWEEN ISNULL(dFromDate,GETDATE()) AND   ISNULL(dToDate,GETDATE())"
                                        Wstr += " AND vUserTypeCode = '" + L_UserType + "'"

                                    End If
                                    If hdnSubGroup.Value <> "" Then
                                        Wstr += " AND vMedExGroupCode = '" + Convert.ToString(hdnSubGroup.Value).Replace("Div", "") + "'"
                                    ElseIf Request.QueryString("Group") <> "" Then
                                        Wstr += " AND vMedExGroupCode = '" + Convert.ToString(Request.QueryString("Group")).Replace("Div", "") + "'"
                                    End If


                                    Wstr += " Order by vWorkSpaceId,iSeqNo"
                                    If Not objHelp.GetViewScreeningTemplateDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_MedExSubjectScreeningDtl, estr_retu) Then
                                        Exit Function
                                    End If
                                    hdnAlreadySaved.Value = "YES"
                                    Dim dv As DataView = ds_MedExSubjectScreeningDtl.Tables(0).DefaultView()
                                    dv.RowFilter = "vMedExGroupCode =" + Session("ScreeningGroup").ToString()
                                    Dim dt As DataTable = dv.ToTable()
                                    ds_MedExSubjectHdrDtl = New DataSet()
                                    ds_MedExSubjectHdrDtl.Tables().Add(dt)


                                End If

                            End If

                            'Dim d1t As DataTable = DV1.ToTable()
                            'If d1t.Rows.Count <= 0 Then
                            '    'If ds_MedExSubjectScreeningDtl.Tables(0).Rows.Count <= 3 And ds_MedExSubjectScreeningDtl.Tables(0).Columns.Contains("DefaultValue") Then

                            '    Wstr = "cActiveFlag <>'" + ActiveFlag_No + "' AND cStatusIndi <>'" + Status_Delete + "' AND cStatusIndi_Dtl <> '" + Status_Delete + "'  And vMedExType <> 'IMPORT'"
                            '    If chkScreeningType.Items(0).Selected Then
                            '        Wstr += " AND vWorkSpaceId='" & Me.Request.QueryString("Workspace") & "'" & _
                            '            " AND vUserLocationCode like'%" + L_LocationCode + "%'"
                            '    End If

                            '    If chkScreeningType.Items(1).Selected Then
                            '        Wstr += IIf(chkScreeningType.Items(0).Selected, " %OR ", " AND ") + " vWorkSpaceId = '" + HProjectId.Value.ToString + "'"
                            '    Else
                            '        Wstr += " AND Getdate() BETWEEN ISNULL(dFromDate,GETDATE()) AND   ISNULL(dToDate,GETDATE())"
                            '        Wstr += " AND vUserTypeCode = '" + L_UserType + "'"

                            '    End If
                            '    If hdnSubGroup.Value <> "" Then
                            '        Wstr += " AND vMedExGroupCode = '" + Convert.ToString(hdnSubGroup.Value).Replace("Div", "") + "'"
                            '    ElseIf Request.QueryString("Group") <> "" Then
                            '        Wstr += " AND vMedExGroupCode = '" + Convert.ToString(Request.QueryString("Group")).Replace("Div", "") + "'"
                            '    End If


                            '    Wstr += " Order by vWorkSpaceId,iSeqNo"
                            '    If Not objHelp.GetViewScreeningTemplateDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_MedExSubjectScreeningDtl, estr_retu) Then
                            '        Exit Function
                            '    End If
                            '    hdnAlreadySaved.Value = "YES"
                            '    Dim dv As DataView = ds_MedExSubjectScreeningDtl.Tables(0).DefaultView()
                            '    dv.RowFilter = "vMedExGroupCode =" + ScreeningGroup
                            '    Dim dt As DataTable = dv.ToTable()
                            '    ds_MedExSubjectHdrDtl = New DataSet()
                            '    ds_MedExSubjectHdrDtl.Tables().Add(dt)


                            'End If


                            Me.Session("S_Temp") = ds_MedExSubjectHdrDtl.Tables(0).DefaultView.ToTable.Copy()
                            Me.Session("S_Temp").TableName = "VIEW_SCREENINGTEMPLATEHDRDTL"
                        End If
                    End If

                    'Me.btnPdf.Style.Add("display", "")
                    If Not fillQCGrid() Then
                        Exit Function
                    End If
                    If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View And _
                        ds_MedExSubjectHdrDtl.Tables(0).Rows.Count <= 0 AndAlso Me.HSubjectId.Value.Trim() <> "" Then
                        Me.ObjCommon.ShowAlert("No Screening Has Been Done For This Subject", Me.Page())
                        Me.BtnQC.Visible = False
                        Return False
                    End If
                    ds_MedExSubject = ds_MedExSubjectHdrDtl
                    If ds_MedExSubjectHdrDtl.Tables(0).Rows(0)("vWorkSpaceId") = "0000000000" Or ds_MedExSubjectHdrDtl.Tables(0).Rows(0)("vWorkSpaceId") = "" Then
                        ds_MedExSubjectHdrDtl.Tables(0).DefaultView.RowFilter = "vDefaultValue = 'Female'"
                        If Not ds_MedExSubjectHdrDtl.Tables(0).DefaultView.ToTable().Rows.Count > 0 Then
                            ds_MedExSubjectHdrDtl.Tables(0).DefaultView.RowFilter = "vDefaultValue = 'Male'"
                            If ds_MedExSubjectHdrDtl.Tables(0).DefaultView.ToTable().Rows.Count > 0 Then
                                ds_MedExSubjectHdrDtl.Tables(0).DefaultView.RowFilter = "vMedExGroupCode <> 00037"
                                dt_Fillter = ds_MedExSubjectHdrDtl.Tables(0).DefaultView.ToTable()
                                dsConvert.Tables.Add(dt_Fillter.DefaultView.ToTable(True, "vMedExCode,vMedExDesc,vMedExGroupCode,vMedExGroupDesc,vMedExSubGroupCode,vMedExSubGroupDesc,vMedExType,vUOM,vMedExValues,vDefaultValue".Split(",")).DefaultView.ToTable())
                                dsConvert.AcceptChanges()
                            Else
                                ds_MedExSubject.Tables(0).DefaultView.RowFilter = "nMedExScreeningHdrNo is not null"
                                dt_GenderWise = ds_MedExSubject.Tables(0).DefaultView.ToTable()
                                dsConvert.Tables.Add(dt_GenderWise.DefaultView.ToTable(True, "vMedExCode,vMedExDesc,vMedExGroupCode,vMedExGroupDesc,vMedExSubGroupCode,vMedExSubGroupDesc,vMedExType,vUOM,vMedExValues,vDefaultValue".Split(",")).DefaultView.ToTable())
                                dsConvert.AcceptChanges()
                            End If
                        Else
                            ds_MedExSubject.Tables(0).DefaultView.RowFilter = "nMedExScreeningHdrNo is not null"
                            dt_GenderWise = ds_MedExSubject.Tables(0).DefaultView.ToTable()
                            dsConvert.Tables.Add(dt_GenderWise.DefaultView.ToTable(True, "vMedExCode,vMedExDesc,vMedExGroupCode,vMedExGroupDesc,vMedExSubGroupCode,vMedExSubGroupDesc,vMedExType,vUOM,vMedExValues,vDefaultValue".Split(",")).DefaultView.ToTable())
                            dsConvert.AcceptChanges()
                        End If
                    Else
                        ds_MedExSubjectHdrDtl.Tables(0).DefaultView.RowFilter = "vDefaultValue = 'F'"
                        If Not ds_MedExSubjectHdrDtl.Tables(0).DefaultView.ToTable().Rows.Count > 0 Then
                            ds_MedExSubjectHdrDtl.Tables(0).DefaultView.RowFilter = "vDefaultValue = 'M'"
                            If ds_MedExSubjectHdrDtl.Tables(0).DefaultView.ToTable().Rows.Count > 0 Then
                                ds_MedExSubjectHdrDtl.Tables(0).DefaultView.RowFilter = "vMedExGroupCode <> 00037"
                                dt_Fillter = ds_MedExSubjectHdrDtl.Tables(0).DefaultView.ToTable()
                                dsConvert.Tables.Add(dt_Fillter.DefaultView.ToTable(True, "vMedExCode,vMedExDesc,vMedExGroupCode,vMedExGroupDesc,vMedExSubGroupCode,vMedExSubGroupDesc,vMedExType,vUOM,vMedExValues,vDefaultValue".Split(",")).DefaultView.ToTable())
                                dsConvert.AcceptChanges()
                            Else
                                ds_MedExSubject.Tables(0).DefaultView.RowFilter = "nMedExScreeningHdrNo is not null"
                                dt_GenderWise = ds_MedExSubject.Tables(0).DefaultView.ToTable()
                                dsConvert.Tables.Add(dt_GenderWise.DefaultView.ToTable(True, "vMedExCode,vMedExDesc,vMedExGroupCode,vMedExGroupDesc,vMedExSubGroupCode,vMedExSubGroupDesc,vMedExType,vUOM,vMedExValues,vDefaultValue".Split(",")).DefaultView.ToTable())
                                dsConvert.AcceptChanges()
                            End If
                        Else
                            ds_MedExSubject.Tables(0).DefaultView.RowFilter = "nMedExScreeningHdrNo is not null"
                            dt_GenderWise = ds_MedExSubject.Tables(0).DefaultView.ToTable()
                            dsConvert.Tables.Add(dt_GenderWise.DefaultView.ToTable(True, "vMedExCode,vMedExDesc,vMedExGroupCode,vMedExGroupDesc,vMedExSubGroupCode,vMedExSubGroupDesc,vMedExType,vUOM,vMedExValues,vDefaultValue".Split(",")).DefaultView.ToTable())
                            dsConvert.AcceptChanges()
                        End If
                    End If

                    'Me.btnPdf.Style.Add("display", "")
                    Dim str As String = GetJson(dsConvert.Tables(0))
                    Me.hfWaterMark.Value = 1
                    Me.HdnPrint.Value = str
                    'Me.btnPdf.OnClientClick = "return fnGetPrintString(""S"")"

                    If Me.Session("VWMedExGroupCode") Is Nothing Then
                        ds_MedExSubject.Tables(0).DefaultView.RowFilter = "cScreeningType <> 'R'"
                    Else
                        Dim dt_MedExGropValue As New DataTable
                        Dim dt_medexfinal As New DataTable
                        Dim dtgroup As DataTable = CType(Me.Session("VWMedExGroupCode"), DataTable)
                        ds_MedExSubject.Tables(0).DefaultView.RowFilter = "cScreeningType <> 'R'"
                        dt_MedExGropValue = ds_MedExSubject.Tables(0).DefaultView.ToTable()
                        For Each row As DataRow In dtgroup.Rows
                            Dim strMedExGroupCode As String
                            strMedExGroupCode = row("vMedExGroupCode")
                            dt_MedExGropValue.DefaultView.RowFilter = "vMedExGroupCode = " + strMedExGroupCode
                            dt_medexfinal.Merge(dt_MedExGropValue.DefaultView.ToTable())
                        Next
                        If (ds_MedExSubject.Tables.Contains("Table")) Then
                            ds_MedExSubject.Tables.RemoveAt(0)
                            ds_MedExSubject.Tables.Add(dt_medexfinal)
                        End If
                    End If

                    Me.ViewState(VS_dtMedEx_Fill) = ds_MedExSubject.Tables(0).DefaultView.ToTable()
                    Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

                    If Not SetControls(CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Copy()) Then
                        Me.ShowErrorMessage("Error in SetControls", "...GenCall_Data")
                        Return False
                    End If
                End If

            ElseIf Me.rblScreeningDate.Items.Count > 0 AndAlso Me.rblScreeningDate.SelectedValue.ToUpper.Trim() = "N" Then 'For New

                If chkScreeningType.SelectedItem Is Nothing Then
                    Me.ObjCommon.ShowAlert("Please Select Screening Type.", Me.Page)
                    Me.rblScreeningDate.ClearSelection()
                    chkScreeningType.Enabled = True
                    Return False
                End If

                If chkScreeningType.Items(1).Selected Then
                    If HProjectId.Value.ToString = "" Or txtproject.Text.Trim = "" Then
                        Me.ObjCommon.ShowAlert("Please Select Project for Project Specific Screening.", Me.Page)
                        chkScreeningType.Enabled = True
                        Me.rblScreeningDate.ClearSelection()
                        Return False
                    Else
                        Wstr = "vWorkSpaceId='" + HProjectId.Value.ToString + "' AND cStatusIndi <>'" + Status_Delete + "'"
                        If Not objHelp.GetWorkspaceScreeningHdr(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr_retu) Then
                            Return False
                        End If
                        If ds.Tables(0).Rows.Count <= 0 Then
                            Me.ObjCommon.ShowAlert("No Template Is Attached For This Project. Please Attach Template For This Project.", Me.Page)
                            Return False
                        End If
                    End If
                End If

                Wstr = "cActiveFlag <>'" + ActiveFlag_No + "' AND cStatusIndi <>'" + Status_Delete + "' AND cStatusIndi_Dtl <> '" + Status_Delete + "'  And vMedExType <> 'IMPORT'"
                If chkScreeningType.Items(0).Selected Then
                    Wstr += " AND vWorkSpaceId='" & Me.Request.QueryString("Workspace") & "'" & _
                        " AND vUserLocationCode like'%" + L_LocationCode + "%'"
                    Wstr += " AND vUserTypeCode = '" + L_UserType + "'"
                End If

                If chkScreeningType.Items(1).Selected Then
                    Wstr += IIf(chkScreeningType.Items(0).Selected, " OR ", " AND ") + " vWorkSpaceId = " + HProjectId.Value.ToString
                    Wstr += " AND vUserTypeCode = '" + L_UserType + "'"
                Else
                    Wstr += " AND Getdate() BETWEEN ISNULL(dFromDate,GETDATE()) AND   ISNULL(dToDate,GETDATE())"
                    Wstr += " AND vUserTypeCode = '" + L_UserType + "'"
                End If

                If hdnSubGroup.Value <> "" Then
                    Wstr += " AND vMedExGroupCode = '" + Convert.ToString(hdnSubGroup.Value).Replace("Div", "") + "'"
                End If

                If hdnSubGroup.Value <> "" Then
                    Wstr += " AND  vMedExGroupCode = " + Convert.ToString(hdnSubGroup.Value).Replace("Div", "")
                ElseIf Request.QueryString("Group") <> Nothing Then
                    Wstr += " AND  vMedExGroupCode = " + Convert.ToString(Request.QueryString("Group"))
                End If

                Wstr += " Order by vWorkSpaceId,iSeqNo"
                If Not objHelp.GetViewScreeningTemplateDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr_retu) Then
                    GenCall_Data = False
                    Exit Function
                End If
                If ds.Tables(0).Rows.Count <= 0 Then
                    Me.ObjCommon.ShowAlert("Either No Attribute is Attached with Screening Or You don't have Group rights. So, please Attach Attribute to Screening or takes rights.", Me.Page)
                    'ObjCommon.ShowAlert("No default template available for this location.", Me.Page)
                    'rblScreeningDate.SelectedIndex = -1
                    Return False
                End If

                Me.ViewState(VS_dtMedEx_Fill) = ds.Tables(0)

                dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vMedExCode,vMedExDesc,vMedExGroupCode,vMedExGroupDesc,vMedExSubGroupCode,vMedExSubGroupDesc,vMedExType,vUOM,vMedExValues,vDefaultValue".Split(",")).DefaultView.ToTable())
                dsConvert.AcceptChanges()
                'Me.btnPdf.Style.Add("display", "")
                Dim str As String = GetJson(dsConvert.Tables(0))
                Me.HdnPrint.Value = str
                Me.hfWaterMark.Value = 0
                'Me.btnPdf.OnClientClick = "return fnGetPrintString(""N"")"
            End If

            If Not IsNothing(Me.Request.QueryString("mode")) AndAlso _
                Me.Request.QueryString("mode").ToString.Trim() = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View
            End If

            If Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso _
                                 (Me.Request.QueryString("SubjectId").ToString.Trim() <> "") Then
            End If

            ds = New DataSet
            If Not objHelp.GetMedExScreeningDtl("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubMedex, estr_retu) Then
                Return False
            End If

            If Not objHelp.GetMedExScreeningHdr("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubMedExMst, estr_retu) Then
                Return False
            End If

            If ds_SubMedex Is Nothing Then
                Throw New Exception(estr_retu)
            End If

            Me.ViewState(VS_dsSubMedex) = ds_SubMedex 'Blank Data Structer for Saveing
            Me.ViewState(VSDtSubMedExMst) = ds_SubMedExMst
            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message.ToString, estr_retu + "...GenCall_Data")
            Return False
        End Try
    End Function

#End Region

#Region "GenCall_ShowUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim dt As New DataTable
        Dim dv As New DataView
        Dim dt_MedExMst As New DataTable
        Dim dr As DataRow
        Dim drGroup As DataRow
        Dim objelement As Object
        Dim StrValidation() As String
        Dim StrGroup(2) As String
        Dim StrGroupCodes As String = String.Empty
        Dim StrGroupDesc As String = String.Empty
        Dim dv_MedexGroup As New DataView
        Dim dt_MedexGroup As New DataTable
        Dim i As Integer = 0
        Dim PrevSubGroupCodes As String = String.Empty
        Dim CntSubGroup As Integer = 0
        Dim CountForFemale As Integer = 0
        Dim sender As Object = Nothing
        Dim e As EventArgs = Nothing
        Dim dsUser As New DataSet
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim objimage As Object
        Dim Copyright As String = String.Empty
        Dim clientname As String = String.Empty
        Dim dt_profiles As New DataTable
        Dim dv_profiles As DataView
        Dim TranNo As Integer = 0
        Dim cDataStatus As String = String.Empty
        Dim nMedExScreeningDtlNo As Integer = 0
        Dim ds_MedExSubjectScreeningDtl1 As DataSet

        Try
            Copyright = System.Configuration.ConfigurationManager.AppSettings("CopyRight")

            Page.Title = " :: Subject Screening :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            Me.CompanyName.Value = Copyright

            dt_profiles = CType(Me.Session(S_Profiles), DataTable)
            dv_profiles = dt_profiles.DefaultView
            dv_profiles.RowFilter = "vUserTypeCode = '" + L_UserType + "'"
            'Me.btnOk.OnClientClick = "return ValidateReview('" & L_FirstName + " " + L_LastName & "','" & dv_profiles.ToTable.Rows(0)("vUserTypeName").ToString() & "','1') false;"
            'Change Vineet'
            If Me.Request.QueryString("mode") = 4 AndAlso Me.Request.QueryString("Workspace").ToString = "0000000000" Then
                If System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge").Contains(Session(S_LocationCode)) = True Then
                    Me.AutoCompleteExtender1.ContextKey = Me.Session(S_LocationCode)
                    Me.AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_OnlyID"
                Else
                    Me.AutoCompleteExtender1.ContextKey = System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge")
                    Me.AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_OnlyIDDataMerg"
                End If
            Else
                If System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge").Contains(Session(S_LocationCode)) = True Then
                    Me.AutoCompleteExtender1.ContextKey = Me.Session(S_LocationCode)
                Else
                    Me.AutoCompleteExtender1.ContextKey = System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge")
                    Me.AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejectedDataMerg"
                End If
            End If
            Me.lblUserName.Text = L_FirstName + " " + L_LastName
            Me.lblTime.Text = Session(S_Time).ToString
            '------------------------------------------------------------
            'Me.AutoCompleteExtender1.ContextKey = L_LocationCode
            Me.AutoCompleteExtenderWorkSpace.ContextKey = "iUserId = " & L_UserId
            PlaceMedEx.Controls.Clear()
            ShowHideControls("S")
            Me.divAudit.Style.Add("display", "none")
            Me.btnDeleteScreenNo.Style.Add("display", "none")
            Me.btnRmkHistory.Style.Add("display", "none")
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

            ElseIf Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                If Me.rblScreeningDate.SelectedValue <> "N" AndAlso Me.rblScreeningDate.SelectedValue <> "" Then
                    Me.btnRmkHistory.Style.Add("display", "block")
                End If
            End If

            If Me.rblScreeningDate.SelectedIndex <= -1 Or IsNothing(Me.ViewState(VS_dtMedEx_Fill)) Then
                ShowHideControls("H")
                Return True
            End If

            If CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows.Count < 1 Then
                ShowHideControls("H")
                If Not IsPostBack Then
                    Me.ObjCommon.ShowAlert("Either No Attribute is Attached with Screening Or You don't have Group rights. So, please Attach Attribute to Screening or takes rights.", Me.Page)
                End If
                'Return False
            End If

            dv_MedexGroup = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).DefaultView
            StrGroup(0) = "vMedExGroupCode"
            StrGroup(1) = "vMedExGroupDesc"
            StrGroup(2) = "cDataStatus"
            dt_MedexGroup = dv_MedexGroup.ToTable(True, StrGroup)
            PlaceMedEx.Controls.Add(New LiteralControl("<fieldset id=""fldDetails"" class=""FieldSetBox"" style=""width: 100%; height: auto; float: left;"">"))
            PlaceMedEx.Controls.Add(New LiteralControl("<legend id=""Legend1"" runat=""server"" class=""LegendText"" style=""color: Black; font-size: 12px;""><img id=""imgfldsActivity"" alt=""SubjectSpecific"" src=""images/expand.jpg"" onclick=""displayProjectInfo(this,'tblAct');""runat=""server"" />Subject Screening Details</legend>"))
            PlaceMedEx.Controls.Add(New LiteralControl("<div id =""tblAct"">"))
            PlaceMedEx.Controls.Add(New LiteralControl("<Table  width=""100%"" >"))
            'For Tab Buttons
            For Each drGroup In dt_MedexGroup.Rows
                If StrGroupCodes <> "" Then
                    StrGroupCodes += ",Div" + drGroup("vMedExGroupCode")
                    StrGroupDesc += "," + drGroup("vMedExGroupDesc")
                Else
                    StrGroupCodes = "Div" + drGroup("vMedExGroupCode")
                    StrGroupDesc = "" + drGroup("vMedExGroupDesc")
                End If
            Next drGroup

            Me.ViewState(VS_MedexGroups) = StrGroupCodes.Trim()
            Dim ds_MedExSubjectScreeningDtl As DataSet
            Dim estr_retu As String

            wstr = "cActiveFlag <>'" + ActiveFlag_No + "' AND cStatusIndi <>'" + Status_Delete + "' AND cStatusIndi_Dtl <> '" + Status_Delete + "'  And vMedExType <> 'IMPORT'"
            If chkScreeningType.Items(0).Selected Then
                wstr += " AND vWorkSpaceId='" & Me.Request.QueryString("Workspace") & "'" & _
                    " AND vUserLocationCode like'%" + L_LocationCode + "%'"
            End If

            If chkScreeningType.Items(1).Selected Then
                wstr += IIf(chkScreeningType.Items(0).Selected, " OR ", " AND ") + " vWorkSpaceId = '" + HProjectId.Value.ToString + "'"
                wstr += " AND vUserTypeCode = '" + L_UserType + "'"
            Else
                wstr += " AND Getdate() BETWEEN ISNULL(dFromDate,GETDATE()) AND   ISNULL(dToDate,GETDATE())"
                wstr += " AND vUserTypeCode = '" + L_UserType + "'"
            End If
            Dim ds_SubjectSexInfo As DataSet
            If HSubjectId.Value <> "" Then
                ds_SubjectSexInfo = objHelp.GetResultSet("SELECT cSex FROM SubjectMaster WHERE vSubjectID = '" + HSubjectId.Value + "'", "SubjectMaster")
                If Not ds_SubjectSexInfo Is Nothing And ds_SubjectSexInfo.Tables(0).Rows.Count > 0 Then
                    vSex = ds_SubjectSexInfo.Tables(0).Rows(0)(0)
                End If
            End If

            If vSex = "M" Then
                wstr += " AND vMedExGroupCode NOT IN (" + ConfigurationManager.AppSettings("FemaleMedExGroupCode") + ")"
            End If

            wstr += " Order by vWorkSpaceId,iSeqNo"

            If rblScreeningDate.SelectedValue = "N" Then



                If Not objHelp.GetViewScreeningTemplateDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_MedExSubjectScreeningDtl, estr_retu) Then

                End If
                Dim StrGroupCodes1 As String = ""
                Dim StrGroupDesc1 As String = ""
                Dim StrGroupDataStatus As String = ""

                StrGroup(0) = "vMedExGroupCode"
                StrGroup(1) = "vMedExGroupDesc"
                StrGroup(2) = "cDataStatus"

                Dim dvGroup As DataView = ds_MedExSubjectScreeningDtl.Tables(0).DefaultView
                Dim dt_MedexGroup1 As DataTable = dvGroup.ToTable(True, StrGroup)
                For Each drGroup In dt_MedexGroup1.Rows
                    If StrGroupCodes1 <> "" Then
                        StrGroupCodes1 += ",Div" + drGroup("vMedExGroupCode")
                        StrGroupDesc1 += "," + drGroup("vMedExGroupDesc")
                        StrGroupDataStatus += "," + drGroup("cDataStatus")
                    Else
                        StrGroupCodes1 = "Div" + drGroup("vMedExGroupCode")
                        StrGroupDesc1 = "" + drGroup("vMedExGroupDesc")
                        StrGroupDataStatus = "" + drGroup("cDataStatus")
                    End If
                Next drGroup
                Me.PhlReviewer.Controls.Clear()
                Me.hdnAllGroupId.Value = StrGroupCodes1


                If Not dt_MedexGroup1 Is Nothing AndAlso dt_MedexGroup1.Rows.Count > 0 Then
                    Me.Session("VWMedExGroupCode") = dt_MedexGroup1
                    Me.GetDropDownList(StrGroupDesc1, StrGroupCodes1, StrGroupDataStatus)
                    Dim lbl As New Label
                    lbl = Getlable("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", "lblRed")
                    lbl.BackColor = Drawing.Color.Red
                    Me.PhlReviewer.Controls.Remove(lbl)
                    Me.PhlReviewer.Controls.Add(lbl)
                    Me.PhlReviewer.Controls.Add(New LiteralControl("-Data Entry Pending, "))
                    lbl = New Label
                    lbl = Getlable("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", "lblGreen")
                    lbl.BackColor = Drawing.Color.Orange
                    Me.PhlReviewer.Controls.Remove(lbl)
                    Me.PhlReviewer.Controls.Add(lbl)
                    Me.PhlReviewer.Controls.Add(New LiteralControl("-Data Entry Continue ,"))
                    lbl = New Label
                    lbl = Getlable("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", "lblBlue")
                    lbl.BackColor = Drawing.Color.Blue
                    Me.PhlReviewer.Controls.Remove(lbl)
                    Me.PhlReviewer.Controls.Add(lbl)
                    Me.PhlReviewer.Controls.Add(New LiteralControl("-Data Entry Completed,"))
                    lbl = New Label
                    lbl = Getlable("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", "lblGray")
                    lbl.BackColor = Drawing.Color.Gray
                    Me.PhlReviewer.Controls.Remove(lbl)
                    Me.PhlReviewer.Controls.Add(lbl)
                    Me.PhlReviewer.Controls.Add(New LiteralControl("-Review Completed "))


                    If Request.QueryString("mode") = "4" Then
                        GETVIEWALLBUTTOn("View All", "ViewAll")
                    ElseIf Convert.ToString(L_WorkFlowStageId) <> WorkFlowStageId_DataEntry Then
                        GETVIEWALLBUTTOn("View All", "ViewAll")
                    End If


                    PlaceMedEx.Controls.Add(New LiteralControl("<img id=""imgActivityLegends"" src=""images/question.gif"" alt=""Activity Legends"" style="" height: auto; float: right;"" runat=""server""  onmouseover=""$('#divActivityLegends').toggle('medium');"" title=""Activity Legends"" />"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<fieldset id=""divActivityLegends"" class=""FieldSetBox"" style=""width: 40%; display:none; height: auto; float: Right;"">"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<legend id=""Legend1"" runat=""server"" class=""LegendText"" style=""color: Black; font-size: 12px;""></legend>"))
                    PlaceMedEx.Controls.Add(PhlReviewer)
                    PlaceMedEx.Controls.Add(New LiteralControl("</fieldset>"))

                Else
                    Me.ObjCommon.ShowAlert("Either No Attribute is Attached with Screening Or You don't have Group rights. So, please Attach Attribute to Screening or takes rights.", Me.Page)
                End If



            Else
                If (Me.Request.QueryString("mode").ToString.Trim() = "4") Then
                    wstr = IIf(Me.HSubjectId.Value.Trim() = "", "0000", Me.HSubjectId.Value.Trim()) + "##" + CDate(Me.rblScreeningDate.SelectedValue.Trim()).Date.ToString("dd-MMM-yyyy") + "##" + "" + "##" + "0"
                ElseIf (Not Me.Request.QueryString("ViewAll") Is Nothing AndAlso Me.Request.QueryString("ViewAll").ToString.Trim() = True) Then
                    wstr = IIf(Me.HSubjectId.Value.Trim() = "", "0000", Me.HSubjectId.Value.Trim()) + "##" + CDate(Me.rblScreeningDate.SelectedValue.Trim()).Date.ToString("dd-MMM-yyyy") + "##" + "" + "##" + L_UserType
                Else
                    wstr = IIf(Me.HSubjectId.Value.Trim() = "", "0000", Me.HSubjectId.Value.Trim()) + "##" + CDate(Me.rblScreeningDate.SelectedValue.Trim()).Date.ToString("dd-MMM-yyyy") + "##" + "" + "##" + L_UserType
                End If


                ds_MedExSubjectScreeningDtl = Me.objHelpdb.ProcedureExecute("dbo.Proc_SCREENINGTEMPLATEHDRDTL", wstr)
                ds_MedExSubjectScreeningDtl.Tables(0).DefaultView.Sort = "cScreeningType,vWorkSpaceId,iSeqNo"
                Dim nScreeningTemplateHdrNo As String = ""
                Dim vWorkSpaceIdForPeojectSpecific As String = ""
                Dim nMedExScreeningHdrNo As String = ""

                For Each dr1 As DataRow In ds_MedExSubjectScreeningDtl.Tables(0).Rows
                    nScreeningTemplateHdrNo = dr1("nScreeningTemplateHdrNo")
                    vWorkSpaceIdForPeojectSpecific = dr1("vWorkSpaceId")
                    nMedExScreeningHdrNo = dr1("nMedExScreeningHdrNo")
                    'nMedExWorkSpaceScreeningHdrNo = dr1("nMedExWorkSpaceScreeningHdrNo")
                    Exit For
                Next

                Dim wStrForvalidation As String

                If (Me.Request.QueryString("mode").ToString.Trim() = "4") Then
                    wStrForvalidation = nScreeningTemplateHdrNo + "##" + "111" + "##" + vWorkSpaceIdForPeojectSpecific + "##" + nMedExScreeningHdrNo + "##" + HSubjectId.Value
                Else
                    wStrForvalidation = nScreeningTemplateHdrNo + "##" + L_UserType + "##" + vWorkSpaceIdForPeojectSpecific + "##" + nMedExScreeningHdrNo + "##" + HSubjectId.Value
                End If


                ds_MedExSubjectScreeningDtl1 = Me.objHelpdb.ProcedureExecute("dbo.Proc_GetScreeningTemplateGroup", wStrForvalidation)

                StrGroup(0) = "vMedExGroupCode"
                StrGroup(1) = "vMedExGroupDesc"
                StrGroup(2) = "cDataStatus"

                Dim dvGroup As DataView = ds_MedExSubjectScreeningDtl1.Tables(0).DefaultView
                Dim dt_Group As DataTable = dvGroup.ToTable(True, StrGroup)
                ds_MedExSubjectScreeningDtl1 = New DataSet()
                ds_MedExSubjectScreeningDtl1.Tables.Add(dt_Group)

                If Not ds_MedExSubjectScreeningDtl1 Is Nothing AndAlso ds_MedExSubjectScreeningDtl1.Tables(0).Rows.Count > 0 Then
                    Dim StrGroupCodes1 As String = ""
                    Dim StrGroupDesc1 As String = ""
                    Dim StrGroupStatus As String = ""

                    StrGroup(0) = "vMedExGroupCode"
                    StrGroup(1) = "vMedExGroupDesc"
                    dvGroup = ds_MedExSubjectScreeningDtl1.Tables(0).DefaultView
                    Dim dt_MedexGroup1 As DataTable = dvGroup.ToTable(True, StrGroup)

                    Me.Session("VWMedExGroupCode") = dt_MedexGroup1

                    For Each drGroup In dt_MedexGroup1.Rows
                        If StrGroupCodes1 <> "" Then
                            StrGroupCodes1 += ",Div" + drGroup("vMedExGroupCode")
                            StrGroupDesc1 += "," + drGroup("vMedExGroupDesc")
                            StrGroupStatus += "," + drGroup("cDataStatus")
                        Else
                            StrGroupCodes1 = "Div" + drGroup("vMedExGroupCode")
                            StrGroupDesc1 = "" + drGroup("vMedExGroupDesc")
                            StrGroupStatus = "" + drGroup("cDataStatus")
                        End If
                    Next drGroup

                    Me.hdnAllGroupId.Value = StrGroupCodes1
                    Me.GetDropDownList(StrGroupDesc1, StrGroupCodes1, StrGroupStatus)

                    'Dim button As New Button
                    'button = GetButtons("", "lblRed")
                    'lbl.BackColor = Drawing.Color.Red
                    'Me.PhlReviewer.Controls.Add(lbl)
                    Me.PhlReviewer.Controls.Clear()
                    Dim lbl As New Label
                    lbl = Getlable("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", "lblRed")
                    lbl.BackColor = Drawing.Color.Red
                    PhlReviewer.Controls.Remove(lbl)
                    Me.PhlReviewer.Controls.Add(lbl)
                    Me.PhlReviewer.Controls.Add(New LiteralControl("-Data Entry Pending, "))
                    lbl = New Label
                    lbl = Getlable("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", "lblGreen")
                    lbl.BackColor = Drawing.Color.Orange
                    PhlReviewer.Controls.Remove(lbl)
                    Me.PhlReviewer.Controls.Add(lbl)
                    Me.PhlReviewer.Controls.Add(New LiteralControl("-Data Entry Continue, "))
                    lbl = New Label
                    lbl = Getlable("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", "lblBlue")
                    lbl.BackColor = Drawing.Color.Blue
                    PhlReviewer.Controls.Remove(lbl)
                    Me.PhlReviewer.Controls.Add(lbl)
                    Me.PhlReviewer.Controls.Add(New LiteralControl("-Data Entry Completed,"))
                    lbl = New Label
                    lbl = Getlable("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", "lblGray")
                    lbl.BackColor = Drawing.Color.Gray
                    Me.PhlReviewer.Controls.Remove(lbl)
                    Me.PhlReviewer.Controls.Add(lbl)
                    Me.PhlReviewer.Controls.Add(New LiteralControl("-Review Completed "))

                    If Request.QueryString("mode") = "4" Then
                        GETVIEWALLBUTTOn("View All", "ViewAll")
                    ElseIf Convert.ToString(L_WorkFlowStageId) <> WorkFlowStageId_DataEntry Then
                        GETVIEWALLBUTTOn("View All", "ViewAll")
                    End If

                    PlaceMedEx.Controls.Add(PhlReviewer)
                    PlaceMedEx.Controls.Add(New LiteralControl("<img id=""imgActivityLegends"" src=""images/question.gif"" alt=""Activity Legends"" style="" height: auto; float: right;"" runat=""server""  onmouseover=""$('#divActivityLegends').toggle('medium');"" title=""Activity Legends"" />"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<fieldset id=""divActivityLegends"" class=""FieldSetBox"" style=""width: 40%; display:none; height: auto; float: Right;"">"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<legend id=""Legend1"" runat=""server"" class=""LegendText"" style=""color: Black; font-size: 12px;""></legend>"))

                    PlaceMedEx.Controls.Add(PhlReviewer)
                    PlaceMedEx.Controls.Add(New LiteralControl("</fieldset>"))

                Else
                    Me.ObjCommon.ShowAlert("Either No Attribute is Attached with Screening Or You don't have Group rights. So, please Attach Attribute to Screening or takes rights.", Me.Page)
                End If

            End If





            PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT>"))
            PlaceMedEx.Controls.Add(New LiteralControl("<Td width=""100%"" white-space: nowrap;>"))
            Me.GetButtons(StrGroupDesc, StrGroupCodes)


            Me.BtnNext.OnClientClick = "return Next('" & StrGroupCodes & "');"
            Me.BtnPrevious.OnClientClick = "return Previous('" & StrGroupCodes & "');"

            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
            PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))


            PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT >"))
            PlaceMedEx.Controls.Add(New LiteralControl("<Td>"))

            If Me.hdnSubGroup.Value <> "" Then
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "SelectcueenrTab", "SelectCurrentTab();", True)
            End If

            If Not ValidateUserWiseScreeningEntry("VALIDATE", Me.Request.QueryString("SubId").ToString.Trim(), Me.Request.QueryString("ScrDt")) Then
                'ObjCommon.ShowAlert("Another User was working on this screening Date of this subjectid", Me.Page)
                For Each li As ListItem In rblScreeningDate.Items
                    'li.Selected = False
                Next
                Session("ScreeningGroup") = ""
                GroupValidation = True
                Dim url As String = Request.Url.ToString()
                url = url.Substring(0, url.IndexOf("Group"))
                Response.Redirect(url, False)
                Return False
                Exit Function
            End If


            If Me.hdnSubGroup.Value <> "" Or (Not Request.QueryString("ViewAll") Is Nothing AndAlso Request.QueryString("ViewAll") = True) Then

                If Not Request.QueryString("ViewAll") Is Nothing AndAlso Request.QueryString("ViewAll") <> True AndAlso Not Session("ScreeningGroup") Is Nothing AndAlso Session("ScreeningGroup").ToString() <> "" Then
                    Dim dvdt_MedexGroup As DataView = dt_MedexGroup.DefaultView
                    dvdt_MedexGroup.RowFilter = "vMedExGroupCode = " + Session("ScreeningGroup").ToString()
                    dt_MedexGroup = New DataTable
                    dt_MedexGroup = dvdt_MedexGroup.ToTable()
                End If

                For Each drGroup In dt_MedexGroup.Rows
                    dt = New DataTable
                    dt_MedExMst = New DataTable
                    dt = Me.ViewState(VS_dtMedEx_Fill)
                    dv = New DataView
                    dv = dt.DefaultView


                    If drGroup.Item("vMedExGroupCode") = "00037" Then
                        CountForFemale += 1
                    End If


                    'dv.RowFilter = "vMedExGroupCode='" & Convert.ToString(IIf(hdnSubGroup.Value <> "", Convert.ToString(hdnSubGroup.Value).Trim.Replace("Div", ""), drGroup("vMedExGroupCode").Trim())) & "'"
                    If Not Request.QueryString("ViewAll") Is Nothing AndAlso Request.QueryString("ViewAll") = True Then
                        dt_MedExMst = dv.ToTable()
                        dt_MedExMst = dt
                    Else
                        dv.RowFilter = "vMedExGroupCode='" & Convert.ToString(drGroup("vMedExGroupCode").Trim()) & "'"
                        dt_MedExMst = dv.ToTable()

                    End If

                    If i = 0 Then
                        PlaceMedEx.Controls.Add(New LiteralControl("<Div id='Div" & drGroup("vMedExGroupCode").ToString.Trim() & "' style=""display:''""" + _
                        IIf(CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View, _
                            " disabled = ""false"" >", " >")))
                    Else
                        PlaceMedEx.Controls.Add(New LiteralControl("<Div id='Div" & drGroup("vMedExGroupCode").ToString.Trim() & "' style=""display:none""" + _
                        IIf(CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View, _
                            " disabled = ""true"" >", " >")))
                    End If
                    PlaceMedEx.Controls.Add(New LiteralControl("<Table width=""100%"" cellspacing=""3px"">")) ' border=""1""



                    For Each dr In dt_MedExMst.Rows
                        HFScreeningHdrlNo.Value = dr("nMedExSCreeningHdrNo")
                        If Not CountForFemale = 0 Then
                            HfMaleCount.Value = dt_MedExMst.Rows.Count
                            CountForFemale = 0
                        End If

                        If PrevSubGroupCodes = "" Or PrevSubGroupCodes <> dr("vMedExSubGroupCode") Then
                            If dr("vMedExSubGroupCode").ToString <> "0000" Then
                                PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""99%"" align=LEFT >"))
                                PlaceMedEx.Controls.Add(New LiteralControl("<Td colspan=""3"" style=""vertical-align:middle;"">"))
                                PlaceMedEx.Controls.Add(New LiteralControl("<div style=""background-color: #ffcc66;border-radius:4px;height: 20px;padding-left: 15px;box-shadow: 5px 5px 5px #888;padding-top:3px;"">"))
                                PlaceMedEx.Controls.Add(Getlable(dr("vMedExSubGroupDesc").ToString.Trim(), dr("vMedExSubGroupCode") + CntSubGroup.ToString.Trim()))
                                CntSubGroup += 1
                                PlaceMedEx.Controls.Add(New LiteralControl("</div>"))
                                PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
                            End If
                        End If
                        If dr("vMedExType").ToString.ToUpper.Trim() = "LABEL" Then
                            PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT width=""99%"">"))
                            PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""vertical-align:middle;;"" colspan=""3"">"))
                            PrevSubGroupCodes = dr("vMedExSubGroupCode")
                        Else
                            PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT width=""99%"">"))
                            PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""vertical-align:middle;width:45%;padding-left:2%;"">"))
                            PrevSubGroupCodes = dr("vMedExSubGroupCode")
                            PlaceMedEx.Controls.Add(GetlableWithHistory(dr("vMedExDesc") & ": ", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("cScreeningType")))
                            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                            PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""vertical-align:middle;width:45%;"">"))

                        End If

                        If Not dt_MedExMst.Columns.Contains("iTranNo") Then
                            TranNo = 0
                        Else
                            TranNo = dr("iTranNo")
                        End If

                        If Not dt_MedExMst.Columns.Contains("cDataStatus") Then
                            cDataStatus = ""
                        Else
                            If Convert.ToString(dr("cDataStatus")) <> "" Then
                                cDataStatus = Convert.ToString(dr("cDataStatus"))
                            Else
                                dr("cDataStatus") = cDataStatus
                                dt_MedExMst.AcceptChanges()
                            End If

                        End If

                        If Not dt_MedExMst.Columns.Contains("nMedExScreeningDtlNo") Then
                            nMedExScreeningDtlNo = 0
                        Else
                            nMedExScreeningDtlNo = dr("nMedExScreeningDtlNo")
                        End If

                        If Not (dr("vValidationType") Is System.DBNull.Value) Then

                            If dr("vValidationType") <> "" And dr("vValidationType") <> "NA" Then
                                StrValidation = Convert.ToString(dr("vValidationType")).Trim().Split(",")
                                HFNumericScale.Value = 0
                                If StrValidation.Length > 2 Then
                                    HFNumericScale.Value = StrValidation(2).ToString()
                                End If

                                If StrValidation.Length > 1 Then
                                    objelement = GetObject(dr("vMedExType"), dr("vMedExCode"), dr("vMedExValues"), IIf(dr("vDefaultValue") Is System.DBNull.Value, "", dr("vDefaultValue")), dr("cScreeningType"), _
                                                            StrValidation(0).ToString.Trim(), StrValidation(1).ToString.Trim(), IIf(dr("vAlertonvalue") Is System.DBNull.Value, "", dr("vAlertonvalue")), _
                                                            IIf(dr("vAlertMessage") Is System.DBNull.Value, "", dr("vAlertMessage")), IIf(dr("vHighRange") Is System.DBNull.Value, "0", dr("vHighRange")), _
                                                            IIf(dr("vLowRange") Is System.DBNull.Value, "0", dr("vLowRange")), _
                                                            IIf(dr("vRefTable") Is System.DBNull.Value, "", dr("vRefTable")), _
                                                            IIf(dr("vRefColumn") Is System.DBNull.Value, "", dr("vRefColumn")), HFNumericScale.Value, cDataStatus, nMedExScreeningDtlNo, TranNo)
                                Else
                                    objelement = GetObject(dr("vMedExType"), dr("vMedExCode"), dr("vMedExValues"), IIf(dr("vDefaultValue") Is System.DBNull.Value, "", dr("vDefaultValue")), dr("cScreeningType"), _
                                                            StrValidation(0).ToString.Trim(), "", IIf(dr("vAlertonvalue") Is System.DBNull.Value, "", dr("vAlertonvalue")), _
                                                            IIf(dr("vAlertMessage") Is System.DBNull.Value, "", dr("vAlertMessage")), IIf(dr("vHighRange") Is System.DBNull.Value, "0", dr("vHighRange")), _
                                                            IIf(dr("vLowRange") Is System.DBNull.Value, "0", dr("vLowRange")), _
                                                            IIf(dr("vRefTable") Is System.DBNull.Value, "", dr("vRefTable")), _
                                                            IIf(dr("vRefColumn") Is System.DBNull.Value, "", dr("vRefColumn")), HFNumericScale.Value, cDataStatus, nMedExScreeningDtlNo, TranNo)
                                End If

                            Else
                                objelement = GetObject(dr("vMedExType"), dr("vMedExCode"), dr("vMedExValues"), IIf(dr("vDefaultValue") Is System.DBNull.Value, "", dr("vDefaultValue")), dr("cScreeningType"), _
                                                         "", "", IIf(dr("vAlertonvalue") Is System.DBNull.Value, "", dr("vAlertonvalue")), _
                                                            IIf(dr("vAlertMessage") Is System.DBNull.Value, "", dr("vAlertMessage")), IIf(dr("vHighRange") Is System.DBNull.Value, "0", dr("vHighRange")), _
                                                            IIf(dr("vLowRange") Is System.DBNull.Value, "0", dr("vLowRange")), _
                                                            IIf(dr("vRefTable") Is System.DBNull.Value, "", dr("vRefTable")), _
                                                            IIf(dr("vRefColumn") Is System.DBNull.Value, "", dr("vRefColumn")), HFNumericScale.Value, cDataStatus, nMedExScreeningDtlNo, TranNo)
                            End If
                        Else
                            objelement = GetObject(dr("vMedExType"), dr("vMedExCode"), dr("vMedExValues"), IIf(dr("vDefaultValue") Is System.DBNull.Value, "", dr("vDefaultValue")), dr("cScreeningType"), _
                                                        "", "", IIf(dr("vAlertonvalue") Is System.DBNull.Value, "", dr("vAlertonvalue")), _
                                                            IIf(dr("vAlertMessage") Is System.DBNull.Value, "", dr("vAlertMessage")), IIf(dr("vHighRange") Is System.DBNull.Value, "0", dr("vHighRange")), _
                                                            IIf(dr("vLowRange") Is System.DBNull.Value, "0", dr("vLowRange")), _
                                                            IIf(dr("vRefTable") Is System.DBNull.Value, "", dr("vRefTable")), _
                                                            IIf(dr("vRefColumn") Is System.DBNull.Value, "", dr("vRefColumn")), HFNumericScale.Value, cDataStatus, nMedExScreeningDtlNo, TranNo)
                        End If
                        PlaceMedEx.Controls.Add(objelement)

                        ''For File Type*************
                        If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                            If dr("vMedExType").ToString.ToUpper.Trim() = "FILE" AndAlso dr("vDefaultValue").ToString.Trim() <> "" Then
                                Dim HLnkFile As New HyperLink
                                HLnkFile.ID = "hlnk" + dr("vMedExCode").ToString.Trim()
                                HLnkFile.NavigateUrl = dr("vDefaultValue").ToString.Trim()
                                HLnkFile.Text = Path.GetFileName(dr("vDefaultValue").ToString.Trim())
                                HLnkFile.Target = "_blank"
                                PlaceMedEx.Controls.Add(HLnkFile)
                            End If
                        End If

                        If Not dr("vUOM") Is System.DBNull.Value AndAlso dr("vUOM") <> "" Then
                            PlaceMedEx.Controls.Add(Getlable(dr("vUOM"), "UOM" + dr("vMedExCode")))
                        End If

                        If dr("vMedExType").ToString.ToUpper.Trim() = "DATETIME" Then
                            objimage = GetDateImage(dr("vMedExCode"), objelement)
                        End If

                        If Is_FormulaEnabled Then
                            If Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_DataEntry Or _
                                        Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_FirstReview Then
                                If Me.rblScreeningDate.SelectedValue <> "N" Then
                                    PlaceMedEx.Controls.Add(GetAutoCalculateButton("Calc", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("vMedExFormula"), dr("vMedexType"), dr("cDataStatus"), ""))
                                Else
                                    PlaceMedEx.Controls.Add(GetAutoCalculateButton("Calc", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("vMedExFormula"), dr("vMedexType"), "", dr("iDecimalNo")))
                                End If

                                Is_FormulaEnabled = False
                            End If
                        End If
                        If Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "TEXTAREA" Then
                            PlaceMedEx.Controls.Add(New LiteralControl("<br/>"))
                            PlaceMedEx.Controls.Add(GetCountLable(dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), _
                                                                  dr("vMedExCode")))
                        End If

                        Try


                            If (Convert.ToString(ConfigurationManager.AppSettings("Medex_Weight")).Contains(dr("vMedExCode"))) Then
                                If (Convert.ToString(ConfigurationManager.AppSettings("Medex_Weight")).Contains(dr("vMedExCode")) And dr("vUOM") = "") Then
                                    PlaceMedEx.Controls.Add(New LiteralControl(" kg "))
                                End If
                                objelement.Attributes.Add("readOnly", "true")
                                PlaceMedEx.Controls.Add(GetWeightButton()) ' added by prayag for weightData
                                PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                            ElseIf (dr("vMedExCode") = "00321" And dr("vUOM") = "") Then
                                PlaceMedEx.Controls.Add(New LiteralControl(" cm </Td>"))
                            ElseIf (dr("vMedExCode") = "00323" And dr("vUOM") = "") Then
                                PlaceMedEx.Controls.Add(New LiteralControl(" kg/m2  </Td>"))
                            Else
                                PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                            End If
                        Catch ex As Exception
                            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                        End Try

                        PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""vertical-align:middle;width:10%;"">"))

                        If Request.QueryString("mode") = "4" AndAlso Convert.ToString(ConfigurationManager.AppSettings("ScreeningIndependentReviewer")).Contains(L_UserType.ToString()) AndAlso Convert.ToString(dr("vMedExType")).ToUpper() <> "LABEL" Then
                            PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("nMedExScreeningDtlNo"), dr("vWorkSpaceID"), dr("nMedExScreeningHdrNo"), dr("vMedexType"), dr("nScreeningDCFNo"), dr("cDCFStatus"), dr("vMedExDesc"), Convert.ToString(dr("DCFBY")), Convert.ToString(dr("DCFiWorkflowStageId"))))
                        End If

                        If dt_MedExMst.Columns.Contains("cDataStatus") Then
                            If rblScreeningDate.SelectedValue.ToString.ToString.Trim() <> "N" Then
                                If dr("cDataStatus").ToString() = "B" Then
                                    If dr("vMedExType").ToString.ToUpper.Trim() <> "LABEL" Then
                                        If dr("nMedExScreeningDtlNo").ToString() <> 0 Then
                                            If dr("vDefaultValue").ToString <> "" Or dr("iTranNo") > 1 And dr("vMedExType").ToString.ToUpper.Trim() <> "LABEL" Then
                                                If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

                                                    If dr("cDataStatus") = "B" AndAlso dr("IsScreeningLock") = 0 Then
                                                        If (L_WorkFlowStageId) = 0 AndAlso dr("vDefaultValue") <> "" Then
                                                            PlaceMedEx.Controls.Add(GetEditButton("Edit", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("nMedExScreeningDtlNo"), dr("vWorkSpaceID"), dr("nMedExScreeningHdrNo"), dr("vMedexType")))
                                                            PlaceMedEx.Controls.Add(GetUpdateButton("Update", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("nMedExScreeningDtlNo"), dr("vWorkSpaceID"), dr("nMedExScreeningHdrNo"), dr("vMedexType")))
                                                        End If

                                                    Else
                                                        If dr("IsScreeningLock") = 0 AndAlso Convert.ToString(dr("vMedExType")).ToUpper() <> "LABEL" Then
                                                            PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("nMedExScreeningDtlNo"), dr("vWorkSpaceID"), dr("nMedExScreeningHdrNo"), dr("vMedexType"), dr("nScreeningDCFNo"), dr("cDCFStatus"), dr("vMedExDesc"), Convert.ToString(dr("DCFBY")), Convert.ToString(dr("DCFiWorkflowStageId"))))
                                                        End If
                                                    End If
                                                    If dr("vDefaultValue") <> "" Then
                                                        If dr("vMedExType").ToString().ToUpper = "CHECKBOX" Then
                                                            For Each li As ListItem In CType(objelement, CheckBoxList).Items
                                                                li.Enabled = False
                                                            Next
                                                        ElseIf dr("vMedExType").ToString().ToUpper = "RADIO" Then
                                                            For Each li As ListItem In CType(objelement, RadioButtonList).Items
                                                                li.Enabled = False
                                                            Next
                                                        Else


                                                            objelement.Attributes.Add("disabled", "true")
                                                        End If
                                                    End If
                                                End If
                                                'PlaceMedEx.Controls.Add(GetAudittrailButton("AuditTrail", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("cScreeningType"), dr("iTranNo"), Convert.ToString(dr("vRemarks")), L_LocationCode))
                                            End If
                                            PlaceMedEx.Controls.Add(GetAudittrailButton("AuditTrail", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("cScreeningType"), dr("iTranNo"), Convert.ToString(dr("vRemarks")), L_LocationCode))
                                        End If
                                    End If

                                ElseIf dr("cDataStatus").ToString() = "D" Then
                                    If dr("vMedExType").ToString.ToUpper.Trim() <> "LABEL" Then
                                        If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                                            If Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_SecondReview Or Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_DataEntry Then

                                                If dr("cDataStatus") <> "B" AndAlso dr("cDataStatus") <> "D" AndAlso dr("IsScreeningLock") = 0 Then
                                                    PlaceMedEx.Controls.Add(GetEditButton("Edit", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("nMedExScreeningDtlNo"), dr("vWorkSpaceID"), dr("nMedExScreeningHdrNo"), dr("vMedexType")))
                                                    PlaceMedEx.Controls.Add(GetUpdateButton("Update", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("nMedExScreeningDtlNo"), dr("vWorkSpaceID"), dr("nMedExScreeningHdrNo"), dr("vMedexType")))
                                                ElseIf dr("cDCFStatus") = "N" AndAlso Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_DataEntry AndAlso dr("IsScreeningLock") = 0 Then
                                                    PlaceMedEx.Controls.Add(GetEditButton("Edit", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("nMedExScreeningDtlNo"), dr("vWorkSpaceID"), dr("nMedExScreeningHdrNo"), dr("vMedexType")))
                                                    PlaceMedEx.Controls.Add(GetUpdateButton("Update", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("nMedExScreeningDtlNo"), dr("vWorkSpaceID"), dr("nMedExScreeningHdrNo"), dr("vMedexType")))
                                                ElseIf dr("cDCFStatus") = "N" AndAlso Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_DataEntry Then
                                                    PlaceMedEx.Controls.Add(GetEditButton("Edit", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("nMedExScreeningDtlNo"), dr("vWorkSpaceID"), dr("nMedExScreeningHdrNo"), dr("vMedexType")))
                                                    PlaceMedEx.Controls.Add(GetUpdateButton("Update", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("nMedExScreeningDtlNo"), dr("vWorkSpaceID"), dr("nMedExScreeningHdrNo"), dr("vMedexType")))
                                                End If

                                            End If

                                            'If (dr("SecondReviewCompleted") <> 0) Then
                                            '    'PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("nMedExScreeningDtlNo"), dr("vWorkSpaceID"), dr("nMedExScreeningHdrNo"), dr("vMedexType"), dr("nScreeningDCFNo"), dr("cDCFStatus")))

                                            If Convert.ToString(ConfigurationManager.AppSettings("ScreeningIndependentReviewer")).Contains(L_UserType) AndAlso dr("IsScreeningLock") = 0 AndAlso Convert.ToString(dr("vMedExType")).ToUpper() <> "LABEL" Then
                                                PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("nMedExScreeningDtlNo"), dr("vWorkSpaceID"), dr("nMedExScreeningHdrNo"), dr("vMedexType"), dr("nScreeningDCFNo"), dr("cDCFStatus"), dr("vMedExDesc"), Convert.ToString(dr("DCFBY")), Convert.ToString(dr("DCFiWorkflowStageId"))))
                                            ElseIf dr("cDCFStatus") = "N" AndAlso Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_DataEntry AndAlso dr("IsScreeningLock") = 0 AndAlso Convert.ToString(dr("vMedExType")).ToUpper() <> "LABEL" Then
                                                PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("nMedExScreeningDtlNo"), dr("vWorkSpaceID"), dr("nMedExScreeningHdrNo"), dr("vMedexType"), dr("nScreeningDCFNo"), dr("cDCFStatus"), dr("vMedExDesc"), Convert.ToString(dr("DCFBY")), Convert.ToString(dr("DCFiWorkflowStageId"))))
                                            ElseIf dr("SecondReviewCompleted") = 0 AndAlso dr("cDataStatus") = "D" AndAlso dr("IsScreeningLock") = 0 AndAlso Convert.ToString(dr("vMedExType")).ToUpper() <> "LABEL" Then
                                                PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("nMedExScreeningDtlNo"), dr("vWorkSpaceID"), dr("nMedExScreeningHdrNo"), dr("vMedexType"), dr("nScreeningDCFNo"), dr("cDCFStatus"), dr("vMedExDesc"), Convert.ToString(dr("DCFBY")), Convert.ToString(dr("DCFiWorkflowStageId"))))
                                            End If

                                            PlaceMedEx.Controls.Add(GetAudittrailButton("AuditTrail", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("cScreeningType"), dr("iTranNo"), dr("vRemarks").ToString(), L_LocationCode))
                                            If dr("vMedExType").ToString().ToUpper = "CHECKBOX" Then
                                                For Each li As ListItem In CType(objelement, CheckBoxList).Items
                                                    li.Enabled = False
                                                Next
                                            ElseIf dr("vMedExType").ToString().ToUpper = "RADIO" Then
                                                For Each li As ListItem In CType(objelement, RadioButtonList).Items
                                                    li.Enabled = False
                                                Next
                                            Else
                                                objelement.Attributes.Add("disabled", "true")
                                            End If
                                        ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                                            PlaceMedEx.Controls.Add(GetAudittrailButton("AuditTrail", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("cScreeningType"), dr("iTranNo"), Convert.ToString(dr("vRemarks")), L_LocationCode))
                                        End If
                                    End If
                                End If
                            Else
                                If Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_DataEntry AndAlso Not Request.QueryString("Group") Is Nothing AndAlso Request.QueryString("Group") <> "" Then
                                    'Me.btnContinueSave.Style.Add("display", "")
                                    Me.BtnSave.Style.Add("display", "")
                                End If
                                'Me.btnRmkHistory.Style.Add("display", "Block")

                            End If
                        End If
                        PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                        PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                    Next
                    i += 1
                    PlaceMedEx.Controls.Add(New LiteralControl("</Table>")) '</Tr>
                    PlaceMedEx.Controls.Add(New LiteralControl("</Div>")) '</Tr>
                    Exit For
                Next drGroup

            End If
            If Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_DataEntry AndAlso Not Request.QueryString("Group") Is Nothing AndAlso Request.QueryString("Group") <> "" Then
                Me.btnContinueSave.Style.Add("display", "")
                Me.BtnSave.Style.Add("display", "")
            End If
            'Me.btnRmkHistory.Style.Add("display", "Block")


            PlaceMedEx.Controls.Add(New LiteralControl("</Td>")) '</Tr>
            PlaceMedEx.Controls.Add(New LiteralControl("</Tr>")) '</Tr>

            PlaceMedEx.Controls.Add(New LiteralControl("</Table>"))
            PlaceMedEx.Controls.Add(New LiteralControl("</div>"))
            PlaceMedEx.Controls.Add(New LiteralControl("</fieldset>"))

            Dim dt1 As DataTable = Me.ViewState(VS_dtMedEx_Fill)

            For Each dr1 As DataRow In dt1.Rows
                If dt1.Columns.Contains("cDataStatus") Then
                    If dr1("cDataStatus") = "D" Then
                        BtnSave.Visible = False
                        btnContinueSave.Visible = False
                        Exit For
                    ElseIf dr1("cDataStatus") = "B" Then
                        BtnSave.Visible = True
                        btnContinueSave.Visible = True
                        Exit For
                    End If
                ElseIf Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_SecondReview Then
                    BtnSave.Visible = False
                    btnContinueSave.Visible = False
                End If
            Next


            '' Added By Ketan
            If txtproject.Text.ToString.Trim() <> "" AndAlso txtSubject.Text.ToString.Trim() <> "" AndAlso ddlScreeningDate.SelectedValue.ToString() = "N" AndAlso Request.QueryString("Group") <> "" Then
                Dim tempProject As String = txtproject.Text.Split("]")(0).Substring(1, txtproject.Text.Split("]")(0).Length - 1)
                Dim tempSubject As String = txtSubject.Text.Split("(")(1).Substring(0, txtSubject.Text.Split("(")(1).Length - 1)
                Dim tempDate As String = ddlScreeningDate.SelectedValue.ToString()
                Dim tempGroup As String = Request.QueryString("Group")

                Me.Session("PlaceMedEx_" + tempProject + tempSubject + tempDate + tempGroup) = PlaceMedEx.Controls

            ElseIf txtproject.Text.ToString.Trim() <> "" AndAlso txtSubject.Text.ToString.Trim() <> "" AndAlso ddlScreeningDate.SelectedValue.ToString() <> "M" AndAlso Request.QueryString("Group") <> "" Then
                Dim tempProject As String = txtproject.Text.Split("]")(0).Substring(1, txtproject.Text.Split("]")(0).Length - 1)
                Dim tempSubject As String = txtSubject.Text.Split("(")(1).Substring(0, txtSubject.Text.Split("(")(1).Length - 1)
                Dim tempDate As String = ddlScreeningDate.SelectedValue.ToString().Replace("/", "").Split(" ")(0)
                Dim tempGroup As String = Request.QueryString("Group")

                Me.Session("PlaceMedEx_" + tempProject + tempSubject + tempDate + tempGroup) = PlaceMedEx.Controls
            Else
                Me.Session("PlaceMedEx") = PlaceMedEx.Controls
            End If

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.BtnSave.Visible = False
                Me.btnContinueSave.Visible = False
            End If
            '************************************
            If Not dt_MedExMst Is Nothing AndAlso dt_MedExMst.Rows.Count > 0 Then
                If dt_MedExMst.Columns.Contains("FirstReviewCompleted") Then
                    'If dt_MedExMst.Rows(0)("FirstReviewCompleted") <> 0 Then
                    If Not ShowHideReview(Convert.ToString(dt_MedExMst.Rows(0)("nMedExScreeningHdrNo")), Convert.ToString(dt_MedExMst.Rows(0)("cDataStatus"))) Then
                        Me.ShowErrorMessage("Error While Showing Review Panel", "")
                        Exit Function
                    End If
                    'End If
                End If
            End If



            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message.ToString, "...GenCall_ShowUI")
            Return False
        End Try
    End Function

    Private Function ShowHideControls(ByVal Type As String) As Boolean

        If Type.ToUpper.Trim() = "H" Then
            ' Me.BtnQC.Visible = False
            Me.BtnPrevious.Visible = False
            Me.BtnNext.Visible = False
            'Me.BtnSave.Visible = False
            'Me.btnContinueSave.Visible = False
            'Me.txtremark.Visible = False
            'Me.lblRemarks.Visible = False
        ElseIf Type.ToUpper.Trim() = "S" Then
            'Me.BtnQC.Visible = True
            Me.BtnPrevious.Visible = True
            Me.BtnNext.Visible = True
            Me.BtnSave.Visible = True
            Me.btnContinueSave.Visible = True
            'Me.txtremark.Visible = True
            'Me.lblRemarks.Visible = True
        End If

        Return True
    End Function

    Private Function SetControls(ByVal dt_screening As DataTable) As Boolean
        Dim dv As New DataView
        Try
            chkScreeningType.ClearSelection()
            dt_screening.DefaultView.RowFilter = "cScreeningType= 'D'"
            If dt_screening.DefaultView.ToTable.Rows.Count > 0 Then
                chkScreeningType.Items(0).Selected = True
            End If
            dt_screening.DefaultView.RowFilter = ""
            dt_screening.DefaultView.RowFilter = "cScreeningType= 'P'"
            If dt_screening.DefaultView.ToTable.Rows.Count > 0 And Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                chkScreeningType.Items(1).Selected = True
                tr_WorkSpace.Style("display") = "block"
                HProjectId.Value = dt_screening.DefaultView.ToTable.Rows(0)("vWorkSpaceId").ToString
                txtproject.Text = "[" + dt_screening.DefaultView.ToTable.Rows(0)("vProjectNo").ToString + "] " + dt_screening.DefaultView.ToTable.Rows(0)("vRequestId").ToString
                dt_screening.DefaultView.RowFilter = ""
                Session("IsProjectSpecificScreening") = True
                Session("hProjectDescProject") = "[" + dt_screening.DefaultView.ToTable.Rows(0)("vProjectNo").ToString + "] " + dt_screening.DefaultView.ToTable.Rows(0)("vRequestId").ToString
                Session("hProjectIdWorkSpace") = dt_screening.DefaultView.ToTable.Rows(0)("vWorkSpaceId").ToString
            Else
                Session("IsProjectSpecificScreening") = False
                Session("hProjectDescProject") = ""
                Session("hProjectIdWorkSpace") = ""
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...SetControls")
            Return False
        End Try
    End Function

    Private Function ShowHideReview(ByVal ScreeningHdrNo As String, ByVal Status As String) As Boolean
        Dim ds_WorkFlow As New DataSet
        Dim wStr As String = String.Empty
        Dim dt_FiltterWorkflowPI As New DataTable
        Dim dt_FiltterWorkflowDE As New DataTable
        Try
            If Me.Request.QueryString("ScrDt") = Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim Or Me.ViewState(VS_CheckEvent) = 1 Then
                If Not Me.trReviewCompleted.Visible = True Or Me.Session(S_UserType) <> Me.Session(S_ScrProfileIndex) Then
                    If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                        wStr = "SELECT * FROM View_ScreeningWorkFlowDtl_Show WHERE nMedExScreeningHdrNo = '" + ScreeningHdrNo + "'"

                        ds_WorkFlow = objHelp.GetResultSet(wStr, "View_ScreeningWorkFlowDtl")

                        If Not ds_WorkFlow Is Nothing Then
                            If ds_WorkFlow.Tables(0).Rows.Count > 0 Then
                                Me.btnReviewHistory.Style.Add("display", "")
                                Me.btnReviewHistory.Attributes.Add("onclick", "return fnReviewAudit('" + ScreeningHdrNo + "','" + Session(S_LocationCode) + "','" + Convert.ToString(L_WorkFlowStageId).Trim() + "');")
                                ds_WorkFlow.Tables(0).DefaultView.RowFilter = "iWorkFlowStageId = 20"
                                dt_FiltterWorkflowPI = ds_WorkFlow.Tables(0).DefaultView.ToTable()
                                ds_WorkFlow.Tables(0).DefaultView.RowFilter = "iWorkFlowStageId = 0"
                                dt_FiltterWorkflowDE = ds_WorkFlow.Tables(0).DefaultView.ToTable()
                                If dt_FiltterWorkflowPI.Rows.Count > 0 Then
                                    If Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_SecondReview Then
                                        Me.trReviewCompleted.Visible = True
                                    Else
                                        Me.trReviewCompleted.Visible = True

                                    End If
                                Else
                                    If ds_WorkFlow.Tables(0).Rows.Count > 0 Then
                                        If Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_SecondReview Then
                                            Me.trReviewCompleted.Visible = True
                                        Else
                                            Me.trReviewCompleted.Visible = False
                                            Me.ChkEligible.Style.Add("display", "")
                                        End If
                                    Else
                                        Me.trReviewCompleted.Visible = False
                                    End If
                                End If

                                If dt_FiltterWorkflowPI.Rows.Count > 0 Then
                                    If Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_SecondReview Then
                                        If dt_FiltterWorkflowDE.Rows.Count < 1 Then
                                            If Convert.ToString(ds_WorkFlow.Tables(0).Rows(0)("cIsEligible").Trim()).ToUpper() = "YES" Then
                                                Me.rblEligibleReview.SelectedValue = "Y"
                                                For Each li As ListItem In rblEligibleReview.Items
                                                    li.Enabled = False
                                                Next
                                            ElseIf Convert.ToString(ds_WorkFlow.Tables(0).Rows(0)("cIsEligible").Trim()).ToUpper() = "NO" Then
                                                Me.rblEligibleReview.SelectedValue = "N"
                                                For Each li As ListItem In rblEligibleReview.Items
                                                    li.Enabled = False
                                                Next
                                            End If
                                        Else
                                            If Convert.ToString(ds_WorkFlow.Tables(0).Rows(0)("cIsEligible").Trim()).ToUpper() = "YES" Then
                                                Me.rblEligible.SelectedValue = "Y"
                                                For Each li As ListItem In rblEligible.Items
                                                    li.Enabled = False
                                                Next
                                            ElseIf Convert.ToString(ds_WorkFlow.Tables(0).Rows(0)("cIsEligible").Trim()).ToUpper() = "NO" Then
                                                Me.rblEligible.SelectedValue = "N"
                                                For Each li As ListItem In rblEligible.Items
                                                    li.Enabled = False
                                                Next
                                            End If
                                        End If

                                    ElseIf Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_DataEntry Then
                                        If Convert.ToString(ds_WorkFlow.Tables(0).Rows(0)("cIsEligible").Trim()).ToUpper() = "YES" Then
                                            Me.rblEligible.SelectedValue = "Y"
                                            For Each li As ListItem In rblEligible.Items
                                                li.Enabled = False
                                            Next
                                        ElseIf Convert.ToString(ds_WorkFlow.Tables(0).Rows(0)("cIsEligible").Trim()).ToUpper() = "NO" Then
                                            Me.rblEligible.SelectedValue = "N"
                                            For Each li As ListItem In rblEligible.Items
                                                li.Enabled = False
                                            Next
                                        End If
                                    End If
                                Else
                                    If Convert.ToString(ds_WorkFlow.Tables(0).Rows(0)("cIsEligible").Trim()).ToUpper() = "YES" AndAlso Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_SecondReview Then
                                        'Me.trReviewCompleted.Visible = True
                                        'Me.ChkEligible.Style.Add("display", "none")
                                        'Me.chkReviewCompleted.Style.Add("display", "")
                                        'Me.chkEligibleReview.Style.Add("display", "none")
                                    End If
                                    If Convert.ToString(ds_WorkFlow.Tables(0).Rows(0)("cIsEligible").Trim()).ToUpper() = "NO" AndAlso Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_SecondReview Then
                                        'Me.trReviewCompleted.Visible = True
                                        'Me.ChkEligible.Style.Add("display", "none")
                                        'Me.chkReviewCompleted.Style.Add("display", "")
                                        'Me.chkEligibleReview.Style.Add("display", "none")
                                    End If
                                    'If Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_SecondReview Then
                                    '    'Me.btnOk.Style.Add("display", "")
                                    'End If
                                    If Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_DataEntry Then
                                        'Me.trReviewCompleted.Visible = True
                                        Me.ChkEligible.Style.Add("display", "")
                                        If Convert.ToString(ds_WorkFlow.Tables(0).Rows(0)("cIsEligible").Trim()).ToUpper() = "YES" Then
                                            Me.rblEligible.SelectedValue = "Y"
                                            For Each li As ListItem In rblEligible.Items
                                                li.Enabled = False
                                            Next
                                            'Me.btnOk.Style.Add("display", "none")
                                            'Me.btnReviewEdit.Style.Add("display", "")
                                            'Me.btnReviewEdit.Attributes.Add("onclick", "return fnReviewEdit('D');")
                                        ElseIf ds_WorkFlow.Tables(0).Rows(0)("cIsEligible").ToString() = "No" Then
                                            'Me.btnOk.Style.Add("display", "none")
                                            Me.rblEligible.SelectedValue = "N"
                                            For Each li As ListItem In rblEligible.Items
                                                li.Enabled = False
                                            Next
                                            'Me.btnOk.Style.Add("display", "none")
                                            'Me.btnReviewEdit.Style.Add("display", "")
                                            'Me.btnReviewEdit.Attributes.Add("onclick", "return fnReviewEdit('D');")
                                        End If
                                    End If
                                End If

                            ElseIf ds_WorkFlow.Tables(0).Rows.Count > 0 AndAlso Status = "D" AndAlso Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_DataEntry Then
                                Me.trReviewCompleted.Visible = True
                                ChkEligible.Visible = True
                                Me.ChkEligible.Style.Add("display", "")
                                Me.chkReviewCompleted.Style.Add("display", "none")
                                Me.chkEligibleReview.Style.Add("display", "none")
                            ElseIf ds_WorkFlow.Tables(0).Rows.Count = 0 AndAlso Status = "D" AndAlso Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_SecondReview Then
                                Me.trReviewCompleted.Visible = True
                                Me.chkEligibleReview.Style.Add("display", "")
                                'Me.ChkEligible.Style.Add("display", "none")
                                Me.chkReviewCompleted.Style.Add("display", "none")
                            End If
                        End If

                    End If
                    If Convert.ToString(L_WorkFlowStageId).Trim() <> WorkFlowStageId_DataEntry AndAlso Convert.ToString(L_WorkFlowStageId).Trim() <> WorkFlowStageId_SecondReview Then
                        Me.trReviewCompleted.Visible = False
                        'Me.ChkEligible.Style.Add("display", "none")
                        Me.chkReviewCompleted.Style.Add("display", "none")
                    End If
                    If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                        wStr = "SELECT * FROM View_ScreeningWorkFlowDtl_Show WHERE nMedExScreeningHdrNo = '" + ScreeningHdrNo + "'"
                        ds_WorkFlow = objHelp.GetResultSet(wStr, "View_ScreeningWorkFlowDtl")
                        If Not ds_WorkFlow Is Nothing Then
                            If ds_WorkFlow.Tables(0).Rows.Count > 0 Then
                                Me.btnReviewHistory.Style.Add("display", "")
                                Me.btnReviewHistory.Attributes.Add("onclick", "return fnReviewAudit('" + ScreeningHdrNo + "','" + Session(S_LocationCode) + "','" + Convert.ToString(L_WorkFlowStageId).Trim() + "');")
                            End If
                        End If
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "Error while Showing Review Grid")
            Return False
        End Try
    End Function

#End Region

#Region "GetLiteral"

    Private Function GetLiteral(ByVal text As String) As Literal
        Dim Lit As New Literal
        Lit.Text = text
        GetLiteral = Lit
    End Function

#End Region

#Region "Getlable"

    Private Function Getlable(ByVal vlabelName As String, ByVal Id As String) As Label
        Dim lab As New Label
        lab.ID = "Lab" & Id
        lab.Text = vlabelName.Trim()
        lab.SkinID = "lblDisplay"
        Getlable = lab
    End Function

    Private Function GetlableWithHistory(ByVal vlabelName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal ScreeningType As String) As Label
        Dim lnk As Label
        lnk = New Label
        lnk.ID = "Lnk" & MedExGroupCode + MedExSubGroupCode + MedExCode
        lnk.Text = vlabelName.Trim()
        lnk.SkinID = "LabelDisplay"
        lnk.CssClass = "Label"
        lnk.Style.Add("word-wrap", "break-word")
        lnk.Style.Add("white-space", "")
        GetlableWithHistory = lnk
    End Function



    Private Function GetCountLable(ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, _
                                       ByVal MedExCode As String) As Label
        Dim lbl As Label
        lbl = New Label
        lbl.ID = "lblCntText" & MedExGroupCode + MedExSubGroupCode + MedExCode
        lbl.CssClass = "CntTextArea"
        lbl.Text = "2000 characters remaining"
        lbl.Style.Add("font-size", "10px")

        GetCountLable = lbl

    End Function

#End Region

#Region "GetButtons"

    Private Function GetButtons(ByVal BtnName As String, ByVal Id As String) As Boolean
        Dim Btn As Button
        Dim index As Integer
        Dim StrGroupCode_arry() As String
        Dim StrGroupDesc_arry() As String
        Dim dtMedExMst As New DataTable
        Dim dvMedExMst As New DataView

        Try
            StrGroupCode_arry = Id.Split(",")
            StrGroupDesc_arry = BtnName.Split(",")
            For index = 0 To StrGroupCode_arry.Length - 1
                Btn = New Button
                Btn.ID = "Btn" & StrGroupCode_arry(index)
                Btn.Text = " " & StrGroupDesc_arry(index).Trim() & " "
                Btn.Style.Add("display", "none  !important;")

                If index = 0 Then
                    Btn.Style.Add("color", "white")
                End If
                Btn.CssClass = "HeadButton"

                Btn.OnClientClick = "return DisplayDiv('" & StrGroupCode_arry(index) & "','" & Id & "');"

                If Btn.Text.ToUpper() = MedExGroupDescForFemale.ToUpper() Then
                    Me.hfMedExGroupCode.Value = Btn.ID.Trim()
                    dtMedExMst = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable)
                    dvMedExMst = dtMedExMst.DefaultView
                    dvMedExMst.RowFilter = "vMedExCode in( '" + ConfigurationManager.AppSettings("MedExCodeForGender") + "')"

                    If dvMedExMst.ToTable().Rows.Count > 0 Then
                        If Convert.ToString(dvMedExMst.ToTable().Rows(0)("vDefaultValue")).Trim.ToUpper() = MedExResultForMale.ToUpper() Then
                            Btn.Text = MedExGroupDescForMale
                            Btn.Attributes.Add("disabled", "true")
                        End If
                    End If
                End If
                PlaceMedEx.Controls.Add(Btn)
            Next
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GetButtons")
            Return False
        End Try
    End Function

    Private Function GetDropDownList(ByVal BtnName As String, ByVal Id As String, ByVal DataStatus As String) As Boolean
        Dim Btn As Button
        Dim index As Integer
        Dim StrGroupCode_arry() As String
        Dim StrGroupDesc_arry() As String
        Dim StrDataStatus_arry() As String
        Dim dtMedExMst As New DataTable
        Dim dvMedExMst As New DataView
        Dim wStr As String
        Dim ds As DataSet
        Dim FreezeStatus As String

        ddlGroup.Items.Clear()

        Try
            Dim estrv As String = ""
            objHelpdb.Proc_ScreeningVersionStatus(ddlScreeningDate.SelectedValue.ToString(), HSubjectId.Value, ds, estrv)

            If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                FreezeStatus = ds.Tables(0).Rows(0)(0)
            End If

            StrGroupCode_arry = Id.Split(",")
            StrGroupDesc_arry = BtnName.Split(",")
            StrDataStatus_arry = DataStatus.Split(",")

            Dim ddl As DropDownList
            ddl = New DropDownList

            ddlGroup.Items.Add(New ListItem("--- Select Group ---", "Div00000", True))
            For index = 0 To StrGroupCode_arry.Length - 1
                'ddl.ID = "ddl" & StrGroupCode_arry(index)

                If (Request.QueryString("Group") <> "") Then
                    ddlGroup.Items.Add(New ListItem(StrGroupDesc_arry(index).ToUpper(), StrGroupCode_arry(index), True))
                    If FreezeStatus = "Freeze" Then
                        ddlGroup.Items.FindByValue(StrGroupCode_arry(index).ToString()).Attributes.Add("style", "color:Gray")
                    ElseIf StrDataStatus_arry(index) = "D" Then
                        ddlGroup.Items.FindByValue(StrGroupCode_arry(index).ToString()).Attributes.Add("style", "color:Blue")
                    ElseIf StrDataStatus_arry(index) = "B" Then
                        ddlGroup.Items.FindByValue(StrGroupCode_arry(index).ToString()).Attributes.Add("style", "color:Orange")
                    Else
                        ddlGroup.Items.FindByValue(StrGroupCode_arry(index).ToString()).Attributes.Add("style", "color:Red")
                    End If

                    hdnSubGroup.Value = "Div" + Request.QueryString("Group")
                Else
                    ddlGroup.Items.Add(New ListItem(StrGroupDesc_arry(index).ToUpper(), StrGroupCode_arry(index)))
                    If FreezeStatus = "Freeze" Then
                        ddlGroup.Items.FindByValue(StrGroupCode_arry(index).ToString()).Attributes.Add("style", "color:Gray")
                    ElseIf StrDataStatus_arry(index) = "D" Then
                        ddlGroup.Items.FindByValue(StrGroupCode_arry(index).ToString()).Attributes.Add("style", "color:Blue")
                    ElseIf StrDataStatus_arry(index) = "B" Then
                        ddlGroup.Items.FindByValue(StrGroupCode_arry(index).ToString()).Attributes.Add("style", "color:Orange")
                    Else
                        ddlGroup.Items.FindByValue(StrGroupCode_arry(index).ToString()).Attributes.Add("style", "color:Red")
                    End If

                End If



                ddlGroup.CssClass = "dropDownList"
                ddlGroup.CssClass = "dropDownListForGroup"
                If (StrGroupCode_arry(index).Replace("Div", "") = Request.QueryString("Group")) Then
                    ddlGroup.SelectedValue = StrGroupCode_arry(index)
                    If FreezeStatus = "Freeze" Then
                        ddlGroup.ForeColor = Color.Gray
                    ElseIf StrDataStatus_arry(index) = "D" Then
                        ddlGroup.ForeColor = Color.Blue
                    ElseIf StrDataStatus_arry(index) = "B" Then
                        ddlGroup.ForeColor = Color.Orange
                    Else
                        ddlGroup.ForeColor = Color.Red
                    End If
                End If

            Next

            PlaceMedEx.Controls.Add(ddlGroup)
            ddlGroup.Style.Add("display", "")
            If (Request.QueryString("Group") <> "") Then
                ddlGroup.SelectedValue = "Div" + Convert.ToString(Request.QueryString("Group"))
                'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "SelectGroup", "SelectGroup(" + Request.QueryString("Group") + ");", True)
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GetButtons")
            Return False
        End Try
    End Function

    Private Function GETVIEWALLBUTTOn(ByVal BtnName As String, ByVal Id As String) As Boolean
        Dim Btn As Button
        Try
            Btn = New Button
            Btn.Text = "View All"
            Btn.ID = "btn" + Id
            Btn.CssClass = "button"
            AddHandler Btn.Click, AddressOf ViewAll_Click
            PlaceMedEx.Controls.Remove(Btn)
            PlaceMedEx.Controls.Add(Btn)
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GetViewButtons")
            Return False
        End Try
    End Function

#End Region

#Region "GetObject"

    Private Function GetObject(ByVal vFieldType As String, ByVal Id As String, ByVal vMedExValues As String, _
                                ByVal dtValues As String, ByVal ScreeningType As String, Optional ByVal Validation As String = "", _
                                Optional ByVal length As String = "", Optional ByVal AlertonValue As String = "", _
                                Optional ByVal AlertMsg As String = "", _
                                Optional ByVal HighRange As String = "0", _
                                Optional ByVal LowRange As String = "0", _
                                Optional ByVal RefTable As String = "", _
                                Optional ByVal RefColumn As String = "", _
                                Optional ByVal NumericScale As String = "0", Optional ByVal cDataStatus As String = "", _
                                Optional ByVal nMedExScreeningDtlNo As String = "", Optional ByVal TranNo As Integer = 0) As Object
        Dim txt As TextBox
        Dim ddl As DropDownList
        Dim FileBro As FileUpload
        Dim RBL As RadioButtonList
        Dim CBL As CheckBoxList
        Dim lbl As Label
        Dim ds_SubjectMst As New DataSet
        Dim wStr As String = String.Empty
        Dim birthDate As Date

        Select Case vFieldType.ToUpper.Trim()
            Case "TEXTBOX"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = ScreeningType + " Required textBox"
                txt.Text = dtValues
                If length <> "" And length <> "0" Then
                    If Validation = "NU" Then
                        txt.MaxLength = length + 1
                    Else
                        txt.MaxLength = length
                    End If
                End If

                HighRange = IIf(HighRange = String.Empty, "0", HighRange)
                LowRange = IIf(LowRange = "", "0", LowRange)

                If Validation = "" Or Validation = "NA" Then
                    txt.Attributes.Add("onblur", "ValidateTextbox(0,this,'No Validation'," & HighRange & "," & LowRange & ");")

                ElseIf Validation = GeneralModule.Val_AN Then
                    txt.Attributes.Add("onblur", "ValidateTextbox(" & GeneralModule.Validation_Alphanumeric & ",this,'Please Enter AlphaNumeric Value');")

                ElseIf Validation = Val_NU Then
                    txt.Attributes.Add("onblur", "ValidateTextboxNumeric(" & GeneralModule.Validation_Numeric & ",this,'Please Enter Numeric or Blank Value','" & HighRange & "','" & LowRange & "','" & Validation & "' , '" & length & "' , '" & NumericScale & "');")
                ElseIf Validation = Val_IN Then
                    txt.Attributes.Add("onblur", "ValidateTextbox(" & GeneralModule.Validation_Integer & ",this,'Please Enter Integer or Blank Value');")
                ElseIf Validation = Val_AL Then
                    txt.Attributes.Add("onblur", "ValidateTextbox('" & GeneralModule.Validation_Alphabet & "',this,'Please Enter Alphabets only');")
                ElseIf Validation = Val_NNI Then
                    txt.Attributes.Add("onblur", "ValidateTextbox('" & GeneralModule.Validation_NotNullInteger & "',this,'Please Enter Integer Value');")
                ElseIf Validation = Val_NNU Then
                    txt.Attributes.Add("onblur", "ValidateTextbox('" & GeneralModule.Validation_NotNullNumeric & "',this,'Please Enter Numeric Value');")
                End If

                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    txt.Enabled = False
                End If
                '***********************
                If Convert.ToString(ConfigurationManager.AppSettings("Medex_Weight")).Contains(Id) Or Convert.ToString(ConfigurationManager.AppSettings("Medex_Height")).Contains(Id) Then
                    txt.Attributes.Add("onChange", "FillBMIValue('" & Convert.ToString(ConfigurationManager.AppSettings("Medex_Height")).Trim() & "','" & Convert.ToString(ConfigurationManager.AppSettings("Medex_Weight")).Trim() & "','" & Convert.ToString(ConfigurationManager.AppSettings("Medex_BMI")) & "');")
                    txt.Attributes.Add("onkeypress", "return isNumberKey(event);")
                    txt.Attributes.Add("MaxLength", "6")

                ElseIf Convert.ToString(ConfigurationManager.AppSettings("Medex_Temperature_F")).Contains(Id) Then
                    txt.Attributes.Add("onblur", "F2C('" & Convert.ToString(ConfigurationManager.AppSettings("Medex_Temperature_F")).Trim() & "','" & Convert.ToString(ConfigurationManager.AppSettings("Medex_Temperature_C")).Trim() & "'," & HighRange & "," & LowRange & ");")
                    Me.HFFerenhitToCelcius.Value = HighRange + "##" + LowRange

                ElseIf Convert.ToString(ConfigurationManager.AppSettings("Medex_Temperature_C")).Contains(Id) Then
                    txt.Attributes.Add("onblur", "C2F('" & Convert.ToString(ConfigurationManager.AppSettings("Medex_Temperature_C")).Trim() & "','" & Convert.ToString(ConfigurationManager.AppSettings("Medex_Temperature_F")).Trim() & "'," & HighRange & "," & LowRange & ");")
                ElseIf Id = GeneralModule.Medex_Eligibilitydeclaredby Then
                    txt.Attributes.Add("ReadOnly", "true")
                    txt.Attributes.Add("onchange", "SetEligibilitydeclaredon('" & GeneralModule.Medex_Eligibilitydeclaredby.Trim() & "','" & GeneralModule.Medex_Eligibilitydeclaredon.Trim() & "');")

                ElseIf Id = GeneralModule.Medex_RecordedBy_BMI Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_PI_Co_I_Designate Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_Physician Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_RecordedBy Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_RecordedBy_ECG Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_RecordedBy_Xray Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_RecordedBy_Lab Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_Eligibilitydeclaredon Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_PICommentsgivenon Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Convert.ToString(ConfigurationManager.AppSettings("Medex_BMI")).Contains(Id) Then
                    txt.Attributes.Add("ReadOnly", "true")

                End If
                GetObject = txt

            Case "COMBOBOX"
                Dim Arrvalue() As String = Nothing
                Dim i As Integer
                Dim ds_ddl As New DataSet
                Dim estr As String = String.Empty

                ddl = New DropDownList
                ddl.ID = Id
                ddl.CssClass = ScreeningType + " Required dropDownList"

                If RefTable.Trim() <> "" And RefColumn.Trim() <> "" Then

                    If Not objHelp.GetFieldsOfTable(RefTable.Trim(), RefColumn.Trim(), "", ds_ddl, estr) Then
                        Me.ObjCommon.ShowAlert(estr, Me.Page())
                        Return Nothing
                    End If
                    ds_ddl.Tables(0).DefaultView.Sort = RefColumn
                    ddl.DataSource = ds_ddl.Tables(0).DefaultView.ToTable()
                    ddl.DataTextField = RefColumn
                    ddl.DataValueField = RefColumn
                    ddl.DataBind()

                Else
                    Arrvalue = Split(vMedExValues, ",")
                    For i = 0 To Arrvalue.Length - 1
                        ddl.Items.Add(New ListItem(Arrvalue(i), Arrvalue(i)))
                    Next
                    If Not dtValues = "" Then
                        ddl.SelectedItem.Text = dtValues
                        ddl.SelectedItem.Value = dtValues
                    End If

                End If

                If AlertonValue.Trim() <> "" Then
                    ddl.Attributes.Add("onchange", "ddlAlerton('" & ddl.ClientID & "','" & AlertonValue & "','" & AlertMsg & "');")
                End If
                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    ddl.Enabled = False
                End If

                GetObject = ddl


            Case "RADIO"
                Dim Arrvalue() As String = Nothing
                Dim i As Integer
                Dim ds_Radio As New DataSet
                Dim estr As String = String.Empty
                RBL = New RadioButtonList
                RBL.ID = Id
                RBL.EnableViewState = True
                RBL.CssClass = ScreeningType + " Required rbl"
                If RefTable.Trim() <> "" And RefColumn.Trim() <> "" Then

                    If Not objHelp.GetFieldsOfTable(RefTable.Trim(), RefColumn.Trim(), "", ds_Radio, estr) Then
                        Me.ObjCommon.ShowAlert(estr, Me.Page())
                        Return Nothing
                    End If

                    ds_Radio.Tables(0).DefaultView.Sort = RefColumn
                    RBL.DataSource = ds_Radio.Tables(0).DefaultView.ToTable()
                    RBL.DataTextField = RefColumn
                    RBL.DataValueField = RefColumn
                    RBL.DataBind()

                Else
                    Arrvalue = Split(vMedExValues, ",")
                    For i = 0 To Arrvalue.Length - 1
                        RBL.Items.Add(New ListItem(Arrvalue(i), Arrvalue(i)))
                    Next

                    If Not dtValues = "" Then
                        RBL.SelectedValue = dtValues
                    End If
                    If ConfigurationManager.AppSettings("MedExCodeForGender").Contains(Id) Then
                        If dtValues = "" Then

                            wStr = "vSubjectId = '" + Me.HSubjectId.Value.Trim() + "' And cStatusIndi <> 'D'"
                            If Not objHelp.GetSubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            ds_SubjectMst, estr) Then
                                Me.ObjCommon.ShowAlert("Error While Setting Birth Date: " + estr, Me.Page)
                            End If

                            If ds_SubjectMst.Tables(0).Rows.Count > 0 AndAlso _
                              (Not ds_SubjectMst.Tables(0).Rows(0)("cSex") Is System.DBNull.Value) AndAlso _
                                      Convert.ToString(ds_SubjectMst.Tables(0).Rows(0)("cSex")).Trim() <> "" Then

                                If Convert.ToString(ds_SubjectMst.Tables(0).Rows(0)("cSex")).ToUpper.Trim() = "M" Then
                                    For value As Integer = 0 To Arrvalue.Length - 1
                                        If Arrvalue(value).ToUpper = "MALE" Then
                                            Gender = Arrvalue(value)
                                        End If
                                    Next

                                ElseIf Convert.ToString(ds_SubjectMst.Tables(0).Rows(0)("cSex")).ToUpper.Trim() = "F" Then
                                    For value As Integer = 0 To Arrvalue.Length - 1
                                        If Arrvalue(value).ToUpper = "FEMALE" Then
                                            Gender = Arrvalue(value)
                                        End If
                                    Next
                                End If
                                RBL.SelectedValue = Gender
                            End If
                        End If
                    End If
                End If

                'If Convert.ToString(RBL.SelectedValue).Trim.ToUpper() = "MALE" And Me.hfMedExGroupCode.Value.Trim() <> "" Then
                '    CType(Me.PlaceMedEx.FindControl(Me.hfMedExGroupCode.Value.Trim()), Button).Text = MedExGroupDescForMale
                '    CType(Me.PlaceMedEx.FindControl(Me.hfMedExGroupCode.Value.Trim()), Button).Attributes.Add("disabled", "true")
                'ElseIf Convert.ToString(RBL.SelectedValue).Trim.ToUpper() = "FEMALE" And Me.hfMedExGroupCode.Value.Trim() <> "" Then
                '    CType(Me.PlaceMedEx.FindControl(Me.hfMedExGroupCode.Value.Trim()), Button).Text = MedExGroupDescForFemale
                '    CType(Me.PlaceMedEx.FindControl(Me.hfMedExGroupCode.Value.Trim()), Button).Attributes.Remove("disabled")
                'End If

                RBL.RepeatDirection = RepeatDirection.Horizontal
                RBL.RepeatColumns = 4

                If AlertonValue.Trim() <> "" Then
                    RBL.Attributes.Add("onclick", "Alerton('" & RBL.ClientID & "','" & AlertonValue & "','" & AlertMsg & "');")
                End If

                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    RBL.Enabled = False
                End If
                If ConfigurationManager.AppSettings("MedExCodeForGender").Contains(RBL.ID) Then
                    Me.hfMedExCodeForGender.Value = RBL.ID

                ElseIf RBL.ID = Medex_ClinicallyFit Then
                    RBL.Attributes.Add("onclick", "JustAlert('" & HfUserName.Value & "','" & Medex_Physician & "','" & Id & "');")

                ElseIf RBL.ID = Medex_SubjectFoundEligible Then
                    RBL.Attributes.Add("onclick", "SetEligibilitydeclaredon('" & GeneralModule.Medex_Eligibilitydeclaredby.Trim() & "','" & GeneralModule.Medex_Eligibilitydeclaredon.Trim() & "','" & HfUserName.Value & "');")

                ElseIf RBL.ID = Medex_RecreationlDrug Then
                    RBL.Attributes.Add("onclick", "SetUserName('" & HfUserName.Value & "','" & GeneralModule.Medex_RecordedBy.Trim & "','" & Id & "');")

                ElseIf RBL.ID = Medex_Clinically_ECG Then
                    RBL.Attributes.Add("onclick", "SetUserName('" & HfUserName.Value & "','" & GeneralModule.Medex_RecordedBy_ECG.Trim & "','" & Id & "');")

                ElseIf RBL.ID = Medex_Clinically_Lab Then
                    RBL.Attributes.Add("onclick", "SetUserName('" & HfUserName.Value & "','" & GeneralModule.Medex_RecordedBy_Lab.Trim & "','" & Id & "');")

                ElseIf RBL.ID = Medex_Consent_SCr Then
                    RBL.Attributes.Add("onclick", "SetUserName('" & HfUserName.Value & "','" & GeneralModule.Medex_RecordedBy_BMI.Trim & "','" & Id & "');")

                ElseIf RBL.ID = Medex_Clinically_Xray Then
                    RBL.Attributes.Add("onclick", "SetUserName('" & HfUserName.Value & "','" & GeneralModule.Medex_RecordedBy_Xray.Trim & "','" & Id & "');")

                End If
                GetObject = RBL

            Case "CHECKBOX"
                Dim Arrvalue() As String = Nothing
                Dim Defvalue() As String = Nothing
                Dim i As Integer
                Dim ds_Check As New DataSet
                Dim estr As String = String.Empty

                CBL = New CheckBoxList
                CBL.ID = Id
                CBL.EnableViewState = True
                CBL.CssClass = ScreeningType + " Required chkbox"
                If RefTable.Trim() <> "" And RefColumn.Trim() <> "" Then

                    If Not objHelp.GetFieldsOfTable(RefTable.Trim(), RefColumn.Trim(), "", ds_Check, estr) Then
                        Me.ObjCommon.ShowAlert(estr, Me.Page())
                        Return Nothing
                    End If

                    ds_Check.Tables(0).DefaultView.Sort = RefColumn
                    CBL.DataSource = ds_Check.Tables(0).DefaultView.ToTable()
                    CBL.DataTextField = RefColumn
                    CBL.DataValueField = RefColumn
                    CBL.DataBind()
                Else
                    Arrvalue = Split(vMedExValues, ",")
                    For i = 0 To Arrvalue.Length - 1
                        CBL.Items.Add(New ListItem(Arrvalue(i), Arrvalue(i)))
                    Next
                End If

                CBL.ClearSelection()
                If Not dtValues = "" Then
                    Defvalue = Split(dtValues, ",")
                    For i = 0 To Defvalue.Length - 1
                        For Each item As ListItem In CBL.Items
                            If item.Value.Trim().ToUpper() = Defvalue(i).Trim().ToUpper() Then
                                item.Selected = True
                                Exit For
                            End If
                        Next item
                    Next i
                End If

                CBL.RepeatDirection = RepeatDirection.Horizontal
                CBL.RepeatColumns = 3
                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    CBL.Enabled = False
                End If
                GetObject = CBL

            Case "FILE"
                FileBro = New FileUpload
                FileBro.ID = "FU" + Id
                FileBro.EnableViewState = True
                FileBro.CssClass = ScreeningType + " Required textBox"
                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    FileBro.Enabled = False
                End If

                GetObject = FileBro

            Case "TEXTAREA"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = ScreeningType + " Required crfentrycontrol"
                txt.Text = dtValues
                txt.TextMode = TextBoxMode.MultiLine
                txt.Width = 462
                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    txt.Enabled = False
                    txt.Rows = Math.Round(txt.Text.ToString.Length / 20)
                End If
                If Id = GeneralModule.Medex_PIComments Then
                    Dim dt_profiles As New DataTable
                    Dim dv_profiles As DataView
                    Me.lblSignername.Text = L_FirstName + " " + L_LastName
                    dt_profiles = CType(Me.Session(S_Profiles), DataTable)
                    dv_profiles = dt_profiles.DefaultView
                    dv_profiles.RowFilter = "vUserTypeCode = '" + L_UserType + "'"
                    Me.lblSignerDesignation.Text = dv_profiles.ToTable.Rows(0)("vUserTypeName").ToString()
                    txt.Attributes.Add("onChange", "Authentication();")
                    Me.HMedex_Medex_PI_Co_I_Designate.Value = Medex_PI_Co_I_Designate
                    Me.HMedex_Medex_PICommentsgivenon.Value = GeneralModule.Medex_PICommentsgivenon.Trim()
                End If

                GetObject = txt

            Case "DATETIME"
                Dim eStr As String = String.Empty
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = ScreeningType + " Required textBox"
                txt.Text = dtValues
                If Convert.ToString(ConfigurationManager.AppSettings("Medex_DateOfBirth")).Contains(Id.ToString()) Then

                    If dtValues = "" Then
                        wStr = "vSubjectId = '" + Me.HSubjectId.Value.Trim() + "' And cStatusIndi <> 'D'"
                        If Not objHelp.GetSubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        ds_SubjectMst, eStr) Then
                            Me.ObjCommon.ShowAlert("Error While Setting Birth Date: " + eStr, Me.Page)
                        End If

                        If ds_SubjectMst.Tables(0).Rows.Count > 0 Then
                            If Not ds_SubjectMst.Tables(0).Rows(0)("dBirthDate") Is System.DBNull.Value AndAlso _
                            ds_SubjectMst.Tables(0).Rows(0)("dBirthDate").ToString.Trim() <> "" Then
                                birthDate = ds_SubjectMst.Tables(0).Rows(0)("dBirthDate")
                                txt.Text = birthDate.ToString("dd-MMM-yyyy")
                            End If
                        End If
                    End If
                    txt.Attributes.Add("onblur", "SetAge('" & Convert.ToString(ConfigurationManager.AppSettings("Medex_DateOfBirth")) & "','" & Convert.ToString(ConfigurationManager.AppSettings("Medex_Age")) & "','" & CDate(ObjCommon.GetCurDatetime(L_TimeZoneName)).Date.ToString("dd-MMM-yyyy") & "');")

                ElseIf Id = GeneralModule.Medex_Eligibilitydeclaredon Then
                    txt.Attributes.Add("ReadOnly", "true")
                ElseIf Id = GeneralModule.Medex_PICommentsgivenon Then
                    txt.Attributes.Add("ReadOnly", "true")
                Else
                    txt.Attributes.Add("OnChange", "DateConvertForScreening(this.value,this)")
                End If
                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    txt.Enabled = False
                End If
                GetObject = txt

            Case "TIME"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = ScreeningType + " Required textBox"
                txt.Text = dtValues
                txt.Attributes.Add("onblur", "TimeConvert(this.value,this)")


                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    txt.Enabled = False
                End If
                GetObject = txt

            Case "ASYNCDATETIME"
                Dim eStr As String = String.Empty
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = ScreeningType + " Required textBox"

                txt.Text = dtValues


                If Convert.ToString(ConfigurationManager.AppSettings("Medex_DateOfBirth")).Contains(Id) Then

                    If dtValues = "" Then

                        wStr = "vSubjectId = '" + Me.HSubjectId.Value.Trim() + "' And cStatusIndi <> 'D'"
                        If Not objHelp.GetSubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        ds_SubjectMst, eStr) Then
                            Me.ObjCommon.ShowAlert("Error While Setting Birth Date: " + eStr, Me.Page)
                        End If

                        If ds_SubjectMst.Tables(0).Rows.Count > 0 Then
                            If Not ds_SubjectMst.Tables(0).Rows(0)("dBirthDate") Is System.DBNull.Value AndAlso _
                            ds_SubjectMst.Tables(0).Rows(0)("dBirthDate").ToString.Trim() <> "" Then

                                birthDate = ds_SubjectMst.Tables(0).Rows(0)("dBirthDate")
                                txt.Text = birthDate.ToString("dd-MMM-yyyy")
                            End If
                        End If

                    End If

                    txt.Attributes.Add("onblur", "SetAge('" & Convert.ToString(ConfigurationManager.AppSettings("Medex_DateOfBirth")).Trim() & "','" & Convert.ToString(ConfigurationManager.AppSettings("Medex_Age")).Trim() & "','" & CDate(ObjCommon.GetCurDatetime(L_TimeZoneName)).Date.ToString("dd-MMM-yyyy") & "');")

                ElseIf Id = GeneralModule.Medex_Eligibilitydeclaredon Then
                    txt.Attributes.Add("ReadOnly", "true")
                ElseIf Id = GeneralModule.Medex_PICommentsgivenon Then
                    txt.Attributes.Add("ReadOnly", "true")
                Else

                    txt.Attributes.Add("onblur", "DateConvertForScreening(this.value,this)")
                End If


                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    txt.Enabled = False
                End If

                GetObject = txt

            Case "ASYNCTIME"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = ScreeningType + " Required textBox"
                txt.Text = dtValues
                txt.Attributes.Add("onblur", "TimeConvert(this.value,this)")
                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    txt.Enabled = False
                End If
                GetObject = txt

            Case "LABEL"
                lbl = New Label
                lbl.ID = "lbl" + Id
                lbl.EnableViewState = True
                lbl.CssClass = ScreeningType + " Label"
                lbl.Style.Add("word-wrap", "break-word")
                lbl.Style.Add("white-space", "none")
                lbl.Text = vMedExValues.Trim()
                lbl.ToolTip = dtValues.Trim()
                GetObject = lbl

            Case "CRFTERM"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = ScreeningType + " textBox"
                txt.Text = dtValues
                GetObject = txt

            Case "FORMULA"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = ScreeningType + " Required textBox"
                txt.Text = dtValues
                txt.Attributes.Add("title", dtValues)
                HighRange = IIf(HighRange = "", "0", HighRange)
                LowRange = IIf(LowRange = "", "0", LowRange)

                Dim checktype As String = ""
                Dim msg As String = ""
                If Validation = "" Or Validation = "NA" Then
                    txt.Attributes.Add("onblur", "ValidateTextbox(0,this,'No Validation'," & HighRange & "," & LowRange & ");")

                ElseIf Validation = GeneralModule.Val_AN Then
                    txt.Attributes.Add("onblur", "ValidateTextbox(" & GeneralModule.Validation_Alphanumeric & ",this,'Please Enter AlphaNumeric Value');")

                ElseIf Validation = Val_NU Then
                    txt.Attributes.Add("onblur", "ValidateTextboxNumeric(" & GeneralModule.Validation_Numeric & ",this,'Please Enter Numeric or Blank Value','" & HighRange & "','" & LowRange & "','" & Validation & "' , '" & length & "' , '" & NumericScale & "');")
                ElseIf Validation = Val_IN Then
                    txt.Attributes.Add("onblur", "ValidateTextbox(" & GeneralModule.Validation_Integer & ",this,'Please Enter Integer or Blank Value');")
                ElseIf Validation = Val_AL Then
                    txt.Attributes.Add("onblur", "ValidateTextbox('" & GeneralModule.Validation_Alphabet & "',this,'Please Enter Alphabets only');")
                ElseIf Validation = Val_NNI Then
                    txt.Attributes.Add("onblur", "ValidateTextbox('" & GeneralModule.Validation_NotNullInteger & "',this,'Please Enter Integer Value');")
                ElseIf Validation = Val_NNU Then
                    txt.Attributes.Add("onblur", "ValidateTextbox('" & GeneralModule.Validation_NotNullNumeric & "',this,'Please Enter Numeric Value');")
                End If
                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    txt.Enabled = False
                End If
                If Convert.ToString(ConfigurationManager.AppSettings("Medex_Weight")).Contains(Id) Or Convert.ToString(ConfigurationManager.AppSettings("Medex_Height")).Contains(Id) Then
                    txt.Attributes.Add("onChange", "FillBMIValue('" & Convert.ToString(ConfigurationManager.AppSettings("Medex_Height")).Trim() & "','" & Convert.ToString(ConfigurationManager.AppSettings("Medex_Weight")).Trim() & "','" & Convert.ToString(ConfigurationManager.AppSettings("Medex_BMI")) & "');")

                ElseIf Convert.ToString(ConfigurationManager.AppSettings("Medex_Temperature_F")).Contains(Id) Then
                    txt.Attributes.Add("onblur", "F2C('" & Convert.ToString(ConfigurationManager.AppSettings("Medex_Temperature_F")).Trim() & "','" & Convert.ToString(ConfigurationManager.AppSettings("Medex_Temperature_C")).Trim() & "'," & HighRange & "," & LowRange & ");")
                    Me.HFFerenhitToCelcius.Value = HighRange + "##" + LowRange

                ElseIf Convert.ToString(ConfigurationManager.AppSettings("Medex_Temperature_C")).Contains(Id) Then
                    txt.Attributes.Add("onblur", "C2F('" & Convert.ToString(ConfigurationManager.AppSettings("Medex_Temperature_C")).Trim() & "','" & Convert.ToString(ConfigurationManager.AppSettings("Medex_Temperature_F")).Trim() & "'," & HighRange & "," & LowRange & ");")
                ElseIf Id = GeneralModule.Medex_Eligibilitydeclaredby Then
                    txt.Attributes.Add("ReadOnly", "true")
                    txt.Attributes.Add("onchange", "SetEligibilitydeclaredon('" & GeneralModule.Medex_Eligibilitydeclaredby.Trim() & "','" & GeneralModule.Medex_Eligibilitydeclaredon.Trim() & "');")
                ElseIf Id = GeneralModule.Medex_RecordedBy_BMI Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_PI_Co_I_Designate Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_Physician Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_RecordedBy Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_RecordedBy_ECG Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_RecordedBy_Xray Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_RecordedBy_Lab Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_Eligibilitydeclaredon Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_PICommentsgivenon Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Convert.ToString(ConfigurationManager.AppSettings("Medex_BMI")).Contains(Id) Then
                    txt.Attributes.Add("ReadOnly", "true")
                End If
                Is_FormulaEnabled = True
                GetObject = txt

            Case "STANDARDDATE"
                Dim ddlDate As New DropDownList
                Dim ddlMonth As New DropDownList
                Dim ddlYear As New DropDownList
                Dim str As String = String.Empty
                Dim estr As String = String.Empty
                Dim dsDate As New DataSet

                ddlDate.ID = Id + "_1"
                ddlDate.CssClass = "dropDownList"
                ddlDate.Width = 80
                ddlMonth.ID = Id + "_2"
                ddlMonth.CssClass = "dropDownList"
                ddlMonth.Width = 80
                ddlYear.ID = Id + "_3"
                ddlYear.CssClass = "dropDownList"
                ddlYear.Width = 80

                ddlYear.Attributes.Add("StandardDate", "Y")
                ddlMonth.Attributes.Add("StandardDate", "Y")
                ddlDate.Attributes.Add("StandardDate", "Y")

                ddlDate.CssClass = ScreeningType + " Required dropDownList"
                ddlMonth.CssClass = ScreeningType + " Required dropDownList"
                ddlYear.CssClass = ScreeningType + " Required dropDownList"


                If Not objHelp.GetDatesMonthsAndYears("PROC_GetDatesMonthsAndYears", dsDate, estr) Then
                    Throw New Exception("Error While Getting Dates,Months and Years.")
                End If

                ddlDate.DataSource = dsDate.Tables(0)
                ddlDate.DataTextField = "Dates"
                ddlDate.DataValueField = "Dates"
                ddlDate.DataBind()
                ddlDate.Items.Insert(0, New ListItem("dd", ""))
                ddlDate.Items.Insert(1, New ListItem("UK", "00"))

                ddlMonth.DataSource = dsDate.Tables(1)
                ddlMonth.DataTextField = "Months"
                ddlMonth.DataValueField = "Val"
                ddlMonth.DataBind()
                ddlMonth.Items.Insert(0, New ListItem("MMM", ""))
                ddlMonth.Items.Insert(1, New ListItem("UNK", "00"))

                ddlYear.DataSource = dsDate.Tables(2)
                ddlYear.DataTextField = "Years"
                ddlYear.DataValueField = "Years"
                ddlYear.DataBind()
                ddlYear.Items.Insert(0, New ListItem("yyyy", ""))
                ddlYear.Items.Insert(1, New ListItem("UKUK", "0000"))

                If Not dtValues = "" Then
                    ddlDate.Items.FindByText(Split(dtValues.ToString, "-")(0).ToString.ToUpper).Selected = True
                    ddlMonth.Items.FindByText(Split(dtValues.ToString, "-")(1).ToString.ToUpper).Selected = True
                    ddlYear.Items.FindByText(Split(dtValues.ToString, "-")(2).ToString.ToUpper).Selected = True
                End If

                ddlDate.Attributes.Add("onchange", "CheckStandardDateAttr(this,'" & ddlDate.ClientID & "','" & ddlMonth.ClientID & "','" & ddlYear.ClientID & "');")
                ddlMonth.Attributes.Add("onchange", "CheckStandardDateAttr(this,'" & ddlDate.ClientID & "','" & ddlMonth.ClientID & "','" & ddlYear.ClientID & "');")
                ddlYear.Attributes.Add("onchange", "CheckStandardDateAttr(this,'" & ddlDate.ClientID & "','" & ddlMonth.ClientID & "','" & ddlYear.ClientID & "');")

                PlaceMedEx.Controls.Add(ddlDate)
                PlaceMedEx.Controls.Add(ddlMonth)


                If rblScreeningDate.SelectedValue.ToString.ToString.Trim() <> "N" Then
                    If cDataStatus.ToString() = "B" Then

                        If nMedExScreeningDtlNo.ToString() <> 0 Then
                            If dtValues.ToString <> "" Or TranNo > 1 Then
                                If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                                    ddlDate.Attributes.Add("disabled", "true")
                                    ddlMonth.Attributes.Add("disabled", "true")
                                End If
                            End If
                        End If
                    ElseIf cDataStatus.ToString() = "D" Then
                        If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                            ddlDate.Attributes.Add("disabled", "true")
                            ddlMonth.Attributes.Add("disabled", "true")
                        End If
                    End If
                End If
                GetObject = ddlYear

            Case Else
                Return Nothing
        End Select
    End Function

#End Region

#Region "GetDateImage"

    Public Function GetDateImage(ByVal vMedExCode As Integer, ByVal objelement As Object) As Object
        Dim imgTo As New System.Web.UI.HtmlControls.HtmlImage()
        imgTo.ID = "img" & CStr(vMedExCode)
        Dim CalendarExtender As AjaxControlToolkit.CalendarExtender = New AjaxControlToolkit.CalendarExtender()
        CalendarExtender.TargetControlID = objelement.id
        CalendarExtender.PopupButtonID = imgTo.ID
        CalendarExtender.Format = "dd-MMM-yyyy"

        PlaceMedEx.Controls.Add(CalendarExtender)
        Return imgTo
    End Function

#End Region

#Region "Show Error Message"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        Me.lblerrormsg.Text = exMessage + "<BR> " + eStr
    End Sub

#End Region

#Region "GetRF"

    Private Function GetRF(ByVal vFieldType As String, ByVal Id As String, ByVal vMedExValues As String) As Object
        Dim RF As New RequiredFieldValidator
        RF.ID = "RF" & Id
        RF.ControlToValidate = Id
        RF.ErrorMessage = "Please Enter the Value"
        RF.SkinID = "ErrorMsg"
        GetRF = RF
    End Function

    Private Function GetREV(ByVal vFieldType As String, ByVal Id As String, ByVal vMedExValues As String, ByVal ValidationType As String) As Object
        Dim REV As New RegularExpressionValidator
        REV.ID = "REV" & Id
        REV.ControlToValidate = Id
        REV.ErrorMessage = "Please Enter the Value"
        REV.SkinID = "ErrorMsg"
        GetREV = REV
    End Function

#End Region

#Region "Save"

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim ds As New DataSet
        Dim estr As String = String.Empty
        Dim dt_top As New DataTable
        Dim Dir As DirectoryInfo
        Dim TranNo_Retu As String = String.Empty
        Dim FolderPath As String = String.Empty
        Dim objCollection As ControlCollection
        Dim objControl As Control
        Dim ObjId As String = String.Empty
        Dim filename As String = String.Empty
        Dim SubjectId As String = String.Empty
        Dim Is_Transaction As Boolean = False
        Dim param As String = String.Empty
        Dim nMedExScreenNo As Integer
        Dim StrRetValue() As String
        Dim dt_profiles As New DataTable
        Dim dv_profiles As New DataView
        Dim scrDt As String = String.Empty
        Dim wStr As String
        Dim ScreeningDate As String = String.Empty
        Dim ds_ScreeningDtl As DataSet
        Dim result_Retu As String = String.Empty
        Dim oblControlType As String = String.Empty
        Dim ControlId As String
        Dim TranNo As Integer = 0
        Dim dt_MedExWorkSpaceScreeningDtl As New DataTable
        Dim flg As Boolean = False
        Dim MedExScreeningHdrNo As String = String.Empty
        Dim dt_SubMedExMst As New DataTable
        Dim dt_MedExScreeningDtl As DataTable
        Dim IsSavedinDB As Boolean = False

        IsSaved = True

        Dim dr As DataRow
        Try

            If rblScreeningDate.SelectedValue.ToString.ToString.Trim() = "N" Then
                ScreeningDate = Nothing
            Else
                ScreeningDate = rblScreeningDate.SelectedValue.ToString.ToString.Trim()
            End If

            BtnExit.Enabled = False

            If hdnAlreadySaved.Value = "YES" Then
                'ds_ScreeningDtl = CType(Me.ViewState(VS_dsSubMedex), DataSet)  ''duplicate Line
                ds_ScreeningDtl = CType(Me.ViewState(VS_dsSubMedex), DataSet)
                ds_ScreeningDtl.Tables(0).Columns.Add("ScreeningType")
                ds_ScreeningDtl.AcceptChanges()


                dt_MedExScreeningDtl = CType(Me.ViewState(VS_dsSubMedex), DataSet).Tables(0).Clone()
                dt_SubMedExMst = CType(Me.ViewState(VSDtSubMedExMst), DataSet).Tables(0)
                ''  objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection) 'Me.PlaceMedEx.Controls ''' Comment by ketan for missing question

                '' Added By Ketan for Missing Question
                If txtproject.Text.ToString.Trim() <> "" AndAlso txtSubject.Text.ToString.Trim() <> "" AndAlso ddlScreeningDate.SelectedValue.ToString() = "N" AndAlso Request.QueryString("Group") <> "" Then
                    Dim tempProject As String = txtproject.Text.Split("]")(0).Substring(1, txtproject.Text.Split("]")(0).Length - 1)
                    Dim tempSubject As String = txtSubject.Text.Split("(")(1).Substring(0, txtSubject.Text.Split("(")(1).Length - 1)
                    Dim tempDate As String = ddlScreeningDate.SelectedValue.ToString()
                    Dim tempGroup As String = Request.QueryString("Group")
                    objCollection = CType(Me.Session("PlaceMedEx_" + tempProject + tempSubject + tempDate + tempGroup), ControlCollection)

                ElseIf txtproject.Text.ToString.Trim() <> "" AndAlso txtSubject.Text.ToString.Trim() <> "" AndAlso ddlScreeningDate.SelectedValue.ToString() <> "M" AndAlso Request.QueryString("Group") <> "" Then
                    Dim tempProject As String = txtproject.Text.Split("]")(0).Substring(1, txtproject.Text.Split("]")(0).Length - 1)
                    Dim tempSubject As String = txtSubject.Text.Split("(")(1).Substring(0, txtSubject.Text.Split("(")(1).Length - 1)
                    Dim tempDate As String = ddlScreeningDate.SelectedValue.ToString().Replace("/", "").Split(" ")(0)
                    Dim tempGroup As String = Request.QueryString("Group")
                    objCollection = CType(Me.Session("PlaceMedEx_" + tempProject + tempSubject + tempDate + tempGroup), ControlCollection)
                Else
                    objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection)
                End If


                Dim cnt As Integer = 0
                For Each objControl In objCollection
                    cnt += 1
                    If Not objControl.ID Is Nothing Then
                        If objControl.ID.ToString.Trim() = "11173" Then
                            TranNo = 0
                        End If
                    End If

                    If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.TextBox" Then
                        TranNo = 0
                        dr = ds_ScreeningDtl.Tables(0).NewRow()
                        dr("nMedExScreeningDtlNo") = cnt
                        dr("nMedExScreeningHdrNo") = hdnMedExScreenHdrno.Value
                        nMedexScreeningHdrNo = hdnMedExScreenHdrno.Value
                        If Convert.ToString(objControl.ID).Contains("txt") Then
                            ObjId = Convert.ToString(objControl.ID).Replace("txt", "")
                        Else
                            ObjId = Convert.ToString(objControl.ID).Trim()
                        End If
                        dr("vMedExCode") = ObjId 'CType(objControl, TextBox).ID
                        dr("vMedExResult") = Request.Form(objControl.ID) 'CType(objControl, TextBox).Text 'CType(Me.FindControl(obj.GetId), TextBox).Text
                        dr("iTranno") = TranNo
                        dr("iModifyBy") = L_UserId
                        dr("ScreeningType") = CType(objControl, TextBox).CssClass.ToString.Split(" ")(0)
                        If Me.ViewState(VS_ContinueMode) = 1 Then
                            dr("cDataStatus") = "B"
                        Else
                            dr("cDataStatus") = "D"
                        End If
                        If IsGunned = True Then
                            dr("cIsGunned") = "Y"
                        Else
                            dr("cIsGunned") = "N"
                        End If
                        ds_ScreeningDtl.Tables(0).Rows.Add(dr)
                        ds_ScreeningDtl.Tables(0).AcceptChanges()

                    ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" Then
                        If Not objControl.ID.Contains("ddlDiv") Then

                            TranNo = 0
                            ObjId = objControl.ID.ToString.Trim()
                            If DirectCast(objControl, System.Web.UI.WebControls.DropDownList).Attributes("StandardDate") = "Y" Then
                                ds_ScreeningDtl.Tables(0).DefaultView.RowFilter = "vMedExCode = '" + ObjId.Substring(0, ObjId.IndexOf("_")).Trim() + "'"
                            Else
                                ds_ScreeningDtl.Tables(0).DefaultView.RowFilter = "vMedExCode = '" + objControl.ID + "'"
                            End If

                            If ds_ScreeningDtl.Tables(0).DefaultView.ToTable.Rows.Count = 0 Then
                                dr = ds_ScreeningDtl.Tables(0).NewRow()
                                dr("nMedExScreeningDtlNo") = cnt
                                dr("nMedExScreeningHdrNo") = hdnMedExScreenHdrno.Value
                                nMedexScreeningHdrNo = hdnMedExScreenHdrno.Value

                                If DirectCast(objControl, System.Web.UI.WebControls.DropDownList).Attributes("StandardDate") = "Y" Then
                                    For Each objControl1 In objCollection
                                        If Not objControl1.ID Is Nothing AndAlso objControl1.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" AndAlso objControl1.ID.ToString.Trim().Substring(0, objControl1.ID.ToString.Trim().IndexOf("_")) = ObjId.Substring(0, ObjId.IndexOf("_")).Trim() Then
                                            If Request.Form(objControl1.ID) = "" Then
                                                flg = True
                                            Else
                                                dr("vMedExResult") += Request.Form(objControl1.ID)
                                            End If
                                        End If
                                    Next
                                    dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("_")).Trim()
                                    dr("vMedExResult") = IIf(flg = True, "", dr("vMedexResult").ToString)
                                Else
                                    dr("vMedExCode") = objControl.ID
                                    dr("vMedExResult") = Request.Form(objControl.ID)
                                End If

                                dr("iTranno") = TranNo
                                dr("iModifyBy") = L_UserId
                                dr("ScreeningType") = CType(objControl, DropDownList).CssClass.ToString.Split(" ")(0)
                                If Me.ViewState(VS_ContinueMode) = 1 Then
                                    dr("cDataStatus") = "B"
                                Else
                                    dr("cDataStatus") = "D"
                                End If
                                If IsGunned = True Then
                                    dr("cIsGunned") = "Y"
                                Else
                                    dr("cIsGunned") = "N"
                                End If
                                ds_ScreeningDtl.Tables(0).Rows.Add(dr)
                                ds_ScreeningDtl.Tables(0).AcceptChanges()
                            End If
                        End If
                    ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.FileUpload" Then
                        If objControl.ID.ToString.Contains("FU") Then
                            ObjId = objControl.ID.ToString.Replace("FU", "")
                        Else
                            ObjId = objControl.ID.ToString.Trim()
                        End If

                        TranNo = 0


                        If IsNothing(CType(FindControl("hlnk" + ObjId), HyperLink)) AndAlso Request.Files(objControl.ID).FileName = "" AndAlso _
                         Not IsNothing(CType(FindControl("hlnk" + ObjId), HyperLink)) Then
                            filename = CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl
                        ElseIf IsNothing(CType(FindControl("hlnk" + ObjId), HyperLink)) AndAlso Request.Files(objControl.ID).FileName <> "" Then
                            filename = "~/" + System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail") + _
                                        GeneralModule.Pro_Screening + "/" + Me.HSubjectId.Value + "/" + Path.GetFileName(Request.Files(objControl.ID).FileName.ToString.Trim())
                        ElseIf Not IsNothing(CType(FindControl("hlnk" + ObjId), HyperLink)) Then
                            filename = CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl
                        Else
                            If Request.Files(objControl.ID).FileName <> "" Then
                                filename = CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl
                            Else
                                filename = ""
                            End If
                        End If

                        dr = ds_ScreeningDtl.Tables(0).NewRow()
                        dr("nMedExScreeningDtlNo") = cnt
                        dr("nMedExScreeningHdrNo") = hdnMedExScreenHdrno.Value
                        nMedexScreeningHdrNo = hdnMedExScreenHdrno.Value
                        dr("vMedExCode") = ObjId
                        If filename.Contains("\") Then
                            dr("vMedexResult") = filename.Split("\\")(2)
                        ElseIf filename.Contains("/") Then
                            dr("vMedExResult") = filename.Split("//")(4)
                        Else
                            dr("vMedexResult") = filename
                        End If
                        dr("iTranno") = TranNo
                        dr("iModifyBy") = L_UserId
                        dr("ScreeningType") = CType(objControl, FileUpload).CssClass.ToString.Split(" ")(0)
                        If Me.ViewState(VS_ContinueMode) = 1 Then
                            dr("cDataStatus") = "B"
                        Else
                            dr("cDataStatus") = "D"
                        End If
                        If IsGunned = True Then
                            dr("cIsGunned") = "Y"
                        Else
                            dr("cIsGunned") = "N"
                        End If
                        ds_ScreeningDtl.Tables(0).Rows.Add(dr)
                        ds_ScreeningDtl.Tables(0).AcceptChanges()

                    ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.RadioButtonList" Then
                        TranNo = 0
                        dr = ds_ScreeningDtl.Tables(0).NewRow()
                        dr("nMedExScreeningDtlNo") = cnt
                        dr("nMedExScreeningHdrNo") = hdnMedExScreenHdrno.Value
                        nMedexScreeningHdrNo = hdnMedExScreenHdrno.Value
                        dr("vMedExCode") = objControl.ID 'CType(objControl, DropDownList).ID
                        Dim rbl As RadioButtonList = CType(FindControl(objControl.ID), RadioButtonList)
                        Dim StrChk As String = String.Empty

                        For index = 0 To rbl.Items.Count - 1
                            StrChk += IIf(rbl.Items(index).Selected, rbl.Items(index).Value.ToString.Trim() + ",", "")
                        Next index

                        If StrChk.Trim() <> "" Then
                            StrChk = StrChk.Substring(0, StrChk.LastIndexOf(","))
                        End If

                        dr("vMedExResult") = StrChk
                        dr("iTranno") = TranNo
                        dr("iModifyBy") = L_UserId
                        dr("ScreeningType") = CType(objControl, RadioButtonList).CssClass.ToString.Split(" ")(0)
                        If Me.ViewState(VS_ContinueMode) = 1 Then
                            dr("cDataStatus") = "B"
                        Else
                            dr("cDataStatus") = "D"
                        End If
                        If IsGunned = True Then
                            dr("cIsGunned") = "Y"
                        Else
                            dr("cIsGunned") = "N"
                        End If
                        ds_ScreeningDtl.Tables(0).Rows.Add(dr)
                        ds_ScreeningDtl.Tables(0).AcceptChanges()


                    ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.CheckBoxList" Then
                        TranNo = 0
                        dr = ds_ScreeningDtl.Tables(0).NewRow()
                        dr("nMedExScreeningDtlNo") = cnt
                        dr("nMedExScreeningHdrNo") = hdnMedExScreenHdrno.Value
                        nMedexScreeningHdrNo = hdnMedExScreenHdrno.Value
                        dr("vMedExCode") = objControl.ID 'CType(objControl, DropDownList).ID
                        Dim chkl As CheckBoxList = CType(FindControl(objControl.ID), CheckBoxList)
                        Dim StrChk As String = String.Empty

                        For index = 0 To chkl.Items.Count - 1
                            StrChk += IIf(chkl.Items(index).Selected, chkl.Items(index).Value.ToString.Trim() + ",", "")
                        Next index

                        If StrChk.Trim() <> "" Then
                            StrChk = StrChk.Substring(0, StrChk.LastIndexOf(","))
                        End If
                        dr("vMedExResult") = StrChk 'Request.Form(objControl.ID) 
                        dr("iTranno") = TranNo
                        dr("iModifyBy") = L_UserId
                        dr("ScreeningType") = CType(objControl, CheckBoxList).CssClass.ToString.Split(" ")(0)
                        If Me.ViewState(VS_ContinueMode) = 1 Then
                            dr("cDataStatus") = "B"
                        Else
                            dr("cDataStatus") = "D"
                        End If
                        If IsGunned = True Then
                            dr("cIsGunned") = "Y"
                        Else
                            dr("cIsGunned") = "N"
                        End If
                        ds_ScreeningDtl.Tables(0).Rows.Add(dr)
                        ds_ScreeningDtl.Tables(0).AcceptChanges()

                    ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.Label" Then
                        If objControl.ID.ToString.Contains("lbl") Then
                            TranNo = 0
                            dr = ds_ScreeningDtl.Tables(0).NewRow()
                            dr("nMedExScreeningDtlNo") = cnt
                            dr("nMedExScreeningHdrNo") = hdnMedExScreenHdrno.Value
                            nMedexScreeningHdrNo = hdnMedExScreenHdrno.Value

                            ObjId = objControl.ID.ToString.Replace("lbl", "")
                            dr("vMedExCode") = ObjId
                            dr("vMedExResult") = CType(objControl, Label).Text
                            dr("iTranno") = TranNo
                            dr("iModifyBy") = L_UserId
                            dr("ScreeningType") = CType(objControl, Label).CssClass.ToString.Split(" ")(0)
                            If Me.ViewState(VS_ContinueMode) = 1 Then
                                dr("cDataStatus") = "B"
                            Else
                                dr("cDataStatus") = "D"
                            End If

                            If IsGunned = True Then
                                dr("cIsGunned") = "Y"
                            Else
                                dr("cIsGunned") = "N"
                            End If
                            ds_ScreeningDtl.Tables(0).Rows.Add(dr)
                            ds_ScreeningDtl.Tables(0).AcceptChanges()
                        End If
                    End If

                Next objControl
                dt_SubMedExMst.TableName = "MedExScreeningHdr"
                ds.Tables.Add(dt_SubMedExMst.Copy)
                ds_ScreeningDtl.Tables(0).DefaultView.RowFilter = "ScreeningType='D'"
                dt_MedExScreeningDtl = ds_ScreeningDtl.Tables(0).DefaultView.ToTable
                If dt_MedExScreeningDtl.Rows.Count > 0 Then
                    dt_MedExScreeningDtl.Columns.Remove("ScreeningType")
                    dt_MedExScreeningDtl.TableName = "MedExScreeningDtl"
                    dt_MedExScreeningDtl.AcceptChanges()
                    ds.Tables.Add(dt_MedExScreeningDtl.Copy)
                End If
                dt_MedExWorkSpaceScreeningDtl = ds_ScreeningDtl.Tables(0).DefaultView.ToTable

                If dt_MedExWorkSpaceScreeningDtl.Rows.Count > 0 Then

                    dt_MedExWorkSpaceScreeningDtl.Columns.Remove("ScreeningType")
                    dt_MedExWorkSpaceScreeningDtl.Columns("nMedExScreeningHdrNo").ColumnName = "nMedExWorkSpaceScreeningHdrNo"
                    dt_MedExWorkSpaceScreeningDtl.Columns("nMedExScreeningDtlNo").ColumnName = "nMedExWorkSpaceScreeningDtlNo"
                    dt_MedExWorkSpaceScreeningDtl.TableName = "MedExWorkSpaceScreeningDtl"
                    IsSaved = True
                    hdnIsSave.Value = True
                    IsSavedinDB = True
                    If Not objLambda.Save_MedExScreeningDtlOnly(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_ScreeningDtl, estr) Then
                        Me.ShowErrorMessage(estr, "....Error while saving in MedExScreeningDtl")
                        Exit Sub
                    End If
                End If
                'Exit Sub
            End If
            If Not Me.ViewState(VS_ContinueMode) = 1 Then
                'If Not Auntheticate() Then
                '    Me.btnSaveuthenticate.Style.Add("display", "")
                '    Me.trEle.Style.Add("display", "")
                '    For Each li As ListItem In rblSaveEle.Items
                '        li.Selected = False
                '    Next
                '    dt_profiles = CType(Me.Session(S_Profiles), DataTable)
                '    dv_profiles = dt_profiles.DefaultView
                '    dv_profiles.RowFilter = "vUserTypeCode = '" + L_UserType + "'"
                '    Me.btnOk.OnClientClick = "return ValidateReview('" & L_FirstName + " " + L_LastName & "','" & dv_profiles.ToTable.Rows(0)("vUserTypeName").ToString() & "','1');"
                '    Me.lblSignername.Text = L_FirstName + L_LastName
                '    Me.lblSignerDesignation.Text = dv_profiles.ToTable.Rows(0)("vUserTypeName").ToString()
                '    Me.MPEAunthticate.Show()
                '    Exit Sub
                'End If
            End If

            'If txtproject.Text = "" Then
            If Not AssignValues() Then
                Me.ObjCommon.ShowAlert("Error While Assigning Data.", Me.Page)
                Exit Sub
            End If
            'End If

            ds = Me.ViewState(VS_dsSubMedex)



            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then


                dComparedTodayDateTime = CDate(ObjCommon.GetCurDatetimeWithOffSet(L_TimeZoneName).DateTime)
                Dim drScreeningTime As DataRow
                If CType(ViewState("dtScreeningTime"), DataTable) Is Nothing Then
                    drScreeningTime = dtScreeningTime.NewRow()
                Else
                    drScreeningTime = CType(ViewState("dtScreeningTime"), DataTable).Rows(0)
                End If
                'drScreeningTime("nScreeningId") = 1
                drScreeningTime("nMedExScreeningHdrNo") = nMedexScreeningHdrNo
                drScreeningTime("vWorkspaceId") = vWorkspaceId
                drScreeningTime("vSubjectId") = vSubjectID
                drScreeningTime("dComparedScreeningDateTime") = CType(dComparedScreeningDateTime, Date).ToString("dd-MMM-yyyy hh:mm:ss").Trim()
                drScreeningTime("dComparedTodayDateTime") = CType(dComparedTodayDateTime, Date).ToString("dd-MMM-yyyy hh:mm:ss").Trim()
                drScreeningTime("iModifyBy") = L_UserId
                If CType(ViewState("dtScreeningTime"), DataTable) Is Nothing Then
                    dtScreeningTime.Rows.Add(drScreeningTime)
                Else
                    dtScreeningTime = CType(ViewState("dtScreeningTime"), DataTable)
                End If
                dtScreeningTime.AcceptChanges()
                dsScreeningTime = New DataSet("dsScreeningTime")
                dsScreeningTime.Tables.Add(dtScreeningTime)
                If Not Me.objLambda.Save_ScreeningTimeMst(Me.ViewState(VS_Choice), dsScreeningTime, L_UserId, TranNo_Retu, Is_Transaction, estr) Then
                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    'Me.btnSubject_Click(sender, e)
                    Exit Sub
                End If
            End If
            IsSaved = True
            hdnIsSave.Value = True
            'If txtproject.Text <> "" Then
            '    ds.Tables(1).DefaultView.ToTable(True, "nMedExWorkSpaceScreeningHdrNo", "iTranno", "vMedExCode", "vMedExResult", "iModifyBy", "dModifyOn", "cStatusIndi", "vRemarks", "cDataStatus", "cIsGunned")
            '    ds.AcceptChanges()
            'End If
            If IsSavedinDB = False Then
                objLambda.Timeout = -1
                If Not Me.objLambda.Save_MedExScreeningTempDtl(Me.ViewState(VS_Choice), ds, L_UserId, TranNo_Retu, Is_Transaction, estr) Then
                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    'Me.btnSubject_Click(sender, e)
                    Exit Sub
                End If

            End If
            If Not Me.ViewState(VS_ContinueMode) = 1 Then
                'If Not ValidateWorkflow("Save") Then
                '    ObjCommon.ShowAlert("Error while validating", Me.Page)
                '    Exit Sub
                'End If
            End If

            ''objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection)

            '' Added By Ketan for Missing Question
            If txtproject.Text.ToString.Trim() <> "" AndAlso txtSubject.Text.ToString.Trim() <> "" AndAlso ddlScreeningDate.SelectedValue.ToString() = "N" AndAlso Request.QueryString("Group") <> "" Then
                Dim tempProject As String = txtproject.Text.Split("]")(0).Substring(1, txtproject.Text.Split("]")(0).Length - 1)
                Dim tempSubject As String = txtSubject.Text.Split("(")(1).Substring(0, txtSubject.Text.Split("(")(1).Length - 1)
                Dim tempDate As String = ddlScreeningDate.SelectedValue.ToString()
                Dim tempGroup As String = Request.QueryString("Group")
                objCollection = CType(Me.Session("PlaceMedEx_" + tempProject + tempSubject + tempDate + tempGroup), ControlCollection)

            ElseIf txtproject.Text.ToString.Trim() <> "" AndAlso txtSubject.Text.ToString.Trim() <> "" AndAlso ddlScreeningDate.SelectedValue.ToString() <> "M" AndAlso Request.QueryString("Group") <> "" Then
                Dim tempProject As String = txtproject.Text.Split("]")(0).Substring(1, txtproject.Text.Split("]")(0).Length - 1)
                Dim tempSubject As String = txtSubject.Text.Split("(")(1).Substring(0, txtSubject.Text.Split("(")(1).Length - 1)
                Dim tempDate As String = ddlScreeningDate.SelectedValue.ToString().Replace("/", "").Split(" ")(0)
                Dim tempGroup As String = Request.QueryString("Group")
                objCollection = CType(Me.Session("PlaceMedEx_" + tempProject + tempSubject + tempDate + tempGroup), ControlCollection)
            Else
                objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection)
            End If


            For Each objControl In objCollection
                filename = String.Empty
                If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.FileUpload" Then

                    If objControl.ID.ToString.Contains("FU") Then
                        ObjId = objControl.ID.ToString.Replace("FU", "")
                    Else
                        ObjId = objControl.ID.ToString.Trim()
                    End If
                    If IsNothing(CType(FindControl("hlnk" + ObjId), HyperLink)) AndAlso Request.Files(objControl.ID).FileName <> "" Then

                        If Me.txtproject.Text <> "" Then
                            FolderPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail"))
                            FolderPath += Me.HFScreeningWorkSpaceID.Value + "/" + Me.HSubjectId.Value + "/" + TranNo_Retu + "/"
                        Else
                            FolderPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail"))
                            FolderPath += GeneralModule.Pro_Screening + "/" + Me.HSubjectId.Value + "/" + TranNo_Retu + "/"
                        End If
                        Dir = New DirectoryInfo(FolderPath)
                        If Not Dir.Exists() Then
                            Dir.Create()
                        End If
                        filename = FolderPath + Path.GetFileName(Request.Files(objControl.ID).FileName.ToString.Trim())
                        Request.Files(objControl.ID).SaveAs(filename)

                    ElseIf Not IsNothing(CType(FindControl("hlnk" + ObjId), HyperLink)) AndAlso CType(FindControl(objControl.ID), FileUpload).HasFile Then
                        If Me.txtproject.Text <> "" Then
                            FolderPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail"))
                            FolderPath += GeneralModule.Pro_Screening + "/" + Me.HFScreeningWorkSpaceID.Value + "/" + Me.HSubjectId.Value + "/" + TranNo_Retu + "/"
                        Else
                            FolderPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail"))
                            FolderPath += GeneralModule.Pro_Screening + "/" + Me.HSubjectId.Value + "/" + TranNo_Retu + "/"
                        End If
                        Dir = New DirectoryInfo(FolderPath)
                        If Not Dir.Exists() Then
                            Dir.Create()
                        End If
                        filename = FolderPath + Path.GetFileName(Request.Files(objControl.ID).FileName.ToString.Trim())
                        Request.Files(objControl.ID).SaveAs(filename)
                    End If
                End If

            Next objControl

            BtnExit.Enabled = True

            Me.txtSubject.Text = ""
            Me.chkScreeningType.ClearSelection()


            nMedExScreenNo = 1
            If Not rblScreeningDate.SelectedValue.ToString.ToString.Trim() = "N" Then
                StrRetValue = TranNo_Retu.Split("/")
                nMedExScreenNo = StrRetValue(0)
            End If
            If Not rblScreeningDate.SelectedValue.ToString.ToString.Trim() <> "N" Then
                If ds.Tables.Count = 4 Then             'For First Time Click On "Save & Continue" OR "Submit"
                    Me.HProjectId.Value = ds.Tables(1).Rows(0)("vWorkspaceId")
                    Me.HSubjectId.Value = ds.Tables(1).Rows(0)("vSubjectId")
                    Me.hMedExNo.Value = ds.Tables(1).Rows(0)("nMedExScreeningHdrNo")
                    scrDt = ds.Tables(1).Rows(0)("dScreenDate").ToString.Trim
                ElseIf ds.Tables.Count = 3 Then
                    Me.HProjectId.Value = ds.Tables(0).Rows(0)("vWorkspaceId")
                    Me.HSubjectId.Value = ds.Tables(0).Rows(0)("vSubjectId")
                    Me.hMedExNo.Value = ds.Tables(0).Rows(0)("nMedExScreeningHdrNo")
                    scrDt = ds.Tables(0).Rows(0)("dScreenDate").ToString.Trim
                End If
            End If
            Me.rblScreeningDate.Items.Clear()
            Me.chkScreeningType.ClearSelection()
            Me.ViewState(VS_SaveMode) = 1
            GenCall()
            If Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso _
                Me.Request.QueryString("SubjectId").ToString.Trim() <> "" Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "", "closewindow()", True)
                Exit Sub
            End If
            If Not fillScreeningDates() Then
                Throw New Exception("Error While Fill Screening Dates")
            End If

            If Not Me.ViewState(VS_ContinueMode) = 1 Then
                Me.ViewState(VS_ContinueMode) = Nothing
                If ScreeningDate Is Nothing Then
                    For i As Integer = 0 To rblScreeningDate.Items.Count - 1
                        If (rblScreeningDate.Items(i).Value.Contains(Convert.ToString(rblScreeningDate.Items(1).Value))) Then
                            rblScreeningDate.SelectedIndex = i
                        End If
                    Next

                Else
                    For i As Integer = 0 To rblScreeningDate.Items.Count - 1
                        If (rblScreeningDate.Items(i).Value.Contains(ScreeningDate)) Then
                            rblScreeningDate.SelectedIndex = i
                        End If
                    Next

                End If

                If Not ValidateUserWiseScreeningEntry("CONTINUE", Me.HSubjectId.Value, rblScreeningDate.SelectedValue) Then
                    'ObjCommon.ShowAlert("Another User was working on this screening Date of this subjectid", Me.Page)
                    For Each li As ListItem In rblScreeningDate.Items
                        li.Selected = False
                    Next
                    Session("ScreeningGroup") = ""
                    GroupValidation = True
                    Dim url As String = Request.Url.ToString()
                    url = url.Substring(0, url.IndexOf("Group"))
                    Response.Redirect(url, False)
                    Exit Sub
                End If

                rblScreeningDate_SelectedIndexChanged(sender, e)
                If IsSaved = True Then
                    'IsSaved = False
                    'Me.ObjCommon.ShowAlert("Record Saved Sucecssfully", Me.Page)
                End If
            Else
                If ScreeningDate Is Nothing Then
                    For i As Integer = 0 To rblScreeningDate.Items.Count - 1
                        If (rblScreeningDate.Items(i).Value.Contains(Convert.ToString(rblScreeningDate.Items(1).Value))) Then
                            rblScreeningDate.SelectedIndex = i
                        End If
                    Next

                Else
                    For i As Integer = 0 To rblScreeningDate.Items.Count - 1
                        If (rblScreeningDate.Items(i).Value.Contains(ScreeningDate)) Then
                            rblScreeningDate.SelectedIndex = i
                        End If
                    Next
                End If
                rblScreeningDate_SelectedIndexChanged(sender, e)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "BtnSave_Click")
        End Try

    End Sub

    Private Function AssignValues() As Boolean
        Dim index As Integer = 0
        Dim StrValue As String = String.Empty
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim str_retn As String = String.Empty
        Dim objCollection As ControlCollection
        Dim objControl As Control
        Dim dr As DataRow
        Dim dt_Save As New DataTable
        Dim dt_SubMedExMst As New DataTable
        Dim ds_SubMedExMst As New DataSet
        Dim ds_SubjectScreeningTemplate As New DataSet
        Dim TranNo As Integer = 0
        Dim estr As String = String.Empty
        Dim ObjId As String = String.Empty
        Dim cnt As Integer
        Dim ds_MedExScreeningHdrHidtory As New DataSet
        Dim wStr As String = String.Empty
        Dim dt_MedExWorkSpaceScreeningHdr As New DataTable
        Dim dt_MedExWorkSpaceScreeningDtl As New DataTable
        Dim dt_MedExScreeningDtl As New DataTable
        Dim ds_EditMedExWorkSpaceScrDtl As New DataSet
        Dim ds_Temp As New DataSet
        Dim MedExScreeningHdrNo As String = String.Empty
        Dim flg As Boolean = False

        Try
            dt_Save = CType(Me.ViewState(VS_dsSubMedex), DataSet).Tables(0).Clone()
            dt_MedExScreeningDtl = CType(Me.ViewState(VS_dsSubMedex), DataSet).Tables(0).Clone()
            dt_SubMedExMst = CType(Me.ViewState(VSDtSubMedExMst), DataSet).Tables(0)
            ''  objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection) 'Me.PlaceMedEx.Controls 

            '' Added By Ketan for Missing Question
            If txtproject.Text.ToString.Trim() <> "" AndAlso txtSubject.Text.ToString.Trim() <> "" AndAlso ddlScreeningDate.SelectedValue.ToString() = "N" AndAlso Request.QueryString("Group") <> "" Then
                Dim tempProject As String = txtproject.Text.Split("]")(0).Substring(1, txtproject.Text.Split("]")(0).Length - 1)
                Dim tempSubject As String = txtSubject.Text.Split("(")(1).Substring(0, txtSubject.Text.Split("(")(1).Length - 1)
                Dim tempDate As String = ddlScreeningDate.SelectedValue.ToString()
                Dim tempGroup As String = Request.QueryString("Group")
                objCollection = CType(Me.Session("PlaceMedEx_" + tempProject + tempSubject + tempDate + tempGroup), ControlCollection)

            ElseIf txtproject.Text.ToString.Trim() <> "" AndAlso txtSubject.Text.ToString.Trim() <> "" AndAlso ddlScreeningDate.SelectedValue.ToString() <> "M" AndAlso Request.QueryString("Group") <> "" Then
                Dim tempProject As String = txtproject.Text.Split("]")(0).Substring(1, txtproject.Text.Split("]")(0).Length - 1)
                Dim tempSubject As String = txtSubject.Text.Split("(")(1).Substring(0, txtSubject.Text.Split("(")(1).Length - 1)
                Dim tempDate As String = ddlScreeningDate.SelectedValue.ToString().Replace("/", "").Split(" ")(0)
                Dim tempGroup As String = Request.QueryString("Group")
                objCollection = CType(Me.Session("PlaceMedEx_" + tempProject + tempSubject + tempDate + tempGroup), ControlCollection)
            Else
                objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection)
            End If


            If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                If Not objHelp.GetMedExScreeningHdr("nMedExScreeningHdrNo=" & CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows(0)("nMedExScreeningHdrNo"), _
                                                                  WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                          ds_SubMedExMst, estr) Then
                    Return False
                End If
                dt_SubMedExMst = ds_SubMedExMst.Tables(0)
                If Me.ViewState(VS_ContinueMode) <> 1 Then
                    For Each drd As DataRow In dt_SubMedExMst.Rows
                        drd("cDataStatus") = "D"
                    Next
                    dt_SubMedExMst.AcceptChanges()
                End If
            Else


                dr = dt_SubMedExMst.NewRow()
                dr("vSubjectId") = Me.HSubjectId.Value
                dr("vWorkspaceId") = GeneralModule.Pro_Screening
                dr("nMedExScreeningHdrNo") = 1
                dr("cIsEligible") = System.DBNull.Value
                If dt_SubMedExMst.Rows.Count > 0 Then
                    dr("nMedExScreeningHdrNo") = dt_SubMedExMst.Rows(0)("nMedExScreeningHdrNo")
                End If
                If Session("dScreenDate") Is Nothing Then
                    dr("dScreenDate") = CDate(ObjCommon.GetCurDatetimeWithOffSet(L_TimeZoneName).DateTime)
                    dr("cIsTranscribe") = "N"
                    dr("vTranscribeRemarks") = ""
                ElseIf Not Session("dScreenDate") Is Nothing AndAlso Session("dScreenDate").ToString() = "" Then
                    dr("dScreenDate") = CDate(ObjCommon.GetCurDatetimeWithOffSet(L_TimeZoneName).DateTime)
                    dr("cIsTranscribe") = "N"
                    dr("vTranscribeRemarks") = ""
                Else
                    If Not Session("isTransCribe") Is Nothing AndAlso Session("isTransCribe").ToString() = "N" Then
                        dr("dScreenDate") = CDate(ObjCommon.GetCurDatetimeWithOffSet(L_TimeZoneName).DateTime)
                        dr("cIsTranscribe") = "N"
                        dr("vTranscribeRemarks") = Convert.ToString(Session("TranscribeRemarks").ToString())
                    Else
                        dr("dScreenDate") = DateTime.Parse(Session("dScreenDate").ToString()).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)
                        dr("cIsTranscribe") = "Y"
                        dr("vTranscribeRemarks") = Convert.ToString(Session("TranscribeRemarks").ToString())
                    End If
                End If
                Session("isTransCribe") = ""
                dComparedScreeningDateTime = CDate(ObjCommon.GetCurDatetimeWithOffSet(L_TimeZoneName).DateTime)
                vSubjectID = Me.HSubjectId.Value
                vWorkspaceId = GeneralModule.Pro_Screening
                nMedexScreeningHdrNo = 1

                dr("iModifyBy") = L_UserId
                If Me.ViewState(VS_ContinueMode) = 1 Then
                    dr("cDataStatus") = "B"
                Else
                    dr("cDataStatus") = "D"
                End If
                dt_SubMedExMst.Rows.Add(dr)
                dt_SubMedExMst.AcceptChanges()
                If Not objHelp.GetSubjectScreeningTemplate("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                                           ds_SubjectScreeningTemplate, estr) Then
                    Return False
                End If
                dr = ds_SubjectScreeningTemplate.Tables(0).NewRow()
                dr("nScreeningTemplateHdrNo") = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows(0)("nScreeningTemplateHdrNo")
                dr("iModifyBy") = L_UserId
                ds_SubjectScreeningTemplate.Tables(0).Rows.Add(dr)
                ds_SubjectScreeningTemplate.Tables(0).AcceptChanges()
                ds.Tables.Add(ds_SubjectScreeningTemplate.Tables(0).Copy)
            End If
            cnt = 0
            If Not dt_Save.Columns.Contains("ScreeningType") Then
                dt_Save.Columns.Add("ScreeningType")
                dt_Save.AcceptChanges()
            End If
            '-------------------
            For Each objControl In objCollection
                cnt += 1
                If Not objControl.ID Is Nothing Then
                    If objControl.ID.ToString.Trim() = "11173" Then
                        TranNo = 0
                    End If
                End If

                If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.TextBox" Then
                    TranNo = 0
                    dr = dt_Save.NewRow()
                    dr("nMedExScreeningDtlNo") = cnt
                    dr("nMedExScreeningHdrNo") = dt_SubMedExMst.Rows(0)("nMedExScreeningHdrNo")
                    'dr("nMedExWorkSpaceScreeningHdrNo") = nMedExWorkSpaceScreeningHdrNo
                    nMedexScreeningHdrNo = dt_SubMedExMst.Rows(0)("nMedExScreeningHdrNo")
                    If objControl.ID.ToString.Contains("txt") Then
                        ObjId = objControl.ID.ToString.Replace("txt", "")
                    Else
                        ObjId = objControl.ID.ToString.Trim()
                    End If
                    dr("vMedExCode") = ObjId
                    dr("vMedExResult") = CType(objControl, TextBox).Text  'CType(objControl, TextBox).Text 'CType(Me.FindControl(obj.GetId), TextBox).Text
                    dr("iTranno") = TranNo
                    dr("iModifyBy") = L_UserId
                    dr("ScreeningType") = CType(objControl, TextBox).CssClass.ToString.Split(" ")(0)
                    If Me.ViewState(VS_ContinueMode) = 1 Then
                        dr("cDataStatus") = "B"
                    Else
                        dr("cDataStatus") = "D"
                    End If

                    If IsGunned = True Then
                        dr("cIsGunned") = "Y"
                    Else
                        dr("cIsGunned") = "N"
                    End If


                    dt_Save.Rows.Add(dr)
                    dt_Save.AcceptChanges()

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" Then
                    TranNo = 0
                    ObjId = objControl.ID.ToString.Trim()
                    If DirectCast(objControl, System.Web.UI.WebControls.DropDownList).Attributes("StandardDate") = "Y" Then
                        dt_Save.DefaultView.RowFilter = "vMedExCode = '" + ObjId.Substring(0, ObjId.IndexOf("_")).Trim() + "'"
                    Else
                        dt_Save.DefaultView.RowFilter = "vMedExCode = '" + objControl.ID + "'"
                    End If

                    If dt_Save.DefaultView.ToTable.Rows.Count = 0 Then
                        dr = dt_Save.NewRow()
                        dr("nMedExScreeningDtlNo") = cnt
                        dr("nMedExScreeningHdrNo") = dt_SubMedExMst.Rows(0)("nMedExScreeningHdrNo")
                        nMedexScreeningHdrNo = dt_SubMedExMst.Rows(0)("nMedExScreeningHdrNo")

                        If DirectCast(objControl, System.Web.UI.WebControls.DropDownList).Attributes("StandardDate") = "Y" Then
                            For Each objControl1 In objCollection
                                If Not objControl1.ID Is Nothing AndAlso objControl1.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" AndAlso objControl1.ID.ToString.Trim().Substring(0, objControl1.ID.ToString.Trim().IndexOf("_")) = ObjId.Substring(0, ObjId.IndexOf("_")).Trim() Then
                                    If Request.Form(objControl1.ID) = "" Then
                                        flg = True
                                    Else
                                        dr("vMedExResult") += Request.Form(objControl1.ID)
                                    End If
                                End If
                            Next
                            dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("_")).Trim()
                            dr("vMedExResult") = IIf(flg = True, "", dr("vMedexResult").ToString)
                        Else
                            dr("vMedExCode") = objControl.ID
                            dr("vMedExResult") = Request.Form(objControl.ID)
                        End If

                        dr("iTranno") = TranNo
                        dr("iModifyBy") = L_UserId
                        dr("ScreeningType") = CType(objControl, DropDownList).CssClass.ToString.Split(" ")(0)
                        If Me.ViewState(VS_ContinueMode) = 1 Then
                            dr("cDataStatus") = "B"
                        Else
                            dr("cDataStatus") = "D"
                        End If
                        If IsGunned = True Then
                            dr("cIsGunned") = "Y"
                        Else
                            dr("cIsGunned") = "N"
                        End If


                        dt_Save.Rows.Add(dr)
                        dt_Save.AcceptChanges()
                    End If
                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.FileUpload" Then
                    Dim filename As String = String.Empty
                    If objControl.ID.ToString.Contains("FU") Then
                        ObjId = objControl.ID.ToString.Replace("FU", "")
                    Else
                        ObjId = objControl.ID.ToString.Trim()
                    End If

                    TranNo = 0


                    If IsNothing(CType(FindControl("hlnk" + ObjId), HyperLink)) AndAlso Request.Files(objControl.ID).FileName = "" AndAlso _
                     Not IsNothing(CType(FindControl("hlnk" + ObjId), HyperLink)) Then
                        filename = CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl
                    ElseIf IsNothing(CType(FindControl("hlnk" + ObjId), HyperLink)) AndAlso Request.Files(objControl.ID).FileName <> "" Then
                        filename = "~/" + System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail") + _
                                    GeneralModule.Pro_Screening + "/" + Me.HSubjectId.Value + "/" + Path.GetFileName(Request.Files(objControl.ID).FileName.ToString.Trim())
                    ElseIf Not IsNothing(CType(FindControl("hlnk" + ObjId), HyperLink)) Then
                        filename = CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl
                    Else
                        If Request.Files(objControl.ID).FileName <> "" Then
                            filename = CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl
                        Else
                            filename = ""
                        End If
                    End If

                    dr = dt_Save.NewRow()
                    dr("nMedExScreeningDtlNo") = cnt
                    dr("nMedExScreeningHdrNo") = dt_SubMedExMst.Rows(0)("nMedExScreeningHdrNo")
                    nMedexScreeningHdrNo = dt_SubMedExMst.Rows(0)("nMedExScreeningHdrNo")
                    dr("vMedExCode") = ObjId
                    If filename.Contains("\") Then
                        dr("vMedexResult") = filename.Split("\\")(2)
                    ElseIf filename.Contains("/") Then
                        dr("vMedExResult") = filename.Split("//")(4)
                    Else
                        dr("vMedexResult") = filename
                    End If
                    dr("iTranno") = TranNo
                    dr("iModifyBy") = L_UserId
                    dr("ScreeningType") = CType(objControl, FileUpload).CssClass.ToString.Split(" ")(0)
                    If Me.ViewState(VS_ContinueMode) = 1 Then
                        dr("cDataStatus") = "B"
                    Else
                        dr("cDataStatus") = "D"
                    End If

                    If IsGunned = True Then
                        dr("cIsGunned") = "Y"
                    Else
                        dr("cIsGunned") = "N"
                    End If


                    dt_Save.Rows.Add(dr)
                    dt_Save.AcceptChanges()

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.RadioButtonList" Then
                    TranNo = 0
                    dr = dt_Save.NewRow()
                    dr("nMedExScreeningDtlNo") = cnt
                    dr("nMedExScreeningHdrNo") = dt_SubMedExMst.Rows(0)("nMedExScreeningHdrNo")
                    nMedexScreeningHdrNo = dt_SubMedExMst.Rows(0)("nMedExScreeningHdrNo")
                    dr("vMedExCode") = objControl.ID 'CType(objControl, DropDownList).ID
                    Dim rbl As RadioButtonList = CType(FindControl(objControl.ID), RadioButtonList)
                    Dim StrChk As String = String.Empty

                    For index = 0 To rbl.Items.Count - 1
                        StrChk += IIf(rbl.Items(index).Selected, rbl.Items(index).Value.ToString.Trim() + ",", "")
                    Next index

                    If StrChk.Trim() <> "" Then
                        StrChk = StrChk.Substring(0, StrChk.LastIndexOf(","))
                    End If

                    dr("vMedExResult") = StrChk
                    dr("iTranno") = TranNo
                    dr("iModifyBy") = L_UserId
                    dr("ScreeningType") = CType(objControl, RadioButtonList).CssClass.ToString.Split(" ")(0)
                    If Me.ViewState(VS_ContinueMode) = 1 Then
                        dr("cDataStatus") = "B"
                    Else
                        dr("cDataStatus") = "D"
                    End If
                    If IsGunned = True Then
                        dr("cIsGunned") = "Y"
                    Else
                        dr("cIsGunned") = "N"
                    End If

                    dt_Save.Rows.Add(dr)
                    dt_Save.AcceptChanges()


                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.CheckBoxList" Then
                    TranNo = 0
                    dr = dt_Save.NewRow()
                    dr("nMedExScreeningDtlNo") = cnt
                    dr("nMedExScreeningHdrNo") = dt_SubMedExMst.Rows(0)("nMedExScreeningHdrNo")
                    nMedexScreeningHdrNo = dt_SubMedExMst.Rows(0)("nMedExScreeningHdrNo")
                    dr("vMedExCode") = objControl.ID 'CType(objControl, DropDownList).ID
                    Dim chkl As CheckBoxList = CType(FindControl(objControl.ID), CheckBoxList)
                    Dim StrChk As String = String.Empty

                    For index = 0 To chkl.Items.Count - 1
                        StrChk += IIf(chkl.Items(index).Selected, chkl.Items(index).Value.ToString.Trim() + ",", "")
                    Next index

                    If StrChk.Trim() <> "" Then
                        StrChk = StrChk.Substring(0, StrChk.LastIndexOf(","))
                    End If
                    dr("vMedExResult") = StrChk 'Request.Form(objControl.ID) 
                    dr("iTranno") = TranNo
                    dr("iModifyBy") = L_UserId
                    dr("ScreeningType") = CType(objControl, CheckBoxList).CssClass.ToString.Split(" ")(0)
                    If Me.ViewState(VS_ContinueMode) = 1 Then
                        dr("cDataStatus") = "B"
                    Else
                        dr("cDataStatus") = "D"
                    End If

                    If IsGunned = True Then
                        dr("cIsGunned") = "Y"
                    Else
                        dr("cIsGunned") = "N"
                    End If

                    dt_Save.Rows.Add(dr)
                    dt_Save.AcceptChanges()

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.Label" Then
                    If objControl.ID.ToString.Contains("lbl") Then
                        TranNo = 0
                        dr = dt_Save.NewRow()
                        dr("nMedExScreeningDtlNo") = cnt
                        dr("nMedExScreeningHdrNo") = dt_SubMedExMst.Rows(0)("nMedExScreeningHdrNo")
                        nMedexScreeningHdrNo = dt_SubMedExMst.Rows(0)("nMedExScreeningHdrNo")

                        ObjId = objControl.ID.ToString.Replace("lbl", "")
                        dr("vMedExCode") = ObjId
                        dr("vMedExResult") = CType(objControl, Label).Text
                        dr("iTranno") = TranNo
                        dr("iModifyBy") = L_UserId
                        dr("ScreeningType") = CType(objControl, Label).CssClass.ToString.Split(" ")(0)
                        If Me.ViewState(VS_ContinueMode) = 1 Then
                            dr("cDataStatus") = "B"
                        Else
                            dr("cDataStatus") = "D"
                        End If

                        If IsGunned = True Then
                            dr("cIsGunned") = "Y"
                        Else
                            dr("cIsGunned") = "N"
                        End If


                        dt_Save.Rows.Add(dr)
                        dt_Save.AcceptChanges()
                    End If
                End If

            Next objControl
            dt_SubMedExMst.TableName = "MedExScreeningHdr"
            ds.Tables.Add(dt_SubMedExMst.Copy)
            dt_Save.DefaultView.RowFilter = "ScreeningType='D'"
            dt_MedExScreeningDtl = dt_Save.DefaultView.ToTable
            If dt_MedExScreeningDtl.Rows.Count > 0 Then
                dt_MedExScreeningDtl.Columns.Remove("ScreeningType")
                dt_MedExScreeningDtl.TableName = "MedExScreeningDtl"
                dt_MedExScreeningDtl.AcceptChanges()
                ds.Tables.Add(dt_MedExScreeningDtl.Copy)
            End If

            dt_Save.DefaultView.RowFilter = ""
            dt_Save.DefaultView.RowFilter = "ScreeningType='P'"
            dt_MedExWorkSpaceScreeningDtl = dt_Save.DefaultView.ToTable
            If dt_MedExWorkSpaceScreeningDtl.Rows.Count > 0 Then

                dt_MedExWorkSpaceScreeningDtl.Columns.Remove("ScreeningType")
                dt_MedExWorkSpaceScreeningDtl.Columns("nMedExScreeningHdrNo").ColumnName = "nMedExWorkSpaceScreeningHdrNo"
                dt_MedExWorkSpaceScreeningDtl.Columns("nMedExScreeningDtlNo").ColumnName = "nMedExWorkSpaceScreeningDtlNo"
                dt_MedExWorkSpaceScreeningDtl.TableName = "MedExWorkSpaceScreeningDtl"

                'if there is data in MedExWorkSpaceScreeningDtl then it will add row in MedExWorkSpaceScreeningHdr
                If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                    dt_MedExWorkSpaceScreeningHdr = dt_SubMedExMst.Copy()
                    dt_MedExWorkSpaceScreeningHdr.Rows(0)("vWorkSpaceId") = HProjectId.Value.ToString
                    vWorkspaceId = HProjectId.Value.ToString
                    dt_MedExWorkSpaceScreeningHdr.Columns.Add("nMedWorkSpaceExScreeningHdrNo")
                    dt_MedExWorkSpaceScreeningHdr.TableName = "MedExWorkSpaceScreeningHdr"
                    dt_MedExWorkSpaceScreeningHdr.AcceptChanges()
                    ds.Tables.Add(dt_MedExWorkSpaceScreeningHdr.Copy)

                ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                    If Not objHelp.GetViewScreeningTemplateHdrDtl("nMedExScreeningHdrNo=" & CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows(0)("nMedExScreeningHdrNo").ToString + " AND vWorkSpaceId <> '0000000000'", _
                                                                 WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                         ds_EditMedExWorkSpaceScrDtl, estr) Then
                        Return False
                    End If

                    If Not objHelp.GetMedExWorkSpaceScreeningHdr("nMedExScreeningHdrNo=" & CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows(0)("nMedExScreeningHdrNo"), _
                                                                 WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                         ds_Temp, estr) Then
                        Return False
                    End If
                    If Not ds_Temp Is Nothing AndAlso ds_Temp.Tables(0).Rows.Count > 0 Then
                        MedExScreeningHdrNo = ds_Temp.Tables(0).Rows(0)("nMedWorkSpaceExScreeningHdrNo").ToString
                    Else

                    End If

                    ds_Temp = New DataSet
                    ds_Temp.Tables.Add(dt_MedExWorkSpaceScreeningDtl.Copy().Clone())
                    Dim nMedExWorkSpaceScreeningHdrNo As String = ""
                    Dim dsMedExWorkSpaceScreeningDtl As DataSet
                    If Not dt_MedExWorkSpaceScreeningDtl Is Nothing AndAlso dt_MedExWorkSpaceScreeningDtl.Rows.Count > 0 Then
                        wStr = "vSubjectId = '" + Request.QueryString("SubId") + "' AND CAST(dScreenDate as DATE) = cast( '" + Request.QueryString("ScrDt") + "' as DATE)"
                        If Not objHelp.GetMedExWorkSpaceScreeningHdr(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                        dsMedExWorkSpaceScreeningDtl, estr) Then
                            Return False
                        End If
                        If Not dsMedExWorkSpaceScreeningDtl Is Nothing AndAlso dsMedExWorkSpaceScreeningDtl.Tables(0).Rows.Count > 0 Then
                            nMedExWorkSpaceScreeningHdrNo = dsMedExWorkSpaceScreeningDtl.Tables(0).Rows(0)("nMedWorkSpaceExScreeningHDRNo")
                        End If

                    End If


                    For index = 0 To dt_MedExWorkSpaceScreeningDtl.Rows.Count - 1
                        ds_EditMedExWorkSpaceScrDtl.Tables(0).DefaultView.RowFilter = "vMedExCode='" + dt_MedExWorkSpaceScreeningDtl.Rows(index)("vMedExCode").ToString + "'"
                        cnt = ds_EditMedExWorkSpaceScrDtl.Tables(0).DefaultView.ToTable.Rows.Count
                        If (cnt <= 0) Or (cnt > 0 AndAlso dt_MedExWorkSpaceScreeningDtl.Rows(index)("vMedExResult").ToString.Trim.ToUpper <> ds_EditMedExWorkSpaceScrDtl.Tables(0).DefaultView.ToTable.Rows(0)("vDefaultValue").ToString.Trim.ToUpper AndAlso ((dt_MedExWorkSpaceScreeningDtl.Rows(index)("vRemarks").ToString = "" AndAlso ds_EditMedExWorkSpaceScrDtl.Tables(0).DefaultView.ToTable.Rows(0)("iTranNo").ToString = 1 AndAlso ds_EditMedExWorkSpaceScrDtl.Tables(0).DefaultView.ToTable.Rows(0)("vDefaultValue").ToString.Trim.ToUpper = "") Or dt_MedExWorkSpaceScreeningDtl.Rows(index)("vRemarks").ToString <> "")) Then
                            dt_MedExWorkSpaceScreeningDtl.Rows(index)("nMedExWorkSpaceScreeningDtlNo") = 0
                            dt_MedExWorkSpaceScreeningDtl.Rows(index)("nMedExWorkSpaceScreeningHdrNo") = IIf(MedExScreeningHdrNo <> "", MedExScreeningHdrNo, nMedExWorkSpaceScreeningHdrNo)
                            ds_Temp.Tables(0).ImportRow(dt_MedExWorkSpaceScreeningDtl.Rows(index))
                        End If
                    Next

                    For Each dr2 As DataRow In dt_MedExWorkSpaceScreeningDtl.Rows
                        dr2("nMedExWorkSpaceScreeningHdrNo") = IIf(MedExScreeningHdrNo <> "", MedExScreeningHdrNo, nMedExWorkSpaceScreeningHdrNo)
                    Next
                    dt_MedExWorkSpaceScreeningDtl.AcceptChanges()

                    ds_Temp.AcceptChanges()
                    If ds_Temp.Tables(0).Rows.Count > 0 Then
                        dt_MedExWorkSpaceScreeningDtl.Clear()
                        dt_MedExWorkSpaceScreeningDtl = ds_Temp.Tables(0).Copy()
                    End If

                End If
            End If
            dt_MedExWorkSpaceScreeningDtl.AcceptChanges()
            ds.Tables.Add(dt_MedExWorkSpaceScreeningDtl.Copy)
            Me.ViewState(VS_dsSubMedex) = ds
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "... AssignValue")
            Return False
        End Try

    End Function

    Protected Sub btnContinueSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinueSave.Click
        Dim Subjectdetails As String = String.Empty
        Dim ScreeningDate As String = String.Empty
        Try
            If rblScreeningDate.SelectedValue.ToString.ToString.Trim() = "N" Then
                If Not Session("dScreenDate") Is Nothing AndAlso Session("dScreenDate").ToString() = "" Then
                    ScreeningDate = Nothing
                    Try
                        Session("dScreenDate") = String.Empty
                        Session("TranscribeRemarks") = Convert.ToString(txtTranscribeRemarks.Text)
                    Catch ex As Exception
                        Session("dScreenDate") = ""
                        Session("TranscribeRemarks") = ""
                    End Try


                End If
            Else
                ScreeningDate = rblScreeningDate.SelectedValue.ToString.ToString.Trim()
                Try
                    Session("dScreenDate") = rblScreeningDate.SelectedValue.ToString.ToString.Trim()
                    Session("TranscribeRemarks") = Convert.ToString(txtTranscribeRemarks.Text)
                Catch ex As Exception
                    Session("dScreenDate") = ""
                    Session("TranscribeRemarks") = ""
                End Try

            End If

            ContinueMode = 1
            Me.ViewState(VS_ContinueMode) = ContinueMode
            Subjectdetails = Me.txtSubject.Text.ToString
            Me.ViewState(VS_ProjectNo) = Me.HProjectId.Value
            Me.ViewState(VS_SubjectID) = Me.HSubjectId.Value
            BtnSave_Click(sender, e)
            PlaceMedEx.Controls.Clear()
            ShowHideControls("H")
            'If Not fillScreeningDates() Then
            '    Throw New Exception("Error While Fill Screening Dates")
            'End If
            Me.ViewState(VS_ContinueMode) = Nothing
            Me.txtSubject.Text = Subjectdetails
            Me.HSubjectId.Value = Me.ViewState(VS_SubjectID).ToString.Trim()
            Me.HProjectId.Value = Me.ViewState(VS_ProjectNo).ToString.Trim()
            If ScreeningDate Is Nothing Or ScreeningDate = "" Then
                rblScreeningDate.SelectedValue = rblScreeningDate.Items(1).Value
            Else
                rblScreeningDate.SelectedValue = ScreeningDate
            End If
            If Not ValidateUserWiseScreeningEntry("CONTINUE", Me.HSubjectId.Value, rblScreeningDate.SelectedValue) Then
                'ObjCommon.ShowAlert("Another User was working on this screening Date of this subjectid", Me.Page)
                For Each li As ListItem In rblScreeningDate.Items
                    li.Selected = False
                Next
                Session("ScreeningGroup") = ""
                GroupValidation = True
                Dim url As String = Request.Url.ToString()
                url = url.Substring(0, url.IndexOf("Group"))
                Response.Redirect(url, False)
                Exit Sub
            End If
            Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&Set=true&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Request.QueryString("SubId") + "&Save=true&Group=" + hdnSubGroup.Value.Replace("Div", ""), False)


        Catch ex As Exception
        End Try
    End Sub

    Protected Sub btnSaveuthenticate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveuthenticate.Click
        BtnSave_Click(sender, e)
    End Sub

#End Region

#Region "Button Click"

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Dim nMedExScreenNo As Integer
        Dim estr As String = String.Empty
        Dim ds As New DataSet
        GroupValidation = False
        IsSaved = False
        Session("IsProjectSpecificScreening") = False
        Session("hProjectDescProject") = ""
        Session("hProjectIdWorkSpace") = ""
        Session("ScreeningDTLNo") = ""
        Session("DCFMedExCode") = ""
        Session("dScreenDate") = ""
        hdnScreeningDate.Value = ""
        'SuccessMsgFlag = False
        nMedExScreenNo = 1
        Session("DCFGenerated") = "False"
        Session("isTransCribe") = ""
        hdnDCFGenerated.Value = ""

        If Not Me.hMedExNo.Value = String.Empty AndAlso rblScreeningDate.SelectedValue.ToString.ToString.Trim() <> "N" Then
            nMedExScreenNo = Me.hMedExNo.Value
        End If

        If Not Me.Request.QueryString("SubId") Is Nothing Then
            If Not Me.Request.QueryString("ScrDt") Is Nothing Then
                If Me.Request.QueryString("ScrDt") <> "N" Then
                    If Not ValidateUserWiseScreeningEntry("REMOVEENTRY", Me.Request.QueryString("SubId").ToString.Trim(), Me.Request.QueryString("ScrDt")) Then
                        ObjCommon.ShowAlert("Error While Removing Entry In Screening Control", Me.Page)
                    End If
                Else
                    If Not ValidateUserWiseScreeningEntry("REMOVEENTRY", Me.Request.QueryString("SubId").ToString.Trim(), Me.rblScreeningDate.SelectedValue.ToString.ToString) Then
                        ObjCommon.ShowAlert("Error While Removing Entry In Screening Control", Me.Page)
                    End If
                End If
            End If
        End If

        If Not objHelp.DeleteScreenigTmpTable(nMedExScreenNo, Me.HSubjectId.Value, ds, estr) Then
            Throw New Exception
        End If
        Me.Session.Remove("PlaceMedEx")
        Dim choice As New WS_Lambda.DataObjOpenSaveModeEnum
        choice = Me.ViewState(VS_Choice)

        If (Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso _
            Me.Request.QueryString("SubjectId").ToString.Trim() <> "") Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "", "closewindow()", True)
            Exit Sub
        End If
        If Me.Request.QueryString("mode") = 4 Then
            If Not IsNothing(Me.Request.QueryString("QC")) AndAlso _
                        (Me.Request.QueryString("QC").ToString.Trim() = "true") Then
                Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&QC=true&Set=true", False)
            ElseIf Not IsNothing(Me.Request.QueryString("ScrHdrNo")) AndAlso _
                        (Me.Request.QueryString("ScrHdrNo").ToString.Trim() <> "") Then
                Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=" + Me.HProjectId.Value + "&ScrHdrNo= " + Me.HScrNo.Value + "&SubId=" + Me.HSubjectId.Value.ToString() + "&Set=true", False)
            Else
                Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&Set=true", False)
            End If
        Else
            Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&Set=true", False)
        End If

    End Sub

    Protected Sub btnSubject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubject.Click
        Try
            IsGunned = False
            IsSaved = False
            PlaceMedEx.Controls.Clear()
            ShowHideControls("H")
            FillProfile()
            If Me.Request.QueryString("mode") = 4 Then
                If Not IsNothing(Me.Request.QueryString("QC")) AndAlso _
                        (Me.Request.QueryString("QC").ToString.Trim() = "true") Then
                    Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&Set=true&QC=true&SubId=" + Me.HSubjectId.Value.ToString() + "", False)
                ElseIf Not IsNothing(Me.Request.QueryString("ScrHdrNo")) AndAlso _
                        (Me.Request.QueryString("ScrHdrNo").ToString.Trim() <> "") Then
                    Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&Set=true&ScrHdrNo= " + Me.HScrNo.Value + "&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Me.HSubjectId.Value.ToString() + "", False)
                Else
                    Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&Set=true&SubId=" + Me.HSubjectId.Value.ToString() + "", False)
                End If
            Else
                Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&Set=true&SubId=" + Me.HSubjectId.Value.ToString() + "", False)
            End If

        Catch ex As Exception
            Me.ObjCommon.ShowAlert(ex.Message, Me.Page)
        End Try

    End Sub

    'Protected Sub btnHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHistory.Click
    '    Dim MedexGroupCode As String = String.Empty

    '    If Me.rblScreeningDate.Items.Count > 0 AndAlso Me.rblScreeningDate.SelectedValue.ToUpper.Trim() = "N" Then 'For New
    '        Me.ObjCommon.ShowAlert("No Audit Trail Available For New Screening", Me.Page())
    '    End If

    '    If Not fillGrid(MedexGroupCode) Then
    '        Exit Sub
    '    End If
    '    Me.MPEDeviation.Show()
    'End Sub

    Protected Sub btnRmkHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRmkHistory.Click
        Dim dsRmkHistory As New DataSet
        Dim dvRmkHistory As New DataView
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty

        Try
            Wstr = "  vSubjectID = '" + Me.HSubjectId.Value + "' And cast(convert(varchar(11),dScreenDate,113) as smalldatetime)= cast(convert(varchar(11),cast('" & Me.rblScreeningDate.SelectedValue.Trim() & "' as datetime),113)as smalldatetime) "
            If Not objHelp.View_MedExScreeningHdrHistory(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                              dsRmkHistory, estr) Then
                Throw New Exception(estr)
            End If

            If dsRmkHistory.Tables(0).Rows.Count > 0 Then
                Me.GVAuditFnlRmk.DataSource = dsRmkHistory
                Me.GVAuditFnlRmk.DataBind()
                Me.divAudit.Style.Add("display", "block")
            Else
                ObjCommon.ShowAlert("No Remark History Found", Me.Page)
                Me.btnRmkHistory.Style.Add("display", "none")
                Exit Sub
            End If
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('divAudit');", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnRmkHistory_Click")
        End Try
    End Sub

    Protected Sub btnDeleteScreenNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteScreenNo.Click


    End Sub

    Protected Sub btnAuthenticate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAuthenticateHide.Click
        Dim ds_SubMedExMst As New DataSet
        Dim estr As String = String.Empty
        Dim dt_SubMedExMst As New DataTable
        Dim nMedExScreenNo As Integer
        Dim ds As New DataSet
        Dim ds_WorkFlow As New DataSet

        Dim ScreeningDate As String = String.Empty

        Try

            If rblScreeningDate.SelectedValue.ToString.ToString.Trim() = "N" Then
                ScreeningDate = Nothing
            Else
                ScreeningDate = rblScreeningDate.SelectedValue.ToString.ToString.Trim()
            End If

            If Not ValidateWorkflow("Auth") Then
                ObjCommon.ShowAlert("Error while validating", Me.Page)
                Exit Sub
            End If

            If Not Me.Request.QueryString("SubId") Is Nothing Then
                If Not Me.Request.QueryString("ScrDt") Is Nothing Then
                    If Me.Request.QueryString("ScrDt") <> "N" Then
                        If Not ValidateUserWiseScreeningEntry("REMOVEENTRY", Me.Request.QueryString("SubId").ToString.Trim(), Me.Request.QueryString("ScrDt")) Then
                            ObjCommon.ShowAlert("Error While Removing Entry In Screening Control", Me.Page)
                        End If
                    Else
                        If Not ValidateUserWiseScreeningEntry("REMOVEENTRY", Me.Request.QueryString("SubId").ToString.Trim(), Me.rblScreeningDate.SelectedValue.ToString.ToString) Then
                            ObjCommon.ShowAlert("Error While Removing Entry In Screening Control", Me.Page)
                        End If
                    End If
                End If
            End If

            BtnExit.Enabled = True

            Me.trReviewCompleted.Visible = False
            For Each Items As ListItem In rblEligible.Items
                Items.Selected = False
            Next

            Me.chkReviewCompleted.Style.Add("display", "none")
            'Me.ChkEligible.Style.Add("display", "none")
            Me.btnReviewHistory.Style.Add("display", "none")
            'Me.btnPdf.Style.Add("display", "none")
            Me.chkScreeningType.ClearSelection()

            Me.rblScreeningDate.Items.Clear()
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "", "ShowHideproject()", True)
            Me.chkScreeningType.ClearSelection()
            If Not Me.hMedExNo.Value = String.Empty AndAlso rblScreeningDate.SelectedValue.ToString.ToString.Trim() <> "N" Then
                nMedExScreenNo = Me.hMedExNo.Value
            End If

            If Not objHelp.DeleteScreenigTmpTable(nMedExScreenNo, Me.HSubjectId.Value, ds, estr) Then
                Throw New Exception
            End If

            Me.ViewState(VS_SaveMode) = 1

            GenCall()

            If Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso _
            Convert.ToString(Me.Request.QueryString("SubjectId")).Trim() <> "" Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "", "closewindow()", True)
                Exit Sub
            End If

            '================================================================

            If Not fillScreeningDates() Then
                Throw New Exception("Error While Fill Screening Dates")
            End If

            If ScreeningDate Is Nothing Then
                rblScreeningDate.SelectedValue = rblScreeningDate.Items(1).Value
            Else
                rblScreeningDate.SelectedValue = ScreeningDate
            End If
            If Not ValidateUserWiseScreeningEntry("CONTINUE", Me.HSubjectId.Value, rblScreeningDate.SelectedValue) Then
                'ObjCommon.ShowAlert("Another User was working on this screening Date of this subjectid", Me.Page)
                For Each li As ListItem In rblScreeningDate.Items
                    li.Selected = False
                Next

                Session("ScreeningGroup") = ""
                GroupValidation = True
                Dim url As String = Request.Url.ToString()
                url = url.Substring(0, url.IndexOf("Group"))
                Response.Redirect(url, False)
                Exit Sub

            End If

            rblScreeningDate_SelectedIndexChanged(sender, e)

            '================================================================

            'Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&Save=True&Set=true", False)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "Error Whille Authenticating ")
        End Try

    End Sub

    Protected Sub ImgbtnShowHome_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgbtnShowHome.Click
        Dim nMedExScreenNo As Integer
        Dim estr As String = String.Empty
        Dim ds As New DataSet
        IsSaved = False
        Session("IsProjectSpecificScreening") = False


        If Not Me.hMedExNo.Value = String.Empty AndAlso rblScreeningDate.SelectedValue.ToString.ToString.Trim() <> "N" Then
            nMedExScreenNo = Me.hMedExNo.Value
        End If

        If Not Me.Request.QueryString("SubId") Is Nothing Then
            If Not Me.Request.QueryString("ScrDt") Is Nothing Then
                If Me.Request.QueryString("ScrDt") <> "N" Then
                    If Not ValidateUserWiseScreeningEntry("REMOVEENTRY", Me.Request.QueryString("SubId").ToString.Trim(), Me.Request.QueryString("ScrDt")) Then
                        ObjCommon.ShowAlert("Error While Removing Entry In Screening Control", Me.Page)
                    End If
                Else
                    If Not ValidateUserWiseScreeningEntry("REMOVEENTRY", Me.Request.QueryString("SubId").ToString.Trim(), Me.rblScreeningDate.SelectedValue.ToString.ToString) Then
                        ObjCommon.ShowAlert("Error While Removing Entry In Screening Control", Me.Page)
                    End If
                End If
            End If
        End If

        nMedExScreenNo = 1
        If Not Me.hMedExNo.Value = "" AndAlso rblScreeningDate.SelectedValue.ToString.ToString.Trim() <> "N" Then
            If Not rblScreeningDate.SelectedValue.ToString.ToString.Trim() = "" Then
                nMedExScreenNo = Me.hMedExNo.Value
            End If
        End If

        If Not objHelp.DeleteScreenigTmpTable(nMedExScreenNo, Me.HSubjectId.Value, ds, estr) Then
            Throw New Exception
        End If
        Me.hMedExNo.Value = ""
        Me.Session(S_ScrProfileIndex) = Nothing
        Me.Response.Redirect("frmMainpage.aspx", False)
    End Sub

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_Check As New DataSet
        Dim dv_Check As New DataView
        Dim ds_Subjects As New DataSet
        Dim ds_CreatedBy As New DataSet
        Dim dv_CreatedBy As New DataView
        If (rblScreeningDate.Items.Count > 0) Then
            Me.rblScreeningDate.Items(0).Enabled = True
        End If



        Try
            If Me.ViewState(VS_Mode) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"

                If Not objHelp.GetCRFLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    ds_Check, eStr) Then
                    Throw New Exception(eStr)
                End If
                If Not ds_Check Is Nothing Then

                    dv_Check = ds_Check.Tables(0).DefaultView
                    dv_Check.Sort = "iTranNo desc"
                    Session("hProjectIdWorkSpace") = HProjectId.Value
                    Session("hProjectDescProject") = txtproject.Text
                    If dv_Check.ToTable().Rows.Count > 0 Then

                        If Convert.ToString(dv_Check.ToTable().Rows(0)("cLockFlag")).Trim.ToUpper() = "L" Then
                            Me.ObjCommon.ShowAlert("Project is Locked. You can not do new Screening.", Me.Page)
                            Me.txtproject.Text = ""
                            Me.HProjectId.Value = ""
                            Session("hProjectIdWorkSpace") = ""
                            Session("hProjectDescProject") = ""
                            Me.rblScreeningDate.Items(0).Enabled = False
                            Exit Sub
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Getting  Lock Details ", ex.Message + "...btnSetProject_Click")
        End Try



    End Sub

    'Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
    '    Me.btnDeleteScreenNo_Click(sender, e)
    '    Me.Response.Redirect("frmMainPage.aspx", False)
    'End Sub
    Protected Sub btnDCFShowHide_Click(ByVal Sender As Object, ByVal e As System.EventArgs) Handles btnDCFShowHide.Click

        Try
            If Not Session("DCFGenerated") Is Nothing AndAlso (hdnDCFGenerated.Value = "True" Or Session("DCFGenerated").ToString() = "True") Then
                If hdnDCFGenerated.Value = "False" Then
                    Session("DCFGenerated") = "False"
                    'Me.mpDCFGenerate.Hide()
                    ShowDCFPopup("H")
                    hdnDCFGenerated.Value = "False"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "DCFS", "SHOEDCFFROMVB('H');", True)
                Else
                    Session("DCFGenerated") = "True"
                    ShowDCFPopup("S")
                    'Me.mpDCFGenerate.Show()
                    hdnDCFGenerated.Value = "True"
                    mpDCFGenerate.Show()
                    'DCFPanel.Update()
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "DCFS", "SHOEDCFFROMVB('S');", True)
                End If
            ElseIf Not Session("DCFGenerated") Is Nothing AndAlso (hdnDCFGenerated.Value = "False" Or Session("DCFGenerated").ToString() = "False") Then
                Session("DCFGenerated") = "False"
                Me.mpDCFGenerate.Hide()
                hdnDCFGenerated.Value = "False"
                'DCFPanel.Update()
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "DCFS", "SHOEDCFFROMVB('H');", True)
            End If
        Catch ex As Exception
        End Try
    End Sub


#Region "QC Div"

    Protected Sub BtnQCSaveSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnQCSaveSend.Click
        Dim estr As String = String.Empty
        Dim ds_QC As New DataSet
        Dim QCMsg As String = String.Empty
        Dim fromEmailId As String = String.Empty
        Dim toEmailId As String = String.Empty
        Dim password As String = String.Empty
        Dim ccEmailId As String = String.Empty
        Dim SubjectLine As String = String.Empty
        Dim wStr As String = String.Empty

        Try
            If Me.lblResponse.Text = "" And CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.ObjCommon.ShowAlert("Please Click Response Button Of Particular QC Comments Against Which You Want To Response", Me.Page)
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHide", "QCDivShowHide('ST');", True)
                Exit Sub
            End If

            If Not AssignValues_MedExScreeningHdrQC(ds_QC) Then
                Me.ObjCommon.ShowAlert("Error While Assigning Data.", Me.Page)
                Exit Sub
            End If


            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                If Not Me.objLambda.Save_MedexScreeningHdrQC(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_QC, L_UserId, estr) Then
                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Exit Sub
                End If


                QCMsg = "QC On Screening of " + Me.txtSubject.Text.Trim() + " <br/><br/>QC : " + Me.RBLQCFlag.SelectedItem.Text.Trim() + _
                        "<br/><br/>QC Comments: " + Me.txtQCRemarks.Value + "<br/><br/>" + _
                        "Comments Given By : " + L_FirstName + " " + L_LastName

                fromEmailId = ConfigurationSettings.AppSettings("Username")
                password = ConfigurationSettings.AppSettings("Password")


                toEmailId = Me.txtToEmailId.Text.Trim() 'ds_EmailAlert.Tables(0).Rows(0)("vToUserEmailId").ToString()
                ccEmailId = Me.txtCCEmailId.Text.Trim() 'ds_EmailAlert.Tables(0).Rows(0)("vCCUserEmailId").ToString()
                SubjectLine = "QC On Screening " + HSubjectId.Value.Trim() 'ds_EmailAlert.Tables(0).Rows(0)("vSubjectLine").ToString()

                Dim sn As New SendMail(Server, Request, Session, SubjectLine, QCMsg, fromEmailId, password, toEmailId, ccEmailId)

                If Not sn.Send(Server, Response, Session, , fromEmailId, password) Then
                    Me.ObjCommon.ShowAlert("QC Comments Saved Successfully But Email Is Not Sent Due To Some Reason", Me.Page)
                    Exit Sub
                End If
                '****************************************************
                sn = Nothing
                Me.ObjCommon.ShowAlert("QC Comments Saved Successfully", Me.Page)

            Else 'For Response
                If Not Me.objLambda.Save_MedexScreeningHdrQC(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_QC, L_UserId, estr) Then
                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Exit Sub
                End If
                Me.ObjCommon.ShowAlert("Response Saved Successfully", Me.Page)

            End If

            If Not fillQCGrid() Then
                Exit Sub
            End If

            Me.txtQCRemarks.Value = ""
            Me.lblResponse.Text = ""
            Me.RBLQCFlag.SelectedValue = "F"
            Me.txtToEmailId.Text = ""
            Me.txtCCEmailId.Text = ""
            Me.Session.Remove("PlaceMedEx")
            rblScreeningDate_SelectedIndexChanged(sender, e) 'Call Screen Date Select
            '***********************

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

    Protected Sub BtnQCSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnQCSave.Click
        Dim estr As String = String.Empty
        Dim ds_QC As New DataSet
        Dim QCMsg As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ScreeningDate As String = String.Empty
        Dim Subjectdetails As String = String.Empty
        Try
            If Me.lblResponse.Text = "" And CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.ObjCommon.ShowAlert("Please Click Response Button Of Particular QC Comments Against Which You Want To Response", Me.Page)
                Me.MpeQC.Show()
                Exit Sub
            End If

            If rblScreeningDate.SelectedValue.ToString.ToString.Trim() = "N" Then

                ScreeningDate = Nothing
            Else
                ScreeningDate = rblScreeningDate.SelectedValue.ToString.ToString.Trim()
            End If
            ContinueMode = 1
            Me.ViewState(VS_ContinueMode) = ContinueMode
            Subjectdetails = Me.txtSubject.Text.ToString
            Me.ViewState(VS_ProjectNo) = Me.HProjectId.Value
            Me.ViewState(VS_SubjectID) = Me.HSubjectId.Value

            If Not AssignValues_MedExScreeningHdrQC(ds_QC) Then
                Me.ObjCommon.ShowAlert("Error While Assigning Data.", Me.Page)
                Exit Sub
            End If


            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                If Not Me.objLambda.Save_MedexScreeningHdrQC(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_QC, L_UserId, estr) Then
                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Exit Sub
                End If
                Me.ObjCommon.ShowAlert("QC Comments Saved Successfully", Me.Page)

            Else 'For Response
                If Not Me.objLambda.Save_MedexScreeningHdrQC(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_QC, L_UserId, estr) Then
                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Exit Sub
                End If
                Me.ObjCommon.ShowAlert("Response Saved Successfully", Me.Page)
            End If

            If Not fillQCGrid() Then
                Exit Sub
            End If
            Me.txtQCRemarks.Value = ""
            Me.lblResponse.Text = ""
            Me.RBLQCFlag.SelectedValue = "F"
            Me.Session.Remove("PlaceMedEx")


            If Not ValidateUserWiseScreeningEntry("CONTINUE", Me.HSubjectId.Value, rblScreeningDate.SelectedValue) Then
                ObjCommon.ShowAlert("Another User was working on this screening Date of this subjectid", Me.Page)
                For Each li As ListItem In rblScreeningDate.Items
                    li.Selected = False
                Next
                Session("ScreeningGroup") = ""
                GroupValidation = True
                Dim url As String = Request.Url.ToString()
                url = url.Substring(0, url.IndexOf("Group"))
                Response.Redirect(url, False)
                Exit Sub
            End If
            If Not fillScreeningDates() Then
                Throw New Exception("Error While Fill Screening Dates")
            End If


            If ScreeningDate Is Nothing Then
                rblScreeningDate.SelectedValue = rblScreeningDate.Items(1).Value
            Else
                rblScreeningDate.SelectedValue = ScreeningDate
            End If
            rblScreeningDate_SelectedIndexChanged(sender, e)
            Me.ViewState(VS_ContinueMode) = Nothing
            Me.txtSubject.Text = Subjectdetails
            Me.HSubjectId.Value = Me.ViewState(VS_SubjectID).ToString.Trim()
            Me.HProjectId.Value = Me.ViewState(VS_ProjectNo).ToString.Trim()

            '***********************

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...BtnQCSave_Click")
        End Try

    End Sub

#End Region

    'Protected Sub btnTranscribeCancel_Click(sender As Object, e As EventArgs) Handles btnTranscribeCancel.Click
    '    hdnIsModelPopupShow.Value = False
    '    MpeTranscribeScreening.Hide()
    '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "TransccribePop", "ShowTranscribePopup();", True)
    'End Sub

    'Protected Sub btnContinue_Click(sender As Object, e As EventArgs) Handles btnContinue.Click
    '    Dim ds_Validation As DataSet
    '    Dim wstr = Convert.ToString(HSubjectId.Value) + "##" + Me.txtTranscribeDate.Text
    '    ds_Validation = Me.objHelpdb.ProcedureExecute("dbo.Proc_ScreeningSameDayValidation", wstr)
    '    If Not ds_Validation Is Nothing AndAlso ds_Validation.Tables(0).Rows.Count > 0 Then
    '        Me.ObjCommon.ShowAlert("Screening For the Subject is done selected date,You can Edit that But cannot add a new Screening for same Date.", Me.Page)
    '        Me.MpeTranscribeScreening.Show()
    '        Exit Sub
    '    End If

    '    If Me.rbtnYes.Checked = True Then
    '        hdnScreeningDate.Value = Convert.ToString(txtTranscribeDate.Text)
    '        hdnTranscribeRemarks.Value = Convert.ToString(txtTranscribeRemarks.Text)
    '        dScreenDate = Convert.ToString(txtTranscribeDate.Text)
    '        TranscribeRemarks = Convert.ToString(txtTranscribeRemarks.Text)
    '    End If
    '    hdnIsModelPopupShow.Value = False
    '    MpeTranscribeScreening.Hide()
    '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "TransccribePopup", "ShowTranscribePopup();", True)
    'End Sub

    Protected Sub btnRedirectPage_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&Set=true&SubId=" + Me.HSubjectId.Value.ToString())
    End Sub

    Protected Sub btnContinueWorking_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinueWorking.Click
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ResetSessionTimer", "btnContinueWorking_Click();", True)
    End Sub

    Protected Sub txtWristBand_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtWristBand.TextChanged
        Try
            Dim ds_Screening As DataSet
            Dim wStr As String

            IsGunned = True
            Dim wStrWrist As String

            ''Change Logic By Vivek Patel
            wStrWrist = "Select * From ScreeningBarcodeMst Where vSubjectId = '" + Convert.ToString(txtWristBand.Text).ToString().Split("-")(0).Trim() + "-" + Convert.ToString(txtWristBand.Text).ToString().Split("-")(1).Trim() + "' AND vScreeningBarcode = '" + txtWristBand.Text.Trim() + "'"
            ds_Screening = objHelp.GetResultSet(wStrWrist, "ScreeningBarcodeMst ")

            If Not ds_Screening Is Nothing AndAlso ds_Screening.Tables(0).Rows.Count = 1 Then

                Dim dateCompare = DateTime.Compare(DateTime.Parse(Convert.ToString(ds_Screening.Tables(0)(0)(6))), DateTime.Now.Date)

                If dateCompare = 1 Then
                    wStr = "Select * From MedExSCreeningHdr Where vSubjectId = '" + Convert.ToString(txtWristBand.Text).Trim.Split(New String() {"--"}, StringSplitOptions.None)(0) + "' AND SUBSTRING(CONVERT(varchar, dScreenDate, 112), 1, 8) =  '" + Convert.ToString(txtWristBand.Text).Trim.Split(New String() {"--"}, StringSplitOptions.None)(1) + "' "

                    ds_Screening = objHelp.GetResultSet(wStr, "MedExSCreeningHdr")

                    If Not ds_Screening Is Nothing And ds_Screening.Tables(0).Rows.Count = 0 Then
                        If Not Request.QueryString("mode") Is Nothing AndAlso Request.QueryString("mode").ToString() = "4" Then
                            Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&Set=true&ScrDt=" + "N" + "&SubId=" + Convert.ToString(txtWristBand.Text).Trim.Split(New String() {"--"}, StringSplitOptions.None)(0), False)
                        Else
                            Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&Set=true&ScrDt=" + "N" + "&SubId=" + Convert.ToString(txtWristBand.Text).Trim.Split(New String() {"--"}, StringSplitOptions.None)(0), False)
                        End If

                    Else
                        If Not Request.QueryString("mode") Is Nothing AndAlso Request.QueryString("mode").ToString() = "4" Then
                            Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&Set=true&ScrDt=" + DateTime.ParseExact(Convert.ToString(txtWristBand.Text).Trim.Split(New String() {"--"}, StringSplitOptions.None)(1), "yyyyMdd", Nothing).ToString("M\/dd\/yyyy hh:mm:ss tt") + "&SubId=" + Convert.ToString(txtWristBand.Text).Trim.Split(New String() {"--"}, StringSplitOptions.None)(0) + "&Group=", False)
                        Else
                            Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&Set=true&ScrDt=" + DateTime.ParseExact(Convert.ToString(txtWristBand.Text).Trim.Split(New String() {"--"}, StringSplitOptions.None)(1), "yyyyMdd", Nothing).ToString("M\/dd\/yyyy hh:mm:ss tt") + "&SubId=" + Convert.ToString(txtWristBand.Text).Trim.Split(New String() {"--"}, StringSplitOptions.None)(0) + "&Group=", False)
                        End If

                    End If
                Else
                    txtWristBand.Text = ""
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "AlertOfWristband", "alert('WristBand is Not Of Current Date.')", True)
                End If
            Else
                txtWristBand.Text = ""
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "AlertOfWristband", "alert('Barcode You Have Gunned Is Not Generated or There is Multiple Barcode Entry of Same Date.')", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "AlertOfWristband", "alert('Please gunned Barcode Properly or Correct Barcode!')", True)
        End Try

    End Sub

    Protected Sub ViewAll_Click(sender As Object, e As EventArgs)

        If Me.Request.QueryString("mode") = 4 Then
            If Not IsNothing(Me.Request.QueryString("QC")) AndAlso _
            (Me.Request.QueryString("QC").ToString.Trim() = "true") Then
                Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&QC=true&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Request.QueryString("SubId") + "&Group=" + Session("ScreeningGroup").ToString() + "&ViewAll=True", False)
            ElseIf Not IsNothing(Me.Request.QueryString("ScrHdrNo")) AndAlso _
                (Me.Request.QueryString("ScrHdrNo").ToString.Trim() <> "") Then
                Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=" + Me.HProjectId.Value + "&ScrHdrNo= " + Me.HScrNo.Value + "&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Request.QueryString("SubId") + "&Group=" + Session("ScreeningGroup").ToString() + "&ViewAll=True", False)
            ElseIf Not IsNothing(Me.Request.QueryString("Attendance")) AndAlso _
                    (Me.Request.QueryString("Attendance").ToString.Trim() = "true") Then

                If Not (Me.Request.QueryString("ScrDt") Is Nothing) Then
                    Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=" + Me.Request.QueryString("Workspace").ToString() + "&ScrDt=" + Me.Request.QueryString("ScrDt").ToString() + "&SubId=" + Request.QueryString("SubId") + "&Attendance=true&Group=" + Session("ScreeningGroup").ToString() + "&ViewAll=True", False)
                Else
                    Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Request.QueryString("SubId") + "&Attendance=true" + "&ViewAll=True", False)
                End If

            Else
                Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Request.QueryString("SubId") + "&Group=" + Session("ScreeningGroup").ToString() + "&ViewAll=True", False)
            End If
        ElseIf Me.ViewState(VS_SaveMode) = 1 Then

            Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&Set=true&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Request.QueryString("SubId") + "&Save=true" + "&ViewAll=True", False)
        Else
            If hdnSubGroup.Value <> "" Then
                Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&Set=true&ScrDt=" + IIf(Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() = "", "N", Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim()) + "&SubId=" + Request.QueryString("SubId") + "&Group=" + Session("ScreeningGroup").ToString() + "&ViewAll=True", False)
            Else
                Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&Set=true&ScrDt=" + IIf(Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() = "", "N", Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim()) + "&SubId=" + Request.QueryString("SubId") + "&Group=" + Session("ScreeningGroup").ToString() + "&ViewAll=True", False)
            End If

        End If

    End Sub
#End Region

#Region "Grid Events"

#Region "GVQCDtl Grid Events"

    Protected Sub GVQCDtl_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVQCDtl.RowCreated
        e.Row.Cells(GVCQC_MedExScreeningHdrQCNo).Visible = False
        e.Row.Cells(GVCQC_SubjectId).Visible = False
        e.Row.Cells(GVCQC_QCFlag).Visible = False
        e.Row.Cells(GVCQC_LnkResponse).Visible = True

        If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
            e.Row.Cells(GVCQC_LnkResponse).Visible = False
        End If
    End Sub

    Protected Sub GVQCDtl_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVQCDtl.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lnkResponse"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkResponse"), LinkButton).CommandName = "Response"
            CType(e.Row.FindControl("lnkResponse"), LinkButton).OnClientClick = "return QCDivShowHide('ST');"
        End If

    End Sub

    Protected Sub GVQCDtl_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GVQCDtl.RowCommand
        Dim index As Integer = CType(e.CommandArgument, Integer)
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds_MEdexInfroHdrQC As New DataSet

        If e.CommandName.ToUpper.Trim() = "RESPONSE" Then
            Me.lblResponse.Text = "Response To: " + Me.GVQCDtl.Rows(index).Cells(GVCQC_QCComment).Text.Trim()

            Wstr = "nMedExScreeningHdrQCNo=" + Me.GVQCDtl.Rows(index).Cells(GVCQC_MedExScreeningHdrQCNo).Text.Trim()
            If Not objHelp.GetMedexScreeningHdrQC(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_MEdexInfroHdrQC, estr) Then
                Me.ObjCommon.ShowAlert(estr, Me.Page)
                Exit Sub
            End If

            Me.ViewState(VS_DtQC) = ds_MEdexInfroHdrQC.Tables(0)
            Me.MpeQC.Show()
        End If
    End Sub
#End Region

#Region "GVAuditFnlRmk"

    Protected Sub GVAuditFnlRmk_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVAuditFnlRmk.RowCreated
        e.Row.Cells(GVAudit_MedExScreeningHdrNo).Visible = False
    End Sub

    Protected Sub GVAuditFnlRmk_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVAuditFnlRmk.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVAudit_SrNO).Text = e.Row.RowIndex + 1 + (Me.GVAuditFnlRmk.PageSize * Me.GVAuditFnlRmk.PageIndex)
        End If
    End Sub

#End Region

#End Region

#Region "Selected Index Change"

    Protected Sub rblScreeningDate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblScreeningDate.SelectedIndexChanged
        Dim wStr As String = String.Empty
        Dim ds_EmailAlert As New DataSet
        Dim eStr As String = String.Empty
        Dim ds_AuditTrail As New DataSet
        Dim dv_AuditTrail As New DataView
        Dim drsub As String = String.Empty
        Dim TodayDate As String = String.Empty
        Dim UserName As String = String.Empty
        Dim Subjedctid As String = String.Empty
        Dim ds_Validation As DataSet

        Try

            If hdnUserTypeName.Value <> hdnWebConfigUserTypeName.Value And rblScreeningDate.SelectedValue = "N" Then
                Dim estrv As String = ""
                Dim wstr1 = Convert.ToString(HSubjectId.Value) + "##" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now)
                objHelp.Proc_ScreeningSameDayValidation(Convert.ToString(HSubjectId.Value), String.Format("{0:dd-MMM-yyyy}", DateTime.Now), ds_Validation, estrv)
                If Not ds_Validation Is Nothing AndAlso ds_Validation.Tables(0).Rows.Count > 0 Then
                    Me.ObjCommon.ShowAlert("Screening of the subject is done today only, you can edit that but cannot add a new screening for today's date !", Me.Page)
                End If

            End If

            Me.PlaceMedEx.Controls.Clear()
            Me.BtnSave.Visible = True
            Me.btnContinueSave.Visible = True
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.BtnSave.Visible = False
                Me.btnContinueSave.Visible = False
            End If

            If ContinueMode = 1 Then
                wStr = "vSubjectId='" & Me.ViewState(VS_SubjectID).ToString & "'"
                Subjedctid = Me.ViewState(VS_SubjectID).ToString
            Else
                wStr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "'"
                Subjedctid = Me.HSubjectId.Value.Trim()

                If Not objHelp.GetData("View_fillScreeningDate", "*", wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                             ds_AuditTrail, eStr) Then
                    Me.ObjCommon.ShowAlert("Error While Getting Data From View_MedExScreeningHdrDtlAuditTrail : " + eStr, Me.Page)
                    Exit Sub
                End If

                dv_AuditTrail = ds_AuditTrail.Tables(0).DefaultView() '.ToTable(True, "dScreenDate,vReviewBy,nMedExScreeningHdrNo".Split(",")).DefaultView()
                dv_AuditTrail.Sort = "dScreenDate desc"
                If rblScreeningDate.SelectedValue.ToString.ToString.Trim() = "N" Then
                    Dim drScreeningTime As DataRow
                    drScreeningTime = dtScreeningTime.NewRow()
                    dNewScreeningDateTime = CDate(ObjCommon.GetCurDatetimeWithOffSet(L_TimeZoneName).DateTime)
                    drScreeningTime("dNewScreeningDateTime") = CType(dNewScreeningDateTime, Date).ToString("dd-MMM-yyyy hh:mm:ss").Trim()
                    dSavedScreeningDateTime = CDate(ObjCommon.GetCurDatetimeWithOffSet(L_TimeZoneName).DateTime)
                    drScreeningTime("dSavedScreeningDateTime") = CType(dSavedScreeningDateTime, Date).ToString("dd-MMM-yyyy hh:mm:ss").Trim()
                    dtScreeningTime.Rows.Add(drScreeningTime)
                    dtScreeningTime.AcceptChanges()
                    ViewState("dtScreeningTime") = dtScreeningTime
                    For Each dr As DataRow In dv_AuditTrail.ToTable().Rows


                        drsub = CType(dr("dScreenDate").ToString.Trim(), Date).ToString("dd-MMM-yyyy").Trim()
                        TodayDate = CDate(ObjCommon.GetCurDatetime(L_TimeZoneName)).Date.ToString("dd-MMM-yyyy").Trim()
                        isScreenDoneToday = False
                        If drsub = TodayDate Then
                            For Each Items As ListItem In rblScreeningDate.Items
                                Items.Selected = False
                            Next
                            isScreenDoneToday = True
                            Me.PlaceMedEx.Controls.Clear()
                            Me.ShowHideControls("H")
                            'Me.btnPdf.Style.Add("display", "none")
                            Me.btnReviewHistory.Style.Add("display", "none")
                            If Me.Request.QueryString("mode") = 4 Then
                                If Not IsNothing(Me.Request.QueryString("QC")) AndAlso _
                        (Me.Request.QueryString("QC").ToString.Trim() = "true") Then
                                    Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&QC=true&SubId=" + Subjedctid + "&ScrDt=" + Me.Request.QueryString("ScrDt").ToString(), False)
                                ElseIf Not IsNothing(Me.Request.QueryString("ScrHdrNo")) AndAlso _
                        (Me.Request.QueryString("ScrHdrNo").ToString.Trim() <> "") Then
                                    Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace= " + Me.HProjectId.Value + "&ScrHdrNo= " + Me.HScrNo.Value + "&SubjectId=" + Subjedctid + "&ScrDt=" + Me.Request.QueryString("ScrDt").ToString(), False)
                                Else
                                    Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&SubId=" + Subjedctid + "&ScrDt=" + Me.Request.QueryString("ScrDt").ToString(), False)
                                End If
                            Else
                                If Not (Me.Request.QueryString("ScrDt") Is Nothing) Then
                                    Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&SubId=" + Subjedctid + "&ScrDt=" + Me.Request.QueryString("ScrDt").ToString(), False)
                                Else
                                    If L_UserType = "0069" Then
                                        Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&SubId=" + Subjedctid + "&ScrDt=N", False)
                                    End If
                                    Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&SubId=" + Subjedctid + "&ScrDt=" + Me.Request.QueryString("ScrDt").ToString(), False)
                                End If

                            End If

                            Exit Sub
                        End If
                    Next

                    chkScreeningType.Enabled = True
                End If
                If Not Me.ViewState(VS_CheckEvent) = 1 Then
                    If Me.Request.QueryString("mode") = 4 Then
                        If Not IsNothing(Me.Request.QueryString("QC")) AndAlso _
                        (Me.Request.QueryString("QC").ToString.Trim() = "true") Then
                            If Not Me.Request.QueryString("Attendance") Is Nothing AndAlso Me.Request.QueryString("Attendance").ToString.Trim() = "true" Then
                                Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&QC=true&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Subjedctid + "&Attendance=true&Group=" + Session("ScreeningGroup").ToString(), False)
                            Else
                                Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&QC=true&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Subjedctid + "&Group=" + Session("ScreeningGroup").ToString(), False)
                            End If

                        ElseIf Not IsNothing(Me.Request.QueryString("ScrHdrNo")) AndAlso _
                            (Me.Request.QueryString("ScrHdrNo").ToString.Trim() <> "") Then
                            If Not Me.Request.QueryString("Attendance") Is Nothing AndAlso Me.Request.QueryString("Attendance").ToString.Trim() = "true" Then
                                Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=" + Me.HProjectId.Value + "&ScrHdrNo= " + Me.HScrNo.Value + "&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&Attendance=true&SubId=" + Subjedctid + "&Attendance=true&Group=" + Session("ScreeningGroup").ToString(), False)
                            Else
                                Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=" + Me.HProjectId.Value + "&ScrHdrNo= " + Me.HScrNo.Value + "&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Subjedctid + "&Group=" + Session("ScreeningGroup").ToString(), False)
                            End If

                        ElseIf Not IsNothing(Me.Request.QueryString("Attendance")) AndAlso _
                                (Me.Request.QueryString("Attendance").ToString.Trim() = "true") Then

                            If Not (Me.Request.QueryString("ScrDt") Is Nothing) Then
                                'Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=" + Me.Request.QueryString("Workspace").ToString() + +"&SubId=" + Subjedctid + "&Attendance=true", False)
                                'Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Subjedctid + "&Attendance=true&Group=" + ScreeningGroup, False)
                                If (rblScreeningDate.SelectedValue = "") Then
                                    If Not Me.Request.QueryString("Attendance") Is Nothing AndAlso Me.Request.QueryString("Attendance").ToString.Trim() = "true" Then
                                        Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&Set=true&SubId=" + Subjedctid + "&Attendance=true&Group=" + Session("ScreeningGroup").ToString(), False)
                                    Else
                                        Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&Set=true&SubId=" + Subjedctid + "&Group=" + Session("ScreeningGroup").ToString(), False)
                                    End If
                                Else
                                    If Not Me.Request.QueryString("Attendance") Is Nothing AndAlso Me.Request.QueryString("Attendance").ToString.Trim() = "true" Then
                                        Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&Set=true&SubId=" + Subjedctid + "&Attendance=true&Group=" + Session("ScreeningGroup").ToString(), False)
                                    Else
                                        Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&Set=true&SubId=" + Subjedctid + "&Group=" + Session("ScreeningGroup").ToString(), False)
                                    End If

                                End If
                            Else
                                If Not Me.Request.QueryString("Attendance") Is Nothing AndAlso Me.Request.QueryString("Attendance").ToString.Trim() = "true" Then
                                    Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&Set=true&SubId=" + Subjedctid + "&Attendance=true&Group=" + Session("ScreeningGroup").ToString(), False)
                                Else
                                    Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&Set=true&SubId=" + Subjedctid + "&Attendance=true&Group=" + Session("ScreeningGroup").ToString(), False)
                                End If

                            End If

                        Else
                            If Not Me.Request.QueryString("Attendance") Is Nothing AndAlso Me.Request.QueryString("Attendance").ToString.Trim() = "true" Then
                                Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Subjedctid + "&Attendance=true&Group=" + Session("ScreeningGroup").ToString(), False)
                            Else
                                Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Subjedctid + "&Group=" + Session("ScreeningGroup").ToString(), False)
                            End If

                        End If
                    ElseIf Me.ViewState(VS_SaveMode) = 1 Then

                        If Not Me.Request.QueryString("Attendance") Is Nothing AndAlso Me.Request.QueryString("Attendance").ToString.Trim() = "true" Then
                            Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&Set=true&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Subjedctid + "&Attendance=true&Save=true&Group=" + Session("ScreeningGroup").ToString(), False)
                        Else
                            Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&Set=true&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Subjedctid + "&Save=true&Group=" + Session("ScreeningGroup").ToString(), False)
                        End If

                        'Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&Set=true&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Subjedctid + "&Save=true", False)
                    Else
                        If Not Me.Request.QueryString("Attendance") Is Nothing AndAlso Me.Request.QueryString("Attendance").ToString.Trim() = "true" Then
                            Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&Set=true&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Subjedctid + "&Attendance=true&Group=" + Session("ScreeningGroup").ToString(), False)
                        Else
                            Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&Set=true&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Subjedctid + "&Group=" + Session("ScreeningGroup").ToString(), False)
                        End If

                    End If

                    Exit Sub
                Else
                    If Not Me.Request.QueryString("Attendance") Is Nothing AndAlso Me.Request.QueryString("Attendance").ToString.Trim() = "true" Then
                        Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&Set=true&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Subjedctid + "&Attendance=true&Group=" + Session("ScreeningGroup").ToString(), False)
                    Else
                        If Not Request.QueryString("mode") Is Nothing AndAlso Request.QueryString("mode") = "4" Then
                            Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&Set=true&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Subjedctid + "&Group=" + Session("ScreeningGroup").ToString(), False)
                        Else
                            Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&Set=true&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Subjedctid + "&Group=" + Session("ScreeningGroup").ToString(), False)
                        End If

                    End If

                End If
                Me.ViewState(VS_CheckEvent) = 1
            End If
            GenCall()
            If Not rblScreeningDate.SelectedValue.ToString.ToString.Trim() = "N" And rblScreeningDate.SelectedValue.ToString.ToString.Trim() <> "" Then
                If Not fillQCGrid() Then
                    Exit Sub
                End If
                If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                    chkScreeningType.Enabled = False
                End If
            Else
                chkScreeningType.Enabled = True
            End If


            wStr = "nEmailAlertId =" + Email_QCOFSCREENING.ToString() + " And cStatusIndi <> 'D'"
            If Not objHelp.GetEmailAlertMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_EmailAlert, eStr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From EmailAlert : " + eStr, Me.Page)
                Exit Sub
            End If

            If ds_EmailAlert.Tables(0).Rows.Count > 0 Then
                Me.txtToEmailId.Text = ds_EmailAlert.Tables(0).Rows(0)("vToUserEmailId").ToString()
                Me.txtCCEmailId.Text = ds_EmailAlert.Tables(0).Rows(0)("vCCUserEmailId").ToString()
            End If

            Me.divAudit.Style.Add("display", "none")

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub chkScreeningType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkScreeningType.SelectedIndexChanged
        Dim rblflag As String = String.Empty
        Try

            For Each li As ListItem In chkScreeningType.Items
                If li.Value = "PS" Then
                    If li.Selected Then
                        Session("IsProjectSpecificScreening") = True
                    Else
                        Session("IsProjectSpecificScreening") = False
                    End If
                End If
            Next

            'If chkScreeningType.SelectedIndex = 1 Then
            '    Session("IsProjectSpecificScreening") = True
            'Else
            '    Session("IsProjectSpecificScreening") = False
            'End If
            'Me.ViewState(VS_CheckEvent) = 1
            rblScreeningDate_SelectedIndexChanged(sender, e)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "chkScreeningType_SelectedIndexChanged")
        End Try
    End Sub

#End Region

#Region "DeleteScreeningtmpTable"
    <Web.Services.WebMethod()> _
    Public Shared Function DeleteScreeningtmpTable(ByVal nMedExScreenNo As String, _
                                       ByVal SubjectID As String) As Boolean
        Dim estr As String = String.Empty
        Dim ds As New DataSet

        Try
            If Not objHelp.DeleteScreenigTmpTable(nMedExScreenNo, SubjectID, ds, estr) Then
                Throw New Exception
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "Helper Functions"

    '#Region "fillGrid"

    '    Private Function fillGrid(ByRef MedexGroupCode As String) As Boolean
    '        Dim ds_AuditTrail As New DataSet
    '        Dim dv_AuditTrail As New DataView
    '        Dim Wstr As String = String.Empty
    '        Dim estr As String = String.Empty
    '        Dim dc_AuditTrailMst As DataColumn

    '        Try
    '            Wstr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "' And vMedexCode='" & Me.hfMedexCode.Value.Trim().Split("###")(0).ToString & "' And " & _
    '                    " dScreenDate='" & Me.rblScreeningDate.SelectedValue & "'"

    '            If Me.hfMedexCode.Value.Trim().Split("###")(3).ToString = "D" Then
    '                If Not objHelp.View_MedExScreeningHdrDtlAuditTrail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                 ds_AuditTrail, estr) Then
    '                    Return False
    '                End If
    '            Else
    '                If Not objHelp.GetData("View_MedExScreeningHdrDtlAuditTrail_ProjectSpecific", "*", Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                 ds_AuditTrail, estr) Then
    '                    Return False
    '                End If
    '            End If

    '            Me.lblMedexDescription.Text = ds_AuditTrail.Tables(0).Rows(0).Item("vMedExDesc").ToString.Trim()
    '            dc_AuditTrailMst = New DataColumn("dModifyOn_IST", System.Type.GetType("System.String"))
    '            ds_AuditTrail.Tables(0).Columns.Add("dModifyOn_IST")
    '            ds_AuditTrail.AcceptChanges()
    '            For Each dr_Audit In ds_AuditTrail.Tables(0).Rows
    '                dr_Audit("dModifyOn_IST") = CDate(dr_Audit("dModifyOn")).ToString("dd-MMM-yyyy HH:mm").Trim() + strServerOffset
    '            Next
    '            ds_AuditTrail.AcceptChanges()
    '            dv_AuditTrail = ds_AuditTrail.Tables(0).DefaultView()
    '            dv_AuditTrail.Sort = "iTranNo desc"

    '            'Me.GVHistoryDtl.DataSource = dv_AuditTrail.ToTable()
    '            'Me.GVHistoryDtl.DataBind()
    '            MedexGroupCode = ds_AuditTrail.Tables(0).Rows(0).Item("vMedExGroupCode").ToString.Trim()

    '            Return True
    '        Catch ex As Exception
    '            Me.ShowErrorMessage(ex.Message, "...fillGrid")
    '            Return False
    '        End Try

    '    End Function

    '#End Region

#Region "fillQCGrid"

    Private Function fillQCGrid() As Boolean
        Dim Ds_QCGrid As New DataSet
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty

        Try
            If Me.rblScreeningDate.Items.Count <= 0 Then
                Return True
            End If

            Wstr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "' And " & _
                   " replace(convert(varchar(11),dScreenDate,113),' ','-')='" & CDate(Me.rblScreeningDate.SelectedValue.Trim()).ToString("dd-MMM-yyyy") & "' and cIsSourceDocComment='N'"

            If Not objHelp.View_MedexScreeningHdrQc(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        Ds_QCGrid, estr) Then
                Me.ShowErrorMessage(estr, estr)
                Return False
            End If
            Me.GVQCDtl.DataSource = Ds_QCGrid
            Me.GVQCDtl.DataBind()
            If Ds_QCGrid.Tables(0).Rows.Count > 0 Then
                'Me.BtnQC.Visible = True
            End If
            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View And _
                Ds_QCGrid.Tables(0).Rows.Count <= 0 Then
                Me.BtnQC.Visible = False
            End If

            If Not IsNothing(Me.Request.QueryString("QC")) AndAlso _
                        (Me.Request.QueryString("QC").ToString.Trim() = "true") Then
                'Me.BtnQC.Visible = True
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

#End Region

#Region "AssignValues_MedExScreeningHdrQC"

    Private Function AssignValues_MedExScreeningHdrQC(ByRef DsSave As DataSet) As Boolean
        Dim DtMedExScreeningHdr As New DataTable
        Dim ObjId As String = String.Empty
        Dim dr As DataRow
        Dim TranNo As Integer = 0
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds_MEdexScreeningHdrQC As New DataSet
        Dim dtMEdexScreeningHdrQC As New DataTable

        Try
            DtMedExScreeningHdr = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable)

            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                If Not objHelp.GetMedexScreeningHdrQC(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_MEdexScreeningHdrQC, estr) Then
                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Return False
                End If

                dtMEdexScreeningHdrQC = ds_MEdexScreeningHdrQC.Tables(0)
                dr = dtMEdexScreeningHdrQC.NewRow
                dr("nMedExScreeningHdrQCNo") = 1
                dr("nMedExScreeningHdrNo") = DtMedExScreeningHdr.Rows(0).Item("nMedExScreeningHdrNo")
                nMedexScreeningHdrNo = DtMedExScreeningHdr.Rows(0).Item("nMedExScreeningHdrNo")
                dr("iTranNo") = 1
                dr("vQCComment") = Me.txtQCRemarks.Value.Trim()
                dr("cQCFlag") = Me.RBLQCFlag.SelectedValue.Trim()
                dr("iModifyBy") = L_UserId
                dr("cStatusIndi") = "N"
                dr("iQCGivenBy") = L_UserId
                dr("dQCGivenOn") = CDate(ObjCommon.GetCurDatetime(L_TimeZoneName))
                dtMEdexScreeningHdrQC.Rows.Add(dr)

            Else
                dtMEdexScreeningHdrQC = Me.ViewState(VS_DtQC)
                For Each dr In dtMEdexScreeningHdrQC.Rows
                    dr("iModifyBy") = L_UserId
                    dr("cStatusIndi") = "E"
                    dr("vResponse") = Me.txtQCRemarks.Value.Trim()
                    dr("iResponseGivenBy") = L_UserId
                    dr("dResponseGivenOn") = CDate(ObjCommon.GetCurDatetime(L_TimeZoneName))
                    dr.AcceptChanges()
                Next dr
            End If

            dtMEdexScreeningHdrQC.AcceptChanges()
            dtMEdexScreeningHdrQC.TableName = "MedExScreeningHdrQC"
            dtMEdexScreeningHdrQC.AcceptChanges()
            DsSave.Tables.Add(dtMEdexScreeningHdrQC.Copy())
            DsSave.AcceptChanges()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

#End Region

    Private Function fillScreeningDates() As Boolean
        Dim ds_AuditTrail As New DataSet
        Dim ds_MedExTrail As New DataSet
        Dim dv_AuditTrail As New DataView
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim selectChk As String = String.Empty
        Try
            Me.ddlScreeningDate.Items.Clear()

            Me.chkScreeningType.Enabled = True
            If Not IsPostBack() Then
                Me.chkScreeningType.ClearSelection()

                If Not Session("IsProjectSpecificScreening") Is Nothing AndAlso Session("IsProjectSpecificScreening").ToString() <> "" AndAlso Session("IsProjectSpecificScreening").ToString() = True Then
                    Me.chkScreeningType.Items(1).Selected = True
                Else
                    Me.chkScreeningType.Items(0).Selected = True
                End If

            End If
            If ContinueMode = 1 Then
                Wstr = "vSubjectId='" & Me.ViewState(VS_SubjectID).ToString & "'"
            Else
                Wstr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "'"
            End If

            If Not Me.HScrNo.Value.Trim = "" Then
                Wstr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "' and nMedexScreeninghdrNo = " & Me.HScrNo.Value.Trim()
            End If

            If Not objHelp.GetData("View_fillScreeningDate", "*", Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                             ds_AuditTrail, estr) Then
                Return False
            End If

            dv_AuditTrail = ds_AuditTrail.Tables(0).DefaultView() '.ToTable(True, "dScreenDate,nMedExScreeningHdrNo,vReviewBy".Split(",")).DefaultView()
            dv_AuditTrail.Sort = "dScreenDate desc"


            Me.rblScreeningDate.Items.Clear()
            For Each dr As DataRow In dv_AuditTrail.ToTable().Rows
                Dim MedexScreening As String = dr("nMedexScreeningHdrNo")
                Dim ds_MaxScreeningLockDtl As DataSet = Nothing
                Dim WstrScreenignLock As String
                If (Request.QueryString("Workspace") <> "0000000000") Then
                    WstrScreenignLock = "vWorkspaceid= '" + Request.QueryString("Workspace") + " ' AND VsubjectId ='" + Request.QueryString("SubId") + "'  and cStatusIndi<>'D' AND nMedexScreeningHdrNo = " + MedexScreening + ""
                Else
                    WstrScreenignLock = "VsubjectId ='" + Request.QueryString("SubId") + "'  and cStatusIndi<>'D' AND nMedexScreeningHdrNo = " + MedexScreening + ""
                End If

                If Not objHelp.View_MaxScreeningLockDtl(WstrScreenignLock, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                 ds_MaxScreeningLockDtl, estr) Then

                    Throw New Exception(estr)
                End If
                Dim Lock As String = "U"
                If ds_MaxScreeningLockDtl.Tables(0).Rows.Count > 0 Then
                    Lock = ds_MaxScreeningLockDtl.Tables(0).Rows(0)(7).ToString
                End If
                If Request.QueryString("ScrDt") = Convert.ToString(dr("dScreenDate")) Then
                    hdnMedExScreenHdrno.Value = dr("nMedExScreeningHdrNo")
                End If

                If (Me.Request.QueryString("Attendance") = "true") Then
                    If (Me.ViewState(VS_SelectedScreening) = "") Then
                        If (Me.Request.QueryString("ScrDt") <> Nothing) Then
                            If ((Format(CDate(Me.Request.QueryString("ScrDt").ToString), "dd-MMM-yyyy")) = (Format(CDate(dr("dScreenDate").ToString), "dd-MMM-yyyy"))) Then
                                If (Lock = "L") Then
                                    Me.rblScreeningDate.Items.Add(New ListItem(Format(CDate(dr("dScreenDate").Date), "dd-MMM-yyyy") + _
                                                                            IIf(Convert.ToString(dr("vProjectNo")).Trim() = "", "", "(Study No: " + Convert.ToString(dr("vProjectNo")) + ")"), _
                                                                           Convert.ToString(dr("dScreenDate"))))
                                    Me.ddlScreeningDate.Items.Add(New ListItem(Format(CDate(dr("dScreenDate").Date), "dd-MMM-yyyy") + _
                                                                            IIf(Convert.ToString(dr("vProjectNo")).Trim() = "", "", "(Study No: " + Convert.ToString(dr("vProjectNo")) + ")"), _
                                                                           Convert.ToString(dr("dScreenDate"))))
                                    If (Me.Request.QueryString("mode") = 4 AndAlso Not Me.Request.QueryString("Attendance") Is Nothing) Then
                                        Exit For
                                    End If
                                Else
                                    Me.rblScreeningDate.Items.Add(New ListItem(Format(CDate(dr("dScreenDate").Date), "dd-MMM-yyyy") + _
                                                                            IIf(Convert.ToString(dr("vProjectNo")).Trim() = "", "", "(Study No: " + Convert.ToString(dr("vProjectNo")) + ")"), _
                                                                            Convert.ToString(dr("dScreenDate"))))

                                    Me.ddlScreeningDate.Items.Add(New ListItem(Format(CDate(dr("dScreenDate").Date), "dd-MMM-yyyy") + _
                                                                            IIf(Convert.ToString(dr("vProjectNo")).Trim() = "", "", "(Study No: " + Convert.ToString(dr("vProjectNo")) + ")"), _
                                                                            Convert.ToString(dr("dScreenDate"))))
                                    If (Me.Request.QueryString("mode") = 4 AndAlso Not Me.Request.QueryString("Attendance") Is Nothing) Then
                                        Exit For
                                    End If
                                End If
                                'If (Me.Request.QueryString("mode") = 4) Then
                                '    Exit For
                                'End If
                            End If
                        Else
                            dv_AuditTrail.RowFilter = "dScreenDate =  MAX(dScreenDate)"
                            If (Lock = "L") Then
                                Me.rblScreeningDate.Items.Add(New ListItem(Format(CDate(dr("dScreenDate").Date), "dd-MMM-yyyy") + _
                                                                        IIf(Convert.ToString(dr("vProjectNo")).Trim() = "", "", "(Study No: " + Convert.ToString(dr("vProjectNo")) + ")"), _
                                                                        Convert.ToString(dr("dScreenDate"))))

                                Me.ddlScreeningDate.Items.Add(New ListItem(Format(CDate(dr("dScreenDate").Date), "dd-MMM-yyyy") + _
                                                                        IIf(Convert.ToString(dr("vProjectNo")).Trim() = "", "", "(Study No: " + Convert.ToString(dr("vProjectNo")) + ")"), _
                                                                        Convert.ToString(dr("dScreenDate"))))
                                If (Me.Request.QueryString("mode") = 4 AndAlso Not Me.Request.QueryString("Attendance") Is Nothing) Then
                                    Exit For
                                End If

                            Else
                                Me.rblScreeningDate.Items.Add(New ListItem(Format(CDate(dr("dScreenDate").Date), "dd-MMM-yyyy") + _
                                                                          IIf(Convert.ToString(dr("vProjectNo")).Trim() = "", "", "(Study No: " + Convert.ToString(dr("vProjectNo")) + ")"), _
                                                                          Convert.ToString(dr("dScreenDate"))))

                                Me.ddlScreeningDate.Items.Add(New ListItem(Format(CDate(dr("dScreenDate").Date), "dd-MMM-yyyy") + _
                                                                          IIf(Convert.ToString(dr("vProjectNo")).Trim() = "", "", "(Study No: " + Convert.ToString(dr("vProjectNo")) + ")"), _
                                                                          Convert.ToString(dr("dScreenDate"))))
                                If (Me.Request.QueryString("mode") = 4 AndAlso Not Me.Request.QueryString("Attendance") Is Nothing) Then
                                    Exit For
                                End If
                            End If


                        End If

                    Else
                        If (Lock = "L") Then
                            Me.rblScreeningDate.Items.Add(New ListItem(Format(CDate(dr("dScreenDate").Date), "dd-MMM-yyyy") + _
                                                                    IIf(Convert.ToString(dr("vProjectNo")).Trim() = "", "", "(Study No: " + Convert.ToString(dr("vProjectNo")) + ")"), _
                                                                    Convert.ToString(dr("dScreenDate"))))

                            Me.ddlScreeningDate.Items.Add(New ListItem(Format(CDate(dr("dScreenDate").Date), "dd-MMM-yyyy") + _
                                                                    IIf(Convert.ToString(dr("vProjectNo")).Trim() = "", "", "(Study No: " + Convert.ToString(dr("vProjectNo")) + ")"), _
                                                                    Convert.ToString(dr("dScreenDate"))))
                            If (Me.Request.QueryString("mode") = 4 AndAlso Not Me.Request.QueryString("Attendance") Is Nothing) Then
                                Exit For
                            End If

                        Else
                            Me.rblScreeningDate.Items.Add(New ListItem(Format(CDate(dr("dScreenDate").Date), "dd-MMM-yyyy") + _
                                                                      IIf(Convert.ToString(dr("vProjectNo")).Trim() = "", "", "(Study No: " + Convert.ToString(dr("vProjectNo")) + ")"), _
                                                                      Convert.ToString(dr("dScreenDate"))))

                            Me.ddlScreeningDate.Items.Add(New ListItem(Format(CDate(dr("dScreenDate").Date), "dd-MMM-yyyy") + _
                                                                      IIf(Convert.ToString(dr("vProjectNo")).Trim() = "", "", "(Study No: " + Convert.ToString(dr("vProjectNo")) + ")"), _
                                                                      Convert.ToString(dr("dScreenDate"))))
                            If (Me.Request.QueryString("mode") = 4 AndAlso Not Me.Request.QueryString("Attendance") Is Nothing) Then
                                Exit For
                            End If
                        End If

                    End If

                ElseIf Not objHelp.ChkLockedScreenDate(dr("nMedExScreeningHdrNo")) AndAlso Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    If (Lock = "L") Then
                        Me.rblScreeningDate.Items.Add(New ListItem(Format(CDate(dr("dScreenDate").Date), "dd-MMM-yyyy") + _
                                                                IIf(Convert.ToString(dr("vProjectNo")).Trim() = "", "", "(Study No: " + Convert.ToString(dr("vProjectNo")) + ")"), _
                                                               Convert.ToString(dr("dScreenDate"))))

                        Me.ddlScreeningDate.Items.Add(New ListItem(Format(CDate(dr("dScreenDate").Date), "dd-MMM-yyyy") + _
                                                                IIf(Convert.ToString(dr("vProjectNo")).Trim() = "", "", "(Study No: " + Convert.ToString(dr("vProjectNo")) + ")"), _
                                                               Convert.ToString(dr("dScreenDate"))))
                        If (Me.Request.QueryString("mode") = 4 AndAlso Not Me.Request.QueryString("Attendance") Is Nothing) Then
                            Exit For
                        End If
                    Else
                        Me.rblScreeningDate.Items.Add(New ListItem(Format(CDate(dr("dScreenDate").Date), "dd-MMM-yyyy") + _
                                                                IIf(Convert.ToString(dr("vProjectNo")).Trim() = "", "", "(Study No: " + Convert.ToString(("vProjectNo")) + ")"), _
                                                               Convert.ToString(dr("dScreenDate"))))

                        Me.ddlScreeningDate.Items.Add(New ListItem(Format(CDate(dr("dScreenDate").Date), "dd-MMM-yyyy") + _
                                                                IIf(Convert.ToString(dr("vProjectNo")).Trim() = "", "", "(Study No: " + Convert.ToString(dr("vProjectNo")) + ")"), _
                                                               Convert.ToString(dr("dScreenDate"))))
                        If (Me.Request.QueryString("mode") = 4 AndAlso Not Me.Request.QueryString("Attendance") Is Nothing) Then
                            Exit For
                        End If


                    End If

                Else
                    If (Request.QueryString("Lock") = "L") Then
                        Me.rblScreeningDate.Items.Add(New ListItem(Format(CDate(dr("dScreenDate").Date), "dd-MMM-yyyy") + _
                                                                      IIf(Convert.ToString(dr("vProjectNo")).Trim() = "", "", "(Study No: " + Convert.ToString(dr("vProjectNo")) + ")"), _
                                                                      Convert.ToString(dr("dScreenDate"))))

                        Me.ddlScreeningDate.Items.Add(New ListItem(Format(CDate(dr("dScreenDate").Date), "dd-MMM-yyyy") + _
                                                                      IIf(Convert.ToString(dr("vProjectNo")).Trim() = "", "", "(Study No: " + Convert.ToString(dr("vProjectNo")) + ")"), _
                                                                      Convert.ToString(dr("dScreenDate"))))
                        If (Me.Request.QueryString("mode") = 4 AndAlso Not Me.Request.QueryString("Attendance") Is Nothing) Then
                            Exit For
                        End If

                    Else
                        Me.rblScreeningDate.Items.Add(New ListItem(Format(CDate(dr("dScreenDate").Date), "dd-MMM-yyyy") + _
                                                                          IIf(Convert.ToString(dr("vProjectNo")).Trim() = "", "", "(Study No: " + Convert.ToString(dr("vProjectNo")) + ")"), _
                                                                          Convert.ToString(dr("dScreenDate"))))

                        Me.ddlScreeningDate.Items.Add(New ListItem(Format(CDate(dr("dScreenDate").Date), "dd-MMM-yyyy") + _
                                                                          IIf(Convert.ToString(dr("vProjectNo")).Trim() = "", "", "(Study No: " + Convert.ToString(dr("vProjectNo")) + ")"), _
                                                                          Convert.ToString(dr("dScreenDate"))))
                        If (Me.Request.QueryString("mode") = 4 AndAlso Not Me.Request.QueryString("Attendance") Is Nothing) Then
                            Exit For
                        End If

                    End If

                    If (ddlProfile.SelectedItem.Text = "QCCo-Ordinator") Then
                        'Me.BtnQC.Style.Add("display", "inline")
                        'Me.BtnQC.Visible = True
                    End If
                    'If (Me.Request.QueryString("mode") = 4 AndAlso Me.Request.QueryString("QC") Is Nothing) Then
                    '    Exit For
                    'End If
                End If

            Next dr

            If (ddlProfile.SelectedItem.Text = "QCCo-Ordinator") Then
                'Me.BtnQC.Style.Add("display", "inline")
                'Me.BtnQC.Visible = True
            End If
            Me.ddlScreeningDate.Items.Insert(0, New ListItem("-- Select Screening Date -- ", "M"))
            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                If Convert.ToString(L_WorkFlowStageId).Trim() <> WorkFlowStageId_SecondReview Then
                    If Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_DataEntry Then
                        Me.rblScreeningDate.Items.Insert(0, New ListItem("New Screening", "N"))
                        Me.ddlScreeningDate.Items.Insert(1, New ListItem("New Screening", "N"))
                    End If

                End If
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...fillScreeningDates")
            Return False
        End Try
    End Function

    Private Function Auntheticate() As Boolean
        Dim pwd As String = String.Empty
        'Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommonP.GetHelpDbTableRef()
        pwd = Me.txtPassword.Text
        pwd = objHelp.EncryptPassword(pwd)

        If L_Password <> pwd.ToString() Then
            ObjCommon.ShowAlert("Password Authentication Fails.", Me.Page)
            Me.txtPassword.Focus()
            Return False
        End If
        Return True
    End Function

    Protected Function Save_ScreenigTmpTable(ByVal dv As DataView, ByRef UserName As String) As Boolean
        Dim eStr As String = String.Empty
        Dim ds_ScreenigTmpTable As New DataSet
        Dim dr_ScreenigTmpTable As DataRow
        Dim dt_ScreenigTmpTable As New DataTable
        Dim ds_ScreenigTmpTableFinal As New DataSet
        Dim drsub As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim estr_retu As String = String.Empty
        Dim UserId As Integer
        Dim ds_UserName As New DataSet
        Dim ds_UserTypeName As New DataSet
        Try
            Wstr = "vSubjectID='" + Me.HSubjectId.Value + "'"
            If Not hMedExNo.Value = "" Then

                If Not objHelp.DeleteScreenigTmpTable(Me.hMedExNo.Value, Me.HSubjectId.Value, ds_ScreenigTmpTable, eStr) Then
                    Return False
                End If
            End If
            Me.hMedExNo.Value = "1"
            If Not rblScreeningDate.SelectedValue.ToString.ToString.Trim() = "N" Then
                dv.RowFilter = "dScreenDate='" + rblScreeningDate.SelectedValue + "'"
                Wstr += " and nMedExScreenNo=" + dv(0)("nMedExScreeningHdrNo").ToString()
                Me.hMedExNo.Value = dv(0)("nMedExScreeningHdrNo").ToString()

            Else
                Wstr += " and nMedExScreenNo=1"
            End If

            If Not objHelp.GetScreenigTmpTable(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                    ds_ScreenigTmpTable, estr_retu) Then
                Return False
            End If
            If ds_ScreenigTmpTable.Tables(0).Rows.Count > 0 Then
                Me.hMedExNo.Value = ""
                UserId = ds_ScreenigTmpTable.Tables(0).Rows(0)("iUserID")
                objHelp.getuserMst("iUserID=" + UserId.ToString(), WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_UserName, eStr)
                UserName = ds_UserName.Tables(0).Rows(0)("vUserName")

                objHelp.getUserTypeMst("vUserTypeCode=" + ds_ScreenigTmpTable.Tables(0).Rows(0)("vUserTypeName").ToString(), WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_UserTypeName, eStr)
                UserName += " ,Profile: " + ds_UserTypeName.Tables(0).Rows(0)("vUserTypeName").ToString()
                Throw New Exception
            End If
            dt_ScreenigTmpTable = ds_ScreenigTmpTable.Tables(0)

            dr_ScreenigTmpTable = dt_ScreenigTmpTable.NewRow()
            dr_ScreenigTmpTable("nMedExScreenNo") = 1
            If Not rblScreeningDate.SelectedValue.ToString.ToString.Trim() = "N" Then
                dr_ScreenigTmpTable("nMedExScreenNo") = dv(0)("nMedExScreeningHdrNo")
            End If

            dr_ScreenigTmpTable("vSubjectID") = Me.HSubjectId.Value
            dr_ScreenigTmpTable("iUserID") = L_UserId
            dr_ScreenigTmpTable("vUserTypeName") = L_UserType
            dr_ScreenigTmpTable("dModifyOn") = DateTime.Now()
            dt_ScreenigTmpTable.Rows.Add(dr_ScreenigTmpTable)
            dt_ScreenigTmpTable.AcceptChanges()
            ds_ScreenigTmpTableFinal.Tables.Add(dt_ScreenigTmpTable.Copy)
            If Not Me.objLambda.Save_ScreenigTmpTable(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_ScreenigTmpTableFinal, L_UserId, eStr) Then
                Throw New Exception
            End If

        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Protected Sub btnSaveRunTime_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveRunTime.Click
        Dim estr As String = String.Empty
        Dim SubjectId As String = String.Empty
        Dim WorkspaceId As String = String.Empty
        Dim ActivityId As String = String.Empty
        Dim NodeId As String = String.Empty
        Dim PeriodId As String = String.Empty
        Dim MySubjectNo As String = String.Empty
        Dim DsSave As New DataSet

        Dim Dir As DirectoryInfo
        Dim Flinfo As FileInfo
        Dim TranNo_Retu As String = String.Empty
        Dim FolderPath As String = String.Empty

        Dim objCollection As ControlCollection
        Dim objControl As Control
        Dim ObjId As String = String.Empty

        Try

            Me.ViewState(VS_Choice) = WS_HelpDbTable.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            If Not Me.objLambda.Save_CRFHdrDtlSubDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, DsSave, False, L_UserId, estr) Then
                Me.ObjCommon.ShowAlert(estr, Me.Page)
                Exit Sub
            Else

                ''objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection) 'Me.PlaceMedEx.Controls 

                '' Added By Ketan for Missing Question
                If txtproject.Text.ToString.Trim() <> "" AndAlso txtSubject.Text.ToString.Trim() <> "" AndAlso ddlScreeningDate.SelectedValue.ToString() = "N" AndAlso Request.QueryString("Group") <> "" Then
                    Dim tempProject As String = txtproject.Text.Split("]")(0).Substring(1, txtproject.Text.Split("]")(0).Length - 1)
                    Dim tempSubject As String = txtSubject.Text.Split("(")(1).Substring(0, txtSubject.Text.Split("(")(1).Length - 1)
                    Dim tempDate As String = ddlScreeningDate.SelectedValue.ToString()
                    Dim tempGroup As String = Request.QueryString("Group")
                    objCollection = CType(Me.Session("PlaceMedEx_" + tempProject + tempSubject + tempDate + tempGroup), ControlCollection)

                ElseIf txtproject.Text.ToString.Trim() <> "" AndAlso txtSubject.Text.ToString.Trim() <> "" AndAlso ddlScreeningDate.SelectedValue.ToString() <> "M" AndAlso Request.QueryString("Group") <> "" Then
                    Dim tempProject As String = txtproject.Text.Split("]")(0).Substring(1, txtproject.Text.Split("]")(0).Length - 1)
                    Dim tempSubject As String = txtSubject.Text.Split("(")(1).Substring(0, txtSubject.Text.Split("(")(1).Length - 1)
                    Dim tempDate As String = ddlScreeningDate.SelectedValue.ToString().Replace("/", "").Split(" ")(0)
                    Dim tempGroup As String = Request.QueryString("Group")
                    objCollection = CType(Me.Session("PlaceMedEx_" + tempProject + tempSubject + tempDate + tempGroup), ControlCollection)
                Else
                    objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection)
                End If


                For Each objControl In objCollection
                    If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.FileUpload" Then
                        Dim filename As String = ""

                        If objControl.ID.ToString.Contains("FU") Then
                            ObjId = objControl.ID.ToString.Replace("FU", "")
                        Else
                            ObjId = objControl.ID.ToString.Trim()
                        End If
                        If Request.Files(objControl.ID).FileName = "" And _
                             Not IsNothing(CType(FindControl("hlnk" + ObjId), HyperLink)) AndAlso _
                             CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl <> "" Then

                            filename = Server.MapPath(CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl)

                            FolderPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail"))
                            FolderPath += WorkspaceId + "/" + NodeId + "/" + SubjectId + "/" + TranNo_Retu + "/"

                            Dir = New DirectoryInfo(FolderPath)
                            If Not Dir.Exists() Then
                                Dir.Create()
                            End If

                            Flinfo = New FileInfo(filename.Trim())
                            Flinfo.CopyTo(FolderPath + Flinfo.Name)

                        ElseIf Request.Files(objControl.ID).FileName <> "" Then
                            FolderPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail"))
                            FolderPath += WorkspaceId + "/" + NodeId + "/" + SubjectId + "/" + TranNo_Retu + "/"

                            Dir = New DirectoryInfo(FolderPath)
                            If Not Dir.Exists() Then
                                Dir.Create()
                            End If

                            filename = FolderPath + Path.GetFileName(Request.Files(objControl.ID).FileName.ToString.Trim())
                            Request.Files(objControl.ID).SaveAs(filename)

                        End If

                    End If
                Next objControl

            End If


            Me.ObjCommon.ShowAlert("Attributes Saved Successfully.", Me.Page)

            Me.Session.Remove("PlaceMedEx")

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "...btnSaveRunTime_Click")
        End Try
    End Sub

    'Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click
    '    'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "AnyDivShowHide('S');", True)
    '    Me.divForEditAttribute.Style.Add("display", "")
    '    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "UnBindClick", "$('#btnEdit').unbind('click');", True)
    '    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "displayBackGround", "$get('ModalBackGround').style.display = 'block';$get('ModalBackGround').style.height = $('#HFHeight').val() + ""px"";$get('ModalBackGround').style.width = $('#HFWidth').val() + ""px""; ", True)
    'End Sub

    'Protected Sub btnSaveRemarksForAttribute_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveRemarksForAttribute.Click
    '    Dim wStr As String = String.Empty
    '    Dim eStr As String = String.Empty
    '    Dim dr As DataRow
    '    Dim ds_ScreeningDtl As New DataSet
    '    Dim ControlId As String
    '    Dim ObjControl As New Control
    '    Dim result_Retu As String = String.Empty
    '    Dim oblControlType As String = String.Empty
    '    Try

    '        If Me.HFScreeningWorkSpaceID.Value.ToString() = "0000000000" Then
    '            wStr = "SELECT * FROM MEDEXSCREENINGDTL WHERE 1=2"
    '            ds_ScreeningDtl = objHelp.GetResultSet(wStr, "MEDEXSCREENINGDTL")
    '        Else

    '        End If


    '        ControlId = Me.hfMedexCode.Value()

    '        If Not Me.GetControlValue(result_Retu, ControlId, ObjControl, oblControlType) Then
    '            Me.ObjCommon.ShowAlert("Problem While Editing Attribute Detail", Me.Page)
    '            Exit Sub
    '        End If

    '        If Me.HFScreeningWorkSpaceID.Value.ToString() = "0000000000" Then
    '            dr = ds_ScreeningDtl.Tables(0).NewRow()
    '            dr("nMedExScreeningHdrNo") = Me.HFScreeningHdrlNo.Value
    '            dr("vMedExCode") = Me.hfMedexCode.Value.ToString()
    '            dr("vMedExResult") = result_Retu
    '            dr("iModifyBy") = Session(S_UserID)
    '            dr("cStatusIndi") = "N"
    '            ds_ScreeningDtl.Tables(0).Rows.Add(dr)
    '            ds_ScreeningDtl.Tables(0).AcceptChanges()

    '            If Not objLambda.Save_MedExScreeningDtlOnly(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_ScreeningDtl, eStr) Then
    '                Me.ShowErrorMessage(eStr, "....Error while saving in MedExScreeningDtl")
    '                Exit Sub
    '            End If
    '        Else

    '        End If
    '        Me.ObjCommon.ShowAlert("Attribute Updated Successfully.", Me.Page)
    '        Me.txtRemarkForAttributeEdit.Text = ""
    '        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "AnyDivShowHide('H');", True)

    '        If Not GenCall() Then
    '            Exit Sub
    '        End If
    '        'Me.HFSessionFlg.Value = "1"


    '    Catch ex As Exception
    '        Me.ShowErrorMessage("Error While Saving Remarks : ", ex.Message)
    '        Me.ObjCommon.ShowAlert("Error While Saving Remarks : " + ex.Message, Me.Page)
    '    End Try
    'End Sub

    Public Function GetJson(ByVal dt As DataTable) As String

        Dim serializer As System.Web.Script.Serialization.JavaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim rows As New List(Of Dictionary(Of String, Object))
        Dim row As Dictionary(Of String, Object)

        For Each dr As DataRow In dt.Rows
            row = New Dictionary(Of String, Object)
            For Each col As DataColumn In dt.Columns
                row.Add(col.ColumnName, dr(col).ToString())
            Next
            rows.Add(row)
        Next
        Return serializer.Serialize(rows)
    End Function

    Protected Sub btnPdf_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdf.Click
        Dim htmlcontent As String = String.Empty
        Dim headercontent As String = String.Empty
        Dim downloadbytes As Byte()
        Dim d1 As Document
        Dim data As String = String.Empty
        Dim watermarkTextFont As System.Drawing.Font
        Dim watermarkTextElement As TextElement
        Dim stylesheetarraylist As New ArrayList
        Dim strProfileStatus As String = String.Empty
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_Check As New DataSet
        Dim pdfFont As System.Drawing.Font
        Try
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "dis", "alert('fired Disable')", True)
            Dim pdfconverter As HtmlToPdfConverter = New HtmlToPdfConverter()
            pdfconverter.PdfDocumentOptions.AvoidImageBreak = True
            pdfconverter.PdfDocumentOptions.AvoidTextBreak = True


            pdfconverter.PdfDocumentOptions.StretchToFit = True
            pdfconverter.LicenseKey = "dfvo+uv66OPj6vrr6PTq+unr9Ovo9OPj4+P66g=="
            pdfconverter.PdfDocumentOptions.TopMargin = 15

            pdfconverter.PdfDocumentOptions.LeftMargin = 20

            pdfconverter.PdfDocumentOptions.JpegCompressionEnabled = False
            pdfconverter.PdfDocumentOptions.JpegCompressionLevel = 0
            pdfconverter.PdfDocumentOptions.ShowHeader = True
            pdfconverter.PdfDocumentOptions.ShowFooter = True

            pdfconverter.PdfHeaderOptions.HeaderHeight = 120

            Dim Path As String = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.ToString.LastIndexOf("/"))
            ImgLogo.Src = Path + ImgLogo.Src.ToString.Replace("~", "")

            headercontent = Regex.Replace(Me.hfHeaderText.Value.ToString(), "<IMG[^>]+", "<IMG id=ImgLogo1 alt=" + ImgLogo.Src.ToString + " align=right src=" + ImgLogo.Src.ToString + " /", RegexOptions.IgnoreCase)
            Me.HdnPrintString.Value = Regex.Replace(Me.HdnPrintString.Value, "disabled=""true""", "").Replace("disabled", "")
            htmlcontent = Me.HdnPrintString.Value.ToString()

            Dim htmlHeader As New HtmlToPdfElement(18.0, 0.0, headercontent, Nothing) '21.0, 0.0,
            pdfconverter.PdfHeaderOptions.AddElement(htmlHeader)
            pdfconverter.PdfHeaderOptions.AddElement(New LineElement(0, 120, pdfconverter.PdfDocumentOptions.PdfPageSize.Width - pdfconverter.PdfDocumentOptions.LeftMargin - pdfconverter.PdfDocumentOptions.RightMargin, 120))
            '======================================================================================
            '==========================================Footer======================================

            pdfFont = New System.Drawing.Font("Tahoma", 8, FontStyle.Bold, GraphicsUnit.Point)
            pdfconverter.PdfFooterOptions.AddElement(New LineElement(0, 0, pdfconverter.PdfDocumentOptions.PdfPageSize.Width - pdfconverter.PdfDocumentOptions.LeftMargin - pdfconverter.PdfDocumentOptions.RightMargin, 0))
            Dim footerText As New TextElement(0, 5, "*This is an Electronically authenticated report.                                                                                                         Page &p; of &P; pages                   ", New Font(New FontFamily("Tahoma"), 8.5, GraphicsUnit.Point))
            footerText.TextAlign = HorizontalTextAlign.Right
            footerText.ForeColor = Color.Black
            footerText.EmbedSysFont = True
            pdfconverter.PdfFooterOptions.AddElement(footerText)
            pdfconverter.PdfFooterOptions.PageNumberingStartIndex = 0
            'pdfconverter.PdfFooterOptions.FooterText = "       *This is an Electronically authenticated report.                                                                                                         Page &p; of &P; pages                   "
            '======================================================================================

            'btnPdf.Enabled = False
            d1 = pdfconverter.ConvertHtmlToPdfDocumentObject(htmlcontent, "")

            If Me.hfWaterMark.Value.ToString <> "0" Then
                watermarkTextFont = New System.Drawing.Font("HelveticaBold", 75, FontStyle.Bold, GraphicsUnit.Point)
                'watermarkTextFont = d1.AddFont(Winnovative.WnvHtmlConvert.PdfDocument.StdFontBaseFamily.HelveticaBold)
                'watermarkTextFont.Size = 75
                watermarkTextElement = New TextElement(100, 400, strProfileStatus + " Draft Copy", watermarkTextFont)
                watermarkTextElement.ForeColor = System.Drawing.Color.Blue
                watermarkTextElement.Opacity = 10
                watermarkTextElement.TextAngle = 45
                For Each pdfPage In d1.Pages
                    pdfPage.AddElement(watermarkTextElement)
                Next
            End If
            ' d1 = pdfconverter.GetPdfDocumentObjectFromHtmlString(htmlcontent)
            downloadbytes = d1.Save()

            Dim response As System.Web.HttpResponse = System.Web.HttpContext.Current.Response
            response.Clear()
            response.ContentType = "application/pdf"
            response.AddHeader("content-disposition", "attachment; filename=pdf1.pdf; size=" & downloadbytes.Length.ToString())
            response.Flush()
            response.BinaryWrite(downloadbytes)
            response.Flush()



        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "...btnPrint_Click")
        End Try
    End Sub

    Protected Sub btnPdfHide_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdfHide.Click
        Dim htmlcontent As String = String.Empty
        Dim headercontent As String = String.Empty
        Dim downloadbytes As Byte()
        Dim d1 As Document
        Dim data As String = String.Empty
        Dim watermarkTextFont As System.Drawing.Font
        Dim watermarkTextElement As TextElement
        Dim stylesheetarraylist As New ArrayList
        Dim strProfileStatus As String = String.Empty
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_Check As New DataSet
        Dim pdfFont As System.Drawing.Font
        Try

            Dim pdfconverter As HtmlToPdfConverter = New HtmlToPdfConverter()
            pdfconverter.PdfDocumentOptions.AvoidImageBreak = True
            pdfconverter.PdfDocumentOptions.AvoidTextBreak = True


            pdfconverter.PdfDocumentOptions.StretchToFit = True
            pdfconverter.LicenseKey = "dfvo+uv66OPj6vrr6PTq+unr9Ovo9OPj4+P66g=="
            pdfconverter.PdfDocumentOptions.TopMargin = 15

            pdfconverter.PdfDocumentOptions.LeftMargin = 20

            pdfconverter.PdfDocumentOptions.JpegCompressionEnabled = False
            pdfconverter.PdfDocumentOptions.JpegCompressionLevel = 0
            pdfconverter.PdfDocumentOptions.ShowHeader = True
            pdfconverter.PdfDocumentOptions.ShowFooter = True

            pdfconverter.PdfHeaderOptions.HeaderHeight = 120

            Dim Path As String = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.ToString.LastIndexOf("/"))
            ImgLogo.Src = Path + ImgLogo.Src.ToString.Replace("~", "")

            headercontent = Regex.Replace(Me.hfHeaderText.Value.ToString(), "<IMG[^>]+", "<IMG id=ImgLogo1 alt=" + ImgLogo.Src.ToString + " align=right src=" + ImgLogo.Src.ToString + " /", RegexOptions.IgnoreCase)
            Me.HdnPrintString.Value = Regex.Replace(Me.HdnPrintString.Value, "disabled=""true""", "").Replace("disabled", "")
            htmlcontent = Me.HdnPrintString.Value.ToString()

            Dim htmlHeader As New HtmlToPdfElement(18.0, 0.0, headercontent, Nothing) '21.0, 0.0,
            pdfconverter.PdfHeaderOptions.AddElement(htmlHeader)
            pdfconverter.PdfHeaderOptions.AddElement(New LineElement(0, 120, pdfconverter.PdfDocumentOptions.PdfPageSize.Width - pdfconverter.PdfDocumentOptions.LeftMargin - pdfconverter.PdfDocumentOptions.RightMargin, 120))
            '======================================================================================
            '==========================================Footer======================================

            pdfFont = New System.Drawing.Font("Tahoma", 8, FontStyle.Bold, GraphicsUnit.Point)
            pdfconverter.PdfFooterOptions.AddElement(New LineElement(0, 0, pdfconverter.PdfDocumentOptions.PdfPageSize.Width - pdfconverter.PdfDocumentOptions.LeftMargin - pdfconverter.PdfDocumentOptions.RightMargin, 0))
            Dim footerText As New TextElement(0, 5, "*This is an Electronically authenticated report.                                                                                                         Page &p; of &P; pages                   ", New Font(New FontFamily("Tahoma"), 8.5, GraphicsUnit.Point))
            footerText.TextAlign = HorizontalTextAlign.Right
            footerText.ForeColor = Color.Black
            footerText.EmbedSysFont = True
            pdfconverter.PdfFooterOptions.AddElement(footerText)
            pdfconverter.PdfFooterOptions.PageNumberingStartIndex = 0
            'pdfconverter.PdfFooterOptions.FooterText = "       *This is an Electronically authenticated report.                                                                                                         Page &p; of &P; pages                   "
            '======================================================================================

            'btnPdf.Enabled = False
            d1 = pdfconverter.ConvertHtmlToPdfDocumentObject(htmlcontent, "")

            If Me.hfWaterMark.Value.ToString <> "0" Then
                watermarkTextFont = New System.Drawing.Font("HelveticaBold", 75, FontStyle.Bold, GraphicsUnit.Point)
                'watermarkTextFont = d1.AddFont(Winnovative.WnvHtmlConvert.PdfDocument.StdFontBaseFamily.HelveticaBold)
                'watermarkTextFont.Size = 75
                watermarkTextElement = New TextElement(100, 400, strProfileStatus + " Draft Copy", watermarkTextFont)
                watermarkTextElement.ForeColor = System.Drawing.Color.Blue
                watermarkTextElement.Opacity = 10
                watermarkTextElement.TextAngle = 45
                For Each pdfPage In d1.Pages
                    pdfPage.AddElement(watermarkTextElement)
                Next
            End If
            ' d1 = pdfconverter.GetPdfDocumentObjectFromHtmlString(htmlcontent)
            downloadbytes = d1.Save()

            Dim response As System.Web.HttpResponse = System.Web.HttpContext.Current.Response
            response.Clear()
            response.ContentType = "application/pdf"
            response.AddHeader("content-disposition", "attachment; filename=pdf1.pdf; size=" & downloadbytes.Length.ToString())
            response.Flush()
            response.BinaryWrite(downloadbytes)
            response.Flush()
            'response.End()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "...btnPrint_Click")
        End Try
    End Sub

    Private Function ValidateWorkflow(ByVal mode As String) As Boolean
        Dim ds_WorkFlow As New DataSet
        Dim ds_SubMedExMst As New DataSet
        Dim estr As String = String.Empty
        Dim dr As DataRow
        Dim ds_Hdr As New DataSet
        Dim ds_Work As New DataSet
        Try
            If Not Auntheticate() Then
                Exit Function
            End If

            If mode = "Auth" Then
                If Not objHelp.GetMedExScreeningHdr("nMedExScreeningHdrNo=" & CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows(0)("nMedExScreeningHdrNo"), _
                                                                  WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                          ds_SubMedExMst, estr) Then
                    Exit Function
                End If

                ds_WorkFlow = objHelp.GetResultSet("SELECT * FROM ScreeningWorkFlowDtl WHERE 1=2", "ScreeningWorkFlowDtl")



                If Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_DataEntry Then
                    If Not ds_WorkFlow Is Nothing Then
                        dr = ds_WorkFlow.Tables(0).NewRow()
                        dr("nMedExScreeningHdrNo") = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows(0)("nMedExScreeningHdrNo").ToString()
                        nMedexScreeningHdrNo = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows(0)("nMedExScreeningHdrNo").ToString()
                        dr("iWorkFlowStageId") = L_WorkFlowStageId.ToString()
                        dr("iModifyBy") = L_UserId
                        dr("cStatusIndi") = "N"
                        dr("vRemark") = Me.txtPiRemarks.Text.Trim()
                        If L_WorkFlowStageId.ToString().ToString() = 0 Then
                            dr("cReviewType") = "E"
                        End If
                        dr("cIsEligible") = Me.hdnchkval.Value.ToString()
                        ds_WorkFlow.Tables(0).Rows.Add(dr)
                        ds_WorkFlow.Tables(0).AcceptChanges()
                    End If
                    For Each dr In ds_SubMedExMst.Tables(0).Rows
                        dr("cIsEligible") = Me.hdnchkval.Value.ToString()
                        dr("iModifyBy") = L_UserId
                        ds_SubMedExMst.Tables(0).AcceptChanges()
                    Next
                    If Not Me.objLambda.Save_MedExScreeningHdrOnly(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_SubMedExMst, estr) Then
                        Me.ObjCommon.ShowAlert(estr, Me.Page)
                        Exit Function
                    End If
                ElseIf Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_SecondReview AndAlso Me.chkEligibleReview.Style("display") = "" Then
                    ds_Work = objHelp.GetResultSet("SELECT * FROM ScreeningWorkflowDtl where nMedExScreeningHdrNo= '" & CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows(0)("nMedExScreeningHdrNo").ToString().Trim() + "'", "ScreeningWorkFlowDtl")
                    If Not ds_WorkFlow Is Nothing Then
                        For index As Integer = 1 To 1
                            dr = ds_WorkFlow.Tables(0).NewRow()
                            dr("nMedExScreeningHdrNo") = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows(0)("nMedExScreeningHdrNo").ToString()
                            dr("iWorkFlowStageId") = L_WorkFlowStageId.ToString()
                            dr("iModifyBy") = L_UserId
                            dr("cStatusIndi") = "N"
                            dr("vRemark") = Me.txtPiRemarks.Text.Trim()
                            dr("cReviewType") = "P"
                            dr("cIsEligible") = Me.hdnchkval.Value.ToString()
                            ds_WorkFlow.Tables(0).Rows.Add(dr)
                        Next
                        ds_WorkFlow.Tables(0).AcceptChanges()
                    End If
                    For Each dr In ds_SubMedExMst.Tables(0).Rows
                        dr("cIsEligible") = Me.hdnchkval.Value.ToString()
                        dr("iModifyBy") = L_UserId
                        ds_SubMedExMst.Tables(0).AcceptChanges()
                    Next
                    If Not Me.objLambda.Save_MedExScreeningHdrOnly(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_SubMedExMst, estr) Then
                        Me.ObjCommon.ShowAlert(estr, Me.Page)
                        Exit Function
                    End If
                ElseIf Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_SecondReview AndAlso Me.chkReviewCompleted.Style("display") = "" Then
                    If Not ds_WorkFlow Is Nothing Then
                        dr = ds_WorkFlow.Tables(0).NewRow()
                        dr("nMedExScreeningHdrNo") = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows(0)("nMedExScreeningHdrNo").ToString()
                        dr("iWorkFlowStageId") = L_WorkFlowStageId.ToString()
                        dr("iModifyBy") = L_UserId
                        dr("cStatusIndi") = "N"
                        dr("vRemark") = Me.txtPiRemarks.Text.Trim()
                        dr("cReviewType") = "P"
                        dr("cIsEligible") = Me.hdnchkval.Value.ToString()
                        ds_WorkFlow.Tables(0).Rows.Add(dr)
                        ds_WorkFlow.Tables(0).AcceptChanges()
                    End If
                End If

                If Not Me.objLambda.Save_ScreeningWorkflow(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_WorkFlow, estr) Then
                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Exit Function
                End If
            ElseIf mode = "Save" Then
                ds_Hdr = objHelp.GetResultSet("SELECT TOP 1 * FROM MEDEXSCREENINGHDR WHERE vSubjectId = '" + Me.HSubjectId.Value + "'ORDER BY dModifyOn DESC", "MedexScreeningHDr")


                If Not objHelp.GetMedExScreeningHdr("nMedExScreeningHdrNo=" & ds_Hdr.Tables(0).Rows(0)("nMedExScreeningHdrNo"), _
                                                                  WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                          ds_SubMedExMst, estr) Then
                    Exit Function
                End If

                ds_WorkFlow = objHelp.GetResultSet("SELECT * FROM ScreeningWorkFlowDtl WHERE 1=2", "ScreeningWorkFlowDtl")
                If Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_DataEntry Then
                    If Not ds_WorkFlow Is Nothing Then
                        dr = ds_WorkFlow.Tables(0).NewRow()
                        dr("nMedExScreeningHdrNo") = ds_Hdr.Tables(0).Rows(0)("nMedExScreeningHdrNo")
                        dr("iWorkFlowStageId") = L_WorkFlowStageId.ToString()
                        dr("iModifyBy") = L_UserId
                        dr("cStatusIndi") = "N"
                        dr("vRemark") = Me.txtPiRemarks.Text.Trim()
                        dr("cReviewType") = "E"
                        dr("cIsEligible") = Me.hdnchkval.Value.ToString()
                        ds_WorkFlow.Tables(0).Rows.Add(dr)
                        ds_WorkFlow.Tables(0).AcceptChanges()
                    End If
                    For Each dr In ds_SubMedExMst.Tables(0).Rows
                        dr("cIsEligible") = Me.hdnchkval.Value.ToString()
                        dr("iModifyBy") = L_UserId
                        ds_SubMedExMst.Tables(0).AcceptChanges()
                    Next
                    If Not Me.objLambda.Save_MedExScreeningHdrOnly(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_SubMedExMst, estr) Then
                        Me.ObjCommon.ShowAlert(estr, Me.Page)
                        Exit Function
                    End If
                ElseIf Convert.ToString(L_WorkFlowStageId).Trim() = WorkFlowStageId_SecondReview Then
                    If Not ds_WorkFlow Is Nothing Then
                        For index As Integer = 1 To 2
                            dr = ds_WorkFlow.Tables(0).NewRow()
                            dr("nMedExScreeningHdrNo") = ds_Hdr.Tables(0).Rows(0)("nMedExScreeningHdrNo")
                            dr("iWorkFlowStageId") = L_WorkFlowStageId.ToString()
                            dr("iModifyBy") = L_UserId
                            dr("cStatusIndi") = "N"
                            If index = 1 Then
                                dr("vRemark") = Me.txtPiRemarks.Text.Trim()
                                dr("cReviewType") = "E"
                                dr("cIsEligible") = Me.hdnchkval.Value.ToString()
                            Else
                                dr("vRemark") = Me.txtPiRemarks.Text.Trim()
                                dr("cReviewType") = "P"
                                dr("cIsEligible") = ""
                            End If
                            ds_WorkFlow.Tables(0).Rows.Add(dr)
                        Next
                        ds_WorkFlow.Tables(0).AcceptChanges()
                    End If

                    For Each dr In ds_SubMedExMst.Tables(0).Rows
                        dr("cIsEligible") = Me.hdnchkval.Value.ToString()
                        dr("iModifyBy") = L_UserId
                        ds_SubMedExMst.Tables(0).AcceptChanges()
                    Next
                    If Not Me.objLambda.Save_MedExScreeningHdrOnly(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_SubMedExMst, estr) Then
                        Me.ObjCommon.ShowAlert(estr, Me.Page)
                        Exit Function
                    End If
                End If
                If Not Me.objLambda.Save_ScreeningWorkflow(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_WorkFlow, estr) Then
                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Exit Function
                End If
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Protected Sub btnRedirect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRedirect.Click
        Try
            FolderPath()
            If Not ValidateUserWiseScreeningEntry("REMOVEENTRY", Me.HSubjectId.Value, rblScreeningDate.SelectedValue) Then
                'ObjCommon.ShowAlert("Another User was working on this screening Date of this subjectid", Me.Page)
                For Each li As ListItem In rblScreeningDate.Items
                    li.Selected = False
                Next
                Session("ScreeningGroup") = ""
                GroupValidation = True
                Dim url As String = Request.Url.ToString()
                url = url.Substring(0, url.IndexOf("Group"))
                Response.Redirect(url, False)
                Exit Sub
            End If
            If Me.Request.QueryString("mode") = 4 Then
                If Not IsNothing(Me.Request.QueryString("QC")) AndAlso _
                (Me.Request.QueryString("QC").ToString.Trim() = "true") Then
                    Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&QC=true&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Me.HSubjectId.Value() + "" + "&Group=" + Session("ScreeningGroup").ToString(), False)
                Else
                    Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Me.HSubjectId.Value() + "&Group = " + Session("ScreeningGroup").ToString(), False)
                End If
            Else
                Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&set=true&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Me.HSubjectId.Value() + "&Group=" + Session("ScreeningGroup").ToString(), False)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "........BtnRedirect")
        End Try
    End Sub

    Protected Sub ddlProfile_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProfile.SelectedIndexChanged
        Dim ds_Profiles As New DataSet
        Dim ds_Workflow As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        'Dim SelectedProfile As String = String.Empty
        Dim ScreeningDate As String = String.Empty
        Dim Subjectdetails As String = String.Empty
        Try
            profileChange = True
            If rblScreeningDate.SelectedValue.ToString.ToString.Trim() = "N" Then
                ScreeningDate = Nothing
            Else
                ScreeningDate = rblScreeningDate.SelectedValue.ToString.ToString.Trim()
            End If

            ContinueMode = 1
            Me.ViewState(VS_ContinueMode) = ContinueMode

            Subjectdetails = Me.txtSubject.Text.ToString
            Me.ViewState(VS_ProjectNo) = Me.HProjectId.Value
            Me.ViewState(VS_SubjectID) = Me.HSubjectId.Value
            PlaceMedEx.Controls.Clear()
            ShowHideControls("H")
            If Not fillScreeningDates() Then
                Throw New Exception("Error While Fill Screening Dates")
            End If
            FillProfile()
            'Me.ViewState(VS_ContinueMode) = Nothing
            Me.txtSubject.Text = Subjectdetails
            Me.HSubjectId.Value = Me.ViewState(VS_SubjectID).ToString.Trim()
            Me.HProjectId.Value = Me.ViewState(VS_ProjectNo).ToString.Trim()

            If ScreeningDate Is Nothing Then
                rblScreeningDate.SelectedValue = rblScreeningDate.Items(1).Value
            Else
                If ScreeningDate <> "" Then
                    rblScreeningDate.SelectedValue = ScreeningDate
                End If
            End If
            If Me.HSubjectId.Value <> "" Then
                If Not ValidateUserWiseScreeningEntry("CONTINUE", Me.HSubjectId.Value, rblScreeningDate.SelectedValue) Then
                    'ObjCommon.ShowAlert("Another User was working on this screening Date of this subjectid", Me.Page)
                    For Each li As ListItem In rblScreeningDate.Items
                        li.Selected = False
                    Next
                    Session("ScreeningGroup") = ""
                    GroupValidation = True
                    Dim url As String = Request.Url.ToString()
                    url = url.Substring(0, url.IndexOf("Group"))
                    Response.Redirect(url, False)
                    Exit Sub
                End If
            End If
            Me.ddlProfile.Items.FindByValue(Me.Session(S_ScrProfileIndex)).Selected = True
            rblScreeningDate_SelectedIndexChanged(sender, e)
            Me.ViewState(VS_ContinueMode) = Nothing
            IsProfileChange = True
            Page_Load(Nothing, Nothing)
            'Response.Redirect("~/frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000")
            'SuccessMsgFlag = False
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "........ddlProfile_SelectedIndexChanged")
        End Try
    End Sub

#End Region

#Region "GetButtons"
    Private Function GetAutoCalculateButton(ByVal vButtonName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal MedExFormula As String, ByVal vMedexType As String, ByVal datastatus As String, ByVal iDecimalNo As String) As Button
        Dim wStr As String = String.Empty
        Dim Ds_Decimal As DataSet
        Dim eStr As String = String.Empty
        Dim DecimalNo As String = String.Empty
        Dim btn As Button
        btn = New Button
        btn.ID = "btnAutoCalculate" & MedExGroupCode + MedExSubGroupCode + MedExCode
        btn.Text = vButtonName.Trim()
        btn.CssClass = "button"
        If iDecimalNo = "" Then
            wStr = "vmedexcode='" & MedExCode & "'"
            If Not objHelp.GetData("medexformulamst", "*", wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                Ds_Decimal, eStr) Then
                Me.ObjCommon.ShowAlert("error while getting data from medexformulamst : " + eStr, Me.Page)
            End If
            iDecimalNo = Ds_Decimal.Tables(0).Rows(0)("idecimalno").ToString()
            Me.HFDecimalNo.Value = DecimalNo
        End If
        Me.HFDecimalNo.Value = iDecimalNo
        btn.OnClientClick = "return MedExFormula('" + MedExCode + "','" + MedExFormula + "','" + iDecimalNo + "');"
        GetAutoCalculateButton = btn
    End Function

    Private Function GetEditButton(ByVal vButtonName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal ScreeningDtlNo As String, ByVal Workspaceid As String, ByVal ScreeningHdrNo As String, ByVal MedexType As String) As System.Web.UI.WebControls.Image
        Dim btn As System.Web.UI.WebControls.Image

        btn = New System.Web.UI.WebControls.Image
        btn.ID = "btnEdit" & MedExGroupCode + MedExSubGroupCode + MedExCode
        btn.ToolTip = vButtonName.Trim()


        If Convert.ToString(L_WorkFlowStageId).Trim() <> WorkFlowStageId_FirstReview AndAlso _
                    Convert.ToString(L_WorkFlowStageId).Trim() <> WorkFlowStageId_DataEntry AndAlso _
                    Convert.ToString(L_WorkFlowStageId).Trim() <> WorkFlowStageId_MedicalCoding AndAlso _
                    Convert.ToString(L_WorkFlowStageId).Trim() <> WorkFlowStageId_SecondReview AndAlso _
                    Convert.ToString(L_WorkFlowStageId).Trim() <> WorkFlowStageId_DataValidator Then
            btn.Attributes.Add("disabled", "true")
        End If

        btn.Attributes.Add("onClick", "return AuditDivShowHide('E','" + MedExCode + "','" + MedExGroupCode + MedExSubGroupCode + MedExCode + "','" + ScreeningDtlNo + "','" + Workspaceid + "','" + ScreeningHdrNo + "','" + MedexType + "');")
        'btn.ImageUrl = "images/Edit2.gif"
        btn.ImageUrl = "images/Edit_Small.png"

        GetEditButton = btn
    End Function

    Private Function GetDCFButton(ByVal vButtonName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal ScreeningDtlNo As String, ByVal Workspaceid As String, ByVal ScreeningHdrNo As String, ByVal MedexType As String, ByVal SccreeningDCFNo As Integer, ByVal DCFSTATUS As String, ByVal Desc As String, ByVal DCFBY As String, ByVal DCFiWorkflowStageId As String) As ImageButton
        Dim btn As ImageButton
        Try
            btn = New ImageButton
            btn.ID = "btnDCF" & MedExGroupCode + MedExSubGroupCode + MedExCode
            btn.ToolTip = vButtonName.Trim()

            If DCFSTATUS = "N" Then
                btn.SkinID = "ImgBtnDCFUpdated"
            ElseIf DCFSTATUS = "N" And L_WorkFlowStageId = 0 Then
                btn.SkinID = "ImgBtnDCFUpdated"
            ElseIf DCFSTATUS <> " " And DCFSTATUS <> "R" And Convert.ToString(ConfigurationManager.AppSettings("ScreeningIndependentReviewer")).Contains(L_UserType.ToString()) Then
                btn.SkinID = "ImgBtnDCFUpdated"
            ElseIf DCFSTATUS = "A" And DCFiWorkflowStageId = L_WorkFlowStageId Then
                btn.SkinID = "ImgBtnDCFUpdated"
            ElseIf DCFSTATUS = "N" And DCFiWorkflowStageId = L_WorkFlowStageId Then
                btn.SkinID = "ImgBtnDCFUpdated"
            Else
                btn.SkinID = "ImgBtnDCF"
            End If

            If Desc <> "" Then
                If Desc.Length > 31 Then
                    Desc = Convert.ToString(Desc).Substring(0, 30) + "..."
                End If
            End If
            Desc = Desc.Replace("'", "\'")    '' add by Ketan Muliya for Handle Special Chacter
            Desc = Desc.Replace("<", "&lt;")
            If Not Request.QueryString("Attendance") Is Nothing AndAlso Request.QueryString("Attendance") = True Then
                btn.OnClientClick = "DCFSHOWHIDE('D','" + MedExCode + "','" + MedExGroupCode + MedExSubGroupCode + MedExCode + "','" + ScreeningDtlNo + "','" + Workspaceid + "','" + ScreeningHdrNo + "','" + MedexType + "', '" + IIf(txtproject.Text = "", "0000000000", HProjectId.Value) + "','" + Desc + "'); return false;"
                btn.ImageUrl = "images/Edit_Small.png"

            Else

                btn.OnClientClick = "DCFSHOWHIDE('D','" + MedExCode + "','" + MedExGroupCode + MedExSubGroupCode + MedExCode + "','" + ScreeningDtlNo + "','" + Workspaceid + "','" + ScreeningHdrNo + "','" + MedexType + "', '" + IIf(Me.HProjectId.Value = "", "0000000000", HProjectId.Value) + "','" + Desc + "'); return false;"
                btn.ImageUrl = "images/Edit_Small.png"
            End If


            GetDCFButton = btn

        Catch ex As Exception
        End Try
    End Function

    Private Function GetAudittrailButton(ByVal vButtonName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal ScreeningType As String, ByVal TranNo As String, ByVal Remarks As String, ByVal LocationCode As String) As ImageButton
        Dim btn As ImageButton
        btn = New ImageButton
        btn.ID = "btnAudittrail" & MedExGroupCode + MedExSubGroupCode + MedExCode
        btn.ToolTip = vButtonName.Trim()

        btn.OnClientClick = "return HistoryDivShowHide('S','" + MedExCode + "','','','" + ScreeningType + "','" + LocationCode + "');"
        btn.SkinID = "ImgBtnAuditTrail"

        If TranNo > 1 And Remarks.Length > 0 Then
            btn.SkinID = "imgbtnaudittrailupdated"
        End If

        GetAudittrailButton = btn
    End Function


    Private Function GetUpdateButton(ByVal vButtonName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal ScreeningDtlNo As String, ByVal Workspaceid As String, ByVal ScreeningHdrNo As String, ByVal MedexType As String) As System.Web.UI.WebControls.Image
        Dim btn As System.Web.UI.WebControls.Image
        btn = New System.Web.UI.WebControls.Image
        btn.ID = "btnUpdate" & MedExGroupCode + MedExSubGroupCode + MedExCode
        btn.ToolTip = vButtonName.Trim()
        btn.Attributes.Add("disabled", "true")
        btn.Attributes.Add("onClick", "return AuditDivShowHide('U','" + MedExCode + "','" + MedExGroupCode + MedExSubGroupCode + MedExCode + "','" + ScreeningDtlNo + "','" + Workspaceid + "','" + ScreeningHdrNo + "','" + MedexType + "');")
        btn.ImageUrl = "Images/Update_Small.png"
        GetUpdateButton = btn
    End Function

    Private Function GetWeightButton() As Button
        Dim ScreenDate As String = rblScreeningDate.SelectedValue.ToString.ToString.Trim()
        Dim Btnweight As New Button
        Btnweight.Text = "Get Weight"
        Btnweight.ID = "Btnweight"
        Btnweight.CssClass = "btn btnadd"
        Btnweight.OnClientClick = "return GetWeightData('" & Convert.ToString(ConfigurationManager.AppSettings("Medex_Height")).Trim() & "','" & Convert.ToString(ConfigurationManager.AppSettings("Medex_Weight")).Trim() & "','" & Convert.ToString(ConfigurationManager.AppSettings("Medex_BMI")).Trim() & "');"

        If ScreenDate = "N" Then
            Btnweight.Enabled = True
        ElseIf Convert.ToDateTime(ScreenDate).ToString("dd/MM/yyyy") = objHelp.GetServerDateTime().ToString("dd/MM/yyyy") Then
            Btnweight.Enabled = True
        Else
            Btnweight.Enabled = False
        End If
        GetWeightButton = Btnweight
    End Function

#End Region

#Region "DropDown List Event"
    Protected Sub btnDropDownSelectedIndexChanged_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDropDownSelectedIndexChanged.Click

    End Sub

    Protected Sub ddlGroup_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim Subjedctid As String = String.Empty
        Subjedctid = Me.HSubjectId.Value.Trim()
        IsSaved = False

        If Me.Request.QueryString("mode") = 4 Then
            If Not IsNothing(Me.Request.QueryString("QC")) AndAlso _
            (Me.Request.QueryString("QC").ToString.Trim() = "true") Then
                Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&QC=true&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Subjedctid + "&Group=" + IIf(Session("ScreeningGroup").ToString() = "00000", "", Session("ScreeningGroup").ToString()), False)
            ElseIf Not IsNothing(Me.Request.QueryString("ScrHdrNo")) AndAlso _
                (Me.Request.QueryString("ScrHdrNo").ToString.Trim() <> "") Then
                Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=" + Me.HProjectId.Value + "&ScrHdrNo= " + Me.HScrNo.Value + "&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Subjedctid + "&Group=" + IIf(Session("ScreeningGroup").ToString() = "00000", "", Session("ScreeningGroup").ToString()), False)
            ElseIf Not IsNothing(Me.Request.QueryString("Attendance")) AndAlso _
                    (Me.Request.QueryString("Attendance").ToString.Trim() = "true") Then

                If Not (Me.Request.QueryString("ScrDt") Is Nothing) Then
                    Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=" + Me.Request.QueryString("Workspace").ToString() + "&ScrDt=" + Me.Request.QueryString("ScrDt").ToString() + "&SubId=" + Subjedctid + "&Attendance=true&Group=" + IIf(Session("ScreeningGroup").ToString() = "00000", "", Session("ScreeningGroup").ToString()), False)
                Else
                    Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Subjedctid + "&Attendance=true&Group=" + IIf(Session("ScreeningGroup").ToString() = "00000", "", Session("ScreeningGroup").ToString()), False)
                End If

            Else
                Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Subjedctid + "&Group=" + IIf(Session("ScreeningGroup").ToString() = "00000", "", Session("ScreeningGroup").ToString()), False)
            End If
        ElseIf Me.ViewState(VS_SaveMode) = 1 Then

            Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&Set=true&ScrDt=" + Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() + "&SubId=" + Subjedctid + "&Save=true", False)
        Else

            If hdnSubGroup.Value <> "" Then
                Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&Set=true&ScrDt=" + IIf(Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() = "", "N", Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim()) + "&SubId=" + Subjedctid + "&Group=" + IIf(Session("ScreeningGroup").ToString() = "00000", "", Session("ScreeningGroup").ToString()), False)
            Else
                Me.Response.Redirect("frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&Set=true&ScrDt=" + IIf(Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim() = "", "N", Me.rblScreeningDate.SelectedValue.ToString.ToString.Trim()) + "&SubId=" + Subjedctid + "&Group=" + IIf(Session("ScreeningGroup").ToString() = "00000", "", Session("ScreeningGroup").ToString()), False)
            End If

        End If
    End Sub

#End Region

#Region "Formula Related Functions"
    Private Function GetControlValue(ByRef result As String, ByVal ControlId As String, ByRef objControl_Retu As Control, ByRef objControlType As String) As Boolean
        Dim objCollection As ControlCollection
        Dim objControl As Control
        Dim ObjId As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim dat As New Date
        Dim dt As DataTable = Nothing
        Dim flg As Boolean = False

        Try

            ''objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection)

            '' Added By Ketan for Missing Question
            If txtproject.Text.ToString.Trim() <> "" AndAlso txtSubject.Text.ToString.Trim() <> "" AndAlso ddlScreeningDate.SelectedValue.ToString() = "N" AndAlso Request.QueryString("Group") <> "" Then
                Dim tempProject As String = txtproject.Text.Split("]")(0).Substring(1, txtproject.Text.Split("]")(0).Length - 1)
                Dim tempSubject As String = txtSubject.Text.Split("(")(1).Substring(0, txtSubject.Text.Split("(")(1).Length - 1)
                Dim tempDate As String = ddlScreeningDate.SelectedValue.ToString()
                Dim tempGroup As String = Request.QueryString("Group")
                objCollection = CType(Me.Session("PlaceMedEx_" + tempProject + tempSubject + tempDate + tempGroup), ControlCollection)

            ElseIf txtproject.Text.ToString.Trim() <> "" AndAlso txtSubject.Text.ToString.Trim() <> "" AndAlso ddlScreeningDate.SelectedValue.ToString() <> "M" AndAlso Request.QueryString("Group") <> "" Then
                Dim tempProject As String = txtproject.Text.Split("]")(0).Substring(1, txtproject.Text.Split("]")(0).Length - 1)
                Dim tempSubject As String = txtSubject.Text.Split("(")(1).Substring(0, txtSubject.Text.Split("(")(1).Length - 1)
                Dim tempDate As String = ddlScreeningDate.SelectedValue.ToString().Replace("/", "").Split(" ")(0)
                Dim tempGroup As String = Request.QueryString("Group")
                objCollection = CType(Me.Session("PlaceMedEx_" + tempProject + tempSubject + tempDate + tempGroup), ControlCollection)
            Else
                objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection)
            End If


            For Each objControl In objCollection

                If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.TextBox" Then
                    If objControl.ID.ToString.Contains("txt") Then
                        ObjId = objControl.ID.ToString.Replace("txt", "")
                    Else
                        ObjId = objControl.ID.ToString.Trim()
                    End If
                    If ObjId = ControlId.Trim.ToUpper() Then
                        If objControl.ID.ToString.Contains("txt") Then
                            ObjId = objControl.ID.ToString.Replace("txt", "")
                        Else
                            ObjId = objControl.ID.ToString.Trim()
                        End If
                        result = Request.Form(objControl.ID)
                        dt = CType(ViewState(VS_dtMedEx_Fill), DataTable)
                        dt.DefaultView().RowFilter = "vMedexCode='" & objControl.ID.ToString.Trim() & "'"
                        dt.DefaultView.ToTable()
                        objControlType = dt.DefaultView.ToTable().Rows(0).Item("vMedexType")
                        objControl_Retu = objControl
                        Return True
                    End If

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" Then
                    ObjId = objControl.ID.ToString.Trim()
                    If DirectCast(objControl, System.Web.UI.WebControls.DropDownList).Attributes("StandardDate") = "Y" Then
                        If result = "" Then
                            For Each objControl1 In objCollection
                                If Not objControl1.ID Is Nothing AndAlso objControl1.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" AndAlso objControl1.ID.ToString.Trim().Substring(0, objControl1.ID.ToString.Trim().IndexOf("_")) = ObjId.Substring(0, ObjId.IndexOf("_")).Trim() Then
                                    If Request.Form(objControl1.ID) = "" Then
                                        flg = True
                                    Else
                                        result += Request.Form(objControl1.ID)
                                    End If
                                    objControl_Retu = objControl1
                                End If
                            Next
                            result = IIf(flg = True, "", result)
                        End If
                    Else
                        If ObjId = ControlId.Trim.ToUpper() Then
                            result = Request.Form(objControl.ID)
                            objControl_Retu = objControl
                            Return True
                        End If
                    End If

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.RadioButtonList" Then
                    ObjId = objControl.ID.ToString.Trim()
                    If ObjId = ControlId.Trim.ToUpper() Then
                        Dim rbl As RadioButtonList = CType(FindControl(objControl.ID), RadioButtonList)
                        Dim StrChk As String = String.Empty

                        For index As Integer = 0 To rbl.Items.Count - 1
                            StrChk += IIf(rbl.Items(index).Selected, rbl.Items(index).Value.ToString.Trim() + ",", "")
                        Next index

                        If StrChk.Trim() <> "" Then
                            StrChk = StrChk.Substring(0, StrChk.LastIndexOf(","))
                        End If
                        result = StrChk
                        objControl_Retu = objControl
                        Return True
                    End If

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.CheckBoxList" Then
                    ObjId = objControl.ID.ToString.Trim()
                    If ObjId = ControlId.Trim.ToUpper() Then
                        Dim chkl As CheckBoxList = CType(FindControl(objControl.ID), CheckBoxList)
                        Dim StrChk As String = String.Empty

                        For index As Integer = 0 To chkl.Items.Count - 1
                            StrChk += IIf(chkl.Items(index).Selected, chkl.Items(index).Value.ToString.Trim() + ",", "")
                        Next index

                        If StrChk.Trim() <> "" Then
                            StrChk = StrChk.Substring(0, StrChk.LastIndexOf(","))
                        End If
                        result = StrChk
                        objControl_Retu = objControl
                        Return True
                    End If
                End If

            Next objControl
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GetControlValue")
            Return False
        End Try
    End Function


#End Region

#Region "FilePath"
    Private Function FolderPath() As Boolean
        Dim objCollection As ControlCollection
        Dim filepath As String = String.Empty
        Dim objControl As Control
        Dim ObjId As String = String.Empty
        Dim filename As String = String.Empty
        Dim Dir As DirectoryInfo
        Dim TranNo_Retu As String
        Try
            TranNo_Retu = Request.Form("__EVENTARGUMENT")
            '' objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection) 'Me.PlaceMedEx.Controls 

            '' Added By Ketan for Missing Question
            If txtproject.Text.ToString.Trim() <> "" AndAlso txtSubject.Text.ToString.Trim() <> "" AndAlso ddlScreeningDate.SelectedValue.ToString() = "N" AndAlso Request.QueryString("Group") <> "" Then
                Dim tempProject As String = txtproject.Text.Split("]")(0).Substring(1, txtproject.Text.Split("]")(0).Length - 1)
                Dim tempSubject As String = txtSubject.Text.Split("(")(1).Substring(0, txtSubject.Text.Split("(")(1).Length - 1)
                Dim tempDate As String = ddlScreeningDate.SelectedValue.ToString()
                Dim tempGroup As String = Request.QueryString("Group")
                objCollection = CType(Me.Session("PlaceMedEx_" + tempProject + tempSubject + tempDate + tempGroup), ControlCollection)

            ElseIf txtproject.Text.ToString.Trim() <> "" AndAlso txtSubject.Text.ToString.Trim() <> "" AndAlso ddlScreeningDate.SelectedValue.ToString() <> "M" AndAlso Request.QueryString("Group") <> "" Then
                Dim tempProject As String = txtproject.Text.Split("]")(0).Substring(1, txtproject.Text.Split("]")(0).Length - 1)
                Dim tempSubject As String = txtSubject.Text.Split("(")(1).Substring(0, txtSubject.Text.Split("(")(1).Length - 1)
                Dim tempDate As String = ddlScreeningDate.SelectedValue.ToString().Replace("/", "").Split(" ")(0)
                Dim tempGroup As String = Request.QueryString("Group")
                objCollection = CType(Me.Session("PlaceMedEx_" + tempProject + tempSubject + tempDate + tempGroup), ControlCollection)
            Else
                objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection)
            End If


            For Each objControl In objCollection
                filename = String.Empty
                If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.FileUpload" Then
                    If objControl.ID.ToString.Contains("FU") Then
                        ObjId = objControl.ID.ToString.Replace("FU", "")
                    Else
                        ObjId = objControl.ID.ToString.Trim()
                    End If
                    If IsNothing(CType(FindControl("hlnk" + ObjId), HyperLink)) AndAlso Request.Files(objControl.ID).FileName <> "" Then

                        If Me.txtproject.Text <> "" Then
                            filepath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail"))
                            filepath += GeneralModule.Pro_Screening + "/" + Me.HFScreeningWorkSpaceID.Value + "/" + Me.HSubjectId.Value + "/" + TranNo_Retu + "/"
                        Else
                            filepath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail"))
                            filepath += GeneralModule.Pro_Screening + "/" + Me.HSubjectId.Value + "/" + TranNo_Retu + "/"
                        End If
                        Dir = New DirectoryInfo(filepath)
                        If Not Dir.Exists() Then
                            Dir.Create()
                        End If
                        filename = filepath + Path.GetFileName(Request.Files(objControl.ID).FileName.ToString.Trim())
                        Request.Files(objControl.ID).SaveAs(filename)
                    ElseIf Not IsNothing(CType(FindControl("hlnk" + ObjId), HyperLink)) AndAlso CType(FindControl(objControl.ID), FileUpload).HasFile Then
                        If Me.txtproject.Text <> "" Then
                            filepath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail"))
                            filepath += Me.HFScreeningWorkSpaceID.Value + "/" + Me.HSubjectId.Value + "/" + TranNo_Retu + "/"
                        Else
                            filepath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail"))
                            filepath += GeneralModule.Pro_Screening + "/" + Me.HSubjectId.Value + "/" + TranNo_Retu + "/"
                        End If
                        Dir = New DirectoryInfo(filepath)
                        If Not Dir.Exists() Then
                            Dir.Create()
                        End If
                        filename = filepath + Path.GetFileName(Request.Files(objControl.ID).FileName.ToString.Trim())
                        Request.Files(objControl.ID).SaveAs(filename)
                    End If
                End If
            Next objControl
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error While copy file in to folder. ", ex.Message)
        End Try
    End Function
#End Region


#Region "ValidateUserWiseScreeningEntry"

    Private Function ValidateUserWiseScreeningEntry(ByVal mode As String, ByVal subjectid As String, ByVal ScreeningDate As String) As Boolean
        Dim ds_Status As DataSet = Nothing
        Dim Wstr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim PrevScreeningDate As String = String.Empty
        Dim dr As DataRow
        Try

            If Not ScreeningDate Is Nothing Or ScreeningDate <> "" Then

                If mode.ToUpper = "VALIDATE" Then

                    If ScreeningDate = "N" Or ScreeningDate = "" Then
                        Return True
                        Exit Function
                    End If
                    If Request.QueryString("Group") = "" Then
                        Wstr = "SELECT * FROM ScreeningEntryControl WHERE vSubjectID = '" + subjectid + "' AND REPLACE(CONVERT(VARCHAR(11),ScreenDate,106),' ','-')  = '" + CType(ScreeningDate, DateTime).ToString("dd-MMM-yyyy") + "' AND iModifyBy <> " + L_UserId + " AND iNodeId = " + "0"
                    Else
                        Wstr = "SELECT * FROM ScreeningEntryControl WHERE vSubjectID = '" + subjectid + "' AND REPLACE(CONVERT(VARCHAR(11),ScreenDate,106),' ','-')  = '" + CType(ScreeningDate, DateTime).ToString("dd-MMM-yyyy") + "' AND iModifyBy <> " + L_UserId + " AND iNodeId = " + Convert.ToString(Convert.ToInt32(Request.QueryString("Group")))
                    End If

                    ds_Status = objHelp.GetResultSet(Wstr, "ScreeningEntryControl")

                    If Not ds_Status Is Nothing Then
                        If ds_Status.Tables(0).Rows.Count > 0 Then
                            'If Not objLambda.Insert_ScreeningEntryControl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, ds_Status, eStr) Then
                            '    Me.ShowErrorMessage(eStr, "Error while Deleting ScreeningEntryControl")
                            '    Return False
                            'End If
                            Return False
                            Exit Try
                        Else
                            If Request.QueryString("Group") <> "" Then
                                dr = ds_Status.Tables(0).NewRow()
                                dr("vSubjectId") = subjectid
                                dr("ScreenDate") = ScreeningDate
                                dr("iModifyBy") = L_UserId
                                dr("iWorkFlowStageId") = L_WorkFlowStageId.ToString()
                                If Request.QueryString("Group") = "" Then
                                    dr("iNodeid") = 0
                                Else
                                    dr("iNodeid") = Convert.ToInt32(Request.QueryString("Group"))
                                End If
                                ds_Status.Tables(0).Rows.Add(dr)
                            End If
                            ds_Status.Tables(0).AcceptChanges()

                            If Not objLambda.Insert_ScreeningEntryControl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Status, eStr) Then
                                Me.ShowErrorMessage(eStr, "Error while inserting ScreeningEntryControl")
                                Return False
                            End If
                        End If
                    End If
                ElseIf mode.ToUpper = "REMOVEENTRY" Then

                    Wstr = "SELECT * FROM ScreeningEntryControl WHERE vSubjectID = '" + subjectid + "' AND REPLACE(CONVERT(VARCHAR(11),ScreenDate,106),' ','-')  = '" + CType(ScreeningDate, DateTime).ToString("dd-MMM-yyyy") + "'" + " AND iModifyBy = " + L_UserId
                    ds_Status = objHelp.GetResultSet(Wstr, "ScreeningEntryControl")
                    If ds_Status.Tables(0).Rows.Count > 0 Then
                        If Not objLambda.Insert_ScreeningEntryControl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, ds_Status, eStr) Then
                            Me.ShowErrorMessage(eStr, "Error while Deleting ScreeningEntryControl")
                            Return False
                        End If
                    End If
                ElseIf mode.ToUpper = "CONTINUE" Then
                    If ScreeningDate = "N" Then
                        Return True
                        Exit Function
                    End If
                    Wstr = "SELECT * FROM ScreeningEntryControl WHERE vSubjectID = '" + subjectid + "' AND REPLACE(CONVERT(VARCHAR(11),ScreenDate,106),' ','-')  = '" + CType(ScreeningDate, DateTime).ToString("dd-MMM-yyyy") + "'"
                    ds_Status = objHelp.GetResultSet(Wstr, "ScreeningEntryControl")

                    If Not ds_Status Is Nothing Then
                        If ds_Status.Tables(0).Rows.Count > 0 Then
                            Return True
                            Exit Try
                        Else
                            If Request.QueryString("Group") <> "" Then
                                dr = ds_Status.Tables(0).NewRow()
                                dr("vSubjectId") = subjectid
                                dr("ScreenDate") = ScreeningDate
                                dr("iModifyBy") = L_UserId
                                dr("iWorkFlowStageId") = L_WorkFlowStageId.ToString()
                                ds_Status.Tables(0).Rows.Add(dr)
                            End If
                            ds_Status.Tables(0).AcceptChanges()

                            If Not objLambda.Insert_ScreeningEntryControl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Status, eStr) Then
                                Me.ShowErrorMessage(eStr, "Error while inserting ScreeningEntryControl")
                                Return False
                            End If
                        End If
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....ValidateUserWiseScreeningEntry")
            Return False
        End Try

    End Function

#End Region

#Region "Fill Profile"

    Private Sub FillProfile()
        Dim dsProfile As New DataSet
        Dim dvProfile As New DataView
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim dsUsers As New DataSet
        Dim dr As DataRow

        Try

            wStr = "cStatusIndi <> 'D' And vUserName = '" + CType(Me.Session(S_UserName), String).Trim() + "'"
            If Not objHelp.getuserMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            dsUsers, eStr) Then
                Throw New Exception(eStr)
            End If

            If dsUsers Is Nothing Then
                Throw New Exception("User Not Found")
            End If

            wStr = ""
            For Each dr In dsUsers.Tables(0).Rows
                If wStr <> "" Then
                    wStr += ","
                End If
                wStr += "'" + dr("vUserTypeCode").ToString() + "'"
            Next

            If Me.Session(S_Profiles) Is Nothing Then
                Throw New Exception("Profile Not Found")
            End If

            dsProfile.Tables.Add(CType(Me.Session(S_Profiles), DataTable).Copy)
            dvProfile = dsProfile.Tables(0).DefaultView
            dvProfile.RowFilter = "vUserTypeCode in(" + wStr + ")"

            Me.ddlProfile.DataSource = dvProfile.ToTable()
            Me.ddlProfile.DataTextField = "vUserTypeName"
            Me.ddlProfile.DataValueField = "vUserTypeCode"
            Me.ddlProfile.DataBind()

            If Not Request.QueryString("Profile") Is Nothing AndAlso Request.QueryString("Profile") <> "" AndAlso profileChange = False Then
                Me.Session(S_ScrProfileIndex) = Request.QueryString("Profile")
            End If
            If Me.ViewState(VS_ContinueMode) Is Nothing Then
                If Not Me.Session(S_ScrProfileIndex) Is Nothing AndAlso Me.Session(S_ScrProfileIndex) <> "" Then
                    Me.ddlProfile.Items.FindByValue(Me.Session(S_ScrProfileIndex)).Selected = True
                Else
                    'Me.ddlProfile.Items.FindByValue(Me.Session(S_UserType)).Selected = True
                    Me.ddlProfile.Items.FindByText(Me.Session(S_UserName)).Selected = True
                End If
            Else
                Me.ddlProfile.Items.FindByValue(Me.Session(S_ScrProfileIndex)).Selected = True
            End If



        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
            If Not IsNothing(dsProfile) Then
                dsProfile.Dispose()
            End If
        End Try

    End Sub

#End Region

    Private Function FillLocalVariables() As Boolean
        Dim ds As New DataSet
        Dim strUsername As String = String.Empty
        Dim strPassword As String = String.Empty
        Dim sParameter As String = String.Empty
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_UserMst As New DataSet
        Dim dv As New DataView
        Dim dt As New DataTable
        Dim dr As DataRow
        Try
            wStr = "vUserName = '" + Me.Session(S_UserName) + "' And cStatusIndi <> 'D'"
            If Not objHelp.getuserMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_UserMst, eStr) Then
                Throw New Exception(eStr)
            End If
            strUsername = Me.Session(S_UserName).ToString()
            strPassword = ds_UserMst.Tables(0).Rows(0)("vLoginPass").ToString.Trim()
            sParameter = strUsername + "##" + strPassword

            If Not objHelp.Proc_Login(sParameter, ds, eStr) Then
                Throw New Exception(eStr)
            End If
            'dv = ds.Tables(0).DefaultView
            dv = ds.Tables(0).DefaultView
            'dv.RowFilter = "iUserId = '" + Me.Session(S_UserID) + "'"
            If Not Me.Session(S_ScrProfileIndex) Is Nothing AndAlso Me.Session(S_ScrProfileIndex) <> "" Then
                dv.RowFilter = "vUserTypeCode = '" + Me.Session(S_ScrProfileIndex) + "'"
            Else
                'dv.RowFilter = "iUserId = '" + Me.Session(S_UserID) + "'"
                dv.RowFilter = "vUserTypeCode= '" + Me.Session(S_UserType) + "'"
                'dv.RowFilter = "vUserTypeName = '" + Me.Session(S_UserName) + "'"
            End If

            dt = dv.ToTable()
            dr = dt.Rows(0)
            'SelectedProfile = Me.Session(S_UserType).ToString()
            L_FirstName = dr.Item("vFirstName")
            L_LastName = dr.Item("vLastName")
            L_UserId = dr.Item("iUserId")
            L_TimeZoneName = dr.Item("vTimeZoneName")
            L_Password = dr.Item("vLoginPass")
            hdnPassWord.Value = dr.Item("vLoginPass")
            L_UserType = dr.Item("vUserTypeCode")
            L_WorkFlowStageId = dr.Item("iWorkFlowStageId")
            L_LocationCode = dr.Item("vLocationCode")
            L_UserTypeName = dr.Item("vUserTypeCode")
            hdnUserTypeName.Value = dr.Item("vUserTypeCode")
            hdnWebConfigUserTypeName.Value = Convert.ToString(ConfigurationManager.AppSettings("DataTranscription"))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("....FillLocalVariables()", ex.Message)
        End Try
    End Function

#Region "Web Methods"

    <Web.Services.WebMethod()> _
    Public Shared Function GetAuditTrailField(ByVal SubjectId As String, ByVal ScreeningDate As String, ByVal ScreeningType As String, ByVal MedexCode As String, ByVal locationcode As String) As String '' Add location code to display Actual time timezone wise.

        Dim ObjCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
        Dim dsConvert As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_Audit As New DataSet
        Dim RowIndex As Integer = 1
        Dim ds As New DataSet
        Dim dc_Audit As New DataColumn
        Try
            wStr = "vSubjectId='" & SubjectId & "' And vMedexCode='" & MedexCode & "' And " & _
                    " dScreenDate='" & ScreeningDate & "' ORDER BY iTranNo ASC"

            If ScreeningType = "D" Then
                If Not objHelp.View_MedExScreeningHdrDtlAuditTrail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                 ds_Audit, eStr) Then
                    Return False
                End If
            Else
                If Not objHelp.GetData("View_MedExScreeningHdrDtlAuditTrail_ProjectSpecific", "*", wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                 ds_Audit, eStr) Then
                    Return False
                End If
            End If
            If Not ds_Audit Is Nothing Then
                If ds_Audit.Tables(0).Rows.Count > 0 Then
                    ds_Audit.Tables(0).Columns.Add("dModifyOffSet", Type.GetType("System.String"))
                    ds_Audit.Tables(0).Columns.Add("dModifyScreeningDt", Type.GetType("System.String"))
                    ds_Audit.Tables(0).Columns.Add(("ActualTIME"), Type.GetType("System.String"))
                    For Each dr_Audit In ds_Audit.Tables(0).Rows
                        dr_Audit("dModifyOffSet") = Convert.ToString(CDate(dr_Audit("dModifyOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                        dr_Audit("dModifyScreeningDt") = Convert.ToString(CDate(dr_Audit("dScreenDate")).ToString("dd-MMM-yyyy"))
                        If Not objHelp.Proc_ActualAuditTrailTime(CDate(dr_Audit("dModifyOn")).ToString("dd-MMM-yyyy HH:mm") + "##" + locationcode, ds, eStr) Then '' Added by Dipen shah on 3-dec-2014
                            Throw New Exception(eStr)
                        End If
                        If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                            dr_Audit("ActualTIME") = CDate(ds.Tables(0).Rows(0).Item("ActualTIME")).ToString("dd-MMM-yyyy HH:mm") + " (" + ds.Tables(0).Rows(0).Item("TimeZoneOffSet").ToString + " GMT)"
                        End If
                    Next
                    ds_Audit.Tables(0).TableName = "tblAudit"
                    dsConvert.Tables.Add(ds_Audit.Tables(0).DefaultView.ToTable(True, "vSubjectId,dModifyScreeningDt,vMedExDesc,vDefaultValue,vRemarks,vModifyBy,dModifyOffSet,ActualTIME".Split(",")).DefaultView.ToTable())
                    Return JsonConvert.SerializeObject(dsConvert.Tables(0))
                End If
            End If
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function GetAuditTrailReview(ByVal ScreeningHdrNo As String, ByVal LocationCode As String) As String
        Dim ObjCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
        Dim dsConvert As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_Audit As New DataSet
        Dim RowIndex As Integer = 1
        Dim Ds As New DataSet
        Try

            wStr = "SELECT * FROM view_ScreeningWorkFlowDtl WHERE  nMedExScreeningHdrNo='" & ScreeningHdrNo & "' ORDER BY dModifyOn asc"
            ds_Audit = objhelpDb.GetResultSet(wStr, "view_ScreeningWorkFlowDtl")

            If Not ds_Audit Is Nothing Then
                If ds_Audit.Tables(0).Rows.Count > 0 Then
                    ds_Audit.Tables(0).Columns.Add("dModifyOffSet", Type.GetType("System.String"))
                    ds_Audit.Tables(0).Columns.Add("ActualTime", Type.GetType("System.String"))
                    For Each dr_Audit In ds_Audit.Tables(0).Rows
                        dr_Audit("dModifyOffSet") = Convert.ToString(CDate(dr_Audit("dModifyOn")).ToString("dd-MMM-yyyy HH:mm"))
                        If Not objHelp.Proc_ActualAuditTrailTime(CDate(dr_Audit("dModifyOn")).ToString("dd-MMM-yyyy HH:mm") + "##" + LocationCode, Ds, eStr) Then
                            Throw New Exception(eStr)
                        End If
                        If Not Ds Is Nothing AndAlso Ds.Tables(0).Rows.Count > 0 Then
                            dr_Audit("ActualTIME") = CDate(Ds.Tables(0).Rows(0).Item("ActualTIME")).ToString("dd-MMM-yyyy HH:mm") + " (" + Ds.Tables(0).Rows(0).Item("TimeZoneOffSet").ToString + " GMT)"
                        End If
                    Next
                    ds_Audit.Tables(0).TableName = "tblAudit"
                    dsConvert.Tables.Add(ds_Audit.Tables(0).DefaultView.ToTable(True, "vRemark,iModifyBy,dModifyOffSet,dScreenDate,vSubjectId,cIsEligible,ActualTIME".Split(",")).DefaultView.ToTable())
                    Return JsonConvert.SerializeObject(dsConvert.Tables(0))
                End If
            End If

        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function GetFormulaeVal(ByVal formula As String, ByVal controltype As String, ByVal DecimalNo As Integer) As String
        Dim objEvaluator As New Evaluator
        Dim FinalResult As String = String.Empty
        Try
            If controltype = "DateTime" Then
                FinalResult = objEvaluator.GetDateDiff(formula)
            Else
                FinalResult = Math.Round(objEvaluator.Eval(formula), DecimalNo, MidpointRounding.AwayFromZero)
            End If
            Return FinalResult
        Catch ex As Exception
            Return ex.Message
        End Try

    End Function

    '<Web.Services.WebMethod()> _
    'Public Shared Function FolderVal(ByVal MedexCode As String, ByVal MedexType As String, ByVal MedexResult As String) As Boolean



    '    Return True
    'End Function

    <Web.Services.WebMethod()> _
    Public Shared Function GetSubjectWeightData(ByVal SubjectId As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim ds As DataSet = Nothing
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim Datetoday As String = objHelp.GetServerDateTime().ToString("yyyy-MM-dd")

        Try

            Wstr = " vWorkSpaceId='0000000000' And iNodeid='1' And vSubjectId='" + SubjectId + "'"
            Wstr += " AND Convert(Date,dModifyOn,110)= Cast('" + Datetoday + "' As Date)"

            If Not objHelp.GetData("InstruMentInterfaceHdr", "vWeight", Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr) Then
                Throw New Exception("Error While get data from InstruMentInterfaceHdr" + estr.ToString)
            End If

            If ds.Tables(0).Rows.Count = 0 Then
                Return "False"
            Else
                strReturn = ds.Tables(0).Rows(0)("vWeight")
                Return strReturn
            End If

        Catch ex As Exception
            Return ex.Message
            Return False
        End Try
        Return True
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function TranscribeAuditTrail(ByVal SubjectId As String, ScreenDate As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim ds As DataSet = Nothing
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim Datetoday As String = objHelp.GetServerDateTime().ToString("yyyy-MM-dd")

        Try
            Wstr = SubjectId + "##" + ScreenDate
            ds = objHelp.ProcedureExecute("Proc_TransCribeScreeningAuditTrail", Wstr)

            If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                If ds.Tables(0).Rows.Count = 0 Then
                    Return "False"
                Else
                    strReturn = JsonConvert.SerializeObject(ds.Tables(0))
                End If
            Else
                Return "False"
            End If
            Return strReturn
        Catch ex As Exception
            Return ex.Message
            Return False
        End Try

    End Function


    <Web.Services.WebMethod()> _
    Public Shared Function FreezeAuditTrail(ByVal SubjectId As String, ScreenDate As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim ds As DataSet = Nothing
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim Datetoday As String = objHelp.GetServerDateTime().ToString("yyyy-MM-dd")

        Try
            Wstr = SubjectId + "##" + ScreenDate
            ds = objHelp.ProcedureExecute("Proc_ScreeningVersionMst", Wstr)

            If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                If ds.Tables(0).Rows.Count = 0 Then
                    Return "False"
                Else
                    strReturn = JsonConvert.SerializeObject(ds.Tables(0))
                End If
            Else
                Return "False"
            End If
            Return strReturn
        Catch ex As Exception
            Return ex.Message
            Return False
        End Try

    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function GenerateDCF(ByVal ScreeningDCFNo As String, ByVal ScreeningDCFDtlNo As String, ByVal vMedExCode As String, ByVal ModifyBy As String, ByVal Type As String, ByVal ReasonForUpdate As String, ByVal WorkSpaceID As String) As String
        Dim returnStr As String = ""
        Dim ObjCommon As New clsCommon
        Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
        Dim ds_ScreeningDCF As New DataSet
        Dim drScreeningDCF As DataRow
        Dim controlid As String = String.Empty
        Dim discrepancy As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_MedExScreeningDtl As New DataSet
        Dim wStr As String = String.Empty
        Dim dsScreeningDCF As New DataSet
        Dim ds_ScreeningDCFStructute As New DataSet
        Try
            returnStr = True

            If Type = "G" Then


                wStr = "SELECT * FROM ScreeningDCfMst  WHERE nMedExScreeningDtlNo = " + ScreeningDCFDtlNo + " AND cDCFStatus = 'N' "
                ds_ScreeningDCF = objHelp.GetResultSet(wStr, "ScreeningDCFMst")

                If Not ds_ScreeningDCF Is Nothing AndAlso ds_ScreeningDCF.Tables(0).Rows.Count > 0 Then
                    returnStr = "DCF Already Generated"
                    Return JsonConvert.SerializeObject(returnStr)
                End If

                wStr = "Select * From ScreeningDCfMst Where 1 = 2 "

                ds_ScreeningDCFStructute = objHelp.GetResultSet(wStr, "ScreeningDCFMst")
                drScreeningDCF = ds_ScreeningDCFStructute.Tables(0).NewRow()

                drScreeningDCF("nScreeningDCFNo") = 0
                drScreeningDCF("nMedExScreeningDtlNo") = ScreeningDCFDtlNo
                drScreeningDCF("iSrNo") = 0
                drScreeningDCF("vMedExcode") = vMedExCode
                drScreeningDCF("cDCFType") = "M"
                drScreeningDCF("iDCFBy") = ModifyBy
                drScreeningDCF("dDCFDate") = System.DateTime.Now()
                drScreeningDCF("vDiscrepancy") = ""
                drScreeningDCF("vSourceResponse") = ReasonForUpdate
                drScreeningDCF("cDCFStatus") = "N"
                drScreeningDCF("iModifyBy") = ModifyBy
                drScreeningDCF("dModifyOn") = DateTime.Now()
                drScreeningDCF("vWorkSpaceId") = IIf(WorkSpaceID = "", "0000000000", WorkSpaceID)
                ds_ScreeningDCFStructute.Tables(0).Rows.Add(drScreeningDCF)
                ds_ScreeningDCFStructute.AcceptChanges()

                If Not objLambda.Save_ScreeningDCFMST(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_ScreeningDCFStructute, eStr) Then
                    returnStr = "Error While Saving."
                    Return JsonConvert.SerializeObject(returnStr)
                End If
            ElseIf Type = "R" Then

                returnStr = "DCF Resolved Successfully"
                wStr = "SELECT * FROM ScreeningDCfMst  WHERE nScreeningDCFNo = " + ScreeningDCFNo
                ds_ScreeningDCF = objHelp.GetResultSet(wStr, "ScreeningDCFMst")

                If Not ds_ScreeningDCF Is Nothing AndAlso ds_ScreeningDCF.Tables(0).Rows.Count > 0 Then
                    For Each dr As DataRow In ds_ScreeningDCF.Tables(0).Rows
                        dr("iStatusChangedBy") = ModifyBy
                        dr("vUpdateRemarks") = ReasonForUpdate
                        dr("cDCFStatus") = "R"
                        dr("iModifyBy") = ModifyBy
                    Next
                    ds_ScreeningDCF.Tables(0).AcceptChanges()

                    If Not objLambda.Save_ScreeningDCFMST(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_ScreeningDCF, eStr) Then
                        returnStr = "Error While Saving."
                        Return JsonConvert.SerializeObject(returnStr)
                    End If
                End If
            End If
            Return JsonConvert.SerializeObject(returnStr)

        Catch ex As Exception
            Return JsonConvert.SerializeObject("False")
        End Try


    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function GETDCFGRID(ByVal ScreeningDCFDtlNo As String, ByVal vMedExCode As String, ByVal WorkSpaceId As String) As String
        Dim ds_ScreeningDCF As DataSet
        Dim returnScreening As String = ""
        Dim wStr As String = ""

        Try
            wStr = Convert.ToString(ScreeningDCFDtlNo) + "##" + Convert.ToString(vMedExCode) + "##" + WorkSpaceId
            ds_ScreeningDCF = objHelp.ProcedureExecute("Proc_GetScreeningDcfMst", wStr)

            Return JsonConvert.SerializeObject(ds_ScreeningDCF.Tables(0))
        Catch ex As Exception
            Return returnScreening
        End Try



    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function ScreeningIsFreeze(ByVal ScreeningDate As String, ByVal SubjectId As String, ByVal vWorkSpaceId As String) As String
        Dim returnScreening As String = "UnFreeze"
        Dim ds_ScreeningVersionStatus As DataSet
        Dim wStr As String = ""

        Try
            wStr = ScreeningDate + "##" + SubjectId + "##" + IIf(vWorkSpaceId = "", "0000000000", vWorkSpaceId)

            ds_ScreeningVersionStatus = objHelp.ProcedureExecute("Proc_ScreeningUnFreezeValidation", wStr)
            If Not ds_ScreeningVersionStatus Is Nothing AndAlso ds_ScreeningVersionStatus.Tables(0).Rows.Count > 0 Then
                returnScreening = Convert.ToString(ds_ScreeningVersionStatus.Tables(0).Rows(0)(0))
            End If
            Return JsonConvert.SerializeObject(returnScreening)
        Catch ex As Exception
            Return returnScreening
        End Try



    End Function


    <Web.Services.WebMethod()> _
    Public Shared Function Save_ScreeningVersionMst(ByVal ScreeningDate As String, ByVal SubjectId As String, ByVal FreezeStatus As String, ByVal Remarks As String, ByVal ModifyBy As String) As String
        Dim returnScreening As String = ""
        Dim ds_ScreeningVersionStatus As DataSet
        Dim wStr As String = ""
        Dim eStr_Ret As String = ""
        Dim dr As DataRow
        Dim MEdExScreeningHdrNo As Integer
        Dim ds_UnfreezeValidation As DataSet

        Try

            wStr = ScreeningDate + "##" + SubjectId
            ds_UnfreezeValidation = objHelp.ProcedureExecute("Proc_ScreeningUnfreezeStatus", wStr)

            If Not ds_UnfreezeValidation Is Nothing AndAlso ds_UnfreezeValidation.Tables(0).Rows.Count > 0 Then
                For Each dr2 As DataRow In ds_UnfreezeValidation.Tables(0).Rows

                    If dr2("cRejectionFlag") = "Not Rejected" Then
                        Return JsonConvert.SerializeObject("Please Reject Subject")
                        Exit For
                    End If
                Next

            End If

            wStr = "SELECT * FROM MedExScreeningHdr Where vSubjectId = '" + SubjectId + "' AND CAST(dScreenDate as DATE) = CAST('" + ScreeningDate + "' as DATE)"

            ds_ScreeningVersionStatus = objHelp.GetResultSet(wStr, "MedExScreeningHdr")

            If Not ds_ScreeningVersionStatus Is Nothing AndAlso ds_ScreeningVersionStatus.Tables(0).Rows.Count > 0 Then
                For Each dr1 As DataRow In ds_ScreeningVersionStatus.Tables(0).Rows
                    MEdExScreeningHdrNo = dr1("nMedExScreeningHdrNo")
                    Exit For
                Next
            Else
                Return JsonConvert.SerializeObject("False")
            End If

            wStr = "  SELECT * FROM ScreeningVersionMst WHERE  1=2"

            ds_ScreeningVersionStatus = objHelp.GetResultSet(wStr, "ScreeningVersionMst")

            dr = ds_ScreeningVersionStatus.Tables(0).NewRow()
            dr("nMEdExScreeningHdrNo") = MEdExScreeningHdrNo
            If FreezeStatus = "Freeze" Then
                dr("cFreezeStatus") = "F"
            Else
                dr("cFreezeStatus") = "U"
            End If
            dr("vRemarks") = Remarks
            dr("cStatusIndi") = "N"
            dr("iModifyBy") = Convert.ToInt64(ModifyBy)

            ds_ScreeningVersionStatus.Tables(0).Rows.Add(dr)
            ds_ScreeningVersionStatus.Tables(0).TableName = "Insert_ScreeningVersionMst"
            ds_ScreeningVersionStatus.AcceptChanges()
            wStr = ScreeningDate + "##" + SubjectId
            returnScreening = "True"
            If Not objLmd.TableInsert(ds_ScreeningVersionStatus, "", eStr_Ret) Then
                returnScreening = "False"
            End If

            Return JsonConvert.SerializeObject(returnScreening)
        Catch ex As Exception
            Return JsonConvert.SerializeObject("False")
        End Try

    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function ScreeningDataEntryCompleted(ByVal ScreeningHdrNo As String) As String
        Dim returnScreening As String = ""
        Dim ds_ScreeningDataWntryComplete As DataSet
        Dim wStr As String = ""
        Dim eStr_Ret As String = ""

        Try
            wStr = ScreeningHdrNo
            ds_ScreeningDataWntryComplete = objHelp.ProcedureExecute("Proc_ScreeningDataEntryCompleted", wStr)
            If Not ds_ScreeningDataWntryComplete Is Nothing AndAlso ds_ScreeningDataWntryComplete.Tables(0).Rows.Count > 0 Then
                returnScreening = Convert.ToString(ds_ScreeningDataWntryComplete.Tables(0).Rows(0)(0))
            End If
            Return JsonConvert.SerializeObject(returnScreening)

        Catch ex As Exception
            Return JsonConvert.SerializeObject("False")
        End Try
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function TranscribeScreeningValidation(ByVal SubjectId As String, ByVal TranscribeDate As String) As String
        Dim ds_Validation As DataSet
        Dim returnScreening As String = ""
        Dim wstr = Convert.ToString(SubjectId) + "##" + TranscribeDate
        ds_Validation = objHelp.ProcedureExecute("dbo.Proc_ScreeningSameDayValidation", wstr)
        If Not ds_Validation Is Nothing AndAlso ds_Validation.Tables(0).Rows.Count > 0 Then
            Return JsonConvert.SerializeObject("Subject Already Screened")
        End If
        Return JsonConvert.SerializeObject("True")

    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function Proc_ValidationForEligibility(ByVal SubjectId As String, ByVal ScreeningDate As String, ByVal PWDEnter As String, ByVal PWDOld As String) As String
        Dim ds_Validation As DataSet
        Dim ds_HardCodeValidationn As DataSet
        Dim returnScreening As String = ""
        Dim pwd As String = String.Empty
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        pwd = PWDEnter
        pwd = objHelp.EncryptPassword(pwd)

        If PWDOld <> pwd.ToString() Then
            Return JsonConvert.SerializeObject("PassWord Incorrect")
        End If

        Dim wstr = Convert.ToString(SubjectId) + "##" + ScreeningDate

        ds_HardCodeValidationn = objHelp.ProcedureExecute("dbo.Proc_ValidationForHardCodeAttribute", wstr)
        If Not ds_HardCodeValidationn Is Nothing AndAlso ds_HardCodeValidationn.Tables(0).Rows.Count > 0 Then
            Return JsonConvert.SerializeObject(ds_HardCodeValidationn.Tables(0).Rows(0)(0))
        End If
        If Not ds_HardCodeValidationn Is Nothing AndAlso ds_HardCodeValidationn.Tables(1).Rows.Count > 0 Then
            Return JsonConvert.SerializeObject(ds_HardCodeValidationn.Tables(1).Rows(0)(0))
        End If


        ds_Validation = objHelp.ProcedureExecute("dbo.Proc_ValidationForEligibility", wstr)
        If Not ds_Validation Is Nothing AndAlso ds_Validation.Tables(0).Rows.Count > 0 Then
            For Each dr As DataRow In ds_Validation.Tables(0).Rows
                If Convert.ToString(dr("cIsEligible")) = "" Or Convert.ToString(dr("cIsEligible")) = "N" Then
                    Return JsonConvert.SerializeObject("N")
                Else
                    Return JsonConvert.SerializeObject("Y")
                End If
            Next
        End If
        Return JsonConvert.SerializeObject("N")
    End Function


    <Web.Services.WebMethod()> _
    Public Shared Function Proc_ScreeningVersionStatus(ByVal SubjectId As String, ByVal ScreeningDate As String) As String
        Dim ds As DataSet
        Dim wStr As String
        Dim status As String = "UnFreeze"
        Try
            wStr = ScreeningDate + "##" + SubjectId
            ds = objHelp.ProcedureExecute("Proc_ScreeningVersionStatus", wStr)

            If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    status = dr("cFreezeStatus")
                Next
                Return JsonConvert.SerializeObject(status)
            End If
            Return JsonConvert.SerializeObject("UnFreeze")
        Catch ex As Exception
            Return JsonConvert.SerializeObject("UnFreeze")
        End Try
    End Function


#End Region

    'Protected Sub btnSaveDiscrepancy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDiscrepancy.Click
    '    Dim ds_ScreeningDCF As New DataSet
    '    Dim drScreeningDCF As DataRow
    '    Dim controlid As String = String.Empty
    '    Dim discrepancy As String = String.Empty
    '    Dim eStr As String = String.Empty
    '    Dim ds_MedExScreeningDtl As New DataSet
    '    Dim wStr As String = String.Empty
    '    Dim dsScreeningDCF As New DataSet

    '    Try

    '        Me.btnSaveDiscrepancy.Enabled = False
    '        wStr = "SELECT * FROM ScreeningDCfMst  WHERE 1=2"
    '        ds_ScreeningDCF = objHelp.GetResultSet(wStr, "ScreeningDCFMst")
    '        drScreeningDCF = ds_ScreeningDCF.Tables(0).NewRow()

    '        drScreeningDCF("nScreeningDCFNo") = 0
    '        drScreeningDCF("nMedExScreeningDtlNo") = ScreeningDTLNo
    '        drScreeningDCF("iSrNo") = 0
    '        drScreeningDCF("vMedExcode") = DCFMedExCode
    '        drScreeningDCF("cDCFType") = "M"
    '        drScreeningDCF("iDCFBy") = Me.Session(S_UserID)
    '        drScreeningDCF("dDCFDate") = System.DateTime.Now()
    '        drScreeningDCF("vDiscrepancy") = discrepancy
    '        drScreeningDCF("vSourceResponse") = Me.txtDiscrepancyRemarks.Text.Trim()
    '        drScreeningDCF("cDCFStatus") = Me.ddlDiscrepancyStatus.SelectedItem.Value.Trim()
    '        drScreeningDCF("iModifyBy") = Me.Session(S_UserID)
    '        drScreeningDCF("dModifyOn") = DateTime.Now()
    '        ds_ScreeningDCF.Tables(0).Rows.Add(drScreeningDCF)
    '        ds_ScreeningDCF.AcceptChanges()

    '        If Not Me.objLambda.Save_ScreeningDCFMST(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_ScreeningDCF, eStr) Then
    '            Me.ObjCommon.ShowAlert(eStr, Me.Page)
    '            Exit Sub
    '        End If
    '    Catch ex As Exception

    '    End Try

    'End Sub

    Public Function ShowDCFPopup(ByVal Show As String) As Boolean
        Dim ds_ScreeningDcfMst As DataSet
        Dim wStr As String = ""

        If Show = "S" Then
            If Not Session("DCFMedExCode") Is Nothing Then
                wStr = Session("DCFMedExCode").ToString() + "##" + Session("ScreeningDTLNo").ToString()
            Else
                wStr = "" + "##" + Session("ScreeningDTLNo").ToString()
            End If

            ds_ScreeningDcfMst = objHelp.ProcedureExecute("Proc_GetScreeningDcfMst", wStr)

            If Not ds_ScreeningDcfMst Is Nothing AndAlso ds_ScreeningDcfMst.Tables(0).Rows.Count > 0 Then
                'GVWDCF.DataSource = ds_ScreeningDcfMst
                'GVWDCF.DataBind()
            End If
            mpDCFGenerate.Show()
        ElseIf Show = "H" Then
            mpDCFGenerate.Hide()
        End If
        Return True
    End Function


    'Protected Sub GVWDCF_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVWDCF.RowDataBound
    '    e.Row.Cells(GVCDCF_nDCFNo).Attributes.Add("style", "display:none;")
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).CommandArgument = e.Row.RowIndex
    '        CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).CommandName = "UPDATE"
    '        CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).Attributes.Add("onclick", "return ShowEditDCF('S'," + e.Row.Cells(GVCDCF_nDCFNo).Text + ");")
    '        e.Row.Cells(GVCDCF_iSrNo).Text = (e.Row.RowIndex + 1).ToString()



    '        'If ((e.Row.RowIndex + 1) Mod 2 = 0) Then

    '        '    Dim TextRemarks As TextBox = DirectCast(e.Row.FindControl("txtRemarks"), TextBox)
    '        '    TextRemarks.BorderStyle = BorderStyle.None
    '        '    TextRemarks.BackColor = Drawing.Color.White
    '        '    TextRemarks.ForeColor = Drawing.Color.Navy
    '        'Else
    '        '    Dim TextRemarks As TextBox = DirectCast(e.Row.FindControl("txtRemarks"), TextBox)
    '        '    TextRemarks.BorderStyle = BorderStyle.None
    '        '    TextRemarks.BackColor = System.Drawing.ColorTranslator.FromHtml("#cee3ed")
    '        '    TextRemarks.ForeColor = Drawing.Color.Navy

    '        'End If

    '        If e.Row.Cells(GVCDCF_UserType).Text.Trim() = Me.Session(S_UserType).ToString() Then
    '            CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).Visible = True
    '        End If

    '        If e.Row.Cells(GVCDCF_cDCFStatus).Text.Trim.ToUpper() = Discrepancy_Answered And _
    '            (e.Row.Cells(GVCDCF_iDCFBy).Text.Trim.ToUpper() <> Convert.ToString(Me.Session(S_UserID)).Trim.ToUpper() And _
    '             e.Row.Cells(GVCDCF_UserType).Text.Trim() <> Me.Session(S_UserType).ToString()) Then
    '            e.Row.Cells(GVCDCF_cDCFStatus).Text = "Answered"
    '            CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).Visible = False
    '        End If

    '        If e.Row.Cells(GVCDCF_cDCFStatus).Text.Trim.ToUpper() = Discrepancy_Generated Then
    '            e.Row.Cells(GVCDCF_cDCFStatus).Text = "Generated"

    '        ElseIf e.Row.Cells(GVCDCF_cDCFStatus).Text.Trim.ToUpper() = Discrepancy_Answered Then
    '            e.Row.Cells(GVCDCF_cDCFStatus).Text = "Answered"

    '        ElseIf e.Row.Cells(GVCDCF_cDCFStatus).Text.Trim.ToUpper() = Discrepancy_AutoResolved Then
    '            e.Row.Cells(GVCDCF_cDCFStatus).Text = "Auto Resolved"
    '            CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).Visible = False

    '        ElseIf e.Row.Cells(GVCDCF_cDCFStatus).Text.Trim.ToUpper() = Discrepancy_Resolved Then
    '            e.Row.Cells(GVCDCF_cDCFStatus).Text = "Resolved"
    '            CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).Visible = False

    '        ElseIf e.Row.Cells(GVCDCF_cDCFStatus).Text.Trim.ToUpper() = Discrepancy_InternallyResolved Then
    '            e.Row.Cells(GVCDCF_cDCFStatus).Text = " Internally Resolved"
    '            CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).Visible = False
    '        End If
    '        'If e.Row.Cells(GVCDCF_UserType).Text.Trim() <> Session(S_ScrProfileIndex).ToString() AndAlso
    '        '    CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).Visible = False Then
    '        'End If
    '        If e.Row.Cells(GVCDCF_cDCFType).Text.Trim.ToUpper() = "S" Then
    '            e.Row.Cells(GVCDCF_cDCFType).Text = "System"
    '            e.Row.Cells(GVCDCF_vCreatedBy).Text = "System"
    '            If e.Row.Cells(GVCDCF_cDCFStatus).Text.Trim.ToUpper() = Discrepancy_AutoResolved Then
    '                'e.Row.Cells(GVCDCF_vUpdatedBy).Text = "System"
    '            End If
    '        ElseIf e.Row.Cells(GVCDCF_cDCFType).Text.Trim.ToUpper() = "M" Then
    '            e.Row.Cells(GVCDCF_cDCFType).Text = "Manual"
    '        End If
    '    End If
    'End Sub

    'Protected Sub imgbtnTranscribe_Click(sender As Object, e As ImageClickEventArgs)
    '    Dim Wstr As String
    '    Dim ds As DataSet
    '    Wstr = Convert.ToString(Request.QueryString("SubId")) + "##" + Convert.ToString(Request.QueryString("ScrDt"))

    '    ds = objHelp.ProcedureExecute("Proc_TransCribeScreeningAuditTrail", Wstr)
    '    If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count Then
    '        grdTranscribeAuditTrail.DataSource = ds
    '        grdTranscribeAuditTrail.DataBind()
    '        TransCribePanel.Update()
    '    End If
    '    Me.hdnIsOpenAuditTrail.Value = "True"
    '    mpTranscribeAuditTrail.Hide()
    '    mpTranscribeAuditTrail.Show()

    'End Sub

    Protected Function FillReason() As Boolean
        Dim estr As String = String.Empty
        Dim Ds_Reason As New DataSet
        Dim wStr As String = String.Empty
        Dim item As New ListItem
        Try

            wStr = " vActivityId ='" & ConfigurationManager.AppSettings.Item("Screening").Trim() & "' and cStatusIndi <> 'D'"

            If Not Me.objHelpdb.View_ReasonMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                            Ds_Reason, estr) Then
                Throw New Exception(estr)
            End If
            If Not Ds_Reason Is Nothing AndAlso Ds_Reason.Tables(0).Rows.Count > 0 Then
                Me.ddlDCFEditReason.DataSource = Ds_Reason.Tables(0)
                Me.ddlDCFEditReason.DataValueField = "nReasonNo"
                Me.ddlDCFEditReason.DataTextField = "vReasonDesc"
                Me.ddlDCFEditReason.DataBind()

                item.Text = "Please Select a Reason"
                item.Value = "0"
                Me.ddlDCFEditReason.Items.Insert(0, item)


                Me.ddlRemarksForEdit.DataSource = Ds_Reason.Tables(0)
                Me.ddlRemarksForEdit.DataValueField = "nReasonNo"
                Me.ddlRemarksForEdit.DataTextField = "vReasonDesc"
                Me.ddlRemarksForEdit.DataBind()

                item.Text = "Please Select a Reason"
                item.Value = "0"
                Me.ddlRemarksForEdit.Items.Insert(0, item)

            End If


            Return True
        Catch ex As Exception

            Return False
        End Try
    End Function

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub


#Region "ECG Review"""

    <Web.Services.WebMethod> _
    Public Shared Function ReviewECG(ByVal vWorkSpaceId As String, ByVal vSubjectId As String, ByVal dScreeningDate As String) As String
        Dim SubjectScreening As frmSubjectScreening_New = New frmSubjectScreening_New()
        Dim dsECGReviewDetails As DataSet = New DataSet()
        Dim wstr As String
        Try
            wstr = vWorkSpaceId + "##" + vSubjectId + "##" + dScreeningDate
            dsECGReviewDetails = objHelp.ProcedureExecute("dbo.Proc_GetSubjectDetailsForScreeningECG", wstr)
            Return JsonConvert.SerializeObject(dsECGReviewDetails.Tables(0))
        Catch ex As Exception
            Return "Error : " + ex.Message.ToString()
        Finally
        End Try

    End Function

    Protected Function ReviewECGDetail(ByVal vWorkSpaceId As String, ByVal vSubjectId As String, ByVal dScreeningDate As String, ByRef dsECGReviewDetails As DataSet) As Boolean

        Dim wstr As String
        Try
            wstr = vWorkSpaceId + "##" + vSubjectId + "##" + dScreeningDate

            dsECGReviewDetails = objHelp.ProcedureExecute("dbo.Proc_GetSubjectDetailsForScreeningECG", wstr)

            If Not dsECGReviewDetails Is Nothing Then
                If dsECGReviewDetails.Tables(0).Rows.Count > 0 Then
                    gdvDocumentList.DataSource = dsECGReviewDetails.Tables(0)
                    gdvDocumentList.DataBind()
                    ''Me.mdDocumentList.Show()
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gdvECGReview", "fnECGReviewUI();", True)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ValidateFillECGGrid", "fnValidateFillECGGrid('" + vSubjectId + "','" + dScreeningDate + "');", True)
                End If
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ValidateFillECGGrid", "fnValidateFillECGGrid('" + vSubjectId + "','" + dScreeningDate + "');", True)
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Protected Sub ImgBtnShowECG_Click(sender As Object, e As ImageClickEventArgs) Handles ImgBtnShowECG.Click
        Dim dScreenDate As Date
        Dim vWorkSpaceId As String = String.Empty
        Dim vSubjectId As String = String.Empty
        Dim wstr As String = String.Empty
        Dim dsECGReviewDetails As DataSet = New DataSet()
        Try
            If Me.txtSubject.Text = "" And Me.HSubjectId.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ValidateFillECGGrid", "fnValidateECGData(""Please Enter SubjectID to review ECG!"");", True)
                Return
            End If
            If chkScreeningType.Items(0).Selected Then ''Generic Screening
                If Me.ddlScreeningDate.SelectedValue = "N" Then ''New Screening
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ValidateFillECGGrid", "fnValidateECGData(""No ECG detail can be available for New Screening!"");", True)
                    Return
                ElseIf Me.ddlScreeningDate.SelectedValue = "M" Then ''Select Screening
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ValidateFillECGGrid", "fnValidateECGData(""Please Select Screening Date!"");", True)
                    Return
                Else
                    dScreenDate = Me.ddlScreeningDate.SelectedValue
                    vWorkSpaceId = "0000000000"
                End If
            Else ''Project Specific Screening
                If Me.txtproject.Text = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ValidateFillECGGrid", "fnValidateECGData(""Please Select Project for Project Specific Screening to review ECG!"");", True)
                    Return
                End If
                If Me.ddlScreeningDate.SelectedValue = "N" Then ''New Screening
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ValidateFillECGGrid", "fnValidateECGData(""No ECG detail can be available for New Screening!"");", True)
                    Return
                ElseIf Me.ddlScreeningDate.SelectedValue = "M" Then ''Select Screening
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ValidateFillECGGrid", "fnValidateECGData(""Please Select Screening Date!"");", True)
                    Return
                Else
                    dScreenDate = Me.ddlScreeningDate.SelectedValue
                    vWorkSpaceId = Me.HProjectId.Value
                End If
            End If
            vSubjectId = Me.HSubjectId.Value

            wstr = vWorkSpaceId + "##" + vSubjectId + "##" + dScreenDate

            dsECGReviewDetails = objHelp.ProcedureExecute("dbo.Proc_GetSubjectDetailsForScreeningECG", wstr)

            If Not dsECGReviewDetails Is Nothing Then
                If dsECGReviewDetails.Tables(0).Rows.Count > 0 Then
                    gdvDocumentList.DataSource = dsECGReviewDetails.Tables(0)
                    gdvDocumentList.DataBind()
                    Me.mdDocumentList.Show()
                    ''Me.mdDocument.Show()
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gdvECGReview", "fnECGReviewUI();", True)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ValidateFillECGGrid", "fnValidateFillECGGrid();", True)
                End If
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ValidateFillECGGrid", "fnValidateFillECGGrid();", True)
            End If

        Catch ex As Exception

        End Try
    End Sub

#End Region

End Class
