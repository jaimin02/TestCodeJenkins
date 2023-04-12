Imports System.Collections.Generic
Imports Winnovative
Imports System.Drawing

Partial Class frmWorkspaceSubjectMst
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
    Private rpage As RepoPage

    Private Const VS_Choice As String = "Choice"
    Private Const VS_Choice1 As String = "Choice"
    Private Const VS_WorkspaceSubjectId As String = "vWorkspaceSubjectId"

    Private Const VS_WorkspaceId As String = "vWorkspaceId"
    Private Const VS_MySubjectNo As String = "iMySubjectNo"

    Private Const VS_SrNo As String = "iSrNo"
    Private Const VS_ReportingDate As String = "dReportingDate"
    Private Const VS_Initials As String = "vInitials"
    Private Const VS_SubjectId As String = "vSubjectId"
    Private Const VS_Mode As String = "chkMode"
    Private Const VS_iMySubjectNo As String = "iMySubjectNo"
    'Private Const Location_Canada As String = "0003"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVCell_SubjectId As Integer = 1
    Private Const GVCell_MySubjectNo As Integer = 2
    Private Const GVCell_Subject As Integer = 3
    Private Const GVCell_Initials As Integer = 4
    Private Const GVCell_Period As Integer = 5
    Private Const GVCell_ReportingDateTime As Integer = 6
    Private Const GVCell_Reason As Integer = 7
    Private Const GVCell_attendenceTakenBy As Integer = 8
    Private Const GVCell_Edit As Integer = 9
    Private Const GVCell_Rejected As Integer = 10
    Private Const GVC_Code As Integer = 11
    Private Const GVC_View As Integer = 12
    Private Const GVCell_cRejected As Integer = 13
    Private Const GVCell_iScrDays As Integer = 14
    Private Const GVCell_iMysubNo As Integer = 15
    Private Const GVCell_Delete As Integer = 16
    Private Const GVCell_Audit As Integer = 17
    Private Const GVCell_ScrHDr As Integer = 18

    Private Const GVCAudit_iAsnNo As Integer = 0
    Private Const GVCAudit_vInitials As Integer = 1
    Private Const GVCAudit_vSubjectID As Integer = 2
    Private Const GVCAudit_vMySubjectNo As Integer = 3
    Private Const GVCAudit_dReportingDate As Integer = 4
    Private Const GVCAudit_vModifyBy As Integer = 5
    Private Const GVCAudit_dModifyOn As Integer = 6
    Private Const GVCAudit_vRemarks As Integer = 7
    Private Const GVCAudit_nWorkspaceSubjectHistoryId As Integer = 8
    Private Const GVCAudit_vWorkspaceSubjectId As Integer = 9
    Private Const GVCAudit_iTranN As Integer = 10

    Private Const GVCAuditTrail_dReportingDate As Integer = 3
    Private Const GVCAuditTrail_dModifyOn As Integer = 5

    Dim eStr_Retu As String
    Dim reportingdate As String = String.Empty
    Dim replaceoffset As String = String.Empty

    Private Const ASN_forExcel As Integer = 0
    Private Const vSubjectId_forExcel As Integer = 1
    Private Const vMySubjectNo_forExcel As Integer = 2
    Private Const FullName_forExcel As Integer = 3
    Private Const vInitials_forExcel As Integer = 4
    Private Const iPeriod_forExcel As Integer = 5
    Private Const dReportingDate_forExcel As Integer = 6
    Private Const vReasonDesc_forExcel As Integer = 7
    Private Const vUserName_forExcel As Integer = 8
    Private Const cRejectionFlag_forExcel As Integer = 9
    Private Const iScrDays_forExcel As Integer = 10

#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            GenCall()
        End If
        If gvwWorkspaceSubjectMst.Rows.Count > 0 Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetAttendanceData", "fsetAttendanceData_Show(); ", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvwWorkspaceSubjectMst", "UIgvwWorkspaceSubjectMst(); ", True)
        End If
    End Sub

#End Region

#Region "GENCALL"

    Private Function GenCall() As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim dt_WorkspaceSubjectmst As DataTable = Nothing
        Dim ds_WorkspaceSubjectmst As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim Choice1 As WS_Lambda.DataObjOpenSaveModeEnum

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Choice = Me.Request.QueryString("mode").ToString
            Me.ViewState(VS_Choice) = Choice   'To use it while saving
            Me.ViewState(VS_Mode) = Choice

            If Not GenCall_ShowUI(Choice, dt_WorkspaceSubjectmst) Then 'For Displaying Data 
                Return False
            End If

            Choice1 = CType("1", WS_Lambda.DataObjOpenSaveModeEnum)
            If Not IsNothing(Me.Request.QueryString("mode")) AndAlso _
                    Me.Request.QueryString("mode") <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Choice1 = Me.Request.QueryString("mode")   'To be used while QC(View)
            End If

            Me.ViewState(VS_Choice) = Choice

            If Choice1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                If (Not Me.Request.QueryString("WorkSpaceId") Is Nothing) AndAlso (Not Me.Request.QueryString("Type") Is Nothing) Then
                    Me.txtproject.Text = Request.QueryString("ProjectNo").ToString()
                    Me.txtproject.Enabled = False
                    HideMenu()
                End If
                chkProject.Visible = False
                tdsubject.Visible = False
                btnAdd.Visible = False
                btnCancel.Visible = False
                tdScreeningValidDays.Visible = False
            End If
            GenCall = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...GenCall")
            Return False
        Finally
        End Try
    End Function
#End Region

#Region "GENCALL_SHOW_UI"

    Private Function GenCall_ShowUI(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, ByVal dt_WorkspaceSubjectmst As DataTable) As Boolean
        Dim WorkspaceId As String = String.Empty
        Dim estr As String = String.Empty
        Dim wstr1 As String = String.Empty
        Dim dsWorkspace As New DataSet
        Dim ds_SubjectBlob As New DataSet
        Dim sender As New Object
        Dim e As New EventArgs
        Try

            Page.Title = ":: Attendance  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            CType(Master.FindControl("lblHeading"), Label).Text = "Attendance"

            If Not IsNothing(Me.Request.QueryString("workspaceid")) Then
                WorkspaceId = Me.Request.QueryString("workspaceid").Trim()
            End If

            If WorkspaceId.Trim() <> "" Then
                If Not Me.objHelp.getworkspacemst("vWorkspaceId='" & WorkspaceId.Trim() & "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            dsWorkspace, estr) Then
                    Me.ShowErrorMessage(estr, "")
                    GenCall_ShowUI = False
                    Exit Function
                End If

                Me.txtproject.Text = dsWorkspace.Tables(0).Rows(0).Item("vWorkSpaceDesc")
                Me.HProjectId.Value = WorkspaceId.Trim()
                fillvalues()
            End If

            If Not (Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Or Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View) Then
                'Me.txtDate.Text = ConvertDbNullToDbTypeDefaultValue(dt_WorkspaceSubjectmst.Rows(0)("dReportingDate"), dt_WorkspaceSubjectmst.Rows(0)("dReportingDate").GetType)
                'Me.txtDate.Text = Format(CDate(Me.txtDate.Text.Trim()), "dd-MMM-yyyy")
                Me.HSubjectId.Value = ConvertDbNullToDbTypeDefaultValue(dt_WorkspaceSubjectmst.Rows(0)("vSubjectId"), dt_WorkspaceSubjectmst.Rows(0)("vSubjectId").GetType)
                'Me.ddlSubjectmst.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_WorkspaceSubjectmst.Rows(0)("vSubjectId"), dt_WorkspaceSubjectmst.Rows(0)("vSubjectId").GetType)
                Me.ddlPeriod.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_WorkspaceSubjectmst.Rows(0)("iPeriod"), dt_WorkspaceSubjectmst.Rows(0)("vSubjectId").GetType)
            End If

            Me.Image1.ImageUrl = "~/Images/NotFound.gif"


            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
                GenCall_ShowUI = True
                Exit Function
            End If


            '==added on 10-sep-2011
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
            '========
            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
            Return False
        End Try
    End Function

#End Region

#Region "Reset Page"
    Private Sub ResetPage()
        'GenCall()

        'Me.txtDate.Text = ""
        Me.txtproject.Text = ""
        Me.HProjectId.Value = ""

        Me.txtScreenDays.Text = ""

        Me.txtSubject.Text = ""
        Me.HSubjectId.Value = ""
        'FillGridView()
        '===added on 16-Jan-2010 by deepak singh
        Me.Image1.ImageUrl = "~/Images/NotFound.gif"
        Me.txtLockRemark.Text = ""
        '=======

        ''Me.ddlSubjectmst.SelectedIndex = "0"
        'Me.ddlPeriod.SelectedIndex = "0"
        Response.Redirect("frmWorkspaceSubjectMst.aspx?mode=1")
    End Sub
#End Region

#Region "Save"

    Private Function AssignUpdatedValues(ByRef Ds_WorkspacesubjectMst As DataSet) As Boolean
        Dim estr As String = String.Empty
        Dim dr As DataRow
        Dim CanStartAfterDetails As String = String.Empty
        Dim MySubjectNo As String = String.Empty
        Dim vMySubjectNo As String = "0"
        Dim dsSubjectMst As New DataSet
        Dim dsWorkspaceSubjectMst As New DataSet
        Dim Wstr As String = "vSubjectId='" + Me.HSubjectId.Value.Trim() + "'"

        If Not objHelp.GetWorkspaceSubjectMst("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_WorkspacesubjectMst, estr) Then
            Throw New Exception("Error While Getting Blank Structure")
        End If

        If (Me.HSubjectId.Value.Trim <> "") Then
            If Not objHelp.GetSubjectMaster(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsSubjectMst, estr) Then
                ShowErrorMessage(eStr_Retu, "")
            End If
        End If

        Try
            'Added on 18-Aug-2009
            MySubjectNo = "0"
            If (Me.ddlPeriod.SelectedValue > 1) Then
                If Not objHelp.GetWorkspaceSubjectMaster("vSubjectId='" + Me.HSubjectId.Value.Trim() + "' and vWorkspaceId='" & _
                                                        Me.HProjectId.Value.Trim() & "' And iPeriod=1 and cStatusindi<>'D' ", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                        dsWorkspaceSubjectMst, estr) Then
                    Throw New Exception(estr)
                End If

                MySubjectNo = dsWorkspaceSubjectMst.Tables(0).Rows(0).Item("iMySubjectNo").ToString.Trim()
                vMySubjectNo = dsWorkspaceSubjectMst.Tables(0).Rows(0).Item("vMySubjectNo").ToString.Trim()

            End If
            '**********************************
            Dim timeZoneInfo As TimeZoneInfo = timeZoneInfo.FindSystemTimeZoneById(Session(S_TimeZoneName).ToString)
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Ds_WorkspacesubjectMst.Tables(0).Clear()
                'Ds_WorkspacesubjectMst.Tables(0).Columns("dReportingDate").DataType = System.Type.GetType("System.datetimeoffset")
                'Ds_WorkspacesubjectMst.Tables(0).Columns("dModifyOn").DataType = System.Type.GetType("System.datetimeoffset")
                dr = Ds_WorkspacesubjectMst.Tables(0).NewRow
                dr("vWorkspaceSubjectId") = "0000"
                dr("vWorkspaceid") = Me.HProjectId.Value.Trim()
                dr("iMySubjectNo") = MySubjectNo
                dr("vMySubjectNo") = vMySubjectNo
                dr("vSubjectId") = Me.HSubjectId.Value.Trim()
                dr("vInitials") = dsSubjectMst.Tables(0).Rows(0)("vInitials")

                dr("iPeriod") = "0"
                If (Me.ddlPeriod.SelectedIndex <> 0) Then
                    dr("iPeriod") = Me.ddlPeriod.SelectedValue
                End If

                dr("dReportingDate") = Me.ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName)) 'CType(System.DateTime.Now.ToString, DateTimeOffset)  'Its getting saved from Insert Procedure
                'If (Me.txtDate.Text <> "") Then
                '    dr("dReportingDate") = IIf(String.IsNullOrEmpty(txtDate.Text.Trim()), System.DBNull.Value, Me.txtDate.Text.Trim())
                'End If

                dr("cRejectionFlag") = "N"
                dr("nReasonNo") = "0"
                dr("dModifyOn") = System.DateTime.Now() 'Format(CDate(System.DateTime.Now.ToString), "yyyy-MM-dd HH:mm") + ":00.0000000 " + timeZoneInfo.BaseUtcOffset.ToString.Substring(0, timeZoneInfo.BaseUtcOffset.ToString.Length - 3)
                dr("cStatusIndi") = ""
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("iScrDays") = Me.txtScreenDays.Text.Trim()
                Ds_WorkspacesubjectMst.Tables(0).Rows.Add(dr)

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                For Each dr In Ds_WorkspacesubjectMst.Tables(0).Rows
                    dr("vWorkspaceid") = Me.HProjectId.Value.Trim
                    dr("IMySubjectNo") = MySubjectNo
                    dr("vMySubjectNo") = vMySubjectNo
                    dr("vSubjectId") = Me.HSubjectId.Value.Trim()
                    dr("vInitials") = dsSubjectMst.Tables(0).Rows(0)("vInitials")
                    dr("iPeriod") = "0"
                    If (Me.ddlPeriod.SelectedIndex <> 0) Then
                        dr("iPeriod") = Me.ddlPeriod.SelectedValue
                    End If

                    dr("dReportingDate") = Me.ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName)) 'CType(System.DateTime.Now.ToString, DateTimeOffset) 'Its getting saved from Insert Procedure
                    'If (Me.txtDate.Text <> "") Then
                    '    dr("dReportingDate") = IIf(String.IsNullOrEmpty(txtDate.Text.Trim()), System.DBNull.Value, Me.txtDate.Text.Trim())
                    'End If

                    dr("cRejectionFlag") = "N"
                    dr("nReasonNo") = "0"
                    dr("dModifyOn") = System.DateTime.Now()
                    dr("cStatusIndi") = ""
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr.AcceptChanges()
                Next
                Ds_WorkspacesubjectMst.AcceptChanges()
            End If

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...AssignUpdatedValues")
            Return False
        End Try
    End Function

    Private Sub SaveRejected(Optional ByVal rejected As Boolean = False)
        Dim dsTemp As New DataSet
        Dim dtTemp As New DataTable
        Dim dsAttendence As New DataSet
        Dim dr As DataRow
        Dim Wstr As String = "vWorkspaceSubjectId ='" + ViewState(GVC_Code) + "'"

        Try
            If objHelp.GetViewWorkspaceSubjectMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsTemp, eStr_Retu) Then
                dtTemp = dsTemp.Tables(0)
                dr = dtTemp.NewRow
                dr("cRejectionFlag") = "N"
                dr("nReasonNo") = System.Decimal.Zero
                dr("iModifyBy") = Me.Session(S_UserID)

                If rejected Then
                    For Each dr In dtTemp.Rows
                        dr("cRejectionFlag") = "Y"
                        dr("nReasonNo") = Me.ddlReject.SelectedValue
                        dr("iModifyBy") = Me.Session(S_UserID)
                        dr.AcceptChanges()
                    Next
                    dtTemp.AcceptChanges()
                End If
            Else
                ShowErrorMessage(eStr_Retu, "...SaveRejected")
            End If

            If Not objLambda.Insert_WorkspaceSubjectMaster_Rejection(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, dsTemp, _
                                    Me.Session(S_UserID), eStr_Retu) Then
                Dim estr As String = "Error Saving Subject Attendance !"
                ObjCommon.ShowAlert(estr, Me)
                Exit Sub
            End If

            If rejected Then
                Me.txtSubjectRemark.Text = ""
                'divRejectSubject.Attributes.Add("style", "display:none")
            End If
            ObjCommon.ShowAlert("Subject Rejected Successfully !", Me)
            Me.txtSubject.Text = ""
            Me.HSubjectId.Value = ""
            FillGridView()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...SaveRejected")
        End Try
    End Sub

    Private Function ValidateSubject() As Boolean
        Dim wStr As String = String.Empty
        Dim ds_WorkspacSubMst As New DataSet
        Dim eStr As String = String.Empty

        Try
            wStr = " vWorkspaceID = '" & Me.HProjectId.Value.Trim() & "' AND vSubjectID = '" _
                  & Me.HSubjectId.Value.Trim() & "' AND iPeriod = " & Me.ddlPeriod.SelectedValue & " And cStatusindi <>'D'"

            If Not objHelp.GetWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                   ds_WorkspacSubMst, eStr) Then
                Throw New Exception(eStr)

            End If

            If ds_WorkspacSubMst.Tables(0).Rows.Count > 0 Then
                ObjCommon.ShowAlert("Attendence of the Subject " + Me.HSubjectId.Value + " is already done in selected Project!", Me)
                Return False
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...ValidateSubject")
            Return False
        End Try
    End Function

#End Region

#Region "Button event"

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Dt As New DataTable
        Dim Ds_Grid As New DataSet
        Dim Ds_WorkspaceSubjectMst As New DataSet
        Dim eStr As String = String.Empty
        Dim ds_type As New Data.DataSet
        Dim objOPws As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
        Dim ds_MedExScreen As New Data.DataSet
        Dim altStr As String = String.Empty
        Dim snd As New Object
        Dim e1 As New System.EventArgs
        Dim ds_NextSubAttendenceDtl As New DataSet
        Dim Str As String = String.Empty

        Try
            ''Validation Added by Aaditya on 02-Jun-2015 For not allow attenedance of subject if attendance done in next child project
            Str = Me.HProjectId.Value.ToString.Trim + "##" + CType(Me.ddlPeriod.SelectedValue, Integer).ToString.Trim

            ds_NextSubAttendenceDtl = objHelp.ProcedureExecute("dbo.Proc_GetNextSubjectAttendenceDetaill", Str)

            If Not IsNothing(ds_NextSubAttendenceDtl) AndAlso ds_NextSubAttendenceDtl.Tables(0).Rows.Count > 0 Then
                ObjCommon.ShowAlert("Next Project " + ds_NextSubAttendenceDtl.Tables(0).Rows(0)("vProjectNo").ToString.Trim() + " Attendance Done. You Can't Proceed.", Me.Page)
                Exit Sub
            End If
            ''Ended By Aaditya
            'Validation of Last Screening Date
            If Not Me.objHelp.GetMedExScreeningHdr("vSubjectId='" & Me.HSubjectId.Value.Trim() & "' order by dScreenDate desc", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                        ds_MedExScreen, eStr) Then
                ObjCommon.ShowAlert("Error While Getting Last Screening Data!", Me.Page)
                Exit Sub
            End If

            If ds_MedExScreen.Tables(0).Rows.Count > 0 Then
                If Val(Me.txtScreenDays.Text) < DateDiff(DateInterval.Day, ObjCommon.GetDTOffsetTOdatetime(ds_MedExScreen.Tables(0).Rows(0).Item("dScreenDate")), CDate(ObjCommon.GetCurDatetime(Me.Session(S_TimeZoneName)))) Then 'CDate(System.DateTime.Now)) Then
                    altStr = "Last Screening Date of this Subject is More than " & Me.txtScreenDays.Text & "Days!Are you sure you want to take Attendance of the Subject?"
                    'Exit Sub

                End If
            Else
                altStr = "No Screening Has Been Done For this Subject! Are you sure you want to take Attendance of the Subject?"
                'Exit Sub
            End If
            '**************************
            If altStr <> "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowAttConfirm", "ShowAttConfirm('" & altStr & "',this);", True)
            Else
                btnchkScrDate_Click(snd, e1)
            End If

        Catch ex As System.Threading.ThreadAbortException
        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr + "...btnAdd_Click")
        End Try
    End Sub

    Protected Sub btnchkScrDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnchkScrDate.Click
        Dim eStr As String = String.Empty
        Dim Ds_WorkspacesubjectMst As DataSet = Nothing

        Try

            If Not ValidateSubject() Then
                Exit Sub
            End If

            If Not AssignUpdatedValues(Ds_WorkspacesubjectMst) Then
                Throw New Exception("Error While Saving Data")
            End If
            Ds_WorkspacesubjectMst.Tables(0).TableName = "VIEW_workspaceSubjectMst"   ' New Values on the form to be updated


            If Not objLambda.Save_InsertWorkspaceSubjectMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_WorkspaceSubjectMst, Ds_WorkspacesubjectMst, Me.Session(S_UserID), eStr) Then
                ObjCommon.ShowAlert("Error while Saving WorkspaceSubjectMst", Me.Page)
                Exit Sub
            End If

            ObjCommon.ShowAlert("Attendance saved successfully !", Me)
            FillGridView()
            Me.txtSubject.Text = ""
            Me.Image1.ImageUrl = "~/Images/NotFound.gif"
            'ResetPage()

        Catch ex As System.Threading.ThreadAbortException
        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr + "...btnchkScrDate_Click")
        End Try

    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'divRejectSubject.Attributes.Add("style", "display:none")
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            SaveRejected(True)
            FillGridView()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnSave_Click")
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ResetPage()
    End Sub

    Protected Sub btnCloseNew_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim WorkspaceId As String = String.Empty
        Dim DMS As String = String.Empty

        Try
            If Not IsNothing(Me.Request.QueryString("Page2")) AndAlso Me.Request.QueryString("Page2").Trim() <> "" Then

                If Me.Request.QueryString("Page2").ToUpper.Trim() = "FRMMEDEXINFOHDRDTL" Then
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "CloseWin", "window.close();", True)
                    Exit Sub
                End If

                WorkspaceId = Me.Request.QueryString("workspaceid").Trim()
                Me.Response.Redirect(Me.Request.QueryString("Page2") & ".aspx?Type=" & Me.Request.QueryString("Type") & "&WorkSpaceId=" & WorkspaceId.Trim() & _
                                  "&page=" & Me.Request.QueryString("Page") & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))
            Else
                Response.Redirect("frmMainPage.aspx")
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnCloseNew_Click")
        End Try
    End Sub

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        'added by vishal for lock/unlock site
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_Check As New DataSet
        Dim dv_Check As New DataView
        Dim ds_Subjects As New DataSet
        Dim ds_CreatedBy As New DataSet
        Dim dv_CreatedBy As New DataView
        Me.chkProject.Checked = True

        Try
            If Me.ViewState(VS_Mode) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"

                If Not Me.objHelp.GetCRFLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    ds_Check, eStr) Then
                    Throw New Exception(eStr)
                End If
                If Not ds_Check Is Nothing Then

                    dv_Check = ds_Check.Tables(0).DefaultView
                    dv_Check.Sort = "iTranNo desc"
                    ' edited by vishal for lock/unlock site
                    If dv_Check.ToTable().Rows.Count > 0 Then

                        If Convert.ToString(dv_Check.ToTable().Rows(0)("cLockFlag")).Trim.ToUpper() = "L" Then
                            Me.ObjCommon.ShowAlert("Project is Locked.", Me.Page)
                            Me.txtproject.Text = ""
                            Me.HProjectId.Value = ""
                            Exit Sub
                        End If
                    End If
                End If
            End If

            If Not Me.fillvalues() Then
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Getting  Lock Details ", ex.Message + "...btnSetProject_Click")
        End Try

    End Sub

    Protected Sub btnSubject_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim wStr1 As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds_SubjectBlob As New DataSet

        '****added on 13-jan-2010 by deepak singh to show default 
        'image if there is no image of subject
        Try
            wStr1 = "cRecordType='P' and iTranNo=(select Max(itranNO) from SubjectBlobDetails where cRecordType='P' and vSubjectId='" & HSubjectId.Value.ToString.Trim & "') and vSubjectId='" & HSubjectId.Value.ToString.Trim & "'"

            If Not Me.objHelp.getSubjectBlobDetails(wStr1, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubjectBlob, estr) Then
                MsgBox("Error while getting Data." + vbCrLf + estr)
                Exit Sub
            End If

            Me.Image1.ImageUrl = "~/Images/NotFound.gif"
            If ds_SubjectBlob.Tables(0).Rows.Count > 0 Then
                Me.Image1.ImageUrl = "frmPIFImage.aspx?subjectid=" + Me.HSubjectId.Value.Trim()
            End If

            ' Me.Image1.ImageUrl = "frmPIFImage.aspx?subjectid=" + subjectId.ToString()
            '************************************
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnSubject_Click")
        End Try
    End Sub

    Protected Sub btnDivOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDivOK.Click
        Dim ds_WorkspaceSubjectMst As New DataSet
        Dim dt_WorkspaceSubjectmst As New DataTable
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty

        Try
            'ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
            wStr = "vWorkspaceSubjectId= '" & Me.ViewState(VS_WorkspaceSubjectId).ToString() & "'"
            If Not objHelp.GetWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                     ds_WorkspaceSubjectMst, eStr) Then
            End If

            For Each dr As DataRow In ds_WorkspaceSubjectMst.Tables(0).Rows
                dr("cStatusindi") = "D"
                dr("vRemarks") = Me.txtLockRemark.Text.Trim
                dr("iAsnNo") = Me.ViewState(VS_SrNo).ToString.Trim
                dr("iModifyBy") = Me.Session(S_UserID)
            Next
            ds_WorkspaceSubjectMst.Tables(0).AcceptChanges()

            If Not objLambda.Save_InsertWorkspaceSubjectMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
             WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_WorkspaceSubjectMst, ds_WorkspaceSubjectMst, Me.Session(S_UserID), eStr) Then

                ObjCommon.ShowAlert("Error while Saving WorkspaceSubjectMst", Me.Page)
                Exit Sub
            End If
            Me.ObjCommon.ShowAlert("Attendance of selected subject deleted successfully !", Me)
            'ResetPage()
            FillGridView()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnDivOK_Click")
        End Try
    End Sub

    Protected Sub btnAudit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAudit.Click
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_WorkspaceSubMstHistory As New DataSet
        Dim dc_AuditTrailMst As New DataColumn
        Dim ds As New DataSet
        Dim bf As New BoundField
        Dim dc As New DataColumn

        Try
            wStr = " vWorkspaceID = '" & Me.HProjectId.Value.Trim() & _
                  "' AND iPeriod = " & Me.ddlPeriod.SelectedValue.ToString() & _
                  " And DeleteIndi ='D' order by dModifyOn desc"

            If Not objHelp.View_WorkSpaceSubjectMstHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                           ds_WorkspaceSubMstHistory, eStr) Then
                Throw New Exception(eStr)
            End If

            ' Added on 18-03-2015 by Jeet Patel
            ' To show IST EST column in Audit Trail
            If ds_WorkspaceSubMstHistory.Tables(0).Rows.Count < 1 Then
                ObjCommon.ShowAlert("No Data Found", Me.Page)
                Exit Sub
            End If

            dc_AuditTrailMst = New DataColumn("dModifyOn_IST", System.Type.GetType("System.String"))
            ds_WorkspaceSubMstHistory.Tables(0).Columns.Add("dModifyOn_IST")
            ds_WorkspaceSubMstHistory.AcceptChanges()

            dc_AuditTrailMst = New DataColumn("ActualTIME", System.Type.GetType("System.String"))
            ds_WorkspaceSubMstHistory.Tables(0).Columns.Add(dc_AuditTrailMst)
            ds_WorkspaceSubMstHistory.AcceptChanges()


            For Each dr_Audit In ds_WorkspaceSubMstHistory.Tables(0).Rows
                dr_Audit("dModifyOn_IST") = (CDate(dr_Audit("dModifyOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)

                If Not objHelp.Proc_ActualAuditTrailTime(CDate(dr_Audit("dModifyOn")).ToString("dd-MMM-yyyy HH:mm") + "##" + Session(S_LocationCode), ds, eStr) Then
                    Throw New Exception(eStr)
                End If
                dr_Audit("ActualTIME") = CDate(ds.Tables(0).Rows(0).Item("ActualTIME")).ToString("dd-MMM-yyyy HH:mm") + " (" + ds.Tables(0).Rows(0).Item("TimeZoneOffSet").ToString + " GMT)"
            Next
            ds_WorkspaceSubMstHistory.AcceptChanges()

            If Me.GVAudit.Columns.Count > 11 Then
                Me.GVAudit.Columns.RemoveAt(GVAudit.Columns.Count - 1)
            End If

            dc = New DataColumn(ds.Tables(0).Columns("ActualTIME").ColumnName)

            bf.DataField = dc.ColumnName
            bf.HeaderText = Session(S_TimeZoneName)
            GVAudit.Columns.Add(bf)
            GVAudit.Columns(11).ItemStyle.Wrap = False
            ' End

            If ds_WorkspaceSubMstHistory.Tables(0).Rows.Count > 0 Then
                Me.GVAudit.DataSource = ds_WorkspaceSubMstHistory
                Me.GVAudit.DataBind()
                Me.MpeAudit.Show()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "GVAudit", "GVAudit(); ", True)
            ElseIf ds_WorkspaceSubMstHistory.Tables(0).Rows.Count <= 0 Then
                ObjCommon.ShowAlert("No Audit Trail Present", Me)
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...btnAudit_Click")
        End Try
    End Sub

    Protected Sub btnExPortToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
        Dim filename As String = String.Empty
        Dim isReportComplete As Boolean = False
        Dim dsWorkspaceSubjectMst As New DataSet
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty
        Try

            Wstr = "vWorkSpaceId='" & Me.HProjectId.Value.Trim() & "'  and iPeriod=" & _
                    IIf(Me.ddlPeriod.SelectedIndex = 0, "iPeriod", Me.ddlPeriod.SelectedValue.Trim()) '& " and cRejectionFlag <> 'Y'"
            If Not IsNothing(Me.Request.QueryString("PeriodId")) Then
                Wstr = "vWorkSpaceId='" & Me.HProjectId.Value.Trim() & _
                                   "' and iPeriod=" & Me.Request.QueryString("PeriodId").Trim() '& " and cRejectionFlag <> 'Y'"
            End If
            Wstr += " and cStatusindi <>'D' Order by cast(dReportingDate as datetimeoffset)"

            If Not objHelp.GetViewWorkspaceSubjectMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        dsWorkspaceSubjectMst, estr) Then
                Me.ObjCommon.ShowAlert("Error while getting Subjects data.", Me.Page)
                Exit Sub
            End If

            Me.gvForExcel.DataSource = dsWorkspaceSubjectMst
            Me.gvForExcel.DataBind()

            If Me.Request.QueryString("mode") = 4 Then
                gvForExcel.Columns(GVCell_Subject).Visible = False
            End If

            If gvForExcel.Rows.Count > 0 Then
                Dim info As String = String.Empty
                Dim gridviewHtml As String = String.Empty

                filename = "Attendance Report For " + Me.txtproject.Text.Trim() + "_" + Date.Today.ToString("dd-MMM-yyyy") + ".xls"
                isReportComplete = True

                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)

                gvForExcel.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()
                gridviewHtml = "<table><tr><td align = ""center"" colspan=""12""><b>" + System.Configuration.ConfigurationManager.AppSettings("Client").ToString() + "<br/></b></td></tr></table><table><tr><td align = ""center"" colspan=""2""><b>" + "Attendance Report For " + Me.txtproject.Text.Trim() + "<br/></b></td></tr></table><span>Print Date:-  " + Date.Today.ToString("dd-MMM-yyyy") + "</span>" + gridviewHtml

                Context.Response.Buffer = True
                Context.Response.ClearContent()
                Context.Response.ClearHeaders()

                Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
                Context.Response.AddHeader("content-Disposition", "attachment; filename=" + filename)
                Context.Response.AddHeader("Content-Length", gridviewHtml.Length)

                Context.Response.Write(gridviewHtml)
                Context.Response.Flush()
                Context.Response.End()

                System.IO.File.Delete(filename)
            Else
                Exit Sub
            End If

        Catch Threadex As Threading.ThreadAbortException
        Catch ex As Exception
            isReportComplete = False
        Finally
            If Not rpage Is Nothing Then
                rpage.CloseReport()
                rpage = Nothing
            End If
        End Try
    End Sub

    Protected Sub btnExporttoPdf_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExporttoPdf.Click
        Dim wstr = "", eStr As String = ""
        Dim ds_check As New DataSet
        Dim ds_MedexScreeningHdrDtl As New DataSet
        Dim ds_project As New DataSet
        Dim strpdf As String = ""
        Dim ds_exportpdf As New DataTable
        Dim downloadbytes As Byte()
        Dim pdfFont As System.Drawing.Font
        Dim d1 As Document
        Dim headercontent As String = String.Empty
        Dim foldername As String = ""
        Dim DocumentsArray() As String
        Dim fInfo As FileInfo = Nothing
        Try
            wstr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D' Order by iTranNo Desc"

            If Not Me.objHelp.GetCRFLockDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_check, eStr) Then
                Throw New Exception(eStr)
            End If
            If ds_check Is Nothing Or ds_check.Tables.Count = 0 Or ds_check.Tables(0) Is Nothing Then
                ObjCommon.ShowAlert("No Project lock detail", Me.Page)
                Exit Sub
            End If
            If ds_check.Tables(0).Rows.Count > 0 Then
                If ds_check.Tables(0).Rows(0)("cLockFlag") = "U" Then
                    ObjCommon.ShowAlert("Project is not Locked. You can not Print the Report.", Me.Page)
                    Exit Sub
                End If
            Else
                ObjCommon.ShowAlert("Project is not Locked. You can not Print the Report.", Me.Page)
                Exit Sub
            End If
            wstr = "vWorkspaceID = '" + Me.HProjectId.Value.Trim() + "' and iperiod = " + Convert.ToString(Me.ddlPeriod.SelectedValue.ToString()) + _
                                " Order by cast(dReportingDate as datetimeoffset) "
            If Not objHelp.view_RptSubAttendance(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                     ds_MedexScreeningHdrDtl, eStr) Then

                Throw New Exception(eStr)
            End If

            If ds_MedexScreeningHdrDtl Is Nothing Or ds_MedexScreeningHdrDtl.Tables.Count = 0 Or ds_MedexScreeningHdrDtl.Tables(0) Is Nothing Or ds_MedexScreeningHdrDtl.Tables(0).Rows.Count = 0 Then
                ObjCommon.ShowAlert("No Data found.", Me.Page)
                Exit Sub
            End If

            If ds_MedexScreeningHdrDtl.Tables(0).Rows.Count > 0 Then

                ds_exportpdf = ds_MedexScreeningHdrDtl.Tables(0).DefaultView.ToTable(False, "vSubjectId,vInitials,vMySubjectNo,dReportingDate,FullName,dModifyOn,dScreenDate,vReasonDesc".Split(","))
                Dim dc As DataColumn = ds_exportpdf.Columns.Add("Seq No.", System.Type.GetType("System.Int32"))
                dc.SetOrdinal(0)
                ds_exportpdf.Columns("vSubjectId").ColumnName = "Subject Id"
                ds_exportpdf.Columns("vInitials").ColumnName = "Initials"
                ds_exportpdf.Columns("vMySubjectNo").ColumnName = "My SubNo"
                ds_exportpdf.Columns("dReportingDate").ColumnName = "Reporting Date"
                ds_exportpdf.Columns("dModifyOn").ColumnName = "Recorded On"
                ds_exportpdf.Columns("dScreenDate").ColumnName = "Screen Date"
                ds_exportpdf.Columns("vReasonDesc").ColumnName = "Rejection Reason"
                ds_exportpdf.AcceptChanges()

                strpdf = "<Table style=""width:100%;border-collapse:collapse;margin:auto; border: 1px Solid Black;"" >"
                strpdf += "<tr style=""color:Black;background-color:White;font-family:'Times New Roman' !important; text-align: center; font-size:12px;"">"
                For i As Integer = 0 To ds_exportpdf.Columns.Count - 1
                    strpdf += "<td style=""border: 1px Solid Black;"">"
                    strpdf += ds_exportpdf.Columns(i).ToString
                    strpdf += "</td>"
                Next
                strpdf += "</tr>"
                For i As Integer = 0 To ds_exportpdf.Rows.Count - 1
                    strpdf += "<tr style=""color:Black;background-color:White;font-family:'Times New Roman' !important; text-align: center; font-size:12px; page-break-inside:avoid;"">"
                    For j As Integer = 0 To ds_exportpdf.Columns.Count - 1
                        strpdf += "<td style=""border: 1px Solid Black;"">"
                        If j = 0 Then
                            strpdf += Convert.ToString(i + 1)
                        Else
                            strpdf += ds_exportpdf.Rows(i)(j).ToString()
                        End If

                        strpdf += "</td>"
                    Next
                    strpdf += "</tr>"
                    strpdf += "<tr style=""color:Black;background-color:White;font-family:'Times New Roman' !important;  font-size:12px; page-break-inside:avoid;"">"
                    strpdf += "</tr>"

                Next
                strpdf += "</Table>"

                Dim pdfconverter As HtmlToPdfConverter = New HtmlToPdfConverter()
                pdfconverter.LicenseKey = "dfvo+uv66OPj6vrr6PTq+unr9Ovo9OPj4+P66g=="
                pdfconverter.PdfDocumentOptions.AvoidImageBreak = True
                pdfconverter.PdfDocumentOptions.AvoidTextBreak = False
                pdfconverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4
                pdfconverter.PdfDocumentOptions.EmbedFonts = True
                pdfconverter.PdfDocumentOptions.JpegCompressionEnabled = True
                pdfconverter.PdfDocumentOptions.JpegCompressionLevel = 0
                pdfconverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Best
                pdfconverter.PdfDocumentOptions.LeftMargin = 72
                pdfconverter.PdfDocumentOptions.RightMargin = 27
                pdfconverter.PdfDocumentOptions.TopMargin = 27
                pdfconverter.PdfDocumentOptions.BottomMargin = 27
                pdfconverter.HtmlViewerWidth = 647     ''Previous 750
                pdfconverter.PdfDocumentOptions.FitWidth = False
                pdfconverter.PdfDocumentOptions.StretchToFit = True
                pdfconverter.PdfBookmarkOptions.AllowDefaultTitle = True
                pdfconverter.PdfBookmarkOptions.AutoBookmarksEnabled = True

                '-----------------------------------------Header---------------------------------------------------
                Dim Path As String = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.ToString.LastIndexOf("/"))
                Path = Path + "/images/lambda_logo.jpg"
                headercontent += "<table  id=""tbldivheader"" style=""padding: 2px; margin: auto; border: solid 1px black; width: 100%; font-family: 'Times New Roman' !Important; font-size:12px;"" align=""left"">"
                headercontent += "<tr>"
                headercontent += "<td>"
                headercontent += "<table>"
                headercontent += "<tr>"
                headercontent += "<td colspan=""3"" style=""vertical-align: top;"">"
                headercontent += "<table>"
                headercontent += "<tr>"
                headercontent += "<td style=""vertical-align: top; font-size: 12px; font-family: Times New Roman !Important;"">"
                headercontent += "<Label  for """ + ConfigurationManager.AppSettings.Item("Client").ToString() + """>" + ConfigurationManager.AppSettings.Item("Client").ToString() + "</asp:Label>"
                headercontent += "</td>"
                headercontent += "</tr>"
                headercontent += "<tr>"
                headercontent += "</tr>"
                headercontent += "<tr>"
                headercontent += "<td style=""vertical-align: bottom; font-size: 12px; font-weight:bold !important; font-family: Times New Roman !Important; text-align: right"">"
                headercontent += "Department: Clinical Pharmacology And Medical Affairs"
                headercontent += "</td>"
                headercontent += "</tr>"
                headercontent += "</table>"
                headercontent += "</td>"
                headercontent += "</tr>"
                headercontent += "<tr>"
                headercontent += "<td style=""font-family: Times New Roman !Important; font-size:12px;"">Project No:"
                headercontent += ds_MedexScreeningHdrDtl.Tables(0).Rows(0)("vProjectNo")
                headercontent += "</td>"
                headercontent += "<td style=""font-family: Times New Roman !Important; font-size:12px; font-weight:bold !important; "">Attendance</td>"
                headercontent += "<td style=""font-family: Times New Roman !Important; font-size:12px;"">Period:"
                headercontent += Convert.ToString(Me.ddlPeriod.SelectedValue.ToString())
                headercontent += "</td>"
                headercontent += "</tr>"
                headercontent += "</table>"
                headercontent += "</td>"
                headercontent += "<td valign=""top"">"
                headercontent += "<img id=ctl00_CPHLAMBDA_ImgLogo alt=""" + Path.ToString() + """ src=""" + Path.ToString() + """right"" alt=""lambda""/>"
                headercontent += "</td>"
                headercontent += "</tr>"
                headercontent += "</table>"
                '--------------------------------------------------------------------------------------------------

                pdfFont = New System.Drawing.Font("Times New Roman", 12, FontStyle.Bold, GraphicsUnit.Point)

                pdfconverter.PdfDocumentOptions.ShowHeader = True
                pdfconverter.PdfFooterOptions.AddElement(New LineElement(0, 0, pdfconverter.PdfDocumentOptions.PdfPageSize.Width - pdfconverter.PdfDocumentOptions.LeftMargin - pdfconverter.PdfDocumentOptions.RightMargin, 0))
                pdfconverter.PdfDocumentOptions.ShowFooter = True
                pdfconverter.PdfFooterOptions.FooterHeight = 1
                pdfconverter.PdfHeaderOptions.HeaderHeight = 110
                Dim Header1 As New HtmlToPdfElement(headercontent, String.Empty)
                Header1.HtmlViewerWidth = 647     ''Previous 750
                Header1.FitWidth = True

                pdfconverter.PdfHeaderOptions.AddElement(Header1)

                foldername = DateTime.Now.ToString("ddMMyyyyhhmmss")
                If Not Directory.Exists(Server.MapPath(ConfigurationManager.AppSettings.Item("uploadfilepath").Trim() + "\Attendancereport\" + Me.HProjectId.Value.ToString() + "\" + foldername + "\" + Convert.ToString(Me.ddlPeriod.SelectedValue.ToString()) + "\FinalReport")) Then
                    Directory.CreateDirectory(Server.MapPath(ConfigurationManager.AppSettings.Item("uploadfilepath").Trim() + "\Attendancereport\" + Me.HProjectId.Value.ToString() + "\" + foldername + "\" + Convert.ToString(Me.ddlPeriod.SelectedValue.ToString()) + "\FinalReport"))
                End If

                d1 = pdfconverter.ConvertHtmlToPdfDocumentObject(strpdf, String.Empty)
                d1.Save(Server.MapPath(ConfigurationManager.AppSettings.Item("uploadfilepath").Trim() + "\Attendancereport\" + Me.HProjectId.Value.ToString() + "\" + foldername + "\" + Convert.ToString(Me.ddlPeriod.SelectedValue.ToString()) + "\FinalReport\02.pdf"))
                d1.AutoCloseAppendedDocs = True
                d1.Close()
                Dim outStream As MemoryStream = New MemoryStream()
                Dim reader As iTextSharp.text.pdf.PdfReader = New iTextSharp.text.pdf.PdfReader(Server.MapPath(ConfigurationManager.AppSettings.Item("uploadfilepath").Trim()) + "\Attendancereport\" + Me.HProjectId.Value.ToString() + "\" + foldername + "\" + Convert.ToString(Me.ddlPeriod.SelectedValue.ToString()) + "\FinalReport\02.pdf")
                Dim stamper As iTextSharp.text.pdf.PdfStamper = New iTextSharp.text.pdf.PdfStamper(reader, outStream)
                reader.ViewerPreferences = iTextSharp.text.pdf.PdfWriter.PageModeUseOutlines
                stamper.Close()
                reader.Close()
                downloadbytes = outStream.ToArray()

                'downloadbytes = d1.Save()

                Dim response As System.Web.HttpResponse = System.Web.HttpContext.Current.Response
                response.Clear()
                response.ContentType = "application/pdf"
                response.AddHeader("content-disposition", "attachment; filename=" + ds_MedexScreeningHdrDtl.Tables(0).Rows(0)("vProjectNo") + "Period:" + Convert.ToString(Me.ddlPeriod.SelectedValue.ToString()) + ".pdf;" + " size=" & downloadbytes.Length.ToString())
                response.Flush()
                response.BinaryWrite(downloadbytes)
                response.Flush()
                response.End()
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...btnExporttoPdf_Click")
        Finally
            If Directory.Exists(Server.MapPath(ConfigurationManager.AppSettings.Item("uploadfilepath").Trim()) + "\Attendancereport\" + Me.HProjectId.Value.ToString()) Then
                If Directory.Exists(Server.MapPath(ConfigurationManager.AppSettings.Item("uploadfilepath").Trim()) + "\Attendancereport\" + Me.HProjectId.Value.ToString() + "\" + foldername + "\" + Convert.ToString(Me.ddlPeriod.SelectedValue.ToString()) + "\FinalReport") Then
                    DocumentsArray = System.IO.Directory.GetFiles(Server.MapPath(ConfigurationManager.AppSettings.Item("uploadfilepath").Trim()) + "\Attendancereport\" + Me.HProjectId.Value.ToString() + "\" + foldername + "\" + Convert.ToString(Me.ddlPeriod.SelectedValue.ToString()) + "\FinalReport")
                    If DocumentsArray.Length > 0 Then
                        For i = 0 To DocumentsArray.Length - 1
                            fInfo = New FileInfo(DocumentsArray(i))
                            If fInfo.Exists Then
                                fInfo.Delete()
                            End If
                        Next i
                    End If
                End If
            End If
        End Try
    End Sub

#End Region

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub

#Region "Grid Event"

#Region "gvwWorkspaceSubjectMst"

    Protected Sub gvwWorkspaceSubjectMst_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        gvwWorkspaceSubjectMst.PageIndex = e.NewPageIndex
        FillGridView()
    End Sub

    Protected Sub gvwWorkspaceSubjectMst_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim index As Integer = e.CommandArgument
        Dim RedirectStr As String = String.Empty
        Dim ds_ScrDt As New DataSet
        'Dim Wstr As String = String.Empty
        'Dim ds_SubDetail As New DataSet

        Try
            If e.CommandName.ToUpper = "EDIT" Then
                Me.Response.Redirect("frmWorkspaceSubjectMst.aspx?mode=2&workspaceid=" & Me.Request.QueryString("workspaceid").Trim() & "&value=" & Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVC_Code).Text.Trim())
            ElseIf e.CommandName.ToUpper = "REJECT" Then

                ViewState(GVC_Code) = Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVC_Code).Text.Trim()

                FillRejectDropDown()

                Me.lblSubjectName.Text = Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVCell_Subject).Text.Trim()
                Me.lblSubjectName.Font.Bold = True

                'Me.trAddRejectSubject.Visible = True
                MPEReject.Show()

                'divRejectSubject.Attributes.Add("style", "display:block")
                'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "DisplayDiv", "SetCenter('" & Me.divRejectSubject.ClientID & "');", True)

            ElseIf e.CommandName.ToUpper = "VIEW" Then
                '------------ Nidhi -------------------
                'Wstr = "vSubjectId='" & Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVCell_SubjectId).Text.Trim() & "'"
                'If Not Me.objHelp.GetView_SubjectMaster(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                '                                                    ds_SubDetail, eStr_Retu) Then
                '    Exit Sub
                'End If
                '-------------------------------------------
                'If ds_SubDetail.Tables(0).Rows(0)("vLocationcode").ToString = Location_Canada Then
                Dim dsWorkspaceSubjectMst As New DataSet
                Dim estr As String = String.Empty
                Dim Wstr As String = String.Empty
                Dim WorkspaceId As String = String.Empty
                WorkspaceId = Me.HProjectId.Value.Trim()
                Dim ds_Temp As New DataSet

                ' Condition Added by Rahul as Issue repoted  Issue no 12063
                Wstr = " vWorkspaceID = '" & Me.HProjectId.Value.Trim() & "' AND  cStatusIndi <> 'D' AND vSubjectID = '" _
                & Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVCell_SubjectId).Text.ToString.Trim() & "'"


                If Not objHelp.GetViewWorkspaceSubjectMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                      dsWorkspaceSubjectMst, estr) Then
                    Me.ObjCommon.ShowAlert("Error while getting Subjects data.", Me.Page)
                    Exit Sub
                End If

                If Not objHelp.GetMedExWorkSpaceScreeningHdr(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                        ds_Temp, estr) Then
                    Me.ObjCommon.ShowAlert("Error while getting Subjects data.", Me.Page)
                    Exit Sub
                End If

                Dim Wstr12 As String = String.Empty
                Dim ds_SubMedex As DataSet = Nothing

                Wstr12 = "vSubjectID = '" & Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVCell_SubjectId).Text.ToString.Trim() & "'"
                If Not objHelp.GetMedExWorkSpaceScreeningHdr(Wstr12, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubMedex, eStr_Retu) Then
                    Exit Sub
                End If


                Dim WorkSpaceId1 As String = String.Empty
                Dim dscreeningDate As String = String.Empty
                ds_Temp.Tables(0).DefaultView.Sort = "dModifyOn desc"
                ds_Temp.Tables(0).AcceptChanges()


                'Dim dt As DataTable = ds_Temp.Tables(0)
                Dim dv As DataView = ds_Temp.Tables(0).DefaultView
                dv.Sort = "dModifyOn desc"

                Dim nMedexScreeningHdrNo As String = "0"
                Dim dtMedesscreening = ds_SubMedex.Tables(0)
                dtMedesscreening.DefaultView.Sort = "dScreenDate desc"
                dtMedesscreening.AcceptChanges()
                Dim dvMed As DataView = dtMedesscreening.DefaultView
                dvMed.Sort = "dScreenDate desc "

                Dim latestscreeningdate As String = String.Empty


                Dim dtWorkspaceSubjectMst As DataTable = dsWorkspaceSubjectMst.Tables(0)

                For Each dr As DataRow In dtWorkspaceSubjectMst.Rows
                    nMedexScreeningHdrNo = dr("nMedExScreeningHdrNo").ToString()
                    Exit For
                Next

                If (dtMedesscreening.Rows.Count > 0) Then
                    latestscreeningdate = "0"
                    dvMed.RowFilter = "vWorkSpaceId = '" + Me.HProjectId.Value + "' "
                    If (dvMed.ToTable.Rows.Count > 0) Then
                        If nMedexScreeningHdrNo <> "0" And dvMed.ToTable.Rows.Count > 1 Then
                            dvMed.RowFilter = "nMedexScreeningHdrNo = '" + nMedexScreeningHdrNo + "' "
                        End If
                        latestscreeningdate = dvMed.ToTable.Rows(0)(2).ToString
                        nMedexScreeningHdrNo = dvMed.ToTable.Rows(0)(1).ToString
                    End If
                End If

                If (dsWorkspaceSubjectMst.Tables(0).Rows.Count > 0) Then
                    For Each dr As DataRow In dsWorkspaceSubjectMst.Tables(0).Rows
                        dscreeningDate = dr("dScreenDate").ToString
                        Exit For
                    Next
                End If

                If (dv.ToTable.Rows.Count > 0) Then
                    WorkSpaceId1 = dv.ToTable.Rows(0)(3).ToString
                    ' dscreeningDate = dv.ToTable.Rows(0)(2).ToString
                End If






                'Dim ds_MaxScreeningLockDtl As DataSet = Nothing
                'Dim WstrScreenignLock As String = "vWorkspaceid= '" + WorkspaceId + " ' AND VsubjectId ='" + Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVCell_SubjectId).Text.Trim() + "'  and cStatusIndi<>'D'  "
                'If Not objHelp.View_MaxScreeningLockDtl(WstrScreenignLock, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                '                                 ds_MaxScreeningLockDtl, estr) Then

                '    Throw New Exception(estr)
                'End If
                'Dim Lock As String = "U"
                'If ds_MaxScreeningLockDtl.Tables(0).Rows.Count > 0 Then
                '    Lock = ds_MaxScreeningLockDtl.Tables(0).Rows(0)(7).ToString
                'End If


                'If WorkSpaceId1 <> String.Empty Then
                '    RedirectStr = "window.open(""" & "frmSubjectScreening_New.aspx?mode=4&Workspace=" + WorkSpaceId1 + "&SubId=" & _
                '                Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVCell_SubjectId).Text.Trim() & "&ScrDt=" & dscreeningDate & "&Lock=" & Lock.ToString & "&Attendance=true"")"
                '    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)

                'Else
                If nMedexScreeningHdrNo <> "0" AndAlso nMedexScreeningHdrNo <> "&nbsp;" Then
                    'If Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVCell_ScrHDr).Text.ToString.Trim() <> "0" AndAlso Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVCell_ScrHDr).Text.ToString.Trim() <> "&nbsp;" Then
                    '' ds_ScrDt = objHelp.GetResultSet("SELECT * FROM MedexScreeningHdr where nMEdexScreeningHdrNo = " + Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVCell_ScrHDr).Text.ToString.Trim() + "", "MedexScreeningHdr")

                    If dscreeningDate <> String.Empty AndAlso (latestscreeningdate <> "") Then
                        If (latestscreeningdate = "0") Then

                            RedirectStr = "window.open(""" & "frmSubjectScreening_New.aspx?mode=4&Workspace=" + WorkspaceId + "&SubId=" & _
                                        Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVCell_SubjectId).Text.Trim() & "&ScrDt=" & dscreeningDate & "&Attendance=true &Group="")"
                            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)
                        Else
                            If (latestscreeningdate <> "0" AndAlso latestscreeningdate <> String.Empty) Then
                                RedirectStr = "window.open(""" & "frmSubjectScreening_New.aspx?mode=4&Workspace=" + WorkspaceId + "&SubId=" & _
                                        Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVCell_SubjectId).Text.Trim() & "&ScrDt=" & latestscreeningdate & "&Attendance=true &Group="")"
                                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)
                            End If
                            RedirectStr = "window.open(""" & "frmSubjectScreening_New.aspx?mode=4&Workspace=" + WorkspaceId + "&SubId=" & _
                                        Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVCell_SubjectId).Text.Trim() & "&ScrDt=" & dscreeningDate & "&Attendance=true &Group="")"
                            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)
                        End If
                    Else
                        If latestscreeningdate <> "0" And latestscreeningdate <> "" Then
                            RedirectStr = "window.open(""" & "frmSubjectScreening_New.aspx?mode=4&Workspace=" + WorkspaceId + "&SubId=" & _
                                        Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVCell_SubjectId).Text.Trim() & "&ScrDt=" & latestscreeningdate & "&Attendance=true &Group="")"
                            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)
                        Else
                            If dscreeningDate <> String.Empty Then
                                RedirectStr = "window.open(""" & "frmSubjectScreening_New.aspx?mode=4&Workspace=" + WorkspaceId + "&SubId=" & _
                                        Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVCell_SubjectId).Text.Trim() & "&ScrDt=" & dscreeningDate & "&Attendance=true&Group="")"
                                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)
                            Else

                                RedirectStr = "window.open(""" & "frmSubjectScreening_New.aspx?mode=4&Workspace=" + WorkspaceId + "&SubId=" & _
                                            Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVCell_SubjectId).Text.Trim() & "&ScrDt=&Attendance=true&Group="")"
                                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)
                            End If
                        End If
                    End If
                ElseIf Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVCell_ScrHDr).Text.ToString.Trim() = "0" Then
                    If latestscreeningdate = "0" Then
                        If latestscreeningdate = "" Then
                            RedirectStr = "window.open(""" & "frmSubjectScreening_New.aspx?mode=4&Workspace=" + WorkspaceId + "&SubId=" & _
                                   Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVCell_SubjectId).Text.Trim() & "&Attendance=true"")"
                            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)
                        Else
                            RedirectStr = "window.open(""" & "frmSubjectScreening_New.aspx?mode=4&Workspace=" + WorkspaceId + "&SubId=" & _
                                    Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVCell_SubjectId).Text.Trim() & "&Attendance=true&Project=true&Group="")"
                            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)
                        End If

                    Else
                        RedirectStr = "window.open(""" & "frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&SubId=" & _
                                    Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVCell_SubjectId).Text.Trim() & "&Attendance=true"")"
                        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)

                    End If


                Else
                    ObjCommon.ShowAlert("No Screening Date Found/Subject is not assigned", Me.Page)
                    Exit Sub
                End If

                'Else
                ' RedirectStr = "window.open(""" & "frmSubjectScreeningNew.aspx?mode=4&Workspace=0000000000&SubjectId=" & _
                '                 Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVCell_SubjectId).Text.Trim() & """)"
                ' End If


            ElseIf e.CommandName.ToUpper = "DELETE" Then
                '01-Feb-11
                If Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVCell_iMysubNo).Text.Trim() <> 0 Then
                    ObjCommon.ShowAlert("Subject is Assigned in Project.You cannot delete Attendance of selected subject", Me)
                    Exit Sub
                End If
                '===
                Me.ViewState(VS_WorkspaceSubjectId) = Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVC_Code).Text.Trim()
                Me.ViewState(VS_SrNo) = Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVC_SrNo).Text.Trim()
                Me.mpeDialog.Show()

            ElseIf e.CommandName.ToUpper = "AUDIT" Then
                GVAudit.DataSource = Nothing
                GVAudit.DataBind()

                ViewState(VS_iMySubjectNo) = Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVCell_iMysubNo).Text.Trim()
                ViewState(VS_WorkspaceSubjectId) = Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVC_Code).Text.Trim()
                If Not ShowAudit() Then
                    Exit Sub
                End If
                Me.MPEAuditTrail.Show()
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...gvwWorkspaceSubjectMst_RowCommand")
        End Try
    End Sub

    Protected Sub gvwWorkspaceSubjectMst_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Try
            If e.Row.RowType = DataControlRowType.DataRow Or _
                          e.Row.RowType = DataControlRowType.Header Or _
                          e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(GVC_Code).Visible = False
                e.Row.Cells(GVCell_Edit).Visible = False
                e.Row.Cells(GVCell_cRejected).Visible = False
                e.Row.Cells(GVCell_iMysubNo).Visible = False
                e.Row.Cells(GVCell_ScrHDr).Style.Add("display", "none")

                If ViewState(VS_Choice1) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    e.Row.Cells(GVCell_Rejected).Visible = False
                    e.Row.Cells(GVCell_Delete).Visible = False
                End If

                If Me.Request.QueryString("mode") = 4 Then
                    e.Row.Cells(GVCell_Subject).Visible = False
                End If

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...gvwWorkspaceSubjectMst_RowCreated")
        End Try
    End Sub

    Protected Sub gvwWorkspaceSubjectMst_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Dim img As HtmlImage = Nothing
        Dim dv As HtmlControl = Nothing

        Try
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                If (Not Me.Request.QueryString("WorkSpaceId") Is Nothing) AndAlso (Not Me.Request.QueryString("Type") Is Nothing) Then
                    If e.Row.RowType <> DataControlRowType.Pager Then
                        e.Row.Cells(GVC_View).Visible = False
                    End If
                End If
            End If
            If e.Row.RowType = DataControlRowType.DataRow Then

                e.Row.Cells(GVC_SrNo).Text = (Me.gvwWorkspaceSubjectMst.PageIndex * Me.gvwWorkspaceSubjectMst.PageSize) + e.Row.RowIndex + 1

                'CType(e.Row.FindControl("imgShow"), ImageButton).Style
                e.Row.Cells(GVCell_Reason).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells(GVCell_Reason).VerticalAlign = HorizontalAlign.Center

                CType(e.Row.FindControl("lnkEdit"), LinkButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkEdit"), LinkButton).CommandName = "EDIT"

                CType(e.Row.FindControl("lnkReject"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkReject"), ImageButton).CommandName = "REJECT"
                e.Row.Cells(GVCell_Rejected).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells(GVCell_Rejected).VerticalAlign = HorizontalAlign.Center
                ' CType(e.Row.FindControl("lnkReject"), LinkButton).Attributes("OnClick") = "return CheckToReject();"

                CType(e.Row.FindControl("lnkView"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkView"), ImageButton).CommandName = "VIEW"
                e.Row.Cells(GVC_View).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells(GVC_View).VerticalAlign = HorizontalAlign.Center

                CType(e.Row.FindControl("ImgCancel"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgCancel"), ImageButton).CommandName = "DELETE"

                If e.Row.Cells(GVCell_cRejected).Text.ToUpper.Trim() = "Y" Then
                    e.Row.BackColor = Drawing.Color.Red
                    'e.Row.Enabled = False
                    CType(e.Row.FindControl("ImgCancel"), ImageButton).Visible = False
                    CType(e.Row.FindControl("lnkReject"), ImageButton).Visible = False
                    img = CType(e.Row.FindControl("imgShow"), HtmlImage)
                    dv = CType(e.Row.FindControl("canal"), HtmlControl)
                    If Not img Is Nothing AndAlso Not dv Is Nothing Then
                        img.Attributes.Add("onclick", "$('#" + dv.ClientID + "').toggle('medium');")
                    End If
                Else
                    CType(e.Row.FindControl("imgShow"), HtmlImage).Style.Add("display", "none")
                End If
                CType(e.Row.FindControl("ImgAudit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgAudit"), ImageButton).CommandName = "AUDIT"
                e.Row.Cells(GVCell_Audit).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells(GVCell_Audit).VerticalAlign = HorizontalAlign.Center

                'reportingdate = e.Row.Cells(GVCell_ReportingDateTime).Text.ToString

                'e.Row.Cells(GVCell_ReportingDateTime).Text = reportingdate.ToString + strServerOffset 'Added By Parth Pandya for change in date and time
                'e.Row.Cells(GVCell_ReportingDateTime).Text = CDate(reportingdate).ToString("dd-MMM-yyyy hh:mm:ss tt") + strServerOffset 'Added By Parth Pandya for Change in time format hh:mm:ss for old data
                'replaceoffset = reportingdate.Substring(reportingdate.Length - 7, 7).ToString.Trim()
                'e.Row.Cells(GVCell_ReportingDateTime).Text = CDate(Replace(reportingdate, replaceoffset, "")).ToString("dd-MMM-yyyy HH:mm").Trim() + strServerOffset ' HH:mm") '& " (" & reportingdate.Substring(reportingdate.Length - 7, 7).ToString.Trim() + " GMT)"
                reportingdate = Replace(e.Row.Cells(GVCell_ReportingDateTime).Text.Trim(), "&nbsp;", "")
                replaceoffset = reportingdate.Substring(reportingdate.Length - 6, 6).ToString.Trim()
                e.Row.Cells(GVCell_ReportingDateTime).Text = CDate(Replace(reportingdate, replaceoffset, "")).ToString("dd-MMM-yyyy HH:mm") & " (" & reportingdate.Substring(reportingdate.Length - 7, 7).ToString.Trim() & " GMT)"
            End If
            'If Me.ViewState(VS_Mode) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
            '    e.Row.Cells(GVC_Code).Visible = False
            '    'CType(e.Row.FindControl("ImgCancel"), ImageButton).Visible = False
            'End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...gvwWorkspaceSubjectMst_RowDataBound")
        End Try
    End Sub

    Protected Sub gvwWorkspaceSubjectMst_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvwWorkspaceSubjectMst.RowDeleting

    End Sub

    Protected Sub gvForExcel_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvForExcel.RowDataBound
        Dim img As HtmlImage = Nothing
        Dim dv As HtmlControl = Nothing

        Try
            If e.Row.RowType = DataControlRowType.DataRow Then

                e.Row.Cells(ASN_forExcel).Text = e.Row.RowIndex + 1

                If e.Row.Cells(cRejectionFlag_forExcel).Text.ToUpper.Trim() = "Y" Then
                    e.Row.BackColor = Drawing.Color.Red
                End If

                reportingdate = e.Row.Cells(dReportingDate_forExcel).Text.ToString
                replaceoffset = reportingdate.Substring(reportingdate.Length - 7, 7).ToString.Trim()
                e.Row.Cells(dReportingDate_forExcel).Text = CDate(Replace(reportingdate, replaceoffset, "")).ToString("dd-MMM-yyyy HH:mm") ' HH:mm") & " (" & reportingdate.Substring(reportingdate.Length - 7, 7).ToString.Trim() + " GMT)"
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...gvForExcel_RowDataBound")
        End Try
    End Sub


#End Region

#Region "GVAudit"

    Protected Sub GVAudit_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVAudit.RowDataBound

        Try
            Dim ds As New DataSet
            Dim eStr As String = String.Empty

            If e.Row.RowType = DataControlRowType.DataRow Or _
                      e.Row.RowType = DataControlRowType.Header Or _
                      e.Row.RowType = DataControlRowType.Footer Then

                e.Row.Cells(GVCAudit_iTranN).Visible = False
                e.Row.Cells(GVCAudit_nWorkspaceSubjectHistoryId).Visible = False
                e.Row.Cells(GVCAudit_vWorkspaceSubjectId).Visible = False

                If e.Row.RowType = DataControlRowType.DataRow Then

                    If Not Convert.ToString(e.Row.Cells(GVCAudit_dReportingDate).Text).Trim = "" Then
                        'reportingdate = Replace(e.Row.Cells(GVCAudit_dReportingDate).Text.Trim(), "&nbsp;", "")
                        'replaceoffset = reportingdate.Substring(reportingdate.Length - 7, 7).ToString.Trim()
                        'e.Row.Cells(GVCAudit_dReportingDate).Text = CDate(Replace(reportingdate, replaceoffset, "")).ToString("dd-MMM-yyyy") ' HH:mm") '& " (" & reportingdate.Substring(reportingdate.Length - 7, 7) + " GMT)"

                        '=======================  Added By Jeet Patel on 22-April-2015 to show canada time for canada subject in Delete Audit Trail ===================================
                        If (e.Row.Cells(GVCAudit_vSubjectID).Text Like "CA*") Then
                            If Not objHelp.Proc_ActualAuditTrailTime(CType(Replace(e.Row.Cells(GVCAudit_dReportingDate).Text.Trim(), "&nbsp;", ""), Date).ToString("dd-MMM-yyyy HH:mm").Trim() + "##" + "0042", ds, eStr) Then  '0042 is for passing Location code of canada
                                Throw New Exception(eStr)
                            End If
                            e.Row.Cells(GVCAudit_dReportingDate).Text = CDate(ds.Tables(0).Rows(0).Item("ActualTIME")).ToString("dd-MMM-yyyy HH:mm") + " EST (" + ds.Tables(0).Rows(0).Item("TimeZoneOffSet").ToString + " GMT)"
                        Else
                            e.Row.Cells(GVCAudit_dReportingDate).Text = CType(Replace(e.Row.Cells(GVCAudit_dReportingDate).Text.Trim(), "&nbsp;", ""), Date).ToString("dd-MMM-yyyy HH:mm").Trim() + strServerOffset
                        End If
                        '===============================================================================================================================================================
                    End If

                    If Not Convert.ToString(e.Row.Cells(GVCAudit_dModifyOn).Text).Trim = "" Then
                        e.Row.Cells(GVCAudit_dModifyOn).Text = CType(Replace(e.Row.Cells(GVCAudit_dModifyOn).Text.Trim(), "&nbsp;", ""), Date).ToString("dd-MMM-yyyy HH:mm").Trim() + strServerOffset
                    End If
                End If
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GVAudit_RowDataBound")
        End Try
    End Sub
#End Region

#Region "GVAuditTrail"

    Protected Sub GVAuditTrail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVAuditTrail.RowDataBound

        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                'e.Row.Cells(GVCAuditTrail_dModifyOn).Text = Replace(e.Row.Cells(GVCAuditTrail_dModifyOn).Text.Trim(), "&nbsp;", "")

                If Not Convert.ToString(e.Row.Cells(GVCAuditTrail_dReportingDate).Text).Trim = "" Then
                    reportingdate = Replace(e.Row.Cells(GVCAuditTrail_dReportingDate).Text.Trim(), "&nbsp;", "")
                    replaceoffset = reportingdate.Substring(reportingdate.Length - 7, 7).ToString.Trim()
                    e.Row.Cells(GVCAuditTrail_dReportingDate).Text = CDate(Replace(reportingdate, replaceoffset, "")).ToString("dd-MMM-yyyy HH:mm") & " (" & reportingdate.Substring(reportingdate.Length - 7, 7).ToString.Trim() & " GMT)"
                End If

                If Not Convert.ToString(e.Row.Cells(GVCAuditTrail_dModifyOn).Text).Trim = "" Then
                    'e.Row.Cells(GVCAuditTrail_dModifyOn).Text = e.Row.Cells(GVCAuditTrail_dModifyOn).Text + strServerOffset
                    e.Row.Cells(GVCAuditTrail_dModifyOn).Text = CType(e.Row.Cells(GVCAuditTrail_dModifyOn).Text, Date).ToString("dd-MMM-yyyy HH:mm").Trim() + strServerOffset
                End If

            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GVAuditTrail_RowDataBound")
        End Try
    End Sub

#End Region

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

#Region "Fill Funstions"

    Private Function fillvalues() As Boolean
        Try
            FillPeriodDropDown()
            FillSubjectDropDown()
            FillGridView()
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...fillvalues")
            Return False
        End Try
    End Function

    'Private Sub FillPeriodDropDown()
    '    Dim dsPeriod As New DataSet
    '    Dim WorkspaceId As String = ""
    '    WorkspaceId = Me.HProjectId.Value.Trim()
    '    If objHelp.FillDropDown("WorkspaceNodeDetail", "iPeriod", "iPeriod", "vWorkSpaceId='" & WorkspaceId & "'", dsPeriod, eStr_Retu) Then
    '        If dsPeriod.Tables(0).Rows.Count > 0 Then
    '            Me.ddlPeriod.DataSource = dsPeriod.Tables(0) '.DefaultView.ToTable("WorkSpaceNodeDetail", True, paramArry(0))
    '            Me.ddlPeriod.DataTextField = "iPeriod"
    '            Me.ddlPeriod.DataValueField = "iPeriod"
    '            Me.ddlPeriod.DataBind()
    '            Me.ddlPeriod.Items.Insert(0, New ListItem("Select Period", "0"))

    '            If Not IsNothing(Me.Request.QueryString("PeriodId")) Then
    '                Me.ddlPeriod.SelectedValue = Me.Request.QueryString("PeriodId").Trim()
    '                Me.ddlPeriod.Enabled = False
    '            End If
    '        End If
    '    Else
    '        ShowErrorMessage(eStr_Retu, "")
    '    End If
    'End Sub

    Private Sub FillPeriodDropDown()
        Dim ds_Periods As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim Periods As Integer = 1

        Try

            Me.ddlPeriod.Items.Clear()
            wStr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'"

            If Not objHelp.GetViewWorkSpaceNodeDetail(wStr, ds_Periods, eStr) Then
                Throw New Exception(eStr)
            End If
            If ds_Periods.Tables(0).Rows.Count > 0 Then

                Periods = ds_Periods.Tables(0).DefaultView.ToTable(True, "iPeriod").Rows.Count
                For count As Integer = 0 To Periods - 1
                    Me.ddlPeriod.Items.Add((count + 1).ToString)
                Next count
                Me.ddlPeriod.Items.Insert(0, New ListItem("Select Period", "0"))

                If Not IsNothing(Me.Request.QueryString("PeriodId")) Then
                    Me.ddlPeriod.SelectedValue = Me.Request.QueryString("PeriodId").Trim()
                    Me.ddlPeriod.Enabled = False
                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillPeriodDropDown")
        End Try

    End Sub

    Private Sub FillSubjectDropDown()
        Dim dsSubject As New DataSet
        Dim ds_WSSubject As New DataSet
        Dim dt_Period As New DataTable
        Dim dv_Period As New DataView
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim WorkspaceId As String = String.Empty
        Dim iperiod As Integer
        WorkspaceId = Me.HProjectId.Value.Trim()

        Try

            Wstr = "vWorkspaceId='" & WorkspaceId.Trim() & "' and cRejectionFlag <> 'Y' and cStatusindi <>'D' order by dreportingdate Desc"
            If Not Me.objHelp.GetWorkspaceSubjectMaster(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                    ds_WSSubject, estr) Then
                Exit Sub
            End If
            iperiod = Me.ddlPeriod.SelectedValue.Trim()
            If Me.ddlPeriod.SelectedIndex = 0 Then
                If ds_WSSubject.Tables(0).Rows.Count > 0 Then
                    dt_Period = ds_WSSubject.Tables(0).DefaultView.ToTable(True, "iPeriod")
                    dv_Period = dt_Period.DefaultView
                    dv_Period.Sort = "iPeriod"
                    iperiod = dv_Period.ToTable.Rows(dt_Period.Rows.Count - 1).Item("iPeriod")
                    dv_Period = Nothing
                End If
            End If

            Me.ddlPeriod.SelectedValue = iperiod

            If ds_WSSubject.Tables(0).Rows.Count > 0 Then

                'SDNidhi
                If Format(CDate(ds_WSSubject.Tables(0).Rows(0).Item("dreportingdate").date), "dd-MMM-yyyy") <> Format(CDate(ObjCommon.GetCurDatetime(Me.Session(S_TimeZoneName))).Date, "dd-MMM-yyyy") Then
                    Me.ObjCommon.ShowAlert("Date of Last Attendance is " & Format(CDate(ds_WSSubject.Tables(0).Rows(0).Item("dreportingdate").Date), "dd-MMM-yyyy") & _
                                                              ". So, please check the Period.", Me.Page)
                End If
                'Vineet'
                If iperiod = 1 Then
                    If System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge").Contains(Session(S_LocationCode)) = True Then
                        Wstr = " vSubjectId not in (Select Distinct vSubjectId from WorkspaceSubjectMst where vWorkspaceId='" & WorkspaceId.Trim() & _
                            "' and iPeriod=1 and cstatusindi <>'D')and vLocationcode='" & Me.Session(S_LocationCode).ToString.Trim() & "'"
                    Else
                        Wstr = " vSubjectId not in (Select Distinct vSubjectId from WorkspaceSubjectMst where vWorkspaceId='" & WorkspaceId.Trim() & _
                          "' and iPeriod=1 and cstatusindi <>'D') and vLocationcode Not In(" + System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge") + ")"
                    End If
                    If Me.chkProject.Checked Then
                        Wstr += " And vSubjectId in (select Distinct vSubjectId from SubjectWorkspaceAllocation where vWorkSpaceId='" & _
                                Me.HProjectId.Value.Trim() & "')"
                    End If

                    Wstr += " AND cRejectionFlag <> 'Y'"
                    Me.AutoCompleteExtender2.ContextKey = "View_SubjectMaster" + "#" + Wstr.Trim()

                ElseIf iperiod > 1 Then
                    Wstr = "vWorkspaceId='" & WorkspaceId.Trim() & "' and cRejectionFlag <> 'Y' and iPeriod =" + (CInt(iperiod) - 1).ToString() + " and " & _
                            " vSubjectId not in (Select Distinct vSubjectId from WorkspaceSubjectMst where vWorkspaceId='" & WorkspaceId.Trim() & _
                            "' and iPeriod=" & iperiod & ") And cStatusIndi <> 'D'"
                    Me.AutoCompleteExtender2.ContextKey = "view_WorkspaceSubjectMst" + "#" + Wstr.Trim()
                End If
            Else
                If System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge").Contains(Session(S_LocationCode)) = True Then
                    Wstr = "vLocationcode ='" & Me.Session(S_LocationCode).ToString.Trim() & "'And cRejectionFlag <> 'Y'"
                Else
                    Wstr = "vLocationcode Not In(" + System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge") + ") And cRejectionFlag <> 'Y'"
                End If

                If Me.chkProject.Checked Then
                    Wstr += " And vSubjectId in (select Distinct vSubjectId from SubjectWorkspaceAllocation where vWorkSpaceId='" & _
                            Me.HProjectId.Value.Trim() & "')"
                End If
                Me.AutoCompleteExtender2.ContextKey = "View_SubjectMaster" + "#" + Wstr.Trim()
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillSubjectDropDown")
        End Try
    End Sub

    Private Sub FillGridView()
        Dim dsWorkspaceSubjectMst As New DataSet
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim WorkspaceId As String = String.Empty
        WorkspaceId = Me.HProjectId.Value.Trim()
        Dim dc_ReportingDate As New DataColumn

        Try
            'commented on 03-Aug-2009

            Wstr = "vWorkSpaceId='" & WorkspaceId & "'  and iPeriod=" & _
                    IIf(Me.ddlPeriod.SelectedIndex = 0, "iPeriod", Me.ddlPeriod.SelectedValue.Trim()) '& " and cRejectionFlag <> 'Y'"
            '***********************************
            If Not IsNothing(Me.Request.QueryString("PeriodId")) Then
                'commented on 03-Aug-2009
                Wstr = "vWorkSpaceId='" & WorkspaceId & _
                                    "' and iPeriod=" & Me.Request.QueryString("PeriodId").Trim() '& " and cRejectionFlag <> 'Y'"
                '**************************
            End If

            'Added order by on 19-Mar-2010
            Wstr += " and cStatusindi <>'D' Order by cast(dReportingDate as datetimeoffset)"
            '*********************
            If Not objHelp.GetViewWorkspaceSubjectMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        dsWorkspaceSubjectMst, estr) Then
                Me.ObjCommon.ShowAlert("Error while getting Subjects data.", Me.Page)
                Exit Sub
            End If

            If dsWorkspaceSubjectMst.Tables(0).Rows.Count > 0 Then
                '*****************************************
                'SDNidhi
                'dc_ReportingDate = New DataColumn("ReportingDate_IST", System.Type.GetType("System.String"))
                'dsWorkspaceSubjectMst.Tables(0).Columns.Add("ReportingDate_IST")
                'dsWorkspaceSubjectMst.AcceptChanges()
                'For index As Integer = 0 To dsWorkspaceSubjectMst.Tables(0).Rows.Count - 1
                '    If Not dsWorkspaceSubjectMst.Tables(0).Rows(index).Item("dReportingDate").ToString() = "" Then
                '        dsWorkspaceSubjectMst.Tables(0).Rows(index).Item("ReportingDate_IST") = Convert.ToString(dsWorkspaceSubjectMst.Tables(0).Rows(index).Item("dReportingDate") + " IST (+" + offset.ToString + " GMT)")
                '    End If
                'Next
                'dsWorkspaceSubjectMst.AcceptChanges()
                '**************************************
                Me.btnAudit.Visible = True
            End If
            If Me.Request.QueryString("Mode") = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                'Me.btnExportToExcel.Style.Add("display", "")
                Me.Exportdiv.Style.Add("display", "")
            End If
            gvwWorkspaceSubjectMst.DataSource = dsWorkspaceSubjectMst
            gvwWorkspaceSubjectMst.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetAttendanceData", "fsetAttendanceData_Show(); ", True)
            If gvwWorkspaceSubjectMst.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvwWorkspaceSubjectMst", "UIgvwWorkspaceSubjectMst(); ", True)
            End If
            Me.lblTotalSub.Text = "Total Number Of Volunteer are :" + dsWorkspaceSubjectMst.Tables(0).Rows.Count.ToString

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillGridView")
        End Try
    End Sub

    Private Sub FillRejectDropDown()
        Dim dsReject As New DataSet
        If objHelp.GetReasonMst("vActivityId='" + GeneralModule.Act_Attendance + "' And cStatusIndi<>'D'", _
            WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsReject, eStr_Retu) Then

            Me.ddlReject.DataSource = dsReject
            Me.ddlReject.DataValueField = "nReasonNo"
            Me.ddlReject.DataTextField = "vReasonDesc"
            Me.ddlReject.DataBind()
            Me.ddlReject.Items.Insert(0, New ListItem("Select Reason", "0"))
        Else
            ObjCommon.ShowAlert("Reason Drop Down could not be loaded.", Me)
        End If
    End Sub

#End Region

#Region "DropDown Events"

    Protected Sub ddlPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Me.ddlPeriod.SelectedIndex > 0 Then
                FillSubjectDropDown()
                FillGridView()
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...ddlPeriod_SelectedIndexChanged")
        End Try
    End Sub

#End Region

#Region "chkProject CheckedChanged Events"

    Protected Sub chkProject_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Me.ddlPeriod.SelectedIndex <> -1 Then
                FillSubjectDropDown()
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...chkProject_CheckedChanged")
        End Try
    End Sub

#End Region

#Region "Functions"

    Public Function CheckInDosingDetail(ByVal iMySubjectNo As String, ByVal iPeriod As String) As Boolean
        Dim Wstr As String = String.Empty
        Dim Estr As String = String.Empty
        Dim Ds_DosingDetails As New DataSet

        Wstr = "vWorkSpaceId='" & HProjectId.Value.ToString() & "' and iMySubjectNo='" & iMySubjectNo & "' and dDosedOn is NOT NULL and cStatusIndi<>'D' and iPeriod='" & iPeriod & "'"

        If Not objHelp.View_DosingDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_DosingDetails, Estr) Then
            Me.ObjCommon.ShowAlert("Error While Getting Data From View_DosingDetail", Me.Page)
            Return False
        End If
        If Ds_DosingDetails.Tables(0).Rows.Count > 0 Then
            Return False
            Exit Function
        End If
        Return True
    End Function

    Private Function ShowAudit() As Boolean
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_WorkspaceSubjectMstHistory As New DataSet
        Dim dc_AuditTrailMst As New DataColumn
        Dim ds As New DataSet
        Dim bf As New BoundField
        Dim dc As New DataColumn

        wStr = " vWorkspaceSubjectId = '" & Me.ViewState(VS_WorkspaceSubjectId).ToString & "'"
        Try
            wStr += " And  vWorkspaceId = '" & Me.HProjectId.Value.ToString & "' And iPeriod=" & ddlPeriod.SelectedValue & "  order by iTranNo"
            If Not objHelp.View_WorkSpaceSubjectMstHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                            ds_WorkspaceSubjectMstHistory, eStr) Then
                Throw New Exception(eStr)
            End If

            ' Added on 18-03-2015 by Jeet Patel
            ' To show IST or EST column in Audit Trail

            If ds_WorkspaceSubjectMstHistory.Tables(0).Rows.Count < 1 Then
                ObjCommon.ShowAlert("No Data Found", Me.Page)
                Return False
            End If


            dc_AuditTrailMst = New DataColumn("dModifyOn_IST", System.Type.GetType("System.String"))
            ds_WorkspaceSubjectMstHistory.Tables(0).Columns.Add("dModifyOn_IST")
            ds_WorkspaceSubjectMstHistory.AcceptChanges()

            dc_AuditTrailMst = New DataColumn("ActualTIME", System.Type.GetType("System.String"))
            ds_WorkspaceSubjectMstHistory.Tables(0).Columns.Add(dc_AuditTrailMst)
            ds_WorkspaceSubjectMstHistory.AcceptChanges()


            For Each dr_Audit In ds_WorkspaceSubjectMstHistory.Tables(0).Rows
                dr_Audit("dModifyOn_IST") = (CDate(dr_Audit("dModifyOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)

                If Not objHelp.Proc_ActualAuditTrailTime(CDate(dr_Audit("dModifyOn")).ToString("dd-MMM-yyyy HH:mm") + "##" + Session(S_LocationCode), ds, eStr) Then
                    Throw New Exception(eStr)
                End If
                dr_Audit("ActualTIME") = CDate(ds.Tables(0).Rows(0).Item("ActualTIME")).ToString("dd-MMM-yyyy HH:mm") + " (" + ds.Tables(0).Rows(0).Item("TimeZoneOffSet").ToString + " GMT)"


            Next
            ds_WorkspaceSubjectMstHistory.AcceptChanges()

            If Me.GVAuditTrail.Columns.Count > 7 Then
                Me.GVAuditTrail.Columns.RemoveAt(GVAuditTrail.Columns.Count - 1)
            End If

            dc = New DataColumn(ds.Tables(0).Columns("ActualTIME").ColumnName)
            bf.DataField = dc.ColumnName
            bf.HeaderText = Session(S_TimeZoneName)
            GVAuditTrail.Columns.Add(bf)
            GVAuditTrail.Columns(7).ItemStyle.Wrap = False
            'End

            If ds_WorkspaceSubjectMstHistory.Tables(0).Rows.Count > 0 Then
                GVAuditTrail.DataSource = ds_WorkspaceSubjectMstHistory
                GVAuditTrail.DataBind()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvAuditTrail", "UIgvAuditTrail(); ", True)

            ElseIf ds_WorkspaceSubjectMstHistory.Tables(0).Rows.Count <= 0 Then
                ObjCommon.ShowAlert("No Audit Trail present", Me)
                Return False
            End If
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...ShowAudit")
            Return False
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

End Class