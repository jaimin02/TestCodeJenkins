
Partial Class frmScheduledActivityDeviationReport
    Inherits System.Web.UI.Page
#Region "Variable Declaration"
    Private ObjCommon As New clsCommon
    Private ObjHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private ObjLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const VS_DtView_WorkSpaceNodeDetail As String = "View_WorkSpaceNodeDetail"
    Private Const VS_Deviation As String = "VS_Deviation"
    Private Const VS_DeviationExporttoExcel As String = "VS_DeviationExporttoExcel"
#End Region
#Region "Form Events"
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If Not GenCall() Then
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....pageLoad", ex)
        End Try
    End Sub
#End Region

#Region "GenCall()"
    Private Function GenCall() As Boolean
        Try
            If Not GenCall_Data() Then
                Exit Function
            End If
            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If
            GenCall = True
        Catch ex As Exception
            GenCall = False
            Me.ShowErrorMessage(ex.Message, "....GenCall", ex)
        End Try
    End Function
#End Region

#Region "GenCall_Data()"
    Private Function GenCall_Data() As Boolean
        Try
            GenCall_Data = True
        Catch ex As Exception
            GenCall_Data = False
            Me.ShowErrorMessage(ex.Message, "....GenCall_Data", ex)
        End Try
    End Function
#End Region

#Region "GenCall_ShowUI()"
    Private Function GenCall_ShowUI() As Boolean
        Try
            Page.Title = " :: Activity Time Deviation Report ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")

            CType(Master.FindControl("lblHeading"), Label).Text = "Activity Time Deviation Report"
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
        Catch ex As Exception
            GenCall_ShowUI = False
            Me.ShowErrorMessage(ex.Message, "....GenCall_ShowUI", ex)
        End Try
    End Function
#End Region

#Region "Fill Dropdown"
    Private Function FillddlPeriod() As Boolean
        Dim ds_WorkSpaceNodeDetail As New Data.DataSet
        Dim dv_WorkSpaceNodeDetail As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim strRowFilter As String = String.Empty
        Try


            wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' And cStatusIndi <> 'D'"

            If Not ObjHelp.GetViewWorkSpaceNodeDetail(wstr, ds_WorkSpaceNodeDetail, estr) Then
                Throw New Exception("Error While Getting Data from WorkSpaceNodeDetail : " + estr)
            End If

            Me.ViewState(VS_DtView_WorkSpaceNodeDetail) = ds_WorkSpaceNodeDetail.Tables(0).Copy()

            dv_WorkSpaceNodeDetail = ds_WorkSpaceNodeDetail.Tables(0).DefaultView
            dv_WorkSpaceNodeDetail.Sort = "iPeriod"

            Me.ddlPeriod.DataSource = dv_WorkSpaceNodeDetail.ToTable(True, "iPeriod".ToString())
            Me.ddlPeriod.DataValueField = "iPeriod"
            Me.ddlPeriod.DataTextField = "iPeriod"
            Me.ddlPeriod.DataBind()
            Me.ddlPeriod.Items.Insert(0, New ListItem("All", "All"))
            Me.ddlPeriod.Items.Insert(0, New ListItem("--Select Period--", 0))

            Return True
        Catch ex As Exception
            FillddlPeriod = False
            Me.ShowErrorMessage(ex.Message, "....FillddlPeriod", ex)
        End Try
    End Function

    Private Function FillddlActivity() As Boolean
        Dim dt_Activity As New DataTable
        Dim dv_Activity As New DataView
        Dim estr As String = String.Empty
        Dim wStr As String = String.Empty
        Try

            'dv_Activity = CType(Me.ViewState(VS_DtView_WorkSpaceNodeDetail), DataTable).Copy().DefaultView
            dv_Activity = CType(Me.ViewState(VS_Deviation), DataTable).Copy().DefaultView
            'dv_Activity.RowFilter = "iPeriod = " + Me.ddlPeriod.SelectedItem.Value.Trim()
            'dv_Activity.Sort = "iNodeID,iNodeNo"
            dv_Activity.RowFilter = "Deviation = 'Y'"
            If ddlPeriod.SelectedIndex > 1 AndAlso ddlPeriod.SelectedValue.ToUpper <> "ALL" Then
                dv_Activity.RowFilter = "Deviation = 'Y' AND Period = " + ddlPeriod.SelectedValue.Trim()
            End If
            dt_Activity = dv_Activity.ToTable()
            If Not dt_Activity Is Nothing AndAlso dt_Activity.Rows.Count > 0 Then
                Me.ddlActivity.DataSource = dt_Activity.DefaultView.ToTable(True, "Scheduling Node", "Scheduling Activity Name")
                Me.ddlActivity.DataValueField = "Scheduling Node"
                Me.ddlActivity.DataTextField = "Scheduling Activity Name"
                Me.ddlActivity.DataBind()
                Me.ddlActivity.Items.Insert(0, New ListItem("All", "All"))
                ' Me.ddlActivity.Items.Insert(0, New ListItem("--Select Activity--", 0))
            Else
                ObjCommon.ShowAlert("No Activity found.", Me)
                Me.ddlActivity.Items.Clear()
                Me.ddlSubject.Items.Clear()
                Return False
            End If
            Return True
        Catch ex As Exception
            FillddlActivity = False
            Me.ShowErrorMessage(ex.Message, "....FillddlActivity", ex)
        End Try
    End Function

    Private Function FillddlSubject() As Boolean
        Dim dtSubjects As New DataTable
        Try
            'wStr = "vWorkspaceId = '" + Me.HProjectId.Value + "' And cStatusIndi <> 'D' "
            'wStr += " And iPeriod = " + Me.ddlPeriod.SelectedItem.Value.Trim() '+ " And cRejectionFlag<>'Y'"
            'wStr += " order by vMySubjectNo"

            'If Not ObjHelp.GetViewWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '                                           dsSubjects, eStr) Then
            '    Me.ShowErrorMessage("Error While Getting Data From ViewWorkspaceSubjectMst : ", eStr)
            '    Exit Function
            'End If
            dtSubjects = CType(Me.ViewState(VS_Deviation), DataTable).Copy()
            dtSubjects.DefaultView.RowFilter = "Deviation = 'Y'"
            If ddlActivity.SelectedIndex > 0 AndAlso ddlActivity.SelectedValue.ToUpper() <> "ALL" Then
                dtSubjects.DefaultView.RowFilter = "Deviation = 'Y' AND [Scheduling Node] = " + ddlActivity.SelectedValue.Trim()
            End If

            dtSubjects = dtSubjects.DefaultView.ToTable()
            If Not dtSubjects Is Nothing AndAlso dtSubjects.Rows.Count > 0 Then
                Me.ddlSubject.DataSource = dtSubjects.DefaultView.ToTable(True, "Subject No.")
                Me.ddlSubject.DataValueField = "Subject No."
                Me.ddlSubject.DataTextField = "Subject No."
                Me.ddlSubject.DataBind()
                Me.ddlSubject.Items.Insert(0, New ListItem("All", "All"))
                'Me.ddlSubject.Items.Insert(0, New ListItem("--Select Subject--", 0))
                ddlSubject.SelectedIndex = 0
            Else
                ObjCommon.ShowAlert("No Subject found.", Me)
                Me.ddlSubject.Items.Clear()
                Return False
            End If
            FillddlSubject = True
        Catch ex As Exception
            FillddlSubject = False
            Me.ShowErrorMessage(ex.Message, "....FillddlSubject", ex)
        End Try
    End Function
#End Region

#Region "Fill Grid"
    Private Function FillGrid() As Boolean
        Dim vPeriod As String = String.Empty
        Dim vActivity As String = String.Empty
        Dim vNodeId As String = String.Empty
        Dim vSubject As String = String.Empty
        Dim eStr As String = String.Empty
        Dim dsDeviation As New DataSet
        Dim paramActivity() As String = {"Project No.", "Period", "Subject No.", "Subject Id", "Reference Activity Name", "Scheduling Activity Name", "Schedule Time", "Actual Time", "Difference Time (Minutes)", "Allow Deviation"}
        Dim strRowFilter As String = String.Empty
        Try


            Me.btnExporttoExcel.Style("display") = "none"
            strRowFilter = "Deviation = 'Y'"
            If ddlPeriod.SelectedIndex > 1 AndAlso ddlPeriod.SelectedValue.ToUpper <> "ALL" Then
                vPeriod = ddlPeriod.SelectedValue.ToString().Trim()
                strRowFilter += " AND Period = " + vPeriod
            End If
            If ddlActivity.SelectedIndex > 0 AndAlso ddlActivity.SelectedValue.ToUpper() <> "ALL" Then
                vNodeId = Me.ddlActivity.SelectedValue.ToString.Trim()
                strRowFilter += " AND [Scheduling Node] = " + vNodeId
            End If
            If ddlSubject.SelectedIndex > 0 AndAlso ddlSubject.SelectedValue.ToUpper <> "ALL" Then
                vSubject = ddlSubject.SelectedValue.ToString().Trim()
                strRowFilter += " AND [Subject No.] = '" + vSubject + "'"
            End If

            dsDeviation.Tables.Add(CType(ViewState(VS_Deviation), DataTable))
            dsDeviation.Tables(0).DefaultView.RowFilter = strRowFilter

            If dsDeviation.Tables(0).DefaultView.Count = 0 Then
                Me.gvDeviation.DataSource = Nothing
                Me.gvDeviation.DataBind()
                Me.btnExporttoExcel.Style("display") = "none"
                Me.fldgvDeviation.Style.Add("display", "none")
                ObjCommon.ShowAlert("No Record found.", Me)
                Return False
                Exit Function
            End If
            Me.gvDeviation.DataSource = dsDeviation.Tables(0).DefaultView.ToTable(True, paramActivity).Copy()
            Me.ViewState(VS_DeviationExporttoExcel) = dsDeviation.Tables(0).DefaultView.ToTable(True, paramActivity).Copy()
            Me.gvDeviation.DataBind()
            Me.btnExporttoExcel.Style("display") = ""
            Me.fldgvDeviation.Style.Add("display", "")

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Filling Grid. ", ex.Message)
            Return False
        End Try
    End Function
#End Region

#Region "Fill ViewState"
    Private Function FillViewState() As Boolean
        Dim vPeriod As String = String.Empty
        Dim vActivity As String = String.Empty
        Dim vNodeId As String = String.Empty
        Dim vSubject As String = String.Empty
        Dim dsDeviation As New DataSet
        Dim eStr As String = String.Empty
        Try
            If ddlPeriod.SelectedIndex > 1 AndAlso ddlPeriod.SelectedValue.ToUpper <> "ALL" Then
                vPeriod = ddlPeriod.SelectedValue.ToString().Trim()
            End If
            If ddlActivity.SelectedIndex > 0 AndAlso ddlActivity.SelectedValue.ToUpper() <> "ALL" Then
                '        vActivity = Me.ddlActivity.SelectedValue.Split("##")(0)
                vNodeId = Me.ddlActivity.SelectedValue.ToString.Trim()
            End If
            If ddlSubject.SelectedIndex > 0 AndAlso ddlSubject.SelectedValue.ToUpper <> "ALL" Then
                vSubject = ddlSubject.SelectedValue.ToString().Trim()
            End If


            If Not ObjHelp.Proc_GetDataForScheduling_Deviation(Me.HProjectId.Value.Trim(), vPeriod, vNodeId, dsDeviation, Session(S_UserID).ToString, eStr) Then
                Throw New Exception(eStr)
            End If

            Me.ViewState(VS_Deviation) = dsDeviation.Tables(0).Copy()
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error While FillViewState. ", ex.Message)
            Return False
        End Try
    End Function
#End Region

#Region "Button Events"
    Protected Sub btnSetProject_Click(sender As Object, e As EventArgs) Handles btnSetProject.Click
        Dim wStr As String = String.Empty
        Try
            wStr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'"
            fldgvDeviation.Style.Add("display", "none")
            gvDeviation.DataSource = Nothing
            gvDeviation.DataBind()
            Me.btnExporttoExcel.Style("display") = "none"
            Me.ddlActivity.Items.Clear()
            Me.ddlSubject.Items.Clear()
            If Not FillddlPeriod() Then
                Exit Sub
            End If
            
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....btnSetProject_Click", ex)
        End Try
    End Sub

    Protected Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        Try
            If ddlPeriod.SelectedValue = "0" Then
                ObjCommon.ShowAlert("Please select period first.", Me.Page)
                Exit Sub
            End If
            If Not Me.FillGrid() Then
                Exit Sub
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Try
            Response.Redirect("frmScheduledActivityDeviationReport.aspx")
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnExporttoExcel_Click(sender As Object, e As EventArgs) Handles btnExporttoExcel.Click
        Dim dt As DataTable = Nothing
        Dim FileName As String = String.Empty
        Dim eStr As String = String.Empty
        Dim vPeriod As String = String.Empty
        Dim vActivity As String = String.Empty
        Dim vNodeId As String = String.Empty
        Dim vSubject As String = String.Empty
        Dim dsDeviation As New DataSet
        Dim strRowFilter As String = String.Empty
        Dim dtDeviation As New DataTable

        Dim style = "<style> .StyleAsText { mso-number-format:\@; } </style><style>.text{ mso-number-format:\@;}</style> "

        Try
            If CType(ViewState(VS_DeviationExporttoExcel), DataTable) Is Nothing Then

                vPeriod = IIf(Me.ddlPeriod.SelectedValue.Trim.ToUpper = "ALL", "", "," & Me.ddlPeriod.SelectedValue & ",")
                vNodeId = IIf(Me.ddlActivity.SelectedValue.Trim.ToUpper = "ALL", "", "," & Me.ddlActivity.SelectedValue & ",")

                If Not ObjHelp.Proc_GetDataForScheduling_Deviation(Me.HProjectId.Value.Trim(), vPeriod, vNodeId, dsDeviation, Session(S_UserID).ToString, eStr) Then
                    Throw New Exception(eStr)
                End If

                dtDeviation = dsDeviation.Tables(0)
                strRowFilter = "Deviation = 'Y'"
                dtDeviation.DefaultView.RowFilter = strRowFilter
                dt = dtDeviation.DefaultView.ToTable().Copy()
            Else
                dt = CType(ViewState(VS_DeviationExporttoExcel), DataTable)
            End If

            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then

                Context.Response.Clear()
                Context.Response.Buffer = True
                Context.Response.ClearContent()
                Context.Response.ClearHeaders()
                FileName = "Activity Time Deviation Report"
                Context.Response.AppendHeader("Content-type", "application/vnd.ms-excel")
                Context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName + ".xls")
                Context.Response.Write(style)
                Context.Response.Write(ReportDetail(dt).ToString().Replace("<td>", "<td class='StyleAsText'>"))
                Context.Response.Flush()
                Context.Response.End()

            Else
                ObjCommon.ShowAlert("No Data Found.", Me.Page)
            End If
        Catch ex As Exception
            ObjCommon.ShowAlert(ex.Message.ToString() + "...btnExporttoExcel_Click", Me.Page)
        End Try
    End Sub
#End Region

#Region "Dropdown Events"
    Protected Sub ddlPeriod_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPeriod.SelectedIndexChanged
        Try
            Me.ddlActivity.Items.Clear()
            Me.ddlSubject.Items.Clear()
            fldgvDeviation.Style.Add("display", "none")
            gvDeviation.DataSource = Nothing
            gvDeviation.DataBind()
            Me.btnExporttoExcel.Style("display") = ""
            If Not Me.FillViewState() Then
                Exit Sub
            End If
            If Not FillddlActivity() Then
                Exit Sub
            End If

            If Not FillddlSubject() Then
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....ddlPeriod_SelectedIndexChanged", ex)
        End Try
    End Sub
    Protected Sub ddlActivity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlActivity.SelectedIndexChanged
        Try
            fldgvDeviation.Style.Add("display", "none")
            Me.ddlSubject.Items.Clear()
            gvDeviation.DataSource = Nothing
            gvDeviation.DataBind()
            Me.btnExporttoExcel.Style("display") = "none"
            If Not FillddlSubject() Then
                Exit Sub
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub ddlSubject_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSubject.SelectedIndexChanged
        Try
            fldgvDeviation.Style.Add("display", "none")
            gvDeviation.DataSource = Nothing
            gvDeviation.DataBind()
            Me.btnExporttoExcel.Style("display") = "none"
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Error Handler"
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        ObjCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        ObjCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub
#End Region
#Region "Other function"
    Private Function ReportDetail(ByVal dt_FileDetail As DataTable) As String
        Dim strMessage As New StringBuilder
        strMessage.Append("<table border=""1""><tr>")

        For Each dc As DataColumn In dt_FileDetail.Columns
            strMessage.Append("<td style='font-weight: bold;text-align:center;'>")
            strMessage.Append(dc.ColumnName)
            strMessage.Append("</td>")
        Next

        strMessage.Append("</tr>")
        For i As Integer = 0 To dt_FileDetail.Rows.Count - 1
            strMessage.Append("<tr>")
            For j As Integer = 0 To dt_FileDetail.Columns.Count - 1
                strMessage.Append("<td>")
                strMessage.Append(dt_FileDetail.Rows(i)(j).ToString)
                strMessage.Append("</td>")
            Next
            strMessage.Append("</tr>")
        Next
        strMessage.Append("</table>")
        Return strMessage.ToString
    End Function
#End Region
End Class
