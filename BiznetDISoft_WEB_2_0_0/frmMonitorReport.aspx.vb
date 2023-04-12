
Partial Class frmReport
    Inherits System.Web.UI.Page


#Region "Variable Declaration"
    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()

    Private eStr_Retu As String = String.Empty

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

            End If
            If Not Page.IsPostBack Then
                If Not GenCall() Then
                    Exit Sub
                End If
            End If
                        '        Exit Sub
            '    End If
            'If Not Is Then
            '    Me.ChkBClient.Checked = True
            '    If Not GenCall() Then
            '        Exit Sub
            '    End If
            'End If
            Me.imgActivityLegends.Attributes.Add("onmouseover", "$('#" + Me.divActivityLegends.ClientID + "').toggle('medium');")
            Me.divActivityLegends.Attributes.Add("onmouseleave", "$('#" + Me.divActivityLegends.ClientID + "').toggle('medium');")
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr_Retu)
        End Try
    End Sub
#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Try

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
        Dim sender As New Object
        Dim e As EventArgs
        Try
            Page.Title = ":: Monitor Report Page :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Master.FindControl("lblHeading"), Label).Text = "Monitor Report"

            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtProject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
            End If



            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
            GenCall_ShowUI = True
        Catch ex As Exception
            GenCall_ShowUI = False
        End Try

    End Function

#End Region

#Region "Report"
    Private Function ShowReport(ByRef exc As String) As Boolean
        Dim ds_Report As New DataSet
        Dim wstr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim str As String = String.Empty
        Try
            wstr += "vUserTypeCode='" & Session(S_UserType) & "'" & _
             "And  vActivityId in ('0008','0009','0010','0011','0012','0013','0014','0015') And cStatusIndi <> 'D'"
            If Not objHelp.GetData("ActivityOperationMatrix", "*", wstr, _
                                    WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                     ds_Report, eStr) Then
                Throw New Exception(eStr)
            End If
            If ds_Report.Tables(0).Rows.Count <= 0 Then
                Me.tdReport.Style.Add("display", "none")
                Return True
                Exit Function
            End If

            str += "<UL>"
            str += "<li><u class=""Label"" style=""color:White;font-weight: bolder;font-size:17px;"">Reports</u>"
            str += "<img id=""Img2"" title=""Close"" alt=""CLoseImage"" src=""images/close.gif"" runat=""server"" align=""right"""
            str += "style=""padding-right: 5px"" onclick=""funCloseReport();"" />"
            str += "</li>"
            For Each dr As Data.DataRow In ds_Report.Tables(0).Rows
                If dr.Item("vActivityId").ToString = "0008" Then
                    str += "<li onclick=""funReport('frmReportReview.aspx?mode=4&Type=RPT&vWorkSpaceId=','0');"">"
                    str += "Lab Report"
                    str += " </li>"
                    Continue For
                End If
                If dr.Item("vActivityId").ToString = "0009" Then
                    str += "<li onclick=""funReport('frmSourceQA.aspx?Mode=4&Type=RPT&Act=QA on PIF&vActivityId= 1186&WorkSpaceId=','2');"">"
                    str += "PIF"
                    str += " </li>"
                    Continue For
                End If
                If dr.Item("vActivityId").ToString = "0010" Then
                    str += "<li onclick=""funReport('frmSourceQA.aspx?mode=4&Type=RPT&Act=QA on MSR&vActivityId= 1185&WorkSpaceId=','1');"">"
                    str += "Screening"
                    str += " </li>"
                    Continue For
                End If
                If dr.Item("vActivityId").ToString = "0011" Then
                    str += "<li onclick=""funReport('frmWorkspaceSubjectMst.aspx?mode=4&Type=RPT&WorkSpaceId=','0');"">"
                    str += "Attendance"
                    str += " </li>"
                    Continue For
                End If
                If dr.Item("vActivityId").ToString = "0012" Then
                    str += " <li onclick=""funReport('frmCTMCRFReport.aspx?mode=4&Type=RPT&vWorkSpaceId=','0');"">"
                    str += "Listing Report"
                    str += " </li>"
                    Continue For
                End If
                If dr.Item("vActivityId").ToString = "0013" Then
                    str += " <li onclick=""funReport('frmActivityDeviationReport.aspx?mode=4&vWorkSpaceId=','0');"">"
                    str += " Activity Deviation Report"
                    str += " </li>"
                    Continue For
                End If
                If dr.Item("vActivityId").ToString = "0014" Then
                    str += " <li onclick=""funReport('frmEditChecksReport.aspx?mode=4&vWorkSpaceId=','0');"">"
                    str += "Edit Check Execution"
                    str += " </li>"
                    Continue For
                End If
                If dr.Item("vActivityId").ToString = "0015" Then
                    str += "<li onclick=""funReport('frmCTMDiscrepancyStatusReport.aspx?mode=4&vWorkSpaceId=','0');"">"
                    str += "Discrepancy Report"
                    str += " </li>"
                    Continue For
                End If
            Next
            str += " </ul>"
            Me.menu.InnerHtml = str.ToString()
            exc = ""
            Return True
        Catch ex As Exception
            exc = ex.ToString
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function
#End Region

#Region "ButtonEvents"
    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click

        Dim wstr As String = String.Empty
        Dim ds_ProjectInfo As New DataSet
        Dim ds_Report As DataSet = Nothing
        Dim Ds_PiName As New DataSet
        Dim dt_Subs As New DataTable
        Dim estr As String = String.Empty
        Dim str As String = String.Empty
        Try
            If Not ShowReport(estr) Then
                Throw New Exception(estr)
            End If
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                Me.MonitorInfo.Style.Add("display", " ")
                If Not GetChildProjects() Then
                    Me.tdReport.Style.Add("display", "none")
                    Exit Sub
                End If
            End If
            wstr += "vWorkspaceId = '" + Me.HProjectId.Value + "'"
            If Not objHelp.GetView_ProjectDetails(wstr, ds_ProjectInfo, estr) Then
                Throw New Exception
            End If

            If ds_ProjectInfo.Tables(0).Rows.Count < 1 Then
                Throw New Exception("Project Information Not Found.")
                Exit Sub
            End If
            Me.MonitorInfo.Style.Add("display", " ")
            'Me.lblManager.Text = ds_ProjectInfo.Tables(0).Rows(0).Item("ProjectManager").ToString().ToUpper()
            'Me.lblPIMonitor.Text = "PI : "
            'If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
            '    Me.lblPIMonitor.Text = " Monitor : "
            'End If

            'Me.lblPI.Text = ds_ProjectInfo.Tables(0).Rows(0).Item("ProjectPI").ToString().ToUpper()
            ' '' Added By Dipen Shah for Disply PI name.
            'If ds_ProjectInfo.Tables(0).Rows(0).Item("vPiName").ToString() <> "" Then
            '    Me.lblPI.Text = ds_ProjectInfo.Tables(0).Rows(0).Item("vPiName").ToString.ToUpper()
            'End If
            '''''For subjects information
            dt_Subs = ds_ProjectInfo.Tables(0).DefaultView().ToTable(True, "TotalSub,iPeriod,Rejected".Split(","))

            Dim table As New HtmlTable
            table.CellPadding = 4
            table.CellSpacing = 6

            Dim rowHeader As New HtmlTableRow

            Dim cellHeadPeriod As New HtmlTableCell
            Dim cellHeadSubs As New HtmlTableCell
            Dim cellHeadDiscon As New HtmlTableCell

            Dim labeHeadPeriod As New Label
            labeHeadPeriod.Text = "Period"
            labeHeadPeriod.CssClass = "Label"

            Dim labelHeadSubs As New Label
            labelHeadSubs.Text = "Total"
            labelHeadSubs.CssClass = "Label"

            Dim labelHeadDiscon As New Label
            labelHeadDiscon.Text = "Discontinued"
            labelHeadDiscon.CssClass = "Label"

            cellHeadPeriod.Controls.Add(labeHeadPeriod)
            cellHeadSubs.Controls.Add(labelHeadSubs)
            cellHeadDiscon.Controls.Add(labelHeadDiscon)

            rowHeader.Cells.Add(cellHeadPeriod)
            rowHeader.Cells.Add(cellHeadSubs)
            rowHeader.Cells.Add(cellHeadDiscon)

            table.Rows.Add(rowHeader)

            For Each drSub As DataRow In dt_Subs.Rows
                Dim row As New HtmlTableRow
                Dim cellPeriod As New HtmlTableCell
                Dim cellSubs As New HtmlTableCell
                Dim cellDiscon As New HtmlTableCell

                Dim labePeriod As New Label
                labePeriod.Text = IIf(drSub("iPeriod").ToString() <> DBNull.Value.ToString(), drSub("iPeriod"), "")
                labePeriod.CssClass = "labeldisplay"

                Dim labelSubs As New Label
                labelSubs.Text = IIf(drSub("TotalSub").ToString() <> DBNull.Value.ToString(), drSub("TotalSub"), "")
                labelSubs.CssClass = "labeldisplay"

                Dim labelDiscon As New Label
                labelDiscon.Text = IIf(drSub("Rejected").ToString() <> DBNull.Value.ToString(), drSub("Rejected"), "")
                labelDiscon.CssClass = "labeldisplay"

                cellPeriod.Controls.Add(labePeriod)
                cellPeriod.Align = "center"

                cellSubs.Controls.Add(labelSubs)
                cellSubs.Align = "center"

                cellDiscon.Controls.Add(labelDiscon)
                cellDiscon.Align = "center"

                row.Cells.Add(cellPeriod)
                row.Cells.Add(cellSubs)
                row.Cells.Add(cellDiscon)

                table.Rows.Add(row)

            Next

            PlaceSubsInfo.Controls.Add(table)
            If Me.HCTMStatus.Value <> "Y" Then
                wstr = String.Empty
                wstr += "vWorkSpaceId =" & Me.HProjectId.Value & " and vActivityId in ('1185','1186') order by vActivityId "
                If Not objHelp.GetViewWorkSpaceNodeDetail(wstr, ds_Report, eStr_Retu) Then
                    Throw New Exception
                End If
                If ds_Report.Tables(0).Rows.Count > 0 Then
                    Me.HQAONMSR.Value = ds_Report.Tables(0).Rows(0).Item("iNodeId").ToString
                    Me.HQAONPIF.Value = ds_Report.Tables(0).Rows(1).Item("iNodeId").ToString
                End If
            End If
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
    Public Shared Function GetProc_TreeViewOfNodes(ByVal WorkSpaceId As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim Ds_ParentActivity As DataSet = Nothing
        Dim str As String = String.Empty
        Dim eStr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim Query_Str As String = String.Empty
        Dim DivName As String = String.Empty
        Try
            If Not objHelp.Proc_GetNodeWithSubjectCount(WorkSpaceId.ToString(), 1, Ds_ParentActivity, eStr) Then
                Throw New Exception
            End If
            str += "<UL ID=""accordion"">"
            For Each drParent As DataRow In Ds_ParentActivity.Tables(0).Rows

                'If drParent("cSubjectWiseFlag").ToString = "N" AndAlso Convert.ToInt32(drParent("ChildNodeTotal")) < 1 Then
                '    'If CTMStatus.ToString() = "Y" Then
                '    '    Query_Str = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" & WorkSpaceId & _
                '    '            "&ActivityId=" & drParent.Item("vActivityId").ToString() & _
                '    '            "&NodeId=" & drParent.Item("iNodeId").ToString() & _
                '    '            "&PeriodId=" & drParent.Item("iPeriod") & _
                '    '            "&Type=BA-BE&SubjectId=0000&ScreenNo=0000" & _
                '    '            "&mode=4&MySubjectNo=0000"
                '    'Else
                '    Query_Str = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" & WorkSpaceId & _
                '                "&ActivityId=" & drParent.Item("vActivityId").ToString() & _
                '                "&NodeId=" & drParent.Item("iNodeId").ToString() & _
                '                "&PeriodId=" & drParent.Item("iPeriod") & _
                '                "&Type=BA-BE&SubjectId=0000&SubjectNo=0000" & _
                '                "&mode=4&MySubjectNo=0000"
                '    'End If
                '    str += "<LI ><DIV onclick=""funOpen('" & Query_Str.ToString() & "')"">" & _
                '            Replace(drParent.Item("vNodeDisplayName").ToString(), "*", " ") & "</DIV></LI>"
                'ElseIf Convert.ToInt32(drParent("ChildNodeTotal")) > 0 Then 'If drParent("cSubjectWiseFlag").ToString = "Y" Then
                'DivName = "Divaccordionlidiv" + drParent.Item("iNodeId").ToString
                str += "<LI><DIV  onClick=""accordionlidiv(this,'" & _
                            drParent.Item("vWorkSpaceId").ToString & "'," & _
                            drParent.Item("iNodeId").ToString & "," & drParent.Item("iPeriod").ToString & "," & _
                            drParent.Item("vActivityId").ToString & ",'" & drParent.Item("HavingTrmplate").ToString & "','" & _
                            drParent("cSubjectWiseFlag").ToString() & "','" & _
                            drParent.Item("vNodeDisplayName").ToString() & "');"">" & _
                            drParent.Item("vNodeDisplayName").ToString().Split("[")(0) & _
                       "</DIV></LI>"
                ' End If

            Next drParent
            str += "</UL>"
            Return str
        Catch ex As Exception
            Return ex.Message
        End Try

    End Function

    <Services.WebMethod()> _
    Public Shared Function getChildActivity(ByVal WorkSpaceId As String, ByVal CTMStatus As Char, ByVal iParentId As Integer, ByVal iPeriod As Integer, ByVal vActivityId As String, ByVal HavingTrmplate As String, ByVal cSubjectWiseFlag As Char, ByVal vNodeDisplayName As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim str As String = String.Empty
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds_ChildActivity As DataSet = Nothing
        Dim Ds_ParentActivity As DataSet = Nothing
        Dim ds_Subject As DataSet = Nothing
        Dim Query_Str As String = String.Empty
        Dim Subjects As String = String.Empty
        Dim DivName As String = String.Empty
        Try
            If Not objHelp.Proc_GetNodeWithSubjectCount(WorkSpaceId.ToString(), iParentId.ToString, ds_ChildActivity, estr) Then
                Throw New Exception
            End If


            If ds_ChildActivity.Tables(0).Rows.Count = 0 Then
                str += "<UL>"
                DivName = "accordionlidivSubject" '+iParentId.ToString
                str += "<LI ><DIV id='" + DivName + "' runat='server' onClick=""accordionlidivSubject(this,'" & _
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
                    'If drChild.Item("iNodeId").ToString = iParentId.ToString Then
                    'str += "<LI ><DIV onClick=""accordionlidivSubject(this,'" & _
                    '                drChild.Item("vWorkSpaceId").ToString & "'," & _
                    '                drChild.Item("iNodeId") & "," & _
                    '                drChild.Item("iPeriod") & "," & _
                    '                drChild.Item("vActivityId") & ",'" & _
                    '                drChild.Item("cSubjectWiseFlag").ToString() & "');"">" & _
                    '                Replace(drChild.Item("vNodeDisplayName").ToString(), "*", " ") & _
                    '                  " [" & drChild.Item("SubEntered").ToString() & "/" & _
                    '                drChild.Item("TotalSubjects").ToString() & "]" & _
                    '                 "</DIV></LI>"
                    'Else
                    'str += "<LI ><DIV onClick=""accordionlidivSubject(this,'" & _
                    '                drChild.Item("vWorkSpaceId").ToString & "'," & _
                    '                drChild.Item("iNodeId") & "," & _
                    '                drChild.Item("iPeriod") & "," & _
                    '                drChild.Item("vActivityId") & ",'" & _
                    '                drChild.Item("cSubjectWiseFlag").ToString() & "');"">" & _
                    '                Replace(drChild.Item("vNodeDisplayName").ToString(), "*", " ") & _
                    '                 "</DIV></LI>"
                    'End If

                    'Else
                    str += "<LI ><DIV onClick=""accordionlidivSubject(this,'" & _
                                        drChild.Item("vWorkSpaceId").ToString & "'," & _
                                         drChild.Item("iNodeId") & "," & _
                                        drChild.Item("iPeriod") & "," & _
                                        drChild.Item("vActivityId") & ",'" & _
                                        drChild.Item("cSubjectWiseFlag").ToString() & "');"">" & _
                                        Replace(drChild.Item("vNodeDisplayName").ToString(), "*", " ") & _
                                        "</DIV></LI>"
                    'End If
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
    Public Shared Function getSubject(ByVal WorkSpaceId As String, ByVal CTMStatus As Char, ByVal iNodeId As String, ByVal iPeriod As String, ByVal vActivityId As String, ByVal cSubjectWiseFlag As Char) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim str As String = String.Empty
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim Query_Str As String = String.Empty
        Dim ds_Subject As DataSet = Nothing
        Dim ds_ChildActivity As DataSet = Nothing
        Dim clr As String = String.Empty
        Dim BodyClr As String = String.Empty

        Try
            wstr += WorkSpaceId.ToString() + "##" + iNodeId.ToString() + "##" + iPeriod.ToString() + "##" + cSubjectWiseFlag.ToString
            If Not objHelp.Proc_GetTotalSubjectForNode(wstr, ds_Subject, estr) Then
                Throw New Exception
            End If
            If Not objHelp.Proc_GetNodeWithSubjectCount(WorkSpaceId.ToString(), iNodeId.ToString(), ds_ChildActivity, estr) Then
                Throw New Exception
            End If


            If ds_Subject.Tables(0).Rows.Count > 0 Then
                str += "<UL>"
                For Each drSubject As DataRow In ds_Subject.Tables(0).Rows

                    BodyClr = String.Empty
                    If Convert.ToString(drSubject("cRejectionFlag")).Trim.ToUpper() = "Y" Then
                        clr = Drawing.ColorTranslator.ToHtml(Drawing.Color.Red)
                    ElseIf Convert.ToString(drSubject("cDataStatus")).Trim.ToUpper() = CRF_DataEntryPending Then
                        clr = Drawing.ColorTranslator.ToHtml(Drawing.Color.Black)
                    ElseIf Convert.ToString(drSubject("cDataStatus")).Trim.ToUpper() = CRF_DataEntry Then
                        clr = Drawing.ColorTranslator.ToHtml(Drawing.Color.Orange)
                    ElseIf Convert.ToString(drSubject("cDataStatus")).Trim.ToUpper() = CRF_Review Then
                        clr = Drawing.ColorTranslator.ToHtml(Drawing.Color.Blue)
                    ElseIf Convert.ToString(drSubject("cDataStatus")).Trim.ToUpper() = CRF_ReviewCompleted Then
                        clr = "#50C000"
                    ElseIf Convert.ToString(drSubject("cDataStatus")).Trim.ToUpper() = CRF_Locked Then
                        clr = "#50C000"
                    End If
                    If CTMStatus.ToString.ToUpper() = "Y" Then
                        Query_Str = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" & WorkSpaceId & _
                                    "&ActivityId=" & vActivityId & _
                                    "&NodeId=" & iNodeId & _
                                    "&PeriodId=" & iPeriod & _
                                    "&SubjectId=" & drSubject.Item("vSubjectId").ToString & _
                                    "&MySubjectNo=" & drSubject.Item("iMySubjectNo").ToString & _
                                    "&mode=4" & _
                                    "&Type=BA-BE" & _
                                    "&ScreenNo=" & drSubject.Item("vMySubjectNo").ToString
                    Else
                        Query_Str = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" & WorkSpaceId & _
                                    "&ActivityId=" & vActivityId & "&NodeId=" & iNodeId & _
                                    "&PeriodId=" & iPeriod & "&SubjectId=" & drSubject.Item("vSubjectId").ToString & _
                                    "&MySubjectNo=" & drSubject.Item("iMySubjectNo").ToString & "&mode=4" & _
                                    "&Type=BA-BE"

                    End If

                    str += "<LI ><DIV  width=""100%"" title=""" + ds_ChildActivity.Tables(0).Rows(0)("vNodeDisplayName").ToString + """ style=""color:" & _
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

    '<Services.WebMethod()> _
    'Public Shared Function getNodeId(ByVal vWorkSpaceId As String) As String
    '    Dim wstr As String = String.Empty
    '    Dim str As String = String.Empty
    '    Dim eStr_Retu As String = String.Empty
    '    Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
    '    Dim ds_NodeID As Data.DataSet = Nothing
    '    Try
    '        wstr += "vWorkSpaceId ='" & vWorkSpaceId.ToString() & "' and vActivityId in ('1185','1186') order by vActivityId "
    '        If Not objHelp.getWorkSpaceNodeDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_NodeID, eStr_Retu) Then
    '            Throw New Exception
    '        End If
    '        If ds_NodeID.Tables(0).Rows.Count > 0 Then
    '            str += ds_NodeID.Tables(0).Rows(0).Item("iNodeId").ToString
    '            str += "##"
    '            str += ds_NodeID.Tables(0).Rows(1).Item("iNodeId").ToString
    '        End If
    '    Catch ex As Exception
    '    End Try
    '    Return str.ToString()
    'End Function

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
                Throw New Exception("Problem While Getting Childs.")
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
                Str += " <table cellpadding=""5""><tr><td>"
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
                Str += "</td></tr><tr><td>"
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
                Str += "</td></tr></table>"


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

End Class
