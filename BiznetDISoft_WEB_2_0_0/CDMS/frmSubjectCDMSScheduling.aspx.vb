Imports Newtonsoft.Json
Partial Class CDMS_frmSubjectCDMSScheduling
    Inherits System.Web.UI.Page

#Region "Variable Declartion"

    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()


    Private Const VS_SubjectMaster As String = "SubjectMaster"

    Private Const GVC_VSubjectName As Integer = 0
    Private Const GVC_SubjectAgeGender As Integer = 1
    Private Const GVC_Vsubjectcode As Integer = 2
    Private Const GVC_image As Integer = 3
    Private Const GVC_FirstName As Integer = 4
    Private Const GVC_Surname As Integer = 5
    Private Const GVC_Sex As Integer = 6


#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Me.hdnSessionuserid.Value = Me.Session(S_UserID)
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
            Me.AutoCompleteExtender2.ContextKey = "iUserId = " & Me.Session(S_UserID)
            Me.AutoCompleteExtender3.ContextKey = Session(S_LocationCode)
            Me.AutoCompleteExtender4.ContextKey = "iUserId = " & Me.Session(S_UserID)
            ' Me.AutoCompleteExtender3.ContextKey = Nothing
            GenCall()
        End If
    End Sub

#End Region

#Region "Gencall"

    Private Sub GenCall()
        GenCallData()
        GenCallShowUI()
    End Sub

#End Region

#Region "GencallData"

    Private Sub GenCallData()

        Try
            'If Not FillGrid() Then
            '    Throw New Exception("GenCallData..FillGrid")
            '    Exit Sub
            'End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try


    End Sub

#End Region

#Region "GenCallshowUI"

    Private Sub GenCallShowUI()
        CType(Me.Master.FindControl("lblHeading"), Label).Text = "Screening Scheduler"
        Page.Title = " :: CDMS ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
    End Sub


#End Region

#Region " Fill Grid"

    Private Function FillGrid(ByVal Mode As String, ByVal ds_Subject As DataSet) As Boolean
        Dim wStr As String = String.Empty
        Dim estr As String = String.Empty
        Dim dr_Row As DataRow
        Dim dStartDate As String = String.Empty
        Dim dEndDate As String = String.Empty
        Dim dStartDateNew As New Date
        Dim dEndDateNew As New Date
        Dim years As Array
        Dim days As Integer
        Dim noofdays As Double
        Dim roundoffyear As String
        Dim subjectage As String

        Try
            If Mode = "1" Then
                wStr = "SELECT vSubjectID,vFirstName,vSurName,dBirthDate,cSex,cStatus FROM  View_CDMSSubjectDetails  WHERE  vSubjectID = '" + Me.HSubjectId.Value + "'"

                ds_Subject = objHelp.GetResultSet(wStr, "View_CDMSSubjectDetails")
                For Each dr As DataRow In ds_Subject.Tables(0).Rows
                    dr("cStatus") = "AC"
                Next
            End If

            If Mode = "3" Then
                If ds_Subject.Tables(0).Rows.Count < 0 Then
                    objCommon.ShowAlert("Error While Getting All Subjects List", Me.Page)
                    Exit Function
                End If
                ds_Subject.Tables(0).DefaultView.Sort = "vFirstName"
                Me.GrdSubject.DataSource = ds_Subject.Tables(0).DefaultView.ToTable()
                Me.GrdSubject.DataBind()
                Exit Function
            End If
            If ds_Subject.Tables(0).Rows.Count < 0 Then
                objCommon.ShowAlert("Error While Getting All Subjects List", Me.Page)
                Exit Function
            Else

                ds_Subject.Tables(0).Columns.Add("vSubjectAge", System.Type.GetType("System.String"))
                For Each dr_Row In ds_Subject.Tables(0).Rows
                    dStartDate = DateTime.Now
                    If Not IsDBNull(dr_Row("dBirthDate")) Then
                        dEndDate = dr_Row("dBirthDate")
                        If Not dEndDate = "" Then
                            dStartDateNew = Date.Parse(dStartDate)
                            dEndDateNew = Date.Parse(dEndDate)
                            days = DateDiff(DateInterval.Day, dEndDateNew, dStartDateNew)
                            noofdays = days / 365.25
                            roundoffyear = noofdays
                            years = roundoffyear.Split(".")
                            subjectage = dr_Row("cSex") + "-" + years(0)
                        End If

                    Else
                        dr_Row("vSubjectAge") = dr_Row("cSex")
                    End If
                    If dr_Row("cStatus") = "AC" Then
                        dr_Row("vSubjectAge") = subjectage + " (" + "Active" + ")"
                    ElseIf dr_Row("cStatus") = "IA" Then
                        dr_Row("vSubjectAge") = subjectage + " (" + "In Active" + ")"
                    ElseIf dr_Row("cStatus") = "HO" Then
                        dr_Row("vSubjectAge") = subjectage + " (" + "Hold" + ")"
                    ElseIf dr_Row("cStatus") = "SC" Then
                        dr_Row("vSubjectAge") = subjectage + " (" + "Screened" + ")"
                    ElseIf dr_Row("cStatus") = "BO" Then
                        dr_Row("vSubjectAge") = subjectage + " (" + "Booked" + ")"
                    ElseIf dr_Row("cStatus") = "OS" Then
                        dr_Row("vSubjectAge") = subjectage + " (" + "On Study" + ")"
                    ElseIf dr_Row("cStatus") = "FI" Then
                        dr_Row("vSubjectAge") = subjectage + " (" + "Forever Ineligible" + ")"
                    Else
                        dr_Row("vSubjectAge") = subjectage + " (" + dr_Row("cStatus") + ")"
                    End If
                Next
            End If

            ds_Subject.Tables(0).DefaultView.Sort = "vFirstName"
            Me.ViewState(VS_SubjectMaster) = ds_Subject
            Me.GrdSubject.DataSource = ds_Subject.Tables(0).DefaultView.ToTable()
            Me.GrdSubject.DataBind()
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "GrdSubject", "javascript:createGrdSubject();", True)
            Return True
        Catch ex As Exception
            Return False
        End Try



    End Function

#End Region

#Region " GrdSubject Events"

    'Protected Sub GrdSubject_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GrdSubject.PageIndexChanging
    '    GrdSubject.PageIndex = e.NewPageIndex
    '    FillGrid(3, CType(Me.ViewState(VS_SubjectMaster), DataSet).Copy())
    'End Sub

    Protected Sub GrdSubject_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GrdSubject.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or _
                e.Row.RowType = DataControlRowType.Header Or _
                e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVC_Vsubjectcode).Style.Add("display", "none")
            e.Row.Cells(GVC_FirstName).Style.Add("display", "none")
            e.Row.Cells(GVC_Surname).Style.Add("display", "none")
            e.Row.Cells(GVC_Sex).Style.Add("display", "none")
        End If
    End Sub

    Protected Sub GrdSubject_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)


        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(GVC_Sex).Text.ToString = "M" Then
                CType(e.Row.Cells(GVC_image).FindControl("imgDrag"), Image).ImageUrl = "~/CDMS/images/male.png"
            ElseIf e.Row.Cells(GVC_Sex).Text.ToString = "F" Then
                CType(e.Row.Cells(GVC_image).FindControl("imgDrag"), Image).ImageUrl = "~/CDMS/images/female.png"
            End If

            CType(e.Row.Cells(GVC_image).FindControl("imgDrag"), Image).ID = e.Row.Cells(GVC_Vsubjectcode).Text
            CType(e.Row.Cells(GVC_image).FindControl(e.Row.Cells(GVC_Vsubjectcode).Text.ToString), Image).ToolTip = "Name : " + e.Row.Cells(GVC_FirstName).Text + " " + e.Row.Cells(GVC_Surname).Text + "  SubjectID : " + e.Row.Cells(GVC_Vsubjectcode).Text
            CType(e.Row.Cells(GVC_VSubjectName).FindControl("lblSubject"), Label).Text = e.Row.Cells(GVC_FirstName).Text + " " + e.Row.Cells(GVC_Surname).Text
            CType(e.Row.Cells(GVC_VSubjectName).FindControl("lblSubjectCode"), HyperLink).Text = "(" + e.Row.Cells(GVC_Vsubjectcode).Text + ")"
            CType(e.Row.Cells(GVC_VSubjectName).FindControl("lblSubjectCode"), HyperLink).NavigateUrl = "frmCDMSSubjectInformation.aspx?Mode=2&Subjectid=" + e.Row.Cells(GVC_Vsubjectcode).Text

        End If

    End Sub

#End Region

#Region " Validate Subject"

    Private Function ValidateSubject(ByVal ds_Study As DataSet, ByVal SubjectID As String, ByRef StrMsg As String) As Boolean
        Dim ds_Subject As New DataSet
        Dim wStr As String = String.Empty
        Dim estr As String = String.Empty


        Try

            If ds_Study.Tables(0).Rows.Count > 0 Then
                ''Added by Dipen Shah on 24th March 2015 To resolve Subject Recruitment issue . 
                If (ds_Study.Tables(0).Rows(0)("vAgeMatchTo").ToString.ToUpper() = "END DATE") Then

                    wStr = "SELECT cSex,vRace,vAvailiability,nWeight,nBMI,iMenstrualCycle,cRegularDiet,cRegular,DATEDIFF(year, dBirthdate,'" + ds_Study.Tables(0).Rows(0)("dEndDate") + "') as nAge FROM SubjectDtlCDMS WHERE  vSubjectID = '" + SubjectID + "'"

                ElseIf (ds_Study.Tables(0).Rows(0)("vAgeMatchTo").ToString.ToUpper() = "START DATE") Then
                    wStr = "SELECT cSex,vRace,vAvailiability,nWeight,nBMI,iMenstrualCycle,cRegularDiet,cRegular,DATEDIFF(year, dBirthdate,'" + ds_Study.Tables(0).Rows(0)("dStartdate") + "') as nAge FROM SubjectDtlCDMS WHERE  vSubjectID = '" + SubjectID + "'"
                End If
            Else
                Throw New Exception
            End If
            ''===========================================================
            ds_Subject = objHelp.GetResultSet(wStr, "SubjectDtlCDMS")

            If ds_Study.Tables(0).Rows(0)("vSex").ToString() <> "B" Then
                If ds_Study.Tables(0).Rows(0)("vSex").ToString() <> ds_Subject.Tables(0).Rows(0)("cSex").ToString() Then
                    StrMsg += "Gender Doesn't match for this study."
                End If
            End If
            If ds_Study.Tables(0).Rows(0)("vRace").ToString() <> "Any Race" Then
                If ds_Study.Tables(0).Rows(0)("vRace").ToString() <> ds_Subject.Tables(0).Rows(0)("vRace").ToString() Then
                    StrMsg += "Race Doesn't match for this study."
                End If
            End If
            If ds_Subject.Tables(0).Rows(0)("cSex").ToString() = "F" Then

                If Not (IIf(ds_Study.Tables(0).Rows(0)("nFeMaleWeightMin").ToString() = "", 0, ds_Study.Tables(0).Rows(0)("nFeMaleWeightMin")) < IIf(ds_Subject.Tables(0).Rows(0)("nWeight").ToString() = "", 0, ds_Subject.Tables(0).Rows(0)("nWeight")) AndAlso IIf(ds_Subject.Tables(0).Rows(0)("nWeight").ToString() = "", 0, ds_Subject.Tables(0).Rows(0)("nWeight")) < IIf(ds_Study.Tables(0).Rows(0)("nFeMaleWeightMax").ToString() = "", 0, ds_Study.Tables(0).Rows(0)("nFeMaleWeightMax"))) Then
                    StrMsg += "Weight is not suitable for this study."
                End If
                If Not (IIf(ds_Study.Tables(0).Rows(0)("iMenstrualCycleMin").ToString() = "", 0, ds_Study.Tables(0).Rows(0)("iMenstrualCycleMin")) <= IIf(ds_Subject.Tables(0).Rows(0)("iMenstrualCycle").ToString() = "", 0, ds_Subject.Tables(0).Rows(0)("iMenstrualCycle")) AndAlso IIf(ds_Subject.Tables(0).Rows(0)("iMenstrualCycle").ToString() = "", 0, ds_Subject.Tables(0).Rows(0)("iMenstrualCycle")) <= IIf(ds_Study.Tables(0).Rows(0)("iMenstrualCycleMax").ToString() = "", 120, ds_Study.Tables(0).Rows(0)("iMenstrualCycleMax"))) Then
                    StrMsg += "Menstrual Cycle is not suitable for this study."
                End If
                If ds_Study.Tables(0).Rows(0)("vRegular").ToString() <> "N/A" Then
                    If ds_Study.Tables(0).Rows(0)("vRegular").ToString() <> ds_Subject.Tables(0).Rows(0)("cRegular").ToString() Then
                        StrMsg += "Regular Criteria Doesn't match for this study."
                    End If
                End If
            ElseIf ds_Subject.Tables(0).Rows(0)("cSex") = "M" Then
                If Not (IIf(ds_Study.Tables(0).Rows(0)("nMaleWeightMin").ToString() = "", 0, ds_Study.Tables(0).Rows(0)("nMaleWeightMin")) < IIf(ds_Subject.Tables(0).Rows(0)("nWeight").ToString() = "", 0, ds_Subject.Tables(0).Rows(0)("nWeight")) AndAlso IIf(ds_Subject.Tables(0).Rows(0)("nWeight").ToString() = "", 0, ds_Subject.Tables(0).Rows(0)("nWeight")) < IIf(ds_Study.Tables(0).Rows(0)("nMaleWeightMax").ToString() = "", 0, ds_Study.Tables(0).Rows(0)("nMaleWeightMax"))) Then
                    StrMsg += "Weight is not suitable for this study."
                End If
            End If
            If Not (IIf(ds_Study.Tables(0).Rows(0)("nBMIMin").ToString() = "", 0, ds_Study.Tables(0).Rows(0)("nBMIMin")) < IIf(ds_Subject.Tables(0).Rows(0)("nBMI").ToString() = "", 0, ds_Subject.Tables(0).Rows(0)("nBMI")) AndAlso IIf(ds_Subject.Tables(0).Rows(0)("nBMI").ToString() = "", 0, ds_Subject.Tables(0).Rows(0)("nBMI")) < IIf(ds_Study.Tables(0).Rows(0)("nBMIMax").ToString() = "", 0, ds_Study.Tables(0).Rows(0)("nBMIMax"))) Then
                StrMsg += "BMI is not suitable for this study."
            End If

            If Not (IIf(ds_Study.Tables(0).Rows(0)("nAgeMin").ToString() = "", 0, ds_Study.Tables(0).Rows(0)("nAgeMin")) <= IIf(ds_Subject.Tables(0).Rows(0)("nAge").ToString() = "", 0, ds_Subject.Tables(0).Rows(0)("nAge")) AndAlso IIf(ds_Subject.Tables(0).Rows(0)("nAge").ToString() = "", 0, ds_Subject.Tables(0).Rows(0)("nAge")) <= IIf(ds_Study.Tables(0).Rows(0)("nAgeMax").ToString() = "", 0, ds_Study.Tables(0).Rows(0)("nAgeMax"))) Then
                StrMsg += "Age is not suitable for this study."
            End If

            If ds_Study.Tables(0).Rows(0)("vRegularDiet").ToString() <> "N/A" Then
                If ds_Study.Tables(0).Rows(0)("vRegularDiet").ToString() <> ds_Subject.Tables(0).Rows(0)("cRegularDiet").ToString() Then
                    StrMsg += "RegularDiet Criteria Doesn't match for this study."
                End If
            End If



            If StrMsg = "" Then
                StrMsg = "It doesn't satisfy the criteria of the study"
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....ValidateSubject")
            Return False
        End Try
    End Function

#End Region

#Region "Button Events"

    'Protected Sub btnAdvanceSearchExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdvanceSearchExport.Click
    '    Dim ds_Field As New DataSet
    '    Dim JSONString As String = String.Empty
    '    Dim fileName As String = String.Empty
    '    Dim dsuserhistory As New DataSet
    '    Try
    '        JSONString = Me.hdnAdvanceSearchExport.Value
    '        ds_Field.Tables.Add(JsonConvert.DeserializeObject(JSONString, GetType(System.Data.DataTable)))
    '        Context.Response.Buffer = True
    '        Context.Response.ClearContent()
    '        Context.Response.ClearHeaders()

    '        Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
    '        Context.Response.AddHeader("Content-Disposition", "attachment; filename=Searched Subject.xls")

    '        Context.Response.Write(ConvertDsuserTO(ds_Field))

    '        Context.Response.Flush()
    '        Context.Response.Close()
    '        'Context.Response.End()

    '        System.IO.File.Delete(fileName)

    '    Catch ex As Exception

    '    End Try
    'End Sub

    Private Function ConvertDsuserTO(ByVal ds As DataSet) As String
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer
        Dim dsConvert As New DataSet

        Try


            For Each dr_Row In ds.Tables(0).Rows
                If dr_Row("dBirthDate") <> "" Then
                    dr_Row("dBirthDate") = CDate(dr_Row("dBirthDate")).ToString("dd-MMM-yyyy")
                End If
            Next

            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""9""><center><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")


            strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")

            dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vSubjectID,vFirstName,vSurName,dBirthDate,cSex,vContactNo1,nHeight,nWeight,nBMI".Split(",")).DefaultView.ToTable())
            dsConvert.AcceptChanges()
            dsConvert.Tables(0).Columns(0).ColumnName = "Subject ID"
            dsConvert.Tables(0).Columns(1).ColumnName = "First Name"
            dsConvert.Tables(0).Columns(2).ColumnName = "Sur Name"
            dsConvert.Tables(0).Columns(3).ColumnName = "Date Of Birth"
            dsConvert.Tables(0).Columns(4).ColumnName = "Gender"
            dsConvert.Tables(0).Columns(5).ColumnName = "Contact No"
            dsConvert.Tables(0).Columns(6).ColumnName = "Height"
            dsConvert.Tables(0).Columns(7).ColumnName = "Weight"
            dsConvert.Tables(0).Columns(8).ColumnName = "Bmi"
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

    Protected Sub btnMatch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMatch.Click

        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_StudyDtl As DataSet = Nothing
        Dim ds_MatchSubject As New DataSet
        Dim sMatchQuery As String = String.Empty
        Dim sParameter As String = String.Empty
        Dim dr As DataRow
        Dim subjectage As String = String.Empty
        Dim StrMsg As String = String.Empty
        Try

            If Me.hdnProjectForsubject.Value.Trim() <> "" Then
                ds_MatchSubject = Nothing
                wStr = "vWorkSpaceId='" + Me.hdnProjectForsubject.Value.Trim() + "' And cStatusIndi <> 'D'"

                If Not objHelp.getStudyDtlCDMS(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_StudyDtl, eStr) Then
                    Throw New Exception(eStr)
                End If

                If Not ds_StudyDtl Is Nothing Then
                    If ds_StudyDtl.Tables(0).Rows.Count > 0 Then
                        dr = ds_StudyDtl.Tables(0).Rows(0)
                        For Each dc As DataColumn In ds_StudyDtl.Tables(0).Columns
                            If dc.ColumnName = "vSex" Then
                                sMatchQuery += IIf(dr(dc.ColumnName).ToString = "B", " ", " SubjectDtlCDMS.cSex = '" + dr(dc.ColumnName).ToString() + "' And")
                            ElseIf dc.ColumnName = "vRace" Then
                                sMatchQuery += IIf(dr(dc.ColumnName).ToString.Contains("Any Race"), " ", " SubjectDtlCDMS.vRace In (select data from dbo.SplitString('" + dr(dc.ColumnName).ToString() + "',',')) And ")
                            ElseIf dc.ColumnName = "vAvailiability" Then
                                sMatchQuery += IIf(dr(dc.ColumnName).ToString.Contains("Any Availability"), " ", " SubjectDtlCDMS.vAvailiability = '" + dr(dc.ColumnName).ToString() + "' And")
                            ElseIf dc.ColumnName = "nMaleWeightMin" Then
                                sMatchQuery += " (SubjectDtlCDMS.cSex = 'M' And CAST(ISNULL(SubjectDtlCDMS.nWeight,0) AS NUMERIC(18,2)) between CAST( '" + IIf(String.IsNullOrEmpty(dr("nMaleWeightMin").ToString), "0", dr("nMaleWeightMin").ToString) + "' As Numeric(18,2)) And  CAST('" + IIf(String.IsNullOrEmpty(dr("nMaleWeightMax").ToString), "0", dr("nMaleWeightMax").ToString) + "' AS NUMERIC(18,2)) Or (SubjectDtlCDMS.cSex = 'F' And CAST(ISNULL(SubjectDtlCDMS.nWeight,0) AS NUMERIC(18,2)) between CAST('" + IIf(String.IsNullOrEmpty(dr("nFeMaleWeightMin").ToString), "0", dr("nFeMaleWeightMin").ToString) + "' AS NUMERIC(18,2)) And CAST('" + IIf(String.IsNullOrEmpty(dr("nFeMaleWeightMax").ToString), "0", dr("nFeMaleWeightMax").ToString) + "' AS NUMERIC(18,2))"
                                sMatchQuery += IIf(dr("vRegular").ToString = "N/A", " ", " AND SubjectDtlCDMS.cRegular='" + dr("vRegular").ToString + "'") + ")) AND "
                            ElseIf dc.ColumnName = "nBMIMin" Then
                                sMatchQuery += " ISNULL(SubjectDtlCDMS.nBMI,0) between CAST(" + dr("nBMIMin").ToString + " As Numeric(18,2))And CAST(" + IIf(dr("nBMIMax").ToString = "0.00", "300", dr("nBMIMax").ToString) + " As Numeric(18,2))And "
                            ElseIf dc.ColumnName = "vRegularDiet" Then
                                sMatchQuery += IIf(dr(dc.ColumnName).ToString = "N/A", " ", " SubjectDtlCDMS.cRegularDiet='" + dr(dc.ColumnName).ToString + "' And ")
                            ElseIf dc.ColumnName = "vSwallowPil" Then
                                sMatchQuery += IIf(dr(dc.ColumnName).ToString = "N/A", " ", " SubjectDtlCDMS.cSwallowPil='" + dr(dc.ColumnName).ToString + "' And ")
                            ElseIf dc.ColumnName = "iMenstrualCycleMin" And dr("vSex").ToString <> "M" Then
                                sMatchQuery += " ISNULL(SubjectDtlCDMS.iMenstrualCycle,0) between " + IIf(String.IsNullOrEmpty(dr("iMenstrualCycleMin").ToString), "0", dr("iMenstrualCycleMin").ToString) + " And " + IIf(IIf(IsDBNull(dr("iMenstrualCycleMax")), "0", dr("iMenstrualCycleMax").ToString()) = "0", "120", dr("iMenstrualCycleMax").ToString) + " And "
                                'ElseIf dc.ColumnName = "vRegular" Then
                                '    sMatchQuery += IIf(dr(dc.ColumnName).ToString = "N/A", " ", " SubjectDtlCDMS.cRegular='" + dr(dc.ColumnName).ToString + "' And ")
                            ElseIf dc.ColumnName = "vAgeMatchTo" And dr("vAgeMatchTo").ToString = "Start Date" And dr("dStartDate").ToString <> "" Then
                                If IsDate(dr("dStartDate")) Then
                                    sMatchQuery += "CONVERT(NUMERIC(18,0),DATEDIFF(DAY,SubjectDtlCDMS.dBirthdate,'" + DateTime.Parse(dr("dStartDate")).ToString("yyyy-MM-dd") + "')/365.25) >= " + dr("nAgeMin").ToString + " And CONVERT(NUMERIC(18,0),DATEDIFF(DAY,SubjectDtlCDMS.dBirthdate,'" + DateTime.Parse(dr("dStartDate")).ToString("yyyy-MM-dd") + "')/365.25) <=" + dr("nAgeMax").ToString + " And "
                                End If
                            ElseIf dc.ColumnName = "vAgeMatchTo" And dr("vAgeMatchTo").ToString = "End Date" And dr("dEndDate").ToString <> "" Then
                                If IsDate(dr("dEndDate")) Then
                                    sMatchQuery += "CONVERT(NUMERIC(18,0),DATEDIFF(DAY,SubjectDtlCDMS.dBirthdate,'" + DateTime.Parse(dr("dEndDate")).ToString("yyyy-MM-dd") + "')/365.25) >= " + dr("nAgeMin").ToString + " And CONVERT(NUMERIC(18,0),DATEDIFF(DAY,SubjectDtlCDMS.dBirthdate,'" + DateTime.Parse(dr("dEndDate")).ToString("yyyy-MM-dd") + "')/365.25) <=" + dr("nAgeMax").ToString + " And "
                                End If
                            ElseIf dc.ColumnName = "vAgeMatchTo" And dr("vAgeMatchTo").ToString = "Start Date-End Date" And dr("dStartDate").ToString <> "" And dr("dEndDate").ToString <> "" Then
                                If IsDate(dr("dStartDate")) AndAlso IsDate(dr("dEndDate")) Then
                                    sMatchQuery += "CONVERT(NUMERIC(18,0),DATEDIFF(DAY,SubjectDtlCDMS.dBirthdate,'" + DateTime.Parse(dr("dStartDate")).ToString("yyyy-MM-dd") + "')/365.25) >= " + dr("nAgeMin").ToString + " And CONVERT(NUMERIC(18,0),DATEDIFF(DAY,SubjectDtlCDMS.dBirthdate,'" + DateTime.Parse(dr("dEndDate")).ToString("yyyy-MM-dd") + "')/365.25) <= " + dr("nAgeMax").ToString + " And "
                                End If
                            End If
                        Next

                        sMatchQuery = sMatchQuery.Substring(0, sMatchQuery.LastIndexOf("And"))
                        If Me.HSubjectId.Value.Trim() <> "" Then
                            sParameter = Me.hdnProjectForsubject.Value.Trim() + "##" + sMatchQuery + "##" + Me.HSubjectId.Value.Trim()
                        Else
                            sParameter = Me.hdnProjectForsubject.Value.Trim() + "##" + sMatchQuery + "##" + "-1"
                        End If


                        If Not objHelp.Proc_GetMathcedSubjects(sParameter, ds_MatchSubject, eStr) Then
                            Throw New Exception(eStr)
                        End If


                        If Not ds_MatchSubject Is Nothing Then
                            If Not ds_MatchSubject.Tables.Count = 0 Then
                                If ds_MatchSubject.Tables(0).Rows.Count > 0 Then
                                    ds_MatchSubject.Tables(0).Columns.Add("vSubjectAge", System.Type.GetType("System.String"))
                                    For Each dr_Row In ds_MatchSubject.Tables(0).Rows
                                        subjectage = dr_Row("cSex") + "-" + (DateDiff(DateInterval.Day, Date.Parse(dr_Row("dBirthDate")), Date.Parse(DateTime.Now)) / 365.25).ToString.Split(".")(0)
                                        If dr_Row("cStatus") = "AC" Then
                                            dr_Row("vSubjectAge") = subjectage + " (" + "Active" + ")"
                                        ElseIf dr_Row("cStatus") = "IA" Then
                                            dr_Row("vSubjectAge") = subjectage + " (" + "Inactive" + ")"
                                        ElseIf dr_Row("cStatus") = "HO" Then
                                            dr_Row("vSubjectAge") = subjectage + " (" + "Active" + ")"
                                        ElseIf dr_Row("cStatus") = "SC" Then
                                            dr_Row("vSubjectAge") = subjectage + " (" + "Screened" + ")"
                                        ElseIf dr_Row("cStatus") = "BO" Then
                                            dr_Row("vSubjectAge") = subjectage + " (" + "Booked" + ")"
                                        ElseIf dr_Row("cStatus") = "OS" Then
                                            dr_Row("vSubjectAge") = subjectage + " (" + "On Study" + ")"
                                        ElseIf dr_Row("cStatus") = "FI" Then
                                            dr_Row("vSubjectAge") = subjectage + " (" + "Forever Ineligible" + ")"
                                        Else
                                            dr_Row("vSubjectAge") = subjectage + " (" + dr_Row("cStatus") + ")"
                                        End If
                                    Next
                                    Me.ViewState(VS_SubjectMaster) = ds_MatchSubject.Copy
                                    ds_MatchSubject.Tables(0).DefaultView.Sort = "vFirstName"
                                    GrdSubject.DataSource = ds_MatchSubject.Tables(0).DefaultView.ToTable()
                                    GrdSubject.DataBind()
                                Else
                                    If Me.HSubjectId.Value.Trim() <> "" Then
                                        If Not ValidateSubject(ds_StudyDtl, Me.HSubjectId.Value.ToString(), StrMsg) Then
                                            Exit Sub
                                        End If
                                        objCommon.ShowAlert(StrMsg, Me.Page)
                                    Else
                                        objCommon.ShowAlert("No Subject Found For This Project Criteria.", Me.Page)
                                    End If
                                    Me.ViewState(VS_SubjectMaster) = Nothing
                                    GrdSubject.DataSource = Nothing
                                    GrdSubject.DataBind()
                                End If
                            Else
                                If Me.HSubjectId.Value.Trim() <> "" Then
                                    If Not ValidateSubject(ds_StudyDtl, Me.HSubjectId.Value.ToString(), StrMsg) Then
                                        Exit Sub
                                    End If
                                    objCommon.ShowAlert(StrMsg, Me.Page)
                                Else
                                    objCommon.ShowAlert("No Subject Found For This Project Criteria.", Me.Page)
                                End If
                                Me.ViewState(VS_SubjectMaster) = Nothing
                                GrdSubject.DataSource = Nothing
                                GrdSubject.DataBind()
                            End If
                        Else
                            If Me.HSubjectId.Value.Trim() <> "" Then
                                objCommon.ShowAlert("Subject Is Not Elgible For This Study ", Me.Page)
                            Else
                                objCommon.ShowAlert("No Subject Found For This Project Criteria.", Me.Page)
                            End If
                            Me.ViewState(VS_SubjectMaster) = Nothing
                            GrdSubject.DataSource = Nothing
                            GrdSubject.DataBind()
                        End If
                    Else
                        objCommon.ShowAlert("Please enter study information for this study.", Me.Page)
                        Me.ViewState(VS_SubjectMaster) = Nothing
                        GrdSubject.DataSource = Nothing
                        GrdSubject.DataBind()
                    End If
                Else
                    objCommon.ShowAlert("Please enter study information for this study.", Me.Page)
                    Me.ViewState(VS_SubjectMaster) = Nothing
                    GrdSubject.DataSource = Nothing
                    GrdSubject.DataBind()
                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "btnMatch...()")
        End Try

    End Sub

    Protected Sub btnSearchSubject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchSubject.Click
        Dim ds_Subject As New DataSet
        If Me.hdnProjectForsubject.Value.Trim = "" Then
            If Not Me.HSubjectId.Value = "" Then
                If Not FillGrid(1, ds_Subject) Then
                    Throw New Exception("btnSearchSubject_Click..FillGrid")
                    Exit Sub
                End If
            End If
        Else
            Me.GrdSubject.DataSource = Nothing
            Me.GrdSubject.DataBind()
        End If

    End Sub

    Protected Sub btnAddToGrid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddToGrid.Click
        Dim ds_Field As New DataSet
        Try
            ds_Field = CType(Me.Session(S_DataSet), DataSet)
            Me.Session(S_DataSet) = Nothing
            If Not FillGrid(2, ds_Field) Then
                Throw New Exception("btnAddToGrid_Click..FillGrid")
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage("Error While BInding", "")
        End Try

    End Sub

    'Protected Sub btnAddToGrid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddToGrid.Click
    '    Dim ds_Field As New DataSet
    '    Dim JSONString As String = String.Empty
    '    Dim fileName As String = String.Empty
    '    Dim dsuserhistory As New DataSet
    '    Dim dvFilter As DataView

    '    Try
    '        JSONString = Me.hdnAdvanceSearchExport.Value
    '        ds_Field.Tables.Add(JsonConvert.DeserializeObject(JSONString, GetType(System.Data.DataTable)))

    '        dvFilter = ds_Field.Tables(0).DefaultView
    '        dvFilter.RowFilter = "cStatus = 'AC' Or cStatus = 'HO' and '" + Date.Now + "' > dEndDate"

    '        ds_Field = New DataSet
    '        ds_Field.Tables.Add(dvFilter.ToTable().Copy())

    '        For Each dr As DataRow In ds_Field.Tables(0).Rows
    '            dr("cStatus") = "AC"
    '        Next

    '        If Not FillGrid(2, ds_Field) Then
    '            Throw New Exception("btnAddToGrid_Click..FillGrid")
    '            Exit Sub
    '        End If
    '        Me.txtSearchSubject.Text = ""
    '        Me.HSubjectId.Value = ""
    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.Message, "")
    '    End Try
    'End Sub

#End Region

#Region "Web Methods"

    <Web.Services.WebMethod()> _
    Public Shared Function UpdateFieldValues(ByVal SubjectId As String, ByVal ColumnName As String, _
                                             ByVal TableName As String, ByVal ChangedValue As String, _
                                             ByVal Remarks As String, ByVal StartDate As String, ByVal EndDate As String) As Boolean

        Dim objCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

        Dim eStr As String = String.Empty

        Dim ds_Consumption As New DataSet
        Dim ds_Field As New DataSet
        Dim dt_Field As New DataTable
        Dim dr_Field As DataRow

        Try


            dt_Field.Columns.Add("vSubjectID", Type.GetType("System.String"))
            dt_Field.Columns.Add("vTableName", Type.GetType("System.String"))
            dt_Field.Columns.Add("vColumnName", Type.GetType("System.String"))
            dt_Field.Columns.Add("vChangedValue", Type.GetType("System.String"))
            dt_Field.Columns.Add("vRemarks", Type.GetType("System.String"))
            dt_Field.Columns.Add("iModifyBy", Type.GetType("System.Int32"))


            dr_Field = dt_Field.NewRow
            dr_Field("vSubjectID") = SubjectId
            dr_Field("vTableName") = TableName
            dr_Field("vColumnName") = ColumnName
            dr_Field("vChangedValue") = ChangedValue
            dr_Field("vRemarks") = Remarks
            dr_Field("iModifyBy") = HttpContext.Current.Session(S_UserID)
            dt_Field.Rows.Add(dr_Field)

            If StartDate <> "" Then
                dr_Field = dt_Field.NewRow
                dr_Field("vSubjectID") = SubjectId
                dr_Field("vTableName") = TableName
                dr_Field("vColumnName") = "dStartDate"
                dr_Field("vChangedValue") = StartDate
                dr_Field("vRemarks") = ""
                dr_Field("iModifyBy") = HttpContext.Current.Session(S_UserID)
                dt_Field.Rows.Add(dr_Field)
            End If

            If EndDate <> "" Then
                dr_Field = dt_Field.NewRow
                dr_Field("vSubjectID") = SubjectId
                dr_Field("vTableName") = TableName
                dr_Field("vColumnName") = "dEndDate"
                dr_Field("vChangedValue") = EndDate
                dr_Field("vRemarks") = ""
                dr_Field("iModifyBy") = HttpContext.Current.Session(S_UserID)
                dt_Field.Rows.Add(dr_Field)
            End If


            ds_Field.Tables.Add(dt_Field)

            If Not objLambda.Insert_UpdateSubjectCDMSDtlField(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Field, _
                                                 HttpContext.Current.Session(S_UserID), eStr) Then
                Return False
                Exit Function
            End If

            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

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
