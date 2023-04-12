Imports Newtonsoft.Json

Partial Class CDMS_frmCDMSConcoMedication
    Inherits System.Web.UI.Page


#Region "Variable Declaration "

    Private objCommon As New clsCommon
    Private objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()


    Public Const VS_Choice As String = "Choice"
    Private Const VS_DtMedication As String = "DtMedication"
    Public Const VS_SubjectID As String = "SubjectID"

    Private Const GVCAddMedication_Select As Integer = 0
    Private Const GVCAddMedication_IdCode As Integer = 1
    Private Const GVCAddMedication_Class As Integer = 2
    Private Const GVCAddMedication_Description As Integer = 3

    Private Const GVCAudit_Code As Integer = 0
    Private Const GVCAudit_Status As Integer = 3
    Private Const GVCAudit_Audit As Integer = 4

    Private Const GVCConcoMedi_Code As Integer = 0
    Private Const GVCConcoMedi_Class As Integer = 1
    Private Const GVCConcoMedi_Description As Integer = 2
    Private Const GVCConcoMedi_Dosage As Integer = 3
    Private Const GVCConcoMedi_StartDate As Integer = 4
    Private Const GVCConcoMedi_EndDate As Integer = 5
    Private Const GVCConcoMedi_Action As Integer = 6

#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Not IsPostBack Then
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
        Dim dt_Medication As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try



            Choice = Me.Request.QueryString("Mode").ToString
            Me.ViewState(VS_Choice) = Choice

            If Not Me.Request.QueryString("SubjectID") Is Nothing Then
                Me.ViewState(VS_SubjectID) = Me.Request.QueryString("SubjectID").ToString
            End If

            If Not GenCall_Data(Choice, dt_Medication) Then
                Exit Function
            End If

            Me.ViewState(VS_DtMedication) = dt_Medication

            If Not GenCall_ShowUI(Choice, dt_Medication) Then
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
        Dim ds_Medication As DataSet = Nothing

        Try


            If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vSubjectID = '" + Me.ViewState(VS_SubjectID).ToString() + "' And cStatusIndi <> 'D'"
            End If

            wStr += " And cStatusIndi <> 'D'"

            If Not objhelpDb.getSubjectDtlCDMSConcoMedication(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                               ds_Medication, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_Medication Is Nothing Then
                Throw New Exception(eStr)
            End If

            dt_Dist_Retu = ds_Medication.Tables(0)
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
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Subject Information - Conco. Medication"
            Me.Page.Title = " :: CDMS - Conco. Medication ::" + System.Configuration.ConfigurationManager.AppSettings("Client")

            If Not Me.ViewState(VS_SubjectID) Is Nothing Then
                Me.lblSubjectConcoMedi.Text = Me.ViewState(VS_SubjectID).ToString

                If Not FillGrid() Then
                    Return False
                End If

                If Not FillListGrid() Then
                    Return False
                End If

                If Me.Session(S_WorkFlowStageId) <> 0 Then
                    Me.btnAddMore.Visible = False
                    Me.btnHistory.Visible = False
                End If
                txtStartDate.Attributes.Add("OnChange", "DateConvertForScreening(this.value,this)")
                txtEndDate.Attributes.Add("OnChange", "DateConvertForScreening(this.value,this)")

            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_ShowUI")
        Finally
        End Try

    End Function

#End Region

#Region "FillGrid"

    Public Function FillGrid() As Boolean

        Dim ds_Medication As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            wStr = "vSubjectID = '" + Me.ViewState(VS_SubjectID).ToString() + "' And cStatusIndi <> 'D'"

            If Not objhelpDb.getSubjectDtlCDMSConcoMedication(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                               ds_Medication, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_Medication Is Nothing Then
                If ds_Medication.Tables(0).Rows.Count > 0 Then
                    Me.grdConcoMedi.DataSource = ds_Medication
                    Me.grdConcoMedi.DataBind()
                    Me.ViewState(VS_DtMedication) = ds_Medication.Tables(0)
                    ds_Medication.Tables(0).DefaultView.RowFilter = "cStatusIndi = 'E'"
                    Me.btnHistory.Text = "History (" + ds_Medication.Tables(0).DefaultView.ToTable().Rows.Count.ToString + ")"
                Else
                    Me.grdConcoMedi.DataSource = Nothing
                    Me.grdConcoMedi.DataBind()
                    Me.ViewState(VS_DtMedication) = ds_Medication.Tables(0)
                End If
            End If

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StrfnApplyDataTableGrid", "fnApplyDataTable();", True)

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillGrid")
            Return False
        End Try
    End Function

    Private Function FillListGrid() As Boolean
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_MedicationList As New DataSet

        Try


            wStr = " cStatusIndi <> 'D' Order by CodeClassOfMedication "

            If Not objhelpDb.getCodeConcoMedicationList(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
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

#End Region

#Region "Assing Values"

    Private Function AssignValues(ByVal Type As String) As Boolean

        Dim dt_Medication As New DataTable
        Dim dt_MedicationEdit As New DataTable
        Dim dr_Medication As DataRow
        Dim strArray() As String


        Try

            dt_Medication = Me.ViewState(VS_DtMedication)

            If Type = "ADD" Then


                dt_Medication.Clear()
                If Me.hdnSelectedConco.Value.Trim = "" Then
                    objCommon.ShowAlert("Please select atleast one concomedication to add", Me.Page)
                    Me.mpConcoMedi.Show()
                    Exit Function
                Else
                    strArray = Me.hdnSelectedConco.Value.Split(",")
                End If


                For Each grdRow As GridViewRow In grdAddConcoMedi.Rows
                    If Array.IndexOf(strArray, grdRow.Cells(GVCAddMedication_IdCode).Text) >= 0 Then
                        'If CType(grdRow.Cells(GVCAddMedication_Select).FindControl("chkCodeList"), CheckBox).Checked Then
                        dr_Medication = dt_Medication.NewRow()
                        dr_Medication("vSubjectId") = Me.ViewState(VS_SubjectID).ToString
                        dr_Medication("vIdCode") = grdRow.Cells(GVCAddMedication_IdCode).Text.Trim()
                        dr_Medication("vClass") = grdRow.Cells(GVCAddMedication_Class).Text.Trim()
                        dr_Medication("vDescription") = grdRow.Cells(GVCAddMedication_Description).Text.Trim()
                        dr_Medication("iModifyBy") = Me.Session(S_UserID)
                        dr_Medication("cStatusIndi") = "N"
                        dt_Medication.Rows.Add(dr_Medication)
                    End If
                Next

            ElseIf Type = "EDIT" Then
                dt_Medication.DefaultView.RowFilter = "nSubjectDtlCDMSConcoMedicationNo = " + Me.hdnConcoMedicationNo.Value
                dt_MedicationEdit = dt_Medication.DefaultView.ToTable()
                dt_MedicationEdit.Columns.Add("vRemarks", System.Type.GetType("System.String"))
                dr_Medication = dt_MedicationEdit.Rows(0)
                dr_Medication("dStartDate") = IIf(String.IsNullOrEmpty(Me.txtStartDate.Text.Trim()), System.DBNull.Value, Me.txtStartDate.Text.Trim())
                dr_Medication("dEndDate") = IIf(String.IsNullOrEmpty(Me.txtEndDate.Text.Trim()), System.DBNull.Value, Me.txtEndDate.Text.Trim())
                dr_Medication("vDosage") = IIf(String.IsNullOrEmpty(Me.txtDosage.Text.Trim()), System.DBNull.Value, Me.txtDosage.Text.Trim())
                dr_Medication("cConfirmed") = Me.ddlConfirmed.SelectedItem.Value.Trim()
                dr_Medication("vComments") = Me.txtComments.Text.Trim()
                dr_Medication("iModifyBy") = Me.Session(S_UserID)
                dr_Medication("vRemarks") = Me.txtRemarks.Text.Trim()
                dr_Medication("cStatusIndi") = "E"
                dt_Medication.Clear()
                dt_Medication = dt_MedicationEdit.Copy()
            End If

            Me.ViewState(VS_DtMedication) = dt_Medication

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignValues()")
            Return False
        End Try

    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSaveAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAdd.Click

        Dim ds_Medication As New DataSet
        Dim eStr As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try

            Choice = Me.ViewState(VS_Choice)

            If Not AssignValues("ADD") Then
                Me.objCommon.ShowAlert("Error while Assinging Data", Me.Page)
                Exit Sub
            End If

            ds_Medication.Tables.Add(CType(Me.ViewState(VS_DtMedication), DataTable).Copy())

            If Not objLambda.Save_SubjectDtlCDMSConcoMedication(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Medication, _
                                                                 Me.Session(S_UserID), eStr) Then
                Me.ShowErrorMessage(eStr, "Error while saving SubjectDtlCDMSConcoMedication.")
                Exit Sub
            End If

            FillGrid()
            Me.ResetPage()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "strfnApplyDataTable", "fnApplyDataTable();", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnAdd")
        End Try

    End Sub

    Protected Sub btnFillGrid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFillGrid.Click
        Me.FillGrid()
        Me.ResetPage()
    End Sub

    Protected Sub btnRemarksUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemarksUpdate.Click

        Dim ds_Medication As New DataSet
        Dim eStr As String = String.Empty

        Try

            If Not AssignValues("EDIT") Then
                Me.objCommon.ShowAlert("Error while Assinging Data", Me.Page)
                Exit Sub
            End If

            ds_Medication.Tables.Add(CType(Me.ViewState(VS_DtMedication), DataTable).Copy())

            If Not objLambda.Save_SubjectDtlCDMSConcoMedication(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_Medication, _
                                                                 Me.Session(S_UserID), eStr) Then
                Me.ShowErrorMessage(eStr, "Error while saving SubjectDtlCDMSConcoMedication.")
                Exit Sub
            End If

            FillGrid()
            Me.ResetPage()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnUpdate")
        End Try
    End Sub

    Protected Sub btnHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHistory.Click
        Dim ds_Audit As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            wStr = "vSubjectID = '" + Me.ViewState(VS_SubjectID).ToString() + "' Order by cStatusIndi Desc"

            If Not objhelpDb.getSubjectDtlCDMSConcoMedication(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                               ds_Audit, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_Audit Is Nothing Then
                If ds_Audit.Tables(0).Rows.Count > 0 Then
                    Me.grdAudit.DataSource = ds_Audit
                    Me.grdAudit.DataBind()
                    Me.mdlAudit.Show()
                Else
                    Me.grdAudit.DataSource = Nothing
                    Me.grdAudit.DataBind()
                End If
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "strfnApplyDataTable", "fnApplyDataTable();", True)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillGrid")
        End Try

    End Sub

    Protected Sub btnSaveCancel_Click(sender As Object, e As EventArgs)
        ResetPage()
    End Sub


#End Region

#Region "Grid Events"

    Protected Sub grdAudit_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAudit.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("imgAudit"), ImageButton).CommandName = "Audit"
            CType(e.Row.FindControl("imgAudit"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.Cells(GVCAudit_Status).FindControl("lblStatus"), Label).Text = "Active"
            If CType(e.Row.Cells(GVCAudit_Status).FindControl("hdnStatus"), HiddenField).Value = "D" Then
                CType(e.Row.Cells(GVCAudit_Status).FindControl("lblStatus"), Label).Text = "Inactive"
            End If
            If CType(e.Row.Cells(GVCAudit_Status).FindControl("hdnStatus"), HiddenField).Value = "E" Then
                CType(e.Row.Cells(GVCAudit_Audit).FindControl("imgAudit"), ImageButton).ImageUrl = "~/CDMS/images/AuditMark_Small.png"
                CType(e.Row.Cells(GVCAudit_Audit).FindControl("lblAuditMark"), Label).Visible = True
            End If
        End If

    End Sub

    Protected Sub grdAudit_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdAudit.RowCommand

        Dim ds_Audit As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try
            If e.CommandName = "Audit" Then

                wStr = "vSubjectID = '" + Me.ViewState(VS_SubjectID).ToString() + "' And vIdCode = '" + _
                        CType(grdAudit.Rows(e.CommandArgument).Cells(GVCAudit_Status).FindControl("hdnIdCode"), HiddenField).Value + "'"

                If Not objhelpDb.View_AuditSubjectDtlCDMSConcoMedication(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
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
                        Me.lblRowAudit.Text = "Record Audit Trail - Conco. Medication (" + grdAudit.Rows(e.CommandArgument).Cells(GVCAudit_Code).Text + ")"
                        Me.mdlAudit.Show()
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

    Protected Sub grdConcoMedi_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdConcoMedi.RowCreated
        If Me.Session(S_WorkFlowStageId) <> 0 Then
            e.Row.Cells(GVCConcoMedi_Action).Style.Add("display", "none")
        End If
    End Sub
#End Region

#Region "Helper Function"

    Private Sub ResetPage()
        Me.txtCode.Text = ""
        Me.txtClass.Text = ""
        Me.txtComments.Text = ""
        Me.txtStartDate.Text = ""
        Me.txtRemarks.Text = ""
        Me.txtEndDate.Text = ""
        Me.ddlConfirmed.SelectedIndex = -1
        Me.txtDosage.Text = ""
        Me.hdnSelectedConco.Value = ""
        For Each grdRow As GridViewRow In grdAddConcoMedi.Rows
            CType(grdRow.Cells(GVCAddMedication_Select).FindControl("chkCodeList"), CheckBox).Checked = False
        Next
    End Sub

#End Region

#Region "Web Methods"

    <Web.Services.WebMethod()> _
   Public Shared Function DeleteMedication(ByVal SubjectId As String, ByVal ConcoMedicationNo As String, ByVal Remarks As String) As String
        Dim objCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

        Dim ds_Medication As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            wStr = " vSubjectId = '" + SubjectId + "' And nSubjectDtlCDMSConcoMedicationNo = '" + ConcoMedicationNo + "' And cStatusIndi <> 'D'"
            If Not objhelpDb.getSubjectDtlCDMSConcoMedication(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
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

            If Not objLambda.Save_SubjectDtlCDMSConcoMedication(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                                                                 ds_Medication, HttpContext.Current.Session(S_UserID), eStr) Then
                Throw New Exception(eStr)
            End If

            Return "Success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function EditMedication(ByVal SubjectId As String, ByVal ConcoMedicationNo As String) As String
        Dim objCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

        Dim ds_Medication As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            wStr = " vSubjectId = '" + SubjectId + "' And nSubjectDtlCDMSConcoMedicationNo = '" + ConcoMedicationNo + "' And cStatusIndi <> 'D'"
            If Not objhelpDb.getSubjectDtlCDMSConcoMedication(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                              ds_Medication, eStr) Then
                Throw New Exception(eStr)
            End If


            If Not ds_Medication Is Nothing Then
                If ds_Medication.Tables(0).Rows.Count > 0 Then
                    Return JsonConvert.SerializeObject(ds_Medication)
                End If
            End If
        Catch ex As Exception
            Return ex.Message
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

