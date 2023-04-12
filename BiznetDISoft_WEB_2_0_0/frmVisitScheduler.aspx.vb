Imports System.Web.Services
Imports Newtonsoft.Json
Imports System.Windows.Forms
Imports Microsoft.Office.Interop.Excel

'Imports Microsoft.Office.Interop.Excel
'Imports Microsoft.Office
'Imports System.Windows.Forms 



Partial Class frmVisitScheduler
    Inherits System.Web.UI.Page

#Region "Parameter Declare"
    Private objCommon As New clsCommon
    Private objhelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
    Private VS_Scheduler As String = "VisitScheduler"
    Private VS_SchedulerTracker As String = "VisitSchedulerTracker"
    Private VS_Actual As String = "VisitActual"

    Private isDataEntered As Boolean
    Private ActualDateVisible As Boolean = True
    Private ActualDateVisible1 As Boolean = True
    Private Const GVC_ProjectNo As Integer = 0
    Private Const GVC_Randomization As Integer = 1
    Private Const GVC_SubjectNo As Integer = 2
    Private Const GVC_vMySubjectNo As Integer = 3
    Private Const GVC_iMySubjectNo As Integer = 4
    Private Const GVC_cScreenFailure As Integer = 5
    Private Const GVC_cDisContinue As Integer = 6
    Private Const GVC_FirStVisit As Integer = 7

    Private ActId As New ArrayList
    Private NodeId As New ArrayList


#End Region

#Region "Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Not IsPostBack Then
            If Not GenCall() Then
            End If
        End If

    End Sub

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Try
            If Not GenCall_Data() Then
                GenCall = False
            End If
            If Not GenCall_ShowUI() Then
                GenCall = False
            End If
            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall")
            GenCall = False
        End Try
    End Function

#End Region

#Region "GenCall Data"

    Private Function GenCall_Data() As Boolean
        Dim ds_Scheduled As DataSet
        Try
            ds_Scheduled = Me.objhelp.GetResultSet("select *  from [VisitScheduler] where 1=2 ", "VisitScheduler")
            ViewState(VS_SchedulerTracker) = ds_Scheduled
            lblModifyby.Text = Session(S_UserID)
            GenCall_Data = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_Data")
            GenCall_Data = False
        End Try
    End Function

#End Region

#Region "GenCall_ShowUI"

    Private Function GenCall_ShowUI() As Boolean
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim sender As New Object
        Dim e As EventArgs
        Try
            'System.Web.UI.Page.Title = New String(":: Visit Scheduler  :: " + System.Configuration.ConfigurationManager.AppSettings("Client"))
            CType(Me.Master.FindControl("lblHeading"), System.Web.UI.WebControls.Label).Text = "Visit  Scheduler"
            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.lblSiteNo.Text = Session(S_ProjectName)
                'Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
            End If

            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
            Me.AutoCompleteExtender1.ServiceMethod = "GetMyProjectCompletionList"
            GenCall_ShowUI = True

            If Not Me.Request.QueryString("WorkSpaceId") Is Nothing Then
                Me.txtproject.Text = Me.Request.QueryString("ProjectName").ToString()
                Me.txtproject.Enabled = False
                Me.HProjectId.Value = Me.Request.QueryString("WorkSpaceId").ToString()
                Me.btnCancel.Visible = False
                btnSetProject_Click(sender, e)
                Exit Function
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...............GenCall_ShowUI")
            GenCall_ShowUI = False
        End Try
    End Function

#End Region

#Region "Button Event"
    Protected Sub btnSetProject_Click(sender As Object, e As EventArgs) Handles btnSetProject.Click
        Dim estr As String = String.Empty
        Dim ds_ScheduledActivity As DataSet
        Dim ds_ActualActivity As DataSet = Nothing
        Dim ds_DisSubject As DataSet = Nothing
        Dim wStr As String = String.Empty

        isDataEntered = True
        ds_ScheduledActivity = objhelp.ProcedureExecute("dbo.PROC_VisitScheduler", HProjectId.Value)
        ds_ActualActivity = objhelp.ProcedureExecute("dbo.PROC_VisitActual", HProjectId.Value)
        ViewState(VS_Actual) = ds_ActualActivity
        ViewState(VS_Scheduler) = ds_ScheduledActivity

        Try
            btnExporttoExcel.Visible = True
            If Not ds_ScheduledActivity Is Nothing AndAlso ds_ScheduledActivity.Tables(0).Rows.Count < 0 Then
                ds_ScheduledActivity = objhelp.ProcedureExecute("dbo.Proc_GETScheduledActivity", HProjectId.Value)
                isDataEntered = False
            Else
                If ds_ScheduledActivity.Tables(0).Rows.Count = 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "nodate", "msgalert('No data found');", True)
                    btnExporttoExcel.Visible = False
                End If

                ViewState(VS_Scheduler) = ds_ScheduledActivity
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "nodate", "msgalert('No data found');", True)
            gvScheduler.DataSource = Nothing
            gvScheduler.DataBind()
            upScheduler.Update()
            Exit Sub
        End Try

        If Not ds_ScheduledActivity Is Nothing Then
            gvScheduler.DataSource = Nothing
            gvScheduler.DataBind()
            upScheduler.Update()
            gvScheduler.DataSource = ds_ScheduledActivity.Tables(0)
            gvScheduler.DataBind()
            upScheduler.Update()
        End If

    End Sub

    Protected Sub btnSchedule_Click(sender As Object, e As EventArgs) Handles btnSchedule.Click
        ''trDiscountinut.Attributes.Add("style", "display:none")
        ''trScreenFailure.Attributes.Add("style", "display:none")
        upModel.Update()
        upScheduler.Update()
        ModalScheduler.Show()
        lblHeader.Text = "Scheduled Visit Information"
        lblSche.Text = "Scheduled Date :"
        btnActualSave.Visible = False
        btnSave.Visible = True
        txtScheduledDate.Text = ""
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "assignData", "AssignData();", True)
        btnSetProject_Click(Nothing, Nothing)
    End Sub

    Protected Sub btnVisit_Click(sender As Object, e As EventArgs) Handles btnVisit.Click
        ''rbtDiscountinue.ClearSelection()
        ''rbtScreen.ClearSelection()
        lblHeader.Text = "Actual Visit Information"
        lblSche.Text = "Actual DOV :"
        btnActualSave.Visible = True
        btnSave.Visible = False
        txtScheduledDate.Text = ""
        ''trDiscountinut.Attributes.Add("style", "display:''")
        ''trScreenFailure.Attributes.Add("style", "display:''")
        upModel.Update()

        upScheduler.Update()
        ModalScheduler.Show()
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "assignDataActual", "AssignData();", True)
        btnSetProject_Click(Nothing, Nothing)
    End Sub

    Protected Sub btnRefreshGrid_Click(sender As Object, e As EventArgs) Handles btnRefreshGrid.Click
        btnSetProject_Click(Nothing, Nothing)
    End Sub
    Protected Sub btnSave_Click(sender As Object, e As EventArgs)
        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SaveScheduler", "SaveScheduler();", True)
        'btnSetProject_Click(Nothing, Nothing)

        'Dim ds_Scheduled As DataSet = CType(ViewState(VS_SchedulerTracker), DataSet)
        'Dim dr As DataRow
        'Dim eStr As String

        'upModel.Update()
        'upScheduler.Update()
        'dr = ds_Scheduled.Tables(0).NewRow()
        'dr("vWorkSpaceId") = HProjectId.Value
        'dr("vRandomisationNo") = Convert.ToString(txtRandomisation.Text)
        'dr("vVisitName") = Convert.ToString(txtVisit.Text)
        'dr("iVisitNo") = Convert.ToInt64(lblVisit.Text.Replace("Visit", ""))
        'dr("dScheduledDate") = txtScheduledDate.Text
        'dr("iModifyby") = Session(S_UserID)
        'ds_Scheduled.Tables(0).Rows.Add(dr)
        'ds_Scheduled.AcceptChanges()

        'If Not objLambda.Save_Scheduler(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Scheduled, Session(S_UserID), eStr) Then
        '    objCommon.ShowAlert("Error While Saving VisitScheduler", Me.Page)
        '    Exit Sub
        'Else
        '    objCommon.ShowAlert("Records Save Successfully", Me.Page)
        'End If




    End Sub

    Protected Sub btnActualSave_Click(sender As Object, e As EventArgs)
        Dim ds_Scheduled As DataSet = CType(ViewState(VS_Scheduler), DataSet)
        Dim dr As DataRow
        upModel.Update()
        upScheduler.Update()
        dr = ds_Scheduled.Tables(0).NewRow()
        dr("vWorkSpaceId") = HProjectId.Value
        dr("vRandomisationNo") = txtRandomisation.Text
        dr("vVisitName") = txtVisit.Text
        dr("iVisitNo") = Convert.ToInt64(lblVisit.Text.Replace("Visit", ""))
        dr("dScheduledDate") = txtScheduledDate.Text
        dr("iModifyby") = Session(S_UserID)
        ds_Scheduled.Tables(0).Rows.Add(dr)
        ds_Scheduled.AcceptChanges()
    End Sub

    Protected Sub btnExporttoExcel_Click(sender As Object, e As EventArgs)
        Dim fileName As String = String.Empty

        Dim ds_ExportExcel As DataSet = ViewState("ds_ExportExcel")

        For i = 0 To ds_ExportExcel.Tables(0).Rows.Count - 1 Step 1
            For j = 0 To ds_ExportExcel.Tables(0).Columns.Count - 1 Step 1
                ds_ExportExcel.Tables(0).Rows(i)(j) = Convert.ToString(ds_ExportExcel.Tables(0).Rows(i)(j)).Replace("</br>", "")
            Next

        Next
        ds_ExportExcel.AcceptChanges()
        gvSchedulerExport.DataSource = ds_ExportExcel
        gvSchedulerExport.DataBind()


        If gvSchedulerExport.Rows.Count > 0 Then
            Dim info As String = String.Empty
            Dim gridviewHtml As String = String.Empty

            fileName = "Visit Scheduler" + ".xls"

            Dim stringWriter As New System.IO.StringWriter()
            Dim writer As New HtmlTextWriter(stringWriter)
            upScheduler.Update()

            gvSchedulerExport.RenderControl(writer)
            gridviewHtml = stringWriter.ToString()

            gridviewHtml = "<table><tr><td align = ""center"" colspan=""7""><b>" + System.Configuration.ConfigurationManager.AppSettings("Client").ToString() + "<br/></b></td></tr></table><table><tr><td align = ""center"" colspan=""2""><b>Visit Scheduled:<br/></b></td></tr></table><span>Print Date:-  " + Date.Today.ToString("dd-MMM-yyyy") + "</span>" + gridviewHtml

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


    End Sub
#End Region

#Region "Grid Event"

    Protected Sub gvScheduler_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvScheduler.RowDataBound

        Dim HdrStr As String = String.Empty
        Dim strHeader As String = String.Empty
        Dim ds_Scheduler As DataSet
        Dim ds_ExportExcel As DataSet = CType(ViewState(VS_Scheduler), DataSet)

        ActualDateVisible1 = True
        If e.Row.RowType = DataControlRowType.Header Then

            For HdrIndex As Integer = 0 To e.Row.Cells.Count - 1

                ActId.Add("0")
                NodeId.Add("0")

                If e.Row.Cells(HdrIndex).Text.Contains("#") Then
                    HdrStr = Context.Server.HtmlDecode(e.Row.Cells(HdrIndex).Text.Trim())
                    ActId.Insert(HdrIndex, HdrStr.Substring(HdrStr.IndexOf("#") + 1, HdrStr.LastIndexOf("#") - (HdrStr.IndexOf("#") + 1)))
                    NodeId.Insert(HdrIndex, HdrStr.Substring(HdrStr.LastIndexOf("#") + 1).Trim())
                    strHeader = HdrStr.Substring(0, HdrStr.IndexOf("#"))
                    e.Row.Cells(HdrIndex).Attributes.Add("title", strHeader)

                    e.Row.Cells(HdrIndex).Text = strHeader
                    'If strHeader.Length > 10 Then
                    '    e.Row.Cells(HdrIndex).Text = strHeader.Split(" ")(0) + "..."
                    'Else
                    '    e.Row.Cells(HdrIndex).Text = strHeader + "..."
                    'End If
                End If
            Next HdrIndex
        End If




        Dim ds_Schedule As DataSet = CType(ViewState(VS_Scheduler), DataSet)
        Dim ds_Actual As DataSet = CType(ViewState(VS_Actual), DataSet)


        If e.Row.RowType = DataControlRowType.DataRow Then
            If isDataEntered = False Then
                lblSiteNo.Text = Convert.ToString(e.Row.Cells(0).Text)
                For cIndex As Integer = 1 To e.Row.Cells.Count - 1
                    Dim txt As Image = New Image()
                    Dim txt1 As Image = New Image()
                    txt.ImageUrl = ("/Images/ScheduledDate.png").ToString()
                    txt1.ImageUrl = ("/Images/ActualDate.png").ToString()

                    'txt.Attributes("../Images/ScheduledDate.png")
                    'txt.sr("../Images/ScheduledDate.png")
                    'txt.ImageUrl("../Images/ActualDate.png")


                    'Dim txt As LinkButton = New LinkButton()
                    'Dim txt1 As LinkButton = New LinkButton()
                    Dim HesderText = Server.HtmlDecode(gvScheduler.HeaderRow.Cells(cIndex).Text.ToString())
                    Dim id As String = "Scheduled_" + HesderText.Trim() + "_" + e.Row.Cells(1).Text
                    Dim id1 As String = "Actual_" + HesderText.Trim() + "_" + e.Row.Cells(1).Text
                    txt.ID = id
                    txt1.ID = id1
                    txt.CssClass = "Scheduled-" + HesderText
                    txt1.CssClass = "Actual-" + HesderText
                    'txt.Text = "ScheduledDate " + "</br>"
                    'txt1.Text = "</br> ActualDate"
                    e.Row.Cells(cIndex).Controls.Add(txt)
                    e.Row.Cells(cIndex).Controls.Add(txt1)

                    If cIndex <> GVC_FirStVisit Then
                        e.Row.Cells(cIndex).Enabled = False
                    Else

                        txt.Attributes.Add("onClick", "return OpenMOdelPopup(this.id)")
                        txt1.Attributes.Add("onClick", " return OpenMOdelPopup(this.id)")
                        txt.Font.Underline = True
                        txt1.Font.Underline = True

                    End If
                    CType(e.Row.FindControl(id), LinkButton).CommandArgument = e.Row.RowIndex
                    CType(e.Row.FindControl(id), LinkButton).CommandName = "Scheduler"
                    CType(e.Row.FindControl(id1), LinkButton).CommandArgument = e.Row.RowIndex
                    CType(e.Row.FindControl(id1), LinkButton).CommandName = "Actual"

                Next
            Else

                'For i As Integer = 0 To ds_DisSubject.Tables(0).Rows.Count - 1
                For cIndex As Integer = GVC_FirStVisit To e.Row.Cells.Count - 1
                    lblSiteNo.Text = Convert.ToString(e.Row.Cells(0).Text)
                    Dim txt As Image = New Image()
                    Dim txt1 As Image = New Image()
                    txt.ImageUrl = ("~/Images/ScheduledDate.png")
                    txt1.ImageUrl = ("~/Images/ActualDate.png")

                    'Dim txt As LinkButton = New LinkButton()
                    'Dim txt1 As LinkButton = New LinkButton()
                    Dim img As Image = New Image()

                    Dim HesderText = Server.HtmlDecode(gvScheduler.HeaderRow.Cells(cIndex).Text.ToString())
                    Dim id As String = "Scheduled_" + HesderText.Trim() + "_" + e.Row.Cells(1).Text + "_" + e.Row.Cells(2).Text + "_" + ActId(cIndex)
                    Dim id1 As String = "Actual_" + HesderText.Trim() + "_" + e.Row.Cells(1).Text + "_" + e.Row.Cells(2).Text + "_" + ActId(cIndex)
                    txt1.ID = id1
                    txt.ID = id
                    img.ID = id
                    img.ImageUrl = "images/audit.png"
                    txt1.CssClass = "Actual-" + HesderText
                    txt.CssClass = "Scheduled-" + HesderText
                    txt.Attributes.Add("width", "20px")
                    txt1.Attributes.Add("width", "20px")
                    txt.ToolTip = "Scheduled Date"
                    txt1.ToolTip = "Actual Date"
                    'txt.Text = "ScheduledDate "
                    'txt1.Text = "</br> ActualDate"
                    Dim lblSchedule As System.Web.UI.WebControls.Label = New System.Web.UI.WebControls.Label()
                    Dim lblActual As System.Web.UI.WebControls.Label = New System.Web.UI.WebControls.Label()
                    lblSchedule.Text = Convert.ToString(ds_Schedule.Tables(0).Rows(e.Row.RowIndex)(cIndex))
                    lblSchedule.ToolTip = "Scheduled Date"
                    lblActual.ToolTip = "Actual Date"
                    lblActual.Text = Convert.ToString(ds_Actual.Tables(0).Rows(e.Row.RowIndex)(cIndex))
                    'Dim DisSubjectId As String = Convert.ToString(ds_DisSubject.Tables(0).Rows(1)(i))
                    'Dim disSubject As String = Convert.ToString(ds_DisSubject.Tables(0).Rows(e.Row.RowIndex)(i))

                    If lblSchedule.Text = "" Then
                        e.Row.Cells(cIndex).Controls.Add(txt)
                    Else
                        e.Row.Cells(cIndex).Controls.Add(lblSchedule)
                        ds_ExportExcel.Tables(0).Rows(e.Row.RowIndex)(cIndex) = Convert.ToString(("Sch: " + Convert.ToString(lblSchedule.Text)))

                    End If

                    If lblActual.Text = "" Then
                        ActualDateVisible = False
                        e.Row.Cells(cIndex).Controls.Add(txt1)
                    Else
                        lblActual.Text = "</br>" + Convert.ToString(ds_Actual.Tables(0).Rows(e.Row.RowIndex)(cIndex))
                        e.Row.Cells(cIndex).Controls.Add(lblActual)
                        ds_ExportExcel.Tables(0).Rows(e.Row.RowIndex)(cIndex) = Convert.ToString(("Sch: " + Convert.ToString(lblSchedule.Text) + " Ach: " + Convert.ToString(lblActual.Text)))
                        ds_ExportExcel.AcceptChanges()
                    End If


                    If cIndex <> GVC_FirStVisit Then
                        If ActualDateVisible = False And ActualDateVisible1 = True AndAlso Not (ds_Schedule.Tables(0).Rows(e.Row.RowIndex)(GVC_cDisContinue) = "Y" Or ds_Schedule.Tables(0).Rows(e.Row.RowIndex)(GVC_cScreenFailure) = "Y") Then
                            txt1.Enabled = True
                            ActualDateVisible = True
                            ActualDateVisible1 = False
                            txt.Enabled = False
                            txt1.Attributes.Add("onClick", " return OpenMOdelPopup(this.id)")
                            txt1.Font.Underline = True

                        Else
                            If Not (ds_Schedule.Tables(0).Rows(e.Row.RowIndex)(GVC_cDisContinue) = "Y" Or ds_Schedule.Tables(0).Rows(e.Row.RowIndex)(GVC_cScreenFailure) = "Y") Then
                                txt1.Enabled = False
                                txt.Enabled = False
                            Else
                                e.Row.Cells(cIndex).Enabled = False
                                txt.Enabled = False
                            End If

                        End If

                    Else
                        If (ds_Schedule.Tables(0).Rows(e.Row.RowIndex)(GVC_cDisContinue) = "Y" Or ds_Schedule.Tables(0).Rows(e.Row.RowIndex)(GVC_cScreenFailure) = "Y") Then
                            txt.Enabled = False
                            txt1.Enabled = False
                        Else
                            txt.Attributes.Add("onClick", " return OpenMOdelPopup(this.id)")
                            txt1.Attributes.Add("onClick", " return OpenMOdelPopup(this.id)")
                            txt.Font.Underline = True
                            txt1.Font.Underline = True
                        End If
                        ActualDateVisible = True
                        If lblActual.Text = "" Then
                            ActualDateVisible1 = False
                        End If
                        'End If
                    End If
                    If Convert.ToString(ds_Actual.Tables(0).Rows(e.Row.RowIndex)(cIndex)).Trim() <> "" Then
                        e.Row.Cells(cIndex).Controls.Add(img)
                        img.Attributes.Add("onClick", " return AuditTrail(this.id)")
                        img.Attributes.Add("class", "image1")
                    End If
                Next
                'Next
                ds_ExportExcel.AcceptChanges()
            End If
            ViewState("ds_ExportExcel") = ds_ExportExcel

        End If
    End Sub

    Protected Sub gvSchedulerExport_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSchedulerExport.RowDataBound
        Dim HdrStr As String = String.Empty
        Dim strHeader As String = String.Empty
        Dim ds_ExportExcel As DataSet = CType(ViewState(VS_Scheduler), DataSet)
        ActualDateVisible1 = True
        If e.Row.RowType = DataControlRowType.Header Then
            For HdrIndex As Integer = 0 To e.Row.Cells.Count - 1
                ActId.Add("0")
                NodeId.Add("0")
                If e.Row.Cells(HdrIndex).Text.Contains("#") Then
                    HdrStr = Context.Server.HtmlDecode(e.Row.Cells(HdrIndex).Text.Trim())
                    ActId.Insert(HdrIndex, HdrStr.Substring(HdrStr.IndexOf("#") + 1, HdrStr.LastIndexOf("#") - (HdrStr.IndexOf("#") + 1)))
                    NodeId.Insert(HdrIndex, HdrStr.Substring(HdrStr.LastIndexOf("#") + 1).Trim())
                    strHeader = HdrStr.Substring(0, HdrStr.IndexOf("#"))
                    e.Row.Cells(HdrIndex).Attributes.Add("title", strHeader)
                    e.Row.Cells(HdrIndex).Text = strHeader
                End If
            Next HdrIndex
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            For HdrIndex As Integer = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(HdrIndex).Text.Replace("</br>", "")
            Next
        End If


    End Sub


    Protected Sub gvSchedulerExport_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSchedulerExport.RowCreated
        If e.Row.RowType = DataControlRowType.Header Or _
                   e.Row.RowType = DataControlRowType.DataRow Or _
                   e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVC_iMySubjectNo).Visible = False
            e.Row.Cells(GVC_cScreenFailure).Visible = False
            e.Row.Cells(GVC_cDisContinue).Visible = False
            e.Row.Cells(GVC_SubjectNo).Visible = False
        End If
    End Sub

    Protected Sub gvScheduler_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvScheduler.RowCreated
        If e.Row.RowType = DataControlRowType.Header Or _
                   e.Row.RowType = DataControlRowType.DataRow Or _
                   e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVC_iMySubjectNo).Visible = False
            e.Row.Cells(GVC_cScreenFailure).Visible = False
            e.Row.Cells(GVC_cDisContinue).Visible = False
            e.Row.Cells(GVC_SubjectNo).Visible = False
        End If
    End Sub

#End Region

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), System.Web.UI.WebControls.Label).Text = exMessage + "<BR>" + eStr
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR>" + eStr)
    End Sub

#End Region

#Region "Web Methods"
    <Web.Services.WebMethod()> _
    Public Shared Function SaveVisitScheduler(ByVal WorkSpaceId As String, ByVal randimisationno As String, ByVal VisitName As String, ByVal ScheduledDate As String, ByVal ModifyBy As String, ByVal SubjectId As String, ByVal iNodeId As Integer) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
        Dim strReturn As String = String.Empty
        Dim ds_Scheduled As DataSet
        Dim dr As DataRow
        Dim eStr As String
        Dim message As String = "Success"

        ds_Scheduled = objHelp.GetResultSet("select *  from [VisitScheduler] where 1=2 ", "VisitScheduler")

        Try
            dr = ds_Scheduled.Tables(0).NewRow()
            dr("vWorkSpaceId") = WorkSpaceId
            dr("vRandomisationNo") = Convert.ToString(randimisationno)
            dr("vVisitName") = Convert.ToString(VisitName)
            dr("iNodeId") = iNodeId
            dr("dScheduledDate") = ScheduledDate
            dr("vSubjectId") = SubjectId
            dr("iModifyby") = ModifyBy
            'dr("iVisitNo") = Convert.ToInt64(VisitName.Trim().Replace("Visit", ""))
            dr("cStatusIndi") = "N"
            ds_Scheduled.Tables(0).Rows.Add(dr)
            ds_Scheduled.AcceptChanges()

            If Not objLambda.Save_Scheduler(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Scheduled, ModifyBy, eStr) Then
                message = "failure"
            End If
            Return message
        Catch ex As Exception
            message = "failure"
            Throw New Exception(ex.Message)
        End Try

    End Function


    <Web.Services.WebMethod()> _
    Public Shared Function SaveVisitActual(ByVal WorkSpaceId As String, ByVal dActualDate As String, ByVal ModifyBy As String, ByVal SubjectId As String, ByVal VisitName As String, ByVal iNodeId As String, ByVal ScreenFailure As String, ByVal Discountinue As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
        Dim strReturn As String = String.Empty
        Dim ds_Scheduled As DataSet
        Dim dr As DataRow
        Dim eStr As String
        Dim message As String = "Success"

        ds_Scheduled = objHelp.GetResultSet("select *  from [VisitScheduler] where 1=2 ", "VisitScheduler")

        Try
            dr = ds_Scheduled.Tables(0).NewRow()
            dr("vWorkSpaceId") = WorkSpaceId
            dr("vSubjectId") = SubjectId
            dr("iModifyby") = ModifyBy
            dr("dModifyOn") = DateTime.Now.ToString()
            dr("dActualDate") = dActualDate
            dr("iNodeId") = iNodeId
            dr("cScreenFailure") = ScreenFailure
            dr("cDiscountinue") = Discountinue
            'dr("iVisitNo") = Convert.ToInt64(VisitName.Trim().Replace("Visit", ""))
            dr("cStatusIndi") = "E"
            ds_Scheduled.Tables(0).Rows.Add(dr)
            ds_Scheduled.AcceptChanges()

            If Not objLambda.Save_Scheduler(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_Scheduled, ModifyBy, eStr) Then
                message = "failure"
            End If
            Return message
        Catch ex As Exception
            message = "failure"
            Throw New Exception(ex.Message)
        End Try

    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function ActualVisitAuditTrail(ByVal WorkSpaceId As String, ByVal SubjectId As String, ByVal iNodeId As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
        Dim strReturn As String = String.Empty
        Dim ds_AuditTrail As DataSet
        Dim dtTempAuditTrail As New System.Data.DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New System.Data.DataTable
        Dim eStr As String
        Dim message As String = "Success"
        Dim wStr As String = String.Empty
        Try
            wStr = WorkSpaceId + "##" + SubjectId + "##" + iNodeId + "##"

            ds_AuditTrail = objHelp.ProcedureExecute("dbo.Proc_ActualVisitAuditTrail", wStr)

            If Not ds_AuditTrail Is Nothing Then
                dtTempAuditTrail.Columns.Add("SrNo")
                dtTempAuditTrail.Columns.Add("ProjectNo")
                dtTempAuditTrail.Columns.Add("Actitivity")
                dtTempAuditTrail.Columns.Add("ActualDate")
                dtTempAuditTrail.Columns.Add("ModifyBy")
                dtTempAuditTrail.Columns.Add("ModifyOn")



                For Each dr As DataRow In ds_AuditTrail.Tables(0).Rows

                    drAuditTrail = dtTempAuditTrail.NewRow()

                    drAuditTrail("SrNo") = i
                    drAuditTrail("ProjectNo") = dr("vProjectNo")
                    drAuditTrail("Actitivity") = dr("vNodeDisplayName")
                    drAuditTrail("ActualDate") = Convert.ToString(CDate(dr("dActualDate")).ToString("dd-MMM-yyyy"))
                    drAuditTrail("ModifyBy") = dr("Modifyby")
                    drAuditTrail("ModifyOn") = Convert.ToString(CDate(dr("dModifyOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)

                    dtTempAuditTrail.Rows.Add(drAuditTrail)
                    dtTempAuditTrail.AcceptChanges()

                    i += 1
                Next

                strReturn = JsonConvert.SerializeObject(dtTempAuditTrail)
                Return strReturn


            End If

            Return message
        Catch ex As Exception
            message = "failure"
            Throw New Exception(ex.Message)
        End Try

    End Function
#End Region

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As System.Web.UI.Control)
        ' Don't remove this function
    End Sub
End Class




