Imports Microsoft.VisualBasic

Public Class ParameterReturnValue
    Private _ParaName As String
    Private _ParaValue As String
    Private _ParaIndex As Integer

    Public Sub New()
        _ParaName = ""
        _ParaValue = ""
        _ParaIndex = -1
    End Sub
    Public Property ParameterName() As String
        Get
            ParameterName = _ParaName
        End Get
        Set(ByVal value As String)
            _ParaName = value
        End Set
    End Property

    Public Property ParameterValue() As Object
        Get
            ParameterValue = _ParaValue
        End Get
        Set(ByVal value As Object)
            _ParaValue = value
        End Set

    End Property
    Public Property ParameterIndex() As Integer
        Get
            ParameterIndex = _ParaIndex
        End Get
        Set(ByVal value As Integer)
            _ParaIndex = value
        End Set
    End Property


End Class