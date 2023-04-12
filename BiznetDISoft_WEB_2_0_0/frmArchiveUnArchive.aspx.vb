Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Partial Class frmArchiveUnArchive
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION"

    Private ObjCommon As New clsCommon
    Private ObjHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private ObjLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Private VS_dsForLabDataStatus As String = "LabDataStatus"
    Private VS_dtArchiveDetail As String = "LastArchiveDetail"
    Private VS_ArchiveStartYear As String = "ArchiveStartYear"
    Private Const VS_ArchiveYear As String = "ArchiveYear"
    Private Const VS_Choice As String = "Choice"
    Private Const VS_ArchiveDetail As String = "ArchiveDetail"
    Private Const GV_WorkspaceId As Integer = 7
    Private Const GV_iArchiveStatus As Integer = 6
    Private LabReportStatus As Integer
    Private EroorCode As Integer


#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If Not GenCall_ShowUI() Then
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".......Page_Load")
        End Try

    End Sub

#End Region

#Region "GenCall_showUI()"

    Private Function GenCall_ShowUI() As Boolean
        Try
            Page.Title = " :: Archive Unacheve ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Archive/Unarchive"
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)

            GenCall_ShowUI = True


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_ShowUI")
        End Try
    End Function

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

#Region "AssignValues"

    Private Function AssignValues() As Boolean
        Dim Ds_ArchieveProjectDetail As New DataSet
        Dim estr_retu As String = String.Empty
        Dim ds_ArchiveDtl As New DataSet
        Dim dr As DataRow = Nothing
        Dim ds_ArchiveDetail As DataSet = Nothing
        Dim UserId As String = String.Empty
        Dim wstr As String = String.Empty
        Dim ds_WorkspaceDetail As DataSet = Nothing
        Dim IsSuccess As Integer
        Dim SuccessLabRpt As Integer
        Dim LabArchiveFlag As String = String.Empty
        Dim ds_LastProjectStatusDetail As DataSet = Nothing
        Dim ds_TocheckLabReportStatus As DataSet = Nothing

        Try
            UserId = Me.Session(S_UserID)


            wstr = "vWorkspaceID='" & Me.HProjectId.Value.Trim() & "' And "
            wstr += " cStatusIndi <> 'D'"
            If Not ObjHelp.view_ArchiveProjectMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_LastProjectStatusDetail, estr_retu) Then
                Return False
            End If

            '=================================to get the Empty Structure Of table=====================================================

            If Not ObjHelp.GetArchieveDetail("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_ArchiveDtl, estr_retu) Then
                Throw New Exception(estr_retu)
            End If


            wstr = "vWorkspaceID='" & Me.HProjectId.Value.Trim() & "' And "
            wstr += " cStatusIndi <> 'D' And iTranNo='1'"

            If Not ObjHelp.GetArchieveDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ArchiveDetail, estr_retu) Then
                Throw New Exception(estr_retu)
            End If

            ViewState(VS_ArchiveStartYear) = ds_ArchiveDetail

            dr = ds_ArchiveDtl.Tables(0).NewRow()
            dr("vWorkspaceID") = HProjectId.Value.Trim
            dr("vSchemaID") = Date.Now.Year.ToString
            dr("iArchiveYear") = DateTime.Now.Year
            dr("cArchiveFlag") = "N"
            If rblArchive.Items(0).Enabled = True Then
                dr("cArchiveFlag") = "Y"
            End If
            If txtYears.Text.Trim().Length = 0 Then
                dr("iArchivedForYrs") = ds_LastProjectStatusDetail.Tables(0).Rows(0)("iArchivedForYrs").ToString
            Else
                dr("iArchivedForYrs") = Me.txtYears.Text.Trim
            End If
            dr("vRemarks") = Me.txtRemark.Text.Trim
            dr("iModifyBy") = Me.Session(S_UserID)
            dr("dModifyOn") = Date.Now.ToString
            dr("cArchiveForLabData") = dr("cArchiveFlag").ToString
            dr("dLabDataModifyOn") = dr("dModifyOn").ToString
            If GV_ArchieveDetail.Columns(8).Visible = True Then
                dr("cArchiveForLabData") = DBNull.Value
                dr("dLabDataModifyOn") = DBNull.Value
            End If

            '===This code is to insert current year if project archive first time else enter year of first time archival==============
            If ds_ArchiveDetail.Tables(0).Rows.Count <> 0 Then
                dr("iArchiveYear") = ds_ArchiveDetail.Tables(0).Rows(0)("iArchiveYear").ToString

            Else
                If Not TransferArchiveDetail(IsSuccess) Then
                    'If (IsSuccess = 1 Or EroorCode = 2) Then
                    ObjCommon.ShowAlert("Project Not Perfectly Archived.Try Again", Me.Page)
                    'EroorCode = 0 '=====it will check that procedure not executed successfully then exit function========
                    Exit Function
                    'End If
                End If
                '======this is to check that user has clicked on Yes (to archive labreport) then proceed======
                GV_ArchieveDetail.Columns(8).Visible = True
                If hdn_LabArchiveFlag.Value.ToString.Trim = "Y" Then
                    dr("cArchiveForLabData") = dr("cArchiveFlag").ToString
                    dr("dLabDataModifyOn") = dr("dModifyOn").ToString
                    If proc_TransferLabRptForArchive(SuccessLabRpt) Then
                        'If (SuccessLabRpt = 1) Then
                        ObjCommon.ShowAlert("LabReport Not Perfectly Archived.Try Again", Me.Page)
                        Exit Function
                        'End If
                    End If
                    
                ElseIf hdn_LabArchiveFlag.Value.ToString.Trim = "" Then
                    dr("cArchiveForLabData") = DBNull.Value
                    dr("dLabDataModifyOn") = DBNull.Value
                    GV_ArchieveDetail.Columns(8).Visible = False
                End If

            End If

            '===============================================================================================================================



            ds_ArchiveDtl.Tables(0).Rows.Add(dr)
            ds_ArchiveDtl.AcceptChanges()


            '=========to save data into Archiveprojectmst using procedure insert_archiveprojectmst===========================================

            If Not ObjLambda.Save_ArchiveDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_ArchiveDtl, UserId, estr_retu) Then
                Throw New Exception(estr_retu)
            End If

            wstr = "vWorkspaceID='" & Me.HProjectId.Value.Trim() & "'"
            If Not ObjHelp.getWorkspaceDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_WorkspaceDetail, estr_retu) Then
                Throw New Exception(estr_retu)
            End If

            ds_WorkspaceDetail.Tables(0).Rows(0).Item(10) = "A"
            If Not ObjLambda.insert_workspace(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_WorkspaceDetail, UserId, estr_retu) Then
                Throw New Exception(estr_retu)
            End If

            If rblArchive.Items(0).Enabled Then
                ObjCommon.ShowAlert("Project Archived Successfully", Me.Page)
            Else
                ObjCommon.ShowAlert("Project UnArchived Successfully", Me.Page)
            End If
            txtRemark.Text = ""
            txtYears.Text = ""
            hdn_LabArchiveFlag.Value = ""
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".......AssignValues")
            Return False
        End Try

    End Function
#End Region

#Region "ArchiveProjectAndLabReoprt"

    '========Added By: vikas=====================================================
    '========Added For:TO Archive The Project And Lab Report Data================

    Private Function TransferArchiveDetail(ByRef IsSuccess As Integer) As Boolean
        Dim UserId As String = String.Empty
        Dim estr_retu As String = String.Empty
        Dim ds_SchemaDetail As DataSet = Nothing
        Dim ds_TransferDetail As DataSet = Nothing
        UserId = Session(S_UserID)
        Dim wstr As String = String.Empty
        Dim workspaceid As String = String.Empty
        Dim ds_WorkspaceDetail As DataSet = Nothing
        Dim dt_ProcParameter As DataTable = New DataTable("ProcParameter")
        Dim dt_Parameter As DataTable = New DataTable("Parameter")
        Try

            wstr = "name='" + CStr(Date.Now.Year) + "'"

            '========== this is to check that schema is already there for current year or not======================

            If Not ObjHelp.getSchema(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SchemaDetail, estr_retu) Then
                Throw New Exception(estr_retu)
            End If

            If ds_SchemaDetail.Tables(0).Rows.Count = 0 Then
                If Not ObjHelp.Proc_Schema(Date.Now.Year, ds_SchemaDetail, estr_retu) Then
                    Throw New Exception(estr_retu)
                End If
            End If

            '====This Code Is to Pass Parameters Of Procedure Through Datatable===========================================
            dt_ProcParameter.Columns.Add("vWorkspaceId")
            dt_ProcParameter.Columns.Add("Schema")
            dt_ProcParameter.Rows.Add()
            dt_ProcParameter.Rows(0).Item(0) = Me.HProjectId.Value.Trim()
            dt_ProcParameter.Rows(0).Item(1) = Date.Now.Year
            '===========================================================================================================

            If Not ObjLambda.Save_TransferDetail(WS_HelpDbTable.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, dt_ProcParameter, True, Session(S_UserID), IsSuccess, estr_retu) Then
                Throw New Exception
            End If
            If IsSuccess = 0 Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......TransferArchiveDetail")
            Return False
        End Try

    End Function

    Private Function proc_TransferLabRptForArchive(ByRef SuccessLabRpt As Integer) As Boolean
        Dim wstr As String = String.Empty
        Dim estr_retu As String = Nothing
        Dim ds_ArchiveDetail As DataSet = Nothing
        Dim dt_Parameter As DataTable = New DataTable("ProcParameters")
        Try
            wstr = "vWorkspaceID='" & Me.HProjectId.Value.Trim() & "' And "
            wstr += " cStatusIndi <> 'D' And iTranNo='1'"

            If Not ObjHelp.GetArchieveDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ArchiveDetail, estr_retu) Then
                Throw New Exception(estr_retu)
            End If

            dt_Parameter.Columns.Add("vWorkspaceId")
            dt_Parameter.Columns.Add("Schema")
            dt_Parameter.Rows.Add()
            dt_Parameter.Rows(0).Item(0) = Me.HProjectId.Value.Trim()
            'ds_ArchiveStartYear = ViewState(VS_ArchiveStartYear)
            dt_Parameter.Rows(0).Item(1) = Date.Now.Year
            If ds_ArchiveDetail.Tables(0).Rows.Count <> 0 Then
                dt_Parameter.Rows(0).Item(1) = ds_ArchiveDetail.Tables(0).Rows(0)("iArchiveYear").ToString
            End If
            If Not ObjLambda.proc_TransferLabRptForArchive(WS_HelpDbTable.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, dt_Parameter, True, Session(S_UserID), SuccessLabRpt, estr_retu) Then
                'EroorCode = 2 '==this is To exit from parent function if error in procedure execution=======
                Throw New Exception
            End If
            If SuccessLabRpt = 0 Then
                Return SuccessLabRpt
            Else
                Return 1
            End If
        Catch ex As Exception
            Return 1
        End Try

    End Function

#End Region

#Region "FillData"

    Private Function FillData() As Boolean
        Dim ds_ArchiveDetail As New DataSet
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim dr As DataRow = Nothing
        Dim dt_ArchiveDetail As New DataTable
        Dim ds_TocheckLabReportStatus As DataSet = Nothing
        Try

            Wstr = "vWorkspaceID='" & Me.HProjectId.Value.Trim() & "' And "
            Wstr += " cStatusIndi <> 'D'"

            '======this is to get latest status and max(itranno)======================================================
            If Not ObjHelp.view_ArchiveProjectMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_ArchiveDetail, estr) Then
                Return False
            End If
            '===========================================================================================================
            'ViewState(VS_dtArchiveDetail) = ds_ArchiveDetail



            Wstr = "vWorkspaceID='" & Me.HProjectId.Value.Trim() & "' And "
            Wstr += " cStatusIndi <> 'D'"

            If Not ObjHelp.GetArchieveDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_TocheckLabReportStatus, estr) Then
                Throw New Exception(estr)
            End If

            If ds_TocheckLabReportStatus.Tables(0).Rows.Count <> 0 Then
                GV_ArchieveDetail.Columns(8).Visible = True
                For i As Integer = 0 To ds_TocheckLabReportStatus.Tables(0).Rows.Count - 1
                    If ds_TocheckLabReportStatus.Tables(0).Rows(i)("cArchiveForLabData").ToString().Trim = "Y" Then
                        GV_ArchieveDetail.Columns(8).Visible = False
                        Exit For
                    End If
                Next
            End If


            'rblArchive.Items(1).Enabled = False
            rblArchive.Items(0).Selected = True

            dt_ArchiveDetail = ds_ArchiveDetail.Tables(0).Copy
            '======this code is to display them status Archived and unArchived in grid based on statys "Y" and "N" in database==================
            If ds_ArchiveDetail.Tables(0).Rows.Count <> 0 Then
                hdn_prjectstatus.Value = (ds_ArchiveDetail.Tables(0).Rows(0).Item(GV_iArchiveStatus)).ToString
                hdn_LabArchiveStatus.Value = IIf(ds_ArchiveDetail.Tables(0).Rows(0).Item("cArchiveForLabData") Is System.DBNull.Value, "", ds_ArchiveDetail.Tables(0).Rows(0).Item("cArchiveForLabData"))
                For i As Integer = 0 To ds_ArchiveDetail.Tables(0).Rows.Count - 1
                    If ds_ArchiveDetail.Tables(0).Rows(i).Item(GV_iArchiveStatus).ToString = "Y" Then
                        txtYears.Text = ds_ArchiveDetail.Tables(0).Rows(0).Item(4)
                        dt_ArchiveDetail.Rows(i).Item(GV_iArchiveStatus) = "Archived"
                        'txtYears.Text = ds_ArchiveDetail.Tables(0).Rows(0).Item(4)
                        rblArchive.Items(0).Enabled = False
                        rblArchive.Items(1).Selected = True
                        rblArchive.Items(1).Enabled = True
                    Else
                        dt_ArchiveDetail.Rows(i).Item(GV_iArchiveStatus) = "UnArchived"
                        rblArchive.Items(0).Enabled = True
                        rblArchive.Items(0).Selected = True
                        rblArchive.Items(1).Selected = False
                        rblArchive.Items(1).Enabled = False
                    End If
                Next
                dt_ArchiveDetail.AcceptChanges()
            End If
            '=======================================================================================================================================
            If ds_ArchiveDetail.Tables(0).Rows.Count <> 0 Then
                GV_ArchieveDetail.DataSource = dt_ArchiveDetail
                GV_ArchieveDetail.DataBind()
                ForExport.DataSource = dt_ArchiveDetail
                ForExport.DataBind()
            Else
                hdn_prjectstatus.Value = 0 '====== If project Is going to archiv then it will ask for years========
                GV_ArchieveDetail.DataSource = Nothing
                GV_ArchieveDetail.DataBind()
                rblArchive.Items(0).Selected = True
                rblArchive.Items(0).Enabled = True
                rblArchive.Items(1).Enabled = False
                rblArchive.Items(1).Selected = False
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Script", "ForEmptyGrid();", True)

            End If
            'txtYears.Text = ""
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".......FillData")
            Return False
        End Try
    End Function

#End Region

#Region "GridEvents"

    Protected Sub GV_ArchieveDetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_ArchieveDetail.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Or e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(GV_WorkspaceId).Visible = False
            End If
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("Audittrail"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("Audittrail"), ImageButton).CommandName = "AUDITTRAIL"
                CType(e.Row.FindControl("ArchiveLabrpt"), LinkButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ArchiveLabrpt"), LinkButton).CommandName = "ArchiveLabData"

            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....GV_ArchieveDetail_RowDataBound")
        End Try
    End Sub

    Protected Sub GV_ArchieveDetail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GV_ArchieveDetail.RowCommand
        Dim wstr As String = String.Empty
        Dim estr_retu As String = String.Empty
        Dim ds_ArchiveDetail As DataSet = Nothing
        Dim index As String
        Dim SuccessLabRpt As Integer
        Dim WorkspaceId As String = String.Empty
        Dim dr As DataRow = Nothing
        index = e.CommandArgument.ToString
        Dim ds_LabArchiveData As DataSet = Nothing
        Dim dc_dModifyon As DataColumn
        Dim dc_dLabDataMedexOn As DataColumn
        If e.CommandName.ToUpper().Trim() = "ARCHIVELABDATA" Then

            wstr = "vWorkspaceID='" & Me.HProjectId.Value.Trim() & "' And "
            wstr += " cStatusIndi <> 'D'"

            '======this is to get latest status and max(itranno)======================================================
            If Not ObjHelp.view_ArchiveProjectMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_LabArchiveData, estr_retu) Then
                Throw New Exception("Error While Getting Last Record")
            End If
            ds_LabArchiveData.Tables(0).Rows(0).Item("cArchiveForLabData") = "Y"
            ds_LabArchiveData.Tables(0).Rows(0).Item("dLabDataModifyOn") = Date.Now.ToString
            ds_LabArchiveData.Tables(0).Columns.Remove("iUserId")
            ds_LabArchiveData.Tables(0).Columns.Remove("vFirstName")
            ds_LabArchiveData.Tables(0).Columns.Remove("vProjectNo")
            ds_LabArchiveData.Tables(0).Columns.Remove("vUserTypeName")
            ds_LabArchiveData.AcceptChanges()
            If proc_TransferLabRptForArchive(SuccessLabRpt) Then
                If (SuccessLabRpt = 1 Or EroorCode = 2) Then
                    ObjCommon.ShowAlert("LabReportS are Not Perfectly Archived.Try Again", Me.Page)
                    Exit Sub
                End If
                GV_ArchieveDetail.Columns(8).Visible = False
            End If
            ds_LabArchiveData.Tables(0).TableName = "ArchiveProjectMst"
            If Not ObjLambda.Save_ArchiveDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_LabArchiveData, Session(S_UserID), estr_retu) Then
                Throw New Exception(estr_retu)
            End If
            ObjCommon.ShowAlert("LabReport For This Project Has Been Archived Successfully", Me.Page)
            GV_ArchieveDetail.Columns(8).Visible = False
        End If


        If e.CommandName.ToUpper().Trim() = "AUDITTRAIL" Then
            WorkspaceId = GV_ArchieveDetail.Rows(index).Cells(7).Text

            wstr = "vWorkspaceID='" + WorkspaceId + "' And "
            wstr += " cStatusIndi <> 'D' order by iTranNo desc"

            If Not ObjHelp.view_ArchiveProjectAuditTrail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ArchiveDetail, estr_retu) Then
                Throw New Exception(estr_retu)
            End If


            dc_dModifyon = New DataColumn("dModifyOn_IST", System.Type.GetType("System.String"))
            dc_dLabDataMedexOn = New DataColumn("dLabDataModifyOn_IST", System.Type.GetType("System.String"))
            ds_ArchiveDetail.Tables(0).Columns.Add("dModifyOn_IST")
            ds_ArchiveDetail.AcceptChanges()
            ds_ArchiveDetail.Tables(0).Columns.Add("dLabDataModifyOn_IST")
            ds_ArchiveDetail.AcceptChanges()

            If ds_ArchiveDetail.Tables(0).Rows.Count <> 0 Then
                For i As Integer = 0 To ds_ArchiveDetail.Tables(0).Rows.Count - 1


                    If ds_ArchiveDetail.Tables(0).Rows(i).Item("cArchiveFlag").ToString = "N" Then
                        ds_ArchiveDetail.Tables(0).Rows(i).Item("cArchiveFlag") = "Project-UnArchived"
                    Else
                        ds_ArchiveDetail.Tables(0).Rows(i).Item("cArchiveFlag") = "Project-Archived"
                    End If
                    If ds_ArchiveDetail.Tables(0).Rows(i).Item("cArchiveForLabData").ToString = "" Then
                        ds_ArchiveDetail.Tables(0).Rows(i).Item("cArchiveForLabData") = "NA"
                    ElseIf ds_ArchiveDetail.Tables(0).Rows(i).Item("cArchiveForLabData") = "Y" Then
                        ds_ArchiveDetail.Tables(0).Rows(i).Item("cArchiveForLabData") = "Lab-Report Archived"
                        ds_ArchiveDetail.Tables(0).Rows(i).Item("dLabDataModifyOn_IST") = Convert.ToString(ds_ArchiveDetail.Tables(0).Rows(i).Item("dLabDataModifyOn") + " IST (+5.5 GMT)")
                    ElseIf ds_ArchiveDetail.Tables(0).Rows(i).Item("cArchiveForLabData") = "N" Then
                        ds_ArchiveDetail.Tables(0).Rows(i).Item("cArchiveForLabData") = "Lab-Report UnArchived"
                        ds_ArchiveDetail.Tables(0).Rows(i).Item("dLabDataModifyOn_IST") = Convert.ToString(ds_ArchiveDetail.Tables(0).Rows(i).Item("dLabDataModifyOn") + " IST (+5.5 GMT)")
                        'ElseIf ds_ArchiveDetail.Tables(0).Rows(i).Item("cArchiveForLabData") = "U" Then
                        '    ds_ArchiveDetail.Tables(0).Rows(i).Item("cArchiveForLabData") = "Lab-Report Archived"
                    End If


                    ds_ArchiveDetail.Tables(0).Rows(i).Item("dModifyOn_IST") = Convert.ToString(ds_ArchiveDetail.Tables(0).Rows(i).Item("dModifyOn") + " IST (+5.5 GMT)")
                Next
            End If



            'For Each dr_dModifyOn In ds_ArchiveDetail.Tables(0).Rows
            '    dr_dModifyOn("dModifyOn_IST") = Convert.ToString(dr_dModifyOn("dModifyOn") + " (IST)")
            'Next
            ds_ArchiveDetail.AcceptChanges()

            GV_Audittrail.DataSource = ds_ArchiveDetail
            GV_Audittrail.DataBind()
            Me.MPEAction.Show()

        End If
    End Sub

#End Region

#Region "BtnClick"

    Protected Function btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) As Boolean Handles btnSetProject.Click

        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty

        Dim ds_ArchiveDetail As DataSet = Nothing
        Wstr = "vWorkspaceID='" & Me.HProjectId.Value.Trim() & "' And "
        Wstr += " cStatusIndi <> 'D'"

        If Not ObjHelp.view_ArchiveProjectMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_ArchiveDetail, estr) Then
            ObjCommon.ShowAlert("Error While Getting Data From view_ArchiveprojectMst", Me.Page)
        End If

        'If ds_ArchiveDetail.Tables(0).Rows.Count <> 0 Then
        '    If ds_ArchiveDetail.Tables(0).Rows(0)("CArchiveForLabData").ToString = "Y" Then
        '        GV_ArchieveDetail.Columns(8).Visible = False
        '    End If
        'End If


        If ds_ArchiveDetail.Tables(0).Rows.Count = 0 Then
            btnSave.Attributes.Add("onclick", "return PromptForLabRptArchive();")
        End If

        If Not Me.FillData() Then
            ObjCommon.ShowAlert("Error While filling ArchiveDetail", Me.Page)
            Exit Function
        End If
        txtYears.Text = ""
        txtRemark.Text = ""
    End Function

    Protected Sub btnShowAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowAll.Click

        Dim ds_ArchiveAllDetail As New DataSet
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim count As Integer = 0
        Try

            Wstr += " cStatusIndi <> 'D'"

            If Not ObjHelp.view_ArchiveProjectMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_ArchiveAllDetail, estr) Then
                Throw New Exception
            End If

            '=========Code to Display Archived Instead of Y and Unarchived Instead Of N================
            If ds_ArchiveAllDetail.Tables(0).Rows.Count <> 0 Then

                For count = 0 To ds_ArchiveAllDetail.Tables(0).Rows.Count - 1

                    If ds_ArchiveAllDetail.Tables(0).Rows(count).Item(GV_iArchiveStatus).ToString = "Y" Then

                        ds_ArchiveAllDetail.Tables(0).Rows(count).Item(GV_iArchiveStatus) = "Archived"

                    Else
                        ds_ArchiveAllDetail.Tables(0).Rows(count).Item(GV_iArchiveStatus) = "UnArchived"
                    End If
                Next

                ds_ArchiveAllDetail.AcceptChanges()
                GV_ArchieveDetail.DataSource = ds_ArchiveAllDetail
                GV_ArchieveDetail.DataBind()
                GV_ArchieveDetail.Columns(8).Visible = False
                '====== this Grid is only To export Into pdf Because datatble grid GV_ArchieveDetail creates problem for export"
                ForExport.DataSource = ds_ArchiveAllDetail
                ForExport.DataBind()

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......btnShowAll_Click")
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            btnSave.Attributes.Remove("onclick")
            If Not AssignValues() Then
                Throw New Exception("Error While Archiving")
            End If
            If Not Me.FillData() Then
                ObjCommon.ShowAlert("Error While Filling ArchiveDetail", Me.Page)
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "......btnSave_Click")
        End Try
    End Sub

    Protected Sub btnExportToPdf_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToPdf.Click

        If GV_ArchieveDetail.Rows.Count <> 0 Then

            Response.Clear()
            Response.Buffer = True
            Response.ContentType = "application/pdf"
            Response.AddHeader("content-disposition", "attachment;filename=ArchiveStatusDetail.pdf")
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Dim StringWriter1 As New StringWriter()
            Dim HtmlTextWriter1 As New HtmlTextWriter(StringWriter1)
            ForExport.HeaderRow.Style.Add("width", "15%")
            ForExport.HeaderRow.Style.Add("font-size", "10px")
            ForExport.Style.Add("text-decoration", "none")
            ForExport.Style.Add("font-family", "Arial, Helvetica, sans-serif;")
            ForExport.Style.Add("font-size", "8px")
            ForExport.Style.Add("font-color", "blue")
            Dim p As Paragraph = New Paragraph("Archive Project Detail")
            p.Alignment = Element.ALIGN_CENTER
            p.SpacingAfter = 10
            ForExport.RenderControl(HtmlTextWriter1)
            Dim StringReader1 As New StringReader(StringWriter1.ToString())
            Dim newDocument As New Document(PageSize.A4, 7.0F, 7.0F, 7.0F, 7.0F)
            Dim HTMLWorker1 As New HTMLWorker(newDocument)
            PdfWriter.GetInstance(newDocument, Response.OutputStream)
            newDocument.Open()
            newDocument.Add(p)
            HTMLWorker1.Parse(StringReader1)
            newDocument.Close()
            Response.Write(newDocument)
            ForExport.DataSource = Nothing
            Response.End()

        End If
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Don't remove this function
        ' This Function is mendetory when you are going to export your grid to excel.
        ' NOTE :: And Click event of button must be in postback trigger. (Page must be loaded)
    End Sub

    Protected Sub btnImportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportToExcel.Click
        Try
            If GV_ArchieveDetail.Rows.Count <> 0 Then

                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                ForExport.RenderControl(writer)
                Dim gridViewhtml As String = stringWriter.ToString()
                Dim fileName As String = "ProjectStatusDetail" & ".xls"
                Context.Response.Buffer = True
                Context.Response.ClearContent()
                Context.Response.ClearHeaders()
                Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
                Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)
                Context.Response.Write(gridViewhtml.Substring(5).Substring(0, gridViewhtml.Substring(5).Length - 6))
                ForExport.DataSource = Nothing
                Context.Response.Flush()
                Context.Response.End()
                File.Delete(fileName)

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......btnImportToExcel_Click")
        End Try
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Try
            Me.Response.Redirect("frmMainPage.aspx?mode=1", False)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....btnExit_Click")
        End Try
    End Sub

#End Region

   
    Protected Sub GV_Audittrail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_Audittrail.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(8).Text.Trim().Equals("&nbsp;") Then
                e.Row.Cells(8).Text = "NA"
            End If
        End If
    End Sub
End Class
