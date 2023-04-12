Imports Newtonsoft.Json
Imports System.Drawing

Partial Class frmSubjectPIFAuditTrial
    Inherits System.Web.UI.Page
#Region "VARIABLE DECLARATION"
    Private ObjCommon As New clsCommon
    Private ObjHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
#End Region

#Region "Page Events"
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            For Each lst As ListItem In rbFilterType.Items
                lst.Attributes.Add("onclick", "ChangeFilterType('" + lst.Text.ToLower() + "');")
            Next
            If Not GenCall() Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "GenCall()"
    Private Function GenCall() As Boolean
        Try
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "PIF Audit Trial"
            Page.Title = ":: PIF Audit Trial  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            Me.AutoCompleteExtenderProject.ContextKey = "iUserId = " & Me.Session(S_UserID)
            GenCall = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall")
            GenCall = False
        End Try
    End Function
#End Region

#Region "Button Event"

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If Not IsNothing(Me.Request.QueryString("Page2")) AndAlso Me.Request.QueryString("Page2").Trim() <> "" Then
            Me.Response.Redirect(Me.Request.QueryString("page2") + ".aspx?WorkspaceId=" + Me.Request.QueryString("WorkspaceId"), False)
        End If

        Me.Response.Redirect("frmMainPage.aspx", False)
    End Sub

    Protected Sub btnSetProject_Click(sender As Object, e As EventArgs) Handles btnSetProject.Click

    End Sub

    Protected Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click

    End Sub

    Protected Sub btnExportToExcelSubject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcelSubject.Click
        Dim ObjHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
        Dim eStr_Retu As String = String.Empty
        Dim wStr As String = "1=2"
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim filename As String = String.Empty
        Dim strMessage As New StringBuilder

        Try
            'wStr = "vWorkspaceID='0000008947'"
            If rbFilterType.SelectedItem.Text.ToUpper.Contains("PROJECT") Then
                wStr = "vWorkspaceID='" + HProjectId.Value + "'"
                filename = txtproject.Text.Split("]")(0).TrimStart("[")
                If Not ObjHelp.GetView_SubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                              ds, eStr_Retu) Then
                    Throw New Exception(eStr_Retu)
                End If
            ElseIf rbFilterType.SelectedItem.Text.ToUpper.Contains("SUBJECT") Then
                wStr = "vSubjectID='" + HSubjectId.Value + "'"
                filename = HSubjectId.Value
                If Not ObjHelp.GetView_SubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                              ds, eStr_Retu) Then
                    Throw New Exception(eStr_Retu)
                End If

            ElseIf rbFilterType.SelectedItem.Text.ToUpper.Contains("DATE") Then
                wStr = "EXEC Proc_PIFSubjectList '" + hdnFromDate.Value + "','" + hdnToDate.Value + "'"
                filename = hdnFromDate.Value + "-TO-" + hdnToDate.Value
                ds = ObjHelp.GetResultSet(wStr, "PIFSubjectList")
            End If
            dt = ds.Tables(0).DefaultView.ToTable(True, "vSubjectID", "FullName", "vInitials")

            dt.Columns("vSubjectID").ColumnName = "Subject ID"
            dt.Columns("FullName").ColumnName = "Full Name"
            dt.Columns("vInitials").ColumnName = "Initials"

            gvExport.DataSource = dt
            gvExport.DataBind()
            gvExport.Style.Item("display") = ""

            If dt.Rows.Count > 0 Then
                gvExport.HeaderRow.BackColor = Color.White
                gvExport.FooterRow.BackColor = Color.White
                For Each cell As TableCell In gvExport.HeaderRow.Cells
                    cell.BackColor = gvExport.HeaderStyle.BackColor
                Next
                For Each row As GridViewRow In gvExport.Rows
                    row.BackColor = Color.White
                    row.Height = 20
                    For Each cell As TableCell In row.Cells
                        If row.RowIndex Mod 2 = 0 Then
                            cell.BackColor = gvExport.AlternatingRowStyle.BackColor
                            cell.ForeColor = gvExport.RowStyle.ForeColor
                        Else
                            cell.BackColor = gvExport.RowStyle.BackColor
                            cell.ForeColor = gvExport.RowStyle.ForeColor
                        End If
                        cell.CssClass = "textmode"
                    Next
                Next
            End If
            'style to format numbers to string
            Dim style As String = "<style> .textmode { } </style>"
            Response.Write(style)



            Dim info As String = String.Empty
            Dim gridviewHtml As String = String.Empty

            filename = "PIF Subject List_" + filename + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

            Dim stringWriter As New System.IO.StringWriter()

            Dim writer As New HtmlTextWriter(stringWriter)
            gvExport.RenderControl(writer)
            gridviewHtml = stringWriter.ToString()
            gridviewHtml = gridviewHtml.Replace("display: none", "")
            strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
            strMessage.Append("</font></strong><center></b></td>")
            strMessage.Append("</tr>")





            strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append("PIF Subject List")
            strMessage.Append("</font></strong><center></b></td>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")


            gridviewHtml = strMessage.ToString() + gridviewHtml
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
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnExportToExcelAuditTrial_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcelAuditTrial.Click
        Dim ObjHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
        Dim eStr_Retu As String = String.Empty
        Dim wStr As String = "1=2"
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim filename As String = String.Empty
        Dim strMessage As New StringBuilder
        Dim BlankSpace As String = "<Table><tr><td></td></tr></Table>"

        Try
            If Not HttpContext.Current.Session("UserID") Is Nothing Then
                wStr = "EXEC Proc_PIFAuditTrial '" + HSelectedSubjectId.Value + "'" + "," + HttpContext.Current.Session(S_UserID)
            Else
                wStr = "EXEC Proc_PIFAuditTrial '" + HSelectedSubjectId.Value + "'"
            End If

            filename = HSelectedSubjectId.Value

            ds = ObjHelp.GetResultSet(wStr, "PIFAuditTrial")

            If Not ds Is Nothing And ds.Tables.Count > 0 Then
                ds.Tables(0).TableName = "tblPersonalDetailAuditTrial"
                ds.Tables(1).TableName = "tblPersonalProofDetailAuditTrial"
                ds.Tables(2).TableName = "tblPersonalHabitDetailAuditTrial"
                ds.Tables(3).TableName = "tblFemaleDetailAuditTrial"
                ds.Tables(4).TableName = "tblContactDetailAuditTrial"
            End If

            gvExport.DataSource = ds.Tables(0)
            gvExport.DataBind()
            gvExport.Style.Item("display") = ""

            GridView1.DataSource = ds.Tables(1)
            GridView1.DataBind()
            GridView1.Style.Item("display") = ""

            GridView2.DataSource = ds.Tables(2)
            GridView2.DataBind()
            GridView2.Style.Item("display") = ""

            GridView3.DataSource = ds.Tables(3)
            GridView3.DataBind()
            GridView3.Style.Item("display") = ""

            GridView4.DataSource = ds.Tables(4)
            GridView4.DataBind()
            GridView4.Style.Item("display") = ""

            If dt.Rows.Count > 0 Then
                gvExport.HeaderRow.BackColor = Color.White
                gvExport.FooterRow.BackColor = Color.White
                For Each cell As TableCell In gvExport.HeaderRow.Cells
                    cell.BackColor = gvExport.HeaderStyle.BackColor
                Next
                For Each row As GridViewRow In gvExport.Rows
                    row.BackColor = Color.White
                    row.Height = 20
                    For Each cell As TableCell In row.Cells
                        If row.RowIndex Mod 2 = 0 Then
                            cell.BackColor = gvExport.AlternatingRowStyle.BackColor
                            cell.ForeColor = gvExport.RowStyle.ForeColor
                        Else
                            cell.BackColor = gvExport.RowStyle.BackColor
                            cell.ForeColor = gvExport.RowStyle.ForeColor
                        End If
                        cell.CssClass = "textmode"
                    Next
                Next
            End If
            'style to format numbers to string
            Dim style As String = "<style> .textmode { } </style>"
            Response.Write(style)



            Dim info As String = String.Empty
            Dim gridviewHtml As String = String.Empty
            Dim gridviewHtml1 As String = String.Empty
            Dim gridviewHtml2 As String = String.Empty
            Dim gridviewHtml3 As String = String.Empty
            Dim gridviewHtml4 As String = String.Empty

            filename = "PIF Subject Audit Trial_" + filename + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

            Dim stringWriter As New System.IO.StringWriter()

            Dim writer As New HtmlTextWriter(stringWriter)
            gvExport.RenderControl(writer)
            gridviewHtml = stringWriter.ToString()

            stringWriter = New System.IO.StringWriter()
            writer = New HtmlTextWriter(stringWriter)
            GridView1.RenderControl(writer)
            gridviewHtml1 = stringWriter.ToString()

            stringWriter = New System.IO.StringWriter()
            writer = New HtmlTextWriter(stringWriter)
            GridView2.RenderControl(writer)
            gridviewHtml2 = stringWriter.ToString()

            stringWriter = New System.IO.StringWriter()
            writer = New HtmlTextWriter(stringWriter)
            GridView3.RenderControl(writer)
            gridviewHtml3 = stringWriter.ToString()

            stringWriter = New System.IO.StringWriter()
            writer = New HtmlTextWriter(stringWriter)
            GridView4.RenderControl(writer)
            gridviewHtml4 = stringWriter.ToString()

            strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
            strMessage.Append("</font></strong><center></b></td>")
            strMessage.Append("</tr>")





            strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append("PIF-AuditTrail")
            strMessage.Append("</font></strong><center></b></td>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")

            BlankSpace += BlankSpace + BlankSpace
            gridviewHtml = strMessage.ToString() + "<Table><tr><td></td></tr></Table>" + gridviewHtml + BlankSpace + gridviewHtml1 + BlankSpace + gridviewHtml2 + BlankSpace + gridviewHtml3 + BlankSpace + gridviewHtml4
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
        Catch ex As Exception

        End Try
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub
#End Region

#Region "Web Methods"
    <Web.Services.WebMethod()> _
    Public Shared Function GetSubjectList(ByVal values As Dictionary(Of String, String)) As String
        Dim ObjCommon As New clsCommon
        Dim ObjHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
        Dim eStr_Retu As String = String.Empty
        Dim wStr As String = "1=2"
        Dim ds As New DataSet
        Dim dt As New DataTable
        Try
            'wStr = "vWorkspaceID='0000008947'"
            If values.Item("FilterFlag").ToUpper.Contains("PROJECT") Then
                wStr = "vWorkspaceID='" + values.Item("ProjectID") + "'"
                If Not ObjHelp.GetView_SubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                              ds, eStr_Retu) Then
                    Throw New Exception(eStr_Retu)
                End If
            ElseIf values.Item("FilterFlag").ToUpper.Contains("SUBJECT") Then
                wStr = "vSubjectID='" + values.Item("SubjectID") + "'"
                If Not ObjHelp.GetView_SubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                              ds, eStr_Retu) Then
                    Throw New Exception(eStr_Retu)
                End If
            ElseIf values.Item("FilterFlag").ToUpper.Contains("DATE") Then
                wStr = "EXEC Proc_PIFSubjectList '" + values.Item("FromDate") + "','" + values.Item("ToDate") + "'"
                ds = ObjHelp.GetResultSet(wStr, "PIFSubjectList")
            End If
            dt = ds.Tables(0).DefaultView.ToTable(True, "vSubjectID", "FullName", "vInitials")
            dt.Columns.Add("AuditTrial")

            Return JsonConvert.SerializeObject(dt)
        Catch ex As Exception
            Return JsonConvert.SerializeObject(ex.ToString)
        End Try

    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function GetSubjectAuditTrial(ByVal values As Dictionary(Of String, String)) As String
        Dim ObjCommon As New clsCommon
        Dim ObjHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
        Dim eStr_Retu As String = String.Empty
        Dim wStr As String = "1=2"
        Dim ds As New DataSet
        Dim dt As New DataTable
        Try
            If Not HttpContext.Current.Session("UserID") Is Nothing Then
                wStr = "EXEC Proc_PIFAuditTrial '" + values.Item("SubjectID") + "'" + "," + HttpContext.Current.Session("UserID")
            Else
                wStr = "EXEC Proc_PIFAuditTrial '" + values.Item("SubjectID") + "'"
            End If

            ds = ObjHelp.GetResultSet(wStr, "PIFAuditTrial")
            If Not ds Is Nothing And ds.Tables.Count > 0 Then
                ds.Tables(0).TableName = "tblPersonalDetailAuditTrial"
                ds.Tables(1).TableName = "tblPersonalProofDetailAuditTrial"
                ds.Tables(2).TableName = "tblPersonalHabitDetailAuditTrial"
                ds.Tables(3).TableName = "tblFemaleDetailAuditTrial"
                ds.Tables(4).TableName = "tblContactDetailAuditTrial"
            End If
            Return JsonConvert.SerializeObject(ds)
        Catch ex As Exception
            Return JsonConvert.SerializeObject(ex.Message)
        End Try

    End Function

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
End Class
