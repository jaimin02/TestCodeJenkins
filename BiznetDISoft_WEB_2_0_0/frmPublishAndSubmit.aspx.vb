Partial Class frmPublishAndSubmit
    Inherits System.Web.UI.Page


#Region "VARIABLE DECLARATIONS"

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
    Private objPublish As Publish.WS_KnowledgeNETPublish = ObjCommon.GetKnowledgeNETPublishRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_Project As String = "Project"
    Private Const VS_PublishableStructure As String = "PublishableStructure"

    'CMS Countries Grid
    'iWorkspaceCMSId, vWorkspaceId, vCountryCode, vCountryName, vCMSTrackingNo
    Private Const GVC_Select As Integer = 0
    Private Const GVC_iWorkspaceCMSId As Integer = 1
    Private Const GVC_vWorkspaceId As Integer = 2
    Private Const GVC_vCountryCode As Integer = 3
    Private Const GVC_vCountryName As Integer = 4
    Private Const GVC_vCMSTrackingNo As Integer = 5

    'Attribute Specific Parent Node's Attributes Grid
    'vWorkspaceId,iNodeId,iAttrId,vNodeName,vAttrName,vAttrValue,cAttrForIndi,vAttrType
    Private Const GVC_vWorkspaceIdAttr As Integer = 1
    Private Const GVC_iNodeId As Integer = 2
    Private Const GVC_iAttrId As Integer = 3
    Private Const GVC_vNodeName As Integer = 4
    Private Const GVC_vNodeDisplayName As Integer = 5
    Private Const GVC_vAttrName As Integer = 6
    Private Const GVC_vAttrValue As Integer = 7
    Private Const GVC_cAttrForIndi As Integer = 8
    Private Const GVC_vAttrType As Integer = 9

    'Published Project Grid
    'SrNO,SubmissionInfoEU14DtlId,WorkspaceId,CurrentSeqNumber,RelatedSeqNo,SubmissionType,
    'Details,View XML,Broken Links,Validate,Recompile,Confirm,Publish Path,Size,Delete
    Private Const GVC_SrNO As Integer = 0
    Private Const GVC_SubmissionInfoEU14DtlId As Integer = 1
    Private Const GVC_SubmissionInfoUSDtlId As Integer = 2
    Private Const GVC_SubmissionInfoCADtlId As Integer = 3
    Private Const GVC_WorkspaceId As Integer = 4
    Private Const GVC_CurrentSeqNumber As Integer = 5
    Private Const GVC_RelatedSeqNo As Integer = 6
    Private Const GVC_SubmissionType As Integer = 7
    Private Const GVC_Details As Integer = 8
    Private Const GVC_ViewXML As Integer = 9
    Private Const GVC_BrokenLinks As Integer = 10
    Private Const GVC_Validate As Integer = 11
    Private Const GVC_Recompile As Integer = 12
    Private Const GVC_Confirm As Integer = 13
    Private Const GVC_PublishPath As Integer = 14
    Private Const GVC_Size As Integer = 15
    Private Const GVC_Delete As Integer = 16
    Private Const GVC_ConfirmValue As Integer = 17
    Private Const GVC_StatusIndi As Integer = 18



#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim estr As String = Nothing
        Dim ds As New DataSet
        Try

            If Not IsPostBack Then
                CType(Me.Master.FindControl("lblHeading"), Label).Text = "Project Publish & Submission"

                If Not GenCall_ShowUI() Then
                    Exit Sub
                End If

                Choice = Me.Request.QueryString("Mode")
                Me.ViewState(VS_Choice) = Choice   'To use it while saving
                If Not Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                    GenCall()
                Else
                    FillProjectddl()
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

            GenCall_ShowUI()
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
            Page.Title = ":: Project Publish & Submission  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblMandatory"), Label).Visible = False
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Project Publish & Submission"

            'added by Deepak Singh on 2-Mar-10 to show the set Project on page load
            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                'Me.txtproject.Text = Session(S_ProjectName)
                'Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
                GenCall_ShowUI = True
                Exit Function
            End If

            '==added on 15-jan-2010 by deepak singh to show project according to user
            ' Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
            '========

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "Fill Functions"

    Private Function FillProjectddl() As Boolean
        Dim ds_Project As New DataSet
        Dim dv_Project As New DataView
        Dim estr As String
        Dim wstr As String

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            wstr = "iuserCode=" + Me.Session(S_UserID)
            If Not objHelp.view_AllWorkspaceSubmissionInfo(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Project, estr) Then
                Return False
            End If

            dv_Project = ds_Project.Tables(0).DefaultView
            Me.ddlProject.DataSource = dv_Project

            Me.ddlProject.DataValueField = "vWorkSpaceId"
            Me.ddlProject.DataTextField = "vWorkSpaceDesc"
            Me.ddlProject.DataBind()
            Me.ddlProject.Items.Insert(0, New ListItem("Select Project", ""))

            'Added Tooltip
            For iddlProject As Integer = 0 To ddlProject.Items.Count - 1
                ddlProject.Items(iddlProject).Attributes.Add("title", ddlProject.Items(iddlProject).Text)
            Next

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function FillSubmissionType(ByVal CountryRegion As String) As Boolean
        Dim ds_SubmissionType As New DataSet
        Dim dv_SubmissionType As New DataView
        Dim ds_Region As New DataSet
        Dim estr As String
        Dim wstr As String

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            wstr = "vRegionName='" + CountryRegion + "'"
            If Not objHelp.getregionmst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Region, estr) Then
                Throw New Exception(estr)
            End If

            wstr = Nothing
            wstr = "vCountryRegionId='" + ds_Region.Tables(0).Rows(0)("vRegionCode").ToString + "'"

            If Not objPublish.GetSubmissionTypeMst(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubmissionType, estr) Then
                Return False
            End If

            dv_SubmissionType = ds_SubmissionType.Tables(0).DefaultView
            Me.ddlSubmissionType.DataSource = dv_SubmissionType

            Me.ddlSubmissionType.DataValueField = "vSubmissionType"
            Me.ddlSubmissionType.DataTextField = "vSubmissionType"
            Me.ddlSubmissionType.DataBind()
            'Me.ddlSubmissionType.Items.Insert(0, New ListItem("Select Submission Type", ""))

            'Added Tooltip
            For iddlSubmissionType As Integer = 0 To ddlSubmissionType.Items.Count - 1
                ddlSubmissionType.Items(iddlSubmissionType).Attributes.Add("title", ddlSubmissionType.Items(iddlSubmissionType).Text)
            Next

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function FillCMSGrid() As Boolean
        Dim ds_CMS As New DataSet
        Dim dv_CMS As New DataView
        Dim ds_Project As New DataSet
        Dim estr As String
        Dim wstr As String

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            pnlgvCMS.Height = 150
            ds_Project = ViewState(VS_Project)

            wstr = "vWorkspaceId='" + ds_Project.Tables(0).Rows(0)("vWorkSpaceId").ToString + "'"

            If Not objPublish.View_GetCMSCountry(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CMS, estr) Then
                Throw New Exception(estr)
            End If

            tdCMSGrid.Style.Add("Display", "none")
            tdCMSNotFound.Style.Add("Display", "none")

            If Not ds_CMS.Tables(0).Rows.Count <= 0 Then
                tdCMSGrid.Style.Add("Display", "block")
            Else
                tdCMSNotFound.Style.Add("Display", "block")
                Exit Function
            End If

            For Each dr As DataRow In ds_CMS.Tables(0).Rows
                If dr("vCMSTrackingNo") = "" Then
                    dr("vCMSTrackingNo") = ds_Project.Tables(0).Rows(0)("vTrackingNo").ToString
                End If
            Next

            ds_CMS.AcceptChanges()
            dv_CMS = ds_CMS.Tables(0).DefaultView
            gvCMS.DataSource = dv_CMS
            Me.gvCMS.DataBind()
            Me.gvCMS.Dispose()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function FillAttrGrid(ByVal ds_ParentNodeAttributes As DataSet) As Boolean
        Dim dv_ParentNodeAttributes As DataView
        Dim height As String
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            dv_ParentNodeAttributes = ds_ParentNodeAttributes.Tables(0).DefaultView
            GvAttributes.DataSource = ds_ParentNodeAttributes
            Me.GvAttributes.DataBind()
            Me.GvAttributes.Dispose()

            height = GvAttributes.Height.ToString
            'divAttrGrid.Style.Add("Height", "150px")
            'If GvAttributes.Rows.Count < 6 Then
            '    divAttrGrid.Style.Add("Height", "150px")
            '    pnlAttrGrid.Height = "140px"
            'End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    ' For EU14 Dtl... '''
    Private Function FillGridProjectGrid(ByVal ds_SubInfoDtl As DataSet) As Boolean
        Dim dv_SubInfoDtl As New DataView
        Try
            If HProjectRegion.Value.ToUpper.Trim = "EU" Then
                ds_SubInfoDtl.Tables(0).Columns.Add("submissioninfoCAdtlId", GetType(String))
                ds_SubInfoDtl.Tables(0).Columns.Add("submissioninfoUSdtlId", GetType(String))
            ElseIf HProjectRegion.Value.ToUpper.Trim = "US" Then
                ds_SubInfoDtl.Tables(0).Columns.Add("submissioninfoEU14dtlId", GetType(String))
                ds_SubInfoDtl.Tables(0).Columns.Add("submissioninfoCAdtlId", GetType(String))
            ElseIf HProjectRegion.Value.ToUpper.Trim = "CA" Then
                ds_SubInfoDtl.Tables(0).Columns.Add("submissioninfoEU14dtlId", GetType(String))
                ds_SubInfoDtl.Tables(0).Columns.Add("submissioninfoUSdtlId", GetType(String))
            End If
            ds_SubInfoDtl.AcceptChanges()
            dv_SubInfoDtl = ds_SubInfoDtl.Tables(0).DefaultView
            HttpContext.Current.Items.Add("SubInfoDtl", dv_SubInfoDtl.ToTable)
            gvPublishedProject.DataSource = dv_SubInfoDtl
            gvPublishedProject.DataBind()

            '''''' Hiding grid controls after confirm '''''''
            HideGridControlsAfterConfirm()

            gvPublishedProject.Dispose()
            trpublishedProject.Style.Add("Display", "block")

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "Grid Events"

    Protected Sub gvCMS_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCMS.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Or e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(GVC_iWorkspaceCMSId).Visible = False
                e.Row.Cells(GVC_vWorkspaceId).Visible = False
                e.Row.Cells(GVC_vCountryCode).Visible = False
            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("ChkMove"), CheckBox).Attributes.Add("onclick", "ChkEnableTextbox('" + CType(e.Row.FindControl("ChkMove"), CheckBox).ClientID + "','" + CType(e.Row.FindControl("txtCMSTrackingNumber"), TextBox).ClientID + "');")
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub GvAttributes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvAttributes.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Or e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(GVC_vWorkspaceIdAttr).Visible = False
                e.Row.Cells(GVC_iNodeId).Visible = False
                e.Row.Cells(GVC_iAttrId).Visible = False
                e.Row.Cells(GVC_cAttrForIndi).Visible = False
                e.Row.Cells(GVC_vAttrType).Visible = False
                'e.Row.Cells(GVC_vNodeDisplayName).Visible = False
            End If


            If e.Row.RowType = DataControlRowType.DataRow Then
                If CType(e.Row.FindControl("txtAttrValue"), TextBox).Text = "" Then
                    CType(e.Row.FindControl("ImgAttrStatus"), Image).ImageUrl = "~/images/Wrong.png"
                Else
                    CType(e.Row.FindControl("ImgAttrStatus"), Image).ImageUrl = "~/images/Correct.png"
                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub gvPublishedProject_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPublishedProject.RowCommand
        Dim index As Integer = e.CommandArgument
        Dim wstr As String = Nothing
        Dim estr As String = Nothing
        Dim SubmissionId As String = Nothing

        Try
            If HProjectRegion.Value.ToUpper.Trim = "EU" Then
                SubmissionId = Me.gvPublishedProject.Rows(index).Cells(GVC_SubmissionInfoEU14DtlId).Text
            ElseIf HProjectRegion.Value.ToUpper.Trim = "US" Then
                SubmissionId = Me.gvPublishedProject.Rows(index).Cells(GVC_SubmissionInfoUSDtlId).Text
            ElseIf HProjectRegion.Value.ToUpper.Trim = "CA" Then
                SubmissionId = Me.gvPublishedProject.Rows(index).Cells(GVC_SubmissionInfoCADtlId).Text
            End If

            If e.CommandName.ToUpper = "MYCONFIRM" Then
                confirmSubmission(SubmissionId)
            ElseIf e.CommandName.ToUpper = "MYRECOMPILE" Then
                getSubmissionDataForRecompile(SubmissionId)
            ElseIf e.CommandName.ToUpper = "MYDETAILS" Then
                displaySubmissionDetailsDiv(SubmissionId)
            ElseIf e.CommandName.ToUpper = "MYSIZE" Then
                CType(Me.gvPublishedProject.Rows(index).Cells(GVC_SubmissionInfoCADtlId).FindControl("lblSize"), Label).Text = getFolder(SubmissionId)
                CType(Me.gvPublishedProject.Rows(index).Cells(GVC_SubmissionInfoCADtlId).FindControl("lblSize"), Label).Visible = True
                CType(Me.gvPublishedProject.Rows(index).Cells(GVC_SubmissionInfoCADtlId).FindControl("lnkSize"), LinkButton).Visible = False
            ElseIf e.CommandName.ToUpper = "MYDELETE" Then
                DeleteSubmission(SubmissionId)
            End If

            ''''' Hidding Controls from Grid After Confirm '''''
            HideGridControlsAfterConfirm()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub gvPublishedProject_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPublishedProject.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Or e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(GVC_WorkspaceId).Visible = False
                e.Row.Cells(GVC_ConfirmValue).Visible = False
                e.Row.Cells(GVC_StatusIndi).Visible = False
                If HProjectRegion.Value.ToUpper.Trim = "EU" Then
                    e.Row.Cells(GVC_SubmissionInfoCADtlId).Visible = False
                    e.Row.Cells(GVC_SubmissionInfoUSDtlId).Visible = False
                ElseIf HProjectRegion.Value.ToUpper.Trim = "US" Then
                    e.Row.Cells(GVC_SubmissionInfoEU14DtlId).Visible = False
                    e.Row.Cells(GVC_SubmissionInfoCADtlId).Visible = False
                ElseIf HProjectRegion.Value.ToUpper.Trim = "CA" Then
                    e.Row.Cells(GVC_SubmissionInfoEU14DtlId).Visible = False
                    e.Row.Cells(GVC_SubmissionInfoUSDtlId).Visible = False
                End If
            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("ImgDetails"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgDetails"), ImageButton).CommandName = "MYDETAILS"

                CType(e.Row.FindControl("ImgViewXML"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgViewXML"), ImageButton).CommandName = "MYVIEWXML"

                CType(e.Row.FindControl("ImgBrokenLinks"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgBrokenLinks"), ImageButton).CommandName = "MYBROKENLINKS"

                CType(e.Row.FindControl("ImgValidate"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgValidate"), ImageButton).CommandName = "MYVALIDATE"

                CType(e.Row.FindControl("ImgRecompile"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgRecompile"), ImageButton).CommandName = "MYRECOMPILE"

                CType(e.Row.FindControl("lnkConfirm"), LinkButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkConfirm"), LinkButton).CommandName = "MYCONFIRM"

                CType(e.Row.FindControl("ImgPublishPath"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgPublishPath"), ImageButton).CommandName = "MYPUBLISHPATH"

                CType(e.Row.FindControl("lnkSize"), LinkButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkSize"), LinkButton).CommandName = "MYSIZE"

                CType(e.Row.FindControl("ImgDelete"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgDelete"), ImageButton).CommandName = "MYDELETE"

                '''''''' Checking that submission is Confirmed or not '''''''''
                If (e.Row.Cells(GVC_ConfirmValue).Text.ToUpper = "Y") Then
                    CType(e.Row.FindControl("lblConfirmed"), Label).Visible = True
                    CType(e.Row.FindControl("lnkConfirm"), LinkButton).Visible = False
                    CType(e.Row.FindControl("ImgRecompile"), ImageButton).Visible = False
                    CType(e.Row.FindControl("ImgDelete"), ImageButton).Visible = False
                End If


                '''''''' Checking that the submission is deleted or not '''''''''
                If (e.Row.Cells(GVC_StatusIndi).Text.ToUpper = "D") Then
                    e.Row.BackColor = Drawing.Color.Pink
                    CType(e.Row.FindControl("ImgViewXML"), ImageButton).Visible = False
                    CType(e.Row.FindControl("ImgBrokenLinks"), ImageButton).Visible = False
                    CType(e.Row.FindControl("ImgValidate"), ImageButton).Visible = False
                    CType(e.Row.FindControl("lblConfirmed"), Label).Visible = False
                    CType(e.Row.FindControl("ImgRecompile"), ImageButton).Visible = False
                    CType(e.Row.FindControl("lnkConfirm"), LinkButton).Visible = False
                    CType(e.Row.FindControl("lnkSize"), LinkButton).Visible = False
                    CType(e.Row.FindControl("ImgPublishPath"), ImageButton).Visible = False
                    CType(e.Row.FindControl("ImgDelete"), ImageButton).Visible = False
                    e.Row.Height = 33
                End If


                '''''''''' Adding mouse hover event on grid to change the color of current row '''''''''
                If e.Row.RowType = DataControlRowType.DataRow Then
                    If e.Row.Cells(GVC_StatusIndi).Text.Trim <> "D" Then
                        If e.Row.RowState = DataControlRowState.Alternate Then
                            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#FFFFE1';")
                            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF';")
                        Else
                            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#FFFFE1';")
                            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#CEE3ED';")
                        End If
                    End If
                End If
                e.Row.Cells(GVC_SrNO).Text = e.Row.RowIndex + (gvCMS.PageSize * gvCMS.PageIndex) + 1
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "Publish Grid Related Functions"

#Region " Confirm "
    Private Function confirmSubmission(ByVal SubmissionId As String) As Boolean
        'Dim ds_submitedProject As New DataSet
        Dim ds_Project As New DataSet
        Dim ds_InternalLabelMst As New DataSet
        Dim ds_SubmissionNodeDtl As New DataSet
        Dim ds As New DataSet
        Dim ds_WorkspaceMst As New DataSet
        Dim ds_SubmissionDtl As New DataSet
        Dim ds_SubmittedWsNdDetail As New DataSet
        Dim dr_SubmittedWsNdDetail As DataRow
        Dim estr As String = Nothing
        Dim wstr As String = Nothing
        Dim wsId As String = Nothing
        Dim str As String = Nothing
        Dim currentSubmissionLabelId As String = Nothing
        Dim labelNo As Integer = Nothing

        Try
            wsId = ddlProject.SelectedItem.Value

            If HProjectRegion.Value.ToUpper.Trim = "EU" Then
                ''''''''''''''''' Getting Record From SubmissionDtl Table ''''''''''''''''''
                wstr = "SubmissionInfoEU14DtlId='" + SubmissionId + "'"
                If Not objPublish.getWorkspaceSubmissionInfoEU14DtlBySubmissionId(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubmissionDtl, wstr) Then
                    Throw New Exception(estr)
                End If
            ElseIf HProjectRegion.Value.ToUpper.Trim = "US" Then
                ''''''''''''''''' Getting Record From SubmissionDtl Table ''''''''''''''''''
                wstr = "SubmissionInfoUSDtlId='" + SubmissionId + "'"
                If Not objPublish.getWorkspaceSubmissionInfoUSDtlBySubmissionId(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubmissionDtl, wstr) Then
                    Throw New Exception(estr)
                End If
            ElseIf HProjectRegion.Value.ToUpper.Trim = "CA" Then
                ''''''''''''''''' Getting Record From SubmissionDtl Table ''''''''''''''''''
                wstr = "SubmissionInfoCADtlId='" + SubmissionId + "'"
                If Not objPublish.getWorkspaceSubmissionInfoCADtlBySubmissionId(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubmissionDtl, wstr) Then
                    Throw New Exception(estr)
                End If
            End If


            '''''' Getting All the labels from internalLabelmst for specific project ''''''
            currentSubmissionLabelId = ds_SubmissionDtl.Tables(0).Rows(0)("LabelId").ToString
            If Not objPublish.Proc_ViewInternalLabel(wsId, ds_InternalLabelMst, estr) Then
                Throw New Exception(estr)
            End If

            '''''''''' Selecting Currrent Label No from Internallabelmst''''''''''''''
            For Each dr As DataRow In ds_InternalLabelMst.Tables(0).Rows
                If (dr("labelId").ToString = currentSubmissionLabelId) Then
                    labelNo = Convert.ToInt32(dr("labelNo").ToString)
                End If
            Next

            ''''''''' Getting Published Nodes for selected Label No '''''''''''''
            If Not objPublish.proc_SubmissionNodeDtl(wsId, labelNo, ds_SubmissionNodeDtl, estr) Then
                Throw New Exception(estr)
            End If

            ''''''''' Updating vLastPublishedVersion in "WorkspaceMst" ''''''''''''''
            ''''''''' Procedure doesn't return dataset but 'Ds' is passed optionally '''''''''
            If Not objPublish.proc_publishVersion(wsId, ds, estr) Then
                Throw New Exception(estr)
            End If

            ''''''''' Updating values in "SubmissionInfoEU14Dtl" table ''''''''''''
            wstr = "vWorkspaceId='" + wsId + "'"
            If Not objHelp.getworkspacemst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_WorkspaceMst, estr) Then
                Throw New Exception(estr)
            End If

            If HProjectRegion.Value.ToUpper.Trim = "EU" Then
                ds_SubmissionDtl.Tables(0).Rows(0)("SubmissionInfoEU14DtlId") = SubmissionId
                ds_SubmissionDtl.Tables(0).Rows(0)("Confirm") = "Y"
                ds_SubmissionDtl.Tables(0).Rows(0)("LastPublishedVersion") = ds_WorkspaceMst.Tables(0).Rows(0)("vLastPublishedVersion").ToString
                ds_SubmissionDtl.Tables(0).Rows(0)("SubmitedBy") = Me.Session(S_UserID)
                ds_SubmissionDtl.AcceptChanges()

                If Not objPublish.Insert_SubmissionInfoEU14Dtl(str, Publish.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_SubmissionDtl.Tables(0), estr) Then
                    Throw New Exception(estr)
                End If
            ElseIf HProjectRegion.Value.ToUpper.Trim = "US" Then
                ds_SubmissionDtl.Tables(0).Rows(0)("SubmissionInfoUSDtlId") = SubmissionId
                ds_SubmissionDtl.Tables(0).Rows(0)("Confirm") = "Y"
                ds_SubmissionDtl.Tables(0).Rows(0)("LastPublishedVersion") = ds_WorkspaceMst.Tables(0).Rows(0)("vLastPublishedVersion").ToString
                ds_SubmissionDtl.Tables(0).Rows(0)("SubmitedBy") = Me.Session(S_UserID)
                ds_SubmissionDtl.AcceptChanges()

                If Not objPublish.Insert_SubmissionInfoUSDtl(str, Publish.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_SubmissionDtl.Tables(0), estr) Then
                    Throw New Exception(estr)
                End If
            ElseIf HProjectRegion.Value.ToUpper.Trim = "CA" Then
                ds_SubmissionDtl.Tables(0).Rows(0)("SubmissionInfoCADtlId") = SubmissionId
                ds_SubmissionDtl.Tables(0).Rows(0)("Confirm") = "Y"
                ds_SubmissionDtl.Tables(0).Rows(0)("LastPublishedVersion") = ds_WorkspaceMst.Tables(0).Rows(0)("vLastPublishedVersion").ToString
                ds_SubmissionDtl.Tables(0).Rows(0)("SubmitedBy") = Me.Session(S_UserID)
                ds_SubmissionDtl.AcceptChanges()

                If Not objPublish.Insert_SubmissionInfoCADtl(str, Publish.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_SubmissionDtl.Tables(0), estr) Then
                    Throw New Exception(estr)
                End If
            End If

            ''''''''' Storing values in "SubmittedWorkspaceNodeDetail" table ''''''''''''
            If Not objPublish.getsubmittedWorkspacenodedetail("", Publish.DataRetrievalModeEnum.DataTable_Empty, ds_SubmittedWsNdDetail, estr) Then
                Throw New Exception(estr)
            End If

            If (ds_SubmissionNodeDtl.Tables(0).Rows.Count > 0) Then
                For Each dr As DataRow In ds_SubmissionNodeDtl.Tables(0).Rows
                    dr_SubmittedWsNdDetail = ds_SubmittedWsNdDetail.Tables(0).NewRow()
                    dr_SubmittedWsNdDetail("vworkspaceId") = wsId.ToString.Trim
                    dr_SubmittedWsNdDetail("iNodeId") = dr("iNodeId").ToString.Trim
                    dr_SubmittedWsNdDetail("vLastPublishVersion") = ds_WorkspaceMst.Tables(0).Rows(0)("vLastPublishedVersion").ToString.Trim
                    dr_SubmittedWsNdDetail("submissionId") = SubmissionId.ToString.Trim
                    dr_SubmittedWsNdDetail("indexId") = "node-" + dr("iNodeId").ToString.Trim
                    ds_SubmittedWsNdDetail.Tables(0).Rows.Add(dr_SubmittedWsNdDetail)
                Next
            End If
            ds_SubmittedWsNdDetail.AcceptChanges()

            If Not objPublish.Insert_SubmittedWorkspaceNodeDetail(Publish.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_SubmittedWsNdDetail, Me.Session(S_UserID), estr) Then
                Throw New Exception(estr)
            End If

            ObjCommon.ShowAlert("Your Submission has been confirmed", Me.Page)


            ''''''' For ReBuild Grid after Submission '''
            displayProjectSubmissionDtl()

            ''''''' Hiding Section after confirm submission as Prevalidaition ''''''''
            trProjectDetail.Style.Add("display", "none")
            trAttribute.Style.Add("display", "none")
            trSubmissionEntry.Style.Add("display", "none")
            trButtons.Style.Add("display", "none")

            'ddlProject_SelectedIndexChanged(ddlProject, Nothing)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function
#End Region

#Region " Recompile "

    Private Function getSubmissionDataForRecompile(ByVal SubmissionId As String) As Boolean
        Dim ds_AllWsSubInfo As New DataSet
        Dim ds_SubInfoDtl As New DataSet
        Dim estr As String = Nothing
        Dim wstr As String = Nothing
        Dim wsId As String = Nothing

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            wsId = ddlProject.SelectedItem.Value
            wstr = "vWorkSpaceId='" + wsId + "'"
            If Not objHelp.view_AllWorkspaceSubmissionInfo(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_AllWsSubInfo, estr) Then
                Throw New Exception(estr)
            End If

            If (HProjectRegion.Value.ToString.ToUpper = "EU") Then
                wstr = "SubmissionInfoEU14DtlId='" + SubmissionId + "'"
                If Not objPublish.getWorkspaceSubmissionInfoEU14DtlBySubmissionId(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubInfoDtl, estr) Then
                    Throw New Exception(estr)
                End If
                txtNewDos.Text = Convert.ToDateTime(ds_SubInfoDtl.Tables(0).Rows(0)("DateOfSubmission").ToString).ToString("dd-MMM-yyyy")
            ElseIf (HProjectRegion.Value.ToString.ToUpper = "US") Then
                wstr = "SubmissionInfoUSDtlId='" + SubmissionId + "'"
                If Not objPublish.getWorkspaceSubmissionInfoUSDtlBySubmissionId(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubInfoDtl, estr) Then
                    Throw New Exception(estr)
                End If
                txtNewDos.Text = Convert.ToDateTime(ds_SubInfoDtl.Tables(0).Rows(0)("DateOfSubmission").ToString).ToString("dd-MMM-yyyy")
            ElseIf (HProjectRegion.Value.ToString.ToUpper = "CA") Then
                wstr = "SubmissionInfoCADtlId='" + SubmissionId + "'"
                If Not objPublish.getWorkspaceSubmissionInfoCADtlBySubmissionId(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubInfoDtl, estr) Then
                    Throw New Exception(estr)
                End If
                txtNewDos.Text = Convert.ToDateTime(ds_SubInfoDtl.Tables(0).Rows(0)("DateOfSubmission").ToString).ToString("dd-MMM-yyyy")
            End If
            HSubmissionPath.Value = ds_SubInfoDtl.Tables(0).Rows(0)("SubmissionPath").ToString + "/" + _
                                                 ds_SubInfoDtl.Tables(0).Rows(0)("CurrentSeqNumber").ToString

            HSubmissionId.Value = SubmissionId.ToString

            mpeRecompile.Show()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function recompileSubmission() As Boolean
        Dim ds_commonWsDtl As New DataSet
        Dim ds_AllWsSubmission As New DataSet
        Dim ds_SubmissionDtl As New DataSet
        Dim ds_WorkspaceLabel As New DataSet
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim wsId As String = String.Empty
        Dim relatedSeqs As String = ""
        Dim submissionType As String = String.Empty
        Dim lastPublishedVersion As String = String.Empty
        Dim currentSeqNumber As String = String.Empty
        Dim subPath As String = String.Empty
        Dim isRMSSelected As String = ""
        Dim addTT As String = ""
        Dim indexXMLFilePath As String = String.Empty
        Dim commonFolderPath As String = String.Empty
        Dim RegionName As String = Nothing
        Dim destinationWorkspaceFolderPath As String = String.Empty
        Dim publishDestinationPath As String = String.Empty
        Dim sourcePath As String = String.Empty
        Dim subVariationMode As String = String.Empty
        Dim defaultStageId As Integer
        Dim newLabelId As String = String.Empty
        Dim ParamOut As String = String.Empty
        Dim directory As DirectoryInfo
        Dim dos As DateTime
        Dim subType As String = String.Empty


        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            wsId = ddlProject.SelectedItem.Value.ToString
            dos = Convert.ToDateTime(Request.Form("ctl00$CPHLAMBDA$txtNewDos"))

            'Source IndexXML File Path
            indexXMLFilePath = HSubmissionPath.Value.ToString.Trim + "/index.xml"

            'Destination workspace folder Path
            wstr = "vWorkspaceId='" + wsId + "'"
            If Not objPublish.view_commonworkspaceDetail(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_commonWsDtl, estr) Then
                Throw New Exception(estr)
            End If
            destinationWorkspaceFolderPath = ds_commonWsDtl.Tables(0).Rows(0)("vBaseWorkFolder").ToString.Trim + "/" + wsId.Trim
            sourcePath = ds_commonWsDtl.Tables(0).Rows(0)("vBaseWorkFolder").ToString.Trim


            ''''''''' Getting Selected Project from view_AllWorkspaceSubmissionInfo ''''''''
            wstr = "vWorkspaceId='" + wsId + "'"
            If Not objHelp.view_AllWorkspaceSubmissionInfo(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_AllWsSubmission, estr) Then
                Throw New Exception(estr)
            End If
            RegionName = ds_AllWsSubmission.Tables(0).Rows(0)("CountryRegionName").ToString.ToUpper().Trim

            'Set stageId and copy files to workspace with all database history entries
            defaultStageId = 100    'All recompile nodes' files will be Approved
            copyPublishedFilesToWorkspace(indexXMLFilePath, destinationWorkspaceFolderPath, Me.Session(S_UserID), defaultStageId, wsId)

            'Creating label for workspace
            If Not objPublish.getcreateWorkspaceLabel(wsId, Me.Session(S_UserID).ToString, ds_WorkspaceLabel, estr) Then
                Throw New Exception(estr)
            End If
            newLabelId = ds_WorkspaceLabel.Tables(0).Rows(0)("LabelId").ToString.Trim



            'Updating labelId for recompiled submission
            If (RegionName = "EU") Then
                wstr = "SubmissionInfoEU14DtlId='" + HSubmissionId.Value.ToString + "'"
                If Not objPublish.getWorkspaceSubmissionInfoEU14DtlBySubmissionId(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubmissionDtl, estr) Then
                    Throw New Exception(estr)
                End If
                publishDestinationPath = ds_SubmissionDtl.Tables(0).Rows(0)("SubmissionPath").ToString.Trim
                subVariationMode = ds_SubmissionDtl.Tables(0).Rows(0)("SubVariationMode").ToString.Trim
                subType = ds_SubmissionDtl.Tables(0).Rows(0)("SubmissionType").ToString.Trim

                ds_SubmissionDtl.Tables(0).Rows(0)("LabelId") = newLabelId.Trim
                ds_SubmissionDtl.Tables(0).Rows(0)("SubmitedBy") = Me.Session(S_UserID).ToString.Trim
                ds_SubmissionDtl.Tables(0).Rows(0)("ModifyBy") = Me.Session(S_UserID).ToString.Trim
                ds_SubmissionDtl.Tables(0).Rows(0)("DateOfSubmission") = Convert.ToDateTime(Request.Form("ctl00$CPHLAMBDA$txtNewDos"))
                ds_SubmissionDtl.AcceptChanges()

                If Not objPublish.Insert_SubmissionInfoEU14Dtl(ParamOut, Publish.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_SubmissionDtl.Tables(0), estr) Then
                    Throw New Exception(estr)
                End If

                relatedSeqs = ds_SubmissionDtl.Tables(0).Rows(0)("RelatedSeqNo").ToString
                submissionType = ds_SubmissionDtl.Tables(0).Rows(0)("SubmissionType").ToString
                lastPublishedVersion = ds_SubmissionDtl.Tables(0).Rows(0)("LastPublishedVersion").ToString
                currentSeqNumber = ds_SubmissionDtl.Tables(0).Rows(0)("CurrentSeqNumber").ToString
                subPath = ds_SubmissionDtl.Tables(0).Rows(0)("SubmissionPath").ToString
                isRMSSelected = ds_SubmissionDtl.Tables(0).Rows(0)("RMSSubmited").ToString

                '**********************************************************************************/
                '   find tracking table xml in already published dossier and if it exists
                '   then again add it to the recompiled dossier.

                commonFolderPath = HSubmissionPath.Value.ToString + "/m1/eu/10-cover/common"

                directory = New DirectoryInfo(commonFolderPath)

                If directory.Exists Then
                    For Each file As FileInfo In directory.GetFiles
                        If file.Exists() Then
                            If (file.Name.Contains("tracking") And file.Name.Contains(".xml") And Not file Is Nothing) Then
                                addTT = "Y"
                            End If
                        End If
                    Next
                End If
                '**********************************************************************************/

            ElseIf (RegionName = "US") Then
                wstr = "SubmissionInfoUSDtlId='" + HSubmissionId.Value.ToString + "'"
                If Not objPublish.getWorkspaceSubmissionInfoUSDtlBySubmissionId(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubmissionDtl, estr) Then
                    Throw New Exception(estr)
                End If
                publishDestinationPath = ds_SubmissionDtl.Tables(0).Rows(0)("SubmissionPath").ToString.Trim
                subType = ds_SubmissionDtl.Tables(0).Rows(0)("SubmissionType").ToString.Trim

                ds_SubmissionDtl.Tables(0).Rows(0)("LabelId") = newLabelId
                ds_SubmissionDtl.Tables(0).Rows(0)("SubmitedBy") = Me.Session(S_UserID)
                ds_SubmissionDtl.Tables(0).Rows(0)("ModifyBy") = Me.Session(S_UserID)
                ds_SubmissionDtl.Tables(0).Rows(0)("DateOfSubmission") = Convert.ToDateTime(Request.Form("ctl00$CPHLAMBDA$txtNewDos"))
                ds_SubmissionDtl.AcceptChanges()

                If Not objPublish.Insert_SubmissionInfoUSDtl(ParamOut, Publish.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_SubmissionDtl.Tables(0), estr) Then
                    Throw New Exception(estr)
                End If

                relatedSeqs = ds_SubmissionDtl.Tables(0).Rows(0)("RelatedSeqNo").ToString
                submissionType = ds_SubmissionDtl.Tables(0).Rows(0)("SubmissionType").ToString
                lastPublishedVersion = ds_SubmissionDtl.Tables(0).Rows(0)("LastPublishedVersion").ToString
                currentSeqNumber = ds_SubmissionDtl.Tables(0).Rows(0)("CurrentSeqNumber").ToString
                subPath = ds_SubmissionDtl.Tables(0).Rows(0)("SubmissionPath").ToString
                addTT = "N"
                isRMSSelected = "N"
            ElseIf (RegionName = "CA") Then
                wstr = "SubmissionInfoCADtlId='" + HSubmissionId.Value.ToString + "'"
                If Not objPublish.getWorkspaceSubmissionInfoCADtlBySubmissionId(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubmissionDtl, estr) Then
                    Throw New Exception(estr)
                End If
                publishDestinationPath = ds_SubmissionDtl.Tables(0).Rows(0)("SubmissionPath").ToString.Trim
                subType = ds_SubmissionDtl.Tables(0).Rows(0)("SubmissionType").ToString.Trim

                ds_SubmissionDtl.Tables(0).Rows(0)("LabelId") = newLabelId
                ds_SubmissionDtl.Tables(0).Rows(0)("SubmitedBy") = Me.Session(S_UserID)
                ds_SubmissionDtl.Tables(0).Rows(0)("ModifyBy") = Me.Session(S_UserID)
                ds_SubmissionDtl.Tables(0).Rows(0)("DateOfSubmission") = Convert.ToDateTime(Request.Form("ctl00$CPHLAMBDA$txtNewDos"))
                ds_SubmissionDtl.AcceptChanges()

                If Not objPublish.Insert_SubmissionInfoCADtl(ParamOut, Publish.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_SubmissionDtl.Tables(0), estr) Then
                    Throw New Exception(estr)
                End If

                relatedSeqs = ds_SubmissionDtl.Tables(0).Rows(0)("RelatedSeqNo").ToString
                submissionType = ds_SubmissionDtl.Tables(0).Rows(0)("SubmissionType").ToString
                lastPublishedVersion = ds_SubmissionDtl.Tables(0).Rows(0)("LastPublishedVersion").ToString
                currentSeqNumber = ds_SubmissionDtl.Tables(0).Rows(0)("CurrentSeqNumber").ToString
                subPath = ds_SubmissionDtl.Tables(0).Rows(0)("SubmissionPath").ToString
                addTT = "N"
                isRMSSelected = "N"
            End If

            ds_AllWsSubmission.Tables(0).Columns.Add("SubmissionId", GetType(String))
            ds_AllWsSubmission.Tables(0).Rows(0)("SubmissionId") = HSubmissionId.Value.ToString.Trim
            ds_AllWsSubmission.AcceptChanges()

            If Not objPublish.publishWorkspaceSubmission(ds_AllWsSubmission.Tables(0), ds_WorkspaceLabel.Tables(0), _
                                currentSeqNumber, relatedSeqs, publishDestinationPath, sourcePath, _
                                isRMSSelected, subVariationMode, addTT, Nothing, dos, Me.Session(S_UserID), subType, sourcePath, estr) Then
                Throw New Exception(estr)
            End If

            ''''''' For ReBuild Grid after Submission '''
            displayProjectSubmissionDtl()

            ObjCommon.ShowAlert("Submission Recompiled Successfully...", Me.Page)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function copyPublishedFilesToWorkspace(ByVal indexXMLFilePath As String, _
                                                   ByVal WorkspacePath As String, _
                                                   ByVal userId As Integer, _
                                                   ByVal defaultStageId As Integer, _
                                                   ByVal wsId As String) As Boolean
        Dim ds_leaf As New DataSet
        Dim ds_WsNodeHistory As New DataSet
        Dim dr_WsNodeHistory As DataRow
        Dim publishPath As String = Nothing
        Dim nodeId As String = Nothing
        Dim file As FileInfo
        Dim iTranNo As Integer
        Dim FolderPath As String = Nothing
        Dim FilePath As String = Nothing
        Dim estr As String = Nothing
        Dim wstr As String = Nothing
        Dim rowCount As Integer = 0

        Try
            copyPublishedFilesToWorkspace = False
            If Not readXMLAndGetFilePaths(indexXMLFilePath, ds_leaf) Then
                Throw New Exception("Error While Reading paths from Index.xml in readXMLAndGetFilePaths() Function")
            End If

            '''''' Getting Blank Structure of NodeHistory Table and Assigning Values to it '''''
            If Not objHelp.getWorkspaceNodeHistory("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_WsNodeHistory, estr) Then
                Throw New Exception(estr)
            End If

            For Each dr As DataRow In ds_leaf.Tables("leaf").Rows
                publishPath = dr("href").ToString.Trim
                nodeId = dr("nodeId").ToString.Trim

                file = New FileInfo(publishPath)
                If file.Exists Then
                    createFolderStruc(file, nodeId, WorkspacePath, wsId, iTranNo)
                    FolderPath = "/" + wsId + "/" + nodeId.ToString + "/" + iTranNo.ToString
                    FilePath = WorkspacePath.Remove(WorkspacePath.LastIndexOf("/"))

                    'Do While (rowCount < ds_leaf.Tables("leaf").Rows.Count)
                    dr_WsNodeHistory = ds_WsNodeHistory.Tables(0).NewRow
                    dr_WsNodeHistory("vWorkSpaceId") = wsId.Trim
                    dr_WsNodeHistory("iNodeId") = Convert.ToInt32(nodeId)
                    dr_WsNodeHistory("iTranNo") = iTranNo
                    dr_WsNodeHistory("vFileName") = file.Name.ToString.Trim
                    dr_WsNodeHistory("vFileType") = file.Extension.ToString
                    dr_WsNodeHistory("vFolderName") = FolderPath.Trim
                    dr_WsNodeHistory("cRequiredFlag") = "Y"
                    dr_WsNodeHistory("iStageId") = defaultStageId
                    dr_WsNodeHistory("vRemark") = ""
                    dr_WsNodeHistory("iModifyBy") = Me.Session(S_UserID)
                    dr_WsNodeHistory("cStatusIndi") = "N"
                    dr_WsNodeHistory("vFilePath") = FilePath + FolderPath + "/" + file.Name
                    dr_WsNodeHistory("vDefaultFileFormat") = ""
                    ds_WsNodeHistory.Tables(0).Rows.Add(dr_WsNodeHistory)

                    '''' Storing Data in WorkspaceNodeAttrHistory ''''
                    updateNodeAttrHistory(wsId, dr_WsNodeHistory("inodeId"), dr_WsNodeHistory("iTranNo"))
                    ds_WsNodeHistory.AcceptChanges()
                End If
            Next

            ''''' Saving Data in workspaceNodeHistory ''''''
            If ds_WsNodeHistory.Tables(0).Rows.Count > 0 Then
                If Not objLambda.Save_WorkspaceNodeHistory_ECTD(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_WsNodeHistory, Me.Session(S_UserID), estr, iTranNo) Then
                    Throw New Exception(estr)
                End If
            End If

            copyPublishedFilesToWorkspace = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function readXMLAndGetFilePaths(ByVal indexXMLFilePath As String, _
                                            ByRef ds_IndexLeaf As DataSet) As Boolean
        Dim ds_RegionalLeaf As New DataSet
        Dim strid As String = Nothing
        Dim RegionalXMLFilePath As String = Nothing
        Dim estr As String = Nothing
        Dim RegionalFilePath As String = Nothing
        Dim rowCount As Integer = 0

        Try
            readXMLAndGetFilePaths = False
            '''''''''' Reading Index.xml '''''''''''
            ds_IndexLeaf.ReadXml(indexXMLFilePath)

            '*************************************************************************************
            '''''''''' Reading Regional.xml ''''''''''
            For Each dr In ds_IndexLeaf.Tables("leaf").Rows
                If dr("href").ToString.EndsWith(".xml") Then
                    RegionalXMLFilePath = HSubmissionPath.Value + "/" + dr("href").ToString
                    RegionalFilePath = dr("href").ToString.Remove(dr("href").ToString.LastIndexOf("/"))
                    ds_RegionalLeaf.ReadXml(RegionalXMLFilePath)
                End If
            Next

            If Not ds_RegionalLeaf.Tables("leaf") Is Nothing Then
                For Each dr In ds_RegionalLeaf.Tables("leaf").Rows
                    dr("href") = RegionalFilePath + "/" + dr("href").ToString
                    ds_RegionalLeaf.AcceptChanges()
                Next
            End If
            '*************************************************************************************

            ''''''''' Merge DataTable of ds_RegionalLeaf In ds_IndexLeaf  '''''''''''
            If Not ds_RegionalLeaf.Tables("leaf") Is Nothing Then
                ds_IndexLeaf.Tables("leaf").Merge(ds_RegionalLeaf.Tables("leaf"))
            End If


            '*************************************************************************************
            ' Added New Column which only stores id of the node
            ' And stores a full path of the node in the href column

            ds_IndexLeaf.Tables("leaf").Columns.Add("nodeId", GetType(String))

            If ds_IndexLeaf.Tables("leaf").Rows.Count > 0 Then
                For Each dr In ds_IndexLeaf.Tables("leaf").Rows
                    strid = dr("id").ToString.Split("-")(1).ToString
                    dr("nodeId") = strid
                    dr("href") = HSubmissionPath.Value + "/" + dr("href").ToString
                Next
                ds_IndexLeaf.Tables("leaf").AcceptChanges()
            End If
            '*************************************************************************************

            ' Removing rows of .xml file from dataset
            Do While (rowCount < ds_IndexLeaf.Tables("leaf").Rows.Count)
                If ds_IndexLeaf.Tables("leaf").Rows(rowCount)("href").ToString.EndsWith(".xml") Then
                    ds_IndexLeaf.Tables("leaf").Rows.RemoveAt(rowCount)
                End If
                rowCount += 1
                ds_IndexLeaf.Tables("leaf").AcceptChanges()
            Loop

            readXMLAndGetFilePaths = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function createFolderStruc(ByVal file As FileInfo, ByVal nodeId As String, _
                                       ByVal WorkspacePath As String, _
                                       ByVal wsId As String, _
                                       ByRef iTranNo As Integer) As Boolean
        Dim ds_WorkspaceDtlEctd As New DataSet
        Dim estr As String = Nothing
        Dim wstr As String = Nothing
        Dim dir As DirectoryInfo
        Dim TranNo As Integer

        Try
            If Not file Is Nothing And file.Exists Then

                wstr = "vWorkspaceid='" + wsId + "'"
                If Not objPublish.GetWorkspaceDtlEctd(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_WorkspaceDtlEctd, estr) Then
                    Throw New Exception(estr)
                End If

                ''''' Updating iLastTranNo in workspaceDtlEctd table and retrieve it '''''
                If Not objPublish.Proc_NewTranNo(Publish.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_WorkspaceDtlEctd, Me.Session(S_UserID), estr, TranNo) Then
                    Throw New Exception(estr)
                End If

                ''''' Creating Directory in Workspace and copying file in it '''''
                WorkspacePath = WorkspacePath.Trim + "/" + nodeId + "/" + TranNo.ToString
                dir = New DirectoryInfo(WorkspacePath)
                If Not dir.Exists Then
                    dir.Create()
                    file.CopyTo(WorkspacePath + "/" + file.Name, True)
                End If

                '''''' Returning iTranNo to "copyPublishedFilesToWorkspace" function '''''
                iTranNo = TranNo
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function updateNodeAttrHistory(ByVal wsId As String, ByVal inodeId As Integer, _
                                            ByVal iTranNo As Integer) As Boolean
        Dim ds_WsNodeAttrDetail As New DataSet
        Dim ds_WsNodeAttrHistory As New DataSet
        Dim dr_WsNodeAttrHistory As DataRow
        Dim estr As String = Nothing
        Dim wstr As String = Nothing
        Try
            wstr = "vWorkspaceId = '" + wsId + "'"
            wstr += "AND " + "iNodeId = " + inodeId.ToString
            If Not objHelp.GetWorkspacenodeattrdetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_WsNodeAttrDetail, estr) Then
                Throw New Exception(estr)
            End If

            If Not objHelp.getWorkspaceNodeAttrHistory("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_WsNodeAttrHistory, estr) Then
                Throw New Exception(estr)
            End If

            'For Each dr_WsNodeAttrDetail As DataRow In ds_WsNodeAttrDetail.Tables(0).Rows
            dr_WsNodeAttrHistory = ds_WsNodeAttrHistory.Tables(0).NewRow
            dr_WsNodeAttrHistory("vWorkSpaceId") = wsId.ToString.Trim
            dr_WsNodeAttrHistory("iNodeId") = inodeId
            dr_WsNodeAttrHistory("iTranNo") = iTranNo
            dr_WsNodeAttrHistory("iAttrId") = ds_WsNodeAttrDetail.Tables(0).Rows(0)("iAttrId")
            dr_WsNodeAttrHistory("vAttrValue") = ds_WsNodeAttrDetail.Tables(0).Rows(0)("vAttrValue")
            dr_WsNodeAttrHistory("vRemark") = ds_WsNodeAttrDetail.Tables(0).Rows(0)("vRemark")
            dr_WsNodeAttrHistory("iStageId") = "12"
            dr_WsNodeAttrHistory("cStatusIndi") = "N"
            dr_WsNodeAttrHistory("iModifyBy") = Me.Session(S_UserID)
            dr_WsNodeAttrHistory("dModifyOn") = System.DateTime.Now.ToString
            ds_WsNodeAttrHistory.Tables(0).Rows.Add(dr_WsNodeAttrHistory)
            'Next
            ds_WsNodeAttrHistory.AcceptChanges()


            '''' NOTE :: Data is not actually storing in database '''
            ''''' Storing data in Workspacenodeattrhistory Table '''''''
            If Not objLambda.Insert_Workspacenodeattrhistory__ForEctd(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_WsNodeAttrHistory, Me.Session(S_UserID), estr) Then
                Throw New Exception(estr)
            End If


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

#End Region

#Region " Display Details "

    Private Function displaySubmissionDetailsDiv(ByVal SubmissionId As String) As Boolean
        Dim ds_allWsSubmissionInfo As New DataSet
        Dim estr As String = Nothing
        Dim wstr As String = Nothing
        Try
            mpeDetails.Show()
            HideControls_DetailDiv()
            ShowControls_DetailDiv()

            wstr = "vWorkSpaceId = '" + ddlProject.SelectedItem.Value.Trim + "'"
            If Not objHelp.view_AllWorkspaceSubmissionInfo(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_allWsSubmissionInfo, estr) Then
                Throw New Exception(estr)
            End If

            If HProjectRegion.Value.ToUpper.Trim = "EU" Then
                setValues_DetailDiv_EU(SubmissionId, ds_allWsSubmissionInfo)
            ElseIf HProjectRegion.Value.ToUpper.Trim = "US" Then
                setValues_DetailDiv_US(SubmissionId, ds_allWsSubmissionInfo)
            ElseIf HProjectRegion.Value.ToUpper.Trim = "CA" Then
                setValues_DetailDiv_CA(SubmissionId, ds_allWsSubmissionInfo)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function HideControls_DetailDiv() As Boolean
        Try
            trProjectName_dtl.Style.Add("Display", "none")
            trApplicationNo_dtl.Style.Add("Display", "none")
            trTrackingNo_Dtl.Style.Add("Display", "none")
            trCompanyName_dtl.Style.Add("Display", "none")
            trProductName_dtl.Style.Add("Display", "none")
            trProductType_dtl.Style.Add("Display", "none")
            trSubCountry_dtl.Style.Add("Display", "none")
            trAgency_dtl.Style.Add("Display", "none")
            trSubType_dtl.Style.Add("Display", "none")
            trSubSeq_dtl.Style.Add("Display", "none")
            trDos_dtl.Style.Add("Display", "none")
            trApplicationType_dtl.Style.Add("Display", "none")
            trSubmittedOn_dtl.Style.Add("Display", "none")
            trSubStatus_dtl.Style.Add("Display", "none")
            trSubMode_dtl.Style.Add("Display", "none")
            trRelatedSeq_dtl.Style.Add("Display", "none")
            trApplicant_dtl.Style.Add("Display", "none")
            trProcedureType_dtl.Style.Add("Display", "none")
            trInventedName_dtl.Style.Add("Display", "none")
            trInn_dtl.Style.Add("Display", "none")
            trSubDesc_dtl.Style.Add("Display", "none")
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function ShowControls_DetailDiv() As Boolean
        Try
            If HProjectRegion.Value.ToUpper.Trim = "EU" Then
                trProjectName_dtl.Style.Add("Display", "block")
                trTrackingNo_Dtl.Style.Add("Display", "block")
                trSubCountry_dtl.Style.Add("Display", "block")
                trAgency_dtl.Style.Add("Display", "block")
                trSubType_dtl.Style.Add("Display", "block")
                trSubSeq_dtl.Style.Add("Display", "block")
                trDos_dtl.Style.Add("Display", "block")
                trSubmittedOn_dtl.Style.Add("Display", "block")
                trSubStatus_dtl.Style.Add("Display", "block")
                trSubMode_dtl.Style.Add("Display", "block")
                trRelatedSeq_dtl.Style.Add("Display", "block")
                trApplicant_dtl.Style.Add("Display", "block")
                trProcedureType_dtl.Style.Add("Display", "block")
                trInventedName_dtl.Style.Add("Display", "block")
                trInn_dtl.Style.Add("Display", "block")
                'trSubDesc_dtl.Style.Add("Display", "block")
            ElseIf HProjectRegion.Value.ToUpper.Trim = "US" Then
                trProjectName_dtl.Style.Add("Display", "block")
                trApplicationNo_dtl.Style.Add("Display", "block")
                trCompanyName_dtl.Style.Add("Display", "block")
                trProductName_dtl.Style.Add("Display", "block")
                trProductType_dtl.Style.Add("Display", "block")
                trSubCountry_dtl.Style.Add("Display", "block")
                trAgency_dtl.Style.Add("Display", "block")
                trSubType_dtl.Style.Add("Display", "block")
                trSubSeq_dtl.Style.Add("Display", "block")
                trDos_dtl.Style.Add("Display", "block")
                trApplicationType_dtl.Style.Add("Display", "block")
                trSubmittedOn_dtl.Style.Add("Display", "block")
                trSubStatus_dtl.Style.Add("Display", "block")
                trSubMode_dtl.Style.Add("Display", "block")
                trRelatedSeq_dtl.Style.Add("Display", "block")
            ElseIf HProjectRegion.Value.ToUpper.Trim = "CA" Then
                trProjectName_dtl.Style.Add("Display", "block")
                trApplicationNo_dtl.Style.Add("Display", "block")
                trProductName_dtl.Style.Add("Display", "block")
                trSubCountry_dtl.Style.Add("Display", "block")
                trAgency_dtl.Style.Add("Display", "block")
                trSubType_dtl.Style.Add("Display", "block")
                trSubSeq_dtl.Style.Add("Display", "block")
                trDos_dtl.Style.Add("Display", "block")
                trSubmittedOn_dtl.Style.Add("Display", "block")
                trSubStatus_dtl.Style.Add("Display", "block")
                trSubMode_dtl.Style.Add("Display", "block")
                trRelatedSeq_dtl.Style.Add("Display", "block")
                trApplicant_dtl.Style.Add("Display", "block")
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function setValues_DetailDiv_EU(ByVal SubmissionId As String, ByVal ds_allWsSubmissionInfo As DataSet) As Boolean
        Dim ds_SubmissionDtl As New DataSet
        Dim ds_SubmissionSubDtl As New DataSet
        Dim estr As String = Nothing
        Dim wstr As String = Nothing
        Try
            wstr = "SubmissionInfoEU14DtlId = '" + SubmissionId + "'"
            If Not objPublish.getWorkspaceSubmissionInfoEU14DtlBySubmissionId(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubmissionDtl, estr) Then
                Throw New Exception(estr)
            End If

            lblProjectName_dtl.Text = ds_allWsSubmissionInfo.Tables(0).Rows(0)("vWorkSpaceDesc").ToString
            lblTrackingNo_dtl.Text = ds_allWsSubmissionInfo.Tables(0).Rows(0)("vTrackingNo").ToString
            lblSubCountry_dtl.Text = ds_allWsSubmissionInfo.Tables(0).Rows(0)("vCountryName").ToString
            lblAgency_dtl.Text = ds_allWsSubmissionInfo.Tables(0).Rows(0)("vAgencyName").ToString
            lblApplicant_dtl.Text = ds_allWsSubmissionInfo.Tables(0).Rows(0)("vApplicant").ToString
            lblProcedureType_dtl.Text = ds_allWsSubmissionInfo.Tables(0).Rows(0)("vProcedureType").ToString
            lblInventedName_dtl.Text = ds_allWsSubmissionInfo.Tables(0).Rows(0)("vInventedName").ToString
            lblInn_dtl.Text = ds_allWsSubmissionInfo.Tables(0).Rows(0)("vInn").ToString

            lblSubmittedOn_dtl.Text = ds_SubmissionDtl.Tables(0).Rows(0)("SubmitedOn").ToString
            lblSubType_dtl.Text = ds_SubmissionDtl.Tables(0).Rows(0)("SubmissionType").ToString
            lblSubSeq_dtl.Text = ds_SubmissionDtl.Tables(0).Rows(0)("CurrentSeqNumber").ToString
            lblDos_dtl.Text = Convert.ToDateTime(ds_SubmissionDtl.Tables(0).Rows(0)("DateOfSubmission").ToString).Date
            lblSubMode_dtl.Text = ds_SubmissionDtl.Tables(0).Rows(0)("SubmissionMode").ToString
            lblRelatedSeq_dtl.Text = ds_SubmissionDtl.Tables(0).Rows(0)("RelatedSeqNo").ToString

            If ds_SubmissionDtl.Tables(0).Rows(0)("RelatedSeqNo").ToString.Trim = "" Then
                lblRelatedSeq_dtl.Text = "-"
            End If

            If ds_SubmissionDtl.Tables(0).Rows(0)("Confirm").ToString.ToUpper = "Y" Then
                lblSubStatus_dtl.Text = "Confirmed"
            Else
                lblSubStatus_dtl.Text = "-"
            End If

            'wstr = "vSubmissionInfoEU14DtlId = '" + SubmissionId + "'"
            'If Not objPublish.getWorkspaceRMSSubmissionInfoEU14(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubmissionSubDtl, estr) Then
            '    Throw New Exception(estr)
            'End If
            'lblSubDesc_dtl.Text = ds_SubmissionSubDtl.Tables(0).Rows(0)("vSubmissionDescription").ToString()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function setValues_DetailDiv_US(ByVal SubmissionId As String, ByVal ds_allWsSubmissionInfo As DataSet) As Boolean
        Dim ds_SubmissionDtl As New DataSet
        Dim estr As String = Nothing
        Dim wstr As String = Nothing
        Try
            wstr = "SubmissionInfoUSDtlId = '" + SubmissionId + "'"
            If Not objPublish.getWorkspaceSubmissionInfoUSDtlBySubmissionId(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubmissionDtl, estr) Then
                Throw New Exception(estr)
            End If

            lblProjectName_dtl.Text = ds_allWsSubmissionInfo.Tables(0).Rows(0)("vWorkSpaceDesc").ToString
            lblApplicationNo_dtl.Text = ds_allWsSubmissionInfo.Tables(0).Rows(0)("vApplicationNo").ToString
            lblSubCountry_dtl.Text = ds_allWsSubmissionInfo.Tables(0).Rows(0)("vCountryName").ToString
            lblCompanyName_dtl.Text = ds_allWsSubmissionInfo.Tables(0).Rows(0)("vCompanyName").ToString
            lblProductName_dtl.Text = ds_allWsSubmissionInfo.Tables(0).Rows(0)("vProductName").ToString
            lblProductType_dtl.Text = ds_allWsSubmissionInfo.Tables(0).Rows(0)("vProductType").ToString
            lblAgency_dtl.Text = ds_allWsSubmissionInfo.Tables(0).Rows(0)("vAgencyName").ToString
            lblApplicationType_dtl.Text = ds_allWsSubmissionInfo.Tables(0).Rows(0)("vApplicationType").ToString

            lblSubType_dtl.Text = ds_SubmissionDtl.Tables(0).Rows(0)("SubmissionType").ToString
            lblSubSeq_dtl.Text = ds_SubmissionDtl.Tables(0).Rows(0)("CurrentSeqNumber").ToString()
            lblRelatedSeq_dtl.Text = ds_SubmissionDtl.Tables(0).Rows(0)("RelatedSeqNo").ToString()
            lblDos_dtl.Text = Convert.ToDateTime(ds_SubmissionDtl.Tables(0).Rows(0)("DateOfSubmission").ToString).Date
            lblSubmittedOn_dtl.Text = ds_SubmissionDtl.Tables(0).Rows(0)("SubmitedOn").ToString
            lblSubMode_dtl.Text = ds_SubmissionDtl.Tables(0).Rows(0)("SubmissionMode").ToString

            If ds_SubmissionDtl.Tables(0).Rows(0)("SubmissionMode").ToString.Trim = "" Then
                lblSubMode_dtl.Text = "-"
            End If

            If ds_SubmissionDtl.Tables(0).Rows(0)("Confirm").ToString.ToUpper = "Y" Then
                lblSubStatus_dtl.Text = "Confirmed"
            Else
                lblSubStatus_dtl.Text = "-"
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function setValues_DetailDiv_CA(ByVal SubmissionId As String, ByVal ds_allWsSubmissionInfo As DataSet) As Boolean
        Dim ds_SubmissionDtl As New DataSet
        Dim estr As String = Nothing
        Dim wstr As String = Nothing
        Try
            wstr = "SubmissionInfoCADtlId = '" + SubmissionId + "'"
            If Not objPublish.getWorkspaceSubmissionInfoCADtlBySubmissionId(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubmissionDtl, estr) Then
                Throw New Exception(estr)
            End If

            lblProjectName_dtl.Text = ds_allWsSubmissionInfo.Tables(0).Rows(0)("vWorkSpaceDesc").ToString
            lblApplicationNo_dtl.Text = ds_allWsSubmissionInfo.Tables(0).Rows(0)("vApplicationNo").ToString
            lblSubCountry_dtl.Text = ds_allWsSubmissionInfo.Tables(0).Rows(0)("vCountryName").ToString
            lblProductName_dtl.Text = ds_allWsSubmissionInfo.Tables(0).Rows(0)("vProductName").ToString
            lblProductType_dtl.Text = ds_allWsSubmissionInfo.Tables(0).Rows(0)("vProductType").ToString
            lblAgency_dtl.Text = ds_allWsSubmissionInfo.Tables(0).Rows(0)("vAgencyName").ToString
            lblApplicationType_dtl.Text = ds_allWsSubmissionInfo.Tables(0).Rows(0)("vApplicationType").ToString
            lblApplicant_dtl.Text = ds_allWsSubmissionInfo.Tables(0).Rows(0)("vApplicant").ToString

            lblSubType_dtl.Text = ds_SubmissionDtl.Tables(0).Rows(0)("SubmissionType").ToString
            lblSubSeq_dtl.Text = ds_SubmissionDtl.Tables(0).Rows(0)("CurrentSeqNumber").ToString()
            lblRelatedSeq_dtl.Text = ds_SubmissionDtl.Tables(0).Rows(0)("RelatedSeqNo").ToString()
            lblDos_dtl.Text = Convert.ToDateTime(ds_SubmissionDtl.Tables(0).Rows(0)("DateOfSubmission").ToString).Date
            lblSubmittedOn_dtl.Text = ds_SubmissionDtl.Tables(0).Rows(0)("SubmitedOn").ToString
            lblSubMode_dtl.Text = ds_SubmissionDtl.Tables(0).Rows(0)("SubmissionMode").ToString

            If ds_SubmissionDtl.Tables(0).Rows(0)("SubmissionMode").ToString.Trim = "" Then
                lblSubMode_dtl.Text = "-"
            End If

            If ds_SubmissionDtl.Tables(0).Rows(0)("Confirm").ToString.ToUpper = "Y" Then
                lblSubStatus_dtl.Text = "Confirmed"
            Else
                lblSubStatus_dtl.Text = "-"
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region " Get Size "

    Private Function getFolder(ByVal SubmissionId As String) As String
        Dim ds_SubInfoDtl As New DataSet
        Dim Size As Double = 0
        Dim StrSize As String = Nothing
        Dim estr As String = Nothing
        Dim wstr As String = Nothing
        Dim DirPath As String = Nothing
        Dim Rootdir As DirectoryInfo
        Try

            If (HProjectRegion.Value.ToString.ToUpper = "EU") Then
                wstr = "SubmissionInfoEU14DtlId='" + SubmissionId + "'"
                If Not objPublish.getWorkspaceSubmissionInfoEU14DtlBySubmissionId(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubInfoDtl, estr) Then
                    Throw New Exception(estr)
                End If
            ElseIf (HProjectRegion.Value.ToString.ToUpper = "US") Then
                wstr = "SubmissionInfoUSDtlId='" + SubmissionId + "'"
                If Not objPublish.getWorkspaceSubmissionInfoUSDtlBySubmissionId(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubInfoDtl, estr) Then
                    Throw New Exception(estr)
                End If
            ElseIf (HProjectRegion.Value.ToString.ToUpper = "CA") Then
                wstr = "SubmissionInfoCADtlId='" + SubmissionId + "'"
                If Not objPublish.getWorkspaceSubmissionInfoCADtlBySubmissionId(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubInfoDtl, estr) Then
                    Throw New Exception(estr)
                End If
            End If

            DirPath = ds_SubInfoDtl.Tables(0).Rows(0)("SubmissionPath").ToString + "/" + _
                                                 ds_SubInfoDtl.Tables(0).Rows(0)("CurrentSeqNumber").ToString

            Rootdir = New DirectoryInfo(DirPath)

            calculateSize(DirPath, Size)
            Size = Size / 1048576
            Size = System.Math.Round(Size, 2)
            StrSize = Size.ToString
            Return StrSize
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Function

    Private Function calculateSize(ByVal Path As String, ByRef Size As Long) As Boolean
        Dim dir As DirectoryInfo
        Dim file As FileInfo
        Try

            dir = New DirectoryInfo(Path)

            ''''''' Getting Sub Directories '''''''
            For Each subdir In dir.GetDirectories
                calculateSize(subdir.FullName, Size)
            Next

            ''''''' Getting File Size '''''''
            For Each file In dir.GetFiles()
                Size += file.Length
            Next

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region " Publish Path "

    Private Function openPublishPath(ByVal SubmissionId As String) As Boolean
        Try

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region " Delete "

    Private Function DeleteSubmission(ByVal SubmissionId As String) As Boolean
        Dim ds_SubmissionDtl As New DataSet
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Try
            If HProjectRegion.Value.ToUpper.Trim = "EU" Then
                ''''''''''''''''' Getting Record From SubmissionDtl Table ''''''''''''''''''
                wstr = "SubmissionInfoEU14DtlId='" + SubmissionId + "'"
                If Not objPublish.getWorkspaceSubmissionInfoEU14DtlBySubmissionId(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubmissionDtl, wstr) Then
                    Throw New Exception(estr)
                End If

                If Not objPublish.Insert_SubmissionInfoEU14Dtl(wstr, Publish.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, ds_SubmissionDtl.Tables(0), estr) Then
                    Throw New Exception(estr)
                End If
            ElseIf HProjectRegion.Value.ToUpper.Trim = "US" Then
                ''''''''''''''''' Getting Record From SubmissionDtl Table ''''''''''''''''''
                wstr = "SubmissionInfoUSDtlId='" + SubmissionId + "'"
                If Not objPublish.getWorkspaceSubmissionInfoUSDtlBySubmissionId(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubmissionDtl, wstr) Then
                    Throw New Exception(estr)
                End If

                If Not objPublish.Insert_SubmissionInfoUSDtl(wstr, Publish.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, ds_SubmissionDtl.Tables(0), estr) Then
                    Throw New Exception(estr)
                End If
            ElseIf HProjectRegion.Value.ToUpper.Trim = "CA" Then
                ''''''''''''''''' Getting Record From SubmissionDtl Table ''''''''''''''''''
                wstr = "SubmissionInfoCADtlId='" + SubmissionId + "'"
                If Not objPublish.getWorkspaceSubmissionInfoCADtlBySubmissionId(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubmissionDtl, wstr) Then
                    Throw New Exception(estr)
                End If

                If Not objPublish.Insert_SubmissionInfoCADtl(wstr, Publish.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, ds_SubmissionDtl.Tables(0), estr) Then
                    Throw New Exception(estr)
                End If
            End If

            ''''''' For ReBuild Grid after Submission '''
            displayProjectSubmissionDtl()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#End Region

#Region "Assign Value"

    Private Function AssignValuesEU14(ByVal ds_project As DataSet) As Boolean
        Dim ds_workspace As New DataSet
        Dim ds_Submission As New DataSet
        Dim estr As String
        Dim wstr As String
        Dim i As Integer = 0
        Dim count As Integer = 0
        Dim iRelatedCount As Integer = 0
        Dim element As Integer = 0
        Dim iSeqNo As Integer = 0
        Dim strSeqNo As String = Nothing

        Dim UserId As Integer
        Dim subType As String
        Dim workspaceId As String
        Dim applicationNumber As String
        Dim projectPublishType As Char
        Dim currentSeqNumber As String
        Dim lastPublishedVersion As String = "-999"
        Dim dos As Date
        Dim relatedSeqNumber As String = Nothing
        Dim subMode As String
        Dim isRMSSelected As Char
        Dim subVariationMode As String
        Dim addTT As Char                   '//N for CP and NP else Y
        Dim lastConfirmedSubmissionPath As String
        Dim baseWorkFolder As String
        Dim subDesc As String
        Dim selectedCMS(-1) As Integer
        Dim trackCMS(-1) As String

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            wstr = "vWorkSpaceId='" + ds_project.Tables(0).Rows(0)("vWorkSpaceId").ToString + "'"
            If Not objHelp.getworkspacemst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_workspace, estr) Then
                Throw New Exception(estr)
            End If

            UserId = Me.Session(S_UserID)
            subType = ddlSubmissionType.SelectedItem.Text
            workspaceId = ds_project.Tables(0).Rows(0)("vWorkSpaceId").ToString
            applicationNumber = txtTrackingNumber.Text.Trim
            projectPublishType = "P"

            '''''' Code for Current Sequence Number ''''''
            If ds_workspace.Tables(0).Rows(0)("vLastPublishedVersion").ToString.Trim = "-999" Then
                currentSeqNumber = "0000"       'Condition
            Else
                iSeqNo = Convert.ToInt32(ds_workspace.Tables(0).Rows(0)("vLastPublishedVersion").ToString.Trim)
                iSeqNo += 1
                strSeqNo = "000" + iSeqNo.ToString
                strSeqNo.Substring(strSeqNo.Length - 4)
                currentSeqNumber = strSeqNo
            End If

            '''''' Code for last Confirmed Submission Path ''''''''
            If currentSeqNumber = "0000" Then
                lastConfirmedSubmissionPath = ""
            Else
                wstr = "WorkspaceId = '" + workspaceId + "' AND Confirm = 'Y' order by submissioninfoEU14dtlId desc"
                If Not objPublish.getWorkspaceSubmissionInfoEU14DtlBySubmissionId(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Submission, estr) Then
                    Throw New Exception(estr)
                End If
                lastConfirmedSubmissionPath = ds_Submission.Tables(0).Rows(0)("SubmissionPath").ToString.Trim
            End If

            lastPublishedVersion = ds_workspace.Tables(0).Rows(0)("vLastPublishedVersion").ToString.Trim    'Condition
            dos = Convert.ToDateTime(Request.Form("ctl00$CPHLAMBDA$txtDos"))
            subMode = ddlTransamissionMedia.SelectedItem.Text
            isRMSSelected = rbRMSCountry.SelectedItem.Value

            '''''' Code for Selected Related Sequence Number ''''''
            'Condition
            iRelatedCount = 0
            relatedSeqNumber = ""
            Do While iRelatedCount < chkRelatedSeqNo.Items.Count
                If chkRelatedSeqNo.Items(iRelatedCount).Selected Then
                    relatedSeqNumber += chkRelatedSeqNo.Items(iRelatedCount).Text.ToString.Trim() + ","
                    'relatedSeqNumber = relatedSeqNumber.Remove(relatedSeqNumber.Length - 1)
                End If
                iRelatedCount += 1
            Loop
            If relatedSeqNumber.Length > 0 Then
                relatedSeqNumber = relatedSeqNumber.Remove(relatedSeqNumber.Length - 1)
            End If

            If ddlSubmissionMode.SelectedItem.Text = "Select Submission Mode" Then
                subVariationMode = ""
            Else
                subVariationMode = ddlSubmissionMode.SelectedItem.Text
            End If

            If (chkAddTrackingTable.Checked = True) Then
                addTT = "Y"
            Else
                addTT = "N"
            End If

            baseWorkFolder = ds_workspace.Tables(0).Rows(0)("vBaseWorkFolder").ToString
            subDesc = txtSubmissionDesc.Text.Trim

            If gvCMS.Rows.Count > 0 Then
                Do While i <= gvCMS.Rows.Count - 1
                    If CType(gvCMS.Rows(i).FindControl("ChkMove"), CheckBox).Checked = True Then
                        count += 1
                    End If
                    i = i + 1
                Loop

                ReDim trackCMS(count - 1)
                ReDim selectedCMS(count - 1)
                i = 0

                Do While i <= gvCMS.Rows.Count - 1
                    If CType(gvCMS.Rows(i).FindControl("ChkMove"), CheckBox).Checked = True Then
                        trackCMS(element) = CType(gvCMS.Rows(i).FindControl("txtCMSTrackingNumber"), TextBox).Text
                        selectedCMS(element) = Convert.ToInt32(gvCMS.Rows(i).Cells(GVC_iWorkspaceCMSId).Text)
                        element += 1
                    End If
                    i = i + 1
                Loop
            End If

            If Not objPublish.saveSubmissionDtl(UserId, subType, workspaceId, applicationNumber, _
                                         projectPublishType, currentSeqNumber, lastPublishedVersion, _
                                         dos, relatedSeqNumber, subMode, isRMSSelected, subVariationMode, _
                                         addTT, lastConfirmedSubmissionPath, baseWorkFolder, subDesc, _
                                         selectedCMS, trackCMS, estr) Then
                Throw New Exception(estr)
            End If

            ObjCommon.ShowAlert("Project Published Successfully", Me.Page)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function AssignValuesUS(ByVal ds_project As DataSet) As Boolean
        Dim ds_workspace As New DataSet
        Dim ds_Submission As New DataSet
        Dim estr As String
        Dim wstr As String
        Dim i As Integer = 0
        Dim count As Integer = 0
        Dim element As Integer = 0
        Dim iSeqNo As Integer = 0
        Dim strSeqNo As String = Nothing
        Dim iRelatedCount As Integer = 0

        Dim UserId As Integer
        Dim subType As String = ""
        Dim workspaceId As String = ""
        Dim applicationNumber As String = ""
        Dim projectPublishType As Char = ""
        Dim currentSeqNumber As String = ""
        Dim lastPublishedVersion As String = "-999"
        Dim dos As Date
        Dim relatedSeqNumber As String = ""
        Dim subMode As String = ""
        Dim isRMSSelected As Char = "N"
        Dim subVariationMode As String = ""
        Dim addTT As Char = "N"              '//N for CP and NP else Y"
        Dim lastConfirmedSubmissionPath As String = ""
        Dim baseWorkFolder As String = ""
        Dim subDesc As String = ""
        Dim selectedCMS(-1) As Integer
        Dim trackCMS(-1) As String
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            wstr = "vWorkSpaceId='" + ds_project.Tables(0).Rows(0)("vWorkSpaceId").ToString + "'"
            If Not objHelp.getworkspacemst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_workspace, estr) Then
                Throw New Exception(estr)
            End If

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            wstr = "vWorkSpaceId='" + ds_project.Tables(0).Rows(0)("vWorkSpaceId").ToString + "'"
            If Not objHelp.getworkspacemst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_workspace, estr) Then
                Throw New Exception(estr)
            End If


            UserId = Me.Session(S_UserID)
            subType = ddlSubmissionType.SelectedItem.Text
            workspaceId = ds_project.Tables(0).Rows(0)("vWorkSpaceId").ToString
            applicationNumber = txtApplicationNumber.Text.Trim
            projectPublishType = "P"

            '''''' Code for Current Sequence Number ''''''
            If ds_workspace.Tables(0).Rows(0)("vLastPublishedVersion").ToString.Trim = "-999" Then
                currentSeqNumber = "0000"       'Condition
            Else
                iSeqNo = Convert.ToInt32(ds_workspace.Tables(0).Rows(0)("vLastPublishedVersion").ToString.Trim)
                iSeqNo += 1
                strSeqNo = "000" + iSeqNo.ToString
                strSeqNo.Substring(strSeqNo.Length - 4)
                currentSeqNumber = strSeqNo
            End If

            '''''' Code for last Confirmed Submission Path ''''''''
            If currentSeqNumber = "0000" Then
                lastConfirmedSubmissionPath = ""
            Else
                wstr = "WorkspaceId = '" + workspaceId + "' AND Confirm = 'Y' order by submissioninfoUSdtlId desc"
                If Not objPublish.getWorkspaceSubmissionInfoUSDtlBySubmissionId(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Submission, estr) Then
                    Throw New Exception(estr)
                End If
                lastConfirmedSubmissionPath = ds_Submission.Tables(0).Rows(0)("SubmissionPath").ToString.Trim
            End If

            lastPublishedVersion = ds_workspace.Tables(0).Rows(0)("vLastPublishedVersion").ToString.Trim    'Condition
            dos = Convert.ToDateTime(Request.Form("ctl00$CPHLAMBDA$txtDos"))
            subMode = ddlTransamissionMedia.SelectedItem.Text

            '''''' Code for Selected Related Sequence Number '''''''Condition
            iRelatedCount = 0
            relatedSeqNumber = ""
            Do While iRelatedCount < chkRelatedSeqNo.Items.Count
                If chkRelatedSeqNo.Items(iRelatedCount).Selected Then
                    relatedSeqNumber += chkRelatedSeqNo.Items(iRelatedCount).Text.ToString.Trim() + ","
                    'relatedSeqNumber = relatedSeqNumber.Remove(relatedSeqNumber.Length - 1)
                End If
                iRelatedCount += 1
            Loop
            If relatedSeqNumber.Length > 0 Then
                relatedSeqNumber = relatedSeqNumber.Remove(relatedSeqNumber.Length - 1)
            End If


            chkRelatedSeqNo.Items.Clear()

            baseWorkFolder = ds_workspace.Tables(0).Rows(0)("vBaseWorkFolder").ToString
            subDesc = txtSubmissionDesc.Text.Trim
            subVariationMode = ""

            If Not objPublish.saveSubmissionDtl(UserId, subType, workspaceId, applicationNumber, _
                                         projectPublishType, currentSeqNumber, lastPublishedVersion, _
                                         dos, relatedSeqNumber, subMode, isRMSSelected, subVariationMode, _
                                         addTT, lastConfirmedSubmissionPath, baseWorkFolder, subDesc, _
                                         selectedCMS, trackCMS, estr) Then
                Throw New Exception(estr)
            End If

            ObjCommon.ShowAlert("Project Published Successfully", Me.Page)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

    Private Function AssignValuesCA(ByVal ds_project As DataSet) As Boolean
        Dim ds_workspace As New DataSet
        Dim ds_Submission As New DataSet
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim i As Integer = 0
        Dim count As Integer = 0
        Dim element As Integer = 0
        Dim iSeqNo As Integer = 0
        Dim strSeqNo As String = Nothing
        Dim iRelatedCount As Integer = 0

        Dim UserId As Integer
        Dim subType As String
        Dim workspaceId As String
        Dim applicationNumber As String
        Dim projectPublishType As Char
        Dim currentSeqNumber As String
        Dim lastPublishedVersion As String = "-999"
        Dim dos As Date
        Dim relatedSeqNumber As String = Nothing
        Dim subMode As String
        Dim isRMSSelected As Char
        Dim subVariationMode As String
        Dim addTT As Char                   '//N for CP and NP else Y
        Dim lastConfirmedSubmissionPath As String
        Dim baseWorkFolder As String
        Dim subDesc As String
        Dim selectedCMS(-1) As Integer
        Dim trackCMS(-1) As String
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            wstr = "vWorkSpaceId='" + ds_project.Tables(0).Rows(0)("vWorkSpaceId").ToString + "'"
            If Not objHelp.getworkspacemst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_workspace, estr) Then
                Throw New Exception(estr)
            End If


            UserId = Me.Session(S_UserID)
            subType = ddlSubmissionType.SelectedItem.Text
            workspaceId = ds_project.Tables(0).Rows(0)("vWorkSpaceId").ToString
            applicationNumber = txtApplicationNumber.Text.Trim
            projectPublishType = "P"

            '''''' Code for Current Sequence Number ''''''
            If ds_workspace.Tables(0).Rows(0)("vLastPublishedVersion").ToString.Trim = "-999" Then
                currentSeqNumber = "0000"       'Condition
            Else
                iSeqNo = Convert.ToInt32(ds_workspace.Tables(0).Rows(0)("vLastPublishedVersion").ToString.Trim)
                iSeqNo += 1
                strSeqNo = "000" + iSeqNo.ToString
                strSeqNo.Substring(strSeqNo.Length - 4)
                currentSeqNumber = strSeqNo
            End If

            '''''' Code for last Confirmed Submission Path ''''''''
            If currentSeqNumber = "0000" Then
                lastConfirmedSubmissionPath = ""
            Else
                wstr = "WorkspaceId = '" + workspaceId + "' AND Confirm = 'Y' order by submissioninfoCAdtlId desc"
                If Not objPublish.getWorkspaceSubmissionInfoCADtlBySubmissionId(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Submission, estr) Then
                    Throw New Exception(estr)
                End If
                lastConfirmedSubmissionPath = ds_Submission.Tables(0).Rows(0)("SubmissionPath").ToString.Trim
            End If


            lastPublishedVersion = ds_workspace.Tables(0).Rows(0)("vLastPublishedVersion").ToString.Trim    'Condition
            dos = Convert.ToDateTime(Request.Form("ctl00$CPHLAMBDA$txtDos"))
            subMode = ddlTransamissionMedia.SelectedItem.Text

            '''''' Code for Selected Related Sequence Number '''''''Condition
            iRelatedCount = 0
            relatedSeqNumber = ""
            Do While iRelatedCount < chkRelatedSeqNo.Items.Count
                If chkRelatedSeqNo.Items(iRelatedCount).Selected Then
                    relatedSeqNumber += chkRelatedSeqNo.Items(iRelatedCount).Text.ToString.Trim() + ","
                    'relatedSeqNumber = relatedSeqNumber.Remove(relatedSeqNumber.Length - 1)
                End If
                iRelatedCount += 1
            Loop
            If relatedSeqNumber.Length > 0 Then
                relatedSeqNumber = relatedSeqNumber.Remove(relatedSeqNumber.Length - 1)
            End If
            chkRelatedSeqNo.Items.Clear()

            baseWorkFolder = ds_workspace.Tables(0).Rows(0)("vBaseWorkFolder").ToString
            subDesc = txtSubmissionDesc.Text.Trim

            If Not objPublish.saveSubmissionDtl(UserId, subType, workspaceId, applicationNumber, _
                                         projectPublishType, currentSeqNumber, lastPublishedVersion, _
                                         dos, relatedSeqNumber, subMode, isRMSSelected, subVariationMode, _
                                         addTT, lastConfirmedSubmissionPath, baseWorkFolder, subDesc, _
                                         selectedCMS, trackCMS, estr) Then
                Throw New Exception(estr)
            End If

            ObjCommon.ShowAlert("Project Published Successfully", Me.Page)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function AssignValues_Attributes() As Boolean
        Dim ds_ParentNodeAttributes As New DataSet
        Dim dt_DistinctParentNodes As New DataTable
        Dim ds_WsNodeDetail As New DataSet
        Dim dr As DataRow
        Dim RowCnt As Integer = 0
        Dim estr As String = Nothing
        Dim wstr As String = Nothing
        Dim wsId As String = Nothing
        Dim attrNodeDisplayName As String = Nothing
        Dim NodeDispName As String = Nothing

        Try

            ''''''''''''''''''''''SAVING DATA IN WORKSPACENODEATTRDETAIL''''''''''''''''''''''''
            If Not objHelp.GetWorkspacenodeattrdetail("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_ParentNodeAttributes, estr) Then
                Throw New Exception(estr)
            End If

            ds_ParentNodeAttributes.Tables(0).Columns.Add("iNodeIdNew", GetType(Integer))

            Do While RowCnt <= GvAttributes.Rows.Count - 1
                dr = ds_ParentNodeAttributes.Tables(0).NewRow()
                dr("vWorkspaceId") = GvAttributes.Rows(RowCnt).Cells(GVC_vWorkspaceIdAttr).Text
                dr("iNodeId") = GvAttributes.Rows(RowCnt).Cells(GVC_iNodeId).Text
                dr("iAttrId") = GvAttributes.Rows(RowCnt).Cells(GVC_iAttrId).Text
                dr("vAttrName") = GvAttributes.Rows(RowCnt).Cells(GVC_vAttrName).Text
                dr("vAttrValue") = CType(GvAttributes.Rows(RowCnt).FindControl("txtAttrValue"), TextBox).Text.Trim
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "E"
                ds_ParentNodeAttributes.Tables(0).Rows.Add(dr)
                ds_ParentNodeAttributes.Tables(0).AcceptChanges()
                RowCnt = RowCnt + 1
            Loop

            wsId = ds_ParentNodeAttributes.Tables(0).Rows(0)("vWorkspaceId").ToString
            If Not objPublish.Insert_WorkspaceNodeAttrDetail_ForEctd(Publish.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_ParentNodeAttributes, Me.Session(S_UserID), estr) Then
                Throw New Exception(estr)
            End If

            ''''''''''''''''''SAVING DATA IN WORKSPACENODEDEATAIL'''''''''''''''''''
            dt_DistinctParentNodes = ds_ParentNodeAttributes.Tables(0).DefaultView.ToTable(True, "iNodeId")

            For Each dr_distinct As DataRow In dt_DistinctParentNodes.Rows
                wstr = "vworkspaceId='" + wsId + "' AND " + "iNodeId=" + dr_distinct("iNodeId").ToString
                If Not objHelp.getWorkSpaceNodeDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_WsNodeDetail, estr) Then
                    Throw New Exception(estr)
                End If

                attrNodeDisplayName = ""
                For Each dr_attr As DataRow In ds_ParentNodeAttributes.Tables(0).Rows
                    If (dr_distinct("iNodeId").ToString.Trim = dr_attr("iNodeId").ToString.Trim) Then
                        If (attrNodeDisplayName.Trim = "") Then
                            attrNodeDisplayName = dr_attr("vAttrValue").ToString.Trim
                        ElseIf (attrNodeDisplayName.Trim <> "") Then
                            If dr_attr("vAttrValue").ToString.Trim <> "" Then
                                attrNodeDisplayName += ", " + dr_attr("vAttrValue").ToString.Trim
                            End If
                        End If
                    End If
                Next

                If attrNodeDisplayName = "" Then
                    NodeDispName = ds_WsNodeDetail.Tables(0).Rows(0)("vNodeDisplayName").ToString
                    ds_WsNodeDetail.Tables(0).Rows(0)("vNodeDisplayName") = Split(NodeDispName, "(")(0).ToString
                Else
                    NodeDispName = ds_WsNodeDetail.Tables(0).Rows(0)("vNodeDisplayName").ToString
                    NodeDispName = Split(NodeDispName, "(")(0).ToString
                    ds_WsNodeDetail.Tables(0).Rows(0)("vNodeDisplayName") = Split(NodeDispName, "(")(0).ToString + "(" + attrNodeDisplayName + ")"
                End If
                ds_WsNodeDetail.AcceptChanges()

                If Not objLambda.Save_WorkSpaceNodeDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_WsNodeDetail, Me.Session(S_UserID), estr) Then
                    Throw New Exception(estr)
                End If
            Next

            '''''''''''''''''REBUILD THE GRID'''''''''''''''''''
            getFilesForPublish()
            ObjCommon.ShowAlert("Attributes Saved Successfully.", Me.Page)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "Set Controls"

    Private Function SetProjectDetails() As Boolean
        Dim ds_Project As New DataSet
        Dim ds_workspace As New DataSet
        Dim ds_ProjectType As New DataSet
        Dim ds_Client As New DataSet
        Dim wstr As String
        Dim estr As String
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            lblProjectName.Text = ""
            lblProjectType.Text = ""
            lblClientName.Text = ""
            ds_Project = ViewState(VS_Project)

            wstr = "vWorkSpaceId='" + ds_Project.Tables(0).Rows(0)("vWorkSpaceId").ToString + "'"
            If Not objHelp.getworkspacemst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_workspace, estr) Then
                Throw New Exception(estr)
            End If

            wstr = "vProjectTypeCode='" + ds_workspace.Tables(0).Rows(0)("vProjectTypeCode").ToString + "'"
            If Not objHelp.getprojectTypeMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ProjectType, estr) Then
                Throw New Exception(estr)
            End If

            wstr = "vClientCode='" + ds_workspace.Tables(0).Rows(0)("vClientCode").ToString + "'"
            If Not objHelp.getclientmst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Client, estr) Then
                Throw New Exception(estr)
            End If

            HProjectRegion.Value = ds_Project.Tables(0).Rows(0)("CountryRegionName").ToString
            lblProjectName.Text = ds_workspace.Tables(0).Rows(0)("vWorkSpaceDesc").ToString
            lblProjectType.Text = ds_ProjectType.Tables(0).Rows(0)("vProjectTypeName").ToString
            lblClientName.Text = ds_Client.Tables(0).Rows(0)("vClientName").ToString
            lblRMSCountryName.Text = "(" + ds_Project.Tables(0).Rows(0)("vCountryName").ToString + ")"

            txtdos.Text = System.DateTime.Today.ToString("dd-MMM-yyyy")
            txtTrackingNumber.Text = ds_Project.Tables(0).Rows(0)("vTrackingNo").ToString
            txtApplicationNumber.Text = ds_Project.Tables(0).Rows(0)("vApplicationNo").ToString

            ddlSubmissionMode.Items.Clear()
            ddlSubmissionMode.Items.Insert(0, "Select Submission Mode")
            ddlSubmissionMode.Items.Insert(1, "Single")
            ddlSubmissionMode.Items.Insert(2, "Grouping")
            ddlSubmissionMode.Items.Insert(3, "Worksharing")
            If ds_Project.Tables(0).Rows(0)("vHighLvlNo").ToString = "" Then
                ddlSubmissionMode.Items.RemoveAt(3)
                ddlSubmissionMode.Items.RemoveAt(2)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function HideControls() As Boolean
        trProjectDetail.Style.Add("Display", "none")
        trAttribute.Style.Add("Display", "none")
        trSubmissionEntry.Style.Add("Display", "none")
        trButtons.Style.Add("Display", "none")
        trProjectSubmissionDetail.Style.Add("Display", "none")
        trpublishedProject.Style.Add("Display", "none")


        tdsubmissionType_1.Style.Add("Display", "none")
        tdsubmissionType_2.Style.Add("Display", "none")
        tdsubmissionMode_1.Style.Add("Display", "none")
        tdsubmissionMode_2.Style.Add("Display", "none")
        tdApplicationNumber_1.Style.Add("Display", "none")
        tdApplicationNumber_2.Style.Add("Display", "none")
        tdTrackingNumber_1.Style.Add("Display", "none")
        tdTrackingNumber_2.Style.Add("Display", "none")
        tdCurrentSeqNumber_1.Style.Add("Display", "none")
        tdCurrentSeqNumber_2.Style.Add("Display", "none")
        tdRelatedSeqNumber_1.Style.Add("Display", "none")
        tdRelatedSeqNumber_2.Style.Add("Display", "none")
        tdDOS_1.Style.Add("Display", "none")
        tdDOS_2.Style.Add("Display", "none")

        tdTransamissionMedia_1.Style.Add("Display", "none")
        tdTransamissionMedia_2.Style.Add("Display", "none")
        tdSubmissionDesc_1.Style.Add("Display", "none")
        tdSubmissionDesc_2.Style.Add("Display", "none")
        tdIncludeRMS_1.Style.Add("Display", "none")
        tdIncludeRMS_2.Style.Add("Display", "none")
        tdAddTrackingTable_1.Style.Add("Display", "none")
        tdSelectCMS.Style.Add("Display", "none")

    End Function

    Private Function SetVisibleControls() As Boolean
        'Dim ds_ProjectWithRights As New DataSet
        Dim ds_Project As New DataSet
        Dim estr As String
        Dim wstr As String

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            HideControls()
            'ds_ProjectWithRights = ViewState(VS_SubmittedProject)
            If Not ddlProject.SelectedIndex > 0 Then
                Exit Function
            End If

            wstr = "vWorkSpaceId=" + ddlProject.SelectedItem.Value.ToString + " AND iUserCode=" + Me.Session(S_UserID)
            If Not objHelp.view_AllWorkspaceSubmissionInfo(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Project, estr) Then
                Throw New Exception(estr)
            End If

            ViewState(VS_Project) = ds_Project

            If (ds_Project.Tables(0).Rows(0)("CountryRegionName").ToString.ToUpper = "US" Or ds_Project.Tables(0).Rows(0)("CountryRegionName").ToString.ToUpper = "CA") Then
                trProjectDetail.Style.Add("Display", "block")
                trAttribute.Style.Add("Display", "block")
                trSubmissionEntry.Style.Add("Display", "block")
                trButtons.Style.Add("Display", "block")
                'trProjectSubmissionDetail.Style.Add("Display", "block")

                tdsubmissionType_1.Style.Add("Display", "block")
                tdsubmissionType_2.Style.Add("Display", "block")
                tdApplicationNumber_1.Style.Add("Display", "block")
                tdApplicationNumber_2.Style.Add("Display", "block")
                tdCurrentSeqNumber_1.Style.Add("Display", "block")
                tdCurrentSeqNumber_2.Style.Add("Display", "block")
                tdRelatedSeqNumber_1.Style.Add("Display", "block")
                tdRelatedSeqNumber_2.Style.Add("Display", "block")
                tdDOS_1.Style.Add("Display", "block")
                tdDOS_2.Style.Add("Display", "block")
                tdTransamissionMedia_1.Style.Add("Display", "block")
                tdTransamissionMedia_2.Style.Add("Display", "block")

                tdsubmissionType_1.Style.Add("width", "200px")
                tdApplicationNumber_1.Style.Add("width", "200px")
                tdCurrentSeqNumber_1.Style.Add("width", "200px")
                tdRelatedSeqNumber_1.Style.Add("width", "200px")
                tdDOS_1.Style.Add("width", "200px")
                tdTransamissionMedia_1.Style.Add("width", "200px")

                tblSub.Style.Add("Width", "500px")
                tblSub.Style.Add("cellpadding", "5")

                FillSubmissionType(ds_Project.Tables(0).Rows(0)("CountryRegionName").ToString)

            ElseIf (ds_Project.Tables(0).Rows(0)("CountryRegionName").ToString.ToUpper = "EU") Then
                trProjectDetail.Style.Add("Display", "block")
                trAttribute.Style.Add("Display", "block")
                trSubmissionEntry.Style.Add("Display", "block")
                trButtons.Style.Add("Display", "block")
                'trProjectSubmissionDetail.Style.Add("Display", "block")
                trpublishedProject.Style.Add("Display", "block")

                tdsubmissionType_1.Style.Add("Display", "block")
                tdsubmissionType_2.Style.Add("Display", "block")
                tdsubmissionMode_1.Style.Add("Display", "block")
                tdsubmissionMode_2.Style.Add("Display", "block")
                tdTrackingNumber_1.Style.Add("Display", "block")
                tdTrackingNumber_2.Style.Add("Display", "block")
                tdCurrentSeqNumber_1.Style.Add("Display", "block")
                tdCurrentSeqNumber_2.Style.Add("Display", "block")
                tdRelatedSeqNumber_1.Style.Add("Display", "block")
                tdRelatedSeqNumber_2.Style.Add("Display", "block")
                tdDOS_1.Style.Add("Display", "block")
                tdDOS_2.Style.Add("Display", "block")
                tdTransamissionMedia_1.Style.Add("Display", "block")
                tdTransamissionMedia_2.Style.Add("Display", "block")
                tdSubmissionDesc_1.Style.Add("Display", "block")
                tdSubmissionDesc_2.Style.Add("Display", "block")
                tdIncludeRMS_1.Style.Add("Display", "block")
                tdIncludeRMS_2.Style.Add("Display", "block")
                tdAddTrackingTable_1.Style.Add("Display", "block")
                tdSelectCMS.Style.Add("Display", "block")

                tblSub.Style.Add("Width", "850px")
                tblSub.Style.Add("cellpadding", "1")

                FillSubmissionType(ds_Project.Tables(0).Rows(0)("CountryRegionName").ToString)
                FillCMSGrid()
            End If

            ''''' Setting Relared Sequence Number ''''''
            setRelatedSeqNo()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function setRelatedSeqNo() As Boolean
        Dim ds_Project As New DataSet
        Dim lastSeqNo As String = Nothing
        Dim iSeq As Integer = Nothing
        Dim vSeq As String = Nothing
        Dim icurSeq As Integer = Nothing
        Dim vcurSeq As String = Nothing
        Dim i As Integer = 0

        Try

            ds_Project = ViewState(VS_Project)
            lastSeqNo = ds_Project.Tables(0).Rows(0)("vLastPublishedVersion").ToString.Trim()
            chkRelatedSeqNo.Items.Clear()

            '''''''' Setting Current Sequence Number ''''''''''
            If lastSeqNo = "-999" Then
                txtCurrentSeqNumber.Text = "0000"
            Else
                icurSeq = CType(lastSeqNo, Integer)
                icurSeq += 1
                vcurSeq = "000" + icurSeq.ToString
                vcurSeq = vcurSeq.Substring(vcurSeq.Length - 4)
                txtCurrentSeqNumber.Text = vcurSeq
            End If


            '''''''' Setting Related Sequence Number ''''''''''
            'lastSeqNo = "0000"
            If lastSeqNo = "-999" Then
                tdRelatedSeqNo.Style.Add("display", "none")
                tdRelatedSeqNoMsg.Style.Add("display", "block")
            Else
                iSeq = CType(lastSeqNo, Integer)
                Do While i <= iSeq
                    vSeq = "000" + i.ToString
                    vSeq = vSeq.Substring(vSeq.Length - 4)
                    chkRelatedSeqNo.Items.Add(vSeq)
                    'chkRelatedSeqNo.Items.Insert(chkRelatedSeqNo.Items.Count, vSeq)
                    i = i + 1
                Loop

                tdRelatedSeqNo.Style.Add("display", "block")
                tdRelatedSeqNoMsg.Style.Add("display", "none")
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "Prevalidation"

    Private Function getFilesForPublish() As Boolean
        Dim ds_FilesForPublish As New DataSet
        Dim ds_SubInfoDtl As New DataSet
        Dim wstr As String
        Dim estr As String
        Dim wsId As String
        Dim isFileForPublish As Boolean = False

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            '''''''GETTING NODES FROM WORKSPACENODEHISTORY WHICH WILL GOING FOR PUBLISH'''''''

            wsId = ddlProject.SelectedItem.Value
            'wsId = "0000002383"
            wstr = "vWorkSpaceId = '" + wsId + "' AND iStageId = 100 AND iTranNo >" + _
                    "(" + _
                        "SELECT ISNULL(MAX(iTranNo),0) FROM InternalLabelHistory WHERE vWorkspaceId = '" + wsId + "' AND iLabelNo =" + _
                        "(" + _
                            "SELECT iLabelNo FROM InternalLabelMst WHERE vWorkspaceId = '" + wsId + "'  AND vLabelId IN" + _
                            "(" + _
                                "SELECT vLabelId FROM View_SubmissionInfoDtlForAllRegions WHERE vWorkspaceId = '" + wsId + "' AND cConfirm = 'Y' AND vCurrentSeqNumber =" + _
                                "(" + _
                                    "SELECT MAX(vCurrentSeqNumber) FROM View_SubmissionInfoDtlForAllRegions where vWorkspaceId = '" + wsId + "' AND cConfirm = 'Y'" + _
                                ")" + _
                            ")" + _
                        ")" + _
                    ")"


            If Not objHelp.getWorkspaceNodeHistory(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_FilesForPublish, estr) Then
                Throw New Exception(estr)
            End If

            If ds_FilesForPublish.Tables(0).Rows.Count > 0 Then
                isFileForPublish = True
                getAttributeSpecificParentNodes(ds_FilesForPublish)
            Else
                trProjectDetail.Style.Add("Display", "none")
                trAttribute.Style.Add("Display", "none")
                trSubmissionEntry.Style.Add("Display", "none")
                trButtons.Style.Add("Display", "none")
                trProjectSubmissionDetail.Style.Add("Display", "none")
                trpublishedProject.Style.Add("Display", "none")

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "message", "alert('There Are No Files For Publish.');", True)
            End If

            '' Checking that if any Submission is done or not to display
            '' Grid.
            displayProjectSubmissionDtl()

            Return isFileForPublish
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function getAttributeSpecificParentNodes(ByVal ds_FilesForPublish As DataSet) As Boolean
        Dim ds_ParentNodeAttributes As New DataSet
        Dim estr As String
        Dim wstr As String
        Dim nodeids As String
        Dim wsId As String

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            For Each dr As DataRow In ds_FilesForPublish.Tables(0).Rows
                nodeids += dr("inodeId").ToString + ","
            Next
            wsId = ds_FilesForPublish.Tables(0).Rows(0)("vWorkSpaceId").ToString
            nodeids = nodeids.Substring(0, nodeids.Length - 1) 'Removing last comma from string

            If Not objPublish.Proc_GetAttributeSpecificParentNodes(wsId, nodeids, ds_ParentNodeAttributes, estr) Then
                Throw New Exception(estr)
            End If

            'ds_ParentNodeAttributes.Clear()
            Me.ViewState(VS_PublishableStructure) = ds_ParentNodeAttributes

            trAttribute.Style.Add("display", "block")
            If ds_ParentNodeAttributes.Tables(0).Rows.Count > 0 Then
                FillAttrGrid(ds_ParentNodeAttributes)
                trAttrGrid.Style.Add("Display", "block")
                trAttrMsg.Style.Add("Display", "none")
            Else
                trAttribute.Style.Add("display", "none")
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function displayProjectSubmissionDtl() As Boolean
        Dim ds_SubInfoDtl As New DataSet
        Dim estr As String = Nothing
        Dim wstr As String = Nothing
        Try
            If HProjectRegion.Value.ToUpper.Trim = "EU" Then
                wstr = "WorkspaceId='" + ddlProject.SelectedItem.Value + "'"
                If Not objPublish.getWorkspaceSubmissionInfoEU14DtlBySubmissionId(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubInfoDtl, estr) Then
                    Throw New Exception(estr)
                End If
            ElseIf HProjectRegion.Value.ToUpper.Trim = "US" Then
                wstr = "WorkspaceId='" + ddlProject.SelectedItem.Value + "'"
                If Not objPublish.getWorkspaceSubmissionInfoUSDtlBySubmissionId(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubInfoDtl, estr) Then
                    Throw New Exception(estr)
                End If
            ElseIf HProjectRegion.Value.ToUpper.Trim = "CA" Then
                wstr = "WorkspaceId='" + ddlProject.SelectedItem.Value + "'"
                If Not objPublish.getWorkspaceSubmissionInfoCADtlBySubmissionId(wstr, Publish.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubInfoDtl, estr) Then
                    Throw New Exception(estr)
                End If
            End If

            If ds_SubInfoDtl.Tables(0).Rows.Count > 0 Then
                FillGridProjectGrid(ds_SubInfoDtl)
            Else
                trpublishedProject.Style.Add("Display", "none")
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function HideGridControlsAfterConfirm() As Boolean
        Try
            '''' **********************************************************************
            ' This function id called after the gvPublishedProject.databind() function
            ' And at the end of RowCommand function of gvPublishedProject
            '''' **********************************************************************
            For Each gr As GridViewRow In gvPublishedProject.Rows
                If gr.Cells(GVC_ConfirmValue).Text.ToUpper.Trim = "Y" Then
                    For Each subGr As GridViewRow In gvPublishedProject.Rows
                        If subGr.Cells(GVC_CurrentSeqNumber).Text = gr.Cells(GVC_CurrentSeqNumber).Text And subGr.Cells(GVC_ConfirmValue).Text <> "Y" Then
                            If Not subGr.Cells(GVC_Recompile).FindControl("ImgRecompile") Is Nothing Then
                                CType(subGr.Cells(GVC_Recompile).FindControl("ImgRecompile"), ImageButton).Visible = False
                            End If
                            If Not subGr.Cells(GVC_Confirm).FindControl("lnkConfirm") Is Nothing Then
                                CType(subGr.Cells(GVC_Confirm).FindControl("lnkConfirm"), LinkButton).Visible = False
                            End If
                            'CType(subGr.Cells(GVC_PublishPath).FindControl("ImgPublishPath"), ImageButton).Visible = False

                            subGr.Cells(GVC_Recompile).Text = "-"
                            subGr.Cells(GVC_Confirm).Text = "-"
                            'subGr.Cells(GVC_PublishPath).Text = "-"
                        End If
                    Next
                End If
            Next
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnSaveAttributes_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAttributes.Click
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            AssignValues_Attributes()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim ds_project As New DataSet
        Dim estr As String
        Dim wstr As String
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            wstr = "vWorkSpaceId=" + ddlProject.SelectedItem.Value.ToString + " AND iUserCode=" + Me.Session(S_UserID)
            If Not objHelp.view_AllWorkspaceSubmissionInfo(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_project, estr) Then
                Throw New Exception(estr)
            End If

            If (ds_project.Tables(0).Rows(0)("CountryRegionName").ToString.ToUpper = "EU") Then
                AssignValuesEU14(ds_project)
            ElseIf (ds_project.Tables(0).Rows(0)("CountryRegionName").ToString.ToUpper = "US") Then
                AssignValuesUS(ds_project)
            ElseIf (ds_project.Tables(0).Rows(0)("CountryRegionName").ToString.ToUpper = "CA") Then
                AssignValuesCA(ds_project)
            End If

            ''''''''' DISPLAYING GRID AFTER SUBMITE BUTTON '''''''''
            displayProjectSubmissionDtl()

            ''''' Setting Relared Sequence Number ''''''
            setRelatedSeqNo()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub ddlProject_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProject.SelectedIndexChanged
        Dim ds_Project As New DataSet
        Dim ds_SubInfoEU14Dtl As New DataSet
        Dim estr As String = Nothing
        Dim wstr As String = Nothing
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            SetVisibleControls()
            SetProjectDetails()

            getFilesForPublish()

            openPublishPath("sss")
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnRecompile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRecompile.Click
        Try
            recompileSubmission()
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
