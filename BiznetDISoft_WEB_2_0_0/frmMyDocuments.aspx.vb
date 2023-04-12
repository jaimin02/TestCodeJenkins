
Partial Class frmMyDocuments
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION"
    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Private Const VS_Result As String = "VS_Result"
    Private Const VS_gvReleaseDetail As String = "VS_gvReleaseDetail"

    ' gvReleaseDetail
    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_nAutoId As Integer = 1
    Private Const GVC_vWorkspaceId As Integer = 2
    Private Const GVC_iParentNodeId As Integer = 3
    Private Const GVC_iNodeId As Integer = 4
    Private Const GVC_iDocLink As Integer = 5
    Private Const GVC_dModifyOn As Integer = 6
    Private Const GVC_iReleasedBy As Integer = 7
    Private Const GVC_vUserName As Integer = 8
    Private Const GVC_DocAction As Integer = 9
    Private Const GVC_vComments As Integer = 10
    Private Const GVC_iStageId As Integer = 11
    Private Const GVC_FilePath As Integer = 12

    ' gvAuditTrail
    Private Const GVC_A_SrNo As Integer = 0
    Private Const GVC_A_NodeDisplayName As Integer = 1
    Private Const GVC_A_ModifyBy As Integer = 2
    Private Const GVC_A_ModifyOn As Integer = 3
    Private Const GVC_A_StatusIndi As Integer = 4
    Private Const GVC_A_Comments As Integer = 5

    ' gvParentAuditTrail
    Private Const GVC_PA_SrNo As Integer = 0
    Private Const GVC_PA_NodeDisplayName As Integer = 1
    Private Const GVC_PA_ModifyBy As Integer = 2
    Private Const GVC_PA_ModifyOn As Integer = 3
    Private Const GVC_PA_StatusIndi As Integer = 4
    Private Const GVC_PA_Comments As Integer = 5

#End Region

#Region "Load Event"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim estr As String = Nothing
        Dim ds As New DataSet
        Try

            If Not IsPostBack Then
                CType(Me.Master.FindControl("lblHeading"), Label).Text = "Department Publish & Submission"

                If Not GenCall_ShowUI() Then
                    Exit Sub
                End If

                Choice = Me.Request.QueryString("Mode")

                If Not Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                    GenCall()
                Else
                    FillddlProject()
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
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

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
            Page.Title = ":: My Document :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblMandatory"), Label).Visible = False
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "My Documents"

            FillddlProject()

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "Grid Events"

    Protected Sub gvReleaseDetail_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvReleaseDetail.PageIndexChanging
        Try
            gvReleaseDetail.PageIndex = e.NewPageIndex
            FillgvReleaseDetail()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub gvReleaseDetail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvReleaseDetail.RowCommand
        Dim index As Integer = e.CommandArgument
        Try
            If e.CommandName.ToUpper = "DOCACTION" Then
                hdnNodeId.Value = Me.gvReleaseDetail.Rows(index).Cells(GVC_iNodeId).Text
                hdnFilePath.Value = Me.gvReleaseDetail.Rows(index).Cells(GVC_FilePath).Text
                txtDocComments.Text = ""
                txtDocComments.Focus()
                FillgvAuditTrail()
                btnShow_Click(sender, e)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub gvReleaseDetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvReleaseDetail.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Or e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(GVC_nAutoId).Visible = False
                e.Row.Cells(GVC_vWorkspaceId).Visible = False
                e.Row.Cells(GVC_iParentNodeId).Visible = False
                e.Row.Cells(GVC_iReleasedBy).Visible = False
                e.Row.Cells(GVC_iStageId).Visible = False
                e.Row.Cells(GVC_iNodeId).Visible = False
                e.Row.Cells(GVC_FilePath).Visible = False
            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("ImgDocAction"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgDocAction"), ImageButton).CommandName = "DOCACTION"

                e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (gvReleaseDetail.PageSize * gvReleaseDetail.PageIndex) + 1
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub gvAuditTrail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAuditTrail.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                If e.Row.Cells(GVC_A_StatusIndi).Text = "R" Then
                    e.Row.Cells(GVC_A_StatusIndi).Text = "Returned"
                ElseIf e.Row.Cells(GVC_A_StatusIndi).Text = "P" Then
                    e.Row.Cells(GVC_A_StatusIndi).Text = "Re-Print"
                End If

                e.Row.Cells(GVC_A_SrNo).Text = e.Row.RowIndex + (gvAuditTrail.PageSize * gvAuditTrail.PageIndex) + 1
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub gvParentAuditTrail_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvParentAuditTrail.PageIndexChanging
        Try
            gvParentAuditTrail.PageIndex = e.NewPageIndex
            FillgvParentAuditTrail()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub gvParentAuditTrail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvParentAuditTrail.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                If e.Row.Cells(GVC_PA_StatusIndi).Text = "R" Then
                    e.Row.Cells(GVC_PA_StatusIndi).Text = "Returned"
                ElseIf e.Row.Cells(GVC_PA_StatusIndi).Text = "P" Then
                    e.Row.Cells(GVC_PA_StatusIndi).Text = "Re-Print"
                End If

                e.Row.Cells(GVC_PA_SrNo).Text = e.Row.RowIndex + (gvParentAuditTrail.PageSize * gvParentAuditTrail.PageIndex) + 1
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "Fill DropDowns"

    Public Function FillddlProject() As Boolean
        Dim ds_Project As DataSet = Nothing
        Dim dv_Project As DataView = Nothing
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Try
            'wstr = "iUserId = " + Me.Session(S_UserID) + " AND vProjectTypeCode='0010'"
            wstr = "iUserId = " + Me.Session(S_UserID) + " AND vProjectTypeCode='0010' AND vLocationCode ='" + Me.Session(S_LocationCode).ToString + "'"

            If Not objHelp.View_MyProjects(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Project, estr) Then
                Throw New Exception(estr)
            End If

            If ds_Project.Tables(0).Rows.Count > 0 Then
                ds_Project.Tables(0).Columns.Add("vProjectName", GetType(String))
                ds_Project.AcceptChanges()

                For Each dr As DataRow In ds_Project.Tables(0).Rows
                    dr("vProjectName") = dr("vWorkSpaceDesc") '+ " - " + "(" + dr("vProjectNo") + ")"
                Next
                ds_Project.AcceptChanges()

                dv_Project = ds_Project.Tables(0).DefaultView
                dv_Project.Sort = "vProjectName"

                Me.ddlProject.DataSource = dv_Project
                Me.ddlProject.DataValueField = "vWorkspaceId"
                Me.ddlProject.DataTextField = "vProjectName"
                Me.ddlProject.DataBind()
                Me.ddlProject.Items.Insert(0, New ListItem("Select Department", ""))

                'Added Tooltip
                For iddlProjectName As Integer = 0 To ddlProject.Items.Count - 1
                    ddlProject.Items(iddlProjectName).Attributes.Add("title", ddlProject.Items(iddlProjectName).Text)
                Next
            Else
                Me.ddlProject.DataSource = Nothing
                Me.ddlProject.DataBind()
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    ''' =======================================================================================
    ''' ADDED BY :: JUGAL KUNDAL
    ''' ADDED ON :: 16-MAY-2012
    ''' REASON   :: FOR FILLING SOP NUMBER DROPDOWN.
    ''' =======================================================================================
    Public Function FillddlSopNo() As Boolean
        Dim dr() As DataRow = Nothing
        Dim ds_Result As DataSet = Nothing
        Dim count As Integer = 0
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim wsId As String = String.Empty
        Try

            wsId = ddlProject.SelectedValue.Trim

            wstr = "vWorkspaceId = '" + wsId + "' AND vMedExCode = '00085' and iParentNodeId = 1"
            If Not objHelp.View_CRFSubDtlForCategory(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Result, estr) Then
                Throw New Exception(estr)
            End If

            ',A-201,A-202,A-203,A-204,A-205
            If Not ds_Result Is Nothing And ds_Result.Tables(0).Rows.Count > 0 Then
                ddlSOPNo.DataSource = ds_Result.Tables(0).DefaultView.ToTable(True, "vmedexResult")
                ddlSOPNo.DataTextField = "vmedexResult"
                ddlSOPNo.DataValueField = "vmedexResult"
                ddlSOPNo.DataBind()
                ddlSOPNo.Items.Insert(0, New ListItem("Select SOP No.", "0"))
                ddlSOPNo.Items.Insert(1, New ListItem("Forms with no SOP No attached", ""))
                'ddl.Items.RemoveAt(2)

                For Each li As ListItem In ddlSOPNo.Items
                    If li.Text = "" Then
                        ddlSOPNo.Items.Remove(li)
                        Exit For
                    End If
                Next

                ''''' Filling Category Dropdown '''''
                dr = ds_Result.Tables(0).Select("vMedExResult = ''")
                If dr.Length > 0 Then
                    For Each dr1 As DataRow In dr
                        ddlCategory.Items.Add(New ListItem(dr1("vNodeDisplayName").ToString, dr1("iNodeId").ToString))
                    Next
                    ddlCategory.Items.Insert(0, New ListItem("Select Form", ""))
                    ddlSOPNo.SelectedIndex = 1
                End If
            Else
                ObjCommon.ShowAlert("No SOP found for this Department.", Me.Page)
            End If
        Catch ex As Exception
            Return ex.Message.ToString
        End Try
    End Function

    ''' =======================================================================================
    ''' ADDED BY :: JUGAL KUNDAL
    ''' ADDED ON :: 16-MAY-2012
    ''' REASON   :: FILLING CATEGORY BASED ON THE SOP SELECTED FROM THE DROPDOWN.
    ''' =======================================================================================
    Public Function FillCategory() As Boolean
        'Dim ddl As New DropDownList
        Dim ds_Result As DataSet = Nothing
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim wsId As String = String.Empty
        Dim SopNo As String = String.Empty
        Try
            wsId = ddlProject.SelectedValue.Trim
            SopNo = ddlSOPNo.SelectedValue.Trim

            wstr = "vWorkspaceId = '" + wsId + "' AND vMedExResult = '" + SopNo + "' AND vMedExCode = '00085' and iParentNodeId = 1"
            If Not objHelp.View_CRFSubDtlForCategory(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Result, estr) Then
                Throw New Exception(estr)
            End If

            If Not ds_Result Is Nothing And ds_Result.Tables(0).Rows.Count > 0 Then
                ddlCategory.DataSource = ds_Result.Tables(0).DefaultView
                ddlCategory.DataValueField = "iNodeId"
                ddlCategory.DataTextField = "vNodeDisplayName"
                ddlCategory.DataBind()
                ddlCategory.Items.Insert(0, New ListItem("Select Form", ""))
            Else
                ObjCommon.ShowAlert("No Form found for this SOP.", Me.Page)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Function

    'Public Function FillddlCategory() As Boolean
    '    Dim ds_Category As DataSet = Nothing
    '    Dim dv_Category As DataView = Nothing
    '    Dim estr As String = String.Empty
    '    Dim wstr As String = String.Empty
    '    Try
    '        wstr = "vWorkspaceId = '" + ddlProject.SelectedItem.Value.Trim + "' AND cStatusIndi <> 'D' AND iParentNodeId = 1"
    '        wstr += " AND iUserId = " + Me.Session(S_UserID) + " AND iStageId = '20'"

    '        'If Not objHelp.getWorkSpaceNodeDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Category, estr) Then
    '        '    Throw New Exception(estr)
    '        'End If

    '        If Not objHelp.GetViewWorkspaceWorkflowUserDtl(wstr, ds_Category, estr) Then
    '            Throw New Exception(estr)
    '        End If

    '        If ds_Category.Tables(0).Rows.Count > 0 Then
    '            dv_Category = ds_Category.Tables(0).DefaultView

    '            Me.ddlCategory.DataSource = dv_Category
    '            Me.ddlCategory.DataValueField = "iNodeId"
    '            Me.ddlCategory.DataTextField = "vNodeDisplayName"
    '            Me.ddlCategory.DataBind()
    '            Me.ddlCategory.Items.Insert(0, New ListItem("Select Category", ""))

    '            'Added Tooltip
    '            For iddlCategory As Integer = 0 To ddlCategory.Items.Count - 1
    '                ddlCategory.Items(iddlCategory).Attributes.Add("title", ddlCategory.Items(iddlCategory).Text)
    '            Next
    '        Else
    '            Me.ddlCategory.Items.Clear()
    '            Me.ddlCategory.DataSource = Nothing
    '            Me.ddlCategory.DataBind()
    '            trbtnSearch.Style.Add("display", "none")
    '            ddlProjectPrefix.Items.Clear()
    '            ddlCategoryPrefix.Items.Clear()
    '            'ddlYear.Items.Clear()
    '            txtStartId.Text = ""
    '            txtEndId.Text = ""

    '            ObjCommon.ShowAlert("You do not have rights on Categories for this Project.", Me.Page)
    '        End If


    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.Message, "")
    '        Return False
    '    End Try
    'End Function

    Public Function FillddlReleasedBy() As Boolean
        Dim ds_Result As DataSet = Nothing
        Dim dv_Result As DataView = Nothing
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim param(1) As String
        Try
            'wstr = "vWorkspaceId = '" + ddlProject.SelectedItem.Value.Trim + "'" + _
            '         " AND iNodeId = " + ddlCategory.SelectedItem.Value.Trim + _
            '         " AND cStatusIndi <> 'D' AND iStageId = '20'"

            'If Not objHelp.GetViewWorkspaceWorkflowUserDtl(wstr, ds_Result, estr) Then
            '    Throw New Exception(estr)
            'End If

            'wstr = "vWorkspaceId = '" + ddlProject.SelectedItem.Value.Trim + "'" + " AND iParentNodeId = " + ddlCategory.SelectedItem.Value.Trim
            wstr = "vWorkspaceId = '" + ddlProject.SelectedItem.Value.Trim + "'"
            If Not objHelp.View_DocReleaseTrack(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Result, estr) Then
                Throw New Exception(estr)
            End If

            If ds_Result.Tables(0).Rows.Count > 0 Then
                param(0) = "iReleasedBy"
                param(1) = "vUserName"
                dv_Result = ds_Result.Tables(0).DefaultView

                Me.ddlReleasedBy.DataSource = dv_Result.ToTable(True, param).DefaultView
                Me.ddlReleasedBy.DataValueField = "iReleasedBy"
                Me.ddlReleasedBy.DataTextField = "vUserName"
                Me.ddlReleasedBy.DataBind()
                Me.ddlReleasedBy.Items.Insert(0, New ListItem("All Users", "0"))

                'Added Tooltip
                For iddlReleasedBy As Integer = 0 To ddlReleasedBy.Items.Count - 1
                    ddlReleasedBy.Items(iddlReleasedBy).Attributes.Add("title", ddlReleasedBy.Items(iddlReleasedBy).Text)
                Next
            Else
                trbtnSearch.Style.Add("display", "none")
                ObjCommon.ShowAlert("Document not available", Me.Page)
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "Helper Functions"

    Public Function SearchDocuments() As Boolean
        Dim ds_Result As DataSet = Nothing
        Dim dr As DataRow
        Dim iStartId As Integer = 0
        Dim iEndId As Integer = 0
        Dim vStartId As String = String.Empty
        Dim vEndId As String = String.Empty
        Try
            ds_Result = Me.ViewState(VS_Result)

            dr = ds_Result.Tables(0).Rows.Find("DMS-CPMA1-2011-0001")

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Public Function AssignValuesDocAction() As Boolean
        Dim ds_DocMgmt As DataSet = Nothing
        Dim dr As DataRow = Nothing
        Dim estr As String = String.Empty
        Dim Action As String = String.Empty
        Try
            If Not objHelp.ReleaseDocMgmt("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_DocMgmt, estr) Then
                Throw New Exception(estr)
            End If

            If Not ds_DocMgmt Is Nothing Then
                dr = ds_DocMgmt.Tables(0).NewRow
                dr("nAutoID") = "0"
                dr("vWorkspaceId") = ddlProject.SelectedItem.Value.Trim
                dr("iNodeId") = hdnNodeId.Value.Trim
                dr("iParentNodeId") = ddlCategory.SelectedItem.Value.Trim
                dr("vComments") = txtDocComments.Text.Trim
                dr("dModifyOn") = System.DateTime.Now
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = rdoAction.SelectedItem.Value.Trim
                dr("vStage") = "APPROVED"
                dr("vDocVersion") = "1.0"
                ds_DocMgmt.Tables(0).Rows.Add(dr)
                ds_DocMgmt.AcceptChanges()

                If Not objLambda.Insert_ReleaseDocMgmt(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_DocMgmt, Me.Session(S_UserID), estr) Then
                    Throw New Exception(estr)
                End If

                If rdoAction.SelectedItem.Value.Trim = "P" Then
                    Action = "Printed"
                Else
                    Action = "Returned"
                End If
                'ObjCommon.ShowAlert("Document " + Action + " Successfully", Me.Page)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "Fill Grid"

    Public Function FillPrefix() As Boolean
        Dim ds_Result As DataSet = Nothing
        Dim dt_ProjectPrefix As DataTable = Nothing
        Dim dt_CategoryPrefix As DataTable = Nothing
        Dim dt_Year As DataTable = Nothing
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Try
            wstr = "vWorkspaceId = '" + ddlProject.SelectedItem.Value.Trim + "' AND iParentNodeId = " + ddlCategory.SelectedItem.Value.Trim

            If Not objHelp.View_MyReleasedDocuments(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Result, estr) Then
                Throw New Exception(estr)
            End If

            If ds_Result.Tables(0).Rows.Count > 0 Then
                ''''' ProjectPrefix Dropdown '''''
                dt_ProjectPrefix = ds_Result.Tables(0).DefaultView.ToTable(True, "vProjectPrefix")
                Me.ddlProjectPrefix.DataSource = dt_ProjectPrefix
                Me.ddlProjectPrefix.DataTextField = "vProjectPrefix"
                Me.ddlProjectPrefix.DataValueField = "vProjectPrefix"
                Me.ddlProjectPrefix.DataBind()

                ''''' CategoryPrefix Dropdown '''''
                dt_CategoryPrefix = ds_Result.Tables(0).DefaultView.ToTable(True, "vCategoryPrefix")
                Me.ddlCategoryPrefix.DataSource = dt_CategoryPrefix
                Me.ddlCategoryPrefix.DataTextField = "vCategoryPrefix"
                Me.ddlCategoryPrefix.DataValueField = "vCategoryPrefix"
                Me.ddlCategoryPrefix.DataBind()

                ''''' Year Dropdown '''''
                'dt_Year = ds_Result.Tables(0).DefaultView.ToTable(True, "vYear")
                'Me.ddlYear.DataSource = dt_Year
                'Me.ddlYear.DataTextField = "vYear"
                'Me.ddlYear.DataValueField = "vYear"
                'Me.ddlYear.DataBind()

                setLastDocId()
                trbtnSearch.Style.Add("Display", "")
                Me.ViewState(VS_Result) = ds_Result
            Else
                ObjCommon.ShowAlert("No Document Found.", Me.Page)
                ddlProjectPrefix.Items.Clear()
                ddlCategoryPrefix.Items.Clear()
                'ddlYear.Items.Clear()
                txtStartId.Text = ""
                txtEndId.Text = ""
                trbtnSearch.Style.Add("display", "none")
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Public Function FillgvReleaseDetail() As Boolean
        Dim ds_Result As DataSet = Nothing
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim Prefix As String = String.Empty
        Dim dc_dModifyOn_IST As DataColumn
        Try
            Prefix = ddlProjectPrefix.SelectedItem.Value.Trim + "-" + ddlCategoryPrefix.SelectedItem.Value.Trim  '+"-" + ddlYear.SelectedItem.Value.Trim

            'wstr = "Select * from View_MyReleasedDocuments where vWorkspaceId = '" + ddlProject.SelectedItem.Value.Trim + _
            '        "' AND iParentNodeId = " + ddlCategory.SelectedItem.Value.Trim + " AND iDocNoId between " + _
            '        txtStartId.Text.Trim + " AND " + txtEndId.Text.Trim
            'wstr += " AND vNodeDisplayName like '%" + Prefix + "%'"
            'wstr += " Order By iDocNoId"

            'ds_Result = objHelp.GetResultSet(wstr, "View_MyReleasedDocuments")

            If ddlReleasedBy.SelectedIndex <> 0 Then
                wstr = "vWorkspaceId = '" + ddlProject.SelectedItem.Value.Trim + "' AND iParentNodeId = " + ddlCategory.SelectedItem.Value.Trim + _
                        " AND iDocNoId between " + txtStartId.Text.Trim + " AND " + txtEndId.Text.Trim + _
                        " AND iReleasedBy = " + ddlReleasedBy.SelectedItem.Value.Trim + _
                        " AND vNodeDisplayName like '%" + Prefix + "%'"
            Else
                wstr = "vWorkspaceId = '" + ddlProject.SelectedItem.Value.Trim + "' AND iParentNodeId = " + ddlCategory.SelectedItem.Value.Trim + _
                        " AND iDocNoId between " + txtStartId.Text.Trim + " AND " + txtEndId.Text.Trim + _
                        " AND vNodeDisplayName like '%" + Prefix + "%'"
            End If

            If ddlReleasedBy.Items.Count > 0 AndAlso ddlReleasedBy.SelectedIndex > 0 Then
                wstr += " AND iReleasedBy = " + ddlReleasedBy.SelectedValue.Trim
            End If

            If rdoDate.SelectedIndex = "1" Then
                wstr += " AND cast(convert(varchar(11),dModifyOn,113) as smalldatetime) >= cast(convert(varchar(11),'" + txtStartDate.Text.Trim + "',113) as smalldatetime)" + _
                        " AND cast(convert(varchar(11),dModifyOn,113) as smalldatetime) <= cast(convert(varchar(11),'" + txtEndDate.Text.Trim + "',113) as smalldatetime)"
            End If
            wstr += " Order By iDocNoId"

            If Not objHelp.View_MyReleasedDocuments(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Result, estr) Then
                Throw New Exception(estr)
            End If

            If ds_Result.Tables(0).Rows.Count > 0 Then
                dc_dModifyOn_IST = New DataColumn("dModifyOn_IST", System.Type.GetType("System.String"))
                ds_Result.Tables(0).Columns.Add("dModifyOn_IST")
                ds_Result.AcceptChanges()
                For Each dr_dModifyOn In ds_Result.Tables(0).Rows
                    'dr_dModifyOn("dModifyOn_IST") = Convert.ToString(dr_dModifyOn("dModifyOn") + strServerOffset)
                    dr_dModifyOn("dModifyOn_IST") = Convert.ToString(CDate(dr_dModifyOn("dModifyOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                Next
                ds_Result.AcceptChanges()
                Me.ViewState(VS_gvReleaseDetail) = ds_Result.Tables(0)
                Me.gvReleaseDetail.DataSource = ds_Result
                Me.gvReleaseDetail.DataBind()
                fldReleaseDetail.Style.Add("Display", "")
                fldReleaseMsg.Style.Add("Display", "none")
                fldParentAuditTrail.Style.Add("Display", "none")
            Else
                Me.ViewState(VS_gvReleaseDetail) = Nothing
                fldReleaseMsg.Style.Add("Display", "")
                fldReleaseDetail.Style.Add("Display", "none")
                fldParentAuditTrail.Style.Add("Display", "none")
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Public Function setLastDocId() As Boolean
        Dim ds_Result As DataSet = Nothing
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim Prefix As String = String.Empty

        Try
            Prefix = ddlProjectPrefix.SelectedItem.Value.Trim + "-" + ddlCategoryPrefix.SelectedItem.Value.Trim + "-" '+ ddlYear.SelectedItem.Value.Trim

            wstr = "vWorkspaceId = '" + ddlProject.SelectedItem.Value.Trim + "' AND iParentNodeId = " + ddlCategory.SelectedItem.Value.Trim + _
                    " Order By iNodeId Desc"

            If Not objHelp.View_MyReleasedDocuments(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Result, estr) Then
                Throw New Exception(estr)
            End If

            If ds_Result.Tables(0).Rows.Count > 0 Then
                lblDocLastId.Text = ds_Result.Tables(0).Rows(0)("vNodeDisplayName").ToString
            Else
                lblDocLastId.Text = "--"
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Public Function FillgvAuditTrail() As Boolean
        Dim ds_Result As DataSet = Nothing
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim dc_dModifyOn_IST As DataColumn
        Try
            wstr = "vWorkspaceId = '" + ddlProject.SelectedItem.Value.Trim + "'"
            wstr += " AND iNodeId = " + hdnNodeId.Value + " Order By dModifyOn Desc"

            If Not objHelp.View_ReleaseDocMgmt(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Result, estr) Then
                Throw New Exception(estr)
            End If

            If ds_Result.Tables(0).Rows.Count > 0 Then
                '**************************************************************
                dc_dModifyOn_IST = New DataColumn("dModifyOn_IST", System.Type.GetType("System.String"))
                ds_Result.Tables(0).Columns.Add("dModifyOn_IST")
                ds_Result.AcceptChanges()
                For Each dr_dModifyOn In ds_Result.Tables(0).Rows
                    'dr_dModifyOn("dModifyOn_IST") = Convert.ToString(dr_dModifyOn("dModifyOn") + strServerOffset)
                    dr_dModifyOn("dModifyOn_IST") = Convert.ToString(CDate(dr_dModifyOn("dModifyOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                Next
                ds_Result.AcceptChanges()
                '************************************************************
                Me.gvAuditTrail.DataSource = ds_Result
                Me.gvAuditTrail.DataBind()
            Else
                Me.gvAuditTrail.DataSource = Nothing
                Me.gvAuditTrail.DataBind()
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Public Function FillgvParentAuditTrail() As Boolean
        Dim ds_Result As DataSet = Nothing
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim dc_dModifyOn_IST As DataColumn
        Try
            wstr = "vWorkspaceId = '" + ddlProject.SelectedItem.Value.Trim + "'"
            wstr += " AND iParentNodeId = " + ddlCategory.SelectedItem.Value.Trim + " Order By dModifyOn Desc"

            If Not objHelp.View_ReleaseDocMgmt(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Result, estr) Then
                Throw New Exception(estr)
            End If

            If ds_Result.Tables(0).Rows.Count > 0 Then
                dc_dModifyOn_IST = New DataColumn("dModifyOn_IST", System.Type.GetType("System.String"))
                ds_Result.Tables(0).Columns.Add("dModifyOn_IST")
                ds_Result.AcceptChanges()
                For Each dr_dModifyOn In ds_Result.Tables(0).Rows
                    'dr_dModifyOn("dModifyOn_IST") = Convert.ToString(dr_dModifyOn("dModifyOn") + strServerOffset)
                    dr_dModifyOn("dModifyOn_IST") = Convert.ToString(CDate(dr_dModifyOn("dModifyOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                Next
                ds_Result.AcceptChanges()
                Me.gvParentAuditTrail.DataSource = ds_Result
                Me.gvParentAuditTrail.DataBind()
                fldReleaseDetail.Style.Add("Display", "none")
                fldReleaseMsg.Style.Add("Display", "none")
                fldParentAuditTrail.Style.Add("Display", "")
            Else
                fldReleaseMsg.Style.Add("Display", "")
                fldReleaseDetail.Style.Add("Display", "none")
                fldParentAuditTrail.Style.Add("Display", "none")
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "Button Events"

    Protected Sub ddlProject_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProject.SelectedIndexChanged
        Try
            fldReleaseDetail.Style.Add("Display", "none")
            fldReleaseMsg.Style.Add("Display", "none")
            fldParentAuditTrail.Style.Add("Display", "none")
            ddlProjectPrefix.Items.Clear()
            ddlCategoryPrefix.Items.Clear()
            ddlSOPNo.Items.Clear()
            ddlCategory.Items.Clear()
            'ddlYear.Items.Clear()
            ddlReleasedBy.Items.Clear()
            trbtnSearch.Style.Add("Display", "none")
            rdoDate.SelectedIndex = 0
            txtStartDate.Text = ""
            txtEndDate.Text = ""
            txtStartId.Text = ""
            txtEndId.Text = ""

            If ddlProject.SelectedIndex <> 0 Then
                'FillddlCategory()
                FillddlSopNo()
                FillddlReleasedBy()
            Else
                trbtnSearch.Style.Add("display", "none")
                ddlCategory.Items.Clear()
                ddlProjectPrefix.Items.Clear()
                ddlCategoryPrefix.Items.Clear()
                'ddlYear.Items.Clear()
                ddlProjectPrefix.Items.Clear()
                ddlCategoryPrefix.Items.Clear()
                'ddlYear.Items.Clear()
                txtStartId.Text = ""
                txtEndId.Text = ""
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub ddlSOPNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSOPNo.SelectedIndexChanged
        Try
            fldReleaseDetail.Style.Add("Display", "none")
            fldReleaseMsg.Style.Add("Display", "none")
            fldParentAuditTrail.Style.Add("Display", "none")
            ddlProjectPrefix.Items.Clear()
            ddlCategoryPrefix.Items.Clear()
            ddlCategory.Items.Clear()
            'ddlYear.Items.Clear()
            'ddlReleasedBy.Items.Clear()
            trbtnSearch.Style.Add("Display", "none")
            rdoDate.SelectedIndex = 0
            txtStartDate.Text = ""
            txtEndDate.Text = ""
            txtStartId.Text = ""
            txtEndId.Text = ""

            If ddlSOPNo.SelectedIndex > 0 Then
                'FillddlCategory()
                FillCategory()
            Else
                trbtnSearch.Style.Add("display", "none")
                ddlCategory.Items.Clear()
                ddlProjectPrefix.Items.Clear()
                ddlCategoryPrefix.Items.Clear()
                'ddlYear.Items.Clear()
                ddlProjectPrefix.Items.Clear()
                ddlCategoryPrefix.Items.Clear()
                'ddlYear.Items.Clear()
                txtStartId.Text = ""
                txtEndId.Text = ""
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub ddlCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCategory.SelectedIndexChanged
        Try
            'ddlReleasedBy.Items.Clear()
            rdoDate.SelectedIndex = 0
            txtStartDate.Text = ""
            txtEndDate.Text = ""
            txtStartId.Text = ""
            txtEndId.Text = ""
            If ddlCategory.SelectedIndex <> 0 Then
                FillPrefix()
                'FillddlReleasedBy()
                fldReleaseDetail.Style.Add("Display", "none")
                fldReleaseMsg.Style.Add("Display", "none")
                fldParentAuditTrail.Style.Add("Display", "none")
            Else
                trbtnSearch.Style.Add("display", "none")
                ddlProjectPrefix.Items.Clear()
                ddlCategoryPrefix.Items.Clear()
                'ddlYear.Items.Clear()
                txtStartId.Text = ""
                txtEndId.Text = ""
                fldReleaseDetail.Style.Add("Display", "none")
                fldReleaseMsg.Style.Add("Display", "none")
                fldParentAuditTrail.Style.Add("Display", "none")
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            trDateText.Style.Add("Display", "none")
            If rdoDate.SelectedIndex = "1" Then
                trDateText.Style.Add("Display", "")
                txtStartDate.Text = hdnStartDate.Value
                txtEndDate.Text = hdnEndDate.Value
            End If

            FillgvReleaseDetail()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnShow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShow.Click
        Try
            MPEAction.Show()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnSaveAction_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAction.Click
        Try
            AssignValuesDocAction()

            If rdoAction.SelectedItem.Value.ToUpper.Trim = "P" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "message", "printDocument('" & hdnFilePath.Value.Trim & "');", True)
            Else
                FillgvAuditTrail()
                MPEAction.Show()
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnParentAuditTrail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnParentAuditTrail.Click
        Try
            FillgvParentAuditTrail()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnExportgvReleaseDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportgvReleaseDetail.Click
        Try
            If Me.gvReleaseDetail.Rows.Count < 1 Then
                Me.ObjCommon.ShowAlert("No Data To Export", Me.Page)
                Exit Sub
            End If
            If Not ExportToExcel("Released Documents Details", CType(Me.ViewState(VS_gvReleaseDetail), DataTable)) Then
                Me.ObjCommon.ShowAlert("Error Occured While Exporting To Excel!", Me.Page)
                Exit Sub
            End If
        Catch ex As Exception
            Me.ObjCommon.ShowAlert(ex.ToString, Me.Page)
            Exit Sub
        End Try
    End Sub

#End Region

#Region "Export To Excel"

    Private Function ExportToExcel(ByVal fileName As String, ByVal dt As DataTable) As Boolean

        Dim ds As New DataSet
        Try

            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            fileName = fileName & ".xls"
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            ds.Tables.Add(dt.Copy()) 'is datatable neccessary
            ds.AcceptChanges()

            Context.Response.Write(ConvertDsuserTO(ds, fileName))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)
            Return True
        Catch ex As Exception
            'Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Function

    Private Function ConvertDsuserTO(ByVal ds As DataSet, ByVal fileName As String) As String
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer
        Dim dsConvert As New DataSet
        Dim Status As String = String.Empty
        'Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_child As New DataSet
        Dim columnList As String = String.Empty
        Dim totalColumns As Integer
        Try
            'Status = "Subjects in Project: " + Me.txtsearch.Text.ToString().Trim()

            Status = "Released Documents Details"
            columnList = "vFilePath,dModifyOn_IST,vUserName,vComments"


            totalColumns = columnList.Split(",").Length

            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=" + totalColumns.ToString + "><center><strong><font color=""#0a22de"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=" + totalColumns.ToString + "><center><strong><font color=""#0a22de"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append(Status)
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")

            strMessage.Append("<tr><td align=""center"" colspan=" + totalColumns.ToString + ">")
            strMessage.Append("</td></tr>")

            strMessage.Append("<tr >")

            dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, columnList.Split(",")).DefaultView.ToTable())
            dsConvert.AcceptChanges()

            dsConvert.Tables(0).Columns(0).ColumnName = "Document Link"
            dsConvert.Tables(0).Columns(1).ColumnName = "Released On"
            dsConvert.Tables(0).Columns(2).ColumnName = "Released By"
            dsConvert.Tables(0).Columns(3).ColumnName = "Comments"

            dsConvert.AcceptChanges()

            For iCol = 0 To dsConvert.Tables(0).Columns.Count - 1

                strMessage.Append("<td align=""center"" ><strong><font color=""#0a22de"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).ToString().Trim())
                strMessage.Append("</font></strong></td>")

            Next
            strMessage.Append("</tr>")

            For j = 0 To dsConvert.Tables(0).Rows.Count - 1

                strMessage.Append("<tr>")

                For i = 0 To dsConvert.Tables(0).Columns.Count - 1
                    strMessage.Append("<td align=""Left"" ><strong><font color=""#000099"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).ToString().Trim())
                    strMessage.Append("</font></strong></td>")
                Next

                'cStatus = dsConvert.Tables(0).Rows(j).Item(0).ToString()

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
