Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.UI.WebControls
Partial Class MasterPage
    Inherits System.Web.UI.MasterPage

#Region "VARIABLE DECLARATION "

    Private objCommon As New clsCommon
    Private Objhelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()

    Private ServerTimeZone As String = ConfigurationSettings.AppSettings("ServerTimezone")
#End Region



#Region "Page Load"

    'Add by shivani pandya
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "assignCSS", "  assignCSS();", True)
        Catch ex As Exception
            Throw New Exception("Error while Page_Init()")
        End Try
    End Sub
    'End

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dsnotice As New DataSet
        Dim estr As String = String.Empty
        Dim ValidRoleOperation As String = String.Empty
        Dim StrUrlcheck As String
        Dim ds_Valid As New DataSet

        Try
            Me.AutoCompleteExtenderForMenu.ContextKey = "vUserTypeCode = '" + Session(S_UserType) + "'"
            Me.hfSession.Value = Session.Timeout
            Me.hfgateway.Value = System.Configuration.ConfigurationManager.AppSettings("DefaultGateway").ToString
            ' Comment By Jeet Patel To change the sequnce to slove object refrence is not set error 
            'hdnSessionUSerID.Value = Me.Session(S_UserID).ToString
            ''Session it will redirect on Login Page with message of session expiration
            'If IsNothing(Session(S_UserID)) Then
            '    Response.Redirect("~/Default.aspx?SessionExpire=true", True)
            'End If
            '====================================================================

            'Session it will redirect on Login Page with message of session expiration

            ' Added By Jeet Patel to change Sequence on 11-May-2015
            If IsNothing(Session(S_UserID)) Then
                Response.Redirect("~/Default.aspx?SessionExpire=true", True)
            End If

            hdnSessionUSerID.Value = Me.Session(S_UserID).ToString
            '====================================================================

            If Not Session(S_ValidUser) = "Yes" Then
                Response.Redirect("~/Default.aspx", True)
            End If

            'For Valid User For Operation Added By Naimesh Dave on 28-Aug-2008
            StrUrlcheck = Me.Request.Url.AbsolutePath
            StrUrlcheck = StrUrlcheck.Substring(StrUrlcheck.LastIndexOf("/") + 1)

            If Not Me.Objhelp.CheckUserRoleOperation(StrUrlcheck, Me.Session(S_UserType).ToString.Trim(), ValidRoleOperation, ds_Valid, estr) Then
                Me.Response.Redirect("~/frmUnderConstruction.aspx", True)
            End If

            If ValidRoleOperation.ToUpper = "NO" Then
                Me.Response.Redirect("~/frmUnderConstruction.aspx", True)
            End If

            '******************************
            Me.lblUserName.Text = Session(S_FirstName).ToString.Trim() + " " + Session(S_LastName).ToString.Trim()
            Me.lblUserName1.Text = Session(S_FirstName).ToString.Trim() + " " + Session(S_LastName).ToString.Trim()

            ''Timezone
            'Dim timeUtc As DateTime = DateTime.UtcNow
            'Dim cstZone As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezoneid)
            '' timezoneid from web.config
            'Dim cstTime As DateTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone)
            Me.lblTime.Text = Session(S_Time).ToString
            'Me.lblTime.Text = Convert.ToString(CDate(Session(S_Time)).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
            'Session(S_Time).ToString("HH:mm") + strServerOffset
            'Convert.ToString(CDate(Session(S_Time)).ToString("dd-MMM-yyyy HH:mm") + strServerOffset) ' + " IST (+5.5 GMT)"
            Me.lblLastLoggedInTime.Text = Session(S_LastLoginDateTime).ToString.Trim()

            If Not Page.IsPostBack Then
                FillProfile() 'Added for "Profile Selection" on 12-March-2010 by Chandresh Vanker
                PopulateMenu()
            Else
                btnContinueWorking_Click(Nothing, Nothing)
                Exit Sub
            End If
            'Add by shivani pandya
            Dim setProfileNew As HttpCookie = New HttpCookie("currentProfile")
            setProfileNew.Value = Me.ddlProfile.SelectedIndex
            Me.Response.Cookies.Add(setProfileNew)

            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "assignCSS", "  assignCSS();", True)
        Catch ex As System.Threading.ThreadAbortException
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

#End Region

#Region "Populate Menu"

    Private Sub PopulateMenu()
        Dim dsMenu As New DataSet
        Dim dvMenu As New DataView
        Try
            ''PopulateUserMenu()

            If Not xmlDataSource1.Data = Nothing Or xmlDataSource1.Data = "" Then
                xmlDataSource1.Data = Nothing
            End If

            dsMenu = GetMenuData(Session(S_UserType))

            'Added on 09-Jul-2009 as suggested by Nikur Sir 
            dvMenu = dsMenu.Tables(0).Copy().DefaultView
            dvMenu.RowFilter = "cOperationType='" & GeneralModule.OpType_BizNetWeb & "'"

            dsMenu = Nothing
            dsMenu = New DataSet
            dsMenu.Tables.Add(dvMenu.ToTable.Copy())
            '**********************************************************

            dsMenu.DataSetName = "Menus"
            dsMenu.Tables(0).TableName = "Menu"
            dsMenu.Relations.Add(New DataRelation("ParentChild", dsMenu.Tables("Menu").Columns("MenuID"), dsMenu.Tables("Menu").Columns("ParentID")))
            dsMenu.Relations(0).Nested = True
            xmlDataSource1.Data = dsMenu.GetXml

        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            If Not IsNothing(dsMenu) Then
                dsMenu.Dispose()
            End If
        End Try
    End Sub

    Private Sub PopulateUserMenu()
        Dim ds_UserMenu As New DataSet
        Dim dv_UserMenu As New DataView
        Dim userMenuString As New StringBuilder()
        Try
            ds_UserMenu = GetMenuData(Session(S_UserType))

            dv_UserMenu = ds_UserMenu.Tables(0).Copy().DefaultView
            dv_UserMenu.RowFilter = "cOperationType='" & GeneralModule.OpType_BizNetWeb & "'"

            ds_UserMenu = Nothing
            ds_UserMenu = New DataSet
            ds_UserMenu.Tables.Add(dv_UserMenu.ToTable.Copy())

            ds_UserMenu.DataSetName = "Menus"
            ds_UserMenu.Tables(0).TableName = "Menu"
            ds_UserMenu.Relations.Add(New DataRelation("ParentChild", ds_UserMenu.Tables("Menu").Columns("MenuID"), ds_UserMenu.Tables("Menu").Columns("ParentID")))
            ds_UserMenu.Relations(0).Nested = True





        Catch ex As Exception
        Finally
        End Try
    End Sub

    Protected Sub menu1_DataBound(sender As Object, e As EventArgs) Handles menu1.DataBound
        Dim userMenuString As New StringBuilder()
        Dim count As Integer = 0
        Try

            userMenuLitral.Text = ""
            menu1.Visible = False
            trMenu.Visible = False

            userMenuString.AppendLine("<div class='contain-to-grid'>")
            userMenuString.AppendLine("<nav class='top-bar' data-topbar='' role='navigation'>")

            userMenuString.AppendLine("<ul class='title-area'>")

            'userMenuString.AppendLine("<li class='name'>")
            'userMenuString.AppendLine("<h1><a href='#'>BizNET</a></h1>")
            'userMenuString.AppendLine("</li>")

            userMenuString.AppendLine("<li class='toggle-topbar menu-icon'>")
            userMenuString.AppendLine("<a href='#'><span>Menu</span></a>")
            userMenuString.AppendLine("</li>")

            userMenuString.AppendLine("</ul>")

            userMenuString.AppendLine("<section class='top-bar-section'>")
            userMenuString.AppendLine("<ul class='left'>")

            For Each lvl1 As MenuItem In menu1.Items
                If lvl1.Text.ToUpper() <> "DashBoard".ToUpper() Then
                    If lvl1.ChildItems.Count > 0 Then
                        userMenuString.AppendLine("<li class='divider'></li>")
                        userMenuString.AppendLine("<li class='has-dropdown not-click level-1'>")
                        userMenuString.AppendLine("<a href='" + lvl1.NavigateUrl + "' class='AspNet-Menu-Link'>" + lvl1.Text + "</a>")
                        userMenuString.AppendLine("<ul class='dropdown m-menu'>")
                        userMenuString.AppendLine("<li class='title back js-generated'>")
                        userMenuString.AppendLine("<h5><a href='javascript:void(0)'>Back</a></h5>")
                        userMenuString.AppendLine("</li>")
                        userMenuString.AppendLine("<li class='parent-link show-for-small-only'><a class='parent-link js-generated' style='text-align: center;'>" + lvl1.Text + "</a></li>")
                        userMenuString.AppendLine("<li>")
                        userMenuString.AppendLine("<div class='row'>")
                        Dim lvl2Count As Integer = 1
                        count = 0
                        For Each lvl2 As MenuItem In lvl1.ChildItems
                            If lvl2.ChildItems.Count > 0 Then

                                If count = 0 Then
                                    count = 1
                                End If

                                If count = 6 Then
                                    count = 1
                                    userMenuString.AppendLine("<div class='medium-3 medium-2 column' style='clear:both;' >")
                                Else
                                    userMenuString.AppendLine("<div class='medium-3 medium-2 column'>")
                                End If

                                'userMenuString.AppendLine("<div class='medium-3 medium-2 column'>")
                                userMenuString.AppendLine("<div class='level-2'>" + lvl2.Text + "</div>")
                                userMenuString.AppendLine("<ul>")
                                For Each lvl3 As MenuItem In lvl2.ChildItems
                                    userMenuString.AppendLine("<li class='level-3'>")
                                    Dim currenturl As String = Request.Url.ToString()
                                    userMenuString.AppendLine("<a href='" + lvl3.NavigateUrl + "' class='AspNet-Menu-Link'><i class=''></i>" + lvl3.Text + "</a>")
                                    userMenuString.AppendLine("</li>")
                                Next
                                count += 1
                                userMenuString.AppendLine("</ul>")
                                userMenuString.AppendLine("</div>")
                            Else
                                'userMenuString.AppendLine("<div class='medium-3 column'>")
                                'userMenuString.AppendLine("<h3>" + lvl2.Text + "</h3>")
                                'userMenuString.AppendLine("</div>")
                                If lvl2Count = 1 Then
                                    userMenuString.AppendLine("<div class='medium-3 medium-2 column'>")
                                    userMenuString.AppendLine("<ul>")
                                    lvl2Count += 1
                                End If
                                userMenuString.AppendLine("<li class='level-2'>")
                                userMenuString.AppendLine("<a href='" + lvl2.NavigateUrl + "' class='AspNet-Menu-Link'><i class=''></i>" + lvl2.Text + "</a>")
                                userMenuString.AppendLine("</li>")
                            End If
                        Next lvl2

                        If lvl2Count = 2 Then
                            userMenuString.AppendLine("</ul>")
                            userMenuString.AppendLine("</div>")
                            lvl2Count += 1
                        End If
                        userMenuString.AppendLine("</div>")
                        userMenuString.AppendLine("<div class='MenuExtraDiv'></div>")

                        userMenuString.AppendLine("</li>")
                        userMenuString.AppendLine("</ul>")
                        userMenuString.AppendLine("</li>")
                    Else
                        userMenuString.AppendLine("<li class='divider'></li>")
                        userMenuString.AppendLine("<li class='level-1'>")
                        userMenuString.AppendLine("<a href='" + lvl1.NavigateUrl + "' class='AspNet-Menu-Link'>" + lvl1.Text + "</a>")
                        userMenuString.AppendLine("</li>")
                    End If
                End If
            Next lvl1

            userMenuString.AppendLine("<li class='divider'></li>")
            userMenuString.AppendLine("</ul>")
            userMenuString.AppendLine("</section>")
            userMenuString.AppendLine("</nav>")
            userMenuString.AppendLine("</div>")
            userMenuLitral.Text = userMenuString.ToString()

        Catch ex As Exception
        Finally
        End Try
    End Sub

#End Region

#Region "Menu Data"

    Public Function GetMenuData(ByVal UserTypeCode As Integer) As DataSet
        Try
            Return Objhelp.GetMenu(UserTypeCode)

        Catch ex As Exception
            Return Nothing
        End Try
    End Function

#End Region

#Region "Fill Profile"

    Private Sub FillProfile()
        Dim dsProfile As New DataSet
        Dim dvProfile As New DataView
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim dsUsers As New DataSet
        Dim dr As DataRow

        Try

            wStr = "cStatusIndi <> 'D' And vUserName = '" + CType(Me.Session(S_UserName), String).Trim() + "'"
            If Not Me.Objhelp.getuserMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            dsUsers, eStr) Then
                Throw New Exception(eStr)
            End If

            If dsUsers Is Nothing Then
                Throw New Exception("User Not Found")
            End If

            wStr = ""
            For Each dr In dsUsers.Tables(0).Rows
                If wStr <> "" Then
                    wStr += ","
                End If
                wStr += "'" + dr("vUserTypeCode").ToString() + "'"
            Next

            If Me.Session(S_Profiles) Is Nothing Then
                Throw New Exception("Profile Not Found")
            End If

            dsProfile.Tables.Add(CType(Me.Session(S_Profiles), DataTable))
            dvProfile = dsProfile.Tables(0).DefaultView
            dvProfile.RowFilter = "vUserTypeCode in(" + wStr + ")"

            Me.ddlProfile.DataSource = dvProfile.ToTable()
            Me.ddlProfile.DataTextField = "vUserTypeName"
            Me.ddlProfile.DataValueField = "vUserTypeCode"
            Me.ddlProfile.DataBind()
            Me.Session(S_Profiles) = dvProfile.ToTable().Copy()

            Me.ddlProfile.Items.FindByValue(Me.Session(S_UserType)).Selected = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
            If Not IsNothing(dsProfile) Then
                dsProfile.Dispose()
            End If
        End Try

    End Sub

#End Region

#Region "Error Handler"
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        'CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        'CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub
#End Region
    Protected Sub ddlProfile_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProfile.SelectedIndexChanged
        Dim wstr As String = String.Empty
        Dim ds_DataEntryControl As DataSet = Nothing
        Dim strRedirect As String = String.Empty
        strRedirect = "~/Logoutpage.aspx?username=" + Me.Session(S_UserName) + "&usertype=" + Me.ddlProfile.SelectedItem.Value.Trim()
        wstr = "select DataEntryControl.vWorkspaceId,DataEntryControl.iNodeId,DataEntryControl.vSubjectId,DataEntryControl.imodifyBy," + _
                                "iWorkFlowStageId,UserMST.vFirstName  + ' '  +UserMST.vLastName as vUserName from DataEntryControl " + _
                                "inner join UserMst on (DataEntryControl.imodifyBy=UserMst.iUserId)   " + _
                                "Where DataEntryControl.imodifyBy='" & Session(S_UserID).ToString & "' "

        ds_DataEntryControl = Objhelp.GetResultSet(wstr, "DataEntryControl")


        If ds_DataEntryControl.Tables(0).Rows.Count > 0 Then
            'objCommon.ShowAlert("Activity is Locked by " + ds_DataEntryControl.Tables(0).Rows(0)("vUserName").ToString + ".", Page)
            objCommon.ShowAlertAndRedirect("Activity is Locked by " + ds_DataEntryControl.Tables(0).Rows(0)("vUserName").ToString + ".", "'frmMainPage.aspx'", Page)
            Me.ddlProfile.SelectedValue = Session(S_UserType).ToString
            Exit Sub
        End If
        Me.Response.Redirect(strRedirect, False)
    End Sub
#Region "Asyncronous PostBack Error Handler"

    Protected Sub ScriptManager1_AsyncPostBackError(ByVal sender As Object, ByVal e As System.Web.UI.AsyncPostBackErrorEventArgs)
        If (e.Exception.Data("ExtraInfo") <> Nothing) Then
            ScriptManager1.AsyncPostBackErrorMessage = _
               e.Exception.Message & _
               e.Exception.Data("ExtraInfo").ToString()
        Else
            ScriptManager1.AsyncPostBackErrorMessage = _
               "An unspecified error occurred."
        End If
    End Sub
#End Region

    Protected Sub btnContinueWorking_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinueWorking.Click
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ResetSessionTimer", "btnContinueWorking_Click();", True)
    End Sub
End Class