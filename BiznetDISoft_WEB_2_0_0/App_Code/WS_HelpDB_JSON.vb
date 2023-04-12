Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Linq
Imports System.Collections.Generic

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class WS_HelpDB_JSON
    Inherits System.Web.Services.WebService
#Region "VARIABLE DECLARATION"
    Private Shared objCommon As New clsCommon
    Private Shared objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private Shared objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
    Private Shared objDataLogic As New clsDataLogic
#End Region

    <WebMethod()> _
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function

#Region "Proc_ActivityWiseVersionControl "
    <WebMethod()> _
    Public Function Proc_ActivityWiseVersionControl(ByVal Parameters As String) As String
        Dim Sql_DataSet As New DataSet
        Dim estr As String = String.Empty
        Try
            If Not objHelp.Proc_ActivityWiseVersionControl(Parameters, Sql_DataSet, estr) Then
                Throw New Exception("Error While Getting ActivityWise Report")
            End If
            Proc_ActivityWiseVersionControl = objDataLogic.SerializeObject(Sql_DataSet)
            Return Proc_ActivityWiseVersionControl
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try
    End Function
#End Region '===============Added by Megha=============

#Region "CDCGetData"
    <WebMethod()> _
    Public Function CDCGetData(ByVal TableName As String, _
                               ByVal ColumnNames As String, _
                               ByVal WhereCondition As String _
                               ) As String
        Dim Sql_DataSet As New DataSet
        Dim estr As String = String.Empty
        Dim ddl As New DropDownList

        Try
            If Not objHelp.GetData(TableName, ColumnNames, WhereCondition, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Sql_DataSet, estr) Then
                Throw New Exception("Error While Getting Data")
            End If
            CDCGetData = objDataLogic.SerializeObject(Sql_DataSet)
            'strJson = JsonConvert.SerializeObject(Sql_DataSet.Tables(0))

            Return CDCGetData
        Catch ex As Exception

            Return ex.Message.ToString()
        End Try
    End Function
#End Region
#Region "GetData"
    <WebMethod()> _
    Public Function GetData_JSON(ByVal TableName As String, _
                            ByVal ColumnNames As String, _
                            ByVal WhereCondition As String _
                            ) As String

        Dim Sql_DataSet As New DataSet
        Dim estr As String = String.Empty
        Dim ddl As New DropDownList
        Dim ddlValue As DataColumn
        Try
            If Not objHelp.GetData(TableName, ColumnNames, WhereCondition, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Sql_DataSet, estr) Then
                Throw New Exception("Error While Getting Data")
            End If
            ddlValue = New DataColumn("iNoeIdnVersionNo", System.Type.GetType("System.String"))
            Sql_DataSet.Tables(0).Columns.Add(ddlValue)
            For Each dr As DataRow In Sql_DataSet.Tables(0).Rows
                dr.Item("iNoeIdnVersionNo") = dr.Item("iNodeId").ToString & "," & dr.Item("nVersionNo").ToString
            Next
            Sql_DataSet.AcceptChanges()
            ddl.DataSource() = Sql_DataSet.Tables(0)
            ddl.DataTextField = "vNodeDisplayName"
            ddl.DataValueField = "iNoeIdnVersionNo"
            ddl.ID = "ddlAcitivity"
            ddl.Attributes.Add("onChange", "fnOnSelectedIndexChange(this);")
            ddl.DataBind()
            ddl.Items.Insert(0, "-- Select Activity--")

            Dim sw As New StringWriter()
            Dim hw As New HtmlTextWriter(sw)
            ddl.RenderControl(hw)

            GetData_JSON = sw.GetStringBuilder().ToString
        Catch ex As Exception
            Return ex.ToString()
        End Try
    End Function
#End Region '=================== Added By Megha ================

#Region "Proc_AttributeWiseVersionControl "
    <WebMethod()> _
    Public Function Proc_AttributeWiseVersionControl(ByVal Parameters As String) As String
        Dim Sql_DataSet As New DataSet
        Dim estr As String = String.Empty
        Try
            If Not objHelp.Proc_AttributeWiseVersionControl(Parameters, Sql_DataSet, estr) Then
                Throw New Exception("Error While Getting ActivityWise Report")
            End If
            Proc_AttributeWiseVersionControl = objDataLogic.SerializeObject(Sql_DataSet)
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try
    End Function
#End Region '===============Added by Megha=============

#Region "Get BA Project Files"
    <WebMethod()> _
    Public Function GetProjectFiles(ByVal Parameters As String) As String
        Dim dsBAFiles As New System.Data.DataSet
        dsBAFiles = objHelp.GetResultSet("Select * from View_BASampleConcentrationFiles where vWorkSpaceId='" & Parameters.Trim & "'", "BASampleConcentrationFiles")
        'dsBAFiles = ObjHelp.GetResultSet("Select * from View_BASampleConcentrationFiles where vWorkSpaceId='" & HProjectId.Value.ToString.Trim & "'", "BASampleConcentrationFiles")
        'ViewState(VS_BASampleConcentrationFiles) = dsBAFiles
        'gvUplodedFiles.DataSource = dsBAFiles.Tables(0)
        'gvUplodedFiles.DataBind()
        Dim strResult As String = GetJson(dsBAFiles.Tables(0)) '"Akhilesh" '
        Return strResult
    End Function

    Public Function GetJson(ByVal dt As DataTable) As String

        Dim serializer As System.Web.Script.Serialization.JavaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim rows As New List(Of Dictionary(Of String, Object))
        Dim row As Dictionary(Of String, Object)

        For Each dr As DataRow In dt.Rows
            row = New Dictionary(Of String, Object)
            For Each col As DataColumn In dt.Columns
                row.Add(col.ColumnName, dr(col).ToString())
            Next
            rows.Add(row)
        Next
        Return serializer.Serialize(rows)
    End Function
#End Region

#Region "Get DataTable Object By Sql Query"

    <WebMethod()> _
    Public Function GetDataTableObjectBySqlQuery(ByVal query As String) As String
        Dim ds As New System.Data.DataSet
        'Dim strTableName As String = query.ToUpper.Substring(query.ToUpper.LastIndexOf("FROM ")).Trim
        'strTableName = strTableName.Replace("FROM ", "").Trim
        'strTableName = strTableName.Substring(0, strTableName.IndexOf(" ", 0))
        ds = objHelp.GetResultSet(query, "TEMPTABLE")
        Dim strResult As String = GetJson(ds.Tables(0)) '"Akhilesh" '
        Return strResult
    End Function

#End Region

End Class
