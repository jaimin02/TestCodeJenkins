
Partial Class frmuserRights
    Inherits System.Web.UI.Page
    Dim ObjCommon As New clsCommon
    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtTemplateWorkFlowUserDtl As String = "DtTemplateWorkFlowUserDtl"
    Private Const VS_TemplateWorkflowUserId As String = "TemplateWorkflowUserId"
    Private Const VS_TemplateId As String = "TemplateId"
    Private Const VS_Default As String = "DefaultUserRight"
#Region "Load Event"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
            End If
        Catch ex As Exception

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

            Me.ViewState(VS_TemplateId) = Me.Request.QueryString("TemplateId").ToString.Trim()
            Me.ViewState(VS_Default) = Me.Request.QueryString("Default").ToString.Trim()
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_TemplateWorkflowUserId) = Me.Request.QueryString("Value").ToString
            End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtTemplateWorkFlowUserDtl) = ds.Tables("TemplateWorkFlowUserDtl")   ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")

        Finally

        End Try

    End Function
#End Region

#Region " GenCall_Data "
    Private Function GenCall_Data(ByRef ds_Retu As DataSet) As Boolean

        Dim wStr As String = ""
        Dim eStr_Retu As String = ""
        Dim Val As String = ""
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing
        'Dim objFFR As New FFReporting.FFReporting

        Try

            Val = Me.ViewState(VS_TemplateWorkflowUserId) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "nTemplateWorkflowUserIde=" + Val.ToString
            End If


            If Not objHelp.getTemplateWorkflowUserDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
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
            Me.ShowErrorMessage(ex.Message, "")

        Finally

        End Try
    End Function
#End Region

#Region " GenCall_showUI() "
    Private Function GenCall_ShowUI() As Boolean
        Dim dt_OpMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try

            Page.Title = ":: User Rights  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Default User Rights"

            dt_OpMst = Me.ViewState(VS_DtTemplateWorkFlowUserDtl)

            Choice = Me.ViewState("Choice")

            If Not FillDropDown() Then
                Exit Function
            End If
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

            End If
            Me.BtnSave.Attributes.Add("OnClick", "return Validation();")

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function
#End Region
#Region "FillDropDown"
    Private Function FillDropDown() As Boolean
        Dim ds_Dept As New Data.DataSet
        Dim ds_Location As New Data.DataSet
        Dim estr As String = ""
        Try

            objHelp.FillDropDown("DeptMst", "vDeptCode", "vDeptName", "", ds_Dept, estr)
            objHelp.FillDropDown("LocationMst", "vLocationCode", "vLocationName", "", ds_Location, estr)

            Me.DDLDepartment.DataSource = ds_Dept
            Me.DDLDepartment.DataValueField = "vDeptCode"
            Me.DDLDepartment.DataTextField = "vDeptName"
            Me.DDLDepartment.DataBind()
            Me.DDLDepartment.Items.Insert(0, New ListItem("select Department", 0))

            Me.DDLLocation.DataSource = ds_Location
            Me.DDLLocation.DataValueField = "vLocationCode"
            Me.DDLLocation.DataTextField = "vLocationName"
            Me.DDLLocation.DataBind()
            Me.DDLLocation.Items.Insert(0, New ListItem("select Location", 0))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function


    Private Function fillUser() As Boolean
        Dim Ds_User As New Data.DataSet
        Dim estr As String = ""
        Try

            If Me.DDLLocation.SelectedIndex = 0 Or Me.DDLDepartment.SelectedIndex = 0 Then
                Return False
            End If

            Me.objHelp.FillDropDown("UserMst", "iUserId", "vUserName", "vDeptCode= '" & Me.DDLDepartment.SelectedValue.Trim() & "' and vLocationCode='" & Me.DDLLocation.SelectedValue.Trim() & "'", Ds_User, estr)

            Me.DDLUser.DataSource = Ds_User
            Me.DDLUser.DataValueField = "iUserId"
            Me.DDLUser.DataTextField = "vUserName"
            Me.DDLUser.DataBind()
            Me.DDLUser.Items.Insert(0, New ListItem("Select User", ""))
            Return True
        Catch ex As Exception
            Return True
        End Try
    End Function

#End Region

#Region "SelectedIndexChanged"
    Protected Sub DDLDepartment_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLDepartment.SelectedIndexChanged
        If Not fillUser() Then
        End If
    End Sub

    Protected Sub DDLLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLLocation.SelectedIndexChanged
        If Not fillUser() Then
        End If
    End Sub
#End Region

#Region "Button Click"
    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim ds_Save As New DataSet
        Dim dt_Save As New DataTable
        Dim estr As String = ""
        Try
            AssignValues()
            ds_Save = New DataSet
            dt_Save = CType(Me.ViewState(VS_DtTemplateWorkFlowUserDtl), DataTable)
            dt_Save.TableName = "TemplateWorkFlowUserDtl"
            ds_Save.Tables.Add(dt_Save)
            If Not objLambda.Save_InsertTemplateWorkflowUserDtl(Me.ViewState(VS_Choice), ds_Save, "Yes", Me.Session(S_UserID), estr) Then
                ObjCommon.ShowAlert("Error While Saving TemplateWorkflowUserDtl", Me.Page)
                Exit Sub
            Else
                ObjCommon.ShowAlert("Record Saved SuccessFuly", Me.Page)
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub
    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Me.Response.Redirect("frmtreenodeMst.aspx?vTemplateId=" & Me.ViewState(VS_TemplateId))
    End Sub
#End Region

#Region "Assign values"
    Private Sub AssignValues()
        Dim dr As DataRow
        Dim dt_UserRights As New DataTable
        dt_UserRights = CType(Me.ViewState(VS_DtTemplateWorkFlowUserDtl), DataTable)
        dt_UserRights.Clear()
        dr = dt_UserRights.NewRow()
        ',,,,,,,
        dr("nTemplateWorkflowUserId") = "00"
        dr("vTemplateId") = Me.ViewState(VS_TemplateId)
        dr("iNodeId") = 0
        dr("iUserId") = Me.DDLUser.SelectedValue
        dr("iStageId") = GeneralModule.Stage_Created
        dr("cCanEdit") = ""
        dr("cCanRead") = ""
        dr("cCanDelete") = ""
        dr("iModifyBy") = Me.Session(S_UserID)
        dt_UserRights.Rows.Add(dr)
        Me.ViewState(VS_DtTemplateWorkFlowUserDtl) = dt_UserRights
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
