#Region " Name Spaces"

Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Configuration

#End Region

Public Class ClsDataLogic_New

#Region "Private Variable"
    Private objConnection As SqlConnection
    Private objCommand As SqlCommand
    Private objTransaction As SqlTransaction
    Private objDataAdapter As SqlDataAdapter
    Private objReader As SqlDataReader

    Private _InnerCall As Boolean
#End Region

#Region "Database Connection SubRutines"

    Private Sub InOpenConnection(ByVal InnerCall_1 As Boolean)

        If objConnection Is Nothing Then
            objConnection = New SqlConnection
            objConnection.ConnectionString = SS.Web.ServerHelper.DefaultDecryption(System.Configuration.ConfigurationManager.AppSettings("connstring"))
            'objConnection.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("connstring")
            objConnection.Open()
            _InnerCall = InnerCall_1
        End If

    End Sub

    Private Sub InCloseConnection(ByVal InnerCall_1 As Boolean)

        If Not objConnection Is Nothing Then
            '
            '  when user call open connection method with in this class then it is consider as inner call
            ' if IncloseConnection is called by this class and open connection is alose called by this class then
            ' this function close the connection.
            ' But, If user call Open connection from out side of this calss and Close connection is call from
            ' this class then connection object is not going to be closed. Close the Connection object will be 
            ' reposibility of caller of Openconnection Method.

            If InnerCall_1 And (Not _InnerCall) Then
                Exit Sub
            End If

            objConnection.Close()
            objConnection.Dispose()
            objConnection = Nothing
        End If

    End Sub

    Public Sub Open_Connection()
        InOpenConnection(False)
    End Sub

    Public Sub Close_Connection()
        InCloseConnection(False)
    End Sub

    Public Sub Begin_Transaction()

        InOpenConnection(True)

        If objTransaction Is Nothing Then
            objTransaction = objConnection.BeginTransaction()
        End If

    End Sub

    Public Sub Commit_Transaction()

        If Not objTransaction Is Nothing Then
            objTransaction.Commit()
            objTransaction = Nothing
        End If
        InCloseConnection(True)
    End Sub

    Public Sub RollBack_Transaction()
        If Not objTransaction Is Nothing Then
            objTransaction.Rollback()
            objTransaction = Nothing
        End If
        InCloseConnection(True)

    End Sub

    Public Sub Dispose()
        Try
            If Not objCommand Is Nothing Then
                objCommand.Dispose()
                objCommand = Nothing
            End If
        Catch ex As Exception

        End Try

        Try
            If Not objTransaction Is Nothing Then
                objTransaction.Dispose()
                objTransaction = Nothing
            End If
        Catch ex As Exception
        End Try

        Try
            If Not objDataAdapter Is Nothing Then
                objDataAdapter.Dispose()
                objDataAdapter = Nothing
            End If
        Catch ex As Exception
        End Try

        Try
            If Not objConnection Is Nothing Then
                objConnection.Close()
                objConnection = Nothing
            End If

        Catch ex As Exception
        End Try

    End Sub

    Protected Overloads Sub Finalize()
        Me.Dispose()
    End Sub

#End Region

#Region "PropertyCollection"

    Friend ReadOnly Property Connection() As Data.SqlClient.SqlConnection
        Get
            Connection = objConnection
        End Get
    End Property

    Friend ReadOnly Property Transaction() As Data.SqlClient.SqlTransaction
        Get
            Transaction = objTransaction
        End Get
    End Property

#End Region

#Region "Data Retrival Function "

    Public Function GetResultSet(ByVal strQuery As String, ByRef dsReturn As DataSet) As Boolean

        Try
            Me.InOpenConnection(True)
            objCommand = New SqlCommand
            objCommand.Connection = objConnection
            objCommand.CommandType = CommandType.Text
            objCommand.CommandText = strQuery

            objDataAdapter = New SqlDataAdapter
            objDataAdapter.SelectCommand = objCommand

            If Not (Me.objTransaction Is Nothing) Then
                objCommand.Transaction = objTransaction
            End If

            If dsReturn.Tables.Count <> 0 Then
                dsReturn.Tables.Clear()
            End If
            objDataAdapter.Fill(dsReturn)
            Return True

        Catch ex As Exception
            Throw ex
        Finally
            Me.InCloseConnection(True)
        End Try


    End Function

    Public Function GetResultSet(ByVal strQuery As String, ByRef dtReturn As DataTable) As Boolean

        Try
            Me.InOpenConnection(True)
            objCommand = New SqlCommand
            objCommand.Connection = objConnection
            objCommand.CommandType = CommandType.Text
            objCommand.CommandText = strQuery

            objDataAdapter = New SqlDataAdapter
            objDataAdapter.SelectCommand = objCommand

            If Not (Me.objTransaction Is Nothing) Then
                objCommand.Transaction = objTransaction
            End If

            If dtReturn Is Nothing Then
                dtReturn = New Data.DataTable
            End If

            objDataAdapter.Fill(dtReturn)

            Return True
        Catch ex As Exception
            Throw ex
        Finally
            Me.InCloseConnection(True)
        End Try


    End Function

    Public Function ExecuteQuery_DataSet(ByVal strQuery As String, _
                                        Optional ByVal CommandTimeOut_1 As Integer = -1) As DataSet

        Dim objDataSet As New DataSet

        Try
            objCommand = New SqlCommand
            objDataAdapter = New SqlDataAdapter

            InOpenConnection(True)

            objCommand.Connection = objConnection
            objCommand.CommandText = strQuery
            objCommand.CommandType = CommandType.Text
            objCommand.Transaction = objTransaction
            objDataAdapter.SelectCommand = objCommand


            If CommandTimeOut_1 >= 0 Then
                objCommand.CommandTimeout = CommandTimeOut_1
            End If
            objCommand.CommandTimeout = 10000
            objDataAdapter.Fill(objDataSet)

        Catch ex As Exception
            Throw ex
            Exit Function
        Finally
            InCloseConnection(True)
        End Try

        Return objDataSet

    End Function

    Friend Function GetDataset(ByVal TblName_1 As String, _
                               ByVal Query_1 As String, _
                               ByVal WhereCondition_1 As String, _
                               ByVal DataRetrival_1 As DataRetrievalModeEnum, _
                               ByRef Sql_DtTbl_Retu As Data.DataTable) As Boolean

        Dim SqlAdp As Data.SqlClient.SqlDataAdapter = Nothing
        Dim Sql_Cmd As Data.SqlClient.SqlCommand = Nothing
        Dim qStr As String = ""

        TblName_1 = TblName_1.ToUpper.Trim

        Try
            If TblName_1 = "" Then
                Throw New Exception("Invalid Table Ref.")
                Exit Function
            End If

            If DataRetrival_1 = DataRetrievalModeEnum.DatatTable_Query Then

                If Query_1.Trim = "" Then
                    Throw New Exception("Pass Query String Is Invalid or Blank")
                    Exit Function
                End If

                qStr = Query_1

            Else

                qStr = "Select * From " + TblName_1

                If DataRetrival_1 = DataRetrievalModeEnum.DataTable_Empty Then

                    WhereCondition_1 = " Where 1 = 2 "

                ElseIf DataRetrival_1 = DataRetrievalModeEnum.DataTable_WithWhereCondition Then

                    If WhereCondition_1.Trim = "" Then
                        Throw New Exception("Invalid or Blank Where Condtion")
                        Exit Function
                    End If
                    WhereCondition_1 = " Where " + WhereCondition_1
                End If

                qStr = qStr + WhereCondition_1

            End If

            InOpenConnection(True)

            Sql_Cmd = New Data.SqlClient.SqlCommand(qStr, Me.objConnection, Me.objTransaction)
            Sql_Cmd.CommandTimeout = 10000
            SqlAdp = New Data.SqlClient.SqlDataAdapter(Sql_Cmd)

            Sql_DtTbl_Retu = New Data.DataTable

            SqlAdp.Fill(Sql_DtTbl_Retu)
            Sql_DtTbl_Retu.TableName = TblName_1

            GetDataset = True
        Catch ex As Exception
            Throw ex
            Exit Function
        Finally
            InCloseConnection(True)
        End Try

    End Function

    Friend Function GetServerDateTime() As DateTime

        Dim qStr As String = ""
        Try
            InOpenConnection(True)

            qStr = "Select GetDate()"
            GetServerDateTime = Me.ExecuteQuery_Scalar(qStr)

            InCloseConnection(True)

        Catch ex As Exception
            InCloseConnection(True)
            Throw ex
            Exit Function
        End Try

    End Function

    Public Function ExecuteQuery_Scalar(ByVal strQuery As String) As Object

        Dim objscalar As Object

        Try
            InOpenConnection(True)

            objCommand = New SqlCommand(strQuery, objConnection, objTransaction)
            objscalar = objCommand.ExecuteScalar
            InCloseConnection(True)

        Catch ex As Exception
            InCloseConnection(True)
            Throw ex
            Exit Function
        End Try

        Return objscalar

    End Function

    Public Function Add_SqlParameter(ByVal ParamName As String, ByVal ParamValue As String, Optional ByVal ParamDire As ParameterDirection = ParameterDirection.Input) As SqlParameter
        Dim sqlParam As SqlParameter
        sqlParam = New SqlParameter
        sqlParam.ParameterName = ParamName
        sqlParam.Value = ParamValue
        sqlParam.Direction = ParamDire
        Return sqlParam
    End Function

    Public Function ExecuteSP_DataSet(ByVal strProcedure As String, _
                                      ByVal colParam As ArrayList) As DataSet
        Dim objDataSet As New DataSet
        Dim param As Object

        Try
            objDataAdapter = New SqlDataAdapter

            Me.InOpenConnection(True)

            objCommand = New SqlCommand(strProcedure, objConnection)
            objCommand.CommandType = CommandType.StoredProcedure

            If Not (Me.objTransaction Is Nothing) Then
                objCommand.Transaction = objTransaction
            End If

            For Each param In colParam
                objCommand.Parameters.Add(param)

            Next param

            objDataAdapter.SelectCommand = objCommand
            objDataAdapter.Fill(objDataSet)
            Return objDataSet

        Catch ex As Exception
            Throw ex
            Exit Function
        Finally
            Me.InCloseConnection(True)
        End Try

    End Function

#End Region

#Region "ProcedureExecute"

    ''' <summary>
    ''' Function Executes Stored Procedure and Returns DataSet
    ''' </summary>
    ''' <param name="strProcedureName">Name of Stored Procedure</param>
    ''' <param name="strParamValue">Parameter Values Concatenated By '##' sign</param>
    ''' <returns>Returns System.Data.DataSet</returns>
    ''' <remarks>Parameter Values must be Concatenated By '##'</remarks>
    ''' 

    Public Function ProcedureExecute(ByVal strProcedureName As String, Optional ByVal strParamValue As String = "") As DataSet
        Dim i As Integer
        Dim ParamValue() As String
        Dim ds As New DataSet
        Dim dsColumn As New DataSet

        Try
            dsColumn = ExecuteQuery_DataSet("exec sp_help '" & strProcedureName & "'", "Columns")

            ParamValue = Split(strParamValue, "##")

            Me.InOpenConnection(True)

            objDataAdapter = New SqlDataAdapter(strProcedureName, objConnection)
            objDataAdapter.SelectCommand.CommandTimeout = 0
            objDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

            If Not (Me.objTransaction Is Nothing) Then
                objCommand.Transaction = objTransaction
            End If

            For i = 0 To dsColumn.Tables(1).Rows.Count - 1
                objDataAdapter.SelectCommand.Parameters.Add(Add_SqlParameter(dsColumn.Tables(1).Rows(i).Item("Parameter_Name"), ParamValue(i)))
            Next

            objDataAdapter.Fill(ds)
            Return ds
            objDataAdapter = Nothing
        Catch ex As Exception
            Return Nothing
        Finally
            Me.InCloseConnection(True)
        End Try
    End Function

    Public Function ProcedureExecute_withoutReturn(ByVal strProcedureName As String, Optional ByVal strParamValue As String = "") As Boolean
        Dim i As Integer
        Dim ParamValue() As String
        Dim ds As New DataSet
        Dim dsColumn As New DataSet

        Try
            dsColumn = ExecuteQuery_DataSet("exec sp_help '" & strProcedureName & "'", "Columns")

            ParamValue = Split(strParamValue, "##")

            Me.InOpenConnection(True)

            objDataAdapter = New SqlDataAdapter(strProcedureName, objConnection)
            objDataAdapter.SelectCommand.CommandTimeout = 0
            objDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

            If Not (Me.objTransaction Is Nothing) Then
                objCommand.Transaction = objTransaction
            End If

            For i = 0 To dsColumn.Tables(1).Rows.Count - 1
                objDataAdapter.SelectCommand.Parameters.Add(Add_SqlParameter(dsColumn.Tables(1).Rows(i).Item("Parameter_Name"), ParamValue(i)))
            Next

            'objDataAdapter.Fill(ds)
            objDataAdapter = Nothing
            Return True

        Catch ex As Exception
            Return False
        Finally
            Me.InCloseConnection(True)
        End Try
    End Function

#End Region

    Public Function ExecuteQuery_DataSet(ByVal strQuery As String, ByVal strTableName As String) As DataSet

        Dim objDataSet As New DataSet(strTableName)

        Try
            objCommand = New SqlCommand
            objDataAdapter = New SqlDataAdapter

            Me.InOpenConnection(True)

            objCommand.Connection = objConnection
            objCommand.CommandText = strQuery
            objCommand.CommandType = CommandType.Text
            objCommand.Transaction = objTransaction
            objCommand.CommandTimeout = 1000000
            objDataAdapter.SelectCommand = objCommand
            objDataAdapter.Fill(objDataSet, strTableName)

        Catch ex As Exception
            Throw ex
            Exit Function
        Finally
            Me.InCloseConnection(True)
        End Try

        Return objDataSet

    End Function

    Public Function ExecuteQuery_Boolean(ByVal strQuery As String, ByRef eStr_Retu As String) As Boolean
        Dim iResult As Int16
        Try

            objCommand = New SqlCommand
            objDataAdapter = New SqlDataAdapter

            Me.InOpenConnection(True)

            objCommand.Connection = objConnection
            objCommand.CommandText = strQuery
            objCommand.CommandType = CommandType.Text
            objCommand.Transaction = objTransaction
            objCommand.CommandTimeout = 0
            iResult = objCommand.ExecuteNonQuery()
            If iResult >= 1 Then
                eStr_Retu = "Record Saved Successfully"
                ExecuteQuery_Boolean = True
            Else
                eStr_Retu = "Record Not Saved"
                ExecuteQuery_Boolean = True
            End If
            'Me.InCloseConnection(True)

        Catch ex As Exception
            eStr_Retu = ex.Message.ToString
            ExecuteQuery_Boolean = False
        Finally
            Me.InCloseConnection(True)
        End Try

    End Function

    Friend Function GetLocationWiseTime(ByVal userid As Integer) As String
        Dim qStr As String = ""
        Try
            InOpenConnection(True)

            qStr = "Select CONVERT( varchar(17),dbo.GetLocationWiseTime(" + userid.ToString + "),113)"

            'dtoffset = Me.ExecuteQuery_Scalar(qStr)
            GetLocationWiseTime = Me.ExecuteQuery_Scalar(qStr)

            InCloseConnection(True)

        Catch ex As Exception
            InCloseConnection(True)
            Throw ex
            Exit Function
        End Try

    End Function

    Public Function ProcedureExecute_WorkspaceNodeDetail(ByVal strProcedureName As String, ByVal DataTable_WorkspaceNodeDetail As DataTable, _
                                                 ByVal WorkSpaceId As String, _
                                                 ByVal ParentNodeId As Integer, _
                                                 ByVal DataOpMode As Integer) As Boolean
        Dim i As Integer
        Dim ParamValue() As String
        Dim ds As New DataSet
        Dim dsColumn As New DataSet
        ' Dim parameter As SqlParameter
        Try
            Me.InOpenConnection(True)

            objDataAdapter = New SqlDataAdapter(strProcedureName, objConnection)
            objDataAdapter.SelectCommand.CommandTimeout = 0
            objDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

            If Not (Me.objTransaction Is Nothing) Then
                objCommand.Transaction = objTransaction
            End If

            objDataAdapter.SelectCommand.Parameters.Add("@WorkSpaceNodeDetail_Temp", DataTable_WorkspaceNodeDetail)
            objDataAdapter.SelectCommand.Parameters.Add("@vWorkSpaceId", WorkSpaceId)
            objDataAdapter.SelectCommand.Parameters.Add("@iParentNodeId", ParentNodeId)
            objDataAdapter.SelectCommand.Parameters.Add("@DATAOPMODE", DataOpMode)
            objDataAdapter.SelectCommand.ExecuteNonQuery()

            Return True
            objDataAdapter = Nothing
        Catch ex As Exception
            Return False
        Finally
            Me.InCloseConnection(True)
        End Try
    End Function


End Class
