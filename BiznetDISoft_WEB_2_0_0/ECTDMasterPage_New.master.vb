
Partial Class ECTDMasterPage_New
    Inherits System.Web.UI.MasterPage

#Region "VARIABLE DECLARATION "

    Private objCommon As New clsCommon
    Private Objhelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()


#End Region

#Region "Page Load"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Me.CheckUserLoginDetails() Then
                Me.Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("MainPage") + "Default.aspx?SessionExpire=true")
                Exit Sub
            End If
            'add by shivani panyda
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "assignCSS", "  assignCSS();", True)
            'End
        Catch ex As Exception
            Throw New Exception("Error while Page_Init()")
        End Try
    End Sub

    Private Function CheckUserLoginDetails() As Boolean
        Dim strUserId As String = ""
        Dim wStr As String = ""
        Dim eStr As String = ""
        Dim ds_UserLoginDetails As New DataSet
        Dim ValidRoleOperation As String = String.Empty
        Dim StrUrlcheck As String
        Dim ds_Valid As New DataSet
        Dim strUrl As String = String.Empty
        Try
            strUserId = Me.Request.QueryString("UserId").ToString()
            wStr = "iUserID = " + strUserId
            If Not Me.Objhelp.GetUserLoginDetails(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_UserLoginDetails, eStr) Then
                Me.ShowErrorMessage(eStr, "Error While Getting UserLoginDetails.")
                Return False
            End If

            If ds_UserLoginDetails.Tables(0).Rows.Count = 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UserLoginAlert", "UserLoginAlert();", True)
                Session.RemoveAll()
                Return False
            End If

            If Not Me.Session(S_UserID) Is Nothing Then
                If Me.Session(S_UserID).ToString() = strUserId Then
                    Me.hfSession.Value = Session.Timeout
                    Me.lblUserName.Text = Session(S_FirstName).ToString.Trim() + " " + Session(S_LastName).ToString.Trim()
                    Me.lblUserName1.Text = Session(S_FirstName).ToString.Trim() + " " + Session(S_LastName).ToString.Trim()
                    Me.lblLastLoggedInTime.Text = Session(S_LastLoginDateTime).ToString.Trim()

                    'Return True  '' Commented by dipen shah for session Issue.
                End If
            End If

            If Not Me.FillUserDetail(strUserId) Then
                Return False
            End If

            'For Valid User For Operation Added By Naimesh Dave on 28-Aug-2008
            StrUrlcheck = Me.Request.Url.AbsolutePath
            StrUrlcheck = StrUrlcheck.Substring(StrUrlcheck.LastIndexOf("/") + 1)

            'strUrl = ConfigurationManager.AppSettings.Item("UrlPath").Trim()
            If Not Me.Objhelp.CheckUserRoleOperation(StrUrlcheck, Me.Session(S_UserType).ToString.Trim(), ValidRoleOperation, ds_Valid, eStr) Then
                Me.Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("MainPage") + "frmUnderConstruction.aspx", True)
            End If

            If ValidRoleOperation.ToUpper = "NO" Then
                Me.Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("MainPage") + "frmUnderConstruction.aspx", True)
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(eStr, ex.Message + ".....CheckUserLoginDetails().")
            Return False
        End Try
    End Function

    Private Function FillUserDetail(ByVal strUserId As String) As Boolean
        Dim dr As DataRow
        Dim wStr As String = ""
        Dim eStr As String = ""
        Dim dsUsers As New DataSet
        Dim dt As New DataTable
        Dim ScopeValue As String = String.Empty
        Dim strTimeZone As String = String.Empty
        Dim strLoginErrorMsg As String = String.Empty
        Dim str() As String
        Dim UTCHourDiff As String = String.Empty
        Try
            wStr = "cStatusIndi <> 'D' And iUserId = " + strUserId
            If Not Me.Objhelp.GetViewUserMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            dsUsers, eStr) Then
                Throw New Exception(eStr)
                Return False
            End If

            If dsUsers Is Nothing Then
                Throw New Exception("User Not Found")
                Return False
            End If
            dt = dsUsers.Tables(0)
            dr = dt.Rows(0)

            Session.Add(S_LoginName, dr.Item("vLoginName").ToString)
            'Session.Add(S_UserID, dr.Item("iUserId").ToString)
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
            'UTCHourDiff = TimeZoneInfo.FindSystemTimeZoneById(Session(S_TimeZoneName).ToString).BaseUtcOffset.ToString
            'UTCHourDiff = If(UTCHourDiff.Substring(0, 1) = "-", UTCHourDiff.Substring(0, 6), "+" + UTCHourDiff.Substring(0, 5))
            Session.Add(S_TimeOffSet, UTCHourDiff)
            '---------------------------------------------
            Session.Add(S_Time, CDate(objCommon.GetCurDatetime(Me.Session(S_TimeZoneName).ToString)).ToString("dd-MMM-yyyy HH:mm") + " (" + Me.Session(S_TimeOffSet).ToString + " GMT)")

            Response.Cookies("UserSingleLogon").Value = System.DateTime.Now.ToString()
            Response.Cookies("UserSingleLogon").Expires = DateTime.Now.AddDays(1)
            Me.hfSession.Value = Session.Timeout
            Me.lblUserName.Text = Session(S_FirstName).ToString.Trim() + " " + Session(S_LastName).ToString.Trim()
            Me.lblUserName1.Text = Session(S_FirstName).ToString.Trim() + " " + Session(S_LastName).ToString.Trim()
            Me.lblTime.Text = Session(S_Time).ToString  ' Added By Jeet Patel on 02-Apr-2015

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(eStr, ex.Message + ".....FillUserDetail().")
            Return False
        End Try
    End Function

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        Me.lblerrormsg.Text = exMessage + "<BR> " + eStr
    End Sub

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        Me.lblerrormsg.Text = exMessage + "<BR> " + eStr
    End Sub

#End Region

    Protected Sub btnContinueWorking_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinueWorking.Click
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ResetSessionTimer", "btnContinueWorking_Click();", True)
    End Sub

#End Region

End Class

