Imports Newtonsoft.Json
Partial Class frmCDMSStudyHistory
    Inherits System.Web.UI.Page

#Region "Variable Declaration "

    Private objCommon As New clsCommon
    Private objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()


    Public Const VS_Choice As String = "Choice"
    Private Const VS_DtStudyHistory As String = "DtStudyHistory"
    Public Const VS_SubjectID As String = "SubjectID"

    'Private Const GVCAddMedication_Select As Integer = 0
    'Private Const GVCAddMedication_IdCode As Integer = 1
    'Private Const GVCAddMedication_Class As Integer = 2
    'Private Const GVCAddMedication_Description As Integer = 3

    Private Const GVCAudit_vProjectNo As Integer = 0
    Private Const GVCAudit_vDrugName As Integer = 1
    Private Const GVCAudit_StartDate As Integer = 2
    Private Const GVCAudit_EndDate As Integer = 3
    Private Const GVCAudit_vComments As Integer = 4
    Private Const GVCAudit_Status As Integer = 5
    Private Const GVCAudit_Audit As Integer = 6

    Private Const GVCStudy_SrNO As Integer = 0
    Private Const GVCStudy_vProjectNo As Integer = 1
    Private Const GVCStudy_vDrugName As Integer = 2
    Private Const GVCStudy_StartDate As Integer = 3
    Private Const GVCStudy_EndDate As Integer = 4
    Private Const GVCStudy_vComments As Integer = 5
    Private Const GVCStudy_Action As Integer = 6

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
        Dim dt_StudyHistory As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try



            Choice = Me.Request.QueryString("Mode").ToString
            Me.ViewState(VS_Choice) = Choice

            If Not Me.Request.QueryString("SubjectID") Is Nothing Then
                Me.ViewState(VS_SubjectID) = Me.Request.QueryString("SubjectID").ToString
            End If

            If Not GenCall_Data(Choice, dt_StudyHistory) Then
                Exit Function
            End If

            Me.ViewState(VS_DtStudyHistory) = dt_StudyHistory

            If Not GenCall_ShowUI(Choice, dt_StudyHistory) Then
                Exit Function
            End If

            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)

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
        Dim ds_StudyHistory As DataSet = Nothing

        Try


            If Me.Request.QueryString("SubjectID") = "" Then
                wStr = "1=2"
            Else
                wStr = "vSubjectID = '" + Me.ViewState(VS_SubjectID).ToString() + "' And cStatusIndi <> 'D'"
            End If

            If Not objhelpDb.getSubjectDtlCDMSStudyHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                               ds_StudyHistory, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_StudyHistory Is Nothing Then
                Throw New Exception(eStr)
            End If

            dt_Dist_Retu = ds_StudyHistory.Tables(0)
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
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Subject Information - Study History"
            Me.Page.Title = " :: CDMS - Study History ::" + System.Configuration.ConfigurationManager.AppSettings("Client")

            Me.lblSubject.Text = Me.ViewState(VS_SubjectID).ToString

            If Not FillGrid() Then
                Return False
            End If

            If Me.Session(S_WorkFlowStageId) <> 0 Then
                Me.btnAddMore.Visible = False
                Me.btnHistory.Visible = False
            End If
            txtStartDate.Attributes.Add("OnChange", "DateConvertForScreening(this.value,this)")
            txtEndDate.Attributes.Add("OnChange", "DateConvertForScreening(this.value,this)")

            txtEditstartdate.Attributes.Add("OnChange", "DateConvertForScreening(this.value,this)")
            txtEditenddate.Attributes.Add("OnChange", "DateConvertForScreening(this.value,this)")

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_ShowUI")
        Finally
        End Try

    End Function

#End Region

#Region "FillGrid"

    Public Function FillGrid() As Boolean

        Dim ds_StudyHistory As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            wStr = "vSubjectID = '" + Me.ViewState(VS_SubjectID).ToString() + "' And cStatusIndi <> 'D'"

            If Not objhelpDb.getView_SubjectDtlCDMSStudyHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                               ds_StudyHistory, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_StudyHistory Is Nothing Then
                If ds_StudyHistory.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To ds_StudyHistory.Tables(0).Rows.Count - 1
                        If ds_StudyHistory.Tables(0).Rows(i)("dStudyStartDate").ToString() <> "" Then
                            If IsDate(ds_StudyHistory.Tables(0).Rows(i)("dStudyStartDate").ToString()) Then
                                ds_StudyHistory.Tables(0).Rows(i)("dStudyStartDate") = DateTime.Parse(ds_StudyHistory.Tables(0).Rows(i)("dStudyStartDate")).ToString("dd-MMM-yyyy")
                            Else
                                ds_StudyHistory.Tables(0).Rows(i)("dStudyStartDate") = ds_StudyHistory.Tables(0).Rows(i)("dStudyStartDate").ToString()
                            End If
                        End If
                        If ds_StudyHistory.Tables(0).Rows(i)("dStudyEndDate").ToString() <> "" Then
                            If IsDate(ds_StudyHistory.Tables(0).Rows(i)("dStudyEndDate").ToString()) Then
                                ds_StudyHistory.Tables(0).Rows(i)("dStudyEndDate") = DateTime.Parse(ds_StudyHistory.Tables(0).Rows(i)("dStudyEndDate")).ToString("dd-MMM-yyyy")
                            Else
                                ds_StudyHistory.Tables(0).Rows(i)("dStudyEndDate") = ds_StudyHistory.Tables(0).Rows(i)("dStudyEndDate").ToString()
                            End If
                        End If
                        ds_StudyHistory.Tables(0).AcceptChanges()
                    Next
                    Me.grdStudyHistory.DataSource = ds_StudyHistory
                    Me.grdStudyHistory.DataBind()
                    Me.ViewState(VS_DtStudyHistory) = ds_StudyHistory.Tables(0)
                    ds_StudyHistory.Tables(0).DefaultView.RowFilter = "cStatusIndi = 'E'"
                    Me.btnHistory.Text = "History (" + ds_StudyHistory.Tables(0).DefaultView.ToTable().Rows.Count.ToString + ")"
                Else
                    Me.grdStudyHistory.DataSource = Nothing
                    Me.grdStudyHistory.DataBind()
                    Me.ViewState(VS_DtStudyHistory) = ds_StudyHistory.Tables(0)
                End If
            End If

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StrfnApplyDataTableGrid", "fnApplyDataTable();", True)

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillGrid")
            Return False
        End Try
    End Function

    'Private Function FillListGrid() As Boolean
    '    Dim wStr As String = String.Empty
    '    Dim eStr As String = String.Empty
    '    Dim ds_StudyHistoryList As New DataSet

    '    Try
    '        
    '        wStr = " cStatusIndi <> 'D' Order by CodeClassOfMedication "

    '        If Not objhelpDb.getCodeConcoMedicationList(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                                  ds_StudyHistoryList, eStr) Then
    '            Throw New Exception(eStr)
    '        End If

    '        If Not ds_StudyHistoryList Is Nothing Then
    '            If ds_StudyHistoryList.Tables(0).Rows.Count > 0 Then
    '                'Me.grdAddConcoMedi.DataSource = ds_StudyHistoryList
    '                'Me.grdAddConcoMedi.DataBind()
    '            End If
    '        End If

    '        Return True
    '    Catch ex As Exception
    '        ShowErrorMessage(ex.Message, "....FillGridList")
    '        Return False
    '    End Try
    'End Function

#End Region

#Region "Assing Values"

    Private Function AssignValues(ByVal Type As String) As Boolean

        Dim dt_StudyHistory As New DataTable
        Dim dt_StudyHistoryEdit As New DataTable
        Dim dr_StudyHistory As DataRow
        Dim eStr As String = String.Empty
        Dim ds_BlankStudyHistory As New DataSet
        Try

            dt_StudyHistory = Me.ViewState(VS_DtStudyHistory)

            If Not objhelpDb.getSubjectDtlCDMSStudyHistory("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                               ds_BlankStudyHistory, eStr) Then
                Throw New Exception(eStr)
            End If

            If Type = "ADD" Then

                dt_StudyHistory.Clear()
                dr_StudyHistory = ds_BlankStudyHistory.Tables(0).NewRow()
                dr_StudyHistory("vSubjectId") = Me.ViewState(VS_SubjectID).ToString
                dr_StudyHistory("vWorkspaceId") = Me.HProjectId.Value.Trim()
                dr_StudyHistory("vDrugName") = IIf(String.IsNullOrEmpty(Me.hdnDrugName.Value.Trim()), System.DBNull.Value, Me.hdnDrugName.Value.Trim())
                dr_StudyHistory("dStudyStartDate") = IIf(String.IsNullOrEmpty(Me.txtStartDate.Text.Trim()), System.DBNull.Value, Me.txtStartDate.Text.Trim())
                dr_StudyHistory("dStudyEndDate") = IIf(String.IsNullOrEmpty(Me.txtEndDate.Text.Trim()), System.DBNull.Value, Me.txtEndDate.Text.Trim())
                dr_StudyHistory("vComments") = IIf(String.IsNullOrEmpty(Me.txtComments.Text.Trim()), System.DBNull.Value, Me.txtComments.Text.Trim())
                dr_StudyHistory("iModifyBy") = Me.Session(S_UserID)
                dr_StudyHistory("cStatusIndi") = "N"
                ds_BlankStudyHistory.Tables(0).Rows.Add(dr_StudyHistory)
                ds_BlankStudyHistory.Tables(0).AcceptChanges()

            ElseIf Type = "EDIT" Then

                dt_StudyHistory.DefaultView.RowFilter = "nSubjectDtlCDMSStudyHistoryNo =" + Me.hdnStudyHistoryNo.Value()
                dt_StudyHistoryEdit = dt_StudyHistory.DefaultView.ToTable()
                ds_BlankStudyHistory.Tables(0).Columns.Add("vRemarks", System.Type.GetType("System.String"))
                dr_StudyHistory = ds_BlankStudyHistory.Tables(0).NewRow()
                dr_StudyHistory("nSubjectDtlCDMSStudyHistoryNo") = IIf(String.IsNullOrEmpty(dt_StudyHistoryEdit.Rows(0).Item("nSubjectDtlCDMSStudyHistoryNo")), System.DBNull.Value, dt_StudyHistoryEdit.Rows(0).Item("nSubjectDtlCDMSStudyHistoryNo"))
                dr_StudyHistory("vSubjectId") = Me.ViewState(VS_SubjectID).ToString
                dr_StudyHistory("vWorkspaceId") = Me.HProjectId.Value.Trim()
                dr_StudyHistory("vDrugName") = IIf(String.IsNullOrEmpty(dt_StudyHistoryEdit.Rows(0).Item("vDrugName")), System.DBNull.Value, dt_StudyHistoryEdit.Rows(0).Item("vDrugName"))
                dr_StudyHistory("dStudyStartDate") = IIf(String.IsNullOrEmpty(Me.txtEditstartdate.Text.Trim()), System.DBNull.Value, Me.txtEditstartdate.Text.Trim())
                dr_StudyHistory("dStudyEndDate") = IIf(String.IsNullOrEmpty(Me.txtEditenddate.Text.Trim()), System.DBNull.Value, Me.txtEditenddate.Text.Trim())
                dr_StudyHistory("vComments") = IIf(String.IsNullOrEmpty(Me.txtEditcomments.Text.Trim()), System.DBNull.Value, Me.txtEditcomments.Text.Trim())
                dr_StudyHistory("iModifyBy") = Me.Session(S_UserID)
                dr_StudyHistory("cStatusIndi") = "E"
                dr_StudyHistory("vRemarks") = Me.txtRemarks.Text.Trim()
                ds_BlankStudyHistory.Tables(0).Rows.Add(dr_StudyHistory)
                ds_BlankStudyHistory.Tables(0).AcceptChanges()

            End If

            Me.ViewState(VS_DtStudyHistory) = ds_BlankStudyHistory.Tables(0)

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignValues()")
            Return False
        End Try

    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSaveAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAdd.Click

        Dim ds_StudyHistory As New DataSet
        Dim eStr As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try

            Choice = Me.ViewState(VS_Choice)

            If Not AssignValues("ADD") Then
                Me.objCommon.ShowAlert("Error while Assinging Data", Me.Page)
                Exit Sub
            End If

            ds_StudyHistory.Tables.Add(CType(Me.ViewState(VS_DtStudyHistory), DataTable).Copy())

            If Not objLambda.Save_SubjectDtlCDMSStudyHistory(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_StudyHistory, _
                                                                 Me.Session(S_UserID), eStr) Then
                Me.ShowErrorMessage(eStr, "Error while saving SubjectDtlCDMSStudyHistory.")
                Exit Sub
            End If

            FillGrid()
            Me.ResetPage()
            objCommon.ShowAlert("Study Details Saved For This Subject", Me.Page)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "strfnApplyDataTable", "fnApplyDataTable();", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnSaveAdd")
        End Try

    End Sub

    Protected Sub btnFillGrid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFillGrid.Click
        Me.FillGrid()
        ' Me.ResetPage()
    End Sub

    Protected Sub btnRemarksUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemarksUpdate.Click

        Dim ds_StudyHistory As New DataSet
        Dim eStr As String = String.Empty

        Try

            If Not AssignValues("EDIT") Then
                Me.objCommon.ShowAlert("Error while Assinging Data", Me.Page)
                Exit Sub
            End If

            ds_StudyHistory.Tables.Add(CType(Me.ViewState(VS_DtStudyHistory), DataTable).Copy())

            If Not objLambda.Save_SubjectDtlCDMSStudyHistory(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_StudyHistory, _
                                                                 Me.Session(S_UserID), eStr) Then
                Me.ShowErrorMessage(eStr, "Error while saving SubjectDtlCDMSStudyHistory.")
                Exit Sub
            End If

            FillGrid()
            'Me.ResetPage()

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

            If Not objhelpDb.getView_SubjectDtlCDMSStudyHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                               ds_Audit, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_Audit Is Nothing Then
                If ds_Audit.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To ds_Audit.Tables(0).Rows.Count - 1
                        If ds_Audit.Tables(0).Rows(i)("dStudyStartDate").ToString() <> "" Then
                            If IsDate(ds_Audit.Tables(0).Rows(i)("dStudyStartDate").ToString()) Then
                                ds_Audit.Tables(0).Rows(i)("dStudyStartDate") = DateTime.Parse(ds_Audit.Tables(0).Rows(i)("dStudyStartDate")).ToString("dd-MMM-yyyy")
                            Else
                                ds_Audit.Tables(0).Rows(i)("dStudyStartDate") = ds_Audit.Tables(0).Rows(i)("dStudyStartDate").ToString()
                            End If
                        End If
                        If ds_Audit.Tables(0).Rows(i)("dStudyEndDate").ToString() <> "" Then
                            If IsDate(ds_Audit.Tables(0).Rows(i)("dStudyEndDate").ToString()) Then
                                ds_Audit.Tables(0).Rows(i)("dStudyEndDate") = DateTime.Parse(ds_Audit.Tables(0).Rows(i)("dStudyEndDate")).ToString("dd-MMM-yyyy")
                            Else
                                ds_Audit.Tables(0).Rows(i)("dStudyEndDate") = ds_Audit.Tables(0).Rows(i)("dStudyEndDate").ToString()
                            End If
                        End If
                        ds_Audit.Tables(0).AcceptChanges()
                    Next
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

                wStr = "vSubjectID = '" + Me.ViewState(VS_SubjectID).ToString() + "' And vWorkspaceID = '" + _
                        CType(grdAudit.Rows(e.CommandArgument).Cells(GVCAudit_Status).FindControl("hdnIdCode"), HiddenField).Value + "'"

                If Not objhelpDb.getView_AuditSubjectDtlCDMSStudyHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                   ds_Audit, eStr) Then
                    Throw New Exception(eStr)
                End If

                If Not ds_Audit Is Nothing Then
                    If ds_Audit.Tables(0).Rows.Count > 0 Then
                        ds_Audit.Tables(0).Columns.Add("dModifyOffSet", Type.GetType("System.String"))
                        For Each dr_Audit In ds_Audit.Tables(0).Rows
                            dr_Audit("dModifyOffSet") = Convert.ToString(CDate(dr_Audit("dModifyOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                            If dr_Audit("dStudyStartDate").ToString() <> "" Then
                                If IsDate(dr_Audit("dStudyStartDate").ToString()) Then
                                    dr_Audit("dStudyStartDate") = DateTime.Parse(dr_Audit("dStudyStartDate")).ToString("dd-MMM-yyyy")
                                Else
                                    dr_Audit("dStudyStartDate") = dr_Audit("dStudyStartDate").ToString()
                                End If
                            End If
                            If dr_Audit("dStudyEndDate").ToString() <> "" Then
                                If IsDate(dr_Audit("dStudyEndDate").ToString()) Then
                                    dr_Audit("dStudyEndDate") = DateTime.Parse(dr_Audit("dStudyEndDate")).ToString("dd-MMM-yyyy")
                                Else
                                    dr_Audit("dStudyEndDate") = dr_Audit("dStudyEndDate").ToString()
                                End If
                            End If
                        Next
                        Me.grdRowAudit.DataSource = ds_Audit
                        Me.grdRowAudit.DataBind()
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StrfnApplyDataTableAudit", "fnApplyDataTable();", True)
                        Me.lblRowAudit.Text = "Record Audit Trail - Study History (" + grdAudit.Rows(e.CommandArgument).Cells(GVCAudit_vProjectNo).Text + ")"
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

    Protected Sub grdStudyHistory_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdStudyHistory.RowCreated
        If Me.Session(S_WorkFlowStageId) <> 0 Then
            e.Row.Cells(GVCStudy_Action).Style.Add("display", "none")
        End If
    End Sub

    Protected Sub grdStudyHistory_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdStudyHistory.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVCStudy_SrNO).Text = e.Row.RowIndex + (grdStudyHistory.PageSize * grdStudyHistory.PageIndex) + 1
        End If

    End Sub
#End Region

#Region "Helper Function"

    Private Sub ResetPage()
        Me.HProjectId.Value = ""
        Me.txtproject.Text = ""
        Me.txtComments.Text = ""
        Me.txtStartDate.Text = ""
        Me.txtRemarks.Text = ""
        Me.txtEndDate.Text = ""
        Me.txtDrug.Text = ""
    End Sub

#End Region

#Region "Web Methods"

    <Web.Services.WebMethod()> _
   Public Shared Function DeleteStudyHistory(ByVal SubjectId As String, ByVal StudyHistoryNo As String, ByVal Remarks As String) As String
        Dim objCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

        Dim ds_StudyHistory As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            wStr = " vSubjectId = '" + SubjectId + "' And nSubjectDtlCDMSStudyHistoryNo = '" + StudyHistoryNo + "' And cStatusIndi <> 'D'"
            If Not objhelpDb.getSubjectDtlCDMSStudyHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                              ds_StudyHistory, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_StudyHistory Is Nothing Then
                If ds_StudyHistory.Tables(0).Rows.Count > 0 Then
                    ds_StudyHistory.Tables(0).Columns.Add("vRemarks", System.Type.GetType("System.String"))
                    ds_StudyHistory.Tables(0).Rows(0)("cStatusIndi") = "D"
                    ds_StudyHistory.Tables(0).Rows(0)("vRemarks") = Remarks
                End If
            End If

            If Not objLambda.Save_SubjectDtlCDMSStudyHistory(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                                                                 ds_StudyHistory, HttpContext.Current.Session(S_UserID), eStr) Then
                Throw New Exception(eStr)
            End If

            Return "Success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function EditStudyHistory(ByVal SubjectId As String, ByVal StudyHistoryNo As String) As String
        Dim objCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

        Dim ds_StudyHistory As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            wStr = " vSubjectId = '" + SubjectId + "' And nSubjectDtlCDMSStudyHistoryNo = '" + StudyHistoryNo + "' And cStatusIndi <> 'D'"
            If Not objhelpDb.getView_SubjectDtlCDMSStudyHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                              ds_StudyHistory, eStr) Then
                Throw New Exception(eStr)
            End If


            If Not ds_StudyHistory Is Nothing Then
                If ds_StudyHistory.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To ds_StudyHistory.Tables(0).Rows.Count - 1
                        If ds_StudyHistory.Tables(0).Rows(i)("dStudyStartDate").ToString() <> "" Then
                            If IsDate(ds_StudyHistory.Tables(0).Rows(i)("dStudyStartDate").ToString()) Then
                                ds_StudyHistory.Tables(0).Rows(i)("dStudyStartDate") = DateTime.Parse(ds_StudyHistory.Tables(0).Rows(i)("dStudyStartDate")).ToString("dd-MMM-yyyy")
                            Else
                                ds_StudyHistory.Tables(0).Rows(i)("dStudyStartDate") = ds_StudyHistory.Tables(0).Rows(i)("dStudyStartDate").ToString()
                            End If
                        End If
                        If ds_StudyHistory.Tables(0).Rows(i)("dStudyEndDate").ToString() <> "" Then
                            If IsDate(ds_StudyHistory.Tables(0).Rows(i)("dStudyEndDate").ToString()) Then
                                ds_StudyHistory.Tables(0).Rows(i)("dStudyEndDate") = DateTime.Parse(ds_StudyHistory.Tables(0).Rows(i)("dStudyEndDate")).ToString("dd-MMM-yyyy")
                            Else
                                ds_StudyHistory.Tables(0).Rows(i)("dStudyEndDate") = ds_StudyHistory.Tables(0).Rows(i)("dStudyEndDate").ToString()
                            End If
                        End If
                        ds_StudyHistory.Tables(0).AcceptChanges()
                    Next
                    Return JsonConvert.SerializeObject(ds_StudyHistory.Tables(0))
                End If
            End If
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function ShowStudyHistory(ByVal wstr As String) As String
        Dim objCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
        Dim ds_StudyHistory As New DataSet
        Dim eStr As String = String.Empty

        Try

            ds_StudyHistory = objhelpDb.GetResultSet(wstr, "View_StudyInformationDtl")
            
            If Not ds_StudyHistory Is Nothing Then
                If ds_StudyHistory.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To ds_StudyHistory.Tables(0).Rows.Count - 1
                        If ds_StudyHistory.Tables(0).Rows(i)("dStartDate").ToString() <> "" Then
                            If IsDate(ds_StudyHistory.Tables(0).Rows(i)("dStartDate").ToString()) Then
                                ds_StudyHistory.Tables(0).Rows(i)("dStartDate") = DateTime.Parse(ds_StudyHistory.Tables(0).Rows(i)("dStartDate")).ToString("dd-MMM-yyyy")
                            Else
                                ds_StudyHistory.Tables(0).Rows(i)("dStartDate") = ds_StudyHistory.Tables(0).Rows(i)("dStartDate").ToString()
                            End If
                        End If
                        If ds_StudyHistory.Tables(0).Rows(i)("dEndDate").ToString() <> "" Then
                            If IsDate(ds_StudyHistory.Tables(0).Rows(i)("dEndDate").ToString()) Then
                                ds_StudyHistory.Tables(0).Rows(i)("dEndDate") = DateTime.Parse(ds_StudyHistory.Tables(0).Rows(i)("dEndDate")).ToString("dd-MMM-yyyy")
                            Else
                                ds_StudyHistory.Tables(0).Rows(i)("dEndDate") = ds_StudyHistory.Tables(0).Rows(i)("dEndDate").ToString()
                            End If
                        End If
                        ds_StudyHistory.Tables(0).AcceptChanges()
                    Next
                    Return JsonConvert.SerializeObject(ds_StudyHistory.Tables(0))
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
