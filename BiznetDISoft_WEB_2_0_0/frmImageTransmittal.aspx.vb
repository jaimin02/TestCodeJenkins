Imports System.Web.Services
Imports System.Configuration
Imports Dicom
Imports Newtonsoft.Json
Imports Microsoft.Graph
Imports System.Net


Partial Class frmImageTransmittal
    Inherits System.Web.UI.Page
    Private objCommon As New clsCommon
    Private _items As Dictionary(Of String, Object)
    Private _selectedItems As Dictionary(Of String, Object)

    Public Property Items As Dictionary(Of String, Object)
        Get
            Return _items
        End Get
        Set(ByVal value As Dictionary(Of String, Object))
            _items = value
        End Set
    End Property

    Public Property SelectedItems As Dictionary(Of String, Object)
        Get
            Return _selectedItems
        End Get
        Set(ByVal value As Dictionary(Of String, Object))
            _selectedItems = value
        End Set
    End Property
    'Friend Function GetDict(ByVal dt As DataTable) As Dictionary(Of String, Object)
    '    Return dt.AsEnumerable().ToDictionary(Of DataRow, String, Object)(Function(row) row.Field(Of String)(1), Function(row) row.Field(Of Object)(0))
    'End Function

#Region "Auto complition"
    <WebMethod>
    Public Shared Function GetMyChildProjectCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        'Dim item() As String

        Dim Wstr_ScopeValue As String = ""

        'To Get Where condition of ScopeVales( Project Type )
        If Not Objcommon.GetScopeValueWithCondition(Wstr_ScopeValue) Then
            Return Nothing
        End If

        'Changed on 01-Sept-2009
        If Not IsNothing(contextKey) AndAlso contextKey.ToString.Trim() <> "" Then
            contextKey += " And " + Wstr_ScopeValue + " AND iUserId=" + HttpContext.Current.Session(S_UserID)
        Else
            contextKey = Wstr_ScopeValue + " AND iUserId=" + HttpContext.Current.Session(S_UserID)
        End If
        '*****************************************

        Dim items As System.Collections.Generic.List(Of String) = New System.Collections.Generic.List(Of String)()

        If count = 0 Then
            count = 10
        End If

        Dim ds As New DataSet()
        Dim estr As String = ""
        Dim result As Boolean = False
        Dim dr As DataRow = Nothing
        Dim whereCondition As String = "cWorkspaceType = 'C' and vRequestId  + vProjectNo " + " Like '%"
        whereCondition += prefixText + "%'" + IIf(contextKey.Trim() <> "", " AND " & contextKey.Trim(), "")


        result = DBHelp.GetFieldsOfTable("View_MyProjects", " * ",
            whereCondition, ds, estr)

        For index As Integer = 0 To ds.Tables(0).Rows.Count - 1
            dr = ds.Tables(0).Rows(index)
            'items.Add("'" + dr.Item("vWorkspaceid").ToString + "#" + dr.Item("vWorkspaceDesc").ToString + _
            '    "#" + dr.Item("vProjectNo").ToString + "#" + _
            '    dr.Item("vClientName").ToString + "#" + dr.Item("vRequestId").ToString())

            'items.Add("'" + dr.Item("vWorkspaceid").ToString + "#" + dr.Item("vProjectNo").ToString + "#" + dr.Item("vRequestId").ToString() + "#" + dr.Item("ParentWorkspaceId").ToString.Trim())
            items.Add("'" + dr.Item("vWorkspaceid").ToString + "#" + dr.Item("vProjectNo").ToString + "#" + dr.Item("vRequestId").ToString())

        Next

        Return items.ToArray()
        'Return item

    End Function

#End Region

#Region "DefualtSiteNo set"
    <WebMethod>
    Public Shared Function DefualtSiteNo() As String
        Dim QStr As String = String.Empty
        Dim ds_Workspace As New DataSet
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Try
            QStr = " Select WorkspaceDefaultWorkflowUserDtl.vWorkspaceId,'[' +vProjectNo+ ']' + vRequestId AS ProjectNo " + vbCrLf +
                   " FROM WorkspaceDefaultWorkflowUserDtl " + vbCrLf +
                   " INNER JOIN view_workspaceprotocoldetail " + vbCrLf +
                   "        ON(WorkspaceDefaultWorkflowUserDtl.vWorkspaceId = view_workspaceprotocoldetail.vWorkspaceId)" + vbCrLf +
                   " WHERE iUserId = " + HttpContext.Current.Session(S_UserID) + " GROUP BY WorkspaceDefaultWorkflowUserDtl.vWorkspaceId,'[' +vProjectNo+ ']' + vRequestId"

            ds_Workspace = objHelp.GetResultSet(QStr, "ImgTransmittalHdr")

            Return ConvertDataTabletoString(ds_Workspace.Tables(0))
        Catch ex As Exception
            Throw New Exception(ex.Message + vbCrLf + "Error in FillGrid.")
        End Try

    End Function
#End Region

#Region "Button Events"
    <WebMethod>
    Public Shared Function btnSetProject(ByVal HProjectId As String) As String

        Dim ds_GetselectedChild As DataSet = Nothing
        Dim dt As New DataTable
        Dim ProjectStatus As String = String.Empty
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim iTran As String = String.Empty
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()

        Try


            'ds_GetselectedChild = objHelp.GetResultSet("Select+'['+ vProjectNo +'] ' + vRequestId AS ProjectNo, vWorkspaceId,vParentWorkspaceId,vProjectNo,vExternalProjectNo  From View_ChildProject where vWorkspaceId ='" + HProjectId.Trim() + "' order by vWorkspaceId asc", "View_ChildProject ")

            ds_GetselectedChild = objHelp.GetResultSet("Select + '[' + WorkSpaceProtocolDetail.vProjectNo + ']' + Workspacemst.vRequestId AS ProjectNo, Workspacemst.vWorkSpaceId AS vWorkspaceId, vParentWorkspaceId, WorkSpaceProtocolDetail.vProjectNo, ProtocolNoDtl.vProjectNo AS vStudyNo FROM Workspacemst INNER JOIN WorkSpaceProtocolDetail ON ( Workspacemst.vWorkSpaceId = WorkSpaceProtocolDetail.vWorkSpaceId ) LEFT JOIN WorkSpaceProtocolDetail AS ProtocolNoDtl WITH(NOLOCK) ON ProtocolNoDtl.vWorkSpaceId=workspacemst.vParentWorkspaceId WHERE Workspacemst.cStatusIndi <> 'D' and Workspacemst.vWorkspaceId ='" + HProjectId.Trim() + "'order by vWorkspaceId asc", "Workspacemst")
            dt = ds_GetselectedChild.Tables(0)

            Return ConvertDataTabletoString(dt) + "#" + HttpContext.Current.Session(S_UserName) + "#" + HttpContext.Current.Session(S_UserID)
        Catch ex As Exception
            '   ShowErrorMessage(ex.Message, "...........btnSetProject_Click")
        End Try

    End Function
#End Region

    <WebMethod>
    Public Shared Function FillModalityDropDown() As String
        Dim ds_modality As New Data.DataSet
        Dim dv_modality As New Data.DataView
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()

        Try
            ds_modality = objHelp.GetResultSet("Select * From View_GetModalityMst", "View_GetModalityMst")
            If ds_modality Is Nothing OrElse ds_modality.Tables.Count <= 0 OrElse ds_modality.Tables(0).Rows.Count <= 0 Then
                Exit Function
            End If

            dv_modality = ds_modality.Tables(0).DefaultView
            dv_modality.RowFilter = "cStatusIndi <> 'D'"

            If dv_modality Is Nothing OrElse dv_modality.Count <= 0 Then
                Exit Function
            End If

            Return ConvertDataTabletoString(dv_modality.ToTable)
        Catch ex As Exception

        End Try
    End Function

    <WebMethod>
    Public Shared Function FillAnatomyDropDown() As String
        Dim ds_Anatomy As New Data.DataSet
        Dim dv_Anatomy As New Data.DataView
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()

        Try
            ds_Anatomy = objHelp.GetResultSet("Select * From View_GetAnatomyMst", "View_GetAnatomyMst")
            If ds_Anatomy Is Nothing OrElse ds_Anatomy.Tables.Count <= 0 OrElse ds_Anatomy.Tables(0).Rows.Count <= 0 Then
                Exit Function
            End If

            Dim dt = New DataTable()
            dv_Anatomy = ds_Anatomy.Tables(0).DefaultView
            dv_Anatomy.RowFilter = "cStatusIndi <> 'D'"
            dt = dv_Anatomy.ToTable()

            If (dt.Rows.Count <> 0) Then

            End If

            If dv_Anatomy Is Nothing OrElse dv_Anatomy.Count <= 0 Then
                Exit Function
            End If

            Return ConvertDataTabletoString(dv_Anatomy.ToTable)
        Catch ex As Exception

        End Try
    End Function

    <WebMethod>
    Public Shared Function FillSubjectDropDown(ByVal HProjectId As String) As String
        Dim ds_subject As New Data.DataSet
        Dim dv_subject As New Data.DataView
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Try
            ds_subject = objHelp.GetResultSet("Select * From View_Subject where vWorkspaceId ='" + HProjectId.Trim() + "' AND vInitials <> 'OOOO'  ORDER BY vSubjectId", "View_Subject ")

            If ds_subject Is Nothing OrElse ds_subject.Tables.Count <= 0 OrElse ds_subject.Tables(0).Rows.Count <= 0 Then
                Exit Function
            End If
            'If ds_subject.Tables(0).Rows(0)("cApprovedSite").ToString = "Y" Then
            dv_subject = ds_subject.Tables(0).DefaultView
            dv_subject.RowFilter = "vInitials <> 'OOOO'"

            If dv_subject Is Nothing OrElse dv_subject.Count <= 0 Then
                Exit Function
            End If
            Return ConvertDataTabletoString(dv_subject.ToTable)
            'End If

            'Return ConvertDataTabletoString(ds_subject.Tables(0))
            'Me.gvwCertificate.DataSource = ds_Certificate.Tables(0).DefaultView.ToTable()
            'Me.gvwCertificate.DataBind()
            'If gvwCertificate.Rows.Count > 0 Then
            '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvwCertificate", "UIgvwCertificate(); ", True)
            'End If

        Catch ex As Exception
            Throw New Exception(ex.Message + vbCrLf + "Error in FillGrid.")
        End Try

    End Function

    <WebMethod>
    Public Shared Function FillVisitDropDown(ByVal HProjectId As String, ByVal SubjectId As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_Visit As New DataSet
        Dim dv_Visit As New DataView
        Dim dt_Visit As New DataTable
        Dim estr As String = String.Empty
        'Dim MARK As String = ConfigurationManager.AppSettings.Item("MARK").Trim()
        'Dim HiddenVisit As String = ConfigurationManager.AppSettings.Item("HiddenVisit").Trim()
        'Dim GLOBAL_RESPONSE As String = ConfigurationManager.AppSettings.Item("GLOBAL_RESPONSE").Trim()
        'Dim ADJUDICATOR As String = ConfigurationManager.AppSettings.Item("ADJUDICATOR").Trim()

        Try
            wStr = HProjectId.Trim().ToString() + "##" + SubjectId.Trim().ToString()
            ds_Visit = objHelp.ProcedureExecute("Proc_GetProjectVisitDetails", wStr)
            dv_Visit = ds_Visit.Tables(0).DefaultView
            'dv_Visit.RowFilter = "vNodeDisplayName Not In ('BL','" + MARK + "','" + GLOBAL_RESPONSE + "','" + ADJUDICATOR + "','" + HiddenVisit + "')"
            dt_Visit = dv_Visit.ToTable()
            Return ConvertDataTabletoString(dt_Visit)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
#Region "PopUp Password check"
    <WebMethod>
    Public Shared Function ValidatePassword(ByVal Password As String) As Boolean

        Dim pwd As String = String.Empty
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        'pwd = Me.txtPassword.Text
        pwd = objHelp.EncryptPassword(Password)


        If (pwd.ToString() = "") Then
            'Me.txtPasswords.Focus()
            'ObjCommon.ShowAlert("Please Enter Password !", Me.Page)
            Return False
        End If

        If Convert.ToString(HttpContext.Current.Session(S_Password)) <> pwd.ToString() Then
            'ObjCommon.ShowAlert("Password Authentication Fails !", Me.Page)
            'Me.txtPasswords.Focus()
            Return False
        End If
        Return True
    End Function
#End Region

#Region "Fill Grid"
    <WebMethod>
    Public Shared Function FillGrid(ByVal WorkspaceId As String, ByVal SubjectId As String) As String
        Dim ds_FillGrid As New Data.DataSet
        Dim wStr As String = String.Empty
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Try
            wStr = WorkspaceId.Trim().ToString() + "##" + SubjectId.Trim().ToString()
            ds_FillGrid = objHelp.ProcedureExecute("Get_ImageTransmittalDetails", wStr)
            'ds_Visit = objHelp.GetResultSet("SELECT * FROM ImageTransmittalImgDtl WHERE vWorkspaceId = '" + WorkspaceId.Trim() + "' AND vSubjectId = '" + SubjectId.Trim() + "'", "ImageTransmittalImgDtl ")

            If ds_FillGrid Is Nothing OrElse ds_FillGrid.Tables.Count <= 0 OrElse ds_FillGrid.Tables(0).Rows.Count <= 0 Then
                Exit Function
            End If
            Return ConvertDataTabletoString(ds_FillGrid.Tables(0))
            'Me.gvwCertificate.DataSource = ds_Certificate.Tables(0).DefaultView.ToTable()
            'Me.gvwCertificate.DataBind()
            'If gvwCertificate.Rows.Count > 0 Then
            '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvwCertificate", "UIgvwCertificate(); ", True)
            'End If
        Catch ex As Exception
            Throw New Exception(ex.Message + vbCrLf + "Error in FillGrid.")
        End Try

    End Function
#End Region

    <WebMethod>
    Public Shared Function AuditTrail(ByVal iImgTransmittalHdrId As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_Certificate As New DataSet
        Dim estr As String = String.Empty

        Try
            wStr = " iImgTransmittalHdrId = '" + iImgTransmittalHdrId + "'"
            If Not objHelp.GetData("View_ImgTransmittalAuditTrail", "*", wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Certificate, estr) Then
                Throw New Exception(estr)
            End If
            Return ConvertDataTabletoString(ds_Certificate.Tables(0))
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    <WebMethod>
    Public Shared Function EditQuery(ByVal vWorkSpaceId As String, ByVal vSubjectId As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_Certificate As New DataSet
        Dim estr As String = String.Empty


        Try
            wStr = " vWorkSpaceId = '" + vWorkSpaceId + "' and vSubjectId ='" + vSubjectId + " ' AND cStatusIndi <> 'D'"
            If Not objHelp.GetData("QueryMaster", "*", wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Certificate, estr) Then
                Throw New Exception(estr)
            End If
            Return JsonConvert.SerializeObject(ds_Certificate.Tables(0))
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    <WebMethod>
    Public Shared Function VisitValidation(ByVal iProjectId As String, ByVal iSubjectId As String, ByVal visitName As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_ImgTrData As New DataSet
        Dim dv As New DataView


        Try
            'wStr = " iImgTransmittalHdrId = '" + iImgTransmittalHdrId + "'"
            ds_ImgTrData = objHelp.GetResultSet("Select * from View_ImgTransmittalAuditTrail where vWorkspaceId = '" + iProjectId.Trim() + "' AND vSubjectId = '" + iSubjectId.Trim() + "'", "View_ImgTransmittalAuditTrail")
            dv = ds_ImgTrData.Tables(0).DefaultView
            dv.RowFilter = "vNodeDisplayName = '" + visitName + "'"
            dv.Sort = "iImgTransmittalDtlID desc"

            Return ConvertDataTabletoString(dv.ToTable())
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

#Region "DataTable to Json Convert"
    <WebMethod>
    Public Shared Function ConvertDataTabletoString(ByVal dt As DataTable) As String
        'convert the filled DT into JSON
        Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim rows As New List(Of Dictionary(Of String, Object))()
        Dim row As Dictionary(Of String, Object)
        For Each dr As DataRow In dt.Rows
            row = New Dictionary(Of String, Object)()
            For Each col As DataColumn In dt.Columns
                row.Add(col.ColumnName, dr(col))
            Next
            rows.Add(row)
        Next
        Return serializer.Serialize(rows)
    End Function
#End Region

#Region "Dicom Tag Edit/Delete"
    <WebMethod>
    Public Shared Function DicomTagChange(ByVal fileName As String, ByVal fromPath As String, ByVal subjectValue As String) As String
        Try
            Dim dirPath = fromPath
            Dim filePath = Path.Combine(dirPath, fileName)

            'Dim backupFolder = dirPath + "BackupFiles"

            'If Not Directory.Exists(backupFolder) Then
            '    Directory.CreateDirectory(backupFolder)
            'End If

            'File.Copy(filePath, Path.Combine(backupFolder, fileName), True)

            Dim openedDicom As DicomFile = DicomFile.Open(filePath, FileReadOption.ReadAll)

            'Dim ArrTag() As String = {"0008,0050", "0008,0081", "0008,0090", "0008,1050", "0010,1001", "0010,1002", "0010,1040"}
            ''Dim remove As DicomTag
            'For index = 0 To ArrTag.Length - 1
            '    'remove = DicomTag.Parse(ArrTag(index).ToString)
            '    If openedDicom.Dataset.Get(DicomTag.Parse(ArrTag(index).ToString)) Then
            '        openedDicom.Dataset.AddOrUpdate(ArrTag(index).ToString, " ")
            '    End If
            'Next

            'openedDicom.Dataset.GetSingleValue(Of String)(DicomTag.Parse("0010,1002"))
            'openedDicom.Dataset.GetCodeItem(DicomTag.Parse("0010,1002"))
            'openedDicom.Dataset.GetCodeItem(DicomTag.Parse("0010,1002")).GetHashCode()
            'openedDicom.Dataset.GetHashCode(,)

            'If openedDicom.Dataset.GetSingleValue(Of String)("0008,0050") Then
            '    Console.WriteLine("0008,0050")
            'End If

            'Dim val As Integer = Nothing

            Dim edit = DicomTag.Parse("0010,0010")
            openedDicom.Dataset.AddOrUpdate(edit, subjectValue)

            openedDicom.Dataset.AddOrUpdate(DicomTag.Parse("0008,0050"), "")
            openedDicom.Dataset.AddOrUpdate(DicomTag.Parse("0008,0081"), "")
            openedDicom.Dataset.AddOrUpdate(DicomTag.Parse("0008,0090"), "")
            openedDicom.Dataset.AddOrUpdate(DicomTag.Parse("0008,1050"), "")
            openedDicom.Dataset.AddOrUpdate(DicomTag.Parse("0010,1001"), "")
            'openedDicom.Dataset.AddOrUpdate(DicomTag.Parse("0010,1002"), val)
            openedDicom.Dataset.AddOrUpdate(DicomTag.Parse("0010,1040"), "")
            openedDicom.Save(filePath)
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Console.WriteLine(ex.ToString)
            Console.WriteLine(ex.Message)
        End Try
    End Function

    <WebMethod>
    Public Shared Function CheckVisitStatus(ByVal HProjectId As String, ByVal SubjectId As String, ByVal VisitName As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_Visit As New DataSet
        Dim estr As String = String.Empty
        Dim estr_retu As String = String.Empty
        Try
            wStr = "vWorkSpaceId = '" + HProjectId + "' AND vSubjectId='" + SubjectId + "' AND ImageReviewStatus <> ''" +
                       " AND Visit = '" + VisitName + "'"
            objHelp.GetData("View_GetVisitStatus", "*", wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Visit, estr_retu)

            Return JsonConvert.SerializeObject(ds_Visit)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
#End Region


#Region "Send Mail for Upload Inage"

    <WebMethod>
    Public Shared Function SendEmail(ByVal vWorkSpaceID As String, ByVal ProjectNo As String, ByVal ScreenNo As String, ByVal vParentWorkSpaceId As String,
                                     ByVal VisitNo As String, ByVal study As String, ByVal vRemark As String, ByVal PreformedDate As String, ByVal PationtInitial As String,
                                     ByVal RandomizationNo As String, ByVal iImgTransmittalDtlId As String) As String

        Dim objcommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
        Dim myHtmlFile As String = String.Empty
        Dim EmailBody As String = String.Empty
        Dim wStr As String = ""
        Dim dsEmail As New DataSet
        Dim ToUser As String = ""
        Dim strFromMail As String = String.Empty
        Dim strMessage As New StringBuilder
        Dim ErrorLog As String = ""
        Dim objMailMessage As System.Net.Mail.MailMessage
        Dim objSmtpClient As System.Net.Mail.SmtpClient = objcommon.getSmtpClient()
        Dim ToEmail As String = String.Empty
        'Dim EmailSubject As String = "DiSoftC-Image upload for Screening - " + ScreenNo 'Commented By Bhargav Thaker 16March2023
        Dim EmailSubject As String = "DiSoftC-Image upload for " + VisitNo.ToString().Trim() + " - " + ScreenNo 'Modify By Bhargav Thaker 16March2023
        Dim UploadedBy As String = HttpContext.Current.Session(S_UserNameWithProfile)
        Dim myBuilder As System.Text.StringBuilder = New System.Text.StringBuilder()
        Dim ds_EmailInfo As DataSet
        ErrorLog += "EmailSubject=" + EmailSubject + vbCrLf
        Dim eStr_Retu As String = String.Empty
        Dim Username As String = String.Empty
        Dim Password As String = String.Empty
        Dim TenantInfo As New SS.Mail.TenantInfo()
        Dim dsExMsgInfo As New DataSet
        Dim drExMsgInfo As DataRow
        Dim ObjLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()

        Try
            wStr = "vSMSLocationCode='" + HttpContext.Current.Session(S_LocationCode) + "' And cStatusIndi <> 'D'"

            If Not objHelp.GetSMSGateWayDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                        ds_EmailInfo, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            Username = Convert.ToString(ds_EmailInfo.Tables(0).Rows(0)("vFromEmail"))
            Password = Convert.ToString(ds_EmailInfo.Tables(0).Rows(0)("vPassword"))
            TenantInfo.TenantId = Convert.ToString(ds_EmailInfo.Tables(0).Rows(0)("vTenantId"))
            TenantInfo.ClientId = Convert.ToString(ds_EmailInfo.Tables(0).Rows(0)("vClientId"))
            TenantInfo.EmailUser = Username
            TenantInfo.EmailPassword = Password
            TenantInfo.Client_secret = Convert.ToString(ds_EmailInfo.Tables(0).Rows(0)("vSecretKey"))
            Dim AuthenticationWithSecretKey = Convert.ToBoolean(ds_EmailInfo.Tables(0).Rows(0)("bAuthSecretKey"))

            ErrorLog = "vWorkSpaceID=" + vWorkSpaceID + vbCrLf
            ErrorLog += "Site_No=" + ProjectNo + vbCrLf

            If (Not iImgTransmittalDtlId.ToString() = "") Then
                objHelp.GetResultSet("UPDATE ImgTransmittalDtl SET cStatusindi = 'N' WHERE iImgTransmittalDtlId ='" + iImgTransmittalDtlId + "'" + vbCrLf +
                "UPDATE ImgTransmittalDtlHst SET cStatusindi = 'N' WHERE iImgTransmittalDtlId ='" + iImgTransmittalDtlId + "' ", "ImageTransmittal ")
            End If

            'Open tags and write the top portion.
            myBuilder.Append("<!DOCTYPE html>")
            myBuilder.Append("<html><head>")
            myBuilder.Append("<title>")
            myBuilder.Append("Page-")
            myBuilder.Append(Guid.NewGuid().ToString())
            myBuilder.Append("</title>")
            myBuilder.Append("<style>")
            myBuilder.Append("body{ font-family:'Times New Roman','Times Roman','Verdana'}")
            myBuilder.Append("table{border-collapse: collapseborder-width: 1pxborder-color:rgb(0,0,0)}")
            myBuilder.Append("thead{}")
            myBuilder.Append("th{padding:8px 30px 8px 8pxborder-bottom-color:rgb(0,0,0)border-width: 0px 1px 2px 1px}")
            myBuilder.Append("td{padding:5px 30px 5px 5px}")
            myBuilder.Append("</style>")
            myBuilder.Append("</head>")
            myBuilder.Append("<body>")

            ' For Header

            EmailBody = ""
            EmailBody += "    <table align='left' cellspacing='2' border='0' width='100%' style='table-layout: fixed'>"
            EmailBody += "        <tr>"
            EmailBody += "				<td> Dear All,</td>"
            EmailBody += "        </tr>"
            EmailBody += "        <tr>"
            EmailBody += "				<td> The image(s) has been uploaded successfully.  </td>"
            EmailBody += "        </tr>"

            'Table Data
            EmailBody += "        <tr>"
            EmailBody += "				<td> Study Protocol: " + study.ToString() + " </td>"
            EmailBody += "        </tr>"
            EmailBody += "        <tr>"
            EmailBody += "				<td> Site Number: " + ProjectNo.ToString() + " </td>"
            EmailBody += "        </tr>"
            EmailBody += "        <tr>"
            EmailBody += "				<td> Screening No.: " + ScreenNo.ToString() + " </td>"
            EmailBody += "        </tr>"
            EmailBody += "        <tr>"
            EmailBody += "				<td> Visit: " + VisitNo.ToString() + " </td>"
            EmailBody += "        </tr>"
            EmailBody += "        <tr>"
            EmailBody += "				<td> Examination Date: " + Convert.ToDateTime(PreformedDate).ToString("dd-MMM-yyyy") + "</ td>"
            EmailBody += "        </tr>"
            EmailBody += "        <tr>"
            EmailBody += "				<td> Patient Initials: " + PationtInitial + " </td>"
            EmailBody += "        </tr>"
            EmailBody += "        <tr>"
            EmailBody += "				<td> Remarks: " + vRemark.ToString() + "</ td>"
            EmailBody += "        </tr>"
            EmailBody += "        <tr>"
            EmailBody += "				<td> Uploaded Date: " + DateTime.Now.ToString("dd-MMM-yyyy") + "</ td>"
            EmailBody += "        </tr>"
            EmailBody += "        <tr>"
            EmailBody += "				<td> Uploaded By: " + UploadedBy.ToString() + "</ td>"
            EmailBody += "        </tr>"
            EmailBody += "    </table>"
            EmailBody += "	<br><br><br>"
            myBuilder.Append(EmailBody)
            EmailBody = String.Empty

            EmailBody += "	<br>"
            EmailBody += "	<table align='left' cellspacing='2' border='0' width='100%' style='table-layout: fixed'>"
            EmailBody += "		<tr>"
            EmailBody += "				<td>This is an automated generated email. Please do not reply directly to this mail. In case you have any questions please DI-Soft-C team and our team will get back to you as soon as possible.</td>"
            EmailBody += "		</tr>		"
            EmailBody += "		<tr>"
            EmailBody += "				<td>&nbsp</td>"
            EmailBody += "		</tr>		"
            EmailBody += "		<tr>"
            EmailBody += "				<td>Thanks,</td>"
            EmailBody += "		</tr>		"
            EmailBody += "		<tr>"
            EmailBody += "				<td>DI-Soft-C Team</ td>"
            EmailBody += "		</tr>"
            EmailBody += "	</table>"

            myBuilder.Append(EmailBody)
            myBuilder.Append("</body>")
            myBuilder.Append("</html>")
            myHtmlFile = myBuilder.ToString()
            ErrorLog += "************************************Email Body add here**************************" + vbCrLf
            ErrorLog += myBuilder.ToString() + vbCrLf
            If vWorkSpaceID.Trim() = "" Then
                vWorkSpaceID = vParentWorkSpaceId
            End If
            wStr = vWorkSpaceID.ToString() + "##1##"  '2 replace with 1 by Bhargav Thaker 06Mar2023
            dsEmail = objHelp.ProcedureExecute("dbo.GetEmailForTranstrion", wStr)
            ErrorLog += "ToUser Email Count=" + dsEmail.Tables(0).Rows.Count.ToString() + vbCrLf

            If dsEmail.Tables(0).Rows.Count > 0 Then
                For Each dr As DataRow In dsEmail.Tables(0).Rows
                    ToUser += Convert.ToString(dr("vEmailId")) + ","
                Next
                ToUser = ToUser.Remove(ToUser.LastIndexOf(","))
                ErrorLog += "After For loop ToUser=" + ToUser.ToString() + vbCrLf
            End If

            If ToUser = "" Then
                ToUser = System.Configuration.ConfigurationSettings.AppSettings("ToUserCer").ToString()
            End If
            ErrorLog += "To=" + strFromMail.ToString() + vbCrLf

            If ToUser <> "" Then
                strFromMail = Username 'Me.Application(GeneralModule.S_CompanyEmail).ToString
                ErrorLog += "************************************To user not null**************************" + vbCrLf
                ErrorLog += "strFromMail=" + strFromMail.ToString() + vbCrLf
                If Not objHelp.GetExMsgInfoDetails("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty,
                                               dsExMsgInfo, eStr_Retu) Then
                    Throw New Exception(eStr_Retu + vbCrLf + "Error while retrieving EXE Msg Info Details.")
                End If
                ErrorLog += "GetExMsgInfoDetails =" + dsExMsgInfo.Tables(0).Rows.Count.ToString() + vbCrLf
                drExMsgInfo = dsExMsgInfo.Tables(0).NewRow
                drExMsgInfo("vNotificationType") = "Mail"
                drExMsgInfo("vSubject") = EmailSubject.ToString()
                drExMsgInfo("vBody") = myHtmlFile
                drExMsgInfo("vFromEmailId") = strFromMail
                drExMsgInfo("vToEmailId") = String.Empty 'Modify by Bhargav Thaker 24Feb2023
                drExMsgInfo("vBCCEmailId") = ToUser 'Added by Bhargav Thaker 24Feb2023
                drExMsgInfo("iCreatedBy") = HttpContext.Current.Session(S_UserID)
                drExMsgInfo("dCreatedDate") = Date.Now
                'objMailMessage = New System.Net.Mail.MailMessage()
                'objMailMessage.From = New Net.Mail.MailAddress(strFromMail, "Biznet") 'change EmailId while Upload for Lambda

                'objMailMessage.IsBodyHtml = True
                ErrorLog += "ToUser=" + ToUser.ToString() + vbCrLf
                'objMailMessage.To.Add(ToUser)
                ErrorLog += "Subject=" + EmailSubject.ToString() + vbCrLf
                'objMailMessage.Subject = EmailSubject.ToString()
                ErrorLog += "Body=" + myHtmlFile.ToString() + vbCrLf
                'objMailMessage.Body = myHtmlFile.ToString
                'ErrorLog += "Before Send=" + objMailMessage.ToString() + vbCrLf
                'Dim emialId As String = "ronak.nayak@sarjen.com"
                Dim emailIds = ToUser.Trim().Split(",")
                Dim ItemsList As New List(Of Recipient)()
                For Each drToEmail In emailIds
                    ItemsList.Add(New Recipient With {.EmailAddress = New EmailAddress With {.Address = Convert.ToString(drToEmail.Trim())}})
                Next
                Dim message = New Message With {
                     .Subject = EmailSubject,
                     .Body = New ItemBody With {.ContentType = BodyType.Html, .Content = myHtmlFile.ToString},
                     .ToRecipients = New List(Of Recipient)(),
                     .BccRecipients = ItemsList
               }
                '.BccRecipients Added by Bhargav Thaker 24Feb2023

                Try
                    For i As Integer = 0 To 25 - 1

                        Try
                            System.Net.ServicePointManager.ServerCertificateValidationCallback = Function(se As Object,
                            cert As System.Security.Cryptography.X509Certificates.X509Certificate,
                            chain As System.Security.Cryptography.X509Certificates.X509Chain,
                            sslerror As System.Net.Security.SslPolicyErrors) True
                            System.Net.ServicePointManager.MaxServicePointIdleTime = 1000
                            System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType.Ssl3 Or (SecurityProtocolType.Tls12 Or (SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls)))
                            'Smtp_Server.Send(e_mail)
                            If AuthenticationWithSecretKey = False Then
                                SS.Mail.EmailSender.SendMailUsingPassword(TenantInfo, message, True)
                            Else
                                SS.Mail.EmailSender.sendMailBySecret(TenantInfo, message, True)
                            End If
                        Catch e As Exception
                            System.Threading.Thread.Sleep(5000)
                            Continue For
                        End Try

                        Exit For
                    Next
                    'ErrorLog += "After Send=" + objMailMessage.ToString() + vbCrLf
                    drExMsgInfo("cIsSent") = "Y"
                    drExMsgInfo("dSentDate") = Date.Now
                Catch ex As Exception
                    drExMsgInfo("cIsSent") = "N"
                    drExMsgInfo("vRemarks") = ex.Message
                End Try
                dsExMsgInfo.Tables(0).Rows.Add(drExMsgInfo)
                dsExMsgInfo.Tables(0).AcceptChanges()
                If Not ObjLambda.SaveExMsgInfo(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add,
                                                dsExMsgInfo, HttpContext.Current.Session(S_UserID), eStr_Retu) Then
                    Throw New Exception(eStr_Retu + vbCrLf + "Error while Saving in Mail Sending Details.")
                    Exit Function
                End If
                strMessage = New StringBuilder
            End If
            Return "All Files uploaded Sucessfully..!"
        Catch ex As Exception
            'Return ex.Message
            WriteError(ErrorLog.ToString() + vbCrLf + ex.Message.ToString() + vbCrLf + "Error while sending Mail.")
            Return ("Error while sending Mail..!" + vbCrLf + ex.Message.ToString())
        End Try
    End Function

#End Region

#Region "Error Handler"
    Public Shared Function WriteError(ByVal ErrorMessage As String)
        Dim Contents As String
        Dim objWriter As StreamWriter
        Dim remoteAddress As String = HttpContext.Current.Request.ServerVariables("REMOTE_HOST")
        Dim bc As HttpBrowserCapabilities = HttpContext.Current.Request.Browser
        Dim strPath As String

        If IsNothing(HttpContext.Current.Session("Path")) Then
            strPath = HttpContext.Current.Server.MapPath("~")
            strPath = strPath + "\ErrorHandler\Error_" + Format(Date.Now, "yyyy-MM-dd").ToString() + ".Log"
            HttpContext.Current.Session("Path") = strPath
        End If

        If System.IO.File.Exists(HttpContext.Current.Session("Path")) Then
            objWriter = New StreamWriter(HttpContext.Current.Session("Path"), True)
        Else
            objWriter = New StreamWriter(HttpContext.Current.Session("Path"), False)
        End If
        Try

            If ErrorMessage.ToString.Trim() <> "" Then
                Contents = "Err.Date: " & DateTime.Now.ToString & ControlChars.NewLine
                Contents += ErrorMessage & vbCrLf
                Contents += "UserNo :" & HttpContext.Current.Session("UserNo") & vbCrLf
                Contents += "UserName :" & HttpContext.Current.Session("UserName") & vbCrLf
                Contents += "User Division :" & HttpContext.Current.Session("DivisionNo") & vbCrLf
                Contents += "URL:" & HttpContext.Current.Request.Url.ToString() & vbCrLf
                Contents += "Javascript: " & bc.EcmaScriptVersion.ToString & vbCrLf
                Contents += "VB Script: " & bc.VBScript.ToString & vbCrLf
                Contents += "Cookies: " & bc.Cookies.ToString & vbCrLf
                Contents += "Remote Address: " & remoteAddress & vbCrLf
                Contents += "=========================================================" & vbCrLf
                objWriter.WriteLine(Contents)
                objWriter.Close()
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            objWriter.Close()
        End Try
    End Function
#End Region
    '#Region "Dicom Tag Edit/Delete"
    '    <WebMethod>
    '    Public Shared Function DicomTagChange(ByVal fileName As String, ByVal fromPath As String, ByVal subjectValue As String) As String
    '        Dim dirPath = fromPath
    '        Dim filePath = Path.Combine(dirPath, fileName)
    '        'Dim backupFolder = dirPath + "BackupFiles"
    '        'If Not Directory.Exists(backupFolder) Then
    '        '    Directory.CreateDirectory(backupFolder)
    '        'End If
    '        'File.Copy(filePath, Path.Combine(backupFolder, fileName), True)

    '        Dim openedDicom As DicomFile = DicomFile.Open(filePath, FileReadOption.ReadAll)

    '        Dim ArrTag() As String = {"0008,0050", "0008,0081", "0008,0090", "0008,1050", "0010,1001", "0010,1002", "0010,1040"}
    '        Dim remove As DicomTag
    '        For index = 0 To ArrTag.Length - 1
    '            remove = DicomTag.Parse(ArrTag(index).ToString)
    '            openedDicom.Dataset.Remove(remove)
    '        Next

    '        Dim edit = DicomTag.Parse("0010,0010")
    '        openedDicom.Dataset.AddOrUpdate(edit, subjectValue)
    '        openedDicom.Save(filePath)
    '    End Function
    '#End Region
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' findRecodeIndexWise("0000010047", "RowIndex < 4", "AH20-00175")
    End Sub

End Class
