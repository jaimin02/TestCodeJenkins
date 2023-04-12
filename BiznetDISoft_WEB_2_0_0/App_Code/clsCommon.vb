Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Linq
Imports System.Net
Imports System.Net.Mail
Imports Microsoft.Graph

Public Class clsCommon
    Private Const cStrErrorPage As String = "ErrorPage"
    Private Const cStrException As String = "Excep"
    Private FirstDateLastMonth As String
    Private LastDateLastMonth As String
#Region " Show Alert "
    Public Sub ShowAlert(ByVal message As String, ByVal pg As System.Web.UI.Page)
        Dim sb As New StringBuilder
        'sb.Append("alert(""")
        'sb.Append(message)
        'sb.Append(""");")

        sb.Append("swal('',""")
        sb.Append(message)
        sb.Append(""");")
        ScriptManager.RegisterClientScriptBlock(pg, Me.GetType(), "showalert", sb.ToString(), True)
    End Sub
#End Region

#Region "GetCurDatetime"

    Public Function GetTimeZone(ByVal LocationCode As String, ByVal passdate As DateTime) As String
        Dim wStr As String = String.Empty
        Dim ds As New DataSet
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = GetHelpDbTableRef()

        Try
            wStr = "SELECT vTimeZoneOffset FROM LocationMst" + _
                  " INNER JOIN TimeZoneMst" + _
                  " ON(TimeZoneMst.vTimeZoneName = LocationMst.vTimeZoneName)" + _
                  " WHERE LocationMst.vLocationCode = '" + HttpContext.Current.Session(S_LocationCode) + "'" + _
                  " And '" + passdate.ToString("dd/MMM/yyyy hh:mm:ss tt") + "' between TimeZoneMst.dDaylightStart and TimeZoneMst.dDaylightEnd " + _
                  " And TimeZoneMst.cStatusIndi <> 'D' " '' Added By Dipen Shah tt for time format on 8-March-2015.



            ds = objHelp.GetResultSet(wStr, "TimeZoneMst")

            If Not ds Is Nothing Then
                If ds.Tables(0).Rows.Count > 0 Then
                    Return ds.Tables(0).Rows(0)("vTimeZoneOffset")
                    Exit Function
                End If
            End If
            Return ""
        Catch ex As Exception
            Return ex.ToString()
        End Try
    End Function

    Public Function GetCurDatetime(Optional ByVal TimeZoneName As String = "") As String
        Dim strTimeZone As String = String.Empty
        Dim str() As String

        Try
            strTimeZone = GetTimeZone(HttpContext.Current.Session(S_LocationCode), DateTime.Now())
            If strTimeZone <> "" Then
                str = strTimeZone.Split(":")
                Return DateTime.UtcNow().AddHours(New TimeSpan(str(0), str(1), 0).TotalHours)
            Else
                Return DateTime.UtcNow().AddHours(New TimeSpan(0, 0, 0).TotalHours)
            End If
        Catch ex As Exception
            Return ex.ToString()
        End Try

    End Function

    Public Function GetCurDatetimeWithOffSet(Optional ByVal TimeZoneName As String = "") As DateTimeOffset

        Dim strTimeZone As String = String.Empty
        Dim str() As String

        Try
            If Convert.ToString(TimeZoneName).Trim() = "India Standard Time" Then
                strTimeZone = "+05:30"
            Else
                strTimeZone = GetTimeZone(HttpContext.Current.Session(S_LocationCode), DateTime.Now())
            End If

            If strTimeZone <> "" Then
                str = strTimeZone.Split(":")
                Dim dat As DateTime = DateTime.UtcNow().AddHours(New TimeSpan(str(0), str(1), 0).TotalHours)
                Dim dt As DateTime = New Date(dat.Year(), dat.Month(), dat.Day(), dat.Hour(), dat.Minute(), 0, 0, DateTimeKind.Unspecified)
                Dim offset As New DateTimeOffset(dt, New TimeSpan(str(0), str(1), 0))
                Return offset
            End If
        Catch ex As Exception
            Return ex.ToString()
        End Try

    End Function

    Public Function GetDayLightSavingTime(ByVal TempDate As DateTime, ByVal offset As String) As DateTime     'Added By Rahul Shah on 19-March-2015 for Getting Day-Light Time in IST

        Dim timezone As TimeZoneInfo
        Dim UtcDate As DateTime
        Dim offsetIndia As String
        Dim dOffsetTotal As Decimal
        Dim str() As String
        Try

            timezone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")
            UtcDate = TimeZoneInfo.ConvertTimeFromUtc(TempDate, timezone)
            offsetIndia = timezone.ToString.Substring(4, 6)
            offset = offset.Replace(":", ".")
            offsetIndia = offsetIndia.Replace(":", ".")

            dOffsetTotal = Convert.ToDecimal(offsetIndia) - Convert.ToDecimal(offset)

            str = dOffsetTotal.ToString.Split(".")

            Return TempDate.AddHours(New TimeSpan(str(0), str(1), 0).TotalHours)

        Catch ex As Exception
            Return ex.ToString()
        End Try

    End Function


    Public Function GetDTOffsetTOdatetime(ByVal dtoffset As Object) As DateTime
        Return New Date(dtoffset.Year(), dtoffset.Month(), dtoffset.Day(), dtoffset.Hour(), dtoffset.Minute(), dtoffset.Second(), dtoffset.Millisecond(), DateTimeKind.Unspecified)
    End Function

#End Region

#Region " SHOW ALERT & REDIRECT "
    Public Sub ShowAlertAndRedirect(ByVal Message As String, ByVal URL As String, ByVal pg As System.Web.UI.Page)
        Dim sb As New StringBuilder
        Message = Message.Replace("""", "\""")
        Message = Message.Replace("'", "\'")
        Message = Message.Replace(vbCr, "\n")
        Message = Message.Replace(vbLf, "\n")
        sb.Append("alert(""")
        sb.Append(Message)
        sb.Append(""");")
        sb.Append("window.location.href=")
        sb.Append(URL)
        sb.Append(";")
        ScriptManager.RegisterClientScriptBlock(pg, Me.GetType(), "ShowAlertAndRedirect", sb.ToString(), True)
    End Sub
#End Region

#Region "Send Email"

    Public Function SendEmail(ByVal ds As DataSet, ByVal Status As String, ByVal vWorkSpaceID As String, ByVal vParentWorkSpaceId As String, ByVal vOperationcode As String, ByVal StudyProtocol As String,
                               ByVal Site_No As String, ByVal EmailSubject As String, ByVal Remarks As String,
                               ByVal UploadedBy As String, ByVal UploadedDate As String, ByRef errormsg As String,
                               ByVal NodeDisplayName As String, ByVal UserId As String, ByVal dsSpineDetail As DataSet, ByVal dsProximalDetail As DataSet) As Boolean


        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = GetHelpDbTableRef()
        Dim objMailMessage As System.Net.Mail.MailMessage
        Dim objSmtpClient As System.Net.Mail.SmtpClient = getSmtpClient()
        Dim dsEmail As New DataSet
        Dim strCertificate As New StringBuilder
        Dim ToEmail As String = String.Empty
        Dim strFromMail As String = String.Empty
        Dim wStr As String = ""
        Dim DatePerformed = "", ToUser As String = ""
        Dim iCol As Integer = 0
        Dim dsExMsgInfo As New DataSet
        Dim objCommon As New clsCommon
        Dim drExMsgInfo As DataRow
        Dim ObjLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
        Dim eStr_Retu As String = String.Empty
        Dim ErrorLog As String = ""
        Dim Username As String = String.Empty
        Dim Password As String = String.Empty
        Dim TenantInfo As New SS.Mail.TenantInfo()
        Dim ds_EmailInfo As DataSet

        Try
            wStr = "vSMSLocationCode='" + HttpContext.Current.Session(S_LocationCode) + "' And cStatusIndi <> 'D'"

            If Not objHelp.GetSMSGateWayDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                        ds_EmailInfo, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            Username = Convert.ToString(ds_EmailInfo.Tables(0).Rows(0)("vFromEmail"))
            Password = Convert.ToString(ds_EmailInfo.Tables(0).Rows(0)("vPassword"))
            TenantInfo.TenantId = Convert.ToString(ds_EmailInfo.Tables(0).Rows(0)("vTenantId"))
            TenantInfo.ClientId = Convert.ToString(ds_EmailInfo.Tables(0).Rows(0)("vClientId"))
            TenantInfo.EmailUser = Username
            TenantInfo.EmailPassword = Password
            TenantInfo.Client_secret = Convert.ToString(ds_EmailInfo.Tables(0).Rows(0)("vSecretKey"))
            Dim AuthenticationWithSecretKey = Convert.ToBoolean(ds_EmailInfo.Tables(0).Rows(0)("bAuthSecretKey"))

            ErrorLog = "DataSetCount=" + ds.Tables(0).Rows.Count.ToString() + vbCrLf
            ErrorLog += "Status=" + Status + vbCrLf
            ErrorLog += "WorkSpaceId=" + vWorkSpaceID + vbCrLf
            ErrorLog += "Operationcode=" + vOperationcode + vbCrLf
            ErrorLog += "StudyProtocol=" + StudyProtocol + vbCrLf
            ErrorLog += "Site_No=" + Site_No + vbCrLf
            ErrorLog += "EmailSubject=" + EmailSubject + vbCrLf
            ErrorLog += " UploadedBy=" + UploadedBy + vbCrLf
            ErrorLog += "UploadedDate=" + UploadedDate + vbCrLf
            ErrorLog += "errormsg=" + errormsg + vbCrLf
            ErrorLog += "NodeDisplayName=" + NodeDisplayName + vbCrLf
            Dim myHtmlFile As String = String.Empty
            Dim EmailBody As String = String.Empty
            Dim myBuilder As System.Text.StringBuilder = New System.Text.StringBuilder()

            myBuilder.Append("<!DOCTYPE html>")
            myBuilder.Append("<html><head>")
            myBuilder.Append("<title>")
            myBuilder.Append("Page-")
            myBuilder.Append("</title>")
            myBuilder.Append("<style>")
            myBuilder.Append("body{ font-family:'Times New Roman','Times Roman','Verdana';}")
            myBuilder.Append("table{border-collapse: collapse;border-width: 1px;border-color:rgb(0,0,0);}")
            myBuilder.Append("thead{}")
            myBuilder.Append("th{padding:8px 30px 8px 8px;border-bottom-color:rgb(0,0,0);border-width: 0px 1px 2px 1px;}")
            myBuilder.Append("td{padding:5px 30px 5px 5px;}")
            myBuilder.Append("</style>")
            myBuilder.Append("</head>")
            myBuilder.Append("<body>")

            'Header
            EmailBody = ""
            EmailBody += "<table align='left' cellspacing='2' border='0' width='100%' style='table-layout: fixed;'>"
            EmailBody += "<tr>"
            EmailBody += "<td> Dear All,</td>"
            EmailBody += "</tr>"
            EmailBody += "<tr>"
            EmailBody += "<td> The image(s) has been reviewed and following is the status:</td>"
            EmailBody += "</tr>"

            'For iCol As Integer = 0 To ds.Tables(0).Rows.Count - 1
            DatePerformed = Convert.ToDateTime(ds.Tables(0).Rows(iCol)("PerformedDate")).ToString("dd-MMM-yyyy")
            If vOperationcode = "2" Then
                If ds.Tables(0).Rows(iCol)("cQCStatus").ToString <> "D" Then
                    Dim stst As String = IIf(ds.Tables(0).Rows(iCol)("cQCStatus").ToString().ToLower() = "a", "Approved", "Rejected")
                    strCertificate.Append("<tr>")
                    strCertificate.Append("<td>Status:<b> " + stst + "</b></td>")
                    strCertificate.Append("</tr>")
                    strCertificate.Append("<tr>")
                    strCertificate.Append("<td> Study Protocol: " + StudyProtocol.ToString + " </td>")
                    strCertificate.Append("</tr>")
                    strCertificate.Append("<tr>")
                    strCertificate.Append("<td> Visit: " + NodeDisplayName + " </td>")
                    strCertificate.Append("</tr>")
                End If
            ElseIf vOperationcode = "3" Then
                If ds.Tables(0).Rows(iCol)("cQC2Status").ToString() <> "D" Then
                    Dim stst As String = IIf(ds.Tables(0).Rows(iCol)("cQC2Status").ToString().ToLower() = "a", "Approved", "Rejected")
                    strCertificate.Append("<tr>")
                    strCertificate.Append("<td>Status: " + stst + " </td>")
                    strCertificate.Append("</tr>")
                    strCertificate.Append("<tr>")
                    strCertificate.Append("<td> Study Protocol: " + StudyProtocol.ToString + " </td>")
                    strCertificate.Append("</tr>")
                    strCertificate.Append("<tr>")
                    strCertificate.Append("<td> Visit: " + NodeDisplayName + " </td>")
                    strCertificate.Append("</tr>")
                End If
            ElseIf vOperationcode = "4" Then
                If ds.Tables(0).Rows(iCol)("cCA1Status").ToString() <> "D" Then
                    Dim stst As String = IIf(ds.Tables(0).Rows(iCol)("cCA1Status").ToString().ToLower() = "a", "Approved", "Rejected")
                    strCertificate.Append("<tr>")
                    strCertificate.Append("<td>Status: " + stst + " </td>")
                    strCertificate.Append("</tr>")
                    strCertificate.Append("<tr>")
                    strCertificate.Append("<td> Study Protocol: " + StudyProtocol.ToString + " </td>")
                    strCertificate.Append("</tr>")
                    strCertificate.Append("<tr>")
                    strCertificate.Append("<td> Visit: " + NodeDisplayName + " </td>")
                    strCertificate.Append("</tr>")
                End If
            End If
            'Next
            EmailBody += strCertificate.ToString
            EmailBody += "<tr>"
            EmailBody += "<td> Site Number: " + Site_No.ToString() + " </td>"
            EmailBody += "</tr>"
            EmailBody += "<tr>"
            If Not String.IsNullOrEmpty(DatePerformed) Then
                EmailBody += "<td> Examination Date: " + DatePerformed + "</td>"
            Else
                EmailBody += "<td> Examination Dated: " + String.Empty + " </td>"
            End If
            EmailBody += "</tr>"
            EmailBody += "<tr>"
            EmailBody += "<td> Remarks: " + Remarks.ToString() + "</ td>"
            EmailBody += "</tr>"
            EmailBody += "<tr>"
            EmailBody += "<td> Reviewed Date: " + DateTime.Now.ToString("dd-MMM-yyyy") + "</ td>"
            EmailBody += "</tr>"
            'If vOperationcode = "3" OrElse Not String.IsNullOrEmpty(SetNo) Then ' if grader review with final eligibility then not allow to add review by
            'EmailBody += "<tr>" 'Commented by Bhargav Thaker 16March2023
            'EmailBody += "<td> Reviewed By: " + UploadedBy.ToString() + "</ td>" 'Commented by Bhargav Thaker 16March2023
            'EmailBody += "</tr>" 'Commented by Bhargav Thaker 16March2023
            'End If
            EmailBody += "</table>"
            EmailBody += "	<br><br><br>"
            myBuilder.Append(EmailBody)
            EmailBody = String.Empty
            If dsSpineDetail IsNot Nothing AndAlso dsSpineDetail.Tables.Count > 0 AndAlso dsSpineDetail.Tables(0).Rows.Count > 0 Then
                EmailBody += "<table style='border:1px solid black;border-collapse:collapse;'>"
                EmailBody += "<tr><th style='border:1px solid;'>Anatomical</th><th style='border:1px solid;'>BMD (g/cm2)</th><th style='border:1px solid;'>BMC (g)</th><th style='border:1px solid;'>Area</th><th style='border:1px solid;'>Young-Adult (T-score)</th><th style='border:1px solid;'>Age-Matched (Z-score)</th><th style='border:1px solid;'>Measurable</th></tr>"
                For Each dr As DataRow In dsSpineDetail.Tables(0).Rows
                    EmailBody += "<tr><td style='border:1px solid;'>" + dr("Anatomical").ToString + " </td><td style='border:1px solid;'>" + dr("BMD").ToString + " </td><td style='border:1px solid;'>" + dr("BMC").ToString + " </td><td style='border:1px solid;'>" + dr("Area").ToString + " </td><td style='border:1px solid;'>" + dr("YoungAdult").ToString + " </td><td style='border:1px solid;'>" + dr("AgeMatched").ToString + " </td><td style='border:1px solid;'>" + dr("Measurable").ToString + " </td></tr>"
                Next
                EmailBody += "	</table>"
            End If
            EmailBody += "	<br>"
            If dsProximalDetail IsNot Nothing AndAlso dsProximalDetail.Tables.Count > 0 AndAlso dsProximalDetail.Tables(0).Rows.Count > 0 Then
                EmailBody += "<table style='border:1px solid black;border-collapse:collapse;'>"
                EmailBody += "<tr><th style='border:1px solid;'>Region</th><th style='border:1px solid;'>BMD (g/cm2)</th><th style='border:1px solid;'>BMC (g)</th><th style='border:1px solid;'>Area</th><th style='border:1px solid;'>Young-Adult (T-score)</th><th style='border:1px solid;'>Age-Matched (Z-score)</th></tr>"
                For Each dr As DataRow In dsProximalDetail.Tables(0).Rows
                    EmailBody += "<tr><td style='border:1px solid;'>" + dr("Region").ToString + " </td><td style='border:1px solid;'>" + dr("BMD").ToString + " </td><td style='border:1px solid;'>" + dr("BMC").ToString + " </td><td style='border:1px solid;'>" + dr("Area").ToString + " </td><td style='border:1px solid;'>" + dr("YA").ToString + " </td><td style='border:1px solid;'>" + dr("AM").ToString + " </td></tr>"
                Next
                EmailBody += "</table>"
            End If
            myBuilder.Append(EmailBody)
            EmailBody = String.Empty

            EmailBody += "	<br>"
            EmailBody += "	<table align='left' cellspacing='2' border='0' width='100%' style='table-layout: fixed;'>"
            EmailBody += "		<tr>"
            EmailBody += "				<td>This is a system generated email. Please do not reply directly to this mail. In case you have any questions please contact DI-Soft-C team and we will get back to you as soon as possible.</td>"
            EmailBody += "		</tr>		"
            EmailBody += "		<tr>"
            EmailBody += "				<td>&nbsp;</td>"
            EmailBody += "		</tr>		"
            EmailBody += "		<tr>"
            EmailBody += "				<td>Thanks,</td>"
            EmailBody += "		</tr>		"
            EmailBody += "		<tr>"
            EmailBody += "				<td>DI-Soft-C Team</ td>"
            EmailBody += "		</tr>"
            EmailBody += "	</table>"

            ErrorLog += "************************************Email Body add here**************************"
            ErrorLog += EmailBody.ToString() + vbCrLf
            myBuilder.Append(EmailBody)

            myBuilder.Append("</body>")
            myBuilder.Append("</html>")
            myHtmlFile = myBuilder.ToString()
            wStr = vWorkSpaceID.ToString() + "##" + vOperationcode.ToString() '"Proc_MedExInfoHdrDtlEdit '" + vWorkSpaceID + "','" + vOperationcode + "'"
            dsEmail = objHelp.ProcedureExecute("dbo.GetEmailForTranstrion", wStr)
            ErrorLog += "ToUser Email Count=" + dsEmail.Tables(0).Rows.Count.ToString() + vbCrLf

            If dsEmail.Tables(0).Rows.Count > 0 Then
                For Each dr As DataRow In dsEmail.Tables(0).Rows
                    ToUser += dr("vEmailId").ToString + "," 'Convert.ToString(dr["vEmailId"]) + ","
                Next
                ToUser = ToUser.Substring(0, ToUser.LastIndexOf(","))
                ErrorLog += "After For loop ToUser=" + ToUser.ToString() + vbCrLf
            End If
            If (Status.ToUpper() = "REJECTED" AndAlso ds.Tables(0).Rows.Count > 0 AndAlso ds.Tables(0).Rows(0)("CurrentUserChangeMailId") <> "") Then
                ErrorLog += "After Reject ToUser=" + ToUser.ToString() + vbCrLf
                ToUser += IIf(ToUser = "", "", ",") + ds.Tables(0).Rows(0)("CurrentUserChangeMailId")
            End If

            If ToUser = "" Then
                ToUser = System.Configuration.ConfigurationSettings.AppSettings("ToUserCer").ToString()
            End If
            If ToUser <> "" Then
                strFromMail = System.Configuration.ConfigurationSettings.AppSettings("Username").ToString() 'Me.Application(GeneralModule.S_CompanyEmail).ToString
                ErrorLog += "************************************To user not null**************************" + vbCrLf
                ErrorLog += "strFromMail=" + strFromMail.ToString() + vbCrLf
                If Not objHelp.GetExMsgInfoDetails("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty,
                                               dsExMsgInfo, eStr_Retu) Then
                    Throw New Exception(eStr_Retu + vbCrLf + "Error while retrieving EXE Msg Info Details.")
                End If

                drExMsgInfo = dsExMsgInfo.Tables(0).NewRow
                drExMsgInfo("vNotificationType") = "Mail"
                drExMsgInfo("vSubject") = EmailSubject.ToString()
                drExMsgInfo("vBody") = myHtmlFile
                drExMsgInfo("vFromEmailId") = strFromMail
                drExMsgInfo("vToEmailId") = String.Empty 'Modify by Bhargav Thaker 24Feb2023
                drExMsgInfo("vBCCEmailId") = ToUser 'Added by Bhargav Thaker 24Feb2023
                drExMsgInfo("iCreatedBy") = UserId
                drExMsgInfo("dCreatedDate") = Date.Now

                'objMailMessage = New System.Net.Mail.MailMessage()
                'objMailMessage.From = New Net.Mail.MailAddress(strFromMail) 'change EmailId while Upload for Lambda

                'objMailMessage.IsBodyHtml = True
                ErrorLog += "ToUser=" + ToUser.ToString() + vbCrLf
                'objMailMessage.To.Add(ToUser)
                ErrorLog += "Subject=" + EmailSubject.ToString() + vbCrLf
                'objMailMessage.Subject = EmailSubject.ToString()
                ErrorLog += "Body=" + myHtmlFile.ToString() + vbCrLf
                'objMailMessage.Body = myHtmlFile.ToString
                'ErrorLog += "Before Send=" + objMailMessage.ToString() + vbCrLf
                Dim emailIds = ToUser.Trim().Split(",")
                Dim ItemsList As New List(Of Recipient)()
                For Each drToEmail In emailIds
                    ItemsList.Add(New Recipient With {.EmailAddress = New EmailAddress With {.Address = Convert.ToString(drToEmail.Trim())}})
                Next
                Dim message = New Message With {
                     .Subject = EmailSubject,
                     .Body = New ItemBody With {.ContentType = BodyType.Html, .Content = myHtmlFile.ToString},
                     .ToRecipients = New List(Of Recipient)(),
                     .BccRecipients = ItemsList
               }
                '.BccRecipients Added by Bhargav Thaker 24Feb2023

                Try
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = Function(se As Object,
                                cert As System.Security.Cryptography.X509Certificates.X509Certificate,
                                chain As System.Security.Cryptography.X509Certificates.X509Chain,
                                sslerror As System.Net.Security.SslPolicyErrors) True
                    System.Net.ServicePointManager.MaxServicePointIdleTime = 1000
                    System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType.Ssl3 Or (SecurityProtocolType.Tls12 Or (SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls)))
                    'objSmtpClient.Send(objMailMessage)
                    'ErrorLog += "After Send=" + objMailMessage.ToString() + vbCrLf
                    If AuthenticationWithSecretKey = False Then
                        SS.Mail.EmailSender.SendMailUsingPassword(TenantInfo, message, True)
                    Else
                        SS.Mail.EmailSender.sendMailBySecret(TenantInfo, message, True)
                    End If
                    drExMsgInfo("cIsSent") = "Y"
                    drExMsgInfo("dSentDate") = Date.Now
                Catch ex As Exception
                    drExMsgInfo("cIsSent") = "N"
                    drExMsgInfo("vRemarks") = ex.Message
                End Try
                dsExMsgInfo.Tables(0).Rows.Add(drExMsgInfo)
                dsExMsgInfo.Tables(0).AcceptChanges()
                If Not ObjLambda.SaveExMsgInfo(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add,
                                                dsExMsgInfo, UserId, eStr_Retu) Then
                    Throw New Exception(eStr_Retu + vbCrLf + "Error while Saving in Mail Sending Details.")
                    Exit Function
                End If
            End If
            'End Get Email Address Of HrDept for email functionality (HardCoded by HRSADMIN ROLE)

            SendEmail = True 'Added by pravin on 09-jan-2020

        Catch ex As Exception
            Throw New Exception(ex.Message.ToString() + vbCrLf + "Error while sending Mail.")
        End Try
    End Function

    Public Function getSmtpClient()
        Dim MailMessage As New MailMessage()

        'Dim SMTPHost As String = HttpContext.Current.Session(S_MailSMTPIP).ToString() 'System.Configuration.ConfigurationSettings.AppSettings("smtpServer").ToString()
        'Dim SMTPUsername As String = HttpContext.Current.Session(S_MailUserName).ToString() 'System.Configuration.ConfigurationSettings.AppSettings("Username").ToString()
        'Dim SMTPPassword As String = HttpContext.Current.Session(S_MailPassword).ToString() 'System.Configuration.ConfigurationSettings.AppSettings("EmailPassword").ToString()
        'Dim SMTPPort As Integer = HttpContext.Current.Session(S_MailSMTPPortNo).ToString() 'Convert.ToInt16(System.Configuration.ConfigurationSettings.AppSettings("ServerPort"))
        'Dim Sslvalue As String = HttpContext.Current.Session(S_MailSSLValue).ToString() 'System.Configuration.ConfigurationSettings.AppSettings("SslValue").ToString()
        Dim SMTPHost As String = System.Configuration.ConfigurationSettings.AppSettings("smtpServer").ToString()
        Dim SMTPUsername As String = System.Configuration.ConfigurationSettings.AppSettings("Username").ToString()
        Dim SMTPPassword As String = System.Configuration.ConfigurationSettings.AppSettings("Password").ToString()
        Dim SMTPPort As Integer = Convert.ToInt16(System.Configuration.ConfigurationSettings.AppSettings("ServerPort"))
        Dim Sslvalue As String = System.Configuration.ConfigurationSettings.AppSettings("SslValue").ToString()


        getSmtpClient = New Net.Mail.SmtpClient(SMTPHost.ToString)
        getSmtpClient.Credentials = New Net.NetworkCredential(SMTPUsername.ToString, SMTPPassword.ToString)
        getSmtpClient.EnableSsl = Sslvalue
        getSmtpClient.Timeout = 2147483647
        getSmtpClient.Port = SMTPPort.ToString

    End Function

#End Region

#Region " SHOW ALERT & REDIRECT AFTER 6 Second"
    Public Sub ShowAlertAndRedirectAfter6Second(ByVal Message As String, ByVal URL As String, ByVal pg As System.Web.UI.Page)
        Dim sb As New StringBuilder
        Message = Message.Replace("""", "\""")
        Message = Message.Replace("'", "\'")
        Message = Message.Replace(vbCr, "\n")
        Message = Message.Replace(vbLf, "\n")
        sb.Append("setTimeout(function(){")
        sb.Append("alert(""")
        sb.Append(Message)
        sb.Append(""");")


        sb.Append("window.location.href=")
        sb.Append(URL)
        sb.Append("}, 700);")
        sb.Append(";")
        ScriptManager.RegisterClientScriptBlock(pg, Me.GetType(), "ShowAlertAndRedirect", sb.ToString(), True)
    End Sub
#End Region

#Region "Conformation"
    Public Function ShowConformation(ByVal message As String, ByVal pg As System.Web.UI.Page) As Boolean
        Dim sb As New StringBuilder
        sb.Append("confirm(""")
        sb.Append(message)
        sb.Append(""");")
        ScriptManager.RegisterClientScriptBlock(pg, Me.GetType(), "Confirm", sb.ToString(), True)
        Return True
    End Function
#End Region
#Region "Menu Data"
    Public Function GetMenuData(ByVal RoleNo As Integer) As DataSet
        Dim SM As String
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = GetHelpDbTableRef()
        Try

            SM = "SELECT * FROM View_Menu" + _
                 " where nRoleNo = " & RoleNo & " " + _
                 " order by ParentID,iSeqNo "

            Return objHelp.GetResultSet(SM, "Menu")

        Catch ex As Exception
            Return Nothing
        Finally
            SM = ""
        End Try
    End Function
#End Region
#Region "Write Error"

    Public Sub WriteError(ByVal server As Object, ByVal request As Object, _
                          ByRef Session As System.Web.SessionState.HttpSessionState, _
                          ByVal extr As Exception, _
                          Optional ByVal ErrorMessage As String = "")
        Dim Contents As String
        Dim objWriter As StreamWriter
        Dim remoteAddress As String = request.Servervariables("REMOTE_HOST")
        Dim bc As HttpBrowserCapabilities = request.Browser
        Dim SplitArray() As String
        Dim strPath As String

        If IsNothing(Session("Path")) Then
            strPath = server.MapPath("~")
            strPath = strPath + "\ErrorHandler\Error_" + Format(Date.Now, "yyyy-MM-dd").ToString() + ".Log"
            Session("Path") = strPath
        End If

        If IO.File.Exists(Session("Path")) Then
            objWriter = New StreamWriter(Session("Path"), True)
        Else
            objWriter = New StreamWriter(Session("Path"), False)
        End If

        Try
            SplitArray = extr.StackTrace.Split(":")

            If ErrorMessage.ToString.Trim() <> "" Then

                Contents = "Err.Date: " & DateTime.Now.ToString & ControlChars.NewLine
                Contents += ErrorMessage & vbCrLf
                Contents += "UserNo :" & Session("UserNo") & vbCrLf
                Contents += "UserName :" & Session("UserName") & vbCrLf
                Contents += "User Division :" & Session("DivisionNo") & vbCrLf
                Contents += "Error Originated From:  " & extr.TargetSite().Name & Environment.NewLine
                Contents += extr.StackTrace.ToString & Environment.NewLine
                Contents += "Line Number:  " & SplitArray(2) & Environment.NewLine
                Contents += "MemberType:  " & extr.TargetSite.MemberType.ToString() & Environment.NewLine
                Contents += "Error Page:  " & SplitArray(1) & Environment.NewLine
                Contents += "Exception Type:  " & extr.GetType.Name & Environment.NewLine
                Contents += "URL:" & request.Url.ToString() & vbCrLf
                Contents += "Javascript: " & bc.EcmaScriptVersion.ToString & vbCrLf
                Contents += "VB Script: " & bc.VBScript.ToString & vbCrLf
                Contents += "Cookies: " & bc.Cookies.ToString & vbCrLf
                Contents += "Remote Address: " & remoteAddress & vbCrLf
                Contents += "=========================================================" & vbCrLf
                objWriter.WriteLine(Contents)
                objWriter.Close()

            End If

        Catch ex As Exception

        Finally
            objWriter.Close()
        End Try

    End Sub

    Public Sub WriteError(ByVal server As Object, ByVal request As Object, _
                          ByRef Session As System.Web.SessionState.HttpSessionState, _
                          Optional ByVal ErrorMessage As String = "")
        Dim Contents As String
        Dim objWriter As StreamWriter
        Dim remoteAddress As String = request.Servervariables("REMOTE_HOST")
        Dim bc As HttpBrowserCapabilities = request.Browser
        Dim strPath As String

        'Session("Path") = "C:\Error.log"
        If IsNothing(Session("Path")) Then
            strPath = server.MapPath("~")
            strPath = strPath + "\ErrorHandler\Error_" + Format(Date.Now, "yyyy-MM-dd").ToString() + ".Log"
            'If strPath.Contains("CDMS") Then
            '    strPath = strPath.Replace("\CDMS\", "\")
            'End If
            'If strPath.Contains("BA") Then
            '    strPath = strPath.Replace("\BA\", "\")
            'End If
            Session("Path") = strPath
        End If

        If IO.File.Exists(Session("Path")) Then
            objWriter = New StreamWriter(Session("Path"), True)
        Else
            objWriter = New StreamWriter(Session("Path"), False)
        End If
        Try

            If ErrorMessage.ToString.Trim() <> "" Then
                Contents = "Err.Date: " & DateTime.Now.ToString & ControlChars.NewLine
                Contents += ErrorMessage & vbCrLf
                Contents += "UserNo :" & Session("UserNo") & vbCrLf
                Contents += "UserName :" & Session("UserName") & vbCrLf
                Contents += "User Division :" & Session("DivisionNo") & vbCrLf
                Contents += "URL:" & request.Url.ToString() & vbCrLf
                Contents += "Javascript: " & bc.EcmaScriptVersion.ToString & vbCrLf
                Contents += "VB Script: " & bc.VBScript.ToString & vbCrLf
                Contents += "Cookies: " & bc.Cookies.ToString & vbCrLf
                Contents += "Remote Address: " & remoteAddress & vbCrLf
                Contents += "=========================================================" & vbCrLf
                objWriter.WriteLine(Contents)
                objWriter.Close()
            End If

        Catch ex As Exception
        Finally
            objWriter.Close()
        End Try
    End Sub

#End Region
#Region "Select Distinct"
    Public Function SelectDistinct(ByVal SourceTable As DataTable, ByVal ParamArray FieldNames() As String) As DataTable
        Dim lastValues() As Object
        Dim newTable As DataTable

        If FieldNames Is Nothing OrElse FieldNames.Length = 0 Then
            Throw New ArgumentNullException("FieldNames")
        End If

        lastValues = New Object(FieldNames.Length - 1) {}
        newTable = New DataTable

        For Each field As String In FieldNames
            newTable.Columns.Add(field, SourceTable.Columns(field).DataType)
        Next

        For Each Row As DataRow In SourceTable.Select("", String.Join(", ", FieldNames))
            If Not fieldValuesAreEqual(lastValues, Row, FieldNames) Then
                newTable.Rows.Add(createRowClone(Row, newTable.NewRow(), FieldNames))

                setLastValues(lastValues, Row, FieldNames)
            End If
        Next

        Return newTable
    End Function

    Private Function fieldValuesAreEqual(ByVal lastValues() As Object, ByVal currentRow As DataRow, ByVal fieldNames() As String) As Boolean
        Dim areEqual As Boolean = True

        For i As Integer = 0 To fieldNames.Length - 1
            If lastValues(i) Is Nothing OrElse Not lastValues(i).Equals(currentRow(fieldNames(i))) Then
                areEqual = False
                Exit For
            End If
        Next

        Return areEqual
    End Function

    Private Function createRowClone(ByVal sourceRow As DataRow, ByVal newRow As DataRow, ByVal fieldNames() As String) As DataRow
        For Each field As String In fieldNames
            newRow(field) = sourceRow(field)
        Next

        Return newRow
    End Function

    Private Sub setLastValues(ByVal lastValues() As Object, ByVal sourceRow As DataRow, ByVal fieldNames() As String)
        For i As Integer = 0 To fieldNames.Length - 1
            lastValues(i) = sourceRow(fieldNames(i))
        Next
    End Sub

#End Region
#Region "Fill DropDown"
    Public Sub FillDropDown(ByVal obj As Object, ByVal DTable As DataTable, ByVal DView As DataView, ByVal DataTextField As String, ByVal DataValueField As String, Optional ByVal StrRowFillter As String = "")
        obj.Items.Clear()
        DView = DTable.DefaultView
        DView.RowFilter = StrRowFillter
        obj.datasource = DView
        obj.DataTextField = DataTextField
        obj.DataValueField = DataValueField
        obj.databind()
    End Sub
#End Region
    'FOR Export to Excel
#Region "Convert DS TO"
    Public Function ConvertDsTO(ByVal ds As DataSet, ByVal Header As String) As String
        Dim i As Integer
        Dim iCol As Integer
        Dim strMessage As New StringBuilder()
        strMessage.Append("<table width=""100%"" border=""1"" align=""center"" cellpadding=""5"" cellspacing=""1"">")
        strMessage.Append("<tr align=""center"">")
        strMessage.Append("<td colspan=" + CType(ds.Tables(0).Columns.Count, String) + "><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
        strMessage.Append(Header)
        strMessage.Append("</font></strong></td>")
        strMessage.Append("</tr>")
        strMessage.Append("<tr>")
        For iCol = 0 To ds.Tables(0).Columns.Count - 1
            strMessage.Append("<td><strong><font color=""#000099"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append(ds.Tables(0).Columns(iCol).ToString) 'Get message from txtBody form field
            strMessage.Append("</font></strong></td>")
        Next
        strMessage.Append("</tr>")
        Dim j As Integer
        'Dim sw As System.IO.StringWriter
        'Dim hw As New System.Web.UI.HtmlTextWriter(sw)

        For j = 0 To ds.Tables(0).Rows.Count - 1
            strMessage.Append("<tr>")
            For i = 0 To ds.Tables(0).Columns.Count - 1
                strMessage.Append("<td><strong><font color=""#000099"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(ds.Tables(0).Rows(j).Item(i)) 'Get message from txtBody form field
                strMessage.Append("</font></strong></td>")
            Next
            strMessage.Append("</tr>")
        Next
        strMessage.Append("</table>")
        ConvertDsTO = strMessage.ToString
    End Function

#End Region
    'FOR Export to Excel
#Region "Export To Excel"
    Public Sub ExportToExcel(ByVal ExportedFileName As String, ByRef ds As DataSet, ByVal ReportHeader As String)
        Dim strWrite As String
        HttpContext.Current.Response.Buffer = True
        HttpContext.Current.Response.ClearContent()
        HttpContext.Current.Response.ClearHeaders()
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + ExportedFileName)
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"
        strWrite = ConvertDsTO(ds, ReportHeader)
        HttpContext.Current.Response.Write(strWrite)
        HttpContext.Current.Response.Flush()
        HttpContext.Current.Response.Close()
        HttpContext.Current.Response.End()
    End Sub
#End Region
#Region "GetDate"
    Public Property Param_FirstDateLastMonth() As String
        Get
            Param_FirstDateLastMonth = FirstDateLastMonth
        End Get
        Set(ByVal value As String)
            value = Format(CDate(Month(value).ToString + "-01-" + Year(value).ToString), "dd-MMM-yyyy")
            value = Format(CDate(DateAdd(DateInterval.Month, -1, CDate(value))), "dd-MMM-yyyy")
            FirstDateLastMonth = value
        End Set
    End Property

    Public Property Param_LastDateLastMonth() As String
        Get
            Param_LastDateLastMonth = LastDateLastMonth
        End Get
        Set(ByVal value As String)
            value = Format(DateAdd(DateInterval.Month, 1, CDate(value)), "dd-MMM-yyyy")
            value = Format(DateAdd(DateInterval.Day, -1, CDate(value)), "dd-MMM-yyyy")
            LastDateLastMonth = value
        End Set
    End Property
#End Region
#Region "Clear Cache"

    Public Shared Sub ClearCache(ByVal pg As System.Web.UI.Page)
        pg.Response.Cache.SetExpires(DateTime.Now)
        pg.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        pg.Response.AddHeader("Pragma", "no-cache")
    End Sub

#End Region
#Region "GetSelectedDate"
    Public Property Param_FirstDateMonth() As String
        Get
            Param_FirstDateMonth = FirstDateLastMonth
        End Get
        Set(ByVal value As String)
            value = Format(CDate(Month(value).ToString + "-01-" + Year(value).ToString), "dd-MMM-yyyy")
            'value = Format(CDate(DateAdd(DateInterval.Month,, CDate(value))), "dd-MMM-yyyy")
            FirstDateLastMonth = value
        End Set
    End Property

    Public Property Param_LastDateMonth() As String
        Get
            Param_LastDateMonth = LastDateLastMonth
        End Get
        Set(ByVal value As String)
            value = Format(DateAdd(DateInterval.Month, 1, CDate(value)), "dd-MMM-yyyy")
            value = Format(DateAdd(DateInterval.Day, -1, CDate(value)), "dd-MMM-yyyy")
            LastDateLastMonth = value
        End Set
    End Property
#End Region

    Public Function GetNullToString(ByVal Val As Object) As String
        Try
            GetNullToString = Val.ToString
        Catch ex As Exception
            GetNullToString = ""
        End Try
    End Function
    Public Function GetObjectOpenMode(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum) As String
        If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
            Return "ADD"
        ElseIf Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete Then
            Return "Delete"
        ElseIf Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
            Return "Edit"
        ElseIf Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
            Return "View"
        End If
    End Function

    Public Function GetScopeValueWithCondition(ByRef wStr_ScopeValue As String) As Boolean
        GetScopeValueWithCondition = False
        Try
            If Convert.ToString(HttpContext.Current.Session(S_ScopeValue)).Length > 0 Then
                wStr_ScopeValue = Convert.ToString(HttpContext.Current.Session(S_ScopeValue))
                wStr_ScopeValue = " vProjectTypeCode in (" + wStr_ScopeValue + ")"
                GetScopeValueWithCondition = True
            End If
        Catch ex As Exception

        End Try
    End Function

    '========================Added on 06-05-2009============For PassWord Policy======By Chandresh Vanker
    '===Added stripaddress_1
#Region "Login/Logout functions"
    Public Function InsertUserLoginDetails(ByVal UserID_1 As String, ByVal stripaddress_1 As String, _
                                      ByVal UTCHourDiff As String, ByVal userAgent As String, ByRef eStr_Retu As String, ByVal sGUID As String, ByVal opType As String) As Boolean         'Changed By Ronak S. Khilosiya for userAgent

        Dim dsUserLoginDetails As New DataSet
        Dim drUserLoginDetails As DataRow
        Dim ObjHelp As WS_HelpDbTable.WS_HelpDbTable = GetHelpDbTableRef()
        Dim ObjLambda As WS_Lambda.WS_Lambda = GetHelpDbLambdaRef()

        Try

            InsertUserLoginDetails = False

            If Not ObjHelp.GetUserLoginDetails("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                               dsUserLoginDetails, eStr_Retu) Then
                eStr_Retu = "Error while retrieving User Login Details" + vbCrLf + eStr_Retu
                Exit Function
            End If

            drUserLoginDetails = dsUserLoginDetails.Tables(0).NewRow
            drUserLoginDetails("iUserID") = UserID_1
            drUserLoginDetails("dLoginDateTime") = DateTime.Now.ToString("dd-MMM-yyyy")
            drUserLoginDetails("dLastActivityDate") = DateTime.Now.ToString("dd-MMM-yyyy")
            '==added for IpAddress By Mrunal Parekh
            drUserLoginDetails("vIPAddress") = stripaddress_1
            drUserLoginDetails("vUTCHourDiff") = UTCHourDiff
            drUserLoginDetails("vUserAgent") = userAgent               'Added By Ronak S. Khilosiya
            drUserLoginDetails("vGUId") = sGUID      ''Added by Ketan Muliya
            drUserLoginDetails("cOperationType") = opType      ''Added by Ketan Muliya
            '======
            dsUserLoginDetails.Tables(0).Rows.Add(drUserLoginDetails)
            dsUserLoginDetails.Tables(0).AcceptChanges()

            If Not ObjLambda.Save_UserLoginDetails(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                                dsUserLoginDetails, "1", eStr_Retu) Then
                eStr_Retu = "Error while Saving in Login User Details" + vbCrLf + eStr_Retu
                Exit Function
            End If

            InsertUserLoginDetails = True

        Catch ex As Exception
            eStr_Retu = "Error occured while inserting record in Login User Details " + vbCrLf + eStr_Retu
            InsertUserLoginDetails = False
        End Try

    End Function

    '' Commented By Jeet Patel on 08-May-2015

    'Public Function UpdateUserLoginDetails(ByVal UserID_1 As String, _
    '                                         ByRef eStr_Retu As String, ByRef isAvailable As Boolean, ByVal strIpAddress As String, ByVal strUserAgent As String) As Boolean
    '    Dim dsUserLoginDetails As New DataSet
    '    Dim ObjHelp As New WS_HelpDbTable.WS_HelpDbTable
    '    Dim ObjLambda As New WS_Lambda.WS_Lambda
    '    Dim wstr As String = String.Empty
    '    Try
    '        'UpdateUserLoginDetails = False
    '        'wstr += " and vIPAddress = '" + strIpAddress.ToString + "' and vUserAgent = '" + strUserAgent.ToString + "' "

    '        'If Not ObjHelp.GetUserLoginDetails(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '        '                                   dsUserLoginDetails, eStr_Retu) Then
    '        '    eStr_Retu = "Error while retrieving User Login Details" + vbCrLf + eStr_Retu
    '        '    Exit Function
    '        'End If

    '        'If Not dsUserLoginDetails Is Nothing Then
    '        '    If dsUserLoginDetails.Tables(0).Rows.Count > 0 Then

    '        '        If Not ObjLambda.Save_UserLoginDetails(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
    '        '                                      dsUserLoginDetails, "1", eStr_Retu) Then
    '        '            eStr_Retu = "Error while Updating User Login Details" + vbCrLf + eStr_Retu
    '        '            Exit Function
    '        '        End If
    '        '    Else
    '        '        isAvailable = True

    '        '    End If

    '        'End If

    '        'UpdateUserLoginDetails = True

    '        UpdateUserLoginDetails = False
    '        wstr = " iuserid = " + UserID_1.ToString

    '        If Not ObjHelp.GetUserLoginDetails(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                           dsUserLoginDetails, eStr_Retu) Then
    '            eStr_Retu = "Error while retrieving User Login Details" + vbCrLf + eStr_Retu
    '            Exit Function
    '        End If

    '        'If Not dsUserLoginDetails Is Nothing Then
    '        If Not dsUserLoginDetails Is Nothing AndAlso dsUserLoginDetails.Tables(0).Rows.Count > 0 Then

    '            wstr += " and vIPAddress = '" + strIpAddress.ToString + "' and vUserAgent = '" + strUserAgent.ToString + "' "

    '            If Not ObjHelp.GetUserLoginDetails(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                               dsUserLoginDetails, eStr_Retu) Then
    '                eStr_Retu = "Error while retrieving User Login Details" + vbCrLf + eStr_Retu
    '                Exit Function
    '            End If


    '            If dsUserLoginDetails.Tables(0).Rows.Count > 0 Then

    '                If Not ObjLambda.Save_UserLoginDetails(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
    '                                              dsUserLoginDetails, "1", eStr_Retu) Then
    '                    eStr_Retu = "Error while Updating User Login Details" + vbCrLf + eStr_Retu
    '                    Exit Function
    '                End If
    '            Else
    '                isAvailable = True

    '            End If
    '        Else
    '            isAvailable = False
    '        End If

    '        UpdateUserLoginDetails = True

    '    Catch ex As Exception
    '        eStr_Retu = "Error occured while Saving User Login Details " + vbCrLf + ex.Message
    '        UpdateUserLoginDetails = False
    '    End Try

    'End Function

    'Public Function DeleteUserLoginDetails(ByVal UserID_1 As String, _
    '                                       ByRef eStr_Retu As String) As Boolean
    '    DeleteUserLoginDetails = True

    '    Dim dsUserLoginDetails As New DataSet
    '    Dim ObjHelp As New WS_HelpDbTable.WS_HelpDbTable
    '    Dim ObjLambda As New WS_Lambda.WS_Lambda

    '    Try

    '        If Not ObjHelp.GetUserLoginDetails("iUserID = " + UserID_1, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                               dsUserLoginDetails, eStr_Retu) Then
    '            eStr_Retu = "Error while retrieving User Login Details" + vbCrLf + eStr_Retu
    '            Exit Function
    '        End If

    '        If Not ObjLambda.Save_UserLoginDetails(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, _
    '                                              dsUserLoginDetails, "1", eStr_Retu) Then
    '            eStr_Retu = "Error while Deleting User Login Details" + vbCrLf + eStr_Retu
    '            Exit Function
    '        End If



    '        DeleteUserLoginDetails = True
    '    Catch ex As Exception
    '        eStr_Retu = ex.Message
    '        DeleteUserLoginDetails = False
    '    End Try
    'End Function
    '''''''''''''''''''''''''''''''''''''''''''''''''''

    ' Added By Jeet Patel on 08-May-2015
    Public Function UpdateUserLoginDetails(ByVal UserID_1 As String, _
                                            ByRef eStr_Retu As String, ByRef isAvailable As Boolean, ByVal strIpAddress As String, ByVal strUserAgent As String, ByVal sGUID As String) As Boolean
        Dim dsUserLoginDetails As New DataSet
        Dim ObjHelp As WS_HelpDbTable.WS_HelpDbTable = GetHelpDbTableRef()
        Dim ObjLambda As WS_Lambda.WS_Lambda = GetHelpDbLambdaRef()
        Dim wstr As String = String.Empty
        Try
            'UpdateUserLoginDetails = False
            'wstr += " and vIPAddress = '" + strIpAddress.ToString + "' and vUserAgent = '" + strUserAgent.ToString + "' "

            'If Not ObjHelp.GetUserLoginDetails(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '                                   dsUserLoginDetails, eStr_Retu) Then
            '    eStr_Retu = "Error while retrieving User Login Details" + vbCrLf + eStr_Retu
            '    Exit Function
            'End If

            'If Not dsUserLoginDetails Is Nothing Then
            '    If dsUserLoginDetails.Tables(0).Rows.Count > 0 Then

            '        If Not ObjLambda.Save_UserLoginDetails(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
            '                                      dsUserLoginDetails, "1", eStr_Retu) Then
            '            eStr_Retu = "Error while Updating User Login Details" + vbCrLf + eStr_Retu
            '            Exit Function
            '        End If
            '    Else
            '        isAvailable = True

            '    End If

            'End If

            'UpdateUserLoginDetails = True
            ' Change by Jeet Patel on 07-May-2015 to get Data name wise from view insted of Table
            UpdateUserLoginDetails = False
            wstr = " vUserName = '" & UserID_1.ToString & "'"

            If Not ObjHelp.View_Userlogindetails(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                               dsUserLoginDetails, eStr_Retu) Then
                eStr_Retu = "Error while retrieving User Login Details" + vbCrLf + eStr_Retu
                Exit Function
            End If

            'If Not dsUserLoginDetails Is Nothing Then
            If Not dsUserLoginDetails Is Nothing AndAlso dsUserLoginDetails.Tables(0).Rows.Count > 0 Then

                wstr += " and vIPAddress = '" + strIpAddress.ToString + "' and vUserAgent = '" + strUserAgent.ToString + "' and vGUId = '" + sGUID.ToString + "' "

                If Not ObjHelp.View_Userlogindetails(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                   dsUserLoginDetails, eStr_Retu) Then
                    eStr_Retu = "Error while retrieving User Login Details" + vbCrLf + eStr_Retu
                    Exit Function
                End If

                dsUserLoginDetails.Tables(0).Columns.Remove("vLocationCode")
                dsUserLoginDetails.Tables(0).Columns.Remove("vUserName")
                dsUserLoginDetails.Tables(0).Columns.Remove("Activeuser")
                dsUserLoginDetails.Tables(0).Columns.Remove("vusertypeName")
                dsUserLoginDetails.Tables(0).Columns.Remove("tmp_dLoginDateTime")
                dsUserLoginDetails.Tables(0).TableName = "UserLoginDetails"
                dsUserLoginDetails.AcceptChanges()
                ' ' ==============================================================

                If dsUserLoginDetails.Tables(0).Rows.Count > 0 Then

                    If Not ObjLambda.Save_UserLoginDetails(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                                                  dsUserLoginDetails, "1", eStr_Retu) Then
                        eStr_Retu = "Error while Updating User Login Details" + vbCrLf + eStr_Retu
                        Exit Function
                    End If
                Else
                    isAvailable = True

                End If
            Else

                If eStr_Retu.ToUpper().ToString().Contains("DEFAULT.ASPX") Then
                    isAvailable = False
                Else
                    isAvailable = True
                End If
            End If

            UpdateUserLoginDetails = True

        Catch ex As Exception
            eStr_Retu = "Error occured while Saving User Login Details " + vbCrLf + ex.Message
            UpdateUserLoginDetails = False
        End Try

    End Function

    Public Function DeleteUserLoginDetails(ByVal UserID_1 As String, _
                                           ByRef eStr_Retu As String) As Boolean
        DeleteUserLoginDetails = True

        Dim dsUserLoginDetails As New DataSet
        Dim ObjHelp As WS_HelpDbTable.WS_HelpDbTable = GetHelpDbTableRef()
        Dim ObjLambda As WS_Lambda.WS_Lambda = GetHelpDbLambdaRef()

        Try
            ' Change by Jeet Patel on 07-May-2015 to get Data name wise from view insted of Table
            If Not ObjHelp.View_Userlogindetails("vUserName ='" & UserID_1 & "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                   dsUserLoginDetails, eStr_Retu) Then
                eStr_Retu = "Error while retrieving User Login Details" + vbCrLf + eStr_Retu
                Exit Function
            End If

            dsUserLoginDetails.Tables(0).Columns.Remove("vLocationCode")
            dsUserLoginDetails.Tables(0).Columns.Remove("vUserName")
            dsUserLoginDetails.Tables(0).Columns.Remove("Activeuser")
            dsUserLoginDetails.Tables(0).Columns.Remove("vusertypeName")
            dsUserLoginDetails.Tables(0).Columns.Remove("tmp_dLoginDateTime")
            dsUserLoginDetails.Tables(0).TableName = "UserLoginDetails"
            dsUserLoginDetails.AcceptChanges()
            ' ' ==============================================================

            If Not ObjLambda.Save_UserLoginDetails(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, _
                                                  dsUserLoginDetails, "1", eStr_Retu) Then
                eStr_Retu = "Error while Deleting User Login Details" + vbCrLf + eStr_Retu
                Exit Function
            End If



            DeleteUserLoginDetails = True
        Catch ex As Exception
            eStr_Retu = ex.Message
            DeleteUserLoginDetails = False
        End Try
    End Function
    '''''''''''''''''''''''
#End Region

    '================================================================================

    '#Region "ChkLockedScreenDate"

    '    Public Function ChkLockedScreenDate(ByVal ScreeningHdrNo As String) As Boolean

    '        Dim wStr As String = String.Empty
    '        Dim ds_MaxScrDtl As New DataSet
    '        Dim eStr As String = String.Empty

    '        Dim ObjHelp As New WS_HelpDbTable.WS_HelpDbTable

    '        Try

    '            ChkLockedScreenDate = True

    '            wStr = " nMedexScreeningHdrNo = " + ScreeningHdrNo + " and cLockUnlockFlag = 'L'"

    '            If Not ObjHelp.View_MaxScreeningLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                ds_MaxScrDtl, eStr) Then

    '                Throw New Exception(eStr)

    '            End If

    '            If ds_MaxScrDtl.Tables(0).Rows.Count > 0 Then

    '                ChkLockedScreenDate = False
    '            End If


    '        Catch ex As Exception
    '            eStr = ex.Message
    '            ChkLockedScreenDate = False
    '        End Try

    '    End Function


#Region "SS Security"
    'Added for SS Security 

    Private Function GetWebServiceObject(ByVal WebServiceType As Type) As System.Web.Services.Protocols.SoapHttpClientProtocol
        Return SS.Web.ServerHelper.CreateWebServiceInstance(WebServiceType)
    End Function
    Public Function GetHelpDbTableRef() As WS_HelpDbTable.WS_HelpDbTable
        Return GetWebServiceObject(GetType(WS_HelpDbTable.WS_HelpDbTable))
    End Function

    Public Function GetHelpDbLambdaRef() As WS_Lambda.WS_Lambda
        Return GetWebServiceObject(GetType(WS_Lambda.WS_Lambda))
    End Function

    Public Function GetLabIntegrationRef() As LabIntegration.LabIntegration
        Return GetWebServiceObject(GetType(LabIntegration.LabIntegration))
    End Function

    Public Function GetKnowledgeNETPublishRef() As Publish.WS_KnowledgeNETPublish
        Return GetWebServiceObject(GetType(Publish.WS_KnowledgeNETPublish))
    End Function

    'Ended for SS Security
#End Region

End Class
Public Module GeneralModule
    Public Const S_CompanyNo As String = "CompanyNo"
    Public Const S_LoginName As String = "LoginName"
    Public Const S_UserID As String = "UserID"
    Public Const S_UserID1 As String = "UserID1"
    Public Const S_UserName As String = "UserName"
    Public Const S_FirstName As String = "FirstName"
    Public Const S_LastName As String = "LastName"
    Public Const S_Password As String = "Password"
    Public Const S_UserType As String = "UserType"
    Public Const S_UserGroup As String = "UserGroup"
    Public Const S_ValidUser As String = "ValidUser"
    Public Const S_TimeOffSet As String = "timeOffSet"
    Public Const S_Time As String = "Time"
    Public Const S_Name As String = "Name"
    Public Const S_ProjectId As String = "ProjectId"
    Public Const S_ProjectName As String = "ProjectName"
    Public Const S_PeriodId As String = "ActivityId"
    Public Const S_CaseRecordId As String = "CaseRecordId"
    Public Const S_LocationCode As String = "LocationCode"
    Public Const S_TimeZoneName As String = "TimeZoneName"
    Public Const S_ScopeNo As String = "ScopeNo"
    Public Const S_ScopeValue As String = "ScopeValues"
    Public Const S_DeptCode As String = "DeptCode"
    Public Const S_EDCUser As String = "EDCUser"
    Public Const S_DynamicPage_URL As String = "DynamicPage_URL"
    Public Const S_IpAddress As String = "IP Address"
    Public Const S_Login As String = "Login Flag"
    Public Const S_GUID As String = "Unique Id"
    Public Const S_LastLoginDateTime As String = "LastLoginDateTime"
    Public Const S_UserNameWithProfile As String = "LoginUserWithProfile"

    Public Const S_TabulerRepeatition As String = "S_TabulerRepeatition"
    Public Const S_GetLetestData As String = "S_GetLetestData"
    Public Const MSLType_Doctor As String = "D"
    Public Const MSLType_Chemist As String = "C"
    Public Const MSLType_Stockiest As String = "W"
    Public Const MSLType_NursingHome As String = "N"
    Public Const S_DataStatus As String = "DataStatus"
    Public Const Validation_NotNullInteger = 1
    Public Const Validation_NotNullNumeric = 2
    Public Const Validation_Integer = 3
    Public Const Validation_Numeric = 4
    Public Const Validation_Alphabet = 5
    Public Const Validation_Alphanumeric = 6
    Public Const Val_AN As String = "AN" '"Alphanumeric"
    Public Const Val_AL As String = "AL" '"Alphabate"
    Public Const Val_NU As String = "NU" '"Numeric"
    Public Const Val_IN As String = "IN" '"Integer"
    Public Const Val_NNI As String = "NNI" '"NotNull Integer"
    Public Const Val_NNU As String = "NNU" '"NotNull Numeric"
    Public Const MileStone_None = 0
    Public Const MileStone_Monitoring = 1
    Public Const MileStone_Scheduling = 2
    Public Const MileStone_Monitoring_Scheduling = 3

    Public Const Act_Attendance As String = "0001" 'Attendance Activity
    Public Const Act_Checkin As String = "0002" 'Check-in Activity
    Public Const Act_Screening As String = "0003" 'Screening Activity
    Public Const Act_IPAdmin As String = "1100" 'IP Administrator Activity
    Public Const Act_CheckOut As String = "1088" 'IP Administrator Activity
    Public Const S_DataSet As String = "DataSet" 'For CDMS Advance Search to Scheduling Page
    Public Const S_ScrProfileIndex As String = "ScrProfileIndex" 'selected index of local screening profile change


    Public Const Pro_Screening As String = "0000000000" 'Screening Project

    Public Const Stage_Created As String = "12" 'Stage Created
    Public Const Stage_published As String = "21" 'stage Created by vishal for published 
    Public Const Stage_Authorized As String = "20" 'Stage Created
    Public Const ActiveFlag_Yes As String = "Y" 'Active
    Public Const ActiveFlag_No As String = "N" 'InActive

    Public Const Medex_BMI As String = "00323,28073" 'Medex of BMI Used in Screenig for BMI Calculation
    Public Const Medex_Height As String = "00321,28071" 'Medex of Height Used in Screenig for BMI Calculation
    Public Const Medex_Weight As String = "00443,28072" 'Medex of Weight Used in Screenig for BMI Calculation

    Public Const Medex_Temperature_F As String = "00362" 'Medex of Temperature in Fahrenheit Used in Screenig for Coversion
    Public Const Medex_Temperature_C As String = "00610" 'Medex of Temperature in Celsius Used in Screenig for Coversion

    Public Const Medex_XRayDate As String = "00001" 'Medex of X-RayDate

    Public Const Medex_DateOfBirth As String = "00608,28065" 'Medex of Date of Birth in Screening to get age.
    Public Const Medex_Age As String = "00609,28066" 'Medex of Age in Screening to get age from DOB.

    Public Const Scope_SAdmin As Integer = 5 'ScopeNo of Super user used in ProjectTypeMst form
    Public Const Scope_SAll As Integer = 7 'ScopeNo of All added by Mrunal on 19-Nov-2011

    Public Const ProjectTypeCode_QC As String = "0010" 'QC Type Projects for Getting Projects in ActivityMaster for Linked Documents
    Public Const ProjectTypeCode_BABE As String = "0002" 'For CRFLockDtl page

    Public Const Medex_VersionNo As String = "00003"
    Public Const Medex_ReleaseDate As String = "00004"
    Public Const Medex_NoOfCopies As String = "00005"
    Public Const Medex_Purpose As String = "00006"
    Public Const Medex_FilePath As String = "00007"
    Public Const Medex_DownloadedBy As String = "00008"
    Public Const Medex_Project As String = "00009"
    Public Const Medex_Activity As String = "00010"
    Public Const Medex_ScanedFile As String = "00011"
    Public Const Medex_SequenceNo As String = "00012"
    Public Const Medex_Oral_Body_Temperature_F As String = "00667" 'Medex of Temperature in Fahrenheit Used in Vital Signs for Coversion
    Public Const Medex_Oral_Body_Temperature_C As String = "00668" 'Medex of Temperature in Celsius Used in Vital Signs for Coversion
    Public Const medex_DosingLabel As String = "01281" 'Medex of Dosing Label used to capture Label BarcodeId 

    'Added for sample collection
    Public Const Medex_Date As String = "00649"
    Public Const Medex_Time As String = "00650"

    Public Const Op_Slot As String = "0002"

    'Added for Sending Email Functionality
    Public Const Email_QCOFPIF = 1
    Public Const Email_QCOFSCREENING = 2
    Public Const Email_QCOFMedexINfoHdr = 3

    Public Const S_ActivityIds As String = "ActivityIds"

    'Added for getting Menu only of BizNEt Web Application
    Public Const OpType_BizNetWeb As String = "BW"
    'Added for getting Menu only of BizNEt Desktop Application on 11-Jan-2010 By Chandresh Vanker
    Public Const OpType_BizNetDesktop As String = "SC"
    'Added for getting Menu only of BizNEt LIMS Application on 18-Jan-2010 By Chandresh Vanker
    Public Const OpType_BizNetLIMS As String = "LD"
    'Added for getting Menu only of Biolyte Application on 05-May-2015 By Maitri Parikh
    Public Const OpType_Biolyte As String = "BL"
    'Added for getting Menu only of Pharmacy Management on 05-July-2017 By Ketan Muliya
    Public Const OpType_PharmacyManagement As String = "PM"
    'Added for getting Menu only of Medical Imaging on 16-Jan-2017 By Vivek Patel
    Public Const OpType_MedicalImaging As String = "MI"
    'Added for getting Menu only of OIMS on 21-Nov-2017 By Ketan Muliya
    Public Const OpType_OIMS As String = "IM"
    'Added for getting Menu only of SDTM on 21-Nov-2017 By Ketan Muliya
    Public Const OpType_SDTM As String = "SD"


    'Added for Samples Send And Receive
    Public Const Sample_Sent As String = "S"
    Public Const Sample_Received As String = "R"
    Public Const Sample_NotReceived As String = "N"
    Public Const Sample_Rejected As String = "J"
    Public Const Sample_Disputed As String = "D"

    Public Const BleedSheet_Remarks As String = "00733"
    Public Const BleedSheet_RemarksIfOther As String = "00734"
    Public Const BleedSheet_DosingDate As String = "00728"
    Public Const BleedSheet_SampleCollDate As String = "00729"
    Public Const BleedSheet_ScheduledTime As String = "00730"
    Public Const BleedSheet_ActualTime As String = "00731"
    Public Const BleedSheet_PKSampleId As String = "00155"
    Public Const PDSample_Remarks As String = "15356"
    'Added on 24-Sep-2009 for checking usertype (compulsory selection of project to CTM users)
    Public Const UserType_CTM As String = "0008"

    Public Const Report_Export_In_Pdf As String = "Pdf"
    Public Const S_ReportName As String = "ReportName"

    'Added on 30-Dec-2009 For getting Work Flow Stage Id -- By Chandresh Vanker
    Public Const S_WorkFlowStageId As String = "WorkFlowStageId"
    Public Const WorkFlowStageId_DataEntry As String = "0"
    Public Const WorkFlowStageId_MedicalCoding As String = "1"
    Public Const WorkFlowStageId_DataValidator As String = "2"
    Public Const WorkFlowStageId_DeleteDataEntry As String = "3"
    Public Const WorkFlowStageId_OnlyView As String = "5"
    Public Const WorkFlowStageId_FirstReview As String = "10"
    Public Const WorkFlowStageId_SecondReview As String = "20"
    Public Const WorkFlowStageId_FinalReviewAndLock As String = "30"
    Public Const EDCUser As String = "Y"

    'Added on 02-Feb-2010 For Getting Data Status of CRF -- By Chandresh Vanker
    Public Const CRF_DataEntryPending As String = "A"
    Public Const CRF_DataEntry As String = "B"
    Public Const CRF_DataEntryCompleted As String = "C"
    Public Const CRF_Review As String = "D"
    Public Const CRF_ReviewCompleted As String = "E"
    Public Const CRF_Locked As String = "F"
    Public Const CRF_Rejected As String = "R"
    Public Const CRFHdr_Locked As String = "L"
    Public Const CRFHdr_Unlocked As String = "U"


    Public Const Query_New As String = "N"
    Public Const Query_Generated As String = "G"
    Public Const Query_Resolved As String = "R"

    Public Const Discrepancy_Generated As String = "N"
    Public Const Discrepancy_AutoResolved As String = "A"
    Public Const Discrepancy_Resolved As String = "R"
    Public Const Discrepancy_Answered As String = "O"
    Public Const Discrepancy_ReReview As String = "Re-reviewed"

    '' Added on 12-May-2011 beacuse of aadding functionallity of Internally Resolved Functionallity ''
    Public Const Discrepancy_InternallyResolved As String = "I"
    '' ******************************************************************************************** ''

    'Added on 28-Apr-2010 for getting tests Groupwise in LabReportReview
    Public Const MedexGrp_Chemistry As String = "00097"
    Public Const MedexGrp_IMMUNOLOGY As String = "00098"
    Public Const MedexGrp_HEMATOLOGY As String = "00099"
    Public Const MedexGrp_URIANALYSIS As String = "00100"
    Public Const MedexGrp_COAGULATION As String = "00101"
    Public Const MedexGrp_STOOL As String = "00103"
    Public Const MedexGrp_HIV1 As String = "00104"
    Public Const MedexGrp_PAPSmear As String = "00105"
    Public Const MedexGrp_CYTOLOGY As String = "00470"
    Public Const MedexGrp_UNRINECYTOLOGY As String = "00581"
    Public Const MedexGrp_IMMUNOGENICITY_TESTING As String = "00411"

    'Added on 12-feb-10 for getting templates of T1 in CopyProject--By Deepak Singh
    Public Const TemplateTypeCode As String = "0001"

    Public Const S_Profiles As String = "UserTypeMst" 'Added for "Profile Selection" 
    Public Const S_SelectedActivity As String = "SelectedActivity" 'Added for frmCTMMedExInfoHdrDtl page Activity Dropdown selection
    Public Const S_SelectedTab As String = "S_SelectedTab" 'Added for frmCTMMedExInfoHdrDtl page Tab Activation
    Public Const S_SelectedRepeatation As String = "SelectedRepeatation"
    Public Const S_RepeatationShow As String = "S_RepeatationShow"

    Public Const Medex_Eligibilitydeclaredby As String = "11171"
    Public Const Medex_Eligibilitydeclaredon As String = "11172"
    Public Const Medex_PIComments As String = "11166"
    Public Const Medex_PICommentsgivenon As String = "11173"

    Public Const MedExCodeForSex As String = "00440"
    Public Const MedExResultForMale As String = "Male"
    Public Const MedExGroupDescForFemale As String = " For Female Only "
    Public Const MedExGroupDescForMale As String = "Not Applicable For Male"

    Public Const Scope_ClinicalTrial As Integer = 1
    Public Const Scope_BABE As Integer = 2
    Public Const Medex_ClinicallyFit As String = "00617" 'Medex of Clinically fit in Screening 
    Public Const Medex_Physician As String = "00618" 'Medex of Physician in Screening 
    Public Const Medex_SubjectFoundEligible As String = "00476" 'Medex of Subject Found Eligible in Screening 
    Public Const Medex_PI_Co_I_Designate As String = "11184" 'Medex of PI/Co-I/Designate in Screening 
    Public Const Medex_RecreationlDrug As String = "00604"
    Public Const Medex_RecordedBy As String = "00454"
    Public Const Medex_Clinically_ECG As String = "00415"
    Public Const Medex_RecordedBy_ECG As String = "00416"
    Public Const Medex_Clinically_Lab As String = "00418"
    Public Const Medex_RecordedBy_Lab As String = "00419"
    Public Const Medex_RecordedBy_BMI As String = "11185"
    Public Const Medex_Consent_SCr As String = "00324"
    Public Const Medex_Clinically_Xray As String = "00413"
    Public Const Medex_RecordedBy_Xray As String = "00411"


    Public Const Act_EndStudy As String = "1090" 'End Study Activtiy's Activity Id
    Public Const Act_SampleAnalysis As String = "1076" 'Sample Analysis's Activity Id
    Public Const Act_ReportDispatch As String = "1418" 'Report Dispacth's Activity Id
    Public Const Act_BARowdataandQAforAudit As String = "1419" 'BA Row Data & Report to QA for Audit's Activity Id
    Public Const Act_ClinicaltoMedicalwriting As String = "1405" 'Clinical to Medical writing's Activity Id
    Public Const Act_ReportReleased As String = "1415" 'Report Released

    Public Const Status_New As String = "N"
    Public Const Status_Edit As String = "E"
    Public Const Status_Delete As String = "D"
    Public offset As Double = 0.0
    Public Const strServerOffset As String = " IST (+5:30 GMT)"
    Public SuperUserId As Integer = 1
    Public SuperUserType As String = "0001"
    Public ScreeningGroup As String = String.Empty
    Public dScreenDate As String = String.Empty
    Public TranscribeRemarks As String = String.Empty
    Public IsSaved As Boolean
    Public IsProjectSpecificScreening As Boolean
    Public hProjectIdWorkSpace As String
    Public hProjectDescProject As String
    Public DCFGenerated As String
    Public DCFMedExCode As String
    Public ScreeningDTLNo As String
    Public GroupValidation As Boolean = False
    Public isTransCribe As String
    Public IsGunned As Boolean = False


    Public Function GetSelectedValuesInString(ByVal Dgv As GridView, _
                                              ByVal ValueColumn_1 As Integer, _
                                              ByVal ValueIsString_1 As Boolean) As String

        Dim Instr_retu As String = ""
        For Each gRow As GridViewRow In Dgv.Rows

            For Each Cnt As Control In gRow.Cells(0).Controls

                If TypeOf Cnt Is CheckBox Then

                    If CType(Cnt, CheckBox).Checked Then

                        Instr_retu += IIf(Instr_retu = "", "", ",") + IIf(ValueIsString_1, "'", "") + gRow.Cells(ValueColumn_1).Text + IIf(ValueIsString_1, "'", "")
                    End If

                End If

            Next Cnt

        Next gRow

        If Instr_retu.Trim() <> "" Then
            Instr_retu = "(" + Instr_retu + ")"
        End If

        Return Instr_retu

    End Function

    Public Function GetSelectedValuesInString(ByVal CheckBoxList_1 As CheckBoxList, _
                                              ByVal ValueIsString_1 As Boolean) As String

        Dim Instr_Retu As String = ""

        For Each l As System.Web.UI.WebControls.ListItem In CheckBoxList_1.Items

            If l.Selected Then
                Instr_Retu += IIf(Instr_Retu = "", "", ",") + IIf(ValueIsString_1, "'", "") + _
                              l.Value.ToString() + IIf(ValueIsString_1, "'", "")
            End If

        Next l

        If Instr_Retu.Trim() <> "" Then
            Instr_Retu = "(" + Instr_Retu + ")"
        End If
        Return Instr_Retu

    End Function

    Public Function GetReportName() As String

        GetReportName = System.Guid.NewGuid.ToString()
        GetReportName = GetReportName.Replace("{", "").Replace("}", "").Replace("-", "")

    End Function

    Public Function InsertOtpINfo(ByVal UserID_1 As String, ByVal OtpCode As String,
                                           ByRef eStr_Retu As String)

        Dim dsOtpInfo As New DataSet
        Dim objCommon As New clsCommon
        Dim drOtpInfo As DataRow

        Dim ObjHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim ObjLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

        Try

            InsertOtpINfo = False

            If Not ObjHelp.GetOTPInfoDetails("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty,
                                               dsOtpInfo, eStr_Retu) Then
                eStr_Retu = "Error while retrieving Otp Info Details" + vbCrLf + eStr_Retu
                Exit Function
            End If

            drOtpInfo = dsOtpInfo.Tables(0).NewRow
            drOtpInfo("iUserID") = UserID_1
            drOtpInfo("dStartTime") = Format(Now, "hh:mm:ss tt")
            drOtpInfo("dEndTime") = Format(Now().AddMinutes(10), "hh:mm:ss tt")
            drOtpInfo("vOTPNo") = OtpCode
            drOtpInfo("IsActive") = "Y"

            dsOtpInfo.Tables(0).Rows.Add(drOtpInfo)
            dsOtpInfo.Tables(0).AcceptChanges()

            If Not ObjLambda.Save_OtpInfoDetails(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add,
                                                dsOtpInfo, "1", eStr_Retu) Then
                eStr_Retu = "Error while Saving in Login Otp Details" + vbCrLf + eStr_Retu
                Exit Function
            End If

            InsertOtpINfo = True

        Catch ex As Exception
            eStr_Retu = "Error occured while inserting record in Login Otp Info Details " + vbCrLf + eStr_Retu
            InsertOtpINfo = False

        End Try

    End Function

End Module


