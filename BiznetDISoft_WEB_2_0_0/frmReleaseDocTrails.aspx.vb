
Partial Class frmReleaseDocTrails
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION"
    Private Shared ObjCommon As New clsCommon
    Private Shared objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private Shared objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_gvReleaseDetail As String = "VS_gvReleaseDetail"

    ' gvReleaseDetail
    Private Const GVC_vWorkspaceId As Integer = 0
    Private Const GVC_iParentNodeId As Integer = 1
    Private Const GVC_iReleasedBy As Integer = 2
    Private Const GVC_iQty As Integer = 3
    Private Const GVC_vFileName As Integer = 4
    Private Const GVC_vNodeDisplayName As Integer = 5
    Private Const GVC_vStartId As Integer = 6
    Private Const GVC_vEndId As Integer = 7
    Private Const GVC_dReleaseDate As Integer = 8
    Private Const GVC_vUserName As Integer = 9
    Private Const GVC_vProjectNo As Integer = 10
    Private Const GVC_vComments As Integer = 11

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
                Me.ViewState(VS_Choice) = Choice   'To use it while saving
                If Not Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                    GenCall()
                Else
                    FillddlProject()
                    FillUserDepartment()
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
            Page.Title = ":: Release Document Trail  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblMandatory"), Label).Visible = False
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Released Document Trail"

            Me.AutoCompleteExtender1.ContextKey = "1=1"

            FillddlProject()
            FillUserDepartment()

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

    Protected Sub gvReleaseDetail_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvReleaseDetail.RowCreated
        Try
            If e.Row.RowType = DataControlRowType.Header Then
                'Build custom header.
                Dim oGridView As GridView = DirectCast(sender, GridView)
                Dim oGridViewRow As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)

                Dim oTableCell As New TableCell
                oTableCell.Text = "File Name"
                oTableCell.ColumnSpan = 1
                oTableCell.RowSpan = 2
                oTableCell.VerticalAlign = VerticalAlign.Middle
                oTableCell.BorderColor = Drawing.Color.White
                oGridViewRow.Cells.Add(oTableCell)

                oTableCell = New TableCell
                oTableCell.Text = "Qty"
                oTableCell.ColumnSpan = 1
                oTableCell.RowSpan = 2
                oTableCell.VerticalAlign = VerticalAlign.Middle
                oTableCell.BorderColor = Drawing.Color.White
                oGridViewRow.Cells.Add(oTableCell)

                oTableCell = New TableCell
                oTableCell.Text = "Form"
                oTableCell.ColumnSpan = 1
                oTableCell.RowSpan = 2
                oTableCell.VerticalAlign = VerticalAlign.Middle
                oTableCell.BorderColor = Drawing.Color.White
                oGridViewRow.Cells.Add(oTableCell)

                oTableCell = New TableCell
                oTableCell.Text = "Form Nos."
                oTableCell.ColumnSpan = 2
                oTableCell.VerticalAlign = VerticalAlign.Middle
                oTableCell.BorderColor = Drawing.Color.White
                oGridViewRow.Cells.Add(oTableCell)

                oTableCell = New TableCell
                oTableCell.Text = "Released On"
                oTableCell.ColumnSpan = 1
                oTableCell.RowSpan = 2
                oTableCell.VerticalAlign = VerticalAlign.Middle
                oTableCell.BorderColor = Drawing.Color.White
                oGridViewRow.Cells.Add(oTableCell)

                oTableCell = New TableCell
                oTableCell.Text = "Released By"
                oTableCell.ColumnSpan = 1
                oTableCell.RowSpan = 2
                oTableCell.VerticalAlign = VerticalAlign.Middle
                oTableCell.BorderColor = Drawing.Color.White
                oGridViewRow.Cells.Add(oTableCell)

                oTableCell = New TableCell
                oTableCell.Text = "Project No."
                oTableCell.ColumnSpan = 1
                oTableCell.RowSpan = 2
                oTableCell.VerticalAlign = VerticalAlign.Middle
                oTableCell.BorderColor = Drawing.Color.White
                oGridViewRow.Cells.Add(oTableCell)

                oTableCell = New TableCell
                oTableCell.Text = "Comments"
                oTableCell.ColumnSpan = 1
                oTableCell.RowSpan = 2
                oTableCell.VerticalAlign = VerticalAlign.Middle
                oTableCell.BorderColor = Drawing.Color.White
                oGridViewRow.Cells.Add(oTableCell)

                oGridViewRow.Style.Add("text-align", "center")
                oGridView.Controls(0).Controls.AddAt(0, oGridViewRow)

                '================================== Sencond Row ========================================
                Dim oGridViewRow1 As New GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert)
                oTableCell = New TableCell
                oTableCell.Text = "From"
                oTableCell.BorderColor = Drawing.Color.White
                oGridViewRow1.Cells.Add(oTableCell)

                oTableCell = New TableCell
                oTableCell.Text = "To"
                oTableCell.BorderColor = Drawing.Color.White
                oGridViewRow1.Cells.Add(oTableCell)

                oGridViewRow1.Style.Add("text-align", "center")
                oGridView.Controls(0).Controls.AddAt(1, oGridViewRow1)

            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub gvReleaseDetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvReleaseDetail.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Or e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(GVC_vWorkspaceId).Visible = False
                e.Row.Cells(GVC_iParentNodeId).Visible = False
                e.Row.Cells(GVC_iReleasedBy).Visible = False
            End If

            If e.Row.RowType = DataControlRowType.Header Then
                e.Row.Visible = False
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
    ''' ADDED ON :: 15-MAY-2012
    ''' REASON   :: FOR FILLING SOP NUMBER DROPDOWN.
    ''' =======================================================================================
    ''' 

    Public Function FillddlSopNo() As Boolean
        Dim dr() As DataRow = Nothing
        Dim ds_Result As DataSet = Nothing
        Dim count As Integer = 0
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim wsId As String = String.Empty
        Dim dvData As DataView

        Try

            wsId = ddlProject.SelectedValue.Trim

            wstr = "vWorkspaceId = '" + wsId + "' AND vMedExCode = '00085' and iParentNodeId = 1"
            If Not objHelp.View_CRFSubDtlForCategory_OLD(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Result, estr) Then
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
                'dr = ds_Result.Tables(0).Select("vMedExResult = ''")
                'If dr.Length > 0 Then
                '    For Each dr1 As DataRow In dr
                '        ddlCategory.Items.Add(New ListItem(dr1("vNodeDisplayName").ToString, dr1("iNodeId").ToString))
                '    Next
                '    ddlCategory.Items.Insert(0, New ListItem("Select Form", ""))
                '    ddlSOPNo.SelectedIndex = 1
                'End If
                '======================================================
                ds_Result.Tables(0).Columns.Add("iNodeIdTranNo", GetType(String))
                ds_Result.AcceptChanges()

                For Each dr_Forms As DataRow In ds_Result.Tables(0).Rows
                    dr_Forms("iNodeIdTranNo") = dr_Forms("iNodeId").ToString + "@@@" + dr_Forms("iTranNo").ToString
                Next

                ds_Result.Tables(0).AcceptChanges()

                ds_Result.Tables(0).DefaultView.RowFilter = "vMedExResult = ''"

                If ds_Result.Tables(0).DefaultView.ToTable.Rows.Count > 0 Then
                    'For Each dr1 As DataRow In ds_Result.Tables(0).DefaultView.ToTable.Rows
                    '    ddl1.Items.Add(New ListItem(dr1("vNodeDisplayName").ToString, dr1("iNodeIdTranNo").ToString))
                    'Next

                    dvData = ds_Result.Tables(0).DefaultView
                    ddlCategory.DataSource = dvData.ToTable(True, "iNodeIdTranNo,vNodeDisplayName".Split(","))
                    ddlCategory.DataValueField = "iNodeIdTranNo"
                    ddlCategory.DataTextField = "vNodeDisplayName"
                    ddlCategory.DataBind()
                    ddlCategory.Items.Insert(0, New ListItem("Select Form", ""))

                    ddlSOPNo.SelectedIndex = 1
                End If
                '=============================================================
            Else
                ObjCommon.ShowAlert("No SOP found for this Department.", Me.Page)
            End If
        Catch ex As Exception
            Return ex.Message.ToString
        End Try
    End Function

    ''' =======================================================================================
    ''' ADDED BY :: JUGAL KUNDAL
    ''' ADDED ON :: 15-MAY-2012
    ''' REASON   :: FILLING CATEGORY BASED ON THE SOP SELECTED FROM THE DROPDOWN.
    ''' =======================================================================================
    ''' 

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
            If Not objHelp.View_CRFSubDtlForCategory_OLD(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Result, estr) Then
                Throw New Exception(estr)
            End If

            ds_Result.Tables(0).Columns.Add("iNodeIdTranNo", GetType(String))
            ds_Result.AcceptChanges()

            For Each dr As DataRow In ds_Result.Tables(0).Rows
                dr("iNodeIdTranNo") = dr("iNodeId").ToString + "@@@" + dr("iTranNo").ToString
            Next

            If Not ds_Result Is Nothing And ds_Result.Tables(0).Rows.Count > 0 Then
                ddlCategory.DataSource = ds_Result.Tables(0).DefaultView
                ddlCategory.DataValueField = "iNodeIdTranNo"
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

    Public Function FillUserDepartment() As Boolean
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim dsDept As New DataSet

        Try
            wstr = "cStatusIndi <> 'D'"
            If Not objHelp.GetDeptMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsDept, estr) Then
                Throw New Exception(estr)
            End If

            If Not dsDept Is Nothing And dsDept.Tables(0).Rows.Count > 0 Then
                Me.ddlUserDept.DataSource = dsDept.Tables(0).DefaultView
                Me.ddlUserDept.DataValueField = "vDeptCode"
                Me.ddlUserDept.DataTextField = "vDeptName"
                Me.ddlUserDept.DataBind()
                Me.ddlUserDept.Items.Insert(0, New ListItem("Select User Department", ""))
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Function

    'Public Function FillForm() As Boolean

    '    Dim ds_Result As DataSet = Nothing
    '    Dim wstr As String = String.Empty
    '    Dim estr As String = String.Empty
    '    Dim wsId As String = String.Empty
    '    Dim SopNo As String = String.Empty
    '    Try
    '        wsId = ddlProject.SelectedValue.Trim
    '        SopNo = ddlSOPNo.SelectedValue.Trim

    '        wstr = "vWorkspaceId = '" + wsId + "' AND vMedExResult = '" + SopNo + "' AND vMedExCode = '00085' and iParentNodeId = 1"
    '        If Not objHelp.View_CRFSubDtlForCategory(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Result, estr) Then
    '            Throw New Exception(estr)
    '        End If

    '        If Not ds_Result Is Nothing And ds_Result.Tables(0).Rows.Count > 0 Then
    '            ddlForm.DataSource = ds_Result.Tables(0).DefaultView
    '            ddlForm.DataValueField = "iNodeId"
    '            ddlForm.DataTextField = "vNodeDisplayName"
    '            ddlForm.DataBind()
    '            ddlForm.Items.Insert(0, New ListItem("Select Form", ""))
    '        Else
    '            ObjCommon.ShowAlert("No Form found for this SOP.", Me.Page)
    '        End If
    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.Message, "")
    '    End Try
    'End Function

#Region "Commented Code"
    '' =====================================================================================
    '' This Function is used before the sop filteration is added to the page to fill the categories.
    '' =====================================================================================
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
    '            ObjCommon.ShowAlert("You do not have rights on Categories for this Project.", Me.Page)
    '        End If


    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.Message, "")
    '        Return False
    '    End Try
    'End Function
#End Region



#End Region

#Region "Fill Grid"

    Public Function FillgvReleaseDetail() As Boolean
        Dim ds_Result As DataSet = Nothing
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim dc_dModifyOn_IST As DataColumn
        Try
            'wstr = "vWorkspaceId = '" + ddlProject.SelectedItem.Value.Trim + "' AND iParentNodeId = " + ddlCategory.SelectedItem.Value.Trim
            'wstr += " Order By iWorkspaceNodeHistoryTranNo desc, dReleaseDate Desc"

            wstr = ""
            If Me.ddlProject.SelectedIndex <> 0 Then
                wstr += "vWorkspaceId = '" + ddlProject.SelectedItem.Value.Trim + "' AND "
            End If

            If Me.ddlCategory.SelectedIndex > 0 Then
                wstr += "iParentNodeId = " + ddlCategory.SelectedValue.ToString.Split("@@@")(0) + " AND iNodeIdTranNo = '" + ddlCategory.SelectedValue.ToString.Split("@@@")(3) + "' AND "
            End If

            If Me.ddlUserDept.SelectedIndex > 0 Then
                wstr += "vDeptCode = '" + Me.ddlUserDept.SelectedItem.Value.Trim + "' AND "
            End If

            If Me.txtDateFrom.Text.ToString.Trim <> "" AndAlso Me.txtDateTo.Text.ToString.Trim <> "" Then
                wstr += "CONVERT(DATE,dReleaseDate) between '" + Date.Parse(Me.txtDateFrom.Text.ToString.Trim) + "' AND '" + Date.Parse(Me.txtDateTo.Text.ToString.Trim) + "' AND "
            End If

            If Me.ddlSOPNo.SelectedIndex > 0 Then
                wstr += "vSOPNo = '" + Me.ddlSOPNo.SelectedItem.Text.Trim + "' AND "
            End If

            If Me.ddlSOPType.SelectedIndex > 0 Then
                wstr += "vSopType = '" + Me.ddlSOPType.SelectedItem.Text.Trim + "' AND "
            End If

            If Me.hdnProjectNo.Value <> "" Then
                wstr += "vProjectNo = '" + Me.hdnProjectNo.Value.Trim + "' AND "
            End If

            If wstr.Trim = "" Then
                'ObjCommon.ShowAlert("Please Select Atleast One Field.", Me.Page)
                Exit Function
            End If

            wstr = wstr.Substring(0, wstr.Length - 4)
            wstr += " AND vFileName IS NOT NULL Order By iWorkspaceNodeHistoryTranNo desc, dReleaseDate Desc"

            If Not objHelp.View_DocReleaseTrack(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Result, estr) Then
                Throw New Exception(estr)
            End If

            If ds_Result.Tables(0).Rows.Count > 0 Then
                dc_dModifyOn_IST = New DataColumn("dModifyOn_IST", System.Type.GetType("System.String"))
                ds_Result.Tables(0).Columns.Add("dModifyOn_IST")
                ds_Result.AcceptChanges()
                For Each dr_dModifyOn In ds_Result.Tables(0).Rows
                    'dr_dModifyOn("dModifyOn_IST") = Convert.ToString(dr_dModifyOn("dReleaseDate") + " IST (+5.5 GMT)")
                    dr_dModifyOn("dModifyOn_IST") = Convert.ToString(CDate(dr_dModifyOn("dReleaseDate")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                Next
                ds_Result.AcceptChanges()
                Me.ViewState(VS_gvReleaseDetail) = ds_Result.Tables(0)
                Me.gvReleaseDetail.DataSource = ds_Result
                Me.gvReleaseDetail.DataBind()
                fldReleaseDetail.Style.Add("Display", "")
                fldReleaseMsg.Style.Add("Display", "none")
            Else
                Me.ViewState(VS_gvReleaseDetail) = Nothing
                fldReleaseMsg.Style.Add("Display", "")
                fldReleaseDetail.Style.Add("Display", "none")
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
            ddlSOPNo.Items.Clear()
            ddlCategory.Items.Clear()

            If ddlProject.SelectedIndex <> 0 Then
                'FillddlCategory()
                FillddlSopNo()
            Else
                ddlCategory.Items.Clear()
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub ddlSOPNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSOPNo.SelectedIndexChanged
        Try
            fldReleaseDetail.Style.Add("Display", "none")
            fldReleaseMsg.Style.Add("Display", "none")
            ddlCategory.Items.Clear()

            If ddlSOPNo.SelectedIndex <> 0 Then
                FillCategory()
            Else
                fldReleaseDetail.Style.Add("Display", "none")
                fldReleaseMsg.Style.Add("Display", "none")
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub ddlCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCategory.SelectedIndexChanged
        Try
            If ddlCategory.SelectedIndex <> 0 Then
                'FillgvReleaseDetail()
            Else
                fldReleaseDetail.Style.Add("Display", "none")
                fldReleaseMsg.Style.Add("Display", "none")
            End If

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
            If Not ExportToExcel("Released Documents Trails", CType(Me.ViewState(VS_gvReleaseDetail), DataTable)) Then
                Me.ObjCommon.ShowAlert("Error Occured While Exporting To Excel!", Me.Page)
                Exit Sub
            End If
        Catch ex As Exception
            Me.ObjCommon.ShowAlert(ex.ToString, Me.Page)
            Exit Sub
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            If Not FillgvReleaseDetail() Then
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Response.Redirect("frmMainPage.aspx")
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

            Status = "Released Documents Trails"
            columnList = "vFileName,iQty,vNodeDisplayName,vStartId,vEndId,dModifyOn_IST,vUserName,ProjectNumber,vComments"

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

            dsConvert.Tables(0).Columns(0).ColumnName = "File Name"
            dsConvert.Tables(0).Columns(1).ColumnName = "Qty"
            dsConvert.Tables(0).Columns(2).ColumnName = "Form"
            dsConvert.Tables(0).Columns(3).ColumnName = "From"
            dsConvert.Tables(0).Columns(4).ColumnName = "To"
            dsConvert.Tables(0).Columns(5).ColumnName = "Released On"
            dsConvert.Tables(0).Columns(6).ColumnName = "Released By"
            dsConvert.Tables(0).Columns(7).ColumnName = "Project No."
            dsConvert.Tables(0).Columns(8).ColumnName = "Comments"

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