
Partial Class frmDefaultDocumentUserRights
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Private Const VS_Choice As String = "Choice"
    Private Const VS_TemplateId As String = "TemplateId"
    Private Const VS_TemplateName As String = "TemplateName"
    Private Const VS_DtTemplateDefaultWorkFlowUserDtl As String = "DtTemplateDefaultWorkFlowUserDtl"
    Private Const VS_StageMst As String = "DtStageMst"
    Private Const VS_Grid As String = "DtGrid"

    Private Const GVC_Delete As Integer = 0
    Private Const GVC_TemplateDefaultWorkflowUserId As Integer = 1
    Private Const GVC_TemplateId As Integer = 2
    Private Const GVC_TemplateDesc As Integer = 3
    Private Const GVC_UserId As Integer = 4
    Private Const GVC_UserName As Integer = 5
    Private Const GVC_UserTypeName As Integer = 6
    Private Const GVC_StageId As Integer = 7
    Private Const GVC_StageDesc As Integer = 8

#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GenCall()
        End If
    End Sub

#End Region

#Region "GenCall "
    Private Function GenCall() As Boolean
        Dim ds As New DataSet

        Try


            Me.ViewState(VS_Choice) = Me.Request.QueryString("mode").ToString.Trim()
            Me.ViewState(VS_TemplateId) = Me.Request.QueryString("TemplateId").ToString.Trim()

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtTemplateDefaultWorkFlowUserDtl) = ds.Tables("View_TemplateDefaultWorkFlowUserDtl")   ' adding blank DataTable in viewstate

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
                wStr = "vTemplateId='" & Me.ViewState(VS_TemplateId).ToString.Trim() & "'"
            End If


            If Not objHelp.View_TemplateDefaultWorkflowUserDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            If ds.Tables(0).Rows.Count <= 0 And _
             Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found For Selected Operation")
            End If

            ds_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_Data")

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


            Page.Title = " :: Template Default Document User Rights ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Default Template Document User Rights"

            Me.lblInformation.Text = "<br/>Template: " & Me.Request.QueryString("TemplateName").ToString.Trim() & "<br/>"

            dt_OpMst = Me.ViewState(VS_DtTemplateDefaultWorkFlowUserDtl)

            Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit 'Always in Edit Mode

            If Not FillDropDown() Then
                Exit Function
            End If

            '*************************

            If Not FillGrid() Then
                Exit Function
            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......GenCall_ShowUI")
        Finally
        End Try
    End Function

#End Region

#Region "FillDropDown"

    Private Function FillDropDown() As Boolean
        Dim ds_UserGroup As New Data.DataSet
        Dim Ds_Template As New Data.DataSet
        Dim Ds_Node As New Data.DataSet
        Dim ds_UserType As New Data.DataSet
        Dim ds_Location As New Data.DataSet
        Dim Ds_Stage As New Data.DataSet
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty

        Try



            If Not Me.objHelp.getuserGroupMst("cStatusIndi <> 'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_UserGroup, estr) Then

                Me.ObjCommon.ShowAlert("Error While getting Data from UserGroupMst", Me.Page)
                Return False

            End If

            Me.DDLUserGroup.DataSource = ds_UserGroup
            Me.DDLUserGroup.DataValueField = "iUserGroupCode"
            Me.DDLUserGroup.DataTextField = "vUserGroupName"
            Me.DDLUserGroup.DataBind()
            Me.DDLUserGroup.Items.Insert(0, New ListItem("--select User Group--", 0))
            Me.DDLUserGroup.SelectedIndex = 0

            If Not Me.objHelp.getUserTypeMst("cStatusIndi <> 'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                   ds_UserType, estr) Then

                Me.ObjCommon.ShowAlert("Error While getting Data From UserTypeMst", Me.Page)
                Return False

            End If

            Me.DdlUserType.DataSource = ds_UserType
            Me.DdlUserType.DataValueField = "vUserTypeCode"
            Me.DdlUserType.DataTextField = "vUserTypeName"
            Me.DdlUserType.DataBind()
            Me.DdlUserType.Items.Insert(0, New ListItem("--select User Type--", 0))
            Me.DdlUserType.SelectedIndex = 0


            'To fill location dropdown : Start

            If Not Me.objHelp.getLocationMst("cStatusIndi <> 'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                   ds_Location, estr) Then

                Me.ObjCommon.ShowAlert("Error While Getting Data From LocationMst", Me.Page)
                Return False

            End If

            Me.DdlLocation.DataSource = ds_Location
            Me.DdlLocation.DataValueField = "vLocationCode"
            Me.DdlLocation.DataTextField = "vLocationName"
            Me.DdlLocation.DataBind()
            Me.DdlLocation.Items.Insert(0, New ListItem("--All Locations--", 0))
            Me.DdlLocation.SelectedIndex = 0
            'To fill location dropdown : End


            If Not Me.objHelp.GetStageMst("cStatusIndi <> 'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                      Ds_Stage, estr) Then

                Me.ObjCommon.ShowAlert("Error While Getting Stages", Me.Page)
                Exit Function

            End If

            Me.ViewState(VS_StageMst) = Ds_Stage.Tables(0)

            Me.chklstStages.DataSource = Ds_Stage.Tables(0)
            Me.chklstStages.DataValueField = "istageid"
            Me.chklstStages.DataTextField = "vStageDesc"
            Me.chklstStages.DataBind()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillDropDown")
            Return False
        End Try
    End Function

    Private Function fillUser() As Boolean
        Dim Ds_User As New Data.DataSet
        Dim Ds_Stage As New Data.DataSet
        Dim Dv_User As New Data.DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Try

            If Me.DDLUserGroup.SelectedIndex = 0 Then
                Return False
            End If

            'Add filter of location if selected any :Start
            wstr = "iUserGroupCode= '" & Me.DDLUserGroup.SelectedValue.Trim() & "'" & _
                                            " And vUserTypeCode= '" & Me.DdlUserType.SelectedValue.Trim() & "'" & " And cStatusindi<>'D'"
            ' cStatusindi<>'D' added by Jeet Patel on 24-Apr-2015 to get only Active User 
            ' " And nScopeNo=" & Me.Session(S_ScopeNo)

            If Me.DdlLocation.SelectedIndex > 0 Then
                wstr += " And vLocationCode = '" & Me.DdlLocation.SelectedValue.Trim() & "'"
            End If
            'Add filter of location if selected any :End

            If Not Me.objHelp.GetViewUserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_User, estr) Then

                Me.ObjCommon.ShowAlert("Error While getting Data From UserMst", Me.Page)
                Return False

            End If

            Dv_User = Ds_User.Tables(0).DefaultView
            Dv_User.Sort = "vUserName"

            Me.chklstUser.DataSource = Dv_User
            Me.chklstUser.DataValueField = "iUserId"
            Me.chklstUser.DataTextField = "vUserName"
            Me.chklstUser.DataBind()


            Return True
        Catch ex As Exception
            Return True
        End Try
    End Function

    Private Function FillGrid() As Boolean
        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim ds_Grid As New DataSet

        Try

            wStr = "cStatusIndi <> 'D' And vTemplateId='" & Me.ViewState(VS_TemplateId).ToString.Trim() & "'"

            If Not objHelp.View_TemplateDefaultWorkflowUserDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_Grid, eStr_Retu) Then

                Response.Write(eStr_Retu)
                Exit Function

            End If

            Me.ViewState(VS_Grid) = ds_Grid.Tables(0)

            If ds_Grid.Tables(0).Rows.Count() <= 0 Then
                Me.BtnDelete.Visible = False
                'Return True
            End If

            Me.GV_UserStage_Edit.DataSource = ds_Grid.Tables(0)
            Me.GV_UserStage_Edit.DataBind()

            Me.BtnDelete.Visible = True



            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "SelectedIndexChanged"

    Protected Sub DDLUserGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLUserGroup.SelectedIndexChanged
        fillUser()
    End Sub

    Protected Sub DdlUserType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        fillUser()
    End Sub

    Protected Sub DdlLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        fillUser()
    End Sub
#End Region

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

            If Not AssignValues(Str_Worn) Then
                Me.ObjCommon.ShowAlert("Error While Assinging Data", Me.Page())
                Exit Sub
            End If

            'If Str_Worn.Trim() <> "" Then
            'Me.ObjCommon.ShowAlert(Str_Worn, Me.Page)
            'End If

            dt_Save = CType(Me.ViewState(VS_DtTemplateDefaultWorkFlowUserDtl), DataTable)
            If dt_Save.Rows.Count > 0 Then

                ds_Save = New DataSet
                dt_Save.TableName = "View_TemplateDefaultWorkFlowUserDtl"
                ds_Save.Tables.Add(dt_Save.Copy())

                If Not objLambda.Save_TemplateDefaultWorkFlowUserDtl(Me.ViewState(VS_Choice), ds_Save, Me.Session(S_UserID), estr) Then
                    Me.ObjCommon.ShowAlert("Error While Saving TemplateDefaultWorkFlowUserDtl", Me.Page)
                    Exit Sub

                End If

            End If

            ObjCommon.ShowAlert("Record Saved SuccessFully ! \n" + Str_Worn, Me.Page)
            ResetPage()
            'FillGrid()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....BtnSave_Click")
        End Try
    End Sub

    Private Function AssignValues(ByRef Str_Worn As String) As Boolean
        Dim dr1 As DataRow
        Dim Index1 As Integer
        Dim Index2 As Integer
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

        Try

            'Retrive ActivityId from TemplateNodeDetail
            dt_UserRights = CType(Me.ViewState(VS_DtTemplateDefaultWorkFlowUserDtl), DataTable)
            dt_UserRights.Clear()

            dt_Grid = CType(Me.ViewState(VS_Grid), DataTable)


            'dt_UserRights.Clear()
            For Index1 = 0 To Me.chklstUser.Items.Count - 1

                If Me.chklstUser.Items(Index1).Selected Then

                    For Index2 = 0 To Me.chklstStages.Items.Count - 1

                        If Me.chklstStages.Items(Index2).Selected Then

                            Duplicate = False

                            For Index3 = 0 To dt_Grid.Rows.Count - 1

                                If dt_Grid.Rows(Index3).Item("iUserId") = Me.chklstUser.Items(Index1).Value And _
                                    dt_Grid.Rows(Index3).Item("iStageId") = Me.chklstStages.Items(Index2).Value And _
                                    dt_Grid.Rows(Index3).Item("cStatusIndi") <> "D" Then

                                    Str_Worn += "\nUser '" & Me.chklstUser.Items(Index1).Text & "' For Stage '" & _
                                                         Me.chklstStages.Items(Index2).Text & "' Is Already Added !"

                                    Duplicate = True
                                    Exit For
                                End If

                            Next Index3

                            If Duplicate = True Then
                                Exit For
                            End If

                            dr1 = dt_UserRights.NewRow()
                            dr1("nTemplateDefaultWorkflowUserId") = dt_UserRights.Rows.Count + 1
                            dr1("vTemplateId") = Me.ViewState(VS_TemplateId)
                            dr1("vTemplateDesc") = Me.ViewState(VS_TemplateName)
                            dr1("iUserId") = Me.chklstUser.Items(Index1).Value
                            dr1("vUserName") = Me.chklstUser.Items(Index1).Text
                            dr1("iStageId") = Me.chklstStages.Items(Index2).Value
                            dr1("vStageDesc") = Me.chklstStages.Items(Index2).Text
                            dr1("cStatusIndi") = "N"
                            dt_UserRights.Rows.Add(dr1)



                        End If
                    Next Index2

                End If

            Next Index1

            Me.ViewState(VS_DtTemplateDefaultWorkFlowUserDtl) = dt_UserRights

            AssignValues = True


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message(), "......AssignValues")
            AssignValues = False
        End Try
    End Function

#End Region

#Region "Button Click"

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Me.Response.Redirect("frmAddTemplateMst.aspx?mode=1")
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

        Try



            dt_UserRights = CType(Me.ViewState(VS_DtTemplateDefaultWorkFlowUserDtl), DataTable).Copy()

            For index = 0 To Me.GV_UserStage_Edit.Rows.Count - 1

                chk = CType(Me.GV_UserStage_Edit.Rows(index).FindControl("CHKDelete"), CheckBox)

                If chk.Checked = True Then

                    dr = dt_UserRights.NewRow()
                    dr("nTemplateDefaultWorkflowUserId") = Me.GV_UserStage_Edit.Rows(index).Cells(GVC_TemplateDefaultWorkflowUserId).Text
                    dr("vTemplateId") = Me.GV_UserStage_Edit.Rows(index).Cells(GVC_TemplateId).Text
                    dr("vTemplateDesc") = Me.GV_UserStage_Edit.Rows(index).Cells(GVC_TemplateDesc).Text
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
                dt_UserRights.TableName = "View_TemplateDefaultWorkFlowUserDtl"
                ds_Delete.Tables.Add(dt_UserRights.Copy())

                If Not objLambda.Save_TemplateDefaultWorkFlowUserDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                        ds_Delete, Me.Session(S_UserID), estr) Then

                    ObjCommon.ShowAlert("Error While Saving TemplateDefaultWorkFlowUserDtl", Me.Page)
                    Exit Sub

                End If

                ObjCommon.ShowAlert("Record Deleted SuccessFully", Me.Page)
                FillGrid()

            End If


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".......BtnDelete_Click")
        End Try

    End Sub

#End Region

#Region "Grid Event"

    Protected Sub GV_UserStage_Edit_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_UserStage_Edit.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            CType(e.Row.FindControl("ImgEdit"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImgEdit"), ImageButton).CommandName = "EDIT"

            CType(e.Row.FindControl("ImgSave"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImgSave"), ImageButton).CommandName = "UPDATE"

            CType(e.Row.FindControl("ImgCancel"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImgCancel"), ImageButton).CommandName = "CANCEL"

            CType(e.Row.FindControl("ImgDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImgDelete"), ImageButton).CommandName = "DELETE"

            CType(e.Row.FindControl("DDLStages"), DropDownList).Enabled = False
            CType(e.Row.FindControl("ImgSave"), ImageButton).Visible = False
            CType(e.Row.FindControl("ImgCancel"), ImageButton).Visible = False
            CType(e.Row.FindControl("ImgEdit"), ImageButton).Visible = True

        End If

    End Sub

    Protected Sub GV_UserStage_Edit_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GV_UserStage_Edit.RowCommand
        Dim index As Integer = e.CommandArgument
        Dim dt_UserRights As New DataTable
        Dim Ds_UserRights As New DataSet
        Dim dt_NewGrid As New DataTable
        Dim dv_Grid As New DataView
        Dim index3 As New Integer
        Dim dr As DataRow
        Dim estr As String = String.Empty
        Dim strmsg As String = String.Empty
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

            dt_UserRights = CType(Me.ViewState(VS_DtTemplateDefaultWorkFlowUserDtl), DataTable).Copy()

            dr = dt_UserRights.NewRow()
            dr("nTemplateDefaultWorkflowUserId") = Me.GV_UserStage_Edit.Rows(index).Cells(GVC_TemplateDefaultWorkflowUserId).Text
            dr("vTemplateId") = Me.GV_UserStage_Edit.Rows(index).Cells(GVC_TemplateId).Text
            dr("vTemplateDesc") = Me.GV_UserStage_Edit.Rows(index).Cells(GVC_TemplateDesc).Text
            dr("iUserId") = Me.GV_UserStage_Edit.Rows(index).Cells(GVC_UserId).Text
            dr("vUserName") = Me.GV_UserStage_Edit.Rows(index).Cells(GVC_UserName).Text

            'For Edit
            If e.CommandName.ToUpper = "UPDATE" Then
                dr("iStageId") = CType(Me.GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).SelectedValue.Trim()
                dr("vStageDesc") = CType(Me.GV_UserStage_Edit.Rows(index).FindControl("DDLStages"), DropDownList).SelectedItem.Text.Trim()
                dr("cStatusIndi") = "E"

                'For Delete
            ElseIf e.CommandName.ToUpper = "DELETE" Then
                dr("iStageId") = Me.GV_UserStage_Edit.Rows(index).Cells(GVC_StageId).Text
                dr("vStageDesc") = Me.GV_UserStage_Edit.Rows(index).Cells(GVC_StageDesc).Text
                dr("cStatusIndi") = "D"
            End If

            dt_UserRights.Rows.Add(dr)

            If dt_UserRights.Rows.Count > 0 Then

                Ds_UserRights = New DataSet
                dt_UserRights.TableName = "View_TemplateDefaultWorkFlowUserDtl"
                Ds_UserRights.Tables.Add(dt_UserRights.Copy())

                If Not objLambda.Save_TemplateDefaultWorkFlowUserDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                     Ds_UserRights, Me.Session(S_UserID), estr) Then

                    ObjCommon.ShowAlert("Error While Saving TemplateDefaultWorkFlowUserDtl", Me.Page)
                    Exit Sub
                End If

                strmsg = "Record Updated SuccessFully"
                If e.CommandName.ToUpper = "DELETE" Then
                    strmsg = "Record Deleted SuccessFully"
                End If

                ObjCommon.ShowAlert(strmsg, Me.Page)
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

    Protected Sub GV_UserStage_Edit_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GV_UserStage_Edit.RowEditing
        e.Cancel = True
    End Sub

    Protected Sub GV_UserStage_Edit_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GV_UserStage_Edit.RowUpdating

    End Sub

    Protected Sub GV_UserStage_Edit_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_UserStage_Edit.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or _
                e.Row.RowType = DataControlRowType.Header Or _
                e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVC_TemplateDefaultWorkflowUserId).Visible = False
            e.Row.Cells(GVC_TemplateId).Visible = False
            e.Row.Cells(GVC_UserId).Visible = False
            e.Row.Cells(GVC_StageId).Visible = False

        End If
    End Sub

    Protected Sub GV_UserStage_Edit_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GV_UserStage_Edit.PageIndexChanging
        GV_UserStage_Edit.PageIndex = e.NewPageIndex
        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub

    Protected Sub GV_UserStage_Edit_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GV_UserStage_Edit.RowCancelingEdit
        e.Cancel = True
    End Sub

    Protected Sub GV_UserStage_Edit_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GV_UserStage_Edit.RowDeleting

    End Sub

#End Region

#Region "ResetPage"
    Private Sub ResetPage()
        'Me.chklstUser.SelectedIndex = -1
        Me.chklstUser.Items.Clear()
        Me.DDLUserGroup.SelectedIndex = 0
        Me.GV_UserStage_Edit.DataSource = Nothing
        Me.GV_UserStage_Edit.DataBind()

        If Not GenCall() Then
            Exit Sub
        End If

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

End Class
