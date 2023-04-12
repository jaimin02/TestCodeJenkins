Imports Newtonsoft.Json
Partial Class CDMS_frmSubjectCDMSAdvanceSearch
    Inherits System.Web.UI.Page
#Region "Variable Declaration "

    Private objCommon As New clsCommon
    Private objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()


    Private Const VS_SubjectDetail As String = "SubjectDetail"
    Private Const VS_CurrentPage As String = "PageNo"
    Private Const VS_TotalRowCount As String = "TotalRowCount"
    Private Const VS_PagerStartPage As String = "PagerStartPage"

    Private Const GVCSub_SrNo As Integer = 0
    Private Const GVCSub_vSubjectId As Integer = 1
    Private Const GVCSub_vSubjectName As Integer = 2
    Private Const GVCSub_dBirthdate As Integer = 3
    Private Const GVCSub_cSex As Integer = 4
    Private Const GVCSub_vContactNo1 As Integer = 5
    Private Const GVCSub_nHeight As Integer = 6
    Private Const GVCSub_nWeight As Integer = 7
    Private Const GVCSub_nBMI As Integer = 8

    Private Const PAGESIZE As Integer = 50

#End Region

#Region "Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Me.SetPaging()
            If Not IsPostBack Then

                Me.Page.Title = " :: CDMS - Advance Search ::" + System.Configuration.ConfigurationManager.AppSettings("Client")
                CType(Master.FindControl("lblHeading"), Label).Text = "Advance Search"

            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, ".....Page_Load")
        End Try

    End Sub

#End Region

#Region "Fill Grid "

    Private Function FillGrid(ByVal pageNo As String) As Boolean
        Dim estr As String = String.Empty
        Dim dr_Row As DataRow
        Dim Param As String = String.Empty
        Dim ds_SubjectDtl As New DataSet
        Dim ds_AllSubject As New DataSet
        Dim Wstr As String = String.Empty
        Try
            If Me.hdnMedicalValue.Value.ToString() = "" Then
                Param = Me.hdnSearchQuery.Value.Trim() + "##" + pageNo.ToString() + "##" + PAGESIZE.ToString() + "##" + "NONMEDI"

                If Not objhelpDb.Proc_CDMSSubjectDtlAdvanceQuery(Param, ds_SubjectDtl, estr) Then
                    Throw New Exception("Error Retrieving Total Record")
                End If

            Else
                Dim query As String() = Convert.ToString(hdnSearchQuery.Value).Split(New String() {"AND", "OR"}, StringSplitOptions.RemoveEmptyEntries)
                Dim Param1 As String = ""
                Dim para As String = ""
                Dim pa As String()
                For i As Integer = 0 To query.Length - 1
                    If (query(i).Contains("vDescription")) Then
                        pa = query(i).Split("=")
                        If Not (para <> "") Then
                            para += Convert.ToString(pa(1))
                        Else
                            para += "," + Convert.ToString(pa(1))
                        End If
                    Else
                        Param1 += " AND " + query(i)
                    End If


                Next
                Param = Param1 + " AND vDescription IN (" + Convert.ToString(para) + ")" + "##" + pageNo.ToString() + "##" + PAGESIZE.ToString() + "##" + Me.hdnMedicalValue.Value.ToString()

                'Param = Me.hdnSearchQuery.Value.Trim() + "##" + pageNo.ToString() + "##" + PAGESIZE.ToString() + "##" + Me.hdnMedicalValue.Value.ToString()

                If Not objhelpDb.Proc_CDMSSubjectDtlAdvanceQuery(Param, ds_SubjectDtl, estr) Then
                    Throw New Exception("Error Retrieving Total Record")


                End If

            End If

            ds_SubjectDtl.Tables(0).Columns.Add("vSubjectName", System.Type.GetType("System.String"))

            For Each dr_Row In ds_SubjectDtl.Tables(0).Rows
                dr_Row("vSubjectName") = dr_Row("vFirstName") + " " + dr_Row("vMiddleName") + " " + dr_Row("vSurName")
            Next
            If Not ds_SubjectDtl Is Nothing Then
                If ds_SubjectDtl.Tables(0).Rows.Count > 0 Then
                    Me.GrdSubject.DataSource = ds_SubjectDtl.Tables(0)
                    Me.GrdSubject.DataBind()
                    Me.Session(S_DataSet) = Nothing
                    Me.Session(S_DataSet) = ds_SubjectDtl.Copy()
                    Me.btnAdvanceSearchExport.Visible = True
                    Me.btnSend.Style.Add("display", "")
                    Me.fldGrid.Visible = True
                    Return True
                End If
            End If
            objCommon.ShowAlert("No Subjects List Found For This Criteria", Me.Page)
            Me.GrdSubject.DataSource = Nothing
            Me.GrdSubject.DataBind()
            Me.btnAdvanceSearchExport.Visible = False
            Me.btnSend.Style.Add("display", "none")
            Me.fldGrid.Visible = False
            Me.Session(S_DataSet) = Nothing
            Return False

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....Fill Grid")
            Me.Session(S_DataSet) = Nothing
            Return False
        End Try
    End Function

#End Region


#Region "GrdSubject Events "

    Protected Sub GrdSubject_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GrdSubject.DataBound
        Me.SetPaging()
    End Sub


    'Protected Sub GrdSubject_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GrdSubject.PageIndexChanging
    '    GrdSubject.PageIndex = e.NewPageIndex
    '    FillGrid(2, CType(Me.ViewState(VS_SubjectDetail), DataSet).Copy())
    'End Sub


    Protected Sub GrdSubject_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GrdSubject.RowDataBound
        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    e.Row.Cells(GVCSub_SrNo).Text = e.Row.RowIndex + (GrdSubject.PageSize * GrdSubject.PageIndex) + 1
        'End If
    End Sub

#End Region

#Region "Button Events "

    Protected Sub btnAdvanceSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdvanceSearch.Click

        Try
            If Me.ViewState(VS_PagerStartPage) Is Nothing OrElse Me.ViewState(VS_CurrentPage) Is Nothing Then
                Me.ViewState(VS_PagerStartPage) = "1"
                Me.ViewState(VS_CurrentPage) = "1"
            End If

            If Not GetRecordsCounts() Then
                Me.ShowErrorMessage("Error While Record Count", "....GetRecordsCounts")
                Exit Sub
            End If

            If Not FillGrid(1) Then
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage("btnAdvanceSearch_Click", ex.Message)
        End Try
    End Sub

    Protected Sub btnAdvanceSearchExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdvanceSearchExport.Click
        Dim fileName As String = String.Empty
        Dim ds As New DataSet
        Dim ds_AllSubject As New DataSet
        Dim Wstr As String = String.Empty
        Try

            If Me.GrdSubject.Rows.Count < 1 Then
                Me.objCommon.ShowAlert("No Data To Export", Me.Page)
                Exit Sub
            End If
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            fileName = "Advance Search Report"
            fileName = fileName & ".xls"
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            If Me.hdnMedicalValue.Value.ToString() = "" Then
                Wstr = "SELECT * FROM View_CDMSSubjectDetails WHERE "
                Wstr += Me.hdnSearchQuery.Value.ToString.Trim().Substring(3, Me.hdnSearchQuery.Value.ToString.Trim().Length - 3)


                ds_AllSubject = objhelpDb.GetResultSet(Wstr, "View_CDMSSubjectDetails")
            Else
                Wstr = "SELECT * FROM View_CDMSSubjectDetails_Medicalcondition WHERE "
                Dim query As String() = Convert.ToString(hdnSearchQuery.Value).Split(New String() {"AND", "OR"}, StringSplitOptions.RemoveEmptyEntries)
                Dim Param1 As String = ""
                Dim para As String = ""
                Dim pa As String()
                For i As Integer = 0 To query.Length - 1
                    If (query(i).Contains("vDescription")) Then
                        pa = query(i).Split("=")
                        If Not (para <> "") Then
                            para += Convert.ToString(pa(1))
                        Else
                            para += "," + Convert.ToString(pa(1))
                        End If
                    Else
                        If (Param1 <> "") Then
                            Param1 += " AND " + query(i)
                        Else
                            Param1 += query(i)
                        End If

                    End If


                Next
                If (Param1 = "") Then
                    Wstr += "  vDescription IN (" + Convert.ToString(para) + ")"
                Else
                    Wstr += Param1 + " AND vDescription IN (" + Convert.ToString(para) + ")"
                End If

                'Wstr += Me.hdnSearchQuery.Value.ToString.Trim().Substring(3, Me.hdnSearchQuery.Value.ToString.Trim().Length - 3)
                Wstr += "AND vDescription = '" + Me.hdnMedicalValue.Value.ToString.Trim() + "'"

                ds_AllSubject = objhelpDb.GetResultSet(Wstr, "View_CDMSSubjectDetails_Medicalcondition")
            End If

            ds.Tables.Add(ds_AllSubject.Tables(0).Copy())
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
        Dim dr_Row As DataRow
        Try
            'If Me.rblstatus.SelectedValue = 0 Then
            '    Status = "Screening Location : " + Me.ddlLoction.SelectedItem.Text.Trim() + " Master List For Project No : " + Me.txtprojectForsubject.Text.Trim()
            'ElseIf Me.rblstatus.SelectedValue = 1 Then
            '    Status = "Screening Location : " + Me.ddlLoction.SelectedItem.Text.Trim() + "                Screening Date : " + Me.txtdate.Text.Trim()
            'End If

            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""11""><center><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("<tr>")

            ds.Tables(0).Columns.Add("DOB", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add("vSubjectName", System.Type.GetType("System.String"))
            For Each dr_Row In ds.Tables(0).Rows
                dr_Row("DOB") = CDate(dr_Row("dBirthDate")).ToString("dd-MMM-yyyy")
                dr_Row("vSubjectName") = dr_Row("vFirstName") + " " + dr_Row("vMiddleName") + " " + dr_Row("vSurName")
            Next
            ds.Tables(0).AcceptChanges()
            dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vSubjectID,vSubjectName,DOB,cSex,vContactNo1,nHeight,nWeight,nBMI".Split(",")).DefaultView.ToTable())
            dsConvert.AcceptChanges()
            dsConvert.Tables(0).Columns(0).ColumnName = "Subject ID"
            dsConvert.Tables(0).Columns(1).ColumnName = "Subject Name"
            dsConvert.Tables(0).Columns(2).ColumnName = "D.O.B."
            dsConvert.Tables(0).Columns(3).ColumnName = "Gender"
            dsConvert.Tables(0).Columns(4).ColumnName = "Contact No"
            dsConvert.Tables(0).Columns(5).ColumnName = "Height"
            dsConvert.Tables(0).Columns(6).ColumnName = "Weight"
            dsConvert.Tables(0).Columns(7).ColumnName = "BMI"
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

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Me.Response.Redirect("frmSubjectCDMSAdvanceSearch.aspx")
    End Sub


#End Region

#Region "Helper Functions/Subs"

    Private Function GetRecordsCounts() As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim dsCount As DataSet = Nothing
        Dim Param As String = String.Empty
        Try


            If Me.hdnMedicalValue.Value.ToString() = "" Then
                Param = Me.hdnSearchQuery.Value.Trim() + "##" + "NONMEDI"

                If Not objhelpDb.Proc_CDMSSubjectDtlAdvanceQuery_COUNT(Param, dsCount, eStr) Then
                    Throw New Exception("Error Retrieving Total Number of Cases")
                End If

            Else
                Dim query As String() = Convert.ToString(hdnSearchQuery.Value).Split(New String() {"AND", "OR"}, StringSplitOptions.RemoveEmptyEntries)
                Dim queryForOr As String() = Convert.ToString(hdnSearchQuery.Value).Split(New String() {"OR"}, StringSplitOptions.RemoveEmptyEntries)
                Dim Param1 As String = ""
                Dim para As String = ""
                Dim para1 As String = ""
                Dim pa As String()
                Dim pa1 As String()
                Dim param2 As String = ""

                For i As Integer = 0 To query.Length - 1
                    If (query(i).Contains("vDescription")) Then
                        pa = query(i).Split("=")
                        If Not (para <> "") Then
                            para += Convert.ToString(pa(1))
                        Else
                            para += "," + Convert.ToString(pa(1))
                        End If
                    Else
                        Param1 += " AND " + query(i)
                    End If


                Next
                Param = Param1 + " AND vDescription IN (" + Convert.ToString(para) + ")" + "##" + Me.hdnMedicalValue.Value.ToString()

                'Param += +"##" + Me.hdnMedicalValue.Value.ToString()
                ''      Param = Me.hdnSearchQuery.Value.Trim() + "##" + Me.hdnMedicalValue.Value.ToString()

                If Not objhelpDb.Proc_CDMSSubjectDtlAdvanceQuery_COUNT(Param, dsCount, eStr) Then
                    Throw New Exception("Error Retrieving Total Number of Cases")
                End If

            End If

            Me.ViewState(VS_TotalRowCount) = Convert.ToString(dsCount.Tables(0).Rows(0)(0))

            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....GetRecordsCounts")
            Return False
        End Try

    End Function

    Private Sub SetPaging()
        Dim totalPages As Integer = 0
        Dim startIndex As Integer = 1
        Dim count As Integer = 1
        Try

            Me.phTopPager.Controls.Clear()
            Me.phBottomPager.Controls.Clear()

            If Not Me.ViewState(VS_TotalRowCount) Is Nothing AndAlso Integer.Parse(Me.ViewState(VS_TotalRowCount)) > 0 Then
                totalPages = Math.Ceiling(Me.ViewState(VS_TotalRowCount) / PAGESIZE)
            End If
            startIndex = Me.ViewState(VS_PagerStartPage)
            If Me.ViewState(VS_PagerStartPage) Is Nothing Then
                startIndex = 1
                Me.ViewState(VS_PagerStartPage) = 1
            End If

            If totalPages > 1 Then
                For index As Integer = startIndex To totalPages
                    Me.phTopPager.Visible = True
                    Me.phBottomPager.Visible = True
                    Dim lnkButton As New LinkButton()
                    If startIndex > 1 And count = 1 Then
                        'This is for first Page <<
                        lnkButton = Me.AddLnkButton("BtnFirstPage", "BtnFirstPage", 1.ToString, "<<")

                        'This is for ellipse ...
                        lnkButton = Me.AddLnkButton("BtnEllipsePrev", "BtnEllipsePrev", (index - 1).ToString(), "...")
                    End If

                    'This is for Numeric Buttons
                    If index = Me.ViewState(VS_CurrentPage) Then
                        lnkButton = Me.AddLnkButton(index.ToString, "BtnPager", index, index, False)
                    Else
                        lnkButton = Me.AddLnkButton(index.ToString, "BtnPager", index, index)
                    End If

                    If count = 10 Then
                        If index < totalPages Then
                            'This is for ellipses ...
                            Me.AddLnkButton("BtnEllipseNext", "BtnEllipseNext", (index + 1).ToString, "...")

                            'This is for Last Page >>
                            Me.AddLnkButton("BtnLastPage", "BtnLastPage", totalPages.ToString, ">>")
                        End If
                        Exit For
                    End If

                    count = count + 1
                Next
            Else
                Me.phTopPager.Visible = False
                Me.phBottomPager.Visible = False
            End If
            'End If
        Catch ex As Exception

        End Try
    End Sub

    Private Function AddLnkButton(ByVal ID_1 As String, ByVal CommandName_1 As String, _
                                  ByVal CommandArg_1 As String, ByVal Text_1 As String, _
                                  Optional ByVal IsEnablePostBack As Boolean = True) As LinkButton
        Dim lnkButton As New LinkButton
        Dim lnkButtonBottom As New LinkButton
        Dim ltr As Literal
        Dim ltrBottom As Literal
        lnkButton = New LinkButton()
        ltr = New Literal()
        lnkButtonBottom = New LinkButton()
        ltrBottom = New Literal()
        lnkButton.ID = "Top" + ID_1
        lnkButton.CommandName = CommandName_1
        lnkButton.CommandArgument = CommandArg_1
        lnkButton.Text = Text_1
        lnkButton.CssClass = "PagerLinks"
        AddHandler lnkButton.Click, AddressOf PagerButton_Click
        If Not IsEnablePostBack Then
            lnkButton.OnClientClick = "return false;"
            lnkButton.Font.Underline = False
        End If

        lnkButtonBottom.ID = "Bottom" + ID_1
        lnkButtonBottom.CommandName = CommandName_1
        lnkButtonBottom.CommandArgument = CommandArg_1
        lnkButtonBottom.Text = Text_1
        lnkButtonBottom.CssClass = "PagerLinks"
        AddHandler lnkButtonBottom.Click, AddressOf PagerButton_Click
        If Not IsEnablePostBack Then
            lnkButtonBottom.OnClientClick = "return false;"
            lnkButtonBottom.Font.Underline = False
        End If

        Me.phTopPager.Controls.Add(lnkButton)
        Me.phBottomPager.Controls.Add(lnkButtonBottom)
        ltr.Text = "&nbsp;"
        ltrBottom.Text = "&nbsp;"
        Me.phTopPager.Controls.Add(ltr)
        Me.phBottomPager.Controls.Add(ltrBottom)
        Return lnkButton
    End Function

    Protected Sub PagerButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lnkButton As LinkButton
        Dim totalPages As Integer = 1
        totalPages = Me.GetTotalPages()
        lnkButton = CType(sender, LinkButton)
        Me.ViewState(VS_CurrentPage) = lnkButton.CommandArgument

        If lnkButton.CommandName.ToUpper = "BtnEllipseNext".ToUpper.ToString Then
            Me.ViewState(VS_PagerStartPage) = lnkButton.CommandArgument
            If (Integer.Parse(totalPages) - Integer.Parse(lnkButton.CommandArgument)) < 9 Then
                Me.ViewState(VS_PagerStartPage) = (Integer.Parse(totalPages) - 9)
            End If
        ElseIf lnkButton.CommandName.ToUpper = "BtnEllipsePrev".ToUpper.ToString Or _
                lnkButton.CommandName.ToUpper = "BtnLastPage".ToUpper.ToString Then
            Me.ViewState(VS_PagerStartPage) = Integer.Parse(lnkButton.CommandArgument) - 9
            If (Integer.Parse(lnkButton.CommandArgument) - 10) < 1 Then
                Me.ViewState(VS_PagerStartPage) = 1
            End If
        ElseIf lnkButton.CommandName.ToUpper = "BtnFirstPage".ToUpper.ToString Then
            Me.ViewState(VS_PagerStartPage) = 1
        End If

        If Not Me.FillGrid(Me.ViewState(VS_CurrentPage)) Then
            Exit Sub
        End If

    End Sub

    Private Function GetTotalPages() As Integer
        GetTotalPages = 1
        If Not Me.ViewState(VS_TotalRowCount) Is Nothing Then
            GetTotalPages = Me.ViewState(VS_TotalRowCount)
        End If

        If GetTotalPages > PAGESIZE Then
            GetTotalPages = Math.Ceiling(Double.Parse(Me.ViewState(VS_TotalRowCount)) / PAGESIZE)
        End If
    End Function

    Private Sub ClearControls()
        GrdSubject.DataSource = Nothing
        GrdSubject.DataBind()
        Me.ViewState(VS_PagerStartPage) = Nothing
        Me.ViewState(VS_CurrentPage) = Nothing
        Me.ViewState(VS_TotalRowCount) = Nothing
        Me.phBottomPager.Controls.Clear()
        Me.phTopPager.Controls.Clear()
        'Me.HSubject.Value = ""
        'Me.txtSubject.Text = ""
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

#Region "Web Methods"
    <Web.Services.WebMethod()> _
    Public Shared Function GetDescriptionColumn(ByVal SelectText As String) As String
        Dim objCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_Audit As New DataSet
        Dim RowIndex As Integer = 1

        Try

            wStr = "SELECT DISTINCT vDescription FROM SubjectDtlCDMSMedicalCondition WHERE vDescription IS NOT NULL AND vDescription LIKE '%" + SelectText + "%'"
            ds_Audit = objhelpDb.GetResultSet(wStr, "SubjectDtlCDMSMedicalCondition")

            If Not ds_Audit Is Nothing Then
                If ds_Audit.Tables(0).Rows.Count > 0 Then
                    Return JsonConvert.SerializeObject(ds_Audit.Tables(0))
                End If
            End If

        Catch ex As Exception
            Return ex.Message
        End Try

    End Function

#End Region

End Class


