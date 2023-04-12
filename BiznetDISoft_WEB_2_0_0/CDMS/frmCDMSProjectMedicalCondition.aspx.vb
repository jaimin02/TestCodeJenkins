Imports Newtonsoft.Json
Partial Class CDMS_frmCDMSProjectMedicalCondition
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATIONS"

    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()


    Private Const VS_DtConcoMedication As String = "ConcoMedication"
    Private Const VS_ProjectID As String = "ProjectID"
    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtMedCondition As String = "Choice"

    Private Const GVCAddMedication_Select As Integer = 0
    Private Const GVCAddMedication_IdCode As Integer = 1
    Private Const GVCAddMedication_Class As Integer = 2
    Private Const GVCAddMedication_Description As Integer = 3

    Private Const GVCAddMedCond_Select As Integer = 0
    Private Const GVCAddMedCond_IdCode As Integer = 1
    Private Const GVCAddMedCond_Type As Integer = 2
    Private Const GVCAddMedCond_SubType As Integer = 3
    Private Const GVCAddMedCond_Description As Integer = 4
    Private Const GVCAddMedCond_Symptom As Integer = 5

    Private Const GVCCondiAudit_Code As Integer = 0
    Private Const GVCCondiAudit_Status As Integer = 4
    Private Const GVCCondiAudit_Audit As Integer = 5

    Private Const GVCMediAudit_Type As Integer = 0
    Private Const GVCMediAudit_Description As Integer = 2
    Private Const GVCMediAudit_Status As Integer = 3
    Private Const GVCMediAudit_Audit As Integer = 4

    Private Const GVCgrdMedicalCondition_Type As Integer = 0
    Private Const GVCgrdMedicalCondition_SubType As Integer = 1
    Private Const GVCgrdMedicalCondition_Description As Integer = 2
    Private Const GVCgrdMedicalCondition_Symptom As Integer = 3
    Private Const GVCgrdMedicalCondition_OnsetDate As Integer = 4
    Private Const GVCgrdMedicalCondition_ResolutionDate As Integer = 5
    Private Const GVCgrdMedicalCondition_Criteria As Integer = 6
    Private Const GVCgrdMedicalCondition_Action As Integer = 7

    Private Const GVCConcoMedi_Code As Integer = 1
    Private Const GVCConcoMedi_Class As Integer = 2
    Private Const GVCConcoMedi_Description As Integer = 3
    Private Const GVCConcoMedi_Dosage As Integer = 4
    Private Const GVCConcoMedi_StartDate As Integer = 5
    Private Const GVCConcoMedi_EndDate As Integer = 6
    Private Const GVCConcoMedi_Criteria As Integer = 7
    Private Const GVCConcoMedi_Action As Integer = 8

#End Region

#Region "Page Load And GenCalls"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
            GenCall()
        End If
    End Sub

    Private Sub GenCall()

        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim dsProject As New DataSet

        Try
            If Not Me.Request.QueryString("WorkspaceId") Is Nothing Then
                Me.HProjectId.Value = Me.Request.QueryString("WorkspaceId").ToString
                Me.ViewState(VS_ProjectID) = Me.Request.QueryString("WorkspaceId").ToString

                wStr = "vWorkspaceId = '" + Me.HProjectId.Value + "' And iUserid = " + Me.Session(S_UserID)
                If Not objHelp.View_MyProjects(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                               dsProject, eStr) Then
                    Throw New Exception("Error while getting Project.")
                    Exit Sub
                End If

                If Not dsProject Is Nothing Then
                    If dsProject.Tables(0).Rows.Count > 0 Then
                        Me.txtproject.Text = "[" + dsProject.Tables(0).Rows(0)("vProjectNo").ToString + "] " + dsProject.Tables(0).Rows(0)("vRequestId").ToString
                    End If
                End If
            End If
            GenCallData()
            GenCallShowUI()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall()")
        End Try

    End Sub

    Private Sub GenCallData()

    End Sub

    Private Sub GenCallShowUI()

        Try

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Study Information - Medical Condition"

            If Not FillMedicalListGrid() Then
                Me.ShowErrorMessage("Error While Filling GridMedical List", "")
                Exit Sub
            End If
            If Not FillConcoListGrid() Then
                Me.ShowErrorMessage("Error While Filling GridConco LIst", "")
                Exit Sub
            End If

            If Not FillGridMedicalconditions() Then
                Exit Sub
            End If
            If Not FillGridConcoMedicalCondtions() Then
                Exit Sub
            End If

            If Me.Session(S_WorkFlowStageId) <> 0 Then
                Me.btnAddMoreMediCond.Visible = False
                Me.btnHistoryCondition.Visible = False
                Me.btnHistoryMedication.Visible = False
            End If

            Page.Title = " :: CDMS Study Information ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            txtOnsetDate.Attributes.Add("OnChange", "DateConvertForScreening(this.value,this)")
            txtResolutionDate.Attributes.Add("OnChange", "DateConvertForScreening(this.value,this)")
            txtStartDateConco.Attributes.Add("OnChange", "DateConvertForScreening(this.value,this)")
            txtEndDateConco.Attributes.Add("OnChange", "DateConvertForScreening(this.value,this)")


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

#End Region

#Region "Fill List Grid"

    Private Function FillMedicalListGrid() As Boolean
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_MediCondiList As New DataSet

        Try


            wStr = " cStatusIndi <> 'D' Order by CodeMedicalConditions "

            If Not objHelp.getMedicalConditionsList(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                      ds_MediCondiList, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_MediCondiList Is Nothing Then
                If ds_MediCondiList.Tables(0).Rows.Count > 0 Then
                    Me.grdAddMedicalCond.DataSource = ds_MediCondiList
                    Me.grdAddMedicalCond.DataBind()
                End If
            End If

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....FillGridList")
            Return False
        End Try
    End Function

    Private Function FillConcoListGrid() As Boolean
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_MedicationList As New DataSet

        Try


            wStr = " cStatusIndi <> 'D' Order by CodeClassOfMedication "

            If Not objHelp.getCodeConcoMedicationList(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                      ds_MedicationList, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_MedicationList Is Nothing Then
                If ds_MedicationList.Tables(0).Rows.Count > 0 Then
                    Me.grdAddConcoMedi.DataSource = ds_MedicationList
                    Me.grdAddConcoMedi.DataBind()
                End If
            End If

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....FillGridList")
            Return False
        End Try
    End Function

    Public Function FillGridConcoMedicalCondtions() As Boolean

        Dim ds_Medication As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try
            If Me.rblconco.SelectedValue <> "" Then

                wStr = "vWorkSpaceId = '" + Me.ViewState(VS_ProjectID).ToString() + "'" + "And cStatusIndi <> 'D'" + " AND vCriteria = '" + Me.rblconco.SelectedValue.ToString() + "'"

            Else
                wStr = "vWorkSpaceId = '" + Me.ViewState(VS_ProjectID).ToString() + "' And cStatusIndi <> 'D'"
            End If

            If Not objHelp.getProjectDtlCDMSConcoMedication(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                               ds_Medication, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_Medication Is Nothing Then
                If ds_Medication.Tables(0).Rows.Count > 0 Then
                    Me.grdConcoMedi.DataSource = ds_Medication
                    Me.grdConcoMedi.DataBind()
                    Me.ViewState(VS_DtConcoMedication) = ds_Medication.Tables(0)
                    ds_Medication.Tables(0).DefaultView.RowFilter = "cStatusIndi = 'E'"
                    Me.btnHistoryMedication.Text = "History (" + ds_Medication.Tables(0).DefaultView.ToTable().Rows.Count.ToString + ")"
                Else
                    Me.grdConcoMedi.DataSource = Nothing
                    Me.grdConcoMedi.DataBind()
                    Me.ViewState(VS_DtConcoMedication) = ds_Medication.Tables(0)
                End If
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillGrid")
            Return False
        End Try
    End Function

    Public Function FillGridMedicalconditions() As Boolean

        Dim ds_MediCond As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try
            If Me.rblMedi.SelectedValue <> "" Then
                wStr = "vWorkSpaceId = '" + Me.ViewState(VS_ProjectID).ToString() + "'" + "And cStatusIndi <> 'D'" + " AND vCriteria = '" + Me.rblMedi.SelectedValue.ToString() + "'"
            Else
                wStr = "vWorkspaceID = '" + Me.ViewState(VS_ProjectID).ToString() + "' And cStatusIndi <> 'D'"
            End If

            If Not objHelp.getProjectDtlCDMSMedicalCondition(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                               ds_MediCond, eStr) Then
                Throw New Exception(eStr)
            End If


            If Not ds_MediCond Is Nothing Then
                If ds_MediCond.Tables(0).Rows.Count > 0 Then
                    Me.grdMedicalCondition.DataSource = ds_MediCond
                    Me.grdMedicalCondition.DataBind()
                    Me.ViewState(VS_DtMedCondition) = ds_MediCond.Tables(0)
                    ds_MediCond.Tables(0).DefaultView.RowFilter = "cStatusIndi = 'E'"
                    Me.btnHistoryCondition.Text = "History (" + ds_MediCond.Tables(0).DefaultView.ToTable().Rows.Count.ToString + ")"
                Else
                    Me.grdMedicalCondition.DataSource = Nothing
                    Me.grdMedicalCondition.DataBind()
                    Me.ViewState(VS_DtMedCondition) = ds_MediCond.Tables(0)
                End If
            End If

            ''If Not ds_MediCond Is Nothing Then
            ''    If ds_MediCond.Tables(0).Rows.Count > 0 Then
            ''        Me.grdMedicalCondition.DataSource = ds_MediCond
            ''        Me.grdMedicalCondition.DataBind()
            ''        Me.ViewState(VS_DtMedCondition) = ds_MediCond.Tables(0)
            'ds_Medication.Tables(0).DefaultView.RowFilter = "cStatusIndi = 'E'"
            'Me.btnHistory.Text = "History (" + ds_Medication.Tables(0).DefaultView.ToTable().Rows.Count.ToString + ")"
            ''    End If
            ''End If
            'Me.grdMedicalCondition.DataSource = ds_MediCond
            'Me.grdMedicalCondition.DataBind()
            'Me.ViewState(VS_DtMedCondition) = ds_MediCond.Tables(0)

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillGrid")
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

#Region "Assing Values"

    Private Function AssignValuesConco(ByVal Type As String, ByRef ds_Save As DataSet) As Boolean

        Dim ds_Medication As DataSet = Nothing
        Dim dt_MedicationEdit As DataTable = Nothing
        'Dim ds_MedicationEdit As DataSet = Nothing
        Dim dt_MedicationEditn As DataTable = Nothing
        Dim dr_Medication As DataRow
        Dim Wstr As String = String.Empty
        Dim Estr As String = String.Empty
        Dim strArray() As String

        Try



            If Type = "ADD" Then

                Wstr = "1=2"
                If Not objHelp.getProjectDtlCDMSConcoMedication(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                               ds_Medication, Estr) Then
                    Throw New Exception(Estr)
                End If

                strArray = Me.hdnSelectedConco.Value.Split(",")
                For Each grdRow As GridViewRow In grdAddConcoMedi.Rows
                    If Array.IndexOf(strArray, grdRow.Cells(GVCAddMedication_IdCode).Text) >= 0 Then
                        'If CType(grdRow.Cells(GVCAddMedication_Select).FindControl("chkCodeList"), CheckBox).Checked Then
                        dr_Medication = ds_Medication.Tables(0).NewRow()
                        dr_Medication("vWorkSpaceId") = Me.ViewState(VS_ProjectID)
                        dr_Medication("vIdCode") = grdRow.Cells(GVCAddMedication_IdCode).Text.Trim()
                        dr_Medication("vClass") = grdRow.Cells(GVCAddMedication_Class).Text.Trim()
                        dr_Medication("vDescription") = grdRow.Cells(GVCAddMedication_Description).Text.Trim()
                        dr_Medication("vCriteria") = Me.hdnrblvalue.Value
                        dr_Medication("iModifyBy") = Me.Session(S_UserID)
                        dr_Medication("cStatusIndi") = "N"
                        ds_Medication.Tables(0).Rows.Add(dr_Medication)
                        ds_Medication.Tables(0).AcceptChanges()
                    End If
                Next
                ds_Save = ds_Medication
            ElseIf Type = "EDIT" Then
                dt_MedicationEdit = CType(Me.ViewState(VS_DtConcoMedication), DataTable).Copy()
                dt_MedicationEdit.DefaultView.RowFilter = "nProjectDtlCDMSConcoMedicationNo = " + Me.hdnConcoMedicationNo.Value
                dt_MedicationEditn = dt_MedicationEdit.DefaultView.ToTable()
                dt_MedicationEditn.Columns.Add("vRemarks", System.Type.GetType("System.String"))
                dr_Medication = dt_MedicationEditn.Rows(0)
                dr_Medication("dStartDate") = IIf(String.IsNullOrEmpty(Me.txtStartDateConco.Text.Trim()), System.DBNull.Value, Me.txtStartDateConco.Text.Trim())
                dr_Medication("dEndDate") = IIf(String.IsNullOrEmpty(Me.txtEndDateConco.Text.Trim()), System.DBNull.Value, Me.txtEndDateConco.Text.Trim())
                dr_Medication("vDosage") = IIf(String.IsNullOrEmpty(Me.txtDosageConco.Text.Trim()), System.DBNull.Value, Me.txtDosageConco.Text.Trim())
                dr_Medication("cConfirmed") = Me.ddlConfirmedConco.SelectedItem.Value.Trim()
                dr_Medication("vComments") = Me.txtCommentsConco.Text.Trim()
                dr_Medication("iModifyBy") = Me.Session(S_UserID)
                dr_Medication("vRemarks") = Me.txtRemarksConco.Text.Trim()
                dr_Medication("cStatusIndi") = "E"
                ds_Save.Tables.Add(dt_MedicationEditn)
            End If



            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignValuesConco()")
            Return False
        End Try

    End Function

    Private Function AssignValuesMedicalCond(ByVal Type As String, ByRef ds_Save As DataSet) As Boolean

        Dim ds_MedicalCondi As DataSet = Nothing
        Dim dt_MedicalCondiEdit As DataTable = Nothing
        Dim dt_MedicalCondiEditn As DataTable = Nothing
        Dim dr_MedicalCondi As DataRow
        Dim Wstr As String = String.Empty
        Dim Estr As String = String.Empty
        Dim strArray() As String

        Try

            If Type = "ADD" Then

                Wstr = "1=2"

                If Not objHelp.getProjectDtlCDMSMedicalCondition(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                   ds_MedicalCondi, Estr) Then
                    Throw New Exception(Estr)
                End If

                strArray = Me.hdnSelectedCondition.Value.Split(",")
                For Each grdRow As GridViewRow In grdAddMedicalCond.Rows
                    If Array.IndexOf(strArray, grdRow.Cells(GVCAddMedCond_IdCode).Text) >= 0 Then
                        'If CType(grdRow.Cells(GVCAddMedCond_Select).FindControl("chkCodeList"), CheckBox).Checked Then
                        dr_MedicalCondi = ds_MedicalCondi.Tables(0).NewRow()
                        dr_MedicalCondi("vWorkSpaceId") = Me.ViewState(VS_ProjectID).ToString
                        dr_MedicalCondi("vIdCode") = grdRow.Cells(GVCAddMedCond_IdCode).Text.Trim()
                        dr_MedicalCondi("vType") = grdRow.Cells(GVCAddMedCond_Type).Text.Trim()
                        dr_MedicalCondi("vSubType") = grdRow.Cells(GVCAddMedCond_SubType).Text.Trim()
                        dr_MedicalCondi("vDescription") = grdRow.Cells(GVCAddMedCond_Description).Text.Trim()
                        dr_MedicalCondi("vSymptom") = grdRow.Cells(GVCAddMedCond_Symptom).Text.Trim()
                        dr_MedicalCondi("vCriteria") = Me.hdnrblvalue.Value.ToString
                        dr_MedicalCondi("iModifyBy") = Me.Session(S_UserID)
                        dr_MedicalCondi("cStatusIndi") = "N"
                        ds_MedicalCondi.Tables(0).Rows.Add(dr_MedicalCondi)
                        ds_MedicalCondi.Tables(0).AcceptChanges()
                    End If
                Next
                ds_Save = ds_MedicalCondi
            ElseIf Type = "EDIT" Then
                dt_MedicalCondiEditn = CType(Me.ViewState(VS_DtMedCondition), DataTable).Copy
                dt_MedicalCondiEditn.DefaultView.RowFilter = "nProjectDtlCSMSMedicalConditionNo = " + Me.hdnMedicalConditionNo.Value
                dt_MedicalCondiEdit = dt_MedicalCondiEditn.DefaultView.ToTable()
                dt_MedicalCondiEdit.Columns.Add("vRemarks", System.Type.GetType("System.String"))
                dr_MedicalCondi = dt_MedicalCondiEdit.Rows(0)
                dr_MedicalCondi("dOnsetDate") = IIf(String.IsNullOrEmpty(Me.txtOnsetDate.Text.Trim()), System.DBNull.Value, Me.txtOnsetDate.Text.Trim())
                dr_MedicalCondi("dResolutionDate") = IIf(String.IsNullOrEmpty(Me.txtResolutionDate.Text.Trim()), System.DBNull.Value, Me.txtResolutionDate.Text.Trim())
                dr_MedicalCondi("vSource") = Me.ddlSource.SelectedItem.Value.Trim()
                dr_MedicalCondi("cConfirmed") = Me.ddlConfirmed.SelectedItem.Value.Trim()
                dr_MedicalCondi("vComments") = Me.txtComments.Text.Trim()
                dr_MedicalCondi("iModifyBy") = Me.Session(S_UserID)
                dr_MedicalCondi("cStatusIndi") = "E"
                dr_MedicalCondi("vRemarks") = Me.txtRemarks.Text.Trim()
                ds_Save.Tables.Add(dt_MedicalCondiEdit)
            End If

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignValuesMedicalCond()")
            Return False
        End Try

    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim Projectid As String = String.Empty
        Me.ViewState(VS_ProjectID) = Me.HProjectId.Value
        If Me.hdnActivetab.Value = "Medical Condition" Then
            If Not FillGridMedicalconditions() Then
                Exit Sub
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Medical", "javascript:OpenMedicalTab();", True)
        ElseIf Me.hdnActivetab.Value = "Conco. Medication" Then
            If Not FillGridConcoMedicalCondtions() Then
                Exit Sub
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Conco", "javascript:OpenConcoTab();", True)
        End If
    End Sub

    Protected Sub btnSaveAddConco_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAddConco.Click
        Dim ds_Medication As New DataSet
        Dim eStr As String = String.Empty


        Try

            If Not AssignValuesConco("ADD", ds_Medication) Then
                Me.objCommon.ShowAlert("Error while Assinging Data", Me.Page)
                Exit Sub
            End If

            If Not objLambda.Save_ProjectDtlCDMSConcoMedication(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Medication, _
                                                                 Me.Session(S_UserID), eStr) Then
                Me.ShowErrorMessage(eStr, "Error while saving SubjectDtlCDMSConcoMedication.")
                Exit Sub
            End If

            If Not FillGridConcoMedicalCondtions() Then
                Me.ShowErrorMessage("Error While Filling Conco Medication Grid", "")
                Exit Try
            End If
            Me.ResetPageConco()
            objCommon.ShowAlert("Conco Medication Criteria Added Sucessfully", Me.Page)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Conco", "javascript:OpenConcoTab();", True)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnAdd")
        End Try
    End Sub

    Protected Sub btnSaveAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAdd.Click
        Dim ds_MediCond As New DataSet
        Dim eStr As String = String.Empty


        Try


            If Not AssignValuesMedicalCond("ADD", ds_MediCond) Then
                Me.objCommon.ShowAlert("Error while Assinging Data", Me.Page)
                Exit Sub
            End If

            If Not objLambda.Save_ProjectDtlCDMSMedicalCondition(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_MediCond, _
                                                                 Me.Session(S_UserID), eStr) Then
                Me.ShowErrorMessage(eStr, "Error while saving ProjectDtlCDMSMedicalCondition.")
                Exit Sub
            End If

            If Not FillGridMedicalconditions() Then
                Me.ShowErrorMessage("Error While Filling Mediccal Grid", "")
                Exit Sub
            End If
            ResetPageMedical()
            objCommon.ShowAlert("Medical Condition Criteria Added Sucessfully", Me.Page)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Conco", "javascript:OpenMedicalTab();", True)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnAdd")
        End Try
    End Sub

    Protected Sub btnRemarksUpdateConco_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemarksUpdateConco.Click

        Dim ds_Medication As New DataSet
        Dim eStr As String = String.Empty

        Try

            If Not AssignValuesConco("EDIT", ds_Medication) Then
                Me.objCommon.ShowAlert("Error while Assinging Data", Me.Page)
                Exit Sub
            End If



            If Not objLambda.Save_ProjectDtlCDMSConcoMedication(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_Medication, _
                                                                 Me.Session(S_UserID), eStr) Then
                Me.ShowErrorMessage(eStr, "Error while saving SubjectDtlCDMSConcoMedication.")
                Exit Sub
            End If

            If Not FillGridConcoMedicalCondtions() Then
                Me.ShowErrorMessage("Error While Filling Conco Medication Grid", "")
                Exit Sub
            End If

            objCommon.ShowAlert("Conco Medication Criteria Updated Sucessfully", Me.Page)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Conco", "javascript:OpenConcoTab();", True)
            ' Me.ResetPage()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnUpdate")
        End Try

    End Sub

    Protected Sub btnConcoFillGrid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConcoFillGrid.Click


        If Not FillGridConcoMedicalCondtions() Then
            Me.ShowErrorMessage("Error While Filling Conco Medication Grid", "")
            Exit Sub
        End If

        objCommon.ShowAlert("Conco Medication Criteria Deleted Sucessfully", Me.Page)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Conco", "javascript:OpenConcoTab();", True)

    End Sub

    Protected Sub btnRemarksUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemarksUpdate.Click
        Dim ds_MediCond As New DataSet
        Dim eStr As String = String.Empty

        Try

            If Not AssignValuesMedicalCond("EDIT", ds_MediCond) Then
                Me.objCommon.ShowAlert("Error while Assinging Data", Me.Page)
                Exit Sub
            End If



            If Not objLambda.Save_ProjectDtlCDMSMedicalCondition(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_MediCond, _
                                                                Me.Session(S_UserID), eStr) Then
                Me.ShowErrorMessage(eStr, "Error while saving ProjectDtlCDMSMedicalCondition.")
                Exit Sub
            End If

            If Not FillGridMedicalconditions() Then
                Me.ShowErrorMessage("Error While Filling Mediccal Grid", "")
                Exit Sub
            End If
            ResetPageMedical()
            objCommon.ShowAlert("Medical Condition Criteria Added Sucessfully", Me.Page)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Conco", "javascript:OpenMedicalTab();", True)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnUpdate")
        End Try

    End Sub

    Protected Sub btnMedicalFillGrid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMedicalFillGrid.Click
        If Not FillGridMedicalconditions() Then
            Me.ShowErrorMessage("Error While Filling Mediccal Grid", "")
            Exit Sub
        End If
        ResetPageMedical()
    End Sub

    Protected Sub btnHistoryCondition_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHistoryCondition.Click

        Dim ds_Audit As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            wStr = "vWorkSpaceId = '" + Me.ViewState(VS_ProjectID).ToString() + "' And vCriteria = '" + Me.rblMedi.SelectedValue.ToString() + "' Order by cStatusIndi Desc"

            If Not objHelp.getProjectDtlCDMSMedicalCondition(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                               ds_Audit, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_Audit Is Nothing Then
                If ds_Audit.Tables(0).Rows.Count > 0 Then
                    Me.grdConditionAudit.DataSource = ds_Audit
                    Me.grdConditionAudit.DataBind()
                    Me.lblCondiAudit.Text = "Audit Trail - Medical Condition (" + Me.rblMedi.SelectedValue.ToString() + ")"
                Else
                    Me.lblCondiAudit.Text = "No Audit Trail For Medical Condition"
                    Me.grdConditionAudit.DataSource = Nothing
                    Me.grdConditionAudit.DataBind()
                End If
            End If
            Me.mdlConditionAudit.Show()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "AuditMedical", "javascript:OpenMedicalTab();", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillAuditGrid")
        End Try
    End Sub

    Protected Sub btnHistoryMedication_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHistoryMedication.Click

        Dim ds_Audit As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            wStr = "vWorkSpaceId = '" + Me.ViewState(VS_ProjectID).ToString() + "' And vCriteria = '" + Me.rblconco.SelectedValue.ToString() + "' Order by cStatusIndi Desc"

            If Not objHelp.getProjectDtlCDMSConcoMedication(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                               ds_Audit, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_Audit Is Nothing Then
                If ds_Audit.Tables(0).Rows.Count > 0 Then
                    Me.grdMedicationAudit.DataSource = ds_Audit
                    Me.grdMedicationAudit.DataBind()
                    Me.lblMediAudit.Text = "Audit Trail - Conco. Medication (" + Me.rblconco.SelectedValue.ToString() + ")"
                Else
                    Me.lblMediAudit.Text = "No Audit Trail For Conco. Medication"
                    Me.grdMedicationAudit.DataSource = Nothing
                    Me.grdMedicationAudit.DataBind()
                End If
            End If
            Me.mdlMedicationAudit.Show()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ConcoAudit", "javascript:OpenConcoTab();", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillAuditGrid")
        End Try

    End Sub

    Protected Sub btnMedicalPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMedicalPrevious.Click
        Me.Response.Redirect("frmCDMSStudyInformation.aspx?Mode=2&WorkspaceId=" + Me.HProjectId.Value)
    End Sub

    Protected Sub btnConcoNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConcoNext.Click
        Me.Response.Redirect("frmCDMSStudyInformation.aspx?Mode=1")
    End Sub

    Protected Sub btnMedicalNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMedicalNext.Click
        Me.Response.Redirect("frmCDMSProjectMedicalCondition.aspx?Mode=1&WorkspaceId=" + Me.HProjectId.Value + "&tab=Medication")
    End Sub

    Protected Sub btnConcoPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConcoPrevious.Click
        Me.Response.Redirect("frmCDMSProjectMedicalCondition.aspx?Mode=1&WorkspaceId=" + Me.HProjectId.Value + "&tab=Medical")
    End Sub

#End Region

#Region "Grid Events"

    Protected Sub grdConditionAudit_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdConditionAudit.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("imgCondAudit"), ImageButton).CommandName = "Audit"
            CType(e.Row.FindControl("imgCondAudit"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.Cells(GVCCondiAudit_Status).FindControl("lblStatus"), Label).Text = "Active"
            If CType(e.Row.Cells(GVCCondiAudit_Status).FindControl("hdnStatus"), HiddenField).Value = "D" Then
                CType(e.Row.Cells(GVCCondiAudit_Status).FindControl("lblStatus"), Label).Text = "Inactive"
            End If
            If CType(e.Row.Cells(GVCCondiAudit_Audit).FindControl("hdnStatus"), HiddenField).Value = "E" Then
                CType(e.Row.Cells(GVCCondiAudit_Audit).FindControl("imgCondAudit"), ImageButton).ImageUrl = "~/CDMS/images/AuditMark_Small.png"
                CType(e.Row.Cells(GVCCondiAudit_Audit).FindControl("lblConditionAuditMark"), Label).Visible = True
            End If
        End If

    End Sub

    Protected Sub grdConditionAudit_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdConditionAudit.RowCommand

        Dim ds_Audit As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try
            If e.CommandName = "Audit" Then

                wStr = "vWorkSpaceId = '" + Me.ViewState(VS_ProjectID).ToString() + _
                        "' And vCriteria = '" + Me.rblMedi.SelectedValue.ToString() + "' And vIdCode = '" + _
                        CType(grdConditionAudit.Rows(e.CommandArgument).Cells(GVCCondiAudit_Status).FindControl("hdnIdCode"), HiddenField).Value + "'"

                If Not objHelp.View_AuditProjectDtlCDMSMedicalCondition(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                   ds_Audit, eStr) Then
                    Throw New Exception(eStr)
                End If

                If Not ds_Audit Is Nothing Then
                    If ds_Audit.Tables(0).Rows.Count > 0 Then
                        ds_Audit.Tables(0).Columns.Add("dModifyOffSet", Type.GetType("System.String"))
                        For Each dr_Audit In ds_Audit.Tables(0).Rows
                            dr_Audit("dModifyOffSet") = Convert.ToString(CDate(dr_Audit("dModifyOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                        Next
                        Me.grdConditionRowAudit.DataSource = ds_Audit
                        Me.grdConditionRowAudit.DataBind()
                        Me.lblCondiRowAudit.Text = "Record Audit Trail - Medical Condition (" + Me.rblMedi.SelectedValue.ToString() + ") (" + grdConditionAudit.Rows(e.CommandArgument).Cells(GVCMediAudit_Type).Text + " - " + grdConditionAudit.Rows(e.CommandArgument).Cells(GVCMediAudit_Description).Text + ")"
                        Me.mdlConditionRowAudit.Show()
                    Else
                        Me.grdConditionRowAudit.DataSource = Nothing
                        Me.grdConditionRowAudit.DataBind()
                    End If
                End If
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "AuditRowMedical", "javascript:OpenMedicalTab();", True)
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...grdAudit_RowCommand()")
        End Try

    End Sub

    Protected Sub grdMedicationAudit_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdMedicationAudit.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("imgMediAudit"), ImageButton).CommandName = "Audit"
            CType(e.Row.FindControl("imgMediAudit"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.Cells(GVCMediAudit_Status).FindControl("lblStatus"), Label).Text = "Active"
            If CType(e.Row.Cells(GVCMediAudit_Status).FindControl("hdnStatus"), HiddenField).Value = "D" Then
                CType(e.Row.Cells(GVCMediAudit_Status).FindControl("lblStatus"), Label).Text = "Inactive"
            End If
            If CType(e.Row.Cells(GVCMediAudit_Audit).FindControl("hdnStatus"), HiddenField).Value = "E" Then
                CType(e.Row.Cells(GVCMediAudit_Audit).FindControl("imgMediAudit"), ImageButton).ImageUrl = "~/CDMS/images/AuditMark_Small.png"
                CType(e.Row.Cells(GVCMediAudit_Audit).FindControl("lblMedicationAuditMark"), Label).Visible = True
            End If
        End If

    End Sub

    Protected Sub grdMedicationAudit_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdMedicationAudit.RowCommand

        Dim ds_Audit As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try
            If e.CommandName = "Audit" Then

                wStr = "vWorkSpaceId = '" + Me.ViewState(VS_ProjectID).ToString() + _
                        "' And vCriteria = '" + Me.rblconco.SelectedValue.ToString() + "' And vIdCode = '" + _
                        CType(grdMedicationAudit.Rows(e.CommandArgument).Cells(GVCMediAudit_Status).FindControl("hdnIdCode"), HiddenField).Value + "'"

                If Not objHelp.View_AuditProjectDtlCDMSConcoMedication(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                   ds_Audit, eStr) Then
                    Throw New Exception(eStr)
                End If

                If Not ds_Audit Is Nothing Then
                    If ds_Audit.Tables(0).Rows.Count > 0 Then
                        ds_Audit.Tables(0).Columns.Add("dModifyOffSet", Type.GetType("System.String"))
                        For Each dr_Audit In ds_Audit.Tables(0).Rows
                            dr_Audit("dModifyOffSet") = Convert.ToString(CDate(dr_Audit("dModifyOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                        Next
                        Me.grdMedicationRowAudit.DataSource = ds_Audit
                        Me.grdMedicationRowAudit.DataBind()
                        Me.lblMediRowAudit.Text = "Record Audit Trail - Conco. Medication (" + Me.rblconco.SelectedValue.ToString() + ") (" + grdMedicationAudit.Rows(e.CommandArgument).Cells(GVCCondiAudit_Code).Text + ")"
                        Me.mdlMedicationRowAudit.Show()
                    Else
                        Me.grdMedicationRowAudit.DataSource = Nothing
                        Me.grdMedicationRowAudit.DataBind()
                    End If
                End If
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ConcoRowAudit", "javascript:OpenConcoTab();", True)
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...grdAudit_RowCommand()")
        End Try

    End Sub

    Protected Sub grdMedicalCondition_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdMedicalCondition.RowCreated
        If Me.Session(S_WorkFlowStageId) <> 0 Then
            e.Row.Cells(GVCgrdMedicalCondition_Action).Style.Add("display", "none")
        End If
    End Sub

    Protected Sub grdConcoMedi_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdConcoMedi.RowCreated
        If Me.Session(S_WorkFlowStageId) <> 0 Then
            e.Row.Cells(GVCConcoMedi_Action).Style.Add("display", "none")
        End If
    End Sub

#End Region

#Region "Reset Function"

    Private Sub ResetPageMedical()
        Me.txtType.Text = ""
        Me.txtSubType.Text = ""
        Me.txtDescrption.Text = ""
        Me.txtOnsetDate.Text = ""
        Me.txtResolutionDate.Text = ""
        Me.ddlSource.SelectedIndex = -1
        Me.txtComments.Text = ""
        Me.txtRemarks.Text = ""
        Me.ddlConfirmed.SelectedIndex = -1
        Me.hdnSelectedCondition.Value = ""
    End Sub

    Private Sub ResetPageConco()
        Me.txtCodeConco.Text = ""
        Me.txtClassConco.Text = ""
        Me.txtCommentsConco.Text = ""
        Me.txtStartDateConco.Text = ""
        Me.txtRemarksConco.Text = ""
        Me.txtEndDateConco.Text = ""
        Me.ddlConfirmedConco.SelectedIndex = -1
        Me.txtDosageConco.Text = ""
        Me.hdnSelectedConco.Value = ""
    End Sub

#End Region

#Region "Web Methods"

    <Web.Services.WebMethod()> _
   Public Shared Function DeleteConcoMedication(ByVal vWorkspaceID As String, ByVal ConcoMedicationNo As String, ByVal Remarks As String) As String
        Dim objCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

        Dim ds_Medication As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            wStr = " vWorkspaceID = '" + vWorkspaceID + "' And nProjectDtlCDMSConcoMedicationNo = '" + ConcoMedicationNo + "' And cStatusIndi <> 'D'"
            If Not objhelpDb.getProjectDtlCDMSConcoMedication(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                              ds_Medication, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_Medication Is Nothing Then
                If ds_Medication.Tables(0).Rows.Count > 0 Then
                    ds_Medication.Tables(0).Columns.Add("vRemarks", System.Type.GetType("System.String"))
                    ds_Medication.Tables(0).Rows(0)("cStatusIndi") = "D"
                    ds_Medication.Tables(0).Rows(0)("vRemarks") = Remarks
                End If
            End If

            If Not objLambda.Save_ProjectDtlCDMSConcoMedication(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                                                                 ds_Medication, HttpContext.Current.Session(S_UserID), eStr) Then
                Throw New Exception(eStr)
            End If

            Return "Success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function EditConcoMedication(ByVal vWorkspaceID As String, ByVal ConcoMedicationNo As String) As String
        Dim objCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

        Dim ds_Medication As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            wStr = " vWorkspaceID = '" + vWorkspaceID + "' And nProjectDtlCDMSConcoMedicationNo = '" + ConcoMedicationNo + "' And cStatusIndi <> 'D'"
            If Not objhelpDb.getProjectDtlCDMSConcoMedication(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                              ds_Medication, eStr) Then
                Throw New Exception(eStr)
            End If


            If Not ds_Medication Is Nothing Then
                If ds_Medication.Tables(0).Rows.Count > 0 Then
                    Return JsonConvert.SerializeObject(ds_Medication)
                End If
            End If
        Catch ex As Exception
            'Return ex.Message
        End Try
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function DeleteMedicalConditionMedical(ByVal vWokspaceId As String, ByVal MedicalConditionNo As String, ByVal Remarks As String) As String
        Dim objCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

        Dim ds_MedicalCond As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            wStr = " vWorkspaceId = '" + vWokspaceId + "' And nProjectDtlCSMSMedicalConditionNo = '" + MedicalConditionNo + "' And cStatusIndi <> 'D'"
            If Not objhelpDb.getProjectDtlCDMSMedicalCondition(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                              ds_MedicalCond, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_MedicalCond Is Nothing Then
                If ds_MedicalCond.Tables(0).Rows.Count > 0 Then
                    ds_MedicalCond.Tables(0).Columns.Add("vRemarks", System.Type.GetType("System.String"))
                    ds_MedicalCond.Tables(0).Rows(0)("cStatusIndi") = "D"
                    ds_MedicalCond.Tables(0).Rows(0)("vRemarks") = Remarks
                End If
            End If

            If Not objLambda.Save_ProjectDtlCDMSMedicalCondition(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                                                                 ds_MedicalCond, HttpContext.Current.Session(S_UserID), eStr) Then
                Throw New Exception(eStr)
            End If

            Return "Success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function EditMedicalConditionMeDical(ByVal vWokspaceId As String, ByVal MedicalConditionNo As String) As String
        Dim objCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

        Dim ds_MedicalCond As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            wStr = " vWorkspaceId = '" + vWokspaceId + "' And nProjectDtlCSMSMedicalConditionNo = '" + MedicalConditionNo + "' And cStatusIndi <> 'D'"
            If Not objhelpDb.getProjectDtlCDMSMedicalCondition(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                              ds_MedicalCond, eStr) Then
                Throw New Exception(eStr)
            End If


            If Not ds_MedicalCond Is Nothing Then
                If ds_MedicalCond.Tables(0).Rows.Count > 0 Then
                    Return JsonConvert.SerializeObject(ds_MedicalCond)
                End If
            End If
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

#End Region

#Region "RadioButton Events"

    Protected Sub rblconco_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblconco.SelectedIndexChanged
        If Me.HProjectId.Value <> "" Then
            If Not FillGridConcoMedicalCondtions() Then
                Exit Sub
            End If
        Else
            objCommon.ShowAlert("Project Not Found,Please Enter Project", Me.Page)
            Me.rblconco.SelectedValue = Nothing
        End If
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Conco", "javascript:OpenConcoTab();", True)
    End Sub

    Protected Sub rblMedi_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblMedi.SelectedIndexChanged
        If Me.HProjectId.Value <> "" Then
            Me.ViewState(VS_ProjectID) = Me.HProjectId.Value
            If Not FillGridMedicalconditions() Then
                Exit Sub
            End If
        Else
            objCommon.ShowAlert("Project Not Found,Please Enter Project", Me.Page)
            Me.rblMedi.SelectedValue = Nothing
        End If
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Conco", "javascript:OpenMedicalTab();", True)
    End Sub

#End Region


End Class
