
Partial Class frmUserReportManagement
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()

    Private Const vs_actuser As String = "dtactuser"
    Private Const vs_cntuser As String = "dtcntuser"
    Private Const vs_userhistory As String = "dtuserhistory"

    Private Const GVActive_UserName As Integer = 0
    Private Const GVActive_UsertypeName As Integer = 1
    Private Const GVActive_LoginDateTime As Integer = 2
    Private Const GVActive_IPAddress As Integer = 3
    Private Const GVActive_vUTCHourDiff As Integer = 4

    Private Const GVHistory_UserName As Integer = 0
    Private Const GVHistory_UsertypeName As Integer = 1
    Private Const GVHistory_LOFlag As Integer = 2
    Private Const GVHistory_InOutDateTime1 As Integer = 3
    Private Const GVHistory_IPAddress As Integer = 4

    Private Const GVFailure_UserName As Integer = 0
    Private Const GVFailure_LastFailedLogin As Integer = 1
    Private Const GVFailure_IPAddress As Integer = 2

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_Id As Integer = 1
    Private Const GVC_UserName As Integer = 2
    Private Const GVC_ScopeNo As Integer = 3
    Private Const GVC_UserGroupCode As Integer = 4
    Private Const GVC_UserGroupName As Integer = 5
    Private Const GVC_FirstName As Integer = 6
    Private Const GVC_LastName As Integer = 7
    Private Const GVC_LoginName As Integer = 8
    Private Const GVC_LoginPass As Integer = 9
    Private Const GVC_UserTypeCode As Integer = 10
    Private Const GVC_UserTypeName As Integer = 11
    Private Const GVC_DeptCode As Integer = 12
    Private Const GVC_DeptName As Integer = 13
    Private Const GVC_LocationCode As Integer = 14
    Private Const GVC_LocationName As Integer = 15
    Private Const GVC_EmailId As Integer = 16
    Private Const GVC_PhoneNo As Integer = 17
    Private Const GVC_ExtNo As Integer = 18
    Private Const GVC_Remark As Integer = 19
    'Private Const GVC_Edit As Integer = 20
    Private Const GVC_ModifyOn As Integer = 20
    Private Const GVC_ModifyBy As Integer = 21

    Dim reportingdate As String = String.Empty
    Dim replaceoffset As String = String.Empty

    Dim activesessions As String = "Active Sessions"
    Dim userloginhistory As String = "User Login History"
    Dim activeuser As String = "Active User"
    Dim inactiveuser As String = "Inactive User"
    Dim alluser As String = "All User"
    Dim blockeduserhistory As String = "Blocked User History"
    Dim userprofile As String = "User Profile :"
    Dim username As String = "User Name :"

#End Region

#Region "Form Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.lbltitle.Visible = False
            Me.pnlactivesession.Visible = False
            Me.Pnlalluser.Visible = False
            Me.Pnluserhistory.Visible = False
            Me.Pnluserfailure.Visible = False
            Me.trDepartment.Visible = False
            Me.trScope.Visible = False
            Me.trDate.Visible = False
            'Me.LblScope.Visible = False
            'Me.DDLScope.Visible = False
            If Not IsPostBack Then
                GenCall()
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "......Page_Load")
        End Try
    End Sub

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Dim eStr As String = String.Empty
        Dim ds As New DataSet


        Try
            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If
            GenCall = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...GenCall")

        End Try
    End Function

#End Region

#Region "GenCall_Data"

    Private Function GenCall_Data() As Boolean
        Exit Function
    End Function

#End Region

#Region "GenCall_ShowUI"

    Private Function GenCall_ShowUI() As Boolean
        Try
            Page.Title = ":: User Report Management  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "User Report Management"

            If Not FillDropDownActSessions() Then
                Exit Function
            End If

            If Not FillDropDownActiveUser() Then
                Exit Function
            End If
            Me.lblprofile.Text = userprofile
            'If Not FillGrid() Then
            '    Exit Function
            'End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....GenCall_ShowUI")

        End Try

    End Function

#End Region

#Region "Fill Control"

    Private Function FillGrid() As Boolean
        Dim dsUserLoginDetails As New DataSet
        Dim dv_cntuser As New Data.DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim wstr1 As String = String.Empty
        Try

            If Not DDlUserName.SelectedIndex = 0 Then
                wstr1 = "AND vUserName = '" + DDlUserName.SelectedItem.Text + "'"
            End If


            If DDLUserType.SelectedIndex = 0 And DDlUserName.SelectedIndex = 0 Then
                If Not objHelp.View_Userlogindetails("1=1 order by dLoginDateTime desc", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                   dsUserLoginDetails, estr) Then
                    Throw New Exception(estr)
                End If
            ElseIf DDLUserType.SelectedIndex = 0 Then
                If Not objHelp.View_Userlogindetails("1=1" + wstr1 + "order by dLoginDateTime desc", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                   dsUserLoginDetails, estr) Then
                    Throw New Exception(estr)
                End If
            Else
                wstr = "vUserTypeName= '" + DDLUserType.SelectedItem.Text + "'" + wstr1 + "order by dLoginDateTime desc"
                If Not objHelp.View_Userlogindetails(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                   dsUserLoginDetails, estr) Then
                    Throw New Exception(estr)
                End If
            End If
            Me.lblprofile.Text = userprofile
            Me.lbltitle.Text = activesessions
            If dsUserLoginDetails.Tables(0).Rows.Count > 0 Then
                dv_cntuser = dsUserLoginDetails.Tables(0).DefaultView()
                'dv_cntuser.Sort = "dloginDateTime"
                Me.ViewState(vs_cntuser) = dv_cntuser.Table
                Me.GVcntuser.DataSource = dv_cntuser
                Me.GVcntuser.DataBind()
                Me.pnlactivesession.Visible = True
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "UIgvmedexRefresh", "UIgvmedexRefresh();", True)

            Else
                objcommon.ShowAlert("No Current User available", Me)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillGrid")

        End Try
    End Function

    Private Function fillcurrentdate() As Boolean
        Try
            Dim FromDateOfOperationalKpi As String = String.Empty
            Dim LastdayOfPpreviousMonth As String = String.Empty
            Dim ToDateOfOperationalKpi As String = String.Empty
            FromDateOfOperationalKpi = DateTime.Now.Day.ToString() & "-" & MonthName(DateTime.Now.Month, True) & "-" & DateTime.Now.Year
            ToDateOfOperationalKpi = DateTime.Now.Day.ToString() & "-" & MonthName(DateTime.Now.Month, True) & "-" & DateTime.Now.Year
            TxtFromDateOfuserhistory.Text = FromDateOfOperationalKpi
            TxtToDateOfuserhistory.Text = ToDateOfOperationalKpi
            TxtFromDateOfuserhistory.Enabled = True
            TxtToDateOfuserhistory.Enabled = True
            LblFromDateForuserhistory.Enabled = True
            LblToDateForuserhistory.Enabled = True
            'BtnGoForuserhistory.Enabled = True
            fillcurrentdate = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....fillcurrentdate")
            fillcurrentdate = False
        End Try

    End Function

    Private Function FillUserLoginhistoryGrid() As Boolean
        Dim dsUserHistory As New DataSet
        Dim dv_Userhistory As New Data.DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim wstr1 As String = String.Empty


        If Not DDlUserName.SelectedIndex = 0 Then
            wstr1 = "AND vUserName = '" + DDlUserName.SelectedItem.Text + "'"
        End If

        If DDLUserType.SelectedIndex = 0 And DDlUserName.SelectedIndex = 0 Then
            wstr = "CONVERT(smalldatetime,dInOutDateTime) between CONVERT(smalldatetime,'" + TxtFromDateOfuserhistory.Text + " 00:00') and CONVERT(smalldatetime,'" + TxtToDateOfuserhistory.Text + " 23:59') order by nUserLoginHistoryNo  desc"
            'wstr = "dInOutDateTime between '" + TxtFromDateOfuserhistory.Text + "' and '" + TxtToDateOfuserhistory.Text + " 23:59:59:999' order by dInOutDateTime desc "
        ElseIf DDLUserType.SelectedIndex = 0 Then
            wstr = "CONVERT(smalldatetime,dInOutDateTime) between CONVERT(smalldatetime,'" + TxtFromDateOfuserhistory.Text + " 00:00') and CONVERT(smalldatetime,'" + TxtToDateOfuserhistory.Text + " 23:59') " + wstr1 + " order by nUserLoginHistoryNo desc"
            'wstr = "dInOutDateTime between '" + TxtFromDateOfuserhistory.Text + "' and '" + TxtToDateOfuserhistory.Text + " 23:59:59:999'" + wstr1 + " order by dInOutDateTime desc "
        Else
            wstr = "vUserTypeName= '" + DDLUserType.SelectedItem.Text + "'" + wstr1 + "AND CONVERT(smalldatetime,dInOutDateTime) between CONVERT(smalldatetime,'" + TxtFromDateOfuserhistory.Text + " 00:00') and CONVERT(smalldatetime,'" + TxtToDateOfuserhistory.Text + " 23:59') order by nUserLoginHistoryNo desc"
            'wstr = "vUserTypeName= '" + DDLUserType.SelectedItem.Text + "'" + wstr1 + " and dInOutDateTime between '" + TxtFromDateOfuserhistory.Text + "' and '" + TxtToDateOfuserhistory.Text + " 23:59:59:999' order by dInOutDateTime desc "
        End If
        Try

            If Not objHelp.view_UserLoginHistory(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                               dsUserHistory, estr) Then
                Throw New Exception(estr)
            End If

            Me.lblprofile.Text = userprofile
            Me.lbltitle.Text = userloginhistory
            If dsUserHistory.Tables(0).Rows.Count > 0 Then
                dv_Userhistory = dsUserHistory.Tables(0).DefaultView()
                Me.ViewState(vs_userhistory) = dv_Userhistory.Table
                Me.GVuserhistory.DataSource = dv_Userhistory
                Me.GVuserhistory.DataBind()
                Me.GVuserhistory.PageIndex = 0
                Me.Pnluserhistory.Visible = True
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "GVuserhistory", "GVuserhistory();", True)
            Else
                Me.GVuserhistory.DataSource = Nothing
                Me.GVuserhistory.DataBind()
                objcommon.ShowAlert("No History Of User Available", Me)

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillUserLoginhistoryGrid")

        End Try

    End Function

    Private Function FillActiveUserGrid() As Boolean
        Dim ds_actuser As New Data.DataSet
        Dim dv_actuser As New Data.DataView
        Dim wstr As String = String.Empty
        Dim wstr1 As String = String.Empty
        Dim estr As String = String.Empty

        If Not DDlUserName.SelectedIndex = 0 Then
            wstr1 = " AND vUserName='" + DDlUserName.SelectedItem.Text + "'"
        End If

        If Not DDLDepartment.SelectedIndex = 0 Then
            wstr1 += " AND vDeptName='" + DDLDepartment.SelectedItem.Text + "'"
        End If

        If DDLUserType.SelectedIndex = 0 Then
            wstr = "cStatusIndi <> 'D'" + wstr1 + " order by vUserName"
        Else
            wstr = "vUserTypeName='" + DDLUserType.Text + "'" + wstr1 + " and cStatusIndi <> 'D' order by vUserName"
        End If

        Try
            If Not objHelp.GetViewUserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_actuser, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From ViewUserMst", estr)
                Exit Function
            End If

            If ds_actuser.Tables(0).Rows.Count < 1 Then
                objcommon.ShowAlert("No Record Found", Me.Page)
                Exit Function
            End If
            Me.lblprofile.Text = userprofile
            Me.lbltitle.Text = activeuser
            dv_actuser = ds_actuser.Tables(0).DefaultView()
            Me.ViewState(vs_actuser) = dv_actuser.Table
            Me.GV_User.DataSource = dv_actuser
            Me.GV_User.DataBind()
            Me.Pnlalluser.Visible = True

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "UIGVUser", "UIGVUser();", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillActiveUserGrid")
        End Try
    End Function

    Private Function FillInactiveUserGrid() As Boolean
        Dim ds_inactuser As New Data.DataSet
        Dim dv_inactuser As New Data.DataView
        Dim wstr As String = String.Empty
        Dim wstr1 As String = String.Empty
        Dim estr As String = String.Empty

        If Not DDLDepartment.SelectedIndex = 0 Then
            wstr1 += " AND vDeptName='" + DDLDepartment.SelectedItem.Text + "'"
        End If

        If DDLUserType.SelectedIndex = 0 Then
            wstr = "cStatusIndi = 'D'" + wstr1 + "order by vUserName"
        Else
            wstr = "vUserTypeName='" + DDLUserType.Text + "'" + wstr1 + "and cStatusIndi = 'D' order by vUserName"
        End If

        Try
            If Not objHelp.GetViewUserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_inactuser, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From ViewUserMst", estr)
                Exit Function
            End If

            If ds_inactuser.Tables(0).Rows.Count < 1 Then
                objcommon.ShowAlert("No Record Found", Me.Page)
                Exit Function
            End If
            Me.lblprofile.Text = userprofile
            dv_inactuser = ds_inactuser.Tables(0).DefaultView()
            Me.ViewState(vs_actuser) = dv_inactuser.Table
            Me.lbltitle.Text = inactiveuser
            Me.GV_User.DataSource = dv_inactuser
            Me.GV_User.DataBind()
            Me.Pnlalluser.Visible = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "UIGVUser", "UIGVUser();", True)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillInactiveUserGrid")
        End Try
    End Function

    Private Function FillAllUserGrid() As Boolean
        Dim ds_RoleOp As New Data.DataSet
        Dim dv_User As New Data.DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim wstr1 As String = String.Empty
        Dim lblexceltitle As String = String.Empty
        Try

            If Not DDlUserName.SelectedIndex = 0 Then
                wstr1 = "AND vUserName = '" + DDlUserName.SelectedItem.Text + "'"
            End If

            If Not DDLDepartment.SelectedIndex = 0 Then
                wstr1 += " AND vDeptName = '" + DDLDepartment.SelectedItem.Text + "'"
            End If

            If Not DDLScope.SelectedIndex = 0 Then
                wstr1 += " AND vScopeName = '" + DDLScope.SelectedItem.Text + "'"
            End If

            If DDLUserType.SelectedIndex = 0 And DDlUserName.SelectedIndex = 0 And DDLDepartment.SelectedIndex = 0 And DDLScope.SelectedIndex = 0 Then

                If Not objHelp.GetViewUserMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_RoleOp, estr) Then
                    Me.ShowErrorMessage("Error While Getting Data From ViewUserMst", estr)
                    Return False
                End If

            ElseIf DDLUserType.SelectedIndex = 0 Then

                If Not objHelp.GetViewUserMst("1=1" + wstr1 + "", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_RoleOp, estr) Then
                    Me.ShowErrorMessage("Error While Getting Data From ViewUserMst", estr)
                    Return False
                End If

            Else
                wstr = "vUserTypeName='" + DDLUserType.Text + "'" + wstr1 + ""
                If Not objHelp.GetViewUserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_RoleOp, estr) Then
                    Me.ShowErrorMessage("Error While Getting Data From ViewUserMst", estr)
                    Return False
                End If
            End If

            If ds_RoleOp.Tables(0).Rows.Count < 1 Then
                objcommon.ShowAlert("No Record Found", Me.Page)
                Return True
            End If
            Me.lblprofile.Text = userprofile
            Me.lbltitle.Text = alluser
            dv_User = ds_RoleOp.Tables(0).DefaultView()
            dv_User.Sort = "vUserName"
            Me.ViewState(vs_actuser) = dv_User.Table
            Me.GV_User.DataSource = dv_User
            Me.GV_User.DataBind()
            Me.pnlactivesession.Visible = False
            Me.Pnlalluser.Visible = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "UIGVUser", "UIGVUser();", True)

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillAllUserGrid")
            Return False
        End Try
    End Function

    Private Function FillUserLoginfailureGrid() As Boolean
        Dim dsUserfailure As New DataSet
        Dim dv_Userfailure As New Data.DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty

        Try
            If DDLUserType.SelectedIndex = 0 Then
                wstr = "cBlockedFlag = 'b' order by dLastFailedLogin desc"
            Else
                wstr = "vUserName='" + DDLUserType.Text + "' and cBlockedFlag = 'b' order by dLastFailedLogin desc"
            End If

            'If Not objHelp.GetUserLoginFailureDetails(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '                                   dsUserfailure, estr) Then
            '    Throw New Exception(estr)
            'End If

            If Not objHelp.GetView_UserLoginFailureDetails(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsUserfailure, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From View_UserLoginFailureDetails", estr)
                Exit Function
            End If

            Me.lblprofile.Text = username
            Me.lbltitle.Text = blockeduserhistory
            If dsUserfailure.Tables(0).Rows.Count > 0 Then
                dv_Userfailure = dsUserfailure.Tables(0).DefaultView()
                Me.ViewState(vs_userhistory) = dv_Userfailure.Table
                Me.GVuserfailure.DataSource = dv_Userfailure
                Me.GVuserfailure.DataBind()
                Me.Pnluserfailure.Visible = True
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "GVuserfailure", "GVuserfailure();", True)
            Else
                objcommon.ShowAlert("No History Of User Available", Me)

            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillUserLoginfailureGrid")

        End Try
    End Function
#End Region

#Region "FillDropDown"

    Private Function FillDropDownActSessions() As Boolean
        Dim ds_UserType As New Data.DataSet
        Dim dv_UserType As New DataView
        Dim estr As String = String.Empty
        Dim wStr As String = String.Empty
        Try


            If Not Me.objHelp.View_Userlogindetails("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, _
                        ds_UserType, estr) Then
                Me.objcommon.ShowAlert("Error While Getting Data From UserTypeMst : " + estr, Me.Page)
                Exit Function
            End If
            dv_UserType = ds_UserType.Tables(0).DefaultView
            dv_UserType = dv_UserType.ToTable(True, "vUserTypeName").DefaultView()
            dv_UserType.Sort = "vUserTypeName"
            Me.DDLUserType.DataSource = dv_UserType.ToTable()
            Me.DDLUserType.DataTextField = "vUserTypeName"
            Me.DDLUserType.DataBind()
            Me.DDLUserType.Items.Insert(0, New ListItem("All Profile", 0))

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillDropDownActSessions")
            Return False
        End Try
    End Function

    Private Function FillDropDownUserHistory() As Boolean
        Dim ds_UserType As New Data.DataSet
        Dim dv_UserType As New DataView
        Dim estr As String = String.Empty
        Dim wStr As String = String.Empty
        Try


            'If Not Me.objHelp.view_UserLoginHistory("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, _
            '            ds_UserType, estr) Then
            '    Me.objcommon.ShowAlert("Error While Getting Data From UserTypeMst : " + estr, Me.Page)
            '    Exit Function
            'End If

            If Not Me.objHelp.getUserTypeMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, _
                       ds_UserType, estr) Then
                Me.objcommon.ShowAlert("Error While getting Data from UserTypeMst", Me.Page)
                Exit Function
            End If

            dv_UserType = ds_UserType.Tables(0).DefaultView
            dv_UserType = dv_UserType.ToTable(True, "vUserTypeName").DefaultView()
            dv_UserType.Sort = "vUserTypeName"
            Me.DDLUserType.DataSource = dv_UserType.ToTable()
            Me.DDLUserType.DataTextField = "vUserTypeName"
            Me.DDLUserType.DataBind()
            Me.DDLUserType.Items.Insert(0, New ListItem("All Profile", 0))

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillDropDownUserHistory")
            Return False
        End Try
    End Function

    Private Function FillDropDownActUser() As Boolean
        Dim ds_UserType As New Data.DataSet
        Dim dv_UserType As New DataView
        Dim estr As String = String.Empty
        Dim wStr As String = String.Empty
        Try

            If ddlUserStatus.Text = alluser Then
                'If Not Me.objHelp.GetViewUserMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, _
                '            ds_UserType, estr) Then
                '    Me.objcommon.ShowAlert("Error While Getting Data From UserTypeMst : " + estr, Me.Page)
                '    Exit Function
                'End If
                If Not Me.objHelp.getUserTypeMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, _
                           ds_UserType, estr) Then
                    Me.objcommon.ShowAlert("Error While getting Data from UserTypeMst", Me.Page)
                    Exit Function
                End If


            ElseIf ddlUserStatus.Text = activeuser Then
                wStr = "cStatusIndi <> 'D'"
                If Not Me.objHelp.GetViewUserMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_UserType, estr) Then
                    Me.objcommon.ShowAlert("Error While Getting Data From UserTypeMst : " + estr, Me.Page)
                    Exit Function
                End If
            Else
                wStr = "cStatusIndi = 'D'"
                If Not Me.objHelp.GetViewUserMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_UserType, estr) Then
                    Me.objcommon.ShowAlert("Error While Getting Data From UserTypeMst : " + estr, Me.Page)
                    Exit Function
                End If

            End If
            dv_UserType = ds_UserType.Tables(0).DefaultView
            dv_UserType = dv_UserType.ToTable(True, "vUserTypeName").DefaultView()
            dv_UserType.Sort = "vUserTypeName"
            Me.DDLUserType.DataSource = dv_UserType.ToTable()
            Me.DDLUserType.DataTextField = "vUserTypeName"
            Me.DDLUserType.DataBind()
            Me.DDLUserType.Items.Insert(0, New ListItem("All Profile", 0))

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillDropDownActUser")
            Return False
        End Try
    End Function

    Private Function FillDropDownActiveUser() As Boolean
        Dim ds_UserType As New Data.DataSet
        Dim dv_UserType As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty

        Try

            If DDLUserType.SelectedIndex = 0 Then
                If Not objHelp.View_Userlogindetails("1=1 order by dLoginDateTime desc", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                   ds_UserType, estr) Then
                    Throw New Exception(estr)
                End If
            Else
                wstr = "vUserTypeName= '" + DDLUserType.SelectedItem.Text + "'order by dLoginDateTime desc"
                If Not objHelp.View_Userlogindetails(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                   ds_UserType, estr) Then
                    Throw New Exception(estr)
                End If
            End If

            dv_UserType = ds_UserType.Tables(0).DefaultView
            dv_UserType = dv_UserType.ToTable(True, "vUserName").DefaultView()
            dv_UserType.Sort = "vUserName"
            Me.DDlUserName.DataSource = dv_UserType.ToTable()
            Me.DDlUserName.DataTextField = "vUserName"
            Me.DDlUserName.DataBind()
            Me.DDlUserName.Items.Insert(0, New ListItem("All User", 0))

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillDropDownActiveUserName")
            Return False
        End Try

    End Function

    Private Function FillDropDownHisoryUser() As Boolean
        Dim ds_UserType As New Data.DataSet
        Dim dv_UserType As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty

        Try

            If DDLUserType.SelectedIndex = 0 Then
                wstr = "cStatusIndi <> ''"
                'wstr = "dInOutDateTime between '" + TxtFromDateOfuserhistory.Text + "' and '" + TxtToDateOfuserhistory.Text + " 23:59:59:999' order by dInOutDateTime desc "
            Else
                wstr = "vUserTypeName= '" + DDLUserType.SelectedItem.Text + "'"
                ' AND cStatusIndi <> 'D' "
                'wstr = "vUserTypeName= '" + DDLUserType.SelectedItem.Text + "' AND CONVERT(smalldatetime,dInOutDateTime) between CONVERT(smalldatetime,'" + TxtFromDateOfuserhistory.Text + " 00:00') and CONVERT(smalldatetime,'" + TxtToDateOfuserhistory.Text + " 23:59') order by dInOutDateTime"
                'wstr = "vUserTypeName= '" + DDLUserType.SelectedItem.Text + "' and dInOutDateTime between '" + TxtFromDateOfuserhistory.Text + "' and '" + TxtToDateOfuserhistory.Text + " 23:59:59:999' order by dInOutDateTime desc "
            End If

            '' Commented By Jeet Patel on 10-Jun-2015
            'If Not objHelp.view_UserLoginHistory(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '                                       ds_UserType, estr) Then
            '    Throw New Exception(estr)
            'End If

            If Not objHelp.GetViewUserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_UserType, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From ViewUserMst", estr)
                Exit Function
            End If

            dv_UserType = ds_UserType.Tables(0).DefaultView
            dv_UserType = dv_UserType.ToTable(True, "vUserName").DefaultView()
            dv_UserType.Sort = "vUserName"
            Me.DDlUserName.DataSource = dv_UserType.ToTable()
            Me.DDlUserName.DataTextField = "vUserName"
            Me.DDlUserName.DataBind()
            Me.DDlUserName.Items.Insert(0, New ListItem("All User", 0))

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillDropDownActiveUserName")
            Return False
        End Try

    End Function

    Private Function FillActiveUser() As Boolean
        Dim ds_actUser As New Data.DataSet
        Dim dv_UserType As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty

        Try

            If DDLUserType.SelectedIndex = 0 Then
                wstr = "cStatusIndi <> 'D' order by vUserName"
            Else
                wstr = "vUserTypeName='" + DDLUserType.Text + "' and cStatusIndi <> 'D' order by vUserName"
            End If

            If Not objHelp.GetViewUserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_actUser, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From ViewUserMst", estr)
                Exit Function
            End If

            dv_UserType = ds_actUser.Tables(0).DefaultView
            dv_UserType = dv_UserType.ToTable(True, "vUserName").DefaultView()
            dv_UserType.Sort = "vUserName"
            Me.DDlUserName.DataSource = dv_UserType.ToTable()
            Me.DDlUserName.DataTextField = "vUserName"
            Me.DDlUserName.DataBind()
            Me.DDlUserName.Items.Insert(0, New ListItem("All User", 0))

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillDropDownActiveUserName")
            Return False
        End Try

    End Function

    Private Function FillActiveUserDepartment() As Boolean
        Dim ds_actUser As New Data.DataSet
        Dim dv_UserType As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim wstr1 As String = String.Empty

        Try

            If Not DDlUserName.SelectedIndex = 0 Then
                wstr1 = " AND vUserName = '" + DDlUserName.SelectedItem.Text + "'"
            End If

            If DDLUserType.SelectedIndex = 0 And DDlUserName.SelectedIndex = 0 Then
                wstr = "cStatusIndi <> 'D' order by vUserName"
            ElseIf DDLUserType.SelectedIndex = 0 Then
                wstr = "cStatusIndi <> 'D'" + wstr1 + "order by vUserName"
            Else
                wstr = "vUserTypeName='" + DDLUserType.Text + "'" + wstr1 + " and cStatusIndi <> 'D' order by vUserName"
            End If

            If Not objHelp.GetViewUserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_actUser, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From ViewUserMst", estr)
                Exit Function
            End If

            dv_UserType = ds_actUser.Tables(0).DefaultView
            dv_UserType = dv_UserType.ToTable(True, "vDeptName").DefaultView()
            dv_UserType.Sort = "vDeptName"
            Me.DDLDepartment.DataSource = dv_UserType.ToTable()
            Me.DDLDepartment.DataTextField = "vDeptName"
            Me.DDLDepartment.DataBind()
            Me.DDLDepartment.Items.Insert(0, New ListItem("All Department", 0))

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillDropDownActiveUserName")
            Return False
        End Try
    End Function

    Private Function FillInactiveDepartment() As Boolean
        Dim ds_actUser As New Data.DataSet
        Dim dv_UserType As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty

        Try

            If DDLUserType.SelectedIndex = 0 Then
                wstr = "cStatusIndi = 'D' order by vUserName"
            Else
                wstr = "vUserTypeName='" + DDLUserType.Text + "' and cStatusIndi = 'D' order by vUserName"
            End If

            If Not objHelp.GetViewUserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_actUser, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From ViewUserMst", estr)
                Exit Function
            End If

            dv_UserType = ds_actUser.Tables(0).DefaultView
            dv_UserType = dv_UserType.ToTable(True, "vDeptName").DefaultView()
            dv_UserType.Sort = "vDeptName"
            Me.DDLDepartment.DataSource = dv_UserType.ToTable()
            Me.DDLDepartment.DataTextField = "vDeptName"
            Me.DDLDepartment.DataBind()
            Me.DDLDepartment.Items.Insert(0, New ListItem("All Department", 0))

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillDropDownActiveUserName")
            Return False
        End Try
    End Function

    Private Function FillAllUserName() As Boolean
        Dim ds_allUser As New Data.DataSet
        Dim dv_UserType As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty

        Try

            If DDLUserType.SelectedIndex = 0 Then

                If Not objHelp.GetViewUserMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_allUser, estr) Then
                    Me.ShowErrorMessage("Error While Getting Data From ViewUserMst", estr)
                    Return False
                End If

            Else
                wstr = "vUserTypeName='" + DDLUserType.Text + "'"
                If Not objHelp.GetViewUserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_allUser, estr) Then
                    Me.ShowErrorMessage("Error While Getting Data From ViewUserMst", estr)
                    Return False
                End If
            End If

            dv_UserType = ds_allUser.Tables(0).DefaultView
            dv_UserType = dv_UserType.ToTable(True, "vUserName").DefaultView()
            dv_UserType.Sort = "vUserName"
            Me.DDlUserName.DataSource = dv_UserType.ToTable()
            Me.DDlUserName.DataTextField = "vUserName"
            Me.DDlUserName.DataBind()
            Me.DDlUserName.Items.Insert(0, New ListItem("All User", 0))

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillDropDownActiveUserName")
            Return False
        End Try
    End Function

    Private Function FillAllDepartment() As Boolean
        Dim ds_allUser As New Data.DataSet
        Dim dv_UserType As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim wstr1 As String = String.Empty

        Try
            If Not DDlUserName.SelectedIndex = 0 Then
                wstr1 = "AND vUserName = '" + DDlUserName.SelectedItem.Text + "'"
            End If

            If DDLUserType.SelectedIndex = 0 And DDlUserName.SelectedIndex = 0 Then

                If Not objHelp.GetViewUserMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_allUser, estr) Then
                    Me.ShowErrorMessage("Error While Getting Data From ViewUserMst", estr)
                    Return False
                End If

            ElseIf DDLUserType.SelectedIndex = 0 Then

                If Not objHelp.GetViewUserMst("1=1" + wstr1 + "", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_allUser, estr) Then
                    Me.ShowErrorMessage("Error While Getting Data From ViewUserMst", estr)
                    Return False
                End If

            Else
                wstr = "vUserTypeName='" + DDLUserType.Text + "'" + wstr1
                If Not objHelp.GetViewUserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_allUser, estr) Then
                    Me.ShowErrorMessage("Error While Getting Data From ViewUserMst", estr)
                    Return False
                End If
            End If

            dv_UserType = ds_allUser.Tables(0).DefaultView
            dv_UserType = dv_UserType.ToTable(True, "vDeptName").DefaultView()
            dv_UserType.Sort = "vDeptName"
            Me.DDLDepartment.DataSource = dv_UserType.ToTable()
            Me.DDLDepartment.DataTextField = "vDeptName"
            Me.DDLDepartment.DataBind()
            Me.DDLDepartment.Items.Insert(0, New ListItem("All Department", 0))

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillDropDownActiveUserName")
            Return False
        End Try
    End Function

    Private Function FillAllScope() As Boolean
        Dim ds_allUser As New Data.DataSet
        Dim dv_UserType As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim wstr1 As String = String.Empty

        Try
            If Not DDlUserName.SelectedIndex = 0 Then
                wstr1 = "AND vUserName = '" + DDlUserName.SelectedItem.Text + "'"
            End If

            If Not DDLDepartment.SelectedIndex = 0 Then
                wstr1 += " AND vDeptName = '" + DDLDepartment.SelectedItem.Text + "'"
            End If

            If DDLUserType.SelectedIndex = 0 And DDlUserName.SelectedIndex = 0 And DDLDepartment.SelectedIndex = 0 Then

                If Not objHelp.GetViewUserMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_allUser, estr) Then
                    Me.ShowErrorMessage("Error While Getting Data From ViewUserMst", estr)
                    Return False
                End If

            ElseIf DDLUserType.SelectedIndex = 0 Then

                If Not objHelp.GetViewUserMst("1=1" + wstr1 + "", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_allUser, estr) Then
                    Me.ShowErrorMessage("Error While Getting Data From ViewUserMst", estr)
                    Return False
                End If

            Else
                wstr = "vUserTypeName='" + DDLUserType.Text + "'" + wstr1
                If Not objHelp.GetViewUserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_allUser, estr) Then
                    Me.ShowErrorMessage("Error While Getting Data From ViewUserMst", estr)
                    Return False
                End If
            End If

            dv_UserType = ds_allUser.Tables(0).DefaultView
            dv_UserType = dv_UserType.ToTable(True, "vScopeName").DefaultView()
            dv_UserType.Sort = "vScopeName"
            Me.DDLScope.DataSource = dv_UserType.ToTable()
            Me.DDLScope.DataTextField = "vScopeName"
            Me.DDLScope.DataBind()
            Me.DDLScope.Items.Insert(0, New ListItem("All Scope", 0))

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillDropDownActiveUserName")
            Return False
        End Try
    End Function

    Private Function FillDropDownBlockUser() As Boolean
        Dim ds_UserType As New Data.DataSet
        Dim dv_UserType As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = "cBlockedFlag = 'b'"
        Try


            'If Not objHelp.GetUserLoginFailureDetails(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '                                   ds_UserType, estr) Then
            '    Throw New Exception(estr)
            'End If


            If Not objHelp.GetView_UserLoginFailureDetails(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_UserType, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From View_UserLoginFailureDetails", estr)
                Exit Function
            End If

            dv_UserType = ds_UserType.Tables(0).DefaultView
            dv_UserType = dv_UserType.ToTable(True, "vUserName").DefaultView()
            dv_UserType.Sort = "vUserName"
            Me.DDLUserType.DataSource = dv_UserType.ToTable()
            Me.DDLUserType.DataTextField = "vUserName"
            Me.DDLUserType.DataBind()
            Me.DDLUserType.Items.Insert(0, New ListItem("All User", 0))

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillDropDownBlockUser")
            Return False
        End Try
    End Function

#End Region

#Region "Button Events"

    'Protected Sub BtnGoForuserhistory_Click()  Commented By Jeet To Fill All User Name at Selecting User Login History
    '    FillDropDownHisoryUser()
    '    Me.trDate.Visible = True
    '    'This code is shifted to  FillloginHistoryGrid
    'End Sub

    ' Added By Jeet Patel on 25-May-2015 as Per CR in version 4.3
    Protected Sub BtnGO_Click() Handles BtnGO.Click

        Me.lbltitle.Visible = True

        If ddlUserStatus.SelectedItem.Text = activesessions Then
            FillGrid()
            Me.pnlactivesession.Visible = True

        ElseIf ddlUserStatus.SelectedItem.Text = userloginhistory Then
            Me.trDate.Visible = True
            FillUserLoginhistoryGrid()
            'Me.Pnluserhistory.Visible = True

        ElseIf ddlUserStatus.SelectedItem.Text = activeuser Then
            Me.trDepartment.Visible = True
            FillActiveUserGrid()
            Me.Pnlalluser.Visible = True

        ElseIf ddlUserStatus.SelectedItem.Text = inactiveuser Then
            Me.trDepartment.Visible = True
            FillInactiveUserGrid()
            Me.Pnlalluser.Visible = True

        ElseIf ddlUserStatus.SelectedItem.Text = alluser Then
            Me.trDepartment.Visible = True
            Me.trScope.Visible = True
            FillAllUserGrid()
            Me.Pnlalluser.Visible = True

        ElseIf ddlUserStatus.SelectedItem.Text = blockeduserhistory Then
            FillUserLoginfailureGrid()
            Me.Pnluserfailure.Visible = True

        End If


        '============================================================
    End Sub

    Protected Sub btnexclcntuser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnexclcntuser.Click

        Dim fileName As String = String.Empty
        Dim dscntuser As New DataSet
        Try

            If Me.GVcntuser.Rows.Count < 1 Then
                Me.objcommon.ShowAlert("No Data To Export", Me.Page)
                Exit Sub
            End If
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            fileName = "Active Sessions Report"
            fileName = fileName & ".xls"
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            dscntuser.Tables.Add(CType(Me.ViewState(vs_cntuser), DataTable).Copy())
            dscntuser.AcceptChanges()

            Context.Response.Write(ConvertDscntuserTO(dscntuser))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception
            'Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnexportexcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnexportexcel.Click
        Dim fileName As String = String.Empty
        Dim ds As New DataSet
        Try

            If Me.GV_User.Rows.Count < 1 Then
                Me.objcommon.ShowAlert("No Data To Export", Me.Page)
                Exit Sub
            End If
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            If ddlUserStatus.SelectedItem.Text = activeuser Then
                fileName = "Active User Report"
            ElseIf ddlUserStatus.SelectedItem.Text = inactiveuser Then
                fileName = "Inactive User Report"
            ElseIf ddlUserStatus.SelectedItem.Text = alluser Then
                fileName = "All User Report"
            End If

            fileName = fileName & ".xls"
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            ds.Tables.Add(CType(Me.ViewState(vs_actuser), DataTable).Copy())
            ds.AcceptChanges()

            Context.Response.Write(ConvertDsTO(ds))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception
            'Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnexptuserhistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnexptuserhistory.Click
        Dim fileName As String = String.Empty
        Dim dsuserhistory As New DataSet
        Try

            If Me.GVuserhistory.Rows.Count < 1 Then
                Me.objcommon.ShowAlert("No Data To Export", Me.Page)
                Exit Sub
            End If
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            fileName = "User History Report"
            fileName = fileName & ".xls"
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            dsuserhistory.Tables.Add(CType(Me.ViewState(vs_userhistory), DataTable).Copy())
            dsuserhistory.AcceptChanges()

            Context.Response.Write(ConvertDsuserTO(dsuserhistory))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....btnexptuserhistory_Click")
        End Try
    End Sub

    Protected Sub btnexptuserfailure_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnexptuserfailure.Click
        Dim fileName As String = String.Empty
        Dim dsuserfailure As New DataSet
        Try

            If Me.GVuserfailure.Rows.Count < 1 Then
                Me.objcommon.ShowAlert("No Data To Export", Me.Page)
                Exit Sub
            End If
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            fileName = "User Failure Report"
            fileName = fileName & ".xls"
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            dsuserfailure.Tables.Add(CType(Me.ViewState(vs_userhistory), DataTable).Copy())
            dsuserfailure.AcceptChanges()

            Context.Response.Write(ConvertDsuserfailureTO(dsuserfailure))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....btnexptuserfailure_Click")
        End Try
    End Sub


#End Region

#Region "Export To Excel for Active Sessions"
    Private Function ConvertDscntuserTO(ByVal ds As DataSet) As String
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer
        Dim dsConvert As New DataSet

        Try

            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""4""><center><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")


            strMessage.Append("Active Sessions")
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")

            dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vUserName,vUsertypeName,tmp_dLoginDateTime,vIPAddress".Split(",")).DefaultView.ToTable())
            dsConvert.AcceptChanges()
            dsConvert.Tables(0).Columns(0).ColumnName = "User Name"
            dsConvert.Tables(0).Columns(1).ColumnName = "User Group"
            dsConvert.Tables(0).Columns(2).ColumnName = "Login Time"
            dsConvert.Tables(0).Columns(3).ColumnName = "IP Address"
            dsConvert.AcceptChanges()

            For iCol = 0 To dsConvert.Tables(0).Columns.Count - 1

                strMessage.Append("<td><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).Trim())
                strMessage.Append("</font></strong></td>")

            Next
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")

            For j = 0 To dsConvert.Tables(0).Rows.Count - 1
                strMessage.Append("<tr>")
                For i = 0 To dsConvert.Tables(0).Columns.Count - 1

                    strMessage.Append("<td><font color=""#000000"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim())
                    strMessage.Append("</font></td>")

                Next
                strMessage.Append("</tr>")
            Next
            strMessage.Append("</table>")

            Return strMessage.ToString

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....ConvertDscntuserTO")
            Return ""
        End Try
    End Function
#End Region

#Region "Export To Excel For Active/Inactive User"
    Private Function ConvertDsTO(ByVal ds As DataSet) As String
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer
        Dim dsConvert As New DataSet

        Try

            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""15""><center><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")


            strMessage.Append(lbltitle.Text)
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")

            dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vUserName,vScopeName,vUserGroupName,vFirstName,vLastName,vLoginName,vUserTypeName,vDeptName,vLocationName,vEmailId,vPhoneNo,vExtNo,vRemark,tmp_dModifyOn,ModifierName".Split(",")).DefaultView.ToTable())
            dsConvert.AcceptChanges()
            dsConvert.Tables(0).Columns(0).ColumnName = "User Name"
            dsConvert.Tables(0).Columns(1).ColumnName = "Scope"
            dsConvert.Tables(0).Columns(2).ColumnName = "User Group"
            dsConvert.Tables(0).Columns(3).ColumnName = "First Name"
            dsConvert.Tables(0).Columns(4).ColumnName = "Last Name"
            dsConvert.Tables(0).Columns(5).ColumnName = "Login Name"
            dsConvert.Tables(0).Columns(6).ColumnName = "User Type"
            dsConvert.Tables(0).Columns(7).ColumnName = "Depatment"
            dsConvert.Tables(0).Columns(8).ColumnName = "Location"
            dsConvert.Tables(0).Columns(9).ColumnName = "Email Id"
            dsConvert.Tables(0).Columns(10).ColumnName = "Phone No"
            dsConvert.Tables(0).Columns(11).ColumnName = "Ext No"
            dsConvert.Tables(0).Columns(12).ColumnName = "Remark"
            dsConvert.Tables(0).Columns(13).ColumnName = "Modify On"
            dsConvert.Tables(0).Columns(14).ColumnName = "Modify By"
            dsConvert.AcceptChanges()

            For iCol = 0 To dsConvert.Tables(0).Columns.Count - 1

                strMessage.Append("<td><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).Trim())
                strMessage.Append("</font></strong></td>")

            Next
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")

            For j = 0 To dsConvert.Tables(0).Rows.Count - 1
                strMessage.Append("<tr>")
                For i = 0 To dsConvert.Tables(0).Columns.Count - 1

                    strMessage.Append("<td><font color=""#000000"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim())
                    strMessage.Append("</font></td>")

                Next
                strMessage.Append("</tr>")
            Next
            strMessage.Append("</table>")

            Return strMessage.ToString

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....ConvertDsTO")
            Return ""
        End Try
    End Function
#End Region

#Region "Export To Excel for User Login History"
    Private Function ConvertDsuserTO(ByVal ds As DataSet) As String
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer
        Dim dsConvert As New DataSet

        Try

            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""5""><center><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")


            strMessage.Append("User Login History")
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")

            dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vUserName,vUsertypeName,cLOFlag,tmp_dInOutDateTime,vIPAddress".Split(",")).DefaultView.ToTable())
            dsConvert.AcceptChanges()
            dsConvert.Tables(0).Columns(0).ColumnName = "User Name"
            dsConvert.Tables(0).Columns(1).ColumnName = "User Profile"
            dsConvert.Tables(0).Columns(2).ColumnName = "Status"
            dsConvert.Tables(0).Columns(3).ColumnName = "Login/Logout Time"
            dsConvert.Tables(0).Columns(4).ColumnName = "IP Address"
            dsConvert.AcceptChanges()

            For iCol = 0 To dsConvert.Tables(0).Columns.Count - 1

                strMessage.Append("<td><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).Trim())
                strMessage.Append("</font></strong></td>")

            Next
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")

            For j = 0 To dsConvert.Tables(0).Rows.Count - 1
                strMessage.Append("<tr>")
                For i = 0 To dsConvert.Tables(0).Columns.Count - 1

                    strMessage.Append("<td><font color=""#000000"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim())
                    strMessage.Append("</font></td>")

                Next
                strMessage.Append("</tr>")
            Next
            strMessage.Append("</table>")

            Return strMessage.ToString

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....ConvertDsuserTO")
            Return ""
        End Try
    End Function
#End Region

#Region "Export To Excel for Blocked User details"
    Private Function ConvertDsuserfailureTO(ByVal ds As DataSet) As String
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer
        Dim dsConvert As New DataSet

        Try

            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""3""><center><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")


            strMessage.Append("Blocked User History")
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")

            dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vUserName,dLastFailedLogin,vIPAddress".Split(",")).DefaultView.ToTable())
            dsConvert.AcceptChanges()
            dsConvert.Tables(0).Columns(0).ColumnName = "User Name"
            dsConvert.Tables(0).Columns(1).ColumnName = "Login Failed Time"
            dsConvert.Tables(0).Columns(2).ColumnName = "IP Address"
            dsConvert.AcceptChanges()

            For iCol = 0 To dsConvert.Tables(0).Columns.Count - 1

                strMessage.Append("<td><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).Trim())
                strMessage.Append("</font></strong></td>")

            Next
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")

            For j = 0 To dsConvert.Tables(0).Rows.Count - 1
                strMessage.Append("<tr>")
                For i = 0 To dsConvert.Tables(0).Columns.Count - 1

                    strMessage.Append("<td><font color=""#000000"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim())
                    strMessage.Append("</font></td>")

                Next
                strMessage.Append("</tr>")
            Next
            strMessage.Append("</table>")

            Return strMessage.ToString

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....ConvertDsuserfailureTO")
            Return ""
        End Try
    End Function
#End Region

#Region "Error Handler"
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objcommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objcommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub
#End Region

#Region "Grid Events"
    Protected Sub GV_User_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or _
        e.Row.RowType = DataControlRowType.Header Or _
        e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVC_Id).Visible = False
            e.Row.Cells(GVC_UserGroupCode).Visible = False
            e.Row.Cells(GVC_UserGroupName).Visible = False
            e.Row.Cells(GVC_UserTypeCode).Visible = False
            e.Row.Cells(GVC_DeptCode).Visible = False
            e.Row.Cells(GVC_LoginName).Visible = False
            e.Row.Cells(GVC_LocationCode).Visible = False
            e.Row.Cells(GVC_LoginPass).Visible = False
        End If
    End Sub
    'Protected Sub GV_User_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
    '    Dim i As Integer = e.CommandArgument
    '    If e.CommandName.ToUpper = "EDIT" Then
    '        Me.Response.Redirect("frmUserMaster.aspx?mode=2&value=" & Me.GV_User.Rows(i).Cells(GVC_Id).Text.Trim())
    '    End If
    'End Sub

    Protected Sub GV_User_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_User.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + 1 + (Me.GV_User.PageSize * Me.GV_User.PageIndex)
            'CType(e.Row.FindControl("lnkEdit"), LinkButton).CommandArgument = e.Row.RowIndex
            'CType(e.Row.FindControl("lnkEdit"), LinkButton).CommandName = "Edit"
        End If

    End Sub

    Protected Sub GV_User_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        GV_User.PageIndex = e.NewPageIndex

        ' Added By Jeet Patel on 18-May-2015 to call Status wise grid on Page index change
        If ddlUserStatus.SelectedItem.Text = activeuser Then
            If Not FillActiveUserGrid() Then
                Exit Sub
            End If
        ElseIf ddlUserStatus.SelectedItem.Text = inactiveuser Then
            If Not FillInactiveUserGrid() Then
                Exit Sub
            End If
        ElseIf ddlUserStatus.SelectedItem.Text = alluser Then
            If Not FillAllUserGrid() Then
                Exit Sub
            End If
        End If

        '======================================================================================

    End Sub

    Protected Sub GVuserhistory_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        GVuserhistory.PageIndex = e.NewPageIndex
        'Me.BtnGoForuserhistory_Click()
        FillUserLoginhistoryGrid()
    End Sub

    Protected Sub GVuserfailure_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        GVuserfailure.PageIndex = e.NewPageIndex
        If Not FillUserLoginfailureGrid() Then
            Exit Sub
        End If
    End Sub

    Protected Sub GVcntuser_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        GVcntuser.PageIndex = e.NewPageIndex
        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub
#End Region

#Region "SelectedIndexChange Event"

    Protected Sub ddlUserStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUserStatus.SelectedIndexChanged

        If ddlUserStatus.SelectedItem.Text = activesessions Then
            Me.trUserName.Visible = True
            Me.trDate.Visible = False
            Me.trDepartment.Visible = False
            Me.trScope.Visible = False
            Me.lblprofile.Text = userprofile
            FillDropDownActSessions()  ' Fill User Profile
            FillDropDownActiveUser()   ' Fill User Name 
            Me.GVcntuser.PageIndex = 0
            'FillGrid()                 
            'Me.pnlactivesession.Visible = True
        ElseIf ddlUserStatus.SelectedItem.Text = userloginhistory Then
            Me.trUserName.Visible = True
            Me.trDate.Visible = True
            Me.trDepartment.Visible = False
            Me.trScope.Visible = False
            Me.lblprofile.Text = userprofile
            FillDropDownUserHistory()               ' Fill User Profile
            Me.GVuserhistory.PageIndex = 0
            fillcurrentdate()                       ' Fill Current Date 
            FillDropDownHisoryUser()                ' Fill User Name in User Name Drop Down
            'FillUserLoginhistoryGrid()
            'Me.Pnluserhistory.Visible = True
        ElseIf ddlUserStatus.SelectedItem.Text = activeuser Then
            Me.trUserName.Visible = True
            Me.trDepartment.Visible = True
            Me.trScope.Visible = False
            Me.trDate.Visible = False
            Me.lblprofile.Text = userprofile
            FillDropDownActUser()           ' Fill User Profile
            FillActiveUser()                ' Fill User Name
            FillActiveUserDepartment()      ' Fill Department
            Me.GV_User.PageIndex = 0
            'FillActiveUserGrid()
            'Me.Pnlalluser.Visible = True
        ElseIf ddlUserStatus.SelectedItem.Text = inactiveuser Then
            Me.trDepartment.Visible = True
            Me.trUserName.Visible = False
            Me.trScope.Visible = False
            Me.trDate.Visible = False
            Me.lblprofile.Text = userprofile
            FillDropDownActUser()           ' Fill User Profile
            FillInactiveDepartment()        ' Fill Department List in Which some User Are inactive
            Me.GV_User.PageIndex = 0
            'FillInactiveUserGrid()
            'Me.Pnlalluser.Visible = True
        ElseIf ddlUserStatus.SelectedItem.Text = alluser Then
            Me.trUserName.Visible = True
            Me.trDepartment.Visible = True
            Me.trScope.Visible = True
            Me.trDate.Visible = False
            Me.lblprofile.Text = userprofile
            FillDropDownActUser()           ' Fill User Profile
            FillAllUserName()               ' Fill User Name
            FillAllDepartment()             ' Fill Department
            FillAllScope()                  ' Fill Scope
            Me.GV_User.PageIndex = 0
            'FillAllUserGrid()
            'Me.Pnlalluser.Visible = True
        ElseIf ddlUserStatus.SelectedItem.Text = blockeduserhistory Then
            Me.trUserName.Visible = False
            Me.trDepartment.Visible = False
            Me.trDate.Visible = False
            Me.trScope.Visible = False
            Me.lblprofile.Text = username
            FillDropDownBlockUser()                 ' Fill User Name
            Me.GVuserfailure.PageIndex = 0
            'FillUserLoginfailureGrid()
            'Me.Pnluserfailure.Visible = True
        End If

    End Sub

    Protected Sub DDLUserType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLUserType.SelectedIndexChanged

        If ddlUserStatus.SelectedItem.Text = activesessions Then
            Me.GVcntuser.PageIndex = 0
            FillDropDownActiveUser()               ' Fill User Name 
            'FillGrid()
            'Me.pnlactivesession.Visible = True
        ElseIf ddlUserStatus.SelectedItem.Text = userloginhistory Then
            Me.trDate.Visible = True
            Me.GVuserhistory.PageIndex = 0
            FillDropDownHisoryUser()                ' Fill User Name 
            'Me.BtnGoForuserhistory_Click()
            'Me.Pnluserhistory.Visible = True
        ElseIf ddlUserStatus.SelectedItem.Text = activeuser Then
            Me.trDepartment.Visible = True
            Me.GV_User.PageIndex = 0
            FillActiveUser()                        ' Fill User Name
            FillActiveUserDepartment()              ' Fill Department
            'FillActiveUserGrid()
            'Me.Pnlalluser.Visible = True
        ElseIf ddlUserStatus.SelectedItem.Text = inactiveuser Then
            Me.GV_User.PageIndex = 0
            Me.trDepartment.Visible = True
            FillInactiveDepartment()
            'FillInactiveUserGrid()
            'Me.Pnlalluser.Visible = True
        ElseIf ddlUserStatus.SelectedItem.Text = alluser Then
            Me.GV_User.PageIndex = 0
            Me.trDepartment.Visible = True
            Me.trScope.Visible = True
            FillAllUserName()               ' Fill User Name
            FillAllDepartment()             ' Fill Department 
            FillAllScope()                  ' Fill Scope of User
            'FillAllUserGrid()
            'Me.Pnlalluser.Visible = True
        ElseIf ddlUserStatus.SelectedItem.Text = blockeduserhistory Then
            Me.GV_User.PageIndex = 0
            'FillUserLoginfailureGrid()
            'Me.Pnluserfailure.Visible = True
        End If

    End Sub

    Protected Sub DDlUserName_SelecedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDlUserName.SelectedIndexChanged

        If ddlUserStatus.SelectedItem.Text = userloginhistory Then
            Me.trDate.Visible = True

        ElseIf ddlUserStatus.SelectedItem.Text = activeuser Then
            FillActiveUserDepartment()
            Me.trDepartment.Visible = True

        ElseIf ddlUserStatus.SelectedItem.Text = alluser Then
            Me.trDepartment.Visible = True
            Me.trScope.Visible = True
            FillAllDepartment()
            FillAllScope()
        End If

    End Sub

    Protected Sub DDLDepaerment_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLDepartment.SelectedIndexChanged
        Me.trDepartment.Visible = True

        If ddlUserStatus.SelectedItem.Text = alluser Then
            Me.trScope.Visible = True
            FillAllScope()
        End If
    End Sub

    Protected Sub DDLScope_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLScope.SelectedIndexChanged
        Me.trDepartment.Visible = True
        Me.trScope.Visible = True
    End Sub

#End Region

End Class
