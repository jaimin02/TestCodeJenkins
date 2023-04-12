
Partial Class frmCrossActivityReport
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "

    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()


    Private Const VS_DtActivityGrid As String = "DtActivityGrid"
    Private Const VS_DtCrossActivityGrid As String = "DtCrossActivityGrid"
    Private Const Subject_Specific As String = "1"
    Private Const Generic_Activity As String = "2"

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            If Not GenCall_ShowUI() Then
                Exit Sub
            End If
        End If

    End Sub

#End Region

#Region "GenCall_ShowUI"

    Private Function GenCall_ShowUI() As Boolean
        Dim sender As New Object
        Dim e As New EventArgs

        Try
            Page.Title = " :: Activity Comparison Report ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Master.FindControl("lblHeading"), Label).Text = "Activity Comparison Report"



            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                Me.btnSetProject_Click(sender, e)
            End If

            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)

            Me.trPeriod.Attributes("style") = ""
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                'Me.trPeriod.Style.Add("display", "none")


            End If
            GenCall_ShowUI = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
        Finally
        End Try

    End Function

#End Region

#Region "Fill Functions" ' added by vishal for period filter

    Private Function FillddlPeriod() As Boolean
        Dim ds_WorkSpaceNodeDetail As New Data.DataSet
        Dim View_WorkSpaceNodeDetail As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty

        Try

            wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' and cSubjectWiseFlag<>'N'"
            If Me.DdlActivityType1.SelectedIndex = Generic_Activity Then
                wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() + "' and cSubjectWiseFlag='N' "
            End If
          
            ds_WorkSpaceNodeDetail = Nothing

            If Not objHelp.GetViewWorkSpaceNodeDetail(wstr, ds_WorkSpaceNodeDetail, estr) Then
                Me.objcommon.ShowAlert("Error While Getting Data From WorkSpaceModeDetail:" + estr, Me.Page)
                Return False
            End If

            View_WorkSpaceNodeDetail = ds_WorkSpaceNodeDetail.Tables(0).DefaultView
            View_WorkSpaceNodeDetail.Sort = "iPeriod"



            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                View_WorkSpaceNodeDetail.RowFilter = "iperiod<>0"
            End If
            Me.ddlperiod1.DataSource = ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable(True, "iPeriod".ToString())
            Me.ddlperiod1.DataValueField = "iPeriod"
            Me.ddlperiod1.DataTextField = "iPeriod"
            Me.ddlperiod1.DataBind()
            Me.ddlperiod1.Items.Insert(0, New ListItem("--Select Period--", 0))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillddlPeriod")
            Return False
        End Try
    End Function

    Private Function FillddlCrossPeriod() As Boolean
        Dim ds_WorkSpaceNodeDetail As New Data.DataSet
        Dim View_WorkSpaceNodeDetail As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty

        Try

            wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' and cSubjectWiseFlag<>'N'"
          
            If Me.DdlActivityType2.SelectedIndex = Generic_Activity Then
                wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() + "' and cSubjectWiseFlag='N' "
            End If

            ds_WorkSpaceNodeDetail = Nothing

            If Not objHelp.GetViewWorkSpaceNodeDetail(wstr, ds_WorkSpaceNodeDetail, estr) Then
                Me.objcommon.ShowAlert("Error While Getting Data From WorkSpaceModeDetail:" + estr, Me.Page)
                Return False
            End If

            View_WorkSpaceNodeDetail = ds_WorkSpaceNodeDetail.Tables(0).DefaultView
            View_WorkSpaceNodeDetail.Sort = "iPeriod"

            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                View_WorkSpaceNodeDetail.RowFilter = "iperiod<>0"
            End If
           
            Me.Ddlperiod2.DataSource = ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable(True, "iPeriod".ToString())
            Me.Ddlperiod2.DataValueField = "iPeriod"
            Me.Ddlperiod2.DataTextField = "iPeriod"
            Me.Ddlperiod2.DataBind()
            Me.Ddlperiod2.Items.Insert(0, New ListItem("--Select Cross Period--", 0))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillddlCrossPeriod")
            Return False
        End Try
    End Function

    Private Function FillVisit() As Boolean

        Dim eStr As String = String.Empty
        Dim ds_Activity As New DataSet
        Dim wStr As String = String.Empty
        Dim sender As New Object
        Dim e As New EventArgs
        Try

            ' changed by vishal to keep filter of period's

            If Me.ddlActivity.SelectedValue.Trim() = Generic_Activity Then
                If Not FillActivity() Then
                    Throw New Exception
                End If
                Exit Function
            End If

            wStr = Me.HProjectId.Value.Trim() + "##" + Me.ddlperiod1.SelectedItem.Value.Trim() 

            'If Me.DdlActivityType1.SelectedIndex = Generic_Activity Then

            'wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'And iperiod = '"
            'wStr += Me.ddlperiod1.SelectedItem.Value.Trim() + "And cSubjectWiseFlag='N'  And iParentNodeId = 1"
            'wStr += " and iNodeId in (select iNodeId from VIEW_MedExWorkspaceHdr where vWorkspaceid='" + Me.HProjectId.Value.Trim() + "'  And vMedExType <> 'Import' And cStatusIndi <> 'D' And cActiveFlag = 'Y')  Order by iNodeNo"
            'wStr += " Order by iNodeNo"

            'End If


            If Not Me.objHelp.Proc_ParentActivity(wStr, ds_Activity, eStr) Then
                Throw New Exception(eStr)
            End If

            Me.ddlVisit.DataSource = ds_Activity.Tables(0)
            Me.ddlVisit.DataTextField = "vNodeDisplayName"
            Me.ddlVisit.DataValueField = "iNodeId"
            Me.ddlVisit.DataBind()
            Me.ddlVisit.Items.Insert(0, New ListItem("--Select Visit/Parent Activity--", 0))


            'tooltip
            For count As Integer = 0 To ddlVisit.Items.Count - 1
                ddlVisit.Items(count).Attributes.Add("title", ddlVisit.Items(count).Text)
            Next

            For iSlcProject As Integer = 0 To ddlCrossVisit.Items.Count - 1
                ddlCrossVisit.Items(iSlcProject).Attributes.Add("title", ddlCrossVisit.Items(iSlcProject).Text)
            Next

            For count As Integer = 0 To ddlActivity.Items.Count - 1
                ddlActivity.Items(count).Attributes.Add("title", ddlActivity.Items(count).Text)
            Next

            For count As Integer = 0 To ddlCrossActivity.Items.Count - 1
                ddlCrossActivity.Items(count).Attributes.Add("title", ddlCrossActivity.Items(count).Text)
            Next

            Return True

        Catch ex As Exception
            ShowErrorMessage("Error While Filling Visits. ", ex.Message)
            Return False
        End Try

    End Function

    Private Function FillCrossVisit() As Boolean

        Dim eStr As String = String.Empty
        Dim ds_Activity As New DataSet
        Dim wStr As String = String.Empty
        Dim sender As New Object
        Dim e As New EventArgs
        Try

            ' changed by vishal to keep filter of period's

            '   wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'And iperiod = '"
            ' wStr += Me.Ddlperiod2.SelectedItem.Value.Trim() + "'And cSubjectWiseFlag<>'N'   And iParentNodeId = 1 Order by iNodeNo"

           

            If Me.DdlActivityType2.SelectedIndex = Generic_Activity Then
                wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And iperiod = '"
                wStr += Me.Ddlperiod2.SelectedItem.Value.Trim() + "' And iParentNodeId = 1"
                wStr += " and iNodeId in (Select isnull(iNodeId,0) From VIEW_MedExWorkspaceHdr Where vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And vMedExType <> 'Import' And cStatusIndi <> 'D' And cActiveFlag = 'Y' )"
                wStr += " And  cSubjectWiseFlag='N'"
                wStr += "  Order by iNodeNo"
                If Not Me.objHelp.GetViewWorkSpaceNodeDetail(wStr, ds_Activity, eStr) Then
                    Throw New Exception(eStr)
                End If
            Else
                wStr = Me.HProjectId.Value.Trim() + "##" + Me.Ddlperiod2.SelectedItem.Value.Trim()
                If Not Me.objHelp.Proc_ParentActivity(wStr, ds_Activity, eStr) Then
                    Throw New Exception(eStr)
                End If
            End If
            Me.ddlCrossVisit.DataSource = ds_Activity.Tables(0)
            Me.ddlCrossVisit.DataTextField = "vNodeDisplayName"
            Me.ddlCrossVisit.DataValueField = "iNodeId"
            Me.ddlCrossVisit.DataBind()

            Me.ddlCrossVisit.Items.Insert(0, New ListItem("--Select Cross Visit/Parent Cross Activity--", 0))
            'tooltip

            For count As Integer = 0 To ddlVisit.Items.Count - 1
                ddlVisit.Items(count).Attributes.Add("title", ddlVisit.Items(count).Text)
            Next

            For iSlcProject As Integer = 0 To ddlCrossVisit.Items.Count - 1
                ddlCrossVisit.Items(iSlcProject).Attributes.Add("title", ddlCrossVisit.Items(iSlcProject).Text)
            Next

            For count As Integer = 0 To ddlActivity.Items.Count - 1
                ddlActivity.Items(count).Attributes.Add("title", ddlActivity.Items(count).Text)
            Next

            For count As Integer = 0 To ddlCrossActivity.Items.Count - 1
                ddlCrossActivity.Items(count).Attributes.Add("title", ddlCrossActivity.Items(count).Text)
            Next


            'Me.ddlCrossVisit_SelectedIndexChanged(sender, e)

            Return True

        Catch ex As Exception
            ShowErrorMessage("Error While Filling Visits. ", ex.Message)
            Return False
        End Try

    End Function

    Private Function FillActivity() As Boolean

        Dim eStr As String = String.Empty
        Dim ds_Activity As New DataSet
        Dim wStr As String = String.Empty

        Try

            'changed by vishal for to get parent activitys

           
            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"

            If Me.DdlActivityType1.SelectedIndex <> Generic_Activity Then
                wStr += " And (iParentNodeId = " + Me.ddlVisit.SelectedItem.Value.Trim()
                wStr += " Or iNodeId = " + Me.ddlVisit.SelectedItem.Value.Trim() + ")"
            End If
            wStr += " And iperiod = " + Me.ddlperiod1.SelectedItem.Value.Trim()
            wStr += " And iNodeId In (Select isnull(iNodeId,0) From VIEW_MedExWorkspaceHdr Where vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And vMedExType <> 'Import' And cStatusIndi <> 'D' And cActiveFlag = 'Y' )"
            If Me.DdlActivityType1.SelectedIndex = Generic_Activity Then
                wStr += " And  cSubjectWiseFlag='N'" '  Order by iNodeNo"
            Else
                wStr += " And cSubjectWiseFlag<>'N'"  'Order by iNodeNo"
            End If
            wStr += " Order by iNodeNo"

            'If Me.DdlActivityType1.SelectedIndex = 2 Then
            '    wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'And iperiod = "
            '    wStr += Me.ddlperiod1.SelectedItem.Value.Trim() + "And  cSubjectWiseFlag='N'" '  Order by iNodeNo"
            'Else
            '    wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And (iParentNodeId = "
            '    wStr += Me.ddlVisit.SelectedItem.Value.Trim() + " Or iNodeId = "
            '    wStr += Me.ddlVisit.SelectedItem.Value.Trim() + ")And iperiod = "
            '    wStr += Me.ddlperiod1.SelectedItem.Value.Trim() + "and cSubjectWiseFlag<>'N'"  'Order by iNodeNo"
            'End If


            If Not Me.objHelp.GetViewWorkSpaceNodeDetail(wStr, ds_Activity, eStr) Then
                Throw New Exception(eStr)
            End If


            Me.ddlActivity.DataSource = ds_Activity.Tables(0)
            Me.ddlActivity.DataTextField = "vNodeDisplayName"
            Me.ddlActivity.DataValueField = "iNodeId"
            Me.ddlActivity.DataBind()

            Me.ddlActivity.Items.Insert(0, New ListItem("--Select  Activity--", 0))

            'tooltip
            For count As Integer = 0 To ddlVisit.Items.Count - 1
                ddlVisit.Items(count).Attributes.Add("title", ddlVisit.Items(count).Text)
            Next

            For iSlcProject As Integer = 0 To ddlCrossVisit.Items.Count - 1
                ddlCrossVisit.Items(iSlcProject).Attributes.Add("title", ddlCrossVisit.Items(iSlcProject).Text)
            Next

            For count As Integer = 0 To ddlActivity.Items.Count - 1
                ddlActivity.Items(count).Attributes.Add("title", ddlActivity.Items(count).Text)
            Next

            For count As Integer = 0 To ddlCrossActivity.Items.Count - 1
                ddlCrossActivity.Items(count).Attributes.Add("title", ddlCrossActivity.Items(count).Text)
            Next


            Return True

        Catch ex As Exception
            ShowErrorMessage("Error While Filling Activities. ", ex.Message)
            Return False
        End Try

    End Function

    Private Function FillCrossActivity() As Boolean

        Dim eStr As String = String.Empty
        Dim ds_Activity As New DataSet
        Dim wStr As String = String.Empty

        Try

            'changed by vishal for to get parent activitys

            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
            If Me.DdlActivityType2.SelectedIndex <> Generic_Activity Then
                wStr += "  And (iParentNodeId = " + Me.ddlCrossVisit.SelectedItem.Value.Trim() + " Or iNodeId = "
                wStr += Me.ddlCrossVisit.SelectedItem.Value.Trim() + ")"
            End If
            wStr += " And iperiod = " + Me.Ddlperiod2.SelectedItem.Value.Trim()
            wStr += " and iNodeId in (Select isnull(iNodeId,0) From VIEW_MedExWorkspaceHdr Where vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And vMedExType <> 'Import' And cStatusIndi <> 'D' And cActiveFlag = 'Y' ) "

            If Me.DdlActivityType2.SelectedIndex = Generic_Activity Then
                wStr += " And  cSubjectWiseFlag='N'  Order by iNodeNo"
            Else
                wStr += " And cSubjectWiseFlag<>'N' Order by iNodeNo"

            End If

            If Not Me.objHelp.GetViewWorkSpaceNodeDetail(wStr, ds_Activity, eStr) Then
                Throw New Exception(eStr)
            End If

            Me.ddlCrossActivity.DataSource = ds_Activity.Tables(0)
            Me.ddlCrossActivity.DataTextField = "vNodeDisplayName"
            Me.ddlCrossActivity.DataValueField = "iNodeId"
            Me.ddlCrossActivity.DataBind()
            Me.ddlCrossActivity.Items.Insert(0, New ListItem("--Select Cross Activity--", 0))

            'tooltip
            For count As Integer = 0 To ddlVisit.Items.Count - 1
                ddlVisit.Items(count).Attributes.Add("title", ddlVisit.Items(count).Text)
            Next

            For iSlcProject As Integer = 0 To ddlCrossVisit.Items.Count - 1
                ddlCrossVisit.Items(iSlcProject).Attributes.Add("title", ddlCrossVisit.Items(iSlcProject).Text)
            Next

            For count As Integer = 0 To ddlActivity.Items.Count - 1
                ddlActivity.Items(count).Attributes.Add("title", ddlActivity.Items(count).Text)
            Next

            For count As Integer = 0 To ddlCrossActivity.Items.Count - 1
                ddlCrossActivity.Items(count).Attributes.Add("title", ddlCrossActivity.Items(count).Text)
            Next


            Return True

        Catch ex As Exception
            ShowErrorMessage("Error While Filling Activities. ", ex.Message)
            Return False
        End Try

    End Function

    Private Function FillGrid() As Boolean

        Dim eStr As String = String.Empty
        Dim ds_Grid As New DataSet
        Dim ds_Grid1 As New DataSet
        Dim dv_Grid As New DataView
        Dim dv_Grid1 As New DataView
        Dim dt_Grid As New DataTable
        Dim dt_Grid1 As New DataTable
        Dim wStr As String = String.Empty
        Dim flag_Selected As Boolean = False

        Try


            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
            wStr += " And (iNodeId = " + Me.ddlActivity.SelectedItem.Value.Trim() + " Or "
            wStr += " iNodeId = " + Me.ddlCrossActivity.SelectedItem.Value.Trim() + ")"

            If Me.DdlActivityType1.SelectedIndex = Subject_Specific Then
                wStr += " And vSubjectId in('0',"
                For Each item In Me.chkLstSubjects.Items
                    If item.Selected Then
                        flag_Selected = True
                        wStr += "'" + item.Value.Trim() + "',"
                    End If
                Next item

                'If flag_Selected = False Then
                '    Me.objcommon.ShowAlert("Please select Subject(s).", Me.Page)
                '    Exit Function
                'End If

                wStr = wStr.Substring(0, wStr.LastIndexOf(","))
                wStr += ") Order by iMySubjectNo,iNodeNo,irepeatNo,IseqNo"

            End If

            If Not Me.objHelp.View_CRFHdrDtlSubDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_Grid, eStr) Then
                Throw New Exception(eStr)
            End If

            'changed by vishal
          
            dv_Grid = ds_Grid.Tables(0).DefaultView
            dv_Grid.RowFilter = "iNodeId = " + Me.ddlActivity.SelectedItem.Value.Trim()

            Me.btnExportGrid.Visible = False
            Me.LblgridActivity.Visible = True

            If dv_Grid.ToTable().Rows.Count > 0 Then
                Me.btnExportGrid.Visible = True
                Me.LblgridActivity.Visible = False
            End If


            Me.ViewState(VS_DtActivityGrid) = dv_Grid.ToTable(True, "vMySubjectNo", "vMedExDesc", "vMedExResult", "iTranNo", "dModifyOnSubDtl", "CRFSubDtlChangedBy", "vModificationRemark", "iRepeatNo")
            Me.gvwActivityGrid.DataSource = CType(ViewState(VS_DtActivityGrid), DataTable)  'dv_Grid.ToTable(True, "vMySubjectNo", "vMedExDesc", "vMedExResult", "iTranNo", "dModifyOnSubDtl", "CRFSubDtlChangedBy", "vModificationRemark", "iRepeatNo")
            Me.gvwActivityGrid.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "fngvwActivityGrid", "fngvwActivityGrid();", True)

            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
            wStr += " And (iNodeId = " + Me.ddlActivity.SelectedItem.Value.Trim() + " Or "
            wStr += " iNodeId = " + Me.ddlCrossActivity.SelectedItem.Value.Trim() + ") Order by iNodeNo,irepeatNo,IseqNo"

            If Me.DdlActivityType2.SelectedIndex = Subject_Specific Then
                wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
                wStr += " And (iNodeId = " + Me.ddlActivity.SelectedItem.Value.Trim() + " Or "
                wStr += " iNodeId = " + Me.ddlCrossActivity.SelectedItem.Value.Trim() + ")"
                wStr += " And vSubjectId in('0',"
                For Each item In Me.ChklstSubjects1.Items
                    If item.Selected Then
                        flag_Selected = True
                        wStr += "'" + item.Value.Trim() + "',"
                    End If
                Next item

                'If flag_Selected = False Then
                '    Me.objcommon.ShowAlert("Please select Subject(s).", Me.Page)
                '    Exit Function
                'End If

                wStr = wStr.Substring(0, wStr.LastIndexOf(","))
                wStr += ") Order by iMySubjectNo,iNodeNo,irepeatNo,IseqNo"

            End If

            If Not Me.objHelp.View_CRFHdrDtlSubDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_Grid1, eStr) Then
                Throw New Exception(eStr)
            End If
            If (ds_Grid.Tables(0).Rows.Count < 1 And ds_Grid1.Tables(0).Rows.Count < 1) Then

                Me.objcommon.ShowAlert("Records Not Found", Me.Page)

            End If

            dv_Grid1 = ds_Grid1.Tables(0).DefaultView
            dv_Grid1.RowFilter = "iNodeId = " + Me.ddlCrossActivity.SelectedItem.Value.Trim()


            Me.btnExportCrossGrid.Visible = False

            Me.lblgridCrossActivity.Visible = True

            If dv_Grid1.ToTable().Rows.Count > 0 Then
                Me.btnExportCrossGrid.Visible = True
                Me.lblgridCrossActivity.Visible = False
            End If

            Me.ViewState(VS_DtCrossActivityGrid) = dv_Grid1.ToTable(True, "vMySubjectNo", "vMedExDesc", "vMedExResult", "iTranNo", "dModifyOnSubDtl", "CRFSubDtlChangedBy", "vModificationRemark", "iRepeatNo")
            Me.gvwCrossActivityGrid.DataSource = CType(ViewState(VS_DtCrossActivityGrid), DataTable) 'dv_Grid1.ToTable(True, "vMySubjectNo", "vMedExDesc", "vMedExResult", "iTranNo", "dModifyOnSubDtl", "CRFSubDtlChangedBy", "vModificationRemark", "iRepeatNo")
            Me.gvwCrossActivityGrid.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "fngvwCrossActivityGrid", "fngvwCrossActivityGrid();", True)
            Return True

        Catch ex As Exception
            ShowErrorMessage("Error While Filling Grid. ", ex.Message)
            Return False
        End Try

    End Function

    Private Function FillSubjects() As Boolean
        Dim eStr As String = String.Empty
        Dim ds_Subjects As New DataSet
        Dim wStr As String = String.Empty
        Dim index As Integer = 0
        Dim lItem As ListItem

        Try
            Me.chkLstSubjects.Items.Clear()

            If Me.DdlActivityType1.SelectedIndex = Generic_Activity Then
                Return True
                Exit Function
            End If
            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' and iMySubjectNo>0  And cStatusIndi <> 'D'"
            If Me.ddlperiod1.SelectedIndex > 0 Then
                wStr += " And iPeriod = " + Me.ddlperiod1.SelectedValue.Trim()
            Else
                wStr += " And iPeriod = 1"
            End If
            wStr += " order by iMySubjectNo"

            If Not Me.objHelp.GetViewWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Subjects, eStr) Then
                Throw New Exception(eStr)
            End If

            'If Not Me.objHelp.View_WorkSpaceSubjectRegistration(wStr + "order by iMySubjectNo", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '                        ds_Subjects, eStr) Then
            '    Throw New Exception(eStr)
            'End If


            If Not ds_Subjects.Tables(0) Is Nothing Then
                Me.ChklstSubjects1.Items.Clear()
                For Each dr As DataRow In ds_Subjects.Tables(0).Rows
                    lItem = New ListItem()
                    lItem.Value = dr("vSubjectId")
                    lItem.Text = Convert.ToString(dr("vInitials")).Trim() + "(" + Convert.ToString(dr("vMySubjectNo")).Trim() + ")(" + Convert.ToString(dr("vRandomizationNo")).Trim() + ")"

                    'If dr("cRejectionflag").ToString.ToUpper = "Y" Then
                    '    lItem.Attributes.Add("style", "color:red")
                    'End If
                    Me.chkLstSubjects.Items.Add(lItem)

                    index += 1
                Next dr
                'ds_Subjects.AcceptChanges()
                'Me.chkLstSubjects.DataSource = ds_Subjects.Tables(0)
                'Me.chkLstSubjects.DataTextField = "FieldToDisplay"
                'Me.chkLstSubjects.DataValueField = "vSubjectId"
                'Me.chkLstSubjects.DataBind()
            End If

            Return True

        Catch ex As Exception
            ShowErrorMessage("Error While Filling Subjects. ", ex.Message)
            Return False
        End Try

    End Function

    Private Function FillCrossSubjects() As Boolean
        Dim eStr As String = String.Empty
        Dim ds_CrossSubjects As New DataSet
        Dim wStr As String = String.Empty
        Dim index As Integer = 0
        Dim lCrossItem As ListItem

        Try

            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' and iMySubjectNo>0  And cStatusIndi <> 'D'"

            If Me.Ddlperiod2.SelectedIndex > 0 Then
                wStr += " And iPeriod = " + Me.Ddlperiod2.SelectedValue.Trim()
            Else
                wStr += " And iPeriod = 1"
            End If

            If Not Me.objHelp.GetViewWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CrossSubjects, eStr) Then
                Throw New Exception(eStr)
            End If

            'If Not Me.objHelp.View_WorkSpaceSubjectRegistration(wStr + "order by iMySubjectNo", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '                        ds_CrossSubjects, eStr) Then
            '    Throw New Exception(eStr)
            'End If

            If Not ds_CrossSubjects.Tables(0) Is Nothing Then
                Me.ChklstSubjects1.Items.Clear()
                For Each dr As DataRow In ds_CrossSubjects.Tables(0).Rows
                    lCrossItem = New ListItem
                    lCrossItem.Value = dr("vSubjectId")
                    lCrossItem.Text = Convert.ToString(dr("vInitials")).Trim() + "(" + Convert.ToString(dr("vMySubjectNo")).Trim() + ")(" + Convert.ToString(dr("vRandomizationNo")).Trim() + ")"
                    'If dr("cRejectionflag").ToString.ToUpper = "Y" Then
                    '    lCrossItem.Attributes.Add("style", "color:red")
                    'End If

                    Me.ChklstSubjects1.Items.Add(lCrossItem)

                    index += 1


                Next dr
                'ds_CrossSubjects.AcceptChanges()
                'Me.ChklstSubjects1.DataSource = ds_CrossSubjects.Tables(0)
                'Me.ChklstSubjects1.DataTextField = "FieldToDisplay"
                'Me.ChklstSubjects1.DataValueField = "vSubjectId"
                'Me.ChklstSubjects1.DataBind()
            End If

            Return True

        Catch ex As Exception
            ShowErrorMessage("Error While Filling Subjects. ", ex.Message)
            Return False
        End Try

    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        ' Me.chkSelectAll.Enabled = True
        'Me.chkSelectAll1.Enabled = True
        resetPage()

        Me.DdlActivityType1.Items.Insert(0, "--Select Activity Type--")
        Me.DdlActivityType1.Items.Insert(Subject_Specific, "Subject Specific")
        Me.DdlActivityType1.Items.Insert(Generic_Activity, "Generic Activity")

        Me.DdlActivityType2.Items.Insert(0, "--Select Cross Activity Type--")
        Me.DdlActivityType2.Items.Insert(Subject_Specific, "Subject Specific")
        Me.DdlActivityType2.Items.Insert(Generic_Activity, "Generic Activity")

    End Sub

    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
        If Not Me.FillGrid() Then
            Exit Sub
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.txtproject.Text = ""
        Me.HProjectId.Value = ""
        Me.resetPage()
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

#Region "Drop Down Events"

    Protected Sub ddlVisit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlVisit.SelectedIndexChanged
        If Not Me.FillActivity() Then
            Exit Sub
        End If
    End Sub


    Protected Sub ddlCrossVisit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCrossVisit.SelectedIndexChanged
        If Not Me.FillCrossActivity() Then
            Exit Sub
        End If
    End Sub

#End Region

#Region "Reset Page"

    Private Sub resetPage()

        Me.chkLstSubjects.Items.Clear()
        Me.ChklstSubjects1.Items.Clear()
        Me.chkSelectAll.Checked = False
        Me.chkSelectAll1.Checked = False

        Me.DdlActivityType1.Items.Clear()
        Me.DdlActivityType2.Items.Clear()
        Me.ddlperiod1.Items.Clear()
        Me.Ddlperiod2.Items.Clear()
        Me.ddlVisit.Items.Clear()
        Me.ddlCrossVisit.Items.Clear()
        Me.ddlActivity.Items.Clear()
        Me.ddlCrossActivity.Items.Clear()
        Me.gvwActivityGrid.DataSource = Nothing
        Me.gvwActivityGrid.DataBind()
        Me.gvwCrossActivityGrid.DataSource = Nothing
        Me.gvwCrossActivityGrid.DataBind()
        Me.ViewState(VS_DtActivityGrid) = Nothing
        Me.ViewState(VS_DtCrossActivityGrid) = Nothing
        Me.btnExportCrossGrid.Visible = False
        Me.btnExportGrid.Visible = False
        Me.LblgridActivity.Visible = False
        Me.lblgridCrossActivity.Visible = False

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

#Region "Export To Excel Logic & Events"

    Protected Sub btnExportGrid_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim fileName As String = String.Empty
        Dim ds As New DataSet
        Try

            If Me.gvwActivityGrid.Rows.Count < 1 Then
                Me.objcommon.ShowAlert("No Data To Export", Me.Page)
                Exit Sub
            End If
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            fileName = "CRFActivityReport"
            fileName = fileName & ".xls"
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            ds.Tables.Add(CType(Me.ViewState(VS_DtActivityGrid), DataTable).Copy())
            ds.AcceptChanges()

            Context.Response.Write(ConvertDsTO(ds))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...btnExportGrid_Click")
        End Try
    End Sub

    Protected Sub btnExportCrossGrid_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim fileName As String = String.Empty
        Dim ds As New DataSet
        Try

            If Me.gvwCrossActivityGrid.Rows.Count < 1 Then
                Me.objcommon.ShowAlert("No Data To Export", Me.Page)
                Exit Sub
            End If
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            fileName = "CRFCrossActivityReport"
            fileName = fileName & ".xls"
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            ds.Tables.Add(CType(Me.ViewState(VS_DtCrossActivityGrid), DataTable).Copy())
            ds.AcceptChanges()

            Context.Response.Write(ConvertDsTO(ds))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...btnExportCrossGrid_Click")
        End Try
    End Sub

    Private Function ConvertDsTO(ByVal ds As DataSet) As String
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer

        Try

            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            strMessage.Append("<td><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append("Project/Site")
            strMessage.Append("</font></strong></td>")
            strMessage.Append("<td colspan=""4""><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append(Me.txtproject.Text.Trim())
            strMessage.Append("</font></strong></td>")
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")

            For iCol = 0 To ds.Tables(0).Columns.Count - 1
                ds.Tables(0).Columns(iCol).ColumnName = Me.gvwActivityGrid.Columns(iCol).HeaderText.Trim()

                If ds.Tables(0).Columns(iCol).ToString <> "TranNo" Then
                    strMessage.Append("<td><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(ds.Tables(0).Columns(iCol).ToString)
                    strMessage.Append("</font></strong></td>")
                End If
            Next
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")

            For j = 0 To ds.Tables(0).Rows.Count - 1
                strMessage.Append("<tr>")
                For i = 0 To ds.Tables(0).Columns.Count - 1
                    If ds.Tables(0).Columns(i).ToString <> "TranNo" Then
                        If IsDBNull(ds.Tables(0).Rows(j).Item(i)) = False Then
                            If (CType(IIf(IsDBNull(ds.Tables(0).Rows(j).Item(i)) = True, "", ds.Tables(0).Rows(j).Item(i)), String) = "N") Then
                                strMessage.Append("<td><strong><font color=""green"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                            Else
                                strMessage.Append("<td><strong><font color=""#000000"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                            End If
                        Else
                            strMessage.Append("<td><strong><font color=""red"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                        End If
                        strMessage.Append(ds.Tables(0).Rows(j).Item(i))
                        strMessage.Append("</font></strong></td>")
                    End If
                Next
                strMessage.Append("</tr>")
            Next
            strMessage.Append("</table>")

            Return strMessage.ToString

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....ConvertDsTO")
            Return ""
        End Try
    End Function

#End Region

#Region "Index change event"

    Protected Sub ddlperiod1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlperiod1.SelectedIndexChanged
        Dim eStr As String = Nothing
        Try
            If Not FillSubjects() Then
                Throw New Exception()
            End If
            If Me.DdlActivityType1.SelectedIndex = Subject_Specific Then
                If Not Me.FillVisit() Then
                    Throw New Exception()
                End If
            Else
                If Not Me.FillActivity() Then
                    Throw New Exception()
                End If

            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Exit Sub
        End Try

    End Sub

    Protected Sub Ddlperiod2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Ddlperiod2.SelectedIndexChanged
        Dim eStr As String = Nothing
        Try
            If Not FillCrossSubjects() Then
                Throw New Exception()
            End If
            If Me.DdlActivityType2.SelectedIndex = Subject_Specific Then

                If Not Me.FillCrossVisit() Then
                    Exit Sub
                End If
            Else
                If Not Me.FillCrossActivity() Then
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
            Exit Sub
        End Try
    End Sub

    Protected Sub DdlActivityType1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlActivityType1.SelectedIndexChanged

        Me.ddlVisit.Enabled = True
        Me.chkSelectAll.Visible = True
        Me.pnlSubjects.Enabled = True
        Me.trsubject.Visible = True
        Me.trvisit.Visible = True
        Me.chkSelectAll.Enabled = True
        Me.chkSelectAll.Checked = False
        Me.ddlActivity.Items.Clear()
        Me.chkLstSubjects.Items.Clear()

        If Not Me.FillddlPeriod() Then
            Exit Sub
        End If

        If Me.ddlperiod1.Items.Count > 0 AndAlso Me.ddlperiod1.SelectedIndex > 0 Then
            If Not Me.FillSubjects() Then
                Exit Sub
            End If
        End If

        If Me.DdlActivityType1.SelectedIndex.ToString.Trim() = Generic_Activity Then
            'If Not Me.FillActivity() Then
            '    Exit Sub
            'End If
            Me.ddlVisit.Enabled = False
            Me.chkSelectAll.Enabled = False
            Me.pnlSubjects.Enabled = False
            Me.ddlVisit.Items.Clear()
            Me.chkSelectAll.Checked = False


            If Me.DdlActivityType2.SelectedIndex.ToString.Trim() = Generic_Activity Then
                Me.trsubject.Visible = False
                Me.trvisit.Visible = False
                Me.chkSelectAll.Visible = False
                Me.chkSelectAll1.Visible = False
            End If

        ElseIf Me.DdlActivityType1.SelectedIndex = 0 Then
            Me.chkSelectAll.Enabled = False
            Me.chkLstSubjects.Items.Clear()
            Me.chkSelectAll.Checked = False
            Me.ddlperiod1.Items.Clear()
            Me.ddlVisit.Items.Clear()
            Me.ddlActivity.Items.Clear()
            Me.gvwActivityGrid.DataSource = Nothing
            Me.gvwActivityGrid.DataBind()
            Me.ViewState(VS_DtActivityGrid) = Nothing
            Me.btnExportGrid.Visible = False
            Me.LblgridActivity.Visible = False
        End If
    End Sub

    Protected Sub DdlActivityType2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlActivityType2.SelectedIndexChanged


        Me.ddlCrossVisit.Enabled = True
        Me.chkSelectAll1.Visible = True
        Me.pnlSubjects1.Enabled = True
        Me.trsubject.Visible = True
        Me.trvisit.Visible = True
        Me.chkSelectAll1.Enabled = True
        Me.chkSelectAll.Checked = False
        Me.ddlCrossActivity.Items.Clear()
        Me.ChklstSubjects1.Items.Clear()

        If Not Me.FillddlCrossPeriod() Then
            Exit Sub
        End If


        If Me.Ddlperiod2.Items.Count > 0 AndAlso Me.Ddlperiod2.SelectedIndex > 0 Then
            If Not Me.FillCrossSubjects() Then
                Exit Sub
            End If
        End If

        If Me.DdlActivityType2.SelectedIndex = Generic_Activity Then
            'If Not Me.FillCrossActivity() Then
            '    Exit Sub
            'End If
            Me.ddlCrossVisit.Enabled = False
            Me.chkSelectAll1.Enabled = False
            Me.pnlSubjects1.Enabled = False
            Me.ddlCrossVisit.Items.Clear()


            If Me.DdlActivityType1.SelectedIndex = Generic_Activity Then
                Me.trsubject.Visible = False
                Me.trvisit.Visible = False
                Me.chkSelectAll.Visible = False
                Me.chkSelectAll1.Visible = False

            End If
        ElseIf Me.DdlActivityType2.SelectedIndex = 0 Then
            Me.chkSelectAll1.Enabled = False
            Me.ChklstSubjects1.Items.Clear()
            Me.chkSelectAll1.Checked = False
            Me.Ddlperiod2.Items.Clear()
            Me.ddlCrossVisit.Items.Clear()
            Me.ddlCrossActivity.Items.Clear()
            Me.gvwCrossActivityGrid.DataSource = Nothing
            Me.gvwCrossActivityGrid.DataBind()
            Me.ViewState(VS_DtCrossActivityGrid) = Nothing
            Me.btnExportCrossGrid.Visible = False
            Me.lblgridCrossActivity.Visible = False

        End If

    End Sub
   
#End Region

#Region "Grid row created"
    Protected Sub gvwActivityGrid_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwActivityGrid.RowCreated
        If Me.DdlActivityType1.SelectedIndex = 2 Then
            e.Row.Cells(0).Visible = False
        End If

    End Sub


    Protected Sub gvwCrossActivityGrid_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwCrossActivityGrid.RowCreated
        If Me.DdlActivityType2.SelectedIndex = 2 Then
            e.Row.Cells(0).Visible = False
        End If
    End Sub
#End Region

   
    
End Class
