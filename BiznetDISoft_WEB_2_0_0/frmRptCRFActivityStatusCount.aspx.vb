Imports System.Drawing

Partial Class frmRptCRFActivityStatusCount
    Inherits System.Web.UI.Page
#Region "Variable Declaration"
    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()

    Private total As Integer = 0
    Private SubjectNo As Integer = 0
    Private DEP As Integer = 0
    Private DEC As Integer = 0
    Private FRP As Integer = 0
    Private SRP As Integer = 0
    Private FnlRP As Integer = 0
    Private Locked As Integer = 0
    Private DCF As Integer = 0
    Private DCF_Generated As Integer = 0
    Private DCF_Answered As Integer = 0

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

    Private Const Vs_ActivityCount As String = "Activity Count"
    Private Const Vs_dsReviewerlevel As String = "dsActivityReviewerlevel"
#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not GenCall() Then
                Exit Sub
            End If
        End If
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

            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
            Return False
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
            Page.Title = " :: CRF Activity Status Summary ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")

            CType(Master.FindControl("lblHeading"), Label).Text = "CRF Activity Status Summary"

            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
            End If
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)

            GenCall_ShowUI = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

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

            If Not ds_Periods.Tables(0).Rows(0)("iNoOfPeriods") Is Nothing Then

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
        End Try

    End Function

#End Region

#Region "GridView Event"

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
                        'e.Row.Cells(GV_SRP).BackColor = ColorTranslator.FromHtml("#800080")
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
                    'e.Row.Cells(GV_SRP).ForeColor = ColorTranslator.FromHtml("#50C000")
                    'e.Row.Cells(GV_FnlRP).ForeColor = ColorTranslator.FromHtml("#006000")
                    'e.Row.Cells(GV_Locked).ForeColor = Drawing.Color.Gray
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
                    'e.Row.Cells(GV_SRP).BackColor = ColorTranslator.FromHtml("#50C000")
                    'e.Row.Cells(GV_FnlRP).BackColor = ColorTranslator.FromHtml("#006000")
                    'e.Row.Cells(GV_Locked).BackColor = Drawing.Color.Gray
                    e.Row.Cells(GV_DCF_Pending).BackColor = Drawing.Color.RoyalBlue
                    e.Row.Cells(GV_DCF_Generated).BackColor = Drawing.Color.RoyalBlue
                    e.Row.Cells(GV_DCF_Answered).BackColor = Drawing.Color.RoyalBlue

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

                    e.Row.Cells(0).CssClass = "FooterColor"
                    e.Row.Cells(GV_SubjectNo).CssClass = "FooterColor"
                    e.Row.Cells(GV_DEP).CssClass = "FooterColor"
                    e.Row.Cells(GV_DEC).CssClass = "FooterColor"
                    e.Row.Cells(GV_FRP).CssClass = "FooterColor"
                    e.Row.Cells(GV_SRP).CssClass = "FooterColor"
                    e.Row.Cells(GV_FnlRP).CssClass = "FooterColor"
                    e.Row.Cells(GV_Locked).CssClass = "FooterColor"
                    e.Row.Cells(GV_DCF_Pending).CssClass = "FooterColor"
                    e.Row.Cells(GV_DCF_Generated).CssClass = "FooterColor"
                    e.Row.Cells(GV_DCF_Answered).CssClass = "FooterColor"


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
                        'e.Row.Cells(GV_SRP).BackColor = ColorTranslator.FromHtml("#800080")
                        'e.Row.Cells(GV_FnlRP).BackColor = ColorTranslator.FromHtml("#006000")
                        'e.Row.Cells(GV_Locked).BackColor = Drawing.Color.Gray
                        e.Row.Cells(GV_SRP).BackColor = ColorTranslator.FromHtml(srpcolor)
                        e.Row.Cells(GV_FnlRP).BackColor = ColorTranslator.FromHtml(fnlrpcolor)
                        e.Row.Cells(GV_Locked).BackColor = ColorTranslator.FromHtml(lcolor)
                        e.Row.Cells(GV_Locked).Visible = True
                        e.Row.Cells(GV_FnlRP).Visible = True
                        e.Row.Cells(GV_SRP).Visible = True
                    End If

            End Select

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        End Try
    End Sub


#End Region

#Region "Button Events"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim eStr As String = String.Empty
        Dim dsWorkSpace As New DataSet
        Try
            If Not objHelp.getworkspacemst(" vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsWorkSpace, eStr) Then
                Throw New Exception(eStr)
            End If

            If dsWorkSpace Is Nothing OrElse dsWorkSpace.Tables(0).Rows.Count = 0 Then
                Throw New Exception("No Records Available In WorkSpaceMst.")
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

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")

        End Try
    End Sub

    Protected Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
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

            iPeriod = "," + Me.ddlPeriods.SelectedValue.ToString() + ","
            If Me.ddlPeriods.SelectedValue = "All" Then
                iPeriod = ""
            End If
            objHelp.Timeout = -1
            If Not objHelp.Proc_GetActivityStatusCountRecords(Me.HProjectId.Value, iPeriod, IIf(Me.chkParent.Checked = True, "Y", "N"), IIf(Me.Session(S_ScopeNo) = Scope_ClinicalTrial, 2, 1), IIf(Me.chkParent.Style("display") = "none", "Y", "N"), dsCount, eStr) Then
                Me.objcommon.ShowAlert("Error in Getting CRF Activity Status Count Records", Me)
                Exit Sub
            End If

            If dsCount.Tables(0).Rows.Count = 0 Then
                Me.objcommon.ShowAlert("No Records Available For Selected Project.", Me)
                Exit Sub
            Else
                Me.gvActivityCount.DataSource = dsCount
                Me.gvActivityCount.DataBind()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ChangeColor", "ChangeColor();", True)
                Me.btnExport.Style("display") = ""
                ViewState(Vs_ActivityCount) = dsCount.Tables(0)
            End If



        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("frmRptCRFActivityStatusCount.aspx")
    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Dim fileName As String = String.Empty
        Dim Str As String = String.Empty

        Try

            fileName = CType(Master.FindControl("lblHeading"), Label).Text + "_" + Me.txtproject.Text.Split(" ")(0) + ".xls"

            Dim stringWriter As New System.IO.StringWriter()
            Dim writer As New HtmlTextWriter(stringWriter)

            Str = "<div><table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0""><tr><td colspan=""11""><center><strong><font color=""black"" size=""9px"" face=""Calibri"">" + System.Configuration.ConfigurationManager.AppSettings("Client") + "</font></strong></td></tr><tr><td colspan=""11""><center><strong><font color=""black"" size=""8px"" face=""Calibri"">" + CType(Master.FindControl("lblHeading"), Label).Text + "</font></strong></td></tr><tr><td><strong><font color=""black"" size=""9px"" face=""Calibri"">Project/Site</font></strong></td><td colspan=""10""><strong><font color=""black"" size=""9px"" face=""Calibri"">" + Me.txtproject.Text.Trim() + "</font></strong></td></tr><tr><td colspan=""11""></td></tr></table></div>"

            Me.gvActivityCount.RenderControl(writer)
            Dim gridViewhtml As String = stringWriter.ToString()

            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)
            Context.Response.Write(Str + gridViewhtml.Substring(5).Substring(0, gridViewhtml.Substring(5).Length - 6))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)
        Catch ex As Exception

        End Try
    End Sub

#End Region
#Region "Dynamic review"

    Private Function GetLegends() As Boolean
        Dim ds As New DataSet
        Dim estr As String = ""

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

#End Region

End Class
