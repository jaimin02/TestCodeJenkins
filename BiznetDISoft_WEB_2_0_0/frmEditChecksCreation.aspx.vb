
Partial Class frmEditChecksCreation
    Inherits System.Web.UI.Page

#Region "Variable Declaration "

    Private objCommon As New clsCommon
    Private objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()


    Private Const VS_DtEditChecks As String = "DtEditChecks"
    Private Const VS_DtEditCheckUpdate As String = "DtEditCheckUpdate"
    Private Const VS_EditCheckData As String = "EditCheckData"
    Private Const VS_DtGrid As String = "DtGrid"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_iPeriod As Integer = 1
    Private Const GVC_Visit As Integer = 2
    Private Const GVC_EditCheckNo As Integer = 8
    Private Const GVC_WorkspaceID As Integer = 9
    Private Const GVC_Period As Integer = 10
    Private Const GVC_ActivityID As Integer = 11
    Private Const GVC_ParentNodeID As Integer = 12

    Private ProjectLock As Boolean = False



#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            Me.Page.Title = " :: Edit Checks ::" + System.Configuration.ConfigurationManager.AppSettings("Client")
            If Not Page.IsPostBack Then
                If Not GenCall_Data() Then
                    Throw New Exception("Error While calling GenCall_Data()")
                End If
            End If

            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                Me.trVisit.Style.Add("display", "table-row")
                Me.trPeriod.Style.Add("display", "none")
            Else
                Me.trPeriod.Style.Add("display", "table-row")
                'Me.trVisit.Style.Add("display", "none")
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try

    End Sub

#End Region

#Region "GenCall Data"

    Private Function GenCall_Data() As Boolean

        Dim dsEditChecks As DataSet = Nothing
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim sender As Object
        Dim e As EventArgs

        Try

            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
            End If

            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID) & " And cWorkspaceType = 'P'"

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Edit Check Creation"
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            wStr = "select * from MedExWorkspaceEditChecks where 1=2"

            dsEditChecks = objhelpDb.GetResultSet(wStr, "MedExWorkspaceEditChecks")

            If dsEditChecks Is Nothing Then
                Throw New Exception(eStr)
            End If

            Me.ViewState(VS_DtEditChecks) = dsEditChecks.Tables(0).Copy()
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)
            Return False
        End Try

    End Function

#End Region

#Region "Set Project"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim ds_CrfLockDetail As DataSet = Nothing
        'Dim VersionNo As Integer = 0
        'Dim VersionDate As Date
        Dim wstr As String = String.Empty
        Dim eStr_Retu As String = String.Empty

        Try

            ' ''====== CRFVersion Control==================================

            'wstr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'"

            'If Not objhelpDb.GetData("CRFVersionMst", "*", wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CrfVersionDetail, eStr_Retu) Then
            '    Throw New Exception(eStr_Retu)
            'End If

            'If ds_CrfVersionDetail.Tables(0).Rows.Count > 0 Then
            '    VersionNo = ds_CrfVersionDetail.Tables(0).Rows(0)("nVersionNo")
            '    VersionDate = ds_CrfVersionDetail.Tables(0).Rows(0)("dVersiondate").ToString
            '    Me.VersionNo.Text = VersionNo.ToString
            '    Me.VersionDate.Text = VersionDate.ToString("dd/MM/yyyy")
            '    Me.VersionDtl.Attributes.Add("style", " ")
            '    If ds_CrfVersionDetail.Tables(0).Rows(0)("cFreezeStatus").ToString.Trim.ToUpper = "U" Then
            '        ImageLockUnlock.Attributes.Add("src", "../images/UnFreeze.jpg")
            '        objCommon.ShowAlert("Project Is In UnFreeze State, First Freeze It Then Proceed", Me.Page)
            '        Exit Sub

            '    End If
            'Else
            '    Me.VersionDtl.Attributes.Add("style", "display:none;")
            'End If
            ' ''==========================================================

            'wstr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'"
            'If Not objhelpDb.GetCRFLockDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CrfLockDetail, eStr_Retu) Then
            '    Throw New Exception(eStr_Retu)
            'End If
            'If ds_CrfLockDetail.Tables(0).Rows.Count > 0 Then
            '    ds_CrfLockDetail.Tables(0).DefaultView.RowFilter = " iTranNo = Max(iTranNo)"
            '    If ds_CrfLockDetail.Tables(0).DefaultView(0)("cLockFlag") = "L" Then
            '        objCommon.ShowAlert("Project Is Locked.", Me.Page)
            '        Me.btnInsertAttribute.Enabled = False
            '        Me.ddlOperator.Enabled = False
            '        Me.txtValue.Enabled = False
            '        Me.btnAND.Enabled = False

            '        'Exit Sub
            '    End If
            'End If

            ' Added By Jeet Patel on 22-Jun-2015 to Check Project Status and Give alert 
            If CheckProjectStatus() Then
                Me.objCommon.ShowAlert("Project is Locked", Me.Page())
            End If
            ' =========================================================================

            If Not FillPeriod() Then
                Throw New Exception("Error While Filling Period")
            End If

            'If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
            '    Me.ddlVisit.Focus()
            'Else
            '    Me.ddlPeriod.Focus()
            'End If


        Catch ex As Exception
            objCommon.ShowAlert(ex.ToString, Me.Page)
        End Try
    End Sub

#End Region

#Region "Control's Events"

    Protected Sub ddlActivity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlActivity.SelectedIndexChanged

        Try
            If Not FillAttribute() Then
                Throw New Exception("Error While Filling Attribute")
            End If

            ' Added By Jeet Patel on 22-Jun-2015 to Check Project Status and Give alert 
            If CheckProjectStatus() Then
                Exit Sub
            End If
            ' =========================================================================

            Me.pnlMedExAttributeValues.Style.Add("display", "none")
            Me.txtValue.Style.Add("display", "inline")
            'Me.pnlMedExAttributeValues.Visible = False
            'Me.txtValue.Visible = True
            Me.chkAllActivity.Checked = False
            Me.btnInsertAttribute.Enabled = True
            Me.ddlOperator.Enabled = True
            Me.txtValue.Enabled = True
            Me.btnAND.Enabled = True
            Me.btnOR.Enabled = True
            Me.btnViewEditChecks.Focus()
            Me.btnSave.Enabled = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try

    End Sub

    Protected Sub ddlPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPeriod.SelectedIndexChanged
        Try
            If Not FillActivity() Then
                Throw New Exception("Error While Filling Activity")
            End If
            Me.ddlActivity.Focus()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try
    End Sub


    Protected Sub ddlVisit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlVisit.SelectedIndexChanged
        Try

            If Not FillActivityF() Then
                Throw New Exception("Error While Filling Activity")
            End If

            Me.ddlActivity.Focus()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try
    End Sub



    Protected Sub lstAttribute_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstAttribute.PreRender
        Try
            If lstAttribute.Items.Count > 0 Then
                For Each item As ListItem In lstAttribute.Items
                    If item.Value <> "" Then
                        item.Attributes.Add("title", item.Text)
                        'item.Attributes.Add("title", item.Value.Split("#")(5) + " - " + item.Value.Split("#")(4) + " - " _
                        '             + item.Value.Split("#")(2))
                    End If
                Next
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try

    End Sub

    Protected Sub btnHdnMedExValues_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHdnMedExValues.Click
        Dim LiValue As String = String.Empty
        Try
            If hdnMedExValues.Value.Trim <> "" Then
                rbtnlstMedExValues.Items.Clear()
                For Each strValue As String In hdnMedExValues.Value.Split(",")
                    If strValue.Trim() <> "" Then
                        rbtnlstMedExValues.Items.Add(strValue.Trim())
                    End If
                Next
                For Each li As ListItem In rbtnlstMedExValues.Items
                    LiValue = Convert.ToString(li.Value)
                    LiValue = LiValue.Replace("'", "\'")         '' Added by ketan for Handle ' 
                    li.Attributes.Add("onclick", "fnInsertRadioValue('" + LiValue + "')")
                Next
                Me.pnlMedExAttributeValues.Style.Add("display", "block")
                Me.txtValue.Style.Add("display", "none")
                'Me.pnlMedExAttributeValues.Visible = True
                'Me.txtValue.Visible = False
                Me.tdValues.VAlign = "top"


            Else
                'Me.pnlMedExAttributeValues.Visible = False
                'Me.txtValue.Visible = True
                Me.pnlMedExAttributeValues.Style.Add("display", "none")
                Me.txtValue.Style.Add("display", "inline")
                Me.tdValues.VAlign = "middle"
            End If
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                If Me.rdbEditChecksType.Items(1).Selected Then
                    Me.chkAllActivity.Visible = False
                    Me.fldsData.Visible = False
                End If
            End If
            If hdnMedExType.Value.ToString.ToUpper = "STANDARDDATE" Then
                Me.txtDate.Style.Add("display", "inline")
                Me.txtValue.Style.Add("display", "none")
            Else
                Me.txtDate.Style.Add("display", "none")
                Me.txtValue.Style.Add("display", "inline")
            End If
            Me.ddlOperator.Focus()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Hide123", "hide1();", True)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Hide12", "hide1();", True)
            Me.ShowErrorMessage(ex.ToString, "")
        End Try
    End Sub

    Protected Sub rdbEditChecksType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbEditChecksType.SelectedIndexChanged
        Me.chkAllActivity.Checked = False
        If Me.rdbEditChecksType.Items.Item(0).Selected Then
            Me.chkAllActivity.Text = "Attach To All Same Activities ?"
        ElseIf Me.rdbEditChecksType.Items.Item(1).Selected Then
            Me.chkAllActivity.Text = "Attach To All Same Periods ?"
        End If
    End Sub

    Protected Sub chkAllActivity_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAllActivity.CheckedChanged


        Dim tbl As New HtmlTable
        Dim row As New HtmlTableRow
        Dim cell As HtmlTableCell
        Dim div As New HtmlGenericControl("div")
        Dim dsPeriod As New DataSet
        Dim dtPeriod As New DataTable
        Dim chkPeriod As New CheckBoxList
        Dim lstItem As New ListItem
        Dim Count As Integer = 0
        Dim dvPeriod As DataView

        Try


            If chkAllActivity.Checked Then
                If Me.HProjectId.Value.Trim() = "" Then
                    objCommon.ShowAlert("Please Enter Project.", Me)
                    Me.chkAllActivity.Checked = False
                    Exit Sub
                ElseIf Me.ddlPeriod.SelectedItem.Text.ToLower = "select period" Then
                    objCommon.ShowAlert("Please Select Period.", Me)
                    Me.chkAllActivity.Checked = False
                    Exit Sub
                ElseIf Me.ddlActivity.SelectedItem.Text.ToLower = "select activity" Then
                    objCommon.ShowAlert("Please Select Activity.", Me)
                    Me.chkAllActivity.Checked = False
                    Exit Sub
                End If
            End If

            If Me.chkAllActivity.Checked AndAlso Me.rdbEditChecksType.Items.Item(0).Selected Then
                If Me.ddlActivity.SelectedValue.ToString.Trim() <> "" And _
                    Me.ddlActivity.SelectedValue.ToString.Trim().ToLower() <> "select activity" Then


                    Dim pnl As Panel
                    dsPeriod = Me.objhelpDb.GetResultSet("select iNodeID,vNodeDisplayName,iPeriod,ActivityWithParent,vActivityId " + _
                                                         " from View_WorkspaceNodeDetail where vWorkspaceId='" + Me.HProjectId.Value + "'" + _
                                                         " And cStatusIndi<>'D' AND vActivityId = '" + ddlActivity.SelectedValue.Split("#")(1).ToString + "'" + _
                                                         " Order by nRefTime,iNodeID", "PeriodDetails")

                    If Not dsPeriod Is Nothing Then

                        dtPeriod = dsPeriod.Tables(0).DefaultView.ToTable(True, "iPeriod")
                        dtPeriod.DefaultView.Sort = "iPeriod ASC"

                        For Each dr As DataRow In dtPeriod.DefaultView.ToTable().Rows

                            dvPeriod = dsPeriod.Tables(0).DefaultView
                            dvPeriod.RowFilter = "iPeriod = " + dr("iPeriod").ToString

                            Count += 1

                            cell = New HtmlTableCell
                            chkPeriod = New CheckBoxList

                            chkPeriod.ID = "Period_" + dr("iPeriod").ToString

                            For Each drItem As DataRow In dvPeriod.ToTable().Rows
                                lstItem = New ListItem
                                lstItem.Text = drItem("vNodeDisplayName")
                                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                                    lstItem.Text = drItem("ActivityWithParent")
                                End If
                                lstItem.Value = drItem("iNodeID")
                                lstItem.Attributes.Add("NodeID", drItem("iNodeID").ToString() + "#" + dr("iPeriod").ToString())
                                lstItem.Attributes.Add("title", "(" + drItem("vActivityId").ToString() + ")")
                                chkPeriod.Items.Add(lstItem)
                            Next
                            'chkPeriod.DataSource = dvPeriod.ToTable()
                            'chkPeriod.DataTextField = "vNodeDisplayName"
                            'chkPeriod.DataValueField = "iNodeID"

                            chkPeriod.DataBind()
                            chkPeriod.Items.Insert(0, "Select All")
                            chkPeriod.Items(0).Attributes.Add("onclick", "fnSelectAllPeriod(this);")

                            chkPeriod.Style.Add("text-align", "left")
                            chkPeriod.Style.Add("margin", "15px 10px 10px 10px")
                            chkPeriod.Style.Add("font-size", "8pt")

                            pnl = New Panel
                            pnl.GroupingText = "<span class='Label'>Period " + dr("iPeriod").ToString + "</span>"
                            pnl.Style.Add("width", "100%")

                            pnl.Controls.Add(chkPeriod)

                            If ((Count Mod 2) <> 0) Then
                                row = New HtmlTableRow
                            End If

                            cell.Controls.Add(pnl)
                            cell.Style.Add("width", "50%")
                            cell.Attributes.Add("vAlign", "top")
                            row.Cells.Add(cell)
                            tbl.Rows.Add(row)

                        Next
                        tbl.Attributes.Add("width", "100%")
                        Me.tdAllActivity.Controls.Add(tbl)

                    End If
                End If
            End If
        Catch ex As Exception

            Me.ShowErrorMessage(ex.ToString, "")

        End Try

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ResetPage()
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim dsEditChecks As New DataSet
        Dim eStr As String = String.Empty

        Try

            If Not AssignValues("ADD") Then
                Throw New Exception("Error while assigning values.")
            End If

            If Not Me.ViewState(VS_DtEditChecks) Is Nothing Then

                dsEditChecks.Tables.Add(CType(Me.ViewState(VS_DtEditChecks), DataTable).Copy())
                dsEditChecks.Tables(0).TableName = "Insert_MedExWorkspaceEditChecks"

                If Not objLambda.TableInsert(dsEditChecks, Me.Session(S_UserID), eStr) Then
                    Throw New Exception("Error while saving MedExWorkspaceEditChecks")
                Else
                    objCommon.ShowAlert("Edit Checks saved successfully", Me.Page)
                    ResetPage(False)
                    Me.pnlMedExAttributeValues.Style.Add("display", "none")
                    Me.txtValue.Style.Add("display", "inline")
                    Me.txtDate.Style.Add("display", "none")
                    Me.txtDateBetween.Style.Add("display", "none")
                    'Me.pnlMedExAttributeValues.Visible = False
                    'Me.txtValue.Visible = True 

                    ' Added By Jeet Patel on 22-Jun-2015 for Create another editcheck after creating one edit check
                    Me.btnInsertAttribute.Enabled = True
                    Me.ddlOperator.Enabled = True
                    Me.txtValue.Enabled = True
                    Me.btnAND.Enabled = True
                    Me.btnOR.Enabled = True

                    ' ===============================================================================

                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try
    End Sub

    Protected Sub btnInsert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInsert.Click
        btnHdnMedExValues_Click(sender, e)
    End Sub

    Protected Sub btnViewEditChecks_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnViewEditChecks.Click

        Try

            If Me.btnViewEditChecks.Text.ToLower() = "create" Then
                Me.tblFormula.Visible = True
                Me.tblAllActivity.Visible = True
                Me.tblButton.Visible = True
                Me.fldsData.Visible = True
                Me.fldsFormula.Visible = True
                Me.tblData.Visible = False
                Me.rdbEditChecksType.Visible = True
                Me.btnViewEditChecks.Text = "View"
                Me.ResetPage(False, "Create")
                'Me.pnlMedExAttributeValues.Visible = False
                'Me.txtValue.Visible = True
                Me.pnlMedExAttributeValues.Style.Add("display", "none")
                Me.txtValue.Style.Add("display", "inline")
                Me.btnExportToExcel.Style.Add("display", "none")

                If Me.ddlActivity.SelectedIndex <> 0 _
                   And Me.ddlActivity.Items.Count > 0 Then
                    Me.btnInsertAttribute.Enabled = True
                    Me.ddlOperator.Enabled = True
                    Me.txtValue.Enabled = True
                    Me.btnAND.Enabled = True
                    Me.btnOR.Enabled = True
                End If

            Else
                Dim index As String = ddlActivity.SelectedIndex
                FillGrid(False)
                Me.ddlActivity.SelectedIndex = Me.ddlActivity.SelectedIndex
                Me.tblFormula.Visible = False
                Me.tblAllActivity.Visible = False
                Me.tblButton.Visible = False
                Me.fldsData.Visible = False
                Me.fldsFormula.Visible = False
                Me.tblData.Visible = True
                Me.rdbEditChecksType.Visible = False
                Me.btnViewEditChecks.Text = "View"
                Me.btnExportToExcel.Style.Add("display", "inline")
                Me.btnViewCancel.Visible = True
            End If


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..btnViewEditChecks_Click")
        End Try

    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Me.pnlMedExAttributeValues.Style.Add("display", "none")
        Me.txtDate.Style.Add("display", "none")
        Me.txtValue.Style.Add("display", "inline")
        Me.fldsData.Visible = True
        Me.chkAllActivity.Visible = True



    End Sub

    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
        Dim eStr As String = String.Empty

        Try
            If Me.grdViewData.Rows.Count < 1 Then
                Me.objCommon.ShowAlert("No data to Export", Me.Page)
                Exit Sub
            End If
            If Not ExportToExcel(CType(Me.ViewState(VS_EditCheckData), DataTable), "EditChecks") Then
                Throw New Exception("Problem while Exporting data")
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Exit Sub
        End Try
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim dsEditChecks As New DataSet
        Dim eStr As String = String.Empty

        Try

            If Me.txtMessage.Text.Trim() = "" Then
                objCommon.ShowAlert("Please Enter Discrepancy Message.", Me)
                Exit Sub
            End If

            If Not AssignValues("UPDATE") Then
                Throw New Exception("Error while assigning values.")
            End If

            If Not Me.ViewState(VS_DtEditChecks) Is Nothing Then

                dsEditChecks.Tables.Add(CType(Me.ViewState(VS_DtEditChecks), DataTable).Copy())
                dsEditChecks.Tables(0).TableName = "Insert_MedExWorkspaceEditChecks"

                If Not objLambda.TableInsert(dsEditChecks, Me.Session(S_UserID), eStr) Then
                    Throw New Exception("Error while updating MedExWorkspaceEditChecks")
                Else
                    objCommon.ShowAlert("Edit Checks update successfully", Me.Page)
                    Me.btnViewEditChecks_Click(sender, e)
                    Me.txtproject.Enabled = True
                    Me.ddlVisit.Enabled = True
                    Me.ddlActivity.Enabled = True
                End If

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try
    End Sub

    Protected Sub btnViewCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnViewCancel.Click
        Me.tblFormula.Visible = True
        Me.tblAllActivity.Visible = True
        Me.tblButton.Visible = True
        Me.fldsData.Visible = True
        Me.fldsFormula.Visible = True
        Me.tblData.Visible = False
        Me.rdbEditChecksType.Visible = True
        Me.ResetPage(False, "Create")
        'Me.pnlMedExAttributeValues.Visible = False
        'Me.txtValue.Visible = True
        Me.pnlMedExAttributeValues.Style.Add("display", "none")
        Me.txtValue.Style.Add("display", "inline")
        Me.btnExportToExcel.Style.Add("display", "none")

        If Me.ddlActivity.SelectedIndex <> 0 _
           And Me.ddlActivity.Items.Count > 0 Then
            Me.btnInsertAttribute.Enabled = True
            Me.ddlOperator.Enabled = True
            Me.txtValue.Enabled = True
            Me.btnAND.Enabled = True
            Me.btnOR.Enabled = True
        End If

        If CheckProjectStatus() Then
            Me.objCommon.ShowAlert("Project is Locked", Me.Page())
            'Exit Sub
        End If

        Me.btnViewCancel.Visible = False
    End Sub

#End Region

#Region "Grid Events"

    Protected Sub grdViewData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdViewData.PageIndexChanging

        Try

            Me.grdViewData.PageIndex = e.NewPageIndex
            FillGrid()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "PageIndexchaning...")
        End Try

    End Sub

    Protected Sub grdViewData_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdViewData.RowCommand

        Dim dtData As New DataTable
        Dim dtDelete As New DataTable
        Dim dsDelete As New DataSet
        Dim index As Integer = e.CommandArgument
        Dim eStr As String = String.Empty
        Dim wStr As String = ""


        Try


            dtData = CType(Me.ViewState(VS_EditCheckData), DataTable)

            If e.CommandName = "Preview" Or e.CommandName = "Edit" Then


                dtData.DefaultView.RowFilter = "nEditCheckNo = " + Me.grdViewData.Rows(index).Cells(GVC_EditCheckNo).Text
                Me.ViewState(VS_DtEditCheckUpdate) = dtData.DefaultView.ToTable()

                FillPeriod()
                Me.ddlPeriod.SelectedIndex = Me.ddlPeriod.Items.IndexOf(Me.ddlPeriod.Items.FindByText(dtData.DefaultView.ToTable().Rows(0)("iPeriod").ToString()))
                Me.ddlVisit.SelectedIndex = Me.ddlVisit.Items.IndexOf(Me.ddlVisit.Items.FindByValue(dtData.DefaultView.ToTable().Rows(0)("iParentNodeId").ToString()))

                'FillActivity(dtData.DefaultView.ToTable().Rows(0)("iPeriod"))
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "selectActivity", "fnSelectActivity(" + dtData.DefaultView.ToTable().Rows(0)("vActivityId").ToString() + ");", True)

                Me.txtFormula.Text = dtData.DefaultView.ToTable().Rows(0)("vQueryMessage")
                Me.txtMessage.Text = dtData.DefaultView.ToTable().Rows(0)("vErrorMessage")

                Me.txtproject.Enabled = False
                Me.ddlPeriod.Enabled = False
                Me.ddlActivity.Enabled = False
                Me.ddlVisit.Enabled = False
                Me.btnAND.Enabled = False
                Me.btnOR.Enabled = False
                Me.btnInsertAttribute.Enabled = False
                Me.ddlOperator.Enabled = False
                Me.txtValue.Enabled = False
                Me.btnUpdate.Visible = False
                If e.CommandName = "Preview" Then
                    Me.txtMessage.Enabled = False
                End If
                If e.CommandName = "Edit" Then
                    Me.btnUpdate.Visible = True
                End If
                Me.btnSave.Visible = False
                Me.btnValidate.Visible = False
                Me.btnCancel.Visible = True
                Me.chkAllActivity.Enabled = False
                Me.btnClear.Enabled = False

                Me.tblData.Visible = False
                Me.tblFormula.Visible = True
                Me.tblAllActivity.Visible = True
                Me.tblButton.Visible = True
                Me.fldsData.Visible = True
                Me.fldsFormula.Visible = True

                'Me.pnlMedExAttributeValues.Visible = False
                'Me.txtValue.Visible = True

                Me.pnlMedExAttributeValues.Style.Add("display", "none")
                Me.txtValue.Style.Add("display", "inline")
                Me.btnExportToExcel.Style.Add("display", "none")
                Me.rdbEditChecksType.Visible = True
                Me.btnViewEditChecks.Text = "View"


            ElseIf e.CommandName = "Delete" Then

                dtData.DefaultView.RowFilter = "nEditCheckNo = " + Me.grdViewData.Rows(index).Cells(GVC_EditCheckNo).Text

                dtDelete = dtData.DefaultView.ToTable()

                For Each dr As DataRow In dtDelete.Rows
                    dr("cStatusIndi") = "D"
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("dModifyOn") = System.DateTime.Now
                    If dr("cEditCheckType").ToString.ToLower() = "corss activity" Then
                        dr("cEditCheckType") = "C"
                    Else
                        dr("cEditCheckType") = "P"
                    End If
                Next


                dtDelete.TableName = "Insert_MedExWorkspaceEditChecks"
                dsDelete.Tables.Add(dtDelete)

                If Not objLambda.TableInsert(dsDelete, Me.Session(S_UserID), eStr) Then
                    Throw New Exception("Error while saving MedExWorkspaceEditChecks")
                Else
                    objCommon.ShowAlert("Edit Checks deleted successfully", Me.Page)
                    FillGrid()
                End If

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "RowCommand...")
        End Try

    End Sub

    Protected Sub grdViewData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdViewData.RowDataBound

        Try

            If e.Row.RowType = DataControlRowType.DataRow Or _
                e.Row.RowType = DataControlRowType.Header Or _
                e.Row.RowType = DataControlRowType.Footer Then

                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                    e.Row.Cells(GVC_iPeriod).Visible = False
                    e.Row.Cells(GVC_Visit).Visible = True
                Else
                    e.Row.Cells(GVC_iPeriod).Visible = True
                    e.Row.Cells(GVC_Visit).Visible = False
                End If

                e.Row.Cells(GVC_EditCheckNo).Visible = False
                e.Row.Cells(GVC_ActivityID).Visible = False
                e.Row.Cells(GVC_Period).Visible = False
                e.Row.Cells(GVC_WorkspaceID).Visible = False
                e.Row.Cells(GVC_ParentNodeID).Visible = False
            End If

            If e.Row.RowType = DataControlRowType.DataRow Then

                e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + 1 + (Me.grdViewData.PageSize * Me.grdViewData.PageIndex)

                'Conditon Added By Jeet Patel on 23-Jun-2015 to Hide Delete and edit Button If Project is Locked
                If ProjectLock Then
                    CType(e.Row.FindControl("lnkPreview"), ImageButton).CommandArgument = e.Row.RowIndex
                    CType(e.Row.FindControl("lnkPreview"), ImageButton).CommandName = "Preview"
                    CType(e.Row.FindControl("lnkDelete"), ImageButton).Visible = False
                    CType(e.Row.FindControl("lnkEdit"), ImageButton).Visible = False
                    '---------
                Else
                    CType(e.Row.FindControl("lnkDelete"), ImageButton).CommandArgument = e.Row.RowIndex
                    CType(e.Row.FindControl("lnkDelete"), ImageButton).CommandName = "Delete"
                    CType(e.Row.FindControl("lnkPreview"), ImageButton).CommandArgument = e.Row.RowIndex
                    CType(e.Row.FindControl("lnkPreview"), ImageButton).CommandName = "Preview"
                    CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
                    CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "Edit"
                End If
            End If

        Catch ex As Exception

            Me.ShowErrorMessage(ex.Message, "grdViewData_RowDataBound...")

        End Try

    End Sub

    Protected Sub grdViewData_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles grdViewData.RowDeleting

    End Sub

    Protected Sub grdViewData_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles grdViewData.RowEditing
        e.Cancel = True
    End Sub

#End Region

#Region "Fill Grid"

    Private Sub FillGrid(Optional ByVal flag As Boolean = True)

        Dim dsData As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            'wStr = " vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' AND iPeriod = " + Me.ddlPeriod.SelectedValue.ToString() + _
            '      " AND vActivityId = '" + Me.ddlActivity.SelectedValue.ToString.Trim().Split("#")(1).ToString() + "' AND cStatusIndi != 'D'" + _
            '      " ORDER BY dmodifyOn DESC"

            'Added By Jeet Patel on 23-Jun-2015
            If CheckProjectStatus() Then
                ProjectLock = True
            End If
            '=================================

            wStr = " vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"

            If Me.ddlPeriod.SelectedIndex > 0 Then
                wStr += " AND iPeriod = " + Me.ddlPeriod.SelectedValue.ToString()
            End If


            'If Me.ddlActivity.SelectedIndex > 0 Then
            '    wStr += " AND iPeriod = " + Me.ddlPeriod.SelectedValue.ToString()
            'End If


            'If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
            If Me.ddlVisit.SelectedIndex > 0 Then
                wStr += " AND iParentNodeID= " + Me.ddlVisit.SelectedValue.ToString()
                'Rahul Shah

            End If
            'End If
            If Me.ddlActivity.SelectedIndex <> 0 And Me.ddlActivity.Items.Count > 0 Then
                wStr += " AND iNodeId= " + Me.ddlActivity.SelectedValue.ToString.Trim().Split("#")(0).ToString()  '' Addded by Rahul Rupareliya  For All Scope instand of CT
            End If


            If Me.ddlActivity.SelectedIndex <> 0 And Me.ddlActivity.Items.Count > 0 Then
                wStr += " AND vActivityId = '" + Me.ddlActivity.SelectedValue.ToString.Trim().Split("#")(1).ToString() + "'"
            End If


            wStr += " AND cStatusIndi != 'D' ORDER BY dmodifyOn DESC"


            If Not objhelpDb.GetData("View_MedExWorkspaceEditChecks", "*", wStr, _
                                    WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    dsData, eStr) Then
                Throw New Exception(eStr)
            End If

            Me.grdViewData.DataSource = Nothing
            Me.grdViewData.DataBind()
            Me.lblNoData.Visible = True


            If Not dsData Is Nothing Then
                If dsData.Tables(0).Rows.Count > 0 Then
                    If flag = False Then
                        Me.grdViewData.PageIndex = 0
                    End If
                    Me.grdViewData.DataSource = dsData
                    Me.grdViewData.DataBind()
                    Me.lblNoData.Visible = False
                    Me.ViewState(VS_EditCheckData) = dsData.Tables(0)
                End If
            End If


        Catch ex As Exception

            Me.ShowErrorMessage(ex.Message, "FillGrid()...")

        End Try


    End Sub

#End Region

#Region "Fill Period "

    Private Function FillPeriod() As Boolean
        Dim ds_Period As DataSet = Nothing
        Dim eStr As String = String.Empty
        Try

            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                Me.ddlPeriod.Items.Insert(0, "1")
                ds_Period = Me.objhelpDb.GetResultSet("select iNodeId,vNodeDisplayName from WorkspaceNodeDetail where vWorkspaceId='" + Me.HProjectId.Value + _
                                                      "' And iParentNodeId = 1 And cStatusIndi<>'D' AND ( vTemplateId <> '0001' OR  vTemplateId IS NULl)   order by iNodeNo", "WorkspaceNodeDetail")
                Me.ddlVisit.DataSource = ds_Period.Tables(0)
                Me.ddlVisit.DataValueField = "iNodeId"
                Me.ddlVisit.DataTextField = "vNodeDisplayName"
                Me.ddlVisit.DataBind()
                Me.ddlVisit.Items.Insert(0, "Select Visit/Parent Activity")

            ElseIf Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                ds_Period = Me.objhelpDb.GetResultSet("select distinct iPeriod , iPeriod as iPeriod1 from WorkspaceNodeDetail where vWorkspaceId='" + Me.HProjectId.Value + "'And cStatusIndi<>'D' AND ( vTemplateId <> '0001' OR  vTemplateId IS NULl)   ", "WorkspaceNodeDetail")
                Me.ddlPeriod.DataSource = ds_Period.Tables(0)
                Me.ddlPeriod.DataValueField = "iPeriod"
                Me.ddlPeriod.DataTextField = "iPeriod1"
                Me.ddlPeriod.DataBind()
                Me.ddlPeriod.Items.Insert(0, "Select Period")
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
            Return False
        End Try
    End Function

#End Region

#Region "Fill Activity"

    Private Function FillActivity(Optional ByVal iPeriod As Integer = 0) As Boolean
        Dim ds_Activity As DataSet = Nothing
        Dim eStr As String = String.Empty
        Try

            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                If Me.ddlVisit.SelectedValue.ToString.Trim() <> "" And _
                   Me.ddlVisit.SelectedValue.ToString.Trim().ToLower() <> "select visit/parent activity" Then
                    ds_Activity = Me.objhelpDb.GetResultSet("select distinct iNodeId,ActivityDisplayName,iParentNodeId,iNodeNo, " + _
                                                            " Convert(varchar,iNodeId) + '#' + vActivityId + '#' + ISNULL(Convert(varchar,nRefTime),'') As NodeAttributeID  " + _
                                                            " from VIEW_MedExWorkspaceHdr where vWorkspaceId='" + Me.HProjectId.Value + "' " + _
                                                            " And cStatusIndi<>'D' And iPeriod=1 And (iParentNodeID = " + Me.ddlVisit.SelectedItem.Value + " Or iNodeId = " + Me.ddlVisit.SelectedItem.Value + ")" + _
                                                            " order by iParentNodeId,iNodeNo ", "WorkspaceNodeDetail ")
                    ddlActivity.DataSource = ds_Activity.Tables(0)
                    ddlActivity.DataValueField = "NodeAttributeID"
                    ddlActivity.DataTextField = "ActivityDisplayName"
                    ddlActivity.DataBind()
                    ddlActivity.Items.Insert(0, "Select Activity")
                    For i As Integer = 1 To ddlActivity.Items.Count - 1
                        ddlActivity.Items(i).Attributes.Add("title", ddlActivity.Items(i).Text + "-(" + ddlActivity.Items(i).Value.Split("#")(1).ToString() + ")")
                    Next
                ElseIf iPeriod <> 0 Then
                    ds_Activity = Me.objhelpDb.GetResultSet("select distinct iNodeId,ActivityDisplayName,iParentNodeId,iNodeNo, " + _
                                                            " Convert(varchar,iNodeId) + '#' + vActivityId + '#' + ISNULL(Convert(varchar,nRefTime),'') As NodeAttributeID  " + _
                                                            " from VIEW_MedExWorkspaceHdr where vWorkspaceId='" + Me.HProjectId.Value + "' " + _
                                                            " And cStatusIndi<>'D' And iPeriod=" + iPeriod + " And (iParentNodeID = " + Me.ddlVisit.SelectedItem.Value + " Or iNodeId = " + Me.ddlVisit.SelectedItem.Value + ")" + _
                                                            " order by iParentNodeId,iNodeNo ", "WorkspaceNodeDetail ")
                    ddlActivity.DataSource = ds_Activity.Tables(0)
                    ddlActivity.DataValueField = "NodeAttributeID"
                    ddlActivity.DataTextField = "ActivityDisplayName"
                    ddlActivity.DataBind()
                    ddlActivity.Items.Insert(0, "Select Activity")
                    For i As Integer = 1 To ddlActivity.Items.Count - 1
                        ddlActivity.Items(i).Attributes.Add("title", ddlActivity.Items(i).Text + "-(" + ddlActivity.Items(i).Value.Split("#")(1).ToString() + ")")
                    Next
                Else
                    ddlActivity.DataSource = Nothing
                    ddlActivity.DataBind()
                    ddlActivity.Items.Clear()
                End If
            ElseIf Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                If Me.ddlPeriod.SelectedValue.ToString.Trim() <> "" And _
                    Me.ddlPeriod.SelectedValue.ToString.Trim().ToLower() <> "select period" Then

                    Dim ds_Period As DataSet
                    ds_Period = Me.objhelpDb.GetResultSet("select iNodeId,vNodeDisplayName from WorkspaceNodeDetail where vWorkspaceId='" + Me.HProjectId.Value + _
                                                          "' And iParentNodeId = 1  And iPeriod=" + ddlPeriod.SelectedValue + "  AND ISNULL(nMilestone,0) <> 2 And cStatusIndi<>'D' order by iNodeNo", "WorkspaceNodeDetail")
                    Me.ddlVisit.DataSource = ds_Period.Tables(0)
                    Me.ddlVisit.DataValueField = "iNodeId"
                    Me.ddlVisit.DataTextField = "vNodeDisplayName"
                    Me.ddlVisit.DataBind()
                    Me.ddlVisit.Items.Insert(0, "Select Visit/Parent Activity")

                    'ds_Activity = Me.objhelpDb.GetResultSet("select distinct iNodeId,ActivityDisplayName,iParentNodeId,iNodeNo, " + _
                    '                                        " Convert(varchar,iNodeId) + '#' + vActivityId + '#' + ISNULL(Convert(varchar,nRefTime),'') As NodeAttributeID  " + _
                    '                                        " from VIEW_MedExWorkspaceHdr where vWorkspaceId='" + Me.HProjectId.Value + "' " + _
                    '                                        " And cStatusIndi<>'D' And iPeriod=" + ddlPeriod.SelectedValue + " order by iParentNodeId,iNodeNo ", "WorkspaceNodeDetail ")
                    'ddlActivity.DataSource = ds_Activity.Tables(0)
                    'ddlActivity.DataValueField = "NodeAttributeID"
                    'ddlActivity.DataTextField = "ActivityDisplayName"
                    'ddlActivity.DataBind()
                    'ddlActivity.Items.Insert(0, "Select Activity")
                    'For i As Integer = 1 To ddlActivity.Items.Count - 1
                    '    ddlActivity.Items(i).Attributes.Add("title", ddlActivity.Items(i).Text + "-(" + ddlActivity.Items(i).Value.Split("#")(1).ToString() + ")")
                    'Next
                ElseIf iPeriod <> 0 Then

                    Dim ds_Period As DataSet
                    ds_Period = Me.objhelpDb.GetResultSet("select iNodeId,vNodeDisplayName from WorkspaceNodeDetail where vWorkspaceId='" + Me.HProjectId.Value + _
                                                          "' And iParentNodeId = 1  And iPeriod=" + iPeriod + "  AND ISNULL(nMilestone,0) <> 2 And cStatusIndi<>'D' order by iNodeNo", "WorkspaceNodeDetail")
                    Me.ddlVisit.DataSource = ds_Period.Tables(0)
                    Me.ddlVisit.DataValueField = "iNodeId"
                    Me.ddlVisit.DataTextField = "vNodeDisplayName"

                    Me.ddlVisit.DataBind()
                    Me.ddlVisit.Items.Insert(0, "Select Visit/Parent Activity")


                    'ds_Activity = Me.objhelpDb.GetResultSet("select distinct iNodeId,ActivityDisplayName,iParentNodeId,iNodeNo, " + _
                    '                                        " Convert(varchar,iNodeId) + '#' + vActivityId + '#' + ISNULL(Convert(varchar,nRefTime),'') As NodeAttributeID  " + _
                    '                                        " from VIEW_MedExWorkspaceHdr where vWorkspaceId='" + Me.HProjectId.Value + "' " + _
                    '                                        " And cStatusIndi<>'D' And iPeriod=" + iPeriod + " order by iParentNodeId,iNodeNo ", "WorkspaceNodeDetail ")
                    'ddlActivity.DataSource = ds_Activity.Tables(0)
                    'ddlActivity.DataValueField = "NodeAttributeID"
                    'ddlActivity.DataTextField = "ActivityDisplayName"
                    'ddlActivity.DataBind()
                    'ddlActivity.Items.Insert(0, "Select Activity")
                    'For i As Integer = 1 To ddlActivity.Items.Count - 1
                    '    ddlActivity.Items(i).Attributes.Add("title", ddlActivity.Items(i).Text + "-(" + ddlActivity.Items(i).Value.Split("#")(1).ToString() + ")")
                    'Next
                Else
                    ddlVisit.DataSource = Nothing
                    ddlVisit.DataBind()
                    ddlVisit.Items.Clear()


                    'ddlActivity.DataSource = Nothing
                    'ddlActivity.DataBind()
                    'ddlActivity.Items.Clear()
                End If

            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
            Return False
        End Try
    End Function

    Protected Function CheckProjectStatus() As Boolean

        Dim wstr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_CrfLockDetail As DataSet = Nothing

        wstr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'"

        If Not objhelpDb.GetCRFLockDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CrfLockDetail, eStr) Then
            Throw New Exception(eStr)
        End If
        If ds_CrfLockDetail.Tables(0).Rows.Count > 0 Then
            ds_CrfLockDetail.Tables(0).DefaultView.RowFilter = " iTranNo = Max(iTranNo)"
            If ds_CrfLockDetail.Tables(0).DefaultView(0)("cLockFlag") = "L" Then
                Me.btnInsertAttribute.Enabled = False
                Me.ddlOperator.Enabled = False
                Me.txtValue.Enabled = False
                Me.btnAND.Enabled = False
                Me.btnOR.Enabled = False
                Me.btnSave.Enabled = False
                Return True
            End If
        End If
        Return False
    End Function

    Private Function FillActivityF(Optional ByVal iPeriod As Integer = 0) As Boolean
        Dim ds_Activity As DataSet = Nothing
        Dim eStr As String = String.Empty

        Try

            If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                If Me.ddlVisit.SelectedValue.ToString.Trim() <> "" And _
                  Me.ddlVisit.SelectedValue.ToString.Trim().ToLower() <> "select visit/parent activity" Then

                    ds_Activity = Me.objhelpDb.GetResultSet("select distinct iNodeId,ActivityDisplayName,iParentNodeId,iNodeNo, " + _
                                                            " Convert(varchar,iNodeId) + '#' + vActivityId + '#' + ISNULL(Convert(varchar,nRefTime),'') As NodeAttributeID  " + _
                                                            " from VIEW_MedExWorkspaceHdr where vWorkspaceId='" + Me.HProjectId.Value + "' " + _
                                                            " And cStatusIndi<>'D' And iPeriod = " + ddlPeriod.SelectedValue + " And (iParentNodeID = " + Me.ddlVisit.SelectedItem.Value + " Or iNodeId = " + Me.ddlVisit.SelectedItem.Value + ")" + _
                                                            " order by iParentNodeId,iNodeNo ", "WorkspaceNodeDetail ")
                    ddlActivity.DataSource = ds_Activity.Tables(0)
                    ddlActivity.DataValueField = "NodeAttributeID"
                    ddlActivity.DataTextField = "ActivityDisplayName"
                    ddlActivity.DataBind()
                    ddlActivity.Items.Insert(0, "Select Activity")
                    For i As Integer = 1 To ddlActivity.Items.Count - 1
                        ddlActivity.Items(i).Attributes.Add("title", ddlActivity.Items(i).Text + "-(" + ddlActivity.Items(i).Value.Split("#")(1).ToString() + ")")
                    Next
                ElseIf iPeriod <> 0 Then
                    ds_Activity = Me.objhelpDb.GetResultSet("select distinct iNodeId,ActivityDisplayName,iParentNodeId,iNodeNo, " + _
                                                            " Convert(varchar,iNodeId) + '#' + vActivityId + '#' + ISNULL(Convert(varchar,nRefTime),'') As NodeAttributeID  " + _
                                                            " from VIEW_MedExWorkspaceHdr where vWorkspaceId='" + Me.HProjectId.Value + "' " + _
                                                            " And cStatusIndi<>'D' And iPeriod=" + ddlPeriod.SelectedValue + " And (iParentNodeID = " + Me.ddlVisit.SelectedItem.Value + " Or iNodeId = " + Me.ddlVisit.SelectedItem.Value + ")" + _
                                                            " order by iParentNodeId,iNodeNo ", "WorkspaceNodeDetail ")
                    ddlActivity.DataSource = ds_Activity.Tables(0)
                    ddlActivity.DataValueField = "NodeAttributeID"
                    ddlActivity.DataTextField = "ActivityDisplayName"
                    ddlActivity.DataBind()
                    ddlActivity.Items.Insert(0, "Select Activity")
                    For i As Integer = 1 To ddlActivity.Items.Count - 1
                        ddlActivity.Items(i).Attributes.Add("title", ddlActivity.Items(i).Text + "-(" + ddlActivity.Items(i).Value.Split("#")(1).ToString() + ")")
                    Next
                Else
                    ddlActivity.DataSource = Nothing
                    ddlActivity.DataBind()
                    ddlActivity.Items.Clear()
                End If
            Else
                If Me.ddlVisit.SelectedValue.ToString.Trim() <> "" And _
                   Me.ddlVisit.SelectedValue.ToString.Trim().ToLower() <> "select visit/parent activity" Then
                    ds_Activity = Me.objhelpDb.GetResultSet("select distinct iNodeId,ActivityDisplayName,iParentNodeId,iNodeNo, " + _
                                                            " Convert(varchar,iNodeId) + '#' + vActivityId + '#' + ISNULL(Convert(varchar,nRefTime),'') As NodeAttributeID  " + _
                                                            " from VIEW_MedExWorkspaceHdr where vWorkspaceId='" + Me.HProjectId.Value + "' " + _
                                                            " And cStatusIndi<>'D' And iPeriod=1 And (iParentNodeID = " + Me.ddlVisit.SelectedItem.Value + " Or iNodeId = " + Me.ddlVisit.SelectedItem.Value + ")" + _
                                                            " order by iParentNodeId,iNodeNo ", "WorkspaceNodeDetail ")
                    ddlActivity.DataSource = ds_Activity.Tables(0)
                    ddlActivity.DataValueField = "NodeAttributeID"
                    ddlActivity.DataTextField = "ActivityDisplayName"
                    ddlActivity.DataBind()
                    ddlActivity.Items.Insert(0, "Select Activity")
                    For i As Integer = 1 To ddlActivity.Items.Count - 1
                        ddlActivity.Items(i).Attributes.Add("title", ddlActivity.Items(i).Text + "-(" + ddlActivity.Items(i).Value.Split("#")(1).ToString() + ")")
                    Next
                ElseIf iPeriod <> 0 Then
                    ds_Activity = Me.objhelpDb.GetResultSet("select distinct iNodeId,ActivityDisplayName,iParentNodeId,iNodeNo, " + _
                                                            " Convert(varchar(10),iNodeId) + '#' + vActivityId + '#' + ISNULL(Convert(varchar(10),nRefTime),'') As NodeAttributeID  " + _
                                                            " from VIEW_MedExWorkspaceHdr where vWorkspaceId='" + Me.HProjectId.Value + "' " + _
                                                            " And cStatusIndi<>'D' And iPeriod=" + iPeriod + " And (iParentNodeID = " + Me.ddlVisit.SelectedItem.Value + " Or iNodeId = " + Me.ddlVisit.SelectedItem.Value + ")" + _
                                                            " order by iParentNodeId,iNodeNo ", "WorkspaceNodeDetail ")
                    ddlActivity.DataSource = ds_Activity.Tables(0)
                    ddlActivity.DataValueField = "NodeAttributeID"
                    ddlActivity.DataTextField = "ActivityDisplayName"
                    ddlActivity.DataBind()
                    ddlActivity.Items.Insert(0, "Select Activity")
                    For i As Integer = 1 To ddlActivity.Items.Count - 1
                        ddlActivity.Items(i).Attributes.Add("title", ddlActivity.Items(i).Text + "-(" + ddlActivity.Items(i).Value.Split("#")(1).ToString() + ")")
                    Next
                Else
                    ddlActivity.DataSource = Nothing
                    ddlActivity.DataBind()
                    ddlActivity.Items.Clear()
                End If

            End If

            Return True
        Catch ex As Exception

            Return False
        End Try

    End Function

#End Region

#Region "Fill Attribute"

    Private Function FillAttribute() As Boolean
        Dim dsAttribute As DataSet = Nothing
        Dim estr As String = String.Empty

        Try

            If Me.ddlPeriod.SelectedValue.ToString.Trim() <> "" And _
               Me.ddlPeriod.SelectedValue.ToString.Trim().ToLower() <> "select period" And _
               Me.ddlActivity.SelectedValue.ToString.Trim() <> "" And _
               Me.ddlActivity.SelectedValue.ToString.Trim().ToLower() <> "select activity" Then

                Me.tdselectedActivity.InnerText = Me.ddlActivity.SelectedItem.Text.ToString

                dsAttribute = Me.objhelpDb.GetResultSet("select vMedExCode,vMedExDesc,vMedExType,vMedExDefaultValue," + _
                                                        " ISNULL(vMedExCode,'') + '#' + ISNULL(vMedExDesc,'') + '#' + ISNULL(vMedExType,'') + '#' + ISNULL(vMedExDefaultValue,'') " + _
                                                        " + '#' + ISNULL(vmedexsubGroupDesc,'') + '#' + ISNULL(vmedexgroupDesc,'') + '#' + ISNULL(vMedExValues,'')  + '#' + ISNULL(vValidationType,'')  As vAllColumns " + _
                                                        " from VIEW_MedExWorkspaceHdr where vWorkspaceId='" + Me.HProjectId.Value + "' And cActiveFlag <> 'N'  And cStatusIndi<>'D' " + _
                                                        " And iPeriod=" + ddlPeriod.SelectedValue + " And vMedExType <> 'Import' And " + _
                                                        " iNodeId=" + ddlActivity.SelectedValue.Split("#")(0).ToString + " order by iNodeNo,iSeqNo ", "tblAttribute")

                If Not dsAttribute Is Nothing Then
                    Me.lstAttribute.DataSource = Nothing
                    Me.lstAttribute.DataSource = dsAttribute.Tables(0)
                    Me.lstAttribute.DataValueField = "vAllColumns"
                    Me.lstAttribute.DataTextField = "vMedExDesc"
                    Me.lstAttribute.DataBind()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "str", "fnListBoxSearch();", True)
                End If
            End If

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
            Return False
        End Try
    End Function

#End Region

#Region "Helper Functions"

    Private Function AssignValues(ByVal type As String) As Boolean

        Dim dsCREditChecks As New DataSet
        Dim dtEditChecks As New DataTable
        Dim dvEditChecks As DataView
        Dim dr As DataRow
        Dim strNode As String = String.Empty
        Dim wStr As String = String.Empty
        Dim strRefTime As String = String.Empty
        Dim strActivity As String = String.Empty
        Dim strRefTimeNull As String = String.Empty
        Dim strNodeIds As String = String.Empty
        Dim strArrayNodes() As String
        Dim strArrayQuery() As String

        Try

            dtEditChecks = CType(Me.ViewState(VS_DtEditChecks), DataTable)

            If type.ToUpper = "ADD" Then

                dtEditChecks.Clear()

                If Me.rdbEditChecksType.Items(0).Selected Then
                    If Me.hdnSaveNode.Value = "" Then
                        strNode = Me.hdnNodeID.Value.Trim() + "#" + Me.ddlPeriod.SelectedValue.ToString() + ","
                    Else
                        strNode = Me.hdnSaveNode.Value
                    End If

                    For Each Str As String In strNode.Split(",")
                        If Str <> "" Then
                            dr = dtEditChecks.NewRow()
                            dr("nEditCheckNo") = 0
                            dr("vWorkspaceId") = Me.HProjectId.Value.Trim()
                            dr("iNodeId") = Str.Split("#")(0).ToString()
                            If Str.Split("#").Length = 2 Then
                                dr("iPeriod") = Str.Split("#")(1).ToString()
                                dr("vActivityId") = Me.ddlActivity.SelectedValue.Split("#")(1).ToString()
                            Else
                                dr("iPeriod") = Str.Split("#")(3).ToString()
                                dr("vActivityId") = Str.Split("#")(1).ToString()
                            End If
                            'dr("iPeriod") = IIf(Str.Split("#").Length = 2, Str.Split("#")(1).ToString(), Str.Split("#")(3).ToString())
                            ''dr("vActivityId") = IIf(Str.Split("#").Length = 2, Me.ddlActivity.SelectedValue.Split("#")(1).ToString(), Str.Split("#")(1).ToString())
                            'dr("iNodeId") = Me.hdnNodeID.Value.Trim()
                            'dr("iPeriod") = Me.ddlPeriod.SelectedValue.ToString()
                            'dr("vActivityId") = Me.ddlActivity.SelectedValue.Split("#")(1).ToString()
                            dr("vQueryMessage") = Me.txtFormula.Text.Trim()
                            'dr("vQueryValue") = "If  " + Me.hdnQueryValue.Value.Replace("(" + Me.hdnCrossNodes.Value.Split("#")(0) + ")", "(" + Str.Split("#")(0) + ")").Trim() + " BEGIN SET @ReturnVal = 'Y' END ELSE BEGIN SET @ReturnVal = 'N' END"
                            dr("vQueryValue") = "If  " + Me.hdnQueryValue.Value.Replace("(" + Me.hdnCrossNodes.Value.Split("#")(0) + ")", "(" + Str.Split("#")(0) + ")").Trim() + " BEGIN SET @ReturnVal = 'Y' END ELSE BEGIN SET @ReturnVal = 'N' END"
                            dr("vErrorMessage") = Me.txtMessage.Text.Trim()
                            dr("cEditCheckType") = IIf(Me.rdbEditChecksType.Items(0).Selected, "P", "C")
                            dr("iModifyBy") = Me.Session(S_UserID)
                            dr("dModifyOn") = System.DateTime.Now
                            dr("cStatusIndi") = "N"
                            dr("vMedExCode") = Convert.ToString(hdnMedExCode.Value) ''Added By Vivek Patel
                            dtEditChecks.Rows.Add(dr)
                        End If
                    Next

                ElseIf Me.rdbEditChecksType.Items(1).Selected Then

                    For Each Str As String In Me.hdnCrossNodes.Value.Split(",")
                        If Str <> "" Then
                            strActivity += "'" + Str.Split("#")(1) + "',"
                            If Str.Split("#")(2) <> "" Then
                                strRefTime += Str.Split("#")(2) + ","
                            Else
                                strRefTimeNull = " or nRefTime is null"
                            End If
                            strNodeIds += Str.Split("#")(0) + ","
                            Exit For
                        End If
                    Next


                    strActivity = strActivity.Substring(0, strActivity.LastIndexOf(","))
                    If strRefTime <> "" Then
                        strRefTime = strRefTime.Substring(0, strRefTime.LastIndexOf(","))
                    End If
                    If strNodeIds <> "" Then
                        strNodeIds = strNodeIds.Substring(0, strNodeIds.LastIndexOf(","))
                    End If

                    wStr = "select vWorkSpaceId,iNodeId,iPeriod,nRefTime,vActivityId from WorkSpaceNodeDetail where vWorkSpaceId = '" + Me.HProjectId.Value + "'" + _
                           " And vActivityId in(" + strActivity + ")" + _
                           " And (nRefTime in (" + IIf(strRefTime <> "", strRefTime, "NULL") + ")" + IIf(strRefTimeNull <> "", strRefTimeNull, "") + ")" + _
                           " And cStatusIndi <> 'D'"

                    If Not Me.chkAllActivity.Checked Then

                        wStr += " And iPeriod = " + Me.ddlPeriod.SelectedValue.ToString()

                    End If

                    If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                        wStr += " and inodeid in (" + strNodeIds + ")"
                    End If

                    dsCREditChecks = objhelpDb.GetResultSet(wStr, "CREditChecks")

                    dvEditChecks = dsCREditChecks.Tables(0).DefaultView
                    dvEditChecks.RowFilter = "vActivityId = " + Me.hdnNodeID.Value.Split("#")(1) + IIf(Me.hdnNodeID.Value.Split("#")(2) <> "", " And nRefTime = " + Me.hdnNodeID.Value.Split("#")(2), "")

                    ReDim Preserve strArrayQuery(dvEditChecks.ToTable().Rows.Count - 1)
                    Dim rowIndex As Integer = 0
                    For Each drCR As DataRow In dvEditChecks.ToTable().Rows
                        strArrayQuery(rowIndex) = Me.hdnQueryValue.Value.Trim()
                        rowIndex = rowIndex + 1
                    Next

                    ReDim Preserve strArrayNodes(Me.hdnCrossNodes.Value.Split(",").Length)
                    strArrayNodes = Me.hdnCrossNodes.Value.Split(",")

                    For Each Str As String In strArrayNodes
                        If Str <> "" Then

                            dvEditChecks = dsCREditChecks.Tables(0).DefaultView

                            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                                dvEditChecks.RowFilter = "vActivityId = " + Str.Split("#")(1) + " And iNodeId = " + Str.Split("#")(0) + " And iPeriod = " + Str.Split("#")(3) + IIf(Str.Split("#")(2) <> "", " And nRefTime = " + Str.Split("#")(2), "")
                                'dvEditChecks.RowFilter = "vActivityId = " + Str.Split("#")(1) + " And iPeriod = " + Str.Split("#")(3) + IIf(Str.Split("#")(2) <> "", " And nRefTime = " + Str.Split("#")(2), "")
                            Else
                                dvEditChecks.RowFilter = "vActivityId = " + Str.Split("#")(1) + " And iPeriod = " + Str.Split("#")(3) + IIf(Str.Split("#")(2) <> "", " And nRefTime = " + Str.Split("#")(2), "")
                            End If


                            For rowIndex = 0 To dvEditChecks.ToTable().Rows.Count - 1
                                strArrayQuery(rowIndex) = strArrayQuery(rowIndex).Replace("(" + Str.Split("#")(0) + ")", "(" + dvEditChecks.ToTable().Rows(rowIndex)("iNodeId").ToString() + ")")
                            Next

                        End If
                    Next

                    dvEditChecks = dsCREditChecks.Tables(0).DefaultView
                    dvEditChecks.RowFilter = "vActivityId = " + Me.hdnNodeID.Value.Split("#")(1) + IIf(Me.hdnNodeID.Value.Split("#")(2) <> "", " And nRefTime = " + Me.hdnNodeID.Value.Split("#")(2), "")

                    rowIndex = 0
                    For Each drCR As DataRow In dvEditChecks.ToTable().Rows

                        dr = dtEditChecks.NewRow()
                        dr("nEditCheckNo") = 0
                        dr("vWorkspaceId") = Me.HProjectId.Value.Trim()
                        dr("iPeriod") = drCR("iPeriod").ToString
                        dr("iNodeId") = drCR("iNodeId").ToString
                        dr("vActivityId") = Me.hdnNodeID.Value.Split("#")(1).ToString
                        dr("vQueryMessage") = Me.txtFormula.Text.Trim()
                        dr("vQueryValue") = "If  " + strArrayQuery(rowIndex).ToString.Trim() + " BEGIN SET @ReturnVal = 'Y' END ELSE BEGIN SET @ReturnVal = 'N' END"
                        dr("vErrorMessage") = Me.txtMessage.Text.Trim()
                        dr("cEditCheckType") = IIf(Me.rdbEditChecksType.Items(0).Selected, "P", "C")
                        dr("iModifyBy") = Me.Session(S_UserID)
                        dr("dModifyOn") = System.DateTime.Now
                        dr("cStatusIndi") = "N"
                        dr("vMedExCode") = Convert.ToString(hdnMedExCode.Value) ''Added By Vivek Patel
                        dtEditChecks.Rows.Add(dr)
                        rowIndex = rowIndex + 1
                    Next

                End If
            ElseIf type.ToUpper = "UPDATE" Then
                dtEditChecks = CType(Me.ViewState(VS_DtEditCheckUpdate), DataTable)
                For Each drUpdate As DataRow In dtEditChecks.Rows
                    drUpdate("cStatusIndi") = "E"
                    drUpdate("iModifyBy") = Me.Session(S_UserID)
                    drUpdate("dModifyOn") = System.DateTime.Now
                    drUpdate("vErrorMessage") = Me.txtMessage.Text.Trim()
                    If drUpdate("cEditCheckType").ToString.ToLower() = "corss activity" Then
                        drUpdate("cEditCheckType") = "C"
                    Else
                        drUpdate("cEditCheckType") = "P"
                    End If
                Next
            End If

            Me.ViewState(VS_DtEditChecks) = dtEditChecks.Copy()
            Return True

        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return False
        End Try

    End Function

    Private Sub ResetPage(Optional ByVal flag As Boolean = True, _
                          Optional ByVal str As String = "")
        Me.hdnSaveNode.Value = ""
        Me.hdnMedExValues.Value = ""
        Me.hdnQueryValue.Value = ""
        Me.hdnCrossNodes.Value = ""
        Me.hdnNodeID.Value = ""
        Me.txtFormula.Text = ""
        Me.txtValue.Text = ""
        Me.txtMessage.Text = ""
        Me.chkAllActivity.Checked = False

        Me.txtproject.Enabled = True
        Me.ddlPeriod.Enabled = True
        Me.ddlActivity.Enabled = True
        Me.ddlVisit.Enabled = True
        Me.txtMessage.Enabled = True
        Me.btnSave.Visible = True
        Me.btnCancel.Visible = True
        'Me.btnValidate.Visible = True
        Me.chkAllActivity.Enabled = True

        If flag = True Then
            Me.txtproject.Text = ""
            Me.HProjectId.Value = ""
            Me.ddlPeriod.Items.Clear()
            Me.ddlActivity.Items.Clear()
            Me.ddlVisit.Items.Clear()
            If Me.ddlPeriod.Items.Count > 0 Then
                Me.ddlPeriod.SelectedIndex = 0
            End If

            Me.lstAttribute.Items.Clear()

        End If

        'If str = "" Then
        ' If Me.ddlActivity.Items.Count > 0 Then
        'Me.ddlActivity.SelectedIndex = 0
        'End If
        ' End If
        Me.rdbEditChecksType.Items(0).Selected = True
        Me.rdbEditChecksType.Items(1).Selected = False

        'Me.pnlMedExAttributeValues.Visible = False
        'Me.txtValue.Visible = True
        Me.pnlMedExAttributeValues.Style.Add("display", "none")
        Me.txtValue.Style.Add("display", "inline")

        Me.btnInsertAttribute.Enabled = False
        Me.ddlOperator.Enabled = False
        Me.txtValue.Enabled = False
        Me.btnAND.Enabled = False
        Me.btnOR.Enabled = False
        Me.btnExportToExcel.Style.Add("display", "none")
        Me.btnUpdate.Visible = False
        Me.chkAllActivity.Visible = True
        Me.fldsData.Visible = True

        Me.chkAllActivity.Text = "Attach To All Same Activities ?"

    End Sub

#End Region

#Region "Export To Excell"

    Private Function ExportToExcel(ByVal Dt As DataTable, ByVal dtString As String) As Boolean
        Dim fileName As String = ""
        Dim eStr As String = String.Empty
        Try
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            fileName = "Edit Checks"
            fileName = fileName & ".xls"
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            Context.Response.Write(ConvertDtTO(Dt, dtString))

            Context.Response.Flush()
            Context.Response.End()

            HttpContext.Current.ApplicationInstance.CompleteRequest()

            System.IO.File.Delete(fileName)
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Return False
            Exit Function
        End Try

    End Function

    Private Function ConvertDtTO(ByVal dt As DataTable, ByVal dtString As String) As String
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer
        Dim SrNo As Integer = 1
        Dim dtConvert As New DataTable
        Try

            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""6""><center><strong><font color=""black"" size=""9px"" face=""Calibri"">")
            strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""6""><center><strong><font color=""black"" size=""8px"" face=""Calibri"">")
            strMessage.Append("Edit Check Details")
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")
            strMessage.Append("<td><strong><font color=""black"" size=""9px"" face=""Calibri"">")
            strMessage.Append("Project/Site")
            strMessage.Append("</font></strong></td>")
            strMessage.Append("<td colspan=""5""><strong><font color=""black"" size=""9px"" face=""Calibri"">")
            strMessage.Append(Me.txtproject.Text.Trim().Substring(1, Me.txtproject.Text.Trim().IndexOf("]") - 1))
            strMessage.Append("</font></strong></td>")
            strMessage.Append("</tr>")


            strMessage.Append("<tr>")

            dt.Columns.Add("vSrNo")
            For Each dr In dt.Rows
                dr("vSrNo") = SrNo
                SrNo += 1
            Next

            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                dtConvert = dt.DefaultView.ToTable(True, "vSrNo,vParentNodeDisplayName,ActivityDisplayName,vQueryMessage,vErrorMessage,cEditCheckType".Split(","))
                dtConvert.AcceptChanges()
                dtConvert.Columns(0).ColumnName = "#"
                dtConvert.Columns(1).ColumnName = "Visit/Parent Activity"
            Else
                dtConvert = dt.DefaultView.ToTable(True, "vSrNo,iPeriod,ActivityDisplayName,vQueryMessage,vErrorMessage,cEditCheckType".Split(","))
                dtConvert.AcceptChanges()
                dtConvert.Columns(0).ColumnName = "#"
                dtConvert.Columns(1).ColumnName = "Period"
            End If

            dtConvert.Columns(2).ColumnName = "Activity"
            dtConvert.Columns(3).ColumnName = "Edit Check Formula"
            dtConvert.Columns(4).ColumnName = "Discrepancy Message"
            dtConvert.Columns(5).ColumnName = "Type"
            dtConvert.AcceptChanges()

            For iCol = 0 To dtConvert.Columns.Count - 1

                strMessage.Append("<td style=""text-align:center;""><strong><font color=""Black"" size=""9px"" face=""Calibri"">")
                strMessage.Append(Convert.ToString(dtConvert.Columns(iCol)).Trim())
                strMessage.Append("</font></strong></td>")

            Next
            strMessage.Append("</tr>")

            For j = 0 To dtConvert.Rows.Count - 1
                strMessage.Append("<tr>")
                For i = 0 To dtConvert.Columns.Count - 1
                    If i = 0 Then
                        strMessage.Append("<td style=""text-align:center;vertical-align:top;""><font color=""black"" size=""2"" face=""Calibri"">")
                    Else
                        strMessage.Append("<td style=""text-align:left;vertical-align:top;""><font color=""black"" size=""2"" face=""Calibri"">")
                    End If
                    strMessage.Append(Convert.ToString(dtConvert.Rows(j).Item(i)).Trim())
                    strMessage.Append("</font></td>")

                Next
                strMessage.Append("</tr>")
            Next
            strMessage.Append("</table>")

            Return strMessage.ToString

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
            Return ex.Message
        End Try
    End Function

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


End Class
