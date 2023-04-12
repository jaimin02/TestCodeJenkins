Imports Newtonsoft.Json

Partial Class CDMS_frmCDMSStudyInformation
    Inherits System.Web.UI.Page

#Region "Variable Declaration "

    Private objCommon As New clsCommon
    Private objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

    Public Const VS_Choice As String = "Choice"
    Public Const VS_DtStudy As String = "DtStudy"
    Public Const VS_DtCDMSDtlHistory As String = "DtCDMSDtlHistory"
    Public Const VS_DtConsumption As String = "DtConsumption"
    Public Const VS_WorkspaceId As String = "WorkspaceId"
    Public Const VS_StudyInformationDtl As String = "StudyInformationDtl"

    Private Const GVCConsu_CDMSConsumptionNo As Integer = 0
    Private Const GVCConsu_Type As Integer = 1
    Private Const GVCConsu_Status As Integer = 2
    Private Const GVCConsu_Min As Integer = 3
    Private Const GVCConsu_Max As Integer = 4
    Private Const GVCConsu_Description As Integer = 5
    Private Const GVCConsu_Frequency As Integer = 6
    Private Const GVCConsu_StartDate As Integer = 7
    Private Const GVCConsu_EndDate As Integer = 8

    Private Const GVCConsuAudit_Code As Integer = 6

#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
                If Not GenCall() Then
                    Throw New Exception("Error While calling GenCall()")
                End If
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try
    End Sub

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim dt_Study As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try



            Choice = Me.Request.QueryString("Mode").ToString
            Me.ViewState(VS_Choice) = Choice

            If Not Me.Request.QueryString("WorkspaceId") Is Nothing Then
                Me.ViewState(VS_WorkspaceId) = Me.Request.QueryString("WorkspaceId").ToString
                Me.HProjectId.Value = Me.Request.QueryString("WorkspaceId").ToString
            End If

            If Not GenCall_Data(Choice, dt_Study) Then
                Exit Function
            End If

            Me.ViewState(VS_DtStudy) = dt_Study

            If Not GenCall_ShowUI(Choice, dt_Study) Then
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "..GenCall")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_Data"

    Private Function GenCall_Data(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                            ByRef dt_Dist_Retu As DataTable) As Boolean

        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_StudyDtl As DataSet = Nothing
        Dim ds_CDMSStudyDtlHistory As DataSet = Nothing
        Dim ds_CDMSStudyConsumption As DataSet = Nothing

        Try



            If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add _
               And Me.Request.QueryString("WorkspaceId") Is Nothing Then
                wStr = "1=2"
            Else
                wStr = "vWorkspaceId='" + Me.Request.QueryString("WorkspaceId").ToString.Trim() + "'"
            End If

            wStr += " And cStatusIndi <> 'D'"

            'StudyDtl
            If Not objhelpDb.getStudyDtlCDMS(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_StudyDtl, eStr) Then
                Throw New Exception(eStr)
            End If

            'StudyDtlCDMSHistory
            If Not objhelpDb.getStudyDtlCDMSHistory("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CDMSStudyDtlHistory, eStr) Then
                Throw New Exception(eStr)
            End If
            Me.ViewState(VS_DtCDMSDtlHistory) = ds_CDMSStudyDtlHistory.Tables(0)

            'StudyDtlCDMSConsumption
            If Not objhelpDb.View_StudyDtlCDMSConsumption(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CDMSStudyConsumption, eStr) Then
                Throw New Exception(eStr)
            End If
            Me.ViewState(VS_DtConsumption) = ds_CDMSStudyConsumption.Tables(0)


            If ds_StudyDtl Is Nothing Then
                Throw New Exception(eStr)
            End If

            'If ds_StudyDtl.Tables(0).Rows.Count <= 0 And _
            '   Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

            '    Throw New Exception("No Records Found...")

            'End If

            dt_Dist_Retu = ds_StudyDtl.Tables(0)
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_Data")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_ShowUI"

    Private Function GenCall_ShowUI(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                                      ByVal dt_ClientMst As DataTable) As Boolean
        Try

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Study Information"
            Me.Page.Title = " :: CDMS - Study Information ::" + System.Configuration.ConfigurationManager.AppSettings("Client")

            If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add _
               And Not Me.Request.QueryString("WorkspaceId") Is Nothing Then
                Me.btnSaveStudy.Visible = False
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StrfnAssignPropertiesGencall", "fnAssignProperties();", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "strfnApplyDataTableKeyGencall", "fnApplyDataTable();", True)
                If Not AssingAttribute() Then
                    Return False
                End If
            End If

            If Not FillConsumptionGrid() Then
                Return False
            End If

            If Not AssignValuesToControl() Then
                Return False
            End If

            If Me.Session(S_WorkFlowStageId) <> 0 Then
                Me.btnSaveStudy.Visible = False
            End If
            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_ShowUI")
        Finally
        End Try

    End Function

#End Region

#Region "Fill Controls"

    Private Function FillConsumptionGrid() As Boolean
        Dim ds_Consumption As New DataSet
        Dim eStr As String = String.Empty

        Try

            If Not objhelpDb.getCDMSConsumption("cStatusIndi <> 'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Consumption, eStr) Then
                Throw New Exception(eStr)
            End If

            Me.grdGeneralConmp.DataSource = ds_Consumption.Tables(0)
            Me.grdGeneralConmp.DataBind()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillConsumptionGrid()")
            Return False
        End Try
    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim ds_Study As New DataSet
        Dim eStr As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try
            Choice = Me.ViewState(VS_Choice)

            If Not AssignValues() Then
                Me.objCommon.ShowAlert("Error while Assinging Data", Me.Page)
                Exit Sub
            End If

            ds_Study.Tables.Add(CType(Me.ViewState(VS_DtStudy), DataTable).Copy())
            ds_Study.Tables.Add(CType(Me.ViewState(VS_DtCDMSDtlHistory), DataTable).Copy())
            ds_Study.Tables.Add(CType(Me.ViewState(VS_DtConsumption), DataTable).Copy())

            If Not objLambda.Save_StudyDtlCDMS(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Study, _
                                                 Me.Session(S_UserID), eStr) Then
                Me.ShowErrorMessage(eStr, "Error while saving studydtl.")
                Exit Sub
            End If
            Me.mdlSaveRedirect.Show()

            'Me.Response.Redirect("frmCDMSProjectMedicalCondition.aspx?Mode=1&WorkspaceId=" + Me.HProjectId.Value())

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnSaveStudy")
        End Try

    End Sub

    Protected Sub btnCancelCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelCancel.Click
        ''ResetPage()
        Me.Response.Redirect("frmCDMSStudyInformation.aspx?Mode=1")
    End Sub

    Protected Sub imgAuditConsumption_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgAuditConsumption.Click
        Dim ds_Audit As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            wStr = "vWorkSpaceId = '" + Me.ViewState(VS_WorkspaceId).ToString() + "' Order by cStatusIndi Desc"

            If Not objhelpDb.View_StudyDtlCDMSConsumption(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                               ds_Audit, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_Audit Is Nothing Then
                If ds_Audit.Tables(0).Rows.Count > 0 Then
                    Me.grdAudit.DataSource = ds_Audit
                    Me.grdAudit.DataBind()
                Else
                    Me.grdAudit.DataSource = Nothing
                    Me.grdAudit.DataBind()
                End If
            End If
            Me.mdlConsAudit.Show()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StrfnApplyDataTable", "fnApplyDataTable();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "fnConDataTablestr", "fnConDataTable();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "fnConDataTablestr1", "fnEditField();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "fnConDataTablestr2", "fnAuditTrail();", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillGrid")
        End Try
    End Sub

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click

        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_StudyInfo As DataSet = Nothing
        Dim ds_CDMSStudyDtl As DataSet = Nothing
        Dim ds_CDMSStudyConsumption As DataSet = Nothing


        Try

            Me.ViewState(VS_WorkspaceId) = Me.HProjectId.Value.Trim()
            Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

            wStr = "vWorkSpaceId='" + Me.ViewState(VS_WorkspaceId).ToString + "' And cStatusIndi <> 'D'"

            If Not objhelpDb.View_StudyInformationDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_StudyInfo, eStr) Then
                Throw New Exception(eStr)
            End If
            Me.ViewState(VS_StudyInformationDtl) = ds_StudyInfo.Tables(0)

            'StudyDtlCDMS
            If Not objhelpDb.getStudyDtlCDMS(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CDMSStudyDtl, eStr) Then
                Throw New Exception(eStr)
            End If
            Me.ViewState(VS_DtStudy) = ds_CDMSStudyDtl.Tables(0)

            'StudyDtlCDMSConsumption
            If Not objhelpDb.View_StudyDtlCDMSConsumption(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CDMSStudyConsumption, eStr) Then
                Throw New Exception(eStr)
            End If
            Me.ViewState(VS_DtConsumption) = ds_CDMSStudyConsumption.Tables(0)

            If Not AssignValuesToControl() Then
                Throw New Exception("AssignValuesToControl")
            End If

            If Not AssingAttribute() Then
                Throw New Exception("AssingAttribute")
            End If

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StrfnFunctionCall", "fnFunctionCall();", True)

            If Not ds_CDMSStudyDtl Is Nothing Then
                If ds_CDMSStudyDtl.Tables(0).Rows.Count > 0 Then
                    Me.btnSaveStudy.Visible = False
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StrfnAssignProperties", "fnAssignProperties();", True)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "strfnApplyDataTableKey", "fnApplyDataTable();", True)
                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...SearchProject()")
        End Try

    End Sub

    Protected Sub btnOkAlert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOkAlert.Click

        Me.Response.Redirect("frmCDMSProjectMedicalCondition.aspx?Mode=1&WorkspaceId=" + Me.HProjectId.Value())

    End Sub

    'Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    '    'ResetPage()
    '    Me.Response.Redirect("frmCDMSStudyInformation.aspx?Mode=1")
    'End Sub

#End Region

#Region "Assign Values"

    Private Function AssignValues() As Boolean

        Dim dt_StudyDtl As New DataTable
        Dim dt_CDMSDtlHistory As New DataTable
        Dim dt_CDMSDtlConsu As New DataTable
        Dim drStudyCDMSDtl As DataRow
        Dim drCDMSDtlHistory As DataRow
        Dim drCDMSDtlConsu As DataRow
        Dim strRace As String = String.Empty

        Try


            'Assign Values for StudyDtl
            dt_StudyDtl = Me.ViewState(VS_DtStudy)
            dt_StudyDtl.Clear()
            drStudyCDMSDtl = dt_StudyDtl.NewRow()

            drStudyCDMSDtl("vWorkSpaceId") = Me.HProjectId.Value.Trim()
            drStudyCDMSDtl("vSex") = Me.ddlSex.SelectedItem.Value.Trim()
            For Each lst As ListItem In Me.chkRace.Items
                If lst.Selected Then
                    strRace += lst.Value + ","
                End If
            Next
            If strRace = "" Then
                drStudyCDMSDtl("vRace") = System.DBNull.Value
            Else
                drStudyCDMSDtl("vRace") = strRace.Substring(0, strRace.Length - 1)
            End If
            drStudyCDMSDtl("vLocation") = Me.Session(S_LocationCode)
            drStudyCDMSDtl("vAgeMatchTo") = Me.ddlAgeMatchTo.SelectedItem.Value.Trim()
            drStudyCDMSDtl("iMenstrualCycleMin") = IIf(String.IsNullOrEmpty(Me.txtMenustralCycleMin.Text.Trim()), System.DBNull.Value, Me.txtMenustralCycleMin.Text.Trim())
            drStudyCDMSDtl("iMenstrualCycleMax") = IIf(String.IsNullOrEmpty(Me.txtMenstrualCycleMax.Text.Trim()), System.DBNull.Value, Me.txtMenstrualCycleMax.Text.Trim())
            drStudyCDMSDtl("vRegular") = Me.ddlRegular.SelectedItem.Value.Trim()
            drStudyCDMSDtl("vAvailiability") = Me.ddlAvailability.SelectedItem.Value.Trim()
            drStudyCDMSDtl("vRegularDiet") = Me.ddlRegularDiet.SelectedItem.Value.Trim()
            drStudyCDMSDtl("vSwallowPil") = Me.ddlSwallowPill.SelectedItem.Value.Trim()
            drStudyCDMSDtl("nMaleWeightMin") = IIf(String.IsNullOrEmpty(Me.txtMakeWeightMin.Text.Trim()), System.DBNull.Value, Me.txtMakeWeightMin.Text.Trim())
            drStudyCDMSDtl("nMaleWeightMax") = IIf(String.IsNullOrEmpty(Me.txtMaleWeightMax.Text.Trim()), System.DBNull.Value, Me.txtMaleWeightMax.Text.Trim())
            drStudyCDMSDtl("nFeMaleWeightMin") = IIf(String.IsNullOrEmpty(Me.txtFemaleWeightMin.Text.Trim()), System.DBNull.Value, Me.txtFemaleWeightMin.Text.Trim())
            drStudyCDMSDtl("nFeMaleWeightMax") = IIf(String.IsNullOrEmpty(Me.txtFemaleWeightMax.Text.Trim()), System.DBNull.Value, Me.txtFemaleWeightMax.Text.Trim())
            drStudyCDMSDtl("nBMIMin") = IIf(String.IsNullOrEmpty(Me.txtBMIMin.Text.Trim()), System.DBNull.Value, Me.txtBMIMin.Text.Trim())
            drStudyCDMSDtl("nBMIMax") = IIf(String.IsNullOrEmpty(Me.txtBMIMax.Text.Trim()), System.DBNull.Value, Me.txtBMIMax.Text.Trim())
            drStudyCDMSDtl("nAgeMin") = IIf(String.IsNullOrEmpty(Me.txtAgeMin.Text.Trim()), System.DBNull.Value, Me.txtAgeMin.Text.Trim())
            drStudyCDMSDtl("nAgeMax") = IIf(String.IsNullOrEmpty(Me.txtAgeMax.Text.Trim()), System.DBNull.Value, Me.txtAgeMax.Text.Trim())
            drStudyCDMSDtl("nBloodDraws") = IIf(String.IsNullOrEmpty(Me.txtNoDraws.Text.Trim()), System.DBNull.Value, Me.txtNoDraws.Text.Trim())
            drStudyCDMSDtl("nBloodRequired") = IIf(String.IsNullOrEmpty(Me.txtRequired.Text.Trim()), System.DBNull.Value, Me.txtRequired.Text.Trim())
            drStudyCDMSDtl("iWashPreStudy") = IIf(String.IsNullOrEmpty(Me.txtPreStudy.Text.Trim()), System.DBNull.Value, Me.txtPreStudy.Text.Trim())
            drStudyCDMSDtl("iWashPostStudy") = IIf(String.IsNullOrEmpty(Me.txtPostStudy.Text.Trim()), System.DBNull.Value, Me.txtPostStudy.Text.Trim())
            drStudyCDMSDtl("iWashPostDosage") = IIf(String.IsNullOrEmpty(Me.txtPostDosage.Text.Trim()), System.DBNull.Value, Me.txtPostDosage.Text.Trim())
            drStudyCDMSDtl("iStudySize") = IIf(String.IsNullOrEmpty(Me.txtStudySize.Text.Trim()), System.DBNull.Value, Me.txtStudySize.Text.Trim())
            drStudyCDMSDtl("iGroup") = IIf(String.IsNullOrEmpty(Me.txtGroup.Text.Trim()), System.DBNull.Value, Me.txtGroup.Text.Trim())
            drStudyCDMSDtl("iPeriods") = IIf(String.IsNullOrEmpty(Me.txtPeriods.Text.Trim()), System.DBNull.Value, Me.txtPeriods.Text.Trim())
            drStudyCDMSDtl("dStartDate") = IIf(String.IsNullOrEmpty(Me.txtStartDate.Text.Trim()), System.DBNull.Value, Me.txtStartDate.Text.Trim())
            drStudyCDMSDtl("dEndDate") = IIf(String.IsNullOrEmpty(Me.txtEnddate.Text.Trim()), System.DBNull.Value, Me.txtEnddate.Text.Trim())
            drStudyCDMSDtl("iStandby") = IIf(String.IsNullOrEmpty(Me.txtStandBy.Text.Trim()), System.DBNull.Value, Me.txtStandBy.Text.Trim())
            drStudyCDMSDtl("iModifyBy") = Me.Session(S_UserID)

            dt_StudyDtl.Rows.Add(drStudyCDMSDtl)
            dt_StudyDtl.TableName = "StudyDtlCDMS"
            Me.ViewState(VS_DtStudy) = dt_StudyDtl

            'Assign Values for StudyDtlCDMSHistory
            dt_CDMSDtlHistory = Me.ViewState(VS_DtCDMSDtlHistory)
            dt_CDMSDtlHistory.Clear()
            For Each dc As DataColumn In dt_StudyDtl.Columns
                If dc.ColumnName <> "nStudyDtlCDMSHistoryNo" And _
                   dc.ColumnName <> "iModifyBy" And _
                   dc.ColumnName <> "dModifyOn" And _
                   dc.ColumnName <> "cStatusIndi" Then
                    drCDMSDtlHistory = dt_CDMSDtlHistory.NewRow()
                    drCDMSDtlHistory("vWorkSpaceId") = Me.HProjectId.Value.Trim()
                    drCDMSDtlHistory("vColumnName") = dc.ColumnName
                    drCDMSDtlHistory("vChangedValue") = dt_StudyDtl.Rows(0)(dc.ColumnName).ToString
                    drCDMSDtlHistory("vRemarks") = ""
                    drCDMSDtlHistory("iModifyBy") = Me.Session(S_UserID)
                    dt_CDMSDtlHistory.Rows.Add(drCDMSDtlHistory)
                End If
            Next
            dt_CDMSDtlHistory.TableName = "StudyDtlCDMSHistory"
            Me.ViewState(VS_DtCDMSDtlHistory) = dt_CDMSDtlHistory

            dt_CDMSDtlConsu = Me.ViewState(VS_DtConsumption)
            dt_CDMSDtlConsu.Clear()
            'Assign Values for StudyDtlCDMSConsumption
            For Each grRow As GridViewRow In Me.grdGeneralConmp.Rows
                drCDMSDtlConsu = dt_CDMSDtlConsu.NewRow()
                drCDMSDtlConsu("nCDMSConsumptionNo") = grRow.Cells(GVCConsu_CDMSConsumptionNo).Text
                drCDMSDtlConsu("vWorkSpaceId") = Me.HProjectId.Value.Trim()
                drCDMSDtlConsu("vStatus") = CType(grRow.Cells(GVCConsu_Status).FindControl("ddlConmpStatus"), DropDownList).SelectedItem.Text.Trim()
                drCDMSDtlConsu("nMin") = IIf(String.IsNullOrEmpty(CType(grRow.Cells(GVCConsu_Min).FindControl("txtConmpMin"), TextBox).Text.Trim()), _
                                                  System.DBNull.Value, _
                                                  CType(grRow.Cells(GVCConsu_Min).FindControl("txtConmpMin"), TextBox).Text.Trim())
                drCDMSDtlConsu("nMax") = IIf(String.IsNullOrEmpty(CType(grRow.Cells(GVCConsu_Max).FindControl("txtConmpMax"), TextBox).Text.Trim()), _
                                                 System.DBNull.Value, _
                                                 CType(grRow.Cells(GVCConsu_Max).FindControl("txtConmpMax"), TextBox).Text.Trim())
                drCDMSDtlConsu("vFrequency") = CType(grRow.Cells(GVCConsu_Frequency).FindControl("ddlConmpFrequency"), DropDownList).SelectedItem.Text.Trim()
                drCDMSDtlConsu("dStartDate") = IIf(String.IsNullOrEmpty(CType(grRow.Cells(GVCConsu_StartDate).FindControl("txtStartDate"), TextBox).Text.Trim()), _
                                                   System.DBNull.Value, _
                                                   CType(grRow.Cells(GVCConsu_StartDate).FindControl("txtStartDate"), TextBox).Text.Trim())
                drCDMSDtlConsu("dEndDate") = IIf(String.IsNullOrEmpty(CType(grRow.Cells(GVCConsu_EndDate).FindControl("txtEndDate"), TextBox).Text.Trim()), _
                                                 System.DBNull.Value, _
                                                 CType(grRow.Cells(GVCConsu_EndDate).FindControl("txtEndDate"), TextBox).Text.Trim())
                drCDMSDtlConsu("iModifyBy") = Me.Session(S_UserID)
                dt_CDMSDtlConsu.Rows.Add(drCDMSDtlConsu)
            Next
            dt_CDMSDtlConsu.TableName = "StudyDtlCDMSConsumption"
            Me.ViewState(VS_DtConsumption) = dt_CDMSDtlConsu

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignValues()")
            Return False
        End Try


    End Function

    Private Function AssignValuesToControl() As Boolean

        Dim dt_StudyInformation As New DataTable
        Dim dt_Study As New DataTable
        Dim dt_StudyConsumption As New DataTable
        Dim dsProject As New DataSet
        Dim ds_Study As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            ResetPage()

            wStr = "vWorkspaceId = '" + Me.HProjectId.Value + "' And iUserid = " + Me.Session(S_UserID)
            If Not objhelpDb.View_MyProjects(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                           dsProject, eStr) Then
                Throw New Exception("Error while getting Project.")
                Exit Function
            End If

            If Not dsProject Is Nothing Then
                If dsProject.Tables(0).Rows.Count > 0 Then
                    Me.txtproject.Text = "[" + dsProject.Tables(0).Rows(0)("vProjectNo").ToString + "] " + dsProject.Tables(0).Rows(0)("vRequestId").ToString
                End If
            End If

            If Not Me.ViewState(VS_WorkspaceId) Is Nothing Then
                wStr = "vWorkSpaceId='" + Me.HProjectId.Value + "' And cStatusIndi <> 'D'"
                If Not objhelpDb.View_StudyInformationDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Study, eStr) Then
                    Throw New Exception(eStr)
                End If
                Me.ViewState(VS_StudyInformationDtl) = ds_Study.Tables(0)
            End If


            If Not Me.ViewState(VS_StudyInformationDtl) Is Nothing Then
                dt_StudyInformation = Me.ViewState(VS_StudyInformationDtl)
                If dt_StudyInformation.Rows.Count > 0 Then
                    Me.txtStudy.Text = dt_StudyInformation.Rows(0)("vProjectNo").ToString
                    Me.txtStudyType.Text = dt_StudyInformation.Rows(0)("vStudyDesign").ToString
                    Me.txtDrug.Text = dt_StudyInformation.Rows(0)("vDrugName").ToString
                    Me.txtSponsorName.Text = dt_StudyInformation.Rows(0)("vClientName").ToString
                    Me.txtSponsorAddress.Text = dt_StudyInformation.Rows(0)("vAddress1").ToString
                    Me.txtStudySize.Text = dt_StudyInformation.Rows(0)("iNoOfSubjects").ToString
                    Me.txtPeriods.Text = dt_StudyInformation.Rows(0)("iNoOfPeriods").ToString
                End If
            End If

            If Not Me.ViewState(VS_DtStudy) Is Nothing Then
                dt_Study = Me.ViewState(VS_DtStudy)
                If dt_Study.Rows.Count > 0 Then

                    If dt_Study.Rows(0)("dStartDate").ToString() <> "" Then
                        If IsDate(dt_Study.Rows(0)("dStartDate").ToString()) Then
                            Me.txtStartDate.Text = DateTime.Parse(dt_Study.Rows(0)("dStartDate")).ToString("dd-MMM-yyyy")
                        Else
                            Me.txtStartDate.Text = dt_Study.Rows(0)("dStartDate").ToString()
                        End If
                    End If
                    If dt_Study.Rows(0)("dEndDate").ToString() <> "" Then
                        If IsDate(dt_Study.Rows(0)("dEndDate").ToString()) Then
                            Me.txtEnddate.Text = DateTime.Parse(dt_Study.Rows(0)("dEndDate")).ToString("dd-MMM-yyyy")
                        Else
                            Me.txtEnddate.Text = dt_Study.Rows(0)("dEndDate").ToString()
                        End If
                    End If
                    Me.txtStudySize.Text = dt_Study.Rows(0)("iStudySize").ToString
                    Me.txtGroup.Text = dt_Study.Rows(0)("iGroup").ToString
                    Me.txtPeriods.Text = dt_Study.Rows(0)("iPeriods").ToString
                    Me.txtNoDraws.Text = dt_Study.Rows(0)("nBloodDraws").ToString
                    Me.txtRequired.Text = dt_Study.Rows(0)("nBloodRequired").ToString
                    Me.txtPreStudy.Text = dt_Study.Rows(0)("iWashPreStudy").ToString
                    Me.txtPostStudy.Text = dt_Study.Rows(0)("iWashPostStudy").ToString
                    Me.txtPostDosage.Text = dt_Study.Rows(0)("iWashPostDosage").ToString
                    Me.ddlSex.SelectedIndex = Me.ddlSex.Items.IndexOf(Me.ddlSex.Items.FindByValue(dt_Study.Rows(0)("vSex").ToString))
                    For Each Str As String In dt_Study.Rows(0)("vRace").ToString.Split(",")
                        For Each lst As ListItem In Me.chkRace.Items
                            If lst.Value = Str Then
                                lst.Selected = True
                            End If
                        Next
                    Next
                    Me.ddlAgeMatchTo.SelectedIndex = Me.ddlAgeMatchTo.Items.IndexOf(Me.ddlAgeMatchTo.Items.FindByValue(dt_Study.Rows(0)("vAgeMatchTo").ToString))
                    Me.ddlAvailability.SelectedIndex = Me.ddlAvailability.Items.IndexOf(Me.ddlAvailability.Items.FindByValue(dt_Study.Rows(0)("vAvailiability").ToString))
                    Me.ddlRegularDiet.SelectedIndex = Me.ddlRegularDiet.Items.IndexOf(Me.ddlRegularDiet.Items.FindByValue(dt_Study.Rows(0)("vRegularDiet").ToString))
                    Me.ddlSwallowPill.SelectedIndex = Me.ddlSwallowPill.Items.IndexOf(Me.ddlSwallowPill.Items.FindByValue(dt_Study.Rows(0)("vSwallowPil").ToString))
                    Me.ddlRegular.SelectedIndex = Me.ddlRegular.Items.IndexOf(Me.ddlRegular.Items.FindByValue(dt_Study.Rows(0)("vRegular").ToString))
                    Me.txtMakeWeightMin.Text = dt_Study.Rows(0)("nMaleWeightMin").ToString
                    Me.txtMaleWeightMax.Text = dt_Study.Rows(0)("nMaleWeightMax").ToString
                    Me.txtFemaleWeightMin.Text = dt_Study.Rows(0)("nFeMaleWeightMin").ToString
                    Me.txtFemaleWeightMax.Text = dt_Study.Rows(0)("nFeMaleWeightMax").ToString
                    Me.txtBMIMax.Text = dt_Study.Rows(0)("nBMIMax").ToString
                    Me.txtBMIMin.Text = dt_Study.Rows(0)("nBMIMin").ToString
                    Me.txtAgeMax.Text = dt_Study.Rows(0)("nAgeMax").ToString
                    Me.txtAgeMin.Text = dt_Study.Rows(0)("nAgeMin").ToString
                    Me.txtMenstrualCycleMax.Text = dt_Study.Rows(0)("iMenstrualCycleMax").ToString
                    Me.txtMenustralCycleMin.Text = dt_Study.Rows(0)("iMenstrualCycleMin").ToString
                    Me.txtStandBy.Text = dt_Study.Rows(0)("iStandBy").ToString
                End If
            End If

            If Not Me.ViewState(VS_DtConsumption) Is Nothing Then
                dt_StudyConsumption = Me.ViewState(VS_DtConsumption)
                If dt_StudyConsumption.Rows.Count > 0 Then
                    grdGeneralConmp.DataSource = dt_StudyConsumption
                    grdGeneralConmp.DataBind()
                    For Each grdRow As GridViewRow In grdGeneralConmp.Rows
                        If dt_StudyConsumption.Rows(grdRow.RowIndex)("dStartDate").ToString() <> "" Then
                            If (IsDate(dt_StudyConsumption.Rows(grdRow.RowIndex)("dStartDate"))) Then
                                CType(grdRow.Cells(GVCConsu_StartDate).FindControl("txtStartDate"), TextBox).Text = DateTime.Parse(dt_StudyConsumption.Rows(grdRow.RowIndex)("dStartDate")).ToString("dd-MMM-yyyy")
                            Else
                                CType(grdRow.Cells(GVCConsu_StartDate).FindControl("txtStartDate"), TextBox).Text = dt_StudyConsumption.Rows(grdRow.RowIndex)("dStartDate")
                            End If
                        End If
                        If dt_StudyConsumption.Rows(grdRow.RowIndex)("dEndDate").ToString() <> "" Then
                            If (IsDate(dt_StudyConsumption.Rows(grdRow.RowIndex)("dEndDate"))) Then
                                CType(grdRow.Cells(GVCConsu_EndDate).FindControl("txtEndDate"), TextBox).Text = DateTime.Parse(dt_StudyConsumption.Rows(grdRow.RowIndex)("dEndDate")).ToString("dd-MMM-yyyy")
                            Else
                                CType(grdRow.Cells(GVCConsu_EndDate).FindControl("txtEndDate"), TextBox).Text = dt_StudyConsumption.Rows(grdRow.RowIndex)("dEndDate")
                            End If
                        End If
                        CType(grdRow.Cells(GVCConsu_Min).FindControl("txtConmpMin"), TextBox).Text = dt_StudyConsumption.Rows(grdRow.RowIndex)("nMin").ToString()
                        CType(grdRow.Cells(GVCConsu_Max).FindControl("txtConmpMax"), TextBox).Text = dt_StudyConsumption.Rows(grdRow.RowIndex)("nMax").ToString()
                        CType(grdRow.Cells(GVCConsu_Status).FindControl("ddlConmpStatus"), DropDownList).SelectedValue = dt_StudyConsumption.Rows(grdRow.RowIndex)("vStatus").ToString()
                        CType(grdRow.Cells(GVCConsu_Frequency).FindControl("ddlConmpFrequency"), DropDownList).SelectedValue = dt_StudyConsumption.Rows(grdRow.RowIndex)("vFrequency").ToString()
                    Next
                End If
            End If

            If Not dt_Study Is Nothing Then
                If dt_Study.Rows.Count > 0 Then
                    Me.btnSaveStudy.Visible = False
                End If
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssingValuesToControl()")
            Return False
        End Try

    End Function

#End Region

#Region "Grid Events"

    Protected Sub grdAudit_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAudit.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("imgAudit"), ImageButton).CommandName = "Audit"
            CType(e.Row.FindControl("imgAudit"), ImageButton).CommandArgument = e.Row.RowIndex
        End If

    End Sub

    Protected Sub grdAudit_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdAudit.RowCommand

        Dim ds_Audit As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try
            If e.CommandName = "Audit" Then

                wStr = "vWorkSpaceId = '" + Me.ViewState(VS_WorkspaceId).ToString() + "' And vConsumptionCode = '" + _
                        CType(grdAudit.Rows(e.CommandArgument).Cells(GVCConsuAudit_Code).FindControl("hdnCode"), HiddenField).Value + "'"

                If Not objhelpDb.View_AuditStudyDtlCDMSConsumption(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                   ds_Audit, eStr) Then
                    Throw New Exception(eStr)
                End If

                If Not ds_Audit Is Nothing Then
                    If ds_Audit.Tables(0).Rows.Count > 0 Then
                        ds_Audit.Tables(0).Columns.Add("dModifyOffSet", Type.GetType("System.String"))
                        For Each dr_Audit In ds_Audit.Tables(0).Rows
                            dr_Audit("dModifyOffSet") = Convert.ToString(CDate(dr_Audit("dModifyOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                        Next
                        Me.grdRowAudit.DataSource = ds_Audit
                        Me.grdRowAudit.DataBind()
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StrfnApplyDataTableAudit", "fnApplyDataTable();", True)
                        Me.mdlRowAudit.Show()
                    Else
                        Me.grdRowAudit.DataSource = Nothing
                        Me.grdRowAudit.DataBind()
                    End If
                End If

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...grdAudit_RowCommand()")
        End Try

    End Sub

    Protected Sub grdGeneralConmp_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdGeneralConmp.RowDataBound

        If e.Row.RowType = DataControlRowType.Header Or _
           e.Row.RowType = DataControlRowType.DataRow Or _
           e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GVCConsu_CDMSConsumptionNo).Visible = False
        End If
    End Sub

#End Region

#Region "Web Methods"

    <Web.Services.WebMethod()> _
    Public Shared Function UpdateFieldValues(ByVal WorkspaceId As String, ByVal ColumnName As String, _
                                             ByVal TableName As String, ByVal ChangedValue As String, _
                                             ByVal Remarks As String, ByVal JSONString As String) As Boolean
        Dim objCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

        Dim eStr As String = String.Empty

        Dim ds_Consumption As New DataSet
        Dim ds_Field As New DataSet
        Dim dt_Field As New DataTable
        Dim dr_Field As DataRow

        Try

            If JSONString = "" Then
                dt_Field.Columns.Add("vWorkSpaceId", Type.GetType("System.String"))
                dt_Field.Columns.Add("vTableName", Type.GetType("System.String"))
                dt_Field.Columns.Add("vColumnName", Type.GetType("System.String"))
                dt_Field.Columns.Add("vChangedValue", Type.GetType("System.String"))
                dt_Field.Columns.Add("vRemarks", Type.GetType("System.String"))
                dt_Field.Columns.Add("iModifyBy", Type.GetType("System.Int32"))


                dr_Field = dt_Field.NewRow
                dr_Field("vWorkSpaceId") = WorkspaceId
                dr_Field("vTableName") = TableName
                dr_Field("vColumnName") = ColumnName
                dr_Field("vChangedValue") = ChangedValue
                dr_Field("vRemarks") = Remarks
                dr_Field("iModifyBy") = HttpContext.Current.Session(S_UserID)
                dt_Field.Rows.Add(dr_Field)

                ds_Field.Tables.Add(dt_Field)

                If Not objLambda.Insert_UpdateStudyCDMSDtlField(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Field, _
                                                     HttpContext.Current.Session(S_UserID), eStr) Then
                    Return False
                    Exit Function
                End If
            Else

                If Not objhelpDb.getStudyDtlCDMSConsumption("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Consumption, eStr) Then
                    Throw New Exception(eStr)
                End If

                ds_Field.Tables.Add(JsonConvert.DeserializeObject(JSONString, GetType(System.Data.DataTable)))

                ds_Consumption.Tables(0).Columns.Add("vRemarks", System.Type.GetType("System.String"))

                For Each dr As DataRow In ds_Field.Tables(0).Rows
                    dr_Field = ds_Consumption.Tables(0).NewRow()
                    dr_Field("nStudyDtlCDMSConsumptionNo") = 1
                    dr_Field("vWorkSpaceId") = WorkspaceId
                    dr_Field("nCDMSConsumptionNo") = dr("nCDMSConsumptionNo")
                    dr_Field("vStatus") = dr("vStatus")
                    dr_Field("nMin") = IIf(String.IsNullOrEmpty(dr("nMin")), System.DBNull.Value, dr("nMin"))
                    dr_Field("nMax") = IIf(String.IsNullOrEmpty(dr("nMax")), System.DBNull.Value, dr("nMax"))
                    dr_Field("vFrequency") = dr("vFrequency")
                    dr_Field("dStartDate") = IIf(String.IsNullOrEmpty(dr("dStartDate")), System.DBNull.Value, dr("dStartDate"))
                    dr_Field("dEndDate") = IIf(String.IsNullOrEmpty(dr("dEndDate")), System.DBNull.Value, dr("dEndDate"))
                    dr_Field("iModifyBy") = HttpContext.Current.Session(S_UserID)
                    dr_Field("dModifyOn") = DateTime.Now
                    dr_Field("vRemarks") = Remarks
                    ds_Consumption.Tables(0).Rows.Add(dr_Field)
                Next

                If Not objLambda.Save_StudyDtlCDMSConsumption(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Consumption, _
                                                     HttpContext.Current.Session(S_UserID), eStr) Then
                    Return False
                    Exit Function
                End If

            End If

            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function GetAuditTrailField(ByVal WorkspaceId As String, ByVal ColumnName As String) As String
        Dim objCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_Audit As New DataSet
        Dim RowIndex As Integer = 1

        Try

            wStr = "vWorkSpaceId = '" + WorkspaceId + "' And vColumnName = '" + ColumnName + "' Order by dModifyOn "

            If Not objhelpDb.View_AuditStudyDtlCDMS(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Audit, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_Audit Is Nothing Then
                If ds_Audit.Tables(0).Rows.Count > 0 Then
                    ds_Audit.Tables(0).TableName = "tblAudit"
                    ds_Audit.Tables(0).Columns.Add("dModifyOffSet", Type.GetType("System.String"))
                    For Each dr_Audit In ds_Audit.Tables(0).Rows
                        '' Added by Rahul 
                        If dr_Audit("vColumnName").ToString().ToUpper = "DBIRTHDATE" Then
                            dr_Audit("vChangedValue") = Convert.ToString(CDate(dr_Audit("vChangedValue")).ToString("dd-MMM-yyyy"))
                        End If
                        '' Ended

                        dr_Audit("dModifyOffSet") = Convert.ToString(CDate(dr_Audit("dModifyOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                    Next
                    Return JsonConvert.SerializeObject(ds_Audit.Tables(0))
                End If
            End If

        Catch ex As Exception
            Return ex.Message
        End Try

    End Function

#End Region

#Region "Helper Function"

    Private Sub ResetPage()
        Me.btnSaveStudy.Visible = True
        Me.txtStudy.Text = ""
        Me.txtStudyType.Text = ""
        Me.txtDrug.Text = ""
        Me.txtStartDate.Text = ""
        Me.txtEnddate.Text = ""
        Me.txtStudySize.Text = ""
        Me.txtGroup.Text = ""
        Me.txtPeriods.Text = ""
        Me.txtNoDraws.Text = ""
        Me.txtRequired.Text = ""
        Me.txtPreStudy.Text = ""
        Me.txtPostStudy.Text = ""
        Me.txtPostDosage.Text = ""
        Me.ddlSex.SelectedIndex = -1
        For Each lst As ListItem In Me.chkRace.Items
            lst.Selected = False
        Next
        Me.ddlAgeMatchTo.SelectedIndex = -1
        Me.ddlRegular.SelectedIndex = -1
        Me.ddlAvailability.SelectedIndex = -1
        Me.ddlRegularDiet.SelectedIndex = -1
        Me.ddlSwallowPill.SelectedIndex = -1
        Me.txtMakeWeightMin.Text = ""
        Me.txtMaleWeightMax.Text = ""
        Me.txtFemaleWeightMax.Text = ""
        Me.txtFemaleWeightMin.Text = ""
        Me.txtAgeMax.Text = ""
        Me.txtAgeMin.Text = ""
        Me.txtBMIMax.Text = ""
        Me.txtBMIMin.Text = ""
        Me.txtMenstrualCycleMax.Text = ""
        Me.txtMenustralCycleMin.Text = ""
        Me.txtStandBy.Text = ""
    End Sub

    Private Function AssingAttribute() As Boolean

        Try

            Me.txtStartDate.Attributes.Add("cName", "dStartDate")
            Me.txtEnddate.Attributes.Add("cName", "dEndDate")
            Me.txtStudySize.Attributes.Add("cName", "iStudySize")
            Me.txtGroup.Attributes.Add("cName", "iGroup")
            Me.txtPeriods.Attributes.Add("cName", "iPeriods")
            Me.txtNoDraws.Attributes.Add("cName", "nBloodDraws")
            Me.ddlSex.Attributes.Add("cName", "vSex")
            Me.txtRequired.Attributes.Add("cName", "nBloodRequired")
            Me.txtPreStudy.Attributes.Add("cName", "iWashPreStudy")
            Me.txtPostDosage.Attributes.Add("cName", "iWashPostDosage")
            Me.txtPostStudy.Attributes.Add("cName", "iWashPostStudy")
            Me.ddlRegular.Attributes.Add("cName", "vRegular")
            Me.chkRace.Attributes.Add("cName", "vRace")
            Me.ddlAgeMatchTo.Attributes.Add("cName", "vAgeMatchTo")
            Me.ddlAvailability.Attributes.Add("cName", "vAvailiability")
            Me.ddlRegularDiet.Attributes.Add("cName", "vRegularDiet")
            Me.ddlSwallowPill.Attributes.Add("cName", "vSwallowPil")
            Me.txtMaleWeightMax.Attributes.Add("cName", "nMaleWeightMax")
            Me.txtMakeWeightMin.Attributes.Add("cName", "nMaleWeightMin")
            Me.txtFemaleWeightMax.Attributes.Add("cName", "nFeMaleWeightMax")
            Me.txtFemaleWeightMin.Attributes.Add("cName", "nFeMaleWeightMin")
            Me.txtBMIMin.Attributes.Add("cName", "nBMIMin")
            Me.txtBMIMax.Attributes.Add("cName", "nBMIMax")
            Me.txtAgeMax.Attributes.Add("cName", "nAgeMax")
            Me.txtAgeMin.Attributes.Add("cName", "nAgeMin")
            Me.txtMenstrualCycleMax.Attributes.Add("cName", "iMenstrualCycleMax")
            Me.txtMenustralCycleMin.Attributes.Add("cName", "iMenstrualCycleMin")
            Me.txtStandBy.Attributes.Add("cName", "iStandBy")

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignAttribute()")
            Return False
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

End Class
