Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Configuration
Imports System.Collections
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControlslnkGenericScreening_click
Imports System.Web.UI.DataVisualization.Charting
Imports System.Web.UI.MobileControls
Imports System.Web.Services
Imports System.Drawing.Image
Imports Newtonsoft.Json

Partial Class frmWorkspaceDefaultWorkflowUserDtl
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Private Const VS_Choice As String = "Choice"
    Private Const VS_WorkspaceId As String = "WorkspaceId"
    Private Const VS_WorkspaceName As String = "WorkspaceName"
    Private Const VS_DtWorkspaceDefaultWorkFlowUserDtl As String = "DtWorkspaceDefaultWorkFlowUserDtl"
    Private Const VS_StageMst As String = "DtStageMst"
    Private Const VS_Grid As String = "DtGrid"

     Private Const GVC_Delete As Integer = 0
    Private Const GVC_WorkspaceDefaultWorkflowUserId As Integer = 1
    Private Const GVC_WorkspaceId As Integer = 2
    Private Const GVC_WorkspaceDesc As Integer = 3
    Private Const GVC_UserId As Integer = 4
    Private Const GVC_UserName As Integer = 5
    Private Const GVC_UserTypeName As Integer = 6
    Private Const GVC_StageId As Integer = 7
    Private Const GVC_StageDesc As Integer = 8
    Private Const GVC_ModifyOn As Integer = 9
    Private Const GVC_ModifyBy As Integer = 10
    Private Const GVC_isActiveuser As Integer = 12
    Private Const GVC_tableName As Integer = 13
    Private Const GVC_iNodeId As Integer = 14

#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GenCall()
        End If
        If Not IsPostBack Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HidedivProjectwiseUserDetail", "HidedivProjectwiseUserDetail(); ", True)
        End If
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getData", "getData();", True)
    End Sub

#End Region

#Region "GenCall "
    Private Function GenCall() As Boolean
        Dim ds As New DataSet

        Try

            Me.ViewState(VS_Choice) = Me.Request.QueryString("mode").ToString.Trim()
            Me.ViewState(VS_WorkspaceId) = Me.Request.QueryString("WorkspaceId").ToString.Trim()


            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtWorkspaceDefaultWorkFlowUserDtl) = ds.Tables("View_WorkspaceDefaultWorkFlowUserDtl")   ' adding blank DataTable in viewstate

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

#Region "GenCall_Data "
    Private Function GenCall_Data(ByRef ds_Retu As DataSet) As Boolean

        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Val As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing
        Try

            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vWorkspaceId='" & Me.ViewState(VS_WorkspaceId).ToString.Trim() & "'"
            End If


            If Not objHelp.View_WorkspaceDefaultWorkflowUserDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            If ds.Tables(0).Rows.Count <= 0 And _
             Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found for Selected Operation")
            End If

            ds_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_Data")

        Finally

        End Try
    End Function
#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim dt_OpMst As DataTable = Nothing
        Dim Ds_Stage As New DataSet
        Dim estr As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try

            Page.Title = ":: Projectwise User Rights   :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Projectwise User Rights"

            Me.lblInformation.Text = "<br/>Project Name: " & Me.Request.QueryString("ProjectNo").ToString.Trim() + Me.Request.QueryString("WorkspaceName").ToString.Trim() & "<br/>"

            dt_OpMst = Me.ViewState(VS_DtWorkspaceDefaultWorkFlowUserDtl)

            Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit 'Always in Edit Mode

            If Not FillDropDown() Then
                Exit Function
            End If

            If Not FillGrid() Then
                Exit Function
            End If

            If Not FillDropDownUserProfile() Then
                Exit Function
            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....GenCall_ShowUI")
        Finally
        End Try
    End Function

#End Region

#Region "FillDropDown"

    Private Function FillDropDown() As Boolean
        Dim ds_UserGroup As New Data.DataSet
        Dim ds_UserType As New Data.DataSet
        Dim ds_Location As New Data.DataSet
        Dim Ds_Workspace As New Data.DataSet
        Dim Ds_Node As New Data.DataSet
        Dim Ds_Stage As New Data.DataSet
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim wstr_1 As String = String.Empty
        Dim scopeno As String = String.Empty
        Dim dv_userscope As DataView
        Dim deptcode As String = Me.Session(S_DeptCode.ToString())
        Dim Type As String = String.Empty
        Dim temptstr As String = String.Empty
        Dim ds_scopevalue As New DataSet
        Dim wstr_2 As String = String.Empty

        Try
            wstr += "cStatusIndi <> 'D'"

            If Not objHelp.getUserTypeMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_scopevalue, estr) Then
                Me.ObjCommon.ShowAlert("Error While getting Data from UserTypeMst!", Me.Page)
                Return False
            End If

            dv_userscope = ds_scopevalue.Tables(0).DefaultView
            dv_userscope.Sort = "vUserTypeName"
            Me.DdlUserType.DataSource = dv_userscope.ToTable()
            Me.DdlUserType.DataValueField = "vUserTypeCode"
            Me.DdlUserType.DataTextField = "vUserTypeName"
            Me.DdlUserType.DataBind()
            Me.DdlUserType.Items.Insert(0, New ListItem("Select User Type", 0))
            Me.DdlUserType.SelectedIndex = 0

            'To fill location dropdown : Start

            If Not Me.objHelp.getLocationMst("cStatusIndi <> 'D'  order by vLocationName", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                   ds_Location, estr) Then
                Me.ObjCommon.ShowAlert("Error While getting Data from LocationMst", Me.Page)
                Return False
            End If

            Me.DdlLocation.DataSource = ds_Location
            Me.DdlLocation.DataValueField = "vLocationCode"
            Me.DdlLocation.DataTextField = "vLocationName"
            Me.DdlLocation.DataBind()
            Me.DdlLocation.Items.Insert(0, New ListItem("All Locations", 0))
            Me.DdlLocation.SelectedIndex = 0

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillDropDown")
            Return False
        End Try
    End Function

    Private Function FillDropDownUserProfile() As Boolean
        Dim ds_UserProfile As New Data.DataSet
        Dim dv_UserProfile As New DataView
        Dim estr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim Type As String = String.Empty
        Dim deptcode As String = Me.Session(S_DeptCode.ToString())

        Try
            Type = Convert.ToString(Me.Request.QueryString("Type")).Trim()
            wStr = "cStatusIndi <> 'D' And vWorkspaceId='" & Me.ViewState(VS_WorkspaceId).ToString.Trim() & "'"
            If (Type.ToString() = "DUTYDELEGATION") Then
                wStr = wStr + " and vDeptCode = '" + deptcode + "'"
            End If

            If Not objHelp.View_WorkspaceDefaultWorkflowUserDtl_New(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_UserProfile, estr) Then

                Response.Write(estr)
                Exit Function

            End If
            dv_UserProfile = ds_UserProfile.Tables(0).DefaultView
            dv_UserProfile = dv_UserProfile.ToTable(True, "vUserTypeName").DefaultView()
            dv_UserProfile.Sort = "vUserTypeName"
            Me.DDLUserprofile.DataSource = dv_UserProfile.ToTable()
            Me.DDLUserprofile.DataTextField = "vUserTypeName"
            Me.DDLUserprofile.DataBind()
            Me.DDLUserprofile.Items.Insert(0, New ListItem("All Profile", 0))

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillDropDownUserProfile")
            Return False
        End Try
    End Function

    Private Function fillUser() As Boolean
        Dim Ds_User As New Data.DataSet
        Dim Ds_Stage As New Data.DataSet
        Dim ds_ScopeMst As New Data.DataSet ' added by vishal
        Dim Dv_User As New Data.DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim Str_ProjectType As String = String.Empty
        
        Dim ds_scopevalue As New DataSet
        Dim Type As String = String.Empty
        Dim deptcode As String = Me.Session(S_DeptCode.ToString())
        
        Try
            Me.hdnUserId.Value = ""

            If Me.DdlUserType.SelectedIndex = 0 Then
                Return False
            End If

            Type = Convert.ToString(Me.Request.QueryString("Type")).Trim()

            If (Type.ToString() = "DUTYDELEGATION") Then
                wstr = "vUserTypeCode= '" & Me.DdlUserType.SelectedValue.Trim() & "'" + " And cStatusIndi <> 'D' and vDeptCode = '" + deptcode + "'"
            Else
                wstr = " vUserTypeCode= '" & Me.DdlUserType.SelectedValue.Trim() & "'" + " And cStatusIndi <> 'D'"
            End If

            If Me.DdlLocation.SelectedIndex > 0 Then
                wstr += wstr + " and vLocationCode = '" & Me.DdlLocation.SelectedValue.Trim() & "'"
            End If

            If Not Me.objHelp.GetViewUserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_scopevalue, estr) Then
                Me.ObjCommon.ShowAlert("Error While getting Data from UserMst!", Me.Page)
                Return False

            End If

            Dv_User = ds_scopevalue.Tables(0).DefaultView()
            Dv_User.Sort = "vUserName"
            Me.ddlUserName.DataSource = Dv_User.ToTable()
            Me.ddlUserName.DataValueField = "iUserId"
            Me.ddlUserName.DataTextField = "vUserName"
            Me.ddlUserName.DataBind()
            
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......fillUser")
            Return True
        End Try
    End Function

    Private Function FillGrid() As Boolean
        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim ds_Grid As New DataSet
        Dim Type As String = String.Empty
        Dim deptcode As String = Me.Session(S_DeptCode.ToString())

        Try
            Type = Convert.ToString(Me.Request.QueryString("Type")).Trim()
            wStr = "cStatusIndi <> 'D' And vWorkspaceId='" & Me.ViewState(VS_WorkspaceId).ToString.Trim() & "'"
            If (Type.ToString() = "DUTYDELEGATION") Then
                If DDLUserprofile.SelectedIndex <= 0 Then
                    wStr = wStr + " and vDeptCode = '" + deptcode + "'"
                Else
                    wStr = wStr + " and vUserTypeName = '" & Me.DDLUserprofile.SelectedItem.Text & "' and vDeptCode = '" + deptcode + "'"
                End If
            Else
                If Not DDLUserprofile.SelectedIndex <= 0 Then
                    wStr = wStr + " and vUserTypeName = '" & Me.DDLUserprofile.SelectedItem.Text & "'"
                End If
            End If
            If Not DDLUserstatus.SelectedIndex <= 0 Then
                wStr = wStr + " and isactiveUser = '" & Me.DDLUserstatus.SelectedItem.Value & "'"
            End If
            wStr = wStr + "order by dmodifyon desc"

            If Not objHelp.View_WorkspaceDefaultWorkflowUserDtl_New(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_Grid, eStr_Retu) Then

                Response.Write(eStr_Retu)
                Exit Function

            End If

            Me.ViewState(VS_Grid) = ds_Grid.Tables(0)

            If ds_Grid.Tables(0).Rows.Count() <= 0 Then
                Me.BtnDelete.Visible = False
                Me.Btnexptexcl.Visible = False
                Me.GV_UserStage_Edit.DataBind()
                If GV_UserStage_Edit.Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGV_UserStage_Edit", "UIGV_UserStage_Edit(); ", True)
                End If
                Return True
            End If

            Me.GV_UserStage_Edit.DataSource = ds_Grid.Tables(0)
            Me.GV_UserStage_Edit.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGV_UserStage_Edit", "UIGV_UserStage_Edit(); ", True)

            Me.BtnDelete.Visible = True
            Me.Btnexptexcl.Visible = True


            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillGrid")
            Return False
        End Try
    End Function

#End Region

    '#Region "SelectedIndexChanged"
    '    Protected Sub DdlUserType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '        FillDropDownUserProfile()
    '        fillUser()
    '    End Sub

    '    Protected Sub DdlLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '        FillDropDownUserProfile()
    '        fillUser()
    '    End Sub

    '#End Region

#Region "Save Button"

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim ds_Save As New DataSet
        Dim dt_Save As New DataTable
        Dim dv_Save As New DataView

        Dim ds_Delete As New DataSet
        Dim dt_Delete As New DataTable
        Dim dv_Delete As New DataView
        Dim Success As Boolean = False
        Dim estr As String = String.Empty
        Dim Str_Worn As String = String.Empty
        Try

            ' Added by Jeet Patel on 06-Jun-2015 
            If CheckProjectStatus() Then
                Me.ObjCommon.ShowAlert("Project is Locked", Me.Page())
                Exit Sub
            End If
            '-------------------------------------------------

            If Not AssignValues(Str_Worn) Then
                Me.ObjCommon.ShowAlert("Error While Assinging Data", Me.Page())
                Exit Sub
            End If

            'If Str_Worn.Trim() <> "" Then
            'Me.ObjCommon.ShowAlert(Str_Worn, Me.Page)
            'End If

            dt_Save = CType(Me.ViewState(VS_DtWorkspaceDefaultWorkFlowUserDtl), DataTable)
            If dt_Save.Rows.Count > 0 Then

                ds_Save = New DataSet
                dt_Save.TableName = "View_WorkspaceDefaultWorkFlowUserDtl"
                ds_Save.Tables.Add(dt_Save.Copy())

                If Not objLambda.Save_WorkspaceDefaultWorkFlowUserDtl(Me.ViewState(VS_Choice), ds_Save, Me.Session(S_UserID), estr) Then
                    Me.ObjCommon.ShowAlert("Error While Saving WorkspaceDefaultWorkFlowUserDtl", Me.Page)
                    Exit Sub

                End If

            End If
            Me.DDLUserstatus.SelectedIndex = 0
            ObjCommon.ShowAlert("Record Saved Successfully. \n" + Str_Worn, Me.Page)
            ResetPage()
            FillGrid()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...BtnSave_Click")
        End Try
    End Sub

    Private Function AssignValues(ByRef Str_Worn As String) As Boolean
        Dim dr1 As DataRow
        Dim Index3 As Integer
        Dim dt_UserRights As New DataTable
        Dim dt_NewGrid As New DataTable
        Dim dt_Grid As New DataTable
        Dim dt_ActOperationMxt As New DataTable
        Dim dv_Grid As New DataView
        Dim Wstr As String = String.Empty
        Dim ds_Dtl As New DataSet
        Dim ds_UserMst As New DataSet
        Dim eStr_Retu As String = String.Empty
        Dim ds_ChildActivity As New DataSet
        Dim Duplicate As Boolean
        Dim ds_Grid As New DataSet

        Try

            'Retrive ActivityId from WorkspaceNodeDetail
            dt_UserRights = CType(Me.ViewState(VS_DtWorkspaceDefaultWorkFlowUserDtl), DataTable)
            dt_UserRights.Clear()

            'dt_Grid = CType(Me.ViewState(VS_Grid), DataTable)

            Wstr = "cStatusIndi <> 'D' And vWorkspaceId='" & Me.ViewState(VS_WorkspaceId).ToString.Trim() & "'"
            If Not objHelp.View_WorkspaceDefaultWorkflowUserDtl_New(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_Grid, eStr_Retu) Then
                Response.Write(eStr_Retu)
            End If
            dt_Grid = CType(ds_Grid.Tables(0), DataTable)

            Dim userArray As String() = Convert.ToString(Me.hdnUserId.Value).Split(New Char() {","c})

            Dim strvalue As String
            For Each strvalue In userArray
                Duplicate = False
                For Index3 = 0 To dt_Grid.Rows.Count - 1
                    If Convert.ToString(dt_Grid.Rows(Index3).Item("vUserName")) = strvalue And _
                        dt_Grid.Rows(Index3).Item("cStatusIndi") <> "D" Then
                        Str_Worn += "\nUser '" & strvalue & "' For Stage "
                        Duplicate = True
                        Exit For
                    End If
                Next Index3
                If Duplicate = True Then

                Else
                    dr1 = dt_UserRights.NewRow()
                    dr1("nWorkspaceDefaultWorkflowUserId") = dt_UserRights.Rows.Count + 1
                    dr1("vWorkspaceId") = Me.ViewState(VS_WorkspaceId)
                    dr1("vWorkspaceDesc") = Me.ViewState(VS_WorkspaceName)
                    dr1("iUserId") = strvalue
                    dr1("vUserName") = ""
                    'If (chkTrainingAssign.Checked = True) Then
                    '    dr1("cIsTrainingAssign") = "Y"
                    '    dr1("vTrainingFinished") = "No"
                    'Else
                    '    dr1("cIsTrainingAssign") = "N"
                    'End If
                    dr1("cStatusIndi") = "N"
                    dr1("iModifyBy") = Me.Session(S_UserID)
                    dt_UserRights.Rows.Add(dr1)
                End If

            Next

            Me.ViewState(VS_DtWorkspaceDefaultWorkFlowUserDtl) = dt_UserRights

            AssignValues = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message(), "......AssignValues")
            AssignValues = False
        End Try
    End Function

#End Region

#Region "Button Click"

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click

        Me.Response.Redirect(Me.Request.QueryString("page").ToString.Trim() & ".aspx?mode=1&Type=" & Me.Request.QueryString("Type").ToString.Trim())

        'Me.Response.Redirect("frmMyProject.aspx?mode=1&Type=ALL")

    End Sub

    Protected Sub BtnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim index As New Integer
        Dim index3 As New Integer
        Dim dt_UserRights As New DataTable
        Dim dt_Grid As New DataTable
        Dim dt_NewGrid As New DataTable
        Dim dv_Grid As New DataView
        Dim chk As New CheckBox
        Dim dr As DataRow
        Dim ds_Delete As New DataSet
        Dim dt_Delete As New DataTable
        Dim dv_Delete As New DataView
        Dim estr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim userstr As String = String.Empty
        Dim ds_Grid As New DataSet

        Try

            ' Added by Jeet Patel on 06-Jun-2015 
            If CheckProjectStatus() Then
                Me.ObjCommon.ShowAlert("Project is Locked", Me.Page())
                Exit Sub
            End If
            '-------------------------------------------------

            dt_UserRights = CType(Me.ViewState(VS_DtWorkspaceDefaultWorkFlowUserDtl), DataTable).Copy()

            For index = 0 To Me.GV_UserStage_Edit.Rows.Count - 1

                chk = CType(Me.GV_UserStage_Edit.Rows(index).FindControl("CHKDelete"), CheckBox)

                If chk.Checked = True Then

                    dr = dt_UserRights.NewRow()
                    dr("nWorkspaceDefaultWorkflowUserId") = Me.GV_UserStage_Edit.Rows(index).Cells(GVC_WorkspaceDefaultWorkflowUserId).Text
                    dr("vWorkspaceId") = Me.GV_UserStage_Edit.Rows(index).Cells(GVC_WorkspaceId).Text
                    dr("vWorkspaceDesc") = Me.GV_UserStage_Edit.Rows(index).Cells(GVC_WorkspaceDesc).Text
                    dr("iUserId") = Me.GV_UserStage_Edit.Rows(index).Cells(GVC_UserId).Text
                    dr("vUserName") = Me.GV_UserStage_Edit.Rows(index).Cells(GVC_UserName).Text
                    dr("iStageId") = Me.GV_UserStage_Edit.Rows(index).Cells(GVC_StageId).Text
                    dr("vStageDesc") = Me.GV_UserStage_Edit.Rows(index).Cells(GVC_StageDesc).Text
                    dr("cStatusIndi") = "D"

                    dt_UserRights.Rows.Add(dr)

                End If

            Next index

            'For Deleted
            If dt_UserRights.Rows.Count > 0 Then

                ds_Delete = New DataSet
                dt_UserRights.TableName = "View_WorkspaceDefaultWorkFlowUserDtl"
                ds_Delete.Tables.Add(dt_UserRights.Copy())

                If Not objLambda.Save_WorkspaceDefaultWorkFlowUserDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                        ds_Delete, Me.Session(S_UserID), estr) Then

                    ObjCommon.ShowAlert("Error While Saving WorkspaceDefaultWorkFlowUserDtl", Me.Page)
                    Exit Sub
                End If

                If userstr.Length <= 0 Then
                    ObjCommon.ShowAlert("Record Deleted SuccessFully", Me.Page)
                    FillGrid()
                Else
                    ObjCommon.ShowAlert(userstr + " - Already worked on this project so you can not delete his/her rights.", Me.Page)
                    FillGrid()
                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......BtnDelete_Click")
        End Try

    End Sub

    Protected Sub Btnhome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btnhome.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

#Region "Grid Event"

    Protected Sub GV_UserStage_Edit_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Then

            CType(e.Row.FindControl("ImgDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImgDelete"), ImageButton).CommandName = "DELETE"
            e.Row.Cells(GVC_ModifyOn).Text = CDate(e.Row.Cells(GVC_ModifyOn).Text).ToString("dd-MMM-yyyy HH:mm") + strServerOffset

            'If (e.Row.Cells(GVC_TrainingStartDate).Text <> "&nbsp;") Then
            '    e.Row.Cells(GVC_TrainingStartDate).Text = CDate(e.Row.Cells(GVC_TrainingStartDate).Text).ToString("dd-MMM-yyyy HH:mm") + strServerOffset
            'End If

            'If (e.Row.Cells(GVC_dTrainingEndDate).Text <> "&nbsp;") Then
            '    e.Row.Cells(GVC_dTrainingEndDate).Text = CDate(e.Row.Cells(GVC_dTrainingEndDate).Text).ToString("dd-MMM-yyyy HH:mm") + strServerOffset
            'End If

            If e.Row.Cells(GVC_isActiveuser).Text.Trim.ToUpper() = "Y" Then
                e.Row.Cells(GVC_Delete).Enabled = False
                CType(e.Row.FindControl("ImgDelete"), ImageButton).Visible = False
            End If

        End If

    End Sub

    Protected Sub GV_UserStage_Edit_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim index As Integer = e.CommandArgument
        Dim dt_UserRights As New DataTable
        Dim Ds_UserRights As New DataSet
        Dim dt_NewGrid As New DataTable
        Dim dv_Grid As New DataView
        Dim index3 As New Integer
        Dim dr As DataRow
        Dim estr As String = String.Empty
        Dim strmsg As String = String.Empty
        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim ds_Grid As New DataSet

        ' Added by Jeet Patel on 06-Jun-2015 
        If CheckProjectStatus() Then
            Me.ObjCommon.ShowAlert("Project is Locked", Me.Page())
            Exit Sub
        End If
        '-------------------------------------------------


        If e.CommandName.ToUpper = "EDIT" Then

            CType(GV_UserStage_Edit.Rows(index).FindControl("ImgSave"), ImageButton).Visible = True
            CType(GV_UserStage_Edit.Rows(index).FindControl("ImgCancel"), ImageButton).Visible = True
            CType(GV_UserStage_Edit.Rows(index).FindControl("ImgEdit"), ImageButton).Visible = False

            CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).Enabled = True
            CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).DataSource = CType(Me.ViewState(VS_StageMst), DataTable)
            CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).DataValueField = "istageid"
            CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).DataTextField = "vStageDesc"
            CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).DataBind()

            CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).SelectedValue = GV_UserStage_Edit.Rows(index).Cells(GVC_StageId).Text.Trim()

        ElseIf e.CommandName.ToUpper = "UPDATE" Or e.CommandName.ToUpper = "DELETE" Then

            If Me.GV_UserStage_Edit.Rows(index).Cells(GVC_tableName).Text = "WorkspaceDefaultWorkflowUserDtl" Then
                dt_UserRights = CType(Me.ViewState(VS_DtWorkspaceDefaultWorkFlowUserDtl), DataTable).Copy()
                dr = dt_UserRights.NewRow()
                dr("nWorkspaceDefaultWorkflowUserId") = Me.GV_UserStage_Edit.Rows(index).Cells(GVC_WorkspaceDefaultWorkflowUserId).Text
            ElseIf Me.GV_UserStage_Edit.Rows(index).Cells(GVC_tableName).Text = "WorkspaceWorkflowUserDtl" Then
                If Not objHelp.getworkspaceWorkflowUserDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, Ds_UserRights, estr) Then
                    ObjCommon.ShowAlert("Error While Saving WorkspaceWorkFlowUserDtl", Me.Page)
                    Exit Sub
                End If
                dt_UserRights = Ds_UserRights.Tables(0)
                dr = dt_UserRights.NewRow()
                dr("nWorkspaceWorkflowUserId") = Me.GV_UserStage_Edit.Rows(index).Cells(GVC_WorkspaceDefaultWorkflowUserId).Text
                dr("iNodeId") = Me.GV_UserStage_Edit.Rows(index).Cells(GVC_iNodeId).Text
            End If

            dr("vWorkspaceId") = Me.GV_UserStage_Edit.Rows(index).Cells(GVC_WorkspaceId).Text
            dr("iUserId") = Me.GV_UserStage_Edit.Rows(index).Cells(GVC_UserId).Text
            dr("iModifyBy") = Me.Session(S_UserID)


            'For Edit
            If e.CommandName.ToUpper = "UPDATE" Then
                dr("iStageId") = CType(Me.GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).SelectedValue.Trim()
                dr("vStageDesc") = CType(Me.GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).SelectedItem.Text.Trim()
                dr("cStatusIndi") = "E"

                'For Delete
            ElseIf e.CommandName.ToUpper = "DELETE" Then
                dr("iStageId") = Me.GV_UserStage_Edit.Rows(index).Cells(GVC_StageId).Text
                'dr("vStageDesc") = Me.GV_UserStage_Edit.Rows(index).Cells(GVC_StageDesc).Text
                dr("cStatusIndi") = "D"
            End If

            dt_UserRights.Rows.Add(dr)

            If dt_UserRights.Rows.Count > 0 Then

                Ds_UserRights = New DataSet
                dt_UserRights.TableName = "View_WorkspaceDefaultWorkFlowUserDtl"
                If Me.GV_UserStage_Edit.Rows(index).Cells(GVC_tableName).Text = "WorkspaceWorkflowUserDtl" Then
                    dt_UserRights.TableName = "WorkspaceWorkFlowUserDtl"
                End If
                Ds_UserRights.Tables.Add(dt_UserRights.Copy())

                If Me.GV_UserStage_Edit.Rows(index).Cells(GVC_tableName).Text = "WorkspaceDefaultWorkflowUserDtl" Then

                    If Not objLambda.Save_WorkspaceDefaultWorkFlowUserDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                         Ds_UserRights, Me.Session(S_UserID), estr) Then

                        ObjCommon.ShowAlert("Error While Saving WorkspaceDefaultWorkFlowUserDtl", Me.Page)
                        Exit Sub

                    End If

                ElseIf Me.GV_UserStage_Edit.Rows(index).Cells(GVC_tableName).Text = "WorkspaceWorkflowUserDtl" Then

                    If Not objLambda.Save_InsertWorkspaceWorkflowUserDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                         Ds_UserRights, "", Me.Session(S_UserID), estr) Then

                        ObjCommon.ShowAlert("Error While Saving WorkspaceWorkFlowUserDtl", Me.Page)
                        Exit Sub

                    End If

                End If

                strmsg = "Record Updated SuccessFully"
                If e.CommandName.ToUpper = "DELETE" Then
                    strmsg = "Record Deleted SuccessFully"
                End If

                ObjCommon.ShowAlert(strmsg, Me.Page)
                Me.DDLUserprofile.SelectedIndex = 0
                FillGrid()
                'Page_Load(sender, e)

            End If

        ElseIf e.CommandName.ToUpper = "CANCEL" Then

            CType(GV_UserStage_Edit.Rows(index).FindControl("ImgSave"), ImageButton).Visible = False
            CType(GV_UserStage_Edit.Rows(index).FindControl("ImgCancel"), ImageButton).Visible = False
            CType(GV_UserStage_Edit.Rows(index).FindControl("ImgEdit"), ImageButton).Visible = True

            CType(GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).Enabled = False


        End If

    End Sub

    Protected Sub GV_UserStage_Edit_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs)
        e.Cancel = True
    End Sub

    Protected Sub GV_UserStage_Edit_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs)

    End Sub

    Protected Sub GV_UserStage_Edit_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or _
                e.Row.RowType = DataControlRowType.Header Or _
                e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVC_WorkspaceDefaultWorkflowUserId).Visible = False
            e.Row.Cells(GVC_WorkspaceId).Visible = False
            e.Row.Cells(GVC_UserId).Visible = False
            e.Row.Cells(GVC_StageId).Visible = False
            e.Row.Cells(GVC_isActiveuser).Visible = False
            e.Row.Cells(GVC_tableName).Visible = False
            e.Row.Cells(GVC_iNodeId).Visible = False
            e.Row.Cells(GVC_ModifyOn).Visible = False
            e.Row.Cells(GVC_ModifyBy).Visible = False
            e.Row.Cells(GVC_StageDesc).Visible = False

        End If
    End Sub

    Protected Sub GV_UserStage_Edit_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        GV_UserStage_Edit.PageIndex = e.NewPageIndex
        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub

    Protected Sub GV_UserStage_Edit_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs)

    End Sub

    Protected Sub GV_UserStage_Edit_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)

    End Sub

#End Region

#Region "ResetPage"
    Private Sub ResetPage()
        Me.GV_UserStage_Edit.DataSource = Nothing
        Me.GV_UserStage_Edit.DataBind()
        Me.ddlUserName.Items.Clear()
        Me.chkTrainingAssign.Checked = False
        If Not GenCall() Then
            Exit Sub
        End If
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ClearValue", "ClearValue(); ", True)

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

    'Protected Sub chklstStages_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles chklstStages.DataBound

    'End Sub

    Protected Sub DdlUserType_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlUserType.DataBound

    End Sub

    Protected Sub DDLUserprofile_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLUserprofile.SelectedIndexChanged
        FillGrid()
    End Sub

    Protected Sub DDLUserstatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLUserstatus.SelectedIndexChanged
        FillGrid()
    End Sub

    'Added for Export to Excel data By Mrunal on 14-dec-2011
    Protected Sub Btnexptexcl_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim fileName As String = String.Empty
        Dim dscntuser As New DataSet
        Try

            


            Dim wStr As String = String.Empty
            Dim eStr_Retu As String = String.Empty
            Dim ds_Grid As New DataSet
            Dim Type As String = String.Empty
            Dim deptcode As String = Me.Session(S_DeptCode.ToString())

            Type = Convert.ToString(Me.Request.QueryString("Type")).Trim()
            wStr = "cStatusIndi <> 'D' And vWorkspaceId='" & Me.ViewState(VS_WorkspaceId).ToString.Trim() & "'"
            If (Type.ToString() = "DUTYDELEGATION") Then
                If DDLUserprofile.SelectedIndex <= 0 Then
                    wStr = wStr + " and vDeptCode = '" + deptcode + "'"
                Else
                    wStr = wStr + " and vUserTypeName = '" & Me.DDLUserprofile.SelectedItem.Text & "' and vDeptCode = '" + deptcode + "'"
                End If
            Else
                If Not DDLUserprofile.SelectedIndex <= 0 Then
                    wStr = wStr + " and vUserTypeName = '" & Me.DDLUserprofile.SelectedItem.Text & "'"
                End If
            End If
            If Not DDLUserstatus.SelectedIndex <= 0 Then
                wStr = wStr + " and isactiveUser = '" & Me.DDLUserstatus.SelectedItem.Value & "'"
            End If
            wStr = wStr + "order by dmodifyon desc"

            If Not objHelp.View_WorkspaceDefaultWorkflowUserDtl_New(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_Grid, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Sub
            End If



            If ds_Grid.Tables(0).Rows.Count < 1 Then
                Me.ObjCommon.ShowAlert("No data to Export", Me.Page)
                Exit Sub
            End If


            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            fileName = "projectwiseright"
            fileName = fileName & ".xls"
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)


            dscntuser.Tables.Add(CType(ds_Grid.Tables(0), DataTable).Copy())
            dscntuser.AcceptChanges()

            Context.Response.Write(ConvertDscntuserTO(dscntuser))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception
            'Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#Region "Export To Excel for Projectwise user rights"
    Private Function ConvertDscntuserTO(ByVal ds As DataSet) As String
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer
        Dim dsConvert As New DataSet

        Try

            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""6""><center><strong><font color=""#000000"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append("Project Wise User Rights")
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""6""><center><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append("Project Name : " + Me.Request.QueryString("ProjectNo").ToString.Trim() + ":" + Me.Request.QueryString("WorkspaceName").ToString.Trim())
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")

            dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vUserName,vUserTypeName,vStageDesc,ModifierName,dModifyOn,IsactiveUser".Split(",")).DefaultView.ToTable())
            dsConvert.AcceptChanges()
            'dsConvert.Tables(0).Columns(0).ColumnName = "Project Name"
            dsConvert.Tables(0).Columns(0).ColumnName = "User Name"
            dsConvert.Tables(0).Columns(1).ColumnName = "Profile"
            dsConvert.Tables(0).Columns(2).ColumnName = "Stage Name"
            dsConvert.Tables(0).Columns(3).ColumnName = "Modify By"
            dsConvert.Tables(0).Columns(4).ColumnName = "Modify On"
            dsConvert.Tables(0).Columns(5).ColumnName = "Is User Involved in Project?"
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
                    If (Convert.ToString(dsConvert.Tables(0).Columns(i).ColumnName).ToUpper = "IS USER INVOLVED IN PROJECT?") Then
                        If ((Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim()) = "N") Then
                            strMessage.Append("NO")
                        ElseIf ((Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim()) = "Y") Then
                            strMessage.Append("YES")
                        End If
                    Else
                        strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim())
                    End If
                    strMessage.Append("</font></td>")

                Next
                strMessage.Append("</tr>")
            Next
            strMessage.Append("</table>")

            Return strMessage.ToString

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "......ConvertDscntuserTO")
            Return ""
        End Try
    End Function
#End Region


    Protected Function CheckProjectStatus() As Boolean

        Dim wstr As String = String.Empty
        Dim ds_CheckStatus As DataSet = New DataSet
        Dim eStr As String = String.Empty
        Dim dv_Check As DataView

        wstr = "vWorkspaceId='" & Me.ViewState(VS_WorkspaceId).ToString.Trim() & "'"

        If Not Me.objHelp.GetCRFLockDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_CheckStatus, eStr) Then
            Throw New Exception(eStr)
        End If
        If Not ds_CheckStatus Is Nothing Then

            dv_Check = ds_CheckStatus.Tables(0).DefaultView
            dv_Check.Sort = "iTranNo desc"
            ' edited by vishal for lock/unlock site
            If dv_Check.ToTable().Rows.Count > 0 Then

                If Convert.ToString(dv_Check.ToTable().Rows(0)("cLockFlag")).Trim.ToUpper() = "L" Then
                    Me.ObjCommon.ShowAlert("Project is Locked.", Me.Page)
                    Return True
                End If
            End If
        End If

        Return False
    End Function

    'Add by shivani pandya for Project lock
#Region "LockImpact"
    <Services.WebMethod()> _
    Public Shared Function LockImpact(ByVal WorkspaceID As String) As String
        Dim ds_Check As DataSet = Nothing
        Dim dtProjectStatus As New DataTable
        Dim ProjectStatus As String = String.Empty
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim iTran As String = String.Empty

        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()

        wStr = "vWorkspaceId = '" + WorkspaceID.ToString() + "' And cStatusIndi <> 'D'"

        If Not objHelp.GetCRFLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_Check, eStr) Then
            Throw New Exception(eStr)
        End If

        If ds_Check.Tables(0).Rows.Count > 0 Then

            dtProjectStatus = ds_Check.Tables(0)
            iTran = dtProjectStatus.Compute("Max(iTranNo)", String.Empty)
            dtProjectStatus.DefaultView.RowFilter = "iTranNo =" + iTran.ToString()

            If dtProjectStatus.DefaultView.ToTable.Rows.Count > 0 Then
                ProjectStatus = dtProjectStatus.DefaultView.ToTable().Rows(0)("cLockFlag").ToString()
            End If
        Else
            ProjectStatus = "U"
        End If

        Return ProjectStatus
    End Function

#End Region

#Region "Web Method"

    <WebMethod> _
    Public Shared Function Proc_GetWorkSpaceProjectTrainingGuidline(ByVal vWorkSpaceId As String) As String

        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wstr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim ds_save As Data.DataSet = Nothing
        Dim eStr As String = String.Empty

        Try
            wstr = "vWorkspaceId = " + vWorkSpaceId + " and cUploadType= 'P' and cDocType = 'T'"

            If Not objHelp.GetViewCRFUploadGuidelineDetailHistory(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_save, eStr) Then
                Throw New Exception("Error while GetCRFUploadGuidelineDetail()")
            End If

            If ds_save.Tables(0).Rows.Count > 0 Then
                strReturn = "true'"
            Else
                strReturn = "false"
            End If

            Return strReturn
        Catch ex As Exception
            Return strReturn
        End Try
    End Function


    <WebMethod> _
    Public Shared Function View_WorkSpaceDefaultUserDtl(ByVal wstr As String, ByVal Type As String, ByVal WorkSpaceID As String) As String
        Dim eStr_Retu As String = String.Empty
        Dim ds_Grid As New DataSet
        Dim deptcode As String = HttpContext.Current.Session(S_DeptCode.ToString())
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim strReturn As String = String.Empty

        Try
            wstr = wstr + " And vWorkspaceId='" & WorkSpaceID & "'"
            If (Type.ToString() = "DUTYDELEGATION") Then
                wstr = wstr + " and vDeptCode = '" + deptcode + "'"
            End If

            wstr = wstr + " order by dmodifyon desc"
            If Not objHelp.View_WorkspaceDefaultWorkflowUserDtl_New(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_Grid, eStr_Retu) Then
                Return False
                Exit Function
            End If
            strReturn = JsonConvert.SerializeObject(ds_Grid)
            Return strReturn
        Catch ex As Exception
            'Me.ShowErrorMessage(ex.Message, "...FillGrid")
            Return strReturn
        End Try

    End Function


    <WebMethod> _
    Public Shared Function Delete_WorkSpaceFlow(ByVal workspaceflowid As Integer, ByVal vWorkspaceId As String, ByVal vtablename As String, ByVal iUserId As String, ByVal iStageId As String) As String
        ',   ByVal iNodeId As String
        ', 
        'ByVal vTablename As String, ByVal iNodeid As Integer
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim Wstr1 As String = String.Empty
        Dim Wstr2 As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim ds_RoleOp As New Data.DataSet
        Dim ds As New Data.DataSet
        Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
        Dim objOPws As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
        Dim Dr_ActOp As DataRow
        Dim Dt_Actopt As New DataTable
        Dim ds_Save1 As New DataSet
        Dim ds_Save2 As New DataSet
        Dim ds_GetTemplateMst As New DataSet
        Dim MultipleTemplatedId As String = String.Empty
        Dim TemplateDesc As String = String.Empty
        Dim dr As DataRow
        Dim ds_Grid As New DataSet
        Dim dt_UserRights As New DataTable
        Dim Ds_UserRights As New DataSet
        Dim dt_NewGrid As New DataTable
        Dim VS_DtWorkspaceDefaultWorkFlowUserDtl As String = "DtWorkspaceDefaultWorkFlowUserDtl"
        Dim iNodeId As String = "0"

        Try

            If vtablename = "WorkspaceDefaultWorkflowUserDtl" Then
                'dt_UserRights = CType(VS_DtWorkspaceDefaultWorkFlowUserDtl, DataTable).Copy()

                If Not objHelp.View_WorkspaceDefaultWorkflowUserDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, Ds_UserRights, estr) Then
                    Exit Function
                End If


                dt_UserRights = Ds_UserRights.Tables(0)
                dr = dt_UserRights.NewRow()
                dr("nWorkspaceDefaultWorkflowUserId") = workspaceflowid
            ElseIf vtablename = "WorkspaceWorkflowUserDtl" Then
                If Not objHelp.getworkspaceWorkflowUserDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, Ds_UserRights, estr) Then
                    'ObjCommon.ShowAlert("Error While Saving WorkspaceWorkFlowUserDtl", Me.Page)
                    Exit Function
                End If

                dt_UserRights = Ds_UserRights.Tables(0)
                dr = dt_UserRights.NewRow()
                dr("nWorkspaceWorkflowUserId") = workspaceflowid
                dr("iNodeId") = iNodeId
            End If


            dr("vWorkspaceId") = vWorkspaceId
            dr("iUserId") = iUserId

            dr("iModifyBy") = HttpContext.Current.Session(S_UserID)
            dr("iStageId") = iStageId
            dr("cStatusIndi") = "D"

            dt_UserRights.Rows.Add(dr)


            If dt_UserRights.Rows.Count > 0 Then

                Ds_UserRights = New DataSet
                dt_UserRights.TableName = "View_WorkspaceDefaultWorkFlowUserDtl"
                If vtablename = "WorkspaceWorkflowUserDtl" Then
                    dt_UserRights.TableName = "WorkspaceWorkFlowUserDtl"
                End If
                Ds_UserRights.Tables.Add(dt_UserRights.Copy())

                If vtablename = "WorkspaceDefaultWorkflowUserDtl" Then

                    If Not objLambda.Save_WorkspaceDefaultWorkFlowUserDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                         Ds_UserRights, HttpContext.Current.Session(S_UserID), estr) Then

                        ''ObjCommon.ShowAlert("Error While Saving WorkspaceDefaultWorkFlowUserDtl", )
                        Exit Function

                    End If

                ElseIf vtablename = "WorkspaceWorkflowUserDtl" Then

                    If Not objLambda.Save_InsertWorkspaceWorkflowUserDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                         Ds_UserRights, "", HttpContext.Current.Session(S_UserID), estr) Then

                        'ObjCommon.ShowAlert("Error While Saving WorkspaceWorkFlowUserDtl",)
                        Exit Function

                    End If

                End If

                'strmsg = "Record Updated SuccessFully"
                'If e.CommandName.ToUpper = "DELETE" Then
                '    strmsg = "Record Deleted SuccessFully"
                'End If

                'ObjCommon.ShowAlert(strmsg, Me.Page)
                'Me.DDLUserprofile.SelectedIndex = 0
                'FillGrid()
                'Page_Load(sender, e)



            End If


            'strReturn = JsonConvert.SerializeObject(ds)

            'Return strReturn
        Catch ex As Exception

            Return strReturn
        End Try

    End Function

    <WebMethod> _
    Public Shared Function FilluserWeb(ByVal wstr As String, ByVal Type As String, ByVal WorkSpaceID As String) As String
        Dim Ds_User As New Data.DataSet
        Dim Ds_Stage As New Data.DataSet
        Dim ds_ScopeMst As New Data.DataSet ' added by vishal
        Dim Dv_User As New Data.DataView
        Dim estr As String = String.Empty
        Dim Str_ProjectType As String = String.Empty
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim ds_scopevalue As New DataSet
        Dim deptcode As String = HttpContext.Current.Session(S_DeptCode.ToString())
        Dim strReturn As String = String.Empty

        Try

            If (Type.ToString() = "DUTYDELEGATION") Then
                wstr = wstr + " and vDeptCode = '" + deptcode + "'"
            End If

            'If Me.DdlLocation.SelectedIndex > 0 Then
            '    'wstr += wstr + " and vLocationCode = '" & Me.DdlLocation.SelectedValue.Trim() & "'"
            'End If

            If Not objHelp.GetViewUserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_scopevalue, estr) Then
                'Me.ObjCommon.ShowAlert("Error While getting Data from UserMst!", Me.Page)
                Return False

            End If

            strReturn = JsonConvert.SerializeObject(ds_scopevalue)
            Return strReturn
        Catch ex As Exception
            'Me.ShowErrorMessage(ex.Message, "...FillGrid")
            Return strReturn
        End Try

    End Function

    <WebMethod> _
    Public Shared Function FillDropDownUserProfileWEB(ByVal wstr As String, ByVal Type As String, ByVal WorkSpaceID As String) As String
        Dim dv_UserProfile As New DataView
        Dim estr As String = String.Empty
        Dim deptcode As String = HttpContext.Current.Session(S_DeptCode.ToString())
        Dim strReturn As String = String.Empty
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim ds_UserProfile As New Data.DataSet

        Try
            wstr = wstr + " And vWorkSpaceID = '" + WorkSpaceID + "'"

            If (Type.ToString() = "DUTYDELEGATION") Then
                wstr = wstr + " and vDeptCode = '" + deptcode + "'"
            End If



            'If Me.DdlLocation.SelectedIndex > 0 Then
            '    'wstr += wstr + " and vLocationCode = '" & Me.DdlLocation.SelectedValue.Trim() & "'"
            'End If

            If Not objHelp.View_WorkspaceDefaultWorkflowUserDtl_New(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                   ds_UserProfile, estr) Then
                Exit Function
            End If

            strReturn = JsonConvert.SerializeObject(ds_UserProfile)
            Return strReturn
        Catch ex As Exception
            'Me.ShowErrorMessage(ex.Message, "...FillGrid")
            Return strReturn
        End Try

    End Function

#End Region

End Class
