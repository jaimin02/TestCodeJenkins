Imports System.Drawing

Partial Class frmCRFActivityStatusReport
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "
    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Dim grdChild As GridView
    Private DEP As Integer = 0
    Private DEC As Integer = 0
    Private FRP As Integer = 0
    Private SRP As Integer = 0
    Private FnlRP As Integer = 0
    Private Locked As Integer = 0
    Private DCF As Integer = 0
    Private SubjectNo As Integer = 0
    Private dtChild As New DataTable
    Private Const Sub_Specific_Activity As Integer = 1
    Private Const Generic_Activity As Integer = 2
    Private Const Data_Entry_Pending As Integer = 1
    Private Const Data_Entry_Continue As Integer = 2
    Private Const First_Review_Pending As Integer = 3
    Private Const Second_Review_Pending As Integer = 4
    Private Const Final_Review_Pending As Integer = 5
    Private Const Reviewed_Locked As Integer = 6
    Private Const GVC_SubjectNo As Integer = 0
    Private Const GVC_ActivityStatus As Integer = 1
    Private Const GVC_DEP As Integer = 2
    Private Const GVC_DEC As Integer = 3
    Private Const GVC_FRP As Integer = 4
    Private Const GVC_SRP As Integer = 5
    Private Const GVC_FnlRP As Integer = 6
    Private Const GVC_Locked As Integer = 7
    Private Const GVC_DCF_Pending As Integer = 8
    Private Const VS_dsMaster As String = "Activity Status Report"
    Private Const Vs_dsReviewerlevel As String = "dsActivitystatusReviewerlevel"
    Dim export As Boolean = False
    Private rPage As RepoPage
#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not GenCall() Then
                Exit Sub
            End If
        End If
        Me.Session(S_SelectedRepeatation) = ""
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Don't remove this function
    End Sub

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            If Not GenCall_ShowUI() Then
                Throw New Exception()
            End If
            Me.hType.Value = "BA-BE"
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                Me.hType.Value = ""
            End If

            GenCall = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".............GenCall")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_ShowUI"

    Private Function GenCall_ShowUI() As Boolean
        Dim eStr As String = ""
        Dim ds_Workspace As New DataSet
        Dim sender As New Object
        Dim e As New EventArgs
        Try
            Page.Title = " :: CRF Activity Status Report ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")

            CType(Master.FindControl("lblHeading"), Label).Text = "CRF Activity Status Report"

            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
            End If

            If Not Request.QueryString("WorkSpaceId") Is Nothing AndAlso Not Request.QueryString("ProjectName") Is Nothing Then
                Me.txtproject.Text = Request.QueryString("ProjectName")
                Me.HProjectId.Value = Request.QueryString("WorkSpaceId")
                btnSetProject_Click(sender, e)
            End If

            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
            Me.imgActivityLegends.Attributes.Add("onmouseover", "$('#" + Me.divActivityLegends.ClientID + "').toggle('fast');")
            Me.divActivityLegends.Attributes.Add("onmouseleave", "$('#" + Me.divActivityLegends.ClientID + "').toggle('fast');")
            Me.tdtypeLabel.Visible = True
            Me.tdtypeddl.Visible = True
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                Me.tdtypeLabel.Visible = True
                Me.tdtypeddl.Visible = True
            End If
            Dim StrSubejct As String

            If (Request.QueryString("ProjectName") <> "") Then
                Me.HProjectId.Value = Request.QueryString("WorkSpaceId").ToString()
                If Not FillDropDownListPeriods(eStr) Then
                    ShowErrorMessage(eStr, "")
                End If
                ddlPeriods.SelectedValue = Request.QueryString("Period").ToString()
                If (Request.QueryString("SubjectId").ToString() = "") Then
                    ddlType.SelectedValue = Sub_Specific_Activity
                    StrSubejct = ""
                    ddlType_SelectedIndexChanged(Nothing, Nothing)
                Else
                    ddlType.SelectedValue = Sub_Specific_Activity
                    StrSubejct = "," + Request.QueryString("SubjectId") + ","
                    ddlType_SelectedIndexChanged(Nothing, Nothing)
                End If
                If Not BindMasterGrid(StrSubejct.ToString(), String.Empty, String.Empty, eStr) Then
                    ShowErrorMessage(eStr, "")
                End If
                txtproject.Text = Request.QueryString("ProjectName")
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Ref", "ChangeUrl('CrfActivity','frmCRFActivityStatusReport.aspx');", True)
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "collapsuncollaps", "displayCRFInfo(ctl00_CPHLAMBDA_imgfldgen,'tblEntryData');", True)
            End If

            GenCall_ShowUI = True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "............GenCall_ShowUI")
            Return False
        Finally
            ds_Workspace.Dispose()
        End Try

    End Function

#End Region

#Region "FillDropDownList Periods"

    Private Function FillDropDownListPeriods(ByRef eStr As String) As Boolean
        Dim ds_Periods As New DataSet
        Dim wStr As String = String.Empty
        Dim Periods As Integer = 1

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            Me.ddlPeriods.Items.Clear()

            wStr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "' and cStatusIndi<>'D'"

            If Not objHelp.GetWorkspaceProtocolDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        ds_Periods, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_Periods Is Nothing Then
                Throw New Exception("Problem while getting data")
            End If

            If Not ds_Periods.Tables(0).Rows(0)("iNoOfPeriods") Is Nothing AndAlso Convert.ToString(ds_Periods.Tables(0).Rows(0)("iNoOfPeriods")).Trim() <> "" Then

                Periods = ds_Periods.Tables(0).Rows(0)("iNoOfPeriods")
                For count As Integer = 0 To Periods - 1
                    Me.ddlPeriods.Items.Add((count + 1).ToString)
                Next count

            End If
            Me.ddlPeriods.Items.Insert(0, "All")
            Return True

        Catch ex As Exception
            ShowErrorMessage("Problem While Filling Periods. ", ex.Message)
            eStr = ex.Message
            Return False
        Finally
            ds_Periods.Dispose()
        End Try

    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim eStr As String = String.Empty
        Try
            Me.tdHRUpper.Style.Add("display", "none")
            Me.tdHRLower.Style.Add("display", "none")
            Me.divSubject.Style.Add("display", "none")
            Me.divActivity.Style.Add("display", "none")
            Me.tvActivity.Nodes.Clear()
            Me.tvSubject.Nodes.Clear()
            Me.tvSubject.Nodes.Clear()
            Me.ddlType.SelectedIndex = 0
            Me.tvActivity.Nodes.Clear()
            Me.grdParent.DataSource = Nothing
            Me.grdParent.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ChangeColor", "ChangeColor();", True)
            Me.fldgrdParent.Style.Add("display", "none")

            If Not FillDropDownListPeriods(eStr) Then
                Throw New Exception(eStr)
            End If
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                Me.ddlType.SelectedValue = 1
                ddlType_SelectedIndexChanged(sender, e)
            End If
            If Not GetLegends() Then
                Exit Sub
            End If

            If Not FillReviewerfilter() Then
                Exit Sub
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".............btnSetProject_Click")
        End Try
    End Sub

    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
        Dim iPeriod As String = String.Empty
        Dim ds As DataSet = New DataSet
        Dim eStr As String = ""
        Dim strSubjectId As String = String.Empty
        Dim strParentID As String = String.Empty
        Dim strActivityId As String = String.Empty
        Dim cSubjectWiseFlag As String = String.Empty
        Dim chCnt As Integer = 0
        Try
            iPeriod = "," + Me.ddlPeriods.SelectedValue.ToString() + ","
            If Me.ddlPeriods.SelectedValue = "All" Then
                iPeriod = ""
            End If
            Me.ddlFilter.Items.FindByValue("0").Attributes.Add("style", "font-weight:bold")
            Me.ddlFilter.Items.FindByValue("1").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Red))
            Me.ddlFilter.Items.FindByValue("2").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Orange))
            Me.ddlFilter.Items.FindByValue("3").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Blue))

            If ddlFilter.Items.Count = 7 Then
                Me.ddlFilter.Items.FindByValue("4").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Purple))
                Me.ddlFilter.Items.FindByValue("5").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Green))
                Me.ddlFilter.Items.FindByValue("6").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Gray))
            ElseIf ddlFilter.Items.Count = 6 Then
                Me.ddlFilter.Items.FindByValue("4").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Purple))
                Me.ddlFilter.Items.FindByValue("5").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Green))
            ElseIf ddlFilter.Items.Count = 5 Then
                Me.ddlFilter.Items.FindByValue("4").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Purple))
            End If

            If Not filllegends() Then
                Exit Sub
            End If

            If Me.ddlType.SelectedValue = Sub_Specific_Activity Then
                If tvSubject.Nodes(0).Checked = False Then
                    For index = 0 To tvSubject.Nodes(0).ChildNodes.Count - 1
                        If tvSubject.Nodes(0).ChildNodes(index).Checked = True Then
                            strSubjectId = strSubjectId + tvSubject.Nodes(0).ChildNodes(index).Value + ","
                        End If
                    Next 'Next Index
                    If strSubjectId <> "" Then
                        strSubjectId.Remove(strSubjectId.Length - 1)
                        strSubjectId = "'," + strSubjectId + "'"
                    End If
                End If
            End If
            If tvActivity.Nodes(0).Checked = False Then
                For index = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                    If tvActivity.Nodes(0).ChildNodes(index).Checked = True Then
                        strParentID = strParentID + tvActivity.Nodes(0).ChildNodes(index).Value + ","
                    End If
                Next
            End If
            If strParentID <> "" Then
                strParentID.Remove(strParentID.Length - 1)
                strParentID = "'," + strParentID + "'"
            End If
            If tvActivity.Nodes(0).Checked = False Then
                For iParent = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                    For iChild = 0 To tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes.Count - 1
                        If tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Checked = True Then
                            strActivityId = strActivityId + tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Value + ","
                        End If
                    Next 'Next iChild
                Next 'Next iParent
            End If

            If strActivityId <> "" Then
                strActivityId.Remove(strActivityId.Length - 1)
                strActivityId = "'," + strActivityId + "'"
            End If

            If Not BindMasterGrid(strSubjectId, strParentID, strActivityId, eStr) Then
                Throw New Exception(eStr)
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideEntryData", "displayCRFInfo(" + Me.imgfldgen.ClientID + ",'tblEntryData');", True)
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "..............btnGo_Click")
        Finally
            ds.Dispose()
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("frmCRFActivityStatusReport.aspx")
    End Sub

    Public Function GenCall_ShowUI1() As Boolean
        Dim iPeriod As String = String.Empty
        Dim ds As DataSet = New DataSet
        Dim eStr As String = ""
        Dim strSubjectId As String = String.Empty
        Dim strParentID As String = String.Empty
        Dim strActivityId As String = String.Empty
        Dim cSubjectWiseFlag As String = String.Empty
        Dim chCnt As Integer = 0

        Try
            If Me.ddlType.SelectedValue = Sub_Specific_Activity Then
                If tvSubject.Nodes(0).Checked = False Then
                    For index = 0 To tvSubject.Nodes(0).ChildNodes.Count - 1
                        If tvSubject.Nodes(0).ChildNodes(index).Checked = True Then
                            strSubjectId = strSubjectId + tvSubject.Nodes(0).ChildNodes(index).Value + ","
                        End If
                    Next 'Next Index
                    If strSubjectId <> "" Then
                        strSubjectId.Remove(strSubjectId.Length - 1)
                        strSubjectId = "'," + strSubjectId + "'"
                    End If
                End If
            End If
            If tvActivity.Nodes(0).Checked = False Then
                For index = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                    If tvActivity.Nodes(0).ChildNodes(index).Checked = True Then
                        strParentID = strParentID + tvActivity.Nodes(0).ChildNodes(index).Value + ","
                    End If
                Next
            End If
            If strParentID <> "" Then
                strParentID.Remove(strParentID.Length - 1)
                strParentID = "'," + strParentID + "'"
            End If
            If tvActivity.Nodes(0).Checked = False Then
                For iParent = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                    For iChild = 0 To tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes.Count - 1
                        If tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Checked = True Then
                            strActivityId = strActivityId + tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Value + ","
                        End If
                    Next 'Next iChild
                Next 'Next iParent
            End If

            If strActivityId <> "" Then
                strActivityId.Remove(strActivityId.Length - 1)
                strActivityId = "'," + strActivityId + "'"
            End If

            If Not BindMasterGrid(strSubjectId, strParentID, strActivityId, eStr) Then
                Throw New Exception(eStr)
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideEntryData", "displayCRFInfo(" + Me.imgfldgen.ClientID + ",'tblEntryData');", True)

        Catch ex As Exception

        End Try
    End Function

#End Region

#Region "Grid Events"

    Protected Sub grdParent_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdParent.RowCommand

        Dim mstrSubjectId As String = String.Empty
        Dim ds As DataSet = New DataSet
        Dim eStr As String = ""
        Dim strParentID As String = String.Empty
        Dim strActivityId As String = String.Empty
        Dim cSubjectWiseFlag As String = String.Empty
        Dim cDataStatus As String = String.Empty
        Dim iWorkflowStageId As String = String.Empty
        Dim strSubjectId As String = String.Empty
        Dim chCnt As Integer = 0
        Dim dv As New DataView
        Dim iPeriod As String = String.Empty
        Dim index As Integer = e.CommandArgument
        Dim ds_reviewer As New DataSet
        Dim dv_reviewer As DataView
        Try

            If Not filllegends() Then
                Exit Sub
            End If

            If e.CommandName.ToUpper = "SHOW" Then

                strSubjectId = "," + CType(Me.grdParent.Rows(index).FindControl("hvSubjectId"), HiddenField).Value + ","

                iPeriod = "," + Me.ddlPeriods.SelectedValue.ToString() + ","
                If Me.ddlPeriods.SelectedValue = "All" Then
                    iPeriod = ""
                End If

                If tvActivity.Nodes(0).Checked = False Then
                    For index = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                        If tvActivity.Nodes(0).ChildNodes(index).Checked = True Then
                            strParentID = strParentID + tvActivity.Nodes(0).ChildNodes(index).Value + ","
                        End If
                    Next
                End If

                If tvActivity.Nodes(0).Checked = False Then
                    For iParent = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                        For iChild = 0 To tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes.Count - 1
                            If tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Checked = True Then
                                strActivityId = strActivityId + tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Value + ","
                                If tvActivity.Nodes(0).ChildNodes(iParent).Checked Then
                                    strActivityId = strActivityId + tvActivity.Nodes(0).ChildNodes(iParent).Value + ","
                                End If
                            End If
                        Next 'Next iChild
                    Next 'Next iParent
                End If
                If strParentID <> "" Then
                    strParentID.Remove(strParentID.Length - 1)
                    strParentID = "'," + strParentID + "'"
                End If
                If strActivityId <> "" Then
                    strActivityId.Remove(strActivityId.Length - 1)
                    strActivityId = "'," + strActivityId + "'"
                End If

                cSubjectWiseFlag = "Y"
                If Me.ddlType.SelectedValue = Generic_Activity Then
                    cSubjectWiseFlag = "N"
                    strSubjectId = ""
                End If

                If Me.ddlFilter.SelectedValue = Data_Entry_Pending Then
                    cDataStatus = ",0,"
                    iWorkflowStageId = ",0,"
                ElseIf ddlFilter.SelectedValue = Data_Entry_Continue Then
                    cDataStatus = ",B,"
                    iWorkflowStageId = ",0,"
                ElseIf ddlFilter.SelectedValue = First_Review_Pending Then
                    cDataStatus = ",D,"
                    iWorkflowStageId = ",0,"
                Else
                    ds_reviewer = CType(Me.Session(Vs_dsReviewerlevel), DataSet)
                    dv_reviewer = ds_reviewer.Tables(0).Copy.DefaultView
                    dv_reviewer.RowFilter = "Reviewer ='" + ddlFilter.SelectedItem.Text.Trim() + "'"

                    If dv_reviewer.ToTable.Rows.Count > 0 Then
                        If dv_reviewer.ToTable.Rows(0)("vStatus") = "L" Then
                            cDataStatus = ",F,"
                        Else
                            cDataStatus = ",E,"
                        End If
                        iWorkflowStageId = "," + Convert.ToString(dv_reviewer.ToTable.Rows(0)("iReviewWorkflowStageId")) + ","
                    End If

                End If

                index = e.CommandArgument
                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                    If Not objHelp.proc_GetCRFActivityStatusCTM(Me.HProjectId.Value, iPeriod, strSubjectId, strParentID, strActivityId, cSubjectWiseFlag, cDataStatus, iWorkflowStageId, ds, eStr) Then
                        Throw New Exception(eStr)
                    End If
                Else
                    If Not objHelp.proc_GetCRFActivityStatusBABE(Me.HProjectId.Value, iPeriod, strSubjectId, strParentID, strActivityId, cSubjectWiseFlag, cDataStatus, iWorkflowStageId, ds, eStr) Then
                        Throw New Exception(eStr)
                    End If
                End If

                CType(Me.grdParent.Rows(index).FindControl("grdChild"), GridView).DataSource = ds.Tables(0)
                CType(Me.grdParent.Rows(index).FindControl("grdChild"), GridView).DataBind()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ChangeColor", "ChangeColor();", True)
                CType(Me.grdParent.Rows(index).FindControl("grdChild"), GridView).Focus()
                CType(Me.grdParent.Rows(index).FindControl("btnShow"), ImageButton).Visible = False
                CType(Me.grdParent.Rows(index).FindControl("btnHide"), ImageButton).Visible = True
                CType(Me.grdParent.Rows(index).FindControl("lblActivity"), Label).Text = "Hide all Details"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideEntryData", "displayCRFInfo(" + Me.imgfldgen.ClientID + ",'tblEntryData');", True)
            End If
            If e.CommandName.ToUpper = "HIDE" Then

                CType(Me.grdParent.Rows(index).FindControl("grdChild"), GridView).DataSource = Nothing
                CType(Me.grdParent.Rows(index).FindControl("grdChild"), GridView).DataBind()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ChangeColor", "ChangeColor();", True)
                CType(Me.grdParent.Rows(index).FindControl("btnShow"), ImageButton).Visible = True
                CType(Me.grdParent.Rows(index).FindControl("btnHide"), ImageButton).Visible = False
                CType(Me.grdParent.Rows(index).FindControl("lblActivity"), Label).Text = "Click to See Details"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideEntryData", "displayCRFInfo(" + Me.imgfldgen.ClientID + ",'tblEntryData');", True)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......chkSelect_CheckedChanged")
        Finally
            ds.Dispose()
            dv.Dispose()
        End Try
    End Sub

    Protected Sub grdParent_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdParent.RowDataBound
        Dim ds As New DataSet

        Try
            ds = CType(Me.Session(Vs_dsReviewerlevel), DataSet).Copy()

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                If e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.Footer Then
                    If ds.Tables(0).Rows(i)("vStatus") = "L" Then
                        e.Row.Cells(GVC_Locked).BackColor = ColorTranslator.FromHtml(ds.Tables(0).Rows(i)("vColorCodeForDynamic"))
                    ElseIf ds.Tables(0).Rows(i)("vStatus") = "FNLRP" Then
                        e.Row.Cells(GVC_FnlRP).BackColor = ColorTranslator.FromHtml(ds.Tables(0).Rows(i)("vColorCodeForDynamic"))
                    ElseIf ds.Tables(0).Rows(i)("vStatus") = "SRP" Then
                        e.Row.Cells(GVC_SRP).BackColor = ColorTranslator.FromHtml(ds.Tables(0).Rows(i)("vColorCodeForDynamic"))
                    End If
                ElseIf e.Row.RowType = DataControlRowType.DataRow Then
                    If ds.Tables(0).Rows(i)("vStatus") = "L" Then
                        e.Row.Cells(GVC_Locked).ForeColor = ColorTranslator.FromHtml(ds.Tables(0).Rows(i)("vColorCodeForDynamic"))
                    ElseIf ds.Tables(0).Rows(i)("vStatus") = "FNLRP" Then
                        e.Row.Cells(GVC_FnlRP).ForeColor = ColorTranslator.FromHtml(ds.Tables(0).Rows(i)("vColorCodeForDynamic"))
                    ElseIf ds.Tables(0).Rows(i)("vStatus") = "SRP" Then
                        e.Row.Cells(GVC_SRP).ForeColor = ColorTranslator.FromHtml(ds.Tables(0).Rows(i)("vColorCodeForDynamic"))
                    End If
                End If
            Next

            Dim dv As New DataView
            If e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(GVC_ActivityStatus).Text = "Activity Detail"
                e.Row.Cells(GVC_DEP).ToolTip = "Data Entry Pending"
                e.Row.Cells(GVC_DEC).ToolTip = "Data Entry Continue"
                e.Row.Cells(GVC_FRP).ToolTip = "Ready For Review"
                e.Row.Cells(GVC_SRP).ToolTip = "First Review Done"
                e.Row.Cells(GVC_FnlRP).ToolTip = "Second Review Done"
                e.Row.Cells(GVC_Locked).ToolTip = "Final Reviewed & Freeze"
                e.Row.Cells(GVC_DCF_Pending).ToolTip = "DCF Pending"

            ElseIf e.Row.RowType = DataControlRowType.DataRow Then

                dv = New DataView(dtChild)
                SubjectNo = SubjectNo + 1
                DEP = DEP + CType(e.Row.Cells(GVC_DEP).Text, Integer)
                DEC = DEC + CType(e.Row.Cells(GVC_DEC).Text, Integer)
                FRP = FRP + CType(e.Row.Cells(GVC_FRP).Text, Integer)
                SRP = SRP + CType(e.Row.Cells(GVC_SRP).Text, Integer)
                FnlRP = FnlRP + CType(e.Row.Cells(GVC_FnlRP).Text, Integer)
                Locked = Locked + CType(e.Row.Cells(GVC_Locked).Text, Integer)
                DCF = DCF + CType(e.Row.Cells(GVC_DCF_Pending).Text, Integer)

                CType(e.Row.FindControl("btnShow"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("btnShow"), ImageButton).CommandName = "SHOW"
                CType(e.Row.FindControl("lblActivity"), Label).Text = "Click to See Details"

                CType(e.Row.FindControl("btnHide"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("btnHide"), ImageButton).CommandName = "HIDE"
                CType(e.Row.FindControl("btnHide"), ImageButton).Visible = False

                e.Row.Cells(GVC_DEP).ToolTip = "Data Entry Pending"
                e.Row.Cells(GVC_DEC).ToolTip = "Data Entry Continue"
                e.Row.Cells(GVC_FRP).ToolTip = "Ready For Review"
                e.Row.Cells(GVC_SRP).ToolTip = "First Review Done"
                e.Row.Cells(GVC_FnlRP).ToolTip = "Second Review Done"
                e.Row.Cells(GVC_Locked).ToolTip = "Final Reviewed & Freeze"
                e.Row.Cells(GVC_DCF_Pending).ToolTip = "DCF Pending"
            Else
                e.Row.Cells(GVC_SubjectNo).Text = "No. of Subjects: " + SubjectNo.ToString()
                e.Row.Cells(GVC_SubjectNo).CssClass = "GridFooterColor"
                e.Row.Cells(GVC_DEP).Text = DEP.ToString()
                e.Row.Cells(GVC_DEP).CssClass = "GridFooterColor"
                e.Row.Cells(GVC_DEC).Text = DEC.ToString()
                e.Row.Cells(GVC_DEC).CssClass = "GridFooterColor"
                e.Row.Cells(GVC_FRP).Text = FRP.ToString()
                e.Row.Cells(GVC_FRP).CssClass = "GridFooterColor"
                e.Row.Cells(GVC_SRP).Text = SRP.ToString()
                e.Row.Cells(GVC_SRP).CssClass = "GridFooterColor"
                e.Row.Cells(GVC_FnlRP).Text = FnlRP.ToString()
                e.Row.Cells(GVC_FnlRP).CssClass = "GridFooterColor"
                e.Row.Cells(GVC_Locked).Text = Locked.ToString()
                e.Row.Cells(GVC_Locked).CssClass = "GridFooterColor"
                e.Row.Cells(GVC_DCF_Pending).Text = DCF.ToString()
                e.Row.Cells(GVC_DCF_Pending).CssClass = "GridFooterColor"
                e.Row.Cells(GVC_ActivityStatus).Text = "Total Activity"
                e.Row.Cells(GVC_ActivityStatus).CssClass = "GridFooterColor"

                Me.hdnSubjectNo.Value = SubjectNo.ToString()
                Me.hdnDEP.Value = DEP.ToString()
                Me.hdnDEC.Value = DEC.ToString()
                Me.hdnFRP.Value = FRP.ToString()
                Me.hdnSRP.Value = SRP.ToString()
                Me.hdnFnlRP.Value = FnlRP.ToString()
                Me.hdnLocked.Value = Locked.ToString()
                Me.hdnDCF.Value = DCF.ToString()
                dtChild = Nothing
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".............grdParent_RowDataBound")
        End Try
    End Sub

    Protected Sub grdChild_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Dim ds As New DataSet
        Dim dv As DataView
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then

                ds = CType(Me.Session(Vs_dsReviewerlevel), DataSet).Copy()
                dv = ds.Tables(0).Copy().DefaultView
                dv.RowFilter = "vStatus = '" + CType(e.Row.FindControl("lblStatus"), Label).Text + "'"

                If CType(e.Row.FindControl("lblStatus"), Label).Text = "DEP" Or CType(e.Row.FindControl("lblStatus"), Label).Text = "" Then
                    CType(e.Row.FindControl("lnkActivity"), LinkButton).ForeColor = Drawing.Color.Red
                    CType(e.Row.FindControl("lnkActivity"), LinkButton).ToolTip = "Data Entry Pending"

                ElseIf CType(e.Row.FindControl("lblStatus"), Label).Text = "DEC" Then
                    CType(e.Row.FindControl("lnkActivity"), LinkButton).ForeColor = Drawing.Color.Orange
                    CType(e.Row.FindControl("lnkActivity"), LinkButton).ToolTip = "Data Entry Continue"

                ElseIf CType(e.Row.FindControl("lblStatus"), Label).Text = "FRP" Then
                    CType(e.Row.FindControl("lnkActivity"), LinkButton).ForeColor = Drawing.Color.Blue
                    CType(e.Row.FindControl("lnkActivity"), LinkButton).ToolTip = "First Review Pending"
                ElseIf CType(e.Row.FindControl("lblStatus"), Label).Text = "SRP" Then
                    If dv.ToTable.Rows.Count > 0 Then
                        CType(e.Row.FindControl("lnkActivity"), LinkButton).ForeColor = ColorTranslator.FromHtml(dv.ToTable.Rows(0)("vColorCodeForDynamic"))
                    End If
                    CType(e.Row.FindControl("lnkActivity"), LinkButton).ToolTip = "Second Review Pending"

                ElseIf CType(e.Row.FindControl("lblStatus"), Label).Text = "FNLRP" Then
                    If dv.ToTable.Rows.Count > 0 Then
                        CType(e.Row.FindControl("lnkActivity"), LinkButton).ForeColor = ColorTranslator.FromHtml(dv.ToTable.Rows(0)("vColorCodeForDynamic"))
                    End If
                    CType(e.Row.FindControl("lnkActivity"), LinkButton).ToolTip = "Final Review Pending"

                ElseIf CType(e.Row.FindControl("lblStatus"), Label).Text = "L" Then
                    If dv.ToTable.Rows.Count > 0 Then
                        CType(e.Row.FindControl("lnkActivity"), LinkButton).ForeColor = ColorTranslator.FromHtml(dv.ToTable.Rows(0)("vColorCodeForDynamic"))
                    End If
                    CType(e.Row.FindControl("lnkActivity"), LinkButton).ToolTip = "Reviewed & Freeze"
                End If
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "..........grdChild_RowDataBound")
        End Try
    End Sub

#End Region

#Region "Extra Functions"

    Protected Function BindSubjectTree(ByRef eStr As String) As Boolean
        Dim strqry As String = String.Empty
        Dim whrCon As String = String.Empty
        Dim dsSubject As DataSet = New DataSet
        Dim dtSubject As New DataTable
        Dim period As String = "1"

        If Me.ddlPeriods.SelectedValue <> "All" Then   'Added on 2-Feb-2012 to avoid subject repetaion in subject tree view by Mrunal Parekh
            period = Me.ddlPeriods.SelectedValue
        End If

        Try
            whrCon = " vWorkspaceId='" + Me.HProjectId.Value + "'" _
                    + " and  iPeriod=" + CInt(period).ToString + "  AND cStatusIndi <> 'D' Order by iMySubjectNo"

            Me.tdHRUpper.Style.Add("display", "none")
            Me.tdHRLower.Style.Add("display", "none")
            Me.divSubject.Style.Add("display", "none")
            Me.tvSubject.Style.Add("Height", "0px")
            If Me.ddlType.SelectedValue = Sub_Specific_Activity Then
                If Not objHelp.GetWorkspaceSubjectMst(whrCon, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsSubject, eStr) Then
                    Throw New Exception(eStr)
                End If
                If Not dsSubject Is Nothing Then
                    If dsSubject.Tables(0).Rows.Count > 0 Then
                        dtSubject = dsSubject.Tables(0)
                        Dim nodeAll As New TreeNode()
                        nodeAll.Text = "All Subject\ScreenNo*"
                        nodeAll.Value = "All Subject\ScreenNo"

                        If (Request.QueryString("ProjectName") <> "" And Request.QueryString("SubjectId") = "") Then
                            nodeAll.Checked = True
                        End If
                        Me.tvSubject.Nodes.Add(nodeAll)
                        For index = 0 To dtSubject.Rows.Count - 1
                            Dim nodeSubject As New TreeNode()
                            nodeSubject.Text = dtSubject.Rows(index).Item("vMySubjectNo").ToString()
                            If dtSubject.Rows(index).Item("cRejectionFlag").ToString() = "Y" Then
                                nodeSubject.Text = "<font color = red>" + dtSubject.Rows(index).Item("vMySubjectNo").ToString() + "</font>"
                            End If
                            nodeSubject.ToolTip = dtSubject.Rows(index).Item("vMySubjectNo").ToString() + "|" + dtSubject.Rows(index).Item("vSubjectId").ToString()
                            nodeSubject.Value = dtSubject.Rows(index).Item("vSubjectId").ToString()
                            nodeSubject.SelectAction = TreeNodeSelectAction.None
                            If (Request.QueryString("ProjectName") <> "" And Request.QueryString("SubjectId") <> "" And Request.QueryString("SubjectId") = dtSubject.Rows(index).Item("vSubjectId").ToString()) Then
                                nodeSubject.Checked = True
                            ElseIf (Request.QueryString("ProjectName") <> "" And Request.QueryString("SubjectId") = "") Then
                                nodeSubject.Checked = True
                            End If
                            nodeSubject.ChildNodes.Add(nodeSubject)

                            Me.tvSubject.Nodes(0).ChildNodes.Add(nodeSubject)
                        Next ' Next Index
                        Me.tvSubject.Nodes(0).ExpandAll()
                        Me.tvSubject.Nodes(0).SelectAction = TreeNodeSelectAction.None

                        Me.tdHRUpper.Style.Add("display", "")
                        Me.tdHRLower.Style.Add("display", "")
                        Me.divSubject.Style.Add("display", "block")
                        Me.tvSubject.Style.Add("Height", "100px")
                    Else
                        objcommon.ShowAlert("No Subject Found For This Project!", Me.Page)
                    End If
                End If

            End If
            Return True
        Catch ex As Exception
            eStr = ex.Message
            Return False
        Finally
            dsSubject.Dispose()
            dtSubject.Dispose()
        End Try
    End Function

    Protected Function BindActivityTree(ByRef eStr As String) As Boolean
        Dim strqry As String = String.Empty
        Dim iPeriod As String = String.Empty
        Dim whrCon As String = String.Empty
        Dim dtActivity As New DataTable
        Dim dsActivity As DataSet = New DataSet
        Dim dvActivity As DataView
        Dim dvChild As DataView
        Dim Subject_Specific As String = String.Empty
        Dim ActNodeAll As New TreeNode()
        Try
            iPeriod = "," + Me.ddlPeriods.SelectedValue.ToString() + ","
            If Me.ddlPeriods.SelectedValue = "All" Then
                iPeriod = ""
            End If
            Subject_Specific = "Y"
            If Me.ddlType.SelectedValue = Generic_Activity Then
                Subject_Specific = "N"
            End If

            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Or Me.Session(S_ScopeNo) = "6" Then ''For CT and ALL scope
                If Not objHelp.Proc_ActivityTreeCTM(Me.HProjectId.Value, iPeriod, Subject_Specific, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, dsActivity, eStr) Then
                    Throw New Exception(eStr)
                End If
            Else
                If Not objHelp.Proc_ActivityTreeBABE(Me.HProjectId.Value, iPeriod, Subject_Specific, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, dsActivity, eStr) Then
                    Throw New Exception(eStr)
                End If
            End If
            dtActivity = dsActivity.Tables(0)
            dvActivity = New DataView(dtActivity)
            dvActivity.RowFilter = "TreeLevel=0"
            ActNodeAll.Text = "All Activity*"
            ActNodeAll.Value = "All Activity"
            Me.tvActivity.Nodes.Add(ActNodeAll)
            If (Request.QueryString("ProjectName") <> "") Then
                ActNodeAll.Checked = True
            End If
            For ParentNode = 0 To dvActivity.Count - 1
                Dim nodeActivity As New TreeNode()
                nodeActivity.Text = dvActivity(ParentNode).Item("Name").ToString()
                nodeActivity.ToolTip = dvActivity(ParentNode).Item("Name").ToString()
                nodeActivity.Value = dvActivity(ParentNode).Item("Id").ToString()
                If (Request.QueryString("ProjectName") <> "") Then
                    nodeActivity.Checked = True
                End If
                nodeActivity.SelectAction = TreeNodeSelectAction.None
                nodeActivity.ChildNodes.Add(nodeActivity)
                dvChild = New DataView(dtActivity)
                dvChild.RowFilter = "Treelevel=1 AND ParentId=" + dvActivity(ParentNode).Item("Id").ToString()
                dvChild.Sort = "iNodeNo"
                For ChildNode = 0 To dvChild.Count - 1
                    Dim nodeChild As New TreeNode()
                    nodeChild.Text = dvChild(ChildNode)("Name").ToString()
                    nodeChild.ToolTip = dvChild(ChildNode)("Name").ToString()
                    nodeChild.Value = dvChild(ChildNode)("Id").ToString()
                    If (Request.QueryString("ProjectName") <> "") Then
                        nodeChild.Checked = True
                    End If
                    nodeChild.SelectAction = TreeNodeSelectAction.None
                    nodeActivity.ChildNodes.Add(nodeChild)
                Next 'Next Child Node
                Me.tvActivity.Nodes(0).ChildNodes.Add(nodeActivity)
            Next 'Newxt Parent Node
            Me.tvActivity.Nodes(0).Expand()
            Me.tvActivity.Nodes(0).SelectAction = TreeNodeSelectAction.None
            Return True

        Catch ex As Exception
            eStr = ex.Message
            Return False
        Finally
            dsActivity.Dispose()
            dtActivity.Dispose()
        End Try
    End Function

    Protected Function BindMasterGrid(ByVal strSubjectId As String, ByVal strParentID As String, ByVal strActivityId As String, ByRef eStr As String) As Boolean
        Dim dsMaster As New DataSet
        Dim cDataStatus As String = String.Empty
        Dim iWorkflowStageId As String = String.Empty
        Dim cSubjectWiseFlag As String = String.Empty
        Dim iPeriod As String = String.Empty
        Dim iProjectType As String = 0
        Dim ds_reviewer As New DataSet
        Dim dv As DataView
        Try
            cSubjectWiseFlag = "Y"
            If Me.ddlType.SelectedValue = Generic_Activity Then
                cSubjectWiseFlag = "N"
                strSubjectId = ""
            End If
            iPeriod = "," + Me.ddlPeriods.SelectedValue.ToString() + ","
            If Me.ddlPeriods.SelectedValue = "All" Then
                iPeriod = ""
            End If

            If Me.ddlFilter.SelectedValue = Data_Entry_Pending Then
                cDataStatus = ",0,"
                iWorkflowStageId = ",0,"
            ElseIf ddlFilter.SelectedValue = Data_Entry_Continue Then
                cDataStatus = ",B,"
                iWorkflowStageId = ",0,"
            ElseIf ddlFilter.SelectedValue = First_Review_Pending Then
                cDataStatus = ",D,"
                iWorkflowStageId = ",0,"
            Else
                ds_reviewer = CType(Me.Session(Vs_dsReviewerlevel), DataSet)
                dv = ds_reviewer.Tables(0).Copy.DefaultView
                dv.RowFilter = "Reviewer ='" + ddlFilter.SelectedItem.Text.Trim() + "'"

                If dv.ToTable.Rows.Count > 0 Then
                    If dv.ToTable.Rows(0)("vStatus") = "L" Then
                        cDataStatus = ",F,"
                    Else
                        cDataStatus = ",E,"
                    End If
                    iWorkflowStageId = "," + Convert.ToString(dv.ToTable.Rows(0)("iReviewWorkflowStageId")) + ","
                End If

            End If

            iProjectType = 1
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                iProjectType = 2
            End If
            objHelp.Timeout = -1

            Me.grdParent.Columns(GVC_DEP).Visible = True
            Me.grdParent.Columns(GVC_DEC).Visible = True
            Me.grdParent.Columns(GVC_FRP).Visible = True
            Me.grdParent.Columns(GVC_SRP).Visible = True
            Me.grdParent.Columns(GVC_FnlRP).Visible = True
            Me.grdParent.Columns(GVC_Locked).Visible = True

            If filllegends() Then
            End If

            If Not objHelp.proc_GetCRFActivityStatusCount(Me.HProjectId.Value, iPeriod, strSubjectId, strParentID, strActivityId, cSubjectWiseFlag, iProjectType, cDataStatus, iWorkflowStageId, dsMaster, eStr) Then
                Me.objcommon.ShowAlert("Error in Getting Data", Me)
                Exit Function
            End If
            Me.ViewState(VS_dsMaster) = dsMaster.Tables(0)
            Me.grdParent.DataSource = dsMaster.Tables(0)
            Me.grdParent.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ChangeColor", "ChangeColor();", True)
            If grdParent.Columns.Count > 0 Then

                ds_reviewer = CType(Me.Session(Vs_dsReviewerlevel), DataSet)
                dv = ds_reviewer.Tables(0).Copy.DefaultView
                dv.RowFilter = "Reviewer ='" + ddlFilter.SelectedItem.Text.Trim() + "'"
                If Me.ddlFilter.SelectedValue = Data_Entry_Pending Then
                    Me.grdParent.Columns(GVC_DEP).Visible = True
                    Me.grdParent.Columns(GVC_DEC).Visible = False
                    Me.grdParent.Columns(GVC_FRP).Visible = False
                    Me.grdParent.Columns(GVC_SRP).Visible = False
                    Me.grdParent.Columns(GVC_FnlRP).Visible = False
                    Me.grdParent.Columns(GVC_Locked).Visible = False

                ElseIf ddlFilter.SelectedValue = Data_Entry_Continue Then

                    Me.grdParent.Columns(GVC_DEP).Visible = False
                    Me.grdParent.Columns(GVC_DEC).Visible = True
                    Me.grdParent.Columns(GVC_FRP).Visible = False
                    Me.grdParent.Columns(GVC_SRP).Visible = False
                    Me.grdParent.Columns(GVC_FnlRP).Visible = False
                    Me.grdParent.Columns(GVC_Locked).Visible = False

                ElseIf ddlFilter.SelectedValue = First_Review_Pending Then

                    Me.grdParent.Columns(GVC_DEP).Visible = False
                    Me.grdParent.Columns(GVC_DEC).Visible = False
                    Me.grdParent.Columns(GVC_FRP).Visible = True
                    Me.grdParent.Columns(GVC_SRP).Visible = False
                    Me.grdParent.Columns(GVC_FnlRP).Visible = False
                    Me.grdParent.Columns(GVC_Locked).Visible = False
                ElseIf ddlFilter.SelectedValue.ToString() = "0" Then
                    Me.grdParent.Columns(GVC_DEP).Visible = True
                    Me.grdParent.Columns(GVC_DEC).Visible = True
                    Me.grdParent.Columns(GVC_FRP).Visible = True
                    Me.grdParent.Columns(GVC_SRP).Visible = False
                    Me.grdParent.Columns(GVC_FnlRP).Visible = False
                    Me.grdParent.Columns(GVC_Locked).Visible = False
                    For i As Integer = 0 To ds_reviewer.Tables(0).Rows.Count - 1
                        If ds_reviewer.Tables(0).Rows(i)("vStatus") = "L" Then
                            Me.grdParent.Columns(GVC_Locked).Visible = True
                        ElseIf ds_reviewer.Tables(0).Rows(i)("vStatus") = "FNLRP" Then
                            If ds_reviewer.Tables(0).Rows.Count = 2 Then
                                Me.grdParent.Columns(GVC_SRP).Visible = True
                            Else
                                Me.grdParent.Columns(GVC_FnlRP).Visible = True
                            End If
                        ElseIf ds_reviewer.Tables(0).Rows(i)("vStatus") = "SRP" Then
                            Me.grdParent.Columns(GVC_SRP).Visible = True
                        End If
                    Next

                Else

                    If dv.ToTable.Rows.Count > 0 Then
                        If dv.ToTable.Rows(0)("vStatus") = "L" Then
                            Me.grdParent.Columns(GVC_DEP).Visible = False
                            Me.grdParent.Columns(GVC_DEC).Visible = False
                            Me.grdParent.Columns(GVC_FRP).Visible = False
                            Me.grdParent.Columns(GVC_SRP).Visible = False
                            Me.grdParent.Columns(GVC_FnlRP).Visible = False
                            Me.grdParent.Columns(GVC_Locked).Visible = True
                        ElseIf dv.ToTable.Rows(0)("vStatus") = "FNLRP" Then
                            Me.grdParent.Columns(GVC_DEP).Visible = False
                            Me.grdParent.Columns(GVC_DEC).Visible = False
                            Me.grdParent.Columns(GVC_FRP).Visible = False
                            Me.grdParent.Columns(GVC_SRP).Visible = False
                            Me.grdParent.Columns(GVC_FnlRP).Visible = True
                            Me.grdParent.Columns(GVC_Locked).Visible = False
                        ElseIf dv.ToTable.Rows(0)("vStatus") = "SRP" Then
                            Me.grdParent.Columns(GVC_DEP).Visible = False
                            Me.grdParent.Columns(GVC_DEC).Visible = False
                            Me.grdParent.Columns(GVC_FRP).Visible = False
                            Me.grdParent.Columns(GVC_SRP).Visible = True
                            Me.grdParent.Columns(GVC_FnlRP).Visible = False
                            Me.grdParent.Columns(GVC_Locked).Visible = False
                        End If

                    End If
                End If
            End If
            Me.fldgrdParent.Style.Add("display", "")
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "..............BindMasterGrid")
            eStr = ex.Message
            Return False
        Finally
            dsMaster.Dispose()
        End Try
    End Function

    Protected Function GetData() As Boolean
        Dim mstrSubjectId As String = String.Empty
        Dim ds As DataSet = New DataSet
        Dim eStr As String = ""
        Dim strParentID As String = String.Empty
        Dim strActivityId As String = String.Empty
        Dim cSubjectWiseFlag As String = String.Empty
        Dim cDataStatus As String = String.Empty
        Dim iWorkflowStageId As String = String.Empty
        Dim strSubjectId As String = String.Empty
        Dim chCnt As Integer = 0
        Dim dv As New DataView
        Dim iPeriod As String = String.Empty
        Try
            If dtChild.Rows.Count = 0 Then
                iPeriod = "," + Me.ddlPeriods.SelectedValue.ToString() + ","
                If Me.ddlPeriods.SelectedValue = "All" Then
                    iPeriod = ""
                End If
                If Me.ddlType.SelectedValue = Sub_Specific_Activity Then
                    If tvSubject.Nodes(0).Checked = False Then
                        For index = 0 To tvSubject.Nodes(0).ChildNodes.Count - 1
                            If tvSubject.Nodes(0).ChildNodes(index).Checked = True Then
                                strSubjectId = strSubjectId + tvSubject.Nodes(0).ChildNodes(index).Value + ","
                            End If
                        Next
                        If strSubjectId <> "" Then
                            strSubjectId.Remove(strSubjectId.Length - 1)
                            strSubjectId = "'," + strSubjectId + "'"
                        End If
                    End If
                End If
                If tvActivity.Nodes(0).Checked = False Then
                    For index = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                        If tvActivity.Nodes(0).ChildNodes(index).Checked = True Then
                            strParentID = strParentID + tvActivity.Nodes(0).ChildNodes(index).Value + ","
                        End If
                    Next
                End If

                If tvActivity.Nodes(0).Checked = False Then
                    For iParent = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                        For iChild = 0 To tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes.Count - 1
                            If tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Checked = True Then
                                strActivityId = strActivityId + tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Value + ","
                                If tvActivity.Nodes(0).ChildNodes(iParent).Checked Then
                                    strActivityId = strActivityId + tvActivity.Nodes(0).ChildNodes(iParent).Value + ","
                                End If
                            End If
                        Next 'Next iChild
                    Next 'Next iParent
                End If
                If strParentID <> "" Then
                    strParentID.Remove(strParentID.Length - 1)
                    strParentID = "'," + strParentID + "'"
                End If
                If strActivityId <> "" Then
                    strActivityId.Remove(strActivityId.Length - 1)
                    strActivityId = "'," + strActivityId + "'"
                End If
                cSubjectWiseFlag = "Y"
                If Me.ddlType.SelectedValue = Generic_Activity Then
                    cSubjectWiseFlag = "N"
                    strSubjectId = ""
                End If
                If Me.ddlFilter.SelectedValue = Data_Entry_Pending Then
                    cDataStatus = ",0,"
                    iWorkflowStageId = ",0,"
                ElseIf ddlFilter.SelectedValue = Data_Entry_Continue Then
                    cDataStatus = ",B,"
                    iWorkflowStageId = ",0,"
                ElseIf ddlFilter.SelectedValue = First_Review_Pending Then
                    cDataStatus = ",D,"
                    iWorkflowStageId = ",0,"
                ElseIf ddlFilter.SelectedValue = Second_Review_Pending Then
                    cDataStatus = ",E,"
                    iWorkflowStageId = ",10,"
                ElseIf ddlFilter.SelectedValue = Final_Review_Pending Then
                    cDataStatus = ",E,"
                    iWorkflowStageId = ",20,"
                ElseIf ddlFilter.SelectedValue = Reviewed_Locked Then
                    cDataStatus = ",F,"
                    iWorkflowStageId = ",30,"
                End If
                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                    If Not objHelp.proc_GetCRFActivityStatusCTM(Me.HProjectId.Value, iPeriod, strSubjectId, strParentID, strActivityId, cSubjectWiseFlag, cDataStatus, iWorkflowStageId, ds, eStr) Then
                        Throw New Exception(eStr)
                    End If
                Else
                    If Not objHelp.proc_GetCRFActivityStatusBABE(Me.HProjectId.Value, iPeriod, strSubjectId, strParentID, strActivityId, cSubjectWiseFlag, cDataStatus, iWorkflowStageId, ds, eStr) Then
                        Throw New Exception(eStr)
                    End If
                End If
                dtChild = ds.Tables(0)
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "................GetData")
        Finally
            ds.Dispose()
            dv.Dispose()
        End Try
    End Function

#End Region

#Region "Drop Down Events"

    Protected Sub ddlType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlType.SelectedIndexChanged
        Dim eStr As String = String.Empty
        Try
            If Not Me.HProjectId.Value.Trim.ToString = "" Then
                Me.tvSubject.Nodes.Clear()
                Me.tvActivity.Nodes.Clear()
                Me.grdParent.DataSource = Nothing
                Me.grdParent.DataBind()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ChangeColor", "ChangeColor();", True)
                Me.fldgrdParent.Style.Add("display", "none")

                Me.tdHRUpper.Style.Add("display", "none")
                Me.tdHRLower.Style.Add("display", "none")
                Me.divSubject.Style.Add("display", "none")
                Me.divActivity.Style.Add("display", "none")
                Me.tvActivity.Nodes.Clear()
                Me.tvSubject.Nodes.Clear()
                If Me.ddlType.SelectedIndex <> 0 Then
                    If Not BindSubjectTree(eStr) Then
                        Throw New Exception(eStr)
                    End If

                    If Not BindActivityTree(eStr) Then
                        Throw New Exception(eStr)
                    End If

                    Me.tdHRUpper.Style.Add("display", "")
                    Me.tdHRLower.Style.Add("display", "")

                    If Me.ddlType.SelectedIndex = 1 Then
                        Me.divSubject.Style.Add("display", "block")
                    End If

                    Me.divActivity.Style.Add("display", "block")
                    btnGo.Visible = True
                End If
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".............ddlType_SelectedIndexChanged")
        End Try
    End Sub

    Protected Sub ddlPeriods_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPeriods.SelectedIndexChanged
        Try
            If Me.ddlType.SelectedIndex = 0 Then

                Me.tdHRUpper.Style.Add("display", "none")
                Me.tdHRLower.Style.Add("display", "none")
                Me.divSubject.Style.Add("display", "none")
                Me.divActivity.Style.Add("display", "none")
                Me.tvActivity.Nodes.Clear()
                Me.tvSubject.Nodes.Clear()
                Me.tvSubject.Nodes.Clear()
                Me.tvActivity.Nodes.Clear()
                Me.grdParent.DataSource = Nothing
                Me.grdParent.DataBind()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ChangeColor", "ChangeColor();", True)
                Me.fldgrdParent.Style.Add("display", "none")
                Exit Sub
            End If
            ddlType_SelectedIndexChanged(sender, e)
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".............ddlPeriods_SelectedIndexChanged")
        End Try
    End Sub

    Protected Sub ddlFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFilter.SelectedIndexChanged
        Try
            Me.ddlFilter.Items.FindByValue("1").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Red) + ";font-size: 8pt")
            Me.ddlFilter.Items.FindByValue("2").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Orange))
            Me.ddlFilter.Items.FindByValue("3").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Blue))
            If ddlFilter.Items.Count = 7 Then
                Me.ddlFilter.Items.FindByValue("4").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Purple))
                Me.ddlFilter.Items.FindByValue("5").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Green))
                Me.ddlFilter.Items.FindByValue("6").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Gray))
            ElseIf ddlFilter.Items.Count = 6 Then
                Me.ddlFilter.Items.FindByValue("4").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Purple))
                Me.ddlFilter.Items.FindByValue("5").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Green))
            ElseIf ddlFilter.Items.Count = 5 Then
                Me.ddlFilter.Items.FindByValue("4").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Purple))
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...........ddlFilter_SelectedIndexChanged")
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

    Protected Sub gvExport_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvExport.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                If e.Row.Cells(2).Text = "" Or e.Row.Cells(2).Text = "&nbsp;" Then
                    e.Row.Cells(2).Text = "Data Entry Pending"

                ElseIf e.Row.Cells(2).Text = "DEC" Then
                    e.Row.Cells(2).Text = "Data Entry Continue"

                ElseIf e.Row.Cells(2).Text = "FRP" Then
                    e.Row.Cells(2).Text = "Ready For Review"

                ElseIf e.Row.Cells(2).Text = "SRP" Then
                    e.Row.Cells(2).Text = "First Review Done"


                ElseIf e.Row.Cells(2).Text = "FNLRP" Then
                    e.Row.Cells(2).Text = "Second Review Done"


                ElseIf e.Row.Cells(2).Text = "L" Then
                    e.Row.Cells(2).Text = "Final Reviewed & Freezed"

                ElseIf e.Row.Cells(2).Text = "DCF" Then
                    e.Row.Cells(2).Text = "DCF"

                End If
            End If
        Catch
        End Try
    End Sub

#Region "Export to Excel"

    Protected Sub btnExportAll_Click(sender As Object, e As EventArgs) Handles btnExportAll.Click


        If hdnexport.Value.ToString() = "0" Then
            ParentActivityExport()
        Else
            ActivityExport()
        End If
       
    End Sub

#End Region

#Region "Report Helper Functions"

    Private Function OpenReport(ByVal ReportFileName_1 As String) As Boolean
        '' This Function open file on physical memory(In HardDist)          
        If File.Exists(ReportFileName_1) = True Then
            File.Delete(ReportFileName_1)
        End If
        rPage = New RepoPage(ReportFileName_1)
    End Function

    Private Sub ReportHeader()
        Dim rRow As RepoRow
        Dim rCell As RepoCell

        rRow = New RepoRow
        rCell = rRow.AddCell("CompanyTitle")
        rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
        rCell.FontBold = True
        rCell.FontSize = 14
        rCell.Value = System.Configuration.ConfigurationManager.AppSettings("Client")
        rCell.NoofCellContain = 10
        rCell.FontColor = Drawing.Color.Maroon
        rCell.BackgroundColor = Drawing.Color.White
        rPage.Say(rRow)

        rRow = New RepoRow
        rCell = rRow.AddCell("ReportTitle")
        rCell.Value = "CRF Activity Status Report"
        rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
        rCell.FontBold = True
        rCell.FontSize = 12
        rCell.FontColor = Drawing.Color.Maroon
        rCell.NoofCellContain = 10
        rPage.Say(rRow)

        rRow = New RepoRow
        rCell = rRow.AddCell("ProjectName")
        rCell.Value = "Project: " & Me.txtproject.Text.Trim() & ""
        rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
        rCell.FontBold = True
        rCell.FontSize = 12
        rCell.FontColor = Drawing.Color.Maroon
        rCell.NoofCellContain = 12
        rPage.Say(rRow)

        rPage.SayBlankRow()

    End Sub

    Private Sub PrintHeader()
        Dim rRow As RepoRow
        Dim index As Integer
        Dim ds_reviewer As New DataSet
        Dim dv As DataView
        rRow = New RepoRow
        rRow = masterRow()
        ds_reviewer = CType(Me.Session(Vs_dsReviewerlevel), DataSet)

        If ddlFilter.SelectedValue.ToString() = "0" Then
            rRow.Cell("SubjectNo\ScreenNo").Value = "SubjectNo\ScreenNo"
            rRow.Cell("SubjectNo\ScreenNo").BackgroundColor = Drawing.Color.Navy
            rRow.Cell("SubjectNo\ScreenNo").FontColor = Drawing.Color.White
            rRow.Cell("DataEntryPending").Value = "Data Entry Pending"
            rRow.Cell("DataEntryPending").BackgroundColor = Drawing.Color.Red
            rRow.Cell("DataEntryPending").FontColor = Drawing.Color.White
            rRow.Cell("DataEntryContinue").Value = "Data Entry Continue"
            rRow.Cell("DataEntryContinue").BackgroundColor = Drawing.Color.Orange
            rRow.Cell("DataEntryContinue").FontColor = Drawing.Color.White
            rRow.Cell("ReadyForReview").Value = "Ready For Review"
            rRow.Cell("ReadyForReview").BackgroundColor = Drawing.Color.Blue
            rRow.Cell("ReadyForReview").FontColor = Drawing.Color.White
            ''Added by nipun khant for dynamic review
            If ds_reviewer.Tables(0).Rows.Count = 1 Then
                rRow.Cell("FinalReviewedFreeze").Value = ds_reviewer.Tables(0).Rows(0)("Reviewer")
                'rRow.Cell("FinalReviewedFreeze").BackgroundColor = ColorTranslator.FromHtml("#FF00FF")
                rRow.Cell("FinalReviewedFreeze").BackgroundColor = ColorTranslator.FromHtml(ds_reviewer.Tables(0).Rows(0)("vColorCodeForDynamic"))
                rRow.Cell("FinalReviewedFreeze").FontColor = Drawing.Color.White
            ElseIf ds_reviewer.Tables(0).Rows.Count = 2 Then

                For I As Integer = 0 To ds_reviewer.Tables(0).Rows.Count - 1
                    If ds_reviewer.Tables(0).Rows(I)("vStatus") = "L" Then
                        rRow.Cell("FinalReviewedFreeze").Value = ds_reviewer.Tables(0).Rows(I)("Reviewer")
                        rRow.Cell("FinalReviewedFreeze").BackgroundColor = ColorTranslator.FromHtml(ds_reviewer.Tables(0).Rows(I)("vColorCodeForDynamic"))
                        'rRow.Cell("FinalReviewedFreeze").BackgroundColor = Drawing.Color.Green
                        rRow.Cell("FinalReviewedFreeze").FontColor = Drawing.Color.White
                    ElseIf ds_reviewer.Tables(0).Rows(I)("vStatus") = "SRP" Then
                        rRow.Cell("FirstReviewDone").Value = ds_reviewer.Tables(0).Rows(I)("Reviewer")
                        rRow.Cell("FirstReviewDone").FontColor = Drawing.Color.White
                        rRow.Cell("FirstReviewDone").BackgroundColor = ColorTranslator.FromHtml(ds_reviewer.Tables(0).Rows(I)("vColorCodeForDynamic"))
                        'rRow.Cell("FirstReviewDone").BackgroundColor = ColorTranslator.FromHtml("#FF00FF")
                    End If
                Next
            ElseIf ds_reviewer.Tables(0).Rows.Count = 3 Then
                For I As Integer = 0 To ds_reviewer.Tables(0).Rows.Count - 1
                    If ds_reviewer.Tables(0).Rows(I)("vStatus") = "L" Then
                        rRow.Cell("FinalReviewedFreeze").Value = ds_reviewer.Tables(0).Rows(I)("Reviewer")
                        'rRow.Cell("FinalReviewedFreeze").BackgroundColor = Drawing.Color.Gray
                        rRow.Cell("FinalReviewedFreeze").BackgroundColor = ColorTranslator.FromHtml(ds_reviewer.Tables(0).Rows(I)("vColorCodeForDynamic"))
                        rRow.Cell("FinalReviewedFreeze").FontColor = Drawing.Color.White
                    ElseIf ds_reviewer.Tables(0).Rows(I)("vStatus") = "FNLRP" Then
                        rRow.Cell("SecondReviewDone").Value = ds_reviewer.Tables(0).Rows(I)("Reviewer")
                        rRow.Cell("SecondReviewDone").FontColor = Drawing.Color.White
                        rRow.Cell("SecondReviewDone").BackgroundColor = ColorTranslator.FromHtml(ds_reviewer.Tables(0).Rows(I)("vColorCodeForDynamic"))
                        'rRow.Cell("FirstReviewDone").BackgroundColor = ColorTranslator.FromHtml("#FF00FF")
                    ElseIf ds_reviewer.Tables(0).Rows(I)("vStatus") = "SRP" Then
                        rRow.Cell("FirstReviewDone").Value = ds_reviewer.Tables(0).Rows(I)("Reviewer")
                        rRow.Cell("FirstReviewDone").BackgroundColor = ColorTranslator.FromHtml(ds_reviewer.Tables(0).Rows(I)("vColorCodeForDynamic"))
                        'rRow.Cell("SecondReviewDone").BackgroundColor = Drawing.Color.Green
                        rRow.Cell("FirstReviewDone").FontColor = Drawing.Color.White
                    End If

                Next
            End If
           
            rRow.Cell("DCF").Value = "DCF"
            rRow.Cell("DCF").BackgroundColor = Drawing.Color.Navy
            rRow.Cell("DCF").FontColor = Drawing.Color.White
        ElseIf ddlFilter.SelectedValue.ToString() = "1" Then
            rRow.Cell("SubjectNo\ScreenNo").Value = "SubjectNo\ScreenNo"
            rRow.Cell("SubjectNo\ScreenNo").BackgroundColor = Drawing.Color.Navy
            rRow.Cell("SubjectNo\ScreenNo").FontColor = Drawing.Color.White
            rRow.Cell("DataEntryPending").Value = "Data Entry Pending"
            rRow.Cell("DataEntryPending").BackgroundColor = Drawing.Color.Red
            rRow.Cell("DataEntryPending").FontColor = Drawing.Color.White
            rRow.Cell("DCF").Value = "DCF"
            rRow.Cell("DCF").BackgroundColor = Drawing.Color.Navy
            rRow.Cell("DCF").FontColor = Drawing.Color.White
        ElseIf ddlFilter.SelectedValue.ToString() = "2" Then
            rRow.Cell("SubjectNo\ScreenNo").Value = "SubjectNo\ScreenNo"
            rRow.Cell("SubjectNo\ScreenNo").BackgroundColor = Drawing.Color.Navy
            rRow.Cell("SubjectNo\ScreenNo").FontColor = Drawing.Color.White
            rRow.Cell("DataEntryContinue").Value = "Data Entry Continue"
            rRow.Cell("DataEntryContinue").BackgroundColor = Drawing.Color.Orange
            rRow.Cell("DataEntryContinue").FontColor = Drawing.Color.White
            rRow.Cell("DCF").Value = "DCF"
            rRow.Cell("DCF").BackgroundColor = Drawing.Color.Navy
            rRow.Cell("DCF").FontColor = Drawing.Color.White
        ElseIf ddlFilter.SelectedValue.ToString() = "3" Then
            rRow.Cell("SubjectNo\ScreenNo").Value = "SubjectNo\ScreenNo"
            rRow.Cell("SubjectNo\ScreenNo").BackgroundColor = Drawing.Color.Navy
            rRow.Cell("SubjectNo\ScreenNo").FontColor = Drawing.Color.White
            rRow.Cell("ReadyForReview").Value = "Ready For Review"
            rRow.Cell("ReadyForReview").BackgroundColor = Drawing.Color.Blue
            rRow.Cell("ReadyForReview").FontColor = Drawing.Color.White
            rRow.Cell("DCF").Value = "DCF"
            rRow.Cell("DCF").BackgroundColor = Drawing.Color.Navy
            rRow.Cell("DCF").FontColor = Drawing.Color.White

            ''Added by nipun khant for dynamic review
        Else
            rRow.Cell("SubjectNo\ScreenNo").Value = "SubjectNo\ScreenNo"
            rRow.Cell("SubjectNo\ScreenNo").BackgroundColor = Drawing.Color.Navy
            rRow.Cell("SubjectNo\ScreenNo").FontColor = Drawing.Color.White

            ds_reviewer = CType(Me.Session(Vs_dsReviewerlevel), DataSet)
            dv = ds_reviewer.Tables(0).Copy.DefaultView
            dv.RowFilter = "Reviewer ='" + ddlFilter.SelectedItem.Text.Trim() + "'"

            If dv.ToTable.Rows.Count > 0 Then
                If dv.ToTable.Rows(0)("vStatus") = "L" Then
                    rRow.Cell("FinalReviewedFreeze").Value = dv.ToTable.Rows(0)("Reviewer")
                    rRow.Cell("FinalReviewedFreeze").BackgroundColor = ColorTranslator.FromHtml(dv.ToTable.Rows(0)("vColorCodeForDynamic"))
                    rRow.Cell("FinalReviewedFreeze").FontColor = Drawing.Color.White

                ElseIf dv.ToTable.Rows(0)("vStatus") = "FNLRP" Then
                    rRow.Cell("SecondReviewDone").Value = dv.ToTable.Rows(0)("Reviewer")
                    rRow.Cell("SecondReviewDone").BackgroundColor = ColorTranslator.FromHtml(dv.ToTable.Rows(0)("vColorCodeForDynamic"))
                    rRow.Cell("SecondReviewDone").FontColor = Drawing.Color.White

                ElseIf dv.ToTable.Rows(0)("vStatus") = "SRP" Then
                    rRow.Cell("FirstReviewDone").Value = dv.ToTable.Rows(0)("Reviewer")
                    rRow.Cell("FirstReviewDone").BackgroundColor = ColorTranslator.FromHtml(dv.ToTable.Rows(0)("vColorCodeForDynamic"))
                    rRow.Cell("FirstReviewDone").FontColor = Drawing.Color.White
                End If

            End If

            rRow.Cell("DCF").Value = "DCF"
            rRow.Cell("DCF").BackgroundColor = Drawing.Color.Navy
            rRow.Cell("DCF").FontColor = Drawing.Color.White

           
        End If

        For index = 0 To rRow.CellCount - 1
            rRow.Cell(index).FontBold = True
            rRow.Cell(index).Alignment = RepoCell.AlignmentEnum.CenterTop
        Next

        rPage.Say(rRow)

    End Sub

    Private Sub ReportDetail()
        Dim RowCnt As Integer
        Dim rRow As RepoRow
        Dim dt_Report As New DataTable
        Dim PreviousCompany As String = ""
        Dim PreviousProject As String = ""
        Dim ds_reviewer As New DataSet
        Dim dv As DataView
        Try

            rRow = masterRow()
            PrintHeader()

            dt_Report = CType(Me.ViewState(VS_dsMaster), DataTable)

            RowCnt = 0
            ds_reviewer = CType(Me.Session(Vs_dsReviewerlevel), DataSet)
            dv = ds_reviewer.Tables(0).Copy.DefaultView
            dv.RowFilter = "Reviewer ='" + ddlFilter.SelectedItem.Text.Trim() + "'"
            Do While RowCnt <= dt_Report.Rows.Count - 1

                If ddlFilter.SelectedValue.ToString() = "0" Then

                    rRow.Cell("SubjectNo\ScreenNo").Value = dt_Report.Rows(RowCnt)("vMySubjectNo").ToString()
                    rRow.Cell("DataEntryPending").Value = dt_Report.Rows(RowCnt)("DEP").ToString()
                    rRow.Cell("DataEntryPending").FontColor = Drawing.Color.Red
                    rRow.Cell("DataEntryContinue").Value = dt_Report.Rows(RowCnt)("DEC").ToString()
                    rRow.Cell("DataEntryContinue").FontColor = Drawing.Color.Orange
                    rRow.Cell("ReadyForReview").Value = dt_Report.Rows(RowCnt)("FRP").ToString()
                    rRow.Cell("ReadyForReview").FontColor = Drawing.Color.Blue

                    ''Added by nipun khant for dynamic review
                    If ds_reviewer.Tables(0).Rows.Count = 1 Then
                        rRow.Cell("FinalReviewedFreeze").Value = dt_Report.Rows(RowCnt)("Locked").ToString()
                        'rRow.Cell("FinalReviewedFreeze").FontColor = ColorTranslator.FromHtml("#FF00FF")
                        rRow.Cell("FinalReviewedFreeze").FontColor = ColorTranslator.FromHtml(ds_reviewer.Tables(0).Rows(0)("vColorCodeForDynamic"))
                    ElseIf ds_reviewer.Tables(0).Rows.Count = 2 Then
                        rRow.Cell("FirstReviewDone").Value = dt_Report.Rows(RowCnt)("SRP").ToString()
                        'rRow.Cell("FirstReviewDone").FontColor = ColorTranslator.FromHtml("#FF00FF")
                        rRow.Cell("FirstReviewDone").FontColor = ColorTranslator.FromHtml(ds_reviewer.Tables(0).Rows(0)("vColorCodeForDynamic"))
                        rRow.Cell("FinalReviewedFreeze").Value = dt_Report.Rows(RowCnt)("Locked").ToString()
                        'rRow.Cell("FinalReviewedFreeze").FontColor = Drawing.Color.Green
                        rRow.Cell("FinalReviewedFreeze").FontColor = ColorTranslator.FromHtml(ds_reviewer.Tables(0).Rows(1)("vColorCodeForDynamic"))
                    ElseIf ds_reviewer.Tables(0).Rows.Count = 3 Then
                        rRow.Cell("FirstReviewDone").Value = dt_Report.Rows(RowCnt)("SRP").ToString()
                        'rRow.Cell("FirstReviewDone").FontColor = ColorTranslator.FromHtml("#FF00FF")
                        rRow.Cell("FirstReviewDone").FontColor = ColorTranslator.FromHtml(ds_reviewer.Tables(0).Rows(0)("vColorCodeForDynamic"))
                        rRow.Cell("SecondReviewDone").Value = dt_Report.Rows(RowCnt)("FnlRP").ToString()
                        'rRow.Cell("SecondReviewDone").FontColor = Drawing.Color.Green
                        rRow.Cell("SecondReviewDone").FontColor = ColorTranslator.FromHtml(ds_reviewer.Tables(0).Rows(1)("vColorCodeForDynamic"))
                        rRow.Cell("FinalReviewedFreeze").Value = dt_Report.Rows(RowCnt)("Locked").ToString()
                        rRow.Cell("FinalReviewedFreeze").FontColor = ColorTranslator.FromHtml(ds_reviewer.Tables(0).Rows(2)("vColorCodeForDynamic"))
                        'rRow.Cell("FinalReviewedFreeze").FontColor = Drawing.Color.Gray
                    End If

                   
                    rRow.Cell("DCF").Value = dt_Report.Rows(RowCnt)("DCF").ToString()
                    rPage.Say(rRow)
                    RowCnt = RowCnt + 1
                ElseIf ddlFilter.SelectedValue.ToString() = "1" Then
                    rRow.Cell("SubjectNo\ScreenNo").Value = dt_Report.Rows(RowCnt)("vMySubjectNo").ToString()
                    rRow.Cell("DataEntryPending").Value = dt_Report.Rows(RowCnt)("DEP").ToString()
                    rRow.Cell("DataEntryPending").FontColor = Drawing.Color.Red
                    rRow.Cell("DCF").Value = dt_Report.Rows(RowCnt)("DCF").ToString()
                    rPage.Say(rRow)
                    RowCnt = RowCnt + 1
                ElseIf ddlFilter.SelectedValue.ToString() = "2" Then
                    rRow.Cell("SubjectNo\ScreenNo").Value = dt_Report.Rows(RowCnt)("vMySubjectNo").ToString()
                    rRow.Cell("DataEntryContinue").Value = dt_Report.Rows(RowCnt)("DEC").ToString()
                    rRow.Cell("DataEntryContinue").FontColor = Drawing.Color.Orange
                    rRow.Cell("DCF").Value = dt_Report.Rows(RowCnt)("DCF").ToString()
                    rPage.Say(rRow)
                    RowCnt = RowCnt + 1
                ElseIf ddlFilter.SelectedValue.ToString() = "3" Then
                    rRow.Cell("SubjectNo\ScreenNo").Value = dt_Report.Rows(RowCnt)("vMySubjectNo").ToString()
                    rRow.Cell("ReadyForReview").Value = dt_Report.Rows(RowCnt)("FRP").ToString()
                    rRow.Cell("ReadyForReview").FontColor = Drawing.Color.Blue
                    rRow.Cell("DCF").Value = dt_Report.Rows(RowCnt)("DCF").ToString()
                    rPage.Say(rRow)
                    RowCnt = RowCnt + 1

                Else
                    ''added by nipun khant for dynamic review
                    rRow.Cell("SubjectNo\ScreenNo").Value = dt_Report.Rows(RowCnt)("vMySubjectNo").ToString()

                    ds_reviewer = CType(Me.Session(Vs_dsReviewerlevel), DataSet)
                    dv = ds_reviewer.Tables(0).Copy.DefaultView
                    dv.RowFilter = "Reviewer ='" + ddlFilter.SelectedItem.Text.Trim() + "'"

                    If dv.ToTable.Rows.Count > 0 Then
                        If dv.ToTable.Rows(0)("vStatus") = "L" Then
                            rRow.Cell("FinalReviewedFreeze").Value = dt_Report.Rows(RowCnt)("Locked").ToString()
                            rRow.Cell("FinalReviewedFreeze").FontColor = ColorTranslator.FromHtml(dv.ToTable.Rows(0)("vColorCodeForDynamic"))
                        ElseIf dv.ToTable.Rows(0)("vStatus") = "FNLRP" Then
                            rRow.Cell("SecondReviewDone").Value = dt_Report.Rows(RowCnt)("Locked").ToString()
                            rRow.Cell("SecondReviewDone").FontColor = ColorTranslator.FromHtml(dv.ToTable.Rows(0)("vColorCodeForDynamic"))
                        ElseIf dv.ToTable.Rows(0)("vStatus") = "SRP" Then
                            rRow.Cell("FirstReviewDone").Value = dt_Report.Rows(RowCnt)("SRP").ToString()
                            rRow.Cell("FirstReviewDone").FontColor = ColorTranslator.FromHtml(dv.ToTable.Rows(0)("vColorCodeForDynamic"))
                        End If

                    End If
                    rRow.Cell("DCF").Value = dt_Report.Rows(RowCnt)("DCF").ToString()
                    rPage.Say(rRow)
                    RowCnt = RowCnt + 1
                  
                End If

            Loop ''detail loop ending
            If RowCnt = dt_Report.Rows.Count Then
                If ddlFilter.SelectedValue.ToString = "0" Then
                    rRow.Cell("SubjectNo\ScreenNo").Value = "No. of Subjects: " + Me.hdnSubjectNo.Value
                    rRow.Cell("SubjectNo\ScreenNo").BackgroundColor = Drawing.Color.Navy
                    rRow.Cell("SubjectNo\ScreenNo").FontColor = Drawing.Color.White
                    rRow.Cell("DataEntryPending").Value = Me.hdnDEP.Value
                    rRow.Cell("DataEntryPending").BackgroundColor = Drawing.Color.Red
                    rRow.Cell("DataEntryPending").FontColor = Drawing.Color.White
                    rRow.Cell("DataEntryContinue").Value = Me.hdnDEC.Value
                    rRow.Cell("DataEntryContinue").BackgroundColor = Drawing.Color.Orange
                    rRow.Cell("DataEntryContinue").FontColor = Drawing.Color.White
                    rRow.Cell("ReadyForReview").Value = Me.hdnFRP.Value
                    rRow.Cell("ReadyForReview").BackgroundColor = Drawing.Color.Blue
                    rRow.Cell("ReadyForReview").FontColor = Drawing.Color.White
                    ''ADded by nipun khant for dynamic review
                    If ds_reviewer.Tables(0).Rows.Count = 1 Then
                        rRow.Cell("FinalReviewedFreeze").Value = Me.hdnLocked.Value
                        'rRow.Cell("FinalReviewedFreeze").BackgroundColor = ColorTranslator.FromHtml("#FF00FF")
                        rRow.Cell("FinalReviewedFreeze").BackgroundColor = ColorTranslator.FromHtml(ds_reviewer.Tables(0).Rows(0)("vColorCodeForDynamic"))
                        rRow.Cell("FinalReviewedFreeze").FontColor = Drawing.Color.White
                    ElseIf ds_reviewer.Tables(0).Rows.Count = 2 Then
                        rRow.Cell("FirstReviewDone").Value = Me.hdnSRP.Value
                        'rRow.Cell("FirstReviewDone").BackgroundColor = ColorTranslator.FromHtml("#FF00FF")
                        rRow.Cell("FirstReviewDone").BackgroundColor = ColorTranslator.FromHtml(ds_reviewer.Tables(0).Rows(0)("vColorCodeForDynamic"))
                        rRow.Cell("FirstReviewDone").FontColor = Drawing.Color.White
                        rRow.Cell("FinalReviewedFreeze").Value = Me.hdnLocked.Value
                        'rRow.Cell("FinalReviewedFreeze").BackgroundColor = Drawing.Color.Green
                        rRow.Cell("FinalReviewedFreeze").BackgroundColor = ColorTranslator.FromHtml(ds_reviewer.Tables(0).Rows(1)("vColorCodeForDynamic"))
                        rRow.Cell("FinalReviewedFreeze").FontColor = Drawing.Color.White
                    ElseIf ds_reviewer.Tables(0).Rows.Count = 3 Then
                        rRow.Cell("FirstReviewDone").Value = Me.hdnSRP.Value
                        'rRow.Cell("FirstReviewDone").BackgroundColor = ColorTranslator.FromHtml("#FF00FF")
                        rRow.Cell("FirstReviewDone").BackgroundColor = ColorTranslator.FromHtml(ds_reviewer.Tables(0).Rows(0)("vColorCodeForDynamic"))
                        rRow.Cell("FirstReviewDone").FontColor = Drawing.Color.White
                        rRow.Cell("SecondReviewDone").Value = Me.hdnFnlRP.Value
                        'rRow.Cell("SecondReviewDone").BackgroundColor = Drawing.Color.Green
                        rRow.Cell("SecondReviewDone").BackgroundColor = ColorTranslator.FromHtml(ds_reviewer.Tables(0).Rows(1)("vColorCodeForDynamic"))
                        rRow.Cell("SecondReviewDone").FontColor = Drawing.Color.White
                        rRow.Cell("FinalReviewedFreeze").Value = Me.hdnLocked.Value
                        'rRow.Cell("FinalReviewedFreeze").BackgroundColor = Drawing.Color.Gray
                        rRow.Cell("FinalReviewedFreeze").BackgroundColor = ColorTranslator.FromHtml(ds_reviewer.Tables(0).Rows(2)("vColorCodeForDynamic"))
                        rRow.Cell("FinalReviewedFreeze").FontColor = Drawing.Color.White
                    End If
                  
                    rRow.Cell("DCF").Value = Me.hdnDCF.Value
                    rRow.Cell("DCF").BackgroundColor = Drawing.Color.Navy
                    rRow.Cell("DCF").FontColor = Drawing.Color.White
                ElseIf ddlFilter.SelectedValue.ToString() = "1" Then
                    rRow.Cell("SubjectNo\ScreenNo").Value = "No. of Subjects: " + Me.hdnSubjectNo.Value
                    rRow.Cell("SubjectNo\ScreenNo").BackgroundColor = Drawing.Color.Navy
                    rRow.Cell("SubjectNo\ScreenNo").FontColor = Drawing.Color.White
                    rRow.Cell("DataEntryPending").Value = Me.hdnDEP.Value
                    rRow.Cell("DataEntryPending").BackgroundColor = Drawing.Color.Red
                    rRow.Cell("DataEntryPending").FontColor = Drawing.Color.White
                    rRow.Cell("DCF").Value = Me.hdnDCF.Value
                    rRow.Cell("DCF").BackgroundColor = Drawing.Color.Navy
                    rRow.Cell("DCF").FontColor = Drawing.Color.White
                ElseIf ddlFilter.SelectedValue.ToString() = "2" Then
                    rRow.Cell("SubjectNo\ScreenNo").Value = "No. of Subjects: " + Me.hdnSubjectNo.Value
                    rRow.Cell("SubjectNo\ScreenNo").BackgroundColor = Drawing.Color.Navy
                    rRow.Cell("SubjectNo\ScreenNo").FontColor = Drawing.Color.White
                    rRow.Cell("DataEntryContinue").Value = Me.hdnDEC.Value
                    rRow.Cell("DataEntryContinue").BackgroundColor = Drawing.Color.Orange
                    rRow.Cell("DataEntryContinue").FontColor = Drawing.Color.White
                    rRow.Cell("DCF").Value = Me.hdnDCF.Value
                    rRow.Cell("DCF").BackgroundColor = Drawing.Color.Navy
                    rRow.Cell("DCF").FontColor = Drawing.Color.White
                ElseIf ddlFilter.SelectedValue.ToString() = "3" Then
                    rRow.Cell("SubjectNo\ScreenNo").Value = "No. of Subjects: " + Me.hdnSubjectNo.Value
                    rRow.Cell("SubjectNo\ScreenNo").BackgroundColor = Drawing.Color.Navy
                    rRow.Cell("SubjectNo\ScreenNo").FontColor = Drawing.Color.White
                    rRow.Cell("ReadyForReview").Value = Me.hdnFRP.Value
                    rRow.Cell("ReadyForReview").BackgroundColor = Drawing.Color.Blue
                    rRow.Cell("ReadyForReview").FontColor = Drawing.Color.White
                    rRow.Cell("DCF").Value = Me.hdnDCF.Value
                    rRow.Cell("DCF").BackgroundColor = Drawing.Color.Navy
                    rRow.Cell("DCF").FontColor = Drawing.Color.White
                    ''Added by nipun khant for dynamic review
                Else
                    rRow.Cell("SubjectNo\ScreenNo").Value = "No. of Subjects: " + Me.hdnSubjectNo.Value
                    rRow.Cell("SubjectNo\ScreenNo").BackgroundColor = Drawing.Color.Navy
                    rRow.Cell("SubjectNo\ScreenNo").FontColor = Drawing.Color.White
                    ds_reviewer = CType(Me.Session(Vs_dsReviewerlevel), DataSet)
                    dv = ds_reviewer.Tables(0).Copy.DefaultView
                    dv.RowFilter = "Reviewer ='" + ddlFilter.SelectedItem.Text.Trim() + "'"

                    If dv.ToTable.Rows.Count > 0 Then
                        If dv.ToTable.Rows(0)("vStatus") = "L" Then
                            rRow.Cell("FinalReviewedFreeze").Value = Me.hdnLocked.Value
                            rRow.Cell("FinalReviewedFreeze").BackgroundColor = ColorTranslator.FromHtml(dv.ToTable.Rows(0)("vColorCodeForDynamic"))
                            rRow.Cell("FinalReviewedFreeze").FontColor = Drawing.Color.White

                        ElseIf dv.ToTable.Rows(0)("vStatus") = "FNLRP" Then
                            rRow.Cell("SecondReviewDone").Value = Me.hdnFnlRP.Value
                            rRow.Cell("SecondReviewDone").BackgroundColor = ColorTranslator.FromHtml(dv.ToTable.Rows(0)("vColorCodeForDynamic"))
                            rRow.Cell("SecondReviewDone").FontColor = Drawing.Color.White

                        ElseIf dv.ToTable.Rows(0)("vStatus") = "SRP" Then
                            rRow.Cell("FirstReviewDone").Value = Me.hdnSRP.Value
                            rRow.Cell("FirstReviewDone").BackgroundColor = ColorTranslator.FromHtml(dv.ToTable.Rows(0)("vColorCodeForDynamic"))
                            rRow.Cell("FirstReviewDone").FontColor = Drawing.Color.White
                        End If

                    End If

                    rRow.Cell("DCF").Value = Me.hdnDCF.Value
                    rRow.Cell("DCF").BackgroundColor = Drawing.Color.Navy
                    rRow.Cell("DCF").FontColor = Drawing.Color.White

                  
                End If
                rPage.Say(rRow)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message + "Error While Print Detail", "")
        End Try
    End Sub

    Private Function masterRow() As RepoRow
        Dim rRow As RepoRow
        Dim rCell As RepoCell
        Dim i As Integer
        Dim ds_reviewer As New DataSet
        Dim dv As DataView

        rRow = New RepoRow
        ds_reviewer = CType(Me.Session(Vs_dsReviewerlevel), DataSet)
        If ddlFilter.SelectedValue.ToString() = "0" Then

            rCell = New RepoCell("SubjectNo\ScreenNo")
            rRow.AddCell(rCell)

            rCell = New RepoCell("DataEntryPending")
            rRow.AddCell(rCell)

            rCell = New RepoCell("DataEntryContinue")
            rRow.AddCell(rCell)

            rCell = New RepoCell("ReadyForReview")
            rRow.AddCell(rCell)

            If ds_reviewer.Tables(0).Rows.Count = 1 Then
                rCell = New RepoCell("FinalReviewedFreeze")
                rRow.AddCell(rCell)
            ElseIf ds_reviewer.Tables(0).Rows.Count = 2 Then
                rCell = New RepoCell("FirstReviewDone")
                rRow.AddCell(rCell)

                rCell = New RepoCell("FinalReviewedFreeze")
                rRow.AddCell(rCell)
            ElseIf ds_reviewer.Tables(0).Rows.Count = 3 Then
                rCell = New RepoCell("FirstReviewDone")
                rRow.AddCell(rCell)
                rCell = New RepoCell("SecondReviewDone")
                rRow.AddCell(rCell)

                rCell = New RepoCell("FinalReviewedFreeze")
                rRow.AddCell(rCell)
            End If

            rCell = New RepoCell("DCF")
            rRow.AddCell(rCell)

        ElseIf ddlFilter.SelectedValue.ToString() = "1" Then

            rCell = New RepoCell("SubjectNo\ScreenNo")
            rRow.AddCell(rCell)

            rCell = New RepoCell("DataEntryPending")
            rRow.AddCell(rCell)

            rCell = New RepoCell("DCF")
            rRow.AddCell(rCell)

        ElseIf ddlFilter.SelectedValue.ToString() = "2" Then

            rCell = New RepoCell("SubjectNo\ScreenNo")
            rRow.AddCell(rCell)

            rCell = New RepoCell("DataEntryContinue")
            rRow.AddCell(rCell)

            rCell = New RepoCell("DCF")
            rRow.AddCell(rCell)

        ElseIf ddlFilter.SelectedValue.ToString() = "3" Then

            rCell = New RepoCell("SubjectNo\ScreenNo")
            rRow.AddCell(rCell)

            rCell = New RepoCell("ReadyForReview")
            rRow.AddCell(rCell)

            rCell = New RepoCell("DCF")
            rRow.AddCell(rCell)

        Else

            dv = ds_reviewer.Tables(0).Copy.DefaultView
            dv.RowFilter = "Reviewer ='" + ddlFilter.SelectedItem.Text.Trim() + "'"

            If dv.ToTable.Rows.Count > 0 Then
                If dv.ToTable.Rows(0)("vStatus") = "L" Then
                    rCell = New RepoCell("FinalReviewedFreeze")
                    rRow.AddCell(rCell)
                ElseIf dv.ToTable.Rows(0)("vStatus") = "FNLRP" Then
                    rCell = New RepoCell("SecondReviewDone")
                    rRow.AddCell(rCell)
                ElseIf dv.ToTable.Rows(0)("vStatus") = "SRP" Then
                    rCell = New RepoCell("FirstReviewDone")
                    rRow.AddCell(rCell)
                End If

            End If
            rCell = New RepoCell("DCF")
            rRow.AddCell(rCell)

        End If

        For i = 0 To rRow.CellCount - 1
            rRow.Cell(i).FontSize = 8
            rRow.Cell(i).LineBottom.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(i).LineTop.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(i).LineRight.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(i).LineLeft.LineType = RepoLine.LineTypeEnum.SingleLine
        Next i

        Return rRow

    End Function

#End Region

#Region "Dynamic review"

    Private Function GetLegends() As Boolean
        Dim ds As New DataSet
        Dim estr As String = String.Empty

        Try
            If Not Me.objHelp.Proc_GetLegends(Me.HProjectId.Value.ToString(), ds, estr) Then
                Throw New Exception("Error While Gettin Proc_getLegends" + estr)
            End If

            If ds Is Nothing Or ds.Tables.Count = 0 Then
                Return False
            End If

            Me.Session(Vs_dsReviewerlevel) = ds.Copy()

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....GetLegends")
            Return False
        End Try
    End Function

    Private Function FillReviewerfilter() As Boolean
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim SeqNo As Integer
        Try

            Me.ddlFilter.Items.Clear()
            dt.Columns.Add(New DataColumn("iSeqNo", GetType(Integer)))
            dt.Columns.Add(New DataColumn("vDesc", GetType(String)))

            dt.AcceptChanges()

            dr = dt.NewRow()
            dr("iSeqNo") = "0"
            dr("vDesc") = "All"
            dt.Rows.InsertAt(dr, 0)
            dt.AcceptChanges()
            dr = dt.NewRow()
            dr("iSeqNo") = "1"
            dr("vDesc") = "Data Entry Pending"
            dt.Rows.InsertAt(dr, 1)
            dt.AcceptChanges()
            dr = dt.NewRow()
            dr("iSeqNo") = "2"
            dr("vDesc") = "Data Entry Continue"
            dt.Rows.InsertAt(dr, 2)
            dt.AcceptChanges()
            dr = dt.NewRow()
            dr("iSeqNo") = "3"
            dr("vDesc") = "Ready For Review"
            dt.Rows.InsertAt(dr, 3)
            dt.AcceptChanges()

            dt.DefaultView.Sort = "iSeqNo Desc"
            ds = CType(Me.Session(Vs_dsReviewerlevel), DataSet)
            If ds.Tables(0).Rows.Count > 0 Then
                SeqNo = Convert.ToInt16(dt.DefaultView.ToTable.Rows(0)("iSeqNo")) + 1
                For Each dr_Reports As DataRow In ds.Tables(0).Rows
                    dr = dt.NewRow
                    dr("iSeqNo") = SeqNo
                    dr("vDesc") = Convert.ToString(dr_Reports("Reviewer"))
                    dt.Rows.Add(dr)
                    dt.AcceptChanges()
                    SeqNo = SeqNo + 1
                Next
            End If
            dt.DefaultView.Sort = "iSeqNo Asc"
            Me.ddlFilter.DataSource = dt
            Me.ddlFilter.DataTextField = "vDesc"
            Me.ddlFilter.DataValueField = "iSeqNo"
            Me.ddlFilter.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ChangeColor", "ChangeColor();", True)

            Me.ddlFilter.Items.FindByValue("0").Attributes.Add("style", "font-weight:bold")
            Me.ddlFilter.Items.FindByValue("1").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Red))
            Me.ddlFilter.Items.FindByValue("2").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Orange))
            Me.ddlFilter.Items.FindByValue("3").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Blue))

            If ddlFilter.Items.Count = 7 Then
                Me.ddlFilter.Items.FindByValue("4").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Purple))
                Me.ddlFilter.Items.FindByValue("5").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Green))
                Me.ddlFilter.Items.FindByValue("6").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Gray))
            ElseIf ddlFilter.Items.Count = 6 Then
                Me.ddlFilter.Items.FindByValue("4").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Purple))
                Me.ddlFilter.Items.FindByValue("5").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Green))
            ElseIf ddlFilter.Items.Count = 5 Then
                Me.ddlFilter.Items.FindByValue("4").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Purple))
            End If
            Me.ddlFilter.SelectedIndex = 0
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".......FillReviewerfilter")
            Return False
        End Try
    End Function

    Private Function filllegends() As Boolean
        Dim ds As New DataSet
        Dim estr As String = ""
        Dim lbl As New Label

        Try
            Me.PhlReviewer.Controls.Clear()
            ds = CType(Me.Session(Vs_dsReviewerlevel), DataSet)

            If ds.Tables(0).Rows.Count > 0 Then

                lbl = Getlable("&nbsp;&nbsp;&nbsp;", "lblRed", "")
                lbl.BackColor = Drawing.Color.Red
                Me.PhlReviewer.Controls.Add(lbl)
                Me.PhlReviewer.Controls.Add(New LiteralControl("-Data Entry Pending, "))
                lbl = New Label
                lbl = Getlable("&nbsp;&nbsp;&nbsp;", "lblOrange", "")
                lbl.BackColor = Drawing.Color.Orange
                Me.PhlReviewer.Controls.Add(lbl)
                Me.PhlReviewer.Controls.Add(New LiteralControl("-Data Entry Continue, "))
                lbl = New Label
                lbl = Getlable("&nbsp;&nbsp;&nbsp;", "lblBlue", "")
                lbl.BackColor = Drawing.Color.Blue
                Me.PhlReviewer.Controls.Add(lbl)
                Me.PhlReviewer.Controls.Add(New LiteralControl("-Ready For Review, "))
                For Each dr As DataRow In ds.Tables(0).Rows
                    lbl = New Label
                    lbl = Getlable("&nbsp;&nbsp;&nbsp;", "lbl" + dr("iActualWorkflowStageId").ToString(), "")
                    lbl.BackColor = System.Drawing.ColorTranslator.FromHtml(dr("vColorCodeForDynamic").ToString())
                    Me.PhlReviewer.Controls.Add(lbl)
                    Me.PhlReviewer.Controls.Add(New LiteralControl("-" + dr("Reviewer").ToString() + ", "))
                Next
            End If

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....GetLegends")
            Return False
        End Try
    End Function

    Private Function Getlable(ByVal vlabelName As String, ByVal Id As String, Optional ByVal vFieldType As String = "") As Label
        Dim lab As Label = New Label
        Try
            lab.ID = "Lab" & Id
            lab.Text = vlabelName.Trim()
            lab.SkinID = "lblDisplay"
            lab.ForeColor = System.Drawing.Color.FromName("Navy")
            If vFieldType.ToUpper.Trim() = "IMPORT" Then
                lab.Visible = False
            End If
            Getlable = lab
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....Getlable")
            Getlable = lab
        End Try

    End Function

#End Region

#Region "Excelselection"
    Private Function ActivityExport() As Boolean

        Dim fileName As String = String.Empty
        Dim Str As String = String.Empty
        Dim isReportComplete As Boolean = False
        Dim dt_Report As New DataTable
        Dim PreviousCompany As String = ""
        Dim PreviousProject As String = ""
        Dim ds_reviewer As New DataSet
        Dim ds As New DataSet
        Dim eStr As String = ""
        Dim strParentID As String = String.Empty
        Dim strActivityId As String = String.Empty
        Dim cSubjectWiseFlag As String = String.Empty
        Dim cDataStatus As String = String.Empty
        Dim iWorkflowStageId As String = String.Empty
        Dim strSubjectId As String = String.Empty
        Dim chCnt As Integer = 0
        Dim iPeriod As String = String.Empty
        Dim dtCRF_Report As New DataTable
        Dim drCRFReport As DataRow
        Dim strMessage As New StringBuilder
        Dim a As New ArrayList
        a.Add("sad")

        Try

            iPeriod = "," + Me.ddlPeriods.SelectedValue.ToString() + ","
            If Me.ddlPeriods.SelectedValue = "All" Then
                iPeriod = ""
            End If

            If tvActivity.Nodes(0).Checked = False Then
                For index = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                    If tvActivity.Nodes(0).ChildNodes(index).Checked = True Then
                        strParentID = strParentID + tvActivity.Nodes(0).ChildNodes(index).Value + ","
                    End If
                Next
            End If

            If Me.ddlType.SelectedValue = Sub_Specific_Activity Then
                If tvSubject.Nodes(0).Checked = False Then
                    For index = 0 To tvSubject.Nodes(0).ChildNodes.Count - 1
                        If tvSubject.Nodes(0).ChildNodes(index).Checked = True Then
                            strSubjectId = strSubjectId + tvSubject.Nodes(0).ChildNodes(index).Value + ","
                        End If
                    Next
                    If strSubjectId <> "" Then
                        strSubjectId.Remove(strSubjectId.Length - 1)
                        strSubjectId = "'," + strSubjectId + "'"
                    End If
                End If
            End If


            If tvActivity.Nodes(0).Checked = False Then
                For iParent = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                    For iChild = 0 To tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes.Count - 1
                        If tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Checked = True Then
                            strActivityId = strActivityId + tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Value + ","
                            If tvActivity.Nodes(0).ChildNodes(iParent).Checked Then
                                strActivityId = strActivityId + tvActivity.Nodes(0).ChildNodes(iParent).Value + ","
                            End If
                        End If
                    Next 'Next iChild
                Next 'Next iParent
            End If
            If strParentID <> "" Then
                strParentID.Remove(strParentID.Length - 1)
                strParentID = "'," + strParentID + "'"
            End If
            If strActivityId <> "" Then
                strActivityId.Remove(strActivityId.Length - 1)
                strActivityId = "'," + strActivityId + "'"
            End If
            cSubjectWiseFlag = "Y"
            If Me.ddlType.SelectedValue = Generic_Activity Then
                cSubjectWiseFlag = "N"
                strSubjectId = ""
            End If
            If Me.ddlFilter.SelectedValue = Data_Entry_Pending Then
                cDataStatus = ",0,"
                iWorkflowStageId = ",0,"
            ElseIf ddlFilter.SelectedValue = Data_Entry_Continue Then
                cDataStatus = ",B,"
                iWorkflowStageId = ",0,"
            ElseIf ddlFilter.SelectedValue = First_Review_Pending Then
                cDataStatus = ",D,"
                iWorkflowStageId = ",0,"
            ElseIf ddlFilter.SelectedValue = Second_Review_Pending Then
                cDataStatus = ",E,"
                iWorkflowStageId = ",10,"
            ElseIf ddlFilter.SelectedValue = Final_Review_Pending Then
                cDataStatus = ",E,"
                iWorkflowStageId = ",20,"
            ElseIf ddlFilter.SelectedValue = Reviewed_Locked Then
                cDataStatus = ",F,"
                iWorkflowStageId = ",30,"
            End If

            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                If Not objHelp.proc_GetCRFActivityStatusCTM(Me.HProjectId.Value, iPeriod, strSubjectId, strParentID, strActivityId, cSubjectWiseFlag, cDataStatus, iWorkflowStageId, ds, eStr) Then
                    Throw New Exception(eStr)
                End If
            Else
                If Not objHelp.proc_GetCRFActivityStatusBABE(Me.HProjectId.Value, iPeriod, strSubjectId, strParentID, strActivityId, cSubjectWiseFlag, cDataStatus, iWorkflowStageId, ds, eStr) Then
                    Throw New Exception(eStr)
                End If
            End If

            If Not dtCRF_Report Is Nothing Then
                dtCRF_Report.Columns.Add("Subject No")
                dtCRF_Report.Columns.Add("Randomization No")
                dtCRF_Report.Columns.Add("Parent/visit")
                dtCRF_Report.Columns.Add("Activity")
                dtCRF_Report.Columns.Add("Status")
                dtCRF_Report.Columns.Add("DCF")

            End If
            dtCRF_Report.AcceptChanges()

            dt_Report = ds.Tables(0)
            Dim dv As New DataView(dt_Report)

            For Each dr As DataRow In dv.ToTable.Rows
                drCRFReport = dtCRF_Report.NewRow()
                drCRFReport("Subject No") = dr("vMySubjectNo").ToString()
                drCRFReport("Randomization No") = dr("vRandomizationNo").ToString()
                drCRFReport("Parent/visit") = dr("parent").ToString()
                drCRFReport("Activity") = dr("child").ToString()
                If dr("Status").ToString() = "" Or dr("Status").ToString() = "&nbsp;" Then
                    drCRFReport("Status") = "Data Entry Pending"
                ElseIf dr("Status").ToString() = "DEC" Then
                    drCRFReport("Status") = "Data Entry Continue"
                ElseIf dr("Status").ToString() = "FRP" Then
                    drCRFReport("Status") = "Ready For Review"
                ElseIf dr("Status").ToString() = "SRP" Then
                    drCRFReport("Status") = "First Review Done"
                ElseIf dr("Status").ToString() = "FNLRP" Then
                    drCRFReport("Status") = "Second Review Done"
                ElseIf dr("Status").ToString() = "L" Then
                    drCRFReport("Status") = "Final Reviewed & Freezed"
                ElseIf dr("Status").ToString() = "DCF" Then
                    drCRFReport("Status") = "DCF"
                End If
                drCRFReport("DCF") = dr("DCF").ToString()

                dtCRF_Report.Rows.Add(drCRFReport)
                dtCRF_Report.AcceptChanges()

            Next
            gvExport.DataSource = dtCRF_Report
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

                fileName = CType(Master.FindControl("lblHeading"), Label).Text + "_" + Me.txtproject.Text.Split(" ")(0) + ".xls"

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

                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("CRF Activity Report")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td><td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td align=""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")

                gridviewHtml = strMessage.ToString() + gridviewHtml
                Context.Response.Buffer = True
                Context.Response.ClearContent()
                Context.Response.ClearHeaders()

                Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
                Context.Response.AddHeader("content-Disposition", "attachment; filename=" + fileName)
                Context.Response.AddHeader("Content-Length", gridviewHtml.Length)

                Context.Response.Write(gridviewHtml)
                Context.Response.Flush()
                Context.Response.End()

                System.IO.File.Delete(fileName)
            Else

                Dim info As String = String.Empty
                Dim gridviewHtml As String = String.Empty

                fileName = CType(Master.FindControl("lblHeading"), Label).Text + "_" + Me.txtproject.Text.Split(" ")(0) + ".xls"

                drCRFReport = dtCRF_Report.NewRow()
                drCRFReport("Parent/visit") = ""
                drCRFReport("Activity") = ""
                drCRFReport("Status") = ""
                drCRFReport("DCF") = ""
                dtCRF_Report.Rows.Add(drCRFReport)
                dtCRF_Report.AcceptChanges()
                gvExport.DataSource = dtCRF_Report
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

                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("CRF Activity Report")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td align=""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")

                gridviewHtml = strMessage.ToString() + gridviewHtml
                Context.Response.Buffer = True
                Context.Response.ClearContent()
                Context.Response.ClearHeaders()

                Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
                Context.Response.AddHeader("content-Disposition", "attachment; filename=" + fileName)
                Context.Response.AddHeader("Content-Length", gridviewHtml.Length)

                Context.Response.Write(gridviewHtml)
                Context.Response.Flush()
                Context.Response.End()

                System.IO.File.Delete(fileName)
                Return True

                Exit Function

            End If
        Catch ex As Exception
            Me.objcommon.ShowAlert("Error While Exporting Data To Excel : " + eStr, Me.Page)
            Return False
            Exit Function
        End Try
        Return True
    End Function

    Private Function ParentActivityExport() As Boolean
        Dim fileName As String = String.Empty
        Dim Str As String = String.Empty
        Dim isReportComplete As Boolean = False
        Try
            Dim a As New ArrayList
            a.Add("sad")

            fileName = CType(Master.FindControl("lblHeading"), Label).Text + "_" + Me.txtproject.Text.Split(" ")(0) + ".xls"
            fileName = Server.MapPath("~/ExcelReportFile/" + fileName)

            OpenReport(fileName)

            ReportHeader()

            ReportDetail()

            isReportComplete = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")

        Finally
            If Not rPage Is Nothing Then
                rPage.CloseReport()
                rPage = Nothing
            End If
        End Try

        If isReportComplete = True Then

            ReportGeneralFunction.ExportFileAtWeb(ReportGeneralFunction.ExportFileAsEnum.ExportAsXLS, HttpContext.Current.Response, fileName)

        End If

    End Function

#End Region

End Class
