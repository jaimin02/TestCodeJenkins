Imports Newtonsoft.Json

Partial Class CDMS_frmCDMSMedicalCondition
    Inherits System.Web.UI.Page

#Region "Variable Declaration "

    Private objCommon As New clsCommon
    Private objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()


    Public Const VS_Choice As String = "Choice"
    Private Const VS_DtMedCondi As String = "DtMedCondi"
    Public Const VS_SubjectID As String = "SubjectID"

    Private Const GVCAddMedCond_Select As Integer = 0
    Private Const GVCAddMedCond_IdCode As Integer = 1
    Private Const GVCAddMedCond_Type As Integer = 2
    Private Const GVCAddMedCond_SubType As Integer = 3
    Private Const GVCAddMedCond_Description As Integer = 4
    Private Const GVCAddMedCond_Symptom As Integer = 5

    Private Const GVCAudit_Type As Integer = 0
    Private Const GVCAudit_Description As Integer = 2
    Private Const GVCAudit_Status As Integer = 4
    Private Const GVCAudit_Audit As Integer = 5

    Private Const GVCMedicalCondition_Type As Integer = 0
    Private Const GVCMedicalCondition_SubType As Integer = 1
    Private Const GVCMedicalCondition_Description As Integer = 2
    Private Const GVCMedicalCondition_Symptom As Integer = 3
    Private Const GVCMedicalCondition_OnsetDate As Integer = 4
    Private Const GVCMedicalCondition_ResolutionDate As Integer = 5
    Private Const GVCMedicalCondition_Comments As Integer = 6
    Private Const GVCMedicalCondition_Action As Integer = 7

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
        Dim dt_MedCondi As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try

           
            Choice = Me.Request.QueryString("Mode").ToString
            Me.ViewState(VS_Choice) = Choice

            If Not Me.Request.QueryString("SubjectID") Is Nothing Then
                Me.ViewState(VS_SubjectID) = Me.Request.QueryString("SubjectID").ToString
            End If

            If Not GenCall_Data(Choice, dt_MedCondi) Then
                Exit Function
            End If

            Me.ViewState(VS_DtMedCondi) = dt_MedCondi

            If Not GenCall_ShowUI(Choice, dt_MedCondi) Then
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
        Dim ds_MediCondi As DataSet = Nothing

        Try


            If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vSubjectID = '" + Me.ViewState(VS_SubjectID).ToString() + "' And cStatusIndi <> 'D'"
            End If

            wStr += " And cStatusIndi <> 'D'"

            If Not objhelpDb.getSubjectDtlCDMSMedicalCondition(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                               ds_MediCondi, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_MediCondi Is Nothing Then
                Throw New Exception(eStr)
            End If

            dt_Dist_Retu = ds_MediCondi.Tables(0)
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

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Subject Information - Medical Condition"
            Me.Page.Title = " :: CDMS - Medical Condition ::" + System.Configuration.ConfigurationManager.AppSettings("Client")

            If Not Me.ViewState(VS_SubjectID) Is Nothing Then

                Me.lblSubjectMedicalCond.Text = Me.ViewState(VS_SubjectID).ToString

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

                txtOnsetDate.Attributes.Add("OnChange", "DateConvertForScreening(this.value,this)")
                txtResolutionDate.Attributes.Add("OnChange", "DateConvertForScreening(this.value,this)")

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

        Dim ds_MediCond As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            wStr = "vSubjectID = '" + Me.ViewState(VS_SubjectID).ToString() + "' And cStatusIndi <> 'D' order by vIdCode"

            If Not objhelpDb.getSubjectDtlCDMSMedicalCondition(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                               ds_MediCond, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_MediCond Is Nothing Then
                If ds_MediCond.Tables(0).Rows.Count > 0 Then
                    Me.grdMedicalCondition.DataSource = ds_MediCond
                    Me.grdMedicalCondition.DataBind()
                    Me.ViewState(VS_DtMedCondi) = ds_MediCond.Tables(0)
                    ds_MediCond.Tables(0).DefaultView.RowFilter = "cStatusIndi = 'E'"
                    Me.btnHistory.Text = "History (" + ds_MediCond.Tables(0).DefaultView.ToTable().Rows.Count.ToString + ")"
                Else
                    Me.grdMedicalCondition.DataSource = Nothing
                    Me.grdMedicalCondition.DataBind()
                    Me.ViewState(VS_DtMedCondi) = ds_MediCond.Tables(0)
                End If
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StrfnApplyDataTableGrid", "fnApplyDataTable();", True)
            ResetPage()
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillGrid")
            Return False
        End Try
    End Function

    Private Function FillListGrid() As Boolean
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_MediCondiList As New DataSet

        Try


            wStr = " cStatusIndi <> 'D' Order by CodeMedicalConditions "

            If Not objhelpDb.getMedicalConditionsList(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                      ds_MediCondiList, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_MediCondiList Is Nothing Then
                If ds_MediCondiList.Tables(0).Rows.Count > 0 Then
                    Me.grdAddMedicalCond.DataSource = ds_MediCondiList
                    Me.grdAddMedicalCond.DataBind()
                End If
            End If

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StrfnApplyDataTableList", "fnApplyDataTable();", True)

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....FillGridList")
            Return False
        End Try
    End Function

#End Region

#Region "Assign Values"

    Private Function AssignValues(ByVal Type As String) As Boolean

        Dim dt_MedicalCondi As New DataTable
        Dim dt_MedicalCondiEdit As New DataTable
        Dim dr_MedicalCondi As DataRow
        Dim strArray() As String

        Try


            dt_MedicalCondi = Me.ViewState(VS_DtMedCondi)

            If Type = "ADD" Then


                dt_MedicalCondi.Clear()
                If Me.hdnSelectedCondition.Value.Trim = "" Then
                    objCommon.ShowAlert("Please Select Atleast One Medicalcondition To Add", Me.Page)
                    Me.mpMedicalCond.Show()
                    Exit Function
                Else
                    strArray = Me.hdnSelectedCondition.Value.Split(",")
                End If

                For Each grdRow As GridViewRow In grdAddMedicalCond.Rows
                    If Array.IndexOf(strArray, grdRow.Cells(GVCAddMedCond_IdCode).Text) >= 0 Then
                        'If CType(grdRow.Cells(GVCAddMedCond_Select).FindControl("chkCodeList"), CheckBox).Checked Then
                        dr_MedicalCondi = dt_MedicalCondi.NewRow()
                        dr_MedicalCondi("vSubjectId") = Me.ViewState(VS_SubjectID).ToString
                        dr_MedicalCondi("vIdCode") = grdRow.Cells(GVCAddMedCond_IdCode).Text.Trim()
                        dr_MedicalCondi("vType") = grdRow.Cells(GVCAddMedCond_Type).Text.Trim()
                        dr_MedicalCondi("vSubType") = grdRow.Cells(GVCAddMedCond_SubType).Text.Trim()
                        dr_MedicalCondi("vDescription") = grdRow.Cells(GVCAddMedCond_Description).Text.Trim()
                        dr_MedicalCondi("vSymptom") = grdRow.Cells(GVCAddMedCond_Symptom).Text.Trim()
                        dr_MedicalCondi("iModifyBy") = Me.Session(S_UserID)
                        dr_MedicalCondi("cStatusIndi") = "N"
                        dt_MedicalCondi.Rows.Add(dr_MedicalCondi)
                    End If
                Next

            ElseIf Type = "EDIT" Then
                dt_MedicalCondi.DefaultView.RowFilter = "nSubjectDtlCSMSMedicalConditionNo = " + Me.hdnMedicalConditionNo.Value
                dt_MedicalCondiEdit = dt_MedicalCondi.DefaultView.ToTable()
                dt_MedicalCondiEdit.Columns.Add("vRemarks", System.Type.GetType("System.String"))
                dr_MedicalCondi = dt_MedicalCondiEdit.Rows(0)
                dr_MedicalCondi("dOnsetDate") = IIf(String.IsNullOrEmpty(Me.txtOnsetDate.Text.Trim()), System.DBNull.Value, Me.txtOnsetDate.Text.Trim())
                dr_MedicalCondi("dResolutionDate") = IIf(String.IsNullOrEmpty(Me.txtResolutionDate.Text.Trim()), System.DBNull.Value, Me.txtResolutionDate.Text.Trim())
                dr_MedicalCondi("vSource") = Me.ddlSource.SelectedItem.Value.Trim()
                dr_MedicalCondi("cConfirmed") = Me.ddlConfirmed.SelectedItem.Value.Trim()
                dr_MedicalCondi("vComments") = Me.txtComments.Text.Trim()
                dr_MedicalCondi("iModifyBy") = Me.Session(S_UserID)
                dr_MedicalCondi("vRemarks") = Me.txtRemarks.Text.Trim()
                dr_MedicalCondi("cStatusIndi") = "E"
                dt_MedicalCondi.Clear()
                dt_MedicalCondi = dt_MedicalCondiEdit.Copy()
            End If

            Me.ViewState(VS_DtMedCondi) = dt_MedicalCondi

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignValues()")
            Return False
        End Try

    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSaveAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim ds_MediCond As New DataSet
        Dim eStr As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try

            Choice = Me.ViewState(VS_Choice)

            If Not AssignValues("ADD") Then
                Me.objCommon.ShowAlert("Error while Assinging Data", Me.Page)
                Exit Sub
            End If

            ds_MediCond.Tables.Add(CType(Me.ViewState(VS_DtMedCondi), DataTable).Copy())

            If Not objLambda.Save_SubjectDtlCDMSMedicalCondition(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_MediCond, _
                                                                 Me.Session(S_UserID), eStr) Then
                Me.ShowErrorMessage(eStr, "Error while saving SubjectDtlCDMSMedicalCondition.")
                Exit Sub
            End If
            FillGrid()
            Me.ResetPage()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "strfnApplyDataTable", "fnApplyDataTable();", True)


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnAdd")
        End Try
    End Sub

    Protected Sub btnRemarksUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemarksUpdate.Click

        Dim ds_MediCond As New DataSet
        Dim eStr As String = String.Empty

        Try

            If Not AssignValues("EDIT") Then
                Me.objCommon.ShowAlert("Error while Assinging Data", Me.Page)
                Exit Sub
            End If

            ds_MediCond.Tables.Add(CType(Me.ViewState(VS_DtMedCondi), DataTable).Copy())

            If Not objLambda.Save_SubjectDtlCDMSMedicalCondition(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_MediCond, _
                                                                 Me.Session(S_UserID), eStr) Then
                Me.ShowErrorMessage(eStr, "Error while saving SubjectDtlCDMSMedicalCondition.")
                Exit Sub
            End If

            FillGrid()
            Me.ResetPage()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnUpdate")
        End Try


    End Sub

    Protected Sub btnFillGrid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFillGrid.Click
        Me.FillGrid()
        Me.ResetPage()
    End Sub

    Protected Sub btnHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHistory.Click
        Dim ds_Audit As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            wStr = "vSubjectID = '" + Me.ViewState(VS_SubjectID).ToString() + "' Order by vIdCode"

            If Not objhelpDb.getSubjectDtlCDMSMedicalCondition(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
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
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StrfnApplyDataTableAudit", "fnApplyDataTable();", True)

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

                If Not objhelpDb.View_AuditSubjectDtlCDMSMedicalCondition(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
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
                        Me.lblRowAudit.Text = "Record Audit Trail - Medical Condition (" + grdAudit.Rows(e.CommandArgument).Cells(GVCAudit_Type).Text + " - " + grdAudit.Rows(e.CommandArgument).Cells(GVCAudit_Description).Text + ")"
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

    Protected Sub grdMedicalCondition_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdMedicalCondition.RowCreated
        If Me.Session(S_WorkFlowStageId) <> 0 Then
            e.Row.Cells(GVCMedicalCondition_Action).Style.Add("display", "none")
        End If
    End Sub
       
#End Region

#Region "Helper Function"

    Private Sub ResetPage()
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
        For Each grdRow As GridViewRow In grdAddMedicalCond.Rows
            CType(grdRow.Cells(GVCAddMedCond_Select).FindControl("chkCodeList"), CheckBox).Checked = False
        Next
    End Sub

#End Region

#Region "Web Methods"

    <Web.Services.WebMethod()> _
    Public Shared Function DeleteMedicalCondition(ByVal SubjectId As String, ByVal MedicalConditionNo As String, ByVal Remarks As String) As String

        Dim objCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

        Dim ds_MedicalCond As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            wStr = " vSubjectId = '" + SubjectId + "' And nSubjectDtlCSMSMedicalConditionNo = '" + MedicalConditionNo + "' And cStatusIndi <> 'D'"
            If Not objhelpDb.getSubjectDtlCDMSMedicalCondition(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
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

            If Not objLambda.Save_SubjectDtlCDMSMedicalCondition(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                                                                 ds_MedicalCond, HttpContext.Current.Session(S_UserID), eStr) Then
                Throw New Exception(eStr)
            End If

            Return "Success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function EditMedicalCondition(ByVal SubjectId As String, ByVal MedicalConditionNo As String) As String
        Dim objCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

        Dim ds_MedicalCond As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            wStr = " vSubjectId = '" + SubjectId + "' And nSubjectDtlCSMSMedicalConditionNo = '" + MedicalConditionNo + "' And cStatusIndi <> 'D'"
            If Not objhelpDb.getSubjectDtlCDMSMedicalCondition(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
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
