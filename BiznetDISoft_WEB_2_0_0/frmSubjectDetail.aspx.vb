Imports System.IO
Partial Class frmSubjectDetail
    Inherits System.Web.UI.Page


#Region "Variable Declaration"

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private ObjPath As New ClsFolderPath
    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtWSSDDetail As String = "DtWorkspaceSubjectDocDetail"
    Private Const VS_WSSDDetailId As String = "vWorkSpaceSubjectDocDetailId"
    Private Const VS_WorkspaceId As String = "vWorkSpaceNo"
    Private Const VS_SubjectId As String = "SubjectId"
    Private Const VS_TNo As String = "TranNo"
    Private Const VS_Path As String = "Path"

    Private Const VS_Comment As String = "Comment Type"
    Private Const GVC_Id As Integer = 1
    Private Const GVC_WorkspaceSubId As Integer = 2
    Private Const GVC_SubjectId As Integer = 3
    Private Const GVC_FullName As Integer = 4
    Private Const GVC_StageDesc As Integer = 10
    Private Const GVC_ChangeStatus As Integer = 11
    Private Const GVC_NoOfComments As Integer = 12
    Private Const GVC_QC As Integer = 13
    Private Const GVC_ProjectNo As Integer = 14

    Private Const VS_dtMedEx_Fill As String = "dtMedEx_Fill"
    Private Const VS_DtQC As String = "dtQC"
    Private Const VS_SubjectName As String = "SubjectName"


    'nWorkSpaceSubjectCommentNo,vWorkSpaceSubjectDocDetailId,iTranNo,FullName
    'vQCComment,cQCFlag,vQCGivenBy,dQCGivenOn,vResponse,vResponseGivenBy,dResponseGivenOn,Response

    Private Const GVCQC_nWorkSpaceSubjectCommentNo As Integer = 0
    Private Const GVCQC_vWorkSpaceSubjectDocDetailId As Integer = 1
    Private Const GVCQC_iTranNo As Integer = 2
    Private Const GVCQC_FullName As Integer = 3
    Private Const GVCQC_vQCComment As Integer = 4
    Private Const GVCQC_cQCFlag As Integer = 5
    Private Const GVCQC_vQCGivenBy As Integer = 6
    Private Const GVCQC_dQCGivenOn As Integer = 7
    Private Const GVCQC_vResponse As Integer = 8
    Private Const GVCQC_vResponseGivenBy As Integer = 9
    Private Const GVCQC_dResponseGivenOn As Integer = 10
    Private Const GVCQC_Response As Integer = 11
    Private Const GVCQC_Delete As Integer = 12

    'added on 5-April-10  by Deepak Singh===
    Private Const GVCPROOF_SubjectProofNo As Integer = 0
    Private Const GVCPROOF_SubjectId As Integer = 1
    Private Const GVCPROOF_TranNo As Integer = 2
    Private Const GVCPROOF_ProofType As Integer = 3
    Private Const GVCPROOF_Proofpath As Integer = 4



#End Region

#Region "Load Event"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Me.Session.Remove("Path")
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
        Dim WorkSpaceId As String = String.Empty
        Dim SubjectNo As String = String.Empty
        Dim Doctype As String = String.Empty
        Dim ActivityName As String = String.Empty
        Try
            Choice = Me.Request.QueryString("Mode")
            WorkSpaceId = Me.Request.QueryString("WorkSpaceId")
            'SubjectNo = Me.Request.QueryString("SubjectNo")
            'Doctype = Me.Request.QueryString("Doctype")
            'ActivityName = Me.Request.QueryString("ActivityName")
            'Doctype = Me.Request.QueryString("DCId")
            'ActivityName = Me.Request.QueryString("ActId")

            Me.ViewState(VS_WorkspaceId) = WorkSpaceId
            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add AndAlso _
                                Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                If Not IsNothing(Me.Request.QueryString("Value").ToString) Then
                    Me.ViewState(VS_WSSDDetailId) = Me.Request.QueryString("Value").ToString
                End If
            End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtWSSDDetail) = ds.Tables("WorkspaceSubjectDocDetails")   ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..GenCall")

        Finally

        End Try

    End Function
#End Region

#Region " GenCall_Data "
    Private Function GenCall_Data(ByRef ds_Retu As DataSet) As Boolean

        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Val As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing
        'Dim objFFR As New FFReporting.FFReporting

        Try

            Val = Me.ViewState(VS_WSSDDetailId) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Or _
                        Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                wStr = "1=2"
            Else
                wStr = "vWorkSpaceSubjectDocDetailId='" + Val.ToString + "'"
            End If


            If Not objHelp.getworkspaceSubjectDocDetails(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            If ds.Tables(0).Rows.Count <= 0 And _
             Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add AndAlso _
                   Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Throw New Exception("No Records Found For Selected Operation")
            End If

            ds_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_Data")

        Finally

        End Try
    End Function
#End Region

#Region " GenCall_showUI() "
    Private Function GenCall_ShowUI() As Boolean
        Dim dt_UserMst As DataSet = Nothing

        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try


            Page.Title = ":: Subjectwise Documents  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Subject Document Detail"

            ' Choice = Me.ViewState("Choice")
            If Not IsNothing(Me.Request.QueryString("workspaceid")) Then
                Me.ViewState(VS_WorkspaceId) = Me.Request.QueryString("workspaceid").Trim()
            End If

            If Not IsNothing(Me.Request.QueryString("SubjectNo")) Then
                Me.txtNoSub.Text = Me.Request.QueryString("SubjectNo").Trim()
            End If

            If Not IsNothing(Me.Request.QueryString("DC")) Then
                Me.TxtDocType.Text = Me.Request.QueryString("DC")
            End If

            If Not IsNothing(Me.Request.QueryString("Act")) Then
                Me.txtActivity.Text = Me.Request.QueryString("Act").Trim()
            End If

            Me.txtNoSub.Enabled = False
            Me.TxtDocType.Enabled = False
            Me.txtActivity.Enabled = False

            Me.ViewState(VS_Comment) = "TALK"
            If Not IsNothing(Me.Request.QueryString("QC")) AndAlso Me.Request.QueryString("QC").ToUpper.Trim() = "Y" Then
                Me.ViewState(VS_Comment) = "QC"
            End If

            If Not FillDropDown() Then
                Return False
            End If

            If Not IsNothing(Me.Request.QueryString("QC")) AndAlso Me.Request.QueryString("QC").ToUpper.Trim() = "Y" Then
                Me.FlUpload.Disabled = True
                Me.DDLSubId.Enabled = False
                Me.DDLStatus.Enabled = False
            End If

            'ResetPage()
            BindGrid(Me.Request.QueryString("DCId")) 'Fill Grid

            Me.BtnSave.Attributes.Add("OnClick", "return Validation();")

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....GenCall_ShowUI")
        Finally
        End Try
    End Function
#End Region

#Region "FillDropDown"
    Private Function FillDropDown() As Boolean
        Dim ds_UserGroup As New Data.DataSet
        Dim ds_DocSuBDetail As New Data.DataSet
        Dim ds_SubDetail As New Data.DataSet
        Dim ds_Status As New Data.DataSet
        Dim estr As String = String.Empty
        Dim WorkSpaceId As String = String.Empty
        Dim NodeId As String = String.Empty
        Try
            WorkSpaceId = Me.ViewState(VS_WorkspaceId)
            NodeId = Me.Request.QueryString("NodeId")

            objHelp.GetDocSubjectDetails(WorkSpaceId, ds_DocSuBDetail, estr)
            objHelp.GetViewProjectNodeUserRightsDetails("vWorkSpaceID='" & WorkSpaceId & "' and iNodeId=" & NodeId & " AND iUserId=" & Me.Session(S_UserID), _
                                    WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Status, estr)

            Me.txtNoSub.Text = ds_DocSuBDetail.Tables("ProDetail").Rows(0).Item("iNoOfSubjects")

            If Not Me.objHelp.GetViewWorkspaceSubjectMst("vWorkSpaceID='" & WorkSpaceId & "' and cRejectionFlag='N'", _
                        WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubDetail, estr) Then

                Me.ObjCommon.ShowAlert("Error while getting Data from View_WorkspaceSubjectMst", Me.Page())
                Exit Function

            End If

            Me.DDLSubId.DataSource = ds_SubDetail.Tables(0)
            Me.DDLSubId.DataValueField = "vWorkspaceSubjectId"
            Me.DDLSubId.DataTextField = "FullNameWithNo"
            Me.DDLSubId.DataBind()
            Me.DDLSubId.Items.Insert(0, New ListItem("select Subject", ""))

            Me.DDLStatus.DataSource = ds_Status.Tables(0).DefaultView.ToTable(True, "iStageId,vStageDesc".Split(","))
            Me.DDLStatus.DataValueField = "iStageId"
            Me.DDLStatus.DataTextField = "vStageDesc"
            Me.DDLStatus.DataBind()
            Me.DDLStatus.Items.Insert(0, New ListItem("Select Status", "0"))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillDropDown")
            Return False
        End Try
    End Function

#End Region

#Region "BindGrid"

    Private Sub BindGrid(ByVal docTypeCode As String)
        Dim ds_SubjectDet As New DataSet
        Dim dv_SubjectDet As DataView = Nothing
        Dim dt_SubjectDet As New DataTable
        Dim ds_SubComments As New DataSet
        Dim dv_SubComments As New DataView
        Dim estr As String = String.Empty
        Try
            If objHelp.GetView_SubjectDocDetails("vWorkSpaceId='" & Me.ViewState(VS_WorkspaceId).ToString.Trim() & _
                        "' and iNodeId='" & Me.Request.QueryString("NodeId").ToString.Trim() & "'", ds_SubjectDet, estr) Then

                If Not String.IsNullOrEmpty(docTypeCode) Then

                    dt_SubjectDet = ds_SubjectDet.Tables(0)
                    dv_SubjectDet = New DataView(dt_SubjectDet)

                    dv_SubjectDet.RowFilter = "vDocTypeCode ='" & docTypeCode & "'"

                    gvwViewSubjectDetail.DataSource = dv_SubjectDet.ToTable 'ds_SubjectDet.Tables(0).Select("vDocTypeCode = " & docTypeCode)
                    gvwViewSubjectDetail.DataBind()

                End If
            Else
                ObjCommon.ShowAlert("Cannot fetch Data", Me)
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....BindGrid")
        End Try
    End Sub

    'added on 5-Apr-10 by deepak singh to show subject's proofdetails
    Private Function BindProofGrid(ByVal SubjectID As String) As Boolean
        Dim dsSubjectProof As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Try


            wStr = "vSubjectID = '" + SubjectID + "' And cstatusindi<>'D'"

            If Not Me.objHelp.GetSubjectProofDetails(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition _
                                    , dsSubjectProof, eStr) Then
                Throw New Exception(eStr)
            End If

            GVSubjectProof.DataSource = dsSubjectProof
            GVSubjectProof.DataBind()

            Return True

        Catch ex As Exception
            ObjCommon.ShowAlert("Error Occured While Getting Subject Proof Details " + ex.Message(eStr), Me)
            Return False
        End Try
    End Function

#End Region

#Region "UpLoad File"
    Private Sub Upload(ByVal TranNo As String)
        Dim iFileSize As Integer
        'Dim DocPath As String = System.Configuration.ConfigurationManager.AppSettings("DocPath")
        Dim DocPath As String = String.Empty  'Me.Session("Path")
        'Dim DocFileSize As Integer = System.Configuration.ConfigurationManager.AppSettings("Size")
        Dim DocValidFile As String = String.Empty
        Dim NewFilePath As String = String.Empty

        DocValidFile = System.Configuration.ConfigurationManager.AppSettings("Validity")
        Dim strValidFile() As String
        Dim iCounter As Integer
        Dim bl As Boolean = False
        Dim strFileName As String = String.Empty
        Dim str As String
        Try

            If Not CreateFolder(TranNo) Then
                ObjCommon.ShowAlert("Error Occured while Creating Folder, Try Again... ", Me)
                Exit Sub
            End If

            DocPath = Me.ViewState(VS_Path).ToString.Trim()

            If Not IsNothing(FlUpload) Then

                iFileSize = FlUpload.PostedFile.ContentLength

                If iFileSize >= 1048576 Then 'Validity of Size
                    ObjCommon.ShowAlert("Error Occured, File Size Should Be Less Than 1 MB...", Me)
                End If

                strValidFile = DocValidFile.Split("#")
                For iCounter = 0 To UBound(strValidFile)

                    If strValidFile(iCounter) = Path.GetExtension(FlUpload.PostedFile.FileName) Then
                        bl = True
                        Exit For
                    End If

                Next iCounter

                If Not bl = True Then 'Validity of Document
                    ObjCommon.ShowAlert("Invalid File,Only PDF & Text File Is Allowed", Me)
                    Exit Sub
                End If

                strFileName = FlUpload.PostedFile.FileName
                str = Replace(strFileName, "\", "\\")

                FlUpload.PostedFile.SaveAs(Server.MapPath(DocPath) & Path.GetFileName(strFileName))

                Me.ViewState(VS_Path) = Me.ViewState(VS_Path).ToString.Trim() & Path.GetFileName(strFileName)
            End If

        Catch ex As Exception
            ObjCommon.ShowAlert("Error Occured While Uploading File, Try Again... ", Me)

        End Try

    End Sub
#End Region

#Region "Create New Folder"
    Private Function CreateFolder(ByVal TranNo As String) As Boolean
        Dim WsId As String = String.Empty
        Dim DCId As String = String.Empty
        Dim WorkspaceSubId As String = String.Empty
        Dim NId As String = String.Empty
        Dim TNo As String = String.Empty
        Dim ActId As String = String.Empty
        Dim SubjectDetail As String = String.Empty
        Dim dir As DirectoryInfo
        Dim Ds_NodeTran As New DataSet
        Dim estr As String = String.Empty
        Try
            WsId = Me.ViewState(VS_WorkspaceId)
            DCId = Me.Request.QueryString("DCId") 'Me.DDLDocType.SelectedValue.Trim()
            WorkspaceSubId = Me.DDLSubId.SelectedValue.Trim()
            ActId = Me.Request.QueryString("ActId") 'Me.DDLActivity.SelectedValue.Trim()
            SubjectDetail = System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail")

            NId = Me.Request.QueryString("NodeId") 'Me.DDLActivity.SelectedValue.Trim()
            TNo = TranNo

            dir = New DirectoryInfo(ObjPath.Param_FolderPath(SubjectDetail, WsId, DCId, WorkspaceSubId, NId, TNo))

            If Not dir.Exists() Then
                dir.Create()
            End If

            Me.ViewState(VS_Path) = SubjectDetail & WsId & "/" & DCId & "/" & WorkspaceSubId & "/" & NId & "/" & TNo & "/"
            'Me.Session("Path") = SubjectDetail & WsId & "/" & DCId & "/" & WorkspaceSubId  & "/" & NId & "/" & TNo & "/"
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
#End Region

#Region "Button Click"

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim ds_Save As New DataSet
        Dim dt_Save As New DataTable
        Dim estr As String = String.Empty
        Dim TranNo_Retu As String = String.Empty
        Try
            AssignValues()
            ds_Save = New DataSet
            dt_Save = CType(Me.ViewState(VS_DtWSSDDetail), DataTable)
            dt_Save.TableName = "workspaceSubjectDocDetails"
            ds_Save.Tables.Add(dt_Save)

            If Not objLambda.Save_workspaceSubjectDocDetail(Me.ViewState(VS_Choice), ds_Save, _
                    Me.Session(S_UserID), TranNo_Retu, estr) Then

                ObjCommon.ShowAlert("Error While Saving WorkspaceSubjectDocDetails", Me.Page)
                Exit Sub

            End If

            Upload(TranNo_Retu) 'To create Folder and Upload File
            ObjCommon.ShowAlert("Record Saved SuccessFully", Me.Page)

            ObjCommon.ShowAlert(Me.ViewState(VS_Path), Me.Page)

            BindGrid(Me.Request.QueryString("DCId")) 'Fill Grid

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....BtnSave_Click")
        End Try
    End Sub

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Dim DMS As String = String.Empty
        ResetPage()
        Me.Response.Redirect(Me.Request.QueryString("Page2").Trim() & ".aspx?WorkSpaceId=" & Me.Request.QueryString("WorkSpaceId").Trim() & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Type=" & Me.Request.QueryString("Type") & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))
        'Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub BtnChangeStatus_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds_WSSDD As New DataSet
        Dim ds_SAVEStatus As New DataSet
        Dim dt_SAVEStatus As New DataTable
        Dim dv_Status As New DataView
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim TranNo_Retu As String = String.Empty
        Dim dr As DataRow
        Wstr = "vWorkSpaceSubjectDocDetailId='" + Me.ViewState(VS_WSSDDetailId).ToString.Trim() + "'"

        If Not Me.objHelp.getworkspaceSubjectDocDetails(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_WSSDD, estr) Then
            ObjCommon.ShowAlert("Error While Getting Data For Change Status.", Me.Page)
            Exit Sub
        End If
        'vWorkSpaceSubjectDocDetailId,vWorkSpaceId,iNodeId,vActivityId,vWorkspaceSubjectId,vDocTypeCode,vDocLink,iTranNo,iStageId,iUploadedBy,dUploadedOn,iModifyBy,dModifyOn,cStatusIndi

        dt_SAVEStatus = ds_WSSDD.Tables(0)

        For Each dr In dt_SAVEStatus.Rows
            dr("iStageId") = Me.DDLStatus.SelectedValue.Trim()
            dr.AcceptChanges()
        Next dr

        dt_SAVEStatus.TableName = "workspaceSubjectDocDetails"
        dt_SAVEStatus.AcceptChanges()
        ds_SAVEStatus.Tables.Add(dt_SAVEStatus.Copy())

        If Not objLambda.Save_workspaceSubjectDocDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                                                        ds_SAVEStatus, Me.Session(S_UserID), TranNo_Retu, estr) Then

            ObjCommon.ShowAlert("Error While Changing Status.", Me.Page)
            Exit Sub

        End If

        ObjCommon.ShowAlert("Status Of Document Changed Successfully.", Me.Page)
        BindGrid(Me.Request.QueryString("DCId")) 'Fill Grid

        Me.divChangeStatus.Visible = False
    End Sub

    Protected Sub btnCloseStatus_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.divChangeStatus.Visible = False
    End Sub

#End Region

#Region "Assign values"
    Private Sub AssignValues()
        Dim dr As DataRow
        Dim dt_workspaceSubjectDocDetails As New DataTable
        Dim strPath As String = String.Empty
        Dim SubjectDetail As String = String.Empty
        Try
            SubjectDetail = System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail")

            If Not IsNothing(FlUpload.PostedFile) Then
                'Here we are not passing TranNo in a filepath. Because it will be added by procedure and will be return back to create.
                strPath = "~/" & SubjectDetail & Me.ViewState(VS_WorkspaceId).ToString.Trim() & "/" & _
                          Me.Request.QueryString("DCId") & "/" & Me.DDLSubId.SelectedValue.Trim() & "/" & _
                          Me.Request.QueryString("NodeId") & "/" & Path.GetFileName(FlUpload.PostedFile.FileName)

            End If

            dt_workspaceSubjectDocDetails = CType(Me.ViewState(VS_DtWSSDDetail), DataTable)
            dt_workspaceSubjectDocDetails.Clear()
            dr = dt_workspaceSubjectDocDetails.NewRow()
            'vWorkSpaceSubjectDocDetailId,vWorkSpaceId,iNodeId,vActivityId,vWorkspaceSubjectId,vDocTypeCode,vDocLink,iTranNo,iUploadedBy,dUploadedOn,iModifyBy
            dr("vWorkSpaceSubjectDocDetailId") = "00"
            dr("vWorkSpaceId") = Me.ViewState(VS_WorkspaceId)
            dr("iNodeId") = Me.Request.QueryString("NodeId")
            dr("vActivityId") = Me.Request.QueryString("ActId") 'Me.DDLActivity.SelectedValue.Trim()
            dr("vWorkspaceSubjectId") = Me.DDLSubId.SelectedValue.Trim()
            dr("vDocTypeCode") = Me.Request.QueryString("DCId") 'Me.DDLDocType.SelectedValue.Trim()
            dr("vDocLink") = strPath
            dr("iTranNo") = 1
            dr("iStageId") = GeneralModule.Stage_Created 'Me.DDLStatus.SelectedValue.Trim()
            dr("iUploadedBy") = Me.Session(S_UserID)
            'dr("dUploadedOn") = "00"
            dr("iModifyBy") = Me.Session(S_UserID)
            dt_workspaceSubjectDocDetails.Rows.Add(dr)
            Me.ViewState(VS_DtWSSDDetail) = dt_workspaceSubjectDocDetails

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....AssignValues")
        End Try
    End Sub
#End Region

#Region "GRIDVIEWSubjectDetail EVENTS"

    Protected Sub gvwViewSubjectDetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwViewSubjectDetail.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblsrNo As Label
            lblsrNo = CType(e.Row.FindControl("lblSrNo"), Label)
            lblsrNo.Text = e.Row.RowIndex + 1

            Dim hlnk As HyperLink
            hlnk = CType(e.Row.FindControl("hlnkDoc"), HyperLink)
            hlnk.Text = Path.GetFileName(hlnk.Text)

            'CType(e.Row.FindControl("lblSrNo"), Label).Text = e.Row.RowIndex + 1
            CType(e.Row.FindControl("LnkStatus"), LinkButton).CommandName = "CHANGE STATUS"
            CType(e.Row.FindControl("LnkStatus"), LinkButton).CommandArgument = e.Row.RowIndex

            CType(e.Row.FindControl("LnkQC"), LinkButton).CommandName = "QA"
            CType(e.Row.FindControl("LnkQC"), LinkButton).CommandArgument = e.Row.RowIndex

            CType(e.Row.FindControl("LnkProof"), LinkButton).CommandName = "PROOF"
            CType(e.Row.FindControl("LnkProof"), LinkButton).CommandArgument = e.Row.RowIndex


        End If
    End Sub

    Protected Sub gvwViewSubjectDetail_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        e.Row.Cells(GVC_Id).Visible = False
        e.Row.Cells(GVC_WorkspaceSubId).Visible = False
        e.Row.Cells(GVC_StageDesc).Visible = False
        e.Row.Cells(GVC_ChangeStatus).Visible = False
        e.Row.Cells(GVC_ProjectNo).Visible = False
        e.Row.Cells(GVC_SubjectId).Visible = False

    End Sub

    Protected Sub gvwViewSubjectDetail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim INDEX As Integer = CType(e.CommandArgument, Integer)
        Dim wStr As String = String.Empty
        Dim ds_EmailAlert As New DataSet
        Dim eStr As String = String.Empty

        Me.ViewState(VS_WSSDDetailId) = gvwViewSubjectDetail.Rows(INDEX).Cells(GVC_Id).Text.Trim
        Me.ViewState(VS_SubjectId) = gvwViewSubjectDetail.Rows(INDEX).Cells(GVC_WorkspaceSubId).Text.Trim
        Me.ViewState(VS_SubjectName) = gvwViewSubjectDetail.Rows(INDEX).Cells(GVC_FullName).Text.Trim
        HfProjectNo.Value = gvwViewSubjectDetail.Rows(INDEX).Cells(GVC_ProjectNo).Text.Trim

        If e.CommandName.ToUpper.Trim() = "QA" Then

            If Not Me.fillQCGrid() Then

                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    Me.gvwViewSubjectDetail.Rows(INDEX).Cells(GVC_QC).Enabled = False
                    Exit Sub
                End If

            End If

            wStr = "nEmailAlertId =" + Email_QCOFMedexINfoHdr.ToString() + " And cStatusIndi <> 'D'"
            If Not Me.objHelp.GetEmailAlertMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_EmailAlert, eStr) Then

                Me.ObjCommon.ShowAlert("Error While Getting Data From EmailAlert : " + eStr, Me.Page)
                Exit Sub

            End If

            If ds_EmailAlert.Tables(0).Rows.Count > 0 Then

                Me.txtToEmailId.Text = ds_EmailAlert.Tables(0).Rows(0)("vToUserEmailId").ToString()
                Me.txtCCEmailId.Text = ds_EmailAlert.Tables(0).Rows(0)("vCCUserEmailId").ToString()

            End If

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHide", "QCDivShowHide('ST');", True)

        ElseIf e.CommandName.ToUpper.Trim() = "PROOF" Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHide", "ProofDivShowHide('S');", True)
            If Not BindProofGrid(gvwViewSubjectDetail.Rows(INDEX).Cells(GVC_SubjectId).Text.Trim) Then
                Me.ObjCommon.ShowAlert("Error While getting Subject Proof Details : " + eStr, Me.Page)
                Exit Sub
            End If


        ElseIf e.CommandName.ToUpper.Trim() = "CHANGE STATUS" Then
            Me.divChangeStatus.Visible = True

        End If

    End Sub

    Protected Sub GVSubjectComments_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lblComSrNo"), Label).Text = e.Row.RowIndex + 1
        End If
    End Sub




#End Region

#Region "GridviewSubjectProofdetail Events"

    Protected Sub GVSubjectProof_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GVSubjectProof.RowCommand
        Dim index As Integer = e.CommandArgument
    End Sub

    Protected Sub GVSubjectProof_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVSubjectProof.RowCreated
        e.Row.Cells(GVCPROOF_SubjectProofNo).Visible = False
        e.Row.Cells(GVCPROOF_TranNo).Visible = False
        e.Row.Cells(GVCPROOF_Proofpath).Visible = False
    End Sub

    Protected Sub GVSubjectProof_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVSubjectProof.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim hlnk As HyperLink
                hlnk = CType(e.Row.FindControl("hlnkFile"), HyperLink)
                hlnk.Text = Path.GetFileName(hlnk.Text)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......GVSubjectProof_RowDataBound")
        End Try
    End Sub
#End Region

#Region "Reset Page"
    Private Sub ResetPage()
        'Me.DDLActivity.SelectedIndex = 0
        'Me.DDLDocType.SelectedIndex = 0
        'Me.txtActivity.Text = ""
        'Me.TxtDocType.Text = ""
        Me.DDLSubId.SelectedIndex = 0
        Me.ViewState(VS_DtWSSDDetail) = Nothing
        Me.ViewState(VS_Path) = Nothing

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

#Region "Div Section"

#Region "Div Button Events"

    Protected Sub BtnQCSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim estr As String = String.Empty
        Dim ds_QC As New DataSet
        Dim QCMsg As String = String.Empty
        Dim wStr As String = String.Empty
        Try



            If Me.lblResponse.Text = "" And CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.ObjCommon.ShowAlert("Please Click Response Button Of Particular QA Comments Against Which You Want To Response", Me.Page)

                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHide", "QCDivShowHide('ST');", True)

                Exit Sub
            End If

            If Not AssignValues_WorkspaceSubjectComment(ds_QC) Then
                Me.ObjCommon.ShowAlert("Error While Assigning Data.", Me.Page)
                Exit Sub
            End If

            'Added for QC Comments
            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

                If Not Me.objLambda.Save_InsertworkspaceSubjectComment(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_QC, "", Me.Session(S_UserID), estr) Then

                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Exit Sub

                End If

                Me.ObjCommon.ShowAlert("QA Comments Saved Successfully", Me.Page)

            Else 'For Response

                If Not Me.objLambda.Save_InsertworkspaceSubjectComment(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_QC, "", Me.Session(S_UserID), estr) Then

                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Exit Sub

                End If

                Me.ObjCommon.ShowAlert("Response Saved Successfully", Me.Page)

            End If

            If Not fillQCGrid() Then
                Exit Sub
            End If

            BindGrid(Me.Request.QueryString("DCId")) 'Fill Grid

            Me.txtQCRemarks.Value = ""
            Me.lblResponse.Text = ""
            Me.RBLQCFlag.SelectedValue = "F"

            Me.txtToEmailId.Text = ""
            Me.txtCCEmailId.Text = ""

            '***********************

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

    Protected Sub BtnQCSaveSend_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim estr As String = String.Empty
        Dim ds_QC As New DataSet
        Dim QCMsg As String = String.Empty
        Dim fromEmailId As String = String.Empty
        Dim toEmailId As String = String.Empty
        Dim password As String = String.Empty
        Dim ccEmailId As String = String.Empty
        Dim SubjectLine As String = String.Empty
        Dim wStr As String = String.Empty
        'Dim ds_EmailAlert As New DataSet
        Try



            If Me.lblResponse.Text = "" And CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.ObjCommon.ShowAlert("Please Click Response Button Of Particular QA Comments Against Which You Want To Response", Me.Page)

                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHide", "QCDivShowHide('S');", True)

                Exit Sub
            End If

            If Not AssignValues_WorkspaceSubjectComment(ds_QC) Then
                Me.ObjCommon.ShowAlert("Error While Assigning Data.", Me.Page)
                Exit Sub
            End If

            'For Sending Mail
            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

                QCMsg = "QA On Screening of " + Me.ViewState(VS_SubjectName) + " (" + Me.ViewState(VS_SubjectId) + ") of Project " + HfProjectNo.Value + _
                                        "<br/><br/>QA : " + Me.RBLQCFlag.SelectedItem.Text.Trim() + _
                                        "<br/><br/>QA Comments: " + Me.txtQCRemarks.Value + "<br/><br/>" + _
                                        "Comments Given By : " + Me.Session(S_FirstName).ToString.Trim() + " " + Me.Session(S_LastName).ToString.Trim()

                SubjectLine = "QA Comments On Screening of Project " + HfProjectNo.Value 'ds_EmailAlert.Tables(0).Rows(0)("vSubjectLine").ToString()

            Else

                QCMsg = "Response of QA Comments On Screening of " + Me.ViewState(VS_SubjectName) + " (" + Me.ViewState(VS_SubjectId) + ") of Project " + HfProjectNo.Value + _
                        "<br/><br/>" + Me.lblResponse.Text.Trim() + "<br/><br/>" + _
                        "<br/><br/>Response: " + Me.txtQCRemarks.Value + "<br/><br/>" + _
                        "Response Given By : " + Me.Session(S_FirstName).ToString.Trim() + " " + Me.Session(S_LastName).ToString.Trim()

                SubjectLine = "Response of QA Comments On Screening of Project " + HfProjectNo.Value

            End If

            fromEmailId = ConfigurationSettings.AppSettings("Username")
            password = ConfigurationSettings.AppSettings("Password")


            'wStr = "nEmailAlertId =" + Email_QCOFSCREENING.ToString() + " And cStatusIndi <> 'D'"
            'If Not Me.objHelp.GetEmailAlertMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '                    ds_EmailAlert, estr) Then

            '    Me.ObjCommon.ShowAlert("Error While Getting Data From EmailAlert : " + estr, Me.Page)
            '    Exit Sub

            'End If

            'If ds_EmailAlert.Tables(0).Rows.Count > 0 Then

            toEmailId = Me.txtToEmailId.Text.Trim() 'ds_EmailAlert.Tables(0).Rows(0)("vToUserEmailId").ToString()
            ccEmailId = Me.txtCCEmailId.Text.Trim() 'ds_EmailAlert.Tables(0).Rows(0)("vCCUserEmailId").ToString()

            Dim sn As New SendMail(Server, Request, Session, SubjectLine, QCMsg, fromEmailId, password, toEmailId, ccEmailId)

            'Changed on 26-Aug-2009
            If Not sn.Send(Server, Response, Session, , fromEmailId, password) Then
                Me.ObjCommon.ShowAlert("Record Saved Successfully But Email Is Not Sent Due To Some Reason", Me.Page)
                Exit Sub
            End If
            '****************************************************

            sn = Nothing

            'Added for QC Comments
            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

                If Not Me.objLambda.Save_InsertworkspaceSubjectComment(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_QC, "", Me.Session(S_UserID), estr) Then

                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Exit Sub

                End If

                Me.ObjCommon.ShowAlert("QA Comments Saved Successfully", Me.Page)

                'Else
                'Me.ObjCommon.ShowAlert("QA Comments Saved Successfully But Email Is Not Sent Due To Some Reason", Me.Page)
                'End If

            Else 'For Response

                If Not Me.objLambda.Save_InsertworkspaceSubjectComment(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_QC, "", Me.Session(S_UserID), estr) Then

                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Exit Sub

                End If

                Me.ObjCommon.ShowAlert("Response Saved Successfully", Me.Page)

            End If

            If Not fillQCGrid() Then
                Exit Sub
            End If

            BindGrid(Me.Request.QueryString("DCId")) 'Fill Grid

            Me.txtQCRemarks.Value = ""
            Me.lblResponse.Text = ""
            Me.RBLQCFlag.SelectedValue = "F"

            Me.txtToEmailId.Text = ""
            Me.txtCCEmailId.Text = ""

            '***********************

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....BtnQCSaveSend_Click")
        End Try
    End Sub

#End Region

#Region "AssignValues_WorkspaceSubjectComment"

    Private Function AssignValues_WorkspaceSubjectComment(ByRef DsSave As DataSet) As Boolean
        Dim DtMedExScreeningHdr As New DataTable
        Dim ObjId As String = String.Empty
        Dim dr As DataRow
        Dim TranNo As Integer = 0
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds_MEdexScreeningHdrQC As New DataSet
        Dim dtMEdexScreeningHdrQC As New DataTable
        Try
            DtMedExScreeningHdr = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable)

            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then


                If Not Me.objHelp.getWorkspaceSubjectComment(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_MEdexScreeningHdrQC, estr) Then
                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Return False
                End If
                dtMEdexScreeningHdrQC = ds_MEdexScreeningHdrQC.Tables(0)
                dr = dtMEdexScreeningHdrQC.NewRow
                'nWorkSpaceSubjectCommentNo,vWorkSpaceSubjectDocDetailId,iTranNo,vQCComment,cQCFlag,iModifyBy
                'dModiFyOn,cStatusIndi,iQCGivenBy,vResponse,iResponseGivenBy,dQCGivenOn,dResponseGivenOn
                dr("nWorkSpaceSubjectCommentNo") = 1
                dr("vWorkSpaceSubjectDocDetailId") = Me.ViewState(VS_WSSDDetailId)
                dr("iTranNo") = 1
                dr("vQCComment") = Me.txtQCRemarks.Value.Trim()
                dr("cQCFlag") = Me.RBLQCFlag.SelectedValue.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dr("iQCGivenBy") = Me.Session(S_UserID)
                dr("dQCGivenOn") = Date.Now.ToString("dd-MMM-yyyy hh:mm tt")
                dtMEdexScreeningHdrQC.Rows.Add(dr)

            ElseIf CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete Then


                dtMEdexScreeningHdrQC = Me.ViewState(VS_DtQC)

                For Each dr In dtMEdexScreeningHdrQC.Rows
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "D"
                    dr.AcceptChanges()
                Next



            Else 'For Response

                    dtMEdexScreeningHdrQC = Me.ViewState(VS_DtQC)

                    For Each dr In dtMEdexScreeningHdrQC.Rows
                        dr("iModifyBy") = Me.Session(S_UserID)
                        dr("cStatusIndi") = "E"
                        dr("vResponse") = Me.txtQCRemarks.Value.Trim()
                        dr("iResponseGivenBy") = Me.Session(S_UserID)
                    dr("dResponseGivenOn") = Date.Now.ToString("dd-MMM-yyyy hh:mm tt")

                        dr.AcceptChanges()
                    Next dr

            End If


            dtMEdexScreeningHdrQC.AcceptChanges()

            dtMEdexScreeningHdrQC.TableName = "WorkspaceSubjectComment"
            dtMEdexScreeningHdrQC.AcceptChanges()

            DsSave.Tables.Add(dtMEdexScreeningHdrQC.Copy())
            DsSave.AcceptChanges()
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....AssignValues_WorkspaceSubjectComment")
            Return False
        End Try

    End Function

#End Region

#Region "fillQCGrid"

    Private Function fillQCGrid() As Boolean
        Dim Ds_QCGrid As New DataSet
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Try

            'CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            Wstr = "vWorkSpaceSubjectDocDetailId='" & Me.ViewState(VS_WSSDDetailId) & "' and cStatusIndi <> 'D'"

            If Not Me.objHelp.GetViewWorkSpaceSubjectComment(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        Ds_QCGrid, estr) Then
                Me.ShowErrorMessage(estr, estr)
                Return False
            End If

            Me.GVQCDtl.DataSource = Ds_QCGrid
            Me.GVQCDtl.DataBind()

            Me.BtnSave.Visible = True

            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View And _
                Ds_QCGrid.Tables(0).Rows.Count <= 0 Then

                Me.BtnSave.Visible = False
                Exit Function

            End If

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....fillQCGrid")
            Return False

        End Try

    End Function

#End Region

#Region "GVQCDtl Grid Events"

    Protected Sub GVQCDtl_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVQCDtl.RowCreated
        e.Row.Cells(GVCQC_nWorkSpaceSubjectCommentNo).Visible = False
        e.Row.Cells(GVCQC_vWorkSpaceSubjectDocDetailId).Visible = False
        e.Row.Cells(GVCQC_cQCFlag).Visible = False

        e.Row.Cells(GVCQC_Response).Visible = True

        If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

            e.Row.Cells(GVCQC_Response).Visible = False
        ElseIf Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
            e.Row.Cells(GVCQC_Delete).visible = False
        End If


    End Sub

    Protected Sub GVQCDtl_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVQCDtl.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lnkResponse"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkResponse"), LinkButton).CommandName = "Response"
            CType(e.Row.FindControl("lnkResponse"), LinkButton).OnClientClick = "return QCDivShowHide('ST');"

            CType(e.Row.FindControl("ImgDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImgDelete"), ImageButton).CommandName = "Delete"
        End If

    End Sub

    Protected Sub GVQCDtl_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GVQCDtl.RowCommand
        Dim index As Integer = CType(e.CommandArgument, Integer)
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds_MEdexInfroHdrQC As New DataSet
        Dim ds_workspaceSubjectComment As New DataSet

        Wstr = "nWorkSpaceSubjectCommentNo=" + Me.GVQCDtl.Rows(index).Cells(GVCQC_nWorkSpaceSubjectCommentNo).Text.Trim()

        If Not Me.objHelp.getWorkspaceSubjectComment(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                ds_MEdexInfroHdrQC, estr) Then

            Me.ObjCommon.ShowAlert(estr, Me.Page)
            Exit Sub

        End If

        Me.ViewState(VS_DtQC) = ds_MEdexInfroHdrQC.Tables(0)

        If e.CommandName.ToUpper.Trim() = "RESPONSE" Then

            Me.lblResponse.Text = "Response To: " + Me.GVQCDtl.Rows(index).Cells(GVCQC_vQCComment).Text.Trim()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHide", "QCDivShowHide('ST');", True)

        ElseIf e.CommandName.ToUpper.Trim() = "DELETE" Then
            ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete

            If Not AssignValues_WorkspaceSubjectComment(ds_MEdexInfroHdrQC) Then
                Me.ShowErrorMessage("", "Error while assigning values")
                Exit Sub
            End If

            If Not Me.objLambda.Save_InsertworkspaceSubjectComment(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_workspaceSubjectComment, "", Me.Session(S_UserID), estr) Then

                Me.ObjCommon.ShowAlert(estr, Me.Page)
                Exit Sub

            End If

            Me.ObjCommon.ShowAlert("Comment Deleted  Successfully", Me.Page)

            ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View

        End If

    End Sub

#End Region

#End Region


    Protected Sub GVQCDtl_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GVQCDtl.RowDeleting

    End Sub
End Class
