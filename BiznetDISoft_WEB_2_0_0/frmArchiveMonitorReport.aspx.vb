
Partial Class frmReport
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()

    Private eStr_Retu As String = ""
    Private Const VS_DdlSubject As String = "Subjects"

#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                Me.tdProjectInfo.Style.Add("display", "none")
                Me.tdProjectActivity.Style.Add("display", "none")
                Me.DvChilds.Style.Add("display", "")
                Me.ChkBClient.Style.Add("display", "")
                Me.HCTMStatus.Value = "Y"
                Me.lblRptScreening.Visible = False
                Me.lblRptAttandance.Visible = False

            End If
            If Not IsPostBack Then
                Me.ChkBClient.Checked = True
                If Not GenCall() Then
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr_Retu)
        End Try
    End Sub
#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            If Not GenCall_ShowUI() Then
                Throw New Exception()
            End If
            GenCall = True

        Catch ex As Exception
            GenCall = False
        End Try
    End Function

#End Region

#Region "GenCall_ShowUI"
    Private Function GenCall_ShowUI() As Boolean
        Try
            Page.Title = " ::  Archive Monitor Report Page ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Master.FindControl("lblHeading"), Label).Text = "Archive Monitor Report"
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
            GenCall_ShowUI = True
        Catch ex As Exception
            GenCall_ShowUI = False
        End Try

    End Function

#End Region

#Region "Reset Page"
    Private Function ResetPage() As Boolean
        Me.MonitorInfo.Style.Add("display", "none")
        Return True
    End Function
#End Region

#Region "ButtonEvents"
    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click

        Dim wstr As String = String.Empty
        Dim ds_ProjectInfo As New DataSet
        Dim ds_Report As DataSet = Nothing
        Dim dt_Subs As New DataTable
        Dim estr As String = String.Empty
        Dim str As String = String.Empty
        Dim schema As String = String.Empty
        Dim ds_AllSubjects As DataSet = Nothing
        Dim dv_AllSubjects As DataTable = Nothing

        Try
            If (HArchivedFlag.Value.ToUpper = "Y") Then
                ResetPage()
                Me.objCommon.ShowAlert("This Project Is Archived To See Its Detail UnArchive It.", Me.Page)
                Me.txtProject.Text = ""
                Exit Sub
            End If
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                Me.MonitorInfo.Style.Add("display", "block")
                If Not GetChildProjects() Then
                    Throw New Exception("Problem while getting child projects")
                End If
                Exit Try
            End If
            wstr += "vWorkspaceId = '" + Me.HProjectId.Value + "'"
            If Not objHelp.GetView_ProjectDetails(wstr, ds_ProjectInfo, estr) Then
                Throw New Exception
            End If
            If ds_ProjectInfo.Tables(0).Rows.Count < 1 Then
                Throw New Exception("Project information not found.")
            End If
            Me.MonitorInfo.Style.Add("display", "block")
            Me.lblManager.Text = ds_ProjectInfo.Tables(0).Rows(0).Item("ProjectManager").ToString().ToUpper()
            Me.lblPIMonitor.Text = "PI : "
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                Me.lblPIMonitor.Text = "Monitor : "
            End If
            Me.lblPI.Text = ds_ProjectInfo.Tables(0).Rows(0).Item("ProjectPI").ToString().ToUpper()

            wstr = String.Empty
            wstr += "vWorkSpaceId =" & Me.HProjectId.Value & " and vActivityId in ('1185','1186') order by vActivityId "
            If Not objHelp.getWorkSpaceNodeDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Report, eStr_Retu) Then
                Throw New Exception
            End If
            If ds_Report.Tables(0).Rows.Count > 0 Then
                Me.HQAONMSR.Value = ds_Report.Tables(0).Rows(0).Item("iNodeId").ToString
                ' Me.HQAONPIF.Value = ds_Report.Tables(0).Rows(1).Item("iNodeId").ToString
            End If

            wstr = Me.HProjectId.Value + "##" + Me.HSchemaId.Value

            If Not objHelp.proc_TotalEnterSubject_Archive(wstr, ds_AllSubjects, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If


            Ddlsubject.DataSource = ds_AllSubjects
            Ddlsubject.DataTextField = "vSubjectNameToDisplay"
            Ddlsubject.DataValueField = "vsubjectvalue"
            Ddlsubject.DataBind()
            Ddlsubject.Items.Insert(0, "--Select Subject--")
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, estr)
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

#Region "WebMethod for Activity"

    <Services.WebMethod()> _
    Public Shared Function GetProc_TreeViewOfNodes(ByVal WorkSpaceId As String, ByVal schema As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim Ds_ParentActivity As DataSet = Nothing
        Dim str As String = String.Empty
        Dim eStr As String = ""
        Dim wstr As String = String.Empty
        Dim Query_Str As String = String.Empty
        Try
            wstr = WorkSpaceId.ToString + "##" + "1" + "##" + schema.ToString
            If Not objHelp.Proc_GetNodeWithSubjectCount_Archive(wstr, Ds_ParentActivity, eStr) Then
                Throw New Exception
            End If
            str += "<UL ID=""accordion"">"
            For Each drParent As DataRow In Ds_ParentActivity.Tables(0).Rows
                str += "<LI><DIV onClick=""accordionlidiv(this,'" & _
                            drParent.Item("vWorkSpaceId").ToString & "'," & _
                            drParent.Item("iNodeId").ToString & "," & drParent.Item("iPeriod").ToString & "," & _
                            drParent.Item("vActivityId").ToString & ",'" & _
                            drParent("cSubjectWiseFlag").ToString() & "','" & _
                            drParent.Item("vNodeDisplayName").ToString() & "');"">" & _
                            drParent.Item("vNodeDisplayName").ToString() & _
                       "</DIV></LI>"
            Next drParent

            str += "</UL>"
            Return str
        Catch ex As Exception
            Return ex.Message
        End Try

    End Function

    <Services.WebMethod()> _
    Public Shared Function getChildActivity(ByVal WorkSpaceId As String, ByVal CTMStatus As Char, ByVal iParentId As Integer, ByVal iPeriod As Integer, ByVal vActivityId As String, ByVal cSubjectWiseFlag As Char, ByVal vNodeDisplayName As String, ByVal Schema As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim str As String = String.Empty
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds_ChildActivity As DataSet = Nothing
        Dim ds_Subject As DataSet = Nothing
        Dim Query_Str As String = String.Empty
        Dim Subjects As String = String.Empty

        Try
            wstr = WorkSpaceId.ToString() + "##" + iParentId.ToString + "##" + Schema.ToString
            If Not objHelp.Proc_GetNodeWithSubjectCount_Archive(wstr, ds_ChildActivity, estr) Then
                Throw New Exception
            End If
            If ds_ChildActivity.Tables(0).Rows.Count = 0 Then
                str += "<UL>"
                str += "<LI ><DIV onClick=""accordionlidivSubject(this,'" & _
                                      WorkSpaceId.ToString & "'," & _
                                      iParentId.ToString & "," & _
                                      iPeriod.ToString & "," & _
                                      vActivityId.ToString & ",'" & _
                                      cSubjectWiseFlag.ToString() & "');"">" & _
                                     vNodeDisplayName.ToString & "</DIV></LI>"
                str += "</UL>"

                'Else
            End If

            If ds_ChildActivity.Tables(0).Rows.Count > 0 Then
                str += "<UL>"
                For Each drChild As DataRow In ds_ChildActivity.Tables(0).Rows
                    'If drChild.Item("cSubjectWiseFlag") = "Y" Then
                    '    If drChild.Item("iNodeId").ToString = iParentId.ToString Then
                    str += "<LI ><DIV onClick=""accordionlidivSubject(this,'" & _
                                    drChild.Item("vWorkSpaceId").ToString & "'," & _
                                    drChild.Item("iNodeId") & "," & _
                                    drChild.Item("iPeriod") & "," & _
                                    drChild.Item("vActivityId") & ",'" & _
                                    drChild.Item("cSubjectWiseFlag").ToString() & "');"">" & _
                                    Replace(drChild.Item("vNodeDisplayName").ToString(), "*", " ") & _
                                     "</DIV></LI>"
                    '            Else
                    '        str += "<LI ><DIV onClick=""accordionlidivSubject(this,'" & _
                    '                        drChild.Item("vWorkSpaceId").ToString & "'," & _
                    '                        drChild.Item("iNodeId") & "," & _
                    '                        drChild.Item("iPeriod") & "," & _
                    '                        drChild.Item("vActivityId") & ",'" & _
                    '                        drChild.Item("cSubjectWiseFlag").ToString() & "');"">" & _
                    '                        Replace(drChild.Item("vNodeDisplayName").ToString(), "*", " ") & _
                    '                         "</DIV></LI>"
                    '            End If

                    '        Else
                    'str += "<LI ><DIV onClick=""accordionlidivSubject(this,'" & _
                    '                    drChild.Item("vWorkSpaceId").ToString & "'," & _
                    '                     drChild.Item("iNodeId") & "," & _
                    '                    drChild.Item("iPeriod") & "," & _
                    '                    drChild.Item("vActivityId") & ",'" & _
                    '                    drChild.Item("cSubjectWiseFlag").ToString() & "');"">" & _
                    '                    Replace(drChild.Item("vNodeDisplayName").ToString(), "*", " ") & _
                    '                     "</DIV></LI>"
                    '        End If
                Next drChild
                str += "</UL>"
            End If

            If Subjects.Length > 0 Then
                If str.Length < 1 Then
                    Return Subjects.ToString()
                End If
                str = Regex.Replace(str, "<UL>", "")
                Subjects = Regex.Replace(Subjects, "</UL>", "")
                str = Subjects + str
            End If
        Catch ex As Exception

        End Try

        Return str.ToString()
    End Function

    <Services.WebMethod()> _
   Public Shared Function getSubject(ByVal WorkSpaceId As String, ByVal CTMStatus As Char, ByVal iNodeId As String, ByVal iPeriod As String, ByVal vActivityId As String, ByVal cSubjectWiseFlag As Char, ByVal subject As String, ByVal schema As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim str As String = String.Empty
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim Query_Str As String = String.Empty
        Dim ds_Subject As DataSet = Nothing
        Dim clr As String = String.Empty
        Dim BodyClr As String = String.Empty



        Try
            wstr += WorkSpaceId + "##" + iNodeId + "##" + subject + "##" + iPeriod + "##" + cSubjectWiseFlag + "##" + schema
            If Not objHelp.Proc_GetTotalSubjectForNode_Archive(wstr, ds_Subject, estr) Then
                Throw New Exception
            End If
            If ds_Subject.Tables(0).Rows.Count > 0 Then
                str += "<UL>"
                For Each drSubject As DataRow In ds_Subject.Tables(0).Rows
                    If CTMStatus.ToString.ToUpper() = "Y" Then
                        Query_Str = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" & WorkSpaceId & _
                                    "&ActivityId=" & vActivityId & _
                                    "&NodeId=" & iNodeId & _
                                    "&PeriodId=" & iPeriod & _
                                    "&SubjectId=" & drSubject.Item("vSubjectId").ToString & _
                                    "&MySubjectNo=" & drSubject.Item("iMySubjectNo").ToString & _
                                    "&Type=BA-BE" & _
                                    "&ScreenNo=" & drSubject.Item("vMySubjectNo").ToString


                    Else
                        Query_Str = "frmArchiveCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" & WorkSpaceId & _
                                    "&ActivityId=" & vActivityId & _
                                    "&NodeId=" & iNodeId & _
                                    "&PeriodId=" & iPeriod & _
                                    "&SubjectId=" & drSubject.Item("vSubjectId").ToString & _
                                    "&MySubjectNo=" & drSubject.Item("iMySubjectNo").ToString & _
                                    "&Type=BA-BE"
                    End If


                    str += "<LI ><DIV width=""100%"" style=""color:" & _
                            clr & ";" & BodyClr & """ onclick=""funOpen('" & _
                            Query_Str.ToString() & "')"">" & drSubject.Item("vSubjectIdToDispaly").ToString() & _
                            "</DIV></LI>"
                Next
                str += "</UL>"
            End If
        Catch ex As Exception

        End Try
        Return str.ToString()
    End Function

#End Region

#Region "WebMethod for LabData"
    <Services.WebMethod()> _
    Public Shared Function GetLabData(ByVal SchemaId As String, ByVal WorkspaceId As String) As String
        Dim str As String = String.Empty
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim ds As Data.DataSet = Nothing

        Try
            wstr += SchemaId.ToString() + "##" + "0" + "##" + WorkspaceId.ToString()
            If Not objHelp.Proc_SubjectLabRptDtl(wstr, ds, estr) Then
                Throw New Exception
            End If
            If ds Is Nothing Then
                str = 0
                Return str.ToString()
            End If
            str = ds.Tables(0).Rows.Count.ToString()
        Catch ex As Exception

        End Try
        Return str.ToString()
    End Function
#End Region

#Region "Child Projects"
    Public Function GetChildProjects() As Boolean
        Dim eStr As String = String.Empty
        Dim param As String = String.Empty
        Dim Ds_Childs As DataSet = Nothing
        Dim ds_ProjectInfo As DataSet = Nothing
        Dim dt_Subs As New DataTable

        Dim Str As String = String.Empty
        Dim wstr As String = String.Empty
        Dim CountChild As Integer = 0

        Dim rblCTM As RadioButtonList

        Try
            If Not Me.objHelp.Proc_GetProjectChilds(Me.HProjectId.Value.ToString, Ds_Childs, eStr) Then
                Throw New Exception("Problem while getting childs.")
            End If
            If Ds_Childs.Tables(0).Rows.Count <= 0 Then
                Exit Function
            End If

            Str = ""
            Str += " <table cellpadding=""5"">"

            If Me.ChkBClient.Checked = True Then
                Ds_Childs.Tables(0).DefaultView.RowFilter = "vWorkSpaceId <> '" & Me.HProjectId.Value.ToString & "'"
            End If

            rblCTM = New RadioButtonList
            rblCTM.EnableViewState = True
            rblCTM.ID = "rblCTMWorkSpaceId"
            rblCTM.DataSource = Ds_Childs.Tables(0)
            rblCTM.DataValueField = "vWorkspaceId"
            rblCTM.DataTextField = "vProjectNo"
            rblCTM.DataBind()

            Dim sw As New StringWriter()
            Dim htw As New HtmlTextWriter(sw)
            For Each dr As DataRow In Ds_Childs.Tables(0).DefaultView.ToTable().Rows
                CountChild += 1
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'rblCTM.Items.Add(New ListItem(dr("vProjectNo"), dr("vWorkSpaceId")))
                rblCTM.Items(CountChild - 1).Attributes.Add("onClick", "SetValue('" & dr("vProjectNo") & "','" & dr("vWorkSpaceID") & "');")
                Str += "<tr><td>"
                Str += "<fieldset id='fProjectChilds" & CountChild & "' style='width: 850px;'>"
                Str += "<legend id='lProjectChilds" & CountChild & "' >"
                Str += "<img id='img" & CountChild & "' "
                Str += "onclick=""displayChildCTM(this,'Child" & CountChild & "','imgExpand" & CountChild & "','ChildInfo" & CountChild & "');"""
                Str += "alt='SubjectSpecific' src='images/expand.jpg' />"
                Str += "<Label ID='lblProjectChilds" & CountChild & "' runat='server' CssClass='Label'>" & dr("vProjectNo") & "</Label>"
                Str += "</legend>"
                Str += "<div id='Child" & CountChild & "' style='display: none; overflow: none; height: 150px; scrollbar-arrow-color: white; scrollbar-face-color: #7db9e8;'>"

                ''''----------Information : Start

                wstr = "vWorkspaceId = '" + Me.HProjectId.Value + "'"
                If Not objHelp.GetView_ProjectDetails(wstr, ds_ProjectInfo, eStr) Then
                    Throw New Exception(eStr)
                End If

                Str += "<fieldset id='fChildInfo' style='width: 770px; height: auto;margin-left:35px;'>" & _
                           "<legend id='lProjectInfo' runat='server'>" & _
                              "<img id='imgExpand" & CountChild & "' alt='SubjectSpecific' src='images/expand.jpg' onclick='displayChildInfoCTM(this,""ChildInfo" & CountChild & """,""Child" & CountChild & """);'/>" & _
                              "<Label ID='lblProjectInfo' runat='server' CssClass='Label'> Project Information </Label>" & _
                          "</legend>" & _
                            "<div id='ChildInfo" & CountChild & "' style='display: none; margin: 5px;'>"
                If ds_ProjectInfo.Tables(0).Rows.Count > 0 Then
                    Str += "<table width='100%'>" & _
                            "<tr>" & _
                                "<td style='width: 19%'>" & _
                                    "<Label ID='lblmgr' CssClass='Label'>Project Manager : </Label>" & _
                                "</td>" & _
                                "<td>" & _
                                    "<Label ID='lblManager' class='Label' CssClass='labeldisplay'>" & _
                                        ds_ProjectInfo.Tables(0).Rows(0).Item("ProjectManager").ToString().ToUpper() & _
                                    "</Label>" & _
                                "</td>" & _
                                "<td align='right'>" & _
                                    "<Label runat='server' ID='lblPIMonitor' CssClass='Label'/>Monitor</Label>" & _
                                "</td>" & _
                                "<td>" & _
                                    "<Label ID='lblPI' Style='width: 50%' class='Label' runat='server' CssClass='labeldisplay'>" & _
                                                ds_ProjectInfo.Tables(0).Rows(0).Item("ProjectPI").ToString().ToUpper() & _
                                     "</Label>" & _
                                "</td>" & _
                            "</tr>" & _
                            "<tr>" & _
                        "<td align='center' colspan='4'>" & _
                            "<hr style='color: Navy; width: 100%;'/>" & _
                        "</td>" & _
                    "</tr>" & _
                    "<tr>" & _
                        "<td>" & _
                            "<Label runat='server' ID='LblSubs' CssClass='Label'>Subjects : </Label>" & _
                        "</td>" & _
                        "<td colspan='3'>" & _
                            "<table CellPadding = 4 CellSpacing = 6>" & _
                                "<tr CssClass = ""Label"">" & _
                                    "<td> Period </td>" & _
                                    "<td> Total </td>" & _
                                    "<td> Discontinued </td>" & _
                                "</tr>"

                    '''''For subjects information
                    dt_Subs = ds_ProjectInfo.Tables(0).DefaultView().ToTable(True, "TotalSub,iPeriod,Rejected".Split(","))

                    For Each drSub As DataRow In dt_Subs.Rows
                        Str += "<tr CssClass = ""labeldisplay"">" & _
                                     "<td>" & drSub("iPeriod") & "</td>" & _
                                     "<td>" & drSub("TotalSub") & "</td>" & _
                                     "<td>" & drSub("Rejected") & "</td>" & _
                                 "</tr>"
                    Next
                    Str += "</table>" & _
                        "</td>" & _
                        "</tr>" & _
                    "</table>"
                Else
                    Str += "No Information found"
                End If
                Str += "</div>" & _
                        "</fieldset>"
                ''''----------Information : End
                Str += "<fieldset id='fChildsActivity" & CountChild & "' style='width: 770px; height:auto;margin-left:35px;'>"
                Str += "<legend id='lChildsActivity" & CountChild & "' >"
                Str += "<img id='img" & CountChild & "' "
                Str += "onclick=""displayProjectActivityCTM(this,'ProjectActivity" & CountChild & "','Child" & CountChild & "'), "
                Str += "funParentActivityCTM(this,'ProjectActivity" & CountChild & "','" & dr("vWorkspaceId") & "');"""
                Str += "alt='SubjectSpecific' src='images/expand.jpg' />"
                Str += "<Label ID='lblProjectActivity" & CountChild & "' runat='server' CssClass='Label'>Project Activities</Label>"
                Str += "</legend>"
                Str += "<div id='ProjectActivity" & CountChild & "' style='display: none; overflow: auto; height: 390px; scrollbar-arrow-color: white; scrollbar-face-color: #7db9e8;'>"
                Str += "</div>"
                Str += "</fieldset>"


                Str += "</div>"
                Str += "</fieldset>"
                Str += "</td></tr>"
            Next
            Str += "</Table>"
            Me.DvChilds.InnerHtml = Str

            rblCTM.RenderControl(htw)

            Me.DivCTM.InnerHtml = sw.GetStringBuilder().ToString()

            GetChildProjects = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message(), eStr)
            GetChildProjects = False
        End Try
    End Function
#End Region

    Protected Sub Ddlsubject_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Ddlsubject.SelectedIndexChanged
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ddlIndexChange", "ddlIndexChange();", True)
    End Sub
End Class
