Imports Newtonsoft.Json
Imports Microsoft
Imports Microsoft.Office.Interop
Imports System.Drawing
Imports System.Collections.Generic
Imports System.Collections
Imports System.Text
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports Winnovative
Imports System.Data.OleDb
Imports System.Data
Imports System.Xml

Partial Class frmCTDeviationReport
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objCommon As New clsCommon
    Private objhelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

    Private eStr_Retu As String = ""
    Public IsProjectLock As Boolean = False

    Private Const VS_WorkSpaceDeviationReport As String = "WorkSpaceDeviationReport"

    Private Const GVC_Edit As Integer = 0
    Private Const GVC_Project As Integer = 1
    Private Const GVC_ParentActivityName As Integer = 2
    Private Const GVC_ChileActivity As Integer = 3
    Private Const GVC_RefActivity As Integer = 4
    Private Const GVC_DeviationNegative As Integer = 5
    Private Const GVC_DeviationPositive As Integer = 6
    Private Const GVC_WindowPeriod As Integer = 7
    Private Const GVC_Update As Integer = 8
    Private Const GVC_Audit As Integer = 9
    Private Const GVC_WorkSpace As Integer = 10
    Private Const GVC_Status As Integer = 11



    'Private Const GVC_WorkspaceID As Integer = 9
    'Private Const GVC_Period As Integer = 10
    'Private Const GVC_ActivityID As Integer = 11
    'Private Const GVC_ParentNodeID As Integer = 12



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
        'Dim ds As New DataSet
        Try
            'If Not GenCall_Data(ds) Then
            '    Me.objCommon.ShowAlert("Error While Getting Data from WorkSpaceNodeDetail.", Me.Page)
            'End If

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
        Dim DS_DeviationReport As DataSet = Nothing

        Try
            DS_DeviationReport = Me.objhelp.GetResultSet("select *  from WorkSpaceDeviationReport where 1=2 ", "WorkSpaceDeviationReport")
            ViewState("DS_DeviationReport") = DS_DeviationReport
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
        Dim e As EventArgs
        Try
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Visit  Deviation Management"
            Page.Title = " :: Visit Deviation Management ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtProject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
            End If

            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID) + "AND vProjectTypeCode = '0014'"
            Me.AutoCompleteExtender2.ContextKey = "iUserId = " & Me.Session(S_UserID) + "AND vProjectTypeCode = '0014'"

            rbtSelection_SelectedIndexChanged(sender, e)
            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...............GenCall_ShowUI")
            GenCall_ShowUI = False
        End Try
    End Function

#End Region

#Region "Button Event"

    Protected Sub btnSetProject_Click(ByVal Sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim Wstr As String = String.Empty
        Dim ActivityparamArry(1) As String
        Dim dt_WorkSpaceActivity As New DataTable
        Dim dt_WorkSpaceNodes As New DataTable
        Dim dt_WorkSpaceNodeDetail As New DataTable
        Dim ds_CRFVersionDetail As DataSet = Nothing
        Dim ds_ParentWorkspace As DataSet = Nothing
        Dim ds_Editchecks As DataSet = Nothing
        Dim ds_CrfHdr As DataSet = Nothing
        Dim ds_Check As DataSet = Nothing
        Dim ds_WorkspaceNodeDetail As DataSet = Nothing
        Dim dv_ParentGrid As New DataView

        Try
            gvBasedOnParentVisit.DataSource = Nothing
            gvBasedOnParentVisit.DataBind()

            gvDeviation.DataSource = Nothing
            gvDeviation.DataBind()

            gvIndependentVisit.DataSource = Nothing
            gvIndependentVisit.DataBind()
            upPeriod.Update()

            'to check Project is Locked or not
            gvDeviation.DataSource = Nothing
            gvDeviation.DataBind()
            upreport.Update()

            Wstr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D' Order by iTranNo desc"
            If Not Me.objhelp.GetCRFLockDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                           ds_Check, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If
            If ds_Check.Tables(0).Rows.Count > 0 Then
                If Convert.ToString(ds_Check.Tables(0).Rows(0)("cLockFlag")).Trim.ToUpper() = "L" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ProjectLocked", "$(document).ready(function(){pageLoad(); msgalert('Project Is Locked')});", True)
                    Me.ViewState(IsProjectLock) = True
                Else
                    Me.ViewState(IsProjectLock) = False
                End If
            End If
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Me.ViewState(IsProjectLock) = False Then
                Wstr = "vWorkspaceId='" + Me.HProjectId.Value.ToString.Trim() + "'"
                If Not objhelp.GetData("View_CRFVersionForDataEntryControl", "*", Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CRFVersionDetail, eStr_Retu) Then  'To get Project Details
                    Throw New Exception(eStr_Retu)
                End If

                If ds_CRFVersionDetail.Tables(0).Rows.Count > 0 Then

                Else

                End If
            End If


            If Not FillParentActivity() Then
                Throw New Exception(eStr_Retu)
            End If

            If Not FillGrid() Then
                ShowErrorMessage("Error While Fill Data", "")
            End If
            'only view the Data
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....btnSetProject")
        End Try

    End Sub

    Protected Sub btnSetProjectReport_Click(sender As Object, e As EventArgs) Handles btnSetProjectReport.Click

        Dim eStr As String = String.Empty
        Dim ds_Subject As New DataSet
        Dim wStr As String = String.Empty

        gvBasedOnParentVisit.DataSource = Nothing
        gvBasedOnParentVisit.DataBind()
        ddlSUbject.DataSource() = Nothing
        ddlSUbject.DataBind()
        ddlSUbject.Items.Clear()
        upreport.Update()
        hdnSubSelection.Value = String.Empty


        gvDeviation.DataSource = Nothing
        gvDeviation.DataBind()

        gvIndependentVisit.DataSource = Nothing
        gvIndependentVisit.DataBind()
        upPeriod.Update()
        wStr = "vWorkSpaceId = '" + Me.HReportProjectId.Value.Trim() + "' OR  (vWorkSpaceID = '" + Me.HReportProjectId.Value.Trim() + "' OR  VParentWorkSPaceID = '" + Me.HReportProjectId.Value.Trim() + "')"
        'wStr = "vWorkspaceId = '" + Me.HReportProjectId.Value.Trim() + "'" + "OR"
        wStr += " Order by cast(iMySubjectNo as Numeric)"
        If Not objhelp.GetViewWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_Subject, eStr) Then
            Throw New Exception(eStr)
        End If

        hdnSubSelection.Value = ""
        ddlSUbject.DataSource = ds_Subject.Tables(0)
        ddlSUbject.DataValueField = "vSubjectId"
        ddlSUbject.DataTextField = "vMySubjectNo"
        ddlSUbject.DataBind()
        upPeriod.Update()
        If Not Activity() Then
            ShowErrorMessage("Error While Fill Activity", "")
        End If

        'If Not FillGrid() Then
        '    ShowErrorMessage("Error While Fill Data", "")
        'End If
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "destroygrid", "destroyGrid();", True)
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        'Dim dt As DataSet = CType(ViewState("DS_DeviationReport"), DataSet)
        Dim dt As DataSet = CType(ViewState("DS_DeviationReport"), DataSet)
        'dt.Tables(0).Columns.Add("vParentDisplayNode")
        'dt.Tables(0).Columns.Add("vChildDisplayNode")

        If (ddlChildActivity.SelectedValue = ddlReferanceActivity.SelectedValue) Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "msgalert('Chiled And RefActivity Sholud Not Be Same Please Change it.')", True)
            Exit Sub
        End If
        Try
            dt.Tables(0).Columns.Add("vProjectName")
        Catch ex As Exception
        End Try
        Try
            dt.Tables(0).Columns.Add("IsAdd")
        Catch ex As Exception
        End Try
        Try
            dt.Tables(0).Columns.Add("vWorkSpaceId")
        Catch ex As Exception
        End Try
        dt.AcceptChanges()
        For Each dr As DataRow In dt.Tables(0).Rows
            If (Convert.ToInt64(dr("iParentNodeId")) = Convert.ToInt64(ddlParentActivity.SelectedItem.Value)) Then
                txtActual.Text = ""
                txtNegative.Text = ""
                txtPositive.Text = ""
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "msgalert('You Have Already Added Deviation For This Visit.')", True)
                Exit Sub
            End If

        Next
        Dim dr1 As DataRow
        Try
            dr1 = dt.Tables(0).NewRow()
            dr1("vWorkSpaceId") = Me.HProjectId.Value
            dr1("iParentNodeId") = ddlParentActivity.SelectedItem.Value
            dr1("iChildNodeId") = ddlChildActivity.SelectedItem.Value
            dr1("iRefNodeId") = ddlReferanceActivity.SelectedItem.Value
            dr1("iWindowPeriod") = Me.txtActual.Text.Trim()
            dr1("iDeviationNegative") = Me.txtNegative.Text
            dr1("iDeviationPositive") = Me.txtPositive.Text
            dr1("cStatusIndi") = "N"
            dr1("iModifyby") = Session(S_UserID)
            dr1("dModifyOn") = DateTime.Now()
            dr1("vProjectName") = HProjectName.Value
            dr1("IsAdd") = "Y"
            dr1("nWorkSpaceDeviationId") = 0
            dt.Tables(0).Rows.Add(dr1)
            dt.AcceptChanges()

            Me.gvDeviation.DataSource = dt.Tables(0)
            Me.gvDeviation.DataBind()

            upPeriod.Update()

            'rbtSelection_SelectedIndexChanged(sender, e)
        Catch ex As Exception

        Finally
            txtActual.Text = ""
            txtNegative.Text = ""
            txtPositive.Text = ""
            ddlParentActivity.SelectedIndex = 0
            ddlChildActivity.SelectedIndex = 0
            ddlReferanceActivity.SelectedIndex = 0

            'If Not SetParameter() Then
            'End If
        End Try
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim ds_WorkSpaceDeviationReport As DataSet
        Dim eStr As String

        ds_WorkSpaceDeviationReport = CType(ViewState("DS_DeviationReport"), DataSet)
        'ds_WorkSpaceDeviationReport = CType(ViewState(VS_WorkSpaceDeviationReport), DataSet)

        If ds_WorkSpaceDeviationReport.Tables(0).Columns.Contains("IsAdd") Then
            ds_WorkSpaceDeviationReport.Tables(0).DefaultView.RowFilter = "IsAdd = 'Y'"
        Else
            Exit Sub
        End If
        ds_WorkSpaceDeviationReport.Tables(0).DefaultView.ToTable().AcceptChanges()

        Try
            ds_WorkSpaceDeviationReport.Tables(0).Columns.Remove("vProjectName")
        Catch ex As Exception
        End Try
        Try
            ds_WorkSpaceDeviationReport.Tables(0).Columns.Remove("IsAdd")
        Catch ex As Exception

        End Try
        ds_WorkSpaceDeviationReport.AcceptChanges()

        Dim dt As DataTable
        dt = ds_WorkSpaceDeviationReport.Tables(0).DefaultView.ToTable()
        dt.TableName = "WorkSpaceDeviationReport"
        dt.AcceptChanges()

        Dim ds As New DataSet
        ds.Tables.Add(dt)
        ds.AcceptChanges()
        If (ds.Tables(0).Rows.Count() = 0) Then
            Exit Sub
        End If

        If Not objLambda.Save_WorkSpaceDeviationReport(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds, Me.Session(S_UserID), eStr) Then
            objCommon.ShowAlert("Error While Saving WorkSpaceDeviationReport", Me.Page)
            Exit Sub
        Else
            objCommon.ShowAlert("Records Saved Successfully", Me.Page)
        End If

        If Not FillGrid() Then

        End If
        'objCommon.ShowAlert("Records Saved Successfully", Me.Page)
        'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "showalert123wewq1212", "Records Saved Successfully", True)
    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As EventArgs)
        Dim ds_Delete As DataSet
        ds_Delete = CType(ViewState("DS_DeviationReport"), DataSet)
        Dim dt As DataTable

        Try
            For Each row As GridViewRow In gvDeviation.Rows()
                'If row.RowType = DataControlRowType.DataRow Then
                Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("chkEdit"), CheckBox)
                If chkRow.Checked Then
                    'Dim name As String = row.Cells(1).Text
                    Dim Id = row.Cells(GVC_WorkSpace).Text

                    ds_Delete.Tables(0).DefaultView.RowFilter = "nWorkSpaceDeviationId=" + Id
                    ds_Delete.AcceptChanges()

                    dt = ds_Delete.Tables(0).DefaultView.ToTable()

                    For i As Integer = 0 To dt.Rows.Count - 1
                        dt.Rows(i)(8) = "D"
                    Next
                    dt.AcceptChanges()
                End If
            Next
            ModalRemarks.Show()
            btnSaveRemarks.Text = "Delete"
            ViewState("Delete_Table") = dt

        Catch ex As Exception

        Finally

        End Try
    End Sub

    Protected Sub btnSaveRemarks_Click(sender As Object, e As EventArgs) Handles btnSaveRemarks.Click
        If (btnSaveRemarks.Text.ToUpper() = "DELETE") Then
            Try


                Dim dt As DataTable

                dt = CType(ViewState("Delete_Table"), DataTable)

                For Each dr In dt.Rows
                    dr("vRemarks") = Me.txtRemarks.Text.Trim()
                    dr("iModifyBy") = Me.Session(S_UserID)
                Next

                Try
                    dt.Columns.Remove("vProjectName")
                Catch ex As Exception
                End Try

                dt.AcceptChanges()
                dt.TableName = "WorkSpaceDeviationReport"
                Dim eStr As String
                Dim ds_WorkSpaceDeviationReport As New DataSet
                ds_WorkSpaceDeviationReport.Tables.Add(dt)
                ds_WorkSpaceDeviationReport.AcceptChanges()

                If Not objLambda.Save_WorkSpaceDeviationReport(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_WorkSpaceDeviationReport, Me.Session(S_UserID), eStr) Then
                    objCommon.ShowAlert("Error While Saving WorkSpaceDeviationReport", Me.Page)
                    Exit Sub
                Else
                    objCommon.ShowAlert("Records Deleted Successfully !", Me.Page)
                    If Not FillGrid() Then
                        ShowErrorMessage("Error While FillGrid", "")
                    End If
                End If

            Catch ex As Exception

            Finally
                txtRemarks.Text = ""
                If (gvDeviation.Rows.Count = 0) Then
                    btnDelete.Style.Add("display", "none")
                    btnExit.Style.Add("display", "none")
                    btnCancel.Style.Add("display", "none")
                End If
            End Try

        Else
            Dim dt As DataTable

            Try

                dt = CType(ViewState("Edit_Table"), DataTable)

                For Each dr In dt.Rows
                    dr("vRemarks") = Me.txtRemarks.Text.Trim()
                    dr("iModifyBy") = Me.Session(S_UserID)
                Next

                Try
                    dt.Columns.Remove("vProjectName")
                Catch ex As Exception
                End Try

                dt.AcceptChanges()
                dt.TableName = "WorkSpaceDeviationReport"
                Dim eStr As String
                Dim ds_WorkSpaceDeviationReport As New DataSet
                ds_WorkSpaceDeviationReport.Tables.Add(dt)
                ds_WorkSpaceDeviationReport.AcceptChanges()

                If Not objLambda.Save_WorkSpaceDeviationReport(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_WorkSpaceDeviationReport, Me.Session(S_UserID), eStr) Then
                    objCommon.ShowAlert("Error While Saving WorkSpaceDeviationReport", Me.Page)
                    Exit Sub
                Else

                    If Not FillGrid() Then
                        ShowErrorMessage("Error While FillGrid", "")
                    End If
                    Me.objCommon.ShowAlert("Record Updated Sucessfully", Me.Page)
                End If

            Catch ex As Exception
                Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
            Finally
                txtRemarks.Text = ""

                If (gvDeviation.Rows.Count = 0) Then
                    btnDelete.Style.Add("display", "none")
                    btnExit.Style.Add("display", "none")
                    btnCancel.Style.Add("display", "none")
                End If

            End Try

        End If


    End Sub

    Protected Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        Dim WorkSpaceId As String
        Dim SubjectId As String = ""
        Dim WorkSpaceDeviationReport As Integer
        Dim RefActivity As Integer
        Dim ChildActivity As Integer
        Dim estr As String
        Dim ds_WorkSpace As DataSet
        Dim SubejctID As String = ""
        SubjectId = hdnSubSelection.Value
        SubjectId = SubjectId.Replace("'", "")
        Try
            'hdnSubSelection.Value = ""
            Dim ds_DeviationPeriod As DataSet

            If Not objhelp.Proc_GetDeviationPeriod(HReportProjectId.Value, ds_DeviationPeriod, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            ViewState("DeviationPeriod") = ds_DeviationPeriod

        Catch ex As Exception

        End Try
        If (rbtreporttype.SelectedIndex = 1) Then
            WorkSpaceId = HReportProjectId.Value.ToString().Trim()
            SubjectId = hdnSubSelection.Value.ToString().Trim()
            WorkSpaceDeviationReport = 0
            SubjectId = SubjectId.Replace("'", "")
            RefActivity = CType(ddlFromVisit.SelectedValue, Integer)
            ChildActivity = CType(ddlToVisit.SelectedValue, Integer)

            If Not objhelp.Proc_WorkSpaceDeviationReportForIndependentReport(WorkSpaceId, SubjectId, WorkSpaceDeviationReport, RefActivity, ChildActivity, ds_WorkSpace, estr) Then
                ShowErrorMessage("Error While get Data WorkSpace", "")
            End If
            If (ds_WorkSpace Is Nothing) Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fixheader", "msgalert('There is No Data')", True)
                Exit Sub
            End If

            If Not (ds_WorkSpace.Tables(0).Rows.Count > 0) Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fixheader", "msgalert('There is No Data')", True)
                Exit Sub
            End If

            Dim dt As New DataTable
            If Not (ds_WorkSpace Is Nothing AndAlso ds_WorkSpace.Tables(0).Rows.Count > 0) Then

                dt.Columns.Add("SrNo", GetType(Integer))
                dt.Columns.Add("ProjectNo", GetType(String))
                dt.Columns.Add("SubjectNo", GetType(String))
                dt.Columns.Add("RandomizationNo", GetType(String))
                Try
                    dt.Columns.Add(ddlFromVisit.SelectedItem.Text, GetType(String))
                Catch ex As Exception
                End Try
                Try
                    dt.Columns.Add(ddlToVisit.SelectedItem.Text, GetType(String))
                Catch ex As Exception
                End Try

                dt.AcceptChanges()

                Dim drAuditTrail As DataRow
                Try
                    For i As Integer = 0 To ds_WorkSpace.Tables(0).Rows.Count - 1
                        drAuditTrail = dt.NewRow()
                        drAuditTrail("SrNo") = i + 1
                        drAuditTrail("ProjectNo") = Convert.ToString(ds_WorkSpace.Tables(0).Rows(i)("vProjectNo"))
                        drAuditTrail("SubjectNo") = Convert.ToString(ds_WorkSpace.Tables(0).Rows(i)("vSubjectId"))
                        drAuditTrail("RandomizationNo") = Convert.ToString(ds_WorkSpace.Tables(0).Rows(i)("vRandomizationNo"))
                        drAuditTrail(ddlFromVisit.SelectedItem.Text) = ""
                        drAuditTrail(ddlToVisit.SelectedItem.Text) = Convert.ToString(ds_WorkSpace.Tables(0).Rows(i)("Diff"))
                        dt.Rows.Add(drAuditTrail)
                        dt.AcceptChanges()
                    Next
                Catch ex As Exception
                Finally
                    hdnSubSelection.Value = ""
                End Try
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fixheader", "msgalert('There Is No Data')", True)
                Exit Sub
            End If

            ViewState("gvBasedOnIndependentVisit") = dt

            gvIndependentVisit.DataSource() = dt
            gvIndependentVisit.DataBind()
            IndependentVisit.Visible = True
            gvIndependentVisit.Visible = True
            gvBasedOnParentVisit.Visible = False
            TrLegends.Visible = True
            upreport.Update()

        Else
            SubejctID = hdnSubSelection.Value
            SubjectId = SubejctID.Replace("'", "")

            WorkSpaceId = HReportProjectId.Value

            If Not objhelp.Proc_WorkSpaceDeviationReportForParentVisit(WorkSpaceId, SubjectId, ds_WorkSpace, estr) Then
                ShowErrorMessage("Error While get Data WorkSpace", "")
            End If

            Dim dt_Structure As DataTable
            Dim dt_Result As DataTable


            If (ds_WorkSpace Is Nothing) Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fixheader", "msgalert('There Is No Data')", True)
                Exit Sub
            End If

            If (ds_WorkSpace.Tables(0).Rows.Count > 0) Then
                dt_Structure = ds_WorkSpace.Tables(0)
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fixheader", "msgalert('There Is No Data')", True)
                Exit Sub
            End If


            Dim dt As DataTable = ds_WorkSpace.Tables(0).Clone

            For Each dc As DataColumn In dt.Columns
                dc.DataType = GetType(String)
                dt.AcceptChanges()
            Next

            For Each dr As DataRow In ds_WorkSpace.Tables(0).Rows
                dt.ImportRow(dr)
            Next


            ''dt = ds_WorkSpace.Tables(0).Copy()
            dt.AcceptChanges()

            'If (ds_WorkSpace.Tables(1).Rows.Count > 0) Then
            '    dt_Structure = ds_WorkSpace.Tables(0)
            'Else
            '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fixheader", "alert('There is no data')", True)
            '    Exit Sub
            'End If


            Try
                If (ds_WorkSpace.Tables(1).Rows.Count > 0) Then
                    dt_Result = ds_WorkSpace.Tables(1)

                    If (dt.Rows.Count > 0) Then
                        For Each dr As DataRow In dt.Rows
                            For Each dr1 As DataRow In dt_Result.Rows
                                If (dr("SubjectNo") = dr1("vSubjectId")) Then
                                    For Each dc As DataColumn In dt.Columns
                                        If (dc.ColumnName = dr1(4)) Then
                                            dr(dc.ColumnName) = dr1(7)
                                            dt.AcceptChanges()
                                        End If
                                    Next
                                End If
                            Next
                        Next

                    End If

                End If
            Catch ex As Exception
            Finally
                hdnSubSelection.Value = ""
            End Try

            dt_Structure.AcceptChanges()
            ViewState("gvBasedOnParentVisit") = dt
            Try
                gvBasedOnParentVisit.DataSource = dt
                gvBasedOnParentVisit.DataBind()
                gvBasedOnParentVisit.Visible = True
                gvIndependentVisit.Visible = False
                TrLegends.Visible = True
                upreport.Update()
            Catch ex As Exception
            End Try
        End If
    End Sub

    Protected Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        Dim dt As New DataTable
        Dim filename As String


        Try
            If (rbtreporttype.SelectedIndex = 1) Then
                If gvIndependentVisit.Rows.Count > 0 Then
                    Dim info As String = String.Empty
                    Dim gridviewHtml As String = String.Empty

                    filename = "Visit Deviation Report" + ".xls"

                    Dim stringWriter As New System.IO.StringWriter()
                    Dim writer As New HtmlTextWriter(stringWriter)
                    gvIndependentVisit.RenderControl(writer)
                    gridviewHtml = stringWriter.ToString()

                    gridviewHtml = "<table><tr><td align = ""center"" colspan=""9""><b>" + System.Configuration.ConfigurationManager.AppSettings("Client").ToString() + "<br/></b></td></tr></table><table><tr><td align = ""center"" colspan=""2""><b>" + " Visit Deviation Report: <br/></b></td></tr></table><span>Print Date:-  " + Date.Today.ToString("dd-MMM-yyyy") + "</span>" + gridviewHtml

                    Context.Response.Buffer = True
                    Context.Response.ClearContent()
                    Context.Response.ClearHeaders()

                    Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
                    Context.Response.AddHeader("content-Disposition", "attachment; filename=" + filename)
                    Context.Response.AddHeader("Content-Length", gridviewHtml.Length)

                    Context.Response.Write(gridviewHtml)
                    Context.Response.Flush()
                    Context.Response.End()
                    'System.IO.File.Delete(filename)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "norecord1324", " msgalert('Please Click On GO Button First.')});", True)
                End If
            Else

                If gvBasedOnParentVisit.Rows.Count > 0 Then
                    Dim info As String = String.Empty
                    Dim gridviewHtml As String = String.Empty

                    filename = "Visit Deviation Report" + ".xls"

                    Dim stringWriter As New System.IO.StringWriter()
                    Dim writer As New HtmlTextWriter(stringWriter)
                    gvBasedOnParentVisit.RenderControl(writer)
                    gridviewHtml = stringWriter.ToString()

                    gridviewHtml = "<table><tr><td align = ""center"" colspan=""9""><b>" + System.Configuration.ConfigurationManager.AppSettings("Client").ToString() + "<br/></b></td></tr></table><table><tr><td align = ""center"" colspan=""2""><b>" + "   Visit Deviation Report: <br/></b></td></tr></table><span>Print Date:-  " + Date.Today.ToString("dd-MMM-yyyy") + "</span>" + gridviewHtml

                    Context.Response.Buffer = True
                    Context.Response.ClearContent()
                    Context.Response.ClearHeaders()

                    Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
                    Context.Response.AddHeader("content-Disposition", "attachment; filename=" + filename)
                    Context.Response.AddHeader("Content-Length", gridviewHtml.Length)

                    Context.Response.Write(gridviewHtml)
                    Context.Response.Flush()
                    Context.Response.End()

                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "norecord123", " msgalert('Please Click On GO Button First.')});", True)
                End If
            End If



        Catch ex As Exception
        End Try

    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub

    Protected Sub btnPDF_Click(sender As Object, e As EventArgs) Handles btnPDF.Click
        Dim pdfFont As System.Drawing.Font
        Dim headercontent As String = String.Empty
        Dim htmlcontent As String = String.Empty
        Dim strdatetime As String = ""
        Dim project() As String
        Dim wstr, ProjectNo, SiteId As String
        Dim d1 As Document
        Dim downloadbytes As Byte()
        Dim ds_project As New DataSet
        Try
            wstr = "vWorkspaceId = '" + Me.HReportProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"
            If Not objhelp.GetFieldsOfTable("View_getWorkspaceDetailForHdr", "*", wstr, ds_project, eStr_Retu) Then
                Throw New Exception("Error while getting Header information. " + eStr_Retu)
            End If

            If ds_project Is Nothing Or ds_project.Tables.Count = 0 Or ds_project.Tables(0) Is Nothing Or ds_project.Tables(0).Rows.Count = 0 Then
                Exit Sub
            End If

            project = ds_project.Tables(0).Rows(0)("vProjectNo").ToString().Split(":")
            Dim strLen As Int32 = project(0).ToString.LastIndexOf("-")
            ProjectNo = project(0).ToString

            If ds_project.Tables(0).Rows(0)("cWorkspaceType").ToString.Trim().ToUpper() = "C" Then
                ProjectNo = project(0).Substring(0, strLen)
                strLen = strLen + 1
                SiteId = project(0).Substring(strLen)
            End If
            Dim Path As String = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.ToString.LastIndexOf("/"))
            Path = Path + "/images/lambda_logo.jpg"
            If (rbtreporttype.SelectedIndex = 1) Then
                If gvIndependentVisit.Rows.Count > 0 Then

                    headercontent += "<html><head></head><body>"
                    headercontent += "<table style=""margin-top: 2px; margin: auto; border: solid 1px black; width: 100%; font-family: 'Times New Roman'; font-size:12px; ""align=""left"">"
                    headercontent += "<tr>"
                    headercontent += "<td>"
                    headercontent += "<Label  for """ + ConfigurationManager.AppSettings.Item("Client").ToString() + """>" + ConfigurationManager.AppSettings.Item("Client").ToString() + "</asp:Label>"
                    headercontent += "</td>"
                    headercontent += "<td>"
                    headercontent += "Visit Deviation Report"
                    headercontent += "</td>"
                    headercontent += "<td align=""right"">Project No:</td>"
                    headercontent += "<td align=""left"">"
                    headercontent += "<Label  for """ + ProjectNo + """>" + ProjectNo + "</asp:Label>"
                    headercontent += "</td>"
                    If ds_project.Tables(0).Rows(0)("cWorkspaceType").ToString.Trim().ToUpper() = "C" Then
                        headercontent += "<td align=""right"">Site Id:</td>"
                        headercontent += "<td align=""left"">"
                        headercontent += "<Label  for """ + SiteId + """>" + SiteId + "</asp:Label>"
                        headercontent += "</td>"
                    End If

                    headercontent += "<td valign=""middle"">"
                    headercontent += "<img id=ctl00_CPHLAMBDA_ImgLogo alt=""" + Path.ToString() + """ src=""" + Path.ToString() + """right"" alt=""lambda"" style=""width:120px; height:120px;""/>"
                    headercontent += "</td>"
                    headercontent += "</tr>"
                    headercontent += "</table>"
                    headercontent += "</body></html>"

                    Dim dataTable As DataTable = TryCast(ViewState("gvBasedOnIndependentVisit"), DataTable)

                    dataTable.Columns(1).ColumnName = "Project No"
                    dataTable.Columns(2).ColumnName = "Subject No"
                    dataTable.Columns(3).ColumnName = "Randomization No"
                    dataTable.AcceptChanges()
                    Dim strpdf As String = ""
                    strpdf = "<Table style=""border: solid 1px gray; border-collapse: collapse; width: 99%; font-family: Times New Roman; font-size:12px !important;"">"
                    strpdf += "<tr style=""color:Black;background-color:White;font-family:'Times New Roman' !important; text-align: center; font-size:16px;"">"
                    For i As Integer = 0 To dataTable.Columns.Count - 1
                        strpdf += "<td style=""border: 1px Solid Black;"">"
                        strpdf += dataTable.Columns(i).ToString
                        strpdf += "</td>"
                    Next
                    strpdf += "</tr>"
                    For i As Integer = 0 To dataTable.Rows.Count - 1
                        strpdf += "<tr style=""color:Black;background-color:White;font-family:'Times New Roman' !important; text-align: center; font-size:16px; page-break-inside:avoid;"">"
                        For j As Integer = 0 To dataTable.Columns.Count - 1
                            strpdf += "<td style=""border: 1px Solid Black;"">"
                            If j = 0 Then
                                strpdf += Convert.ToString(i + 1)
                            Else
                                strpdf += dataTable.Rows(i)(j).ToString()
                            End If

                            strpdf += "</td>"
                        Next
                        strpdf += "</tr>"
                        strpdf += "<tr style=""color:Black;background-color:White;font-family:'Times New Roman' !important;  font-size:16px; page-break-inside:avoid;"">"
                        strpdf += "</tr>"

                    Next
                    strpdf += "</Table>"


                    Dim pdfconverter As HtmlToPdfConverter = New HtmlToPdfConverter()
                    pdfconverter.LicenseKey = "dfvo+uv66OPj6vrr6PTq+unr9Ovo9OPj4+P66g=="
                    pdfconverter.PdfDocumentOptions.AvoidImageBreak = True
                    pdfconverter.PdfDocumentOptions.AvoidTextBreak = False
                    pdfconverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4
                    pdfconverter.PdfDocumentOptions.EmbedFonts = True
                    pdfconverter.PdfDocumentOptions.JpegCompressionEnabled = True
                    pdfconverter.PdfDocumentOptions.JpegCompressionLevel = 0
                    pdfconverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Best
                    pdfconverter.PdfDocumentOptions.LeftMargin = 72
                    pdfconverter.PdfDocumentOptions.RightMargin = 27
                    pdfconverter.PdfDocumentOptions.TopMargin = 27
                    pdfconverter.PdfDocumentOptions.BottomMargin = 27
                    pdfconverter.HtmlViewerWidth = 662     ''Previous 750
                    pdfconverter.PdfDocumentOptions.FitWidth = False
                    pdfconverter.PdfDocumentOptions.StretchToFit = True
                    pdfconverter.PdfBookmarkOptions.AllowDefaultTitle = True
                    pdfconverter.PdfBookmarkOptions.AutoBookmarksEnabled = True
                    pdfFont = New System.Drawing.Font("Times New Roman", 12, FontStyle.Bold, GraphicsUnit.Point)

                    pdfconverter.PdfDocumentOptions.ShowHeader = True
                    'Dim Path As String = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.ToString.LastIndexOf("/"))
                    'ImgLogo.Src = Path + "/images/lambda_logo.jpg"
                    'headercontent = Regex.Replace(Me.HFHeaderPDF.Value.ToString(), "<IMG[^>]+", "<IMG id=ctl00_CPHLAMBDA_ImgLogo alt=" + ImgLogo.Src.ToString + " align=right src=" + ImgLogo.Src.ToString + " /", RegexOptions.IgnoreCase)

                    pdfconverter.PdfHeaderOptions.HeaderHeight = 140
                    Dim Header1 As New HtmlToPdfElement(headercontent, String.Empty)
                    Header1.HtmlViewerWidth = 662     ''Previous 750
                    Header1.FitWidth = False
                    pdfconverter.PdfHeaderOptions.AddElement(Header1)

                    pdfconverter.PdfDocumentOptions.ShowFooter = True
                    pdfconverter.PdfFooterOptions.FooterHeight = 50
                    'If hdnSubSelection.Value.Trim() <> "" Then
                    ''    'pdfconverter.PdfFooterOptions.AddElement(New TextElement(0, 15, "[Authenticated By:" + CType(Session(VS_AuthenticatedBy), String) + "]  [Authenticated On:" + CType(Session(VS_AuthenticatedOn), String) + "]", PdfFont))
                    ''End If
                    pdfconverter.PdfFooterOptions.PageNumberingStartIndex = 0
                    pdfconverter.PdfFooterOptions.AddElement(New LineElement(0, 0, pdfconverter.PdfDocumentOptions.PdfPageSize.Width - pdfconverter.PdfDocumentOptions.LeftMargin - pdfconverter.PdfDocumentOptions.RightMargin, 0))
                    Dim footerText As New TextElement(0, 5, "                                                                                                  Page &p; of &P;                       ", New Font(New FontFamily("Times New Roman"), 12, GraphicsUnit.Point))
                    footerText.TextAlign = HorizontalTextAlign.Right
                    footerText.ForeColor = Color.Navy
                    footerText.EmbedSysFont = True
                    pdfconverter.PdfFooterOptions.AddElement(footerText)

                    'Dim writer As New StringWriter
                    'Dim htmlWriter As New HtmlTextWriter(writer)
                    'Try
                    '    gvIndependentVisit.RenderControl(htmlWriter)
                    'Catch ex As HttpException

                    'End Try

                    'htmlcontent = Me.HFHeaderLabel.Value.ToString()
                    'htmlcontent = htmlcontent + writer.ToString()

                    btnPDF.Enabled = False

                    d1 = pdfconverter.ConvertHtmlToPdfDocumentObject(strpdf, "")
                    downloadbytes = d1.Save()

                    Dim response As System.Web.HttpResponse = System.Web.HttpContext.Current.Response
                    response.Clear()
                    response.ContentType = "application/pdf"
                    response.AddHeader("content-disposition", "attachment; filename= Visit Deviation Report.pdf; size=" & downloadbytes.Length.ToString())
                    response.Flush()
                    response.BinaryWrite(downloadbytes)
                    response.Flush()
                    response.End()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "norecord12", " msgalert('Please Click On GO Button First.')});", True)
                End If


            Else

                If (gvBasedOnParentVisit.Rows.Count > 0) Then


                    Dim dataTable As DataTable = TryCast(ViewState("gvBasedOnParentVisit"), DataTable)
                    'Dim Row As Integer = dataTable.Rows.Count
                    'Dim column As Integer = dataTable.Columns.Count
                    'Dim table As Integer = (column - 3) / 2

                    'Dim ds As New DataSet

                    'For i As Integer = 0 To table - 1
                    '    Dim dt1 As New DataTable
                    '    dt1.Columns.Add("ProjectNo")
                    '    dt1.Columns.Add("SubjectId")
                    '    dt1.Columns.Add("RandomizatioNo")
                    '    dt1.Columns.Add("Visit1")
                    '    dt1.Columns.Add("Visit2")
                    '    dt1.AcceptChanges()
                    '    ds.Tables.Add(dt1)
                    '    ds.AcceptChanges()
                    'Next

                    'ds.AcceptChanges()

                    'For j As Integer = 0 To table
                    '    For i As Integer = 0 To Row
                    '        ds.Tables(i).NewRow()
                    '        ds.Tables(i).AcceptChanges()

                    '        For Each dr In ds.Tables(i).Rows
                    '            dr("ProjectNo") = dataTable.Rows(1)
                    '            dr("SubjectId") = dataTable.Rows(2)
                    '            dr("RandomizatioNo") = dataTable.Rows(3)
                    '            dr("Visit1") = ""
                    '            dr("Visit2") = ""
                    '            ds.Tables(i).Rows.Add()
                    '        Next
                    '    Next
                    'Next
                    'ds.AcceptChanges()
                    'Dim Visit As String

                    dataTable.Columns(0).ColumnName = "Project No"
                    dataTable.Columns(1).ColumnName = "Subject no"
                    dataTable.Columns(2).ColumnName = "Randomization No"
                    dataTable.AcceptChanges()
                    Dim strpdf As String = ""
                    strpdf = "<Table style=""border: solid 1px gray; border-collapse: collapse; width: 99%; font-family: Times New Roman; font-size:12px !important;"">"
                    strpdf += "<tr style=""color:Black;background-color:White;font-family:'Times New Roman' !important; text-align: center; font-size:16px;"">"
                    For i As Integer = 0 To dataTable.Columns.Count - 1
                        strpdf += "<td style=""border: 1px Solid Black;"">"
                        strpdf += dataTable.Columns(i).ToString
                        strpdf += "</td>"
                    Next
                    strpdf += "</tr>"
                    For i As Integer = 0 To dataTable.Rows.Count - 1
                        strpdf += "<tr style=""color:Black;background-color:White;font-family:'Times New Roman' !important; text-align: center; font-size:16px; page-break-inside:avoid;"">"
                        For j As Integer = 0 To dataTable.Columns.Count - 1
                            strpdf += "<td style=""border: 1px Solid Black;"">"
                            If j = 0 Then
                                strpdf += dataTable.Rows(i)(j).ToString()
                                'strpdf += Convert.ToString(i + 1)
                            Else
                                strpdf += dataTable.Rows(i)(j).ToString()
                            End If

                            strpdf += "</td>"
                        Next
                        strpdf += "</tr>"
                        strpdf += "<tr style=""color:Black;background-color:White;font-family:'Times New Roman' !important;  font-size:16px; page-break-inside:avoid;"">"
                        strpdf += "</tr>"

                    Next
                    strpdf += "</Table>"

                    headercontent += "<html><head></head><body>"
                    headercontent += "<table style=""margin-top: 2px; margin: auto; border: solid 1px black; width: 100%; font-family: 'Times New Roman'; font-size:12px; ""align=""left"">"
                    headercontent += "<tr>"
                    headercontent += "<td>"
                    headercontent += "<Label  for """ + ConfigurationManager.AppSettings.Item("Client").ToString() + """>" + ConfigurationManager.AppSettings.Item("Client").ToString() + "</asp:Label>"
                    headercontent += "</td>"
                    headercontent += "<td>"
                    headercontent += "Visit Deviation Report"
                    headercontent += "</td>"
                    headercontent += "<td align=""right"">Project No:</td>"
                    headercontent += "<td align=""left"">"
                    headercontent += "<Label  for """ + ProjectNo + """>" + ProjectNo + "</asp:Label>"
                    headercontent += "</td>"
                    If ds_project.Tables(0).Rows(0)("cWorkspaceType").ToString.Trim().ToUpper() = "C" Then
                        headercontent += "<td align=""right"">Site Id:</td>"
                        headercontent += "<td >"
                        headercontent += "<Label  for """ + SiteId + """>" + SiteId + "</asp:Label>"
                        headercontent += "</td>"
                    End If

                    headercontent += "<td valign=""middle"" align=""right"">"
                    headercontent += "<img id=ctl00_CPHLAMBDA_ImgLogo alt=""" + Path.ToString() + """ src=""" + Path.ToString() + """right"" alt=""lambda"" style=""width:120px; height:120px;""/>"
                    headercontent += "</td>"
                    headercontent += "</tr>"
                    headercontent += "</table>"
                    headercontent += "</body></html>"

                    ''Commented By Nipun Khant 10-10-2015 12:04
                    '========================================================================
                    'Dim j As Integer = 3
                    'For I As Integer = 0 To table - 1
                    '    Dim k As Integer = 1
                    '    If j >= dataTable.Columns.Count - 1 Then
                    '        k = 0
                    '    End If

                    '    PlaceMedEx.Controls.Add(New LiteralControl("<Tr style=""page-break-inside:avoid; font-family: Times New Roman; font-size:12px !important;"">"))
                    '    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""border: solid 1px gray; font-family: Times New Roman; font-size:12px !important; text-align: center;"">"))
                    '    PlaceMedEx.Controls.Add(New LiteralControl(dataTable.Columns(0).ToString()))
                    '    PlaceMedEx.Controls.Add(New LiteralControl("</td>"))

                    '    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""border: solid 1px gray; font-family: Times New Roman; font-size:12px !important; text-align: center;"">"))
                    '    PlaceMedEx.Controls.Add(New LiteralControl(dataTable.Columns(1).ToString()))

                    '    PlaceMedEx.Controls.Add(New LiteralControl("</td>"))
                    '    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""border: solid 1px gray; font-family: Times New Roman; font-size:12px !important; text-align: center;"">"))
                    '    PlaceMedEx.Controls.Add(New LiteralControl(dataTable.Columns(2).ToString()))

                    '    PlaceMedEx.Controls.Add(New LiteralControl("</td>"))

                    '    For l As Integer = 0 To k
                    '        PlaceMedEx.Controls.Add(New LiteralControl("<td style=""border: solid 1px gray; font-family: Times New Roman; font-size:12px !important; text-align: center;"">"))
                    '        PlaceMedEx.Controls.Add(New LiteralControl(dataTable.Columns(j + l).ToString()))
                    '        PlaceMedEx.Controls.Add(New LiteralControl("</td>"))
                    '    Next

                    '    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                    '    For i1 As Integer = 0 To dataTable.Rows.Count - 1
                    '        PlaceMedEx.Controls.Add(New LiteralControl("<Tr style=""page-break-inside:avoid; font-family: Times New Roman; font-size:12px !important;"">"))
                    '        PlaceMedEx.Controls.Add(New LiteralControl("<td style=""border: solid 1px gray; font-family: Times New Roman; font-size:12px !important; text-align: center;"">"))
                    '        PlaceMedEx.Controls.Add(New LiteralControl(dataTable.Rows(i1)(0).ToString()))
                    '        PlaceMedEx.Controls.Add(New LiteralControl("</td>"))
                    '        PlaceMedEx.Controls.Add(New LiteralControl("<td style=""border: solid 1px gray; font-family: Times New Roman; font-size:12px !important; text-align: center;"">"))
                    '        PlaceMedEx.Controls.Add(New LiteralControl(dataTable.Rows(i1)(1).ToString()))
                    '        PlaceMedEx.Controls.Add(New LiteralControl("</td>"))
                    '        PlaceMedEx.Controls.Add(New LiteralControl("<td style=""border: solid 1px gray; font-family: Times New Roman; font-size:12px !important; text-align: center;"">"))
                    '        PlaceMedEx.Controls.Add(New LiteralControl(dataTable.Rows(i1)(2).ToString()))
                    '        PlaceMedEx.Controls.Add(New LiteralControl("</td>"))

                    '        For l As Integer = 0 To k

                    '            PlaceMedEx.Controls.Add(New LiteralControl("<td style=""border: solid 1px gray; font-family: Times New Roman; font-size:12px !important; text-align: center;"">"))
                    '            PlaceMedEx.Controls.Add(New LiteralControl(dataTable.Rows(i1)(j + l).ToString()))
                    '            PlaceMedEx.Controls.Add(New LiteralControl("</td>"))
                    '        Next
                    '        PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                    '    Next

                    '    j = j + 2
                    'Next
                    'PlaceMedEx.Controls.Add(New LiteralControl("</Table"))
                    '========================================================================

                    Dim sb As New StringBuilder()
                    Dim sw As New StringWriter(sb)
                    Dim writer As New HtmlTextWriter(sw)

                    PlaceMedEx.RenderControl(writer)
                    sb.ToString()

                    If gvBasedOnParentVisit.Rows.Count > 0 Then
                        Dim pdfconverter As HtmlToPdfConverter = New HtmlToPdfConverter()
                        pdfconverter.LicenseKey = "dfvo+uv66OPj6vrr6PTq+unr9Ovo9OPj4+P66g=="
                        pdfconverter.PdfDocumentOptions.AvoidImageBreak = True
                        pdfconverter.PdfDocumentOptions.AvoidTextBreak = False
                        pdfconverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Landscape
                        'pdfconverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4
                        pdfconverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A3
                        pdfconverter.PdfDocumentOptions.EmbedFonts = True
                        pdfconverter.PdfDocumentOptions.JpegCompressionEnabled = True
                        pdfconverter.PdfDocumentOptions.JpegCompressionLevel = 0
                        pdfconverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Best
                        pdfconverter.PdfDocumentOptions.TopMargin = 27
                        pdfconverter.PdfDocumentOptions.BottomMargin = 27
                        pdfconverter.HtmlViewerWidth = 1727     ''Previous 750
                        'pdfconverter.PdfDocumentOptions.FitWidth = False
                        'pdfconverter.PdfDocumentOptions.StretchToFit = True
                        pdfconverter.PdfBookmarkOptions.AllowDefaultTitle = True
                        pdfconverter.PdfBookmarkOptions.AutoBookmarksEnabled = True
                        pdfFont = New System.Drawing.Font("Times New Roman", 12, FontStyle.Bold, GraphicsUnit.Point)

                        pdfconverter.PdfDocumentOptions.ShowHeader = True
                        'Dim Path As String = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.ToString.LastIndexOf("/"))
                        'ImgLogo.Src = Path + "/images/lambda_logo.jpg"
                        'headercontent = Regex.Replace(Me.HFHeaderPDF.Value.ToString(), "<IMG[^>]+", "<IMG id=ctl00_CPHLAMBDA_ImgLogo style=""width:120px; height:110px;"" alt=" + ImgLogo.Src.ToString + " align=right src=" + ImgLogo.Src.ToString + " /", RegexOptions.IgnoreCase)

                        pdfconverter.PdfHeaderOptions.HeaderHeight = 120
                        Dim Header1 As New HtmlToPdfElement(headercontent, String.Empty)
                        Header1.HtmlViewerWidth = 1727     ''Previous 750
                        'Header1.FitWidth = False
                        'pdfconverter.PdfDocumentOptions.StretchToFit = True
                        pdfconverter.PdfHeaderOptions.AddElement(Header1)

                        pdfconverter.PdfDocumentOptions.ShowFooter = True
                        pdfconverter.PdfFooterOptions.FooterHeight = 50
                        pdfconverter.PdfFooterOptions.PageNumberingStartIndex = 0
                        pdfconverter.PdfFooterOptions.AddElement(New LineElement(0, 0, 1700, 0))
                        Dim footerText As New TextElement(0, 5, "                                                                                                                                                                                                                                                                  Page &p; of &P;                                 ", New Font(New FontFamily("Times New Roman"), 12, GraphicsUnit.Point))
                        footerText.Width = 1700
                        footerText.TextAlign = HorizontalTextAlign.Left
                        footerText.ForeColor = Color.Navy
                        footerText.EmbedSysFont = True
                        pdfconverter.PdfFooterOptions.AddElement(footerText)

                        'Dim SB As New StringBuilder()
                        'Dim SW As New StringWriter(SB)
                        'Dim htmlTW As New HtmlTextWriter(SW)

                        'gvBasedOnParentVisit.RenderControl(htmlTW)

                        ' Get the HTML into a string.
                        ' This will be used in the body of the email report.
                        '---------------------------------------------------
                        ''  Dim dataGridHTML As String = SB.ToString()

                        'htmlcontent = sb.ToString()

                        d1 = pdfconverter.ConvertHtmlToPdfDocumentObject(strpdf, "")
                        downloadbytes = d1.Save()

                        Dim response As System.Web.HttpResponse = System.Web.HttpContext.Current.Response
                        response.Clear()
                        response.ContentType = "application/pdf"
                        response.AddHeader("content-disposition", "attachment; filename=  Visit Deviation Report.pdf; size=" & downloadbytes.Length.ToString())
                        response.Flush()
                        response.BinaryWrite(downloadbytes)
                        response.Flush()
                        response.End()
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "norecord1", " msgalert('Please Click On GO Button First.')});", True)
                End If
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message.ToString(), "..................btnPDF_Click")
        End Try
    End Sub

    Protected Function SetParameter()
        Try
            txtActual.Text = ""
            txtNegative.Text = ""
            txtPositive.Text = ""
            ddlParentActivity.SelectedIndex = 0
            ddlChildActivity.SelectedIndex = 0
            ddlReferanceActivity.SelectedIndex = 0
            txtProject.Text = ""
            HProjectId.Value = ""
        Catch ex As Exception

        End Try
    End Function

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs)
        Response.Redirect("frmCTDeviationReport.aspx")
    End Sub

    Protected Sub btnCancel1_Click(sender As Object, e As EventArgs)
        txtRemarks.Text = ""
    End Sub

    Protected Sub btnExit_Click(sender As Object, e As EventArgs)
        Me.Response.Redirect("frmMainPage.aspx", False)
    End Sub

    Protected Sub btnExitBasedOnParent_Click(sender As Object, e As EventArgs)
        Me.Response.Redirect("frmMainPage.aspx", False)
    End Sub

#End Region

#Region "Radio button Event"

    Protected Sub rbtreporttype_SelectedIndexChanged(sender As Object, e As EventArgs)

        txtProject.Text = ""
        txtProjectReport.Text = ""
        HProjectId.Value = ""
        HReportProjectId.Value = ""
        ddlSUbject.DataSource() = Nothing
        ddlSUbject.DataBind()
        gvBasedOnParentVisit.DataSource = Nothing
        gvBasedOnParentVisit.DataBind()
        gvDeviation.DataSource = Nothing
        gvDeviation.DataBind()
        gvIndependentVisit.DataSource = Nothing
        gvIndependentVisit.DataBind()
        hdnSubSelection.Value = ""
        upreport.Update()
        If (rbtreporttype.SelectedIndex = 0) Then
            BasedOnParentVisit.Visible = True
            IndependentVisit.Visible = False
            trindependantvisit.Visible = False
            txtProject.Text = ""
        Else
            BasedOnParentVisit.Visible = False
            IndependentVisit.Visible = True
            trindependantvisit.Visible = True
            txtProjectReport.Text = ""
            If Not Activity() Then

            End If
        End If

    End Sub

    Protected Sub rbtSelection_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtSelectioncontent.SelectedIndexChanged
        Try
            'If Not resetpage() Then
            '    Exit Sub
            'End If
            txtProject.Text = ""
            txtProjectReport.Text = ""
            HProjectId.Value = ""
            HReportProjectId.Value = ""
            upreport.Update()
            rbtreporttype.SelectedIndex = 0
            ddlSUbject.DataSource = Nothing
            ddlSUbject.DataBind()
            ddlFromVisit.DataSource() = Nothing
            ddlFromVisit.DataBind()
            ddlToVisit.DataSource() = Nothing
            ddlToVisit.DataBind()
            upreport.Update()
            rbtreporttype_SelectedIndexChanged(sender, e)

            If Me.rbtSelectioncontent.SelectedValue.Trim() = "0" Then
                Me.divMainContent.Style.Add("Display", "")
                Me.divradioreportmain.Style.Add("Display", "none")
                'Me.HProjectId.Value = ""
                'Me.HReportProjectId.Value = ""
                gvBasedOnParentVisit.DataSource() = Nothing
                gvBasedOnParentVisit.DataBind()
                gvIndependentVisit.DataSource() = Nothing
                gvIndependentVisit.DataBind()
                upreport.Update()
                If Not FillGrid() Then
                    ShowErrorMessage("Error While FillGrid", "")
                    Exit Sub
                End If
                btnDelete.Style.Add("display", "none")
                btnSave.Style.Add("display", "none")
                btnExit.Style.Add("display", "none")
                btnCancel.Style.Add("display", "none")

            Else
                gvBasedOnParentVisit.DataSource() = Nothing
                gvBasedOnParentVisit.DataBind()
                gvIndependentVisit.DataSource() = Nothing
                gvIndependentVisit.DataBind()
                upreport.Update()

                Me.divMainContent.Style.Add("Display", "none")
                Me.divradioreportmain.Style.Add("Display", "")
                'Me.HProjectId.Value = ""
                'Me.HReportProjectId.Value = ""
                If Not FillGrid() Then
                    ShowErrorMessage("Error While FillGrid", "")
                    Exit Sub
                End If
                btnDelete.Style.Add("display", "none")
                btnSave.Style.Add("display", "none")
                btnExit.Style.Add("display", "none")
                btnCancel.Style.Add("display", "none")

            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub rbtAddEdit_SelectedIndexChanged(sender As Object, e As EventArgs)
        txtProject.Text = ""
        txtProjectReport.Text = ""
        HProjectId.Value = ""
        HReportProjectId.Value = ""
        ddlSUbject.Items.Clear()
        upreport.Update()

        gvDeviation.DataSource = Nothing
        gvDeviation.DataBind()
        upreport.Update()


        If (rbtAddEdit.SelectedIndex = 0) Then
            divAdd.Visible = True
            btnSave.Visible = True
            btnDelete.Style.Add("display", "none")
            If Not FillGrid() Then
                ShowErrorMessage("Error While FillGrid", "")
                Exit Sub
            End If
            btnDelete.Style.Add("display", "none")
            btnSave.Style.Add("display", "none")
            btnExit.Style.Add("display", "none")
            btnCancel.Style.Add("display", "none")
        Else
            divAdd.Visible = False
            btnSave.Visible = False
            btnDelete.Style.Add("display", "")
            If Not FillGrid() Then
                ShowErrorMessage("Error While FillGrid", "")
                Exit Sub
            End If
            btnDelete.Style.Add("display", "none")
            btnSave.Style.Add("display", "none")
            btnExit.Style.Add("display", "none")
            btnCancel.Style.Add("display", "none")
        End If
    End Sub

#End Region

#Region "Dropdown List Event"
    Protected Sub ddlParentActivity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlParentActivity.SelectedIndexChanged
        If Not FillChildActivity() Then
            Throw New Exception("Error While Filling Activity")
        End If
    End Sub
#End Region

#Region "Gridview Event"
    Protected Sub gvDeviation_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDeviation.RowDataBound

        Dim ds_Period As DataSet = Nothing
        Dim eStr As String = String.Empty
        Dim ds_Activity As DataSet = Nothing
        Dim ds_RefActivity As DataSet = Nothing
        Dim dt As DataSet
        dt = CType(ViewState("DS_DeviationReport"), DataSet)
        Try
            If (rbtAddEdit.SelectedIndex = 0) Then
                '' e.Row.Cells(GVC_Edit).Style.Add("display", "none")
                e.Row.Cells(GVC_Edit).Visible = False
            Else
                '' e.Row.Cells(GVC_Edit).Style.Add("display", "")
                e.Row.Cells(GVC_Edit).Visible = True
            End If
            upPeriod.Update()
            e.Row.Cells(GVC_WorkSpace).Style.Add("display", "none")
            e.Row.Cells(GVC_Status).Style.Add("display", "none")
            If e.Row.RowType = DataControlRowType.DataRow Then

                If (rbtAddEdit.SelectedIndex = 1 Or rbtAddEdit.SelectedIndex = 0) Then

                    CType(e.Row.FindControl("lnk"), LinkButton).CommandArgument = e.Row.RowIndex
                    CType(e.Row.FindControl("lnk"), LinkButton).CommandName = "MyValue"

                    upPeriod.Update()
                    Dim ds As DataSet

                    ds = CType(ViewState("DS_DeviationReport"), DataSet)

                    Dim row As Integer = e.Row.RowIndex
                    Dim col As Integer = CType(GVC_ParentActivityName, Integer)
                    Dim ParentActivityId As String = ds.Tables(0).Rows(e.Row.RowIndex)("iParentNodeId")
                    Dim ChildActivityId As String = ds.Tables(0).Rows(e.Row.RowIndex)("iChildNodeId")
                    Dim RefActivityId As String = ds.Tables(0).Rows(e.Row.RowIndex)("iRefNodeId")

                    Dim ddlgvParentActivity As DropDownList = DirectCast(e.Row.FindControl("ddlgvParentActivity"), DropDownList)

                    ds_Period = Me.objhelp.GetResultSet("select iNodeId,vNodeDisplayName from WorkspaceNodeDetail where vWorkspaceId='" + Me.HProjectId.Value + _
                                                          "' And iParentNodeId = 1 And cStatusIndi<>'D' ", "WorkspaceNodeDetail")

                    ds_Period.Tables(0).DefaultView.RowFilter = "iNodeId =" + ParentActivityId

                    ddlgvParentActivity.DataSource = ds_Period.Tables(0)
                    ddlgvParentActivity.DataValueField = "iNodeId"
                    ddlgvParentActivity.DataTextField = "vNodeDisplayName"
                    ddlgvParentActivity.DataBind()
                    'ddlgvParentActivity.SelectedValue = ds_Period.Tables(0).DefaultView.ToTable.Rows(0)("iNodeId")
                    ddlgvParentActivity.SelectedValue = ds_Period.Tables(0).DefaultView.ToTable.Rows(0)("vNodeDisplayName")
                    ddlgvParentActivity.SelectedIndex = ds_Period.Tables(0).DefaultView.ToTable.Rows(0)("iNodeId")
                    ddlgvParentActivity.ToolTip = ds_Period.Tables(0).DefaultView.ToTable.Rows(0)("vNodeDisplayName")

                    Dim ddlgvChildActivity As DropDownList = DirectCast(e.Row.FindControl("ddlgvChildActivity"), DropDownList)

                    If Me.ddlParentActivity.SelectedValue.ToString.Trim() <> "" Then
                        ds_Activity = Me.objhelp.GetResultSet("select distinct iNodeId,ActivityDisplayName,iParentNodeId,iNodeNo, " + _
                                                                " Convert(varchar,iNodeId) + '#' + vActivityId + '#' + ISNULL(Convert(varchar,nRefTime),'') As NodeAttributeID  " + _
                                                                " from VIEW_MedExWorkspaceHdr where vWorkspaceId='" + Me.HProjectId.Value + "' " + _
                                                                " And cStatusIndi<>'D' And iPeriod=1 And (iParentNodeID = " + ddlgvParentActivity.SelectedItem.Value + " Or iNodeId = " + ddlgvParentActivity.SelectedItem.Value + ")" + _
                                                                " order by iParentNodeId,iNodeNo ", "WorkspaceNodeDetail ")

                        'Dim nWorkSpaceId1 As Integer = CType(ds.Tables(0).Rows(e.Row.RowIndex)(0).ToString(), Integer)
                        'If (nWorkSpaceId1 <> 0) Then
                        ds_Activity.Tables(0).DefaultView.RowFilter = "iNodeId= " + ChildActivityId
                        'End If


                        ddlgvChildActivity.DataSource = ds_Activity.Tables(0)
                        ddlgvChildActivity.DataValueField = "iNodeId"
                        ddlgvChildActivity.DataTextField = "ActivityDisplayName"
                        ddlgvChildActivity.DataBind()
                        'ddlgvChildActivity.SelectedValue = ds_Period.Tables(0).DefaultView.ToTable.Rows(0)("iNodeId")

                        'If (nWorkSpaceId1 = 0) Then
                        '    ddlgvChildActivity.SelectedValue = ds_Activity.Tables(0).DefaultView.ToTable.Rows(0)("iNodeId")
                        '    'ddlgvChildActivity.SelectedIndex = ds_Activity.Tables(0).DefaultView.ToTable.Rows(0)("ActivityDisplayName")
                        'Else
                        '    ddlgvChildActivity.SelectedValue = ds_Activity.Tables(0).DefaultView.ToTable.Rows(0)("ActivityDisplayName")
                        '    ddlgvChildActivity.SelectedIndex = ds_Activity.Tables(0).DefaultView.ToTable.Rows(0)("iNodeId")
                        'End If


                        ddlgvChildActivity.SelectedValue = ds_Activity.Tables(0).DefaultView.ToTable.Rows(0)("ActivityDisplayName")
                        ddlgvChildActivity.SelectedIndex = ds_Activity.Tables(0).DefaultView.ToTable.Rows(0)("iNodeNo")
                        ddlgvChildActivity.ToolTip = ds_Activity.Tables(0).DefaultView.ToTable.Rows(0)("ActivityDisplayName")


                        Dim ddlgvRefeActivity As DropDownList = DirectCast(e.Row.FindControl("ddlgvRefeActivity"), DropDownList)


                        ds_RefActivity = Me.objhelp.GetResultSet("Select  iNodeId ,vActivityName ,ActivityWithParent From View_WorkSpaceNodeDetail where vWorkspaceId='" + Me.HProjectId.Value + _
                                                                        "' And iParentNodeId NOT IN (0,1) And cStatusIndi<>'D' ", "WorkspaceNodeDetail")


                        'Dim nWorkSpaceId12 As Integer = CType(ds.Tables(0).Rows(e.Row.RowIndex)(0).ToString(), Integer)
                        'If (nWorkSpaceId12 <> 0) Then
                        ds_RefActivity.Tables(0).DefaultView.RowFilter = "iNodeId= " + RefActivityId
                        'End If




                        ddlgvRefeActivity.DataSource = ds_RefActivity.Tables(0)
                        ddlgvRefeActivity.DataValueField = "iNodeId"
                        ddlgvRefeActivity.DataTextField = "ActivityWithParent"
                        ddlgvRefeActivity.DataBind()
                        'If (nWorkSpaceId12 = 0) Then
                        ddlgvRefeActivity.SelectedValue = ds_RefActivity.Tables(0).Rows(0)("iNodeId")
                        'ddlgvRefeActivity.SelectedIndex = ds_RefActivity.Tables(0).Rows(0)("ActivityWithParent")
                        'Else
                        ddlgvRefeActivity.SelectedValue = ds_RefActivity.Tables(0).DefaultView.ToTable.Rows(0)("ActivityWithParent")
                        ddlgvRefeActivity.SelectedIndex = ds_RefActivity.Tables(0).DefaultView.ToTable.Rows(0)("iNodeId")
                        ddlgvRefeActivity.ToolTip = ds_RefActivity.Tables(0).DefaultView.ToTable.Rows(0)("ActivityWithParent")
                    End If


                End If
                'Dim nWorkSpaceId As Integer = CType(ds.Tables(0).Rows(e.Row.RowIndex)(0).ToString(), Integer)
                'If (nWorkSpaceId <> 0) Then
                e.Row.Cells(GVC_Project).Enabled = False
                e.Row.Cells(GVC_ParentActivityName).Enabled = False
                e.Row.Cells(GVC_ChileActivity).Enabled = False
                e.Row.Cells(GVC_RefActivity).Enabled = False
                e.Row.Cells(GVC_DeviationNegative).Enabled = False
                e.Row.Cells(GVC_DeviationPositive).Enabled = False
                e.Row.Cells(GVC_WindowPeriod).Enabled = False
                e.Row.Cells(GVC_Update).Enabled = False
                'e.Row.Cells(10).Visible = False
                'e.Row.Cells(10).Style.Add("display", "none")
                btnSave.Style.Add("display", "")
                btnDelete.Style.Add("display", "none")
                btnCancel.Style.Add("display", "")
                btnExit.Style.Add("display", "")
                'End If
                'e.Row.Cells(GVC_Update).Enabled = False
                'e.Row.Cells(GVC_Project).Enabled = False
                'e.Row.Cells(GVC_ParentActivityName).Enabled = False
                upGvDeviation.Update()
                upPeriod.Update()
            Else
                Dim row As Integer = e.Row.RowIndex
                Dim col As Integer = CType(GVC_ParentActivityName, Integer)
                Dim ParentActivityId As String = dt.Tables(0).Rows(row)(col).ToString()
                Dim ChildActivityId As String = dt.Tables(0).Rows(row)(CType(GVC_ChileActivity, Integer)).ToString()
                Dim RefActivityId As String = dt.Tables(0).Rows(row)(CType(GVC_RefActivity, Integer)).ToString()

                Dim ddlgvParentActivity As DropDownList = DirectCast(e.Row.FindControl("ddlgvParentActivity"), DropDownList)

                ds_Period = Me.objhelp.GetResultSet("select iNodeId,vNodeDisplayName from WorkspaceNodeDetail where vWorkspaceId='" + Me.HProjectId.Value + _
                                                      "' And iParentNodeId = 1 And cStatusIndi<>'D' ", "WorkspaceNodeDetail")
                'Me.ddlgvParentActivity.DataSource = ds_Period.Tables(0)
                'Me.ddlgvParentActivity.DataValueField = "iNodeId"
                'Me.ddlgvParentActivity.DataTextField = "vNodeDisplayName"
                'Me.ddlgvParentActivity.DataBind()

                'Dim Parent As String = CType(e.Row.FindControl("lblParentActivity"), Label).Text

                ds_Period.Tables(0).DefaultView.RowFilter = "iNodeId =" + ParentActivityId

                ddlgvParentActivity.DataSource = ds_Period.Tables(0)
                ddlgvParentActivity.DataValueField = "iNodeId"
                ddlgvParentActivity.DataTextField = "vNodeDisplayName"
                ddlgvParentActivity.DataBind()
                'ddlgvParentActivity.SelectedValue = ds_Period.Tables(0).DefaultView.ToTable.Rows(0)("iNodeId")
                ddlgvParentActivity.SelectedValue = ds_Period.Tables(0).DefaultView.ToTable.Rows(0)("vNodeDisplayName")
                ddlgvParentActivity.SelectedIndex = ds_Period.Tables(0).DefaultView.ToTable.Rows(0)("iNodeId")


                Dim ddlgvChildActivity As DropDownList = DirectCast(e.Row.FindControl("ddlgvChildActivity"), DropDownList)

                If Me.ddlParentActivity.SelectedValue.ToString.Trim() <> "" Then
                    ds_Activity = Me.objhelp.GetResultSet("select distinct iNodeId,ActivityDisplayName,iParentNodeId,iNodeNo, " + _
                                                            " Convert(varchar,iNodeId) + '#' + vActivityId + '#' + ISNULL(Convert(varchar,nRefTime),'') As NodeAttributeID  " + _
                                                            " from VIEW_MedExWorkspaceHdr where vWorkspaceId='" + Me.HProjectId.Value + "' " + _
                                                            " And cStatusIndi<>'D' And iPeriod=1 And (iParentNodeID = " + ddlgvParentActivity.SelectedItem.Value + " Or iNodeId = " + ddlgvParentActivity.SelectedItem.Value + ")" + _
                                                            " order by iParentNodeId,iNodeNo ", "WorkspaceNodeDetail ")

                    ds_Activity.Tables(0).DefaultView.RowFilter = "iNodeId= " + ChildActivityId

                    ddlgvChildActivity.DataSource = ds_Activity.Tables(0)
                    ddlgvChildActivity.DataValueField = "iNodeId"
                    ddlgvChildActivity.DataTextField = "ActivityDisplayName"
                    ddlgvChildActivity.DataBind()
                    'ddlgvChildActivity.SelectedValue = ds_Period.Tables(0).DefaultView.ToTable.Rows(0)("iNodeId")
                    ddlgvChildActivity.SelectedValue = ds_Activity.Tables(0).DefaultView.ToTable.Rows(0)("ActivityDisplayName")
                    ddlgvChildActivity.SelectedIndex = ds_Activity.Tables(0).DefaultView.ToTable.Rows(0)("iNodeNo")



                    Dim ddlgvRefeActivity As DropDownList = DirectCast(e.Row.FindControl("ddlgvRefeActivity"), DropDownList)


                    ds_RefActivity = Me.objhelp.GetResultSet("Select  iNodeId ,vActivityName ,ActivityWithParent From View_WorkSpaceNodeDetail where vWorkspaceId='" + Me.HProjectId.Value + _
                                                                    "' And iParentNodeId NOT IN (0,1) And cStatusIndi<>'D' ", "WorkspaceNodeDetail")


                    ds_RefActivity.Tables(0).DefaultView.RowFilter = "iNodeId= " + RefActivityId

                    ddlgvRefeActivity.DataSource = ds_RefActivity.Tables(0)
                    ddlgvRefeActivity.DataValueField = "iNodeId"
                    ddlgvRefeActivity.DataTextField = "ActivityWithParent"
                    ddlgvRefeActivity.DataBind()

                    ddlgvRefeActivity.SelectedValue = ds_RefActivity.Tables(0).DefaultView.ToTable.Rows(0)("ActivityWithParent")
                    ddlgvRefeActivity.SelectedIndex = ds_RefActivity.Tables(0).DefaultView.ToTable.Rows(0)("iNodeId")




                    'e.Row.Cells(GVC_Project).Style.Add("disabled", "disabled")
                    'e.Row.Cells(GVC_ParentActivityName).Style.Add("disabled", "disabled")
                    'e.Row.Cells(GVC_ChileActivity).Style.Add("disabled", "disabled")
                    'e.Row.Cells(GVC_RefActivity).Style.Add("disabled", "disabled")
                    'e.Row.Cells(GVC_DeviationNegative).Style.Add("disabled", "disabled")
                    'e.Row.Cells(GVC_DeviationPositive).Style.Add("disabled", "disabled")
                    'e.Row.Cells(GVC_WindowPeriod).Style.Add("disabled", "disabled")
                    'e.Row.Cells(GVC_Edit).Style.Add("display", "none")
                    e.Row.Cells(GVC_Edit).Visible = True
                    e.Row.Cells(GVC_Project).Enabled = False
                    e.Row.Cells(GVC_ParentActivityName).Enabled = False
                    e.Row.Cells(GVC_ChileActivity).Enabled = False
                    e.Row.Cells(GVC_RefActivity).Enabled = False
                    e.Row.Cells(GVC_DeviationNegative).Enabled = False
                    e.Row.Cells(GVC_DeviationPositive).Enabled = False
                    e.Row.Cells(GVC_WindowPeriod).Enabled = False
                    btnSave.Style.Add("display", "")
                    btnDelete.Style.Add("display", "none")
                    btnCancel.Style.Add("display", "")
                    btnExit.Style.Add("display", "")
                    upGvDeviation.Update()
                    upPeriod.Update()
                End If
                e.Row.Cells(GVC_Update).Enabled = False
                e.Row.Cells(GVC_Project).Enabled = False
                e.Row.Cells(GVC_ParentActivityName).Enabled = False

            End If
            'End If


        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvDeviation_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvDeviation.RowCommand
        Try
            If e.CommandName.ToUpper = "MYVALUE" Then

                Dim Val As String = String.Empty
                Dim wStr As String = String.Empty
                Dim DS_DeviationReport As DataSet = CType(ViewState("DS_DeviationReport"), DataSet)


                'Dim ddlgvParentActivity As DropDownList = DirectCast(e..   FindControl("ddlgvParentActivity"), DropDownList)

                upPeriod.Update()

                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gvDeviation.Rows(index)

                'Dim id As Integer = Me.gvDeviation.Rows(e.CommandArgument).Cells(10).Text.Trim()

                Dim ddlgvChildActivity As DropDownList = DirectCast(gvRow.FindControl("ddlgvChildActivity"), DropDownList)
                Dim ddlgvRefeActivity As DropDownList = DirectCast(gvRow.FindControl("ddlgvRefeActivity"), DropDownList)
                Dim neg As TextBox = CType(gvRow.FindControl("txtDeviationNegative"), TextBox)
                Dim pos As TextBox = CType(gvRow.FindControl("txtDeviationPositive"), TextBox)

                Dim child As Integer = DirectCast(gvRow.FindControl("ddlgvChildActivity"), DropDownList).SelectedValue
                Dim Ref As Integer = DirectCast(gvRow.FindControl("ddlgvRefeActivity"), DropDownList).SelectedValue
                Dim devNegh As Integer = CType(gvRow.FindControl("txtDeviationNegative"), TextBox).Text
                Dim devPosit As Integer = CType(gvRow.FindControl("txtDeviationPositive"), TextBox).Text
                Dim win As Integer = CType(gvRow.FindControl("txtWindowPeriod"), TextBox).Text
                Dim Id1 = gvRow.Cells(GVC_WorkSpace).Text
                Dim dt As DataTable

                DS_DeviationReport.Tables(0).DefaultView.RowFilter = "nWorkSpaceDeviationId=" + Id1

                dt = DS_DeviationReport.Tables(0).DefaultView.ToTable()

                For Each dr As DataRow In dt.Rows
                    dr("iChildNodeId") = child
                    dr("iRefNodeId") = Ref
                    dr("iDeviationNegative") = devNegh
                    dr("iDeviationPositive") = devPosit
                    dr("iWindowPeriod") = win
                Next
                dt.AcceptChanges()
                ViewState("Edit_Table") = dt
                btnSaveRemarks.Text = "Update"
                ModalRemarks.Show()
                btnSaveRemarks.Text = "Update"

            End If

        Catch ex As Exception

        End Try

    End Sub

    Protected Sub gvBasedOnParentVisit_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBasedOnParentVisit.RowDataBound
        Try
            Dim ds As DataSet
            ds = CType(ViewState("DeviationPeriod"), DataSet)

            If e.Row.RowType = DataControlRowType.DataRow Then
                For cIndex As Integer = 3 To e.Row.Cells.Count - 1
                    If (CType(e.Row.Cells(cIndex).Text.Trim.ToUpper().Split("(")(0), Integer) <> 0) Then
                        Dim HesderText = Server.HtmlDecode(gvBasedOnParentVisit.HeaderRow.Cells(cIndex).Text.ToString())
                        Try
                            ds.Tables(0).DefaultView.RowFilter = "vNodeDisplayName ='" + HesderText.Trim().ToString() + "'"
                            If (ds.Tables(0).DefaultView.ToTable.Rows.Count > 0) Then
                                'If e.Row.Cells(5).Text.Trim.ToUpper() <> "" Then
                                '    If (e.Row.Cells(cIndex).Text.Trim.ToUpper() <= ds.Tables(0).DefaultView.ToTable().Rows(0)("Negative")) Then
                                '        e.Row.Cells(cIndex).ForeColor = System.Drawing.Color.Red
                                '    ElseIf (e.Row.Cells(cIndex).Text.Trim.ToUpper() >= ds.Tables(0).DefaultView.ToTable().Rows(0)("Positive")) Then
                                '        e.Row.Cells(cIndex).ForeColor = System.Drawing.Color.Green
                                '    ElseIf (e.Row.Cells(cIndex).Text.Trim.ToUpper() <= ds.Tables(0).DefaultView.ToTable().Rows(0)("Positive") AndAlso e.Row.Cells(cIndex).Text.Trim.ToUpper() >= ds.Tables(0).DefaultView.ToTable().Rows(0)("Negative")) Then
                                '        e.Row.Cells(cIndex).ForeColor = ColorTranslator.FromHtml("#593C1F")
                                '    End If
                                'End If

                                If e.Row.Cells(5).Text.Trim.ToUpper() <> "" Then
                                    If (CType(e.Row.Cells(cIndex).Text.Trim.ToUpper().Split("(")(0), Integer) > 0) Then
                                        If (CType(e.Row.Cells(cIndex).Text.Trim.ToUpper().Split("(")(0), Integer) <= CType(ds.Tables(0).DefaultView.ToTable().Rows(0)("Positive"), Integer) AndAlso CType(e.Row.Cells(cIndex).Text.Trim.ToUpper().Split("(")(0), Integer) >= CType(ds.Tables(0).DefaultView.ToTable().Rows(0)("Negative"), Integer)) Then
                                            e.Row.Cells(cIndex).ForeColor = System.Drawing.Color.Green
                                        ElseIf (CType(e.Row.Cells(cIndex).Text.Trim.ToUpper().Split("(")(0), Integer) >= CType(ds.Tables(0).DefaultView.ToTable().Rows(0)("Positive"), Integer) AndAlso CType(e.Row.Cells(cIndex).Text.Trim.ToUpper().Split("(")(0), Integer) >= CType(ds.Tables(0).DefaultView.ToTable().Rows(0)("Negative"), Integer)) Then
                                            e.Row.Cells(cIndex).ForeColor = System.Drawing.Color.Red
                                        ElseIf (CType(e.Row.Cells(cIndex).Text.Trim.ToUpper().Split("(")(0), Integer) <= CType(ds.Tables(0).DefaultView.ToTable().Rows(0)("Positive"), Integer) AndAlso CType(e.Row.Cells(cIndex).Text.Trim.ToUpper().Split("(")(0), Integer) <= CType(ds.Tables(0).DefaultView.ToTable().Rows(0)("Negative"), Integer)) Then
                                            e.Row.Cells(cIndex).ForeColor = System.Drawing.Color.Red
                                        ElseIf (e.Row.Cells(cIndex).Text.Trim.ToUpper().Split("(")(0) = 0) Then
                                            e.Row.Cells(cIndex).ForeColor = System.Drawing.Color.Gray
                                        End If
                                    Else

                                        If (CType(e.Row.Cells(cIndex).Text.Trim.ToUpper().Split("(")(0), Integer) <= CType(ds.Tables(0).DefaultView.ToTable().Rows(0)("Positive"), Integer) AndAlso CType(e.Row.Cells(cIndex).Text.Trim.ToUpper().Split("(")(0), Integer) >= CType(ds.Tables(0).DefaultView.ToTable().Rows(0)("Negative"), Integer)) Then
                                            e.Row.Cells(cIndex).ForeColor = System.Drawing.Color.Green
                                        ElseIf (CType(e.Row.Cells(cIndex).Text.Trim.ToUpper().Split("(")(0), Integer) <= CType(ds.Tables(0).DefaultView.ToTable().Rows(0)("Positive"), Integer) AndAlso CType(e.Row.Cells(cIndex).Text.Trim.ToUpper().Split("(")(0), Integer) <= CType(ds.Tables(0).DefaultView.ToTable().Rows(0)("Negative"), Integer)) Then
                                            e.Row.Cells(cIndex).ForeColor = System.Drawing.Color.Red
                                        ElseIf (CType(e.Row.Cells(cIndex).Text.Trim.ToUpper().Split("(")(0), Integer) = 0) Then
                                            e.Row.Cells(cIndex).ForeColor = System.Drawing.Color.Gray
                                        End If
                                    End If

                                End If



                                'e.Row.Cells(cIndex).ForeColor = System.Drawing.Color.Red
                            End If
                        Catch ex As Exception
                        End Try
                    Else
                        e.Row.Cells(cIndex).ForeColor = System.Drawing.Color.Gray
                        'e.Row.Cells(cIndex).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    End If
                Next
            End If

        Catch ex As Exception

        End Try

    End Sub




    Protected Sub gvIndependentVisit_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvIndependentVisit.RowDataBound
        Try
            Dim ds As DataSet
            ds = CType(ViewState("DeviationPeriod"), DataSet)

            ds.Tables(0).DefaultView.RowFilter = "IChildNodeId= " + ddlToVisit.SelectedValue + "AND iRefNodeId = " + ddlFromVisit.SelectedValue
            ds.Tables(0).AcceptChanges()

            If e.Row.RowType = DataControlRowType.DataRow Then
                If (CType(ds.Tables(0).DefaultView.ToTable().Rows.Count, Integer) <> 0) Then
                    For cIndex As Integer = 3 To e.Row.Cells.Count - 1
                        If e.Row.Cells(5).Text.Trim.ToUpper() <> "" Then
                            'If (e.Row.Cells(5).Text.Trim.ToUpper() <= ds.Tables(0).DefaultView.ToTable().Rows(0)("Negative")) Then
                            '    e.Row.Cells(5).ForeColor = System.Drawing.Color.Red
                            'ElseIf (e.Row.Cells(5).Text.Trim.ToUpper() >= ds.Tables(0).DefaultView.ToTable().Rows(0)("Positive")) Then
                            '    e.Row.Cells(5).ForeColor = System.Drawing.Color.Yellow
                            'ElseIf (e.Row.Cells(5).Text.Trim.ToUpper() <= ds.Tables(0).DefaultView.ToTable().Rows(0)("Positive") AndAlso e.Row.Cells(5).Text.Trim.ToUpper() >= ds.Tables(0).DefaultView.ToTable().Rows(0)("Negative")) Then
                            '    e.Row.Cells(5).ForeColor = ColorTranslator.FromHtml("#593C1F")
                            'End If

                            If e.Row.Cells(5).Text.Trim.ToUpper() <> "" Then
                                If (CType(e.Row.Cells(5).Text.Trim.ToUpper().Split("(")(0), Integer) > 0) Then
                                    If (CType(e.Row.Cells(5).Text.Trim.ToUpper().Split("(")(0), Integer) <= CType(ds.Tables(0).DefaultView.ToTable().Rows(0)("Positive"), Integer) AndAlso CType(e.Row.Cells(5).Text.Trim.ToUpper().Split("(")(0), Integer) >= CType(ds.Tables(0).DefaultView.ToTable().Rows(0)("Negative"), Integer)) Then
                                        e.Row.Cells(5).ForeColor = System.Drawing.Color.Green
                                    ElseIf (CType(e.Row.Cells(5).Text.Trim.ToUpper().Split("(")(0), Integer) >= CType(ds.Tables(0).DefaultView.ToTable().Rows(0)("Positive"), Integer) AndAlso CType(e.Row.Cells(5).Text.Trim.ToUpper().Split("(")(0), Integer) >= CType(ds.Tables(0).DefaultView.ToTable().Rows(0)("Negative"), Integer)) Then
                                        e.Row.Cells(5).ForeColor = System.Drawing.Color.Red
                                    ElseIf (CType(e.Row.Cells(5).Text.Trim.ToUpper().Split("(")(0), Integer) <= CType(ds.Tables(0).DefaultView.ToTable().Rows(0)("Positive"), Integer) AndAlso CType(e.Row.Cells(5).Text.Trim.ToUpper().Split("(")(0), Integer) <= CType(ds.Tables(0).DefaultView.ToTable().Rows(0)("Negative"), Integer)) Then
                                        e.Row.Cells(5).ForeColor = System.Drawing.Color.Red
                                    ElseIf (CType(e.Row.Cells(5).Text.Trim.ToUpper().Split("(")(0), Integer) = 0) Then
                                        e.Row.Cells(cIndex).ForeColor = System.Drawing.Color.Gray
                                    End If

                                Else

                                    If (CType(e.Row.Cells(5).Text.Trim.ToUpper(), Integer) <= CType(ds.Tables(0).DefaultView.ToTable().Rows(0)("Positive"), Integer) AndAlso CType(e.Row.Cells(5).Text.Trim.ToUpper(), Integer) >= CType(ds.Tables(0).DefaultView.ToTable().Rows(0)("Negative"), Integer)) Then
                                        e.Row.Cells(5).ForeColor = System.Drawing.Color.Green
                                    ElseIf (CType(e.Row.Cells(5).Text.Trim.ToUpper(), Integer) <= CType(ds.Tables(0).DefaultView.ToTable().Rows(0)("Positive"), Integer) AndAlso CType(e.Row.Cells(5).Text.Trim.ToUpper(), Integer) <= CType(ds.Tables(0).DefaultView.ToTable().Rows(0)("Negative"), Integer)) Then
                                        e.Row.Cells(5).ForeColor = System.Drawing.Color.Red
                                    ElseIf (CType(e.Row.Cells(5).Text.Trim.ToUpper(), Integer) = 0) Then
                                        e.Row.Cells(5).ForeColor = System.Drawing.Color.Gray
                                    End If
                                End If

                            End If

                        End If

                    Next

                End If
            End If

        Catch ex As Exception

        End Try

    End Sub


#End Region

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR>" + eStr
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR>" + eStr)
    End Sub

#End Region

#Region "Helper function"

    Public Function resetpage() As Boolean
        Try

            Me.HProjectId.Value = ""
            Me.txtProject.Text = ""
            Me.txtProjectReport.Text = ""
            Me.HReportProjectId.Value = ""
            Me.ViewState.Clear()
            GenCall()
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...resetpage()")
            Return False
        End Try

    End Function

#End Region

#Region "Fill DropDown"
    Function FillParentActivity() As Boolean
        Dim ds_Period As DataSet = Nothing
        Dim eStr As String = String.Empty
        Dim ds_Activity As DataSet = Nothing
        Try

            If CType(Me.Session(S_ScopeNo), Integer) = Scope_ClinicalTrial Then
                Me.ddlParentActivity.Items.Insert(0, "1")
                ds_Period = Me.objhelp.GetResultSet("select iNodeId,vNodeDisplayName from WorkspaceNodeDetail where vWorkspaceId='" + Me.HProjectId.Value + _
                                                      "' And iParentNodeId = 1 And cStatusIndi<>'D' AND ( vTemplateId <> '0001' OR  vTemplateId IS NULl)   ", "WorkspaceNodeDetail")
                Me.ddlParentActivity.DataSource = ds_Period.Tables(0)
                Me.ddlParentActivity.DataValueField = "iNodeId"
                Me.ddlParentActivity.DataTextField = "vNodeDisplayName"
                Me.ddlParentActivity.DataBind()
                Me.ddlParentActivity.Items.Insert(0, "Select Visit/Parent Activity")

            ElseIf Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                Me.ddlParentActivity.Items.Insert(0, "1")
                ds_Period = Me.objhelp.GetResultSet("select iNodeId,vNodeDisplayName from WorkspaceNodeDetail where vWorkspaceId='" + Me.HProjectId.Value + _
                                                      "' And iParentNodeId = 1 And cStatusIndi<>'D'   AND ( vTemplateId <> '0001' OR  vTemplateId IS NULl)  ", "WorkspaceNodeDetail")
                Me.ddlParentActivity.DataSource = ds_Period.Tables(0)
                Me.ddlParentActivity.DataValueField = "iNodeId"
                Me.ddlParentActivity.DataTextField = "vNodeDisplayName"
                Me.ddlParentActivity.DataBind()
                Me.ddlParentActivity.Items.Insert(0, "Select Visit/Parent Activity")
            End If



            upPeriod.Update()
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
            Return False
        End Try



    End Function

    Function FillChildActivity() As Boolean
        Dim ds_Activity As DataSet = Nothing
        Dim eStr As String = String.Empty
        Try

            'If CType(Me.Session(S_ScopeNo), Integer) = Scope_ClinicalTrial Then
            If Me.ddlParentActivity.SelectedValue.ToString.Trim() <> "" Then
                ds_Activity = Me.objhelp.GetResultSet("select distinct iNodeId,ActivityDisplayName,iParentNodeId,iNodeNo, ActivityDisplayNameWithOutPeriod ," + _
                                                        " Convert(varchar,iNodeId) + '#' + vActivityId + '#' + ISNULL(Convert(varchar,nRefTime),'') As NodeAttributeID  " + _
                                                        " from VIEW_MedExWorkspaceHdr where vWorkspaceId='" + Me.HProjectId.Value + "' " + _
                                                        " And cStatusIndi<>'D' And iPeriod=1 And (iParentNodeID = " + Me.ddlParentActivity.SelectedItem.Value + " Or iNodeId = " + Me.ddlParentActivity.SelectedItem.Value + ")" + _
                                                        " order by iParentNodeId,iNodeNo ", "WorkspaceNodeDetail ")
                ddlChildActivity.DataSource = ds_Activity.Tables(0)
                ddlChildActivity.DataValueField = "iNodeId"
                ddlChildActivity.DataTextField = "ActivityDisplayNameWithOutPeriod"
                ddlChildActivity.DataBind()
                ddlChildActivity.Items.Insert(0, "Select Child Activity")
                For i As Integer = 1 To ddlChildActivity.Items.Count - 1
                    'ddlChildActivity.Items(i).Attributes.Add("title", ddlChildActivity.Items(i).Text + "-(" + ddlChildActivity.Items(i).Value.Split("#")(1).ToString() + ")")
                Next

            End If


            ds_Activity = Me.objhelp.GetResultSet("Select  iNodeId ,vActivityName ,ActivityWithParent From View_WorkSpaceNodeDetail where vWorkspaceId='" + Me.HProjectId.Value + _
                                                                    "' And iParentNodeId not in (0,1) And cStatusIndi<>'D' ", "WorkspaceNodeDetail")
            ddlReferanceActivity.DataSource = ds_Activity.Tables(0)
            Me.ddlReferanceActivity.DataValueField = "iNodeId"
            Me.ddlReferanceActivity.DataTextField = "ActivityWithParent"
            ddlReferanceActivity.DataBind()
            ddlReferanceActivity.Items.Insert(0, "Select Reference Activity")
            For i As Integer = 1 To ddlChildActivity.Items.Count - 1
                'ddlReferanceActivity.Items(i).Attributes.Add("title", ddlChildActivity.Items(i).Text + "-(" + ddlChildActivity.Items(i).Value.Split("#")(1).ToString() + ")")
            Next

            'End If



            Return True
        Catch ex As Exception
            Return False
        Finally
            txtNegative.Text = ""
            txtPositive.Text = ""
            txtActual.Text = ""
        End Try
    End Function

    Function FillReferanceActivity() As Boolean

    End Function

    Function Activity() As Boolean
        Dim ds_Activity As DataSet = Nothing
        Dim eStr As String = String.Empty
        Try
            ds_Activity = Me.objhelp.GetResultSet("select distinct iNodeId,ActivityDisplayName, ActivityDisplayNameWithOutPeriod, iParentNodeId,iNodeNo, " + _
                                                        " Convert(varchar,iNodeId) + '#' + vActivityId + '#' + ISNULL(Convert(varchar,nRefTime),'') As NodeAttributeID  " + _
                                                        " from VIEW_MedExWorkspaceHdr where vWorkspaceId='" + Me.HReportProjectId.Value + "' " + _
                                                        " And cStatusIndi<>'D' And iPeriod=1 And (iParentNodeID <> 1)" + _
                                                        " order by iParentNodeId,iNodeNo ", "WorkspaceNodeDetail ")
            ddlFromVisit.DataSource = ds_Activity.Tables(0)
            ddlFromVisit.DataValueField = "iNodeId"
            ddlFromVisit.DataTextField = "ActivityDisplayNameWithOutPeriod"
            ddlFromVisit.DataBind()
            ddlFromVisit.Items.Insert(0, "Select From  Activity")

            ddlToVisit.DataSource = ds_Activity.Tables(0)
            ddlToVisit.DataValueField = "iNodeId"
            ddlToVisit.DataTextField = "ActivityDisplayNameWithOutPeriod"
            ddlToVisit.DataBind()
            ddlToVisit.Items.Insert(0, "Select TO  Activity")

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
#End Region

#Region "FillGrid"
    Function FillGrid() As Boolean

        Dim dt_WorkSpaceDeviationReport As DataSet
        Dim Wstr As String
        If (rbtSelectioncontent.SelectedIndex = 0) Then
            Wstr = Me.HProjectId.Value
        Else
            Wstr = Me.HReportProjectId.Value
        End If

        Try
            If (Wstr <> "") Then

                Dim ds_DeviationPeriod As DataSet
                If Not objhelp.Proc_GetWorkSpaceDeviationReport(Wstr, dt_WorkSpaceDeviationReport, eStr_Retu) Then
                    Throw New Exception(eStr_Retu)
                End If



                'dt_WorkSpaceDeviationReport.Tables(0).TableName = dt_WorkSpaceDeviationReport.Tables(0).TableName("WorkSpaceDeviationReport")

                ViewState(VS_WorkSpaceDeviationReport) = dt_WorkSpaceDeviationReport
                ViewState("DS_DeviationReport") = dt_WorkSpaceDeviationReport


                gvDeviation.DataSource() = Nothing
                gvDeviation.DataBind()
                upGvDeviation.Update()
            End If


            'ViewState("DS_DeviationReport") = dt_WorkSpaceDeviationReport.Tables(0)

            If Not (dt_WorkSpaceDeviationReport Is Nothing) Then
                If (dt_WorkSpaceDeviationReport.Tables(0).Rows.Count > 0) Then
                    gvDeviation.DataSource() = dt_WorkSpaceDeviationReport.Tables(0)
                    gvDeviation.DataBind()
                    upPeriod.Update()
                    upGvDeviation.Update()
                    If Me.rbtSelectioncontent.SelectedValue.Trim() = "0" Then
                        Me.divMainContent.Style.Add("Display", "")
                        Me.divradioreportmain.Style.Add("Display", "none")
                    Else
                        Me.divMainContent.Style.Add("Display", "none")
                        Me.divradioreportmain.Style.Add("Display", "")

                    End If
                Else
                    Me.btnSave.Style.Add("Display", "none")
                    Me.btnCancel.Style.Add("Display", "none")
                    Me.btnExit.Style.Add("Display", "none")
                    Me.btnDelete.Style.Add("Display", "none")
                    ''ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "nodata", " alert('There is no data.');", True)
                    Return True
                    Exit Function

                End If
            End If

            If (rbtAddEdit.SelectedIndex = 0) Then
                btnDelete.Style.Add("display", "none")
            Else
                btnDelete.Style.Add("display", "")
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function
#End Region

    Protected Sub chkEdit_CheckedChanged(sender As Object, e As EventArgs)

        Dim ds_Period As DataSet = Nothing
        Dim eStr As String = String.Empty
        Dim ds_Activity As DataSet = Nothing
        Dim ds_RefActivity As DataSet = Nothing
        Dim dt As DataSet

        Dim gvRow As GridViewRow = CType(CType(sender, Control).Parent.Parent,  _
                                        GridViewRow)
        Dim index As Integer = gvRow.RowIndex
        Dim chk As CheckBox = CType(sender, CheckBox)
        If (chk.Checked = True) Then
            gvRow.Cells(GVC_ChileActivity).Enabled = True
            gvRow.Cells(GVC_RefActivity).Enabled = True
            gvRow.Cells(GVC_WindowPeriod).Enabled = True
            gvRow.Cells(GVC_DeviationNegative).Enabled = True
            gvRow.Cells(GVC_DeviationPositive).Enabled = True
            gvRow.Cells(GVC_Update).Enabled = True

            Dim ddlgvParentActivity As DropDownList = DirectCast(gvRow.FindControl("ddlgvParentActivity"), DropDownList)
            Dim ddlgvChildActivity As DropDownList = DirectCast(gvRow.FindControl("ddlgvChildActivity"), DropDownList)
            Dim ddlgvRefeActivity As DropDownList = DirectCast(gvRow.FindControl("ddlgvRefeActivity"), DropDownList)

            Dim Selectchild = ddlgvChildActivity.SelectedValue
            Dim SelectRef = DirectCast(gvRow.FindControl("ddlgvRefeActivity"), DropDownList).SelectedValue


            If Me.ddlParentActivity.SelectedValue.ToString.Trim() <> "" Then
                ds_Activity = Me.objhelp.GetResultSet("select distinct iNodeId,ActivityDisplayName,iParentNodeId,iNodeNo, " + _
                                                        " Convert(varchar,iNodeId) + '#' + vActivityId + '#' + ISNULL(Convert(varchar,nRefTime),'') As NodeAttributeID  " + _
                                                        " from VIEW_MedExWorkspaceHdr where vWorkspaceId='" + Me.HProjectId.Value + "' " + _
                                                        " And cStatusIndi<>'D' And iPeriod=1 And (iParentNodeID = " + ddlgvParentActivity.SelectedItem.Value + " Or iNodeId = " + ddlgvParentActivity.SelectedItem.Value + ")" + _
                                                        " order by iParentNodeId,iNodeNo ", "WorkspaceNodeDetail ")



                ddlgvChildActivity.DataSource = Nothing
                ddlgvChildActivity.DataSource = ds_Activity.Tables(0)
                ddlgvChildActivity.DataValueField = "iNodeId"
                ddlgvChildActivity.DataTextField = "ActivityDisplayName"
                ddlgvChildActivity.DataBind()

                For i As Integer = 0 To ds_Activity.Tables(0).Rows.Count - 1
                    ddlgvChildActivity.ToolTip = ds_Activity.Tables(0).DefaultView.ToTable.Rows(i)("ActivityDisplayName")
                Next




                'ddlgvChildActivity.SelectedValue = ds_Period.Tables(0).DefaultView.ToTable.Rows(0)("iNodeId")

                'ds_Activity.Tables(0).DefaultView.RowFilter = "iNodeId= " + ddlgvChildActivity.SelectedItem.Value

                ddlgvChildActivity.SelectedValue = Selectchild
                ''ddlgvChildActivity.SelectedIndex = ds_Activity.Tables(0).DefaultView.ToTable.Rows(0)("iNodeNo")


                'Dim ddlgvRefeActivity As DropDownList = DirectCast(gvRow.FindControl("ddlgvRefeActivity"), DropDownList)


                ds_RefActivity = Me.objhelp.GetResultSet("Select  iNodeId ,vActivityName ,ActivityWithParent From View_WorkSpaceNodeDetail where vWorkspaceId='" + Me.HProjectId.Value + _
                                                                "' And iParentNodeId NOT IN (0,1) And cStatusIndi<>'D' ", "WorkspaceNodeDetail")




                ddlgvRefeActivity.DataSource = Nothing
                ddlgvRefeActivity.DataSource = ds_RefActivity.Tables(0)
                ddlgvRefeActivity.DataValueField = "iNodeId"
                ddlgvRefeActivity.DataTextField = "ActivityWithParent"
                ddlgvRefeActivity.DataBind()


                For i As Integer = 0 To ds_Activity.Tables(0).Rows.Count - 1
                    ddlgvRefeActivity.ToolTip = ds_RefActivity.Tables(0).DefaultView.ToTable.Rows(i)("ActivityWithParent")
                Next

                'ds_RefActivity.Tables(0).DefaultView.RowFilter = "iNodeId= " + ddlgvRefeActivity.SelectedItem.Value
                ddlgvRefeActivity.SelectedValue = SelectRef
                'ddlgvRefeActivity.SelectedIndex = ds_RefActivity.Tables(0).DefaultView.ToTable.Rows(0)("iNodeId")

            End If
        Else
            If Not FillGrid() Then

            End If
            gvRow.Cells(GVC_ChileActivity).Enabled = False
            gvRow.Cells(GVC_RefActivity).Enabled = False
            gvRow.Cells(GVC_WindowPeriod).Enabled = False
            gvRow.Cells(GVC_DeviationNegative).Enabled = False
            gvRow.Cells(GVC_DeviationPositive).Enabled = False
            gvRow.Cells(GVC_Update).Enabled = False
        End If
    End Sub

#Region "Web Methods"
    <Web.Services.WebMethod()> _
    Public Shared Function AuditTrailForActiveInActiveUser(ByVal WorkSpaceDeviationId As Integer, ByVal WorkSpaceId As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim ds As DataSet
        Dim eStr As String = String.Empty
        Dim strReturn As String = String.Empty
        Try

            Dim profilenmae As String
            Dim username1 As String


            If Not objHelp.Proc_GetAuditTrailWorkSpaeceDeviationReport(WorkSpaceDeviationId, WorkSpaceId, ds, eStr) Then
                Throw New Exception(eStr)
            End If


            Dim dtTempAuditTrail As New DataTable


            If Not dtTempAuditTrail Is Nothing Then
                dtTempAuditTrail.Columns.Add("Sr.No.", GetType(String))
                dtTempAuditTrail.Columns.Add("ProjectNo", GetType(String))
                dtTempAuditTrail.Columns.Add("ParentActivity", GetType(String))
                dtTempAuditTrail.Columns.Add("ChildActivity", GetType(String))
                dtTempAuditTrail.Columns.Add("RefActivity", GetType(String))
                dtTempAuditTrail.Columns.Add("WindowPeriod", GetType(String))
                dtTempAuditTrail.Columns.Add("DeviationPositive", GetType(String))
                dtTempAuditTrail.Columns.Add("DeviationNegative", GetType(String))
                dtTempAuditTrail.Columns.Add("ModifyBy", GetType(String))
                dtTempAuditTrail.Columns.Add("Modifyon", GetType(String))
                dtTempAuditTrail.Columns.Add("Remarks", GetType(String))
            End If

            dtTempAuditTrail.AcceptChanges()

            Dim drAuditTrail As DataRow

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                drAuditTrail = dtTempAuditTrail.NewRow()
                drAuditTrail("Sr.No.") = ""
                drAuditTrail("ProjectNo") = Convert.ToString(ds.Tables(0).Rows(i)("ProjectNo"))
                drAuditTrail("ParentActivity") = Convert.ToString(ds.Tables(0).Rows(i)("ParentActivity").ToString())
                drAuditTrail("ChildActivity") = Convert.ToString(ds.Tables(0).Rows(i)("ChildActivity").ToString())
                drAuditTrail("RefActivity") = Convert.ToString(ds.Tables(0).Rows(i)("RefActivity"))
                drAuditTrail("WindowPeriod") = Convert.ToString(ds.Tables(0).Rows(i)("WindowPeriod"))
                drAuditTrail("DeviationPositive") = Convert.ToString(ds.Tables(0).Rows(i)("DeviationPositive"))
                drAuditTrail("DeviationNegative") = Convert.ToString(ds.Tables(0).Rows(i)("DeviationNegative"))
                drAuditTrail("ModifyBy") = Convert.ToString(ds.Tables(0).Rows(i)("ModifyBy"))
                drAuditTrail("ModifyOn") = Convert.ToString(CDate(ds.Tables(0).Rows(i)("ModifyOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                drAuditTrail("Remarks") = Convert.ToString(ds.Tables(0).Rows(i)("Remarks"))
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




End Class
