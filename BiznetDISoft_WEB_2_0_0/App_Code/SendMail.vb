Imports System.Web.Mail
Imports System.IO

Public Class SendMail
    Inherits MailMessage

    Public Sub New(ByRef Server As Object, ByRef Request As Object, ByRef Session As System.Web.SessionState.HttpSessionState, Optional ByVal MailSubject As String = "(No Subject)", Optional ByVal ErrorMessage As String = "", Optional ByVal fromEmailId As String = "", Optional ByVal password As String = "", Optional ByVal toEmailId As String = "", Optional ByVal ccEmailId As String = "")

        MyClass.From = fromEmailId 'ConfigurationSettings.AppSettings("mailFrom")
        MyClass.To = toEmailId 'ConfigurationSettings.AppSettings("mailTo")
        MyClass.Cc = ccEmailId 'ConfigurationSettings.AppSettings("CCTo")
        MyClass.Subject = MailSubject

        Dim bc As HttpBrowserCapabilities = Request.Browser
        Dim remoteAddress As String = Request.Servervariables("REMOTE_HOST")
        Dim strErrMsg As String

        If ErrorMessage = "" Then
            strErrMsg = Server.GetLastError.ToString()
        Else
            strErrMsg = ErrorMessage
        End If


        'MyClass.Body = "<font face=verdana color=red>" _
        '                       & "<b>URL:" & Request.Url.ToString() & "</b>" _
        '                       & "<pre><font color='red'><b>ERROR MSG:</b>" & strErrMsg & "</pre>" _
        '                       & "Browser: " & bc.Browser.ToString & bc.Version.ToString & "<br>" _
        '                       & "Javascript: " & bc.JavaScript.ToString & "<br>" _
        '                       & "VB Script: " & bc.VBScript.ToString & "<br>" _
        '                       & "Cookies: " & bc.Cookies.ToString & "<br>" _
        '                       & "Remote Address: " & remoteAddress & "<br>" _
        '                       & "</font>"

        MyClass.Body = "<pre><font face=verdana color='navy'><b>" & strErrMsg & "</b></font></pre>"

        MyClass.BodyFormat = Mail.MailFormat.Html

        SmtpMail.SmtpServer = ConfigurationSettings.AppSettings("smtpServer")

    End Sub

    Public Sub New(Optional ByVal Message As String = "Error Occured!!!", Optional ByVal fromEmailId As String = "", Optional ByVal password As String = "", Optional ByVal toEmailId As String = "", Optional ByVal ccEmailId As String = "")
        MyClass.From = fromEmailId 'ConfigurationSettings.AppSettings("mailFrom")
        MyClass.To = toEmailId 'ConfigurationSettings.AppSettings("mailTo")
        MyClass.Cc = ccEmailId 'ConfigurationSettings.AppSettings("CCTo")
        MyClass.Subject = "Site Error -- " & ConfigurationSettings.AppSettings("ClientName")
        MyClass.Body = Message
        MyClass.BodyFormat = Mail.MailFormat.Html
        SmtpMail.SmtpServer = ConfigurationSettings.AppSettings("smtpServer")
    End Sub

    'Changed on 26-Aug-2009: Make Sub to function
    Public Function Send(ByRef Server As Object, ByRef Response As Object, ByRef Session As System.Web.SessionState.HttpSessionState, Optional ByVal RedirectFlag As Boolean = True, Optional ByVal fromEmailId As String = "", Optional ByVal password As String = "", Optional ByVal toEmailId As String = "", Optional ByVal ccEmailId As String = "") As Boolean
        Try
            Dim SMTPPort As String
            SMTPPort = ConfigurationSettings.AppSettings("ServerPort").ToString.Trim()
            Server.ClearError()

            SmtpMail.SmtpServer = ConfigurationSettings.AppSettings("smtpServer") '"smtp.gmail.com" 'smtp is :smtp.gmail.com
            ' - smtp.gmail.com use smtp authentication

            MyClass.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1")
            MyClass.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", fromEmailId) 'Provide Sender EMail id here
            MyClass.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", password) 'Provide PAssword here
            ' - smtp.gmail.com use port 465 or 587
            MyClass.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", SMTPPort) 'port is: 465
            ' - smtp.gmail.com use STARTTLS (some call this SSL)

            ' For Local PC
            'MyClass.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true")

            ' For Server
            MyClass.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "false")

            SmtpMail.Send(Me)

            Send = True
        Catch ex As Exception
            Me.WriteError(Response)
            Send = False
        End Try

    End Function
    '***************************************************************************

    Public Sub WriteError(ByRef Response As Object)
        Response.write(MyClass.Body)
    End Sub

End Class