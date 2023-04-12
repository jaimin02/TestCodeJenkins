
Imports System.Data.OleDb
Imports System.Data.DataTableExtensions
Imports System.Data
Imports Microsoft.Office.Interop
Imports Microsoft.VisualBasic.FileIO
Imports Microsoft.Office.Core
Imports System.Web.Services
Imports Newtonsoft.Json


Partial Class frmManageGlobalDictionary
    Inherits System.Web.UI.Page


#Region " Variable Declaration "

    Dim objCommon As New clsCommon
    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
    Dim objDataLogic As New clsDataLogic


    Dim eStr As String
    Dim eStr_retu As String


    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_RefTableName As Integer = 1
    Private Const GVC_RefTableRemark As Integer = 1
    Private Const GVC_Browse As Integer = 2
    Private Const GVC_CurrnetStatus As Integer = 3
    Private Const GVC_status As Integer = 4
    Private Const GVC_RefTableNo As Integer = 5

    Private Const Vs_FilePath As String = "FilePath"
    Private Const VS_DtMedDRA As String = "DtMedDRA"
    Private Const VS_DtMedDRATable As String = "dt_Blanktable"
    Private Const Vs_dsMedDRALog As String = "dsMedDRALog"
    Private Ds_MedDRALog As DataSet
    Dim dt_MedDRA_1_mdhierarchy As New DataTable
    Private Shared nRefMasterNo As String = String.Empty
    Private Shared mode As String = String.Empty
    Private Shared RefTableName As String = String.Empty


    Private Const Vs_dsGlDictionary As String = "ReferenceTableDefinitions"
    Dim dsBulkSave As New DataSet



#End Region

#Region " Load Events "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Not IsPostBack Then
                Me.GenCall()
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "projectGrid", "bindgvwMngGlDictionary();", True)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)

        End Try

    End Sub

#End Region

#Region " GenCall() "

    Protected Function GenCall() As Boolean
        Dim ds As New DataSet
        Try
            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If
            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall")

        Finally

        End Try

    End Function

#End Region

#Region "GenCall_Data"
    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean
        Dim wstr As String = String.Empty
        Dim estr_Retu As String = String.Empty
        Dim ds_GlobalDictionary As New DataSet

        Try

            wstr = "cRefTableType = 'D'"
            If Not objHelp.GetReferenceTableDefinitions(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_GlobalDictionary, estr_Retu) Then
                objCommon.ShowAlert("Error While Geting Data", Me)
                Return False

            End If
            ViewState(Vs_dsGlDictionary) = ds_GlobalDictionary
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....GenCall_Data")
        End Try
    End Function
#End Region

#Region "Gencall_ShowUI"
    Private Function GenCall_ShowUI() As Boolean
        Dim dt_UserMst As DataTable = Nothing

        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Try
            Page.Title = ":: Global Dictionary Management :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Global Dictionary Management"

            If Not FillGrid() Then
                Me.objCommon.ShowAlert("Error while filling Grid", Me)
            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
        Finally
        End Try
    End Function

#End Region

#Region "FillGrid"
    Private Function FillGrid()
        Dim dt_GlobalDictionary As New DataTable
        dt_GlobalDictionary = CType(ViewState(Vs_dsGlDictionary), DataSet).Tables(0)
        If dt_GlobalDictionary.Rows.Count <= 0 Then
            Return False
        End If

        Me.gvwMngGlDictionary.DataSource = dt_GlobalDictionary
        Me.gvwMngGlDictionary.DataBind()
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "projectGrid", "bindgvwMngGlDictionary();", True)
        Return True
    End Function
#End Region

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
    End Sub

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
    End Sub

#End Region

#Region "Grid Manage Global Dictinary Events"
    Protected Sub gvwMngGlDictionary_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvwMngGlDictionary.PageIndexChanging
        Me.gvwMngGlDictionary.PageIndex = e.NewPageIndex
        FillGrid()
    End Sub

    Protected Sub gvwMngGlDictionary_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwMngGlDictionary.RowCommand
        Try
            Dim q As String = String.Empty
            Dim ds_globaldictionary As New DataSet
            Dim dsReferenceTable As New DataSet
            Dim eStr As String = String.Empty
            Dim dsCheckUsedTable As New DataSet
            ds_globaldictionary = CType(ViewState(Vs_dsGlDictionary), DataSet)

            txtRemarks.Text = ""
            If e.CommandName.ToUpper = "STATUS" Then
                mdlRemarks.Show()
                nRefMasterNo = Convert.ToString(e.CommandArgument).Trim()
                mode = "STATUS"
               
            End If

            If e.CommandName.ToUpper = "DELETE" Then

                Dim commandArgs As String() = e.CommandArgument.ToString().Split(New Char() {";"c})
                Dim firstArgVal As String = commandArgs(0)
                Dim secondArgVal As String = commandArgs(1)

                nRefMasterNo = firstArgVal
                mode = "DELETE"
                RefTableName = secondArgVal

            End If
        Catch ex As Exception

            Me.ShowErrorMessage(ex.Message, eStr)

        End Try
    End Sub

    Protected Sub gvwMngGlDictionary_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwMngGlDictionary.RowCreated
        Try
            e.Row.Cells(GVC_RefTableNo).Style.Add("display", "none")
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub gvwMngGlDictionary_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwMngGlDictionary.RowDataBound
        Dim ds_globaldictionary As New DataSet
        ds_globaldictionary = CType(ViewState(Vs_dsGlDictionary), DataSet)
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then

                e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (Me.gvwMngGlDictionary.PageSize * Me.gvwMngGlDictionary.PageIndex) + 1
                CType(e.Row.FindControl("lnkBrowse"), LinkButton).OnClientClick = "return OpenWindow('frmCTMMeddraBrowse.aspx?nRefMasterNo=" + e.Row.Cells(GVC_RefTableNo).Text.Trim() + "');"

                CType(e.Row.FindControl("LnkGrdstatus"), LinkButton).OnClientClick = "return show_confirm(this);"
                CType(e.Row.FindControl("LnkGrdstatus"), LinkButton).Attributes.Add("mode", "STATUS")
                CType(e.Row.FindControl("LnkGrdstatus"), LinkButton).Attributes.Add("nRefMasterNo", e.Row.Cells(GVC_RefTableNo).Text)
                CType(e.Row.FindControl("LnkGrdstatus"), LinkButton).CommandArgument = e.Row.Cells(GVC_RefTableNo).Text
                CType(e.Row.FindControl("LnkGrdstatus"), LinkButton).CommandName = "STATUS"

                CType(e.Row.FindControl("lnkDelete"), ImageButton).OnClientClick = "return show_confirm(this);"
                CType(e.Row.FindControl("lnkDelete"), ImageButton).Attributes.Add("RefTableName", e.Row.Cells(GVC_RefTableName).Text)
                CType(e.Row.FindControl("lnkDelete"), ImageButton).Attributes.Add("mode", "DELETE")
                CType(e.Row.FindControl("lnkDelete"), ImageButton).Attributes.Add("nRefMasterNo", e.Row.Cells(GVC_RefTableNo).Text)

                CType(e.Row.FindControl("lnkDelete"), ImageButton).CommandArgument = e.Row.Cells(GVC_RefTableNo).Text + ";" + e.Row.Cells(GVC_RefTableName).Text
                CType(e.Row.FindControl("lnkDelete"), ImageButton).CommandName = "DELETE"

                CType(e.Row.FindControl("lnkAudit"), ImageButton).Attributes.Add("nRefMasterNo", e.Row.Cells(GVC_RefTableNo).Text)

                If ds_globaldictionary.Tables(0).Rows(e.Row.RowIndex)("cActiveFlag") = "Y" Then
                    CType(e.Row.FindControl("lblCurrentStatus"), Label).Text = "Active"
                    CType(e.Row.FindControl("LnkGrdstatus"), LinkButton).Text = "InActive"

                ElseIf ds_globaldictionary.Tables(0).Rows(e.Row.RowIndex)("cActiveFlag") <> "Y" Then
                    CType(e.Row.FindControl("lblCurrentStatus"), Label).Text = "InActive"
                    CType(e.Row.FindControl("LnkGrdstatus"), LinkButton).Text = "Active"
                    e.Row.BackColor = Drawing.Color.FromArgb(255, 189, 121)
                End If
              

            End If

        Catch ex As Exception

        End Try

    End Sub

#End Region

#Region "Button Click Event"
    Protected Sub btnExit_Click(sender As Object, e As EventArgs)
        Response.Redirect("frmMainPage.aspx")
    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        txtDictionaryName.Text = ""
        txtmedDraVersion.Text = ""

    End Sub

    Protected Sub btnRemarksUpdate_Click(sender As Object, e As EventArgs) Handles btnRemarksUpdate.Click
        Dim q As String = String.Empty
        Dim ds_globaldictionary As New DataSet
        Dim dsReferenceTable As New DataSet
        Dim eStr As String = String.Empty
        Dim dsCheckUsedTable As New DataSet
        ds_globaldictionary = CType(ViewState(Vs_dsGlDictionary), DataSet)
        Try

            If mode = "STATUS" Then

                If Not objHelp.GetReferenceTableDefinitions("nRefMasterNo='" + nRefMasterNo + "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsReferenceTable, eStr) Then
                    objCommon.ShowAlert("Error While Getting Reference Table", Me.Page)
                    Exit Sub
                End If

                If dsReferenceTable.Tables("ReferenceTableDefinitions").Rows(0).Item("cActiveFlag") = "Y" Then
                    dsReferenceTable.Tables("ReferenceTableDefinitions").Rows(0).Item("cActiveFlag") = "N"
                    dsReferenceTable.Tables("ReferenceTableDefinitions").Rows(0).Item("vRemark") = Convert.ToString(txtRemarks.Text)
                    dsReferenceTable.Tables("ReferenceTableDefinitions").Rows(0).Item("iModifyBy") = Me.Session(S_UserID)
                Else
                    dsReferenceTable.Tables("ReferenceTableDefinitions").Rows(0).Item("cActiveFlag") = "Y"
                    dsReferenceTable.Tables("ReferenceTableDefinitions").Rows(0).Item("vRemark") = Convert.ToString(txtRemarks.Text)
                    dsReferenceTable.Tables("ReferenceTableDefinitions").Rows(0).Item("iModifyBy") = Me.Session(S_UserID)
                End If
                dsReferenceTable.AcceptChanges()
                If Not objLambda.Save_ReferenceTableDefinitions(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, dsReferenceTable, Me.Session(S_UserID), eStr) Then
                    objCommon.ShowAlert("Error While Saving Reference Table", Me.Page)
                    Exit Sub
                End If
                objCommon.ShowAlert("Status Changed!", Me.Page)
                
            End If

            If mode = "DELETE" Then
                If Not objHelp.GetMedExMst("vRefTable='" + RefTableName + "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsCheckUsedTable, eStr) Then
                    Me.ShowErrorMessage("Error On Checking Weather Dictionary In Use", eStr)
                    Exit Sub
                End If
                If dsCheckUsedTable.Tables(0).Rows.Count > 0 Then
                    objCommon.ShowAlert("This Dictionary Is Used While Creating A Attribute" + Environment.NewLine + "You Can Not Delete This Dictionary", Me.Page)
                    Exit Sub
                End If

                If Not objHelp.GetReferenceTableDefinitions("nRefMasterNo='" + nRefMasterNo.Trim() + "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsReferenceTable, eStr) Then
                    objCommon.ShowAlert("Error While Getting Reference Table", Me.Page)
                    Exit Sub
                End If

                dsReferenceTable.Tables("ReferenceTableDefinitions").Rows(0).Item("cStatusIndi") = "D"
                dsReferenceTable.Tables("ReferenceTableDefinitions").Rows(0).Item("vRemark") = Convert.ToString(txtRemarks.Text)

                dsReferenceTable.AcceptChanges()
                If Not objLambda.Save_ReferenceTableDefinitions(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, dsReferenceTable, HttpContext.Current.Session(S_UserID), eStr) Then
                    objCommon.ShowAlert("Error While Saving Reference Table", Me.Page)
                    Exit Sub
                End If

            End If
            txtRemarks.Text = ""
            mdlRemarks.Hide()
            GenCall()
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Uploading the excel file"
    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        Dim fileReader As System.IO.StreamReader
        Dim filePath As String
        Dim CurLine As String = ""
        Dim Filename As String = ""
        Dim tpStr As String = ""
        Dim arStr As String()
        Dim ds_save As New DataSet
        Dim dt_save As New DataTable
        Dim dt_MedDRA_1_mdhierarchy As New DataSet
        Dim dt_low_level_term As New DataSet
        Dim eStr As String = ""
        Dim eStr_Retu As String = ""
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim MedDRA_1_mdhierarchy As String = ""
        Dim low_level_term As String = ""
        Dim Counter As Integer = 0
        Dim strLongFilePath As String = ""
        Dim strFileName As String
        Dim MedAsciiDir As DirectoryInfo
        Dim DirPth As String = ""
        Dim ColumnsCount As Integer = 0

        Dim dsRefTable As New DataSet
        Dim drRefTable As DataRow
        Dim wStr As String = String.Empty
        Dim extension As String = String.Empty

        Dim temp_dt As New DataTable
        Dim temp_ds As New DataSet

        Dim count As Integer = 0
        Try

            If Validation() Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "projectGrid", "bindgvwMngGlDictionary();", True)
                Exit Sub
            End If

            If Not (Convert.ToString(AdFlUpload.PostedFile.FileName) = "") Then
                strFileName = AdFlUpload.PostedFile.FileName
                filePath = AdFlUpload.PostedFile.FileName

                extension = Path.GetExtension(AdFlUpload.PostedFile.FileName)
                If Not (extension.ToString = ".asc" Or extension.ToString = ".ASC") Then
                    Me.objCommon.ShowAlert("Invalid File!", Me)
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "projectGrid", "bindgvwMngGlDictionary();", True)
                    Exit Sub
                End If

                Dim iIndex As Integer = 0
                If strFileName.Contains("\") Then
                    iIndex = strFileName.LastIndexOf("\")
                    If Not iIndex = -1 Then
                        strFileName = strFileName.Substring(iIndex + 1, strFileName.Length - iIndex - 1)
                    End If
                    filePath = strFileName
                End If

                DirPth = Server.MapPath("MedTryAscii")
                MedAsciiDir = New DirectoryInfo(DirPth)

                filePath = Server.MapPath("MedTryAscii/" + strFileName).ToString()

                If Not MedAsciiDir.Exists Then
                    MedAsciiDir.Create()
                End If

                If File.Exists(filePath) Then
                    File.Delete(filePath)
                End If

                AdFlUpload.PostedFile.SaveAs(Server.MapPath("MedTryAscii") & "\" & strFileName)

                fileReader = New System.IO.StreamReader(filePath, System.Text.Encoding.Default)
                Me.ViewState(Vs_FilePath) = strFileName
                'Dictionary()
                GetBlankTableStructure("MedDRA_1_mdhierarchy")
                dt_MedDRA_1_mdhierarchy = CType(Me.ViewState(VS_DtMedDRA), DataSet)
                MedDRA_1_mdhierarchy = Me.ViewState(VS_DtMedDRATable)

                While fileReader.Peek <> -1
                    CurLine = fileReader.ReadLine
                    tpStr = CurLine
                    arStr = tpStr.Split("$".ToCharArray())
                    If (arStr.Length() <> 13) Then
                        Me.objCommon.ShowAlert("Please Select Proper MedDRA_1_mdhierarchy ASCII File!", Me)
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "projectGrid", "bindgvwMngGlDictionary();", True)
                        Exit Sub
                    End If
                    AssignValues(dt_MedDRA_1_mdhierarchy, arStr)
                    Counter += 1

                    'If Counter > 5000 Then
                    '    Me.Save_BulkCopy(MedDRA_1_mdhierarchy, dt_MedDRA_1_mdhierarchy)
                    '    Counter = 0
                    '    dt_MedDRA_1_mdhierarchy.Clear()
                    'End If
                End While

                fileReader.Close()
                fileReader.Dispose()

                'Me.Save_BulkCopy(MedDRA_1_mdhierarchy, dt_MedDRA_1_mdhierarchy)
                'dt_MedDRA_1_mdhierarchy.Clear()
                'dt_MedDRA_1_mdhierarchy = Nothing
                'objLambda.Timeout = -1

            End If

            If Not (Convert.ToString(AdFlUpload_LLT.Value) = "") Then
                strFileName = AdFlUpload_LLT.PostedFile.FileName
                filePath = AdFlUpload_LLT.PostedFile.FileName

                extension = Path.GetExtension(AdFlUpload_LLT.PostedFile.FileName)
                If Not (extension.ToString = ".asc" Or extension.ToString = ".ASC") Then
                    Me.objCommon.ShowAlert("Invalid File!", Me)
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "projectGrid", "bindgvwMngGlDictionary();", True)

                    Exit Sub
                End If

                Dim iIndex As Integer = 0
                If strFileName.Contains("\") Then
                    iIndex = strFileName.LastIndexOf("\")
                    If Not iIndex = -1 Then
                        strFileName = strFileName.Substring(iIndex + 1, strFileName.Length - iIndex - 1)
                    End If
                    filePath = strFileName
                End If

                DirPth = Server.MapPath("MedTryAscii")
                MedAsciiDir = New DirectoryInfo(DirPth)

                filePath = Server.MapPath("MedTryAscii/" + strFileName).ToString()

                If Not MedAsciiDir.Exists Then
                    MedAsciiDir.Create()
                End If

                If File.Exists(filePath) Then
                    File.Delete(filePath)
                End If

                AdFlUpload_LLT.PostedFile.SaveAs(Server.MapPath("MedTryAscii") & "\" & strFileName)

                fileReader = New System.IO.StreamReader(filePath, System.Text.Encoding.Default)
                Me.ViewState(Vs_FilePath) = strFileName
                'Dictionary()
                GetBlankTableStructure("MedDRA_1_low_level_term")
                dt_low_level_term = CType(Me.ViewState(VS_DtMedDRA), DataSet)
                low_level_term = Me.ViewState(VS_DtMedDRATable)

                While fileReader.Peek <> -1
                    CurLine = fileReader.ReadLine
                    tpStr = CurLine
                    arStr = tpStr.Split("$".ToCharArray())
                    If (arStr.Length() <> 12) Then
                        Me.objCommon.ShowAlert("Please Select Proper MedDRA_1_low_level_term ASCII File!", Me)
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "projectGrid", "bindgvwMngGlDictionary();", True)

                        Exit Sub
                    End If
                    AssignValues(dt_low_level_term, arStr)
                    Counter += 1

                    'If Counter > 5000 Then
                    '    Me.Save_BulkCopy(low_level_term, dt_low_level_term)
                    '    Counter = 0
                    '    dt_low_level_term.Clear()
                    'End If

                End While

                fileReader.Close()
                fileReader.Dispose()

            End If

            Counter = 0
            If Not dt_MedDRA_1_mdhierarchy Is Nothing And Not dt_low_level_term Is Nothing Then

                For Each dr As DataRow In dt_MedDRA_1_mdhierarchy.Tables(0).Rows
                    Counter += 1
                    If Counter > 5000 Then
                        Me.Save_BulkCopy(MedDRA_1_mdhierarchy, temp_ds)
                        Counter = 0
                        temp_dt.Clear()
                        temp_ds.Clear()
                    End If
                    If temp_ds.Tables.Count = 0 Then
                        If Not temp_dt Is Nothing Then
                            temp_dt.Columns.Add("vMeddraVersion")
                            temp_dt.Columns.Add("pt_code")
                            temp_dt.Columns.Add("hlt_code")
                            temp_dt.Columns.Add("hlgt_code")
                            temp_dt.Columns.Add("soc_code")
                            temp_dt.Columns.Add("pt_name")
                            temp_dt.Columns.Add("hlt_name")
                            temp_dt.Columns.Add("hlgt_name")
                            temp_dt.Columns.Add("soc_name")
                            temp_dt.Columns.Add("soc_abbrev")
                            temp_dt.Columns.Add("null_field")
                            temp_dt.Columns.Add("pt_soc_code")
                            temp_dt.Columns.Add("Primary_soc_fg")
                            temp_dt.Columns.Add("nMedDRAmdihierarchyNo")
                        End If
                        temp_dt.ImportRow(dr)
                        temp_ds.Tables.Add(temp_dt)
                    Else
                        temp_ds.Tables(0).ImportRow(dr)
                    End If
                Next

                If temp_ds.Tables.Count > 0 Then
                    Me.Save_BulkCopy(MedDRA_1_mdhierarchy, temp_ds)
                    dt_MedDRA_1_mdhierarchy.Clear()
                    dt_MedDRA_1_mdhierarchy = Nothing
                    objLambda.Timeout = -1
                End If
                Counter = 0
                temp_dt.Clear()
                'temp_dt = New DataTable
                temp_ds.Tables.Clear()
                temp_dt.Columns.Clear()

                For Each dr As DataRow In dt_low_level_term.Tables(0).Rows
                    Counter += 1
                    If Counter > 5000 Then
                        Me.Save_BulkCopy(low_level_term, temp_ds)
                        Counter = 0
                        temp_dt.Clear()
                    End If

                    If temp_ds.Tables.Count = 0 Then
                        If Not temp_dt Is Nothing Then
                            temp_dt.Columns.Add("vMeddraVersion")
                            temp_dt.Columns.Add("llt_code")
                            temp_dt.Columns.Add("llt_name")
                            temp_dt.Columns.Add("pt_code")
                            temp_dt.Columns.Add("llt_whoart_code")
                            temp_dt.Columns.Add("llt_harts_code")
                            temp_dt.Columns.Add("llt_costart_sym")
                            temp_dt.Columns.Add("llt_icd9_code")
                            temp_dt.Columns.Add("llt_icd9cm_code")
                            temp_dt.Columns.Add("llt_icd10_code")
                            temp_dt.Columns.Add("llt_currency")
                            temp_dt.Columns.Add("llt_jart_code")
                            temp_dt.Columns.Add("nMedDRAllttermNo")
                        End If
                        temp_dt.ImportRow(dr)
                        temp_ds.Tables.Add(temp_dt)
                    Else
                        temp_ds.Tables(0).ImportRow(dr)
                    End If
                Next
                If temp_ds.Tables.Count > 0 Then
                    Me.Save_BulkCopy(low_level_term, temp_ds)
                    dt_low_level_term.Clear()
                    dt_low_level_term = Nothing
                    objLambda.Timeout = -1
                End If
            End If

            wStr = "1=2"
            If Not objHelp.GetReferenceTableDefinitions(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, dsRefTable, eStr_Retu) Then
                objCommon.ShowAlert("Error While Getting  Reference Table Definition.", Me.Page)
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "projectGrid", "bindgvwMngGlDictionary();", True)
                Exit Sub
            End If
            drRefTable = dsRefTable.Tables("ReferenceTableDefinitions").NewRow()
            drRefTable("vRefTableName") = txtDictionaryName.Text.Replace(" ", "_")

            drRefTable("vRefTableRemark") = txtmedDraVersion.Text.Trim()
            drRefTable("cRefTableType") = "D"
            drRefTable("cActiveFlag") = "Y"
            drRefTable("vUploadedFilePath") = Convert.ToString(DirPth)
            drRefTable("iModifyBy") = Me.Session(S_UserID)
            drRefTable("dModifyOn") = System.DateTime.Now
            drRefTable("cStatusIndi") = "N"
            drRefTable("cDictionaryType") = "AE"
            dsRefTable.Tables("ReferenceTableDefinitions").Rows.Add(drRefTable)
            dsRefTable.AcceptChanges()


            If Not objLambda.Save_ReferenceTableDefinitions(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, dsRefTable, Me.Session(S_UserID), eStr_Retu) Then
                objCommon.ShowAlert("Error While Saving Reference Table Definition.", Me.Page)
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "projectGrid", "bindgvwMngGlDictionary();", True)
                Exit Sub
            End If


            wStr = "select count(*) from " + Convert.ToString(txtDictionaryName.Text).Trim() + " where vMeddraVersion = '" + Convert.ToString(txtmedDraVersion.Text).Trim() + "' "
            count = objHelp.ExecuteQuery_Scalar(wStr)

            objCommon.ShowAlert("" + Convert.ToString(count) + " Rows Uploaded Successfully For MedDRA Version " + Convert.ToString(txtmedDraVersion.Text) + " !", Me.Page)
            txtDictionaryName.Text = ""
            txtmedDraVersion.Text = ""
            GenCall()

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "projectGrid", "bindgvwMngGlDictionary();", True)
            If Not fileReader Is Nothing Then
                fileReader.Dispose()
            End If
            ShowErrorMessage(ex.Message, "")
        End Try

    End Sub
    Protected Sub Save_BulkCopy(ByVal tablename As String, ByRef dt_OpMst As DataSet)
        objLambda.Timeout = 900000
        If Not objLambda.BulkCopy(tablename, dt_OpMst, eStr_retu) Then
            Me.ShowErrorMessage("", eStr_retu)
            Exit Sub
        End If
    End Sub

#End Region

#Region "AssignValues"
    Private Sub AssignValues(ByVal dt_OpMst As DataSet, ByVal arStr As String())
        Dim strlen As String = ""
        Dim dr As DataRow
        Dim eStr As String = ""
        Dim i As Integer = 0
        Try
            dr = dt_OpMst.Tables(0).NewRow
            For index As Integer = 1 To dt_OpMst.Tables(0).Columns.Count - 1
                dr("vMedDRAVersion") = Convert.ToString(txtmedDraVersion.Text)
                If arStr(index - 1).ToString = "" Then
                    dr(index) = DBNull.Value
                Else
                    dr(index) = arStr(index - 1)
                End If

            Next
            dt_OpMst.Tables(0).Rows.Add(dr)

        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
        End Try
    End Sub
    Private Sub AssignValuesFormedDRALog()
        Dim dr_log As DataRow

        Ds_MedDRALog = CType(Me.ViewState(Vs_dsMedDRALog), DataSet)
        Ds_MedDRALog.Clear()
        dr_log = Ds_MedDRALog.Tables(0).NewRow()
        dr_log("nmedDRAVersionNo") = "Varsion name"
        dr_log("vFileName") = "File name"
        dr_log("dUploadDate") = Now.Date
        dr_log("iModifyBy") = Me.Session(S_UserID).ToString
        Ds_MedDRALog.Tables(0).Rows.Add(dr_log)
        Ds_MedDRALog.AcceptChanges()

    End Sub
#End Region

#Region "BlankDatatable"
    Private Sub GetBlankTableStructure(ByVal TableName As String)
        Dim eStr As String = ""
        Dim ds_FillSet As New DataSet
        Dim dt_Blanktable = TableName
        Dim eStr_Retu As String = ""
        Try

            If objHelp.GetFieldsOfTable(dt_Blanktable, "*", "1=2", ds_FillSet, eStr_Retu) Then
                Me.ViewState(VS_DtMedDRATable) = dt_Blanktable
                Me.ViewState(VS_DtMedDRA) = ds_FillSet
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
        End Try
    End Sub
#End Region

#Region "Save_BulkCopy"



    'Protected Sub Save_BulkCopy(ByVal tablename As String, ByVal dsSave As DataSet)
    '    objLambda.Timeout = 900000
    '    If Not objLambda.BulkCopy(tablename, dsSave, eStr_retu) Then
    '        Me.ShowErrorMessage("", eStr_retu)
    '        Exit Sub
    '    End If

    'End Sub

#End Region

#Region "Validation"
    Private Function Validation() As Boolean
        Dim count As String = "0"
        Dim wstr As String = String.Empty

        Try
            wstr += "select count(*) from sys.tables where name = '" + (txtDictionaryName.Text.Trim).Replace(" ", "_") + "' "
            count = objHelp.ExecuteQuery_Scalar(wstr)
            If (count <> "0") Then
                Me.objCommon.ShowAlert("Dictionary Name '" + txtDictionaryName.Text.Trim + "' Allready Exist. Please enter another Dictionary Name.", Me)
                Return True
            End If

            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function
#End Region

#Region "Web Method"
    <WebMethod> _
    Public Shared Function VersionExist(ByVal Version As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wstr As String = String.Empty
        Dim count As String = "0"
        Dim msg As String = String.Empty
        Try

            count = "0"
            wstr += "Select count(vRefTableRemark) from ReferenceTableDefinitions where vRefTableRemark = '" + Version + "' "
            count = objHelp.ExecuteQuery_Scalar(wstr)
            If (count <> "0") Then
                If Convert.ToString(msg) = "" Then
                    msg = "MedDRA_1_low_level_term"
                Else
                    msg += " , MedDRA_1_low_level_term"
                End If
            End If
            If Convert.ToString(msg) <> "" Then
                msg += " in MedDRA version is all ready exist."
            End If
            Return msg
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function

    <WebMethod> _
    Public Shared Function TableNameExist(ByVal tablename As String) As String

        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wstr As String = String.Empty
        Dim count As String = "0"
        Try
            wstr += "Select count(vRefTableName) from ReferenceTableDefinitions where vRefTableName = '" + (tablename).Replace(" ", "_") + "' "
            count = objHelp.ExecuteQuery_Scalar(wstr)
            If count <> "0" Then
                Return "true"
            Else
                Return "false"
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    <WebMethod> _
    Public Shared Function AuditTrail(ByVal nRefMasterNo As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_RefMasterHistory As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtRefMasterHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Try


            wStr = " nRefMasterNo = '" + nRefMasterNo + "' Order by nRefMasterHistoryNo DESC"
            If Not objHelp.GetReferenceTableDefinitionsHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_RefMasterHistory, estr) Then
                Throw New Exception(estr)
            End If

            If Not dtRefMasterHistory Is Nothing Then
                dtRefMasterHistory.Columns.Add("SrNo")
                dtRefMasterHistory.Columns.Add("vRefTableName")
                dtRefMasterHistory.Columns.Add("cActiveFlag")
                dtRefMasterHistory.Columns.Add("vRemark")
                dtRefMasterHistory.Columns.Add("vModifyBy")
                dtRefMasterHistory.Columns.Add("dModifyOn")
            End If
            dt = ds_RefMasterHistory.Tables(0)
            Dim dv As New DataView(dt)

            For Each dr As DataRow In dv.ToTable.Rows

                drAuditTrail = dtRefMasterHistory.NewRow()

                drAuditTrail("SrNo") = i
                drAuditTrail("vRefTableName") = dr("vRefTableName").ToString()
                drAuditTrail("cActiveFlag") = dr("cActiveFlag").ToString()
                drAuditTrail("vRemark") = dr("vRemark").ToString()
                drAuditTrail("vModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("dModifyOn") = Convert.ToString(dr("dModifyOn"))
                dtRefMasterHistory.Rows.Add(drAuditTrail)
                dtRefMasterHistory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtRefMasterHistory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function


#End Region


End Class
