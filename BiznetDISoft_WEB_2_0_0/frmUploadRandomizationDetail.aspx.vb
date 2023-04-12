Imports System.Web.Services
Imports Newtonsoft.Json

Partial Class frmUploadRandomizationDetail
    Inherits System.Web.UI.Page


#Region "Variable Declaration"
    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()
    Private Const Vs_AllowRemarks As String = "AllowRemarks"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_producttype As Integer = 4
    Private Const GVC_radcode As Integer = 6

    Private Const GVRH_SrNo As Integer = 0
    Private Const GVRH_ProjectNo As Integer = 1
    Private Const GVRH_Range As Integer = 2
    Private Const GVRH_UploadedBy As Integer = 3
    Private Const GVRH_UploadedOn As Integer = 4
    Private Const GVRH_Remarks As Integer = 5
    Private Const GVRH_imgView As Integer = 6
    Private Const GVRH_FileNo As Integer = 7

    Private Const CSV_A As String = " Test Product"
    Private Const CSV_B As String = "Reference Product"
    Private Const VS_dsRandomization As String = "ds_Randomization"
    Private ColSelection As String = String.Empty

#End Region

#Region "Form Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Page.Title = ":: Upload Randomization Details :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Upload Randomization"

            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
            End If

            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideSponsorDetails", "HideSponsorDetails(); ", True)
            End If
            'for filtering the Projects according to user  07-09-2011
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
            '======
            bindGrid()    ''Added by ketan
        End If

    End Sub
#End Region

#Region "csvToDAtatable and FormatDatatable"

    Public Function csvToDataTable(ByVal file As String, ByVal isRowOneHeader As Boolean) As DataTable
        Dim csvDataTable = New DataTable()
        Dim csvData() As String = IO.File.ReadAllLines(file)

        Try

            If csvData.Length = 0 Then
                Throw New Exception("CSV File Appears to be Empty")
            End If

            If csvData(0).ToUpper.Trim.Contains("RANDOMIZATIONCODE") Then 'Added by Mrunal to check sheet is Normal
                Throw New Exception("File Is Not Compatible")
            End If

            Dim headings() As String
            Dim index As Integer = 0
            Dim isblankHdr As Boolean = True
            Dim Hdrindex As Integer = 0

            If isRowOneHeader Then

                index = 1

                For indexcsvdata As Integer = 0 To csvData.Length - 1

                    'replace spaces with underscores for column names
                    headings = csvData(indexcsvdata).Split(",")
                    For indexheading As Integer = 0 To headings.Length - 1
                        If headings(indexheading).ToUpper.Trim.Contains("PERIOD") Then
                            isblankHdr = False
                            Exit For
                        End If
                    Next indexheading
                    If isblankHdr = False Then
                        Hdrindex = indexcsvdata
                        Exit For
                    End If

                Next indexcsvdata

                headings = csvData(Hdrindex).Split(",")

                For index1 As Integer = 0 To headings.Length - 1
                    headings(index1) = headings(index1).Replace(" ", "_")
                    csvDataTable.Columns.Add(headings(index1), GetType(String))
                Next index1

            Else

                For indexcheck As Integer = 0 To csvData.Length - 1
                    If csvData(indexcheck).ToString.ToUpper.Contains("SPECIFICATION") And _
                       csvData(indexcheck).ToString.ToUpper.Contains("TREATMENT") Then
                        Exit For
                    ElseIf indexcheck = csvData.Length - 1 Then

                        Return Nothing
                        Exit Function
                    End If
                Next
                headings = csvData(0).Split(",")
                For index2 As Integer = 0 To headings.Length - 1

                    csvDataTable.Columns.Add("col" + (index2 + 1).ToString(), GetType(String))
                Next index2
            End If
            Dim isblank As Boolean = True

            For indexcsvdata1 As Integer = index To csvData.Length - 1
                Dim row As DataRow = csvDataTable.NewRow()
                Dim iscolumn As Boolean = False

                For indexheading1 As Integer = 0 To headings.Length - 1

                    If csvData(indexcsvdata1).Contains(",") AndAlso (csvData(indexcsvdata1).ToUpper.Contains("SUBJECT")) Then
                        isblank = False
                        iscolumn = True
                    End If

                    If csvData(indexcsvdata1).Contains(",") And isblank = False Then
                        row(indexheading1) = csvData(indexcsvdata1).Split(",")(indexheading1)
                    End If

                Next

                If isblank = False And iscolumn = False And csvData(indexcsvdata1).ToString.Trim() <> "" Then

                    csvDataTable.Rows.Add(row)
                End If
            Next

        Catch ex As Exception
            Me.objcommon.ShowAlert("File Is Not Compatible", Me.Page)
            Me.GV_Randomization.Style.Add("display", "none")
        End Try

        Return csvDataTable

    End Function

    Public Function formatdataset(ByVal Ds As DataTable) As DataSet
        Dim flie As String = FlAttachment.PostedFile.FileName
        Dim dt_format As DataTable = Ds
        Dim dr As DataRow
        Dim dr1 As DataRow
        Dim dt As New DataTable
        Dim dt_Randomization As DataTable
        Dim wStr As String = ""
        Dim wstr1 As String = ""
        Dim indexdatarow As Integer
        Dim isheading1 As Boolean = False
        Dim isheading2 As Boolean = False
        Dim eStr_Retu As String = ""
        Dim ds_Subjectid As New DataSet
        Dim dv_Subjectid As New DataView
        Dim dt_Subjectid As New DataTable
        Dim ds_Subject As New DataSet
        Dim a(20), b(20) As String
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
        Dim ds_Randomization As DataSet = Nothing
        Dim len As Integer = 0
        Dim periods As Integer
        Dim StartPosition As Integer = 0
        Dim MatchValue As Boolean = False
        Dim wStr2 As String = String.Empty
        Dim ds_RandomizationFileNo As New DataSet

        Try

            For indexdatarow = 0 To Ds.Rows.Count - 1
                dr = Ds.Rows(indexdatarow)
                MatchValue = False

                If dr("col1") <> "" AndAlso (AscW(dr("col1").ToString.Replace("""", "").Trim.ToUpper) >= 65 AndAlso AscW(dr("col1").ToString.Replace("""", "").Trim.ToUpper) <= 90) Then

                    If dr("col2").ToString.Replace("""", "").Trim.ToUpper = "SPECIFICATION" Or isheading1 = True Then
                        a(len) = dr("col1").ToString.ToUpper
                        b(len) = dr("col2")
                        isheading1 = True

                    ElseIf dr("col2").ToString.Replace("""", "").Trim.ToUpper = "TREATMENT" Or isheading2 = True Then

                        dr = Ds.Rows(indexdatarow)
                        a(len) = dr("col2").ToString.ToUpper
                        b(len) = dr("col1")

                        isheading2 = True

                    End If
                    len += 1
                End If

            Next

            wStr2 = "Select Max(ISNULL(nFileNo,0)) As nFileNo From RandomizationDetail WITH(NOLOCK)"

            ds_RandomizationFileNo = objHelp.GetResultSet(wStr2, "RandomizationDetail")
            wStr = "1=2"

            If Not objHelp.GetRandomizationDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                             ds_Randomization, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            wstr1 = "vWorkspaceId=" & Me.HProjectId.Value

            If Not objHelp.GetWorkspaceProtocolDetail(wstr1, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                    ds_Subjectid, eStr_Retu) Then
                Throw New Exception(eStr_Retu)

            End If

            dt_Randomization = ds_Randomization.Tables(0)
            dt_Randomization.Columns.Add("vProjectNo")
            For index_csv As Integer = 0 To dt_format.Rows.Count - 1

                dr1 = dt_format.Rows(index_csv)

                If dr1("Col2").ToString.Length - 1 < 0 Then
                    Exit For
                End If
                Dim sequencelength As Integer = dr1("Col2").ToString.Replace("""", "").Length
                periods = dt_format.Columns.Count - 2
                If dt_format.Columns.Count = 2 Then
                    periods = 1
                End If
                For indexperiod As Integer = 0 To periods - 1

                    dr = dt_Randomization.NewRow()
                    dr("vWorkspaceId") = Me.HProjectId.Value
                    dr("vProjectNo") = ds_Subjectid.Tables(0).Rows(0).Item("vProjectNo")
                    dr("iMysubjectNo") = dr1("Col1")
                    dr("iPeriod") = indexperiod + 1

                    Dim treatment As String = ""
                    Dim indexAssign1 As Integer = indexperiod + 3
                    treatment = dr1("Col" + indexAssign1.ToString()).ToString.Replace("""", "").Trim()

                    For j As Integer = 0 To len - 1
                        If treatment = a(j).Trim Then
                            dr("vProductType") = b(j).Replace("""", "")
                            Exit For

                        End If
                    Next

                    dr("vSequence") = dr1("Col2")
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "N"
                    dr("vRemark") = Me.txtRemarks.Text.ToString.Trim()
                    dr("nFileNo") = IIf(ds_RandomizationFileNo.Tables(0).Rows(0).Item("nFileNo").ToString.Trim() = "", 0, ds_RandomizationFileNo.Tables(0).Rows(0).Item("nFileNo")) + 1
                    dr("vFormulationType") = treatment.ToString.Trim()

                    dt_Randomization.Rows.Add(dr)
                    dt_Randomization.AcceptChanges()

                Next indexperiod

            Next index_csv

            dt_Subjectid = dt_Randomization.Copy()
            ds_Subject.Tables.Add(dt_Subjectid)
            dv_Subjectid = dt_Randomization.DefaultView().ToTable(False, "vWorkspaceId,iMysubjectNo,iPeriod,vProductType,iModifyBy,cStatusIndi".Split(",")).DefaultView
            dt_Randomization = dv_Subjectid.ToTable.Copy


            If Not objLambda.Save_RandomizationDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                      ds_Randomization, Me.Session(S_UserID), eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

        Catch ex As Exception
            Me.objcommon.ShowAlert("File Is Not Compatible", Me.Page)
        Finally
        End Try
        Return (ds_Subject)
    End Function

    Public Function csvDBToDataTable(ByVal file As String, ByVal isRowOneHeader As Boolean) As DataTable
        Dim csvDataTable = New DataTable()
        Dim csvData() As String = IO.File.ReadAllLines(file)

        Try
            If csvData.Length = 0 Then
                Throw New Exception("CSV File Appears to be Empty")
            End If

            If Not csvData(0).ToUpper.Trim.Contains("RANDOMIZATIONCODE") Then 'Added by Mrunal to check sheet for Double blinded or Not
                Throw New Exception("File Is Not Compatible")
            End If

            Dim headings() As String
            Dim index As Integer = 0
            Dim isblankHdr As Boolean = True
            Dim Hdrindex As Integer = 0

            If isRowOneHeader Then

                index = 1

                For indexcsvdata As Integer = 0 To csvData.Length - 1

                    'replace spaces with underscores for column names
                    headings = csvData(indexcsvdata).Split(",")
                    For indexheading As Integer = 0 To headings.Length - 1
                        If headings(indexheading).ToUpper.Trim.Contains("PERIOD") Then
                            isblankHdr = False
                            Exit For
                        End If
                    Next indexheading
                    If isblankHdr = False Then
                        Hdrindex = indexcsvdata
                        Exit For
                    End If

                Next indexcsvdata

                headings = csvData(Hdrindex).Split(",")

                For index1 As Integer = 0 To headings.Length - 1
                    headings(index1) = headings(index1).Replace(" ", "_")
                    csvDataTable.Columns.Add(headings(index1), GetType(String))
                Next index1

            Else
                For indexcheck As Integer = 0 To csvData.Length - 1
                    If csvData(indexcheck).ToString.ToUpper.Contains("SPECIFICATION") And _
                       csvData(indexcheck).ToString.ToUpper.Contains("TREATMENT") Then
                        Exit For
                    ElseIf indexcheck = csvData.Length - 1 Then

                        Return Nothing
                        Exit Function
                    End If
                Next
                headings = csvData(0).Split(",")
                For index2 As Integer = 0 To headings.Length - 1

                    csvDataTable.Columns.Add("col" + (index2 + 1).ToString(), GetType(String))
                Next index2
            End If
            Dim isblank As Boolean = True

            For indexcsvdata1 As Integer = index To csvData.Length - 1
                Dim row As DataRow = csvDataTable.NewRow()
                Dim iscolumn As Boolean = False

                For indexheading1 As Integer = 0 To headings.Length - 1

                    If csvData(indexcsvdata1).Contains(",") AndAlso (csvData(indexcsvdata1).ToUpper.Contains("SUBJECT")) And csvData(indexcsvdata1).ToUpper.ToString.Replace("""", "").Contains("RANDOMIZATIONCODE") Then
                        isblank = False
                        iscolumn = True
                    End If

                    If csvData(indexcsvdata1).Contains(",") And isblank = False Then
                        row(indexheading1) = csvData(indexcsvdata1).Split(",")(indexheading1)
                    End If

                Next

                If isblank = False And iscolumn = False And csvData(indexcsvdata1).ToString.Trim() <> "" Then

                    csvDataTable.Rows.Add(row)
                End If

            Next
        Catch ex As Exception
            Me.objcommon.ShowAlert("File Is Not Compatible", Me.Page)
        End Try

        Return csvDataTable

    End Function

    Public Function formatDoubleBlindedDataset(ByVal Ds As DataTable) As DataSet
        Dim flie As String = FlAttachment.PostedFile.FileName
        Dim dt_format As DataTable = Ds
        Dim dr As DataRow
        Dim dr1 As DataRow
        Dim dt As New DataTable
        Dim dt_Randomization As DataTable
        Dim wStr As String = String.Empty
        Dim wstr1 As String = String.Empty
        Dim indexdatarow As Integer
        Dim isheading1 As Boolean = False
        Dim isheading2 As Boolean = False
        Dim eStr_Retu As String = String.Empty
        Dim ds_Subjectid As New DataSet
        Dim dv_Subjectid As New DataView
        Dim dt_Subjectid As New DataTable
        Dim ds_Subject As New DataSet
        Dim a(10), b(10) As String
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
        Dim ds_Randomization As DataSet = Nothing
        Dim len As Integer = 0
        Dim periods As Integer
        Dim val As Integer
        Dim StartPosition As Integer = 0
        Dim tempindex As Integer = 0
        Dim tempexsub As Integer = 0
        Dim MatchValue As Boolean = False
        Dim wStr2 As String = String.Empty
        Dim ds_RandomizationFileNo As New DataSet

        Try
            For indexdatarow = 0 To Ds.Rows.Count - 1
                dr = Ds.Rows(indexdatarow)

                If dr("col1") <> "" AndAlso (AscW(dr("col1").ToString.Replace("""", "").Trim.ToUpper) >= 65 AndAlso AscW(dr("col1").ToString.Replace("""", "").Trim.ToUpper) <= 90) Then

                    If dr("col2").ToString.Replace("""", "").Trim.ToUpper = "SPECIFICATION" Or isheading1 = True Then
                        a(len) = dr("col1").ToString.ToUpper
                        b(len) = dr("col2")
                        isheading1 = True

                    ElseIf dr("col2").ToString.Replace("""", "").Trim.ToUpper = "TREATMENT" Or isheading2 = True Then

                        dr = Ds.Rows(indexdatarow)
                        a(len) = dr("col2").ToString.ToUpper
                        b(len) = dr("col1")

                        isheading2 = True

                    End If
                    len += 1
                End If

            Next
            wStr2 = "Select Max(ISNULL(nFileNo,0)) As nFileNo From RandomizationDetail WITH(NOLOCK)"

            ds_RandomizationFileNo = objHelp.GetResultSet(wStr2, "RandomizationDetail")
            wStr = "1=2"

            If Not objHelp.GetRandomizationDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                             ds_Randomization, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            wstr1 = "vWorkspaceId=" & Me.HProjectId.Value

            If Not objHelp.GetWorkspaceProtocolDetail(wstr1, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                    ds_Subjectid, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            dt_Randomization = ds_Randomization.Tables(0)
            dt_Randomization.Columns.Add("vProjectNo")
            For index_csv As Integer = 0 To dt_format.Rows.Count - 1
                dr1 = dt_format.Rows(index_csv)

                If dr1("Col2").ToString.Length - 1 < 0 Then
                    If dr1("Col1").ToString.Length > 0 Then
                        ds_Subject = Nothing
                        Throw New Exception()
                    End If
                    Exit For
                End If
                Dim sequencelength As Integer = dr1("Col2").ToString.Replace("""", "").Length

                periods = dt_format.Columns.Count - 3
                If dt_format.Columns.Count = 3 Then
                    periods = 1
                End If

                For indexperiod As Integer = 0 To periods - 1
                    dr = dt_Randomization.NewRow()
                    dr("vWorkspaceId") = Me.HProjectId.Value
                    dr("vProjectNo") = ds_Subjectid.Tables(0).Rows(0).Item("vProjectNo")
                    dr("iMysubjectNo") = dr1("Col1")
                    dr("iPeriod") = indexperiod + 1
                    dr("vRandomizationcode") = dr1("col" + (dt_format.Columns.Count).ToString()) 'Last column of Excelsheet

                    Dim treatment As String = ""
                    Dim indexAssign1 As Integer = indexperiod + 3
                    treatment = dr1("Col" + indexAssign1.ToString()).ToString.Replace("""", "").Trim()


                    For j As Integer = 0 To len - 1
                        If treatment = a(j).Trim Then
                            dr("vProductType") = b(j).Replace("""", "")
                            Exit For

                        End If
                    Next
                    dr("vSequence") = dr1("Col2")
                    
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "N"
                    dr("vRemark") = Me.txtRemarks.Text.ToString.Trim()
                    dr("nFileNo") = IIf(ds_RandomizationFileNo.Tables(0).Rows(0).Item("nFileNo").ToString.Trim() = "", 0, ds_RandomizationFileNo.Tables(0).Rows(0).Item("nFileNo")) + 1
                    dr("vFormulationType") = treatment.ToString.Trim()

                    dt_Randomization.Rows.Add(dr)
                    dt_Randomization.AcceptChanges()

                Next indexperiod
            Next index_csv

            dt_Subjectid = dt_Randomization.Copy()
            ds_Subject.Tables.Add(dt_Subjectid)
            dv_Subjectid = dt_Randomization.DefaultView().ToTable(False, "vWorkspaceId,iMysubjectNo,iPeriod,vProductType,iModifyBy,cStatusIndi,vRandomizationCode".Split(",")).DefaultView
            dt_Randomization = dv_Subjectid.ToTable.Copy


            If Not objLambda.Save_RandomizationDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                      ds_Randomization, Me.Session(S_UserID), eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

        Catch ex As Exception
            Me.objcommon.ShowAlert("File Is Not Compatible", Me.Page)
            ds_Subject = Nothing
        Finally
        End Try
        Return (ds_Subject)
    End Function
#End Region

#Region "Grid Events"
    Private Function fillgrid(ByVal dt_fillgrid As DataSet) As Boolean
        Dim estr As String = String.Empty
        Dim row_count As Integer
        Dim dt_fillgridData As New DataTable

        Try
            dt_fillgridData = ViewState("dt_gridDtlData")
            dt_fillgrid.Tables.Add(dt_fillgridData.Copy())
            GV_Randomization.DataSource = dt_fillgrid
            dt_fillgrid.Tables(0).DefaultView.RowFilter = "vRandomizationcode <> '' " ' Added By Mrunal Parekh on 03-jan-2012 for randomizationcode
            row_count = dt_fillgrid.Tables(0).DefaultView.ToTable.Rows.Count
            If row_count > 0 Then
                Me.chkdoubleblinded.Checked = True
            End If
            GV_Randomization.DataBind()
            mpeRandomizationDtl.Show()
            Me.GV_Randomization.Style.Add("display", "")
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message(), ".fillgrid")
        End Try
    End Function
    Protected Sub GV_Randomization_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GV_Randomization.PageIndexChanging
        Dim dt_ViewReasonMst As New DataSet
        GV_Randomization.PageIndex = e.NewPageIndex
        fillgrid(dt_ViewReasonMst)
    End Sub
    Protected Sub GV_Randomization_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_Randomization.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GVC_radcode).Visible = False
            If Me.chkdoubleblinded.Checked = True Then
                e.Row.Cells(GVC_producttype).Visible = True
                e.Row.Cells(GVC_radcode).Visible = True
            End If
        End If
    End Sub

    Protected Sub GV_Randomization_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_Randomization.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + 1 + (Me.GV_Randomization.PageSize * Me.GV_Randomization.PageIndex)
        End If
    End Sub

#End Region

#Region "Header Grid Data"
    Private Function fillgridData(ByVal dt_fillgrid As DataSet) As Boolean
        Dim estr As String = String.Empty
        Dim row_count As Integer

        Try
            dt_fillgrid = CType(ViewState("dt_grid"), DataSet)
            GV_RandomizationHdr.DataSource = dt_fillgrid
            dt_fillgrid.Tables(0).DefaultView.RowFilter = "vRandomizationcode <> '' "
            row_count = dt_fillgrid.Tables(0).DefaultView.ToTable.Rows.Count
            If row_count > 0 Then
                Me.chkdoubleblinded.Checked = True
            End If
            GV_RandomizationHdr.DataBind()
            Me.GV_RandomizationHdr.Style.Add("display", "")
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message(), ".fillgrid")
        End Try
    End Function
    Protected Sub GV_RandomizationHdr_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GV_RandomizationHdr.PageIndexChanging
        Dim dt_ViewReasonMst As New DataSet
        GV_RandomizationHdr.PageIndex = e.NewPageIndex
        fillgridData(dt_ViewReasonMst)

    End Sub

    Protected Sub GV_RandomizationHdr_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_RandomizationHdr.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.Footer Then
            If Me.chkdoubleblinded.Checked = True Then
                e.Row.Cells(GVC_producttype).Visible = True
                e.Row.Cells(GVC_radcode).Visible = True
            End If
        End If
    End Sub

    Protected Sub GV_RandomizationHdr_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_RandomizationHdr.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + 1 + (Me.GV_RandomizationHdr.PageSize * Me.GV_RandomizationHdr.PageIndex)
            CType(e.Row.FindControl("imgView"), ImageButton).CommandArgument = e.Row.RowIndex
        End If
    End Sub
    Protected Sub GV_RandomizationHdr_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GV_RandomizationHdr.RowCommand
        Dim ds_Details As New DataSet
        Dim eStr_Retu As String = String.Empty
        Dim wstr As String = String.Empty
        Dim wstrd As String = String.Empty
        Dim i As Integer = e.CommandArgument
        Dim MinRange As String = String.Empty
        Dim MaxRange As String = String.Empty
        Dim Range As String = String.Empty
        Dim FileNo As String = String.Empty
        Try
            MinRange = Me.GV_RandomizationHdr.Rows(i).Cells(GVRH_Range).Text.ToString.Trim().Split("-")(0)
            MaxRange = Me.GV_RandomizationHdr.Rows(i).Cells(GVRH_Range).Text.ToString.Trim().Split("-")(1)
            Range = Me.GV_RandomizationHdr.Rows(i).Cells(GVRH_Range).Text.ToString.Trim()
            FileNo = CType(Me.GV_RandomizationHdr.Rows(i).FindControl("lblnFileNo"), Label).Text

            ds_Details = Me.ViewState("dt_gridDtl")

            If Not ds_Details.Tables(0).Rows.Count > 0 Then
                Me.GV_Randomization.Style.Add("display", "none")
                Exit Sub
            End If

            If ds_Details.Tables(0).Rows.Count > 0 Then
                ds_Details.Tables(0).DefaultView.RowFilter = "nFileNo = '" & FileNo.ToString.Trim() & "'"

                Me.GV_Randomization.DataSource = Nothing
                Me.GV_Randomization.DataSource = ds_Details.Tables(0).DefaultView.ToTable().Copy()
                ViewState("dt_gridDtlData") = ds_Details.Tables(0).DefaultView.ToTable().Copy()
                Me.lblRandomizationDtl.Text = "Randomization Details for" + " [ " + Range + " ] "
                Me.GV_Randomization.PageIndex = 0
                Me.GV_Randomization.DataBind()
                mpeRandomizationDtl.Show()
                Me.chkdoubleblinded.Enabled = False
            Else
                Me.GV_Randomization.DataSource = Nothing
                Me.GV_Randomization.DataBind()
                objcommon.ShowAlert("NO RECORDS FOUND!", Me)
                Exit Sub
            End If
        Catch ex As Exception
            Me.objcommon.ShowAlert("Randomization Details Not Found!." + ex.Message.ToString() + ex.StackTrace.ToString(), Me.Page)
        Finally
            ds_Details = Nothing
        End Try

    End Sub

#End Region

#Region "Button Event"
    Protected Sub btnSaveRemarks_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveRemarks.Click
        Me.ViewState(Vs_AllowRemarks) = "N"
        MPERemarks.Hide()
        buttonSave_Click(Me.buttonSave, New EventArgs())
    End Sub

    Protected Sub buttonSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles buttonSave.Click

        Dim dtRandomizationDetail As New DataTable
        Dim dt As DataSet
        Dim files As String = FlAttachment.PostedFile.FileName
        Dim Serverfile As String = Server.MapPath(ConfigurationManager.AppSettings.Item("FolderForSubjectDetail").Trim() + "\SasFile") + "\File2.csv"
        Dim wstr As String = String.Empty
        Dim wstrd As String = String.Empty
        Dim filepath As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim iperiods As Integer = 0
        Dim ds_period As New DataSet
        Dim ds_Randomization As New DataSet
        Dim ds_DosingDtl As New DataSet
        Dim str As String = String.Empty
        Dim StrMinSubjectNo As String = String.Empty
        Dim StrMaxSubjectNo As String = String.Empty
        Dim strOldSubjectNo As String = String.Empty
        Dim dt_oldSubjectNo As New DataTable

        Dim ds_ProductTypePMS As New DataSet
        Dim TypeExist As Boolean = False
        Dim StartCount As Integer = 0

        Try

            If Me.ViewState(Vs_AllowRemarks) = "Y" AndAlso Not IsNothing(Me.ViewState("dt_gridDtl")) Then
                If Me.ViewState("dt_gridDtl").Tables(0).Rows.Count > 0 Then
                    MPERemarks.Show()
                    If File.Exists(Serverfile) Then
                        File.Delete(Serverfile)
                    End If
                    filepath = Server.MapPath(ConfigurationManager.AppSettings.Item("FolderForSubjectDetail").Trim() + "\SasFile")
                    FlAttachment.PostedFile.SaveAs(Server.MapPath(ConfigurationManager.AppSettings.Item("FolderForSubjectDetail").Trim() + "\SasFile") + "\File2.csv")
                    Me.txtRemarks.Text = ""
                    Exit Sub
                End If
            Else
                If Me.ViewState(Vs_AllowRemarks) Is Nothing Then
                    If File.Exists(Serverfile) Then
                        File.Delete(Serverfile)
                    End If
                    filepath = Server.MapPath(ConfigurationManager.AppSettings.Item("FolderForSubjectDetail").Trim() + "\SasFile")
                    FlAttachment.PostedFile.SaveAs(Server.MapPath(ConfigurationManager.AppSettings.Item("FolderForSubjectDetail").Trim() + "\SasFile") + "\File2.csv")

                End If
                wstr = "vWorkspaceId='" & Me.HProjectId.Value & "' And cStatusIndi <> 'D'"
                If Not objHelp.GetWorkspaceProtocolDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_period, eStr_Retu) Then
                    Response.Write(eStr_Retu)
                End If

                If Convert.ToString(ds_period.Tables(0).Rows(0).Item("iNoOfPeriods")).Trim() <> "" Then
                    iperiods = Convert.ToInt64(ds_period.Tables(0).Rows(0).Item("iNoOfPeriods").ToString())
                    ViewState("iperiods") = iperiods
                End If


                If chkdoubleblinded.Checked = True Then
                    dtRandomizationDetail = csvDBToDataTable(Serverfile, False)
                Else

                    dtRandomizationDetail = csvToDataTable(Serverfile, False)
                End If
                If dtRandomizationDetail Is Nothing Then
                    objcommon.ShowAlert("Please Select A Normal CSV File", Me)
                    Exit Sub
                End If

                ''str = Me.HProjectId.Value.ToString.Trim
                ds_ProductTypePMS = objHelp.ProcedureExecute("Proc_GetProductTypePMS", Me.HProjectId.Value.ToString.Trim)

                If ds_ProductTypePMS Is Nothing Or (Not ds_ProductTypePMS Is Nothing AndAlso ds_ProductTypePMS.Tables(0).Rows.Count <= 0) Then
                    objcommon.ShowAlert("Product Type Is Not Define In PMS", Me)
                    Exit Sub
                End If
                For i As Integer = 0 To dtRandomizationDetail.Rows.Count - 1
                    If IsNumeric(dtRandomizationDetail.Rows(i)("Col1").ToString()) AndAlso CInt(dtRandomizationDetail.Rows(i)("Col1").ToString()) <> 0 Then
                        str = str & dtRandomizationDetail.Rows(i)("Col1").ToString()
                        str += If((i < dtRandomizationDetail.Rows.Count - 4), ",", String.Empty)
                    Else
                        ''Added By ketan For Check Product Type From PMS
                        If StartCount = 1 Then
                            For Each dr In ds_ProductTypePMS.Tables(0).Rows
                                If Convert.ToString(dr("vProductType")).Trim().ToUpper = Convert.ToString(dtRandomizationDetail.Rows(i)("Col1")).Trim().ToUpper Then
                                    TypeExist = True
                                End If
                            Next
                            If TypeExist = False Then
                                objcommon.ShowAlert("Product Type " + Convert.ToString(dtRandomizationDetail.Rows(i)("Col1")) + " Is Not Define In PMS", Me)
                                Exit Sub
                            Else
                                TypeExist = False
                            End If
                        End If

                        If dtRandomizationDetail.Rows(i)("Col1").ToString().ToUpper = "SPECIFICATION" Then
                            StartCount = 1
                        End If
                    End If
                    ''Ended by ketan
                Next
                StrMaxSubjectNo = str.Split(",")(0)

                If chkdoubleblinded.Checked = False Then
                    If dtRandomizationDetail.Columns.Count > 2 Then
                        If iperiods <> dtRandomizationDetail.Columns.Count - 2 Then
                            objcommon.ShowAlert("No Of Periods In CSV Does Not Match With Periods In Project Selected", Me)
                            Me.ViewState(Vs_AllowRemarks) = "Y"
                            Exit Sub
                        End If
                    Else 'As in parellal when there is one period it hase 2 columns
                        If iperiods <> dtRandomizationDetail.Columns.Count - 1 Then
                            objcommon.ShowAlert("No Of Periods In CSV Does Not Match With Periods In Project Selected", Me)
                            Me.ViewState(Vs_AllowRemarks) = "Y"
                            Exit Sub
                        End If
                    End If
                Else
                    If dtRandomizationDetail.Columns.Count > 3 Then ' 2 replace by 3 for radmomizationcode column by Mrunal
                        If iperiods <> dtRandomizationDetail.Columns.Count - 3 Then ' 2 replace by 3 for radmomizationcode column by Mrunal
                            objcommon.ShowAlert("No Of Periods In CSV Does Not Match With Periods In Project Selected", Me)
                            Me.ViewState(Vs_AllowRemarks) = "Y"
                            Exit Sub
                        End If
                    Else
                        If iperiods <> dtRandomizationDetail.Columns.Count - 2 Then ' 1 replace by 2 for radmomizationcode column by Mrunal
                            objcommon.ShowAlert("No Of Periods In CSV Does Not Match With Periods In Project Selected", Me)
                            Me.ViewState(Vs_AllowRemarks) = "Y"
                            Exit Sub
                        End If
                    End If
                End If

                wstr = "vWorkspaceId='" & Me.HProjectId.Value & "' And cStatusIndi <> 'D'"
                wstrd = "vParentWorkspaceId='" & Me.HProjectId.Value & "' And cStatusIndi <> 'D'"

                If Not objHelp.GetRandomizationDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Randomization, eStr_Retu) Then
                    Response.Write(eStr_Retu)
                End If
                ''Added by Aaditya
                If Not IsNothing(ds_Randomization.Tables(0)) AndAlso ds_Randomization.Tables(0).Rows.Count > 0 Then
                    ds_Randomization.Tables(0).DefaultView.Sort = "iMySubjectNo Desc"
                    StrMinSubjectNo = CInt(ds_Randomization.Tables(0).DefaultView.ToTable.Rows(0)("iMySubjectNo").ToString) + 1

                    ds_Randomization.Tables(0).DefaultView.RowFilter = "iMySubjectNo IN (" + str.ToString() + ")"
                    ds_Randomization.Tables(0).DefaultView.ToTable().Copy()

                    If Me.HIsTestSite.Value = "N" Or Me.HIsTestSite.Value = "" Then
                        If Not objHelp.GetDosingDetail(wstrd, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DosingDtl, eStr_Retu) Then
                            Me.ShowErrorMessage(eStr_Retu.ToString(), eStr_Retu)
                        End If

                    Else
                        If Not objHelp.GetDosingDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DosingDtl, eStr_Retu) Then
                            Me.ShowErrorMessage(eStr_Retu.ToString(), eStr_Retu)
                        End If
                    End If

                    If Not IsNothing(ds_DosingDtl) AndAlso ds_DosingDtl.Tables(0).Rows.Count > 0 Then
                        ds_DosingDtl.Tables(0).DefaultView.RowFilter = "iMySubjectNo IN (" + str.ToString() + ")"
                        ds_DosingDtl.Tables(0).DefaultView.ToTable().Copy()

                        If ds_DosingDtl.Tables(0).DefaultView.ToTable().Copy().Rows.Count > 0 Then
                            objcommon.ShowAlert("You Can Not Upload Randomization File As Dosing Labels Are Generated For This Project or its Child.First Delete Labels And Upload Again.", Me.Page())
                            Me.ViewState(Vs_AllowRemarks) = "Y"
                            Exit Sub
                        End If
                    End If

                End If

                ''Ended by Aaditya
                If ds_Randomization.Tables(0).DefaultView.ToTable().Copy().Rows.Count > 0 Then ''ds_Randomization.Tables(0).Rows.Count > 0 Then
                    ViewState(VS_dsRandomization) = ds_Randomization.Tables(0).DefaultView.ToTable().Copy()
                    Me.ViewState(Vs_AllowRemarks) = "Y"

                    dt_oldSubjectNo = ds_Randomization.Tables(0).DefaultView.ToTable(True, "iMySubjectNo")
                    dt_oldSubjectNo.DefaultView.Sort = "iMySubjectNo Asc"

                    For i As Integer = 0 To dt_oldSubjectNo.DefaultView.ToTable.Rows.Count - 1
                        strOldSubjectNo = strOldSubjectNo & dt_oldSubjectNo.DefaultView.ToTable.Rows(i)("iMySubjectNo").ToString()
                        strOldSubjectNo += If((i < dt_oldSubjectNo.DefaultView.ToTable.Rows.Count - 1), ",", String.Empty)
                    Next
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Show", "ShowConfirmation('" + strOldSubjectNo + "');", True)

                ElseIf Not ds_Randomization.Tables(0).DefaultView.ToTable().Copy().Rows.Count > 0 Then
                    If chkdoubleblinded.Checked = True Then
                        ''Added by Aaditya
                        If Not IsNothing(ds_Randomization.Tables(0)) AndAlso ds_Randomization.Tables(0).Rows.Count > 0 Then
                            If StrMaxSubjectNo <> StrMinSubjectNo Then
                                objcommon.ShowAlert("Subject No. sequence is not correct in file. Kindly update and upload again.", Me)
                                Me.ViewState(Vs_AllowRemarks) = "Y"
                                Exit Sub
                            End If
                        End If
                        ''Ended by Aaditya
                        dtRandomizationDetail = csvDBToDataTable(Serverfile, False)
                        dt = formatDoubleBlindedDataset(dtRandomizationDetail)
                    Else
                        If Not IsNothing(ds_Randomization.Tables(0)) AndAlso ds_Randomization.Tables(0).Rows.Count > 0 Then
                            If StrMaxSubjectNo <> StrMinSubjectNo Then
                                objcommon.ShowAlert("Subject No. sequence is not correct in file. Kindly update and upload again.", Me)
                                Me.ViewState(Vs_AllowRemarks) = "Y"
                                Exit Sub
                            End If
                        End If
                        dtRandomizationDetail = csvToDataTable(Serverfile, False)
                        dt = formatdataset(dtRandomizationDetail)

                    End If
                    If IsNothing(dt) Then
                        Me.GV_Randomization.Style.Add("display", "none")
                        Exit Sub
                    End If
                    objcommon.ShowAlert("File Uploaded Successfully !", Me)
                    ViewState("dt_grid") = dt
                    btnSetProject_Click(sender, e)
                End If
            End If
        Catch ex As Exception
            Me.objcommon.ShowAlert("File Is Not Compatible." + ex.Message.ToString() + ex.StackTrace.ToString(), Me.Page)
            Me.ShowErrorMessage("File is Not Compatible. " + ex.Message.ToString() + ex.StackTrace.ToString(), ex.Message)
        End Try

    End Sub

    Protected Sub buttonExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim wstr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_randomization As DataSet = Nothing
        Dim ds_randomizationDtl As DataSet = Nothing
        Dim ds_Dosing As DataSet = Nothing
        Dim wstrd As String = String.Empty
        Dim Str As String = String.Empty

        Try
            HParentWorkSpaceId.Value = ""
            HIsTestSite.Value = ""
            Me.chkdoubleblinded.Checked = False
            Me.GV_RandomizationHdr.DataSource = Nothing
            Me.GV_RandomizationHdr.DataBind()

            If Not Me.GetParentWorkSpaceId() Then
                Me.objcommon.ShowAlert("Error While Getting Data Form view_WorkSpaceProtocoldetail", Me.Page)
                Exit Sub
            End If

            If HParentWorkSpaceId.Value <> "" And (HIsTestSite.Value = "N" Or HIsTestSite.Value = "") Then
                objcommon.ShowAlert("You can't upload Randomization Sheet to this site", Me.Page)
                Exit Sub
            End If

            Me.buttonSave.Style.Add("display", "")
            wstr = "vWorkspaceId='" & Me.HProjectId.Value & "' And cStatusIndi <> 'D'"
            wstrd = "vParentWorkspaceId='" & Me.HProjectId.Value & "' And cStatusIndi <> 'D'"

            If Not objHelp.GetView_RandomizationDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_randomizationDtl, eStr) Then
                Me.ShowErrorMessage("Problem while geting data for the project", eStr)
            End If

            If Not ds_randomizationDtl.Tables(0).Rows.Count > 0 Then
                Exit Sub
            ElseIf ds_randomizationDtl.Tables(0).Rows.Count > 0 Then
                Me.ViewState("dt_gridDtl") = ds_randomizationDtl
                Me.ViewState(Vs_AllowRemarks) = "Y"
            End If

            Str = Me.HProjectId.Value.ToString.Trim
            ds_randomization = objHelp.ProcedureExecute("Proc_GetRandomizationDetailData", Str)

            If Not IsNothing(ds_randomization) AndAlso ds_randomization.Tables(0).Rows.Count > 0 Then
                ds_randomization.Tables(0).Columns.Add("dModifyOffSet", Type.GetType("System.String"))
                For Each dr_Audit In ds_randomization.Tables(0).Rows
                    dr_Audit("dModifyOffSet") = Convert.ToString(CDate(dr_Audit("dUploadedOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                Next
            End If

            If Not ds_randomization.Tables(0).Rows.Count > 0 Then
                Me.GV_RandomizationHdr.Style.Add("display", "none")
                Exit Sub
            End If
            Me.ViewState("dt_grid") = ds_randomization
            '  Me.fillgridData(ds_randomization) ''Commented by ketan
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "projectGrid", "projectGrid();", True)    '' Added by ketan for get data Randomization
        Catch ex As Exception
            Me.objcommon.ShowAlert("File Is Not Compatible." + ex.Message.ToString() + ex.StackTrace.ToString(), Me.Page)
            Me.ShowErrorMessage("File is Not Compatible. " + ex.Message.ToString() + ex.StackTrace.ToString(), ex.Message)
        End Try
    End Sub


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

    Protected Sub LnkBtnCsvFileFormat_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnCsvFileFormat.Click
        If Me.chkdoubleblinded.Checked = True Then
            mpeDialogdoubleblind.Show()
        Else
            mpeDialog.Show()
        End If

    End Sub

    Protected Sub BtnDeleteOld_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnDeleteOld.Click

        Dim dtRandomizationDetail As New DataTable
        Dim dt As DataSet
        Dim Serverfile As String = Server.MapPath(ConfigurationManager.AppSettings.Item("FolderForSubjectDetail").Trim() + "\SasFile") + "\File2.csv"
        Dim wstr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim iperiods As Integer = 0
        Dim ds_period As New DataSet
        Dim ds_Randomization As New DataSet
        Dim dr_Randomization As DataRow
        Dim dtDeleteOldData As New DataTable

        Try
            dtDeleteOldData = CType(ViewState(VS_dsRandomization), DataTable)
            ds_Randomization.Tables.Add(dtDeleteOldData.Copy())

            If ds_Randomization.Tables(0).Rows.Count > 0 Then

                For index As Integer = 0 To ds_Randomization.Tables(0).Rows.Count - 1
                    dr_Randomization = ds_Randomization.Tables(0).Rows(index)
                    dr_Randomization("cStatusIndi") = "D"
                    ds_Randomization.AcceptChanges()
                Next index

                If Not objLambda.Save_RandomizationDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_Randomization, Me.Session(S_UserID), eStr_Retu) Then
                    Me.ShowErrorMessage("Problem While Deleting Previous File", eStr_Retu)
                    Exit Sub
                End If

            End If

            If chkdoubleblinded.Checked = True Then
                dtRandomizationDetail = csvDBToDataTable(Serverfile, False)
                dt = formatDoubleBlindedDataset(dtRandomizationDetail)
            Else
                dtRandomizationDetail = csvToDataTable(Serverfile, False)
                dt = formatdataset(dtRandomizationDetail)
            End If
            If IsNothing(dt) Then
                Me.GV_Randomization.Style.Add("display", "none")
                Exit Sub
            End If
            objcommon.ShowAlert("File Uploaded Successfully !", Me)
            ViewState("dt_grid") = dt
            btnSetProject_Click(sender, e)

        Catch ex As Exception
            Me.objcommon.ShowAlert("File is Not Compatible." + ex.Message.ToString() + ex.StackTrace.ToString(), Me.Page)
            Me.ShowErrorMessage("File is Not Compatible. " + ex.Message.ToString() + ex.StackTrace.ToString(), ex.Message)
            Me.GV_Randomization.Style.Add("display", "none")
        End Try
    End Sub

    Protected Sub BtnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnUpdate.Click

        Dim dtRandomizationDetail As New DataTable
        Dim dt As DataSet
        Dim Serverfile As String = Server.MapPath(ConfigurationManager.AppSettings.Item("FolderForSubjectDetail").Trim() + "\SasFile") + "\File2.csv"
        Dim wstr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim iperiods As Integer = 0
        Dim ds_period As New DataSet

        Try

            If chkdoubleblinded.Checked = True Then
                dtRandomizationDetail = csvDBToDataTable(Serverfile, False)
                dt = formatDoubleBlindedDataset(dtRandomizationDetail)
            Else
                dtRandomizationDetail = csvToDataTable(Serverfile, False)
                dt = formatdataset(dtRandomizationDetail)
            End If

            objcommon.ShowAlert("File Uploaded Successfully !", Me)
            ViewState("dt_grid") = dt
            btnSetProject_Click(sender, e)

        Catch ex As Exception
            Me.objcommon.ShowAlert("File Is Not Compatible." + ex.Message.ToString() + ex.StackTrace.ToString(), Me.Page)
            Me.ShowErrorMessage("File Is Not Compatible. " + ex.Message.ToString() + ex.StackTrace.ToString(), ex.Message)
        End Try

    End Sub

#Region "GetParentWorkSpaceId"
    Private Function GetParentWorkSpaceId() As Boolean
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds As New DataSet
        wStr = "vWorkspaceId='" + HProjectId.Value + "'"
        If Not objHelp.GetData("view_workspaceprotocoldetail", "vParentWorkspaceId,cIsTestSite", wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr) Then
            Throw New Exception(eStr)
        End If
        If ds.Tables(0).Rows.Count > 0 And IsDBNull(ds.Tables(0).Rows(0).Item("vParentWorkspaceId")) = False Then
            HParentWorkSpaceId.Value = ds.Tables(0).Rows(0).Item("vParentWorkspaceId")
            If IsDBNull(ds.Tables(0).Rows(0).Item("cIsTestSite")) = False Then
                HIsTestSite.Value = ds.Tables(0).Rows(0).Item("cIsTestSite")
            End If
        End If
        Return True
    End Function
#End Region
    'Added by ketan

    Private Function bindGrid() As Boolean
        Dim dtRandomizationGrid As New DataTable
        Dim drActivity As DataRow
        Try
            If Not dtRandomizationGrid Is Nothing Then
                dtRandomizationGrid.Columns.Add("SrNo")
                dtRandomizationGrid.Columns.Add("ProjectNo")
                dtRandomizationGrid.Columns.Add("FileName")
                dtRandomizationGrid.Columns.Add("UpdatedBy")
                dtRandomizationGrid.Columns.Add("UploadedOn")
                dtRandomizationGrid.Columns.Add("Remarks")
                dtRandomizationGrid.Columns.Add("AuditTrail")
            End If
            drActivity = dtRandomizationGrid.NewRow()
            dtRandomizationGrid.Rows.Add(drActivity)
            dtRandomizationGrid.AcceptChanges()
            gvRandomizationHdr.DataSource = dtRandomizationGrid
            gvRandomizationHdr.DataBind()
            Me.ViewState("dt_grid") = dtRandomizationGrid

            Return True
        Catch ex As Exception
            Me.objCommon.ShowAlert("Error while bindGrid()", Me.Page)
            Return False
        End Try
    End Function

#Region "Web Method"

    <WebMethod> _
    Public Shared Function FillProjectGrid(ByVal WorkspaceID As String) As String
        Dim strReturn As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_randomization As New DataSet
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim dtData As New DataTable
        Dim dtProjectGrid As New DataTable
        Dim drProject As DataRow
        Dim i As Integer = 1
        Dim dt As DataTable = Nothing

        Try
            wStr = "vWorkspaceId = " + WorkspaceID
            ds_randomization = objHelp.ProcedureExecute("Proc_GetRandomizationDetailData", WorkspaceID)
            dt = ds_randomization.Tables(0)
            Dim dv As New DataView(dt)
            If Not dtProjectGrid Is Nothing Then
                dtProjectGrid.Columns.Add("SrNo")
                dtProjectGrid.Columns.Add("ProjectNo")
                dtProjectGrid.Columns.Add("FileName")
                dtProjectGrid.Columns.Add("UploadedBy")
                dtProjectGrid.Columns.Add("UploadedOn")
                dtProjectGrid.Columns.Add("Remarks")
                dtProjectGrid.Columns.Add("AuditTrail")
                dtProjectGrid.Columns.Add("nFileNo")

            End If
            If dv.ToTable.Rows.Count > 0 Then

                For Each dr In dt.Rows
                    drProject = dtProjectGrid.NewRow()
                    drProject("SrNo") = i
                    drProject("ProjectNo") = dr("vProjectNo").ToString()
                    drProject("FileName") = dr("Range").ToString()
                    drProject("UploadedBy") = dr("vUploadedBy").ToString()
                    drProject("UploadedOn") = Convert.ToString(CDate(dr("dUploadedOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                    drProject("Remarks") = dr("vRemarks").ToString()
                    drProject("AuditTrail") = ""
                    drProject("nFileNo") = dr("nFileNo").ToString()

                    dtProjectGrid.Rows.Add(drProject)
                    i = i + 1
                Next
                dtProjectGrid.AcceptChanges()
            End If

            strReturn = JsonConvert.SerializeObject(dtProjectGrid)
            Return strReturn
        Catch ex As Exception
            Throw New Exception("Error while FillActivityGrid")
        End Try
    End Function


    <WebMethod> _
    Public Shared Function AuditTrail(ByVal WorkspaceID As String, ByVal nFileNo As String) As String
        Dim wStr As String = String.Empty
        Dim WhereString As String = String.Empty
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim strReturn As String = String.Empty
        Dim ds_save As New DataSet
        Dim ds_randomizationDtl As New DataSet
        Dim estr As String = String.Empty
        Dim dtRandomizationDtl As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Try

            wStr = "vWorkspaceId = " + WorkspaceID + " and nFileNo = '" + nFileNo + "' "

            If Not objHelp.GetView_RandomizationDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_randomizationDtl, estr) Then

                Throw New Exception("Problem while geting data for the project")
            End If

            If Not dtRandomizationDtl Is Nothing Then
                dtRandomizationDtl.Columns.Add("SrNo")
                dtRandomizationDtl.Columns.Add("ProjectNo")
                dtRandomizationDtl.Columns.Add("SubjectNo")
                dtRandomizationDtl.Columns.Add("Period")
                dtRandomizationDtl.Columns.Add("ProductType")
                dtRandomizationDtl.Columns.Add("RandomizationCode")
            End If
            dt = ds_randomizationDtl.Tables(0)
            Dim dv As New DataView(dt)
            
            For Each dr As DataRow In dv.ToTable.Rows

                drAuditTrail = dtRandomizationDtl.NewRow()

                drAuditTrail("SrNo") = i
                drAuditTrail("ProjectNo") = dr("vProjectNo").ToString()
                drAuditTrail("SubjectNo") = dr("iMySubjectNo").ToString()
                drAuditTrail("Period") = dr("iPeriod").ToString()
                drAuditTrail("ProductType") = dr("vProductType").ToString()
                drAuditTrail("RandomizationCode") = Convert.ToString(dr("vRandomizationcode"))
                dtRandomizationDtl.Rows.Add(drAuditTrail)
                dtRandomizationDtl.AcceptChanges()
                i += 1
            Next

            strReturn = JsonConvert.SerializeObject(dtRandomizationDtl)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

#End Region

End Class


