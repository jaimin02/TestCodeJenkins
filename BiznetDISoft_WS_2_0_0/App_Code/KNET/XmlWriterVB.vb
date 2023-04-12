Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.Text
Imports System.IO
Imports System.Xml


Namespace com.docmgmt.struts.resources

    Public Class XmlWriter

        Private writer As XmlWriter ' underlying writer
        Private stack As Stack ' of xml entity names
        Private attrs As StringBuilder ' current attribute string
        Private empty As Boolean ' is the current node empty
        Private closed As Boolean ' is the current node closed...

        ''' <summary>
        ''' Create an XmlWriter on top of an existing java.io.Writer.
        ''' </summary>
        Public Sub New(ByVal writer As XmlWriter)
            Me.writer = writer
            Me.closed = True
            Me.stack = New Stack()
        End Sub

        ''' <summary>
        ''' Begin to output an entity. 
        ''' </summary>
        ''' <param name="String"> name of entity. </param>
        Public Overridable Function writeEntity(ByVal name As String) As XmlWriter
            Try
                closeOpeningTag()
                Me.closed = False
                Me.writer.writeText("<")
                Me.writer.writeText(name)
                stack.Push(name)
                Me.empty = True
                Return Me
            Catch ioe As IO.IOException
                Throw New XmlException()
            End Try
        End Function

        ' close off the opening tag
        Private Sub closeOpeningTag()
            If Not Me.closed Then
                writeAttributes()
                Me.closed = True
                Me.writer.writeText(">")
            End If
        End Sub

        ' write out all current attributes
        Private Sub writeAttributes()
            If Me.attrs IsNot Nothing Then
                Me.writer.writeText(Me.attrs.ToString())
                Me.attrs.Length = 0
                Me.empty = False
            End If
        End Sub

        ''' <summary>
        ''' Write an attribute out for the current entity. 
        ''' Any xml characters in the value are escaped.
        ''' Currently it does not actually throw the exception, but 
        ''' the api is set that way for future changes.
        ''' </summary>
        ''' <param name="String"> name of attribute. </param>
        ''' <param name="String"> value of attribute. </param>
        Public Overridable Function writeAttribute(ByVal attr As String, ByVal value As String) As XmlWriter

            ' maintain api
            If False Then
                Throw New XmlException()
            End If

            If Me.attrs Is Nothing Then
                Me.attrs = New StringBuilder()
            End If
            Me.attrs.Append(" ")
            Me.attrs.Append(attr)
            Me.attrs.Append("=""")
            Me.attrs.Append(escapeXml(value))
            Me.attrs.Append("""")
            Return Me
        End Function

        ''' <summary>
        ''' End the current entity. This will throw an exception 
        ''' if it is called when there is not a currently open 
        ''' entity.
        ''' </summary>
        Public Overridable Function endEntity() As XmlWriter
            Try
                If Me.stack.Count = 0 Then
                    Throw New XmlException("Called endEntity too many times. ")
                End If
                Dim name As String = CStr(Me.stack.Pop())
                If name IsNot Nothing Then
                    If Me.empty Then
                        writeAttributes()
                        Me.writer.writeText("/>")
                    Else
                        Me.writer.writeText("</")
                        Me.writer.writeText(name)
                        Me.writer.writeText(">")
                        Me.writer.writeText(vbLf)
                    End If
                    Me.empty = False
                End If
                Return Me
            Catch ioe As IOException
                Throw New XmlException()
            End Try
        End Function

        ''' <summary>
        ''' Close this writer. It does not close the underlying 
        ''' writer, but does throw an exception if there are 
        ''' as yet unclosed tags.
        ''' </summary>
        Public Overridable Sub close()
            If (Not Me.stack.Count) = 0 Then
                Throw New XmlException("Tags are not all closed. " & "Possibly, " & Me.stack.Pop() & " is unclosed. ")
            End If
        End Sub

        ''' <summary>
        ''' Output body text. Any xml characters are escaped. 
        ''' </summary>
        Public Overridable Function writeText(ByVal text As String) As XmlWriter
            Try
                closeOpeningTag()
                Me.empty = False
                Me.writer.writeText(escapeXml(text))
                Return Me
            Catch ioe As IOException
                Throw New XmlException()
            End Try
        End Function

        ' Static functions lifted from generationjava helper classes
        ' to make the jar smaller.

        ' from XmlW
        Public Shared Function escapeXml(ByVal str As String) As String
            str = replaceString(str, "&", "&amp;")
            str = replaceString(str, "<", "&lt;")
            str = replaceString(str, ">", "&gt;")
            str = replaceString(str, """", "&quot;")
            str = replaceString(str, "'", "&apos;")
            Return str
        End Function

        ' from StringW
        Public Shared Function replaceString(ByVal text As String, ByVal repl As String, ByVal [with] As String) As String
            Return replaceString(text, repl, [with], -1)
        End Function
        ''' <summary>
        ''' Replace a string with another string inside a larger string, for
        ''' the first n values of the search string.
        ''' </summary>
        ''' <param name="text"> String to do search and replace in </param>
        ''' <param name="repl"> String to search for </param>
        ''' <param name="with"> String to replace with </param>
        ''' <param name="n">    int    values to replace
        ''' </param>
        ''' <returns> String with n values replacEd </returns>
        Public Shared Function replaceString(ByVal text As String, ByVal repl As String, ByVal [with] As String, ByVal max As Integer) As String
            Dim buffer As New StringBuilder(text.Length)
            Dim start As Integer = 0
            Dim [end] As Integer = 0
            Try
                If text Is Nothing Then
                    Return Nothing
                End If
                [end] = text.IndexOf(repl, start)
                Do While [end] <> -1
                    Buffer.Append(text.Substring(start, [end] - start)).Append([with])
                    start = [end] + repl.Length

                    max -= 1
                    If max = 0 Then
                        Exit Do
                    End If
                    [end] = text.IndexOf(repl, start)
                Loop
                Buffer.Append(text.Substring(start))
                Return Buffer.ToString()
            Catch ex As Exception
                Return ""
            End Try
        End Function

    End Class

End Namespace