Imports System.Collections.Generic
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Partial Class frmPIFImage
    Inherits System.Web.UI.Page

    Dim ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private SubjectId As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Page.Title = ":: PIFImage :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
        If Not IsNothing(Me.Request.QueryString("subjectid")) Then
            SubjectId = Me.Request.QueryString("subjectid").ToString()
            DisplayImage()
        End If
    End Sub

    Private Sub DisplayImage()
        Dim wstr As String = ""
        Dim ds_SubjectBlob As New DataSet
        Dim estr As String = ""
        Dim imgConverted As Bitmap
        Dim bytes As Byte()
        Try
            wstr = "cRecordType='P' and iTranNo=(select Max(itranNO) from SubjectBlobDetails where cRecordType='P' and vSubjectId='" & SubjectId.ToString() & "') and vSubjectId='" & SubjectId.ToString() & "'"

            If Not Me.objHelp.getSubjectBlobDetails(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubjectBlob, estr) Then
                MsgBox("Error while getting Data." + vbCrLf + estr)
                Exit Sub
            End If

            If ds_SubjectBlob.Tables(0).Rows.Count > 0 Then
                bytes = ds_SubjectBlob.Tables(0).Rows(0).Item("vtranValue")
                imgConverted = BytesToBmp_Serialized(bytes)

                Page.Response.ContentType = "image/jpeg"

                imgConverted.Save(Page.Response.OutputStream, ImageFormat.Gif)

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Private Function BytesToBmp_Serialized(ByVal bmpBytes As Byte()) As Bitmap
        Dim bf As New BinaryFormatter()
        ' copy the bytes to the memory
        Dim ms As New MemoryStream(bmpBytes)

        Try
            Return DirectCast(bf.Deserialize(ms), Bitmap)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Function

#Region "Error Handler"
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        ObjCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        ObjCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub
#End Region

End Class
