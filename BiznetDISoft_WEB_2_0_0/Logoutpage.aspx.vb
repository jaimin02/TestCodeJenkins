
Partial Class Logoutpage
    Inherits System.Web.UI.Page
    Private objCommon As New clsCommon
    Private objHelpDB As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
    Private estr As String = ""
    Private strIpAddress As String = String.Empty
    Private userAgent As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim estr As String = ""
        Dim di As DirectoryInfo
        Dim Path As String = ""
        Dim fi() As FileInfo
        Dim index As Integer = 0
        Dim indexDir As Integer = 0
        Dim diChild() As DirectoryInfo
        Dim wstr As String = String.Empty

        Dim dsUserLoginHistory As New DataSet
        Dim dtUserLoginHistory As New DataTable
        Dim dsUserLoginDetails As New DataSet
        Dim eStr_Retu As String = String.Empty

        Try
            ' Change By Jeet Patel To Restrict user Login Functionality Name wise on 08-May-2015
            'If Not objCommon.DeleteUserLoginDetails(Me.Session(GeneralModule.S_UserName), estr) Then
            '    Me.objCommon.ShowAlert(estr, Me.Page)
            'End If

            ' Added By Jeet Patel on 25-Jun-2015 to Solve Logout Functionality Bug
            If IsNothing(Session(S_UserID)) Then
                Response.Redirect("~/Default.aspx?SessionExpire=true", True)
            End If

            If Me.Session(S_Login).ToString = "2" Then
                If Not objCommon.DeleteUserLoginDetails(Me.Session(GeneralModule.S_UserName), estr) Then
                    Me.objCommon.ShowAlert(estr, Me.Page)
                End If
            ElseIf Me.Session(S_Login).ToString = "1" Then

                wstr = "vUserName='" + Me.Session(S_UserName).ToString + "' AND vIPAddress ='" + Me.Session(S_IpAddress).ToString + "'"

                If Not objHelpDB.View_Userlogindetails(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                       dsUserLoginDetails, eStr_Retu) Then
                    eStr_Retu = "Error while retrieving User Login Details" + vbCrLf + eStr_Retu
                    Exit Sub
                End If

                If dsUserLoginDetails.Tables(0).Rows.Count > 0 Then
                    If Not objCommon.DeleteUserLoginDetails(Me.Session(GeneralModule.S_UserName), estr) Then
                        Me.objCommon.ShowAlert(estr, Me.Page)
                    End If
                Else
                    Me.Response.Redirect("Default.aspx", True)
                    Exit Sub
                End If

            End If
            ' ==============================================================

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


            strIpAddress = Me.Request.Headers("HTTP_X_FORWARDED_FOR")
            If strIpAddress Is Nothing OrElse strIpAddress = String.Empty Then
                strIpAddress = Request.ServerVariables("REMOTE_ADDR")
            End If

            If Me.AssignValueForLoginHistory(dtUserLoginHistory) Then
                dsUserLoginHistory.Tables.Clear()
                dsUserLoginHistory.Tables.Add(dtUserLoginHistory)
                ' ''  commented by ketan muliya  -> double entry restricted in history table.
                'If Not objLambda.Save_UserLoginHistory(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                '                                      dsUserLoginHistory, Me.Session(S_UserID), estr) Then
                '    Me.objCommon.ShowAlert(estr, Me.Page)
                'End If
            End If

            '**************************************

            'Temporary files and Directories deleting code - Added by Chandresh Vanker on 06-July-2009

            Path = Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings("TempSubjectProofDetails"))
            Path += CType(Me.Session(S_UserID), String)

            di = New DirectoryInfo(Path)

            If di.Exists() Then

                diChild = di.GetDirectories()
                For indexDir = 0 To di.GetDirectories.Length - 1

                    fi = diChild(indexDir).GetFiles()
                    For index = 0 To fi.Length - 1
                        fi(index).Delete()
                    Next index
                    'diChild(indexDir).Delete()

                Next indexDir

                'di.Delete()
            End If

            fi = Nothing
            di = Nothing
            '+++++++++++++++For deleting the cookie on logout
            If Not Response.Cookies("UserSingleLogon") Is Nothing Then
                Response.Cookies("UserSingleLogon").Expires = DateTime.Now.AddDays(-1)
            End If

            '*********************************************

            'Added for "Profile Selection" on 12-March-2010 by Chandresh Vanker
            If Me.Request.QueryString("username") <> Nothing AndAlso _
                            Me.Request.QueryString("usertype") <> Nothing Then
                Dim strRedirect As String = String.Empty
                strRedirect = "Default.aspx?username=" + Convert.ToString(Me.Request.QueryString("username")) + "&usertype=" + Convert.ToString(Me.Request.QueryString("usertype"))
                Me.Response.Redirect(strRedirect, True)
                Exit Sub
            End If
            '**********************
          

            Me.Session.Clear()
            Me.Response.Redirect("Default.aspx", True)

        Catch ex As OperationAbortedException
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ex.Message + " Problem in Page_Load "
        Catch ex As Exception
            Me.objCommon.ShowAlert(ex.Message, Me.Page)
        End Try

    End Sub

    Private Function AssignValueForLoginHistory(ByRef dtUserLoginDetails As DataTable) As Boolean
        AssignValueForLoginHistory = True
        Dim dsUserLoginHistory As New DataSet
        Dim dsUserLoginDetails As New DataSet
        Dim wStr_UserLoginHistory As String = String.Empty
        Dim dr As DataRow
        Dim wstr As String = String.Empty
        Dim eStr_Retu As String = ""
        Dim strServerDateTime As String = String.Empty
        Try
            If Not objHelpDB.getUserLoginHistory(wStr_UserLoginHistory, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                                 dsUserLoginHistory, estr) Then
                Throw New Exception(estr)
            End If
           
            strServerDateTime = objHelpDB.GetServerDateTime()
            dtUserLoginDetails = New DataTable
            dtUserLoginDetails = dsUserLoginHistory.Tables(0).Clone()
            dtUserLoginDetails.TableName = dsUserLoginHistory.Tables(0).TableName
            dtUserLoginDetails.AcceptChanges()

            dr = dtUserLoginDetails.NewRow

            dr("iUserId") = Me.Session(S_UserID)
            dr("cLOFlag") = "O"
            dr("dInOutDateTime") = strServerDateTime
            dr("iModifyBy") = Me.Session(S_UserID)
            dr("cStatusIndi") = "N"
            dr("vIPAddress") = strIpAddress
            dr("vUserAgent") = userAgent
            dtUserLoginDetails.Rows.Add(dr)

            dtUserLoginDetails.AcceptChanges()
        Catch ex As Exception
            AssignValueForLoginHistory = False
        End Try
    End Function

End Class
