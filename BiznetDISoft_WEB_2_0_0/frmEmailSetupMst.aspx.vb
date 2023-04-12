Imports Microsoft.Win32
Imports System.Web.Services
Imports Newtonsoft.Json
Imports System.Drawing

Partial Class frmEmailSetupMst
    Inherits System.Web.UI.Page
#Region "Variable Declaration"

    Private objCommon As New clsCommon
    Private objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtEmailSetupMst As String = "DtEmailSetupMst"
    Private Const Vs_EmailSetupId As String = "iEmailSetupId"
    Private Const VS_EmailSetup As String = "EmailSetup"
    Dim eStr As String

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_ProjectNo As Integer = 1
    Private Const GVC_vOperation As Integer = 2
    Private Const GVC_vUserTypeName As Integer = 3
    Private Const GVC_vUserName As Integer = 4
    Private Const GVC_iModifyBy As Integer = 5
    Private Const GVC_dModifyOnT As Integer = 6
    Private Const GVC_iEmailSetupId As Integer = 7
    Private Const GVC_status As Integer = 8

    Dim EmailSetupCol As Collection

    Private Const ValueName As String = "Display"


    Private Shared Gv_index As Integer = 0

    Private Shared index As Integer = 0
    Private Shared iEmailSetupId As String = String.Empty
    Private Shared Status As String = String.Empty

#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.AutoCompleteExtender2.ContextKey = "iUserId = " & Me.Session(S_UserID)
            GenCall()
            Exit Sub
            'Else
            '    If Not FillGrid() Then
            '        objCommon.ShowAlert("Error While Binding Grid", Me.Page)
            '    End If
        End If
    End Sub
#End Region

#Region "GenCall"
    Private Function GenCall() As Boolean
        Try
            Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            If Not GenCall_ShowUI() Then
                Exit Function
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideEmailSetupDetails", "HideEmailSetupDetails(); ", True)
            End If
            GenCall = True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...GenCall")
        Finally
        End Try
    End Function
#End Region

#Region "GenCall_Data "
    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean

        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Val As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing

        Try
            Val = Me.ViewState(Vs_EmailSetupId) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "iEmailSetupId=" + Val.ToString
            End If
            wStr += " And cStatusIndi <> 'D'"

            If Not objhelpDb.GetEmailSetupDeatil(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            If ds.Tables(0).Rows.Count <= 0 And
             Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found For Email Setup")
            End If

            ds_DWR_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..GenCall_Data")

        Finally

        End Try
    End Function
#End Region

#Region "GenCall_ShowUI"
    Private Function GenCall_ShowUI() As Boolean
        Dim dt_EmailsetupMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Try
            CType(Master.FindControl("lblHeading"), Label).Text = "Email Setup "

            Page.Title = ":: Email Setup :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            Me.trRemarks.Attributes.Add("display", "none")
            Me.chkSelectAllUser.Style.Add("display", "none")
            Me.chkSelectAllUser.Attributes.Add("onclick", "CheckBoxListSelection(" + Me.chklstUser.ClientID + ",this);")
            Me.chkSelectAllStudy.Attributes.Add("onclick", "CheckBoxListSelection(" + Me.chklststudy.ClientID + ",this);")
            ' Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID) & " And cWorkspaceType = 'P'"
            'If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
            '    Me.txtproject.Text = Session(S_ProjectName)
            '    Me.HProjectId.Value = Session(S_ProjectId)
            'End If
            Me.btnCancel.Visible = False
            If Not FillOperation() Then
                objCommon.ShowAlert("Error While set Operation dropdown", Me.Page)
                Return False
            End If

            If Not FillProfile() Then
                objCommon.ShowAlert("Error While Getting Profile List", Me.Page)
                Return False
            End If
            If Not FillGrid() Then
                objCommon.ShowAlert("Error While Binding Grid", Me.Page)
                Return False
            End If

            dt_EmailsetupMst = Me.ViewState(VS_DtEmailSetupMst)

            Choice = Me.ViewState(VS_Choice)

            GenCall_ShowUI = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
            Return False
        End Try
    End Function
#End Region

#Region "Fill FillDropdown"
    Private Function FillOperation() As Boolean
        Try
            ddlOperation.Items.Add(New ListItem("Select Operation", 0))
            'ddlOperation.Items.Add(New ListItem("Upload Certificate", 2))
            ddlOperation.Items.Add(New ListItem("Image Upload", 1))
            ddlOperation.Items.Add(New ListItem("Qc1 Review", 2))
            ddlOperation.Items.Add(New ListItem("Qc2 Review", 3))
            ddlOperation.Items.Add(New ListItem("CA1 Review", 4))
            ddlOperation.Items.Add(New ListItem("R1 Review", 5))
            ddlOperation.Items.Add(New ListItem("R2 Review", 6))
            ddlOperation.Items.Add(New ListItem("Adjudicator Review", 7))
            'ddlOperation.Items.Add(New ListItem("Sr. Grader Review", 5))
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...FillDropdown")
            Return False
        End Try
    End Function

    Private Function FillProfile() As Boolean
        Dim Wstr As String = String.Empty
        Dim dsProfile As New DataSet
        Dim dt_SortProfile As New DataTable
        Dim eStr As String = String.Empty
        Try

            Wstr = "cStatusIndi <> 'D'"
            If Not objhelpDb.getUserTypeMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsProfile, eStr) Then
                Me.ShowErrorMessage("", eStr)
                Exit Function
            End If

            If Not dsProfile Is Nothing Then
                If dsProfile.Tables(0).Rows.Count > 0 Then
                    dsProfile.Tables(0).DefaultView.ToTable(True, "vUserTypeName")
                    dt_SortProfile = dsProfile.Tables(0).DefaultView.ToTable()
                    dt_SortProfile.DefaultView.Sort = "vUserTypeName"
                    Me.ddlProfile.DataSource = dt_SortProfile.DefaultView.ToTable()
                    Me.ddlProfile.DataValueField = "vUserTypeCode"
                    Me.ddlProfile.DataTextField = "vUserTypeName"
                    Me.ddlProfile.DataBind()
                    Me.ddlProfile.Items.Insert(0, "Select Profile")
                End If
            End If
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...FillDropdown")
            Return False
        End Try
    End Function

    Private Function FillGrid() As Boolean
        Dim Wstr As String = String.Empty
        Dim ds_EmailSetup As New DataSet
        Dim eStr As String = String.Empty
        Try
            Me.gvEmailSetup.DataSource = Nothing
            Me.gvEmailSetup.DataBind()

            'Wstr = "cStatusIndi <> 'D'" 'Comment condition By Bhargav Thaker 23March2023
            Wstr = "1=1" 'Condition Added by Bhargav Thaker 23March2023
            If Not objhelpDb.GetEmailSetupDeatil(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_EmailSetup, eStr) Then
                Me.ShowErrorMessage("", eStr)
                Exit Function
            End If
            If Not ds_EmailSetup Is Nothing Then
                If ds_EmailSetup.Tables(0).Rows.Count > 0 Then
                    Me.gvEmailSetup.DataSource = ds_EmailSetup.Tables(0)
                    Me.gvEmailSetup.DataBind()
                    If gvEmailSetup.Rows.Count > 0 Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvEmailSetup", "UIgvEmailSetup(); ", True)
                    End If
                End If
            End If

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...FillDropdown")
            Return False
        End Try
    End Function

    Private Function FillUserCheckboxList(UserTypecode As String) As Boolean
        Dim dsUserName As New DataSet
        Dim Wstr As String = String.Empty
        Dim eStr As String = String.Empty
        Try
            Wstr = "cStatusIndi <> 'D' AND vUserTypeCode ='" + UserTypecode + "' ORDER BY vUserName"
            If Not objhelpDb.getuserMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                        dsUserName, eStr) Then
                Throw New Exception(eStr)
                Return False
            End If

            If dsUserName.Tables(0).Rows.Count > 0 Then
                Me.chklstUser.DataSource = dsUserName.Tables(0)
                Me.chklstUser.DataTextField = "vUserName"
                Me.chklstUser.DataValueField = "iUserId"
                Me.chklstUser.DataBind()
            Else
                Me.chklstUser.Items.Clear()
                Me.objCommon.ShowAlert("User not found for this profile", Me.Page())
            End If

            For Each lstItem As ListItem In chklstUser.Items

                lstItem.Attributes.Add("onclick", "SetCheckUncheckAll(document.getElementById('" + chklstUser.ClientID +
                                                    "'), document.getElementById('" + Me.chkSelectAllUser.ClientID + "'));")
            Next lstItem

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillUserCheckboxList")
            Return False
        End Try
        Return True
    End Function
#End Region

    Private Function FillUseridList(UserTypecode As String, EmailId As String) As String
        Dim dsUserName As New DataSet
        Dim Wstr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim Result As String = String.Empty

        Try
            Dim MyString As String = EmailId
            Dim MyArray() As String = MyString.Split(",")
            Dim MyList As List(Of String) = MyArray.ToList()

            Wstr = "cStatusIndi <> 'D' AND vUserTypeCode ='" + UserTypecode + "' ORDER BY vUserName"
            If Not objhelpDb.getuserMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                        dsUserName, eStr) Then
                Throw New Exception(eStr)
                Return False
            End If

            If dsUserName.Tables(0).Rows.Count > 0 Then
                'For Each lstItem As String In MyList
                For i As Integer = 0 To MyList.Count - 1
                    For j As Integer = 0 To dsUserName.Tables(0).Rows.Count - 1
                        If MyList(i).Trim() = dsUserName.Tables(0).Rows(j).Item("vEmailId").ToString().Trim() Then
                            Result += dsUserName.Tables(0).Rows(j)("iUserId").ToString() + ","
                            Exit For
                        End If
                    Next
                    'If String.Equals(dsUserName.Tables(0).Rows(i).Item("vEmailId"), lstItem.Replace(" ", "")) Then
                    'If Convert.ToString(dsUserName.Tables(0).Rows(i).Item("vEmailId")).Contains(lstItem.Replace(" ", "")) Then
                    '    For Each lst As ListItem In Me.chklstUser.Items
                    '        If lst.Value = dsUserName.Tables(0).Rows(i)("iUserId").ToString() Then
                    '            lst.Selected = True
                    '        End If
                    '    Next lst
                    'End If
                Next
                'Next

            End If
            Return Result

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillUserCheckboxList")
            Return Result
        End Try
        Return Result
    End Function

#Region "Reset Page"
    Private Sub ResetPage()
        'Me.txtproject.Text = ""
        Me.chkSelectAllStudy.Checked = False
        Me.txtstudy.Enabled = True
        ddlOperation.Enabled = True
        ddlProfile.Enabled = True
        chkSelectAllStudy.Enabled = True
        Me.trRemarks.Visible = False
        Me.btnCancel.Visible = False
        Me.ddlOperation.SelectedIndex = 0
        'Me.ddlProfile.SelectedIndex = 0
        Me.ddlProfile.ClearSelection()
        'Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
        Me.btnSave.Text = "Save"
        Me.btnSave.ToolTip = "Save"
        Me.chklstUser.Items.Clear()
        Me.chklststudy.Items.Clear()
        'Me.chkStudy.Checked = False
        Me.txtstudy.Text = ""
        Me.txtMailId.Text = ""
        Me.txtRemark.Text = ""
        'Me.Response.Redirect("frmEmailSetupMst.aspx?mode=1")
        GenCall()
    End Sub

#End Region

#Region "Grid Events"
    'Protected Sub gvEmailSetup_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvEmailSetup.PageIndexChanging
    '    gvEmailSetup.PageIndex = e.NewPageIndex
    '    If Not FillGrid() Then
    '        objCommon.ShowAlert("Error while binding", Me.Page)
    '        Exit Sub
    '    End If
    'End Sub

    Protected Sub gvEmailSetup_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvEmailSetup.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or
                e.Row.RowType = DataControlRowType.Header Or
                e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GVC_iEmailSetupId).Style.Add("display", "none")
            e.Row.Cells(GVC_status).Visible = False
        End If
    End Sub

    Protected Sub gvEmailSetup_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvEmailSetup.RowCommand
        Try
            Gv_index = e.CommandArgument
            Dim wStr As String = ""
            Dim ds As New DataSet
            Dim ds_EmailSetup As DataSet = New Data.DataSet
            Dim eStr As String = String.Empty
            Dim Dr As DataRow
            Dim dt As New DataTable
            index = Me.gvEmailSetup.Rows(Gv_index).Cells(GVC_iEmailSetupId).Text.Trim()
            iEmailSetupId = index
            wStr = "iEmailSetupId = " & index

            If e.CommandName.ToUpper = "EDIT" Then
                wStr = "iEmailSetupId=" & index
                If Not objhelpDb.GetEmailSetupDeatil(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                    ds, eStr) Then
                    Me.ShowErrorMessage("Error While Getting Data From EmailSetupDeatil", eStr)
                    Exit Sub
                End If
                Me.ViewState(VS_DtEmailSetupMst) = ds.Tables(0)
                dt = Me.ViewState(VS_DtEmailSetupMst)
                Me.txtstudy.Text = dt.Rows(0)("SiteID")
                Me.txtstudy.Enabled = False
                Me.HParentProjectId.Value = dt.Rows(0)("vParentWorkspaceId")
                Me.ddlOperation.SelectedIndex = dt.Rows(0)("vOperationcode")
                Me.ddlOperation.SelectedItem.Text = dt.Rows(0)("vOperation")
                Me.ddlProfile.SelectedItem.Text = dt.Rows(0)("vUserTypeName")
                Me.ddlProfile.SelectedValue = dt.Rows(0)("vUserTypeCode")
                ddlOperation.Enabled = False
                ddlProfile.Enabled = False
                chkSelectAllStudy.Enabled = False
                If Not FillUserCheckboxList(dt.Rows(0)("vUserTypeCode")) Then
                    objCommon.ShowAlert("Error While FillUserCheckboxList", Me.Page)
                    Exit Sub
                End If
                If Not Fillsite(dt.Rows(0)("vParentWorkspaceId"), dt.Rows(0)("vWorkspaceId")) Then
                    objCommon.ShowAlert("Error While Fillsite", Me.Page)
                    Exit Sub
                End If

                Dim userid = FillUseridList(dt.Rows(0)("vUserTypeCode"), dt.Rows(0)("vEmailId"))
                Dim iUserID As String = String.Empty
                'If condition added by Bhargav Thaker 03Mar2023
                If userid.Length > 0 Then
                    iUserID = userid.Remove(userid.Length - 1)
                End If

                For Each lst As ListItem In Me.chklstUser.Items
                    If iUserID.Contains(lst.Value) Then
                        lst.Selected = True
                    End If
                Next lst

                Me.txtMailId.Text = dt.Rows(0)("vEmailId")
                Me.chklststudy.Enabled = False
                Me.ViewState(VS_DtEmailSetupMst) = dt
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                Me.btnSave.Text = "Update"
                Me.btnSave.ToolTip = "Update"
                Me.trRemarks.Visible = True
                Me.btnCancel.Visible = True
                If Not FillGrid() Then
                    Me.ShowErrorMessage("Error While Binding", "gvEmailSetup_RowCommand")
                    Exit Sub
                End If
            End If

            If e.CommandName.ToUpper = "DELETE" Then
                Status = "Delete"
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                For Each Dr In ds_EmailSetup.Tables(0).Rows
                    Dr("iModifyBy") = Session(S_UserID)
                    Dr("cStatusIndi") = "D"
                    Dr.AcceptChanges()
                Next Dr

                ds_EmailSetup.Tables(0).AcceptChanges()

                If Not objLambda.Save_EmailSetupMst(Me.ViewState(VS_Choice), ds_EmailSetup, Me.Session(S_UserID), eStr) Then
                    objCommon.ShowAlert("Error While Saving EmailSetup", Me.Page)
                    Exit Sub
                End If

                objCommon.ShowAlert("Record Deleted Successfully!", Me.Page)

                If Not FillGrid() Then
                    Me.ShowErrorMessage("Error While Binding", "gvEmailSetup_RowCommand")
                    Exit Sub
                End If
                ResetPage()

            End If

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Getting EmailSetup details...", "gvEmailSetup_RowCommand")

        End Try
    End Sub

    Private Function Fillsite(vParentWorkspaceId As String, vWorkspaceId As String) As Boolean
        Dim Wstr As String = String.Empty
        Dim ds_ProjectNo As New DataSet
        Dim estr As String = String.Empty
        Dim Index As Integer
        Dim ds_UserList As New DataSet
        Dim Ds_RequestId As New DataSet
        Try
            If vWorkspaceId.ToString() = "" Then
                Wstr = "ParentWorkspaceId ='" + vParentWorkspaceId + "' AND cWorkspaceType ='C'AND cStatusIndi <> 'D'order by vProjectNo "
                If Not objhelpDb.GetViewgetWorkspaceDetailForHdr(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ProjectNo, estr) Then
                    Me.objCommon.ShowAlert("Error while getting data from workspaceprotocoldetail", Me.Page)
                    Return False
                End If

                If ds_ProjectNo.Tables(0).Rows.Count > 0 Then
                    Me.chklststudy.DataSource = ds_ProjectNo.Tables(0)
                    Me.chklststudy.DataTextField = "vProjectNo"
                    Me.chklststudy.DataValueField = "vWorkspceId"
                    Me.chklststudy.DataBind()
                Else
                    Me.chklststudy.Items.Clear()
                    Me.objCommon.ShowAlert("User not found for this profile", Me.Page())
                End If
            Else
                Wstr = "vWorkspaceId ='" + vWorkspaceId + "' AND cWorkspaceType ='C'AND cStatusIndi <> 'D'order by vProjectNo "
                If Not objhelpDb.GetViewgetWorkspaceDetailForHdr(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ProjectNo, estr) Then
                    Me.objCommon.ShowAlert("Error while getting data from workspaceprotocoldetail", Me.Page)
                    Return False
                End If

                If ds_ProjectNo.Tables(0).Rows.Count > 0 Then
                    Me.chklststudy.DataSource = ds_ProjectNo.Tables(0)
                    Me.chklststudy.DataTextField = "vProjectNo"
                    Me.chklststudy.DataValueField = "vWorkspceId"
                    Me.chklststudy.DataBind()
                Else
                    Me.chklststudy.Items.Clear()
                    Me.objCommon.ShowAlert("User not found for this profile", Me.Page())
                End If
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SelectAllFields", "SelectAllFields(); ", True)
            For Each lstItem As ListItem In chklststudy.Items

                lstItem.Attributes.Add("onclick", "SetCheckUncheckAll(document.getElementById('" + chklststudy.ClientID +
                                               "'), document.getElementById('" + Me.chkSelectAllStudy.ClientID + "'));")

            Next lstItem
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillUserCheckboxList")
            Return False
        End Try
        Return True
    End Function

    Protected Sub gvEmailSetup_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvEmailSetup.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (gvEmailSetup.PageSize * gvEmailSetup.PageIndex) + 1
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "Edit"
            CType(e.Row.FindControl("lnkDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkDelete"), ImageButton).CommandName = "DELETE"
            CType(e.Row.FindControl("lnkAudit"), ImageButton).Attributes.Add("iEmailSetupId", e.Row.Cells(GVC_iEmailSetupId).Text.Trim)
            If e.Row.Cells(GVC_status).Text = "D" Then
                e.Row.BackColor = Drawing.Color.FromArgb(255, 189, 121)
                CType(e.Row.FindControl("lnkDelete"), ImageButton).Enabled = False
                CType(e.Row.FindControl("lnkDelete"), ImageButton).ToolTip = "InActived"
                CType(e.Row.FindControl("lnkEdit"), ImageButton).Enabled = False
                CType(e.Row.FindControl("lnkEdit"), ImageButton).ToolTip = "InActived"
            End If
        End If
    End Sub

    Protected Sub gvEmailSetup_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvEmailSetup.RowEditing
        e.Cancel = True
    End Sub

    Protected Sub gvEmailSetup_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvEmailSetup.RowDeleting

    End Sub

#End Region

#Region "Assignvalues"

    Private Function AssignUpdatedValues(ByRef ds_Save As DataSet, ByVal type As String) As Boolean

        Dim dr As DataRow
        Dim ds_EmailSetup As New DataSet
        Dim dt_Save As New DataTable
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim strOfUserCode As String = ""
        Dim EmailId As String = String.Empty
        Dim StudyFlag As String = String.Empty
        Try
            dt_Save = CType(Me.ViewState(VS_DtEmailSetupMst), DataTable)
            EmailId = Me.txtMailId.Text
            If Me.chkSelectAllStudy.Checked = True Then
                StudyFlag = "Y"
            Else
                StudyFlag = "N"
            End If

            If type.ToUpper = "EDIT" Then
                wStr = "iEmailSetupId='" & dt_Save.Rows(0).Item("iEmailSetupId") & "'"
            Else
                wStr = "1=2"
            End If

            If Not objhelpDb.GetEmailSetupMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_EmailSetup, eStr) Then
                Me.ShowErrorMessage("", eStr)
                Exit Function
            End If
            dt_Save = ds_EmailSetup.Tables(0)
            If type.ToUpper = "ADD" Then
                For Each lstItemstudy As ListItem In Me.chklststudy.Items
                    If lstItemstudy.Selected Then
                        dr = ds_EmailSetup.Tables(0).NewRow()
                        If Me.chkSelectAllStudy.Checked = True Then
                            dr("vWorkspaceId") = ""
                        Else
                            dr("vWorkspaceId") = lstItemstudy.Value
                        End If
                        dr("vOperationcode") = Me.ddlOperation.SelectedValue.Trim()
                        dr("vOperation") = ddlOperation.SelectedItem.Text
                        dr("vUserTypeCode") = Me.ddlProfile.SelectedValue.Trim()
                        For Each lstItem As ListItem In Me.chklstUser.Items
                            If lstItem.Selected Then
                                dr("iUserId") = lstItem.Value.Trim()
                                Exit For
                            End If
                        Next lstItem
                        dr("vRemark") = ""
                        dr("iModifyBy") = Session(S_UserID)
                        dr("cStatusIndi") = "N"
                        dr("vEmailId") = EmailId.Trim()
                        dr("cIsStudy") = StudyFlag
                        dr("vParentWorkspaceId") = Me.HParentProjectId.Value
                        ds_EmailSetup.Tables(0).Rows.Add(dr)
                    End If
                Next lstItemstudy
                ds_Save = ds_EmailSetup

            ElseIf type.ToUpper = "EDIT" Then
                For Each dr In dt_Save.Rows
                    dr("iModifyBy") = Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr("vEmailId") = EmailId.Trim()
                    dr("vRemark") = Me.txtRemark.Text
                    dr.AcceptChanges()
                Next
                dt_Save.AcceptChanges()
            End If
            Me.ViewState(VS_DtEmailSetupMst) = dt_Save
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...AssignUpdates")
            Return False
        End Try


    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim ds_Save As New DataSet
        Dim eStr As String = String.Empty
        Dim dt_EmailSetUpMst As DataTable = Nothing
        Dim str_iEmailSetupId As String = ""
        Dim wStr As String = ""
        Dim ds_EmailSetup As New DataSet
        Try

            str_iEmailSetupId = "0"  'Convert.ToString(dt_EmailSetUpMst.Rows(0)("iEmailSetupId"))
            If Not Me.CheckDuplicateEmailSetup(str_iEmailSetupId) Then
                objCommon.ShowAlert("Email SetUp Already Exists !!!", Me)
                Exit Sub
            End If

            'If Not AssignUpdatedValues(ds_Save) Then
            '    objCommon.ShowAlert("Error While Assigning values", Me.Page)
            '    Exit Sub
            'End If
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                If Not AssignUpdatedValues(ds_Save, "Edit") Then
                    Exit Sub
                End If
                ds_Save = New DataSet
                ds_Save.Tables.Add(CType(Me.ViewState(VS_DtEmailSetupMst), Data.DataTable).Copy())
                ds_Save.Tables(0).TableName = "EmailSetup"

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Me.GenCall_Data(ds_Save)
                Me.ViewState(VS_DtEmailSetupMst) = ds_Save.Tables("EmailSetup")   ' adding blank DataTable in viewstate
                If Not AssignUpdatedValues(ds_Save, "Add") Then
                    Exit Sub
                End If

            End If
            If Not objLambda.Save_EmailSetupMst(Me.ViewState(VS_Choice), ds_Save, Me.Session(S_UserID), eStr) Then
                objCommon.ShowAlert("Error While Saving EmailSetupMst", Me.Page)
                Exit Sub
            End If

            If Not FillGrid() Then
                Me.ShowErrorMessage("Error While Binding", "btn_Save")
                Exit Sub
            End If
            Me.objCommon.ShowAlert("Record Saved Sucessfully", Me.Page)
            ResetPage()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "Error While Saving")
        End Try
    End Sub

    Protected Sub btnclose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnclose.Click
        Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnupdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Response.Redirect("frmEmailSetupMst.aspx?mode=1")
    End Sub
#End Region

#Region "CHECKBOX EVENTS "

    Protected Sub chkSelectAllUser_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelectAllUser.CheckedChanged

        If Me.chkSelectAllUser.Checked = False Then

            Me.chklstUser.ClearSelection()

        ElseIf Me.chkSelectAllUser.Checked = True Then

            If Me.chklstUser.Items.Count > 0 Then
                Me.chklstUser.ClearSelection()

                For Each lstItem As ListItem In Me.chklstUser.Items
                    lstItem.Selected = Me.chkSelectAllUser.Checked
                Next lstItem

            End If
        End If
        If Not FillMailId() Then
            objCommon.ShowAlert("Error While Getting Maild Id", Me.Page)
        End If
    End Sub

    Protected Sub chkSelectAllStudy_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelectAllStudy.CheckedChanged

        If Me.chkSelectAllStudy.Checked = False Then

            Me.chklststudy.ClearSelection()

        ElseIf Me.chkSelectAllStudy.Checked = True Then

            If Me.chklststudy.Items.Count > 0 Then
                Me.chklststudy.ClearSelection()

                For Each lstItem As ListItem In Me.chklststudy.Items
                    lstItem.Selected = Me.chkSelectAllStudy.Checked
                Next lstItem

            End If
        End If

    End Sub
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

    Protected Sub ddlProfile_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProfile.SelectedIndexChanged
        Dim UsertypeCode As String = ddlProfile.SelectedValue
        If Not FillUserCheckboxList(UsertypeCode) Then
            objCommon.ShowAlert("Error While FillUserCheckboxList", Me.Page)
            Exit Sub
        End If
    End Sub
    Protected Sub gvEmailSetup_RowCommand1(sender As Object, e As GridViewCommandEventArgs)

    End Sub
    Protected Sub btnRemarksUpdate_Click(sender As Object, e As EventArgs)
        Dim ds_Delete As New DataSet
        Dim estr As String = ""
        Dim dr As DataRow
        Dim dt_EmailSetup As DataTable
        Dim wStr As String = String.Empty

        index = hdnEmailSetupNO.Value
        Try
            wStr = "iEmailSetupId=" & index
            If Not objhelpDb.GetEmailSetupDeatil(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                    ds_Delete, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From EmailSetupDeatil", estr)
                Exit Sub
            End If
            dt_EmailSetup = ds_Delete.Tables(0)
            For Each dr In dt_EmailSetup.Rows
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("vRemark") = Me.txtRemarks_delete.Text.Trim()
                dr("cStatusIndi") = "D"
                dr.AcceptChanges()
            Next
            dt_EmailSetup.AcceptChanges()
            dt_EmailSetup.TableName = "EmailSetup"
            ds_Delete = Nothing
            ds_Delete = New DataSet
            ds_Delete.Tables.Add(dt_EmailSetup.Copy())

            If Not objLambda.Save_EmailSetupMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, ds_Delete, Me.Session(S_UserID), estr) Then
                objCommon.ShowAlert("Error While InActivate EmailSetUp!", Me.Page)
                Exit Sub
            End If
            objCommon.ShowAlert("Record Deleted Successfully!", Me.Page)
            txtRemarks_delete.Text = Nothing
            ResetPage()
            If Not FillGrid() Then
                Me.ShowErrorMessage("Error While Binding", "gvEmailSetUp_RowCommand")
                Exit Sub
            End If
            'If Not Me.GenCall() Then
            '    Exit Sub
            'End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")

        End Try
    End Sub

    Private Function CheckDuplicateEmailSetup(Optional ByVal str_iEmailSetupId As String = "0") As Boolean
        Dim dsEmailSetUpMst As New DataSet
        Dim wStr_TechnicianMst As String = ""
        Dim User_ProfileID As String = ""
        Dim MultiWorkspaceID As String = ""
        Try
            CheckDuplicateEmailSetup = True
            For Each lstItem As ListItem In Me.chklstUser.Items
                If lstItem.Selected Then
                    User_ProfileID += lstItem.Value.Trim() + ","
                End If
            Next
            'If Not chkStudy.Checked = True Then
            '    If User_ProfileID <> "" And HProjectId.Value.Trim.Length > 0 Then
            '        wStr_TechnicianMst = "vOperationcode='" + Me.ddlOperation.SelectedValue.Trim + "' AND vWorkspaceId ='" + Me.HProjectId.Value.Trim() + "' AND vUserTypeCode ='" + Me.ddlProfile.SelectedValue.Trim() + "' AND iUserId IN(" + User_ProfileID.TrimEnd(",") + ") AND cStatusIndi <> 'D'"
            '        'Else
            '        '    wStr_TechnicianMst = "vOperationcode='" + Me.ddlOperation.SelectedValue.Trim + "' AND vWorkspaceId ='" + Me.HProjectId.Value.Trim() + "' AND vUserTypeCode ='" + Me.ddlProfile.SelectedValue.Trim() + "' AND iUserId IN(" + Me.chklstUser.SelectedValue.Trim() + ") AND cStatusIndi <> 'D'"
            '    End If
            'Else
            For Each lstItemstudy As ListItem In Me.chklststudy.Items
                If lstItemstudy.Selected Then
                    MultiWorkspaceID += "'" & lstItemstudy.Value.Trim() & "',"
                End If
            Next
            If User_ProfileID <> "" And HParentProjectId.Value.Trim.Length > 0 Then
                wStr_TechnicianMst = "vOperationcode='" + Me.ddlOperation.SelectedValue.Trim + "' AND vWorkspaceId IN(" + MultiWorkspaceID.TrimEnd(",") + ") AND vUserTypeCode ='" + Me.ddlProfile.SelectedValue.Trim() + "' AND iUserId IN(" + User_ProfileID.TrimEnd(",") + ") AND cStatusIndi <> 'D'"
            End If
            'End If

            If Not objhelpDb.GetEmailSetupDeatil(wStr_TechnicianMst, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                           dsEmailSetUpMst, eStr) Then
                Throw New Exception(eStr)
            End If

            'If condition added by Bhargav Thaker 07Mar2023
            If Me.chklststudy.Items.Count > 1 Then
                If dsEmailSetUpMst.Tables(0).Rows.Count > 0 Then
                    CheckDuplicateEmailSetup = False
                    For Each drUserId As DataRow In dsEmailSetUpMst.Tables("View_EmailSetUp").Rows
                        For Each lstItem As ListItem In Me.chklstUser.Items
                            If lstItem.Selected Then
                                If lstItem.Value = drUserId("iUserId") Then
                                    lstItem.Selected = False
                                Else
                                    lstItem.Selected = True
                                    CheckDuplicateEmailSetup = True
                                End If
                            End If
                        Next
                    Next
                End If
            End If
        Catch ex As Exception
            CheckDuplicateEmailSetup = True
            Throw ex
        End Try
    End Function
#Region "Fill EmailId"
    Private Function FillMailId() As Boolean
        Dim Param As String = String.Empty
        Dim dsProfile As New DataSet
        Dim dt_SortProfile As New DataTable
        Dim eStr As String = String.Empty
        Dim dsUserName As New DataSet
        Dim UsertypeCode As String = ddlProfile.SelectedItem.Value
        Dim UserName As String = String.Empty
        Dim UserName1 As String = String.Empty
        'Dim i As Integer = 0
        Try
            For Each i As ListItem In chklstUser.Items
                If i.Selected Then
                    UserName += Convert.ToString(i.Text) + ","
                End If
            Next i

            'Condition added by Bhargav Thaker 28Feb2023
            If UserName.Length > 0 Then
                UserName1 = UserName.Remove(UserName.Length - 1)
            End If

            'Param = "vUserName ='" + UserName + "'##vUserTypeCode ='" + UsertypeCode + "' "
            Param = UserName1 + "##" + UsertypeCode
            If Not objhelpDb.Proc_EmailSetupMailId(Param, dsUserName, eStr) Then
                Throw New Exception("Error Retrieving Total Record")
            End If
            If dsUserName.Tables(0).Rows.Count > 0 Then
                Me.txtMailId.Text = Convert.ToString(dsUserName.Tables(0).Rows(0).Item("vEmailId"))
            Else
                Me.txtMailId.Text = ""
            End If
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...FillMailId")
            Return False
        End Try
    End Function

#End Region
    Protected Sub chklstUser_SelectedIndexChanged(sender As Object, e As EventArgs)
        If Not FillMailId() Then
            objCommon.ShowAlert("Error While Getting Maild Id", Me.Page)
        End If
    End Sub
    Protected Sub chkStudy_CheckedChanged(sender As Object, e As EventArgs)
        If Me.chkSelectAllStudy.Checked = True Then
            chklststudy.Enabled = False
            'txtproject.Enabled = False
            'txtproject.Text = ""
            'studydiv.Visible = True
        Else
            chklststudy.Enabled = True
            'txtproject.Enabled = True
            'txtstudy.Text = ""
            'studydiv.Visible = False
        End If
    End Sub
    Protected Sub chklststudy_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub
    Protected Sub btnstudy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnstudy.Click
        Dim Wstr As String = String.Empty
        Dim ds_ProjectNo As New DataSet
        Dim estr As String = String.Empty
        Dim Index As Integer
        Dim ds_UserList As New DataSet
        Dim Ds_RequestId As New DataSet
        Wstr = "ParentWorkspaceId ='" + Me.HParentProjectId.Value.Trim() + "' AND cWorkspaceType ='C'AND cStatusIndi <> 'D'order by vProjectNo "
        If Not objhelpDb.GetViewgetWorkspaceDetailForHdr(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ProjectNo, estr) Then
            Me.objCommon.ShowAlert("Error while getting data from workspaceprotocoldetail", Me.Page)
            Exit Sub
        End If
        'For Index = 0 To ds_ProjectNo.Tables(0).Rows.Count - 1 Step 1
        '    If Not Convert.ToString(ds_ProjectNo.Tables(0).Rows(Index).Item("vprojectNo")).Trim() = "" Then
        '        If ds_ProjectNo.Tables(0).Rows(Index).Item("vRequestId").ToString().Trim() = "0000000000" Then
        '        Else
        '            chklststudy.Items.Add(New ListItem(Convert.ToString(ds_ProjectNo.Tables(0).Rows(Index).Item("vProjectNo")).Trim()))
        '        End If

        '    End If
        If ds_ProjectNo.Tables(0).Rows.Count > 0 Then
            Me.chklststudy.DataSource = ds_ProjectNo.Tables(0)
            Me.chklststudy.DataTextField = "vProjectNo"
            Me.chklststudy.DataValueField = "vWorkspceId"
            Me.chklststudy.DataBind()
        Else
            Me.chklststudy.Items.Clear()
            Me.objCommon.ShowAlert("User not found for this profile", Me.Page())
        End If
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SelectAllFields", "SelectAllFields(); ", True)
        For Each lstItem As ListItem In chklststudy.Items

            lstItem.Attributes.Add("onclick", "SetCheckUncheckAll(document.getElementById('" + chklststudy.ClientID +
                                                "'), document.getElementById('" + Me.chkSelectAllStudy.ClientID + "'));")

        Next lstItem


    End Sub

#Region "Web Method"

    <WebMethod>
    Public Shared Function AuditTrail(ByVal iEmailSetupId As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_EmailSetuptmst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim dtEmailSetupHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Try

            wStr = " iEmailSetupId = '" + iEmailSetupId + "' Order by iEmailSetupHistoryId DESC"
            If Not objHelp.GetView_EmailSetupHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_EmailSetuptmst, estr) Then
                Throw New Exception(estr)
            End If

            If Not dtEmailSetupHistory Is Nothing Then
                dtEmailSetupHistory.Columns.Add("SrNo")
                dtEmailSetupHistory.Columns.Add("SiteId")
                dtEmailSetupHistory.Columns.Add("Operation")
                dtEmailSetupHistory.Columns.Add("Profile")
                dtEmailSetupHistory.Columns.Add("EmailId")
                dtEmailSetupHistory.Columns.Add("Remarks")
                dtEmailSetupHistory.Columns.Add("ModifyBy")
                dtEmailSetupHistory.Columns.Add("ModifyOn")
            End If
            dt = ds_EmailSetuptmst.Tables(0)
            Dim dv As New DataView(dt)

            For Each dr As DataRow In dv.ToTable.Rows

                drAuditTrail = dtEmailSetupHistory.NewRow()

                drAuditTrail("SrNo") = i
                drAuditTrail("SiteId") = dr("SiteID").ToString()
                drAuditTrail("Operation") = dr("vOperation").ToString()
                drAuditTrail("Profile") = dr("vUserTypeName").ToString()
                drAuditTrail("EmailId") = dr("vEmailId").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyby").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ChangeOn"))
                dtEmailSetupHistory.Rows.Add(drAuditTrail)
                dtEmailSetupHistory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtEmailSetupHistory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

#End Region
End Class