Imports System.IO
Partial Class frmDocumentDetail_New
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtWSNHistory As String = "DtWorkspacenodehistory"
    Private Const VS_WSSDDetailId As String = "WorkspaceSubjectDocDetailId"
    Private Const VS_WorkspaceId As String = "vWorkSpaceNo"
    Private Const VS_NId As String = "NodeId"
    Private Const VS_TNo As String = "TranId"
    Private Const VS_TempTNo As String = "TempTranId"
    Private Const VS_CheckOut As String = "Dt_CheckOut"
    Private Const VS_LockFlag As String = "LockFlag"
    Private Const VS_QC As String = "QC"
    Private Const VS_Remark As String = "Remark"
    Private Const VS_UserVersion As String = "UserVersion"
    Private Const VS_EffectiveDate As String = "EffectiveDate"
    Private Const VS_ValidUpto As String = "dExpiryDate" 'added by vishal for valid upto date for expiry
    Private Const VS_ValidDays As String = "ValidDays"
    Private Const VS_DocTranNo As String = "DocTranNo"
    Private Const VS_ProposedDocs As String = "ProposedDocs" ' added by vishal for Proposed Docs
    Private Const VS_ValidDate As String = "VS_ValidDate" ' added by vishal for Proposed Docs
    Private Const VS_ExpiryDate As String = "VS_ExpiryDate" 'added by for Effective docs 
    Private Const VS_EffectiveDocs As String = "EffectiveDocs" ' added by vishal for Effective Docs
    Private Const VS_NodeAndTranNo As String = "NodeAndTranNo" 'added by vishal
    Private Const VS_Gv_grid1 As String = "grid1" 'added by vishal


    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private ObjPath As New ClsFolderPath
#End Region

#Region "Load Event"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GenCall()
        End If

    End Sub
#End Region

#Region " GenCall() "

    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim WorkSpaceId As String = ""
        Dim NodeId As String = ""
        Try

            Choice = Me.Request.QueryString("Mode")
            WorkSpaceId = Me.Request.QueryString("WorkSpaceId")
            NodeId = Me.Request.QueryString("NodeId")
            Me.ViewState(VS_WorkspaceId) = WorkSpaceId
            Me.ViewState(VS_NId) = NodeId
            Me.ViewState(VS_Choice) = Choice   'To use it while saving
            Me.ViewState(VS_QC) = Me.Request.QueryString("QC")

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
            Page.Title = " :: Document Detail ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Document Detail"

            If Not FillControl() Then
                Return False
            End If
            If Not FillGrid() Then 'Fill Grid
                Return False
            End If

            Me.tdCommEntry.Visible = False
            Me.RBLQCAuth.Visible = False
            Me.TdQC.Visible = False

            If Me.ViewState(VS_QC).ToString.ToUpper.Trim() = "Y" Then
                Me.RBLQCAuth.Visible = True
                Me.TdQC.Visible = True
                Me.tdCommEntry.Visible = True
                Me.tdComment.Visible = False
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
        Dim ds_DocCreated As New Data.DataSet
        Dim ds_DocPublished As New Data.DataSet 'added by vishal
        Dim ds_DocApprove As New Data.DataSet
        Dim ds_Reference As New Data.DataSet
        Dim estr As String = ""
        Dim WorkSpaceId As String = ""
        Dim NodeId As String = ""
        Dim DistParm(1) As String
        Dim wstrDocCreated As String = ""
        Dim wstrDocPublished As String = ""
        Dim wstrDocApprove As String = ""

        Try

            WorkSpaceId = Me.ViewState(VS_WorkspaceId)
            NodeId = Me.ViewState(VS_NId)

            objHelp.FillDropDown("workspaceprotocoldetail", "vProjectNo", "vWorkSpaceId", "vWorkSpaceId='" & WorkSpaceId & "'", ds_Project, estr)
            objHelp.GetViewProjectActivityCurrAttr("vWorkSpaceID='" & WorkSpaceId & "' and iNodeId=" & NodeId, ds_ActivitySuBDetail, estr)
            objHelp.GetViewProjectNodeUserRightsDetails("vWorkSpaceID='" & WorkSpaceId & "' and iNodeId=" & NodeId & " AND iUserId=" & Me.Session(S_UserID), _
                                                WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Status, estr)

            Me.txtProjNo.Text = ds_Project.Tables(0).Rows(0).Item("vProjectNo")
            Me.txtActivityName.Text = ds_ActivitySuBDetail.Tables(0).Rows(0).Item("vActivityName")
            Me.txtDocStatus.Text = ds_ActivitySuBDetail.Tables(0).Rows(0).Item("vAttr99StageDesc")
            Me.HdfBaseFolder.Value = ds_ActivitySuBDetail.Tables(0).Rows(0).Item("vBaseWorkFolder").ToString.Trim() + "/"
            Me.txtProjNo.Enabled = False
            Me.txtActivityName.Enabled = False
            Me.txtDocStatus.Enabled = False


            '---Add By Satyam on 4th april 2009-----
            'Me.lblDocCreated.Text = "<FONT size=""1""><a href=""" & Replace(ds_ActivitySuBDetail.Tables(0).Rows(0).Item("vDocPath"), "/", "\") & """ target=""_blank"">" & Path.GetFileName(ds_ActivitySuBDetail.Tables(0).Rows(0).Item("vDocPath")) & "</a></font>"
            'Me.LnkDocCreated.Text = Path.GetFileName(ds_ActivitySuBDetail.Tables(0).Rows(0).Item("vDocPath"))
            'Me.LnkDocCreated.CommandName = ds_ActivitySuBDetail.Tables(0).Rows(0).Item("vDocPath")
            'Me.LnkDocCreated.Visible = False


            '---Document created Link---------------
            wstrDocCreated = "vWorkSpaceId='" + WorkSpaceId + "' And iStageID='" + GeneralModule.Stage_Created.Trim() + "' And INodeID='" + NodeId + "'  and crequiredFlag='D'"
            objHelp.GetViewWorkSpaceNodeHistory(wstrDocCreated, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DocCreated, estr)
            If ds_DocCreated.Tables(0).Rows.Count > 0 Then


                wstrDocPublished = "vWorkSpaceId='" + WorkSpaceId + "' And iStageID='" + GeneralModule.Stage_published.Trim() + "' And INodeID='" + NodeId + "'  and crequiredFlag='P'"
                objHelp.GetViewWorkSpaceNodeHistory(wstrDocPublished, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DocPublished, estr)

                wstrDocApprove = "vWorkSpaceId='" + WorkSpaceId + "' And iStageID='" + GeneralModule.Stage_Authorized.Trim() + "' And INodeID='" + NodeId + "' and crequiredFlag='D'"
                wstrDocApprove += " And vFileName = '" + ds_DocCreated.Tables(0).Rows(0).Item("vFileName").ToString() + "'"

                objHelp.GetViewWorkSpaceNodeHistory(wstrDocApprove, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DocApprove, estr)

                If ds_DocApprove.Tables(0).Rows.Count > 0 Then

                    Me.lblDocCreated.Visible = False
                    Me.LnkDocCreated.Visible = False
                Else
                    Me.lblDocCreated.Visible = True
                    Me.LnkDocCreated.Visible = True
                    Me.lblDocCreated.Text = "<FONT size=""1""><a href=""" & Replace(ds_DocCreated.Tables(0).Rows(0).Item("vDocPath"), "/", "\") & """ target=""_blank"">" & Path.GetFileName(ds_DocCreated.Tables(0).Rows(0).Item("vDocPath")) & "</a></font>"
                    Me.LnkDocCreated.Text = Path.GetFileName(ds_DocCreated.Tables(0).Rows(0).Item("vDocPath"))
                    Me.LnkDocCreated.CommandName = ds_DocCreated.Tables(0).Rows(0).Item("vDocPath")
                    Me.LnkDocCreated.Visible = False
                    ViewState(VS_Remark) = ds_DocCreated.Tables(0).Rows(0).Item("vRemark")
                    ViewState(VS_UserVersion) = ds_DocCreated.Tables(0).Rows(0).Item("vUserVersion")
                    ViewState(VS_EffectiveDate) = ds_DocCreated.Tables(0).Rows(0).Item("EffectiveDate")
                    ViewState(VS_ValidUpto) = ds_DocCreated.Tables(0).Rows(0).Item("dExpiryDate") 'added by vishal for expiry date
                    ViewState(VS_ValidDays) = ds_DocCreated.Tables(0).Rows(0).Item("ValidDays")
                    ViewState(VS_DocTranNo) = ds_DocCreated.Tables(0).Rows(0).Item("iTranNo")

                End If
            End If


            '---Document Approve link---------------
            wstrDocApprove = "vWorkSpaceId='" + WorkSpaceId + "' And iStageID='" + GeneralModule.Stage_published.Trim() + "' And INodeID='" + NodeId + "' and crequiredFlag='D'"
            objHelp.GetViewWorkSpaceNodeHistory(wstrDocApprove, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DocApprove, estr)
            If ds_DocApprove.Tables(0).Rows.Count > 0 Then
                Me.lblDocApprove.Text = "<FONT size=""1""><a href=""" & Replace(ds_DocApprove.Tables(0).Rows(0).Item("vDocPath"), "/", "\") & """ target=""_blank"">" & Path.GetFileName(ds_DocApprove.Tables(0).Rows(0).Item("vDocPath")) & "</a></font>"
                Me.LnkDocApproved.Text = Path.GetFileName(ds_DocApprove.Tables(0).Rows(0).Item("vDocPath"))
                Me.LnkDocApproved.CommandName = ds_DocApprove.Tables(0).Rows(0).Item("vDocPath")
                Me.LnkDocApproved.Visible = False
                Me.lblEffectiveDate.Text = Convert.ToString(ds_DocApprove.Tables(0).Rows(0).Item("EffectiveDate"))
                Me.lblUserVersion.Text = Convert.ToString(ds_DocApprove.Tables(0).Rows(0).Item("vUserVersion"))
                Me.lblValidDays.Text = Convert.ToString(ds_DocApprove.Tables(0).Rows(0).Item("ValidDays"))
                ViewState(VS_Remark) = ds_DocApprove.Tables(0).Rows(0).Item("vRemark")
                ViewState(VS_UserVersion) = ds_DocApprove.Tables(0).Rows(0).Item("vUserVersion")
                ViewState(VS_EffectiveDate) = ds_DocApprove.Tables(0).Rows(0).Item("EffectiveDate")
                ViewState(VS_ValidDays) = ds_DocApprove.Tables(0).Rows(0).Item("ValidDays")
                ViewState(VS_DocTranNo) = ds_DocCreated.Tables(0).Rows(0).Item("iTranNo")
            End If


            DistParm = "istageid,vStageDesc".Split(",")
            Me.DDLStatus.DataSource = ds_Status.Tables(0).DefaultView.ToTable(True, DistParm)
            Me.DDLStatus.DataValueField = "iStageId"
            Me.DDLStatus.DataTextField = "vStageDesc"
            Me.DDLStatus.DataBind()
            Me.DDLStatus.Items.Insert(0, New ListItem("Select Status", "0"))

            Me.DDLStatus.Enabled = (Me.ViewState(VS_QC).ToString.ToUpper = "N")

            '---Reference Approved Documents---------------
            If Not Me.objHelp.getActivityDocLinkMatrix("vActivityId='" & ds_ActivitySuBDetail.Tables(0).Rows(0).Item("vActivityId") & "'", _
                                                WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Reference, estr) Then

                Me.ObjCommon.ShowAlert("Error while getting Data of Reference Document", Me.Page())
                Return False
            End If

            Me.TrReference.Visible = False
            If ds_Reference.Tables(0).Rows.Count > 0 Then

                Me.TrReference.Visible = True
                Me.Gv_Reference.DataSource = ds_Reference.Tables(0)
                Me.Gv_Reference.DataBind()

            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function FillGrid() As Boolean
        Dim ds_Grid As New Data.DataSet
        Dim ds_Proposed As New Data.DataSet
        Dim ds_Effective As New Data.DataSet
        Dim dv_Grid As New Data.DataView
        Dim dv_Proposed As New Data.DataView
        Dim dv_Effective As New DataView
        Dim dv_Effective1 As New DataView
        Dim estr As String = ""
        Dim WorkSpaceId As String = ""
        Dim NodeId As String = ""
        Dim VS_iTranNos As String = ""
        Dim wstr As String = ""
        Dim nodeTranNo As String = ""


        Try
            WorkSpaceId = Me.ViewState(VS_WorkspaceId)
            NodeId = Me.ViewState(VS_NId)
            'added by vishal for Proposed Docs ,Effectivedocs ,Audit Trails Starts
            wstr = "vWorkSpaceId='" + WorkSpaceId + "' and iNodeId='" + NodeId + "'"
            objHelp.GetViewWorkSpaceNodeHistory(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Proposed, estr)

            If Not objHelp.GetFieldsOfTable("Workspacenodehistory", "isNull(Max(iTranNo),1) as iTranNo", wstr, ds_Proposed, estr) Then
                Return False
            End If
            VS_iTranNos = ds_Proposed.Copy.Tables(0).Rows(0).Item("iTranNo")


            wstr = "vWorkSpaceId='" + WorkSpaceId + "' and iNodeId='" + NodeId + "' and iTranNo='" + VS_iTranNos + "'"

            objHelp.GetViewWorkSpaceNodeHistory(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Proposed, estr)
            dv_Proposed = New Data.DataView
            dv_Proposed = ds_Proposed.Copy.Tables(0).DefaultView
            dv_Proposed.RowFilter = "cRequiredFlag='D'"
            Me.lblProposedDocs.Visible = False
            Me.Gv_ProposedDocument.Visible = True
            Me.Gv_ProposedDocument.DataSource = dv_Proposed.ToTable()
            Me.Gv_ProposedDocument.DataBind()

            'dv_Grid1 = New Data.DataView
            'dv_Grid1 = ds_Grid1.Copy.Tables(0).DefaultView
            'dv_Grid1.RowFilter = "cRequiredFlag='C' and iStageId='14' or cRequiredFlag='C' and  iStageId='20'"
            'Me.lblATAfterPublished.Visible = False
            'Me.Gv_ATafterPublished.Visible = True
            'Me.Gv_ATafterPublished.DataSource = dv_Grid1.ToTable()
            'Me.Gv_ATafterPublished.DataBind()


            objHelp.GetDataForCommentGrid("vWorkSpaceID='" & WorkSpaceId & "' and iNodeId=" & NodeId, ds_Grid, estr)
            dv_Grid = New Data.DataView
            dv_Grid = ds_Grid.Tables(0).DefaultView
            dv_Grid.RowFilter = "cRequiredFlag='C'"
            Me.GV_Comment.DataSource = dv_Grid.ToTable()
            Me.GV_Comment.DataBind()

            'added by vishal for Audit trail before published
            wstr = "vWorkSpaceId='" + WorkSpaceId + "' and iNodeId='" + NodeId + "'"
            objHelp.getWorkspaceNodeHistory(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Effective, estr)

            If ds_Effective.Tables(0).Rows.Count > 0 Then


                dv_Effective = New Data.DataView
                dv_Effective1 = New Data.DataView
                dv_Effective = ds_Effective.Tables(0).DefaultView

                dv_Effective1 = ds_Effective.Tables(0).DefaultView
                dv_Effective1.RowFilter = "cRequiredFlag='D'"  ' And iTranNo = " + VS_iTranNos
                dv_Effective1.ToTable().AcceptChanges()
                If dv_Effective1.ToTable.Rows.Count > 0 Then

                    dv_Effective1.RowFilter = "cRequiredFlag='D' and iStageId='20'"
                    Me.GV_EffectiveDocs.DataSource = dv_Effective1.ToTable()
                    Me.GV_EffectiveDocs.DataBind()
                    '  End If
                    dv_Effective.RowFilter = "cRequiredFlag='D' or cRequiredFlag='P'" ' And vFolderName= "+ViewState(VS_NodeAndTranNo).ToString 
                    ViewState(VS_Gv_grid1) = ds_Effective
                    Me.Gv_ATbeforePublished.DataSource = dv_Effective.ToTable()
                    Me.Gv_ATbeforePublished.DataBind()
                    ' Me.Gv_ProposedDocument.Visible = False
                    'Me.lblProposedDocs.Visible = True
                    '  Me.Gv_ATafterPublished.Visible = False
                    ' Me.lblATAfterPublished.Visible = True

                End If
            End If



            'dv_Grid = New Data.DataView
            'dv_Grid = ds_Grid.Tables(0).DefaultView
            'dv_Grid.RowFilter = "cRequiredFlag='D'"
            'Me.Gv_DocHistory.DataSource = dv_Grid.ToTable()
            'Me.Gv_DocHistory.DataBind()

            dv_Grid = New Data.DataView
            dv_Grid = ds_Grid.Tables(0).DefaultView
            dv_Grid.RowFilter = "cRequiredFlag='Q'"
            Me.GV_QCComment.DataSource = dv_Grid.ToTable()
            Me.GV_QCComment.DataBind()


            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "New TranNo"

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
                Wstr = "vWorkSpaceId='" & WsId + "' and iNodeId= " & NId & " and cRequiredFlag='D' and iStageId=" & Me.DDLStatus.SelectedValue.Trim()
            ElseIf type.ToUpper = "COM" Then
                Wstr = "vWorkSpaceId='" & WsId + "' and iNodeId= " & NId & " and cRequiredFlag='C' and iStageId=" & Me.DDLStatus.SelectedValue.Trim()
            ElseIf type.ToUpper = "QC" Then
                Wstr = "vWorkSpaceId='" & WsId + "' and iNodeId= " & NId & " and cRequiredFlag='Q' and iStageId=" & Me.DDLStatus.SelectedValue.Trim()
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

#End Region

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
            TNo = Me.ViewState(VS_TNo)
            'For New TranNo

            Me.ViewState(VS_NId) = NId

            Me.HdfFolder.Value = ""

            If type.ToUpper = "TEMP" Then
                DocDetail = "TempDocDetail/" 'System.Configuration.ConfigurationManager.AppSettings("BaseWorkFolder")   
                Me.HdfFolder.Value = WsId & "/" & NId & "/" & TNo
            Else
                DocDetail = Me.HdfBaseFolder.Value.Trim() 'System.Configuration.ConfigurationManager.AppSettings("BaseWorkFolder")
                Me.HdfFolder.Value = NId & "/" & TNo
            End If

            'dir = New DirectoryInfo(ObjPath.Param_FolderPath(DocDetail & Me.HdfFolder.Value.Trim()))
            dir = New DirectoryInfo(Server.MapPath(DocDetail & Me.HdfFolder.Value.Trim()))
            If Not dir.Exists() Then
                dir.Create()
            End If

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

                    If iFileSize >= 15728640 Then 'Validity of Size
                        ObjCommon.ShowAlert("Error Occured, File size should be less than 15 MB...", Me)
                        Return False
                    End If

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
                    'Me.ViewState("FileName") = FlUpload.PostedFile.FileName
                    Dim str As String = Replace(Me.FlUpload.PostedFile.FileName, "\", "\\")

                    Me.FlUpload.PostedFile.SaveAs(Server.MapPath(DocPath) & Path.GetFileName(Me.FlUpload.PostedFile.FileName))

                    Me.HdfFileName.Value = ""
                    Me.HdfFileName.Value = Path.GetFileName(Me.FlUpload.PostedFile.FileName)


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
                Dim Tempfilename As String = "TempDocDetail/" + Me.ViewState(VS_WorkspaceId) + "/" + Me.HdfFolder.Value.Trim() + "/"
                If Not File.Exists(Server.MapPath(DocPath + Me.HdfFileName.Value.ToString.Trim())) Then
                    File.Move(Server.MapPath(Tempfilename.Trim() + "/" + Me.HdfFileName.Value.ToString.Trim()), Server.MapPath(DocPath + Me.HdfFileName.Value.ToString.Trim()))
                End If

                'Commeted because it will cause to "logout" By Naimesh Dave
                ''To Delete TempFolder of NodeId
                'dir = New DirectoryInfo(Server.MapPath(Tempfilename))
                'If dir.Exists() Then
                '    dir.Delete(True)
                'End If 
                '**************************88

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

                    If iFileSize >= 1048576 Then 'Validity of Size
                        ObjCommon.ShowAlert("Error Occured, File size should be less than 1 MB...", Me)
                        Return False
                    End If

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


                End If
            ElseIf Type.ToUpper = "QC" Then 'To Upload Document for Comment

                If Not NewTranNo("QC") Then
                    ObjCommon.ShowAlert("Error Occured while Getting Tran No., Try Again... ", Me)
                    Return False
                End If
                If Not CreateFolder("QC") Then
                    ObjCommon.ShowAlert("Error Occured while Creating Folder, Try Again... ", Me)
                    'Exit Function
                    Return False
                End If

                DocPath = Me.HdfBaseFolder.Value.Trim() + Me.HdfFolder.Value.Trim() + "/"

                If Not IsNothing(Me.FlUploadCom) Then

                    iFileSize = Me.FlUploadCom.PostedFile.ContentLength

                    If iFileSize >= 1048576 Then 'Validity of Size
                        ObjCommon.ShowAlert("Error Occured, File size should be less than 1 MB...", Me)
                        Return False
                    End If

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


                End If
            Else 'For Other then Created
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
                If Me.LnkDocCreated.Text <> "" Then

                    Me.FlUploadCom.PostedFile.SaveAs(Server.MapPath(DocPath) & Me.LnkDocCreated.Text.Trim())

                    Me.HdfFileName.Value = ""
                    Me.HdfFileName.Value = Me.LnkDocCreated.Text.Trim()

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
                dr("vUserVersion") = Me.txtVersionNo.Text
                dr("EffectiveDate") = IIf(String.IsNullOrEmpty(Convert.ToString(Me.txtdoer.Text.Trim)), System.DBNull.Value, Me.txtdoer.Text.Trim)
                dr("dExpiryDate") = IIf(String.IsNullOrEmpty(Convert.ToString(Me.TextValidDate.Text.Trim)), System.DBNull.Value, Me.TextValidDate.Text.Trim) 'added by vishal for valid upto
                dr("ValidDays") = Me.txtValidDays.Text.Trim()
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
                dr("vRemark") = Me.txtComments.Text.Trim
                dr("iStageId") = Me.DDLStatus.SelectedValue.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dr("vUserVersion") = Me.ViewState(VS_UserVersion)
                dr("EffectiveDate") = IIf(String.IsNullOrEmpty(Convert.ToString(Me.ViewState(VS_EffectiveDate))), System.DBNull.Value, Me.ViewState(VS_EffectiveDate))
                dr("dExpiryDate") = IIf(String.IsNullOrEmpty(Convert.ToString(Me.ViewState(VS_ValidUpto))), System.DBNull.Value, Me.ViewState(VS_ValidUpto)) 'added for Valid Date upto
                dr("ValidDays") = Me.ViewState(VS_ValidDays)
                dt_Workspacenodehistory.Rows.Add(dr)
            ElseIf Type.ToUpper = "QC" Then
                dr = dt_Workspacenodehistory.NewRow()
                dr("vWorkSpaceId") = Me.ViewState(VS_WorkspaceId)
                dr("iNodeId") = Me.ViewState(VS_NId)
                dr("iTranNo") = Me.ViewState(VS_TNo)
                dr("vFileName") = Me.HdfFileName.Value.ToString.Trim()
                dr("vFileType") = Path.GetExtension(Me.HdfFileName.Value.ToString.Trim())
                dr("vFolderName") = Me.HdfFolder.Value.ToString.Trim() 'Me.ViewState(VS_Path) 'Me.Session("Path").ToString.Trim()
                dr("cRequiredFlag") = "Q"
                dr("cQCAuthorization") = Me.RBLQCAuth.SelectedValue.Trim()
                dr("vDefaultFileFormat") = ""
                dr("vRemark") = Me.txtComments.Text.Trim()
                dr("iStageId") = Me.DDLStatus.SelectedValue.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dr("vUserVersion") = Me.ViewState(VS_UserVersion)
                dr("EffectiveDate") = IIf(String.IsNullOrEmpty(Convert.ToString(Me.ViewState(VS_EffectiveDate))), System.DBNull.Value, Me.ViewState(VS_EffectiveDate))
                dr("dExpiryDate") = IIf(String.IsNullOrEmpty(Convert.ToString(Me.ViewState(VS_ValidUpto))), System.DBNull.Value, Me.ViewState(VS_ValidUpto)) 'added by vishal for valid upto
                dr("ValidDays") = Me.ViewState(VS_ValidDays)
                dt_Workspacenodehistory.Rows.Add(dr)

            Else

                dr = dt_Workspacenodehistory.NewRow()
                dr("vWorkSpaceId") = Me.ViewState(VS_WorkspaceId)
                dr("iNodeId") = Me.ViewState(VS_NId)
                dr("iTranNo") = Me.ViewState(VS_DocTranNo)
                dr("vFileName") = Me.HdfFileName.Value.ToString.Trim() 'Me.LnkDocCreated.Text.Trim()
                dr("vFileType") = Path.GetExtension(Me.HdfFileName.Value.ToString.Trim())
                dr("vFolderName") = Me.HdfFolder.Value.ToString.Trim()
                dr("cRequiredFlag") = "D"
                dr("vDefaultFileFormat") = ""
                dr("vRemark") = Me.txtComments.Text.Trim
                dr("iStageId") = Me.DDLStatus.SelectedValue.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dr("vUserVersion") = Me.ViewState(VS_UserVersion)
                dr("EffectiveDate") = IIf(String.IsNullOrEmpty(Convert.ToString(Me.ViewState(VS_EffectiveDate))), System.DBNull.Value, Me.ViewState(VS_EffectiveDate))
                dr("dExpiryDate") = IIf(String.IsNullOrEmpty(Convert.ToString(Me.ViewState(VS_ValidUpto))), System.DBNull.Value, Me.ViewState(VS_ValidUpto)) 'added by vishal for valid upto
                dr("ValidDays") = Me.ViewState(VS_ValidDays)
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

        strfname = Me.LnkDocCreated.Text.Trim() 'CType(ViewState("Fname"), String)

        strFileIndex = strfname.LastIndexOf("/")
        strfname = Mid(strfname, 2)


        'strFinal = HttpContext.Current.Server.MapPath("Attachment/" + strfname)
        strFinal = HttpContext.Current.Server.MapPath(Me.LnkDocCreated.CommandName.Trim())
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
        Dim WorkspaceId As String = ""
        Dim NodeId As String = ""
        Dim ds_Grid As New DataSet
        Dim dv_Grid As New DataView

        Try
            If Me.DDLStatus.SelectedValue > GeneralModule.Stage_Created Then
                ' AndAlso Me.ViewState(VS_TNo) = 1 Then
                If Not Upload("NotCreate") Then 'To create Folder and Upload File
                    Exit Sub
                End If
                If Not AssignValues("NotCreate") Then
                    Exit Sub
                End If
            End If


            If Not IsNothing(Me.FlUpload) Then
                If Me.ViewState(VS_QC).ToString.ToUpper.Trim() = "Y" Then
                    If Me.FlUploadCom.PostedFile.FileName <> "" Then
                        If Not Upload("QC") Then 'To create Folder and Upload File
                            Exit Sub
                        End If
                    Else
                        If Not NewTranNo("QC") Then
                            ObjCommon.ShowAlert("Error Occured while Getting Tran No., Try Again... ", Me)
                        End If
                    End If
                    If Not AssignValues("QC") Then
                        Exit Sub
                    End If
                Else
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
        Dim DMS As String = ""
        Me.Response.Redirect(Me.Request.QueryString("Page2").Trim() & ".aspx?WorkSpaceId=" & Me.Request.QueryString("WorkSpaceId").Trim() & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Type=" & Me.Request.QueryString("Type") & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))
        'Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub BtnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim DMS As String = ""
        Me.Response.Redirect(Me.Request.QueryString("Page2").Trim() & ".aspx?WorkSpaceId=" & Me.Request.QueryString("WorkSpaceId").Trim() & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Type=" & Me.Request.QueryString("Type") & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))
    End Sub

#End Region

#Region "Reset Page"

    Private Sub ResetPage()
        Me.ViewState(VS_DtWSNHistory) = Nothing
        Me.ViewState(VS_Choice) = Nothing
        Me.ViewState(VS_WSSDDetailId) = Nothing
        Me.ViewState(VS_WorkspaceId) = Nothing
        Me.ViewState(VS_NId) = Nothing
        Me.ViewState(VS_TNo) = Nothing
        Me.ViewState(VS_TempTNo) = Nothing
        Me.ViewState(VS_CheckOut) = Nothing
        Me.ViewState(VS_LockFlag) = Nothing

        Me.HdfFileName.Value = ""
        Me.HdfBaseFolder.Value = ""
        Me.HdfFolder.Value = ""
        Me.HdfTranNo.Value = ""

        Me.lblUploadedLink.Text = ""
        Me.txtValidDays.Text = ""
        Me.txtRemarks.Text = ""
        Me.txtVersionNo.Text = ""
        Me.txtdoer.Text = ""
        Me.TextValidDate.Text = ""

        Me.txtComments.Text = ""
        'Me.Response.Redirect("frmDocumentDetail.aspx?Mode=1&WorkSpaceId=" & Me.ViewState(VS_WorkspaceId) & "&NodeId=" & Me.ViewState(VS_NId) & "&Mode=1" & "&Page=" & Me.Request.QueryString("Page") & "&Page2=" & Me.Request.QueryString("Page2"))

        GenCall()

        Me.DDLStatus.SelectedIndex = 0
        Me.BtnBack.Visible = True

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

#Region "Selected Change "

    Protected Sub DDLStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds_CheckOut As New DataSet
        Dim ds_Locked As New DataSet
        Dim dt_CheckOut As New DataTable
        Dim estr As String = ""
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Me.BtnLockNode.Enabled = True
            Me.BtnClose.Enabled = True

            ''Me.BtnUpLoad1.Disabled = True
            Me.BtnUnLock.Enabled = False
            Me.BtnUnlockWS.Enabled = False

            Me.tdCommEntry.Visible = False

            If Me.DDLStatus.SelectedValue <> GeneralModule.Stage_Created Then 'Hard-Coded For Created Stage
                Me.tdCommEntry.Visible = True
                Me.BtnBack.Visible = False

            ElseIf Me.DDLStatus.SelectedValue = GeneralModule.Stage_Created Then 'Hard-Coded For Created Stage
                Me.BtnBack.Visible = True

                ' Me.Div_Lock.Visible = True
                Me.DDLStatus.Enabled = False

                ''Checking If Node is Locked or Not By any User
                If Not Me.objHelp.GetCheckedoutfiledetail("vWorkSpaceID='" & Me.ViewState(VS_WorkspaceId) & "' and iNodeId=" & Me.ViewState(VS_NId) & " and itranNo=(select Max(iTranNo) from checkedoutfiledetail where vWorkSpaceId='" & Me.ViewState(VS_WorkspaceId) & "' and iNodeId=" & Me.ViewState(VS_NId) & ") and  isNodeLocked='Y'", _
                        WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Locked, estr) Then
                    ObjCommon.ShowAlert("Error Occured While Checking ", Me)
                    Exit Sub
                End If
                ''If Node is Locked By any User then it will check it

                If ds_Locked.Tables(0).Rows.Count < 1 Then
                    Me.ViewState(VS_LockFlag) = "N"
                    Exit Sub
                End If

                ''Checking if Locked By own or Any other
                If ds_Locked.Tables(0).Rows(0).Item("isNodeLocked").ToString.ToUpper = "Y" And _
                    ds_Locked.Tables(0).Rows(0).Item("iModifyBY") <> Me.Session(S_UserID) Then

                    ObjCommon.ShowAlert("This Node is Locked By Another User, Try Again... ", Me)
                    Exit Sub

                End If
                ''Checking if Locked By own then give last Uploaded Document in Lable.
                Me.BtnLockNode.Enabled = False
                Me.BtnUpLoad1.Disabled = False
                Me.BtnUnlockWS.Enabled = True
                Me.BtnClose.Enabled = True

                NewTranNo("TEMP")
                Me.HdfFileName.Value = "TempDocDetail/" & Me.ViewState(VS_WorkspaceId).ToString.Trim() & "/" & ds_Locked.Tables(0).Rows(0).Item("iNodeId").ToString.Trim() & _
                                        "/" & Me.ViewState(VS_TNo).ToString.Trim() & "/" & ds_Locked.Tables(0).Rows(0).Item("vFileName").ToString.Trim()

                If Not IsNothing(Path.GetFileName(Me.HdfFileName.Value())) AndAlso _
                    Path.GetFileName(Me.HdfFileName.Value()) <> "" Then

                    Me.BtnUnLock.Enabled = True

                End If

                Me.lblUploadedLink.Text = "<table><tr><td align=""right"" class=""Label""><Strong>Last Uploaded file: </Strong><FONT size=""1""><a href=""" & Replace(Me.HdfFileName.Value(), "/", "\") & """ target=""_blank"">" & Path.GetFileName(Me.HdfFileName.Value()) & "</a></font></td></tr></table>"
                Me.txtVersionNo.Text = ds_Locked.Tables(0).Rows(0).Item("vUserVersion").ToString.Trim()
                Me.txtRemarks.Text = ds_Locked.Tables(0).Rows(0).Item("vRemark").ToString.Trim()
                Me.txtdoer.Text = ""
                Me.txtValidDays.Text = ds_Locked.Tables(0).Rows(0).Item("ValidDays").ToString.Trim()
                If Not ds_Locked.Tables(0).Rows(0).Item("EffectiveDate") Is System.DBNull.Value Then
                    Me.txtdoer.Text = ds_Locked.Tables(0).Rows(0).Item("EffectiveDate")
                    If Not ds_Locked.Tables(0).Rows(0).Item("dExpiryDate") Is System.DBNull.Value Then
                        Me.TextValidDate.Text = ds_Locked.Tables(0).Rows(0).Item("dExpiryDate") 'added by vishal for valid upto
                    End If
                End If

            End If
            '***********
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub
#End Region

#Region "Grid Event"

    Protected Sub GV_EffectiveDocs_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GV_EffectiveDocs.RowCommand


        ' MdlEffectiveAt.Show()



    End Sub

    'Protected Sub GV_EffectiveDocs_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GV_EffectiveDocs.RowCommand
    '    If e.CommandName = "Details" Then




    '    End If
    'End Sub
    Protected Sub GV_EffectiveDocs_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_EffectiveDocs.RowCreated
        'e.Row.Cells(2).Visible = False
        e.Row.Cells(5).Visible = False
        e.Row.Cells(6).Visible = False
    End Sub

    Protected Sub GV_EffectiveDocs_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_EffectiveDocs.RowDataBound
        'added by vishal for Effective Docs to give Link in FileName

        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(6).Text = "12" Then
                e.Row.Cells(7).Text = "Created"

            ElseIf e.Row.Cells(6).Text = "14" Then
                e.Row.Cells(7).Text = "Reviewed"

            ElseIf e.Row.Cells(6).Text = "20" Then
                e.Row.Cells(7).Text = "Authorized"
            ElseIf e.Row.Cells(6).Text = "21" Then
                e.Row.Cells(7).Text = "Published"

            End If

            CType(e.Row.FindControl("LblGv_EffectiveDocumentSrNo"), Label).Text = e.Row.RowIndex + 1
            CType(e.Row.FindControl("hlnk_EffectiveDocument"), HyperLink).NavigateUrl = Me.HdfBaseFolder.Value & e.Row.Cells(2).Text.Trim() & _
                                                                         "/" & CType(e.Row.FindControl("hlnk_EffectiveDocument"), HyperLink).Text.Trim()
            'ViewState(VS_ValidDate) = e.Row.Cells(5).Text

            'CType(e.Row.Cells(, BoundField).DataFormatString = "{0:dd-MMM-yyyy}"
        End If
        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    CType(e.Row.FindControl("LblGv_EffectiveDocumentSrNo"), Label).Text = e.Row.RowIndex + 1
        '    CType(e.Row.FindControl("hlnk_EffectiveDocument"), HyperLink).NavigateUrl = Me.HdfBaseFolder.Value & e.Row.Cells(2).Text.Trim() & _
        '                                                                "/" & CType(e.Row.FindControl("hlnk_EffectiveDocument"), HyperLink).Text.Trim()
        '    'CType(e.Row.Cells(, BoundField).DataFormatString = "{0:dd-MMM-yyyy}"
        '    'ViewState(VS_ExpiryDate) = e.Row.Cells(5).Text 'added by vishal to check date in Effective Grid
        '    CType(e.Row.FindControl("lnkBtn"), LinkButton).CommandName = "DETAILS"
        '    CType(e.Row.FindControl("lnkBtn"), LinkButton).CommandArgument = e.Row.RowIndex
        'End If
    End Sub
    Protected Sub Gv_ProposedDocument_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Gv_ProposedDocument.RowCreated
        e.Row.Cells(5).Visible = False
        e.Row.Cells(6).Visible = False
    End Sub


    Protected Sub Gv_ProposedDocument_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Gv_ProposedDocument.RowDataBound
        'added by vishal for Proposed docs to give Link in FileName

        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(6).Text = "12" Then
                e.Row.Cells(7).Text = "Created"

            ElseIf e.Row.Cells(6).Text = "14" Then
                e.Row.Cells(7).Text = "Reviewed"

            ElseIf e.Row.Cells(6).Text = "20" Then
                e.Row.Cells(7).Text = "Authorized"
            ElseIf e.Row.Cells(6).Text = "21" Then
                e.Row.Cells(7).Text = "Published"

            End If

            CType(e.Row.FindControl("LblGv_ProposedDocumentSrNo"), Label).Text = e.Row.RowIndex + 1
            CType(e.Row.FindControl("hlnk_ProposedDocument"), HyperLink).NavigateUrl = Me.HdfBaseFolder.Value & e.Row.Cells(5).Text.Trim() & _
                                                                        "/" & CType(e.Row.FindControl("hlnk_ProposedDocument"), HyperLink).Text.Trim()
            'ViewState(VS_ValidDate) = e.Row.Cells(5).Text

            'CType(e.Row.Cells(, BoundField).DataFormatString = "{0:dd-MMM-yyyy}"
        End If
    End Sub



    Protected Sub Gv_ATbeforePublished_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Gv_ATbeforePublished.RowCreated
        Try
            e.Row.Cells(6).Visible = False
            e.Row.Cells(7).Visible = False
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")

        End Try

    End Sub
    Protected Sub Gv_ATbeforePublished_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Gv_ATbeforePublished.RowDataBound
        'added by vishal for audit trial to give Link in FileName
        Try


            If e.Row.Cells(7).Text = "12" Then
                e.Row.Cells(8).Text = "Created"

            ElseIf e.Row.Cells(7).Text = "14" Then
                e.Row.Cells(8).Text = "Reviewed"

            ElseIf e.Row.Cells(7).Text = "20" Then
                e.Row.Cells(8).Text = "Authorized"
            ElseIf e.Row.Cells(7).Text = "21" Then
                e.Row.Cells(8).Text = "Published"

            End If
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("LblATbeforeSrNo"), Label).Text = e.Row.RowIndex + 1
                CType(e.Row.FindControl("hlnkATbefore"), HyperLink).NavigateUrl = Me.HdfBaseFolder.Value & e.Row.Cells(6).Text.Trim() & _
                                                                            "/" & CType(e.Row.FindControl("hlnkATbefore"), HyperLink).Text.Trim()
                'CType(e.Row.Cells(, BoundField).DataFormatString = "{0:dd-MMM-yyyy}"
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")

        End Try
    End Sub


    'Protected Sub Gv_ATafterPublished_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Gv_ATafterPublished.RowDataBound
    '    'added by vishal for audit trial to give Link in FileName

    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        CType(e.Row.FindControl("LblATafterSrNo"), Label).Text = e.Row.RowIndex + 1
    '        CType(e.Row.FindControl("hlnkATafter"), HyperLink).NavigateUrl = Me.HdfBaseFolder.Value & e.Row.Cells(7).Text.Trim() & _
    '                                                                    "/" & CType(e.Row.FindControl("hlnkATafter"), HyperLink).Text.Trim()
    '        'CType(e.Row.Cells(, BoundField).DataFormatString = "{0:dd-MMM-yyyy}"
    '    End If
    'End Sub

    'Protected Sub Gv_ATafterPublished_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Gv_ATafterPublished.RowCreated
    '    e.Row.Cells(7).Visible = False
    'End Sub
    Protected Sub GV_Comment_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("LblSrNo"), Label).Text = e.Row.RowIndex + 1
            CType(e.Row.FindControl("hlnkFile"), HyperLink).NavigateUrl = Me.HdfBaseFolder.Value & e.Row.Cells(7).Text.Trim() & _
                                                        "/" & CType(e.Row.FindControl("hlnkFile"), HyperLink).Text.Trim()

            'CType(e.Row.Cells(, BoundField).DataFormatString = "{0:dd-MMM-yyyy}"
        End If

    End Sub

    Protected Sub GV_Comment_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        e.Row.Cells(7).Visible = False
    End Sub

    Protected Sub GV_QCComment_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        e.Row.Cells(7).Visible = False
    End Sub

    Protected Sub GV_QCComment_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("LblQCSrNo"), Label).Text = e.Row.RowIndex + 1
            CType(e.Row.FindControl("hlnkQCFile"), HyperLink).NavigateUrl = Me.HdfBaseFolder.Value & e.Row.Cells(7).Text.Trim() & _
                                                        "/" & CType(e.Row.FindControl("hlnkQCFile"), HyperLink).Text.Trim()

            If e.Row.Cells(8).Text = "Y" Then
                e.Row.Cells(8).Text = "Approved"
            ElseIf e.Row.Cells(8).Text = "N" Then
                e.Row.Cells(8).Text = "Rejected"
            ElseIf e.Row.Cells(8).Text = "F" Then
                e.Row.Cells(8).Text = "FeedBack"
            End If

        End If
    End Sub

    Protected Sub Gv_Reference_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        e.Row.Cells(5).Visible = False
        'e.Row.Cells(7).Visible = False
    End Sub

    Protected Sub Gv_Reference_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("LblrefSrNo"), Label).Text = e.Row.RowIndex + 1
            CType(e.Row.FindControl("hlnkrefFile"), HyperLink).NavigateUrl = e.Row.Cells(5).Text.Trim()
            'CType(e.Row.FindControl("hlnkrefFile"), HyperLink).Text = e.Row.Cells(7).Text
            'CType(e.Row.Cells(, BoundField).DataFormatString = "{0:dd-MMM-yyyy}"
        End If
    End Sub

#End Region

#Region "Buttons for Lock"

    Protected Sub BtnLockNode_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds_CheckOut As New DataSet
        Dim ds_Locked As New DataSet
        Dim dt_CheckOut As New DataTable
        Dim estr As String = ""
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Me.ViewState(VS_LockFlag) = "N" Then
                If Not LockUnlock("Lock") Then
                    ObjCommon.ShowAlert("Error Occured While Checking", Me)
                    Exit Sub
                End If

                ObjCommon.ShowAlert("Node is Locked Now", Me)
                Me.BtnLockNode.Enabled = False
                Me.BtnUpLoad1.Disabled = False
                Me.BtnUnlockWS.Enabled = True
                Me.BtnClose.Enabled = True
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub BtnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        '  Me.Div_Lock.Visible = False
        Me.DDLStatus.SelectedIndex = 0
        Me.DDLStatus.Enabled = True
    End Sub

    Protected Sub BtnUpLoad1_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            'added by vishal for date validation
            'If CType(ViewState(VS_ValidDate), Date) > CType(Me.txtdoer.Text, Date) Then
            '    ObjCommon.ShowAlert("Effective date is Less then Previous Expiry Date", Me.Page)
            '    Exit Sub
            'End If
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not Upload("TEMPDOC") Then 'To create Folder and Upload File
                Exit Sub
            End If

            LockUnlock("UPLOAD")
            ObjCommon.ShowAlert("Uploaded Successfully. Now to save Unlock the Node", Me.Page)

            Me.DDLStatus_SelectedIndexChanged(sender, e)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub BtnUnLock_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds_Save As New DataSet
        Dim dt_Save As New DataTable
        Dim estr As String = ""
        Dim dsLocked As New DataSet
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not Me.objHelp.GetCheckedoutfiledetail("vWorkSpaceID='" & Me.ViewState(VS_WorkspaceId) & _
                        "' and iNodeId=" & Me.ViewState(VS_NId) & " and itranNo=(select Max(iTranNo) from checkedoutfiledetail where vWorkSpaceId='" & _
                        Me.ViewState(VS_WorkspaceId) & "' and iNodeId=" & Me.ViewState(VS_NId) & ") and  isNodeLocked='Y' ", _
                        WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsLocked, estr) Then

                ObjCommon.ShowAlert("Error Occured While Checking ", Me)
                Exit Sub

            End If

            Me.HdfFileName.Value = dsLocked.Tables(0).Rows(0)("vFileName")

            If Not Upload("DOC") Then 'To create Folder and Upload File
                Exit Sub
            End If

            If Not LockUnlock("UNLOCK") Then

                ObjCommon.ShowAlert("Error Occured While Checking ", Me)
                Exit Sub

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
            End If

            'ObjCommon.ShowAlert("Record Saved SuccessFully and Node is Now Unlocked.", Me.Page)
            'Server.Transfer("frmProjectDetailMst.aspx?WorkSpaceId=0000000051&Page=frmMyProject&Type=ALL")
            'Me.Response.Redirect("frmDocumentDetail_New.aspx?Mode=1&WorkSpaceId=" & Me.ViewState(VS_WorkspaceId) & _
            '                       "&NodeId=" & Me.ViewState(VS_NId) & "&Page=" & Me.Request.QueryString("Page") & _
            '                        "&Page2=" & Me.Request.QueryString("Page2") & "&Type=" & Me.Request.QueryString("Type") & "&QC=N")

            'Me.Div_Lock.Visible = False
            Me.DDLStatus.Enabled = True
            ResetPage()
            Me.DDLStatus.SelectedIndex = 0

        Catch ex As Exception
            'Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub BtnUnlockWS_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dir As DirectoryInfo
        Dim tempfile As FileInfo
        Dim dirName As String = ""

        tempfile = New FileInfo(Server.MapPath(Me.HdfFileName.Value))

        If tempfile.Exists() Then
            tempfile.Delete()
        End If

        'Commeted because it will cause to "logout" By Naimesh Dave
        'dirName = Me.HdfFileName.Value.Replace("/" + Path.GetFileName(Me.HdfFileName.Value()), "")
        'dir = New DirectoryInfo(Server.MapPath(dirName))
        'If dir.Exists() Then
        '    dir.Delete(True)
        'End If
        '***************************

        If Not LockUnlock("UNLOCKWS") Then

            ObjCommon.ShowAlert("Error Occured While Checking ", Me)
            Exit Sub

        End If

        ObjCommon.ShowAlert("Node is Unlocked Without Save", Me)
        Me.BtnLockNode.Enabled = True
        ' Me.Div_Lock.Visible = False
        Me.DDLStatus.Enabled = True
        ResetPage()

    End Sub

    Private Function LockUnlock(ByVal Type As String) As Boolean
        Dim ds_CheckOut As New DataSet
        Dim ds_CheckOut_Blank As New DataSet
        Dim ds_Locked As New DataSet
        Dim dt_CheckOut As New DataTable
        Dim dr As DataRow
        Dim estr As String = ""

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

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

            ElseIf Type.ToUpper = "UPLOAD" Then
                dr("vFileName") = Me.HdfFileName.Value
                dr("isNodeLocked") = "Y"
                dr("vRemark") = Me.txtRemarks.Text.Trim
                dr("vUserVersion") = Me.txtVersionNo.Text.Trim
                dr("EffectiveDate") = IIf(String.IsNullOrEmpty(Convert.ToString(Me.txtdoer.Text.Trim)), System.DBNull.Value, Me.txtdoer.Text.Trim)
                dr("dExpiryDate") = IIf(String.IsNullOrEmpty(Convert.ToString(Me.TextValidDate.Text.Trim)), System.DBNull.Value, Me.TextValidDate.Text.Trim) 'added by vishal for valid upto expiry date
                dr("ValidDays") = Me.txtValidDays.Text.Trim()

            ElseIf Type.ToUpper = "UNLOCK" Then
                dr("vFileName") = Me.HdfFileName.Value
                dr("isNodeLocked") = "N"
                dr("vRemark") = Me.txtRemarks.Text.Trim
                dr("vUserVersion") = Me.txtVersionNo.Text.Trim
                dr("EffectiveDate") = IIf(String.IsNullOrEmpty(Convert.ToString(Me.txtdoer.Text.Trim)), System.DBNull.Value, Me.txtdoer.Text.Trim)
                dr("dExpiryDate") = IIf(String.IsNullOrEmpty(Convert.ToString(Me.TextValidDate.Text.Trim)), System.DBNull.Value, Me.TextValidDate.Text.Trim) 'added by vishal for valid upto expiry date
                dr("ValidDays") = Me.txtValidDays.Text.Trim()

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

            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "Grid for Lock"
    'Protected Sub Gv_DocHistory_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        CType(e.Row.FindControl("LblDocSrNo"), Label).Text = e.Row.RowIndex + 1
    '        CType(e.Row.FindControl("hlnkDocFile"), HyperLink).NavigateUrl = Me.HdfBaseFolder.Value & e.Row.Cells(6).Text.Trim() & _
    '                                                                    "/" & CType(e.Row.FindControl("hlnkDocFile"), HyperLink).Text.Trim()
    '        'CType(e.Row.Cells(, BoundField).DataFormatString = "{0:dd-MMM-yyyy}"
    '    End If
    'End Sub

    Protected Sub Gv_DocHistory_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        'e.Row.Cells(7).Visible = False
    End Sub

#End Region


    Protected Sub DDLStatus_SelectedIndexChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLStatus.SelectedIndexChanged
        Dim WorkspaceId As String = String.Empty
        Dim NodeId As String = String.Empty
        Dim Ds_Version As New DataSet
        Dim estr As String = String.Empty
        Dim VersionNo As Integer = 1.0
        Dim wstr As String = String.Empty

        If DDLStatus.SelectedIndex = 1 Then
            WorkSpaceId = Me.ViewState(VS_WorkspaceId)
            NodeId = Me.ViewState(VS_NId)

            wstr = "vWorkSpaceId='" + WorkspaceId + "' and iNodeId='" + NodeId + "'"
            objHelp.GetViewWorkSpaceNodeHistory(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_Version, estr)
            If (Ds_Version.Tables(0).Rows.Count < 1) Then
                txtVersionNo.Text = VersionNo
            Else
                VersionNo = CType(Ds_Version.Tables(0).Rows(0).Item("iTranNo"), Integer)
                txtVersionNo.Text += CType(VersionNo + 1, String)
            End If


            MdlCreate.Show()

        End If
    End Sub

    Protected Sub BtnLockNode_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnLockNode.Click

        MdlCreate.Show()




    End Sub

    Protected Sub BtnUpLoad1_ServerClick1(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnUpLoad1.ServerClick
        MdlCreate.Show()



    End Sub
    'Protected Sub lnkBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    '    Dim ds_Grid3 As New DataSet
    '    Dim dv_Grid3 As New DataView

    '    Dim estr As String = ""
    '    Dim WorkSpaceId As String = ""
    '    Dim NodeId As String = ""
    '    Dim VS_iTranNos As String = ""
    '    Dim wstr As String = ""
    '    Dim nodeTranNo As String = ""


    '    Try
    '        Dim gvr As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)

    '        ds_Grid3 = ViewState(VS_Gv_grid1)
    '        dv_Grid3 = ds_Grid3.Tables(0).DefaultView
    '        dv_Grid3.RowFilter = "cRequiredFlag='D' and vFolderName='" + gvr.Cells(2).Text.Trim() + "'"
    '        Me.Gv_ATbeforePublished.DataSource = dv_Grid3.ToTable()
    '        Me.Gv_ATbeforePublished.DataBind()
    '        'DivPopUpEffectiveAtDocs.Style("display") = "block"

    '        'MdlEffectiveAt.Show()

    '    Catch ex As Exception

    '        Me.ShowErrorMessage(ex.Message, "")


    '    End Try

    'Private Function FillPopUpForEffective() As Boolean
    '  

    'End Function



End Class
