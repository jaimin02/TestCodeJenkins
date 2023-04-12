
Partial Class frmMyProject
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATIONS"

    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

    Private Const VS_WorkspaceId As String = "workspaceId"
    Private Const VS_MyProject As String = "Myproject"
    Private Const VS_CurrentPage As String = "PageNo"
    Private Const VS_TotalRowCount As String = "TotalRowCount"
    Private Const VS_PagerStartPage As String = "PagerStartPage"

    Private ds_Projects As DataSet
    Private eStr_Retu As String = String.Empty

    Private Const GVC_WorkspaceId As Integer = 0
    Private Const GVC_WorkspaceDesc As Integer = 1
    Private Const GVC_vProjectNo As Integer = 3
    Private Const GVC_Status As Integer = 11
    Private Const GVC_LnkBtnChangeStatus As Integer = 12
    Private Const GVC_LnkBtnSubDet As Integer = 13
    Private Const GVC_LnkBtnProjectDet As Integer = 14
    Private Const GVC_DefaultUserRights As Integer = 15
    Private Const GVC_cWorkspaceType As Integer = 16

    Private Const Phase_PreClinical As String = "PRE-CLINICAL"
    Private Const Phase_Study As String = "STUDY"
    Private Const Phase_Terminated As String = "TERMINATED"
    Private Const Phase_InitialQuotation As String = "INITIAL QUOTATION"
    Private Const Phase_DocumentPreparation As String = "DOCUMENT PREPARATION"
    Private Const Phase_NotRewarded As String = "NOT REWARDED"
    Private Const Phase_Analysis As String = "SAMPLE ANALYSIS"

    Private Const TemplateType_ClinicalStudy As String = "0002"
    Private Const TemplateType_DocumentPreparation As String = "0003"
    Private Const PAGESIZE As Integer = 25

#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        clsCommon.ClearCache(Me)


        If Not IsPostBack Then
            If Not GenCall_ShowUI() Then
                Exit Sub
            End If
        End If

    End Sub

#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim sender As New Object
        Dim e As New EventArgs
        Dim scopeno As String = String.Empty
        Dim PMScopeNo As String = Scope_SAll.ToString()
        Try
            Page.Title = "::  My Projects :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblMandatory"), Label).Visible = False
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "My Projects"
            Me.btnClose.Attributes.Add("OnClientClick", "ShowHideDiv('N');")
            Me.txtPeriod.Attributes.Add("onblur", "return Numeric();")

            'added by Deepak Singh on 2-Mar-10 to show the set Project on page load
            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
            End If

            '==added on 15-jan-2010 by deepak singh to show project according to user
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)

            '' === For give all rights to pmadmin ==
            scopeno = Me.Session(S_ScopeNo).ToString()

            If (scopeno = PMScopeNo) Then
                Me.AutoCompleteExtender1.ServiceMethod = "GetMyProjectCompletionListwithworkspacedesc"
            Else
                Me.AutoCompleteExtender1.ServiceMethod = "GetMyProjectCompletionList"
            End If

            '========
            'BindGrid()

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....GenCall_ShowUI")
        End Try
    End Function

#End Region

#Region "Gencall_Data"

    Private Function Gencall_Data() As Boolean
        ds_Projects = New DataSet
        Dim Wstr As String = String.Empty
        Dim dsCount As New DataSet
        Try


            'Providing parameteres to procedure
            Wstr = Me.Session(S_UserID) + "##" + Convert.ToString(Me.ViewState(VS_CurrentPage)).Trim() + "##" + PAGESIZE.ToString()

            If Not Me.objHelp.Proc_MyProjects(Wstr, ds_Projects, eStr_Retu) Then
                Throw New Exception("Error while getting projects:" & eStr_Retu.ToString())
            End If

            ViewState(VS_MyProject) = ds_Projects

            Gencall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....Gencall_Data")
        End Try
    End Function

#End Region

#Region "FillDropDown"

    Private Sub FillDropDown(ByVal TTC As String)
        Dim dsDropDown As New DataSet
        Dim Wstr As String = String.Empty
        Dim Wstr_Scope As String = String.Empty
        Dim dv_DropDown As New DataView
        Try


            'To Get Where condition of ScopeVales( Project Type )
            If Not objCommon.GetScopeValueWithCondition(Wstr_Scope) Then
                Exit Sub
            End If

            Wstr = Wstr_Scope & " AND vTemplateTypeCode = '" & TTC & "'"
            If Not objHelp.GetViewTemplateMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsDropDown, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            If dsDropDown.Tables(0).Rows.Count <= 0 Then
                objCommon.ShowAlert("No Record Found For Template DropDown", Me.Page)
                ddlTemplate.Enabled = False
                Exit Sub
            End If

            dv_DropDown = dsDropDown.Tables(0).DefaultView
            dv_DropDown.Sort = "vTemplateDesc"
            ddlTemplate.DataSource = dv_DropDown
            ddlTemplate.DataValueField = "vTemplateId"
            ddlTemplate.DataTextField = "vTemplateDesc"
            ddlTemplate.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ThemeSelection();", "ThemeSelection();", True)

            For Cnt As Integer = 0 To Me.ddlTemplate.Items.Count - 1
                Me.ddlTemplate.Items(Cnt).Attributes.Add("title", Me.ddlTemplate.Items(Cnt).Text.Trim())
            Next Cnt

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".......FillDropDown")
        End Try
    End Sub

    Private Function BindStatusDropDown(ByVal filter As String) As DataTable
        Dim dtStatus As DataTable = Nothing
        Dim dr As DataRow
        Dim dvStatusTemp As DataView = Nothing
        Dim dvStatus As DataView = Nothing
        Try

            dtStatus = New DataTable("dtStatus")
            dtStatus.Columns.Add(New DataColumn("SrNo", GetType(Integer)))
            dtStatus.Columns.Add(New DataColumn("StatusValue", GetType(String)))
            dtStatus.Columns.Add(New DataColumn("StatusText", GetType(String)))

            dr = dtStatus.NewRow
            dr("SrNo") = 0
            dr("StatusValue") = "I"
            dr("StatusText") = "Initial Quotation"
            dtStatus.Rows.Add(dr)

            dr = dtStatus.NewRow
            dr("SrNo") = 1
            dr("StatusValue") = "P"
            dr("StatusText") = "Pre-Clinical"   'Pre-Clinical replace by Pre-Study
            dtStatus.Rows.Add(dr)

            dr = dtStatus.NewRow
            dr("SrNo") = 2
            dr("StatusValue") = "S"
            dr("StatusText") = "Study"
            dtStatus.Rows.Add(dr)

            '===Added By Mrunal Parekh on 12-Jan-2012 for Analysis Phase
            ' If Not filter = "Study" Then
            dr = dtStatus.NewRow
            dr("SrNo") = 3
            dr("StatusValue") = "L"
            dr("StatusText") = "Sample Analysis"
            dtStatus.Rows.Add(dr)
            ' End If

            dr = dtStatus.NewRow
            dr("SrNo") = 4
            dr("StatusValue") = "D"
            dr("StatusText") = "Document Preparation"
            dtStatus.Rows.Add(dr)

            dr = dtStatus.NewRow
            dr("SrNo") = 5
            dr("StatusValue") = "N"
            dr("StatusText") = "Not Rewarded"
            dtStatus.Rows.Add(dr)

            dr = dtStatus.NewRow
            dr("SrNo") = 6
            dr("StatusValue") = "C"
            dr("StatusText") = "Completed"
            dtStatus.Rows.Add(dr)

            dr = dtStatus.NewRow
            dr("SrNo") = 7
            dr("StatusValue") = "A"
            dr("StatusText") = "Archived"
            dtStatus.Rows.Add(dr)

            dr = dtStatus.NewRow
            dr("SrNo") = 8
            dr("StatusValue") = "T"
            dr("StatusText") = "Terminated"
            dtStatus.Rows.Add(dr)

            If (filter.ToUpper = "Study COMPLETED" Or filter.ToUpper = "SAMPLE ANALYSIS COMPLETED" Or filter.ToUpper = "DOCUMENT PREPARATION") Then
                filter = "Sample Analysis"
            End If
            dvStatusTemp = New DataView(dtStatus)
            dvStatusTemp.RowFilter = " StatusText = '" & filter & "'"

            dvStatus = New DataView(dtStatus)
            dvStatus.RowFilter = "SrNo > " + dvStatusTemp.Item(0)(0).ToString
            dtStatus = dvStatus.ToTable

            Return dtStatus

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....BindStatusDropDown")
            Return Nothing
        End Try
    End Function

#End Region

#Region "GRIDVIEW EVENTS"

    Private Sub BindGrid()
        ds_Projects = New DataSet
        Dim dv_Projects As DataView = Nothing
        Dim Type As String = String.Empty
        Dim DMS As String = String.Empty
        Try


            Try
                Type = Convert.ToString(Me.Request.QueryString("Type")).Trim()
            Catch ex As Exception
            End Try


            ds_Projects = CType(ViewState(VS_MyProject), DataSet)

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
            ''Added By Vivek Patel
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gvwProjectsUI", "gvwProjectsUI(); ", True)
            ''Completed By Vivek Patel
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ThemeSelection();", "ThemeSelection();", True)
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fixheader", "fixheader()", True)
            DMS = IIf(Me.Request.QueryString("DMS") Is Nothing, "", Convert.ToString(Me.Request.QueryString("DMS")))

            If Type.ToUpper = "OPERATIONAL" Then

                Me.gvwProjects.Columns(GVC_LnkBtnChangeStatus).Visible = False
                Me.gvwProjects.Columns(GVC_DefaultUserRights).Visible = False

                If Not IsNothing(DMS) AndAlso DMS.ToUpper = "Y" Then

                    Me.gvwProjects.Columns(GVC_LnkBtnChangeStatus).Visible = False
                    Me.gvwProjects.Columns(GVC_DefaultUserRights).Visible = True

                End If

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...BindGrid")
        End Try
    End Sub

    Protected Sub gvwProjects_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvwProjects.DataBound

    End Sub

    Protected Sub gvwProjects_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvwProjects.Sorting
        Try

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
                    ViewState("sortDirection") = "ASC"
                    If ViewState("sortDirection").ToString.Contains("ASC") Then
                        ViewState("sortDirection") = "DESC"
                    End If
                End If
            End If

            BindGrid()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....gvwProjects_Sorting")
        End Try
    End Sub

    Protected Sub gvwProjects_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwProjects.RowDataBound
        Dim Type As String = String.Empty
        Dim DMS As String = String.Empty
        Dim str As String = String.Empty
        Dim workspacedesc As String = String.Empty
        Dim RedirectStr As String = String.Empty

        Try
            Try
                e.Row.Cells(GVC_cWorkspaceType).Visible = False
                Type = Convert.ToString(Me.Request.QueryString("Type")).Trim()
                DMS = IIf(Me.Request.QueryString("DMS") Is Nothing, "", Convert.ToString(Me.Request.QueryString("DMS")))
            Catch ex As Exception

            End Try

            If e.Row.RowType = DataControlRowType.DataRow Then

                If Not e.Row.Cells(GVC_Status).Text.Trim.ToUpper = Phase_Study Then
                    e.Row.Cells(GVC_LnkBtnSubDet).FindControl("lnkBtnSubDet").Visible = False
                End If

                '===added on 13-jan-2010 by deepak to show projwct name in tooltip
                str = Replace(CType(e.Row.FindControl("lblProjectName"), Label).Text, "*", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")
                If e.Row.ToolTip <> Nothing AndAlso str <> String.Empty Then
                    e.Row.ToolTip = str.Replace("&nbsp;", "")
                End If


                '==============

                If Type.ToUpper = "OPERATIONAL" Then

                    Me.gvwProjects.Columns(GVC_LnkBtnChangeStatus).Visible = False
                    Me.gvwProjects.Columns(GVC_DefaultUserRights).Visible = False

                    If Not IsNothing(DMS) AndAlso DMS.ToUpper = "Y" Then

                        Me.gvwProjects.Columns(GVC_LnkBtnChangeStatus).Visible = False
                        Me.gvwProjects.Columns(GVC_DefaultUserRights).Visible = True

                    End If

                ElseIf Type.ToUpper = "MONITORING" Then

                    Me.gvwProjects.Columns(GVC_LnkBtnChangeStatus).Visible = False
                    Me.gvwProjects.Columns(GVC_DefaultUserRights).Visible = False

                ElseIf Type.ToUpper() = "DUTYDELEGATION" Then

                    Me.gvwProjects.Columns(GVC_LnkBtnChangeStatus).Visible = False

                End If

            End If


            If e.Row.RowType = DataControlRowType.DataRow Then

                'Added by Mrunal on 28-Nov-2011 to Get dutydelegation type in Querystring
                workspacedesc = CType(e.Row.Cells(GVC_WorkspaceDesc).FindControl("lblProjectName"), Label).Text

                If Type.ToUpper() = "DUTYDELEGATION" Then
                    RedirectStr = "frmWorkspaceDefaultWorkflowUserDtl.aspx?mode=1&page=frmMyProject&Type=" _
                                    & Type & "&WorkspaceId=" & e.Row.Cells(GVC_WorkspaceId).Text.ToString() & _
                                 "&WorkspaceName=" & CType(e.Row.Cells(GVC_WorkspaceDesc).FindControl("lblProjectName"), Label).Text & _
                                 "&ProjectNo=" & e.Row.Cells(GVC_vProjectNo).Text.ToString()
                Else
                    RedirectStr = "frmWorkspaceDefaultWorkflowUserDtl.aspx?mode=1&page=frmMyProject&Type=ALL&WorkspaceId=" & e.Row.Cells(GVC_WorkspaceId).Text.ToString() & _
                                 "&WorkspaceName=" & CType(e.Row.Cells(GVC_WorkspaceDesc).FindControl("lblProjectName"), Label).Text & _
                                 "&ProjectNo=" & e.Row.Cells(GVC_vProjectNo).Text.ToString()
                End If

                CType(e.Row.Cells(GVC_DefaultUserRights).FindControl("lbldocright"), HtmlAnchor).HRef = RedirectStr
                If hndLockStatus.Value.Trim = "Lock" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "EnabledControlForProjectLock", "EnabledControlForProjectLock();", True)
                End If

            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gvwProjectsUI", "gvwProjectsUI(); ", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".......gvwProjects_RowDataBound")
        End Try
    End Sub

    Protected Sub gvwProjects_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwProjects.RowCreated

        If e.Row.RowType = DataControlRowType.DataRow Or _
        e.Row.RowType = DataControlRowType.Header Or _
        e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GVC_WorkspaceId).Visible = False 'First Column is Hidden
            '===added on 13-jan-2010 by deepak singh
            e.Row.Cells(GVC_WorkspaceDesc).Visible = False
            '===========
        End If

    End Sub

    Protected Sub lnkBtnSubDet_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim gvr As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        Dim workSpaceId As String = gvr.Cells(GVC_WorkspaceId).Text
        Response.Redirect("frmSubjectDetail.aspx?WorkSpaceId=" & workSpaceId & "&Page=frmMyProject")
    End Sub

    Protected Sub lnkBtnProDet_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim gvr As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        Dim workSpaceId As String = gvr.Cells(GVC_WorkspaceId).Text
        Dim Type As String = String.Empty
        Dim DMS As String = String.Empty
        Try
            Type = Me.Request.QueryString("Type").Trim()
        Catch ex As Exception
        End Try
        DMS = IIf(Me.Request.QueryString("DMS") Is Nothing, "", Convert.ToString(Me.Request.QueryString("DMS")))
        Response.Redirect("frmProjectDetailMst.aspx?WorkSpaceId=" & workSpaceId & "&Page=frmMyProject&Type=" & Type & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))
    End Sub

    Protected Sub lnkBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim gvr As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        Dim currentStatus As String = gvr.Cells(GVC_Status).Text.Trim
        Dim dt As New DataTable
        Try

            ViewState(VS_WorkspaceId) = gvr.Cells(GVC_WorkspaceId).Text.Trim
            Me.HFProjectStatus.Value = gvr.Cells(GVC_cWorkspaceType).Text.ToUpper.Trim
            If currentStatus.ToUpper = Phase_InitialQuotation Or currentStatus.ToUpper = Phase_PreClinical Then
                Me.objCommon.ShowAlert("This Is Only To Inform You That If You Want To Edit Project Detail Then Change It Right Now.", Me)
            End If

            dt = BindStatusDropDown(currentStatus)

            ddlStatus.Items.Clear()
            ddlStatus.DataSource = dt
            ddlStatus.DataValueField = "StatusValue"
            ddlStatus.DataTextField = "StatusText"
            ddlStatus.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ThemeSelection();", "ThemeSelection();", True)

            Me.ddlTemplate.Enabled = False
            Me.txtPeriod.Enabled = False



            If ddlStatus.SelectedValue.ToUpper = "S" Then
                If gvr.Cells(GVC_cWorkspaceType).Text.ToUpper.Trim <> "C" Then
                    FillDropDown(TemplateType_ClinicalStudy)
                    ddlTemplate.Enabled = True
                End If
                Me.txtPeriod.Enabled = True

            ElseIf ddlStatus.SelectedValue.ToUpper = "D" Then
                'If gvr.Cells(GVC_cWorkspaceType).Text.ToUpper.Trim <> "C" Then ' commented by megha for demo purpose
                '    FillDropDown(TemplateType_DocumentPreparation)
                '    ddlTemplate.Enabled = True
                'End If
                Me.txtPeriod.Enabled = True

            End If


            CType(Me.Master.FindControl("Scriptmanager1"), ScriptManager).RegisterClientScriptBlock(Me, Me.GetType, "ShowHideDiv", "ShowHideDiv('Y');", True)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....lnkBtn_Click")
        End Try
    End Sub

#End Region

#Region "ddlStatus SelectedIndexChanged"

    Protected Sub ddlStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            Select Case ddlStatus.SelectedItem.Text.Trim.ToUpper()

                Case Phase_PreClinical
                    Me.ddlTemplate.Enabled = False
                    Me.txtPeriod.Enabled = False

                Case Phase_Study
                    If Me.HFProjectStatus.Value <> "C" Then
                        ddlTemplate.Enabled = True
                        FillDropDown(TemplateType_ClinicalStudy)
                    End If
                    Me.txtPeriod.Enabled = True


                Case Phase_Terminated
                    Me.ddlTemplate.Enabled = False
                    Me.txtPeriod.Enabled = False

                Case Phase_DocumentPreparation
                    'If Me.HFProjectStatus.value <> "C" Then
                    '    ddlTemplate.Enabled = True
                    '    FillDropDown(TemplateType_DocumentPreparation)
                    'End If
                    Me.ddlTemplate.Enabled = False
                    Me.txtPeriod.Enabled = False


                Case Phase_Analysis 'Added By Mrunal PArekh on 12-Jan-2012 for Analysis Phase
                    'If Me.HFProjectStatus.Value = "C" Then
                    '    ddlTemplate.Enabled = True
                    '    FillDropDown(TemplateType_ClinicalStudy)
                    'End If
                    Me.ddlTemplate.Enabled = False
                    Me.txtPeriod.Enabled = True


                Case Else
                    ddlTemplate.Enabled = False
                    Me.txtPeriod.Text = "1"
                    Me.txtPeriod.Enabled = False

            End Select

            CType(Me.Master.FindControl("Scriptmanager1"), ScriptManager).RegisterClientScriptBlock(Me, Me.GetType, "ShowHideDiv", "ShowHideDiv('Y');", True)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......ddlStatus_SelectedIndexChanged")
        End Try
    End Sub

#End Region

#Region "Helper Functions/Subs"

    Private Function GetRecordsCounts() As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim dsCount As New DataSet
        Try


            If Me.HProjectId.Value.ToString.Trim() <> "" Then
                Me.ViewState(VS_TotalRowCount) = "1"
                Return True
            End If

            wStr = Me.Session(S_UserID)

            If Not Me.objHelp.Proc_GetMyProjectsCount(wStr, dsCount, eStr) Then
                Throw New Exception("Error getting count for projects:" & eStr.ToString())
            End If

            Me.ViewState(VS_TotalRowCount) = Convert.ToString(dsCount.Tables(0).Rows(0)(0))

            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....GetRecordsCounts")
            Return False
        End Try

    End Function



    Private Function AddLnkButton(ByVal ID_1 As String, ByVal CommandName_1 As String, _
                                  ByVal CommandArg_1 As String, ByVal Text_1 As String, _
                                  Optional ByVal IsEnablePostBack As Boolean = True) As LinkButton
        Dim lnkButton As New LinkButton
        Dim lnkButtonBottom As New LinkButton
        Dim ltr As Literal
        Dim ltrBottom As Literal
        lnkButton = New LinkButton()
        ltr = New Literal()
        lnkButtonBottom = New LinkButton()
        ltrBottom = New Literal()
        lnkButton.ID = "Top" + ID_1
        lnkButton.CommandName = CommandName_1
        lnkButton.CommandArgument = CommandArg_1
        lnkButton.Text = Text_1
        lnkButton.CssClass = "PagerLinks"
        AddHandler lnkButton.Click, AddressOf PagerButton_Click
        If Not IsEnablePostBack Then
            lnkButton.OnClientClick = "return false;"
            lnkButton.Font.Underline = False
        End If

        lnkButtonBottom.ID = "Bottom" + ID_1
        lnkButtonBottom.CommandName = CommandName_1
        lnkButtonBottom.CommandArgument = CommandArg_1
        lnkButtonBottom.Text = Text_1
        lnkButtonBottom.CssClass = "PagerLinks"
        AddHandler lnkButtonBottom.Click, AddressOf PagerButton_Click
        If Not IsEnablePostBack Then
            lnkButtonBottom.OnClientClick = "return false;"
            lnkButtonBottom.Font.Underline = False
        End If

        Me.phTopPager.Controls.Add(lnkButton)
        Me.phBottomPager.Controls.Add(lnkButtonBottom)
        ltr.Text = "&nbsp;"
        ltrBottom.Text = "&nbsp;"
        Me.phTopPager.Controls.Add(ltr)
        Me.phBottomPager.Controls.Add(ltrBottom)
        Return lnkButton
    End Function

    Protected Sub PagerButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lnkButton As LinkButton
        Dim totalPages As Integer = 1
        totalPages = Me.GetTotalPages()
        lnkButton = CType(sender, LinkButton)
        Me.ViewState(VS_CurrentPage) = lnkButton.CommandArgument

        If lnkButton.CommandName.ToUpper = "BtnEllipseNext".ToUpper.ToString Then
            If (Integer.Parse(totalPages) - Integer.Parse(lnkButton.CommandArgument)) < 9 Then
                Me.ViewState(VS_PagerStartPage) = (Integer.Parse(totalPages) - 9)
            Else
                Me.ViewState(VS_PagerStartPage) = lnkButton.CommandArgument
            End If
        ElseIf lnkButton.CommandName.ToUpper = "BtnEllipsePrev".ToUpper.ToString Or _
                lnkButton.CommandName.ToUpper = "BtnLastPage".ToUpper.ToString Then
            If (Integer.Parse(lnkButton.CommandArgument) - 10) < 1 Then
                Me.ViewState(VS_PagerStartPage) = 1
            Else
                Me.ViewState(VS_PagerStartPage) = Integer.Parse(lnkButton.CommandArgument) - 9
            End If
        ElseIf lnkButton.CommandName.ToUpper = "BtnFirstPage".ToUpper.ToString Then
            Me.ViewState(VS_PagerStartPage) = 1
        End If

        If Not Me.Gencall_Data() Then
            Throw New Exception()
        End If

        Me.BindGrid()

    End Sub

    Private Function GetTotalPages() As Integer
        GetTotalPages = 1
        If Not Me.ViewState(VS_TotalRowCount) Is Nothing Then
            GetTotalPages = Me.ViewState(VS_TotalRowCount)
        End If

        If GetTotalPages > PAGESIZE Then
            GetTotalPages = Math.Ceiling(Double.Parse(Me.ViewState(VS_TotalRowCount)) / PAGESIZE)
        End If
    End Function

    Private Sub ClearControls()
        gvwProjects.DataSource = Nothing
        gvwProjects.DataBind()
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ThemeSelection();", "ThemeSelection();", True)
        Me.ViewState(VS_PagerStartPage) = Nothing
        Me.ViewState(VS_CurrentPage) = Nothing
        Me.ViewState(VS_TotalRowCount) = Nothing
        Me.phBottomPager.Controls.Clear()
        Me.phTopPager.Controls.Clear()
    End Sub

    Private Function UpdateProjectStatus(ByRef dsWorkSpaceStatusDtl As DataSet) As Boolean
        Dim DrNew As DataRow
        Try

            If Not objHelp.getWorkSpaceStatusDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, dsWorkSpaceStatusDtl, eStr_Retu) Then
                Throw New Exception("Error While Getting Data From WorkSpaceStatusDtl:" + eStr_Retu)
            End If

            DrNew = dsWorkSpaceStatusDtl.Tables(0).NewRow()
            DrNew("nWorkSpaceStatusDtlNo") = 0
            DrNew("vWorkSpaceId") = ViewState("workspaceId")
            DrNew("cProjectStatus") = Me.ddlStatus.SelectedValue.Trim()
            DrNew("dStatusChangedOn") = Date.Now.ToString()
            DrNew("vRemarks") = Me.txtReason.Text.Trim()
            DrNew("iModifyBy") = Me.Session(S_UserID)
            DrNew("dModifyOn") = Date.Now.ToString()
            DrNew("cStatusIndi") = "N"
            dsWorkSpaceStatusDtl.Tables(0).Rows.Add(DrNew)
            dsWorkSpaceStatusDtl.Tables(0).AcceptChanges()
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".......UpdateProjectStatus")
            Return False
        End Try

    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dsWorkspaceMst As New DataSet
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds_WorkSpacemst As New DataSet
        Dim dtWorkspaceMst As New DataTable
        Dim dsWorkSpaceStatusDtl As New DataSet
        Try

            If ddlStatus.SelectedItem.Text.Trim.ToUpper() <> Phase_Study And ddlStatus.SelectedItem.Text.Trim.ToUpper() <> Phase_Analysis Then

                Wstr = "vWorkSpaceId='" & ViewState("workspaceId") & "'"
                If Not objHelp.getworkspacemst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_WorkSpacemst, estr) Then
                    Me.objCommon.ShowAlert("Error While Getting Data From WorkSpaceMst", Me.Page)
                End If

                For Each Dr In ds_WorkSpacemst.Tables(0).Rows
                    ds_WorkSpacemst.Tables(0).Rows(0)("cProjectstatus") = Me.ddlStatus.SelectedValue.Trim()
                Next
                ds_WorkSpacemst.Tables(0).AcceptChanges()

                '**********Updating Project Phase Status**************
                If Not Me.UpdateProjectStatus(dsWorkSpaceStatusDtl) Then
                    Me.objCommon.ShowAlert("Problem While Updating Project Status.", Me.Page)
                    Exit Sub
                End If
                '******************************************************

                ds_WorkSpacemst.Tables.Add(dsWorkSpaceStatusDtl.Tables(0).Copy())
                ds_WorkSpacemst.Tables(1).TableName = "WorkSpaceStatusDtl"
                ds_WorkSpacemst.AcceptChanges()

                If Not objLambda.Save_workspacemst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_WorkSpacemst, Me.Session(S_UserID), "", estr) Then
                    Me.objCommon.ShowAlert("Error While Updating Data", Me.Page)
                    Exit Sub
                End If
                ds_WorkSpacemst.Tables(0).Rows.Clear()

            Else

                CreateDataTable()
                AssignValues()
                dsWorkspaceMst.Tables.Clear()
                dtWorkspaceMst = ViewState("dtWorkspaceMst")

                dsWorkspaceMst = New DataSet
                dsWorkspaceMst.Tables.Add(dtWorkspaceMst)

                objLambda.Timeout = 180000

                If Not objLambda.Save_ProcChangeProjectStatus(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                                dsWorkspaceMst, Session(S_UserID), eStr_Retu) Then
                    objCommon.ShowAlert(eStr_Retu.Replace("'", "\'") + " : " + "Error While changing project status", Me)
                    Throw New Exception(eStr_Retu)
                    'CType(Me.Master.FindControl("Scriptmanager1"), ScriptManager).RegisterClientScriptBlock(Me, Me.GetType, "ShowHideDiv", "ShowHideDiv('N');", True)
                End If
            End If


            objCommon.ShowAlert("Project Status Changed Successfully", Me)

            'For filling updated grid
            If Convert.ToString(Me.HProjectId.Value).Trim() <> "" Then
                Me.btnSetProject_Click(sender, e)
            Else
                If Not Me.Gencall_Data() Then
                    Throw New Exception()
                End If
                Me.BindGrid()
            End If
            '****************************
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gvwProjectsUI", "gvwProjectsUI(); ", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......btnSave_Click")
        End Try
    End Sub

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim ds_SearchProjects As New DataSet
        Dim Wstr As String = String.Empty
        Try


            Me.ClearControls()

            Wstr = "iUserId=" + Me.Session(S_UserID) + " and vworkspaceid = '" + Me.HProjectId.Value.Trim + "'"

            If Not objHelp.View_MyProjects(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                ds_SearchProjects, eStr_Retu) Then
                Throw New Exception("Error displaying projects:" & eStr_Retu.ToString())
            End If

            ViewState(VS_MyProject) = ds_SearchProjects

            gvwProjects.DataSource = ds_SearchProjects
            gvwProjects.DataBind()
            ''Added By Vivek Patel
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gvwProjectsUI", "gvwProjectsUI(); ", True)
            ''Completyed by Vivek Patel
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ThemeSelection();", "ThemeSelection();", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "GetData", "getData(this);", True)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......btnSetProject_Click")
        End Try
    End Sub

    Protected Sub btnAllProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAllProject.Click
        Try

            Me.txtproject.Text = ""
            Me.HProjectId.Value = ""

            Me.ViewState(VS_PagerStartPage) = "1"
            Me.ViewState(VS_CurrentPage) = "1"

            If Not GetRecordsCounts() Then
                Throw New Exception()
            End If

            If Not Me.Gencall_Data() Then
                Throw New Exception()
            End If

            Me.BindGrid()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "........btnAllProject_Click")
        End Try

    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gvwProjectsUI", "gvwProjectsUI(); ", True)
    End Sub
    '=================
#End Region

#Region "AssignValues "

    Private Sub AssignValues()
        Dim dtWorkspaceMst As DataTable = ViewState("dtWorkspaceMst")
        Dim dr As DataRow
        Try

            dr = dtWorkspaceMst.NewRow
            dr("vWorkSpaceId") = ViewState("workspaceId")

            dr("vTemplateId") = "0"
            If ddlTemplate.Enabled Then
                dr("vTemplateId") = ddlTemplate.SelectedValue.Trim.ToString
            End If

            dr("cProjectStatus") = ddlStatus.SelectedValue.Trim.ToString
            dr("NoOfPeriod") = Me.txtPeriod.Text.Trim()
            dr("iModifyBy") = Session(S_UserID)
            dr("vRemarks") = Me.txtReason.Text.Trim()
            dtWorkspaceMst.Rows.Add(dr)

            ViewState("dtWorkspacemst") = dtWorkspaceMst

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......AssignValues")
        End Try
    End Sub

    Private Sub CreateDataTable()
        Dim dt As New DataTable
        dt.TableName = "WorkspaceMst"
        Dim dc As New DataColumn
        dc.ColumnName = "vTemplateId"
        dc.DataType = GetType(String)
        dt.Columns.Add(dc)

        dt.Columns.Add(New DataColumn("vWorkspaceId", GetType(String)))
        dt.Columns.Add(New DataColumn("cProjectStatus", GetType(String)))
        dt.Columns.Add(New DataColumn("NoOfPeriod", GetType(String)))
        dt.Columns.Add(New DataColumn("iModifyBy", GetType(Integer)))
        dt.Columns.Add(New DataColumn("vRemarks", GetType(String)))

        ViewState("dtWorkspaceMst") = dt
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

    ''Added By Vivek Patel
    Protected Sub gvwProjects_PreRender(sender As Object, e As EventArgs) Handles gvwProjects.PreRender
        If gvwProjects.Rows.Count > 0 Or Not gvwProjects.DataSource Is Nothing Then
            gvwProjects.UseAccessibleHeader = True
            gvwProjects.HeaderRow.TableSection = TableRowSection.TableHeader
            gvwProjects.FooterRow.TableSection = TableRowSection.TableFooter
            gvwProjects.Width = 80%
        End If
    End Sub
    ''Ended by Vivek Patel
End Class