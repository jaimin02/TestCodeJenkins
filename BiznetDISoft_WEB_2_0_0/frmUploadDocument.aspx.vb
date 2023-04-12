Imports System.Web.Services
Imports Newtonsoft.Json

Partial Class frmUploadDocument
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

    Private Choice As WS_Lambda.DataObjOpenSaveModeEnum
    Private MasterEntry As WS_Lambda.MasterEntriesEnum
    Private estr_Retu As String = ""
    Public abc As String = String.Empty

    Private Const SrNo As Integer = 0
    Private Const Activity As Integer = 1
    Private Const Period As Integer = 2
    Private Const UserName As Integer = 3
    Private Const Upload As Integer = 4
    Private Const Audit As Integer = 5

    Private Const VS_Activity As String = "Activity"
    Private Const VS_ActivityGrid As String = "ActivityGrid"
    Private Const VS_FilePath As String = "FilePath"
    Private Const VS_CRFUploadGuidelineDtl As String = "CRFUploadGuideLineDtl"
    Dim k As Integer = 0
    Private Const S_Activity As String = "Activity"
    Private Const cFilePath As String = "CRFUploadDoc"
    Private Const S_projectInfo As String = "S_projectInfo"

#End Region

#Region "Load Event"
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If Not GenCall() Then
                    Throw New Exception("Error occured While calling GenCall()")
                    Exit Try
                End If
            End If
        Catch ex As Exception
            Me.objCommon.ShowAlert("Error while Page_Load()", Me.Page)
        End Try
    End Sub
#End Region

#Region "GenCall"
    Private Function GenCall() As Boolean
        Dim ds_save As New DataSet
        'Dim Mode As String = String.Empty
        Me.hdnMode.Value = Me.Request.QueryString("mode")
        Try
            If Not GenCall_Data(ds_save, "1=2") Then
                GenCall = False
                Exit Function
            End If
            'Mode = Me.Request.QueryString("mode")

            If Not GenCall_ShowUI() Then
                GenCall = False
                Exit Function
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideSponsorDetails", "HideSponsorDetails(); ", True)
            End If
            GenCall = True
        Catch ex As Exception
            Me.objCommon.ShowAlert("Error while GenCall()", Me.Page)
            GenCall = False
        End Try
    End Function
#End Region

#Region "GenCall_Data"
    Private Function GenCall_Data(ByRef ds_save As DataSet, ByVal wstr As String) As Boolean
        Dim estr As String = String.Empty
        Dim DataRetrievalMode As String = String.Empty
        Dim Mode = Me.Request.QueryString("mode")
        Try

            If Not objHelp.GetCRFUploadGuidelineDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_save, estr) Then
                Me.objCommon.ShowAlert("Error while getting data from GetCRFUploadGuidelineDetail", Me.Page)
            End If
            If ds_save.Tables(0).Columns.Contains("TempFilePath") Then
                ds_save.Tables(0).Columns.Add("TempFilePath")
            End If
            GenCall_Data = True
        Catch ex As Exception
            Me.objCommon.ShowAlert("Error while GenCall_Data()", Me.Page)
            GenCall_Data = False
        End Try
    End Function
#End Region

#Region "GenCall_ShowUI"
    Private Function GenCall_ShowUI() As Boolean
        Dim Mode As String = String.Empty
        Try
            Mode = Me.Request.QueryString("mode")
            Page.Title = " :: Upload Guidelines ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            If Mode = 1 Then
                CType(Me.Master.FindControl("lblHeading"), Label).Text = "Upload Training Guidelines"
                'rbtSpecific.Items(1).Selected = True
                'trRbtnlst.Visible = False
                trRbtnlst.Style.Add("display", "none")
                tdPeriod.Style.Add("display", "none")
                ddlPeriod.Style.Add("display", "none")
            Else
                CType(Me.Master.FindControl("lblHeading"), Label).Text = "Upload Guidelines"
            End If

            If Not bindGrid() Then
                Throw New Exception("Error while bind grid")
            End If
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
            GenCall_ShowUI = True
        Catch ex As Exception
            Me.objCommon.ShowAlert("Error while GenCall_ShowUI()", Me.Page)
            GenCall_ShowUI = False
        End Try
    End Function
#End Region

#Region "Button Events"

    Protected Sub btnDeleteFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteFile.Click
        Dim Type As String = String.Empty
        Dim ActivityId As String = String.Empty
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds_save As New DataSet
        Dim dt As New DataTable
        Dim FilePath As String = String.Empty
        Dim FPath As String = String.Empty
        Dim Rpath() As String
        Dim Str As String = String.Empty

        Try
            wstr = "vWorkspaceId=" + Me.HProjectId.Value.Trim() + "  and cStatusIndi <>'D' and cUploadType='P' "
            If Not GenCall_Data(ds_save, wstr) Then
                Exit Sub
            End If
            If ds_save.Tables(0).Rows.Count > 0 Then
                For Each dr As DataRow In ds_save.Tables(0).Rows
                    FilePath = dr("vFilePath")
                    Rpath = FilePath.Split("/")
                    FilePath = Server.MapPath(cFilePath) + "/" + Rpath(2) + "/" + Rpath(3) + "/" + Rpath(4)
                    Dim file As New FileInfo(FilePath)
                    If file.Exists Then
                        file.Delete()
                    End If
                Next

                btnDeleteFile.Attributes.Add("Status", "D")

            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "projectGrid", "projectGrid();", True)
        Catch ex As Exception
            'Throw New Exception("Error while btnDeleteFile_Click()")
        End Try
    End Sub

    Protected Sub btnCancle_Click(sender As Object, e As EventArgs) Handles btnCancle.Click
        If Not ResetPage() Then
            Throw New Exception("Error while ResetPage()")
        End If
    End Sub

    Protected Sub btnActivityFileUpload_Click(sender As Object, e As EventArgs) Handles btnActivityFileUpload.Click
        Try
            If Not SaveUploadedFile() Then
                Throw New Exception("Error while SaveUploadedFile()")
            End If
        Catch ex As Exception
            Me.objCommon.ShowAlert("Error while btnActivityFileUpload_Click()", Me.Page)
        End Try
    End Sub

    Protected Sub btnSetProject_Click(sender As Object, e As EventArgs) Handles btnSetProject.Click
        Dim status As String = String.Empty
        Dim mode As String = String.Empty
        mode = Me.Request.QueryString("mode")
        Try
            If Not Me.FillActivityDropdown() Then
                Me.objCommon.ShowAlert("Error while FillActivityDropdown()", Me.Page)
            End If
            status = getLockStatus()
            If mode = 0 Then

                If rbtSpecific.SelectedItem.Text = "Activity Specific" Then
                    If status = "L" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "AlertLocActivity", "AlertLocActivity();", True)
                    End If
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getSpecification", "getSpecification();", True)
                End If
                If rbtSpecific.SelectedItem.Text = "Project Specific" Then
                    '   ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ClearGrid", "ClearGrid();", True)
                    '   ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "RemoveCss", "RemoveCss();", True)
                    If status = "L" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "LockStatusforProject", "LockStatusforProject();", True)
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getSpecification", "getSpecification();", True)
                    End If
                End If
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getSpecification_new", "getSpecification_new();", True)
            End If
        Catch ex As Exception
            Me.objCommon.ShowAlert("Error while btnSetProject_Click()", Me.Page)
        End Try
    End Sub

    Protected Sub btnUpload_Click(sender As Object, e As EventArgs)
        Try
            If Not SaveUploadedFile() Then
                Throw New Exception("Error while SaveUploadedFile()")
            End If
        Catch ex As Exception
            Me.objCommon.ShowAlert("Error while SaveUploadedFile()", Me.Page)
        End Try
    End Sub
    Protected Sub btnRemarksUpdate_Click(sender As Object, e As EventArgs) Handles btnRemarksUpdate.Click
        Try
            If Not SaveUploadedFile() Then
                Throw New Exception("Error while SaveUploadedFile()")
            End If
        Catch ex As Exception
            Me.objCommon.ShowAlert("Error while SaveUploadedFile()", Me.Page)
        End Try

    End Sub

    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub
#End Region

#Region "DropDown Event"
    Protected Sub ddlPeriod_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPeriod.SelectedIndexChanged
        Try
            If rbtSpecific.SelectedItem.Text = "Activity Specific" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ClearGrid", "ClearGrid();", True)
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "RemoveCss", "RemoveCss();", True)
            End If
            If rbtSpecific.SelectedItem.Text = "Project Specific" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ClearGrid", "ClearGrid();", True)
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "RemoveCss", "RemoveCss();", True)
                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getSpecification", "getSpecification();", True)
            End If
        Catch ex As Exception
            Throw New Exception("Error while ddlPeriod_SelectedIndexChanged()")
        End Try
    End Sub
#End Region

#Region "Other Function"

    Private Function SaveUploadedFile() As Boolean
        Dim ds_save As New DataSet
        Dim Choice_1 As DataObjOpenSaveModeEnum
        Dim ShowAlter As Boolean = True
        Dim Mode As String = String.Empty
        Mode = Me.Request.QueryString("mode")
        Try
            ds_save = AssignValuetoCRFUploadDtl()
            'If Mode = 1 Then
            If Not ds_save Is Nothing Then
                If (Mode = 1) Then
                    For Each dr As DataRow In ds_save.Tables(0).Rows
                        If dr("cStatusIndi") = "N" Then
                            Choice_1 = DataObjOpenSaveModeEnum.DataObjOpenMode_Add
                        End If
                        If dr("cStatusIndi") = "E" Or dr("cStatusIndi") = "D" Then
                            Choice_1 = DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                        End If
                    Next
                    If Not ds_save Is Nothing Then
                        If Not Me.objLambda.Save_CRFUPLOADGUIDELINEDTL(Choice_1, ds_save, Me.Session(S_UserID).ToString, estr_Retu) Then
                            Me.objCommon.ShowAlert("Error occured while saving in Save_CRFUPLOADGUIDELINEDTL", Me)
                            Exit Function
                        End If
                        For Each dr As DataRow In ds_save.Tables(0).Rows
                            If dr("cStatusIndi") = "D" Then
                                ShowAlter = False
                            End If
                        Next
                        If ShowAlter = True Then
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SuccessAlert", "SuccessAlert();", True)
                        End If
                    End If
                    If rbtSpecific.SelectedItem.Text = "Activity Specific" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getSpecification", "getSpecification();", True)
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "populateGrid", "populateGrid();", True)
                    End If
                Else


                    For Each dr As DataRow In ds_save.Tables(0).Rows
                        If dr("cStatusIndi") = "N" Then
                            Choice_1 = DataObjOpenSaveModeEnum.DataObjOpenMode_Add
                        End If
                        If dr("cStatusIndi") = "E" Or dr("cStatusIndi") = "D" Then
                            Choice_1 = DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                        End If
                    Next
                    If Not ds_save Is Nothing Then
                        If Not Me.objLambda.Save_CRFUPLOADGUIDELINEDTL(Choice_1, ds_save, Me.Session(S_UserID).ToString, estr_Retu) Then
                            Me.objCommon.ShowAlert("Error occured while saving in Save_CRFUPLOADGUIDELINEDTL", Me)
                            Exit Function
                        End If
                        For Each dr As DataRow In ds_save.Tables(0).Rows
                            If dr("cStatusIndi") = "D" Then
                                ShowAlter = False
                            End If
                        Next
                        If ShowAlter = True Then
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SuccessAlert", "SuccessAlert();", True)
                        End If
                    End If
                    If rbtSpecific.SelectedItem.Text = "Activity Specific" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getSpecification", "getSpecification();", True)
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "populateGrid", "populateGrid();", True)
                    End If
                End If
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getSpecification", "getSpecification();", True)
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "LockStatusforProject", "LockStatusforProject();", True)
            End If
            'End If
            Return True
        Catch ex As Exception
            Me.objCommon.ShowAlert("Error while SaveUploadedFile()", Me.Page)
            Return False
        End Try
    End Function

    Private Function CheckActivityExixstorNot() As String
        Dim Type As String = String.Empty
        Dim ActivityId As String = String.Empty
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds_save As New DataSet
        Dim dt As New DataTable
        Try
            ActivityId = hdnActivityName.Value.Trim()
            wstr = "vActivityId =" + ActivityId.ToString() + " and vWorkspaceId=" + Me.HProjectId.Value.Trim() + " and iNodeId=" + hndNodeID.Value.Trim() + ""
            If Not GenCall_Data(ds_save, wstr) Then
                Return Nothing
                Exit Function
            End If
            dt = ds_save.Tables(0)
            dt.DefaultView.RowFilter = "vActivityId = '" + ActivityId.ToString() + "' and iPeriod = " + ddlPeriod.SelectedItem.Text + " and vWorkspaceId=" + Me.HProjectId.Value.Trim() + " and iNodeId = " + hndNodeID.Value.Trim() + " "
            If dt.DefaultView.ToTable.Rows.Count > 0 Then
                If Not DeleteFileToDisk(dt.DefaultView.ToTable) Then
                    Throw New Exception("Error while DeleteFileToDisk()")
                End If
                Type = "Edit"
            Else
                Type = "Add"
            End If
            Return Type
        Catch ex As Exception

        End Try
    End Function

    Private Function CheckProjectExixstorNot() As String
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim type As String = String.Empty
        Dim ds_save As New DataSet
        Dim dt As New DataTable
        Dim DataRetrievalMode As String = String.Empty
        Dim Mode As String = String.Empty
        Try
            Mode = Me.Request.QueryString("mode")
            If (Mode = 1) Then
                wstr = "vWorkspaceId =" + Me.HProjectId.Value.Trim() + " and cDocType=  'T' and cUploadType=  'P' and iPeriod= " + ddlPeriod.SelectedItem.Text + " and cStatusIndi <> 'D' "
            Else
                wstr = "vWorkspaceId =" + Me.HProjectId.Value.Trim() + " and cUploadType=  'P' and iPeriod= " + ddlPeriod.SelectedItem.Text + " and cStatusIndi <> 'D' "
            End If
            If Not GenCall_Data(ds_save, wstr) Then
                Return Nothing
                Exit Function
            End If
            dt = ds_save.Tables(0)
            dt.DefaultView.RowFilter = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
            If dt.DefaultView.ToTable.Rows.Count > 0 Then
                If Not DeleteFileToDisk(dt.DefaultView.ToTable) Then
                    Throw New Exception("Error while DeleteFileToDisk()")
                End If
                type = "Edit"
            Else
                type = "Add"
            End If
            Return type
        Catch ex As Exception
            Me.objCommon.ShowAlert("Error while CheckProjectExixstorNot()", Me.Page)
        End Try
    End Function

    Private Function DeleteFileToDisk(ByVal Dt_Save As DataTable) As Boolean

        Dim dt As New DataTable
        Dim ActivityId As String = String.Empty
        Dim FilePath As String = String.Empty
        Dim FolderPath As String = Nothing
        Dim Rpath() As String = Nothing
        Dim wstr As String = String.Empty
        Dim Mode As String = String.Empty
        Try
            Mode = Me.Request.QueryString("mode")
            If Mode = 1 Then
                dt = Dt_Save
                dt = Dt_Save.Copy
                dt.DefaultView.RowFilter = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' and cUploadType= 'P' and cDocType= 'T' and iPeriod=" + ddlPeriod.SelectedItem.Text + ""
            Else
                If rbtSpecific.SelectedItem.Text = "Project Specific" Then

                    dt = Dt_Save
                    dt = Dt_Save.Copy
                    dt.DefaultView.RowFilter = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' and cUploadType= 'P' and iPeriod=" + ddlPeriod.SelectedItem.Text + ""
                End If

                If rbtSpecific.SelectedItem.Text = "Activity Specific" Then
                    ActivityId = hdnActivityName.Value.Trim()
                    dt = Dt_Save
                    dt = Dt_Save.Copy
                    dt.DefaultView.RowFilter = "vActivityId =" + ActivityId.ToString() + " and iPeriod= " + ddlPeriod.SelectedItem.Text + " and iNodeId= " + hndNodeID.Value.Trim() + " "
                End If
            End If
            

            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    FilePath = dr("vFilePath")
                    If FilePath <> "" Then
                        Rpath = FilePath.Split("/")
                        If (Mode = 1) Then
                            FilePath = Server.MapPath(cFilePath) + "/" + Rpath(2) + "/" + Rpath(3) + "/" + Rpath(4)
                        Else
                            If rbtSpecific.SelectedItem.Text = "Project Specific" Then
                                FilePath = Server.MapPath(cFilePath) + "/" + Rpath(2) + "/" + Rpath(3) + "/" + Rpath(4)
                            End If
                            If rbtSpecific.SelectedItem.Text = "Activity Specific" Then
                                FilePath = Server.MapPath(cFilePath) + "/" + Rpath(2) + "/" + Rpath(3) + "/" + Rpath(4) + "/" + Rpath(5) + "/" + Rpath(6)
                            End If
                        End If
                        
                    End If
                Next
                'for delete file
                If FilePath <> "" Then
                    Dim file As New FileInfo(FilePath)
                    If file.Exists Then
                        file.Delete()
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub SaveFileToDisk(ByRef filePath As String, ByVal maxSerialNumber As Integer)
        Dim tmpFilePath As String = Server.MapPath(cFilePath)
        Dim dirProj As DirectoryInfo = Nothing
        Dim dirActi As DirectoryInfo = Nothing
        Dim fileName As String = ""
        Dim strDate As String = ""
        Dim sDate() As String = Nothing
        Dim sTime() As String = Nothing
        Dim sPath() As String = Nothing
        Dim CurrentDate As String = Nothing
        Dim CurrentTime As String = Nothing
        Dim fString As String = ""
        Dim ProjectPath As String = String.Empty
        Dim pPath As String = String.Empty
        Dim ActivityPath As String = String.Empty
        Dim dtActivity As New DataTable
        Dim ActivityId As String = String.Empty
        Dim TempProjectName As String = Nothing
        Dim ProjectName() As String = Nothing
        Dim PName As String = String.Empty
        Dim tempFilePath() As String = Nothing
        Dim p() As String = Nothing
        Dim FileLength As Integer
        Dim NodeId As Integer = 0
        Dim Mode As String = Me.Request.QueryString("mode")
        Dim TrainingProjectPath As String = String.Empty
        Try

            TempProjectName = txtProject.Text.ToString()
            If TempProjectName.Contains("/") = "True" Then
                ProjectName = TempProjectName.Split("/")
                PName = ProjectName(0) + "_" + ProjectName(1) + "_" + ProjectName(2) + "_" + ProjectName(3)
                p = ProjectName(0).Split(" ")
                PName = p(0)
            ElseIf TempProjectName.Contains("[") = "True" Then
                ProjectName = TempProjectName.Split("[")
                p = ProjectName(1).Split("]")
                PName = p(0)
            End If

            ProjectPath = tmpFilePath + "\" + PName.ToString()

            dirProj = New DirectoryInfo(ProjectPath)
            If Not dirProj.Exists Then
                dirProj.Create()
            End If
            If (Mode = 1) Then
                ProjectPath = ProjectPath + "\" + "T"
                dirProj = New DirectoryInfo(ProjectPath)
                If Not dirProj.Exists Then
                    dirProj.Create()
                End If

                fileName = Path.GetFileName(flCRFUploadDoc.PostedFile.FileName)

                FileLength = Convert.ToInt32(fileName.Length)
            Else
                If rbtSpecific.SelectedItem.Text = "Project Specific" Then

                    ProjectPath = ProjectPath + "\" + ddlPeriod.SelectedItem.Text
                    dirProj = New DirectoryInfo(ProjectPath)
                    If Not dirProj.Exists Then
                        dirProj.Create()
                    End If

                    fileName = Path.GetFileName(flCRFUploadDoc.PostedFile.FileName)

                    FileLength = Convert.ToInt32(fileName.Length)
                End If

                If rbtSpecific.SelectedItem.Text = "Activity Specific" Then

                    ActivityId = hdnActivityName.Value.Trim()
                    NodeId = hndNodeID.Value.Trim()

                    ActivityPath = ProjectPath + "\" + ActivityId.ToString() + "\" + NodeId.ToString() + "\" + ddlPeriod.SelectedItem.Text

                    dirProj = New DirectoryInfo(ActivityPath)
                    If Not dirProj.Exists Then
                        dirProj.Create()
                    End If

                    FileLength = Convert.ToInt32(fileName.Length)

                    fileName = Path.GetFileName(fuActivityFile.PostedFile.FileName)
                End If
            End If
            

            fileName = Path.GetFileNameWithoutExtension(fileName) + maxSerialNumber.ToString() + Path.GetExtension(fileName)
            fileName = fileName.Replace(" ", "_")
            ''tempFilePath = fileName.Split(".")
            ''fileName = tempFilePath(0) + "." + tempFilePath(1)

            If (Mode = 1) Then
                TrainingProjectPath = ProjectPath + "\" + fileName
            Else
                If rbtSpecific.SelectedItem.Text = "Project Specific" Then
                    ProjectPath = ProjectPath + "\" + fileName
                End If

                If rbtSpecific.SelectedItem.Text = "Activity Specific" Then
                    ActivityPath = ActivityPath + "\" + fileName
                End If
            End If


            filePath = filePath + "\" + fileName

            If (Mode = 1) Then
                flCRFUploadDoc.SaveAs(TrainingProjectPath)
                filePath = TrainingProjectPath
            Else
                If rbtSpecific.SelectedItem.Text = "Project Specific" Then
                    flCRFUploadDoc.SaveAs(ProjectPath)
                    filePath = ProjectPath
                End If

                If rbtSpecific.SelectedItem.Text = "Activity Specific" Then
                    fuActivityFile.SaveAs(ActivityPath)
                    filePath = ActivityPath
                End If
            End If

            If (Mode = 1) Then
                filePath = "/" + cFilePath + "/" + PName.ToString() + "/" + "T" + "/" + fileName
            Else
                If rbtSpecific.SelectedItem.Text = "Project Specific" Then
                    filePath = "/" + cFilePath + "/" + PName.ToString() + "/" + ddlPeriod.SelectedItem.Text + "/" + fileName
                End If

                If rbtSpecific.SelectedItem.Text = "Activity Specific" Then
                    filePath = "/" + cFilePath + "/" + PName.ToString() + "/" + hdnActivityName.Value.Trim() + "/" + hndNodeID.Value.Trim() + "/" + ddlPeriod.SelectedItem.Text + "/" + fileName
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function AssignValuetoCRFUploadDtl() As DataSet
        Dim dtCRFUploadGuidLine As New DataTable
        Dim drCRFUpload As DataRow
        Dim filePath As String = ""
        Dim maxSrNumber As Integer = 1
        Dim maxNumber As Integer = 1
        Dim DS_Save As New DataSet
        Dim type As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim fPath() As String = Nothing
        Dim ProjectLockStatus As String = String.Empty
        Dim estr As String = String.Empty
        Dim dv_Check As New DataView
        Dim LockStatus As String = "U"
        Dim Mode As String = String.Empty
        Try
            Mode = Me.Request.QueryString("mode")
            If Mode = 1 Then
                type = CheckProjectExixstorNot()
            Else
                If rbtSpecific.SelectedItem.Text = "Project Specific" Then
                    type = CheckProjectExixstorNot()
                End If
                If rbtSpecific.SelectedItem.Text = "Activity Specific" Then
                    type = CheckActivityExixstorNot()
                End If
            End If
            
            'Logic for add file
            If type = "Add" Then
                If Not GenCall_Data(DS_Save, "1=2") Then
                    Return Nothing
                    Exit Function
                End If
                SaveFileToDisk(filePath, maxSrNumber)
                drCRFUpload = DS_Save.Tables(0).NewRow()
                drCRFUpload("vWorkspaceId") = Me.HProjectId.Value.Trim()
                drCRFUpload("vProjectNo") = ""

                If (rbtSpecific.SelectedItem.Text = "Project Specific" And Mode = "1") Then
                    drCRFUpload("cUploadType") = "P"
                    drCRFUpload("iPeriod") = ddlPeriod.SelectedItem.Text
                    'drCRFUpload("cDocType") = "T"
                    'drCRFUpload("vTrainingFinished") = "No"
                Else
                    drCRFUpload("cUploadType") = "P"
                    drCRFUpload("iPeriod") = ddlPeriod.SelectedItem.Text
                    'drCRFUpload("cDocType") = "G"
                    'drCRFUpload("vTrainingFinished") = " "
                End If

                If rbtSpecific.SelectedItem.Text = "Activity Specific" Then
                    drCRFUpload("cUploadType") = "A"
                    drCRFUpload("iPeriod") = ddlPeriod.SelectedItem.Text
                    drCRFUpload("vActivityId") = hdnActivityName.Value.Trim()
                    drCRFUpload("iNodeId") = hndNodeID.Value.Trim()
                End If
                drCRFUpload("vFilePath") = filePath
                drCRFUpload("iModifyBy") = Me.Session(S_UserID)
                drCRFUpload("cStatusIndi") = "N"

                DS_Save.Tables(0).Rows.Add(drCRFUpload)
                DS_Save.Tables(0).AcceptChanges()
            End If

            'Logic for edit file
            If type = "Edit" Then
                If Mode = 1 Then
                    Wstr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' and cUploadType=  'P'  and cDocType=  'T' and cStatusIndi <> 'D' and iPeriod= " + ddlPeriod.SelectedItem.Text + ""
                Else
                    If rbtSpecific.SelectedItem.Text = "Project Specific" Then
                        Wstr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' and cUploadType=  'P' and cStatusIndi <> 'D' and iPeriod= " + ddlPeriod.SelectedItem.Text + ""
                    End If

                    If rbtSpecific.SelectedItem.Text = "Activity Specific" Then
                        Wstr = "vActivityId =" + hdnActivityName.Value.Trim() + "and iPeriod = " + ddlPeriod.SelectedItem.Text + " and iNodeId=" + hndNodeID.Value.Trim() + ""
                    End If
                End If
                

                If Not GenCall_Data(DS_Save, Wstr) Then
                    Return Nothing
                    Exit Function
                End If
                SaveFileToDisk(filePath, maxSrNumber)
                For Each dr As DataRow In DS_Save.Tables(0).Rows
                    dr("vFilePath") = filePath
                    dr("cStatusIndi") = "E"
                    dr("vRemark") = Convert.ToString(txtRemarks.Text).Trim()
                    dr("iModifyBy") = Me.Session(S_UserID)
                Next
            End If
            Me.Session(S_projectInfo) = DS_Save

            Return DS_Save
        Catch ex As Exception

        End Try
    End Function

    Private Function FillActivityDropdown() As Boolean
        Dim wstr_AllActivities As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds_AllActivities As New DataSet
        Dim dv_AllActivities As New DataView
        Dim Periods As Integer = 1
        Dim ProjectType As String = String.Empty
        Dim dsProjects As New DataSet
        Dim Wstr_Scope As String = String.Empty
        Dim ds_ProjectType As New DataSet
        Try

            Me.ddlPeriod.Items.Clear()

            wstr_AllActivities = "vWorkSpaceId=" & Me.HProjectId.Value.Trim()

            '  Me.Session("WorkSpaceId") = Me.HProjectId.Value.Trim()

            If Not objHelp.GetViewWorkSpaceNodeDetail(wstr_AllActivities, ds_AllActivities, estr) Then
                Me.objCommon.ShowAlert("Error while getting data from workspacenodedetail", Me.Page)
            End If

            'If Not objHelp.view_workspaceprotocoldetail(wstr_AllActivities, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsProjects, estr) Then
            '    Return False
            'End If

            'Wstr_Scope = "vProjectTypeCode = " + dsProjects.Tables(0).Rows(0)("vProjectTypeCode").ToString()

            'If Not objHelp.GetviewProjectTypeMst(Wstr_Scope, _
            '                WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ProjectType, estr) Then
            '    Return False
            'End If

            'If ds_ProjectType.Tables(0).Rows.Count > 0 Then
            '    If ds_ProjectType.Tables(0).Rows(0)("vProjectTypeName") = "BA-BE" Then
            '        hndProjectStatus.Value = "BABE"
            '    End If
            '    If ds_ProjectType.Tables(0).DefaultView.ToTable.Rows(0)("vProjectTypeName") = "Clinical Trial" Then
            '        hndProjectStatus.Value = "CT"
            '    End If
            'End If

            If ds_AllActivities.Tables(0).Rows.Count > 0 Then
                Periods = ds_AllActivities.Tables(0).DefaultView.ToTable(True, "iPeriod").Rows.Count
                For count As Integer = 0 To Periods - 1
                    Me.ddlPeriod.Items.Add((count + 1).ToString)
                Next count
                Me.Session(S_Activity) = ds_AllActivities.Tables(0)
            End If

            FillActivityDropdown = True
        Catch ex As Exception
            Me.objCommon.ShowAlert("Error while FillActivityDropdown()", Me.Page)
            FillActivityDropdown = False
        End Try
    End Function

    Private Function bindGrid() As Boolean
        Dim dtActivityGrid As New DataTable
        Dim drActivity As DataRow
        Try
            If Not dtActivityGrid Is Nothing Then
                dtActivityGrid.Columns.Add("SrNo")
                dtActivityGrid.Columns.Add("Activites")
                dtActivityGrid.Columns.Add("Period")
                dtActivityGrid.Columns.Add("UserName")
                dtActivityGrid.Columns.Add("Upload")
                dtActivityGrid.Columns.Add("AudtiTrail")
            End If
            drActivity = dtActivityGrid.NewRow()
            dtActivityGrid.Rows.Add(drActivity)
            dtActivityGrid.AcceptChanges()
            gvActivityGrid.DataSource = dtActivityGrid
            gvActivityGrid.DataBind()
            Me.ViewState(VS_Activity) = dtActivityGrid
            Return True
        Catch ex As Exception
            Me.objCommon.ShowAlert("Error while bindGrid()", Me.Page)
            Return False
        End Try
    End Function

    Private Function ResetPage() As Boolean
        Try
            Me.txtProject.Text = ""
            Me.ddlPeriod.Items.Clear()
            Me.Session(S_projectInfo) = Nothing
            Me.Session("S_ActivityGrid") = Nothing
            Me.Session("Status") = Nothing
            Me.Session("WorkSpaceId") = Nothing
            Return True
        Catch ex As Exception
            Me.objCommon.ShowAlert("Error while ResetPage()", Me.Page)
            Return False
        End Try
    End Function
#End Region

#Region "Web Method"
    <WebMethod> _
    Public Shared Function FillProjectGrid(ByVal Period As String, ByVal WorkspaceID As String, ByVal cDocType As String) As String
        Dim strReturn As String = String.Empty
        Dim ds_Project As New DataSet
        Dim dtProjectGrid As New DataTable
        Dim drProject As DataRow
        Dim i As Integer = 1
        Dim wStr As String = String.Empty
        Dim ds_save As DataSet
        Dim estr As String = String.Empty
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim dtData As New DataTable
        Dim dt As New DataTable
        Dim ds_CheckStatus As DataSet
        Dim dv_Check As DataView
        Dim LockStatus As String = "U"
        Try
            If Not dtProjectGrid Is Nothing Then
                dtProjectGrid.Columns.Add("SrNo")
                dtProjectGrid.Columns.Add("ProjectNo")
                dtProjectGrid.Columns.Add("FileName")
                dtProjectGrid.Columns.Add("UpdatedBy")
                dtProjectGrid.Columns.Add("UploadedOn")
                dtProjectGrid.Columns.Add("AuditTrail")
                dtProjectGrid.Columns.Add("LockStatus")
                dtProjectGrid.Columns.Add("StatusIndi")
            End If

            wStr = "vWorkspaceId = " + WorkspaceID + " and iPeriod = " + Period + " and cUploadType= 'P'"

            If Not objHelp.GetViewCRFUploadGuidelineDetailHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_save, estr) Then
                Throw New Exception("Error while GetCRFUploadGuidelineDetail()")
            End If

            dt = ds_save.Tables(0)

            Dim dv As New DataView(dt)
            If Not ds_save Is Nothing And ds_save.Tables(0).Rows.Count > 0 Then
                dt = ds_save.Tables(0)
                dv.Sort = "dUploadedOn DESC"
            End If

            If dv.ToTable.Rows.Count > 0 Then
                wStr = "vWorkspaceId='" + WorkspaceID + "'"

                If Not objHelp.GetCRFLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                   ds_CheckStatus, estr) Then
                    Throw New Exception(estr)
                End If

                If Not ds_CheckStatus Is Nothing Then
                    dv_Check = ds_CheckStatus.Tables(0).DefaultView
                    dv_Check.Sort = "iTranNo desc"
                    ' edited by vishal for lock/unlock site
                    If dv_Check.ToTable().Rows.Count > 0 Then
                        If Convert.ToString(dv_Check.ToTable().Rows(0)("cLockFlag")).Trim.ToUpper() = "L" Then
                            LockStatus = "L"
                        End If
                    End If
                End If
            End If

            If dv.ToTable.Rows.Count > 0 Then
                drProject = dtProjectGrid.NewRow()
                drProject("SrNo") = i
                drProject("ProjectNo") = dv.ToTable.Rows(0)("vProjectNo").ToString()
                drProject("FileName") = dv.ToTable.Rows(0)("vFilePath").ToString()
                drProject("UpdatedBy") = dv.ToTable.Rows(0)("Modifyby").ToString()
                drProject("UploadedOn") = Convert.ToString(CDate(dv.ToTable.Rows(0)("dUploadedOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                drProject("AuditTrail") = ""
                drProject("LockStatus") = LockStatus
                drProject("StatusIndi") = dv.ToTable.Rows(0)("cStatusIndi").ToString()
                dtProjectGrid.Rows.Add(drProject)
                dtProjectGrid.AcceptChanges()
            End If

            strReturn = JsonConvert.SerializeObject(dtProjectGrid)

            Return strReturn
        Catch ex As Exception
            Throw New Exception("Error while FillActivityGrid")
        End Try
    End Function

    <WebMethod> _
    Public Shared Function FillActivityGrid(ByVal Period As String, ByVal WorkspaceID As String) As String
        Dim dtActivity As New DataTable
        Dim dtActivityGrid As New DataTable
        Dim drActivity As DataRow = Nothing
        Dim drTemp As DataRow = Nothing
        Dim i As Integer = 1
        Dim strReturn As String = String.Empty

        Dim wStr As String = String.Empty
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim ds_AllActivities As New DataSet
        Dim estr As String = String.Empty
        Dim dsChildGrid As New DataSet
        Dim dv As New DataView
        Dim ds_CheckStatus As DataSet
        Dim LockStatus As String = "U"
        Dim dv_Check As New DataView

        Try

            wStr = "vWorkspaceId='" + WorkspaceID + "'"
            'wStr = " AND cDocType='T'  "

            'If (rbtSpecific.SelectedItem.Text = "Project Specific" And Mode = "1") Then
            '    wStr += " AND cDocType = 'T'"
            'Else
            '    wStr += " AND cDocType = 'G'"
            'End If

            If Not objHelp.GetCRFLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                               ds_CheckStatus, estr) Then
                Throw New Exception(estr)
            End If

            If Not ds_CheckStatus Is Nothing Then
                dv_Check = ds_CheckStatus.Tables(0).DefaultView
                dv_Check.Sort = "iTranNo desc"
                ' edited by vishal for lock/unlock site
                If dv_Check.ToTable().Rows.Count > 0 Then
                    If Convert.ToString(dv_Check.ToTable().Rows(0)("cLockFlag")).Trim.ToUpper() = "L" Then
                        LockStatus = "L"
                    End If
                End If
            End If


            If Not dtActivityGrid Is Nothing Then
                dtActivityGrid.Columns.Add("Add")
                dtActivityGrid.Columns.Add("blank")
                dtActivityGrid.Columns.Add("SrNo")
                dtActivityGrid.Columns.Add("Activites")
                dtActivityGrid.Columns.Add("Period")
                'dtActivityGrid.Columns.Add("UserName")
                dtActivityGrid.Columns.Add("LockStatus")
                dtActivityGrid.Columns.Add("vActivityId")
                dtActivityGrid.Columns.Add("iNodeId")

            End If

            wStr = WorkspaceID.ToString() + "##" + Period.ToString()

            If HttpContext.Current.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                dsChildGrid = objHelp.ProcedureExecute("dbo.Proc_GetParentNodeForUpload_CT ", wStr)
            Else
                dsChildGrid = objHelp.ProcedureExecute("dbo.Proc_GetParentNodeForUpload_BABE ", wStr)
            End If

            For Each dr As DataRow In dsChildGrid.Tables(0).Rows
                If dr("ParentActivityName") <> "" And dr("vParentActivityId") <> "" Then

                    drActivity = dtActivityGrid.NewRow()
                    drActivity("Add") = ""
                    drActivity("blank") = ""
                    drActivity("SrNo") = i
                    drActivity("Activites") = dr("ParentActivityName")
                    drActivity("Period") = Period
                    ' drActivity("UserName") = HttpContext.Current.Session(S_UserName)
                    drActivity("LockStatus") = LockStatus
                    drActivity("vActivityId") = dr("vParentActivityId")
                    drActivity("iNodeId") = dr("iNodeId")
                    dtActivityGrid.Rows.Add(drActivity)
                    dtActivityGrid.AcceptChanges()
                    i += 1
                End If

            Next
            HttpContext.Current.Session("S_ActivityGrid") = dtActivityGrid
            strReturn = JsonConvert.SerializeObject(dtActivityGrid)

            Return strReturn
        Catch ex As Exception
            Throw New Exception("Error while FillActivityGrid")
        End Try
    End Function

    <WebMethod> _
    Public Shared Function CheckUploadFile(ByVal vActiviyId As String, ByVal Period As String, ByVal WorkspaceID As String, ByVal NodeId As String, ByVal cDocType As String) As String
        Dim Status As String = String.Empty
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim ds_save As New DataSet
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Try

            If (vActiviyId = "") Then
                wstr = "vWorkspaceId = " + WorkspaceID + " and cUploadType= 'p' and iPeriod = " + Period + " and cStatusIndi <> 'D'"
            Else
                wstr = "vWorkspaceId = " + WorkspaceID + " and vActivityId ='" + vActiviyId + "' and iPeriod = " + Period + " and iNodeId=" + NodeId + " and cUploadType= 'A' "
            End If
            'If (vActiviyId = "" And cDocType = "G") Then
            '    wstr = "vWorkspaceId = " + WorkspaceID + " and cUploadType = 'p' and iPeriod = " + Period + " and cStatusIndi <> 'D' and cDocType = " + cDocType + ""
            'Else
            '    wstr = "vWorkspaceId = " + WorkspaceID + " and vActivityId ='" + vActiviyId + "' and iPeriod = " + Period + " and iNodeId=" + NodeId + " and cUploadType= 'A' "
            'End If
            'If (vActiviyId = "" And cDocType = "T") Then
            '    wstr = "vWorkspaceId = " + WorkspaceID + " and cUploadType = 'p' and iPeriod = " + Period + " and cStatusIndi <> 'D' and cDocType = " + cDocType + ""
            'Else
            '    wstr = "vWorkspaceId = " + WorkspaceID + " and vActivityId ='" + vActiviyId + "' and iPeriod = " + Period + " and iNodeId=" + NodeId + " and cUploadType= 'A' "
            'End If

            If Not objHelp.GetCRFUploadGuidelineDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_save, estr) Then
                Throw New Exception("Error while GetCRFUploadGuidelineDetail()")
            End If

            If ds_save.Tables(0).Rows.Count > 0 Then
                Status = True
            Else
                HttpContext.Current.Session("Status") = "NotReplace"
                Status = False
            End If

            Return Status
        Catch ex As Exception
            Throw New Exception("Error while CheckUploadFile()")
        End Try
    End Function

    <WebMethod> _
    Public Shared Function AuditTrail(ByVal vActiviyId As String, ByVal ActivityName As String, ByVal vPeriod As String, ByVal WorkspaceID As String, ByVal NodeId As String) As String
        Dim wStr As String = String.Empty
        Dim WhereString As String = String.Empty
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim strReturn As String = String.Empty
        Dim ds_save As New DataSet
        Dim DS_Activity As New DataSet
        Dim estr As String = String.Empty
        Dim dtTempAuditTrail As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Try

            If vActiviyId = "" Then
                wStr = "vWorkspaceId = " + WorkspaceID + " and cUploadType = 'p' and iPeriod = " + vPeriod + " "
            Else
                wStr = "vWorkspaceId = " + WorkspaceID + " and vActivityId ='" + vActiviyId + "' and iPeriod = " + vPeriod + " and iNodeId = '" + NodeId + "' "
            End If

            If Not objHelp.GetViewCRFUploadGuidelineDetailHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_save, estr) Then
                Throw New Exception("Error while GetCRFUploadGuidelineDetail()")
            End If

            If Not dtTempAuditTrail Is Nothing Then
                dtTempAuditTrail.Columns.Add("SrNo")
                dtTempAuditTrail.Columns.Add("ProjectNo")
                If vActiviyId <> "" Then
                    dtTempAuditTrail.Columns.Add("ActivityName")
                End If
                dtTempAuditTrail.Columns.Add("vFilePath")
                dtTempAuditTrail.Columns.Add("vRemark")
                dtTempAuditTrail.Columns.Add("ModifyBy")
                dtTempAuditTrail.Columns.Add("UploadedOn")
            End If
            dt = ds_save.Tables(0)
            Dim dv As New DataView(dt)
            If Not ds_save Is Nothing And ds_save.Tables(0).Rows.Count > 0 Then
                dt = ds_save.Tables(0)
                '   Dim dv As New DataView(dt)          
                dv.Sort = "dUploadedOn DESC"
            End If

            For Each dr As DataRow In dv.ToTable.Rows

                drAuditTrail = dtTempAuditTrail.NewRow()

                drAuditTrail("SrNo") = i
                drAuditTrail("ProjectNo") = dr("vProjectNo")
                If vActiviyId <> "" Then
                    drAuditTrail("ActivityName") = ActivityName
                End If
                drAuditTrail("vFilePath") = dr("vFilePath")
                drAuditTrail("vRemark") = dr("vRemark")
                drAuditTrail("ModifyBy") = dr("Modifyby")
                drAuditTrail("UploadedOn") = Convert.ToString(CDate(dr("dUploadedOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)

                dtTempAuditTrail.Rows.Add(drAuditTrail)
                dtTempAuditTrail.AcceptChanges()

                i += 1
            Next

            strReturn = JsonConvert.SerializeObject(dtTempAuditTrail)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    <WebMethod> _
    Public Shared Function FillChildGrid(ByVal vActiviyId As String, ByVal ActivityName As String, ByVal Period As String, ByVal WorkspaceID As String, ByVal NodeId As String) As String
        Dim wStr As String = String.Empty
        Dim dsChildGrid As New DataSet
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim strReturn As String = String.Empty
        Dim dtchildActivity As New DataTable
        Dim i As Integer = 1
        Dim drActivity As DataRow
        Dim estr As String = String.Empty
        Dim wString As String = String.Empty
        Dim dtData As New DataTable
        Dim FilePath As String = String.Empty
        Dim FileEmpty As String = "No File Uploaded"
        Dim UserName As String = String.Empty
        Dim DSFilePath As DataSet
        Dim dPath As New DataTable
        Dim dt As New DataTable
        Dim ds_CheckStatus As DataSet
        Dim dv_Check As New DataView
        Dim LockStatus As String = "U"
        Try

            wStr = "vWorkspaceId='" + WorkspaceID + "'"

            If Not objHelp.GetCRFLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                               ds_CheckStatus, estr) Then
                Throw New Exception(estr)
            End If

            If Not ds_CheckStatus Is Nothing Then
                dv_Check = ds_CheckStatus.Tables(0).DefaultView
                dv_Check.Sort = "iTranNo desc"
                ' edited by vishal for lock/unlock site
                If dv_Check.ToTable().Rows.Count > 0 Then
                    If Convert.ToString(dv_Check.ToTable().Rows(0)("cLockFlag")).Trim.ToUpper() = "L" Then
                        LockStatus = "L"
                    End If
                End If
            End If

            If Not dtchildActivity Is Nothing Then
                dtchildActivity.Columns.Add("blank")
                dtchildActivity.Columns.Add("SrNo")
                dtchildActivity.Columns.Add("Activites")
                dtchildActivity.Columns.Add("Period")
                dtchildActivity.Columns.Add("UserName")
                dtchildActivity.Columns.Add("FileName")
                dtchildActivity.Columns.Add("Upload")
                dtchildActivity.Columns.Add("AuditTrail")
                dtchildActivity.Columns.Add("vActivityId")
                dtchildActivity.Columns.Add("LockStatus")
                dtchildActivity.Columns.Add("NodeId")
            End If

            wStr = WorkspaceID.ToString() + "##" + vActiviyId.ToString() + "##" + Period.ToString() + "##" + NodeId
            dsChildGrid = objHelp.ProcedureExecute("Proc_GetChildNodeDtl", wStr)


            If Not dsChildGrid Is Nothing And dsChildGrid.Tables(0).Rows.Count > 0 Then
                Dim dv_ChildGrid As New DataView(dsChildGrid.Tables(0))
                dv_ChildGrid.Sort = "iNodeNo"

                If Not dsChildGrid Is Nothing Then
                    For Each dr As DataRow In dv_ChildGrid.ToTable.Rows
                        dtData = dsChildGrid.Tables(0)
                        drActivity = dtchildActivity.NewRow()
                        drActivity("blank") = ""
                        drActivity("SrNo") = i
                        drActivity("Activites") = dr("vNodeDisplayName")
                        drActivity("Period") = Period
                        drActivity("FileName") = "No File Uploaded"
                        drActivity("UserName") = ""
                        drActivity("Upload") = ""
                        drActivity("AuditTrail") = ""
                        drActivity("vActivityId") = dr("vActivityId")
                        drActivity("LockStatus") = LockStatus
                        drActivity("NodeId") = dr("iNodeId")

                        dtchildActivity.Rows.Add(drActivity)
                        dtchildActivity.AcceptChanges()
                        i += 1
                    Next

                    wStr = WorkspaceID.ToString() + "##" + vActiviyId.ToString() + "##" + Period.ToString()
                    DSFilePath = objHelp.ProcedureExecute("Proc_GetUploadedFileName", wStr)
                    dPath = DSFilePath.Tables(0)

                    For Each dr As DataRow In dtchildActivity.Rows
                        For Each drFile As DataRow In dPath.Rows
                            If drFile("iNodeID").ToString() <> "" Then
                                If dr("NodeID") = drFile("iNodeID") And dr("Period") = drFile("iPeriod") Then
                                    dr("FileName") = drFile("vFilePath")
                                    dr("UserName") = drFile("Modifyby")
                                End If
                            End If
                        Next
                    Next
                End If

            End If
            strReturn = JsonConvert.SerializeObject(dtchildActivity)
            Return strReturn
        Catch ex As Exception
            Throw New Exception("Error while FillChildGrid()")
        End Try
    End Function

    <WebMethod> _
    Public Shared Function CheckProjectScope(ByVal WorkspaceID As String) As String
        Dim Status As String = String.Empty
        Try
            If HttpContext.Current.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                Status = "CT"
            End If

            If HttpContext.Current.Session(S_ScopeNo) = Scope_BABE Then
                Status = "BABE"
            End If
            Return Status
        Catch ex As Exception
            Throw New Exception("Error while CheckProjectStatus()")
        End Try
    End Function

#End Region

    Private Function getLockStatus() As String
        Dim Wstr As String = String.Empty
        Dim ds_CheckStatus As New DataSet
        Dim estr As String = String.Empty
        Dim dv_Check As New DataView
        Dim LockStatus As String = "U"
        Dim DS_Save As New DataSet
        Dim WString As String = String.Empty
        Try
            If rbtSpecific.SelectedItem.Text = "Activity Specific" Then
                Wstr = "vWorkspaceId='" + Me.HProjectId.Value.Trim() + "' and cUploadType='A'"

            Else
                Wstr = "vWorkspaceId='" + Me.HProjectId.Value.Trim() + "'  and cUploadType='P'"
            End If

            If Not GenCall_Data(DS_Save, Wstr) Then
                Return Nothing
                Exit Function
            End If

            If DS_Save.Tables(0).Rows.Count = 0 Or rbtSpecific.SelectedItem.Text = "Activity Specific" Then
                WString = "vWorkspaceId='" + Me.HProjectId.Value.Trim() + "'"
                If Not objHelp.GetCRFLockDtl(WString, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                               ds_CheckStatus, estr) Then
                    Throw New Exception(estr)
                End If

                If Not ds_CheckStatus Is Nothing Then
                    dv_Check = ds_CheckStatus.Tables(0).DefaultView
                    dv_Check.Sort = "iTranNo desc"
                    ' edited by vishal for lock/unlock site
                    If dv_Check.ToTable().Rows.Count > 0 Then
                        If Convert.ToString(dv_Check.ToTable().Rows(0)("cLockFlag")).Trim.ToUpper() = "L" Then
                            LockStatus = "L"
                        End If
                    End If
                End If

            End If
            Return LockStatus
        Catch ex As Exception

        End Try
    End Function
End Class

