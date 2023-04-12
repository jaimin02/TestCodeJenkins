Imports Microsoft.VisualBasic

Public Class WorkspaceNodeAttrHistory
    Public vWorkspaceId
    Public iNodeId

    Public Property WorkspaceId() As String
        Get
            WorkspaceId = vWorkspaceId

        End Get
        Set(ByVal value As String)
            vWorkspaceId = value
        End Set
    End Property

    Public Property NodeId() As String
        Get
            NodeId = iNodeId
        End Get
        Set(ByVal value As String)
            iNodeId = value
        End Set
    End Property
End Class

