
Partial Class frmEncryptDecryptPassword1
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Dim ObjCommon As New clsCommon
    Private Const VS_UserId As String = "UserId"
    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
    Dim Password As String
#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Page.Title = " :: User Password Detail ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
        If Not IsPostBack Then
            fillDDlLoginName()
        End If

    End Sub

#End Region

    '====Created By Deepak Singh on 18-09-09===to Store and Get Encrypted Password from/into DataBase====== 

#Region "Password Encryption"
    Public Function EncryptPassword(ByVal password As String) As String

        Dim len As Int16 = password.Length  'setting len = length of the entered password

        Dim pwd(len) As String              'declaring pwd as a string array of length len


        pwd(0) = password.ToString          'assigning password in pwd(0) 

        Dim cpwd(len) As Char               'cpwd as char array of the same length

        Dim index As Int16
        Dim Lascii(len) As Integer               'Lascii(len) to store corresponding ascii of password

        Dim Sascii(len) As Integer               'Sascii(len) to store value of substituted ascii

        Dim result As New TextBox           'to store the resulting appended string
        Dim Reverse As String
        Dim retstr As String                'returning the obtained string
        Dim countadd As Integer = 0
        cpwd = pwd(0).ToCharArray


        For index = 0 To len - 1
            If countadd = 4 Then
                countadd = 0
            End If
            Lascii(index) = AscW(cpwd(index))

            Sascii(index) = Lascii(index) + (countadd + 1)
            '==========added on 12-10-09=======
            'If Sascii(index) = 13 Then
            '    Sascii(index) = Lascii(index)
            'End If
            cpwd(index) = ChrW(Sascii(index))
            result.Text += cpwd(index)
            countadd += 1
        Next index
        Reverse = result.Text
        retstr = ReversePassword(Reverse)
        Return retstr
    End Function
#End Region

#Region "Reverse Password"

    Public Function ReversePassword(ByVal password As String) As String

        Dim len As Int16 = password.Length
        Dim index As Integer
        Dim Keytextbox As New TextBox
        Dim key As String
        Dim Pwd(len) As Char
        Dim pwd1(len) As String

        Pwd = password.ToCharArray
        Dim intLen As Integer = len - 1
        Dim Reverse(intLen) As Char

        For index = 0 To intLen
            Reverse(index) = Pwd(intLen - index)
            Keytextbox.Text += Reverse(index)
        Next index

        'For index = len - 1 To 0
        '    Keytextbox.Text += Pwd(index)
        'Next index

        key = Keytextbox.Text
        Return (key)

    End Function

#End Region

#Region "Password Decryption"
    Public Function DecryptPassword(ByVal revpassword As String) As String
        Password = ReversePassword(revpassword)
        Dim len As Int16 = Password.Length  'setting len = length of the entered password

        Dim pwd(len) As String              'declaring pwd as a string array of length len


        pwd(0) = Password.ToString          'assigning password in pwd(0) 

        Dim cpwd(len) As Char               'cpwd as char array of the same length

        Dim index As Int16
        Dim Lascii(len) As Integer               'Lascii(len) to store corresponding ascii of password

        Dim Sascii(len) As Integer               'Sascii(len) to store value of substituted ascii

        Dim result As New TextBox           'to store the resulting appended string

        Dim retstr As String                'returning the obtained string
        Dim countadd As Integer = 0
        cpwd = pwd(0).ToCharArray


        For index = 0 To len - 1
            If countadd = 4 Then
                countadd = 0
            End If
            Lascii(index) = AscW(cpwd(index))
            Sascii(index) = Lascii(index) - (countadd + 1)
            '===========added on 12-10-09=======
            'If Sascii(index) = 131 Then
            '    Sascii(index)=
            'End If
            cpwd(index) = ChrW(Sascii(index))
            result.Text += cpwd(index)
            countadd += 1
        Next index
        retstr = result.Text
        Return retstr
    End Function
#End Region

#Region "Button Click Events"

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TextBox1.Text.Trim = "" Then
            Password = CType(ViewState("loginpass"), String)
        Else
            Password = TextBox1.Text
        End If
        TextBox2.Text = EncryptPassword(Password)
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click

        If TextBox2.Text.Trim = "" Then
            Password = CType(ViewState("LoginPass"), String)
        Else
            Password = TextBox2.Text
        End If
        TextBox3.Text = DecryptPassword(Password)
    End Sub

#End Region

#Region "Update(Encrypt) All Passwords"
    Private Function AssignValues(ByVal type As String) As Boolean
        Dim dr As DataRow
        Dim dt_User As New DataTable
        Dim ds_UserMst As New DataSet
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim VS_Choice As String = "Choice"
        'Dim dt_LoginName As New DataTable

        Try
            'Me.ViewState(VS_UserId) = Me.Request.QueryString("Value").ToString
            Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
            wstr = "iUserId<>1"


            If Not objHelp.getuserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_UserMst, estr) Then

                Me.ShowErrorMessage("Error While Getting Data From UserMst", estr)
                Exit Function

            End If
            ViewState("dt_LoginName") = ds_UserMst.Tables(0)
            dt_User = ds_UserMst.Tables(0)
            For Each dr In dt_User.Rows
                Try

                    If type.ToUpper = "E" Then
                        dr("vLoginPass") = EncryptPassword(dr("vLoginPass"))
                    ElseIf type.ToUpper = "D" Then
                        dr("vLoginPass") = DecryptPassword(dr("vLoginPass"))
                    End If

                    dr.AcceptChanges()

                Catch ex As Exception

                End Try
            Next
            dt_User.AcceptChanges()

            'ds_UserMst.Tables.Add(dt_User)

            If Not objLambda.Save_InsertUserMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_UserMst, ds_UserMst, 1, estr) Then
                ObjCommon.ShowAlert("Error While Saving UserMst", Me.Page)
                Exit Function
            End If

            ObjCommon.ShowAlert("Record Saved SuccessFully", Me.Page)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....AssignValues")
            Return False
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
#End Region

#Region "Fill LoginName and Events"
    Private Sub fillDDlLoginName()
        Dim ds_login As New DataSet
        Dim dt_login As New DataTable
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty

        If Not Session(S_UserID) = SuperUserId Then
            wstr = "vLocationCode='" + Me.Session(S_LocationCode) + "' AND "
        End If

        wstr += "cStatusIndi <> 'D' Order by vUserName"
        If Not objHelp.GetViewUserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_login, estr) Then
            Me.ShowErrorMessage("Error While Getting Data From UserMst", estr)
            Exit Sub
        End If

        dt_login = ds_login.Tables(0)
        ViewState("dt_login") = dt_login

        dt_login.Columns.Add("DisplayName")
        dt_login.AcceptChanges()

        For Each dr As DataRow In dt_login.Rows
            dr("DisplayName") = Convert.ToString(dr("vUserName")).Trim() + " - " + Convert.ToString(dr("vUserTypeName")).Trim()
        Next dr

        ddlLoginName.DataTextField = "DisplayName"
        ddlLoginName.DataValueField = "iUserId"
        ddlLoginName.DataSource = dt_login
        ddlLoginName.DataBind()
        ddlLoginName.Items.Insert(0, New ListItem("--Select Login Name--", ""))
    End Sub

    Protected Sub ddlLoginName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLoginName.SelectedIndexChanged
        If ddlLoginName.SelectedIndex = 0 Then
            Button1.Enabled = True
        Else
            Button1.Enabled = False
        End If
        Dim dt_login1 As New DataTable
        Dim dv_login As DataView
        Dim passwrd As String = ""
        dt_login1 = CType(ViewState("dt_login"), DataTable)
        dv_login = dt_login1.DefaultView
        dv_login.RowFilter = "iUserId=" & ddlLoginName.SelectedValue
        dt_login1 = dv_login.ToTable()
        passwrd = dt_login1.Rows(0)("vLoginPass")
        ViewState("LoginPass") = passwrd
    End Sub
#End Region

    '===================================================================================================

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click
        AssignValues("E")
    End Sub

    Protected Sub Button4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button4.Click
        AssignValues("D")
    End Sub

    Protected Sub Button5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button5.Click
        Response.Redirect("frmMainPage.aspx")
    End Sub

End Class
