Imports Newtonsoft.Json
Partial Class CDMS_frmCDMSStudyStatusLog
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objCommon As New clsCommon
    Private objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()


    Private Const VS_DtParent As String = "DtParent"
    
    Private Const gvwSubjectForProject_SrNo As Integer = 0
    Private Const gvwSubjectForProject_cStatus As Integer = 1
    Private Const gvwSubjectForProject_TotalSubject As Integer = 2

    Private Const gvChildGrid_SrNo As Integer = 0
    Private Const gvChildGrid_vSubjectID As Integer = 1
    Private Const gvChildGrid_FullName As Integer = 2
    Private Const gvChildGrid_Age As Integer = 3


    Dim FOOTERVALUE As Integer = 0

#End Region

#Region "Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Me.Page.Title = " :: CDMS :: Study Status Log ::" + System.Configuration.ConfigurationManager.AppSettings("Client")
            If Not IsPostBack Then
                Me.AutoCompleteExtender2.ContextKey = "iUserId = " & Me.Session(S_UserID)
                CType(Master.FindControl("lblHeading"), Label).Text = "Study Status Log"
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, ".....Page_Load")
        End Try

    End Sub

#End Region

#Region "Button Events"

    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click

        Dim ds_subject As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty

        Try

            If Me.HdnWorkspaceId.Value <> "" And Me.txtsearch.Text.ToString().Trim() <> "" Then

                wStr = "SELECT * FROM View_SubjectDtlCDMSStatusLog where vWorkSpaceId = '" + Me.HdnWorkspaceId.Value.ToString().Trim() + "'"
                'wStr = "SELECT vSubjectId,vFirstName,cStatus FROM View_CDMSSubjectDetailsStatus WHERE  vWorkspaceid = '" + Me.HdnWorkspaceId.Value.ToString().Trim() + "' AND cStatus = 'BO'"

                If Not objhelpDb.View_GetSubjectDtlCDMSStatusLog(wStr, ds_subject, eStr) Then
                    Throw New Exception(eStr)
                End If

                If ds_subject Is Nothing Then
                    Exit Sub
                End If

                If ds_subject.Tables(0).Rows.Count > 0 Then

                    Me.ViewState(VS_DtParent) = ds_subject.Tables(0)

                    If Not FillGrid("ParentGrid", ds_subject.Tables(0)) Then
                        Exit Sub
                    End If

                Else
                    Me.gvwSubjectForProject.DataSource = Nothing
                    Me.gvwSubjectForProject.DataBind()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Display", "ButtonexportHide();", True)
                    ' Me.btnExport.Visible = False
                    Me.objCommon.ShowAlert("No Data Found For This Project.", Me.Page)
                End If
            Else
                Me.objCommon.ShowAlert("Please Select The Project.", Me.Page)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString + " :: Error While Getting Data", ".....btnGo_Click")
            Exit Sub
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        txtsearch.Text = ""
        ' btnExport.Visible = False
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Display", "ButtonexportHide();", True)
        Me.gvwSubjectForProject.DataSource = Nothing
        Me.gvwSubjectForProject.DataBind()
        Me.fldResult.Visible = False
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim fileName As String = String.Empty
        Dim ds As New DataSet
        Try

            If Me.gvwSubjectForProject.Rows.Count < 1 Then
                Me.objCommon.ShowAlert("No Data To Export", Me.Page)
                Exit Sub
            End If
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            fileName = "Project Subject Report"
            fileName = fileName & ".xls"
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            ds.Tables.Add(CType(Me.ViewState(VS_DtParent), DataTable).Copy()) 'is datatable neccessary
            ds.AcceptChanges()

            Context.Response.Write(ConvertDsuserTO(ds))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Private Function ConvertDsuserTO(ByVal ds As DataSet) As String
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer
        Dim dsConvert As New DataSet
        Dim Status As String = String.Empty
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_child As New DataSet
        Dim cStatus As String = String.Empty
        Try
            Status = "Subjects in Project: " + Me.txtsearch.Text.ToString().Trim()
            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""6""><center><strong><font color=""#0a22de"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""6""><center><strong><font color=""#0a22de"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append(Status)
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")

            strMessage.Append("<tr><td align=""center"" colspan=""6"">")
            strMessage.Append("</td></tr>")

            strMessage.Append("<tr >")

            dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "Status,TotalSubject".Split(",")).DefaultView.ToTable())
            dsConvert.AcceptChanges()

            dsConvert.AcceptChanges()

            dsConvert.Tables(0).Columns(1).ColumnName = "Total Subjects"

            For iCol = 0 To dsConvert.Tables(0).Columns.Count - 1

                strMessage.Append("<td align=""center"" colspan=""3""><strong><font color=""#0a22de"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).ToString().Trim())
                strMessage.Append("</font></strong></td>")

            Next
            strMessage.Append("</tr>")

            For j = 0 To dsConvert.Tables(0).Rows.Count - 1

                strMessage.Append("<tr>")

                For i = 0 To dsConvert.Tables(0).Columns.Count - 1
                    strMessage.Append("<td align=""Left"" colspan=""3""><strong><font color=""#000099"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).ToString().Trim())
                    strMessage.Append("</font></strong></td>")
                Next

                cStatus = dsConvert.Tables(0).Rows(j).Item(0).ToString()

                strMessage.Append("</tr>")

                '--------------FOR CHILD GRIDVIEW----------------

                strMessage.Append("<tr><td align=""center"" colspan=""6"">")
                strMessage.Append("</td></tr>")

                wStr = "SELECT vSubjectID,FullName FROM View_SubjectDtlCDMSStatusLog_Dtl "

                If cStatus = "Active" Then
                    cStatus = "AC"
                ElseIf cStatus = "In Active" Then
                    cStatus = "IA"
                ElseIf cStatus = "Hold" Then
                    cStatus = "HO"
                ElseIf cStatus = "Screened" Then
                    cStatus = "SC"
                ElseIf cStatus = "Booked" Then
                    cStatus = "BO"
                ElseIf cStatus = "On Study" Then
                    cStatus = "OS"
                ElseIf cStatus = "Forever Ineligible" Then
                    cStatus = "FI"
                End If

                wStr += "where cStatus='" + cStatus + "' and vWorkSpaceId='" + Me.HdnWorkspaceId.Value.ToString().Trim() + "'"

                If Not objhelpDb.View_GetSubjectDtlCDMSStatusLog(wStr, ds_child, eStr) Then
                    Throw New Exception(eStr)
                End If

                strMessage.Append("<tr>")
                Dim dsConvert_child As New DataSet
                dsConvert_child.Tables.Add(ds_child.Tables(0).DefaultView.ToTable(True, "vSubjectID,FullName".Split(",")).DefaultView.ToTable())
                dsConvert_child.AcceptChanges()

                dsConvert_child.Tables(0).Columns(0).ColumnName = "Subject ID"
                dsConvert_child.Tables(0).Columns(1).ColumnName = "Full Name"

                For iCol = 0 To dsConvert_child.Tables(0).Columns.Count - 1

                    strMessage.Append("<td align=""Center"" style=""background-color:#9DA7F2"" colspan=""3""><strong><font color=""White"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(Convert.ToString(dsConvert_child.Tables(0).Columns(iCol)).ToString().Trim())
                    strMessage.Append("</font></strong></td>")

                Next

                strMessage.Append("</tr>")

                For k = 0 To dsConvert_child.Tables(0).Rows.Count - 1
                    strMessage.Append("<tr>")

                    For i = 0 To dsConvert_child.Tables(0).Columns.Count - 1
                        strMessage.Append("<td align=""Left"" colspan=""3""><font color=""#000000"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                        strMessage.Append(Convert.ToString(dsConvert_child.Tables(0).Rows(k).Item(i)).ToString().Trim())
                        strMessage.Append("</font></td>")
                    Next
                    strMessage.Append("</tr>")
                Next
                strMessage.Append("<tr><td align=""center"" colspan=""6"">")
                strMessage.Append("</td></tr>")


                '-------------------------------------------

            Next
            strMessage.Append("</table>")

            Return strMessage.ToString

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....ConvertDsuserTO")
            Return ""
        End Try
    End Function
#End Region

#Region "FillGrid"

    Private Function FillGrid(ByVal mode As String, ByVal dt_Subject As DataTable) As Boolean

        Dim SerialNo As Integer = 0

        Try

            If (mode = "ParentGrid") Then
                Me.gvwSubjectForProject.DataSource = Nothing
                Me.gvwSubjectForProject.DataBind()
                ' Me.btnExport.Visible = False
                ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Display", "ButtonexportHide();", True)

                If Not dt_Subject Is Nothing Then
                    If dt_Subject.Rows.Count > 0 Then
                        If dt_Subject.Rows.Count > 0 Then

                            dt_Subject.Columns.Add("Sr.No.", GetType(String))
                            dt_Subject.Columns.Add("Status", GetType(String))

                            For Each dr_Row In dt_Subject.Rows
                                If dr_Row("cStatus") = "AC" Then
                                    dr_Row("Status") = "Active"
                                ElseIf dr_Row("cStatus") = "IA" Then
                                    dr_Row("Status") = "In Active"
                                ElseIf dr_Row("cStatus") = "HO" Then
                                    dr_Row("Status") = "Hold"
                                ElseIf dr_Row("cStatus") = "SC" Then
                                    dr_Row("Status") = "Screened"
                                ElseIf dr_Row("cStatus") = "BO" Then
                                    dr_Row("Status") = "Booked"
                                ElseIf dr_Row("cStatus") = "OS" Then
                                    dr_Row("Status") = "On Study"
                                ElseIf dr_Row("cStatus") = "FI" Then
                                    dr_Row("Status") = "Forever Ineligible"
                                Else
                                    dr_Row("cStatus") = "Not Found"
                                End If

                                SerialNo += 1
                                dr_Row("Sr.No.") = SerialNo

                                FOOTERVALUE += Convert.ToInt32(dr_Row("TotalSubject").ToString().Trim())

                            Next

                            '  Me.btnExport.Visible = True
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Display", "Buttonexport();", True)
                            ' UpdatePanel2.Update()

                            Me.gvwSubjectForProject.DataSource = dt_Subject
                            Me.gvwSubjectForProject.DataBind()


                        End If

                    End If

                End If

            ElseIf (mode = "ChildGrid") Then
                If Not dt_Subject Is Nothing Then
                    If dt_Subject.Rows.Count > 0 Then

                        dt_Subject.Columns.Add("Sr.No.", GetType(String))
                        Dim index As Integer = 1
                        For Each dr_Row In dt_Subject.Rows
                            dr_Row("Sr.No.") = index
                            index += 1
                        Next

                        dt_Subject.AcceptChanges()

                    End If

                End If
            End If
            Me.fldResult.Visible = True
            Return True

        Catch ex As Exception
            Me.fldResult.Visible = False
            Me.ShowErrorMessage("Error While Binding Grid", "....FillGrid")
            Return False
        End Try
    End Function

#End Region

#Region "GridView Events"

    Protected Sub gvwSubjectForProject_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwSubjectForProject.RowCommand

        Dim ds As New DataSet()
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim gvChildGrid = CType(Me.gvwSubjectForProject.Rows(e.CommandArgument).FindControl("gvChildGrid"), GridView)
        Try

            If e.CommandName = "Show" Then

                Dim imgbtnExpand = CType(Me.gvwSubjectForProject.Rows(CType(e.CommandArgument, Integer)).FindControl("imgbtnExpand"), ImageButton)

                wStr = "SELECT vSubjectID,FullName,nAge FROM View_SubjectDtlCDMSStatusLog_Dtl where cStatus='" + CType(Me.gvwSubjectForProject.Rows(e.CommandArgument).FindControl("hdncStatus"), HiddenField).Value + "' and vWorkSpaceId='" + Me.HdnWorkspaceId.Value.ToString().Trim() + "'"

                If Not objhelpDb.View_GetSubjectDtlCDMSStatusLog(wStr, ds, eStr) Then
                    Throw New Exception(eStr)
                End If

                If ds Is Nothing Then
                    Exit Sub
                End If

                If ds.Tables(0).Rows.Count > 0 Then
                    If Not FillGrid("ChildGrid", ds.Tables(0)) Then
                        Exit Sub
                    End If
                End If

                gvChildGrid.DataSource = ds.Tables(0)
                gvChildGrid.DataBind()
                imgbtnExpand.Focus() 

                imgbtnExpand.CommandName = "Hide"
                imgbtnExpand.ToolTip = "Hide"
                imgbtnExpand.ImageUrl = "../images/collapse.png"

            ElseIf e.CommandName = "Hide" Then
                Dim imgbtnExpand = CType(Me.gvwSubjectForProject.Rows(CType(e.CommandArgument, Integer)).FindControl("imgbtnExpand"), ImageButton)
                imgbtnExpand.CommandName = "Show"
                imgbtnExpand.ImageUrl = "../images/expand.png"
                imgbtnExpand.ToolTip = "Show"

                CType(Me.gvwSubjectForProject.Rows(e.CommandArgument).FindControl("gvChildGrid"), GridView).DataSource = Nothing
                CType(Me.gvwSubjectForProject.Rows(e.CommandArgument).FindControl("gvChildGrid"), GridView).DataBind()

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString + " :: Error While Getting Data", ".....gvwSubjectForProject.RowCommand")
            Exit Sub
        End Try
    End Sub

    Protected Sub gvwSubjectForProject_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwSubjectForProject.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("imgbtnExpand"), ImageButton).CommandName = "Show" ' Add tool tip
            CType(e.Row.FindControl("imgbtnExpand"), ImageButton).ToolTip = "Show"
            CType(e.Row.FindControl("imgbtnExpand"), ImageButton).CommandArgument = e.Row.RowIndex
        End If
        If e.Row.RowType = DataControlRowType.Footer Then
            CType(e.Row.FindControl("lblFooterTotalSubject"), Label).Text = FOOTERVALUE
            e.Row.Cells(gvwSubjectForProject_cStatus).Text = "Total Subjects"
            e.Row.ForeColor = System.Drawing.Color.Green
            e.Row.Font.Size = 9
            e.Row.Cells(gvwSubjectForProject_cStatus).CssClass = "Color"
        End If

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