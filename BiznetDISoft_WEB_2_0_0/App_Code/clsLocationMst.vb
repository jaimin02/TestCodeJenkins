Imports Microsoft.VisualBasic
Imports System.Collections
Imports system.Collections.Generic
Public Class clsLocationMst
    Inherits clsDataLogic

    Private pCode As String
    Private pName As String
    Private pRemark As String
    Private pInitial As String
    Private pModifyBy As Integer
    Private pModifyOn As DateTime
    Private pStatusIndication As String
    Private objCommon As clsCommon
    Private ObjHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()

    Private objConnection As SqlConnection
    Private objCommand As SqlCommand
    Private objDataAdapter As SqlDataAdapter

    Public Property Code() As String
        Get
            Return pCode
        End Get
        Set(ByVal value As String)
            If value Is DBNull.Value OrElse value Is Nothing Then
                pCode = ""
            Else
                pCode = value
            End If
        End Set
    End Property

    Public Property Name() As String
        Get
            Return pName
        End Get
        Set(ByVal value As String)
            If value Is DBNull.Value OrElse value Is Nothing Then
                pName = ""
            Else
                pName = value
            End If
        End Set
    End Property

    Public Property Remark() As String
        Get
            Return pRemark
        End Get
        Set(ByVal value As String)
            If value Is DBNull.Value OrElse value Is Nothing Then
                pRemark = ""
            Else
                pRemark = value
            End If
        End Set
    End Property

    Public Property Initial() As String
        Get
            Return pInitial
        End Get
        Set(ByVal value As String)
            If value Is DBNull.Value OrElse value Is Nothing Then
                pInitial = ""
            Else
                pInitial = value
            End If
        End Set
    End Property

    Public Property ModifyBy() As Integer
        Get
            Return pModifyBy
        End Get
        Set(ByVal value As Integer)

            If Not Integer.TryParse(value, pModifyBy) Then
                pModifyBy = 0
            End If

        End Set
    End Property
    Public Property StatusIndication() As String
        Get
            Return pStatusIndication
        End Get
        Set(ByVal value As String)
            If value Is DBNull.Value OrElse value Is Nothing Then
                pStatusIndication = ""
            Else
                pStatusIndication = value
            End If
        End Set
    End Property
    Public Property ModifyOn() As DateTime
        Get
            Return pModifyOn
        End Get
        Set(ByVal value As DateTime)
            If DateTime.TryParse(value, pModifyOn) Then
                pModifyOn = Nothing
            End If

        End Set
    End Property

    Public Function GetLocationMst(ByVal Wstr As String) As List(Of clsLocationMst)
        Dim ds As New DataSet
        Dim estr As String = ""
        Dim ObjLocation As clsLocationMst
        Dim LstLocation As New List(Of clsLocationMst)

        Me.ObjHelp.getLocationMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds, estr)

        For Each dr As DataRow In ds.Tables(0).Rows

            ObjLocation = New clsLocationMst
            ObjLocation.Code = Convert.ToString(dr("vLocationCode"))
            ObjLocation.Name = Convert.ToString(dr("vLocationName"))
            ObjLocation.Remark = Convert.ToString(dr("vRemark"))
            ObjLocation.Initial = Convert.ToString(dr("vLocationInitiate"))
            ObjLocation.ModifyBy = Convert.ToString(dr("iModifyBy"))
            ObjLocation.ModifyOn = Convert.ToString(dr("dModifyOn"))
            ObjLocation.StatusIndication = Convert.ToString(dr("cStatusIndi"))

            LstLocation.Add(ObjLocation)
        Next dr

        Return LstLocation

    End Function


    Public Function Insert_Location(ByVal ObjLocation As clsLocationMst) As String
        Dim ds As New DataSet
        Dim estr As String = ""
        Dim strProcedure As String = ""

        objDataAdapter = New SqlDataAdapter

        OpenConnection()
        strProcedure = "Insert_LocationMst"
        objCommand = New SqlCommand(strProcedure, objConnection)
        objCommand.CommandType = CommandType.StoredProcedure

        objCommand.Parameters.Add(ObjLocation.Code)
        objCommand.Parameters.Add(ObjLocation.Name)
        objCommand.Parameters.Add(ObjLocation.Remark)
        objCommand.Parameters.Add(ObjLocation.ModifyBy)
        objCommand.Parameters.Add(ObjLocation.StatusIndication)
        objCommand.Parameters.Add(1)

        objDataAdapter.SelectCommand = objCommand

        CloseConnection()

        Return ObjLocation.Name

    End Function

End Class
