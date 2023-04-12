
Partial Class frmScreeningStudyUpdateRecord
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    'Private Const SubjectType As String = "I" 'Means Inhouse
    Private Const SubjectType As String = "C" 'Data Mergin
    Private Const VS_Choice As String = "Choice"
    Private Const GVC_EditButton As Integer = 0
    Private Const GVC_SrNo As Integer = 1
    Private Const GVC_SubjectScreeningRecordNo As Integer = 2
    Private Const GVC_TranNo As Integer = 3
    Private Const GVC_ScreeningDate As Integer = 4
    Private Const GVC_ScreeningRecordedBy As Integer = 5
    Private Const GVC_XRayDate As Integer = 6
    Private Const GVC_NextXRayDate As Integer = 7
    Private Const GVC_ScreeningFUDate As Integer = 8
    Private Const GVC_ScreeningFUDate2 As Integer = 9
    Private Const GVC_Remark As Integer = 10
    Private Const GVC_Selected As Integer = 11
    Private Const GVC_ReasonForRejection As Integer = 12

    Private Const GVC_EnrolledFlag As Integer = 13
    Private Const GVC_ReasonForNotParticipated As Integer = 14
    Private Const GVC_ProjectNo As Integer = 15
    Private Const GVC_SubjectNo As Integer = 16
    Private Const GVC_RecordedBy As Integer = 17
    Private Const GVC_LastSampleDate As Integer = 18
    Private Const GVC_StudyCompleted As Integer = 19
    Private Const GVC_ReasonForNonCompletion As Integer = 20
    Private Const GVC_NoOfDays As Integer = 21
    Private Const GVC_Eligibledate As Integer = 22
    Private Const GVC_SampleRecordedBy As Integer = 23
    Private Const GVC_VerifiedBy As Integer = 24
    Private Const GVC_CheckedBy As Integer = 25
    Private Const VS_ScreeningRecordsDetails As String = "ScreeningRecordsDetails"

#End Region

#Region "Load Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region " GenCall() "

    Private Function GenCall() As Boolean
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim Wstr As String = String.Empty

        Try
            Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View  'Me.Request.QueryString("Mode")

            Wstr = " cRejectionFlag <> 'Y'" + " And cSubjectType<>'" + SubjectType + "'"

            If System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge").Contains(Session(S_LocationCode)) = True Then
                Wstr = "  vLocationcode='" + Session(S_LocationCode) + "' AND cRejectionFlag <> 'Y'" + " And cSubjectType<>'" + SubjectType + "'"
                Me.AutoCompleteExtender1.ContextKey = Me.Session(S_LocationCode)
                Me.AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected"
            Else
                Wstr = " cRejectionFlag <> 'Y'" + " And cSubjectType<>'" + SubjectType + "'"
                Me.AutoCompleteExtender1.ContextKey = System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge")
            End If
            If Me.Request.QueryString("Mode") = "1" Then
                'Vineet'
                If System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge").Contains(Session(S_LocationCode)) = True Then
                    Me.AutoCompleteExtender1.ContextKey = Me.Session(S_LocationCode)
                    Me.AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_InHouse"
                    Me.AutoCompleteExtender1.ContextKey = "View_SubjectMaster" + "#" + Wstr.Trim() + "#"
                    Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                Else
                    Wstr = "vLocationcode Not In(" + System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge") + ") AND cRejectionFlag <> 'Y'"
                    Wstr += " And cSubjectType <>'" + SubjectType + "'"
                    AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_InHouse"
                    Me.AutoCompleteExtender1.ContextKey = "View_SubjectMaster" + "#" + Wstr.Trim() + "#"
                    Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                End If
            End If

            'Vineet'
            'Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View  'Me.Request.QueryString("Mode")
            ''Vineet'
            ''Wstr = "  vLocationcode='" + Session(S_LocationCode) + "' AND cRejectionFlag <> 'Y'" + " And cSubjectType<>'" + SubjectType + "'"
            'Wstr = " cRejectionFlag <> 'Y'" + " And cSubjectType<>'" + SubjectType + "'"
            ''To use it while saving
            'Me.AutoCompleteExtender1.ContextKey = System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge")
            'If Me.Request.QueryString("Mode") = "1" Then
            '    ' Wstr = " vLocationcode='" + Session(S_LocationCode) + "' AND cRejectionFlag <> 'Y'"
            '    'Vineet'
            '    Wstr = "vLocationcode Not In(" + System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge") + ") AND cRejectionFlag <> 'Y'"
            '    Wstr += " And cSubjectType='" + SubjectType + "'"
            '    AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_InHouse"
            '    Me.AutoCompleteExtender1.ContextKey = "View_SubjectMaster" + "#" + Wstr.Trim() + "#"
            '    Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
            'End If
            Me.ViewState(VS_Choice) = Choice
            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If
            pnlScreeningStudyRecords.Visible = False
            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....GenCall")
        Finally
        End Try
    End Function

#End Region

#Region " GenCall_showUI() "

    Private Function GenCall_ShowUI() As Boolean
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try
            Page.Title = ":: Screening Study Record :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Screening Study Record"
            Choice = Me.ViewState("Choice")
            GenCall_ShowUI = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall")
        Finally
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

#Region "FillGrid"

    Private Function FillGrid() As Boolean
        Dim dsSubjectScreeningRecordDetail As New DataSet
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty

        Try

            Me.GV_ScreeningStudyRecords.DataSource = Nothing
            Me.GV_ScreeningStudyRecords.DataBind()
            'To Get Where condition of ScopeVales( Project Type )
            'If Not ObjCommon.GetScopeValueWithCondition(Wstr) Then
            '    Return False
            'End If
            'Vineet'
            If System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge").Contains(Session(S_LocationCode)) = True Then
                Wstr = "vSubjectId ='" + Me.HSubjectId.Value.Trim() + "' And vLocationCode = '" + Me.Session(S_LocationCode) + "'And cStatusIndi <> 'D' Order by convert(datetime,dScreeningDate,103)"
            Else
                Wstr = "vSubjectId ='" + Me.HSubjectId.Value.Trim() + "'And cStatusIndi <> 'D' Order by convert(datetime,dScreeningDate,103)"
            End If
            'Wstr = "vSubjectId ='" + Me.HSubjectId.Value.Trim() + "' And vLocationCode = '" + Me.Session(S_LocationCode) + "'And cStatusIndi <> 'D' Order by convert(datetime,dScreeningDate,103)"
            If Not objHelp.View_SubjectScreeningRecordDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            dsSubjectScreeningRecordDetail, estr) Then
                Return False
            End If

            ViewState("ScreeningRecordsDetails") = dsSubjectScreeningRecordDetail.Tables("VIEW_SUBJECTSCREENINGRECORDDETAIL")

            Me.GV_ScreeningStudyRecords.DataSource = dsSubjectScreeningRecordDetail
            Me.GV_ScreeningStudyRecords.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGVScreeningRecords", "UIGVScreeningRecords(); ", True)

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillGrid")
            Return False
        End Try
    End Function

#End Region

#Region "GRID EVENTS"

    Protected Sub GV_ScreeningStudyRecords_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GV_ScreeningStudyRecords.PageIndex = e.NewPageIndex
        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub

    Protected Sub GV_ScreeningStudyRecords_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or _
                    e.Row.RowType = DataControlRowType.Footer Or _
                    e.Row.RowType = DataControlRowType.Header Then

            'e.Row.Cells(GVC_SrNo).Visible = False
            If ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                'e.Row.Cells(GVC_EditButton).Style("display:none")
                e.Row.Cells(GVC_EditButton).Attributes.Add("style", "display:none")
                ' e.Row.Cells(GVC_EditButton).Visible = False

            ElseIf ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                e.Row.Cells(GVC_XRayDate).Attributes.Add("style", "display:none")
                e.Row.Cells(GVC_NextXRayDate).Attributes.Add("style", "display:none")
                e.Row.Cells(GVC_ScreeningFUDate).Attributes.Add("style", "display:none")
                e.Row.Cells(GVC_ScreeningFUDate2).Attributes.Add("style", "display:none")
                e.Row.Cells(GVC_Remark).Attributes.Add("style", "display:none")
                e.Row.Cells(GVC_SubjectNo).Attributes.Add("style", "display:none")
                e.Row.Cells(GVC_StudyCompleted).Attributes.Add("style", "display:none")
                e.Row.Cells(GVC_ReasonForNonCompletion).Attributes.Add("style", "display:none")
                e.Row.Cells(GVC_VerifiedBy).Attributes.Add("style", "display:none")
                e.Row.Cells(GVC_CheckedBy).Attributes.Add("style", "display:none")

            End If


            e.Row.Cells(GVC_SubjectScreeningRecordNo).Attributes.Add("style", "display:none")
            e.Row.Cells(GVC_TranNo).Attributes.Add("style", "display:none")
        End If
    End Sub

    Protected Sub GV_ScreeningStudyRecords_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            pnlScreeningStudyRecords.Visible = True
            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (Me.GV_ScreeningStudyRecords.PageSize * Me.GV_ScreeningStudyRecords.PageIndex) + 1
            '=========For setting the value in the drop down

            Dim Rowindex As Integer = CType(e.Row.Cells(GVC_SrNo).Text, Integer)
            CType(e.Row.Cells(GVC_Selected).FindControl("ddlIsSelected"), DropDownList).SelectedValue = _
            CType(ViewState(VS_ScreeningRecordsDetails), DataTable).Rows(Rowindex - 1)("cSelectedFlag").ToString
            CType(e.Row.Cells(GVC_Selected).FindControl("ddlIsParticipated"), DropDownList).SelectedValue = _
            CType(ViewState(VS_ScreeningRecordsDetails), DataTable).Rows(Rowindex - 1)("cEnrolledFlag").ToString
            CType(e.Row.Cells(GVC_Selected).FindControl("ddlStudyCompleted"), DropDownList).SelectedValue = _
            CType(ViewState(VS_ScreeningRecordsDetails), DataTable).Rows(Rowindex - 1)("cStudyCompleted").ToString

            '=====for disabling all the  rows other than Row being editing
            If GV_ScreeningStudyRecords.EditIndex <> e.Row.RowIndex Then
                CType(e.Row.Cells(GVC_Selected).FindControl("ddlIsSelected"), DropDownList).Enabled = False
                CType(e.Row.Cells(GVC_Selected).FindControl("ddlIsParticipated"), DropDownList).Enabled = False
                CType(e.Row.Cells(GVC_Selected).FindControl("ddlStudyCompleted"), DropDownList).Enabled = False
            End If

        End If
    End Sub

    Protected Sub GV_ScreeningStudyRecords_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GV_ScreeningStudyRecords.RowCancelingEdit
        'Reset the edit index.
        GV_ScreeningStudyRecords.EditIndex = -1
        'Bind data to the GridView control.
        GV_ScreeningStudyRecords.DataSource = CType(ViewState(VS_ScreeningRecordsDetails), DataTable)
        GV_ScreeningStudyRecords.DataBind()

    End Sub

    Protected Sub GV_ScreeningStudyRecords_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GV_ScreeningStudyRecords.RowEditing
        GV_ScreeningStudyRecords.EditIndex = e.NewEditIndex
        'Bind data to the GridView control.

        GV_ScreeningStudyRecords.DataSource = CType(ViewState(VS_ScreeningRecordsDetails), DataTable)
        GV_ScreeningStudyRecords.DataBind()

    End Sub

    Protected Sub GV_ScreeningStudyRecords_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GV_ScreeningStudyRecords.RowUpdating
        UpdateValues(e)
        btnSave.Visible = True
        GV_ScreeningStudyRecords.EditIndex = -1
        'Bind data to the GridView control.
        GV_ScreeningStudyRecords.DataSource = CType(ViewState(VS_ScreeningRecordsDetails), DataTable)
        GV_ScreeningStudyRecords.DataBind()
    End Sub

#End Region

#Region "Button Events"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetSubject.Click
        If ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
            btnAddNewRecord.Visible = True
        End If
        GV_ScreeningStudyRecords.EditIndex = -1
        If Not FillGrid() Then

            Exit Sub
        End If
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx?mode=4")
    End Sub

#End Region

#Region "Update Screeing records values"
    Sub UpdateValues(ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs)
        Dim dt As New DataTable
        dt = CType(ViewState("ScreeningRecordsDetails"), DataTable)
        'Update the values.
        Dim GvRow As GridViewRow = GV_ScreeningStudyRecords.Rows(e.RowIndex)
        Dim estr As String = ""
        '((TextBox)(row.Cells(1).Controls(0))).Text

        Try

            If Not Request.Form(GvRow.Cells(GVC_ScreeningDate).Controls.Item(0).UniqueID) Is Nothing AndAlso Request.Form(GvRow.Cells(GVC_ScreeningDate).Controls.Item(0).UniqueID).ToString <> "" Then
                dt.Rows(GvRow.DataItemIndex)("dScreeningDate") = Request.Form(GvRow.Cells(GVC_ScreeningDate).Controls.Item(0).UniqueID).ToString
            End If


            If Not Request.Form(GvRow.Cells(GVC_ScreeningRecordedBy).Controls.Item(0).UniqueID) Is Nothing Then
                dt.Rows(GvRow.DataItemIndex)("vScreeningRecordedBy") = Request.Form(GvRow.Cells(GVC_ScreeningRecordedBy).Controls.Item(0).UniqueID).ToString
            End If
            'If   andalso Request.Form(GvRow.Cells(GVC_XRayDate).Controls.Item(0).UniqueID).ToString.Trim <> "" Then
            '    dt.Rows(GvRow.DataItemIndex)("dXRayDate") = Convert.ToDateTime(Request.Form(GvRow.Cells(GVC_XRayDate).Controls.Item(0).UniqueID).ToString)
            'End If
            'If Request.Form(GvRow.Cells(GVC_NextXRayDate).Controls.Item(0).UniqueID).ToString.Trim <> "" Then
            '    dt.Rows(GvRow.DataItemIndex)("dNextXRayDate") = Convert.ToDateTime(Request.Form(GvRow.Cells(GVC_NextXRayDate).Controls.Item(0).UniqueID).ToString)
            'End If

            'If Request.Form(GvRow.Cells(GVC_ScreeningFUDate).Controls.Item(0).UniqueID).ToString.Trim <> "" Then
            '    dt.Rows(GvRow.DataItemIndex)("dScreeningFUDate") = Convert.ToDateTime(Request.Form(GvRow.Cells(GVC_ScreeningFUDate).Controls.Item(0).UniqueID))
            'End If
            'If Request.Form(GvRow.Cells(GVC_ScreeningFUDate2).Controls.Item(0).UniqueID).ToString.Trim <> "" Then
            '    dt.Rows(GvRow.DataItemIndex)("dScreeningFUDate2") = Convert.ToDateTime(Request.Form(GvRow.Cells(GVC_ScreeningFUDate2).Controls.Item(0).UniqueID))
            'End If
            'dt.Rows(GvRow.DataItemIndex)("vRemark") = Request.Form(GvRow.Cells(GVC_Remark).Controls.Item(0).UniqueID)
            dt.Rows(GvRow.DataItemIndex)("cSelectedFlag") = CType(GvRow.FindControl("ddlIsSelected"), DropDownList).SelectedValue

            If Not Request.Form(GvRow.Cells(GVC_ReasonForRejection).Controls.Item(0).UniqueID) Is Nothing Then
                dt.Rows(GvRow.DataItemIndex)("vReasonForRejection") = Request.Form(GvRow.Cells(GVC_ReasonForRejection).Controls.Item(0).UniqueID)
            End If


            dt.Rows(GvRow.DataItemIndex)("cEnrolledFlag") = CType(GvRow.FindControl("ddlIsParticipated"), DropDownList).SelectedValue

            If Not Request.Form(GvRow.Cells(GVC_ReasonForNotParticipated).Controls.Item(0).UniqueID) Is Nothing Then
                dt.Rows(GvRow.DataItemIndex)("vReasonForNotParticipated") = Request.Form(GvRow.Cells(GVC_ReasonForNotParticipated).Controls.Item(0).UniqueID)
            End If

            If Not Request.Form(GvRow.Cells(GVC_ProjectNo).Controls.Item(0).UniqueID) Is Nothing Then
                dt.Rows(GvRow.DataItemIndex)("vProjectNo") = Request.Form(GvRow.Cells(GVC_ProjectNo).Controls.Item(0).UniqueID)
            End If
            'dt.Rows(GvRow.DataItemIndex)("vSubjectNo") = Request.Form(GvRow.Cells(GVC_SubjectNo).Controls.Item(0).UniqueID)
            ' dt.Rows(GvRow.DataItemIndex)("vRecordedBy") = Request.Form(GvRow.Cells(GVC_RecordedBy).Controls.Item(0).UniqueID)
            If Not Request.Form(GvRow.Cells(GVC_LastSampleDate).Controls.Item(0).UniqueID) Is Nothing AndAlso Request.Form(GvRow.Cells(GVC_LastSampleDate).Controls.Item(0).UniqueID) <> "" Then
                dt.Rows(GvRow.DataItemIndex)("dLastSampleDate") = Request.Form(GvRow.Cells(GVC_LastSampleDate).Controls.Item(0).UniqueID)
            End If
            'dt.Rows(GvRow.DataItemIndex)("cStudyCompleted") = CType(GvRow.FindControl("ddlStudyCompleted"), DropDownList).SelectedValue
            'dt.Rows(GvRow.DataItemIndex)("vReason") = Request.Form(GvRow.Cells(GVC_ReasonForNonCompletion).Controls.Item(0).UniqueID)
            If Not Request.Form(GvRow.Cells(GVC_NoOfDays).Controls.Item(0).UniqueID) Is Nothing AndAlso Request.Form(GvRow.Cells(GVC_NoOfDays).Controls.Item(0).UniqueID) <> "" Then
                dt.Rows(GvRow.DataItemIndex)("nNoOfDays") = Integer.Parse(Request.Form(GvRow.Cells(GVC_NoOfDays).Controls.Item(0).UniqueID))
            End If

            If Not Request.Form(GvRow.Cells(GVC_Eligibledate).Controls.Item(0).UniqueID) Is Nothing AndAlso Request.Form(GvRow.Cells(GVC_Eligibledate).Controls.Item(0).UniqueID) <> "" Then
                dt.Rows(GvRow.DataItemIndex)("dEligibledate") = Request.Form(GvRow.Cells(GVC_Eligibledate).Controls.Item(0).UniqueID)
            End If

            If Not Request.Form(GvRow.Cells(GVC_SampleRecordedBy).Controls.Item(0).UniqueID) Is Nothing Then
                dt.Rows(GvRow.DataItemIndex)("vSampleRecordedBy") = Request.Form(GvRow.Cells(GVC_SampleRecordedBy).Controls.Item(0).UniqueID)
            End If
            'dt.Rows(GvRow.DataItemIndex)("vVerifiedBy") = Request.Form(GvRow.Cells(GVC_VerifiedBy).Controls.Item(0).UniqueID)
            'dt.Rows(GvRow.DataItemIndex)("vCheckedBy") = Request.Form(GvRow.Cells(GVC_CheckedBy).Controls.Item(0).UniqueID)
            dt.Rows(GvRow.DataItemIndex)("iModifyBy") = Me.Session(S_UserID)
            dt.Rows(GvRow.DataItemIndex)("dModifyOn") = DateTime.Now
            dt.TableName = "SubjectScreeningRecordDetail"
            dt.AcceptChanges()

            ViewState("ScreeningRecordsDetails") = dt

        Catch ex As Exception
            ObjCommon.ShowAlert("Error Occured while Updating Records." + ex.Message, Me.Page)
        End Try



    End Sub
#End Region

#Region "Add New Row & Button save"
    Protected Sub btnAddNewRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNewRecord.Click
        '==========For Adding a New Row of Subject if Details of given subject Not Found in SubjectScreeningRecordDetail Table

        Dim dt_ScreeningRcdDtl As DataTable
        Dim dr As DataRow
        dt_ScreeningRcdDtl = CType(ViewState(VS_ScreeningRecordsDetails), DataTable)
        dr = dt_ScreeningRcdDtl.NewRow
        dr("nSubjectScreeningRecordNo") = 0
        dr("dScreeningDate") = Date.Today.GetDateTimeFormats()(10)
        dr("vSubjectId") = Me.HSubjectId.Value.Trim()
        dr("vScreeningRecordedBy") = Me.Session(S_UserName)
        dr("vRecordedBy") = Me.Session(S_UserName)
        dr("vSampleRecordedBy") = Me.Session(S_UserName)
        dr("nNoOfDays") = 90
        dt_ScreeningRcdDtl.Rows.Add(dr)
        dt_ScreeningRcdDtl.AcceptChanges()
        ViewState("ScreeningRecordsDetails") = dt_ScreeningRcdDtl

        'Bind data to the GridView control.


        GV_ScreeningStudyRecords.EditIndex = GV_ScreeningStudyRecords.Rows.Count
        GV_ScreeningStudyRecords.DataSource = CType(ViewState(VS_ScreeningRecordsDetails), DataTable)
        GV_ScreeningStudyRecords.DataBind()
        btnSave.Visible = True
        'GV_ScreeningStudyRecords.EditIndex = GV_ScreeningStudyRecords.Rows.c
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim dsUpdate, dsNew As New DataSet
        Dim dt As DataTable
        Dim estr As String = String.Empty
        Dim dv As DataView

        Try
            GV_ScreeningStudyRecords.EditIndex = -1
            btnSave.Visible = False
            dt = CType(ViewState("ScreeningRecordsDetails"), DataTable)
            dv = dt.DefaultView
            dv.RowFilter = "nSubjectScreeningRecordNo <> 0"
            dv.Table.TableName = "SubjectScreeningRecordDetail"
            dsUpdate.Tables.Add(dv.ToTable().Copy())

            dv = dt.DefaultView
            dv.RowFilter = "nSubjectScreeningRecordNo = 0"
            dv.Table.TableName = "SubjectScreeningRecordDetail"
            dsNew.Tables.Add(dv.ToTable().Copy())
            If dsUpdate.Tables(0).Rows.Count > 0 Then
                If dsUpdate.Tables(0).Rows.Count > 0 And Not objLambda.Save_SubjectScreeningRecordDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, dsUpdate, Me.Session(S_UserID), estr) Then
                    ObjCommon.ShowAlert("Error While Updating Record." + estr, Me.Page)
                    Exit Sub
                End If
            End If

            If dsNew.Tables(0).Rows.Count > 0 Then
                If Not objLambda.Save_SubjectScreeningRecordDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, dsNew, Me.Session(S_UserID), estr) Then
                    ObjCommon.ShowAlert("Error While Adding New Record." + estr, Me.Page)
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, estr)
        End Try

        ObjCommon.ShowAlert("Screening Records Details Updated Sucessfully.", Me.Page)
    End Sub

#End Region

    End Class
