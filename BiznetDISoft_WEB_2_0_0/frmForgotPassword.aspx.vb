
Partial Class frmForgotPassword

#Region "Variable Declaration"
    Inherits System.Web.UI.Page
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
    Public User_Name As String
    Public UserProfile As String
    Public IUserId As String
#End Region

#Region "Page Load"
    Public Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Session("UserName") Is Nothing Then
                Response.Redirect("default.aspx")
            End If

            User_Name = Session("UserName")
            UserProfile = Session("UserProfile")
            IUserId = Session("IUserId")
            lblUserName.Text = User_Name

            Dim ds_data As New DataSet
            Dim wStr_PasswordPolicyMst As String = " cActiveFlag = 'Y' AND cStatusIndi <> 'C'"
            If Not objHelp.GetPasswordPolicyMst(wStr_PasswordPolicyMst, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                                ds_data, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            For Each dr As DataRow In ds_data.Tables(0).Rows
                If Convert.ToString(dr("vPolicyDesc")).Trim.ToLower = "MinLength".ToLower Then
                    txtpwdlen.Value = Convert.ToInt32(dr("vValue"))
                    Exit For
                End If
            Next dr

            If Not Me.IsPostBack Then
                GenCall()
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "", ex)
        End Try
    End Sub
#End Region

#Region "GENCALL"

    Private Sub GenCall()
        Dim whereCondition As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim dsUserMst As New DataSet
        Dim EncryptPwd As String = String.Empty

        Try
            whereCondition = " vUserName='" + User_Name + "' And cStatusIndi <> 'D'"
            If Mode.ToUpper() = "RSTPWD" Then
                whereCondition = " vUserName='" + User_Name + "'" ' and vUserTypeCode='" + UserTypeCode + "'"
            End If

            If Not objHelp.getuserMst(whereCondition, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    dsUserMst, eStr_Retu) Then
                ShowErrorMessage("", eStr_Retu)
                Exit Sub
            End If

            Me.ViewState(VS_dtUserMst) = dsUserMst.Tables(0)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "", ex)
        End Try
    End Sub

#End Region

#Region "Button Event"
    Protected Sub BtnSubmit_Click(sender As Object, e As EventArgs) Handles BtnSubmit.Click
        Try

            Dim dsUserMst As New DataSet
            Dim dtUserMst As New DataTable
            Dim eStr As String = String.Empty
            Dim EncryptPwd As String
            Dim TempSession As String = String.Empty

            If Not Me.ValidatePasswordPolicy(eStr) Then
                objcommon.ShowAlert(eStr, Me)
                Exit Sub
            End If

            If Not Me.ViewState(VS_dtUserMst) Is Nothing Then
                dtUserMst = CType(Me.ViewState(VS_dtUserMst), DataTable)
                EncryptPwd = Me.objHelp.EncryptPassword(Me.txtNewPassword.Text.Trim)
                For Each dr As DataRow In dtUserMst.Rows
                    dr("vLoginPass") = EncryptPwd
                    dr("iModifyBy") = IUserId
                Next
                dtUserMst.AcceptChanges()
                dsUserMst.Tables.Add(dtUserMst)
            Else
                Exit Sub
            End If

            If Not objLambda.Insert_ChangePassword(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                                    dsUserMst, IUserId, eStr) Then

                Me.ShowErrorMessage("", eStr)
                Exit Sub
            End If

            lblMessage.Text = "Password Changed Successfully"

            txtNewConfPass.Text = ""
            txtNewPassword.Text = ""

            If Session("UserName") <> Nothing Or Session("UserProfile") <> Nothing Or Session("IUserId") <> Nothing Then
                Session("UserName") = Nothing
                Session("UserProfile") = Nothing
                Session("IUserId") = Nothing

            End If
            objcommon.ShowAlertAndRedirect("Password Changed Successfully.", "'Default.aspx'", Page)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "", ex)
        End Try
    End Sub
#End Region

#Region "Validate Function"

    Private Function ValidatePasswordPolicy(ByRef strMessage As String) As Boolean
        ValidatePasswordPolicy = True
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
            Dim wStr_PasswordPolicyMst As String = " cActiveFlag = 'Y' AND cStatusIndi <> 'C'"
            Dim wStr_PasswordHistory As String = " iUserID=" + IUserId + _
                                 " AND cStatusIndi <> 'C' ORDER BY iSrNo DESC "

            If Not objHelp.GetPasswordPolicyMst(wStr_PasswordPolicyMst, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                dsPasswordPolicyMst, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            For Each dr As DataRow In dsPasswordPolicyMst.Tables(0).Rows
                If Convert.ToString(dr("vPolicyDesc")).Trim.ToLower = "MatchHistory".ToLower Then
                    PwdHistoryCount = Convert.ToInt32(dr("vValue"))

                    If Not objHelp.GetPasswordHistory(wStr_PasswordHistory, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
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
#End Region

#Region "ERROR MESSAGE "

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objcommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objcommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub

#End Region

#Region "Exit"
    Public Sub pageExit()
        Dim confirmValue As String = Request.Form("confirm_value")
        Try
            If confirmValue = "Yes" Then
                Session("UserName") = Nothing
                Session("UserProfile") = Nothing
                Session("IUserId") = Nothing
                ScriptManager.RegisterClientScriptBlock(Me.Page(), Me.GetType(), "Redir", "AlertAndRedirect('Default.aspx','You Have Not Changed Your Password!')", True)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "", ex)
        End Try

    End Sub
#End Region

End Class
