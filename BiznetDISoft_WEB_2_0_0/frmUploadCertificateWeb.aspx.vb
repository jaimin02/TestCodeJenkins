Imports System.Web.Services
Imports System.Configuration
Imports Dicom
Imports Newtonsoft.Json

Partial Class frmUploadCertificateWeb
    Inherits System.Web.UI.Page

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

            ds_GetselectedChild = objHelp.GetResultSet("Select + '[' + WorkSpaceProtocolDetail.vProjectNo + ']' + Workspacemst.vRequestId AS ProjectNo, Workspacemst.vWorkSpaceId AS vWorkspaceId, vParentWorkspaceId, WorkSpaceProtocolDetail.vProjectNo FROM Workspacemst INNER JOIN WorkSpaceProtocolDetail ON ( Workspacemst.vWorkSpaceId = WorkSpaceProtocolDetail.vWorkSpaceId ) WHERE Workspacemst.cStatusIndi <> 'D' and Workspacemst.vWorkspaceId ='" + HProjectId.Trim() + "'order by vWorkspaceId asc", "Workspacemst")
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
    '<WebMethod>
    'Public Shared Function FillGrid(ByVal WorkspaceId As String, ByVal SubjectId As String) As String
    '    Dim ds_FillGrid As New Data.DataSet
    '    Dim wStr As String = String.Empty
    '    Dim objCommon As New clsCommon
    '    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    '    Try
    '        wStr = WorkspaceId.Trim().ToString() + "##" + SubjectId.Trim().ToString()
    '        ds_FillGrid = objHelp.ProcedureExecute("Get_ImageTransmittalDetails", wStr)
    '        'ds_Visit = objHelp.GetResultSet("SELECT * FROM ImageTransmittalImgDtl WHERE vWorkspaceId = '" + WorkspaceId.Trim() + "' AND vSubjectId = '" + SubjectId.Trim() + "'", "ImageTransmittalImgDtl ")

    '        If ds_FillGrid Is Nothing OrElse ds_FillGrid.Tables.Count <= 0 OrElse ds_FillGrid.Tables(0).Rows.Count <= 0 Then
    '            Exit Function
    '        End If
    '        Return ConvertDataTabletoString(ds_FillGrid.Tables(0))
    '        'Me.gvwCertificate.DataSource = ds_Certificate.Tables(0).DefaultView.ToTable()
    '        'Me.gvwCertificate.DataBind()
    '        'If gvwCertificate.Rows.Count > 0 Then
    '        '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvwCertificate", "UIgvwCertificate(); ", True)
    '        'End If

    '    Catch ex As Exception
    '        Throw New Exception(ex.Message + vbCrLf + "Error in FillGrid.")
    '    End Try

    'End Function

    <WebMethod>
    Public Shared Function FillGrid(ByVal WorkspaceId As String) As String
        Dim ds_Certificate As New Data.DataSet
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Try
            'wStr = WorkspaceId.Trim().ToString()
            'ds_Certificate = objHelp.ProcedureExecute("Get_CertificateDetails", wStr)
            ds_Certificate = objHelp.GetResultSet("Select * From View_CertificateList " + vbCrLf +
                                                      "where vWorkspaceId ='" + WorkspaceId.Trim() + "'", "View_CertificateList ")

            If ds_Certificate Is Nothing OrElse ds_Certificate.Tables.Count <= 0 OrElse ds_Certificate.Tables(0).Rows.Count <= 0 Then
                Exit Function
            End If
            Return ConvertDataTabletoString(ds_Certificate.Tables(0))
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
    Public Shared Function AuditTrail(ByVal iCertificateMasterId As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_Certificate As New DataSet
        Dim estr As String = String.Empty

        Try
            wStr = " iCertificateMasterId = '" + iCertificateMasterId + "'"
            If Not objHelp.GetData("View_CertificateAuditTrail", "*", wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Certificate, estr) Then
                Throw New Exception(estr)
            End If
            Return ConvertDataTabletoString(ds_Certificate.Tables(0))
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    <WebMethod>
    Public Shared Function VisitValidation(ByVal iProjectId As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_ImgTrData As New DataSet
        Dim dv As New DataView


        Try
            'wStr = " iImgTransmittalHdrId = '" + iImgTransmittalHdrId + "'"
            ds_ImgTrData = objHelp.GetResultSet("Select * from View_CertificateAuditTrail where vWorkspaceId = '" + iProjectId.Trim() + "'", "View_ImgTransmittalAuditTrail")
            dv = ds_ImgTrData.Tables(0).DefaultView
            'dv.RowFilter = "vNodeDisplayName = '" + visitName + "'"
            dv.Sort = "iCertificateMasterId desc"

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


End Class
