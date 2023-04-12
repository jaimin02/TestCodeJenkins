Imports Newtonsoft.Json
Imports System.Web.Services
Imports System.Drawing

Partial Class frmReasonMst
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objlambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()

    Private Const VS_ReasonNo As String = "vReasonNo"
    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtReasonMst As String = "DtReasonMst"
    Private Const VS_DtView_ReasonMst As String = "DtView_ReasonMst"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_ReasonNo As Integer = 1
    Private Const GVC_ActivityId As Integer = 2
    Private Const GVC_ActivityName As Integer = 3
    Private Const GVC_ReasonDesc As Integer = 4
    Private Const GVC_Edit As Integer = 7
    Private Const GVC_Delete As Integer = 6
    Private Const GVC_cStatusIndi As Integer = 5

    Private Const Cons_Data_Edition As String = "Data Edition"
    Private Const Cons_Subject_Rejection As String = "Subject Rejection"
    Private Const DataEdition As Integer = 1
    Private Const SubjectRejection As Integer = 2

    Private Shared Gv_index As Integer = 0
    Private Shared index As Integer = 0
    Private rPage As RepoPage


    Private Shared Status As String = String.Empty


#End Region

#Region "Form Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
            End If
        Catch ex As Exception

        End Try



    End Sub
#End Region

#Region "GenCall"
    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Dim dt_ActivityMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try
            Choice = Me.Request.QueryString("mode")
            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            Me.ViewState(VS_ReasonNo) = 0

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_ReasonNo) = Me.Request.QueryString("value").ToString
            End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtReasonMst) = ds.Tables("ReasonMst")   ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideReasonDetails", "HideReasonDetails(); ", True)
            End If
            GenCall = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...GenCall")
        Finally
        End Try
    End Function
#End Region

#Region "GenCall_Data"
    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean

        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Val As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing

        Try

            Val = Me.ViewState(VS_ReasonNo) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "nReasonNo=" + Val.ToString
            End If

            If Not objHelp.GetReasonMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            If ds.Tables(0).Rows.Count <= 0 And _
             Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found for Selected Operation")
            End If

            ds_DWR_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCallData")

        Finally
        End Try
    End Function
#End Region

#Region "GenCall_ShowUI"
    Private Function GenCall_ShowUI() As Boolean
        Dim dt_ReasonMst As DataTable = Nothing
        Dim dt_ViewReasonMst As DataTable = Nothing
        Dim dv_ViewReasonMst As DataView = Nothing
        Dim dr As DataRow
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = ":: Reason Master :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Reason Master"

            dt_ReasonMst = Me.ViewState(VS_DtReasonMst)

            Choice = Me.ViewState(VS_Choice)

            If Not FillGrid(dt_ViewReasonMst) Then
                Exit Function
            End If

            If Not FillActivityDropDown() Then
                Exit Function
            End If

            'Added for selected index

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                'This is done because we do not have ActivityName in the table so we take Data from View
                dv_ViewReasonMst = dt_ViewReasonMst.DefaultView
                dv_ViewReasonMst.RowFilter = "nReasonNo=" + Me.ViewState(VS_ReasonNo).ToString.Trim()
                dt_ViewReasonMst = Nothing
                dt_ViewReasonMst = New DataTable
                dt_ViewReasonMst = dv_ViewReasonMst.ToTable()
                dr = dt_ViewReasonMst.Rows(0)


                '' Commented By Dharmesh on 03-Apr-2011 ''
                'Me.txtActivity.Text = dr("vActivityName")

                Me.TxtReason.Text = dr("vReasonDesc")
                'added by vishal 
                Me.DdlListActivity.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_ReasonMst.Rows(0)("vActivityId"), dt_ReasonMst.Rows(0)("vActivityId").GetType)


                '' Commented By Dharmesh on 03-Apr-2011 ''
                'Me.HActivityId.Value = dr("vActivityId")

            End If

            'ResetPage()
            'Me.BtnSave.Attributes.Add("OnClick", "return Validation();")

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "........GenCall_ShowUI")
        Finally
        End Try
    End Function

#End Region


    '' Added By Dharmesh H.Salla
#Region "Fill Activity Drop Down List"

    Public Function FillActivityDropDown() As Boolean
        Dim Wstr As String = String.Empty
        Dim Estr As String = String.Empty
        Dim Ds_Activity As New DataSet
        Dim Dv_ActivityName As DataView
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Choice = Me.ViewState(VS_Choice)

            Wstr = "vProjectTypeCode in(" & Session(S_ScopeValue) & ")"
            If Not objHelp.GetActivityCodeDetails(Ds_Activity, Wstr, Estr) Then
                Throw New Exception(Estr)
            End If

            If Ds_Activity.Tables(0).Rows.Count = 0 Then
                Throw New Exception("No Any Activity Found.")
            End If

            Dv_ActivityName = Ds_Activity.Tables(0).DefaultView().ToTable(True, "vActivityName,vActivityId".Split(", ")).DefaultView()
            Dv_ActivityName.Sort = "vActivityName"
            DdlListActivity.DataSource = Dv_ActivityName.ToTable()
            DdlListActivity.DataTextField = "vActivityName"
            DdlListActivity.DataValueField = "vActivityId"
            DdlListActivity.DataBind()
            Me.DdlListActivity.Items.Insert(0, New ListItem(" Select Sequence No", ""))
            'DdlListActivity.Items.Insert(0, "Select Activity")

            'If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

            'Me.DdlListActivity.SelectedValue = Me.ViewState(VS_ReasonNo)


            'If Me.Request.QueryString("ActName").ToString() = Cons_Data_Edition Then
            '    DdlListActivity.SelectedIndex = DataEdition
            'Else
            '    DdlListActivity.SelectedIndex = SubjectRejection
            'End If

            'End If

            Return True

        Catch ex As Exception
            Me.objcommon.ShowAlert(ex.Message.ToString, Me.Page)
            Exit Function
        End Try

    End Function

#End Region

    '' **** '''''''''''''''''''''''''''

#Region "FillGrid"
    Private Function FillGrid(ByRef dt_ViewReasonMst As DataTable) As Boolean
        Dim ds_Reason As New Data.DataSet
        Dim dv_Reason As New Data.DataView
        Dim estr As String = ""
        Dim wStr As String = ""
        Try
            wStr = "vProjectTypeCode in(" & Session(S_ScopeValue) & ")"

            If Not objHelp.View_ReasonMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                        ds_Reason, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From ViewReasonMst", estr)
                Return False
            End If

            If ds_Reason.Tables(0).Rows.Count < 1 Then
                objcommon.ShowAlert("No Record Found", Me.Page)
                Return True
            End If
            dv_Reason = ds_Reason.Tables(0).DefaultView()
            dv_Reason.Sort = "vActivityName,vReasonDesc"
            Me.GV_Reason.DataSource = dv_Reason.ToTable()
            Me.GV_Reason.DataBind()

            dt_ViewReasonMst = dv_Reason.ToTable()
            Try
                'If GV_Reason.Rows.Count > 0 Then
                '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvReasonMst", "UIgvReasonMst(); ", True)   ''Added by ketan
                'End If
            Catch ex As Exception

            End Try
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function
#End Region

#Region "Button Events"

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim ds_Save As New DataSet
        Dim dt_Save As New DataTable
        Dim estr As String = String.Empty
        Dim message As String = String.Empty
        Try
            If Not AssignValues() Then
                Exit Sub
            End If
            ds_Save = New DataSet
            dt_Save = CType(Me.ViewState(VS_DtReasonMst), DataTable)
            dt_Save.TableName = "ReasonMst"
            ds_Save.Tables.Add(dt_Save)

            If Not objlambda.Save_ReasonMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_ReasonMst, _
                    ds_Save, Me.Session(S_UserID), estr) Then
                objcommon.ShowAlert("Error While Saving UserMst", Me.Page)
                Exit Sub
            End If

            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "Reason Details Saved Successfully !", "Reason Details Updated Successfully !")
            ResetPage()
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('" + message + "','1')", True)
            If Not GenCall() Then
                objcommon.ShowAlert("Error while getting reasons", Me.Page())
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub
    Protected Sub btnRemarksUpdate_Click(sender As Object, e As EventArgs)
        Dim ds_Delete As New DataSet
        Dim estr As String = ""
        Dim dr As DataRow
        Dim dt_Reason As DataTable
        Dim wStr As String = String.Empty

        index = Me.GV_Reason.Rows(Gv_index).Cells(GVC_ReasonNo).Text.Trim()


        Try
            wStr = "nReasonNo=" & index
            If Not objHelp.GetReasonMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_Delete, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From ReasonMst", estr)
                Exit Sub
            End If
            dt_Reason = ds_Delete.Tables(0)
            For Each dr In dt_Reason.Rows
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("vRemarks") = Me.txtRemarks_delete.Text.Trim()
                dr("cStatusIndi") = "D"
                dr.AcceptChanges()
            Next
            dt_Reason.AcceptChanges()
            dt_Reason.TableName = "ReasonMst"
            ds_Delete = Nothing
            ds_Delete = New DataSet
            ds_Delete.Tables.Add(dt_Reason.Copy())
            If Not objlambda.Save_ReasonMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_ReasonMst, _
                    ds_Delete, Me.Session(S_UserID), estr) Then
                objcommon.ShowAlert("Error While Deleting ReasonMst", Me.Page)
                Exit Sub
            End If
            ResetPage()
            objcommon.ShowAlert("Record Deleted Successfully !", Me.Page)
            txtRemarks_delete.Text = Nothing

            If Not Me.GenCall() Then
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")

        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ResetPage()
        Me.Response.Redirect("frmReasonMst.aspx?mode=1")
    End Sub

#End Region

#Region "Assign values"

    Private Function AssignValues() As Boolean
        Dim dr As DataRow
        Dim dt_User As New DataTable
        Dim ds_Check As New DataSet
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Try

            'For validating Duplication

            '' Changed By Dharmesh on 03-Apr-2011 ''
            wstr = "cStatusIndi <> 'D' And vActivityId='" & Me.DdlListActivity.SelectedValue.Trim() & "'"

            wstr += " And vReasonDesc='" & Me.TxtReason.Text.Trim() & "'"

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                wstr += " And nReasonNo <> " + Me.ViewState(VS_ReasonNo).ToString()
            End If

            If Not objHelp.GetReasonMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Check, estr) Then

                Me.ShowErrorMessage("Error While Getting Data From ReasonMst", estr)
                Exit Function

            End If

            If ds_Check.Tables(0).Rows.Count > 0 Then

                objcommon.ShowAlert("Reason Already Exists for Selected Activity", Me.Page)
                Exit Function

            End If
            '***********************************

            dt_User = CType(Me.ViewState(VS_DtReasonMst), DataTable)
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                dt_User.Clear()
                dr = dt_User.NewRow()
                'nReasonNo,vActivityId,vReasonDesc,iModifyBy,dModifyOn,cStatusIndi
                dr("nReasonNo") = 0

                '' Changed By Dharmesh on 03-Apr-2011 ''
                dr("vActivityId") = Me.DdlListActivity.SelectedValue.Trim()
                dr("vReasonDesc") = Me.TxtReason.Text.Trim()
                dr("vRemarks") = Me.txtRemarks.Text.Trim()
                '' Changed By Dharmesh on 03-Apr-2011 ''
                dr("vActivityId") = Me.DdlListActivity.SelectedValue.Trim()

                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dt_User.Rows.Add(dr)

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                For Each dr In dt_User.Rows

                    '' Changed By Dharmesh on 03-Apr-2011 ''
                    dr("vActivityId") = Me.DdlListActivity.SelectedValue.Trim()

                    dr("vReasonDesc") = Me.TxtReason.Text.Trim()
                    dr("vRemarks") = Me.txtRemarks.Text.Trim()
                    '' Changed By Dharmesh on 03-Apr-2011 ''
                    ''dr("vActivityId") = Me.DdlListActivity.SelectedValue.Trim()

                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr.AcceptChanges()
                Next
                dt_User.AcceptChanges()


            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete Then 'Added by simki

                For Each dr In dt_User.Rows
                    dr("vActivityId") = Me.DdlListActivity.SelectedValue.Trim()
                    dr("vReasonDesc") = Me.TxtReason.Text.Trim()
                    dr("vRemarks") = Me.txtRemarks_delete.Text.Trim()
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "D"
                    dr.AcceptChanges()
                Next
                dt_User.AcceptChanges()
            End If



            Me.ViewState(VS_DtReasonMst) = dt_User
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "Reset Page"

    Private Sub ResetPage()
        Try
            '' Changed By Dharmesh on 03-Apr-2011 ''
            Me.DdlListActivity.SelectedIndex = -1

            Me.TxtReason.Text = ""

            '' Commented By Dharmesh on 03-Apr-2011 ''
            'Me.HActivityId.Value = ""

            Me.ViewState(VS_DtReasonMst) = Nothing
            'Me.Response.Redirect("frmReasonMst.aspx?mode=1", False)

            'Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

            'If Not Me.GenCall() Then
            '    Exit Sub
            'End If

        Catch ex As Exception
            'Me.ShowErrorMessage(ex.Message)
        End Try
    End Sub

#End Region

#Region "Grid Event"

    'Protected Sub GV_Reason_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GV_Reason.PageIndexChanging
    '    Dim dt_ViewReasonMst As New DataTable
    '    GV_Reason.PageIndex = e.NewPageIndex
    '    If Not FillGrid(dt_ViewReasonMst) Then
    '        Exit Sub
    '    End If
    'End Sub

    Protected Sub GV_Reason_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Try
            Gv_index = e.CommandArgument
            Dim wStr As String = ""
            Dim Ds As DataSet = New Data.DataSet
            Dim eStr As String = String.Empty
            Dim Dr As DataRow

            index = Me.GV_Reason.Rows(Gv_index).Cells(GVC_ReasonNo).Text.Trim()
            If e.CommandName.ToUpper = "EDIT" Then

                wStr = "nReasonNo=" & index
                If Not objHelp.GetReasonMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds, eStr) Then
                    objcommon.ShowAlert(eStr, Me.Page())
                End If

                If Ds.Tables(0).Rows.Count > 0 Then
                    Dr = Ds.Tables(0).Rows(0)
                    Me.DdlListActivity.SelectedValue = Dr.Item("vActivityId")
                    Me.TxtReason.Text = Dr.Item("vReasonDesc")
                    Me.ViewState(VS_ReasonNo) = Dr.Item("nReasonNo")
                    Me.BtnSave.Text = "Update"
                    Me.BtnSave.ToolTip = "Update"
                End If

                Me.ViewState(VS_DtReasonMst) = Ds.Tables(0)
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

            ElseIf e.CommandName.ToUpper = "DELETE" Then
                Status = "DELETE"

                btnRemarksUpdate.Text = "Delete"
                btnRemarksUpdate.ToolTip = "Delete"
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete


                Gv_index = e.CommandArgument
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Function DeleteReason(ByVal index As Integer) As Boolean
        Dim wstr As String = ""
        Dim ds_Delete As New DataSet
        Dim estr As String = ""
        Dim dr As DataRow
        Dim dt_Reason As DataTable
        ' Dim ds As DataSet

        Try
            wstr = "nReasonNo=" & Me.GV_Reason.Rows(index).Cells(GVC_ReasonNo).Text.Trim()

            If Not objHelp.GetReasonMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_Delete, estr) Then

                Me.ShowErrorMessage("Error While Getting Data From ReasonMst", estr)
                Exit Function

            End If
            dt_Reason = ds_Delete.Tables(0)
            For Each dr In dt_Reason.Rows
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("vRemarks") = Me.txtRemarks_delete.Text.Trim()
                dr("cStatusIndi") = "D"
                dr.AcceptChanges()
            Next
            dt_Reason.AcceptChanges()
            dt_Reason.TableName = "ReasonMst"

            ds_Delete = Nothing
            ds_Delete = New DataSet
            ds_Delete.Tables.Add(dt_Reason.Copy())
            If Not objlambda.Save_ReasonMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_ReasonMst, _
                    ds_Delete, Me.Session(S_UserID), estr) Then
                objcommon.ShowAlert("Error While Deleting ReasonMst", Me.Page)
                Exit Function
            End If

            ResetPage()
            objcommon.ShowAlert("Record Deleted SuccessFully", Me.Page)
            txtRemarks_delete.Text = Nothing
            'Me.DdlListActivity.SelectedIndex = 0
            'Me.TxtReason.Text = ""
            'Me.ViewState(VS_DtReasonMst) = Nothing
            'Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

            If Not Me.GenCall() Then
                Exit Function
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function


    Protected Sub GV_Reason_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Try


            e.Row.Cells(GVC_ReasonNo).Visible = False
            e.Row.Cells(GVC_ActivityId).Visible = False
            e.Row.Cells(GVC_cStatusIndi).Visible = False


            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + 1 + (Me.GV_Reason.PageSize * Me.GV_Reason.PageIndex)
                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "Edit"
                CType(e.Row.FindControl("lnkDelete"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkDelete"), ImageButton).CommandName = "Delete"
                CType(e.Row.FindControl("lnkAudit"), ImageButton).Attributes.Add("nReasonNo", e.Row.Cells(GVC_ReasonNo).Text.Trim)

                If e.Row.Cells(GVC_cStatusIndi).Text = "D" Then
                    e.Row.BackColor = Drawing.Color.FromArgb(255, 189, 121)

                    CType(e.Row.FindControl("lnkDelete"), ImageButton).Enabled = False
                    CType(e.Row.FindControl("lnkEdit"), ImageButton).Enabled = False

                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GV_Reason_RowDataBound")
        End Try
    End Sub

#End Region

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objcommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objcommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub
#End Region

    Protected Sub GV_Reason_PreRender(sender As Object, e As EventArgs) Handles GV_Reason.PreRender
        'Try
        '    If GV_Reason.Rows.Count > 0 Then
        '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvReasonMst", "UIgvReasonMst(); ", True)
        '    End If
        'Catch ex As Exception

        'End Try
    End Sub
    Protected Sub GV_Reason_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GV_Reason.RowDeleting

    End Sub

    Protected Sub GV_Reason_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GV_Reason.RowEditing
        e.Cancel = True
    End Sub

#Region "Web Method"

    <WebMethod> _
    Public Shared Function AuditTrail(ByVal nReasonNo As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_ReasonMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtReasonMstHistrory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable

        Dim vTableName As String = String.Empty
        Dim vIdName As String = String.Empty
        Dim AuditFieldName As String = String.Empty
        Dim AuditFieldValue As String = String.Empty
        Dim vOtherTableName As String = String.Empty
        Dim vOtherIdName As String = String.Empty


        Try
            vTableName = "ReasonMstHistory"
            vIdName = "vActivityId"
            AuditFieldName = "nReasonNo"
            AuditFieldValue = nReasonNo
            vOtherTableName = "ActivityMst"
            vOtherIdName = "vActivityId"


            Dim Param As String = ""


            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName
            If Not objHelp.Proc_GetAuditTrail(wStr, ds_ReasonMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtReasonMstHistrory Is Nothing Then
                dtReasonMstHistrory.Columns.Add("SrNo")
                dtReasonMstHistrory.Columns.Add("ReasonDesc")
                dtReasonMstHistrory.Columns.Add("ActivityName")
                dtReasonMstHistrory.Columns.Add("Remarks")
                dtReasonMstHistrory.Columns.Add("ModifyBy")
                dtReasonMstHistrory.Columns.Add("ModifyOn")
            End If
            dt = ds_ReasonMst.Tables(0)
            Dim dv As New DataView(dt)

            For Each dr As DataRow In dv.ToTable.Rows

                drAuditTrail = dtReasonMstHistrory.NewRow()

                drAuditTrail("SrNo") = i
                drAuditTrail("ReasonDesc") = dr("vreasonDesc").ToString()
                drAuditTrail("ActivityName") = dr("vActivityName").ToString()
                drAuditTrail("Remarks") = dr("vRemarks").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtReasonMstHistrory.Rows.Add(drAuditTrail)
                dtReasonMstHistrory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtReasonMstHistrory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function



    Protected Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Dim wStr As String = String.Empty
        Dim ds_ReasonMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtReasonMstHistrory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable


        Dim filename As String = String.Empty
        Dim vTableName As String = String.Empty
        Dim vIdName As String = String.Empty
        Dim AuditFieldName As String = String.Empty
        Dim AuditFieldValue As String = String.Empty
        Dim vOtherTableName As String = String.Empty
        Dim vOtherIdName As String = String.Empty
        Dim strMessage As New StringBuilder


        vTableName = "ReasonMstHistory"
        vIdName = "vActivityId"
        AuditFieldName = "nReasonNo"
        AuditFieldValue = hdnReasonNo.Value
        vOtherTableName = "ActivityMst"
        vOtherIdName = "vActivityId"

        Try
            Dim Param As String = ""


            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName


            If Not objHelp.Proc_GetAuditTrail(wStr, ds_ReasonMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtReasonMstHistrory Is Nothing Then
                dtReasonMstHistrory.Columns.Add("SrNo")
                dtReasonMstHistrory.Columns.Add("ReasonDesc")
                dtReasonMstHistrory.Columns.Add("Acitivity Name")
                dtReasonMstHistrory.Columns.Add("Remarks")
                dtReasonMstHistrory.Columns.Add("ModifyBy")
                dtReasonMstHistrory.Columns.Add("ModifyOn")
            End If
            dtReasonMstHistrory.AcceptChanges()
            dt = ds_ReasonMst.Tables(0)
            Dim dv As New DataView(dt)

            For Each dr As DataRow In dv.ToTable.Rows

                drAuditTrail = dtReasonMstHistrory.NewRow()

                drAuditTrail("SrNo") = i
                drAuditTrail("ReasonDesc") = dr("vReasonDesc").ToString()
                drAuditTrail("Acitivity Name") = dr("vActivityName").ToString()
                drAuditTrail("Remarks") = dr("vRemarks").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtReasonMstHistrory.Rows.Add(drAuditTrail)
                dtReasonMstHistrory.AcceptChanges()
                i += 1
            Next




            gvExport.DataSource = dtReasonMstHistrory
            gvExport.DataBind()
            If gvExport.Rows.Count > 0 Then

                gvExport.HeaderRow.BackColor = Color.White
                gvExport.FooterRow.BackColor = Color.White
                For Each cell As TableCell In gvExport.HeaderRow.Cells
                    cell.BackColor = gvExport.HeaderStyle.BackColor
                Next
                For Each row As GridViewRow In gvExport.Rows
                    row.BackColor = Color.White
                    row.Height = 20
                    For Each cell As TableCell In row.Cells
                        If row.RowIndex Mod 2 = 0 Then
                            cell.BackColor = gvExport.AlternatingRowStyle.BackColor
                            cell.ForeColor = gvExport.RowStyle.ForeColor
                        Else
                            cell.BackColor = gvExport.RowStyle.BackColor
                            cell.ForeColor = gvExport.RowStyle.ForeColor
                        End If
                        cell.CssClass = "textmode"
                    Next
                Next

                'style to format numbers to string
                Dim style As String = "<style> .textmode { } </style>"
                Response.Write(style)



                Dim info As String = String.Empty
                Dim gridviewHtml As String = String.Empty

                filename = "Audit Trail_" + hdnReasonNo.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                Dim stringWriter As New System.IO.StringWriter()

                Dim writer As New HtmlTextWriter(stringWriter)
                gvExport.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")





                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Reason Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")


                gridviewHtml = strMessage.ToString() + gridviewHtml
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

                Dim info As String = String.Empty
                Dim gridviewHtml As String = String.Empty

                filename = "Audit Trail_" + hdnReasonNo.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"


                drAuditTrail = dtReasonMstHistrory.NewRow()

                drAuditTrail("SrNo") = ""
                drAuditTrail("Reason Desc") = ""
                drAuditTrail("ActivityName") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("ModifyBy") = ""
                drAuditTrail("ModifyOn") = ""
                dtReasonMstHistrory.Rows.Add(drAuditTrail)
                dtReasonMstHistrory.AcceptChanges()
                gvExport.DataSource = dtReasonMstHistrory
                gvExport.DataBind()


                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                gvExport.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()


                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Reason Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")


                gridviewHtml = strMessage.ToString() + gridviewHtml
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
                Exit Sub
            End If
        Catch ex As Exception
            Me.objcommon.ShowAlert("Error While Exporting Data To Excel : " + estr, Me.Page)

            Exit Sub
        End Try

    End Sub



    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub
#End Region

End Class
