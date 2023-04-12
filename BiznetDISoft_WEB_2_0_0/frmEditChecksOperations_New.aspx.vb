
Partial Class frmEditChecksOperations
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objCommon As New clsCommon
    Private objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

    Private VS_Choice As String = "Choice"
    Private VS_DtCRFDetail As String = "DtCRFDetail"

    Private VS_DtEditChecksHdr As String = "DtEditChecksHdr"
    Private VS_DtEditChecksDtl As String = "DtEditChecksDtl"

    Private VS_DtGridHdr As String = "DtGridHdr"
    Private VS_DtGridHdrDtl As String = "DtGridHdrDtl"

    Private Const GVCHdr_SrNo As Integer = 0
    'Private Const GVCHdr_EditChecksHdrNo As Integer = 1
    Private Const GVCHdr_vSubjectId As Integer = 1
    Private Const GVCHdr_WorkspaceId As Integer = 2
    Private Const GVCHdr_Initials As Integer = 3
    Private Const GVCHdr_ScreenNo As Integer = 4
    Private Const GVCHdr_RandomizationNo As Integer = 5
    'Private Const GVCHdr_TranNo As Integer = 6
    'Private Const GVCHdr_Activity As Integer = 7
    Private Const GVCHdr_LoginName As Integer = 6
    Private Const GVCHdr_FiredDate As Integer = 7
    Private Const GVCHdr_View As Integer = 8
    Private Const GVCHDR_RejectionFlag As Integer = 9

    Private Const GVCDtl_SrNo As Integer = 0
    Private Const GVCDtl_EditChecksHdrNo As Integer = 1
    Private Const GVCDtl_EditChecksDtlNo As Integer = 2
    Private Const GVCDtl_ScreenNo As Integer = 3
    'Private Const GVCDtl_ParentActivity As Integer = 4
    Private Const GVCDtl_Activity As Integer = 4
    Private Const GVCDtl_CrossActivity As Integer = 5
    Private Const GVCDtl_Repeatation As Integer = 6
    Private Const GVCDtl_FireDate As Integer = 7
    Private Const GVCDtl_QueryValue As Integer = 8
    Private Const GVCDtl_Remarks As Integer = 9
    Private Const GVCDtl_GenerateQueryFlag As Integer = 10
    Private Const GVCDtl_GenerateQueryCheckBox As Integer = 11
    Private Const GVCDtl_GenerateQueryRemarks As Integer = 12
    Private Const GVCDtl_ResolveQueryFlag As Integer = 13
    Private Const GVCDtl_ResolveQueryCheckBox As Integer = 14
    Private Const GVCDtl_ResolveQueryRemarks As Integer = 15
    Private Const GVCDtl_MySubjectNo As Integer = 16
    Private Const GVCDtl_Period As Integer = 17
    Private Const GVCDtl_vActivityId As Integer = 18
    Private Const GVCDtl_iNodeId As Integer = 19
    Private Const GVCDtl_vSubjectId As Integer = 20
    Private Const GVCDtl_SourceNodeId_If As Integer = 21
    Private Const GVCDtl_CrossActivity_NodeId As Integer = 22
    Private Const GVCDtl_CrossActivity_ActivityId As Integer = 23
    Private Const GVCDtl_CrossActivity_Period As Integer = 24

    Private Const Vs_EditchecksHdr As String = "EditchecksHdr"
    Private Const Vs_EditchecksDtl As String = "EditchecksDtl"

    Shared Shared_Initial As String = String.Empty
    Shared Shared_ScreenNo As String = String.Empty
    Shared Shared_RandomizationNo As String = String.Empty

    Private rPage As RepoPage

    Dim estr As String = Nothing

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            If Not Me.GENCALL_DATA() Then
                Exit Sub
            End If
        End If
    End Sub

#End Region

#Region "GENCALL_DATA"

    Private Function GENCALL_DATA() As Boolean
        Dim ds_EditChecksHdr As DataSet = Nothing
        Dim ds_EditChecksDtl As DataSet = Nothing
        Dim eStr As String = String.Empty
        Dim sender As New Object
        Dim e As New EventArgs
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            'Added on 09-Apr-10 to show the set Project on page load
            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
            End If

            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
            '========================================================================

            If Not Me.objhelpDb.GetEditChecksHdr("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                                         ds_EditChecksHdr, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_EditChecksHdr Is Nothing Then
                Throw New Exception(eStr)
            End If

            Me.ViewState(VS_DtEditChecksHdr) = ds_EditChecksHdr.Tables(0)


            If Not Me.objhelpDb.GetEditChecksDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                                         ds_EditChecksDtl, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_EditChecksDtl Is Nothing Then
                Throw New Exception(eStr)
            End If

            Me.ViewState(VS_DtEditChecksDtl) = ds_EditChecksDtl.Tables(0)

            If Not Me.GENCALL_ShowUI() Then
                Exit Function
            End If

            GENCALL_DATA = True

        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)
            Return False
        End Try
    End Function

#End Region

#Region "GENCALL_ShowUI"

    Private Function GENCALL_ShowUI() As Boolean
        Dim eStr As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim sender As New Object
        Dim e As New EventArgs
        Try
            Page.Title = " :: Edit Checks Operations  ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            Me.ViewState(VS_Choice) = Choice

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Edit Checks Operations"
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            'Added on 09-Apr-10 to show the set Project on page load
            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
            End If

            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
            '========================================================================

            GENCALL_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)
            Return False
        End Try
    End Function

#End Region

#Region "Fill Visits"

    Private Function FillVisits() As Boolean
        Dim Ds_Visits As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            wStr = " cStatusIndi <> 'D'"
            wStr += " And vWorkspaceId = '" & Me.HProjectId.Value.Trim() & "' "
            wStr += " And iParentNodeId = 1"
            wStr += " And ISNULL(vTemplateId,'') <> '0001' And ("
            wStr += " iNodeId in (Select distinct iNodeId From View_MedExWorkSpaceHdr "
            wStr += " Where vWorkspaceId = '" & Me.HProjectId.Value.Trim() & "' "
            wStr += " And cStatusIndi <> 'D' And upper(IsNull(vMedExType,'')) <> 'IMPORT') "
            wStr += " Or iNodeId in (Select distinct iParentNodeId From View_MedExWorkSpaceHdr "
            wStr += " Where vWorkspaceId = '" & Me.HProjectId.Value.Trim() & "' "
            wStr += " And cStatusIndi <> 'D' And upper(IsNull(vMedExType,'')) <> 'IMPORT') "
            wStr += " ) order by iNodeNo"

            If Not Me.objhelpDb.getWorkSpaceNodeDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                Ds_Visits, eStr) Then
                Throw New Exception(eStr)
            End If

            Ds_Visits.Tables(0).Columns.Add("NodeIdActivityId")
            Ds_Visits.AcceptChanges()

            For Each dr As DataRow In Ds_Visits.Tables(0).Rows
                dr("NodeIdActivityId") = dr("iNodeId").ToString() & "#" & dr("vActivityId").ToString() & "#" & dr("vActivityId").ToString()
            Next dr
            Ds_Visits.AcceptChanges()

            Me.ddlVisit.DataSource = Ds_Visits.Tables(0)
            Me.ddlVisit.DataTextField = "vNodeDisplayName"
            Me.ddlVisit.DataValueField = "NodeIdActivityId"
            Me.ddlVisit.DataBind()
            Me.ddlVisit.Items.Insert(0, "All Visits/Parent Activities")
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage("Problem While Filling Visits", ex.Message)
            Return False
        End Try
    End Function

#End Region

#Region "Get CRF Detail"

    Private Function GetCRFDetail(ByRef dt_CRFDetail As DataTable) As Boolean
        Dim ds_CRFDetail As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim Subjects As String = String.Empty

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            wStr = "vWorkspaceId = '" & Me.HProjectId.Value.Trim() & "' And cCRFDtlDataStatus <> 'B' "

            If Not Me.objhelpDb.View_CRFHdrDtlSubDtl_Edit(wStr, "vNodeDisplayName,vSubjectId,vDefaultValue,vMedExType,iNodeId,vMedExCode,iRepeatNo,nCRFDtlNo,iPeriod,vActivityId,cIsRepeatable", ds_CRFDetail, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_CRFDetail Is Nothing Then
                Throw New Exception("Error While Getting CRF Detail")
            End If

            dt_CRFDetail = ds_CRFDetail.Tables(0)

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Problem While Getting CRF Detail", ex.Message)
            Return False
        End Try
    End Function

#End Region

#Region "Fill Grids"

    Private Function FillEditChecksHdrGrid() As Boolean
        Dim NodeId As Integer = 0
        Dim eStr As String = String.Empty
        Dim ds_EditChecksHdr As New DataSet

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Me.GVEditChecksHdr.DataSource = Nothing
            Me.GVEditChecksHdrDtl.DataSource = Nothing
            Me.GVEditChecksHdrDtl.DataBind()

            If Me.ddlVisit.SelectedIndex > 0 Then
                NodeId = Me.ddlVisit.SelectedValue.Substring(0, Me.ddlVisit.SelectedValue.IndexOf("#"))
            End If

            If Not Me.objhelpDb.Get_EditChecksHdr_MaxTran(Me.HProjectId.Value.Trim(), NodeId, ds_EditChecksHdr, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_EditChecksHdr.Tables(0).Rows.Count > 0 Then
                Me.Btn_viewAll.Style.Add("display", "Block")
                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "CreateGridHeaderForHdr", "CreateGridHeaderForHdr();", True)
            End If

            Me.GVEditChecksHdr.DataSource = ds_EditChecksHdr.Tables(0)
            Me.GVEditChecksHdr.DataBind()
            Me.pnlHdr.Attributes.Add("BorderStyle", "Double")

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Filling Grid", ex.Message)
            Return False
        End Try

    End Function

    Private Function FillEditChecksDtlGrid(ByVal SubjectId As String, Optional ByVal Initial As String = "", _
                                           Optional ByVal ScreenNo As String = "", Optional ByVal RandomizationNo As String = "") As Boolean
        Dim NodeId As Integer = 0
        Dim eStr As String = String.Empty
        Dim ds_EditChecksDtl As New DataSet

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Me.GVEditChecksHdrDtl.DataSource = Nothing

            If Me.ddlVisit.SelectedIndex > 0 Then
                NodeId = Me.ddlVisit.SelectedValue.Substring(0, Me.ddlVisit.SelectedValue.IndexOf("#"))
            End If

            If Not Me.objhelpDb.Get_EditChecksHdrDtl(Me.HProjectId.Value.Trim(), SubjectId, NodeId, ds_EditChecksDtl, eStr) Then
                Throw New Exception(eStr)
            End If

            Me.ViewState(VS_DtGridHdrDtl) = ds_EditChecksDtl.Tables(0)
            Me.GVEditChecksHdrDtl.DataSource = ds_EditChecksDtl.Tables(0)
            Me.GVEditChecksHdrDtl.DataBind()
            Me.btnExportToExcell.Style.Add("display", "none")

            If Me.GVEditChecksHdrDtl.Rows.Count > 0 Then
                Me.btnExportToExcell.Style.Add("display", "")
            End If
            Initial = Shared_Initial
            ScreenNo = Shared_ScreenNo
            RandomizationNo = Shared_RandomizationNo

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Filling Grid", ex.Message)
            Return False
        End Try

    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_Check As New DataSet
        Try

            wStr = "vWorkspaceId = '" & Me.HProjectId.Value.Trim() & "' And cStatusIndi <> 'D' Order By iTranNo desc"

            If Not Me.objhelpDb.GetCRFLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_Check, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_Check Is Nothing Then

                If ds_Check.Tables(0).Rows.Count > 0 Then
                    If ds_Check.Tables(0).Rows(0).Item("cLockFlag").ToString.Trim.ToUpper() = "L" Then
                        Me.objCommon.ShowAlert("Site is Locked.", Me.Page)
                        Me.txtproject.Text = ""
                        Me.HProjectId.Value = ""
                        Exit Sub
                    End If

                End If

            End If

            If Not Me.FillVisits() Then
                Exit Sub
            End If

            'If Not Me.FillSubjects() Then
            '    Exit Sub
            'End If

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Getting CRF Lock Details ", ex.Message)
        End Try
    End Sub

    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_MedExEditChecks As New DataSet
        Dim ds_ParentWorkspace As New DataSet
        Dim SelectedVisit As String = String.Empty
        Dim ActivityId(0) As String
        Dim dt_CRFDetail As DataTable = Nothing

        Try
            Me.GVEditChecksHdrDtl.DataSource = Nothing
            Me.GVEditChecksHdrDtl.DataBind()

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not Me.GetCRFDetail(dt_CRFDetail) Then
                Exit Sub
            End If

            wStr = "vWorkspaceId = '" & Me.HProjectId.Value.Trim() & "'"
            wStr += " And cStatusIndi <> 'D'"

            If Not Me.objhelpDb.getworkspacemst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_ParentWorkspace, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_ParentWorkspace.Tables(0).Rows.Count < 1 Then
                Exit Sub
            End If

            wStr = "vWorkspaceId = '" & Convert.ToString(ds_ParentWorkspace.Tables(0).Rows(0)("vWorkspaceId")).Trim() & "'"
            If ds_ParentWorkspace.Tables(0).Rows(0)("cWorkspaceType").Trim() <> "P" Then
                wStr = "vWorkspaceId = '" & Convert.ToString(ds_ParentWorkspace.Tables(0).Rows(0)("vParentWorkspaceId")).Trim() & "'"
            End If

            If Me.ddlVisit.SelectedIndex > 0 Then
                SelectedVisit = Me.ddlVisit.SelectedValue.Substring(0, Me.ddlVisit.SelectedValue.IndexOf("#"))
                wStr += " And (iParentNodeId = " & SelectedVisit
                wStr += " Or iSourceNodeId_If = " & SelectedVisit & " )"
            End If
            'End If
            wStr += " And cStatusIndi <> 'D'"

            If Not Me.objhelpDb.GetMedExEditChecks(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition _
                                    , ds_MedExEditChecks, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_MedExEditChecks Is Nothing Then
                Throw New Exception("Error While Getting Edit Checks Details")
            End If

            If ds_MedExEditChecks.Tables(0).Rows.Count < 1 Then
                Me.objCommon.ShowAlert("No Edit Checks Entered", Me.Page)
                Exit Sub
            End If

            ActivityId(0) = Me.ddlVisit.SelectedValue.ToString.Trim()
            Me.ddlVisit.SelectedValue.ToString.Trim()

            Me.FireEditChecks(ActivityId, ds_MedExEditChecks.Tables(0), dt_CRFDetail)

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Show", "ShowConfirmation();", True)
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Processing : " & ex.Message, "")
        End Try

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.ResetPage()
        If Not Me.GENCALL_ShowUI() Then
            Exit Sub
        End If
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnView.Click
        Me.btnExportToExcell.Style.Add("display", "none")
        If Not Me.FillEditChecksHdrGrid() Then
            Exit Sub
        End If

    End Sub

    Protected Sub Btn_viewAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_viewAll.Click
        Dim eStr As String = Nothing
        Dim wStr As String = Nothing
        Dim NodeId As Integer = 0
        Dim ds_EditChecksDtl As DataSet = Nothing

        Try
            If Me.ddlVisit.SelectedIndex > 0 Then
                NodeId = Me.ddlVisit.SelectedValue.Substring(0, Me.ddlVisit.SelectedValue.IndexOf("#"))
            End If
            'If Not Me.objhelpDb.View_EditChecksHdrDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '                ds_EditChecksDtl, eStr) Then
            '    Throw New Exception(eStr)
            'End If
            If Not Me.objhelpDb.Get_EditChecksHdrDtl(Me.HProjectId.Value.Trim(), 0, NodeId, ds_EditChecksDtl, eStr) Then
                Throw New Exception(eStr)
            End If

            Me.ViewState(VS_DtGridHdrDtl) = ds_EditChecksDtl.Tables(0)
            Me.GVEditChecksHdrDtl.DataSource = ds_EditChecksDtl.Tables(0)
            Me.GVEditChecksHdrDtl.DataBind()
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "CreateGridHeaderForDtl", "CreateGridHeaderForDtl();", True)

            'lblInitial.Text = "No Query Found For Patient With - "
            Me.btnExportToExcell.Style.Add("display", "none")

            If Me.GVEditChecksHdrDtl.Rows.Count > 0 Then
                'Me.pnlDtl.Attributes.Add("BorderStyle", "Double")
                Me.btnExportToExcell.Style.Add("display", "")

            End If
            'lblInitial.Text = ""
            'lblScreenNo.Text = ""
            'lblRandomizationNo.Text = ""



        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message(), eStr)
        End Try
    End Sub

    Protected Sub btnExportToExcell_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcell.Click
        Dim fileName As String = ""
        Dim ds As New DataSet
        Try

            If Me.GVEditChecksHdrDtl.Rows.Count < 1 Then
                Me.objCommon.ShowAlert("No data to Export", Me.Page)
                Exit Sub
            End If
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            fileName = "EditChecks"
            fileName = fileName & ".xls"
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" & fileName)

            ds.Tables.Add(CType(Me.ViewState(VS_DtGridHdrDtl), DataTable).Copy())
            ds.AcceptChanges()

            Context.Response.Write(ConvertDsTO(ds))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "Fire Edit Checks"

    Private Sub FireEditChecks(ByVal ActivityId As String(), ByRef dt_MedExEditChecks_Ref As DataTable, _
                               ByRef dt_CRFDetail As DataTable)
        Dim dt_EditChecksHdr As New DataTable
        Dim dr_EditChecksHdr As DataRow
        Dim dt_EditChecksDtl As New DataTable
        Dim dt_MedExEditChecks As DataTable
        Dim dr_EditChecksDtl As DataRow

        Dim dv_EditChecksHdr As New DataView
        Dim dv_EditChecksDtl As New DataView
        Dim dv_CRFDetail As New DataView

        Dim ds_Save As New DataSet
        Dim ds_EditChecksHdrDtl As New DataSet

        Dim Source_MedExValue_If As String = String.Empty
        Dim Source_MedExType_If As String = String.Empty
        Dim Target_MedExValue_If As String = String.Empty
        Dim Target_MedExType_If As String = String.Empty

        Dim Source_MedExValue_Then As String = String.Empty
        Dim Source_MedExType_Then As String = String.Empty
        Dim Target_MedExValue_Then As String = String.Empty
        Dim Target_MedExType_Then As String = String.Empty

        Dim filter As String = String.Empty
        Dim Op_If As String = String.Empty
        Dim Op_Then As String = String.Empty

        Dim counter As Integer = 0
        Dim Is_Query As Boolean = False
        Dim NotToFire As Boolean = False

        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim Subject_Count As Integer = 0
        Dim RowFilter As String = String.Empty

        Dim dr_Detail() As DataRow
        Dim dr_DetailThen() As DataRow

        Dim repeatNoSourceIF As Integer = 0
        Dim repeatNoTargetIF As Integer = 0
        Dim repeatNoSourceThen As Integer = 0
        Dim repeatNoTargetThen As Integer = 0

        Dim ActivitySourceIF As String = String.Empty
        Dim ActivityTargetIF As String = String.Empty
        Dim ActivitySourceThen As String = String.Empty
        Dim ActivityTargetThen As String = String.Empty


        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            dt_EditChecksHdr = CType(Me.ViewState(VS_DtEditChecksHdr), DataTable)
            dt_EditChecksHdr.Rows.Clear()
            dt_EditChecksDtl = CType(Me.ViewState(VS_DtEditChecksDtl), DataTable)
            dt_EditChecksDtl.Rows.Clear()

            dv_CRFDetail = dt_CRFDetail.DefaultView.ToTable(True, "vSubjectId").DefaultView

            '''''To have all the parent activities in loop
            If Me.ddlVisit.SelectedIndex = 0 Then
                Array.Resize(ActivityId, Me.ddlVisit.Items.Count - 1)
                For i As Integer = 1 To Me.ddlVisit.Items.Count - 1
                    ActivityId(i - 1) = Me.ddlVisit.Items(i).Value.ToString()
                Next i
            End If

            ''''''''''Parent activity array:Start
            For i As Integer = 0 To ActivityId.Length - 1 '.ToArray.Length
                dt_MedExEditChecks = dt_MedExEditChecks_Ref.Copy()

                RowFilter = " (iParentNodeId = " & ActivityId(i).Substring(0, ActivityId(i).IndexOf("#"))
                RowFilter += " Or iSourceNodeId_If = " & ActivityId(i).Substring(0, ActivityId(i).IndexOf("#")) & " )"

                dt_MedExEditChecks.DefaultView.RowFilter = RowFilter
                dt_MedExEditChecks = dt_MedExEditChecks.DefaultView.ToTable()

                '''''''''''If the current parent node have no data continue with next
                If dt_MedExEditChecks.Rows.Count < 1 Then
                    Continue For
                End If

                '''''''''''''Loop of subjects :Start
                For Each drSubject As DataRow In dv_CRFDetail.ToTable().Rows
                    dt_EditChecksHdr.Rows.Clear()
                    dt_EditChecksDtl.Rows.Clear()

                    'Count subject to use in hdrno
                    Subject_Count = Subject_Count + 1

                    '''''''''''For loop for edit checks :Start
                    For Each dr As DataRow In dt_MedExEditChecks.Rows
                        Op_If = String.Empty
                        NotToFire = False
                        counter += 1

                        filter = "iNodeId = " & Convert.ToString(dr("iSourceNodeId_If")) & " And vMedExCode = '"
                        filter += Convert.ToString(dr("vSourceMedExCode_If")) & "'"
                        filter += " And vSubjectId = '" & drSubject("vSubjectId") & "'"
                        dr_Detail = dt_CRFDetail.Select(filter)

                        'Condition added to check if data entry is not done
                        If Convert.ToString(dr("iSourceNodeId_If")) <> "" And dr_Detail.Length < 1 Then
                            NotToFire = True
                            Continue For
                        End If
                        dr_EditChecksHdr = dt_EditChecksHdr.NewRow()
                        dr_EditChecksHdr("nEditChecksHdrNo") = Me.HProjectId.Value.Trim() + Convert.ToString(dr("iSourceNodeId_If")) + Subject_Count ' + Convert.ToString(dr("iSourceNodeId_If"))
                        dr_EditChecksHdr("vWorkspaceId") = Me.HProjectId.Value.Trim()
                        dr_EditChecksHdr("iPeriod") = Convert.ToInt32(dr_Detail(0).Item("iPeriod"))
                        dr_EditChecksHdr("iNodeId") = Convert.ToString(dr("iSourceNodeId_If"))
                        dr_EditChecksHdr("vActivityId") = Convert.ToString(dr_Detail(0).Item("vActivityId"))
                        dr_EditChecksHdr("vSubjectId") = drSubject("vSubjectId")
                        dr_EditChecksHdr("iTranNo") = 1
                        dr_EditChecksHdr("iModifyBy") = Me.Session(S_UserID)
                        dt_EditChecksHdr.Rows.Add(dr_EditChecksHdr)
                        dt_EditChecksHdr.AcceptChanges()

                        Op_If = Convert.ToString(dr("vOperator_If")).Trim()

                        ''''''''''1st loop:Start
                        For indexIf As Integer = 0 To dr_Detail.Length - 1
                            repeatNoSourceIF = 0
                            ActivitySourceIF = String.Empty
                            Source_MedExType_If = String.Empty
                            Source_MedExValue_If = String.Empty
                            Target_MedExValue_If = String.Empty

                            '''''''''''if repeatitions
                            If dr_Detail.Length > 1 Then
                                repeatNoSourceIF = Convert.ToInt32(dr_Detail(indexIf)("iRepeatNo"))
                                ActivitySourceIF = Convert.ToString(dr_Detail(indexIf)("vNodeDisplayName")).Trim()
                            End If

                            Source_MedExValue_If = Convert.ToString(dr_Detail(indexIf)("vDefaultValue")).Trim()
                            Source_MedExType_If = Convert.ToString(dr_Detail(indexIf)("vMedExType")).Trim()
                            '''''''''''''''''Set target values
                            Target_MedExValue_If = Convert.ToString(dr("vTargetValue_If")).Trim()

                            '''''''''''''''target_if exists or not:Start
                            If Convert.ToString(dr("vTargetMedExCode_If")).Trim() <> "" Then
                                Dim dr_TargetDetail() As DataRow
                                Dim dr_TargetCRFDetail() As DataRow
                                filter = "iNodeId = " & Convert.ToString(dr("iTargetNodeId_If")) & " And vMedExCode = '"
                                filter += dr("vTargetMedExCode_If") & "'"
                                filter += " And vSubjectId = '" & drSubject("vSubjectId") & "'"
                                dr_TargetCRFDetail = dt_CRFDetail.Select(filter)
                                dr_TargetDetail = dr_TargetCRFDetail

                                'Condition added to check if data entry is not done
                                If Convert.ToString(dr("vTargetMedExCode_If")).Trim() <> "" And dr_TargetCRFDetail.Length < 1 Then
                                    NotToFire = True
                                    Continue For
                                End If

                                If dr_Detail(indexIf)("cIsRepeatable").ToString = "Y" And dr_TargetCRFDetail(0)("cIsRepeatable").ToString = "Y" Then
                                    filter = "iNodeId = " & Convert.ToString(dr("iTargetNodeId_If")) & " And vMedExCode = '"
                                    filter += dr("vTargetMedExCode_If") & "' And nCRFDtlNo = " & Convert.ToString(dr_TargetCRFDetail(0)("nCRFDtlNo"))
                                    filter += " And iRepeatNo = " & Convert.ToString(dr_Detail(indexIf)("iRepeatNo"))
                                    filter += " And vSubjectId = '" & drSubject("vSubjectId") & "'"
                                    dr_TargetDetail = dt_CRFDetail.Select(filter)
                                End If

                                For indexTargetIf As Integer = 0 To dr_TargetDetail.Length - 1

                                    repeatNoTargetIF = 0
                                    ActivityTargetIF = String.Empty
                                    Target_MedExValue_If = String.Empty
                                    Target_MedExType_If = String.Empty

                                    Target_MedExValue_If = Convert.ToString(dr_TargetDetail(indexTargetIf)("vDefaultValue"))
                                    If dr_TargetDetail.Length > 1 And dr_Detail(indexIf)("cIsRepeatable").ToString = "Y" Then
                                        repeatNoTargetIF = Convert.ToString(dr_TargetDetail(indexTargetIf)("iRepeatNo"))
                                        ActivityTargetIF = Convert.ToString(dr_TargetDetail(indexTargetIf)("vNodeDisplayName"))
                                        Target_MedExValue_If = Convert.ToString(dr_TargetDetail(indexTargetIf)("vDefaultValue"))
                                        Target_MedExType_If = Convert.ToString(dr_TargetDetail(indexTargetIf)("vMedExType"))
                                        Exit For
                                    End If

                                    ''''''''''''Source_Then exists or not:Start
                                    Source_MedExValue_Then = String.Empty
                                    Source_MedExType_Then = String.Empty
                                    Target_MedExValue_Then = String.Empty
                                    Target_MedExType_Then = String.Empty
                                    Op_Then = String.Empty

                                    If Convert.ToString(dr("iSourceNodeId_Then")).Trim() <> "" AndAlso _
                                                                    Convert.ToString(dr("iSourceNodeId_Then")).Trim() <> "0" Then

                                        Dim dr_SourceCRFDetailThen() As DataRow
                                        filter = "iNodeId = " & Convert.ToString(dr("iSourceNodeId_Then")) & " And vMedExCode = '"
                                        filter += Convert.ToString(dr("vSourceMedExCode_Then")) & "'"
                                        filter += " And vSubjectId = '" & drSubject("vSubjectId") & "'"
                                        dr_SourceCRFDetailThen = dt_CRFDetail.Select(filter)

                                        Op_Then = Convert.ToString(dr("vOperator_Then")).Trim()


                                        'Condition added to check if data entry is not done
                                        If dr_SourceCRFDetailThen.Length < 1 Then
                                            NotToFire = True
                                            Continue For
                                        End If
                                        repeatNoSourceThen = 0
                                        dr_DetailThen = dr_SourceCRFDetailThen
                                        If dr_SourceCRFDetailThen(0)("cIsRepeatable") = "Y" And dr_Detail(indexIf)("cIsRepeatable").ToString = "Y" Then
                                            filter = "iNodeId = " & Convert.ToString(dr("iSourceNodeId_Then")) & " And vMedExCode = '"
                                            filter += dr("vSourceMedExCode_Then") & "'"
                                            filter += " And vSubjectId = '" & drSubject("vSubjectId") & "'"
                                            filter += " And iRepeatNo = " & Convert.ToString(dr_Detail(indexIf)("iRepeatNo"))
                                            dr_DetailThen = dt_CRFDetail.Select(filter)
                                        End If


                                        For indexThen As Integer = 0 To dr_DetailThen.Length - 1
                                            repeatNoSourceThen = 0
                                            ActivitySourceThen = String.Empty
                                            Source_MedExValue_Then = String.Empty
                                            Source_MedExType_Then = String.Empty
                                            Target_MedExValue_Then = String.Empty


                                            If dr_DetailThen.Length > 1 Then
                                                repeatNoSourceThen = Convert.ToInt32(dr_DetailThen(indexThen)("iRepeatNo"))
                                                ActivitySourceThen = Convert.ToString(dr_DetailThen(indexThen)("vNodeDisplayName")).Trim()
                                            End If

                                            Source_MedExValue_Then = Convert.ToString(dr_DetailThen(indexThen)("vDefaultValue")).Trim()
                                            Source_MedExType_Then = Convert.ToString(dr_DetailThen(indexThen)("vMedExType")).Trim()
                                            Target_MedExValue_Then = Convert.ToString(dr("vTargetValue_Then")).Trim()

                                            If dr_Detail(indexIf)("cIsRepeatable").ToString = "Y" And dr_DetailThen.Length > 1 Then
                                                Source_MedExValue_Then = Convert.ToString(dr_DetailThen(indexIf)("vDefaultValue")).Trim()
                                                Source_MedExType_Then = Convert.ToString(dr_DetailThen(indexIf)("vMedExType")).Trim()
                                                Exit For
                                            End If

                                            ''''''''''''Target_Then exists or not:Start
                                            If Convert.ToString(dr("vTargetMedExCode_Then")).Trim() <> "" Then
                                                Dim dr_TargetCRFDetailThen() As DataRow
                                                filter = "iNodeId = " & Convert.ToString(dr("iTargetNodeId_Then")) & " And vMedExCode = '"
                                                filter += dr("vTargetMedExCode_Then") & "'"
                                                filter += " And vSubjectId = '" & drSubject("vSubjectId") & "'"
                                                dr_TargetCRFDetailThen = dt_CRFDetail.Select(filter)

                                                If Convert.ToString(dr("vTargetMedExCode_Then")).Trim() <> "" And dr_TargetCRFDetailThen.Length < 1 Then
                                                    NotToFire = True
                                                    Continue For
                                                End If

                                                If dr_SourceCRFDetailThen(0)("cIsRepeatable") = "Y" And dr_TargetCRFDetailThen(0)("cIsRepeatable") = "Y" Then
                                                    filter = "iNodeId = " & Convert.ToString(dr("iTargetNodeId_Then")) & " And vMedExCode = '"
                                                    filter += dr("vTargetMedExCode_Then") & "'"
                                                    filter += " And vSubjectId = '" & drSubject("vSubjectId") & "'"
                                                    filter += " And iRepeatNo = '" & Convert.ToString(dr_DetailThen(indexThen)("iRepeatNo")) & "'"
                                                    dr_TargetCRFDetailThen = dt_CRFDetail.Select(filter)
                                                End If

                                                For indexTargetThen As Integer = 0 To dr_DetailThen.Length - 1

                                                    repeatNoTargetThen = 0
                                                    ActivityTargetThen = String.Empty
                                                    Target_MedExValue_Then = String.Empty
                                                    Target_MedExType_Then = String.Empty

                                                    repeatNoTargetThen = Convert.ToString(dr_TargetCRFDetailThen(indexTargetThen)("iRepeatNo"))
                                                    ActivityTargetThen = Convert.ToString(dr_TargetCRFDetailThen(indexTargetThen)("vNodeDisplayName"))
                                                    Target_MedExValue_Then = Convert.ToString(dr_TargetCRFDetailThen(indexTargetThen)("vDefaultValue"))
                                                    Target_MedExType_Then = Convert.ToString(dr_TargetCRFDetailThen(indexTargetThen)("vMedExType"))

                                                    ''''''''''''''''''''''''''''''fire
                                                    dr_EditChecksDtl = dt_EditChecksDtl.NewRow()
                                                    dr_EditChecksDtl("bIsQuery") = 0
                                                    Is_Query = False

                                                    If NotToFire = True Then
                                                        Continue For
                                                    End If

                                                    Me.GetResult(Source_MedExValue_If, Source_MedExType_If, _
                                                                Target_MedExValue_If, Target_MedExType_If, _
                                                                Op_If.ToUpper(), _
                                                                Source_MedExValue_Then, Source_MedExType_Then, _
                                                                Target_MedExValue_Then, Target_MedExType_Then, _
                                                                Op_Then.ToUpper(), Is_Query)

                                                    If Is_Query Then
                                                        dr_EditChecksDtl("bIsQuery") = 1
                                                    End If
                                                    dr_EditChecksDtl("nEditChecksHdrNo") = Me.HProjectId.Value.Trim() + Convert.ToString(dr("iSourceNodeId_If")) + Subject_Count
                                                    dr_EditChecksDtl("nEditChecksDtlNo") = counter
                                                    dr_EditChecksDtl("nMedExEditCheckNo") = Convert.ToString(dr("nMedExEditCheckNo"))
                                                    dr_EditChecksDtl("nCRFDtlNo") = Convert.ToString(dr_Detail(indexIf)("nCRFDtlNo"))
                                                    dr_EditChecksDtl("cStatusIndi") = "N"
                                                    dr_EditChecksDtl("dModifyOn") = DateTime.Now()
                                                    dr_EditChecksDtl("vQueryValue") = Convert.ToString(dr("vCondition")).Trim()
                                                    dr_EditChecksDtl("cQueryStatus") = Query_New
                                                    dr_EditChecksDtl("vRemarks") = IIf(repeatNoSourceIF > 0, "[" & ActivitySourceIF & " Repetition : " & repeatNoSourceIF & "]", "") _
                                                            & IIf(repeatNoTargetIF > 0, "[" & ActivityTargetIF & " Repetition : " & repeatNoTargetIF & "]", "") _
                                                            & IIf(repeatNoSourceThen > 0, "[" & ActivitySourceThen & " Repetition : " & repeatNoSourceThen & "]", "") _
                                                            & IIf(repeatNoTargetThen > 0, "[" & ActivityTargetThen & " Repetition : " & repeatNoTargetThen & "]", "") _
                                                            & Convert.ToString(dr("vRemarks")).Trim()


                                                    dr_EditChecksDtl("iModifyBy") = Me.Session(S_UserID)

                                                    dt_EditChecksDtl.Rows.Add(dr_EditChecksDtl)
                                                    dt_EditChecksDtl.AcceptChanges()

                                                    '''''''''''''''''''''''''''''''''End fire
                                                Next indexTargetThen

                                            Else ''''''''''''Target_Then not exists:Start

                                                ''''''''''''''''''''''''''''''fire
                                                dr_EditChecksDtl = dt_EditChecksDtl.NewRow()
                                                dr_EditChecksDtl("bIsQuery") = 0
                                                Is_Query = False

                                                If NotToFire = True Then
                                                    Continue For
                                                End If

                                                Me.GetResult(Source_MedExValue_If, Source_MedExType_If, _
                                                            Target_MedExValue_If, Target_MedExType_If, _
                                                            Op_If.ToUpper(), _
                                                            Source_MedExValue_Then, Source_MedExType_Then, _
                                                            Target_MedExValue_Then, Target_MedExType_Then, _
                                                            Op_Then.ToUpper(), Is_Query)

                                                If Is_Query Then
                                                    dr_EditChecksDtl("bIsQuery") = 1
                                                End If
                                                dr_EditChecksDtl("nEditChecksHdrNo") = Me.HProjectId.Value.Trim() + Convert.ToString(dr("iSourceNodeId_If")) + Subject_Count
                                                dr_EditChecksDtl("nEditChecksDtlNo") = counter
                                                dr_EditChecksDtl("nMedExEditCheckNo") = Convert.ToString(dr("nMedExEditCheckNo"))
                                                dr_EditChecksDtl("nCRFDtlNo") = Convert.ToString(dr_Detail(indexIf)("nCRFDtlNo"))
                                                dr_EditChecksDtl("cStatusIndi") = "N"
                                                dr_EditChecksDtl("dModifyOn") = DateTime.Now()
                                                dr_EditChecksDtl("vQueryValue") = Convert.ToString(dr("vCondition")).Trim()
                                                dr_EditChecksDtl("cQueryStatus") = Query_New
                                                dr_EditChecksDtl("vRemarks") = IIf(repeatNoSourceIF > 0, "[" & ActivitySourceIF & " Repetition : " & repeatNoSourceIF & "]", "") _
                                                        & IIf(repeatNoTargetIF > 0, "[" & ActivityTargetIF & " Repetition : " & repeatNoTargetIF & "]", "") _
                                                        & IIf(repeatNoSourceThen > 0, "[" & ActivitySourceThen & " Repetition : " & repeatNoSourceThen & "]", "") _
                                                        & Convert.ToString(dr("vRemarks")).Trim()


                                                dr_EditChecksDtl("iModifyBy") = Me.Session(S_UserID)

                                                dt_EditChecksDtl.Rows.Add(dr_EditChecksDtl)
                                                dt_EditChecksDtl.AcceptChanges()

                                                '''''''''''''''''''''''''''''''''End fire

                                            End If ''''''''''''Target_Then exists or not:Start
                                        Next indexThen

                                    Else ''''''''''''Source_Then not exists:End

                                        ''''''''''''''''''''''''''''''fire
                                        dr_EditChecksDtl = dt_EditChecksDtl.NewRow()
                                        dr_EditChecksDtl("bIsQuery") = 0
                                        Is_Query = False

                                        If NotToFire = True Then
                                            Continue For
                                        End If

                                        Me.GetResult(Source_MedExValue_If, Source_MedExType_If, _
                                                    Target_MedExValue_If, Target_MedExType_If, _
                                                    Op_If.ToUpper(), _
                                                    Source_MedExValue_Then, Source_MedExType_Then, _
                                                    Target_MedExValue_Then, Target_MedExType_Then, _
                                                    Op_Then.ToUpper(), Is_Query)

                                        If Is_Query Then
                                            dr_EditChecksDtl("bIsQuery") = 1
                                        End If
                                        dr_EditChecksDtl("nEditChecksHdrNo") = Me.HProjectId.Value.Trim() + Convert.ToString(dr("iSourceNodeId_If")) + Subject_Count
                                        dr_EditChecksDtl("nEditChecksDtlNo") = counter
                                        dr_EditChecksDtl("nMedExEditCheckNo") = Convert.ToString(dr("nMedExEditCheckNo"))
                                        dr_EditChecksDtl("nCRFDtlNo") = Convert.ToString(dr_Detail(indexIf)("nCRFDtlNo"))
                                        dr_EditChecksDtl("cStatusIndi") = "N"
                                        dr_EditChecksDtl("dModifyOn") = DateTime.Now()
                                        dr_EditChecksDtl("vQueryValue") = Convert.ToString(dr("vCondition")).Trim()
                                        dr_EditChecksDtl("cQueryStatus") = Query_New
                                        dr_EditChecksDtl("vRemarks") = IIf(repeatNoSourceIF > 0, "[" & ActivitySourceIF & " Repetition : " & repeatNoSourceIF & "]", "") _
                                                & IIf(repeatNoTargetIF > 0, "[" & ActivityTargetIF & " Repetition : " & repeatNoTargetIF & "]", "") _
                                                & Convert.ToString(dr("vRemarks")).Trim()


                                        dr_EditChecksDtl("iModifyBy") = Me.Session(S_UserID)

                                        dt_EditChecksDtl.Rows.Add(dr_EditChecksDtl)
                                        dt_EditChecksDtl.AcceptChanges()

                                        '''''''''''''''''''''''''''''''''End fire
                                    End If ''''''''''''Source_Then exists or not:End

                                    '''''''''''''''''''''''''''''''''''''''''''''''''
                                Next indexTargetIf

                            ElseIf Convert.ToString(dr("vTargetMedExCode_If")).Trim() = "" AndAlso _
                                    (Convert.ToString(dr("iSourceNodeId_Then")).Trim() <> "" AndAlso _
                                    Convert.ToString(dr("iSourceNodeId_Then")).Trim() <> "0") Then '''''''''''''''target_if not exists and Source_Then exists or not:Start

                                Source_MedExValue_Then = ""
                                Source_MedExType_Then = ""
                                Target_MedExValue_Then = ""
                                Op_Then = ""
                                Dim dr_SourceCRFDetailThen() As DataRow
                                filter = "iNodeId = " & Convert.ToString(dr("iSourceNodeId_Then")) & " And vMedExCode = '"
                                filter += Convert.ToString(dr("vSourceMedExCode_Then")) & "'"
                                filter += " And vSubjectId = '" & drSubject("vSubjectId") & "'"
                                dr_SourceCRFDetailThen = dt_CRFDetail.Select(filter)

                                Op_Then = Convert.ToString(dr("vOperator_Then")).Trim()


                                'Condition added to check if data entry is not done
                                If dr_SourceCRFDetailThen.Length < 1 Then
                                    NotToFire = True
                                    Continue For
                                End If
                                repeatNoSourceThen = 0
                                dr_DetailThen = dr_SourceCRFDetailThen
                                If dr_SourceCRFDetailThen(0)("cIsRepeatable") = "Y" And dr_Detail(indexIf)("cIsRepeatable").ToString = "Y" Then
                                    filter = "iNodeId = " & Convert.ToString(dr("iSourceNodeId_Then")) & " And vMedExCode = '"
                                    filter += dr("vSourceMedExCode_Then") & "'"
                                    filter += " And vSubjectId = '" & drSubject("vSubjectId") & "'"
                                    filter += " And iRepeatNo = " & Convert.ToString(dr_Detail(indexIf)("iRepeatNo"))
                                    dr_DetailThen = dt_CRFDetail.Select(filter)
                                End If


                                For indexThen As Integer = 0 To dr_DetailThen.Length - 1
                                    repeatNoSourceThen = 0
                                    ActivitySourceThen = String.Empty
                                    Source_MedExValue_Then = String.Empty
                                    Source_MedExType_Then = String.Empty
                                    Target_MedExValue_Then = String.Empty


                                    If dr_DetailThen.Length > 1 Then
                                        repeatNoSourceThen = Convert.ToInt32(dr_DetailThen(indexThen)("iRepeatNo"))
                                        ActivitySourceThen = Convert.ToString(dr_DetailThen(indexThen)("vNodeDisplayName")).Trim()
                                    End If

                                    Source_MedExValue_Then = Convert.ToString(dr_DetailThen(indexThen)("vDefaultValue")).Trim()
                                    Source_MedExType_Then = Convert.ToString(dr_DetailThen(indexThen)("vMedExType")).Trim()
                                    Target_MedExValue_Then = Convert.ToString(dr("vTargetValue_Then")).Trim()

                                    If dr_Detail(indexIf)("cIsRepeatable").ToString = "Y" And dr_DetailThen.Length > 1 Then
                                        Source_MedExValue_Then = Convert.ToString(dr_DetailThen(indexIf)("vDefaultValue")).Trim()
                                        Source_MedExType_Then = Convert.ToString(dr_DetailThen(indexIf)("vMedExType")).Trim()
                                        Exit For
                                    End If

                                    ''''''''''''Target_Then exists or not:Start
                                    If Convert.ToString(dr("vTargetMedExCode_Then")).Trim() <> "" Then
                                        Dim dr_TargetCRFDetailThen() As DataRow
                                        filter = "iNodeId = " & Convert.ToString(dr("iTargetNodeId_Then")) & " And vMedExCode = '"
                                        filter += dr("vTargetMedExCode_Then") & "'"
                                        filter += " And vSubjectId = '" & drSubject("vSubjectId") & "'"
                                        dr_TargetCRFDetailThen = dt_CRFDetail.Select(filter)

                                        If Convert.ToString(dr("vTargetMedExCode_Then")).Trim() <> "" And dr_TargetCRFDetailThen.Length < 1 Then
                                            NotToFire = True
                                            Continue For
                                        End If

                                        If dr_SourceCRFDetailThen(0)("cIsRepeatable") = "Y" And dr_TargetCRFDetailThen(0)("cIsRepeatable") = "Y" Then
                                            filter = "iNodeId = " & Convert.ToString(dr("iTargetNodeId_Then")) & " And vMedExCode = '"
                                            filter += dr("vTargetMedExCode_Then") & "'"
                                            filter += " And vSubjectId = '" & drSubject("vSubjectId") & "'"
                                            filter += " And iRepeatNo = '" & Convert.ToString(dr_DetailThen(indexThen)("iRepeatNo")) & "'"
                                            dr_TargetCRFDetailThen = dt_CRFDetail.Select(filter)
                                        End If

                                        For indexTargetThen As Integer = 0 To dr_DetailThen.Length - 1

                                            repeatNoTargetThen = 0
                                            ActivityTargetThen = String.Empty
                                            Target_MedExValue_Then = String.Empty
                                            Target_MedExType_Then = String.Empty

                                            repeatNoTargetThen = Convert.ToString(dr_TargetCRFDetailThen(indexTargetThen)("iRepeatNo"))
                                            ActivityTargetThen = Convert.ToString(dr_TargetCRFDetailThen(indexTargetThen)("vNodeDisplayName"))
                                            Target_MedExValue_Then = Convert.ToString(dr_TargetCRFDetailThen(indexTargetThen)("vDefaultValue"))
                                            Target_MedExType_Then = Convert.ToString(dr_TargetCRFDetailThen(indexTargetThen)("vMedExType"))

                                            ''''''''''''''''''''''''''''''fire
                                            dr_EditChecksDtl = dt_EditChecksDtl.NewRow()
                                            dr_EditChecksDtl("bIsQuery") = 0
                                            Is_Query = False

                                            If NotToFire = True Then
                                                Continue For
                                            End If

                                            Me.GetResult(Source_MedExValue_If, Source_MedExType_If, _
                                                        Target_MedExValue_If, Target_MedExType_If, _
                                                        Op_If.ToUpper(), _
                                                        Source_MedExValue_Then, Source_MedExType_Then, _
                                                        Target_MedExValue_Then, Target_MedExType_Then, _
                                                        Op_Then.ToUpper(), Is_Query)

                                            If Is_Query Then
                                                dr_EditChecksDtl("bIsQuery") = 1
                                            End If
                                            dr_EditChecksDtl("nEditChecksHdrNo") = Me.HProjectId.Value.Trim() + Convert.ToString(dr("iSourceNodeId_If")) + Subject_Count
                                            dr_EditChecksDtl("nEditChecksDtlNo") = counter
                                            dr_EditChecksDtl("nMedExEditCheckNo") = Convert.ToString(dr("nMedExEditCheckNo"))
                                            dr_EditChecksDtl("nCRFDtlNo") = Convert.ToString(dr_Detail(indexIf)("nCRFDtlNo"))
                                            dr_EditChecksDtl("cStatusIndi") = "N"
                                            dr_EditChecksDtl("dModifyOn") = DateTime.Now()
                                            dr_EditChecksDtl("vQueryValue") = Convert.ToString(dr("vCondition")).Trim()
                                            dr_EditChecksDtl("cQueryStatus") = Query_New
                                            dr_EditChecksDtl("vRemarks") = IIf(repeatNoSourceIF > 0, "[" & ActivitySourceIF & " Repetition : " & repeatNoSourceIF & "]", "") _
                                                    & IIf(repeatNoSourceThen > 0, "[" & ActivitySourceThen & " Repetition : " & repeatNoSourceThen & "]", "") _
                                                    & IIf(repeatNoTargetThen > 0, "[" & ActivityTargetThen & " Repetition : " & repeatNoTargetThen & "]", "") _
                                                    & Convert.ToString(dr("vRemarks")).Trim()


                                            dr_EditChecksDtl("iModifyBy") = Me.Session(S_UserID)

                                            dt_EditChecksDtl.Rows.Add(dr_EditChecksDtl)
                                            dt_EditChecksDtl.AcceptChanges()

                                            '''''''''''''''''''''''''''''''''End fire
                                        Next indexTargetThen

                                    Else ''''''''''''Target_Then not exists:Start

                                        ''''''''''''''''''''''''''''''fire
                                        dr_EditChecksDtl = dt_EditChecksDtl.NewRow()
                                        dr_EditChecksDtl("bIsQuery") = 0
                                        Is_Query = False

                                        If NotToFire = True Then
                                            Continue For
                                        End If

                                        Me.GetResult(Source_MedExValue_If, Source_MedExType_If, _
                                                    Target_MedExValue_If, Target_MedExType_If, _
                                                    Op_If.ToUpper(), _
                                                    Source_MedExValue_Then, Source_MedExType_Then, _
                                                    Target_MedExValue_Then, Target_MedExType_Then, _
                                                    Op_Then.ToUpper(), Is_Query)

                                        If Is_Query Then
                                            dr_EditChecksDtl("bIsQuery") = 1
                                        End If
                                        dr_EditChecksDtl("nEditChecksHdrNo") = Me.HProjectId.Value.Trim() + Convert.ToString(dr("iSourceNodeId_If")) + Subject_Count
                                        dr_EditChecksDtl("nEditChecksDtlNo") = counter
                                        dr_EditChecksDtl("nMedExEditCheckNo") = Convert.ToString(dr("nMedExEditCheckNo"))
                                        dr_EditChecksDtl("nCRFDtlNo") = Convert.ToString(dr_Detail(indexIf)("nCRFDtlNo"))
                                        dr_EditChecksDtl("cStatusIndi") = "N"
                                        dr_EditChecksDtl("dModifyOn") = DateTime.Now()
                                        dr_EditChecksDtl("vQueryValue") = Convert.ToString(dr("vCondition")).Trim()
                                        dr_EditChecksDtl("cQueryStatus") = Query_New
                                        dr_EditChecksDtl("vRemarks") = IIf(repeatNoSourceIF > 0, "[" & ActivitySourceIF & " Repetition : " & repeatNoSourceIF & "]", "") _
                                                & IIf(repeatNoSourceThen > 0, "[" & ActivitySourceThen & " Repetition : " & repeatNoSourceThen & "]", "") _
                                                & Convert.ToString(dr("vRemarks")).Trim()


                                        dr_EditChecksDtl("iModifyBy") = Me.Session(S_UserID)

                                        dt_EditChecksDtl.Rows.Add(dr_EditChecksDtl)
                                        dt_EditChecksDtl.AcceptChanges()

                                        '''''''''''''''''''''''''''''''''End fire

                                    End If ''''''''''''Target_Then exists or not:End
                                Next indexThen
                            Else ''''''''''''''Source_Then and target_if not exists : Start

                                ''''''''''''''''''''''''''''''fire
                                dr_EditChecksDtl = dt_EditChecksDtl.NewRow()
                                dr_EditChecksDtl("bIsQuery") = 0
                                Is_Query = False

                                If NotToFire = True Then
                                    Continue For
                                End If

                                Me.GetResult(Source_MedExValue_If, Source_MedExType_If, _
                                            Target_MedExValue_If, Target_MedExType_If, _
                                            Op_If.ToUpper(), _
                                            Source_MedExValue_Then, Source_MedExType_Then, _
                                            Target_MedExValue_Then, Target_MedExType_Then, _
                                            Op_Then.ToUpper(), Is_Query)

                                If Is_Query Then
                                    dr_EditChecksDtl("bIsQuery") = 1
                                End If
                                dr_EditChecksDtl("nEditChecksHdrNo") = Me.HProjectId.Value.Trim() + Convert.ToString(dr("iSourceNodeId_If")) + Subject_Count
                                dr_EditChecksDtl("nEditChecksDtlNo") = counter
                                dr_EditChecksDtl("nMedExEditCheckNo") = Convert.ToString(dr("nMedExEditCheckNo"))
                                dr_EditChecksDtl("nCRFDtlNo") = Convert.ToString(dr_Detail(indexIf)("nCRFDtlNo"))
                                dr_EditChecksDtl("cStatusIndi") = "N"
                                dr_EditChecksDtl("dModifyOn") = DateTime.Now()
                                dr_EditChecksDtl("vQueryValue") = Convert.ToString(dr("vCondition")).Trim()
                                dr_EditChecksDtl("cQueryStatus") = Query_New
                                dr_EditChecksDtl("vRemarks") = IIf(repeatNoSourceIF > 0, "[" & ActivitySourceIF & " Repetition : " & repeatNoSourceIF & "]", "") _
                                        & IIf(repeatNoTargetIF > 0, "[" & ActivityTargetIF & " Repetition : " & repeatNoTargetIF & "]", "") _
                                        & Convert.ToString(dr("vRemarks")).Trim()


                                dr_EditChecksDtl("iModifyBy") = Me.Session(S_UserID)

                                dt_EditChecksDtl.Rows.Add(dr_EditChecksDtl)
                                dt_EditChecksDtl.AcceptChanges()

                                '''''''''''''''''''''''''''''''''End fire

                            End If ''''''''''''Source_Then not exists and target_if exists or not:End

                            Source_MedExValue_If = String.Empty
                            Source_MedExType_If = String.Empty
                            Target_MedExValue_If = String.Empty
                            Target_MedExType_If = String.Empty
                            Source_MedExValue_Then = String.Empty
                            Source_MedExType_Then = String.Empty
                            Target_MedExValue_Then = String.Empty
                            Target_MedExType_Then = String.Empty
                            filter = String.Empty
                            Op_If = String.Empty
                            Op_Then = String.Empty


                        Next indexIf ''''''''''1st loop:End
                    Next dr '''''''''''For loop for edit checks :End

                    '''''''''''To save fired editchecks :Start
                    dv_EditChecksHdr = dt_EditChecksHdr.DefaultView
                    dt_EditChecksHdr = Nothing
                    dt_EditChecksHdr = dv_EditChecksHdr.ToTable(True, "nEditChecksHdrNo,vWorkspaceId,iPeriod,iNodeId,vActivityId,vSubjectId,iTranNo,iModifyBy".Split(",")).Copy()

                    For Each dr As DataRow In dt_EditChecksHdr.Rows

                        dv_EditChecksHdr = dt_EditChecksHdr.DefaultView
                        dv_EditChecksHdr.RowFilter = "nEditChecksHdrNo = " & Convert.ToString(dr("nEditChecksHdrNo")).Trim()

                        dv_EditChecksDtl = dt_EditChecksDtl.DefaultView
                        dv_EditChecksDtl.RowFilter = "nEditChecksHdrNo = " & Convert.ToString(dr("nEditChecksHdrNo")).Trim()

                        ds_Save = Nothing
                        ds_Save = New DataSet()
                        ds_Save.Tables.Add(dv_EditChecksHdr.ToTable().Copy())
                        ds_Save.AcceptChanges()
                        ds_Save.Tables.Add(dv_EditChecksDtl.ToTable().Copy())
                        ds_Save.AcceptChanges()

                        If Not Me.objLambda.Save_EditChecksHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                                ds_Save, Me.Session(S_UserID), eStr) Then
                            Throw New Exception(eStr)
                        End If

                    Next dr '''''''''''To save fired editchecks :End

                    dv_EditChecksHdr.RowFilter = ""
                    dv_EditChecksDtl.RowFilter = ""

                Next drSubject '''''''''''''Loop of subjects :End


            Next i ''''''''''Parent activity array:End


        Catch ex As Exception
            Me.ShowErrorMessage("Error While Processing Edit Checks : " + ex.Message, "")
        End Try

    End Sub

    Private Sub GetDateTimeResult_If(ByVal Op As String, ByVal SourceValue As String, ByVal TargetValue As String, ByRef Is_Query As Boolean)
        If IsDate(TargetValue) Then
            If Op = ">" Then

                If CDate(SourceValue) > CDate(TargetValue) Then
                    Is_Query = True
                End If

            ElseIf Op = ">=" Then

                If CDate(SourceValue) >= CDate(TargetValue) Then
                    Is_Query = True
                End If

            ElseIf Op = "<" Then

                If CDate(SourceValue) < CDate(TargetValue) Then
                    Is_Query = True
                End If

            ElseIf Op = "<=" Then

                If CDate(SourceValue) <= CDate(TargetValue) Then
                    Is_Query = True
                End If

            ElseIf Op = "=" Then

                If CDate(SourceValue) = CDate(TargetValue) Then
                    Is_Query = True
                End If

            ElseIf Op = "<>" Then

                If CDate(SourceValue) <> CDate(TargetValue) Then
                    Is_Query = True
                End If
            End If
        ElseIf TargetValue = "NULL" Then
            If Op = "=" Then

                If IsDBNull(SourceValue) Then
                    Is_Query = True
                End If

            ElseIf Op = "<>" Then

                If Not IsDBNull(SourceValue) Then
                    Is_Query = True
                End If
            End If
        End If

    End Sub

    Private Sub GetTextValueResult_If(ByVal Op As String, ByVal SourceValue As String, ByVal TargetValue As String, ByRef Is_Query As Boolean)

        If Op = ">" Then

            If Val(SourceValue) > Val(TargetValue) Then
                Is_Query = True
            End If

        ElseIf Op = ">=" Then

            If Val(SourceValue) >= Val(TargetValue) Then
                Is_Query = True
            End If

        ElseIf Op = "<" Then

            If Val(SourceValue) < Val(TargetValue) Then
                Is_Query = True
            End If

        ElseIf Op = "<=" Then

            If Val(SourceValue) <= Val(TargetValue) Then
                Is_Query = True
            End If

        ElseIf Op = "=" Then

            If SourceValue.ToUpper = TargetValue.ToUpper Then
                Is_Query = True
            End If

        ElseIf Op = "<>" Then

            If SourceValue.ToUpper <> TargetValue.ToUpper Then
                Is_Query = True
            End If

        End If

    End Sub

    Private Sub GetDateTimeResult_Then(ByVal Op As String, ByVal SourceValue As String, ByVal TargetValue As String, ByRef Is_Query As Boolean)
        If IsDate(TargetValue) Then
            If Op = ">" Then

                If CDate(SourceValue) > CDate(TargetValue) Then
                    Is_Query = True
                End If

            ElseIf Op = ">=" Then

                If CDate(SourceValue) >= CDate(TargetValue) Then
                    Is_Query = True
                End If

            ElseIf Op = "<" Then

                If CDate(SourceValue) < CDate(TargetValue) Then
                    Is_Query = True
                End If

            ElseIf Op = "<=" Then

                If CDate(SourceValue) <= CDate(TargetValue) Then
                    Is_Query = True
                End If

            ElseIf Op = "=" Then

                If CDate(SourceValue) = CDate(TargetValue) Then
                    Is_Query = True
                End If

            ElseIf Op = "<>" Then

                If CDate(SourceValue) <> CDate(TargetValue) Then
                    Is_Query = True
                End If

            End If
        ElseIf TargetValue = "NULL" Then
            If Op = "=" Then

                If IsDBNull(SourceValue) Then
                    Is_Query = True
                End If

            ElseIf Op = "<>" Then

                If Not IsDBNull(SourceValue) Then
                    Is_Query = True
                End If
            End If
        End If
    End Sub

    Private Sub GetTextValueResult_Then(ByVal Op As String, ByVal SourceValue As String, ByVal TargetValue As String, ByRef Is_Query As Boolean)

        If Op = ">" Then

            If Val(SourceValue) > Val(TargetValue) Then
                Is_Query = True
            End If

        ElseIf Op = ">=" Then

            If Val(SourceValue) >= Val(TargetValue) Then
                Is_Query = True
            End If

        ElseIf Op = "<" Then

            If Val(SourceValue) < Val(TargetValue) Then
                Is_Query = True
            End If

        ElseIf Op = "<=" Then

            If Val(SourceValue) <= Val(TargetValue) Then
                Is_Query = True
            End If

        ElseIf Op = "=" Then

            If SourceValue.ToUpper = TargetValue.ToUpper Then
                Is_Query = True
            End If

        ElseIf Op = "<>" Then

            If SourceValue.ToUpper <> TargetValue.ToUpper Then
                Is_Query = True
            End If

        End If

    End Sub

    Private Sub GetResult(ByVal SourceValue_If As String, ByVal SourceType_If As String, _
                      ByVal TargetValue_If As String, ByVal TargetType_If As String, _
                      ByVal Op_If As String, _
                      ByVal SourceValue_Then As String, ByVal SourceType_Then As String, _
                      ByVal TargetValue_Then As String, ByVal TargetType_Then As String, _
                      ByVal Op_Then As String, _
                      ByRef Is_Query As Boolean)
        Dim strChkLen As Int16 = 0
        Dim strDesc As String = String.Empty
        Dim strTargetChkLen As Int16 = 0
        Dim strTargetDesc As String = String.Empty
        Try

            ''''''''''''''check if date is not in proper dateformate editchecks will fire without query:Start
            ' Addecd to check if date contains any other values then editchecks not need to fire.
            If SourceType_If = "DateTime" Or SourceType_If = "AsyncDateTime" Then
                If SourceValue_If <> DBNull.Value.ToString Or SourceValue_If <> "" Then
                    If IsDate(SourceValue_If) = False Then
                        Is_Query = 0
                        Exit Sub
                    End If
                End If
            End If
            ' Addecd to check if date contains any other values then editchecks not need to fire.
            If SourceType_Then = "DateTime" Or SourceType_Then = "AsyncDateTime" Then
                If SourceValue_Then <> DBNull.Value.ToString Or SourceValue_Then <> "" Then
                    If IsDate(SourceValue_Then) = False Then
                        Is_Query = 0
                        Exit Sub
                    End If
                End If
            End If
            If TargetType_If = "DateTime" Or TargetType_If = "AsyncDateTime" Then
                If TargetValue_If <> DBNull.Value.ToString Or TargetValue_If <> "" Then
                    If IsDate(TargetValue_If) = False Then
                        Is_Query = 0
                        Exit Sub
                    End If
                End If
                ' Addecd to check if date contains any other values then editchecks not need to fire.
            End If
            If TargetType_Then = "DateTime" Or TargetType_Then = "AsyncDateTime" Then
                If TargetValue_Then <> DBNull.Value.ToString Or TargetValue_Then <> "" Then
                    If IsDate(TargetValue_Then) = False Then
                        Is_Query = 0
                        Exit Sub
                    End If
                End If
            End If ''''''''''''''check if date is not in proper dateformate editchecks will fire without query:End

            If (SourceType_If = "DateTime" Or SourceType_If = "AsyncDateTime") Or (SourceType_If = "Time" Or SourceType_If = "AsyncTime") Then

                If SourceValue_If <> "" AndAlso (TargetValue_If <> "NULL" Or TargetValue_If <> "") Then
                    GetDateTimeResult_If(Op_If, SourceValue_If, TargetValue_If, Is_Query)
                End If

                If SourceValue_If = "" AndAlso (TargetValue_If = "NULL" Or TargetValue_If = "") Then
                    TargetValue_If = ""
                    GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
                End If

                If SourceValue_If <> "" AndAlso (TargetValue_If = "NULL" Or TargetValue_If = "") Then
                    TargetValue_If = ""
                    GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
                End If

                If SourceValue_If = "" AndAlso (TargetValue_If <> "NULL" Or TargetValue_If <> "") Then
                    GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
                End If

                If Op_Then <> "" AndAlso Is_Query = True Then
                    If (SourceValue_Then <> "" Or SourceValue_Then.ToUpper() <> "NULL") Then
                        Is_Query = False
                        If (TargetValue_Then.ToUpper() = "NULL" Or TargetValue_Then.ToUpper() = "") Then
                            TargetValue_Then = ""
                            GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                        ElseIf TargetValue_Then.ToUpper() <> "NULL" And TargetValue_Then.ToUpper() <> "" Then
                            GetDateTimeResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                        End If
                    Else
                        If TargetValue_Then = "NULL" Or TargetValue_Then = "" Then
                            Is_Query = False
                            TargetValue_Then = ""
                            GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                        ElseIf (TargetValue_Then <> "NULL" And TargetValue_Then <> "") Then
                            Is_Query = False
                            GetDateTimeResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                        End If
                    End If
                End If

            ElseIf SourceType_If = "CheckBox" Then
                strChkLen = SourceValue_If.Split(",").Length
                strDesc = SourceValue_If
                For i = 0 To strChkLen - 1
                    SourceValue_If = strDesc.Split(",")(i).ToString

                    If SourceValue_If <> "" AndAlso TargetValue_If <> "" Then
                        GetTextValueResult_If(Op_If, SourceValue_If, TargetValue_If, Is_Query)
                    End If

                    If SourceValue_If = "" AndAlso (TargetValue_If = "NULL" Or TargetValue_If = "") Then
                        TargetValue_If = ""
                        GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
                    End If
                    If SourceValue_If <> "" AndAlso (TargetValue_If = "NULL" Or TargetValue_If = "") Then
                        TargetValue_If = ""
                        GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
                    End If
                    If SourceValue_If = "" AndAlso (TargetValue_If <> "NULL" And TargetValue_If <> "") Then
                        GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
                    End If

                    If Op_Then <> "" AndAlso Is_Query = True Then
                        If SourceValue_Then <> "" Then
                            Is_Query = False
                            If TargetValue_Then.ToUpper() = "NULL" Or TargetValue_Then.ToUpper() = "" Then
                                TargetValue_Then = ""
                                GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                            ElseIf TargetValue_Then.ToUpper() <> "NULL" And TargetValue_Then.ToUpper() <> "" Then
                                GetTextValueResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                            End If
                        Else
                            If (TargetValue_Then = "NULL" Or TargetValue_Then = "") Then
                                Is_Query = False
                                TargetValue_Then = ""
                                GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                            Else
                                Is_Query = False
                                GetTextValueResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                            End If

                        End If

                    End If
                Next
            Else 'If SourceType_If = "TextBox" Then

                If SourceValue_If <> "" AndAlso TargetValue_If <> "" Then
                    GetTextValueResult_If(Op_If, SourceValue_If, TargetValue_If, Is_Query)
                End If

                If SourceValue_If = "" AndAlso (TargetValue_If = "NULL" Or TargetValue_If = "") Then
                    TargetValue_If = ""
                    GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
                End If
                If SourceValue_If <> "" AndAlso (TargetValue_If = "NULL" Or TargetValue_If = "") Then
                    TargetValue_If = ""
                    GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
                End If
                If SourceValue_If = "" AndAlso (TargetValue_If <> "NULL" And TargetValue_If <> "") Then
                    GetNullValueChecked(Op_If, SourceValue_If, TargetValue_If, Is_Query)
                End If
                If Op_Then <> "" AndAlso Is_Query = True Then
                    If SourceType_Then = "CheckBox" Then
                        strTargetChkLen = SourceValue_Then.Split(",").Length
                        strTargetDesc = SourceValue_Then
                        For iLen = 0 To strTargetChkLen - 1
                            SourceValue_Then = strTargetDesc.Split(",")(iLen)

                            If SourceValue_Then <> "" Then
                                Is_Query = False
                                If TargetValue_Then.ToUpper() = "NULL" Or TargetValue_Then.ToUpper() = "" Then
                                    TargetValue_Then = ""
                                    GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                                ElseIf TargetValue_Then.ToUpper() <> "NULL" And TargetValue_Then.ToUpper() <> "" Then
                                    GetTextValueResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                                End If
                            Else
                                If (TargetValue_Then = "NULL" Or TargetValue_Then = "") Then
                                    Is_Query = False
                                    TargetValue_Then = ""
                                    GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                                Else
                                    Is_Query = False
                                    GetTextValueResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                                End If

                            End If


                        Next
                    Else
                        If SourceValue_Then <> "" Then
                            Is_Query = False
                            If TargetValue_Then.ToUpper() = "NULL" Or TargetValue_Then.ToUpper() = "" Then
                                TargetValue_Then = ""
                                GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                            ElseIf TargetValue_Then.ToUpper() <> "NULL" And TargetValue_Then.ToUpper() <> "" Then
                                GetTextValueResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                            End If
                        Else
                            If (TargetValue_Then = "NULL" Or TargetValue_Then = "") Then
                                Is_Query = False
                                TargetValue_Then = ""
                                GetNullValueChecked(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                            Else
                                Is_Query = False
                                GetTextValueResult_Then(Op_Then, SourceValue_Then, TargetValue_Then, Is_Query)
                            End If

                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Processing Edit Checks ", ex.Message)
        End Try

    End Sub

    Private Sub GetNullValueChecked(ByVal Op As String, ByVal SourceValue As String, ByVal TargetValue As String, ByRef Is_Query As Boolean)
        If Op = "=" Then

            If SourceValue = TargetValue Then
                Is_Query = True
            End If

        ElseIf Op = "<>" Then

            If SourceValue <> TargetValue Then
                Is_Query = True
            End If

        End If

    End Sub

#End Region

#Region "ResetPage"

    Private Sub ResetPage()

        Me.ViewState(VS_DtEditChecksDtl) = Nothing
        Me.ViewState(VS_DtEditChecksHdr) = Nothing
        Me.Btn_viewAll.Style.Add("Display", "none")
        Me.btnExportToExcell.Style.Add("display", "none")
        Me.GVEditChecksHdrDtl.DataSource = Nothing
        Me.GVEditChecksHdrDtl.DataBind()
        Me.GVEditChecksHdr.DataSource = Nothing
        Me.GVEditChecksHdr.DataBind()
        Me.txtproject.Text = ""
        Me.ddlVisit.Items.Clear()
        If Not Me.GENCALL_DATA() Then
            Exit Sub
        End If

    End Sub

#End Region

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage & "<BR> " & eStr
        objCommon.WriteError(Server, Request, Session, exMessage & "<BR> " & eStr)
    End Sub

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage & "<BR> " & eStr
        objCommon.WriteError(Server, Request, Session, ex, exMessage & "<BR> " & eStr)
    End Sub

#End Region

#Region "Grid View Hdr Events"

    Protected Sub GVEditChecksHdr_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GVEditChecksHdr.RowCommand

        Dim index As Integer = e.CommandArgument

        If e.CommandName.ToUpper() = "VIEW" Then

            Shared_Initial = Me.GVEditChecksHdr.Rows(index).Cells(GVCHdr_Initials).Text
            Shared_ScreenNo = Me.GVEditChecksHdr.Rows(index).Cells(GVCHdr_ScreenNo).Text
            Shared_RandomizationNo = Me.GVEditChecksHdr.Rows(index).Cells(GVCHdr_RandomizationNo).Text

            If Not Me.FillEditChecksDtlGrid(Me.GVEditChecksHdr.Rows(index).Cells(GVCHdr_vSubjectId).Text, _
                                            Shared_Initial, Shared_ScreenNo, Shared_RandomizationNo) Then
                Exit Sub
            End If

        End If

    End Sub

    Protected Sub GVEditChecksHdr_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVEditChecksHdr.RowCreated

        If e.Row.RowType = DataControlRowType.Header Or _
           e.Row.RowType = DataControlRowType.DataRow Or _
           e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVCHdr_vSubjectId).Visible = False
            e.Row.Cells(GVCHdr_WorkspaceId).Visible = False
            e.Row.Cells(GVCHDR_RejectionFlag).Visible = False

        End If

    End Sub

    Protected Sub GVEditChecksHdr_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVEditChecksHdr.RowDataBound

        Dim index As Integer = e.Row.RowIndex

        If e.Row.RowType = DataControlRowType.DataRow Then

            e.Row.Cells(GVCHdr_SrNo).Text = index + (Me.GVEditChecksHdr.PageSize * Me.GVEditChecksHdr.PageIndex) + 1

            CType(e.Row.Cells(GVCHdr_View).FindControl("lnkbtnView"), LinkButton).CommandArgument = index
            CType(e.Row.Cells(GVCHdr_View).FindControl("lnkbtnView"), LinkButton).CommandName = "VIEW"
            e.Row.Cells(GVCHdr_FiredDate).Text = CType(e.Row.Cells(GVCHdr_FiredDate).Text, DateTime).ToString("dd-MMM-yyyy")
            If e.Row.Cells(GVCHDR_RejectionFlag).Text.ToString() = "Y" Then
                e.Row.ControlStyle.BackColor = Drawing.Color.Red
            End If
        End If

    End Sub

#End Region

#Region "Grid View Dtl Events"

    Protected Sub GVEditChecksHdrDtl_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVEditChecksHdrDtl.RowCreated

        If e.Row.RowType = DataControlRowType.Header Or _
            e.Row.RowType = DataControlRowType.DataRow Or _
            e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVCDtl_EditChecksHdrNo).Visible = False
            e.Row.Cells(GVCDtl_EditChecksDtlNo).Visible = False
            e.Row.Cells(GVCDtl_GenerateQueryFlag).Visible = False
            e.Row.Cells(GVCDtl_GenerateQueryCheckBox).Visible = False
            e.Row.Cells(GVCDtl_GenerateQueryRemarks).Visible = False
            e.Row.Cells(GVCDtl_ResolveQueryFlag).Visible = False
            e.Row.Cells(GVCDtl_ResolveQueryCheckBox).Visible = False
            e.Row.Cells(GVCDtl_ResolveQueryRemarks).Visible = False
            e.Row.Cells(GVCDtl_iNodeId).Visible = False
            e.Row.Cells(GVCDtl_vActivityId).Visible = False
            e.Row.Cells(GVCDtl_vSubjectId).Visible = False
            e.Row.Cells(GVCDtl_MySubjectNo).Visible = False
            e.Row.Cells(GVCDtl_Period).Visible = False
            e.Row.Cells(GVCDtl_SourceNodeId_If).Visible = False
            e.Row.Cells(GVCDtl_CrossActivity_NodeId).Visible = False
            e.Row.Cells(GVCDtl_CrossActivity_Period).Visible = False
            e.Row.Cells(GVCDtl_CrossActivity_ActivityId).Visible = False

            e.Row.Cells(GVCDtl_ScreenNo).Style.Add("width", "80px")
            e.Row.Cells(GVCDtl_SrNo).Style.Add("width", "17px")
        End If

    End Sub

    Protected Sub GVEditChecksHdrDtl_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVEditChecksHdrDtl.RowDataBound

        Dim index As Integer = e.Row.RowIndex
        Dim img As Image
        Dim RedirectStr As String = String.Empty
        Dim RedirectStrCross As String = String.Empty

        If e.Row.RowType = DataControlRowType.Header Then

            If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = "0" Then
                CType(e.Row.FindControl("chkGenerateQueryAll"), CheckBox).Attributes.Add("display", "none")
            End If

        End If

        If e.Row.RowType = DataControlRowType.DataRow Then

            RedirectStr = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" & Me.HProjectId.Value.ToString() & _
                "&ActivityId=" & e.Row.Cells(GVCDtl_vActivityId).Text.ToString() & _
                "&NodeId=" & e.Row.Cells(GVCDtl_SourceNodeId_If).Text.ToString() & _
                "&PeriodId=" & e.Row.Cells(GVCDtl_Period).Text.ToString() & _
                "&SubjectId=" & e.Row.Cells(GVCDtl_vSubjectId).Text.ToString() & "&Type=BA-BE" & _
                "&MySubjectNo=" & e.Row.Cells(GVCDtl_MySubjectNo).Text.ToString() & _
                "&ScreenNo=" & e.Row.Cells(GVCDtl_ScreenNo).Text.ToString() & _
                "&mode=" & WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

            CType(e.Row.Cells(GVCDtl_Activity).FindControl("lBtn_Activity"), LinkButton).OnClientClick = "return OpenWindow('" & RedirectStr & "');"

            RedirectStrCross = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" & Me.HProjectId.Value.ToString() & _
                            "&ActivityId=" & e.Row.Cells(GVCDtl_CrossActivity_ActivityId).Text.ToString() & _
                            "&NodeId=" & e.Row.Cells(GVCDtl_CrossActivity_NodeId).Text.ToString() & _
                            "&PeriodId=" & e.Row.Cells(GVCDtl_CrossActivity_Period).Text.ToString() & _
                            "&SubjectId=" & e.Row.Cells(GVCDtl_vSubjectId).Text.ToString() & "&Type=BA-BE" & _
                            "&MySubjectNo=" & e.Row.Cells(GVCDtl_MySubjectNo).Text.ToString() & _
                            "&ScreenNo=" & e.Row.Cells(GVCDtl_ScreenNo).Text.ToString() & _
                            "&mode=" & WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

            CType(e.Row.Cells(GVCDtl_Activity).FindControl("lBtn_CrossActivity"), LinkButton).OnClientClick = "return OpenWindow('" & RedirectStrCross & "');"

            ''''''''''''''''''''''''''''''''''''

            e.Row.Cells(GVCDtl_SrNo).Text = index + (Me.GVEditChecksHdrDtl.PageSize * Me.GVEditChecksHdrDtl.PageIndex) + 1
            e.Row.Cells(GVCDtl_FireDate).Text = CType(e.Row.Cells(GVCDtl_FireDate).Text, DateTime).ToString("dd-MMM-yyyy")

            e.Row.Cells(GVCDtl_ScreenNo).Style.Add("width", "80px")
            e.Row.Cells(GVCDtl_SrNo).Style.Add("width", "17px")
            If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = "0" Then
                CType(e.Row.FindControl("chkGenerateQuery"), CheckBox).Attributes.Add("display", "none")
            End If

            If Convert.ToString(e.Row.Cells(GVCDtl_GenerateQueryFlag).Text).Trim.ToUpper() <> "Y" AndAlso _
                Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> "0" Then
            End If

            If Convert.ToString(e.Row.Cells(GVCDtl_ResolveQueryFlag).Text).Trim.ToUpper() <> "Y" AndAlso _
                Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() <> "0" Then
            End If

            If Convert.ToString(e.Row.Cells(GVCDtl_GenerateQueryFlag).Text).Trim.ToUpper() = "Y" Then

                CType(e.Row.Cells(GVCDtl_GenerateQueryCheckBox).FindControl("chkGenerateQuery"), CheckBox).Checked = False
                CType(e.Row.Cells(GVCDtl_GenerateQueryCheckBox).FindControl("chkGenerateQuery"), CheckBox).Visible = False
                img = New Image
                img.ImageUrl = "~/images/tick.jpg"
                e.Row.Cells(GVCDtl_GenerateQueryCheckBox).Controls.Add(img)

            End If

            If Convert.ToString(e.Row.Cells(GVCDtl_ResolveQueryFlag).Text).Trim.ToUpper() = "Y" Then

                CType(e.Row.Cells(GVCDtl_ResolveQueryCheckBox).FindControl("chkResolveQuery"), CheckBox).Checked = False
                CType(e.Row.Cells(GVCDtl_ResolveQueryCheckBox).FindControl("chkResolveQuery"), CheckBox).Visible = False
                img = New Image
                img.ImageUrl = "~/images/tick.jpg"
                e.Row.Cells(GVCDtl_ResolveQueryCheckBox).Controls.Add(img)

            End If

        End If

    End Sub

#End Region

#Region "Div Buttons Events"

    Protected Sub btndivGenerateQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndivGenerateQuery.Click

        Dim dt_EditChecksDtl As New DataTable
        Dim dt_New As New DataTable
        Dim ds_Save As New DataSet
        Dim index As Integer = 0
        Dim dr() As DataRow
        Dim drChild As DataRow
        Dim drNew As DataRow
        Dim chk As CheckBox
        Dim eStr As String = String.Empty

        Try

            dt_EditChecksDtl = CType(Me.ViewState(VS_DtGridHdrDtl), DataTable).Copy()
            dt_New = CType(Me.ViewState(VS_DtEditChecksDtl), DataTable).Copy()
            dt_New.Rows.Clear()

            For index = 0 To Me.GVEditChecksHdrDtl.Rows.Count - 1

                chk = CType(Me.GVEditChecksHdrDtl.Rows(index).Cells(GVCDtl_GenerateQueryCheckBox).FindControl("chkGenerateQuery"), CheckBox)

                If Not chk Is Nothing Then

                    If chk.Checked Then

                        dr = dt_EditChecksDtl.Select("nEditChecksDtlNo = " & Me.GVEditChecksHdrDtl.Rows(index).Cells(GVCDtl_EditChecksDtlNo).Text.Trim())

                        If Not dr Is Nothing Then

                            For Each drChild In dr
                                drNew = dt_New.NewRow()
                                drNew("nEditChecksDtlNo") = drChild("nEditChecksDtlNo")
                                drNew("nEditChecksHdrNo") = drChild("nEditChecksHdrNo")
                                drNew("nMedExEditCheckNo") = drChild("nMedExEditCheckNo")
                                drNew("nCRFDtlNo") = drChild("nCRFDtlNo")
                                drNew("bIsQuery") = drChild("bIsQuery")
                                drNew("vQueryValue") = drChild("vQueryValue")
                                drNew("cQueryStatus") = Query_Generated
                                drNew("vRemarks") = drChild("vRemarks")
                                drNew("iModifyBy") = Me.Session(S_UserID)
                                drNew("cStatusIndi") = "E"
                                drNew("cGenerateFlag") = "Y"
                                drNew("vGenerateRemark") = Me.txtGenerateQueryRemarks.Text.Trim()
                                drNew("cResolvedFlag") = drChild("cResolvedFlag")
                                drNew("vResolvedRemark") = drChild("vResolvedRemark")
                                dt_New.Rows.Add(drNew)
                            Next drChild

                        End If

                    End If

                End If

            Next index

            ds_Save.Tables.Add(dt_New.Copy())

            If Not Me.objLambda.Save_EditChecksDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                                            ds_Save, Me.Session(S_UserID), eStr) Then
                Throw New Exception(eStr)
            End If

            Me.objCommon.ShowAlert("Query Generated Successfully", Me.Page)

            If Not Me.FillEditChecksDtlGrid(ds_Save.Tables(0).Rows(0)("nEditChecksHdrNo").ToString.Trim()) Then
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Generating Query.", ex.Message)
        End Try

    End Sub

    Protected Sub btndivResolveQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndivResolveQuery.Click

        Dim dt_EditChecksDtl As New DataTable
        Dim dt_New As New DataTable
        Dim ds_Save As New DataSet
        Dim index As Integer = 0
        Dim dr() As DataRow
        Dim drChild As DataRow
        Dim drNew As DataRow
        Dim chk As CheckBox
        Dim eStr As String = String.Empty

        Try

            dt_EditChecksDtl = CType(Me.ViewState(VS_DtGridHdrDtl), DataTable).Copy()
            dt_New = CType(Me.ViewState(VS_DtEditChecksDtl), DataTable).Copy()
            dt_New.Rows.Clear()

            For index = 0 To Me.GVEditChecksHdrDtl.Rows.Count - 1

                chk = CType(Me.GVEditChecksHdrDtl.Rows(index).Cells(GVCDtl_ResolveQueryCheckBox).FindControl("chkResolveQuery"), CheckBox)

                If Not chk Is Nothing Then

                    If chk.Checked Then

                        dr = dt_EditChecksDtl.Select("nEditChecksDtlNo = " & Me.GVEditChecksHdrDtl.Rows(index).Cells(GVCDtl_EditChecksDtlNo).Text.Trim())

                        If Not dr Is Nothing Then

                            For Each drChild In dr
                                drNew = dt_New.NewRow()
                                drNew("nEditChecksDtlNo") = drChild("nEditChecksDtlNo")
                                drNew("nEditChecksHdrNo") = drChild("nEditChecksHdrNo")
                                drNew("nMedExEditCheckNo") = drChild("nMedExEditCheckNo")
                                drNew("nCRFDtlNo") = drChild("nCRFDtlNo")
                                drNew("bIsQuery") = drChild("bIsQuery")
                                drNew("vQueryValue") = drChild("vQueryValue")
                                drNew("cQueryStatus") = Query_Resolved
                                drNew("vRemarks") = drChild("vRemarks")
                                drNew("iModifyBy") = Me.Session(S_UserID)
                                drNew("cStatusIndi") = "E"
                                drNew("cGenerateFlag") = drChild("cGenerateFlag")
                                drNew("vGenerateRemark") = drChild("vGenerateRemark")
                                drNew("cResolvedFlag") = "Y"
                                drNew("vResolvedRemark") = Me.txtResolveQueryRemarks.Text.Trim()
                                dt_New.Rows.Add(drNew)
                            Next drChild

                        End If

                    End If

                End If

            Next index

            ds_Save.Tables.Add(dt_New.Copy())

            If Not Me.objLambda.Save_EditChecksDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                                            ds_Save, Me.Session(S_UserID), eStr) Then
                Throw New Exception(eStr)
            End If

            Me.objCommon.ShowAlert("Query Resolved Successfully", Me.Page)

            If Not Me.FillEditChecksDtlGrid(ds_Save.Tables(0).Rows(0)("nEditChecksHdrNo").ToString.Trim()) Then
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Resolving Query.", ex.Message)
        End Try

    End Sub

#End Region

#Region "Export To Excell"
    Private Function ConvertDsTO(ByVal ds As DataSet) As String
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer
        Dim dsConvert As New DataSet
        Try

            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            strMessage.Append("<td><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append("Project/Site")
            strMessage.Append("</font></strong></td>")
            strMessage.Append("<td colspan=""6""><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append(Me.txtproject.Text.Trim())
            strMessage.Append("</font></strong></td>")
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")

            dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vMySubjectNo,vNodeDisplayName,CrossActivity,iRepeatNo,dFiredDate,vQueryValue,vRemarks".Split(",")).DefaultView.ToTable())
            dsConvert.AcceptChanges()
            dsConvert.Tables(0).Columns(0).ColumnName = "MySubjectNo/ScreenNo"
            dsConvert.Tables(0).Columns(1).ColumnName = "Activity"
            dsConvert.Tables(0).Columns(2).ColumnName = "Cross Activity"
            dsConvert.Tables(0).Columns(3).ColumnName = "Repeatation"
            dsConvert.Tables(0).Columns(4).ColumnName = "Executed Date"
            dsConvert.Tables(0).Columns(5).ColumnName = "Query"
            dsConvert.Tables(0).Columns(6).ColumnName = "Remarks"
            dsConvert.AcceptChanges()

            For iCol = 0 To dsConvert.Tables(0).Columns.Count - 1

                strMessage.Append("<td><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).Trim())
                strMessage.Append("</font></strong></td>")

            Next
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")

            For j = 0 To dsConvert.Tables(0).Rows.Count - 1
                strMessage.Append("<tr>")
                For i = 0 To dsConvert.Tables(0).Columns.Count - 1

                    strMessage.Append("<td><strong><font color=""#000000"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim())
                    strMessage.Append("</font></strong></td>")

                Next
                strMessage.Append("</tr>")
            Next
            strMessage.Append("</table>")

            Return strMessage.ToString

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
            Return ""
        End Try
    End Function
#End Region

End Class
