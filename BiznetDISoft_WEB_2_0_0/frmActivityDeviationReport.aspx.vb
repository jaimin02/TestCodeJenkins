Imports System.Web.Services

Partial Class frmActivityDeviationReport
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Dim objCommon As New clsCommon
    Dim objHelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()

    Dim estr As String = String.Empty

    Dim GvDeviation_vProjectNo As Integer = 0
    Dim GvDeviation_iPeriod As Integer = 1
    Dim GvDeviation_vSubjectId As Integer = 2
    Dim GvDeviation_vNodeDisplayName As Integer = 3
    Dim GvDeviation_vPendingNodes As Integer = 4
    Dim GvDeviation_vMySubjectNo As Integer = 5
    Dim GvDeviation_activity As Integer = 6
    Dim GvDeviation_vRemarks As Integer = 7
    Dim GvDeviation_vDataEntryUser As Integer = 8
    Dim GvDeviation_dModifiedDate As Integer = 9
    Dim GvDeviation_iNodeId As Integer = 10


    Dim VS_ActivityDeviation As String = "ds_ActivityDeviation"

    Dim Dt_DeviationReport As DataTable = Nothing
    Dim dt_innerGrid As DataTable = Nothing
#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Page.Title = " :: Activity Deviation Report :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            If Not IsPostBack Then
                CType(Master.FindControl("lblHeading"), Label).Text = "Activity Deviation Report"
                Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)

                '' === For give all rights to pmadmin ==
                If (Me.Session(S_ScopeNo).ToString() = Scope_SAll.ToString()) Then
                    Me.AutoCompleteExtender1.ServiceMethod = "GetMyProjectCompletionListwithworkspacedesc"
                Else
                    Me.AutoCompleteExtender1.ServiceMethod = "GetMyProjectCompletionList"
                End If

                If (Not Request.QueryString("mode") Is Nothing) AndAlso (Me.Request.QueryString("mode") = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View) Then
                    If Not Request.QueryString("vWorkSpaceId") Is Nothing Then
                        Me.HProjectId.Value = Request.QueryString("vWorkSpaceId")
                        Me.txtproject.Text = Me.Request.QueryString("ProjectNo").ToString()
                        Me.txtproject.Enabled = False
                        btnSetProject_Click(sender, e)
                        BtnSearch_Click(sender, e)
                        deviation(True)
                        HideMenu()
                        Exit Sub
                    End If
                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....Page_Load")
            Exit Sub
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

#Region "Reset Controls"
    Private Function ResetControl() As Boolean
        Me.GvDeviation.DataSource = Nothing
        Me.GvDeviation.DataBind()
        Me.divGrid.Style.add("display", "none")
        Me.GvDeviation.EmptyDataText = ""
        Me.ddlActivity.Items.Clear()
        Me.ddlSubject.Items.Clear()
        Return True
    End Function
#End Region

#Region "Button Events"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim wStr As String = String.Empty
        Dim Periods As String = String.Empty
        Dim i As Integer = 0
        Dim ds_Period As New DataSet
        Dim View_WorkSpaceNodeDetail As New DataView
        Try
            If Not ResetControl() Then
                Throw New Exception("Error while reset controls")
            End If
            wStr = " vWorkspaceid = '" & Me.HProjectId.Value & "'"

            If Not objHelpDb.GetViewWorkSpaceNodeDetail(wStr, ds_Period, estr) Then
                Me.objCommon.ShowAlert("Error While Getting Data from WorkSpaceModeDetail:" + estr, Me.Page)

            End If

            View_WorkSpaceNodeDetail = ds_Period.Tables(0).DefaultView
            View_WorkSpaceNodeDetail.Sort = "iPeriod"

            Me.ddlPeriod.DataSource = ds_Period.Tables(0).DefaultView.ToTable(True, "iPeriod".ToString())
            Me.ddlPeriod.DataValueField = "iPeriod"
            Me.ddlPeriod.DataTextField = "iPeriod"
            Me.ddlPeriod.DataBind()
            Me.ddlPeriod.Items.Insert(0, New ListItem("--Select Period--", 0))
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, estr)
            Exit Sub
        End Try
    End Sub

    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click

        Try
            If Not deviation(True) Then
                Me.divExpandable.Style.Add("display", "none")
                Exit Sub
            End If
            Me.divExpandable.Style.Add("display", "")


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, estr)
            Exit Sub
        End Try
    End Sub

    Protected Sub Btn_SearchAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_SearchAll.Click
        Try
            Me.ddlActivity.SelectedIndex = 0
            Me.ddlSubject.SelectedIndex = 0
            If Not deviation(True) Then
                Throw New Exception
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, estr)
            Exit Sub
        End Try
    End Sub

#End Region

#Region "Grid Event"
    Protected Sub GvDeviation_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvDeviation.RowDataBound
        Dim Param As String = String.Empty
        Dim NodeId As String = String.Empty

        Dim Ds_Nodes As DataSet = New DataSet
        Dim GridId As String = String.Empty

        Try
            'If GvDeviation.Rows.Count < 1 Then
            '    Exit Sub
            'End If
            e.Row.Cells(GvDeviation_vPendingNodes).Visible = False
            e.Row.Cells(GvDeviation_vSubjectId).Style.Add("display", "none")
            e.Row.Cells(GvDeviation_iNodeId).Style.Add("display", "none")

            If e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(GvDeviation_vPendingNodes).Style.Add("display", "none")
            End If
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(GvDeviation_vPendingNodes).Style.Add("display", "none")
                GridId = CType(e.Row.FindControl("GvPendingNodes"), GridView).ClientID.ToString()

                Param = Me.HProjectId.Value.ToString() + "##" + e.Row.Cells(GvDeviation_vPendingNodes).Text.ToString() + "##" + e.Row.Cells(GvDeviation_iNodeId).Text + "##" + e.Row.Cells(GvDeviation_vSubjectId).Text + "##"

                If Not objHelpDb.Proc_GetNodeInformation(Param, Ds_Nodes, estr) Then
                    Throw New Exception(estr)
                End If

                'dt_innerGrid = Ds_Nodes.Tables(0).Copy()
                'dt_innerGrid.AcceptChanges()
                If Ds_Nodes.Tables(0).Rows.Count > 0 Then

                    '  CType(e.Row.FindControl("ShowHideActivities"), HtmlImage).Attributes.Add("onClick", "GetPendingNodes('" & GridId & "');") '& "','" & e.Row.Cells(GvDeviation_vPendingNodes).Text.ToString() & "');")
                    CType(e.Row.FindControl("GvPendingNodes"), GridView).DataSource = Ds_Nodes
                    CType(e.Row.FindControl("GvPendingNodes"), GridView).DataBind()
                    CType(e.Row.FindControl("GvPendingNodes"), GridView).Style.Add("display", "none")
                End If
                CType(e.Row.Cells(GvDeviation_activity).FindControl("ShowPendingActivity"), HtmlImage).Attributes.Add("src", "images/Plus.gif")
                CType(e.Row.Cells(GvDeviation_activity).FindControl("ShowPendingActivity"),  _
                            HtmlImage).Attributes.Add("OnClick", "return chk_Status(" & CType(e.Row.Cells(GvDeviation_activity).FindControl("ShowPendingActivity") _
                                                      , HtmlImage).ClientID & "," & CType(e.Row.FindControl("GvPendingNodes"), GridView).ClientID & ");")


            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, estr)
            Exit Sub
        End Try

    End Sub
#End Region

#Region "Web Method"
#Region "Get Pending Nodes"
    <Services.WebMethod()> _
    Public Shared Function GetPendingNodes() As String 'ByVal Param As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim DsPendingNodes As New DataSet
        Dim estr As String = String.Empty

        Try

            'If Not objHelp.Proc_GetNodeInformation(Param, DsPendingNodes, estr) Then
            '    Throw New Exception(estr)
            'End If
            Dim dt As New DataTable
            dt.Columns.Add("vPendingNodeDisplayName")
            DsPendingNodes.Tables.Add(dt)
            DsPendingNodes.Tables(0).Rows.Add("Hi")
            DsPendingNodes.Tables(0).Rows.Add("Hi")
            DsPendingNodes.Tables(0).Rows.Add("Hi")
            DsPendingNodes.Tables(0).Rows.Add("Hi")
            Return DsPendingNodes.GetXml()
        Catch ex As Exception
            'ShowErrorMessage(ex.Message, estr)
            Return ex.Message()
        End Try
    End Function
#End Region
#End Region

#Region "Fill Grid With Deviation"
    Private Function deviation(ByVal str As Boolean) As Boolean
        Dim ds_ActivityDeviation As DataSet
        Dim wStr As String = String.Empty
        Try
            'CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            wStr = " vWorkspaceId = '" & Me.HProjectId.Value & "'"
            If Me.ddlPeriod.SelectedIndex > 0 Then
                wStr += " And iPeriod = " & Me.ddlPeriod.SelectedValue
            End If
            If Me.ddlActivity.SelectedIndex > 0 Then
                wStr += " And vNodeDisplayName = '" & Me.ddlActivity.SelectedValue & "'"
            End If
            If Me.ddlSubject.SelectedIndex > 0 Then
                wStr += " And vSubjectId = '" & Me.ddlSubject.SelectedValue & "'"
            End If
            wStr += " order by dModifiedDate "
            If Not objHelpDb.View_WorkspaceActivitySequenceDeviation(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ActivityDeviation, estr) Then
                Throw New Exception(estr)
            End If
            'Dt_DeviationReport = ds_ActivityDeviation.Tables(0).Copy()
            'Dt_DeviationReport.AcceptChanges()
            If ds_ActivityDeviation.Tables(0).Rows.Count < 1 Then
                Me.ddlActivity.Items.Clear()
                Me.ddlActivity.Items.Insert(0, "All Activity")
                Me.ddlSubject.Items.Clear()
                Me.ddlSubject.Items.Insert(0, "All Subject")


                objCommon.ShowAlert("No Deviation Found", Me.Page)
                Me.GvDeviation.Style.Add("display", "none")
                deviation = True
                If Not ResetControl() Then
                    Throw New Exception("Error while reset controls")
                End If
                Exit Function
            End If
            If str = True Then
                'Fill DropDown With Distict value-----------

                ds_ActivityDeviation.Tables(0).DefaultView.RowFilter = "iPeriod=" & ddlPeriod.SelectedValue.ToString()
                Me.ddlActivity.DataSource = ds_ActivityDeviation.Tables(0).DefaultView.ToTable(True, "vNodeDisplayName")
                Me.ddlActivity.DataValueField = "vNodeDisplayName"
                Me.ddlActivity.DataTextField = "vNodeDisplayName"
                Me.ddlActivity.DataBind()
                Me.ddlActivity.Items.Insert(0, "All Activity")
                Me.ddlActivity.ClearSelection()

                Me.ddlSubject.DataSource = ds_ActivityDeviation.Tables(0).DefaultView.ToTable(True, "vSubjectId,vMySubjectNo".Split(","))
                Me.ddlSubject.DataValueField = "vSubjectId"
                Me.ddlSubject.DataTextField = "vMySubjectNo"
                Me.ddlSubject.DataBind()
                Me.ddlSubject.Items.Insert(0, "All Subject")
                Me.ddlSubject.ClearSelection()
            End If

            Me.GvDeviation.Style.Add("display", "")
            Me.GvDeviation.DataSource = ds_ActivityDeviation
            Me.GvDeviation.DataBind()
            Me.divGrid.Style.Add("display", "")
            Me.btnExportToExcel.Style.Add("display", "")

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, estr)
            Return False
            Exit Function
        End Try



    End Function
#End Region

#Region "DropDown Activity"
    Protected Sub ddlSubject_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSubject.SelectedIndexChanged
        If Not deviation(False) Then
            Exit Sub
        End If
    End Sub

    Protected Sub ddlActivity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlActivity.SelectedIndexChanged
        If Not deviation(False) Then
            Exit Sub
        End If
    End Sub
#End Region

#Region "Export To Excel"

    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
        Try
            'gridview 
            'Dim table As New DataTable()
            'For Each column As DataColumn In Dt_DeviationReport.Columns
            '    table.Columns.Add(column.ColumnName, column.DataType)
            'Next




            'For Each column As DataColumn In dt_innerGrid.Columns
            '    If column.ColumnName = "iNodeId" Or column.ColumnName = "vSubjectID" Then
            '        table.Columns.Add(column.ColumnName + "2", column.DataType)
            '    Else
            '        table.Columns.Add(column.ColumnName, column.DataType)
            '    End If

            'Next

            '    Dim results As DataTable
            'results = Dt_DeviationReport .AsEnumerable().Join(dt_innerGrid .AsEnumerable(),on Dt_DeviationReport["vSubjectID"] equals dt_innerGrid["vSubjectID"]select new()

            'a => a.Field<String>("vSubjetcID"),
            'b => b.Field<String>("vSubjetcID"),
            '(a, b) =>
            '{
            '    DataRow row = table.NewRow();
            '    row.ItemArray = a.ItemArray.Concat(b.ItemArray).ToArray();
            '    table.Rows.Add(row);
            '    return row;
            '});

            If Not GvDeviation.Rows.Count <> 0 Then
                objCommon.ShowAlert("Data Not Available.", Me.Page)
                Exit Sub
            End If
            Dim stringWriter As New System.IO.StringWriter()
            Dim writer As New HtmlTextWriter(stringWriter)


            Me.GvDeviation.RenderControl(writer)
            Dim gridViewhtml As String = stringWriter.ToString()

            Dim fileName As String = "Deviation Report" & ".xls"

            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            Context.Response.Write(gridViewhtml.Substring(5).Substring(0, gridViewhtml.Substring(5).Length - 6))

            Context.Response.Flush()
            Context.Response.End()
            Context.Response.Close()

            File.Delete(fileName)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Don't remove this function
        ' This Function is mendetory when you are going to export your grid to excel.
        ' NOTE :: And Click event of button must be in postback trigger. (Page must be loaded)
    End Sub

#End Region

    Protected Sub ddlPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPeriod.SelectedIndexChanged
        Try
            If Me.ddlActivity.SelectedIndex < 0 AndAlso Me.ddlSubject.SelectedIndex < 0 Then
                Me.BtnSearch_Click(sender, e)
                Exit Sub
            End If
            ddlActivity.SelectedIndex = 0
            ddlSubject.SelectedIndex = 0
            Me.BtnSearch_Click(sender, e)
        Catch ex As Exception
        End Try
    End Sub
End Class
