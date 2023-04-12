<%@ Application Language="VB" %>
<%@ Import Namespace="System.Web.Configuration" %>

<script RunAt="server">

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)

        ' Code that runs on application startup
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
    End Sub

    Sub Application_PreRequestHandlerExecute(ByVal sender As Object, ByVal e As EventArgs)
        If Not HttpContext.Current.Session Is Nothing Then
            If Not HttpContext.Current.Session(S_UserID) Is Nothing AndAlso
                Convert.ToString(HttpContext.Current.Session(S_UserID)).Trim.Length > 0 Then
                Dim objCommon As New clsCommon
                Dim eStr_Retu As String = ""
                Dim strRedirect As String = String.Empty
                Dim strIpAddress As String = String.Empty
                Dim strFilePath As String = String.Empty
                Dim strUserAgent As String = String.Empty
                Dim isAvailable As Boolean = False
                Dim sGUId As String = String.Empty  ''Added by Ketan Muliya

                ''Below code commented for logout issue in dynamic IP
                ''strIpAddress = Me.Request.Headers("HTTP_X_FORWARDED_FOR")
                ''If strIpAddress Is Nothing OrElse strIpAddress = String.Empty Then
                ''    strIpAddress = Request.ServerVariables("REMOTE_ADDR")
                ''End If
                strIpAddress = Convert.ToString(HttpContext.Current.Session(S_IpAddress))
                sGUId = Convert.ToString(HttpContext.Current.Session(S_GUID))  ''Added by Ketan Muliya
                ''Ended
                strFilePath = Request.FilePath
                eStr_Retu = strFilePath

                strUserAgent = Request.UserAgent
                If strUserAgent.IndexOf("MSIE") > -1 Then
                    strUserAgent = "MSIE"
                ElseIf strUserAgent.IndexOf("Firefox/") > -1 Then
                    strUserAgent = "Firefox"
                ElseIf strUserAgent.IndexOf("Chrome/") > -1 Then
                    strUserAgent = "Chrome"
                ElseIf strUserAgent.Contains("CriOS/") Then
                    strUserAgent = "Chrome"
                ElseIf strUserAgent.Contains("Safari/") Then
                    strUserAgent = "Safari"
                Else
                    strUserAgent = "Other"
                End If

                ' Change By Jeet Patel To Restrict user Login Functionality Name wise on 07-May-2015
                If Not objCommon.UpdateUserLoginDetails(HttpContext.Current.Session(S_UserName), eStr_Retu, isAvailable, strIpAddress, strUserAgent, sGUId) Then

                End If
                ' ==============================================================

                If Not Request.Cookies("UserSingleLogon") Is Nothing Then
                    'Response.Cookies("UserSingleLogon").Expires=DateTime.Now.AddMinutes(1)
                    Response.Cookies("UserSingleLogon").Value = System.DateTime.Now.ToString()
                Else
                    Response.Cookies("UserSingleLogon").Value = System.DateTime.Now.ToString()
                    Response.Cookies("UserSingleLogon").Expires = DateTime.Now.AddDays(1)
                End If


                If isAvailable = True And Not HttpContext.Current.Session(S_ValidUser) Is Nothing Then

                    'strRedirect = "Logoutpage.aspx?username=" + Me.Session(S_UserName) + "&usertype=" + Me.Session(S_UserType)
                    strRedirect = "default.aspx?SessionExpire=True"

                    If eStr_Retu.ToUpper().ToString().Contains("DEFAULT.ASPX") Or eStr_Retu.ToUpper().ToString().Contains("LOGOUTPAGE.ASPX") Then
                        Me.Response.Redirect(strRedirect, False)
                    Else
                        Me.Response.Redirect(strRedirect, True)
                    End If

                End If
                If TypeOf Context.Handler Is IRequiresSessionState OrElse TypeOf Context.Handler Is IReadOnlySessionState Then
                    Dim sessionState = TryCast(ConfigurationManager.GetSection("system.web/sessionState"), SessionStateSection)
                    Dim cookieName = If(sessionState IsNot Nothing AndAlso Not String.IsNullOrEmpty(sessionState.CookieName), sessionState.CookieName, "ASP.NET_SessionId")

                    If Request.Cookies(cookieName) IsNot Nothing AndAlso Session IsNot Nothing AndAlso Session.SessionID IsNot Nothing Then
                        Response.Cookies(cookieName).Value = Session.SessionID
                        Response.Cookies(cookieName).Path = Request.ApplicationPath + "; SameSite=Lax" + ";HTTPOnly"
                    End If
                End If
            End If
        End If
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub

    'Sub Application_EndRequest(ByVal sender As Object, ByVal e As EventArgs)
    '    Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate") ' HTTP 1.1.
    '    Response.AppendHeader("Pragma", "no-cache") 'HTTP 1.0.
    '    Response.AppendHeader("Expires", "0") ' Proxies.
    'End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1))
        Response.Cache.SetNoStore()
    End Sub
    Protected Sub Application_PreSendRequestHeaders()
        If HttpContext.Current IsNot Nothing Then
            HttpContext.Current.Response.Headers.Remove("Server")
            HttpContext.Current.Response.Headers.Add("CSRF-Token", System.Guid.NewGuid().ToString())
            'HttpContext.Current.Response.Headers.Add("Access-Control-Allow-Origin", "*")
            HttpContext.Current.Request.Headers.Add("CSRF-Token", System.Guid.NewGuid().ToString())
        End If
    End Sub
</script>
