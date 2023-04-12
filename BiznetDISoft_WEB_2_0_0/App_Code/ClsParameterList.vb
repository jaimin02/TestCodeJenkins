Imports Microsoft.VisualBasic

Public Class ClsParameterList
    Public Enum ParameterNoEnum
        DWREditDays = 1
        MTPLock = 2
    End Enum
    Public Enum ParameterDbTypeEnum
        DbType_varchar = 0
        DbType_Date = 1
        DbType_Integer = 2
        DbType_Boolean = 3
        DbType_Char = 4
        DbType_Double = 5
        DbType_TinyInteger = 6
    End Enum
    Private ParameterListArray() As ParameterInfo
    Public Sub New()

        Dim tLen_Array As Integer = -1
        Dim ParaInfo As ParameterInfo

        '--DCREditDays
        tLen_Array += 1
        ReDim Preserve ParameterListArray(tLen_Array)
        ParaInfo = New ParameterInfo(ParameterNoEnum.DWREditDays, _
                                            "DWR Edit Days", _
                                            "Days for DWR Edit If (0) Days then DCR is Editable for infinit days other wise it Editable Before given days.", _
                                            ParameterDbTypeEnum.DbType_Integer, "0", False)


        ParameterListArray(tLen_Array) = ParaInfo

        '--MTPLock
        tLen_Array += 1
        ReDim Preserve ParameterListArray(tLen_Array)
        ParaInfo = New ParameterInfo(ParameterNoEnum.MTPLock, _
                                            "MTP Lock Days", _
                                            "Days for MTP Lock.", _
                                            ParameterDbTypeEnum.DbType_Integer, "0", True)


        ParameterListArray(tLen_Array) = ParaInfo

    End Sub
    Public Function ParameterInfoByIndex(ByVal Index_1 As Integer) As ParameterInfo

        ParameterInfoByIndex = ParameterListArray(Index_1 - 1)

    End Function
    Public Function ParameterInfoByParameterNo(ByVal ParameterBool_1 As ClsParameterList.ParameterNoEnum) As ParameterInfo

        For Each PInfo As ParameterInfo In ParameterListArray

            If PInfo.ParameterNo = ParameterBool_1 Then
                Return PInfo
            End If

        Next PInfo

    End Function
    Public ReadOnly Property ParameterCount() As Integer
        Get
            Dim Len_1 As Integer

            Try
                Len_1 = ParameterListArray.Length
            Catch ex As Exception
                Len_1 = 0
            End Try

            Return Len_1

        End Get
    End Property
End Class
Public Class ParameterInfo
    Private _ParameterNo As ClsParameterList.ParameterNoEnum
    Private _ParameterName As String
    Private _ParameterDesc As String
    Private _ParameterDbType As ClsParameterList.ParameterDbTypeEnum
    Private _DefaultValue As String
    Private _IsCorporateLevelParameter As Boolean

    Friend Sub New(ByVal ParameterNo_1 As ClsParameterList.ParameterNoEnum, _
                   ByVal ParameterName_1 As String, _
                   ByVal ParameterDesc_1 As String, _
                   ByVal ParameterDbType_1 As ClsParameterList.ParameterDbTypeEnum, _
                   ByVal DefaultValue_1 As String, _
                   ByVal IsCorporateLevelParamter_1 As Boolean)

        _ParameterNo = ParameterNo_1
        _ParameterDbType = ParameterDbType_1
        _ParameterName = ParameterName_1
        _ParameterDesc = ParameterDesc_1
        _DefaultValue = DefaultValue_1
        _IsCorporateLevelParameter = IsCorporateLevelParamter_1


    End Sub

    Public ReadOnly Property ParameterNo() As ClsParameterList.ParameterNoEnum
        Get
            Return _ParameterNo
        End Get
    End Property
    Public ReadOnly Property ParameterDbType() As ClsParameterList.ParameterDbTypeEnum
        Get
            Return _ParameterDbType
        End Get
    End Property
    Public ReadOnly Property ParameterName() As String
        Get
            Return _ParameterName
        End Get
    End Property
    Public ReadOnly Property ParameterDesc() As String
        Get
            Return _ParameterDesc
        End Get
    End Property
    Public ReadOnly Property DefaultValue() As String
        Get
            Return _DefaultValue
        End Get
    End Property
    Public ReadOnly Property IsCorporateLevelParameter() As Boolean
        Get
            Return _IsCorporateLevelParameter
        End Get
    End Property
End Class