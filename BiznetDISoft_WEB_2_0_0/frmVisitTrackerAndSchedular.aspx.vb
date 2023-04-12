Imports Newtonsoft.Json

Partial Class frmVisitTrackerAndSchedular
    Inherits System.Web.UI.Page


#Region "Variable Declaration"

    Private objCommon As New clsCommon
    Private objhelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

    Private eStr_Retu As String = ""
    Public IsProjectLock As Boolean = False

    Private Const GVC_TrackerNo As Integer = 0
    Private Const GVC_Activity As Integer = 4
    Private Const GVC_ActualDay As Integer = 5
    Private Const GVC_Negative As Integer = 6
    Private Const GVC_Positive As Integer = 7
    Private Const GVC_Update As Integer = 8


#End Region

#Region "Page_Load"

    Protected Sub Page_Load() Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....Page_Load")
        End Try
    End Sub

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Try
            If Not GenCall_Data() Then
                GenCall = False
            End If
            If Not GenCall_ShowUI() Then
                GenCall = False
            End If
            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall")
            GenCall = False
        End Try
    End Function

#End Region

#Region "GenCall Data"

    Private Function GenCall_Data() As Boolean
        Dim Val As String = String.Empty
        Dim wStr As String = String.Empty
        Dim DS_visitTrackerBlank As DataSet = Nothing

        Try
            DS_visitTrackerBlank = Me.objhelp.GetResultSet("select *  from VisitTracker where 1=2 ", "VisitTracker")
            ViewState("DS_visitTrackerBlank") = DS_visitTrackerBlank
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_Data")
            GenCall_Data = False
        End Try
    End Function

#End Region

#Region "GenCall_ShowUI"

    Private Function GenCall_ShowUI() As Boolean
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim sender As New Object

        Try
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Visit Tracker"
            Page.Title = " :: Visit Deviation Management ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtProject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
            End If

            Me.AutoCompleteExtenderadd.ContextKey = "iUserId = " & Me.Session(S_UserID) + " AND vProjectTypeCode = '0014'"
            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...............GenCall_ShowUI")
            GenCall_ShowUI = False
        End Try
    End Function

#End Region

#Region "Button Event"

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs)
        Dim DS_SaveVisitTracker As DataSet = Nothing
        Dim DS_BlankVisitTracker As DataSet = Nothing

        DS_BlankVisitTracker = CType(ViewState("DS_visitTrackerBlank"), DataSet)
        DS_SaveVisitTracker = DS_BlankVisitTracker.Clone.Copy

        If Not Valid() Then
            Exit Sub
        End If

        Dim dr As DataRow
        Try
            dr = DS_SaveVisitTracker.Tables(0).NewRow()
            dr("nTrackerNo") = 1
            dr("vWorkSpaceId") = Me.HProjectId.Value
            dr("iVisitNo") = HfVisitNo.Value
            dr("iNodeId") = ddlParentActivity.SelectedValue.ToString()
            dr("cDateType") = IIf(gvDeviation.Rows.Count > 0, "", rbtdateselecton.SelectedValue)
            dr("iActualDays") = txtActual.Text
            dr("iDeviationNegative") = IIf(Me.txtNegative.Text.ToString() = "", 0, Me.txtNegative.Text.ToString())
            dr("iDeviationPositive") = IIf(Me.txtPositive.Text.ToString() = "", 0, Me.txtPositive.Text.ToString())
            dr("cStatusIndi") = "N"
            dr("iModifyby") = Session(S_UserID)
            dr("vRemarks") = ""

            DS_SaveVisitTracker.Tables(0).Rows.Add(dr)
            DS_SaveVisitTracker.AcceptChanges()
            DS_SaveVisitTracker.Tables(0).TableName = "VisitTracker"

            If Not objLambda.Save_VisitTracker(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, DS_SaveVisitTracker, eStr_Retu) Then
                objCommon.ShowAlert("Error While Saving Data in VisitTracker", Me.Page)
                Exit Sub
            End If

            If Not FillGrid() Then
                ShowErrorMessage("Error While FillGrid", "")
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....")
        Finally
            txtActual.Text = ""
            txtNegative.Text = ""
            txtPositive.Text = ""
            DS_SaveVisitTracker = Nothing
        End Try
    End Sub

    Protected Sub btnSaveRemarks_Click(sender As Object, e As EventArgs) Handles btnSaveRemarks.Click
        Try
            Dim dtvisitEdit As New DataTable
            Dim DS_SaveVisitTrackerdtl As New DataSet

            dtvisitEdit = CType(ViewState("Edit_Table"), DataTable)

            For Each dr In dtvisitEdit.Rows
                dr("vRemarks") = Me.txtRemarks.Text.Trim()
            Next

            Try
                dtvisitEdit.Columns.Remove("vProjectName")
            Catch ex As Exception
            End Try

            dtvisitEdit.AcceptChanges()
            dtvisitEdit.TableName = "VisitTracker"

            DS_SaveVisitTrackerdtl.Tables.Add(dtvisitEdit)
            DS_SaveVisitTrackerdtl.AcceptChanges()

            If Not objLambda.Save_VisitTracker(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, DS_SaveVisitTrackerdtl, eStr_Retu) Then
                objCommon.ShowAlert("Error While Saving Data in VisitTracker", Me.Page)
                Exit Sub
            Else
                objCommon.ShowAlert("Records Updated Successfully!", Me.Page)
                If Not FillGrid() Then
                    ShowErrorMessage("Error While FillGrid", "")
                End If
            End If


        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".")
        Finally
            txtRemarks.Text = ""
            ViewState("Edit_Table") = Nothing
        End Try

    End Sub

    Protected Sub btnSetProject_Click(sender As Object, e As EventArgs) Handles btnSetProject.Click

        If Not FillGrid() Then
            ShowErrorMessage("Error While FillGrid", "")
        End If

        If Not FillParentActivity() Then
            Throw New Exception(eStr_Retu)
        End If

    End Sub

    Protected Sub btnClear_Click(sender As Object, e As EventArgs)
        Me.gvDeviation.DataSource = Nothing
        Me.gvDeviation.DataBind()
        upGvDeviation.Update()
        lblNote.Text = ""
        rbtdateselecton.SelectedIndex = -1
        rbtdateselecton.Style.Add("display", "")
        ViewState("Edit_Table") = Nothing
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

#Region "Fill Function"
    Function FillParentActivity() As Boolean
        Dim ds_Period As DataSet = Nothing
        Dim eStr As String = String.Empty
        Dim ds_Activity As DataSet = Nothing
        Try

            'If CType(Me.Session(S_ScopeNo), Integer) = Scope_ClinicalTrial Then

            Me.ddlParentActivity.Items.Insert(0, "1")
            ds_Period = Me.objhelp.GetResultSet("select iNodeId,vNodeDisplayName from WorkspaceNodeDetail where vWorkspaceId='" + Me.HProjectId.Value + _
                                                  "' And iParentNodeId = 1 And cStatusIndi<>'D' AND ( vTemplateId <> '0001' OR  vTemplateId IS NULl) Order by iNodeNo  ", "WorkspaceNodeDetail")
            Me.ddlParentActivity.DataSource = ds_Period.Tables(0)
            Me.ddlParentActivity.DataValueField = "iNodeId"
            Me.ddlParentActivity.DataTextField = "vNodeDisplayName"
            Me.ddlParentActivity.DataBind()
            Me.ddlParentActivity.Items.Insert(0, "Select Visit/Parent activity")

            'ElseIf Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
            '    Me.ddlParentActivity.Items.Insert(0, "1")
            '    ds_Period = Me.objhelp.GetResultSet("select iNodeId,vNodeDisplayName from WorkspaceNodeDetail where vWorkspaceId='" + Me.HProjectId.Value + _
            '                                          "' And iParentNodeId = 1 And cStatusIndi<>'D'   AND ( vTemplateId <> '0001' OR  vTemplateId IS NULl) Order by iNodeNo  ", "WorkspaceNodeDetail")
            '    Me.ddlParentActivity.DataSource = ds_Period.Tables(0)
            '    Me.ddlParentActivity.DataValueField = "iNodeId"
            '    Me.ddlParentActivity.DataTextField = "vNodeDisplayName"
            '    Me.ddlParentActivity.DataBind()
            '    Me.ddlParentActivity.Items.Insert(0, "Select Visit/Parent Activity")
            'End If



            upPeriod.Update()
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
            Return False
        End Try



    End Function

    Function FillGrid() As Boolean

        Dim ds_getvisittracker As DataSet
        Dim parameter As String
        Dim Note As String = String.Empty

        parameter = HProjectId.Value + "##" + Session(S_UserID).ToString() + "##"
        Try
            ds_getvisittracker = objhelp.ProcedureExecute("dbo.GetVisitTracker", parameter.ToString)

            ViewState("DS_DeviationReport") = ds_getvisittracker
            gvDeviation.DataSource() = ds_getvisittracker.Tables(0)
            gvDeviation.DataBind()

            'upPeriod.Update()
            upGvDeviation.Update()

            If Not ds_getvisittracker Is Nothing Then
                If ds_getvisittracker.Tables(0).Rows.Count > 0 Then
                    ds_getvisittracker.Tables(0).DefaultView.RowFilter = "iVisitNo = Max(iVisitNo)"
                    HfVisitNo.Value = Convert.ToInt32(ds_getvisittracker.Tables(0).DefaultView(0)("iVisitNo").ToString) + 1
                    Note = ds_getvisittracker.Tables(0).Rows(0).Item("cDateType").ToString()
                    rbtdateselecton.Style.Add("display", "none")
                    Me.lblNote.Text = "Note *: Day 0 Is Calculated From Visit " + IIf(Note.ToString = "S", "1", IIf(Note.ToString = "R", "2", "1"))
                Else
                    HfVisitNo.Value = 1
                    rbtdateselecton.Style.Add("display", "")
                    lblNote.Text = ""
                End If
            End If

            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    Function Valid() As Boolean
        Dim Ds_Grid As DataSet
        Dim Dvgrid As New DataView
        Dim Dtgridvalid As DataTable = Nothing


        Try

            Ds_Grid = CType(ViewState("DS_DeviationReport"), DataSet)
            Dvgrid = New DataView(Ds_Grid.Tables(0))
            Dvgrid.RowFilter = "InodeId = '" + ddlParentActivity.SelectedValue.ToString + "'"

            Dtgridvalid = Dvgrid.ToTable()

            If Dtgridvalid.Rows.Count > 0 Then
                objCommon.ShowAlert("Activity Already Saved!", Me.Page)
                Return False
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
#End Region

#Region "CheckBox Event"
    Protected Sub chkEdit_CheckedChanged(sender As Object, e As EventArgs)
        Dim ds_Period As DataSet = Nothing
        Dim eStr As String = String.Empty
        Dim ds_Activity As DataSet = Nothing
        Dim ds_RefActivity As DataSet = Nothing


        Dim gvRow As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
        Dim index As Integer = gvRow.RowIndex
        Dim chk As CheckBox = CType(sender, CheckBox)

        If (chk.Checked = True) Then
            gvRow.Cells(GVC_Activity).Enabled = True
            gvRow.Cells(GVC_ActualDay).Enabled = True
            gvRow.Cells(GVC_Negative).Enabled = True
            gvRow.Cells(GVC_Positive).Enabled = True
            gvRow.Cells(GVC_Update).Enabled = True

            Dim ddlgvChildActivity As DropDownList = DirectCast(gvRow.FindControl("ddlgvChildActivity"), DropDownList)

            Me.HfInodeid.Value = ddlgvChildActivity.SelectedValue

            ds_Period = Me.objhelp.GetResultSet("select iNodeId,vNodeDisplayName from WorkspaceNodeDetail where vWorkspaceId='" + Me.HProjectId.Value + _
                                                          "' And iParentNodeId = 1 And cStatusIndi<>'D' AND ( vTemplateId <> '0001' OR  vTemplateId IS NULl) Order by iNodeNo", "WorkspaceNodeDetail")

            ddlgvChildActivity.DataSource = ds_Period.Tables(0)
            ddlgvChildActivity.DataValueField = "iNodeId"
            ddlgvChildActivity.DataTextField = "vNodeDisplayName"
            ddlgvChildActivity.DataBind()

            ddlgvChildActivity.SelectedValue = Me.HfInodeid.Value
        Else
            gvRow.Cells(GVC_Activity).Enabled = False
            gvRow.Cells(GVC_ActualDay).Enabled = False
            gvRow.Cells(GVC_Negative).Enabled = False
            gvRow.Cells(GVC_Positive).Enabled = False
            gvRow.Cells(GVC_Update).Enabled = False

        End If
    End Sub
#End Region

#Region "Gridview Event"
    Protected Sub gvDeviation_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDeviation.RowDataBound

        Dim ds_Period As DataSet = Nothing
        Dim eStr As String = String.Empty
        Dim ds_RefActivity As DataSet = Nothing
        Dim ds As DataSet
        ds = CType(ViewState("DS_DeviationReport"), DataSet)
        Try
            e.Row.Cells(GVC_Activity).Enabled = False
            e.Row.Cells(GVC_ActualDay).Enabled = False
            e.Row.Cells(GVC_Negative).Enabled = False
            e.Row.Cells(GVC_Positive).Enabled = False
            e.Row.Cells(GVC_Update).Enabled = False
            CType(e.Row.FindControl("lnk"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnk"), LinkButton).CommandName = "MyValue"

            Dim InodeId As String = ds.Tables(0).Rows(e.Row.RowIndex)("iNodeid")

            Dim ddlgvChildActivity As DropDownList = DirectCast(e.Row.FindControl("ddlgvChildActivity"), DropDownList)

            ds_Period = Me.objhelp.GetResultSet("select iNodeId,vNodeDisplayName from WorkspaceNodeDetail where vWorkspaceId='" + Me.HProjectId.Value + _
                                               "'And iNodeid='" + InodeId.ToString() + "' And iParentNodeId = 1 AND ( vTemplateId <> '0001' OR  vTemplateId IS NULl) And cStatusIndi<>'D' ", "WorkspaceNodeDetail")

            ddlgvChildActivity.DataSource = ds_Period.Tables(0)
            ddlgvChildActivity.DataValueField = "iNodeId"
            ddlgvChildActivity.DataTextField = "vNodeDisplayName"
            ddlgvChildActivity.DataBind()

            ddlgvChildActivity.SelectedValue = ds_Period.Tables(0).DefaultView.ToTable.Rows(0)("vNodeDisplayName")
            ddlgvChildActivity.ToolTip = ds_Period.Tables(0).DefaultView.ToTable.Rows(0)("vNodeDisplayName")

            upPeriod.Update()

        Catch ex As Exception
            'ShowErrorMessage(ex.Message, ".....bindGrid")
        End Try
    End Sub

    Protected Sub gvDeviation_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvDeviation.RowCommand
        Try
            If e.CommandName.ToUpper = "MYVALUE" Then

                Dim DS_DeviationReport As New DataSet
                Dim DsCheckProject As New DataSet
                Dim parameter As String

                DS_DeviationReport = CType(ViewState("DS_DeviationReport"), DataSet)

                upPeriod.Update()

                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gvDeviation.Rows(index)

                Dim devNegh As Integer = CType(gvRow.FindControl("txtDeviationNegative"), TextBox).Text
                Dim devPosit As Integer = CType(gvRow.FindControl("txtDeviationPositive"), TextBox).Text
                Dim win As Integer = CType(gvRow.FindControl("txtActualDays"), TextBox).Text
                Dim Id1 As Integer = CType(gvRow.FindControl("txtTracker"), TextBox).Text

                Dim ddlgvChildActivity As DropDownList = DirectCast(gvRow.FindControl("ddlgvChildActivity"), DropDownList)
                Dim Selectchild = ddlgvChildActivity.SelectedValue

                parameter = HProjectId.Value.ToString()
                DsCheckProject = objhelp.ProcedureExecute("dbo.CheckScheduleProject", parameter.ToString)

                If DsCheckProject.Tables(0).Rows.Count > 0 Then
                    objCommon.ShowAlert("Project has been Scheduled you can not do any changes!", Me.Page)
                    Exit Sub
                End If

                If HfInodeid.Value <> Selectchild Then

                    DS_DeviationReport.Tables(0).DefaultView.RowFilter = "InodeId = '" + Selectchild + "'"
                    If DS_DeviationReport.Tables(0).DefaultView.ToTable().Rows.Count > 0 Then
                        objCommon.ShowAlert("Activity Already Saved!", Me.Page)
                        Exit Sub
                    End If

                End If

                Dim dtEditvisit As DataTable
                DS_DeviationReport.Tables(0).DefaultView.RowFilter = "nTrackerNo=" + Id1.ToString()
                dtEditvisit = DS_DeviationReport.Tables(0).DefaultView.ToTable()

                For Each dr As DataRow In dtEditvisit.Rows
                    dr("iNodeid") = Selectchild
                    dr("iActualDays") = win
                    dr("iDeviationNegative") = devNegh
                    dr("iDeviationPositive") = devPosit
                    dr("iModifyBy") = Session(S_UserID).ToString()

                Next
                dtEditvisit.AcceptChanges()
                ViewState("Edit_Table") = dtEditvisit
                ModalRemarks.Show()

            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        End Try

    End Sub
#End Region

#Region "Web Methods"
    <Web.Services.WebMethod()> _
    Public Shared Function AuditTrailForVisitTracker(ByVal WorkSpaceDeviationId As Integer) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim ds As DataSet
        Dim eStr As String = String.Empty
        Dim Parameter As String = String.Empty
        Dim strReturn As String = String.Empty
        Try

            Parameter = WorkSpaceDeviationId.ToString() + "##"

            ds = objHelp.ProcedureExecute("dbo.GetVisitTrackerAudit", Parameter.ToString)

            Dim dtTempAuditTrail As New DataTable

            If Not dtTempAuditTrail Is Nothing Then
                dtTempAuditTrail.Columns.Add("vProjectNo", GetType(String))
                dtTempAuditTrail.Columns.Add("iVisitNo", GetType(String))
                dtTempAuditTrail.Columns.Add("vNodeDisplayName", GetType(String))
                dtTempAuditTrail.Columns.Add("iActualDays", GetType(String))
                dtTempAuditTrail.Columns.Add("iDeviationNegative", GetType(String))
                dtTempAuditTrail.Columns.Add("iDeviationPositive", GetType(String))
                dtTempAuditTrail.Columns.Add("vRemarks", GetType(String))
                dtTempAuditTrail.Columns.Add("iModifyBy", GetType(String))
                dtTempAuditTrail.Columns.Add("dModifyOn", GetType(String))

            End If

            dtTempAuditTrail.AcceptChanges()

            Dim drAuditTrail As DataRow

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                drAuditTrail = dtTempAuditTrail.NewRow()
                drAuditTrail("vProjectNo") = Convert.ToString(ds.Tables(0).Rows(i)("vProjectNo"))
                drAuditTrail("iVisitNo") = Convert.ToString(ds.Tables(0).Rows(i)("iVisitNo").ToString())
                drAuditTrail("vNodeDisplayName") = Convert.ToString(ds.Tables(0).Rows(i)("vNodeDisplayName").ToString())
                drAuditTrail("iActualDays") = Convert.ToString(ds.Tables(0).Rows(i)("iActualDays").ToString())
                drAuditTrail("iDeviationNegative") = Convert.ToString(ds.Tables(0).Rows(i)("iDeviationNegative"))
                drAuditTrail("iDeviationPositive") = Convert.ToString(ds.Tables(0).Rows(i)("iDeviationPositive"))
                drAuditTrail("vRemarks") = Convert.ToString(ds.Tables(0).Rows(i)("vRemarks"))
                drAuditTrail("iModifyBy") = Convert.ToString(ds.Tables(0).Rows(i)("vUserName"))
                drAuditTrail("dModifyOn") = Convert.ToString(CDate(ds.Tables(0).Rows(i)("dModifyOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                dtTempAuditTrail.Rows.Add(drAuditTrail)
                dtTempAuditTrail.AcceptChanges()
            Next i

            strReturn = JsonConvert.SerializeObject(dtTempAuditTrail)

            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function
#End Region

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR>" + eStr
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR>" + eStr)
    End Sub

#End Region

End Class
