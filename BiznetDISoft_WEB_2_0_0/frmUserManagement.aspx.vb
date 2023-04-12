Imports Newtonsoft.Json
Imports Microsoft
Imports Microsoft.Office.Interop

Partial Class frmUserManagement
    Inherits System.Web.UI.Page

#Region " Variable Declaration "

    Dim objCommon As New clsCommon
    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Dim objPVNet As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
    Dim eStr As String
    Dim eStr_retu As String

    Private Const VS_DtUserType As String = "DtUserType"
    Private Const VS_LoginName As String = "LoginName" 'New Created
    Private Const VS_LoginPass As String = "LoginPass" 'New Created
    Private Const VS_UserName As String = "UserName" ' Textbox and D.D. Text


    Private Const GVC_UserTypeNo As Integer = 0
    Private Const GVC_UserTypeName As Integer = 1
    Private Const GVC_LoginName As Integer = 2
    Private Const GVC_LoginPass As Integer = 3
    Private Const GVC_Add As Integer = 4
    Private Const GVC_RstPwd As Integer = 5
    Private Const GVC_CurrnetStatus As Integer = 6
    Private Const GVC_status As Integer = 7
    Private Const GVC_Unlock As Integer = 8


    Private Const VS_Choice As String = "Choice"
    Private Const VS_UserId As String = "UserId"
    Private Const VS_DtUserMst As String = "DtUserMst" ' 'MyAdd Mode
    Private Const S_DtEditUser As String = "DtEditUser" 'Proc_CopyUser ' Edit Mode
    Private Const VS_EditUserName As String = "DtEditUserName"

    Private Const Vs_SelectionMode As String = "SelectionMode" 'DD Selection
    Private Const VS_ActiveUserMst As String = "ActiveUserMst" 'Status Table
    Private Const VS_ActFlag As String = "ActiveFlag" ''Flag ActiveInactive

    Private Const S_UserTypeMst As String = "DtUserTypeMst"

    Private CurrentIndex As Integer

#End Region

#Region " Load Events "


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Not IsPostBack Then
                Me.GenCall()
                HFUserName.Value = Me.Session(S_UserName).ToString()
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)

        End Try

    End Sub

#End Region

#Region " GenCall() "

    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try
            Choice = Me.Request.QueryString("Mode")
            Me.ViewState(VS_Choice) = Choice   'To use it while saving
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

            End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtUserMst) = ds.Tables("UserMst")   ' adding blank DataTable in viewstate
            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall")

        Finally

        End Try

    End Function

#End Region

#Region " GenCall_Data "
    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean

        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Val As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing
        Dim ds_fillUserType As DataSet = Nothing

        Try

            Val = Me.ViewState(VS_UserId) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)
            Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "nUserId=" + Val.ToString
            End If

            If Not objHelp.getuserMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Return False
            End If

            ds_fillUserType = Me.objHelp.GetResultSet("select vUserTypeCode,vUserTypeName from UserTypeMst", "View_UserMst")

            'If Not objHelp.getUserTypeMst("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_fillUserType, eStr_Retu) Then
            '    Response.Write(eStr_Retu)
            '    Exit Function
            'End If
            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            Else
                Me.Session(S_UserTypeMst) = ds_fillUserType.Tables(0)
            End If

            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            If ds.Tables(0).Rows.Count <= 0 And _
             Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found For Selected Operation")
            End If

            ds_DWR_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_Data")
            Return False
        Finally

        End Try
    End Function
#End Region

#Region " GenCall_showUI() "

    Private Function GenCall_ShowUI() As Boolean
        Dim dt_UserMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Try
            Page.Title = ":: User Management :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "User Management"

            dt_UserMst = Me.ViewState(VS_DtUserMst)

            Choice = Me.ViewState(VS_Choice)
            DDLLocation.Enabled = True

            Me.txtloginfrom.Attributes.Add("onchange", "dateformatvalidator(1)")
            Me.txtLoginTo.Attributes.Add("onchange", "dateformatvalidator(2)")


            If Not FillDropDown() Then
                Return False
            End If

            Me.btnadd.Attributes.Add("OnClick", "return Validation();")
            Me.btnedit.Attributes.Add("OnClick", "return EditValidation();")

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_showUI")
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
        Dim ds_ScopeMst As New DataSet
        Dim estr As String = String.Empty
        Try

            objHelp.FillDropDown("DeptMst", "vDeptCode", "vDeptName", "cStatusindi <> 'D'", ds_Dept, estr)
            objHelp.FillDropDown("ScopeMst", "nScopeNo", "vScopeName", "cStatusindi <> 'D'", ds_ScopeMst, estr)
            objHelp.FillDropDown("LocationMst", "vLocationCode", "vLocationName", IIf(Session(S_UserType) = SuperUserType, "", "vLocationCode='" + Me.Session(S_LocationCode) + "' AND ") + "cStatusIndi<>'D'", ds_Location, estr)


            Me.DDLScopeMst.DataSource = ds_ScopeMst
            Me.DDLScopeMst.DataValueField = "nScopeNo"
            Me.DDLScopeMst.DataTextField = "vScopeName"
            Me.DDLScopeMst.DataBind()
            Me.DDLScopeMst.Items.Insert(0, New ListItem("Select Scope", 0))

            Me.DDLLocation.DataSource = ds_Location
            Me.DDLLocation.DataValueField = "vLocationCode"
            Me.DDLLocation.DataTextField = "vLocationName"
            Me.DDLLocation.DataBind()
            Me.DDLLocation.Items.Insert(0, New ListItem("Select Location", 0))

            Me.DDLDept.DataSource = ds_Dept
            Me.DDLDept.DataValueField = "vDeptCode"
            Me.DDLDept.DataTextField = "vDeptName"
            Me.DDLDept.DataBind()
            Me.DDLDept.Items.Insert(0, New ListItem("Select Department", 0))

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillDropDown")
            Return False
        End Try
    End Function

#End Region

#Region " RblMode SelectedIndex "

    Protected Sub rblmode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblmode.SelectedIndexChanged
        Try
            If rblmode.SelectedItem.Value.ToUpper.ToString() = "A" Then

                Me.txtUserAdd.Visible = True
                Me.ddlUserEdit.Visible = False
                Me.TblAdd.Visible = True
                Me.lblMode.Text = "Enter User Name*:"
                Me.ViewState(Vs_SelectionMode) = "A"
                Me.btnadd.Visible = True
                Me.btncancel.Visible = True
                Me.btnedit.Visible = False
                Me.ResetPage()

            ElseIf rblmode.SelectedItem.Value.ToUpper.ToString() = "E" Then

                Me.ddlUserEdit.Visible = True
                Me.txtUserAdd.Visible = False
                Me.lblMode.Text = "Select User Name*:"
                Me.FillUserName()
                Me.ViewState(Vs_SelectionMode) = "E"
                Me.btnadd.Visible = False
                Me.TblAdd.Visible = False
                Me.btnedit.Visible = False
                Me.btncancel.Visible = False
                Me.ResetPage()
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)

        End Try

    End Sub

#End Region

#Region "Fill UserName"

    Private Sub FillUserName()

        Dim ds_FillUsername As New DataSet
        Dim dv_FillUsername As New DataView
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim dt_SortUserName As New DataTable
        Try

            ''Below code commented and added by Aaditya for issue to show all user for Superadmin profile
            ''If Not Session(S_UserID) = SuperUserId Then
            If Not Convert.ToString(Session(S_UserType)).Trim() = SuperUserType Then
                wStr = "vLocationCode='" + Me.Session(S_LocationCode) + "' "
                wStr += " Order by vUserName"
                If Not Me.objHelp.getuserMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_FillUsername, eStr) Then
                    Throw New Exception(eStr)
                End If
            Else
                If Not Me.objHelp.getuserMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, _
                                ds_FillUsername, eStr) Then
                    Throw New Exception(eStr)
                End If
            End If

            If ds_FillUsername.Tables(0).Rows.Count > 0 Then

                dv_FillUsername = ds_FillUsername.Tables(0).DefaultView.ToTable(True, "vUserName").DefaultView
                dt_SortUserName = dv_FillUsername.ToTable()
                dt_SortUserName.DefaultView.Sort = "vUserName"
                Me.ddlUserEdit.DataValueField = "vUserName"
                Me.ddlUserEdit.DataTextField = "vUserName"
                Me.ddlUserEdit.DataSource = dt_SortUserName.DefaultView.ToTable()
                Me.ddlUserEdit.DataBind()
                Me.ddlUserEdit.Items.Insert(0, " Select User Name ")

                For iMedexGroup As Integer = 0 To ddlUserEdit.Items.Count - 1
                    ddlUserEdit.Items(iMedexGroup).Attributes.Add("class", "SelectItem")
                Next iMedexGroup
            End If


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)

        End Try

    End Sub

#End Region

#Region "Fill ActiveUserName"

    Private Sub FillActiveUserName(ByVal wStr_UserTypeCode As String)

        Dim ds_FillActiveUser As New DataSet
        Dim dt_FillActiveUser As New DataTable
        Dim wStr_ActUser As String


        wStr_ActUser = "vUserTypeCode ='" + wStr_UserTypeCode + "' and vUserName = '" + Me.ViewState(VS_UserName) + "'"


        Try
            If Not objHelp.getuserMst(wStr_ActUser, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_FillActiveUser, eStr_retu) Then
                Throw New Exception(eStr)
            End If


            If ds_FillActiveUser.Tables(0).Rows.Count > 0 Then
                dt_FillActiveUser = ds_FillActiveUser.Tables(0)
                Me.ViewState(VS_ActiveUserMst) = dt_FillActiveUser

                If dt_FillActiveUser.Rows(0).Item("cStatusIndi").ToString.ToUpper() = "D" Then
                    Me.AssignValues("ACTIVE")
                    Me.ViewState(VS_ActFlag) = "ACT"
                Else
                    Me.AssignValues("INACTIVE")
                    Me.ViewState(VS_ActFlag) = "INACT"
                End If

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)

        End Try

    End Sub
#End Region

#Region " Button Events "
    Protected Sub btnadd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnadd.Click
        Try
            'test for validation
            If Me.txtUserAdd.Text = "" Then
                Me.objCommon.ShowAlert("Pls Enter User Name", Me.Page)
                Exit Sub
            End If

            Me.ViewState(VS_UserName) = Me.txtUserAdd.Text.Trim()

            'If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
            'End If
            If Me.rblmode.SelectedValue.ToString() = "A" Then
                If Not ValidateUserBeforeSave() Then
                    Exit Sub
                End If
            End If
            Me.fillAddGrid()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
        End Try
    End Sub

    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Me.ResetPage()
    End Sub

    Protected Sub btnedit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnedit.Click
        Dim Ds_SaveEditUser As New DataSet
        Dim dt_EditUser As New DataTable
        Dim dt_CopyEdit As New DataTable
        Dim Wstr_UserNm As String = String.Empty
        Try
            If Me.ddlUserEdit.SelectedIndex = -1 Then
                Me.objCommon.ShowAlert("Please Select User Name", Me.Page)
                Exit Sub
            End If
            ''Commented and Added by Aaditya for Solve Issue of old data updation
            Me.ViewState(VS_UserName) = Me.ddlUserEdit.SelectedItem.Value.Trim()
            Wstr_UserNm = "vUserName = '" + Me.ddlUserEdit.SelectedItem.Value.Trim() + "'"

            If Not Me.objHelp.getuserMst(Wstr_UserNm, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_SaveEditUser, eStr_retu) Then
                Throw New Exception(eStr)
                Exit Sub
            End If

            dt_EditUser = Ds_SaveEditUser.Tables(0)
            For Each dr In dt_EditUser.Rows
                dr("iUserGroupCode") = 2
                dr("nScopeNo") = Me.DDLScopeMst.SelectedItem.Value.Trim()
                dr("vUserName") = Me.ddlUserEdit.SelectedItem.Text.Trim()
                dr("vFirstName") = Me.TxtFirstName.Text.Trim()
                dr("vLastName") = Me.TxtLastName.Text.Trim()
                dr("vLocationCode") = Me.DDLLocation.SelectedValue.Trim()
                dr("vDeptCode") = Me.DDLDept.SelectedValue.Trim()
                dr("vEmailId") = Me.txtEmail.Text.Trim()
                dr("vPhoneNo") = Me.txtPhNo.Text.Trim()
                dr("vExtNo") = Me.txtExtNo.Text.Trim()
                dr("vRemark") = Me.txtRemark.Text.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)

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

                If chkmfa.Checked = True Then
                    dr("isMFA") = "Y"
                Else
                    dr("isMFA") = "N"
                End If

                If chkEmail.Checked = True Then
                    If (Me.txtEmail.Text.Trim() = "") Then
                        objCommon.ShowAlert("Please Enter Email", Me.Page)
                        Exit Sub
                    Else
                        dr("isMFAEmail") = "Y"
                    End If

                Else
                        dr("isMFAEmail") = "N"
                End If

                If chksms.Checked = True Then
                    If (Me.txtPhNo.Text.Trim() = "") Then
                        objCommon.ShowAlert("Please Enter Phone No.", Me.Page)
                        Exit Sub
                    Else
                        dr("isMFASms") = "Y"
                    End If
                Else
                    dr("isMFASms") = "N"
                End If

                If chkmfa.Checked = True And chkEmail.Checked <> True And chksms.Checked <> True Then
                    objCommon.ShowAlert("Please select at leaset one chekbox", Me.Page)
                    Exit Sub
                End If

                'if Condtion added by Bhargav Thaker 27Feb2023
                If dr("UUserID").ToString() Is Nothing OrElse dr("UUserID").ToString() = "" Then
                    dr("UUserID") = Guid.NewGuid()
                End If
                dr.AcceptChanges()
            Next
            dt_EditUser.AcceptChanges()
            ''Ended by Aaditya
            ''Ds_SaveEdit = New DataSet
            ''Dt_saveEdit = CType(Me.ViewState(VS_EditUserName), DataTable).Copy()
            dt_EditUser.TableName = "UserMst"
            dt_EditUser = dt_EditUser.Copy()
            Dim Ds_SaveEdit = New DataSet
            Ds_SaveEdit.Tables.Add(dt_EditUser)

            If Not objPVNet.Save_InsertUserMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_UserMst, Ds_SaveEdit, Me.Session(S_UserID), eStr) Then
                objCommon.ShowAlert("Error While Saving UserMst", Me.Page)
                Exit Sub
            Else
                objCommon.ShowAlert("User Updated Successfully", Me.Page)
                Me.ResetPage()
                Me.ddlUserEdit.SelectedIndex = -1
            End If

        Catch ThreadEx As Threading.ThreadAbortException

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
        End Try
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim ds_Lockedusers As New DataSet
        Dim ds_Unlockuser As New DataSet
        Dim drNew As DataRow
        Dim IsActive As String

        CurrentIndex = HFCurrentIndex.Value

        If (btnSave.Text.Contains("Change Status")) Then
            Try
                Dim ds_EditSave As New DataSet
                Dim dt_EditUserManage As New DataTable
                '' Me.FillActiveUserName(Me.gvwUserType.Rows(CurrentIndex).Cells(GVC_UserTypeNo).Text.Trim())

                If (Me.ViewState(VS_ActFlag) = "ACT") Then
                    IsActive = "Active"
                Else
                    IsActive = "InActive"
                End If

                ds_EditSave = New DataSet
                dt_EditUserManage = CType(Me.ViewState(VS_ActiveUserMst), DataTable).Copy()
                dt_EditUserManage.TableName = "UserMst"
                ds_EditSave.Tables.Add(dt_EditUserManage)

                For Each dr In ds_EditSave.Tables(0).Rows
                    dr("vRemark") = txtRemarks.Text.Trim()
                    dr("IsActive") = IsActive
                Next

                ds_EditSave.AcceptChanges()

                If Not objPVNet.Save_InsertUserMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_UserMst, ds_EditSave, Me.Session(S_UserID), eStr) Then
                    objCommon.ShowAlert("Error While Saving UserMst", Me.Page)
                    Exit Sub
                Else
                    objCommon.ShowAlert("Status Changed Successfully", Me.Page)
                    'Dim status As String = String.Empty
                    'status = ds_EditSave.Tables(0).Rows(0).Item("cStatusindi")
                    'If Me.ViewState(VS_ActFlag) = "ACT" And status = "N" Or status = "E" Then
                    If Me.ViewState(VS_ActFlag) = "ACT" Then
                        CType(Me.gvwUserType.Rows(CurrentIndex).FindControl("LnkGrdstatus"), LinkButton).Enabled = True
                        CType(Me.gvwUserType.Rows(CurrentIndex).FindControl("LnkGrdstatus"), LinkButton).Text = "Inactive"
                        CType(Me.gvwUserType.Rows(CurrentIndex).FindControl("lblCurrentStatus"), Label).Text = "Active"
                        CType(Me.gvwUserType.Rows(CurrentIndex).FindControl("lnkGrdRstPwd"), LinkButton).Enabled = True
                    ElseIf Me.ViewState(VS_ActFlag) = "INACT" Then
                        CType(Me.gvwUserType.Rows(CurrentIndex).FindControl("LnkGrdstatus"), LinkButton).Enabled = True
                        CType(Me.gvwUserType.Rows(CurrentIndex).FindControl("LnkGrdstatus"), LinkButton).Text = "Active"
                        CType(Me.gvwUserType.Rows(CurrentIndex).FindControl("lblCurrentStatus"), Label).Text = "Inactive"
                        CType(Me.gvwUserType.Rows(CurrentIndex).FindControl("lnkGrdRstPwd"), LinkButton).Enabled = False
                    End If
                End If
            Catch ex As Exception
                ShowErrorMessage("", "Error While Save in Insert_UserMst ")
                ModalRemarks.Hide()
            Finally
                ModalRemarks.Hide()
                txtRemarks.Text = String.Empty
            End Try
        ElseIf (btnSave.Text.Contains("UnLock User")) Then
            Try
                Dim wStr As String
                wStr = "vUserName='" + Me.gvwUserType.Rows(CurrentIndex).Cells(GVC_LoginName).Text.Trim() + "' And ISNULL(cStatusIndi,'N') <> 'D' Order by nSrNo desc"

                If Not objHelp.GetUserLoginFailureDetails(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                            ds_Lockedusers, eStr_retu) Then
                    Throw New Exception(eStr_retu)
                End If

                ds_Unlockuser = ds_Lockedusers.Copy()
                ds_Unlockuser.Tables(0).Clear()
                drNew = ds_Unlockuser.Tables(0).NewRow()

                For Each dr In ds_Lockedusers.Tables(0).Rows
                    If Convert.ToString(dr("cBlockedFlag")) = "B" Then
                        drNew("vUserName") = dr("vUserName")
                        drNew("dLastFailedLogin") = dr("dLastFailedLogin")
                        drNew("nAttemptCount") = 1
                        drNew("cBlockedFlag") = "N"
                        drNew("vIPAddress") = dr("vIPAddress")
                        drNew("dModifyOn") = dr("dModifyOn")
                        drNew("cStatusIndi") = "N"
                        drNew("iUserId") = dr("iUserId")
                        drNew("iModifyBy") = CType(Session(S_UserID), Integer)
                        drNew("vRemarks") = txtRemarks.Text.Trim()
                        Exit For
                    End If
                Next

                ds_Unlockuser.Tables(0).Rows.Add(drNew)
                ds_Unlockuser.AcceptChanges()

                If Not Me.objPVNet.Save_UserLoginFailureDetails(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add,
                                                                ds_Unlockuser, eStr_retu) Then
                    Throw New Exception
                End If
                Me.objCommon.ShowAlert("Username Unlocked Successfully", Me.Page)
                Me.gvwUserType.Rows(CurrentIndex).Cells(GVC_Unlock).Enabled = False
            Catch ex As Exception
            Finally
                txtRemarks.Text = String.Empty
            End Try
        End If
    End Sub

    Protected Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim ds As DataSet
        Dim eStr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim ProfileName As String
        Dim UserName As String = ddlUserEdit.SelectedItem.ToString()
        Dim filename As String = String.Empty
        Dim isReportComplete As Boolean = False
        Dim str() As String = HFProfileName.Value.Split("+")
        ProfileName = str(0) + "_" + str(1)
        Dim prof As String
        Dim Location As String
        Dim dtTempAuditTrail As New DataTable

        For i As Integer = 0 To str.Length - 1
            prof = str(i).ToString()
        Next

        ProfileName = str(0).Substring(3, str(0).Length - 3)

        Try
            If Not objHelp.Proc_AuditTrailofActiveInactiveUser(prof, ProfileName, ds, eStr) Then
                Throw New Exception(eStr)
            End If

            Dim column As DataColumn
            column = New DataColumn()
            column.DataType = System.Type.GetType("System.Int32")
            column.ColumnName = "Sr.No"
            ds.Tables(0).Columns.Add(column)
            ds.AcceptChanges()

            Dim Count As Integer = 0
            Dim drAuditTrail As DataRow

            For Each dr In ds.Tables(0).Rows
                Count = Count + 1
                dr("Sr.No") = Count
            Next
            ds.AcceptChanges()

            If ds.Tables(0).Rows.Count > 0 Then

                Location = ds.Tables(0).Rows(0)("IST").ToString().Split(" ")(2).ToString().Split("(")(1)
                Location = IIf(Location = "+05:30", "India standard time ", "Eastern standard time")

            End If

            If Not dtTempAuditTrail Is Nothing Then
                dtTempAuditTrail.Columns.Add("Sr.No.", GetType(String))
                dtTempAuditTrail.Columns.Add("UserName", GetType(String))
                dtTempAuditTrail.Columns.Add("Remark", GetType(String))
                dtTempAuditTrail.Columns.Add("User Status", GetType(String))
                dtTempAuditTrail.Columns.Add("User Type", GetType(String))
                dtTempAuditTrail.Columns.Add("ModifyBy", GetType(String))
                dtTempAuditTrail.Columns.Add("ModifyOn", GetType(String))
                dtTempAuditTrail.Columns.Add(Location, GetType(String))
            End If

            dtTempAuditTrail.AcceptChanges()


            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                drAuditTrail = dtTempAuditTrail.NewRow()
                drAuditTrail("Sr.No.") = i + 1
                drAuditTrail("User Type") = IIf(Convert.ToString(ds.Tables(0).Rows(i)("vUserTypeCode")) = "", Convert.ToString(ds.Tables(0).Rows(0)("vUserTypeCode")), Convert.ToString(ds.Tables(0).Rows(i)("vUserTypeCode")))
                drAuditTrail("Remark") = Convert.ToString(ds.Tables(0).Rows(i)("vRemark").ToString())
                drAuditTrail("User Status") = Convert.ToString(ds.Tables(0).Rows(i)("cBlockedFlag"))
                drAuditTrail("UserName") = Convert.ToString(ds.Tables(0).Rows(i)("vUserName"))
                drAuditTrail("ModifyBy") = Convert.ToString(ds.Tables(0).Rows(i)("ModifyBy"))
                drAuditTrail("ModifyOn") = Convert.ToString(CDate(ds.Tables(0).Rows(i)("dModifyOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                drAuditTrail(Location) = Convert.ToString(ds.Tables(0).Rows(i)("IST"))
                dtTempAuditTrail.Rows.Add(drAuditTrail)
                dtTempAuditTrail.AcceptChanges()
            Next i


            gvExport.DataSource = dtTempAuditTrail
            gvExport.DataBind()

            If gvExport.Rows.Count > 0 Then
                Dim info As String = String.Empty
                Dim gridviewHtml As String = String.Empty

                filename = "Audit Trail " + ".xls"

                isReportComplete = True
                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                gvExport.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                gridviewHtml = "<table><tr><td align = ""center"" colspan=""7""><b>" + System.Configuration.ConfigurationManager.AppSettings("Client").ToString() + "<br/></b></td></tr></table><table><tr><td align = ""center"" colspan=""2""><b>" + " Active/In Active Report of : " + UserName + "<br/></b></td></tr></table><span>Print Date:-  " + Date.Today.ToString("dd-MMM-yyyy") + "</span>" + gridviewHtml

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

                filename = "Audit Trail " + ".xls"


                drAuditTrail = dtTempAuditTrail.NewRow()
                drAuditTrail("Sr.No.") = ""
                drAuditTrail("User Type") = ""
                drAuditTrail("Remark") = ""
                drAuditTrail("User Status") = ""
                drAuditTrail("UserName") = ""
                drAuditTrail("ModifyBy") = ""
                drAuditTrail("ModifyOn") = ""
                dtTempAuditTrail.Rows.Add(drAuditTrail)
                dtTempAuditTrail.AcceptChanges()



                gvExport.DataSource = dtTempAuditTrail
                gvExport.DataBind()


                isReportComplete = True
                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                gvExport.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                gridviewHtml = "<table><tr><td align = ""center"" colspan=""7""><b>" + System.Configuration.ConfigurationManager.AppSettings("Client").ToString() + "<br/></b></td></tr></table><table><tr><td align = ""center"" colspan=""2""><b>" + " Active/In Active Report of : " + UserName + "<br/></b></td></tr></table><span>Print Date:-  " + Date.Today.ToString("dd-MMM-yyyy") + "</span>" + gridviewHtml

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
            Throw New Exception(ex.Message)
        End Try
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub

#End Region

#Region " Reset Page "

    Private Sub ResetPage()

        Me.txtUserAdd.Text = ""
        'Me.txtUName.Text = ""
        Me.txtRemark.Text = ""
        Me.txtPhNo.Text = ""
        Me.TxtLastName.Text = ""
        Me.TxtFirstName.Text = ""
        Me.txtExtNo.Text = ""
        Me.txtEmail.Text = ""
        Me.DDLScopeMst.SelectedIndex = -1
        Me.DDLLocation.SelectedIndex = -1
        Me.DDLDept.SelectedIndex = -1
        Me.gvwUserType.DataSource = Nothing
        Me.gvwUserType.DataBind()
        Me.ddlUserEdit.SelectedIndex = -1
        Me.txtloginfrom.Text = ""
        Me.txtLoginTo.Text = ""
        Me.chkmfa.Checked = False
        Me.chkEmail.Checked = False
        Me.chksms.Checked = False
        trmfssend.Visible = False

    End Sub

#End Region

#Region " Create Table "
    Private Sub AddColumnUserTypeDtl() 'As DataTable
        Dim DtUserType As New DataTable
        Dim LoginName As DataColumn = New DataColumn("LoginName")
        Dim LoginPass As DataColumn = New DataColumn("LoginPass")
        Dim Add As DataColumn = New DataColumn("Add")
        Dim RstPwd As DataColumn = New DataColumn("ResetPassword")
        Dim currentstatus As DataColumn = New DataColumn("CurrentStatus")
        Dim Status As DataColumn = New DataColumn("ChangeStatusTo")

        Dim UserChar As String = String.Empty


        Dim dr As DataRow

        DtUserType = CType(Me.Session(S_UserTypeMst), Data.DataTable).Copy()

        Try

            LoginName.DataType = System.Type.GetType("System.String")
            DtUserType.Columns.Add(LoginName)

            LoginPass.DataType = System.Type.GetType("System.String")
            DtUserType.Columns.Add(LoginPass)

            LoginName.DataType = System.Type.GetType("System.String")
            DtUserType.Columns.Add(Add)

            LoginName.DataType = System.Type.GetType("System.String")
            DtUserType.Columns.Add(RstPwd)

            LoginName.DataType = System.Type.GetType("System.String")
            DtUserType.Columns.Add(currentstatus)

            LoginName.DataType = System.Type.GetType("System.String")
            DtUserType.Columns.Add(Status)



            DtUserType.AcceptChanges()

            For Each dr In DtUserType.Rows
                UserChar = dr("vUserTypeName").ToString()

                If UserChar.Length > 3 Then
                    UserChar = UserChar.Substring(0, 3)
                End If

                dr("LoginName") = UserChar + "_" + Me.ViewState(VS_UserName)
                dr("LoginPass") = Me.ViewState(VS_UserName)
                dr("Add") = ""
                dr("ResetPassword") = ""
                dr("CurrentStatus") = ""
                dr("ChangeStatusTo") = ""



                DtUserType.AcceptChanges()
            Next

            Me.ViewState(VS_DtUserType) = DtUserType

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......AddColumnUserTypeDtl")
        End Try
    End Sub
#End Region

#Region " Fill Grid"
    Private Sub fillAddGrid()
        Try
            Me.AddColumnUserTypeDtl()
            Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            Me.AssignValues("ADD")
            Me.gvwUserType.Visible = True
            Me.gvwUserType.DataSource = Me.ViewState(VS_DtUserType)
            Me.gvwUserType.DataBind()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
        End Try
    End Sub
#End Region

#Region "GridView Events"
    Protected Sub gvwUserType_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwUserType.RowCommand

        Dim dt_UserManage As New DataTable
        Dim ds_EditSave As New DataSet
        Dim dt_EditUserManage As New DataTable
        Dim ds_Save As New DataSet
        Dim EncryptPwd As String = String.Empty
        Dim isPasswordChanged As Boolean = False
        Dim dr_UsrMng As DataRow
        Dim Currindex As Integer = e.CommandArgument
        CurrentIndex = Currindex
        HFCurrentIndex.Value = Currindex
        Dim ds_Lockedusers As New DataSet
        Dim wStr As String = String.Empty
        Dim ds_Unlockuser As New DataSet
        Dim Wstr_UserNm As String = String.Empty

        Try
            If e.CommandName.ToUpper = "MYADD" Then

                dt_UserManage = Me.ViewState(VS_DtUserMst)
                dr_UsrMng = dt_UserManage.Rows(0)
                dr_UsrMng("vUserTypeCode") = Me.gvwUserType.Rows(Currindex).Cells(GVC_UserTypeNo).Text.Trim()
                dr_UsrMng("vLoginName") = Me.gvwUserType.Rows(Currindex).Cells(GVC_LoginName).Text.Trim()
                dr_UsrMng("IsActive") = "Active"
                '====added on 9-12-09 by Deepak Singh=====
                EncryptPwd = Me.objHelp.EncryptPassword(Me.gvwUserType.Rows(Currindex).Cells(GVC_LoginPass).Text.Trim())
                dr_UsrMng("vLoginPass") = EncryptPwd
                '===============
                'dr_UsrMng("vLoginPass") = Me.gvwUserType.Rows(Currindex).Cells(GVC_LoginPass).Text.Trim()
                dr_UsrMng("vUserName") = Me.ViewState(VS_UserName)

                dt_UserManage.AcceptChanges()

                ds_Save = New DataSet
                dt_UserManage = CType(Me.ViewState(VS_DtUserMst), DataTable)
                dt_UserManage.TableName = "UserMst"
                ds_Save.Tables.Add(dt_UserManage)

                If Not objPVNet.Save_InsertUserMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_UserMst, ds_Save, Me.Session(S_UserID), eStr) Then
                    objCommon.ShowAlert("Error While Saving UserMst", Me.Page)
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Update Start", "Updated();", True)
                    Exit Sub
                Else
                    objCommon.ShowAlert("User Type Added Successfully", Me.Page)
                    If Not ValidateBeforeSave(Me.gvwUserType.Rows(Currindex).Cells(GVC_UserTypeNo).Text.Trim()) Then
                        CType(Me.gvwUserType.Rows(Currindex).FindControl("lnkGrdADD"), LinkButton).Enabled = False
                        CType(Me.gvwUserType.Rows(Currindex).FindControl("lnkGrdRstPwd"), LinkButton).Enabled = True
                        CType(Me.gvwUserType.Rows(Currindex).FindControl("lnkGrdstatus"), LinkButton).Enabled = True
                        CType(Me.gvwUserType.Rows(Currindex).FindControl("lnkGrdstatus"), LinkButton).Text = "Inactive"
                        CType(Me.gvwUserType.Rows(Currindex).FindControl("lblCurrentStatus"), Label).Text = "Active"

                    End If
                    If isPasswordChanged Then
                        'Insert Row in Password History Master
                        If Not objPVNet.Insert_ChangePassword(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add,
                                                               ds_Save, Me.Session(S_UserID), eStr) Then
                            objCommon.ShowAlert("Error While Saving Password History", Me.Page)
                            Exit Sub
                        End If
                    End If
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Update Start", "Updated();", True)
                End If
            ElseIf e.CommandName.ToUpper = "RESETPWD" Then
                'CType(Me.gvwUserType.Rows(Currindex).FindControl("lnkGrdRstPwd"), LinkButton).PostBackUrl = "frmChangePwd.aspx?vUserName=" & Me.ViewState(VS_UserName) & "&Mode=RstPwd" & "&vUserTypeCode=" & Me.gvwUserType.Rows(Currindex).Cells(GVC_UserTypeNo).Text.Trim()
            ElseIf e.CommandName.ToUpper = "STATUS" Then
                Me.FillActiveUserName(Me.gvwUserType.Rows(Currindex).Cells(GVC_UserTypeNo).Text.Trim())
                btnSave.Text = "Change Status"
                lblChangeStatus.Text = "Changing status to <b>" + "" + e.CommandSource.Text.ToString() + "</b>"
                ModalRemarks.Show()

            ElseIf e.CommandName.ToUpper = "UNLOCK" Then
                btnSave.Text = "UnLock User"
                ModalRemarks.Show()

            End If

        Catch Threadex As Threading.ThreadAbortException

        Catch ex As Exception

            Me.ShowErrorMessage(ex.Message, eStr)

        End Try
    End Sub

    Protected Sub LnkGrdRstPwd_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim q As String = String.Empty
        Dim gvr As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        ' q = "window.open(""" + "frmChangePwd.aspx?vUserName=" + Me.ViewState(VS_UserName) + "&Mode=RstPwd" + "&vUserTypeCode=" + gvr.Cells(GVC_UserTypeNo).Text.Trim() + """" + ")"
        q = "window.open(""" + "frmChangePwd.aspx?vUserName=" + Me.ViewState(VS_UserName) + "&Mode=RstPwd" + "&Reset=Y" + "&vUserTypeCode=" + gvr.Cells(GVC_UserTypeNo).Text.Trim() + """" + ")"
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", q, True)
    End Sub

    Protected Sub gvwUserType_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvwUserType.PageIndexChanging

    End Sub

    Protected Sub gvwUserType_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwUserType.RowDataBound
        Dim LoginName As String = String.Empty

        If e.Row.RowType = DataControlRowType.DataRow Then


            CType(e.Row.FindControl("lnkGrdADD"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkGrdADD"), LinkButton).CommandName = "MYADD"

            CType(e.Row.FindControl("lnkGrdRstPwd"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkGrdRstPwd"), LinkButton).CommandName = "RESETPWD"

            CType(e.Row.FindControl("LnkGrdstatus"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("LnkGrdstatus"), LinkButton).CommandName = "STATUS"

            CType(e.Row.FindControl("LnkGrdUnLock"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("LnkGrdUnLock"), LinkButton).CommandName = "UNLOCK"



            If Not ValidateBeforeSave(e.Row.Cells(GVC_UserTypeNo).Text.Trim()) Then
                CType(e.Row.FindControl("lnkGrdADD"), LinkButton).Enabled = False
                CType(e.Row.FindControl("LnkGrdstatus"), LinkButton).Enabled = True
                CType(e.Row.FindControl("LnkGrdstatus"), LinkButton).Text = "Inactive"
                CType(e.Row.FindControl("lblCurrentStatus"), Label).Text = "Active"
                CType(e.Row.FindControl("lnkGrdRstPwd"), LinkButton).Enabled = True

            End If

            If Not ValidateUserStatus(e.Row.Cells(GVC_UserTypeNo).Text.Trim()) Then
                CType(e.Row.FindControl("LnkGrdstatus"), LinkButton).Enabled = True
                CType(e.Row.FindControl("LnkGrdstatus"), LinkButton).Text = "Active"
                CType(e.Row.FindControl("lblCurrentStatus"), Label).Text = "Inactive"
                CType(e.Row.FindControl("lnkGrdRstPwd"), LinkButton).Enabled = False
            End If


        End If

        If Me.rblmode.SelectedItem.Text.ToUpper().Trim() = "EDIT" Then
            LoginName = Me.ResetLoginName(e.Row.Cells(GVC_UserTypeNo).Text.Trim())
            If LoginName <> "" Then
                e.Row.Cells(GVC_LoginName).Text = LoginName
            End If
        End If

        e.Row.Cells(GVC_Unlock).Enabled = False
        If Me.ViewState(Vs_SelectionMode) = "E" Then
            If e.Row.RowType = DataControlRowType.DataRow Then

                If Not ValidateUserLocked(e.Row.Cells(GVC_UserTypeNo).Text.Trim()) Then
                    e.Row.Cells(GVC_Unlock).Enabled = True
                End If
            End If
        End If

        If e.Row.RowType = DataControlRowType.DataRow Or _
            e.Row.RowType = DataControlRowType.Header Or _
            e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVC_LoginPass).Visible = False
            e.Row.Cells(GVC_UserTypeNo).Visible = False

        End If


    End Sub

#End Region

    Private Function ValidateBeforeSave(ByVal UserTypeCode As String) As Boolean
        Dim wStr_ValidateUser As String = String.Empty
        Dim dsClientMst As New DataSet
        ValidateBeforeSave = False

        If Me.ViewState(Vs_SelectionMode) = "A" Then
            wStr_ValidateUser = "vUserName='" + Me.txtUserAdd.Text.Trim + "' and vUserTypeCode='" + UserTypeCode + "'"
        ElseIf Me.ViewState(Vs_SelectionMode) = "E" Then
            wStr_ValidateUser = "vUserName='" + Me.ddlUserEdit.SelectedItem.Text.Trim + "' and vUserTypeCode='" + UserTypeCode + "'"

        End If

        Try
            If Not objHelp.getuserMst(wStr_ValidateUser, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    dsClientMst, eStr_retu) Then
                Throw New Exception(eStr_retu)
            End If

            If dsClientMst.Tables(0).Rows.Count <= 0 Then
                ValidateBeforeSave = True
            Else
                ValidateBeforeSave = False
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...ValidateBeforeSave")
        End Try

    End Function

    Private Function ValidateUserBeforeSave() As Boolean
        Dim wStr_ValidateUser As String = String.Empty
        Dim dsClientMst As New DataSet
        ValidateUserBeforeSave = False
        Try

            'Added By Pratik Soni To Avoid Duplication of "FirstName & LastName"

            wStr_ValidateUser = "cStatusIndi <> 'D' AND vFirstName = '" + Me.TxtFirstName.Text.Trim.ToString + "' AND vLastName = '" + Me.TxtLastName.Text.Trim.ToString + "'"
            If Not objHelp.getuserMst(wStr_ValidateUser, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsClientMst, eStr) Then
                Me.ShowErrorMessage("Error While Getting Data From UserMst", eStr)
                Return False
            End If

            If dsClientMst.Tables(0).Rows.Count > 0 Then
                ValidateUserBeforeSave = False
                objCommon.ShowAlert("FirstName & LastName Already Exist!", Me.Page)
                Return False
            End If
            '--------------------------------------------------------------------

            wStr_ValidateUser = "vUserName='" + Me.ViewState(VS_UserName) + "'"

            If Not objHelp.getuserMst(wStr_ValidateUser, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    dsClientMst, eStr_retu) Then
                Throw New Exception(eStr_retu)
            End If

            If dsClientMst.Tables(0).Rows.Count <= 0 Then
                ValidateUserBeforeSave = True
            Else
                ValidateUserBeforeSave = False
                objCommon.ShowAlert("Username Already Exists !!!", Me)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....ValidateUserBeforeSave")
        End Try

    End Function

    Private Function ValidateUserStatus(ByVal UserTypeCode As String) As Boolean
        Dim wStr_ValidateUser As String = String.Empty
        Dim dsClientMst As New DataSet
        ValidateUserStatus = False

        If Me.ViewState(Vs_SelectionMode) = "A" Then
            wStr_ValidateUser = "vUserName='" + Me.txtUserAdd.Text.Trim + "' and vUserTypeCode='" + UserTypeCode + "' and cStatusIndi = 'D'"
        ElseIf Me.ViewState(Vs_SelectionMode) = "E" Then
            wStr_ValidateUser = "vUserName='" + Me.ddlUserEdit.SelectedItem.Text.Trim + "' and vUserTypeCode='" + UserTypeCode + "' and cStatusIndi = 'D'"
        End If

        Try
            If Not objHelp.getuserMst(wStr_ValidateUser, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    dsClientMst, eStr_retu) Then
                Throw New Exception(eStr_retu)
            End If

            If dsClientMst.Tables(0).Rows.Count <= 0 Then
                ValidateUserStatus = True
            Else
                ValidateUserStatus = False
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...ValidateUserStatus")
        End Try

    End Function

    Private Function ValidateUserLocked(ByVal LoginName As String) As Boolean
        Dim wStr_ValidateUser As String = String.Empty
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim ds1 As DataSet
        Dim wstr As String

        Try
            wstr = "vUserTypecode='" + LoginName.Trim + " ' AND vUserName = '" + ddlUserEdit.SelectedValue.Trim + "'"

            If Not objHelp.getuserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds1, eStr_retu) Then
                Response.Write(eStr_retu)
                Return False
            End If

        Catch ex As Exception

        End Try

        Dim userid As String = ""
        For Each dr1 As DataRow In ds1.Tables(0).Rows
            userid = dr1("iUserId").ToString()
        Next

        ValidateUserLocked = True

        If userid <> "" Then

            wStr_ValidateUser = "iUserId =" + userid + "order by dModifyOn desc"

            Try
                If Not objHelp.GetUserLoginFailureDetails(wStr_ValidateUser, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        ds, eStr_retu) Then
                    Throw New Exception(eStr_retu)
                End If

                For Each dr In ds.Tables(0).Rows
                    If Convert.ToString(dr("cBlockedFlag")) = "B" Then
                        ValidateUserLocked = False
                        Exit Function
                    Else
                        ValidateUserLocked = False
                        Exit For
                    End If
                Next
                ValidateUserLocked = True
            Catch ex As Exception
                Me.ShowErrorMessage(ex.Message, "....ValidateUserLocked")
            End Try
        End If

    End Function

    Private Function ResetLoginName(ByVal UserTypeCode As String) As String
        Dim wStr_ValidateUser As String = String.Empty
        Dim dsUserMst As New DataSet
        ResetLoginName = ""

        Try
            wStr_ValidateUser = "vUserName='" + Me.ddlUserEdit.SelectedItem.Text.Trim + "' and vUserTypeCode='" + UserTypeCode + "'" +
                                " and ISNULL(cstatusindi,'N') <> 'D'"

            If Not objHelp.getuserMst(wStr_ValidateUser, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                      dsUserMst, eStr_retu) Then
                Throw New Exception(eStr_retu)
            End If

            If dsUserMst.Tables(0).Rows.Count <= 0 Then
                ResetLoginName = ""
                Return ResetLoginName
            Else
                ResetLoginName = Convert.ToString(dsUserMst.Tables(0).Rows(0)("vLoginName"))
                Return ResetLoginName
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....ResetLoginName")
        End Try
    End Function

#Region "Assign values"
    Private Sub AssignValues(ByVal type As String)
        Dim dr As DataRow
        Dim dt_User As New DataTable
        Dim dt_EditUser As New DataTable
        Dim dt_ActiveUser As New DataTable
        Dim wstr As String = String.Empty
        Dim ds_FillEditUser As DataSet = Nothing

        If type.ToUpper = "ADD" Then
            dt_User = CType(Me.ViewState(VS_DtUserMst), DataTable)
            dt_User.Clear()
            dr = dt_User.NewRow()
            dr("iUserId") = "0000"
            dr("iUserGroupCode") = 2
            dr("nScopeNo") = Me.DDLScopeMst.SelectedItem.Value.Trim()
            dr("vUserName") = Me.txtUserAdd.Text.Trim()
            dr("vFirstName") = Me.TxtFirstName.Text.Trim()
            dr("vLastName") = Me.TxtLastName.Text.Trim()
            dr("vLoginName") = ""
            dr("vLoginPass") = ""
            dr("vUserTypeCode") = 0
            dr("vLocationCode") = Me.DDLLocation.SelectedValue.Trim()
            dr("vDeptCode") = Me.DDLDept.SelectedValue.Trim()
            dr("vEmailId") = Me.txtEmail.Text.Trim()
            dr("vPhoneNo") = Me.txtPhNo.Text.Trim()
            dr("vExtNo") = Me.txtExtNo.Text.Trim()
            dr("vRemark") = Me.txtRemark.Text.Trim()
            dr("iModifyBy") = Me.Session(S_UserID)

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

            If chkmfa.Checked = True Then
                dr("isMFA") = "Y"
            Else
                dr("isMFA") = "N"
            End If

            If chkEmail.Checked = True Then
                If (Me.txtEmail.Text.Trim() = "") Then
                    objCommon.ShowAlert("Please Enter Email", Me.Page)
                    Exit Sub
                Else
                    dr("isMFAEmail") = "Y"
                End If

            Else
                dr("isMFAEmail") = "N"
            End If

            If chksms.Checked = True Then
                If (Me.txtPhNo.Text.Trim() = "") Then
                    objCommon.ShowAlert("Please Enter Phone No.", Me.Page)
                    Exit Sub
                Else
                    dr("isMFASms") = "Y"
                End If
            Else
                dr("isMFASms") = "N"
            End If

            If chkmfa.Checked = True And chkEmail.Checked <> True And chksms.Checked <> True Then
                objCommon.ShowAlert("Please select at leaset one chekbox", Me.Page)
                Exit Sub
            End If
            dt_User.Rows.Add(dr)
            Me.ViewState(VS_DtUserMst) = dt_User
        ElseIf type.ToUpper = "EDIT" Then
            wstr = "vUserName = '" + Me.ddlUserEdit.SelectedItem.Value.Trim() + "'"
            If Not Me.objHelp.getuserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_FillEditUser, eStr_retu) Then
                Throw New Exception(eStr)
                Exit Sub
            End If
            dt_EditUser = ds_FillEditUser.Tables(0)
            For Each dr In dt_EditUser.Rows
                dr("iUserGroupCode") = 2
                dr("nScopeNo") = Me.DDLScopeMst.SelectedItem.Value.Trim()
                dr("vUserName") = Me.ddlUserEdit.SelectedItem.Text.Trim()
                dr("vFirstName") = Me.TxtFirstName.Text.Trim()
                dr("vLastName") = Me.TxtLastName.Text.Trim()
                dr("vLocationCode") = Me.DDLLocation.SelectedValue.Trim()
                dr("vDeptCode") = Me.DDLDept.SelectedValue.Trim()
                dr("vEmailId") = Me.txtEmail.Text.Trim()
                dr("vPhoneNo") = Me.txtPhNo.Text.Trim()
                dr("vExtNo") = Me.txtExtNo.Text.Trim()
                dr("vRemark") = Me.txtRemark.Text.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)

                dr("cStatusIndi") = "E"
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
                If chkmfa.Checked = True Then
                    dr("isMFA") = "Y"
                Else
                    dr("isMFA") = "N"
                End If

                If chkEmail.Checked = True Then
                    If (Me.txtEmail.Text.Trim() = "") Then
                        objCommon.ShowAlert("Please Enter Email", Me.Page)
                        Exit Sub
                    Else
                        dr("isMFAEmail") = "Y"
                    End If
                Else
                    dr("isMFAEmail") = "N"
                End If

                If chksms.Checked = True Then
                    If (Me.txtPhNo.Text.Trim() = "") Then
                        objCommon.ShowAlert("Please Enter Phone No.", Me.Page)
                        Exit Sub
                    Else
                        dr("isMFASms") = "Y"
                    End If
                Else
                    dr("isMFASms") = "N"
                End If

                If chkmfa.Checked = True And chkEmail.Checked <> True And chksms.Checked <> True Then
                    objCommon.ShowAlert("Please select at leaset one chekbox", Me.Page)
                    Exit Sub
                End If

                'if Condtion added by Bhargav Thaker 28Feb2023
                If dr("UUserID").ToString() Is Nothing OrElse dr("UUserID").ToString() = "" Then
                    dr("UUserID") = Guid.NewGuid()
                End If
                dr.AcceptChanges()
            Next
            dt_EditUser.AcceptChanges()
            Me.ViewState(VS_EditUserName) = dt_EditUser

        ElseIf type.ToUpper = "INACTIVE" Then
            dt_ActiveUser = CType(Me.ViewState(VS_ActiveUserMst), DataTable)
            For Each dr In dt_ActiveUser.Rows
                dr("cStatusIndi") = "D"
                dr("iModifyBy") = Me.Session(S_UserID)
                dr.AcceptChanges()
            Next dr
            Me.ViewState(VS_ActiveUserMst) = dt_ActiveUser
        ElseIf type.ToUpper = "ACTIVE" Then
            dt_ActiveUser = CType(Me.ViewState(VS_ActiveUserMst), DataTable)
            For Each dr In dt_ActiveUser.Rows
                dr("cStatusIndi") = "N"
                dr("iModifyBy") = Me.Session(S_UserID)

                dr.AcceptChanges()
            Next dr
            Me.ViewState(VS_ActiveUserMst) = dt_ActiveUser
        End If
    End Sub
#End Region

#Region "Edit User Dropdown Events"

    Protected Sub ddlEdit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUserEdit.SelectedIndexChanged
        Dim Wstr_UserNm As String = String.Empty

        Try
            Wstr_UserNm = "vUserName = '" + Me.ddlUserEdit.SelectedItem.Value.Trim() + "'"
            If Not Me.fillEditUserName(Wstr_UserNm) Then
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
        End Try

    End Sub

#End Region

#Region "FillEditUserName"
    Private Function fillEditUserName(ByVal username As String) As Boolean
        Dim ds_FillEditUser As New DataSet
        Dim dr As DataRow
        fillEditUserName = False

        Try

            If Not Me.objHelp.getuserMst(username, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_FillEditUser, eStr_retu) Then
                Throw New Exception(eStr)
                Exit Function
            End If

            If ds_FillEditUser.Tables(0).Rows.Count > 0 Then
                Me.ViewState(VS_EditUserName) = ds_FillEditUser.Tables(0)

                If ds_FillEditUser.Tables(0).Rows.Count > 0 Then
                    Dim isMFA As Boolean
                    Dim isMFAEmail As Boolean
                    Dim isMFASms As Boolean

                    dr = ds_FillEditUser.Tables(0).Rows(0)
                    Me.DDLScopeMst.SelectedIndex = Me.DDLScopeMst.Items.IndexOf(Me.DDLScopeMst.Items.FindByValue(Convert.ToString(dr("nScopeNo"))))
                    Me.TxtFirstName.Text = dr("vFirstName")
                    Me.TxtLastName.Text = dr("vLastName")
                    Me.DDLLocation.SelectedIndex = Me.DDLLocation.Items.IndexOf(Me.DDLLocation.Items.FindByValue(Convert.ToString(dr("vLocationCode"))))
                    Me.DDLDept.SelectedValue = dr("vDeptCode")
                    Me.txtEmail.Text = dr("vEmailId")
                    Me.txtPhNo.Text = dr("vPhoneNo")
                    Me.txtExtNo.Text = dr("vExtNo")
                    Me.txtRemark.Text = dr("vRemark")
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
                    If dr("isMFA").ToString() = "Y" Then
                        Me.trmfssend.Visible = True
                    Else
                        Me.trmfssend.Visible = False
                    End If

                    If dr("isMFA").ToString() = "Y" Then
                        isMFA = True
                        trmfssend.Visible = True
                    Else
                        isMFA = False
                    End If
                    If dr("isMFAEmail").ToString() = "Y" Then
                        isMFAEmail = True
                    Else
                        isMFAEmail = False
                    End If
                    If dr("isMFASms").ToString() = "Y" Then
                        isMFASms = True
                    Else
                        isMFASms = False
                    End If
                    Me.chkmfa.Checked = Convert.ToBoolean(Convert.ToBoolean(Me.chkmfa.Checked.CompareTo(Convert.ToBoolean(Me.chkmfa.Checked.CompareTo(isMFA)))))
                    Me.chkEmail.Checked = Convert.ToBoolean(Convert.ToBoolean(Me.chkEmail.Checked.CompareTo(Convert.ToBoolean(Me.chkEmail.Checked.CompareTo(isMFAEmail)))))
                    Me.chksms.Checked = Convert.ToBoolean(Convert.ToBoolean(Me.chksms.Checked.CompareTo(Convert.ToBoolean(Me.chksms.Checked.CompareTo(isMFASms)))))
                    Me.TblAdd.Visible = True
                    Me.btnedit.Visible = True
                    Me.btncancel.Visible = True

                    Me.ViewState(VS_UserName) = Me.ddlUserEdit.SelectedItem.Value.Trim()

                    Me.fillAddGrid()
                Else
                    Me.TblAdd.Visible = False
                    Me.btnedit.Visible = False
                End If
                fillEditUserName = True
            Else
                fillEditUserName = False
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            fillEditUserName = False
        End Try

    End Function

#End Region

#Region "Reset ViewState"

    Private Sub ResetViewstate()
        Me.ViewState(VS_ActFlag) = Nothing
        Me.ViewState(VS_ActiveUserMst) = Nothing
        Me.ViewState(VS_DtUserMst) = Nothing
        Me.ViewState(VS_DtUserType) = Nothing
        Me.ViewState(VS_EditUserName) = Nothing
        Me.ViewState(VS_UserName) = Nothing
        Me.ViewState(VS_LoginName) = Nothing
        Me.ViewState(VS_LoginPass) = Nothing
        Me.ViewState(Vs_SelectionMode) = Nothing
    End Sub

#End Region

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub

#End Region

#Region "Web Methods"
    <Web.Services.WebMethod()> _
    Public Shared Function AuditTrailForActiveInActiveUser(ByVal ProfileName As String, ByVal UserName As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim ds As DataSet
        Dim eStr As String = String.Empty
        Dim strReturn As String = String.Empty
        Try

            Dim profilenmae As String
            Dim username1 As String

            Dim array() As String
            array = ProfileName.Split("+")
            profilenmae = array(1).ToString()
            username1 = array(0).ToString()

            If Not objHelp.Proc_AuditTrailofActiveInactiveUser(profilenmae, username1, ds, eStr) Then
                Throw New Exception(eStr)
            End If


            Dim column As DataColumn
            column = New DataColumn()
            Dim drAuditTrail As DataRow
            column.DataType = System.Type.GetType("System.Int32")
            column.ColumnName = "Sr.No"
            ds.Tables(0).Columns.Add(column)
            ds.AcceptChanges()

            Dim dtTempAuditTrail As New DataTable

            If Not dtTempAuditTrail Is Nothing Then
                dtTempAuditTrail.Columns.Add("Sr.No.", GetType(String))
                dtTempAuditTrail.Columns.Add("vUserName", GetType(String))
                dtTempAuditTrail.Columns.Add("dModifyOn", GetType(String))
                dtTempAuditTrail.Columns.Add("vRemark", GetType(String))
                dtTempAuditTrail.Columns.Add("cBlockedFlag", GetType(String))
                dtTempAuditTrail.Columns.Add("vUserTypeCode", GetType(String))
                dtTempAuditTrail.Columns.Add("ModifyBy", GetType(String))
                dtTempAuditTrail.Columns.Add("IST", GetType(String))
            End If

            dtTempAuditTrail.AcceptChanges()


            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                drAuditTrail = dtTempAuditTrail.NewRow()
                drAuditTrail("Sr.No.") = ""
                drAuditTrail("vUserName") = Convert.ToString(ds.Tables(0).Rows(i)("vUserName"))
                drAuditTrail("dModifyOn") = Convert.ToString(CDate(ds.Tables(0).Rows(i)("dModifyOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                drAuditTrail("vRemark") = Convert.ToString(ds.Tables(0).Rows(i)("vRemark").ToString())
                drAuditTrail("vUserTypecode") = Convert.ToString(ds.Tables(0).Rows(i)("vUserTypecode").ToString())
                drAuditTrail("cBlockedFlag") = Convert.ToString(ds.Tables(0).Rows(i)("cBlockedFlag"))
                drAuditTrail("ModifyBy") = Convert.ToString(ds.Tables(0).Rows(i)("ModifyBy"))
                drAuditTrail("IST") = Convert.ToString(ds.Tables(0).Rows(i)("IST"))
                dtTempAuditTrail.Rows.Add(drAuditTrail)
                dtTempAuditTrail.AcceptChanges()
            Next i


            strReturn = JsonConvert.SerializeObject(dtTempAuditTrail)

            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function

#End Region

    Protected Sub chkmfa_CheckedChanged(sender As Object, e As EventArgs) Handles chkmfa.CheckedChanged
        If chkmfa.Checked = True Then
            Me.trmfssend.Visible = True
            Me.chkEmail.Checked = False
            Me.chksms.Checked = False
        Else
            Me.trmfssend.Visible = False
            Me.chkEmail.Checked = False
        End If


    End Sub

End Class
