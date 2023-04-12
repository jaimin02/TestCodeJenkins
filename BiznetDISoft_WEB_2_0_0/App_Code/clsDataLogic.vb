Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO

Public Class clsDataLogic

    Dim ClassNAme As String = "clsDataLogic"
    Dim ErrMsg As String
    Dim objConn As SqlConnection
    Dim objSqlCommand As SqlCommand
    Dim objTran As SqlTransaction
    Dim objDataAdpt As SqlDataAdapter
    Dim ds As DataSet
    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")

    ''Execute Query and Return Datatable
    Public Function ExecuteQuery(ByVal StrQuery As String, ByVal strTableNAme As String) As DataTable
        Dim ErrLoc As String = ClassNAme + ".ExecuteQuery(strquery,strTableName)"
        Try

            ds = New DataSet
            objSqlCommand = New SqlCommand
            objSqlCommand.Connection = New SqlClient.SqlConnection(System.Configuration.ConfigurationManager.AppSettings("connstring"))
            objSqlCommand.CommandType = CommandType.Text
            objSqlCommand.CommandText = StrQuery

            objDataAdpt = New SqlDataAdapter
            objDataAdpt.SelectCommand = objSqlCommand

            If Not (Me.objTran Is Nothing) Then
                objSqlCommand.Transaction = objTran
            End If

            If Not ds.Tables("" & strTableNAme & "") Is Nothing Then
                ds.Tables("" & strTableNAme & "").Clear()
            End If

            objDataAdpt.Fill(ds)
            CloseConnection()
            Return ds.Tables(0)
        Catch ex As Exception

        End Try
    End Function
    Public Sub OpenConnection()
        If objConn Is Nothing Then
            objConn = New SqlConnection
            objConn.ConnectionString = strConn
            objConn.Open()
        End If
    End Sub
    Public Sub CloseConnection()
        If Not objConn Is Nothing Then
            objConn.Close()
            objConn.Dispose()
            objConn = Nothing
        End If
    End Sub
    Public Function SerializeObject(ByVal Sql_DataSet As DataSet) As String
        Try
            Dim serializer As System.Web.Script.Serialization.JavaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
            Dim rows As New Generic.List(Of Generic.Dictionary(Of String, Object))
            Dim row As Generic.Dictionary(Of String, Object)
            Dim dt As DataTable = Sql_DataSet.Tables(0)
            For Each dr As DataRow In dt.Rows
                row = New Generic.Dictionary(Of String, Object)
                For Each col As DataColumn In dt.Columns
                    row.Add(col.ColumnName, dr(col))
                Next
                rows.Add(row)
            Next

            Return serializer.Serialize(rows)
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try

    End Function
End Class
