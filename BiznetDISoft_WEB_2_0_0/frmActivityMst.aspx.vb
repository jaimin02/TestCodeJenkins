Imports System.Data
Imports System.Web.Services
Imports Newtonsoft.Json

Partial Class frmAddActivityMst
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objOPws As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()


    'Private Const VS_ActivityId As String = "vActivityId"
    Private Const VS_Choice As String = "Choice"
    Private Const VS_ActivityMst As String = "dtActivityMst"
    Private Const VS_ActivityDocLinkMatrix As String = "dtActivityDocLinkMatrix"
    Private Const VS_ActivityMedExTemplateDtl As String = "dtActivityMedExTemplateDtl"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_ActivityNAme As Integer = 1
    Private Const GVC_ActivityGroupNAme As Integer = 2
    Private Const GVC_DocName As Integer = 3
    Private Const GVC_Subjectwiseflag As Integer = 4
    Private Const GVC_DeptName As Integer = 5
    Private Const GVC_PeriodSpecific As Integer = 6
    Private Const GVC_Milestone As Integer = 7
    Private Const GVC_CanStartAfter As Integer = 8
    'Private Const GVC_ActId As Integer = 10
    Private Const GVC_Edit As Integer = 10
    Private Const GVC_Delete As Integer = 11
    Private Const GVC_ActivityId As Integer = 12
    Private Const GVC_DocCode As Integer = 13

    Private Const GVActC_ActDocLinkNo As Integer = 0
    Private Const GVActC_SrNo As Integer = 1
    Private Const GVActC_DocTypeCode As Integer = 2
    Private Const GVActC_DocType As Integer = 3
    Private Const GVActC_ProjId As Integer = 4
    Private Const GVActC_Proj As Integer = 5
    Private Const GVActC_NodeId As Integer = 6
    Private Const GVActC_ActId As Integer = 7
    Private Const GVActC_Act As Integer = 8

#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not GenCall() Then
                Exit Sub
            End If
        End If
        Try
            If gvactivity.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvactivity", "UIgvactivity(); ", True)
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "GenCall"
    Private Function GenCall() As Boolean
        Dim dt_ActivityMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim ActivityId As String = String.Empty

        Try
            Choice = Me.Request.QueryString("mode")
            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                'Me.ViewState(VS_ActivityId) = Me.Request.QueryString("value").ToString
                ActivityId = Me.Request.QueryString("value").ToString
            End If

            ''''Check for Valid User''''''''''''''
            If Not GenCall_Data(Choice, dt_ActivityMst, ActivityId) Then ' For Data Retrieval
                Return False
            End If

            Me.ViewState(VS_ActivityMst) = dt_ActivityMst ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI(Choice, dt_ActivityMst, ActivityId) Then 'For Displaying Data 
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
            ShowErrorMessage(ex.Message, "...GenCall")
            Return False
        Finally
        End Try
    End Function
#End Region

#Region "GenCall_Data"
    Private Function GenCall_Data(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                            ByRef dt_Dist_Retu As DataTable, ByVal ActivityId As String) As Boolean

        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds As DataSet = Nothing
        Dim ds_ActivityMedExTemplateDtl As New DataSet
        Try

            If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vActivityId=" + ActivityId 'Value of where condition
            End If

            If Not objHelp.getActivityMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr) Then
                Throw New Exception(eStr)
            End If

            'If ds Is Nothing Then
            '    Throw New Exception(eStr)
            'End If

            If ds.Tables(0).Rows.Count <= 0 And _
               Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found for Selected role")
            End If

            dt_Dist_Retu = ds.Tables(0)
            If Not objHelp.GetActivityMedExTemplateDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                        ds_ActivityMedExTemplateDtl, eStr) Then
                Throw New Exception(eStr)
            End If
            Me.ViewState(VS_ActivityMedExTemplateDtl) = ds_ActivityMedExTemplateDtl.Tables(0)

            GenCall_Data = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "GenCall_Data")
            Return False
        Finally
        End Try
    End Function
#End Region

#Region "GenCall_ShowUI"
    Private Function GenCall_ShowUI(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                                      ByVal dt_Dist As DataTable, ByVal ActivityId As String) As Boolean
        Dim dt_ActivityMst As DataTable = Nothing
        Dim indexchklst As Integer
        Dim indexArr As Integer
        Dim CanstartAfter As String = String.Empty
        Dim estr As String = String.Empty
        Dim ArrCanstartAfter() As String
        Dim dsActivityDocLink As New DataSet
        Dim dsActivityMedExTemplateDtl As New DataSet

        Try
            'Page.Title = " :: Activity/Node Master :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            'CType(Master.FindControl("lblHeading"), Label).Text = "Activity/Node Master"

            Page.Title = " :: Activity Template Management :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Master.FindControl("lblHeading"), Label).Text = "Activity Template Management"

            dt_ActivityMst = Me.ViewState(VS_ActivityMst)

            If Not FillControls() Then
                Return False
            End If

            If Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.txtactivityname.Text = ConvertDbNullToDbTypeDefaultValue(dt_ActivityMst.Rows(0)("vActivityName"), dt_ActivityMst.Rows(0)("vActivityName").GetType)
                Me.ddldoctypecode.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_ActivityMst.Rows(0)("vDocTypeCode"), dt_ActivityMst.Rows(0)("vDocTypeCode").GetType)
                Me.ddlsubjectwiseflag.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_ActivityMst.Rows(0)("cSubjectWiseFlag"), dt_ActivityMst.Rows(0)("cSubjectWiseFlag").GetType)
                Me.ddldeptcode.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_ActivityMst.Rows(0)("vDeptCode"), dt_ActivityMst.Rows(0)("vDeptCode").GetType)
                Me.txtdays.Text = ConvertDbNullToDbTypeDefaultValue(dt_ActivityMst.Rows(0)("ncompletiondays"), dt_ActivityMst.Rows(0)("ncompletiondays").GetType)
                Me.ddlActivityGroup.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_ActivityMst.Rows(0)("vActivityGroupId"), dt_ActivityMst.Rows(0)("vActivityGroupId").GetType)

                Me.RBLPeriod.SelectedValue = "N"
                If Not dt_ActivityMst.Rows(0)("cPeriodSpecific") Is System.DBNull.Value Then
                    Me.RBLPeriod.SelectedValue = dt_ActivityMst.Rows(0)("cPeriodSpecific")
                End If

                Me.rbtnlstIsRepeatable.SelectedValue = "N"
                If Not dt_ActivityMst.Rows(0)("cIsRepeatable") Is System.DBNull.Value Then
                    Me.rbtnlstIsRepeatable.SelectedValue = dt_ActivityMst.Rows(0)("cIsRepeatable")
                End If
                'added on 25-10-10
                'Removed on 16-02-2011 -> Pratiksha
                'If Not dt_ActivityMst.Rows(0)("cActivityType") Is System.DBNull.Value Then
                '    Me.ddlActivityType.SelectedValue = dt_ActivityMst.Rows(0)("cActivityType")
                'End If
                '========
                '========
                Me.ddlMileStone.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_ActivityMst.Rows(0)("nMilestone"), dt_ActivityMst.Rows(0)("nMilestone").GetType)
                CanstartAfter = dt_ActivityMst.Rows(0)("vCanStartAfter")
                ArrCanstartAfter = CanstartAfter.Split(",")

                For indexchklst = 0 To chklstCanstart.Items.Count - 1
                    For indexArr = 0 To ArrCanstartAfter.Length - 1
                        If chklstCanstart.Items(indexchklst).Value = ArrCanstartAfter(indexArr).ToString.Trim() Then
                            Me.chklstCanstart.Items(indexchklst).Selected = True
                        End If
                    Next indexArr
                Next indexchklst

                'For ActivityDocLinkMatrix
                If Not Me.objHelp.getActivityDocLinkMatrix("vActivityId='" & ActivityId & "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsActivityDocLink, estr) Then
                    Me.objcommon.ShowAlert("Error while getting data from ActivityDocLinkMatrix: " + estr, Me.Page)
                    Return False
                End If

                Me.btnSave.Text = "Update"
                Me.btnSave.ToolTip = "Update"
                Me.ViewState(VS_ActivityDocLinkMatrix) = dsActivityDocLink.Tables(0)
                BindActivityDocLinkMatrix()
                '****************************

                'For ActivityMedExTemplateDtl
                If Not Me.objHelp.GetActivityMedExTemplateDtl("vActivityId='" & ActivityId & "' and cStatusIndi<>'" + Status_Delete + "'", _
                        WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                        dsActivityMedExTemplateDtl, estr) Then
                    Me.objcommon.ShowAlert("Error while getting data from ActivityMedExTemplateDtl: " + estr, Me.Page)
                    Return False
                End If

                Me.ViewState(VS_ActivityMedExTemplateDtl) = dsActivityMedExTemplateDtl.Tables(0)
                If dsActivityMedExTemplateDtl.Tables(0).Rows.Count > 0 Then
                    Me.ddlMedExTemplate.SelectedValue = dsActivityMedExTemplateDtl.Tables(0).Rows(0).Item("vMedExTemplateId")
                End If
                '****************************
            End If

            'Me.btnSave.Attributes.Add("OnClick", "return Validation();")
            'Me.txtdays.Attributes.Add("onblur", "return Numeric();")

            GenCall_ShowUI = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "GenCall_ShowUI")
            GenCall_ShowUI = False
        End Try
    End Function
#End Region

#Region "Reset Page"
    Private Sub ResetPage()
        Me.txtactivityname.Text = String.Empty
        Me.ddldoctypecode.SelectedIndex = 0
        Me.ddlsubjectwiseflag.SelectedIndex = 0
        Me.ddldeptcode.SelectedIndex = 0
        Me.ddlActivityGroup.SelectedIndex = 0
        Me.ddlMileStone.SelectedIndex = 0
        Me.txtdays.Text = String.Empty
        Me.RBLPeriod.SelectedValue = "N"
        Me.rbtnlstIsRepeatable.SelectedValue = "N"
        Me.chklstCanstart.ClearSelection()
        Me.ViewState(VS_ActivityMst) = Nothing
        Me.ViewState(VS_ActivityDocLinkMatrix) = Nothing
        Me.ViewState(VS_ActivityMedExTemplateDtl) = Nothing
        'Response.Redirect("frmActivityMst.aspx?mode=1", False)
    End Sub
#End Region

#Region "Fill Controls"
    Private Function FillControls() As Boolean
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim Wstr1 As String = String.Empty
        Dim ds As New DataSet
        Dim ds_ActivityGroup As New DataSet
        Dim ds_Activity As New DataSet
        Dim dv_Activity As New DataView
        Dim ds_type As New DataSet
        Dim ds_dept As New DataSet
        Dim ds_subject As New DataSet
        Dim ds_start As New DataSet
        Dim dsProjects As New DataSet
        Dim dv_ActivityGroup As New DataView
        Dim dv_DocTypeName As New DataView
        Dim dv_DeptName As New DataView

        Dim ds_MedExTemplate As New DataSet
        Dim dv_MedExTemplate As New DataView

        Try
            'To Get Where condition of ScopeVales( Project Type )
            If Not objcommon.GetScopeValueWithCondition(Wstr) Then
                Return False
            End If

            If Not objHelp.GetviewActivityGroupMst(Wstr + " And cStatusIndi <> '" + Status_Delete + "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ActivityGroup, estr) Then
                ShowErrorMessage("Error", estr)
                Return False
            End If

            dv_ActivityGroup = ds_ActivityGroup.Tables(0).DefaultView.ToTable(True, "vActivityGroupName,vActivityGroupId".Split(",")).DefaultView
            dv_ActivityGroup.Sort = "vActivityGroupName"
            Me.ddlActivityGroup.DataSource = dv_ActivityGroup
            Me.ddlActivityGroup.DataTextField = "vActivityGroupName"
            Me.ddlActivityGroup.DataValueField = "vActivityGroupId"
            Me.ddlActivityGroup.DataBind()
            Me.ddlActivityGroup.Items.Insert(0, New ListItem("Select Activity Group", ""))
            'added on 27-11-09---Bydeepak singh
            Wstr1 = Wstr & "and cstatusindi <>'" + Status_Delete + "'"

            If Not objHelp.GetActivityCodeDetails(ds, Wstr1, estr) Then
                objcommon.ShowAlert("Error while FillGrid", Me.Page)
            End If
            dv_Activity = ds.Tables(0).DefaultView
            dv_Activity.Sort = "vActivityName"
            Me.chklstCanstart.DataSource = dv_Activity.ToTable()
            Me.chklstCanstart.DataValueField = "vActivityId"
            Me.chklstCanstart.DataTextField = "vActivityName"
            Me.chklstCanstart.DataBind()

            Me.ddlsubjectwiseflag.Items.Insert(0, New ListItem("Yes", "T"))
            Me.ddlsubjectwiseflag.Items.Insert(1, New ListItem("No", "N"))
            Me.ddlsubjectwiseflag.Items.Insert(2, New ListItem("View", "V"))


            objHelp.FillDropDown("doctypemst", "vDocTypeCode", "vDocTypeName", "", ds_type, estr)

            dv_DocTypeName = ds_type.Tables(0).DefaultView
            dv_DocTypeName.Sort = "vDocTypeName"
            Me.ddldoctypecode.DataSource = dv_DocTypeName
            Me.ddldoctypecode.DataValueField = "vDocTypeCode"
            Me.ddldoctypecode.DataTextField = "vDocTypeName"
            Me.ddldoctypecode.DataBind()
            Me.ddldoctypecode.Items.Insert(0, New ListItem("Select Document Type", ""))

            Me.ddlRefDocType.DataSource = dv_DocTypeName
            Me.ddlRefDocType.DataValueField = "vDocTypeCode"
            Me.ddlRefDocType.DataTextField = "vDocTypeName"
            Me.ddlRefDocType.DataBind()
            Me.ddlRefDocType.Items.Insert(0, New ListItem("Select Ref. DocType", ""))

            objHelp.FillDropDown("deptmst", "vDeptCode", "vDeptName", "", ds_dept, estr)
            dv_DeptName = ds_dept.Tables(0).DefaultView
            dv_DeptName.Sort = "vDeptName"
            Me.ddldeptcode.DataSource = dv_DeptName
            Me.ddldeptcode.DataValueField = "vDeptCode"
            Me.ddldeptcode.DataTextField = "vDeptName"
            Me.ddldeptcode.DataBind()
            Me.ddldeptcode.Items.Insert(0, New ListItem("Select Department", ""))


            '  Me.gvactivity.DataSource = ds
            ' Me.gvactivity.DataBind()

            If Not Me.objHelp.getworkspacemst("vProjectTypeCode='" & GeneralModule.ProjectTypeCode_QC & "' order by vWorkSpaceDesc", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        dsProjects, estr) Then
                Me.objcommon.ShowAlert("Error while getting QC projects: " + estr, Me.Page)
                Return False
            End If

            Me.ddlWorkspace.DataSource = dsProjects
            Me.ddlWorkspace.DataTextField = "vWorkSpaceDesc"
            Me.ddlWorkspace.DataValueField = "vWorkSpaceId"
            Me.ddlWorkspace.DataBind()
            Me.ddlWorkspace.Items.Insert(0, New ListItem("Select Project", ""))

            If Not Me.objHelp.GetMedExTemplateDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_MedExTemplate, estr) Then
                Me.objcommon.ShowAlert("Error while getting MedExTemplateDetails : " + estr, Me.Page)
                Return False
            End If

            dv_MedExTemplate = ds_MedExTemplate.Tables(0).DefaultView.ToTable(True, "vTemplateName,vMedExTemplateId".Split(",")).DefaultView
            dv_MedExTemplate.Sort = "vTemplateName"
            Me.ddlMedExTemplate.DataSource = dv_MedExTemplate
            Me.ddlMedExTemplate.DataTextField = "vTemplateName"
            Me.ddlMedExTemplate.DataValueField = "vMedExTemplateId"
            Me.ddlMedExTemplate.DataBind()
            Me.ddlMedExTemplate.Items.Insert(0, New ListItem("Select Attribute Template", ""))

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...FillControls")
            Return False
        End Try
    End Function
#End Region

#Region "DropDown SelectedIndexChanged"

    Protected Sub ddlWorkspace_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWorkspace.SelectedIndexChanged
        Dim dsNodes As New DataSet
        Dim dvNodes As New DataView
        Dim estr As String = String.Empty
        Dim wstrDocApprove As String = String.Empty
        Try


            'Only approved Documents will be there for reference.
            'As Suggested by Nikur Sir.
            wstrDocApprove = "vWorkSpaceId='" + Me.ddlWorkspace.SelectedValue.Trim() + "' And iStageID='" + GeneralModule.Stage_Authorized.Trim() + _
                             "' And crequiredFlag='D'"
            If Not Me.objHelp.GetViewWorkSpaceNodeHistory(wstrDocApprove, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsNodes, estr) Then
                Me.objcommon.ShowAlert("Error while getting data from WorkspaceNodeHistory: " + estr, Me.Page)
                Exit Sub
            End If

            'If Not Me.objHelp.GetViewWorkSpaceNodeDetail("vWorkspaceId='" & Me.ddlWorkspace.SelectedValue.Trim() & "'", _
            '        dsNodes, estr) Then
            '    Me.objcommon.ShowAlert("Error while getting Data from WorkspaceNodeHistory: " + estr, Me.Page)
            '    Exit Sub
            'End If
            '****************************

            dvNodes = dsNodes.Tables(0).DefaultView()
            dvNodes.Sort = "vNodeDisplayName"

            Me.ddlNode.DataSource = dvNodes
            Me.ddlNode.DataTextField = "vNodeDisplayName"
            Me.ddlNode.DataValueField = "iNodeId"
            Me.ddlNode.DataBind()
            Me.ddlNode.Items.Insert(0, New ListItem("Select Node", ""))
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...ddlWorkspace_SelectedIndexChanged")
        End Try
    End Sub

#End Region

#Region "Button Events"

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ResetPage()
        Response.Redirect("frmActivityMst.aspx?mode=1", False)
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("frmMainPage.aspx", False)
    End Sub

    Protected Sub btnViewAllRec_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnViewAllRec.Click
        Dim Wstr_Scope As String = String.Empty
        'To Get Where condition of ScopeVales( Project Type )
        If Not objcommon.GetScopeValueWithCondition(Wstr_Scope) Then
            Exit Sub
        End If

        Wstr_Scope += " And cStatusIndi<>'" + Status_Delete + "'"

        BindGrid(Wstr_Scope)
    End Sub

    Protected Sub btnSearchActivity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchActivity.Click
        Dim Wstr_Scope As String = String.Empty

        If Not objcommon.GetScopeValueWithCondition(Wstr_Scope) Then
            Exit Sub
        End If
        Wstr_Scope += " And cStatusIndi <>'" + Status_Delete + "' "
        BindGrid(Wstr_Scope & " And vActivityId='" & Me.HActivityId.Value.Trim() & "'")
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click
        Try

            AssignActDocLinkValues()
            BindActivityDocLinkMatrix()
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...BtnAdd_Click")
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim Dt As New DataTable
        Dim Ds As DataSet
        Dim Ds_Grid As New DataSet
        Dim Ds_activitycodegrid As New DataSet
        Dim eStr As String = String.Empty
        Dim ds_type As New Data.DataSet
        Dim message As String = String.Empty

        Try


            If Not AssignUpdatedValues("Edit") Then
                Exit Sub
            End If
            Ds = New DataSet
            Ds.Tables.Add(CType(Me.ViewState(VS_ActivityMst), Data.DataTable).Copy())
            Ds.Tables(0).TableName = "activityMst"   ' New Values on the form to be updated

            If Not IsNothing(Me.ViewState(VS_ActivityMedExTemplateDtl)) Then
                If CType(Me.ViewState(VS_ActivityMedExTemplateDtl), Data.DataTable).Copy().Rows.Count > 0 Then
                    Ds.Tables.Add(CType(Me.ViewState(VS_ActivityMedExTemplateDtl), Data.DataTable).Copy())
                    Ds.Tables(Ds.Tables.Count - 1).TableName = "ActivityMedExTemplateDtl"   ' New Values on the form to be updated
                End If
            End If

            If Not IsNothing(Me.ViewState(VS_ActivityDocLinkMatrix)) Then
                If CType(Me.ViewState(VS_ActivityDocLinkMatrix), Data.DataTable).Copy().Rows.Count > 0 Then
                    Ds.Tables.Add(CType(Me.ViewState(VS_ActivityDocLinkMatrix), Data.DataTable).Copy())
                    Ds.Tables(Ds.Tables.Count - 1).TableName = "ActivityDocLinkMatrix"   ' New Values on the form to be updated
                End If
            End If

            If Not objOPws.Save_InsertActivityMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_activitymst, _
                                Ds, "1", eStr) Then
                objcommon.ShowAlert("Error while saving record: " & eStr, Me.Page)
                Exit Sub
            End If

            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "Record saved successfully !", "Record Updated successfully !")
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Alertpppppp", "ShowAlert('" + message + "')", True)

            ' objcommon.ShowAlert("Record saved successfully !", Me)
            ResetPage()
            '     Response.Redirect("frmActivityMst.aspx?mode=1", False)
        Catch ex As System.Threading.ThreadAbortException
        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr + "...btnSave_Click")
        End Try
    End Sub

#End Region

#Region "AssignUpdatedValues"

    Private Function AssignUpdatedValues(ByVal Type As String) As Boolean
        Dim estr As String = String.Empty
        Dim dtOld As New DataTable
        Dim dr As DataRow
        Dim CanStartAfterDetails As String = String.Empty
        Dim i As Integer
        Dim ds_Check As New DataSet
        Dim dtActivityMedExTemplateDtl As New DataTable
        Dim Wstr As String = String.Empty
        Dim ArrCanStart() As String
        Dim ds_Save As New DataSet
        Dim Wstr_Scope As String = String.Empty
        Try

            dtOld = Me.ViewState(VS_ActivityMst)
            '=====added on 18-11-09==By Deepak singh======
            If Type = "Edit" Then
                dtActivityMedExTemplateDtl = Me.ViewState(VS_ActivityMedExTemplateDtl)

                'For validating Duplication
                Wstr = "cStatusIndi<>'" + Status_Delete + "' And vActivityGroupId='" & Me.ddlActivityGroup.SelectedValue.Trim() & _
                                                    "' And vActivityName='" & Me.txtactivityname.Text.Trim() & "'"
                If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                    Wstr += " And vActivityId <> '" + dtOld.Rows(0).Item("vActivityId").ToString.Trim() + "'"
                End If

                If Not objHelp.getActivityMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                    ds_Check, estr) Then
                    Me.ShowErrorMessage("Error While Getting Data From ActivityMst", estr)
                    Return False
                End If

                If ds_Check.Tables(0).Rows.Count > 0 Then
                    objcommon.ShowAlert("Activity name already exists for selected activity group", Me.Page)

                    If gvactivity.Rows.Count > 0 Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvactivity", "UIgvactivity(); ", True)
                    End If

                    Exit Function

                    Return False
                End If

                '*************************************
                'Validation of canSartAfter Activity for Scheduled Activity as suggested by Nikur Sir
                ds_Check = Nothing
                ds_Check = New DataSet

                If Me.ddlMileStone.SelectedValue = MileStone_Scheduling Or Me.ddlMileStone.SelectedValue = MileStone_Monitoring_Scheduling Then
                    If Me.chklstCanstart.SelectedIndex > -1 Then
                        For i = 0 To Me.chklstCanstart.Items.Count - 1
                            If Me.chklstCanstart.Items(i).Selected = True Then
                                If CanStartAfterDetails = "" Then
                                    CanStartAfterDetails = Me.chklstCanstart.Items(i).Value.Trim()
                                Else
                                    CanStartAfterDetails = CanStartAfterDetails + "," + Me.chklstCanstart.Items(i).Value.Trim()
                                End If
                            End If
                        Next

                        CanStartAfterDetails = CanStartAfterDetails.Replace(",", "','")
                        ArrCanStart = CanStartAfterDetails.Split(",")

                        Wstr = "vActivityId in ('" & CanStartAfterDetails & "') and nMilestone in (" & _
                                MileStone_Scheduling.ToString.Trim() & "," & MileStone_Monitoring_Scheduling.ToString.Trim() & ")"
                        If Not objHelp.getActivityMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                          ds_Check, estr) Then
                            Me.ShowErrorMessage("Error While Getting Data From ActivityMst For Canstart After", estr)
                            Return False
                        End If

                        If ds_Check.Tables(0).Rows.Count < ArrCanStart.Length() Then
                            objcommon.ShowAlert("Please select can start activities which are of 'Scheduling' or 'Monitoring & Scheduling' category !", Me.Page)
                            Return False
                        End If
                    End If
                End If
                '******************

                If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                    dtOld.Clear()
                    dr = dtOld.NewRow
                    dr("vActivityId") = "0000"
                    dr("vActivityGroupId") = Me.ddlActivityGroup.SelectedValue.Trim()
                    dr("vActivityName") = Me.txtactivityname.Text.Trim()
                    dr("vDocTypeCode") = IIf(String.IsNullOrEmpty(Me.ddldoctypecode.SelectedValue.Trim), Nothing, Me.ddldoctypecode.SelectedValue.Trim)
                    dr("cSubjectWiseFlag") = Me.ddlsubjectwiseflag.SelectedItem 'Me.ddlsubjectwiseflag.SelectedValue.Trim
                    dr("vDeptCode") = Me.ddldeptcode.SelectedValue.Trim()
                    dr("nCompletionDays") = Me.txtdays.Text.Trim()
                    dr("cPeriodSpecific") = Me.RBLPeriod.SelectedValue
                    dr("cIsRepeatable") = Me.rbtnlstIsRepeatable.SelectedValue
                    '==added on 25-10-10
                    'Removed on 16-02-2011 -> Pratiksha 
                    'dr("cActivityType") = Me.ddlActivityType.SelectedValue
                    '======
                    '======
                    dr("nMilestone") = Me.ddlMileStone.SelectedValue
                    dr("vCanStartAfter") = IIf(CanStartAfterDetails = "", "0000", CanStartAfterDetails.Trim())
                    dr("iModifyBy") = Me.Session(S_UserID)

                    dtOld.Rows.Add(dr)
                    '------------For dtActivityMedExTemplateDtl
                    dtActivityMedExTemplateDtl.Clear()
                    dr = dtActivityMedExTemplateDtl.NewRow

                    dr("nActivityMedExTemplateDtlNo") = 0
                    dr("vActivityId") = "0000"
                    dr("vMedExTemplateId") = ""
                    If Me.ddlMedExTemplate.SelectedIndex <> 0 Then
                        dr("vMedExTemplateId") = Me.ddlMedExTemplate.SelectedValue.Trim()
                    End If
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = Status_New
                    dtActivityMedExTemplateDtl.Rows.Add(dr)
                    '------------------------------

                ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                    For Each dr In dtOld.Rows
                        dr("vActivityName") = Me.txtactivityname.Text.Trim
                        dr("vActivityGroupId") = Me.ddlActivityGroup.SelectedValue.Trim()
                        dr("vDocTypeCode") = IIf(String.IsNullOrEmpty(Me.ddldoctypecode.SelectedValue.Trim), Nothing, Me.ddldoctypecode.SelectedValue.Trim)
                        dr("cSubjectWiseFlag") = Me.ddlsubjectwiseflag.SelectedItem 'Me.ddlsubjectwiseflag.SelectedValue.Trim
                        dr("vDeptCode") = Me.ddldeptcode.SelectedValue.Trim
                        dr("nCompletionDays") = Me.txtdays.Text.Trim
                        dr("cPeriodSpecific") = Me.RBLPeriod.SelectedValue
                        dr("cIsRepeatable") = Me.rbtnlstIsRepeatable.SelectedValue
                        '==added on 25-10-20
                        'Removed on 16-02-2011 -> Pratiksha 
                        'dr("cActivityType") = Me.ddlActivityType.SelectedValue
                        '=======
                        dr("nMilestone") = Me.ddlMileStone.SelectedValue

                        'For i = 0 To Me.chklstCanstart.Items.Count - 1
                        '    If Me.chklstCanstart.Items(i).Selected = True Then
                        '        If CanStartAfterDetails = "" Then
                        '            CanStartAfterDetails = Me.chklstCanstart.Items(i).Value.Trim()
                        '        Else
                        '            CanStartAfterDetails = CanStartAfterDetails + "," + Me.chklstCanstart.Items(i).Value.Trim()
                        '        End If
                        '    End If
                        'Next
                        dr("vCanStartAfter") = IIf(CanStartAfterDetails = "", "0000", CanStartAfterDetails.Trim())
                        dr("iModifyBy") = Me.Session(S_UserID)
                        dr.AcceptChanges()
                    Next
                    dtOld.AcceptChanges()

                    '------------For dtActivityMedExTemplateDtl
                    If dtActivityMedExTemplateDtl.Rows.Count > 0 Then

                        For Each dr In dtActivityMedExTemplateDtl.Rows
                            dr("vMedExTemplateId") = ""
                            If Me.ddlMedExTemplate.SelectedIndex <> 0 Then
                                dr("vMedExTemplateId") = Me.ddlMedExTemplate.SelectedValue.Trim()
                            End If
                            dr("iModifyBy") = Me.Session(S_UserID)
                            dr("cStatusIndi") = Status_Edit
                            dr.AcceptChanges()
                        Next
                        dtActivityMedExTemplateDtl.AcceptChanges()

                    Else
                        dtActivityMedExTemplateDtl.Clear()
                        dr = dtActivityMedExTemplateDtl.NewRow
                        dr("nActivityMedExTemplateDtlNo") = 0
                        dr("vActivityId") = "0000"
                        dr("vMedExTemplateId") = ""
                        If Me.ddlMedExTemplate.SelectedIndex <> 0 Then
                            dr("vMedExTemplateId") = Me.ddlMedExTemplate.SelectedValue.Trim()
                        End If
                        dr("iModifyBy") = Me.Session(S_UserID)
                        dr("cStatusIndi") = Status_New
                        dtActivityMedExTemplateDtl.Rows.Add(dr)
                    End If
                    '------------------------------------------------
                End If
            ElseIf Type = "Delete" Then
                For Each dr In dtOld.Rows
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = Status_Delete
                    dr.AcceptChanges()
                Next
                dtOld.TableName = "ActivityMst"
                ds_Save.Tables.Add(dtOld.Copy())

                If Not objOPws.Save_InsertActivityMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_activitymst, ds_Save, "1", estr) Then

                    objcommon.ShowAlert("Error while deleteing from attribute master !", Me.Page)
                    Return False
                End If
                objcommon.ShowAlert("Attribute deleted successFully !", Me.Page)
                If Not objcommon.GetScopeValueWithCondition(Wstr_Scope) Then
                    Return False
                End If

                Wstr_Scope += " And cStatusIndi<>'" + Status_Delete + "'"
                BindGrid(Wstr_Scope)
            End If

            Me.ViewState(VS_ActivityMst) = dtOld
            Me.ViewState(VS_ActivityMedExTemplateDtl) = dtActivityMedExTemplateDtl
            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...AssignUpdatedValues")
            Return False
        End Try
    End Function

    Private Function GetCanStartAfterDetails(ByRef CanStartAfterDetails As String) As Boolean
        Dim canStartAfter As String = String.Empty
        Dim dtCanStartAfter As DataTable = ViewState("dtCanStartAfter")
        Dim dr As DataRow
        Dim index As Integer = 0
        Dim estr As String = String.Empty
        CanStartAfterDetails = String.Empty

        Try
            ShowErrorMessage("", "")

            For Each dr In dtCanStartAfter.Rows
                canStartAfter += dr("vActivityId").ToString.Trim & ","
            Next
            canStartAfter = canStartAfter.Substring(0, canStartAfter.Length - 1)
            CanStartAfterDetails = canStartAfter

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...GetCanStartAfterDetails")
            Return False
        End Try
    End Function

#End Region

#Region "BindGrid"

    Private Sub BindGrid(ByVal wstr As String)
        Dim dsTemp As New DataSet
        Dim errStr As String = String.Empty

        If objHelp.GetActivityCodeDetails(dsTemp, wstr, errStr) Then
            ViewState("ActivityMst") = dsTemp
            Me.gvactivity.ShowFooter = False
            Me.gvactivity.DataSource = dsTemp
            Me.gvactivity.DataBind()

            If gvactivity.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvactivity", "UIgvactivity(); ", True)
            End If

            dsTemp.Dispose()
        End If
    End Sub

#End Region

#Region "Grid Events"

#Region "GVActivity Grid "

    Protected Sub gvactivity_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvactivity.PageIndexChanging
        gvactivity.PageIndex = e.NewPageIndex
        Dim Wstr_Scope As String = String.Empty

        Try
            'To Get Where condition of ScopeVales( Project Type )
            If Not objcommon.GetScopeValueWithCondition(Wstr_Scope) Then
                Exit Sub
            End If
            Wstr_Scope += " And cStatusIndi<>'" + Status_Delete + "'"
            BindGrid(Wstr_Scope)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...gvactivity_PageIndexChanging")
        End Try
    End Sub

    Protected Sub gvactivity_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvactivity.RowDataBound
        Dim lblSubjectWiseFlag As New Label
        Dim estr As String = String.Empty
        Dim canStartAfter As String = String.Empty
        Dim tempStartAfter As String = String.Empty
        Dim strquery As String = String.Empty
        Dim dsactivity As New DataSet
        Dim objCommon As New clsCommon
        Dim objLmd As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim index As Integer = 0

        Try


            If e.Row.RowType = DataControlRowType.DataRow Then

                e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (gvactivity.PageSize * gvactivity.PageIndex) + 1
                CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandName = "Edit"

                CType(e.Row.FindControl("ImgDelete"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgDelete"), ImageButton).CommandName = "Delete"

                If e.Row.Cells(GVC_Subjectwiseflag).Text.Trim.ToUpper() = "T" Then
                    e.Row.Cells(GVC_Subjectwiseflag).Text = "Yes"
                End If
                If e.Row.Cells(GVC_Subjectwiseflag).Text.Trim.ToUpper() = "Y" Then
                    e.Row.Cells(GVC_Subjectwiseflag).Text = "Yes"
                End If

                If e.Row.Cells(GVC_Subjectwiseflag).Text.Trim.ToUpper() = "N" Then
                    e.Row.Cells(GVC_Subjectwiseflag).Text = "No"
                End If
                If e.Row.Cells(GVC_Subjectwiseflag).Text.Trim.ToUpper() = "F" Then
                    e.Row.Cells(GVC_Subjectwiseflag).Text = "No"
                End If
                If e.Row.Cells(GVC_Subjectwiseflag).Text.Trim.ToUpper() = "V" Then
                    e.Row.Cells(GVC_Subjectwiseflag).Text = "View"
                End If

                If e.Row.Cells(GVC_PeriodSpecific).Text.Trim.ToUpper() = "Y" Then
                    e.Row.Cells(GVC_PeriodSpecific).Text = "Yes"
                Else
                    e.Row.Cells(GVC_PeriodSpecific).Text = "No"
                End If

                If e.Row.Cells(GVC_Milestone).Text.Trim.ToUpper() = "" Then
                    e.Row.Cells(GVC_Milestone).Text = "None"
                End If

                If e.Row.Cells(GVC_Milestone).Text.Trim.ToUpper() = "0" Then
                    e.Row.Cells(GVC_Milestone).Text = "None"
                End If

                If e.Row.Cells(GVC_Milestone).Text.Trim.ToUpper() = "1" Then
                    e.Row.Cells(GVC_Milestone).Text = "Monitoring"
                End If

                If e.Row.Cells(GVC_Milestone).Text.Trim.ToUpper() = "2" Then
                    e.Row.Cells(GVC_Milestone).Text = "Scheduling"
                End If

                If e.Row.Cells(GVC_Milestone).Text.Trim.ToUpper() = "3" Then
                    e.Row.Cells(GVC_Milestone).Text = "Monitoring & Scheduling"
                End If

                If e.Row.Cells(GVC_Milestone).Text.Trim.ToUpper() = "4" Then
                    e.Row.Cells(GVC_Milestone).Text = "None"
                End If

            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, estr + "...gvactivity_RowDataBound")
        End Try
    End Sub

    Protected Sub gvactivity_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvactivity.RowCommand
        Dim index As Integer = e.CommandArgument
        Dim wstr As String = String.Empty
        Dim ds_Save1 As New DataSet
        Dim ds_Save2 As New DataSet
        Dim ds_GetTemplateMst As New DataSet
        Dim estr As String = String.Empty
        Dim Wstr1 As String = String.Empty
        Dim Wstr2 As String = String.Empty
        Dim Index1 As Integer
        Dim IndexforTemplatename As Integer
        Dim MultipleTemplatedId As String = String.Empty
        Dim TemplateDesc As String = String.Empty

        Try
            If e.CommandName.ToUpper = "EDIT" Then
                Me.Response.Redirect("frmActivityMst.aspx?mode=2&value=" & CType(Me.gvactivity.Rows(index).FindControl("lblId"), Label).Text.Trim(), False)

            ElseIf e.CommandName.ToUpper = "DELETE" Then
                Wstr1 = "vActivityId='" & CType(Me.gvactivity.Rows(index).FindControl("lblId"), Label).Text.Trim() & "' AND cStatusIndi<>'" + Status_Delete + "'"
                If Not objHelp.getTemplateNodeDetail(Wstr1, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Save2, estr) Then
                    Me.ShowErrorMessage("Error While Getting Data From TemplateNodeDDetail For Canstart After", estr)
                Else
                    If ds_Save2.Tables(0).Rows.Count > 0 Then
                        For Index1 = 0 To ds_Save2.Tables(0).Rows.Count - 1 Step 1
                            If Index1 = ds_Save2.Tables(0).Rows.Count - 1 Then
                                MultipleTemplatedId += ds_Save2.Tables(0).Rows(Index1).Item("vTemplateId").ToString().Trim()
                            Else
                                MultipleTemplatedId += ds_Save2.Tables(0).Rows(Index1).Item("vTemplateId").ToString().Trim() & ","
                            End If
                        Next
                        Wstr2 = "vTemplateId in(" & MultipleTemplatedId & ") And cStatusIndi <> '" + Status_Delete + "'"
                        If Not objHelp.getTemplateMst(Wstr2, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_GetTemplateMst, estr) Then
                            Me.ShowErrorMessage("Error While Getting Data From GetTemplateMst", estr)
                        Else
                            TemplateDesc = "You can not delete this activity untill  it will not be deleted from following activity Template" & "\n" & "\n"
                            For IndexforTemplatename = 0 To ds_GetTemplateMst.Tables(0).Rows.Count - 1 Step 1

                                TemplateDesc += ds_GetTemplateMst.Tables(0).Rows(IndexforTemplatename).Item("vTemplateDesc").ToString().Trim() & "\n"
                            Next
                            objcommon.ShowAlert(TemplateDesc, Me.Page)
                        End If
                    Else
                        wstr = "vActivityId='" & CType(Me.gvactivity.Rows(index).FindControl("lblId"), Label).Text.Trim() & "'"
                        If Not objHelp.getActivityMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                    ds_Save1, estr) Then
                            Me.ShowErrorMessage("Error While Getting Data From ActivityMst For Canstart After", estr)
                            Exit Sub
                        End If
                        ViewState(VS_ActivityMst) = ds_Save1.Tables(0)
                        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                        AssignUpdatedValues("Delete")
                    End If
                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...gvactivity_RowCommand")
        End Try
    End Sub

#End Region

#Region "GVActivityDocLink Grid "

    Protected Sub GVActivityDocLink_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVActivityDocLink.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(GVActC_ActDocLinkNo).Visible = False
                e.Row.Cells(GVActC_DocTypeCode).Visible = False
                e.Row.Cells(GVActC_ProjId).Visible = False
                e.Row.Cells(GVActC_ActId).Visible = False
                e.Row.Cells(GVActC_NodeId).Visible = False

                If e.Row.RowType = DataControlRowType.DataRow Then
                    e.Row.Cells(GVActC_SrNo).Text = e.Row.RowIndex + 1
                    CType(e.Row.FindControl("ImgDelete"), ImageButton).CommandArgument = e.Row.RowIndex
                    CType(e.Row.FindControl("ImgDelete"), ImageButton).CommandName = "DELETE"
                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GVActivityDocLink_RowDataBound")
        End Try
    End Sub

    Protected Sub GVActivityDocLink_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GVActivityDocLink.RowCommand
        Dim Index As Integer = e.CommandArgument
        Dim dtActivityDocLink As New DataTable
        Dim dr As DataRow

        Try


            If e.CommandName.ToUpper.Trim() = "DELETE" Then
                dtActivityDocLink = CType(Me.ViewState(VS_ActivityDocLinkMatrix), DataTable)

                For Each dr In dtActivityDocLink.Rows
                    If dr("nActivityDocLinkNo") = Me.GVActivityDocLink.Rows(Index).Cells(GVActC_ActDocLinkNo).Text.Trim() Then

                        If Val(Me.GVActivityDocLink.Rows(Index).Cells(GVActC_ActDocLinkNo).Text.Trim) > 0 Then
                            dr("cStatusIndi") = Status_Delete
                            dtActivityDocLink.AcceptChanges()
                        Else
                            dtActivityDocLink.Rows.Remove(dr)
                            dtActivityDocLink.AcceptChanges()
                        End If
                        Exit For
                    End If
                Next dr
                Me.ViewState(VS_ActivityDocLinkMatrix) = dtActivityDocLink
                BindActivityDocLinkMatrix()
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...GVActivityDocLink_RowCommand")
        End Try
    End Sub

#End Region

#End Region

#Region "AssignActDocLinkValues"

    Protected Sub AssignActDocLinkValues()
        Dim dsActivityDocLink As New DataSet
        Dim dtActivityDocLink As New DataTable
        Dim dvActivityDocLink As New DataView
        Dim dsNodes As New DataSet
        Dim estr As String = String.Empty
        Dim dr As DataRow
        Try


            If IsNothing(Me.ViewState(VS_ActivityDocLinkMatrix)) Then
                If Not Me.objHelp.getActivityDocLinkMatrix("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, dsActivityDocLink, estr) Then
                    Me.objcommon.ShowAlert("Error while getting data from ActivityDocLinkMatrix: " + estr, Me.Page)
                    Exit Sub
                End If
                Me.ViewState(VS_ActivityDocLinkMatrix) = dsActivityDocLink.Tables(0)
            End If

            If Not Me.objHelp.GetViewWorkSpaceNodeDetail("vWorkspaceId='" & Me.ddlWorkspace.SelectedValue.Trim() & "' And iNodeId=" & Me.ddlNode.SelectedValue.Trim(), _
                   dsNodes, estr) Then
                Me.objcommon.ShowAlert("Error while getting data from WorkspaceNodeHistory: " + estr, Me.Page)
                Exit Sub
            End If

            If CType(Me.ViewState(VS_ActivityDocLinkMatrix), DataTable).Rows.Count > 1 Then
                dvActivityDocLink = CType(Me.ViewState(VS_ActivityDocLinkMatrix), DataTable).Copy().DefaultView()
                dvActivityDocLink.RowFilter = "vDocTypeCode='" & Me.ddlRefDocType.SelectedValue.Trim() & _
                                            "' And vLinkedWorkSpaceId='" & Me.ddlWorkspace.SelectedValue.Trim() & _
                                            "' And iLinkedNodeId=" & Me.ddlNode.SelectedValue.Trim() & _
                                            " And vLinkedActivityId ='" & dsNodes.Tables(0).Rows(0).Item("vActivityId") & "' " 'And cStatusIndi <> 'D'"

                If dvActivityDocLink.ToTable().Rows.Count >= 1 Then

                    If dvActivityDocLink.ToTable().Rows(0).Item("cStatusIndi").ToString.ToUpper.Trim() = Status_Delete Then
                        dtActivityDocLink = CType(Me.ViewState(VS_ActivityDocLinkMatrix), DataTable)
                        For Each dr In dtActivityDocLink.Rows

                            If dr("nActivityDocLinkNo") = dvActivityDocLink.ToTable().Rows(0).Item("nActivityDocLinkNo") Then
                                dr("cStatusIndi") = Status_New
                                dtActivityDocLink.AcceptChanges()
                                Me.ViewState(VS_ActivityDocLinkMatrix) = dtActivityDocLink
                                BindActivityDocLinkMatrix()
                                Exit For
                            End If
                        Next dr
                    Else
                        Me.objcommon.ShowAlert("Referance already added. Please, select other", Me.Page)
                    End If
                    Exit Sub
                End If
            End If

            dtActivityDocLink = CType(Me.ViewState(VS_ActivityDocLinkMatrix), DataTable)
            dr = dtActivityDocLink.NewRow
            dr("nActivityDocLinkNo") = 0 '-999 + dtActivityDocLink.Rows.Count
            dr("vDocTypeCode") = Me.ddlRefDocType.SelectedValue.Trim()
            dr("vDocTypeName") = Me.ddlRefDocType.SelectedItem.Text.Trim()
            dr("vLinkedWorkSpaceId") = Me.ddlWorkspace.SelectedValue.Trim()
            dr("vWorkSpaceDesc") = Me.ddlWorkspace.SelectedItem.Text.Trim()
            dr("vLinkedActivityId") = dsNodes.Tables(0).Rows(0).Item("vActivityId")
            dr("vActivityName") = Me.ddlNode.SelectedItem.Text.Trim()
            dr("iLinkedNodeId") = Me.ddlNode.SelectedValue.Trim()
            dr("vNodeDisplayName") = Me.ddlNode.SelectedItem.Text.Trim()
            dr("iModifyBy") = Me.Session(S_UserID).ToString.Trim()
            dr("cStatusIndi") = Status_New

            dtActivityDocLink.Rows.Add(dr)
            Me.ViewState(VS_ActivityDocLinkMatrix) = dtActivityDocLink

            BindActivityDocLinkMatrix()

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...AssignActDocLinkValues")
        End Try
    End Sub

#End Region

#Region "BindActivityDocLinkMatrix"

    Private Sub BindActivityDocLinkMatrix()
        Dim dv_Grid As New DataView
        Try

            dv_Grid = CType(Me.ViewState(VS_ActivityDocLinkMatrix), DataTable).Copy().DefaultView()
            dv_Grid.RowFilter = "cStatusIndi <> '" + Status_Delete + "'"
            Me.GVActivityDocLink.DataSource = dv_Grid
            Me.GVActivityDocLink.DataBind()

            If GVActivityDocLink.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGVActivityDocLink", "UIGVActivityDocLink(); ", True)
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...BindActivityDocLinkMatrix")
        End Try
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

    Protected Sub gvactivity_RowDeleting(sender As Object, e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvactivity.RowDeleting

    End Sub

    Protected Sub gvactivity_RowEditing(sender As Object, e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvactivity.RowEditing
        e.Cancel = True
    End Sub

    <WebMethod> _
    Public Shared Function View_Activitymst() As String
        Dim ds As New Data.DataSet
        Dim dv_User As New Data.DataView
        Dim estr As String = String.Empty
        Dim lblexceltitle As String = String.Empty
        Dim wstr As String = String.Empty
        Dim Wstr1 As String = String.Empty
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()

        Dim strReturn As String = String.Empty

        Try

            If Not ObjCommon.GetScopeValueWithCondition(wstr) Then
                Return False
            End If

            Wstr1 = wstr & "and cstatusindi <>'D'"

            If Not objHelp.GetActivityCodeDetails(ds, Wstr1, estr) Then
                Return False
            End If


            strReturn = JsonConvert.SerializeObject(ds)

            Return strReturn

        Catch ex As Exception

            Return strReturn
        End Try

    End Function
    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click

        Me.Response.Redirect("frmActivityMst.aspx?mode=2&value=" & Me.hdnEditedId.Value)

    End Sub
    <WebMethod> _
    Public Shared Function Delete_ActivityMst(ByVal id As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim Wstr1 As String = String.Empty
        Dim Wstr2 As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim ds_RoleOp As New Data.DataSet
        Dim ds As New Data.DataSet
        Dim objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()
        Dim objOPws As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()
        Dim Dr_ActOp As DataRow
        Dim Dt_Actopt As New DataTable
        Dim ds_Save1 As New DataSet
        Dim ds_Save2 As New DataSet
        Dim ds_GetTemplateMst As New DataSet
        Dim MultipleTemplatedId As String = String.Empty
        Dim TemplateDesc As String = String.Empty


        Try

            Wstr1 = "  cStatusIndi <> 'D' AND vActivityId = '" + id + "' "
            If Not objHelp.getTemplateNodeDetail(Wstr1, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Save2, estr) Then
                ' Me.ShowErrorMessage("Error While Getting Data From TemplateNodeDDetail For Canstart After", estr)

            Else
                If Not IsNothing(ds_Save2) AndAlso ds_Save2.Tables(0).Rows.Count > 0 Then
                    For Index1 = 0 To ds_Save2.Tables(0).Rows.Count - 1 Step 1
                        If Index1 = ds_Save2.Tables(0).Rows.Count - 1 Then
                            MultipleTemplatedId += ds_Save2.Tables(0).Rows(Index1).Item("vTemplateId").ToString().Trim()
                        Else
                            MultipleTemplatedId += ds_Save2.Tables(0).Rows(Index1).Item("vTemplateId").ToString().Trim() & ","
                        End If
                    Next
                    Wstr2 = "vTemplateId in(" & MultipleTemplatedId & ") And cStatusIndi <> '" + Status_Delete + "'"
                    If Not objHelp.getTemplateMst(Wstr2, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_GetTemplateMst, estr) Then
                        ' Me.ShowErrorMessage("Error While Getting Data From GetTemplateMst", estr)

                    Else
                        TemplateDesc = "You can not delete this activity untill  it will not be deleted from following activity Template" & "\n" & "\n"
                        For IndexforTemplatename = 0 To ds_GetTemplateMst.Tables(0).Rows.Count - 1 Step 1

                            TemplateDesc += ds_GetTemplateMst.Tables(0).Rows(IndexforTemplatename).Item("vTemplateDesc").ToString().Trim() & "\n"
                        Next
                        '  ObjCommon.ShowAlert(TemplateDesc, Me.Page)
                    End If
                Else
                    Wstr = "vActivityId='" + id + "' "
                    If Not objHelp.getActivityMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                ds_Save1, estr) Then
                        ' Me.ShowErrorMessage("Error While Getting Data From ActivityMst For Canstart After", estr)
                        Return False
                        Exit Function
                    End If
                End If
            End If
            Dt_Actopt.Columns.Add("vActivityId")
            Dt_Actopt.Columns.Add("cStatusIndi")
            Dt_Actopt.Columns.Add("iModifyBy")
            Dt_Actopt.AcceptChanges()
            Dr_ActOp = Dt_Actopt.NewRow()
            Dr_ActOp("vActivityId") = id
            Dr_ActOp("cStatusIndi") = Status_Delete
            Dr_ActOp("iModifyBy") = HttpContext.Current.Session(S_UserID)
            Dt_Actopt.Rows.Add(Dr_ActOp)
            Dt_Actopt.TableName = "ActivityMst"
            ds_RoleOp.Tables.Add(Dt_Actopt.Copy())
            If ds_RoleOp.Tables(0).Rows.Count > 0 Then
                If Not objOPws.Save_InsertActivityMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_activitymst, ds_RoleOp, "1", estr) Then

                    'ObjCommon.ShowAlert("Error while deleteing from attribute master", Me.Page)
                    Return False
                End If
            End If
            If Not objcommon.GetScopeValueWithCondition(Wstr) Then
                Return False
            End If

            Wstr1 = Wstr & "and cstatusindi <>'D'"

            If Not objHelp.GetActivityCodeDetails(ds, Wstr1, estr) Then
                Return False
            End If


            strReturn = JsonConvert.SerializeObject(ds)

            Return strReturn
        Catch ex As Exception

            Return strReturn
        End Try

    End Function

End Class
