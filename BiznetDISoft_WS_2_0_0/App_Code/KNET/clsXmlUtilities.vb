Imports Microsoft.VisualBasic
Imports System.Xml
Imports System.Collections.Generic

Public Class clsXmlUtilities

    Friend Shared Function getNodeAttribute(ByVal node As XmlNode, ByVal attribute As String) As String
        getNodeAttribute = ""
        Dim retString As String = String.Empty

        Try
            retString = node.Attributes(attribute).InnerText

            Return retString
        Catch ex As Exception

        End Try
    End Function

    Public Shared Function getChildNodes(ByVal node As XmlNode) As List(Of XmlNode)
        getChildNodes = Nothing
        Dim nodeList As List(Of XmlNode) = Nothing
        Try
            For Each childNode As XmlNode In node
                nodeList.Add(childNode)
            Next childNode

            Return nodeList
        Catch ex As Exception

        End Try
    End Function

    Public Shared Function getChildNodes(ByVal node As XmlNode, ByVal child As String) As List(Of XmlNode)
        getChildNodes = Nothing
        Dim nodeList As List(Of XmlNode) = Nothing

        Try
            For Each childNode As XmlNode In node
                If childNode.Name.ToUpper = child.ToUpper Then
                    nodeList.Add(childNode)
                End If
            Next childNode

            Return nodeList
        Catch ex As Exception

        End Try
    End Function

    Public Shared Function getChildNode(ByVal node As XmlNode, ByVal child As String) As XmlNode
        getChildNode = Nothing

        Dim retNode As XmlNode = Nothing

        Try
            For Each childNode As XmlNode In node
                If childNode.Name.ToUpper = child.ToUpper Then
                    retNode = childNode
                    Exit For
                End If
            Next childNode

            Return retNode
        Catch ex As Exception

        End Try
    End Function

    Public Shared Function checkExists(ByVal node As XmlNode, ByVal sequence As String) As Boolean
        checkExists = False
        Dim newCurrNode As XmlNode = Nothing

        Dim exists As Boolean = True
        Dim seq() As String = Nothing

        Try
            seq = sequence.Split("/")


            For Each strSeq As String In seq
                newCurrNode = Nothing
                For Each childNode As XmlNode In node
                    If childNode.Name.ToUpper = strSeq.ToUpper Then
                        newCurrNode = childNode
                    End If
                Next
                If IsNothing(newCurrNode) Then
                    exists = False
                    Exit For
                End If
                node = newCurrNode
            Next strSeq

            Return exists
        Catch ex As Exception

        End Try

    End Function

    Public Shared Function createIfNotExists(ByVal document As XmlDocument, ByVal node As XmlNode, _
                                             ByVal sequence As String) As Boolean
        createIfNotExists = False
        Dim newCurrNode As XmlNode = Nothing
        Dim newElement As XmlElement = Nothing
        Dim seq() As String = Nothing

        Try
            If checkExists(node, sequence) Then
                Return True
            End If

            seq = sequence.Split("/")
            For Each strSeq As String In seq
                newCurrNode = Nothing

                For Each childNode As XmlNode In node
                    If childNode.Name.ToUpper = strSeq.ToUpper Then
                        newCurrNode = childNode
                    End If
                Next childNode

                If IsNothing(newCurrNode) Then
                    newElement = document.CreateElement(strSeq)
                    node.AppendChild(newElement)
                    newCurrNode = newElement
                End If
                node = newCurrNode

            Next strSeq

            Return checkExists(node, sequence)
        Catch ex As Exception

        End Try
    End Function

    Public Shared Function checkExists(ByVal node As XmlNode, ByVal sequence As List(Of clsNodeContents)) As XmlNode
        checkExists = Nothing
        Dim newCurrNode As XmlNode = Nothing

        Try
            For Each seq As clsNodeContents In sequence

                newCurrNode = Nothing
                For Each childNodes As XmlNode In node
                    If seq.equals(childNodes) Then
                        newCurrNode = childNodes
                        Exit For
                    End If
                Next childNodes

                If IsNothing(newCurrNode) Then
                    node = Nothing
                    Exit For
                End If
                node = newCurrNode
            Next seq

            Return node
        Catch ex As Exception

        End Try
    End Function

    Public Shared Function createIfNotExists(ByVal document As XmlDocument, ByVal node As XmlNode, ByVal sequence As List(Of clsNodeContents)) As XmlNode
        createIfNotExists = Nothing

        Dim newElement As XmlElement = Nothing
        Dim checkNode As XmlNode = Nothing
        Dim bkNode As XmlNode = Nothing
        Dim newCurrNode As XmlNode = Nothing

        Dim cnt As Integer
        Dim attr() As String

        Try
            checkNode = checkExists(node, sequence)
            If Not IsNothing(checkNode) Then
                Return checkNode
            End If

            bkNode = node

            For Each seq As clsNodeContents In sequence
                newCurrNode = Nothing
                For Each childNode As XmlNode In node
                    If seq.equals(childNode) Then
                        newCurrNode = childNode
                    End If
                Next childNode

                If IsNothing(newCurrNode) Then
                    newElement = document.CreateElement(seq.nodeName)
                    newElement.InnerText = IIf(Not IsNothing(seq.nodeContent), seq.nodeContent, "")
                    node.AppendChild(newElement)

                    cnt = 0
                    Do While (Not IsNothing(seq.attributes) AndAlso seq.attributes.Count > 0 AndAlso cnt < seq.attributes.Count)
                        attr = seq.attributes(cnt).Split("=")
                        newElement.SetAttribute(attr(0), attr(1))
                        cnt += 1
                    Loop
                    newCurrNode = newElement
                End If
                node = newCurrNode
            Next seq

            Return checkExists(bkNode, sequence)
        Catch ex As Exception

        End Try
    End Function

    Public Shared Function getPreviousSibling(ByVal Node As XmlNode) As XmlNode
        getPreviousSibling = Nothing
        Dim previousSibling As XmlNode = Nothing

        previousSibling = Node.PreviousSibling
        While True
            If IsNothing(previousSibling) Then Exit While

            If previousSibling.NodeType = XmlNodeType.Element Then Exit While

            previousSibling = previousSibling.PreviousSibling

        End While

        Return previousSibling
    End Function

    Public Shared Function getNextSibling(ByVal Node As XmlNode) As XmlNode
        getNextSibling = Nothing
        Dim nextSibling As XmlNode = Nothing

        nextSibling = Node.NextSibling
        While True
            If IsNothing(nextSibling) Then Exit While

            If nextSibling.NodeType = XmlNodeType.Element Then Exit While

            nextSibling = nextSibling.NextSibling
        End While

        Return nextSibling
    End Function

    Public Shared Function getFirstChild(ByVal Node As XmlNode) As XmlNode
        getFirstChild = Nothing
        Dim firstChild As XmlNode = Nothing

        firstChild = Node.FirstChild
        While True
            If IsNothing(firstChild) Then Exit While

            If firstChild.NodeType = XmlNodeType.Element Then Exit While

            firstChild = firstChild.NextSibling
        End While

        Return firstChild
    End Function

    Public Shared Sub main(ByVal args() As String)
        Dim document As XmlDocument
        Dim rootNode As XmlNode = Nothing

        Try
            document = New XmlDocument()
            document.Save("/home/Tianeptine-Lupin-454/0000/index.xml")

            rootNode = document.GetElementById("ectd:ectd")

            Dim fileOutputStream As New System.IO.FileStream("/home/tst.xls", IO.FileMode.Open)

            Dim SteamWriter As New IO.StreamWriter(fileOutputStream)

            Console.SetOut(SteamWriter)


            Console.WriteLine("Node ID\tTitle\tChecksum\tOperation\tPath\tPath Length\tFile Exists")
            check(rootNode, "/home/Tianeptine-Lupin-454/0000/")
            fileOutputStream.Close()

        Catch ex As Exception

        End Try
    End Sub

    Private Shared Sub check(ByVal node As XmlNode, ByVal path As String)
        Dim nodeName As String = String.Empty
        Dim hrefAttr As String = String.Empty
        Dim chksAttr As String = String.Empty
        Dim ndIdAttr As String = String.Empty
        Dim title As String = String.Empty
        Dim operation As String = String.Empty

        Dim titleNode As XmlNode = Nothing
        Dim file As System.IO.FileStream = Nothing


        If node.ChildNodes.Count = 0 Then Exit Sub

        For Each childNode As XmlNode In node
            nodeName = childNode.Name

            If nodeName.Equals("leaf") Then
                getNodeAttribute(childNode, "xlink:href")
                If Not hrefAttr.Contains("m3") Then
                    Continue For
                End If

                chksAttr = getNodeAttribute(childNode, "checksum")
                ndIdAttr = getNodeAttribute(childNode, "ID")
                titleNode = getFirstChild(childNode)
                title = titleNode.Value
                operation = getNodeAttribute(childNode, "operation")
                file = System.IO.File.Create(path + hrefAttr)

                Console.WriteLine(ndIdAttr + "\t" + title + "\t" + chksAttr + "\t" + operation + "\t" + hrefAttr + "\t" + hrefAttr.Length() + "\t" + System.IO.File.Exists(file.Name).ToString)
            Else
                check(childNode, path)
            End If

        Next childNode

    End Sub
End Class