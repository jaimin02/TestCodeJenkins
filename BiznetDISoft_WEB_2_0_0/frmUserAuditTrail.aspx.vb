
Partial Class frmUserAuditTrail
    Inherits System.Web.UI.Page

#Region " VARIABLE DECLARATION "

    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private eStr_Retu As String

    Private Const GVC_UserID As Integer = 0


#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            If Not Me.GenCall() Then
                Exit Sub
            End If
        End If
    End Sub

#Region " GENCALL "

    Private Function GenCall() As Boolean
        GenCall = True

        Try
            GenCall_Data()
            GenCall_ShowUI()
        Catch ex As Exception
            GenCall = False
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Function

    Private Sub GenCall_Data()

    End Sub

    Private Sub GenCall_ShowUI()
        Try

            Page.Title = ":: User Audit Trail  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            If Not Me.FillUserDropDownList() Then
                Exit Sub
            End If

        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region " FILL FUNCTIONS AND SUBS "

    Private Function FillUserDropDownList() As Boolean
        Dim dsUserMst As New DataSet
        Dim wStr_UserMst As String = String.Empty

        Try
            If Not objHelp.GetUsersForDropDown("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, _
                                               dsUserMst, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            Me.ddlUsers.DataSource = dsUserMst.Tables(0)
            Me.ddlUsers.DataValueField = "iUserId"
            Me.ddlUsers.DataTextField = "vDisplayName"
            Me.ddlUsers.DataBind()
            Me.ddlUsers.Items.Insert(0, New ListItem("All", "0"))

            'For Each lstItm As ListItem In ddlUsers.Items
            '    lstItm.Attributes.Add("title", lstItm.Text)
            'Next
            'Me.ddlUsers.Attributes.Add("onmouseover", "this.title = this.options[this.selectedIndex].title;")

            FillUserDropDownList = True
        Catch ex As Exception
            FillUserDropDownList = False
            Me.ShowErrorMessage("Error Filling User Drop Down List", "", ex)
        End Try
    End Function

    Private Sub FillUserAuditGrid()
        Dim dsUserAuditTrail As New DataSet
        Dim wStr_UserAuditTrail As String = "1=1"
        Try
            If Me.ddlUsers.SelectedIndex > 0 Then
                wStr_UserAuditTrail = "iUserId=" + Me.ddlUsers.SelectedValue.Trim
            End If

            If Not objHelp.View_UserAuditTrail(wStr_UserAuditTrail, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                      dsUserAuditTrail, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            Me.gvwUserAuditTrail.DataSource = dsUserAuditTrail.Tables(0)
            Me.gvwUserAuditTrail.DataBind()

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region " BUTTON EVENTS "

    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnView.Click

        Try

            Me.FillUserAuditGrid()
        Catch ex As Exception
            Me.ShowErrorMessage("Error Generating User Audit Trail", "", ex)
        End Try
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Fixheader", "Fixheader()", True)

    End Sub

#End Region

#Region " GRID VIEW EVENTS "

    Protected Sub gvwUserAuditTrail_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvwUserAuditTrail.PageIndexChanging
        gvwUserAuditTrail.PageIndex = e.NewPageIndex
        FillUserAuditGrid()

    End Sub

    Protected Sub gvwUserAuditTrail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwUserAuditTrail.RowDataBound
        e.Row.Cells(GVC_UserID).Visible = False
    End Sub

    Protected Sub gvwUserAuditTrail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwUserAuditTrail.RowCommand
        Dim strQuery As String = String.Empty
        If e.CommandName.ToString.Trim.ToUpper = "UserHistory".ToUpper() Then
            strQuery = "window.open('frmUserMstHistoryAuditTrail.aspx?value=" + e.CommandArgument.ToString.Trim + _
                            "','_blank');"
        ElseIf e.CommandName.ToString.Trim.ToUpper = "PasswordHistory".ToUpper Then
            strQuery = "window.open('frmPasswordHistoryAuditTrail.aspx?value=" + e.CommandArgument.ToString.Trim + _
                            "','_blank');"
        ElseIf e.CommandName.ToString.Trim.ToUpper = "LoginDetails".ToUpper Then
            strQuery = "window.open('frmLoginAuditTrail.aspx?value=" + e.CommandArgument.ToString.Trim + _
                            "','_blank');"
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "WINOPEN", strQuery, True)
    End Sub


#End Region



#Region " ERROR MESSAGE "

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
