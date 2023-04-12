Imports System.Web.UI

Partial Class frmSubjectWiseEntry
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION"
    Private ObjCommon As New clsCommon
    Private ObjHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private Const VS_DtCRFHdrDtlSubDtl As String = "CRFHdrDtlSubDtl"
    Private Const VS_Controls As String = "VS_Controls"
    Private Const VS_ControlsSubject As String = "VS_ControlsSubject"
#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not GenCall() Then
                Exit Sub
            End If
        End If
    End Sub

#End Region

#Region "GenCall()"

    Private Function GenCall() As Boolean
        Dim Sender As New Object
        Dim e As EventArgs
        Try
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Activity Data In Tabular Format"
            Page.Title = ":: Activity Data In Table Format  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(Sender, e)
            End If
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                Me.trPeriod.Visible = False
            Else
                Me.trPeriod.Visible = True
            End If
            GenCall = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall")
            GenCall = False
        End Try
    End Function

#End Region

#Region "GenCall_showUI()"

    Private Function GenCall_ShowUI() As Boolean
        Try

            If Not FillddlPeriod() Then
                Return False
            End If
            GenCall_ShowUI = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
            GenCall_ShowUI = False
        End Try
    End Function

#End Region

#Region "Fill Function"

    Private Function FillddlPeriod() As Boolean
        Dim ds_Period As New DataSet
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Try
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                Me.ddlPeriod.Items.Insert(1, "1")
                ds_Period = Me.ObjHelp.GetResultSet("select iNodeId,vNodeDisplayName from View_WorkspaceNodeDetail where vWorkspaceId='" + Me.HProjectId.Value + _
                                                      "' And iParentNodeId = 1 And cStatusIndi<>'D' AND ( vTemplateId <> '0001' OR  vTemplateId IS NULl)   order by iNodeNo", "WorkspaceNodeDetail")
                Me.ddlVisit.DataSource = ds_Period.Tables(0)
                Me.ddlVisit.DataValueField = "iNodeId"
                Me.ddlVisit.DataTextField = "vNodeDisplayName"
                Me.ddlVisit.DataBind()
                Me.ddlVisit.Items.Insert(0, "Select Visit/Parent Activity")
                Me.ddlActivity.DataSource = Nothing
                Me.ddlActivity.DataBind()
                Me.ddlActivity.Items.Clear()
                Me.ddlActivity.Items.Insert(0, "Select Activity")
                If Me.rblSubjectSpecific.SelectedIndex = 0 Then
                    If Not FillChkSubject() Then
                        Return False
                    End If
                Else
                    Me.trChkActivity.Style.Add("display", "none")
                    Me.trChkSubject.Style.Add("display", "none")
                    Me.ViewState(VS_Controls) = Nothing
                    Me.ViewState(VS_ControlsSubject) = Nothing
                End If
            Else
                ds_Period = Me.ObjHelp.GetResultSet("select distinct iPeriod from View_WorkspaceNodeDetail where vWorkspaceId='" + Me.HProjectId.Value + "'And cStatusIndi<>'D' AND ( vTemplateId <> '0001' OR  vTemplateId IS NULl)   ", "View_WorkspaceNodeDetail")
                Me.ddlPeriod.DataSource = ds_Period.Tables(0)
                Me.ddlPeriod.DataValueField = "iPeriod"
                Me.ddlPeriod.DataTextField = "iPeriod"
                Me.ddlPeriod.DataBind()
                Me.ddlPeriod.Items.Insert(0, New ListItem("Select Period", 0))
                If ds_Period.Tables(0).Rows.Count > 1 Then
                    Me.ddlPeriod.Items.Insert(1, New ListItem("All", "All"))
                End If
                Me.ddlVisit.DataSource = Nothing
                Me.ddlVisit.DataBind()
                Me.ddlVisit.Items.Clear()
                Me.ddlVisit.Items.Insert(0, "Select Visit/Parent Activity")
                Me.ddlActivity.DataSource = Nothing
                Me.ddlActivity.DataBind()
                Me.ddlActivity.Items.Insert(0, "Select Activity")
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillddlPeriod")
            Return False
        End Try
    End Function

    Private Function FillddlActivity(SenderId As String) As Boolean
        Dim ds_WorkSpaceNodeDetail As New DataSet
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim strPeriod As String = String.Empty
        Dim IsParent As Boolean = True
        Try
            If Me.ddlVisit.SelectedIndex = 0 And SenderId = "ddlVisit" Then
                Me.ddlActivity.DataSource = Nothing
                Me.ddlActivity.DataBind()
                Me.ddlActivity.Items.Clear()
                Me.ddlActivity.Items.Insert(0, New ListItem("Select Activity", 0))
                Return True
            End If
            If SenderId = "ddlVisit" Then
                IsParent = False
            End If
            If ddlPeriod.SelectedValue = "All" Then
                For Each lst As ListItem In ddlPeriod.Items
                    If lst.Text <> "Select Period" And lst.Text <> "All" Then
                        strPeriod += lst.Text + ","
                    End If
                Next
                strPeriod = "IN (" + strPeriod.TrimEnd(",") + ")"
            Else
                strPeriod = "= " + ddlPeriod.SelectedValue
            End If
            wstr = "vWorkspaceId = '" & Me.HProjectId.Value.Trim() & "'"
            If Me.Session(S_ScopeNo).ToString <> Scope_ClinicalTrial Then
                wstr += " AND iPeriod " + strPeriod
            End If
            If IsParent Then
                wstr += " AND iParentNodeId = 1"
            Else
                If Me.ddlVisit.SelectedIndex <> 0 Then
                    wstr += " AND iParentNodeId = " + Me.ddlVisit.SelectedValue.Split("#")(0)
                End If
            End If
            'wstr += " AND cSubjectWiseFlag = '" & Me.rblSubjectSpecific.SelectedValue & "'"
            wstr += " AND cStatusIndi <> 'D' ORDER BY iParentNodeId,iNodeNo"

            If Not ObjHelp.GetViewWorkSpaceNodeDetail(wstr, ds_WorkSpaceNodeDetail, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From View_WorkSpaceNodeDetail:" + estr, Me.Page)
                Return False
            End If
            ds_WorkSpaceNodeDetail.Tables(0).Columns.Add("NodeActivityTime", GetType(Object), "iNodeId+'#'+vActivityId")
            If IsParent Then
                Me.ddlVisit.DataSource = ds_WorkSpaceNodeDetail.Tables(0)
                Me.ddlVisit.DataValueField = "NodeActivityTime"
                Me.ddlVisit.DataTextField = "vNodeDisplayName"
                Me.ddlVisit.DataBind()
                Me.ddlVisit.Items.Insert(0, New ListItem("Select Visit/Parent Activity", 0))
                Me.ddlActivity.DataSource = Nothing
                Me.ddlActivity.DataBind()
                Me.ddlActivity.Items.Clear()
                Me.ddlActivity.Items.Insert(0, New ListItem("Select Activity", 0))
            Else
                Me.ddlActivity.DataSource = ds_WorkSpaceNodeDetail.Tables(0)
                Me.ddlActivity.DataValueField = "NodeActivityTime"
                Me.ddlActivity.DataTextField = "vNodeDisplayName"
                Me.ddlActivity.DataBind()
                Me.ddlActivity.Items.Insert(0, New ListItem("Select Activity", 0))
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......FillddlActivity")
            Return False
        End Try

    End Function

    Private Function FillChkSubject() As Boolean
        Dim ds_WorkSpaceSubjectMst As New Data.DataSet
        Dim dv_WorkSpaceSubjectMst As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim dt_Filtter As DataTable
        Dim strPeriod As String = String.Empty
        Try
            If ddlPeriod.SelectedValue = "All" Then
                For Each lst As ListItem In ddlPeriod.Items
                    If lst.Text <> "Select Period" And lst.Text <> "All" Then
                       strPeriod += lst.Text + ","
                    End If
                Next
                strPeriod = "IN (" + strPeriod.TrimEnd(",") + ")"
            Else
                strPeriod = "= " + ddlPeriod.SelectedValue
            End If
            wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "'"
            'If Me.Session(S_ScopeNo).ToString <> Scope_ClinicalTrial Then
            '    wstr += " And iPeriod " + strPeriod + " And cStatusIndi <> 'D' AND len (iMySubjectNo) > 3 "
            'End If
            wstr += "AND cStatusIndi <> 'D'"
            If Not ObjHelp.GetViewWorkspaceSubjectMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_WorkSpaceSubjectMst, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data from View_WorkspaceSubjectMst:" + estr, Me.Page)
                Return False
            End If
            If ds_WorkSpaceSubjectMst.Tables(0).Rows.Count > 0 Then
                dv_WorkSpaceSubjectMst = ds_WorkSpaceSubjectMst.Tables(0).DefaultView
                dv_WorkSpaceSubjectMst.Sort = "FullNameWithNo"
                dt_Filtter = dv_WorkSpaceSubjectMst.ToTable().DefaultView.ToTable(True, "vWorkspaceSubjectId,iPeriod,FullNameWithNo,cRejectionFlag,InitialWithNo".Split(","))
                Me.ViewState(VS_ControlsSubject) = Nothing
                Me.ViewState(VS_ControlsSubject) = dt_Filtter
                If Not CreateSubject(estr) Then
                    Throw New Exception("Error While Create Subject ... " + estr)
                End If
                Me.trChkSubject.Style.Add("display", "")
            Else
                Me.trChkSubject.Style.Add("display", "none")
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "........FillChkSubject")
            Return False
        End Try
    End Function

    Private Function FillGrid() As Boolean
        Dim ds_CRFHdrDtlSubDtl As DataSet = Nothing
        Dim estr As String = String.Empty
        Dim Parameters As String = String.Empty
        Dim Subjects As String = String.Empty
        Dim CountSubject As Integer = 0
        Dim strSameActivity As String = String.Empty
        Dim strNodeId As String = String.Empty
        Dim strPeriod As String = String.Empty
        Try
            If Me.rblSubjectSpecific.SelectedValue = "Y" Then
                Subjects = Me.hdnSubjectID.Value
            Else
                Subjects = "0000"
            End If
            If ddlPeriod.SelectedValue = "All" Then
                For Each lst As ListItem In ddlPeriod.Items
                    If lst.Text <> "Select Period" And lst.Text <> "All" Then
                        strPeriod += lst.Text + ","
                    End If
                Next
                strPeriod = strPeriod.TrimEnd(",")
            Else
                strPeriod = ddlPeriod.SelectedValue
            End If
            'If Me.Session(S_ScopeNo).ToString <> Scope_ClinicalTrial Then
            '    strSameActivity = hdnNodeId.Value
            '    Parameters = Me.HProjectId.Value.Trim() + "##" + strSameActivity + "##" + Subjects + "##" + hdnColumnHeader.Value '+ strPeriod
            '    If Not ObjHelp.Proc_GetTabularDataForActivity(Parameters, ds_CRFHdrDtlSubDtl, estr) Then
            '        Throw New Exception("error while getting data from proc_gettabulardataforactivity. " + estr.ToString)
            '    End If
            'Else
            '    If CountSubject > 10 Then
            '        ObjCommon.ShowAlert("Maximum 10 subjects are allowed at once", Me.Page)
            '        Me.gvwMedExInfoHdr.DataSource = Nothing
            '        Me.gvwMedExInfoHdr.DataBind()
            '        Return True
            '    End If
            '    strSameActivity = hdnActivityID.Value
            '    strNodeId = hdnNodeId.Value
            '    Parameters = Me.HProjectId.Value.Trim() + "##" + strSameActivity + "##" + strNodeId + "##" + Subjects + "##" + hdnColumnHeader.Value '+ "##" + strPeriod
            '    If Not ObjHelp.Proc_GetTabularDataForActivity_CTM(Parameters, ds_CRFHdrDtlSubDtl, estr) Then
            '        Throw New Exception("Error While Getting Data From Proc_GetTabularDataForActivity_CTM. " + estr.ToString)
            '    End If
            'End If

            strSameActivity = hdnNodeId.Value
            Parameters = Me.HProjectId.Value.Trim() + "##" + strSameActivity + "##" + Subjects + "##" + hdnColumnHeader.Value '+ strPeriod
            If Not ObjHelp.Proc_GetTabularDataForActivity(Parameters, ds_CRFHdrDtlSubDtl, estr) Then
                Throw New Exception("error while getting data from proc_gettabulardataforactivity. " + estr.ToString)
            End If

            If Not ds_CRFHdrDtlSubDtl Is Nothing Then
                If ds_CRFHdrDtlSubDtl.Tables.Count Then
                    If ds_CRFHdrDtlSubDtl.Tables(0).Rows.Count > 0 Then
                        Me.gvwMedExInfoHdr.DataSource = ds_CRFHdrDtlSubDtl.Tables(0).DefaultView.ToTable
                        Me.gvwMedExInfoHdr.DataBind()
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "HideProjectDetail", "Display(" + img1.ClientID + ",'divProjectFilterDetail');", True)
                        Return True
                    Else
                        Me.gvwMedExInfoHdr.DataSource = Nothing
                        Me.gvwMedExInfoHdr.DataBind()
                        Me.ObjCommon.ShowAlert("No Data Available For Selected Criteria.", Me.Page)
                        Return True
                    End If
                Else
                    Me.gvwMedExInfoHdr.DataSource = Nothing
                    Me.gvwMedExInfoHdr.DataBind()
                    Me.ObjCommon.ShowAlert("No Data Available For Selected Criteria.", Me.Page)
                    Return True
                End If
            Else
                Me.gvwMedExInfoHdr.DataSource = Nothing
                Me.gvwMedExInfoHdr.DataBind()
                Me.ObjCommon.ShowAlert("No Data Available For Selected Criteria.", Me.Page)
                Return True
            End If
            If Not CreateControl(estr) Then
                Throw New Exception("Error while Create Same Activity ... " + estr)
            End If
            If Not CreateSubject(estr) Then
                Throw New Exception("Error While Create Subject ... " + estr)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillGrid")
            Return False
        End Try
    End Function

    Private Function CreateControl(ByRef eStr As String) As Boolean
        Dim dtPeriod As New DataTable
        Dim dtPeriodControl As New DataTable
        Dim dvPeriod As DataView
        Dim tbl As New HtmlTable
        Dim row As New HtmlTableRow
        Dim cell As HtmlTableCell
        Dim chkPeriod As New CheckBoxList
        Dim lstItem As New ListItem
        Dim Count As Integer = 0
        Dim pnl As Panel
        Dim IsAllActivitySelect As Boolean = True

        Try
            If Not CType(Me.ViewState(VS_Controls), DataSet) Is Nothing Then
                dtPeriodControl = CType(Me.ViewState(VS_Controls), DataSet).Tables(0)
                If Not dtPeriodControl Is Nothing Then
                    If dtPeriodControl.Rows.Count > 0 Then

                        dtPeriod = dtPeriodControl.DefaultView.ToTable(True, "iPeriod")
                        dtPeriod.DefaultView.Sort = "iPeriod ASC"

                        For Each dr As DataRow In dtPeriod.DefaultView.ToTable().Rows
                            IsAllActivitySelect = True
                            dvPeriod = dtPeriodControl.DefaultView
                            dvPeriod.RowFilter = "iPeriod = " + dr("iPeriod").ToString

                            Count += 1

                            cell = New HtmlTableCell
                            chkPeriod = New CheckBoxList

                            chkPeriod.ID = "Period_" + dr("iPeriod").ToString

                            For Each drItem As DataRow In dvPeriod.ToTable().Rows
                                lstItem = New ListItem
                                lstItem.Text = drItem("ActivityWithParent")
                                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                                    lstItem.Text = drItem("ActivityWithParent")
                                End If
                                lstItem.Attributes.Add("class", "chkActivity chkSelectUnSelect")
                                lstItem.Attributes.Add("NodeID", drItem("iNodeID").ToString() + "#" + drItem("vActivityId").ToString())
                                lstItem.Attributes.Add("title", "(" + drItem("vActivityId").ToString() + "#" + drItem("iNodeID").ToString() + ")")
                                lstItem.Attributes.Add("onclick", "fnCheckSelect(this);")
                                If Array.IndexOf(Me.hdnNodeId.Value.Split(","), Convert.ToString(drItem("iNodeID"))) <> -1 Then
                                    lstItem.Selected = True
                                Else
                                    IsAllActivitySelect = False
                                End If
                                chkPeriod.Items.Add(lstItem)
                            Next

                            chkPeriod.DataBind()
                            chkPeriod.Items.Insert(0, "Select All")
                            chkPeriod.Items(0).Attributes.Add("onclick", "fnSelectAll(this);")
                            If IsAllActivitySelect Then
                                chkPeriod.Items(0).Selected = True
                            End If

                            chkPeriod.Style.Add("text-align", "left")
                            chkPeriod.Style.Add("margin", "15px 10px 10px 10px")
                            chkPeriod.Style.Add("font-size", "8pt")

                            pnl = New Panel
                            pnl.ID = "Pnl_" + dr("iPeriod").ToString
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
                        Me.trChkActivity.Style.Add("display", "")
                    End If
                End If
            Else
                Me.trChkActivity.Style.Add("display", "none")
            End If
            
        Catch ex As Exception
            eStr = ex.ToString
            Return False
        End Try
        Return True
    End Function

    Private Function CreateSubject(ByRef eStr As String) As Boolean
        Dim dtSubjectFilter As New DataTable
        Dim dtSubjectControl As DataTable
        Dim dvPeriod As DataView
        Dim tbl As New HtmlTable
        Dim row As New HtmlTableRow
        Dim cell As HtmlTableCell
        Dim chkPeriod As New CheckBoxList
        Dim lstItem As New ListItem
        Dim Count As Integer = 0
        Dim pnl As Panel
        Dim IsAllSubjectSelect As Boolean = True
        Try
            dtSubjectControl = CType(Me.ViewState(VS_ControlsSubject), DataTable)
            If Not dtSubjectControl Is Nothing Then
                If dtSubjectControl.Rows.Count > 0 Then

                    dtSubjectFilter = dtSubjectControl.DefaultView.ToTable(True, "iPeriod")
                    dtSubjectFilter.DefaultView.Sort = "iPeriod ASC"

                    For Each dr As DataRow In dtSubjectFilter.DefaultView.ToTable().Rows
                        IsAllSubjectSelect = True
                        dvPeriod = dtSubjectControl.DefaultView
                        dvPeriod.RowFilter = "iPeriod = " + dr("iPeriod").ToString

                        Count += 1

                        cell = New HtmlTableCell
                        chkPeriod = New CheckBoxList

                        chkPeriod.ID = "Subject_Period_" + dr("iPeriod").ToString
                        chkPeriod.RepeatColumns = 5
                        chkPeriod.RepeatDirection = RepeatDirection.Horizontal
                        For Each i As DataRow In dvPeriod.ToTable().Rows
                            lstItem = New ListItem
                            lstItem.Text = i("InitialWithNo")
                            lstItem.Attributes.Add("class", "chkSubject chkSelectUnSelect")
                            lstItem.Attributes.Add("onclick", "fnCheckSelect(this);")
                            lstItem.Attributes.Add("SubjectId", Convert.ToString(i("vWorkspaceSubjectId")))
                            If Array.IndexOf(Me.hdnSubjectID.Value.Split(","), i("vWorkspaceSubjectId")) <> -1 Then
                                lstItem.Selected = True
                            Else
                                IsAllSubjectSelect = False
                            End If
                            chkPeriod.Items.Add(lstItem)
                        Next
                        chkPeriod.DataBind()
                        chkPeriod.Items.Insert(0, "Select All")
                        chkPeriod.Items(0).Attributes.Add("onclick", "fnSelectAll(this);")
                        If IsAllSubjectSelect Then
                            chkPeriod.Items(0).Selected = True
                        End If
                        chkPeriod.Style.Add("text-align", "left")
                        chkPeriod.Style.Add("margin", "15px 10px 10px 10px")
                        chkPeriod.Style.Add("font-size", "8pt")

                        pnl = New Panel
                        pnl.ID = "Subject_Pnl_" + dr("iPeriod").ToString
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
                    Me.tdSubject.Controls.Add(tbl)
                End If
            End If
        Catch ex As Exception
            eStr = ex.ToString
            Return False
        End Try
        Return True
    End Function

#End Region

#Region "SELECTION CHANGE EVENT"

    Protected Sub chkAllActivity_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAllActivity.CheckedChanged
        Dim dsPeriod As New DataSet
        Dim wStr As String = String.Empty
        Dim estr As String = String.Empty
        Try
            If chkAllActivity.Checked Then
                If Me.HProjectId.Value.Trim() = "" Then
                    objCommon.ShowAlert("Please Enter Project.", Me)
                    Me.chkAllActivity.Checked = False
                ElseIf Me.ddlPeriod.SelectedIndex = 0 Then
                    If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                        ObjCommon.ShowAlert("Please Select Period.", Me)
                        Me.chkAllActivity.Checked = False
                    ElseIf Me.ddlVisit.SelectedIndex = 0 Then
                        ObjCommon.ShowAlert("Please Select Visit/Parent Activity.", Me)
                        Me.chkAllActivity.Checked = False
                    ElseIf Me.ddlActivity.SelectedIndex = 0 Then
                        ObjCommon.ShowAlert("Please Select Activity.", Me)
                        Me.chkAllActivity.Checked = False
                    End If
                ElseIf Me.ddlVisit.SelectedIndex = 0 Then
                    ObjCommon.ShowAlert("Please Select Visit/Parent Activity.", Me)
                    Me.chkAllActivity.Checked = False
                ElseIf Me.ddlActivity.SelectedIndex = 0 Then
                    ObjCommon.ShowAlert("Please Select Activity.", Me)
                    Me.chkAllActivity.Checked = False
                End If
            End If

            If Me.chkAllActivity.Checked Then
                If Me.ddlActivity.SelectedValue.ToString.Trim() <> "" And _
                    Me.ddlActivity.SelectedValue.ToString.Trim().ToLower() <> "select activity" Then

                    dsPeriod = Me.ObjHelp.GetResultSet("select iNodeID,vNodeDisplayName,iPeriod,ActivityWithParent,vActivityId " + _
                                                         " from View_WorkspaceNodeDetail where vWorkspaceId='" + Me.HProjectId.Value + "'" + _
                                                         " And cStatusIndi<>'D' AND vActivityId = '" + ddlActivity.SelectedValue.Split("#")(1).ToString + "'" + _
                                                         " Order by nRefTime,iNodeID", "PeriodDetails")

                    Me.ViewState(VS_Controls) = dsPeriod
                    Me.hdnNodeId.Value = Me.ddlActivity.SelectedValue.Split("#")(0)
                    Me.hdnActivityID.Value = Me.ddlActivity.SelectedValue.Split("#")(1)
                    If Not CreateControl(estr) Then
                        Throw New Exception("Error while Create Same Activity ... " + estr)
                    End If
                End If
            Else
                Me.ViewState(VS_Controls) = Nothing
                Me.trChkActivity.Style.Add("display", "none")
                Me.hdnNodeId.Value = ""
                Me.hdnActivityID.Value = ""
            End If
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                If Not CreateSubject(estr) Then
                    Throw New Exception("Error While Create Subject ... " + estr)
                End If
            ElseIf Me.ddlPeriod.SelectedIndex <> 0 Then
                If Not CreateSubject(estr) Then
                    Throw New Exception("Error While Create Subject ... " + estr)
                End If
            End If
            Me.gvwMedExInfoHdr.DataSource = Nothing
            Me.gvwMedExInfoHdr.DataBind()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try

    End Sub

    Protected Sub ddlPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim estr As String = String.Empty
        Try
            Me.chkAllActivity.Checked = False
            Me.gvwMedExInfoHdr.DataSource = Nothing
            Me.gvwMedExInfoHdr.DataBind()
            If Not FillddlActivity(Convert.ToString(sender.id)) Then
                Exit Sub
            End If
            If Me.rblSubjectSpecific.SelectedIndex = 0 Then
                If Convert.ToString(sender.id) = "ddlPeriod" Or Me.ViewState(VS_ControlsSubject) Is Nothing Then
                    If Not FillChkSubject() Then
                        Exit Sub
                    End If
                    Me.trChkActivity.Style.Add("display", "none")
                Else
                    If Not CreateSubject(estr) Then
                        Throw New Exception("Error While Create Subject ... " + estr)
                    End If
                    Me.trChkSubject.Style.Add("display", "")
                End If
            Else
                Me.ViewState(VS_Controls) = Nothing
                Me.ViewState(VS_ControlsSubject) = Nothing
                Me.trChkActivity.Style.Add("display", "none")
                Me.trChkSubject.Style.Add("display", "none")
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try
    End Sub

    Protected Sub rblSubjectSpecific_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            ddlPeriod_SelectedIndexChanged(sender, e)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try
    End Sub

    Protected Sub ddlActivity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlActivity.SelectedIndexChanged
        Dim estr As String = String.Empty
        Try
            Me.gvwMedExInfoHdr.DataSource = Nothing
            Me.gvwMedExInfoHdr.DataBind()
            Me.chkAllActivity.Checked = False
            Me.hdnNodeId.Value = ""
            Me.hdnActivityID.Value = ""
            Me.ViewState(VS_Controls) = Nothing
            Me.trChkActivity.Style.Add("display", "none")
            If Not CreateSubject(estr) Then
                Throw New Exception("Error While Create Subject ... " + estr)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "ddlActivity_SelectedIndexChanged")
        End Try
    End Sub

    Protected Sub ddlVisit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlVisit.SelectedIndexChanged
        Try
            ddlPeriod_SelectedIndexChanged(sender, e)
            Me.ddlActivity.Focus()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try
    End Sub

#End Region

#Region "BUTTON EVENT"

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim estr As String = String.Empty
        Try
            If Not FillGrid() Then
                Exit Sub
            End If
            If Not CreateControl(estr) Then
                Throw New Exception("Error while Create Same Activity ... " + estr)
            End If
            If Not CreateSubject(estr) Then
                Throw New Exception("Error While Create Subject ... " + estr)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......btnSearch_Click")
        End Try
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("frmMainPage.aspx", False)
    End Sub

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Me.ViewState(VS_DtCRFHdrDtlSubDtl) = Nothing
            Me.ViewState(VS_ControlsSubject) = Nothing
            Me.ViewState(VS_ControlsSubject) = Nothing
            Me.chkAllActivity.Checked = False
            Me.ddlVisit.DataSource = Nothing
            Me.ddlVisit.DataBind()
            Me.ddlVisit.Items.Clear()
            Me.ddlVisit.Items.Insert(0, "Select Visit/Parent Activity")

            Me.ddlActivity.DataSource = Nothing
            Me.ddlActivity.DataBind()
            Me.ddlActivity.Items.Clear()
            Me.ddlActivity.Items.Insert(0, "Select Activity")

            Me.ddlPeriod.DataSource = Nothing
            Me.ddlPeriod.DataBind()
            Me.ddlPeriod.Items.Clear()
            Me.ddlPeriod.Items.Insert(0, New ListItem("Select Period", 0))

            Me.gvwMedExInfoHdr.DataSource = Nothing
            Me.gvwMedExInfoHdr.DataBind()
            If Not Me.GenCall_ShowUI() Then
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......btnSetProject_Click")
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Response.Redirect("frmSubjectWiseEntry.aspx")
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim curContext As System.Web.HttpContext = System.Web.HttpContext.Current
            Dim strWriter As System.IO.StringWriter = Nothing
            Dim htmlWriter As System.Web.UI.HtmlTextWriter = Nothing
            Dim gridViewhtml As String = String.Empty
            If gvwMedExInfoHdr.DataSource Is Nothing Then
                curContext.Response.Clear()
                curContext.Response.Buffer = True
            End If
            curContext.Response.AddHeader("content-disposition", "attachment; filename=" + System.Web.HttpUtility.UrlEncode("ActivityDataInTableFormat", System.Text.Encoding.UTF8) + ".xls")
            curContext.Response.ContentType = "application/vnd.ms-excel"
            strWriter = New System.IO.StringWriter()
            htmlWriter = New System.Web.UI.HtmlTextWriter(strWriter)
            gridViewhtml = "<table><tr><td colspan=""10""><center><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">Lambda Therapeutic Research </font></strong><center></td></tr>"
            gridViewhtml += "<tr><td colspan=""10""><center><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">Project No : " + Me.txtproject.Text.Trim() + "</font></strong><center></td></tr>"
            gridViewhtml += "<tr><td colspan=""10""><center><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">Activity : " + Me.ddlActivity.SelectedItem.Text.Trim() + "</font></strong><center></td></tr>"
            gridViewhtml += "</table>"
            gridViewhtml += strWriter.ToString()
            Context.Response.Write(gridViewhtml)
            gvwMedExInfoHdr.RenderControl(htmlWriter)
            curContext.Response.Write(strWriter.ToString())
            curContext.Response.Flush()
            curContext.Response.End()
        Catch ex As Threading.ThreadAbortException
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "btnExport_Click")
        End Try
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        Return
    End Sub

#End Region

#Region "Reset Page"

    Protected Sub ResetPage()
        Me.ViewState(VS_DtCRFHdrDtlSubDtl) = Nothing
        Me.ddlActivity.SelectedIndex = 0
        Me.ddlPeriod.SelectedIndex = 0
        Me.txtproject.Text = ""
        Me.HProjectId.Value = ""
        Me.gvwMedExInfoHdr.DataSource = Nothing
        Me.gvwMedExInfoHdr.DataBind()
        If Not GenCall() Then
            Exit Sub
        End If
    End Sub

    Protected Sub ResetDetail()
        ddlActivity.DataSource = Nothing
        ddlActivity.DataBind()
        ddlActivity.Items.Clear()
        Me.ddlActivity.Items.Insert(0, "Select Activity")

        ddlVisit.DataSource = Nothing
        ddlVisit.DataBind()
        ddlVisit.Items.Clear()
        Me.ddlVisit.Items.Insert(0, "Select Visit/Parent Activity")
    End Sub

#End Region

#Region "GRID EVENTS"

    Protected Sub gvwMedExInfoHdr_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwMedExInfoHdr.RowDataBound
        Dim HdrName As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            Dim headerArr() As String
            For HdrName = 0 To e.Row.Cells.Count - 1
                If Me.Session(S_ScopeNo).ToString <> Scope_ClinicalTrial Then
                    headerArr = e.Row.Cells(HdrName).Text.Split("#")
                Else
                    headerArr = e.Row.Cells(HdrName).Text.Split(New String() {"#39;"}, StringSplitOptions.None)
                End If
                e.Row.Cells(HdrName).Text = ""
                If headerArr.Length > 1 Then
                    For count = 0 To headerArr.Length - 2
                        e.Row.Cells(HdrName).Text += Convert.ToString(headerArr(count))
                    Next
                    If headerArr(headerArr.Length - 1).Contains("#") Then
                        e.Row.Cells(HdrName).Text += headerArr(headerArr.Length - 1).Split("#")(0)
                    End If
                Else
                    e.Row.Cells(HdrName).Text = Convert.ToString(headerArr(0).Split("#")(0))
                End If
                e.Row.Cells(HdrName).Text = e.Row.Cells(HdrName).Text.Replace("&", "'")
                ''e.Row.Cells(HdrName).Text = e.Row.Cells(HdrName).Text.Split("#")(0).ToString
            Next
        End If
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