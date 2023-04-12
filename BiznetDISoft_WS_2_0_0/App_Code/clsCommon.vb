Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Configuration
Imports System.IO

Public Class clsCommon
    Inherits ClsDataLogic_New

#Region "Fill DropDown List [User Control]"

    Public Function FillUserControl(ByVal DatatextField As String, _
                                ByVal DataValueField As String, _
                                ByVal TableName As String) As DataSet


        Dim paramlist As New ArrayList
        Dim objdata As New ClsDataLogic_New
        Dim estr_Retu As String = ""
        Try
            paramlist.Add(New SqlParameter("@DatatextField", SqlDbType.VarChar, 50, ParameterDirection.Input, False, 0, 0, "DatatextField", DataRowVersion.Current, DatatextField))
            paramlist.Add(New SqlParameter("@DataValueField", SqlDbType.VarChar, 50, ParameterDirection.Input, False, 0, 0, "DataValueField", DataRowVersion.Current, DataValueField))
            paramlist.Add(New SqlParameter("@TableName", SqlDbType.VarChar, 50, ParameterDirection.Input, False, 0, 0, "TableName", DataRowVersion.Current, TableName))
            Return ExecuteSP_DataSet("Proc_FillControl", paramlist)
        Catch ex As Exception
            estr_Retu += ex.Message
            Return Nothing
        Finally
            paramlist = Nothing
            objdata = Nothing
        End Try

    End Function

#End Region

#Region "Check Valid Login "

    Public Function ValidateUser(ByVal UserName As String, ByVal Password As String) As DataSet
        Dim ParamList As New ArrayList
        Dim objdata As New ClsDataLogic_New
        Try
            ParamList.Add(New SqlParameter("@UserName", SqlDbType.VarChar, 50, ParameterDirection.Input, False, 0, 0, "vUserName", DataRowVersion.Current, UserName))
            ParamList.Add(New SqlParameter("@PassWord", SqlDbType.VarChar, 20, ParameterDirection.Input, False, 0, 0, "vPassWord", DataRowVersion.Current, Password))
            Return objdata.ExecuteSP_DataSet("Proc_Login", ParamList)
        Catch ex As Exception
            Return Nothing
        Finally
            ParamList = Nothing
            objdata = Nothing
        End Try
    End Function

#End Region

    Public Function getMenu(ByVal UserTypeCode As String) As DataSet
        'Dim ds As New DataSet
        Return ExecuteQuery_DataSet("SELECT * FROM View_Menu" +
                " where vUserTypeCode = " & UserTypeCode & " " +
                " order by ParentID,iSeqNo ", "Table")
    End Function

#Region "GetMenu-Dashboard"
    Public Function getMenuDashboard(ByVal UserTypeCode As String, ByVal AdjucatorConstant As String) As DataSet
        'Dim ds As New DataSet
        Return ExecuteQuery_DataSet("SELECT * FROM View_Menu" +
                " where vUserTypeCode = " & UserTypeCode & "" +
                " order by ParentID,iSeqNo ", "Table")
    End Function
#End Region

#Region "Select Distinct"

    Public Function SelectDistinct(ByVal SourceTable As DataTable, ByVal ParamArray FieldNames() As String) As DataTable
        Dim lastValues() As Object
        Dim newTable As DataTable

        If FieldNames Is Nothing OrElse FieldNames.Length = 0 Then
            Throw New ArgumentNullException("FieldNames")
        End If

        lastValues = New Object(FieldNames.Length - 1) {}
        newTable = New DataTable

        For Each field As String In FieldNames
            newTable.Columns.Add(field, SourceTable.Columns(field).DataType)
        Next

        For Each Row As DataRow In SourceTable.Select("", String.Join(", ", FieldNames))
            If Not fieldValuesAreEqual(lastValues, Row, FieldNames) Then
                newTable.Rows.Add(createRowClone(Row, newTable.NewRow(), FieldNames))

                setLastValues(lastValues, Row, FieldNames)
            End If
        Next

        Return newTable
    End Function

    Private Function fieldValuesAreEqual(ByVal lastValues() As Object, ByVal currentRow As DataRow, ByVal fieldNames() As String) As Boolean
        Dim areEqual As Boolean = True

        For i As Integer = 0 To fieldNames.Length - 1
            If lastValues(i) Is Nothing OrElse Not lastValues(i).Equals(currentRow(fieldNames(i))) Then
                areEqual = False
                Exit For
            End If
        Next

        Return areEqual
    End Function

    Private Function createRowClone(ByVal sourceRow As DataRow, ByVal newRow As DataRow, ByVal fieldNames() As String) As DataRow
        For Each field As String In fieldNames
            newRow(field) = sourceRow(field)
        Next

        Return newRow
    End Function

    Private Sub setLastValues(ByVal lastValues() As Object, ByVal sourceRow As DataRow, ByVal fieldNames() As String)
        For i As Integer = 0 To fieldNames.Length - 1
            lastValues(i) = sourceRow(fieldNames(i))
        Next
    End Sub

#End Region

    Public Function CreateColumn(ByVal TableName_1 As DataTable, _
                                  ByVal ColumnName_1 As String, _
                        ByVal Type_1 As System.Type) As Boolean

        Dim dc As DataColumn
        dc = New DataColumn
        dc.ColumnName = ColumnName_1
        dc.DataType = Type_1
        TableName_1.Columns.Add(dc)
        Return True
    End Function

    '=========================Added on 16-09-09=====to Encrypt/Decrypt Password====By Deepak Singh
#Region "Encrypt/Decrypt Password"

    Public Function EncryptPassword(ByVal password As String) As String

        Dim len As Int16 = password.Length  'setting len = length of the entered password
        Dim pwd(len) As String              'declaring pwd as a string array of length len
        pwd(0) = password.ToString          'assigning password in pwd(0) 
        Dim cpwd(len) As Char               'cpwd as char array of the same length
        Dim index As Int16
        Dim Lascii(len) As Integer          'Lascii(len) to store corresponding ascii of password
        Dim Sascii(len) As Integer          'Sascii(len) to store value of substituted ascii
        Dim result As New TextBox           'to store the resulting appended string
        Dim Reverse As String
        Dim retstr As String                'returning the obtained string
        Dim CountAdd As Integer = 0
        cpwd = pwd(0).ToCharArray

        For index = 0 To len - 1
            If CountAdd = 4 Then
                CountAdd = 0
            End If
            Lascii(index) = AscW(cpwd(index))

            Sascii(index) = Lascii(index) + (CountAdd + 1)
            cpwd(index) = ChrW(Sascii(index))
            result.Text += cpwd(index)
            CountAdd += 1
        Next index
        Reverse = result.Text
        retstr = ReversePassword(Reverse)
        Return retstr

    End Function
    '======
    Public Function ReversePassword(ByVal password As String) As String

        Dim len As Int16 = password.Length
        Dim index As Integer
        Dim Keytextbox As New TextBox
        Dim key As String
        Dim Pwd(len) As Char
        Dim pwd1(len) As String

        Pwd = password.ToCharArray
        Dim intLen As Integer = len - 1
        Dim Reverse(intLen) As Char
        For index = 0 To intLen
            Reverse(index) = Pwd(intLen - index)
            Keytextbox.Text += Reverse(index)
        Next index
        key = Keytextbox.Text
        Return (key)

    End Function

    Public Function DecryptPassword(ByVal revpassword As String) As String

        Dim Password As String = ReversePassword(revpassword)   'to get the Reversed Value of stored encrypted password
        Dim len As Int16 = Password.Length                      'setting len = length of the entered password
        Dim pwd(len) As String                                  'declaring pwd as a string array of length len
        pwd(0) = Password.ToString                              'assigning password in pwd(0) 
        Dim cpwd(len) As Char                                   'cpwd as char array of the same length
        Dim index As Int16
        Dim Lascii(len) As Integer                              'Lascii(len) to store corresponding ascii of password
        Dim Sascii(len) As Integer                              'Sascii(len) to store value of substituted ascii
        Dim result As New TextBox                               'to store the resulting appended string
        Dim retstr As String                                    'returning the obtained string
        Dim CountAdd As Integer = 0
        cpwd = pwd(0).ToCharArray
        For index = 0 To len - 1
            If CountAdd = 4 Then
                CountAdd = 0
            End If
            Lascii(index) = AscW(cpwd(index))
            Sascii(index) = Lascii(index) - (CountAdd + 1)

            cpwd(index) = ChrW(Sascii(index))
            result.Text += cpwd(index)
            CountAdd += 1
        Next index

        retstr = result.Text
        Return retstr

    End Function

#End Region
    '=====================================================================================

    '*************KNET********************8

    Private Shared md5 As System.Security.Cryptography.MD5 = md5.Create()

    Public Shared Function CalculateChecksum(ByVal filePath As String) As String
        Dim checksum() As Byte
        Using stream As FileStream = File.OpenRead(filePath)
            checksum = md5.ComputeHash(stream)
            Return (BitConverter.ToString(checksum).Replace("-", String.Empty))
        End Using
    End Function

    '****************************************

    'Added By Bhargav Thaker Start
#Region "Get ParentWorkspaceId"
    Public Function GetParentWorkspaceId(ByVal vWorkspaceId As String) As String
        Dim Result As String = String.Empty
        Try
            Dim Sqlstr As String = String.Empty
            Sqlstr = "SELECT vParentWorkspaceId FROM Workspacemst "
            Sqlstr += "WHERE Workspacemst.cStatusIndi <> 'D' and Workspacemst.vWorkspaceId ='" + vWorkspaceId.Trim() + "'"

            Dim ds_ParentWorkspaceId As DataSet = ExecuteQuery_DataSet(Sqlstr)
            If ds_ParentWorkspaceId.Tables(0).Rows.Count > 0 Then
                Result = ds_ParentWorkspaceId.Tables(0).Rows(0)("vParentWorkspaceId").ToString()
            End If
            Return Result
        Catch ex As Exception
            Result = String.Empty
            Return Result
        End Try
    End Function
#End Region

#Region "Check Validation before Delete CRF Bunch Activity"
    Public Function CRFBunch_Validation(ByVal vWorkspaceId As String, ByVal vSubjectId As String, ByVal iBunchNo As String, ByVal iNodeId As String, ByRef eStr_Retu As String) As Boolean
        Try
            Dim Sqlstr As String = String.Empty
            Sqlstr = "SELECT View_GetVisitStatus.iNodeId, View_GetVisitStatus.ImageReviewStatus FROM dbo.View_GetVisitStatus "
            Sqlstr += "INNER JOIN dbo.CRFBunchDetail ON (CRFBunchDetail.vWorkSpaceId = View_GetVisitStatus.vWorkspaceId AND CRFBunchDetail.vSubjectId = View_GetVisitStatus.vSubjectId AND CRFBunchDetail.iNodeId = View_GetVisitStatus.iNodeId) "
            Sqlstr += "WHERE View_GetVisitStatus.vWorkspaceId='" + vWorkspaceId.Trim() + "' AND View_GetVisitStatus.vSubjectId='" + vSubjectId.Trim() + "' AND CRFBunchDetail.iBunchId='" + iBunchNo.Trim() + "' "
            Sqlstr += "AND CRFBunchDetail.iNodeId='" + iNodeId.Trim() + "' AND CRFBunchDetail.cStatusIndi<>'D' "
            Sqlstr += "GROUP BY View_GetVisitStatus.iNodeId,View_GetVisitStatus.ImageReviewStatus"

            Dim ds_CheckStatus As DataSet = ExecuteQuery_DataSet(Sqlstr)
            If ds_CheckStatus.Tables(0).Rows.Count > 0 Then
                Dim ReviewStatus As String = String.Empty
                ReviewStatus = ds_CheckStatus.Tables(0).Rows(0)("ImageReviewStatus").ToString().Trim()
                If ReviewStatus = "R1" Or ReviewStatus = "R2" Then
                    eStr_Retu = "Cant Delete. Activity Bunch In Process!"
                    CRFBunch_Validation = False
                    Return CRFBunch_Validation
                End If
            End If

            Sqlstr = String.Empty
            Sqlstr = "SELECT COUNT(*) AS NextBunch FROM dbo.CRFBunchDetail "
            Sqlstr += "WHERE vWorkSpaceId='" + vWorkspaceId.Trim() + "' AND vSubjectId='" + vSubjectId.Trim() + "' AND iBunchId>'" + iBunchNo.Trim() + "' AND cStatusIndi<>'D'"

            Dim ds_NextBunch As DataSet = ExecuteQuery_DataSet(Sqlstr)
            If ds_NextBunch.Tables(0).Rows.Count > 0 Then
                Dim IsNextBunchTheir = ds_NextBunch.Tables(0).Rows(0)("NextBunch").ToString().Trim()
                If IsNextBunchTheir <> 0 Then
                    eStr_Retu = "Cant Delete. Fisrt Delete Next Activity Bunch!"
                    CRFBunch_Validation = False
                    Return CRFBunch_Validation
                End If
            End If

            CRFBunch_Validation = True
        Catch ex As Exception
            eStr_Retu = ex.Message
            CRFBunch_Validation = False
            Return CRFBunch_Validation
        End Try
    End Function
#End Region

#Region "Get Visit list"
    Public Function GetVisitList(ByVal vWorkspaceId As String, ByVal vSubjectId As String) As DataSet
        Dim ds_Visit As New DataSet
        Try
            Dim Sqlstr As String = String.Empty
            Sqlstr = "SELECT ImgTransmittalHdr.iNodeId,View_WorkSpaceNodeDetail.vNodeDisplayName FROM dbo.ImgTransmittalHdr "
            Sqlstr += "INNER JOIN (SELECT iImgTransmittalHdrId,MAX(dModifyOn) AS ModifyDate FROM dbo.ImgTransmittalDtl GROUP BY iImgTransmittalHdrId) ModifyDates ON (ImgTransmittalHdr.iImgTransmittalHdrId = ModifyDates.iImgTransmittalHdrId) "
            Sqlstr += "INNER JOIN dbo.ImgTransmittalDtl ON (ImgTransmittalDtl.iImgTransmittalHdrId = ModifyDates.iImgTransmittalHdrId AND ImgTransmittalDtl.dModifyOn=ModifyDates.ModifyDate) "
            Sqlstr += "INNER JOIN dbo.View_WorkSpaceNodeDetail ON (View_WorkSpaceNodeDetail.vWorkSpaceId = ImgTransmittalHdr.vWorkspaceId AND View_WorkSpaceNodeDetail.iNodeId = ImgTransmittalHdr.iNodeId) "
            Sqlstr += "WHERE ImgTransmittalHdr.vWorkspaceId='" + vWorkspaceId.Trim() + "' AND ImgTransmittalHdr.vSubjectId='" + vSubjectId.Trim() + "' AND ImgTransmittalDtl.cReviewStatus='QC2A' "
            Sqlstr += "AND ImgTransmittalHdr.cStatusIndi<>'D' AND ImgTransmittalDtl.cStatusIndi<>'D' AND View_WorkSpaceNodeDetail.cStatusIndi<>'D'"

            ds_Visit = ExecuteQuery_DataSet(Sqlstr, "tblVisit")
            Return ds_Visit
        Catch ex As Exception
            ds_Visit = New DataSet
            Return ds_Visit
        End Try
    End Function
#End Region

#Region "Get BunchId"
    Public Function GetBunchId(ByVal vWorkspaceId As String, ByVal vSubjectId As String) As String
        Dim Result As String = String.Empty
        Try
            Dim Sqlstr As String = String.Empty
            Sqlstr = "SELECT ISNULL(MAX(iBunchId),0) + 1 AS iBunchId FROM dbo.CRFBunchDetail "
            Sqlstr += "WHERE vWorkSpaceId='" + vWorkspaceId.Trim() + "' AND vSubjectId='" + vSubjectId.Trim() + "' AND cStatusIndi<>'D'"

            Dim ds_GetBunchId As DataSet = ExecuteQuery_DataSet(Sqlstr)
            If ds_GetBunchId.Tables(0).Rows.Count > 0 Then
                Result = ds_GetBunchId.Tables(0).Rows(0)("iBunchId").ToString()
            End If
            Return Result
        Catch ex As Exception
            Result = String.Empty
            Return Result
        End Try
    End Function
#End Region

#Region "Get ActivityId"
    Public Function GetActivityId(ByVal vWorkspaceId As String, ByVal iNodeId As String) As String
        Dim Result As String = String.Empty
        Try
            Dim Sqlstr As String = String.Empty
            Sqlstr = "SELECT TOP 1 vActivityId FROM dbo.View_WorkSpaceNodeDetail "
            Sqlstr += "WHERE vWorkSpaceId='" + vWorkspaceId.Trim() + "' AND iNodeId=" + iNodeId.Trim() + " "

            Dim ds_GetActivityId As DataSet = ExecuteQuery_DataSet(Sqlstr)
            If ds_GetActivityId.Tables(0).Rows.Count > 0 Then
                Result = ds_GetActivityId.Tables(0).Rows(0)("vActivityId").ToString()
            End If
            Return Result
        Catch ex As Exception
            Result = String.Empty
            Return Result
        End Try
    End Function
#End Region

#Region "Get CRF Bunch Records"
    Public Function GetCRFBunchRecord(ByVal vWorkspaceId As String, ByVal vSubjectId As String) As DataSet
        Dim Sqlstr As String = String.Empty
        Sqlstr = "SELECT * FROM View_GetCRFBunchDetails WHERE vWorkSpaceId='" + vWorkspaceId.Trim() + "' AND vSubjectId='" + vSubjectId.Trim() + "' AND cStatusIndi<>'D'"
        Return ExecuteQuery_DataSet(Sqlstr, "Table")
    End Function
#End Region

#Region "Get Template Details For Case Assignment"
    Public Function GetTemplateDetails(ByVal vWorkspaceId As String) As DataSet
        Dim Sqlstr As String = String.Empty
        Sqlstr = "SELECT iSeqNo, vMedExCode, vMedExDesc, vMedExGroupCode, vMedExValues FROM dbo.View_MedExWorkSpaceDtl "
        Sqlstr += "WHERE vWorkSpaceId='" + vWorkspaceId.Trim() + "' AND vMedExGroupDesc LIKE '%Case Assignment%' AND vNodeDisplayName LIKE '%Case Assignment%' "
        Sqlstr += "GROUP BY iSeqNo, vMedExCode, vMedExDesc, vMedExGroupCode, vMedExValues "
        Sqlstr += "ORDER BY iSeqNo ASC"
        Return ExecuteQuery_DataSet(Sqlstr, "tbl")
    End Function
#End Region

#Region "Get Project Name"
    Public Function GetProjectName(ByVal vWorkspaceId As String, ByVal iUserId As String) As String
        Dim Result As String = String.Empty
        Try
            Dim Sqlstr As String = String.Empty
            Sqlstr = "SELECT TOP 1 '[' + vProjectNo + '] ' + vRequestId As ProjectName FROM dbo.View_MyProjects WHERE vWorkspaceId='" + vWorkspaceId.Trim() + "' AND iUserId=" + iUserId

            Dim ds_GetProName As DataSet = ExecuteQuery_DataSet(Sqlstr)
            If ds_GetProName.Tables(0).Rows.Count > 0 Then
                Result = ds_GetProName.Tables(0).Rows(0)("ProjectName").ToString()
            End If
            Return Result
        Catch ex As Exception
            Result = String.Empty
            Return Result
        End Try
    End Function
#End Region

#Region "Get SubjectNo"
    Public Function GetSubjectNo(ByVal vWorkspaceId As String, ByVal vSubjectId As String) As String
        Dim Result As String = String.Empty
        Try
            Dim Sqlstr As String = String.Empty
            Sqlstr = "SELECT TOP 1 ISNULL(vInitials,'')+'-'+ISNULL(vMySubjectNo,'')+'- ('+vSubjectId+')' As SubjectNo FROM dbo.view_WorkspaceSubjectMst WHERE vWorkspaceId='" + vWorkspaceId.Trim() + "' AND vSubjectId='" + vSubjectId + "'"

            Dim ds_GetSubNo As DataSet = ExecuteQuery_DataSet(Sqlstr)
            If ds_GetSubNo.Tables(0).Rows.Count > 0 Then
                Result = ds_GetSubNo.Tables(0).Rows(0)("SubjectNo").ToString()
            End If
            Return Result
        Catch ex As Exception
            Result = String.Empty
            Return Result
        End Try
    End Function
#End Region

    'Added By Bhargav Thaker End

End Class
Friend Class SaveDataInDbObj
    Private _objDtLogic As ClsDataLogic_New

    Public Sub New(ByVal ObjDtLogic As ClsDataLogic_New)

        If ObjDtLogic Is Nothing Then
            Throw New Exception("Invalid Object of Data Logic")
            Exit Sub
        End If

        If ObjDtLogic.Transaction Is Nothing Then
            Throw New Exception("Transaction object is not started")
            Exit Sub
        End If

        _objDtLogic = ObjDtLogic

    End Sub

    Private Function getSqlObjectInfo(ByVal SqlObjectName_1 As String, _
                                      ByRef dtTbl_Retu As Data.DataTable, _
                                      ByRef eStr_Retu As String) As Boolean
        Dim qStr As String = ""

        Try
            SqlObjectName_1 = "'" + SqlObjectName_1.ToUpper + "'"

            qStr = "Select SysColumns.Name as ColName, SysColumns.Colid as ColIndex, " + _
                   "       Type_Name(SysColumns.xType) as ColDbType, " + _
                   "       ColumnProperty(SysColumns.id, SysColumns.Name,'IsOutParam') as IsOutPut, " + _
                   "       ColumnProperty(SysColumns.id, SysColumns.Name,'IsIdentity') as IsAutoId, " + _
                   "       SysColumns.Length " + _
                   " From SysColumns " + _
                   " Where SysColumns.Id= Object_Id(" + SqlObjectName_1 + ")" + " " + _
                   " Order by SysColumns.Colid"

            dtTbl_Retu = _objDtLogic.ExecuteQuery_DataSet(qStr).Tables(0)
            getSqlObjectInfo = True

        Catch ex As Exception
            getSqlObjectInfo = False
            eStr_Retu = ex.Message
        End Try

    End Function

    Public Function SaveInDb(ByVal ProcedureName_1 As String, _
                             ByVal tbl4Save As Data.DataTable, _
                             ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                             ByRef ProcedureOutPutVal_Retu As ArrayList, _
                             ByRef eStr_Retu As String, _
                             Optional ByVal SetTimeOut As Boolean = False) As Boolean

        Dim tbl_ProcDtl As Data.DataTable = Nothing
        Dim SqlPara As Data.SqlClient.SqlParameter = Nothing
        Dim ParaRetuVal As ParameterReturnValue = Nothing
        Dim sqlCmd As Data.SqlClient.SqlCommand = Nothing
        Dim ParaRowArr As ArrayList = Nothing
        Dim Proc_ColName As String = ""
        Dim ColIndex_1 As Integer = -1



        Try
            ProcedureOutPutVal_Retu = New ArrayList()
            eStr_Retu = ""

            If ProcedureName_1.Trim = "" Then
                eStr_Retu = "Procedure Name Is Invalid or Blank"
                Exit Function
            End If
            If Not getSqlObjectInfo(ProcedureName_1, tbl_ProcDtl, eStr_Retu) Then
                eStr_Retu += vbCrLf + "Error Occured While Retrieving Procedure Info."
                Exit Function
            End If
            If tbl_ProcDtl.Rows.Count = 0 Then
                eStr_Retu = "No Procedure Information is found for procedure = " + ProcedureName_1
                Exit Function
            End If
            'AS PER DISCUSSION WITH GUNJAN BHAI

            ''viraj
            'If tbl4Save.Rows.Count = 0 Then
            '    eStr_Retu = "No data in table = "
            '    Exit Function
            'End If
            For Each dr_Save As Data.DataRow In tbl4Save.Rows

                sqlCmd = New SqlCommand(ProcedureName_1, _objDtLogic.Connection, _objDtLogic.Transaction)
                sqlCmd.CommandType = CommandType.StoredProcedure
                ParaRowArr = New ArrayList

                For Each dr_P As Data.DataRow In tbl_ProcDtl.Rows

                    Proc_ColName = dr_P("COLNAME").ToString.Substring(1).ToUpper
                    ColIndex_1 = CType(dr_P("COLINDEX"), System.Int32) - 1

                    SqlPara = New Data.SqlClient.SqlParameter()
                    SqlPara.ParameterName = dr_P("COLNAME").ToString
                    SqlPara.Direction = ParameterDirection.Input

                    If Proc_ColName = "DATAOPMODE" Then

                        SqlPara.Value = Convert.ToInt32(Choice_1)

                    ElseIf Convert.ToInt32(dr_P("ISOUTPUT")) = 0 Then ' when column is not output parameter

                        SqlPara.Value = dr_Save(Proc_ColName)

                    ElseIf Convert.ToInt32(dr_P("ISOUTPUT")) = 1 Then ' when column is output parameter

                        SqlPara.Direction = ParameterDirection.Output
                        SqlPara.Size = dr_P("LENGTH")

                        ParaRetuVal = New ParameterReturnValue
                        ParaRetuVal.ParameterName = dr_P("COLNAME")
                        ParaRetuVal.ParameterIndex = ColIndex_1
                        ParaRowArr.Add(ParaRetuVal)

                    End If

                    sqlCmd.Parameters.Add(SqlPara)

                Next dr_P

                If SetTimeOut Then
                    sqlCmd.CommandTimeout = 180000
                End If
                sqlCmd.ExecuteNonQuery()

                'If Choice_1 = DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then ' Added By Hiral 17-May-2007
                For Each ParaRetuVal In ParaRowArr

                    ParaRetuVal.ParameterValue = IIf(IsDBNull(sqlCmd.Parameters.Item(ParaRetuVal.ParameterName).Value), "", sqlCmd.Parameters.Item(ParaRetuVal.ParameterName).Value)

                Next ParaRetuVal

                ProcedureOutPutVal_Retu.Add(ParaRowArr)
                'End If

            Next dr_Save

            SaveInDb = True

        Catch ex As Exception
            SaveInDb = False
            ProcedureOutPutVal_Retu = Nothing
            eStr_Retu = ex.Message
            Exit Function
        End Try

    End Function

    'Public Function AssignValues(ByVal TblName_1 As String, _
    '                             ByVal Pk_ColumnNamesArr() As String, _
    '                             ByVal Choice_1 As DataObjOpenSaveModeEnum, _
    '                             ByVal Dt_Source As Data.DataTable, _
    '                             ByVal Dt_Target As Data.DataTable, _
    '                             ByVal Dt_Audit As Data.DataTable, _
    '                             ByVal Required_Audit As Boolean, _
    '                             ByVal UserCode_1 As String, _
    '                             ByVal ServerDateTime_1 As DateTime, _
    '                             ByRef eStr_Retu As String) As Boolean

    '    Dim dr_S As Data.DataRow = Nothing
    '    Dim dr_T As Data.DataRow = Nothing
    '    Dim dr_Audit As Data.DataRow = Nothing
    '    Dim dtColArr() As DataColumn
    '    Dim ColValArr() As String
    '    Dim Dt_TblProp As Data.DataTable = Nothing
    '    Dim Dr_TblPropArr() As Data.DataRowView = Nothing
    '    Dim IsAutoId As Boolean = False

    '    Try
    '        If Pk_ColumnNamesArr.Length = 0 Then
    '            eStr_Retu = "Primary Key Column Ref. Is Invalid or Blank"
    '            Exit Function
    '        End If

    '        If Dt_Source Is Nothing Then
    '            eStr_Retu = "Source Table Ref. Is Invalid or Null"
    '            Exit Function
    '        End If

    '        If Dt_Target Is Nothing Then
    '            eStr_Retu = "Target Table Ref. Is Invalid or Null"
    '            Exit Function
    '        End If

    '        'Temporary Commented to test SchemeMaster
    '        If Required_Audit Then
    '            If Dt_Audit Is Nothing Then
    '                eStr_Retu = "Auditorial Table Ref. Is Invalid or Null"
    '                Exit Function
    '            End If
    '        End If

    '        '--Retrieve the Column property of the table 'Added by Gunjan on 04-Jan-2007
    '        '*********************************************************************
    '        If Not getSqlObjectInfo(TblName_1, Dt_TblProp, eStr_Retu) Then
    '            eStr_Retu += "Error occured while Retrieving Columns Information of Table"
    '            Exit Function
    '        End If

    '        '-- Creating Primary Key Column Name
    '        '
    '        ReDim Preserve ColValArr(Pk_ColumnNamesArr.Length - 1)
    '        ReDim Preserve dtColArr(Pk_ColumnNamesArr.Length - 1)

    '        Dt_TblProp.DefaultView.Sort = "ColName"

    '        For i As Integer = 0 To Pk_ColumnNamesArr.Length - 1

    '            IsAutoId = False
    '            Dr_TblPropArr = Dt_TblProp.DefaultView.FindRows(Pk_ColumnNamesArr(i))

    '            If Dr_TblPropArr.Length > 0 Then
    '                IsAutoId = (Dr_TblPropArr(0)("IsAutoId") = 1)
    '            End If

    '            dtColArr(i) = Dt_Target.Columns(Pk_ColumnNamesArr(i))

    '            If IsAutoId Then
    '                dtColArr(i).AutoIncrement = True
    '                dtColArr(i).AutoIncrementSeed = 1
    '                dtColArr(i).AutoIncrementStep = 1
    '            End If

    '        Next i
    '        '*********************************************************************
    '        Dt_Target.PrimaryKey = dtColArr

    '        For Each dr_S In Dt_Source.Rows

    '            If Choice_1 = DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

    '                dr_T = Dt_Target.NewRow

    '            Else

    '                For i As Integer = 0 To Pk_ColumnNamesArr.Length - 1
    '                    ColValArr(i) = dr_S(Pk_ColumnNamesArr(i))
    '                Next i

    '                dr_T = Dt_Target.Rows.Find(ColValArr)


    '            End If ' if of checking Data Saving Mode Enum

    '            For Each dc_S As Data.DataColumn In Dt_Source.Columns

    '                If dc_S.ColumnName.ToUpper <> "VUSERCODE" And _
    '                   dc_S.ColumnName.ToUpper <> "DMODIFYON" And _
    '                   dc_S.ColumnName.ToUpper <> "CREPLICAFLAG" And _
    '                   Required_Audit Then


    '                    If Choice_1 = DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then 'Changed by Hiral ( due to DBNULL error in date )

    '                        'IIf(IsDBNull(ConvertDbNullToDbTypeDefaultValue(dr_S(dc_S.ColumnName), dc_S.DataType, Choice_1)), "", ConvertDbNullToDbTypeDefaultValue(dr_S(dc_S.ColumnName), dc_S.DataType, Choice_1))
    '                        'IIf(IsDBNull(ConvertDbNullToDbTypeDefaultValue(dr_T(dc_S.ColumnName), dc_S.DataType, Choice_1)), "", ConvertDbNullToDbTypeDefaultValue(dr_T(dc_S.ColumnName), dc_S.DataType, Choice_1))


    '                        'Null cannot be compared so it is converted to string
    '                        If IIf(IsDBNull(ConvertDbNullToDbTypeDefaultValue(dr_S(dc_S.ColumnName), dc_S.DataType, Choice_1)), "", ConvertDbNullToDbTypeDefaultValue(dr_S(dc_S.ColumnName), dc_S.DataType, Choice_1)) <> _
    '                        IIf(IsDBNull(ConvertDbNullToDbTypeDefaultValue(dr_T(dc_S.ColumnName), dc_S.DataType, Choice_1)), "", ConvertDbNullToDbTypeDefaultValue(dr_T(dc_S.ColumnName), dc_S.DataType, Choice_1)) Then
    '                            'If ConvertDbNullToDbTypeDefaultValue(dr_S(dc_S.ColumnName), dc_S.DataType, Choice_1) <> ConvertDbNullToDbTypeDefaultValue(dr_T(dc_S.ColumnName), dc_S.DataType, Choice_1) Then

    '                            dr_Audit = Dt_Audit.NewRow

    '                            dr_Audit("VTBLNAME") = TblName_1
    '                            dr_Audit("VUSERCODE") = UserCode_1
    '                            dr_Audit("VCOLUMNNAME") = dc_S.ColumnName
    '                            dr_Audit("VOLDVALUE") = dr_T(dc_S.ColumnName)
    '                            dr_Audit("VNEWVALUE") = dr_S(dc_S.ColumnName)

    '                            dr_Audit("VRIDCOLUMNNAME") = dtColArr(0).ColumnName 'Added by Hiral on 22-May-2007
    '                            dr_Audit("NRIDNO") = dr_T(dtColArr(0).ColumnName) 'Added by Hiral on 22-May-2007

    '                            dr_Audit("DMODIFYON") = ServerDateTime_1
    '                            dr_Audit("CREPLICAFLAG") = "N"

    '                            Dt_Audit.Rows.Add(dr_Audit)

    '                        End If

    '                    End If

    '                End If

    '                If Not Dt_Target.Columns(dc_S.ColumnName).AutoIncrement Then

    '                    If dc_S.ColumnName.ToUpper = "VUSERCODE" Then
    '                        dr_T(dc_S.ColumnName) = UserCode_1
    '                    ElseIf dc_S.ColumnName.ToUpper = "DMODIFYON" Then
    '                        dr_T(dc_S.ColumnName) = ServerDateTime_1
    '                    ElseIf dc_S.ColumnName.ToUpper = "CREPLICAFLAG" Then
    '                        dr_T(dc_S.ColumnName) = IIf(Choice_1 = DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "N", "E")
    '                    Else
    '                        dr_T(dc_S.ColumnName) = ConvertDbNullToDbTypeDefaultValue(dr_S(dc_S.ColumnName), dc_S.DataType) ' assign in column
    '                    End If

    '                End If

    '            Next dc_S

    '            If Choice_1 = DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
    '                Dt_Target.Rows.Add(dr_T)
    '            End If

    '        Next dr_S

    '        AssignValues = True

    '    Catch ex As Exception
    '        eStr_Retu = ex.Message
    '        AssignValues = False
    '    End Try

    'End Function

    Public Function AssignValues(ByVal TblName_1 As String, _
                                 ByVal Pk_ColumnNamesArr() As String, _
                                 ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                 ByVal Dt_Source As Data.DataTable, _
                                 ByVal Dt_Target As Data.DataTable, _
                                 ByVal Dt_Audit As Data.DataTable, _
                                 ByVal Required_Audit As Boolean, _
                                 ByVal UserCode_1 As String, _
                                 ByVal ServerDateTime_1 As DateTime, _
                                 ByRef eStr_Retu As String) As Boolean

        Dim dr_S As Data.DataRow = Nothing
        Dim dr_T As Data.DataRow = Nothing
        Dim dr_Audit As Data.DataRow = Nothing
        Dim dtColArr() As DataColumn
        Dim ColValArr() As String
        Dim Dt_TblProp As Data.DataTable = Nothing
        Dim Dr_TblPropArr() As Data.DataRowView = Nothing
        Dim IsAutoId As Boolean = False

        Try
            If Pk_ColumnNamesArr.Length = 0 Then
                eStr_Retu = "Primary Key Column Ref. Is Invalid or Blank"
                Exit Function
            End If

            If Dt_Source Is Nothing Then
                eStr_Retu = "Source Table Ref. Is Invalid or Null"
                Exit Function
            End If

            If Dt_Target Is Nothing Then
                eStr_Retu = "Target Table Ref. Is Invalid or Null"
                Exit Function
            End If

            'Temporary Commented to test SchemeMaster
            If Required_Audit Then
                If Dt_Audit Is Nothing Then
                    eStr_Retu = "Auditorial Table Ref. Is Invalid or Null"
                    Exit Function
                End If
            End If

            '--Retrieve the Column property of the table 'Added by Gunjan on 04-Jan-2007
            '*********************************************************************
            If Not getSqlObjectInfo(TblName_1, Dt_TblProp, eStr_Retu) Then
                eStr_Retu += "Error occured while Retrieving Columns Information of Table"
                Exit Function
            End If

            '-- Creating Primary Key Column Name
            '
            ReDim Preserve ColValArr(Pk_ColumnNamesArr.Length - 1)
            ReDim Preserve dtColArr(Pk_ColumnNamesArr.Length - 1)

            Dt_TblProp.DefaultView.Sort = "ColName"

            For i As Integer = 0 To Pk_ColumnNamesArr.Length - 1

                IsAutoId = False
                Dr_TblPropArr = Dt_TblProp.DefaultView.FindRows(Pk_ColumnNamesArr(i))

                If Dr_TblPropArr.Length > 0 Then
                    IsAutoId = (Dr_TblPropArr(0)("IsAutoId") = 1)
                End If

                dtColArr(i) = Dt_Target.Columns(Pk_ColumnNamesArr(i))

                If IsAutoId Then
                    dtColArr(i).AutoIncrement = True
                    dtColArr(i).AutoIncrementSeed = 1
                    dtColArr(i).AutoIncrementStep = 1
                End If

            Next i
            '*********************************************************************
            Dt_Target.PrimaryKey = dtColArr

            For Each dr_S In Dt_Source.Rows

                If Choice_1 = DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                    dr_T = Dt_Target.NewRow

                Else

                    For i As Integer = 0 To Pk_ColumnNamesArr.Length - 1
                        ColValArr(i) = dr_S(Pk_ColumnNamesArr(i))
                    Next i

                    dr_T = Dt_Target.Rows.Find(ColValArr)


                End If ' if of checking Data Saving Mode Enum

                For Each dc_S As Data.DataColumn In Dt_Source.Columns

                    If dc_S.ColumnName.ToUpper <> "VUSERCODE" And _
                       dc_S.ColumnName.ToUpper <> "DMODIFYON" And _
                       dc_S.ColumnName.ToUpper <> "CREPLICAFLAG" And _
                       Required_Audit Then


                        If Choice_1 = DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then 'Changed by Hiral ( due to DBNULL error in date )

                            'IIf(IsDBNull(ConvertDbNullToDbTypeDefaultValue(dr_S(dc_S.ColumnName), dc_S.DataType, Choice_1)), "", ConvertDbNullToDbTypeDefaultValue(dr_S(dc_S.ColumnName), dc_S.DataType, Choice_1))
                            'IIf(IsDBNull(ConvertDbNullToDbTypeDefaultValue(dr_T(dc_S.ColumnName), dc_S.DataType, Choice_1)), "", ConvertDbNullToDbTypeDefaultValue(dr_T(dc_S.ColumnName), dc_S.DataType, Choice_1))


                            'Null cannot be compared so it is converted to string
                            If IIf(IsDBNull(ConvertDbNullToDbTypeDefaultValue(dr_S(dc_S.ColumnName), dc_S.DataType, Choice_1)), "", ConvertDbNullToDbTypeDefaultValue(dr_S(dc_S.ColumnName), dc_S.DataType, Choice_1)) <> _
                            IIf(IsDBNull(ConvertDbNullToDbTypeDefaultValue(dr_T(dc_S.ColumnName), dc_S.DataType, Choice_1)), "", ConvertDbNullToDbTypeDefaultValue(dr_T(dc_S.ColumnName), dc_S.DataType, Choice_1)) Then
                                'If ConvertDbNullToDbTypeDefaultValue(dr_S(dc_S.ColumnName), dc_S.DataType, Choice_1) <> ConvertDbNullToDbTypeDefaultValue(dr_T(dc_S.ColumnName), dc_S.DataType, Choice_1) Then

                                dr_Audit = Dt_Audit.NewRow

                                dr_Audit("VTBLNAME") = TblName_1
                                dr_Audit("VUSERCODE") = UserCode_1
                                dr_Audit("VCOLUMNNAME") = dc_S.ColumnName
                                dr_Audit("VOLDVALUE") = dr_T(dc_S.ColumnName)
                                dr_Audit("VNEWVALUE") = dr_S(dc_S.ColumnName)

                                dr_Audit("VRIDCOLUMNNAME") = dtColArr(0).ColumnName 'Added by Hiral on 22-May-2007
                                dr_Audit("NRIDNO") = dr_T(dtColArr(0).ColumnName) 'Added by Hiral on 22-May-2007

                                dr_Audit("DMODIFYON") = ServerDateTime_1
                                dr_Audit("CREPLICAFLAG") = "N"

                                Dt_Audit.Rows.Add(dr_Audit)

                            End If

                        End If

                    End If

                    If Not Dt_Target.Columns(dc_S.ColumnName).AutoIncrement Then
                        If Not dc_S.DataType.Name.ToString.ToUpper = "DATETIMEOFFSET" Then
                            If dc_S.ColumnName.ToUpper = "VUSERCODE" Then
                                dr_T(dc_S.ColumnName) = UserCode_1
                            ElseIf dc_S.ColumnName.ToUpper = "DMODIFYON" Then
                                dr_T(dc_S.ColumnName) = ServerDateTime_1
                            ElseIf dc_S.ColumnName.ToUpper = "CREPLICAFLAG" Then
                                dr_T(dc_S.ColumnName) = IIf(Choice_1 = DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "N", "E")
                            Else
                                dr_T(dc_S.ColumnName) = ConvertDbNullToDbTypeDefaultValue(dr_S(dc_S.ColumnName), dc_S.DataType) ' assign in column
                            End If
                        End If
                    End If

                Next dc_S

                If Choice_1 = DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                    Dt_Target.Rows.Add(dr_T)
                End If

            Next dr_S

            AssignValues = True

        Catch ex As Exception
            eStr_Retu = ex.Message
            AssignValues = False
        End Try

    End Function

    Public Function AssignValuesForCRFHDRDTLSUBDTL(ByVal TblName_1 As String, _
                                ByVal Pk_ColumnNamesArr() As String, _
                                ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                ByVal Dt_Source As Data.DataTable, _
                                ByVal Dt_Target As Data.DataTable, _
                                ByVal Dt_Audit As Data.DataTable, _
                                ByVal Required_Audit As Boolean, _
                                ByVal UserCode_1 As String, _
                                ByVal ServerDateTime_1 As DateTime, _
                                ByRef eStr_Retu As String) As Boolean

        Dim dr_S As Data.DataRow = Nothing
        Dim dr_T As Data.DataRow = Nothing
        Dim dr_Audit As Data.DataRow = Nothing
        Dim dtColArr() As DataColumn
        Dim ColValArr() As String
        Dim Dt_TblProp As Data.DataTable = Nothing
        Dim Dr_TblPropArr() As Data.DataRowView = Nothing
        Dim IsAutoId As Boolean = False

        Try
            If Pk_ColumnNamesArr.Length = 0 Then
                eStr_Retu = "Primary Key Column Ref. Is Invalid or Blank"
                Exit Function
            End If

            If Dt_Source Is Nothing Then
                eStr_Retu = "Source Table Ref. Is Invalid or Null"
                Exit Function
            End If

            If Dt_Target Is Nothing Then
                eStr_Retu = "Target Table Ref. Is Invalid or Null"
                Exit Function
            End If

            'Temporary Commented to test SchemeMaster
            If Required_Audit Then
                If Dt_Audit Is Nothing Then
                    eStr_Retu = "Auditorial Table Ref. Is Invalid or Null"
                    Exit Function
                End If
            End If

            '--Retrieve the Column property of the table 'Added by Gunjan on 04-Jan-2007
            '*********************************************************************
            If Not getSqlObjectInfo(TblName_1, Dt_TblProp, eStr_Retu) Then
                eStr_Retu += "Error occured while Retrieving Columns Information of Table"
                Exit Function
            End If

            '-- Creating Primary Key Column Name
            '
            ReDim Preserve ColValArr(Pk_ColumnNamesArr.Length - 1)
            ReDim Preserve dtColArr(Pk_ColumnNamesArr.Length - 1)

            Dt_TblProp.DefaultView.Sort = "ColName"

            For i As Integer = 0 To Pk_ColumnNamesArr.Length - 1

                IsAutoId = False
                Dr_TblPropArr = Dt_TblProp.DefaultView.FindRows(Pk_ColumnNamesArr(i))

                If Dr_TblPropArr.Length > 0 Then
                    IsAutoId = (Dr_TblPropArr(0)("IsAutoId") = 1)
                End If

                dtColArr(i) = Dt_Target.Columns(Pk_ColumnNamesArr(i))

                If IsAutoId Then
                    dtColArr(i).AutoIncrement = True
                    dtColArr(i).AutoIncrementSeed = 1
                    dtColArr(i).AutoIncrementStep = 1
                End If

            Next i
            '*********************************************************************
            Dt_Target.PrimaryKey = dtColArr

            For Each dr_S In Dt_Source.Rows

                If Choice_1 = DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                    dr_T = Dt_Target.NewRow

                Else

                    For i As Integer = 0 To Pk_ColumnNamesArr.Length - 1
                        ColValArr(i) = dr_S(Pk_ColumnNamesArr(i))
                    Next i

                    dr_T = Dt_Target.Rows.Find(ColValArr)


                End If ' if of checking Data Saving Mode Enum

                For Each dc_S As Data.DataColumn In Dt_Source.Columns

                    If dc_S.ColumnName.ToUpper <> "VUSERCODE" And _
                       dc_S.ColumnName.ToUpper <> "DMODIFYON" And _
                       dc_S.ColumnName.ToUpper <> "CREPLICAFLAG" And _
                       Required_Audit Then


                        If Choice_1 = DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then 'Changed by Hiral ( due to DBNULL error in date )

                            'IIf(IsDBNull(ConvertDbNullToDbTypeDefaultValue(dr_S(dc_S.ColumnName), dc_S.DataType, Choice_1)), "", ConvertDbNullToDbTypeDefaultValue(dr_S(dc_S.ColumnName), dc_S.DataType, Choice_1))
                            'IIf(IsDBNull(ConvertDbNullToDbTypeDefaultValue(dr_T(dc_S.ColumnName), dc_S.DataType, Choice_1)), "", ConvertDbNullToDbTypeDefaultValue(dr_T(dc_S.ColumnName), dc_S.DataType, Choice_1))


                            'Null cannot be compared so it is converted to string
                            If IIf(IsDBNull(ConvertDbNullToDbTypeDefaultValue(dr_S(dc_S.ColumnName), dc_S.DataType, Choice_1)), "", ConvertDbNullToDbTypeDefaultValue(dr_S(dc_S.ColumnName), dc_S.DataType, Choice_1)) <> _
                            IIf(IsDBNull(ConvertDbNullToDbTypeDefaultValue(dr_T(dc_S.ColumnName), dc_S.DataType, Choice_1)), "", ConvertDbNullToDbTypeDefaultValue(dr_T(dc_S.ColumnName), dc_S.DataType, Choice_1)) Then
                                'If ConvertDbNullToDbTypeDefaultValue(dr_S(dc_S.ColumnName), dc_S.DataType, Choice_1) <> ConvertDbNullToDbTypeDefaultValue(dr_T(dc_S.ColumnName), dc_S.DataType, Choice_1) Then

                                dr_Audit = Dt_Audit.NewRow

                                dr_Audit("VTBLNAME") = TblName_1
                                dr_Audit("VUSERCODE") = UserCode_1
                                dr_Audit("VCOLUMNNAME") = dc_S.ColumnName
                                dr_Audit("VOLDVALUE") = dr_T(dc_S.ColumnName)
                                dr_Audit("VNEWVALUE") = dr_S(dc_S.ColumnName)

                                dr_Audit("VRIDCOLUMNNAME") = dtColArr(0).ColumnName 'Added by Hiral on 22-May-2007
                                dr_Audit("NRIDNO") = dr_T(dtColArr(0).ColumnName) 'Added by Hiral on 22-May-2007

                                dr_Audit("DMODIFYON") = ServerDateTime_1
                                dr_Audit("CREPLICAFLAG") = "N"

                                Dt_Audit.Rows.Add(dr_Audit)

                            End If

                        End If

                    End If

                    If Not Dt_Target.Columns(dc_S.ColumnName).AutoIncrement Then
                        If Not dc_S.DataType.Name.ToString.ToUpper = "DATETIMEOFFSET" Then
                            If dc_S.ColumnName.ToUpper = "VUSERCODE" Then
                                dr_T(dc_S.ColumnName) = UserCode_1
                                ''ElseIf dc_S.ColumnName.ToUpper = "DMODIFYON" Then
                                ''dr_T(dc_S.ColumnName) = ServerDateTime_1
                            ElseIf dc_S.ColumnName.ToUpper = "CREPLICAFLAG" Then
                                dr_T(dc_S.ColumnName) = IIf(Choice_1 = DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "N", "E")
                            Else
                                dr_T(dc_S.ColumnName) = ConvertDbNullToDbTypeDefaultValue(dr_S(dc_S.ColumnName), dc_S.DataType) ' assign in column
                            End If
                        End If
                    End If

                Next dc_S

                If Choice_1 = DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                    Dt_Target.Rows.Add(dr_T)
                End If

            Next dr_S

            AssignValuesForCRFHDRDTLSUBDTL = True

        Catch ex As Exception
            eStr_Retu = ex.Message
            AssignValuesForCRFHDRDTLSUBDTL = False
        End Try

    End Function

    Public Function AssignValuesinDeActivateScheme(ByVal TblName_1 As String, _
                                   ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                   ByVal columnName As String, _
                                   ByVal nRID As Integer, _
                                   ByVal OldVal As Date, _
                                   ByVal NewVal As Date, _
                                   ByVal Dt_Audit As Data.DataTable, _
                                   ByVal Required_Audit As Boolean, _
                                   ByVal UserCode_1 As String, _
                                   ByRef eStr_Retu As String) As Boolean
        Try
            If Required_Audit Then


                If Choice_1 = DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                    Dim dr_Audit As Data.DataRow
                    dr_Audit = Dt_Audit.NewRow

                    dr_Audit("VTBLNAME") = TblName_1
                    dr_Audit("VUSERCODE") = UserCode_1
                    dr_Audit("VCOLUMNNAME") = columnName
                    dr_Audit("VOLDVALUE") = OldVal
                    dr_Audit("VNEWVALUE") = NewVal
                    dr_Audit("VRIDCOLUMNNAME") = "NRID"
                    dr_Audit("NRIDNO") = nRID
                    Dt_Audit.Rows.Add(dr_Audit)

                End If

            End If
            Return True
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try

    End Function

    Friend Function ConvertDbNullToDbTypeDefaultValue(ByVal Val As Object, ByVal DbType_1 As System.Type, Optional ByVal Choice_1 As DataObjOpenSaveModeEnum = DataObjOpenSaveModeEnum.DataObjOpenMode_None) As Object
        Dim DbTypeName_1 As String = ""
        Dim defDateTime As DateTime
        Dim DefChar As Char

        DbTypeName_1 = DbType_1.Name.ToUpper

        If DbTypeName_1 = "STRING" Then

            Try
                'Added by Hiral on 16-Aug-2007 to allow NULL instead of space
                If IsDBNull(Val) Then
                    ConvertDbNullToDbTypeDefaultValue = Val
                Else
                    ConvertDbNullToDbTypeDefaultValue = Val.ToString
                End If
            Catch ex As Exception
                ConvertDbNullToDbTypeDefaultValue = ""
            End Try

        ElseIf DbTypeName_1 = "CHAR" Then

            Try
                'Added by Hiral on 16-Aug-2007
                If IsDBNull(Val) Then
                    ConvertDbNullToDbTypeDefaultValue = Val
                Else
                    ConvertDbNullToDbTypeDefaultValue = Convert.ToChar(Val)
                End If

            Catch ex As Exception
                ConvertDbNullToDbTypeDefaultValue = DefChar
            End Try


        ElseIf DbTypeName_1 = "DATETIME" Then

            ConvertDbNullToDbTypeDefaultValue = Val

            'AS PER DISCUSSION WITH YASHESH SIR
            Try
                ConvertDbNullToDbTypeDefaultValue = Convert.ToDateTime(Val)
            Catch ex As Exception
                'Added by Hiral to enter NULL in DATETIME field in database
                If Choice_1 = DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                    If IsDBNull(Val) Then
                        ConvertDbNullToDbTypeDefaultValue = Val
                    Else
                        ConvertDbNullToDbTypeDefaultValue = defDateTime
                    End If
                ElseIf Choice_1 = DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                    ConvertDbNullToDbTypeDefaultValue = defDateTime
                End If

            End Try

        ElseIf DbTypeName_1 = "BOOLEAN" Then

            Try
                ConvertDbNullToDbTypeDefaultValue = Convert.ToBoolean(Val)
            Catch ex As Exception
                ConvertDbNullToDbTypeDefaultValue = False
            End Try

        ElseIf DbTypeName_1 = "DECIMAL" Or DbTypeName_1 = "INT16" Or DbTypeName_1 = "INT32" Or
               DbTypeName_1 = "INT64" Or DbTypeName_1 = "DOUBLE" Or DbTypeName_1 = "BYTE" Or
               DbTypeName_1 = "SINGLE" Then

            If IsDBNull(Val) Then
                ConvertDbNullToDbTypeDefaultValue = 0
            Else
                ConvertDbNullToDbTypeDefaultValue = Val
            End If

            'Added by Bhargav Thaker 28Feb2023
        ElseIf DbTypeName_1 = "GUID" Then
            If IsDBNull(val) Then
                ConvertDbNullToDbTypeDefaultValue = Guid.NewGuid()
            Else
                ConvertDbNullToDbTypeDefaultValue = Val
            End If
        End If

    End Function

    Public Sub Dispose()
        _objDtLogic = Nothing
    End Sub

    Protected Overrides Sub Finalize()
        Me.Dispose()
    End Sub

End Class

'***************KNET Functionalities

Public Structure FileType
    Private _Name As String
    Private _Path As String
    Private _FullName As String
    Private _File As File

    ''' <summary>
    '''  Returns FileName as string
    ''' </summary>        
    Public ReadOnly Property FileName() As String
        Get
            Return _Name
        End Get
    End Property
    ''' <summary>
    ''' Retrun path of parent directory of file as string
    ''' </summary>    
    Public ReadOnly Property FilePath() As String
        Get
            Return _Path
        End Get
    End Property
    ''' <summary>
    ''' Get or Set FullPath of File as string
    ''' </summary>    
    Public Property FullName() As String
        Get
            Return _FullName
        End Get
        Set(ByVal FullPath As String)
            FullPath = FullPath.Replace("/", "\")
            _FullName = FullPath
            _Path = FullPath.Substring(0, FullPath.LastIndexOf("\"))
            _Name = FullPath.Substring(FullPath.LastIndexOf("\") + 1)
        End Set
    End Property
    ''' <summary>
    ''' Set file object or Get file object
    ''' </summary>    
    Public Property File() As File
        Get
            Return _File
        End Get
        Set(ByVal File As File)
            _File = File
        End Set
    End Property
End Structure
Public Module WorkspaceFile
    Public Const TRACKING_TABLE_XML As String = "Tracking Table"
    Public Const PIM_ZIP As String = ""
    Public Const TRACKING_TABLE_XML_FOR_EU14 As String = "EU14 Tracking Table"
End Module

'**********************