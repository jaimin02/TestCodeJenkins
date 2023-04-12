
Partial Class frmProjectArchival
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATIONS"

    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()


    Private Const VS_WorkspaceId As String = "workspaceId"
    Private Const VS_WorkspaceName As String = "workspaceDesc"

    Private ds_Projects As DataSet
    Private eStr_Retu As String = ""

    Private Const GVC_WorkspaceId As Integer = 0
    Private Const GVC_WorkspaceDesc As Integer = 1
    Private Const GVC_ProjectNo As Integer = 3
    Private Const GVC_Status As Integer = 11
    Private Const GVC_LnkBtnChangeStatus As Integer = 12
    Private Const GVC_LnkBtnProjectDet As Integer = 13
    Private Const GVC_DefaultUserRights As Integer = 14

    Private templateId As String = ""
    Private workspaceId As String = ""
    Private status As String = ""

#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        clsCommon.ClearCache(Me)

        If Not IsPostBack Then
            GenCall_ShowUI()
        End If
    End Sub

#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Page.Title = ":: Projects Archival  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
        CType(Me.Master.FindControl("lblMandatory"), Label).Visible = False
        CType(Me.Master.FindControl("lblHeading"), Label).Text = "Projects Archival"
        BindGrid()

    End Function

#End Region

#Region "Bind Grid"

    Private Sub BindGrid()

        ds_Projects = New DataSet
        Dim dv_Projects As DataView = Nothing
        Dim Type As String = ""
        Dim UserId As String = ""
        Dim DMS As String = ""
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            'Type = Me.Request.QueryString("Type").Trim()
            UserId = Me.Session(S_UserID) 'IIf(Type.ToUpper = "ALL", "", Me.Session(S_UserID))

            If Not objHelp.View_MyProjects("(cProjectStatus='S' or cProjectStatus='D' or cProjectStatus='A') and iUserId=" + UserId, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                ds_Projects, eStr_Retu) Then

                objCommon.ShowAlert("Error displaying projects:" & eStr_Retu.ToString(), Me)
                Exit Sub

            End If


            If (Not ViewState("sortExpression") Is Nothing) And (Not ViewState("sortDirection") Is Nothing) Then
                dv_Projects = New DataView()
                dv_Projects = ds_Projects.Tables(0).DefaultView

                dv_Projects.Sort = ViewState("sortExpression").ToString & " " & ViewState("sortDirection")


                gvwProjects.DataSource = dv_Projects
                gvwProjects.DataBind()

            Else

                gvwProjects.DataSource = ds_Projects
                gvwProjects.DataBind()

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "GRIDVIEW EVENTS"

    Protected Sub gvwProjects_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvwProjects.Sorting

        If ViewState("sortExpression") Is Nothing Then
            ViewState("sortExpression") = e.SortExpression
        End If

        If ViewState("sortDirection") Is Nothing Then
            ViewState("sortDirection") = "ASC"
        Else
            If ViewState("sortExpression").ToString <> e.SortExpression Then
                ViewState("sortExpression") = e.SortExpression
                ViewState("sortDirection") = "ASC"
            Else
                If ViewState("sortDirection").ToString.Contains("ASC") Then
                    ViewState("sortDirection") = "DESC"
                Else
                    ViewState("sortDirection") = "ASC"
                End If
            End If

        End If

        BindGrid()

    End Sub

    Protected Sub gvwProjects_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then

            CType(e.Row.FindControl("lnkBtnRights"), LinkButton).Enabled = False

            If e.Row.Cells(GVC_Status).Text.ToUpper.Trim() = "ARCHIVED" Then

                CType(e.Row.FindControl("lnkBtn"), LinkButton).Enabled = False
                CType(e.Row.FindControl("lnkBtnRights"), LinkButton).Enabled = True
                CType(e.Row.FindControl("lnkBtnRights"), LinkButton).PostBackUrl = "frmWorkspaceDefaultWorkflowUserDtl.aspx?mode=1&WorkspaceId=" & _
                                                                            e.Row.Cells(GVC_WorkspaceId).Text.Trim() & "&WorkspaceName=" & e.Row.Cells(GVC_WorkspaceDesc).Text.Trim() & _
                                                                            "&page=frmProjectArchival" & "&Type=" & Me.Request.QueryString("Type").Trim()

            End If

            CType(e.Row.FindControl("lnkBtn"), LinkButton).OnClientClick = "return confirm('Are You Sure You Want To Archive Project No.: " & e.Row.Cells(GVC_ProjectNo).Text.Trim() & "');"
        End If

    End Sub

    Protected Sub gvwProjects_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or _
        e.Row.RowType = DataControlRowType.Header Or _
        e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GVC_WorkspaceId).Visible = False 'First Column is Hidden
        End If

    End Sub

    Protected Sub gvwProjects_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        gvwProjects.PageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Protected Sub lnkBtnProDet_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim gvr As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        Dim workSpaceId As String = gvr.Cells(GVC_WorkspaceId).Text
        Dim Type As String = ""
        Dim DMS As String = ""

        'Type = Me.Request.QueryString("Type").Trim()
        'DMS = IIf(Convert.ToString(Me.Request.QueryString("DMS")) Is Nothing, "", Convert.ToString(Me.Request.QueryString("DMS")))
        Response.Redirect("frmProjectDetailMst.aspx?WorkSpaceId=" & workSpaceId & "&Page=frmMyProject&Type=" & Type)

    End Sub

    Protected Sub lnkBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim gvr As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        Dim currentStatus As String = gvr.Cells(GVC_Status).Text.Trim
        Dim dt As New DataTable
        'Logic of Archive Project
        Dim estr As String = ""
        Dim Str_Worn As String = ""
        Dim wStr As String = ""

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            ViewState(VS_WorkspaceId) = gvr.Cells(GVC_WorkspaceId).Text.Trim

            If Not SaveValues() Then
                Me.objCommon.ShowAlert("Error While Assinging Data", Me.Page())
                Exit Sub
            End If

            BindGrid()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try


        'CType(Me.Master.FindControl("Scriptmanager1"), ScriptManager).RegisterClientScriptBlock(Me, Me.GetType, "ShowHideDiv", "ShowHideDiv('Y');", True)

    End Sub

#End Region

#Region "AssignValues "

    Private Function SaveValues() As Boolean
        Dim dr1 As DataRow
        Dim dt_UserRights As New DataTable
        Dim dt_WorksapaceMst As New DataTable
        Dim Wstr As String = ""
        Dim estr As String = ""
        Dim ReqId As String = ""
        Dim Duplicate As Boolean

        Dim ds_Save As New DataSet

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""


            Wstr = "vWorkspaceId='" & Me.ViewState(VS_WorkspaceId).ToString.Trim() & "' and cStatusIndi<>'D'"

            'For Removing Default User Rights of Project 
            If Not objHelp.View_WorkspaceDefaultWorkflowUserDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_Save, eStr_Retu) Then

                Response.Write(eStr_Retu)
                Exit Function

            End If

            dt_UserRights = ds_Save.Tables(0)
            'dt_UserRights.Clear()
            Duplicate = False

            For Each dr1 In dt_UserRights.Rows

                'Because as per discussion other than him/her all users will disabled
                If dr1("iUserId") = Me.Session(S_UserID).ToString.Trim() Then
                    Duplicate = True
                    Continue For
                End If

                dr1("cStatusIndi") = "D"
                dr1("iModifyBy") = Me.Session(S_UserID).ToString.Trim()
                dt_UserRights.AcceptChanges()

            Next dr1

            If dt_UserRights.Rows.Count > 0 Then

                ds_Save = New DataSet
                dt_UserRights.TableName = "View_WorkspaceDefaultWorkFlowUserDtl"
                ds_Save.Tables.Add(dt_UserRights.Copy())

                If Not objLambda.Save_WorkspaceDefaultWorkFlowUserDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_Save, Me.Session(S_UserID), estr) Then
                    Me.objCommon.ShowAlert("Error While Saving WorkspaceDefaultWorkFlowUserDtl", Me.Page)
                    Exit Function

                End If

            End If

            If Duplicate = False Then
                dr1 = dt_UserRights.NewRow()
                dr1("nWorkspaceDefaultWorkflowUserId") = dt_UserRights.Rows.Count + 1
                dr1("vWorkspaceId") = Me.ViewState(VS_WorkspaceId)
                dr1("vWorkspaceDesc") = Me.ViewState(VS_WorkspaceName)
                dr1("iUserId") = Me.Session(S_UserID).ToString.Trim()
                dr1("vUserName") = Me.Session(S_UserName).ToString.Trim()
                dr1("iStageId") = Stage_Authorized.Trim()
                dr1("vStageDesc") = "Authorized"
                dr1("iModifyBy") = Me.Session(S_UserID).ToString.Trim()
                dr1("cStatusIndi") = "N"
                dt_UserRights.Rows.Add(dr1)
                dt_UserRights.AcceptChanges()


                If dt_UserRights.Rows.Count > 0 Then

                    ds_Save = New DataSet
                    dt_UserRights.TableName = "View_WorkspaceDefaultWorkFlowUserDtl"
                    ds_Save.Tables.Add(dt_UserRights.Copy())

                    If Not objLambda.Save_WorkspaceDefaultWorkFlowUserDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Save, Me.Session(S_UserID), estr) Then
                        Me.objCommon.ShowAlert("Error While Saving WorkspaceDefaultWorkFlowUserDtl", Me.Page)
                        Exit Function

                    End If

                End If

            End If

            'For Removing ActivitySpecific User Rights of Project 
            ds_Save = New DataSet
            dt_UserRights = New DataTable

            If Not objHelp.getworkspaceWorkflowUserDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_Save, eStr_Retu) Then

                Response.Write(eStr_Retu)
                Exit Function

            End If

            dt_UserRights = ds_Save.Tables(0)
            'dt_UserRights.Clear()
            Duplicate = False

            For Each dr1 In dt_UserRights.Rows

                'Because as per discussion other than him/her all users will disabled
                If dr1("iUserId") = Me.Session(S_UserID).ToString.Trim() Then
                    Duplicate = True
                    Continue For
                End If

                dr1("cStatusIndi") = "D"
                dr1("iModifyBy") = Me.Session(S_UserID).ToString.Trim()
                dt_UserRights.AcceptChanges()

            Next dr1

            If dt_UserRights.Rows.Count > 0 Then

                ds_Save = New DataSet
                dt_UserRights.TableName = "WorkSpaceWorkFlowUserDtl"
                ds_Save.Tables.Add(dt_UserRights.Copy())

                If Not objLambda.Save_InsertWorkspaceWorkflowUserDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_Save, "N", _
                    Me.Session(S_UserID), estr) Then

                    Me.objCommon.ShowAlert("Error While Saving WorkspaceDefaultWorkFlowUserDtl", Me.Page)
                    Exit Function

                End If

            End If

            If Duplicate = False Then
                dr1 = dt_UserRights.NewRow()
                'nWorkspaceWorkflowUserId,vWorkspaceId,iNodeId,iUserId,iStageId,cCanEdit,cCanRead,cCanDelete,iModifyBy,dModifyOn,cStatusIndi
                dr1("nWorkspaceWorkflowUserId") = dt_UserRights.Rows.Count + 1
                dr1("vWorkspaceId") = Me.ViewState(VS_WorkspaceId)
                dr1("iNodeId") = 1
                dr1("iUserId") = Me.Session(S_UserID).ToString.Trim()
                dr1("iStageId") = Stage_Authorized.Trim()
                dr1("cCanEdit") = "Y"
                dr1("cCanRead") = "Y"
                dr1("cCanDelete") = "Y"
                dr1("iModifyBy") = Me.Session(S_UserID).ToString.Trim()
                dr1("cStatusIndi") = "N"
                dt_UserRights.Rows.Add(dr1)
                dt_UserRights.AcceptChanges()


                If dt_UserRights.Rows.Count > 0 Then

                    ds_Save = New DataSet
                    dt_UserRights.TableName = "WorkSpaceWorkFlowUserDtl"
                    ds_Save.Tables.Add(dt_UserRights.Copy())

                    If Not objLambda.Save_InsertWorkspaceWorkflowUserDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Save, "N", _
                            Me.Session(S_UserID), estr) Then

                        Me.objCommon.ShowAlert("Error While Saving WorkspaceDefaultWorkFlowUserDtl", Me.Page)
                        Exit Function

                    End If

                End If

            End If

            'For Changeing Phase of Project 
            ds_Save = New DataSet

            Wstr = "vWorkspaceId='" & Me.ViewState(VS_WorkspaceId).ToString.Trim() & "'"
            If Not objHelp.getworkspacemst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                              ds_Save, estr) Then

                Response.Write(eStr_Retu)
                Exit Function

            End If

            dt_WorksapaceMst = ds_Save.Tables(0)
            For Each dr1 In dt_WorksapaceMst.Rows

                'Changed to Archived Phase
                dr1("cProjectStatus") = "A"
                dr1("iModifyBy") = Me.Session(S_UserID).ToString.Trim()
                dt_WorksapaceMst.AcceptChanges()

            Next dr1

            ds_Save = New DataSet
            ds_Save.Tables.Add(dt_WorksapaceMst.Copy())

            If Not objLambda.Save_workspacemst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_Save, Me.Session(S_UserID), ReqId, estr) Then
                Me.objCommon.ShowAlert("Error While Saving WorkspaceDefaultWorkFlowUserDtl", Me.Page)
                Exit Function

            End If

            objCommon.ShowAlert("Project Archived Successfully", Me.Page)

            SaveValues = True


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message(), "")
            SaveValues = False
        End Try
    End Function

#End Region

#Region "Button Search"

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        SearchProject()
    End Sub

    Protected Sub txtSearchProject_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearchProject.TextChanged
        SearchProject()
    End Sub

    Private Sub SearchProject()
        Dim ds_SearchProjects As New DataSet
        Dim dv_Projects As DataView = Nothing
        Dim Type As String = ""
        Dim UserId As String = ""
        Dim Wstr As String = ""
        Dim DMS As String = ""
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            'Type = Me.Request.QueryString("Type").Trim()
            UserId = Me.Session(S_UserID) 'IIf(Type.ToUpper = "ALL", "", Me.Session(S_UserID))
            Wstr = "iUserId=" + UserId + " and vProjectNo like '%" + txtSearchProject.Text.Trim + "%'"
            If Not objHelp.View_MyProjects(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                ds_SearchProjects, eStr_Retu) Then

                objCommon.ShowAlert("Error displaying projects:" & eStr_Retu.ToString(), Me)
                Exit Sub

            End If
            If ds_SearchProjects.Tables(0).Rows.Count <= 0 Then
                objCommon.ShowAlert(" Projects Id not entered Correctly OR Project of entered ID does not exist" & eStr_Retu.ToString(), Me)
                'BindGrid()
                GenCall_ShowUI()

            ElseIf ds_SearchProjects.Tables(0).Rows.Count >= 0 Then
                gvwProjects.DataSource = ds_SearchProjects
                gvwProjects.DataBind()
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
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





    
End Class
