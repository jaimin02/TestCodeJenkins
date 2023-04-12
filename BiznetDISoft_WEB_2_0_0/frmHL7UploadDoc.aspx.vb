
Partial Class frmHL7UploadDoc
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Dim objCommon As New clsCommon
    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
    Private rPage As RepoPage
    Dim estr As String = String.Empty
    Dim Wstr As String = String.Empty

    'For Save
    Private Const ProjectNo As Integer = 0
    Private Const Activity As Integer = 1
    Private Const vReferredBy As Integer = 2
    Private Const LabId As Integer = 3
    Private Const SubjectId As Integer = 4
    Private Const SubjectNo As Integer = 5
    Private Const Sex As Integer = 6
    Private Const DOB As Integer = 7
    Private Const TestGroup As Integer = 8
    Private Const Test As Integer = 9
    Private Const Result As Integer = 10
    Private Const UOM As Integer = 11
    Private Const LowRange As Integer = 12
    Private Const HighRange As Integer = 13
    Private Const LowHighUOMRange As Integer = 14
    Private Const Flag As Integer = 15
    Private Const DateOfCollection As Integer = 16
    Private Const UniqueNo As Integer = 17
    Private Const UniqueNO_OBR As Integer = 18
    Private Const Result_Flag As Integer = 19

    'For Retrive
    Private Const ProjectNo_Grid As Integer = 0
    Private Const Activity_Grid As Integer = 1
    Private Const vReferredBy_Grid As Integer = 2
    Private Const vLabId_Grid As Integer = 3
    Private Const DateOfCollection_Grid As Integer = 4
    Private Const SubjectId_Grid As Integer = 5
    Private Const Subject_No As Integer = 6
    Private Const Sex_Grid As Integer = 7
    Private Const DOB_Grid As Integer = 8
    Private Const Age_Grid As Integer = 9
    Private Const TestGroup_Grid As Integer = 10
    Private Const Test_Grid As Integer = 11
    Private Const Result_Grid As Integer = 12
    Private Const Flag_Grid As Integer = 13
    Private Const UOM_Grid As Integer = 14
    Private Const LowRange_Grid As Integer = 15
    Private Const HighRange_Grid As Integer = 16
    Private Const vLowHighUOM As Integer = 17
    Private Const UniqueNo_Grid As Integer = 18
    Private Const UniqueNO_OBR_Grid As Integer = 19
    Private Const Result_Flag_Grid As Integer = 20


    Private Const ProjectNo_HL7_PID_2_3 As Integer = 2
    Private Const LabId_HL7_PID_3 As Integer = 3
    Private Const SubjectId_HL7_PID As Integer = 5
    Private Const SubjectNo_HL7_PID As Integer = 5
    Private Const Sex_HL7_PID As Integer = 8
    Private Const DOB_HL7_PID As Integer = 7

    Private Const Activity_HL7_NTE As Integer = 2
    Private Const vReferredBy_HL7_NTE As Integer = 2

    Private Const UniqueNo_HL7_OBR_4_0 As Integer = 4
    Private Const TestGroup_HL7_OBR_4_3 As Integer = 4
    Private Const DateOfCollection_HL7_OBR As Integer = 7


    Private Const UniqueNoOFOBR_HL7_OBX_3_0_0 As Integer = 3
    Private Const Test_HL7_OBX_3_1 As Integer = 3
    Private Const Result_HL7_OBX As Integer = 5
    Private Const UOM_HL7_OBX As Integer = 6
    Private Const LowRange_HL7_OBX_7_0 As Integer = 7
    Private Const HighRange_HL7_OBX_7_0 As Integer = 7
    Private Const LowHighUOMRange_HL7_OBX_7_1 As Integer = 7
    Private Const Flag_HL7_OBX As Integer = 8

    Private Const Code_Entry_OBX As Integer = 2

#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If Not genCallShowUI() Then
                    Throw New Exception("Error in GenCallShowUI")
                End If
            End If
        Catch Ex As Exception
            Me.ShowErrorMessage(Ex.ToString, "")
        End Try
    End Sub
#End Region

#Region "GenCallShowUI()"
    Private Function genCallShowUI() As Boolean
        Try

            Me.Page.Title = " :: Upload HL7 Documnet :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Upload HL7 Documnet"
            Me.ddlProject.Items.Insert(0, "Select Project")
            Me.ddlActivty.Items.Insert(0, "Select Actvity")
            Me.ddlSubjectList.Items.Insert(0, "Select Subject")
            If Not fillProject() Then
                Throw New Exception("Error while binding project")
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
        ObjCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        ObjCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub

#End Region

#Region "Button Click"
    Protected Sub btnAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAttach.Click
        Dim Extension As String = String.Empty
        Dim DocumentName As String = String.Empty
        Dim cnt As Integer = 1
        Dim line As String = String.Empty
        Dim Reader As StreamReader
        Dim ds_HL7 As New DataSet
        Dim dc As New DataColumn
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer = 0
        Try
            Me.gdvData.DataSource = Nothing
            Me.gdvData.DataBind()
            Me.trData.Style.Add("display", "none")
            Me.btnExport.Style.Add("display", "none")
            Me.ddlActivty.SelectedIndex = 0

            If Me.fuDocument.HasFile Then
                If fuDocument.PostedFile.ContentLength > 0 Then
                    Extension = Path.GetExtension(Me.fuDocument.FileName.ToUpper()).ToString()
                    If Extension = ".HL7" Or Extension = ".BIO" Then
                        DocumentName = "\HL7Document\" + Path.GetFileName(Me.fuDocument.FileName.Trim()).ToString()
FileCheck:
                        If System.IO.File.Exists(Server.MapPath("~" + DocumentName)) Then
                            DocumentName = "\HL7Document\" + Path.GetFileName(Me.fuDocument.PostedFile.FileName).Trim.Split(".")(0).Trim() + "(" + cnt.ToString() + ")" + Path.GetExtension(fuDocument.FileName).ToUpper()
                            DocumentName = DocumentName.ToString.Trim()
                            cnt += 1
                            GoTo FileCheck
                        Else
                            Me.fuDocument.PostedFile.SaveAs(Server.MapPath("~" + DocumentName))
                            Reader = New StreamReader(Server.MapPath("~" + DocumentName))
                            dc.ColumnName = "line"
                            dt.Columns.Add(dc)
                            ds_HL7.Tables.Add(dt)

                            Do While Reader.Peek >= 0
                                line = Reader.ReadLine()
                                dr = ds_HL7.Tables(0).NewRow()
                                dr("line") = line
                                ds_HL7.Tables(0).Rows.Add(dr)
                            Loop
                            ds_HL7.AcceptChanges()



                            If Not DataToSave(ds_HL7) Then
                                Exit Sub
                            End If

                            If Not fillProject() Then
                                Throw New Exception("Error while get projects")
                            End If

                        End If
                    Else
                        objCommon.ShowAlert("file should be .HL7", Me.Page)
                        Return
                    End If
                End If
            Else
                objCommon.ShowAlert("Please select Hl7 file to read data", Me.Page)
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim fileName As String = ""
        Dim isReportComplete As Boolean = False
        Dim style As String = "<style>.text{mso-number-format:\@;}</style>"
        Try
            If gdvData.Rows.Count > 0 Then
                Dim Info As String = String.Empty
                Dim gridViewhtml As String = String.Empty
                fileName = "HL7Decoding-" + Date.Today.ToString("dd-MMM-yyyy") + ".xls"
                isReportComplete = True

                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)

                gdvData.RenderControl(writer)
                gridViewhtml = stringWriter.ToString()
                gridViewhtml = "<table><tr><td align = ""center"" colspan=""2""><b>" + System.Configuration.ConfigurationManager.AppSettings("Client").ToString() + "(Gamma Dynacare Medical Laboratory)<br/></b></td></tr></table><span>Print Date:-  " + Date.Today.ToString("dd-MMM-yyyy") + "</span>" + gridViewhtml
                

                Context.Response.Buffer = True
                Context.Response.ClearContent()
                Context.Response.ClearHeaders()

                Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
                Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)
                Context.Response.AddHeader("Content-Length", gridViewhtml.Length)

                Context.Response.Write(style)
                Context.Response.Write(gridViewhtml)
                Context.Response.Flush()
                Context.Response.End()

                System.IO.File.Delete(fileName)
            Else
                Exit Sub
            End If

        Catch Threadex As Threading.ThreadAbortException
        Catch ex As Exception
            isReportComplete = False
        Finally
            If Not rPage Is Nothing Then
                rPage.CloseReport()
                rPage = Nothing
            End If
        End Try

    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub

    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
        Dim ds As DataSet = Nothing
        Try
            Me.gdvData.DataSource = Nothing
            Me.gdvData.DataBind()
            Me.trData.Style.Add("display", "none")
            Me.btnExport.Style.Add("display", "none")

            If Me.ddlProject.SelectedIndex = 0 Then
                objCommon.ShowAlert("Select Project", Me.Page)
                Exit Sub
            End If

            Wstr = "vProjectNo='" + Me.ddlProject.SelectedValue.ToString.Trim() + "'"
            If Me.ddlActivty.SelectedIndex > 0 Then
                Wstr += " And vActivityName='" + Me.ddlActivty.SelectedValue.ToString.Trim() + "'"
            End If

            If Me.ddlSubjectList.SelectedIndex > 0 Then
                Wstr += " And vSubjectId='" + Me.ddlSubjectList.SelectedValue.ToString.Trim() + "'"
            End If

            If Not objHelp.GetData("VIEW_HL7Data", "*", Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr) Then
                Throw New Exception("Error While get data of HL7...View_Hl7 " + estr.ToString)
            End If

            If ds.Tables(0).Rows.Count > 0 Then
                For Each dr_Row In ds.Tables(0).Rows
                    If dr_Row("cFlag") = "N" Then
                        dr_Row("cFlag") = ""
                    ElseIf dr_Row("cFlag") = "A" Then
                        dr_Row("cFlag") = "Abnormal"
                    ElseIf dr_Row("cFlag") = "L" Then
                        dr_Row("cFlag") = "LO"
                    ElseIf dr_Row("cFlag") = "H" Then
                        dr_Row("cFlag") = "HI"
                    ElseIf dr_Row("cFlag") = "LL" Then
                        dr_Row("cFlag") = "Critically Low"
                    ElseIf dr_Row("cFlag") = "HH" Then
                        dr_Row("cFlag") = "Critically High"
                    End If
                Next
                Me.gdvData.DataSource = ds.Tables(0)
                Me.gdvData.DataBind()
                Me.trData.Style.Add("display", "")
                Me.btnExport.Style.Add("display", "")
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try
    End Sub
#End Region

#Region "HL7 Document DS"
    Private Function DataToSave(ByVal ds_hl7 As DataSet) As Boolean
        Dim dr As DataRow
        Dim dc As DataColumn
        Dim dt As New DataTable
        Dim ds_ForSave As New DataSet
        Dim ds As New DataSet
        Dim BirthDate As New Date
        Dim DateOfCollection_hl7 As New Date
        Dim cnt_NTE As Integer = 0
        Try
            dc = New DataColumn
            dc.ColumnName = "ProjectNo"
            dt.Columns.Add(dc)


            dc = New DataColumn
            dc.ColumnName = "Activity"
            dt.Columns.Add(dc)


            dc = New DataColumn
            dc.ColumnName = "ReferreddBy"
            dt.Columns.Add(dc)



            dc = New DataColumn
            dc.ColumnName = "LabId"
            dt.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "SubjectId"
            dt.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "Subject No"
            dt.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "Sex"
            dt.Columns.Add(dc)


            dc = New DataColumn
            dc.ColumnName = "DOB"
            dt.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "TestGroup"
            dt.Columns.Add(dc)


            dc = New DataColumn
            dc.ColumnName = "Test"
            dt.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "Result"
            dt.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "UOM"
            dt.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "LowRange"
            dt.Columns.Add(dc)


            dc = New DataColumn
            dc.ColumnName = "HighRange"
            dt.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "LowHighRangeUOM"
            dt.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "Flag"
            dt.Columns.Add(dc)


            dc = New DataColumn
            dc.ColumnName = "DateOfCollection"
            dt.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "UniqueNo"
            dt.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "UniqueNO_OBR"
            dt.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "Result Flag"
            dt.Columns.Add(dc)
           
            ds_ForSave.Tables.Add(dt)
            ds_ForSave.Tables(0).AcceptChanges()

            dr = ds_ForSave.Tables(0).NewRow()
            For i As Integer = 0 To ds_hl7.Tables(0).Rows.Count - 1
                If ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(0).ToString.Trim.ToUpper() = "PID" Then

                    If i <> 0 Then
                        ds_ForSave.Tables(0).Rows.Add(dr)
                        ds_ForSave.AcceptChanges()
                        dr = ds_ForSave.Tables(0).NewRow()
                        cnt_NTE = 0
                    End If
                    dr(ProjectNo) = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(ProjectNo_HL7_PID_2_3).Split("^")(3).ToString.Trim()
                    dr(LabId) = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(LabId_HL7_PID_3).ToString.Trim()

                    If ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(SubjectId_HL7_PID).ToString.Trim().Length > 10 Then
                        dr(SubjectId) = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(SubjectId_HL7_PID).ToString.Trim().Substring(0, 10).ToString().Trim()
                    Else
                        dr(SubjectId) = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(SubjectId_HL7_PID).ToString.Trim()
                    End If

                    If ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(SubjectNo_HL7_PID).ToString.Trim().Length > 10 Then
                        dr(SubjectNo) = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(SubjectNo_HL7_PID).ToString.Trim().Substring(ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(SubjectNo_HL7_PID).ToString.Trim().Length - 4, 4).ToString.Trim()
                    Else
                        dr(SubjectNo) = ""
                    End If


                    

                    dr(Sex) = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(Sex_HL7_PID).ToString.Trim()
                    BirthDate = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(DOB_HL7_PID).ToString.Trim().Substring(0, 4) + "-" + ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(DOB_HL7_PID).ToString.Trim().Substring(4, 2) + "-" + ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(DOB_HL7_PID).ToString.Trim().Substring(6, 2)
                    dr(DOB) = BirthDate.ToString("dd-MMM-yyyy")
                    'dr(DOB) = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(DOB_HL7_PID).ToString.Trim()

                ElseIf ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(0).ToString.Trim.ToUpper() = "NTE" Then
                    If cnt_NTE = 0 Then
                        dr(Activity) = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(Activity_HL7_NTE).ToString.Trim().Replace("VISIT:", "").Trim()
                        cnt_NTE += 1
                    ElseIf cnt_NTE = 1 Then
                        dr(vReferredBy) = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(vReferredBy).ToString.Trim().Replace("Copy from Ordering Physician", "").Trim()
                        cnt_NTE += 1
                    End If

                ElseIf ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(0).ToString.Trim.ToUpper() = "OBR" Then
                    If (dr(TestGroup).ToString <> "") Then
                        ds_ForSave.Tables(0).Rows.Add(dr)
                        ds_ForSave.AcceptChanges()
                        dr = ds_ForSave.Tables(0).NewRow()
                        cnt_NTE = 0

                        dr(ProjectNo) = ds_ForSave.Tables(0).Rows(ds_ForSave.Tables(0).Rows.Count - 1).Item(ProjectNo).ToString.Trim()
                        dr(LabId) = ds_ForSave.Tables(0).Rows(ds_ForSave.Tables(0).Rows.Count - 1).Item(LabId).ToString.Trim()
                        dr(SubjectId) = ds_ForSave.Tables(0).Rows(ds_ForSave.Tables(0).Rows.Count - 1).Item(SubjectId).ToString.Trim()
                        dr(SubjectNo) = ds_ForSave.Tables(0).Rows(ds_ForSave.Tables(0).Rows.Count - 1).Item(SubjectNo).ToString.Trim()
                        dr(Sex) = ds_ForSave.Tables(0).Rows(ds_ForSave.Tables(0).Rows.Count - 1).Item(Sex).ToString.Trim()
                        dr(DOB) = ds_ForSave.Tables(0).Rows(ds_ForSave.Tables(0).Rows.Count - 1).Item(DOB).ToString.Trim()
                        dr(Activity) = ds_ForSave.Tables(0).Rows(ds_ForSave.Tables(0).Rows.Count - 1).Item(Activity).ToString.Trim()
                        dr(vReferredBy) = ds_ForSave.Tables(0).Rows(ds_ForSave.Tables(0).Rows.Count - 1).Item(vReferredBy).ToString.Trim()

                    End If
                    dr(TestGroup) = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(TestGroup_HL7_OBR_4_3).Split("^")(3).ToString.Trim()
                    DateOfCollection_hl7 = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(DateOfCollection_HL7_OBR).ToString.Trim().Substring(0, 4) + "-" + ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(DateOfCollection_HL7_OBR).ToString.Trim().Substring(4, 2) + "-" + ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(DateOfCollection_HL7_OBR).ToString.Trim().Substring(6, 2) + " " + ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(DateOfCollection_HL7_OBR).ToString.Trim().Substring(8, 2) + ":" + ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(DateOfCollection_HL7_OBR).ToString.Trim().Substring(10, 2)
                    dr(DateOfCollection) = DateOfCollection_hl7.ToString("dd-MMM-yyy hh:mm:ss")
                    'dr(DateOfCollection) = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(DateOfCollection_HL7_OBR).ToString.Trim()
                    dr(UniqueNo) = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(UniqueNo_HL7_OBR_4_0).Split("^")(0).ToString.Trim()

                ElseIf ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(0).ToString.Trim.ToUpper() = "OBX" Then
                    If (i <> 0) Then
                        If (ds_hl7.Tables(0).Rows(i - 1).Item("line").ToString.Split("|")(UniqueNoOFOBR_HL7_OBX_3_0_0).Split("^")(0).Split("&")(0).ToString.Trim() = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(UniqueNoOFOBR_HL7_OBX_3_0_0).Split("^")(0).Split("&")(0).ToString.Trim()) Then
                            ds_ForSave.Tables(0).Rows.Add(dr)
                            ds_ForSave.AcceptChanges()
                            dr = ds_ForSave.Tables(0).NewRow()
                            cnt_NTE = 0

                            dr(ProjectNo) = ds_ForSave.Tables(0).Rows(ds_ForSave.Tables(0).Rows.Count - 1).Item(ProjectNo).ToString.Trim()
                            dr(LabId) = ds_ForSave.Tables(0).Rows(ds_ForSave.Tables(0).Rows.Count - 1).Item(LabId).ToString.Trim()
                            dr(SubjectId) = ds_ForSave.Tables(0).Rows(ds_ForSave.Tables(0).Rows.Count - 1).Item(SubjectId).ToString.Trim()
                            dr(SubjectNo) = ds_ForSave.Tables(0).Rows(ds_ForSave.Tables(0).Rows.Count - 1).Item(SubjectNo).ToString.Trim()
                            dr(Sex) = ds_ForSave.Tables(0).Rows(ds_ForSave.Tables(0).Rows.Count - 1).Item(Sex).ToString.Trim()
                            dr(DOB) = ds_ForSave.Tables(0).Rows(ds_ForSave.Tables(0).Rows.Count - 1).Item(DOB).ToString.Trim()
                            dr(Activity) = ds_ForSave.Tables(0).Rows(ds_ForSave.Tables(0).Rows.Count - 1).Item(Activity).ToString.Trim()
                            dr(vReferredBy) = ds_ForSave.Tables(0).Rows(ds_ForSave.Tables(0).Rows.Count - 1).Item(vReferredBy).ToString.Trim()


                            dr(TestGroup) = ds_ForSave.Tables(0).Rows(ds_ForSave.Tables(0).Rows.Count - 1).Item(TestGroup).ToString.Trim()  'ds_hl7.Tables(0).Rows(i - 1).Item("line").ToString.Split("|")(TestGroup_HL7_OBR_4_3).Split("^")(3).ToString.Trim()
                            dr(DateOfCollection) = ds_ForSave.Tables(0).Rows(ds_ForSave.Tables(0).Rows.Count - 1).Item(DateOfCollection).ToString.Trim()  'DateOfCollection_hl7.ToString("dd-MMM-yyy hh:mm:ss")
                            dr(UniqueNo) = ds_ForSave.Tables(0).Rows(ds_ForSave.Tables(0).Rows.Count - 1).Item(UniqueNo).ToString.Trim()  'ds_hl7.Tables(0).Rows(i - 1).Item("line").ToString.Split("|")(UniqueNo_HL7_OBR_4_0).Split("^")(0).ToString.Trim()
                        End If
                    End If

                    dr(Result_Flag) = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(Code_Entry_OBX).ToString.Trim.ToUpper()
                    dr(UniqueNO_OBR) = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(UniqueNoOFOBR_HL7_OBX_3_0_0).Split("^")(0).Split("&")(0).ToString.Trim()
                    dr(Test) = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(Test_HL7_OBX_3_1).Split("^")(1).ToString.Trim()

                    If ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(Code_Entry_OBX).ToString.Trim.ToUpper() = "NM" Then
                        dr(Result) = CDbl(ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(Result_HL7_OBX).ToString.Trim())
                    ElseIf ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(Code_Entry_OBX).ToString.Trim.ToUpper() = "FT" Then
                        dr(Result) = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(Result_HL7_OBX).ToString.Trim().Replace("\.br\~", " ").Trim()
                    Else
                        dr(Result) = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(Result_HL7_OBX).ToString.Trim()
                    End If

                    dr(UOM) = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(UOM_HL7_OBX).ToString.Trim()

                    'Starting of LOW Range
                    If ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(LowRange_HL7_OBX_7_0).Split("^")(0).Split("-")(0).ToString.Trim() = "<" Then
                        dr(LowRange) = CDbl(0)
                    ElseIf IsNumeric(ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(LowRange_HL7_OBX_7_0).Split("^")(0).Split("-")(0).ToString.Trim()) Then
                        dr(LowRange) = CDbl(ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(LowRange_HL7_OBX_7_0).Split("^")(0).Split("-")(0).ToString.Trim())
                    Else
                        dr(LowRange) = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(LowRange_HL7_OBX_7_0).Split("^")(0).Split("-")(0).ToString.Trim()
                    End If
                    'End OF LOW Range

                    'Starting OF High Range
                    If ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(HighRange_HL7_OBX_7_0).Split("^")(0).Split("-").Length > 1 Then
                        If ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(HighRange_HL7_OBX_7_0).Split("^")(0).Split("-")(1).ToString.Trim() = "" Then
                            dr(HighRange) = CDbl(0)
                        ElseIf IsNumeric(ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(HighRange_HL7_OBX_7_0).Split("^")(0).Split("-")(1).ToString.Trim()) Then
                            dr(HighRange) = CDbl(ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(HighRange_HL7_OBX_7_0).Split("^")(0).Split("-")(1).ToString.Trim())
                        Else
                            dr(HighRange) = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(HighRange_HL7_OBX_7_0).Split("^")(0).Split("-")(1).ToString.Trim()
                        End If
                    Else
                        dr(HighRange) = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(HighRange_HL7_OBX_7_0).ToString.Trim()
                    End If
                    'End OF High Range

                    If ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(LowHighUOMRange_HL7_OBX_7_1).Split("^").Length > 1 Then
                        dr(LowHighUOMRange) = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(LowHighUOMRange_HL7_OBX_7_1).Split("^")(1).ToString()
                    End If


                    dr(Flag) = ds_hl7.Tables(0).Rows(i).Item("line").ToString.Split("|")(Flag_HL7_OBX).ToString.Trim()

                End If
                ds_ForSave.AcceptChanges()
            Next
            ds_ForSave.Tables(0).Rows.Add(dr)
            ds_ForSave.AcceptChanges()

            ds_ForSave.Tables(0).Rows.RemoveAt(0)
            ds_ForSave.AcceptChanges()

            If Not objHelp.GetData("Hl7Data", "*", "1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr) Then
                Throw New Exception("Error while get data in hl7 table..." + estr.ToString())
            End If

            For i As Integer = 0 To ds_ForSave.Tables(0).Rows.Count - 1
                dr = ds.Tables(0).NewRow()
                'dr.Item(vProjectNo_DB) = ds_ForSave.Tables(0).Rows(i).Item(ProjectNo).ToString.Trim()
                'dr.Item (vActivityName_DB)=
                dr.Item("nHL7DataNo") = 0
                dr.Item("vProjectNo") = ds_ForSave.Tables(0).Rows(i).Item(ProjectNo).ToString.Trim()
                dr.Item("vActivityName") = ds_ForSave.Tables(0).Rows(i).Item(Activity).ToString.Trim()
                dr.Item("vReferredBy") = ds_ForSave.Tables(0).Rows(i).Item(vReferredBy).ToString.Trim()
                dr.Item("vLabId") = ds_ForSave.Tables(0).Rows(i).Item(LabId).ToString.Trim()
                dr.Item("vSubjectId") = ds_ForSave.Tables(0).Rows(i).Item(SubjectId).ToString.Trim()
                dr.Item("vSubjectNo") = ds_ForSave.Tables(0).Rows(i).Item(SubjectNo).ToString.Trim()
                dr.Item("cSex") = ds_ForSave.Tables(0).Rows(i).Item(Sex).ToString.Trim()
                dr.Item("dDateOfBirth") = CDate(ds_ForSave.Tables(0).Rows(i).Item(DOB).ToString.Trim())
                dr.Item("vTestGroup") = ds_ForSave.Tables(0).Rows(i).Item(TestGroup).ToString.Trim()
                dr.Item("vTest") = ds_ForSave.Tables(0).Rows(i).Item(Test).ToString.Trim()
                dr.Item("vResult") = ds_ForSave.Tables(0).Rows(i).Item(Result).ToString.Trim()
                dr.Item("vUOM") = ds_ForSave.Tables(0).Rows(i).Item(UOM).ToString.Trim()
                dr.Item("vLowRange") = ds_ForSave.Tables(0).Rows(i).Item(LowRange).ToString.Trim()
                dr.Item("vHighRange") = ds_ForSave.Tables(0).Rows(i).Item(HighRange).ToString.Trim()
                dr.Item("vLowHighUOM") = ds_ForSave.Tables(0).Rows(i).Item(LowHighUOMRange).ToString.Trim()
                dr.Item("cFlag") = ds_ForSave.Tables(0).Rows(i).Item(Flag).ToString.Trim()
                dr.Item("dDateofCollection") = CDate(ds_ForSave.Tables(0).Rows(i).Item(DateOfCollection).ToString.Trim())
                dr.Item("vUniqueNo") = ds_ForSave.Tables(0).Rows(i).Item(UniqueNo).ToString.Trim()
                dr.Item("vUniqueNo_OBR") = ds_ForSave.Tables(0).Rows(i).Item(UniqueNO_OBR).ToString.Trim()
                dr.Item("vResultFlag") = ds_ForSave.Tables(0).Rows(i).Item(Result_Flag).ToString.Trim()
                dr.Item("iModiFyBy") = Session(S_UserID).ToString

                ds.Tables(0).Rows.Add(dr)
                ds.AcceptChanges()
            Next

            If Not objLambda.Insert_HL7Data(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds, estr) Then
                Throw New Exception("Error while save data in hl7 table..." + estr.ToString())
            End If
            objCommon.ShowAlert("Data Uploaded Successfully In BizNET", Me.Page)
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message.ToString(), "")
            Return False
        End Try
    End Function
#End Region

#Region "Fill Project DropDown"
    Private Function fillProject() As Boolean
        Dim ds As DataSet = Nothing
        Try
            Wstr = "select distinct vProjectNo from HL7Data where cStatusindi<>'D'"
            ds = objHelp.GetResultSet(Wstr, "HL7Data")
            If ds.Tables(0).Rows.Count > 0 Then
                Me.ddlProject.DataSource = ds.Tables(0)
                Me.ddlProject.DataValueField = "vProjectNo"
                Me.ddlProject.DataTextField = "vProjectNo"
                Me.ddlProject.DataBind()
                Me.ddlProject.Items.Insert(0, "Select Project")
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
            Return False
        End Try
    End Function
#End Region

#Region "Fill Activity DropDown"
    Private Function fillActivity() As Boolean
        Dim ds As DataSet = Nothing
        Try
            Me.ddlActivty.Items.Clear()

            If ddlProject.SelectedIndex = 0 Then
                Me.ddlActivty.Items.Insert(0, "Select Actvity")
                Return True
            End If

            Wstr = "select distinct vActivityName from HL7Data where vProjectNo ='" + Me.ddlProject.SelectedValue.ToString.Trim() + "'  and cStatusindi<>'D'"
            ds = objHelp.GetResultSet(Wstr, "HL7Data")
            If ds.Tables(0).Rows.Count > 0 Then
                Me.ddlActivty.DataSource = ds.Tables(0)
                Me.ddlActivty.DataValueField = "vActivityName"
                Me.ddlActivty.DataTextField = "vActivityName"
                Me.ddlActivty.DataBind()
            End If
            Me.ddlActivty.Items.Insert(0, "Select Actvity")
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
            Return False
        End Try
    End Function
#End Region

#Region "Fill Subject DropDown"
    Private Function fillSubjectList() As Boolean
        Dim ds As DataSet = Nothing
        Try
            Me.ddlSubjectList.Items.Clear()

            If ddlProject.SelectedIndex = 0 Then
                Me.ddlActivty.Items.Insert(0, "Select Subject")
                Return True
            End If

            Wstr = "select distinct HL7Data.vSubjectId,SubjectMaster.vFirstName +' '+SubjectMaster.vSurName+' ('+SubjectMaster.vSubjectId+')' as subjectName from HL7Data "
            Wstr += "inner join SubjectMaster "
            Wstr += "on(HL7Data.vSubjectId=SubjectMaster.vSubjectID "
            Wstr += "And SubjectMaster.cStatusIndi <> 'D'"
            Wstr += "And  HL7Data.cStatusindi <>'D')"
            Wstr += "where HL7Data.vProjectNo ='" + Me.ddlProject.SelectedValue.ToString.Trim() + "' "

            If ddlActivty.SelectedIndex > 0 Then
                Wstr += " and HL7Data.vActivityName ='" + Me.ddlActivty.SelectedValue.Trim() + "'"
            End If

            ds = objHelp.GetResultSet(Wstr, "HL7Data")
            If ds.Tables(0).Rows.Count > 0 Then
                Me.ddlSubjectList.DataSource = ds.Tables(0)
                Me.ddlSubjectList.DataValueField = "vSubjectId"
                Me.ddlSubjectList.DataTextField = "vSubjectId"
                Me.ddlSubjectList.DataBind()
            End If
            Me.ddlSubjectList.Items.Insert(0, "Select Subject")
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
            Return False
        End Try
    End Function
#End Region

#Region "Drop Down Events"
    Protected Sub ddlProject_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProject.SelectedIndexChanged
        Try

            Me.gdvData.DataSource = Nothing
            Me.gdvData.DataBind()
            Me.btnExport.Style.Add("display", "none")

            If Not fillActivity() Then
                Throw New Exception("Error while binding activity")
            End If

            If Not fillSubjectList() Then
                Throw New Exception("Error while binding subjectlist")
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try
    End Sub

    Protected Sub ddlActivty_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlActivty.SelectedIndexChanged
        Me.gdvData.DataSource = Nothing
        Me.gdvData.DataBind()
        Me.btnExport.Style.Add("display", "none")

        If Not fillSubjectList() Then
            Throw New Exception("Error while binding subjectlist")
        End If
    End Sub

    Protected Sub ddlSubjectList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSubjectList.SelectedIndexChanged
        Me.gdvData.DataSource = Nothing
        Me.gdvData.DataBind()
        Me.btnExport.Style.Add("display", "none")
    End Sub
#End Region

#Region "Grid Events"
    Protected Sub gdvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvData.RowDataBound
        Dim dDate As New Date
        Try
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(UniqueNo_Grid).Visible = False
                e.Row.Cells(UniqueNO_OBR_Grid).Visible = False
                e.Row.Cells(Result_Flag_Grid).Visible = False
                e.Row.Cells(UOM_Grid).Visible = False
                e.Row.Cells(LowRange_Grid).Visible = False
                e.Row.Cells(HighRange_Grid).Visible = False
                e.Row.Cells(DOB_Grid).Visible = False
                e.Row.Cells(Age_Grid).Visible = False
            End If
            If e.Row.RowType = DataControlRowType.DataRow Then
                'dDate = CDate(e.Row.Cells(DOB_Grid).Text)
                'e.Row.Cells(DOB_Grid).Text = dDate.ToString("dd/MM/yyy")
                dDate = CDate(e.Row.Cells(DateOfCollection_Grid).Text)
                e.Row.Cells(DateOfCollection_Grid).Text = dDate.ToString("dd-MMM-yyy hh:mm:ss")

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try
    End Sub
#End Region
   

End Class
