Imports System.Drawing

Partial Class frmVisitSchedulerReport
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

    Private Const GV_SubjectNo As Integer = 1
    Private Const GV_DEP As Integer = 2
    Private Const GV_DEC As Integer = 3
    Private Const GV_FRP As Integer = 4
    Private Const GV_SRP As Integer = 5
    Private Const GV_FnlRP As Integer = 6
    Private Const GV_Locked As Integer = 7
    Private Const GV_DCF_Generated As Integer = 8
    Private Const GV_DCF_Answered As Integer = 9
    Private Const GV_DCF_Pending As Integer = 10
    Private total As Integer = 0
    Private DCF_Generated As Integer = 0
    Private DCF_Answered As Integer = 0

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            rbtnType.SelectedIndex = 0
            divSchedulerRrepot.Visible = True
            divSummeryRepot.Visible = False
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
            Page.Title = " :: Visit Scheduler Report ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")

            CType(Master.FindControl("lblHeading"), Label).Text = "Visit Scheduler Report"

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
            Me.AutoCompleteExtender2.ContextKey = "iUserId = " & Me.Session(S_UserID)
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
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Ref", "ChangeUrl('VisitScheduler','frmVisitSchedulerReport.aspx');", True)
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
        ddlSummeryPeriod.Items.Clear()
        ddlPeriods.Items.Clear()
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            Me.ddlPeriods.Items.Clear()
            If rbtnType.SelectedIndex = 0 Then
                wStr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "' and cStatusIndi<>'D'"
            Else
                wStr = "vWorkSpaceId = '" + Me.hdnSummeryProjectId.Value.Trim() + "' and cStatusIndi<>'D'"
            End If


            If Not objHelp.GetWorkspaceProtocolDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        ds_Periods, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_Periods Is Nothing Then
                Throw New Exception("Problem while getting data")
            End If

            If Not ds_Periods.Tables(0).Rows(0)("iNoOfPeriods") Is Nothing Then

                Periods = ds_Periods.Tables(0).Rows(0)("iNoOfPeriods")
                For count As Integer = 0 To Periods - 1
                    If rbtnType.SelectedIndex = 0 Then
                        Me.ddlPeriods.Items.Add((count + 1).ToString)
                    Else
                        Me.ddlSummeryPeriod.Items.Add((count + 1).ToString)
                    End If



                Next count

            End If
            If rbtnType.SelectedIndex = 0 Then
                Me.ddlPeriods.Items.Insert(0, "All")
            Else
                Me.ddlSummeryPeriod.Items.Insert(0, "All")
            End If


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

    Protected Sub btnSummarySetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSummarySetProject.Click
        Dim eStr As String = String.Empty
        Dim dsWorkSpace As New DataSet
        Try
            If Not objHelp.getworkspacemst(" vWorkSpaceId = '" + Me.hdnSummeryProjectId.Value.Trim() + "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsWorkSpace, eStr) Then
                Throw New Exception(eStr)
            End If

            If dsWorkSpace Is Nothing OrElse dsWorkSpace.Tables(0).Rows.Count = 0 Then
                Throw New Exception("No records available in WorkSpaceMst.")
            End If
            Me.chkParent.Checked = False
            Me.chkParent.Style("display") = "none"

            If dsWorkSpace.Tables(0).Rows(0)("cWorkSpaceType") = "P" Then
                Me.chkParent.Style("display") = ""
            End If

            Me.gvActivityCount.DataSource = Nothing
            Me.gvActivityCount.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ChangeColor", "ChangeColor();", True)
            Me.btnExport.Style("display") = "none"

            If Not FillDropDownListPeriods(eStr) Then
                Throw New Exception(eStr)
            End If
            upSummeryReport.Update()
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")

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
                    Next
                Next
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

    Protected Sub btnSummerygo_Click(sender As Object, e As EventArgs) Handles btnSummerygo.Click
        Dim iPeriod As String = String.Empty
        Dim dsCount As New DataSet
        Dim eStr As String = String.Empty
        Try
            Me.gvActivityCount.DataSource = Nothing
            Me.gvActivityCount.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ChangeColor", "ChangeColor();", True)
            Me.btnExport.Style("display") = "none"

            If Not Me.GetLegends() Then
                Exit Sub
            End If

            iPeriod = "," + Me.ddlSummeryPeriod.SelectedValue.ToString() + ","
            If Me.ddlSummeryPeriod.SelectedValue = "All" Then
                iPeriod = ""
            End If
            Dim wstr As String

            If chkParent.Style("display") = "none" Then
                wstr = Me.hdnSummeryProjectId.Value + "##" + iPeriod + "##" + "Y" + "##" + "2" + "##" + "Y" + "##"
            Else
                If chkParent.Checked = True Then
                    wstr = Me.hdnSummeryProjectId.Value + "##" + iPeriod + "##" + "Y" + "##" + "2" + "##" + "Y" + "##"
                Else
                    wstr = Me.hdnSummeryProjectId.Value + "##" + iPeriod + "##" + "Y" + "##" + "2" + "##" + "N" + "##"
                End If
            End If


            
            objHelp.Timeout = -1
            dsCount = objHelp.ProcedureExecute("dbo.Proc_GetVisitSchedulerSummeryReport", wstr)


            If Not dsCount Is Nothing AndAlso dsCount.Tables(0).Rows.Count = 0 Then
                Me.objcommon.ShowAlert("No records available for selected Project.", Me)
                Exit Sub
            Else
                Me.gvActivityCount.DataSource = dsCount
                Me.gvActivityCount.DataBind()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ChangeColor", "ChangeColorForSummery();", True)
                Me.btnExport.Style("display") = ""
                'ViewState(Vs_ActivityCount) = dsCount.Tables(0)
            End If



        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        End Try


    End Sub


    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("frmVisitSchedulerReport.aspx")
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
                    If chkUnScheduled.Checked = True Then
                        objHelp.Timeout = -1
                        Dim wStr = Me.HProjectId.Value + "##" + iPeriod + "##" + strSubjectId + "##" + strParentID + "##" + strActivityId + "##" + cSubjectWiseFlag + "##" + "" + "##" + "" + "##" + "" + "##"
                        ds = objHelp.ProcedureExecute("dbo.Proc_getVisitUnSchedulerReport_ForCT", wStr)
                    Else
                        If Not objHelp.Proc_getVisitSchedulerReport_ForCT(Me.HProjectId.Value, iPeriod, strSubjectId, strParentID, strActivityId, cSubjectWiseFlag, cDataStatus, iWorkflowStageId, ds, eStr) Then
                            Throw New Exception(eStr)
                        End If
                    End If
                    
                Else
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
                e.Row.Cells(GVC_DEC).ToolTip = "Data Entry Countinue"
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
                e.Row.Cells(GVC_DEC).ToolTip = "Data Entry Countinue"
                e.Row.Cells(GVC_FRP).ToolTip = "Ready For Review"
                e.Row.Cells(GVC_SRP).ToolTip = "First Review Done"
                e.Row.Cells(GVC_FnlRP).ToolTip = "Second Review Done"
                e.Row.Cells(GVC_Locked).ToolTip = "Final Reviewed & Freeze"
                e.Row.Cells(GVC_DCF_Pending).ToolTip = "DCF Pending"
            Else
                e.Row.Cells(GVC_SubjectNo).Text = "No. of Subjects: " + SubjectNo.ToString()
                e.Row.Cells(GVC_DEP).Text = DEP.ToString()
                e.Row.Cells(GVC_DEC).Text = DEC.ToString()
                e.Row.Cells(GVC_FRP).Text = FRP.ToString()
                e.Row.Cells(GVC_SRP).Text = SRP.ToString()
                e.Row.Cells(GVC_FnlRP).Text = FnlRP.ToString()
                e.Row.Cells(GVC_Locked).Text = Locked.ToString()
                e.Row.Cells(GVC_DCF_Pending).Text = DCF.ToString()
                e.Row.Cells(GVC_ActivityStatus).Text = "Total Activity"

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
                    CType(e.Row.FindControl("lnkActivity"), LinkButton).ToolTip = "Data Entry Countinue"

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

    Protected Sub grdExportToExcel_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdExportToExcel.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(5).Text = "FRP" Then
                e.Row.Cells(5).Text = "Ready For Review"
                e.Row.Cells(3).Text = ""
                e.Row.Cells(7).Text = ""
            ElseIf e.Row.Cells(5).Text = "SRP" Then
                e.Row.Cells(5).Text = "First Review Done"
                e.Row.Cells(3).Text = ""
                e.Row.Cells(7).Text = ""
            ElseIf e.Row.Cells(5).Text = "DEC" Then
                e.Row.Cells(5).Text = "Data Entry Continue"
                e.Row.Cells(3).Text = ""
                e.Row.Cells(7).Text = ""
            ElseIf e.Row.Cells(5).Text = "FNLRP" Then
                e.Row.Cells(5).Text = "Second Review Done"
                e.Row.Cells(3).Text = ""
                e.Row.Cells(7).Text = ""
            ElseIf e.Row.Cells(5).Text = "DEP" Then
                e.Row.Cells(5).Text = "Data Entry Pending"
            ElseIf e.Row.Cells(5).Text = "Locked" Then
                e.Row.Cells(5).Text = "Final Reviewed & Freezed"
                e.Row.Cells(3).Text = ""
                e.Row.Cells(7).Text = ""
            ElseIf e.Row.Cells(5).Text = "L" Then
                e.Row.Cells(5).Text = "Final Reviewed & Freezed"
                e.Row.Cells(3).Text = ""
                e.Row.Cells(7).Text = ""
            End If
        End If
    End Sub
    Protected Sub gvActivityCount_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvActivityCount.RowDataBound
        Dim ds As New DataSet
        Dim dv As DataView
        Dim iworkflowfinal As Integer = 0
        Dim srpcolor As String = ""
        Dim fnlrpcolor As String = ""
        Dim lcolor As String = ""
        Try

            ds = CType(Me.Session(Vs_dsReviewerlevel), DataSet).Copy()
            dv = ds.Tables(0).Copy().DefaultView
            dv.RowFilter = "vStatus = 'L'"

            If dv.ToTable.Rows.Count > 0 Then
                iworkflowfinal = Convert.ToInt32(dv.ToTable.Rows(0)("iActualWorkflowStageId"))
            End If

            dv = ds.Tables(0).Copy().DefaultView
            dv.Sort = "iActualWorkflowStageId asc"

            If dv.ToTable.Rows.Count = 1 Then
                srpcolor = dv.ToTable.Rows(0)("vColorCodeForDynamic")
            ElseIf dv.ToTable.Rows.Count = 2 Then
                srpcolor = dv.ToTable.Rows(0)("vColorCodeForDynamic")
                fnlrpcolor = dv.ToTable.Rows(1)("vColorCodeForDynamic")
            ElseIf dv.ToTable.Rows.Count = 3 Then
                srpcolor = dv.ToTable.Rows(0)("vColorCodeForDynamic")
                fnlrpcolor = dv.ToTable.Rows(1)("vColorCodeForDynamic")
                lcolor = dv.ToTable.Rows(2)("vColorCodeForDynamic")
            End If

            Select Case e.Row.RowType
                Case DataControlRowType.Header
                    e.Row.Cells(GV_DEP).BackColor = Drawing.Color.Red
                    e.Row.Cells(GV_DEC).BackColor = Drawing.Color.Orange
                    e.Row.Cells(GV_FRP).BackColor = Drawing.Color.Blue

                    If iworkflowfinal = 10 Then
                        'e.Row.Cells(GV_Locked).BackColor = ColorTranslator.FromHtml("#800080")
                        e.Row.Cells(GV_Locked).BackColor = ColorTranslator.FromHtml(srpcolor)
                        e.Row.Cells(GV_SRP).Visible = False
                        e.Row.Cells(GV_FnlRP).Visible = False
                        e.Row.Cells(GV_Locked).Visible = True
                    ElseIf iworkflowfinal = 20 Then
                        'e.Row.Cells(GV_SRP).BackColor = ColorTranslator.FromHtml("#800080")
                        'e.Row.Cells(GV_Locked).BackColor = ColorTranslator.FromHtml("#006000")
                        e.Row.Cells(GV_SRP).BackColor = ColorTranslator.FromHtml(srpcolor)
                        e.Row.Cells(GV_Locked).BackColor = ColorTranslator.FromHtml(fnlrpcolor)
                        e.Row.Cells(GV_FnlRP).Visible = False
                        e.Row.Cells(GV_Locked).Visible = True
                        e.Row.Cells(GV_SRP).Visible = True
                    ElseIf iworkflowfinal = 30 Then
                        e.Row.Cells(GV_SRP).BackColor = ColorTranslator.FromHtml(srpcolor)
                        e.Row.Cells(GV_FnlRP).BackColor = ColorTranslator.FromHtml(fnlrpcolor)
                        e.Row.Cells(GV_Locked).BackColor = ColorTranslator.FromHtml(lcolor)
                        e.Row.Cells(GV_Locked).Visible = True
                        e.Row.Cells(GV_FnlRP).Visible = True
                        e.Row.Cells(GV_SRP).Visible = True
                    End If

                    'e.Row.Cells(GV_SRP).BackColor = ColorTranslator.FromHtml("#50C000")
                    'e.Row.Cells(GV_FnlRP).BackColor = ColorTranslator.FromHtml("#006000")
                    'e.Row.Cells(GV_Locked).BackColor = Drawing.Color.Gray
                    e.Row.Cells(GV_DCF_Pending).BackColor = Drawing.Color.RoyalBlue
                    e.Row.Cells(GV_DCF_Generated).BackColor = Drawing.Color.RoyalBlue
                    e.Row.Cells(GV_DCF_Answered).BackColor = Drawing.Color.RoyalBlue

                Case DataControlRowType.DataRow
                    total += 1
                    e.Row.HorizontalAlign = HorizontalAlign.Center
                    e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Left

                    SubjectNo = SubjectNo + CType(e.Row.Cells(GV_SubjectNo).Text, Integer)

                    DEP = DEP + CType(e.Row.Cells(GV_DEP).Text, Integer)
                    DEC = DEC + CType(e.Row.Cells(GV_DEC).Text, Integer)
                    FRP = FRP + CType(e.Row.Cells(GV_FRP).Text, Integer)
                    SRP = SRP + CType(e.Row.Cells(GV_SRP).Text, Integer)
                    FnlRP = FnlRP + CType(e.Row.Cells(GV_FnlRP).Text, Integer)
                    Locked = Locked + CType(e.Row.Cells(GV_Locked).Text, Integer)
                    DCF = DCF + CType(e.Row.Cells(GV_DCF_Pending).Text, Integer)
                    DCF_Generated = DCF_Generated + CType(e.Row.Cells(GV_DCF_Generated).Text, Integer)
                    DCF_Answered = DCF_Answered + CType(e.Row.Cells(GV_DCF_Answered).Text, Integer)

                    e.Row.Cells(GV_DEP).ForeColor = Drawing.Color.Red
                    e.Row.Cells(GV_DEC).ForeColor = Drawing.Color.Orange
                    e.Row.Cells(GV_FRP).ForeColor = Drawing.Color.Blue
                    e.Row.Cells(GV_DCF_Pending).ForeColor = Drawing.Color.RoyalBlue
                    e.Row.Cells(GV_DCF_Generated).ForeColor = Drawing.Color.RoyalBlue
                    e.Row.Cells(GV_DCF_Answered).ForeColor = Drawing.Color.RoyalBlue
                    If iworkflowfinal = 10 Then
                        'e.Row.Cells(GV_Locked).ForeColor = ColorTranslator.FromHtml("#800080")
                        e.Row.Cells(GV_Locked).ForeColor = ColorTranslator.FromHtml(srpcolor)
                        e.Row.Cells(GV_SRP).Visible = False
                        e.Row.Cells(GV_FnlRP).Visible = False
                        e.Row.Cells(GV_Locked).Visible = True
                    ElseIf iworkflowfinal = 20 Then
                        'e.Row.Cells(GV_SRP).ForeColor = ColorTranslator.FromHtml("#800080")
                        'e.Row.Cells(GV_Locked).ForeColor = ColorTranslator.FromHtml("#006000")
                        e.Row.Cells(GV_SRP).ForeColor = ColorTranslator.FromHtml(srpcolor)
                        e.Row.Cells(GV_Locked).ForeColor = ColorTranslator.FromHtml(fnlrpcolor)
                        e.Row.Cells(GV_FnlRP).Visible = False
                        e.Row.Cells(GV_Locked).Visible = True
                        e.Row.Cells(GV_SRP).Visible = True
                    ElseIf iworkflowfinal = 30 Then
                        'e.Row.Cells(GV_SRP).ForeColor = ColorTranslator.FromHtml("#800080")
                        'e.Row.Cells(GV_FnlRP).ForeColor = ColorTranslator.FromHtml("#006000")
                        'e.Row.Cells(GV_Locked).ForeColor = Drawing.Color.Gray
                        e.Row.Cells(GV_SRP).ForeColor = ColorTranslator.FromHtml(srpcolor)
                        e.Row.Cells(GV_FnlRP).ForeColor = ColorTranslator.FromHtml(fnlrpcolor)
                        e.Row.Cells(GV_Locked).ForeColor = ColorTranslator.FromHtml(lcolor)
                        e.Row.Cells(GV_Locked).Visible = True
                        e.Row.Cells(GV_FnlRP).Visible = True
                        e.Row.Cells(GV_SRP).Visible = True
                    End If

                Case DataControlRowType.Footer

                    e.Row.Cells(GV_DEP).BackColor = Drawing.Color.Red
                    e.Row.Cells(GV_DEC).BackColor = Drawing.Color.Orange
                    e.Row.Cells(GV_FRP).BackColor = Drawing.Color.Blue
                    e.Row.Cells(GV_SRP).BackColor = ColorTranslator.FromHtml(srpcolor)
                    e.Row.Cells(GV_FnlRP).BackColor = ColorTranslator.FromHtml(fnlrpcolor)
                    e.Row.Cells(GV_Locked).BackColor = ColorTranslator.FromHtml(lcolor)
                    e.Row.Cells(GV_DCF_Pending).BackColor = Drawing.Color.RoyalBlue
                    e.Row.Cells(GV_DCF_Generated).BackColor = Drawing.Color.RoyalBlue
                    e.Row.Cells(GV_DCF_Answered).BackColor = Drawing.Color.RoyalBlue

                    e.Row.Cells(GV_DCF_Answered).ForeColor = Drawing.Color.White

                    e.Row.Cells(0).Text = "Total: " + total.ToString
                    e.Row.Cells(GV_SubjectNo).Text = SubjectNo.ToString()
                    e.Row.Cells(GV_DEP).Text = DEP.ToString()
                    e.Row.Cells(GV_DEC).Text = DEC.ToString()
                    e.Row.Cells(GV_FRP).Text = FRP.ToString()
                    e.Row.Cells(GV_SRP).Text = SRP.ToString()
                    e.Row.Cells(GV_FnlRP).Text = FnlRP.ToString()
                    e.Row.Cells(GV_Locked).Text = Locked.ToString()
                    e.Row.Cells(GV_DCF_Pending).Text = DCF.ToString()
                    e.Row.Cells(GV_DCF_Generated).Text = DCF_Generated.ToString()
                    e.Row.Cells(GV_DCF_Answered).Text = DCF_Answered.ToString()


                    e.Row.Cells(0).CssClass = "GridForeColor"
                    e.Row.Cells(GV_SubjectNo).CssClass = "GridForeColor"
                    e.Row.Cells(GV_DEP).CssClass = "GridForeColor"
                    e.Row.Cells(GV_DEC).CssClass = "GridForeColor"
                    e.Row.Cells(GV_FRP).CssClass = "GridForeColor"
                    e.Row.Cells(GV_SRP).CssClass = "GridForeColor"
                    e.Row.Cells(GV_FnlRP).CssClass = "GridForeColor"
                    e.Row.Cells(GV_Locked).CssClass = "GridForeColor"
                    e.Row.Cells(GV_DCF_Pending).CssClass = "GridForeColor"
                    e.Row.Cells(GV_DCF_Generated).CssClass = "GridForeColor"
                    e.Row.Cells(GV_DCF_Answered).CssClass = "GridForeColor"


                    If iworkflowfinal = 10 Then
                        e.Row.Cells(GV_Locked).BackColor = ColorTranslator.FromHtml(srpcolor)
                        e.Row.Cells(GV_SRP).Visible = False
                        e.Row.Cells(GV_FnlRP).Visible = False
                        e.Row.Cells(GV_Locked).Visible = True
                    ElseIf iworkflowfinal = 20 Then
                        e.Row.Cells(GV_SRP).BackColor = ColorTranslator.FromHtml(srpcolor)
                        e.Row.Cells(GV_Locked).BackColor = ColorTranslator.FromHtml(fnlrpcolor)
                        e.Row.Cells(GV_FnlRP).Visible = False
                        e.Row.Cells(GV_Locked).Visible = True
                        e.Row.Cells(GV_SRP).Visible = True
                    ElseIf iworkflowfinal = 30 Then
                        e.Row.Cells(GV_SRP).BackColor = ColorTranslator.FromHtml(srpcolor)
                        e.Row.Cells(GV_FnlRP).BackColor = ColorTranslator.FromHtml(fnlrpcolor)
                        e.Row.Cells(GV_Locked).BackColor = ColorTranslator.FromHtml(lcolor)
                        e.Row.Cells(GV_Locked).Visible = True
                        e.Row.Cells(GV_FnlRP).Visible = True
                        e.Row.Cells(GV_SRP).Visible = True
                    End If
                    e.Row.Cells(0).ForeColor = Drawing.Color.White
                    e.Row.Cells(GV_SubjectNo).ForeColor = Drawing.Color.White
                    e.Row.Cells(GV_DEP).ForeColor = Drawing.Color.White
                    e.Row.Cells(GV_DEC).ForeColor = Drawing.Color.White
                    e.Row.Cells(GV_FRP).ForeColor = Drawing.Color.White
                    e.Row.Cells(GV_SRP).ForeColor = Drawing.Color.White
                    e.Row.Cells(GV_FnlRP).ForeColor = Drawing.Color.White
                    e.Row.Cells(GV_Locked).ForeColor = Drawing.Color.White
                    e.Row.Cells(GV_DCF_Pending).ForeColor = Drawing.Color.White
                    e.Row.Cells(GV_DCF_Generated).ForeColor = Drawing.Color.White
                    e.Row.Cells(GV_DCF_Answered).ForeColor = Drawing.Color.White

            End Select

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
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

        If Me.ddlPeriods.SelectedValue <> "All" Then
            period = Me.ddlPeriods.SelectedValue
        End If

        Try
            whrCon = " vWorkspaceId='" + Me.HProjectId.Value + "'" _
                    + " and  iPeriod=" + CInt(period).ToString + " Order by iMySubjectNo"
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
                            ElseIf dtSubject.Rows(index).Item("cScreenFailure").ToString() = "Y" Then
                                nodeSubject.Text = "<font color = red>" + dtSubject.Rows(index).Item("vMySubjectNo").ToString() + "</font>"
                            ElseIf dtSubject.Rows(index).Item("cDisContinue").ToString() = "Y" Then
                                nodeSubject.Text = "<font color = Purple>" + dtSubject.Rows(index).Item("vMySubjectNo").ToString() + "</font>"
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
                        Next
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

            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
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
        Dim dsMaster As DataSet = New DataSet()
        Dim cDataStatus As String = String.Empty
        Dim iWorkflowStageId As String = String.Empty
        Dim cSubjectWiseFlag As String = String.Empty
        Dim iPeriod As String = String.Empty
        Dim iProjectType As String = 0
        Dim ds_reviewer As New DataSet
        Dim dv As DataView
        Dim wStr As String
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

            If chkUnScheduled.Checked = True Then
                objHelp.Timeout = -1
                wStr = Me.HProjectId.Value + "##" + iPeriod + "##" + strSubjectId + "##" + strParentID + "##" + strActivityId + "##" + cSubjectWiseFlag + "##" + iProjectType + "##" + "" + "##" + "" + "##"
                dsMaster = objHelp.ProcedureExecute("dbo.Proc_GetVisitUnSchedulerreport", wStr)
            Else
                If Not objHelp.Proc_GetVisitSchedulerreport(Me.HProjectId.Value, iPeriod, strSubjectId, strParentID, strActivityId, cSubjectWiseFlag, iProjectType, cDataStatus, iWorkflowStageId, dsMaster, eStr) Then
                    Me.objcommon.ShowAlert("Error in Getting Data", Me)
                    dsMaster = New DataSet()
                    Exit Function
                End If
            End If
            If dsMaster Is Nothing Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "nodate", "alert('No data found');", True)
                Return True
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

#Region "Radio Button Event"
    Protected Sub rbtnType_SelectedIndexChanged(sender As Object, e As EventArgs)
        If rbtnType.SelectedIndex = 0 Then
            divSchedulerRrepot.Visible = True
            divSummeryRepot.Visible = False
            grdParent.DataSource = Nothing
            grdParent.DataBind()
            txtproject.Text = String.Empty
            ddlPeriods.Items.Clear()
            ddlFilter.Items.Clear()
            tvSubject.DataSource = Nothing
            tvSubject.DataBind()
            Me.divSubject.Style.Add("display", "none")
            Me.divActivity.Style.Add("display", "none")
            upcontrols.Update()
        Else
            divSchedulerRrepot.Visible = False
            divSummeryRepot.Visible = True
            gvActivityCount.DataSource = Nothing
            gvActivityCount.DataBind()
            txtSummeryProject.Text = String.Empty
            ddlSummeryPeriod.Items.Clear()
            upcontrols.Update()
            Me.fldgrdParent.Style.Add("display", "none")
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

#Region "Export to Excel"

    Protected Sub btnExportAll_Click(sender As Object, e As EventArgs) Handles btnExportAll.Click
        Dim fileName As String = String.Empty
        Dim Str As String = String.Empty
        Dim isReportComplete As Boolean = False
        Dim RowCnt As Integer
        Dim rRow As RepoRow
        Dim dt_Report As New DataTable
        Dim PreviousCompany As String = ""
        Dim PreviousProject As String = ""
        Dim ds_reviewer As New DataSet
        Dim dv As DataView

        Dim eStr As String = ""
        Dim strParentID As String = String.Empty
        Dim strActivityId As String = String.Empty
        Dim cSubjectWiseFlag As String = String.Empty
        Dim cDataStatus As String = String.Empty
        Dim iWorkflowStageId As String = String.Empty
        Dim strSubjectId As String = String.Empty
        Dim chCnt As Integer = 0
        Dim iPeriod As String = String.Empty
        Dim dv_reviewer As DataView
        Dim ds_Report As DataSet
        Try
            Dim a As New ArrayList
            a.Add("sad")

            fileName = CType(Master.FindControl("lblHeading"), Label).Text + "_" + Me.txtproject.Text.Split(" ")(0) + ".xls"
            fileName = Server.MapPath("~/ExcelReportFile/" + fileName)

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
                        Next 'Next Index
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

                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                    If chkUnScheduled.Checked = True Then
                        objHelp.Timeout = -1
                        Dim wStr = Me.HProjectId.Value + "##" + iPeriod + "##" + "" + "##" + "" + "##" + "" + "##" + "Y" + "##" + "" + "##" + "" + "##"
                        ds_Report = objHelp.ProcedureExecute("dbo.Proc_getVisitUnSchedulerReport_ForCTDataExport", wStr)

                    Else
                        If Not objHelp.Proc_getVisitSchedulerReport_ForCTDataExport(Me.HProjectId.Value, iPeriod, strSubjectId, strParentID, strActivityId, cSubjectWiseFlag, cDataStatus, iWorkflowStageId, ds_Report, eStr) Then
                            Throw New Exception(eStr)
                        End If
                    End If
                Else
                End If

                If Not ds_Report Is Nothing Then
                    dt_Report = ds_Report.Tables(0)
                    grdExportToExcel.DataSource = dt_Report
                    grdExportToExcel.DataBind()
                End If


                If grdExportToExcel.Rows.Count > 0 Then
                    Dim info As String = String.Empty
                    Dim gridviewHtml As String = String.Empty

                    fileName = "Visit Scheduler Report" + ".xls"

                    isReportComplete = True
                    Dim stringWriter As New System.IO.StringWriter()
                    Dim writer As New HtmlTextWriter(stringWriter)
                    grdExportToExcel.RenderControl(writer)
                    gridviewHtml = stringWriter.ToString()

                    gridviewHtml = "<table><tr><td align = ""center"" colspan=""7""><b>" + System.Configuration.ConfigurationManager.AppSettings("Client").ToString() + "<br/></b></td></tr></table><table><tr><td align = ""center"" colspan=""2""><b>Visit Scheduled Report :<br/></b></td></tr></table><span>Print Date:-  " + Date.Today.ToString("dd-MMM-yyyy") + "</span>" + gridviewHtml

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
                End If



            Catch ex As Exception
                Me.ShowErrorMessage(ex.Message + "Error While Print Detail", "")
            End Try

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

    End Sub

#End Region

#Region "Report Helper Functions"

    Private Function OpenReport(ByVal ReportFileName_1 As String) As Boolean
        If File.Exists(ReportFileName_1) = True Then
            File.Delete(ReportFileName_1)
        End If
        rPage = New RepoPage(ReportFileName_1)
    End Function

#End Region

#Region "Dynamic review"

    Private Function GetLegends() As Boolean
        Dim ds As New DataSet
        Dim dv As DataView
        Dim estr As String = ""

        Try
            If Not Me.objHelp.Proc_GetLegends(IIf(rbtnType.SelectedIndex = 0, Me.HProjectId.Value.ToString(), hdnSummeryProjectId.Value.ToString()), ds, estr) Then
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
                Me.PhlReviewer.Controls.Add(New LiteralControl("-Data Entry Countinue, "))
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

    Protected Sub hdnbutton_Click(sender As Object, e As EventArgs)

    End Sub

    
    Protected Sub btnExport_Click(sender As Object, e As EventArgs)
        Dim filename As String
        If gvActivityCount.Rows.Count > 0 Then
            Dim info As String = String.Empty
            Dim gridviewHtml As String = String.Empty

            filename = "VisitScheduler Summery Report" + ".xls"

            Dim stringWriter As New System.IO.StringWriter()
            Dim writer As New HtmlTextWriter(stringWriter)
            gvActivityCount.RenderControl(writer)
            gridviewHtml = stringWriter.ToString()

            gridviewHtml = "<table><tr><td align = ""center"" colspan=""7""><b>" + System.Configuration.ConfigurationManager.AppSettings("Client").ToString() + "<br/></b></td></tr></table><table><tr><td align = ""center"" colspan=""2""><b>" + " Visit Scheduler Summery Report : " + "<br/></b></td></tr></table><span>Print Date:-  " + Date.Today.ToString("dd-MMM-yyyy") + "</span>" + gridviewHtml

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
        End If

    End Sub

    Protected Sub btnSummeryCancel_Click(sender As Object, e As EventArgs)
        Me.txtSummeryProject.Text = ""
        Me.hdnSummeryProjectId.Value = ""
        ddlSummeryPeriod.Items.Clear()
        gvActivityCount.DataSource = Nothing
        gvActivityCount.DataBind()
    End Sub
End Class
