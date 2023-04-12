
Partial Class frmReceiveSamples
    Inherits System.Web.UI.Page

#Region " VARIABLE DECLARATION "

    Dim ObjCommon As New clsCommon
    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
    'Dim objIntegration As New WS_Integration.Integration

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtGetSampleDetail As String = "SampleDetail" 'gvwSampleDetail
    Private Const VS_DtSampleTypeSendReceiveDetail As String = "SampleTypeSendReceiveDetail"
    Private Const VS_nSampleTypeDetailNo As String = "nSampleTypeDetailNo"
    Private Const VS_LocationMst As String = "LocationMst"

    Private Const VS_DsHDRDTL As String = "HDRDTL" 'DS to pass to Web Service of "suflam"

    Private Const gvw_chkSelect As Integer = 0
    Private Const gvw_nSampleTypeDetailNo As Integer = 1
    Private Const gvw_vSampleBarCode As Integer = 2
    Private Const gvw_vSampleTypedesc As Integer = 3
    Private Const gvw_cSampleStatusflag As Integer = 4
    Private Const gvw_vWorkspacedesc As Integer = 5
    Private Const gvw_vSubjectID As Integer = 6
    Private Const gvw_vSubjectName As Integer = 7
    Private Const gvw_vNodeDisplayName As Integer = 8
    Private Const gvw_dSendOnDate As Integer = 9
    Private Const gvw_vSendBYUser As Integer = 10
    Private Const gvw_vSendBYDeptName As Integer = 11

#End Region

#Region "Form Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Me.txtSampleId.Focus()
                Me.rblSelection.Items(0).Selected = True 'Written here because of "reset page" problem.
                Me.pnlProjectSpecific.Visible = False
                GenCall()
            End If

        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region " GenCall() "
    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try
            Choice = "1"
            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If
            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "........GenCall")
        Finally
        End Try
    End Function
#End Region

#Region " GenCall_Data "
    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean

        Dim eStr_Retu As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing
        Dim dsSampleType As New DataSet
        Dim dsLocation As New DataSet
        Dim eStr As String
        Dim dvLocation As DataView


        Try
            Choice = Me.ViewState(VS_Choice)

            If Not objHelp.GetSampleTypeSendReceiveDetail("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                  ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            Me.ViewState(VS_DtSampleTypeSendReceiveDetail) = ds.Tables(0)

            If Not objHelp.GetSampleTypeMst(" cStatusIndi <> 'D' ", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsSampleType, eStr) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            If Not objHelp.getLocationMst("  cStatusIndi <> 'D' ", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsLocation, eStr) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If
            ViewState(VS_LocationMst) = dsLocation.Tables(0)
            dvLocation = dsLocation.Tables(0).DefaultView()
            dvLocation.RowFilter = "cLocationType='L'"

            ddlSampleType.DataSource = dsSampleType
            ddlSampleType.DataTextField = "vSampleTypeDesc"
            ddlSampleType.DataValueField = "cSampleTypeCode"
            ddlSampleType.DataBind()
            ddlSampleType.Items.Insert(0, New ListItem("<--Select Sample Type-->"))

            ddlLocationSite.DataSource = dvLocation.ToTable()
            ddlLocationSite.DataTextField = "vLocationName"
            ddlLocationSite.DataValueField = "vLocationCode"
            ddlLocationSite.DataBind()
            ddlLocationSite.Items.Insert(0, New ListItem("<-Select Location->"))
            ds_DWR_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_Data")

        Finally

        End Try
    End Function
#End Region

#Region " GenCall_showUI() "
    Private Function GenCall_ShowUI() As Boolean
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = ":: Receive Samples  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Receive Samples"
            Choice = Me.ViewState(VS_Choice)

            Me.btnReceiveAll.Visible = False
            Me.txtFromDate.Text = Date.Now.ToString("dd-MMM-yyyy")
            Me.txtToDate.Text = Date.Now.ToString("dd-MMM-yyyy")

            If Not FillGridgvwSampleDetail("") Then
                Return False
            End If



            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
        Finally
        End Try
    End Function
#End Region

#Region "FillGrid"

    Private Function FillGridgvwSampleDetail(ByVal type As String) As Boolean
        Dim ds_gvwSampleDetail As New Data.DataSet
        Dim dt_gvwSampleDetail As New DataTable
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim Wstr_Scope As String = String.Empty
        Dim dvSampleDetail, dvSampleDetailsAll As DataView

        Try
            If Not Me.ViewState(VS_DtGetSampleDetail) Is Nothing Then


                dvSampleDetail = CType(Me.ViewState(VS_DtGetSampleDetail), DataTable).DefaultView
                If ddlSampleType.SelectedIndex > 0 Then
                    dvSampleDetail.RowFilter = "cSampleTypeCode='" + ddlSampleType.SelectedValue + "'"
                    dvSampleDetail.Sort = "vSampleBarCode,cSampleTypeCode"
                End If
                If ddlLocationSite.SelectedIndex > 0 Then
                    dvSampleDetail.RowFilter = "vLocationCode='" + ddlLocationSite.SelectedValue + "'"
                    'dvSampleDetail.Sort = "vSampleBarCode,cSampleTypeCode"
                End If

                dt_gvwSampleDetail = dvSampleDetail.ToTable
                ds_gvwSampleDetail.Tables.Add(dt_gvwSampleDetail.Copy())

            Else

                Me.gvwSample.DataSource = Nothing
                Me.gvwSample.DataBind()

                If Me.rblSelection.Items(0).Selected Then

                    wstr = "vWorkspaceId = '" + Pro_Screening + "' And "

                Else
                    wstr = "vWorkspaceId <> '" + Pro_Screening + "' And "

                    If type <> "All" Then

                        wstr = "vWorkspaceId = '" & Me.HProjectId.Value.Trim() & "' And "

                    End If


                End If

                'To Get Where condition of ScopeVales( Project Type )
                If Not ObjCommon.GetScopeValueWithCondition(Wstr_Scope) Then
                    Exit Function
                End If

                'wstr += " (dSendOnDate is Not NULL And dSendOnDate <> '') And " & Wstr_Scope
                'wstr += " And dReceivedOnDate is NULL And vSendToDept='" + Me.Session(S_DeptCode) + "'"
                'wstr += " And (CAST(CONVERT(varchar,dSendOnDate,106) as datetime) between '" + Me.txtFromDate.Text.ToString() + "'"
                'wstr += " And '" + Me.txtToDate.Text.ToString() + "')"
                'wstr += " And (vSubjectId is Not NULL or vSubjectId <> '') "
                'wstr += " And cStatusIndi <> 'D' And (cSampleStatusFlag <> 'D' or cSampleStatusFlag <> 'R')"
                'wstr += " And cSampleStatusFlag = 'S'"

                wstr += Wstr_Scope
                wstr += " And dReceivedOnDate is NULL And vSendToDept='" + Me.Session(S_DeptCode) + "'"
                wstr += " And (CAST(CONVERT(varchar,dSendOnDate,106) as datetime) between '" + Me.txtFromDate.Text.ToString() + "'"
                wstr += " And '" + Me.txtToDate.Text.ToString() + "')"
                wstr += " And cStatusIndi <> 'D'" ' And (cSampleStatusFlag <> 'D' or cSampleStatusFlag <> 'R')"
                wstr += " And cSampleStatusFlag = 'S'"


                'If ddlSampleType.SelectedIndex > 0 Then
                '    wstr += " And cSampleTypeCode='" + ddlSampleType.SelectedValue + "'"
                'End If
                wstr += " Order By vSampleBarCode "
                If Not objHelp.View_SampleTypeSendReceiveDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_gvwSampleDetail, estr) Then
                    Me.ShowErrorMessage("Error While Getting Data From View_SampleDetail", estr)
                    Return False
                End If
                dvSampleDetailsAll = ds_gvwSampleDetail.Tables(0).DefaultView()
                dvSampleDetailsAll.Sort = "cSampleTypeCode"
                Me.ViewState(VS_DtGetSampleDetail) = dvSampleDetailsAll.ToTable()
                ds_gvwSampleDetail = New DataSet
                ds_gvwSampleDetail.Tables.Add(dvSampleDetailsAll.ToTable())
            End If

            Me.gvwSample.DataSource = ds_gvwSampleDetail.Tables(0)
            Me.gvwSample.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetReceiveSample", "fsetReceiveSample_Show(); ", True)

            If Me.gvwSample.Rows.Count < 1 Then
                Me.btnReceiveAll.Visible = False
                Me.txtSampleId.Visible = False
                Me.lblSampleId.Visible = False

            ElseIf Me.gvwSample.Rows.Count > 0 Then
                Me.btnReceiveAll.Visible = True
                Me.txtSampleId.Visible = True
                Me.lblSampleId.Visible = True

                Me.txtSampleId.Focus()
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillGridgvwSampleDetail")
            Return False
        End Try
    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            'Me.btnSearch.Attributes.Add("OnClick", "return Validation();")
            Me.ViewState(VS_DtGetSampleDetail) = Nothing
            If Me.HProjectId.Value = "All" Then
                If Not FillGridgvwSampleDetail("All") Then
                    Exit Sub
                End If
            Else
                If Not FillGridgvwSampleDetail("") Then
                    Exit Sub
                End If
            End If
            If gvwSample.Rows.Count > 0 Then
                'Me.lblSampleType.Visible = True
                'Me.ddlSampleType.Visible = True
                Me.lblLocationSite.Visible = True
                Me.ddlLocationSite.Visible = True
                Me.lblSelect.Visible = True
                Me.ddlSelect.Visible = True
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetReceiveSample", "fsetReceiveSample_Show();", True)
            Else
                Me.lblSampleType.Visible = False
                Me.ddlSampleType.Visible = False
                Me.lblLocationSite.Visible = False
                Me.ddlLocationSite.Visible = False
                Me.lblSelect.Visible = False
                Me.ddlSelect.Visible = False
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetReceiveSample", "fsetReceiveSample_Show();", True)
            End If
            ddlSampleType.SelectedIndex = 0
            Me.txtSampleId.Text = ""
            Me.txtSampleId.Focus()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....btnSearch_Click")
        End Try
    End Sub

    Protected Sub btnReceiveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dt_DtSampleTypeSendReceiveDetail As New DataTable
        Dim ds_save As New DataSet
        Dim eStr_Retu As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_Integration As New DataSet
        Dim ds_HDRDTL As New DataSet
        Dim strOutPut As String = String.Empty
        Try
            Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                AssignValues("AddAll")
            End If

            dt_DtSampleTypeSendReceiveDetail = CType(Me.ViewState(VS_DtSampleTypeSendReceiveDetail), DataTable)
            ds_save = New DataSet
            ds_save.Tables.Add(dt_DtSampleTypeSendReceiveDetail.Copy())
            ds_save.Tables(0).TableName = "SampleTypeSendReceiveDetail"

            ''-----------------------Integration Code Starts--------------------

            'wStr = "nSampleTypeDetailNo in("
            'For Each dr In ds_save.Tables(0).Rows
            '    wStr += dr("nSampleTypeDetailNo").ToString() + ","
            'Next
            'wStr += "0)"
            ''wStr = "nSampleTypeDetailNo = " + ds_save.Tables(0).Rows(0).Item("nSampleTypeDetailNo").ToString()

            ''If Not objHelp.View_SampleTypeSendReceiveDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Integration, eStr_Retu) Then
            ''    Me.ShowErrorMessage("Error While Geeting Data From View_SubjectSampleDetail_LabIntegration", eStr_Retu)
            ''End If

            'If ds_Integration.Tables(0).Rows.Count > 0 Then

            '    If Not CreateTablesHDRDTL() Then
            '        ObjCommon.ShowAlert("Error While Integration", Me.Page)
            '    End If

            '    ds_HDRDTL = CType(Me.ViewState(VS_DsHDRDTL), DataSet)

            '    For Each dr In ds_Integration.Tables(0).Rows

            '        drHDR = ds_HDRDTL.Tables("dtHDR").NewRow()
            '        drHDR("LAB_ID") = dr("nSampleTypeDetailNo")
            '        drHDR("REGISTRATION_DATE") = dr("dEnrollmentDate").ToString()
            '        drHDR("PATIENT_ID") = dr("vSubjectID").ToString()
            '        drHDR("PATIENT_NAME") = dr("FullName").ToString()
            '        drHDR("SAMPLE_DATE") = dr("dCollectionDateTime").ToString()
            '        drHDR("RECEIVED_DATE") = dr("dReceivedOnDate").ToString()
            '        drHDR("GENDER") = dr("cSex").ToString()
            '        drHDR("BIRTH_DATE") = dr("dBirthDate").ToString()
            '        drHDR("SITE_CODE") = dr("vLocationInitiate").ToString()
            '        drHDR("PROJECT_CODE") = dr("vWorkspaceId").ToString()
            '        drHDR("SUBJECT_NO") = dr("iMySubjectNo").ToString()
            '        drHDR("PATIENT_AGE") = dr("Age").ToString()
            '        drHDR("VISIT_TYPE") = dr("vActivityName").ToString()
            '        drHDR("PATIENT_INITIALS") = dr("vInitials").ToString()
            '        drHDR("STATUS") = "N"
            '        drHDR("WEIGHT") = dr("nWeight")
            '        drHDR("USER_ID") = Me.Session(S_UserID).ToString()
            '        drHDR("USER_DATE") = Today.Date.Now.ToString()
            '        drHDR("USER_HOST_NAME") = ""

            '        ds_HDRDTL.Tables("dtHDR").Rows.Add(drHDR)
            '        ds_HDRDTL.Tables("dtHDR").AcceptChanges()

            '        drDTL = ds_HDRDTL.Tables("dtDTL").NewRow()
            '        drDTL("LAB_ID") = dr("vSampleBarCode").ToString()
            '        drDTL("SR_NO") = dr("nSampleTypeDetailNo")
            '        drDTL("TEST_CODE") = dr("vMedExCode").ToString()
            '        ds_HDRDTL.Tables("dtDTL").Rows.Add(drDTL)
            '        ds_HDRDTL.Tables("dtDTL").AcceptChanges()

            '    Next

            '    Me.ViewState(VS_DsHDRDTL) = ds_HDRDTL
            'End If

            'strOutPut = objIntegration.saveSampleRegistration(ds_HDRDTL)

            '-----------------------Integration Code Ends----------------------

            'If strOutPut.ToUpper.Trim() <> "Y" Then

            '    ObjCommon.ShowAlert("Error While Receiving Sample Details", Me.Page)
            '    Exit Sub

            'End If

            If Not objLambda.Save_SampleTypeSendReceiveDetail(Me.ViewState(VS_Choice), ds_save, Me.Session(S_UserID), eStr_Retu) Then
                ObjCommon.ShowAlert("Error While Receiving Sample Details", Me.Page)
                Exit Sub
            Else
                ObjCommon.ShowAlert("Received Successfully", Me.Page)
            End If

            Me.ViewState(VS_DtGetSampleDetail) = Nothing

            If Me.HProjectId.Value = "All" Then
                If Not FillGridgvwSampleDetail("All") Then
                    Exit Sub
                End If
            Else
                If Not FillGridgvwSampleDetail("") Then
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr_Retu)
        End Try

    End Sub

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            Me.ViewState(VS_DtGetSampleDetail) = Nothing
            If Not FillGridgvwSampleDetail("") Then
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnSetProject_Click")
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.txtFromDate.Text = ""
        Me.txtToDate.Text = ""
        Me.txtproject.Text = ""
        Me.HProjectId.Value = ""
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Response.Redirect("frmMainPage.aspx")
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

#Region "Assign Values"
    Private Sub AssignValues(ByVal mode As String, Optional ByVal Index As Integer = -1)

        Dim drSendReceive As DataRow
        Dim dt_DtSampleTypeSendReceiveDetail As New DataTable
        Dim ds As New DataSet
        Dim wstr As String = String.Empty
        Dim estr_Retu As String = String.Empty
        Dim IndexSample As Integer = 0
        Dim chkSample As CheckBox
        Dim nSampleTypeDetailNo As Integer
        Try

            If Index >= 0 Then
                nSampleTypeDetailNo = Me.gvwSample.Rows(Index).Cells(gvw_nSampleTypeDetailNo).Text.Trim()
            End If

            Me.ViewState(VS_DtSampleTypeSendReceiveDetail) = Nothing

            If mode.ToUpper.Trim() = "ADDSINGLE" Or mode.ToUpper.Trim() = "REJECTSINGLE" Or mode.ToUpper.Trim() = "NOTRECEIVEDSINGLE" Then

                wstr = "nSampleTypeDetailNo =" + nSampleTypeDetailNo.ToString()

            ElseIf mode.ToUpper.Trim() = "ADDALL" Then

                wstr = "nSampleTypeDetailNo in("
                For IndexSample = 0 To Me.gvwSample.Rows.Count - 1
                    chkSample = Me.gvwSample.Rows(IndexSample).FindControl("chkSelectSample")
                    If Not chkSample Is Nothing AndAlso chkSample.Checked Then
                        wstr += gvwSample.Rows(IndexSample).Cells(gvw_nSampleTypeDetailNo).Text + ","
                    End If
                Next IndexSample
                wstr += "0)"

            End If

            wstr += " And cStatusIndi <> 'D'"

            If Not objHelp.GetSampleTypeSendReceiveDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                  ds, estr_Retu) Then
                Response.Write(estr_Retu)
                Exit Sub
            End If

            dt_DtSampleTypeSendReceiveDetail = ds.Tables(0)

            'For Each drSendReceive In dt_DtSampleTypeSendReceiveDetail.Rows
            '    'Updating to SampleTypeSendReceiveDetail
            '    'drSendReceive("nSampleTypeDetailNo") = gvwSample.Rows(IndexSample).Cells(gvw_nSampleTypeDetailNo).Text
            '    If mode.ToUpper.Trim() <> "NOTRECEIVEDSINGLE" Then

            '        drSendReceive("dReceivedOnDate") = Today.Date.Now.ToString()
            '        drSendReceive("iReceivedUser") = Me.Session(S_UserID)

            '        drSendReceive("cSampleStatusFlag") = Sample_Received
            '        If mode.ToUpper.Trim() = "REJECTSINGLE" Then
            '            drSendReceive("cSampleStatusFlag") = Sample_Rejected
            '            drSendReceive("vRemark") = CType(Me.gvwSample.Rows(Index).FindControl("txtRejectionRemark"), TextBox).Text.Trim()
            '        End If

            '    ElseIf mode.ToUpper.Trim() = "NOTRECEIVEDSINGLE" Then

            '        drSendReceive("cSampleStatusFlag") = Sample_NotReceived
            '        drSendReceive("vRemark") = CType(Me.gvwSample.Rows(Index).FindControl("txtRejectionRemark"), TextBox).Text.Trim()

            '    End If

            '    drSendReceive("iModifyBy") = Me.Session(S_UserID)
            '    'drSendReceive("dModifyOn") = ""
            '    drSendReceive("cStatusIndi") = "E"
            '    dt_DtSampleTypeSendReceiveDetail.AcceptChanges()
            'Next

            ' comment and chnaged by prayag

            For Each drSendReceive In dt_DtSampleTypeSendReceiveDetail.Rows

                If mode.ToUpper.Trim() <> "NOTRECEIVEDSINGLE" Then

                    drSendReceive("dReceivedOnDate") = Today.Date.Now.ToString()
                    drSendReceive("iReceivedUser") = Me.Session(S_UserID)

                    drSendReceive("cSampleStatusFlag") = Sample_Received
                    drSendReceive("iModifyBy") = Me.Session(S_UserID)
                    drSendReceive("cStatusIndi") = "E"

                    If mode.ToUpper.Trim() = "REJECTSINGLE" Then
                        drSendReceive("dReceivedOnDate") = DBNull.Value
                        drSendReceive("cSampleStatusFlag") = Sample_Disputed
                        drSendReceive("vRemark") = CType(Me.gvwSample.Rows(Index).FindControl("txtRejectionRemark"), TextBox).Text.Trim()
                        drSendReceive("iModifyBy") = Me.Session(S_UserID)
                        drSendReceive("cStatusIndi") = Sample_Disputed
                    End If

                ElseIf mode.ToUpper.Trim() = "NOTRECEIVEDSINGLE" Then

                    drSendReceive("cSampleStatusFlag") = Sample_NotReceived
                    drSendReceive("vRemark") = CType(Me.gvwSample.Rows(Index).FindControl("txtRejectionRemark"), TextBox).Text.Trim()
                    drSendReceive("iModifyBy") = Me.Session(S_UserID)
                    drSendReceive("cStatusIndi") = "E"

                End If

                dt_DtSampleTypeSendReceiveDetail.AcceptChanges()
            Next

            Me.ViewState(VS_DtSampleTypeSendReceiveDetail) = dt_DtSampleTypeSendReceiveDetail

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....AssignValues")
        End Try
    End Sub
#End Region

#Region "ResetPage"
    Protected Sub resetpage()
        'Me.HProjectId.Value = ""

        Me.txtSampleId.Visible = False
        Me.lblSampleId.Visible = False
        Me.ViewState(VS_Choice) = Nothing
        Me.ViewState(VS_DtGetSampleDetail) = Nothing
        Me.ViewState(VS_DtSampleTypeSendReceiveDetail) = Nothing

        Me.txtFromDate.Text = Date.Now.ToString("dd-MMM-yyyy")
        Me.txtToDate.Text = Date.Now.ToString("dd-MMM-yyyy")

        Me.gvwSample.DataSource = Nothing
        Me.gvwSample.DataBind()

        GenCall()
    End Sub
#End Region

#Region "Grid Events"

    Protected Sub gvwSample_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or _
                    e.Row.RowType = DataControlRowType.Header Or _
                    e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(gvw_nSampleTypeDetailNo).Visible = False
            e.Row.Cells(gvw_cSampleStatusflag).Visible = False
            e.Row.Cells(gvw_vSubjectName).Visible = False

        End If
    End Sub

    Protected Sub gvwSample_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then

            e.Row.Cells(gvw_dSendOnDate).Text = CType(e.Row.Cells(gvw_dSendOnDate).Text, DateTime).ToString("dd-MMM-yyyy HH:mm tt")

            CType(e.Row.FindControl("btnReceive"), Button).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("btnReceive"), Button).CommandName = "Receive"

            CType(e.Row.FindControl("btnReject"), Button).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("btnReject"), Button).CommandName = "Reject"

            CType(e.Row.FindControl("btnNotReceived"), Button).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("btnNotReceived"), Button).CommandName = "Not Received"

            'Me.pnlgvwSample.Height = 300
            'Me.pnlgvwSample.ScrollBars = ScrollBars.Vertical
            'If Not Me.gvwSample.Rows.Count >= 10 Then
            '    Me.pnlgvwSample.Height = Me.gvwSample.Height
            '    Me.pnlgvwSample.ScrollBars = ScrollBars.None
            'End If
        End If
    End Sub

    Protected Sub gvwSample_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim dt_DtSampleTypeSendReceiveDetail As New DataTable
        Dim dt_DtSampletemp As New DataTable
        Dim ds_save As New DataSet
        Dim eStr_Retu As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_Integration As New DataSet
        Dim ds_HDRDTL As New DataSet
        Dim drHDR As DataRow
        Dim drDTL As DataRow
        Dim dr As DataRow
        Dim strOutPut As String = String.Empty
        Try
            Dim i As Integer = e.CommandArgument
            If e.CommandName.ToUpper = "RECEIVE" Then

                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

                If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                    AssignValues("AddSingle", i)
                End If

                dt_DtSampleTypeSendReceiveDetail = CType(Me.ViewState(VS_DtSampleTypeSendReceiveDetail), DataTable)
                ds_save = New DataSet
                ds_save.Tables.Add(dt_DtSampleTypeSendReceiveDetail.Copy())
                ds_save.Tables(0).TableName = "SampleTypeSendReceiveDetail"

                If Not objLambda.Save_SampleTypeSendReceiveDetail(Me.ViewState(VS_Choice), ds_save, Me.Session(S_UserID), eStr_Retu) Then
                    ObjCommon.ShowAlert("Error While Receiving Sample Details", Me.Page)
                    Exit Sub
                Else
                    ObjCommon.ShowAlert("Received Successfully", Me.Page)
                End If

            ElseIf e.CommandName.ToUpper = "REJECT" Then

                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit


                If CType(Me.gvwSample.Rows(i).FindControl("txtRejectionRemark"), TextBox).Text.Trim() = "" Then
                    Me.ObjCommon.ShowAlert("Please Enter Remarks", Me.Page())
                    Exit Sub
                End If

                If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                    AssignValues("RejectSingle", i)
                End If

                dt_DtSampleTypeSendReceiveDetail = CType(Me.ViewState(VS_DtSampleTypeSendReceiveDetail), DataTable)

                ' added by prayag for add only one row of status D
                If dt_DtSampleTypeSendReceiveDetail.Rows.Count > 1 Then
                    dt_DtSampletemp = dt_DtSampleTypeSendReceiveDetail.Clone()
                    For Each dr In dt_DtSampleTypeSendReceiveDetail.Rows
                        dt_DtSampletemp.ImportRow(dr)
                        Exit For
                    Next
                    dt_DtSampleTypeSendReceiveDetail = dt_DtSampletemp.Copy
                End If



                ds_save = New DataSet
                ds_save.Tables.Add(dt_DtSampleTypeSendReceiveDetail.Copy())
                ds_save.Tables(0).TableName = "SampleTypeSendReceiveDetail"

                If Not objLambda.Save_SampleTypeSendReceiveDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_save, Me.Session(S_UserID), eStr_Retu) Then
                    ObjCommon.ShowAlert("Error While Rejecting Sample Details", Me.Page)
                    Exit Sub
                Else
                    ObjCommon.ShowAlert(" Sample Discard Successfully!", Me.Page)
                End If

            ElseIf e.CommandName.ToUpper = "NOT RECEIVED" Then

                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit


                If CType(Me.gvwSample.Rows(i).FindControl("txtRejectionRemark"), TextBox).Text.Trim() = "" Then
                    Me.ObjCommon.ShowAlert("Please Enter Remarks", Me.Page())
                    Exit Sub
                End If

                If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                    AssignValues("NotReceivedSingle", i)
                End If

                dt_DtSampleTypeSendReceiveDetail = CType(Me.ViewState(VS_DtSampleTypeSendReceiveDetail), DataTable)
                ds_save = New DataSet
                ds_save.Tables.Add(dt_DtSampleTypeSendReceiveDetail.Copy())
                ds_save.Tables(0).TableName = "SampleTypeSendReceiveDetail"

                If Not objLambda.Save_SampleTypeSendReceiveDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_save, Me.Session(S_UserID), eStr_Retu) Then
                    ObjCommon.ShowAlert("Error While Rejecting Sample Details", Me.Page)
                    Exit Sub
                Else
                    ObjCommon.ShowAlert("Sample Not Received", Me.Page)
                End If

            End If


            Me.ViewState(VS_DtGetSampleDetail) = Nothing

            If Me.HProjectId.Value = "All" Then
                If Not FillGridgvwSampleDetail("All") Then
                    Exit Sub
                End If
            Else
                If Not FillGridgvwSampleDetail("") Then
                    Exit Sub
                End If
            End If


        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr_Retu)
        End Try

    End Sub

    Protected Sub gvwSample_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        gvwSample.PageIndex = e.NewPageIndex
        If Me.HProjectId.Value = "All" Then
            If Not FillGridgvwSampleDetail("All") Then
                Exit Sub
            End If
        Else
            If Not FillGridgvwSampleDetail("") Then
                Exit Sub
            End If
        End If
    End Sub

#End Region

#Region "CheckBox Events"
    Protected Sub chkAllProjects_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Me.chkAllProjects.Checked = True Then
                Me.HProjectId.Value = "All"
                Me.txtproject.Text = ""
            Else
                Me.HProjectId.Value = Nothing
            End If
            Me.btnSearch_Click(sender, e)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....chkAllProjects_CheckedChanged")
        End Try
    End Sub
#End Region

#Region "Creating Tables HDRDTL"

    Private Function CreateTablesHDRDTL() As Boolean
        Dim dtHDR As New DataTable
        Dim dtDTL As New DataTable
        Dim dc As DataColumn
        Dim ds As New DataSet
        Try
            '------------------Creating dtHDR table-------------------
            dc = New DataColumn
            dc.ColumnName = "LAB_ID"
            dc.DataType = GetType(Integer)
            dtHDR.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "REGISTRATION_DATE"
            dc.DataType = GetType(String)
            dtHDR.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "PATIENT_ID"
            dc.DataType = GetType(String)
            dtHDR.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "PATIENT_NAME"
            dc.DataType = GetType(String)
            dtHDR.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "SAMPLE_DATE"
            dc.DataType = GetType(String)
            dtHDR.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "RECEIVED_DATE"
            dc.DataType = GetType(String)
            dtHDR.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "GENDER"
            dc.DataType = GetType(Char)
            dtHDR.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "BIRTH_DATE"
            dc.DataType = GetType(String)
            dtHDR.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "SITE_CODE"
            dc.DataType = GetType(String)
            dtHDR.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "PROJECT_CODE"
            dc.DataType = GetType(String)
            dtHDR.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "SUBJECT_NO"
            dc.DataType = GetType(String)
            dtHDR.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "PATIENT_AGE"
            dc.DataType = GetType(String)
            dtHDR.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "VISIT_TYPE"
            dc.DataType = GetType(String)
            dtHDR.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "PATIENT_INITIALS"
            dc.DataType = GetType(String)
            dtHDR.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "PATIENT_CONDITION"
            dc.DataType = GetType(String)
            dtHDR.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "STATUS"
            dc.DataType = GetType(Char)
            dtHDR.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "WEIGHT"
            dc.DataType = GetType(Double)
            dtHDR.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "USER_ID"
            dc.DataType = GetType(String)
            dtHDR.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "USER_DATE"
            dc.DataType = GetType(String)
            dtHDR.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "USER_HOST_NAME"
            dc.DataType = GetType(String)
            dtHDR.Columns.Add(dc)
            '------------------------------------------------------------
            '------------------Creating dtDTL table-------------------

            dc = New DataColumn
            dc.ColumnName = "LAB_ID"
            dc.DataType = GetType(String)
            dtDTL.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "SR_NO"
            dc.DataType = GetType(Integer)
            dtDTL.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "TEST_CODE"
            dc.DataType = GetType(String)
            dtDTL.Columns.Add(dc)
            '---------------------------------------------------

            ds.Tables.Add(dtHDR.Copy())
            ds.Tables.Add(dtDTL.Copy())
            ds.Tables(0).TableName = "dtHDR"
            ds.Tables(1).TableName = "dtDTL"
            Me.ViewState(VS_DsHDRDTL) = ds

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...CreateTablesHDRDTL")
            Return False
        End Try

    End Function

#End Region

#Region "TextBox Events"

    Protected Sub txtSampleId_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSampleId.TextChanged
        Dim dt_SampleDetail As DataTable
        Dim dr As DataRow
        Dim dt_DtSampleTypeSendReceiveDetail As New DataTable
        Dim ds_save As New DataSet
        Dim eStr_Retu As String = String.Empty
        Dim index As Integer = 0
        Dim flag As Boolean = False 'Used for checking barcode sampleId

        Dim wStr As String = String.Empty
        Dim ds_Integration As New DataSet
        Dim ds_HDRDTL As New DataSet
        Dim drHDR As DataRow
        Dim drDTL As DataRow
        Dim strOutPut As String = String.Empty
        Try

            dt_SampleDetail = Me.ViewState(VS_DtGetSampleDetail)

            For Each dr In dt_SampleDetail.Rows

                If dr("vSampleBarCode") = txtSampleId.Text.Trim() Then
                    flag = True
                    Exit For
                End If
                index += 1
            Next

            Me.txtSampleId.Text = ""
            Me.txtSampleId.Focus()

            If Not flag Then
                Exit Sub
            End If

            Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                AssignValues("AddSingle", index)
            End If

            dt_DtSampleTypeSendReceiveDetail = CType(Me.ViewState(VS_DtSampleTypeSendReceiveDetail), DataTable)
            ds_save = New DataSet
            ds_save.Tables.Add(dt_DtSampleTypeSendReceiveDetail.Copy())
            ds_save.Tables(0).TableName = "SampleTypeSendReceiveDetail"

            ''-----------------------Integration Code Starts--------------------

            'wStr = "nSampleTypeDetailNo = " + ds_save.Tables(0).Rows(0).Item("nSampleTypeDetailNo").ToString()

            ''If Not objHelp.Get_ViewSubjectSampleDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Integration, eStr_Retu) Then
            ''    Me.ShowErrorMessage("Error While Geeting Data From View_SubjectSampleDetail_LabIntegration", eStr_Retu)
            ''End If

            ''If Not objHelp.View_SampleTypeSendReceiveDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Integration, eStr_Retu) Then
            ''    Me.ShowErrorMessage("Error While Geeting Data From View_SubjectSampleDetail_LabIntegration", eStr_Retu)
            ''End If

            'If ds_Integration.Tables(0).Rows.Count > 0 Then

            '    If Not CreateTablesHDRDTL() Then
            '        ObjCommon.ShowAlert("Error While Integration", Me.Page)
            '    End If

            '    ds_HDRDTL = CType(Me.ViewState(VS_DsHDRDTL), DataSet)

            '    For Each dr In ds_Integration.Tables(0).Rows

            '        drHDR = ds_HDRDTL.Tables("dtHDR").NewRow()
            '        drHDR("LAB_ID") = dr("nSampleTypeDetailNo")
            '        drHDR("REGISTRATION_DATE") = dr("dEnrollmentDate").ToString()
            '        drHDR("PATIENT_ID") = dr("vSubjectID").ToString()
            '        drHDR("PATIENT_NAME") = dr("FullName").ToString()
            '        drHDR("SAMPLE_DATE") = dr("dCollectionDateTime").ToString()
            '        drHDR("RECEIVED_DATE") = dr("dReceivedOnDate").ToString()
            '        drHDR("GENDER") = dr("cSex").ToString()
            '        drHDR("BIRTH_DATE") = dr("dBirthDate").ToString()
            '        drHDR("SITE_CODE") = dr("vLocationInitiate").ToString()
            '        drHDR("PROJECT_CODE") = dr("vWorkspaceId").ToString()
            '        drHDR("SUBJECT_NO") = dr("iMySubjectNo").ToString()
            '        drHDR("PATIENT_AGE") = dr("Age").ToString()
            '        drHDR("VISIT_TYPE") = dr("vActivityName").ToString()
            '        drHDR("PATIENT_INITIALS") = dr("vInitials").ToString()
            '        drHDR("STATUS") = "N"
            '        drHDR("WEIGHT") = dr("nWeight")
            '        drHDR("USER_ID") = Me.Session(S_UserID).ToString()
            '        drHDR("USER_DATE") = Today.Date.Now.ToString()
            '        drHDR("USER_HOST_NAME") = ""

            '        ds_HDRDTL.Tables("dtHDR").Rows.Add(drHDR)
            '        ds_HDRDTL.Tables("dtHDR").AcceptChanges()

            '        drDTL = ds_HDRDTL.Tables("dtDTL").NewRow()
            '        drDTL("LAB_ID") = dr("vSampleBarCode").ToString()
            '        drDTL("SR_NO") = dr("nSampleTypeDetailNo")
            '        drDTL("TEST_CODE") = dr("vMedExCode").ToString()
            '        ds_HDRDTL.Tables("dtDTL").Rows.Add(drDTL)
            '        ds_HDRDTL.Tables("dtDTL").AcceptChanges()

            '    Next

            '    Me.ViewState(VS_DsHDRDTL) = ds_HDRDTL
            'End If

            ''strOutPut = objIntegration.saveSampleRegistration(ds_HDRDTL)

            ''If strOutPut.ToUpper.Trim() <> "Y" Then

            ''    ObjCommon.ShowAlert("Error While Receiving Sample Details", Me.Page)
            ''    Exit Sub

            ''End If
            ''-----------------------Integration Code Ends----------------------

            If Not objLambda.Save_SampleTypeSendReceiveDetail(Me.ViewState(VS_Choice), ds_save, Me.Session(S_UserID), eStr_Retu) Then
                ObjCommon.ShowAlert("Error While Receiving SampleTypeSendReceiveDetail", Me.Page)
                Exit Sub
            Else
                ObjCommon.ShowAlert("Sample Received Successfully", Me.Page)
            End If

            Me.ViewState(VS_DtGetSampleDetail) = Nothing

            If Me.HProjectId.Value = "All" Then
                If Not FillGridgvwSampleDetail("All") Then
                    Exit Sub
                End If
            Else
                If Not FillGridgvwSampleDetail("") Then
                    Exit Sub
                End If
            End If

            Me.txtSampleId.Text = ""
            Me.txtSampleId.Focus()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...txtSampleId_TextChanged")
        End Try

    End Sub

#End Region

#Region "RadioButtonList Events"

    Protected Sub rblSelection_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Me.resetpage()
            If Me.rblSelection.Items(0).Selected = False Then
                Me.pnlProjectSpecific.Visible = True
                Me.btnSearch.Attributes.Add("OnClick", "return Validation();")
            Else
                Me.btnSearch.Attributes.Remove("OnClick")
                Me.pnlProjectSpecific.Visible = False
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....rblSelection_SelectedIndexChanged")
        End Try
    End Sub

#End Region

#Region "Drop downs Selected index changed"
    Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSampleType.SelectedIndexChanged
        If Me.HProjectId.Value = "All" Then
            If Not FillGridgvwSampleDetail("All") Then
                Exit Sub
            End If
        Else
            If Not FillGridgvwSampleDetail("") Then
                Exit Sub
            End If
        End If
    End Sub

    Protected Sub ddlLocationSite_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLocationSite.SelectedIndexChanged
        If Me.HProjectId.Value = "All" Then
            If Not FillGridgvwSampleDetail("All") Then
                Exit Sub
            End If
        Else
            If Not FillGridgvwSampleDetail("") Then
                Exit Sub
            End If
        End If
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetReceiveSample", "fsetReceiveSample_Show(); ", True)
    End Sub
    Protected Sub ddlSelect_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSelect.SelectedIndexChanged
        Dim dv As New DataView
        If ddlSelect.SelectedValue = "L" Then
            dv = CType(ViewState(VS_LocationMst), DataTable).DefaultView
            dv.RowFilter = "cLocationType='L'"
            ddlLocationSite.DataSource = dv.ToTable()
            ddlLocationSite.DataBind()
            ddlLocationSite.Items.Insert(0, "<-Select Location->")
        ElseIf ddlSelect.SelectedValue = "S" Then
            dv = CType(ViewState(VS_LocationMst), DataTable).DefaultView
            dv.RowFilter = "cLocationType='S'"
            ddlLocationSite.DataSource = dv.ToTable()
            ddlLocationSite.DataBind()
            ddlLocationSite.Items.Insert(0, "<-Select Site->")
        End If
    End Sub

#End Region

    
    
   
End Class