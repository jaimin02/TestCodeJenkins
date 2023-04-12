Imports System.Web.Services
Imports Newtonsoft.Json
Imports System.Linq
Imports System.Web.UI.Page

Partial Class frmCTMMedExInfoHdrDtl
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

    Private Const VS_dtMedEx_Fill As String = "dt_MedEx_Fill"
    Private Const VS_dtMedEx_Fill_Backup As String = "dt_MedEx_Fill_BackUp"
    Private Const VS_DtCRFHdr As String = "DtCRFHdr"
    Private Const VS_DtCRFDtl As String = "DtCRFDtl"
    Private Const VS_DtCRFSubDtl As String = "DtCRFSubDtl"
    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtDCF As String = "DtDCF"
    Private Const VS_DCFNo As String = "DCFNo"
    Private Const VS_DataStatus As String = "DataStatus"
    Private Const VS_ReviewFlag As String = "ReviewFlag"
    Public S_DiscrpancyStatus As String = "DiscrpancyStatus"
    Public S_TabulerActivity As String = "TabulerActivity"
    Public S_tabuler As String = "tabuler"
    Private Const S_Repeatition As String = "S_Repeatition"
    Private Const S_Review As String = "S_Review"

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
    Private Const GVCDCF_vUpdateRemarks As Integer = 10
    Private Const GVCDCF_cDCFStatus As Integer = 11
    Private Const GVCDCF_vUpdatedBy As Integer = 12
    Private Const GVCDCF_dStatusChangedOn As Integer = 13
    Private Const GVCDCF_UserType As Integer = 15
    Private Const GVCDCF_IntrenallyResolved As Integer = 17

    Private Is_ComboGlobalDictionary As Boolean = False
    Private Is_FormulaEnabled As Boolean = False

    Private Const ActId_DataEdition_CTM As String = "0006"
    Private Const ActId_DataEdition_BABE As String = "0007"

    Public Const Type_BABE As String = "BA-BE"


    Private Const VS_DependencyMedEx As String = "VS_DependencyMedEx"

    Public dtDepedencyMedEx As New DataTable
    Dim Selectvar As String = String.Empty
    Public strTimeDifference As String = String.Empty
    Dim SubmitedDataFlagForTabulerRepetition As Boolean = False
    Private Const Vs_dsReviewerlevel As String = "vs_dsreviewerlevel"
    Private Const vs_workflowstageidfordynamic As String = "vs_workflowstageidfordynamic"
    Dim RepHeader As String = String.Empty
    Dim flagDCF As Boolean = False
    Private CurrentPageSessionVariable As String
    ''Added by Vivek Patel
    Dim dt_EditCheckQuery As New DataTable

#End Region

#Region "Page Load"

    'Add by shivani pandya
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "assignCSS", "  assignCSS();", True)
        Catch ex As Exception
            Throw New Exception("Error while Page_Init()")
        End Try
    End Sub
    'End

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim cVersionStatus As Char
        Dim wstr As String = String.Empty
        Dim ds_DataEntryControl As DataSet = Nothing
        Dim estr As String = String.Empty
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim Return_Val As Char
        Try

            If (hdnCookiesCurrentProfile.Value = "0") Then
                Dim setProfileNew As HttpCookie = New HttpCookie("currentProfile")
                setProfileNew.Value = Request.Cookies("CurrentProfile").Value
                Me.Response.Cookies.Add(setProfileNew)
                Me.hdnCookiesCurrentProfile.Value = Request.Cookies("CurrentProfile").Value
            End If
            '' Me.hdnServerDateTime.Value = Format(CDate(objCommon.GetCurDatetime(Me.Session(S_TimeZoneName))).Date, "dd-MMM-yyyy") Commented by Aaditya on 16-Nov-2015
            Me.HFWorkspaceId.Value = Me.Request.QueryString("WorkSpaceId").ToString
            Me.HFParentNodeId.Value = Me.Request.QueryString("NodeId").ToString
            Me.HFParentActivityId.Value = Me.Request.QueryString("ActivityId").ToString
            Me.HFPeriodId.Value = Me.Request.QueryString("PeriodId").ToString
            Me.HFSubjectId.Value = Me.Request.QueryString("SubjectId").ToString
            Me.HFMySubjectNo.Value = Me.Request.QueryString("MySubjectNo").ToString
            CurrentPageSessionVariable = Me.HFWorkspaceId.Value + Me.HFSubjectId.Value + Me.HFParentNodeId.Value
            If Me.Session(S_UserID) Is Nothing Then
                Response.Redirect("default.aspx?SessionExpire=true", False)
            End If
            Dim ds_Scheduled As DataSet

            If Me.Request.QueryString("UserTypeCode") IsNot Nothing AndAlso
                Me.Session(S_UserType).ToString.Trim <> Me.Request.QueryString("UserTypeCode").ToString Then
                'Session(S_UserType) = Me.Request.QueryString("UserTypeCode").ToString
                Me.hdnuserprofile.Value = Session(S_UserType)
                Me.ViewState(S_UserType) = Me.Request.QueryString("UserTypeCode").ToString
                'Session(S_UserID) = Me.Request.QueryString("UserId").ToString

            Else
                Me.hdnuserprofile.Value = Me.Session(S_UserType).ToString.Trim
                Me.ViewState(S_UserType) = Me.Session(S_UserType)
            End If
            ''Added By Vivek Patel
            hdniUserId.Value = Session(S_UserID)
            hdniUserIP.Value = Request.ServerVariables("REMOTE_ADDR")
            DISoftURL.Value = Convert.ToString(ConfigurationManager.AppSettings.Item("DISoftURL").Trim())


            wstr = HFWorkspaceId.Value + "##" + HFParentNodeId.Value + "##" + HFSubjectId.Value + "##"
            ds_Scheduled = objHelp.ProcedureExecute("dbo.Proc_GetDataVisitTracker", wstr)

            Try
                If Not ds_Scheduled Is Nothing AndAlso ds_Scheduled.Tables(0).Rows.Count > 0 Then
                    Dim dv As DataView = ds_Scheduled.Tables(0).DefaultView
                    dv.RowFilter = "dActualDate IS NOT NULL"
                    If dv.Table().DefaultView.ToTable().Rows.Count > 0 Then

                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "closePage", "window.onunload = CloseWindow();", True)
                    End If
                End If
            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "closePage", "window.onunload = CloseWindow();", True)
                Throw New Exception(ex.Message)
            End Try



            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                Me.HFScreenNo.Value = Me.Request.QueryString("ScreenNo").ToString
            Else
                Me.HFScreenNo.Value = Me.Request.QueryString("SubjectId").ToString
            End If
            'Me.HFWorkFlowStageId.Value = Session(S_WorkFlowStageId)
            Me.lblUserName.Text = Session(S_FirstName).ToString.Trim() + " " + Session(S_LastName).ToString.Trim()
            Me.lblTime.Text = Session(S_Time).ToString
            'Me.lblTime.Text = Convert.ToString(CDate(Session(S_Time)).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)

            Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            If Not IsNothing(Me.Request.QueryString("Mode")) Then
                If Convert.ToString(Me.Request.QueryString("Mode")).Trim() <> "" Then
                    Me.ViewState(VS_Choice) = Me.Request.QueryString("Mode").ToString
                End If
            End If

            If Not IsNothing(Me.Request.QueryString("Type")) Then
                If Convert.ToString(Me.Request.QueryString("Type")).Trim() <> "" Then
                    Me.HFType.Value = Me.Request.QueryString("Type").ToString
                End If
            End If

            Me.trimageinformation.Visible = True
            Me.trReviewCompleted.Style.Add("display", "none")

            If Not Me.GetLegends() Then
                Throw New Exception("Error while set legends... GetLegends")
            End If
            Me.HFWorkFlowStageId.Value = Session(vs_workflowstageidfordynamic)

            If Not closeDiv() Then
                Throw New Exception("Error while set Popup... CloseDiv")
            End If
            Me.HFIsRepeatNo.Value = 0
            BtnExit.Visible = True
            If Not IsNothing(Me.Request.QueryString("From")) AndAlso Convert.ToString(Me.Request.QueryString("From")).Trim().ToUpper = "SCH" Then
                BtnExit.Visible = False
            End If
            'hdnIsPopup.Value = "false"
            If Not IsPostBack() Then
                If Not objHelp.GetData("WorkspaceMst", "isnull(vParentWorkspaceId,vWorkspaceId)as vParentWorkspaceId", " vWorkspaceId='" + Me.HFWorkspaceId.Value.Trim + "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DataEntryControl, estr) Then
                    Throw New Exception("Error while get Parent Project Id")
                End If
                Me.HFParentWorkspaceId.Value = ds_DataEntryControl.Tables(0).Rows(0).Item("vParentWorkspaceId").ToString.Trim()
                ds_DataEntryControl = Nothing
                Me.HFActivityId.Value = Me.Request.QueryString("ActivityId").ToString
                Me.HFNodeId.Value = Me.Request.QueryString("NodeId").ToString

                If Not DisplayHeader() Then
                    Exit Sub
                End If

                Me.trimageinformation.Visible = True
                Me.trReviewCompleted.Style.Add("display", "none")
                Me.BtnSave.Visible = False
                Me.btnSaveAndContinue.Visible = False
                Me.BtnNext.Visible = False
                Me.BtnPrevious.Visible = False

                If Not FillActivities() Then
                    Exit Sub
                End If


                'If Request.Cookies(Me.Request.QueryString("WorkSpaceId").ToString + ":" + Me.Request.QueryString("NodeId").ToString + ":" + Me.Request.QueryString("PeriodId").ToString + ":" + Me.Request.QueryString("SubjectId").ToString + ":" + Me.Request.QueryString("MySubjectNo").ToString) Is Nothing Then
                '    myCookie = New HttpCookie(Me.Request.QueryString("WorkSpaceId").ToString + ":" + Me.Request.QueryString("NodeId").ToString + ":" + Me.Request.QueryString("PeriodId").ToString + ":" + Me.Request.QueryString("SubjectId").ToString + ":" + Me.Request.QueryString("MySubjectNo").ToString)
                '    myCookie("WorkSpaceId") = Me.Request.QueryString("WorkSpaceId").ToString
                '    myCookie("NodeId") = Me.Request.QueryString("NodeId").ToString
                '    myCookie("PeriodId") = Me.Request.QueryString("PeriodId").ToString
                '    myCookie("SubjectId") = Me.Request.QueryString("SubjectId").ToString
                '    myCookie("MySubjectNo") = Me.Request.QueryString("MySubjectNo").ToString
                '    'myCookie.Expires = DateTime.Now.AddDays(-1D)
                '    Response.Cookies.Add(myCookie)
                'Else
                '    myCookie = New HttpCookie(Me.Request.QueryString("WorkSpaceId").ToString + ":" + Me.Request.QueryString("NodeId").ToString + ":" + Me.Request.QueryString("PeriodId").ToString + ":" + Me.Request.QueryString("SubjectId").ToString + ":" + Me.Request.QueryString("MySubjectNo").ToString)
                '    If myCookie.Name.Split(":")(0) = Me.Request.QueryString("Workspaceid") AndAlso myCookie.Name.Split(":")(1) = Me.Request.QueryString("NodeId") AndAlso myCookie.Name.Split(":")(2) = Me.Request.QueryString("PeriodId") AndAlso myCookie.Name.Split(":")(3) = Me.Request.QueryString("SubjectId") AndAlso myCookie.Name.Split(":")(4) = Me.Request.QueryString("MySubjectNo") Then
                '        Me.lblDataEntrycontroller.Text = "Same User"
                '        Me.MpeDataentryControl.Show()
                '        Exit Sub
                '    End If
                'End If
                If IsNothing(Me.Request.QueryString("From")) Then


                    If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                        wstr = "select DataEntryControl.vWorkspaceId,DataEntryControl.iNodeId,DataEntryControl.vSubjectId,DataEntryControl.imodifyBy," +
                                "iWorkFlowStageId,UserMST.vFirstName  + ' '  +UserMST.vLastName as vUserName from DataEntryControl " +
                                "inner join UserMst on (DataEntryControl.imodifyBy=UserMst.iUserId)   " +
                                "Where vSubjectId='" & Me.Request.QueryString("SubjectId").ToString & "' And vWorkspaceId='" & Me.HFWorkspaceId.Value & "' And iNodeId=" & Me.ddlActivities.SelectedValue.Split("#")(1) + " and iWorkFlowStageId = " + Session(S_WorkFlowStageId)

                        ds_DataEntryControl = objHelp.GetResultSet(wstr, "DataEntryControl")

                        If ds_DataEntryControl.Tables(0).Rows.Count > 0 Then


                            If ds_DataEntryControl.Tables(0).Rows(0).Item("iModifyBy") <> Session(S_UserID) AndAlso ds_DataEntryControl.Tables(0).Rows(0).Item("iWorkFlowStageId") = Session(S_WorkFlowStageId) Then
                                'Me.lblDataEntrycontroller.Text = ds_DataEntryControl.Tables(0).Rows(0).Item("vUserName").ToString
                                Me.lblDataEntrycontroller.Text = "For this subject some work is already going on by " + ds_DataEntryControl.Tables(0).Rows(0).Item("vUserName").ToString + ". You Can not continue."
                                Me.MpeDataentryControl.Show()
                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "RemoveCookies({flag:'LOAD',wId:'" + Me.Request.QueryString("WorkSpaceId").ToString + "',nId:'" + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + "',pId:'" + Me.Request.QueryString("PeriodId").ToString + "',sId:'" + Me.Request.QueryString("SubjectId").ToString + "'});", True)
                                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "Open(" + Me.Request.QueryString("WorkSpaceId").ToString + "," + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + "," + Me.Request.QueryString("PeriodId").ToString + "," + Me.Request.QueryString("MySubjectNo").ToString + ");", True)
                                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "Open({flag:'LOAD',wId:'" + Me.Request.QueryString("WorkSpaceId").ToString + "',nId:'" + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + "',pId:'" + Me.Request.QueryString("PeriodId").ToString + "',sId:'" + Me.Request.QueryString("SubjectId").ToString + "'});", True)
                                Exit Sub
                            Else
                                'Me.lblDataEntrycontroller.Text = ds_DataEntryControl.Tables(0).Rows(0).Item("vUserName").ToString
                                'Me.MpeDataentryControl.Show()
                                'Exit Sub
                                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "Open(" + Me.Request.QueryString("WorkSpaceId").ToString + "," + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + "," + Me.Request.QueryString("PeriodId").ToString + "," + Me.Request.QueryString("MySubjectNo").ToString + ");", True)

                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "Open({flag:'LOAD',wId:'" + Me.Request.QueryString("WorkSpaceId").ToString + "',nId:'" + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + "',pId:'" + Me.Request.QueryString("PeriodId").ToString + "',sId:'" + Me.Request.QueryString("SubjectId").ToString + "'});", True)
                            End If

                        Else
                            dr = ds_DataEntryControl.Tables(0).NewRow()
                            dr("vWorkspaceId") = Me.HFWorkspaceId.Value
                            dr("iNodeId") = ddlActivities.SelectedValue.Split("#")(1)
                            dr("vSubjectId") = Me.Request.QueryString("SubjectId").ToString
                            dr("iModifyBy") = Session(S_UserID)
                            dr("iWorkFlowStageId") = Session(S_WorkFlowStageId)

                            ds_DataEntryControl.Tables(0).Rows.Add(dr)
                            ds_DataEntryControl.Tables(0).AcceptChanges()
                            If Not objLambda.insert_DataEntryControl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_DataEntryControl, estr, Return_Val) Then
                                Throw New Exception("Error While saving information of Data Entry Control...PageLoad()..")
                            End If

                            If Return_Val = "Y" Then
                                Me.lblDataEntrycontroller.Text = "For this subject some work is already going on by " + ds_DataEntryControl.Tables(0).Rows(0).Item("vUserName").ToString + ". You Can not continue."
                                Me.MpeDataentryControl.Show()
                                Exit Sub
                            End If
                            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "Open(" + HFWorkspaceId.Value + "," + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + "," + Me.Request.QueryString("PeriodId").ToString + "," + Me.Request.QueryString("SubjectId").ToString.Replace("-", "").ToString + "," + Me.Request.QueryString("MySubjectNo").ToString + ");", True)
                            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "Open(" + Me.Request.QueryString("WorkSpaceId").ToString + "," + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + "," + Me.Request.QueryString("PeriodId").ToString + "," + Me.Request.QueryString("MySubjectNo").ToString + ");", True)
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "Open({flag:'LOAD',wId:'" + Me.Request.QueryString("WorkSpaceId").ToString + "',nId:'" + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + "',pId:'" + Me.Request.QueryString("PeriodId").ToString + "',sId:'" + Me.Request.QueryString("SubjectId").ToString + "'});", True)

                        End If

                    End If

                End If

                If Me.ViewState(VS_DataStatus) = CRF_DataEntryPending Then
                    cVersionStatus = getCRFVersion()
                    If cVersionStatus = "U" Then
                        objCommon.ShowAlert("Project Structure Is Not Freeze,To Do Data Entry Freeze It ", Me.Page)
                        Exit Sub
                    End If
                End If

                If HFSubjectId.Value <> "0000" Then
                    If Me.ViewState(VS_DataStatus) = CRF_DataEntryPending AndAlso
                        Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                        ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                        If Session(vs_workflowstageidfordynamic) = WorkFlowStageId_DataEntry AndAlso Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                            If Not CheckPendingActivity() Then
                                Throw New Exception
                            End If
                        End If
                        ''Commented by nipun khant for dynamic review
                        'If Session(S_WorkFlowStageId) = WorkFlowStageId_DataEntry AndAlso Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                        '    If Not CheckPendingActivity() Then
                        '        Throw New Exception
                        '    End If
                        'End If
                        Exit Sub
                    End If
                End If
            End If

            If Not Me.ViewState(VS_DataStatus) = CRF_DataEntryPending Then
                If Not CRFVersion_ShowUI() Then
                    Throw New Exception("Error while Displaying CRF Version")
                End If
            End If

            'Added by shivani pandya for tabuler repetition
            If Me.hndGridViewStatus.Value.Trim() = "Grid" Then
                Me.Session(S_TabulerRepeatition) = "AllData"
            Else
                Me.Session(S_TabulerRepeatition) = ""
            End If

            If Not GenCall() Then
                CheckVisitStatus(Me.HFWorkspaceId.Value, Me.HFSubjectId.Value, Me.HFParentActivityId.Value, Me.HFParentNodeId.Value, Session(S_UserType))
                Exit Sub
            End If

            If Me.Request.QueryString("UserTypeCode") IsNot Nothing Then
                CheckVisitStatus(Me.HFWorkspaceId.Value, Me.HFSubjectId.Value, Me.HFParentActivityId.Value, Me.HFParentNodeId.Value, Me.Request.QueryString("UserTypeCode").ToString)
            Else
                CheckVisitStatus(Me.HFWorkspaceId.Value, Me.HFSubjectId.Value, Me.HFParentActivityId.Value, Me.HFParentNodeId.Value, Session(S_UserType))
            End If

            If Page.IsPostBack Then
                btnContinueWorking_Click(Nothing, Nothing)
                Exit Sub
            End If

        Catch ex As Exception
            Me.objCommon.ShowAlert(ex.Message + "...Page_Load", Me.Page())
        End Try

    End Sub

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Dim S_RepeatationShow As String = String.Empty
        ' Me.ViewState(VS_DataStatus) = CRF_DataEntryCompleted
        Try
            If Me.HFSubjectId.Value.Trim() = "" Then
                Me.ddlRepeatNo.Items.Clear()
            End If
            If Me.Session(S_TabulerRepeatition) <> "AllData" Then
                If Me.Session(S_GetLetestData + CurrentPageSessionVariable + hdnActivityName.Value) <> "getData" Then
                    If Me.hndRepetitionNo.Value.Trim() <> "" And Me.hndRepetitionNo.Value.Trim() <> "N" Then
                        Me.Session(S_SelectedRepeatation + CurrentPageSessionVariable + hdnActivityName.Value) = Nothing
                        Me.Session(S_SelectedRepeatation + CurrentPageSessionVariable + hdnActivityName.Value) = Me.hndRepetitionNo.Value.Trim()
                    Else
                        If Me.ddlRepeatNo.SelectedValue <> "N" Then
                            Me.hndRepetitionNo.Value = Me.Session(S_SelectedRepeatation + CurrentPageSessionVariable + hdnActivityName.Value)
                        End If
                    End If
                End If
            End If

            If Me.hndLetestData.Value.Trim() <> "" Then
                Me.Session(S_RepeatationShow) = Me.hndLetestData.Value.Trim()
            Else
                Me.hndLetestData.Value = Me.Session(S_RepeatationShow)
            End If

            If Not GenCall_Data() Then
                Return False
            End If

            If Not GenCall_ShowUI() Then
                Return False
            End If

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
        Dim ds_CRFSubjectHdrDtl As DataSet = Nothing
        Dim dv_CRFSubjectHdrDtl As DataView
        Dim estr_retu As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim ds_CRFHdrDtlSubDtl As New DataSet
        Dim dt As DataTable = Nothing
        Dim dt_CRFHdrDtlSubDtl As DataTable
        Dim dv As DataView
        Dim dtData As New DataTable
        Dim clr As String = Drawing.ColorTranslator.ToHtml(Drawing.Color.Red)
        Dim dv_review As New DataView
        Dim ds_review As New DataSet

        Try
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                If Not Me.objHelp.GetDCFMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty,
                            ds, estr_retu) Then
                    Throw New Exception(estr_retu)
                End If
                Me.ViewState(VS_DtDCF) = ds.Tables(0).Copy()

                ds = Nothing
                ds = New DataSet

                If Not objHelp.GetCRFHdr("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds, estr_retu) Then
                    Throw New Exception(estr_retu)
                End If
                Me.ViewState(VS_DtCRFHdr) = ds.Tables(0).Copy()

                ds = Nothing
                ds = New DataSet

                If Not objHelp.GetCRFDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds, estr_retu) Then
                    Throw New Exception(estr_retu)
                End If
                Me.ViewState(VS_DtCRFDtl) = ds.Tables(0).Copy()

                ds = Nothing
                ds = New DataSet

                If Not objHelp.GetCRFSubDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds, estr_retu) Then
                    Throw New Exception(estr_retu)
                End If
                Me.ViewState(VS_DtCRFSubDtl) = ds.Tables(0).Copy()
            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                If Not Me.objHelp.GetDCFMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty,
                            ds, estr_retu) Then
                    Throw New Exception(estr_retu)
                End If
                Me.ViewState(VS_DtDCF) = ds.Tables(0).Copy()
            End If

            If Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso
                        (Me.Request.QueryString("SubjectId").ToString.Trim() <> "") Then

                If Me.ddlRepeatNo.Items.Count <= 0 Then
                    If Not fillRepeatation() Then
                        Throw New Exception("Error while Filling repetation.. FillRepeatition")
                    End If
                End If
                If Not Session(S_SelectedRepeatation + CurrentPageSessionVariable) Is Nothing And Me.hndRepetitionNo.Value.Trim() <> "N" And Me.Session(S_TabulerRepeatition) <> "AllData" Then
                    Me.HFIsRepeatNo.Value = 1
                End If
            End If

            If Me.ddlRepeatNo.Items.Count > 0 AndAlso Me.ddlRepeatNo.SelectedValue.ToUpper.Trim() <> "N" Then 'For Old

                'If CType(Me.ViewState(VS_dtMedEx_Fill), DataTable) Is Nothing Then


                'Commented by shivani pandya for latest repeatition

                'Wstr = "cActiveFlag<>'N' and vWorkSpaceId='" + Me.HFWorkspaceId.Value.Trim() + "'" + _
                '        " and vActivityId='" & Me.HFActivityId.Value.Trim() & "' and vSubjectId='" & _
                '        Me.HFSubjectId.Value.Trim() & "'" & _
                '    " and iPeriod=" & Me.HFPeriodId.Value.Trim() & " And iNodeId=" & _
                '        Me.HFNodeId.Value.Trim()

                'If ddlRepeatNo.SelectedIndex > 2 Then
                '    Wstr += " And iRepeatNo= " & ddlRepeatNo.SelectedValue & ""
                'End If

                'Wstr += "  Order by RepetitionNo,iSeqNo OPTION (MAXDOP 1)"
                'If Not Me.objHelp.View_CRFHdrDtlSubDtl_Edit(Wstr, "*,DENSE_RANK() OVER(PARTITION BY View_CRFHdrDtlSubDtl_Edit.nCRFHdrNo,View_CRFHdrDtlSubDtl_Edit.vActivityId,View_CRFHdrDtlSubDtl_Edit.vSubjectId ORDER BY View_CRFHdrDtlSubDtl_Edit.nCRFHdrNo,View_CRFHdrDtlSubDtl_Edit.vActivityId,View_CRFHdrDtlSubDtl_Edit.vSubjectId,View_CRFHdrDtlSubDtl_Edit.iRepeatNo) as [RepetitionNo] ", ds_CRFSubjectHdrDtl, estr_retu) Then
                '    Throw New Exception(estr_retu)
                'End If

                If hndGridViewStatus.Value.Trim() = "Grid" Or Me.Session(S_GetLetestData + CurrentPageSessionVariable + hdnActivityName.Value) = "getData" Then
                    hndRepetitionNo.Value = ""
                    For Each item As ListItem In ddlRepeatNo.Items
                        If item.Value <> "N" Then
                            hndRepetitionNo.Value = hndRepetitionNo.Value + item.Value + ","
                        End If
                    Next
                End If

                If (Request.QueryString("Repeatation") <> "") Then
                    hndRepetitionNo.Value = Request.QueryString("Repeatation")
                    ddlRepeatNo.ClearSelection()
                    ddlRepeatNo.SelectedIndex = hndRepetitionNo.Value
                    ddlRepeatNo.SelectedValue = hndRepetitionNo.Value
                    ddlRepeatNo.Enabled = False
                End If

                Wstr = Me.HFWorkspaceId.Value.Trim() + "##" + Me.HFActivityId.Value.Trim() + "##" + Me.HFSubjectId.Value.Trim() + "##" + Me.HFPeriodId.Value.Trim() + "##" + Me.HFNodeId.Value.Trim() + "##" + hndRepetitionNo.Value.Trim()
                ds_CRFSubjectHdrDtl = objHelp.ProcedureExecute("Proc_SelectedRepeatition", Wstr)

                'If Me.Session(S_GetLetestData) = "getData" Then
                '    dtData = ds_CRFSubjectHdrDtl.Tables(0)
                '    dtData.DefaultView.RowFilter = "cCRFDtlDataStatus = 'D'"
                '    hndRepetitionNo.Value = ""
                '    For Each dr As DataRow In dtData.DefaultView.ToTable(True, "RepetitionNo").Rows
                '        hndRepetitionNo.Value = hndRepetitionNo.Value + dr("RepetitionNo").ToString() + ","
                '    Next
                '    Me.ViewState(VS_dtMedEx_Fill) = dtData.DefaultView.ToTable()
                'Else
                '    Me.ViewState(VS_dtMedEx_Fill) = ds_CRFSubjectHdrDtl.Tables(0).Copy()
                'End If    
                'Dim dv_ActivityStatus As DataView

                'If Not ds_CRFSubjectHdrDtl Is Nothing Then
                '    dv_ActivityStatus = ds_CRFSubjectHdrDtl.Tables(0).DefaultView
                'End If

                'If dv_ActivityStatus.ToTable().Rows.Count > 0 Then

                '    If Not hndRepetitionNo.Value Is Nothing AndAlso Convert.ToString(Me.hndRepetitionNo.Value.ToUpper()) <> "N" Then
                '        dv_ActivityStatus.RowFilter = "iNodeId = " + Me.HFNodeId.Value.Trim() + " And RepetitionNo In ( " & dv_ActivityStatus.ToTable().Compute("Max(RepetitionNo)", String.Empty) & ")"
                '        If dv_ActivityStatus.ToTable().Rows.Count > 0 Then
                '            Me.HFReviewedWorkFlowId.Value = dv_ActivityStatus.ToTable().Rows(0)("iWorkFlowStageId")
                '            Me.HFImportedDataWorkFlowId.Value = dv_ActivityStatus.ToTable().Rows(0)("iWorkFlowStageId")
                '            Me.ViewState(VS_DataStatus) = dv_ActivityStatus.ToTable().Rows(0)("cCRFDtlDataStatus")
                '            If Convert.ToString(dv_ActivityStatus.ToTable().Rows(0)("CRFReviewedBy")).Trim() <> "" AndAlso Convert.ToString(dv_ActivityStatus.ToTable().Rows(0)("cCRFDtlDataStatus")).Trim() <> CRF_Review Then
                '                Me.lblLastReviewedBy.Text = "Last Review Done By : " + Convert.ToString(dv_ActivityStatus.ToTable().Rows(0)("CRFReviewedBy")).Trim() + " - " + Convert.ToString(dv_ActivityStatus.ToTable().Rows(0)("CRFReviewedByUserType")).Trim()
                '            End If
                '        End If
                '    Else
                '        'dv_ActivityStatus.Sort = "iRepeatNo asc"
                '        'Me.HFReviewedWorkFlowId.Value = dv_ActivityStatus.ToTable().Rows(0)("iReviewedWorkFlowStageId")
                '        dv_ActivityStatus.Sort = "RepetitionNo,dModifyOnWorkflowDtl Desc"   ''Added By Rahul Shah
                '        Me.HFReviewedWorkFlowId.Value = dv_ActivityStatus.ToTable().Rows(0)("iReviewedWorkFlowStageId")
                '        Me.HFImportedDataWorkFlowId.Value = dv_ActivityStatus.ToTable().Rows(0)("iWorkFlowStageId")
                '        Me.ViewState(VS_DataStatus) = dv_ActivityStatus.ToTable().Rows(0)("cCRFDtlDataStatus")
                '        If Convert.ToString(dv_ActivityStatus.ToTable().Rows(0)("CRFReviewedBy")).Trim() <> "" AndAlso Convert.ToString(dv_ActivityStatus.ToTable().Rows(0)("cCRFDtlDataStatus")).Trim() <> CRF_Review Then
                '            Me.lblLastReviewedBy.Text = "Last Review Done By : " + Convert.ToString(dv_ActivityStatus.ToTable().Rows(0)("CRFReviewedBy")).Trim() + " - " + Convert.ToString(dv_ActivityStatus.ToTable().Rows(0)("CRFReviewedByUserType")).Trim()
                '        End If
                '    End If
                'End If




                Me.ViewState(VS_dtMedEx_Fill) = ds_CRFSubjectHdrDtl.Tables(0).Copy()
                Me.ViewState(VS_dtMedEx_Fill_Backup) = ds_CRFSubjectHdrDtl.Tables(0).Copy()

                '====================Added By PRatik Soni FOR DMS==========================
                If Not IsNothing(Me.Request.QueryString("vmedexResult")) Then

                    Dim ds_temp As New DataSet
                    ds_temp = ds_CRFSubjectHdrDtl.Clone
                    ds_temp.Tables(0).ImportRow(ds_CRFSubjectHdrDtl.Tables(0).Rows(0))
                    Me.ViewState(VS_dtMedEx_Fill) = ds_temp.Tables(0).Copy()
                    Me.ViewState(VS_dtMedEx_Fill_Backup) = ds_temp.Tables(0).Copy()

                End If
                '==========================================================================

                ' End If
                If ds_CRFSubjectHdrDtl Is Nothing Then
                    Me.ViewState(VS_dtMedEx_Fill) = CType(Me.ViewState(VS_dtMedEx_Fill_Backup), DataTable).Copy()
                    dt_CRFHdrDtlSubDtl = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Copy()
                    ds_CRFSubjectHdrDtl = New DataSet
                    ds_CRFSubjectHdrDtl.Tables.Add(dt_CRFHdrDtlSubDtl)
                End If
                If ds_CRFSubjectHdrDtl.Tables(0).Rows.Count > 0 Then

                    dt = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Copy()
                    If Me.hndLetestData.Value.Trim <> "GetAuditTrail" Then
                        dt.DefaultView.RowFilter = " RepetitionNo=  " & ddlRepeatNo.SelectedValue
                    End If

                    If dt.DefaultView.ToTable.Rows.Count <= 0 Then
                        Me.ViewState(VS_dtMedEx_Fill) = Nothing
                        Me.ViewState(VS_dtMedEx_Fill) = CType(Me.ViewState(VS_dtMedEx_Fill_Backup), DataTable).Copy()
                        dt = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Copy()
                        dt.DefaultView.RowFilter = " RepetitionNo= " & ddlRepeatNo.SelectedValue
                    End If

                    Me.HFMedexInfoDtlTranNo.Value = dt.DefaultView.ToTable.Rows(0).Item("iTranNo")
                    Me.HFCRFHdrNo.Value = dt.DefaultView.ToTable.Rows(0).Item("nCRFHdrNo")
                    Me.HFCRFDtlNo.Value = dt.DefaultView.ToTable.Rows(0).Item("nCRFDtlNo")
                    Me.HFCRFDtlLockStatus.Value = dt.DefaultView.ToTable.Rows(0).Item("cCRFDtlLockStatus")
                    Me.ViewState(VS_DataStatus) = dt.DefaultView.ToTable.Rows(0).Item("cCRFDtlDataStatus")
                    Me.ViewState(VS_DataStatus) = dt.DefaultView.ToTable.Rows(0).Item("cCRFDtlDataStatus")

                    If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                    End If

                    dv = ds_CRFSubjectHdrDtl.Tables(0).DefaultView



                    'dv.RowFilter = "(cDCFStatus = '" + Discrepancy_Generated + "' Or cDCFStatus = '" + Discrepancy_Answered _
                    '                    + "') And ((( vDCFByProfile = '" + Me.Session(S_UserType) + _
                    '                    "' Or DCFByWorkFlowStageId < " + Me.Session(S_WorkFlowStageId) _
                    '                    + ") And cDCFType = 'M') Or (cDCFType = 'S')) And DCFByWorkFlowStageId <> " & WorkFlowStageId_OnlyView
                    'Me.HFDCFCount.Value = dv.ToTable.Rows.Count
                    ' Me.HFDCFCount.Value = ds_DCF.Tables(0).Rows.Count

                    ''For showing data entry completed data
                    'dv_CRFSubjectHdrDtl = ds_CRFSubjectHdrDtl.Tables(0).DefaultView
                    'dv_CRFSubjectHdrDtl.RowFilter = "cCRFDtlDataStatus <> '" + CRF_DataEntry + "'"
                    'Me.ViewState(VS_dtMedEx_Fill) = dv_CRFSubjectHdrDtl.ToTable()
                    ''*************
                    'For showing data entry continue data


                    If Session(S_DataStatus) <> "" Then
                        Session(S_DataStatus) = Nothing
                        ds_CRFSubjectHdrDtl = New DataSet
                        ds_CRFSubjectHdrDtl.Tables.Add(CType(Me.ViewState(VS_dtMedEx_Fill_Backup), DataTable).Copy())
                        dv_CRFSubjectHdrDtl = ds_CRFSubjectHdrDtl.Tables(0).DefaultView
                        If (Me.Session(S_GetLetestData + CurrentPageSessionVariable + hdnActivityName.Value) = "getData") Then

                        Else
                            dv_CRFSubjectHdrDtl.RowFilter = "cCRFDtlDataStatus <> 'B' "
                        End If


                        Me.ViewState(VS_dtMedEx_Fill) = dv_CRFSubjectHdrDtl.ToTable()
                        If dv_CRFSubjectHdrDtl.ToTable().Rows.Count > 0 Then
                            Me.HFMedexInfoDtlTranNo.Value = dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("iTranNo")
                            Me.HFCRFHdrNo.Value = dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("nCRFHdrNo")
                            Me.HFCRFDtlNo.Value = dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("nCRFDtlNo")
                            Me.HFCRFDtlLockStatus.Value = dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("cCRFDtlLockStatus")
                            Me.ViewState(VS_DataStatus) = dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("cCRFDtlDataStatus")
                            If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                            End If
                            dv = dv_CRFSubjectHdrDtl.ToTable.DefaultView
                            'dv.RowFilter = "(cDCFStatus = '" + Discrepancy_Generated + "' Or cDCFStatus = '" + Discrepancy_Answered _
                            '            + "') And ((( vDCFByProfile = '" + Me.Session(S_UserType) + _
                            '            "' Or DCFByWorkFlowStageId < " + Me.Session(S_WorkFlowStageId) _
                            '            + ") And cDCFType = 'M') Or (cDCFType = 'S')) And DCFByWorkFlowStageId <> " & WorkFlowStageId_OnlyView
                            'Wstr = "(cDCFStatus = '" + Discrepancy_Generated + "' Or cDCFStatus = '" + Discrepancy_Answered _
                            '           + "') And ((( vUserTypeCode = '" + Me.Session(S_UserType) + _
                            '           "' Or CRFWorkflowBy < " + Me.Session(S_WorkFlowStageId) _
                            '           + ") And cDCFType = 'M') Or (cDCFType = 'S')) And CRFWorkflowBy <> " & WorkFlowStageId_OnlyView + _
                            '           " And nCRFDtlNo =" + Me.HFCRFDtlNo.Value.Trim()

                            'If Not objHelp.GetData("VIEW_DCFMst_Optimized", "*", Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DCF, estr_retu) Then
                            '    Throw New Exception("Error while getting status of DCF..." + estr_retu)
                            'End If
                            ''Me.HFDCFCount.Value = dv.ToTable.Rows.Count
                            'Me.HFDCFCount.Value = ds_DCF.Tables(0).Rows.Count
                        Else
                            objCommon.ShowAlert("There is no any submitted Data for this Subject", Me.Page)
                            hndRepetitionNo.Value = Me.ddlRepeatNo.SelectedValue
                            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "getRepeatitionDropDownColor", "getRepeatitionDropDownColor();", True)
                            SubmitedDataFlagForTabulerRepetition = True
                            Return False
                        End If

                    Else
                        dv_CRFSubjectHdrDtl = ds_CRFSubjectHdrDtl.Tables(0).DefaultView
                        If Me.hndLetestData.Value.Trim <> "GetAuditTrail" And Me.hndGridViewStatus.Value.Trim() <> "Grid" And Me.Session("ViewAll") <> Nothing AndAlso Me.Session("ViewAll").ToString() <> "Y" Then
                            dv_CRFSubjectHdrDtl.RowFilter = "RepetitionNo  = " + Me.ddlRepeatNo.SelectedValue
                        End If

                        If dv_CRFSubjectHdrDtl.ToTable().Rows.Count > 0 Then

                            If dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("cCRFDtlDataStatus") = CRF_DataEntry Then
                                Me.ViewState(VS_dtMedEx_Fill) = dv_CRFSubjectHdrDtl.ToTable()
                                Me.HFMedexInfoDtlTranNo.Value = dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("iTranNo")
                                Me.HFCRFHdrNo.Value = dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("nCRFHdrNo")
                                Me.HFCRFDtlNo.Value = dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("nCRFDtlNo")
                                Me.HFCRFDtlLockStatus.Value = dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("cCRFDtlLockStatus")
                                Me.ViewState(VS_DataStatus) = dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("cCRFDtlDataStatus")
                                If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                                    Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                                End If
                                dv = dv_CRFSubjectHdrDtl.ToTable.DefaultView
                                'dv.RowFilter = "(cDCFStatus = '" + Discrepancy_Generated + "' Or cDCFStatus = '" + Discrepancy_Answered _
                                '            + "') And ((( vDCFByProfile = '" + Me.Session(S_UserType) + _
                                '            "' Or DCFByWorkFlowStageId < " + Me.Session(S_WorkFlowStageId) _
                                '            + ") And cDCFType = 'M') Or (cDCFType = 'S')) And DCFByWorkFlowStageId <> " & WorkFlowStageId_OnlyView
                                'Me.HFDCFCount.Value = dv.ToTable.Rows.Count
                                'Wstr = "(cDCFStatus = '" + Discrepancy_Generated + "' Or cDCFStatus = '" + Discrepancy_Answered _
                                '      + "') And ((( vUserTypeCode = '" + Me.Session(S_UserType) + _
                                '      "' Or CRFWorkflowBy < " + Me.Session(S_WorkFlowStageId) _
                                '      + ") And cDCFType = 'M') Or (cDCFType = 'S')) And CRFWorkflowBy <> " & WorkFlowStageId_OnlyView + _
                                '      " And nCRFDtlNo =" + Me.HFCRFDtlNo.Value.Trim()

                                'If Not objHelp.GetData("VIEW_DCFMst_Optimized", "*", Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DCF, estr_retu) Then
                                '    Throw New Exception("Error while getting status of DCF..." + estr_retu)
                                'End If
                                'Me.HFDCFCount.Value = ds_DCF.Tables(0).Rows.Count
                            Else

                                Me.HFMedexInfoDtlTranNo.Value = dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("iTranNo")
                                Me.HFCRFHdrNo.Value = dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("nCRFHdrNo")
                                Me.HFCRFDtlNo.Value = dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("nCRFDtlNo")
                                Me.HFCRFDtlLockStatus.Value = dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("cCRFDtlLockStatus")
                                Me.ViewState(VS_DataStatus) = dv_CRFSubjectHdrDtl.ToTable.Rows(0).Item("cCRFDtlDataStatus")
                                If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                                    Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                                End If
                                dv = dv_CRFSubjectHdrDtl.ToTable.DefaultView
                                'dv.RowFilter = "(cDCFStatus = '" + Discrepancy_Generated + "' Or cDCFStatus = '" + Discrepancy_Answered _
                                '            + "') And ((( vDCFByProfile = '" + Me.Session(S_UserType) + _
                                '            "' Or DCFByWorkFlowStageId < " + Me.Session(S_WorkFlowStageId) _
                                '            + ") And cDCFType = 'M') Or (cDCFType = 'S')) And DCFByWorkFlowStageId <> " & WorkFlowStageId_OnlyView
                                'Me.HFDCFCount.Value = dv.ToTable.Rows.Count
                                'Wstr = "(cDCFStatus = '" + Discrepancy_Generated + "' Or cDCFStatus = '" + Discrepancy_Answered _
                                '      + "') And ((( vUserTypeCode = '" + Me.Session(S_UserType) + _
                                '      "' Or CRFWorkflowBy < " + Me.Session(S_WorkFlowStageId) _
                                '      + ") And cDCFType = 'M') Or (cDCFType = 'S')) And CRFWorkflowBy <> " & WorkFlowStageId_OnlyView + _
                                '      " And nCRFDtlNo =" + Me.HFCRFDtlNo.Value.Trim()

                                'If Not objHelp.GetData("VIEW_DCFMst_Optimized", "*", Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DCF, estr_retu) Then
                                '    Throw New Exception("Error while getting status of DCF..." + estr_retu)
                                'End If

                                'Me.HFDCFCount.Value = ds_DCF.Tables(0).Rows.Count

                            End If
                        End If
                    End If
                    '****************

                    'Add by shivani pandya

                    If btnGridViewDisplay.Visible = True And Me.hndGridViewStatus.Value.Trim() = "Grid" Then
                        Me.HFCRFDtlNo.Value = dt.DefaultView.ToTable.Rows(0).Item("nCRFDtlNo")
                    End If
                    'end

                    Me.btnReviewHistory.Visible = True
                    Me.btnReviewHistory.OnClientClick = "return show_popup('frmCRFReviewHistory.aspx?CRFDtlNo=" + Me.HFCRFDtlNo.Value.Trim() + "')"

                End If
                If CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_DataEntry AndAlso
                                   Me.Session(S_WorkFlowStageId) = WorkFlowStageId_DataEntry AndAlso
                                   CType(Me.ViewState(VS_Choice), String).ToUpper() <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    If Me.hndRepetitionNo.Value.Split(",").Length = 1 Then
                        Me.BtnSave.Visible = True
                        Me.btnSaveAndContinue.Visible = True
                    End If
                End If

                If CType(Me.ViewState(VS_DataStatus), String).ToUpper() <> CRF_DataEntry Then
                    Me.BtnSave.Visible = False
                    Me.btnSaveAndContinue.Visible = False
                    'Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View
                    If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                    End If
                End If

                Me.trReviewCompleted.Visible = True
                Me.trReviewCompleted.Disabled = False
                Me.btnOk.Visible = True
                Me.chkReviewCompleted.Checked = False
                '*************Checking for Lock Status of CRFDtl************
                If Not Me.HFCRFDtlLockStatus.Value Is Nothing AndAlso
                       Convert.ToString(Me.HFCRFDtlLockStatus.Value).Trim.ToUpper() = CRFHdr_Locked Then
                    Me.chkReviewCompleted.Checked = True
                    Me.trReviewCompleted.Disabled = True
                    Me.trReviewCompleted.Visible = False
                    Me.btnOk.Visible = False
                End If
                '*************************************

                '************Showing Edit Checks Queries*******************
                If Not IsPostBack Then
                    If Not Me.fillQueryGrid() Then
                        Throw New Exception("Error While Filling Query Grid.")
                    End If

                    If Not ShowNoOfQuery() Then
                        Throw New Exception("Error While Filling Edit checks...GenCall_Data")
                    End If
                    ''Added By Vivek Patel
                    'If Not ShowEditCheckQuery() Then
                    '    Throw New Exception("Error While Filling Edit checks...GenCall_Data")
                    'End If
                End If
                '**********************************************************
                ''Added By Vivek Patel
                If Not ShowEditCheckQuery() Then
                    Throw New Exception("Error While Filling Attribute Wise Edit checks")
                End If
            ElseIf Me.ddlRepeatNo.Items.Count > 0 AndAlso Me.ddlRepeatNo.SelectedValue.ToUpper.Trim() = "N" Then 'For New

                Wstr = "cActiveFlag<>'N' and vWorkSpaceId='" & Me.HFParentWorkspaceId.Value.Trim() & "'" &
                        " and vActivityId='" & Me.HFActivityId.Value.Trim() & "'" &
                        " And iNodeId=" &
                        Me.HFNodeId.Value.Trim() & " AND  cIsVisibleFront <> 'N' Order by iSeqNo"

                If Not Me.objHelp.GetViewMedExWorkSpaceDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr_retu) Then
                    Throw New Exception(estr_retu)
                End If
                Me.ViewState(VS_dtMedEx_Fill) = ds.Tables(0).Copy()
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

                Me.chkReviewCompleted.Checked = False
                Me.btnOk.Visible = False
                Me.trReviewCompleted.Disabled = True
                Me.trReviewCompleted.Visible = False

                'Changed as when data entry is pending then GetViewMedExWorkSpaceDtl is used for crf
                ' And so status should be CRF_DataEntryPending and not CRF_DataEntry. -Pratiksha
                'Me.ViewState(VS_DataStatus) = CRF_DataEntry
                Me.ViewState(VS_DataStatus) = CRF_DataEntryPending
                Me.BtnSave.Visible = True
                Me.btnSaveAndContinue.Visible = True

                If Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_DataEntry Or Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    Me.BtnSave.Visible = False
                    Me.btnSaveAndContinue.Visible = False
                End If

            End If

            'Added by Vimal Ghoniya
            'Wstr = " SELECT * FROM View_MedExWorkspaceDependency WHERE vWorkspaceId = '" + Me.HFWorkspaceId.Value + "' AND vSourceActivityId = '" + Me.HFActivityId.Value.Trim() + "' AND iSourceNodeId ='" + Me.ddlActivities.SelectedValue.Split("#")(1).Trim.ToString() + "'  AND cDependencyType = 'F'"

            ''aaded by ketan for Dependacy Issue
            Wstr = " SELECT * FROM View_MedExWorkspaceDependency WHERE vWorkspaceId = '" + Me.HFWorkspaceId.Value + "' AND vSourceActivityId = '" + Me.HFActivityId.Value.Trim() + "' AND cDependencyType = 'F' AND iNodeId= " + Me.ddlActivities.SelectedItem.Value.Split("#")(1).ToString() + " "
            ds = Me.objHelp.GetResultSet(Wstr, "Dependency")

            If Not ds Is Nothing Then
                dtDepedencyMedEx = ds.Tables(0)
            End If

            'Me.ViewState(VS_DependencyMedEx) = dsDe.Tables(0)

            'End

            If Not Me.IsPostBack() Then
                ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                If Me.Session(vs_workflowstageidfordynamic) <> WorkFlowStageId_MedicalCoding Then
                    ''Commented by nipun khant for dynamic review
                    'If Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_MedicalCoding Then
                    ''Commented by nipun khant for dynamic review 
                    'If Me.Session(S_WorkFlowStageId) > WorkFlowStageId_DataEntry AndAlso (Me.ViewState(VS_DataStatus) = CRF_DataEntry Or Me.ViewState(VS_DataStatus) = CRF_DataEntryPending) Then
                    ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                    If Me.Session(vs_workflowstageidfordynamic) > WorkFlowStageId_DataEntry AndAlso (Me.ViewState(VS_DataStatus) = CRF_DataEntry Or Me.ViewState(VS_DataStatus) = CRF_DataEntryPending) Then
                        If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                            Me.BtnSave.Visible = False
                            Me.btnSaveAndContinue.Visible = False
                            Me.trReviewCompleted.Visible = False
                            If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View
                                'Me.objCommon.ShowAlert("Data Entry Or Review Is Not Done By Lower Stage For Selected Activity.", Me.Page)
                                'Exit Function
                            End If
                        End If

                    ElseIf Not Me.HFReviewedWorkFlowId.Value Is Nothing AndAlso Convert.ToString(Me.HFReviewedWorkFlowId.Value).Trim() <> "" Then

                        ' Condition changed for view mode -- Pratiksha
                        ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                        If Convert.ToString(Me.Session(vs_workflowstageidfordynamic)) <> WorkFlowStageId_OnlyView Or WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View <> Me.ViewState(VS_Choice) Then
                            ''Commented by nipun khant for dynamic review
                            'If Convert.ToString(Me.Session(S_WorkFlowStageId)) <> WorkFlowStageId_OnlyView Or WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View <> Me.ViewState(VS_Choice) Then
                            If Convert.ToString(Me.HFReviewedWorkFlowId.Value) <> WorkFlowStageId_DataEntry Then
                                ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                                If Convert.ToInt32(Me.HFReviewedWorkFlowId.Value) = Convert.ToInt32(Me.Session(vs_workflowstageidfordynamic)) Or
                                    Convert.ToInt32(Me.HFReviewedWorkFlowId.Value) > Convert.ToInt32(Me.Session(vs_workflowstageidfordynamic)) Then

                                    ''Commented by nipun khant for dynamic review
                                    'If Convert.ToInt32(Me.HFReviewedWorkFlowId.Value) = Convert.ToInt32(Me.Session(S_WorkFlowStageId)) Or _
                                    'Convert.ToInt32(Me.HFReviewedWorkFlowId.Value) > Convert.ToInt32(Me.Session(S_WorkFlowStageId)) Then
                                    'Me.objCommon.ShowAlert("Review Already Done For Selected Activity.", Me.Page)
                                    Me.ViewState(VS_ReviewFlag) = "YES"
                                    Me.BtnSave.Visible = False
                                    Me.btnSaveAndContinue.Visible = False
                                    Return True
                                End If

                            End If

                            If Convert.ToString(Me.HFReviewedWorkFlowId.Value) = WorkFlowStageId_DataEntry AndAlso
                                Me.ddlRepeatNo.SelectedValue.Trim.ToUpper() <> "N" Then
                                Me.ViewState(VS_ReviewFlag) = "NO"
                                ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                                'Me.Session(S_WorkFlowStageId) = WorkFlowStageId_DataEntry AndAlso _ ''Commented by nipun khant for dynamic review
                                If CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_DataEntry AndAlso
                                    Me.Session(vs_workflowstageidfordynamic) = WorkFlowStageId_DataEntry AndAlso
                                    CType(Me.ViewState(VS_Choice), String).ToUpper() <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                                    Me.BtnSave.Visible = True
                                    Me.btnSaveAndContinue.Visible = True
                                    Me.trReviewCompleted.Visible = False
                                Else
                                    ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                                    If Convert.ToInt32(Me.HFReviewedWorkFlowId.Value) <= (Convert.ToInt32(Me.Session(vs_workflowstageidfordynamic)) - 10) Then
                                        ''Commented by nipun khant for dynamic review
                                        'If Convert.ToInt32(Me.HFReviewedWorkFlowId.Value) <= (Convert.ToInt32(Me.Session(S_WorkFlowStageId)) - 10) Then
                                        Me.BtnSave.Visible = False
                                        Me.btnSaveAndContinue.Visible = False
                                        If CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_DataEntry Then
                                            Me.trReviewCompleted.Visible = False
                                        End If
                                    End If
                                End If
                            End If

                            ' Condition changed for view mode -- pratiksha
                            ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic

                            If Convert.ToInt32(Me.HFReviewedWorkFlowId.Value) < (Convert.ToInt32(Me.Session(vs_workflowstageidfordynamic)) - 10) And Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                                If Convert.ToInt32(Me.HFImportedDataWorkFlowId.Value) = (Convert.ToInt32(Me.Session(vs_workflowstageidfordynamic))) Then
                                    'This is done only for activity of lab data import, because from import lab data page, only entries are being made and no review process is done.
                                    Return True
                                End If
                                'Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View
                            End If

                            ''Commented by nipun khant for dynamic review
                            '================================================
                            'If Convert.ToInt32(Me.HFReviewedWorkFlowId.Value) < (Convert.ToInt32(Me.Session(S_WorkFlowStageId)) - 10) And Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

                            '    If Convert.ToInt32(Me.HFImportedDataWorkFlowId.Value) = (Convert.ToInt32(Me.Session(S_WorkFlowStageId))) Then
                            '        'This is done only for activity of lab data import, because from import lab data page, only entries are being made and no review process is done.
                            '        Return True
                            '    End If

                            '    Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View
                            '    'Me.objCommon.ShowAlert("Data Entry Or Review Is Not Done By Lower Stage For Selected Activity.", Me.Page)
                            '    'Exit Function

                            'End If
                            '================================================

                        Else
                            Me.trReviewCompleted.Visible = False
                        End If
                    End If

                    ''Commented by nipun khant for dynamic review
                    'ElseIf Me.Session(S_WorkFlowStageId) = WorkFlowStageId_MedicalCoding Then
                    ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic

                ElseIf Me.Session(vs_workflowstageidfordynamic) = WorkFlowStageId_MedicalCoding Then
                    Me.trReviewCompleted.Visible = False
                End If
            End If

            'edit by vishal
            Dim ds_Check As New DataSet

            If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Wstr = "vWorkspaceId = '" + Me.HFWorkspaceId.Value + "' And cStatusIndi <> 'D' Order by iTranNo desc"
                If Not Me.objHelp.GetCRFLockDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                    ds_Check, estr_retu) Then
                    Throw New Exception(estr_retu)
                End If
                If Not ds_Check Is Nothing Then
                    ' edited by vishal for lock/unlock site
                    If ds_Check.Tables(0).Rows.Count > 0 Then
                        If Convert.ToString(ds_Check.Tables(0).Rows(0)("cLockFlag")).Trim.ToUpper() = CRFHdr_Locked Then
                            Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View
                        End If
                    End If
                End If
            End If

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message.ToString, estr_retu)
            Return False
        End Try
    End Function

#End Region

#Region "DisplayHeader"

    Private Function DisplayHeader() As Boolean
        Dim Wstr As String = String.Empty
        Dim ds_Heading As New DataSet
        Dim estr As String = String.Empty
        Dim ds_WorkspaceNodeDetail As New DataSet
        Dim dt_heading As New DataTable
        Dim vMysubjectId As String = String.Empty

        Dim ds_WorkspaceSubjectMst As New DataSet
        Try

            Wstr = "vWorkspaceId='" & Me.HFWorkspaceId.Value.Trim() & "' and cStatusIndi <> 'D'"

            If Not Me.objHelp.GetWorkspaceProtocolDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition _
                                    , ds_Heading, estr) Then
                Throw New Exception("Error while getting Header information. " + estr)
            End If
            dt_heading = ds_Heading.Tables(0)

            '********Getting Activity name for display
            Wstr += " And vActivityId = '" + Me.HFActivityId.Value.Trim() + "' And iNodeId = " + Me.HFNodeId.Value.Trim()
            If Not Me.objHelp.GetViewWorkSpaceNodeDetail(Wstr, ds_WorkspaceNodeDetail, estr) Then
                Throw New Exception("Error while getting Header information. " + estr)
            End If
            '*********************************

            '***********Getting Randomization No or vMySubjectNo for display
            Wstr = "vWorkspaceId='" & Me.HFWorkspaceId.Value.Trim() & "' and cStatusIndi <> 'D' And "
            Wstr += " iMySubjectNo = " + Me.HFMySubjectNo.Value.Trim() & " and iperiod = " & Convert.ToString(Me.HFPeriodId.Value).Trim()
            If Not Me.objHelp.GetWorkspaceSubjectMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                                     ds_WorkspaceSubjectMst, estr) Then
                Throw New Exception("Error while getting Header information. " + estr)
            End If
            '*************************************************

            If dt_heading.Rows.Count > 0 Then

                Me.lblHeader.Text = "Project No: " + dt_heading.Rows(0).Item("vProjectNo").ToString.Trim() +
                                        ",       Subject ID : " + Me.HFSubjectId.Value.Trim() +
                                        ",       Subject No : 0" +
                                        ",       Visit: " +
                                        ",       Period: " + Convert.ToString(Me.HFPeriodId.Value).Trim()

                If ds_WorkspaceNodeDetail.Tables(0).Rows.Count > 0 Then

                    Me.lblHeader.Text = "Project No: " + dt_heading.Rows(0).Item("vProjectNo").ToString.Trim() +
                                        ",       Subject ID : " + Me.HFSubjectId.Value.Trim() +
                                        ",       Subject No : 0" +
                                        ",       Visit: " + Convert.ToString(ds_WorkspaceNodeDetail.Tables(0).Rows(0)("vNodeDisplayName")).Trim() +
                                        ",       Period: " + Convert.ToString(Me.HFPeriodId.Value).Trim()

                    If ds_WorkspaceSubjectMst.Tables(0).Rows.Count > 0 Then

                        Me.lblHeader.Text = "Project No: " + dt_heading.Rows(0).Item("vProjectNo").ToString.Trim() +
                                        ",       Subject ID : " + Me.HFSubjectId.Value.Trim() +
                                        ",       Subject No : " + Convert.ToString(ds_WorkspaceSubjectMst.Tables(0).Rows(0)("vMySubjectNo")).Trim() +
                                        ",       Visit: " + Convert.ToString(ds_WorkspaceNodeDetail.Tables(0).Rows(0)("vNodeDisplayName")).Trim() +
                                        ",       Period: " + Convert.ToString(Me.HFPeriodId.Value).Trim()

                    End If
                End If
            End If

            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                If dt_heading.Rows.Count > 0 Then
                    Me.lblHeader.Text = "Site No: " + dt_heading.Rows(0).Item("vProjectNo").ToString.Trim() +
                                ",       Subject No : " + Me.HFScreenNo.Value.Trim()

                    If ds_WorkspaceSubjectMst.Tables(0).Rows.Count > 0 Then
                        If Convert.ToString(ds_WorkspaceSubjectMst.Tables(0).Rows(0)("vRandomizationNo")).Trim() <> "" Then
                            Me.lblHeader.Text += ",       Randomization No : " + Convert.ToString(ds_WorkspaceSubjectMst.Tables(0).Rows(0)("vRandomizationNo")).Trim()
                        End If
                    End If

                    '' Aded BY Rahul Rupareliya 
                    If ds_WorkspaceNodeDetail.Tables(0).Rows.Count > 0 Then
                        If (Request.QueryString("Activityname") = "") Then
                            Me.lblHeader.Text += ",       Visit: " + ds_WorkspaceNodeDetail.Tables(0).Rows(0)("vNodeDisplayName").ToString()
                        Else
                            Me.lblHeader.Text += ",       Visit: " + Request.QueryString("Activityname")
                        End If

                    End If
                    '' Ended

                    If Convert.ToString(dt_heading.Rows(0).Item("iNoOfPeriods")).Trim() <> "1" Then
                        Me.lblHeader.Text += ",       Period: " + Convert.ToString(Me.HFPeriodId.Value).Trim()
                    End If

                End If

            End If

            dt_heading = Nothing

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...DisplayHeader")
            Return False
        End Try
    End Function

#End Region

#Region "Fill Edit Reason DropDown"

    Private Function FillEditDropDown() As Boolean
        Dim estr As String = String.Empty
        Dim Ds_Reason As New DataSet
        Dim wStr As String = String.Empty
        Dim item As New ListItem

        Try
            wStr = " vActivityId ='" & ActId_DataEdition_BABE & "' and cStatusIndi <> 'D'"
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                wStr = " vActivityId ='" & ActId_DataEdition_CTM & "' and cStatusIndi <> 'D'"
            End If

            If Not Me.objHelp.View_ReasonMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                                        Ds_Reason, estr) Then
                Throw New Exception(estr)
            End If

            Me.DdlEditRemarks.DataSource = Ds_Reason.Tables(0)
            Me.DdlEditRemarks.DataValueField = "nReasonNo"
            Me.DdlEditRemarks.DataTextField = "vReasonDesc"
            Me.DdlEditRemarks.DataBind()

            item.Text = "Please Select a Reason"
            item.Value = "0"
            Me.DdlEditRemarks.Items.Insert(0, item)

            If Me.Session(S_DiscrpancyStatus) = "tabuler" And Me.btnGridViewDisplay.Visible = True Then
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "setData", "setData();", True)
                Session(S_DiscrpancyStatus) = ""
                Me.hndDiscrpancyStatus.Value = "Discrpancy"
            End If

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Filling Reasons ,", ex.Message)
            Return False
        End Try

    End Function

    Private Function FillDirectUpdateDropDown() As Boolean
        Dim estr As String = String.Empty
        Dim Ds_Reason As New DataSet
        Dim wStr As String = String.Empty
        Dim item As New ListItem
        Try
            wStr = " vActivityId ='" & ConfigurationManager.AppSettings.Item("DataUpdate_BABE").Trim() & "' and cStatusIndi <> 'D'"
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                wStr = " vActivityId ='" & ConfigurationManager.AppSettings.Item("DataUpdate_CTM").Trim() & "' and cStatusIndi <> 'D'"
            End If

            If Not Me.objHelp.View_ReasonMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                                        Ds_Reason, estr) Then
                Throw New Exception(estr)
            End If

            Me.ddlDirectUpdateRemarks.DataSource = Ds_Reason.Tables(0)
            Me.ddlDirectUpdateRemarks.DataValueField = "nReasonNo"
            Me.ddlDirectUpdateRemarks.DataTextField = "vReasonDesc"
            Me.ddlDirectUpdateRemarks.DataBind()

            item.Text = "Please Select a Reason"
            item.Value = "0"
            Me.ddlDirectUpdateRemarks.Items.Insert(0, item)

            If Me.Session(S_DiscrpancyStatus) = "tabuler" And Me.btnGridViewDisplay.Visible = True Then
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "setData", "setData();", True)
                Session(S_DiscrpancyStatus) = ""
                Me.hndDiscrpancyStatus.Value = "Discrpancy"
            End If

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Filling Reasons ,", ex.Message)
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
        Dim StrGroup(1) As String
        Dim StrGroupCodes As String = String.Empty
        Dim StrGroupDesc As String = String.Empty
        Dim dt_heading As New DataTable
        Dim dv_MedexGroup As DataView
        Dim dt_MedexGroup As New DataTable
        Dim i As Integer = 0
        Dim CntSubGroup As Integer = 0
        Dim PrevSubGroupCodes As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim ds_Heading As New DataSet
        Dim estr As String = String.Empty
        Dim ds_WorkspaceNodeDetail As New DataSet
        Dim StrMedExCodes As String = String.Empty
        Dim Repeatation As Integer = 1
        Dim PreviousRepeatation As Integer = 0
        Dim objimage As Object
        Dim Copyright As String = String.Empty
        Dim clientname As String = String.Empty
        Dim dvDependencySource As DataView
        Dim strTargetDependecyMedExCode As String = String.Empty
        Dim strSourceDependecyMedExCode As String = String.Empty
        Dim strSourceMedExValue As String = String.Empty
        Dim strDependencyValue As String = String.Empty
        Dim dcfFlag As Boolean = False
        Dim ds_DCF As DataSet = Nothing
        Dim TranNo As Integer = 0
        Dim j As Integer = 1
        Dim LastRepetition As Integer = 0
        Dim dtData As New DataTable
        Dim dtGetData As New DataTable
        Dim dvRepetationShowHide As New DataView

        Try
            Copyright = System.Configuration.ConfigurationManager.AppSettings("CopyRight")

            Page.Title = " :: Subject Attributes Detail :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            Me.CompanyName.Value = Copyright
            'If Not Me.Session("PlaceMedex") Is Nothing Then
            '    objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection)
            '    For Each objControl In objCollection
            '        PlaceMedEx.Controls.Remove(objControl)
            '    Next
            'End If

            PlaceMedEx.Controls.Clear()


            If CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows.Count < 1 Then
                Me.BtnSave.Visible = False
                Me.btnSaveAndContinue.Visible = False
                Me.BtnNext.Visible = False
                Me.BtnPrevious.Visible = False
                If Not IsPostBack Then
                    Me.objCommon.ShowAlert("No Attribute Is Attached With This Activity. So, Please Attach Attribute To This Activity.", Me.Page)
                End If
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "getRepeatitionDropDownColor", "getRepeatitionDropDownColor();", True)
                Return False
            End If

            dt_MedexGroup = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Copy()
            If Session(S_DataStatus) <> "" AndAlso ddlRepeatNo.SelectedValue <> "N" Then
                dt_MedexGroup.DefaultView.RowFilter = " RepetitionNo =" & ddlRepeatNo.SelectedValue.ToString
            End If
            dt_MedexGroup = dt_MedexGroup.DefaultView.ToTable
            Dim ds As New DataSet
            ds.Tables.Add(dt_MedexGroup.Copy())
            dv_MedexGroup = ds.Tables(0).DefaultView

            StrGroup(0) = "vMedExGroupCode"
            StrGroup(1) = "vMedExGroupDesc"
            dt_MedexGroup = dv_MedexGroup.ToTable(True, StrGroup)
            If dt_MedexGroup.Rows.Count > 1 Then
                Me.BtnNext.Visible = True
                Me.BtnPrevious.Visible = True
            End If
            PlaceMedEx.Controls.Add(New LiteralControl("<fieldset class=""FieldSetBox"" style=""padding: 1.6%; width: 94%; float: left;"">"))

            PlaceMedEx.Controls.Add(New LiteralControl("<Table id='tblMain' style=""width: 100%;border-radius:5px;padding-left:2%;padding-right:2%"">"))
            'For Tab Buttons
            For Each drGroup In dt_MedexGroup.Rows

                If StrGroupCodes <> "" Then
                    StrGroupCodes += ",Div" + drGroup("vMedExGroupCode")
                    StrGroupDesc += "," + drGroup("vMedExGroupDesc")
                    RepHeader = StrGroupDesc
                Else
                    StrGroupCodes = "Div" + drGroup("vMedExGroupCode")
                    StrGroupDesc = "" + drGroup("vMedExGroupDesc")
                    RepHeader = StrGroupDesc
                End If

            Next

            PlaceMedEx.Controls.Add(New LiteralControl("<tr>"))
            PlaceMedEx.Controls.Add(New LiteralControl("<Td style="" vertical-align:top"">"))
            PlaceMedEx.Controls.Add(New LiteralControl("<div style=""display: ; min-width: 100%; width: auto !important; width: 100%;" +
                    " align=""center"" id=""divMedExGroups"" runat=""server"">"))

            PlaceMedEx.Controls.Add(New LiteralControl("<Table style=""width: 100%; border-radius:5px;"">"))
            PlaceMedEx.Controls.Add(New LiteralControl("<tr>"))

            PlaceMedEx.Controls.Add(New LiteralControl("<Td style="" width:980px; vertical-align:top"">"))
            Me.GetButtons(StrGroupDesc, StrGroupCodes)

            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
            PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
            PlaceMedEx.Controls.Add(New LiteralControl("</Table>"))
            PlaceMedEx.Controls.Add(New LiteralControl("</div>"))

            Me.BtnNext.OnClientClick = "return Next('" & StrGroupCodes & "');"
            Me.BtnPrevious.OnClientClick = "return Previous('" & StrGroupCodes & "');"

            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
            PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

            '**********For user's readibility************************
            PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" height=""2px"" ALIGN=LEFT style=""BACKGROUND-COLOR: #FFFFFF"">"))
            PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""white-space: nowrap; vertical-align:middle"">"))
            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
            PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
            '*************************

            PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT >"))
            PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""vertical-align:top"">"))

            For Each drGroup In dt_MedexGroup.Rows
                dt = New DataTable
                dt_MedExMst = New DataTable
                dt = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Copy()
                dv = New DataView
                dv = dt.DefaultView

                If Me.hndGridViewStatus.Value.Trim() = "Grid" And btnGridViewDisplay.Visible = True Then
                    dv.RowFilter = "cCRFDtlDataStatus <> 'B' AND  vMedExGroupCode='" & drGroup("vMedExGroupCode").ToString.Trim() & "' "
                    Me.hndGridViewStatus.Value = ""
                Else
                    If CType(HFIsRepeatNo.Value, Integer) AndAlso ddlRepeatNo.SelectedValue <> "N" Then
                        If flagDCF = True Then
                            dv.RowFilter = "vMedExGroupCode='" & drGroup("vMedExGroupCode").ToString.Trim() & "' And RepetitionNo = " + ddlRepeatNo.SelectedValue.ToString()
                        Else
                            dv.RowFilter = "vMedExGroupCode='" & drGroup("vMedExGroupCode").ToString.Trim() & "' And RepetitionNo In ( " & hndRepetitionNo.Value.Trim() & ")"
                        End If
                    Else
                        dv.RowFilter = "vMedExGroupCode='" & drGroup("vMedExGroupCode").ToString.Trim() & "' "
                    End If
                End If

                dt_MedExMst = dv.ToTable()
                If Me.Session(S_SelectedTab) = Nothing Then
                    Me.Session(S_SelectedTab) = ""
                End If

                If HFActivateTab.Value <> "" Then
                    Me.Session(S_SelectedTab) = HFActivateTab.Value
                Else
                    Me.HFActivateTab.Value = Me.Session(S_SelectedTab).ToString()
                End If
                If Me.Session(S_SelectedTab) = "" Then
                    If i = 0 Then
                        PlaceMedEx.Controls.Add(New LiteralControl("<Div id='Div" & drGroup("vMedExGroupCode").ToString.Trim() & "' style=""display:""" +
                         " >")) 'Added for QC Comments on 22-May-2009

                        'Added on 27-Jul-2009
                        'HFActivateTab.Value = "Div" + drGroup("vMedExGroupCode")
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "CurrentTab", "currTab='Div" & drGroup("vMedExGroupCode").ToString.Trim() & "';", True)
                        '***********************
                    Else
                        PlaceMedEx.Controls.Add(New LiteralControl("<Div id='Div" & drGroup("vMedExGroupCode").ToString.Trim() & "' style=""display:none""" +
                         " >")) 'Added for QC Comments on 22-May-2009
                    End If
                Else
                    If Me.Session(S_SelectedTab).ToString() = "Div" + drGroup("vMedExGroupCode") Then
                        PlaceMedEx.Controls.Add(New LiteralControl("<Div id='Div" & drGroup("vMedExGroupCode").ToString.Trim() & "' style=""display:""" +
                         " >")) 'Added for QC Comments on 22-May-2009

                        'Added on 27-Jul-2009

                        'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "ShowHideDiv", "ShowHideDiv('divMedExGroups','imgMedExGroup')", True)
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "CurrentTab", "currTab='" & Me.Session(S_SelectedTab).ToString() & "';", True)

                    Else
                        PlaceMedEx.Controls.Add(New LiteralControl("<Div id='Div" & drGroup("vMedExGroupCode").ToString.Trim() & "' style=""display:none""" +
                        " >")) 'Added for QC Comments on 22-May-2009
                    End If
                End If
                Me.Session(S_SelectedTab) = Nothing

                'PlaceMedEx.Controls.Add(New LiteralControl("<Table width=""100%"" style=""border: solid 1px gray"">")) ' border=""1""
                PlaceMedEx.Controls.Add(New LiteralControl("<Table id=""tblPlaceMedEx"" width=""100%"" style=""border: solid 1px gray;border-radius:5px;"">")) 'Added on 30-01-2010 to fix the size of display

                'Added By Vimal Ghoniya for Field Dependency
                If Not dtDepedencyMedEx Is Nothing Then
                    For Each drMedExCode As DataRow In dtDepedencyMedEx.Rows
                        strTargetDependecyMedExCode += "[" & drMedExCode("vTargetMedExCode") & "],"
                    Next

                    If strTargetDependecyMedExCode <> "" Then
                        strTargetDependecyMedExCode = strTargetDependecyMedExCode.Substring(0, strTargetDependecyMedExCode.LastIndexOf(","))
                    End If
                End If
                'End  

                'Add by shivani pandya for show letest Repetition
                If Me.Session(S_TabulerRepeatition) <> "AllData" Or Me.Session(S_TabulerRepeatition) = Nothing Then
                    If Me.hndRepetitionNo.Value.Trim() = "" Then
                        If Me.Session(S_GetLetestData + CurrentPageSessionVariable + hdnActivityName.Value) <> "getData" Then
                            LastRepetition = dt_MedExMst.Compute("Max(RepetitionNo)", String.Empty)
                            dt_MedExMst.DefaultView.RowFilter = "RepetitionNo = " + LastRepetition.ToString()
                            dt_MedExMst = dt_MedExMst.DefaultView.ToTable().Copy
                            'Else
                            'Me.Session(S_GetLetestData) = ""
                        End If
                    End If
                End If
                'End

                For Each dr In dt_MedExMst.Rows

                    strSourceDependecyMedExCode = ""
                    strSourceMedExValue = ""
                    strDependencyValue = ""

                    If StrMedExCodes <> String.Empty Then
                        StrMedExCodes += ","
                    End If
                    StrMedExCodes += Convert.ToString(dr("vMedExCode")).Trim()

                    ' If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                    If Me.ViewState(VS_DataStatus) <> CRF_DataEntryPending Then
                        Repeatation = dr("iRepeatNo")
                    End If

                    'If Me.ViewState(VS_DataStatus) <> CRF_DataEntry AndAlso Me.ViewState(VS_DataStatus) <> CRF_DataEntryPending Then

                    '    If dr("iWorkflowStageId") = (Convert.ToInt32(Me.Session(S_WorkFlowStageId)) - 10) AndAlso Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    '        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                    '    End If

                    If ddlRepeatNo.Items.Count >= 2 AndAlso ddlRepeatNo.SelectedValue <> "N" Then

                        If PreviousRepeatation <> Repeatation Then
                            PlaceMedEx.Controls.Add(New LiteralControl("<Tr id=""Repetition_" + Convert.ToString(dr("RepetitionNo")) + """ class=""Repetition_" + Convert.ToString(dr("RepetitionNo")) + """  width=""100%"" height=""1px"" ALIGN=LEFT style=""background-color:#ffcc66;color:navy;"">"))
                            'PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" ALIGN=LEFT style=""BACKGROUND-COLOR: #ffcc66"">")) '008ecd
                            PlaceMedEx.Controls.Add(New LiteralControl("<Td colspan=""2"" style=""white-space: nowrap; vertical-align:middle; font-weight: bold; font-size: 12px; "">"))
                            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                                If ddlRepeatNo.Items.Count > 0 Then
                                    PlaceMedEx.Controls.Add(New LiteralControl(StrGroupDesc + " :-" + Convert.ToString(dr("RepetitionNo"))))
                                End If
                            Else
                                If ddlRepeatNo.Items.Count > 2 Then
                                    PlaceMedEx.Controls.Add(New LiteralControl(StrGroupDesc + " :-" + Convert.ToString(dr("RepetitionNo"))))
                                End If
                            End If
                            'If ddlRepeatNo.Items.Count > 2 Then
                            '    PlaceMedEx.Controls.Add(New LiteralControl("Repetition " + Convert.ToString(dr("RepetitionNo"))))
                            'End If
                            'PlaceMedEx.Controls.Add(Getlable("Repeatation " + Convert.ToString(dr("iRepeatNo")), "Repeatation" + Convert.ToString(dr("iRepeatNo"))))
                            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                            PlaceMedEx.Controls.Add(New LiteralControl("<Td align=""right"" style=""white-space: nowrap; vertical-align:middle; font-weight: bold; font-size: 12px; "">"))

                            'If (CType(Me.Session(S_WorkFlowStageId), String) = WorkFlowStageId_DeleteDataEntry) Then
                            '    PlaceMedEx.Controls.Add(GetDeleteRepetitionButton(dr("nCRFDtlNo").ToString + "_" + Convert.ToString(dr("RepetitionNo"))))
                            'End If

                            'change condition from dr("iWorkflowstageId") <> WorkFlowStageId_FinalReviewAndLock to dr("cCRFDtlDataStatus") <> CRF_Locked
                            'If (((CType(Me.Session(S_ScopeNo), String) = Scope_BABE And CType(Me.Session(S_WorkFlowStageId), String) = WorkFlowStageId_DeleteDataEntry) Or
                            '       (CType(Me.Session(S_ScopeNo), String) = Scope_ClinicalTrial And CType(Me.Session(vs_workflowstageidfordynamic), String) = WorkFlowStageId_DataEntry And dr("iWorkFlowstageId") = 0)) _
                            '       And dr("cCRFDtlDataStatus") <> CRF_Locked) Then

                            '    PlaceMedEx.Controls.Add(GetDeleteRepetitionButton(dr("nCRFDtlNo").ToString + "_" + Convert.ToString(dr("RepetitionNo")))) '' added by prayag iWorkFlowstageId=0 for ct

                            'End If

                            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                            PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                        End If
                    End If

                    ' End If
                    If Me.ViewState(VS_DataStatus) <> CRF_DataEntryPending Then
                        PreviousRepeatation = dr("iRepeatNo")
                    End If
                    ' End If

                    If PrevSubGroupCodes = "" Or PrevSubGroupCodes <> dr("vMedExSubGroupCode") Then

                        If Convert.ToString(dr("vMedExSubGroupCode")).Trim() <> "0000" Then 'Added for default SubGroup

                            PlaceMedEx.Controls.Add(New LiteralControl("<Tr id=""Repetition_" + Convert.ToString(dr("RepetitionNo")) + "_" + Convert.ToString(j) + """  class=""Repetition_" + Convert.ToString(dr("RepetitionNo")) + """ width=""100%"" ALIGN=LEFT style=""BACKGROUND-COLOR: #58BDF7"">")) 'ffcc66
                            PlaceMedEx.Controls.Add(New LiteralControl("<Td colspan=""3"" style=""vertical-align:middle"">"))    ' Change by Hiren Rami For Designing Changes
                            PlaceMedEx.Controls.Add(Getlable(dr("vMedExSubGroupDesc").ToString.Trim(), dr("vMedExSubGroupCode") + CntSubGroup.ToString.Trim() + "R" + Convert.ToString(Repeatation)))
                            CntSubGroup += 1
                            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                            PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                        End If

                    End If

                    If Me.ViewState(VS_DataStatus) <> CRF_DataEntryPending Then
                        PlaceMedEx.Controls.Add(New LiteralControl("<Tr id=""Repetition_" + Convert.ToString(dr("RepetitionNo")) + """ ALIGN=LEFT>"))
                    Else
                        PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT>"))
                    End If



                    If Convert.ToString(dr("vMedExType")).Trim.ToUpper() <> "LABEL" Then
                        If Me.ViewState(VS_DataStatus) <> CRF_DataEntryPending Then
                            PlaceMedEx.Controls.Add(New LiteralControl("<Td style="" width:40%;  vertical-align:middle"" class=""Repetition_" + Convert.ToString(dr("RepetitionNo")) + """ ALIGN=LEFT>"))
                        Else
                            PlaceMedEx.Controls.Add(New LiteralControl("<Td style="" width:40%; vertical-align:middle"" ALIGN=LEFT>"))
                        End If

                        PrevSubGroupCodes = dr("vMedExSubGroupCode")
                        PlaceMedEx.Controls.Add(GetlableWithHistory(dr("vMedExDesc") & ": ", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), Repeatation))
                        PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                        PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""vertical-align: middle;width:60%"" ALIGN=LEFT>"))
                    Else
                        PlaceMedEx.Controls.Add(New LiteralControl("<Td colspan=""2"" style="" vertical-align:middle"" ALIGN=LEFT>"))
                        PrevSubGroupCodes = dr("vMedExSubGroupCode")
                        'PlaceMedEx.Controls.Add(GetlableWithHistory(dr("vMedExDesc") & ": ", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode")))
                    End If


                    'Added By Vimal Ghoniya
                    strSourceDependecyMedExCode = ""
                    dvDependencySource = dtDepedencyMedEx.DefaultView
                    dvDependencySource.RowFilter = "vSourceMedExCode = '" & dr("vMedExCode") & "'"

                    For Each drSourceMedEx As DataRow In dvDependencySource.ToTable().Rows
                        strDependencyValue = ""
                        For Each strVal As String In drSourceMedEx("vMedExValue").ToString().Split(",")
                            strDependencyValue += "[" + strVal + "]"
                        Next
                        strSourceDependecyMedExCode += drSourceMedEx("vTargetMedExCode").ToString() + "R" + Convert.ToString(Repeatation) + "#" + drSourceMedEx("vMedExType").ToString() + "#" + strDependencyValue + ","
                    Next

                    If dvDependencySource.ToTable().Rows.Count > 0 Then
                        strSourceMedExValue = dvDependencySource.ToTable().Rows(0)("vMedExValue")
                    End If

                    If strSourceDependecyMedExCode <> "" Then
                        strSourceDependecyMedExCode = strSourceDependecyMedExCode.Substring(0, strSourceDependecyMedExCode.LastIndexOf(","))
                    End If

                    'End
                    If Not dt_MedExMst.Columns.Contains("iTranNo") Then
                        TranNo = 0
                    Else
                        TranNo = dr("iTranNo")
                    End If

                    Dim numeric As String = ""

                    If Not (dr("vValidationType") Is System.DBNull.Value) Then
                        If dr("vValidationType") <> "" And dr("vValidationType") <> "NA" Then
                            StrValidation = dr("vValidationType").ToString.Trim().Split(",")
                            HFNumericScale.Value = 0
                            If StrValidation.Length > 2 Then
                                HFNumericScale.Value = StrValidation(2).ToString()
                                numeric = StrValidation(1).ToString() + "," + StrValidation(2).ToString()
                            End If
                            If StrValidation.Length > 1 Then

                                objelement = GetObject(dr("vMedExType"), dr("vMedExGroupCode"), dr("vMedExSubGroupCode"),
                                                       dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("vMedExValues"),
                                                       IIf(dr("vDefaultValue") Is System.DBNull.Value, "", dr("vDefaultValue")),
                                                       StrValidation(0).ToString.Trim(), StrValidation(1).ToString.Trim(),
                                                       IIf(dr("vAlertonvalue") Is System.DBNull.Value, "", dr("vAlertonvalue")),
                                                       IIf(dr("vAlertMessage") Is System.DBNull.Value, "", dr("vAlertMessage")),
                                                       IIf(dr("vHighRange") Is System.DBNull.Value, "0", dr("vHighRange")),
                                                       IIf(dr("vLowRange") Is System.DBNull.Value, "0", dr("vLowRange")),
                                                       IIf(dr("vRefTable") Is System.DBNull.Value, "", dr("vRefTable")),
                                                       IIf(dr("vRefColumn") Is System.DBNull.Value, "", dr("vRefColumn")),
                                                       IIf(dr("cAlertType") Is System.DBNull.Value, "", dr("cAlertType")),
                                                       HFNumericScale.Value, strSourceDependecyMedExCode,
                                                       strTargetDependecyMedExCode, strSourceMedExValue, TranNo, numeric)
                            Else
                                objelement = GetObject(dr("vMedExType"), dr("vMedExGroupCode"), dr("vMedExSubGroupCode"),
                                                       dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("vMedExValues"),
                                                       IIf(dr("vDefaultValue") Is System.DBNull.Value, "", dr("vDefaultValue")),
                                                       StrValidation(0).ToString.Trim(), "", IIf(dr("vAlertonvalue") Is System.DBNull.Value,
                                                       "", dr("vAlertonvalue")), IIf(dr("vAlertMessage") Is System.DBNull.Value,
                                                       "", dr("vAlertMessage")), IIf(dr("vHighRange") Is System.DBNull.Value, "0",
                                                       dr("vHighRange")), IIf(dr("vLowRange") Is System.DBNull.Value, "0", dr("vLowRange")),
                                                       IIf(dr("vRefTable") Is System.DBNull.Value, "", dr("vRefTable")),
                                                       IIf(dr("vRefColumn") Is System.DBNull.Value, "", dr("vRefColumn")),
                                                       IIf(dr("cAlertType") Is System.DBNull.Value, "", dr("cAlertType")),
                                                       HFNumericScale.Value, strSourceDependecyMedExCode,
                                                       strTargetDependecyMedExCode, strSourceMedExValue, TranNo, numeric)
                            End If

                        Else
                            objelement = GetObject(dr("vMedExType"), dr("vMedExGroupCode"), dr("vMedExSubGroupCode"),
                                                   dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("vMedExValues"),
                                                   IIf(dr("vDefaultValue") Is System.DBNull.Value, "", dr("vDefaultValue")),
                                                   "", "", IIf(dr("vAlertonvalue") Is System.DBNull.Value, "", dr("vAlertonvalue")),
                                                   IIf(dr("vAlertMessage") Is System.DBNull.Value, "", dr("vAlertMessage")),
                                                   IIf(dr("vHighRange") Is System.DBNull.Value, "0", dr("vHighRange")),
                                                   IIf(dr("vLowRange") Is System.DBNull.Value, "0", dr("vLowRange")),
                                                   IIf(dr("vRefTable") Is System.DBNull.Value, "", dr("vRefTable")),
                                                   IIf(dr("vRefColumn") Is System.DBNull.Value, "", dr("vRefColumn")),
                                                   IIf(dr("cAlertType") Is System.DBNull.Value, "", dr("cAlertType")),
                                                   HFNumericScale.Value, strSourceDependecyMedExCode,
                                                   strTargetDependecyMedExCode, strSourceMedExValue, TranNo, numeric)
                        End If
                    Else
                        objelement = GetObject(dr("vMedExType"), dr("vMedExGroupCode"), dr("vMedExSubGroupCode"),
                                               dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("vMedExValues"),
                                               IIf(dr("vDefaultValue") Is System.DBNull.Value, "", dr("vDefaultValue")),
                                               "", "", IIf(dr("vAlertonvalue") Is System.DBNull.Value, "", dr("vAlertonvalue")),
                                               IIf(dr("vAlertMessage") Is System.DBNull.Value, "", dr("vAlertMessage")),
                                               IIf(dr("vHighRange") Is System.DBNull.Value, "0", dr("vHighRange")),
                                               IIf(dr("vLowRange") Is System.DBNull.Value, "0", dr("vLowRange")),
                                               IIf(dr("vRefTable") Is System.DBNull.Value, "", dr("vRefTable")),
                                               IIf(dr("vRefColumn") Is System.DBNull.Value, "", dr("vRefColumn")),
                                               IIf(dr("cAlertType") Is System.DBNull.Value, "", dr("cAlertType")),
                                               HFNumericScale.Value, strSourceDependecyMedExCode,
                                               strTargetDependecyMedExCode, strSourceMedExValue, TranNo, numeric)
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

                    'For Print UOM 
                    If Not dr("vUOM") Is System.DBNull.Value AndAlso dr("vUOM").ToString.Trim() <> "" Then
                        PlaceMedEx.Controls.Add(Getlable(dr("vUOM"), "UOM" + dr("vMedExCode") + "R" + Convert.ToString(Repeatation)))
                    End If

                    '*********************************
                    'for calendar image & Validating Edc USer
                    If Not Me.Session(S_EDCUser).ToString = EDCUser Then
                        If dr("vMedExType").ToString.ToUpper.Trim() = "DATETIME" Then
                            objimage = GetDateImage(dr("vMedExCode"), objelement)
                        End If
                    End If

                    If Is_ComboGlobalDictionary Then
                        ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                        If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_MedicalCoding Then
                            ''Commented by nipun khant for dynamic review
                            'If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_MedicalCoding Then
                            PlaceMedEx.Controls.Add(GetBrowseButton("Browse", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"),
                                                                    dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nMedExWorkSpaceDtlNo"),
                                                                    strTargetDependecyMedExCode))
                            Is_ComboGlobalDictionary = False
                        End If
                    End If

                    If Is_FormulaEnabled Then
                        ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                        If Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() = WorkFlowStageId_DataEntry Or
                                    Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() = WorkFlowStageId_FirstReview Then
                            PlaceMedEx.Controls.Add(GetAutoCalculateButton("Calc", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"),
                                                                           dr("vMedExCode") + "R" + Convert.ToString(Repeatation), Convert.ToString(dr("vMedExFormula")), Convert.ToString(dr("iDecimalNo")),
                                                                           strTargetDependecyMedExCode))
                            Is_FormulaEnabled = False
                        End If
                        ''Commented by nipun khant for dynamic review
                        'If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_DataEntry Or _
                        '            Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_FirstReview Then
                        '    PlaceMedEx.Controls.Add(GetAutoCalculateButton("Calc", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), _
                        '                                                   dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("vMedExFormula"), dr("iDecimalNo"), _
                        '                                                   strTargetDependecyMedExCode))
                        '    Is_FormulaEnabled = False
                        'End If
                    End If

                    If Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "TEXTAREA" Then
                        PlaceMedEx.Controls.Add(New LiteralControl("<br/>"))
                        PlaceMedEx.Controls.Add(GetCountLable(dr("vMedExGroupCode"), dr("vMedExSubGroupCode"),
                                                              dr("vMedExCode") + "R" + Convert.ToString(Repeatation)))
                    End If


                    'Added By Pratiksha To have date or time buttons //Enhancement in EDC
                    ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                    If Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() = WorkFlowStageId_DataEntry AndAlso
                        Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                        ''Commented by nipun khant for dynamic review
                        'If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_DataEntry AndAlso _
                        '    Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                        If Me.Session(S_EDCUser).ToString = EDCUser Then
                            If Not dt_MedExMst.Columns.Contains("iTranNo") Then
                                If Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "DATETIME" Then
                                    PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                                    PlaceMedEx.Controls.Add(GetAutoDateButton(dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), Repeatation, strSourceDependecyMedExCode,
                                                                              strTargetDependecyMedExCode, strSourceMedExValue))
                                    PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                                    PlaceMedEx.Controls.Add(GetClearButton(dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), Repeatation, strSourceDependecyMedExCode,
                                                                           strTargetDependecyMedExCode, strSourceMedExValue))
                                ElseIf Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "TIME" Then
                                    PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                                    PlaceMedEx.Controls.Add(GetAutoTimeButton(dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), Repeatation, strSourceDependecyMedExCode,
                                                                              strTargetDependecyMedExCode, strSourceMedExValue))
                                    PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                                    PlaceMedEx.Controls.Add(GetClearButton(dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), Repeatation, strSourceDependecyMedExCode,
                                                                           strTargetDependecyMedExCode, strSourceMedExValue))

                                ElseIf Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "STANDARDDATETIME" Then ''Added by ketan
                                    PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                                    PlaceMedEx.Controls.Add(GetAutoStandardDateButton(dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), Repeatation, strSourceDependecyMedExCode,
                                                                              strTargetDependecyMedExCode, strSourceMedExValue))
                                    PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                                    PlaceMedEx.Controls.Add(GetClearStandardDateButton(dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), Repeatation, strSourceDependecyMedExCode,
                                                                           strTargetDependecyMedExCode, strSourceMedExValue))
                                End If
                            ElseIf CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_DataEntry Then
                                If Convert.ToString(dr("vDefaultValue")).Trim() = "" Or dr("iTranNo") < 1 Then
                                    If Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "DATETIME" Then
                                        PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                                        PlaceMedEx.Controls.Add(GetAutoDateButton(dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), Repeatation, strSourceDependecyMedExCode,
                                                                                  strTargetDependecyMedExCode, strSourceMedExValue))
                                        PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                                        PlaceMedEx.Controls.Add(GetClearButton(dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), Repeatation, strSourceDependecyMedExCode,
                                                                               strTargetDependecyMedExCode, strSourceMedExValue))
                                    ElseIf Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "TIME" Then
                                        PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                                        PlaceMedEx.Controls.Add(GetAutoTimeButton(dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), Repeatation, strSourceDependecyMedExCode,
                                                                                  strTargetDependecyMedExCode, strSourceMedExValue))
                                        PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                                        PlaceMedEx.Controls.Add(GetClearButton(dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), Repeatation, strSourceDependecyMedExCode,
                                                                               strTargetDependecyMedExCode, strSourceMedExValue))
                                    End If
                                End If
                            End If
                        End If
                    End If
                    ''''''''''''''''''''''''''''''''''''''''''''''''

                    PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                    'PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""white-space: nowrap; vertical-align:middle"" ALIGN=LEFT>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""white-space: nowrap "" ALIGN=""LEFT"">"))

                    objelement.Attributes.Remove("disabled")
                    ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                    If (Me.ddlRepeatNo.SelectedIndex = 0 AndAlso
                            (Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() <> WorkFlowStageId_DataEntry AndAlso
                             Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() <> WorkFlowStageId_FirstReview)) Or Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                        ''Commented by nipun khant for dynamic review
                        'If (Me.ddlRepeatNo.SelectedIndex = 0 AndAlso _
                        '        (Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_DataEntry AndAlso _
                        '         Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_FirstReview)) Or Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                        If Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "RADIO" Then
                            For Each lst As ListItem In CType(objelement, RadioButtonList).Items
                                lst.Enabled = False
                            Next
                        ElseIf Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "CHECKBOX" Then
                            For Each lst As ListItem In CType(objelement, CheckBoxList).Items
                                lst.Enabled = False
                            Next
                        Else
                            objelement.Attributes.Add("disabled", "true")
                        End If
                    End If

                    'Rahul Rupareliya For DOM Profile
                    If Me.ViewState("IR") = "Y" Then
                        'PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))
                        'PlaceMedEx.Controls.Add(GetEditButton("Edit", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), dr("vMedexType")))
                        'PlaceMedEx.Controls.Add(GetUpdateButton("Update", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), dr("vMedexType")))

                    End If
                    If Me.Session(S_WorkFlowStageId) = "2" Then
                        If dr("vMedExDesc") <> "" And Convert.ToString(dr("vMedExValues")).ToUpper <> "NOTE" Then
                            PlaceMedEx.Controls.Add(GetEditButton("Edit", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), dr("vMedexType")))
                            PlaceMedEx.Controls.Add(GetUpdateButton("Update", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), dr("vMedexType")))
                            'PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))
                        End If
                    End If

                    If Me.ddlRepeatNo.SelectedIndex <> 0 AndAlso
                            (CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_Review Or
                                CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_ReviewCompleted Or
                                CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_Locked Or
                                CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_Rejected) Then 'added by vasimkhan for rejected ckecklist Enabled false
                        'Or _
                        'CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_DataEntry) Then

                        If Convert.ToString(dr("vMedExType")).Trim.ToUpper() <> "LABEL" Then

                            If Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "RADIO" Then
                                For Each lst As ListItem In CType(objelement, RadioButtonList).Items
                                    lst.Enabled = False
                                Next
                            ElseIf Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "CHECKBOX" Then
                                For Each lst As ListItem In CType(objelement, CheckBoxList).Items
                                    lst.Enabled = False
                                Next
                            Else
                                objelement.Attributes.Add("disabled", "true")
                            End If





                            'Condition changed to solve problem of view mode -- Pratiksha
                            If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

                                ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                                If Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() <> WorkFlowStageId_OnlyView Then
                                    ''Commented by nipun khant for dynamic review
                                    'If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_OnlyView Then
                                    If Not Me.HFCRFDtlLockStatus.Value Is Nothing AndAlso
                                           Not Convert.ToString(Me.HFCRFDtlLockStatus.Value).Trim.ToUpper() = CRFHdr_Locked Then
                                        'added by Megha as view-CRFHDRDTLSubDtl_Edit giving only max DCF Result which cause into false validation 
                                        If Not objHelp.GetData("DCFMst", "*", " nCRFDtlNo =" + dr("nCRFDtlNo").ToString.Trim() + " And vMedexCode='" + dr("vMedexCode").ToString.Trim() + "' And cStatusindi<>'D' AND (cDCFStatus = 'N' OR cDCFStatus = 'O') order by cDCFStatus ", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DCF, estr) Then
                                            Throw New Exception("Error while get data from DCFMst")
                                        End If

                                        If ds_DCF.Tables(0).Rows.Count > 0 Then
                                            'Checking if Discrepancy is there or not in field
                                            If Convert.ToString(ds_DCF.Tables(0).Rows(0).Item("cDCFStatus")).Trim.ToUpper() <> Discrepancy_Answered AndAlso
                                                    Convert.ToString(ds_DCF.Tables(0).Rows(0).Item("cDCFStatus")).Trim.ToUpper() <> Discrepancy_Resolved AndAlso
                                                    Convert.ToString(ds_DCF.Tables(0).Rows(0).Item("cDCFStatus")).Trim.ToUpper() <> Discrepancy_InternallyResolved AndAlso
                                                    Convert.ToString(ds_DCF.Tables(0).Rows(0).Item("cDCFStatus")).Trim.ToUpper() <> Discrepancy_AutoResolved Then

                                                'AndAlso CType(Me.Session(S_UserID), Integer) = CType(Convert.ToString(dr("iDCFBy")).Trim(), Integer) Then
                                                ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                                                If Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() = WorkFlowStageId_DataEntry Then
                                                    ''Commented by nipun khant for dynamic review
                                                    'If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_DataEntry Then
                                                    If (Me.Session(S_EDCUser).ToString() = EDCUser And
                                                        (Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "DATETIME" Or Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "TIME")) Then
                                                    Else
                                                        PlaceMedEx.Controls.Add(GetEditButton("Edit", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), dr("vMedexType")))
                                                        PlaceMedEx.Controls.Add(GetUpdateButton("Update", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), dr("vMedexType")))
                                                    End If
                                                End If

                                                objelement.Attributes.Add("title", Convert.ToString(dr("vSourceResponse")).Trim())

                                            End If
                                        End If
                                        ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic

                                        If Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "COMBOGLOBALDICTIONARY" AndAlso
                                                                Me.Session(S_WorkFlowStageId).Trim() = WorkFlowStageId_MedicalCoding Then
                                            ''Commented by nipun khant for dynamic review
                                            'If Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "COMBOGLOBALDICTIONARY" AndAlso _
                                            '                        Me.Session(S_WorkFlowStageId) = WorkFlowStageId_MedicalCoding Then
                                            PlaceMedEx.Controls.Add(GetEditButton("Edit", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), dr("vMedexType")))
                                            PlaceMedEx.Controls.Add(GetUpdateButton("Update", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), dr("vMedexType")))

                                        End If

                                    End If

                                    If dt.Columns.Contains("nCRFDtlNo") Then

                                        'Condition changed for view mode -pratiksha
                                        If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                                            'If Convert.ToString(Me.ViewState(VS_ReviewFlag)).ToUpper() <> "YES" Then
                                            If Convert.ToString(dr("nDCFNo")).Trim() <> "" AndAlso
                                                  Convert.ToString(dr("cDCFStatus")).Trim.ToUpper() <> Discrepancy_Answered AndAlso
                                                  Convert.ToString(dr("cDCFStatus")).Trim.ToUpper() <> Discrepancy_Resolved AndAlso
                                                   Convert.ToString(dr("cDCFStatus")).Trim.ToUpper() <> Discrepancy_InternallyResolved AndAlso
                                                  Convert.ToString(dr("cDCFStatus")).Trim.ToUpper() <> Discrepancy_AutoResolved Then
                                                'Changed for view mode --Pratiksha
                                                'If Me.Session(S_WorkFlowStageId) >= Me.HFReviewedWorkFlowId.Value.Trim() Then
                                                ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                                                If Me.Session(vs_workflowstageidfordynamic) >= dr("iWorkFlowstageId").ToString() And Me.Session(vs_workflowstageidfordynamic) <> WorkFlowStageId_OnlyView AndAlso
                                                    dr("iWorkFlowstageId") = Me.Session(vs_workflowstageidfordynamic) - 10 Then
                                                    PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))
                                                    dcfFlag = True
                                                ElseIf (dr("iWorkFlowstageId") = Me.Session(vs_workflowstageidfordynamic) - 10 AndAlso dr("iWorkFlowstageId") = Me.Session(vs_workflowstageidfordynamic)) AndAlso Me.Session(vs_workflowstageidfordynamic) <> WorkFlowStageId_DataEntry Then
                                                    PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))
                                                    dcfFlag = True
                                                ElseIf (dr("iWorkFlowstageId") = Me.Session(vs_workflowstageidfordynamic) - 10 AndAlso (dr("iWorkFlowstageId") = Me.Session(vs_workflowstageidfordynamic) AndAlso Convert.ToString(dr("nDCFNo")).Trim() <> "")) Then
                                                    PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))
                                                    dcfFlag = True
                                                ElseIf (dr("iWorkFlowstageId") >= Me.Session(vs_workflowstageidfordynamic) AndAlso (dr("iWorkFlowstageId") >= Me.Session(vs_workflowstageidfordynamic) AndAlso Convert.ToString(dr("nDCFNo")).Trim() <> "") AndAlso Me.Session(vs_workflowstageidfordynamic) = 0) Then
                                                    PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))
                                                    dcfFlag = True
                                                End If
                                                ''Commented by nipun khant for dynamic review
                                                'If Me.Session(S_WorkFlowStageId) >= dr("iWorkFlowstageId").ToString() And Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_OnlyView AndAlso _
                                                '    dr("iWorkFlowstageId") = Me.Session(S_WorkFlowStageId) - 10 Then
                                                '    PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))
                                                '    dcfFlag = True
                                                'ElseIf Me.Session(S_WorkFlowStageId) = WorkFlowStageId_DataEntry Then
                                                '    PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))
                                                '    dcfFlag = True
                                                'End If
                                                ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                                            ElseIf Me.Session(vs_workflowstageidfordynamic) >= WorkFlowStageId_DataEntry And (dr("iWorkFlowstageId") = Me.Session(vs_workflowstageidfordynamic) - 10 And dr("cCRFDtlDatastatus") <> CRF_DataEntry) Then
                                                PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))
                                                dcfFlag = True
                                            ElseIf Me.Session(vs_workflowstageidfordynamic) = WorkFlowStageId_DataValidator Then
                                                ''Commented by nipun khant for dynamic review
                                                'ElseIf Me.Session(S_WorkFlowStageId) = WorkFlowStageId_DataValidator Then
                                                PlaceMedEx.Controls.Add(GetEditButton("Edit", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), dr("vMedexType")))
                                                PlaceMedEx.Controls.Add(GetUpdateButton("Update", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), dr("vMedexType")))

                                                'ElseIf Convert.ToString(Me.ViewState(VS_ReviewFlag)).ToUpper() <> "YES" AndAlso _
                                                '    Me.Session(S_WorkFlowStageId) >= WorkFlowStageId_FirstReview Then
                                                '    PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))


                                            ElseIf Not dr("iWorkFlowstageId") Is Nothing AndAlso
                                                dr("iWorkFlowstageId").ToString() < Me.Session(vs_workflowstageidfordynamic) AndAlso
                                                dr("iWorkFlowstageId") = Me.Session(vs_workflowstageidfordynamic) - 10 Then



                                                ''Commented by nipun khant for dynamic review
                                                'ElseIf Not dr("iWorkFlowstageId") Is Nothing AndAlso _
                                                '    dr("iWorkFlowstageId").ToString() < Me.Session(S_WorkFlowStageId) AndAlso _
                                                '    dr("iWorkFlowstageId") = Me.Session(S_WorkFlowStageId) - 10 Then
                                                If Me.Session(vs_workflowstageidfordynamic) >= WorkFlowStageId_DataEntry And (dr("iWorkFlowstageId") = Me.Session(vs_workflowstageidfordynamic) - 10 And dr("cCRFDtlDatastatus") <> CRF_DataEntry) Then
                                                    'If (dr("cCRFDtlDatastatus") <> CRF_DataEntry AndAlso dr("cCRFDtlDatastatus") >= Me.Session(vs_workflowstageidfordynamic)) Then
                                                    PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))
                                                    dcfFlag = True
                                                Else
                                                    'PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))
                                                End If


                                                'ElseIf Me.Session(S_WorkFlowStageId) = WorkFlowStageId_FinalReviewAndLock AndAlso _
                                                '    Convert.ToString(Me.HFCRFDtlLockStatus.Value).Trim.ToUpper() <> CRFHdr_Locked Then
                                                '    PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))

                                                ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                                            ElseIf Me.Session(vs_workflowstageidfordynamic) = WorkFlowStageId_DataEntry AndAlso
                                                Convert.ToString(Me.ViewState(VS_ReviewFlag)).ToUpper() <> "YES" AndAlso
                                                dr("iWorkFlowstageId").ToString() < Me.Session(vs_workflowstageidfordynamic) Then
                                                ''Commented by nipun khant for dynamic review
                                                'ElseIf Me.Session(S_WorkFlowStageId) = WorkFlowStageId_DataEntry AndAlso _
                                                'Convert.ToString(Me.ViewState(VS_ReviewFlag)).ToUpper() <> "YES" AndAlso _
                                                'dr("iWorkFlowstageId").ToString() < Me.Session(S_WorkFlowStageId) Then
                                                PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))
                                                dcfFlag = True

                                                ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                                            ElseIf Me.Session(vs_workflowstageidfordynamic) >= dr("iWorkFlowstageId").ToString() AndAlso
                                        Me.Session(vs_workflowstageidfordynamic) = WorkFlowStageId_DataEntry AndAlso
                                                    dr("cCRFDtlDatastatus").ToString() <> CRF_DataEntryPending AndAlso
                                                    dr("cCRFDtlDatastatus").ToString() <> CRF_DataEntry Then
                                                ''Commented by nipun khant for dynamic review
                                                'ElseIf Me.Session(S_WorkFlowStageId) >= dr("iWorkFlowstageId").ToString() AndAlso _
                                                'Me.Session(S_WorkFlowStageId) = WorkFlowStageId_DataEntry AndAlso _
                                                '        dr("cCRFDtlDatastatus").ToString() <> CRF_DataEntryPending AndAlso _
                                                '        dr("cCRFDtlDatastatus").ToString() <> CRF_DataEntry Then
                                                PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))
                                                dcfFlag = True

                                            End If
                                            'End If

                                        End If

                                    End If
                                    ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                                ElseIf Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() = WorkFlowStageId_OnlyView Then
                                    ''Commented by nipun khant for dynamic review
                                    'ElseIf Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_OnlyView Then
                                    If (dr("cCRFDtlDatastatus") <> "B") Then
                                        PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))
                                        dcfFlag = True
                                    End If

                                End If

                            ElseIf Not dr("iWorkFlowstageId") Is Nothing AndAlso
                                dr("iWorkFlowstageId").ToString() = "0" AndAlso
                                 Me.Session("vReviewerlevel").ToString().ToUpper() = "FIRST REVIEW" Then
                                If (dr("cCRFDtlDatastatus") <> CRF_DataEntry) Then
                                    PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))
                                    dcfFlag = True
                                End If

                                '*****************
                            End If

                            'PlaceMedEx.Controls.Add(GetAudittrailButton("AuditTrail", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("iTranNo"), dr("nCRFDtlNo")))
                            'Showing image information for identifying them
                            Me.trimageinformation.Visible = True
                            '***************************

                        End If

                    ElseIf Me.ddlRepeatNo.SelectedIndex <> 0 AndAlso
                            (CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_DataEntry) Then

                        If Convert.ToString(dr("vMedExType")).Trim.ToUpper() <> "LABEL" Then
                            'Condition changed to solve problem of view mode -- Pratiksha
                            'If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_OnlyView Or dataMode_String <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                            If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                                ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                                If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_OnlyView Then
                                    ''Commented by nipun khant for dynamic review
                                    'If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_OnlyView Then
                                    If Not Me.HFCRFDtlLockStatus.Value Is Nothing AndAlso
                                           Not Convert.ToString(Me.HFCRFDtlLockStatus.Value).Trim.ToUpper() = CRFHdr_Locked Then

                                        If Convert.ToString(dr("vDefaultValue")).Trim() <> "" Or dr("iTranNo") > 1 Then
                                            ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                                            If Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() = WorkFlowStageId_DataEntry Then
                                                ''Commented by nipun khant for dynamic review
                                                'If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_DataEntry Then

                                                If Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "RADIO" Then
                                                    For Each lst As ListItem In CType(objelement, RadioButtonList).Items
                                                        lst.Enabled = False
                                                    Next
                                                ElseIf Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "CHECKBOX" Then
                                                    For Each lst As ListItem In CType(objelement, CheckBoxList).Items
                                                        lst.Enabled = False
                                                    Next
                                                Else
                                                    objelement.Attributes.Add("disabled", "true")
                                                End If

                                                If (Me.Session(S_EDCUser).ToString() = EDCUser And
                                                    (Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "DATETIME" Or Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "TIME")) Then
                                                Else
                                                    ''Add by shivani pandya
                                                    If dr("cCRFDtlDataStatus").ToString = "B" Then
                                                        If Me.hndRepetitionNo.Value.Split(",").Length = 1 Then
                                                            PlaceMedEx.Controls.Add(GetEditButton("Edit", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), dr("vMedexType")))
                                                            PlaceMedEx.Controls.Add(GetUpdateButton("Update", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), dr("vMedexType")))
                                                        Else
                                                            Me.BtnSave.Visible = False
                                                            Me.btnSaveAndContinue.Visible = False
                                                        End If
                                                    Else
                                                        'PlaceMedEx.Controls.Add(GetDCFButton("DCF", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), IIf(Convert.ToString(dr("iSrNo")) = "", "0", dr("iSrNo")), Convert.ToString(dr("cDCFStatus")).Trim()))
                                                    End If
                                                    'PlaceMedEx.Controls.Add(GetEditButton("Edit", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), dr("vMedexType")))
                                                    'PlaceMedEx.Controls.Add(GetUpdateButton("Update", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("nCRFDtlNo"), dr("vMedexType")))
                                                End If
                                            End If
                                        ElseIf (dr("cCRFDtlDataStatus").ToString <> "B" AndAlso dr("itranNo") = 1) Then
                                            If Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "RADIO" Then
                                                For Each lst As ListItem In CType(objelement, RadioButtonList).Items
                                                    lst.Enabled = False
                                                Next
                                            ElseIf Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "CHECKBOX" Then
                                                For Each lst As ListItem In CType(objelement, CheckBoxList).Items
                                                    lst.Enabled = False
                                                Next

                                            ElseIf Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "STANDARDDATE" Then
                                                Dim id = dr("vMedExCode") + "R" + Convert.ToString(Repeatation) + "_1"
                                                Dim id1 = dr("vMedExCode") + "R" + Convert.ToString(Repeatation) + "_2"
                                                Dim id2 = dr("vMedExCode") + "R" + Convert.ToString(Repeatation) + "_3"
                                                Dim ddl As DropDownList = FindControl(id)
                                                ddl.Enabled = False
                                                Dim ddl1 As DropDownList = FindControl(id1)
                                                ddl1.Enabled = False
                                                Dim ddl2 As DropDownList = FindControl(id2)
                                                ddl2.Enabled = False

                                            ElseIf Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "STANDARDDATETIME" Then
                                                Dim id = dr("vMedExCode") + "R" + Convert.ToString(Repeatation) + "_1"
                                                Dim id1 = dr("vMedExCode") + "R" + Convert.ToString(Repeatation) + "_2"
                                                Dim id2 = dr("vMedExCode") + "R" + Convert.ToString(Repeatation) + "_3"
                                                Dim id3 = dr("vMedExCode") + "R" + Convert.ToString(Repeatation) + "_4"
                                                Dim id4 = dr("vMedExCode") + "R" + Convert.ToString(Repeatation) + "_5"
                                                Dim ddl As DropDownList = FindControl(id)
                                                ddl.Enabled = False
                                                Dim ddl1 As DropDownList = FindControl(id1)
                                                ddl1.Enabled = False
                                                Dim ddl2 As DropDownList = FindControl(id2)
                                                ddl2.Enabled = False
                                                Dim ddl3 As DropDownList = FindControl(id3)
                                                ddl3.Enabled = False
                                                Dim ddl4 As DropDownList = FindControl(id4)
                                                ddl4.Enabled = False
                                            Else
                                                objelement.Attributes.Add("disabled", "true")
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If


                    If Convert.ToString(dr("vMedExType")).Trim.ToUpper() <> "LABEL" Then
                        If dt.Columns.Contains("nCRFDtlNo") Then
                            PlaceMedEx.Controls.Add(GetAudittrailButton("AuditTrail", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode") + "R" + Convert.ToString(Repeatation), dr("iTranNo"), dr("nCRFDtlNo"), Convert.ToString(dr("vSourceResponse"))))
                        End If
                        If Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "COMBOGLOBALDICTIONARY" Then
                            ' Added to have comboglobal available for medical coder
                            objelement.Attributes.Add("disabled", "")
                            ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                            If Me.Session(vs_workflowstageidfordynamic) <> WorkFlowStageId_MedicalCoding Then
                                objelement.Attributes.Add("disabled", "true")
                            End If
                            ''Commented by nipun khant for dynamic review
                            'If Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_MedicalCoding Then
                            '    objelement.Attributes.Add("disabled", "true")
                            'End If
                        End If
                    End If

                    '#################################################### For Edit Check Query Added By Vivek Patel '####################################################'
                    If dt.Columns.Contains("vMedExCode") And dt_EditCheckQuery.Rows.Count > 0 Then
                        Dim dv_EditCheckQuery As New DataView(dt_EditCheckQuery)
                        Dim dt_distinctEditCheckQuery As DataTable = dv_EditCheckQuery.ToTable(True, "vMedExCode", "RepetitionNo")
                        For Each row As DataRow In dt_distinctEditCheckQuery.Rows
                            ''strDetail = row.Item("Detail")
                            If Convert.ToString(dr("vMedExCode")) = Convert.ToString(row("vMedExCode")) And dr("RepetitionNo") = row("RepetitionNo") Then
                                Dim dv_vMedExCode = New DataView(dt_EditCheckQuery)
                                dv_vMedExCode.RowFilter = "vMedExCode='" + dr("vMedExCode").ToString() + "'"
                                'PlaceMedEx.Controls.Add(GetEditCheckQueryButton("Edit Check Query", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), "VK", dv_vMedExCode.Count.ToString()))
                                PlaceMedEx.Controls.Add(GetEditCheckQueryButton("Edit Check Query", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), "R", Convert.ToString(Repeatation), "VK", Convert.ToString(dv_vMedExCode.Count)))
                                'hdnMedExCode.Value = String.Empty
                                'hdnMedExCode.Value = dr("vMedExCode").ToString()
                            End If
                        Next row
                    End If
                    '########################################################## For Edit Check Query '##########################################################'

                    PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                    '**********For User's readibility**************

                    If Me.ViewState(VS_DataStatus) <> CRF_DataEntryPending Then
                        PlaceMedEx.Controls.Add(New LiteralControl("<Tr id=""Repetition_" + Convert.ToString(dr("RepetitionNo")) + """  class=""Repetition_" + Convert.ToString(dr("RepetitionNo")) + """ width=""100%"" height=""2px"" ALIGN=LEFT style=""BACKGROUND-COLOR: #FFFFFF"">"))
                    Else
                        PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" height=""2px"" ALIGN=LEFT style=""BACKGROUND-COLOR: #FFFFFF"">"))
                    End If

                    PlaceMedEx.Controls.Add(New LiteralControl("<Td colspan=""3"" style=""white-space: nowrap; vertical-align:middle"">"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                    If Me.ViewState(VS_DataStatus) <> CRF_DataEntryPending Then
                        PlaceMedEx.Controls.Add(New LiteralControl("<Tr id=""Repetition_" + Convert.ToString(dr("RepetitionNo")) + """  class=""Repetition_" + Convert.ToString(dr("RepetitionNo")) + """ width=""100%"" height=""1px"" ALIGN=LEFT style=""BACKGROUND-COLOR: #1560a1"">")) '#FFC300
                    Else
                        PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" height=""1px"" ALIGN=LEFT style=""BACKGROUND-COLOR: #1560a1"">")) '#FFC300
                    End If

                    PlaceMedEx.Controls.Add(New LiteralControl("<Td colspan=""3"" style=""white-space: nowrap; vertical-align:middle"">"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                    If Me.ViewState(VS_DataStatus) <> CRF_DataEntryPending Then
                        PlaceMedEx.Controls.Add(New LiteralControl("<Tr id=""Repetition_" + Convert.ToString(dr("RepetitionNo")) + """  class=""Repetition_" + Convert.ToString(dr("RepetitionNo")) + """ width=""100%"" height=""2px"" ALIGN=LEFT style=""BACKGROUND-COLOR: #FFFFFF"">"))
                    Else
                        PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" height=""2px"" ALIGN=LEFT style=""BACKGROUND-COLOR: #FFFFFF"">"))
                    End If
                    PlaceMedEx.Controls.Add(New LiteralControl("<Td colspan=""3"" style=""white-space: nowrap; vertical-align:middle"">"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
                    '****************************************
                    j = j + 1
                Next dr

                i += 1
                PlaceMedEx.Controls.Add(New LiteralControl("</Table>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</Div>"))

            Next drGroup

            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
            PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
            PlaceMedEx.Controls.Add(New LiteralControl("</fieldset>"))
            PlaceMedEx.Controls.Add(New LiteralControl("</Table>"))
            Me.Session("PlaceMedEx") = PlaceMedEx.Controls



            'Added By Pratik Soni FOR DMS - To Set Defaul SOP To Textbox
            If Not IsNothing(Me.Request.QueryString("vmedexResult")) Then
                Dim medexResult As String = Me.Request.QueryString("vmedexResult").ToString.Trim()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetTextBoxValue", "document.getElementById('00085R1').value = '" + medexResult.ToString + "';", True)
            End If


            dt_MedExMst = New DataTable
            dt_MedExMst = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Copy()

            Me.trReviewCompleted.Style.Add("display", "")
            ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
            If Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() <> WorkFlowStageId_DataEntry AndAlso
                Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() <> WorkFlowStageId_FirstReview Then
                Me.BtnSave.Visible = False
                Me.btnSaveAndContinue.Visible = False
            End If
            If Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() = WorkFlowStageId_DataEntry Or
                               Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() = WorkFlowStageId_OnlyView Or
                                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Or
                                Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() = WorkFlowStageId_DataValidator Then
                Me.trReviewCompleted.Style.Add("display", "none")
            End If
            ''==============================================================================================
            ''=============Commented by nipun khant for dynamic review======================================
            'If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_DataEntry AndAlso _
            '    Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_FirstReview Then
            '    Me.BtnSave.Visible = False
            '    Me.btnSaveAndContinue.Visible = False
            'End If
            'If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_DataEntry Or _
            '                   Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_OnlyView Or _
            '                    Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Or _
            '                    Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_DataValidator Then
            '    Me.trReviewCompleted.Style.Add("display", "none")
            'End If
            ''==============================================================================================

            If Convert.ToString(Me.ViewState(VS_ReviewFlag)).Trim.ToUpper() = "YES" Or Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.trReviewCompleted.Style.Add("display", "none")
            End If

            ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
            If Me.Session(vs_workflowstageidfordynamic) <> WorkFlowStageId_DataEntry AndAlso Me.Session(vs_workflowstageidfordynamic) <> WorkFlowStageId_OnlyView AndAlso dcfFlag = True Then
                ''Commented by nipun khant for dynamic review
                'If Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_DataEntry AndAlso Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_OnlyView AndAlso dcfFlag = True Then

                'Add by shivani pandya for Review All and Review single
                dtData = Me.ViewState(VS_dtMedEx_Fill_Backup)
                dtGetData = dtData.DefaultView.ToTable(True, "iWorkFlowstageId")
                If dtGetData.Rows.Count = 1 Then
                    Me.trReviewCompleted.Style.Add("display", "")
                Else
                    Me.trReviewCompleted.Style.Add("display", "none")
                    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "DisableCheckbox", "DisableCheckbox();", True)
                End If
                'Me.trReviewCompleted.Style.Add("display", "")
            Else
                Me.trReviewCompleted.Style.Add("display", "none")
            End If
            Me.imgShow.Attributes.Add("onmouseover", "$('#" + Me.canal.ClientID + "').toggle('medium');")
            Me.canal.Attributes.Add("onmouseleave", "$('#" + Me.canal.ClientID + "').toggle('medium');")

            Me.imgActivityLegends.Attributes.Add("onmouseover", "$('#" + Me.divActivityLegends.ClientID + "').toggle('medium');")
            Me.divActivityLegends.Attributes.Add("onmouseleave", "$('#" + Me.divActivityLegends.ClientID + "').toggle('medium');")

            If Not FillEditDropDown() Then
                Me.objCommon.ShowAlert("Error While Filling Edit Drop Down", Me.Page)
                Return False
            End If

            If Not FillDirectUpdateDropDown() Then
                Me.objCommon.ShowAlert("Error While Filling Edit Drop Down", Me.Page)
                Return False
            End If

            '*************Electronic Signature***************************
            ''Commented by nipun khant for dynamic review
            'If Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_FinalReviewAndLock AndAlso Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
            '    Me.trName.Visible = False
            '    Me.trDesignation.Visible = False
            '    Me.trRemarks.Visible = False
            '    'Me.divAuthentication.Style.Add("height", "150px")
            'ElseIf Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_SecondReview AndAlso Me.Session(S_ScopeNo) = Scope_BABE Then
            '    Me.trName.Visible = False
            '    Me.trDesignation.Visible = False
            '    Me.trRemarks.Visible = False
            '    'Me.divAuthentication.Style.Add("height", "150px")
            'End If
            '==============================================================

            ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
            Dim ds_reviewer As New DataSet
            Dim dv_reviewer As DataView
            ds_reviewer = CType(Me.Session(Vs_dsReviewerlevel), DataSet)
            dv_reviewer = ds_reviewer.Tables(0).Copy.DefaultView
            dv_reviewer.RowFilter = "iReviewWorkflowStageId = " + Me.Session(S_WorkFlowStageId).ToString() + " and vUserTypeCode = '" + Me.Session(S_UserType) + "'"
            If dv_reviewer.ToTable().Rows.Count = 1 Then
                If dv_reviewer.ToTable().Rows(0)("cAuthenticationDialog").ToString().ToUpper() = "N" Then
                    Me.trName.Visible = False
                    Me.trDesignation.Visible = False
                    Me.trRemarks.Visible = False
                    Me.trpassword.Visible = False

                ElseIf dv_reviewer.ToTable().Rows(0)("cAuthenticationDialog").ToString().ToUpper() = "Y" Then
                    Me.trName.Visible = True
                    Me.trDesignation.Visible = True
                    Me.trRemarks.Visible = True
                    Me.trpassword.Visible = True

                End If
            End If
            '==============================================================
            Me.lblSignername.Text = Me.Session(S_FirstName).ToString() + " " + Me.Session(S_LastName).ToString()
            Dim dt_Profiles As New DataTable
            dt_Profiles = CType(Me.Session(S_Profiles), DataTable)
            Dim dv_Profiles As DataView
            dv_Profiles = dt_Profiles.DefaultView
            dv_Profiles.RowFilter = "vUserTypeCode = '" + Me.Session(S_UserType).ToString() + "'"
            Me.lblSignerDesignation.Text = dv_Profiles.ToTable.Rows(0)("vUserTypeName").ToString()
            'Me.lblSignDateTime.Text = Me.objHelp.GetServerDateTime().ToString("dd-MMM-yyyy hh:mm")
            Me.lblSignRemarks.Text = "I attest to the accuracy and integrity of the data being reviewed."

            '*************Electronic Signature***************************

            If strTimeDifference <> "" Then
                objCommon.ShowAlert(strTimeDifference, Me.Page)
            End If
            '************Repeatation Show/Hode logic******************
            dvRepetationShowHide = Me.Session("RepeationShowHide")
            dvRepetationShowHide.RowFilter = "vActivityId = '" + Me.HFActivityId.Value.Trim() + "' And iNodeId = " + Me.HFNodeId.Value.Trim()
            'Me.imgViewAll.Style.Add("display", "none")
            Me.ddlRepeatNo.Style.Add("display", "none") ' dv.ToTable().Rows.Count
            If dvRepetationShowHide.ToTable().Rows.Count > 0 Then
                If Convert.ToString(dvRepetationShowHide.ToTable().Rows(0)("cIsRepeatable")).Trim.ToUpper() = "N" Then
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "RepeatitionShowHide", "RepeatitionShowHide();", True)
                End If
            End If
            '******************************            
            'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "RepeatitionShowHide", "RepeatitionShowHide();", True)

            'Change color of Repeatition dropdown
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "getRepeatitionDropDownColor", "getRepeatitionDropDownColor();", True)
            Return True
        Catch ex As Exception

            ShowErrorMessage(ex.Message.ToString, "....GenCall_ShowUI")
        End Try
    End Function

#End Region

#Region "Getlable"

    Private Function Getlable(ByVal vlabelName As String, ByVal Id As String, Optional ByVal vFieldType As String = "") As Label
        Dim lab As Label
        lab = New Label
        lab.ID = "Lab" & Id
        lab.Text = vlabelName.Trim()
        lab.SkinID = "LabelDisplay"   '' Added By Prayag For Micro Symbol Change to m
        lab.ForeColor = System.Drawing.Color.FromName("Navy")
        lab.Attributes.Add("Style", "font-size: 13px;font-weight: bold;")
        If vFieldType.ToUpper.Trim() = "IMPORT" Then
            lab.Visible = False
        End If
        Getlable = lab
    End Function

    Private Function GetlableWithHistory(ByVal vlabelName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal Repeatition As Integer) As Label
        Dim lnk As Label

        'Change for Header bold by shivani pandya
        Dim HeaderBold() As String
        Dim HeaderData() As String

        lnk = New Label
        lnk.ID = "Lnk" + MedExCode
        lnk.Text = vlabelName.Trim()
        lnk.SkinID = "LabelDisplay"

        lnk.CssClass = "Label"
        lnk.Style.Add("word-wrap", "break-word")
        lnk.Style.Add("white-space", "")

        'Change for Header bold by shivani pandya
        HeaderBold = vlabelName.Split(" ")
        If HeaderBold.Length > 1 Then
            HeaderData = HeaderBold(0).Split(".")
            If HeaderData.Length > 1 Then
                If HeaderData(1) = "0" Then
                    lnk.Style.Add("font-weight", "bold")
                End If
            End If
        End If

        'Commented because applicable when displaying attributes in table format
        'If Repeatition <> 1 Then
        '    lnk.Style.Add("display", "none")
        'End If
        GetlableWithHistory = lnk
    End Function

#End Region

#Region "GetButtons"

    Private Function GetButtons(ByVal BtnName As String, ByVal Id As String) As Boolean
        Dim Btn As Button
        Dim index As Integer
        Dim StrGroupCode_arry() As String
        Dim StrGroupDesc_arry() As String
        Try
            StrGroupCode_arry = Id.Split(",")
            StrGroupDesc_arry = BtnName.Split(",")
            For index = 0 To StrGroupCode_arry.Length - 1
                Btn = New Button
                Btn.ID = "Btn" & StrGroupCode_arry(index)
                Btn.Text = " " & StrGroupDesc_arry(index).Trim() & " "

                If StrGroupDesc_arry(index).Trim().Length > 30 Then
                    Btn.Text = " " & StrGroupDesc_arry(index).Substring(0, 30).Trim() & "... "
                End If

                Btn.ToolTip = StrGroupDesc_arry(index).Trim()
                Btn.CssClass = "buttonForTabActive"

                If Convert.ToString(Me.HFActivateTab.Value).Trim() <> "" Or
                        Convert.ToString(Me.Session(S_SelectedTab)).Trim() <> "" Then

                    If Convert.ToString(Me.HFActivateTab.Value).Trim() = StrGroupCode_arry(index) Then
                        Btn.CssClass = "buttonForTabInActive"
                    ElseIf Convert.ToString(Me.Session(S_SelectedTab)).Trim() = StrGroupCode_arry(index) Then
                        Btn.CssClass = "buttonForTabInActive"
                    End If

                Else
                    If index = 0 Then
                        Btn.CssClass = "buttonForTabInActive"
                    End If
                End If

                Btn.OnClientClick = "return DisplayDiv('" & StrGroupCode_arry(index) & "','" & Id & "');"
                PlaceMedEx.Controls.Add(Btn)
                PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
            Next

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..GetButtons")
            Return False
        End Try
    End Function

#End Region

#Region "GetObject"

    Private Function GetObject(ByVal vFieldType As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String,
                               ByVal Id As String, ByVal vMedExValues As String, ByVal dtValues As String,
                               Optional ByVal Validation As String = "", Optional ByVal length As String = "",
                               Optional ByVal AlertonValue As String = "", Optional ByVal AlertMsg As String = "",
                               Optional ByVal HighRange As String = "0", Optional ByVal LowRange As String = "0",
                               Optional ByVal RefTable As String = "", Optional ByVal RefColumn As String = "",
                               Optional ByVal IsNotNull As String = "", Optional ByVal NumericScale As String = "0",
                               Optional ByVal TargetMedExCode As String = "", Optional ByVal DependecyMedExCodes As String = "",
                               Optional ByVal DependentMedExValue As String = "", Optional ByVal TranNo As Integer = 0, Optional ByVal Numeric As String = "") As Object


        Dim txt As TextBox
        Dim ddl As DropDownList
        Dim FileBro As FileUpload
        Dim RBL As RadioButtonList
        Dim CBL As CheckBoxList
        Dim lbl As Label
        Dim strDeependentMedExValue As String = String.Empty
        Dim ConvertDateTime As DateTime

        AlertMsg = AlertMsg.Replace(vbLf, [String].Empty)
        AlertMsg = AlertMsg.Replace(vbCr, [String].Empty)
        AlertMsg = AlertMsg.Replace(vbTab, [String].Empty)

        If DependecyMedExCodes <> "" Then

            For Each Str As String In DependentMedExValue.Split(",")
                strDeependentMedExValue += "[" + Str + "],"
            Next
            If strDeependentMedExValue <> "" Then
                strDeependentMedExValue = strDeependentMedExValue.Substring(0, strDeependentMedExValue.LastIndexOf(","))
            End If
        End If


        Select Case vFieldType.ToUpper.Trim()

            Case "TEXTBOX"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                If IsNotNull.ToUpper() = "Y" Then
                    txt.CssClass = "Required textBox"
                End If

                If Not dtValues = "" Then
                    txt.Text = dtValues
                    'txt.Enabled = False
                    txt.Attributes.Add("disabled", "true")
                End If

                ' If parseDate(txt.Text) Then
                '     txt.Text = Convert.ToString(CDate(txt.Text).ToString("dd-MMM-yyyy HH:mm"))
                ' End If
                txt.Attributes.Add("title", dtValues)

                HighRange = IIf(HighRange = "", "0", HighRange)
                LowRange = IIf(LowRange = "", "0", LowRange)

                Dim checktype As String = ""
                Dim msg As String = ""

                If Validation = "" Or Validation = "NA" Then
                    checktype = "0"
                    msg = "No Validation"
                ElseIf Validation = GeneralModule.Val_AN Then
                    checktype = GeneralModule.Validation_Alphanumeric.ToString()
                    msg = "Please Enter AlphaNumeric Value"
                ElseIf Validation = GeneralModule.Val_NU Then
                    checktype = GeneralModule.Validation_Numeric.ToString()
                    msg = "Please Enter Numeric or Blank Value"
                ElseIf Validation = GeneralModule.Val_IN Then
                    checktype = GeneralModule.Validation_Integer.ToString()
                    msg = "Please Enter Integer or Blank Value"
                ElseIf Validation = GeneralModule.Val_AL Then
                    checktype = GeneralModule.Validation_Alphabet.ToString()
                    msg = "Please Enter Alphabets only"
                ElseIf Validation = GeneralModule.Val_NNI Then
                    checktype = GeneralModule.Validation_NotNullInteger.ToString()
                    msg = "Please Enter Integer Value"
                ElseIf Validation = GeneralModule.Val_NNU Then
                    checktype = GeneralModule.Validation_NotNullNumeric.ToString()
                    msg = "Please Enter Numeric Value"
                End If

                'If AlertonValue.Trim() <> "" OrElse length.Trim() <> "" Then
                '    checktype = "0"
                'End If

                'txt.Attributes.Add("onblur", "ValidateTextbox('" & checktype & "',this,'" & msg & "'," & _
                '                           HighRange & "," & LowRange & ",'" & AlertonValue & "','" & AlertMsg.Replace("'", "\'") & "','" & length & "','" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")
                txt.Attributes.Add("onblur", "ValidateTextbox('" & checktype & "',this,'" & msg & "'," &
                                          HighRange & "," & LowRange & ",'" & AlertonValue & "','" & AlertMsg.Replace("'", "\'") & "','" & length & "','" & IsNotNull & "','btnUpdate" &
                                          MedExGroupCode & MedExSubGroupCode & Id & "','" & HFNumericScale.Value & "' ,'" & Validation & "','" & TargetMedExCode & "');")


                If Id = GeneralModule.Medex_Oral_Body_Temperature_F Then

                    txt.Attributes.Add("onblur", "F2C('" & GeneralModule.Medex_Oral_Body_Temperature_F.Trim() & "','" & GeneralModule.Medex_Oral_Body_Temperature_C.Trim() & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "','" & TargetMedExCode & "');")

                End If
                '************************************
                'added on 24-Apr-2010 by Deepak Singh
                If Id = GeneralModule.medex_DosingLabel Then
                    txt.Attributes.Add("onchange", "SetDosingTime('00650','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "')")
                End If

                txt.Attributes.Add("onfocus", "SetValue(this.id,this.value);")

                If Numeric <> "" Then
                    txt.Attributes.Add("onkeypress", "return LengthValidation(" & Numeric.Split(",")(0) & "," & Numeric.Split(",")(1) & "," & "this" & ")")
                End If


                Dim wStr As String
                Dim ds_Dependancy As DataSet = Nothing
                Dim Status As String = ""
                Dim ds_MedExDependenctValue As DataSet = Nothing
                Dim wStrMedexValue As String = ""
                Dim sourcemedex As String = ""
                Dim Sourcevalue As String = ""
                Dim Targetvalue As String = ""

                If DependecyMedExCodes <> "" Then


                    Dim vMedEx As String = Convert.ToString(DependecyMedExCodes.Split(",")(0))

                    wStrMedexValue = "  Select * From MedexDependency Where vWorkspaceId = " + HFParentWorkspaceId.Value + " AND vTargetMedExCode in (" + Id.Substring(0, 5) + ") AND cStatusIndi <> 'D'"
                    ds_MedExDependenctValue = Me.objHelp.GetResultSet(wStrMedexValue, "MedExDependency")

                    If Not ds_MedExDependenctValue Is Nothing AndAlso ds_MedExDependenctValue.Tables.Count > 0 AndAlso ds_MedExDependenctValue.Tables(0).Rows.Count > 0 Then
                        sourcemedex = ds_MedExDependenctValue.Tables(0).Rows(0)("vSourceMedExCode")
                        Sourcevalue = ds_MedExDependenctValue.Tables(0).Rows(0)("vMedExValue")
                    End If

                    If sourcemedex <> "" Then

                        wStr = "Select CRFDTL.cDataStatus, CRFSUBDTL.vMedExResult  From CRFHDR INNER JOIN CRFDTL On CRFHDR.nCRFHDRNo = CRFDTL.nCRFHDRNo INNER JOIN CRFSUBDTL On CRFSUBDTL.nCRFDtlNo = CRFDTL.nCRFDtlNo   Where vWorkSpaceId = '" + Request.QueryString("WorkSpaceId") + "' AND INodeId = " + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + " AND vSubjectId = '" + Request.QueryString("SubjectId") + "' AND CRFHDR.cStatusIndi <> 'D'AND CRFDTL.cStatusIndi <> 'D'  AND vMedExCode = '" + sourcemedex + "'   Order by iTranNo desc"
                        ds_Dependancy = Me.objHelp.GetResultSet(wStr, "MedExDependency")

                    End If
                    If Not ds_Dependancy Is Nothing AndAlso ds_Dependancy.Tables.Count > 0 AndAlso ds_Dependancy.Tables(0).Rows.Count > 0 Then
                        Status = ds_Dependancy.Tables(0).Rows(0)("cDataStatus")
                        Targetvalue = ds_Dependancy.Tables(0).Rows(0)("vMedExResult")
                    Else
                        Status = "D"
                    End If



                    If DependecyMedExCodes.Contains("[" + Id.Substring(0, Id.LastIndexOf("R")) + "]") And Status = "D" Then
                        txt.Enabled = False
                        'txt.Attributes.Add("disabled", "disabled")
                    ElseIf Targetvalue.ToUpper() = Sourcevalue.ToUpper() Then
                        txt.Enabled = True
                    ElseIf Targetvalue.ToUpper() <> Sourcevalue.ToUpper() And Status <> "" Then
                        txt.Enabled = False
                    End If
                End If
                If TargetMedExCode <> "" Then
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "BindTrigger" + Id, "BindTrigger('" + txt.ClientID + "','blur');", True)
                End If



                GetObject = txt


            Case "COMBOBOX"
                Dim Arrvalue() As String = Nothing
                Dim i As Integer
                Dim ds_ddl As New DataSet
                Dim estr As String = String.Empty

                ddl = New DropDownList
                ddl.ID = Id
                ddl.CssClass = "dropDownList"

                If IsNotNull.ToUpper() = "Y" Then
                    ddl.CssClass = "Required dropDownList"
                End If

                If RefTable.Trim() <> "" Then

                    If Not Me.objHelp.GetFieldsOfTable(RefTable.Trim(), RefColumn.Trim(), "", ds_ddl, estr) Then
                        Me.objCommon.ShowAlert(estr, Me.Page())
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
                        ddl.Items.Add(New ListItem(Arrvalue(i).Trim(), Arrvalue(i).Trim()))
                        ddl.Items(i).Attributes.Add("title", Arrvalue(i).Trim())
                    Next

                    If Not dtValues = "" Then
                        ddl.SelectedValue = dtValues.Trim()
                        ddl.Attributes.Add("title", dtValues)
                        'ddl.Enabled = False
                        ddl.Attributes.Add("disabled", "true")
                    End If

                End If

                ddl.Attributes.Add("onblur", "checkddlNotNull('" & ddl.ClientID & "','" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")

                'If AlertonValue.Trim() <> "" Then
                ddl.Attributes.Add("onchange", "ddlAlerton('" & ddl.ClientID & "','" & AlertonValue & "','" & AlertMsg.Replace("'", "\'") & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "','" & TargetMedExCode & "');")
                'End If

                ddl.Attributes.Add("onfocus", "SetValue('" & ddl.ClientID & "','" + Convert.ToString(dtValues).Trim() + "');")

                Dim wStr As String
                Dim ds_Dependancy As DataSet = Nothing
                Dim Status As String = ""
                Dim ds_MedExDependenctValue As DataSet = Nothing
                Dim wStrMedexValue As String = ""
                Dim sourcemedex As String = ""
                Dim Sourcevalue As String = ""
                Dim Targetvalue As String = ""

                If DependecyMedExCodes <> "" Then


                    Dim vMedEx As String = Convert.ToString(DependecyMedExCodes.Split(",")(0))

                    wStrMedexValue = "  Select * From MedexDependency Where vWorkspaceId = " + HFParentWorkspaceId.Value + " AND vTargetMedExCode in (" + Id.Substring(0, 5) + ") AND cStatusIndi <> 'D'"
                    ds_MedExDependenctValue = Me.objHelp.GetResultSet(wStrMedexValue, "MedExDependency")

                    If Not ds_MedExDependenctValue Is Nothing AndAlso ds_MedExDependenctValue.Tables.Count > 0 AndAlso ds_MedExDependenctValue.Tables(0).Rows.Count > 0 Then
                        sourcemedex = ds_MedExDependenctValue.Tables(0).Rows(0)("vSourceMedExCode")
                        Sourcevalue = ds_MedExDependenctValue.Tables(0).Rows(0)("vMedExValue")
                    End If
                    If sourcemedex <> "" Then
                        wStr = "Select CRFDTL.cDataStatus, CRFSUBDTL.vMedExResult  From CRFHDR INNER JOIN CRFDTL On CRFHDR.nCRFHDRNo = CRFDTL.nCRFHDRNo INNER JOIN CRFSUBDTL On CRFSUBDTL.nCRFDtlNo = CRFDTL.nCRFDtlNo   Where vWorkSpaceId = '" + Request.QueryString("WorkSpaceId") + "' AND INodeId = " + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + " AND vSubjectId = '" + Request.QueryString("SubjectId") + "' AND CRFHDR.cStatusIndi <> 'D'AND CRFDTL.cStatusIndi <> 'D'  AND vMedExCode = '" + sourcemedex + "'   Order by iTranNo desc"
                        ds_Dependancy = Me.objHelp.GetResultSet(wStr, "MedExDependency")
                    End If
                    If Not ds_Dependancy Is Nothing AndAlso ds_Dependancy.Tables.Count > 0 AndAlso ds_Dependancy.Tables(0).Rows.Count > 0 Then
                        Status = ds_Dependancy.Tables(0).Rows(0)("cDataStatus")
                        Targetvalue = ds_Dependancy.Tables(0).Rows(0)("vMedExResult")
                    Else
                        Status = "D"
                    End If



                    If DependecyMedExCodes.Contains("[" + Id.Substring(0, Id.LastIndexOf("R")) + "]") And Status = "D" Then
                        ddl.Enabled = False
                        'ddl.Attributes.Add("disabled", "disabled")
                    ElseIf Targetvalue.ToUpper() = Sourcevalue.ToUpper() Then
                        ddl.Enabled = True
                        'ddl.Attributes.Add("disabled", "disabled")
                    ElseIf Targetvalue.ToUpper() <> Sourcevalue.ToUpper() And Status <> "" Then
                        ddl.Enabled = False
                    End If
                End If

                If TargetMedExCode <> "" Then
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "BindTrigger" + Id, "BindTrigger('" + ddl.ClientID + "','change');", True)
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

                If IsNotNull.ToUpper() = "Y" Then
                    RBL.CssClass = "Required"
                End If

                If RefTable.Trim() <> "" Then

                    If Not Me.objHelp.GetFieldsOfTable(RefTable.Trim(), RefColumn.Trim(), "", ds_Radio, estr) Then
                        Me.objCommon.ShowAlert(estr, Me.Page())
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
                        RBL.Items.Add(New ListItem(Arrvalue(i).Trim(), Arrvalue(i).Trim().ToUpper()))
                        RBL.Items(i).Attributes.Add("onblur", "checkRBLNotNull('" & RBL.ClientID & "','" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")
                        RBL.Items(i).Attributes.Add("onfocus", "SetValue('" & RBL.ClientID & "','" + Convert.ToString(dtValues).Trim() + "');")
                        RBL.Items(i).Attributes.Add("onclick", "Alerton('" & RBL.ClientID & "','" & AlertonValue & "','" & AlertMsg.Replace("'", "\'") & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "','" + Convert.ToString(dtValues).Trim() + "','" & TargetMedExCode & "');")
                    Next
                    If Not dtValues = "" Then
                        RBL.SelectedValue = dtValues.Trim().ToUpper()
                        'RBL.Enabled = False
                        RBL.Attributes.Add("disabled", "true")
                    End If

                End If

                RBL.RepeatDirection = RepeatDirection.Horizontal
                RBL.RepeatColumns = 3

                RBL.Attributes.Add("onblur", "checkRBLNotNull('" & RBL.ClientID & "','" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")

                'If AlertonValue.Trim() <> "" Then
                '    RBL.Attributes.Add("onclick", "Alerton('" & RBL.ClientID & "','" & AlertonValue & "','" & AlertMsg.Replace("'", "\'") & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")
                'End If

                RBL.Attributes.Add("ondblclick", "RemoveSelection('" & RBL.ClientID & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")
                'RBL.Attributes.Add("onfocus", "SetValue('" & RBL.ClientID & "','" + Convert.ToString(dtValues).Trim() + "');")

                Dim wStr As String
                Dim ds_Dependancy As DataSet = Nothing
                Dim Status As String = ""
                Dim ds_MedExDependenctValue As DataSet = Nothing
                Dim wStrMedexValue As String = ""
                Dim sourcemedex As String = ""
                Dim Sourcevalue As String = ""
                Dim Targetvalue As String = ""


                If DependecyMedExCodes <> "" Then


                    Dim vMedEx As String = Convert.ToString(DependecyMedExCodes.Split(",")(0))

                    wStrMedexValue = "  Select * From MedexDependency Where vWorkspaceId = " + HFParentWorkspaceId.Value + " AND vTargetMedExCode in (" + Id.Substring(0, 5) + ") AND cStatusIndi <> 'D'"
                    ds_MedExDependenctValue = Me.objHelp.GetResultSet(wStrMedexValue, "MedExDependency")

                    If Not ds_MedExDependenctValue Is Nothing AndAlso ds_MedExDependenctValue.Tables.Count > 0 AndAlso ds_MedExDependenctValue.Tables(0).Rows.Count > 0 Then
                        sourcemedex = ds_MedExDependenctValue.Tables(0).Rows(0)("vSourceMedExCode")
                        Sourcevalue = ds_MedExDependenctValue.Tables(0).Rows(0)("vMedExValue")
                    End If
                    If sourcemedex <> "" Then
                        wStr = "Select CRFDTL.cDataStatus, CRFSUBDTL.vMedExResult  From CRFHDR INNER JOIN CRFDTL On CRFHDR.nCRFHDRNo = CRFDTL.nCRFHDRNo INNER JOIN CRFSUBDTL On CRFSUBDTL.nCRFDtlNo = CRFDTL.nCRFDtlNo   Where vWorkSpaceId = '" + Request.QueryString("WorkSpaceId") + "' AND INodeId = " + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + " AND vSubjectId = '" + Request.QueryString("SubjectId") + "' AND CRFHDR.cStatusIndi <> 'D'AND CRFDTL.cStatusIndi <> 'D'  AND vMedExCode = '" + sourcemedex + "'   Order by iTranNo desc"
                        ds_Dependancy = Me.objHelp.GetResultSet(wStr, "MedExDependency")
                    End If
                    If Not ds_Dependancy Is Nothing AndAlso ds_Dependancy.Tables.Count > 0 AndAlso ds_Dependancy.Tables(0).Rows.Count > 0 Then
                        Status = ds_Dependancy.Tables(0).Rows(0)("cDataStatus")
                        Targetvalue = ds_Dependancy.Tables(0).Rows(0)("vMedExResult")
                    Else
                        Status = "D"
                    End If



                    If DependecyMedExCodes.Contains("[" + Id.Substring(0, Id.LastIndexOf("R")) + "]") And Status = "D" Then
                        RBL.Enabled = False
                        'RBL.Attributes.Add("disabled", "disabled")
                    ElseIf Targetvalue.ToUpper() = Sourcevalue.ToUpper() Then
                        RBL.Enabled = True
                    ElseIf Targetvalue.ToUpper() <> Sourcevalue.ToUpper() And Status <> "" Then
                        RBL.Enabled = False
                    End If
                End If

                If TargetMedExCode <> "" Then
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "BindTrigger" + Id, "BindTrigger('" + RBL.ClientID + "','click');", True)
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

                If IsNotNull.ToUpper() = "Y" Then
                    CBL.CssClass = "Required"
                End If

                If RefTable.Trim() <> "" Then

                    If Not Me.objHelp.GetFieldsOfTable(RefTable.Trim(), RefColumn.Trim(), "", ds_Check, estr) Then
                        Me.objCommon.ShowAlert(estr, Me.Page())
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
                        CBL.Items.Add(New ListItem(Arrvalue(i).Trim(), Arrvalue(i).Trim()))
                    Next

                End If

                CBL.ClearSelection()
                If Not dtValues = "" Then
                    Defvalue = Split(dtValues, ",")
                    Dim SetValue As String = ""
                    'CBL.Enabled = False
                    CBL.Attributes.Add("disabled", "true")
                    For i = 0 To Defvalue.Length - 1
                        For Each item As ListItem In CBL.Items
                            If item.Value.Trim().ToUpper() = Defvalue(i).Trim().ToUpper() Then
                                item.Selected = True
                                If SetValue.Trim() <> "" Then
                                    SetValue += "##"
                                End If
                                SetValue += Defvalue(i).Trim()
                                'Exit For
                            End If
                            item.Attributes.Add("onfocus", "SetValue('" & CBL.ClientID & "','" + SetValue + "');")
                            item.Attributes.Add("onblur", "checkCBLNotNull('" & CBL.ClientID & "','" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")
                            item.Attributes.Add("onclick", "AlertonCheckBox(this.id,'" & AlertonValue & "','" & AlertMsg.Replace("'", "\'") & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "','" & SetValue & "','" & TargetMedExCode & "','" & Id & "');")
                        Next item
                    Next i

                Else
                    For Each item As ListItem In CBL.Items
                        item.Attributes.Add("onfocus", "SetValue('" & CBL.ClientID & "','');")
                        item.Attributes.Add("onblur", "checkCBLNotNull('" & CBL.ClientID & "','" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")
                        item.Attributes.Add("onclick", "AlertonCheckBox(this,'" & AlertonValue & "','" & AlertMsg.Replace("'", "\'") & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "','','" & TargetMedExCode & "','" & Id & "');")
                    Next item

                End If

                CBL.RepeatDirection = RepeatDirection.Horizontal
                CBL.RepeatColumns = 3


                CBL.Attributes.Add("onblur", "checkCBLNotNull('" & CBL.ClientID & "','" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")

                Dim wStr As String
                Dim ds_Dependancy As DataSet = Nothing
                Dim Status As String = ""
                Dim ds_MedExDependenctValue As DataSet = Nothing
                Dim wStrMedexValue As String = ""
                Dim sourcemedex As String = ""
                Dim Sourcevalue As String = ""
                Dim Targetvalue As String = ""

                If DependecyMedExCodes <> "" Then

                    Dim vMedEx As String = Convert.ToString(DependecyMedExCodes.Split(",")(0))

                    wStrMedexValue = "  Select * From MedexDependency Where vWorkspaceId = " + HFParentWorkspaceId.Value + " AND vTargetMedExCode in (" + Id.Substring(0, 5) + ") AND cStatusIndi <> 'D'"
                    ds_MedExDependenctValue = Me.objHelp.GetResultSet(wStrMedexValue, "MedExDependency")

                    If Not ds_MedExDependenctValue Is Nothing AndAlso ds_MedExDependenctValue.Tables.Count > 0 AndAlso ds_MedExDependenctValue.Tables(0).Rows.Count > 0 Then
                        sourcemedex = ds_MedExDependenctValue.Tables(0).Rows(0)("vSourceMedExCode")
                        Sourcevalue = ds_MedExDependenctValue.Tables(0).Rows(0)("vMedExValue")
                    End If
                    If sourcemedex <> "" Then
                        wStr = "Select CRFDTL.cDataStatus, CRFSUBDTL.vMedExResult  From CRFHDR INNER JOIN CRFDTL On CRFHDR.nCRFHDRNo = CRFDTL.nCRFHDRNo INNER JOIN CRFSUBDTL On CRFSUBDTL.nCRFDtlNo = CRFDTL.nCRFDtlNo   Where vWorkSpaceId = '" + Request.QueryString("WorkSpaceId") + "' AND INodeId = " + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + " AND vSubjectId = '" + Request.QueryString("SubjectId") + "' AND CRFHDR.cStatusIndi <> 'D'AND CRFDTL.cStatusIndi <> 'D'  AND vMedExCode = '" + sourcemedex + "'   Order by iTranNo desc"
                        ds_Dependancy = Me.objHelp.GetResultSet(wStr, "MedExDependency")
                    End If

                    If Not ds_Dependancy Is Nothing AndAlso ds_Dependancy.Tables.Count > 0 AndAlso ds_Dependancy.Tables(0).Rows.Count > 0 Then
                        Status = ds_Dependancy.Tables(0).Rows(0)("cDataStatus")
                        Targetvalue = ds_Dependancy.Tables(0).Rows(0)("vMedExResult")
                    Else
                        Status = "D"
                    End If

                End If

                If DependecyMedExCodes.Contains("[" + Id.Substring(0, Id.LastIndexOf("R")) + "]") And Status = "D" Then

                    ''CBL.Enabled = False   Commented by Prayag Patel For not Data not Save while having a Dependancy in attribute
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "Cblenable", "Cblenable('" + CBL.ClientID + "');", True)
                ElseIf Targetvalue.ToUpper() = Sourcevalue.ToUpper() Then
                    'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "Cblenable", "Cblenable('" + CBL.ClientID + "');", True)
                ElseIf Targetvalue.ToUpper() <> Sourcevalue.ToUpper() And Status <> "" Then
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "Cblenable", "Cblenable('" + CBL.ClientID + "');", True)
                End If



                If TargetMedExCode <> "" Then
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "BindTrigger" + Id, "BindTrigger('" + CBL.ClientID + "','click');", True)
                End If

                GetObject = CBL

            Case "FILE"
                FileBro = New FileUpload
                FileBro.ID = "FU" + Id
                FileBro.EnableViewState = True
                FileBro.CssClass = "textBox"

                If IsNotNull.ToUpper() = "Y" Then
                    FileBro.CssClass = "Required textBox"
                End If

                If DependecyMedExCodes.Contains("[" + Id.Substring(0, Id.LastIndexOf("R")) + "]") Then
                    FileBro.Enabled = False
                    'FileBro.Attributes.Add("disabled", "disabled")
                End If

                GetObject = FileBro

            Case "TEXTAREA"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox crfentrycontrol"
                If IsNotNull.ToUpper() = "Y" Then
                    txt.CssClass = "Required textBox crfentrycontrol"
                End If

                txt.Attributes.Add("title", dtValues)

                If Not dtValues = "" Then
                    txt.Text = dtValues
                    'txt.Enabled = False
                    txt.Attributes.Add("disabled", "true")
                End If
                txt.TextMode = TextBoxMode.MultiLine
                txt.Width = 275
                txt.Height = 70

                HighRange = IIf(HighRange = "", "0", HighRange)
                LowRange = IIf(LowRange = "", "0", LowRange)

                Dim checktype As String = ""
                Dim msg As String = ""

                If Validation = "" Or Validation = "NA" Then
                    checktype = "0"
                    msg = "No Validation"
                ElseIf Validation = GeneralModule.Val_AN Then
                    checktype = GeneralModule.Validation_Alphanumeric.ToString()
                    msg = "Please Enter AlphaNumeric Value"
                ElseIf Validation = GeneralModule.Val_NU Then
                    checktype = GeneralModule.Validation_Numeric.ToString()
                    msg = "Please Enter Numeric or Blank Value"
                ElseIf Validation = GeneralModule.Val_IN Then
                    checktype = GeneralModule.Validation_Integer.ToString()
                    msg = "Please Enter Integer or Blank Value"
                ElseIf Validation = GeneralModule.Val_AL Then
                    checktype = GeneralModule.Validation_Alphabet.ToString()
                    msg = "Please Enter Alphabets only"
                ElseIf Validation = GeneralModule.Val_NNI Then
                    checktype = GeneralModule.Validation_NotNullInteger.ToString()
                    msg = "Please Enter Integer Value"
                ElseIf Validation = GeneralModule.Val_NNU Then
                    checktype = GeneralModule.Validation_NotNullNumeric.ToString()
                    msg = "Please Enter Numeric Value"
                End If

                'If AlertonValue.Trim() <> "" OrElse length.Trim() <> "" Then
                '    checktype = "0"
                'End If

                'txt.Attributes.Add("onblur", "ValidateTextbox('" & checktype & "',this,'" & msg & "'," & _
                '                           HighRange & "," & LowRange & ",'" & AlertonValue & "','" & AlertMsg.Replace("'", "\'") & "','" & length & "','" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")
                txt.Attributes.Add("onblur", "ValidateTextbox('" & checktype & "',this,'" & msg & "'," &
                                                              HighRange & "," & LowRange & ",'" & AlertonValue & "','" &
                                                              AlertMsg.Replace("'", "\'") & "','" & length & "','" & IsNotNull & "','btnUpdate" &
                                                              MedExGroupCode & MedExSubGroupCode & Id & "','" & HFNumericScale.Value & "' ,'" &
                                                              Validation & "','" & TargetMedExCode & "');")
                txt.Attributes.Add("onfocus", "SetValue(this.id,this.value);")

                Dim wStr As String
                Dim ds_Dependancy As DataSet = Nothing
                Dim Status As String = ""
                Dim ds_MedExDependenctValue As DataSet = Nothing
                Dim wStrMedexValue As String = ""
                Dim sourcemedex As String = ""
                Dim Sourcevalue As String = ""
                Dim Targetvalue As String = ""

                If DependecyMedExCodes <> "" Then

                    Dim vMedEx As String = Convert.ToString(DependecyMedExCodes.Split(",")(0))

                    wStrMedexValue = "  Select * From MedexDependency Where vWorkspaceId = " + HFParentWorkspaceId.Value + " AND vTargetMedExCode in (" + Id.Substring(0, 5) + ") AND cStatusIndi <> 'D'"
                    ds_MedExDependenctValue = Me.objHelp.GetResultSet(wStrMedexValue, "MedExDependency")

                    If Not ds_MedExDependenctValue Is Nothing AndAlso ds_MedExDependenctValue.Tables.Count > 0 AndAlso ds_MedExDependenctValue.Tables(0).Rows.Count > 0 Then
                        sourcemedex = ds_MedExDependenctValue.Tables(0).Rows(0)("vSourceMedExCode")
                        Sourcevalue = ds_MedExDependenctValue.Tables(0).Rows(0)("vMedExValue")
                    End If
                    If sourcemedex <> "" Then
                        wStr = "Select CRFDTL.cDataStatus, CRFSUBDTL.vMedExResult  From CRFHDR INNER JOIN CRFDTL On CRFHDR.nCRFHDRNo = CRFDTL.nCRFHDRNo INNER JOIN CRFSUBDTL On CRFSUBDTL.nCRFDtlNo = CRFDTL.nCRFDtlNo   Where vWorkSpaceId = '" + Request.QueryString("WorkSpaceId") + "' AND INodeId = " + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + " AND vSubjectId = '" + Request.QueryString("SubjectId") + "' AND CRFHDR.cStatusIndi <> 'D'AND CRFDTL.cStatusIndi <> 'D'  AND vMedExCode = '" + sourcemedex + "'   Order by iTranNo desc"
                        ds_Dependancy = Me.objHelp.GetResultSet(wStr, "MedExDependency")
                    End If
                    If Not ds_Dependancy Is Nothing AndAlso ds_Dependancy.Tables.Count > 0 AndAlso ds_Dependancy.Tables(0).Rows.Count > 0 Then
                        Status = ds_Dependancy.Tables(0).Rows(0)("cDataStatus")
                        Targetvalue = ds_Dependancy.Tables(0).Rows(0)("vMedExResult")
                    Else
                        Status = "D"
                    End If



                    If DependecyMedExCodes.Contains("[" + Id.Substring(0, Id.LastIndexOf("R")) + "]") And Status = "D" Then
                        txt.Enabled = False
                        'txt.Attributes.Add("disabled", "disabled")
                    ElseIf Targetvalue.ToUpper() = Sourcevalue.ToUpper() Then
                        txt.Enabled = True
                    ElseIf Targetvalue.ToUpper() <> Sourcevalue.ToUpper() And Status <> "" Then
                        txt.Enabled = False
                    End If
                End If

                If TargetMedExCode <> "" Then
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "BindTrigger" + Id, "BindTrigger('" + txt.ClientID + "','blur');", True)
                End If


                GetObject = txt

            Case "DATETIME"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                ''Enhancement in EDC
                If Me.Session(S_EDCUser) = EDCUser Then
                    'txt.Enabled = False
                    'txt.ReadOnly = True
                    txt.Attributes.Add("ReadOnly", "true")
                End If
                ''''''''''''''''''

                If IsNotNull.ToUpper() = "Y" Then
                    'txt.CssClass = "textBox"
                    txt.CssClass = "Required textBox" ''Added by Dipen Shah
                End If

                txt.Text = IIf(dtValues = "", "", dtValues)

                If Not dtValues = "" Then
                    txt.Text = dtValues
                    'txt.Enabled = False
                    txt.Attributes.Add("disabled", "true")
                End If

                txt.Attributes.Add("OnChange", "DateValidationForCTM(this.value,this,'" & IsNotNull & "','btnUpdate" &
                                   MedExGroupCode & MedExSubGroupCode & Id & "','" & TargetMedExCode & "')")


                txt.Attributes.Add("title", txt.Text)
                txt.Attributes.Add("onfocus", "SetValue(this.id,this.value);")


                Dim wStr As String
                Dim ds_Dependancy As DataSet = Nothing
                Dim Status As String = ""
                Dim ds_MedExDependenctValue As DataSet = Nothing
                Dim wStrMedexValue As String = ""
                Dim sourcemedex As String = ""
                Dim Sourcevalue As String = ""
                Dim Targetvalue As String = ""

                If DependecyMedExCodes <> "" Then
                    Dim vMedEx As String = Convert.ToString(DependecyMedExCodes.Split(",")(0))

                    wStrMedexValue = "  Select * From MedexDependency Where vWorkspaceId = " + HFParentWorkspaceId.Value + " AND vTargetMedExCode in (" + Id.Substring(0, 5) + ") AND cStatusIndi <> 'D'"
                    ds_MedExDependenctValue = Me.objHelp.GetResultSet(wStrMedexValue, "MedExDependency")

                    If Not ds_MedExDependenctValue Is Nothing AndAlso ds_MedExDependenctValue.Tables.Count > 0 AndAlso ds_MedExDependenctValue.Tables(0).Rows.Count > 0 Then
                        sourcemedex = ds_MedExDependenctValue.Tables(0).Rows(0)("vSourceMedExCode")
                        Sourcevalue = ds_MedExDependenctValue.Tables(0).Rows(0)("vMedExValue")
                    End If
                    If sourcemedex <> "" Then
                        wStr = "Select CRFDTL.cDataStatus, CRFSUBDTL.vMedExResult  From CRFHDR INNER JOIN CRFDTL On CRFHDR.nCRFHDRNo = CRFDTL.nCRFHDRNo INNER JOIN CRFSUBDTL On CRFSUBDTL.nCRFDtlNo = CRFDTL.nCRFDtlNo   Where vWorkSpaceId = '" + Request.QueryString("WorkSpaceId") + "' AND INodeId = " + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + " AND vSubjectId = '" + Request.QueryString("SubjectId") + "' AND CRFHDR.cStatusIndi <> 'D'AND CRFDTL.cStatusIndi <> 'D'  AND vMedExCode = '" + sourcemedex + "'   Order by iTranNo desc"
                        ds_Dependancy = Me.objHelp.GetResultSet(wStr, "MedExDependency")
                    End If
                    If Not ds_Dependancy Is Nothing AndAlso ds_Dependancy.Tables.Count > 0 AndAlso ds_Dependancy.Tables(0).Rows.Count > 0 Then
                        Status = ds_Dependancy.Tables(0).Rows(0)("cDataStatus")
                        Targetvalue = ds_Dependancy.Tables(0).Rows(0)("vMedExResult")
                    Else
                        Status = "D"
                    End If

                End If

                If DependecyMedExCodes.Contains("[" + Id.Substring(0, Id.LastIndexOf("R")) + "]") And Status = "D" Then
                    txt.Enabled = False
                    'txt.Attributes.Add("disabled", "disabled")
                ElseIf Targetvalue.ToUpper() = Sourcevalue.ToUpper() Then
                    txt.Enabled = True
                ElseIf Targetvalue.ToUpper() <> Sourcevalue.ToUpper() And Status <> "" Then
                    txt.Enabled = False
                End If

                If TargetMedExCode <> "" Then
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "BindTrigger" + Id, "BindTrigger('" + txt.ClientID + "','change');", True)
                End If


                GetObject = txt

            Case "TIME"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                ''Enhancement in EDC
                If Me.Session(S_EDCUser) = EDCUser Then
                    'txt.Enabled = False
                    'txt.ReadOnly = True
                    txt.Attributes.Add("ReadOnly", "true")
                End If
                '''''''''''''''''''''''''''''
                If IsNotNull.ToUpper() = "Y" Then
                    txt.CssClass = "Required textBox"
                End If

                If Not dtValues = "" Then
                    txt.Text = dtValues
                    'txt.Enabled = False
                    txt.Attributes.Add("disabled", "true")
                End If
                txt.Attributes.Add("title", dtValues)

                txt.Attributes.Add("onblur", "AutoTimeConvert(this.value,this,'" & IsNotNull & "','btnUpdate" &
                                   MedExGroupCode & MedExSubGroupCode & Id & "','" & TargetMedExCode & "')")
                txt.Attributes.Add("onfocus", "SetValue(this.id,this.value);")

                Dim wStr As String
                Dim ds_Dependancy As DataSet = Nothing
                Dim Status As String = ""
                Dim ds_MedExDependenctValue As DataSet = Nothing
                Dim wStrMedexValue As String = ""
                Dim sourcemedex As String = ""
                Dim Sourcevalue As String = ""
                Dim Targetvalue As String = ""

                If DependecyMedExCodes <> "" Then
                    Dim vMedEx As String = Convert.ToString(DependecyMedExCodes.Split(",")(0))

                    wStrMedexValue = "  Select * From MedexDependency Where vWorkspaceId = " + HFParentWorkspaceId.Value + " AND vTargetMedExCode in (" + Id.Substring(0, 5) + ") AND cStatusIndi <> 'D'"
                    ds_MedExDependenctValue = Me.objHelp.GetResultSet(wStrMedexValue, "MedExDependency")

                    If Not ds_MedExDependenctValue Is Nothing AndAlso ds_MedExDependenctValue.Tables.Count > 0 AndAlso ds_MedExDependenctValue.Tables(0).Rows.Count > 0 Then
                        sourcemedex = ds_MedExDependenctValue.Tables(0).Rows(0)("vSourceMedExCode")
                        Sourcevalue = ds_MedExDependenctValue.Tables(0).Rows(0)("vMedExValue")
                    End If
                    If sourcemedex <> "" Then
                        wStr = "Select CRFDTL.cDataStatus, CRFSUBDTL.vMedExResult  From CRFHDR INNER JOIN CRFDTL On CRFHDR.nCRFHDRNo = CRFDTL.nCRFHDRNo INNER JOIN CRFSUBDTL On CRFSUBDTL.nCRFDtlNo = CRFDTL.nCRFDtlNo   Where vWorkSpaceId = '" + Request.QueryString("WorkSpaceId") + "' AND INodeId = " + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + " AND vSubjectId = '" + Request.QueryString("SubjectId") + "' AND CRFHDR.cStatusIndi <> 'D'AND CRFDTL.cStatusIndi <> 'D'  AND vMedExCode = '" + sourcemedex + "'   Order by iTranNo desc"
                        ds_Dependancy = Me.objHelp.GetResultSet(wStr, "MedExDependency")
                    End If
                    If Not ds_Dependancy Is Nothing AndAlso ds_Dependancy.Tables.Count > 0 AndAlso ds_Dependancy.Tables(0).Rows.Count > 0 Then
                        Status = ds_Dependancy.Tables(0).Rows(0)("cDataStatus")
                        Targetvalue = ds_Dependancy.Tables(0).Rows(0)("vMedExResult")
                    Else
                        Status = "D"
                    End If
                End If


                If DependecyMedExCodes.Contains("[" + Id.Substring(0, Id.LastIndexOf("R")) + "]") And Status = "D" Then
                    txt.Enabled = False
                    'txt.Attributes.Add("disabled", "disabled")
                ElseIf Targetvalue.ToUpper() = Sourcevalue.ToUpper() Then
                    txt.Enabled = True
                ElseIf Targetvalue.ToUpper() <> Sourcevalue.ToUpper() And Status <> "" Then
                    txt.Enabled = False
                End If

                If TargetMedExCode <> "" Then
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "BindTrigger" + Id, "BindTrigger('" + txt.ClientID + "','blur');", True)
                End If


                GetObject = txt

                ''''''''''''''''''''''''''''''''''
            Case "ASYNCDATETIME"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"

                If IsNotNull.ToUpper() = "Y" Then
                    txt.CssClass = "Required textBox"
                End If

                txt.Text = IIf(dtValues = "", "", dtValues)
                If Not dtValues = "" Then
                    txt.Text = dtValues
                    'txt.Enabled = False
                    txt.Attributes.Add("disabled", "true")
                End If

                txt.Attributes.Add("onblur", "DateValidationForCTM(this.value,this,'" & IsNotNull & "','btnUpdate" &
                                   MedExGroupCode & MedExSubGroupCode & Id & "','" & TargetMedExCode & "')")

                If Me.Session(S_ScopeNo) = Scope_BABE Then
                    If (Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add) Then
                        'txt.Text = IIf(dtValues = "", System.DateTime.Now.ToString("dd-MMM-yyyy"), dtValues)
                        'txt.Text = IIf(dtValues = "", Format(CDate(objCommon.GetCurDatetime(Me.Session(S_TimeZoneName))).Date, "dd-MMM-yyyy"), dtValues)

                        If dtValues = "" Then
                            ConvertDateTime = CDate(objCommon.GetCurDatetime(Me.Session(S_TimeZoneName)))
                            If (System.DateTime.Now - ConvertDateTime).TotalHours > 12 Then
                                txt.Text = ""
                                strTimeDifference = "There are some datetime differenece while getting datetime.Please verify the datetime."
                            Else
                                'As discussed with Ms. Bhumi to resolve the issue of canada - auto date captured - By Vimal
                                'txt.Text = Format(ConvertDateTime.Date, "dd-MMM-yyyy")
                                txt.Text = ""
                            End If
                        Else
                            txt.Text = dtValues
                        End If

                    End If
                    If dtValues = "" AndAlso Me.HFSubjectId.Value.Trim() = "0000" Then
                        txt.Text = ""
                    End If
                    txt.Attributes.Add("onblur", "DateValidation(this.value,this,'" & IsNotNull & "','btnUpdate" &
                                       MedExGroupCode & MedExSubGroupCode & Id & "','" & TargetMedExCode & "')")
                End If

                txt.Attributes.Add("title", txt.Text)
                txt.Attributes.Add("onfocus", "SetValue(this.id,this.value);")

                Dim wStr As String
                Dim ds_Dependancy As DataSet = Nothing
                Dim Status As String = ""
                Dim ds_MedExDependenctValue As DataSet = Nothing
                Dim wStrMedexValue As String = ""
                Dim sourcemedex As String = ""
                Dim Sourcevalue As String = ""
                Dim Targetvalue As String = ""

                If DependecyMedExCodes <> "" Then

                    Dim vMedEx As String = Convert.ToString(DependecyMedExCodes.Split(",")(0))

                    wStrMedexValue = "  Select * From MedexDependency Where vWorkspaceId = " + HFParentWorkspaceId.Value + " AND vTargetMedExCode in (" + Id.Substring(0, 5) + ") AND cStatusIndi <> 'D'"
                    ds_MedExDependenctValue = Me.objHelp.GetResultSet(wStrMedexValue, "MedExDependency")

                    If Not ds_MedExDependenctValue Is Nothing AndAlso ds_MedExDependenctValue.Tables.Count > 0 AndAlso ds_MedExDependenctValue.Tables(0).Rows.Count > 0 Then
                        sourcemedex = ds_MedExDependenctValue.Tables(0).Rows(0)("vSourceMedExCode")
                        Sourcevalue = ds_MedExDependenctValue.Tables(0).Rows(0)("vMedExValue")
                    End If
                    If sourcemedex <> "" Then
                        wStr = "Select CRFDTL.cDataStatus, CRFSUBDTL.vMedExResult  From CRFHDR INNER JOIN CRFDTL On CRFHDR.nCRFHDRNo = CRFDTL.nCRFHDRNo INNER JOIN CRFSUBDTL On CRFSUBDTL.nCRFDtlNo = CRFDTL.nCRFDtlNo   Where vWorkSpaceId = '" + Request.QueryString("WorkSpaceId") + "' AND INodeId = " + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + " AND vSubjectId = '" + Request.QueryString("SubjectId") + "' AND CRFHDR.cStatusIndi <> 'D'AND CRFDTL.cStatusIndi <> 'D'  AND vMedExCode = '" + sourcemedex + "'   Order by iTranNo desc"
                        ds_Dependancy = Me.objHelp.GetResultSet(wStr, "MedExDependency")
                    End If
                    If Not ds_Dependancy Is Nothing AndAlso ds_Dependancy.Tables.Count > 0 AndAlso ds_Dependancy.Tables(0).Rows.Count > 0 Then
                        Status = ds_Dependancy.Tables(0).Rows(0)("cDataStatus")
                        Targetvalue = ds_Dependancy.Tables(0).Rows(0)("vMedExResult")
                    Else
                        Status = "D"
                    End If

                End If
                If DependecyMedExCodes.Contains("[" + Id.Substring(0, Id.LastIndexOf("R")) + "]") And Status = "D" Then
                    txt.Enabled = False
                    'txt.Attributes.Add("disabled", "disabled")
                ElseIf Targetvalue.ToUpper() = Sourcevalue.ToUpper() Then
                    txt.Enabled = True
                ElseIf Targetvalue.ToUpper() <> Sourcevalue.ToUpper() And Status <> "" Then
                    txt.Enabled = False
                End If

                If TargetMedExCode <> "" Then
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "BindTrigger" + Id, "BindTrigger('" + txt.ClientID + "','blur');", True)
                End If


                GetObject = txt

            Case "ASYNCTIME"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"

                If IsNotNull.ToUpper() = "Y" Then
                    txt.CssClass = "Required textBox"
                End If

                txt.Text = dtValues
                If Not dtValues = "" Then
                    txt.Text = dtValues
                    'txt.Enabled = False
                    txt.Attributes.Add("disabled", "true")
                End If
                txt.Attributes.Add("title", dtValues)

                txt.Attributes.Add("onblur", "AutoTimeConvert(this.value,this,'" & IsNotNull & "','btnUpdate" & MedExGroupCode &
                                   MedExSubGroupCode & Id & "','" & TargetMedExCode & "')")
                txt.Attributes.Add("onfocus", "SetValue(this.id,this.value);")

                Dim wStr As String
                Dim ds_Dependancy As DataSet = Nothing
                Dim Status As String = ""
                Dim ds_MedExDependenctValue As DataSet = Nothing
                Dim wStrMedexValue As String = ""
                Dim sourcemedex As String = ""
                Dim Sourcevalue As String = ""
                Dim Targetvalue As String = ""

                If DependecyMedExCodes <> "" Then

                    Dim vMedEx As String = Convert.ToString(DependecyMedExCodes.Split(",")(0))

                    wStrMedexValue = "  Select * From MedexDependency Where vWorkspaceId = " + HFParentWorkspaceId.Value + " AND vTargetMedExCode in (" + Id.Substring(0, 5) + ") AND cStatusIndi <> 'D'"
                    ds_MedExDependenctValue = Me.objHelp.GetResultSet(wStrMedexValue, "MedExDependency")

                    If Not ds_MedExDependenctValue Is Nothing AndAlso ds_MedExDependenctValue.Tables.Count > 0 AndAlso ds_MedExDependenctValue.Tables(0).Rows.Count > 0 Then
                        sourcemedex = ds_MedExDependenctValue.Tables(0).Rows(0)("vSourceMedExCode")
                        Sourcevalue = ds_MedExDependenctValue.Tables(0).Rows(0)("vMedExValue")
                    End If
                    If sourcemedex <> "" Then
                        wStr = "Select CRFDTL.cDataStatus, CRFSUBDTL.vMedExResult  From CRFHDR INNER JOIN CRFDTL On CRFHDR.nCRFHDRNo = CRFDTL.nCRFHDRNo INNER JOIN CRFSUBDTL On CRFSUBDTL.nCRFDtlNo = CRFDTL.nCRFDtlNo   Where vWorkSpaceId = '" + Request.QueryString("WorkSpaceId") + "' AND INodeId = " + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + " AND vSubjectId = '" + Request.QueryString("SubjectId") + "' AND CRFHDR.cStatusIndi <> 'D'AND CRFDTL.cStatusIndi <> 'D'  AND vMedExCode = '" + sourcemedex + "'   Order by iTranNo desc"
                        ds_Dependancy = Me.objHelp.GetResultSet(wStr, "MedExDependency")
                    End If
                    If Not ds_Dependancy Is Nothing AndAlso ds_Dependancy.Tables.Count > 0 AndAlso ds_Dependancy.Tables(0).Rows.Count > 0 Then
                        Status = ds_Dependancy.Tables(0).Rows(0)("cDataStatus")
                        Targetvalue = ds_Dependancy.Tables(0).Rows(0)("vMedExResult")
                    Else
                        Status = "D"
                    End If

                End If

                If DependecyMedExCodes.Contains("[" + Id.Substring(0, Id.LastIndexOf("R")) + "]") And Status = "D" Then
                    txt.Enabled = False
                    'txt.Attributes.Add("disabled", "disabled")
                ElseIf Targetvalue.ToUpper() = Sourcevalue.ToUpper() Then
                    txt.Enabled = True
                ElseIf Targetvalue.ToUpper() <> Sourcevalue.ToUpper() And Status <> "" Then
                    txt.Enabled = False
                End If

                If TargetMedExCode <> "" Then
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "BindTrigger" + Id, "BindTrigger('" + txt.ClientID + "','blur');", True)
                End If


                GetObject = txt

                ''''''''''''''''''''''''''''''''''

            Case "IMPORT"
                'Hf = New HiddenField
                'Hf.ID = Id
                'Hf.Value = dtValues.Trim()

                'GetObject = Hf
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                If IsNotNull.ToUpper() = "Y" Then
                    txt.CssClass = "Required textBox"
                End If

                If Not dtValues = "" Then
                    txt.Text = dtValues
                    'txt.Enabled = False
                    txt.Attributes.Add("disabled", "true")
                End If
                txt.Attributes.Add("title", dtValues)

                If DependecyMedExCodes.Contains("[" + Id.Substring(0, Id.LastIndexOf("R")) + "]") Then
                    txt.Enabled = False
                    'txt.Attributes.Add("disabled", "disabled")
                End If


                GetObject = txt

            Case "COMBOGLOBALDICTIONARY"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                If IsNotNull.ToUpper() = "Y" Then
                    txt.CssClass = "Required textBox"
                End If

                If Not dtValues = "" Then
                    txt.Text = dtValues
                    txt.Attributes.Add("title", dtValues)
                    'txt.Enabled = False
                    txt.Attributes.Add("disabled", "true")
                End If

                Is_ComboGlobalDictionary = True

                HighRange = IIf(HighRange = "", "0", HighRange)
                LowRange = IIf(LowRange = "", "0", LowRange)

                Dim checktype As String = ""
                Dim msg As String = ""

                If Validation = "" Or Validation = "NA" Then
                    checktype = "0"
                    msg = "No Validation"
                ElseIf Validation = GeneralModule.Val_AN Then
                    checktype = GeneralModule.Validation_Alphanumeric.ToString()
                    msg = "Please Enter AlphaNumeric Value"
                ElseIf Validation = GeneralModule.Val_NU Then
                    checktype = GeneralModule.Validation_Numeric.ToString()
                    msg = "Please Enter Numeric or Blank Value"
                ElseIf Validation = GeneralModule.Val_IN Then
                    checktype = GeneralModule.Validation_Integer.ToString()
                    msg = "Please Enter Integer or Blank Value"
                ElseIf Validation = GeneralModule.Val_AL Then
                    checktype = GeneralModule.Validation_Alphabet.ToString()
                    msg = "Please Enter Alphabets only"
                ElseIf Validation = GeneralModule.Val_NNI Then
                    checktype = GeneralModule.Validation_NotNullInteger.ToString()
                    msg = "Please Enter Integer Value"
                ElseIf Validation = GeneralModule.Val_NNU Then
                    checktype = GeneralModule.Validation_NotNullNumeric.ToString()
                    msg = "Please Enter Numeric Value"
                End If

                'If AlertonValue.Trim() <> "" OrElse length.Trim() <> "" Then
                '    checktype = "0"
                'End If

                txt.Attributes.Add("onblur", "ValidateTextbox('" & checktype & "',this,'" & msg & "'," &
                                                              HighRange & "," & LowRange & ",'" & AlertonValue &
                                                              "','" & AlertMsg.Replace("'", "\'") & "','" & length &
                                                              "','" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode &
                                                              Id & "','','','" & TargetMedExCode & "');")

                txt.Attributes.Add("onfocus", "SetValue(this.id,this.value);")

                'If DependecyMedExCodes.Contains("[" + Id.Substring(0, Id.LastIndexOf("R")) + "]") Then
                '    txt.Enabled = False
                '    'txt.Attributes.Add("disabled", "disabled")
                'End If

                If TargetMedExCode <> "" Then
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "BindTrigger" + Id, "BindTrigger('" + txt.ClientID + "','blur');", True)
                End If


                GetObject = txt

            Case "FORMULA"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                If IsNotNull.ToUpper() = "Y" Then
                    txt.CssClass = "Required textBox"
                End If

                If Not dtValues = "" Then
                    txt.Text = dtValues
                    txt.Attributes.Add("title", dtValues)
                    'txt.Enabled = False
                    txt.Attributes.Add("disabled", "true")
                End If

                HighRange = IIf(HighRange = "", "0", HighRange)
                LowRange = IIf(LowRange = "", "0", LowRange)

                Dim checktype As String = ""
                Dim msg As String = ""

                If Validation = "" Or Validation = "NA" Then
                    checktype = "0"
                    msg = "No Validation"
                ElseIf Validation = GeneralModule.Val_AN Then
                    checktype = GeneralModule.Validation_Alphanumeric.ToString()
                    msg = "Please Enter AlphaNumeric Value"
                ElseIf Validation = GeneralModule.Val_NU Then
                    checktype = GeneralModule.Validation_Numeric.ToString()
                    msg = "Please Enter Numeric or Blank Value"
                ElseIf Validation = GeneralModule.Val_IN Then
                    checktype = GeneralModule.Validation_Integer.ToString()
                    msg = "Please Enter Integer or Blank Value"
                ElseIf Validation = GeneralModule.Val_AL Then
                    checktype = GeneralModule.Validation_Alphabet.ToString()
                    msg = "Please Enter Alphabets only"
                ElseIf Validation = GeneralModule.Val_NNI Then
                    checktype = GeneralModule.Validation_NotNullInteger.ToString()
                    msg = "Please Enter Integer Value"
                ElseIf Validation = GeneralModule.Val_NNU Then
                    checktype = GeneralModule.Validation_NotNullNumeric.ToString()
                    msg = "Please Enter Numeric Value"
                End If

                'If AlertonValue.Trim() <> "" OrElse length.Trim() <> "" Then
                '    checktype = "0"
                'End If

                'txt.Attributes.Add("onblur", "ValidateTextbox('" & checktype & "',this,'" & msg & "'," & _
                '                           HighRange & "," & LowRange & ",'" & AlertonValue & "','" & AlertMsg.Replace("'", "\'") & "','" & length & "','" & IsNotNull & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "');")


                txt.Attributes.Add("onblur", "ValidateTextbox('" & checktype & "',this,'" & msg & "'," &
                                                              HighRange & "," & LowRange & ",'" & AlertonValue & "','" &
                                                              AlertMsg.Replace("'", "\'") & "','" & length & "','" & IsNotNull &
                                                              "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "','" &
                                                              HFNumericScale.Value & "' ,'" & Validation & "','" &
                                                              TargetMedExCode & "');")

                If Id = GeneralModule.Medex_Oral_Body_Temperature_F Then

                    txt.Attributes.Add("onblur", "F2C('" & GeneralModule.Medex_Oral_Body_Temperature_F.Trim() & "','" & GeneralModule.Medex_Oral_Body_Temperature_C.Trim() & "','btnUpdate" & MedExGroupCode & MedExSubGroupCode & Id & "','" &
                                                      TargetMedExCode & "');")

                End If
                txt.Attributes.Add("onfocus", "SetValue(this.id,this.value);")

                Is_FormulaEnabled = True

                Dim wStr As String
                Dim ds_Dependancy As DataSet = Nothing
                Dim Status As String = ""
                Dim ds_MedExDependenctValue As DataSet = Nothing
                Dim wStrMedexValue As String = ""
                Dim sourcemedex As String = ""
                Dim Sourcevalue As String = ""
                Dim Targetvalue As String = ""

                If DependecyMedExCodes <> "" Then
                    Dim vMedEx As String = Convert.ToString(DependecyMedExCodes.Split(",")(0))

                    wStrMedexValue = "  Select * From MedexDependency Where vWorkspaceId = " + HFParentWorkspaceId.Value + " AND vTargetMedExCode in (" + Id.Substring(0, 5) + ") AND cStatusIndi <> 'D'"
                    ds_MedExDependenctValue = Me.objHelp.GetResultSet(wStrMedexValue, "MedExDependency")

                    If Not ds_MedExDependenctValue Is Nothing AndAlso ds_MedExDependenctValue.Tables.Count > 0 AndAlso ds_MedExDependenctValue.Tables(0).Rows.Count > 0 Then
                        sourcemedex = ds_MedExDependenctValue.Tables(0).Rows(0)("vSourceMedExCode")
                        Sourcevalue = ds_MedExDependenctValue.Tables(0).Rows(0)("vMedExValue")
                    End If
                    If sourcemedex <> "" Then
                        wStr = "Select CRFDTL.cDataStatus, CRFSUBDTL.vMedExResult  From CRFHDR INNER JOIN CRFDTL On CRFHDR.nCRFHDRNo = CRFDTL.nCRFHDRNo INNER JOIN CRFSUBDTL On CRFSUBDTL.nCRFDtlNo = CRFDTL.nCRFDtlNo   Where vWorkSpaceId = '" + Request.QueryString("WorkSpaceId") + "' AND INodeId = " + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + " AND vSubjectId = '" + Request.QueryString("SubjectId") + "' AND CRFHDR.cStatusIndi <> 'D'AND CRFDTL.cStatusIndi <> 'D'  AND vMedExCode = '" + sourcemedex + "'   Order by iTranNo desc"
                        ds_Dependancy = Me.objHelp.GetResultSet(wStr, "MedExDependency")
                    End If
                    If Not ds_Dependancy Is Nothing AndAlso ds_Dependancy.Tables.Count > 0 AndAlso ds_Dependancy.Tables(0).Rows.Count > 0 Then
                        Status = ds_Dependancy.Tables(0).Rows(0)("cDataStatus")
                        Targetvalue = ds_Dependancy.Tables(0).Rows(0)("vMedExResult")
                    Else
                        Status = "D"
                    End If
                End If
                If DependecyMedExCodes.Contains("[" + Id.Substring(0, Id.LastIndexOf("R")) + "]") And Status = "D" Then
                    txt.Enabled = False
                    'txt.Attributes.Add("disabled", "disabled")
                ElseIf Targetvalue.ToUpper() = Sourcevalue.ToUpper() Then
                    txt.Enabled = True
                ElseIf Targetvalue.ToUpper() <> Sourcevalue.ToUpper() And Status <> "" Then
                    txt.Enabled = False

                End If

                If TargetMedExCode <> "" Then
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "BindTrigger" + Id, "BindTrigger('" + txt.ClientID + "','blur');", True)
                End If


                GetObject = txt

            Case "LABEL"
                lbl = New Label
                lbl.ID = "lbl" + Id
                lbl.EnableViewState = False
                lbl.CssClass = "Label"
                'lbl.Width = "500"
                lbl.Style.Add("word-wrap", "break-word")
                lbl.Style.Add("white-space", "none")
                lbl.Text = vMedExValues.Trim()
                lbl.ToolTip = dtValues.Trim()


                Dim wStr As String
                Dim ds_Dependancy As DataSet = Nothing
                Dim Status As String = ""
                Dim ds_MedExDependenctValue As DataSet = Nothing
                Dim wStrMedexValue As String = ""
                Dim sourcemedex As String = ""
                Dim Sourcevalue As String = ""
                Dim Targetvalue As String = ""

                If DependecyMedExCodes <> "" Then
                    Dim vMedEx As String = Convert.ToString(DependecyMedExCodes.Split(",")(0))

                    wStrMedexValue = "  Select * From MedexDependency Where vWorkspaceId = " + HFParentWorkspaceId.Value + " AND vTargetMedExCode in (" + Id.Substring(0, 5) + ") AND cStatusIndi <> 'D'"
                    ds_MedExDependenctValue = Me.objHelp.GetResultSet(wStrMedexValue, "MedExDependency")

                    If Not ds_MedExDependenctValue Is Nothing AndAlso ds_MedExDependenctValue.Tables.Count > 0 AndAlso ds_MedExDependenctValue.Tables(0).Rows.Count > 0 Then
                        sourcemedex = ds_MedExDependenctValue.Tables(0).Rows(0)("vSourceMedExCode")
                        Sourcevalue = ds_MedExDependenctValue.Tables(0).Rows(0)("vMedExValue")
                    End If
                    If sourcemedex <> "" Then
                        wStr = "Select CRFDTL.cDataStatus, CRFSUBDTL.vMedExResult  From CRFHDR INNER JOIN CRFDTL On CRFHDR.nCRFHDRNo = CRFDTL.nCRFHDRNo INNER JOIN CRFSUBDTL On CRFSUBDTL.nCRFDtlNo = CRFDTL.nCRFDtlNo   Where vWorkSpaceId = '" + Request.QueryString("WorkSpaceId") + "' AND INodeId = " + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + " AND vSubjectId = '" + Request.QueryString("SubjectId") + "' AND CRFHDR.cStatusIndi <> 'D'AND CRFDTL.cStatusIndi <> 'D'  AND vMedExCode = '" + sourcemedex + "'   Order by iTranNo desc"
                        ds_Dependancy = Me.objHelp.GetResultSet(wStr, "MedExDependency")
                    End If
                    If Not ds_Dependancy Is Nothing AndAlso ds_Dependancy.Tables.Count > 0 AndAlso ds_Dependancy.Tables(0).Rows.Count > 0 Then
                        Status = ds_Dependancy.Tables(0).Rows(0)("cDataStatus")
                        Targetvalue = ds_Dependancy.Tables(0).Rows(0)("vMedExResult")
                    Else
                        Status = "D"
                    End If
                End If

                If DependecyMedExCodes.Contains("[" + Id.Substring(0, Id.LastIndexOf("R")) + "]") And Status = "D" Then
                    lbl.Enabled = False
                ElseIf Targetvalue.ToUpper() = Sourcevalue.ToUpper() Then
                    lbl.Enabled = True
                    'lbl.Attributes.Add("disabled", "disabled")
                ElseIf Targetvalue.ToUpper() <> Sourcevalue.ToUpper() And Status <> "" Then
                    lbl.Enabled = False
                End If

                GetObject = lbl

            Case "CRFTERM"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                If Not dtValues = "" Then
                    txt.Text = dtValues
                    'txt.Enabled = False
                    txt.Attributes.Add("disabled", "true")
                End If


                Dim wStr As String
                Dim ds_Dependancy As DataSet = Nothing
                Dim Status As String = ""
                Dim ds_MedExDependenctValue As DataSet = Nothing
                Dim wStrMedexValue As String = ""
                Dim sourcemedex As String = ""
                Dim Sourcevalue As String = ""
                Dim Targetvalue As String = ""


                Dim vMedEx As String = Convert.ToString(DependecyMedExCodes.Split(",")(0))

                wStrMedexValue = "  Select * From MedexDependency Where vWorkspaceId = " + HFParentWorkspaceId.Value + " AND vTargetMedExCode in (" + Id.Substring(0, 5) + ") AND cStatusIndi <> 'D'"
                ds_MedExDependenctValue = Me.objHelp.GetResultSet(wStrMedexValue, "MedExDependency")

                If Not ds_MedExDependenctValue Is Nothing AndAlso ds_MedExDependenctValue.Tables.Count > 0 AndAlso ds_MedExDependenctValue.Tables(0).Rows.Count > 0 Then
                    sourcemedex = ds_MedExDependenctValue.Tables(0).Rows(0)("vSourceMedExCode")
                    Sourcevalue = ds_MedExDependenctValue.Tables(0).Rows(0)("vMedExValue")
                End If
                If sourcemedex <> "" Then


                    wStr = "Select CRFDTL.cDataStatus, CRFSUBDTL.vMedExResult  From CRFHDR INNER JOIN CRFDTL On CRFHDR.nCRFHDRNo = CRFDTL.nCRFHDRNo INNER JOIN CRFSUBDTL On CRFSUBDTL.nCRFDtlNo = CRFDTL.nCRFDtlNo   Where vWorkSpaceId = '" + Request.QueryString("WorkSpaceId") + "' AND INodeId = " + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + " AND vSubjectId = '" + Request.QueryString("SubjectId") + "' AND CRFHDR.cStatusIndi <> 'D'AND CRFDTL.cStatusIndi <> 'D'  AND vMedExCode = '" + sourcemedex + "'   Order by iTranNo desc"
                    ds_Dependancy = Me.objHelp.GetResultSet(wStr, "MedExDependency")
                End If
                If Not ds_Dependancy Is Nothing AndAlso ds_Dependancy.Tables.Count > 0 AndAlso ds_Dependancy.Tables(0).Rows.Count > 0 Then
                    Status = ds_Dependancy.Tables(0).Rows(0)("cDataStatus")
                    Targetvalue = ds_Dependancy.Tables(0).Rows(0)("vMedExResult")
                Else
                    Status = "D"
                End If



                If DependecyMedExCodes.Contains("[" + Id.Substring(0, Id.LastIndexOf("R")) + "]") And Status = "D" Then
                    txt.Enabled = False
                    'txt.Attributes.Add("disabled", "disabled")
                ElseIf Targetvalue.ToUpper() = Sourcevalue.ToUpper() Then
                    txt.Enabled = True
                ElseIf Targetvalue.ToUpper() <> Sourcevalue.ToUpper() And Status <> "" Then
                    txt.Enabled = False
                End If

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

                If IsNotNull.ToUpper() = "Y" Then
                    ddlDate.CssClass = "Required dropDownList"
                    ddlMonth.CssClass = "Required dropDownList"
                    ddlYear.CssClass = "Required dropDownList"
                End If

                If Not objHelp.GetDatesMonthsAndYears("PROC_GetDatesMonthsAndYears", dsDate, estr) Then
                    Throw New Exception("Error While Getting Dates,Months and Years.")
                End If


                ddlDate.DataSource = dsDate.Tables(0)
                ddlDate.DataTextField = "Dates"
                ddlDate.DataValueField = "Dates"
                ddlDate.DataBind()
                ddlDate.Items.Insert(0, New ListItem("DD", ""))
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
                ddlYear.Items.Insert(0, New ListItem("YYYY", ""))
                ddlYear.Items.Insert(1, New ListItem("UKUK", "0000"))

                If Not dtValues = "" Then
                    ddlDate.Items.FindByText(Split(dtValues.ToString, "-")(0).ToString.ToUpper).Selected = True
                    ddlMonth.Items.FindByText(Split(dtValues.ToString, "-")(1).ToString.ToUpper).Selected = True
                    ddlYear.Items.FindByText(Split(dtValues.ToString, "-")(2).ToString.ToUpper).Selected = True

                    'ddlDate.Enabled = False
                    'ddlMonth.Enabled = False
                    'ddlYear.Enabled = False
                    ddlDate.Attributes.Add("disabled", "true")
                    ddlMonth.Attributes.Add("disabled", "true")
                    ddlYear.Attributes.Add("disabled", "true")

                End If

                ddlDate.Attributes.Add("onchange", "CheckStandardDateAttr(this,'" & ddlDate.ClientID & "','" & ddlMonth.ClientID & "','" & ddlYear.ClientID & "','" & TargetMedExCode & "');")
                ddlMonth.Attributes.Add("onchange", "CheckStandardDateAttr(this,'" & ddlDate.ClientID & "','" & ddlMonth.ClientID & "','" & ddlYear.ClientID & "','" & TargetMedExCode & "');")
                ddlYear.Attributes.Add("onchange", "CheckStandardDateAttr(this,'" & ddlDate.ClientID & "','" & ddlMonth.ClientID & "','" & ddlYear.ClientID & "','" & TargetMedExCode & "');")

                PlaceMedEx.Controls.Add(ddlDate)
                PlaceMedEx.Controls.Add(ddlMonth)

                ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                If (Me.ddlRepeatNo.SelectedIndex = 0 AndAlso
                          (Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() <> WorkFlowStageId_DataEntry AndAlso
                          Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() <> WorkFlowStageId_FirstReview)) Or Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

                    ddlDate.Attributes.Add("disabled", "true")
                    ddlMonth.Attributes.Add("disabled", "true")
                End If
                ''Commented by nipun khant for dynamic review
                'If (Me.ddlRepeatNo.SelectedIndex = 0 AndAlso _
                '         (Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_DataEntry AndAlso _
                '          Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_FirstReview)) Or Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

                '    ddlDate.Attributes.Add("disabled", "true")
                '    ddlMonth.Attributes.Add("disabled", "true")
                'End If

                If Me.ddlRepeatNo.SelectedIndex <> 0 AndAlso
                          (CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_Review Or
                              CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_ReviewCompleted Or
                              CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_Locked) Then
                    ddlDate.Attributes.Add("disabled", "true")
                    ddlMonth.Attributes.Add("disabled", "true")

                ElseIf Me.ddlRepeatNo.SelectedIndex <> 0 AndAlso
                          (CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_DataEntry) Then

                    If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

                        ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                        If Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() <> WorkFlowStageId_OnlyView Then
                            ''Commented by nipun khant for dynamic review
                            'If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_OnlyView Then

                            If Not Me.HFCRFDtlLockStatus.Value Is Nothing AndAlso
                                   Not Convert.ToString(Me.HFCRFDtlLockStatus.Value).Trim.ToUpper() = CRFHdr_Locked Then

                                If dtValues <> "" Or TranNo > 1 Then
                                    ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                                    If Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() = WorkFlowStageId_DataEntry Then
                                        ddlDate.Attributes.Add("disabled", "true")
                                        ddlMonth.Attributes.Add("disabled", "true")
                                    End If

                                    ''Commented by nipun khant for dynamic review
                                    'If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_DataEntry Then
                                    '    ddlDate.Attributes.Add("disabled", "true")
                                    '    ddlMonth.Attributes.Add("disabled", "true")
                                    'End If

                                End If
                            End If
                        End If
                    End If
                End If

                If DependecyMedExCodes.Contains("[" + ddlDate.ID.Substring(0, ddlDate.ID.LastIndexOf("R")) + "]") Then
                    ddlDate.Enabled = False
                    ddlMonth.Enabled = False
                    ddlYear.Enabled = False
                End If

                'If TargetMedExCode <> "" Then
                '    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "BindTrigger" + Id, "BindTrigger('" + ddlDate.ClientID + "','change');", True)
                '    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "BindTrigger" + Id, "BindTrigger('" + ddlMonth.ClientID + "','change');", True)
                '    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "BindTrigger" + Id, "BindTrigger('" + ddlYear.ClientID + "','change');", True)
                'End If

                GetObject = ddlYear

            Case "STANDARDDATETIME"
                Dim ddlDate As New DropDownList
                Dim ddlMonth As New DropDownList
                Dim ddlYear As New DropDownList
                Dim str As String = String.Empty
                Dim estr As String = String.Empty
                Dim dsDate As New DataSet
                Dim ddlHH As New DropDownList
                Dim ddlMM As New DropDownList

                ddlDate.ID = Id + "_1"
                ddlDate.CssClass = "dropDownList"
                ddlDate.Width = 80
                ddlMonth.ID = Id + "_2"
                ddlMonth.CssClass = "dropDownList"
                ddlMonth.Width = 80
                ddlYear.ID = Id + "_3"
                ddlYear.CssClass = "dropDownList"
                ddlYear.Width = 80

                ddlHH.ID = Id + "_4"
                ddlHH.CssClass = "dropDownList"
                ddlHH.Width = 80

                ddlMM.ID = Id + "_5"
                ddlMM.CssClass = "dropDownList"
                ddlMM.Width = 80

                ddlYear.Attributes.Add("StandardDate", "Y")
                ddlMonth.Attributes.Add("StandardDate", "Y")
                ddlDate.Attributes.Add("StandardDate", "Y")
                ddlHH.Attributes.Add("StandardDate", "Y")
                ddlMM.Attributes.Add("StandardDate", "Y")

                ddlYear.Attributes.Add("StandardDateTime", "Y")
                ddlMonth.Attributes.Add("StandardDateTime", "Y")
                ddlDate.Attributes.Add("StandardDateTime", "Y")
                ddlHH.Attributes.Add("StandardDateTime", "Y")
                ddlMM.Attributes.Add("StandardDateTime", "Y")

                If IsNotNull.ToUpper() = "Y" Then
                    ddlDate.CssClass = "Required dropDownList"
                    ddlMonth.CssClass = "Required dropDownList"
                    ddlYear.CssClass = "Required dropDownList"
                    ddlHH.CssClass = "Required dropDownList"
                    ddlMM.CssClass = "Required dropDownList"
                End If


                If Not objHelp.GetDatesMonthsAndYears("PROC_GetDatesMonthsYearsHouresAndTime", dsDate, estr) Then
                    Throw New Exception("Error While Getting Dates,Months and Years.")
                End If


                ddlDate.DataSource = dsDate.Tables(0)
                ddlDate.DataTextField = "Dates"
                ddlDate.DataValueField = "Dates"
                ddlDate.DataBind()
                ddlDate.Items.Insert(0, New ListItem("DD", "DD"))
                ddlDate.Items.Insert(1, New ListItem("UK", "00"))

                ddlMonth.DataSource = dsDate.Tables(1)
                ddlMonth.DataTextField = "Months"
                ddlMonth.DataValueField = "Val"
                ddlMonth.DataBind()
                ddlMonth.Items.Insert(0, New ListItem("MMM", "MMM"))
                ddlMonth.Items.Insert(1, New ListItem("UNK", "00"))

                ddlYear.DataSource = dsDate.Tables(2)
                ddlYear.DataTextField = "Years"
                ddlYear.DataValueField = "Years"
                ddlYear.DataBind()
                ddlYear.Items.Insert(0, New ListItem("YYYY", "YYYY"))
                ddlYear.Items.Insert(1, New ListItem("UKUK", "0000"))

                ddlHH.DataSource = dsDate.Tables(3)
                ddlHH.DataTextField = "HH"
                ddlHH.DataValueField = "HH"
                ddlHH.DataBind()
                ddlHH.Items.Insert(0, New ListItem("HH", "HH"))

                ddlMM.DataSource = dsDate.Tables(4)
                ddlMM.DataTextField = "MM"
                ddlMM.DataValueField = "MM"
                ddlMM.DataBind()
                ddlMM.Items.Insert(0, New ListItem("MM", "MM"))


                If Not dtValues = "" Then
                    ddlDate.Items.FindByText(Split(dtValues.ToString, "-")(0).ToString.ToUpper).Selected = True
                    ddlMonth.Items.FindByText(Split(dtValues.ToString, "-")(1).ToString.ToUpper).Selected = True
                    ddlYear.Items.FindByText(Split(dtValues.ToString, "-")(2).ToString.ToUpper).Selected = True

                    'ddlDate.Enabled = False
                    'ddlMonth.Enabled = False
                    'ddlYear.Enabled = False

                    ddlDate.Attributes.Add("disabled", "true")
                    ddlMonth.Attributes.Add("disabled", "true")
                    ddlYear.Attributes.Add("disabled", "true")

                    If Split(dtValues.ToString, "-").Length > 4 AndAlso Split(dtValues.ToString, "-")(3).ToString.ToUpper = "" Then
                    Else
                        If Split(dtValues.ToString, "-").Length > 4 Then
                            ddlHH.Items.FindByText(Split(dtValues.ToString, "-")(3).ToString.ToUpper).Selected = True
                            ddlMM.Items.FindByText(Split(dtValues.ToString, "-")(4).ToString.ToUpper).Selected = True

                            'ddlHH.Enabled = False
                            'ddlMM.Enabled = False

                            ddlHH.Attributes.Add("disabled", "true")
                            ddlMM.Attributes.Add("disabled", "true")

                        End If
                    End If
                End If

                ddlDate.Attributes.Add("onchange", "CheckStandardDateAttr(this,'" & ddlDate.ClientID & "','" & ddlMonth.ClientID & "','" & ddlYear.ClientID & "','" & TargetMedExCode & "');")
                ddlMonth.Attributes.Add("onchange", "CheckStandardDateAttr(this,'" & ddlDate.ClientID & "','" & ddlMonth.ClientID & "','" & ddlYear.ClientID & "','" & TargetMedExCode & "');")
                ddlYear.Attributes.Add("onchange", "CheckStandardDateAttr(this,'" & ddlDate.ClientID & "','" & ddlMonth.ClientID & "','" & ddlYear.ClientID & "','" & TargetMedExCode & "');")
                ddlHH.Attributes.Add("onchange", "CheckStandardDateAttr(this,'" & ddlDate.ClientID & "','" & ddlMonth.ClientID & "','" & ddlYear.ClientID & "','" & TargetMedExCode & "');")
                ddlMM.Attributes.Add("onchange", "CheckStandardDateAttr(this,'" & ddlDate.ClientID & "','" & ddlMonth.ClientID & "','" & ddlYear.ClientID & "','" & TargetMedExCode & "');")


                PlaceMedEx.Controls.Add(ddlDate)
                PlaceMedEx.Controls.Add(ddlMonth)
                PlaceMedEx.Controls.Add(ddlYear)
                PlaceMedEx.Controls.Add(ddlHH)

                ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                If (Me.ddlRepeatNo.SelectedIndex = 0 AndAlso
                          (Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() <> WorkFlowStageId_DataEntry AndAlso
                          Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() <> WorkFlowStageId_FirstReview)) Or Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

                    ddlDate.Attributes.Add("disabled", "true")
                    ddlMonth.Attributes.Add("disabled", "true")
                    ddlYear.Attributes.Add("disabled", "true")
                    ddlHH.Attributes.Add("disabled", "true")
                End If
                ''Commented by nipun khant for dynamic review
                'If (Me.ddlRepeatNo.SelectedIndex = 0 AndAlso _
                '         (Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_DataEntry AndAlso _
                '          Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_FirstReview)) Or Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

                '    ddlDate.Attributes.Add("disabled", "true")
                '    ddlMonth.Attributes.Add("disabled", "true")
                'End If

                If Me.ddlRepeatNo.SelectedIndex <> 0 AndAlso
                          (CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_Review Or
                              CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_ReviewCompleted Or
                              CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_Locked) Then
                    ddlDate.Attributes.Add("disabled", "true")
                    ddlMonth.Attributes.Add("disabled", "true")
                    ddlYear.Attributes.Add("disabled", "true")
                    ddlHH.Attributes.Add("disabled", "true")

                ElseIf Me.ddlRepeatNo.SelectedIndex <> 0 AndAlso
                          (CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_DataEntry) Then

                    If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

                        ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                        If Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() <> WorkFlowStageId_OnlyView Then
                            ''Commented by nipun khant for dynamic review
                            'If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_OnlyView Then

                            If Not Me.HFCRFDtlLockStatus.Value Is Nothing AndAlso
                                   Not Convert.ToString(Me.HFCRFDtlLockStatus.Value).Trim.ToUpper() = CRFHdr_Locked Then

                                If dtValues <> "" Or TranNo > 1 Then
                                    ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                                    If Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() = WorkFlowStageId_DataEntry Then
                                        ddlDate.Attributes.Add("disabled", "true")
                                        ddlMonth.Attributes.Add("disabled", "true")
                                        ddlYear.Attributes.Add("disabled", "true")
                                        ddlHH.Attributes.Add("disabled", "true")
                                    End If

                                    ''Commented by nipun khant for dynamic review
                                    'If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_DataEntry Then
                                    '    ddlDate.Attributes.Add("disabled", "true")
                                    '    ddlMonth.Attributes.Add("disabled", "true")
                                    'End If

                                End If
                            End If
                        End If
                    End If
                End If

                Dim wStr As String
                Dim ds_Dependancy As DataSet = Nothing
                Dim Status As String = ""
                Dim ds_MedExDependenctValue As DataSet = Nothing
                Dim wStrMedexValue As String = ""
                Dim sourcemedex As String = ""
                Dim Sourcevalue As String = ""
                Dim Targetvalue As String = ""

                If DependecyMedExCodes <> "" Then


                    Dim vMedEx As String = Convert.ToString(DependecyMedExCodes.Split(",")(0))

                    wStrMedexValue = "  Select * From MedexDependency Where vWorkspaceId = " + HFParentWorkspaceId.Value + " AND vTargetMedExCode in (" + Id.Substring(0, 5).Replace("[", "").Replace("]", "") + ") AND cStatusIndi <> 'D'"
                    ds_MedExDependenctValue = Me.objHelp.GetResultSet(wStrMedexValue, "MedExDependency")

                    If Not ds_MedExDependenctValue Is Nothing AndAlso ds_MedExDependenctValue.Tables.Count > 0 AndAlso ds_MedExDependenctValue.Tables(0).Rows.Count > 0 Then
                        sourcemedex = ds_MedExDependenctValue.Tables(0).Rows(0)("vSourceMedExCode")
                        Sourcevalue = ds_MedExDependenctValue.Tables(0).Rows(0)("vMedExValue")
                    End If

                    If Sourcevalue <> "" Then
                        wStr = "Select CRFDTL.cDataStatus, CRFSUBDTL.vMedExResult  From CRFHDR INNER JOIN CRFDTL On CRFHDR.nCRFHDRNo = CRFDTL.nCRFHDRNo INNER JOIN CRFSUBDTL On CRFSUBDTL.nCRFDtlNo = CRFDTL.nCRFDtlNo   Where vWorkSpaceId = '" + Request.QueryString("WorkSpaceId") + "' AND INodeId = " + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + " AND vSubjectId = '" + Request.QueryString("SubjectId") + "' AND CRFHDR.cStatusIndi <> 'D'AND CRFDTL.cStatusIndi <> 'D'  AND vMedExCode = '" + sourcemedex + "'   Order by iTranNo desc"
                        ds_Dependancy = Me.objHelp.GetResultSet(wStr, "MedExDependency")
                    End If
                    If Not ds_Dependancy Is Nothing AndAlso ds_Dependancy.Tables.Count > 0 AndAlso ds_Dependancy.Tables(0).Rows.Count > 0 Then
                        Status = ds_Dependancy.Tables(0).Rows(0)("cDataStatus")
                        Targetvalue = ds_Dependancy.Tables(0).Rows(0)("vMedExResult")
                    Else
                        Status = "D"
                    End If

                End If

                If DependecyMedExCodes.Contains("[" + Id.Substring(0, Id.LastIndexOf("R")) + "]") And Status = "D" Then
                    ddlDate.Enabled = False
                    ddlMonth.Enabled = False
                    ddlYear.Enabled = False
                    ddlHH.Enabled = False
                    ddlMM.Enabled = False
                ElseIf Targetvalue.ToUpper() = Sourcevalue.ToUpper() Then
                    ddlDate.Enabled = True
                    ddlMonth.Enabled = True
                    ddlYear.Enabled = True
                    ddlHH.Enabled = True
                    ddlMM.Enabled = True
                ElseIf Targetvalue.ToUpper() <> Sourcevalue.ToUpper() And Status <> "" Then
                    ddlDate.Enabled = False
                    ddlMonth.Enabled = False
                    ddlYear.Enabled = False
                    ddlHH.Enabled = False
                    ddlMM.Enabled = False
                End If

                'If TargetMedExCode <> "" Then
                '    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "BindTrigger" + Id, "BindTrigger('" + ddlDate.ClientID + "','change');", True)
                '    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "BindTrigger" + Id, "BindTrigger('" + ddlMonth.ClientID + "','change');", True)
                '    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "BindTrigger" + Id, "BindTrigger('" + ddlYear.ClientID + "','change');", True)
                'End If

                If Me.Session(S_EDCUser) = EDCUser Then
                    ddlDate.Enabled = False
                    ddlMonth.Enabled = False
                    ddlYear.Enabled = False
                    ddlHH.Enabled = False
                    ddlMM.Enabled = False
                End If

                GetObject = ddlMM

            Case Else
                Return Nothing
        End Select
    End Function

#End Region

#Region "GetDateImage"
    'Added By Debashis Sahoo for calendar image
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

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        objCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub

#End Region

#Region "AssignValues"

    Private Function AssignValues(ByVal SubjectId As String, ByVal WorkspaceId As String,
                                ByVal ActivityId As String, ByVal NodeId As String,
                                ByVal PeriodId As String,
                                ByVal MySubjectNo As String, ByRef DsSave As DataSet) As Boolean
        Dim DsCRFHdr As New DataSet
        Dim DsCRFDtl As New DataSet
        Dim DtCRFHdr As New DataTable
        Dim DtCRFDtl As New DataTable
        Dim DtCRFSubDtl As New DataTable

        Dim objCollection As ControlCollection
        Dim objControl As Control
        Dim ObjId As String = String.Empty
        Dim dr As DataRow
        Dim TranNo As Integer = 0
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim dsNodeInfo As New DataSet
        Dim NodeIndex As String = String.Empty
        Dim ds_DCF As New DataSet
        Dim dt_DCF As New DataTable
        Dim flg As Boolean = False
        Dim ControlDesc As String = String.Empty
        Dim ControlId As String = String.Empty
        Try

            Wstr = "vWorkSpaceId='" & Me.HFParentWorkspaceId.Value.Trim() & "' and iNodeId='" & NodeId & "'" &
                    " and vActivityId='" & ActivityId & "'"

            If Not Me.objHelp.getWorkSpaceNodeDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                            dsNodeInfo, estr) Then
                Me.objCommon.ShowAlert("Error while getting NodeIndex", Me)
                Return False
            End If
            NodeIndex = dsNodeInfo.Tables(0).Rows(0)("iNodeIndex").ToString()

            objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection) 'Me.PlaceMedEx.Controls 

            DtCRFHdr = CType(Me.ViewState(VS_DtCRFHdr), DataTable)
            DtCRFDtl = CType(Me.ViewState(VS_DtCRFDtl), DataTable)
            DtCRFSubDtl = CType(Me.ViewState(VS_DtCRFSubDtl), DataTable)

            '*********Checking MedEx values on 25-Dec-2009******************
            If Not Me.objHelp.GetDCFMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty,
                                    ds_DCF, estr) Then
                Return False
            End If
            Me.ViewState(VS_DtDCF) = ds_DCF.Tables(0).Copy()
            '*************************************

            If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                If Not Me.objHelp.GetCRFHdr("nCRFHdrNo=" & CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows(0)("nCRFHdrNo"),
                                                    WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                            DsCRFHdr, estr) Then
                    Return False
                End If
                DtCRFHdr = DsCRFHdr.Tables(0)
                For Each dr In DtCRFHdr.Rows
                    dr("dModifyOn") = DateTime.Now()
                Next
                DtCRFHdr.AcceptChanges()
            Else

                DtCRFHdr.Clear()
                dr = DtCRFHdr.NewRow
                'nCRFHdrNo, vWorkSpaceId,dStartDate,iPeriod,iNodeId,iNodeIndex,vActivityId,cLockStatus
                dr("nCRFHdrNo") = 1
                If DtCRFHdr.Rows.Count > 0 Then
                    dr("nCRFHdrNo") = DtCRFHdr.Rows(0)("nCRFHdrNo")
                End If

                dr("vWorkSpaceId") = WorkspaceId
                'dr("dStartDate") = System.DateTime.Now
                dr("dStartDate") = CDate(objCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                dr("iPeriod") = PeriodId
                dr("iNodeId") = NodeId
                dr("iNodeIndex") = NodeIndex
                dr("vActivityId") = ActivityId
                dr("cLockStatus") = "U" 'cLockStatus
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dr("dModifyOn") = DateTime.Now()
                'dr.AcceptChanges()
                DtCRFHdr.Rows.Add(dr)
                DtCRFHdr.TableName = "CRFHdr"
                DtCRFHdr.AcceptChanges()
            End If

            If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                If Not Me.objHelp.GetCRFDtl("nCRFDtlNo=" & CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows(0)("nCRFDtlNo"),
                                                    WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                            DsCRFDtl, estr) Then
                    Return False
                End If
                DtCRFDtl = DsCRFDtl.Tables(0)
                For Each dr In DtCRFDtl.Rows
                    If rblApprovalStatus.SelectedValue.ToString() = "A" OrElse rblApprovalStatus.SelectedValue.ToString() = "" Then
                        dr("cDataStatus") = CRF_DataEntryCompleted
                    Else
                        dr("cDataStatus") = "R"
                    End If
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr("vRemarks") = txtQCRemarks.Value.Trim()
                    'dr("cDataStatus") = CRF_DataEntryCompleted
                    dr("dModifyOn") = DateTime.Now()
                Next

                DtCRFDtl.AcceptChanges()

                If CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_DataEntry Then
                    For Each dr In DtCRFDtl.Rows
                        If rblApprovalStatus.SelectedValue.ToString() = "A" OrElse rblApprovalStatus.SelectedValue.ToString() = "" Then
                            dr("cDataStatus") = CRF_DataEntry
                        Else
                            dr("cDataStatus") = "R"
                        End If
                        'dr("cDataStatus") = CRF_DataEntry
                        dr("dModifyOn") = DateTime.Now()
                    Next
                    DtCRFDtl.AcceptChanges()
                End If

            Else

                DtCRFDtl.Clear()
                dr = DtCRFDtl.NewRow
                'nCRFDtlNo,nCRFHdrNo,iRepeatNo,dRepeatationDate,vSubjectId,iMySubjectNo,cLockStatus,iWorkFlowstageId
                dr("nCRFDtlNo") = 1
                If DtCRFDtl.Rows.Count > 0 Then
                    dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
                End If
                dr("nCRFHdrNo") = DtCRFHdr.Rows(0)("nCRFHdrNo").ToString.Trim()
                dr("iRepeatNo") = 1 'iRepeatNo
                'dr("dRepeatationDate") = System.DateTime.Now
                dr("dRepeatationDate") = CDate(objCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                dr("vSubjectId") = SubjectId
                dr("iMySubjectNo") = MySubjectNo
                dr("cLockStatus") = "U" 'cLockStatus
                dr("iWorkFlowstageId") = Me.Session(S_WorkFlowStageId)
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dr("dModifyOn") = DateTime.Now()
                If rblApprovalStatus.SelectedValue.ToString() = "A" OrElse rblApprovalStatus.SelectedValue.ToString() = "" Then
                    dr("cDataStatus") = CRF_DataEntryCompleted
                Else
                    dr("cDataStatus") = "R"
                End If
                'dr("cDataStatus") = CRF_DataEntryCompleted
                If CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_DataEntry Then
                    dr("cDataStatus") = CRF_DataEntry
                End If
                If txtQCRemarks.Value <> "" OrElse txtQCRemarks.Value Is Nothing Then
                    dr("vRemarks") = txtQCRemarks.Value
                End If

                'dr.AcceptChanges()
                DtCRFDtl.Rows.Add(dr)
                DtCRFDtl.TableName = "CRFDtl"
                DtCRFDtl.AcceptChanges()
            End If

            DtCRFSubDtl.Clear()
            'For Detail Table

            For Each objControl In objCollection
                'nCRFSubDtlNo,nCRFDtlNo,iTranNo,vMedExCode,dMedExDatetime,vMedExResult,vModificationRemark

                If Not objControl.ID Is Nothing Then 'Commented for Tabulor Format'AndAlso Convert.ToString(objControl.ID).Trim().Contains("R1000")

                    If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.Label" AndAlso
                               objControl.ID.ToString.Trim().Contains("Lnk") Then
                        Dim objLbl As Label = CType(objControl, Label)
                        ControlId = objControl.ID.ToString.Replace("Lnk", "")
                        ControlDesc = objLbl.Text
                    End If

                    If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.TextBox" Then
                        TranNo += 1
                        If objControl.ID.ToString.Contains("txt") Then
                            ObjId = objControl.ID.ToString.Replace("txt", "")
                        Else
                            ObjId = objControl.ID.ToString.Trim()
                        End If

                        dr = DtCRFSubDtl.NewRow
                        dr("nCRFSubDtlNo") = TranNo
                        dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
                        dr("iTranNo") = TranNo
                        dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
                        'If ControlId.ToUpper() = ObjId.ToUpper() AndAlso ControlDesc.Trim() <> "" Then
                        dr("vMedExDesc") = ControlDesc.Trim.Substring(0, ControlDesc.Trim.Length - 1)
                        'End If
                        'SDNidhi
                        'dr("dMedExDateTime") = System.DateTime.Now
                        dr("dMedExDateTime") = CDate(objCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                        dr("vMedexResult") = Request.Form(objControl.ID) 'CType(objControl, TextBox).Text CType(Me.FindControl(obj.GetId), TextBox).Text
                        'dr("vMedexResult") = CType(objControl, TextBox).Text
                        dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
                        dr("iModifyBy") = Me.Session(S_UserID)
                        dr("cStatusIndi") = "N"
                        dr("dModifyOn") = DateTime.Now()
                        If Me.CheckDiscrepancy(objControl, ObjId, Request.Form(objControl.ID), "S",
                                               DtCRFDtl.Rows(0)("nCRFDtlNo")) Then
                            dr("cStatusIndi") = "A"
                        End If
                        DtCRFSubDtl.Rows.Add(dr)
                        DtCRFSubDtl.AcceptChanges()

                    ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" Then

                        ObjId = objControl.ID.ToString.Trim()
                        DtCRFSubDtl.DefaultView.RowFilter = "vMedExCode = '" + ObjId.Substring(0, ObjId.IndexOf("R")).Trim() + "'"
                        If DtCRFSubDtl.DefaultView.ToTable.Rows.Count = 0 Then
                            TranNo += 1

                            dr = DtCRFSubDtl.NewRow

                            dr("nCRFSubDtlNo") = TranNo
                            dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
                            dr("iTranNo") = TranNo
                            dr("vMedExDesc") = ControlDesc.Trim.Substring(0, ControlDesc.Trim.Length - 1)
                            dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
                            dr("iModifyBy") = Me.Session(S_UserID)
                            dr("cStatusIndi") = "N"
                            dr("dMedExDateTime") = CDate(objCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                            dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
                            dr("dModifyOn") = DateTime.Now()
                            If Me.CheckDiscrepancy(objControl, ObjId, Request.Form(objControl.ID), "S",
                                           DtCRFDtl.Rows(0)("nCRFDtlNo")) Then
                                dr("cStatusIndi") = "A"
                            End If
                            If DirectCast(objControl, System.Web.UI.WebControls.DropDownList).Attributes("StandardDate") = "Y" Then
                                flg = False
                                For Each objControl1 In objCollection
                                    If Not objControl1.ID Is Nothing AndAlso objControl1.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" AndAlso objControl1.ID.ToString.Trim().Substring(0, objControl1.ID.ToString.Trim().IndexOf("R")) = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() Then
                                        If Request.Form(objControl1.ID) Is Nothing Then
                                            dr("vMedexResult") = Request.Form(objControl1.ID)
                                        ElseIf Request.Form(objControl1.ID) = "" Then
                                            flg = True
                                        Else
                                            dr("vMedexResult") += Request.Form(objControl1.ID)
                                        End If
                                    End If
                                Next
                                dr("vMedexResult") = IIf(flg = True, "", dr("vMedexResult"))
                            Else

                                dr("vMedexResult") = Request.Form(objControl.ID)
                                'dr("vMedexResult") = CType(objControl, DropDownList).Text

                            End If

                            DtCRFSubDtl.Rows.Add(dr)
                            DtCRFSubDtl.AcceptChanges()
                        End If


                    ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.FileUpload" Then
                        Dim filename As String = ""

                        TranNo += 1
                        If objControl.ID.ToString.Contains("FU") Then
                            ObjId = objControl.ID.ToString.Replace("FU", "")
                        Else
                            ObjId = objControl.ID.ToString.Trim()
                        End If


                        If CType(FindControl(objControl.ID), FileUpload).FileName = "" And
                            Not IsNothing(CType(FindControl("hlnk" + ObjId.Substring(0, ObjId.IndexOf("R")).Trim()), HyperLink)) AndAlso
                            CType(FindControl("hlnk" + ObjId.Substring(0, ObjId.IndexOf("R")).Trim()), HyperLink).NavigateUrl <> "" Then

                            filename = CType(FindControl("hlnk" + ObjId.Substring(0, ObjId.IndexOf("R")).Trim()), HyperLink).NavigateUrl

                        ElseIf Request.Files(objControl.ID).FileName <> "" Then

                            filename = "~/" + System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail") +
                                        WorkspaceId + "/" + NodeId + "/" + SubjectId + "/" + Path.GetFileName(Request.Files(objControl.ID).FileName.ToString.Trim())
                        End If

                        dr = DtCRFSubDtl.NewRow
                        dr("nCRFSubDtlNo") = TranNo
                        dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
                        dr("iTranNo") = TranNo
                        dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
                        dr("vMedExDesc") = ControlDesc.Trim.Substring(0, ControlDesc.Trim.Length - 1)
                        'SDNidhi
                        'dr("dMedExDateTime") = System.DateTime.Now
                        dr("dMedExDateTime") = CDate(objCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                        dr("vMedexResult") = filename
                        dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
                        dr("iModifyBy") = Me.Session(S_UserID)
                        dr("cStatusIndi") = "N"
                        dr("dModifyOn") = DateTime.Now()
                        DtCRFSubDtl.Rows.Add(dr)
                        DtCRFSubDtl.AcceptChanges()

                    ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.RadioButtonList" Then
                        TranNo += 1
                        ObjId = objControl.ID.ToString.Trim()
                        dr = DtCRFSubDtl.NewRow
                        dr("nCRFSubDtlNo") = TranNo
                        dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
                        dr("iTranNo") = TranNo
                        dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
                        dr("vMedExDesc") = ControlDesc.Trim.Substring(0, ControlDesc.Trim.Length - 1)
                        'SDNidhi
                        'dr("dMedExDateTime") = System.DateTime.Now
                        dr("dMedExDateTime") = CDate(objCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                        dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
                        dr("iModifyBy") = Me.Session(S_UserID)
                        dr("cStatusIndi") = "N"
                        dr("dModifyOn") = DateTime.Now()
                        'Dim rbl As RadioButtonList = CType(FindControl(objControl.ID), RadioButtonList)
                        Dim rbl As RadioButtonList = CType(objControl, RadioButtonList)
                        Dim StrChk As String = ""

                        For index As Integer = 0 To rbl.Items.Count - 1
                            StrChk += IIf(rbl.Items(index).Selected, rbl.Items(index).Value.ToString.Trim() + ",", "")
                        Next index

                        If StrChk.Trim() <> "" Then
                            StrChk = StrChk.Substring(0, StrChk.LastIndexOf(","))
                        End If
                        dr("vMedexResult") = StrChk
                        If Me.CheckDiscrepancy(objControl, ObjId, StrChk, "S",
                                               DtCRFDtl.Rows(0)("nCRFDtlNo")) Then
                            dr("cStatusIndi") = "A"
                        End If
                        DtCRFSubDtl.Rows.Add(dr)
                        DtCRFSubDtl.AcceptChanges()

                    ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.CheckBoxList" Then
                        TranNo += 1
                        ObjId = objControl.ID.ToString.Trim()
                        dr = DtCRFSubDtl.NewRow
                        dr("nCRFSubDtlNo") = TranNo
                        dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
                        dr("iTranNo") = TranNo
                        dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
                        dr("vMedExDesc") = ControlDesc.Trim.Substring(0, ControlDesc.Trim.Length - 1)
                        'SDNidhi
                        'dr("dMedExDateTime") = System.DateTime.Now
                        dr("dMedExDateTime") = CDate(objCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                        dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
                        dr("iModifyBy") = Me.Session(S_UserID)
                        dr("cStatusIndi") = "N"
                        dr("dModifyOn") = DateTime.Now()
                        Dim chkl As CheckBoxList = CType(objControl, CheckBoxList)
                        Dim StrChk As String = ""

                        For index As Integer = 0 To chkl.Items.Count - 1
                            StrChk += IIf(chkl.Items(index).Selected, chkl.Items(index).Value.ToString.Trim() + ",", "")
                        Next index

                        If StrChk.Trim() <> "" Then
                            StrChk = StrChk.Substring(0, StrChk.LastIndexOf(","))
                        End If
                        dr("vMedexResult") = StrChk
                        If Me.CheckDiscrepancy(objControl, ObjId, StrChk, "S",
                                               DtCRFDtl.Rows(0)("nCRFDtlNo")) Then
                            dr("cStatusIndi") = "A"
                        End If
                        DtCRFSubDtl.Rows.Add(dr)
                        DtCRFSubDtl.AcceptChanges()

                    ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.HiddenField" Then
                        TranNo += 1
                        ObjId = objControl.ID.ToString.Trim()
                        dr = DtCRFSubDtl.NewRow

                        '******Adding Header & footer to the document**********************

                        Dim ds_WorkSpaceNodeHistory As New DataSet
                        Dim filename As String = String.Empty
                        Dim versionNo As String = String.Empty
                        Wstr = "vWorkSpaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And iNodeId=" + NodeId.Trim() + " And iStageId =" & GeneralModule.Stage_Authorized

                        If Not objHelp.GetViewWorkSpaceNodeHistory(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                                                ds_WorkSpaceNodeHistory, estr) Then
                            Me.ShowErrorMessage("Error While Getting Data From View_WorkSpaceNodeHistory", estr)
                        End If

                        If Not ds_WorkSpaceNodeHistory.Tables(0).Rows.Count < 1 Then

                            filename = ds_WorkSpaceNodeHistory.Tables(0).Rows(0).Item("vDocPath").ToString()
                            versionNo = ds_WorkSpaceNodeHistory.Tables(0).Rows(0).Item("vUSerVersion").ToString()
                        End If

                        '*****************************************
                        ObjId = ObjId.Substring(0, ObjId.IndexOf("R")).Trim()
                        If ObjId = GeneralModule.Medex_FilePath.Trim() Then
                            dr("vMedexResult") = filename  'File Name from WorkspaceNodeHistory
                        ElseIf ObjId = GeneralModule.Medex_DownloadedBy.Trim() Then
                            dr("vMedexResult") = Me.Session(S_UserID)
                        ElseIf ObjId = GeneralModule.Medex_VersionNo.Trim() Then
                            dr("vMedexResult") = versionNo 'Version No from WorkspaceNodeHistory
                        ElseIf ObjId = GeneralModule.Medex_ReleaseDate.Trim() Then
                            'dr("vMedexResult") = Date.Now.Date.ToString("dd-MMM-yyyy") 'Version No from WorkspaceNodeHistory
                            dr("vMedexResult") = CDate(objCommon.GetCurDatetime(Me.Session(S_TimeZoneName))).Date.ToString("dd-MMM-yyyy")
                        Else
                            dr("vMedexResult") = Request.Form(objControl.ID) 'CType(objControl, TextBox).Text 'CType(Me.FindControl(obj.GetId), TextBox).Text
                        End If

                        dr("nCRFSubDtlNo") = TranNo
                        dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
                        dr("iTranNo") = TranNo
                        dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
                        dr("vMedExDesc") = ControlDesc.Trim.Substring(0, ControlDesc.Trim.Length - 1)
                        'SDNidhi
                        'dr("dMedExDateTime") = System.DateTime.Now
                        dr("dMedExDateTime") = CDate(objCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                        dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
                        dr("iModifyBy") = Me.Session(S_UserID)
                        dr("cStatusIndi") = "N"
                        dr("dModifyOn") = DateTime.Now()
                        DtCRFSubDtl.Rows.Add(dr)
                        DtCRFSubDtl.AcceptChanges()

                    ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.Label" AndAlso
                               objControl.ID.ToString.Trim().Contains("lbl") Then

                        Dim objLbl As Label = CType(objControl, Label)

                        If objLbl.CssClass.Contains("notsaved") Then
                            Continue For
                        End If

                        ControlId = objControl.ID.ToString.Replace("Lnk", "")
                        ControlDesc = objLbl.Text

                        TranNo += 1
                        ObjId = objControl.ID.ToString.Replace("lbl", "")
                        dr = DtCRFSubDtl.NewRow
                        dr("nCRFSubDtlNo") = TranNo
                        dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
                        dr("iTranNo") = TranNo
                        dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
                        dr("vMedExDesc") = ""
                        'SDNidhi
                        'dr("dMedExDateTime") = System.DateTime.Now
                        dr("dMedExDateTime") = CDate(objCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                        dr("vMedexResult") = ControlDesc.Trim()
                        dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
                        dr("iModifyBy") = Me.Session(S_UserID)
                        dr("cStatusIndi") = "N"
                        dr("dModifyOn") = DateTime.Now()
                        DtCRFSubDtl.Rows.Add(dr)
                        DtCRFSubDtl.AcceptChanges()

                    End If

                End If

            Next objControl
            '****************************************

            DtCRFSubDtl.TableName = "CRFSubDtl"
            DtCRFSubDtl.AcceptChanges()

            DsSave = Nothing
            DsSave = New DataSet
            DsSave.Tables.Add(DtCRFHdr.Copy())
            DsSave.Tables.Add(DtCRFDtl.Copy())
            DsSave.Tables.Add(DtCRFSubDtl.Copy())
            DsSave.AcceptChanges()

            dt_DCF = CType(Me.ViewState(VS_DtDCF), DataTable).Copy()
            dt_DCF.TableName = "DCFMst"
            dt_DCF.AcceptChanges()
            DsSave.Tables.Add(dt_DCF.Copy())
            DsSave.AcceptChanges()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..AssignValues")
            Return False
        End Try

    End Function

#End Region

#Region "Edit Checks"

    Private Sub EditChecks()
        Dim ds_MedExEditChecks As New DataSet
        Dim ds_EditChecksHdr As New DataSet
        Dim ds_EditChecksDtl As New DataSet
        Dim ds_CRFDetail As New DataSet
        Dim ds_ParentWorkspace As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ParentWorkspaceId As String = String.Empty

        Try
            wStr = "vWorkspaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And cStatusIndi <> 'D'"
            If Not Me.objHelp.getworkspacemst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                    ds_ParentWorkspace, eStr) Then
                Throw New Exception(eStr)
            End If
            If ds_ParentWorkspace Is Nothing Then
                Throw New Exception("Error While Getting Edit Parent Project Detail")
            End If
            If ds_ParentWorkspace.Tables(0).Rows.Count > 0 Then
                If Convert.ToString(ds_ParentWorkspace.Tables(0).Rows(0)("vParentWorkspaceId")).Trim() <> "" Then
                    ParentWorkspaceId = Convert.ToString(ds_ParentWorkspace.Tables(0).Rows(0)("vParentWorkspaceId")).Trim()
                End If
            End If

            wStr = "vWorkspaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And cStatusIndi <> 'D'"
            If ParentWorkspaceId.Trim() <> "" Then
                wStr = "vWorkspaceId = '" + ParentWorkspaceId + "' And cStatusIndi <> 'D'"
            End If


            ''Commented as the query was creating problem when opend from report and 
            ''               dataentry is changed(AS parentid is the nodeid stored from query string.)-Pratiksha
            'If Me.Session(S_ScopeNo) = GeneralModule.Scope_ClinicalTrial Then
            '    wStr += " And iParentNodeId = " + Me.HFParentNodeId.Value.Trim() + " "
            'End If
            wStr += " And iSourceNodeId_If = " + Me.HFNodeId.Value.Trim() + " "
            wStr += " And (iTargetNodeId_If = 0 Or iTargetNodeId_If = " + Me.HFNodeId.Value.Trim() + ")"
            wStr += " And (iSourceNodeId_Then = 0 Or iSourceNodeId_Then = " + Me.HFNodeId.Value.Trim() + ")"
            wStr += " And (iTargetNodeId_Then = 0 Or iTargetNodeId_Then = " + Me.HFNodeId.Value.Trim() + ")"

            If Not Me.objHelp.GetMedExEditChecks(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition _
                                    , ds_MedExEditChecks, eStr) Then
                Throw New Exception(eStr)
            End If
            If ds_MedExEditChecks Is Nothing Then
                Throw New Exception("Error While Getting Edit Checks Detail")
            End If

            If ds_MedExEditChecks.Tables(0).Rows.Count < 1 Then
                Exit Sub
            End If

            wStr = "vWorkspaceId = '" & Me.HFWorkspaceId.Value.Trim() & "' And cStatusIndi <> 'D'"
            wStr += " And iNodeId = " & Me.HFNodeId.Value.Trim() & " And vSubjectId = '" & Me.HFSubjectId.Value.Trim() & "'"
            wStr += " OPTION (MAXDOP 1)"
            ''''''''''''''''''''''''''''''''''''''''''''''''''''

            If Not Me.objHelp.View_CRFHdrDtlSubDtl_Edit(wStr, "*,DENSE_RANK() OVER(PARTITION BY View_CRFHdrDtlSubDtl_Edit.nCRFHdrNo,View_CRFHdrDtlSubDtl_Edit.vActivityId,View_CRFHdrDtlSubDtl_Edit.vSubjectId ORDER BY View_CRFHdrDtlSubDtl_Edit.nCRFHdrNo,View_CRFHdrDtlSubDtl_Edit.vActivityId,View_CRFHdrDtlSubDtl_Edit.vSubjectId,View_CRFHdrDtlSubDtl_Edit.iRepeatNo) as [RepetitionNo] ", ds_CRFDetail, eStr) Then
                Throw New Exception(eStr)
            End If
            If ds_CRFDetail Is Nothing Then
                Throw New Exception("Error While Getting CRF Detail")
            End If

            If Not Me.objHelp.GetEditChecksHdr("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty,
                                                         ds_EditChecksHdr, eStr) Then
                Throw New Exception(eStr)
            End If
            If ds_EditChecksHdr Is Nothing Then
                Throw New Exception(eStr)
            End If

            If Not Me.objHelp.GetEditChecksDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty,
                                                         ds_EditChecksDtl, eStr) Then
                Throw New Exception(eStr)
            End If
            If ds_EditChecksDtl Is Nothing Then
                Throw New Exception(eStr)
            End If

            Me.FireEditChecks(ds_MedExEditChecks, ds_CRFDetail, ds_EditChecksHdr, ds_EditChecksDtl)

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Executing Edit Checks", ex.Message)
        End Try
    End Sub

    Private Sub FireEditChecks(ByVal ds_MedExEditChecks As DataSet, ByVal ds_CRFDetail As DataSet,
                                ByVal ds_EditChecksHdr As DataSet, ByVal ds_EditChecksDtl As DataSet)
        Dim dt_EditChecksHdr As New DataTable
        Dim dr_EditChecksHdr As DataRow
        Dim dt_EditChecksDtl As New DataTable
        Dim dr_EditChecksDtl As DataRow

        Dim dv_EditChecksHdr As New DataView
        Dim dv_EditChecksDtl As New DataView

        Dim ds_Save As New DataSet
        Dim ds_EditChecksHdrDtl As New DataSet

        Dim dt_MedExEditChecks As New DataTable
        Dim dt_CRFDetail As New DataTable
        Dim dv_CRFDetail As New DataView

        Dim Source_MedExValue_If As String = String.Empty
        Dim Source_MedExType_If As String = String.Empty
        Dim Target_MedExValue_If As String = String.Empty
        Dim Target_MedExType_If As String = String.Empty

        Dim Source_MedExValue_Then As String = String.Empty
        Dim Source_MedExType_Then As String = String.Empty
        Dim Target_MedExValue_Then As String = String.Empty
        Dim Target_MedExType_Then As String = String.Empty

        Dim filter As String = String.Empty
        Dim Op_If As String = String.Empty
        Dim Op_Then As String = String.Empty

        Dim counter As Integer = 0
        Dim Is_Query As Boolean = False

        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim Subject_Count As Integer = 0

        Dim dr_Detail() As DataRow
        Dim dr_DetailThen() As DataRow

        Try
            dt_EditChecksHdr = ds_EditChecksHdr.Tables(0).Copy()
            dt_EditChecksDtl = ds_EditChecksDtl.Tables(0).Copy()
            dt_MedExEditChecks = ds_MedExEditChecks.Tables(0).Copy()
            dt_CRFDetail = ds_CRFDetail.Tables(0).Copy()

            dv_CRFDetail = dt_CRFDetail.DefaultView.ToTable(True, "vSubjectId".Split(",")).DefaultView

            For Each drSubject As DataRow In dv_CRFDetail.ToTable().Rows
                Subject_Count = Subject_Count + 1
                dr_EditChecksHdr = dt_EditChecksHdr.NewRow()
                dr_EditChecksHdr("nEditChecksHdrNo") = Subject_Count
                dr_EditChecksHdr("vWorkspaceId") = Me.HFWorkspaceId.Value.Trim()
                dr_EditChecksHdr("iPeriod") = 1
                dr_EditChecksHdr("iNodeId") = Me.HFNodeId.Value.Trim()
                dr_EditChecksHdr("vActivityId") = Me.HFActivityId.Value.Trim()
                dr_EditChecksHdr("vSubjectId") = drSubject("vSubjectId")
                'dr_EditChecksHdr("dFiredDate") =
                dr_EditChecksHdr("iTranNo") = 1
                dr_EditChecksHdr("iModifyBy") = Me.Session(S_UserID)
                dt_EditChecksHdr.Rows.Add(dr_EditChecksHdr)
                dt_EditChecksHdr.AcceptChanges()

                For Each dr As DataRow In dt_MedExEditChecks.Rows

                    counter += 1

                    filter = "iNodeId = " + Convert.ToString(dr("iSourceNodeId_If")) + " And vMedExCode = '"
                    filter += Convert.ToString(dr("vSourceMedExCode_If")) + "'"
                    filter += " And vSubjectId = '" + drSubject("vSubjectId") + "'"
                    dr_Detail = dt_CRFDetail.Select(filter)

                    For index As Integer = 0 To dr_Detail.Length - 1

                        Dim count As Integer = dr_Detail.Length

                        Source_MedExValue_If = String.Empty
                        Source_MedExType_If = String.Empty
                        Target_MedExValue_If = String.Empty

                        '**************If Condition
                        Source_MedExValue_If = Convert.ToString(dr_Detail(index)("vDefaultValue")).Trim()
                        Source_MedExType_If = Convert.ToString(dr_Detail(index)("vMedExType")).Trim()
                        Target_MedExValue_If = Convert.ToString(dr("vTargetValue_If")).Trim()
                        If Convert.ToString(dr("vTargetMedExCode_If")).Trim() <> "" Then
                            Target_MedExValue_If = String.Empty
                            Target_MedExType_If = String.Empty

                            Dim dr_TargetCRFDetail() As DataRow
                            filter = "iNodeId = " + Convert.ToString(dr("iTargetNodeId_If")) + " And vMedExCode = '"
                            filter += dr("vTargetMedExCode_If") + "'"
                            filter += " And vSubjectId = '" + drSubject("vSubjectId") + "'"
                            dr_TargetCRFDetail = dt_CRFDetail.Select(filter)


                            If dr_TargetCRFDetail.Length > 0 Then
                                Target_MedExValue_If = Convert.ToString(dr_TargetCRFDetail(index)("vDefaultValue"))
                                Target_MedExType_If = Convert.ToString(dr_TargetCRFDetail(index)("vMedexType"))
                            End If

                        End If
                        Op_If = Convert.ToString(dr("vOperator_If")).Trim()
                        '***********************

                        '*************Then Condition
                        Source_MedExValue_Then = ""
                        Source_MedExType_Then = ""
                        Target_MedExValue_Then = ""
                        Op_Then = ""

                        If Convert.ToString(dr("iSourceNodeId_Then")).Trim() <> "" AndAlso
                                                        Convert.ToString(dr("iSourceNodeId_Then")).Trim() <> "0" Then

                            filter = "iNodeId = " + Convert.ToString(dr("iSourceNodeId_Then")) + " And vMedExCode = '"
                            filter += Convert.ToString(dr("vSourceMedExCode_Then")) + "'"
                            filter += " And vSubjectId = '" + drSubject("vSubjectId") + "'"
                            dr_DetailThen = dt_CRFDetail.Select(filter)

                            If dr_DetailThen.Length > 0 Then

                                Source_MedExValue_Then = Convert.ToString(dr_DetailThen(index)("vDefaultValue")).Trim()
                                Source_MedExType_Then = Convert.ToString(dr_DetailThen(index)("vMedExType")).Trim()
                                Target_MedExValue_Then = Convert.ToString(dr("vTargetValue_Then")).Trim()

                                If Convert.ToString(dr("vTargetMedExCode_Then")).Trim() <> "" Then
                                    Dim dr_TargetCRFDetail() As DataRow
                                    filter = "iNodeId = " + Convert.ToString(dr("iTargetNodeId_Then")) + " And vMedExCode = '"
                                    filter += dr("vTargetMedExCode_Then") + "'"
                                    filter += " And vSubjectId = '" + drSubject("vSubjectId") + "'"
                                    filter += " And nCRFDtlNo = " + Convert.ToString(dr_Detail(index)("nCRFDtlNo"))
                                    filter += " And iRepeatNo = " + Convert.ToString(dr_Detail(index)("iRepeatNo"))
                                    dr_TargetCRFDetail = dt_CRFDetail.Select(filter)

                                    If dr_TargetCRFDetail.Length > 0 Then
                                        Dim dr_TargetDetail() As DataRow
                                        filter = "iNodeId = " + Convert.ToString(dr("iTargetNodeId_Then")) + " And vMedExCode = '"
                                        filter += dr("vTargetMedExCode_Then") + "' And nCRFDtlNo = " + Convert.ToString(dr_TargetCRFDetail(index)("nCRFDtlNo"))
                                        filter += " And iRepeatNo = " + Convert.ToString(dr_TargetCRFDetail(index)("iRepeatNo"))
                                        filter += " And vSubjectId = '" + drSubject("vSubjectId") + "'"
                                        dr_TargetDetail = dt_CRFDetail.Select(filter)
                                        If dr_TargetDetail.Length > 0 Then
                                            Target_MedExValue_Then = Convert.ToString(dr_TargetDetail(index)("vDefaultValue"))
                                            Target_MedExType_Then = Convert.ToString(dr_TargetDetail(index)("vMedexType"))
                                        End If
                                    End If
                                End If
                                Op_Then = Convert.ToString(dr("vOperator_Then")).Trim()
                            End If

                        End If
                        '***********************

                        dr_EditChecksDtl = dt_EditChecksDtl.NewRow()
                        dr_EditChecksDtl("bIsQuery") = 0
                        Is_Query = False

                        Me.GetResult(Source_MedExValue_If, Source_MedExType_If,
                                     Target_MedExValue_If, Target_MedExType_If,
                                     Op_If.ToUpper(),
                                     Source_MedExValue_Then, Source_MedExType_Then,
                                     Target_MedExValue_Then, Target_MedExType_Then,
                                     Op_Then.ToUpper(), Is_Query)

                        If Is_Query Then
                            dr_EditChecksDtl("bIsQuery") = 1
                        End If
                        dr_EditChecksDtl("nEditChecksHdrNo") = Subject_Count
                        dr_EditChecksDtl("nEditChecksDtlNo") = counter
                        dr_EditChecksDtl("nMedExEditCheckNo") = Convert.ToString(dr("nMedExEditCheckNo"))
                        dr_EditChecksDtl("nCRFDtlNo") = Convert.ToString(dr_Detail(index)("nCRFDtlNo"))
                        dr_EditChecksDtl("vQueryValue") = Convert.ToString(dr("vCondition")).Trim()
                        dr_EditChecksDtl("cQueryStatus") = Query_Generated
                        dr_EditChecksDtl("vRemarks") = IIf(Convert.ToString(dr_Detail(index)("iRepeatNo")).Trim() > 0 And count > 1, "[" & Convert.ToString(dr_Detail(index)("vNodeDisplayName")).Trim() & " Repetition : " & Convert.ToString(dr_Detail(index)("iRepeatNo")).Trim() & "]", "") & Convert.ToString(dr("vRemarks")).Trim()
                        dr_EditChecksDtl("cStatusIndi") = "N"
                        dr_EditChecksDtl("cGenerateFlag") = "Y"
                        dr_EditChecksDtl("vGenerateRemark") = "On Submit"
                        dr_EditChecksDtl("iModifyBy") = Me.Session(S_UserID)

                        dt_EditChecksDtl.Rows.Add(dr_EditChecksDtl)
                        dt_EditChecksDtl.AcceptChanges()

                        Source_MedExValue_If = String.Empty
                        Source_MedExType_If = String.Empty
                        Target_MedExValue_If = String.Empty
                        Target_MedExType_If = String.Empty
                        Source_MedExValue_Then = String.Empty
                        Source_MedExType_Then = String.Empty
                        Target_MedExValue_Then = String.Empty
                        Target_MedExType_Then = String.Empty
                        filter = String.Empty
                        Op_If = String.Empty
                        Op_Then = String.Empty

                    Next index
                Next dr
            Next drSubject

            dv_EditChecksHdr = dt_EditChecksHdr.DefaultView
            dt_EditChecksHdr = Nothing
            dt_EditChecksHdr = New DataTable()
            dt_EditChecksHdr = dv_EditChecksHdr.ToTable().Copy()

            For Each dr As DataRow In dt_EditChecksHdr.Rows

                dv_EditChecksHdr = dt_EditChecksHdr.DefaultView
                dv_EditChecksHdr.RowFilter = "nEditChecksHdrNo = " + Convert.ToString(dr("nEditChecksHdrNo")).Trim()

                dv_EditChecksDtl = dt_EditChecksDtl.DefaultView
                dv_EditChecksDtl.RowFilter = "nEditChecksHdrNo = " + Convert.ToString(dr("nEditChecksHdrNo")).Trim()

                ds_Save = Nothing
                ds_Save = New DataSet()
                ds_Save.Tables.Add(dv_EditChecksHdr.ToTable().Copy())
                ds_Save.AcceptChanges()
                ds_Save.Tables.Add(dv_EditChecksDtl.ToTable().Copy())
                ds_Save.AcceptChanges()

                If Not Me.objLambda.Save_EditChecksHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add,
                                        ds_Save, Me.Session(S_UserID), eStr) Then
                    Throw New Exception(eStr)
                End If
            Next
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Processing Edit Checks : " + ex.Message, "...FireEditChecks")
        End Try

    End Sub

    Private Sub GetDateTimeResult_If(ByVal Op As String, ByVal SourceValue As String, ByVal TargetValue As String, ByRef Is_Query As Boolean)
        If IsDate(TargetValue) Then
            If Op = ">" Then

                If CDate(SourceValue) > CDate(TargetValue) Then
                    Is_Query = True
                End If

            ElseIf Op = ">=" Then

                If CDate(SourceValue) >= CDate(TargetValue) Then
                    Is_Query = True
                End If

            ElseIf Op = "<" Then

                If CDate(SourceValue) < CDate(TargetValue) Then
                    Is_Query = True
                End If

            ElseIf Op = "<=" Then

                If CDate(SourceValue) <= CDate(TargetValue) Then
                    Is_Query = True
                End If

            ElseIf Op = "=" Then

                If CDate(SourceValue) = CDate(TargetValue) Then
                    Is_Query = True
                End If

            ElseIf Op = "<>" Then

                If CDate(SourceValue) <> CDate(TargetValue) Then
                    Is_Query = True
                End If
            End If
        ElseIf TargetValue = "NULL" Then
            If Op = "=" Then

                If IsDBNull(SourceValue) Then
                    Is_Query = True
                End If

            ElseIf Op = "<>" Then

                If Not IsDBNull(SourceValue) Then
                    Is_Query = True
                End If
            End If
        End If

    End Sub

    Private Sub GetTextValueResult_If(ByVal Op As String, ByVal SourceValue As String, ByVal TargetValue As String, ByRef Is_Query As Boolean)

        If Op = ">" Then

            If Val(SourceValue) > Val(TargetValue) Then
                Is_Query = True
            End If

        ElseIf Op = ">=" Then

            If Val(SourceValue) >= Val(TargetValue) Then
                Is_Query = True
            End If

        ElseIf Op = "<" Then

            If Val(SourceValue) < Val(TargetValue) Then
                Is_Query = True
            End If

        ElseIf Op = "<=" Then

            If Val(SourceValue) <= Val(TargetValue) Then
                Is_Query = True
            End If

        ElseIf Op = "=" Then

            If SourceValue.ToUpper = TargetValue.ToUpper Then
                Is_Query = True
            End If

        ElseIf Op = "<>" Then

            If SourceValue.ToUpper <> TargetValue.ToUpper Then
                Is_Query = True
            End If

        End If

    End Sub

    Private Sub GetDateTimeResult_Then(ByVal Op As String, ByVal SourceValue As String, ByVal TargetValue As String, ByRef Is_Query As Boolean)
        If IsDate(TargetValue) Then
            If Op = ">" Then

                If CDate(SourceValue) > CDate(TargetValue) Then
                    Is_Query = True
                End If

            ElseIf Op = ">=" Then

                If CDate(SourceValue) >= CDate(TargetValue) Then
                    Is_Query = True
                End If

            ElseIf Op = "<" Then

                If CDate(SourceValue) < CDate(TargetValue) Then
                    Is_Query = True
                End If

            ElseIf Op = "<=" Then

                If CDate(SourceValue) <= CDate(TargetValue) Then
                    Is_Query = True
                End If

            ElseIf Op = "=" Then

                If CDate(SourceValue) = CDate(TargetValue) Then
                    Is_Query = True
                End If

            ElseIf Op = "<>" Then

                If CDate(SourceValue) <> CDate(TargetValue) Then
                    Is_Query = True
                End If

            End If
        ElseIf TargetValue = "NULL" Then
            If Op = "=" Then

                If IsDBNull(SourceValue) Then
                    Is_Query = True
                End If

            ElseIf Op = "<>" Then

                If Not IsDBNull(SourceValue) Then
                    Is_Query = True
                End If
            End If
        End If
    End Sub

    Private Sub GetTextValueResult_Then(ByVal Op As String, ByVal SourceValue As String, ByVal TargetValue As String, ByRef Is_Query As Boolean)

        If Op = ">" Then

            If Val(SourceValue) > Val(TargetValue) Then
                Is_Query = True
            End If

        ElseIf Op = ">=" Then

            If Val(SourceValue) >= Val(TargetValue) Then
                Is_Query = True
            End If

        ElseIf Op = "<" Then

            If Val(SourceValue) < Val(TargetValue) Then
                Is_Query = True
            End If

        ElseIf Op = "<=" Then

            If Val(SourceValue) <= Val(TargetValue) Then
                Is_Query = True
            End If

        ElseIf Op = "=" Then

            If SourceValue.ToUpper = TargetValue.ToUpper Then
                Is_Query = True
            End If

        ElseIf Op = "<>" Then

            If SourceValue.ToUpper <> TargetValue.ToUpper Then
                Is_Query = True
            End If

        End If

    End Sub

    Private Sub GetResult(ByVal SourceValue_If As String, ByVal SourceType_If As String,
                    ByVal TargetValue_If As String, ByVal TargetType_If As String,
                    ByVal Op_If As String,
                    ByVal SourceValue_Then As String, ByVal SourceType_Then As String,
                    ByVal TargetValue_Then As String, ByVal TargetType_Then As String,
                    ByVal Op_Then As String,
                    ByRef Is_Query As Boolean)
        Dim strChkLen As Int16 = 0
        Dim strDesc As String = String.Empty
        Dim strTargetChkLen As Int16 = 0
        Dim strTargetDesc As String = String.Empty
        Try

            ''''''''''''''check if date is not in proper dateformate editchecks will fire without query:Start
            ' Addecd to check if date contains any other values then editchecks not need to fire.
            If SourceType_If = "DateTime" Or SourceType_If = "AsyncDateTime" Then
                If SourceValue_If <> DBNull.Value.ToString Or SourceValue_If <> "" Then
                    If IsDate(SourceValue_If) = False Then
                        Is_Query = 0
                        Exit Sub
                    End If
                End If
            End If
            ' Addecd to check if date contains any other values then editchecks not need to fire.
            If SourceType_Then = "DateTime" Or SourceType_Then = "AsyncDateTime" Then
                If SourceValue_Then <> DBNull.Value.ToString Or SourceValue_Then <> "" Then
                    If IsDate(SourceValue_Then) = False Then
                        Is_Query = 0
                        Exit Sub
                    End If
                End If
            End If
            If TargetType_If = "DateTime" Or TargetType_If = "AsyncDateTime" Then
                If TargetValue_If <> DBNull.Value.ToString Or TargetValue_If <> "" Then
                    If IsDate(TargetValue_If) = False Then
                        Is_Query = 0
                        Exit Sub
                    End If
                End If
                ' Addecd to check if date contains any other values then editchecks not need to fire.
            End If
            If TargetType_Then = "DateTime" Or TargetType_Then = "AsyncDateTime" Then
                If TargetValue_Then <> DBNull.Value.ToString Or TargetValue_Then <> "" Then
                    If IsDate(TargetValue_Then) = False Then
                        Is_Query = 0
                        Exit Sub
                    End If
                End If
            End If ''''''''''''''check if date is not in proper dateformate editchecks will fire without query:End

            If (SourceType_If = "DateTime" Or SourceType_If = "AsyncDateTime") Or (SourceType_If = "Time" Or SourceType_If = "AsyncTime") Then

                If SourceValue_If <> "" AndAlso (TargetValue_If <> "NULL" Or TargetValue_If <> "") Then
                    GetDateTimeResult_If(Op_If, SourceValue_If, TargetValue_If, Is_Query)
                End If

                If SourceValue_If = "" AndAlso (TargetValue_If = "NULL" Or TargetValue_If = "") Then
                    TargetValue_If = ""
                    GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
                End If

                If SourceValue_If <> "" AndAlso (TargetValue_If = "NULL" Or TargetValue_If = "") Then
                    TargetValue_If = ""
                    GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
                End If

                If SourceValue_If = "" AndAlso (TargetValue_If <> "NULL" Or TargetValue_If <> "") Then
                    GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
                End If

                If Op_Then <> "" AndAlso Is_Query = True Then
                    If (SourceValue_Then <> "" Or SourceValue_Then.ToUpper() <> "NULL") Then
                        Is_Query = False
                        If (TargetValue_Then.ToUpper() = "NULL" Or TargetValue_Then.ToUpper() = "") Then
                            TargetValue_Then = ""
                            GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                        ElseIf TargetValue_Then.ToUpper() <> "NULL" And TargetValue_Then.ToUpper() <> "" Then
                            GetDateTimeResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                        End If
                    Else
                        If TargetValue_Then = "NULL" Or TargetValue_Then = "" Then
                            Is_Query = False
                            TargetValue_Then = ""
                            GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                        ElseIf (TargetValue_Then <> "NULL" And TargetValue_Then <> "") Then
                            Is_Query = False
                            GetDateTimeResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                        End If
                    End If
                End If

            ElseIf SourceType_If = "CheckBox" Then
                strChkLen = SourceValue_If.Split(",").Length
                strDesc = SourceValue_If
                For i = 0 To strChkLen - 1
                    SourceValue_If = strDesc.Split(",")(i).ToString

                    If SourceValue_If <> "" AndAlso TargetValue_If <> "" Then
                        GetTextValueResult_If(Op_If, SourceValue_If, TargetValue_If, Is_Query)
                    End If

                    If SourceValue_If = "" AndAlso (TargetValue_If = "NULL" Or TargetValue_If = "") Then
                        TargetValue_If = ""
                        GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
                    End If
                    If SourceValue_If <> "" AndAlso (TargetValue_If = "NULL" Or TargetValue_If = "") Then
                        TargetValue_If = ""
                        GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
                    End If
                    If SourceValue_If = "" AndAlso (TargetValue_If <> "NULL" And TargetValue_If <> "") Then
                        GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
                    End If

                    If Op_Then <> "" AndAlso Is_Query = True Then
                        If SourceValue_Then <> "" Then
                            Is_Query = False
                            If TargetValue_Then.ToUpper() = "NULL" Or TargetValue_Then.ToUpper() = "" Then
                                TargetValue_Then = ""
                                GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                            ElseIf TargetValue_Then.ToUpper() <> "NULL" And TargetValue_Then.ToUpper() <> "" Then
                                GetTextValueResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                            End If
                        Else
                            If (TargetValue_Then = "NULL" Or TargetValue_Then = "") Then
                                Is_Query = False
                                TargetValue_Then = ""
                                GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                            Else
                                Is_Query = False
                                GetTextValueResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                            End If

                        End If

                    End If
                Next
            Else 'If SourceType_If = "TextBox" Then

                If SourceValue_If <> "" AndAlso TargetValue_If <> "" Then
                    GetTextValueResult_If(Op_If, SourceValue_If, TargetValue_If, Is_Query)
                End If

                If SourceValue_If = "" AndAlso (TargetValue_If = "NULL" Or TargetValue_If = "") Then
                    TargetValue_If = ""
                    GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
                End If
                If SourceValue_If <> "" AndAlso (TargetValue_If = "NULL" Or TargetValue_If = "") Then
                    TargetValue_If = ""
                    GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
                End If
                If SourceValue_If = "" AndAlso (TargetValue_If <> "NULL" And TargetValue_If <> "") Then
                    GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
                End If
                If Op_Then <> "" AndAlso Is_Query = True Then
                    If SourceType_Then = "CheckBox" Then
                        strTargetChkLen = SourceValue_Then.Split(",").Length
                        strTargetDesc = SourceValue_Then
                        For iLen = 0 To strTargetChkLen - 1
                            SourceValue_Then = strTargetDesc.Split(",")(iLen)

                            If SourceValue_Then <> "" Then
                                Is_Query = False
                                If TargetValue_Then.ToUpper() = "NULL" Or TargetValue_Then.ToUpper() = "" Then
                                    TargetValue_Then = ""
                                    GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                                ElseIf TargetValue_Then.ToUpper() <> "NULL" And TargetValue_Then.ToUpper() <> "" Then
                                    GetTextValueResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                                End If
                            Else
                                If (TargetValue_Then = "NULL" Or TargetValue_Then = "") Then
                                    Is_Query = False
                                    TargetValue_Then = ""
                                    GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                                Else
                                    Is_Query = False
                                    GetTextValueResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                                End If

                            End If
                        Next
                    Else
                        If SourceValue_Then <> "" Then
                            Is_Query = False
                            If TargetValue_Then.ToUpper() = "NULL" Or TargetValue_Then.ToUpper() = "" Then
                                TargetValue_Then = ""
                                GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                            ElseIf TargetValue_Then.ToUpper() <> "NULL" And TargetValue_Then.ToUpper() <> "" Then
                                GetTextValueResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                            End If
                        Else
                            If (TargetValue_Then = "NULL" Or TargetValue_Then = "") Then
                                Is_Query = False
                                TargetValue_Then = ""
                                GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                            Else
                                Is_Query = False
                                GetTextValueResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                            End If

                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Processing Edit Checks ", ex.Message + "...GetResult")
        End Try

    End Sub

    Private Sub GetNullValueChecked(ByVal Op As String, ByVal SourceValue As String, ByVal TargetValue As String, ByRef Is_Query As Boolean)
        If Op = "=" Then
            If SourceValue = TargetValue Then
                Is_Query = True
            End If
        ElseIf Op = "<>" Then
            If SourceValue <> TargetValue Then
                Is_Query = True
            End If
        End If
    End Sub

#End Region

#Region "Checking & Assigning DCF Values"

    Private Function CheckDiscrepancy(ByVal objControl As Control, ByVal objId As String, ByVal objValue As String, ByVal DCFType As Char,
                                     ByVal nCRFDtlNo As String) As Boolean
        Dim dt_CheckValue As New DataTable
        Dim dv_CheckValue As New DataView
        Dim LowRange As String = String.Empty
        Dim HighRange As String = String.Empty
        Dim ValidationType() As String
        Dim Alertonvalue As String = String.Empty
        Dim AlertMessage As String = String.Empty
        Dim AlertType As String = String.Empty
        Dim Is_Discrepancy As Boolean = False
        CheckDiscrepancy = False
        Dim SourceResponse As String = String.Empty
        Try
            dt_CheckValue = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Copy()
            dv_CheckValue = dt_CheckValue.DefaultView()
            dv_CheckValue.RowFilter = "vMedExCode = '" + objId.Substring(0, objId.IndexOf("R")).Trim() + "'"

            LowRange = dv_CheckValue.ToTable().Rows(0)("vLowRange").ToString.Trim()
            HighRange = dv_CheckValue.ToTable().Rows(0)("vHighRange").ToString.Trim()
            ValidationType = dv_CheckValue.ToTable().Rows(0)("vValidationType").ToString.Trim.Split(",")

            Alertonvalue = dv_CheckValue.ToTable().Rows(0)("vAlertonvalue").ToString.Trim()
            AlertMessage = dv_CheckValue.ToTable().Rows(0)("vAlertMessage").ToString.Trim()
            AlertType = dv_CheckValue.ToTable().Rows(0)("cAlertType").ToString.Trim()

            If Not objValue Is Nothing Then


                If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.TextBox" Then

                    Me.CheckIsNotNull(objValue, AlertType, Is_Discrepancy)
                    If Is_Discrepancy Then
                        If Not Me.AssignDCFValues(objValue, "Please specify value", DCFType, objId, nCRFDtlNo) Then
                            Exit Function
                        End If
                        CheckDiscrepancy = True
                        Exit Function
                    End If

                    'Commednted by Rahul Rupareliya  because it is already througn JavaScripts

                    'If Convert.ToString(objValue).Trim() <> "" Then
                    '    Me.CheckValidationType(ValidationType(0).Trim(), objValue, Is_Discrepancy)
                    '    If Is_Discrepancy Then
                    '        If Not Me.AssignDCFValues(objValue, "Validation Type Mismatch", DCFType, objId, nCRFDtlNo) Then
                    '            Exit Function
                    '        End If
                    '        CheckDiscrepancy = True
                    '        Exit Function
                    '    End If
                    'End If

                    'Me.CheckLength(ValidationType(1).Trim(), objValue, Is_Discrepancy)
                    If Is_Discrepancy Then
                        If Not Me.AssignDCFValues(objValue, "Lengh Exceeded", DCFType, objId, nCRFDtlNo) Then
                            Exit Function
                        End If
                        CheckDiscrepancy = True
                        Exit Function
                    End If

                    'Commented the code because alert is used only for the alerting the user. values can be exceed.
                    'Me.CheckLowHighRange(LowRange, HighRange, objValue, Is_Discrepancy)
                    'If Is_Discrepancy Then
                    '    If Not Me.AssignDCFValues(objValue, "Value must be between " + LowRange.Trim() + " and " + HighRange.Trim(), DCFType, objId) Then
                    '        Exit Function
                    '    End If
                    '    CheckDiscrepancy = True
                    '    Exit Function
                    'End If

                    'Commented the code because alert is used only for the alerting the user.
                    'Me.CheckAlertType(Alertonvalue, AlertType, objValue, Is_Discrepancy)
                    'If Is_Discrepancy Then
                    '    If Not Me.AssignDCFValues(objValue, AlertMessage, DCFType, objId) Then
                    '        Exit Function
                    '    End If
                    '    CheckDiscrepancy = True
                    '    Exit Function
                    'End If

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" Then

                    Me.CheckIsNotNull(objValue, AlertType, Is_Discrepancy)
                    If Is_Discrepancy Then
                        If Not Me.AssignDCFValues(objValue, "Please specify value", DCFType, objId, nCRFDtlNo) Then
                            Exit Function
                        End If
                        CheckDiscrepancy = True
                        Exit Function
                    End If

                    'Commented the code because alert is used only for the alerting the user.
                    'Me.CheckAlertType(Alertonvalue, AlertType, objValue, Is_Discrepancy)
                    'If Is_Discrepancy Then
                    '    If Not Me.AssignDCFValues(objValue, AlertMessage, DCFType, objId) Then
                    '        Exit Function
                    '    End If
                    '    CheckDiscrepancy = True
                    '    Exit Function
                    'End If

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.RadioButtonList" Then

                    Me.CheckIsNotNull(objValue, AlertType, Is_Discrepancy)
                    If Is_Discrepancy Then
                        If Not Me.AssignDCFValues(objValue, "Please specify value", DCFType, objId, nCRFDtlNo) Then
                            Exit Function
                        End If
                        CheckDiscrepancy = True
                        Exit Function
                    End If

                    'Commented the code because alert is used only for the alerting the user.
                    'Me.CheckAlertType(Alertonvalue, AlertType, objValue, Is_Discrepancy)
                    'If Is_Discrepancy Then
                    '    If Not Me.AssignDCFValues(objValue, AlertMessage, DCFType, objId) Then
                    '        Exit Function
                    '    End If
                    '    CheckDiscrepancy = True
                    '    Exit Function
                    'End If

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.CheckBoxList" Then

                    Me.CheckIsNotNull(objValue, AlertType, Is_Discrepancy)
                    If Is_Discrepancy Then
                        If Not Me.AssignDCFValues(objValue, "Please specify value", DCFType, objId, nCRFDtlNo) Then
                            Exit Function
                        End If
                        CheckDiscrepancy = True
                        Exit Function
                    End If

                    'Commented the code because alert is used only for the alerting the user.
                    'Me.CheckAlertType(Alertonvalue, AlertType, objValue, Is_Discrepancy)
                    'If Is_Discrepancy Then
                    '    If Not Me.AssignDCFValues(objValue, AlertMessage, DCFType, objId) Then
                    '        Exit Function
                    '    End If
                    '    CheckDiscrepancy = True
                    '    Exit Function
                    'End If

                End If
            End If
            Return False
        Catch ex As Exception
            Me.ShowErrorMessage("Problem While Checking Discrepancy : ", ex.Message + "...CheckDiscrepancy")
            Return True
        End Try

    End Function

    Private Sub CheckIsNotNull(ByVal objValue As String, ByVal IsNotNull As String, ByRef Is_Discrepancy As Boolean)

        If IsNotNull.Trim.ToUpper() = "Y" AndAlso objValue.Trim() = "" Then
            Is_Discrepancy = True
        End If

    End Sub

    'Private Sub CheckLowHighRange(ByVal LowRange As String, ByVal HighRange As String, ByVal objValue As String, ByRef Is_Discrepancy As Boolean)
    '    Dim lr As Integer = 0
    '    Dim hr As Integer = 0
    '    Dim value As Integer = 0

    '    If LowRange <> "" Then
    '        If HighRange <> "" Then
    '            If Not Integer.TryParse(LowRange, lr) Then
    '                Is_Discrepancy = True
    '            End If
    '            If Not Integer.TryParse(HighRange, hr) Then
    '                Is_Discrepancy = True
    '            End If
    '            If Not Integer.TryParse(objValue, value) Then
    '                Is_Discrepancy = True
    '            End If

    '            If value < lr OrElse value > hr Then
    '                Is_Discrepancy = True
    '            End If
    '        End If
    '    Else
    '        If HighRange <> "" Then
    '            If Not Integer.TryParse(HighRange, hr) Then
    '                Is_Discrepancy = True
    '            End If

    '            If Not Integer.TryParse(objValue, value) Then
    '                Is_Discrepancy = True
    '            End If

    '            If value > hr Then
    '                Is_Discrepancy = True
    '            End If

    '        End If
    '    End If
    'End Sub

    Private Sub CheckValidationType(ByVal ValidationType As String, ByVal objValue As String, ByRef Is_Discrepancy As Boolean)

        If ValidationType <> "" Then

            If ValidationType.ToUpper() = Val_IN Then
                Dim result As Integer = 0
                If Not Integer.TryParse(objValue, result) Then
                    Is_Discrepancy = True
                End If
            ElseIf ValidationType.ToUpper() = Val_NU Then
                Dim result As Integer = 0
                If Not Decimal.TryParse(objValue, result) Then
                    Is_Discrepancy = True
                End If
            ElseIf ValidationType.ToUpper() = Val_AL Then
                Dim result As Char
                Dim str() As String
                str = objValue.Split("")
                For index As Integer = 0 To str.Length - 1
                    If Not Char.TryParse(str(index), result) Then
                        Is_Discrepancy = True
                        Exit Sub
                    End If
                Next

            ElseIf ValidationType.ToUpper() = Val_NNI OrElse ValidationType.ToUpper() = Val_NNU Then
                Dim result As Integer = 0
                If objValue.Trim() = "" AndAlso Not Integer.TryParse(objValue, result) Then
                    Is_Discrepancy = True
                End If

            ElseIf ValidationType.ToUpper() = Val_AN Then

                Dim resultchar As Char
                Dim resultinteger As Integer = 0
                Dim str() As String
                str = objValue.Split("")
                For index As Integer = 0 To str.Length - 1
                    If Not Char.TryParse(str(index), resultchar) OrElse
                                            Not Integer.TryParse(str(index), resultinteger) Then
                        Is_Discrepancy = True
                        Exit Sub
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub CheckLength(ByVal length As String, ByVal objValue As String, ByRef Is_Discrepancy As Boolean)
        If length <> "" AndAlso length <> "0" Then
            Dim result As Integer = 0
            If Not Integer.TryParse(length, result) Then
                Is_Discrepancy = True
            End If

            If objValue.Length > result Then
                Is_Discrepancy = True
            End If
        End If
    End Sub

    'Private Sub CheckAlertType(ByVal Alertonvalue As String, ByVal AlertType As String, ByVal objValue As String, ByRef Is_Discrepancy As Boolean)
    '    Dim values() As String
    '    Dim count As Integer = 0
    '    values = objValue.Split(",")
    '    For count = 0 To values.Length - 1

    '        If Alertonvalue <> "" Then
    '            If Alertonvalue.ToUpper() = values(count).ToUpper() Then
    '                Is_Discrepancy = True
    '                Exit For
    '            End If
    '        End If

    '    Next count
    'End Sub

    Private Function AssignDCFValues(ByVal dicrepancy As String, ByVal dicrepancyResponse As String, ByVal DCFType As Char, ByVal MedExCode As String,
                                   ByVal nCRFDtlNo As String) As Boolean
        Dim dt_DCF As New DataTable
        Dim drDCF As DataRow
        Try
            dt_DCF = CType(Me.ViewState(VS_DtDCF), DataTable).Copy()

            'nDCFNo,nCRFSubDtlNo,cDCFType,iDCFBy,dDCFDate,vDiscrepancy,vSourceResponse
            'cDCFStatus,iStatusChangedBy,dStatusChangedOn,iModifyBy,

            drDCF = dt_DCF.NewRow()
            drDCF("nDCFNo") = 0
            drDCF("nCRFDtlNo") = 0

            If nCRFDtlNo <> "" Then
                drDCF("nCRFDtlNo") = nCRFDtlNo
            End If

            'If Me.HFCRFDtlNo.Value.Trim() <> "" Then
            '    drDCF("nCRFDtlNo") = CType(Me.HFCRFDtlNo.Value.Trim(), Integer)
            'End If

            drDCF("iSrNo") = 0
            drDCF("vMedExcode") = MedExCode
            drDCF("cDCFType") = DCFType
            drDCF("iDCFBy") = Me.Session(S_UserID)
            drDCF("dDCFDate") = System.DateTime.Now()
            drDCF("vDiscrepancy") = dicrepancy
            drDCF("vSourceResponse") = dicrepancyResponse
            drDCF("cDCFStatus") = "N"
            drDCF("iModifyBy") = Me.Session(S_UserID)
            drDCF("dModifyOn") = DateTime.Now()
            dt_DCF.Rows.Add(drDCF)
            dt_DCF.AcceptChanges()

            Me.ViewState(VS_DtDCF) = dt_DCF.Copy()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Problem While Assigning DCF Values : ", ex.Message + "...AssignDCFValues")
            Return False
        End Try

    End Function

#End Region

#Region "Review Completed"

    Private Sub ReviewAllActivities()
        Dim ds_CRFWorkFlowDtl As New DataSet
        Dim ds_CRFDtl As New DataSet
        Dim ds_CRFHdr As New DataSet
        Dim dv_CRFDtl As DataView
        Dim ds_DCF As New DataSet
        Dim dr_DCF() As DataRow
        Dim dr As DataRow
        Dim dr_WorkFlowDtl As DataRow
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_reviewer As New DataSet
        Dim dv_reviewer As DataView
        Dim idynamicworkflowstageid As Integer = 0

        Try
            ' Change to review all activities of the parent activity --Pratiksha
            wStr = "vWorkSpaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And iPeriod = " + Me.HFPeriodId.Value.Trim() + " And "
            wStr += "iNodeId in (Select iNodeId From View_WorkSpaceNodeDetail Where vWorkSpaceId = '" + Me.HFWorkspaceId.Value.Trim() + "'"
            If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                If Me.HFSubjectId.Value.Trim.ToString() = "0000" Then
                    wStr += " And cSubjectWiseFlag = 'N' "
                Else
                    wStr += " And cSubjectWiseFlag = 'Y' "
                End If
            End If
            wStr += " And iPeriod = " + Me.HFPeriodId.Value.Trim() + " And (iParentNodeId = " + Me.HFParentNodeId.Value.Trim()
            If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                wStr += "  Or iNodeId =  " + Me.HFParentNodeId.Value.Trim()
            End If
            wStr += "  ) )"

            'wStr = "vWorkSpaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And iPeriod = " + Me.HFPeriodId.Value.Trim() + " And "
            'wStr += "iNodeId in (Select iNodeId From WorkspaceNodeDetail Where vWorkSpaceId = '" + Me.HFWorkspaceId.Value.Trim()
            'wStr += "' And iPeriod = " + Me.HFPeriodId.Value.Trim() + " And iParentNodeId = " + Me.HFParentNodeId.Value.Trim() + ")"
            '--Pratiksha

            If Not Me.objHelp.GetCRFHdr(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                ds_CRFHdr, eStr) Then
                Throw New Exception(eStr)
            End If

            wStr = "vSubjectId = '" + Me.HFSubjectId.Value.Trim() + "' And nCRFHdrNo in("

            If Not ds_CRFHdr Is Nothing Then
                For Each dr In ds_CRFHdr.Tables(0).Rows
                    wStr += Convert.ToString(dr("nCRFHdrNo")).Trim() + ","
                Next dr
            End If
            ''Added by nipun khant for dynamic review====================================
            ds_reviewer = CType(Me.Session(Vs_dsReviewerlevel), DataSet)
            dv_reviewer = ds_reviewer.Tables(0).Copy.DefaultView
            dv_reviewer.RowFilter = "iReviewWorkflowStageId = " + Me.Session(S_WorkFlowStageId) + " AND vUserTypeCode = '" + Me.Session(S_UserType) + "'"

            If dv_reviewer.ToTable().Rows.Count = 0 Then
                Exit Sub
            End If
            idynamicworkflowstageid = dv_reviewer.ToTable.Rows(0)("iActualWorkflowStageId")
            ''===========================================================================
            wStr = wStr.Substring(0, wStr.LastIndexOf(","))
            ''Added by nipun khant for dynamic review
            wStr += ") And iWorkFlowStageId = " + (idynamicworkflowstageid - 10).ToString()
            ''Commented by nipun khant for dynamic review
            'wStr += ") And iWorkFlowStageId = " + (Convert.ToInt32(Me.Session(S_WorkFlowStageId)) - 10).ToString()
            wStr += " And cDataStatus <> '" + CRF_DataEntry + "'"

            If Not Me.objHelp.GetCRFDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                            ds_CRFDtl, eStr) Then
                Throw New Exception(eStr)
            End If

            ''wStr = "iDCFBy = " + Convert.ToString(Me.Session(S_UserID)).Trim() + " And (cDCFStatus = '"
            'wStr = " (cDCFStatus = '"
            'wStr += Discrepancy_Generated + "' Or cDCFStatus = '" + Discrepancy_Answered + "') And nCRFDtlNo in("
            'If Not ds_CRFDtl Is Nothing Then
            '    For Each dr In ds_CRFDtl.Tables(0).Rows
            '        wStr += Convert.ToString(dr("nCRFDtlNo")).Trim() + ","
            '    Next dr
            'End If
            'wStr = wStr.Substring(0, wStr.LastIndexOf(",")) + ")"

            'If Not Me.objHelp.GetDCFMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '                            ds_DCF, eStr) Then
            '    Throw New Exception(eStr)
            'End If
            wStr = " (cDCFStatus = '" + Discrepancy_Generated + "' Or cDCFStatus = '" + Discrepancy_Answered + "')"
            ''Added by nipun khant for dynamic review====================================
            wStr += " And (((vUserTypeCode = '" + Convert.ToString(Me.Session(S_UserType)).Trim() _
                    + "' Or CRFWorkflowBy < " + Convert.ToString(idynamicworkflowstageid) + ") And cDCFType = 'M') "
            ''Commented by nipun khant for dynamic review
            'wStr += " And (((vUserTypeCode = '" + Convert.ToString(Me.Session(S_UserType)).Trim() _
            '        + "' Or CRFWorkflowBy < " + Me.Session(S_WorkFlowStageId).ToString() + ") And cDCFType = 'M') "
            wStr += " Or cDCFType = 'S') And CRFWorkflowBy <> " & WorkFlowStageId_OnlyView
            wStr += " And nCRFDtlNo in("
            If Not ds_CRFDtl Is Nothing Then
                For Each dr In ds_CRFDtl.Tables(0).Rows
                    wStr += Convert.ToString(dr("nCRFDtlNo")).Trim() + ","
                Next dr
            End If
            wStr = wStr.Substring(0, wStr.LastIndexOf(",")) + ")"

            If Not Me.objHelp.View_DCFMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                        ds_DCF, eStr) Then
                Throw New Exception(eStr)
            End If
            If Not ds_CRFDtl Is Nothing AndAlso Not ds_DCF Is Nothing Then
                For Each dr In ds_CRFDtl.Tables(0).Rows
                    dr_DCF = ds_DCF.Tables(0).Select("nCRFDtlNo = " + Convert.ToString(dr("nCRFDtlNo")).Trim())
                    If dr_DCF.Length > 0 Then
                        dr("cStatusIndi") = "D"
                        ds_CRFDtl.Tables(0).AcceptChanges()
                    End If
                Next dr
            End If
            dv_CRFDtl = ds_CRFDtl.Tables(0).Copy.DefaultView
            dv_CRFDtl.RowFilter = "cStatusIndi <> 'D'"
            ds_CRFDtl = Nothing
            ds_CRFDtl = New DataSet
            ds_CRFDtl.Tables.Add(dv_CRFDtl.ToTable().Copy())

            If Not Me.objHelp.GetCRFWorkFlowDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty,
                                ds_CRFWorkFlowDtl, eStr) Then
                Throw New Exception(eStr)
            End If

            For Index As Integer = 0 To ds_CRFDtl.Tables(0).Rows.Count - 1

                ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                If Convert.ToString(ds_CRFDtl.Tables(0).Rows(Index)("iWorkFlowStageId")).Trim.ToUpper() =
                                            CType(Me.Session(vs_workflowstageidfordynamic), String).ToUpper() Then
                    ds_CRFDtl.Tables(0).Rows(Index).Delete()
                    Continue For
                End If
                ''Commented by nipun khant for dynamic review
                'If Convert.ToString(ds_CRFDtl.Tables(0).Rows(Index)("iWorkFlowStageId")).Trim.ToUpper() = _
                '                            CType(Me.Session(S_WorkFlowStageId), String).ToUpper() Then
                '    ds_CRFDtl.Tables(0).Rows(Index).Delete()
                '    Continue For
                'End If

                ds_CRFDtl.Tables(0).Rows(Index)("iWorkFlowStageId") = Me.Session(S_WorkFlowStageId)
                ds_CRFDtl.Tables(0).Rows(Index)("cDataStatus") = CRF_ReviewCompleted

                ''Added by nipun khant for dynamic review
                dv_reviewer = ds_reviewer.Tables(0).Copy.DefaultView
                dv_reviewer.RowFilter = "iReviewWorkflowStageId = " + Me.Session(S_WorkFlowStageId) + " and vUserTypeCode = '" + Me.Session(S_UserType) + "'"

                If dv_reviewer.ToTable().Rows.Count > 0 Then
                    If Convert.ToString(dv_reviewer.ToTable.Rows(0)("vStatus")).ToUpper = "L" Then
                        ds_CRFDtl.Tables(0).Rows(Index)("cDataStatus") = CRF_Locked
                    End If
                End If


                ''Commented by nipun khant for dynamic review
                'If CType(Me.Session(S_WorkFlowStageId), String).ToUpper() = WorkFlowStageId_FinalReviewAndLock Then
                '    ds_CRFDtl.Tables(0).Rows(Index)("cDataStatus") = CRF_Locked
                'End If

                dr_WorkFlowDtl = ds_CRFWorkFlowDtl.Tables(0).NewRow()
                dr_WorkFlowDtl("nCRFWorkFlowNo") = 0
                dr_WorkFlowDtl("nCRFDtlNo") = Convert.ToString(ds_CRFDtl.Tables(0).Rows(Index)("nCRFDtlNo")).Trim()
                dr_WorkFlowDtl("iTranNo") = 0
                dr_WorkFlowDtl("iWorkFlowStageId") = Me.Session(S_WorkFlowStageId)
                dr_WorkFlowDtl("iModifyBy") = Me.Session(S_UserID)
                dr_WorkFlowDtl("cStatusIndi") = "N"
                dr_WorkFlowDtl("cReviewFlag") = "A"
                ds_CRFWorkFlowDtl.Tables(0).Rows.Add(dr_WorkFlowDtl)
                ds_CRFWorkFlowDtl.AcceptChanges()

            Next Index

            ds_CRFDtl.AcceptChanges()
            If ds_CRFDtl.Tables(0).Rows.Count > 0 Then
                If Not Me.objLambda.Save_CRFDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit,
                                ds_CRFDtl, Me.Session(S_UserID), eStr) Then
                    Throw New Exception(eStr)
                End If
            End If

            If ds_CRFWorkFlowDtl.Tables(0).Rows.Count > 0 Then

                If Not Me.objLambda.Save_CRFWorkFlowDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add,
                               ds_CRFWorkFlowDtl, Me.Session(S_UserID), eStr) Then
                    Throw New Exception(eStr)
                End If
            End If

            '  Me.objCommon.ShowAlert("Review All Completed Successfully.", Me.Page)
            'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "DivAuthenticationHideShow('H');", True)
            'Me.MpeAuthentication.Hide()
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "funCloseDiv", "funCloseDiv('divAuthentication');", True)

        Catch ex As Exception
            Me.ShowErrorMessage("Problem While Reviewing All Activities. ", eStr)
        End Try

    End Sub

    Protected Sub btnAuthenticate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAuthenticate.Click
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_CRFWorkFlowDtl As New DataSet
        Dim ds_CRFDtl As New DataSet
        Dim Pwd As String = String.Empty
        Dim ds_reviewer As New DataSet
        Dim dv_reviewer As DataView

        Try
            ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
            ds_reviewer = CType(Me.Session(Vs_dsReviewerlevel), DataSet)
            dv_reviewer = ds_reviewer.Tables(0).Copy.DefaultView
            dv_reviewer.RowFilter = "iReviewWorkflowStageId = " + Me.Session(S_WorkFlowStageId).ToString() + " and vUserTypeCode = '" + Me.Session(S_UserType) + "'"
            If dv_reviewer.ToTable().Rows.Count = 1 Then
                If dv_reviewer.ToTable().Rows(0)("cAuthenticationDialog").ToString().ToUpper() = "Y" Then

                    Pwd = Me.txtPassword.Text.Trim()
                    Pwd = objHelp.EncryptPassword(Pwd)

                    If Pwd.ToUpper() <> CType(Me.Session(S_Password), String).ToUpper() Then
                        Me.txtPassword.Text = ""
                        objCommon.ShowAlert("Password Authentication Fails.", Me.Page)
                        'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "DivAuthenticationHideShow('S');", True)
                        Me.divAuthentication.Style.Add("display", "")
                        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "UnBindClick", "$('#btnAuthenticate').unbind('click');", True)
                        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "displayBackGround", "displayBackGround();", True)
                        Me.txtPassword.Focus()
                        Exit Sub
                    End If
                End If
            ElseIf dv_reviewer.ToTable().Rows.Count = 0 Then
                objCommon.ShowAlert("You are no authorize person to review activity.", Me.Page)
                'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "DivAuthenticationHideShow('S');", True)
                Me.divAuthentication.Style.Add("display", "")
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "UnBindClick", "$('#btnAuthenticate').unbind('click');", True)
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "displayBackGround", "displayBackGround();", True)
                Me.txtPassword.Focus()
                Exit Sub
            End If

            If Not SaveCRFDtl() Then
                Me.objCommon.ShowAlert("Error occured while saving CRFWorkFlow Detail.", Me.Page)
                Exit Sub
            End If

            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "funCloseDiv", "funCloseDiv('divAuthentication');", True)

        Catch ex As Exception
            Me.ShowErrorMessage("Problem While Review Completed : ", ex.Message + "...btnAuthenticate_Click")
        End Try

    End Sub

    Private Function SaveCRFDtl() As Boolean

        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_CRFWorkFlowDtl As New DataSet
        Dim dr As DataRow
        Dim ds_CRFDtl As New DataSet
        Dim StrRedirect As String = String.Empty
        Dim ds_reviewer As New DataSet
        Dim dv_reviewer As DataView

        Try
            If Me.chkReviewAll.Checked Then

                Me.ReviewAllActivities()
                Me.HFSessionFlg.Value = "1"
                If Not Getstring(StrRedirect) Then
                    Me.objCommon.ShowAlert("Error occured while getting string.", Me.Page)
                    Return False
                    Exit Function
                End If
                objCommon.ShowAlertAndRedirectAfter6Second("Review All Completed Successfully.", "'" + StrRedirect + "'", Me.Page)
                Return True
                Exit Function

            End If

            ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
            If Me.HFCRFDtlNo.Value.Trim() = "" OrElse Me.Session(vs_workflowstageidfordynamic) Is Nothing OrElse
                                    Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() = "" Then
                Me.objCommon.ShowAlert("CRFDtl No./WorkFlowStage Id Not Found.", Me.Page)
                Return False
                Exit Function
            End If
            ''Commented by nipun khant for dynamic review
            'If Me.HFCRFDtlNo.Value.Trim() = "" OrElse Me.Session(S_WorkFlowStageId) Is Nothing OrElse _
            '                        Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = "" Then
            '    Me.objCommon.ShowAlert("CRFDtl No./WorkFlowStage Id Not Found.", Me.Page)
            '    Return False
            '    Exit Function
            'End If
            '**************Checking if Review already done ******************

            'wStr = "nCRFDtlNo = " + Me.HFCRFDtlNo.Value.Trim() + " And iWorkFlowStageId = " + _
            '                    Me.Session(S_WorkFlowStageId) + " And cStatusIndi <> 'D'"

            'If Not Me.objHelp.GetCRFWorkFlowDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '                    ds_CRFWorkFlowDtl, eStr) Then
            '    Throw New Exception(eStr)
            'End If

            'If ds_CRFWorkFlowDtl.Tables(0) Is Nothing Then
            '    Throw New Exception("Review Details Not Found.")
            'End If

            'If ds_CRFWorkFlowDtl.Tables(0).Rows.Count > 0 Then
            '    Me.objCommon.ShowAlert("Review Already Done For Current User Type.", Me.Page)
            '    'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "DivAuthenticationHideShow('H');", True)
            '    Me.MpeAuthentication.Hide()
            '    Exit Sub
            'End If

            '*******************************************
            '*****Added for Re-review***********
            If Not Me.objHelp.GetCRFWorkFlowDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty,
                                ds_CRFWorkFlowDtl, eStr) Then
                Throw New Exception(eStr)
            End If
            '***********************************
            ds_CRFWorkFlowDtl.Tables(0).Clear()
            ds_CRFWorkFlowDtl.AcceptChanges()

            dr = ds_CRFWorkFlowDtl.Tables(0).NewRow()
            'nCRFWorkFlowNo,nCRFDtlNo,iTranNo,iWorkFlowStageId,dModifyOn,iModifyBy,cStatusIndi
            dr("nCRFWorkFlowNo") = 0
            dr("nCRFDtlNo") = Me.HFCRFDtlNo.Value.Trim()
            dr("iTranNo") = 0
            dr("iWorkFlowStageId") = Me.Session(S_WorkFlowStageId)
            dr("iModifyBy") = Me.Session(S_UserID)
            dr("cStatusIndi") = "N"
            dr("cReviewFlag") = "M"
            ds_CRFWorkFlowDtl.Tables(0).Rows.Add(dr)
            ds_CRFWorkFlowDtl.AcceptChanges()

            If Not Me.objLambda.Save_CRFWorkFlowDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add,
                    ds_CRFWorkFlowDtl, Me.Session(S_UserID), eStr) Then
                Throw New Exception(eStr)
            End If

            '*********Updating datastatus flag for review
            wStr = "nCRFDtlNo = " + Me.HFCRFDtlNo.Value.Trim() + " And cStatusIndi <> 'D'"

            If Not Me.objHelp.GetCRFDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                ds_CRFDtl, eStr) Then
                Throw New Exception(eStr)
            End If

            ds_reviewer = CType(Me.Session(Vs_dsReviewerlevel), DataSet)
            dv_reviewer = ds_reviewer.Tables(0).Copy.DefaultView
            dv_reviewer.RowFilter = "iReviewWorkflowStageId = " + Me.Session(S_UserType) + " and vUserTypeCode = '" + Me.Session(S_UserType) + "'"

            For Each dr In ds_CRFDtl.Tables(0).Rows
                dr("cDataStatus") = CRF_ReviewCompleted
                dr("iWorkFlowStageId") = Me.Session(S_WorkFlowStageId)
                ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                If dv_reviewer.ToTable.Rows.Count > 0 Then
                    If Convert.ToString(dv_reviewer.ToTable.Rows(0)("vStatus")).ToUpper() = "L" Then
                        dr("cDataStatus") = CRF_Locked
                    End If
                End If

                ''Commented by nipun khant for dynamic review
                'If CType(Me.Session(S_WorkFlowStageId), String).ToUpper() = WorkFlowStageId_FinalReviewAndLock Then
                '    dr("cDataStatus") = CRF_Locked
                'End If

                'If CType(Me.Session(S_WorkFlowStageId), String).ToUpper() = WorkFlowStageId_FinalReviewAndLock Then
                '    dr("cDataStatus") = CRF_Locked
                'End If
            Next
            ds_CRFDtl.AcceptChanges()
            If Not Me.objLambda.Save_CRFDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit,
                            ds_CRFDtl, Me.Session(S_UserID), eStr) Then
                Throw New Exception(eStr)
            End If

            Me.HFSessionFlg.Value = "1"
            If Not Getstring(StrRedirect) Then
                Me.objCommon.ShowAlert("Error occured while getting string.", Me.Page)
                Return False
                Exit Function
            End If

            objCommon.ShowAlertAndRedirectAfter6Second("Review Completed Successfully.", "'" + StrRedirect + "'", Me.Page)

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Return False
        End Try
    End Function

    Private Function Getstring(ByRef StrRedirect As String, Optional ByVal strFrom As String = "") As Boolean
        Dim choice As String = String.Empty
        Dim eStr As String = String.Empty
        Try

            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                choice = "1"
            ElseIf CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                choice = "2"
            ElseIf CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                choice = "4"
            End If

            If Me.HFType.Value = Type_BABE Then
                'If Me.Session(S_ScopeNo) = Scope_BABE Then
                StrRedirect = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + Me.HFWorkspaceId.Value.Trim() +
                                  "&ActivityId=" + Me.HFParentActivityId.Value.Trim() + "&NodeId=" + Me.HFParentNodeId.Value.Trim() +
                                  "&PeriodId=" + Me.HFPeriodId.Value.Trim() + "&Type=" + Type_BABE + "&SubjectId=" + Me.HFSubjectId.Value.Trim() +
                                  "&MySubjectNo=" + Me.HFMySubjectNo.Value.Trim() + "&ScreenNo=" + Me.HFScreenNo.Value.Trim() '+ "&Mode=" + choice
                If Not IsNothing(Me.Request.QueryString("Mode")) Then
                    If Convert.ToString(Me.Request.QueryString("Mode")).Trim() <> "" Then
                        StrRedirect += "&Mode=" + Me.Request.QueryString("Mode")
                    End If
                End If

                If Not IsNothing(Me.Request.QueryString("From")) AndAlso Convert.ToString(Me.Request.QueryString("From")).Trim().ToUpper = "SCH" Then
                    If Not Session(S_DynamicPage_URL) Is Nothing AndAlso Session(S_DynamicPage_URL) <> "" Then
                        StrRedirect = Session(S_DynamicPage_URL)
                    ElseIf StrRedirect.ToString <> "" Then
                        StrRedirect += "&From=SCH"
                    End If
                End If

            Else

                StrRedirect = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + Me.HFWorkspaceId.Value.Trim() +
                                  "&ActivityId=" + Me.HFParentActivityId.Value.Trim() + "&NodeId=" + Me.HFParentNodeId.Value.Trim() +
                                  "&PeriodId=" + Me.HFPeriodId.Value.Trim() + "&SubjectId=" + Me.HFSubjectId.Value.Trim() +
                                  "&MySubjectNo=" + Me.HFMySubjectNo.Value.Trim() + "&ScreenNo=" + Me.HFScreenNo.Value.Trim()
                If Not IsNothing(Me.Request.QueryString("Mode")) Then
                    If Convert.ToString(Me.Request.QueryString("Mode")).Trim() <> "" Then
                        StrRedirect += "&Mode=" + Me.Request.QueryString("Mode")
                    End If
                End If
                If Not IsNothing(Me.Request.QueryString("From")) AndAlso Convert.ToString(Me.Request.QueryString("From")).Trim().ToUpper = "SCH" Then
                    If StrRedirect.ToString <> "" Then
                        StrRedirect += "&From=SCH"
                    End If
                End If
            End If

            Me.Session(S_SelectedActivity + CurrentPageSessionVariable) = Me.ddlActivities.SelectedIndex.ToString()

            Me.Session(S_SelectedTab) = Me.HFActivateTab.Value
            If Me.HFSessionFlg.Value = "" Then
                Me.Session(S_SelectedTab) = Nothing
            End If
            Me.HFActivateTab.Value = ""
            'Session(S_DynamicPage_URL) = Nothing
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Return False
        End Try
    End Function

#End Region

#Region "Repeatation Related"

    Private Function fillRepeatation() As Boolean
        Dim ds_AuditTrail As New DataSet
        Dim dv_AuditTrail As New DataView
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim RepeatitonData() As String
        Try

            Wstr = "vWorkspaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And vSubjectId='" &
                    Me.HFSubjectId.Value.Trim() & "' And " &
                  " iNodeId=" & Me.HFNodeId.Value.Trim() & " And vActivityId = '" & Me.HFActivityId.Value.Trim() & "'"

            'If Not Me.objHelp.View_CRFHdrDtlSubDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '                 ds_AuditTrail, estr) Then
            '    Return False
            'End If

            If Not Me.objHelp.GetData("View_CrfHdrDtlRepetation", "*,DENSE_RANK() OVER(PARTITION BY nCRFHdrNo,vActivityId,vSubjectId ORDER BY nCRFHdrNo,vActivityId,vSubjectId,iRepeatNo) as [RepetitionNo] ", Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_AuditTrail, estr) Then
                Return False
            End If

            Me.Session(S_Repeatition) = ds_AuditTrail.Tables(0)

            dv_AuditTrail = ds_AuditTrail.Tables(0).DefaultView().ToTable(True, "RepetitionNo,dRepeatationDate,CRFDtlChangedBy".Split(",")).DefaultView()
            dv_AuditTrail.Sort = "RepetitionNo desc"

            Me.ddlRepeatNo.Items.Clear()

            For Each dr As DataRow In dv_AuditTrail.ToTable().Rows

                Me.ddlRepeatNo.Items.Add(New ListItem(dr("RepetitionNo").ToString() + "-" +
                                        IIf(dr("CRFDtlChangedBy").ToString.Trim() = "", "", "" +
                                        dr("CRFDtlChangedBy").ToString() + " On: " + CType(dr("dRepeatationDate"), Date).ToString("dd-MMM-yyyy-HH:mm")),
                                        dr("RepetitionNo").ToString())) '' Time Format changed By Dipen Shah on 7-March-2015 to change the time format in repeatation (24 hour format from 12 hour format)

            Next dr


            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

                Me.ddlRepeatNo.Items.Insert(0, New ListItem("New Repetition", "N"))
                '  Me.ddlRepeatNo.SelectedIndex = 0
                If Me.ddlRepeatNo.Items.Count > 1 Then
                    ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                    If Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() <> WorkFlowStageId_DataEntry Then
                        Me.ddlRepeatNo.SelectedItem.Enabled = False
                        Me.ddlRepeatNo.SelectedIndex = 1
                    End If
                    ''Commented by nipun khant for dynamic review
                    'If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_DataEntry Then
                    '    Me.ddlRepeatNo.SelectedItem.Enabled = False
                    '    Me.ddlRepeatNo.SelectedIndex = 1
                    'End If
                    If Me.Session(S_SelectedRepeatation + CurrentPageSessionVariable + hdnActivityName.Value) Is Nothing Or Convert.ToString(Me.Session(S_SelectedRepeatation + CurrentPageSessionVariable + hdnActivityName.Value)).ToUpper() = "N" Then
                        If Convert.ToString(Me.Session(S_SelectedRepeatation + CurrentPageSessionVariable + hdnActivityName.Value)).ToUpper() = "" And Me.Session(S_GetLetestData + CurrentPageSessionVariable + hdnActivityName.Value) <> "getData" Then
                            Me.ddlRepeatNo.SelectedIndex = 1
                            Me.hndRepetitionNo.Value = ddlRepeatNo.SelectedValue.Trim()
                            Me.Session(S_SelectedRepeatation + CurrentPageSessionVariable + hdnActivityName.Value) = ddlRepeatNo.SelectedValue.Trim()
                        Else
                            Me.ddlRepeatNo.SelectedIndex = 1
                        End If
                    Else

                        Try
                            If ddlRepeatNo.Items.Count - 1 < Me.hndRepetitionNo.Value.Split(",")(0) Then
                                Me.ddlRepeatNo.SelectedValue = Me.ddlRepeatNo.Items.Count - 1
                                Me.hndRepetitionNo.Value = Me.ddlRepeatNo.Items.Count - 1
                            Else
                                Me.ddlRepeatNo.SelectedValue = Me.hndRepetitionNo.Value.Split(",")(0)
                            End If

                        Catch ex As Exception
                            Try
                                Me.ddlRepeatNo.SelectedValue = Me.ddlRepeatNo.Items.Count - 1
                                Me.hndRepetitionNo.Value = Me.ddlRepeatNo.Items.Count - 1
                            Catch exx As Exception
                                Me.hndRepetitionNo.Value = 1
                            End Try

                        End Try

                    End If
                End If
            End If

            'Commented by shivani pandya
            'If Not Me.Session(S_SelectedRepeatation) Is Nothing Then

            '    If Convert.ToString(Me.Session(S_SelectedRepeatation)).ToUpper() = "N" Then
            '        Me.ddlRepeatNo.SelectedIndex = 0
            '    Else
            '        Me.ddlRepeatNo.SelectedIndex = Me.ddlRepeatNo.Items.IndexOf(Me.ddlRepeatNo.Items.FindByValue(CType(Me.Session(S_SelectedRepeatation), Integer).ToString()))
            '        Me.HFIsRepeatNo.Value = 1
            '    End If
            'End If

            'Me.Session(S_SelectedRepeatation) = Nothing

            If Me.ddlRepeatNo.SelectedValue.Trim.ToUpper() = "N" Then
                Me.HFReviewedWorkFlowId.Value = ""
                Me.HFImportedDataWorkFlowId.Value = ""
                Me.lblLastReviewedBy.Text = ""
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...fillRepeatation")
            Return False
        End Try

    End Function

    Protected Sub ddlRepeatNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRepeatNo.SelectedIndexChanged
        Dim dt As DataTable = Nothing
        Dim wstr As String = String.Empty
        Dim ds_DataEntryControl As DataSet = Nothing
        Dim estr As String = String.Empty
        Dim ds As New DataSet
        Dim dr As DataRow
        Try
            Me.Session(S_GetLetestData + CurrentPageSessionVariable + hdnActivityName.Value) = Nothing
            If Me.ddlRepeatNo.SelectedValue.Trim() <> "N" Then
                Selectvar = Me.ddlActivities.SelectedIndex.ToString
                '  Me.Session(S_SelectedRepeatation) = Me.ddlRepeatNo.SelectedValue.Trim()
                Me.ViewState(VS_dtMedEx_Fill) = CType(Me.ViewState(VS_dtMedEx_Fill_Backup), DataTable).Copy()
                Me.PlaceMedEx.Controls.Clear()
                Me.HFIsRepeatNo.Value = 1
                dt = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Copy()

                If flagDCF = True Then
                    dt.DefaultView.RowFilter = " RepetitionNo=" + Me.ddlRepeatNo.SelectedValue.Trim()
                Else
                    dt.DefaultView.RowFilter = " RepetitionNo In (" + Me.hndRepetitionNo.Value.Trim() + ")"
                End If

                If dt.DefaultView.ToTable.Rows.Count <= 0 Then
                    Me.ViewState(VS_dtMedEx_Fill) = Nothing
                    Me.ViewState(VS_dtMedEx_Fill) = CType(Me.ViewState(VS_dtMedEx_Fill_Backup), DataTable).Copy()
                    dt = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Copy()
                    If flagDCF = True Then
                        dt.DefaultView.RowFilter = " RepetitionNo=" + Me.ddlRepeatNo.SelectedValue.Trim()
                    Else
                        dt.DefaultView.RowFilter = " RepetitionNo In (" + Me.hndRepetitionNo.Value.Trim() + ")"
                    End If
                End If

                Me.ViewState(VS_dtMedEx_Fill) = Nothing
                Me.ViewState(VS_dtMedEx_Fill) = dt.DefaultView.ToTable.Copy()

                If Not GenCall_ShowUI() Then
                    Me.ViewState(VS_dtMedEx_Fill) = Nothing
                    Me.ViewState(VS_dtMedEx_Fill) = dt.Copy()
                    Throw New Exception("Error in filtering Repetition No.... ddlRepeatNo_SelectedIndexChanged")
                End If
                Me.ViewState(VS_dtMedEx_Fill) = Nothing
                Me.ViewState(VS_dtMedEx_Fill) = dt.Copy()

            End If
            If Me.ddlRepeatNo.SelectedValue.Trim() = "N" Then
                Selectvar = Me.ddlActivities.SelectedIndex.ToString
                Me.PlaceMedEx.Controls.Clear()
                If Not GenCall_ShowUI() Then
                    Throw New Exception("Error in filtering Repetition No.... ddlRepeatNo_SelectedIndexChanged")
                End If
            End If

            ddlRepeatNo.Style.Add("display", "none")

            'btn.Style.Add("display", "none")

            'If Not FillActivities() Then
            '    Throw New Exception("Error While Filling Activity... ddlRepeatNo_SelectedIndexChanged")
            'End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "Open({flag:'LOAD',wId:'" + Me.Request.QueryString("WorkSpaceId").ToString + "',nId:'" + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + "',pId:'" + Me.Request.QueryString("PeriodId").ToString + "',sId:'" + Me.Request.QueryString("SubjectId").ToString + "'});", True)
        Catch ex As Exception
            Me.Session(S_SelectedRepeatation + CurrentPageSessionVariable + hdnActivityName.Value) = Nothing
            objCommon.ShowAlert(ex.ToString, Me.Page())
        End Try
    End Sub

#End Region

#Region "GetAuditButtons"

    Private Function GetBrowseButton(ByVal vButtonName As String, ByVal MedExGroupCode As String,
                                     ByVal MedExSubGroupCode As String, ByVal MedExCode As String,
                                     ByVal nMedExWorkSpaceDtlNo As String, Optional ByVal DependecyMedExCodes As String = "") As Button
        Dim btn As Button
        btn = New Button
        btn.ID = "btnBrowse" & MedExGroupCode + MedExSubGroupCode + MedExCode
        btn.Text = vButtonName.Trim()
        btn.CssClass = "button"

        btn.Style.Add("display", "")

        'Condition changed for view mode -- Pratiksha
        ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
        If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_OnlyView Or Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
            btn.Style.Add("display", "none")
        End If

        If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_MedicalCoding Then
            btn.Attributes.Add("disabled", "true")
        End If

        ''Commented by nipun khant for dynamic review===============
        'If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_OnlyView Or Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
        '    btn.Style.Add("display", "none")
        'End If

        'If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_MedicalCoding Then
        '    btn.Attributes.Add("disabled", "true")
        'End If
        ''=============================================================

        btn.OnClientClick = "return MeddraBrowser('" + MedExCode + "','" + nMedExWorkSpaceDtlNo + "');"

        If DependecyMedExCodes.Contains("[" + MedExCode.Substring(0, MedExCode.LastIndexOf("R")) + "]") Then
            btn.Attributes.Add("disabled", "disabled")
        End If

        GetBrowseButton = btn
    End Function

    Private Function GetAutoCalculateButton(ByVal vButtonName As String, ByVal MedExGroupCode As String,
                                            ByVal MedExSubGroupCode As String, ByVal MedExCode As String,
                                            ByVal MedExFormula As String, ByVal DecimalNo As String,
                                            Optional ByVal DependecyMedExCodes As String = "") As Button


        Dim wStr As String = String.Empty
        Dim Ds_Decimal As DataSet
        Dim eStr As String = String.Empty
        Dim btn As Button


        btn = New Button
        btn.ID = "btnAutoCalculate" & MedExGroupCode + MedExSubGroupCode + MedExCode
        btn.Text = vButtonName.Trim()
        btn.CssClass = "button"
        If DecimalNo = "" Then
            If (Convert.ToString(MedExCode) <> "") Then
                wStr = "vmedexcode='" & MedExCode.Substring(0, 5) & "'"
            End If
            If Not objHelp.GetData("medexformulamst", "*", wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                Ds_Decimal, eStr) Then
                Me.objCommon.ShowAlert("error while getting data from medexformulamst : " + eStr, Me.Page)
            End If
            If Ds_Decimal IsNot Nothing AndAlso Ds_Decimal.Tables(0).Rows.Count > 0 Then
                DecimalNo = Ds_Decimal.Tables(0).Rows(0)("idecimalno").ToString()
            End If
            '  Me.hfDecimalNo.Value = DecimalNo
        End If
        '  Me.hfDecimalNo.Value = DecimalNo
        btn.OnClientClick = "return MedExFormula('" + MedExCode + "','" + MedExFormula + "','" + DecimalNo + "');"



        If DependecyMedExCodes.Contains("[" + MedExCode.Substring(0, MedExCode.LastIndexOf("R")) + "]") Then
            btn.Attributes.Add("disabled", "disabled")
        End If

        GetAutoCalculateButton = btn
    End Function

    Private Function GetEditButton(ByVal vButtonName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal CRFDtlNo As String, ByVal MedexType As String) As ImageButton
        Dim btn As ImageButton
        btn = New ImageButton
        btn.ID = "btnEdit" & MedExGroupCode + MedExSubGroupCode + MedExCode
        btn.ToolTip = vButtonName.Trim()

        ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
        If Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() <> WorkFlowStageId_FirstReview AndAlso
            Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() <> WorkFlowStageId_DataEntry AndAlso
            Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() <> WorkFlowStageId_MedicalCoding AndAlso
            Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() <> WorkFlowStageId_DataValidator Then
            'btn.Attributes.Add("disabled", "true")
        End If
        ''Commented by nipun khant for dynamic review
        'If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_FirstReview AndAlso _
        '            Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_DataEntry AndAlso _
        '            Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_MedicalCoding AndAlso _
        '            Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> WorkFlowStageId_DataValidator Then
        '    btn.Attributes.Add("disabled", "true")
        'End If

        btn.OnClientClick = "return AuditDivShowHide('E','" + MedExCode + "','" + MedExGroupCode + MedExSubGroupCode + MedExCode + "','" + CRFDtlNo + "','" + MedexType + "');"
        btn.SkinID = "ImgBtnEdit"

        GetEditButton = btn
    End Function

    Private Function GetUpdateButton(ByVal vButtonName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal CRFDtlNo As String, ByVal MedexType As String) As ImageButton
        Dim btn As ImageButton
        btn = New ImageButton
        btn.ID = "btnUpdate" & MedExGroupCode + MedExSubGroupCode + MedExCode
        btn.ToolTip = vButtonName.Trim()
        btn.Attributes.Add("disabled", "true")
        'btn.Style.Add("display", "none")
        btn.OnClientClick = "return AuditDivShowHide('U','" + MedExCode + "','" + MedExGroupCode + MedExSubGroupCode + MedExCode + "','" + CRFDtlNo + "','" + MedexType + "');"
        btn.SkinID = "ImgBtnSave"

        GetUpdateButton = btn
    End Function

    Private Function GetDCFButton(ByVal vButtonName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal CRFDtlNo As String, ByVal TranNo As Integer, ByVal DCFStatus As String) As ImageButton
        Try
            If Not Me.HFCRFDtlLockStatus.Value Is Nothing AndAlso
                           Convert.ToString(Me.HFCRFDtlLockStatus.Value).Trim.ToUpper() <> CRFHdr_Locked Then
                Dim btn As ImageButton
                btn = New ImageButton
                btn.ID = "btnForDCF" & MedExGroupCode + MedExSubGroupCode + MedExCode
                btn.ToolTip = vButtonName.Trim()
                btn.OnClientClick = "return AuditDivShowHide('D','" + MedExCode + "','','" + CRFDtlNo + "');"
                btn.SkinID = "ImgBtnDCF"

                If TranNo > 0 AndAlso (DCFStatus = Discrepancy_Generated Or DCFStatus = Discrepancy_Answered) Then
                    btn.SkinID = "ImgBtnDCFUpdated"
                End If
                GetDCFButton = btn
            End If
        Catch ex As Exception
        End Try
    End Function

    Private Function GetSaveRunTimeButton(ByVal vButtonName As String) As Button
        Dim btn As Button
        btn = New Button
        btn.ID = "btnSaveRunTime" & vButtonName.ToString()
        btn.ToolTip = "Save"
        btn.Text = "Save"
        btn.OnClientClick = "return  ('S','','','0');"
        btn.CssClass = "button"
        GetSaveRunTimeButton = btn
    End Function

    Private Function GetAudittrailButton(ByVal vButtonName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal TranNo As Integer, ByVal CRFDtlNo As String, ByVal Remarks As String) As ImageButton
        Dim btn As ImageButton
        btn = New ImageButton
        btn.ID = "btnAudittrail" & MedExGroupCode + MedExSubGroupCode + MedExCode
        btn.ToolTip = vButtonName.Trim()

        btn.OnClientClick = "return HistoryDivShowHide('A','" + MedExCode + "','','','" + CRFDtlNo + "');"
        btn.SkinID = "ImgBtnAuditTrail"

        If TranNo > 1 AndAlso Remarks.Length > 0 Then
            btn.SkinID = "ImgBtnAuditTrailUpdated"
        End If
        GetAudittrailButton = btn
    End Function
    '#################################################### For Edit Check Query Added By Vivek Patel '####################################################'
    'Private Function GetEditCheckQueryButton(ByVal vButtonName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal R As String, ByVal EditCheckCount As String) As ImageButton
    Private Function GetEditCheckQueryButton(ByVal vButtonName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal R As String, ByVal RepetationNo As String, ByVal VK As String, ByVal EditCheckCount As String) As ImageButton
        Dim btn As ImageButton
        btn = New ImageButton
        btn.ID = "btnEditCheckQuery" & MedExGroupCode + MedExSubGroupCode + MedExCode + R + RepetationNo + VK
        btn.ToolTip = vButtonName.Trim() + " : (" + EditCheckCount + ")"
        btn.OnClientClick = "return EditCheckQuery('A','" + MedExCode + "','" + RepetationNo + "');"
        btn.SkinID = "ImgBtnEditCheckQuery"
        GetEditCheckQueryButton = btn
        ''ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "EditcheckQueryUI", "EditcheckQueryUI(); ", True)
    End Function
    '#####################################################################################################################################################'

    ''Enhancement in EDC
    Private Function GetAutoDateButton(ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String,
                                       ByVal MedExCode As String, ByVal Repeatation As Integer,
                                       Optional ByVal TargetMedExCode As String = "", Optional ByVal DependecyMedExCodes As String = "",
                                       Optional ByVal DependentMedExValue As String = "") As Button
        Dim btn As Button
        btn = New Button
        btn.ID = "btnCurrDate" & MedExGroupCode + MedExSubGroupCode + MedExCode
        btn.ToolTip = "Current Date"
        btn.CssClass = "button"
        btn.Width = "45"

        Me.hdnServerDateTime.Value = Session(S_TimeZoneName) '' Added by Aaditya on 16-Nov-2016
        btn.OnClientClick = "return GetCurrentDate('" & MedExCode + "R" + Convert.ToString(Repeatation) & "','" & TargetMedExCode & "');" '('E','" + MedExCode + "','" + MedExGroupCode + MedExSubGroupCode + MedExCode + "','" + CRFDtlNo + "');"
        'btn.SkinID = "ImgBtnEdit"
        btn.Text = "Auto"

        If DependecyMedExCodes.Contains("[" + MedExCode + "]") Then
            btn.Attributes.Add("disabled", "disabled")
        End If

        GetAutoDateButton = btn
    End Function

    Private Function GetAutoTimeButton(ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String,
                                       ByVal MedExCode As String, ByVal Repeatation As Integer,
                                       Optional ByVal TargetMedExCode As String = "", Optional ByVal DependecyMedExCodes As String = "",
                                       Optional ByVal DependentMedExValue As String = "") As Button
        Dim btn As Button
        btn = New Button
        btn.ID = "btnCurrTime" & MedExGroupCode + MedExSubGroupCode + MedExCode
        btn.ToolTip = "Current Time"
        btn.CssClass = "button"
        btn.Width = "45"

        Me.HFServerTime.Value = Session(S_TimeZoneName) '' Added by Dipen Shah on 2-Dec-2014
        btn.OnClientClick = "return GetCurrentTime('" & MedExCode + "R" + Convert.ToString(Repeatation) & "','" & TargetMedExCode & "');" '('E','" + MedExCode + "','" + MedExGroupCode + MedExSubGroupCode + MedExCode + "','" + CRFDtlNo + "');"
        'btn.SkinID = "ImgBtnEdit"
        btn.Text = "Auto"

        If DependecyMedExCodes.Contains("[" + MedExCode + "]") Then
            btn.Attributes.Add("disabled", "disabled")
        End If

        GetAutoTimeButton = btn
    End Function

    Private Function GetAutoStandardDateButton(ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String,
                                     ByVal MedExCode As String, ByVal Repeatation As Integer,
                                     Optional ByVal TargetMedExCode As String = "", Optional ByVal DependecyMedExCodes As String = "",
                                     Optional ByVal DependentMedExValue As String = "") As Button
        Dim btn As Button
        btn = New Button
        btn.ID = "btnCurrDate" & MedExGroupCode + MedExSubGroupCode + MedExCode
        btn.ToolTip = "Current Date"
        btn.CssClass = "button"
        btn.Width = "45"

        Me.hdnServerDateTime.Value = Session(S_TimeZoneName)
        btn.OnClientClick = "return GetCurrentStandardDate('" & MedExCode + "R" + Convert.ToString(Repeatation) & "','" & TargetMedExCode & "');"
        btn.Text = "Auto"

        If DependecyMedExCodes.Contains("[" + MedExCode + "]") Then
            btn.Attributes.Add("disabled", "disabled")
        End If

        GetAutoStandardDateButton = btn

    End Function

    Private Function GetClearButton(ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String,
                                    ByVal MedExCode As String, ByVal Repeatation As Integer,
                                    Optional ByVal TargetMedExCode As String = "", Optional ByVal DependecyMedExCodes As String = "",
                                    Optional ByVal DependentMedExValue As String = "") As Button
        Dim btn As Button
        btn = New Button
        btn.ID = "btnClear" & MedExGroupCode + MedExSubGroupCode + MedExCode
        btn.ToolTip = "Clear"
        btn.CssClass = "button"
        btn.Width = "45"


        btn.OnClientClick = "return ClearVal('" & MedExCode + "R" + Convert.ToString(Repeatation) & "','" & TargetMedExCode & "');" '('E','" + MedExCode + "','" + MedExGroupCode + MedExSubGroupCode + MedExCode + "','" + CRFDtlNo + "');"
        'btn.SkinID = "ImgBtnEdit"
        btn.Text = "Clear"

        If DependecyMedExCodes.Contains("[" + MedExCode + "]") Then
            btn.Attributes.Add("disabled", "disabled")
        End If

        GetClearButton = btn
    End Function
    Private Function GetClearStandardDateButton(ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String,
                                 ByVal MedExCode As String, ByVal Repeatation As Integer,
                                 Optional ByVal TargetMedExCode As String = "", Optional ByVal DependecyMedExCodes As String = "",
                                 Optional ByVal DependentMedExValue As String = "") As Button
        Dim btn As Button
        btn = New Button
        btn.ID = "btnClear" & MedExGroupCode + MedExSubGroupCode + MedExCode
        btn.ToolTip = "Clear"
        btn.CssClass = "button"
        btn.Width = "45"


        btn.OnClientClick = "return ClearStandardDateVal('" & MedExCode + "R" + Convert.ToString(Repeatation) & "','" & TargetMedExCode & "');" '('E','" + MedExCode + "','" + MedExGroupCode + MedExSubGroupCode + MedExCode + "','" + CRFDtlNo + "');"
        'btn.SkinID = "ImgBtnEdit"
        btn.Text = "Clear"

        If DependecyMedExCodes.Contains("[" + MedExCode + "]") Then
            btn.Attributes.Add("disabled", "disabled")
        End If

        GetClearStandardDateButton = btn
    End Function
    Private Function GetCountLable(ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String,
                                       ByVal MedExCode As String) As Label
        Dim lbl As Label
        lbl = New Label
        lbl.ID = "lblCntText" & MedExGroupCode + MedExSubGroupCode + MedExCode
        lbl.CssClass = "CntTextArea notsaved"
        lbl.Text = "3000 characters remaining"
        lbl.Style.Add("font-size", "10px")

        GetCountLable = lbl

    End Function
    Private Function GetDeleteRepetitionButton(ByVal vButtonName As String) As ImageButton
        Dim btn As ImageButton
        btn = New ImageButton
        btn.ID = "btnDeleteRepetition" + vButtonName.ToString
        btn.ToolTip = IIf(ddlRepeatNo.Items.Count = 2, "Delete", "Delete Repetition")
        btn.OnClientClick = "return DeleteRepetition('" + vButtonName.ToString + "');"
        btn.SkinID = "ImgBtnDelete"

        GetDeleteRepetitionButton = btn
    End Function

    '''''''''''''''''''''''''''''''

#End Region

#Region "Buttons Click Events"

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim estr As String = String.Empty
        Dim SubjectId As String = String.Empty
        Dim WorkspaceId As String = String.Empty
        Dim ParentWorkSpaceId As String = String.Empty
        Dim ActivityId As String = String.Empty
        Dim NodeId As String = String.Empty
        Dim PeriodId As String = String.Empty
        Dim MySubjectNo As String = String.Empty
        Dim DsSave As New DataSet

        Dim Dir As DirectoryInfo
        Dim Flinfo As FileInfo
        Dim TranNo_Retu As String = String.Empty
        Dim FolderPath As String = String.Empty
        Dim Reviewstatus As String
        Dim objCollection As ControlCollection
        Dim objControl As Control
        Dim ObjId As String = String.Empty
        Dim StrRedirect As String = String.Empty
        Dim pwd As String = String.Empty

        Try
            Me.ViewState(VS_DataStatus) = CRF_DataEntryCompleted

            pwd = objHelp.EncryptPassword(txtPasswords.Text.ToString())
            If Convert.ToString(Me.Session(S_UserType)) = "0120" OrElse Convert.ToString(Me.Session(S_UserType)) = "0121" OrElse Convert.ToString(Me.Session(S_UserType)) = "0122" Then
                If Convert.ToString(Me.Session(S_Password)) <> pwd.ToString() Then
                    lblSignAuthUserName.Text = lblUserName.Text
                    'txtPasswords.Focus()
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "tblDisabled", "tblDisabled('tblDigitalFP');", True)
                    lblSignAuthDateTime.Text = CDate(DateTime.Now).ToString("dd-MMM-yyyy hh:mm tt")
                    Me.myModalSignAuth.Style.Clear()
                    Me.myModalSignAuth.Style.Add("display", "Block")
                    'myModalSignAuth.Visible = True
                    Exit Sub
                    'objCommon.ShowAlert("Password Authentication Fails !", Me.Page)
                Else
                    Me.txtPasswords.Focus()
                End If
            End If

            SubjectId = Me.HFSubjectId.Value.Trim()
            WorkspaceId = Me.HFWorkspaceId.Value.Trim()
            ActivityId = Me.HFActivityId.Value.Trim()
            NodeId = Me.HFNodeId.Value.Trim()
            PeriodId = Me.HFPeriodId.Value.Trim()
            MySubjectNo = Me.HFMySubjectNo.Value.Trim()

            If Me.Session(S_ScopeNo).ToString <> Scope_ClinicalTrial AndAlso
                Me.txtContent.Text.Trim.ToString <> "" Then

                If Not AssignValuesForSequenceDeviation(DsSave) Then
                    Me.objCommon.ShowAlert("Error While Assigning Data.", Me.Page)
                    Exit Sub
                End If

                If Not objLambda.Insert_WorkspaceActivitySequenceDeviation(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add,
                                                                       DsSave, Me.Session(S_UserID), estr) Then
                    Me.objCommon.ShowAlert("Error While inserting Data.", Me.Page)
                    Exit Sub
                End If
            End If

            If Not AssignValues(SubjectId, WorkspaceId, ActivityId, NodeId, PeriodId, MySubjectNo, DsSave) Then
                Me.objCommon.ShowAlert("Error While Assigning Data.", Me.Page)
                Exit Sub
            End If

            If Not Me.objLambda.Save_CRFHdrDtlSubDtl(Me.ViewState(VS_Choice), DsSave, False, Me.Session(S_UserID), estr) Then
                Me.objCommon.ShowAlert(estr, Me.Page)
                Exit Sub
            Else

                objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection) 'Me.PlaceMedEx.Controls 
                For Each objControl In objCollection
                    If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.FileUpload" Then
                        Dim filename As String = ""

                        If objControl.ID.ToString.Contains("FU") Then
                            ObjId = objControl.ID.ToString.Replace("FU", "")
                        Else
                            ObjId = objControl.ID.ToString.Trim()
                        End If
                        If CType(FindControl(objControl.ID), FileUpload).FileName = "" And
                             Not IsNothing(CType(FindControl("hlnk" + ObjId.Substring(0, ObjId.IndexOf("R")).Trim()), HyperLink)) AndAlso
                             CType(FindControl("hlnk" + ObjId.Substring(0, ObjId.IndexOf("R")).Trim()), HyperLink).NavigateUrl <> "" Then

                            filename = Server.MapPath(CType(FindControl("hlnk" + ObjId.Substring(0, ObjId.IndexOf("R")).Trim()), HyperLink).NavigateUrl)

                            FolderPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail"))
                            FolderPath += WorkspaceId + "/" + NodeId + "/" + SubjectId + "/" + TranNo_Retu + "/"

                            Dir = New DirectoryInfo(FolderPath)
                            If Not Dir.Exists() Then
                                Dir.Create()
                            End If

                            Flinfo = New FileInfo(filename.Trim())
                            'Flinfo.CopyTo(FolderPath + Flinfo.Name)

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
            If DsSave.Tables("CRFSubDtl").Rows.Count > 0 Then
                Me.HFCRFDtlNo.Value = DsSave.Tables("CRFSubDtl").Rows(0).Item("nCRFDtlNo")
            End If

            If Not FireDynamicEditChecks() Then
                Throw New Exception("Error While FiredDyanmic Edit Checks...btnSave")
            End If
            Me.EditChecks()
            Me.objCommon.ShowAlert("Attributes Saved Successfully.", Me.Page)
            Me.Session.Remove("PlaceMedEx")
            Me.HFSessionFlg.Value = "1"
            'Me.ddlActivities_SelectedIndexChanged(sender, e)

            Dim vOperationCode As String = ""
            If Convert.ToString(ConfigurationManager.AppSettings("QCUserCode")).Contains(ViewState(S_UserType).ToString()) Then
                vOperationCode = "2"
            ElseIf Convert.ToString(ConfigurationManager.AppSettings("QC2UserCode")).Contains(ViewState(S_UserType).ToString()) Then
                vOperationCode = "3"
            ElseIf Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(ViewState(S_UserType).ToString()) Then
                vOperationCode = "4"
            Else
                vOperationCode = ""
            End If
            'Dim SetNo As String = Convert.ToString(ds_WorkspaceNodeDetail.Tables(0).Rows(0)("vNodeDisplayName")).Trim()
            Dim Site_No As String = ""
            Dim StudyProtocol As String = String.Empty
            Dim EmailSubject As String = String.Empty
            Dim SendEmail As Boolean = True
            Dim NodeDisplayName As String = String.Empty
            Dim PationtInitial As String = String.Empty
            Dim vMySubjectNo As String = String.Empty
            Dim vRandomizationNo As String = String.Empty
            Dim Status As String = ""
            Dim ds_GetselectedChild As DataSet = New DataSet
            Dim Params As String = String.Empty
            Dim dsSpineDetail As DataSet = Nothing
            Dim dsProximalDetail As New DataSet
            Dim Remarks As String = txtQCRemarks.Value.ToString()
            ParentWorkSpaceId = Me.HFParentWorkspaceId.Value.Trim()
            Params = HFWorkspaceId.Value.ToString() + "##" + HFSubjectId.Value.ToString() + "##" +
                         HFParentActivityId.Value.ToString() + "##" + HFParentNodeId.Value.ToString() + "##" + ""
            ds_GetselectedChild = objHelp.ProcedureExecute("dbo.Pro_GetImgTransmittalDtlForQCReview", Params)

            If ds_GetselectedChild.Tables(0).Rows.Count > 0 Then
                Site_No = Convert.ToString(ds_GetselectedChild.Tables(0).Rows(0).Item("SiteNo"))
                StudyProtocol = Convert.ToString(ds_GetselectedChild.Tables(0).Rows(0).Item("ProtocolNo"))
                Try
                    PationtInitial = Convert.ToString(ds_GetselectedChild.Tables(0).Rows(0).Item("vInitials"))
                    NodeDisplayName = Convert.ToString(ds_GetselectedChild.Tables(0).Rows(0).Item("vNodeDisplayName"))
                    vMySubjectNo = Convert.ToString(ds_GetselectedChild.Tables(0).Rows(0).Item("vMySubjectNo"))
                    vRandomizationNo = Convert.ToString(ds_GetselectedChild.Tables(0).Rows(0).Item("vRandomizationNo"))
                Catch ex As Exception
                End Try
            End If

            'If NodeDisplayName = "ELIGIBILITY-REVIEW" Then
            '    If vOperationCode = "2" Then
            '        EmailSubject = "DiSoftC - QC1 Review for" + Me.ddlActivities.SelectedItem.Text + " for Screening No - " + PationtInitial + "/" + vMySubjectNo
            '        If Not Me.ddlActivities.SelectedItem.Text.Trim().ToUpper().Contains("FINAL") Then
            '            SendEmail = False
            '        End If
            '        If Status.ToLower() = "rejected" OrElse Status = "Send Back" Then
            '            SendEmail = True
            '        End If
            '    ElseIf vOperationCode = "3" Then
            '        EmailSubject = "DiSoftC - QC2 Review for" + Me.ddlActivities.SelectedItem.Text + " for Screening No - " + PationtInitial + "/" + vMySubjectNo
            '        If Not Me.ddlActivities.SelectedItem.Text.Trim().ToUpper().Contains("FINAL") Then
            '            SendEmail = False
            '        End If
            '        If Status.ToLower() = "rejected" OrElse Status = "Send Back" Then
            '            SendEmail = True
            '        End If
            '    ElseIf vOperationCode = "4" Then
            '        EmailSubject = "DiSoftC - CA1 Review for " + Me.ddlActivities.SelectedItem.Text + " for Screening No - " + PationtInitial + "/" + vMySubjectNo
            '        If Not Me.ddlActivities.SelectedItem.Text.Trim().ToUpper().Contains("FINAL") Then
            '            SendEmail = False
            '        End If
            '        If Status.ToLower() = "rejected" OrElse Status = "Send Back" Then
            '            SendEmail = True
            '        End If
            '    Else
            '        If vOperationCode = "2" Then
            '            EmailSubject = "DiSoftC - QC1 Review for - " + Me.ddlActivities.SelectedItem.Text + " for Screening No/Randomization No - " + vMySubjectNo + "/" + vRandomizationNo
            '            If Not Me.ddlActivities.SelectedItem.Text.Trim().ToUpper().Contains("FINAL") Then
            '                SendEmail = False
            '            End If
            '            If Status.ToLower() = "rejected" OrElse Status = "Send Back" Then
            '                SendEmail = True
            '            End If
            '        ElseIf vOperationCode = "3" Then
            '            EmailSubject = "DiSoftC - QC2 Review for " + Me.ddlActivities.SelectedItem.Text + " for Screening No/Randomization No - " + vMySubjectNo + "/" + vRandomizationNo
            '            If Not Me.ddlActivities.SelectedItem.Text.Trim().ToUpper().Contains("FINAL") Then
            '                SendEmail = False
            '            End If
            '            If Status.ToLower() = "rejected" OrElse Status = "Send Back" Then
            '                SendEmail = True
            '            End If
            '        ElseIf vOperationCode = "4" Then
            '            EmailSubject = "DiSoftC - CA1 Review for " + Me.ddlActivities.SelectedItem.Text + " for Screening No/Randomization No - " + vMySubjectNo + "/" + vRandomizationNo
            '            If Not Me.ddlActivities.SelectedItem.Text.Trim().ToUpper().Contains("FINAL") Then
            '                SendEmail = False
            '            End If
            '            If Status.ToLower() = "rejected" OrElse Status = "Send Back" Then
            '                SendEmail = True
            '            End If
            '        End If
            '    End If
            'End If

            'Commented by Bhargav Thaker 16March2023
            'If vOperationCode = "2" Then
            '    EmailSubject = "DiSoftC - " + Me.ddlActivities.SelectedItem.Text + " for Screening - " + vMySubjectNo
            'ElseIf vOperationCode = "3" Then
            '    EmailSubject = "DiSoftC - " + Me.ddlActivities.SelectedItem.Text + " for Screening - " + vMySubjectNo
            'ElseIf vOperationCode = "4" Then
            '    EmailSubject = "DiSoftC - " + Me.ddlActivities.SelectedItem.Text + " for Screening - " + vMySubjectNo
            'End If
            'Modify by Bhargav Thaker 16March2023
            If vOperationCode = "2" Then
                EmailSubject = "DiSoftC - " + Me.ddlActivities.SelectedItem.Text + " for " + NodeDisplayName.ToString().Trim() + " - " + vMySubjectNo
            ElseIf vOperationCode = "3" Then
                EmailSubject = "DiSoftC - " + Me.ddlActivities.SelectedItem.Text + " for " + NodeDisplayName.ToString().Trim() + " - " + vMySubjectNo
            ElseIf vOperationCode = "4" Then
                EmailSubject = "DiSoftC - " + Me.ddlActivities.SelectedItem.Text + " for " + NodeDisplayName.ToString().Trim() + " - " + vMySubjectNo
            End If

            If vRandomizationNo <> "" Then
                EmailSubject += "/" + vRandomizationNo
            End If

            For Each dr1 As DataRow In ds_GetselectedChild.Tables(0).Rows
                If Convert.ToString(ConfigurationManager.AppSettings("QCUserCode")).Contains(Me.hdnuserprofile.Value) Then
                    dr1("cQCStatus") = IIf(String.IsNullOrEmpty(rblApprovalStatus.SelectedValue.ToString()), "D", rblApprovalStatus.SelectedValue.ToString())
                    'If String.IsNullOrEmpty(SetNo) Then
                    dr1("vStatusReasonQC") = Convert.ToString(txtQCRemarks.Value.Trim())
                    'End If
                ElseIf Convert.ToString(ConfigurationManager.AppSettings("QC2UserCode")).Contains(Me.hdnuserprofile.Value) Then
                    dr1("cQC2Status") = IIf(String.IsNullOrEmpty(rblApprovalStatus.SelectedValue.ToString()), "D", rblApprovalStatus.SelectedValue.ToString())
                    'If String.IsNullOrEmpty(SetNo) Then
                    dr1("vStatusReasonQC2") = Convert.ToString(txtQCRemarks.Value.Trim())
                    'End If
                ElseIf Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(Me.hdnuserprofile.Value) Then
                    dr1("cCA1Status") = IIf(String.IsNullOrEmpty(rblApprovalStatus.SelectedValue.ToString()), "D", rblApprovalStatus.SelectedValue.ToString())
                    'If String.IsNullOrEmpty(SetNo) Then
                    dr1("vStatusReasonCA1") = Convert.ToString(txtQCRemarks.Value.Trim())
                    'End If
                End If
            Next
            If SendEmail = True AndAlso ds_GetselectedChild.Tables(0).Rows.Count > 0 Then
                SendEmail = objCommon.SendEmail(ds_GetselectedChild, Status, WorkspaceId, ParentWorkSpaceId, vOperationCode, StudyProtocol, Site_No, EmailSubject, Remarks,
                                        Me.Session(S_UserNameWithProfile), DateTime.Now().ToString, "", NodeDisplayName.ToString(), Session(S_UserID), dsSpineDetail, dsProximalDetail)
            End If
            If IsNothing(Me.Request.QueryString("From")) Then
                Me.ddlActivities_SelectedIndexChanged(sender, e)
            Else

                If Not Getstring(StrRedirect) Then
                    Me.objCommon.ShowAlert("Error occured while getting string.", Me.Page)
                    Exit Sub
                End If
                Me.Response.Redirect(StrRedirect, False)

            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "Open({flag:'LOAD',wId:'" + Me.Request.QueryString("WorkSpaceId").ToString + "',nId:'" + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + "',pId:'" + Me.Request.QueryString("PeriodId").ToString + "',sId:'" + Me.Request.QueryString("SubjectId").ToString + "'});", True)

            ' If Convert.ToString(Me.Session(S_UserType)) = "0120" OrElse Convert.ToString(Me.Session(S_UserType)) = "0121" Then
            'Reviewstatus = IIf(rblApprovalStatus.SelectedValue.ToString() <> "A", "R", "")

            Reviewstatus = rblApprovalStatus.SelectedValue.ToString()
            If Not SaveImageHdrdetailValues(Reviewstatus) Then
                Me.objCommon.ShowAlert("Error While Assigning Data.", Me.Page)
            End If

            Me.Session(S_SelectedRepeatation + CurrentPageSessionVariable + hdnActivityName.Value) = ""
            'End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "......BtnSave_Click")
        End Try
    End Sub

    Protected Sub btnSignAuthOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSignAuthOK.Click
        Dim Ds_Subjectmst As New DataSet
        Dim eStr As String = String.Empty
        Dim Msg As String = String.Empty

        Try
            If Auntheticate(Me.txtPasswords.Text) Then
                Me.myModalSignAuth.Style.Remove("display")
                Me.myModalSignAuth.Attributes.Add("display", "None")
                BtnSave_Click(sender, e)
                'Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert(11)", True)
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "myModalSignAuth", "SignAuthModalClose(); ", True)
                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "KitDataSave", "KitDataSave('" + Me.Session(S_UserID) + "');", True)
                'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "ShowAttSubmit", "ShowAttSubmit('" & "Success" & "',this);", True)

            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "..........BtnSaveSubjectMst_Click")
        End Try

    End Sub

    Protected Sub btnSaveAndContinue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAndContinue.Click
        Dim estr As String = String.Empty
        Dim SubjectId As String = String.Empty
        Dim RedirectStr As String = String.Empty
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
        Dim StrRedirect As String = String.Empty

        Try

            Me.ViewState(VS_DataStatus) = CRF_DataEntry

            SubjectId = Me.HFSubjectId.Value.Trim()
            WorkspaceId = Me.HFWorkspaceId.Value.Trim()
            ActivityId = Me.HFActivityId.Value.Trim()
            NodeId = Me.HFNodeId.Value.Trim()
            PeriodId = Me.HFPeriodId.Value.Trim()
            MySubjectNo = Me.HFMySubjectNo.Value.Trim()

            If Me.Session(S_ScopeNo).ToString <> Scope_ClinicalTrial AndAlso
               Me.txtContent.Text.Trim.ToString <> "" Then

                If Not AssignValuesForSequenceDeviation(DsSave) Then
                    Me.objCommon.ShowAlert("Error While Assigning Data.", Me.Page)
                    Exit Sub
                End If

                If Not objLambda.Insert_WorkspaceActivitySequenceDeviation(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add,
                                                                       DsSave, Me.Session(S_UserID), estr) Then
                    Me.objCommon.ShowAlert("Error While inserting Data.", Me.Page)
                    Exit Sub
                End If
            End If

            If Not AssignValuesForSaveAndContinue(SubjectId, WorkspaceId, ActivityId, NodeId, PeriodId, MySubjectNo, DsSave) Then
                Me.objCommon.ShowAlert("Error While Assigning Data.", Me.Page)
                Exit Sub
            End If

            If Not Me.objLambda.Save_CRFHdrDtlSubDtl(Me.ViewState(VS_Choice), DsSave, False, Me.Session(S_UserID), estr) Then
                Me.objCommon.ShowAlert(estr, Me.Page)
                Exit Sub
            Else

                objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection) 'Me.PlaceMedEx.Controls 
                For Each objControl In objCollection
                    If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.FileUpload" Then
                        Dim filename As String = ""

                        If objControl.ID.ToString.Contains("FU") Then
                            ObjId = objControl.ID.ToString.Replace("FU", "")
                        Else
                            ObjId = objControl.ID.ToString.Trim()
                        End If
                        ''modify by ketan 
                        If CType(FindControl(objControl.ID), FileUpload).FileName = "" And
                             Not IsNothing(CType(FindControl("hlnk" + ObjId.Substring(0, ObjId.IndexOf("R")).Trim()), HyperLink)) AndAlso
                             CType(FindControl("hlnk" + ObjId.Substring(0, ObjId.IndexOf("R")).Trim()), HyperLink).NavigateUrl <> "" Then

                            filename = Server.MapPath(CType(FindControl("hlnk" + ObjId.Substring(0, ObjId.IndexOf("R")).Trim()), HyperLink).NavigateUrl)

                            FolderPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail"))
                            FolderPath += WorkspaceId + "/" + NodeId + "/" + SubjectId + "/" + TranNo_Retu + "/"

                            Dir = New DirectoryInfo(FolderPath)
                            If Not Dir.Exists() Then
                                Dir.Create()
                            End If

                            Flinfo = New FileInfo(filename.Trim())
                            'Flinfo.CopyTo(FolderPath + Flinfo.Name)

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



            Me.Session.Remove("PlaceMedEx")
            Me.HFSessionFlg.Value = "1"
            If Me.Request.QueryString("From") Is Nothing Then
                Me.objCommon.ShowAlert("Attributes Saved Successfully.", Me.Page)
                Me.ddlActivities_SelectedIndexChanged(sender, e)
            Else
                If Not Session(S_DynamicPage_URL) Is Nothing AndAlso Session(S_DynamicPage_URL) <> "" Then
                    Me.ddlActivities_SelectedIndexChanged(sender, e)

                End If

                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ParentWindow", "ParentButton();", True)
            End If


            'If Not IsNothing(Me.Request.QueryString("From")) AndAlso Convert.ToString(Me.Request.QueryString("From")).Trim().ToUpper = "SCH" Then

            '    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "Release", "closewindow('D',$get('btnSaveAndContinue'));", True)
            'End If


        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "...btnSaveAndContinue_Click")
        End Try
    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "AnyDivShowHide('S');", True)
        'Add by shivani pandya for All Repeatition
        If Me.Session(S_GetLetestData + CurrentPageSessionVariable + hdnActivityName.Value) <> "getData" Then
            Me.Session(S_GetLetestData + CurrentPageSessionVariable + hdnActivityName.Value) = ""
        End If
        Me.divForEditAttribute.Style.Add("display", "")
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "UnBindClick", "$('#btnEdit').unbind('click');", True)
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "displayBackGround", "displayBackGround();", True)
        Me.Session(S_SelectedRepeatation + CurrentPageSessionVariable + hdnActivityName.Value) = Nothing
    End Sub

    Protected Sub btnDCF_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDCF.Click
        If Not Me.fillDCFGrid() Then
            Exit Sub
        End If
        'Add by shivani pandya
        If Me.Session(S_GetLetestData + CurrentPageSessionVariable + hdnActivityName.Value) <> "getData" Then
            Me.Session(S_GetLetestData + CurrentPageSessionVariable + hdnActivityName.Value) = ""
        End If
        flagDCF = True
        'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "AnyDivShowHide('DCFSHOW');", True)
        'Me.MpeDCF.Show()       
        '  Me.divDCF.Style.Add("display", "")
        Me.divDCF.Style.Remove("display")
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "getContentData", " getContentData();", True)
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "UnBindClick", "$('#btnDCF').unbind('click');", True)
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "displayBackGround", "displayBackGround();", True)

    End Sub

    Protected Sub btnSaveDiscrepancy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDiscrepancy.Click
        Dim ds_DCF As New DataSet
        Dim drDCF As DataRow
        Dim controlid As String = String.Empty
        Dim ObjControl As Control
        Dim discrepancy As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_CRFDtl As New DataSet
        Dim wStr As String = String.Empty
        Dim oblControlType As String = String.Empty
        Dim dsDCF As New DataSet
        Try


            Me.btnSaveDiscrepancy.Enabled = False
            ds_DCF.Tables.Add(CType(Me.ViewState(VS_DtDCF), DataTable).Copy())

            'nDCFNo,nCRFSubDtlNo,cDCFType,iDCFBy,dDCFDate,vDiscrepancy,vSourceResponse
            'cDCFStatus,iStatusChangedBy,dStatusChangedOn,iModifyBy,

            drDCF = ds_DCF.Tables(0).NewRow()
            'controlid = Me.hfMedexCode.Value.Trim()
            controlid = Me.hfMedexCode.Value.Substring(0, Me.hfMedexCode.Value.IndexOf("R"))
            Dim MedExtype = HFMedexType.Value
            If Not Me.GetControlValue(discrepancy, controlid, ObjControl, oblControlType) Then
                Me.objCommon.ShowAlert("Problem While Editing Attribute Detail", Me.Page)
                Exit Sub
            End If

            If Me.HFCRFHdrNo.Value.Trim() = "" OrElse Me.ddlRepeatNo.SelectedItem.Value.Trim.ToUpper() = "N" Then
                Me.objCommon.ShowAlert("CRFHdrNo Not Available", Me.Page())
                Exit Sub
            End If

            wStr = "nCRFHdrNo = " + Me.HFCRFHdrNo.Value.Trim() + " And vSubjectId = '" + Me.HFSubjectId.Value.Trim() +
                    "' And iRepeatNo = " + Me.hfMedexCode.Value.Substring(Me.hfMedexCode.Value.IndexOf("R") + 1) + " And cStatusIndi <> 'D'"

            If Not Me.objHelp.GetCRFDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                        ds_CRFDtl, eStr) Then
                Throw New Exception("Error While Getting Data From CRFDtl. " + eStr)
            End If

            If ds_CRFDtl Is Nothing Then
                Throw New Exception("CRFDtl No. Not found.")
            End If

            If ds_CRFDtl.Tables(0).Rows.Count < 0 Then
                Throw New Exception("CRFDtl No. Not Available.")
            End If

            wStr = " nCRFDtlNo =  " + ds_CRFDtl.Tables(0).Rows(0).Item("nCRFDtlNo").ToString + " and vMedExCode = '" + Me.hfMedexCode.Value.Substring(0, Me.hfMedexCode.Value.IndexOf("R")) + "' and cDCFStatus = 'N' "
            If Not objHelp.GetDCFMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsDCF, eStr) Then
                Throw New Exception("Error While Getting Data From DCFMst. " + eStr)
            End If

            If dsDCF.Tables(0).Rows.Count > 0 Then
                objCommon.ShowAlert("Discrepancy is already generated on this field.", Me)
                Exit Sub
            End If

            drDCF("nDCFNo") = 0
            drDCF("nCRFDtlNo") = ds_CRFDtl.Tables(0).Rows(0).Item("nCRFDtlNo")
            drDCF("iSrNo") = 0
            drDCF("vMedExcode") = Me.hfMedexCode.Value.Substring(0, Me.hfMedexCode.Value.IndexOf("R"))
            drDCF("cDCFType") = "M"
            drDCF("iDCFBy") = Me.Session(S_UserID)
            drDCF("dDCFDate") = System.DateTime.Now()
            drDCF("vDiscrepancy") = discrepancy
            drDCF("vSourceResponse") = Me.txtDiscrepancyRemarks.Text.Trim()
            drDCF("cDCFStatus") = Me.ddlDiscrepancyStatus.SelectedItem.Value.Trim()
            drDCF("iModifyBy") = Me.Session(S_UserID)
            drDCF("dModifyOn") = DateTime.Now()
            ds_DCF.Tables(0).Rows.Add(drDCF)
            ds_DCF.AcceptChanges()

            If Not Me.objLambda.Save_DCFMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add,
                                    ds_DCF, Me.Session(S_UserID), eStr) Then
                Throw New Exception(eStr)
            End If

            Me.objCommon.ShowAlert("Discrepancy Added Successfully.", Me.Page)
            Me.txtDiscrepancyRemarks.Text = ""
            If Not Me.fillDCFGrid() Then
                Exit Sub
            End If
            'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "AnyDivShowHide('DCFSHOW');", True)
            'Me.MpeDCF.Show()
            Me.divDCF.Style.Add("display", "")
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "UnBindClick", "$('#btnSaveDiscrepancy').unbind('click');", True)
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "displayBackGround", "displayBackGround();", True)

            Me.HFSessionFlg.Value = "1"
            ' Me.Session(S_SelectedRepeatation) = ddlRepeatNo.SelectedValue
            Me.ddlActivities_SelectedIndexChanged(sender, e)


            If btnGridViewDisplay.Visible = True And hndtabluler.Value = "Tabuler" Then
                Session(S_DiscrpancyStatus) = "tabuler"
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnSaveDiscrepancy_Click")
        End Try
    End Sub

    Protected Sub btnSaveRemarksForAttribute_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveRemarksForAttribute.Click
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_CRFSubDtl As New DataSet
        Dim dr As DataRow
        Dim ControlId As String
        Dim ObjControl As Control
        Dim dt_DCF As New DataTable
        Dim result_Retu As String = String.Empty
        Dim ControlDesc As String = String.Empty
        Dim ds_CRFDtl As New DataSet
        Dim oblControlType As String = String.Empty
        Dim ds_DCF As New DataSet
        Dim dsDCF As New DataSet
        Dim discrepancy As String = String.Empty
        Dim DCFNo As String = String.Empty
        Dim dsFormula As New DataSet
        Dim dsFormulaDCF As New DataSet
        Dim drFormulaDCF As DataRow
        Dim ds_CRFHdr As New DataSet

        Try
            If Not Me.objHelp.GetCRFSubDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty,
                        ds_CRFSubDtl, eStr) Then
                Throw New Exception(eStr)
            End If

            dr = ds_CRFSubDtl.Tables(0).NewRow()
            ControlId = Me.hfMedexCode.Value.Substring(0, Me.hfMedexCode.Value.IndexOf("R"))

            If Not Me.GetControlValue(result_Retu, ControlId, ObjControl, oblControlType) Then
                Me.objCommon.ShowAlert("Problem While Editing Attribute Detail", Me.Page)
                Exit Sub
            End If

            result_Retu = Server.HtmlDecode(result_Retu)  ''Added by Ketan For Handle Special Charcter

            If Me.HFCRFHdrNo.Value.Trim() = "" OrElse Me.ddlRepeatNo.SelectedItem.Value.Trim.ToUpper() = "N" Then
                Me.objCommon.ShowAlert("CRFHdrNo Not Available", Me.Page())
                Exit Sub
            End If

            wStr = "nCRFHdrNo = " + Me.HFCRFHdrNo.Value.Trim() + " And vSubjectId = '" + Me.HFSubjectId.Value.Trim() +
                    "' And iRepeatNo = " + Me.hfMedexCode.Value.Substring(Me.hfMedexCode.Value.IndexOf("R") + 1) + " And cStatusIndi <> 'D'"

            If Not Me.objHelp.GetCRFDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                        ds_CRFDtl, eStr) Then
                Throw New Exception("Error While Getting Data From CRFDtl. " + eStr)
            End If

            If ds_CRFDtl Is Nothing Then
                Throw New Exception("CRFDtl No. Not found.")
            End If

            If ds_CRFDtl.Tables(0).Rows.Count < 0 Then
                Throw New Exception("CRFDtl No. Not Available.")
            End If

            dr("nCRFSubDtlNo") = 0
            dr("nCRFDtlNo") = ds_CRFDtl.Tables(0).Rows(0).Item("nCRFDtlNo")
            dr("iTranNo") = Me.HFMedexInfoDtlTranNo.Value.Trim()
            dr("vMedExCode") = Me.hfMedexCode.Value.Substring(0, Me.hfMedexCode.Value.IndexOf("R"))
            'dr("vMedExDesc") = No need to pass, because procedure is taking care of this
            'SDNidhi
            'dr("dMedExDateTime") = System.DateTime.Now
            dr("dMedExDateTime") = CDate(objCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
            dr("vMedexResult") = result_Retu
            dr("vModificationRemark") = Me.HdReasonDesc.Value.ToString()
            dr("iModifyBy") = Me.Session(S_UserID)
            dr("cStatusIndi") = "N"
            dr("dModifyOn") = DateTime.Now()
            If CheckDiscrepancy(ObjControl, Me.hfMedexCode.Value.Trim(), dr("vMedexResult"), "S",
                                ds_CRFDtl.Tables(0).Rows(0).Item("nCRFDtlNo")) Then
                dr("cStatusIndi") = "D"
                dt_DCF = CType(Me.ViewState(VS_DtDCF), DataTable)
                dt_DCF.TableName = "DCFMst"
                ds_CRFSubDtl.Tables.Add(dt_DCF.Copy())
                ds_CRFSubDtl.AcceptChanges()
            End If

            ds_CRFSubDtl.Tables(0).Rows.Add(dr)
            ds_CRFSubDtl.Tables(0).AcceptChanges()

            wStr = "nCRFHdrNo=" & Me.HFCRFHdrNo.Value.Trim() & " And vSubjectID = '" + Me.HFSubjectId.Value.Trim() +
                    "' And vMedExCode = '" + Me.hfMedexCode.Value.Substring(0, Me.hfMedexCode.Value.IndexOf("R")) +
                    "' And iRepeatNo = " + Me.hfMedexCode.Value.Substring(Me.hfMedexCode.Value.IndexOf("R") + 1) + " And cStatusIndi <> 'D' AND cDCFStatus = 'N' "

            If Not objHelp.GetData("VIEW_DCFMst_Optimized", "*", wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DCF, eStr) Then
                Throw New Exception(eStr)
            End If

            wStr = "nCRFHdrNo=" & Me.HFCRFHdrNo.Value.Trim()
            If Not Me.objHelp.GetCRFHdr(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CRFHdr, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_CRFHdr Is Nothing Then
                Throw New Exception("CRFHdr No. Not found.")
            End If

            If ds_CRFHdr.Tables(0).Rows.Count = 0 Then
                Throw New Exception("CRFHdr No. Not Available.")
            End If

            wStr = " vWorkspaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' AND iNodeId = " + ds_CRFHdr.Tables(0).Rows(0)("iNodeId").ToString() + " ANd vMedexType='Formula' AND vMedexFormula like '%" + ControlId + "%'  and vMedexFormula is not null and vMedexFormula <> '' and cStatusIndi <> 'D'"

            If Not Me.objHelp.GetViewMedExWorkSpaceDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsFormula, eStr) Then
                Throw New Exception(eStr)
            End If

            dsFormulaDCF = Nothing
            If Not dsFormula Is Nothing Then
                If dsFormula.Tables(0).Rows.Count > 0 Then
                    dsFormulaDCF = ds_DCF.Clone()
                    dsFormulaDCF.Tables(0).Columns.Add("vModificationRemark")
                    For Each drDCF As DataRow In dsFormula.Tables(0).Rows
                        DirectCast(Me.ViewState(VS_dtMedEx_Fill), DataTable).DefaultView.RowFilter = "vMedExCode = '" + drDCF("vMedExCode").ToString() + "'"
                        If DirectCast(Me.ViewState(VS_dtMedEx_Fill), DataTable).DefaultView.ToTable.Rows.Count > 0 Then
                            drFormulaDCF = dsFormulaDCF.Tables(0).NewRow()
                            drFormulaDCF("nCRFDtlNo") = ds_CRFDtl.Tables(0).Rows(0).Item("nCRFDtlNo")
                            drFormulaDCF("iSrNo") = "1"
                            drFormulaDCF("vMedExCode") = drDCF("vMedExCode").ToString()
                            drFormulaDCF("cDCFType") = "S"
                            drFormulaDCF("iDCFBy") = Me.Session(S_UserID)
                            drFormulaDCF("dDCFDate") = System.DateTime.Now
                            drFormulaDCF("vDiscrepancy") = ""
                            drFormulaDCF("vSourceResponse") = "Please correct value."
                            drFormulaDCF("cDCFStatus") = "N"
                            drFormulaDCF("iStatusChangedBy") = 0
                            drFormulaDCF("iModifyBy") = Me.Session(S_UserID)
                            drFormulaDCF("dModifyOn") = System.DateTime.Now
                            drFormulaDCF("cStatusIndi") = "N"
                            drFormulaDCF("vModificationRemark") = ""

                            dsFormulaDCF.Tables(0).Rows.Add(drFormulaDCF)
                        End If
                    Next
                    dsFormulaDCF.Tables(0).TableName = "DCFMst"
                End If
            End If

            If ds_DCF.Tables(0).Rows.Count = 0 Then
                objCommon.ShowAlert("No discrepancy generated.", Me)
            Else

                For Each drDCF As DataRow In ds_DCF.Tables(0).Rows
                    DCFNo += drDCF("nDCFNo").ToString + ","
                Next
                DCFNo = DCFNo.Remove(DCFNo.Length - 1)
                wStr = "nDCFNo in (" + DCFNo.ToString + ") And cStatusIndi <> 'D'"
                If Not Me.objHelp.GetDCFMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                dsDCF, eStr) Then
                    Throw New Exception(eStr)
                End If

                If dsDCF.Tables(0) Is Nothing OrElse dsDCF.Tables(0).Rows.Count < 1 Then
                    Me.objCommon.ShowAlert("DCF No. Not Found", Me.Page)
                    Exit Sub
                End If

                For Each drDCF As DataRow In ds_DCF.Tables(0).Rows
                    For Each dr_DCF As DataRow In dsDCF.Tables(0).Rows
                        If drDCF("nDCFNo").ToString = dr_DCF("nDCFNo").ToString Then
                            dr_DCF("vSourceResponse") = drDCF("vSourceResponse").ToString.Trim()

                            If drDCF("vUserTypeCode").ToString.Trim() = Me.Session(S_UserType) Then
                                dr_DCF("cDCFStatus") = "R"

                                If drDCF("cDCFType").ToString.Trim().ToUpper() = "S" Then
                                    dr_DCF("cDCFStatus") = "I"
                                End If
                            Else
                                dr_DCF("cDCFStatus") = "O"
                                If drDCF("cDCFType").ToString.Trim().ToUpper() = "S" Then
                                    dr_DCF("cDCFStatus") = "I"
                                End If
                                ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                                If drDCF("cDCFType").ToString.Trim().ToUpper() = "S" AndAlso
                                        Me.Session(vs_workflowstageidfordynamic) = WorkFlowStageId_DataEntry Then
                                    dr_DCF("cDCFStatus") = "I"
                                End If
                                ''Commented by nipun khant for dynamic review
                                'If drDCF("cDCFType").ToString.Trim().ToUpper() = "S" AndAlso _
                                '        Me.Session(S_WorkFlowStageId) = WorkFlowStageId_DataEntry Then
                                '    dr_DCF("cDCFStatus") = "I"
                                'End If
                            End If
                            dr_DCF("vModificationRemark") = Me.HdReasonDesc.Value.ToString()
                            dr_DCF("iStatusChangedBy") = Me.Session(S_UserID)
                            dr_DCF("iModifyBy") = Me.Session(S_UserID)
                            dsDCF.AcceptChanges()
                        End If
                    Next

                Next
                'If Not Me.objLambda.Save_DCFMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                '                           dsDCF, Me.Session(S_UserID), eStr) Then
                '    Throw New Exception(eStr)
                'End If
                dsDCF.Tables(0).TableName = "DCFMst_Update"
                ds_CRFSubDtl.Tables.Add(dsDCF.Tables(0).Copy())
            End If

            If Not dsFormulaDCF Is Nothing Then
                ds_CRFSubDtl.Tables.Add(dsFormulaDCF.Tables(0).Copy())
            End If


            If Not Me.objLambda.Save_CRFSubDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add,
               ds_CRFSubDtl, Me.Session(S_UserID), eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_CRFDtl.Tables(0).Rows(0).Item("cDataStatus") = CRF_Review Then
                EditChecks()
                If Not FireDynamicEditChecks() Then
                    Throw New Exception("Errror While Fire EditChecks...btnSaveRemarksForAttribute")
                End If
            End If

            Me.objCommon.ShowAlert("Attribute Updated Successfully.", Me.Page)
            Me.txtRemarkForAttributeEdit.Text = ""
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "AnyDivShowHide('H');", True)

            'If Not GenCall() Then
            '    Exit Sub
            'End If
            Me.HFSessionFlg.Value = "1"
            Me.ddlActivities_SelectedIndexChanged(sender, e)

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Saving Remarks : ", ex.Message)
            Me.objCommon.ShowAlert("Error While Saving Remarks : " + ex.Message, Me.Page)
        End Try
    End Sub

    Protected Sub btnAudittrail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAudittrail.Click
        Dim MedexGroupCode As String = String.Empty
        'Add by shivani pandya for all repeatition
        If Me.Session(S_GetLetestData + CurrentPageSessionVariable + hdnActivityName.Value) <> "getData" Then
            Me.Session(S_GetLetestData + CurrentPageSessionVariable + hdnActivityName.Value) = ""
        End If
        If Not fillGrid(MedexGroupCode) Then
            Exit Sub
        End If
        Me.divHistoryDtl.Style.Add("display", "")
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "getContentData", " getContentData();", True)
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "UnBindClick", "$('#btnAudittrail').unbind('click');", True)
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "displayBackGround", "displayBackGround();", True)
    End Sub
    '##########################################################For Edit Check Query Added by Vivek Patel##########################################################
    Protected Sub btnEditCheckQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditCheckQuery.Click
        Dim MedexGroupCode As String = String.Empty
        If Not ShowEditCheckQuery() Then
            Exit Sub
        End If
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fnEditCheckQueryUI", "fnEditCheckQueryUI(); ", True)
        Me.divEditChecksQuery.Style.Add("display", "")
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "getContentData", " getContentData();", True)
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "UnBindClick", "$('#btnEditCheckQuery').unbind('click');", True)
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "displayBackGround", "displayBackGround();", True)
    End Sub
    '#################################################################################################################################################################

    Protected Sub btnUpdateDiscrepancy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateDiscrepancy.Click
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim dsDCF As New DataSet
        Dim dr As DataRow
        Dim controlId As String = String.Empty
        Dim ObjControl As Control
        Dim discrepancy As String = String.Empty
        Dim oblControlType As String = String.Empty

        Try
            'Add by shivani pandya for All repeatition
            If Me.Session(S_GetLetestData + CurrentPageSessionVariable + hdnActivityName.Value) <> "getData" Then
                Me.Session(S_GetLetestData + CurrentPageSessionVariable + hdnActivityName.Value) = ""
            End If
            If Me.ViewState(VS_DCFNo) Is Nothing OrElse CType(Me.ViewState(VS_DCFNo), String).Trim() = "" Then
                Me.objCommon.ShowAlert("DCF No. Not Found", Me.Page)
                Exit Sub
            End If

            wStr = "nDCFNo = " + CType(Me.ViewState(VS_DCFNo), String).Trim() + " And cStatusIndi <> 'D'"
            If Not Me.objHelp.GetDCFMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                        dsDCF, eStr) Then
                Throw New Exception(eStr)
            End If

            If dsDCF.Tables(0) Is Nothing OrElse dsDCF.Tables(0).Rows.Count < 1 Then
                Me.objCommon.ShowAlert("DCF No. Not Found", Me.Page)
                Exit Sub
            End If

            For Each dr In dsDCF.Tables(0).Rows
                controlId = Me.hfMedexCode.Value.Trim()

                If Not Me.GetControlValue(discrepancy, controlId, ObjControl, oblControlType) Then
                    Me.objCommon.ShowAlert("Problem While Editing Attribute Detail", Me.Page)
                    Exit Sub
                End If
                'dr("vModificationRemark") = dr("vModificationRemark")
                dr("vSourceResponse") = Me.txtDiscrepancyRemarks.Text.Trim()
                dr("cDCFStatus") = Me.ddlDiscrepancyStatus.SelectedItem.Value.Trim()
                dr("iStatusChangedBy") = Me.Session(S_UserID)
                dr("iModifyBy") = Me.Session(S_UserID)
                'Add by shivani pandya for DCF Update Remarks                
                dr("vUpdateRemarks") = IIf(Me.txtDCFUpdateRemarks.Text.Trim() = "", HdReasonDesc.Value, Me.txtDCFUpdateRemarks.Text.Trim())
            Next
            dsDCF.AcceptChanges()

            If Not Me.objLambda.Save_DCFMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit,
                                    dsDCF, Me.Session(S_UserID), eStr) Then
                Throw New Exception(eStr)
            End If

            Me.objCommon.ShowAlert("Discrepancy Updated Successfully.", Me.Page)
            Me.txtDiscrepancyRemarks.Text = ""
            If Not Me.fillDCFGrid() Then
                Exit Sub
            End If
            'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "AnyDivShowHide('DCFSHOW');", True)
            'Me.MpeDCF.Show()
            Me.divDCF.Style.Add("display", "")
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "UnBindClick", "$('#btnUpdateDiscrepancy').unbind('click');", True)
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "displayBackGround", "displayBackGround();", True)

            Me.HFSessionFlg.Value = "1"
            Me.ddlActivities_SelectedIndexChanged(sender, e)

            If btnGridViewDisplay.Visible = True And hndtabluler.Value = "Tabuler" Then
                Session(S_DiscrpancyStatus) = "tabuler"
            End If

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Updating DCF. ", ex.Message)
        End Try
    End Sub

    Protected Sub btnMeddraBrowse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMeddraBrowse.Click
        Dim RedirectStr As String = String.Empty
        Dim MedExCode As String = String.Empty
        MedExCode = Me.hfMedexCode.Value.Trim()
        MedExCode = MedExCode.Substring(0, MedExCode.IndexOf("R"))
        RedirectStr = "window.open(""" + "frmCTMMeddraBrowse.aspx?MedExCode=" & MedExCode & """)"

        ScriptManager.RegisterClientScriptBlock(Me.Page(), Me.GetType(), "OpenWindow", RedirectStr, True)
    End Sub

    Protected Sub btnAutoCalculate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAutoCalculate.Click
        Dim formula As String = String.Empty
        Dim Deci As Integer
        Dim MedExes() As String
        Dim index As Integer = 0
        Dim result As String = String.Empty
        Dim objControl As Control = Nothing
        Dim evaluator As New Evaluator
        Dim FinalResult As Double = 0
        Dim oblControlType As String = String.Empty
        Try

            formula = Me.HFMedExFormula.Value.Trim()
            Deci = Me.hfDecimalNo.Value
            MedExes = formula.Split("?")
            formula = formula.Replace("?", "")

            For index = 0 To MedExes.Length - 1
                If MedExes(index).Length = 5 Then


                    If Not Me.GetControlValueForMedexFromula(result, MedExes(index).ToString + "R" + hfMedexCode.Value.Substring(hfMedexCode.Value.IndexOf("R") + 1), objControl, oblControlType) Then
                        Exit Sub
                    End If
                    If result.ToString.Trim() = "" Then
                        result = 0
                    End If
                    formula = formula.Replace(MedExes(index).Trim(), result)
                End If
            Next index

            If oblControlType = "DateTime" Then
                ' ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "DiffAge", "DiffAge(' & formula & ", True)
                FinalResult = evaluator.GetDateDiff(formula)
            Else
                FinalResult = Math.Round(evaluator.Eval(formula), Deci)
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetResult", "SetFormulaResult('" + FinalResult.ToString() + "','" + Deci.ToString() + "');", True)

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Auto Calculation. ", ex.Message)
        End Try
    End Sub

    Protected Sub BtnDosingTime_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnDosingTime.Click
        Dim objControl As Control
        Dim MedexId As String = "00650"
        Dim result As String = String.Empty
        Dim oblControlType As String = String.Empty

        If Not Me.GetControlValue(result, MedexId, objControl, oblControlType) Then
            Exit Sub
        End If
    End Sub

    Protected Sub btnOk_MPEID_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk_MPEID.Click
        'This event is created to show the attribute template
        ' Here no need to call gencall as the pageload event will automaticaly handel this
        Try
            Me.HFRemarks.Value = Me.txtContent.Text
            Me.MPEActivitySequence.Hide()
            If Not FillActivities() Then
                Throw New Exception("Error While Feeling Activity")
            End If
            Me.ddlActivities.Style.Add("color", "Red")
            Me.Session(S_SelectedActivity + CurrentPageSessionVariable) = Nothing

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "Open({flag:'LOAD',wId:'" + Me.Request.QueryString("WorkSpaceId").ToString + "',nId:'" + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + "',pId:'" + Me.Request.QueryString("PeriodId").ToString + "',sId:'" + Me.Request.QueryString("SubjectId").ToString + "'});", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnOk_MPEID_Click")
        End Try
    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim ds_DCF As New DataSet
        Dim estr_retu As String = String.Empty
        Dim wstr As String = String.Empty
        Dim ds_reviewer As New DataSet
        Dim dv_reviewer As DataView
        Try
            'wstr = "(cDCFStatus = '" + Discrepancy_Generated + "' Or cDCFStatus = '" + Discrepancy_Answered _
            '                                  + "') And ((( vUserTypeCode = '" + Me.Session(S_UserType) + _
            '                                  "' Or CRFWorkflowBy < " + Me.Session(S_WorkFlowStageId) _
            '                                  + ") And cDCFType = 'M') Or (cDCFType = 'S')) And CRFWorkflowBy <> " & WorkFlowStageId_OnlyView

            ds_reviewer = CType(Me.Session(Vs_dsReviewerlevel), DataSet)
            dv_reviewer = ds_reviewer.Tables(0).Copy.DefaultView
            dv_reviewer.RowFilter = "iReviewWorkflowStageId = " + Me.Session(S_WorkFlowStageId) + " and vUsertypecode='" + Me.Session(S_UserType) + "'"

            If dv_reviewer.ToTable.Rows.Count = 0 Then
                Exit Sub
            End If
            wstr = " (cDCFStatus = '" + Discrepancy_Generated + "' or (cDCFStatus = '" + Discrepancy_Answered + "' and CRFWorkflowBy = " + Convert.ToString(dv_reviewer.ToTable.Rows(0)("iActualWorkflowStageId")) + ")) " +
                    " and cDCFType in ('M','S') AND CRFWorkflowBy <> " & WorkFlowStageId_OnlyView

            If Me.HReviewFlag.Value = 1 Then
                wstr += " And nCRFDtlNo =" + Me.HFCRFDtlNo.Value.Trim()
            Else
                wstr += " And nCRFHdrNo =" + Me.HFCRFHdrNo.Value.Trim() + " And vSubjectId ='" + Me.HFSubjectId.Value.Trim() + "'"
                'wstr += " And iParentNodeId = " + Me.HFParentNodeId.Value.Trim() + " And vSubjectId ='" + Me.HFSubjectId.Value.Trim() + "'"
            End If

            If Not objHelp.GetData("VIEW_DCFMst_Optimized", "*", wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DCF, estr_retu) Then
                Throw New Exception("Error while getting status of DCF..." + estr_retu)
            End If
            If ds_DCF.Tables(0).Rows.Count > 0 Then
                objCommon.ShowAlert(ds_DCF.Tables(0).Rows.Count.ToString() + " Discrepancy Pending, You Can Not Review.", Me.Page)
                If Me.HReviewFlag.Value = 1 Then
                    Exit Sub
                End If
            End If

            ''Added by nipun khant for dynamic review
            If dv_reviewer.ToTable().Rows(0)("cAuthenticationDialog").ToString().ToUpper() = "N" Then
                ''Commented by nipun khant for dynamic review
                'If (Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_FinalReviewAndLock AndAlso Me.Session(S_ScopeNo) = Scope_ClinicalTrial) Or (Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_SecondReview AndAlso Me.Session(S_ScopeNo) = Scope_BABE) Then
                If Not SaveCRFDtl() Then
                    Me.objCommon.ShowAlert("Error occured while saving CRFWorkFlow Detail.", Me.Page)
                    Exit Sub
                End If
            Else
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "UnBindClick", "$('#btnOk').unbind('click');", True)
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "displayBackGround", "displayBackGround();", True)
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "displaydivAuthentication", "displaydivAuthentication();", True)
            End If

        Catch ex As Exception

        End Try
    End Sub

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

            Me.ViewState(VS_DataStatus) = CRF_DataEntryCompleted
            Me.ViewState(VS_Choice) = WS_HelpDbTable.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

            SubjectId = Me.HFSubjectId.Value.Trim()
            WorkspaceId = Me.HFWorkspaceId.Value.Trim()
            ActivityId = Me.HFActivityId.Value.Trim()
            NodeId = Me.HFNodeId.Value.Trim()
            PeriodId = Me.HFPeriodId.Value.Trim()
            MySubjectNo = Me.HFMySubjectNo.Value.Trim()

            If Not AssignValues(SubjectId, WorkspaceId, ActivityId, NodeId, PeriodId, MySubjectNo, DsSave) Then
                Me.objCommon.ShowAlert("Error While Assigning Data.", Me.Page)
                Exit Sub
            End If

            If Not Me.objLambda.Save_CRFHdrDtlSubDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, DsSave, False, Me.Session(S_UserID), estr) Then
                Me.objCommon.ShowAlert(estr, Me.Page)
                Exit Sub
            Else

                objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection) 'Me.PlaceMedEx.Controls 
                For Each objControl In objCollection
                    If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.FileUpload" Then
                        Dim filename As String = ""

                        If objControl.ID.ToString.Contains("FU") Then
                            ObjId = objControl.ID.ToString.Replace("FU", "")
                        Else
                            ObjId = objControl.ID.ToString.Trim()
                        End If
                        If Request.Files(objControl.ID).FileName = "" And
                             Not IsNothing(CType(FindControl("hlnk" + ObjId), HyperLink)) AndAlso
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
            'Check data status before executing editchecks
            Me.EditChecks()

            Me.objCommon.ShowAlert("Attributes Saved Successfully.", Me.Page)

            Me.Session.Remove("PlaceMedEx")
            Me.HFSessionFlg.Value = "1"
            Me.ddlActivities_SelectedIndexChanged(sender, e)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "...btnSaveRunTime_Click")
        End Try
    End Sub

    Protected Sub imgViewAll_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgViewAll.Click
        ddlActivities_SelectedIndexChanged(sender, e)
        'Add by shivani pandya for view letest repeatition
        Me.Session(S_GetLetestData + CurrentPageSessionVariable + hdnActivityName.Value) = "getData"
        Me.Session("ViewAll") = "Y"
        Me.hndGridViewStatus.Value = "Grid"
        'Me.Session(S_DataStatus) = "B"
        Me.hndRepetitionNo.Value = ""
        Me.Session(S_SelectedRepeatation + CurrentPageSessionVariable + hdnActivityName.Value) = Nothing
        'End
    End Sub
    Protected Sub btnDeleteRepetition_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteRepetition.Click
        Dim MedexGroupCode As String = String.Empty
        If Not fillGrid(MedexGroupCode) Then
            Exit Sub
        End If
        Me.divHistoryDtl.Style.Add("display", "")
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "getContentData", " getContentData();", True)
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "UnBindClick", "$('#btnDeleteRepetition').unbind('click');", True)
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "displayBackGround", "displayBackGround();", True)
    End Sub
    Protected Sub btnSaveDeleteRepetition_Click(sender As Object, e As EventArgs) Handles btnSaveDeleteRepetition.Click
        Dim str As String = String.Empty
        Dim estr As String = String.Empty
        Dim StrRedirect As String = String.Empty
        Try
            If Me.ddlRepeatNo.Controls.Count = 2 Then
                Selectvar = Me.ddlActivities.SelectedIndex.ToString
                Me.PlaceMedEx.Controls.Clear()
                If Not GenCall_ShowUI() Then
                    Throw New Exception("Error in filtering Repetition No.... ddlRepeatNo_SelectedIndexChanged")
                End If
            End If

            str = " EXEC Proc_UpdateDeletedRepetition " + Me.hdnDeleteRepetitionNo.Value.Split("_")(0).ToString + " ,  " + Me.Session(S_UserID).ToString() + " , '" + Me.txtDeleteRepetition.Text + "'"

            If Not objHelp.ExecuteQuery_Boolean(str, estr) Then
                Throw New Exception(estr)
            Else
                If Not Getstring(StrRedirect) Then
                    Me.objCommon.ShowAlert("Error occured while getting string.", Me.Page)
                    Exit Sub
                End If

                objCommon.ShowAlertAndRedirectAfter6Second("Repetition Deleted Successfully.", "'" + StrRedirect + "'", Me.Page)
            End If

            'add by shivani pandya
            If btnGridViewDisplay.Visible = True And hndtabluler.Value = "Tabuler" Then
                Session(S_DiscrpancyStatus) = "tabuler"
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnSaveDeleteRepetition_Click")
        End Try

    End Sub

    'Protected Sub btnOkDataEntryControl_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOkDataEntryControl.Click
    '    Dim ds_DataEntryControl As DataSet = Nothing
    '    Dim estr As String = String.Empty
    '    Try
    '        If Not objHelp.GetData("DataEntryControl", "*", "vSubjectId='" & Me.HFScreenNo.Value & "' And vWorkspaceId='" & Me.HFWorkspaceId.Value & "' And iNodeId=" & ddlActivities.SelectedValue.Split("#")(1), WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DataEntryControl, estr) Then
    '            Throw New Exception("Error While getting information of Data Entry Control...PageLoad()..")
    '        End If

    '        ds_DataEntryControl.Tables(0).Rows(0).Item("iModiFyBy") = Session(S_UserID)
    '        ds_DataEntryControl.Tables(0).AcceptChanges()
    '        If Not objLambda.insert_DataEntryControl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_DataEntryControl, estr) Then
    '            Throw New Exception("Error While saving information of Data Entry Control...PageLoad()..")
    '        End If
    '        Me.MpeDataentryControl.Hide()
    '    Catch ex As Exception
    '        objCommon.ShowAlert(ex.ToString, Me.Page())
    '    End Try
    'End Sub


#End Region

#Region "AssignImageHdrdetailValues"

    Private Function SaveImageHdrdetailValues(Reviewstatus As String) As Boolean
        Dim wstr As String = String.Empty
        Dim ds_ImgTransmittalHdr As New DataSet
        Dim estr As String = String.Empty
        Dim Status As String = String.Empty

        'Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

        Try
            If Convert.ToString(ConfigurationManager.AppSettings("QCUserCode")).Contains(Session(S_UserType)) AndAlso Reviewstatus = "A" Then
                Status = "QC1A"
            ElseIf Convert.ToString(ConfigurationManager.AppSettings("QCUserCode")).Contains(Session(S_UserType)) AndAlso Reviewstatus = "R" Then
                Status = "QC1R"
            End If

            If Convert.ToString(ConfigurationManager.AppSettings("QC2UserCode")).Contains(Session(S_UserType)) AndAlso Reviewstatus = "A" Then
                Status = "QC2A"
            ElseIf Convert.ToString(ConfigurationManager.AppSettings("QC2UserCode")).Contains(Session(S_UserType)) AndAlso Reviewstatus = "R" Then
                Status = "QC2R"
            End If

            If Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(Session(S_UserType)) AndAlso Reviewstatus = "A" Then
                Status = "CA1A"
            ElseIf Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(Session(S_UserType)) AndAlso Reviewstatus = "R" Then
                Status = "CA1R"
            End If

            wstr = "select * from ImgTransmittalHdr where  vWorkspaceId='" + Me.HFWorkspaceId.Value.Trim() + "' AND vSubjectId ='" + Me.HFSubjectId.Value.Trim() + "' AND vActivityId='" + HFParentActivityId.Value.Trim() + "' AND iNodeId='" + HFParentNodeId.Value.Trim() + "' AND cStatusIndi <> 'D' "
            ds_ImgTransmittalHdr = objHelp.GetResultSet(wstr, "ImgTransmittalHdr")

            wstr = ""

            If ds_ImgTransmittalHdr.Tables(0).Rows.Count > 0 Then

                For Each dr As DataRow In ds_ImgTransmittalHdr.Tables(0).Rows
                    dr("cReviewStatus") = Status
                Next

                wstr = ds_ImgTransmittalHdr.Tables(0).Rows(0)("iImgTransmittalHdrId").ToString() + "##" + Reviewstatus


                ds_ImgTransmittalHdr.AcceptChanges()
                If Not objLambda.Save_ImgTransmittalHdr(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_ImgTransmittalHdr, Me.Session(S_UserID), estr) Then
                    Me.objCommon.ShowAlert(estr, Me.Page)
                    Return False
                    Exit Function
                End If
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".......SaveImageHdrdetailValues")
            Return False
        End Try
    End Function
#End Region

#Region "Authenticate Password"
    Private Function Auntheticate(ByVal Password As String) As Boolean
        Dim pwd As String = String.Empty
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        'pwd = Me.txtPassword.Text
        pwd = objHelp.EncryptPassword(Password)


        If (pwd.ToString() = "") Then
            Me.txtPasswords.Focus()
            ObjCommon.ShowAlert("Please Enter Password !", Me.Page)
            Return False
        End If

        If Convert.ToString(Me.Session(S_Password)) <> pwd.ToString() Then
            ObjCommon.ShowAlert("Password Authentication Fails !", Me.Page)
            Me.txtPasswords.Focus()
            Return False
        End If
        Return True
    End Function

#End Region

#Region "CRFVersion"
    Private Function CRFVersion_ShowUI() As Boolean
        Dim ds_CRFVersion As DataSet = Nothing
        Dim VersionNo As Integer = 0
        Dim VersionDate As Date
        Dim wstr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Try
            '====== CRFVersion Control==================================
            wstr = Me.HFWorkspaceId.Value.Trim() + "##" + Me.HFPeriodId.Value.Trim() + "##" + Me.HFNodeId.Value.Trim() + "##" + Me.HFSubjectId.Value.Trim()
            If Not objHelp.Proc_SubjectWiseVersionDtls(wstr, ds_CRFVersion, eStr_Retu) Then
                Throw New Exception("Error While Getting Data From CRFDtl")
            End If
            If Not ds_CRFVersion Is Nothing AndAlso ds_CRFVersion.Tables(0).Rows.Count < 0 Then

                If ds_CRFVersion.Tables(0).Rows(0)("nVersionNo").ToString <> "" Then
                    VersionNo = ds_CRFVersion.Tables(0).Rows(0)("nVersionNo")
                    VersionDate = ds_CRFVersion.Tables(0).Rows(0)("dVersionDate")
                    Me.VersionNo.Text = VersionNo.ToString
                    Me.VersionDate.Text = VersionDate.ToString("dd-MMM-yyyy")
                    Me.VersionDtl.Attributes.Add("style", "display:;")
                Else
                    Me.VersionDtl.Attributes.Add("style", "display:none;")
                End If
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "...CRFVersion_ShowUI")
            Return False
        End Try

    End Function

    Private Function getCRFVersion() As Char
        Dim ds_CrfVersionDetail As DataSet = Nothing
        Dim VersionNo As Integer = 0
        Dim VersionDate As Date
        Dim wstr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Try

            ''====== CRFVersion Control==================================

            wstr = "vWorkSpaceId = '" + Me.HFWorkspaceId.Value.Trim() + "'"
            If Not objHelp.GetData("View_CRFVersionForDataEntryControl", "*", wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CrfVersionDetail, eStr_Retu) Then
                Throw New Exception("Error While Getting Status For CrfVersion")
            End If

            If Not ds_CrfVersionDetail Is Nothing AndAlso ds_CrfVersionDetail.Tables(0).Rows.Count > 0 Then
                VersionNo = ds_CrfVersionDetail.Tables(0).Rows(0)("nVersionNo")
                VersionDate = ds_CrfVersionDetail.Tables(0).Rows(0)("dVersiondate").ToString
                Me.VersionNo.Text = VersionNo.ToString
                Me.VersionDate.Text = VersionDate.ToString("dd-MMM-yyyy")
                Me.VersionDtl.Attributes.Add("style", "display:;")
                If ds_CrfVersionDetail.Tables(0).Rows(0)("cFreezeStatus").ToString.Trim.ToUpper = "U" Then
                    ImageLockUnlock.Attributes.Add("src", "images/UnFreeze.jpg")
                    getCRFVersion = "U" 'U=UnFreeze
                    Exit Function
                End If
                getCRFVersion = "F" 'F=Freeze
            Else
                Me.VersionDtl.Attributes.Add("style", "display:none;")
                getCRFVersion = "N" 'N=Not Applicable(For This Project There is No Data Entry In CRFVersionTable)
            End If
            '==========================================================
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString(), "...getCRFVersion")
            Return ""
        End Try

    End Function
#End Region

#Region "AssignValuesForSequenceDeviation"

    Private Function AssignValuesForSequenceDeviation(ByRef DsSave As DataSet) As Boolean
        Dim DsCRFDtl As New DataSet
        Dim DtSequenceDeviation As New DataTable
        Dim dr As DataRow
        Dim estr As String = String.Empty
        Dim ControlId As String = String.Empty

        Try
            If Not objHelp.GetData("WorkSpaceActivitySequenceDeviation", "*", "", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, DsSave, estr) Then
                Throw New Exception("Error While Saving Data For ActivitySeuenceDeviation")
            End If

            DtSequenceDeviation = DsSave.Tables(0)
            dr = DtSequenceDeviation.NewRow
            dr("iSequenceDeviation") = 1
            dr("vWorkspaceId") = Me.HFWorkspaceId.Value
            dr("iPeriod") = Me.HFPeriodId.Value
            dr("vSubjectId") = Me.HFSubjectId.Value
            dr("iNodeId") = Me.HFNodeId.Value
            dr("vPendingNodes") = Me.HFPendingNode.Value
            dr("vRemarks") = Me.HFRemarks.Value
            dr("iUserId") = Me.Session(S_UserID).ToString()
            DtSequenceDeviation.Rows.Add(dr)
            DtSequenceDeviation.AcceptChanges()

            DsSave = Nothing
            DsSave = New DataSet
            DsSave.Tables.Add(DtSequenceDeviation.Copy())
            DsSave.AcceptChanges()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignValuesForSequenceDeviation")
            Return False
        End Try

    End Function

#End Region

#Region "Fill And Helper Functions"

    Private Function GetControlValue(ByRef result As String, ByVal ControlId As String, ByRef objControl_Retu As Control, ByRef oblControlType As String) As Boolean
        Dim objCollection As ControlCollection
        Dim objControl As Control
        Dim ObjId As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim dt As DataTable
        Dim flg As Boolean = False


        Try

            objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection)

            For Each objControl In objCollection

                If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.TextBox" Then
                    If objControl.ID.ToString.Contains("txt") Then
                        ObjId = objControl.ID.ToString.Replace("txt", "")
                    Else
                        ObjId = objControl.ID.ToString.Trim()
                    End If
                    If ObjId.ToString = hfMedexCode.Value Then
                        ObjId = ObjId.Substring(0, ObjId.IndexOf("R"))

                        'If objControl.ID.Trim.ToUpper() = ControlId.Trim.ToUpper() Then
                        If ObjId = ControlId.Trim.ToUpper() Then
                            If objControl.ID.ToString.Contains("txt") Then
                                ObjId = objControl.ID.ToString.Replace("txt", "")
                            Else
                                ObjId = objControl.ID.ToString.Trim()
                            End If


                            ObjId = ObjId.Substring(0, ObjId.IndexOf("R"))

                            dt = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Copy()
                            dt.DefaultView().RowFilter = "vMedexCode='" & ObjId & "' And iRepeatNo=" & hfMedexCode.Value.Substring((hfMedexCode.Value.IndexOf("R") + 1), hfMedexCode.Value.Length - (hfMedexCode.Value.IndexOf("R") + 1))
                            dt.DefaultView.ToTable()
                            oblControlType = dt.DefaultView.ToTable().Rows(0).Item("vMedexType")
                            objControl_Retu = objControl


                            If (oblControlType.ToUpper() = "DATETIME" Or oblControlType.ToUpper() = "TIME" Or oblControlType.ToUpper() = "ASYNCTIME" Or oblControlType.ToUpper() = "ASYNCDATETIME") Then
                                result = Request.Form(objControl.ID).ToString().Split(",")(0)
                            Else
                                result = Request.Form(objControl.ID)
                            End If


                            Return True
                        End If
                    End If
                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" Then
                    ObjId = objControl.ID.ToString.Trim()
                    '    Me.hfMedexCode.Value
                    If DirectCast(objControl, System.Web.UI.WebControls.DropDownList).Attributes("StandardDate") = "Y" Then
                        If ObjId.Substring(0, ObjId.IndexOf("_")) = Me.hfMedexCode.Value Then
                            If result = "" Then
                                For Each objControl1 In objCollection
                                    If Not objControl1.ID Is Nothing AndAlso objControl1.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" AndAlso DirectCast(objControl1, System.Web.UI.WebControls.DropDownList).Attributes("StandardDate") = "Y" AndAlso objControl1.ID.ToString.Trim().Substring(0, objControl1.ID.ToString.Trim().IndexOf("_")) = Me.hfMedexCode.Value Then
                                        If Request.Form(objControl1.ID) = "" Then
                                            flg = True
                                        Else
                                            result += Request.Form(objControl1.ID).ToString().Split(",")(0)
                                        End If
                                        objControl_Retu = objControl1
                                    End If
                                Next
                                result = IIf(flg = True, "", result)
                            End If
                        End If
                    Else
                        If ObjId.ToString = hfMedexCode.Value Then
                            ObjId = ObjId.Substring(0, ObjId.IndexOf("R"))

                            If ObjId = ControlId.Trim.ToUpper() Then
                                result = Request.Form(objControl.ID)
                                objControl_Retu = objControl
                                Return True
                            End If
                        End If
                    End If

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.FileUpload" Then
                    If objControl.ID.Trim.ToUpper() = ControlId.Trim.ToUpper() Then
                        Dim filename As String = String.Empty
                        If objControl.ID.ToString.Contains("FU") Then
                            ObjId = objControl.ID.ToString.Replace("FU", "")
                        Else
                            ObjId = objControl.ID.ToString.Trim()
                        End If
                        ObjId = ObjId.Substring(0, ObjId.IndexOf("R"))

                        If Request.Files(objControl.ID).FileName = "" And
                            Not IsNothing(CType(FindControl("hlnk" + ObjId), HyperLink)) AndAlso
                            CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl <> "" Then

                            filename = CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl

                        ElseIf Request.Files(objControl.ID).FileName <> "" Then

                            filename = "~/" + System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail") +
                                        Me.HFWorkspaceId.Value.Trim() + "/" + Me.HFNodeId.Value.Trim() + "/" + Me.HFSubjectId.Value.Trim() + "/" + Path.GetFileName(Request.Files(objControl.ID).FileName.ToString.Trim())
                        End If
                        result = filename
                        objControl_Retu = objControl
                        Return True
                    End If

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.RadioButtonList" Then
                    ObjId = objControl.ID.ToString.Trim()
                    If ObjId.ToString = hfMedexCode.Value.ToString Then
                        ObjId = ObjId.Substring(0, ObjId.IndexOf("R"))
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

                            result = Request.Form(objControl.ID)

                            If Convert.ToString(Me.HFRadioButtonValue.Value).Trim() = "NULL" Then
                                result = ""
                            End If
                            Me.HFRadioButtonValue.Value = ""
                            objControl_Retu = objControl
                            Return True
                        End If
                    End If
                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.CheckBoxList" Then
                    ObjId = objControl.ID.ToString.Trim()
                    If ObjId.ToString = hfMedexCode.Value.ToString Then
                        ObjId = ObjId.Substring(0, ObjId.IndexOf("R"))
                        If ObjId = ControlId.Trim.ToUpper() Then
                            'Dim chkl As CheckBoxList = CType(FindControl(objControl.ID), CheckBoxList)
                            'Dim StrChk As String = String.Empty

                            'For index As Integer = 0 To chkl.Items.Count - 1
                            '    StrChk += IIf(chkl.Items(index).Selected, chkl.Items(index).Value.ToString.Trim() + ",", "")
                            'Next index

                            'If StrChk.Trim() <> "" Then
                            '    StrChk = StrChk.Substring(0, StrChk.LastIndexOf(","))
                            'End If
                            result = Me.HFChkSelectedList.Value.ToString.Trim()
                            objControl_Retu = objControl
                            Return True
                        End If
                    End If

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.HiddenField" Then
                    If objControl.ID.Trim.ToUpper() = ControlId.Trim.ToUpper() Then
                        '******Adding Header & footer to the document**********************
                        Dim ds_WorkSpaceNodeHistory As New DataSet
                        Dim filename As String = String.Empty
                        Dim versionNo As String = String.Empty
                        Wstr = "vWorkSpaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And iNodeId=" + Me.HFNodeId.Value.Trim() + " And iStageId =" & GeneralModule.Stage_Authorized

                        If Not objHelp.GetViewWorkSpaceNodeHistory(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                                                ds_WorkSpaceNodeHistory, estr) Then
                            Me.ShowErrorMessage("Error While Getting Data From View_WorkSpaceNodeHistory", estr)
                        End If

                        If Not ds_WorkSpaceNodeHistory.Tables(0).Rows.Count < 1 Then

                            filename = ds_WorkSpaceNodeHistory.Tables(0).Rows(0).Item("vDocPath").ToString()
                            versionNo = ds_WorkSpaceNodeHistory.Tables(0).Rows(0).Item("vUSerVersion").ToString()
                        End If
                        '*****************************************
                        ObjId = ObjId.Substring(0, ObjId.IndexOf("R"))

                        If ObjId = GeneralModule.Medex_FilePath.Trim() Then
                            result = filename  'File Name from WorkspaceNodeHistory
                        ElseIf ObjId = GeneralModule.Medex_DownloadedBy.Trim() Then
                            result = Me.Session(S_UserID)
                        ElseIf ObjId = GeneralModule.Medex_VersionNo.Trim() Then
                            result = versionNo 'Version No from WorkspaceNodeHistory
                        ElseIf ObjId = GeneralModule.Medex_ReleaseDate.Trim() Then
                            result = Date.Now.Date.ToString("dd-MMM-yyyy") 'Version No from WorkspaceNodeHistory
                        Else
                            result = Request.Form(objControl.ID) 'CType(objControl, TextBox).Text 'CType(Me.FindControl(obj.GetId), TextBox).Text
                        End If
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

    Private Function GetControlValueForMedexFromula(ByRef result As String, ByVal ControlId As String, ByRef objControl_Retu As Control, ByRef oblControlType As String) As Boolean
        Dim objCollection As ControlCollection
        Dim objControl As Control
        Dim ObjId As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim dt As DataTable


        Try

            objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection)

            For Each objControl In objCollection

                If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.TextBox" Then
                    If objControl.ID.ToString.Contains("txt") Then
                        ObjId = objControl.ID.ToString.Replace("txt", "")
                    Else
                        ObjId = objControl.ID.ToString.Trim()
                    End If
                    ' If ObjId.ToString = hfMedexCode.Value Then
                    'ObjId = ObjId.Substring(0, ObjId.IndexOf("R"))

                    'If objControl.ID.Trim.ToUpper() = ControlId.Trim.ToUpper() Then
                    If ObjId = ControlId.Trim.ToUpper() Then
                        If objControl.ID.ToString.Contains("txt") Then
                            ObjId = objControl.ID.ToString.Replace("txt", "")
                        Else
                            ObjId = objControl.ID.ToString.Trim()
                        End If
                        ObjId = ObjId.Substring(0, ObjId.IndexOf("R"))
                        result = Request.Form(objControl.ID)

                        dt = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Copy()
                        dt.DefaultView().RowFilter = "vMedexCode='" & ObjId & "' And iRepeatNo=" & hfMedexCode.Value.Substring((hfMedexCode.Value.IndexOf("R") + 1), hfMedexCode.Value.Length - (hfMedexCode.Value.IndexOf("R") + 1))
                        dt.DefaultView.ToTable()
                        oblControlType = dt.DefaultView.ToTable().Rows(0).Item("vMedexType")
                        objControl_Retu = objControl
                        Return True
                    End If
                    ' End If
                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" Then
                    ObjId = objControl.ID.ToString.Trim()
                    'If ObjId.ToString = hfMedexCode.Value Then
                    '    ObjId = ObjId.Substring(0, ObjId.IndexOf("R"))
                    If ObjId = ControlId.Trim.ToUpper() Then
                        result = Request.Form(objControl.ID)
                        objControl_Retu = objControl
                        Return True
                        'End If
                    End If

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.FileUpload" Then
                    If objControl.ID.Trim.ToUpper() = ControlId.Trim.ToUpper() Then
                        Dim filename As String = String.Empty
                        If objControl.ID.ToString.Contains("FU") Then
                            ObjId = objControl.ID.ToString.Replace("FU", "")
                        Else
                            ObjId = objControl.ID.ToString.Trim()
                        End If
                        ObjId = ObjId.Substring(0, ObjId.IndexOf("R"))

                        If Request.Files(objControl.ID).FileName = "" And
                            Not IsNothing(CType(FindControl("hlnk" + ObjId), HyperLink)) AndAlso
                            CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl <> "" Then

                            filename = CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl

                        ElseIf Request.Files(objControl.ID).FileName <> "" Then

                            filename = "~/" + System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail") +
                                        Me.HFWorkspaceId.Value.Trim() + "/" + Me.HFNodeId.Value.Trim() + "/" + Me.HFSubjectId.Value.Trim() + "/" + Path.GetFileName(Request.Files(objControl.ID).FileName.ToString.Trim())
                        End If
                        result = filename
                        objControl_Retu = objControl
                        Return True
                    End If

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.RadioButtonList" Then
                    ObjId = objControl.ID.ToString.Trim()
                    'If ObjId.ToString = hfMedexCode.Value.ToString Then
                    '    ObjId = ObjId.Substring(0, ObjId.IndexOf("R"))
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

                        If Convert.ToString(Me.HFRadioButtonValue.Value).Trim() = "NULL" Then
                            result = ""
                        End If
                        Me.HFRadioButtonValue.Value = ""
                        objControl_Retu = objControl
                        Return True
                    End If
                    'End If
                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.CheckBoxList" Then
                    ObjId = objControl.ID.ToString.Trim()
                    'If ObjId.ToString = hfMedexCode.Value.ToString Then
                    '    ObjId = ObjId.Substring(0, ObjId.IndexOf("R"))
                    If ObjId = ControlId.Trim.ToUpper() Then
                        'Dim chkl As CheckBoxList = CType(FindControl(objControl.ID), CheckBoxList)
                        'Dim StrChk As String = String.Empty

                        'For index As Integer = 0 To chkl.Items.Count - 1
                        '    StrChk += IIf(chkl.Items(index).Selected, chkl.Items(index).Value.ToString.Trim() + ",", "")
                        'Next index

                        'If StrChk.Trim() <> "" Then
                        '    StrChk = StrChk.Substring(0, StrChk.LastIndexOf(","))
                        'End If
                        result = Me.HFChkSelectedList.Value.Trim()
                        objControl_Retu = objControl
                        Return True
                        ' End If
                    End If

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.HiddenField" Then
                    If objControl.ID.Trim.ToUpper() = ControlId.Trim.ToUpper() Then
                        '******Adding Header & footer to the document**********************
                        Dim ds_WorkSpaceNodeHistory As New DataSet
                        Dim filename As String = String.Empty
                        Dim versionNo As String = String.Empty
                        Wstr = "vWorkSpaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And iNodeId=" + Me.HFNodeId.Value.Trim() + " And iStageId =" & GeneralModule.Stage_Authorized

                        If Not objHelp.GetViewWorkSpaceNodeHistory(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                                                ds_WorkSpaceNodeHistory, estr) Then
                            Me.ShowErrorMessage("Error While Getting Data From View_WorkSpaceNodeHistory", estr)
                        End If

                        If Not ds_WorkSpaceNodeHistory.Tables(0).Rows.Count < 1 Then

                            filename = ds_WorkSpaceNodeHistory.Tables(0).Rows(0).Item("vDocPath").ToString()
                            versionNo = ds_WorkSpaceNodeHistory.Tables(0).Rows(0).Item("vUSerVersion").ToString()
                        End If
                        '*****************************************
                        ObjId = ObjId.Substring(0, ObjId.IndexOf("R"))

                        If ObjId = GeneralModule.Medex_FilePath.Trim() Then
                            result = filename  'File Name from WorkspaceNodeHistory
                        ElseIf ObjId = GeneralModule.Medex_DownloadedBy.Trim() Then
                            result = Me.Session(S_UserID)
                        ElseIf ObjId = GeneralModule.Medex_VersionNo.Trim() Then
                            result = versionNo 'Version No from WorkspaceNodeHistory
                        ElseIf ObjId = GeneralModule.Medex_ReleaseDate.Trim() Then
                            result = Date.Now.Date.ToString("dd-MMM-yyyy") 'Version No from WorkspaceNodeHistory
                        Else
                            result = Request.Form(objControl.ID) 'CType(objControl, TextBox).Text 'CType(Me.FindControl(obj.GetId), TextBox).Text
                        End If
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

    'Private Function fillGrid(ByRef MedexGroupCode As String) As Boolean
    '    Dim ds_AuditTrail As New DataSet
    '    Dim dv_AuditTrail As New DataView
    '    Dim Wstr As String = String.Empty
    '    Dim estr As String = String.Empty
    '    Dim dc_Audit As DataColumn

    '    Try
    '        If Me.HFCRFHdrNo.Value.Trim() = "" OrElse Me.ddlRepeatNo.SelectedItem.Value.Trim.ToUpper() = "N" Then
    '            Me.objCommon.ShowAlert("No History Available.", Me.Page())
    '            Return False
    '        End If

    '        'Wstr = "vSubjectId='" & Me.HFSubjectId.Value.Trim() & "' And vMedexCode='" & Me.hfMedexCode.Value.Substring(0, Me.hfMedexCode.Value.IndexOf("R")) & "' And " & _
    '        '        " nCRFHdrNo=" & Me.HFCRFHdrNo.Value.Trim() & " And iNodeId=" & Me.HFNodeId.Value.Trim() & _
    '        '        " And nCRFDtlNo=" & Me.HFCRFDtlNo.Value.Trim()

    '        Wstr = "nCRFHdrNo=" & Me.HFCRFHdrNo.Value.Trim() & " And vSubjectID = '" + Me.HFSubjectId.Value.Trim() + _
    '                "' And vMedExCode = '" + Me.hfMedexCode.Value.Substring(0, Me.hfMedexCode.Value.IndexOf("R")) + _
    '                "' And iNodeId=" + Me.HFNodeId.Value.Trim() + " And iRepeatNo = " + Me.hfMedexCode.Value.Substring(Me.hfMedexCode.Value.IndexOf("R") + 1)

    '        'If Not Me.objHelp.View_CRFHdrDtlSubDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '        '                 ds_AuditTrail, estr) Then
    '        '    Me.objCommon.ShowAlert("Problem While Getting Data Of Audit Trail. " + estr, Me.Page())
    '        '    Return False
    '        'End If

    '        If Not Me.objHelp.GetData("View_CRFHdrDtlAuditTrail", "*", Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_AuditTrail, estr) Then
    '            Me.objCommon.ShowAlert("Problem While Getting Data Of Audit Trail. " + estr, Me.Page())
    '            Return False
    '        End If

    '        Me.lblMedexDescription.Text = ""

    '        If ds_AuditTrail.Tables(0).Rows.Count < 1 Then
    '            Return False
    '        End If

    '        Me.lblMedexDescription.EnableViewState = True
    '        Me.lblMedexDescription.Text = ds_AuditTrail.Tables(0).Rows(0).Item("vMedExDesc").ToString.Trim()
    '        'Me.lblMedexDescription.CssClass = "Label"
    '        Me.lblMedexDescription.Style.Add("word-wrap", "break-word")
    '        Me.lblMedexDescription.Style.Add("white-space", "none")
    '        dc_Audit = New DataColumn("dModifyOnSubDtl_IST", System.Type.GetType("System.String"))
    '        ds_AuditTrail.Tables(0).Columns.Add("dModifyOnSubDtl_IST")
    '        ds_AuditTrail.AcceptChanges()
    '        For Each dr_Audit In ds_AuditTrail.Tables(0).Rows
    '            dr_Audit("dModifyOnSubDtl_IST") = Convert.ToString(CDate(dr_Audit("dModifyOnSubDtl")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
    '        Next
    '        ds_AuditTrail.AcceptChanges()
    '        dv_AuditTrail = ds_AuditTrail.Tables(0).DefaultView.ToTable(True, "iTranNo,vMedExResult,vModificationRemark,CRFSubDtlChangedBy,dModifyOnSubDtl,dModifyOnSubDtl_IST".Split(",")).DefaultView 'vSubjectId,iMySubjectNo,vMedExDesc
    '        'dv_AuditTrail.Sort = "iRepeatNo desc"

    '        Me.GVHistoryDtl.DataSource = dv_AuditTrail.ToTable()
    '        Me.GVHistoryDtl.DataBind()

    '        MedexGroupCode = ds_AuditTrail.Tables(0).Rows(0).Item("vMedExGroupCode").ToString.Trim()

    '        Return True
    '    Catch ex As Exception
    '        objCommon.ShowAlert(ex.Message + "....fillGrid", Me.Page())
    '        Return False
    '    End Try

    'End Function
    Private Function fillGrid(ByRef MedexGroupCode As String) As Boolean
        Dim ds_AuditTrail As New DataSet
        Dim dv_AuditTrail As New DataView
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim dc_Audit As DataColumn
        Dim ds As New DataSet
        Try
            If Me.HFCRFHdrNo.Value.Trim() = "" OrElse Me.ddlRepeatNo.SelectedItem.Value.Trim.ToUpper() = "N" Then
                Me.objCommon.ShowAlert("No History Available.", Me.Page())
                Return False
            End If

            Wstr = "nCRFHdrNo=" & Me.HFCRFHdrNo.Value.Trim() & " And vSubjectID = '" + Me.HFSubjectId.Value.Trim() +
                    "' And vMedExCode = '" + Me.hfMedexCode.Value.Substring(0, Me.hfMedexCode.Value.IndexOf("R")) +
                    "' And iNodeId=" + Me.HFNodeId.Value.Trim() + " And iRepeatNo = " + Me.hfMedexCode.Value.Substring(Me.hfMedexCode.Value.IndexOf("R") + 1)


            If Not Me.objHelp.GetData("View_CRFHdrDtlAuditTrail", "*", Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_AuditTrail, estr) Then
                Me.objCommon.ShowAlert("Problem While Getting Data Of Audit Trail. " + estr, Me.Page())
                Return False
            End If

            Me.lblMedexDescription.Text = ""

            If ds_AuditTrail.Tables(0).Rows.Count < 1 Then
                Return False
            End If

            Me.lblMedexDescription.EnableViewState = True
            Me.lblMedexDescription.Text = ds_AuditTrail.Tables(0).Rows(0).Item("vMedExDesc").ToString.Trim()
            Me.lblMedexDescription.Style.Add("word-wrap", "break-word")
            Me.lblMedexDescription.Style.Add("white-space", "none")

            dc_Audit = New DataColumn("dModifyOnSubDtl_IST", System.Type.GetType("System.String"))
            ds_AuditTrail.Tables(0).Columns.Add("dModifyOnSubDtl_IST")
            ds_AuditTrail.AcceptChanges()

            dc_Audit = New DataColumn("ActualTIME", System.Type.GetType("System.String"))
            ds_AuditTrail.Tables(0).Columns.Add(dc_Audit)
            ds_AuditTrail.AcceptChanges()

            For Each dr_Audit In ds_AuditTrail.Tables(0).Rows
                dr_Audit("dModifyOnSubDtl_IST") = Convert.ToString(CDate(dr_Audit("dModifyOnSubDtl")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)

                If Not objHelp.Proc_ActualAuditTrailTime(CDate(dr_Audit("dModifyOnSubDtl")).ToString("dd-MMM-yyyy HH:mm") + "##" + Session(S_LocationCode), ds, estr) Then
                    Throw New Exception(estr)
                End If
                dr_Audit("ActualTIME") = CDate(ds.Tables(0).Rows(0).Item("ActualTIME")).ToString("dd-MMM-yyyy HH:mm") + " (" + ds.Tables(0).Rows(0).Item("TimeZoneOffSet").ToString + " GMT)"
            Next
            ds_AuditTrail.AcceptChanges()
            'dv_AuditTrail = ds_AuditTrail.Tables(0).DefaultView.ToTable(True, "iTranNo,vMedExResult,vModificationRemark,CRFSubDtlChangedBy,dModifyOnSubDtl,dModifyOnSubDtl_IST,ActualTIME".Split(",")).DefaultView 'vSubjectId,iMySubjectNo,vMedExDesc
            'dv_AuditTrail.Sort = "iRepeatNo desc"

            If Me.GVHistoryDtl.Columns.Count > 5 Then
                Me.GVHistoryDtl.Columns.RemoveAt(GVHistoryDtl.Columns.Count - 1)
            End If


            Dim bf As BoundField = New BoundField
            Dim dc As DataColumn
            dc = New DataColumn(ds.Tables(0).Columns("ActualTIME").ColumnName)
            bf.DataField = dc.ColumnName
            bf.HeaderText = Session(S_TimeZoneName)
            GVHistoryDtl.Columns.Add(bf)

            Me.GVHistoryDtl.DataSource = ds_AuditTrail.Tables(0)
            Me.GVHistoryDtl.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fnAditTrail", "fnAditTrail(); ", True)
            MedexGroupCode = ds_AuditTrail.Tables(0).Rows(0).Item("vMedExGroupCode").ToString.Trim()

            Return True
        Catch ex As Exception
            objCommon.ShowAlert(ex.Message + "....fillGrid", Me.Page())
            Return False
        End Try

    End Function
    Private Function fillDCFGrid() As Boolean
        Dim ds_DCF As New DataSet
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim dc_Audit As DataColumn

        Try
            'If Me.HFCRFDtlNo.Value.Trim() = "" OrElse Me.ddlRepeatNo.SelectedItem.Value.Trim.ToUpper() = "N" Then
            '    Me.objCommon.ShowAlert("CRFDtlNo Not Available", Me.Page())
            '    Exit Function
            'End If

            'Wstr = "nCRFDtlNo=" & Me.HFCRFDtlNo.Value.Trim() & " And vMedExCode = '" + _
            '                        Me.hfMedexCode.Value.Substring(0, Me.hfMedexCode.Value.IndexOf("R")) + "' And cStatusIndi <> 'D'" '+ _
            ''" And iDCFBy = " + Me.ySession(S_UserID)

            If Me.HFCRFHdrNo.Value.Trim() = "" OrElse Me.ddlRepeatNo.SelectedItem.Value.Trim.ToUpper() = "N" Then
                Me.objCommon.ShowAlert("CRFHdrNo Not Available", Me.Page())
                Return False
            End If

            Wstr = "nCRFHdrNo=" & Me.HFCRFHdrNo.Value.Trim() & " And vSubjectID = '" + Me.HFSubjectId.Value.Trim() +
                    "' And vMedExCode = '" + Me.hfMedexCode.Value.Substring(0, Me.hfMedexCode.Value.IndexOf("R")) +
                    "' And iRepeatNo = " + Me.hfMedexCode.Value.Substring(Me.hfMedexCode.Value.IndexOf("R") + 1) + " And cStatusIndi <> 'D'"

            'If Not Me.objHelp.View_DCFMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '                 ds_DCF, estr) Then
            '    Throw New Exception(estr)
            'End If

            If Not objHelp.GetData("VIEW_DCFMst_Optimized", "*", Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DCF, estr) Then
                Throw New Exception(estr)
            End If

            Me.lblAttributeDCF.Text = ""
            'Me.lblAttributeDCF.CssClass = "Label"
            Me.lblAttributeDCF.Style.Add("word-wrap", "break-word")
            Me.lblAttributeDCF.Style.Add("white-space", "none")

            If ds_DCF.Tables(0).Rows.Count > 0 Then
                Me.lblAttributeDCF.Text = ds_DCF.Tables(0).Rows(0).Item("vMedExDesc").ToString.Trim()
                dc_Audit = New DataColumn("dDCFDate_IST", System.Type.GetType("System.String"))
                dc_Audit = New DataColumn("dStatusChangedOn_IST", System.Type.GetType("System.String"))

                ds_DCF.Tables(0).Columns.Add("dDCFDate_IST")
                ds_DCF.AcceptChanges()
                ds_DCF.Tables(0).Columns.Add("dStatusChangedOn_IST")
                ds_DCF.AcceptChanges()
                For index As Integer = 0 To ds_DCF.Tables(0).Rows.Count - 1

                    'ds_DCF.Tables(0).Rows(index).Item("dDCFDate_IST") = Convert.ToString(ds_DCF.Tables(0).Rows(index).Item("dDCFDate") + strServerOffset)

                    '--------------ADDED by PRATIK SONI

                    'Dim subStr As String = Convert.ToString(ds_DCF.Tables(0).Rows(index).Item("dDCFDate"))
                    'Dim subStr1 As String = subStr.Substring(0, subStr.LastIndexOf(":"))
                    'Dim subStr2 As String = subStr.Substring(18, 2)
                    'subStr = subStr1 + subStr2

                    ds_DCF.Tables(0).Rows(index).Item("dDCFDate_IST") = CDate(Convert.ToString(ds_DCF.Tables(0).Rows(index).Item("dDCFDate"))).ToString("dd-MMM-yyyy HH:mm") + strServerOffset

                    '------------------------------------------


                    If Not ds_DCF.Tables(0).Rows(index).Item("dStatusChangedOn").ToString() = "" Then
                        ds_DCF.Tables(0).Rows(index).Item("dStatusChangedOn_IST") = CDate(ds_DCF.Tables(0).Rows(index).Item("dStatusChangedOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset

                    End If
                Next
                ds_DCF.AcceptChanges()
            End If


            If ds_DCF.Tables(0).Rows.Count > 0 Then
                Me.GVWDCF.DataSource = ds_DCF
                Me.GVWDCF.DataBind()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fnDCF", "fnDCF(); ", True)
            Else
                Me.GVWDCF.DataSource = Nothing
                Me.GVWDCF.DataBind()

            End If
            Me.txtDiscrepancyRemarks.Text = ""
            Me.ddlDiscrepancyStatus.SelectedIndex = -1
            Me.ddlDiscrepancyStatus.Enabled = False

            Me.txtDiscrepancyRemarks.Enabled = True
            Me.btnUpdateDiscrepancy.Visible = False
            Me.btnSaveDiscrepancy.Visible = True

            'If Me.ViewState(VS_ReviewFlag) = "YES" AndAlso Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_OnlyView Then
            If Me.ViewState(VS_ReviewFlag) = "YES" AndAlso Me.Session(vs_workflowstageidfordynamic) <> WorkFlowStageId_OnlyView Then
                'Me.btnSaveDiscrepancy.Enabled = False
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..fillGrid")
            Return False
        End Try

    End Function

    Private Function fillQueryGrid() As Boolean
        Dim ds_EditChecksDtl As New DataSet
        Dim Wstr As String = String.Empty
        Dim str As String = String.Empty
        Dim estr As String = String.Empty

        Try
            'Wstr = "vWorkspaceId = '" + Me.HFWorkspaceId.Value.Trim() + "'" & _
            '        " and vSubjectId='" & Me.HFSubjectId.Value.Trim() & "' And bIsQuery = 1" & _
            '        " And iNodeId=" & Me.HFNodeId.Value.Trim()
            '''''Commented by pratiksha to solve the problem of show query as edit check operation page stores only period 1 for every editchecks
            '" and iPeriod=" & Me.HFPeriodId.Value.Trim() & 

            '" and nCRFDtlNo = " + Me.HFCRFDtlNo.Value.Trim() + "" & _
            'If Not Me.objHelp.View_EditChecksDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '                ds_EditChecksDtl, estr) Then
            '    Throw New Exception(estr)
            'End If

            str = Me.HFWorkspaceId.Value.Trim() + "##" + Me.HFSubjectId.Value.Trim() + "##1##" + Me.HFNodeId.Value.Trim()
            ds_EditChecksDtl = objHelp.ProcedureExecute("Proc_EditChecksDtl", str)

            Me.gvwQueries.DataSource = Nothing
            If Not ds_EditChecksDtl Is Nothing Then
                Me.gvwQueries.DataSource = ds_EditChecksDtl.Tables(0)
            End If
            Me.gvwQueries.DataBind()

            If Me.gvwQueries.Rows.Count > 0 Then
                Me.btnShowquery.Visible = True
                Me.btnShowquery.ToolTip = "No of queries on this page : " + Me.gvwQueries.Rows.Count.ToString()
                Me.btnShowquery.Text = "Show Query (" + Me.gvwQueries.Rows.Count.ToString() + ")"
                Me.btnShowquery.Style.Add("color", "Red")

                'To solve problem of query disply wrong number of queries
                'Me.btnShowquery.Attributes.Add("text", Me.gvwQueries.Rows.Count.ToString())
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...fillQueryGrid")
            Return False
        End Try

    End Function

    Private Function ShowNoOfQuery() As Boolean

        Dim dsReport As DataSet = Nothing
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty



        Try
            wStr = " vWorkspaceId='" + Me.HFWorkspaceId.Value + "' And iNodeId=" + Me.ddlActivities.SelectedValue.Split("##")(1) +
                   " And vactivityId='" + Me.ddlActivities.SelectedValue.Split("##")(0) + "' And vSubjectId='" + Me.HFSubjectId.Value +
                   "' And cResolvedFlag<>'Y' "

            If Not Me.objHelp.GetData("View_WorkspaceEditChecksHdrDTL", "*", wStr,
                                      WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                      dsReport, eStr) Then
                Throw New Exception("Error while get Information about EditChecks...ShowNoOfQuery")
            End If

            If Not dsReport Is Nothing Then
                If dsReport.Tables(0).Rows.Count > 0 Then
                    Me.GDVEditcheck.DataSource = Nothing
                    dsReport.Tables(0).Columns.Add("vFiredDate")
                    For Each dr In dsReport.Tables(0).Rows
                        If dr("dFiredDate").ToString() <> "" Then
                            dr("vFiredDate") = Convert.ToString(CDate(dr("dFiredDate")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                        End If
                    Next
                    Me.GDVEditcheck.DataSource = dsReport.Tables(0)
                    Me.GDVEditcheck.DataBind()
                    Me.imgbtnShowQuery.Style.Add("display", "")
                    Me.lblNoOfQuery.Style.Add("display", "")
                    Me.lblNoOfQuery.Text = "Query: (" + dsReport.Tables(0).Rows.Count.ToString() + ")"


                    Return True
                End If
            End If
            Me.imgbtnShowQuery.Style.Add("display", "none")
            Me.lblNoOfQuery.Style.Add("display", "none")
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

    '#################################################### For Edit Check Query Added By Vivek Patel '####################################################'
    Private Function ShowEditCheckQuery() As Boolean

        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim MedExCode As String = String.Empty
        Dim iRepeatNo As String = String.Empty
        'Dim dsEditCheck As DataSet = Nothing
        'Dim dtEditCheck As DataTable = Nothing
        'Dim dvEditCheck As DataView = Nothing
        Dim dsEditCheck As New DataSet
        Dim dtEditCheck As New DataTable

        Try

            If ViewState("EditCheckQuery") Is Nothing Then


                wStr = " vWorkspaceId='" + Me.HFWorkspaceId.Value + "' And iNodeId=" + Me.ddlActivities.SelectedValue.Split("##")(1) +
                  " And vactivityId='" + Me.ddlActivities.SelectedValue.Split("##")(0) + "' And vSubjectId='" + Me.HFSubjectId.Value +
                 "' And  cResolvedFlag<>'Y' "

                If Not Me.objHelp.GetData("View_WorkspaceEditChecksHdrDTL", "*", wStr,
                                  WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                  dsEditCheck, eStr) Then
                    Throw New Exception("Error while get Information about EditChecks...ShowEditCheckQuery")
                End If

                If Not dsEditCheck Is Nothing Then
                    If dsEditCheck.Tables(0).Rows.Count > 0 Then
                        Me.GDVEditcheckQuery.DataSource = Nothing
                        dsEditCheck.Tables(0).Columns.Add("vFiredDate")
                        For Each dr In dsEditCheck.Tables(0).Rows
                            If dr("dFiredDate").ToString() <> "" Then
                                dr("vFiredDate") = Convert.ToString(CDate(dr("dFiredDate")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                            End If
                        Next
                        dt_EditCheckQuery = dsEditCheck.Tables(0)
                        ViewState("EditCheckQuery") = dsEditCheck.Tables(0)
                    End If
                End If
                Me.divEditChecksQuery.Style.Add("display", "none")
            Else
                dt_EditCheckQuery = CType(ViewState("EditCheckQuery"), DataTable)
            End If

            If (Convert.ToString(hdnMedExCode.Value) <> "" Or Convert.ToString(hdnMedExCode.Value) <> Nothing Or Convert.ToString(hdnMedExCode.Value) <> String.Empty) And (Convert.ToString(hdniRepeatNo.Value) <> "" Or Convert.ToString(hdniRepeatNo.Value) <> Nothing Or Convert.ToString(hdniRepeatNo.Value) <> String.Empty) Then
                If dt_EditCheckQuery.Rows.Count > 0 Or Not dt_EditCheckQuery Is Nothing Then
                    MedExCode = hdnMedExCode.Value
                    iRepeatNo = hdniRepeatNo.Value
                    dtEditCheck = dt_EditCheckQuery
                    dtEditCheck.DefaultView.RowFilter = "vMedExCode = '" + MedExCode + "' and iRepeatNo = '" + iRepeatNo + "'"
                    hdnMedExCode.Value = String.Empty

                    If Not dtEditCheck Is Nothing Then
                        If dtEditCheck.Rows.Count > 0 Then
                            Me.GDVEditcheckQuery.DataSource = Nothing
                            For Each dr In dtEditCheck.Rows
                                If dr("dFiredDate").ToString() <> "" Then
                                    dr("vFiredDate") = Convert.ToString(CDate(dr("dFiredDate")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                                End If
                            Next
                            Me.GDVEditcheckQuery.DataSource = dtEditCheck
                            Me.GDVEditcheckQuery.DataBind()
                            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvClient", "UIgvClient(); ", True)

                        End If
                    End If
                End If
                Me.divEditChecksQuery.Style.Add("display", "none")
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function
    '###################################################################################################################################################################'

#End Region

#Region "DCFGrid Events"

    Protected Sub GVWDCF_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GVWDCF.RowCommand
        Dim rindex As Integer = e.CommandArgument
        Try
            If e.CommandName.ToUpper() = "UPDATE" Then

                Me.ViewState(VS_DCFNo) = Me.GVWDCF.Rows(rindex).Cells(GVCDCF_nDCFNo).Text.Trim()
                Me.txtDiscrepancyRemarks.Text = CType(Me.GVWDCF.Rows(rindex).FindControl("txtRemarks"), TextBox).Text
                Me.txtDiscrepancyRemarks.Enabled = True
                Me.ddlDiscrepancyStatus.Enabled = True

                If Me.GVWDCF.Rows(rindex).Cells(GVCDCF_UserType).Text.Trim() = Me.Session(S_UserType) Then

                    Me.ddlDiscrepancyStatus.SelectedIndex = 2
                    If Me.GVWDCF.Rows(rindex).Cells(GVCDCF_cDCFType).Text.Trim().ToUpper() = "SYSTEM" Then
                        Me.ddlDiscrepancyStatus.SelectedIndex = 3
                    End If

                Else
                    Me.ddlDiscrepancyStatus.SelectedIndex = 1
                    If Me.GVWDCF.Rows(rindex).Cells(GVCDCF_cDCFType).Text.Trim().ToUpper() = "SYSTEM" Then
                        Me.ddlDiscrepancyStatus.SelectedIndex = 3
                    End If
                    ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
                    If Me.Session(vs_workflowstageidfordynamic) = WorkFlowStageId_DataEntry Then
                        Me.ddlDiscrepancyStatus.SelectedValue = "O"
                    End If
                    If Me.GVWDCF.Rows(rindex).Cells(GVCDCF_cDCFType).Text.Trim().ToUpper() = "SYSTEM" AndAlso
                            Me.Session(vs_workflowstageidfordynamic) = WorkFlowStageId_DataEntry Then
                        Me.ddlDiscrepancyStatus.SelectedIndex = 3
                    End If
                    ''Commented by nipun khant for dynamic review
                    'If Me.Session(S_WorkFlowStageId) = WorkFlowStageId_DataEntry Then
                    '    Me.ddlDiscrepancyStatus.SelectedValue = "O"
                    'End If
                    'If Me.GVWDCF.Rows(rindex).Cells(GVCDCF_cDCFType).Text.Trim().ToUpper() = "SYSTEM" AndAlso _
                    '        Me.Session(S_WorkFlowStageId) = WorkFlowStageId_DataEntry Then
                    '    Me.ddlDiscrepancyStatus.SelectedIndex = 3
                    'End If
                End If

                Me.ddlDiscrepancyStatus.Enabled = False
                Me.btnUpdateDiscrepancy.Visible = True
                Me.btnSaveDiscrepancy.Visible = False

                '   btnUpdateDiscrepancy_Click(sender, e)

            End If

            'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "AnyDivShowHide('DCFSHOW');", True)
            'Me.MpeDCF.Show() 
            If Me.GVWDCF.Rows(rindex).Cells(GVCDCF_vUpdateRemarks).Text.Trim() <> "" AndAlso Me.GVWDCF.Rows(rindex).Cells(GVCDCF_vUpdateRemarks).Text.Trim() <> "&nbsp;" AndAlso Me.GVWDCF.Rows(rindex).Cells(GVCDCF_vUpdateRemarks).Text.Trim() <> "&amp;nbsp;" Then
                txtDCFUpdateRemarks.Text = Me.GVWDCF.Rows(rindex).Cells(GVCDCF_vUpdateRemarks).Text.Trim() ''changed by prayag
                btnUpdateRemarks_Click(Nothing, Nothing)
            Else
                Me.divDCF.Style.Add("display", "")
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "displayBackGround", "displayBackGround();", True)
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "getRemarks", "getRemarks();", True)

            End If

            'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "UnBindClick", "$('#divDCF').unbind('click');", True)

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Updating DCF. ", ex.Message)
        Finally
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fnDCF", "fnDCF(); ", True)
        End Try
    End Sub

    Protected Sub GVWDCF_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVWDCF.RowCreated
        If e.Row.RowType = DataControlRowType.Header Or
                e.Row.RowType = DataControlRowType.DataRow Or
                e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GVCDCF_nDCFNo).Visible = False
            e.Row.Cells(GVCDCF_nCRFDtlNo).Visible = False
            e.Row.Cells(GVCDCF_vMedExCode).Visible = False
            e.Row.Cells(GVCDCF_iDCFBy).Visible = False
            e.Row.Cells(GVCDCF_UserType).Visible = False
        End If
    End Sub

    Protected Sub GVWDCF_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVWDCF.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).CommandName = "UPDATE"

            ' '' Added on 12-May-2011 beacuse of aadding functionallity of Internally Resolved Functionallity ''
            'CType(e.Row.FindControl("LnkBtnInternallyResolved"), LinkButton).CommandArgument = e.Row.RowIndex
            'CType(e.Row.FindControl("LnkBtnInternallyResolved"), LinkButton).CommandName = "IR"
            ' '' ********************************************************************************************* ''
            e.Row.Cells(GVCDCF_iSrNo).Text = (e.Row.RowIndex + 1).ToString()

            If ((e.Row.RowIndex + 1) Mod 2 = 0) Then

                Dim TextRemarks As TextBox = DirectCast(e.Row.FindControl("txtRemarks"), TextBox)
                TextRemarks.BorderStyle = BorderStyle.None
                TextRemarks.BackColor = Drawing.Color.White
                TextRemarks.ForeColor = Drawing.Color.Navy
            Else
                Dim TextRemarks As TextBox = DirectCast(e.Row.FindControl("txtRemarks"), TextBox)
                TextRemarks.BorderStyle = BorderStyle.None
                TextRemarks.BackColor = System.Drawing.ColorTranslator.FromHtml("#cee3ed")
                TextRemarks.ForeColor = Drawing.Color.Navy

            End If

            If e.Row.Cells(GVCDCF_UserType).Text.Trim() = Me.Session(S_UserType).ToString() Then
                CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).Visible = True
            End If

            If e.Row.Cells(GVCDCF_cDCFStatus).Text.Trim.ToUpper() = Discrepancy_Answered And
                (e.Row.Cells(GVCDCF_iDCFBy).Text.Trim.ToUpper() <> Convert.ToString(Me.Session(S_UserID)).Trim.ToUpper() And
                 e.Row.Cells(GVCDCF_UserType).Text.Trim() <> Me.Session(S_UserType).ToString()) Then
                e.Row.Cells(GVCDCF_cDCFStatus).Text = "Answered"
                CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).Visible = False
            End If

            If e.Row.Cells(GVCDCF_cDCFStatus).Text.Trim.ToUpper() = Discrepancy_Generated Then
                e.Row.Cells(GVCDCF_cDCFStatus).Text = "Generated"

            ElseIf e.Row.Cells(GVCDCF_cDCFStatus).Text.Trim.ToUpper() = Discrepancy_Answered Then
                e.Row.Cells(GVCDCF_cDCFStatus).Text = "Answered"

            ElseIf e.Row.Cells(GVCDCF_cDCFStatus).Text.Trim.ToUpper() = Discrepancy_AutoResolved Then
                e.Row.Cells(GVCDCF_cDCFStatus).Text = "Auto Resolved"
                'CType(e.Row.FindControl("LnkBtnInternallyResolved"), LinkButton).Visible = False
                CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).Visible = False

            ElseIf e.Row.Cells(GVCDCF_cDCFStatus).Text.Trim.ToUpper() = Discrepancy_Resolved Then
                e.Row.Cells(GVCDCF_cDCFStatus).Text = "Resolved"
                CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).Visible = False

            ElseIf e.Row.Cells(GVCDCF_cDCFStatus).Text.Trim.ToUpper() = Discrepancy_InternallyResolved Then
                e.Row.Cells(GVCDCF_cDCFStatus).Text = " Internally Resolved"
                CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).Visible = False
            End If

            ''Added by nipun khant for dynamic review --> replace S_WorkFlowStageId->vs_workflowstageidfordynamic
            If e.Row.Cells(GVCDCF_UserType).Text.Trim() <> Me.Session(S_UserType).ToString() AndAlso
                Me.Session(vs_workflowstageidfordynamic) <> WorkFlowStageId_DataEntry Then
                CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).Visible = False
            End If
            ''Commented by nipun khant for dynamic review
            'If e.Row.Cells(GVCDCF_UserType).Text.Trim() <> Me.Session(S_UserType).ToString() AndAlso _
            '    Me.Session(S_WorkFlowStageId) <> WorkFlowStageId_DataEntry Then
            '    CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).Visible = False
            'End If

            If e.Row.Cells(GVCDCF_cDCFType).Text.Trim.ToUpper() = "S" Then
                e.Row.Cells(GVCDCF_cDCFType).Text = "System"
                e.Row.Cells(GVCDCF_vCreatedBy).Text = "System"
                If e.Row.Cells(GVCDCF_cDCFStatus).Text.Trim.ToUpper() = Discrepancy_AutoResolved Then
                    e.Row.Cells(GVCDCF_vUpdatedBy).Text = "System"
                End If
                'CType(e.Row.FindControl("lnkbtnUpdate"), LinkButton).Visible = False

            ElseIf e.Row.Cells(GVCDCF_cDCFType).Text.Trim.ToUpper() = "M" Then
                e.Row.Cells(GVCDCF_cDCFType).Text = "Manual"
            End If
        End If


    End Sub

    Protected Sub GVWDCF_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GVWDCF.RowUpdating

    End Sub

#End Region

#Region "AssignValuesForSaveAndContinue"

    Private Function AssignValuesForSaveAndContinue(ByVal SubjectId As String, ByVal WorkspaceId As String,
                                ByVal ActivityId As String, ByVal NodeId As String,
                                ByVal PeriodId As String,
                                ByVal MySubjectNo As String, ByRef DsSave As DataSet) As Boolean
        Dim DsCRFHdr As New DataSet
        Dim DsCRFDtl As New DataSet
        Dim DtCRFHdr As New DataTable
        Dim DtCRFDtl As New DataTable
        Dim DtCRFSubDtl As New DataTable

        Dim objCollection As ControlCollection
        Dim objControl As Control
        Dim ObjId As String = String.Empty
        Dim dr As DataRow
        Dim TranNo As Integer = 0
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim dsNodeInfo As New DataSet
        Dim NodeIndex As String = String.Empty
        Dim ds_DCF As New DataSet
        Dim dt_DCF As New DataTable
        Dim flg As Boolean = False
        Dim ControlDesc As String = String.Empty
        Dim ControlId As String = String.Empty

        Try
            Wstr = "vWorkSpaceId='" & Me.HFParentWorkspaceId.Value.Trim() & "' and iNodeId='" & NodeId & "'" &
                    " and vActivityId='" & ActivityId & "'"

            If Not Me.objHelp.getWorkSpaceNodeDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                            dsNodeInfo, estr) Then
                Me.objCommon.ShowAlert("Error while getting NodeIndex", Me)
                Return False
            End If
            NodeIndex = dsNodeInfo.Tables(0).Rows(0)("iNodeIndex").ToString()

            objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection) 'Me.PlaceMedEx.Controls 

            DtCRFHdr = CType(Me.ViewState(VS_DtCRFHdr), DataTable)
            DtCRFDtl = CType(Me.ViewState(VS_DtCRFDtl), DataTable)
            DtCRFSubDtl = CType(Me.ViewState(VS_DtCRFSubDtl), DataTable)

            '*********Checking MedEx values on 25-Dec-2009******************
            If Not Me.objHelp.GetDCFMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty,
                                    ds_DCF, estr) Then
                Return False
            End If
            Me.ViewState(VS_DtDCF) = ds_DCF.Tables(0).Copy()
            '*************************************

            If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                If Not Me.objHelp.GetCRFHdr("nCRFHdrNo=" & CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows(0)("nCRFHdrNo"),
                                                    WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                            DsCRFHdr, estr) Then
                    Return False
                End If
                DtCRFHdr = DsCRFHdr.Tables(0)

            Else
                DtCRFHdr.Clear()
                dr = DtCRFHdr.NewRow
                'nCRFHdrNo, vWorkSpaceId,dStartDate,iPeriod,iNodeId,iNodeIndex,vActivityId,cLockStatus
                dr("nCRFHdrNo") = 1
                If DtCRFHdr.Rows.Count > 0 Then
                    dr("nCRFHdrNo") = DtCRFHdr.Rows(0)("nCRFHdrNo")
                End If

                dr("vWorkSpaceId") = WorkspaceId
                'dr("dStartDate") = System.DateTime.Now
                dr("dStartDate") = CDate(objCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                dr("iPeriod") = PeriodId
                dr("iNodeId") = NodeId
                dr("iNodeIndex") = NodeIndex
                dr("vActivityId") = ActivityId
                dr("cLockStatus") = "U" 'cLockStatus
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dr("dModifyOn") = DateTime.Now()
                'dr.AcceptChanges()
                DtCRFHdr.Rows.Add(dr)
                DtCRFHdr.TableName = "CRFHdr"
                DtCRFHdr.AcceptChanges()
            End If

            If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                If Not Me.objHelp.GetCRFDtl("nCRFDtlNo=" & CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows(0)("nCRFDtlNo"),
                                                    WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                            DsCRFDtl, estr) Then
                    Return False
                End If
                DtCRFDtl = DsCRFDtl.Tables(0)

            Else
                DtCRFDtl.Clear()
                dr = DtCRFDtl.NewRow
                'nCRFDtlNo,nCRFHdrNo,iRepeatNo,dRepeatationDate,vSubjectId,iMySubjectNo,cLockStatus,iWorkFlowstageId
                dr("nCRFDtlNo") = 1
                If DtCRFDtl.Rows.Count > 0 Then
                    dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
                End If
                dr("nCRFHdrNo") = DtCRFHdr.Rows(0)("nCRFHdrNo").ToString.Trim()
                dr("iRepeatNo") = 1 'iRepeatNo
                dr("dRepeatationDate") = CDate(objCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                dr("vSubjectId") = SubjectId
                dr("iMySubjectNo") = MySubjectNo
                dr("cLockStatus") = "U" 'cLockStatus
                dr("iWorkFlowstageId") = Me.Session(S_WorkFlowStageId)
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dr("dModifyOn") = DateTime.Now()
                dr("cDataStatus") = CRF_DataEntryCompleted
                If CType(Me.ViewState(VS_DataStatus), String).ToUpper() = CRF_DataEntry Then
                    dr("cDataStatus") = CRF_DataEntry
                End If

                'dr.AcceptChanges()
                DtCRFDtl.Rows.Add(dr)
                DtCRFDtl.TableName = "CRFDtl"
                DtCRFDtl.AcceptChanges()
            End If

            DtCRFSubDtl.Clear()
            'For Detail Table
            For Each objControl In objCollection
                'nCRFSubDtlNo,nCRFDtlNo,iTranNo,vMedExCode,dMedExDatetime,vMedExResult,vModificationRemark

                If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.Label" AndAlso
                           objControl.ID.ToString.Trim().Contains("Lnk") Then
                    Dim objLbl As Label = CType(objControl, Label)
                    ControlId = objControl.ID.ToString.Replace("Lnk", "")
                    ControlDesc = objLbl.Text
                End If

                If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.TextBox" Then
                    TranNo += 1
                    If objControl.ID.ToString.Contains("txt") Then
                        ObjId = objControl.ID.ToString.Replace("txt", "")
                    Else
                        ObjId = objControl.ID.ToString.Trim()
                    End If

                    dr = DtCRFSubDtl.NewRow
                    dr("nCRFSubDtlNo") = TranNo
                    dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
                    dr("iTranNo") = TranNo
                    dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
                    dr("vMedExDesc") = ControlDesc.Trim.Substring(0, ControlDesc.Trim.Length - 1)
                    'SDNidhi
                    'dr("dMedExDateTime") = System.DateTime.Now
                    dr("dMedExDateTime") = CDate(objCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                    dr("vMedexResult") = Request.Form(objControl.ID) 'CType(objControl, TextBox).Text CType(Me.FindControl(obj.GetId), TextBox).Text
                    'dr("vMedexResult") = CType(objControl, TextBox).Text
                    dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "N"
                    dr("dModifyOn") = DateTime.Now()
                    'If Me.CheckDiscrepancy(objControl, ObjId, Request.Form(objControl.ID), "S") Then
                    '    dr("cStatusIndi") = "A"
                    'End If
                    DtCRFSubDtl.Rows.Add(dr)
                    DtCRFSubDtl.AcceptChanges()

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" Then
                    ObjId = objControl.ID.ToString.Trim()
                    DtCRFSubDtl.DefaultView.RowFilter = "vMedExCode = '" + ObjId.Substring(0, ObjId.IndexOf("R")).Trim() + "'"
                    If DtCRFSubDtl.DefaultView.ToTable.Rows.Count = 0 Then
                        TranNo += 1

                        dr = DtCRFSubDtl.NewRow
                        dr("nCRFSubDtlNo") = TranNo
                        dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
                        dr("iTranNo") = TranNo
                        dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
                        dr("vMedExDesc") = ControlDesc.Trim.Substring(0, ControlDesc.Trim.Length - 1)
                        'SDNidhi
                        'dr("dMedExDateTime") = System.DateTime.Now
                        dr("dMedExDateTime") = CDate(objCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                        '    dr("vMedexResult") = Request.Form(objControl.ID) 'CType(objControl, TextBox).Text 'CType(Me.FindControl(obj.GetId), TextBox).Text
                        dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
                        dr("iModifyBy") = Me.Session(S_UserID)
                        dr("cStatusIndi") = "N"
                        dr("dModifyOn") = DateTime.Now()
                        'If Me.CheckDiscrepancy(objControl, ObjId, Request.Form(objControl.ID), "S") Then
                        '    dr("cStatusIndi") = "A"
                        'End If
                        If DirectCast(objControl, System.Web.UI.WebControls.DropDownList).Attributes("StandardDate") = "Y" Then
                            flg = False
                            For Each objControl1 In objCollection
                                If Not objControl1.ID Is Nothing AndAlso objControl1.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" AndAlso objControl1.ID.ToString.Trim().Substring(0, objControl1.ID.ToString.Trim().IndexOf("R")) = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() Then
                                    If Request.Form(objControl1.ID) Is Nothing Then
                                        dr("vMedexResult") = Request.Form(objControl1.ID)
                                        'dr("vMedexResult") = CType(objControl1, DropDownList).Text
                                    ElseIf Request.Form(objControl1.ID) = "" Then
                                        flg = True
                                    Else
                                        dr("vMedexResult") += Request.Form(objControl1.ID)
                                        'dr("vMedexResult") += CType(objControl1, DropDownList).Text
                                    End If

                                End If
                            Next
                            dr("vMedexResult") = IIf(flg = True, "", dr("vMedexResult"))
                        Else

                            dr("vMedexResult") = Request.Form(objControl.ID)
                        End If

                        DtCRFSubDtl.Rows.Add(dr)
                        DtCRFSubDtl.AcceptChanges()
                    End If

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.FileUpload" Then
                    Dim filename As String = ""

                    TranNo += 1
                    If objControl.ID.ToString.Contains("FU") Then
                        ObjId = objControl.ID.ToString.Replace("FU", "")
                    Else
                        ObjId = objControl.ID.ToString.Trim()
                    End If

                    ''Modify by ketan
                    If CType(FindControl(objControl.ID), FileUpload).FileName = "" And
                        Not IsNothing(CType(FindControl("hlnk" + ObjId.Substring(0, ObjId.IndexOf("R")).Trim()), HyperLink)) AndAlso
                        CType(FindControl("hlnk" + ObjId.Substring(0, ObjId.IndexOf("R")).Trim()), HyperLink).NavigateUrl <> "" Then

                        filename = CType(FindControl("hlnk" + ObjId.Substring(0, ObjId.IndexOf("R")).Trim()), HyperLink).NavigateUrl

                    ElseIf Request.Files(objControl.ID).FileName <> "" Then

                        filename = "~/" + System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail") +
                                    WorkspaceId + "/" + NodeId + "/" + SubjectId + "/" + Path.GetFileName(Request.Files(objControl.ID).FileName.ToString.Trim())
                    End If

                    dr = DtCRFSubDtl.NewRow
                    dr("nCRFSubDtlNo") = TranNo
                    dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
                    dr("iTranNo") = TranNo
                    dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
                    dr("vMedExDesc") = ControlDesc.Trim.Substring(0, ControlDesc.Trim.Length - 1)
                    'SDNidhi
                    'dr("dMedExDateTime") = System.DateTime.Now
                    dr("dMedExDateTime") = CDate(objCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                    dr("vMedexResult") = filename
                    dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "N"
                    dr("dModifyOn") = DateTime.Now()
                    DtCRFSubDtl.Rows.Add(dr)
                    DtCRFSubDtl.AcceptChanges()

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.RadioButtonList" Then
                    TranNo += 1
                    ObjId = objControl.ID.ToString.Trim()
                    dr = DtCRFSubDtl.NewRow
                    dr("nCRFSubDtlNo") = TranNo
                    dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
                    dr("iTranNo") = TranNo
                    dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
                    dr("vMedExDesc") = ControlDesc.Trim.Substring(0, ControlDesc.Trim.Length - 1)
                    'SDNidhi
                    'dr("dMedExDateTime") = System.DateTime.Now
                    dr("dMedExDateTime") = CDate(objCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                    dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "N"
                    dr("dModifyOn") = DateTime.Now()
                    Dim rbl As RadioButtonList = CType(objControl, RadioButtonList)
                    Dim StrChk As String = ""

                    For index As Integer = 0 To rbl.Items.Count - 1
                        StrChk += IIf(rbl.Items(index).Selected, rbl.Items(index).Value.ToString.Trim() + ",", "")
                    Next index

                    If StrChk.Trim() <> "" Then
                        StrChk = StrChk.Substring(0, StrChk.LastIndexOf(","))
                    End If
                    dr("vMedexResult") = StrChk
                    'If Me.CheckDiscrepancy(objControl, ObjId, StrChk, "S") Then
                    '    dr("cStatusIndi") = "A"
                    'End If
                    DtCRFSubDtl.Rows.Add(dr)
                    DtCRFSubDtl.AcceptChanges()

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.CheckBoxList" Then
                    TranNo += 1
                    ObjId = objControl.ID.ToString.Trim()
                    dr = DtCRFSubDtl.NewRow
                    dr("nCRFSubDtlNo") = TranNo
                    dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
                    dr("iTranNo") = TranNo
                    dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
                    dr("vMedExDesc") = ControlDesc.Trim.Substring(0, ControlDesc.Trim.Length - 1)
                    'SDNidhi
                    'dr("dMedExDateTime") = System.DateTime.Now
                    dr("dMedExDateTime") = CDate(objCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                    dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "N"
                    dr("dModifyOn") = DateTime.Now()
                    Dim chkl As CheckBoxList = CType(objControl, CheckBoxList)
                    Dim StrChk As String = ""

                    For index As Integer = 0 To chkl.Items.Count - 1
                        StrChk += IIf(chkl.Items(index).Selected, chkl.Items(index).Value.ToString.Trim() + ",", "")
                    Next index

                    If StrChk.Trim() <> "" Then
                        StrChk = StrChk.Substring(0, StrChk.LastIndexOf(","))
                    End If
                    dr("vMedexResult") = StrChk
                    'If Me.CheckDiscrepancy(objControl, ObjId, StrChk, "S") Then
                    '    dr("cStatusIndi") = "A"
                    'End If
                    DtCRFSubDtl.Rows.Add(dr)
                    DtCRFSubDtl.AcceptChanges()

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.HiddenField" Then
                    TranNo += 1
                    ObjId = objControl.ID.ToString.Trim()
                    dr = DtCRFSubDtl.NewRow

                    '******Adding Header & footer to the document**********************

                    Dim ds_WorkSpaceNodeHistory As New DataSet
                    Dim filename As String = ""
                    Dim versionNo As String = ""
                    Wstr = "vWorkSpaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And iNodeId=" + NodeId.Trim() + " And iStageId =" & GeneralModule.Stage_Authorized

                    If Not objHelp.GetViewWorkSpaceNodeHistory(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                                            ds_WorkSpaceNodeHistory, estr) Then
                        Me.ShowErrorMessage("Error While Getting Data From View_WorkSpaceNodeHistory", estr)
                    End If

                    If Not ds_WorkSpaceNodeHistory.Tables(0).Rows.Count < 1 Then

                        filename = ds_WorkSpaceNodeHistory.Tables(0).Rows(0).Item("vDocPath").ToString()
                        versionNo = ds_WorkSpaceNodeHistory.Tables(0).Rows(0).Item("vUSerVersion").ToString()
                    End If

                    '*****************************************
                    ObjId = ObjId.Substring(0, ObjId.IndexOf("R")).Trim()
                    If ObjId = GeneralModule.Medex_FilePath.Trim() Then
                        dr("vMedexResult") = filename  'File Name from WorkspaceNodeHistory
                    ElseIf ObjId = GeneralModule.Medex_DownloadedBy.Trim() Then
                        dr("vMedexResult") = Me.Session(S_UserID)
                    ElseIf ObjId = GeneralModule.Medex_VersionNo.Trim() Then
                        dr("vMedexResult") = versionNo 'Version No from WorkspaceNodeHistory
                    ElseIf ObjId = GeneralModule.Medex_ReleaseDate.Trim() Then
                        dr("vMedexResult") = Date.Now.Date.ToString("dd-MMM-yyyy") 'Version No from WorkspaceNodeHistory
                    Else
                        dr("vMedexResult") = Request.Form(objControl.ID) 'CType(objControl, TextBox).Text 'CType(Me.FindControl(obj.GetId), TextBox).Text
                        'dr("vMedexResult") = CType(objControl, TextBox).Text
                    End If

                    dr("nCRFSubDtlNo") = TranNo
                    dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
                    dr("iTranNo") = TranNo
                    dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
                    dr("vMedExDesc") = ControlDesc.Trim.Substring(0, ControlDesc.Trim.Length - 1)
                    'SDNidhi
                    'dr("dMedExDateTime") = System.DateTime.Now
                    dr("dMedExDateTime") = CDate(objCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                    dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "N"
                    dr("dModifyOn") = DateTime.Now()
                    DtCRFSubDtl.Rows.Add(dr)
                    DtCRFSubDtl.AcceptChanges()

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.Label" AndAlso
                       objControl.ID.ToString.Trim().Contains("lbl") Then

                    Dim objLbl As Label = CType(objControl, Label)

                    If objLbl.CssClass.Contains("notsaved") Then
                        Continue For
                    End If

                    ControlId = objControl.ID.ToString.Replace("Lnk", "")
                    ControlDesc = objLbl.Text

                    TranNo += 1
                    ObjId = objControl.ID.ToString.Replace("lbl", "")
                    dr = DtCRFSubDtl.NewRow
                    dr("nCRFSubDtlNo") = TranNo
                    dr("nCRFDtlNo") = DtCRFDtl.Rows(0)("nCRFDtlNo")
                    dr("iTranNo") = TranNo
                    dr("vMedExCode") = ObjId.Substring(0, ObjId.IndexOf("R")).Trim() 'CType(objControl, TextBox).ID
                    dr("vMedExDesc") = ""
                    'SDNidhi
                    'dr("dMedExDateTime") = System.DateTime.Now
                    dr("dMedExDateTime") = CDate(objCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                    dr("vMedexResult") = ControlDesc.Trim()
                    dr("vModificationRemark") = "" 'Me.txtModificationRemark.text.trim()
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "N"
                    dr("dModifyOn") = DateTime.Now()
                    DtCRFSubDtl.Rows.Add(dr)
                    DtCRFSubDtl.AcceptChanges()

                End If
            Next objControl
            '****************************************

            DtCRFSubDtl.TableName = "CRFSubDtl"
            DtCRFSubDtl.AcceptChanges()

            DsSave = Nothing
            DsSave = New DataSet
            DsSave.Tables.Add(DtCRFHdr.Copy())
            DsSave.Tables.Add(DtCRFDtl.Copy())
            DsSave.Tables.Add(DtCRFSubDtl.Copy())
            DsSave.AcceptChanges()

            dt_DCF = CType(Me.ViewState(VS_DtDCF), DataTable).Copy()
            dt_DCF.TableName = "DCFMst"
            dt_DCF.AcceptChanges()
            DsSave.Tables.Add(dt_DCF.Copy())
            DsSave.AcceptChanges()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....AssignValuesForSaveAndContinue")
            Return False
        End Try

    End Function

#End Region

#Region "Activity DropDown Related"

    Private Function FillActivities() As Boolean
        Dim ds As New DataSet
        Dim dv As New DataView
        Dim ds_ActivityStatus As New DataSet
        Dim ds_review As New DataSet
        Dim dv_review As New DataView
        Dim dv_ActivityStatus As New DataView
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim clr As String = Drawing.ColorTranslator.ToHtml(Drawing.Color.Red)
        Dim cnt As Integer = 0
        Dim ds_Dependent As New DataSet
        Dim dtDependent As DataTable
        Dim strDependent As String = String.Empty
        Dim wStrDependent As String = String.Empty
        Dim ds_DependentValue As New DataSet

        Try
            'added by deepak Singh to merge with BA-BE
            '''''''''''Scop filter is removed as BABE projects also can see child activity in dropdown
            wStr = " vWorkspaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And vSubjectId ='" + Me.HFSubjectId.Value.Trim() + "' And (iParentNodeId = " + Me.HFParentNodeId.Value.Trim() + " Or iNodeId = " + Me.HFNodeId.Value.Trim() + " Or iNodeId = " + Me.HFParentNodeId.Value.Trim() + ")"
            If Me.HFType.Value = Type_BABE Then
                wStr = " vWorkspaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And iNodeId = " + Me.HFNodeId.Value.Trim()
            End If
            wStr += " AND cStatusindi <> 'D'"
            'Condition Added to identify subject specific study --Pratiksha
            If Me.HFSubjectId.Value.Trim() = "0000" AndAlso Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                wStr += " AND cSubjectWiseFlag = 'N'"
            ElseIf Me.HFSubjectId.Value.Trim() <> "0000" AndAlso Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                wStr += " AND cSubjectWiseFlag = 'Y'"
            End If
            '--Pratiksha

            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                If Not Session(S_WorkFlowStageId) = WorkFlowStageId_OnlyView Then
                    'If IsNothing(Me.Request.QueryString("UserTypeCode")) Then
                    '    wStr += "And vUserTypeCode = '" + Session(S_UserType) + "' "
                    'Else
                    '    wStr += "And vUserTypeCode = '" + Me.Request.QueryString("UserTypeCode").ToString + "' "
                    'End If
                    wStr += " And vUserTypeCode = '" + ViewState(S_UserType) + "' "
                    If Me.ViewState(S_UserType) = Me.hdnuserprofile.Value Then
                        wStr += " And iUserId = '" + Session(S_UserID) + "' "
                    End If
                    'If IsNothing(Me.Request.QueryString("UserId")) Then
                    '    wStr += "And iUserId = '" + Session(S_UserID) + "' "
                    'Else
                    '    wStr += "And iUserId = '" + Me.Request.QueryString("UserId").ToString + "' "
                    'End If

                    wStr += " Order by iNodeNo "
                    If Not Me.objHelp.GetViewWorkSpaceUserNodeDetail(wStr, ds, eStr) Then
                        Throw New Exception(eStr)
                    End If
                Else
                    wStr += " Order by iNodeNo "
                    If Not Me.objHelp.GetViewWorkSpaceUserNodeDetail(wStr, ds, eStr) Then
                        Throw New Exception(eStr)
                    End If
                End If
                    Else
                wStr += " Order by iNodeNo "
                If Not Me.objHelp.GetViewWorkSpaceNodeDetail(wStr, ds, eStr) Then
                    Throw New Exception(eStr)
                End If
            End If

            If ds.Tables(0).Rows.Count = 0 Then
                objCommon.ShowAlert("User have No Activity Rights.", Me.Page)

                'Me.ShowErrorMessage("User have No Activity Rights.", ".......FillActivities")
                Return False
            End If

            'ds.Tables(0).Columns.Add("ActivityNodeId")
            'ds.AcceptChanges()
            'For Each dr As DataRow In ds.Tables(0).Rows
            '    dr("ActivityNodeId") = dr("vActivityId").ToString() + "#" + dr("iNodeId").ToString()
            'Next
            'ds.AcceptChanges()

            dv = ds.Tables(0).DefaultView

            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                dv.RowFilter = "iParentNodeId = " + Me.HFParentNodeId.Value.Trim()
                If dv.ToTable().Rows.Count < 1 Then
                    dv = ds.Tables(0).DefaultView
                    dv.RowFilter = "iNodeId = " + Me.HFNodeId.Value.Trim()
                End If

                'Added By Bhargav Thaker 27March2023
                If Session(S_WorkFlowStageId) = WorkFlowStageId_OnlyView Then
                    dv.RowFilter += " AND vUserTypeCode = '" + ViewState(S_UserType) + "' "
                End If
            End If

            'Added By Vimal Ghoniya For Activity Depedency
            wStr = "SELECT MedExDependency.* from MedExDependency INNER JOIN WorkspaceMst " +
                    " ON((WorkspaceMst.vParentWorkspaceId = MedExDependency.vWorkspaceId " +
                    "   OR WorkspaceMst.vWorkspaceId = MedExDependency.vWorkspaceId) " +
                    "   AND WorkspaceMst.cStatusIndi <> 'D') " +
                    " WHERE WorkspaceMst.vWorkspaceId = '" + Me.HFWorkspaceId.Value.Trim() + "'" +
                    " AND MedExDependency.cDependencyType = 'A' AND MedExDependency.cStatusIndi <> 'D'"

            ds_Dependent = Me.objHelp.GetResultSet(wStr, "MedExDependency")

            If Not ds_Dependent Is Nothing Then
                If ds_Dependent.Tables(0).Rows.Count > 0 Then
                    For Each dr As DataRow In ds_Dependent.Tables(0).Rows
                        wStrDependent = "(SELECT ISNULL(vMedexResult,'') " +
                                        " FROM CRFHdr INNER JOIN CRFDtl ON(CRFHDR.nCRFHdrNo=CRFDtl.nCRFHdrNo " +
                                        " AND CRFDtl.cStatusIndi <> 'D' ) INNER JOIN CRFSubDtl ON(CRFDtl.nCRFDtlNo=CRFSubDtl.nCRFDtlNo " +
                                        " AND CRFSubDtl.cStatusIndi <> 'D' ) INNER JOIN(SELECT nCRFDtlNo,vMedExCode,MAX(iTranNo) AS MaxTranNo " +
                                        " FROM CRFSubDtl WHERE CRFSubDtl.cStatusIndi <> 'D' GROUP BY nCRFDtlNo,vMedExCode)CRFSubDtlMax " +
                                        " ON(CRFSubDtl.vMedExCode= CRFSubDtlMax.vMedExCode AND CRFSubDtl.iTranNo= CRFSubDtlMax.MaxTranNo " +
                                        " AND CRFSubDtl.nCRFDtlNo = CRFSubDtlMax.nCRFDtlNo) " +
                                        " WHERE CRFHdr.cStatusIndi <> 'D' AND " +
                                        " CRFHdr.vWorkSpaceId= '" + Me.HFWorkspaceId.Value.Trim() + "' AND CRFHdr.iNodeId = " + dr("iSourceNodeId").ToString + "" +
                                        " AND CRFDtl.vSubjectId= '" + Me.HFSubjectId.Value.Trim() + "' " +
                                        " AND CRFDtl.iRepeatNo=1 AND CRFSubDtl.vMedExCode='" + dr("vSourceMedExCode").ToString + "' )"

                        ds_DependentValue = Me.objHelp.GetResultSet(wStrDependent, "DependentValue")

                        If Not ds_DependentValue Is Nothing Then
                            If ds_DependentValue.Tables(0).Rows.Count > 0 Then
                                If ds_DependentValue.Tables(0).Rows(0)(0).ToString.ToLower = dr("vMedExValue").ToString.ToLower Then
                                    Continue For
                                End If
                            End If
                        End If
                        strDependent += "'" + dr("vTargetActivityId").ToString + "#" + dr("iTargetNodeId").ToString + "',"
                    Next
                End If
            End If

            If strDependent <> "" Then
                strDependent = strDependent.Substring(0, strDependent.LastIndexOf(","))
                dtDependent = dv.ToTable()
                dv = dtDependent.DefaultView
            End If
            'End

            'Added By Bhargav Thaker 27March2023
            If ds.Tables(0).Rows.Count <> 0 Then
                Dim count As Integer = 0
                For Each rowView As DataRowView In dv
                    Dim row As DataRow = rowView.Row
                    count = count + 1
                    If count > 1 Then
                        row.Delete()
                        row.AcceptChanges()
                    End If
                Next
            End If

            Me.ddlActivities.DataSource = dv.ToTable()
            Me.ddlActivities.DataTextField = "vNodeDisplayName"
            Me.ddlActivities.DataValueField = "ActivityDisplayId"
            Me.ddlActivities.DataBind()

            For Count As Integer = 0 To Me.ddlActivities.Items.Count - 1
                Me.ddlActivities.Items(Count).Attributes.Add("title", Me.ddlActivities.Items(Count).Text.Trim())
            Next Count

            If Not Me.Session(S_SelectedActivity + CurrentPageSessionVariable) Is Nothing Then
                Me.ddlActivities.SelectedIndex = CType(Me.Session(S_SelectedActivity + CurrentPageSessionVariable), Integer)
            End If

            If Selectvar <> "" Then
                Me.ddlActivities.SelectedIndex = Selectvar.ToString()
            End If

            Me.HFActivityId.Value = Me.ddlActivities.SelectedItem.Value.Substring(0, 4)
            Me.HFNodeId.Value = Me.ddlActivities.SelectedItem.Value.Substring(5)
            Me.hdnActivityName.Value = Me.ddlActivities.SelectedItem.Text

            Me.Session(S_SelectedActivity + CurrentPageSessionVariable) = Nothing

            'Add by shivani for Tabuler repetition   
            Me.Session(S_tabuler) = Nothing
            Me.Session(S_tabuler) = dv.ToTable()

            For Each dr As DataRow In dv.ToTable().Rows
                If Not Me.ddlActivities.Items.FindByValue(dr("vActivityId").ToString() + "#" + dr("iNodeId").ToString()) Is Nothing Then
                    Me.ddlActivities.Items.FindByValue(dr("vActivityId").ToString() + "#" + dr("iNodeId").ToString()).Attributes.Add("style", "color:Red")
                    Me.ddlActivities.Style.Add("color", "Red")
                End If
            Next

            '************Repeatation Show/Hode logic******************

            Me.Session("RepeationShowHide") = ds.Tables(0).DefaultView
            dv = ds.Tables(0).DefaultView
            dv.RowFilter = "vActivityId = '" + Me.HFActivityId.Value.Trim() + "' And iNodeId = " + Me.HFNodeId.Value.Trim()
            Me.imgViewAll.Style.Add("display", "none")
            Me.ddlRepeatNo.Style.Add("display", "none")
            If dv.ToTable().Rows.Count > 0 Then
                If Convert.ToString(dv.ToTable().Rows(0)("cIsRepeatable")).Trim.ToUpper() = "Y" Then
                    Me.ddlRepeatNo.Style.Add("display", "")
                    Me.imgViewAll.Style.Add("display", "")
                    btnGridViewDisplay.Visible = True   ''Added by Shivani for Tabular Repetition management changes
                End If
                If Convert.ToString(dv.ToTable().Rows(0)("cIsRepeatable")).Trim.ToUpper() = "N" Then
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "RepeatitionShowHide", "RepeatitionShowHide();", True)
                End If
            End If
            '******************************

            '****************Setting colors to activities for displaying their status**************
            'added by deepak singh to merge in BA-BE

            wStr = "vWorkspaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And iPeriod = " + Me.HFPeriodId.Value.Trim()
            wStr += " And (iParentNodeId = " + Me.HFParentNodeId.Value.Trim() + " Or iNodeId = " + Me.HFNodeId.Value.Trim() + ") And vSubjectId = '" + Me.HFSubjectId.Value.Trim() + "'"

            If Me.Session(S_ScopeNo) = Scope_BABE Then
                wStr = "vWorkspaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And iPeriod = " + Me.HFPeriodId.Value.Trim() + " And iNodeid = " + Me.HFNodeId.Value.Trim() + " And vSubjectId = '" + Me.HFSubjectId.Value.Trim() + "'"
            End If

            If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                wStr = "vWorkspaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And iPeriod = " + Me.HFPeriodId.Value.Trim()
                wStr += " And (iParentNodeId = " + Me.HFParentNodeId.Value.Trim() + " Or iNodeId = " + Me.HFParentNodeId.Value.Trim() + ") And vSubjectId = '" + Me.HFSubjectId.Value.Trim() + "'"
            End If
            '=================================

            'If Not Me.objHelp.View_CRFActivityStatus(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '                    ds_ActivityStatus, eStr) Then
            '    Throw New Exception(eStr)
            'End If

            If Not Me.objHelp.View_CRFActivityStatus_New(wStr, "*,DENSE_RANK() OVER(PARTITION BY nCRFHdrNo,vActivityId,vSubjectId ORDER BY nCRFHdrNo,vActivityId,vSubjectId,iRepeatNo) as [RepetitionNo] ", ds_ActivityStatus, eStr) Then
                Throw New Exception(eStr)
            End If

            'wStr = Me.HFWorkspaceId.Value.Trim() + "##" + Me.HFActivityId.Value.Trim() + "##" + Me.HFSubjectId.Value.Trim() + "##" + Me.HFPeriodId.Value.Trim() + "##" + Me.HFNodeId.Value.Trim() + "##" + hndRepetitionNo.Value.Trim()
            'ds_ActivityStatus = objHelp.ProcedureExecute("Proc_SelectedRepeatition", wStr)

            Me.HFReviewedWorkFlowId.Value = ""
            Me.HFImportedDataWorkFlowId.Value = ""
            Me.lblLastReviewedBy.Text = ""

            '  Me.ddlActivities.ForeColor = Drawing.Color.Red
            ''Added by nipun khant for dynamic review
            ds_review = CType(Me.Session(Vs_dsReviewerlevel), DataSet)
            Me.Session(S_Review) = CType(Me.Session(Vs_dsReviewerlevel), DataSet)
            '======================================================
            If Not ds_ActivityStatus Is Nothing Then

                dv_ActivityStatus = ds_ActivityStatus.Tables(0).DefaultView

                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                    dv_ActivityStatus.RowFilter = "iParentNodeId = " + Me.HFParentNodeId.Value.Trim()
                    If dv_ActivityStatus.ToTable().Rows.Count < 1 Then
                        dv_ActivityStatus = ds_ActivityStatus.Tables(0).DefaultView
                        dv_ActivityStatus.RowFilter = "iNodeId = " + Me.HFNodeId.Value.Trim()
                    End If
                End If

                'For tabuler Repetition
                Me.Session(S_TabulerActivity) = Nothing
                Me.Session(S_TabulerActivity) = dv_ActivityStatus.ToTable()

                For Each dr As DataRow In dv_ActivityStatus.ToTable().Rows

                    'If Convert.ToString(dr("cCRFDtlDataStatus")).Trim.ToUpper() = CRF_DataEntryPending Then
                    '    clr = Drawing.ColorTranslator.ToHtml(Drawing.Color.Red)
                    'Else
                    If Convert.ToString(dr("cCRFDtlDataStatus")).Trim.ToUpper() = CRF_DataEntry Then
                        clr = Drawing.ColorTranslator.ToHtml(Drawing.Color.Orange)
                        'ElseIf Convert.ToString(dr("cCRFDtlDataStatus")).Trim.ToUpper() = CRF_DataEntryCompleted Then

                    ElseIf Convert.ToString(dr("cCRFDtlDataStatus")).Trim.ToUpper() = CRF_Review Then
                        clr = Drawing.ColorTranslator.ToHtml(Drawing.Color.Blue)
                        ''Added by nipun khant for dynamic review
                        ''=========================================================================
                        ''==================Added by Shyam Kamdar =====================
                    ElseIf Convert.ToString(dr("cCRFDtlDataStatus")).Trim.ToUpper() = CRF_ReviewCompleted Then
                        clr = Drawing.ColorTranslator.ToHtml(Drawing.Color.Blue)
                    Else
                        dv_review = ds_review.Tables(0).Copy.DefaultView
                        dv_review.RowFilter = "iReviewWorkflowStageId=" + Convert.ToString(dr("iWorkFlowStageId")).Trim()
                        clr = Drawing.ColorTranslator.ToHtml(Drawing.Color.Red)
                        'If dv_review.ToTable.Rows.Count > 0 Then
                        '    clr = Convert.ToString(dv_review.ToTable.Rows(0)("vColorCodeForDynamic"))
                        'End If
                        ''=========================================================================
                        ''Commented by nipun khant for dynamic review
                        ''=========================================================================
                        'ElseIf Convert.ToString(dr("cCRFDtlDataStatus")).Trim.ToUpper() = CRF_ReviewCompleted Then
                        '    If Convert.ToString(dr("iWorkFlowStageId")).Trim() = WorkFlowStageId_FirstReview Then
                        '        clr = "#50C000"
                        '    ElseIf Convert.ToString(dr("iWorkFlowStageId")).Trim() = WorkFlowStageId_SecondReview Then
                        '        clr = "#006000"
                        '    End If
                        'ElseIf Convert.ToString(dr("cCRFDtlDataStatus")).Trim.ToUpper() = CRF_Locked Then
                        '    clr = Drawing.ColorTranslator.ToHtml(Drawing.Color.Gray)
                        ''=========================================================================
                    End If

                    If Not Me.ddlActivities.Items.FindByValue(dr("vActivityId").ToString() + "#" + dr("iNodeId").ToString()) Is Nothing Then
                        Me.ddlActivities.Items.FindByValue(dr("vActivityId").ToString() + "#" + dr("iNodeId").ToString()).Attributes.Add("style", "color:" + clr)
                        If Me.ddlActivities.SelectedValue = dr("vActivityId").ToString() + "#" + dr("iNodeId").ToString() Then
                            Me.ddlActivities.Style.Add("color", clr)
                        End If
                    End If



                    '***********Reviewed By Displaying Logic********

                    'Commented because color codes are displayed for indication
                    'If Convert.ToString(dr("CRFReviewedBy")).Trim() <> "" Then

                    '    Me.ddlActivities.Items.FindByValue(dr("vActivityId").ToString() + "#" + dr("iNodeId").ToString()).Attributes.Add("title", "Last Review Done By : " + Convert.ToString(dr("CRFReviewedBy")).Trim() + " - " + Convert.ToString(dr("CRFReviewedByUserType")).Trim())

                    'End If

                    '***********************************************
                Next

                'Me.ddlActivities.ForeColor = System.Drawing.Color.FromName(ddlActivities.SelectedItem.Text)

                'Add by shivani pandya
                If Me.Session(S_TabulerRepeatition) <> "AllData" Then
                    If Me.Session(S_GetLetestData + CurrentPageSessionVariable + hdnActivityName.Value) <> "getData" Then
                        If Me.hndRepetitionNo.Value.Trim() <> "" And Me.hndRepetitionNo.Value.Trim() <> "N" Then
                            Me.Session(S_SelectedRepeatation + CurrentPageSessionVariable + hdnActivityName.Value) = Nothing
                            Me.Session(S_SelectedRepeatation + CurrentPageSessionVariable + hdnActivityName.Value) = Me.hndRepetitionNo.Value.Trim()
                        Else
                            If Me.ddlRepeatNo.SelectedValue <> "N" Then
                                Me.hndRepetitionNo.Value = Me.Session(S_SelectedRepeatation + CurrentPageSessionVariable + hdnActivityName.Value)
                            End If
                        End If
                    End If
                End If

                Me.ViewState(VS_DataStatus) = CRF_DataEntryPending

                dv_ActivityStatus.RowFilter = "iNodeId = " + Me.HFNodeId.Value.Trim()
                'Add by shivani pandya for open that dynamic page from form
                If hndRepetitionNo.Value = "" Then
                    If dv_ActivityStatus.ToTable().Rows.Count > 0 Then
                        hndRepetitionNo.Value = dv_ActivityStatus.ToTable().Compute("Max(RepetitionNo)", String.Empty)
                    End If
                End If
                If dv_ActivityStatus.ToTable().Rows.Count > 0 Then

                    If Not hndRepetitionNo.Value Is Nothing AndAlso Convert.ToString(Me.hndRepetitionNo.Value.ToUpper()) <> "N" Then
                        dv_ActivityStatus.RowFilter = "iNodeId = " + Me.HFNodeId.Value.Trim() + " And RepetitionNo In ( " & IIf(hndRepetitionNo.Value.Trim() = "", dv_ActivityStatus.ToTable().Compute("Max(RepetitionNo)", String.Empty), hndRepetitionNo.Value.Trim()) & ")"
                        If dv_ActivityStatus.ToTable().Rows.Count > 0 Then
                            Me.HFReviewedWorkFlowId.Value = dv_ActivityStatus.ToTable().Rows(0)("iReviewedWorkFlowStageId")
                            Me.HFImportedDataWorkFlowId.Value = dv_ActivityStatus.ToTable().Rows(0)("iWorkFlowStageId")
                            Me.ViewState(VS_DataStatus) = dv_ActivityStatus.ToTable().Rows(0)("cCRFDtlDataStatus")
                            If Convert.ToString(dv_ActivityStatus.ToTable().Rows(0)("CRFReviewedBy")).Trim() <> "" AndAlso Convert.ToString(dv_ActivityStatus.ToTable().Rows(0)("cCRFDtlDataStatus")).Trim() <> CRF_Review Then
                                Me.lblLastReviewedBy.Text = "Last Review Done By : " + Convert.ToString(dv_ActivityStatus.ToTable().Rows(0)("CRFReviewedBy")).Trim() + " - " + Convert.ToString(dv_ActivityStatus.ToTable().Rows(0)("CRFReviewedByUserType")).Trim()
                            End If
                        End If
                    Else
                        'dv_ActivityStatus.Sort = "iRepeatNo asc"
                        'Me.HFReviewedWorkFlowId.Value = dv_ActivityStatus.ToTable().Rows(0)("iReviewedWorkFlowStageId")
                        dv_ActivityStatus.Sort = "RepetitionNo,dModifyOnWorkflowDtl Desc"   ''Added By Rahul Shah
                        Me.HFReviewedWorkFlowId.Value = dv_ActivityStatus.ToTable().Rows(0)("iReviewedWorkFlowStageId")
                        Me.HFImportedDataWorkFlowId.Value = dv_ActivityStatus.ToTable().Rows(0)("iWorkFlowStageId")
                        Me.ViewState(VS_DataStatus) = dv_ActivityStatus.ToTable().Rows(0)("cCRFDtlDataStatus")
                        If Convert.ToString(dv_ActivityStatus.ToTable().Rows(0)("CRFReviewedBy")).Trim() <> "" AndAlso Convert.ToString(dv_ActivityStatus.ToTable().Rows(0)("cCRFDtlDataStatus")).Trim() <> CRF_Review Then
                            Me.lblLastReviewedBy.Text = "Last Review Done By : " + Convert.ToString(dv_ActivityStatus.ToTable().Rows(0)("CRFReviewedBy")).Trim() + " - " + Convert.ToString(dv_ActivityStatus.ToTable().Rows(0)("CRFReviewedByUserType")).Trim()
                        End If
                    End If
                End If
            End If
            '*****************end of setting Activity status****************

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".......FillActivities")
            Return False
        End Try

    End Function

    Private Function ActivityDropdownColor() As Boolean
        Dim dtTemp As New DataTable
        Dim dt_ActivityStatusTemp As New DataTable
        Dim clr As String = Drawing.ColorTranslator.ToHtml(Drawing.Color.Red)
        Dim dv_review As DataView
        Dim ds_review As New DataSet
        Try
            dtTemp = Me.Session(S_tabuler)
            For Each dr As DataRow In dtTemp.Rows
                If Not Me.ddlActivities.Items.FindByValue(dr("vActivityId").ToString() + "#" + dr("iNodeId").ToString()) Is Nothing Then
                    Me.ddlActivities.Items.FindByValue(dr("vActivityId").ToString() + "#" + dr("iNodeId").ToString()).Attributes.Add("style", "color:Red")
                    Me.ddlActivities.Style.Add("color", "Red")
                End If
            Next

            dt_ActivityStatusTemp = Me.Session(S_TabulerActivity)
            ''added by nipun khant for dynamic review
            ds_review = CType(Me.Session(Vs_dsReviewerlevel), DataSet)
            '======================================================
            For Each dr As DataRow In dt_ActivityStatusTemp.Rows

                'If Convert.ToString(dr("cCRFDtlDataStatus")).Trim.ToUpper() = CRF_DataEntryPending Then
                '    clr = Drawing.ColorTranslator.ToHtml(Drawing.Color.Red)
                'Else
                If Convert.ToString(dr("cCRFDtlDataStatus")).Trim.ToUpper() = CRF_DataEntry Then
                    clr = Drawing.ColorTranslator.ToHtml(Drawing.Color.Orange)
                    'ElseIf Convert.ToString(dr("cCRFDtlDataStatus")).Trim.ToUpper() = CRF_DataEntryCompleted Then

                ElseIf Convert.ToString(dr("cCRFDtlDataStatus")).Trim.ToUpper() = CRF_Review Then
                    clr = Drawing.ColorTranslator.ToHtml(Drawing.Color.Blue)
                    ''Added by nipun khant for dynamic review
                    ''=========================================================================
                    ''==================Added by Shyam Kamdar =====================
                ElseIf Convert.ToString(dr("cCRFDtlDataStatus")).Trim.ToUpper() = CRF_ReviewCompleted Then
                    clr = Drawing.ColorTranslator.ToHtml(Drawing.Color.Blue)
                Else
                    dv_review = ds_review.Tables(0).Copy.DefaultView
                    dv_review.RowFilter = "iReviewWorkflowStageId=" + Convert.ToString(dr("iWorkFlowStageId")).Trim()
                    clr = Drawing.ColorTranslator.ToHtml(Drawing.Color.Red)
                    'If dv_review.ToTable.Rows.Count > 0 Then
                    '    clr = Convert.ToString(dv_review.ToTable.Rows(0)("vColorCodeForDynamic"))
                    'End If
                    ''=========================================================================
                    ''Commented by nipun khant for dynamic review
                    ''=========================================================================
                    'ElseIf Convert.ToString(dr("cCRFDtlDataStatus")).Trim.ToUpper() = CRF_ReviewCompleted Then
                    '    If Convert.ToString(dr("iWorkFlowStageId")).Trim() = WorkFlowStageId_FirstReview Then
                    '        clr = "#50C000"
                    '    ElseIf Convert.ToString(dr("iWorkFlowStageId")).Trim() = WorkFlowStageId_SecondReview Then
                    '        clr = "#006000"
                    '    End If
                    'ElseIf Convert.ToString(dr("cCRFDtlDataStatus")).Trim.ToUpper() = CRF_Locked Then
                    '    clr = Drawing.ColorTranslator.ToHtml(Drawing.Color.Gray)
                End If

                If Not Me.ddlActivities.Items.FindByValue(dr("vActivityId").ToString() + "#" + dr("iNodeId").ToString()) Is Nothing Then
                    Me.ddlActivities.Items.FindByValue(dr("vActivityId").ToString() + "#" + dr("iNodeId").ToString()).Attributes.Add("style", "color:" + clr)
                    If Me.ddlActivities.SelectedValue = dr("vActivityId").ToString() + "#" + dr("iNodeId").ToString() Then
                        Me.ddlActivities.Style.Add("color", clr)
                    End If
                End If
            Next

        Catch ex As Exception

        End Try
    End Function

    Protected Sub ddlActivities_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlActivities.SelectedIndexChanged
        Dim StrRedirect As String = String.Empty
        Dim choice As String = String.Empty
        Dim wstr As String = String.Empty
        Dim ds_DataEntryControl As DataSet = Nothing
        Dim estr As String = String.Empty
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim Return_Val As Char
        Try
            'add by shivani pandya for all repeatition
            If Me.Session(S_GetLetestData + CurrentPageSessionVariable + hdnActivityName.Value) <> "getData" Then
                Me.Session(S_GetLetestData + CurrentPageSessionVariable + hdnActivityName.Value) = ""
            End If
            If Not Getstring(StrRedirect) Then
                Me.objCommon.ShowAlert("Error occured while getting string.", Me.Page)
                Exit Sub
            End If
            If sender.GetType.Name <> "Button" Then
                Me.Session(S_SelectedRepeatation + CurrentPageSessionVariable + hdnActivityName.Value) = Nothing
                flagDCF = True
                Me.hndRepetitionNo.Value = ""
            End If

            If IsNothing(Me.Request.QueryString("From")) Then

                If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    wstr = "select DataEntryControl.vWorkspaceId,DataEntryControl.iNodeId,DataEntryControl.vSubjectId,DataEntryControl.imodifyBy," +
                            "iWorkFlowStageId,UserMST.vFirstName  + ' '  +UserMST.vLastName as vUserName from DataEntryControl " +
                            "inner join UserMst on (DataEntryControl.imodifyBy=UserMst.iUserId)   " +
                            "Where vSubjectId='" & Me.Request.QueryString("SubjectId").ToString & "' And vWorkspaceId='" & Me.HFWorkspaceId.Value & "' And iNodeId=" & Me.ddlActivities.SelectedValue.Split("#")(1) + " and iWorkFlowStageId = " + Session(S_WorkFlowStageId)

                    ds_DataEntryControl = objHelp.GetResultSet(wstr, "DataEntryControl")

                    If ds_DataEntryControl.Tables(0).Rows.Count > 0 Then


                        If ds_DataEntryControl.Tables(0).Rows(0).Item("iModifyBy") <> Session(S_UserID) AndAlso ds_DataEntryControl.Tables(0).Rows(0).Item("iWorkFlowStageId") = Session(S_WorkFlowStageId) Then
                            Me.lblDataEntrycontroller.Text = "For this subject some work is already going on by " + ds_DataEntryControl.Tables(0).Rows(0).Item("vUserName").ToString + ". You Can not continue."
                            Me.MpeDataentryControl.Show()
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "RemoveCookies({flag:'LOAD',wId:'" + Me.Request.QueryString("WorkSpaceId").ToString + "',nId:'" + HFNodeId.Value.ToString + "',pId:'" + Me.Request.QueryString("PeriodId").ToString + "',sId:'" + Me.Request.QueryString("SubjectId").ToString + "'});", True)
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "Open1({flag:'COMBO',wId:'" + Me.Request.QueryString("WorkSpaceId").ToString + "',nId:'" + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + "',pId:'" + Me.Request.QueryString("PeriodId").ToString + "',sId:'" + Me.Request.QueryString("SubjectId").ToString + "'});", True)
                            Exit Sub
                        Else
                            'Me.lblDataEntrycontroller.Text = ds_DataEntryControl.Tables(0).Rows(0).Item("vUserName").ToString
                            'Me.MpeDataentryControl.Show()
                            'Exit Sub
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "Open1({flag:'COMBO',wId:'" + Me.Request.QueryString("WorkSpaceId").ToString + "',nId:'" + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + "',pId:'" + Me.Request.QueryString("PeriodId").ToString + "',sId:'" + Me.Request.QueryString("SubjectId").ToString + "'});", True)
                        End If

                    Else
                        dr = ds_DataEntryControl.Tables(0).NewRow()
                        dr("vWorkspaceId") = Me.HFWorkspaceId.Value
                        dr("iNodeId") = ddlActivities.SelectedValue.Split("#")(1)
                        dr("vSubjectId") = Me.Request.QueryString("SubjectId").ToString
                        dr("iModifyBy") = Session(S_UserID)
                        dr("iWorkFlowStageId") = Session(S_WorkFlowStageId)

                        ds_DataEntryControl.Tables(0).Rows.Add(dr)
                        ds_DataEntryControl.Tables(0).AcceptChanges()
                        If Not objLambda.insert_DataEntryControl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_DataEntryControl, estr, Return_Val) Then
                            Throw New Exception("Error While saving information of Data Entry Control...PageLoad()..")
                        End If
                        If Return_Val = "Y" Then
                            Me.lblDataEntrycontroller.Text = "For this subject some work is already going on by " + ds_DataEntryControl.Tables(0).Rows(0).Item("vUserName").ToString + ". You Can not continue."
                            Me.MpeDataentryControl.Show()
                            Exit Sub
                        End If
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetCookie", "Open1({flag:'COMBO',wId:'" + Me.Request.QueryString("WorkSpaceId").ToString + "',nId:'" + Me.ddlActivities.SelectedValue.Split("#")(1).ToString + "',pId:'" + Me.Request.QueryString("PeriodId").ToString + "',sId:'" + Me.Request.QueryString("SubjectId").ToString + "'});", True)
                    End If

                End If

            End If

            Me.Response.Redirect(StrRedirect, False)

            'If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
            '    choice = "1"
            'ElseIf CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
            '    choice = "2"
            'ElseIf CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
            '    choice = "4"
            'End If

            'If Me.HFType.Value = Type_BABE Then
            '    'If Me.Session(S_ScopeNo) = Scope_BABE Then
            '    StrRedirect = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + Me.HFWorkspaceId.Value.Trim() + _
            '                      "&ActivityId=" + Me.HFParentActivityId.Value.Trim() + "&NodeId=" + Me.HFParentNodeId.Value.Trim() + _
            '                      "&PeriodId=" + Me.HFPeriodId.Value.Trim() + "&Type=" + Type_BABE + "&SubjectId=" + Me.HFSubjectId.Value.Trim() + _
            '                      "&MySubjectNo=" + Me.HFMySubjectNo.Value.Trim() + "&ScreenNo=" + Me.HFScreenNo.Value.Trim() '+ "&Mode=" + choice
            '    If Not IsNothing(Me.Request.QueryString("Mode")) Then
            '        If Convert.ToString(Me.Request.QueryString("Mode")).Trim() <> "" Then
            '            StrRedirect += "&Mode=" + Me.Request.QueryString("Mode")
            '        End If
            '    End If
            'Else

            '    StrRedirect = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + Me.HFWorkspaceId.Value.Trim() + _
            '                      "&ActivityId=" + Me.HFParentActivityId.Value.Trim() + "&NodeId=" + Me.HFParentNodeId.Value.Trim() + _
            '                      "&PeriodId=" + Me.HFPeriodId.Value.Trim() + "&SubjectId=" + Me.HFSubjectId.Value.Trim() + _
            '                      "&MySubjectNo=" + Me.HFMySubjectNo.Value.Trim() + "&ScreenNo=" + Me.HFScreenNo.Value.Trim() '+ "&Mode=" + choice
            '    If Not IsNothing(Me.Request.QueryString("Mode")) Then
            '        If Convert.ToString(Me.Request.QueryString("Mode")).Trim() <> "" Then
            '            StrRedirect += "&Mode=" + Me.Request.QueryString("Mode")
            '        End If
            '    End If
            'End If

            'Me.Session(S_SelectedActivity) = Me.ddlActivities.SelectedIndex.ToString()

            'Me.Session(S_SelectedTab) = Me.HFActivateTab.Value
            'If Me.HFSessionFlg.Value = "" Then
            '    Me.Session(S_SelectedTab) = Nothing
            'End If
            'Me.HFActivateTab.Value = ""

            'Me.ViewState(VS_dtMedEx_Fill) = Nothing
            'Me.ViewState(VS_DtCRFHdr) = Nothing
            'Me.ViewState(VS_DtCRFDtl) = Nothing
            'Me.ViewState(VS_DtCRFSubDtl) = Nothing
            'Me.ViewState(VS_Choice) = Nothing
            'Me.ViewState(VS_DtDCF) = Nothing
            'Me.ViewState(VS_DCFNo) = Nothing
            'Me.ViewState(VS_DataStatus) = Nothing
            'Me.ViewState(VS_ReviewFlag) = Nothing

            'Me.Response.Redirect(StrRedirect, False)
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "Check For Pending Activity"
    'Added on 1-3-2012 by Megha shah----
    Private Function CheckPendingActivity() As Boolean
        Dim eStr As String = String.Empty
        Dim Param As String = String.Empty
        Dim Ds_Structure As DataSet = Nothing
        Dim SeqNo As Integer
        Dim Str As String = String.Empty
        Dim count As Integer = 0
        Dim PendingNode As String = String.Empty

        Param = Me.HFWorkspaceId.Value.Trim() + "##" + Me.HFSubjectId.Value.Trim() + "##" + Me.HFPeriodId.Value
        If Not objHelp.Proc_GetStructure(Param, Ds_Structure, eStr) Then
            Return False
        End If

        If Ds_Structure.Tables(0).Rows.Count > 0 Then

            SeqNo = Ds_Structure.Tables(0).Select(" iNodeId = " & Me.HFNodeId.Value)(0).Item("SeqNo")
            Ds_Structure.Tables(0).DefaultView.RowFilter = " SeqNo < " & SeqNo.ToString & " And ActivityStatus = '" & CRF_DataEntryPending & "'"
            If Ds_Structure.Tables(0).DefaultView.ToTable.Rows.Count > 0 Then
                For Each dr As DataRow In Ds_Structure.Tables(0).DefaultView.ToTable.Rows
                    count += 1
                    If PendingNode.ToString.Trim = "" Then
                        PendingNode = dr.Item("iNodeId").ToString()
                    Else
                        PendingNode = PendingNode.ToString() + "," + dr.Item("iNodeId").ToString()
                    End If
                    Str += count.ToString & ". " & dr.Item("vNodeDisplayName").ToString().ToUpper() & "<br/>"
                Next dr
            End If
        End If

        ' --------added by Megha shah for calling GenCall for those activity which are pending and not having other pending activity
        If Ds_Structure.Tables(0).DefaultView.ToTable.Rows.Count <= 0 Then
            If Not GenCall() Then
                CheckPendingActivity = False
            End If
            CheckPendingActivity = True
            Exit Function
        End If
        '-------------------------------------------------------

        If count > 0 Then
            Str += " activity/s pending For subject " & Ds_Structure.Tables(0).DefaultView.ToTable().Rows(0)("vMySubjectNo")
        End If
        If Not Str.ToString.Trim() = "" Then
            Me.lblContent.Text = Str.ToString()
            Me.HFPendingNode.Value = PendingNode.ToString()
            Me.Session(S_SelectedActivity + CurrentPageSessionVariable) = Me.ddlActivities.SelectedIndex.ToString
            If Me.Request.QueryString("From") Is Nothing Then

                'hdnIsPopup.Value = "true"
                Me.MPEActivitySequence.Show()
            Else
                If Not Session(S_DynamicPage_URL) Is Nothing AndAlso Session(S_DynamicPage_URL) <> "" Then
                    If Not GenCall() Then
                        Session(S_DynamicPage_URL) = Nothing
                        Return False
                    End If
                End If
            End If
            'Session(S_DynamicPage_URL) = Nothing
            Me.ddlActivities.Style.Add("color", "Red")
        End If
        Return True

    End Function
#End Region

#Region "Close All Popup"
    Private Function closeDiv() As Boolean
        Try
            Me.divHistoryDtl.Style.Add("display", "none")
            Me.divDCF.Style.Add("display", "none")
            Me.divAuthentication.Style.Add("display", "none")
            Me.divForEditAttribute.Style.Add("display", "none")
            Me.divEditChecks.Style.Add("display", "none")
            Me.divEditChecksQuery.Style.Add("display", "none") ''Added by Vivek Patel
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
#End Region

#Region "Dynamic EditChecks"
    Private Function FireDynamicEditChecks() As Boolean
        Dim Param As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds_WorkspaceEditChecksHdrDtl As DataSet = Nothing
        Try
            Param = Me.HFWorkspaceId.Value + "##" + Session(S_UserID) + "##" + Me.ddlActivities.SelectedValue.Split("#")(1) + "##" + Me.HFSubjectId.Value + "##" + Me.HFCRFDtlNo.Value + "##" + ""
            If Not objHelp.Proc_ExecuteEditChecks_WithinPage(Param, ds_WorkspaceEditChecksHdrDtl, estr) Then
                Throw New Exception("Error While Check For EditChecks")
            End If
            Return True
        Catch ex As Exception
            objCommon.ShowAlert(ex.ToString, Me.Page())
            Return False
        End Try
    End Function

    Protected Sub imgbtnShowQuery_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnShowQuery.Click
        Try
            Me.divEditChecks.Style.Add("display", "")
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "UnBindClick", "$('#imgbtnShowQuery').unbind('click');", True)
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "displayBackGround", "displayBackGround();", True)
            ''Added By Vivek Patel
            If GDVEditcheck.Rows.Count > 0 Or Not GDVEditcheck.DataSource Is Nothing Then
                GDVEditcheck.UseAccessibleHeader = True
                GDVEditcheck.HeaderRow.TableSection = TableRowSection.TableHeader
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "GDVEditcheck", "fnGDVEditcheckUI(); ", True)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    'Private Function ShowQuery() As Boolean
    '    Try
    '        Dim estr As String = String.Empty
    '        Dim ds_EditCheckReport As DataSet = Nothing
    '        If Not Me.objHelp.GetData("View_WorkspaceEditChecksHdrDTL", "*", " vWorkspaceId='" + Me.HFWorkspaceId.Value + "' And iNodeId=" + Me.ddlActivities.SelectedValue.Split("##")(1) + " And vactivityId='" + Me.ddlActivities.SelectedValue.Split("##")(0) + "' And vSubjectId='" + Me.HFSubjectId.Value + "' And cResolvedFlag<>'Y' ", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_EditCheckReport, estr) Then
    '            Throw New Exception("Error while get Information about EditChecks...imgbtnShowQuery_Click")
    '        End If
    '        If ds_EditCheckReport.Tables(0).Rows.Count > 0 Then
    '            Me.GDVEditcheck.DataSource = ds_EditCheckReport.Tables(0)
    '            Me.GDVEditcheck.DataBind()
    '            Me.imgbtnShowQuery.Style.Add("display", "")
    '            Return True
    '        End If
    '        Me.imgbtnShowQuery.Style.Add("display", "none")
    '        Return True
    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.Message, "")
    '        Return False
    '    End Try
    'End Function

#End Region

    Protected Sub btnContinueWorking_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinueWorking.Click
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ResetSessionTimer", "btnContinueWorking_Click();", True)
    End Sub

#Region "WebMethods"
    '' Added by Dipen Shah for auto time calculation of application serevr.
    <Web.Services.WebMethod()>
    Public Shared Function GetServerTime(ByVal timeZone As String) As String

        Dim objCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

        Try

            Dim da As DateTime = CDate(objCommon.GetCurDatetimeWithOffSet(timeZone).DateTime()).GetDateTimeFormats()(23)
            Return da.ToString("dd-MMM-yyyy HH:mm").ToString()
        Catch ex As Exception
            Return ex.Message
            Return False
        End Try
        Return True
    End Function

    <WebMethod>
    Public Shared Function ViewDocument(ByVal ActivityId As String, ByVal Period As String, ByVal WorkSpaceId As String, ByVal NodeId As String) As String
        Dim wstr As String = String.Empty
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim FilePath As String = String.Empty
        Dim dsChildGrid As New DataSet
        Try

            wstr = WorkSpaceId.ToString() + "##" + ActivityId.ToString() + "##" + Period.ToString() + "##" + NodeId
            dsChildGrid = objHelp.ProcedureExecute("dbo.Proc_GetActivityFilePath", wstr)

            If Not dsChildGrid Is Nothing AndAlso dsChildGrid.Tables(0).Rows.Count > 0 Then
                FilePath = System.Configuration.ConfigurationManager.AppSettings("CRFUploadFilePath") + dsChildGrid.Tables(0).Rows(0)("vFilePath").ToString()
            End If

            Return FilePath
        Catch ex As Exception
            Return False
        End Try
    End Function

    <WebMethod>
    Public Shared Function TablulerRepetationGrid(ByVal WorkspaceID As String, ByVal ActivityId As String, ByVal SubjectId As String, ByVal Period As String, ByVal NodeId As String) As String
        Dim Wstr As String = String.Empty
        Dim estr_retu As String = String.Empty
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim ds_CRFSubjectHdrDtl As New DataSet
        Dim dtTablulerRepetation As New DataTable
        Dim AttributeValue As String = String.Empty
        Dim drTable As DataRow
        Dim Repetation As String = String.Empty
        Dim ds_Basic As New DataSet
        Dim strReturn As String = String.Empty
        Dim CodeNo As Integer = 0
        Dim StrGroup(1) As String
        Dim dt_MedexGroup As New DataTable
        Dim dv_MedexGroup As New DataView
        Dim StrGroupCodes As String = String.Empty
        Dim StrGroupDesc As String = String.Empty

        Try

            Wstr = "cActiveFlag<>'N' and vWorkSpaceId='" + WorkspaceID + "'" +
                      " and vActivityId='" + ActivityId + "' and vSubjectId='" +
                          SubjectId + "'" &
                      " and iPeriod=" + Period + " And iNodeId=" +
                      NodeId

            Wstr += "  Order by RepetitionNo,iSeqNo OPTION (MAXDOP 1)"

            If Not objHelp.View_CRFHdrDtlSubDtl_Edit(Wstr, "*,DENSE_RANK() OVER(PARTITION BY View_CRFHdrDtlSubDtl_Edit.nCRFHdrNo,View_CRFHdrDtlSubDtl_Edit.vActivityId,View_CRFHdrDtlSubDtl_Edit.vSubjectId ORDER BY View_CRFHdrDtlSubDtl_Edit.nCRFHdrNo,View_CRFHdrDtlSubDtl_Edit.vActivityId,View_CRFHdrDtlSubDtl_Edit.vSubjectId,View_CRFHdrDtlSubDtl_Edit.iRepeatNo) as [RepetitionNo] ", ds_CRFSubjectHdrDtl, estr_retu) Then
                Throw New Exception(estr_retu)
            End If

            dv_MedexGroup = ds_CRFSubjectHdrDtl.Tables(0).DefaultView

            StrGroup(0) = "vMedExGroupCode"
            StrGroup(1) = "vMedExGroupDesc"
            dt_MedexGroup = dv_MedexGroup.ToTable(True, StrGroup)

            For Each drGroup In dt_MedexGroup.Rows

                If StrGroupCodes <> "" Then
                    StrGroupCodes += ",Div" + drGroup("vMedExGroupCode")
                    StrGroupDesc += "," + drGroup("vMedExGroupDesc")
                Else
                    StrGroupCodes = "Div" + drGroup("vMedExGroupCode")
                    StrGroupDesc = "" + drGroup("vMedExGroupDesc")
                End If

            Next

            Wstr = "cActiveFlag<>'N' and vWorkSpaceId='" + WorkspaceID + "'" &
                        " and vActivityId='" + ActivityId + "'" +
                        " And iNodeId=" +
                        NodeId + " Order by iSeqNo"

            If Not objHelp.GetViewMedExWorkSpaceDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Basic, estr_retu) Then
                Throw New Exception(estr_retu)
            End If

            ds_CRFSubjectHdrDtl.Tables(0).DefaultView.RowFilter = "vMedExDesc='' and RepetitionNo='1' "
            If ds_CRFSubjectHdrDtl.Tables(0).DefaultView.ToTable.Rows.Count > 0 Then
                CodeNo = Convert.ToInt32(ds_CRFSubjectHdrDtl.Tables(0).DefaultView.ToTable.Rows(0)("vMedExCode").ToString)
            End If

            dtTablulerRepetation.Columns.Add("Repetition")

            For Each dr As DataRow In ds_Basic.Tables(0).Rows
                If dr("vMedExCode") <> CodeNo Then
                    AttributeValue = dr("vMedExDesc").ToString()
                    dtTablulerRepetation.Columns.Add(AttributeValue)
                    AttributeValue = ""
                End If
            Next

            For Each drCRF As DataRow In ds_CRFSubjectHdrDtl.Tables(0).Rows

                ds_CRFSubjectHdrDtl.Tables(0).DefaultView.RowFilter = "RepetitionNo = " + drCRF("RepetitionNo").ToString()
                drTable = dtTablulerRepetation.NewRow()

                If drCRF("RepetitionNo").ToString() <> Repetation.ToString() Then

                    Repetation = drCRF("RepetitionNo")
                    drTable("Repetition") = StrGroupDesc + "_" + Repetation

                    For Each dr As DataRow In ds_CRFSubjectHdrDtl.Tables(0).DefaultView.ToTable.Rows
                        If dr("vMedExDesc").ToString() <> "" Then
                            AttributeValue = dr("vMedExDesc").ToString()
                            drTable(AttributeValue) = dr("vDefaultValue").ToString()
                            AttributeValue = ""
                        End If
                    Next

                    dtTablulerRepetation.Rows.Add(drTable)
                    dtTablulerRepetation.AcceptChanges()
                End If

            Next
            strReturn = JsonConvert.SerializeObject(dtTablulerRepetation)
            Return strReturn
        Catch ex As Exception
            Throw New Exception("Error while TablulerRepetation()")
        End Try
    End Function

    'Add by shivani pandya for Repeatiton dropdown color
    <WebMethod>
    Public Shared Function getRepeatitionColor()
        Dim dtRepeat As New DataTable
        Dim dsReview As New DataSet
        Dim dtRepeatData As New DataTable
        Dim StrDataStatus As String = ","
        Dim dv_review As New DataView
        Try
            dtRepeat = HttpContext.Current.Session(S_Repeatition)
            dsReview = HttpContext.Current.Session(S_Review)
            For Each dr As DataRow In dtRepeat.Rows
                If dr("cCRFDtlDataStatus") = CRF_DataEntry Then
                    StrDataStatus = StrDataStatus + Convert.ToString(dr("RepetitionNo")) + ":" + "Orange" + ","
                End If
                If dr("cCRFDtlDataStatus") = CRF_Review Then
                    StrDataStatus = StrDataStatus + Convert.ToString(dr("RepetitionNo")) + ":" + "Blue" + ","
                End If
                If dr("cCRFDtlDataStatus") = CRF_ReviewCompleted Then   'add by Shyam Kamdar 17-04-2021
                    StrDataStatus = StrDataStatus + Convert.ToString(dr("RepetitionNo")) + ":" + "Blue" + ","
                End If
                If dr("iWorkFlowstageId") <> 0 Then
                    dv_review = dsReview.Tables(0).Copy.DefaultView
                    dv_review.RowFilter = "iReviewWorkflowStageId=" + Convert.ToString(dr("iWorkFlowStageId")).Trim()
                    If dv_review.ToTable.Rows.Count > 0 Then
                        'StrDataStatus = StrDataStatus + Convert.ToString(dr("RepetitionNo")) + ":" + Convert.ToString(dv_review.ToTable.Rows(0)("vColorCodeForDynamic")) + ","
                        StrDataStatus = StrDataStatus + Convert.ToString(dr("RepetitionNo")) + ":" + "Blue" + ","
                    Else
                        StrDataStatus = StrDataStatus + Convert.ToString(dr("RepetitionNo")) + ":" + "Red" + ","
                    End If
                Else
                    If dr("cCRFDtlDataStatus") <> CRF_DataEntry And dr("cCRFDtlDataStatus") <> CRF_Review Then
                        StrDataStatus = StrDataStatus + Convert.ToString(dr("RepetitionNo")) + ":" + "Red" + ","
                    End If
                End If
            Next
            ' StrDataStatus = StrDataStatus + ","
            Return JsonConvert.SerializeObject(StrDataStatus)
        Catch ex As Exception
            Throw New Exception("Error while getRepeatitionColor")
        End Try
    End Function

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

#End Region

#Region "Tabluler Repetataion"
    Protected Sub btnGridViewDisplay_Click(sender As Object, e As EventArgs) Handles btnGridViewDisplay.Click
        Dim wstr As String = String.Empty
        Dim dtRepetition As New DataTable
        Dim ds_AuditTrail As New DataSet
        Dim estr As String = String.Empty
        ' Dim Repetition As String = "Repetition "
        Dim dtAttribute As New DataTable
        Dim estr_retu As String = String.Empty
        Dim ds As New DataSet
        Dim ds_CRFSubjectHdrDtl As New DataSet
        Dim CodeNo As Integer = 0

        Try

            ActivityDropdownColor()

            If SubmitedDataFlagForTabulerRepetition = False Then
                wstr = "vWorkspaceId = '" + Me.HFWorkspaceId.Value.Trim() + "' And vSubjectId='" &
                 Me.HFSubjectId.Value.Trim() & "' And " &
               " iNodeId=" & Me.HFNodeId.Value.Trim() & " And vActivityId = '" & Me.HFActivityId.Value.Trim() & "'"

                If Not Me.objHelp.GetData("View_CrfHdrDtlRepetation", "*,DENSE_RANK() OVER(PARTITION BY nCRFHdrNo,vActivityId,vSubjectId ORDER BY nCRFHdrNo,vActivityId,vSubjectId,iRepeatNo) as [RepetitionNo] ", wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_AuditTrail, estr) Then
                    Throw New Exception("Error while View_CrfHdrDtlRepetation()")
                End If

                If ds_AuditTrail.Tables(0).Rows.Count > 0 Then
                    dtRepetition = ds_AuditTrail.Tables(0)
                    dtRepetition.Columns.Add("RepetitionName")
                    For Each dr As DataRow In dtRepetition.Rows
                        dr("RepetitionName") = RepHeader + "_" + dr("RepetitionNo").ToString()
                    Next
                End If

                If dtRepetition.Rows.Count > 0 Then
                    ddlGridRepetation.DataSource = dtRepetition
                    ddlGridRepetation.DataValueField = "RepetitionNo"
                    ddlGridRepetation.DataTextField = "RepetitionName"
                    ddlGridRepetation.DataBind()
                End If

                ' dtAttribute = Me.ViewState(VS_dtMedEx_Fill)


                wstr = "cActiveFlag<>'N' and vWorkSpaceId='" & Me.HFParentWorkspaceId.Value.Trim() & "'" &
                        " and vActivityId='" & Me.HFActivityId.Value.Trim() & "'" &
                        " And iNodeId=" &
                        Me.HFNodeId.Value.Trim() & " Order by iSeqNo"

                If Not Me.objHelp.GetViewMedExWorkSpaceDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr_retu) Then
                    Throw New Exception(estr_retu)
                End If

                dtAttribute = ds.Tables(0)

                If dtAttribute.Rows.Count > 0 And dtRepetition.Rows.Count > 0 Then

                    wstr = "cActiveFlag<>'N' and vWorkSpaceId='" + Me.HFWorkspaceId.Value.Trim() + "'" +
                          " and vActivityId='" + Me.HFActivityId.Value.Trim() + "' and vSubjectId='" +
                              Me.HFSubjectId.Value.Trim() + "'" &
                          " and iPeriod=" + Me.HFPeriodId.Value.Trim() + " And iNodeId=" +
                          Me.HFNodeId.Value.Trim()

                    wstr += "  Order by RepetitionNo,iSeqNo OPTION (MAXDOP 1)"

                    If Not objHelp.View_CRFHdrDtlSubDtl_Edit(wstr, "*,DENSE_RANK() OVER(PARTITION BY View_CRFHdrDtlSubDtl_Edit.nCRFHdrNo,View_CRFHdrDtlSubDtl_Edit.vActivityId,View_CRFHdrDtlSubDtl_Edit.vSubjectId ORDER BY View_CRFHdrDtlSubDtl_Edit.nCRFHdrNo,View_CRFHdrDtlSubDtl_Edit.vActivityId,View_CRFHdrDtlSubDtl_Edit.vSubjectId,View_CRFHdrDtlSubDtl_Edit.iRepeatNo) as [RepetitionNo] ", ds_CRFSubjectHdrDtl, estr_retu) Then
                        Throw New Exception(estr_retu)
                    End If

                    ds_CRFSubjectHdrDtl.Tables(0).DefaultView.RowFilter = "vMedExDesc='' and RepetitionNo='1' "

                    If ds_CRFSubjectHdrDtl.Tables(0).DefaultView.ToTable.Rows.Count > 0 Then
                        CodeNo = Convert.ToInt32(ds_CRFSubjectHdrDtl.Tables(0).DefaultView.ToTable.Rows(0)("vMedExCode").ToString)
                        dtAttribute.DefaultView.RowFilter = "vMedExCode <> " + CodeNo.ToString()
                        ddlGridActivity.DataSource = dtAttribute.DefaultView.ToTable()
                    Else
                        ddlGridActivity.DataSource = dtAttribute
                    End If
                    ddlGridActivity.DataTextField = "vMedExDesc"
                    ddlGridActivity.DataValueField = "vMedExCode"
                    ddlGridActivity.DataBind()
                End If

                If dtRepetition.Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "TablulerRepetationSetting", "TablulerRepetationSetting();", True)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "tablulerRepetaitonAlter", "tablulerRepetaitonAlter();", True)
                End If
            Else
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "buttonSettings()", "buttonSettings();", True)
            End If
        Catch ex As Exception
            Throw New Exception("Error while btnGridViewFormate_Click()")
        End Try
    End Sub

    Protected Sub btnClearDynamicPage_Click(sender As Object, e As EventArgs) Handles btnClearDynamicPage.Click
        Try
            If Not GenCall_ShowUI() Then
                Throw New Exception("Error while ClearDynamicPage()")
            End If
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "TablulerRepetationSetting", "TablulerRepetationSetting();", True)
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "TablulerFormate", "TablulerFormate();", True)
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "GenerateData", "GenerateData();", True)
        Catch ex As Exception
            Throw New Exception("Error while btnClearDynamicPage_Click()")
        End Try
    End Sub
#End Region

    Protected Sub btnExitGrid_Click(sender As Object, e As EventArgs) Handles btnExitGrid.Click
        Try
            ActivityDropdownColor()
        Catch ex As Exception
            Throw New Exception("Error while btnExitGrid_Click()")
        End Try
    End Sub
#Region "Dynamic review"

    Private Function GetLegends() As Boolean
        Dim ds As New DataSet
        Dim dv As DataView
        Dim estr As String = ""

        Try
            If Not Me.objHelp.Proc_GetLegends(Me.HFWorkspaceId.Value.ToString(), ds, estr) Then
                Throw New Exception("Error While Gettin Proc_getLegends" + estr)
            End If

            If ds Is Nothing Or ds.Tables.Count = 0 Then
                Return False
            End If

            Me.Session(Vs_dsReviewerlevel) = ds.Copy()

            If Not Me.filllegends() Then
                Return False
            End If

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....GetLegends")
            Return False
        End Try
    End Function

    Private Function filllegends() As Boolean
        Dim ds As New DataSet
        Dim estr As String = ""
        Dim lbl As New Label
        Dim dv As DataView
        Try
            Me.PhlReviewer.Controls.Clear()

            If Not Me.objHelp.Proc_GetProjectReviewerLevel(Me.HFWorkspaceId.Value.ToString.Trim(), ds, estr) Then
                Throw New Exception("Error while getting data from Proc_GetProjectReviewerLevel " + estr.Trim)
            End If

            dv = ds.Tables(0).Copy.DefaultView
            dv.RowFilter = "vUsertypecode like '%" + Convert.ToString(Me.Session(S_UserType)) + "%'"

            If Me.Session(S_WorkFlowStageId) <> 0 Then

                If dv.ToTable.Rows.Count = 0 Then
                    'Me.Session(vs_workflowstageidfordynamic) = Me.Session(S_WorkFlowStageId)
                    Me.Session(vs_workflowstageidfordynamic) = 4
                    Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View
                Else
                    Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
                    Me.Session(vs_workflowstageidfordynamic) = Convert.ToInt32(dv.ToTable.Rows(0)("iActualWorkflowStageId"))
                    Me.Session("vReviewerlevel") = Convert.ToString(dv.ToTable.Rows(0)("vReviewerlevel"))
                    If Convert.ToInt32(dv.ToTable.Rows(0)("iActualWorkflowStageId")) = 5 Then
                        Me.ViewState("IR") = "Y"
                        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
                    End If

                End If
                If (Me.Session(S_WorkFlowStageId) = WorkFlowStageId_MedicalCoding) Then
                    Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                End If
            Else
                Me.Session(vs_workflowstageidfordynamic) = 0
            End If

            ds = CType(Me.Session(Vs_dsReviewerlevel), DataSet)

            If ds.Tables(0).Rows.Count > 0 Then

                lbl = Getlable("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", "lblRed", "")
                lbl.BackColor = Drawing.Color.Red
                Me.PhlReviewer.Controls.Add(lbl)
                Me.PhlReviewer.Controls.Add(New LiteralControl("-Data Entry Pending, "))
                lbl = New Label
                lbl = Getlable("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", "lblOrange", "")
                lbl.BackColor = Drawing.Color.Orange
                Me.PhlReviewer.Controls.Add(lbl)
                Me.PhlReviewer.Controls.Add(New LiteralControl("-Data Entry Continue, "))
                lbl = New Label
                lbl = Getlable("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", "lblBlue", "")
                lbl.BackColor = Drawing.Color.Blue
                Me.PhlReviewer.Controls.Add(lbl)
                Me.PhlReviewer.Controls.Add(New LiteralControl("-Ready For Review, "))
                Me.PhlReviewer.Controls.Add(New LiteralControl("<br />"))
                Me.PhlReviewer.Controls.Add(New LiteralControl("<br />"))
                Dim int As Integer = ds.Tables(0).Rows.Count = 0
                For Each dr As DataRow In ds.Tables(0).Rows
                    int += 1
                    lbl = New Label
                    lbl = Getlable("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", "lbl" + dr("iActualWorkflowStageId").ToString(), "")
                    lbl.BackColor = System.Drawing.ColorTranslator.FromHtml(dr("vColorCodeForDynamic").ToString())
                    Me.PhlReviewer.Controls.Add(lbl)
                    If int = ds.Tables(0).Rows.Count Then
                        Me.PhlReviewer.Controls.Add(New LiteralControl("-" + dr("Reviewer").ToString() + " "))
                    Else
                        Me.PhlReviewer.Controls.Add(New LiteralControl("-" + dr("Reviewer").ToString() + " ,"))
                    End If

                Next
            End If

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....GetLegends")
            Return False
        End Try
    End Function

#End Region

    Protected Sub btnRepeatGo_Click(sender As Object, e As EventArgs) Handles btnRepeatGo.Click
        Dim dtData As New DataTable
        Dim dtGetData As New DataTable
        Try
            If Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() = WorkFlowStageId_FirstReview Or
               Convert.ToString(Me.Session(vs_workflowstageidfordynamic)).Trim() = WorkFlowStageId_SecondReview Then
                dtData = Me.ViewState(VS_dtMedEx_Fill_Backup)
                dtGetData = dtData.DefaultView.ToTable(True, "cCRFDtlDataStatus")
                'If dtGetData.Rows.Count = 1 Then
                'dtGetData.DefaultView.RowFilter = "cCRFDtlDataStatus ='B' "
                'If dtGetData.DefaultView.ToTable().Rows.Count > 0 Then
                '    Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View
                'End If
                'End If
            End If

            ActivityDropdownColor()
            ddlRepeatNo_SelectedIndexChanged(Nothing, Nothing)
        Catch ex As Exception
            Throw New Exception("Error while Go click")
        End Try
    End Sub

    Protected Sub btnUpdateRemarks_Click(sender As Object, e As EventArgs) Handles btnUpdateRemarks.Click
        Try
            btnUpdateDiscrepancy_Click(Nothing, Nothing)
            GenCall()
            ActivityDropdownColor()
        Catch ex As Exception
            Throw New Exception("Error while btnDCFUpdateRemarks")
        End Try

    End Sub

    <Web.Services.WebMethod()>
    Public Shared Function CheckValidationForDataEntry(ByVal WorkSpaceId As String, ByVal iNodeId As String, ByVal vSubjectId As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
        Dim strReturn As String = String.Empty
        Dim ds_Scheduled As DataSet
        Dim wStr As String

        Dim message As String = "Success"

        wStr = WorkSpaceId + "##" + iNodeId + "##" + vSubjectId + "##"

        ds_Scheduled = objHelp.ProcedureExecute("dbo.Proc_GetDataVisitTracker", wStr)

        Try
            If Not ds_Scheduled Is Nothing AndAlso ds_Scheduled.Tables(0).Rows.Count > 0 Then
                Dim dv As DataView = ds_Scheduled.Tables(0).DefaultView
                dv.RowFilter = "dActualDate IS NOT NULL"
                If dv.Table().DefaultView.ToTable().Rows.Count > 0 Then

                Else
                    message = "failure"
                End If
            End If
            Return message
        Catch ex As Exception
            message = "failure"
            Throw New Exception(ex.Message)
        End Try

    End Function
    ''Added By Vivek Patel
    Protected Sub GDVEditcheckQuery_PreRender(sender As Object, e As EventArgs) Handles GDVEditcheckQuery.PreRender
        If GDVEditcheckQuery.Rows.Count > 0 Or Not GDVEditcheckQuery.DataSource Is Nothing Then
            GDVEditcheckQuery.UseAccessibleHeader = True
            GDVEditcheckQuery.HeaderRow.TableSection = TableRowSection.TableHeader
            GDVEditcheckQuery.FooterRow.TableSection = TableRowSection.TableFooter
        End If
    End Sub
    ''Completed By Vivek Patel

    Protected Sub GVHistoryDtl_PreRender(sender As Object, e As EventArgs) Handles GVHistoryDtl.PreRender
        If GVHistoryDtl.Rows.Count > 0 Or Not GVHistoryDtl.DataSource Is Nothing Then
            GVHistoryDtl.UseAccessibleHeader = True
            GVHistoryDtl.HeaderRow.TableSection = TableRowSection.TableHeader
            GVHistoryDtl.FooterRow.TableSection = TableRowSection.TableFooter
        End If
    End Sub

    Protected Sub GVWDCF_PreRender(sender As Object, e As EventArgs) Handles GVWDCF.PreRender
        If GVWDCF.Rows.Count > 0 Or Not GVWDCF.DataSource Is Nothing Then
            GVWDCF.UseAccessibleHeader = True
            GVWDCF.HeaderRow.TableSection = TableRowSection.TableHeader
            GVWDCF.FooterRow.TableSection = TableRowSection.TableFooter
        End If
    End Sub

    '<Web.Services.WebMethod()>
    Private Function CheckVisitStatus(ByVal WorkSpaceId As String,
                                            ByVal SubjectId As String,
                                            ByVal ActivityId As String,
                                            ByVal NodeId As String,
                                            ByVal UserTypeCode As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim wstr As String = String.Empty
        Dim estr_retu As String = String.Empty
        Dim dsVisit As New DataSet
        Dim dv As DataView
        Dim returnData As String = String.Empty
        Dim strStatus As String = ""
        Dim strStatusGrader As String = ""

        Try
            wstr = "WorkSpaceId = '" + WorkSpaceId + "' AND SubjectId='" + SubjectId + "' AND " +
                       "vActivityId = '" + ActivityId + "' AND iNodeId='" + NodeId + "'"
            'If Not String.IsNullOrEmpty(ImageType) Then
            '    wstr += " AND vImageType='" + ImageType + "'"
            'End If

            If Not objHelp.GetData("View_GetVisitStatusCheck", "TOP 1 *", wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsVisit, estr_retu) Then
                Throw New Exception("Error while getting detail of VisitStatus..." + estr_retu)
            End If

            If dsVisit.Tables(0).Rows.Count > 0 Then
                If dsVisit.Tables(0).Rows.Count = 1 Then
                    'If Convert.ToString(ConfigurationManager.AppSettings("QCUserCode")).Contains(UserTypeCode) Then
                    '    strStatus = dsVisit.Tables(0).Rows(0).Item("cReviewStatus").ToString.Trim()
                    '    'If (String.IsNullOrEmpty(strStatus)) Then
                    '    '    returnData = "2#"
                    '    'Else
                    '    If strStatus.ToUpper() = "R" Then
                    '        Me.BtnSave.Visible = False
                    '        Me.btnSaveAndContinue.Visible = False
                    '        'returnData = "0#Uploaded Images was rejected, Please re-upload image!"
                    '        Me.objCommon.ShowAlert("Uploaded Images was rejected, Please re-upload image!", Me.Page)
                    '    End If
                    '    'End If
                    'End If
                    strStatus = dsVisit.Tables(0).Rows(0).Item("cReviewStatus").ToString.Trim()
                    If Convert.ToString(ConfigurationManager.AppSettings("QCUserCode")).Contains(UserTypeCode) AndAlso UserTypeCode = Me.Session(S_UserType) Then


                        If strStatus.ToUpper() = "QC2R" Then
                            'returnData = "0#Uploaded Images was rejected, Please re-upload image!"
                            Me.BtnSave.Visible = False
                            Me.btnSaveAndContinue.Visible = False
                            Me.objCommon.ShowAlert("Images have been rejected by QC2.", Me.Page)
                        ElseIf strStatus.ToUpper() = "CA1R" Then
                            'returnData = "0#Please compelete QC2 review first"
                            Me.BtnSave.Visible = False
                            Me.btnSaveAndContinue.Visible = False
                            Me.objCommon.ShowAlert("Images have been rejected by CA1.", Me.Page)
                            'ElseIf strStatus.ToUpper() = "QC2" Then
                            '    returnData = "2#"
                        ElseIf strStatus.ToUpper() = "QC1R" Then
                            'returnData = "0#Please compelete QC2 review first"
                            Me.BtnSave.Visible = False
                            Me.btnSaveAndContinue.Visible = False
                            Me.objCommon.ShowAlert("Images have been rejected.", Me.Page)
                        End If

                    End If
                    If Convert.ToString(ConfigurationManager.AppSettings("QC2UserCode")).Contains(UserTypeCode) AndAlso UserTypeCode = Me.Session(S_UserType) Then

                        If (String.IsNullOrEmpty(strStatus)) Then
                            'returnData = "0#Please compelete QC1 review first"
                            Me.BtnSave.Visible = False
                            Me.btnSaveAndContinue.Visible = False
                            Me.objCommon.ShowAlert("Image QC1 review is pending.", Me.Page)
                        Else
                            If strStatus.ToUpper() = "QC1R" Then
                                'returnData = "0#Uploaded Images was rejected, Please re-upload image!"
                                Me.BtnSave.Visible = False
                                Me.btnSaveAndContinue.Visible = False
                                Me.objCommon.ShowAlert("Images have been rejected by QC1.", Me.Page)
                                'ElseIf strStatus.ToUpper() = "QC1" Then
                                '    returnData = "2#"
                            ElseIf strStatus.ToUpper() = "CA1R" Then
                                'returnData = "0#Please compelete QC2 review first"
                                Me.BtnSave.Visible = False
                                Me.btnSaveAndContinue.Visible = False
                                Me.objCommon.ShowAlert("Images have been rejected by CA1.", Me.Page)
                                'ElseIf strStatus.ToUpper() = "QC2" Then
                                '    returnData = "2#"
                            ElseIf strStatus.ToUpper() = "QC2R" Then
                                'returnData = "0#Please compelete QC2 review first"
                                Me.BtnSave.Visible = False
                                Me.btnSaveAndContinue.Visible = False
                                Me.objCommon.ShowAlert("Images have been rejected", Me.Page)
                            End If
                        End If
                    End If

                    If Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(UserTypeCode) AndAlso UserTypeCode = Me.Session(S_UserType) Then
                        'strStatus = dsVisit.Tables(0).Rows(0).Item("cReviewStatus").ToString.Trim()
                        If (String.IsNullOrEmpty(strStatus)) Then
                            'returnData = "0#Please compelete QC1 review first"
                            Me.BtnSave.Visible = False
                            Me.btnSaveAndContinue.Visible = False
                            Me.objCommon.ShowAlert("Image QC review is pending", Me.Page)
                            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Messagebox", "alert('Please compelete QC1 review first');", True)
                        Else
                            If strStatus.ToUpper() = "QC2R" Then
                                'returnData = "0#Uploaded Images was rejected, Please re-upload image!"
                                Me.BtnSave.Visible = False
                                Me.btnSaveAndContinue.Visible = False
                                Me.objCommon.ShowAlert("Images have been rejected by QC2.", Me.Page)
                            ElseIf strStatus.ToUpper() = "QC1R" Then
                                'returnData = "0#Please compelete QC2 review first"
                                Me.BtnSave.Visible = False
                                Me.btnSaveAndContinue.Visible = False
                                Me.objCommon.ShowAlert("Images have been rejected QC1.", Me.Page)
                                'ElseIf strStatus.ToUpper() = "QC2" Then
                                '    returnData = "2#"
                            ElseIf strStatus.ToUpper() = "CA1R" Then
                                'returnData = "0#Please compelete QC2 review first"
                                Me.BtnSave.Visible = False
                                Me.btnSaveAndContinue.Visible = False
                                Me.objCommon.ShowAlert("Images have been rejected.", Me.Page)
                                'ElseIf strStatus.ToUpper() = "QC2" Then
                                '    returnData = "2#"

                            ElseIf strStatus.ToUpper() = "QC1A" Then
                                'returnData = "0#Please compelete QC2 review first"
                                Me.BtnSave.Visible = False
                                Me.btnSaveAndContinue.Visible = False
                                Me.objCommon.ShowAlert("Image QC2 review is pending", Me.Page)

                            End If

                        End If
                    End If

                    If Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(Me.Session(S_UserType).ToString.Trim) AndAlso UserTypeCode = Convert.ToString(ConfigurationManager.AppSettings("QCUserCode")) Then
                        'strStatus = dsVisit.Tables(0).Rows(0).Item("cReviewStatus").ToString.Trim()
                        If (String.IsNullOrEmpty(strStatus)) Then
                            'returnData = "0#Please compelete QC1 review first"
                            Me.BtnSave.Visible = False
                            Me.btnSaveAndContinue.Visible = False
                            Me.objCommon.ShowAlert("Image QC review is pending", Me.Page)
                            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Messagebox", "alert('Please compelete QC1 review first');", True)
                        End If
                    ElseIf Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(Me.Session(S_UserType).ToString.Trim) AndAlso UserTypeCode = Convert.ToString(ConfigurationManager.AppSettings("QC2UserCode")) Then
                        If (String.IsNullOrEmpty(strStatus)) Then
                            'returnData = "0#Please compelete QC1 review first"
                            Me.BtnSave.Visible = False
                            Me.btnSaveAndContinue.Visible = False
                            Me.objCommon.ShowAlert("Image QC review is pending", Me.Page)
                            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Messagebox", "alert('Please compelete QC1 review first');", True)
                        Else
                            If strStatus.ToUpper() = "QC1R" Then
                                'returnData = "0#Uploaded Images was rejected, Please re-upload image!"
                                Me.BtnSave.Visible = False
                                Me.btnSaveAndContinue.Visible = False
                                Me.objCommon.ShowAlert("Images have been rejected by QC1.", Me.Page)
                            ElseIf strStatus.ToUpper() = "QC1A" Then
                                'returnData = "0#Please compelete QC2 review first"
                                Me.BtnSave.Visible = False
                                Me.btnSaveAndContinue.Visible = False
                                Me.objCommon.ShowAlert("Image QC2 review is pending", Me.Page)
                            End If
                        End If
                    End If
                    If Session(S_WorkFlowStageId) = WorkFlowStageId_OnlyView Then
                        If strStatus.ToUpper() = "QC2A" Then
                            'returnData = "0#Uploaded Images was rejected, Please re-upload image!"
                            Me.BtnSave.Visible = False
                            Me.btnSaveAndContinue.Visible = False
                            Me.objCommon.ShowAlert("Image CA review is pending", Me.Page)
                        ElseIf strStatus.ToUpper() = "CA1R" Then
                            'returnData = "0#Please compelete QC2 review first"
                            Me.BtnSave.Visible = False
                            Me.btnSaveAndContinue.Visible = False
                            Me.objCommon.ShowAlert("Images have been rejected by CA1.", Me.Page)
                            'ElseIf strStatus.ToUpper() = "QC2" Then
                            '    returnData = "2#"
                        ElseIf strStatus.ToUpper() = "QC1A" Then
                            'returnData = "0#Please compelete QC2 review first"
                            Me.BtnSave.Visible = False
                            Me.btnSaveAndContinue.Visible = False
                            Me.objCommon.ShowAlert("Image QC2 review is pending", Me.Page)
                        ElseIf strStatus.ToUpper() = "QC1R" Then
                            'returnData = "0#Please compelete QC2 review first"
                            Me.BtnSave.Visible = False
                            Me.btnSaveAndContinue.Visible = False
                            Me.objCommon.ShowAlert("Image QC2 review is pending", Me.Page)
                        ElseIf strStatus.ToUpper() = "" Then
                            Me.BtnSave.Visible = False
                            Me.btnSaveAndContinue.Visible = False
                            Me.objCommon.ShowAlert("Image QC1 review is pending", Me.Page)
                        End If
                        'returnData = "2#Activity is pending"
                    Else
                        returnData = "0#UserType Code not found!"
                    End If
                Else
                    Me.BtnSave.Visible = False
                    Me.btnSaveAndContinue.Visible = False
                    'Me.objCommon.ShowAlert("UserType Code not found!", Me.Page)
                End If
            Else
                'returnData = "0#Please First Upload Images!"
                Me.BtnSave.Visible = False
                Me.btnSaveAndContinue.Visible = False
                Me.objCommon.ShowAlert("Please First Upload Images!", Me.Page)
            End If

        Catch ex As Exception
            Return ex.ToString()
        Finally

        End Try
    End Function

#Region "Button Events"
    Protected Sub btnQuerySave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnQuerySave.Click
        Dim Ds_QueryMaster As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = ""
        Dim dr As DataRow
        Dim ds_Heading As New DataSet
        Dim ds_WorkspaceNodeDetail As New DataSet
        Dim dt_heading As New DataTable
        Dim vMysubjectId As String = String.Empty
        Dim QueryStatus As String
        Dim ds_WorkspaceSubjectMst As New DataSet
        Try
            wStr = "vWorkspaceId='" & Me.HFWorkspaceId.Value.Trim() & "' and cStatusIndi <> 'D'"
            If Not Me.objHelp.GetWorkspaceProtocolDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Heading, eStr) Then
                Throw New Exception("Error while getting Header information. " + eStr)
            End If
            dt_heading = ds_Heading.Tables(0)

            '********Getting Activity name for display
            wStr += " And vActivityId = '" + Me.HFActivityId.Value.Trim() + "' And iNodeId = " + Me.HFNodeId.Value.Trim()
            If Not Me.objHelp.GetViewWorkSpaceNodeDetail(wStr, ds_WorkspaceNodeDetail, eStr) Then
                Throw New Exception("Error while getting Header information. " + eStr)
            End If
            '*********************************

            '***********Getting Randomization No or vMySubjectNo for display
            wStr = "vWorkspaceId='" & Me.HFWorkspaceId.Value.Trim() & "' and cStatusIndi <> 'D' And "
            wStr += " iMySubjectNo = " + Me.HFMySubjectNo.Value.Trim() & " and iperiod = " & Convert.ToString(Me.HFPeriodId.Value).Trim()
            If Not Me.objHelp.GetWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                                     ds_WorkspaceSubjectMst, eStr) Then
                Throw New Exception("Error while getting Header information. " + eStr)
            End If


            Ds_QueryMaster = objHelp.GetResultSet("select * from QueryMaster WHERE 1=2", "QueryMaster ")

            dr = Ds_QueryMaster.Tables(0).NewRow()
            dr("vWorkspaceId") = Me.HFWorkspaceId.Value.Trim()
            dr("vProjectNo") = dt_heading.Rows(0).Item("vProjectNo").ToString.Trim()
            dr("vSubjectId") = Me.HFSubjectId.Value.Trim()
            dr("vMySubjectNo") = Convert.ToString(ds_WorkspaceSubjectMst.Tables(0).Rows(0)("vMySubjectNo")).Trim()
            dr("iNodeId") = Me.HFParentNodeId.Value
            dr("vActivityId") = Me.HFParentActivityId.Value
            dr("vNodeDisplayName") = Convert.ToString(ds_WorkspaceNodeDetail.Tables(0).Rows(0)("ParentActivityName")).Trim()
            dr("vRemarks") = Me.txtRemarkQuery.Text
            dr("cStatusIndi") = "N"
            dr("iCreatedBy") = Session(S_UserID)
            dr("iModifyBy") = Session(S_UserID)
            'dr("vParentActivityName") = Convert.ToString(ds_WorkspaceNodeDetail.Tables(0).Rows(0)("ParentActivityName")).Trim()
            Ds_QueryMaster.Tables(0).Rows.Add(dr)
            If Not objLambda.Save_QueryMaster(Me.ViewState(VS_Choice), Ds_QueryMaster, Me.Session(S_UserID), eStr) Then
                objCommon.ShowAlert("Error While Saving EmailSetupMst", Me.Page)
                Exit Sub
            End If

            If CheckReUpload.Checked Then
                QueryStatus = "R"
            End If

            If CheckQueryApprove.Checked Then
                QueryStatus = "A"
            End If

            If Not SaveQueryMstValues(QueryStatus) Then
                Me.objCommon.ShowAlert("Error While Assigning Data.", Me.Page)
            End If

            If Not SaveImageHdrQuerytails(QueryStatus) Then
                Me.objCommon.ShowAlert("Error While Assigning Data.", Me.Page)
            End If
            'Reviewstatus = CheckReUpload.SelectedValue.ToString()


            Me.objCommon.ShowAlert("Record Saved Sucessfully", Me.Page)
            ResetPage()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "QueryDetailsClose", "QueryDetailsClose();", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "Error While Saving")
        End Try
    End Sub
#End Region
    Private Function SaveQueryMstValues(QueryStatus As String) As Boolean
        Dim wstr As String = String.Empty
        Dim ds_QueryMaster As New DataSet
        Dim estr As String = String.Empty
        Dim Status As String = String.Empty

        'Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

        Try
            If Convert.ToString(ConfigurationManager.AppSettings("QCUserCode")).Contains(Session(S_UserType)) AndAlso QueryStatus = "A" Then
                Status = "QC1A"
            ElseIf Convert.ToString(ConfigurationManager.AppSettings("QCUserCode")).Contains(Session(S_UserType)) AndAlso QueryStatus = "R" Then
                Status = "QC1R"
            End If


            If Convert.ToString(ConfigurationManager.AppSettings("QC2UserCode")).Contains(Session(S_UserType)) AndAlso QueryStatus = "A" Then
                Status = "QC2A"
            ElseIf Convert.ToString(ConfigurationManager.AppSettings("QC2UserCode")).Contains(Session(S_UserType)) AndAlso QueryStatus = "R" Then
                Status = "QC2R"
            End If

            If Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(Session(S_UserType)) AndAlso QueryStatus = "A" Then
                Status = "CA1A"
            ElseIf Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(Session(S_UserType)) AndAlso QueryStatus = "R" Then
                Status = "CA1R"
            End If

            wstr = "select * from QueryMaster where  vWorkspaceId='" + Me.HFWorkspaceId.Value.Trim() + "' AND vSubjectId ='" + Me.HFSubjectId.Value.Trim() + "' AND vActivityId='" + HFParentActivityId.Value.Trim() + "' AND iNodeId='" + HFParentNodeId.Value.Trim() + "' AND cStatusIndi <> 'D' "
            ds_QueryMaster = objHelp.GetResultSet(wstr, "QueryMaster")

            wstr = ""

            If ds_QueryMaster.Tables(0).Rows.Count > 0 Then

                For Each dr As DataRow In ds_QueryMaster.Tables(0).Rows
                    dr("cQueryStatus") = Status
                Next

                'wstr = ds_QueryMaster.Tables(0).Rows(0)("iImgTransmittalHdrId").ToString() + "##" + QueryStatus


                ds_QueryMaster.AcceptChanges()
                If Not objLambda.Save_QueryMaster(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_QueryMaster, Me.Session(S_UserID), estr) Then
                    Me.objCommon.ShowAlert(estr, Me.Page)
                    Return False
                    Exit Function
                End If
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".......SaveQueryMstValues")
            Return False
        End Try
    End Function

    Private Function SaveImageHdrQuerytails(QueryStatus As String) As Boolean
        Dim wstr As String = String.Empty
        Dim ds_ImgTransmittalHdr As New DataSet
        Dim estr As String = String.Empty
        Dim Status As String = String.Empty

        'Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

        Try
            If Convert.ToString(ConfigurationManager.AppSettings("QCUserCode")).Contains(Session(S_UserType)) AndAlso QueryStatus = "A" Then
                Status = "QC1A"
            ElseIf Convert.ToString(ConfigurationManager.AppSettings("QCUserCode")).Contains(Session(S_UserType)) AndAlso QueryStatus = "R" Then
                Status = "QC1R"
            End If


            If Convert.ToString(ConfigurationManager.AppSettings("QC2UserCode")).Contains(Session(S_UserType)) AndAlso QueryStatus = "A" Then
                Status = "QC2A"
            ElseIf Convert.ToString(ConfigurationManager.AppSettings("QC2UserCode")).Contains(Session(S_UserType)) AndAlso QueryStatus = "R" Then
                Status = "QC2R"
            End If

            If Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(Session(S_UserType)) AndAlso QueryStatus = "A" Then
                Status = "CA1A"
            ElseIf Convert.ToString(ConfigurationManager.AppSettings("CAUserCode")).Contains(Session(S_UserType)) AndAlso QueryStatus = "R" Then
                Status = "CA1R"
            End If

            wstr = "select * from ImgTransmittalHdr where  vWorkspaceId='" + Me.HFWorkspaceId.Value.Trim() + "' AND vSubjectId ='" + Me.HFSubjectId.Value.Trim() + "' AND vActivityId='" + HFParentActivityId.Value.Trim() + "' AND iNodeId='" + HFParentNodeId.Value.Trim() + "' AND cStatusIndi <> 'D' "
            ds_ImgTransmittalHdr = objHelp.GetResultSet(wstr, "ImgTransmittalHdr")

            wstr = ""

            If ds_ImgTransmittalHdr.Tables(0).Rows.Count > 0 Then

                For Each dr As DataRow In ds_ImgTransmittalHdr.Tables(0).Rows
                    dr("cQueryStatus") = Status
                Next

                wstr = ds_ImgTransmittalHdr.Tables(0).Rows(0)("iImgTransmittalHdrId").ToString() + "##" + QueryStatus


                ds_ImgTransmittalHdr.AcceptChanges()
                If Not objLambda.Save_ImgTransmittalHdr(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_ImgTransmittalHdr, Me.Session(S_UserID), estr) Then
                    Me.objCommon.ShowAlert(estr, Me.Page)
                    Return False
                    Exit Function
                End If
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".......SaveImageHdrdetailValues")
            Return False
        End Try
    End Function

#Region "Reset Page"
    Private Sub ResetPage()
        Me.txtRemarkQuery.Text = ""
        Me.BtnSave.Text = "Save"
        Me.BtnSave.ToolTip = "Save"
    End Sub
#End Region
End Class
