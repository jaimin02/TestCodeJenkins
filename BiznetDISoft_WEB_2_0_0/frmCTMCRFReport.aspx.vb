
Partial Class frmCTMCRFReport
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "

    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()

    Private Const ReportType_CRF As String = "CRF"
    Private Const ReportType_MSR As String = "MSR"
    Private Const ReportType_PIF As String = "PIF"
    Private Const ReportType_LAB As String = "LAB"
    Private Const VS_Choice As String = "Choice"
    Private Const VS_Condition As String = "Condition"

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            If Not GenCall_ShowUI() Then
                Exit Sub
            End If
            Exit Sub
        End If

    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'Me.Session.Remove("PlaceSearchOptions")
    End Sub

#End Region

#Region "GenCall_ShowUI"

    Private Function GenCall_ShowUI() As Boolean
        Dim sender As New Object
        Dim e As New EventArgs
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try
            Page.Title = " :: Listing Report ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Master.FindControl("lblHeading"), Label).Text = "Listing Report"
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
            Me.btnSearch.Visible = False
            'added on 15-May-2012 by Megha for view mode
            If (Not Me.Request.QueryString("mode") Is Nothing) Then
                Choice = Me.Request.QueryString("mode").ToString
                Me.ViewState(VS_Choice) = Choice
            End If

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                If (Not Me.Request.QueryString("vWorkSpaceId") Is Nothing) AndAlso (Not Me.Request.QueryString("Type") Is Nothing) Then
                    Me.txtproject.Text = Me.Request.QueryString("ProjectNo").ToString
                    Me.txtproject.Enabled = False
                    Me.btnCancel.Visible = False
                    Me.HProjectId.Value = Me.Request.QueryString("vWorkSpaceId").ToString
                    Me.btnSetProject_Click(sender, e)
                    Me.rbtnlstReportTypes.Items(1).Enabled = False
                    Me.rbtnlstReportTypes.Items(2).Enabled = False
                    Me.rbtnlstReportTypes.Items(3).Enabled = False
                    FillChkLstColumns()
                    HideMenu()
                    Exit Function
                End If
            End If

            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                Me.btnSetProject_Click(sender, e)
            End If



            If Not FillChkLstColumns() Then
                Exit Function
            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            ShowErrorMessage("Error While Displaying Data. ", ex.Message)
        Finally
        End Try

    End Function

#End Region

#Region "FillddlPeriod"

    Private Function FillddlPeriod() As Boolean
        Dim eStr As String = String.Empty
        Dim ds As New DataSet
        Dim dv As DataView
        Dim wStr As String = String.Empty

        Try


            wStr = " vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"

            If Not objHelp.GetViewWorkSpaceNodeDetail(wStr, ds, eStr) Then
                Throw New Exception(eStr)
            End If

            dv = ds.Tables(0).DefaultView.ToTable(True, "iPeriod".Split(",")).DefaultView

            Me.ddlPeriod.DataSource = dv.ToTable()
            Me.ddlPeriod.DataTextField = "iPeriod"
            Me.ddlPeriod.DataValueField = "iPeriod"
            Me.ddlPeriod.DataBind()
            Me.ddlPeriod.Items.Insert(0, "All Periods")

            Return True

        Catch ex As Exception
            ShowErrorMessage("Error While Filling Period. ", ex.Message)
            Return False
        End Try

    End Function

#End Region

#Region "HideMenu"

    Private Sub HideMenu()
        Dim tmpMenu As Menu
        Dim tmpddlprofile As DropDownList

        tmpMenu = CType(Me.Master.FindControl("menu1"), Menu)
        tmpddlprofile = CType(Me.Master.FindControl("ddlProfile"), DropDownList)

        If Not tmpMenu Is Nothing Then
            tmpMenu.Visible = False
        End If

        If Not tmpddlprofile Is Nothing Then
            tmpddlprofile.Enabled = False
        End If

    End Sub

#End Region

#Region "FillChkLstSubjects"

    Private Function FillChkLstSubjects() As Boolean
        Dim eStr As String = String.Empty
        Dim ds_Subjects As New DataSet
        Dim wStr As String = String.Empty
        Dim lItems As ListItem

        Try

            wStr = " vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D' And iMySubjectNo > 0 "
            If Me.ddlPeriod.SelectedIndex.ToString.Trim() > 0 Then
                wStr += " And iPeriod = " + Me.ddlPeriod.SelectedValue.Trim()
            Else
                wStr += " And iPeriod = 1 "
            End If
            wStr += " order by iMySubjectNo"

            If Not objHelp.GetViewWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                    ds_Subjects, eStr) Then
                Throw New Exception(eStr)
            End If
            Me.chklstSubjects.Items.Clear()
            'ds_Subjects.Tables(0).Columns.Add("InitialsWithScreenNo")
            'ds_Subjects.AcceptChanges()

            For Each dr As DataRow In ds_Subjects.Tables(0).Rows
                lItems = New ListItem
                lItems.Text = Convert.ToString(dr("vMySubjectNo")).Trim() + " - " + Convert.ToString(dr("vInitials")).Trim()
                lItems.Value = dr("vSubjectId")
                'If dr("cRejectionFlag") = "Y" Then
                '    lItems.Attributes.Add("style", "color:red")
                'End If
                Me.chklstSubjects.Items.Add(lItems)
            Next
            'ds_Subjects.AcceptChanges()

            'Me.chklstSubjects.DataSource = ds_Subjects.Tables(0)
            'Me.chklstSubjects.DataTextField = "InitialsWithScreenNo"
            'Me.chklstSubjects.DataValueField = "vSubjectId"

            'If Me.rbtnlstReportTypes.SelectedValue.ToUpper() = ReportType_MSR Then
            '    Me.chklstSubjects.DataValueField = "nMedExScreeningHdrNo"
            'End If

            'Me.chklstSubjects.DataBind()

            Return True

        Catch ex As Exception
            ShowErrorMessage("Error While Filling Subjects. ", ex.Message)
            Return False
        End Try

    End Function

#End Region

#Region "FillChkLstColumns"

    Private Function FillChkLstColumns() As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim Pameter As String = String.Empty
        Dim ds_Columns As New DataSet

        Try


            Pameter = "View_CRFDetailedReport"
            wStr = " SysColumns.Name not in ('vWorkSpaceId','vSubjectId')"

            If Me.rbtnlstReportTypes.SelectedValue.ToUpper() = ReportType_MSR Then
                Pameter = "View_MedExScreeningHdrDtl_ForReport" 'added by vishal
                wStr = " SysColumns.Name not in ('nMedExWorkSpaceDtlNo','nMedExScreeningHdrNo','vWorkSpaceId','iSeqNo','vSubjectId')"

            ElseIf Me.rbtnlstReportTypes.SelectedValue.ToUpper() = ReportType_PIF Then
                Pameter = "View_PIFSubjectMaster_ForReport"
                wStr = " SysColumns.Name not in ('WorkSpaceId')"

            ElseIf Me.rbtnlstReportTypes.SelectedValue.ToUpper() = ReportType_LAB Then
                Pameter = "View_SubjectLabRptDtl_ForReport"
                wStr = " SysColumns.Name not in ('vWorkSpaceId','vSubjectId','nMedExScreeningHdrNo')"

            End If

            If Not objHelp.GetColumnNamesWithWhereCondition(Pameter, wStr, ds_Columns, eStr) Then
                Throw New Exception(eStr)
            End If

            Me.chklstColumns.DataSource = ds_Columns.Tables(0)
            Me.chklstColumns.DataTextField = "ColumnName"
            Me.chklstColumns.DataValueField = "ColumnName"
            Me.chklstColumns.DataBind()

            Return True

        Catch ex As Exception
            ShowErrorMessage("Error While Filling Fields. ", ex.Message)
            Return False
        End Try

    End Function

#End Region

#Region "FillGrid"

    Private Function FillGrid() As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim Columns As String = String.Empty
        Dim PerameterView As String = String.Empty
        Dim Subjects As String = String.Empty
        Dim ds_CRFDtl As New DataSet

        Try

            gvwCRFDtl.DataSource = Nothing

            For index As Integer = 0 To Me.chklstColumns.Items.Count - 1
                If Me.chklstColumns.Items(index).Selected Then
                    If Columns.Trim() <> "" OrElse Columns <> String.Empty Then
                        Columns += ","
                    End If
                    Columns += "[" + Me.chklstColumns.Items(index).Value.Trim() + "]"
                End If
            Next

            For index As Integer = 0 To Me.chklstSubjects.Items.Count - 1
                If Me.chklstSubjects.Items(index).Selected Then
                    If Subjects.Trim() <> "" OrElse Subjects <> String.Empty Then
                        If Me.chklstSubjects.Items(index).Value.Trim() <> "" Then
                            Subjects += ","
                        End If
                    End If
                    If Me.chklstSubjects.Items(index).Value.Trim() <> "" Then
                        Subjects += "'" + Me.chklstSubjects.Items(index).Value.Trim() + "'"
                    End If
                End If
            Next

            If Me.chkGenericActivities.Checked And Subjects.Trim() <> "" Then
                Subjects += ",'0000'"
            ElseIf Me.chkGenericActivities.Checked And Subjects.Trim() = "" Then
                Subjects += "'0000'"
            End If

            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
            wStr += " And vSubjectId in (" + Subjects + ")"

            If Me.rbtnlstReportTypes.SelectedValue.ToUpper() = ReportType_CRF AndAlso _
                                                                          Me.ddlPeriod.SelectedIndex <> 0 Then
                wStr += " And Period = " + Me.ddlPeriod.SelectedValue.ToString()
            End If

            If Me.rbtnlstReportTypes.SelectedValue.ToUpper() = ReportType_CRF Then
                PerameterView = "View_CRFDetailedReport"
                If Me.rbtnlstOptions.Items(1).Selected Then
                    PerameterView = "View_CRFDetailedReport_WithAuditTrail"
                End If

            ElseIf Me.rbtnlstReportTypes.SelectedValue.ToUpper() = ReportType_MSR Then
                wStr = "vSubjectId in (" + Subjects + ")"
                PerameterView = "View_MedExScreeningHdrDtl_ForReport"

            ElseIf Me.rbtnlstReportTypes.SelectedValue.ToUpper() = ReportType_PIF Then
                wStr = " SubjectIdNo in (" + Subjects + ")"
                PerameterView = "View_PIFSubjectMaster_ForReport"

            ElseIf Me.rbtnlstReportTypes.SelectedValue.ToUpper() = ReportType_LAB Then
                PerameterView = "View_SubjectLabRptDtl_ForReport"

            End If

            If Not Me.ViewState(VS_Condition) Is Nothing Then
                wStr += " And " + Me.ViewState(VS_Condition)
            End If

            Me.objHelp.Timeout = 300000
            If Not Me.objHelp.GetFieldsOfTable(PerameterView, Columns, wStr, ds_CRFDtl, eStr) Then
                Throw New Exception(eStr)
            End If

            'For index As Integer = 0 To ds_CRFDtl.Tables(0).Columns.Count - 1
            '    ds_CRFDtl.Tables(0).Columns(index).ColumnName = Replace(ds_CRFDtl.Tables(0).Columns(index).ColumnName, " ", "").Trim()
            'Next index
            'ds_CRFDtl.AcceptChanges()

            gvwCRFDtl.DataSource = Nothing
            gvwCRFDtl.DataBind()

            Me.btnSearch.Visible = False
            Me.btnExportToExcel.Visible = False

            If Me.rbtnlstGridOptions.Items(1).Selected Then
                If Not BindGrid(ds_CRFDtl.Tables(0)) Then
                    Throw New Exception()
                End If
            End If

            Me.CreateSearchControls(ds_CRFDtl.Tables(0))
            Return True
        Catch ex As Exception
            ShowErrorMessage("Error While Filling Grid. ", ex.Message)
            Return False
        End Try

    End Function

    Private Function BindGrid(ByVal dt As DataTable) As Boolean

        Try


            gvwCRFDtl.DataSource = Nothing

            gvwCRFDtl.DataSource = dt
            gvwCRFDtl.DataBind()

            Me.btnSearch.Visible = False
            Me.btnExportToExcel.Visible = False
            If dt.Rows.Count > 0 Then
                Me.btnSearch.Visible = True
                Me.btnExportToExcel.Visible = True
            End If

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvwCRFDtl", "UIgvwCRFDtl(); ", True)

            Return True

        Catch ex As Exception
            ShowErrorMessage("Error While Binding Grid. ", ex.Message)
            Return False
        End Try

    End Function

#End Region

#Region "Helper Sub & Functions"

    Private Sub CreateSearchControls(ByVal dt_CRFDtl As DataTable)
        Dim dv_CRFDtl As DataView
        Dim eStr As String = String.Empty
        Dim index As Integer = 0
        Dim strHdr As String = String.Empty
        Dim ColumnName As String = String.Empty
        Dim count As Integer = 0
        Dim indexes() As Integer

        Try


            If dt_CRFDtl Is Nothing Then
                Exit Sub
            End If

            PlaceSearchOptions.Controls.Clear()

            If Me.rbtnlstGridOptions.Items(0).Selected Then

                PlaceSearchOptions.Controls.Add(New LiteralControl("<Table border=""1"" id=""test"" class=""display"" style=""width: 100%"">"))
                PlaceSearchOptions.Controls.Add(New LiteralControl("<thead>"))
                PlaceSearchOptions.Controls.Add(New LiteralControl("<tr>"))

                For index = 0 To Me.chklstColumns.Items.Count - 1

                    If Me.chklstColumns.Items(index).Selected Then

                        'strHdr = dt_CRFDtl.Columns(Me.chklstColumns.Items(index).Value.Trim.Replace(" ", "")).ColumnName.Trim()
                        strHdr = dt_CRFDtl.Columns(Me.chklstColumns.Items(index).Value.Trim()).ColumnName.Trim()
                        PlaceSearchOptions.Controls.Add(New LiteralControl("<th style=""width: 100%"">" + strHdr + "</th>"))

                        ReDim Preserve indexes(count)
                        indexes(count) = index
                        count += 1

                    End If

                Next

                PlaceSearchOptions.Controls.Add(New LiteralControl("</tr>"))
                PlaceSearchOptions.Controls.Add(New LiteralControl("</thead>"))
                PlaceSearchOptions.Controls.Add(New LiteralControl("<tbody>"))

                For index = 0 To dt_CRFDtl.Rows.Count - 1

                    PlaceSearchOptions.Controls.Add(New LiteralControl("<Tr class=""gradeA"" ALIGN=CENTER>"))

                    For innerindex As Integer = 0 To indexes.Length - 1

                        'ColumnName = Me.chklstColumns.Items(indexes(innerindex)).Text.Trim.Replace(" ", "")
                        ColumnName = Me.chklstColumns.Items(indexes(innerindex)).Text.Trim()

                        PlaceSearchOptions.Controls.Add(New LiteralControl("<Td ALIGN=LEFT style=""width: 0%"">" + _
                                                        IIf(Convert.ToString(dt_CRFDtl.Rows(index)(ColumnName)).Trim() = "", "&nbsp", _
                                                            dt_CRFDtl.Rows(index)(ColumnName).ToString())))
                        PlaceSearchOptions.Controls.Add(New LiteralControl("</Td>"))

                    Next

                    PlaceSearchOptions.Controls.Add(New LiteralControl("</Tr>"))

                Next index

                PlaceSearchOptions.Controls.Add(New LiteralControl("</tbody>"))
                PlaceSearchOptions.Controls.Add(New LiteralControl("</Table>"))

            Else

                '**************For dynamic drop down filters***************

                PlaceSearchOptions.Controls.Add(New LiteralControl("<Table border=""1"" style=""width: 100%"" id=""test1"">"))
                PlaceSearchOptions.Controls.Add(New LiteralControl("<tr>"))
                PlaceSearchOptions.Controls.Add(New LiteralControl("<td style=""width: 100%"">"))

                For index = 0 To dt_CRFDtl.Columns.Count - 1

                    Dim ddl As New DropDownList
                    ddl.ID = dt_CRFDtl.Columns(index).ColumnName.Replace(" ", "").Replace("/", "").Replace(".", "")

                    dv_CRFDtl = dt_CRFDtl.DefaultView.ToTable(True, dt_CRFDtl.Columns(index).ColumnName.Split(",")).DefaultView
                    ddl.DataSource = dv_CRFDtl.ToTable()
                    ddl.DataTextField = dt_CRFDtl.Columns(index).ColumnName
                    ddl.DataValueField = dt_CRFDtl.Columns(index).ColumnName
                    ddl.DataBind()
                    ddl.Items.Insert(0, "Select " + dt_CRFDtl.Columns(index).ColumnName.Trim())
                    ddl.SelectedIndex = 0
                    ddl.Attributes.Add("style", "width:18%")
                    'ddl.CssClass = "DynamicDDL"
                    For cnt As Integer = 0 To ddl.Items.Count - 1
                        ddl.Items(cnt).Attributes.Add("title", ddl.Items(cnt).Text)
                    Next cnt
                    PlaceSearchOptions.Controls.Add(ddl)
                    PlaceSearchOptions.Controls.Add(New LiteralControl("&nbsp"))
                    'ddl.AutoPostBack = True

                Next index

                PlaceSearchOptions.Controls.Add(New LiteralControl("</td>"))
                PlaceSearchOptions.Controls.Add(New LiteralControl("</tr>"))
                PlaceSearchOptions.Controls.Add(New LiteralControl("</Table>"))

                '*****************************************
            End If

            Me.Session("PlaceSearchOptions") = PlaceSearchOptions.Controls

        Catch ex As Exception
            ShowErrorMessage("Error While Creating Filters. ", ex.Message)
        End Try
    End Sub

    Private Function ConvertDsTO(ByVal ds As DataSet) As String
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer

        Try


            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            strMessage.Append("<td><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append("Site")
            strMessage.Append("</font></strong></td>")
            strMessage.Append("<td colspan=""4""><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append(Me.txtproject.Text.Trim())
            strMessage.Append("</font></strong></td>")
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")
            For iCol = 0 To ds.Tables(0).Columns.Count - 1
                If ds.Tables(0).Columns(iCol).ToString <> "TranNo" Then
                    strMessage.Append("<td><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(ds.Tables(0).Columns(iCol).ToString)
                    strMessage.Append("</font></strong></td>")
                End If
            Next
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")

            For j = 0 To ds.Tables(0).Rows.Count - 1
                strMessage.Append("<tr>")
                For i = 0 To ds.Tables(0).Columns.Count - 1
                    If ds.Tables(0).Columns(i).ToString <> "TranNo" Then
                        If IsDBNull(ds.Tables(0).Rows(j).Item(i)) = False Then
                            If (CType(IIf(IsDBNull(ds.Tables(0).Rows(j).Item(i)) = True, "", ds.Tables(0).Rows(j).Item(i)), String) = "N") Then
                                strMessage.Append("<td><strong><font color=""green"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                            Else
                                strMessage.Append("<td><strong><font color=""#000000"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                            End If
                        Else
                            strMessage.Append("<td><strong><font color=""red"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                        End If
                        If ds.Tables(0).Columns(i).ToString.Trim.Contains("Remark Value") Then
                            ' Dim rtf As New Windows.Forms.RichTextBox()
                            ' rtf.Rtf = ds.Tables(0).Rows(j).Item(i).ToString.Trim
                            ' 'strMessage.Append(rtf.Text.Trim)
                            ' rtf.Dispose()
                        Else
                            strMessage.Append(ds.Tables(0).Rows(j).Item(i).ToString())
                        End If

                        strMessage.Append("</font></strong></td>")

                    End If
                Next
                strMessage.Append("</tr>")
            Next
            strMessage.Append("</table>")

            Return strMessage.ToString

        Catch ex As Exception
            ShowErrorMessage("Error While Exporting Data. ", ex.Message)
            Return ""
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

#Region "Button Events"

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.ResetPage()
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_Check As New DataSet
        Dim dv_Check As New DataView
        Try


            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D' Order by iTranNo desc"

            If Not Me.objHelp.GetCRFLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_Check, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_Check Is Nothing Then

                dv_Check = ds_Check.Tables(0).DefaultView
                'dv_Check.Sort = "iTranNo desc"

                If dv_Check.ToTable().Rows.Count > 0 Then

                    If Convert.ToString(dv_Check.ToTable().Rows(0)("cLockFlag")).Trim.ToUpper() = "L" Then
                        Me.objcommon.ShowAlert("Project/Site Is Locked.", Me.Page)
                        'Me.txtproject.Text = ""
                        'Me.HProjectId.Value = ""
                        'Exit Sub
                    ElseIf Convert.ToString(dv_Check.ToTable().Rows(0)("cLockFlag")).Trim.ToUpper() = "U" Then
                        Me.objcommon.ShowAlert("Project/Site Is Unlocked.", Me.Page)
                    End If

                Else
                    Me.objcommon.ShowAlert("Project/Site Is Unlocked.", Me.Page)
                End If
            Else
                Me.objcommon.ShowAlert("Project/Site Is Unlocked.", Me.Page)
            End If

            If Me.rbtnlstReportTypes.SelectedValue.ToUpper() = ReportType_CRF Then
                If Not Me.FillddlPeriod() Then
                    Exit Sub
                End If
            End If

            If Not Me.FillChkLstSubjects() Then
                Exit Sub
            End If

            Me.PlaceSearchOptions.Controls.Clear()

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Getting CRF Lock Details ", ex.Message)
        End Try

    End Sub

    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
        Me.ViewState(VS_Condition) = Nothing

        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub

    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim Columns As String = String.Empty
        Dim PerameterView As String = String.Empty
        Dim Subjects As String = String.Empty
        Dim ds_CRFDtl As New DataSet

        Dim fileName As String = String.Empty
        Dim ds As New DataSet

        Dim objCollection As ControlCollection
        Dim objControl As Control
        Dim ObjId As String = String.Empty
        Dim wStrNew As String = String.Empty
        Dim dt As New DataTable
        Dim dv As New DataView
        Dim wStrAll As String = String.Empty
        Dim ddlID As String = String.Empty
        Dim val() As String = Nothing
        Try
            If Me.Session("PlaceSearchOptions") Is Nothing Then
                If Me.gvwCRFDtl.Rows.Count > 0 Then
                    If Not FillGrid() Then
                        Exit Sub
                    End If
                End If
            End If

            gvwCRFDtl.DataSource = Nothing

            objCollection = CType(Me.Session("PlaceSearchOptions"), ControlCollection)

            Dim str As String = hdf_AllDDLControlVal.Value
            '' Added by ketan
            For Each objControl In objCollection

                If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" Then

                    ObjId = objControl.ID.ToString.Trim()

                    wStr = CType(objControl, DropDownList).SelectedItem.Text

                    Dim val1 As String = String.Empty
                    For Each word In Str.Split("##")
                        Val = word.Split("@@")
                        For count = 0 To Val.Length - 1
                            val1 = Val(count)
                            If ObjId = Val(count) Then
                                ObjId = wStr.Replace("Select ", "")
                                wStr = Val(count + 2)
                                If Not wStr Is Nothing Then
                                    If wStr.ToUpper() <> "" Then
                                        If wStrAll.Trim() <> "" Then
                                            wStrAll += " And "
                                        End If
                                        wStrAll += "[" + ObjId.Trim() + "] = '" + wStr + "'"
                                    End If
                                End If
                            End If
                            Exit For
                        Next
                    Next

                End If

            Next objControl
            ''Ended by ketan


            ''Commented by ketan
            'For Each objControl In objCollection

            '    If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" Then

            '        ObjId = objControl.ID.ToString.Trim()

            '        wStrNew = CType(objControl, DropDownList).SelectedItem.Text

            '        If Not wStrNew Is Nothing Then

            '            If wStrNew.ToUpper() <> "" And CType(objControl, DropDownList).SelectedIndex <> 0 Then
            '                If wStrAll.Trim() <> "" Then
            '                    wStrAll += " And "
            '                End If
            '                wStrAll += "[" + ObjId.Trim() + "] = '" + wStrNew + "'"
            '            End If

            '        End If

            '    End If

            'Next objControl

            ''Endeded by ketan


            For index As Integer = 0 To Me.chklstColumns.Items.Count - 1
                If Me.chklstColumns.Items(index).Selected Then
                    If Columns.Trim() <> "" OrElse Columns <> String.Empty Then
                        Columns += ","
                    End If
                    Columns += "[" + Me.chklstColumns.Items(index).Value.Trim() + "]"
                End If
            Next

            For index As Integer = 0 To Me.chklstSubjects.Items.Count - 1
                If Me.chklstSubjects.Items(index).Selected Then
                    If Subjects.Trim() <> "" OrElse Subjects <> String.Empty Then
                        If Me.chklstSubjects.Items(index).Value.Trim() <> "" Then
                            Subjects += ","
                        End If
                    End If
                    If Me.chklstSubjects.Items(index).Value.Trim() <> "" Then
                        Subjects += "'" + Me.chklstSubjects.Items(index).Value.Trim() + "'"
                    End If
                End If
            Next

            If Me.chkGenericActivities.Checked And Subjects.Trim() <> "" Then
                Subjects += ",'0000'"
            ElseIf Me.chkGenericActivities.Checked And Subjects.Trim() = "" Then
                Subjects += "'0000'"
            End If

            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
            wStr += " And vSubjectId in (" + Subjects + ")"

            If Me.rbtnlstReportTypes.SelectedValue.ToUpper() = ReportType_CRF AndAlso _
                                                                          Me.ddlPeriod.SelectedIndex <> 0 Then
                wStr += " And Period = " + Me.ddlPeriod.SelectedValue.ToString()
            End If

            If Me.rbtnlstReportTypes.SelectedValue.ToUpper() = ReportType_CRF Then
                PerameterView = "View_CRFDetailedReport"
                If Me.rbtnlstOptions.Items(1).Selected Then
                    PerameterView = "View_CRFDetailedReport_WithAuditTrail"
                End If

            ElseIf Me.rbtnlstReportTypes.SelectedValue.ToUpper() = ReportType_MSR Then
                wStr = "vSubjectId in (" + Subjects + ")"
                PerameterView = "View_MedExScreeningHdrDtl_ForReport"

            ElseIf Me.rbtnlstReportTypes.SelectedValue.ToUpper() = ReportType_PIF Then
                wStr = " SubjectIdNo in (" + Subjects + ")"
                PerameterView = "View_PIFSubjectMaster_ForReport"

            ElseIf Me.rbtnlstReportTypes.SelectedValue.ToUpper() = ReportType_LAB Then
                PerameterView = "View_SubjectLabRptDtl_ForReport"

            End If

            If wStrAll.Trim() <> "" Then
                wStr += " And " + wStrAll
            End If

            If Not Me.ViewState(VS_Condition) Is Nothing Then
                wStr += " And " + Me.ViewState(VS_Condition)
            End If

            Me.objHelp.Timeout = 300000
            If Not Me.objHelp.GetFieldsOfTable(PerameterView, Columns, wStr, ds_CRFDtl, eStr) Then
                Throw New Exception(eStr)
            End If

            'For index As Integer = 0 To ds_CRFDtl.Tables(0).Columns.Count - 1
            '    ds_CRFDtl.Tables(0).Columns(index).ColumnName = Replace(ds_CRFDtl.Tables(0).Columns(index).ColumnName, " ", "").Trim()
            'Next index
            'ds_CRFDtl.AcceptChanges()

            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            fileName = "CRFReport"
            fileName = fileName & ".xls"
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            Context.Response.Write(ConvertDsTO(ds_CRFDtl))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)


        Catch ex As Exception
            ShowErrorMessage("Error While Exporting Data. ", ex.Message)
        End Try

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim objCollection As ControlCollection
        Dim objControl As Control
        Dim ObjId As String = String.Empty
        Dim wStr As String = String.Empty
        Dim dt As New DataTable
        Dim dv As New DataView
        Dim wStrAll As String = String.Empty

        Dim eStr As String = String.Empty
        Dim wStrUpdated As String = String.Empty
        Dim Columns As String = String.Empty
        Dim PerameterView As String = String.Empty
        Dim Subjects As String = String.Empty
        Dim ds_CRFDtl As New DataSet

        Dim val() As String = Nothing
        Dim ddlID As String = String.Empty

        Try

            Dim str As String = hdf_AllDDLControlVal.Value

            objCollection = CType(Me.Session("PlaceSearchOptions"), ControlCollection)

            For Each objControl In objCollection

                If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" Then

                    ObjId = objControl.ID.ToString.Trim()

                    wStr = CType(objControl, DropDownList).SelectedItem.Text

                    Dim val1 As String = String.Empty
                    For Each word In str.Split("##")
                        val = word.Split("@@")
                        For count = 0 To val.Length - 1
                            val1 = val(count)
                            If ObjId = val(count) Then
                                ObjId = wStr.Replace("Select ", "")
                                wStr = val(count + 2)
                                If Not wStr Is Nothing Then
                                    If wStr.ToUpper() <> "" Then
                                        If wStrAll.Trim() <> "" Then
                                            wStrAll += " And "
                                        End If
                                        wStrAll += "[" + ObjId.Trim() + "] = '" + wStr + "'"
                                    End If
                                End If
                            End If
                            Exit For
                        Next
                    Next


                    'If Not wStr Is Nothing Then

                    '    If wStr.ToUpper() <> "" And CType(objControl, DropDownList).SelectedIndex <> 0 Then
                    '        If wStrAll.Trim() <> "" Then
                    '            wStrAll += " And "
                    '        End If
                    '        wStrAll += "[" + ObjId.Trim() + "] = '" + wStr + "'"
                    '    End If

                    'End If



                End If

            Next objControl


            gvwCRFDtl.DataSource = Nothing

            For index As Integer = 0 To Me.chklstColumns.Items.Count - 1
                If Me.chklstColumns.Items(index).Selected Then
                    If Columns.Trim() <> "" OrElse Columns <> String.Empty Then
                        Columns += ","
                    End If
                    Columns += "[" + Me.chklstColumns.Items(index).Value.Trim() + "]"
                End If
            Next

            For index As Integer = 0 To Me.chklstSubjects.Items.Count - 1
                If Me.chklstSubjects.Items(index).Selected Then
                    If Subjects.Trim() <> "" OrElse Subjects <> String.Empty Then
                        If Me.chklstSubjects.Items(index).Value.Trim() <> "" Then
                            Subjects += ","
                        End If
                    End If
                    If Me.chklstSubjects.Items(index).Value.Trim() <> "" Then
                        Subjects += "'" + Me.chklstSubjects.Items(index).Value.Trim() + "'"
                    End If
                End If
            Next

            If Me.chkGenericActivities.Checked And Subjects.Trim() <> "" Then
                Subjects += ",'0000'"
            ElseIf Me.chkGenericActivities.Checked And Subjects.Trim() = "" Then
                Subjects += "'0000'"
            End If

            wStrUpdated = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
            wStrUpdated += " And vSubjectId in (" + Subjects + ")"

            If Me.rbtnlstReportTypes.SelectedValue.ToUpper() = ReportType_CRF AndAlso _
                                                                          Me.ddlPeriod.SelectedIndex <> 0 Then
                wStrUpdated += " And Period = " + Me.ddlPeriod.SelectedValue.ToString()
            End If

            If Me.rbtnlstReportTypes.SelectedValue.ToUpper() = ReportType_CRF Then
                PerameterView = "View_CRFDetailedReport"
                If Me.rbtnlstOptions.Items(1).Selected Then
                    PerameterView = "View_CRFDetailedReport_WithAuditTrail"
                End If

            ElseIf Me.rbtnlstReportTypes.SelectedValue.ToUpper() = ReportType_MSR Then
                wStrUpdated = "vSubjectId in (" + Subjects + ")"
                PerameterView = "View_MedExScreeningHdrDtl_ForReport"

            ElseIf Me.rbtnlstReportTypes.SelectedValue.ToUpper() = ReportType_PIF Then
                wStrUpdated = " SubjectIdNo in (" + Subjects + ")"
                PerameterView = "View_PIFSubjectMaster_ForReport"

            ElseIf Me.rbtnlstReportTypes.SelectedValue.ToUpper() = ReportType_LAB Then
                PerameterView = "View_SubjectLabRptDtl_ForReport"

            End If

            If wStrAll.Trim() <> "" Then
                wStrUpdated += " And " + wStrAll
            End If
            If Convert.ToString(wStrUpdated) <> "" Then
                Me.ViewState(VS_Condition) = wStrUpdated
            End If

            Me.objHelp.Timeout = 300000
            If Not Me.objHelp.GetFieldsOfTable(PerameterView, Columns, wStrUpdated, ds_CRFDtl, eStr) Then
                Throw New Exception(eStr)
            End If

            'For index As Integer = 0 To ds_CRFDtl.Tables(0).Columns.Count - 1
            '    ds_CRFDtl.Tables(0).Columns(index).ColumnName = Replace(ds_CRFDtl.Tables(0).Columns(index).ColumnName, " ", "").Trim()
            'Next index
            'ds_CRFDtl.AcceptChanges()

            If Not Me.BindGrid(ds_CRFDtl.Tables(0)) Then
                Throw New Exception
            End If

            If Me.Session("PlaceSearchOptions") Is Nothing Then
                Me.CreateSearchControls(ds_CRFDtl.Tables(0))
            End If

        Catch ex As Exception
            ShowErrorMessage("Error While Searching Data. ", ex.Message)
        End Try

    End Sub

#End Region

#Region "Grid Events"

    Protected Sub gvwCRFDtl_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        gvwCRFDtl.PageIndex = e.NewPageIndex
        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub

#End Region

#Region "ResetPage"

    Private Sub ResetPage()
        Me.chkGenericActivities.Checked = False
        Me.chkSelectAllFields.Checked = False
        Me.chkSelectAllSubjects.Checked = False
        Me.HProjectId.Value = ""
        Me.txtproject.Text = ""
        Me.chklstSubjects.Items.Clear()
        Me.PlaceSearchOptions.Controls.Clear()
        Me.gvwCRFDtl.DataSource = Nothing
        Me.gvwCRFDtl.DataBind()
        Me.btnSearch.Visible = False
        Me.btnExportToExcel.Visible = False
        Me.chklstColumns.ClearSelection()
        Me.ddlPeriod.Items.Clear()
    End Sub

#End Region

#Region "Radiobuttonlist Report Types Events"

    Protected Sub rbtnlstReportTypes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnlstReportTypes.SelectedIndexChanged

        Me.trPeriodAndGenericActivities.Attributes("style") = ""
        If Me.rbtnlstReportTypes.SelectedValue.ToUpper() <> ReportType_CRF Then
            Me.trPeriodAndGenericActivities.Style.Add("display", "none")
        End If

        Me.ResetPage()

        ' commented as reset page reset all the values and so no need to set project
        'Me.btnSetProject_Click(sender, e)

        If Not FillChkLstColumns() Then
            Exit Sub
        End If

    End Sub

#End Region

    'added by vishal 26-04-2011
#Region "text change event"
    Protected Sub txtproject_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtproject.TextChanged
        Me.chkSelectAllFields.Checked = False
        Me.chklstColumns.ClearSelection()
        Me.chkSelectAllSubjects.Checked = False
        Me.chklstSubjects.ClearSelection()
        Me.gvwCRFDtl.Controls.Clear()
    End Sub
#End Region

#Region "Selected index change"
    Protected Sub ddlPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPeriod.SelectedIndexChanged
        If Not Me.FillChkLstSubjects() Then
            Exit Sub
        End If
    End Sub
#End Region



End Class
