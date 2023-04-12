
Partial Class CDMS_frmCDMSSubjectChangeStatus
    Inherits System.Web.UI.Page

#Region "Variable Declaration "

    Private objCommon As New clsCommon
    Private objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()


    Private Const VS_DtSubjectMst As String = "DtSubjectMst"
    Private Const VS_Mode As String = "Mode"
    Private Const VS_CurrentSubject As String = "CurrentSubject"

    Private Const GVCSub_SrNo As Integer = 0
    Private Const GVCSub_Select As Integer = 1
    Private Const GVCSub_vSubjectCode As Integer = 2
    Private Const GVCSub_vRsvpId As Integer = 3
    Private Const GVCSub_vSubjectId As Integer = 4
    Private Const GVCSub_vSubjectName As Integer = 5
    Private Const GVCSub_vInitials As Integer = 6
    Private Const GVCSub_cSex As Integer = 7
    Private Const GVCSub_dBirthdate As Integer = 8
    Private Const GVCSub_cStatus As Integer = 9
    Private Const GVCSub_dStartDate As Integer = 10
    Private Const GVCSub_dEndDate As Integer = 11


#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Not IsPostBack Then
                Me.AutoCompleteExtender2.ContextKey = "iUserId = " & Me.Session(S_UserID)
                Me.Page.Title = " :: CDMS - Subject Report ::" + System.Configuration.ConfigurationManager.AppSettings("Client")
                Me.ViewState(VS_Mode) = "False"
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try

    End Sub

#End Region

#Region "FillGrid"

    Private Function FillGrid(ByVal mode As String, ByVal dt_Subject As DataTable) As Boolean
        'Dim dt_Subject As DataTable = Nothing
        'Dim dt_sortsubject As DataTable = Nothing
        Try

            Me.gvwSubjectstatus.DataSource = Nothing
            Me.gvwSubjectstatus.DataBind()
            Me.ViewState(VS_CurrentSubject) = Nothing
            Me.btnExport.Visible = False

            If Not dt_Subject Is Nothing Then
                If dt_Subject.Rows.Count > 0 Then

                    dt_Subject = CType(Me.ViewState(VS_DtSubjectMst), DataSet).Tables(0).Copy()
                    'If mode = "Project" Then
                    '    If Me.ddlstatus.SelectedValue <> "0" Then
                    '        dt_sortsubject.DefaultView.RowFilter = "cStatus = '" + Me.ddlstatus.SelectedValue.Trim() + "'"
                    '        dt_Subject = dt_sortsubject.DefaultView.ToTable
                    '    Else
                    '        dt_Subject = dt_sortsubject
                    '    End If
                    'ElseIf mode = "Status" Then
                    '    dt_Subject = dt_sortsubject
                    'End If

                    If dt_Subject.Rows.Count > 0 Then
                        dt_Subject.Columns.Add("vSubjectName", System.Type.GetType("System.String"))
                        For Each dr_Row In dt_Subject.Rows
                            dr_Row("vSubjectName") = dr_Row("vFirstName") + "   " + dr_Row("vMiddleName") + "   " + dr_Row("vSurName")
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
                            Else
                                dr_Row("cStatus") = "Not Found"
                            End If
                        Next
                        Me.btnExport.Visible = True
                        Me.ViewState(VS_CurrentSubject) = dt_Subject
                        Me.gvwSubjectstatus.DataSource = dt_Subject
                        Me.gvwSubjectstatus.DataBind()
                    End If
                End If
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Binding Grid", "....FillGrid")
            Return False
        End Try
    End Function

#End Region

#Region "Grid Events"

    Protected Sub gvwSubjectstatus_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwSubjectstatus.RowCreated

        e.Row.Cells(GVCSub_vSubjectCode).Style.Add("display", "none")
        If Me.ddlstatus.SelectedValue <> "HO" Then
            e.Row.Cells(GVCSub_dStartDate).Style.Add("display", "none")
            e.Row.Cells(GVCSub_dEndDate).Style.Add("display", "none")
        End If
        If Me.hdnProjectForsubject.Value = "" AndAlso Me.txtprojectForsubject.Text = "" Then
            e.Row.Cells(GVCSub_Select).Style.Add("display", "none")
        End If
    End Sub

    Protected Sub gvwSubjectstatus_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwSubjectstatus.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVCSub_SrNo).Text = e.Row.RowIndex + (gvwSubjectstatus.PageSize * gvwSubjectstatus.PageIndex) + 1
            CType(e.Row.Cells(GVCSub_vSubjectId).FindControl("lblSubjectCode"), HyperLink).Text = e.Row.Cells(GVCSub_vSubjectCode).Text
            CType(e.Row.Cells(GVCSub_vSubjectId).FindControl("lblSubjectCode"), HyperLink).NavigateUrl = "frmCDMSSubjectInformation.aspx?Mode=2&SubjectID=" + e.Row.Cells(GVCSub_vSubjectCode).Text

        End If

    End Sub

#End Region

#Region "Button Events"

    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
        Dim Wstr As String = String.Empty
        Dim ds_subject As DataSet = Nothing

        Try

            If Me.hdnProjectForsubject.Value <> "" And Me.txtprojectForsubject.Text.Trim() <> "" Then

                Wstr = "SELECT vSubjectId,vFirstName,vMiddleName,vSurName,vInitials,dBirthdate,cSex,cStatus,vRsvpId,dStartDate,dEndDate FROM View_CDMSSubjectDetailsStatus WHERE  vWorkspaceid = '" + Me.hdnProjectForsubject.Value + "'"

                If Me.ddlstatus.SelectedValue <> "0" Then
                    Wstr += " AND cStatus = '" + Me.ddlstatus.SelectedValue.Trim() + "'"
                End If

                ds_subject = objhelpDb.GetResultSet(Wstr, "View_CDMSSubjectDetailsStatus")

                If ds_subject.Tables(0).Rows.Count > 0 Then
                    Me.ViewState(VS_DtSubjectMst) = ds_subject
                    Me.ViewState(VS_Mode) = "True"
                    If Not FillGrid("Project", ds_subject.Tables(0)) Then
                        Exit Sub
                    End If
                Else
                    Me.gvwSubjectstatus.DataSource = Nothing
                    Me.gvwSubjectstatus.DataBind()
                    Me.btnExport.Visible = False
                End If

            Else

                Wstr = "SELECT vSubjectId,vFirstName,vMiddleName,vSurName,vInitials,dBirthdate,cSex,cStatus,vRsvpId,dStartDate,dEndDate FROM View_CDMSSubjectDetailsStatus WHERE "
                If Me.ddlstatus.SelectedValue <> "0" Then
                    Wstr += " cStatus = '" + Me.ddlstatus.SelectedValue.Trim() + "'"
                End If
                ds_subject = objhelpDb.GetResultSet(Wstr, "View_CDMSSubjectDetailsStatus")

                If ds_subject.Tables(0).Rows.Count > 0 Then
                    Me.ViewState(VS_DtSubjectMst) = ds_subject
                    If Not FillGrid("Status", ds_subject.Tables(0)) Then
                        Exit Sub
                    End If
                Else
                    Me.gvwSubjectstatus.DataSource = Nothing
                    Me.gvwSubjectstatus.DataBind()
                    Me.btnExport.Visible = False
                End If


            End If



        Catch ex As Exception
            Me.ShowErrorMessage("Error While Getting Data", ".....btnGo_Click")
            Exit Sub
        End Try

    End Sub

    Protected Sub btnsetsubjectForProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsetsubjectForProject.Click
        Try
            If Me.hdnProjectForsubject.Value <> "" And Me.txtprojectForsubject.Text.Trim() <> "" Then
                Me.gvwSubjectstatus.DataSource = Nothing
                Me.gvwSubjectstatus.DataBind()
                Me.ddlstatus.SelectedValue = 0
                Me.btnExport.Visible = False
            End If
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Getting Projectid ", ".....btnsetsubjectForProject_Click")
            Exit Sub
        End Try
    End Sub

    Protected Sub btnChangeStatus_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChangeStatus.Click
        Dim ds_Field As New DataSet
        Dim dt_Field As New DataTable
        Dim dr_Field As DataRow
        Dim eStr As String = String.Empty
        Dim chk As CheckBox
        Dim Wstr As String = String.Empty
        Dim ds_subject As DataSet = Nothing
        Dim ds_subjectaftersave As DataSet = Nothing

        Dim ds_Status As New DataSet
        'Dim dt_Status As New DataTable
        Dim dr_Status As DataRow

        Dim vWorkSpaceId As String = String.Empty

        Try

            If ddlchangeStatus.SelectedValue.Trim() = "HO" Then
                Me.txtStatusEndDate.Text = ""
                Me.txtStatusStartDate.Text = ""
                txtStatusStartDate.Attributes.Add("OnChange", "DateConvertForScreening(this.value,this)")
                txtStatusEndDate.Attributes.Add("OnChange", "DateConvertForScreening(this.value,this)")
                Me.ModalChangeStatus.Show()
                Exit Sub
            End If

            dt_Field.Columns.Add("vSubjectID", Type.GetType("System.String"))
            dt_Field.Columns.Add("vTableName", Type.GetType("System.String"))
            dt_Field.Columns.Add("vColumnName", Type.GetType("System.String"))
            dt_Field.Columns.Add("vChangedValue", Type.GetType("System.String"))
            dt_Field.Columns.Add("vRemarks", Type.GetType("System.String"))
            dt_Field.Columns.Add("iModifyBy", Type.GetType("System.Int32"))

            '------Added By Pratik Soni

            Wstr = "SELECT * FROM SubjectDtlCDMSStatus WHERE 1=2"
            ds_Status = objhelpDb.GetResultSet(Wstr, "SubjectDtlCDMSStatus")

            '--------------------

            For index As Integer = 0 To gvwSubjectstatus.Rows.Count - 1

                chk = CType(Me.gvwSubjectstatus.Rows(index).Cells(GVCSub_Select).FindControl("ChkMove"), CheckBox)
                If chk.Checked Then
                    dr_Field = dt_Field.NewRow
                    dr_Field("vSubjectID") = Me.gvwSubjectstatus.Rows(index).Cells(GVCSub_vSubjectCode).Text.ToString.Trim()
                    dr_Field("vTableName") = "SUBJECTDTLCDMS"
                    dr_Field("vColumnName") = "cStatus"
                    dr_Field("vChangedValue") = Me.ddlchangeStatus.SelectedValue.Trim()
                    dr_Field("vRemarks") = "Changed From Subject Status page"
                    dr_Field("iModifyBy") = HttpContext.Current.Session(S_UserID)
                    dt_Field.Rows.Add(dr_Field)

                    '------Added By Pratik Soni
                    
                    dr_Status = ds_Status.Tables(0).NewRow
                    dr_Status("nSubjectDtlCDMSStatusNo") = 0
                    dr_Status("vSubjectId") = Me.gvwSubjectstatus.Rows(index).Cells(GVCSub_vSubjectCode).Text.ToString.Trim()
                    dr_Status("vWorkSpaceId") = hdnProjectForsubject.Value()
                    dr_Status("iTranNo") = 0
                    dr_Status("cStatus") = Me.ddlchangeStatus.SelectedValue.ToString().Trim()
                    dr_Status("iModifyBy") = Session(S_UserID)
                    dr_Status("cStatusIndi") = "N"
                    ds_Status.Tables(0).Rows.Add(dr_Status)

                    '------------------------
                End If
            Next



            ds_Field.Tables.Add(dt_Field)

            If Not objLambda.Insert_UpdateSubjectCDMSDtlField(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Field, _
                                                 HttpContext.Current.Session(S_UserID), eStr) Then

                Exit Sub
            End If


            ' ------- Added by Pratik Soni --------

            If Not objLambda.Save_SubjectDtlCDMSStatus(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Status, eStr) Then
                Exit Sub
            End If

            ' -------------------------------------


            If Me.ViewState(VS_Mode).ToString = "True" Then
                Wstr = "SELECT vSubjectId,vFirstName,vMiddleName,vSurName,vInitials,dBirthdate,cSex,cStatus,vRsvpId,dStartDate,dEndDate FROM View_CDMSSubjectDetailsStatus WHERE  vWorkspaceid = '" + Me.hdnProjectForsubject.Value + "' AND cStatus = '" + Me.ddlchangeStatus.SelectedValue.Trim() + "'"
            Else
                Wstr = "SELECT vSubjectId,vFirstName,vMiddleName,vSurName,vInitials,dBirthdate,cSex,cStatus,vRsvpId,dStartDate,dEndDate FROM VIEW_CDMSSubjectdetails WHERE cStatus = '" + Me.ddlchangeStatus.SelectedValue.Trim() + "'"
            End If

            ds_subjectaftersave = objhelpDb.GetResultSet(Wstr, "View_CDMSSubjectDetailsStatus")

            If ds_subjectaftersave.Tables(0).Rows.Count > 0 Then
                Me.ViewState(VS_DtSubjectMst) = ds_subjectaftersave
                If Not FillGrid("Status", ds_subjectaftersave.Tables(0)) Then
                    Exit Sub
                End If
            Else
                objCommon.ShowAlert("Error While Getting Subject Status Details", Me.Page)
                Me.gvwSubjectstatus.DataSource = Nothing
                Me.gvwSubjectstatus.DataBind()
                Exit Sub
            End If


            objCommon.ShowAlert("Subject Status Changed Sucessfully", Me.Page)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString() + "    ----  Error While Changing Status", "...btnChangeStatus_Click")
            Exit Sub
        End Try
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim fileName As String = String.Empty
        Dim ds As New DataSet
        Try

            If Me.gvwSubjectstatus.Rows.Count < 1 Then
                Me.objCommon.ShowAlert("No Data To Export", Me.Page)
                Exit Sub
            End If
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            fileName = "User Status Report"
            fileName = fileName & ".xls"
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            ds.Tables.Add(CType(Me.ViewState(VS_CurrentSubject), DataTable).Copy())
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
            Status = "LIST OF SUBJECTS :" + Me.ddlstatus.SelectedItem.Text.Trim()
            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""6""><center><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""6""><center><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append(Status)
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")

            dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vSubjectID,vRsvpId,vSubjectName,vInitials,cSex,cStatus".Split(",")).DefaultView.ToTable())
            dsConvert.AcceptChanges()
            dsConvert.Tables(0).Columns(0).ColumnName = "Subject ID"
            dsConvert.Tables(0).Columns(1).ColumnName = "RSVP ID"
            dsConvert.Tables(0).Columns(2).ColumnName = "Subject Name"
            dsConvert.Tables(0).Columns(3).ColumnName = "Initials"
            dsConvert.Tables(0).Columns(4).ColumnName = "Gender"
            dsConvert.Tables(0).Columns(5).ColumnName = "Status"
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

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim ds_Field As New DataSet
        Dim dt_Field As New DataTable
        Dim dr_Field As DataRow
        Dim eStr As String = String.Empty
        Dim chk As CheckBox
        Dim Wstr As String = String.Empty
        Dim ds_subject As DataSet = Nothing
        Dim ds_subjectaftersave As DataSet = Nothing
        Dim ds_Status As New DataSet
        Dim dr_Status As DataRow

        Try
            dt_Field.Columns.Add("vSubjectID", Type.GetType("System.String"))
            dt_Field.Columns.Add("vTableName", Type.GetType("System.String"))
            dt_Field.Columns.Add("vColumnName", Type.GetType("System.String"))
            dt_Field.Columns.Add("vChangedValue", Type.GetType("System.String"))
            dt_Field.Columns.Add("vRemarks", Type.GetType("System.String"))
            dt_Field.Columns.Add("iModifyBy", Type.GetType("System.Int32"))

            Wstr = "SELECT * FROM SubjectDtlCDMSStatus WHERE 1=2"
            ds_Status = objhelpDb.GetResultSet(Wstr, "SubjectDtlCDMSStatus")

            For index As Integer = 0 To gvwSubjectstatus.Rows.Count - 1
                chk = CType(Me.gvwSubjectstatus.Rows(index).Cells(GVCSub_Select).FindControl("ChkMove"), CheckBox)
                If chk.Checked Then
                    dr_Field = dt_Field.NewRow
                    dr_Field("vSubjectID") = Me.gvwSubjectstatus.Rows(index).Cells(GVCSub_vSubjectCode).Text.ToString.Trim()
                    dr_Field("vTableName") = "SUBJECTDTLCDMS"
                    dr_Field("vColumnName") = "cStatus"
                    dr_Field("vChangedValue") = Me.ddlchangeStatus.SelectedValue.Trim()
                    dr_Field("vRemarks") = "Changed From Subject Status page"
                    dr_Field("iModifyBy") = HttpContext.Current.Session(S_UserID)
                    dt_Field.Rows.Add(dr_Field)
                    If Me.txtStatusStartDate.Text.Trim() <> "" Then
                        dr_Field = dt_Field.NewRow
                        dr_Field("vSubjectID") = Me.gvwSubjectstatus.Rows(index).Cells(GVCSub_vSubjectCode).Text.ToString.Trim()
                        dr_Field("vTableName") = "SUBJECTDTLCDMS"
                        dr_Field("vColumnName") = "dStartDate"
                        dr_Field("vChangedValue") = Me.txtStatusStartDate.Text.Trim()
                        dr_Field("vRemarks") = "Changed From Subject Status page"
                        dr_Field("iModifyBy") = HttpContext.Current.Session(S_UserID)
                        dt_Field.Rows.Add(dr_Field)
                    End If
                    If Me.txtStatusStartDate.Text.Trim() <> "" Then
                        dr_Field = dt_Field.NewRow
                        dr_Field("vSubjectID") = Me.gvwSubjectstatus.Rows(index).Cells(GVCSub_vSubjectCode).Text.ToString.Trim()
                        dr_Field("vTableName") = "SUBJECTDTLCDMS"
                        dr_Field("vColumnName") = "dEndDate"
                        dr_Field("vChangedValue") = Me.txtStatusStartDate.Text.Trim()
                        dr_Field("vRemarks") = "Changed From Subject Status page"
                        dr_Field("iModifyBy") = HttpContext.Current.Session(S_UserID)
                        dt_Field.Rows.Add(dr_Field)
                    End If
                    dr_Status = ds_Status.Tables(0).NewRow
                    dr_Status("nSubjectDtlCDMSStatusNo") = 0
                    dr_Status("vSubjectId") = Me.gvwSubjectstatus.Rows(index).Cells(GVCSub_vSubjectCode).Text.ToString.Trim()
                    dr_Status("vWorkSpaceId") = hdnProjectForsubject.Value()
                    dr_Status("iTranNo") = 0
                    dr_Status("cStatus") = Me.ddlchangeStatus.SelectedValue.ToString().Trim()
                    dr_Status("iModifyBy") = Session(S_UserID)
                    dr_Status("cStatusIndi") = "N"
                    ds_Status.Tables(0).Rows.Add(dr_Status)
                End If
            Next

            ds_Field.Tables.Add(dt_Field)

            If Not objLambda.Insert_UpdateSubjectCDMSDtlField(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Field, _
                                                 HttpContext.Current.Session(S_UserID), eStr) Then

                Exit Sub
            End If


            If Not objLambda.Save_SubjectDtlCDMSStatus(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Status, eStr) Then
                Exit Sub
            End If

            If Me.ViewState(VS_Mode).ToString = "True" Then
                Wstr = "SELECT vSubjectId,vFirstName,vMiddleName,vSurName,vInitials,dBirthdate,cSex,cStatus,vRsvpId,dStartDate,dEndDate FROM View_CDMSSubjectDetailsStatus WHERE  vWorkspaceid = '" + Me.hdnProjectForsubject.Value + "' AND cStatus = '" + Me.ddlchangeStatus.SelectedValue.Trim() + "'"
            Else
                Wstr = "SELECT vSubjectId,vFirstName,vMiddleName,vSurName,vInitials,dBirthdate,cSex,cStatus,vRsvpId,dStartDate,dEndDate FROM VIEW_CDMSSubjectdetails WHERE cStatus = '" + Me.ddlchangeStatus.SelectedValue.Trim() + "'"
            End If

            ds_subjectaftersave = objhelpDb.GetResultSet(Wstr, "View_CDMSSubjectDetailsStatus")

            If ds_subjectaftersave.Tables(0).Rows.Count > 0 Then
                Me.ViewState(VS_DtSubjectMst) = ds_subjectaftersave
                Me.ddlstatus.SelectedValue = Me.ddlchangeStatus.SelectedValue.Trim()
                If Not FillGrid("Status", ds_subjectaftersave.Tables(0)) Then
                    Exit Sub
                End If
            Else
                objCommon.ShowAlert("Error While Getting Subject Status Details", Me.Page)
                Me.gvwSubjectstatus.DataSource = Nothing
                Me.gvwSubjectstatus.DataBind()
                Exit Sub
            End If

            Me.ModalChangeStatus.Hide()
            objCommon.ShowAlert("Subject Status Changed Sucessfully", Me.Page)

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Response.Redirect("frmCDMSSubjectChangeStatus.aspx")
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


End Class
