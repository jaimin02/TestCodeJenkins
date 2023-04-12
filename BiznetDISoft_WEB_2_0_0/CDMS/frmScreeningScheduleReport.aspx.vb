
Partial Class CDMS_frmScreeningScheduleReport
    Inherits System.Web.UI.Page

#Region "Variable Declaration "

    Private objCommon As New clsCommon
    Private objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()


    Private Const VS_SubjectDetail As String = "SubjectDetail"

    Private Const GVCSub_SrNo As Integer = 0
    Private Const GVCSub_vProjectNo As Integer = 1
    Private Const GVCSub_vSubjectId As Integer = 2
    Private Const GVCSub_vRsvpId As Integer = 3
    Private Const GVCSub_vSubjectName As Integer = 4
    Private Const GVCSub_vInitials As Integer = 5
    Private Const GVCSub_dBirthdate As Integer = 6
    Private Const GVCSub_cSex As Integer = 7
    Private Const GVCSub_vContactNo1 As Integer = 8
    Private Const GVCSub_dScheduledate As Integer = 9
    Private Const GVCSub_vStartTime As Integer = 10
    Private Const GVCSub_cStatus As Integer = 11



#End Region

#Region "Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Not IsPostBack Then
                Me.AutoCompleteExtender2.ContextKey = "iUserId = " & Me.Session(S_UserID)
                Me.Page.Title = " :: CDMS - Screening Schedule Report ::" + System.Configuration.ConfigurationManager.AppSettings("Client")
                CType(Master.FindControl("lblHeading"), Label).Text = "Screening Schedule Report"
                Me.rblstatus.SelectedValue = 0
                Me.trProject.Style.Add("display", "")
                If Not FillActivityDropDown() Then
                    objCommon.ShowAlert("Error While Filling Location List", Me.Page)
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, ".....Page_Load")
        End Try

    End Sub

#End Region

#Region "Rblstatus Events "

    Protected Sub rblstatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblstatus.SelectedIndexChanged
        If Me.rblstatus.SelectedValue = 0 Then
            Me.trProject.Style.Add("display", "")
            ' Me.trDate.Style.Add("display", "none")
            Me.txtprojectForsubject.Text = ""
            Me.hdnProjectForsubject.Value = Nothing
            Me.ddlLoction.SelectedValue = 0
        ElseIf Me.rblstatus.SelectedValue = 1 Then
            Me.trProject.Style.Add("display", "none")
            Me.trDate.Style.Add("display", "")
            Me.txtdate.Text = ""
            Me.ddlLoction.SelectedValue = 0
        End If
        Me.GrdSubject.DataSource = Nothing
        Me.GrdSubject.DataBind()
        Me.btnExport.Visible = False
        Me.ddlLoction.SelectedValue = "0042"
    End Sub

#End Region

#Region "Fill DropDown"
    Public Function FillActivityDropDown() As Boolean
        Dim Wstr As String = String.Empty
        Dim Estr As String = String.Empty
        Dim Ds_location As New DataSet



        Try
            Wstr = "SELECT vLocationName,vLocationCode  FROM LocationMst"

            Ds_location = objhelpDb.GetResultSet(Wstr, "LocationMst")
            If Ds_location.Tables(0).Rows.Count = 0 Then
                Throw New Exception("Location List Not Found.")
                Exit Function
            End If
            Me.ddlLoction.DataSource = Ds_location.Tables(0)
            Me.ddlLoction.DataTextField = "vLocationName"
            Me.ddlLoction.DataValueField = "vLocationCode"
            Me.ddlLoction.DataBind()
            Me.ddlLoction.Items.Insert(0, New ListItem(" Select Location", "0"))
            Me.ddlLoction.SelectedValue = "0042"
            Return True

        Catch ex As Exception
            Me.objCommon.ShowAlert(ex.Message.ToString, Me.Page)
            Exit Function
        End Try

    End Function
#End Region

#Region "Fill Grid "

    Private Function FillGrid(ByVal Mode As String, ByVal ds_Subject As DataSet) As Boolean
        Dim wStr As String = String.Empty
        Dim estr As String = String.Empty
        Dim dr_Row As DataRow


        Try
            If ds_Subject.Tables(0).Rows.Count < 0 Then
                objCommon.ShowAlert("Error While Getting All Subjects List", Me.Page)
                Me.GrdSubject.DataSource = Nothing
                Me.GrdSubject.DataBind()
                Me.btnExport.Visible = False
                Exit Function
            End If
            If Mode = 1 Then
                ds_Subject.Tables(0).Columns.Add("vSubjectName", System.Type.GetType("System.String"))

                For Each dr_Row In ds_Subject.Tables(0).Rows
                    dr_Row("vSubjectName") = dr_Row("vFirstName") + " " + dr_Row("vMiddleName") + " " + dr_Row("vSurName")
                    If dr_Row("cStatus") = "AC" Then
                        dr_Row("cStatus") = "Active"
                    ElseIf dr_Row("cStatus") = "IA" Then
                        dr_Row("cStatus") = "In Active"
                    ElseIf dr_Row("cStatus") = "HO" Then
                        If Not IsDBNull(dr_Row("dEndDate")) Then
                            If Date.Now > dr_Row("dEndDate") Then
                                dr_Row("cStatus") = "Active"
                            Else
                                dr_Row("cStatus") = "Hold"
                            End If
                        Else
                            dr_Row("cStatus") = "Hold"
                        End If
                    ElseIf dr_Row("cStatus") = "SC" Then
                        dr_Row("cStatus") = "Screened"
                    ElseIf dr_Row("cStatus") = "BO" Then
                        dr_Row("cStatus") = "Booked"
                    ElseIf dr_Row("cStatus") = "OS" Then
                        dr_Row("cStatus") = "On Study"
                    ElseIf dr_Row("cStatus") = "FI" Then
                        dr_Row("cStatus") = "Forever Ineligible"
                    End If
                Next

            End If


            Me.ViewState(VS_SubjectDetail) = ds_Subject.Copy
            Me.GrdSubject.DataSource = ds_Subject.Tables(0)
            Me.GrdSubject.DataBind()
            Me.btnExport.Visible = True
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "GrdSubject", "javascript:createGrdSubject();", True)
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....Fill Grid")
            Me.GrdSubject.DataSource = Nothing
            Me.GrdSubject.DataBind()
            Me.btnExport.Visible = False
            Return False
        End Try
    End Function

#End Region

#Region "GrdSubject Events "

    Protected Sub GrdSubject_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GrdSubject.PageIndexChanging
        GrdSubject.PageIndex = e.NewPageIndex
        FillGrid(2, CType(Me.ViewState(VS_SubjectDetail), DataSet).Copy())
    End Sub

    'Protected Sub GrdSubject_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GrdSubject.RowCreated
    '    If e.Row.RowType = DataControlRowType.DataRow Or _
    '            e.Row.RowType = DataControlRowType.Header Or _
    '            e.Row.RowType = DataControlRowType.Footer Then


    '    End If
    'End Sub

    Protected Sub GrdSubject_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)


        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVCSub_SrNo).Text = e.Row.RowIndex + (GrdSubject.PageSize * GrdSubject.PageIndex) + 1
        End If

    End Sub

#End Region

#Region "Button Events "

    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
        Dim ds_Subject As DataSet = Nothing
        Dim Wstr As String = String.Empty
        Dim Estr As String = String.Empty

        Try
            Wstr = "SELECT vWorkSpaceId,vSubjectId,vProjectNo,dStartTime,vLocationCode,dScheduledate,vFirstName,vMiddleName,vSurName,vInitials,vRsvpId,dBirthdate,cSex,cStatus,vContactNo1,dStartDate,dEndDate FROM View_CDMSSubjectDetailsStatus"
            If Me.rblstatus.SelectedValue = 0 Then

                If Me.hdnProjectForsubject.Value <> "" Then
                    Wstr += " WHERE vWorkspaceId = '" + Me.hdnProjectForsubject.Value.ToString().Trim() + "' AND vLocationCode = '" + Me.ddlLoction.SelectedValue.ToString.Trim() + "'"
                    If Me.txtdate.Text.Trim() <> "" Then
                        Wstr += " AND dScheduledate = '" + Me.txtdate.Text.Trim() + "'"
                    End If
                    Wstr += "ORDER BY vProjectNo,dScheduledate,dStartTime"
                End If

            ElseIf Me.rblstatus.SelectedValue = 1 Then

                Wstr += " WHERE dScheduledate = '" + Me.txtdate.Text.Trim() + "' AND vLocationCode = '" + Me.ddlLoction.SelectedValue.ToString.Trim() + "' ORDER BY vProjectNo,dScheduledate,dStartTime"

            End If
            ds_Subject = objhelpDb.GetResultSet(Wstr, "View_CDMSSubjectDetailsStatus")

            If Not ds_Subject Is Nothing Then
                If ds_Subject.Tables(0).Rows.Count > 0 Then
                    If Not FillGrid(1, ds_Subject) Then
                        Me.GrdSubject.DataSource = Nothing
                        Me.GrdSubject.DataBind()
                        Me.btnExport.Visible = False
                        Exit Sub
                    End If
                Else
                    objCommon.ShowAlert("No Record Found", Me.Page)
                    Me.GrdSubject.DataSource = Nothing
                    Me.GrdSubject.DataBind()
                    Me.btnExport.Visible = False
                    Exit Sub
                End If
            Else
                objCommon.ShowAlert("No Record Found", Me.Page)
                Me.GrdSubject.DataSource = Nothing
                Me.GrdSubject.DataBind()
                Me.btnExport.Visible = False
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....btnGo_Click")
            Exit Try
        End Try
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim fileName As String = String.Empty
        Dim ds As New DataSet
        Try

            If Me.GrdSubject.Rows.Count < 1 Then
                Me.objCommon.ShowAlert("No Data To Export", Me.Page)
                Exit Sub
            End If
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            fileName = "User Schedule Report"
            fileName = fileName & ".xls"
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)


            ds.Tables.Add(CType(Me.ViewState(VS_SubjectDetail), DataSet).Tables(0).Copy())
            ds.AcceptChanges()

            Context.Response.Write(ConvertDsuserTO(ds))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception
            'Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

    Private Function ConvertDsuserTO(ByVal ds As DataSet) As String
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer
        Dim dsConvert As New DataSet
        Dim Status As String = String.Empty

        Try
            If Me.rblstatus.SelectedValue = 0 Then
                Status = "Screening Location : " + Me.ddlLoction.SelectedItem.Text.Trim() + " Master List For Project No : " + Me.txtprojectForsubject.Text.Trim()
            ElseIf Me.rblstatus.SelectedValue = 1 Then
                Status = "Screening Location : " + Me.ddlLoction.SelectedItem.Text.Trim() + "                Screening Date : " + Me.txtdate.Text.Trim()
            End If

            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            'strMessage.Append("<td colspan=""5"">")
            'strMessage.Append(Convert.ToBase64String(byt))
            'strMessage.Append("</td>")
            strMessage.Append("<td colspan=""11""><center><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""11""><center><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append(Status)
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")
            ds.Tables(0).Columns.Add("DOB", System.Type.GetType("System.String"))
            For Each dr_Row In ds.Tables(0).Rows
                dr_Row("DOB") = CDate(dr_Row("dBirthDate")).ToString("dd-MMM-yyyy")
            Next
            dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vProjectNo,vSubjectID,vRsvpId,vSubjectName,vInitials,DOB,cSex,vContactNo1,dStartTime,dScheduledate,cStatus".Split(",")).DefaultView.ToTable())
            dsConvert.AcceptChanges()
            dsConvert.Tables(0).Columns(0).ColumnName = "Project No"
            dsConvert.Tables(0).Columns(1).ColumnName = "Subject ID"
            dsConvert.Tables(0).Columns(2).ColumnName = "RSVP ID"
            dsConvert.Tables(0).Columns(3).ColumnName = "Subject Name"
            dsConvert.Tables(0).Columns(4).ColumnName = "Initials"
            dsConvert.Tables(0).Columns(5).ColumnName = "D.O.B."
            dsConvert.Tables(0).Columns(6).ColumnName = "Gender"
            dsConvert.Tables(0).Columns(7).ColumnName = "Contact No"
            dsConvert.Tables(0).Columns(8).ColumnName = "Appointment Time"
            dsConvert.Tables(0).Columns(9).ColumnName = "Appointment Date"
            dsConvert.Tables(0).Columns(10).ColumnName = "Status"
            dsConvert.AcceptChanges()

            For iCol = 0 To dsConvert.Tables(0).Columns.Count - 1

                strMessage.Append("<td><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).Trim())
                strMessage.Append("</font></strong></td>")

            Next
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")

            For j = 0 To dsConvert.Tables(0).Rows.Count - 1
                strMessage.Append("<tr>")
                For i = 0 To dsConvert.Tables(0).Columns.Count - 1

                    strMessage.Append("<td><font color=""#000000"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim())
                    strMessage.Append("</font></td>")

                Next
                strMessage.Append("</tr>")
            Next
            strMessage.Append("</table>")

            Return strMessage.ToString

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....ConvertDsuserTO")
            Return ""
        End Try
    End Function

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Response.Redirect("frmScreeningScheduleReport.aspx")
    End Sub

#End Region

#Region "Error Handler "

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub

#End Region


    
End Class
