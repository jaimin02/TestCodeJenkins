
Partial Class frmWorkspaceVisitDtl
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "

    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()

    Private Const VS_WorkspaceId As String = "vWorkspaceId"

    Private Const GVCWorkspace_SrNo As Integer = 0
    Private Const GVCWorkspace_WorkspaceId As Integer = 1
    Private Const GVCWorkspace_WorkspaceDesc As Integer = 2
    Private Const GVCWorkspace_Visit As Integer = 3
    Private Const GVCWorkspace_Subject As Integer = 4

#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not GenCall_ShowUI() Then
                Exit Sub
            End If
        End If
    End Sub

#End Region

#Region "GenCall_ShowUI"

    Private Function GenCall_ShowUI() As Boolean
        Dim eStr As String = ""
        Dim wStr As String = ""

        Try

            Page.Title = ":: Project Visit Detail  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")


            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            CType(Master.FindControl("lblHeading"), Label).Text = "Project Visit Detail"

            Me.txtFromDate.Attributes.Add("ReadOnly", "ReadOnly")
            Me.txtToDate.Attributes.Add("ReadOnly", "ReadOnly")

            If Not FillddlProjectGroup() Then
                Return False
            End If

            If Not IsNothing(Me.Request.QueryString("ProjectGroupId")) AndAlso _
                            Me.Request.QueryString("ProjectGroupId").Trim() <> "" Then
                Me.ddlProjectGroup.SelectedItem.Value = Me.Request.QueryString("ProjectGroupId").ToString

                If Not FillGridWorkspaceSubject() Then
                    Return False
                End If

            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function

#End Region

#Region "Fill Functions"

    Private Function FillddlProjectGroup() As Boolean
        Dim wStr As String = ""
        Dim eStr As String = ""
        Dim ds_ProjectGroup As New DataSet
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not objcommon.GetScopeValueWithCondition(wStr) Then
                Exit Function
            End If

            wStr += " And cStatusIndi <> 'D' order by vProjectGroupDesc"
            If Not objHelp.GetProjectgroupMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                        ds_ProjectGroup, eStr) Then
                Throw New Exception(eStr)
            End If

            Me.ddlProjectGroup.DataSource = ds_ProjectGroup.Tables(0)
            Me.ddlProjectGroup.DataValueField = "nProjectGroupNo"
            Me.ddlProjectGroup.DataTextField = "vProjectGroupDesc"
            Me.ddlProjectGroup.DataBind()
            Me.ddlProjectGroup.Items.Insert(0, New ListItem("Select Project Group", "0"))

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

    Private Function FillGridWorkspaceSubject() As Boolean
        Dim wStr As String = ""
        Dim eStr As String = ""
        Dim ds_WorkspaceSubject As New DataSet
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Me.gvWorkspaceSubject.DataSource = Nothing

            wStr = "nProjectGroupNo=" + Me.ddlProjectGroup.SelectedItem.Value.Trim() + " And cStatusIndi <> 'D'"
            wStr += " And (dCreatedOn >= '" + Me.txtFromDate.Text.Trim() + "'"
            wStr += " And dCreatedOn <= '" + Me.txtToDate.Text.Trim() + "') order by vWorkspaceId"
            If Not objHelp.View_WorkSpaceVisitDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                        ds_WorkspaceSubject, eStr) Then
                Throw New Exception(eStr)
            End If

            Me.gvWorkspaceSubject.DataSource = ds_WorkspaceSubject
            Me.gvWorkspaceSubject.DataBind()

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

#End Region

#Region "Grid Event"

    Protected Sub gvWorkspaceSubject_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvWorkspaceSubject.RowCommand
        Dim index As Integer = e.CommandArgument
        Dim RedirectStr As String = ""
        If e.CommandName.ToUpper = "EDIT" Then

            RedirectStr = "window.open(""" + "frmProjectDetailMst.aspx?WorkSpaceId=" & _
                    Me.gvWorkspaceSubject.Rows(index).Cells(GVCWorkspace_WorkspaceId).Text.Trim() _
                    & "&Page=frmClinicalTrialData"")"

            ScriptManager.RegisterClientScriptBlock(Me.Page(), Me.GetType(), "OpenWindow", RedirectStr, True)

        End If
    End Sub

    Protected Sub gvWorkspaceSubject_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvWorkspaceSubject.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or _
                      e.Row.RowType = DataControlRowType.Header Or _
                      e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVCWorkspace_WorkspaceId).Visible = False
        End If
    End Sub

    Protected Sub gvWorkspaceSubject_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvWorkspaceSubject.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVCWorkspace_SrNo).Text = e.Row.RowIndex + (gvWorkspaceSubject.PageSize * gvWorkspaceSubject.PageIndex) + 1

            'CType(e.Row.FindControl("lnkEdit"), LinkButton).CommandArgument = e.Row.RowIndex
            'CType(e.Row.FindControl("lnkEdit"), LinkButton).CommandName = "Edit"

        End If
    End Sub

    Protected Sub gvWorkspaceSubject_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvWorkspaceSubject.PageIndexChanging
        gvWorkspaceSubject.PageIndex = e.NewPageIndex
        If Not Me.FillGridWorkspaceSubject() Then
            Exit Sub
        End If
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

#Region "Button Events"

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not Me.FillGridWorkspaceSubject() Then
            Exit Sub
        End If
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

End Class
