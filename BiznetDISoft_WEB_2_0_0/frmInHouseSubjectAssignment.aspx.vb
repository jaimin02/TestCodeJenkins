Imports System.Collections.Generic

Partial Class frmInHouseSubjectAssignment
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    'Private Const SubjectType As String = "I"

    Private Const SubjectType As String = "C"

    Private Const VS_Choice As String = "Choice"
    Private Const VS_WorkspaceSubjectMst As String = "dtWorkspaceSubjectMst"
    Private Const VS_WorkspaceSubjectId As String = "vWorkspaceSubjectId"
    Private Const VS_WorkspaceProtocolDetail As String = "dtWorkspaceProtocolDetail"
    Private Const VS_SrNo As String = "iSrNo"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVCell_SubjectId As Integer = 1
    Private Const GVCell_MySubjectNo As Integer = 2
    Private Const GVCell_Subject As Integer = 3
    Private Const GVCell_Initials As Integer = 4
    Private Const GVCell_Period As Integer = 5
    Private Const GVCell_ReportingDateTime As Integer = 6
    Private Const GVCell_Reason As Integer = 7
    Private Const GVCell_attendenceTakenBy As Integer = 8
    Private Const GVCell_Dlete As Integer = 9
    Private Const GVCell_RejectReason As Integer = 10
    Private Const GVCell_Rejected As Integer = 11
    Private Const GVC_Code As Integer = 8
    Private Const GVC_nWorkspaceSubjectId As Integer = 8
    Private Const GVCell_cRejected As Integer = 12
    '01-Feb-11
    Private Const GVCell_iMysubNo As Integer = 2
    '==

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

    Dim eStr_Retu As String
    Dim reportingdate As String = String.Empty
    Dim replaceoffset As String = String.Empty

#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            ' for inhouse projects only
            Me.AutoCompleteExtender1.ContextKey = "vProjectTypeCode = '0016'"
            If Not Page.IsPostBack Then
                GenCall()
            End If
            'If gvwWorkspaceSubjectMst.Rows.Count > 0 Then
            '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvwWorkspaceSubjectMst", "UIgvwWorkspaceSubjectMst(); ", True)
            'End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...Page_Load")
        End Try
    End Sub

#End Region

#Region "GENCALL"

    Private Function GenCall() As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim dt_WorkspaceSubjectmst As DataTable = Nothing
        Dim ds_WorkspaceSubjectmst As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Choice = Me.Request.QueryString("mode").ToString
            Me.ViewState(VS_Choice) = Choice   'To use it while saving
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_WorkspaceSubjectId) = Me.Request.QueryString("Value").ToString
            End If
            ''''Check for Valid User''''''''''''''
            If Not GenCall_Data(Choice, dt_WorkspaceSubjectmst) Then ' For Data Retrieval
                Return False
            End If
            Me.ViewState(VS_WorkspaceSubjectMst) = dt_WorkspaceSubjectmst ' adding blank DataTable in viewstate
            If Not GenCall_ShowUI(Choice, dt_WorkspaceSubjectmst) Then 'For Displaying Data 
                Return False
            End If
            GenCall = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...GenCall")
            Return False
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_Data"

    Private Function GenCall_Data(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                            ByRef dt_Dist_Retu As DataTable) As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_WorkspaceSubjectMst As DataSet = Nothing
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vWorkspaceSubjectId=" + Me.ViewState(VS_WorkspaceSubjectId).ToString() 'Value of where condition
            End If

            If Not objHelp.GetWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_WorkspaceSubjectMst, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_WorkspaceSubjectMst Is Nothing Then
                Throw New Exception(eStr)
            End If

            If ds_WorkspaceSubjectMst.Tables(0).Rows.Count <= 0 And _
               Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found for Selected role")
            End If
            dt_Dist_Retu = ds_WorkspaceSubjectMst.Tables(0)
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_Data")
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
        Dim Sender As New Object
        Dim e As EventArgs
        Try
            Page.Title = " :: Attendance ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Master.FindControl("lblHeading"), Label).Text = "In-House Subject Assignment"

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

            End If
            fillvalues()

            If Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.txtDate.Text = ConvertDbNullToDbTypeDefaultValue(dt_WorkspaceSubjectmst.Rows(0)("dReportingDate"), dt_WorkspaceSubjectmst.Rows(0)("dReportingDate").GetType)
                Me.txtDate.Text = Format(CDate(Me.txtDate.Text.Trim()), "dd-MMM-yyyy")
                Me.HSubjectId.Value = ConvertDbNullToDbTypeDefaultValue(dt_WorkspaceSubjectmst.Rows(0)("vSubjectId"), dt_WorkspaceSubjectmst.Rows(0)("vSubjectId").GetType)
                Me.ddlPeriod.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_WorkspaceSubjectmst.Rows(0)("iPeriod"), dt_WorkspaceSubjectmst.Rows(0)("vSubjectId").GetType)

            End If

            '==added on 16-jan-2010 by deepak singh 
            Me.Image1.ImageUrl = "~/images/NotFound.gif"
            '=======


            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(Sender, e)
            End If
            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
            Return False
        End Try
    End Function

#End Region

#Region "Reset Page"
    Private Sub ResetPage()

        Me.txtSubject.Text = ""
        Me.HSubjectId.Value = ""
        FillGridView()
        '===added on 16-Jan-2010 by deepak singh
        Me.Image1.ImageUrl = "~/images/NotFound.gif"
        Me.txtLockRemark.Text = ""
        '=======

    End Sub
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

#Region "Save"

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Ds_stagemat As DataSet
        Dim eStr As String = String.Empty

        Try
            AssignUpdatedValues()

            If Not ValidateSubject() Then
                Exit Sub
            End If

            Ds_stagemat = New DataSet
            Ds_stagemat.Tables.Add(CType(Me.ViewState(VS_WorkspaceSubjectMst), Data.DataTable).Copy())
            Ds_stagemat.Tables(0).TableName = "VIEW_workspaceSubjectMst"   ' New Values on the form to be updated

            If Not objLambda.Save_InsertWorkspaceSubjectMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_WorkspaceSubjectMst, Ds_stagemat, Me.Session(S_UserID), eStr) Then
                ObjCommon.ShowAlert("Error while Saving WorkspaceSubjectMst", Me.Page)
                Exit Sub
            End If

            ObjCommon.ShowAlert("Attendance saved successfully.", Me)
            ResetPage()

        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
        End Try

    End Sub

    Protected Sub btnchkScrDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnchkScrDate.Click
    End Sub

    Private Function AssignUpdatedValues() As Boolean
        Dim estr As String = String.Empty
        Dim dtOld As New DataTable
        Dim dr As DataRow
        Dim CanStartAfterDetails As String = String.Empty
        Dim MySubjectNo As String = String.Empty
        Dim dsSubjectMst As New DataSet
        Dim dsWorkspaceSubjectMst As New DataSet
        Dim Wstr As String = "vSubjectId='" + Me.HSubjectId.Value.Trim() + "'"
        Dim WorkspaceId As String = String.Empty
        Dim dssubjectno As New DataSet
        WorkspaceId = Me.HProjectId.Value.Trim() 'Me.Request.QueryString("workspaceid").Trim()

        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

        If (Me.HSubjectId.Value.Trim <> "") Then
            If Not objHelp.GetSubjectMaster(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsSubjectMst, estr) Then
                ShowErrorMessage(eStr_Retu, "")
            End If
        End If

        Try
            dtOld = Me.ViewState(VS_WorkspaceSubjectMst)
            ' for assigning imysubjectno :Start
            If Not Me.objHelp.GetFieldsOfTable("workspacesubjectmst", "isnull(max(imysubjectno),0) as maxsubno", _
                    "vworkspaceid = '" + WorkspaceId + "' and imysubjectno < 2000  And iPeriod = " & Me.ddlPeriod.SelectedValue.Trim(), _
                    dssubjectno, estr) Then
                Return False
            End If

            If dssubjectno.Tables(0).Rows(0)("maxsubno") = 0 Then
                MySubjectNo = 1001
            Else
                MySubjectNo = dssubjectno.Tables(0).Rows(0)("maxsubno") + 1
            End If

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                dtOld.Clear()
                dr = dtOld.NewRow
                dr("vWorkspaceSubjectId") = "0000"
                dr("vWorkspaceid") = WorkspaceId
                dr("iMySubjectNo") = MySubjectNo
                dr("vMySubjectNo") = MySubjectNo
                dr("vSubjectId") = Me.HSubjectId.Value.Trim()
                dr("vInitials") = dsSubjectMst.Tables(0).Rows(0)("vInitials")

                dr("iPeriod") = "0"
                If (Me.ddlPeriod.SelectedIndex <> 0) Then
                    dr("iPeriod") = Me.ddlPeriod.SelectedValue
                End If

                'SDNidhi
                dr("dReportingDate") = Me.ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName)) 'System.DateTime.Now.ToString
                If (Me.txtDate.Text <> "") Then
                    dr("dReportingDate") = IIf(String.IsNullOrEmpty(txtDate.Text.Trim()), System.DBNull.Value, Me.txtDate.Text.Trim())
                End If

                dr("cRejectionFlag") = "N"
                dr("nReasonNo") = "0"
                dr("dModifyOn") = Today.Date.ToString("dd-MMM-yyyy")
                dr("cStatusIndi") = ""
                dr("iModifyBy") = Me.Session(S_UserID)
                dtOld.Rows.Add(dr)

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                For Each dr In dtOld.Rows

                    dr("vWorkspaceid") = WorkspaceId
                    dr("IMySubjectNo") = MySubjectNo
                    dr("vMySubjectNo") = MySubjectNo
                    dr("vSubjectId") = Me.HSubjectId.Value.Trim()
                    dr("vInitials") = dsSubjectMst.Tables(0).Rows(0)("vInitials")

                    dr("iPeriod") = "0"
                    If (Me.ddlPeriod.SelectedIndex <> 0) Then
                        dr("iPeriod") = Me.ddlPeriod.SelectedValue
                    End If

                    'SdNidhi
                    dr("dReportingDate") = Me.ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName)) 'System.DateTime.Now.ToString
                    If (Me.txtDate.Text <> "") Then
                        dr("dReportingDate") = IIf(String.IsNullOrEmpty(txtDate.Text.Trim()), System.DBNull.Value, Me.txtDate.Text.Trim())
                    End If

                    dr("cRejectionFlag") = "N"
                    dr("nReasonNo") = "0"
                    dr("dModifyOn") = Today.Date.ToString("dd-MMM-yyyy")
                    dr("cStatusIndi") = ""
                    dr("iModifyBy") = Me.Session(S_UserID)
                    'dr("vRemarks") = Me.txtLockRemark.Text.Trim()
                    dr.AcceptChanges()

                Next
                dtOld.AcceptChanges()

            End If

            Me.ViewState(VS_WorkspaceSubjectMst) = dtOld
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
        Dim Wstr As String = "vWorkspaceSubjectId ='" + ViewState(VS_WorkspaceSubjectId) + "'"

        If objHelp.GetViewWorkspaceSubjectMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsTemp, eStr_Retu) Then
            dtTemp = dsTemp.Tables(0)
            dr = dtTemp.NewRow

            dr("cRejectionFlag") = "N"
            dr("nReasonNo") = System.Decimal.Zero
            dr("iModifyBy") = Me.Session(S_UserID)

            If rejected Then
                For Each dr In dtTemp.Rows
                    dr("cRejectionFlag") = "Y"

                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("vRemarks") = Me.txtRejectReason.Text.Trim()
                    dr.AcceptChanges()
                Next
                dtTemp.AcceptChanges()
            End If
        Else
            ShowErrorMessage(eStr_Retu, "...SaveRejected")
        End If
        'dsTemp.Tables(0).TableName = "view_WorkspaceSubjectMst"
        If Not objLambda.Save_InsertWorkspaceSubjectMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                                WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_WorkspaceSubjectMst, dsTemp, _
                                Me.Session(S_UserID), eStr_Retu) Then
            Dim estr As String = "Error Saving Subject Attendance !"
            ObjCommon.ShowAlert(estr + eStr_Retu, Me)
            Exit Sub
        End If

        If rejected Then
            Me.txtRejectReason.Text = ""
            divRejectSubject.Attributes.Add("style", "display:none")
        End If
        ObjCommon.ShowAlert("Subject Rejected Successfully.", Me)
        Me.txtSubject.Text = ""
        Me.HSubjectId.Value = ""


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

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        divRejectSubject.Attributes.Add("style", "display:none")
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        SaveRejected(True)
        FillGridView()
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ResetPage()
    End Sub

    Protected Sub btnCloseNew_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim WorkspaceId As String = String.Empty
        Dim DMS As String = String.Empty
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
    End Sub

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        fillvalues()
    End Sub

    Protected Sub btnSubject_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim wStr1 As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds_SubjectBlob As New DataSet

        'image if there is no image of subject

        wStr1 = "cRecordType='P' and iTranNo=(select Max(itranNO) from SubjectBlobDetails where cRecordType='P' and vSubjectId='" & HSubjectId.Value.ToString.Trim & "') and vSubjectId='" & HSubjectId.Value.ToString.Trim & "'"

        If Not Me.objHelp.getSubjectBlobDetails(wStr1, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubjectBlob, estr) Then
            MsgBox("Error while getting Data." + vbCrLf + estr)
            Exit Sub
        End If

        Me.Image1.ImageUrl = "~/images/NotFound.gif"
        If ds_SubjectBlob.Tables(0).Rows.Count > 0 Then
            Me.Image1.ImageUrl = "frmPIFImage.aspx?subjectid=" + Me.HSubjectId.Value.Trim()
        End If

        ' Me.Image1.ImageUrl = "frmPIFImage.aspx?subjectid=" + subjectId.ToString()
        '************************************

    End Sub

    Protected Sub BtnAddPeriod_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAddPeriod.Click
        Dim estr As String = String.Empty
        Dim dtOld As New DataTable
        Dim dr As DataRow
        Dim Ds_WorkspaceProtocolDetail As New DataSet
        Dim RowCount As Integer = 0
        Dim RowOldValue As Integer
        Dim wStr As String = String.Empty
        Dim ds_WorkspacenodeDetail, dsForINodeID As New DataSet
        Dim strNodeNotFound As String = String.Empty
        Dim WorkspaceId As String = String.Empty

        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

        dtOld = ViewState(VS_WorkspaceProtocolDetail)
        RowCount = dtOld.Rows.Count

        dr = dtOld.NewRow()
        If RowCount > 0 Then
            dr = dtOld.Rows(0)
            RowOldValue = Convert.ToInt32(dtOld.Rows(0).Item("iNoOfPeriods"))
        End If
        dr("iNoOfPeriods") = 1
        If RowCount > 0 Then
            dr("iNoOfPeriods") = 1
            If Not dtOld.Rows(0).Item("iNoOfPeriods") Is System.DBNull.Value Then
                dr("iNoOfPeriods") = RowOldValue + 1
            End If
        End If

        If RowCount > 0 Then
            dr.AcceptChanges()
        Else
            dtOld.Rows.Add(dr)
        End If

        dtOld.AcceptChanges()
        Me.ViewState(VS_WorkspaceProtocolDetail) = dtOld
        Ds_WorkspaceProtocolDetail = New DataSet
        Ds_WorkspaceProtocolDetail.Tables.Add(CType(Me.ViewState(VS_WorkspaceProtocolDetail), Data.DataTable).Copy())
        Ds_WorkspaceProtocolDetail.Tables(0).TableName = "WorkspaceProtocolDetail"   ' New Values on the form to be updated

        If Not objLambda.Save_WorkspaceProtocoldetail(Me.ViewState(VS_Choice), Ds_WorkspaceProtocolDetail, Me.Session(S_UserID), WorkspaceId, estr) Then
            ObjCommon.ShowAlert("Error while Saving Period", Me.Page)
            Exit Sub
        End If

        '********************Added For Automatically adding a activities**********[08-Nov-2011]
        wStr = " vWorkspaceid = '" + Me.HProjectId.Value + "'"
        If Not objHelp.getWorkSpaceNodeDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_WorkspacenodeDetail, eStr_Retu) Then
            Response.Write(eStr_Retu)
            Exit Sub
        End If
        ds_WorkspacenodeDetail.Tables(0).Rows.Clear()



        If Not objHelp.getWorkSpaceNodeDetail(wStr + " And iPeriod=" + RowOldValue.ToString, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsForINodeID, estr) Then
            ObjCommon.ShowAlert("Error While Getting INodeID for adding a Node in Project Structure", Me.Page)
            Exit Sub
        End If
        '==Akhilesh=='
        '*============Adding "In-H Safety Sample Collection" Activity-----0051----

        If dsForINodeID.Tables("WorkSpaceNodeDetail").Select("vActivityId=0051", "iNodeId Desc").Length > 0 Then
            dr = ds_WorkspacenodeDetail.Tables(0).NewRow
            dr("iNodeId") = dsForINodeID.Tables("WorkSpaceNodeDetail").Select("vActivityId=0051", "iNodeId Desc")(0)("iNodeId")
            dr("vWorkspaceId") = Me.HProjectId.Value
            dr("vNodeName") = "In-H Safety Sample Collection"
            dr("vActivityId") = "0051"
            dr("iPeriod") = RowOldValue + 1
            dr("vNodeDisplayName") = "In-H Safety Sample Collection" + "(Period-" + (RowOldValue + 1).ToString + ")"
            dr("iModifyBy") = Me.Session(S_UserID)
            ds_WorkspacenodeDetail.Tables(0).Rows.Add(dr)

            ds_WorkspacenodeDetail.Tables(0).TableName = "WorkspaceNodeDetail"   ' New Values on the form to be updated


            If ds_WorkspacenodeDetail.Tables("WorkspaceNodeDetail").Rows.Count > 0 Then
                If Not objLambda.Save_InsertWorkspaceNodeAfter(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_WorkspacenodeDetail, Me.Session("UserCode"), estr) Then
                    Me.ObjCommon.ShowAlert(estr, Me.Page())
                    Exit Sub
                End If
            End If
            ds_WorkspacenodeDetail.Tables(0).Rows.Clear()

        Else
            strNodeNotFound += " #Post Study Sample."
        End If

        '*============Adding "Medical Screening recrords" Activity===0052=== 
        If dsForINodeID.Tables("WorkSpaceNodeDetail").Select("vActivityId=0052", "iNodeId Desc").Length > 0 Then
            dr = ds_WorkspacenodeDetail.Tables(0).NewRow
            dr("iNodeId") = dsForINodeID.Tables("WorkSpaceNodeDetail").Select("vActivityId=0052", "iNodeId Desc")(0)("iNodeId")
            dr("vWorkspaceId") = Me.HProjectId.Value
            dr("vNodeName") = "Medical Screening recrords"
            dr("vActivityId") = "0052"
            dr("iPeriod") = RowOldValue + 1
            dr("vNodeDisplayName") = "Medical Screening Records." + "(Period-" + (RowOldValue + 1).ToString + ")"
            dr("iModifyBy") = Me.Session(S_UserID)
            ds_WorkspacenodeDetail.Tables(0).Rows.Add(dr)
            ds_WorkspacenodeDetail.Tables(0).TableName = "WorkspaceNodeDetail"   ' New Values on the form to be updated

            If ds_WorkspacenodeDetail.Tables("WorkspaceNodeDetail").Rows.Count > 0 Then
                If Not objLambda.Save_InsertWorkspaceNodeAfter(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_WorkspacenodeDetail, Me.Session("UserCode"), estr) Then
                    Me.ObjCommon.ShowAlert(estr, Me.Page())
                    Exit Sub
                End If
            End If
            ds_WorkspacenodeDetail.Tables(0).Rows.Clear()
        Else
            strNodeNotFound += " #Medical Screening Records."
        End If


        '*============Adding "Blood Collection Form" Activity===0053===
        If dsForINodeID.Tables("WorkSpaceNodeDetail").Select("vActivityId=0053", "iNodeId Desc").Length > 0 Then
            dr = ds_WorkspacenodeDetail.Tables(0).NewRow
            dr("iNodeId") = dsForINodeID.Tables("WorkSpaceNodeDetail").Select("vActivityId=0053", "iNodeId Desc")(0)("iNodeId")
            dr("vWorkspaceId") = Me.HProjectId.Value
            dr("vNodeName") = "Blood Collection Form"
            dr("vActivityId") = "0053"
            dr("iPeriod") = RowOldValue + 1
            dr("vNodeDisplayName") = "Blood Collection Form" + "(Period-" + (RowOldValue + 1).ToString + ")"
            dr("iModifyBy") = Me.Session(S_UserID)
            ds_WorkspacenodeDetail.Tables(0).Rows.Add(dr)

            ds_WorkspacenodeDetail.Tables(0).TableName = "WorkspaceNodeDetail"   ' New Values on the form to be updated

            If ds_WorkspacenodeDetail.Tables("WorkspaceNodeDetail").Rows.Count > 0 Then
                If Not objLambda.Save_InsertWorkspaceNodeAfter(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_WorkspacenodeDetail, Me.Session("UserCode"), estr) Then
                    Me.ObjCommon.ShowAlert(estr, Me.Page())
                    Exit Sub
                End If
            End If
            ds_WorkspacenodeDetail.Tables(0).Rows.Clear()
        Else
            strNodeNotFound += " #Blood Collection Form."
        End If

        '*=========================================================================================
        FillPeriodDropDown()
        FillGridView()

        If strNodeNotFound <> "" Then
            Me.ObjCommon.ShowAlert("These Activities Not Found While Adding Nodes in Project Structure." + strNodeNotFound, Me.Page)
        End If

    End Sub

    Protected Sub btnDivOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDivOK.Click
        Dim ds_WorkspaceSubjectMst As New DataSet
        Dim dt_WorkspaceSubjectmst As New DataTable
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty

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

        Me.ObjCommon.ShowAlert("Attendance of selected subject deleted successfully.", Me)
        ResetPage()

    End Sub

    Protected Sub btnAudit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAudit.Click
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_WorkspaceSubMstHistory As New DataSet

        Try
            wStr = " vWorkspaceID = '" & Me.HProjectId.Value.Trim() & _
                  "' AND iPeriod = " & Me.ddlPeriod.SelectedValue.ToString() & _
                  " And DeleteIndi ='D' order by dModifyOn desc"

            If Not objHelp.View_WorkSpaceSubjectMstHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                           ds_WorkspaceSubMstHistory, eStr) Then
                Throw New Exception(eStr)
            End If


            If ds_WorkspaceSubMstHistory.Tables(0).Rows.Count > 0 Then
                Me.GVAudit.DataSource = ds_WorkspaceSubMstHistory
                Me.GVAudit.DataBind()
                Me.MpeAudit.Show()
            ElseIf ds_WorkspaceSubMstHistory.Tables(0).Rows.Count <= 0 Then
                ObjCommon.ShowAlert("No Audit Trail Present", Me)
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...btnAudit_Click")
        End Try

    End Sub

#End Region

#Region "Grid Event"

    Protected Sub gvwWorkspaceSubjectMst_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        gvwWorkspaceSubjectMst.PageIndex = e.NewPageIndex
        FillGridView()
    End Sub

    Protected Sub gvwWorkspaceSubjectMst_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwWorkspaceSubjectMst.RowCommand
        Dim index As Integer = e.CommandArgument
        Dim RedirectStr As String = ""

        If e.CommandName.ToUpper = "DELETE" Then
            '01-Feb-11
            If Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVCell_iMysubNo).Text.Trim() <> 0 Then
                ObjCommon.ShowAlert("Subject is Assigned in Project.You cannot delete Attendance of selected subject", Me)
                Exit Sub
            End If
            '===
            Me.ViewState(VS_WorkspaceSubjectId) = Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVC_Code).Text.Trim()
            Me.ViewState(VS_SrNo) = Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVC_SrNo).Text.Trim()
            Me.mpeDialog.Show()
        ElseIf e.CommandName.ToUpper = "REJECT" Then
            Me.lblSubjectName.Text = Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVCell_Subject).Text.Trim()
            Me.ViewState(VS_WorkspaceSubjectId) = Me.gvwWorkspaceSubjectMst.Rows(index).Cells(GVC_nWorkspaceSubjectId).Text.Trim()
            Me.mpeRejectSubject.Show()
        End If

    End Sub

    Protected Sub gvwWorkspaceSubjectMst_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Try
            If e.Row.RowType = DataControlRowType.DataRow Or _
                          e.Row.RowType = DataControlRowType.Header Or _
                          e.Row.RowType = DataControlRowType.Footer Then

                e.Row.Cells(GVC_nWorkspaceSubjectId).Visible = False
                e.Row.Cells(GVCell_Dlete).Visible = False
                e.Row.Cells(GVCell_cRejected).Visible = False
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...gvwWorkspaceSubjectMst_RowCreated")
        End Try
    End Sub

    Protected Sub gvwWorkspaceSubjectMst_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Dim img As HtmlImage = Nothing
        Dim dv As HtmlControl = Nothing

        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(GVC_SrNo).Text = (Me.gvwWorkspaceSubjectMst.PageIndex * Me.gvwWorkspaceSubjectMst.PageSize) + e.Row.RowIndex + 1

                dv = CType(e.Row.FindControl("canal"), HtmlControl)
                img = CType(e.Row.FindControl("imgShow"), HtmlImage)
                If Not img Is Nothing AndAlso Not dv Is Nothing Then
                    img.Attributes.Add("onClick", "$('#" + dv.ClientID + "').toggle('medium');")
                End If

                CType(e.Row.FindControl("ImgReject"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgReject"), ImageButton).CommandName = "REJECT"
                e.Row.Cells(GVCell_Rejected).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells(GVCell_Rejected).VerticalAlign = HorizontalAlign.Center

                CType(e.Row.FindControl("ImgCancel"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgCancel"), ImageButton).CommandName = "DELETE"
                If e.Row.Cells(GVCell_cRejected).Text.ToUpper.Trim() = "Y" Then
                    e.Row.BackColor = Drawing.Color.Red
                    e.Row.Enabled = False
                End If

                e.Row.Cells(GVCell_RejectReason).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells(GVCell_RejectReason).VerticalAlign = HorizontalAlign.Center

                reportingdate = e.Row.Cells(GVCell_ReportingDateTime).Text.ToString
                replaceoffset = reportingdate.Substring(reportingdate.Length - 7, 7).ToString.Trim()
                e.Row.Cells(GVCell_ReportingDateTime).Text = CDate(Replace(reportingdate, replaceoffset, "")).ToString("dd-MMM-yyyy HH:mm") & " (" & reportingdate.Substring(reportingdate.Length - 7, 7).ToString.Trim() + " GMT)"

            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...gvwWorkspaceSubjectMst_RowDataBound")
        End Try

    End Sub


    Protected Sub gvwWorkspaceSubjectMst_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvwWorkspaceSubjectMst.RowDeleting

    End Sub

    Protected Sub GVAudit_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVAudit.RowCreated

        If e.Row.RowType = DataControlRowType.DataRow Or _
                      e.Row.RowType = DataControlRowType.Header Or _
                      e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVCAudit_iTranN).Visible = False
            e.Row.Cells(GVCAudit_nWorkspaceSubjectHistoryId).Visible = False
            e.Row.Cells(GVCAudit_vWorkspaceSubjectId).Visible = False
        End If

    End Sub

    Protected Sub GVAudit_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVAudit.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            e.Row.Cells(GVCAudit_dReportingDate).Text = Replace(e.Row.Cells(GVCAudit_dReportingDate).Text.Trim(), "&nbsp;", "")
            e.Row.Cells(GVCAudit_dModifyOn).Text = Replace(e.Row.Cells(GVCAudit_dModifyOn).Text.Trim(), "&nbsp;", "")

            'SDNidhi
            If Not Convert.ToString(e.Row.Cells(GVCAudit_dReportingDate).Text).Trim = "" Then
                reportingdate = Replace(e.Row.Cells(GVCAudit_dReportingDate).Text.Trim(), "&nbsp;", "")
                replaceoffset = reportingdate.Substring(reportingdate.Length - 7, 7).ToString.Trim()
                e.Row.Cells(GVCAudit_dReportingDate).Text = CDate(Replace(reportingdate, replaceoffset, "")).ToString("dd-MMM-yyyy HH:mm") & " (" & reportingdate.Substring(reportingdate.Length - 7, 7) + " GMT)"
            End If

            If Not Convert.ToString(e.Row.Cells(GVCAudit_dModifyOn).Text).Trim = "" Then
                e.Row.Cells(GVCAudit_dModifyOn).Text = CType(Replace(e.Row.Cells(GVCAudit_dModifyOn).Text.Trim(), "&nbsp;", ""), Date).ToString("dd-MMM-yyyy HH:mm").Trim() + strServerOffset
            End If

        End If
    End Sub

#End Region

#Region "Fill Functions"

    Private Function fillvalues() As Boolean
        Try
            FillPeriodDropDown()
            FillSubjectDropDown()
            FillGridView()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


    Private Sub FillPeriodDropDown()
        Dim wStr As String = ""
        Dim dsPeriod As New DataSet
        Dim iPeriodNumbers As Integer = 0

        wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
        If Not objHelp.GetWorkspaceProtocolDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                        dsPeriod, eStr_Retu) Then
            Me.ShowErrorMessage(eStr_Retu, "")
            Exit Sub
        End If
        Me.ViewState(VS_WorkspaceProtocolDetail) = dsPeriod.Tables(0) ' adding DataTable in viewstate

        Me.ddlPeriod.Items.Clear()
        Me.ddlPeriod.Items.Insert(0, New ListItem("Select Period", "0"))
        If dsPeriod.Tables(0).Rows.Count > 0 Then
            iPeriodNumbers = dsPeriod.Tables(0).Rows(0).Item("iNoOfPeriods")
        End If
        If iPeriodNumbers > 0 Then
            For count As Integer = 1 To dsPeriod.Tables(0).Rows(0).Item("iNoOfPeriods")
                Me.ddlPeriod.Items.Add(count.ToString())
            Next

            Me.ddlPeriod.SelectedValue = iPeriodNumbers
            Me.BtnAddPeriod.Visible = True
        End If


        If Not IsNothing(Me.Request.QueryString("PeriodId")) Then
            Me.ddlPeriod.SelectedValue = Me.Request.QueryString("PeriodId").Trim()
            Me.ddlPeriod.Enabled = False
        End If

    End Sub

    Private Sub FillSubjectDropDown()
        Dim dsSubject As New DataSet
        Dim ds_WSSubject As New DataSet
        Dim dt_Period As New DataTable
        Dim dv_Period As New DataView
        Dim estr As String = ""
        Dim Wstr As String = ""
        Dim WorkspaceId As String = ""
        Dim iperiod As Integer

        WorkspaceId = Me.HProjectId.Value.Trim()

        Wstr = "vWorkspaceId='" & WorkspaceId.Trim() & "' and cStatusindi <>'D' order by dreportingdate Desc"

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
        'dv_Period.DefaultView.Sort = "iPeriod"

        If ds_WSSubject.Tables(0).Rows.Count > 0 Then
            'If Format(CDate(ds_WSSubject.Tables(0).Rows(0).Item("dreportingdate")), "dd-MMM-yyyy") <> Format(System.DateTime.Now(), "dd-MMM-yyyy") Then
            'SDNidhi
            If Format(CDate(ds_WSSubject.Tables(0).Rows(0).Item("dreportingdate").date), "dd-MMM-yyyy") <> Format(CDate(ObjCommon.GetCurDatetime(Me.Session(S_TimeZoneName))).Date, "dd-MMM-yyyy") Then

                Me.ObjCommon.ShowAlert("Date of Last Attendance is " & Format(CDate(ds_WSSubject.Tables(0).Rows(0).Item("dreportingdate").Date), "dd-MMM-yyyy") & _
                                                          ". So, please check the Period.", Me.Page)
            End If
            'Vineet'
            If Me.ddlPeriod.SelectedValue.Trim() > 0 Then 'ds_WSSubject.Tables(0).Rows(0).Item("iPeriod") 
                If System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge").Contains(Session(S_LocationCode)) = True Then
                    Wstr = " vSubjectId not in (Select Distinct vSubjectId from WorkspaceSubjectMst where vWorkspaceId='" & WorkspaceId.Trim() & _
                            "' and iPeriod=" & Me.ddlPeriod.SelectedValue.Trim() & " and cstatusindi <>'D') and vLocationcode='" & Me.Session(S_LocationCode).ToString.Trim() & "'"
                Else
                    Wstr = " vSubjectId not in (Select Distinct vSubjectId from WorkspaceSubjectMst where vWorkspaceId='" & WorkspaceId.Trim() & _
                            "' and iPeriod=" & Me.ddlPeriod.SelectedValue.Trim() & " and cstatusindi <>'D') and vLocationcode Not In(" + System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge") + ")"
                End If

                Wstr += " AND cRejectionFlag <> 'Y' AND  cSubjectType <> '" + SubjectType + "'"
                Me.AutoCompleteExtender2.ContextKey = "View_SubjectMaster" + "#" + Wstr.Trim() + "#"
            End If

        Else
            If System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge").Contains(Session(S_LocationCode)) = True Then
                Wstr = "vLocationcode='" & Me.Session(S_LocationCode).ToString.Trim() & "' AND cRejectionFlag <> 'Y' AND cSubjectType <> '" + SubjectType + "'"
            Else
                Wstr = "vLocationcode Not In(" + System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge") + ") AND cRejectionFlag <> 'Y' AND cSubjectType <> '" + SubjectType + "'"
            End If
            If Me.HProjectId.Value.Trim.Length > 0 Then
                If Me.ddlPeriod.SelectedValue = 1 Then 'ds_WSSubject.Tables(0).Rows(0).Item("iPeriod") 

                    Wstr = " vSubjectId not in (Select Distinct vSubjectId from WorkspaceSubjectMst where vWorkspaceId='" & Me.HProjectId.Value.Trim() & _
                            "' and iPeriod=1 and cstatusindi <>'D') AND cSubjectType <> '" + SubjectType + "'"

                    Me.AutoCompleteExtender2.ContextKey = "View_SubjectMaster" + "#" + Wstr.Trim() + "#"

                ElseIf Me.ddlPeriod.SelectedValue.Trim() > 1 Then 'ds_WSSubject.Tables(0).Rows(0).Item("iPeriod") 

                    Wstr = " cRejectionFlag <> 'Y' and vSubjectId not in (Select Distinct vSubjectId from WorkspaceSubjectMst where vWorkspaceId='" & Me.HProjectId.Value.Trim() & _
                            "' and iPeriod=" & Me.ddlPeriod.SelectedValue.Trim() & ") AND cSubjectType <> '" + SubjectType + "'"

                    Me.AutoCompleteExtender2.ContextKey = "view_WorkspaceSubjectMst" + "#" + Wstr.Trim() + "#"

                End If
            End If
            Me.AutoCompleteExtender2.ContextKey = "View_SubjectMaster" + "#" + Wstr.Trim() + "#"

        End If

    End Sub

    Private Sub FillGridView()
        Dim dsWorkspaceSubjectMst As New DataSet
        Dim estr As String = ""
        Dim Wstr As String = ""
        Dim WorkspaceId As String = ""
        WorkspaceId = Me.HProjectId.Value.Trim()

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
        Wstr += " and cStatusindi <>'D'  Order by cast(dReportingDate as datetimeoffset)"
        '*********************
        If Not objHelp.GetViewWorkspaceSubjectMstInHouse(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    dsWorkspaceSubjectMst, estr) Then
            Me.ObjCommon.ShowAlert("Error While Getting Subjects Data.", Me.Page)
            Exit Sub
        End If

        gvwWorkspaceSubjectMst.DataSource = dsWorkspaceSubjectMst
        gvwWorkspaceSubjectMst.DataBind()
        If gvwWorkspaceSubjectMst.Rows.Count > 0 Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvwWorkspaceSubjectMst", "UIgvwWorkspaceSubjectMst(); ", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetInHouse", "fsetInHouse_Show(); ", True)
        End If
        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetInHouse", "fsetInHouse_Show(); ", True)
        Me.lblTotalSub.Text = "Total Number Of Volunteers Are :" + dsWorkspaceSubjectMst.Tables(0).Rows.Count.ToString

    End Sub

    Private Sub FillRejectDropDown()
        Dim dsReject As New DataSet
        If objHelp.GetReasonMst("vActivityId='" + GeneralModule.Act_Attendance + "' And cStatusIndi<>'D'", _
            WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsReject, eStr_Retu) Then
        Else
            ObjCommon.ShowAlert("Reason Drop Down could not be loaded.", Me)
        End If
    End Sub

#End Region

#Region "DropDown Events"

    Protected Sub ddlPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.ddlPeriod.SelectedIndex > 0 Then
            FillSubjectDropDown()
            FillGridView()
        End If

    End Sub

#End Region

End Class