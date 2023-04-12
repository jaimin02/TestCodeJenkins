Imports System.Web.Services
Imports Newtonsoft.Json

Partial Class frmCopyProject
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATIONS"

    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

    Private Const VS_WorkspaceId As String = "workspaceId"
    Private Const VS_dtWorkspaceMst As String = "dtWorkspaceMst"
    Private Const VS_ProjectType As String = "ProjectType"
    Private Const VS_LocationCode As String = "LocationCode"
    Private Const VS_ProjectTypeCode As String = "ProjectTypeCode"



    Private ds_Projects As DataSet
    Private eStr_Retu As String = String.Empty

    Private Const GVC_WorkspaceId As Integer = 0
    Private Const GVC_WorkspaceDesc As Integer = 1
    Private Const GVC_ProjectTypeName As Integer = 9
    Private Const GVC_Status As Integer = 11
    Private Const GVC_ClientCode As Integer = 12
    Private Const GVC_DrugCode As Integer = 13
    Private Const GVC_LocationCode As Integer = 14
    Private Const GVC_ProjectTypeCode As Integer = 15
    Private Const GVC_LnkBtnProjectDet As Integer = 16
    Private Const GVC_LnkBtnChangeStatus As Integer = 17

    Private workspaceId As String = String.Empty

#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        clsCommon.ClearCache(Me)

        If Not IsPostBack Then
            GenCall_ShowUI()
            If gvwProjects.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvwProjects", "UIgvwProjects(); ", True)
            End If
        End If
    End Sub

#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Try
            Page.Title = " ::  Copy Project  ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblMandatory"), Label).Visible = False
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Copy Projects"
            Me.btnClose.Attributes.Add("OnClientClick", "ShowHideDivCopy('N');")
            BindGrid()

            'Added condition on 24-Jun-2009 by Chandresh Vanker

            Me.trDrug.Visible = True
            Me.trSponsor.Visible = True
            Me.trProjectName.Visible = False

            If Not IsNothing(Me.Request.QueryString("Type")) AndAlso Me.Request.QueryString("Type").Trim.ToUpper() = "DMS" Then
                Me.trDrug.Visible = False
                Me.trSponsor.Visible = False
                Me.trProjectName.Visible = True
            Else
                FillDropDown()
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_ShowUI")
        End Try


    End Function

#End Region

#Region "FillDropDown and CheckChildProject"

    Private Sub FillDropDown()
        Dim dsDrug As New DataSet
        Dim dsClient As New DataSet
        Dim ds_TemplateMst As New DataSet
        Dim Wstr As String = String.Empty
        Dim Wstr_Scope As String = String.Empty
        Dim dv_DropDown As New DataView

        Try



            If Not objCommon.GetScopeValueWithCondition(Wstr_Scope) Then
                Exit Sub
            End If


            Wstr = "cStatusIndi <> 'D'"
            If Not objHelp.getdrugmst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    dsDrug, eStr_Retu) Then

                Me.ShowErrorMessage(eStr_Retu, "")
                Exit Sub

            End If

            If Not objHelp.getclientmst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    dsClient, eStr_Retu) Then

                Me.ShowErrorMessage(eStr_Retu, "")
                Exit Sub

            End If
            'added by deepak Singh on 12-feb-10 to fill Template
            Wstr += "And " & Wstr_Scope & " and vTemplatetypecode='" & TemplateTypeCode & "'"
            If Not objHelp.getTemplateMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                  ds_TemplateMst, eStr_Retu) Then

                Me.ShowErrorMessage(eStr_Retu, "")
                Exit Sub

            End If
            '==========

            dv_DropDown = dsDrug.Tables(0).DefaultView
            dv_DropDown.Sort = "vDrugName"
            Me.SlcDrug.DataSource = dv_DropDown
            Me.SlcDrug.DataValueField = "vDrugCode"
            Me.SlcDrug.DataTextField = "vDrugName"
            Me.SlcDrug.DataBind()

            dv_DropDown = Nothing
            dv_DropDown = New DataView

            dv_DropDown = dsClient.Tables(0).DefaultView
            dv_DropDown.Sort = "vClientName"
            Me.SlcSponsor.DataSource = dv_DropDown
            Me.SlcSponsor.DataValueField = "vClientCode"
            Me.SlcSponsor.DataTextField = "vClientName"
            Me.SlcSponsor.DataBind()

            dv_DropDown = Nothing
            dv_DropDown = New DataView

            dv_DropDown = ds_TemplateMst.Tables(0).DefaultView
            dv_DropDown.Sort = "vTemplateDesc"
            Me.SlcTemplate.DataSource = dv_DropDown
            Me.SlcTemplate.DataValueField = "vTemplateId"
            Me.SlcTemplate.DataTextField = "vTemplateDesc"
            Me.SlcTemplate.DataBind()




        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "........FillDropDown")
        End Try
    End Sub
    'added by deepak singh to not copy a child Project on 12-feb-10
    Private Function CheckChildProject(ByVal Workspaceid As String) As Boolean
        Dim wstr As String = String.Empty
        Dim ds_workspaceMst As New DataSet
        Dim eStr_Retu As String = String.Empty
        Dim dr_workspaceMst As DataRow
        Try
            wstr = "vworkspaceid='" & Workspaceid & "' and cStatusindi <>'D'"

            If Not objHelp.getworkspacemst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_workspaceMst, eStr_Retu) Then
                objCommon.ShowAlert(eStr_Retu, Me)
                Exit Function
            End If
            dr_workspaceMst = ds_workspaceMst.Tables(0).Rows(0)
            If dr_workspaceMst("cWorkspaceType").ToString.ToUpper.Trim = "C" Then
                objCommon.ShowAlert("You can not copy a Child Project", Me)
                Return False
            End If

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...CheckChildProject")
        End Try

    End Function

#End Region

#Region "GRIDVIEW EVENTS"

    Private Sub BindGrid()
        Dim dv_Projects As DataView = Nothing

        Try



            ds_Projects = New DataSet
            Dim wStr As String = "iUserId=" + Me.Session(S_UserID) + "AND vWorkspaceId = ParentWorkspaceId "

            If Not objHelp.View_MyProjects(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                ds_Projects, eStr_Retu) Then

                objCommon.ShowAlert("Error displaying projects:" & eStr_Retu.ToString(), Me)
                Exit Sub

            End If

            If (Not ViewState("sortExpression") Is Nothing) And (Not ViewState("sortDirection") Is Nothing) Then

                dv_Projects = New DataView()
                dv_Projects = ds_Projects.Tables(0).DefaultView
                dv_Projects.Sort = ViewState("sortExpression").ToString & " " & ViewState("sortDirection")
                gvwProjects.DataSource = dv_Projects
                ' gvwProjects.DataBind()
                If gvwProjects.Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvwProjects", "UIgvwProjects(); ", True)
                End If
            Else

                gvwProjects.DataSource = ds_Projects
                ' gvwProjects.DataBind()
                If gvwProjects.Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvwProjects", "UIgvwProjects(); ", True)
                End If

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....BindGrid")
        End Try
    End Sub

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
            CType(e.Row.FindControl("ImgCopy"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImgCopy"), ImageButton).CommandName = "COPY"


            Dim RedirectStr As String = "frmProtocolDetail.aspx?mode=1&Workspace=" + e.Row.Cells(GVC_WorkspaceId).Text

            CType(e.Row.FindControl("ImgCopy"), ImageButton).OnClientClick = "return OpenWindow('" + RedirectStr + "');"

            CType(e.Row.FindControl("lnkBtnProDet"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkBtnProDet"), LinkButton).CommandName = "PROJECTDETAILS"

            '===added on 12-Feb-2010 by deepak to show projwct name in tooltip
            Dim str As String = Replace(CType(e.Row.FindControl("lblProjectName"), Label).Text, "*", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")
            e.Row.ToolTip = str.Replace("&nbsp;", "")
            '==============
        End If

    End Sub

    Protected Sub gvwProjects_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or _
        e.Row.RowType = DataControlRowType.Header Or _
        e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVC_WorkspaceId).Visible = False 'First Column is Hidden
            e.Row.Cells(GVC_DrugCode).Visible = False
            e.Row.Cells(GVC_ClientCode).Visible = False
            e.Row.Cells(GVC_LocationCode).Visible = False
            e.Row.Cells(GVC_ProjectTypeCode).Visible = False
            e.Row.Cells(GVC_WorkspaceDesc).Visible = False

        End If

    End Sub

    Protected Sub gvwProjects_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        gvwProjects.PageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Protected Sub gvwProjects_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        ' Dim Index As Integer = CInt(e.CommandArgument)
        Dim dt As New DataTable
        Dim workSpaceId As String = String.Empty
        Dim Type As String = String.Empty
        Dim DMS As String = String.Empty
        Try
            Dim Index As Integer = e.CommandArgument
            workSpaceId = Me.gvwProjects.Rows(Index).Cells(GVC_WorkspaceId).Text

            If e.CommandName.ToUpper = "COPY" Then

                If Not CheckChildProject(workSpaceId) Then
                    Exit Sub
                End If

                ViewState(VS_WorkspaceId) = Me.gvwProjects.Rows(Index).Cells(GVC_WorkspaceId).Text.Trim
                ViewState(VS_ProjectType) = Me.gvwProjects.Rows(Index).Cells(GVC_ProjectTypeName).Text.Trim
                '=====added by deepak singh on 27-Jan-10 to save Locationcode and projecttypecode(for RID)
                ViewState(VS_LocationCode) = Me.gvwProjects.Rows(Index).Cells(GVC_LocationCode).Text.Trim
                ViewState(VS_ProjectTypeCode) = Me.gvwProjects.Rows(Index).Cells(GVC_ProjectTypeCode).Text.Trim
                '==========
                Me.SlcDrug.SelectedValue = IIf(Me.gvwProjects.Rows(Index).Cells(GVC_DrugCode).Text Is System.DBNull.Value, 0, Me.gvwProjects.Rows(Index).Cells(GVC_DrugCode).Text)
                Me.SlcSponsor.SelectedValue = IIf(Me.gvwProjects.Rows(Index).Cells(GVC_ClientCode).Text Is System.DBNull.Value, 0, Me.gvwProjects.Rows(Index).Cells(GVC_ClientCode).Text)

                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "ShowHideDivCopy", "ShowHideDivCopy('Y');", True)

            ElseIf e.CommandName.ToUpper = "PROJECTDETAILS" Then

                Type = Me.Request.QueryString("Type").Trim()
                DMS = IIf(Convert.ToString(Me.Request.QueryString("DMS")) Is Nothing, "", Convert.ToString(Me.Request.QueryString("DMS")))
                Response.Redirect("frmProjectDetailMst.aspx?WorkSpaceId=" & workSpaceId & "&Page=frmCopyProject&Type=" & Type & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))

            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....gvwProjects_RowCommand")
        End Try

    End Sub

#End Region

    'Need To See..........................................................

#Region "SAVE "

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dsWorkspaceMst As New DataSet
        Dim dtWorkspaceMst As New DataTable
        Dim RequestID As String = String.Empty
        Dim Retu_WorkspaceId As String = String.Empty
        Dim dsWorkspaceNodeHistory As New DataSet
        Dim dtWorkspaceNodeHistory As New DataTable

        Dim wStr As String = String.Empty
        Dim dt_WorkspaceId As New DataTable ' Used to get workspaceId of copied project

        Dim dir As DirectoryInfo
        Dim DocPath As String = String.Empty
        Dim FileName As String = String.Empty
        Dim FileMove As FileInfo
        Try



            CreateDataTable()
            AssignValues()

            dtWorkspaceMst = ViewState(VS_WorkspaceId)
            dtWorkspaceMst.TableName = "WorkspaceMst"
            dsWorkspaceMst = New DataSet
            dsWorkspaceMst.Tables.Add(dtWorkspaceMst)

            If Not objLambda.Proc_CopyProjects(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                dsWorkspaceMst, Session(S_UserID), RequestID, Retu_WorkspaceId, eStr_Retu) Then

                objCommon.ShowAlert(eStr_Retu.Replace("'", "\'") + " : " + "Error While changing project status", Me)
                Exit Sub
            End If

            'Copying Authorized documents with folders to newly created project by chadresh vanker on 25-jun-09.

            wStr = "iStageId=20 and cStatusIndi <> 'D' and vWorkSpaceId='" + Retu_WorkspaceId + "'"
            If Not Me.objHelp.GetViewWorkSpaceNodeHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                        dsWorkspaceNodeHistory, eStr_Retu) Then
                objCommon.ShowAlert("Error While Getting Data From View_WorkspaceNodeHistory" + eStr_Retu, Me)
                Exit Sub
            End If

            If dsWorkspaceNodeHistory.Tables(0).Rows.Count > 1 Then
                dtWorkspaceNodeHistory = dsWorkspaceNodeHistory.Tables(0).DefaultView.ToTable(True, "vWorkSpaceId,vFileName,vFolderName,vDocPath".Split(","))

                For Each dr As DataRow In dtWorkspaceNodeHistory.Rows

                    DocPath = dr("vDocPath").ToString.Trim()
                    DocPath = DocPath.Substring(0, DocPath.IndexOf("/") + 1)
                    DocPath += Retu_WorkspaceId + "/"
                    DocPath += dr("vFolderName").ToString.Trim()

                    dir = New DirectoryInfo(Server.MapPath(DocPath))

                    If Not dir.Exists() Then
                        dir.Create()
                    End If

                    dt_WorkspaceId = ViewState(VS_WorkspaceId)
                    FileName = dr("vDocPath").ToString.Trim()
                    FileName = FileName.Replace("/" + Retu_WorkspaceId + "/", "/" + dt_WorkspaceId.Rows(0)("vWorkspaceId").ToString() + "/")

                    FileMove = New FileInfo(Server.MapPath(FileName))

                    FileName = dr("vDocPath").ToString.Trim()

                    FileMove.CopyTo(Server.MapPath(FileName))

                Next dr

            End If

            '*******************************************************************

            BindGrid()
            objCommon.ShowAlert("Project Copied Successfully With Reqest Id: " & RequestID, Me)
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "ShowHideDivCopy", "ShowHideDivCopy('N');", True)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......btnSave_Click")
        End Try

    End Sub

#End Region

#Region "AssignValues & CreateDataTable"

    Private Sub AssignValues()

        Dim dtWorkspaceMst As DataTable = ViewState(VS_dtWorkspaceMst)
        Dim dr As DataRow
        Dim WorkSpaceDesc As String = String.Empty
        workspaceId = ViewState(VS_WorkspaceId)

        dr = dtWorkspaceMst.NewRow
        dr("vWorkSpaceId") = workspaceId

        'Added condition on 24-Jun-2009 by Chandresh Vanker

        Try
            If Me.trDrug.Visible AndAlso Me.trSponsor.Visible Then

                WorkSpaceDesc = Me.ViewState(VS_ProjectType) & "-" & _
                            Me.SlcDrug.Items(Me.SlcDrug.SelectedIndex).Text.Trim() & "-" & _
                            Me.SlcSponsor.Items(Me.SlcSponsor.SelectedIndex).Text.Trim()

                dr("vDrugCode") = Me.SlcDrug.SelectedValue
                dr("vClientCode") = Me.SlcSponsor.SelectedValue
                '====added on 27-Jan-10 by Deepak Singh
                dr("vLocationCode") = CType(ViewState(VS_LocationCode), String)
                dr("vProjectTypeCode") = CType(ViewState(VS_ProjectTypeCode), String)
                '========

            Else
                WorkSpaceDesc = Me.txtProjectName.Text.Trim()
                dr("vDrugCode") = "0000"
                dr("vClientCode") = "0000"

            End If

            '*****************************************************
            dr("vWorkSpaceDesc") = WorkSpaceDesc
            'dr("UserRights") = Me.RBLUserRights.SelectedValue.Trim()
            'dr("Attributes") = Me.RBLAttributes.SelectedValue.Trim()
            dr("vTemplateid") = Me.SlcTemplate.SelectedValue.Trim()
            dr("Docs") = Me.RBLDoc.SelectedValue.Trim()
            dr("iModifyBy") = Session(S_UserID)

            dtWorkspaceMst.Rows.Add(dr)

            ViewState(VS_WorkspaceId) = dtWorkspaceMst
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....AssignValues")
        End Try

    End Sub

    Private Sub CreateDataTable()

        Dim dt As New DataTable
        dt.TableName = "WorkspaceMst"
        Dim dc As New DataColumn
        Try
            dt.Columns.Add(New DataColumn("vWorkspaceId", GetType(String)))
            dt.Columns.Add(New DataColumn("vWorkSpaceDesc", GetType(String)))
            dt.Columns.Add(New DataColumn("vDrugCode", GetType(String)))
            dt.Columns.Add(New DataColumn("vClientCode", GetType(String)))
            'dt.Columns.Add(New DataColumn("UserRights", GetType(String)))
            'dt.Columns.Add(New DataColumn("Attributes", GetType(String)))
            dt.Columns.Add(New DataColumn("vTemplateId", GetType(String)))
            dt.Columns.Add(New DataColumn("Docs", GetType(String)))
            dt.Columns.Add(New DataColumn("iModifyBy", GetType(Integer)))
            '====added on 27-Jan-10 by Deepak Singh
            dt.Columns.Add(New DataColumn("vLocationCode", GetType(String)))
            dt.Columns.Add(New DataColumn("vProjectTypeCode", GetType(String)))
            '==================
            dt.Columns.Add(New DataColumn("vRequestId", GetType(String)))

            ViewState(VS_dtWorkspaceMst) = dt
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..CreateTable")
        End Try




    End Sub

#End Region
    '***********************************************************
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

    <WebMethod> _
    Public Shared Function View_MyProjects() As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim wStr As String = "iUserId=" + HttpContext.Current.Session(S_UserID) + "AND vWorkspaceId = ParentWorkspaceId "
        Dim ds_Projects As New DataSet
        ' Dim dv_Projects As New DataView
        Dim eStr_Retu As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim dt As New DataTable
        Dim ds_Projectsfinal As New DataSet

        Try
            If Not objHelp.View_MyProjects(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                ds_Projects, eStr_Retu) Then
                Return False
            End If

            dt = ds_Projects.Tables(0)
            Dim dv_Projects As New DataView(dt)
            dv_Projects.Sort = "vWorkspaceId DESC"

            ds_Projectsfinal.Tables.Add(dv_Projects.ToTable())
            strReturn = JsonConvert.SerializeObject(ds_Projectsfinal)
            Return strReturn




        Catch ex As Exception

            Return strReturn
        End Try


    End Function
    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click

      
        Me.Response.Redirect("frmProtocolDetail.aspx?mode=1&Workspace=" & Me.hdnEditedId.Value)
       

    End Sub
    Protected Sub btnLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLink.Click
        Dim Type As String = String.Empty
        Dim DMS As String = String.Empty


        Type = Me.Request.QueryString("Type").Trim()
        DMS = IIf(Convert.ToString(Me.Request.QueryString("DMS")) Is Nothing, "", Convert.ToString(Me.Request.QueryString("DMS")))
        Response.Redirect("frmProjectDetailMst.aspx?WorkSpaceId=" & Me.hdnlinkedId.Value & "&Page=frmCopyProject&Type=" & Type & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))


    End Sub
End Class