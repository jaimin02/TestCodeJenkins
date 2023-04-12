Imports Microsoft.VisualBasic
Imports System.Xml
Imports System.Collections.Generic

Public Class clsNodeContents

    Friend nodeName As String
    Friend nodeContent As String
    Friend attributes As List(Of String)
    Private nodeType As Short

    Public Sub New(ByVal nodeName As String, ByVal nodeContent As String, ByVal attributes As List(Of String), ByVal nodeType As Short)
        Me.nodeName = nodeName
        Me.nodeContent = nodeContent
        Me.attributes = attributes
        Me.nodeType = nodeType
    End Sub

    Public Sub New(ByVal nodeName As String, ByVal nodeContent As String, ByVal attributes As List(Of String))
        Me.New(nodeName, nodeContent, attributes, XmlNodeType.Element)
    End Sub

    Public Shadows Function equals(ByVal node As XmlNode) As Boolean
        equals = False
        Dim attr() As String = Nothing
        Dim cnt As Integer = 0

        If Not node.Name = nodeName Then
            Return False
        End If

        If Not node.NodeType = nodeType Then
            Return False
        End If

        Do While (Not IsNothing(attributes) AndAlso attributes.Count > 0 AndAlso cnt < attributes.Count())

            attr = attributes.Item(cnt).Split("=")
            If Not clsXmlUtilities.getNodeAttribute(node, attr(0)).Equals(attr(1)) Then
                Return False
            End If
            cnt += 1
        Loop

        Return True

    End Function
End Class