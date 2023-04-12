Imports System.Net.Mail
Partial Class frmSendMail
    Inherits System.Web.UI.Page

#Region "variable declaration"
    Dim objCommon As New clsCommon
#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Page.Title = ":: Send Mail :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
        If Not Page.IsPostBack Then
            FillEmailId()
        End If
        Me.HideMenu()

    End Sub

#End Region

#Region "FillEmailId"

    Private Sub FillEmailId()

        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
        CType(Me.Master.FindControl("lblHeading"), Label).Text = "Send Mail"

        TxtTo.Text = Session("UserEmail")
        If Request.QueryString("ActivityDone").ToString() = "Start" Then
            TxtSubject.Text = "Project Actvity Tracking Updates-" & Session("projectno") & "-" & Session("ActivityEndingName") & "- Started"
        ElseIf Request.QueryString("ActivityDone") = "End" Then
            TxtSubject.Text = "BizNET Notification- PN :" & Session("projectno") & "- AOI :" & Session("ActivityEndingName") & "- Ended"
        Else
            TxtSubject.Text = "BizNET Notification- PN :" & Session("projectno") & "- AOI :" & Session("ActivityEndingName") & ""
        End If


    End Sub

#End Region

#Region "Send Mail"

    Public Sub SendMail(ByVal mailbody As String)
        Dim fromEmailId As String = ""
        Dim toEmailId As String = ""
        Dim password As String = ""
        Dim ccEmailId As String = ""
        Dim smtp As New SmtpClient
        Dim mailmsg As New MailMessage
        Dim wStr As String = ""
        Dim StrTo() As String
        Dim StrCC() As String

        Try

            If TxtTo.Text = String.Empty Then

                Me.objCommon.ShowAlert("There is no any e-mail id exist, at least one e-mail id should be there.", Me)

            ElseIf TxtCc.Text = String.Empty And TxtTo.Text <> String.Empty Then

                fromEmailId = ConfigurationSettings.AppSettings("Username")
                password = ConfigurationSettings.AppSettings("Password")
                smtp = New SmtpClient
                smtp.Credentials = New Net.NetworkCredential(fromEmailId, password)
                smtp.EnableSsl = True
                smtp.Host = ConfigurationSettings.AppSettings("smtpServer")
                smtp.Port = ConfigurationSettings.AppSettings("ServerPort")
                mailmsg = New MailMessage
                mailmsg.IsBodyHtml = True
                mailmsg.From = New MailAddress(fromEmailId)

                StrTo = Me.TxtTo.Text.Trim.Split(",")
                For count As Integer = 0 To StrTo.Length - 1
                    mailmsg.To.Add(New MailAddress(StrTo(count).Trim()))
                Next

                mailmsg.Subject = TxtSubject.Text
                mailmsg.Body = mailbody & "<br/>" & TxtBody.Text
                smtp.Send(mailmsg)
                Me.objCommon.ShowAlert("Mail Sent.", Me)
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "", "closewindow()", True)

            Else

                fromEmailId = ConfigurationSettings.AppSettings("Username")
                password = ConfigurationSettings.AppSettings("Password")
                smtp = New SmtpClient
                smtp.Credentials = New Net.NetworkCredential(fromEmailId, password)
                smtp.EnableSsl = False
                smtp.Host = ConfigurationSettings.AppSettings("smtpServer")
                smtp.Port = ConfigurationSettings.AppSettings("ServerPort")
                mailmsg = New MailMessage
                mailmsg.IsBodyHtml = True
                mailmsg.From = New MailAddress(fromEmailId)

                StrTo = Me.TxtTo.Text.Trim.Split(",")
                For count As Integer = 0 To StrTo.Length - 1
                    mailmsg.To.Add(New MailAddress(StrTo(count).Trim()))
                Next

                mailmsg.Subject = TxtSubject.Text
                mailmsg.Body = mailbody & "<br/>" & TxtBody.Text

                StrCC = Me.TxtCc.Text.Trim.Split(",")
                For count As Integer = 0 To StrCC.Length - 1
                    mailmsg.CC.Add(New MailAddress(StrCC(count).Trim()))
                Next

                smtp.Send(mailmsg)
                Me.objCommon.ShowAlert("Mail Sent.", Me)
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "", "closewindow()", True)

            End If

        Catch ex As Exception
            Me.objCommon.ShowAlert("Error While Sending E-Mail. " + ex.Message, Me.Page)
        End Try

    End Sub

#End Region

#Region "HideMenu"

    Private Sub HideMenu()
        Dim tmpMenu As Menu
        Dim tmpddlprofile As DropDownList

        tmpMenu = CType(Me.Master.FindControl("menu1"), Menu)
        tmpddlprofile = CType(Me.Master.FindControl("ddlProfile"), DropDownList)

        If Not tmpMenu Is Nothing Then
            tmpMenu.Visible = False
        End If

        If Not tmpddlprofile Is Nothing Then
            tmpddlprofile.Enabled = False
        End If

    End Sub

#End Region

#Region "Button Events"

    Protected Sub BtnSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSend.Click
        SendMail(Session("Body"))
    End Sub

#End Region
    
End Class
