Partial Class frmProjectSubmissionDtl
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATIONS"

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const VS_WorkspaceId As String = "workspaceId"
    Private Const VS_MyProject As String = "Myproject"
    Private Const VS_Country As String = "Country"
    Private Const VS_MyProject_Edit As String = "Myproject_Edit"
    Private Const VS_TotalRowCount As String = "TotalRowCount"
    Private Const GVC_LnkBtnChangeStatus As Integer = 12
    Private Const GVC_DefaultUserRights As Integer = 15
    Private Const VS_Choice As String = "Choice"
    Private eStr_Retu As String = ""
    Private Const PAGESIZE As Integer = 25
    Private Const VS_WorkspaceCMS As String = "workspaceCMS"

    'iWorkspaceCMSId, vWorkspaceId, vCountryCode, vAgencyCode, iWaveNo, dModifyOn, iModifyBy
    'cStatusIndi, vCMSTrackingNo, vCountryName, vAgencyName

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_iWorkspaceCMSId As Integer = 1
    Private Const GVC_vWorkspaceId As Integer = 2
    Private Const GVC_iWaveNo As Integer = 3
    Private Const GVC_vCountryName As Integer = 4
    Private Const GVC_vAgencyName As Integer = 5
    Private Const GVC_vCMSTrackingNo As Integer = 6

#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim estr As String = Nothing
        Dim ds As New DataSet
        Try

            If Not IsPostBack Then
                CType(Me.Master.FindControl("lblHeading"), Label).Text = "Project Submission"

                If Not GenCall_ShowUI() Then
                    Exit Sub
                End If

                Choice = Me.Request.QueryString("Mode")
                Me.ViewState(VS_Choice) = Choice   'To use it while saving
                If Not Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                    GenCall()
                Else
                    ''
                End If


            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "GenCall"
    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Choice = Me.Request.QueryString("Mode")
            Me.ViewState(VS_Choice) = Choice   'To use it while saving
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                'Me.ViewState(VS_WorkSpaceId) = Me.Request.QueryString("Value").ToString
                'Me.ViewState(VS_WorkSpaceId) = Me.HWorkspaceId.Value.Trim()
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally

        End Try

    End Function
#End Region

#Region "GenCall_Data "

    Private Function GenCall_Data() As Boolean
        Dim ds_Projects As New DataSet
        Dim wStr As String = ""
        Dim eStr As String = ""
        Dim Val As String = ""
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds_ClientMst As DataSet = Nothing
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""


            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        Finally

        End Try
    End Function

#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim sender As New Object
        Dim e As New EventArgs
        Try
            Page.Title = ":: Project Submission Details  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblMandatory"), Label).Visible = False
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Project Submission"

            'added by Deepak Singh on 2-Mar-10 to show the set Project on page load
            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
                GenCall_ShowUI = True
                Exit Function
            End If

            '==added on 15-jan-2010 by deepak singh to show project according to user
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
            '========

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "Fill Dropdowns"

    Private Function FillRegionDropDown() As Boolean
        Dim ds_Region As New DataSet
        Dim dv_Region As New DataView
        Dim estr As String = ""

        Try
            If Not objHelp.getregionmst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_Region, estr) Then
                Throw New Exception(estr)
            End If

            dv_Region = ds_Region.Tables(0).DefaultView
            dv_Region.Sort = "vRegionName"

            Me.ddlRegion.DataSource = dv_Region
            Me.ddlRegion.DataValueField = "vRegionCode"
            Me.ddlRegion.DataTextField = "vRegionName"
            Me.ddlRegion.DataBind()
            Me.ddlRegion.Items.Insert(0, New ListItem("Select Region", ""))

            'Added Tooltip
            For iddlRegion As Integer = 0 To ddlRegion.Items.Count - 1
                ddlRegion.Items(iddlRegion).Attributes.Add("title", ddlRegion.Items(iddlRegion).Text)
            Next
            Me.ddlCountry.Items.Insert(0, New ListItem("Select Country", ""))

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function FillRegionDropDown_Edit() As Boolean
        Dim ds_SubmittedProject As New DataSet
        Dim ds_Region As New DataSet
        Dim dv_Region As New DataView
        Dim estr As String = ""
        Dim wstr As String = ""
        Dim RegionCode As String = ""

        Try
            ds_SubmittedProject = ViewState(VS_MyProject_Edit)
            RegionCode = ds_SubmittedProject.Tables(0).Rows(0)("CountryRegion").ToString

            wstr = "vRegionCode = '" + RegionCode + "'"
            If Not objHelp.getregionmst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Region, estr) Then
                Throw New Exception(estr)
            End If

            dv_Region = ds_Region.Tables(0).DefaultView
            dv_Region.Sort = "vRegionName"

            Me.ddlRegion.DataSource = dv_Region
            Me.ddlRegion.DataValueField = "vRegionCode"
            Me.ddlRegion.DataTextField = "vRegionName"
            Me.ddlRegion.DataBind()
            Me.ddlRegion.Items.Insert(0, New ListItem("Select Region", ""))

            'Added Tooltip
            For iddlRegion As Integer = 0 To ddlRegion.Items.Count - 1
                ddlRegion.Items(iddlRegion).Attributes.Add("title", ddlRegion.Items(iddlRegion).Text)
            Next
            ddlRegion.SelectedIndex = 1
            Me.ddlRegion_SelectedIndexChanged(ddlRegion, New EventArgs())
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function FillAgencyDropDown() As Boolean
        Dim ds_Agency As New DataSet
        Dim dv_Agency As New DataView
        Dim ds_Country As New DataSet
        Dim dv_Country As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim rmsCountryId As String = String.Empty

        Try
            wstr = "vAgencyRegionCode=" + ddlCountry.SelectedItem.Value.ToString
            If Not objHelp.GetAgencyMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Agency, estr) Then
                Throw New Exception(estr)
            End If

            dv_Agency = ds_Agency.Tables(0).DefaultView
            ddlAgencyName.DataSource = dv_Agency
            Me.ddlAgencyName.DataValueField = "vAgencyCode"
            Me.ddlAgencyName.DataTextField = "vAgencyName"
            Me.ddlAgencyName.DataBind()
            Me.ddlAgencyName.Items.Insert(0, New ListItem("Select Agency", ""))

            'Added Tooltip
            For iddlAgencyName As Integer = 0 To ddlAgencyName.Items.Count - 1
                ddlAgencyName.Items(iddlAgencyName).Attributes.Add("title", ddlAgencyName.Items(iddlAgencyName).Text)
            Next

            '''''''''''''REMOVING SELECTED RMS COUNTRY FROM CMS COUNTRY DROPDOWN'''''''''''''''''
            ds_Country = Me.ViewState(VS_Country)
            ddlCMSCountryName.Items.Clear()
            dv_Country = ds_Country.Tables(0).DefaultView
            dv_Country.Sort = "vCountryName"
            Me.ddlCMSCountryName.DataSource = dv_Country
            Me.ddlCMSCountryName.DataValueField = "vCountryId"
            Me.ddlCMSCountryName.DataTextField = "vCountryName"
            Me.ddlCMSCountryName.DataBind()
            Me.ddlCMSCountryName.Items.Insert(0, New ListItem("Select Country", ""))

            'Added Tooltip
            'For iddlCMSCountryName As Integer = 0 To ddlCMSCountryName.Items.Count - 2
            '    ddlCMSCountryName.Items(iddlCMSCountryName).Attributes.Add("title", ddlCountry.Items(iddlCMSCountryName).Text)
            'Next

            rmsCountryId = ddlCountry.SelectedItem.Value.ToString
            For iddlCMSCountryName As Integer = 0 To ddlCMSCountryName.Items.Count - 1
                If (ddlCMSCountryName.Items(iddlCMSCountryName).Value = rmsCountryId) Then
                    ddlCMSCountryName.Items.RemoveAt(iddlCMSCountryName)
                    Exit For
                End If
            Next

            '''''''''''''FILLING CMS AGENCY DROPDOWN'''''''''''''''''
            Me.ddlCMSAgencyName.DataValueField = "vAgencyCode"
            Me.ddlCMSAgencyName.DataTextField = "vAgencyName"
            Me.ddlCMSAgencyName.DataBind()
            Me.ddlCMSAgencyName.Items.Insert(0, New ListItem("Select Agency", ""))

            'Added Tooltip
            For iddlCMSAgencyName As Integer = 0 To ddlCMSAgencyName.Items.Count - 1
                ddlCMSAgencyName.Items(iddlCMSAgencyName).Attributes.Add("title", ddlCMSAgencyName.Items(iddlCMSAgencyName).Text)
            Next

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

    Private Function FillCountryDropDown() As Boolean
        Dim ds_Country As New DataSet
        Dim dv_Country As New DataView
        Dim estr As String = ""
        Dim wstr As String = ""

        Try

            wstr = "vRegionId=" + ddlRegion.SelectedItem.Value
            If Not objHelp.GetCountryMaster(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Country, estr) Then
                Throw New Exception(estr)
            End If

            If Not ds_Country Is Nothing Then
                dv_Country = ds_Country.Tables(0).DefaultView
                dv_Country.Sort = "vCountryName"
                Me.ddlCountry.DataSource = dv_Country
                Me.ddlCountry.DataValueField = "vCountryId"
                Me.ddlCountry.DataTextField = "vCountryName"
                Me.ddlCountry.DataBind()
                Me.ddlCountry.Items.Insert(0, New ListItem("Select Country", ""))

                'Added Tooltip
                For iddlCountry As Integer = 0 To ddlCountry.Items.Count - 1
                    ddlCountry.Items(iddlCountry).Attributes.Add("title", ddlCountry.Items(iddlCountry).Text)
                Next

                ''''''''''FILLING CMS COUNTRY DROPDOWN'''''''''''''''
                Me.ddlCMSCountryName.DataSource = dv_Country
                Me.ddlCMSCountryName.DataValueField = "vCountryId"
                Me.ddlCMSCountryName.DataTextField = "vCountryName"
                Me.ddlCMSCountryName.DataBind()
                Me.ddlCMSCountryName.Items.Insert(0, New ListItem("Select Country", ""))

                ''''''''''STORING COUNTRY'S DS FOR FUTURE USE'''''''''''''''
                Me.ViewState(VS_Country) = ds_Country

                'Added Tooltip
                For iddlCMSCountryName As Integer = 0 To ddlCMSCountryName.Items.Count - 1
                    ddlCMSCountryName.Items(iddlCMSCountryName).Attributes.Add("title", ddlCMSCountryName.Items(iddlCMSCountryName).Text)
                Next
            End If


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Function

    Private Function FillCMSAgencyDropDown() As Boolean
        Dim ds_Agency As New DataSet
        Dim dv_Agency As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty

        Try
            wstr = "vAgencyRegionCode=" + ddlCMSCountryName.SelectedItem.Value.ToString
            If Not objHelp.GetAgencyMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Agency, estr) Then
                Throw New Exception(estr)
            End If

            dv_Agency = ds_Agency.Tables(0).DefaultView
            ddlCMSAgencyName.DataSource = dv_Agency

            Me.ddlCMSAgencyName.DataValueField = "vAgencyCode"
            Me.ddlCMSAgencyName.DataTextField = "vAgencyName"
            Me.ddlCMSAgencyName.DataBind()
            Me.ddlCMSAgencyName.Items.Insert(0, New ListItem("Select Agency", ""))

            'Added Tooltip
            For iddlCMSAgencyName As Integer = 0 To ddlCMSAgencyName.Items.Count - 1
                ddlCMSAgencyName.Items(iddlCMSAgencyName).Attributes.Add("title", ddlCMSAgencyName.Items(iddlCMSAgencyName).Text)
            Next

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

#End Region

#Region "BindGrid"

    Private Sub BindGrid()
        Dim ds_WorkspaceCMS As New DataSet
        Dim dv_WorkspaceCMS As New DataView
        Dim estr As String = Nothing
        Dim wstr As String = Nothing

        Try
            wstr = "vWorkspaceId ='" + Me.HProjectId.Value.Trim() + "' AND cStatusIndi <> '" + "D" + "'"
            wstr += " Order By iWorkspaceCMSId Desc"
            If Not objHelp.view_WorkspaceCmsMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_WorkspaceCMS, estr) Then
                Exit Sub
            End If

            dv_WorkspaceCMS = ds_WorkspaceCMS.Tables(0).DefaultView
            gvCMS.DataSource = dv_WorkspaceCMS
            Me.gvCMS.DataBind()
            Me.gvCMS.Dispose()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "Grid Events"

    Protected Sub gvCMS_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCMS.RowCommand
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim index As Integer = e.CommandArgument
        Dim ds_workspaceCMS As New DataSet
        Dim dt_workspaceCMS As New DataTable
        Dim vTrackingNoNew As String = Nothing

        Dim wstr As String = Nothing
        Dim estr As String = Nothing

        Try
            Me.MPECMS.Show()
            wstr = "iWorkspaceCMSId =" & Me.gvCMS.Rows(index).Cells(GVC_iWorkspaceCMSId).Text
            If Not objHelp.WorkspaceCMSMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_workspaceCMS, estr) Then
                Exit Sub
            End If

            If e.CommandName.ToUpper = "MYEDIT" Then
                CType(gvCMS.Rows(index).FindControl("ImgSave"), ImageButton).Visible = True
                CType(gvCMS.Rows(index).FindControl("ImgCancel"), ImageButton).Visible = True
                CType(gvCMS.Rows(index).FindControl("ImgEdit"), ImageButton).Visible = False
                CType(gvCMS.Rows(index).FindControl("txtTrackingNumber"), TextBox).Enabled = True
            End If

            If e.CommandName.ToUpper = "MYDELETE" Then
                '''''''''''DELETING ROW FROM DB..''''''''' {Setting cStatusindi='D'}
                Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

                ds_workspaceCMS.Tables(0).Rows(0)("cStatusIndi") = "D"
                ds_workspaceCMS.AcceptChanges()

                If Not objLambda.Save_WorkspaceCMSMst(Choice, ds_workspaceCMS, Me.Session(S_UserID), estr) Then
                    Throw New Exception(estr)
                End If

                Me.BindGrid()
                Me.ResetCMSControls()
            End If

            If e.CommandName.ToUpper = "MYSAVE" Then
                Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                vTrackingNoNew = CType(Me.gvCMS.Rows(index).FindControl("txtTrackingNumber"), TextBox).Text

                ds_workspaceCMS.Tables(0).Rows(0)("vCMSTrackingNo") = vTrackingNoNew
                ds_workspaceCMS.AcceptChanges()

                If Not objLambda.Save_WorkspaceCMSMst(Choice, ds_workspaceCMS, Me.Session(S_UserID), estr) Then
                    Throw New Exception(estr)
                End If

                Me.BindGrid()
                Me.ResetCMSControls()

                CType(gvCMS.Rows(index).FindControl("ImgSave"), ImageButton).Visible = False
                CType(gvCMS.Rows(index).FindControl("ImgCancel"), ImageButton).Visible = False
                CType(gvCMS.Rows(index).FindControl("ImgEdit"), ImageButton).Visible = True
                CType(gvCMS.Rows(index).FindControl("txtTrackingNumber"), TextBox).Enabled = False
            End If

            If e.CommandName.ToUpper = "MYCANCEL" Then
                CType(gvCMS.Rows(index).FindControl("ImgSave"), ImageButton).Visible = False
                CType(gvCMS.Rows(index).FindControl("ImgCancel"), ImageButton).Visible = False
                CType(gvCMS.Rows(index).FindControl("ImgEdit"), ImageButton).Visible = True
                CType(gvCMS.Rows(index).FindControl("txtTrackingNumber"), TextBox).Enabled = False
            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvCMS_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCMS.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Or e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(GVC_iWorkspaceCMSId).Visible = False
                e.Row.Cells(GVC_vWorkspaceId).Visible = False
            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("ImgEdit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgEdit"), ImageButton).CommandName = "MYEDIT"
                CType(e.Row.FindControl("ImgDelete"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgDelete"), ImageButton).CommandName = "MYDELETE"
                CType(e.Row.FindControl("ImgSave"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgSave"), ImageButton).CommandName = "MYSAVE"
                CType(e.Row.FindControl("ImgCancel"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgCancel"), ImageButton).CommandName = "MYCANCEL"

                CType(e.Row.FindControl("ImgSave"), ImageButton).Visible = False
                CType(e.Row.FindControl("ImgCancel"), ImageButton).Visible = False
                CType(e.Row.FindControl("ImgEdit"), ImageButton).Visible = True

                '''''''''' Adding mouse hover event on grid to change the color of current row '''''''''
                If e.Row.RowType = DataControlRowType.DataRow Then
                    If e.Row.RowState = DataControlRowState.Alternate Then
                        e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#FFFFE1';")
                        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF';")
                    Else
                        e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#FFFFE1';")
                        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#CEE3ED';")
                    End If
                End If

                e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (gvCMS.PageSize * gvCMS.PageIndex) + 1
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

#End Region

#Region "Helper Functions/Sub"

    Private Sub FillProjectDtl()
        Dim ds_project As New DataSet
        Dim ds_SubmittedProject As New DataSet
        Dim ds_usermst As New DataSet
        Dim wstr As String = Nothing
        Dim estr As String = Nothing
        Dim flag As Boolean = Nothing
        ds_project = ViewState(VS_MyProject)
        ds_SubmittedProject = ViewState(VS_MyProject_Edit)


        Try
            Me.lblProjectName.Text = ds_project.Tables(0).Rows(0)("vWorkspaceDesc").ToString
            Me.lblProjectType.Text = ds_project.Tables(0).Rows(0)("vProjectTypeName").ToString
            Me.lblClientName.Text = ds_project.Tables(0).Rows(0)("vClientName").ToString
            Me.lblRegionName.Text = ds_project.Tables(0).Rows(0)("vRegionName").ToString


            ''''''Labels of Details'''''''''
            If Not ds_SubmittedProject Is Nothing Then
                wstr = "iUserId=" + ds_SubmittedProject.Tables(0).Rows(0)("iModifyBy").ToString
                If Not objHelp.getuserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_usermst, eStr_Retu) Then
                    Throw New Exception(estr)
                End If

                Me.lblDetailsEditedBy.Text = ds_usermst.Tables(0).Rows(0)("vUserName").ToString
                Me.lblDetailsEditedOn.Text = ds_SubmittedProject.Tables(0).Rows(0)("dModifyOn").ToString
                ds_SubmittedProject.Dispose()
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Private Sub SetVisibleControls()
        DisableControls()
        If ddlRegion.SelectedItem.Text = "us" Or ddlRegion.SelectedItem.Text = "US" Then
            lblDetailTag.Text = "Specify following details for US-Regional"
            trPanel3.Style.Add("Display", "block")

            trAgencyName.Style.Add("Display", "block")
            trApplicationNo.Style.Add("Display", "block")
            trAgencyName.Style.Add("Display", "block")
            trCompanyName.Style.Add("Display", "block")
            trProductName.Style.Add("Display", "block")
            trProductType.Style.Add("Display", "block")
            trApplicationType.Style.Add("Display", "block")
        ElseIf ddlRegion.SelectedItem.Text = "eu" Or ddlRegion.SelectedItem.Text = "EU" Then
            lblDetailTag.Text = "Specify following details for EU-Regional"
            trPanel3.Style.Add("Display", "block")

            trRegionalVersion.Style.Add("Display", "block")
            trTrackingNo.Style.Add("Display", "block")
            trHighLevelNo.Style.Add("Display", "block")
            trApplicant.Style.Add("Display", "block")
            trProcedureType.Style.Add("Display", "block")
            trInventedName.Style.Add("Display", "block")
            trINN.Style.Add("Display", "block")
            trSubmissionDesc.Style.Add("Display", "block")
        ElseIf ddlRegion.SelectedItem.Text = "ca" Or ddlRegion.SelectedItem.Text = "CA" Then
            lblDetailTag.Text = "Specify following details for CA-Regional"
            trPanel3.Style.Add("Display", "block")

            trApplicationNo.Style.Add("Display", "block")
            trProductName.Style.Add("Display", "block")
            trApplicant.Style.Add("Display", "block")
        End If

    End Sub

    Private Sub DisableControls()
        Try
            trPanel3.Style.Add("Display", "none")

            trApplicationNo.Style.Add("Display", "none")
            trCompanyName.Style.Add("Display", "none")
            trProductName.Style.Add("Display", "none")
            trProductType.Style.Add("Display", "none")
            trApplicationType.Style.Add("Display", "none")
            trRegionalVersion.Style.Add("Display", "none")
            trTrackingNo.Style.Add("Display", "none")
            trHighLevelNo.Style.Add("Display", "none")
            trApplicant.Style.Add("Display", "none")
            trProcedureType.Style.Add("Display", "none")
            trInventedName.Style.Add("Display", "none")
            trINN.Style.Add("Display", "none")
            trSubmissionDesc.Style.Add("Display", "none")
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Private Sub SetSavedValues()
        Dim ds_SubmittedProject As New DataSet
        Dim ds_usermst As New DataSet
        Dim estr As String = ""
        Dim wstr As String = ""

        Try
            ds_SubmittedProject = ViewState(VS_MyProject_Edit)

            Me.ddlCountry.Items.Clear()
            Me.ddlCountry.Items.Insert(0, New ListItem("Select Country", ""))
            Me.ddlCountry.Items.Insert(1, New ListItem(ds_SubmittedProject.Tables(0).Rows(0)("vCountryName"), ds_SubmittedProject.Tables(0).Rows(0)("vCountryCode")))
            Me.ddlCountry.SelectedIndex = 1
            Me.ddlCountry.Enabled = False
            Me.ddlRegion.Enabled = False
            Me.ddlCountry_SelectedIndexChanged(ddlRegion, New EventArgs())

            ''''''''Textbox'''''''''
            Me.txtApplicationNumber.Text = ds_SubmittedProject.Tables(0).Rows(0)("vApplicationNo").ToString
            Me.txtCompanyName.Text = ds_SubmittedProject.Tables(0).Rows(0)("vCompanyName").ToString
            Me.txtProductName.Text = ds_SubmittedProject.Tables(0).Rows(0)("vProductName").ToString
            Me.txtTrackingNumber.Text = ds_SubmittedProject.Tables(0).Rows(0)("vTrackingNo").ToString
            Me.txtHighLevelNumber.Text = ds_SubmittedProject.Tables(0).Rows(0)("vHighLvlNo").ToString
            Me.txtApplicant.Text = ds_SubmittedProject.Tables(0).Rows(0)("vApplicant").ToString
            Me.txtInventedName.Text = ds_SubmittedProject.Tables(0).Rows(0)("vInventedName").ToString
            Me.txtINN.Text = ds_SubmittedProject.Tables(0).Rows(0)("vInn").ToString

            '''''''Dropdown''''''''''''
            If ddlRegion.SelectedItem.Text = "US" Then
                Me.ddlProductType.SelectedValue = ds_SubmittedProject.Tables(0).Rows(0)("vProductType").ToString
                Me.ddlApplicationType.SelectedValue = ds_SubmittedProject.Tables(0).Rows(0)("vApplicationType").ToString
            ElseIf ddlRegion.SelectedItem.Text = "EU" Then
                Me.ddlProcedureType.SelectedValue = ds_SubmittedProject.Tables(0).Rows(0)("vProcedureType").ToString
                Me.ddlProcedureType_SelectedIndexChanged(ddlProcedureType, New EventArgs())
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "Assign Values"

    Private Function AssignValuesCMS(ByVal mode As String) As Boolean
        Dim dr As DataRow = Nothing
        Dim dt_CMS As New DataTable
        Dim ds_CMS As New DataSet
        Dim estr As String = Nothing
        Dim wstr As String = Nothing

        Try
            If Not objHelp.WorkspaceCMSMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_CMS, estr) Then
                Throw New Exception(estr)
            End If

            dt_CMS = ds_CMS.Tables(0)
            dr = dt_CMS.NewRow()
            dr("iWorkspaceCMSId") = "01"
            dr("vWorkspaceId") = Me.HProjectId.Value.Trim()
            dr("vCountryCode") = ddlCMSCountryName.SelectedItem.Value
            dr("vAgencyCode") = ddlCMSAgencyName.SelectedItem.Value
            dr("iWaveNo") = Convert.ToInt32(txtWave.Text)
            dr("dModifyOn") = System.DateTime.Now()
            dr("iModifyBy") = Convert.ToInt32(Me.Session(S_UserID))
            dr("cStatusIndi") = "N"
            dr("vCMSTrackingNo") = txtCMSTrackingNumber.Text
            dt_CMS.Rows.Add(dr)
            dt_CMS.AcceptChanges()

            If Not objLambda.Save_WorkspaceCMSMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_CMS, Me.Session(S_UserID), estr) Then
                Return False
            End If

            Me.BindGrid()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
            Exit Function
        End Try
    End Function

    Private Function AssignValuesSubmission() As Boolean
        Dim dr As DataRow = Nothing
        Dim dt_Submission As New DataTable
        Dim ds_Submission As New DataSet
        Dim estr As String = Nothing
        Dim wstr As String = Nothing

        Try
            If (ddlRegion.SelectedItem.Text.ToUpper = "US") Then
                If Not objHelp.GetSubmissioninfoUSMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_Submission, estr) Then
                    Return False
                End If

                dt_Submission = ds_Submission.Tables(0)
                dr = dt_Submission.NewRow()
                dr("vSubmissionInfoUSId") = "0000"
                dr("vWorkspaceId") = Me.HProjectId.Value.Trim()
                dr("vApplicationNo") = txtApplicationNumber.Text
                dr("vCountryCode") = ddlCountry.SelectedItem.Value
                dr("vAgencyName") = ddlAgencyName.SelectedItem.Text
                dr("vCompanyName") = txtCompanyName.Text
                dr("dDateOfSubmission") = System.DateTime.Now()
                dr("vProductName") = txtProductName.Text
                dr("vProductType") = ddlProductType.SelectedItem.Text
                dr("vApplicationType") = ddlApplicationType.SelectedItem.Text
                dr("iModifyBy") = Convert.ToInt32(Me.Session(S_UserID))
                dr("dModifyOn") = System.DateTime.Now()
                dr("cStatusIndi") = "N"
                dt_Submission.Rows.Add(dr)
                dt_Submission.AcceptChanges()

                If Not objLambda.Save_submissioninfoUSMSt(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Submission, Me.Session(S_UserID), estr) Then
                    Throw New Exception(estr)
                End If

            ElseIf (ddlRegion.SelectedItem.Text.ToUpper = "EU") Then
                If Not objHelp.GetSubmissioninfoEU14Mst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_Submission, estr) Then
                    Return False
                End If

                dt_Submission = ds_Submission.Tables(0)
                dr = dt_Submission.NewRow()
                dr("vSubmissionInfoEU14Id") = "0000"
                dr("vWorkspaceId") = Me.HProjectId.Value.Trim()
                dr("vTrackingNo") = txtTrackingNumber.Text
                dr("vHighLvlNo") = txtHighLevelNumber.Text
                dr("vCountryCode") = ddlCountry.SelectedItem.Value
                dr("vApplicant") = txtApplicant.Text
                dr("vAgencyName") = ddlAgencyName.SelectedItem.Text
                dr("vProcedureType") = ddlProcedureType.SelectedItem.Text
                dr("vInventedName") = txtInventedName.Text
                dr("vInn") = txtINN.Text
                dr("iModifyBy") = Convert.ToInt32(Me.Session(S_UserID))
                dr("dModifyOn") = System.DateTime.Now()
                dr("cStatusIndi") = "N"
                dt_Submission.Rows.Add(dr)
                dt_Submission.AcceptChanges()

                If Not objLambda.Save_submissioninfoEU14MSt(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Submission, Me.Session(S_UserID), estr) Then
                    Throw New Exception(estr)
                End If

            ElseIf (ddlRegion.SelectedItem.Text.ToUpper = "CA") Then
                If Not objHelp.GetSubmissioninfoCAMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_Submission, estr) Then
                    Return False
                End If

                dt_Submission = ds_Submission.Tables(0)
                dr = dt_Submission.NewRow()
                dr("vSubmissionInfoCAId") = "0000"
                dr("vWorkspaceId") = Me.HProjectId.Value.Trim()
                dr("vApplicationNo") = txtApplicationNumber.Text
                dr("vCountryCode") = ddlCountry.SelectedItem.Value
                dr("vAgencyName") = ddlAgencyName.SelectedItem.Text
                dr("dDateOfSubmission") = System.DateTime.Now()
                dr("vApplicant") = txtApplicant.Text
                dr("vProductName") = txtProductName.Text
                dr("iModifyBy") = Convert.ToInt32(Me.Session(S_UserID))
                dr("dModifyOn") = System.DateTime.Now()
                dr("cStatusIndi") = "N"
                dt_Submission.Rows.Add(dr)
                dt_Submission.AcceptChanges()

                If Not objLambda.Save_submissioninfoCAMSt(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Submission, Me.Session(S_UserID), estr) Then
                    Throw New Exception(estr)
                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Function

#End Region

#Region "Reset Page"

    Private Sub ResetCMSControls()
        Try
            txtWave.Text = "1"
            ddlCMSCountryName.SelectedIndex = 0
            ddlCMSAgencyName.SelectedIndex = 0
            txtCMSTrackingNumber.Text = ""
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Private Sub ResetSubmissionControls()
        Try
            Me.lblDetailsEditedBy.Text = ""
            Me.lblDetailsEditedOn.Text = ""

            Me.ddlRegion.Items.Clear()
            Me.ddlCountry.Items.Clear()
            Me.ddlAgencyName.Items.Clear()
            Me.txtApplicationNumber.Text = ""
            Me.txtTrackingNumber.Text = ""
            Me.txtCompanyName.Text = ""
            Me.txtProductName.Text = ""
            'Me.ddlProductType.Items.Clear()
            'Me.ddlApplicationType.Items.Clear()
            Me.txtHighLevelNumber.Text = ""
            Me.txtApplicant.Text = ""
            'Me.ddlProcedureType.Items.Clear()
            Me.txtInventedName.Text = ""
            Me.txtINN.Text = ""
            Me.txtSubmissionDesc.Text = ""

            Me.lblTemplateName.Text = ""
            Me.lblDetailsEditedBy.Text = ""
            Me.lblDetailsEditedOn.Text = ""
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "Button Events"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim ds_SearchProjects As New DataSet
        Dim ds_SubmittedProject As New DataSet
        Dim Wstr As String = ""
        Dim estr As String = ""

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Wstr = "iUserId=" + Me.Session(S_UserID) + " and vworkspaceid = '" + Me.HProjectId.Value.Trim + "'"

            If Not objHelp.View_MyProjects(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                ds_SearchProjects, eStr_Retu) Then
                Throw New Exception("Error displaying projects:" & eStr_Retu.ToString())
            End If

            ViewState(VS_MyProject) = ds_SearchProjects

            If Not ds_SearchProjects.Tables(0).Rows.Count <= 0 Then
                trMainContainer.Style.Add("Display", "block")
                trPanel2.Style.Add("Display", "block")
                trButtons.Style.Add("Display", "block")
            Else
                trMainContainer.Style.Add("Display", "none")
                trPanel2.Style.Add("Display", "none")
                trButtons.Style.Add("Display", "none")
            End If


            Wstr = Nothing
            Wstr = "vworkspaceid = '" + Me.HProjectId.Value.Trim + "'"

            If Not objHelp.view_AllWorkspaceSubmissionInfo(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubmittedProject, eStr_Retu) Then
                Throw New Exception(estr)
            End If

            If ds_SubmittedProject.Tables(0).Rows.Count > 0 Then
                ResetSubmissionControls()
                ViewState(VS_MyProject_Edit) = ds_SubmittedProject
                FillProjectDtl()
                FillRegionDropDown_Edit()
                SetSavedValues()
            Else
                '''''First Submission''''
                ResetSubmissionControls()
                trPanel2.Style.Add("Display", "block")
                trPanel3.Style.Add("Display", "none")
                trRegionalVersion.Style.Add("display", "none")
                trApplicationNo.Style.Add("display", "none")
                trTrackingNo.Style.Add("display", "none")
                trButtons.Style.Add("Display", "block")

                ddlRegion.Enabled = True
                ddlCountry.Enabled = True

                If ddlAgencyName.Items.Count > 0 Then
                    ddlCountry.Items.Clear()
                    ddlAgencyName.Items.Clear()
                End If

                FillProjectDtl()
                FillRegionDropDown()

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub ddlRegion_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRegion.SelectedIndexChanged
        Dim ds_SubmittedProject As New DataSet
        Try
            ds_SubmittedProject = ViewState(VS_MyProject_Edit)
            FillCountryDropDown()
            SetVisibleControls()
            If ddlRegion.Items.Count > 0 And Not ds_SubmittedProject Is Nothing Then
                ddlCountry.SelectedIndex = 1
                Me.ddlCountry_SelectedIndexChanged(ddlCountry, New EventArgs())
            End If

            'ddlAgencyName.SelectedIndex = 0
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub ddlCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCountry.SelectedIndexChanged
        Dim ds_SubmittedProject As New DataSet
        Dim Agency As String
        Try
            ds_SubmittedProject = ViewState(VS_MyProject_Edit)
            ddlAgencyName.Items.Clear()
            FillAgencyDropDown()
            ddlAgencyName.SelectedIndex = 0


            If Not ds_SubmittedProject Is Nothing Then
                Agency = ds_SubmittedProject.Tables(0).Rows(0)("vAgencyName").ToString
                ddlAgencyName.SelectedIndex = ddlAgencyName.Items.IndexOf(ddlAgencyName.Items.FindByText(Agency))
                If ds_SubmittedProject.Tables(0).Rows(0)("CountryRegionName").ToString.ToUpper = "EU" Then
                    ddlRegionalVersion.SelectedIndex = 1
                End If

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub ddlProcedureType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProcedureType.SelectedIndexChanged
        Try
            tdCMSDetails.Style.Add("Display", "none")
            If ddlProcedureType.SelectedItem.Text = "mutual-recognition" Then
                tdCMSDetails.Style.Add("Display", "block")
            ElseIf ddlProcedureType.SelectedItem.Text = "decentralised" Then
                tdCMSDetails.Style.Add("Display", "block")
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub ddlCMSCountryName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCMSCountryName.SelectedIndexChanged
        Try
            FillCMSAgencyDropDown()
            Me.MPECMS.Show()
            ddlCMSAgencyName.SelectedIndex = 0
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnCMSDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCMSDetails.Click
        Try
            Me.BindGrid()
            Me.MPECMS.Show()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnAddCMS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddCMS.Click
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim ds_WorkspaceCMS As New DataSet
        Dim estr As String
        Dim wstr As String
        Try
            Me.MPECMS.Show()
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""


            '''''''''' Checking that Country is already available or not. '''''''''
            wstr = "vWorkspaceId ='" + Me.HProjectId.Value.Trim() + "' AND cStatusIndi <> '" + "D" + "'"
            If Not objHelp.view_WorkspaceCmsMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_WorkspaceCMS, estr) Then
                Throw New Exception(estr)
            End If

            For Each dr As DataRow In ds_WorkspaceCMS.Tables(0).Rows
                If (dr("vCountryCode") = ddlCMSCountryName.SelectedItem.Value.Trim) Then
                    ObjCommon.ShowAlert("Country is already selected", Me.Page)
                    Exit Sub
                End If
            Next

            Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            Me.AssignValuesCMS(Choice)

            Me.ResetCMSControls()
            Me.MPECMS.Show()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Me.AssignValuesSubmission()
            Me.ResetSubmissionControls()
            'Me.btnSetProject_Click(BtnSave, New EventArgs())
            divCMS.Style.Add("Display", "none")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "message", "alert('Submission Details Saved Successfully..');location.href = 'frmProjectSubmissionDtl.aspx?mode=1';", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

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

