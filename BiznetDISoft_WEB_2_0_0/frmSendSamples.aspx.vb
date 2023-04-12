Imports System.Collections.Generic

Partial Class frmSendSamples
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION"
    Dim ObjCommon As New clsCommon
    Private objhelpDb As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtGetSampleDetail As String = "SampleDetail" 'gvwSampleDetail
    Private Const VS_DtSampleTypeSendReceiveDetails As String = "SampleTypeSendReceiveDetails"
    Private Const VS_nSampleTypeDetailNo As String = "nSampleTypeDetailNo"
    Private Const VS_BarcodeFlag As Boolean = False

    Private Const gvw_chkSelect As Integer = 0
    Private Const gvw_nSampleTypeDetailNo As Integer = 1
    Private Const gvw_vSampleBarCode As Integer = 2
    Private Const gvw_vSampleTypedesc As Integer = 3
    Private Const gvw_cSampleStatusflag As Integer = 4
    Private Const gvw_vWorkspacedesc As Integer = 5
    Private Const gvw_vSubjectID As Integer = 6
    Private Const gvw_MySubjectNo As Integer = 7
    Private Const gvw_vSubjectName As Integer = 8
    Private Const gvw_vNodeDisplayName As Integer = 9
    Private Const gvw_dCollectionDateTime As Integer = 10
    Private Const gvD_dCollectionDateTime As Integer = 7
    Private Const gvD_Discarddateandtime As Integer = 9

#End Region

#Region "Form Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Not IsPostBack Then

                Me.rblSelection.Items(0).Selected = True 'Written here because of "reset page" problem.
                Me.pnlProjectSpecific.Visible = False

                GenCall()

            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....Page_Load")
        End Try
    End Sub

#End Region

#Region "GenCall"

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
            Me.ShowErrorMessage(ex.Message, "....GenCall")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_Data"

    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean

        Dim eStr_Retu As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing

        Try

            Choice = Me.ViewState(VS_Choice)

            If Not objHelp.GetSampleTypeSendReceiveDetail("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                  ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            Me.ViewState(VS_DtSampleTypeSendReceiveDetails) = ds.Tables(0)

            ds_DWR_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_Data")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_showUI"

    Private Function GenCall_ShowUI() As Boolean
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = ":: Send Samples  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = String.Empty
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Send Samples"
            Choice = Me.ViewState(VS_Choice)

            'If Not FillGridgvwSampleDetail("", "1") Then

            '    Exit Function
            'End If
            ' commented by prayag and added as below
            Me.pnlgvwSample.Visible = False
            Me.btnSendAll.Visible = False
            Me.lblSendToDivision.Visible = False
            Me.ddlDivision.Visible = False
            Me.txtSampleId.Visible = False
            Me.lblSampleId.Visible = False
            Me.pnlPagging.Visible = False
            Me.txtfromDate.Text = Date.Now.ToString("dd-MMM-yyyy")
            Me.txttoDate.Text = Date.Now.ToString("dd-MMM-yyyy")

            'If Not FillddlDivision() Then
            '    Exit Function
            'End If
            'for filtering the Projects according to user  07-09-2011
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
            '======
            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
        Finally
        End Try
    End Function

#End Region

#Region "FillDropDowns"

    Private Function FillddlDivision() As Boolean
        Dim ds_DeptMst As New Data.DataSet
        Dim estr As String = String.Empty
        Dim dv_DeptMst As New DataView
        Try

            objHelp.GetDeptMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_DeptMst, estr)

            dv_DeptMst = ds_DeptMst.Tables(0).DefaultView
            dv_DeptMst.Sort = "vDeptName"
            Me.ddlDivision.DataSource = dv_DeptMst
            Me.ddlDivision.DataValueField = "vDeptCode"
            Me.ddlDivision.DataTextField = "vDeptName"
            Me.ddlDivision.DataBind()
            Me.ddlDivision.Items.Insert(0, New ListItem("--Select Division--", 0))

            Me.ddlDivDivision.DataSource = dv_DeptMst
            Me.ddlDivDivision.DataValueField = "vDeptCode"
            Me.ddlDivDivision.DataTextField = "vDeptName"
            Me.ddlDivDivision.DataBind()
            Me.ddlDivDivision.Items.Insert(0, New ListItem("--Select Division--", 0))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillddlDivision")
            Return False
        End Try
    End Function

#End Region

#Region "FillGrid"

    Private Function FillGridgvwSampleDetail(ByVal type As String, ByVal pagenumber As String) As Boolean
        Dim ds_gvwSampleDetail As New Data.DataSet
        Dim dt_gvwSampleDetail As New DataTable
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim vWorkspaceId As String = String.Empty
        Dim vLocationCode As String = String.Empty
        Dim SelectRecord As String = String.Empty
        Dim Wstr_Scope As String = String.Empty
        Dim pageSize As String = String.Empty
        Dim Fromdate As String = String.Empty
        Dim ToDate As String = String.Empty
        Dim Parameter As String = String.Empty
        Try

            If Not Me.ViewState(VS_DtGetSampleDetail) Is Nothing Then

                dt_gvwSampleDetail = CType(Me.ViewState(VS_DtGetSampleDetail), DataTable)
                ds_gvwSampleDetail.Tables.Add(dt_gvwSampleDetail)

            Else

                Me.gvwSample.DataSource = Nothing
                Me.gvwSample.DataBind()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetSendSample", "fsetSendSample_Show(); ", True)
                vWorkspaceId = Pro_Screening.ToString()
                SelectRecord = "0"
                pageSize = Me.gvwSample.PageSize.ToString()

                If Not Me.rblSelection.Items(0).Selected Then
                    If type <> "All" Then
                        vWorkspaceId = Me.HProjectId.Value.Trim().ToString()
                    Else
                        SelectRecord = "1"
                    End If
                End If




                If vWorkspaceId = "" Then
                    Me.pnlgvwSample.Visible = False
                    Me.btnSendAll.Visible = False
                    Me.lblSendToDivision.Visible = False
                    Me.ddlDivision.Visible = False
                    Me.txtSampleId.Visible = False
                    Me.lblSampleId.Visible = False
                    Me.pnlPagging.Visible = False
                    Return True
                End If

                If Convert.ToString(HttpContext.Current.Session(S_ScopeValue)).Length > 0 Then
                    Wstr_Scope = HttpContext.Current.Session(S_ScopeValue).ToString()
                Else
                    ObjCommon.ShowAlert("ScopeValue not available.", Me.Page)
                    Return False
                End If
                vLocationCode = Me.Session(S_LocationCode).ToString()
                Wstr_Scope = Wstr_Scope.Replace("'", "")

                Fromdate = Me.txtfromDate.Text.ToString()
                ToDate = Me.txttoDate.Text.ToString()

                ' added fromdate and todate by prayag
                'If Not Me.objHelp.Proc_SendSamples(vWorkspaceId, vLocationCode, Wstr_Scope, SelectRecord, pageSize, pagenumber, Fromdate, ToDate, ds_gvwSampleDetail, estr) Then
                '    Me.ShowErrorMessage("Error While Getting Data From proc_sendsample", estr)
                '    Return False
                'End If

                ' added for short time due to not chnage in web servise

                Parameter = vWorkspaceId + "##" + vLocationCode + "##" + Wstr_Scope + "##" + SelectRecord + "##" + pageSize + "##" + pagenumber + "##" + Fromdate + "##" + ToDate + "##" + Session(S_UserID).ToString
                ds_gvwSampleDetail = Me.objhelpDb.ProcedureExecute("dbo.Proc_SendSamples", Parameter.ToString)

            End If

            If ds_gvwSampleDetail Is Nothing Then
                ObjCommon.ShowAlert("No data found.", Me.Page)
                Return False
            End If

            If ds_gvwSampleDetail.Tables.Count = 0 Then
                ObjCommon.ShowAlert("No data found.", Me.Page)
                Return False
            End If

            If ds_gvwSampleDetail.Tables(0).Rows.Count > 0 Then
                If hndLockStatus.Value.Trim() = "Lock" And chkAllProjects.Checked = False Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "DisabledData", "DisabledData();", True)
                End If
                If hndLockStatus.Value.Trim() = "UnLock" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "EnabledData", "EnabledData();", True)
                End If
                Me.gvwSample.DataSource = ds_gvwSampleDetail.Tables(0)
                Me.gvwSample.DataBind()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetSendSample", "fsetSendSample_Show(); ", True)
                Me.ViewState(VS_DtGetSampleDetail) = ds_gvwSampleDetail.Tables(0)
                If Not Me.PopulatePager(ds_gvwSampleDetail.Tables(0).Rows(0)("TotalRecord").ToString(), pagenumber) Then
                    Return False
                End If

            End If

            If Me.gvwSample.Rows.Count < 1 Then
                Me.pnlgvwSample.Visible = False
                Me.btnSendAll.Visible = False
                Me.lblSendToDivision.Visible = False
                Me.ddlDivision.Visible = False
                Me.txtSampleId.Visible = False
                Me.lblSampleId.Visible = False
                Me.pnlPagging.Visible = False
                UpdatePanel1.Update()
                Return True
            End If

            Me.pnlgvwSample.Visible = True
            Me.btnSendAll.Visible = True
            Me.lblSendToDivision.Visible = True
            Me.ddlDivision.Visible = True
            Me.txtSampleId.Visible = True
            Me.lblSampleId.Visible = True
            Me.pnlPagging.Visible = True
            Me.txtSampleId.Focus()
            UpdatePanel1.Update()

            'for fix header of gridview
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "FixHeader", "FixHeader(" & Me.gvwSample.ClientID & ");", True)
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetSendSample", "fsetSendSample_Show(); ", True)
            Return True


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillGridgvwSampleDetail")
            Return False
        End Try
    End Function

    Private Function FillDiscardDetail() As Boolean

        ' addded by prayag
        Dim dt_DtSampleTypeSendReceiveDetails As New DataTable
        Dim ds As New DataSet
        Dim eStr_Retu As String = String.Empty
        Dim Parameter As String = String.Empty
        Dim Fromdate As String
        Dim Todate As String
        Try
            Fromdate = Me.txtfromDate.Text
            Todate = Me.txttoDate.Text

            Parameter = Fromdate + "##" + Todate + "##" + Session(S_UserID).ToString()

            ds = Me.objhelpDb.ProcedureExecute("dbo.Proc_CancelSampleTypeDetail", Parameter.ToString)

            If Not ds Is Nothing Then
                If ds.Tables(0).Rows.Count = 0 Then
                    ObjCommon.ShowAlert("No Record Found!", Me)
                    Return False
                    Exit Function
                End If
            Else
                ObjCommon.ShowAlert("Error While calling Proc_CancelSampleTypeDetail", Me)
                Return False
                Exit Function
            End If
            Me.PannelgvDiscard.Visible = True
            Me.gvDiscardSample.DataSource = ds.Tables(0)
            Me.gvDiscardSample.DataBind()
            UpdatePanel1.Update()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetSendSample", "fsetSendSample_Show(); ", True)

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....FillDiscardDetail")
            Return False
        End Try
    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSend_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            Me.ddlDivDivision.SelectedIndex = Me.ddlDivision.SelectedIndex

            If Me.txtSampleId.Text = "Barcode" Or Me.txtSampleId.Text <> "" Then
                Me.txtSampleId.Text = ""
                Exit Sub
            End If

            Me.btnDivSend.Focus()
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Show", "ShowElement('" + CType(sender, Button).ClientID + "','" + divSend.ClientID + "');", True)

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...btnSend_Click")
        End Try
    End Sub

    Protected Sub btnSendAll_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dt_DtSampleTypeSendReceiveDetails As New DataTable
        Dim ds_save As New DataSet
        Dim eStr_Retu As String = String.Empty
        Try


            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                AssignValues("AddAll")
            End If

            dt_DtSampleTypeSendReceiveDetails = CType(Me.ViewState(VS_DtSampleTypeSendReceiveDetails), DataTable)
            ds_save = New DataSet
            ds_save.Tables.Add(dt_DtSampleTypeSendReceiveDetails.Copy())
            ds_save.Tables(0).TableName = "SampleTypeSendReceiveDetail"

            If Not objLambda.Save_SampleTypeSendReceiveDetail(Me.ViewState(VS_Choice), ds_save, Me.Session(S_UserID), eStr_Retu) Then
                ObjCommon.ShowAlert("Error While Sending SampleTypeSendReceiveDetail", Me.Page)
                Exit Sub
            End If

            If ds_save.Tables(0).Rows.Count > 0 Then
                ObjCommon.ShowAlert("Selected Sample Sent Succesfully.", Me.Page)
            Else
                ObjCommon.ShowAlert("No Sample Is Selected.", Me.Page)
            End If

            'ObjCommon.ShowAlert(" Sample Sent Successfully", Me.Page)

            Me.ViewState(VS_DtGetSampleDetail) = Nothing

            If Me.HProjectId.Value = "All" Then
                If Not FillGridgvwSampleDetail("All", "1") Then
                    Exit Sub
                End If
            Else
                If Not FillGridgvwSampleDetail("", "1") Then
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
            Me.chkAllProjects.Checked = False


            'If Not FillGridgvwSampleDetail("", "1") Then
            '    Exit Sub
            'End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....btnSetProject_Click")
        End Try
    End Sub

    Protected Sub btnDivSend_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dt_DtSampleTypeSendReceiveDetails As New DataTable
        Dim ds_save As New DataSet
        Dim eStr_Retu As String = String.Empty
        Try

            Me.txtSampleId.Text = ""

            If Me.ddlDivDivision.SelectedIndex.ToString() = "0" Then
                ObjCommon.ShowAlert("Select Division", Me.Page)
                Exit Sub
            End If

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                AssignValues("AddSingle")
            End If

            dt_DtSampleTypeSendReceiveDetails = CType(Me.ViewState(VS_DtSampleTypeSendReceiveDetails), DataTable)
            ds_save = New DataSet
            ds_save.Tables.Add(dt_DtSampleTypeSendReceiveDetails.Copy())
            ds_save.Tables(0).TableName = "SampleTypeSendReceiveDetail"

            If Not objLambda.Save_SampleTypeSendReceiveDetail(Me.ViewState(VS_Choice), ds_save, Me.Session(S_UserID), eStr_Retu) Then
                ObjCommon.ShowAlert("Error While Sending SampleTypeSendReceiveDetail", Me.Page)
                Exit Sub
            End If

            ObjCommon.ShowAlert("Sample Sent Successfully", Me.Page)

            Me.ViewState(VS_DtGetSampleDetail) = Nothing

            If Me.HProjectId.Value = "All" Then
                If Not FillGridgvwSampleDetail("All", "1") Then
                    Exit Sub
                End If
            Else
                If Not FillGridgvwSampleDetail("", "1") Then
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr_Retu)
        End Try
    End Sub

    Protected Sub btnDivClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.txtSampleId.Text = ""
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Hide", "HideElement('" + divSend.ClientID + "');", True)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetSendSample", "fsetSendSample_Show(); ", True)
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.txtproject.Text = ""
        Me.HProjectId.Value = ""
        Me.ddlDivision.SelectedIndex = 0
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
      
        If rblSelection.SelectedValue = "00" Then
            If Not FillGridgvwSampleDetail("", "1") Then
                Exit Sub
            End If
            Exit Sub
        End If

        If rblSelection.SelectedValue = "01" Then

            If Me.chkAllProjects.Checked = True Then
                Me.HProjectId.Value = "All"
                Me.txtproject.Text = ""
                If Not FillGridgvwSampleDetail("All", "1") Then
                    Exit Sub
                End If

                Exit Sub
            Else
                If Not FillGridgvwSampleDetail("", "1") Then
                    Exit Sub
                End If
                Exit Sub
            End If
        End If

        If rblSelection.SelectedValue = "02" Then
            If Not FillDiscardDetail() Then
                Exit Sub
            End If
            Exit Sub
        End If
       
    End Sub

#End Region

#Region "Assign Values"

    Private Sub AssignValues(ByVal mode As String, Optional ByVal index As Integer = -1)

        Dim drSendReceive As DataRow
        Dim dt_DtSampleTypeSendReceiveDetails As New DataTable
        Dim estr_Retu As String = String.Empty
        Dim IndexSample As Integer = 0
        Dim chkSample As CheckBox
        Dim ds As New DataSet
        Try

            dt_DtSampleTypeSendReceiveDetails = CType(Me.ViewState(VS_DtSampleTypeSendReceiveDetails), DataTable)
            dt_DtSampleTypeSendReceiveDetails.Clear()

            If mode.ToUpper.Trim() = "ADDSINGLE" Then

                If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                    'Inserting to SampleTypeSendReceiveDetail
                    drSendReceive = dt_DtSampleTypeSendReceiveDetails.NewRow()
                    drSendReceive("nSampleTypeSendReceiveDetailNo") = 1
                    drSendReceive("nSampleTypeDetailNo") = Me.ViewState(VS_nSampleTypeDetailNo).ToString
                    drSendReceive("vSendBYDept") = Me.Session(S_DeptCode)
                    drSendReceive("vSendToDept") = Me.ddlDivDivision.SelectedItem.Value.ToString()
                    drSendReceive("iSentUser") = Me.Session(S_UserID)
                    drSendReceive("cSampleStatusFlag") = Sample_Sent
                    drSendReceive("iModifyBy") = Me.Session(S_UserID)
                    drSendReceive("cStatusIndi") = "N"
                    dt_DtSampleTypeSendReceiveDetails.Rows.Add(drSendReceive)
                    dt_DtSampleTypeSendReceiveDetails.AcceptChanges()

                End If

            ElseIf mode.ToUpper.Trim() = "ADDALL" Then

                For IndexSample = 0 To Me.gvwSample.Rows.Count - 1

                    chkSample = Me.gvwSample.Rows(IndexSample).FindControl("chkSelectSample")
                    If Not chkSample Is Nothing AndAlso chkSample.Checked Then

                        'Inserting to SampleTypeSendReceiveDetail
                        drSendReceive = dt_DtSampleTypeSendReceiveDetails.NewRow()
                        drSendReceive("nSampleTypeSendReceiveDetailNo") = IndexSample
                        drSendReceive("nSampleTypeDetailNo") = Me.gvwSample.Rows(IndexSample).Cells(gvw_nSampleTypeDetailNo).Text
                        drSendReceive("vSendBYDept") = Me.Session(S_DeptCode)
                        drSendReceive("vSendToDept") = Me.ddlDivision.SelectedItem.Value.ToString()
                        drSendReceive("iSentUser") = Me.Session(S_UserID)
                        drSendReceive("cSampleStatusFlag") = Sample_Sent
                        drSendReceive("iModifyBy") = Me.Session(S_UserID)
                        drSendReceive("cStatusIndi") = "N"
                        dt_DtSampleTypeSendReceiveDetails.Rows.Add(drSendReceive)
                        dt_DtSampleTypeSendReceiveDetails.AcceptChanges()

                    End If
                Next IndexSample

            ElseIf mode.ToUpper.Trim() = "REJECTSINGLE" Then

                If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                    'Inserting to SampleTypeSendReceiveDetail
                    drSendReceive = dt_DtSampleTypeSendReceiveDetails.NewRow()
                    drSendReceive("nSampleTypeSendReceiveDetailNo") = 1
                    drSendReceive("nSampleTypeDetailNo") = Me.ViewState(VS_nSampleTypeDetailNo).ToString
                    drSendReceive("vSendBYDept") = Me.Session(S_DeptCode)
                    drSendReceive("vSendToDept") = Me.ddlDivDivision.SelectedItem.Value.ToString()
                    drSendReceive("iSentUser") = Me.Session(S_UserID)
                    drSendReceive("vRemark") = CType(Me.gvwSample.Rows(index).FindControl("txtDisputeRemark"), TextBox).Text.Trim()
                    drSendReceive("cSampleStatusFlag") = Sample_Disputed
                    drSendReceive("iModifyBy") = Me.Session(S_UserID)
                    drSendReceive("cStatusIndi") = "D"
                    dt_DtSampleTypeSendReceiveDetails.Rows.Add(drSendReceive)
                    dt_DtSampleTypeSendReceiveDetails.AcceptChanges()

                End If

            End If

            Me.ViewState(VS_DtSampleTypeSendReceiveDetails) = dt_DtSampleTypeSendReceiveDetails

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignValues")
        End Try
    End Sub

#End Region

#Region "ResetPage"

    Protected Sub resetpage()

        Me.HProjectId.Value = ""
        Me.chkAllProjects.Checked = False
        Me.ddlDivision.DataSource = Nothing
        Me.ddlDivision.DataBind()
        Me.ddlDivDivision.DataSource = Nothing
        Me.ddlDivDivision.DataBind()
        Me.ViewState(VS_Choice) = Nothing
        Me.ViewState(VS_DtGetSampleDetail) = Nothing
        Me.ViewState(VS_DtSampleTypeSendReceiveDetails) = Nothing
        Me.pnlgvwSample.Visible = False
        Me.gvwSample.DataSource = Nothing
        Me.gvwSample.DataBind()
        Me.gvDiscardSample.DataSource = Nothing
        Me.gvDiscardSample.DataBind()
        Me.PannelgvDiscard.Visible = False
        UpdatePanel1.Update()

        GenCall()
    End Sub

#End Region

#Region "Grid Events"

    Protected Sub gvwSample_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then

            'e.Row.Cells(gvw_dCollectionDateTime).Text = CType(e.Row.Cells(gvw_dCollectionDateTime).Text, DateTime).ToString("dd-MMM-yyyy HH:mm:ss tt") + strServerOf

            CType(e.Row.FindControl("btnSend"), Button).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("btnSend"), Button).CommandName = "Send"

            CType(e.Row.FindControl("btnDispute"), Button).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("btnDispute"), Button).CommandName = "Dispute"

            Me.pnlgvwSample.Height = 300
            Me.pnlgvwSample.ScrollBars = ScrollBars.Vertical

            If Not Me.gvwSample.Rows.Count >= 10 Then
                Me.pnlgvwSample.Height = Me.gvwSample.Height
                Me.pnlgvwSample.ScrollBars = ScrollBars.None
            End If

        End If

    End Sub

    Protected Sub gvwSample_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or _
                    e.Row.RowType = DataControlRowType.Header Or _
                    e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(gvw_nSampleTypeDetailNo).Visible = False
            e.Row.Cells(gvw_cSampleStatusflag).Visible = False

        End If
    End Sub

    Protected Sub gvwSample_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim i As Integer = e.CommandArgument
        Dim dt_DtSampleTypeSendReceiveDetails As New DataTable
        Dim ds_save As New DataSet
        Dim eStr_Retu As String = String.Empty

        If Me.ViewState(VS_BarcodeFlag) = False Then

            If e.CommandName.ToUpper = "SEND" AndAlso txtSampleId.Text = "" Then

                If Not CType(Me.gvwSample.Rows(i).FindControl("chkSelectSample"), CheckBox).Checked Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetSendSample", "fsetSendSample_Show(); ", True)
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Hide", "HideElement('" + divSend.ClientID + "');", True)
                    Me.ObjCommon.ShowAlert("No Sample Is Selected.", Me.Page())
                    Exit Sub
                End If
                Me.ViewState(VS_nSampleTypeDetailNo) = Nothing
                Me.ViewState(VS_nSampleTypeDetailNo) = Me.gvwSample.Rows(i).Cells(gvw_nSampleTypeDetailNo).Text.Trim()

            ElseIf e.CommandName.ToUpper = "DISPUTE" Then

                If CType(Me.gvwSample.Rows(i).FindControl("txtDisputeRemark"), TextBox).Text.Trim() = "" Then
                    Me.ObjCommon.ShowAlert("Please Enter Remarks", Me.Page())
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsetSendSample", "fsetSendSample_Show(); ", True)
                    Exit Sub
                End If

                Me.ViewState(VS_nSampleTypeDetailNo) = Nothing
                Me.ViewState(VS_nSampleTypeDetailNo) = Me.gvwSample.Rows(i).Cells(gvw_nSampleTypeDetailNo).Text.Trim()


                AssignValues("RejectSingle", i)

                dt_DtSampleTypeSendReceiveDetails = CType(Me.ViewState(VS_DtSampleTypeSendReceiveDetails), DataTable)
                ds_save = New DataSet
                ds_save.Tables.Add(dt_DtSampleTypeSendReceiveDetails.Copy())
                ds_save.Tables(0).TableName = "SampleTypeSendReceiveDetail"

                If Not objLambda.Save_SampleTypeSendReceiveDetail(Me.ViewState(VS_Choice), ds_save, Me.Session(S_UserID), eStr_Retu) Then
                    ObjCommon.ShowAlert("Error While Sending SampleTypeSendReceiveDetail", Me.Page)
                    Exit Sub
                End If

                'ObjCommon.ShowAlert(" Sample Sent Successfully", Me.Page) changed by prayag
                ObjCommon.ShowAlert(" Sample Discard Successfully!", Me.Page)


                Me.ViewState(VS_DtGetSampleDetail) = Nothing

                If Me.HProjectId.Value = "All" Then
                    If Not FillGridgvwSampleDetail("All", "1") Then
                        Exit Sub
                    End If
                Else
                    If Not FillGridgvwSampleDetail("", "1") Then
                        Exit Sub
                    End If
                End If


            End If

        End If

        Me.txtSampleId.Text = ""
        Me.ViewState(VS_BarcodeFlag) = False

    End Sub


    Protected Sub gvDiscardSample_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then

            'e.Row.Cells(gvD_dCollectionDateTime).Text = CType(e.Row.Cells(gvD_dCollectionDateTime).Text, DateTime).ToString("dd-MMM-yyyy HH:mm:ss tt") + strServerOffset

            'e.Row.Cells(gvD_Discarddateandtime).Text = CType(e.Row.Cells(gvD_Discarddateandtime).Text, DateTime).ToString("dd-MMM-yyyy HH:mm:ss tt") + strServerOffset

            Me.PannelgvDiscard.Height = 300
            Me.PannelgvDiscard.ScrollBars = ScrollBars.Vertical

            If Not Me.gvDiscardSample.Rows.Count >= 10 Then
                Me.PannelgvDiscard.Height = Me.gvDiscardSample.Height
                Me.PannelgvDiscard.ScrollBars = ScrollBars.None
            End If

        End If

    End Sub

    Protected Sub gvDiscardSample_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or _
                    e.Row.RowType = DataControlRowType.Header Or _
                    e.Row.RowType = DataControlRowType.Footer Then


        End If
    End Sub
    'Protected Sub gvwSample_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
    '    gvwSample.PageIndex = e.NewPageIndex
    '    If Me.HProjectId.Value = "All" Then
    '        If Not FillGridgvwSampleDetail("All") Then
    '            Exit Sub
    '        End If
    '    Else
    '        If Not FillGridgvwSampleDetail("") Then
    '            Exit Sub
    '        End If
    '    End If
    'End Sub

#End Region

#Region "CheckBox Events"

    Protected Sub chkAllProjects_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try


            Me.ViewState(VS_DtGetSampleDetail) = Nothing

            'If Me.chkAllProjects.Checked = True Then

            '    Me.HProjectId.Value = "All"
            '    Me.txtproject.Text = ""
            '    If Not FillGridgvwSampleDetail("All", "1") Then
            '        Exit Sub
            '    End If

            '    Exit Sub

            'End If

            'Me.HProjectId.Value = Nothing
            'If Not FillGridgvwSampleDetail("", "1") Then
            '    Exit Sub
            'End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...chkAllProjects_CheckedChanged")
        End Try
    End Sub

#End Region

#Region "TextBox Events"

    Protected Sub txtSampleId_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dt_gvwSample As DataTable
        Dim dr As DataRow
        Dim index As Integer = 0
        Dim isSample As Boolean = False
        Try
            Me.ViewState(VS_BarcodeFlag) = True


            dt_gvwSample = Me.ViewState(VS_DtGetSampleDetail)

            Me.ddlDivDivision.SelectedIndex = Me.ddlDivision.SelectedIndex
            Me.btnDivSend.Focus()
            For Each dr In dt_gvwSample.Rows

                If dr("vSampleBarCode") = txtSampleId.Text.Trim() Then
                    isSample = True
                    Me.ViewState(VS_nSampleTypeDetailNo) = Nothing
                    Me.ViewState(VS_nSampleTypeDetailNo) = Me.gvwSample.Rows(index).Cells(gvw_nSampleTypeDetailNo).Text.Trim()
                    Me.lblSendTo.Text = Me.txtSampleId.Text + " Send To Division"
                    Me.txtSampleId.Text = "Barcode"
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Show", "ShowElement('" + CType(sender, TextBox).ClientID + "','" + divSend.ClientID + "');", True)
                    Exit Sub
                End If
                index += 1
            Next
            '=========Added on 17-Nov-09 for validation=======
            If isSample = False And txtSampleId.Text.Trim() <> "" Then
                ObjCommon.ShowAlert("Entered SampleId Does Not Present!", Me.Page)
            End If
            '====================================================
            Me.txtSampleId.Text = "0"
            If Me.rblSelection.Items(0).Selected Then
                btnSend_Click(sender, e)
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...txtSampleId_TextChanged")
        End Try
    End Sub

#End Region

#Region "Radio Button Events"

    Protected Sub rblSelection_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Me.resetpage()

            If Me.rblSelection.Items(2).Selected = True Then ' added by prayag
                Me.pnlProjectSpecific.Visible = False
                Exit Sub
            End If

            If Me.rblSelection.Items(0).Selected = False Then
                Me.pnlProjectSpecific.Visible = True
                Exit Sub
            End If

            Me.pnlProjectSpecific.Visible = False
            

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....rblSelection_SelectedIndexChanged")
        End Try
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

    Private Function PopulatePager(ByVal recordCount As Integer, ByVal currentPage As Integer) As Boolean
        Try
            rptPager.DataSource = Nothing
            rptPager.DataBind()
            Dim PageSize As String = Me.gvwSample.PageSize.ToString()
            Dim dblPageCount As Double = CType((CType(recordCount, Decimal) / Decimal.Parse(PageSize)), Double)
            Dim pageCount As Integer = CType(Math.Ceiling(dblPageCount), Integer)
            Dim pages As New List(Of ListItem)
            If (pageCount > 0) Then
                Dim i As Integer = 1
                Do While (i <= pageCount)
                    pages.Add(New ListItem(i.ToString, i.ToString, (i <> currentPage)))
                    i = (i + 1)
                Loop
            End If
            rptPager.DataSource = pages
            rptPager.DataBind()
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....PopulatePager")
            Return False
        End Try
    End Function

    Protected Sub Page_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim pageIndex As Integer = Integer.Parse(CType(sender, LinkButton).CommandArgument)
            Me.ViewState(VS_DtGetSampleDetail) = Nothing
            If Me.HProjectId.Value = "All" Then
                If Not FillGridgvwSampleDetail("All", pageIndex.ToString()) Then
                    Exit Sub
                End If
            Else
                If Not FillGridgvwSampleDetail("", pageIndex.ToString()) Then
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....Page_Changed")
        End Try
    End Sub

    'Add by shivani pandya for Project lock
#Region "LockImpact"
    <Services.WebMethod()> _
    Public Shared Function LockImpact(ByVal WorkspaceID As String) As String
        Dim ds_Check As DataSet = Nothing
        Dim dtProjectStatus As New DataTable
        Dim ProjectStatus As String = String.Empty
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim iTran As String = String.Empty
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()

        wStr = "vWorkspaceId = '" + WorkspaceID.ToString() + "' And cStatusIndi <> 'D'"

        If Not objHelp.GetCRFLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_Check, eStr) Then
            Throw New Exception(eStr)
        End If

        If ds_Check.Tables(0).Rows.Count > 0 Then

            dtProjectStatus = ds_Check.Tables(0)
            iTran = dtProjectStatus.Compute("Max(iTranNo)", String.Empty)
            dtProjectStatus.DefaultView.RowFilter = "iTranNo =" + iTran.ToString()

            If dtProjectStatus.DefaultView.ToTable.Rows.Count > 0 Then
                ProjectStatus = dtProjectStatus.DefaultView.ToTable().Rows(0)("cLockFlag").ToString()
            End If
        Else
            ProjectStatus = "U"
        End If

        Return ProjectStatus
    End Function
#End Region

End Class