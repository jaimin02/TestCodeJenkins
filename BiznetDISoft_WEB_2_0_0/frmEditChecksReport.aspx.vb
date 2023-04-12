
Partial Class frmEditChecksReport
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Private objCommon As New clsCommon
    Dim ObjHelpDB As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()

    Dim GvECRpt_SrNo As Integer = 0
    Dim GvECRpt_Remark As Integer = 1
    Dim GvECRpt_Query As Integer = 2
    Dim GvECRpt_ParentActivity As Integer = 3
    Dim GvECRpt_Activity As Integer = 4
    Dim GvECRpt_EnteredBy As Integer = 5
    Dim GvECRpt_EnteredOn As Integer = 6
    Dim GvECRpt_QueryStatus As Integer = 7
    Dim GvECRpt_QueryStatusLnk As Integer = 8
    Dim GvECRpt_nMedExEditCheckNo As Integer = 9

    Dim NotExecuted As String = "Not Executed"
    Dim NoQuery As String = "Executed without Query"
    Dim Query As String = "Executed with Query"

    Dim Resloved As String = "Resolved"
    'Dim IsQuery As String = "Is Query"
    'Dim NotQuery As String = "Not Query"
    Dim Resloved_Val As Integer = 1
    Dim IsQuery_Val As Integer = 2
    Dim NotQuery_Val As Integer = 3

    Dim GvExRpt_SrNo As Integer = 0
    Dim GvExRpt_iSourceNodeId_If As Integer = 1
    Dim GvExRpt_bIsQuery As Integer = 2
    Dim GvExRpt_Source_If_Period As Integer = 3
    Dim GvExRpt_CrossActivityPeriod As Integer = 4
    Dim GvExRpt_CrossActivityNode As Integer = 5
    Dim GvExRpt_vSubjectId As Integer = 6
    Dim GvExRpt_vMySubjectNo As Integer = 7
    Dim GvExRpt_vInitials As Integer = 8
    Dim GvExRpt_dFiredDate As Integer = 9
    Dim GvExRpt_vUserName As Integer = 10
    Dim GvExRpt_Activity As Integer = 11
    Dim GvExRpt_CrossActivity As Integer = 12
    Dim GvExRpt_QueryStatus As Integer = 13
    Dim GvExRpt_CrossActivityId As Integer = 14
    Dim GvExRpt_ActivityId As Integer = 15
    Dim GvExRpt_iMySubjectNo As Integer = 16

    Dim Vs_EditChecks As String = "dsEditChecks"
    Dim Vs_EditChecksReport As String = "dsEditChecksReport"
    Dim Vs_EditChecksFiltered As String = "dtEditChecksFiltered"
    Dim Vs_EditChecksReportFiltered As String = "dtEditChecksReportFiltered"

    Dim vs_QueryStatusClicked As String = "QueryStatusClicked"

#End Region

#Region "Page Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Page.Title = " :: Edit Checks Report  ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Master.FindControl("lblHeading"), Label).Text = "Edit Checks Execution Report"
            CType(Master.FindControl("lblMandatory"), Label).Text = ""
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID) & " And cProjectStatus Not In('I','T','P')"
            'Me.AutoExtenderProject.ContextKey = "iUserId = " & Me.Session(S_UserID) & " And cProjectStatus Not In('I','T','P')"
            'If Not GenCall() Then
            '    Exit Sub
            'End If

            If (Not Request.QueryString("mode") Is Nothing) AndAlso (Me.Request.QueryString("mode") = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View) Then
                If Not Request.QueryString("vWorkSpaceId") Is Nothing Then
                    Me.HProjectId.Value = Request.QueryString("vWorkSpaceId")
                    Me.txtProject.Text = Me.Request.QueryString("ProjectNo").ToString()
                    Me.txtProject.Enabled = False
                    BtnSetProject_Click(sender, e)
                    HideMenu()
                End If
            End If
        End If
    End Sub
#End Region

#Region "GenCall"
    'Private Function GenCall() As Boolean
    '    Dim eStr As String = String.Empty
    '    Try
    '        'If Not GenCall_Data() Then
    '        'End If
    '        If Not GenCall_ShowUI() Then
    '            Throw New Exception(eStr)
    '        End If
    '        Return True
    '    Catch ex As Exception
    '        Return eStr
    '        Exit Function
    '    End Try
    'End Function
    ''Private Function GenCall_Data() As Boolean
    ''    Try

    ''    Catch ex As Exception

    ''    End Try
    ''End Function
    'Private Function GenCall_ShowUI() As Boolean
    '    Dim eStr As String = String.Empty
    '    Try
    '        Return True

    '    Catch ex As Exception
    '        Return eStr
    '        Exit Function
    '    End Try
    'End Function
#End Region

#Region "Button Events"

    Protected Sub BtnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSetProject.Click
        Dim DS_Periods As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            ''''Get period
            If Me.HProjectId.Value.ToString <> "" Then
                wStr = " vWorkspaceId = '" & Me.HProjectId.Value.ToString() & "'"
            End If

            If Not ObjHelpDB.GetWorkspaceProtocolDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, DS_Periods, eStr) Then
                Throw New Exception(eStr)
            End If

            If DS_Periods.Tables(0).Rows.Count < 0 Then
                Throw New Exception("No periods found")
            End If
            Me.DdlPeriod.Items.Clear()
            For i As Integer = 0 To DS_Periods.Tables(0).Rows(0)("iNoOfPeriods")
                If i = 0 Then
                    Me.DdlPeriod.Items.Insert(i, "All Period")
                    Continue For
                End If
                Me.DdlPeriod.Items.Insert(i, i)
            Next i
            'Me.DdlPeriod.Items.Insert(DS_Periods.Tables(0).Rows(0)("iNoOfPeriods") + 1, "All Period")
            ''''''''

            ''''Get Activity
            If Not Me.GetActivity() Then
                Throw New Exception(eStr)
            End If
            ''''''''

            If Not GetEditChecks() Then
                Throw New Exception("Error while searching editchecks")
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message(), eStr)
            Exit Sub
        End Try

    End Sub

    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click
        Dim eStr As String = ""
        Try
            If Not FillGrid(False) Then
                Throw New Exception("Error while searching editchecks")
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Exit Sub
        End Try
    End Sub

    Protected Sub BtnRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnRefresh.Click
        Dim eStr As String = String.Empty
        Try
            Me.txtEndDt.Text = ""
            Me.txtStartDt.Text = ""
            If Me.DdlEnteredBy.Items.Count > 0 Then
                Me.DdlEnteredBy.SelectedIndex = 0
            End If
            If Me.DdlParentActivity.Items.Count > 0 Then
                Me.DdlParentActivity.SelectedIndex = 0
            End If
            If Me.DdlPeriod.Items.Count > 0 Then
                Me.DdlPeriod.SelectedIndex = 0
            End If
            If Me.DdlQryStatus.Items.Count > 0 Then
                Me.DdlQryStatus.SelectedIndex = 0
            End If
            Me.RblSubSpecific.SelectedIndex = 0

            If Not GetEditChecks() Then
                Throw New Exception("Error while searching editchecks")
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message(), eStr)
            Exit Sub
        End Try

    End Sub

    Protected Sub BtnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnReset.Click
        Dim eStr As String = String.Empty
        Try
            Me.DdlSubjects.SelectedIndex = 0
            Me.DdlEnteredBy.SelectedIndex = 0
            Me.txtFrmDate.Text = ""
            Me.txtToDate.Text = ""

            If Not Me.FillExecutedEditChecksGrid(True) Then
                Throw New Exception(eStr)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Exit Sub
        End Try
    End Sub

    Protected Sub BtnSearchSubjects_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearchSubjects.Click
        Dim eStr As String = String.Empty
        Try
            If Not Me.FillExecutedEditChecksGrid(False) Then
                Throw New Exception(eStr)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Exit Sub
        End Try
    End Sub

    Protected Sub btnExportEditChecks_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportEditChecks.Click
        Dim eStr As String = String.Empty

        Try
            If Me.GvEditChecksReport.Rows.Count < 1 Then
                Me.objCommon.ShowAlert("No data to Export", Me.Page)
                Exit Sub
            End If
            If Not ExportToExcel(CType(Me.ViewState(Vs_EditChecksFiltered), DataTable), "EditChecks") Then
                Throw New Exception("Problem while Exporting data")
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Exit Sub
        End Try
    End Sub

    Protected Sub btnExportSubjects_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportSubjects.Click
        Dim eStr As String = String.Empty

        Try
            If Me.GvEditChecksReport.Rows.Count < 1 Then
                Me.objCommon.ShowAlert("No data to Export", Me.Page)
                Exit Sub
            End If
            If Not ExportToExcel(CType(Me.ViewState(Vs_EditChecksReportFiltered), DataTable), "SubjectEditChecks") Then
                Throw New Exception("Problem while Exporting data")
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Exit Sub
        End Try
    End Sub

#End Region

#Region "HideMenu"

    Private Sub HideMenu()
        Dim tmpMenu As Menu
        Dim tmpddlprofile As DropDownList

        tmpMenu = CType(Me.Master.FindControl("menu1"), Menu)
        tmpddlprofile = CType(Me.Master.FindControl("ddlProfile"), DropDownList)

        If Not tmpMenu Is Nothing Then
            tmpMenu.Visible = False
        End If

        If Not tmpddlprofile Is Nothing Then
            tmpddlprofile.Enabled = False
        End If

    End Sub

#End Region

#Region "SelectedIndexChanged"

    Protected Sub DdlPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlPeriod.SelectedIndexChanged
        Dim eStr As String = String.Empty
        Try
            If Not Me.GetActivity() Then
                Throw New Exception(eStr)
            End If
            If Not FillGrid(False) Then
                Throw New Exception("Error while searching editchecks")
            End If
            If Me.HFPanelValues.Value.Trim.ToString.ToUpper = "YES" Then
                Me.pnlSearch.Style.Add("display", "")
                'Me.ImgDetail.Src = "images/collapse.jpg"
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message(), eStr)
            Exit Sub
        End Try
    End Sub

    Protected Sub RblSubSpecific_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RblSubSpecific.SelectedIndexChanged
        Try
            If Not Me.GetActivity() Then
                Exit Sub
            End If
            If Not FillGrid(False) Then
                Throw New Exception("Error while searching editchecks")
            End If
            If Me.HFPanelValues.Value.Trim.ToString.ToUpper = "YES" Then
                Me.pnlSearch.Style.Add("display", "")
                'Me.ImgDetail.Src = "images/collapse.jpg"
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message())
            Exit Sub
        End Try
    End Sub

    Protected Sub DdlEnteredBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlEnteredBy.SelectedIndexChanged
        Dim eStr As String = String.Empty
        Try
            If Not FillGrid(False) Then
                Throw New Exception("Error while searching editchecks")
            End If
            If Me.HFPanelValues.Value.Trim.ToString.ToUpper = "YES" Then
                Me.pnlSearch.Style.Add("display", "")
                'Me.ImgDetail.Src = "images/collapse.jpg"
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Exit Sub
        End Try
    End Sub

    Protected Sub DdlParentActivity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlParentActivity.SelectedIndexChanged
        Dim eStr As String = String.Empty
        Try
            If Not FillGrid(False) Then
                Throw New Exception("Error while searching editchecks")
            End If
            If Me.HFPanelValues.Value.Trim.ToString.ToUpper = "YES" Then
                Me.pnlSearch.Style.Add("display", "")
                'Me.ImgDetail.Src = "images/collapse.jpg"
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Exit Sub
        End Try

    End Sub

    Protected Sub DdlQryStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlQryStatus.SelectedIndexChanged
        Dim eStr As String = String.Empty
        Try
            If Not FillGrid(False) Then
                Throw New Exception("Error while searching editchecks")
            End If
            If Me.HFPanelValues.Value.Trim.ToString.ToUpper = "YES" Then
                Me.pnlSearch.Style.Add("display", "")
                'Me.ImgDetail.Src = "images/collapse.jpg"
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Exit Sub
        End Try

    End Sub

    Protected Sub DdlFiredBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlFiredBy.SelectedIndexChanged
        Dim eStr As String = String.Empty
        Try
            If Not Me.FillExecutedEditChecksGrid(False) Then
                Throw New Exception(eStr)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Exit Sub
        End Try
    End Sub

    Protected Sub DdlSubjects_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlSubjects.SelectedIndexChanged
        Dim eStr As String = String.Empty
        Try
            If Not Me.FillExecutedEditChecksGrid(False) Then
                Throw New Exception(eStr)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Exit Sub
        End Try
    End Sub

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

#Region "Fill Functions"

    Private Function GetActivity() As Boolean
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim Ds_Activity As New DataSet
        Dim Dv_Activity As New DataView

        Try

            Me.DdlParentActivity.Items.Clear()

            wStr = " vWorkspaceId = '" & Me.HProjectId.Value.ToString() & "' And cStatusIndi <> 'D'"
            If Me.RblSubSpecific.SelectedValue <> "A" Then
                wStr += " And cSubjectWiseFlag = '" & Me.RblSubSpecific.SelectedValue.ToString() & "'"
            End If

            'If Me.DdlPeriod.SelectedIndex.ToString() = "0" Then
            '    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "alert", "alert(""Please Select Period"");", True)
            '    'Return False
            '    Me.DdlParentActivity.Items.Insert(0, "Select Activity")
            '    Return True
            '    Exit Function
            'End If

            If Me.DdlPeriod.SelectedIndex.ToString() <> "0" Then
                wStr += " And iPeriod = " & Me.DdlPeriod.SelectedValue.ToString()
            End If

            wStr += " And iParentNodeId = 1"
            wStr += " And ISNULL(vTemplateId,'') <> '0001' And ("
            wStr += " iNodeId in (Select distinct iNodeId From View_MedExWorkSpaceHdr "
            wStr += " Where vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' "
            wStr += " And cStatusIndi <> 'D' And upper(IsNull(vMedExType,'')) <> 'IMPORT') "
            wStr += " Or iNodeId in (Select distinct iParentNodeId From View_MedExWorkSpaceHdr "
            wStr += " Where vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' "
            wStr += " And cStatusIndi <> 'D' And upper(IsNull(vMedExType,'')) <> 'IMPORT') "
            wStr += " ) order by iNodeNo"

            If Not Me.ObjHelpDB.GetViewWorkSpaceNodeDetail(wStr, Ds_Activity, eStr) Then
                Throw New Exception(eStr)
            End If

            If Ds_Activity.Tables(0).Rows.Count = 0 Then
                Me.DdlParentActivity.Items.Insert(0, "Select Activity")
                Return True
                Exit Function
            End If

            Me.DdlParentActivity.DataSource = Ds_Activity.Tables(0)
            Me.DdlParentActivity.DataTextField = "vNodeDisplayName"
            Me.DdlParentActivity.DataValueField = "iNodeId"
            Me.DdlParentActivity.DataBind()
            Me.DdlParentActivity.Items.Insert(0, "All Activity")

            For i As Integer = 0 To Me.DdlParentActivity.Items.Count - 1
                Me.DdlParentActivity.Items(i).Attributes.Add("title", Me.DdlParentActivity.Items(i).Text.ToString())
            Next

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Return False
            Exit Function
        End Try
    End Function

    Private Function GetEditChecks() As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim vWorkspaceId As String = String.Empty
        Dim vParentWorkspaceId As String = String.Empty
        Dim dsEditChecksRpt As New DataSet
        Dim dsParentWorkspace As New DataSet
        Dim Param As String = String.Empty
        Dim RowFilter As String = String.Empty

        Try
            wStr = " vWorkspaceId = '" + Me.HProjectId.Value.ToString() + "' And cStatusIndi <> 'D'"

            If Not ObjHelpDB.getworkspacemst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsParentWorkspace, eStr) Then
                Throw New Exception(eStr)
            End If

            If dsParentWorkspace.Tables(0).Rows.Count < 1 Then
                Throw New Exception("No Detail found for this project")
            End If

            vWorkspaceId = Me.HProjectId.Value.ToString()
            vParentWorkspaceId = Me.HProjectId.Value.ToString()

            If ConvertDbNullToDbTypeDefaultValue(dsParentWorkspace.Tables(0).Rows(0)("cWorkspaceType"), dsParentWorkspace.Tables(0).Rows(0)("cWorkspaceType").GetType) <> "P" And _
                 ConvertDbNullToDbTypeDefaultValue(dsParentWorkspace.Tables(0).Rows(0)("cWorkspaceType"), dsParentWorkspace.Tables(0).Rows(0)("cWorkspaceType").GetType) <> "" Then
                vParentWorkspaceId = Convert.ToString(dsParentWorkspace.Tables(0).Rows(0)("vParentWorkspaceId")).Trim()
            End If

            Param = vWorkspaceId + "##" + vParentWorkspaceId + "##"

            If Not ObjHelpDB.Get_EditChecksReport(Param, dsEditChecksRpt, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not dsEditChecksRpt.Tables(0).Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "alert", "alert(""No Edit Checks Entered for this project"");", True)
                Return True
                Exit Function
            End If

            Me.btnExportEditChecks.Style.Add("display", "")
            Me.ViewState(Vs_EditChecks) = dsEditChecksRpt

            ''''Entered By
            Me.DdlEnteredBy.DataSource = dsEditChecksRpt.Tables(0).DefaultView.ToTable(True, "iModifyBy,vUserName".Split(","))
            Me.DdlEnteredBy.DataValueField = "iModifyBy"
            Me.DdlEnteredBy.DataTextField = "vUserName"
            Me.DdlEnteredBy.DataBind()
            Me.DdlEnteredBy.Items.Insert(0, "All")
            Me.DdlEnteredBy.SelectedIndex = 0
            '''''''''

            ''''QueryStatus
            Me.DdlQryStatus.DataSource = dsEditChecksRpt.Tables(0).DefaultView.ToTable(True, "QueryStatus")
            Me.DdlQryStatus.DataValueField = "QueryStatus"
            Me.DdlQryStatus.DataTextField = "QueryStatus"
            Me.DdlQryStatus.DataBind()
            Me.DdlQryStatus.Items.Insert(0, "All")
            Me.DdlQryStatus.SelectedIndex = 0
            '''''''
            If Not FillGrid(True) Then
                Throw New Exception("Error while searching editchecks")
            End If

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Return False
            Exit Function
        End Try

    End Function

    Private Function FillGrid(ByVal FillFunction As Boolean) As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim vWorkspaceId As String = String.Empty
        Dim vParentWorkspaceId As String = String.Empty
        Dim dsEditChecksRpt As New DataSet
        Dim dsParentWorkspace As New DataSet
        Dim Param As String = String.Empty
        Dim RowFilter As String = String.Empty

        Try
            ''''Row filter for searched result in grid
            RowFilter = "1=1"
            If Not FillFunction Then
                If Me.DdlPeriod.SelectedIndex > 0 Then
                    RowFilter += " And Source_If_Period = " + Me.DdlPeriod.SelectedValue.ToString()
                End If

                If Me.DdlParentActivity.SelectedIndex > 0 Then
                    RowFilter += " And (iParentNodeId = " + Me.DdlParentActivity.SelectedValue.ToString() + " Or iSourceNodeId_If = " + Me.DdlParentActivity.SelectedValue.ToString() + "   )"
                End If

                If Me.DdlEnteredBy.SelectedIndex > 0 Then
                    RowFilter += " And iModifyBy = " + Me.DdlEnteredBy.SelectedValue.ToString()
                End If

                If Me.DdlQryStatus.SelectedIndex > 0 Then
                    RowFilter += " And QueryStatus = '" + Me.DdlQryStatus.SelectedValue.ToString() + "'"
                End If

                If Me.txtStartDt.Text.Trim.ToString <> "" And Me.txtEndDt.Text.Trim.ToString <> "" Then
                    RowFilter += " And (dModifyOn >= '" + Me.txtStartDt.Text.Trim.ToString + "' And dModifyOn <= '" + Me.txtEndDt.Text.Trim.ToString + "' )"
                End If

                If Me.RblSubSpecific.SelectedIndex > 0 Then
                    RowFilter += " And cSubjectWiseFlag = '" + Me.RblSubSpecific.SelectedValue.ToString() + "'"
                End If

                '''''''
            End If

            dsEditChecksRpt = CType(Me.ViewState(Vs_EditChecks), DataSet)

            dsEditChecksRpt.Tables(0).DefaultView.RowFilter = RowFilter
            Me.GvEditChecksReport.DataSource = dsEditChecksRpt.Tables(0).DefaultView
            Me.GvEditChecksReport.DataBind()
            Me.fsSearch.Style.Add("display", "")
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "FixHeader", "FixHeader(" & Me.GvEditChecksReport.ClientID & ");", True)

            Me.ViewState(Vs_EditChecksFiltered) = dsEditChecksRpt.Tables(0).DefaultView.ToTable()

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Return False
            Exit Function
        End Try

    End Function

    Private Function FillExecutedEditChecks(ByVal nMedexEditChecksNo As String, ByVal vWorkspaceId As String) As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim dsExecutedEditChecksRpt As New DataSet
        Dim Param As String = String.Empty
        Dim RowFilter As String = String.Empty

        Try

            Me.txtFrmDate.Text = ""
            Me.txtToDate.Text = ""

            Param = vWorkspaceId + "##" + nMedexEditChecksNo + "##"
            If Not ObjHelpDB.Get_EditChecksExecutedReport(Param, dsExecutedEditChecksRpt, eStr) Then
                Throw New Exception(eStr)
            End If

            If dsExecutedEditChecksRpt.Tables(0).Rows.Count < 1 Then
                Throw New Exception("No subjects found")
            End If


            Me.ViewState(Vs_EditChecksReport) = dsExecutedEditChecksRpt


            If Not Me.FillFiredBy(dsExecutedEditChecksRpt) Then
                Throw New Exception(eStr)
            End If

            If Not Me.FillExecutedEditChecksGrid(True) Then
                Throw New Exception(eStr)
            End If

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Return False
            Exit Function
        End Try

    End Function

    Private Function FillExecutedEditChecksGrid(ByVal CallFunction As Boolean) As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim dsExecutedEditChecksRpt As New DataSet
        Dim Param As String = String.Empty
        Dim RowFilter As String = String.Empty

        Try

            dsExecutedEditChecksRpt = CType(Me.ViewState(Vs_EditChecksReport), DataSet)

            Me.LblQuery.Text = dsExecutedEditChecksRpt.Tables(0).Rows(0)("vRemarks").ToString()
            If Me.ViewState(vs_QueryStatusClicked).ToString = "1" Then
                Me.LblLegends.Text = " Only Subjects with query will be displayed."
            End If

            RowFilter = " bIsQuery = " & Me.ViewState(vs_QueryStatusClicked).ToString
            If Not CallFunction Then
                If Me.DdlFiredBy.SelectedIndex > 0 Then
                    RowFilter += " And FiredById = " + Me.DdlFiredBy.SelectedValue.Trim.ToString()
                End If
                If Me.DdlSubjects.SelectedIndex > 0 Then
                    RowFilter += " And vMySubjectNo = '" + Me.DdlSubjects.SelectedValue.Trim.ToString() + "'"
                End If
                If Me.txtToDate.Text.Trim.ToString <> "" And Me.txtFrmDate.Text.Trim.ToString() <> "" Then
                    RowFilter += " And (dFiredDate >= '" + Me.txtFrmDate.Text.Trim.ToString() + "' And dFiredDate <= '" + Me.txtToDate.Text.Trim.ToString() + "')"
                End If
            End If

            dsExecutedEditChecksRpt.Tables(0).DefaultView.RowFilter = RowFilter
            Me.GvExecutedReport.DataSource = dsExecutedEditChecksRpt.Tables(0).DefaultView
            Me.GvExecutedReport.DataBind()

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "FixHeader", "FixHeader(" & Me.GvExecutedReport.ClientID & ")", True)

            If CallFunction Then
                If Not Me.FillSubjects(dsExecutedEditChecksRpt.Tables(0).DefaultView, Me.DdlSubjects.SelectedValue.Trim.ToString()) Then
                    Throw New Exception(eStr)
                End If
            End If


            Me.ViewState(Vs_EditChecksReportFiltered) = dsExecutedEditChecksRpt.Tables(0).DefaultView.ToTable()


            Me.MPExecutedEditChecks.Show()
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Return False
            Exit Function
        End Try

    End Function

    Private Function FillSubjects(ByVal Dv_Subjects As DataView, ByVal SelectedSub As Integer) As Boolean
        Dim eStr As String = String.Empty
        Dim Dt_Sub As DataTable = Nothing
        Try
            Dt_Sub = Dv_Subjects.ToTable(True, "vMySubjectNo,vSubjectId".Split(","))
            Dt_Sub.Columns.Add("DisplaySubject")
            Dt_Sub.AcceptChanges()
            For rowCount As Integer = 0 To Dt_Sub.Rows.Count - 1
                Dt_Sub.Rows(rowCount)("DisplaySubject") = Dt_Sub.Rows(rowCount)("vMySubjectNo") + "(" + Dt_Sub.Rows(rowCount)("vSubjectId") + ")"
            Next
            Me.DdlSubjects.Items.Clear()
            Me.DdlSubjects.DataSource = Dt_Sub
            Me.DdlSubjects.DataTextField = "DisplaySubject"
            Me.DdlSubjects.DataValueField = "vMySubjectNo"

            Me.DdlSubjects.DataBind()
            Me.DdlSubjects.Items.Insert(0, "All Subject")
            Me.DdlSubjects.Items(0).Value = 0
            Me.DdlSubjects.SelectedIndex = 0

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Return False
            Exit Function
        End Try

    End Function

    Private Function FillFiredBy(ByVal Ds_Users As DataSet) As Boolean
        Dim eStr As String = String.Empty
        Try
            Me.DdlFiredBy.DataSource = Ds_Users.Tables(0).DefaultView.ToTable(True, "FiredById,vUserName".Split(","))
            Me.DdlFiredBy.DataTextField = "vUserName"
            Me.DdlFiredBy.DataValueField = "FiredById"
            Me.DdlFiredBy.DataBind()
            Me.DdlFiredBy.Items.Insert(0, "All")
            Me.DdlFiredBy.SelectedIndex = 0

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Return False
            Exit Function
        End Try

    End Function

#End Region

#Region "Grid Events"

    Protected Sub GvEditChecksReport_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GvEditChecksReport.RowCommand
        Dim Index As Integer = e.CommandArgument
        Dim nMedexEditChecksNo As String = Me.GvEditChecksReport.Rows(Index).Cells(GvECRpt_nMedExEditCheckNo).Text.ToString
        Dim vworkspaceId As String = Me.HProjectId.Value.ToString()
        Dim eStr As String = String.Empty

        Try
            If e.CommandName.ToUpper.Trim.ToString = "VIEWEXECUTED" Then
                If Me.GvEditChecksReport.Rows(Index).Cells(GvECRpt_QueryStatus).Text = Query Then
                    Me.ViewState(vs_QueryStatusClicked) = "1"
                ElseIf Me.GvEditChecksReport.Rows(Index).Cells(GvECRpt_QueryStatus).Text = NoQuery Then
                    Me.ViewState(vs_QueryStatusClicked) = "0"
                End If
                If Not FillExecutedEditChecks(nMedexEditChecksNo, vworkspaceId) Then
                    Throw New Exception("Error while searching editchecks")
                End If
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Exit Sub
        End Try
    End Sub

    Protected Sub GvEditChecksReport_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvEditChecksReport.RowDataBound
        Dim eStr As String = String.Empty
        Try

            If e.Row.RowType = DataControlRowType.DataRow Or _
                e.Row.RowType = DataControlRowType.Footer Or _
                e.Row.RowType = DataControlRowType.Header Then


                e.Row.Cells(GvECRpt_QueryStatus).Attributes.Add("style", "display:none")
                e.Row.Cells(GvECRpt_nMedExEditCheckNo).Attributes.Add("style", "display:none")

                If e.Row.RowType = DataControlRowType.DataRow Then
                    e.Row.Cells(GvECRpt_SrNo).Text = e.Row.RowIndex + 1.ToString()
                    If e.Row.Cells(GvECRpt_QueryStatus).Text = Query Then
                        'CType(e.Row.FindControl("lBnQueryStatus"), LinkButton).Attributes.Add("OnClick", "return ViewDetail(" & e.Row.Cells(GvECRpt_nMedExEditCheckNo).Text.ToString() & ");") ' & e.Row.Cells(GvECRpt_nMedExEditCheckNo).Text.ToString() & "); return false;")
                        CType(e.Row.FindControl("lBnQueryStatus"), LinkButton).Text = "Executed with Query"
                        CType(e.Row.FindControl("lBnQueryStatus"), LinkButton).CommandArgument = e.Row.RowIndex
                        CType(e.Row.FindControl("lBnQueryStatus"), LinkButton).CommandName = "VIEWEXECUTED"
                    ElseIf e.Row.Cells(GvECRpt_QueryStatus).Text = NoQuery Then
                        'CType(e.Row.FindControl("lBnQueryStatus"), LinkButton).Attributes.Add("OnClick", "return ViewDetail(" & e.Row.Cells(GvECRpt_nMedExEditCheckNo).Text.ToString() & ");") ' & e.Row.Cells(GvECRpt_nMedExEditCheckNo).Text.ToString() & "); return false;")
                        CType(e.Row.FindControl("lBnQueryStatus"), LinkButton).Text = "Executed without Query"
                        CType(e.Row.FindControl("lBnQueryStatus"), LinkButton).CommandArgument = e.Row.RowIndex
                        CType(e.Row.FindControl("lBnQueryStatus"), LinkButton).CommandName = "VIEWEXECUTED"
                    End If
                    If e.Row.Cells(GvECRpt_QueryStatus).Text = NotExecuted Then
                        e.Row.Cells(GvECRpt_QueryStatusLnk).Text = e.Row.Cells(GvECRpt_QueryStatus).Text
                    End If
                    e.Row.Cells(GvECRpt_EnteredOn).Text = CType(e.Row.Cells(GvECRpt_EnteredOn).Text, DateTime).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset
                End If
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Exit Sub
        End Try
    End Sub

    Protected Sub GvExecutedReport_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvExecutedReport.RowDataBound
        Dim eStr As String = String.Empty
        Try
            Dim RedirectStr As String = String.Empty
            If e.Row.RowType = DataControlRowType.Header Or _
                e.Row.RowType = DataControlRowType.DataRow Or _
                e.Row.RowType = DataControlRowType.Footer Then

                e.Row.Cells(GvExRpt_bIsQuery).Attributes.Add("style", "display:none")
                e.Row.Cells(GvExRpt_CrossActivityNode).Attributes.Add("style", "display:none")
                e.Row.Cells(GvExRpt_CrossActivityPeriod).Attributes.Add("style", "display:none")
                e.Row.Cells(GvExRpt_iSourceNodeId_If).Attributes.Add("style", "display:none")
                'e.Row.Cells(GvExRpt_Source_If_Period).Attributes.Add("style", "display:none")
                e.Row.Cells(GvExRpt_ActivityId).Attributes.Add("style", "display:none")
                e.Row.Cells(GvExRpt_CrossActivityId).Attributes.Add("style", "display:none")

                e.Row.Cells(GvExRpt_vSubjectId).Attributes.Add("style", "display:none")
                e.Row.Cells(GvExRpt_iMySubjectNo).Attributes.Add("style", "display:none")

                If e.Row.RowType = DataControlRowType.DataRow Or _
                        e.Row.RowType = DataControlRowType.Header Then

                    If e.Row.RowType = DataControlRowType.DataRow Then
                        e.Row.Cells(GvExRpt_SrNo).Text = e.Row.RowIndex + 1
                        If (Not Request.QueryString("mode") Is Nothing) AndAlso (Me.Request.QueryString("mode") = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View) Then
                            RedirectStr = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" & Me.HProjectId.Value.ToString() & _
                               "&ActivityId=" & e.Row.Cells(GvExRpt_ActivityId).Text.ToString() & _
                                "&NodeId=" & e.Row.Cells(GvExRpt_iSourceNodeId_If).Text.ToString() & _
                                "&PeriodId=" & e.Row.Cells(GvExRpt_Source_If_Period).Text.ToString() & _
                                "&SubjectId=" & e.Row.Cells(GvExRpt_vSubjectId).Text.ToString() & "&Type=BA-BE" & _
                                "&MySubjectNo=" & e.Row.Cells(GvExRpt_iMySubjectNo).Text.ToString() & _
                                "&ScreenNo=" & e.Row.Cells(GvExRpt_vMySubjectNo).Text.ToString() & _
                                "&mode=" & WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View
                        Else
                            RedirectStr = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" & Me.HProjectId.Value.ToString() & _
                               "&ActivityId=" & e.Row.Cells(GvExRpt_ActivityId).Text.ToString() & _
                                "&NodeId=" & e.Row.Cells(GvExRpt_iSourceNodeId_If).Text.ToString() & _
                                "&PeriodId=" & e.Row.Cells(GvExRpt_Source_If_Period).Text.ToString() & _
                                "&SubjectId=" & e.Row.Cells(GvExRpt_vSubjectId).Text.ToString() & "&Type=BA-BE" & _
                                "&MySubjectNo=" & e.Row.Cells(GvExRpt_iMySubjectNo).Text.ToString() & _
                                "&ScreenNo=" & e.Row.Cells(GvExRpt_vMySubjectNo).Text.ToString() & _
                                "&mode=" & WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
                        End If
                        CType(e.Row.FindControl("lBtn_Activity"), LinkButton).OnClientClick = "return OpenWindow('" + RedirectStr + "');"
                        If (Not Request.QueryString("mode") Is Nothing) AndAlso (Me.Request.QueryString("mode") = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View) Then
                            RedirectStr = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" & Me.HProjectId.Value.ToString() & _
                         "&ActivityId=" & e.Row.Cells(GvExRpt_CrossActivityId).Text.ToString() & _
                         "&NodeId=" & e.Row.Cells(GvExRpt_CrossActivityNode).Text.ToString() & _
                         "&PeriodId=" & e.Row.Cells(GvExRpt_CrossActivityPeriod).Text.ToString() & _
                         "&SubjectId=" & e.Row.Cells(GvExRpt_vSubjectId).Text.ToString() & "&Type=BA-BE" & _
                         "&MySubjectNo=" & e.Row.Cells(GvExRpt_iMySubjectNo).Text.ToString() & _
                         "&ScreenNo=" & e.Row.Cells(GvExRpt_vMySubjectNo).Text.ToString() & _
                         "&mode=" & WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View
                        Else
                            RedirectStr = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" & Me.HProjectId.Value.ToString() & _
                           "&ActivityId=" & e.Row.Cells(GvExRpt_CrossActivityId).Text.ToString() & _
                           "&NodeId=" & e.Row.Cells(GvExRpt_CrossActivityNode).Text.ToString() & _
                           "&PeriodId=" & e.Row.Cells(GvExRpt_CrossActivityPeriod).Text.ToString() & _
                           "&SubjectId=" & e.Row.Cells(GvExRpt_vSubjectId).Text.ToString() & "&Type=BA-BE" & _
                           "&MySubjectNo=" & e.Row.Cells(GvExRpt_iMySubjectNo).Text.ToString() & _
                           "&ScreenNo=" & e.Row.Cells(GvExRpt_vMySubjectNo).Text.ToString() & _
                           "&mode=" & WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
                        End If
                       

                        CType(e.Row.FindControl("lBtn_CrossActivity"), LinkButton).OnClientClick = "return OpenWindow('" + RedirectStr + "');"
                        e.Row.Cells(GvExRpt_dFiredDate).Text = CType(e.Row.Cells(GvExRpt_dFiredDate).Text, DateTime).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset
                    End If
                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Exit Sub
        End Try
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

            fileName = "EditChecks"
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
        Dim dtConvert As New DataTable
        Try

            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            strMessage.Append("<td><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append("Project/Site")
            strMessage.Append("</font></strong></td>")
            If dtString = "EditChecks" Then
                strMessage.Append("<td colspan=""6""><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            Else
                strMessage.Append("<td colspan=""7""><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            End If
            strMessage.Append(Me.txtProject.Text.Trim())
            strMessage.Append("</font></strong></td>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")
            strMessage.Append("</tr>")

            If dtString <> "EditChecks" Then
                strMessage.Append("<tr>")
                strMessage.Append("<td><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Remark")
                strMessage.Append("</font></strong></td>")
                strMessage.Append("<td colspan=""7""><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(Me.LblQuery.Text.Trim())
                strMessage.Append("</font></strong></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr>")
                strMessage.Append("</tr>")
            End If

            'strMessage.Append("<tr>")
            'strMessage.Append("</tr>")

            strMessage.Append("<tr>")

            If dtString = "EditChecks" Then
                dtConvert = dt.DefaultView.ToTable(True, "vRemarks,vCondition,ParentActivity,Source_If_Activity,vUserName,dModifyOn,QueryStatus".Split(","))
                dtConvert.AcceptChanges()
                dtConvert.Columns(0).ColumnName = "Remark"
                dtConvert.Columns(1).ColumnName = "Query"
                dtConvert.Columns(2).ColumnName = "Parent Activity"
                dtConvert.Columns(3).ColumnName = "Activity"
                dtConvert.Columns(4).ColumnName = "Entered By"
                dtConvert.Columns(5).ColumnName = "Entered On"
                dtConvert.Columns(6).ColumnName = "Query Status"
                dtConvert.AcceptChanges()
            Else
                dtConvert = dt.DefaultView.ToTable(True, "Source_If_Period,vMySubjectNo,vInitials,dFiredDate,vUserName,Source_If_Activity,CrossActivity,QueryStatus".Split(","))
                dtConvert.AcceptChanges()
                dtConvert.Columns(0).ColumnName = "Period"
                dtConvert.Columns(1).ColumnName = "Screen No"
                dtConvert.Columns(2).ColumnName = "Initials"
                dtConvert.Columns(3).ColumnName = "Executed Date"
                dtConvert.Columns(4).ColumnName = "Executed By"
                dtConvert.Columns(5).ColumnName = "Activity"
                dtConvert.Columns(6).ColumnName = "Cross Activity"
                dtConvert.Columns(7).ColumnName = "Query Status"
                dtConvert.AcceptChanges()
            End If


            For iCol = 0 To dtConvert.Columns.Count - 1

                strMessage.Append("<td><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(Convert.ToString(dtConvert.Columns(iCol)).Trim())
                strMessage.Append("</font></strong></td>")

            Next
            strMessage.Append("</tr>")

            'strMessage.Append("<tr>")
            'strMessage.Append("</tr>")

            For j = 0 To dtConvert.Rows.Count - 1
                strMessage.Append("<tr>")
                For i = 0 To dtConvert.Columns.Count - 1

                    strMessage.Append("<td><strong><font color=""#000000"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(Convert.ToString(dtConvert.Rows(j).Item(i)).Trim())
                    strMessage.Append("</font></strong></td>")

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

End Class
