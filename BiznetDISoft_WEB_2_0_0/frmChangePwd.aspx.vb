
Partial Class frmChangePwd
    Inherits System.Web.UI.Page

#Region " VARIABLE DECLARATION "
    Private objcommon As New clsCommon
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private Const VS_dtUserMst As String = "dtUserMst"

    Private strMessage As String = String.Empty
    Private Mode As String = String.Empty
    Private UserTypeCode As String = String.Empty
    Private UserName As String = String.Empty

    Private eStr_Retu As String
    Private Const VS_IsExpired As String = "IsExpired"
    Private Const VS_oldpassword As String = "OldPassword"
#End Region

#Region "Form Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(S_UserID) Is Nothing Then
            Session(S_UserID) = Session(S_UserID1).ToString
        End If

        'BtnSubmit.OnClientClick = "return CheckVal();" 'Comment By Bhargav Thaker 28Mar2023
        CType(Me.Master.FindControl("lblHeading"), Label).Text = "Change Password"
        Page.Title = " :: Change Password ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
        If Not Me.IsPostBack Then
            Me.ViewState(VS_IsExpired) = "False"
            If Not Me.Request.QueryString("Expired") Is Nothing AndAlso
                Me.Request.QueryString("Expired").Trim = "1" Then
                Me.HideMenu()
                Me.ViewState(VS_IsExpired) = "True"
            End If

            If Not Me.Request.QueryString("Msg") Is Nothing AndAlso
                Me.Request.QueryString("Msg").Trim.Length > 0 Then
                strMessage = Me.Request.QueryString("Msg").Trim
                Me.lblMessage.Text = strMessage
            End If

            If Not Me.Request.QueryString("Mode") Is Nothing AndAlso
                  Me.Request.QueryString("Mode").Trim.Length > 0 Then
                Mode = Me.Request.QueryString("Mode").Trim
                UserName = Me.Request.QueryString("vUserName").Trim
                UserTypeCode = Me.Request.QueryString("vUserTypeCode").Trim
                'Me.txtOldPassWord .Text = 
            End If

            'Added By Bhargav Thaker 28Mar2023 Start
            Dim ds_data As New DataSet
            Dim wStr_PasswordPolicyMst As String = " cActiveFlag = 'Y' AND cStatusIndi <> 'C'"
            If Not objHelp.GetPasswordPolicyMst(wStr_PasswordPolicyMst, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                                ds_data, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            For Each dr As DataRow In ds_data.Tables(0).Rows
                If Convert.ToString(dr("vPolicyDesc")).Trim.ToLower = "MinLength".ToLower Then
                    txtpwdlen.Value = Convert.ToInt32(dr("vValue"))
                End If
                If Convert.ToString(dr("vPolicyDesc")).Trim.ToLower = "PwdStyle".ToLower Then
                    txtpwdstyle.Value = Convert.ToInt32(dr("vValue"))
                    Exit For
                End If
            Next dr
            'Added By Bhargav Thaker 28Mar2023 End

            GenCall()
        End If
    End Sub
#End Region

#Region " GENCALL "
    Private Sub GenCall()
        Dim whereCondition As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim dsUserMst As New DataSet
        Dim EncryptPwd As String = String.Empty
        Dim dr As DataRow

        whereCondition = " vUserName='" + Session(S_UserName) + "' And cStatusIndi <> 'D'"
        If Mode.ToUpper() = "RSTPWD" Then
            whereCondition = " vUserName='" + UserName + "'" ' and vUserTypeCode='" + UserTypeCode + "'"
        End If

        If Not objHelp.getuserMst(whereCondition, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                dsUserMst, eStr_Retu) Then
            ShowErrorMessage("", eStr_Retu)
            Exit Sub
        End If

        Me.ViewState(VS_dtUserMst) = dsUserMst.Tables(0)
        If Mode.ToUpper() = "RSTPWD" Then

            dr = dsUserMst.Tables(0).Rows(0)
            Me.ViewState(VS_oldpassword) = dr("vLoginPass").ToString()
            Me.txtOldPassWord.Attributes.Add("value", Me.ViewState(VS_oldpassword))
            Me.HideMenu()
        End If
    End Sub
#End Region

#Region " BUTTON EVENTS "
    Protected Sub BtnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSubmit.Click
        Dim dsUserMst As New DataSet
        Dim dtUserMst As New DataTable
        Dim eStr As String = String.Empty
        Dim EncryptPwd As String
        Dim DecryptPwd As String
        Dim TempSession As String = String.Empty

        Try
            TempSession = Session(S_UserID).ToString()
            If Not Me.Request.QueryString("Mode") Is Nothing AndAlso
                            Me.Request.QueryString("Mode").Trim.Length > 0 Then
                If Not Me.ValidatePasswordPolicy(eStr) Then
                    objcommon.ShowAlert(eStr, Me)
                    Exit Sub
                End If

                If Not Me.ViewState(VS_dtUserMst) Is Nothing Then
                    dtUserMst = CType(Me.ViewState(VS_dtUserMst), DataTable)
                    EncryptPwd = Me.objHelp.EncryptPassword(Me.txtNewPassword.Text.Trim)
                    For Each dr As DataRow In dtUserMst.Rows
                        dr("vLoginPass") = EncryptPwd
                        dr("iModifyBy") = Me.Session(S_UserID)
                    Next
                    'dtUserMst.Rows(0)("vLoginPass") = EncryptPwd
                    'dtUserMst.Rows(0)("iModifyBy") = Me.Session(S_UserID)
                    dtUserMst.AcceptChanges()
                    dsUserMst.Tables.Add(dtUserMst)
                Else
                    Exit Sub
                End If
            Else

                DecryptPwd = objHelp.DecryptPassword(Session(S_Password).ToString)
                If LCase(DecryptPwd) <> LCase(txtOldPassWord.Text.Trim) Then
                    lblMessage.Text = "Old Password Is Not Valid"
                    Exit Sub
                Else
                    If Not Me.ValidatePasswordPolicy(eStr) Then
                        objcommon.ShowAlert(eStr, Me)
                        Exit Sub
                    End If

                    If Not Me.ViewState(VS_dtUserMst) Is Nothing Then
                        dtUserMst = CType(Me.ViewState(VS_dtUserMst), DataTable)
                        EncryptPwd = Me.objHelp.EncryptPassword(Me.txtNewPassword.Text.Trim)
                        For Each dr As DataRow In dtUserMst.Rows
                            dr("vLoginPass") = EncryptPwd
                            dr("iModifyBy") = Me.Session(S_UserID)
                        Next
                        'dtUserMst.Rows(0)("vLoginPass") = EncryptPwd
                        'dtUserMst.Rows(0)("iModifyBy") = Me.Session(S_UserID)
                        dtUserMst.AcceptChanges()
                        dsUserMst.Tables.Add(dtUserMst)
                    Else
                        Exit Sub
                    End If
                End If
            End If

            If Not objLambda.Insert_ChangePassword(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, dsUserMst, Me.Session(S_UserID), eStr) Then
                Me.ShowErrorMessage("", eStr)
                Exit Sub
            End If

            If Not Me.Request.QueryString("Mode") Is Nothing AndAlso
                          Convert.ToString(Me.Request.QueryString("Mode")).Trim() = "RstPwd" Then
                Me.txtOldPassWord.Attributes.Add("value", "")

                'To redirect to home page if new user : Start
                If Session("UserName") = Me.Request.QueryString("vUserName") And Convert.ToString(Me.Request.QueryString("Reset")).Trim() = "Y" Then 'if change passwpord from usercreation module
                    ScriptManager.RegisterClientScriptBlock(Me.Page(), Me.GetType(), "Redir", "ShowAlert('Password Changed Successfully','5')", True)

                ElseIf Session("UserName") = Me.Request.QueryString("vUserName") Then 'IF USER IS NEW
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('Password Changed Successfully',1)", True)
                Else
                    ScriptManager.RegisterClientScriptBlock(Me.Page(), Me.GetType(), "Redir", "ShowAlert('Password Changed Successfully','5')", True)
                End If
                'To redirect to home page if new user : End
                Exit Sub
            End If

            Session(S_Password) = EncryptPwd
            txtNewConfPass.Text = ""
            txtNewPassword.Text = ""
            txtOldPassWord.Text = ""
            Me.txtOldPassWord.Attributes.Add("value", "")
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('Password Changed Successfully.',1)", True)
            'ScriptManager.RegisterClientScriptBlock(Me.Page(), Me.GetType(), "Redir", "AlertAndRedirect('frmmainpage.aspx','Password Changed Successfully')", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "", ex)
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'If Me.ViewState(VS_IsExpired) = "True" Then
        '    Response.Redirect("Default.aspx")
        'Else
        '    Response.Redirect("frmmainpage.aspx")
        'End If

        If Me.ViewState(VS_IsExpired) = "True" Then
            'Session.Abandon()
            'Response.Redirect("Default.aspx")
            ' Change Redirect functionality on 25-Jun-2015 for Solving Bug
            Response.Redirect("Logoutpage.aspx", True)
            ' ============================================================
        End If
        Dim parameter As String = String.Empty
        If Request.QueryString.HasKeys() Then
            If Convert.ToString(Me.Request.QueryString("Reset")).Trim() = "" Then
                parameter = "read"
            Else
                parameter = Convert.ToString(Me.Request.QueryString("Reset")).Trim()
            End If

            If Not Me.Request.QueryString("Mode") Is Nothing AndAlso
                   Convert.ToString(Me.Request.QueryString("Mode")).Trim() = "RstPwd" And Convert.ToString(Me.Request.QueryString("Reset")).Trim() = "Y" Then

                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinClose", "window.close()", True)
            Else
                If Session("UserName") = Me.Request.QueryString("vUserName") Then
                    Response.Redirect("Logoutpage.aspx", True)
                End If
                'To clear session of new user if do not change password : End
                ScriptManager.RegisterClientScriptBlock(Me.Page(), Me.GetType(), "Redir", "AlertAndRedirect('Default.aspx','Password Not Changed')", True)
                Exit Sub

            End If
        Else
            Me.Response.Redirect("frmMainPage.aspx")
        End If

        'If Not Me.Request.QueryString("Mode") Is Nothing AndAlso _
        '       Convert.ToString(Me.Request.QueryString("Mode")).Trim() = "RstPwd" Then
        '    'Me.txtOldPassWord.Attributes.Add("value", "")
        '    'To clear session of new user if do not change password : Start
        '    If Session("UserName") = Me.Request.QueryString("vUserName") Then
        '        Response.Redirect("Logoutpage.aspx", True)
        '    End If
        '    'To clear session of new user if do not change password : End
        '    ScriptManager.RegisterClientScriptBlock(Me.Page(), Me.GetType(), "Redir", "AlertAndRedirect('Default.aspx','Password Not Changed')", True)
        '    Exit Sub
        'End If
        'Response.Redirect("frmmainpage.aspx")

    End Sub

    Private Function ValidatePasswordPolicy(ByRef strMessage As String) As Boolean
        ValidatePasswordPolicy = True
        Dim wStr_PasswordPolicyMst As String = " cActiveFlag = 'Y' AND cStatusIndi <> 'C'"
        Dim wStr_PasswordHistory As String = " iUserID=" + Me.Session(S_UserID) +
                                " AND cStatusIndi <> 'C' ORDER BY iSrNo DESC "
        Dim dsPasswordPolicyMst As New DataSet
        Dim dsPasswordHistory As New DataSet
        Dim PwdHistoryCount As Integer = 0
        Dim minLength As Integer = 0
        Dim pwdRegex As Regex = Nothing
        Dim tempMessage As String = String.Empty
        Dim MatchChar As Integer = 0
        Dim dv As New DataView
        Dim oldPasswordFromHistory As String = String.Empty
        Dim newPasswordFromUserMst As String = String.Empty
        Try
            If Not objHelp.GetPasswordPolicyMst(wStr_PasswordPolicyMst, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                                dsPasswordPolicyMst, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            For Each dr As DataRow In dsPasswordPolicyMst.Tables(0).Rows
                If Convert.ToString(dr("vPolicyDesc")).Trim.ToLower = "MatchHistory".ToLower Then
                    PwdHistoryCount = Convert.ToInt32(dr("vValue"))

                    If Not objHelp.GetPasswordHistory(wStr_PasswordHistory, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                                      dsPasswordHistory, eStr_Retu) Then
                        Throw New Exception(eStr_Retu)
                    End If
                    If dsPasswordHistory.Tables(0).Rows.Count < PwdHistoryCount Then
                        PwdHistoryCount = dsPasswordHistory.Tables(0).Rows.Count
                    End If

                    For index As Integer = 0 To PwdHistoryCount - 1
                        'Get Number of Characters to match
                        dv = dsPasswordPolicyMst.Tables(0).DefaultView
                        dv.RowFilter = "vPolicyDesc = 'MatchChar'"
                        oldPasswordFromHistory = Convert.ToString(dsPasswordHistory.Tables(0).Rows(index)("vPassword")).Trim.ToLower
                        newPasswordFromUserMst = Me.objHelp.EncryptPassword(Me.txtNewPassword.Text.Trim.ToLower)
                        If dv.ToTable.Rows.Count > 0 Then
                            If Integer.TryParse(Convert.ToString(dv.ToTable.Rows(0)("vValue")), MatchChar) Then
                                If MatchChar > 0 Then
                                    If oldPasswordFromHistory.Trim.Length > MatchChar Then
                                        oldPasswordFromHistory = oldPasswordFromHistory.Substring(0, MatchChar)
                                    End If

                                    If newPasswordFromUserMst.Trim.Length > MatchChar Then
                                        newPasswordFromUserMst = newPasswordFromUserMst.Substring(0, MatchChar)
                                    End If
                                End If
                            End If
                        End If
                        If Convert.ToString(oldPasswordFromHistory).Trim.ToLower = newPasswordFromUserMst.Trim.ToLower Then
                            If MatchChar > 0 Then
                                strMessage = "First " + MatchChar.ToString() + " Characters Match With Password In List Of Previous " + Convert.ToString(dr("vValue")) + " Passwords !!!"
                            Else
                                strMessage = "Password Already Exists In The List Of Previous " + Convert.ToString(dr("vValue")) + " Passwords !!!"
                            End If
                            ValidatePasswordPolicy = False
                        End If
                    Next

                ElseIf Convert.ToString(dr("vPolicyDesc")).Trim.ToLower = "MinLength".ToLower Then
                    If Integer.TryParse(Convert.ToString(dr("vValue")), minLength) Then
                        If Me.txtNewPassword.Text.Trim.Length < minLength Then
                            If strMessage.Trim.Length > 0 Then
                                strMessage += "\n"
                            End If
                            strMessage += "Password Length Should Be Minimum " + minLength.ToString() + " Characters"
                            ValidatePasswordPolicy = False
                        End If
                    End If

                ElseIf Convert.ToString(dr("vPolicyDesc")).Trim.ToLower = "PwdStyle".ToLower Then
                    If Integer.TryParse(Convert.ToString(dr("vValue")), minLength) Then
                        If minLength = 1 Then
                            pwdRegex = New Regex("^(?=.*\d)(?=.*[a-zA-Z])", RegexOptions.IgnoreCase)
                            tempMessage = "Password Must Contain One Alphabet [A-Z] And One Digit [0-9] !!!"

                        ElseIf minLength = 2 Then
                            pwdRegex = New Regex("^(?=.*\d)(?=.*[a-zA-Z])(?=.*[!@#$%])(?!.*\s)", RegexOptions.IgnoreCase)
                            tempMessage = "Password Must Contain Atleast One Alphabet [A-Z] And One Digit [0-9] And One Special Character [!@#$%^&*] !!!"
                        End If

                        If Not pwdRegex.IsMatch(Me.txtNewPassword.Text.Trim) Then
                            If strMessage.Trim.Length > 0 Then
                                strMessage += "\n"
                            End If
                            strMessage += tempMessage
                            ValidatePasswordPolicy = False
                        End If
                    End If
                End If
            Next dr

        Catch ex As Exception
            Throw New Exception("ValidatePasswordPolicy:" + ex.Message)
        End Try
    End Function

    'Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
    '    Me.Response.Redirect("frmMainPage.aspx")
    'End Sub

    'Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    '    'If Me.ViewState(VS_IsExpired) = "True" Then
    '    '    Response.Redirect("Default.aspx")
    '    'Else
    '    '    Response.Redirect("frmmainpage.aspx")
    '    'End If

    '    If Me.ViewState(VS_IsExpired) = "True" Then
    '        'Session.Abandon()
    '        'Response.Redirect("Default.aspx")
    '        ' Change Redirect functionality on 25-Jun-2015 for Solving Bug
    '        Response.Redirect("Logoutpage.aspx", True)
    '        ' ============================================================
    '    End If
    '    Dim parameter As String = String.Empty
    '    If Convert.ToString(Me.Request.QueryString("Reset")).Trim() = "" Then
    '        parameter = "read"
    '    Else
    '        parameter = Convert.ToString(Me.Request.QueryString("Reset")).Trim()
    '    End If

    '    If Not Me.Request.QueryString("Mode") Is Nothing AndAlso _
    '           Convert.ToString(Me.Request.QueryString("Mode")).Trim() = "RstPwd" And Convert.ToString(Me.Request.QueryString("Reset")).Trim() = "Y" Then
    '        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinClose", "window.close()", True)
    '    Else
    '        If Session("UserName") = Me.Request.QueryString("vUserName") Then
    '            Response.Redirect("Logoutpage.aspx", True)
    '        End If
    '        'To clear session of new user if do not change password : End
    '        ScriptManager.RegisterClientScriptBlock(Me.Page(), Me.GetType(), "Redir", "AlertAndRedirect('Default.aspx','Password Not Changed')", True)
    '        Exit Sub
    '    End If

    '    'If Not Me.Request.QueryString("Mode") Is Nothing AndAlso _
    '    '       Convert.ToString(Me.Request.QueryString("Mode")).Trim() = "RstPwd" Then
    '    '    'Me.txtOldPassWord.Attributes.Add("value", "")
    '    '    'To clear session of new user if do not change password : Start
    '    '    If Session("UserName") = Me.Request.QueryString("vUserName") Then
    '    '        Response.Redirect("Logoutpage.aspx", True)
    '    '    End If
    '    '    'To clear session of new user if do not change password : End
    '    '    ScriptManager.RegisterClientScriptBlock(Me.Page(), Me.GetType(), "Redir", "AlertAndRedirect('Default.aspx','Password Not Changed')", True)
    '    '    Exit Sub
    '    'End If
    '    'Response.Redirect("frmmainpage.aspx")
    'End Sub

#End Region

    Private Sub HideMenu()
        Dim tmpMenu As Menu
        tmpMenu = CType(Me.Master.FindControl("menu1"), Menu)

        If Not tmpMenu Is Nothing Then
            'tmpMenu.Items.Clear()
            tmpMenu.Visible = False
        End If
    End Sub

    Private Sub ShowMenu()
        Dim tmpMenu As Menu
        tmpMenu = CType(Me.Master.FindControl("menu1"), Menu)

        If Not tmpMenu Is Nothing Then
            'tmpMenu.Visible = True
        End If
    End Sub

#Region " ERROR MESSAGE "
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objcommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objcommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub
#End Region

End Class
