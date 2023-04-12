
Imports Microsoft.VisualBasic
<System.Serializable()> _
Public Class ClsMedExDtls
    Private pTextValue As String
    Private pId As String

    Public Property GetValue() As String
        Get
            Return pTextValue
        End Get
        Set(ByVal value As String)
            pTextValue = value
        End Set
    End Property

    Public Property GetId() As String
        Get
            Return pId
        End Get
        Set(ByVal value As String)
            pId = value
        End Set
    End Property

End Class
