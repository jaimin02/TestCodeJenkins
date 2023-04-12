Imports System.Net.Mail

Partial Class frmProjectDetailMst
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATIONS"
    Private objCommon As New clsCommon
    Private objHelpDBTable As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()


    Private Const VS_ActivityName As String = "ActivityName"
    Private Const VS_dsSourceDocDtl As String = "SourceDocDtl"

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
    Private Const GRDCell_btnTalk As Integer = 12
    Private Const GRDCell_btnSubjectMedEx As Integer = 13
    Private Const GRDCell_bntQC As Integer = 14
    Private Const GRDCell_btnSlot As Integer = 15
    Private Const GRDCell_btnActStart As Integer = 16
    Private Const GRDCell_btnActComp As Integer = 17
    Private Const GRDCell_btnView As Integer = 18
    Private Const GRDCell_btnDocs As Integer = 19
    Private Const GRDCell_btnSubject As Integer = 20
    Private Const GRDCell_btnRights As Integer = 21
    Private Const GRDCell_btnAuditTrail As Integer = 22
    'Private Const GRDCell_btnSubjectMedEx As Integer = 20

    'Private Const GRDCell_bntQC As Integer = 22

    Private Const GRDCell_ActId As Integer = 23
    Private Const GRDCell_DocId As Integer = 24
    Private Const GRDCell_SubjectWise As Integer = 25
    Private Const GRDCell_ResourceCode As Integer = 26
    Private Const GRDCell_LocationCode As Integer = 27
    Private Const GRDCell_ActStartModifyBy As Integer = 28
    Private Const GRDCell_ActEndModifyOn As Integer = 29

    Private ds_ProjectsDetail As DataSet
    Private eStr_Retu As String = String.Empty
    Private prodetail As String = String.Empty
    Private wrkspId As String
    Private SelectedIndex As Integer = 0
    Private vs_projectstatus As String = String.Empty
    Private vs_projectstatusdesc As String = String.Empty

#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ''add by shivani pandya for latest repeatition
        'Me.Session(S_SelectedRepeatation) = Nothing

        If IsNothing(Session(S_UserID)) Then
            Response.Redirect("~/Default.aspx?SessionExpire=true", True)
        End If

        If Not Session(S_ValidUser) = "Yes" Then
            Response.Redirect("~/Default.aspx", True)
        End If
        If Not IsPostBack Then

            GenCall_ShowUI()
        Else
            btnContinueWorking_Click(Nothing, Nothing)
            Exit Sub
        End If

        '  ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getData", "getData();", True)
    End Sub
#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim dt_OpMst As DataTable = Nothing
        Dim parentname As String = String.Empty
        Try
            Page.Title = "::  Project Details Master  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            If Request.QueryString("ParentName") <> "" Then
                parentname = Request.QueryString("ParentName").ToString().Trim()
                If parentname <> "" Then
                    BtnBack.Text = "Close"
                    BtnBack.ToolTip = "Close"

                    BtnBack2.Text = "Close"
                    BtnBack2.ToolTip = "Close"
                End If
            Else
            End If

            Me.lblerrormsg.Text = ""
            ' Me.txtFromDate.Text = Date.Today.AddDays(-7).ToString("dd-MMM-yy")
            '============added on 25-11-09===
            Me.ViewState("SubjectId") = Nothing
            Me.ViewState("MySubjectNo") = Nothing
            '================================
            ViewProjectSummary()
            BindGrid()

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
        Finally
        End Try
    End Function

#End Region

#Region "DATA RETRIEVEL "

    Private Sub ViewProjectSummary()
        Dim workSpaceId As String = Me.Request.QueryString("WorkSpaceId")
        Dim dt_ProjectSummary As DataTable = New DataTable
        Dim Wstr As String = String.Empty
        ds_ProjectsDetail = New DataSet
        Dim ds_SourceDocDtl As New DataSet
        Dim eStr As String = String.Empty
        Dim ds_projectstatusremark As New DataSet

        Try

            Wstr = "vWorkspaceId='" & workSpaceId & "'"
            If objHelpDBTable.GetViewgetWorkspaceDetailForHdr(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ProjectsDetail, eStr_Retu) Then
                dt_ProjectSummary = ds_ProjectsDetail.Tables(0)
                Me.ViewState("vs_projectstatus") = ds_ProjectsDetail.Tables(0).Rows(0).Item("cProjectStatus").ToString()
                Me.ViewState("vs_projectstatusdesc") = ds_ProjectsDetail.Tables(0).Rows(0).Item("cProjectStatusDesc").ToString()
                Dim strProjectSummary As String = ""

                Wstr = "vWorkspaceId='" & workSpaceId & "' and (cProjectStatus = 'S' or cProjectStatus = 'L')and vRemarks is not null order by dmodifyon desc"
                If Not objHelpDBTable.getWorkSpaceStatusDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_projectstatusremark, eStr_Retu) Then
                    objCommon.ShowAlert(eStr_Retu, Me)
                End If

                If Not ds_projectstatusremark.Tables(0).Rows.Count > 0 Then
                    ds_projectstatusremark = Nothing
                End If
                strProjectSummary = "<style>.design {border-collapse: collapse;}.design td{border: 1px solid black;}</style>"
                strProjectSummary += "<table style=""font-size:9pt"" width=""100%"" align=""right"" cellpadding=""2px"">"
                strProjectSummary += "<tbody><tr><td style=""text-align: right"" colspan=""4""><font color=""chocolate""> Welcome, " & Session(S_FirstName).ToString.Trim() & " " + Session(S_LastName).ToString.Trim() & "</font></td></tr></tbody></table>"

                strProjectSummary += "<table style=""font-size:9pt; border:1px solid #000;"" class=""design"" width=""100%"" align=""center""  cellpadding=""2px"">"
                strProjectSummary += "<tbody><tr><td style=""text-align: center"" colspan=""4"" bgcolor=""DARKSEAGREEN"" ><strong><font color=""WHITE"">Project Details</font></strong></td></tr>"

                strProjectSummary += "<tr width=""100%"">"
                strProjectSummary += "<td align="" style=""width:0%;"" bgcolor=""gainsboro""><font color=""initial"">Project No</td><td style="""">" & dt_ProjectSummary.Rows(0)("vProjectNo").ToString & "</FONT></td>"
                strProjectSummary += "<td  bgcolor=""gainsboro""><font color=""initial"">Drug</font></td>"
                strProjectSummary += "<td align=""left"" word-break:="" break-all=""> " & dt_ProjectSummary.Rows(0)("vDrugName").ToString & "</td></tr>"

                strProjectSummary += "<tr width=""100%"">"
                strProjectSummary += " <td  bgcolor=""gainsboro""> <font color=""initial"">Sponsor</font></td>"
                strProjectSummary += "<td align=""left"" word-break:="" break-all="">" & dt_ProjectSummary.Rows(0)("vClientName").ToString & "</td>"
                strProjectSummary += " <td  bgcolor=""gainsboro""><font color=""initial"">Submissions</font></td><td align=""left"">" & dt_ProjectSummary.Rows(0)("vRegionName").ToString & "</td></tr>"

                strProjectSummary += "<tr>"
                strProjectSummary += "<td align=""""  bgcolor=""gainsboro""><font color=""initial"">No. of Subjects</font></td>"
                strProjectSummary += "<td align="""">" & dt_ProjectSummary.Rows(0)("iNoOfSubjects").ToString & "</td>"
                strProjectSummary += "<td align="""" width=""22%""  bgcolor=""gainsboro""><font color=""initial"">Principal Investigator</font></td><td align=""left"">" & dt_ProjectSummary.Rows(0)("PIName").ToString & "</td></tr>"

                strProjectSummary += "<tr>"
                strProjectSummary += "<td  bgcolor=""gainsboro""><font color=""initial"">Project Manager</font></td>"
                strProjectSummary += "<td>" & dt_ProjectSummary.Rows(0)("vProjectManager").ToString & "</td>"

                strProjectSummary += "<td align="""" width=""0""  bgcolor=""gainsboro""><font color=""initial"">Project Co-ordinator</font></td>"
                strProjectSummary += "<td align=""left"">" & dt_ProjectSummary.Rows(0)("vProjectCoordinator").ToString & "</td></tr>"


                If ds_ProjectsDetail Is Nothing Then
                    strProjectSummary += "<tr><td  bgcolor=""gainsboro""><font color=""initial"">Scope Of Services</font></td><td align=""left"" ></td><td></td><td></td></tr>"
                Else
                    strProjectSummary += "<tr><td  bgcolor=""gainsboro""><font color=""initial"">Scope Of Services</font></td><td align=""left"" >" & Convert.ToString(ds_ProjectsDetail.Tables(0).Rows(0)("vServiceName")) & "</td><td bgcolor=""gainsboro""></td><td></td></tr>"
                End If

                strProjectSummary += "</tbody></table>"

                'strProjectSummary = "<table style=""font-size:9pt"" width=""90%"" align=""right"" cellpadding=""2px""><tr><td style=""text-align: right"" colspan=""4""> Welcome, " & _
                '     Session(S_FirstName).ToString.Trim() & " " + Session(S_LastName).ToString.Trim() & "</td></tr>"
                'strProjectSummary += "</table>"
                'strProjectSummary += "<table style=""font-size:9pt"" width=""90%"" align=""center"" cellpadding=""2px""><tr><td style=""text-align: center"" colspan=""4""><strong>Project Details</strong><hr></td></tr>"

                'strProjectSummary += "<tr width=""100%""><td align=""right"" style=""width:25%"">Project No:</td><td style=""width:10%"" align=""left"">" & dt_ProjectSummary.Rows(0)("vProjectNo").ToString & "</td>" & _
                '                     "<td align=""right"" width=""7%"" >Drug:</td><td  align=""left"" width=""20%"" word-break: break-all> " & dt_ProjectSummary.Rows(0)("vDrugName").ToString & "</td></tr>" & _
                '                     "<tr width=""100%""><td align=""right"" width=""10%"">Sponsor:</td><td align=""left"" width=""20%"" word-break: break-all>" & dt_ProjectSummary.Rows(0)("vClientName").ToString & "</td>" & _
                '                     "<td align=""right"">Submissions:</td><td align=""left"">" & dt_ProjectSummary.Rows(0)("vRegionName").ToString & "</td></tr>" & _
                '                     "<tr><td align=""right"" >No. of Subjects:</td><td align=""left"" >" & dt_ProjectSummary.Rows(0)("iNoOfSubjects").ToString & "</td>" & _
                '                     "<td align=""right"" width=""22%"">Principal Investigator:</td><td align=""left"">" & dt_ProjectSummary.Rows(0)("PIName").ToString & "</td></tr>" & _
                '                     "<tr><td align=""right"">Project Manager:</td><td align=""left"">" & dt_ProjectSummary.Rows(0)("vProjectManager").ToString & "</td>" & _
                '                     "<td align=""right""  width=""20%"">Project Co-ordinator:</td><td align=""left"">" & dt_ProjectSummary.Rows(0)("vProjectCoordinator").ToString & "</td></tr>"
                'If ds_ProjectsDetail Is Nothing Then
                '    strProjectSummary += "<tr><td align=""right"">Scope Of Services:</td><td align=""left"" ></td></tr>"
                'Else
                '    strProjectSummary += "<tr><td align=""right"" >Scope Of Services:</td><td align=""left"" >" & Convert.ToString(ds_ProjectsDetail.Tables(0).Rows(0)("vServiceName")) & "</td></tr>"
                'End If


                'strProjectSummary += "</table>"


                lblProjectSummary.Text = strProjectSummary


            Else
                objCommon.ShowAlert(eStr_Retu, Me)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...ViewProjectSummary")
        End Try

    End Sub

    Private Sub BindGrid()
        Dim Type As String = String.Empty
        Dim WorkspaceId As String = String.Empty
        Dim UserId As String = String.Empty
        Dim MileStone As String = String.Empty
        Dim Operational As String = String.Empty
        Dim dv_ProjectsDetail As DataView = Nothing
        Dim sds_ProjectsDetail As DataSet = Nothing
        Me.lblerrormsg.Text = String.Empty
        Dim index As Integer = 0
        Dim dr As DataRow
        Dim drNew As DataRow
        Dim datasetcount As Integer = 0
        Dim ParentNodeID As String = String.Empty
        Dim dv_ParentNodeId As New DataView
        Dim dt_ParentNodeId As New DataTable
        Dim ds_ParentNodeId As New DataSet


        Try

            WorkspaceId = Me.Request.QueryString("WorkSpaceId")
            UserId = Session(S_UserID)

            If Not IsNothing(Me.Request.QueryString("Type")) Then
                Type = Me.Request.QueryString("Type").Trim()

                If Type.ToUpper.Trim() = "ALL" Then
                    MileStone = ""
                    Operational = "NO"
                ElseIf Type.ToUpper.Trim() = "MONITORING" Then
                    MileStone = "3"
                    Operational = "NO"

                    Me.gvwProjectsdetail.Columns(GRDCell_btnSubjectMedEx).Visible = False
                    Me.gvwProjectsdetail.Columns(GRDCell_btnView).Visible = False
                    Me.gvwProjectsdetail.Columns(GRDCell_DocId).Visible = False
                    Me.gvwProjectsdetail.Columns(GRDCell_btnSubject).Visible = False
                    Me.gvwProjectsdetail.Columns(GRDCell_btnRights).Visible = False
                    Me.gvwProjectsdetail.Columns(GRDCell_btnAuditTrail).Visible = False
                    Me.gvwProjectsdetail.Columns(GRDCell_bntQC).Visible = False
                    Me.gvwProjectsdetail.Columns(GRDCell_btnDocs).Visible = False

                    Me.gvwProjectsdetail.Columns(GRDCell_ActStartModifyBy).Visible = True
                    Me.gvwProjectsdetail.Columns(GRDCell_ActEndModifyOn).Visible = True

                ElseIf Type.ToUpper.Trim() = "MILESTONE" Then
                    MileStone = GeneralModule.MileStone_Monitoring.ToString.Trim() & "," & _
                                GeneralModule.MileStone_Monitoring_Scheduling.ToString.Trim() '"2,3"
                    Operational = "NO"
                ElseIf Type.ToUpper.Trim() = "OPERATIONAL" Then
                    MileStone = ""
                    'Operational = "YES"
                    Operational = "NO"

                    Me.gvwProjectsdetail.Columns(GRDCell_btnSlot).Visible = False
                    Me.gvwProjectsdetail.Columns(GRDCell_btnActStart).Visible = False
                    Me.gvwProjectsdetail.Columns(GRDCell_btnActComp).Visible = False
                    Me.gvwProjectsdetail.Columns(GRDCell_btnTalk).Visible = False
                    Me.gvwProjectsdetail.Columns(GRDCell_DocId).Visible = False
                    Me.gvwProjectsdetail.Columns(GRDCell_btnSubject).Visible = False
                    Me.gvwProjectsdetail.Columns(GRDCell_btnRights).Visible = False
                    Me.gvwProjectsdetail.Columns(GRDCell_btnAuditTrail).Visible = False
                    Me.gvwProjectsdetail.Columns(GRDCell_btnTalk).Visible = False
                    Me.gvwProjectsdetail.Columns(GRDCell_btnDocs).Visible = False
                    Me.gvwProjectsdetail.Columns(GRDCell_ShStart).Visible = False
                    Me.gvwProjectsdetail.Columns(GRDCell_ShEnd).Visible = False
                    Me.gvwProjectsdetail.Columns(GRDCell_Start).Visible = False
                    Me.gvwProjectsdetail.Columns(GRDCell_End).Visible = False

                    If Not IsNothing(Me.Request.QueryString("DMS")) Then
                        Dim strDMS As String = Me.Request.QueryString("DMS").Trim()
                        If strDMS.ToUpper.Trim() = "Y" Then
                            Me.gvwProjectsdetail.Columns(GRDCell_btnDocs).Visible = True
                        End If
                    End If
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




            If Not Me.objHelpDBTable.Proc_ProjectNodeCommandButtonRights(WorkspaceId, UserId, MileStone, Operational, ds_ProjectsDetail, eStr_Retu) Then
                Exit Sub
            End If

            'dv_ProjectsDetail = ds_ProjectsDetail.Tables(0).Copy().DefaultView()
            If Type.ToUpper.Trim() = "MONITORING" Then
                MileStone = "1"
                Operational = "NO"
                sds_ProjectsDetail = New DataSet
                If Not Me.objHelpDBTable.Proc_ProjectNodeCommandButtonRights(WorkspaceId, UserId, MileStone, Operational, sds_ProjectsDetail, eStr_Retu) Then

                    Exit Sub
                End If

                For Each dr In sds_ProjectsDetail.Tables(0).Rows

                    drNew = ds_ProjectsDetail.Tables(0).NewRow()

                    For ColIndex As Integer = 0 To sds_ProjectsDetail.Tables(0).Columns.Count - 1
                        drNew(ColIndex) = dr(ColIndex)
                    Next ColIndex

                    ds_ProjectsDetail.Tables(0).Rows.Add(drNew)


                Next dr
                ds_ProjectsDetail.AcceptChanges()
                'added on 5-Oct-2010 to not to show activities whose parent is not monitoring or Scheduling 
                For indexParNode As Integer = 0 To ds_ProjectsDetail.Tables(0).Rows.Count - 1
                    dv_ParentNodeId = ds_ProjectsDetail.Tables(0).DefaultView
                    dv_ParentNodeId.RowFilter = "iNodeId = " + Convert.ToString(ds_ProjectsDetail.Tables(0).Rows(indexParNode)("iParentNodeId"))

                    If Not dv_ParentNodeId.ToTable.Rows.Count > 0 Then
                        If Convert.ToString(ds_ProjectsDetail.Tables(0).Rows(indexParNode)("iParentNodeId")) <> "1" Then
                            ParentNodeID += Convert.ToString(ds_ProjectsDetail.Tables(0).Rows(indexParNode)("iParentNodeId")) + ","
                        End If
                    End If

                Next

                If ParentNodeID.Trim() <> "" And ParentNodeID.Contains(",") Then
                    ParentNodeID = ParentNodeID.Substring(0, ParentNodeID.LastIndexOf(","))
                End If

            End If


            dv_ProjectsDetail = ds_ProjectsDetail.Tables(0).Copy().DefaultView()
            If Not ParentNodeID = String.Empty Then
                dv_ProjectsDetail.RowFilter = "iParentNodeId not in (" + ParentNodeID + ")"
            End If
            '===============
            If Not IsNothing(Me.Request.QueryString("Period")) AndAlso Me.Request.QueryString("Period").Trim.ToUpper() <> "" Then

                dv_ProjectsDetail.RowFilter = "iPeriod=" & Me.Request.QueryString("Period").Trim()

            End If
            If Type.ToUpper.Trim() = "OPERATIONAL" Then
                dv_ProjectsDetail.RowFilter = " TemplateId <> '0001' "
            End If
            ViewState(VS_ActivityName) = dv_ProjectsDetail.ToTable()

            gvwProjectsdetail.DataSource = dv_ProjectsDetail.ToTable()
            'getData()
            gvwProjectsdetail.DataBind()
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getData", "getData();", True)

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....BindGrid")
        End Try

        Me.SetFocus("gvwProjectsdetail_ctl" & ViewState("SelectedIndex") + 2 & "_btnActStart") 'Added By Mrunal to Set focus
    End Sub

#End Region

    '#Region "getData"

    '    Private Sub getData()
    '        Dim ds_Check As DataSet = Nothing
    '        Dim dtProjectStatus As New DataTable
    '        Dim ProjectStatus As String = String.Empty
    '        Dim eStr As String = String.Empty
    '        Dim wStr As String = String.Empty
    '        Dim iTran As String = String.Empty
    '        Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
    '        Try
    '            wStr = "vWorkspaceId = '" + Me.Request.QueryString("WorkSpaceId") + "' And cStatusIndi <> 'D'"

    '            If Not objHelp.GetCRFLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                ds_Check, eStr) Then
    '                Throw New Exception(eStr)
    '            End If

    '            If ds_Check.Tables(0).Rows.Count > 0 Then

    '                dtProjectStatus = ds_Check.Tables(0)
    '                iTran = dtProjectStatus.Compute("Max(iTranNo)", String.Empty)
    '                dtProjectStatus.DefaultView.RowFilter = "iTranNo =" + iTran.ToString()

    '                If dtProjectStatus.DefaultView.ToTable.Rows.Count > 0 Then
    '                    Me.hndLockStatus.Value = dtProjectStatus.DefaultView.ToTable().Rows(0)("cLockFlag").ToString()
    '                End If
    '            Else
    '                Me.hndLockStatus.Value = "U"
    '            End If
    '        Catch ex As Exception
    '            Throw New Exception("Error while getdata")
    '        End Try
    '    End Sub

    '#End Region


#Region "BUTTON EVENTS "

    Protected Sub lnkBtndoc_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim gvr As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        Dim workSpaceId As String = gvr.Cells(0).Text
        Response.Redirect("frmProjectDetailMst.aspx?WorkSpaceId=" & workSpaceId)
    End Sub

    Protected Sub lnkBtndocstage_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim gvr As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        Dim workSpaceId As String = gvr.Cells(0).Text
        Response.Redirect("frmProjectDetailMst.aspx?WorkSpaceId=" & workSpaceId)
    End Sub

    Protected Sub BtnCloseDiv_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Me.divQC.Visible = False
        Me.HFQC.Value = ""
        Me.RBLQC.SelectedValue = Nothing
    End Sub

    Protected Sub Btnclose_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btnclose.Click
        Me.txtdob.Text = ""
    End Sub

    Protected Sub btnSaveDiv_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim RedirectStr As String = String.Empty
        Dim workspaceId As String = String.Empty
        Dim nodeId As String = String.Empty
        Dim StartDate As String = String.Empty
        Dim wstr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim SubjectLine As String = String.Empty
        Dim Count As Integer = 0
        Dim Emailcount As Integer = 0
        Dim MultipleUserId As String = String.Empty
        Dim MultipleEmail As String = String.Empty
        Dim IndexEmail As Integer = 0
        'Dim Dsiuserid As DataSet
        'Dim Ds_Email As DataSet
        'Dim Dv_iUserId As DataView
        'Dim Dv_Email As DataView
        Dim IndexForTableAdd As Integer = 0
        Dim TotalCount As Integer = 0
        Dim ActivityTable As New Table
        Dim tabrow As New TableRow
        Dim tabcolDisplayName As New TableCell
        Dim tabcolActualEndDate As New TableCell
        Dim tabcolActualStartDate As New TableCell
        Dim tabcolScheduleEndDate As New TableCell
        Dim tabcolScheduleStartDate As New TableCell
        Dim tabcolModifiedBy As New TableCell
        Dim tabcolActivityEndedOn As New TableCell
        Dim cActivityEndedOn As New TableCell
        Dim vActivity As New TableCell
        Dim Ds_ActivityName As New DataSet
        Dim SelectedGridRow As Integer = 0
        Dim ActualStartDate As String = String.Empty
        Dim ScheduleStartDate As String = String.Empty
        Dim ActualEndDate As String = String.Empty
        Dim ScheduleEndDate As String = String.Empty
        Dim ProjectName As String = String.Empty
        Dim index As Integer = CType(ViewState("index"), Integer)
        Dim ds_EmailId As New DataSet
        Dim ds_ChangeStatus As New DataSet
        Dim ActivityId As String = String.Empty
        Dim Wstr_Scope As String = String.Empty

        Try

            workspaceId = Me.Request.QueryString("WorkSpaceId")
            nodeId = Me.gvwProjectsdetail.Rows(index).Cells(GRDCell_NodeId).Text
            StartDate = Me.gvwProjectsdetail.Rows(index).Cells(GRDCell_Start).Text
            ViewState("text") = Me.txtdob.Text.ToString.Trim()

            If Me.HF.Value = "START" Then

                If SaveinWorkSpaceNodeAttrHistory("START", workspaceId, nodeId) Then

                    ActivityId = Me.gvwProjectsdetail.Rows(ViewState("SelectedIndex")).Cells(GRDCell_ActId).Text
                    If Not objHelpDBTable.Proc_GetNotificationEmailId(ActivityId, "Start", "Email", ds_EmailId, eStr) Then
                        Me.objCommon.ShowAlert("Error While Getting Email IDs", Me.Page)
                        Throw New Exception(eStr)
                    End If

                    If Not ds_EmailId Is Nothing Then
                        If Not ds_EmailId.Tables(0).Rows.Count < 1 Then
                            ViewState("ActivityDone") = "START"
                            Mail(ds_EmailId)
                            Dim ts As New System.Threading.Thread(AddressOf SendMail)
                            ts.Start()
                            'SendMail("End")
                        End If
                    End If

                    ViewProjectSummary()
                    BindGrid()
                    'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", "disableMenuBarAndToolBar('frmSendMail.aspx?ActivityDone=Start')", True)

                End If

            ElseIf Me.HF.Value = "COMPLETE" Then

                If Not StartDate = "&nbsp;" Or StartDate = "" Then

                    If CType(StartDate, Date) > CType(Me.txtdob.Text, Date) Then

                        'Me.div2.Attributes.Add("style", "display:none")
                        objCommon.ShowAlert("End Date must be greater then Start Date", Me)
                        txtdob.Text = Date.Today.ToString("dd-MMM-yyyy").Trim()
                        Me.mdlSetDate.Show()
                        Me.LblDiv2Heading.Text = "End Date"
                        Me.txtdob.Text = ""
                        'Me.gvwProjectsdetail.Enabled = False
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + Me.div2.ClientID.ToString.Trim() + _
                                                                 "');", True)
                        Exit Sub

                    End If

                End If

                If SaveinWorkSpaceNodeAttrHistory("COMPLETE", workspaceId, nodeId) Then
                    ActivityId = Me.gvwProjectsdetail.Rows(ViewState("SelectedIndex")).Cells(GRDCell_ActId).Text
                    If Not objHelpDBTable.Proc_GetNotificationEmailId(ActivityId, "End", "Email", ds_EmailId, eStr) Then
                        Me.objCommon.ShowAlert("Error While Getting Email IDs", Me.Page)
                        Throw New Exception(eStr)
                    End If

                    If Not ds_EmailId Is Nothing Then
                        If Not ds_EmailId.Tables(0).Rows.Count < 1 Then
                            ViewState("ActivityDone") = "END"
                            Mail(ds_EmailId)
                            Dim ts As New System.Threading.Thread(AddressOf SendMail)
                            ts.Start()
                            'SendMail("End")
                        End If
                    End If

                    ViewProjectSummary()
                    BindGrid()
                    'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", "disableMenuBarAndToolBar('frmSendMail.aspx?ActivityDone=End')", True)

                End If

            End If
            Btnclose_click(sender, e)
        Catch ex As Exception
            Me.objCommon.ShowAlert("Error While Starting/Ending An Activity. " + ex.Message, Me.Page)
        End Try

    End Sub


#End Region

    Private Sub Mail(ByVal ds_Mail As DataSet)
        Dim Dv_Email As DataView
        Dim Ds_Email As New DataSet
        Dim Emailcount As Integer = 0
        Dim MultipleEmail As String = String.Empty
        Dim SubjectLine As String = String.Empty
        Dim Ds_ActivityName As New DataSet
        Dim SelectedGridRow As Integer = 0
        Dim TotalCount As Integer = 0
        Dim tabcolDisplayName As New TableCell
        Dim tabcolActualEndDate As New TableCell
        Dim tabcolActualStartDate As New TableCell
        Dim tabcolScheduleEndDate As New TableCell
        Dim tabcolScheduleStartDate As New TableCell
        Dim tabcolModifiedBy As New TableCell
        Dim tabcolActivityEndedOn As New TableCell
        Dim cActivityEndedOn As New TableCell
        Dim vActivity As New TableCell
        Dim tabrow As New TableRow
        Dim ActivityTable As New Table
        Dim ActualStartDate As String = String.Empty
        Dim ScheduleStartDate As String = String.Empty
        Dim ActualEndDate As String = String.Empty
        Dim ScheduleEndDate As String = String.Empty
        Try


            Dv_Email = ds_Mail.Tables(0).DefaultView.ToTable(True, "vEmailId").DefaultView()
            Emailcount = Dv_Email.Count

            For index = 0 To Emailcount - 1

                If MultipleEmail.Trim() <> "" AndAlso _
                                Convert.ToString(Dv_Email.Item(index)("vEmailId")).Trim() <> "" Then
                    MultipleEmail += ","
                End If

                MultipleEmail += Convert.ToString(Dv_Email.Item(index)("vEmailId")).Trim()

            Next

            Ds_ActivityName.Tables.Add(CType(ViewState(VS_ActivityName), DataTable))
            SelectedGridRow = Me.ViewState("SelectedIndex")
            TotalCount = Ds_ActivityName.Tables(0).Rows.Count

            For IndexForTableAdd = 0 To TotalCount - 1

                tabcolDisplayName.Text = Convert.ToString(Ds_ActivityName.Tables(0).Rows(IndexForTableAdd)("vNodeDisplayName")).Trim()
                tabcolActualEndDate.Text = Convert.ToString(Ds_ActivityName.Tables(0).Rows(IndexForTableAdd)("vAttr4Value")).Trim()
                tabcolActualStartDate.Text = Convert.ToString(Ds_ActivityName.Tables(0).Rows(IndexForTableAdd)("vAttr6Value")).Trim()
                tabcolScheduleStartDate.Text = Convert.ToString(Ds_ActivityName.Tables(0).Rows(IndexForTableAdd)("vAttr2Value")).Trim()
                tabcolScheduleEndDate.Text = Convert.ToString(Ds_ActivityName.Tables(0).Rows(IndexForTableAdd)("vAttr1Value")).Trim()
                tabcolModifiedBy.Text = Convert.ToString(Ds_ActivityName.Tables(0).Rows(IndexForTableAdd)("vActivityEndedBy")).Trim()
                tabcolActivityEndedOn.Text = Convert.ToString(Ds_ActivityName.Tables(0).Rows(IndexForTableAdd)("cActivityEndedOn")).Trim()

                tabrow.Cells.Add(tabcolDisplayName)
                tabrow.Cells.Add(tabcolActualEndDate)
                tabrow.Cells.Add(tabcolActualStartDate)
                tabrow.Cells.Add(tabcolScheduleStartDate)
                tabrow.Cells.Add(tabcolScheduleEndDate)
                tabrow.Cells.Add(tabcolModifiedBy)
                tabrow.Cells.Add(tabcolActivityEndedOn)

                ActivityTable.Rows.Add(tabrow)

            Next

            SubjectLine = "<html><table><tr><td align=left>Project No :</td><td align=left colspan=6>"
            SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(0)("vProjectNo")).Trim()
            SubjectLine += "</td></tr><tr><td align=left>Project Name :</td><td align=left colspan=6>"
            SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(0)("vWorkspaceDesc")).Trim()
            SubjectLine += "</td></tr><tr><td align=left>Legends :</td><td align=left colspan=8>1) PN : Project Number<br/>2) AOI : Activity Of Interest<br/><font color=#347235>3) Dark Green : Activity Has Just Been Ended</font><br/><font color=#4CC417>4) Light Green : Activity Has Already Been Ended</font><br/><font color=Red>5) Red : Activity Delayed</font><br/><fonts color=#488AC7>6) Light Blue : Pending Activity</font> </td></tr>"
            SubjectLine += "<tr bgcolor=#1560A1 ><td align=center style=color:white> Actvity Name</td><td align=center style=color:white> Scheduled Start Date</td><td align=center style=color:white> Scheduled End Date</td><td align=center style=color:white> Actual Start Date </td><td align=center style=color:white> Actual End Date </td><td align=center style=color:white> Modified By</td><td align=center style=color:white> Activity Ended On</td></tr>"

            Dim RowStyle As String = String.Empty

            For CountMail As Integer = 0 To Ds_ActivityName.Tables(0).Rows.Count - 1

                ActualStartDate = Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr6Value")).Trim()
                ActualEndDate = Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr4Value")).Trim()
                ScheduleStartDate = Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr1Value")).Trim()
                ScheduleEndDate = Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr2Value"))

                If CountMail = SelectedGridRow Then
                    If ViewState("ActivityDone").ToString.ToUpper = "START" Then
                        ActualStartDate = ViewState("text")
                    Else
                        ActualEndDate = ViewState("text")
                    End If


                    If ActualStartDate <> "" Then

                        If ActualStartDate <> "" And ScheduleStartDate <> "" And ActualEndDate <> "" And ScheduleEndDate <> "" Then

                            If CType(ActualStartDate, Date) > CType(ScheduleStartDate, Date) And CType(ActualEndDate, Date) > CType(ScheduleEndDate, Date) Then

                                SubjectLine += "<tr bgcolor=#347235><td>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vNodeDisplayName")).Replace("*", "")
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr1Value"))
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr2Value"))
                                SubjectLine += "</td><td  bgcolor=#FF0000 >"
                                SubjectLine += ActualStartDate.ToString.Trim()
                                SubjectLine += "</td><td bgcolor=#FF0000 >"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr6Value"))
                                SubjectLine += "</td><td >"
                                SubjectLine += Session(S_FirstName)
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += objCommon.GetCurDatetime(Session(S_TimeZoneName))
                                SubjectLine += "</td></tr>"

                            ElseIf CType(ActualStartDate, Date) > CType(ScheduleStartDate, Date) And CType(ActualEndDate, Date) <= CType(ScheduleEndDate, Date) Then

                                SubjectLine += "<tr bgcolor=#347235><td>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vNodeDisplayName")).Replace("*", "")
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr1Value"))
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr2Value"))
                                SubjectLine += "</td><td  bgcolor=#FF0000>"
                                SubjectLine += ActualStartDate.ToString.Trim()
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr6Value"))
                                SubjectLine += "</td><td  style=color:white>"
                                SubjectLine += Session(S_FirstName)
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += objCommon.GetCurDatetime(Session(S_TimeZoneName))
                                SubjectLine += "</td></tr>"

                            ElseIf CType(ActualStartDate, Date) <= CType(ScheduleStartDate, Date) And CType(ActualEndDate, Date) > CType(ScheduleEndDate, Date) Then

                                SubjectLine += "<tr bgcolor=#347235><td style=color:white> "
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vNodeDisplayName")).Replace("*", "")
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr1Value"))
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr2Value"))
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += ActualStartDate.ToString.Trim()
                                SubjectLine += "</td><td bgcolor=#FF0000>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr6Value"))
                                SubjectLine += "</td><td style=color:white >"
                                SubjectLine += Session(S_FirstName)
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += objCommon.GetCurDatetime(Session(S_TimeZoneName))
                                SubjectLine += "</td></tr>"

                            Else

                                SubjectLine += "<tr bgcolor=#347235><td style=color:white>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vNodeDisplayName")).Replace("*", "")
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr1Value"))
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr2Value"))
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += ActualStartDate.ToString.Trim()
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr6Value"))
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += Session(S_FirstName)
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += objCommon.GetCurDatetime(Session(S_TimeZoneName))
                                SubjectLine += "</td></tr>"

                            End If

                        Else

                            SubjectLine += "<tr bgcolor=#347235><td style=color:white>"
                            SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vNodeDisplayName")).Replace("*", "")
                            SubjectLine += "</td><td style=color:white>"
                            SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr1Value"))
                            SubjectLine += "</td><td style=color:white>"
                            SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr2Value"))
                            SubjectLine += "</td><td style=color:white>"
                            SubjectLine += ActualStartDate.ToString.Trim()
                            SubjectLine += "</td><td style=color:white>"
                            SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr6Value"))
                            SubjectLine += "</td><td style=color:white >"
                            SubjectLine += Session(S_FirstName)
                            SubjectLine += "</td><td style=color:white>"
                            SubjectLine += objCommon.GetCurDatetime(Session(S_TimeZoneName))
                            SubjectLine += "</td></tr>"

                        End If

                    ElseIf ActualEndDate <> "" Then

                        If ActualStartDate <> "" And ScheduleStartDate <> "" And ActualEndDate <> "" And ScheduleEndDate <> "" Then
                            If CType(ActualStartDate, Date) > CType(ScheduleStartDate, Date) And CType(ActualEndDate, Date) > CType(ScheduleEndDate, Date) Then

                                SubjectLine += "<tr bgcolor=#4CC417><td style=color:white>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vNodeDisplayName")).Replace("*", "")
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr1Value"))
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr2Value"))
                                SubjectLine += "</td><td  bgcolor=#FF0000>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr4Value"))
                                SubjectLine += "</td><td bgcolor=#FF0000>"
                                SubjectLine += ActualEndDate.ToString.Trim()
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += Session(S_FirstName)
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += objCommon.GetCurDatetime(Session(S_TimeZoneName))
                                SubjectLine += "</td></tr>"

                            ElseIf CType(ActualStartDate, Date) > CType(ScheduleStartDate, Date) And CType(ActualEndDate, Date) <= CType(ScheduleEndDate, Date) Then

                                SubjectLine += "<tr bgcolor=#4CC417><td style=color:white>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vNodeDisplayName")).Replace("*", "")
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr1Value"))
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr2Value"))
                                SubjectLine += "</td><td  bgcolor=#FF0000>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr4Value"))
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += ActualEndDate.ToString.Trim()
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += Session(S_FirstName)
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += objCommon.GetCurDatetime(Session(S_TimeZoneName))
                                SubjectLine += "</td></tr>"

                            ElseIf CType(ActualStartDate, Date) <= CType(ScheduleStartDate, Date) And CType(ActualEndDate, Date) > CType(ScheduleEndDate, Date) Then

                                SubjectLine += "<tr bgcolor=#4CC417><td style=color:white>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vNodeDisplayName")).Replace("*", "")
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr1Value"))
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr2Value"))
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr4Value"))
                                SubjectLine += "</td><td bgcolor=#FF0000 >"
                                SubjectLine += ActualEndDate.ToString.Trim()
                                SubjectLine += "</td><td  style=color:white>"
                                SubjectLine += Session(S_FirstName)
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += objCommon.GetCurDatetime(Session(S_TimeZoneName))
                                SubjectLine += "</td></tr>"

                            Else

                                SubjectLine += "<tr bgcolor=#4CC417><td style=color:white>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vNodeDisplayName")).Replace("*", "")
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr1Value"))
                                SubjectLine += "</td><td  style=color:white>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr2Value"))
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr4Value"))
                                SubjectLine += "</td><td  style=color:white>"
                                SubjectLine += ActualEndDate.ToString.Trim()
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += Session(S_FirstName)
                                SubjectLine += "</td><td style=color:white>"
                                SubjectLine += objCommon.GetCurDatetime(Session(S_TimeZoneName))
                                SubjectLine += "</td></tr>"

                            End If

                        Else

                            SubjectLine += "<tr bgcolor=#4CC417><td style=color:white>"
                            SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vNodeDisplayName")).Replace("*", "")
                            SubjectLine += "</td><td style=color:white>"
                            SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr1Value"))
                            SubjectLine += "</td><td style=color:white>"
                            SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr2Value"))
                            SubjectLine += "</td><td style=color:white>"
                            SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr4Value"))
                            SubjectLine += "</td><td  style=color:white>"
                            SubjectLine += ActualEndDate.ToString.Trim()
                            SubjectLine += "</td><td  style=color:white>"
                            SubjectLine += Session(S_FirstName)
                            SubjectLine += "</td><td style=color:white>"
                            SubjectLine += objCommon.GetCurDatetime(Session(S_TimeZoneName))
                            SubjectLine += "</td></tr>"

                        End If


                        'SubjectLine += "<tr bgcolor=#347235><td style=color:white>"
                        'SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vNodeDisplayName")).Replace("*", "")
                        'SubjectLine += "</td><td style=color:white>"
                        'SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr1Value"))
                        'SubjectLine += "</td><td style=color:white>"
                        'SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr2Value"))
                        'SubjectLine += "</td><td style=color:white>"
                        'SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr6Value"))
                        'SubjectLine += "</td><td style=color:white>"
                        'SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr4Value"))
                        'SubjectLine += "</td><td style=color:white>"
                        'SubjectLine +=  Session(S_FirstName) 
                        'SubjectLine += "</td><td style=color:white>"
                        'SubjectLine += objCommon.GetCurDatetime(Session(S_TimeZoneName))
                        'SubjectLine += "</td></tr>"

                    End If

                Else




                    SubjectLine += "<tr bgcolor=#488AC7><td style=color:white>"
                    SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vNodeDisplayName")).Replace("*", "")
                    SubjectLine += "</td><td style=color:white> "
                    SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr1Value"))
                    SubjectLine += "</td><td style=color:white>"
                    SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr2Value"))
                    SubjectLine += "</td><td style=color:white>"
                    SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr4Value"))
                    SubjectLine += "</td><td style=color:white>"
                    SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vAttr6Value"))
                    SubjectLine += "</td><td style=color:white>"
                    SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("vActivityEndedBy"))
                    SubjectLine += "</td><td style=color:white>"
                    SubjectLine += Convert.ToString(Ds_ActivityName.Tables(0).Rows(CountMail)("cActivityEndedOn"))
                    SubjectLine += "</td></tr>"

                End If



            Next

            SubjectLine += "</table></html>"
            Session("UserEmail") = MultipleEmail
            Session("Body") = SubjectLine
            Session("projectno") = Convert.ToString(Ds_ActivityName.Tables(0).Rows(0)("vProjectNo")).Trim()
            Session("ActivityEndingName") = Convert.ToString(Ds_ActivityName.Tables(0).Rows(SelectedGridRow)("NodeDisName")).Replace("*", "")
        Catch ex As Exception
            objCommon.ShowAlert(ex.ToString, Me.Page)
        End Try
    End Sub

    Private Sub SendMail()
        Dim fromEmailId As String = String.Empty
        Dim toEmailId As String = String.Empty
        Dim password As String = String.Empty
        Dim ccEmailId As String = String.Empty
        Dim smtp As New SmtpClient
        Dim mailmsg As New MailMessage
        Dim wStr As String = String.Empty
        Dim StrTo() As String
        Dim ActivityDone As String = ViewState("ActivityDone")

        Try


            StrTo = Session("UserEmail").ToString().Split(",")
            For count As Integer = 0 To StrTo.Length - 1
                mailmsg.To.Add(New MailAddress(StrTo(count).Trim()))
            Next

            If ActivityDone = "END" Then

                fromEmailId = ConfigurationSettings.AppSettings("Username")

                password = ConfigurationSettings.AppSettings("Password")
                smtp = New SmtpClient
                smtp.Credentials = New Net.NetworkCredential(fromEmailId, password)
                smtp.EnableSsl = ConfigurationSettings.AppSettings("SslValue")
                smtp.Host = ConfigurationSettings.AppSettings("smtpServer")
                smtp.Port = ConfigurationSettings.AppSettings("ServerPort")
                smtp.Timeout = 2147483647
                'mailmsg = New MailMessage
                mailmsg.IsBodyHtml = True

                mailmsg.From = New MailAddress(fromEmailId)
                mailmsg.Subject = "BizNET Notification- PN :" & Session("projectno") & "- AOI :" & Session("ActivityEndingName") & "- Ended"
                mailmsg.Body = Session("Body") & "<br/>"
                '& TxtBody.Text
                smtp.Send(mailmsg)
                'smtp.SendAsync(mailmsg, mailmsg)
                Me.objCommon.ShowAlert("Mail Sent.", Me)


            ElseIf ActivityDone = "START" Then

                fromEmailId = ConfigurationSettings.AppSettings("Username")
                password = ConfigurationSettings.AppSettings("Password")
                smtp = New SmtpClient
                smtp.Credentials = New Net.NetworkCredential(fromEmailId, password)
                smtp.EnableSsl = ConfigurationSettings.AppSettings("SslValue")
                smtp.Host = ConfigurationSettings.AppSettings("smtpServer")
                smtp.Port = ConfigurationSettings.AppSettings("ServerPort")
                'mailmsg = New MailMessage
                mailmsg.IsBodyHtml = True
                smtp.Timeout = 2147483647
                mailmsg.From = New MailAddress(fromEmailId)

                StrTo = Session("UserEmail").ToString().Split(",")
                For count As Integer = 0 To StrTo.Length - 1
                    mailmsg.To.Add(New MailAddress(StrTo(count).Trim()))
                Next

                mailmsg.Subject = "BizNET Notification- PN :" & Session("projectno") & "- AOI :" & Session("ActivityEndingName") & "- Started"
                mailmsg.Body = Session("Body") & "<br/>"
                '& TxtBody.Text

                smtp.Send(mailmsg)
                'smtp.SendAsync(mailmsg, mailmsg)
                Me.objCommon.ShowAlert("Mail Sent.", Me)


            End If
        Catch ex As Exception
            Me.objCommon.ShowAlert("Error While Sending Mail " + ex.Message, Me.Page)
        End Try
    End Sub

#Region "GRID VIEW EVENTS "

    Protected Sub gvwProjectsdetail_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Or _
                e.Row.RowType = DataControlRowType.Header Or _
                e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GRDCell_Location).Visible = False
            e.Row.Cells(GRDCell_DocType).Visible = False
            e.Row.Cells(GRDCell_DocStage).Visible = False
            e.Row.Cells(GRDCell_Period).Visible = False

            e.Row.Cells(GRDCell_NodeId).Visible = False
            e.Row.Cells(GRDCell_Status).Visible = False
            e.Row.Cells(GRDCell_ActId).Visible = False
            e.Row.Cells(GRDCell_DocId).Visible = False
            'e.Row.Cells(GRDCell_Period).Visible = False
            e.Row.Cells(GRDCell_SubjectWise).Visible = False
            e.Row.Cells(GRDCell_ResourceCode).Visible = False
            e.Row.Cells(GRDCell_LocationCode).Visible = False

            'e.Row.Cells(GRDCell_btnSubjectMedEx).Visible = True
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
        Dim Location As String = String.Empty
        Dim strEndDate As String = String.Empty
        Dim strToday As String = String.Empty
        Dim strShEndDate As String = String.Empty

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
            'Dim str As String = Replace(CType(e.Row.FindControl("lblActivity"), Label).Text, "*", "")
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

            'FOr Change formate from "dd-MMM-yyyy" to "dd-MMM-yyyy"
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
                SelectedIndex = e.Row.RowIndex
                'Changed by Vishal on 05-Sep-2008
                If DateTime.Parse(strShEndDate) = DateTime.Parse(strEndDate) Then
                    e.Row.BackColor = Drawing.Color.Gray
                ElseIf DateTime.Parse(strShEndDate) < DateTime.Parse(strEndDate) Then
                    e.Row.Cells(GRDCell_End).BackColor = Drawing.Color.Red
                    e.Row.Cells(GRDCell_End).ForeColor = Drawing.Color.White
                End If
            End If

            If CType(e.Row.FindControl("btnActStart"), LinkButton).Enabled = True Then
                CType(e.Row.FindControl("btnActStart"), LinkButton).OnClientClick = "return confirm('Are you sure you want to Start Activity?');"
            End If

            If CType(e.Row.FindControl("btnActComp"), LinkButton).Enabled = True Then
                CType(e.Row.FindControl("btnActComp"), LinkButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("btnActComp"), LinkButton).CommandName = "Activity Complete"
                CType(e.Row.FindControl("btnActComp"), LinkButton).OnClientClick = "return confirm('Are you sure you want to End Activity?');"
            End If

            If Not IsNothing(Me.Request.QueryString("Type")) AndAlso Me.Request.QueryString("Type").Trim().ToUpper = "DOC" Then
                CType(e.Row.FindControl("btnSubjectMedEx"), LinkButton).Text = "Download"
            End If

            'If Me.hndLockStatus.Value.Trim() = "L" Then
            '    CType(e.Row.FindControl("bntQC"), LinkButton).Visible = False
            '    CType(e.Row.FindControl("btnActStart"), LinkButton).Visible = False
            '    CType(e.Row.FindControl("btnActComp"), LinkButton).Visible = False
            'End If

            '**************************

        End If
    End Sub

    Protected Sub gvwProjectsdetail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim Index As Integer = e.CommandArgument
        ViewState("index") = Index
        Dim workspaceId As String = String.Empty
        Dim nodeId As String = String.Empty
        Dim DocId As String = String.Empty
        Dim Doc As String = String.Empty
        Dim ActId As String = String.Empty
        Dim Act As String = String.Empty
        Dim Period As String = String.Empty
        Dim WinOpen As String = String.Empty
        Dim SubjectWise As String = String.Empty
        Dim RedirectStr As String = String.Empty
        Dim LocationCode As String = String.Empty
        Dim ResourceCode As String = String.Empty
        Dim ds_pmlst As New DataSet
        Dim dv_pmlst As New DataView
        Dim DMS As String = String.Empty
        Dim MySubjectNo As String = String.Empty
        Dim SubjectId As String = String.Empty
        Dim startdate As String = String.Empty
        Dim ds_SourceDocDtl As New DataSet
        Dim dvSourceDoc As DataView
        Dim eStr As String = String.Empty
        Dim Wstr As String = String.Empty


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
        Act = Replace(CType(Me.gvwProjectsdetail.Rows(Index).FindControl("lblActivity"), Label).Text, "&nbsp;", "").ToString.Trim()
        SubjectWise = Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_SubjectWise).Text
        ResourceCode = Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_ResourceCode).Text
        LocationCode = Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_LocationCode).Text
        DMS = IIf(Convert.ToString(Me.Request.QueryString("DMS")) Is Nothing, "", Convert.ToString(Me.Request.QueryString("DMS")))

        If e.CommandName.ToUpper = "SLOT" Then

            If (Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_ShStart).Text <> "" Or Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_ShStart).Text <> "&nbsp;") And _
                (Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_ShEnd).Text <> "" Or Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_ShEnd).Text <> "&nbsp;") Then

                Response.Redirect("frmSlotDateandLocation.aspx?workspaceid=" + workspaceId & "&nodeId=" & nodeId & "&Act=" & Act & "&Start=" & Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_ShStart).Text & "&End=" & _
                              Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_ShEnd).Text & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmProjectDetailMst&Type=" & Me.Request.QueryString("Type") & _
                              IIf(ResourceCode.Trim <> "", "&Res=" & ResourceCode, "") & IIf(LocationCode.Trim <> "", "&Loc=" & LocationCode, "") & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))

            End If
            Response.Redirect("frmSlotDateandLocation.aspx?workspaceid=" + workspaceId & "&nodeId=" & nodeId & "&Act=" & Act & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmProjectDetailMst&Type=" & Me.Request.QueryString("Type") & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))



        ElseIf e.CommandName.ToUpper = "SUBJECT" Then
            Response.Redirect("frmSubjectDetail.aspx?workspaceid=" + workspaceId & "&NodeId=" & nodeId & "&DCId=" & DocId & "&DC=" & Doc & "&ActId=" & ActId & "&Act=" & Act & "&Mode=1" & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmProjectDetailMst&Type=" & Me.Request.QueryString("Type") & "&QC=N" & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))


        ElseIf e.CommandName.ToUpper = "AUDIT" Then
            Response.Redirect("frmAuditTrailMst.aspx?workspaceid=" + workspaceId & "&nodeId=" & nodeId & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmProjectDetailMst&Type=" & Me.Request.QueryString("Type") & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))


        ElseIf e.CommandName.ToUpper = "DOCS" Then
            Response.Redirect("frmDocumentDetail_New.aspx?WorkSpaceId=" + workspaceId & "&NodeId=" & nodeId & "&Mode=1" & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmProjectDetailMst&Type=" & Me.Request.QueryString("Type") & "&QC=N" & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))


        ElseIf e.CommandName.ToUpper = "TALK" Then
            Response.Redirect("frmProjectTalk.aspx?mode=1&workspaceid=" + workspaceId & "&nodeId=" & nodeId & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmProjectDetailMst&Type=" & Me.Request.QueryString("Type") & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))


        ElseIf e.CommandName.ToUpper = "RIGHTS" Then
            Response.Redirect("frmWorkspaceUserRights.aspx?mode=1&workspaceid=" + workspaceId & "&nodeId=" & nodeId & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmProjectDetailMst&Type=" & Me.Request.QueryString("Type") & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))


        ElseIf e.CommandName.ToUpper = "ACTIVITY START" Then

            Me.HF.Value = "START"
            SelectedIndex = e.CommandArgument
            ViewState("SelectedIndex") = SelectedIndex
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' dharmesh salla 18-sept-2010  that checks whether activity has been started already or not??''

            '=========Added By Mrunal Parekh on 12-Jan-2012
            'If Me.gvwProjectsdetail.Rows(CInt(ViewState("SelectedIndex"))).Cells(GRDCell_ActId).Text.ToUpper() = 3217 Then
            '    Dim temp As New Integer
            '    For i As Integer = 0 To gvwProjectsdetail.Rows.Count - 1
            '        If gvwProjectsdetail.Rows(i).Cells(GRDCell_ActId).Text = 1090 Then
            '            If gvwProjectsdetail.Rows(i).Cells(GRDCell_Start).Text = "&nbsp;" Then
            '                Me.objCommon.ShowAlert("you can not Start Analysis Phase Because Study Phase is not Completed", Me)
            '                Exit Sub
            '            End If
            '        End If
            '    Next
            'End If

            If Me.gvwProjectsdetail.Rows(CInt(ViewState("SelectedIndex"))).Cells(GRDCell_ActId).Text.ToUpper() = Act_ReportDispatch And ViewState("vs_projectstatus") = "O" Then
                Dim temp As New Integer
                For i As Integer = 0 To gvwProjectsdetail.Rows.Count - 1
                    If gvwProjectsdetail.Rows(i).Cells(GRDCell_ActId).Text = Act_SampleAnalysis Then
                        If gvwProjectsdetail.Rows(i).Cells(GRDCell_Start).Text = "&nbsp;" Then

                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowConfirmation", "ShowConfirmation();", True)
                        End If
                    End If

                Next
            End If

            '=======
            If gvwProjectsdetail.Rows(SelectedIndex).Cells(GRDCell_Start).Text <> "&nbsp;" Then

                Me.objCommon.ShowAlert("Activity has already been started", Me)
                Exit Sub
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Else  'commented by dharmesh'
                'SDNidhi
                'txtdob.Text = Date.Today.ToString("dd-MMM-yyyy").Trim()
                txtdob.Text = Format(CDate(objCommon.GetCurDatetime(Me.Session(S_TimeZoneName))).Date, "dd-MMM-yyyy").Trim()
                Me.mdlSetDate.Show()
                'Me.gvwProjectsdetail.Enabled = False
                Me.LblDiv2Heading.Text = "Start Date"
                Me.LblDate.Text = "Start Date :" & " "
                ViewState("SelectedIndex") = SelectedIndex

                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + Me.div2.ClientID.ToString.Trim() + "');", True)


            End If  'commented by dharmesh'

            'Me.txtdob.Attributes.Add("onblur", "DateConvert_Format(" + Me.txtdob.ClientID + ")")
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

        ElseIf e.CommandName.ToUpper = "ACTIVITY COMPLETE" Then

            Me.HF.Value = "COMPLETE"
            SelectedIndex = e.CommandArgument
            ViewState("SelectedIndex") = SelectedIndex
            LblDate.Text = "End Date :" & " "
            ''''''''''''''''''' dharmesh salla 18-sept-2010' that checks whether if activty has been started? and if it is started  then it will allow to  end activity else it will not  allow''''''''''''''''''''''''''''''



            If gvwProjectsdetail.Rows(SelectedIndex).Cells(GRDCell_Start).Text = "&nbsp;" Then
                Me.objCommon.ShowAlert("you cannot end an activity, which is not being started yet", Me)
                Exit Sub
            End If


            ''''''''''''''' new comment
            If gvwProjectsdetail.Rows(SelectedIndex).Cells(GRDCell_End).Text <> "&nbsp;" Then
                Me.objCommon.ShowAlert("Activity has already been ended", Me)
                Exit Sub

            Else

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'SDNidhi
                'txtdob.Text = Date.Today.ToString("dd-MMM-yyyy").Trim()
                txtdob.Text = Format(CDate(objCommon.GetCurDatetime(Me.Session(S_TimeZoneName))).Date, "dd-MMM-yyyy").Trim()
                'Me.div2.Attributes.Add("style", "display:block")
                'Me.gvwProjectsdetail.Enabled = False
                Me.LblDiv2Heading.Text = "End Date"
                LblDate.Text = "End Date :" & " "
                Me.mdlSetDate.Show()
                'Me.div2.Attributes.Add("style", "display:block")
                'Me.gvwProjectsdetail.Enabled = False

                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + Me.div2.ClientID.ToString.Trim() + _
                                                            "');", True)
            End If     'commented by dharmesh salla' 

            'Me.div2.Visible = True
            '    Dim val As String = "12-Dec-09"
            'If SaveinWorkSpaceNodeAttrHistory("COMPLETE", workspaceId, nodeId, Val) Then
            'ViewProjectSummary()
            'BindGrid()
            'End If

        ElseIf e.CommandName.ToUpper = "SUBJECTMEDEX" Then
            '===========added on 18-11-09 by deepak singh on discussion with Naimesh Bhai and Mihir bhai======
            If SubjectId <> "" And Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_SubjectWise).Text.ToUpper.Trim() = "Y" Then
                ' SubjectId = Me.gvWorkSpaceSubjectMst.Rows(Index).Cells(GVC_SubjectId).Text

                RedirectStr = "window.open(""" + "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + workspaceId.Trim() + _
                              "&ActivityId=" + ActId + _
                              "&NodeId=" + nodeId + "&ScreenNo=" + SubjectId + "&PeriodId=" + Period + "&SubjectId=" + SubjectId + "&Type=BA-BE" + _
                              "&MySubjectNo=" + MySubjectNo + """)"

                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)
                Exit Sub
            End If
            '=================================================================
            If ActId = Act_Attendance Then
                Response.Redirect("frmWorkspaceSubjectMst.aspx?mode=1&workspaceid=" + workspaceId & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmProjectDetailMst&Type=" & Me.Request.QueryString("Type") & "&PeriodId=" & Period & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))
            End If

            If Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_SubjectWise).Text.ToUpper.Trim() = "Y" Then
                'Added on 17-Sep-2009 by Chandresh Vanker For CTM Flow.....
                If Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso Me.Request.QueryString("SubjectId").Trim.ToUpper() <> "" Then
                    Response.Redirect("frmWorkspaceSubjectMedExInfo.aspx?mode=1&workspaceid=" + workspaceId & _
                                "&nodeId=" & nodeId & "&ActivityId=" & ActId & "&PeriodId=" & Period & _
                                "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmProjectDetailMst&Type=" & _
                                Me.Request.QueryString("Type") & IIf(DMS.Trim() = "", "", "&DMS=" & DMS) & _
                                "&SubjectId=" & Me.Request.QueryString("SubjectId").Trim())
                    Exit Sub
                End If
                '***************
                Response.Redirect("frmWorkspaceSubjectMedExInfo.aspx?mode=1&workspaceid=" + workspaceId & "&nodeId=" & nodeId & "&ActivityId=" & ActId & "&PeriodId=" & Period & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmProjectDetailMst&Type=" & Me.Request.QueryString("Type") & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))
            Else
                'added querystring Type=BA-BE when the page is called from this page by deepak singh 
                RedirectStr = "window.open(""" + "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + workspaceId.Trim() + _
                                  "&ActivityId=" + ActId + "&NodeId=" + nodeId + _
                                  "&PeriodId=" + Period + "&SubjectId=0000" + "&ScreenNo=0000&Type=BA-BE" + _
                                  "&MySubjectNo=0000"")"
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)
            End If

        ElseIf e.CommandName.ToUpper = "VIEW" Then

            If ActId = Act_Attendance Then
                Response.Redirect("frmWorkspaceSubjectMst.aspx?Mode=4&workspaceid=" + workspaceId & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmProjectDetailMst&Type=" & Me.Request.QueryString("Type") & "&PeriodId=" & Period & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))
            End If

            If SubjectWise = "Y" Then
                Response.Redirect("frmInProcQC.aspx?workspaceid=" + workspaceId & "&NodeId=" & nodeId & "&Period=" & Period & "&ActId=" & ActId & "&Act=" & Act & "&Mode=4" & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmProjectDetailMst&Type=" & Me.Request.QueryString("Type"))
            Else
                RedirectStr = "window.open(""" + "frmCTMMedexInfoHdrDtl.aspx?Mode=4&WorkSpaceId=" + workspaceId.Trim() + _
                                  "&ActivityId=" + ActId + "&NodeId=" + nodeId + _
                                  "&PeriodId=" + Period + "&ScreenNo=0000&Type=BA-BE" + "&SubjectId=0000" + "&MySubjectNo=0000"")"

                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)
            End If

        ElseIf e.CommandName.ToUpper = "QC" Then

            If SubjectWise.ToUpper.Trim() = "Y" Then

                If Not objHelpDBTable.GetSourceDocDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, _
                        ds_SourceDocDtl, eStr) Then

                    Throw New Exception(eStr)
                End If

                dvSourceDoc = ds_SourceDocDtl.Tables(0).DefaultView
                dvSourceDoc.RowFilter = "vActivityID = '" + ActId + "'"
                Me.RBLQC.Items(2).Enabled = True
                If Not dvSourceDoc.ToTable.Rows.Count > 0 Then
                    Me.RBLQC.Items(2).Enabled = False
                End If
                Me.mdlDivQc.Show()
                'Me.divQC.Visible = True
                Me.LblActivity.Text = Replace(Act, "&nbsp;", "").Trim()
                Me.HFQC.Value = workspaceId & "," & nodeId & "," & DocId & "," & Doc & "," & _
                                Period & "," & ActId & "," & Replace(Act, "&nbsp;", "").Trim() & "," & _
                                Me.gvwProjectsdetail.Rows(Index).Cells(GRDCell_SubjectWise).Text.ToUpper.Trim()


                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + Me.divQC.ClientID.ToString.Trim() + _
                                                         "');", True)
            Else
                Response.Redirect("frmDocumentDetail_New.aspx?WorkSpaceId=" + workspaceId & "&NodeId=" & nodeId & "&Mode=1" & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmProjectDetailMst&Type=" & Me.Request.QueryString("Type") & "&QC=Y" & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))

            End If

        End If

    End Sub

#End Region

#Region "SaveinWorkSpaceNodeAttrHistory"
    Private Function SaveinWorkSpaceNodeAttrHistory(ByVal type As String, ByVal workspaceId As String, ByVal nodeId As String) As Boolean
        Dim Ds_Ret As New DataSet
        Dim Ds_Save As New DataSet
        Dim ds_WorkSpacemst As New DataSet
        Dim dt_WorkSpaceMstUpdate As New DataTable
        Dim Dt_Save As New DataTable
        Dim Dr As DataRow
        Dim estr As String = String.Empty
        Dim val As String = CType(ViewState("text"), String)
        Dim Wstr As String = String.Empty
        Try
            If Not Me.objHelpDBTable.getWorkspaceNodeAttrHistory("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_Ret, estr) Then
                Me.objCommon.ShowAlert("Error while Fetching Data from WorkspaceNodeAttrHistory", Me)
                Return False
            End If

            If type.ToUpper = "START" Then
                If CDate(txtdob.Text) > CDate(DateTime.Now.Date) Then
                    Me.objCommon.ShowAlert("The Start Date Cannot be greater then Today's Date", Me.Page)
                    gvwProjectsdetail.Enabled = True
                    Exit Function
                End If

                '===Added By Mrunal Parekh on 13-Jan-2012 for start Activity==================== 
                Wstr = "vWorkSpaceId='" & Me.Request.QueryString("WorkSpaceId") & "'"
                If Not objHelpDBTable.getworkspacemst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_WorkSpacemst, estr) Then
                    Me.objCommon.ShowAlert("Error While Getting Data From WorkSpaceMst", Me.Page)
                End If

                If Me.gvwProjectsdetail.Rows(CInt(ViewState("SelectedIndex"))).Cells(GRDCell_ActId).Text.ToUpper() = Act_EndStudy.ToUpper() Then
                    If Not (ViewState("vs_projectstatus") = "S" Or ViewState("vs_projectstatus") <> "C") Then
                        Me.objCommon.ShowAlert("Project status will not change to 'Clinical Phase' ,as Project is in " & ViewState("vs_projectstatusdesc").ToString() & " phase, this will just save your selected date", Me.Page)
                        'Exit Function
                    Else
                        For Each Dr In ds_WorkSpacemst.Tables(0).Rows
                            ds_WorkSpacemst.Tables(0).Rows(0)("cProjectstatus") = "S"
                        Next
                        ds_WorkSpacemst.Tables(0).AcceptChanges()
                        dt_WorkSpaceMstUpdate = ds_WorkSpacemst.Tables(0)
                        dt_WorkSpaceMstUpdate.AcceptChanges()
                        If Not objLambda.Save_workspacemst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_WorkSpacemst, Me.Session(S_UserID), "", estr) Then
                            Me.objCommon.ShowAlert("Error While Updating Data", Me.Page)
                            Exit Function
                        End If
                        ds_WorkSpacemst.Tables(0).Rows.Clear()
                    End If
                ElseIf Me.gvwProjectsdetail.Rows(CInt(ViewState("SelectedIndex"))).Cells(GRDCell_ActId).Text.ToUpper() = Act_SampleAnalysis.ToUpper() Then
                    'If Not ViewState("vs_projectstatus") = "L" Then   'Commented By Mrunal to Remove validation for sample analysis study
                    '    If Not ViewState("vs_projectstatus") = "O" Then
                    '        Me.objCommon.ShowAlert("You cannot start Analytical Phase. Because Project is in " & ViewState("vs_projectstatusdesc").ToString() & " phase", Me.Page)
                    '        Exit Function
                    '    End If
                    'End If
                    Dim flag As Char = "Y"
                    For i As Integer = 0 To Me.gvwProjectsdetail.Rows.Count - 1
                        If gvwProjectsdetail.Rows(i).Cells(GRDCell_ActId).Text.ToUpper() = Act_BARowdataandQAforAudit.ToUpper() Then
                            If gvwProjectsdetail.Rows(i).Cells(GRDCell_Start).Text <> "" And gvwProjectsdetail.Rows(i).Cells(GRDCell_Start).Text <> "&nbsp;" Then
                                flag = "N"
                            End If
                        End If
                    Next
                    If (flag = "N" Or (ViewState("vs_projectstatus") = "C")) Then   'Commented By Mrunal to Remove validation for sample analysis study
                        Me.objCommon.ShowAlert("Project status will not change to 'Analytical Phase'as Project is in " & ViewState("vs_projectstatusdesc").ToString() & " phase, this will just save your selected date", Me.Page)
                        '  Exit Function
                    Else


                        For Each Dr In ds_WorkSpacemst.Tables(0).Rows
                            ds_WorkSpacemst.Tables(0).Rows(0)("cProjectstatus") = "L"
                        Next
                        ds_WorkSpacemst.Tables(0).AcceptChanges()
                        dt_WorkSpaceMstUpdate = ds_WorkSpacemst.Tables(0)
                        dt_WorkSpaceMstUpdate.AcceptChanges()
                        If Not objLambda.Save_workspacemst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_WorkSpacemst, Me.Session(S_UserID), "", estr) Then
                            Me.objCommon.ShowAlert("Error While Updating Data", Me.Page)
                            Exit Function
                        End If
                        ds_WorkSpacemst.Tables(0).Rows.Clear()
                    End If
                ElseIf Me.gvwProjectsdetail.Rows(CInt(ViewState("SelectedIndex"))).Cells(GRDCell_ActId).Text.ToUpper() = Act_BARowdataandQAforAudit.ToUpper() Then

                    'If Not (ViewState("vs_projectstatus") = "S" Or ViewState("vs_projectstatus") = "L") Then   'Commented By Mrunal to Remove validation for sample analysis study
                    '    Me.objCommon.ShowAlert("This will not change project status as 'Document' as Project is in " & ViewState("vs_projectstatusdesc").ToString() & " phase,this will just save your selected date", Me.Page)
                    '    ' Exit Function
                    'Else

                    If (ViewState("vs_projectstatus") = "C") Then   'Commented By Mrunal to Remove validation for sample analysis study
                        Me.objCommon.ShowAlert("Project status will not change to 'Document Phase'as Project is in " & ViewState("vs_projectstatusdesc").ToString() & " phase, this will just save your selected date", Me.Page)
                        '  Exit Function
                    Else
                        For Each Dr In ds_WorkSpacemst.Tables(0).Rows
                            ds_WorkSpacemst.Tables(0).Rows(0)("cProjectstatus") = "D"
                        Next
                        ds_WorkSpacemst.Tables(0).AcceptChanges()
                        dt_WorkSpaceMstUpdate = ds_WorkSpacemst.Tables(0)
                        dt_WorkSpaceMstUpdate.AcceptChanges()
                        If Not objLambda.Save_workspacemst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_WorkSpacemst, Me.Session(S_UserID), "", estr) Then
                            Me.objCommon.ShowAlert("Error While Updating Data", Me.Page)
                            Exit Function
                        End If
                        ds_WorkSpacemst.Tables(0).Rows.Clear()
                    End If
                    'End If
                ElseIf Me.gvwProjectsdetail.Rows(CInt(ViewState("SelectedIndex"))).Cells(GRDCell_ActId).Text.ToUpper() = Act_ClinicaltoMedicalwriting.ToUpper() Then
                    If (ViewState("vs_projectstatus") = "L") Or (ViewState("vs_projectstatus") = "C") Then   'Commented By Mrunal to Remove validation for sample analysis study
                        Me.objCommon.ShowAlert("Project status will not change to 'Document' as Project is in " & ViewState("vs_projectstatusdesc").ToString() & " phase,this will just save your selected date", Me.Page)
                        ' Exit Function
                    Else


                        For Each Dr In ds_WorkSpacemst.Tables(0).Rows
                            ds_WorkSpacemst.Tables(0).Rows(0)("cProjectstatus") = "D"
                        Next
                        ds_WorkSpacemst.Tables(0).AcceptChanges()
                        dt_WorkSpaceMstUpdate = ds_WorkSpacemst.Tables(0)
                        dt_WorkSpaceMstUpdate.AcceptChanges()
                        If Not objLambda.Save_workspacemst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_WorkSpacemst, Me.Session(S_UserID), "", estr) Then
                            Me.objCommon.ShowAlert("Error While Updating Data", Me.Page)
                            Exit Function
                        End If
                        ds_WorkSpacemst.Tables(0).Rows.Clear()
                    End If
                ElseIf Me.gvwProjectsdetail.Rows(CInt(ViewState("SelectedIndex"))).Cells(GRDCell_ActId).Text.ToUpper() = Act_ReportDispatch.ToUpper() Then
                    For Each Dr In ds_WorkSpacemst.Tables(0).Rows
                        ds_WorkSpacemst.Tables(0).Rows(0)("cProjectstatus") = "C"
                    Next
                    ds_WorkSpacemst.Tables(0).AcceptChanges()
                    dt_WorkSpaceMstUpdate = ds_WorkSpacemst.Tables(0)
                    dt_WorkSpaceMstUpdate.AcceptChanges()
                    If Not objLambda.Save_workspacemst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_WorkSpacemst, Me.Session(S_UserID), "", estr) Then
                        Me.objCommon.ShowAlert("Error While Updating Data", Me.Page)
                        Exit Function
                    End If
                    ds_WorkSpacemst.Tables(0).Rows.Clear()
                End If
                'ElseIf Me.gvwProjectsdetail.Rows(CInt(ViewState("SelectedIndex"))).Cells(GRDCell_ActId).Text.ToUpper() = Act_ReportDispatch.ToUpper() Then
                '    If Not ViewState("vs_projectstatus") = "O" Then
                '        If Not ViewState("vs_projectstatus") = "Y" Then
                '            Me.objCommon.ShowAlert("You cannot start Document Phase. Because Project is in " & ViewState("vs_projectstatusdesc").ToString() & " phase", Me.Page)
                '            Return False
                '            Exit Function
                '        End If
                '    Else
                '        If ViewState("vs_projectstatus") = "S" Or ViewState("vs_projectstatus") = "L" Then
                '            Me.objCommon.ShowAlert("You cannot start Document Phase. Because Project is in " & ViewState("vs_projectstatusdesc").ToString() & " phase", Me.Page)

                '            Return False
                '            Exit Function
                '        End If
                '    End If

                '    For Each Dr In ds_WorkSpacemst.Tables(0).Rows
                '        ds_WorkSpacemst.Tables(0).Rows(0)("cProjectstatus") = "D"
                '    Next
                '    ds_WorkSpacemst.Tables(0).AcceptChanges()
                '    dt_WorkSpaceMstUpdate = ds_WorkSpacemst.Tables(0)
                '    dt_WorkSpaceMstUpdate.AcceptChanges()
                '    If Not objLambda.Save_workspacemst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_WorkSpacemst, Me.Session(S_UserID), "", estr) Then
                '        Me.objCommon.ShowAlert("Error While Updating Data", Me.Page)
                '        Exit Function
                '    End If
                '    ds_WorkSpacemst.Tables(0).Rows.Clear()




                '============================================================


                'Dt_Save = Ds_Ret.Tables(0)0
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

                If CDate(txtdob.Text) > CDate(DateTime.Now.Date) Then
                    Me.objCommon.ShowAlert("The End Date Cannot be greater then Today's Date", Me.Page)
                    gvwProjectsdetail.Enabled = True
                    Exit Function
                End If

                '    Wstr = "vWorkSpaceId='" & Me.Request.QueryString("WorkSpaceId") & "'"
                '    If Not objHelpDBTable.getworkspacemst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_WorkSpacemst, estr) Then
                '        Me.objCommon.ShowAlert("Error While Getting Data From WorkSpaceMst", Me.Page)
                '    End If
                ''====Commented by Mrunal Parekh
                '    'If Me.gvwProjectsdetail.Rows(CInt(ViewState("SelectedIndex"))).Cells(GRDCell_ActId).Text.ToUpper() = Act_EndStudy.ToUpper() Then

                '    '    For Each Dr In ds_WorkSpacemst.Tables(0).Rows
                '    '        ds_WorkSpacemst.Tables(0).Rows(0)("cProjectstatus") = "L" '===L is replace by O by Mrunal for study completed
                '    '    Next
                '    '    ds_WorkSpacemst.Tables(0).AcceptChanges()
                '    '    dt_WorkSpaceMstUpdate = ds_WorkSpacemst.Tables(0)
                '    '    dt_WorkSpaceMstUpdate.AcceptChanges()
                '    '    If Not objLambda.Save_workspacemst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_WorkSpacemst, Me.Session(S_UserID), "", estr) Then
                '    '        Me.objCommon.ShowAlert("Error While Updating Data", Me.Page)
                '    '        Exit Function
                '    '    End If
                '    '    ds_WorkSpacemst.Tables(0).Rows.Clear()
                '    '   

                '    'ElseIf Me.gvwProjectsdetail.Rows(CInt(ViewState("SelectedIndex"))).Cells(GRDCell_ActId).Text.ToUpper() = Act_SampleAnalysis.ToUpper() Then

                '    '    For Each Dr In ds_WorkSpacemst.Tables(0).Rows
                '    '        ds_WorkSpacemst.Tables(0).Rows(0)("cProjectstatus") = "D"  ''D replace by LE by Mrunal
                '    '    Next
                '    '    ds_WorkSpacemst.Tables(0).AcceptChanges()
                '    '    dt_WorkSpaceMstUpdate = ds_WorkSpacemst.Tables(0)
                '    '    dt_WorkSpaceMstUpdate.AcceptChanges()
                '    '    If Not objLambda.Save_workspacemst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_WorkSpacemst, Me.Session(S_UserID), "", estr) Then
                '    '        Me.objCommon.ShowAlert("Error While Updating Data", Me.Page)
                '    '        Exit Function
                '    '    End If
                '    '    ds_WorkSpacemst.Tables(0).Rows.Clear()

                '    'ElseIf Me.gvwProjectsdetail.Rows(CInt(ViewState("SelectedIndex"))).Cells(GRDCell_ActId).Text.ToUpper() = Act_ReportDispatch.ToUpper() Then

                '    '    For Each Dr In ds_WorkSpacemst.Tables(0).Rows
                '    '        ds_WorkSpacemst.Tables(0).Rows(0)("cProjectstatus") = "C"  'Addd by Mrunal
                '    '    Next
                '    '    ds_WorkSpacemst.Tables(0).AcceptChanges()
                '    '    dt_WorkSpaceMstUpdate = ds_WorkSpacemst.Tables(0)
                '    '    dt_WorkSpaceMstUpdate.AcceptChanges()
                '    '    If Not objLambda.Save_workspacemst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_WorkSpacemst, Me.Session(S_UserID), "", estr) Then
                '    '        Me.objCommon.ShowAlert("Error While Updating Data", Me.Page)
                '    '        Exit Function
                '    '    End If
                '    '    ds_WorkSpacemst.Tables(0).Rows.Clear()
                '    'End If

                ''=======Added By Mrunal Parekh on 19-Jan-2012 for complete activity 

                'If Me.gvwProjectsdetail.Rows(CInt(ViewState("SelectedIndex"))).Cells(GRDCell_ActId).Text.ToUpper() = Act_EndStudy.ToUpper() Then

                '    For Each Dr In ds_WorkSpacemst.Tables(0).Rows
                '        ds_WorkSpacemst.Tables(0).Rows(0)("cProjectstatus") = "O" '===L is replace by O by Mrunal for study completed
                '    Next
                '    ds_WorkSpacemst.Tables(0).AcceptChanges()
                '    dt_WorkSpaceMstUpdate = ds_WorkSpacemst.Tables(0)
                '    dt_WorkSpaceMstUpdate.AcceptChanges()
                '    If Not objLambda.Save_workspacemst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_WorkSpacemst, Me.Session(S_UserID), "", estr) Then
                '        Me.objCommon.ShowAlert("Error While Updating Data", Me.Page)
                '        Exit Function
                '    End If
                '    ds_WorkSpacemst.Tables(0).Rows.Clear()

                'ElseIf Me.gvwProjectsdetail.Rows(CInt(ViewState("SelectedIndex"))).Cells(GRDCell_ActId).Text.ToUpper() = Act_SampleAnalysis.ToUpper() Then

                '    For Each Dr In ds_WorkSpacemst.Tables(0).Rows
                '        ds_WorkSpacemst.Tables(0).Rows(0)("cProjectstatus") = "Y"  'D replace by Y by Mrunal
                '    Next
                '    ds_WorkSpacemst.Tables(0).AcceptChanges()
                '    dt_WorkSpaceMstUpdate = ds_WorkSpacemst.Tables(0)
                '    dt_WorkSpaceMstUpdate.AcceptChanges()
                '    If Not objLambda.Save_workspacemst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_WorkSpacemst, Me.Session(S_UserID), "", estr) Then
                '        Me.objCommon.ShowAlert("Error While Updating Data", Me.Page)
                '        Exit Function
                '    End If
                '    ds_WorkSpacemst.Tables(0).Rows.Clear()

                'ElseIf Me.gvwProjectsdetail.Rows(CInt(ViewState("SelectedIndex"))).Cells(GRDCell_ActId).Text.ToUpper() = Act_ReportDispatch.ToUpper() Then

                '    For Each Dr In ds_WorkSpacemst.Tables(0).Rows
                '        ds_WorkSpacemst.Tables(0).Rows(0)("cProjectstatus") = "C"  'Addd by Mrunal
                '    Next
                '    ds_WorkSpacemst.Tables(0).AcceptChanges()
                '    dt_WorkSpaceMstUpdate = ds_WorkSpacemst.Tables(0)
                '    dt_WorkSpaceMstUpdate.AcceptChanges()
                '    If Not objLambda.Save_workspacemst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_WorkSpacemst, Me.Session(S_UserID), "", estr) Then
                '        Me.objCommon.ShowAlert("Error While Updating Data", Me.Page)
                '        Exit Function
                '    End If
                '    ds_WorkSpacemst.Tables(0).Rows.Clear()
                'End If


                ''===Added By Mrunal Parekh on 12-Jan-2012===========
                ''ElseIf Me.gvwProjectsdetail.Rows(CInt(ViewState("SelectedIndex"))).Cells(GRDCell_ActId).Text.ToUpper() = 3217 Then

                ''    For Each Dr In ds_WorkSpacemst.Tables(0).Rows
                ''        ds_WorkSpacemst.Tables(0).Rows(0)("cProjectstatus") = "L"
                ''    Next
                ''    ds_WorkSpacemst.Tables(0).AcceptChanges()
                ''    dt_WorkSpaceMstUpdate = ds_WorkSpacemst.Tables(0)
                ''    dt_WorkSpaceMstUpdate.AcceptChanges()
                ''    If Not objLambda.Save_workspacemst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_WorkSpacemst, Me.Session(S_UserID), "", estr) Then
                ''        Me.objCommon.ShowAlert("Error While Updating Data", Me.Page)
                ''        Exit Function
                ''    End If
                ''    ds_WorkSpacemst.Tables(0).Rows.Clear()
                ''=======================


                ''=============End of project status change

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

            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...SaveinWorkSpaceNodeAttrHistory")
            Return False
        End Try

    End Function
#End Region

#Region "Back Buttons"
    Protected Sub BtnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnBack.Click
        If Request.QueryString("ParentName") <> "" Then
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "closechildwindow", "closechildwindow();", True)
        Else
            BackButton()
        End If

    End Sub

    Protected Sub BtnBack2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnBack2.Click
        If Request.QueryString("ParentName") <> "" Then
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "closechildwindow", "closechildwindow();", True)
        Else
            BackButton()
        End If
    End Sub

    Protected Sub BackButton()
        Dim Type As String = String.Empty
        Dim DMS As String = String.Empty
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
        Dim workspaceId As String = String.Empty
        Dim nodeId As String = String.Empty
        Dim DocId As String = String.Empty
        Dim Doc As String = String.Empty
        Dim Period As String = String.Empty
        Dim ActId As String = String.Empty
        Dim Act As String = String.Empty
        Dim SubjectWise As String = String.Empty
        Dim RedirectStr As String = String.Empty

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

            ' for QA 
            If Me.Session(S_UserType) = "0012" Or Me.Session(S_UserType) = "0011" Then

                Response.Redirect("frmSubjectDetail.aspx?workspaceid=" + workspaceId & "&nodeId=" & nodeId & "&DCId=" & DocId & "&DC=" & Doc & "&ActId=" & ActId & "&Act=" & Act & "&Mode=4" & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmProjectDetailMst&Type=" & Me.Request.QueryString("Type") & "&QC=Y")
            Else
                Response.Redirect("frmSubjectDetail.aspx?workspaceid=" + workspaceId & "&nodeId=" & nodeId & "&DCId=" & DocId & "&DC=" & Doc & "&ActId=" & ActId & "&Act=" & Act & "&Mode=1" & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmProjectDetailMst&Type=" & Me.Request.QueryString("Type") & "&QC=Y")
            End If

        ElseIf Me.RBLQC.SelectedValue.ToUpper.Trim = "INPROC" Then

            If SubjectWise = "Y" Then
                Response.Redirect("frmInprocQC.aspx?workspaceid=" + workspaceId & "&NodeId=" & nodeId & "&Period=" & Period & "&ActId=" & ActId & "&Act=" & Act & "&Mode=4" & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Page2=frmProjectDetailMst&Type=" & Me.Request.QueryString("Type") & "&QC=Y")

            Else
                'RedirectStr = "window.open(""" + "frmMedExInfoHdrDtl.aspx?Mode=4&WorkSpaceId=" + workspaceId.Trim() + _
                '                  "&ActivityId=" + ActId + "&NodeId=" + nodeId + _
                '                  "&PeriodId=" + Period + "&SubjectId=0000" + "&QC=Y" + _
                '                  "&MySubjectNo=0000"")"


                RedirectStr = "window.open(""" + "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + workspaceId.Trim() + _
                              "&ActivityId=" + ActId + _
                              "&NodeId=" + nodeId + "&ScreenNo=0000&PeriodId=" + Period + "&SubjectId=0000" + "&QC=Y" + _
                                  "&MySubjectNo=0000"")"

                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)

            End If
            'added on 1-May-2010
        ElseIf Me.RBLQC.SelectedValue.ToUpper.Trim = "SOURCE" Then

            RedirectStr = "window.open(""" + "frmSourceQA.aspx?&WorkSpaceId=" + workspaceId.Trim() + _
                            "&NodeId=" + nodeId + "&Act=" + Act + _
                              "&vActivityId= " + ActId + """)"
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "RefreshPage", "RefreshPage();", True)
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)



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

    Protected Sub btnContinueWorking_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinueWorking.Click
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ResetSessionTimer", "btnContinueWorking_Click();", True)
    End Sub


End Class
