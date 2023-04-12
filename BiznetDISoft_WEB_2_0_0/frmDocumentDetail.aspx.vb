Imports System.IO
Partial Class frmDocumentDetail
    Inherits System.Web.UI.Page
    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtWSNHistory As String = "DtWorkspacenodehistory"
    Private Const VS_WSSDDetailId As String = "WorkspaceSubjectDocDetailId"
    Private Const VS_WorkspaceId As String = "vWorkSpaceNo"
    Private Const VS_NId As String = "NodeId"
    Private Const VS_TNo As String = "TranId"
    Private Const VS_TempTNo As String = "TempTranId"
    Private Const VS_CheckOut As String = "Dt_CheckOut"
    Private Const VS_LockFlag As String = "LockFlag"
    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
    Private ObjPath As New ClsFolderPath

    'Private Const VS_Path As String = "Path"
    'Private Const VS_File As String = "FileName"

#Region "Load Event"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then

                Me.ViewState(VS_DtWSNHistory) = Nothing
                Me.ViewState(VS_Choice) = Nothing
                Me.ViewState(VS_WSSDDetailId) = Nothing
                Me.ViewState(VS_WorkspaceId) = Nothing
                Me.ViewState(VS_NId) = Nothing
                Me.ViewState(VS_TNo) = Nothing
                Me.ViewState(VS_TempTNo) = Nothing
                Me.ViewState(VS_CheckOut) = Nothing
                Me.ViewState(VS_LockFlag) = Nothing

                GenCall()
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region " GenCall() "

    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim WorkSpaceId As String = ""
        Dim NodeId As String = ""
        Try

            Me.Session.Remove("Path")

            Choice = Me.Request.QueryString("Mode")
            WorkSpaceId = Me.Request.QueryString("WorkSpaceId")
            NodeId = Me.Request.QueryString("NodeId")
            Me.ViewState(VS_WorkspaceId) = WorkSpaceId
            Me.ViewState(VS_NId) = NodeId
            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            'Check Point
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_WSSDDetailId) = Me.Request.QueryString("Value").ToString
            End If
            'Check Point
            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If
            'Check Point
            Me.ViewState(VS_DtWSNHistory) = ds.Tables("Workspacenodehistory")   ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")

        Finally

        End Try

    End Function
#End Region

#Region " GenCall_Data "
    Private Function GenCall_Data(ByRef ds_Retu As DataSet) As Boolean

        Dim wStr As String = ""
        Dim eStr_Retu As String = ""
        Dim Val As String = ""
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing

        Try

            Val = Me.ViewState(VS_WSSDDetailId) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vWorkSpaceId=" + Val.ToString
            End If


            If Not objHelp.getWorkspaceNodeHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            If ds.Tables(0).Rows.Count <= 0 And _
             Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found for Selected Operation")
            End If

            ds_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")

        Finally

        End Try
    End Function
#End Region

#Region " GenCall_showUI() "
    Private Function GenCall_ShowUI() As Boolean
        Dim dt_UserMst As DataSet = Nothing

        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = " :: Document Details  ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Document Detail"

            If Not FillControl() Then
                Return False
            End If
            If Not FillGrid() Then 'Fill Grid
                Return False
            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function
#End Region

#Region "FillControl"
    Private Function FillControl() As Boolean
        Dim ds_Project As New Data.DataSet
        Dim ds_ActivitySuBDetail As New Data.DataSet
        Dim ds_Status As New Data.DataSet
        Dim estr As String = ""
        Dim WorkSpaceId As String = ""
        Dim NodeId As String = ""
        Dim DistParm(1) As String
        'Dim Create As Boolean = False
        Try
            WorkSpaceId = Me.ViewState(VS_WorkspaceId)
            NodeId = Me.ViewState(VS_NId)

            objHelp.FillDropDown("workspaceprotocoldetail", "vProjectNo", "vWorkSpaceId", "vWorkSpaceId='" & WorkSpaceId & "'", ds_Project, estr)
            objHelp.GetViewProjectActivityCurrAttr("vWorkSpaceID='" & WorkSpaceId & "' and iNodeId=" & NodeId, ds_ActivitySuBDetail, estr)
            objHelp.GetViewProjectNodeUserRightsDetails("vWorkSpaceID='" & WorkSpaceId & "' and iNodeId=" & NodeId & " AND iUserId=" & Me.Session(S_UserID), WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Status, estr)

            Me.txtProjNo.Text = ds_Project.Tables(0).Rows(0).Item("vProjectNo")
            Me.txtActivityName.Text = ds_ActivitySuBDetail.Tables(0).Rows(0).Item("vActivityName")
            Me.txtProjNo.Enabled = False
            Me.txtActivityName.Enabled = False
            Me.txtDocStatus.Enabled = False

            Me.Label1.Text = "<FONT size=""1""><a href=""" & Replace(ds_ActivitySuBDetail.Tables(0).Rows(0).Item("vDocPath"), "/", "\") & """ target=""_blank"">" & Path.GetFileName(ds_ActivitySuBDetail.Tables(0).Rows(0).Item("vDocPath")) & "</a></font>"

            Me.LnkDoc.Text = Path.GetFileName(ds_ActivitySuBDetail.Tables(0).Rows(0).Item("vDocPath"))
            Me.LnkDoc.CommandName = ds_ActivitySuBDetail.Tables(0).Rows(0).Item("vDocPath")
            Me.LnkDoc.Visible = False

            Me.txtDocStatus.Text = ds_ActivitySuBDetail.Tables(0).Rows(0).Item("vAttr99StageDesc")
            Me.HdfBaseFolder.Value = ds_ActivitySuBDetail.Tables(0).Rows(0).Item("vBaseWorkFolder").ToString.Trim() + "/"

            'For i = 0 To ds_Status.Tables(0).Rows.Count - 1 'For Hidding FileUpload
            '    If ds_Status.Tables(0).Rows(i).Item("iStageId") = 1 Then
            '        Create = True
            '        Exit For
            '    End If
            'Next

            'If Create = True Then
            '    'Me.TdUpload.Visible = True
            '    'Me.TdUploadLbl.Visible = True
            '    'Me.TdUpload.Disabled = True
            '    'Me.TdUploadLbl.Disabled = True
            'Else
            '    'Me.TdUpload.Visible = False
            '    'Me.TdUploadLbl.Visible = False
            '    'Me.TdUpload.Disabled = True
            '    'Me.TdUploadLbl.Disabled = True
            '    '
            'End If '***************************************
            DistParm = "istageid, vStageDesc".Split(",")
            Me.DDLStatus.DataSource = ds_Status.Tables(0).DefaultView.ToTable(True, DistParm)
            Me.DDLStatus.DataValueField = "iStageId"
            Me.DDLStatus.DataTextField = "vStageDesc"
            Me.DDLStatus.DataBind()
            Me.DDLStatus.Items.Insert(0, New ListItem("Select Status", "0"))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function
    Private Function FillGrid() As Boolean
        Dim ds_Grid As New Data.DataSet
        Dim dv_Grid As New Data.DataView
        Dim estr As String = ""
        Dim WorkSpaceId As String = ""
        Dim NodeId As String = ""
        Try
            WorkSpaceId = Me.ViewState(VS_WorkspaceId)
            NodeId = Me.ViewState(VS_NId)

            objHelp.GetDataForCommentGrid("vWorkSpaceID='" & WorkSpaceId & "' and iNodeId=" & NodeId, ds_Grid, estr)
            dv_Grid = New Data.DataView
            dv_Grid = ds_Grid.Tables(0).DefaultView
            dv_Grid.RowFilter = "cRequiredFlag='C'"
            Me.GV_Comment.DataSource = dv_Grid.ToTable()
            Me.GV_Comment.DataBind()

            dv_Grid = New Data.DataView
            dv_Grid = ds_Grid.Tables(0).DefaultView
            dv_Grid.RowFilter = "cRequiredFlag='D'"
            Me.Gv_DocHistory.DataSource = dv_Grid.ToTable()
            Me.Gv_DocHistory.DataBind()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function
#End Region

    Private Function NewTranNo(ByVal type As String) As Boolean
        Dim WsId As String = ""
        Dim NId As String = ""
        Dim TNo As String = ""
        Dim Ds_Tran As New DataSet
        Dim estr As String = ""
        Dim Wstr As String = ""
        Try
            WsId = Me.ViewState(VS_WorkspaceId)
            NId = Me.ViewState(VS_NId)
            Me.ViewState(VS_TNo) = Nothing
            If type.ToUpper = "DOC" Or type.ToUpper = "TEMP" Then
                Wstr = "vWorkSpaceId='" & WsId + "' and iNodeId= " & NId & " and cRequiredFlag='D'"
            ElseIf type.ToUpper = "COM" Then
                Wstr = "vWorkSpaceId='" & WsId + "' and iNodeId= " & NId & " and cRequiredFlag='C'"
            End If
            If Not objHelp.GetFieldsOfTable("Workspacenodehistory", "isNull(Max(iTranNo+1),1) as TranNo", Wstr, Ds_Tran, estr) Then
                Return False
            End If
            TNo = Ds_Tran.Tables(0).Rows(0).Item("TranNo")

            Me.ViewState(VS_TNo) = TNo
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


#Region "Create New Folder"
    Private Function CreateFolder(ByVal type As String) As Boolean
        Dim WsId As String = ""
        Dim DCId As String = ""
        Dim SubId As String = ""
        Dim NId As String = ""
        Dim TNo As String = ""
        Dim ActId As String = ""
        Dim DocDetail As String = ""
        Dim dir As DirectoryInfo
        Dim Ds_Tran As New DataSet
        Dim estr As String = ""
        Try
            WsId = Me.ViewState(VS_WorkspaceId)
            NId = Me.ViewState(VS_NId)


            If type.ToUpper = "TEMP" Then
                DocDetail = "TempDocDetail/" 'System.Configuration.ConfigurationManager.AppSettings("BaseWorkFolder")   
            Else
                DocDetail = Me.HdfBaseFolder.Value.Trim() 'System.Configuration.ConfigurationManager.AppSettings("BaseWorkFolder")
            End If
            TNo = Me.ViewState(VS_TNo)
            'For New TranNo

            Me.ViewState(VS_NId) = NId

            Me.HdfFolder.Value = ""
            Me.HdfFolder.Value = WsId & "/" & NId & "/" & TNo

            dir = New DirectoryInfo(ObjPath.Param_FolderPath(DocDetail & WsId & "/" & NId & "/" & TNo))
            If Not dir.Exists() Then
                dir.Create()
            End If

            'If type.ToUpper = "DOC" Then 'Check Point for delete Folder
            '    'dir = New DirectoryInfo(ObjPath.Param_FolderPath("TempDocDetail/" & WsId & "/" & NId & "/" & TNo))
            '    ''dir = New DirectoryInfo(ObjPath.Param_FolderPath(Me.HdfBaseFolder.Value.Trim() & WsId & "/" & NId & "/" & TNo))
            '    'If dir.Exists() Then
            '    '    dir.Delete(True)
            '    'End If
            'End If
            'Me.Session("Path") = SubjectDetail & WsId & "/" & DCId & "/" & SubId & "/" & NId & "/" & TNo & "/"
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
#End Region

#Region "UpLoad File"
    Private Function Upload(ByVal Type As String) As Boolean
        Dim iFileSize As Integer
        'Dim DocPath As String = System.Configuration.ConfigurationManager.AppSettings("DocPath")
        Dim DocPath As String = "" 'Me.Session("Path")
        'Dim DocFileSize As Integer = System.Configuration.ConfigurationManager.AppSettings("Size")
        Dim DocValidFile As String = ""
        Dim NewFilePath As String = ""

        DocValidFile = System.Configuration.ConfigurationManager.AppSettings("Validity")
        Dim strValidFile() As String
        Dim iCounter As Integer
        Dim bl As Boolean = False
        Dim dir As DirectoryInfo
        Try
            'If Not IsNothing(Me.Session("FileSize")) Then
            '    Me.Session("FileSize") = Nothing
            'End If



            'DocPath = Me.ViewState(VS_Path).ToString.Trim()


            If Not IsNothing(Me.Session("FileName")) Then
                Me.Session("FileName") = Nothing
            End If
            If Type.ToUpper = "TEMPDOC" Then 'To Upload Document
                If Not NewTranNo("TEMP") Then
                    ObjCommon.ShowAlert("Error Occured while Getting Tran No., Try Again... ", Me)
                    Return False
                End If

                If Not CreateFolder("TEMP") Then
                    ObjCommon.ShowAlert("Error Occured while Creating Folder, Try Again... ", Me)
                    'Exit Function
                    Return False
                End If

                DocPath = "TempDocDetail/" + Me.HdfFolder.Value.Trim() + "/"

                If Not IsNothing(Me.FlUpload) Then

                    iFileSize = Me.FlUpload.PostedFile.ContentLength

                    If iFileSize < 1048576 Then 'Validity of Size

                        strValidFile = DocValidFile.Split("#")
                        For iCounter = 0 To UBound(strValidFile)
                            If strValidFile(iCounter) = Path.GetExtension(Me.FlUpload.PostedFile.FileName) Then
                                bl = True
                                Exit For
                            End If
                        Next

                        If Not bl = True Then 'Validity of Document
                            ObjCommon.ShowAlert("Invalid File,Only PDF & text File is allowed", Me)
                            'Exit Function
                            Return False
                        End If

                        'Me.Session("FileSize") = iFileSize.ToString + "KB"
                        'Me.Session("FileName") = FlUpload.PostedFile.FileName
                        Dim str As String = Replace(Me.FlUpload.PostedFile.FileName, "\", "\\")

                        Me.FlUpload.PostedFile.SaveAs(Server.MapPath(DocPath) & Path.GetFileName(Me.FlUpload.PostedFile.FileName))

                        Me.HdfFileName.Value = ""
                        Me.HdfFileName.Value = Path.GetFileName(Me.FlUpload.PostedFile.FileName)

                    Else
                        ObjCommon.ShowAlert("Error Occured, File size should be less than 1 MB...", Me)
                        Return False
                    End If
                End If
            ElseIf Type.ToUpper = "DOC" Then 'To Upload Document
                If Not NewTranNo("DOC") Then
                    ObjCommon.ShowAlert("Error Occured while Getting Tran No., Try Again... ", Me)
                    Return False
                End If
                If Not CreateFolder("DOC") Then
                    ObjCommon.ShowAlert("Error Occured while Creating Folder, Try Again... ", Me)
                    'Exit Function
                    Return False
                End If

                DocPath = Me.HdfBaseFolder.Value.Trim() + Me.HdfFolder.Value.Trim() + "/"
                If Not File.Exists(Server.MapPath(DocPath + Me.HdfFileName.Value.ToString.Trim())) Then
                    File.Move(Server.MapPath("TempDocDetail/" + Me.HdfFolder.Value.Trim() + "/" + Me.HdfFileName.Value.ToString.Trim()), Server.MapPath(DocPath + Me.HdfFileName.Value.ToString.Trim()))
                End If


                'To Delete TempFolder of NodeId
                dir = New DirectoryInfo(Server.MapPath("TempDocDetail/" + Me.ViewState(VS_WorkspaceId) + "/" + Me.ViewState(VS_NId) + "/" + Me.ViewState(VS_TempTNo)))
                If dir.Exists() Then
                    dir.Delete(True)
                End If '*********

            ElseIf Type.ToUpper = "COM" Then 'To Upload Document for Comment
                If Not NewTranNo("COM") Then
                    ObjCommon.ShowAlert("Error Occured while Getting Tran No., Try Again... ", Me)
                    Return False
                End If
                If Not CreateFolder("COM") Then
                    ObjCommon.ShowAlert("Error Occured while Creating Folder, Try Again... ", Me)
                    'Exit Function
                    Return False
                End If

                DocPath = Me.HdfBaseFolder.Value.Trim() + Me.HdfFolder.Value.Trim() + "/"

                If Not IsNothing(Me.FlUploadCom) Then

                    iFileSize = Me.FlUploadCom.PostedFile.ContentLength

                    If iFileSize < 1048576 Then 'Validity of Size

                        strValidFile = DocValidFile.Split("#")
                        For iCounter = 0 To UBound(strValidFile)
                            If strValidFile(iCounter) = Path.GetExtension(Me.FlUploadCom.PostedFile.FileName) Then
                                bl = True
                                Exit For
                            End If
                        Next

                        If Not bl = True Then 'Validity of Document
                            ObjCommon.ShowAlert("Invalid File,Only PDF & text File is allowed", Me)
                            'Exit Function
                            Return False
                        End If


                        Dim str As String = Replace(Me.FlUploadCom.PostedFile.FileName, "\", "\\")

                        Me.FlUploadCom.PostedFile.SaveAs(Server.MapPath(DocPath) & Path.GetFileName(Me.FlUploadCom.PostedFile.FileName))

                        Me.HdfFileName.Value = ""
                        Me.HdfFileName.Value = Path.GetFileName(Me.FlUploadCom.PostedFile.FileName)

                    Else
                        ObjCommon.ShowAlert("Error Occured, File size should be less than 1 MB...", Me)
                    End If
                End If
            Else 'For Other then Created
                If Not NewTranNo("COM") Then
                    ObjCommon.ShowAlert("Error Occured while Getting Tran No., Try Again... ", Me)
                    Return False
                End If
                If Not CreateFolder("COM") Then
                    ObjCommon.ShowAlert("Error Occured while Creating Folder, Try Again... ", Me)
                    'Exit Function
                    Return False
                End If
                DocPath = Me.HdfBaseFolder.Value.Trim() + Me.HdfFolder.Value.Trim() + "/"
                If Me.LnkDoc.Text <> "" Then

                    Me.FlUploadCom.PostedFile.SaveAs(Server.MapPath(DocPath) & Me.LnkDoc.Text.Trim())

                    Me.HdfFileName.Value = ""
                    Me.HdfFileName.Value = Me.LnkDoc.Text.Trim()

                Else
                    ObjCommon.ShowAlert("Error Occured, File size should be less than 1 MB...", Me)
                End If

            End If
            Return True
        Catch ex As Exception
            ObjCommon.ShowAlert("Error Occured while Uploading File, Try Again... ", Me)
            Return False
        End Try

    End Function
#End Region

#Region "Assign values"
    Private Function AssignValues(ByVal Type As String) As Boolean
        Dim dr As DataRow
        Dim dt_Workspacenodehistory As New DataTable
        Try

            dt_Workspacenodehistory = CType(Me.ViewState(VS_DtWSNHistory), DataTable)
            'dt_Workspacenodehistory.Clear()
            'vWorkSpaceId,iNodeId,iTranNo,vFileName,vFileType,vFolderName,cRequiredFlag,vDefaultFileFormat,vRemark,iStageId,iModifyBy
            If Type.ToUpper = "DOC" Then
                dr = dt_Workspacenodehistory.NewRow()
                dr("vWorkSpaceId") = Me.ViewState(VS_WorkspaceId)
                dr("iNodeId") = Me.ViewState(VS_NId)
                dr("iTranNo") = Me.ViewState(VS_TNo)
                dr("vFileName") = Me.HdfFileName.Value.ToString.Trim()
                dr("vFileType") = Path.GetExtension(Me.HdfFileName.Value.ToString.Trim())
                dr("vFolderName") = Me.HdfFolder.Value.ToString.Trim() 'Me.ViewState(VS_Path) 'Me.Session("Path").ToString.Trim()
                dr("cRequiredFlag") = "D"
                dr("vDefaultFileFormat") = ""
                dr("vRemark") = Me.txtRemarks.Text.Trim()
                dr("iStageId") = Me.DDLStatus.SelectedValue.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dt_Workspacenodehistory.Rows.Add(dr)
            ElseIf Type.ToUpper = "COM" Then
                dr = dt_Workspacenodehistory.NewRow()
                dr("vWorkSpaceId") = Me.ViewState(VS_WorkspaceId)
                dr("iNodeId") = Me.ViewState(VS_NId)
                dr("iTranNo") = Me.ViewState(VS_TNo)
                dr("vFileName") = Me.HdfFileName.Value.ToString.Trim()
                dr("vFileType") = Path.GetExtension(Me.HdfFileName.Value.ToString.Trim())
                dr("vFolderName") = Me.HdfFolder.Value.ToString.Trim() 'Me.ViewState(VS_Path) 'Me.Session("Path").ToString.Trim()
                dr("cRequiredFlag") = "C"
                dr("vDefaultFileFormat") = ""
                dr("vRemark") = Me.txtComments.Text.Trim()
                dr("iStageId") = Me.DDLStatus.SelectedValue.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dt_Workspacenodehistory.Rows.Add(dr)
            Else
                dr = dt_Workspacenodehistory.NewRow()
                dr("vWorkSpaceId") = Me.ViewState(VS_WorkspaceId)
                dr("iNodeId") = Me.ViewState(VS_NId)
                dr("iTranNo") = Me.ViewState(VS_TNo)
                dr("vFileName") = Me.HdfFileName.Value.ToString.Trim()
                dr("vFileType") = Path.GetExtension(Me.HdfFileName.Value.ToString.Trim())
                dr("vFolderName") = Me.HdfFolder.Value.ToString.Trim() 'Me.ViewState(VS_Path) 'Me.Session("Path").ToString.Trim()
                dr("cRequiredFlag") = "D"
                dr("vDefaultFileFormat") = ""
                dr("vRemark") = "N/A"
                dr("iStageId") = Me.DDLStatus.SelectedValue.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dt_Workspacenodehistory.Rows.Add(dr)
            End If

            Me.ViewState(VS_DtWSNHistory) = dt_Workspacenodehistory
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function
#End Region

#Region "Button Click"
    Protected Sub LnkDoc_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strtype As String
        Dim strfname As String
        Dim strFileIndex As String
        Dim strFinal As String

        strfname = Me.LnkDoc.Text.Trim() 'CType(ViewState("Fname"), String)

        strFileIndex = strfname.LastIndexOf("/")
        strfname = Mid(strfname, 2)


        'strFinal = HttpContext.Current.Server.MapPath("Attachment/" + strfname)
        strFinal = HttpContext.Current.Server.MapPath(Me.LnkDoc.CommandName.Trim())
        strFileIndex = strfname.LastIndexOf(".")
        strtype = Mid(strfname, strFileIndex + 1)

        HttpContext.Current.Response.Buffer = True
        HttpContext.Current.Response.ClearContent()
        HttpContext.Current.Response.ClearHeaders()

        If strtype = ".doc" Then
            HttpContext.Current.Response.AddHeader("Content-type", "application/vnd.ms-doc")
        ElseIf strtype = ".pdf" Then
            HttpContext.Current.Response.AddHeader("Content-type", "application/vnd.ms-pdf")
        End If


        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + strfname)

        Dim MyFileStream As FileStream
        Dim FileSize As Long
        MyFileStream = New FileStream(strFinal, FileMode.Open)
        FileSize = MyFileStream.Length
        Dim Buffer(CInt(FileSize)) As Byte
        MyFileStream.Read(Buffer, 0, CInt(FileSize))
        MyFileStream.Close()

        HttpContext.Current.Response.BinaryWrite(Buffer)
        HttpContext.Current.Response.Flush()
        HttpContext.Current.Response.Close()
    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim ds_Save As New DataSet
        Dim dt_Save As New DataTable
        Dim estr As String = ""
        Try
            If Me.DDLStatus.SelectedValue > 1 Then
                If Not Upload("NotCreate") Then 'To create Folder and Upload File
                    Exit Sub
                End If
                If Not AssignValues("NotCreate") Then
                    Exit Sub
                End If
            End If
            'If Me.TdUpload.Disabled = True Then
            '    If Not Upload("NotCreate") Then 'To create Folder and Upload File
            '        Exit Sub
            '    End If
            '    If Not AssignValues("NotCreate") Then
            '        Exit Sub
            '    End If
            'Else
            '    If Me.FlUpload.PostedFile.FileName <> "" Then
            '        If Not Upload("Doc") Then 'To create Folder and Upload File
            '            Exit Sub
            '        End If
            '        If Not AssignValues("Doc") Then
            '            Exit Sub
            '        End If
            '    Else
            '        If Not Upload("NotCreate") Then 'To create Folder and Upload File
            '            Exit Sub
            '        End If
            '        If Not AssignValues("NotCreate") Then
            '            Exit Sub
            '        End If
            '    End If
            'End If

            If Not IsNothing(Me.FlUpload) Then
                If Me.FlUploadCom.PostedFile.FileName <> "" Then
                    If Not Upload("Com") Then 'To create Folder and Upload File
                        Exit Sub
                    End If
                Else
                    If Not NewTranNo("COM") Then
                        ObjCommon.ShowAlert("Error Occured while Getting Tran No., Try Again... ", Me)
                    End If
                End If
                If Not AssignValues("Com") Then
                    Exit Sub
                End If
            End If
            ds_Save = New DataSet
            dt_Save = CType(Me.ViewState(VS_DtWSNHistory), DataTable)
            dt_Save.TableName = "Workspacenodehistory"
            ds_Save.Tables.Add(dt_Save)
            If Not objLambda.Save_InsertWorkspaceNodeHistory(Me.ViewState(VS_Choice), ds_Save, Me.Session(S_UserID), estr) Then
                ObjCommon.ShowAlert("Error While Saving Workspacenodehistory", Me.Page)
                Exit Sub
            Else
                ObjCommon.ShowAlert("Record Saved SuccessFully", Me.Page)
                ResetPage()
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        'ResetPage()
        Me.Response.Redirect(Me.Request.QueryString("Page2").Trim() & ".aspx?WorkSpaceId=" & Me.Request.QueryString("WorkSpaceId").Trim() & "&Page=" & Me.Request.QueryString("Page").Trim())
        'Me.Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

#Region "Reset Page"
    Private Sub ResetPage()
        'Me.DDLStatus.SelectedIndex = 0
        'Me.ViewState(VS_DtWSNHistory) = Nothing
        'Me.ViewState(VS_Choice) = Nothing
        'Me.ViewState(VS_WSSDDetailId) = Nothing
        'Me.ViewState(VS_WorkspaceId) = Nothing
        'Me.ViewState(VS_NId) = Nothing
        'Me.ViewState(VS_TNo) = Nothing
        'Me.ViewState(VS_TempTNo) = Nothing
        'Me.ViewState(VS_CheckOut) = Nothing
        'Me.ViewState(VS_LockFlag) = Nothing
        Me.Response.Redirect("frmDocumentDetail.aspx?Mode=1&WorkSpaceId=" & Me.ViewState(VS_WorkspaceId) & "&NodeId=" & Me.ViewState(VS_NId) & "&Mode=1" & "&Page=" & Me.Request.QueryString("Page") & "&Page2=" & Me.Request.QueryString("Page2"))
        'GenCall()
    End Sub
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

#Region " Selected Change "
    Protected Sub DDLStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds_CheckOut As New DataSet
        Dim ds_Locked As New DataSet
        Dim dt_CheckOut As New DataTable
        Dim estr As String = ""
        Try
            If Me.DDLStatus.SelectedValue = 1 Then 'Hard-Coded For Created Stage
                Me.Div_Lock.Visible = True

                If Not Me.objHelp.GetFieldsOfTable("checkedoutfiledetail", "iModifyBY,isnull(count(*),0) as Locked ", "vWorkSpaceID='" & Me.ViewState(VS_WorkspaceId) & "' and iNodeId=" & Me.ViewState(VS_NId) & " and itranNo=(select Max(iTranNo) from checkedoutfiledetail where vWorkSpaceId='" & Me.ViewState(VS_WorkspaceId) & "' and iNodeId=" & Me.ViewState(VS_NId) & ") and  isNodeLocked='Y' group By iModifyBY ", ds_Locked, estr) Then
                    ObjCommon.ShowAlert("Error Occured While Checking ", Me)
                Else
                    If ds_Locked.Tables(0).Rows.Count > 0 Then
                        'Checking if Locked By own
                        If ds_Locked.Tables(0).Rows(0).Item("Locked") > 0 And ds_Locked.Tables(0).Rows(0).Item("iModifyBY") <> Me.Session(S_UserID) Then
                            ObjCommon.ShowAlert("This Node is Locked By Another User, Try Again... ", Me)
                        Else
                            Me.BtnLockNode.Enabled = False
                            Me.Panel1.Visible = True
                            Me.tdGeneral.Disabled = True
                        End If
                        '***********
                    Else
                        Me.ViewState(VS_LockFlag) = "N"
                    End If
                End If
            Else
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub
#End Region

#Region "Grid Event"
    Protected Sub GV_Comment_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("LblSrNo"), Label).Text = e.Row.RowIndex + 1
            CType(e.Row.FindControl("hlnkFile"), HyperLink).NavigateUrl = Me.HdfBaseFolder.Value & e.Row.Cells(5).Text.Trim() & _
                                                                        "/" & CType(e.Row.FindControl("hlnkFile"), HyperLink).Text.Trim()

            'CType(e.Row.Cells(, BoundField).DataFormatString = "{0:dd-MMM-yyyy}"
        End If
    End Sub

    Protected Sub GV_Comment_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        e.Row.Cells(4).Visible = False
        e.Row.Cells(6).Visible = False
    End Sub
#End Region

#Region "Buttons for Lock"

    Protected Sub BtnLockNode_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds_CheckOut As New DataSet
        Dim ds_Locked As New DataSet
        Dim dt_CheckOut As New DataTable
        Dim estr As String = ""
        Try
            If Me.ViewState(VS_LockFlag) = "N" Then
                If Not LockUnlock("Lock") Then
                    ObjCommon.ShowAlert("Error Occured While Checking", Me)
                    Exit Sub
                Else
                    ObjCommon.ShowAlert("Node is Locked Now", Me)
                    Me.BtnLockNode.Enabled = False
                    Me.Panel1.Visible = True
                End If
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub BtnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Div_Lock.Visible = False
        Me.DDLStatus.SelectedIndex = 0
    End Sub

    Protected Sub BtnUpLoad1_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Not Upload("TEMPDOC") Then 'To create Folder and Upload File
                Exit Sub
            Else
                Me.Panel1.Visible = False
                ObjCommon.ShowAlert("Uploaded Successfully. Now to save Unlock the Node", Me.Page)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub BtnUnLock_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds_Save As New DataSet
        Dim dt_Save As New DataTable
        Dim estr As String = ""
        Try
            If Not Upload("DOC") Then 'To create Folder and Upload File
                Exit Sub
            End If

            If Not LockUnlock("UNLOCK") Then
                ObjCommon.ShowAlert("Error Occured While Checking ", Me)
                Exit Sub
            Else
            End If
            If Not AssignValues("Doc") Then
                Exit Sub
            End If
            ds_Save = New DataSet
            dt_Save = CType(Me.ViewState(VS_DtWSNHistory), DataTable)
            dt_Save.TableName = "Workspacenodehistory"
            ds_Save.Tables.Add(dt_Save)
            If Not objLambda.Save_InsertWorkspaceNodeHistory(Me.ViewState(VS_Choice), ds_Save, Me.Session(S_UserID), estr) Then
                ObjCommon.ShowAlert("Error While Saving Workspacenodehistory.", Me.Page)
                Exit Sub
            Else
                ObjCommon.ShowAlert("Record Saved SuccessFully and Node is Now Unlocked.", Me.Page)
                Me.BtnLockNode.Enabled = True
                Me.Panel1.Visible = False
                Me.Div_Lock.Visible = False
                Me.DDLStatus.SelectedIndex = 0
                ResetPage()
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub BtnUnlockWS_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dir As DirectoryInfo
        dir = New DirectoryInfo(Server.MapPath("TempDocDetail/" + Me.ViewState(VS_WorkspaceId) + "/" + Me.ViewState(VS_NId) + "/" + Me.ViewState(VS_TempTNo)))
        If dir.Exists() Then
            dir.Delete(True)
        End If

        If Not LockUnlock("UNLOCKWS") Then
            ObjCommon.ShowAlert("Error Occured While Checking ", Me)
            Exit Sub
        Else
            ObjCommon.ShowAlert("Node is Unlocked Without Save", Me)
            Me.BtnLockNode.Enabled = True
            Me.Panel1.Visible = False
            Me.Div_Lock.Visible = False
            Me.DDLStatus.SelectedIndex = 0
        End If
    End Sub

    Private Function LockUnlock(ByVal Type As String) As Boolean
        Dim ds_CheckOut As New DataSet
        Dim ds_CheckOut_Blank As New DataSet
        Dim ds_Locked As New DataSet
        Dim dt_CheckOut As New DataTable
        Dim dr As DataRow
        Dim estr As String = ""

        Try
            If Me.ViewState(VS_CheckOut) Is Nothing Then
                If Not Me.objHelp.GetCheckedoutfiledetail("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CheckOut_Blank, estr) Then
                    ObjCommon.ShowAlert("Error Occured While Creating Blank Structure, Try Again... ", Me)
                    Return False
                End If
                Me.ViewState(VS_CheckOut) = ds_CheckOut_Blank
            End If

            ds_CheckOut = Me.ViewState(VS_CheckOut)
            ds_CheckOut.Tables(0).Clear()
            dr = ds_CheckOut.Tables(0).NewRow()
            'vWorkSpaceId,iNodeId,iTranNo,iPrevTranNo,vFileName,iModifyBy,dModifyOn,isNodeLocked,cStatusIndi
            dr("vWorkSpaceId") = Me.ViewState(VS_WorkspaceId)
            dr("iNodeId") = Me.ViewState(VS_NId)
            dr("iTranNo") = "00"
            dr("iPrevTranNo") = "00"
            If Type.ToUpper = "LOCK" Then
                dr("vFileName") = " "
                dr("isNodeLocked") = "Y"
            ElseIf Type.ToUpper = "UNLOCK" Then
                dr("vFileName") = Me.HdfFileName.Value
                dr("isNodeLocked") = "N"
            ElseIf Type.ToUpper = "UNLOCKWS" Then
                dr("vFileName") = " "
                dr("isNodeLocked") = "N"
            End If

            dr("iModifyBy") = Me.Session(S_UserID)
            dr("cStatusIndi") = "N"
            ds_CheckOut.Tables(0).Rows.Add(dr)
            ds_CheckOut.Tables(0).AcceptChanges()

            If Not Me.objLambda.Save_Insertcheckedoutfiledetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_CheckOut, Me.Session(S_UserID), estr) Then
                ObjCommon.ShowAlert("Error Occured while Locking", Me)
                Return False
            Else
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "Grid for Lock"
    Protected Sub Gv_DocHistory_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("LblDocSrNo"), Label).Text = e.Row.RowIndex + 1
            CType(e.Row.FindControl("hlnkDocFile"), HyperLink).NavigateUrl = Me.HdfBaseFolder.Value & e.Row.Cells(6).Text.Trim() & _
                                                                        "/" & CType(e.Row.FindControl("hlnkDocFile"), HyperLink).Text.Trim()
            'CType(e.Row.Cells(, BoundField).DataFormatString = "{0:dd-MMM-yyyy}"
        End If
    End Sub

    Protected Sub Gv_DocHistory_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        e.Row.Cells(4).Visible = False
        e.Row.Cells(6).Visible = False
    End Sub

#End Region

End Class

