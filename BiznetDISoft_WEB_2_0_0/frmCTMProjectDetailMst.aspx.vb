
Partial Class frmCTMProjectDetailMst
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATIONS"
    Private objCommon As New clsCommon
    Private objHelpDBTable As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()


    Private Const GRDCell_ActName As Integer = 0
    Private Const GRDCell_Period As Integer = 1
    Private Const GRDCell_NodeId As Integer = 2
    Private Const GRDCell_deptName As Integer = 3
    Private Const GRDCell_ShStart As Integer = 4
    Private Const GRDCell_ShEnd As Integer = 5
    Private Const GRDCell_Start As Integer = 6
    Private Const GRDCell_End As Integer = 7
    Private Const GRDCell_Location As Integer = 8
    Private Const GRDCell_Status As Integer = 9
    Private Const GRDCell_DocType As Integer = 10
    Private Const GRDCell_DocStage As Integer = 11
    Private Const GRDCell_btnSubjectMedEx As Integer = 12
    Private Const GRDCell_bntQC As Integer = 13
    Private Const GRDCell_btnSlot As Integer = 14
    Private Const GRDCell_btnActStart As Integer = 15
    Private Const GRDCell_btnActComp As Integer = 16
    Private Const GRDCell_btnTalk As Integer = 17
    Private Const GRDCell_btnDocs As Integer = 18
    Private Const GRDCell_btnSubject As Integer = 19
    Private Const GRDCell_btnRights As Integer = 20
    Private Const GRDCell_btnAuditTrail As Integer = 21
    'Private Const GRDCell_btnSubjectMedEx As Integer = 20
    Private Const GRDCell_btnView As Integer = 22
    'Private Const GRDCell_bntQC As Integer = 22

    Private Const GRDCell_ActId As Integer = 23
    Private Const GRDCell_DocId As Integer = 24
    Private Const GRDCell_SubjectWise As Integer = 25
    Private Const GRDCell_ResourceCode As Integer = 26
    Private Const GRDCell_LocationCode As Integer = 27

    Private ds_ProjectsDetail As DataSet
    Private eStr_Retu As String = ""
    Private prodetail As String = ""
    Private wrkspId As String
#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            GenCall_ShowUI()

        End If

    End Sub
#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim dt_OpMst As DataTable = Nothing

        Try
            Page.Title = " :: Project Details Master ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            Me.lblerrormsg.Text = ""
            ' Me.txtFromDate.Text = Date.Today.AddDays(-7).ToString("dd-MMM-yy")
            '============added on 25-11-09===
            Me.ViewState("SubjectId") = Nothing
            Me.ViewState("MySubjectNo") = Nothing
            '================================
            ViewProjectSummary()
            BindGrid()

            If Me.gvwProjectsdetail.Rows.Count = 1 Then
                ImediateRedirect()
            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function

#End Region

    Protected Sub ImediateRedirect()
        Dim Index As Integer = 0
        Dim workspaceId As String = ""
        Dim nodeId As String = ""
        Dim ActId As String = ""
        Dim Period As String = ""
        Dim RedirectStr As String = ""
        Dim MySubjectNo As String = ""
        Dim SubjectId As String = ""

        workspaceId = IIf(Convert.ToString(Me.Request.QueryString("WorkSpaceId")) Is Nothing, "", Convert.ToString(Me.Request.QueryString("WorkSpaceId")))
        nodeId = Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_NodeId).Text
        ActId = Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_ActId).Text
        Period = Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_Period).Text
        MySubjectNo = IIf(Convert.ToString(Me.Request.QueryString("MysubjectNo")) Is Nothing, "", Convert.ToString(Me.Request.QueryString("MysubjectNo")))
        SubjectId = IIf(Convert.ToString(Me.Request.QueryString("SubjectId")) Is Nothing, "", Convert.ToString(Me.Request.QueryString("SubjectId")))

        RedirectStr = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + workspaceId.Trim() + _
                          "&ActivityId=" + ActId + "&NodeId=" + nodeId + _
                          "&PeriodId=" + Period + "&SubjectId=" + SubjectId + _
                          "&MySubjectNo=" + MySubjectNo
        Me.Response.Redirect(RedirectStr)

    End Sub

#Region "DATA RETRIEVEL "

    Private Sub ViewProjectSummary()
        Dim workSpaceId As String = Me.Request.QueryString("WorkSpaceId")
        Dim dt_ProjectSummary As DataTable = New DataTable
        Dim Wstr As String = ""
        ds_ProjectsDetail = New DataSet

        Try
            Me.lblerrormsg.Text = ""
            Wstr = "vWorkspaceId='" & workSpaceId & "'"
            If objHelpDBTable.GetViewgetWorkspaceDetailForHdr(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ProjectsDetail, eStr_Retu) Then
                dt_ProjectSummary = ds_ProjectsDetail.Tables(0)
                Dim strProjectSummary As String = ""
                strProjectSummary = "<table style=""font-size:9pt"" width=""100%"" align=""center"" cellpadding=""2px""><tr><td colspan=""4""><strong>Project Details</strong><hr></td></tr>"

                strProjectSummary += "<tr><td align=""right"" style=""width:25%"">Project No:</td><td style=""width:25%"" align=""left"">" & dt_ProjectSummary.Rows(0)("vProjectNo").ToString & "</td>" & _
                                     "<td align=""right"" >Drug:</td><td  align=""left""> " & dt_ProjectSummary.Rows(0)("vDrugName").ToString & "</td></tr>" & _
                                     "<tr><td align=""right"" >Sponsor:</td><td align=""left"">" & dt_ProjectSummary.Rows(0)("vClientName").ToString & "</td>" & _
                                     "<td align=""right"">Submissions:</td><td align=""left"">" & dt_ProjectSummary.Rows(0)("vRegionName").ToString & "</td></tr>" & _
                                     "<tr><td align=""right"" >No. of Subjects:</td><td align=""left"" >" & dt_ProjectSummary.Rows(0)("iNoOfSubjects").ToString & "</td></tr>" & _
                                     "<tr><td align=""right"">Project Manager:</td><td align=""left"">" & dt_ProjectSummary.Rows(0)("vProjectManager").ToString & "</td>" & _
                                     "<td align=""right"">Project Co-ordinator:</td><td align=""left"">" & dt_ProjectSummary.Rows(0)("vProjectCoordinator").ToString & "</td></tr>"

                strProjectSummary += "</table><hr>"
                strProjectSummary += "<table style=""font-size:9pt"" width=""100%"" align=""right"" cellpadding=""2px""><tr><td colspan=""4""> Welcome, " & _
                                     Session(S_FirstName).ToString.Trim() & " " + Session(S_LastName).ToString.Trim() & "</td></tr>"
                strProjectSummary += "</table><hr>"

                lblProjectSummary.Text = strProjectSummary
            Else
                objCommon.ShowAlert(eStr_Retu, Me)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

    Private Sub BindGrid()
        Dim Type As String = ""
        Dim WorkspaceId As String = ""
        Dim UserId As String = ""
        Dim MileStone As String = ""
        Dim Operational As String = ""
        Dim dv_ProjectsDetail As DataView = Nothing
        Me.lblerrormsg.Text = ""

        WorkspaceId = Me.Request.QueryString("WorkSpaceId")
        UserId = Session(S_UserID)

        If Not IsNothing(Me.Request.QueryString("Type")) Then
            Type = Me.Request.QueryString("Type").Trim()

            If Type.ToUpper.Trim() = "ALL" Then
                MileStone = ""
                Operational = "NO"
            ElseIf Type.ToUpper.Trim() = "MILESTONE" Then
                MileStone = GeneralModule.MileStone_Monitoring.ToString.Trim() & "," & _
                            GeneralModule.MileStone_Monitoring_Scheduling.ToString.Trim() '"2,3"
                Operational = "NO"
            ElseIf Type.ToUpper.Trim() = "OPERATIONAL" Then
                MileStone = ""
                'Operational = "YES"
                Operational = "NO"
            End If

            'Added by satyam 03-march-2009 
            'for Control distribution documents
            If Type.ToUpper = "DOC" Then

                MileStone = ""
                Operational = "NO"

                Me.gvwProjectsdetail.Columns(GRDCell_btnSlot).Visible = False
                Me.gvwProjectsdetail.Columns(GRDCell_btnActStart).Visible = False
                Me.gvwProjectsdetail.Columns(GRDCell_btnActComp).Visible = False
                Me.gvwProjectsdetail.Columns(GRDCell_btnTalk).Visible = False
                Me.gvwProjectsdetail.Columns(GRDCell_DocId).Visible = False
                Me.gvwProjectsdetail.Columns(GRDCell_btnSubject).Visible = False
                Me.gvwProjectsdetail.Columns(GRDCell_btnRights).Visible = False
                Me.gvwProjectsdetail.Columns(GRDCell_btnAuditTrail).Visible = False
                Me.gvwProjectsdetail.Columns(GRDCell_bntQC).Visible = False

            End If

        End If

        ds_ProjectsDetail = New DataSet

        Try

            If Not Me.objHelpDBTable.Proc_ProjectNodeCommandButtonRights(WorkspaceId, UserId, MileStone, Operational, ds_ProjectsDetail, eStr_Retu) Then
                Exit Sub
            End If

            dv_ProjectsDetail = ds_ProjectsDetail.Tables(0).Copy().DefaultView()

            If Not IsNothing(Me.Request.QueryString("Period")) AndAlso Me.Request.QueryString("Period").Trim.ToUpper() <> "" Then

                dv_ProjectsDetail.RowFilter = "iPeriod=" & Me.Request.QueryString("Period").Trim()

            End If

            gvwProjectsdetail.DataSource = dv_ProjectsDetail.ToTable()
            gvwProjectsdetail.DataBind()

        Catch ex As Exception

            ShowErrorMessage(ex.Message, "")

        End Try
    End Sub

#End Region

#Region "BUTTON EVENTS "

    Protected Sub lnkBtndoc_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim gvr As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        Dim workSpaceId As String = gvr.Cells(0).Text
        Response.Redirect("frmCTMProjectDetailMst.aspx?WorkSpaceId=" & workSpaceId)
    End Sub

    Protected Sub lnkBtndocstage_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim gvr As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        Dim workSpaceId As String = gvr.Cells(0).Text
        Response.Redirect("frmCTMProjectDetailMst.aspx?WorkSpaceId=" & workSpaceId)
    End Sub

    Protected Sub BtnCloseDiv_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.divQC.Visible = False
        Me.HFQC.Value = ""
        Me.gvwProjectsdetail.Enabled = True
    End Sub
    Protected Sub Btnclose_click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Me.div2.Attributes.Add("style", "display:none")
        Me.txtdob.Text = ""
        Me.div2.Visible = False
        Me.gvwProjectsdetail.Enabled = True
    End Sub

    Protected Sub btnSaveDiv_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim workspaceId As String = ""
        Dim nodeId As String = ""
        Dim index As Integer = CType(ViewState("index"), Integer)
        workspaceId = Me.Request.QueryString("WorkSpaceId")
        nodeId = Me.gvwProjectsdetail.Rows(index).Cells(GRDCell_NodeId).Text

        ViewState("text") = Me.txtdob.Text.ToString.Trim()
        If Me.HF.Value = "START" Then
            If SaveinWorkSpaceNodeAttrHistory("START", workspaceId, nodeId) Then
                ViewProjectSummary()
                BindGrid()
            End If
        ElseIf Me.HF.Value = "COMPLETE" Then
            If SaveinWorkSpaceNodeAttrHistory("COMPLETE", workspaceId, nodeId) Then
                ViewProjectSummary()
                BindGrid()
            End If

        End If
    End Sub

 
#End Region

#Region "GRID VIEW EVENTS "

    Protected Sub gvwProjectsdetail_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Or _
                e.Row.RowType = DataControlRowType.Header Or _
                e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GRDCell_NodeId).Visible = False
            e.Row.Cells(GRDCell_Status).Visible = False
            e.Row.Cells(GRDCell_ActId).Visible = False
            e.Row.Cells(GRDCell_DocId).Visible = False
            'e.Row.Cells(GRDCell_Period).Visible = False
            e.Row.Cells(GRDCell_SubjectWise).Visible = False
            e.Row.Cells(GRDCell_ResourceCode).Visible = False
            e.Row.Cells(GRDCell_LocationCode).Visible = False

            'Added on 03-Oct-2009 as per Lambda's Requirement
            'e.Row.Cells(GRDCell_btnView).Visible = False
            'If Me.Request.QueryString("Type").ToUpper.Trim() = "ALL" Then
            e.Row.Cells(GRDCell_btnView).Visible = True
            'End If
            '*********************************

        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("id", e.Row.RowIndex)
            e.Row.Attributes.Add("onclick", "ShowColor(this);")
        End If

    End Sub

    Protected Sub gvwProjectsdetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Dim Location As String = ""
        Dim strEndDate As String = ""
        Dim strToday As String = ""
        Dim strShEndDate As String = ""

        If e.Row.RowType = DataControlRowType.DataRow Then

            CType(e.Row.FindControl("btnSlot"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("btnActStart"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("btnActComp"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("btnTalk"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("btnDocs"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("btnSubject"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("btnRights"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("btnAuditTrail"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("btnSubjectMedEx"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("bntQC"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("btnView"), LinkButton).CommandArgument = e.Row.RowIndex


            Dim str As String = Replace(CType(e.Row.FindControl("lblActivity"), Label).Text, "*", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")
            CType(e.Row.FindControl("lblActivity"), Label).Text = str
            If e.Row.Cells(GRDCell_Status).Text = "Y" Then
                'e.Row.Cells(GRDCell_NodeId).Text
            End If

            e.Row.ToolTip = str.Replace("&nbsp;", "")
            e.Row.Cells(GRDCell_Location).ToolTip = e.Row.Cells(GRDCell_Location).Text.ToString().Replace("&nbsp;", "")


            'For Check in Activity Act_Checkin on 17-Sep-2009
            If e.Row.Cells(GRDCell_ActId).Text.Replace("&nbsp;", "") = Act_Checkin Or _
                e.Row.Cells(GRDCell_ActId).Text.Replace("&nbsp;", "") = Act_IPAdmin Or _
                e.Row.Cells(GRDCell_ActId).Text.Replace("&nbsp;", "") = Act_CheckOut Then

                e.Row.Cells(GRDCell_ActName).BackColor = Drawing.Color.LightGray
                'e.Row.Cells(GRDCell_ActId).Font.Bold = True
            End If
            '*******************

            If e.Row.Cells(GRDCell_Location).Text.Length > 10 Then
                Location = e.Row.Cells(GRDCell_Location).Text
                e.Row.Cells(GRDCell_Location).Text = Location.Substring(0, 10)
            End If

            'FOr Change formate from "dd-MMM-yyyy" to "dd-MMM-yy"
            If e.Row.Cells(GRDCell_ShStart).Text <> "" And e.Row.Cells(GRDCell_ShStart).Text <> "&nbsp;" Then
                e.Row.Cells(GRDCell_ShStart).Text = CDate(e.Row.Cells(GRDCell_ShStart).Text).ToString("dd-MMM-yyyy")
            End If

            If e.Row.Cells(GRDCell_ShEnd).Text <> "" And e.Row.Cells(GRDCell_ShEnd).Text <> "&nbsp;" Then
                e.Row.Cells(GRDCell_ShEnd).Text = CDate(e.Row.Cells(GRDCell_ShEnd).Text).ToString("dd-MMM-yyyy")
            End If

            If e.Row.Cells(GRDCell_Start).Text <> "" And e.Row.Cells(GRDCell_Start).Text <> "&nbsp;" Then
                e.Row.Cells(GRDCell_Start).Text = CDate(e.Row.Cells(GRDCell_Start).Text).ToString("dd-MMM-yyyy")
                'CType(e.Row.FindControl("btnActStart"), LinkButton).Enabled = False
            End If

            If e.Row.Cells(GRDCell_End).Text <> "" And e.Row.Cells(GRDCell_End).Text <> "&nbsp;" Then
                e.Row.Cells(GRDCell_End).Text = CDate(e.Row.Cells(GRDCell_End).Text).ToString("dd-MMM-yyyy")
                'CType(e.Row.FindControl("btnActComp"), LinkButton).Enabled = False
            End If

            '****************************************************

            'For Activity Not yet Started
            If (e.Row.Cells(GRDCell_Start).Text <> "" And e.Row.Cells(GRDCell_Start).Text <> "&nbsp;") And (e.Row.Cells(GRDCell_End).Text = "" Or e.Row.Cells(GRDCell_End).Text = "&nbsp;") Then
                e.Row.BackColor = Drawing.Color.FromArgb(240, 240, 240)
                'e.Row.Attributes.Add("backgroundColor", "#ffdead")
                'e.Row.ForeColor = Drawing.Color.White

            End If
            '*******************

            'For Activity Delayed by Today
            'Changed by Vishal on 05-Sep-2008
            If (e.Row.Cells(GRDCell_ShEnd).Text <> "" And e.Row.Cells(GRDCell_ShEnd).Text <> "&nbsp;") And (e.Row.Cells(GRDCell_End).Text = "" Or e.Row.Cells(GRDCell_End).Text = "&nbsp;") Then
                strEndDate = e.Row.Cells(GRDCell_ShEnd).Text
                strToday = DateTime.Today.ToString("dd-MMM-yyyy")

                If DateTime.Parse(strEndDate) < DateTime.Parse(strToday) Then
                    e.Row.Cells(GRDCell_ShEnd).BackColor = Drawing.Color.Red
                    e.Row.Cells(GRDCell_ShEnd).ForeColor = Drawing.Color.White
                End If

            End If
            '**************************

            'For Activity Completed
            If (e.Row.Cells(GRDCell_ShEnd).Text <> "" And e.Row.Cells(GRDCell_End).Text <> "") And (e.Row.Cells(GRDCell_ShEnd).Text <> "&nbsp;" And e.Row.Cells(GRDCell_End).Text <> "&nbsp;") Then
                strShEndDate = e.Row.Cells(GRDCell_ShEnd).Text
                strEndDate = e.Row.Cells(GRDCell_End).Text

                'Changed by Vishal on 05-Sep-2008
                If DateTime.Parse(strShEndDate) = DateTime.Parse(strEndDate) Then
                    e.Row.BackColor = Drawing.Color.Gray
                ElseIf DateTime.Parse(strShEndDate) < DateTime.Parse(strEndDate) Then
                    e.Row.Cells(GRDCell_End).BackColor = Drawing.Color.Red
                    e.Row.Cells(GRDCell_End).ForeColor = Drawing.Color.White
                End If
            End If

            If CType(e.Row.FindControl("btnActStart"), LinkButton).Enabled = True Then
                'Me.div2.Attributes.Add("style", "display:block")

                CType(e.Row.FindControl("btnActStart"), LinkButton).OnClientClick = "return confirm('Are you sure you want to Start Activity?');"
            End If

            If CType(e.Row.FindControl("btnActComp"), LinkButton).Enabled = True Then
                CType(e.Row.FindControl("btnActComp"), LinkButton).OnClientClick = "return confirm('Are you sure you want to End Activity?');"
            End If


            If Not IsNothing(Me.Request.QueryString("Type")) AndAlso Me.Request.QueryString("Type").Trim().ToUpper = "DOC" Then
                CType(e.Row.FindControl("btnSubjectMedEx"), LinkButton).Text = "Download"
            End If

            '**************************

        End If
    End Sub

    Protected Sub gvwProjectsdetail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim Index As Integer = e.CommandArgument
        ViewState("index") = Index
        Dim workspaceId As String = ""
        Dim nodeId As String = ""
        Dim DocId As String = ""
        Dim Doc As String = ""
        Dim ActId As String = ""
        Dim Act As String = ""
        Dim Period As String = ""
        Dim WinOpen As String = ""
        Dim SubjectWise As String = ""
        Dim RedirectStr As String = ""
        Dim LocationCode As String = ""
        Dim ResourceCode As String = ""
        Dim ds_pmlst As New DataSet
        Dim dv_pmlst As New DataView
        Dim DMS As String = ""
        Dim MySubjectNo As String = ""
        Dim SubjectId As String = ""
        If Me.Request.QueryString("Page").Trim() = "frmVisitDetail" And Me.ViewState("MySubjectNo") Is Nothing Then
            MySubjectNo = Me.Request.QueryString("MysubjectNo").Trim()
            SubjectId = Me.Request.QueryString("SubjectId").Trim()
        Else
            MySubjectNo = ""
            SubjectId = ""
        End If
        '====added on 25-11-09--
        Me.ViewState("MySubjectNo") = MySubjectNo
        Me.ViewState("SubjectId") = SubjectId




        '======================
        workspaceId = Me.Request.QueryString("WorkSpaceId")
        nodeId = Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_NodeId).Text
        DocId = Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_DocId).Text
        Doc = Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_DocType).Text
        ActId = Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_ActId).Text
        Period = Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_Period).Text
        Act = LTrim(Replace(CType(Me.gvwProjectsdetail.Rows(Index).FindControl("lblActivity"), Label).Text, "*", ""))
        SubjectWise = Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_SubjectWise).Text
        ResourceCode = Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_ResourceCode).Text
        LocationCode = Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_LocationCode).Text
        DMS = IIf(Convert.ToString(Me.Request.QueryString("DMS")) Is Nothing, "", Convert.ToString(Me.Request.QueryString("DMS")))

        If e.CommandName.ToUpper = "SLOT" Then

            If (Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_ShStart).Text <> "" Or Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_ShStart).Text <> "&nbsp;") And _
                (Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_ShEnd).Text <> "" Or Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_ShEnd).Text <> "&nbsp;") Then

                Response.Redirect("frmSlotDateandLocation.aspx?workspaceid=" + workspaceId & "&nodeId=" & nodeId & "&Act=" & Act & "&Start=" & Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_ShStart).Text & "&End=" & _
                              Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_ShEnd).Text & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmCTMProjectDetailMst&Type=" & Me.Request.QueryString("Type") & _
                              IIf(ResourceCode.Trim <> "", "&Res=" & ResourceCode, "") & IIf(LocationCode.Trim <> "", "&Loc=" & LocationCode, "") & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))

            End If
            Response.Redirect("frmSlotDateandLocation.aspx?workspaceid=" + workspaceId & "&nodeId=" & nodeId & "&Act=" & Act & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmCTMProjectDetailMst&Type=" & Me.Request.QueryString("Type") & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))

        End If

        If e.CommandName.ToUpper = "SUBJECT" Then
            Response.Redirect("frmSubjectDetail.aspx?workspaceid=" + workspaceId & "&NodeId=" & nodeId & "&DCId=" & DocId & "&DC=" & Doc & "&ActId=" & ActId & "&Act=" & Act & "&Mode=1" & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmCTMProjectDetailMst&Type=" & Me.Request.QueryString("Type") & "&QC=N" & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))
        End If

        If e.CommandName.ToUpper = "AUDIT" Then
            Response.Redirect("frmAuditTrailMst.aspx?workspaceid=" + workspaceId & "&nodeId=" & nodeId & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmCTMProjectDetailMst&Type=" & Me.Request.QueryString("Type") & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))
        End If

        If e.CommandName.ToUpper = "DOCS" Then
            Response.Redirect("frmDocumentDetail_New.aspx?WorkSpaceId=" + workspaceId & "&NodeId=" & nodeId & "&Mode=1" & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmCTMProjectDetailMst&Type=" & Me.Request.QueryString("Type") & "&QC=N" & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))
        End If

        If e.CommandName.ToUpper = "TALK" Then
            Response.Redirect("frmProjectTalk.aspx?mode=1&workspaceid=" + workspaceId & "&nodeId=" & nodeId & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmCTMProjectDetailMst&Type=" & Me.Request.QueryString("Type") & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))
        End If

        If e.CommandName.ToUpper = "RIGHTS" Then
            Response.Redirect("frmWorkspaceUserRights.aspx?mode=1&workspaceid=" + workspaceId & "&nodeId=" & nodeId & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmCTMProjectDetailMst&Type=" & Me.Request.QueryString("Type") & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))
        End If

        If e.CommandName.ToUpper = "ACTIVITY START" Then
            Me.HF.Value = "START"
            Me.div2.Visible = True
            Me.gvwProjectsdetail.Enabled = False
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + Me.div2.ClientID.ToString.Trim() + _
                                                     "');", True)

            '===added on 08-12-09===
            'If Not objHelpDBTable.GetParameterList("nParameterNo=5 and cActiveflag='Y'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_pmlst, eStr_Retu) Then
            '    '    objCommon.ShowAlert("Error while retreiving PamaterList", Me)
            '    '    Exit Sub
            '    'End If
            '    'If ds_pmlst.Tables(0).Rows(0)("vParameterValue") = "Y" Then
            '    '    'If SaveinWorkSpaceNodeAttrHistory("START", workspaceId, nodeId) Then
            '    '    'ViewProjectSummary()
            '    '    'BindGrid()
            '    '    'End If

            '    'End If
            '    '=======================
            'If SaveinWorkSpaceNodeAttrHistory("START", workspaceId, nodeId) Then
            'ViewProjectSummary()
            'BindGrid()
            'End If

        End If

        If e.CommandName.ToUpper = "ACTIVITY COMPLETE" Then
            Me.HF.Value = "COMPLETE"
            Me.div2.Visible = True
            Me.gvwProjectsdetail.Enabled = False
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + Me.div2.ClientID.ToString.Trim() + _
                                                     "');", True)
            '    Dim val As String = "12-Dec-09"
            'If SaveinWorkSpaceNodeAttrHistory("COMPLETE", workspaceId, nodeId, Val) Then
            'ViewProjectSummary()
            'BindGrid()
            'End If
        End If

        If e.CommandName.ToUpper = "SUBJECTMEDEX" Then
            MySubjectNo = IIf(Convert.ToString(Me.Request.QueryString("MysubjectNo")) Is Nothing, "", Convert.ToString(Me.Request.QueryString("MysubjectNo")))
            SubjectId = IIf(Convert.ToString(Me.Request.QueryString("SubjectId")) Is Nothing, "", Convert.ToString(Me.Request.QueryString("SubjectId")))

            RedirectStr = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + workspaceId.Trim() + _
                              "&ActivityId=" + ActId + "&NodeId=" + nodeId + _
                              "&PeriodId=" + Period + "&SubjectId=" + SubjectId + _
                              "&MySubjectNo=" + MySubjectNo
            Me.Response.Redirect(RedirectStr)
            '===========added on 18-11-09 by deepak singh on discussion with Naimesh Bhai and Mihir bhai======
            'If SubjectId <> "" And Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_SubjectWise).Text.ToUpper.Trim() = "Y" Then
            '    ' SubjectId = Me.gvWorkSpaceSubjectMst.Rows(Index).Cells(GVC_SubjectId).Text

            '    RedirectStr = "window.open(""" + "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + workspaceId.Trim() + _
            '     "&ActivityId=" + ActId + _
            '     "&NodeId=" + nodeId + "&PeriodId=" + Period + "&SubjectId=" + SubjectId + _
            '    "&MySubjectNo=" + MySubjectNo + """)"
            '    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)
            '    Exit Sub
            'End If
            ''=================================================================
            'If ActId = Act_Attendance Then
            '    Response.Redirect("frmWorkspaceSubjectMst.aspx?mode=1&workspaceid=" + workspaceId & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmCTMProjectDetailMst&Type=" & Me.Request.QueryString("Type") & "&PeriodId=" & Period & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))
            'End If

            'If Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_SubjectWise).Text.ToUpper.Trim() = "Y" Then
            '    'Added on 17-Sep-2009 by Chandresh Vanker For CTM Flow.....
            '    If Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso Me.Request.QueryString("SubjectId").Trim.ToUpper() <> "" Then
            '        Response.Redirect("frmWorkspaceSubjectMedExInfo.aspx?mode=1&workspaceid=" + workspaceId & _
            '                    "&nodeId=" & nodeId & "&ActivityId=" & ActId & "&PeriodId=" & Period & _
            '                    "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmCTMProjectDetailMst&Type=" & _
            '                    Me.Request.QueryString("Type") & IIf(DMS.Trim() = "", "", "&DMS=" & DMS) & _
            '                    "&SubjectId=" & Me.Request.QueryString("SubjectId").Trim())
            '        Exit Sub
            '    End If
            '    '***************
            '    Response.Redirect("frmWorkspaceSubjectMedExInfo.aspx?mode=1&workspaceid=" + workspaceId & "&nodeId=" & nodeId & "&ActivityId=" & ActId & "&PeriodId=" & Period & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmCTMProjectDetailMst&Type=" & Me.Request.QueryString("Type") & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))
            'Else
            '    RedirectStr = "window.open(""" + "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + workspaceId.Trim() + _
            '                      "&ActivityId=" + ActId + "&NodeId=" + nodeId + _
            '                      "&PeriodId=" + Period + "&SubjectId=0000" + _
            '                      "&MySubjectNo=0000"")"
            '    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)
            'End If

        End If

        If e.CommandName.ToUpper = "VIEW" Then

            If ActId = Act_Attendance Then
                Response.Redirect("frmWorkspaceSubjectMst.aspx?Mode=4&workspaceid=" + workspaceId & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmCTMProjectDetailMst&Type=" & Me.Request.QueryString("Type") & "&PeriodId=" & Period & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))
            End If

            If SubjectWise = "Y" Then

                Response.Redirect("frmInProcQC.aspx?workspaceid=" + workspaceId & "&NodeId=" & nodeId & "&Period=" & Period & "&ActId=" & ActId & "&Act=" & Act & "&Mode=4" & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmCTMProjectDetailMst&Type=" & Me.Request.QueryString("Type"))
            Else
                RedirectStr = "window.open(""" + "frmCTMMedExInfoHdrDtl.aspx?Mode=4&WorkSpaceId=" + workspaceId.Trim() + _
                                  "&ActivityId=" + ActId + "&NodeId=" + nodeId + _
                                  "&PeriodId=" + Period + "&SubjectId=0000" + "MySubjectNo=0000"")"

                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)

            End If

        End If


        If e.CommandName.ToUpper = "QC" Then

            If SubjectWise.ToUpper.Trim() = "Y" Then

                Me.divQC.Visible = True
                Me.LblActivity.Text = Replace(Act, "&nbsp;", "").Trim()
                Me.HFQC.Value = workspaceId & "," & nodeId & "," & DocId & "," & Doc & "," & _
                                Period & "," & ActId & "," & Replace(Act, "&nbsp;", "").Trim() & "," & _
                                Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_SubjectWise).Text.ToUpper.Trim()

                Me.gvwProjectsdetail.Enabled = False
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + Me.divQC.ClientID.ToString.Trim() + _
                                                         "');", True)
            Else
                Response.Redirect("frmDocumentDetail_New.aspx?WorkSpaceId=" + workspaceId & "&NodeId=" & nodeId & "&Mode=1" & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmCTMProjectDetailMst&Type=" & Me.Request.QueryString("Type") & "&QC=Y" & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))

            End If

        End If

    End Sub

#End Region

#Region "SaveinWorkSpaceNodeAttrHistory"
    Private Function SaveinWorkSpaceNodeAttrHistory(ByVal type As String, ByVal workspaceId As String, ByVal nodeId As String) As Boolean
        Dim Ds_Ret As New DataSet
        Dim Ds_Save As New DataSet
        Dim Dt_Save As New DataTable
        Dim Dr As DataRow
        Dim estr As String = ""
        Dim val As String = CType(ViewState("text"), String)
        Try
            If Not Me.objHelpDBTable.getWorkspaceNodeAttrHistory("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_Ret, estr) Then
                Me.objCommon.ShowAlert("Error while Fatching Data from WorkspaceNodeAttrHistory", Me)
                Return False
            End If

            If type.ToUpper = "START" Then
                'Dt_Save = Ds_Ret.Tables(0)
                Dr = Ds_Ret.Tables(0).NewRow()
                'vWorkSpaceId,iNodeId,iTranNo,iAttrId,vAttrValue,iStageId,vRemark,iModifyBy,dModifyOn,cStatusIndi
                Dr("vWorkSpaceId") = workspaceId
                Dr("iNodeId") = nodeId
                Dr("iTranNo") = "00"
                Dr("iAttrId") = "6"
                Dr("vAttrValue") = val
                'Today.Now.ToString("dd-MMM-yyyy hh:mm")
                Dr("iStageId") = GeneralModule.Stage_Authorized
                Dr("vRemark") = "ACTIVITY START"
                Dr("iModifyBy") = Me.Session(S_UserID)
                'Dr("vRemark") = ""
                Ds_Ret.Tables(0).Rows.Add(Dr)
                Ds_Ret.Tables(0).AcceptChanges()
            End If
            If type.ToUpper = "COMPLETE" Then
                'Dt_Save = Ds_Ret.Tables(0)
                Dr = Ds_Ret.Tables(0).NewRow()
                'vWorkSpaceId,iNodeId,iTranNo,iAttrId,vAttrValue,iStageId,vRemark,iModifyBy,dModifyOn,cStatusIndi
                Dr("vWorkSpaceId") = workspaceId
                Dr("iNodeId") = nodeId
                Dr("iTranNo") = "00"
                Dr("iAttrId") = "3"
                Dr("vAttrValue") = "Y"
                Dr("iStageId") = GeneralModule.Stage_Authorized
                Dr("vRemark") = "Completed"
                Dr("iModifyBy") = Me.Session(S_UserID)
                'Dr("vRemark") = ""
                Ds_Ret.Tables(0).Rows.Add(Dr)

                Dr = Ds_Ret.Tables(0).NewRow()
                'vWorkSpaceId,iNodeId,iTranNo,iAttrId,vAttrValue,iStageId,vRemark,iModifyBy,dModifyOn,cStatusIndi
                Dr("vWorkSpaceId") = workspaceId
                Dr("iNodeId") = nodeId
                Dr("iTranNo") = "00"
                Dr("iAttrId") = "4"
                Dr("vAttrValue") = val
                'Today.Now.ToString("dd-MMM-yyyy hh:mm")
                Dr("iStageId") = GeneralModule.Stage_Authorized
                Dr("vRemark") = "Finished"
                Dr("iModifyBy") = Me.Session(S_UserID)
                'Dr("vRemark") = ""
                Ds_Ret.Tables(0).Rows.Add(Dr)
                Ds_Ret.Tables(0).AcceptChanges()
            End If
            'Ds_Save.Tables.Add(Dt_Save)
            If Not Me.objLambda.Save_InsertDataForWorkspaceNodeAttrHistory(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, Ds_Ret, Me.Session(S_UserID), estr) Then
                Me.objCommon.ShowAlert("Error while Saveing Data into WorkspaceNodeAttrHistory", Me)
                Return False

            End If
            Me.txtdob.Text = ""
            Me.HF.Value = ""
            Me.div2.Visible = False
            Me.gvwProjectsdetail.Enabled = True
            'Me.objCommon.ShowAlert("Record Save SuccessFully", Me)
            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function
#End Region

#Region "Back Buttons"
    Protected Sub BtnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnBack.Click
        BackButton()
    End Sub

    Protected Sub BtnBack2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnBack2.Click
        BackButton()
    End Sub

    Protected Sub BackButton()
        Dim Type As String = ""
        Dim DMS As String = ""
        Type = IIf(Convert.ToString(Me.Request.QueryString("Type")) Is Nothing, "", Convert.ToString(Me.Request.QueryString("Type")))
        'Type = Me.Request.QueryString("Type").Trim()
        DMS = IIf(Convert.ToString(Me.Request.QueryString("DMS")) Is Nothing, "", Convert.ToString(Me.Request.QueryString("DMS")))

        'For CTM Department from Visit Detail form 
        If Not IsNothing(Me.Request.QueryString("Page")) AndAlso Me.Request.QueryString("Page").Trim.ToUpper() = "FRMVISITDETAIL" Then
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "CloseWin", "window.close();", True)
            Exit Sub
        End If
        '**************************

        Me.Response.Redirect(Me.Request.QueryString("Page").Trim() & ".aspx?mode=1&Type=" & Type & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))

    End Sub
#End Region

#Region "Redio Button Event"

    Protected Sub RBLQC_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strQC() As String
        Dim workspaceId As String = ""
        Dim nodeId As String = ""
        Dim DocId As String = ""
        Dim Doc As String = ""
        Dim Period As String = ""
        Dim ActId As String = ""
        Dim Act As String = ""
        Dim SubjectWise As String = ""
        Dim RedirectStr As String = ""

        strQC = Split(Me.HFQC.Value, ",")

        workspaceId = strQC(0)
        nodeId = strQC(1)
        DocId = strQC(2)
        Doc = strQC(3)
        Period = strQC(4)
        ActId = strQC(5)
        Act = strQC(6)
        SubjectWise = strQC(7)

        If Me.RBLQC.SelectedValue.ToUpper.Trim = "DOC" Then
            Response.Redirect("frmSubjectDetail.aspx?workspaceid=" + workspaceId & "&nodeId=" & nodeId & "&DCId=" & DocId & "&DC=" & Doc & "&ActId=" & ActId & "&Act=" & Act & "&Mode=4" & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmCTMProjectDetailMst&Type=" & Me.Request.QueryString("Type") & "&QC=Y")

        ElseIf Me.RBLQC.SelectedValue.ToUpper.Trim = "INPROC" Then

            If SubjectWise = "Y" Then
                Response.Redirect("frmInprocQC.aspx?workspaceid=" + workspaceId & "&NodeId=" & nodeId & "&Period=" & Period & "&ActId=" & ActId & "&Act=" & Act & "&Mode=4" & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmCTMProjectDetailMst&Type=" & Me.Request.QueryString("Type") & "&QC=Y")

            Else
                RedirectStr = "window.open(""" + "frmCTMMedExInfoHdrDtl.aspx?Mode=4&WorkSpaceId=" + workspaceId.Trim() + _
                                  "&ActivityId=" + ActId + "&NodeId=" + nodeId + _
                                  "&PeriodId=" + Period + "&SubjectId=0000" + "&QC=Y" + _
                                  "&MySubjectNo=0000"")"
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)

            End If


        End If

    End Sub

#End Region

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        'CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        Me.lblerrormsg.Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)

    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        'CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        Me.lblerrormsg.Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub
#End Region

End Class
