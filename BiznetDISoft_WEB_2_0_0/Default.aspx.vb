Imports System.Collections.Generic
Imports System.Reflection
Imports System.Net
Imports System.Net.Mail
Imports System.Web.Script.Services
Imports Microsoft.Graph

Partial Class _Default
    'Inherits System.Web.UI.Page
    Inherits SS.Web.UI.SSPage

#Region "Variable Declaration"
    Private strLoginName As String
    Private strUsername As String
    Private strPassword As String
    Private strPassword1 As String
    Private objCommon As New clsCommon
    Private objWriter As StreamWriter
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
    Dim tmpVarUser As String = String.Empty
    Dim tmpVarPage As String = String.Empty
    Private eStr_Retu As String = String.Empty

    Private Choice As WS_Lambda.DataObjOpenSaveModeEnum
    Dim NoOfAttempt As Double
    Dim LockOutTime As Double
    Private Ds_UserLoginFailure As New DataSet
    Private BlockMsg As String = String.Empty
    Private userAgent As String = String.Empty
    Private strIpAddress As String = String.Empty
    Private strHostName As String = String.Empty
    Dim RedirectStr As String = String.Empty
    Private VS_Profiles As String = "UserTypeMst"
    Dim UTCHourDiff As String = String.Empty
    Dim isreadonly As PropertyInfo
    Private Password As String = String.Empty
    Private sGUID As String = String.Empty
    Protected Property NamePass As String
    Private VS_SMSInfo As String = "SMSInfo"
    Private VS_SMSGateWay As String = "SMSGateWayDetail"
#End Region

#Region "Form Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim wStr As String = String.Empty
        Dim ds_UserMst As New DataSet
        Dim clientname As String = String.Empty
        Dim strRedirect As String = String.Empty
        clientname = System.Configuration.ConfigurationManager.AppSettings("Client")
        Page.Title = " :: Login :: " + clientname

        Me.EnableEncryptionScript = True
        If Not Page.IsPostBack Then

            'If session expires it will show this message
            If Request.Form.Count > 0 Then
                If Request.Form.AllKeys.Contains("globaluserid") AndAlso Request.Form.AllKeys.Contains("token") Then
                    Dim strResult As String
                    Dim reader As IO.StreamReader
                    Dim writer As IO.StreamWriter
                    Dim res As String = ""
                    If Not Request.Form("token").ToString() = "" Then
                        Try
                            '    Dim targetURI As New Uri("http://localhost:12159/api/Login/ValidateToken")
                            '    Dim Request As HttpWebRequest
                            '    Request = DirectCast(WebRequest.Create(targetURI), HttpWebRequest)
                            '    Request.Method = "GET"
                            '    Request.ContentType = "application/json"
                            '    Dim Response As HttpWebResponse = Request.GetResponse()
                            '    If Response.StatusCode = System.Net.HttpStatusCode.OK Then
                            '        Dim responseStream As System.IO.StreamReader = New System.IO.StreamReader(Response.GetResponseStream())
                            '        strResult = responseStream.ReadToEnd()
                            '    End If
                            Dim client As HttpWebRequest
                            Dim targetURI As New Uri("https://90.0.1.162/UserGetWayAPI/api/Login/ValidateToken")
                            client = DirectCast(WebRequest.Create(targetURI.AbsoluteUri), HttpWebRequest)
                            client.Method = "GET"
                            client.ContentLength = 0
                            client.ContentType = "application/JSON"
                            client.ServerCertificateValidationCallback = Function() True
                            client.Headers.Add("token", Request.Form("token").ToString())

                            Dim hwebresponse As HttpWebResponse = client.GetResponse
                            reader = New IO.StreamReader(hwebresponse.GetResponseStream, System.Text.Encoding.UTF8)
                            res = reader.ReadToEnd()
                            reader.Close()

                            If Not Convert.ToBoolean(res) Then
                                Exit Sub
                            End If

                        Catch ex As System.Net.WebException
                            Throw New Exception(ex.Message)
                        End Try


                    End If

                    If Not Request.Form("globaluserid").ToString() = "" Then
                        wStr = "UUserID = '" + Request.Form("globaluserid").ToString().ToUpper + "' and cStatusIndi <> 'D'"

                        If Not Me.objHelp.getuserMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                        ds_UserMst, eStr_Retu) Then
                            Throw New Exception(eStr_Retu)
                        End If

                        If Not ds_UserMst Is Nothing Then
                            If ds_UserMst.Tables(0).Rows.Count > 0 Then
                                txtUserName.Value = Convert.ToString(ds_UserMst.Tables(0).Rows(0)("vUserName"))
                                strUsername = txtUserName.Value

                                FillProfile()
                                Me.ddlProfile.Items.FindByValue(Convert.ToString(ds_UserMst.Tables(0).Rows(0)("vUserTypeCode"))).Selected = True
                                If Convert.ToString(Me.Session("TEMPPASSWORD")) <> "" Then
                                    Password = Convert.ToString(Me.Session("TEMPPASSWORD"))
                                    Me.Session("TEMPPASSWORD") = ""
                                Else
                                    Password = ds_UserMst.Tables(0).Rows(0)("vLoginPass").ToString.Trim()
                                    Password = Me.objHelp.DecryptPassword(Password)
                                End If
                                'If Not Me.ValidateLogin() Then
                                '    Exit Sub
                                'End If
                                RLogin()
                                'Login()
                            End If
                        End If
                        Exit Sub
                    End If
                End If
            End If

            If Not IsNothing(Request.QueryString("SessionExpire")) And Convert.ToString(Request.QueryString("SessionExpire")) = "True" Then
                If hdntemp.Value Is Nothing Then
                    Me.objCommon.ShowAlert("Your previous session has expired. Please login again.", Me.Page)
                End If
                Session.RemoveAll()
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "alert", "Changeurl();", True)   ''added by Rahul shah on 1-apr-2015 

                Exit Sub
            End If
            'Added for "Profile Selection" on 12-March-2010 by Chandresh Vanker
            If Me.Request.QueryString("username") <> Nothing AndAlso _
                        Me.Request.QueryString("usertype") <> Nothing Then

                FillProfile()
                Me.ddlProfile.Items.FindByValue(Convert.ToString(Me.Request.QueryString("usertype"))).Selected = True

                txtUserName.Value = Convert.ToString(Me.Request.QueryString("username"))
                strUsername = txtUserName.Value
                wStr = "vUserName = '" + strUsername + "' and cStatusIndi <> 'D'"

                If Not Me.objHelp.getuserMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_UserMst, eStr_Retu) Then
                    Throw New Exception(eStr_Retu)
                End If

                ''txtPassword.Value = ds_UserMst.Tables(0).Rows(0)("vLoginPass").ToString.Trim()
                ''txtPassword.Value = Me.objHelp.DecryptPassword(txtPassword.Value)
                Password = ds_UserMst.Tables(0).Rows(0)("vLoginPass").ToString.Trim()
                Password = Me.objHelp.DecryptPassword(Password)
                Me.hdnRelogin.Value = True
                If Not Me.IsPostBack Then
                    Me.NamePass = Password
                End If
                Login()
                Exit Sub
            End If


        End If

    End Sub

#End Region

#Region "RLogin"
    Private Sub RLogin()
        Dim ds As New DataSet
        Dim dv As New DataView
        Dim dt As New DataTable
        Dim dr As Data.DataRow
        Dim ScopeValue As String = String.Empty
        Dim strLoginErrorMsg As String = String.Empty
        Dim ds_Check As New DataSet
        Dim wStr As String = String.Empty
        Dim Authorized As Boolean = False
        Dim strTimeZone As String = String.Empty
        Dim str() As String
        Dim InActiveFlag As Char
        Dim dsLastLoginDate As New DataSet

        Try
            '**********Checking whether user have rights of profile or not
            If Not objHelp.Proc_UserMst(Me.txtUserName.Value.Trim(), Me.ddlProfile.SelectedItem.Value.Trim(), OpType_BizNetWeb, ds_Check, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            strLoginName = ds_Check.Tables(0).Rows(0)("vLoginName")
            strUsername = txtUserName.Value
            If Password Is Nothing Or Password = String.Empty Then
                Password = Request.Form("password")
                strPassword = objHelp.EncryptPassword(Me.DecryptValue(Password))
            Else
                strPassword = objHelp.EncryptPassword(Password)
            End If
            Session.Add(S_Profiles, CType(Me.ViewState(VS_Profiles), DataTable))
            strIpAddress = Me.Request.Headers("HTTP_X_FORWARDED_FOR")
            If strIpAddress Is Nothing OrElse strIpAddress = String.Empty Then
                strIpAddress = Request.ServerVariables("REMOTE_ADDR")
            End If

            ds = objHelp.ValidateUser(strUsername, strPassword)
            If Not IsNothing(ds) Then
                dv = ds.Tables(0).DefaultView
                dv.RowFilter = "vUserTypeCode = '" + Me.ddlProfile.SelectedItem.Value.Trim() + "'"
                dt = dv.ToTable()
                dr = dt.Rows(0)

                Session.RemoveAll()
                Session.Add(S_Profiles, CType(Me.ViewState(VS_Profiles), DataTable))
                Session.Add(S_LoginName, dr.Item("vLoginName").ToString)
                Session.Add(S_UserID, dr.Item("iUserId").ToString)
                Session.Add(S_UserName, dr.Item("vUserName"))
                Session.Add(S_FirstName, dr.Item("vFirstName"))
                Session.Add(S_LastName, dr.Item("vLastName"))
                Session.Add(S_Password, dr.Item("vLoginPass"))
                Session.Add(S_UserType, dr.Item("vUserTypeCode"))
                Session.Add(S_DeptCode, dr.Item("vDeptCode"))
                Session.Add(S_ValidUser, "Yes")
                Session.Add(S_LocationCode, dr.Item("vLocationCode").ToString)
                Session.Add(S_TimeZoneName, dr.Item("vTimeZoneName").ToString)
                Session.Add(S_ScopeNo, dr.Item("nScopeNo").ToString)
                Session.Add(S_EDCUser, dr.Item("cIsEDCUser").ToString)
                Session.Add(S_IpAddress, strIpAddress.ToString)
                Session.Add(S_Login, "1")
                Session.Add(S_LastLoginDateTime, ds_Check.Tables(0).Rows(0)("LastLoggdInTime"))
                sGUID = System.Guid.NewGuid.ToString()
                Session.Add(S_GUID, sGUID.ToString)
                Session.Add(S_UserNameWithProfile, dr.Item("UserNameWithProfile").ToString)

                ScopeValue = "'" & Convert.ToString(dr.Item("vScopeValues")).Replace(",", "','") & "'"
                Session.Add(S_ScopeValue, ScopeValue)
                Session.Add(S_UserGroup, dr.Item("iUserGroupCode").ToString)
                Session.Add(S_WorkFlowStageId, dr.Item("iWorkFlowStageId").ToString)

                strTimeZone = objCommon.GetTimeZone(Me.Session(S_LocationCode), DateTime.Now())
                If strTimeZone <> "" Then
                    str = strTimeZone.Split(":")
                    offset = New TimeSpan(str(0), str(1), 0).TotalHours
                End If

                UTCHourDiff = objCommon.GetTimeZone(Me.Session(S_LocationCode), DateTime.Now())
                Session.Add(S_TimeOffSet, UTCHourDiff)
                Session.Add(S_Time, CDate(objCommon.GetCurDatetime(Me.Session(S_TimeZoneName).ToString)).ToString("dd-MMM-yyyy HH:mm") + " (" + Me.Session(S_TimeOffSet).ToString + " GMT)")

                If Me.Session(S_UserID) <> "1" Then ' SuperUser=1
                    ValidateLastLoginDaysDiff(InActiveFlag)
                    If InActiveFlag = "Y" Then
                        Exit Sub
                    End If
                End If

                If Not Me.ValidateSingleUser(strLoginErrorMsg) Then
                    Exit Sub
                End If

            End If
            Response.Cookies("UserSingleLogon").Value = System.DateTime.Now.ToString()
                Response.Cookies("UserSingleLogon").Expires = DateTime.Now.AddDays(1)
            '==============================================================================

            If ds_Check.Tables(0).Rows(0)("iUserGroupCode").ToString.Trim = "1" Then
                SuperUserId = Me.Session(S_UserID)

            End If
            Me.Response.Redirect("frmMainPage.aspx?usertype=" + Me.Session(S_UserType), False)


        Catch ex As System.Threading.ThreadAbortException
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            objCommon.ShowAlert(ex.ToString, Me.Page)
            Session.RemoveAll()

        End Try
    End Sub
#End Region

#Region "Button Events"

    Protected Sub btnMediator_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMediator.Click
        Dim dsProfile As New DataSet
        Dim dvProfile As DataView
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try
            wStr = "vUserName = '" + Me.txtUserName.Value.Trim() + "' And cStatusIndi <> 'D' Order by vUserTypeName"
            If Not Me.objHelp.GetViewUserMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            dsProfile, eStr) Then
                Throw New Exception(eStr)
            End If

            If dsProfile Is Nothing Then
                Throw New Exception("No Profile Found")
            End If

            dvProfile = dsProfile.Tables(0).DefaultView.ToTable(True, "vUserTypeCode,vUserTypeName".Split(",")).DefaultView

            Me.ddlProfile.DataSource = dvProfile.ToTable()
            Me.ddlProfile.DataTextField = "vUserTypeName"
            Me.ddlProfile.DataValueField = "vUserTypeCode"
            Me.ddlProfile.DataBind()

            Me.ViewState(VS_Profiles) = dvProfile.ToTable()

            'Me.hdnpassword.Focus()
        Catch ex As Exception
            Me.objCommon.ShowAlert("Database Not Connected.", Me.Page)
            Me.ShowErrorMessage("Database Not Connected. ", ex.Message)
        Finally
            If Not IsNothing(dsProfile) Then
                dsProfile.Dispose()
            End If
        End Try
    End Sub

    'Protected Sub ImgBtnLogin_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgBtnLogin.Click

    '    If Me.Session(S_UserID) Is Nothing Then
    '        Login()

    '        If Not GetSetProject() Then
    '            objCommon.ShowAlert("Error while GetSetproject...", Me.Page)
    '        End If

    '    Else
    '        objCommon.ShowAlert("You Are Not Allowed To Log In With The Same Profile In The Same Browser", Me.Page)
    '    End If

    'End Sub

    Private Sub ValidatePasswordPolicy()
        Dim dsPolicyMst As New DataSet
        Dim dsPasswordHistory As New DataSet
        Dim wStr_PasswordPolicyMst As String = String.Empty
        Dim dr As DataRow
        Dim ExpiryDays As Integer
        Dim wStr_PasswordHistory As String = "iUserId=" + Me.Session(S_UserID) + _
                                            " AND cStatusIndi <> 'C' ORDER BY iSrNo DESC "
        Dim IsPasswordExpired As Boolean = False
        Dim ExpiredMessage As String = String.Empty
        Dim TempDate As DateTime
        Dim TempSession As String
        Try
            wStr_PasswordPolicyMst = "cStatusIndi <> 'C'"
            If Not objHelp.GetPasswordPolicyMst(wStr_PasswordPolicyMst, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                dsPolicyMst, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            For Each dr In dsPolicyMst.Tables(0).Rows

                If Convert.ToString(dr("cActiveFlag")) = "Y" Then
                    If Convert.ToString(dr("vPolicyDesc")).Trim.ToLower = "SingleUser".ToLower AndAlso _
                            Convert.ToString(dr("vValue")) = "Y" Then

                    ElseIf Convert.ToString(dr("vPolicyDesc")).Trim.ToLower = "ExpiryDays".ToLower Then
                        If Integer.TryParse(Convert.ToString(dr("vValue")).Trim, ExpiryDays) Then
                            If Not objHelp.GetPasswordHistory(wStr_PasswordHistory, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                              dsPasswordHistory, eStr_Retu) Then
                                Throw New Exception(eStr_Retu)
                            End If
                            IsPasswordExpired = False
                            If dsPasswordHistory.Tables(0).Rows.Count <= 0 Then
                                ExpiredMessage = "Please change your Password !!!"
                                IsPasswordExpired = True
                            Else

                                If DateTime.TryParse(Convert.ToString(dsPasswordHistory.Tables(0).Rows(0)("dChangedDate")), TempDate) Then
                                    If DateTime.Today.Subtract(TempDate).TotalDays >= ExpiryDays Then

                                        ExpiredMessage = "Password Expired, Please change your password. !!!"
                                        IsPasswordExpired = True
                                    End If
                                End If
                            End If
                            If IsPasswordExpired Then
                                Response.Redirect("frmChangePwd.aspx?Expired=1&Msg=" + ExpiredMessage, True)
                                Exit Sub
                            End If
                        End If
                    End If
                End If
            Next

        Catch ex As System.Threading.ThreadAbortException
            TempSession = Session(S_UserID).ToString
            Session(S_UserID1) = TempSession
            Session.Remove(S_UserID)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            objCommon.ShowAlert(ex.Message, Me)
        End Try
    End Sub

    Private Sub ValidateLastLoginDaysDiff(Optional ByRef InActiveFlag As String = "N")
        Dim wstr As String = String.Empty
        Dim wStr_PasswordPolicyMst As String
        Dim dsLastLogin As New DataSet
        Dim dsPasswordPolicyMst As New DataSet
        Dim nDays As New Int16
        Dim sParameter As String = String.Empty
        Dim TempSession As String = String.Empty
        Try

            wStr_PasswordPolicyMst = "vPolicyDesc='Autolock_Inactiveuser' AND cActiveFlag='Y' AND cStatusIndi <> 'C'"
            dsPasswordPolicyMst = Nothing

            If Not objHelp.GetPasswordPolicyMst(wStr_PasswordPolicyMst, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                               dsPasswordPolicyMst, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            sParameter = Convert.ToString(Convert.ToInt32(Session(S_UserID).ToString))

            If Not objHelp.Proc_InActiveAccount(sParameter, dsLastLogin, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            If dsLastLogin Is Nothing Then
                Me.objCommon.ShowAlert("Parameter Autolock_Inactiveuser Details Not Found!", Me.Page)
                Exit Sub
            End If

            If Convert.ToInt32(dsLastLogin.Tables(0).Rows(0)("nDays")) > 0 Then
                If dsLastLogin.Tables(0).Rows(0)("nDays") > Convert.ToInt32(dsPasswordPolicyMst.Tables(0).Rows(0)("vValue")) Then

                    wstr = "Update UserMst Set cStatusIndi = 'D' Where iUserId =" & Convert.ToString(Convert.ToInt32(Session(S_UserID).ToString)) & ""
                    objHelp.GetResultSet(wstr, "UserMst")
                    Me.objCommon.ShowAlert("This User Profile Has been Made Inactive As It Was Not Used Since Last 90 Days, Please Contact Your Administrator !", Me.Page)
                    InActiveFlag = "Y"
                    Me.ddlProfile.Items.Clear()
                    Me.txtUserName.Value = ""
                    'Me.hdnpassword.Text = ""
                    Session.RemoveAll()
                    Exit Sub
                End If
            End If
            InActiveFlag = "N"
        Catch ex As System.Threading.ThreadAbortException
            TempSession = Session(S_UserID).ToString
            Session(S_UserID1) = TempSession
            Session.Remove(S_UserID)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            objCommon.ShowAlert(ex.Message, Me)
        End Try
    End Sub

    Private Function ValidateSingleUser(ByRef strLoginErrorMsg As String) As Boolean
        Dim userAgent As String
        ValidateSingleUser = True
        Dim dsUserLoginDetails As New DataSet
        Dim wStr_UserLoginDetails As String = String.Empty
        Dim dtUserLoginDetails As New DataTable
        Dim strUserLoginMessage As String = String.Empty
        Dim wStr_PasswordPolicyMst As String = String.Empty
        Dim dsPasswordPolicyMst As New DataSet
        Dim MaxLoginMins As Double = 0

        Dim dsUserLoginHistory As New DataSet
        Dim dtUserLoginHistory As New DataTable

        userAgent = Request.UserAgent

        If userAgent.IndexOf("MSIE") > -1 Then
            userAgent = "MSIE"
        ElseIf userAgent.IndexOf("Firefox/") > -1 Then
            userAgent = "Firefox"
        ElseIf userAgent.IndexOf("Chrome/") > -1 Then
            userAgent = "Chrome"
        ElseIf userAgent.Contains("CriOS/") Then
            userAgent = "Chrome"
        ElseIf userAgent.Contains("Safari/") Then
            userAgent = "Safari"
        Else
            userAgent = "Other"
        End If

        Try
            wStr_UserLoginDetails = "vUserName='" & Session(S_UserName) & "'"
            If Not objHelp.View_Userlogindetails(wStr_UserLoginDetails, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                               dsUserLoginDetails, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            dtUserLoginDetails = dsUserLoginDetails.Tables(0)
            If dtUserLoginDetails.Rows.Count > 0 Then

                strUserLoginMessage = "confirmLoginAgain("
                strUserLoginMessage += "'" + dtUserLoginDetails.Rows(0)("vIPAddress").ToString.Trim() + "',"
                strUserLoginMessage += "'" + dtUserLoginDetails.Rows(0)("vUserAgent").ToString.Trim() + "',"
                strUserLoginMessage += "'" + strIpAddress + "',"
                strUserLoginMessage += "'" + userAgent + "',"
                strUserLoginMessage += "'" + txtUserName.Value + "'"
                strUserLoginMessage += ");"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "loginAgain", strUserLoginMessage, True)
                ValidateSingleUser = False
            Else
                If Not objCommon.InsertUserLoginDetails(Me.Session(S_UserID), strIpAddress, UTCHourDiff, userAgent, eStr_Retu, sGUID, GeneralModule.OpType_BizNetWeb) Then
                    Throw New Exception(eStr_Retu)
                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...ValidateSingleUser")
            Throw ex
            ValidateSingleUser = False
        End Try
    End Function

    Private Function AssignValueForLoginHistory(ByRef dtUserLoginHistory As DataTable) As Boolean
        AssignValueForLoginHistory = True
        Dim dsUserLoginHistory As New DataSet
        Dim wStr_UserLoginHistory As String = String.Empty
        Dim dr As DataRow
        Dim strServerDateTime As String = String.Empty
        Dim dsUserLoginDetails As New DataSet
        Dim wStr_UserLoginDetails = "iUserID=" + Me.Session(S_UserID)
        Try
            If Not objHelp.getUserLoginHistory(wStr_UserLoginHistory, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                                 dsUserLoginHistory, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            If Not objHelp.GetUserLoginDetails(wStr_UserLoginDetails, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                               dsUserLoginDetails, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            strServerDateTime = objHelp.GetServerDateTime()

            dtUserLoginHistory = New DataTable
            dtUserLoginHistory = dsUserLoginHistory.Tables(0).Clone()
            dtUserLoginHistory.TableName = dsUserLoginHistory.Tables(0).TableName
            dtUserLoginHistory.AcceptChanges()

            dr = dtUserLoginHistory.NewRow
            dr("iUserID") = Me.Session(S_UserID)
            dr("cLOFlag") = "O"
            If dsUserLoginDetails.Tables(0).Rows.Count > 0 Then
                dr("dInOutDateTime") = Convert.ToString(dsUserLoginDetails.Tables(0).Rows(0)("dLastActivityDate"))
            Else
                dr("dInOutDateTime") = strServerDateTime
            End If

            dr("iModifyBy") = Me.Session(S_UserID)
            dr("cStatusIndi") = "N"
            dr("vIpAddress") = strIpAddress
            dr("vUserAgent") = userAgent  ''Added by Rahul Shah
            dtUserLoginHistory.Rows.Add(dr)
            dtUserLoginHistory.AcceptChanges()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignValueForLoginHistory")
            AssignValueForLoginHistory = False
        End Try
    End Function

    Private Function ActiveLoginFailedAttempt() As Boolean
        Dim Ds_PasswordPolicy As New DataSet
        Dim wstr As String = String.Empty

        Try

            wstr = "vPolicyDesc in ('NoOfAttempt','LockOutTime') and cActiveFlag = 'Y' and cStatusIndi <> 'D'"

            If Not Me.objHelp.GetPasswordPolicyMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                   Ds_PasswordPolicy, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            If Ds_PasswordPolicy.Tables(0).Rows.Count <> 2 Then
                Return False
            End If

            If Not Double.TryParse(Ds_PasswordPolicy.Tables(0).Rows(0)("vValue"), NoOfAttempt) Then
                Return False
            End If

            If Not Double.TryParse(Ds_PasswordPolicy.Tables(0).Rows(1)("vValue"), LockOutTime) Then
                Return False
            End If

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....ActiveLoginFailedAttempt")
            Return False
        End Try
    End Function

    Private Function ValidateBlocked() As Boolean
        Dim wstr As String = String.Empty
        Dim LatestLoginTime As DateTime
        Dim dv_UserLoginFailure As New DataView
        Try

            wstr = "upper(vUserName) = '" + strLoginName + "' and cstatusindi <> 'D' Order by nSrNo desc"

            If Not Me.objHelp.GetUserLoginFailureDetails(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                         Ds_UserLoginFailure, eStr_Retu) Then
                Throw New Exception
            End If


            Ds_UserLoginFailure.Tables(0).TableName = "UserLoginFailureDetails"
            For Each dr_loginfailure As DataRow In Ds_UserLoginFailure.Tables(0).Rows
                If Convert.ToString(dr_loginfailure("cBlockedFlag")) = "B" Then
                    If DateTime.TryParse(Convert.ToString(dr_loginfailure("dLastFailedLogin")), LatestLoginTime) Then
                        'If DateTime.Now.Subtract(LatestLoginTime).TotalMinutes < LockOutTime Then
                        Me.objCommon.ShowAlert("Your username is locked due to multiple incorrect login attempts. Please contact your Administrator!", Me)
                        Return False
                        'End If
                    End If
                End If
                Exit For
            Next dr_loginfailure

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....ValidateBlocked")
            Return False
        End Try
    End Function

    Private Function AssingLoginFailureDetails() As Boolean
        Dim LatestLoginTime As DateTime
        Dim dr_LoginFaliure As DataRow
        Dim IsBlock As Boolean = False
        Dim index As Integer = 0
        Dim dt As DataTable
        Dim UserId As Integer
        Dim AttempCount As Integer = 0 'Added by Bhargav Thaker 13Mar2023

        dt = CType(Me.ViewState(VS_Profiles), DataTable)
        dt.DefaultView.RowFilter = "vUserTypeName=" + "'" + ddlProfile.SelectedItem.ToString().Trim() + "'"
        UserId = CType(dt.DefaultView.ToTable.Rows(0)(0), Integer)

        Try
            If Not Ds_UserLoginFailure.Tables.Contains("UserLoginFailureDetails") Then
                Return False
            End If

            For Each dr_failure As DataRow In Ds_UserLoginFailure.Tables(0).Rows
                If Convert.ToString(dr_failure("cBlockedFlag")).ToUpper().Trim() <> "B" Then
                    If DateTime.TryParse(Convert.ToString(dr_failure("dLastFailedLogin")), LatestLoginTime) Then
                        If DateTime.Now.Subtract(LatestLoginTime).TotalMinutes > LockOutTime Then
                            dr_failure("dLastFailedLogin") = DateTime.Now()
                            dr_failure("nAttemptCount") = 1
                            AttempCount = Convert.ToInt32(dr_failure("nAttemptCount")) 'Added by Bhargav Thaker 13Mar2023
                        ElseIf DateTime.Now.Subtract(LatestLoginTime).TotalMinutes < LockOutTime Then
                            dr_failure("dLastFailedLogin") = DateTime.Now()
                            dr_failure("nAttemptCount") += 1
                            AttempCount = Convert.ToInt32(dr_failure("nAttemptCount")) 'Added by Bhargav Thaker 13Mar2023
                            If dr_failure("nAttemptCount") > NoOfAttempt Then
                                dr_failure("cBlockedFlag") = "B"
                                IsBlock = True
                            End If
                        End If
                    End If
                    dr_failure("dModifyOn") = DateTime.Now()
                    dr_failure("cStatusIndi") = "N"
                    dr_failure("vRemarks") = "User Locked by System"    '' Added by Rahul 
                    dr_failure("iUserId") = UserId
                    dr_failure("iModifyBy") = 0
                    Ds_UserLoginFailure.Tables(0).AcceptChanges()
                    Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

                    AttempCount = NoOfAttempt - AttempCount 'Added by Bhargav Thaker 13ar2023
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "spnRemainAttemp", "spnRemainAttemp(" + Convert.ToString(AttempCount) + ");", True) 'Added by Bhargav Thaker 13ar2023
                    Exit For
                End If
            Next dr_failure

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                Ds_UserLoginFailure.Tables(0).Clear()
                dr_LoginFaliure = Ds_UserLoginFailure.Tables(0).NewRow()
                dr_LoginFaliure("vUserName") = strLoginName
                dr_LoginFaliure("dLastFailedLogin") = DateTime.Now()
                dr_LoginFaliure("nAttemptCount") = 1
                dr_LoginFaliure("cBlockedFlag") = "N"
                dr_LoginFaliure("vIPAddress") = strIpAddress
                dr_LoginFaliure("dModifyOn") = DateTime.Now()
                dr_LoginFaliure("cStatusIndi") = "N"
                dr_LoginFaliure("vRemarks") = "Locked by System"
                dr_LoginFaliure("iUserId") = UserId
                dr_LoginFaliure("iModifyBy") = 0
                Ds_UserLoginFailure.Tables(0).Rows.Add(dr_LoginFaliure)
                Ds_UserLoginFailure.AcceptChanges()
                Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            End If

            If Ds_UserLoginFailure.Tables(0).Rows.Count > 1 Then
                For index = 0 To Ds_UserLoginFailure.Tables(0).Rows.Count - 1
                    If index > 0 Then
                        If index > Ds_UserLoginFailure.Tables(0).Rows.Count - 1 Then
                            Exit For
                        End If
                        Ds_UserLoginFailure.Tables(0).Rows(index).Delete()
                        Ds_UserLoginFailure.AcceptChanges()
                        index = index - 1
                    End If
                Next index
            End If

            If Not objLambda.Save_UserLoginFailureDetails(Choice, Ds_UserLoginFailure, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            If IsBlock Then
                BlockMsg = "Your Username Is Locked Due To Multiple Incorrect Login Attempts !!!"
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssingLoginFailureDetails")
            Return False
        End Try
    End Function

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Try
            Session.RemoveAll()  ''Added by Rahul Shah
            If Me.Session(S_UserID) Is Nothing Then
                Login()

                If Not GetSetProject() Then
                    objCommon.ShowAlert("Error while GetSetproject...", Me.Page)
                End If

            Else
                objCommon.ShowAlert("You Are Not Allowed To Log In With The Same Profile In The Same Browser", Me.Page)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnLoginAgain_Click(sender As Object, e As EventArgs)
        Dim strRedirect As String = String.Empty
        Session.Add(S_Login, "2")
        strRedirect = "Logoutpage.aspx?username=" + Me.Session(S_UserName) + "&usertype=" + Me.Session(S_UserType)
        Me.Response.Redirect(strRedirect, False)
    End Sub

#End Region

#Region "Error Handler"
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        objCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub
#End Region

#Region "Fill Profile"

    Private Sub FillProfile()
        Dim dsProfile As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try
            wStr = "cStatusIndi <> 'D' Order by vUserTypeName"
            If Not Me.objHelp.getUserTypeMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            dsProfile, eStr) Then
                Throw New Exception(eStr)
            End If

            If dsProfile Is Nothing Then
                Throw New Exception("No Profile Found")
            End If

            Me.ddlProfile.DataSource = dsProfile.Tables(0)
            Me.ddlProfile.DataTextField = "vUserTypeName"
            Me.ddlProfile.DataValueField = "vUserTypeCode"
            Me.ddlProfile.DataBind()

            Me.ViewState(VS_Profiles) = dsProfile.Tables(0).Copy()

        Catch ex As Exception
            Me.objCommon.ShowAlert("Database Not Connected.", Me.Page)
            Me.ShowErrorMessage("Database Not Connected. ", ex.Message)
        Finally
            If Not IsNothing(dsProfile) Then
                dsProfile.Dispose()
            End If
        End Try

    End Sub

#End Region

#Region "Login"

    Private Sub Login()
        Dim ds As New DataSet
        Dim dv As New DataView
        Dim dt As New DataTable
        Dim dr As Data.DataRow
        Dim ScopeValue As String = String.Empty
        Dim strLoginErrorMsg As String = String.Empty
        Dim ds_Check As New DataSet
        Dim wStr As String = String.Empty
        Dim Authorized As Boolean = False
        Dim strTimeZone As String = String.Empty
        Dim str() As String
        Dim InActiveFlag As Char
        Dim dsLastLoginDate As New DataSet

        Try
            '**********Checking whether user have rights of profile or not
            If Not objHelp.Proc_UserMst(Me.txtUserName.Value.Trim(), Me.ddlProfile.SelectedItem.Value.Trim(), OpType_BizNetWeb, ds_Check, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            If Not IsNothing(ds_Check) Then
                If Not ds_Check.Tables(0).Rows.Count > 0 Then
                    Me.objCommon.ShowAlert("You Do Not Have Rights Of Selected Profile", Me.Page)
                    Exit Sub
                End If

            End If

            If Not ds_Check.Tables(0).Rows(0).Item("dFromDate") = "" Then
                If Not ds_Check.Tables(0).Rows(0).Item("dFromDate") = "Jan  1 1900 12:00AM" Then
                    If Not (ds_Check.Tables(0).Rows(0).Item("dFromDate") < CType(objCommon.GetCurDatetime(ds_Check.Tables(0).Rows(0).Item("vTimeZoneName").ToString), DateTime) AndAlso CType(objCommon.GetCurDatetime(ds_Check.Tables(0).Rows(0).Item("vTimeZoneName").ToString), DateTime) < ds_Check.Tables(0).Rows(0).Item("dToDate")) Then
                        objCommon.ShowAlert("You Are Not Authorised User,Please Contact Your Administrator", Me.Page)
                        Exit Sub
                    End If
                End If
            End If

            strLoginName = ds_Check.Tables(0).Rows(0)("vLoginName")
            strUsername = txtUserName.Value
            If Password Is Nothing Or Password = String.Empty Then
                Password = Request.Form("password")
                strPassword = objHelp.EncryptPassword(Me.DecryptValue(Password))
            Else
                strPassword = objHelp.EncryptPassword(Password)
            End If
            Session.Add(S_Profiles, CType(Me.ViewState(VS_Profiles), DataTable))
            strIpAddress = Me.Request.Headers("HTTP_X_FORWARDED_FOR")
            If strIpAddress Is Nothing OrElse strIpAddress = String.Empty Then
                strIpAddress = Request.ServerVariables("REMOTE_ADDR")
            End If

            If Me.ActiveLoginFailedAttempt() Then
                If Not Me.ValidateBlocked() Then
                    Me.txtUserName.Value = ""
                    'Me.hdnpassword.Text = ""
                    Me.ddlProfile.Items.Clear()
                    Session.RemoveAll()
                    Exit Sub
                End If
            End If

            ds = objHelp.ValidateUser(strUsername, strPassword)
            If Not IsNothing(ds) Then
                dv = ds.Tables(0).DefaultView
                dv.RowFilter = "vUserTypeCode = '" + Me.ddlProfile.SelectedItem.Value.Trim() + "'"
                dt = dv.ToTable()

                If dt.Rows.Count <= 0 Then
                    Me.AssingLoginFailureDetails()
                    If BlockMsg <> "" Then
                        Me.objCommon.ShowAlert(BlockMsg, Me)
                        Exit Sub
                    End If
                    'objCommon.ShowAlert("Login Failed. Try Again", Me.Page)
                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "SpanLoginFailedMessage", "SpanLoginFailedMessage();", True)
                    Me.txtUserName.Value = ""
                    ' Me.hdnpassword.Text = ""
                    Me.ddlProfile.Items.Clear()
                    Session.RemoveAll()
                    Exit Sub
                End If

                If ds.Tables(0).Rows(0)("cStatusIndi").ToString.Trim.ToUpper() = "C" Then
                    objCommon.ShowAlert("User Inactive, Please Contact Your Administrator !!!", Me.Page)
                    Exit Sub
                End If
                dr = dt.Rows(0)

                Session.RemoveAll()
                Session.Add(S_Profiles, CType(Me.ViewState(VS_Profiles), DataTable))
                Session.Add(S_LoginName, dr.Item("vLoginName").ToString)
                Session.Add(S_UserID, dr.Item("iUserId").ToString)
                Session.Add(S_UserName, dr.Item("vUserName"))
                Session.Add(S_FirstName, dr.Item("vFirstName"))
                Session.Add(S_LastName, dr.Item("vLastName"))
                Session.Add(S_Password, dr.Item("vLoginPass"))
                Session.Add(S_UserType, dr.Item("vUserTypeCode"))
                Session.Add(S_DeptCode, dr.Item("vDeptCode"))
                Session.Add(S_ValidUser, "Yes")
                Session.Add(S_LocationCode, dr.Item("vLocationCode").ToString)
                Session.Add(S_TimeZoneName, dr.Item("vTimeZoneName").ToString)
                Session.Add(S_ScopeNo, dr.Item("nScopeNo").ToString)
                Session.Add(S_EDCUser, dr.Item("cIsEDCUser").ToString)
                Session.Add(S_IpAddress, strIpAddress.ToString)
                Session.Add(S_Login, "1")
                Session.Add(S_LastLoginDateTime, ds_Check.Tables(0).Rows(0)("LastLoggdInTime"))
                sGUID = System.Guid.NewGuid.ToString()
                Session.Add(S_GUID, sGUID.ToString)
                Session.Add(S_UserNameWithProfile, dr.Item("UserNameWithProfile").ToString)

                ScopeValue = "'" & Convert.ToString(dr.Item("vScopeValues")).Replace(",", "','") & "'"
                Session.Add(S_ScopeValue, ScopeValue)
                Session.Add(S_UserGroup, dr.Item("iUserGroupCode").ToString)
                Session.Add(S_WorkFlowStageId, dr.Item("iWorkFlowStageId").ToString)

                strTimeZone = objCommon.GetTimeZone(Me.Session(S_LocationCode), DateTime.Now())
                If strTimeZone <> "" Then
                    str = strTimeZone.Split(":")
                    offset = New TimeSpan(str(0), str(1), 0).TotalHours
                End If

                UTCHourDiff = objCommon.GetTimeZone(Me.Session(S_LocationCode), DateTime.Now())
                Session.Add(S_TimeOffSet, UTCHourDiff)
                Session.Add(S_Time, CDate(objCommon.GetCurDatetime(Me.Session(S_TimeZoneName).ToString)).ToString("dd-MMM-yyyy HH:mm") + " (" + Me.Session(S_TimeOffSet).ToString + " GMT)")

                If Me.Session(S_UserID) <> "1" Then ' SuperUser=1
                    ValidateLastLoginDaysDiff(InActiveFlag)
                    If InActiveFlag = "Y" Then
                        Exit Sub
                    End If
                End If

                If Not Me.ValidateSingleUser(strLoginErrorMsg) Then
                    Exit Sub
                End If

                If Me.Session(S_UserID) <> "1" Then ' SuperUser=1
                    Me.ValidatePasswordPolicy()
                End If

            End If

            strPassword = objHelp.DecryptPassword(strPassword)
            If strUsername.Trim.ToUpper() = strPassword.Trim.ToUpper() Then
                Dim q As String = String.Empty
                q = "frmChangePwd.aspx?vUserName=" + Me.Session(S_UserName).ToString() + "&Mode=RstPwd" + "&Reset=N" + "&vUserTypeCode=" + Me.Session(S_UserType).ToString()
                Me.Response.Redirect(q, True)
                Exit Sub
            End If

            If ds_Check.Tables(0).Rows(0)("isMFA").ToString.Trim.ToUpper() = "Y" AndAlso (ds_Check.Tables(0).Rows(0)("isMFAEmail").ToString.Trim.ToUpper() = "Y" OrElse ds_Check.Tables(0).Rows(0)("isMFASMS").ToString.Trim.ToUpper() = "Y") Then
                GetOtp()
                'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "timer", "timer(120);", True)
            Else
                ''===========Creating a cookie on client side to moniter client's  logon state(15-Sept-2011)
                Response.Cookies("UserSingleLogon").Value = System.DateTime.Now.ToString()
                Response.Cookies("UserSingleLogon").Expires = DateTime.Now.AddDays(1)
                '==============================================================================

                If ds_Check.Tables(0).Rows(0)("iUserGroupCode").ToString.Trim = "1" Then
                    SuperUserId = Me.Session(S_UserID)

                End If
                Dim ds_DownTime As DataSet = Nothing
                Dim dv_DownTime As DataView
                Dim dt_DownTime As DataTable = Nothing

                If Not ConfigurationManager.AppSettings("UnderMaintanance") Is Nothing Then
                    If ConfigurationManager.AppSettings("UnderMaintanance").ToString = "Y" Then
                        ds_DownTime = objHelp.ProcedureExecute("dbo.Proc_DownTimeMaster", Session(S_UserID))
                        dv_DownTime = ds_DownTime.Tables(0).DefaultView()
                        dv_DownTime.RowFilter = "iUserId = " + Session(S_UserID)
                        dt_DownTime = dv_DownTime.ToTable()
                    End If
                End If
                If Not ds_DownTime Is Nothing AndAlso ds_DownTime.Tables(0).Rows.Count > 0 Then
                    If Not dt_DownTime Is Nothing AndAlso dt_DownTime.Rows.Count > 0 Then
                        Me.Response.Redirect("frmMainPage.aspx?usertype=" + Me.Session(S_UserType), False)
                    Else
                        Me.Response.Redirect("logoutPage.aspx", False)
                        Me.Response.Redirect("frmUnderMaintanance.aspx", False)
                        Session.RemoveAll()
                    End If
                Else
                    Me.Response.Redirect("frmMainPage.aspx?usertype=" + Me.Session(S_UserType), False)
                End If

            End If

        Catch ex As System.Threading.ThreadAbortException
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            objCommon.ShowAlert(ex.ToString, Me.Page)
            Session.RemoveAll()
        End Try
    End Sub

#End Region



#Region "SetProjectMatrix" '' Added By Dipen Shah To set Latest Project To the respective User.
    Public Function GetSetProject() As Boolean

        Dim wStr As String = String.Empty
        Dim ParaMeters As String = String.Empty
        Dim Ds As New DataSet

        Try
            ParaMeters = Me.Session(S_UserID)
            If Not objHelp.Proc_SetProjectMatrix(ParaMeters, Ds, eStr_Retu) Then
                objCommon.ShowAlert("Error While Proc_SetProjectMatrix", Me.Page)
            End If
            If Ds.Tables.Count <> 0 AndAlso Ds.Tables(0).Rows.Count > 0 AndAlso Ds.Tables(0).Rows(0)("WorkSpaceId") <> "" AndAlso Ds.Tables(0).Rows(0)("ProjectNo") <> "" Then
                Session.Add(S_ProjectName, Ds.Tables(0).Rows(0)("ProjectNo"))
                Session.Add(S_ProjectId, Ds.Tables(0).Rows(0)("WorkSpaceId"))
            End If
            Return True
        Catch ex As Exception
            objCommon.ShowAlert("Eroor While getSetProject....", Me.Page)
            Return False
        End Try

    End Function


#End Region

#Region "CheckUserScope"
    <Web.Services.WebMethod()>
    Public Shared Function CheckUserScope(ByVal UserName As String, ByVal Profile As String) As String
        Dim clsCommon As New clsCommon
        Dim objLambda As WS_Lambda.WS_Lambda = clsCommon.GetHelpDbLambdaRef()
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = clsCommon.GetHelpDbTableRef()

        Dim ReturnValue As String = String.Empty
        Dim Ds As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Try
            wStr = "vUserTypeCode='" + Profile + "' and vUserName='" + UserName + "' And cStatusindi <> 'D'"

            If Not objHelp.GetData("UserMst", "nScopeNo,vUserTypeCode,vUserName", wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds, eStr) Then
                Throw New Exception(eStr + "Error While getData()...")
            End If

            ReturnValue = "Success-" + Ds.Tables(0).Rows(0)("nScopeNo").ToString().Trim()
        Catch ex As Exception
            Throw New Exception(ex.Message + "error while checkuserScope()")
        End Try
        Return ReturnValue
    End Function
    <Web.Services.WebMethod()>
    Public Shared Function SetPageLoad() As String
        Dim clsCommon As New clsCommon
        Dim objHelpDB As WS_HelpDbTable.WS_HelpDbTable = clsCommon.GetHelpDbTableRef()
        Dim ds_ParameterList As New DataSet
        Dim ReturnVal As String = String.Empty
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Try
            wStr = "vParameterName = 'cAllowSMS' And cActiveFlag = 'Y' And cStatusIndi <> 'D'"

            If Not objHelpDB.GetParameterList(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                                ds_ParameterList, eStr) Then
                Throw New Exception(eStr)
            End If

            ReturnVal = Convert.ToString(ds_ParameterList.Tables(0).Rows(0)("vParameterValue"))

        Catch ex As Exception
            Throw New Exception("Error while SetPageLoad()")
        End Try
        Return ReturnVal
    End Function

#End Region

#Region "Change Password"
    Protected Sub ForgotPassword(sender As Object, e As EventArgs)
        Dim confirmValue As String = Request.Form("confirm_value")
        If confirmValue = "Yes" Then
            ResetPassword()
        Else
            ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('Enter Password To Login!')", True)
        End If

    End Sub

    Public Sub ResetPassword()
        Dim ds_Check As New DataSet
        Dim ds_SMSEmail As New DataSet
        Dim Mobile_No As String = String.Empty
        Dim EmailId As String = String.Empty
        Dim user_name As String = String.Empty
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim SMSURL As String = String.Empty
        Dim SMSUser As String = String.Empty
        Dim SMSPassw As String = String.Empty
        Dim SMSSender As String = String.Empty
        Dim OTPCode As String = String.Empty
        Dim OTPMessage As String = String.Empty
        Dim LabelMessage As String = String.Empty
        Dim SMSSendingURL As String = String.Empty

        Try
            Dim EnteredUserName = txtUserName.Value
            wStr = "vUserName='" + txtUserName.Value + "' And vUserTypeCode='" + ddlProfile.SelectedItem.Value.Trim() + "' And cStatusIndi <> 'D'"

            If Not Me.objHelp.GetViewUserMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_Check, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            Mobile_No = Convert.ToString(ds_Check.Tables(0).Rows(0)("vPhoneNo"))
            EmailId = Convert.ToString(ds_Check.Tables(0).Rows(0)("vEmailId"))
            user_name = ds_Check.Tables(0)(0)(3).ToString
            If Mobile_No = "" AndAlso EmailId = "" Then
                objCommon.ShowAlert("Mobile No and Email Information not found. Please contact System Administrator.", Page)
                Exit Sub
            End If

            If (EnteredUserName.ToUpper = user_name.ToUpper) Then
                Dim numbers As String = "1234567890"

                Dim characters As String = numbers
                characters += numbers

                Dim length As Integer = 6
                Dim otp As String = String.Empty
                For i As Integer = 0 To length - 1
                    Dim character As String = String.Empty
                    Do
                        Dim index As Integer = New Random().Next(0, characters.Length)
                        character = characters.ToCharArray()(index).ToString()
                    Loop While otp.IndexOf(character) <> -1
                    otp += character
                Next
                OTPCode = otp
                OTPGenerated.Value = OTPCode
                OTPMessage = "Dear " + Me.txtUserName.Value.Trim() + ",  " + OTPCode + " is your OTP to reset password of DiSoft Sarjen Systems Pvt Ltd https://www.sarjen.com"

                wStr = "vSMSLocationCode='" + Convert.ToString(ds_Check.Tables(0).Rows(0)("vLocationCode")) + "' And cStatusIndi <> 'D'"

                If Not Me.objHelp.GetSMSGateWayDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                    ds_SMSEmail, eStr_Retu) Then
                    Throw New Exception(eStr_Retu)
                End If

                If ds_SMSEmail Is Nothing OrElse ds_SMSEmail.Tables.Count <= 0 OrElse ds_SMSEmail.Tables(0).Rows.Count <= 0 Then
                    objCommon.ShowAlert("Email Information not found. Please contact System Administrator.", Page)
                    Exit Sub
                End If

                If Session("UserName") <> Nothing Or Session("UserProfile") <> Nothing Or Session("IUserId") <> Nothing Then
                    Session("UserName") = Nothing
                    Session("UserProfile") = Nothing
                    Session("IUserId") = Nothing
                End If
                Session("UserName") = txtUserName.Value
                Session("UserProfile") = ddlProfile.SelectedItem.Text.Trim()
                Session("IUserId") = Convert.ToInt32(ds_Check.Tables(0)(0)(0))

                LabelMessage = "You Will Get an OTP On"
                If Mobile_No <> "" Then
                    SMSDetails(OTPCode, ds_SMSEmail.Tables(0), Mobile_No, Convert.ToInt32(ds_Check.Tables(0)(0)(0)))
                    LabelMessage += " Your Registered Mobile Number(******" + Mobile_No.Substring(Mobile_No.Length - 4) + ")"
                End If
                If EmailId <> "" Then
                    EmailDetails(OTPCode, EmailId, Convert.ToInt32(ds_Check.Tables(0)(0)(0)), ds_SMSEmail.Tables(0))
                    LabelMessage += " Your Registered mail Id(******" + EmailId.Substring(EmailId.Length - 6) + ")"
                End If

                'Dim message As String = "You Will Get an OTP On Your Registerd Mobile Number : +91******" + Mobile_No.Substring(Mobile_No.Length - 4) & "\n"
                Dim script As String = "<script type='text/javascript' defer='defer'> alert('" + LabelMessage + "');</script>"
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "AlertBox", script)
                ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "OTPPrompt();", True)
            Else
                objCommon.ShowAlert("Invalid User Name!", Me)
                Exit Sub
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

    Public Sub GetOtp()
        Dim ds_Check1 As New DataSet
        Dim ds_Check As New DataSet
        Dim ds_SMSEmail As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim SMSURL As String = String.Empty
        Dim SMSUser As String = String.Empty
        Dim SMSPassw As String = String.Empty
        Dim SMSSender As String = String.Empty

        Try
            Dim EnteredUserName = txtUserName.Value
            wStr = "vUserName='" + txtUserName.Value + "' And vUserTypeCode='" + ddlProfile.SelectedItem.Value.Trim() + "' And cStatusIndi <> 'D'"

            If Not objHelp.Proc_UserMst(Me.txtUserName.Value.Trim(), Me.ddlProfile.SelectedItem.Value.Trim(), OpType_BizNetWeb, ds_Check1, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            If Not Me.objHelp.GetViewUserMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                ds_Check, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            Dim user_name = ds_Check.Tables(0)(0)(3).ToString

            If (EnteredUserName.ToUpper = user_name.ToUpper) Then
                ' Dim alphabets As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
                ' Dim small_alphabets As String = "abcdefghijklmnopqrstuvwxyz"
                Dim numbers As String = "1234567890"

                Dim characters As String = numbers
                characters += numbers

                Dim length As Integer = 6
                Dim otp As String = String.Empty
                For i As Integer = 0 To length - 1
                    Dim character As String = String.Empty
                    Do
                        Dim index As Integer = New Random().Next(0, characters.Length)
                        character = characters.ToCharArray()(index).ToString()
                    Loop While otp.IndexOf(character) <> -1
                    otp += character
                Next
                Dim OTPCode As String = otp
                OTPGenerated.Value = OTPCode

                Dim Mobile_No As String = Convert.ToString(ds_Check.Tables(0).Rows(0)("vPhoneNo"))
                Dim vEmailId As String = Convert.ToString(ds_Check.Tables(0).Rows(0)("vEmailId"))
                Dim vDeptName As String = Convert.ToString(ds_Check.Tables(0).Rows(0)("vDeptName"))

                wStr = "vSMSLocationCode='" + Me.Session(S_LocationCode) + "' And cStatusIndi <> 'D'"

                If Not Me.objHelp.GetSMSGateWayDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                        ds_SMSEmail, eStr_Retu) Then
                    Throw New Exception(eStr_Retu)
                End If

                If ds_SMSEmail Is Nothing OrElse ds_SMSEmail.Tables.Count <= 0 OrElse ds_SMSEmail.Tables(0).Rows.Count <= 0 Then
                    objCommon.ShowAlert("Email Information not found. Please contact System Administrator.", Page)
                    Exit Sub
                End If
                Me.ViewState(VS_SMSGateWay) = ds_SMSEmail.Tables(0).Copy()
                'ViewState(VS_SMSInfo) = ds_SMS

                If ds_Check1.Tables(0).Rows(0)("isMFASms").ToString.Trim.ToUpper() = "Y" Then
                    SMSDetails(OTPCode, ds_SMSEmail.Tables(0), Mobile_No, Convert.ToInt32(ds_Check.Tables(0)(0)(0)))
                    lblOtpmsg.Visible = True
                    lblOtpSmsMsg.Visible = True
                    lblOtpSmsMsg.Text = "SMS : " + Mobile_No.Substring(0, 2) + "*******" + Mobile_No.Substring(8)
                Else
                    lblOtpSmsMsg.Visible = False
                End If

                If Session("UserName") <> Nothing Or Session("UserProfile") <> Nothing Or Session("IUserId") <> Nothing Then
                    Session("UserName") = Nothing
                    Session("UserProfile") = Nothing
                    Session("IUserId") = Nothing

                End If
                Session("UserName") = txtUserName.Value
                Session("UserProfile") = ddlProfile.SelectedItem.Text.Trim()
                Session("IUserId") = Convert.ToInt32(ds_Check.Tables(0)(0)(0))

                If ds_Check1.Tables(0).Rows(0)("isMFAEmail").ToString.Trim.ToUpper() = "Y" AndAlso vEmailId <> "" Then
                    EmailDetails(OTPCode, vEmailId, Convert.ToInt32(ds_Check.Tables(0)(0)(0)), ds_SMSEmail.Tables(0))
                    Dim user_Email = vEmailId.ToString()
                    Dim Email As String
                    Email = user_Email.Substring(0, 2)
                    Dim LastEmail As String() = user_Email.Split("@".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                    lblOtpmsg.Visible = True
                    lblOtpEmailMsg.Visible = True
                    lblOtpEmailMsg.Text = "Email : " + Email + "********" + "@" + LastEmail(1)
                Else
                    lblOtpEmailMsg.Visible = False
                End If

                Dim ds_ChOTPCount As New DataSet
                If Not InsertOtpINfo(Me.Session(S_UserID), OTPCode, eStr_Retu) Then
                    objCommon.ShowAlert("Error while saving OTPInfo.", Me.Page)
                Else
                    ImgBtnLogin.Visible = False
                    ImgBtnReLogin.Visible = True
                    dvotp.Visible = True
                    Me.NamePass = Password
                End If
            Else
                objCommon.ShowAlert("Invalid User Name!", Me)
                Exit Sub
            End If
        Catch ex As Exception
            objCommon.ShowAlert(ex.Message, Me)
        End Try
    End Sub

    Public Sub SuccessLogin()
        Dim ds_Check As New DataSet
        Dim bExpiryDays As Boolean = False
        Dim userType As String = String.Empty
        Dim strUserLoginMessage As String
        Dim ds As New DataSet
        Dim dv As New DataView
        Dim dt As New DataTable
        Dim dr As Data.DataRow

        If Not objHelp.Proc_UserMst(Me.txtUserName.Value.Trim(), Me.ddlProfile.SelectedItem.Value.Trim(), OpType_BizNetWeb, ds_Check, eStr_Retu) Then
            Throw New Exception(eStr_Retu)
        End If

        If Not IsNothing(ds_Check) Then
            If Not ds_Check.Tables(0).Rows.Count > 0 Then
                Me.objCommon.ShowAlert("You do not have rights of selected profile", Me.Page)
                Exit Sub
            End If

        End If

        If Not ds_Check.Tables(0).Rows(0).Item("dFromDate") = "" Then
            If Not ds_Check.Tables(0).Rows(0).Item("dFromDate") = "Jan  1 1900 12:00AM" Then
                If Not (ds_Check.Tables(0).Rows(0).Item("dFromDate") < CType(objCommon.GetCurDatetime(ds_Check.Tables(0).Rows(0).Item("vTimeZoneName").ToString), DateTime) AndAlso CType(objCommon.GetCurDatetime(ds_Check.Tables(0).Rows(0).Item("vTimeZoneName").ToString), DateTime) < ds_Check.Tables(0).Rows(0).Item("dToDate")) Then
                    objCommon.ShowAlert("You are not authorised user,Please contact your administrator", Me.Page)
                    Exit Sub
                End If
            End If
        End If
        'Password = hdnpassword.Value
        strLoginName = ds_Check.Tables(0).Rows(0)("vLoginName")
        strUsername = txtUserName.Value
        If Password Is Nothing Or Password = String.Empty Then
            Password = Request.Form("password")
            If Me.hdnRelogin.Value = True Then
                strPassword = objHelp.EncryptPassword(Password)
            Else
                strPassword = objHelp.EncryptPassword(Me.DecryptValue(Password))
            End If
        Else
            strPassword = objHelp.EncryptPassword(Password)
        End If
        Session.Add(S_Profiles, CType(Me.ViewState(VS_Profiles), DataTable))
        strIpAddress = Me.Request.Headers("HTTP_X_FORWARDED_FOR")
        If strIpAddress Is Nothing OrElse strIpAddress = String.Empty Then
            strIpAddress = Request.ServerVariables("REMOTE_ADDR")
        End If

        If Me.ActiveLoginFailedAttempt() Then
            If Not Me.ValidateBlocked() Then
                Me.txtUserName.Value = ""
                'hdnpassword.Value = ""
                Me.ddlProfile.Items.Clear()
                Session.RemoveAll()
                Exit Sub
            End If
        End If

        ds = objHelp.ValidateUser(strUsername, strPassword)
        If Not IsNothing(ds) Then
            dv = ds.Tables(0).DefaultView
            dv.RowFilter = "vUserTypeCode = '" + Me.ddlProfile.SelectedItem.Value.Trim() + "'"
            dt = dv.ToTable()

            If dt.Rows.Count <= 0 Then
                Me.AssingLoginFailureDetails()
                If BlockMsg <> "" Then
                    Me.objCommon.ShowAlert(BlockMsg, Me)
                    Exit Sub
                End If
                'objCommon.ShowAlert("Login Failed. Try Again", Me.Page)
                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "SpanLoginFailedMessage", "SpanLoginFailedMessage();", True)
                Me.txtUserName.Value = ""
                'hdnpassword.Value = ""
                Me.ddlProfile.Items.Clear()
                Session.RemoveAll()
                Exit Sub
            End If

            If ds.Tables(0).Rows(0)("cStatusIndi").ToString.Trim.ToUpper() = "C" Then
                objCommon.ShowAlert("User inactive, please contact your administrator !!!", Me.Page)
                Exit Sub
            End If
            dr = dt.Rows(0)
        End If

        strPassword = objHelp.DecryptPassword(strPassword)
        If strUsername.Trim.ToUpper() = strPassword.Trim.ToUpper() Then
            Dim q As String = String.Empty
            q = "frmChangePwd.aspx?vUserName=" + Me.Session(S_UserName).ToString() + "&Mode=RstPwd" + "&Reset=N" + "&vUserTypeCode=" + Me.Session(S_UserType).ToString()
            Me.Response.Redirect(q, True)
            Exit Sub
        End If
        ''===========Creating a cookie on client side to moniter client's  logon state(15-Sept-2011)
        Response.Cookies("UserSingleLogon").Value = System.DateTime.Now.ToString()
        Response.Cookies("UserSingleLogon").Expires = DateTime.Now.AddDays(1)
        '==============================================================================

        If ds_Check.Tables(0).Rows(0)("iUserGroupCode").ToString.Trim = "1" Then
            SuperUserId = Me.Session(S_UserID)
        End If

        Dim ds_DownTime As DataSet = Nothing
        Dim dv_DownTime As DataView
        Dim dt_DownTime As DataTable = Nothing

        If Not ConfigurationManager.AppSettings("UnderMaintanance") Is Nothing Then
            If ConfigurationManager.AppSettings("UnderMaintanance").ToString = "Y" Then
                ds_DownTime = objHelp.ProcedureExecute("dbo.Proc_DownTimeMaster", Session(S_UserID))
                dv_DownTime = ds_DownTime.Tables(0).DefaultView()
                dv_DownTime.RowFilter = "iUserId = " + Session(S_UserID)
                dt_DownTime = dv_DownTime.ToTable()
            End If
        End If
        If Not ds_DownTime Is Nothing AndAlso ds_DownTime.Tables(0).Rows.Count > 0 Then
            If Not dt_DownTime Is Nothing AndAlso dt_DownTime.Rows.Count > 0 Then
                If bExpiryDays Then
                    userType = Me.Session(S_UserType)

                    strUserLoginMessage = "SpanLoginMessage("
                    strUserLoginMessage += "'" + userType + "',"
                    'strUserLoginMessage += "'" + Convert.ToString(ExpiryDays.ToString()) + "'"
                    strUserLoginMessage += ");"

                    ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "SpanLoginMessage", strUserLoginMessage, True)
                    Exit Sub
                Else
                    Me.Response.Redirect("frmMainPage.aspx?usertype=" + Me.Session(S_UserType), False)
                End If
            Else
                Me.Response.Redirect("logoutPage.aspx", False)
                Me.Response.Redirect("frmUnderMaintanance.aspx", False)
                Session.RemoveAll()
            End If
        Else
            If bExpiryDays Then
                userType = Me.Session(S_UserType)
                strUserLoginMessage = "SpanLoginMessage("
                strUserLoginMessage += "'" + userType + "',"
                strUserLoginMessage += ");"

                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "SpanLoginMessage", strUserLoginMessage, True)
                Exit Sub
            Else
                Me.Response.Redirect("frmMainPage.aspx?usertype=" + Me.Session(S_UserType), False)
            End If
        End If
    End Sub

    Protected Sub lnkresendOTP_Click(sender As Object, e As EventArgs)
        Dim ds_Check As New DataSet

        If Not objHelp.Proc_UserMst(Me.txtUserName.Value.Trim(), Me.ddlProfile.SelectedItem.Value.Trim(), OpType_BizNetWeb, ds_Check, eStr_Retu) Then
            Throw New Exception(eStr_Retu)
        End If
        Password = Request.Form("password")
        Password = Me.DecryptValue(Password)
        If ds_Check.Tables(0).Rows(0)("isMFA").ToString.Trim.ToUpper() = "Y" Then
            GetOtp()
        Else

            Me.objCommon.ShowAlert("You do not have rights of isMFA profile", Me.Page)
        End If
    End Sub
    Protected Sub BtnReLogin_Click(sender As Object, e As EventArgs) Handles BtnReLogin.Click
        SuccessLogin()
    End Sub

    <Web.Services.WebMethod()>
    Public Shared Function GetOTPInfo(ByVal txtOtpNo As String) As String
        Dim clsCommon As New clsCommon
        Dim ds_ChOTPInfo As New DataSet
        Dim Count As Integer = 0
        Dim objHelpDB As WS_HelpDbTable.WS_HelpDbTable = clsCommon.GetHelpDbTableRef()
        Dim Result As String = String.Empty
        Dim eStr As String = String.Empty
        Try
            If Not objHelpDB.Proc_OTPInfo(HttpContext.Current.Session(S_UserID), ds_ChOTPInfo, eStr) Then
                Throw New Exception(eStr)
            End If

            For i As Integer = 0 To ds_ChOTPInfo.Tables(0).Rows.Count - 1
                If (txtOtpNo = ds_ChOTPInfo.Tables(0).Rows(i)("vOTPNo").ToString().Trim().ToUpper()) Then
                    Result = "Success"
                    Exit For
                Else
                    Count = Count + 1
                End If
            Next

            If Count = ds_ChOTPInfo.Tables(0).Rows.Count Then
                Result = "error"
                txtOtpNo = ""
            End If
            Return Result
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Sub SMSDetails(ByVal OTPCode As String, ByVal SMSDetails As DataTable, ByVal Mobile_No As String, ByVal S_UserID As String)
        Dim SMSURL As String = String.Empty
        Dim SMSUser As String = String.Empty
        Dim SMSPassw As String = String.Empty
        Dim SMSSender As String = String.Empty
        Dim OTPMessage As String = String.Empty
        'Dim OTPMessage As String = OTPCode + " Is your one time password. Kindly use this OTP To access Disoft C. Please don't share with anyone powered by Sarjen Systems."

        Try
            OTPMessage = OTPCode + " Is your one time password. Kindly use this OTP To access Disoft. Please don't share with anyone powered by Sarjen Systems."
            SMSURL = Convert.ToString(SMSDetails.Rows(0)("vSMSUrl"))
            SMSUser = Convert.ToString(SMSDetails.Rows(0)("vSMSUser"))
            SMSPassw = Convert.ToString(SMSDetails.Rows(0)("vSMSPwd"))
            SMSSender = Convert.ToString(SMSDetails.Rows(0)("vSMSSender"))
            Dim SMSSendingURL As String = SMSURL + "username=" + SMSUser + "&pass=" + SMSPassw + "&senderid=" + SMSSender + "&dest_mobileno=" + HttpUtility.UrlEncode(Mobile_No) + "&message=" + OTPMessage + "&response=Y"
            Dim myReq As HttpWebRequest = DirectCast(WebRequest.Create(SMSSendingURL), HttpWebRequest)
            Dim myResp As HttpWebResponse = DirectCast(myReq.GetResponse(), HttpWebResponse)
            Dim respStreamReader As New System.IO.StreamReader(myResp.GetResponseStream())
            Dim responseString As String = respStreamReader.ReadToEnd()
            respStreamReader.Close()
            myResp.Close()
            ExMsgInfoDetails("SMS", "", OTPMessage, "", "", S_UserID, "Y", "", Mobile_No)
        Catch ex As Exception
            ExMsgInfoDetails("SMS", "", OTPMessage, "", "", S_UserID, "N", ex.Message, Mobile_No)
            objCommon.ShowAlert(ex.ToString(), Page)
        End Try
    End Sub

    Public Sub EmailDetails(ByVal OTPCode As String, ByVal vEmailId As String, ByVal S_UserID As String, ByVal TenantDt As DataTable)
        Dim Emailmessage As String = String.Empty
        Dim myHtmlFile As String = String.Empty
        Dim ToEmail As String = String.Empty
        Dim OTPGenDate As DateTime = DateTime.Now
        Dim FinalOTPDate As String = OTPGenDate.ToString("dd-MMM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture)
        Dim EmailSubject As String = "OTP For Login"
        Dim UploadedBy As String = HttpContext.Current.Session(S_UserNameWithProfile)
        Dim myBuilder As System.Text.StringBuilder = New System.Text.StringBuilder()
        Dim strMessage As New StringBuilder

        Dim TenantInfo As New SS.Mail.TenantInfo()
        TenantInfo.TenantId = TenantDt.Rows(0)("vTenantId").ToString()
        TenantInfo.ClientId = TenantDt.Rows(0)("vClientId").ToString()
        TenantInfo.Client_secret = TenantDt.Rows(0)("vSecretKey").ToString()
        TenantInfo.EmailUser = TenantDt.Rows(0)("vFromEmail").ToString()
        TenantInfo.EmailPassword = TenantDt.Rows(0)("vPassword").ToString()
        Dim AuthenticationKeySecretKey As Boolean = TenantDt.Rows(0)("bAuthSecretKey")

        Try
            Dim sBody As String = String.Empty
            Dim strFromMail As String = System.Configuration.ConfigurationSettings.AppSettings("Username").ToString()

            sBody = "Dear User" + "<br>" + "<br>"
            sBody = sBody + "We have sent you this email in response to reset your Disoft C application password as per below details." + "<br>" + "<br>"
            sBody = sBody + "<span style=""width:100%; text-align:center; font-family:Verdana; font-size:12px;margin-left: 270px"">OTP Details</span>" + "<br>"
            sBody = sBody + "--------------------------------------------------------------------------------------------" + "<br>"
            sBody = sBody + "Requested By&ensp;&ensp;&ensp; : " + txtUserName.Value + "<br>"
            sBody = sBody + "Requested Date &ensp;: " + FinalOTPDate + "<br>"
            sBody = sBody + "OTP &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp; : " + "<b>" + OTPCode + "</b>" + "<br>" + "<br>"
            sBody = sBody + "--------------------------------------------------------------------------------------------" + "<br>"
            sBody = sBody + "Please contact Disoft C system administrator if you have not done this request." + "<br>" + "<br>" + "<br>"
            sBody = sBody + "<b>This is an auto generated mail, please do not reply on this email id.</b>" + "<br>" + "<br>"
            sBody = sBody + "Confidential: The content of all the emails sent and/or received by us (including any attachment) is confidential to the intended recipient at the email address to which it has been addressed. It may not be disclosed to or used by anyone other than those addressed, nor may it be copied, disseminated, or distributed in any way. Doing of such acts strictly prohibited. If you have received this email or file in error, please notify the system manager and delete this email from your system. We will ensure such a mistake does not occur in the future. Please note that as a recipient it is your responsibility to check E-mails for malicious software. Finally, the opinions disclosed by sender do not have to reflect those of the company, therefore the company refuses to take any liability for the damage caused by the content of this E-mail."

            Dim message = New Message With {
                     .Subject = EmailSubject,
                     .Body = New ItemBody With {.ContentType = BodyType.Html, .Content = sBody.ToString},
                     .ToRecipients = New List(Of Recipient)(),
                     .BccRecipients = New List(Of Recipient)() From {New Recipient With {.EmailAddress = New EmailAddress With {.Address = vEmailId.Trim()}}}
               }
            '.BccRecipients Added by Bhargav Thaker 24Feb2023

            Try
                System.Net.ServicePointManager.ServerCertificateValidationCallback = Function(se As Object,
                cert As System.Security.Cryptography.X509Certificates.X509Certificate,
                chain As System.Security.Cryptography.X509Certificates.X509Chain,
                sslerror As System.Net.Security.SslPolicyErrors) True
                System.Net.ServicePointManager.MaxServicePointIdleTime = 1000
                System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType.Ssl3 Or (SecurityProtocolType.Tls12 Or (SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls)))
                If AuthenticationKeySecretKey Then
                    SS.Mail.EmailSender.sendMailBySecret(TenantInfo, message, True)
                Else
                    SS.Mail.EmailSender.SendMailUsingPassword(TenantInfo, message, True)
                End If
                Emailmessage = "And Email Id : " + vEmailId
                ExMsgInfoDetails("Mail", EmailSubject, sBody.ToString(), strFromMail, vEmailId, S_UserID, "Y", "", "")
            Catch ex As Exception
                Throw ex
            End Try
        Catch error_t As Exception
            objCommon.ShowAlert(error_t.ToString(), Page)
        End Try
    End Sub

    Public Sub ExMsgInfoDetails(ByVal vNotificationType As String, ByVal vSubject As String,
                                ByVal vBody As String, ByVal vFromEmailId As String, ByVal vToEmailId As String,
                                ByVal iCreatedBy As String, ByVal cIsSent As String, ByVal vRemarks As String, ByVal Mobile_No As String)
        Dim dsExMsgInfo As New DataSet
        Dim drExMsgInfo As DataRow
        If Not objHelp.GetExMsgInfoDetails("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty,
                                               dsExMsgInfo, eStr_Retu) Then
            Throw New Exception(eStr_Retu + vbCrLf + "Error while retrieving EXE Msg Info Details.")
        End If
        drExMsgInfo = dsExMsgInfo.Tables(0).NewRow
        drExMsgInfo("vNotificationType") = vNotificationType
        drExMsgInfo("vSubject") = vSubject.ToString()
        drExMsgInfo("vBody") = vBody
        drExMsgInfo("vFromEmailId") = vFromEmailId
        drExMsgInfo("vToEmailId") = String.Empty 'Modify by Bhargav Thaker 24Feb2023
        drExMsgInfo("vBCCEmailId") = vToEmailId 'Added by Bhargav Thaker 24Feb2023
        drExMsgInfo("iCreatedBy") = iCreatedBy
        drExMsgInfo("dCreatedDate") = Date.Now
        drExMsgInfo("vPhoneNo") = Mobile_No
        drExMsgInfo("cIsSent") = cIsSent
        If cIsSent = "Y" Then
            drExMsgInfo("dSentDate") = Date.Now
        Else
            drExMsgInfo("vRemarks") = vRemarks
        End If
        dsExMsgInfo.Tables(0).Rows.Add(drExMsgInfo)
        dsExMsgInfo.Tables(0).AcceptChanges()
        If Not objLambda.SaveExMsgInfo(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add,
                                        dsExMsgInfo, HttpContext.Current.Session(S_UserID), eStr_Retu) Then
            Throw New Exception(eStr_Retu + vbCrLf + "Error while Saving in Mail Sending Details.")
            Exit Sub
        End If
    End Sub
End Class