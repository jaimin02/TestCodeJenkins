Imports System.Web
Imports System.Xml
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Partial Class frmRptDeleteRepetition
    Inherits System.Web.UI.Page
#Region "Variable Declaration"
    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
#End Region

#Region "Page Events"
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
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
            Page.Title = " :: CRF Activity Deletion Report ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")

            CType(Master.FindControl("lblHeading"), Label).Text = "CRF Activity Deletion Report"
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
            End If
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

    Private Function FillDropDownList(ByVal iPeriod As String, ByVal vActivity As String, ByVal vSubject As String, vDeletedBy As String, ByRef eStr As String) As Boolean
        Dim dsDeletedRecords As New DataSet
        Dim estr_retu As String = String.Empty
        Dim Periods As Integer = 1
        Dim lItem As ListItem
        Dim paramPeriod() As String = {"Period"}
        Dim paramActivity() As String = {"Workspace Id.", "Activity Id.", "Activity Name"}
        Dim paramSubject() As String = {"Workspace Id.", "Subject No.", "Subject Id.", "SubjectId"}
        Dim paramDeletedBy() As String = {"Deleted User Id.", "Deleted By"}
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            'Me.ddlSubject.Items.Clear()

            If Not objHelp.Proc_GetDeletedCRFRecords(Me.HProjectId.Value.Trim(), iPeriod, vActivity, vSubject, vDeletedBy, IIf(Me.chkParent.Style("display") = "", "Y", "N"), IIf(Me.chkParent.Style("display") = "none", "Y", "N"), dsDeletedRecords, estr_retu) Then
                Throw New Exception(eStr)
            End If

            If dsDeletedRecords Is Nothing OrElse dsDeletedRecords.Tables(0).Rows.Count = 0 Then
                objcommon.ShowAlert("No data found for selected project", Me)
                Return False
                Exit Function
            End If

            If iPeriod.ToString = "" Then
                Me.ddlPeriods.DataSource = dsDeletedRecords.Tables(0).DefaultView.ToTable(True, paramPeriod)
                Me.ddlPeriods.DataTextField = "Period"
                Me.ddlPeriods.DataValueField = "Period"
                Me.ddlPeriods.DataBind()
                Me.ddlPeriods.Items.Insert(0, "All")
            End If

            If vActivity.ToString = "" Then
                Me.ddlActivity.DataSource = dsDeletedRecords.Tables(0).DefaultView.ToTable(True, paramActivity)
                Me.ddlActivity.DataTextField = "Activity Name"
                Me.ddlActivity.DataValueField = "Activity Id."
                Me.ddlActivity.DataBind()
                Me.ddlActivity.Items.Insert(0, "All")
            End If


            'Me.ddlSubject.DataSource = dsDeletedRecords.Tables(0).DefaultView.ToTable(True, paramSubject)
            'Me.ddlSubject.DataTextField = "SubjectId"
            'Me.ddlSubject.DataValueField = "vSubjectId"
            'Me.ddlSubject.DataBind()
            'Me.ddlSubject.Items.Insert(0, "All")

            'ddlSubject.AddItems(dsDeletedRecords.Tables(0).DefaultView.ToTable(True, paramSubject), "SubjectId", "vSubjectId")
            '  ddlSubject.AssignText("All")
            If vSubject.ToString = "" Then
                For Each dr As DataRow In dsDeletedRecords.Tables(0).DefaultView.ToTable(True, paramSubject).Rows
                    lItem = New ListItem
                    'lItem.Text = dr("SubjectId")
                    'lItem.Value = dr("Subject Id.")
                    'Me.chkLstSubjects.Items.Add(lItem)
                    Me.chkLstSubjects.Items.Add(dr("SubjectId"))
                Next dr
            End If

            If vDeletedBy.ToString = "" Then
                Me.ddlDeletedBy.DataSource = dsDeletedRecords.Tables(0).DefaultView.ToTable(True, paramDeletedBy)
                Me.ddlDeletedBy.DataTextField = "Deleted By"
                Me.ddlDeletedBy.DataValueField = "Deleted User Id."
                Me.ddlDeletedBy.DataBind()
                Me.ddlDeletedBy.Items.Insert(0, "All")
            End If

            Return True

        Catch ex As Exception
            ShowErrorMessage("Problem While Filling Periods. ", ex.Message)
            eStr = ex.Message
            Return False
        End Try

    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim eStr As String = String.Empty
        Dim dsWorkSpace As New DataSet
        Dim vPeriod As String = String.Empty
        Dim vActivity As String = String.Empty
        Dim vSubject As String = String.Empty
        Dim vDeletedBy As String = String.Empty
        Try
            ClearData()
            Me.ddlPeriods.Items.Clear()
            Me.ddlActivity.Items.Clear()
            Me.ddlDeletedBy.Items.Clear()
            Me.fldgrdParent.Style.Add("display", "none")

            If Not objHelp.getworkspacemst(" vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsWorkSpace, eStr) Then
                Throw New Exception(eStr)
            End If

            If dsWorkSpace Is Nothing OrElse dsWorkSpace.Tables(0).Rows.Count = 0 Then
                Throw New Exception("No records available in WorkSpaceMst.")
            End If
            Me.chkParent.Checked = False
            Me.chkParent.Style("display") = "none"

            If dsWorkSpace.Tables(0).Rows(0)("cWorkSpaceType") = "P" Then
                Me.chkParent.Style("display") = ""
                Me.chkParent.Checked = True
            End If

            If Not FillDropDownList("", "", "", "", eStr) Then
                Throw New Exception(eStr)
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")

        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("frmRptDeleteRepetition.aspx")
    End Sub

    Protected Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        If Not Me.FillGrid() Then
            Exit Sub
        End If
    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Dim fileName As String = String.Empty
        Dim Str As String = String.Empty
        Try

            fileName = CType(Master.FindControl("lblHeading"), Label).Text + "_" + Me.txtproject.Text.Split(" ")(0) + ".xls"

            Dim stringWriter As New System.IO.StringWriter()
            Dim writer As New HtmlTextWriter(stringWriter)

            Str = "<div><table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0""><tr><td colspan=""9""><center><strong><font color=""black"" size=""8px"" face=""Calibri"">" + CType(Master.FindControl("lblHeading"), Label).Text + "_" + Me.txtproject.Text.Split(" ")(0) + "</font></strong></td></tr><tr><td colspan=""9""></td></tr></table></div>"

            Me.gvDeletedRecords.RenderControl(writer)
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

    Protected Sub btnPDF_Click(sender As Object, e As EventArgs) Handles btnPDF.Click
        Dim fileName As String = String.Empty

        Try

            fileName = CType(Master.FindControl("lblHeading"), Label).Text + "_" + Me.txtproject.Text.Split(" ")(0) + ".pdf"
            Response.Clear()
            Response.Buffer = True
            Response.AddHeader("Content-type", "application/pdf")
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName)
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Dim StringWriter1 As New StringWriter()
            Dim HtmlTextWriter1 As New HtmlTextWriter(StringWriter1)
            gvDeletedRecords.HeaderRow.ForeColor = Drawing.Color.Navy
            gvDeletedRecords.HeaderRow.Style.Add("width", "15%")
            gvDeletedRecords.HeaderRow.Style.Add("font-size", "10px")
            gvDeletedRecords.HeaderRow.HorizontalAlign = HorizontalAlign.Center
            gvDeletedRecords.Style.Add("text-decoration", "none")
            gvDeletedRecords.Style.Add("font-family", "Arial, Helvetica, sans-serif;")
            gvDeletedRecords.Style.Add("font-size", "8px")
            gvDeletedRecords.Style.Add("font-color", "blue")
            Dim p As Paragraph = New Paragraph(CType(Master.FindControl("lblHeading"), Label).Text + "_" + Me.txtproject.Text.Split(" ")(0))
            p.Alignment = Element.ALIGN_CENTER
            p.SpacingAfter = 10
            gvDeletedRecords.RenderControl(HtmlTextWriter1)
            Dim StringReader1 As New StringReader(StringWriter1.ToString())
            Dim newDocument As New Document(PageSize.A4, 7.0F, 7.0F, 7.0F, 7.0F)
            Dim HTMLWorker1 As New HTMLWorker(newDocument)
            PdfWriter.GetInstance(newDocument, Response.OutputStream)
            newDocument.Open()
            newDocument.Add(p)
            HTMLWorker1.Parse(StringReader1)
            newDocument.Close()
            Response.Write(newDocument)
            gvDeletedRecords.DataSource = Nothing
            Response.End()


        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "Drop down events"

    Protected Sub ddlPeriods_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPeriods.SelectedIndexChanged
        Dim estr As String = String.Empty
        Try
            Me.ddlActivity.Items.Clear()
            Me.ddlDeletedBy.Items.Clear()
            ClearData()

            If Not FillDropDownList(IIf(Me.ddlPeriods.SelectedItem.Text.ToUpper = "ALL", "", Me.ddlPeriods.SelectedValue.ToString), "", "", "", estr) Then
                Throw New Exception(estr)
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub ddlActivity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlActivity.SelectedIndexChanged
        Dim estr As String = String.Empty
        Try
            Me.ddlDeletedBy.Items.Clear()
            ClearData()

            If Not FillDropDownList(IIf(Me.ddlPeriods.SelectedItem.Text.ToUpper = "ALL", "", Me.ddlPeriods.SelectedValue.ToString), IIf(Me.ddlActivity.SelectedItem.Text.ToUpper = "ALL", "", Me.ddlActivity.SelectedValue.ToString), "", "", estr) Then
                Throw New Exception(estr)
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub ddlDeletedBy_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDeletedBy.SelectedIndexChanged
        Dim estr As String = String.Empty
        Try

            ClearData()

            If Not FillDropDownList(IIf(Me.ddlPeriods.SelectedItem.Text.ToUpper = "ALL", "", Me.ddlPeriods.SelectedValue.ToString), IIf(Me.ddlActivity.SelectedItem.Text.ToUpper = "ALL", "", Me.ddlActivity.SelectedValue.ToString), "", IIf(Me.ddlDeletedBy.SelectedItem.Text.ToUpper = "ALL", "", Me.ddlDeletedBy.SelectedValue.ToString), estr) Then
                Throw New Exception(estr)
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "Fill Grid"
    Private Function FillGrid() As Boolean
        Dim vPeriod As String = String.Empty
        Dim vActivity As String = String.Empty
        Dim vSubject As String = String.Empty
        Dim vDeletedBy As String = String.Empty
        Dim eStr As String = String.Empty
        Dim dsDeletedRecords As New DataSet
        Dim paramActivity() As String = {"Project No.", "Subject Id.", "Subject No.", "Period", "Parent Activity Name", "Activity Name", "Repetition No.", "Deleted By", "Deleted On", "Deletion Remarks"}
        Dim paramAttribute() As String = {"Project No.", "Subject Id.", "Subject No.", "Period", "Parent Activity Name", "Activity Name", "Repetition No.", "Attribute Name", "Attribute Value", "Modify By", "Modify On", "Attribute Remarks", "Deleted By", "Deleted On", "Deletion Remarks"}
        Try

            'Me.gvDeletedRecords.DataSource = Nothing
            'Me.gvDeletedRecords.DataBind()
            'Me.btnExport.Style("display") = "none"

            vPeriod = IIf(Me.ddlPeriods.SelectedItem.Text.ToUpper = "ALL", "", Me.ddlPeriods.SelectedValue.ToString)
            vActivity = IIf(Me.ddlActivity.SelectedItem.Text.ToUpper = "ALL", "", Me.ddlActivity.SelectedValue.ToString)
            vDeletedBy = IIf(Me.ddlDeletedBy.SelectedItem.Text.ToUpper = "ALL", "", Me.ddlDeletedBy.SelectedValue.ToString)

            For Each item In Me.chkLstSubjects.Items
                If item.Selected Then
                    vSubject += item.Value.Trim().ToString.Split("||")(2).Trim() + ","
                End If
            Next item

            vSubject = IIf(Me.chkSelectAll.Checked = True, "", vSubject)

            If Not objHelp.Proc_GetDeletedCRFRecords(Me.HProjectId.Value.Trim(), vPeriod, vActivity, vSubject, vDeletedBy, IIf(Me.chkParent.Checked = True, "Y", "N"), IIf(Me.chkParent.Style("display") = "none", "Y", "N"), dsDeletedRecords, eStr) Then
                Throw New Exception(eStr)
            End If

            If dsDeletedRecords.Tables(0).Rows.Count = 0 Then
                Me.gvDeletedRecords.DataSource = Nothing
                Me.gvDeletedRecords.DataBind()
                Me.btnExport.Style("display") = "none"
                Me.btnPDF.Style("display") = "none"
                objcommon.ShowAlert("No Record found.", Me)
                Return False
                Exit Function
            End If

            Me.gvDeletedRecords.DataSource = IIf(Me.ddlRptType.SelectedValue.ToString.ToUpper = "ACT", dsDeletedRecords.Tables(0).DefaultView.ToTable(True, paramActivity), dsDeletedRecords.Tables(0).DefaultView.ToTable(True, paramAttribute))
            Me.gvDeletedRecords.DataBind()
            Me.btnExport.Style("display") = ""
            Me.btnPDF.Style("display") = ""
            Me.fldgrdParent.Style.Add("display", "")

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Filling Grid. ", ex.Message)
            Return False
        End Try
    End Function
#End Region
    
#Region "Clear Data"
    Private Function ClearData() As Boolean
        Me.chkSelectAll.Checked = False
        Me.chkLstSubjects.Items.Clear()
        Me.gvDeletedRecords.DataSource = Nothing
        Me.gvDeletedRecords.DataBind()
        Me.btnExport.Style("display") = "none"
        Me.btnPDF.Style("display") = "none"
        Return True
    End Function
#End Region
    
End Class
