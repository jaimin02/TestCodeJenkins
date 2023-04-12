
Partial Class frmHousingActivities
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objcommon As New clsCommon
    Private objhelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()


    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtParentActivity As String = "DtParentActivity"
    Private Const VS_Dt_GVHousing As String = "Dt_GVHousing"
    Private Const VS_MedExWorkspaceId As String = "MedExWorkspaceId"
    Private Const Vs_DtWorkSpaceNodeDetail As String = "DtWorkSpaceNodeDetail"
    Private Const Normal As String = "00"
    Private Const Multidose As String = "01"

    Private Const GVC_Move As Integer = 0
    Private Const GVC_SRNo As Integer = 1
    Private Const GVC_DisplayName As Integer = 2
    Private Const GVC_iNodeIndex As Integer = 3
    Private Const GVC_iNodeId As Integer = 4
    Private Const GVC_IsPreDose As Integer = 5

#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Not IsPostBack Then
                GenCall()
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....Page_Load")
        End Try
    End Sub
#End Region

#Region "GenCall "
    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try


            Choice = Me.Request.QueryString("Mode")
            Me.ViewState(VS_Choice) = Choice   'To use it while saving
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_MedExWorkspaceId) = Me.Request.QueryString("Value").ToString
            End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Me.objcommon.ShowAlert("Error While Getting Data from WorkSpaceNodeDetail.", Me.Page)
                Exit Function
            End If

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If
            GenCall = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall")
        Finally
        End Try
    End Function
#End Region

#Region "GenCall_Data "
    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean

        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Val As String = String.Empty
        Dim ds_MedExWorkspace As New DataSet
        Dim DS_WorkSpaceNodeDetail As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try



            Val = Me.ViewState(VS_MedExWorkspaceId) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vWorkSpaceId='" + Val.ToString + "' and cCloneFlage='H'"
            End If

            If objhelp.GetViewWorkSpaceNodeDetail(wStr, DS_WorkSpaceNodeDetail, eStr_Retu) Then 'get Blank Table From MedExWorkspaceHdr
                Me.ViewState(Vs_DtWorkSpaceNodeDetail) = DS_WorkSpaceNodeDetail.Tables(0)
                Me.ViewState(VS_Dt_GVHousing) = DS_WorkSpaceNodeDetail.Tables(0)
            End If

            If DS_WorkSpaceNodeDetail Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            If DS_WorkSpaceNodeDetail.Tables(0).Rows.Count <= 0 And _
             Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found for Selected Operation")
            End If

            ds_DWR_Retu = DS_WorkSpaceNodeDetail
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_Data")

        Finally

        End Try
    End Function
#End Region

#Region "GenCall_showUI "
    Private Function GenCall_ShowUI() As Boolean
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim sender As New Object
        Dim e As EventArgs
        Try



            CType(Me.Master.FindControl("lblHeading"), Label).Text = "ECRF Protocol Definition"

            Page.Title = " :: ECRF Protocol Definition ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")

            Choice = Me.ViewState(VS_Choice)

            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtProject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
            End If


            'for filtering the Projects according to user
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
            '======

            If Not FillDropdown() Then
                Exit Function
            End If

            Me.ImgBtnUp.Visible = False
            Me.ImgBtnDown.Visible = False

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

            End If

            Me.txtRepeatation.Attributes.Add("onblur", "IsNumericAndNotZero(this);")

            'If Not IsNothing(Me.Request.QueryString("Saved")) AndAlso Me.Request.QueryString("Saved").ToUpper.Trim() = "Y" Then
            '    Me.objcommon.ShowAlert("HOUSING ACTIVITY SAVED SUCCESFULLY.", Me.Page)
            'End If

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...............GenCall_ShowUI")
        Finally
        End Try
    End Function
#End Region

#Region "Fill DropDown"
    Private Function FillDropdown() As Boolean
        Dim Wstr_Scope As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds As New Data.DataSet
        Dim ds_Group As New Data.DataSet
        Dim dv_Activity As New DataView
        Dim ds_MedExWorkspaceHdr As New Data.DataSet
        Try



            'To Get Where condition of ScopeVales( Project Type )
            If Not objcommon.GetScopeValueWithCondition(Wstr_Scope) Then
                Exit Function
            End If
            Wstr_Scope += " And cStatusIndi <> 'D'"
            If Not objhelp.GetviewActivityGroupMst(Wstr_Scope, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Group, estr) Then
                ShowErrorMessage("", estr)
                Return False
            End If

            dv_Activity = ds_Group.Tables(0).DefaultView
            dv_Activity.Sort = "vActivityGroupName"
            Me.ddlActivityGroup2.DataSource = dv_Activity.ToTable()
            Me.ddlActivityGroup2.DataTextField = "vActivityGroupName"
            Me.ddlActivityGroup2.DataValueField = "vActivityGroupId"
            Me.ddlActivityGroup2.DataBind()
            Me.ddlActivityGroup2.Items.Insert(0, New ListItem("select ActivityGroup", "1"))

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...FillDropdown")
            Return False
        End Try
    End Function
#End Region

#Region "Project Button"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim Wstr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim ds_WorkSpaceNodeDetail As New DataSet
        Dim dt_WorkSpaceNodeDetail As New DataTable

        Dim dt_MaxNodeId As New DataTable
        Dim dv_MaxNodeId As New DataView

        Dim dt_MaxNodeIndex As New DataTable
        Dim dv_MaxNodeIndex As New DataView

        Dim dt_WorkSpaceActivity As New DataTable
        Dim dt_WorkSpaceNodes As New DataTable
        Dim paramArry(2) As String
        Dim ActivityparamArry(1) As String
        Dim NodeparamArry(1) As String
        Dim dsEdit As New DataSet
        Dim ds_ParentWorkspace As New DataSet
        Dim ds_Editchecks As New DataSet
        Dim ds_CrfHdr As New DataSet
        Dim ds_CrfVersionDetail As DataSet = Nothing
        Dim VersionNo As Integer = 0
        Dim VersionDate As Date

        Try

            chkAll.Enabled = True
            ChkPeriod.Enabled = True
            ddlPeriod.Enabled = True
            ddlParentAct.Enabled = True
            ddlRefAct.Enabled = True
            Me.btnSchedule.Visible = False
            Me.ImgBtnUp.Visible = False
            Me.ImgBtnDown.Visible = False
            BtnAdd.Style.Add("display", "")
            BtnSave.Style.Add("display", "")
            GV_Housing.DataSource = Nothing
            GV_Housing.DataBind()
            Me.GV_Housing.Enabled = True

            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowTractivitygroup", "Showfields()", True)
            'If Not Me.DisableControl() Then
            '    Throw New Exception("Error In Disable Control")
            'End If
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Wstr = "vWorkSpaceId='" + Me.HProjectId.Value.ToString.Trim() + "' and cStatusIndi<>'D'"



            ''====== CRFVersion Control==================================

            Wstr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'"

            If Not objhelp.GetData("View_CRFVersionForDataEntryControl", "*", Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CrfVersionDetail, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            If ds_CrfVersionDetail.Tables(0).Rows.Count > 0 Then

                VersionNo = ds_CrfVersionDetail.Tables(0).Rows(0)("nVersionNo")
                VersionDate = ds_CrfVersionDetail.Tables(0).Rows(0)("dVersiondate").ToString
                Me.VersionNo.Text = VersionNo.ToString
                Me.VersionDate.Text = VersionDate.ToString("dd-MMM-yyyy")
                Me.VersionDtl.Style.Add("display", "")
                'Me.VersionDtl.Attributes.Add("style", "display:inline !important;")
                If ds_CrfVersionDetail.Tables(0).Rows(0)("cFreezeStatus").ToString.Trim.ToUpper = "F" Then
                    ImageLockUnlock.Attributes.Add("src", "images/Freeze.jpg")
                    objcommon.ShowAlert("Project Is In Freeze State, First UnFreeze It Then Proceed", Me.Page())
                    If Not Me.DisableControl() Then
                        Throw New Exception("Error In Disable Control")
                    End If
                Else
                    ImageLockUnlock.Attributes.Add("src", "images/UnFreeze.jpg")
                    If Not Me.EnableControl() Then
                        Throw New Exception("Error In Enable Control")
                    End If
                End If
            Else
                Me.VersionDtl.Attributes.Add("style", "display:none;")
            End If
            ''==========================================================

            'To check project is site/Chield or Project
            If Not Me.objhelp.getworkspacemst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_ParentWorkspace, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If
            If ds_ParentWorkspace.Tables(0).Rows.Count > 0 Then
                If ds_ParentWorkspace.Tables(0).Rows(0).Item("cWorkspaceType") = "C" Then
                    objcommon.ShowAlert("You Can not Use This Page For Site/Child Project. Kindly Use Project Structure Management page.", Me.Page())
                    'Me.HProjectId.Value = ""
                    'Me.txtProject.Text = ""
                    'Exit Sub
                    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideTractivitygroup", "Showfields('H')", True)
                    If Not Me.DisableControl() Then
                        Throw New Exception("Error In Disable Control")
                    End If
                End If
            End If
            ''''''''''''''''''''''''
            'To check is Data Entry Start from IPAdministrator Page ==== added By Megha
            Wstr = String.Empty
            Wstr = "vWorkSpaceId='" + Me.HProjectId.Value.ToString.Trim() + "' and cStatusIndi<>'D'"
            ds_ParentWorkspace = Nothing
            If Not Me.objhelp.GetData("DosingDetail", "*", Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_ParentWorkspace, eStr_Retu) Then
                Throw New Exception("Error While Checking For IPADministratorChange")
            End If
            If ds_ParentWorkspace.Tables(0).Rows.Count > 0 Then

                objcommon.ShowAlert("For This Page Dosing Of Subject Is started You Can Not Use This Page. Kindly Use Project Structure Management Page.", Me.Page())
                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideTractivitygroup1", "Showfields('H')", True)
                If Not Me.DisableControl() Then
                    Throw New Exception("Error In Disable Control")
                End If
            End If
            '''''''''''''''''''''''''

            'To check is Data Entry Start from PkSample Page ==== added By Megha
            Wstr = String.Empty
            Wstr = "vWorkSpaceId='" + Me.HProjectId.Value.ToString.Trim() + "' and cStatusIndi<>'D'"
            ds_ParentWorkspace = Nothing
            If Not Me.objhelp.GetData("pksampledetail", "*", Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_ParentWorkspace, eStr_Retu) Then
                Throw New Exception("Error While Checking For PkSample Pages")
            End If
            If ds_ParentWorkspace.Tables(0).Rows.Count > 0 Then
                objcommon.ShowAlert("You Can Not use This Page For Site/Child Project. Kindly Use Project Structure Management Page.", Me.Page())
                'Me.HProjectId.Value = ""
                'Me.txtProject.Text = ""
                'Exit Sub
                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideTractivitygroup2", "Showfields('H')", True)
                If Not Me.DisableControl() Then
                    Throw New Exception("Error In Disable Control")
                End If
            End If
            '''''''''''''''''''''''''


            ' To Check Editchecks are entered or not
            If Not objhelp.GetEditChecksHdr(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Editchecks, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            If ds_Editchecks.Tables(0).Rows.Count > 0 Then
                objcommon.ShowAlert("Edit Checks Has Been Entered For This Project. Kindly Use Project Structure Management Page.", Me.Page())
                'Me.HProjectId.Value = ""
                'Me.txtProject.Text = ""
                ''BtnAdd.Style.Add("display", "none")
                'BtnSave.Style.Add("display", "none")
                hidefields()
                'Added By Mrunal Parekh on 17-Jan-2012 for hide some fields
                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideTractivitygroup3", "Showfields('H')", True)
                If Not Me.DisableControl() Then
                    Throw New Exception("Error In Disable Control")
                End If
                ' Exit Sub

            End If
            '''''''''''''''''''''''''

            ' To Data Entry is started or not
            If Not objhelp.GetCRFHdr(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CrfHdr, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            If ds_CrfHdr.Tables(0).Rows.Count > 0 Then
                objcommon.ShowAlert("Data Entry has been started for this project. Kindly use Project Structure Management page.", Me.Page())
                'Me.HProjectId.Value = ""
                'Me.txtProject.Text = ""
                'BtnAdd.Style.Add("display", "none")
                'BtnSave.Style.Add("display", "none")
                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideTractivitygroup4", "Showfields('H')", True)
                If Not Me.DisableControl() Then
                    Throw New Exception("Error In Disable Control")
                End If
                ' Exit Sub
            End If

            '''''''''''''''''''''''''
            Wstr += " Order by iPeriod"
            If objhelp.GetViewWorkSpaceNodeDetail(Wstr, ds_WorkSpaceNodeDetail, eStr_Retu) Then
                Me.ViewState(VS_DtParentActivity) = ds_WorkSpaceNodeDetail.Tables(0)
            End If

            paramArry(0) = "iPeriod"
            paramArry(1) = "iNodeId"
            paramArry(2) = "iNodeIndex"

            '=== value 00 for Normal and 01 for Multidose
            If rbtSelection.SelectedValue = Normal Then

                ActivityparamArry(0) = "vActivityName"
                ActivityparamArry(1) = "vActivityId"

                NodeparamArry(0) = "vActivityName"
                NodeparamArry(1) = "vActivityId"

                dt_WorkSpaceNodeDetail = ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable("WorkSpaceNodeDetail", True, paramArry(0))
                dt_MaxNodeId = ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable("NodeId", True, paramArry(1))
                dt_MaxNodeIndex = ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable("NodeIndex", True, paramArry(2))
                dt_WorkSpaceActivity = ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable("Activity", True, ActivityparamArry)

                dt_WorkSpaceActivity.DefaultView.Sort = "vActivityName"

                Me.ddlParentAct.DataSource = dt_WorkSpaceActivity.DefaultView.ToTable()
                Me.ddlParentAct.DataTextField = "vActivityName"
                Me.ddlParentAct.DataValueField = "vActivityId"
                Me.ddlParentAct.DataBind()
                Me.ddlParentAct.Items.Insert(0, New ListItem("select Parent Activity", "0"))

                Me.ddlRefAct.DataSource = dt_WorkSpaceActivity.DefaultView.ToTable()
                Me.ddlRefAct.DataTextField = "vActivityName"
                Me.ddlRefAct.DataValueField = "vActivityId"
                Me.ddlRefAct.DataBind()
                Me.ddlRefAct.Items.Insert(0, New ListItem("select Reference Activity", "0"))

            Else

                ActivityparamArry(0) = "vNodeDisplayName"
                ActivityparamArry(1) = "iNodeId"

                NodeparamArry(0) = "vNodeDisplayName"
                NodeparamArry(1) = "iNodeId"

                dt_WorkSpaceNodeDetail = ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable("WorkSpaceNodeDetail", True, paramArry(0))
                dt_MaxNodeId = ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable("NodeId", True, paramArry(1))
                dt_MaxNodeIndex = ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable("NodeIndex", True, paramArry(2))
                dt_WorkSpaceActivity = ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable("Activity", True, ActivityparamArry)
                dt_WorkSpaceNodes = ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable("NodeName", True, NodeparamArry)

                dt_WorkSpaceActivity.DefaultView.Sort = "vNodeDisplayName"

                Me.ddlParentAct.DataSource = dt_WorkSpaceActivity.DefaultView.ToTable()
                Me.ddlParentAct.DataTextField = "vNodeDisplayName"
                Me.ddlParentAct.DataValueField = "iNodeId"
                Me.ddlParentAct.DataBind()
                Me.ddlParentAct.Items.Insert(0, New ListItem("select Parent Activity", "0"))

                Me.ddlRefAct.DataSource = dt_WorkSpaceNodes.DefaultView.ToTable()
                Me.ddlRefAct.DataTextField = "vNodeDisplayName"
                Me.ddlRefAct.DataValueField = "iNodeId"
                Me.ddlRefAct.DataBind()
                Me.ddlRefAct.Items.Insert(0, New ListItem("select Reference Activity", "0"))

            End If

            dv_MaxNodeId = dt_MaxNodeId.Copy().DefaultView()
            dv_MaxNodeId.Sort = "iNodeId"

            Me.HfMaxNodeId.Value = dv_MaxNodeId.ToTable.Rows(dv_MaxNodeId.ToTable.Rows.Count - 1).Item("iNodeId")

            dv_MaxNodeIndex = dt_MaxNodeIndex.Copy().DefaultView
            dv_MaxNodeIndex.Sort = "iNodeIndex"
            Me.HfMaxNodeIndex.Value = dv_MaxNodeIndex.ToTable.Rows(dv_MaxNodeIndex.ToTable.Rows.Count - 1).Item("iNodeIndex")

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....btnSetProject_Click")
        End Try
    End Sub

#End Region

#Region "SelectedIndexChanged"

    Protected Sub ddlActivityGroup2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlActivityGroup2.SelectedIndexChanged
        FillActivity()
    End Sub

    Protected Sub ddlParentAct_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dt_Period As New DataTable
        Dim Str_Rowfilter As String = ""

        Str_Rowfilter = "vActivityId='" & Me.ddlParentAct.SelectedValue.Trim() & "'"

        If rbtSelection.SelectedValue = Multidose Then
            Str_Rowfilter = "iNodeId = " & Me.ddlParentAct.SelectedValue.Trim()
        End If

        CType(Me.ViewState(VS_DtParentActivity), DataTable).DefaultView.RowFilter = Str_Rowfilter.ToString()
        dt_Period = CType(Me.ViewState(VS_DtParentActivity), DataTable).DefaultView.ToTable(True, "iPeriod")

        Me.ddlPeriod.Items.Clear()
        Me.HfPeriod.Value = dt_Period.Rows(dt_Period.Rows.Count - 1).Item("iPeriod")

        Me.ddlPeriod.DataSource = dt_Period
        Me.ddlPeriod.DataTextField = "iPeriod"
        Me.ddlPeriod.DataValueField = "iPeriod"
        Me.ddlPeriod.DataBind()

        Me.ddlPeriodSearch.DataSource = dt_Period
        Me.ddlPeriodSearch.DataTextField = "iPeriod"
        Me.ddlPeriodSearch.DataValueField = "iPeriod"
        Me.ddlPeriodSearch.DataBind()

        Me.chkAll.Enabled = True
        If rbtSelection.SelectedValue = Multidose Then
            Me.chkAll.Checked = False
            Me.chkAll.Enabled = False
            Me.ChkPeriod.Checked = True
        End If

    End Sub

    Protected Sub rbtSelection_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtSelection.SelectedIndexChanged
        resetpage()
        'Me.Response.Redirect("frmHousingActivities.aspx?mode=1", False)
    End Sub

#End Region

#Region "Fill Activity"

    Private Function FillActivity() As Boolean
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim ds As New Data.DataSet
        Dim ds_type As New Data.DataSet
        Dim dv_Activity As New DataView
        Try


            wstr = "vActivityGroupId='" & Me.ddlActivityGroup2.SelectedValue.Trim() & "' And cStatusIndi <> 'D'"

            If Not objhelp.getActivityMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_type, estr) Then
                ShowErrorMessage("", estr)
            End If

            dv_Activity = ds_type.Tables(0).DefaultView
            dv_Activity.Sort = "vActivityName"

            Me.ddlAct.DataSource = dv_Activity.ToTable()
            Me.ddlAct.DataValueField = "vActivityId"
            Me.ddlAct.DataTextField = "vActivityName"
            Me.ddlAct.DataBind()
            Me.ddlAct.Items.Insert(0, New ListItem("Please Select Activity", "1"))

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......FillActivity")
        End Try

    End Function

#End Region

#Region "Checked Changed"
    Protected Sub chkAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.ChkPeriod.Checked = True Then

            Me.chkAll.Checked = True
            Me.ChkPeriod.Checked = False
            Me.ddlPeriod.Enabled = False

        ElseIf Me.chkAll.Checked = True And Me.ChkPeriod.Checked = False Then

            Me.chkAll.Checked = True
            Me.ChkPeriod.Checked = False
            Me.ddlPeriod.Enabled = False

        Else

            Me.chkAll.Checked = False
            Me.ChkPeriod.Checked = True
            Me.ddlPeriod.Enabled = True

        End If


    End Sub

    Protected Sub ChkPeriod_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If rbtSelection.SelectedValue = Multidose Then
            Exit Sub
        End If

        If Me.chkAll.Checked = True Then ' And Me.ChkPeriod.Checked = False Then

            Me.chkAll.Checked = False
            Me.ChkPeriod.Checked = True
            Me.ddlPeriod.Enabled = True

        ElseIf Me.chkAll.Checked = False And Me.ChkPeriod.Checked = True Then

            Me.chkAll.Checked = False
            Me.ChkPeriod.Checked = True
            Me.ddlPeriod.Enabled = True

        Else

            Me.chkAll.Checked = True
            Me.ChkPeriod.Checked = False
            Me.ddlPeriod.Enabled = False

        End If

    End Sub

    Protected Sub chkAllSearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.ChkPeriodSearch.Checked = True Then

            Me.chkAllSearch.Checked = True
            Me.ChkPeriodSearch.Checked = False
            Me.ddlPeriodSearch.Enabled = False

        ElseIf Me.chkAllSearch.Checked = True And Me.ChkPeriodSearch.Checked = False Then

            Me.chkAllSearch.Checked = True
            Me.ChkPeriodSearch.Checked = False
            Me.ddlPeriodSearch.Enabled = False

        Else

            Me.chkAllSearch.Checked = False
            Me.ChkPeriodSearch.Checked = True
            Me.ddlPeriodSearch.Enabled = True

        End If

    End Sub

    Protected Sub ChkPeriodSearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If rbtSelection.SelectedValue = Multidose Then
            Exit Sub
        End If

        If Me.chkAllSearch.Checked = True Then ' And Me.ChkPeriod.Checked = False Then

            Me.chkAllSearch.Checked = False
            Me.ChkPeriodSearch.Checked = True
            Me.ddlPeriodSearch.Enabled = True

        ElseIf Me.chkAllSearch.Checked = False And Me.ChkPeriodSearch.Checked = True Then

            Me.chkAllSearch.Checked = False
            Me.ChkPeriod.Checked = True
            Me.ddlPeriod.Enabled = True

        Else

            Me.chkAllSearch.Checked = True
            Me.ChkPeriodSearch.Checked = False
            Me.ddlPeriodSearch.Enabled = False

        End If

    End Sub

#End Region

#Region "Add Button"

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click
        Dim irep As Integer
        Dim iperiod As Integer
        Dim DtWorkSpaceNodeDetail As New DataTable
        Dim DvWorkSpaceNodeDetail As New DataView
        Dim Dr As DataRow
        Dim Ds_MaxNodeNo As New DataSet

        Dim INodeId As Integer
        Dim INodeIndex As Integer
        Dim MaxPeriod As Integer

        Try



            INodeId = Val(Me.HfMaxNodeId.Value) + 1
            INodeIndex = Val(Me.HfMaxNodeIndex.Value) + 1
            MaxPeriod = Val(Me.HfPeriod.Value)

            DtWorkSpaceNodeDetail = CType(Me.ViewState(VS_Dt_GVHousing), DataTable)

            'vWorkSpaceId, iNodeIndex, iNodeId, vTemplateId, iNodeNo, vNodeName, vNodeDisplayName, cNodeTypeIndi, 
            'iParentNodeId, iperiod, nMilestone, vActivityId, vFolderName, cCloneFlag, cRequiredFlag, dCheckOutOn, 
            'iCheckOutBy, cPublishFlag, vDocTemplateId, iDocTemplateTranNo, vRemark, iModifyBy, dModifyOn, cStatusIndi, 
            'vDefaultFileFormat, vDocTemplatePath

            For irep = 1 To Val(Me.txtRepeatation.Text)

                Dr = DtWorkSpaceNodeDetail.NewRow
                Dr("vWorkSpaceId") = Me.HProjectId.Value.Trim()
                Dr("iNodeIndex") = INodeIndex
                Dr("iNodeId") = INodeId
                Dr("vNodeName") = Me.ddlAct.SelectedItem.Text.Trim()
                Dr("vNodeDisplayName") = Me.ddlAct.SelectedItem.Text.Trim() + "-" + irep.ToString.Trim()
                Dr("cNodeTypeIndi") = "N"
                Dr("iParentNodeId") = 0 'Me.ddlParentAct.SelectedValue.Trim()
                Dr("iperiod") = iperiod
                Dr("vActivityId") = Me.ddlAct.SelectedValue.Trim()
                If Me.txtRefTimeInterval.Text.Trim = "" Then
                    Dr("nRefTime") = Nothing
                Else
                    Dr("nRefTime") = Format(Val(Me.txtRefTimeInterval.Text.Trim()), "00.00")
                End If


                If irep > 1 Then
                    If Me.txtRefTimeInterval.Text.Trim = "" Then
                        Dr("nRefTime") = Nothing
                    Else
                        Dr("nRefTime") = Format(Val(TimeInterval(DtWorkSpaceNodeDetail.Rows(DtWorkSpaceNodeDetail.Rows.Count - 1).Item("nRefTime") + _
                                                Val(Me.txtRefTimeInterval.Text.Trim()))), "00.00")
                    End If
                End If
                If Me.txtDevTime.Text.Trim = "" Then
                    Dr("nDeviationTime") = Nothing
                Else
                    Dr("nDeviationTime") = Format(Val(Me.txtDevTime.Text.Trim()), "00.00")
                End If


                'If irep > 1 Then

                '    Dr("nDeviationTime") = Format(Val(TimeInterval(DtWorkSpaceNodeDetail.Rows(DtWorkSpaceNodeDetail.Rows.Count - 1).Item("nDeviationTime") + _
                '                            Val(Me.txtRefTimeInterval.Text.Trim()))), "00.00")
                'End If

                Dr("iRefNodeId") = 0
                Dr("cCloneFlag") = "H"
                Dr("cRequiredFlag") = "Y"
                Dr("cPublishFlag") = "N"
                Dr("iModifyBy") = Me.Session(S_UserID)

                DtWorkSpaceNodeDetail.Rows.Add(Dr)
                DtWorkSpaceNodeDetail.AcceptChanges()
                INodeId += 1

            Next irep

            'Me.HfMaxNodeId.Value = DtWorkSpaceNodeDetail.Rows(DtWorkSpaceNodeDetail.Rows.Count - 1).Item("iNodeId")
            'Me.HfMaxNodeIndex.Value = DtWorkSpaceNodeDetail.Rows(DtWorkSpaceNodeDetail.Rows.Count - 1).Item("iNodeIndex")

            DvWorkSpaceNodeDetail = DtWorkSpaceNodeDetail.DefaultView
            DvWorkSpaceNodeDetail.Sort = "iNodeId" '"iNodeIndex"
            Me.ViewState(VS_Dt_GVHousing) = DvWorkSpaceNodeDetail.ToTable()
            Me.GV_Housing.DataSource = DvWorkSpaceNodeDetail.ToTable()
            Me.GV_Housing.DataBind()


            Me.HfMaxNodeId.Value = DvWorkSpaceNodeDetail.ToTable.Rows(DvWorkSpaceNodeDetail.ToTable.Rows.Count - 1).Item("iNodeId")

            DvWorkSpaceNodeDetail = Nothing
            DvWorkSpaceNodeDetail = New DataView
            DvWorkSpaceNodeDetail = DtWorkSpaceNodeDetail.Copy().DefaultView
            DvWorkSpaceNodeDetail.Sort = "iNodeIndex"

            Me.HfMaxNodeIndex.Value = DvWorkSpaceNodeDetail.ToTable.Rows(DvWorkSpaceNodeDetail.ToTable.Rows.Count - 1).Item("iNodeIndex")



            If GV_Housing.Rows.Count > 1 Then

                Me.btnSchedule.Visible = True
                Me.ImgBtnUp.Visible = True
                Me.ImgBtnDown.Visible = True
                Me.ddlParentAct.Enabled = False
                Me.ddlRefAct.Enabled = False

            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....BtnAdd_Click")
        End Try
    End Sub

#End Region

#Region "Move Up - Down"

    Protected Sub ImgBtnUp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgBtnUp.Click
        MoveUp_Down("UP")
    End Sub

    Protected Sub ImgBtnDown_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgBtnDown.Click
        MoveUp_Down("DOWN")
    End Sub

    Private Sub MoveUp_Down(ByVal Move As String)
        Dim index As Integer
        Dim CntCheck As Integer
        Dim NodeIndex As Integer
        Dim NodeId As Integer
        Dim Dt_GVHousing As New DataTable
        Dim Dv_GVHousing As New DataView
        Dim iDr As Integer
        Try
           
            CntCheck = 0
            For index = 0 To Me.GV_Housing.Rows.Count - 1
                If CType(Me.GV_Housing.Rows(index).FindControl("ChkMove"), CheckBox).Checked = True Then
                    CntCheck += 1
                    If CntCheck > 1 Then
                        Me.objcommon.ShowAlert("Please select only one", Me.Page)
                        Exit Sub
                    End If

                    NodeIndex = Me.GV_Housing.Rows(index).Cells(GVC_iNodeIndex).Text
                    NodeId = Me.GV_Housing.Rows(index).Cells(GVC_iNodeId).Text

                End If
            Next
            If CntCheck = 0 Then
                Me.objcommon.ShowAlert("Please select Atleast one", Me.Page)
                Exit Sub
            End If
            Dt_GVHousing = CType(Me.ViewState(VS_Dt_GVHousing), DataTable)

            For iDr = 0 To Dt_GVHousing.Rows.Count - 1
                If Dt_GVHousing.Rows(iDr).Item("vNodeName") <> CType(Me.GV_Housing.Rows(iDr).FindControl("txtDesc"), TextBox).Text Then
                    Dt_GVHousing.Rows(iDr).Item("vNodeName") = CType(Me.GV_Housing.Rows(iDr).FindControl("txtDesc"), TextBox).Text
                    Dt_GVHousing.Rows(iDr).Item("vNodeDisplayName") = CType(Me.GV_Housing.Rows(iDr).FindControl("txtDesc"), TextBox).Text
                    If CType(Me.GV_Housing.Rows(iDr).FindControl("chkIsPredose"), CheckBox).Checked = True Then
                        Dt_GVHousing.Rows(iDr).Item("cIsPreDose") = "Y"
                    End If
                    Dt_GVHousing.AcceptChanges()
                End If
            Next

            For iDr = 0 To Dt_GVHousing.Rows.Count - 1
                If Dt_GVHousing.Rows(iDr).Item("iNodeIndex") = NodeIndex And _
                                    Dt_GVHousing.Rows(iDr).Item("iNodeId") = NodeId Then
                    If Move.ToUpper = "UP" Then

                        Dt_GVHousing.Rows(iDr).Item("iNodeId") = Dt_GVHousing.Rows(iDr - 1).Item("iNodeId")
                        Dt_GVHousing.Rows(iDr - 1).Item("iNodeId") = NodeId

                    End If

                    If Move.ToUpper = "DOWN" Then

                        Dt_GVHousing.Rows(iDr).Item("iNodeId") = Dt_GVHousing.Rows(iDr + 1).Item("iNodeId")
                        Dt_GVHousing.Rows(iDr + 1).Item("iNodeId") = NodeId

                    End If
                    Dt_GVHousing.AcceptChanges()

                    Me.HFGV_NodeId.Value = Dt_GVHousing.Rows(iDr).Item("iNodeId")

                    Exit For
                End If
            Next
            Dv_GVHousing = Dt_GVHousing.DefaultView
            Dv_GVHousing.Sort = "iNodeId" '"iNodeIndex"
            Me.ViewState(VS_Dt_GVHousing) = Dv_GVHousing.ToTable()
            Me.GV_Housing.DataSource = Dv_GVHousing.ToTable()
            Me.GV_Housing.DataBind()

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "........MoveUp_Down")
        End Try
    End Sub

#End Region

#Region "Grid Event"

    Protected Sub GV_Housing_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        e.Row.Cells(GVC_iNodeId).Visible = False
        e.Row.Cells(GVC_iNodeIndex).Visible = False
        e.Row.Cells(GVC_IsPreDose).Visible = False
    End Sub

    Protected Sub GV_Housing_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVC_SRNo).Text = e.Row.RowIndex + 1

            CType(e.Row.FindControl("lnkUpdateTime"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkUpdateTime"), LinkButton).CommandName = "UPDATE"

            If e.Row.Cells(GVC_iNodeId).Text = Me.HFGV_NodeId.Value Then
                CType(e.Row.FindControl("ChkMove"), CheckBox).Checked = True
            End If
            '****Added by Chandresh Vanker on 27-March-2009****************

            CType(e.Row.FindControl("imgbtnDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("imgbtnDelete"), ImageButton).CommandName = "DELETE"
            If e.Row.Cells(GVC_IsPreDose).Text = "Y" Then
                CType(e.Row.FindControl("chkIsPredose"), CheckBox).Checked = True
            End If

            '**************************************************************
        End If
    End Sub

    Protected Sub GV_Housing_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim index As Integer = CInt(e.CommandArgument)
        Dim DtWorkSpaceNodeDetail As New DataTable
        Dim DvWorkSpaceNodeDetail As New DataView
        Dim dr As DataRow
        Dim wStr As String = String.Empty
        Dim ds_Check As New DataSet
        Dim eStr As String = String.Empty

        If e.CommandName.ToUpper.Trim() = "UPDATE" Then
            DtWorkSpaceNodeDetail = CType(Me.ViewState(VS_Dt_GVHousing), DataTable)

            For Each dr In DtWorkSpaceNodeDetail.Rows

                If dr("iNodeId") = Me.GV_Housing.Rows(index).Cells(GVC_iNodeId).Text.Trim() Then
                    dr("nRefTime") = CType(Me.GV_Housing.Rows(index).FindControl("txtRefTime"), TextBox).Text.Trim()
                    dr("vNodeDisplayName") = CType(Me.GV_Housing.Rows(index).FindControl("txtDesc"), TextBox).Text.Trim()
                    dr("nDeviationTime") = CType(Me.GV_Housing.Rows(index).FindControl("txtDevTime"), TextBox).Text.Trim()
                    If CType(Me.GV_Housing.Rows(index).FindControl("chkIsPredose"), CheckBox).Checked = True Then
                        dr("cIsPreDose") = "Y"
                    Else
                        dr("cIsPreDose") = "N"
                    End If
                    Exit For
                End If

            Next

            DvWorkSpaceNodeDetail = DtWorkSpaceNodeDetail.DefaultView
            Me.ViewState(VS_Dt_GVHousing) = DvWorkSpaceNodeDetail.ToTable()
            Me.GV_Housing.DataSource = DvWorkSpaceNodeDetail.ToTable()
            Me.GV_Housing.DataBind()

        End If

        '****Added by Chandresh Vanker on 27-March-2009****************

        If e.CommandName.ToUpper.Trim() = "DELETE" Then

            DtWorkSpaceNodeDetail = CType(Me.ViewState(VS_Dt_GVHousing), DataTable)
            DtWorkSpaceNodeDetail.Rows(index).Delete()
            DtWorkSpaceNodeDetail.AcceptChanges()
            DvWorkSpaceNodeDetail = DtWorkSpaceNodeDetail.DefaultView
            Me.ViewState(VS_Dt_GVHousing) = DvWorkSpaceNodeDetail.ToTable()

            Me.GV_Housing.DataSource = DvWorkSpaceNodeDetail.ToTable()
            Me.GV_Housing.DataBind()
        End If

        '**************************************************************
    End Sub

    Protected Sub GV_Housing_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs)

    End Sub

    Protected Sub GV_Housing_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)

    End Sub


#End Region

#Region "Save Button & AssingValues"

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim estr As String = String.Empty
        Dim Dt_Save As New DataTable
        Dim Ds_Save As New DataSet

        Try

            If Not AssignValues() Then
                Me.objcommon.ShowAlert("Error While Assigning Values.", Me.Page)
                Exit Sub
            End If

            Dt_Save = Me.ViewState(Vs_DtWorkSpaceNodeDetail)
            Ds_Save.Tables.Add(Dt_Save.Copy)

            'Only For Edit Mode --- Firt Delete and then again Add ---- As per discussion

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                objLambda.Timeout = -1
                If Not Me.objLambda.Edit_HousingDetail(Me.ViewState(VS_Choice), Ds_Save, Me.Session(S_UserID), estr) Then
                    Me.objcommon.ShowAlert("ERROR WHILE SAVING VALUES IN WORKSPACENODEDETAIL ... " + estr.ToString(), Me.Page)
                    Exit Sub
                End If

                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowAlert", "alert('Housing Activity Saved Successfully.');document.location.href = 'frmHousingActivities.aspx?mode=1&Saved=""Y""';", True)

                'Me.Response.Redirect("frmHousingActivities.aspx?mode=1&Saved=Y", False)
                Exit Sub


            End If

            '***************************

            If Ds_Save.Tables(0).Columns.Contains("vActivityName") Then
                Ds_Save.Tables(0).Columns.Remove("vActivityName")
            End If

            If Ds_Save.Tables(0).Columns.Contains("vParentActivityId") Then
                Ds_Save.Tables(0).Columns.Remove("vParentActivityId")
            End If

            If Ds_Save.Tables(0).Columns.Contains("vRefActivityId") Then
                Ds_Save.Tables(0).Columns.Remove("vRefActivityId")
            End If

            If Ds_Save.Tables(0).Columns.Contains("vWorkSpaceDesc") Then
                Ds_Save.Tables(0).Columns.Remove("vWorkSpaceDesc")
            End If

            If Ds_Save.Tables(0).Columns.Contains("vDocTypeCode") Then
                Ds_Save.Tables(0).Columns.Remove("vDocTypeCode")
            End If

            If Ds_Save.Tables(0).Columns.Contains("vLocationCode") Then
                Ds_Save.Tables(0).Columns.Remove("vLocationCode")
            End If

            If Ds_Save.Tables(0).Columns.Contains("vLocationName") Then
                Ds_Save.Tables(0).Columns.Remove("vLocationName")
            End If

            If Ds_Save.Tables(0).Columns.Contains("vRefActivityEndtime") Then
                Ds_Save.Tables(0).Columns.Remove("vRefActivityEndtime")
            End If

            If Ds_Save.Tables(0).Columns.Contains("NewRefTime") Then
                Ds_Save.Tables(0).Columns.Remove("NewRefTime")
            End If

            'Added on 14-Sep-2009 by chandresh vanker for removing extra column
            If Ds_Save.Tables(0).Columns.Contains("cSubjectWiseFlag") Then
                Ds_Save.Tables(0).Columns.Remove("cSubjectWiseFlag")
            End If
            '******************************
            '======added on 21-11-09==by Deepak Singh to remove DispalyName as it is not in Table(but in view)===
            If Ds_Save.Tables(0).Columns.Contains("DisplayNode") Then
                Ds_Save.Tables(0).Columns.Remove("DisplayNode")
            End If
            '===========================================================================

            If Ds_Save.Tables(0).Columns.Contains("cIsRepeatable") Then
                Ds_Save.Tables(0).Columns.Remove("cIsRepeatable")
            End If

            If Ds_Save.Tables(0).Columns.Contains("vProjectNo") Then
                Ds_Save.Tables(0).Columns.Remove("vProjectNo")
            End If

            If Ds_Save.Tables(0).Columns.Contains("ActivityDisplayName") Then
                Ds_Save.Tables(0).Columns.Remove("ActivityDisplayName")
            End If

            If Ds_Save.Tables(0).Columns.Contains("ActivityDisplayId") Then
                Ds_Save.Tables(0).Columns.Remove("ActivityDisplayId")
            End If

            If Ds_Save.Tables(0).Columns.Contains("ParentActivityName") Then
                Ds_Save.Tables(0).Columns.Remove("ParentActivityName")
            End If

            If Ds_Save.Tables(0).Columns.Contains("ActivityWithParent") Then
                Ds_Save.Tables(0).Columns.Remove("ActivityWithParent")
            End If



            Ds_Save.Tables(0).TableName = "WorkSpaceNodeDetail"
            Ds_Save.Tables(0).AcceptChanges()

            If Not Me.objLambda.Save_WorkSpaceNodeDetail(Me.ViewState(VS_Choice), Ds_Save, Me.Session(S_UserID), estr) Then
                Me.objcommon.ShowAlert("ERROR WHILE SAVING VALUES IN WORKSPACENODEDETAIL ... " + estr.ToString(), Me.Page)
                Exit Sub
            End If

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowAlert", "alert('HOUSING ACTIVITY SAVED SUCCESFULLY.');document.location.href = 'frmHousingActivities.aspx?mode=1&Saved=Y';", True)
            'Me.Response.Redirect("frmHousingActivities.aspx?mode=1&Saved=Y", False)

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".......BtnSave_Click")
        End Try
    End Sub

    Private Function AssignValues() As Boolean
        Dim iRows As Integer
        Dim iperiod As Integer
        Dim DtWorkSpaceNodeDetail As New DataTable
        Dim Dt_GV As New DataTable
        Dim Ds_MaxNodeNo As New DataSet
        Dim Ds_ParentDtl As New DataSet
        Dim Dt_ParentDtl As New DataTable
        Dim Dr As DataRow
        Dim INodeId As Integer
        Dim INodeIndex As Integer
        Dim MaxPeriod As Integer
        Dim MaxNodeNo As Integer
        Dim IParentNodeId As Integer
        Dim ParentActivity As String = String.Empty
        Dim IRefNodeId As Integer
        Dim RefActivityId As String = String.Empty
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim Wstr_Ref As String = String.Empty
        Dim Dt_RefDtl As New DataTable
        Dim Dv_ParentDtl As New DataView
        Dim str_ParRowfilter As String = String.Empty
        Dim str_RefRowfilter As String = String.Empty

        Try
            DtWorkSpaceNodeDetail = CType(Me.ViewState(Vs_DtWorkSpaceNodeDetail), DataTable)
            Dt_GV = CType(Me.ViewState(VS_Dt_GVHousing), DataTable)

            INodeId = Val(Dt_GV.Rows(0)("iNodeId"))
            MaxPeriod = Val(Me.HfPeriod.Value)
            INodeIndex = Val(Me.HfMaxNodeIndex.Value)

            'vWorkSpaceId, iNodeIndex, iNodeId, vTemplateId, iNodeNo, vNodeName, vNodeDisplayName, cNodeTypeIndi, 
            'iParentNodeId, iperiod, nMilestone, vActivityId, vFolderName, cCloneFlag, cRequiredFlag, dCheckOutOn, 
            'iCheckOutBy, cPublishFlag, vDocTemplateId, iDocTemplateTranNo, vRemark, iModifyBy, dModifyOn, cStatusIndi, 
            'vDefaultFileFormat, vDocTemplatePath

            DtWorkSpaceNodeDetail.Clear()

            Dt_ParentDtl = CType(Me.ViewState(VS_DtParentActivity), DataTable)
            Dv_ParentDtl = Dt_ParentDtl.DefaultView
            Dv_ParentDtl.RowFilter = "iPeriod <>0"
            Dt_ParentDtl = Dv_ParentDtl.ToTable(True, "iPeriod")

            If Me.ChkPeriod.Checked = True Then

                iperiod = Me.ddlPeriod.SelectedValue.Trim()

                'For Finding Parent NodeId
                Dt_RefDtl = Nothing
                Dt_RefDtl = New DataTable

                Dt_RefDtl = CType(Me.ViewState(VS_DtParentActivity), DataTable)
                str_ParRowfilter = "vActivityId='" & Me.ddlParentAct.SelectedValue.Trim() & "' And " & _
                                   " iPeriod=" & iperiod
                If rbtSelection.SelectedValue = Multidose Then
                    str_ParRowfilter = "iNodeId = " & Me.ddlParentAct.SelectedValue.Trim()
                End If
                Dt_RefDtl.DefaultView.RowFilter = str_ParRowfilter.ToString()

                ParentActivity = Me.ddlParentAct.SelectedValue.Trim()
                If rbtSelection.SelectedValue = Multidose Then
                    ParentActivity = Dt_RefDtl.DefaultView.ToTable().Rows(0).Item("vActivityId")
                End If

                IParentNodeId = Dt_RefDtl.DefaultView.ToTable().Rows(0).Item("iNodeId")

                '**************** Parent NodeId End 

                'For Finding Reference NodeId
                Dt_RefDtl = Nothing
                Dt_RefDtl = New DataTable

                Dt_RefDtl = CType(Me.ViewState(VS_DtParentActivity), DataTable)
                str_RefRowfilter = "vActivityId='" & Me.ddlRefAct.SelectedValue.Trim() & "' And " & _
                                    " iPeriod=" & iperiod
                If rbtSelection.SelectedValue = Multidose Then
                    str_RefRowfilter = "iNodeId =" & Me.ddlRefAct.SelectedValue.Trim() & " And " & _
                                                        " iPeriod=" & iperiod
                End If

                Dt_RefDtl.DefaultView.RowFilter = str_RefRowfilter.ToString()

                If Dt_RefDtl.DefaultView.ToTable().Rows.Count < 1 Then
                    'reference activity is not period specific
                    Exit Function
                End If

                IRefNodeId = Dt_RefDtl.DefaultView.ToTable().Rows(0).Item("iNodeId")
                RefActivityId = Me.ddlRefAct.SelectedValue.Trim()
                If rbtSelection.SelectedValue = Multidose Then
                    IRefNodeId = Me.ddlRefAct.SelectedValue.Trim()
                    RefActivityId = Dt_RefDtl.DefaultView.ToTable().Rows(0).Item("vActivityId")
                End If


                '**************** Reference NodeId End 

                'For Finding Maximum NodeNo
                Dt_RefDtl = Nothing
                Dt_RefDtl = New DataTable

                Dt_RefDtl = CType(Me.ViewState(VS_DtParentActivity), DataTable)

                Dt_RefDtl.DefaultView.RowFilter = "iParentNodeId=" & IParentNodeId
                Dt_RefDtl.DefaultView.Sort = "iNodeNo"
                '**************** Reference NodeId End 

                MaxNodeNo = 1

                If Dt_RefDtl.DefaultView.ToTable().Rows.Count > 0 Then

                    MaxNodeNo = Dt_RefDtl.DefaultView.ToTable().Rows(Dt_RefDtl.DefaultView.ToTable().Rows.Count - 1).Item("iNodeNo") + 1

                End If

                For iRows = 0 To Dt_GV.Rows.Count - 1
                    Dr = DtWorkSpaceNodeDetail.NewRow
                    Dr("vWorkSpaceId") = Me.HProjectId.Value.Trim()
                    Dr("iNodeIndex") = Dt_GV.Rows(iRows)("iNodeIndex")
                    Dr("iNodeId") = INodeId 'Dt_GV.Rows(iRows)("iNodeId")
                    Dr("iNodeNo") = MaxNodeNo
                    Dr("vNodeName") = Dt_GV.Rows(iRows)("vNodeName")
                    Dr("vNodeDisplayName") = CType(Me.GV_Housing.Rows(iRows).FindControl("txtDesc"), TextBox).Text
                    Dr("cNodeTypeIndi") = "N"
                    Dr("iParentNodeId") = IParentNodeId 'Dt_GV.Rows(iRows)("iParentNodeId")
                    Dr("iperiod") = iperiod
                    Dr("vActivityId") = Dt_GV.Rows(iRows)("vActivityId")
                    Dr("nRefTime") = TimeInterval(CType(Me.GV_Housing.Rows(iRows).FindControl("txtRefTime"), TextBox).Text.Trim())
                    Dr("nDeviationTime") = TimeInterval(CType(Me.GV_Housing.Rows(iRows).FindControl("txtDevTime"), TextBox).Text.Trim())
                    Dr("iRefNodeId") = IRefNodeId
                    Dr("vParentActivityId") = ParentActivity
                    Dr("vRefActivityId") = RefActivityId
                    Dr("cCloneFlag") = "H"
                    Dr("cRequiredFlag") = "Y"
                    Dr("cPublishFlag") = "N"
                    Dr("iModifyBy") = Me.Session(S_UserID)
                    Dr("cStatusIndi") = "N"
                    If CType(Me.GV_Housing.Rows(iRows).FindControl("chkIsPredose"), CheckBox).Checked = True Then
                        Dr("cIsPredose") = "Y"
                    Else
                        Dr("cIsPredose") = "N"
                    End If
                    DtWorkSpaceNodeDetail.Rows.Add(Dr)
                    DtWorkSpaceNodeDetail.AcceptChanges()
                    INodeId += 1
                    MaxNodeNo += 1

                Next iRows

            ElseIf Me.chkAll.Checked = True Then

                For iperiod = 0 To Dt_ParentDtl.Rows.Count - 1 'Val(MaxPeriod)

                    'For Finding Parent NodeId
                    Dt_RefDtl = Nothing
                    Dt_RefDtl = New DataTable

                    Dt_RefDtl = CType(Me.ViewState(VS_DtParentActivity), DataTable)
                    str_ParRowfilter = "vActivityId='" & Me.ddlParentAct.SelectedValue.Trim() & "' And " & _
                                    " iPeriod=" & Dt_ParentDtl.Rows(iperiod).Item("iPeriod")
                    If rbtSelection.SelectedValue = Multidose Then
                        str_ParRowfilter = "iNodeId= " & Me.ddlParentAct.SelectedValue.Trim()
                    End If
                    Dt_RefDtl.DefaultView.RowFilter = str_ParRowfilter.ToString()

                    ParentActivity = Me.ddlParentAct.SelectedValue.Trim()
                    If rbtSelection.SelectedValue = Multidose Then
                        ParentActivity = Dt_RefDtl.DefaultView.ToTable().Rows(0).Item("vActivityId")
                    End If

                    IParentNodeId = Dt_RefDtl.DefaultView.ToTable().Rows(0).Item("iNodeId")

                    '**************** Parent NodeId End 

                    'For Finding Reference NodeId
                    Dt_RefDtl = Nothing
                    Dt_RefDtl = New DataTable

                    Dt_RefDtl = CType(Me.ViewState(VS_DtParentActivity), DataTable)

                    str_RefRowfilter = "vActivityId='" & Me.ddlRefAct.SelectedValue.Trim() & "' And " & _
                                    " iPeriod=" & Dt_ParentDtl.Rows(iperiod).Item("iPeriod")
                    If rbtSelection.SelectedValue = Multidose Then '
                        str_RefRowfilter = "iNodeID ='" & Me.ddlRefAct.SelectedValue.Trim() & "' And " & _
                                                 " iPeriod=" & Dt_ParentDtl.Rows(iperiod).Item("iPeriod")
                    End If

                    Dt_RefDtl.DefaultView.RowFilter = str_RefRowfilter

                    If Dt_RefDtl.DefaultView.ToTable().Rows.Count < 1 Then
                        Continue For
                    End If

                    IRefNodeId = Dt_RefDtl.DefaultView.ToTable().Rows(0).Item("iNodeId")
                    RefActivityId = Me.ddlRefAct.SelectedValue.Trim()
                    If rbtSelection.SelectedValue = Multidose Then
                        IRefNodeId = Me.ddlRefAct.SelectedValue.Trim()
                        RefActivityId = Dt_RefDtl.DefaultView.ToTable().Rows(0).Item("vActivityId")
                    End If

                    '**************** Reference NodeId End 


                    'For Finding Maximum NodeNo
                    Dt_RefDtl = Nothing
                    Dt_RefDtl = New DataTable

                    Dt_RefDtl = CType(Me.ViewState(VS_DtParentActivity), DataTable)

                    Dt_RefDtl.DefaultView.RowFilter = "iParentNodeId=" & IParentNodeId
                    Dt_RefDtl.DefaultView.Sort = "iNodeNo"
                    '**************** Reference NodeId End 

                    MaxNodeNo = 1

                    If Dt_RefDtl.DefaultView.ToTable().Rows.Count > 0 Then

                        MaxNodeNo = Dt_RefDtl.DefaultView.ToTable().Rows(Dt_RefDtl.DefaultView.ToTable().Rows.Count - 1).Item("iNodeNo") + 1

                    End If

                    For iRows = 0 To Dt_GV.Rows.Count - 1

                        Dr = DtWorkSpaceNodeDetail.NewRow
                        Dr("vWorkSpaceId") = Me.HProjectId.Value.Trim()
                        Dr("iNodeIndex") = Dt_GV.Rows(iRows)("iNodeIndex")
                        Dr("iNodeId") = INodeId 'Dt_GV.Rows(iRows)("iNodeId")
                        Dr("iNodeNo") = MaxNodeNo
                        Dr("vNodeName") = Dt_GV.Rows(iRows)("vNodeName")
                        Dr("vNodeDisplayName") = CType(Me.GV_Housing.Rows(iRows).FindControl("txtDesc"), TextBox).Text
                        Dr("cNodeTypeIndi") = "N"
                        Dr("iParentNodeId") = IParentNodeId 'Dt_GV.Rows(iRows)("iParentNodeId")
                        Dr("iperiod") = Dt_ParentDtl.Rows(iperiod).Item("iperiod")
                        Dr("vActivityId") = Dt_GV.Rows(iRows)("vActivityId")
                        Dr("nRefTime") = TimeInterval(CType(Me.GV_Housing.Rows(iRows).FindControl("txtRefTime"), TextBox).Text.Trim())
                        Dr("nDeviationTime") = TimeInterval(CType(Me.GV_Housing.Rows(iRows).FindControl("txtDevTime"), TextBox).Text.Trim())
                        Dr("iRefNodeId") = IRefNodeId
                        Dr("vParentActivityId") = ParentActivity
                        Dr("vRefActivityId") = RefActivityId
                        Dr("cCloneFlag") = "H"
                        Dr("cRequiredFlag") = "Y"
                        Dr("cPublishFlag") = "N"
                        Dr("iModifyBy") = Me.Session(S_UserID)
                        Dr("cStatusIndi") = "N"
                        If CType(Me.GV_Housing.Rows(iRows).FindControl("chkIsPredose"), CheckBox).Checked = True Then
                            Dr("cIsPredose") = "Y"
                        Else
                            Dr("cIsPredose") = "N"
                        End If
                        DtWorkSpaceNodeDetail.Rows.Add(Dr)
                        DtWorkSpaceNodeDetail.AcceptChanges()
                        INodeId += 1
                        MaxNodeNo += 1

                    Next iRows

                Next iperiod

            End If

            Me.ViewState(Vs_DtWorkSpaceNodeDetail) = DtWorkSpaceNodeDetail
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....AssignValues")
            Return False
        End Try
    End Function
    'Added By Mrunal To hide fields
    Private Function hidefields() As Boolean
        chkAll.Enabled = False
        ChkPeriod.Enabled = False
        ddlPeriod.Enabled = False
        btnSchedule.Visible = False
        ImgBtnUp.Visible = False
        ImgBtnDown.Visible = False
        Me.GV_Housing.Enabled = False
    End Function

#End Region

#Region "Cancel Button & ResetPage()"

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        resetpage()
        Me.txtRepeatation.Text = ""
        Me.Response.Redirect("frmHousingActivities.aspx?mode=1")
    End Sub

    Protected Sub resetpage()

        Me.HFGV_NodeId.Value = ""
        Me.HfMaxNodeId.Value = ""
        Me.HfMaxNodeIndex.Value = ""
        Me.HfPeriod.Value = ""
        Me.HProjectId.Value = ""
        'Me.txtRepeatation.Text = ""
        Me.txtProject.Text = ""
        Me.ddlAct.DataSource = Nothing
        Me.ddlAct.DataBind()
        Me.ddlActivityGroup2.DataSource = Nothing
        Me.ddlActivityGroup2.DataBind()
        Me.ddlParentAct.DataSource = Nothing
        Me.ddlParentAct.DataBind()
        Me.ImgBtnUp.Visible = False
        Me.ImgBtnDown.Visible = False
        Me.GV_Housing.DataSource = Nothing
        Me.GV_Housing.DataBind()
        Me.ViewState(VS_Choice) = Nothing
        Me.ViewState(VS_Dt_GVHousing) = Nothing
        Me.ViewState(Vs_DtWorkSpaceNodeDetail) = Nothing
        Me.ViewState(VS_DtParentActivity) = Nothing

        GenCall()

    End Sub

#End Region

#Region "Helper Function to validate timeInterval"

    Private Function TimeInterval(ByVal CrTime As Decimal) As String
        Dim tottime As Decimal
        Dim arrtime() As String
        Dim hh As Integer, MM As Integer
        Dim StrMM As String = String.Empty
        Dim Newtime As Decimal
        Try

            Newtime = CrTime

            If CrTime.ToString.Trim().IndexOf(".") > -1 Then
                arrtime = Split(CrTime.ToString.Trim(), ".")
                tottime = arrtime(0) * 60

                If arrtime(1).Length < 2 Then
                    arrtime(1) = arrtime(1).Trim() + "0"
                End If

                If arrtime(1) > 59 Then
                    tottime = tottime + arrtime(1)
                    hh = CType(Math.Floor(tottime / 60), Integer)
                    MM = CType(Math.Floor(tottime Mod 60), Integer)
                    StrMM = MM

                    If MM.ToString.Length < 2 Then
                        StrMM = "0" + MM.ToString.Trim()
                    End If
                Else
                    hh = arrtime(0)
                    StrMM = arrtime(1)
                End If
                If arrtime(0).ToString() <> "-0".ToString.Trim() Then
                    Newtime = hh.ToString.Trim() + "." + StrMM.Trim()
                Else
                    Newtime = "-0".ToString.Trim() + "." + StrMM.Trim()
                End If

            End If

            Return Newtime

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....TimeInterval")
            Return ""

        End Try
    End Function

#End Region

#Region "Schedule Button Click"

    Protected Sub btnSchedule_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim irep As Integer
        Dim DtWorkSpaceNodeDetail As New DataTable
        Dim DvWorkSpaceNodeDetail As New DataView
        Dim Dr As DataRow
        Dim Ds_MaxNodeNo As New DataSet

        Dim INodeId As Integer

        Try



            DtWorkSpaceNodeDetail = CType(Me.ViewState(VS_Dt_GVHousing), DataTable)

            For irep = 0 To DtWorkSpaceNodeDetail.Rows.Count - 1
                DtWorkSpaceNodeDetail.Rows(irep).Item("nRefTime") = CType(Me.GV_Housing.Rows(irep).FindControl("txtRefTime"), TextBox).Text.Trim()
                DtWorkSpaceNodeDetail.Rows(irep).Item("vNodeName") = CType(Me.GV_Housing.Rows(irep).FindControl("txtDesc"), TextBox).Text
                DtWorkSpaceNodeDetail.Rows(irep).Item("vNodeDisplayName") = CType(Me.GV_Housing.Rows(irep).FindControl("txtDesc"), TextBox).Text
                If CType(Me.GV_Housing.Rows(irep).FindControl("chkIsPredose"), CheckBox).Checked = True Then
                    DtWorkSpaceNodeDetail.Rows(irep).Item("cIsPreDose") = "Y"
                End If
            Next irep

            DtWorkSpaceNodeDetail.AcceptChanges()

            INodeId = DtWorkSpaceNodeDetail.Rows(0).Item("iNodeId").ToString.Trim()

            'vWorkSpaceId, iNodeIndex, iNodeId, vTemplateId, iNodeNo, vNodeName, vNodeDisplayName, cNodeTypeIndi, 
            'iParentNodeId, iperiod, nMilestone, vActivityId, vFolderName, cCloneFlag, cRequiredFlag, dCheckOutOn, 
            'iCheckOutBy, cPublishFlag, vDocTemplateId, iDocTemplateTranNo, vRemark, iModifyBy, dModifyOn, cStatusIndi, 
            'vDefaultFileFormat, vDocTemplatePath

            DtWorkSpaceNodeDetail.DefaultView.Sort = "nRefTime,iNodeId"
            DtWorkSpaceNodeDetail = DtWorkSpaceNodeDetail.DefaultView.ToTable()

            For Each Dr In DtWorkSpaceNodeDetail.Rows
                Dr("iNodeId") = INodeId
                INodeId += 1
            Next Dr
            DtWorkSpaceNodeDetail.AcceptChanges()

            'Me.HfMaxNodeId.Value = DtWorkSpaceNodeDetail.Rows(DtWorkSpaceNodeDetail.Rows.Count - 1).Item("iNodeId")
            'Me.HfMaxNodeIndex.Value = DtWorkSpaceNodeDetail.Rows(DtWorkSpaceNodeDetail.Rows.Count - 1).Item("iNodeIndex")

            DvWorkSpaceNodeDetail = DtWorkSpaceNodeDetail.Copy().DefaultView
            DvWorkSpaceNodeDetail.Sort = "iNodeId" '"iNodeIndex"

            Me.ViewState(VS_Dt_GVHousing) = DvWorkSpaceNodeDetail.ToTable()
            Me.GV_Housing.DataSource = DvWorkSpaceNodeDetail.ToTable()
            Me.GV_Housing.DataBind()

            Me.HfMaxNodeId.Value = DvWorkSpaceNodeDetail.ToTable.Rows(DvWorkSpaceNodeDetail.ToTable.Rows.Count - 1).Item("iNodeId")

            DvWorkSpaceNodeDetail = Nothing
            DvWorkSpaceNodeDetail = New DataView
            DvWorkSpaceNodeDetail = DtWorkSpaceNodeDetail.Copy().DefaultView
            DvWorkSpaceNodeDetail.Sort = "iNodeIndex"

            Me.HfMaxNodeIndex.Value = DvWorkSpaceNodeDetail.ToTable.Rows(DvWorkSpaceNodeDetail.ToTable.Rows.Count - 1).Item("iNodeIndex")

            If GV_Housing.Rows.Count > 1 Then
                Me.ImgBtnUp.Visible = True
                Me.ImgBtnDown.Visible = True
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....btnSchedule_Click")
        End Try
    End Sub

#End Region

#Region "Search & Exit Buttons"

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        If Me.txtProject.Text.Trim() = "" Then
            Me.objcommon.ShowAlert("Please Enter Project: ", Me.Page)
            Exit Sub
        End If
        Me.divSearch.Visible = True
        Me.pnlSearch.Visible = True

        Me.chkAllSearch.Enabled = True
        If rbtSelection.SelectedValue = Multidose Then
            Me.chkAllSearch.Checked = False
            Me.chkAllSearch.Enabled = False
            Me.ChkPeriodSearch.Checked = True
        End If

        'Me.ddlParentAct.Enabled = False
        'Me.ddlRefAct.Enabled = False



    End Sub

    Protected Sub BtnSearchDiv_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds_WorkspaceNodeDetail As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim dv_Grid As New DataView

        Me.btnSchedule.Visible = False
        Me.ImgBtnUp.Visible = False
        Me.ImgBtnDown.Visible = False

        wStr = "vWorkSpaceId = '" & Me.HProjectId.Value.Trim() & "' And vParentActivityId = '" & _
               Me.ddlParentAct.SelectedValue.Trim() & "' And cCloneFlag = 'H' and cStatusIndi<>'D' " & _
               " And vRefActivityId='" & Me.ddlRefAct.SelectedValue.Trim() & "'"

        If rbtSelection.SelectedValue = Multidose Then
            wStr = "vWorkSpaceId = '" & Me.HProjectId.Value.Trim() & "' And iParentNodeId = " & _
               Me.ddlParentAct.SelectedValue.Trim() & " And cCloneFlag = 'H' and cStatusIndi<>'D' " & _
               " And iRefNodeId = " & Me.ddlRefAct.SelectedValue.Trim()
        End If


        If Not objhelp.GetViewWorkSpaceNodeDetail(wStr, ds_WorkspaceNodeDetail, eStr_Retu) Then

            Me.objcommon.ShowAlert("Error while getting Information: " & eStr_Retu, Me.Page)
            Exit Sub

        End If

        ds_WorkspaceNodeDetail.Tables(0).TableName = "WorkSpaceNodeDetail"
        ds_WorkspaceNodeDetail.Tables(0).AcceptChanges()

        Me.ViewState(Vs_DtWorkSpaceNodeDetail) = ds_WorkspaceNodeDetail.Tables(0)

        Me.ViewState(VS_Choice) = WS_HelpDbTable.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

        'B'coz of Perios Selection
        If Me.ChkPeriodSearch.Checked = True Then
            dv_Grid = Nothing
            dv_Grid = New DataView

            dv_Grid = ds_WorkspaceNodeDetail.Tables(0).DefaultView()
            dv_Grid.RowFilter = "iPeriod =" & Me.ddlPeriodSearch.SelectedValue.Trim()

            Me.ChkPeriod.Checked = True
            Me.ChkPeriod.Checked = True

            Me.ddlPeriod.SelectedValue = Me.ddlPeriodSearch.SelectedValue
            Me.ddlPeriod.Enabled = False

        ElseIf Me.chkAllSearch.Checked = True Then

            dv_Grid = Nothing
            dv_Grid = New DataView

            dv_Grid = ds_WorkspaceNodeDetail.Tables(0).DefaultView()
            dv_Grid.RowFilter = "iPeriod = 1"

            If dv_Grid.ToTable.Rows.Count <= 0 Then
                dv_Grid = Nothing
                dv_Grid = New DataView

                dv_Grid = ds_WorkspaceNodeDetail.Tables(0).DefaultView()
                dv_Grid.RowFilter = "iPeriod = 2"

            End If

            Me.chkAll.Checked = True
            Me.ChkPeriod.Checked = False

        End If

        dv_Grid.Sort = "iNodeId"
        Me.ViewState(VS_Dt_GVHousing) = dv_Grid.ToTable()

        Me.GV_Housing.DataSource = dv_Grid
        Me.GV_Housing.DataBind()

        If GV_Housing.Rows.Count > 1 Then
            Me.btnSchedule.Visible = True
            Me.ImgBtnUp.Visible = True
            Me.ImgBtnDown.Visible = True

            Me.chkAll.Enabled = False
            Me.ChkPeriod.Enabled = False

        End If

        If GV_Housing.Enabled = False Then
            Me.btnSchedule.Visible = False
            Me.ImgBtnUp.Visible = False
            Me.ImgBtnDown.Visible = False
        End If

        Me.divSearch.Visible = False
        Me.pnlSearch.Visible = False

        If dv_Grid.ToTable().Rows.Count <= 0 Then
            Me.objcommon.ShowAlert("No Housing Activity Found! " & eStr_Retu, Me.Page)
        End If

    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Me.divSearch.Visible = False
        Me.pnlSearch.Visible = False

        Me.ddlParentAct.Enabled = True
        Me.ddlRefAct.Enabled = True

    End Sub

    Protected Sub btnExit1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit1.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

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

#Region "Disable Control"
    Public Function DisableControl() As Boolean
        Try
            Me.tractivitygroup.Visible = False
            Me.tractivity.Visible = False
            Me.trrepetitions.Visible = False
            Me.BtnSave.Visible = False
            Me.BtnAdd.Visible = False
            Me.chkAll.Visible = False
            Me.ChkPeriod.Visible = False
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
#End Region

#Region "Enable Control"
    Public Function EnableControl() As Boolean
        Try
            Me.tractivitygroup.Visible = True
            Me.tractivity.Visible = True
            Me.trrepetitions.Visible = True
            Me.BtnSave.Visible = True
            Me.BtnAdd.Visible = True
            Me.chkAll.Visible = True
            Me.ChkPeriod.Visible = True
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
#End Region
End Class
