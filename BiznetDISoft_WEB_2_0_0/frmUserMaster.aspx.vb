Imports System.Web.Services
Imports Newtonsoft.Json
Imports System.Drawing

Partial Class frmUserMaster
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Dim ObjCommon As New clsCommon
    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtUserMst As String = "DtUserMst"
    Private Const VS_UserId As String = "UserId"

    Private Const vs_actuser As String = "dtactuser"
    Private Const vs_cntuser As String = "dtcntuser"
    Private Const vs_userhistory As String = "dtuserhistory"

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
    Private Const GVC_ModifyOn As Integer = 20
    Private Const GVC_Edit As Integer = 22

#End Region

#Region "Form Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try
            Choice = Me.Request.QueryString("Mode")
            Me.ViewState(VS_Choice) = Choice   'To use it while saving
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_UserId) = Me.Request.QueryString("Value").ToString
            End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Return False
            End If

            Me.ViewState(VS_DtUserMst) = ds.Tables("UserMst")   ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Return False
            End If
            If Not IsPostBack Then

                If (Choice = 1) Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideSponsorDetails", "HideSponsorDetails()", True)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideSponsorDetails", "", True)
                End If
            End If
            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall")
            Return False
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_Data"

    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean

        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Val As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing
        Dim datamode As WS_HelpDbTable.DataRetrievalModeEnum
        Try

            Val = Me.ViewState(VS_UserId) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                'SD
                'wStr = "1=2"
                If Session(S_UserID) = SuperUserId Then
                    wStr = ""
                    datamode = WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords
                Else
                    wStr = "vLocationCode='" + Me.Session(S_LocationCode) + "' AND cStatusIndi = 'D' order by vUserName"
                    datamode = WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition
                End If
            Else
                wStr = "iUserId=" + Val.ToString
                datamode = WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition
            End If

            If Not objHelp.getuserMst(wStr, datamode, ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Return False
            End If
            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            If ds.Tables(0).Rows.Count <= 0 And _
             Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found for Selected Operation")
            End If

            ds_DWR_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......GenCall_Data")
            Return False
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_showUI"

    Private Function GenCall_ShowUI() As Boolean
        Dim dt_UserMst As DataTable = Nothing
        Dim dr As DataRow
        Dim pwd As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try

            Page.Title = ":: User Master  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "User Master"

            dt_UserMst = Me.ViewState(VS_DtUserMst)

            Choice = Me.ViewState(VS_Choice)

            If Not FillDropDown() Then
                Return False
            End If

            If Not FillGrid() Then
                Return False
            End If

            If Not Me.Session(S_UserID) = SuperUserId Then
                DDLLocation.SelectedValue = Me.Session(S_LocationCode)
                DDLLocation.Enabled = False
            Else
                DDLLocation.Enabled = True
            End If

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                dr = dt_UserMst.Rows(0)
                Me.ddlScope.SelectedValue = dr("nScopeNo")
                Me.DDLUserGroup.SelectedValue = dr("iUserGroupCode")
                Me.txtUName.Text = dr("vUserName")
                Me.TxtFirstName.Text = dr("vFirstName")
                Me.TxtLastName.Text = dr("vLastName")

                pwd = objHelp.DecryptPassword(Convert.ToString(dr("vLoginPass")))
                Me.txtLPass.Attributes.Add("value", pwd)

                Me.DDLUserType.SelectedValue = dr("vUserTypeCode")
                Me.DDLLocation.SelectedValue = dr("vLocationCode")
                Me.DDLDept.SelectedValue = dr("vDeptCode")
                Me.txtEmail.Text = dr("vEmailId")
                Me.txtPhNo.Text = dr("vPhoneNo")
                Me.txtExtNo.Text = dr("vExtNo")
                'Me.txtRemark.Text = dr("vRemark")
                If Not IsDBNull(dr("dFromDate")) Then
                    If dr("dFromDate") = #1/1/1900# Then
                        Me.txtloginfrom.Text = ""
                    Else
                        Me.txtloginfrom.Text = CType(dr("dFromDate"), Date).ToString("dd-MMM-yyyy")
                    End If
                End If
                If Not IsDBNull(dr("dToDate")) Then
                    If dr("dToDate") = #1/1/1900# Then
                        Me.txtLoginTo.Text = ""
                    Else
                        Me.txtLoginTo.Text = CType(dr("dToDate"), Date).ToString("dd-MMM-yyyy")
                    End If
                End If
                Me.BtnSave.Text = "Update"
                Me.BtnSave.ToolTip = "Update"
            End If
            Me.txtloginfrom.Attributes.Add("onchange", "dateformatvalidator(1)")
            Me.txtLoginTo.Attributes.Add("onchange", "dateformatvalidator(2)")
            'ResetPage()
            'Me.BtnSave.Attributes.Add("OnClick", "return Validation();")

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI()")
            Return False
        Finally
        End Try
    End Function
#End Region

#Region "FillDropDown"

    Private Function FillDropDown() As Boolean
        Dim ds_UserGroup As New Data.DataSet
        Dim ds_UserType As New Data.DataSet
        Dim ds_Dept As New Data.DataSet
        Dim ds_Location As New Data.DataSet
        Dim ds_Scope As New Data.DataSet
        Dim dv_UserGroup As New DataView
        Dim dv_UserType As New DataView
        Dim dv_Dept As New DataView
        Dim dv_Location As New DataView
        Dim dv_Scope As New DataView
        Dim estr As String = String.Empty
        Dim wStr As String = String.Empty
        Try

            wStr = "cStatusIndi <> 'D'"
            If Not Me.objHelp.GetDeptMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                        ds_Dept, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From DeptMst : " + estr, Me.Page)
                Return False
            End If
            dv_Dept = ds_Dept.Tables(0).DefaultView
            dv_Dept.Sort = "vDeptName"
            Me.DDLDept.DataSource = dv_Dept.ToTable()
            Me.DDLDept.DataValueField = "vDeptCode"
            Me.DDLDept.DataTextField = "vDeptName"
            Me.DDLDept.DataBind()
            Me.DDLDept.Items.Insert(0, New ListItem("Select Department", 0))


            If Not Me.objHelp.getuserGroupMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                        ds_UserGroup, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From UserGroupMst : " + estr, Me.Page)
                Return False
            End If
            dv_UserGroup = ds_UserGroup.Tables(0).DefaultView
            dv_UserGroup.Sort = "vUserGroupName"
            Me.DDLUserGroup.DataSource = dv_UserGroup.ToTable()
            Me.DDLUserGroup.DataValueField = "iUserGroupCode"
            Me.DDLUserGroup.DataTextField = "vUserGroupName"
            Me.DDLUserGroup.DataBind()
            Me.DDLUserGroup.Items.Insert(0, New ListItem("Select User Group", 0))


            If Not Me.objHelp.getUserTypeMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                        ds_UserType, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From UserTypeMst : " + estr, Me.Page)
                Return False
            End If
            dv_UserType = ds_UserType.Tables(0).DefaultView
            dv_UserType.Sort = "vUserTypeName"
            Me.DDLUserType.DataSource = dv_UserType.ToTable()
            Me.DDLUserType.DataValueField = "vUserTypeCode"
            Me.DDLUserType.DataTextField = "vUserTypeName"
            Me.DDLUserType.DataBind()
            Me.DDLUserType.Items.Insert(0, New ListItem("Select User Type", 0))


            If Not Me.objHelp.getLocationMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                        ds_Location, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From LocationMst : " + estr, Me.Page)
                Return False
            End If
            dv_Location = ds_Location.Tables(0).DefaultView()
            dv_Location.Sort = "vLocationName"
            Me.DDLLocation.DataSource = dv_Location.ToTable()
            Me.DDLLocation.DataValueField = "vLocationCode"
            Me.DDLLocation.DataTextField = "vLocationName"
            Me.DDLLocation.DataBind()
            Me.DDLLocation.Items.Insert(0, New ListItem("Select Location", 0))

            If Not Me.objHelp.GetScopeMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                        ds_Scope, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From ScopeMst : " + estr, Me.Page)
                Return False
            End If

            dv_Scope = ds_Scope.Tables(0).DefaultView()
            dv_Scope.Sort = "vScopeName"
            Me.ddlScope.DataSource = dv_Scope.ToTable()
            Me.ddlScope.DataValueField = "nScopeNo"
            Me.ddlScope.DataTextField = "vScopeName"
            Me.ddlScope.DataBind()
            Me.ddlScope.Items.Insert(0, New ListItem("Select Scope", 0))

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillDropDown")
            Return False
        End Try
    End Function

#End Region

#Region "FillGrid"

    Private Function FillGrid() As Boolean
        Dim ds_RoleOp As New Data.DataSet
        Dim dv_User As New Data.DataView
        Dim estr As String = String.Empty
        Dim lblexceltitle As String = String.Empty
        Dim wstr As String = String.Empty
        Dim datamode As WS_HelpDbTable.DataRetrievalModeEnum
        'SD
        'Dim wstr As String = IIf(Session(S_UserID) = SuperUserId, "1=2", "vLocationCode='" + Me.Session(S_LocationCode) + "'")
        Try
            If Session(S_UserID) = SuperUserId Then
                wstr = ""
                datamode = WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords
            Else
                wstr = "vLocationCode='" + Me.Session(S_LocationCode) + "'"
                datamode = WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition
            End If
            If Not objHelp.GetViewUserMst(wstr, datamode, ds_RoleOp, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From ViewUserMst", estr)
                Return False
            End If

            If ds_RoleOp.Tables(0).Rows.Count < 1 Then
                ObjCommon.ShowAlert("No Record Found", Me.Page)
                Return True
            End If
            ' Me.lblexceltitle.Text = "All User"
            dv_User = ds_RoleOp.Tables(0).DefaultView()
            dv_User.Sort = "vUserName"
            Me.ViewState(vs_actuser) = dv_User.Table
            Me.GV_User.DataSource = dv_User
            '  Me.GV_User.DataBind()
            ' If GV_User.Rows.Count > 0 Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGV_User", "UIGV_User(); ", True)
            'End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillGrid")
            Return False
        End Try
    End Function

#End Region

#Region "Button Events"

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim ds_Save As New DataSet
        Dim dt_Save As New DataTable
        Dim estr As String = String.Empty
        Dim message As String = String.Empty
        Try

            If Not AssignValues() Then
                Exit Sub
            End If
            ds_Save = New DataSet
            dt_Save = CType(Me.ViewState(VS_DtUserMst), DataTable)
            dt_Save.TableName = "UserMst"
            ds_Save.Tables.Add(dt_Save)
            If Not objLambda.Save_InsertUserMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_UserMst, ds_Save, Me.Session(S_UserID), estr) Then
                ObjCommon.ShowAlert("Error While Saving UserMst", Me.Page)
                Exit Sub
            End If
            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "User Details Saved Successfully !", "User Details Updated Successfully !")
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('" + message + "')", True)
            'ObjCommon.ShowAlert("Record Saved SuccessFully", Me.Page)
            ResetPage()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...BtnSave_Click")
        End Try
    End Sub

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ResetPage()
        Me.Response.Redirect("frmUserMaster.aspx?mode=1")
    End Sub


    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click

        Me.Response.Redirect("frmUserMaster.aspx?mode=2&value=" & Me.hdnEditedId.Value)

    End Sub
    ' For User Active/Inactive user Details added by Mrunal Parekh
    'Protected Sub btnInactUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInactUser.Click
    '    Dim ds_inactuser As New Data.DataSet
    '    Dim dv_inactuser As New Data.DataView
    '    Dim wstr As String = "cStatusIndi = 'D'"
    '    Dim estr As String = String.Empty

    '    Try

    '        wstr += IIf(Session(S_UserID) = SuperUserId, "", " AND vLocationCode='" + Me.Session(S_LocationCode) + "'")
    '        wstr += " order by vUserName"
    '        If Not objHelp.GetViewUserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_inactuser, estr) Then
    '            Me.ShowErrorMessage("Error While Getting Data From ViewUserMst", estr)
    '            Exit Sub
    '        End If

    '        If ds_inactuser.Tables(0).Rows.Count < 1 Then
    '            ObjCommon.ShowAlert("No Record Found", Me.Page)
    '            Exit Sub
    '        End If

    '        dv_inactuser = ds_inactuser.Tables(0).DefaultView()
    '        Me.ViewState(vs_actuser) = dv_inactuser.Table
    'Me.lblexceltitle.Text = "Inactive User"
    'dv_inactuser.Sort = "vUserName"
    'Me.GV_User.DataSource = dv_inactuser
    'Me.GV_User.DataBind()
    'If GV_User.Rows.Count > 0 Then
    '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGV_User", "UIGV_User(); ", True)
    'End If

    'Catch ex As Exception
    '    Me.ShowErrorMessage(ex.Message, "....btnInactUser_Click")
    'End Try



    ' End Sub

    ' Protected Sub btnactUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnactUser.Click
    'Dim ds_actuser As New Data.DataSet
    ' Dim dv_actuser As New Data.DataView
    ' Dim wstr As String = "cStatusIndi <> 'D'"
    ' Dim estr As String = String.Empty

    ' Try
    'SD
    ' wstr += IIf(Session(S_UserID) = SuperUserId, "", " AND vLocationCode='" + Me.Session(S_LocationCode) + "'")
    ' wstr += " order by vUserName"
    'If Not objHelp.GetViewUserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_actuser, estr) Then
    'Me.ShowErrorMessage("Error While Getting Data From ViewUserMst", estr)
    ' Exit Sub
    '  End If

    ' If ds_actuser.Tables(0).Rows.Count < 1 Then
    ' ObjCommon.ShowAlert("No Record Found", Me.Page)
    'Exit Sub
    ' End If
    ' Me.lblexceltitle.Text = "Active User"
    ' dv_actuser = ds_actuser.Tables(0).DefaultView()
    ' Me.ViewState(vs_actuser) = dv_actuser.Table
    ' Me.GV_User.DataSource = dv_actuser
    ' Me.GV_User.DataBind()
    ' If GV_User.Rows.Count > 0 Then
    '  ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGV_User", "UIGV_User(); ", True)
    '  End If

    ' Catch ex As Exception
    '  Me.ShowErrorMessage(ex.Message, "...btnactUser_Click")
    '   End Try

    ' End Sub

    'Protected Sub btnalluser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnalluser.Click
    'Dim eStr As String = String.Empty
    ' If Not FillGrid() Then
    'Me.ShowErrorMessage("Error while filling grid", eStr)
    ' Exit Sub
    ' End If
    ' End Sub

    Protected Sub btnexportexcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnexportexcel.Click
        Dim fileName As String = String.Empty
        Dim ds As New DataSet
        Dim estr As String = String.Empty
        Dim wstr As String
        Try

            'If Me.GV_User.Rows.Count < 1 Then
            '    Me.ObjCommon.ShowAlert("No data to Export", Me.Page)
            '    Exit Sub
            'End If
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            fileName = "userreport"
            fileName = fileName & ".xls"
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            If (Convert.ToString(hdnActiveData.Value).ToLower = "inactive-user") Then
                wstr = "cStatusIndi = 'D'"
                wstr += IIf(HttpContext.Current.Session(S_UserID) = SuperUserId, "", " AND vLocationCode='" + HttpContext.Current.Session(S_LocationCode) + "'")
                wstr += " order by vUserName"
                If Not objHelp.GetViewUserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr) Then

                    Exit Sub
                End If

            ElseIf (Convert.ToString(hdnActiveData.Value).ToLower = "active-user") Then
                wstr = "cStatusIndi <> 'D'"
                wstr += IIf(HttpContext.Current.Session(S_UserID) = SuperUserId, "", " AND vLocationCode='" + HttpContext.Current.Session(S_LocationCode) + "'")
                wstr += " order by vUserName"
                If Not objHelp.GetViewUserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr) Then

                    Exit Sub
                End If

            ElseIf (Convert.ToString(hdnActiveData.Value).ToLower = "all-user") Then
                ds.Tables.Add(CType(Me.ViewState(vs_actuser), DataTable).Copy())
                ds.AcceptChanges()
            End If

            Context.Response.Write(ConvertDsTO(ds))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception
            'Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub
    '----------------------------------------------------------
    'For current Online User
    Protected Sub btncntuser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncntuser.Click
        Dim dsUserLoginDetails As New DataSet
        Dim dv_cntuser As New Data.DataView
        Dim estr As String = String.Empty
        'SD
        'Dim wstr As String = IIf(Session(S_UserID) = SuperUserId, "", "vLocationCode='" + Me.Session(S_LocationCode) + "'")
        Dim wstr As String = String.Empty
        Dim datamode As WS_HelpDbTable.DataRetrievalModeEnum

        Try
            If Session(S_UserID) = SuperUserId Then
                wstr = ""
                datamode = WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords
            Else
                wstr = "vLocationCode='" + Me.Session(S_LocationCode) + "'"
                datamode = WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition
            End If

            If Not objHelp.View_Userlogindetails(wstr, datamode, dsUserLoginDetails, estr) Then
                Throw New Exception(estr)
            End If

            If dsUserLoginDetails.Tables(0).Rows.Count > 0 Then
                dv_cntuser = dsUserLoginDetails.Tables(0).DefaultView()
                Me.ViewState(vs_cntuser) = dv_cntuser.Table
                Me.GVcntuser.DataSource = dv_cntuser
                Me.GVcntuser.DataBind()
                Me.Mpusermst.Show()
            Else
                ObjCommon.ShowAlert("No Current User available", Me)
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "All_user", "activeuser(); ", True)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btncntuser_Click")

        End Try
    End Sub

    'Create Excel for current online User
    Protected Sub exclcntuser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles exclcntuser.Click

        Dim fileName As String = String.Empty
        Dim dscntuser As New DataSet
        Try

            If Me.GVcntuser.Rows.Count < 1 Then
                Me.ObjCommon.ShowAlert("No data to Export", Me.Page)
                Exit Sub
            End If
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            fileName = "activeuserreport"
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

    Protected Sub btnhistoryuser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhistoryuser.Click
        Dim dsUserHistory As New DataSet
        Dim dv_Userhistory As New Data.DataView
        Dim estr As String = String.Empty

        Try
            If Not fillcurrentdate() Then
                Me.ShowErrorMessage("Error While fill Current Date", estr)
            End If

            Me.BtnGoForuserhistory_Click(sender, e)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnhistoryuser_Click")

        End Try

    End Sub

    Protected Sub BtnGoForuserhistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGoForuserhistory.Click
        Dim dsUserHistory As New DataSet
        Dim dv_Userhistory As New Data.DataView
        Dim estr As String = String.Empty
        Dim wstr As String = "dInOutDateTime between '" + TxtFromDateOfuserhistory.Text + "' and '" + TxtToDateOfuserhistory.Text + " 23:59:59:999'"
        Try
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "All_user", "activeuser(); ", True)
            wstr += IIf(Session(S_UserID) = SuperUserId, "", "AND vLocationCode='" + Me.Session(S_LocationCode) + "'")
            If Not objHelp.view_UserLoginHistory(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                               dsUserHistory, estr) Then
                Throw New Exception(estr)
            End If

            If dsUserHistory.Tables(0).Rows.Count > 0 Then
                dv_Userhistory = dsUserHistory.Tables(0).DefaultView()
                Me.ViewState(vs_userhistory) = dv_Userhistory.Table
                Me.GVuserhistory.DataSource = dv_Userhistory
                Me.GVuserhistory.DataBind()
                Me.MPEUserhistory.Show()

            Else
                ObjCommon.ShowAlert("No History of User available", Me)
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...BtnGoForuserhistory_Click")
        End Try

    End Sub

    Protected Sub btnexptuserhistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnexptuserhistory.Click
        Dim fileName As String = ""
        Dim dsuserhistory As New DataSet
        Try

            If Me.GVuserhistory.Rows.Count < 1 Then
                Me.ObjCommon.ShowAlert("No data to Export", Me.Page)
                Exit Sub
            End If
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            fileName = "userhistoryreport"
            fileName = fileName & ".xls"
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            dsuserhistory.Tables.Add(CType(Me.ViewState(vs_userhistory), DataTable).Copy())
            dsuserhistory.AcceptChanges()

            Context.Response.Write(ConvertDsuserhistoryTO(dsuserhistory))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnexptuserhistory_Click")
        End Try
    End Sub

#End Region

#Region "Assign values"

    Private Function AssignValues() As Boolean
        Dim dr As DataRow
        Dim dt_User As New DataTable
        Dim ds_Check As New DataSet
        Dim estr As String = ""
        Dim wstr As String = ""
        Dim EncryptPwd As String
        Dim txtPassword As String
        'Dim txtUserName As String
        'Dim key As String
        Try

            'Added By Pratik Soni To Avoid Duplication of "FirstName & LastName"
            If Me.BtnSave.Text = "Save" Then
                wstr = "cStatusIndi <> 'D' AND vFirstName = '" + Me.TxtFirstName.Text.Trim.ToString + "' AND vLastName = '" + Me.TxtLastName.Text.Trim.ToString + "'"
                If Not objHelp.getuserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Check, estr) Then
                    Me.ShowErrorMessage("Error While Getting Data From UserMst", estr)
                    Return False
                End If

                If ds_Check.Tables(0).Rows.Count > 0 Then
                    ObjCommon.ShowAlert("FirstName & LastName Already Exist !", Me.Page)
                    Return False
                End If
            End If
            '--------------------------------------------------------------------
            'For validating Duplication
            wstr = "cStatusIndi <> 'D' And vUserName='" & Me.txtUName.Text.Trim()
            wstr += "' AND iUserGroupCode = " & Me.DDLUserGroup.SelectedItem.Value
            wstr += " AND nScopeNo =" & Me.ddlScope.SelectedItem.Value
            wstr += " AND vUserTypeCode = '" & Me.DDLUserType.SelectedItem.Value & "'"
            wstr += " AND vDeptCode = '" & Me.DDLDept.SelectedItem.Value & "'"
            wstr += IIf(Session(S_UserID) = SuperUserId, "", "AND vLocationCode='" + Me.Session(S_LocationCode) + "'")

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                wstr += " And iUserId <> " + Me.ViewState(VS_UserId)
            End If

            If Not objHelp.getuserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Check, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From UserMst", estr)
                Return False
            End If

            If ds_Check.Tables(0).Rows.Count > 0 Then
                ObjCommon.ShowAlert("User Name Already Exists", Me.Page)
                Return False
            End If
            '***********************************
            '===Added on 16-09-09 by Deepak singh to encrypt Password==== 
            txtPassword = Me.txtLPass.Text.Trim()
            'txtUserName = txtUName.Text.Trim
            'key = Me.ObjCommon.ReversePassword(txtUserName)
            EncryptPwd = Me.objHelp.EncryptPassword(txtPassword)

            '============================================================

            dt_User = CType(Me.ViewState(VS_DtUserMst), DataTable)
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                dt_User.Clear()
                dr = dt_User.NewRow()
                'iUserId,iUserGroupCode,vUserName,vLoginName,vLoginPass,vUserTypeCode,vEmailId,vPhoneNo,vExtNo,vRemark,iModifyBy
                dr("iUserId") = "0000"
                dr("iUserGroupCode") = Me.DDLUserGroup.SelectedValue.Trim()
                dr("nScopeNo") = Me.ddlScope.SelectedValue.Trim
                dr("vUserName") = Me.txtUName.Text.Trim()
                dr("vFirstName") = Me.TxtFirstName.Text.Trim()
                dr("vLastName") = Me.TxtLastName.Text.Trim()
                dr("vLoginName") = Me.txtUName.Text.Trim()
                '=========added on 16-09-09========
                dr("vLoginPass") = EncryptPwd
                '=============================
                'dr("vLoginPass") = Me.txtLPass.Text.Trim()
                dr("vUserTypeCode") = Me.DDLUserType.SelectedValue.Trim()
                dr("vLocationCode") = Me.DDLLocation.SelectedValue.Trim()
                dr("vDeptCode") = Me.DDLDept.SelectedValue.Trim()
                dr("vEmailId") = Me.txtEmail.Text.Trim()
                dr("vPhoneNo") = Me.txtPhNo.Text.Trim()
                dr("vExtNo") = Me.txtExtNo.Text.Trim()
                dr("vRemark") = Me.txtRemark.Text.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)
                'If txtloginfrom.Text.ToString.Trim() <> "" And txtLoginTo.Text.ToString.Trim() <> "" Then
                '    If Me.txtloginfrom.Text = Date.Now().Date Or Me.txtloginfrom.Text < Date.Now().Date Then
                '        dr("cStatusIndi") = "N"
                '    Else
                '        dr("cStatusIndi") = "D"
                '    End If
                'Else
                '    dr("cStatusIndi") = "N"
                'End If
                If txtloginfrom.Text.ToString.Trim() <> "" Then
                    dr("dFromDate") = Me.txtloginfrom.Text.Trim
                Else
                    dr("dFromDate") = "01-Jan-1900"
                End If
                If txtLoginTo.Text.ToString.Trim() <> "" Then
                    dr("dToDate") = Me.txtLoginTo.Text.Trim() + " " + "23:59:00.00"
                Else
                    dr("dToDate") = "01-Jan-1900"
                End If
                dt_User.Rows.Add(dr)

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                For Each dr In dt_User.Rows
                    dr("iUserGroupCode") = Me.DDLUserGroup.SelectedValue.Trim()
                    dr("nScopeNo") = Me.ddlScope.SelectedValue.Trim
                    dr("vUserName") = Me.txtUName.Text.Trim()
                    dr("vFirstName") = Me.TxtFirstName.Text.Trim()
                    dr("vLastName") = Me.TxtLastName.Text.Trim()
                    dr("vLoginName") = Me.txtUName.Text.Trim()
                    '===Added on 16-09-09 by Deepak singh to encrypt Password====
                    dr("vLoginPass") = EncryptPwd
                    '==================================
                    'dr("vLoginPass") = Me.txtLPass.Text.Trim()
                    dr("vUserTypeCode") = Me.DDLUserType.SelectedValue.Trim()
                    dr("vLocationCode") = Me.DDLLocation.SelectedValue.Trim()
                    dr("vDeptCode") = Me.DDLDept.SelectedValue.Trim()
                    dr("vEmailId") = Me.txtEmail.Text.Trim()
                    dr("vPhoneNo") = Me.txtPhNo.Text.Trim()
                    dr("vExtNo") = Me.txtExtNo.Text.Trim()
                    dr("vRemark") = Me.txtRemark.Text.Trim()
                    dr("iModifyBy") = Me.Session(S_UserID)
                    'If txtloginfrom.Text.ToString.Trim() <> "" And txtLoginTo.Text.ToString.Trim() <> "" Then
                    '    If Me.txtloginfrom.Text = Date.Now().Date Or Me.txtloginfrom.Text < Date.Now().Date Then
                    '        dr("cStatusIndi") = "N"
                    '    Else
                    '        dr("cStatusIndi") = "D"
                    '    End If
                    'Else
                    '    dr("cStatusIndi") = "E"
                    'End If
                    dr("cStatusIndi") = "E"
                    If txtloginfrom.Text.ToString.Trim() <> "" Then
                        dr("dFromDate") = Me.txtloginfrom.Text.Trim   'CDate(objCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                    Else
                        dr("dFromDate") = "01-Jan-1900"
                    End If
                    If txtLoginTo.Text.ToString.Trim() <> "" Then
                        dr("dToDate") = Me.txtLoginTo.Text.Trim() + " " + "23:59:00.00"
                    Else
                        dr("dToDate") = "01-Jan-1900"
                    End If
                    dr.AcceptChanges()
                Next
                dt_User.AcceptChanges()
            End If
            Me.ViewState(VS_DtUserMst) = dt_User
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "Reset Page"

    Private Sub ResetPage()
        Me.DDLUserGroup.SelectedIndex = 0
        Me.txtUName.Text = ""
        Me.txtLPass.Text = ""
        Me.DDLUserType.SelectedIndex = 0
        Me.DDLLocation.SelectedIndex = 0
        Me.DDLDept.SelectedIndex = 0
        Me.txtEmail.Text = ""
        Me.txtPhNo.Text = ""
        Me.txtExtNo.Text = ""
        Me.txtRemark.Text = ""
        Me.ViewState(VS_DtUserMst) = Nothing
        'Me.Response.Redirect("frmUserMaster.aspx?mode=1")
    End Sub

#End Region

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

#Region "Grid Events"

    Protected Sub GV_User_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or _
        e.Row.RowType = DataControlRowType.Header Or _
        e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVC_Id).Visible = False
            e.Row.Cells(GVC_UserGroupCode).Visible = False
            e.Row.Cells(GVC_UserTypeCode).Visible = False
            e.Row.Cells(GVC_DeptCode).Visible = False
            e.Row.Cells(GVC_LocationCode).Visible = False
            e.Row.Cells(GVC_LoginPass).Visible = False
        End If
    End Sub

    Protected Sub GV_User_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim i As Integer = e.CommandArgument
        If e.CommandName.ToUpper = "EDIT" Then
            Me.Response.Redirect("frmUserMaster.aspx?mode=2&value=" & Me.GV_User.Rows(i).Cells(GVC_Id).Text.Trim())
        End If
    End Sub

    Protected Sub GV_User_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + 1 + (Me.GV_User.PageSize * Me.GV_User.PageIndex)
            e.Row.Cells(GVC_ModifyOn).Text = CDate(e.Row.Cells(GVC_ModifyOn).Text).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "Edit"

            'CType(e.Row.FindControl("lnkAudit"), ImageButton).Attributes.Add("iUserId", e.Row.Cells(GVC_Id).Text)

        End If
    End Sub

    Protected Sub GV_User_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        GV_User.PageIndex = e.NewPageIndex
        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub

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


            'strMessage.Append(lblexceltitle.Text)
            strMessage.Append(hdnActiveData.Value)
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")

            dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vUserName,vScopeName,vUserGroupName,vFirstName,vLastName,vLoginName,vUserTypeName,vDeptName,vLocationName,vEmailId,vPhoneNo,vExtNo,vRemark,dModifyOn,ModifierName".Split(",")).DefaultView.ToTable())
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
            dsConvert.Tables(0).Columns(14).ColumnName = "Modifier Name"
            dsConvert.AcceptChanges()

            For iCol = 0 To dsConvert.Tables(0).Columns.Count - 1

                strMessage.Append("<td bgcolor=""#1560a1""><strong><font color=""#FFFFFF"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).Trim())
                strMessage.Append("</font></strong></td>")

            Next
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")

            For j = 0 To dsConvert.Tables(0).Rows.Count - 1
                strMessage.Append("<tr>")
                For i = 0 To dsConvert.Tables(0).Columns.Count - 1

                    strMessage.Append("<td align=""left"" bgcolor=""84c8e6""><font color=""#191970"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim())
                    strMessage.Append("</font></td>")

                Next
                strMessage.Append("</tr>")
            Next
            strMessage.Append("</table>")

            Return strMessage.ToString

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "........ConvertDsTO")
            Return ""
        End Try
    End Function
#End Region

#Region "Export To Excel for Active User"
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


            strMessage.Append("Current Online User")
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")

            dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vUserName,vUsertypeName,dLoginDateTime,vIPAddress".Split(",")).DefaultView.ToTable())
            dsConvert.AcceptChanges()
            dsConvert.Tables(0).Columns(0).ColumnName = "User Name"
            dsConvert.Tables(0).Columns(1).ColumnName = "User Group"
            dsConvert.Tables(0).Columns(2).ColumnName = "Login Time"
            dsConvert.Tables(0).Columns(3).ColumnName = "IP Address"
            dsConvert.AcceptChanges()

            For iCol = 0 To dsConvert.Tables(0).Columns.Count - 1

                strMessage.Append("<td bgcolor=""#1560a1""><strong><font color=""#FFFFFF"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).Trim())
                strMessage.Append("</font></strong></td>")

            Next
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")

            For j = 0 To dsConvert.Tables(0).Rows.Count - 1
                strMessage.Append("<tr>")
                For i = 0 To dsConvert.Tables(0).Columns.Count - 1

                    strMessage.Append("<td align=""left"" bgcolor=""84c8e6""><font color=""#191970"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim())
                    strMessage.Append("</font></td>")

                Next
                strMessage.Append("</tr>")
            Next
            strMessage.Append("</table>")

            Return strMessage.ToString

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...ConvertDscntuserTO")
            'Return ""
            Return False
        End Try
    End Function
#End Region

#Region "Functions"

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
            BtnGoForuserhistory.Enabled = True
            fillcurrentdate = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..fillcurrentdate")
            fillcurrentdate = False
        End Try

    End Function

#End Region

#Region "Export To Excel for User Login History"
    Private Function ConvertDsuserhistoryTO(ByVal ds As DataSet) As String
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer
        Dim dsConvert As New DataSet

        Try

            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""5""><center><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")


            strMessage.Append("Active Sessions")
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")

            dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vUserName,vUsertypeName,cLOFlag,dInOutDateTime,vIPAddress".Split(",")).DefaultView.ToTable())
            dsConvert.AcceptChanges()
            dsConvert.Tables(0).Columns(0).ColumnName = "User Name"
            dsConvert.Tables(0).Columns(1).ColumnName = "User Profile"
            dsConvert.Tables(0).Columns(2).ColumnName = "Status"
            dsConvert.Tables(0).Columns(3).ColumnName = "Login/Logout Time"
            dsConvert.Tables(0).Columns(4).ColumnName = "IP Address"
            dsConvert.AcceptChanges()

            For iCol = 0 To dsConvert.Tables(0).Columns.Count - 1

                strMessage.Append("<td bgcolor=""#1560a1""><strong><font color=""#FFFFFF"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).Trim())
                strMessage.Append("</font></strong></td>")

            Next
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")

            For j = 0 To dsConvert.Tables(0).Rows.Count - 1
                strMessage.Append("<tr>")
                For i = 0 To dsConvert.Tables(0).Columns.Count - 1

                    strMessage.Append("<td align=""left"" bgcolor=""84c8e6""><font color=""#191970"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim())
                    strMessage.Append("</font></td>")

                Next
                strMessage.Append("</tr>")
            Next
            strMessage.Append("</table>")

            Return strMessage.ToString

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...ConvertDsuserhistoryTO")
            Return ""
        End Try
    End Function
#End Region

    <WebMethod> _
    Public Shared Function View_UserMst() As String
        Dim ds_RoleOp As New Data.DataSet
        Dim dv_User As New Data.DataView
        Dim estr As String = String.Empty
        Dim lblexceltitle As String = String.Empty
        Dim wstr As String = String.Empty
        Dim datamode As WS_HelpDbTable.DataRetrievalModeEnum
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim strReturn As String = String.Empty
        Dim Final_ds As DataSet
        'SD
        'Dim wstr As String = IIf(Session(S_UserID) = SuperUserId, "1=2", "vLocationCode='" + Me.Session(S_LocationCode) + "'")
        Try
            If HttpContext.Current.Session(S_UserID) = SuperUserId Then
                wstr = " "
                datamode = WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords
            Else
                wstr = "vLocationCode='" + HttpContext.Current.Session(S_LocationCode) + "'"
                datamode = WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition
            End If
            If Not objHelp.GetViewUserMst(wstr, datamode, ds_RoleOp, estr) Then
                Return False
            End If


            ' Me.lblexceltitle.Text = "All User"
            strReturn = JsonConvert.SerializeObject(ds_RoleOp)
            Final_ds = ds_RoleOp.Copy()
            Return strReturn

        Catch ex As Exception

            Return strReturn
        End Try

    End Function

    <WebMethod> _
    Public Shared Function View_InactiveUserMst() As String
        Dim ds_inactuser As New Data.DataSet
        Dim dv_inactuser As New Data.DataView
        Dim wstr As String = "cStatusIndi = 'D'"
        Dim estr As String = String.Empty
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim strReturn As String = String.Empty
        Dim Final_ds As DataSet
        Try
            'SD
            wstr += IIf(HttpContext.Current.Session(S_UserID) = SuperUserId, "", " AND vLocationCode='" + HttpContext.Current.Session(S_LocationCode) + "'")
            wstr += " order by vUserName"
            If Not objHelp.GetViewUserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_inactuser, estr) Then
                Return False
                Exit Function
            End If


            ' Me.lblexceltitle.Text = "All User"
            strReturn = JsonConvert.SerializeObject(ds_inactuser)
            Final_ds = ds_inactuser.Copy()
            Return strReturn

        Catch ex As Exception

            Return strReturn

        End Try

    End Function

    <WebMethod> _
    Public Shared Function View_activeUserMst() As String
        Dim ds_actuser As New Data.DataSet
        Dim dv_actuser As New Data.DataView
        Dim wstr As String = "cStatusIndi <> 'D'"
        Dim estr As String = String.Empty
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()

        Dim strReturn As String = String.Empty

        Try
            'SD
            wstr += IIf(HttpContext.Current.Session(S_UserID) = SuperUserId, "", " AND vLocationCode='" + HttpContext.Current.Session(S_LocationCode) + "'")
            wstr += " order by vUserName"
            If Not objHelp.GetViewUserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_actuser, estr) Then
                Return False
                Exit Function
            End If


            '  Me.lblexceltitle.Text = "Active User"
            strReturn = JsonConvert.SerializeObject(ds_actuser)

            Return strReturn

        Catch ex As Exception

            Return strReturn

        End Try
    End Function

    <WebMethod> _
    Public Shared Function AuditTrail(ByVal id As String) As String
        Dim ds_RoleOp As New Data.DataSet

        Dim estr As String = String.Empty
        Dim lblexceltitle As String = String.Empty
        Dim wstr As String = String.Empty
        Dim datamode As WS_HelpDbTable.DataRetrievalModeEnum
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim dtUserMstHistrory As New DataTable
        Dim strReturn As String = String.Empty
        Dim dt As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1

        Try

            wstr = " iUserId = '" + id + "'"
            datamode = WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition

            If Not objHelp.View_UserMstHistoryAuditTrail(wstr, datamode, ds_RoleOp, estr) Then
                Return False
            End If

            If Not dtUserMstHistrory Is Nothing Then
                dtUserMstHistrory.Columns.Add("SrNo")
                dtUserMstHistrory.Columns.Add("UserName")
                ' dtUserMstHistrory.Columns.Add("Scope")
                dtUserMstHistrory.Columns.Add("UserGroupName")
                dtUserMstHistrory.Columns.Add("FirstName")
                dtUserMstHistrory.Columns.Add("LastName")
                dtUserMstHistrory.Columns.Add("LoginName")
                dtUserMstHistrory.Columns.Add("UserTypeName")
                dtUserMstHistrory.Columns.Add("DeptName")
                dtUserMstHistrory.Columns.Add("LocationName")
                dtUserMstHistrory.Columns.Add("Emailid")
                ' dtUserMstHistrory.Columns.Add("Phoneno.")
                ' dtUserMstHistrory.Columns.Add("ExtNo.")
                dtUserMstHistrory.Columns.Add("Remarks")
                dtUserMstHistrory.Columns.Add("ModifyBy")
                dtUserMstHistrory.Columns.Add("ModifyOn")
            End If
            dt = ds_RoleOp.Tables(0)
            Dim dv As New DataView(dt)

            For Each dr As DataRow In dv.ToTable.Rows

                drAuditTrail = dtUserMstHistrory.NewRow()

                drAuditTrail("SrNo") = i
                drAuditTrail("UserName") = dr("vUserName").ToString()
                'drAuditTrail("Scope") = dr("vScopeName").ToString()
                drAuditTrail("UserGroupName") = dr("vUserGroupName").ToString()
                drAuditTrail("FirstName") = dr("vFirstName").ToString()
                drAuditTrail("LastName") = dr("vLastName").ToString()
                drAuditTrail("LoginName") = dr("vLoginName").ToString()
                drAuditTrail("UserTypeName") = dr("vUserTypeName").ToString()
                drAuditTrail("DeptName") = dr("vDeptName").ToString()
                drAuditTrail("LocationName") = dr("vLocationName").ToString()
                drAuditTrail("Emailid") = dr("vEmailId").ToString()
                ' drAuditTrail("Phoneno") = dr("vPhoneNo").ToString()
                ' drAuditTrail("ExtNo") = dr("vExtNo").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("dModifyOn"))
                dtUserMstHistrory.Rows.Add(drAuditTrail)
                dtUserMstHistrory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtUserMstHistrory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try


        ' Me.lblexceltitle.Text = "All User"
        'strReturn = JsonConvert.SerializeObject(dtUserMstHistrory)

        Return strReturn



    End Function

    Protected Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_ClientMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtUserMstHistrory As New DataTable
        Dim drAuditTrail As DataRow
        Dim datamode As WS_HelpDbTable.DataRetrievalModeEnum
        Dim i As Integer = 1
        Dim dt As New DataTable


        Dim filename As String = String.Empty
        Dim vTableName As String = String.Empty
        Dim vIdName As String = String.Empty
        Dim AuditFieldName As String = String.Empty
        Dim AuditFieldValue As String = String.Empty
        Dim vOtherTableName As String = String.Empty
        Dim vOtherIdName As String = String.Empty
        Dim strMessage As New StringBuilder

        Try

            'vTableName = "ClientMstHistory"
            'vIdName = ""
            'AuditFieldName = "vClientCode"
            'AuditFieldValue = hdnClientCode.Value
            'vOtherTableName = ""
            'vOtherIdName = ""

            wStr = " iUserId = '" + hdnUserName.Value + "' "
            datamode = WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition

            If Not objHelp.View_UserMstHistoryAuditTrail(wStr, datamode, ds_ClientMst, estr) Then
                Return
            End If

            If Not dtUserMstHistrory Is Nothing Then
                dtUserMstHistrory.Columns.Add("SrNo")

                dtUserMstHistrory.Columns.Add("UserName")
                dtUserMstHistrory.Columns.Add("UserGroupName")
                dtUserMstHistrory.Columns.Add("FirstName")
                dtUserMstHistrory.Columns.Add("LastName")
                dtUserMstHistrory.Columns.Add("LoginName")
                dtUserMstHistrory.Columns.Add("UserTypeName")
                dtUserMstHistrory.Columns.Add("DeptName")
                dtUserMstHistrory.Columns.Add("LocationName")
                dtUserMstHistrory.Columns.Add("Emailid")
                dtUserMstHistrory.Columns.Add("Remarks")
                dtUserMstHistrory.Columns.Add("ModifyBy")
                dtUserMstHistrory.Columns.Add("ModifyOn")
            End If

            dtUserMstHistrory.AcceptChanges()
            dt = ds_ClientMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtUserMstHistrory.NewRow()

                drAuditTrail("SrNo") = i
                drAuditTrail("UserName") = dr("vUserName").ToString()
                drAuditTrail("UserGroupName") = dr("vUserGroupName").ToString()
                drAuditTrail("FirstName") = dr("vFirstName").ToString()
                drAuditTrail("LastName") = dr("vLastName").ToString()
                drAuditTrail("LoginName") = dr("vLoginName").ToString()
                drAuditTrail("UserTypeName") = dr("vUserTypeName").ToString()
                drAuditTrail("DeptName") = dr("vDeptName").ToString()
                drAuditTrail("LocationName") = dr("vLocationName").ToString()
                drAuditTrail("Emailid") = dr("vEmailId").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("dModifyOn"))
                dtUserMstHistrory.Rows.Add(drAuditTrail)
                dtUserMstHistrory.AcceptChanges()
                i += 1
            Next

            gvExport.DataSource = dtUserMstHistrory
            gvExport.DataBind()
            If gvExport.Rows.Count > 0 Then

                gvExport.HeaderRow.BackColor = Color.White
                gvExport.FooterRow.BackColor = Color.White
                For Each cell As TableCell In gvExport.HeaderRow.Cells
                    cell.BackColor = gvExport.HeaderStyle.BackColor
                Next
                For Each row As GridViewRow In gvExport.Rows
                    row.BackColor = Color.White
                    row.Height = 20
                    For Each cell As TableCell In row.Cells
                        If row.RowIndex Mod 2 = 0 Then
                            cell.BackColor = gvExport.AlternatingRowStyle.BackColor
                            cell.ForeColor = gvExport.RowStyle.ForeColor
                        Else
                            cell.BackColor = gvExport.RowStyle.BackColor
                            cell.ForeColor = gvExport.RowStyle.ForeColor
                        End If
                        cell.CssClass = "textmode"
                    Next
                Next

                'style to format numbers to string
                Dim style As String = "<style> .textmode { } </style>"
                Response.Write(style)



                Dim info As String = String.Empty
                Dim gridviewHtml As String = String.Empty

                filename = "Audit Trail_" + hdnUserName.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                Dim stringWriter As New System.IO.StringWriter()

                Dim writer As New HtmlTextWriter(stringWriter)
                gvExport.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""13""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")





                strMessage.Append("<td colspan=""13""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("User-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")


                gridviewHtml = strMessage.ToString() + gridviewHtml
                Context.Response.Buffer = True
                Context.Response.ClearContent()
                Context.Response.ClearHeaders()

                Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
                Context.Response.AddHeader("content-Disposition", "attachment; filename=" + filename)
                Context.Response.AddHeader("Content-Length", gridviewHtml.Length)

                Context.Response.Write(gridviewHtml)
                Context.Response.Flush()
                Context.Response.End()

                System.IO.File.Delete(filename)
            Else

                Dim info As String = String.Empty
                Dim gridviewHtml As String = String.Empty

                filename = "Audit Trail_" + hdnUserName.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"


                drAuditTrail = dtUserMstHistrory.NewRow()
                drAuditTrail("SrNo") = i
                drAuditTrail("UserName") = ""

                drAuditTrail("Remarks") = ""
                drAuditTrail("ModifyBy") = ""
                drAuditTrail("ModifyOn") = ""
                dtUserMstHistrory.Rows.Add(drAuditTrail)
                dtUserMstHistrory.AcceptChanges()
                gvExport.DataSource = dtUserMstHistrory
                gvExport.DataBind()


                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                gvExport.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()


                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""13""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""13""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Client Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font><td></td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td align=""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")


                gridviewHtml = strMessage.ToString() + gridviewHtml
                Context.Response.Buffer = True
                Context.Response.ClearContent()
                Context.Response.ClearHeaders()

                Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
                Context.Response.AddHeader("content-Disposition", "attachment; filename=" + filename)
                Context.Response.AddHeader("Content-Length", gridviewHtml.Length)

                Context.Response.Write(gridviewHtml)
                Context.Response.Flush()
                Context.Response.End()

                System.IO.File.Delete(filename)
                Exit Sub
            End If
        Catch ex As Exception
            Me.ObjCommon.ShowAlert("Error While Exporting Data To Excel : " + estr, Me.Page)

            Exit Sub
        End Try
    End Sub

    Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

        ' Verifies that the control is rendered
    End Sub



End Class

