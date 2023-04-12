Partial Class frmDocumentManagementProject
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

    Private dt As New DataTable
    Private Const VS_WorkSpaceId As String = "WorkSpaceId"
    Private Const VS_NodeId As String = "NodeId"
    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtWorkSpaceNode As String = "DtWorkSpaceNode"
    Private Const VS_ActivityIds As String = "ActivityId"
    Private Const VS_toolTip As String = "toolTip"

    Private Const Docs As String = "Docs"
    Private Const SubDocs As String = "SubDocs"
    Private Const SubsDetail As String = "SubsDetail"
    Private Const Talk As String = "Talk"
    Private Const VS_dtReleaseDocumenTDetail As String = "ReleaseDocumentDetails" ' added by vishal
    Private Const VS_WorkSpaceNodeHistoryNo As String = "nWorkSpaceNodeHistoryNo" 'added by vishal

#End Region

#Region "Page_Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GenCall()
        End If
        AutoCompleteExtender2.ContextKey = ""
    End Sub

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim ds As New DataSet
        Try
            CType(Master.FindControl("lblerrormsg"), Label).Text = ""
            Choice = CType("1", WS_Lambda.DataObjOpenSaveModeEnum)
            iRelease.Style("display") = "none" 'added by vishal

            'Me.ViewState(VS_dtReleaseDocumenTDetail) = ds.Tables("ReleaseDocumentDetails")   ' adding blank DataTable in viewstate by vishal


            Me.ViewState(VS_Choice) = Choice

            If Not GenCall_ShowUI(Choice) Then 'For Displaying Data 
                Exit Function
            End If

            GenCall = True


        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_ShowUI"

    Private Function GenCall_ShowUI(ByVal Choice As WS_Lambda.DataObjOpenSaveModeEnum) As Boolean

        Try
            Page.Title = " ::  Document Management Project ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Master.FindControl("lblerrormsg"), Label).Text = ""
            CType(Master.FindControl("lblHeading"), Label).Text = "Document Management Project"

            If Not Me.Session(S_ActivityIds) Is Nothing Then
                Me.ViewState(VS_ActivityIds) = Me.Session(S_ActivityIds)
                Me.Session.Remove(S_ActivityIds)
            End If

            If Not Me.Request.QueryString("WorkspaceId") Is Nothing Then
                Me.ViewState(VS_WorkSpaceId) = Me.Request.QueryString("WorkspaceId").ToString()
                BindTree()
            End If

            '' Added by vishal
            If Not Me.Request.QueryString("NodeId") Is Nothing Then
                Me.ViewState(VS_NodeId) = Me.Request.QueryString("NodeId").ToString()
            End If
            ''''''''''
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Function

#End Region

#Region "Bindtree"

    Private Sub BindTree()

        Dim Ds_WorkSpaceNode As New DataSet
        Dim dsVacantPosition As New DataSet
        Dim dsWorkspace As New DataSet
        Dim dt_Workspace As New DataTable
        Dim CurrentNode As New TreeNode
        Dim ParentNode As New TreeNode
        Dim bln As Boolean = False
        Dim cnt As Integer = 0
        Dim eStr As String = ""

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Me.TVWorkspace.Nodes.Clear()
            'Ds_WorkSpaceNode = Me.objHelp.GetProc_TreeViewOfNodes(Me.ViewState(VS_WorkSpaceId).ToString())
            If Not Me.objHelp.Proc_ProjectNodeCommandButtonRights(Me.ViewState(VS_WorkSpaceId).ToString(), Me.Session(S_UserID), _
                                        "", "No", Ds_WorkSpaceNode, eStr) Then
                Me.objCommon.ShowAlert("Error While Getting Data From Proc_ProjectNodeCommandButtonRights : " + eStr, Me.Page)
                Exit Sub
            End If

            Me.ViewState(VS_DtWorkSpaceNode) = Ds_WorkSpaceNode.Tables(0)
            Me.txtproject.Text = Ds_WorkSpaceNode.Tables(0).Rows(0)("vWorkSpaceDesc").ToString()
            Me.HProjectId.Value = Ds_WorkSpaceNode.Tables(0).Rows(0)("vWorkSpaceId").ToString()
           
            dt = Ds_WorkSpaceNode.Tables(0)
            TestLast("0", New TreeNode())

            TVWorkspace.ExpandAll()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Private Sub TestLast(ByVal parentId As String, ByVal tn As TreeNode)
        Dim dr() As DataRow = dt.Select("iParentNodeId=" & parentId)
        Dim parentNode As New TreeNode
        Dim wStr As String = ""
        Dim ds_Docs As New DataSet
        Dim dv_Docs As New DataView
        Dim eStr As String = ""
        Dim toolTip As String = ""
        Dim Str As String = ""
        Dim drDoc As DataRow
        Dim ActivityIds() As String
        Dim ActivityId As String = ""
        Dim image As New ImageButton



        Try


            wStr = "vWorkSpaceId='" + Me.ViewState(VS_WorkSpaceId).ToString().Trim() + "' And iStageID='" + GeneralModule.Stage_Authorized.Trim() + "' And crequiredFlag='D'"
            If Not Me.objHelp.GetViewWorkSpaceNodeHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_Docs, eStr) Then
                Me.objCommon.ShowAlert("Error While Getting Data From View_WorkSpaceNodeHistory : " + eStr, Me.Page)
                Exit Sub
            End If

            For index As Integer = 0 To dr.Length - 1

                dv_Docs = ds_Docs.Tables(0).DefaultView
                dv_Docs.RowFilter = "iNodeId = " + dr(index)("iNodeId").ToString()


                parentNode = New TreeNode

                Str = Replace(dr(index)("vNodeDisplayName").ToString, "*", "") & "(" & dr(index)("vNodeName") & ")"

                For Each drDoc In dv_Docs.ToTable().Rows

                    Str += "-----<A Href = """ & drDoc("vDocPath").ToString().Replace("/", "\") & """  target = ""_blank""><font color=purple>" & drDoc("vFileName").ToString() & " </A>" '<img src=..\Images\acrobat.png />
                Next drDoc

                parentNode.Text = Str

                parentNode.Value = Replace(dr(index)("vNodeDisplayName").ToString, "*", "")
                parentNode.ImageToolTip = dr(index)("iNodeId")

                toolTip = dr(index)("vActivityId").ToString.Trim() + "," + dr(index)("iNodeId").ToString.Trim() _
                            + "," + dr(index)("vDocTypeCode").ToString.Trim() + "," _
                            + dr(index)("vDocTypeName").ToString.Trim() + "," + dr(index)("SubjectMedEx").ToString.Trim() _
                            + "," + dr(index)("iPeriod").ToString.Trim() + "," + dr(index)("cSubjectWiseFlag").ToString.Trim() _
                            + "," + dr(index)("vNodeDisplayName").ToString.Trim()

                parentNode.ToolTip = toolTip

                'parentNode.NavigateUrl = "javascript:IsAnyNodeChecked('" + TVWorkspace.ClientID + "');"

                '-------------Added in accordance with "frmSearchMedExInfoHdrDtl"----------------------

                If Not (Me.ViewState(VS_ActivityIds) Is Nothing Or Me.ViewState(VS_ActivityIds) = "") Then
                    ActivityIds = CType(Me.ViewState(VS_ActivityIds), String).Split(",")
                End If

                If Not (ActivityIds Is Nothing) Then
                    For count As Integer = 0 To ActivityIds.Length - 1

                        If dr(index)("vActivityId").ToString.Trim() = ActivityIds(count).ToString.Trim() Then

                            parentNode.Text = "<font color=green>" + Str

                        End If

                    Next count
                End If

                '---------------------------------------------------------------------------------------

                If parentId = "0" Then
                    TVWorkspace.Nodes.Add(parentNode)
                Else
                    tn.ChildNodes.Add(parentNode)
                End If

                TestLast(dr(index)("iNodeId"), parentNode)

            Next index

            'Me.TVWorkspace.Attributes.Add("OnClick", "ShowElement('" + divact.ClientID + "');")
            Me.TVWorkspace.Attributes.Add("OnClick", "IsAnyNodeChecked('" + TVWorkspace.ClientID + "');")

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub TVWorkspace_TreeNodeCheckChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.TreeNodeEventArgs) Handles TVWorkspace.TreeNodeCheckChanged

        'Me.ViewState(VS_toolTip) = e.Node.ToolTip.ToString()
        'If e.Node.Checked = True Then
        '    e.Node.Checked = False
        '    Me.lblNode.Text = e.Node.Value
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "Show", "ShowElement('Yes');", True)
        'End If

        Dim ActivityId As String = ""
        Dim NodeId As String = ""
        Dim DocId As String = ""
        Dim Doc As String = ""
        Dim Act As String = ""
        Dim Period As String = ""
        Dim SubjectMedEx As String = ""
        Dim SubjectWiseFlag As Char
        Dim RedirectStr As String = ""
        Dim Str() As String
       

        Dim wStr As String = ""
        Dim eStr As String = ""
        Dim ds_MedExInfoHdrDtl As New DataSet
        Dim dt_WorkSpaceNode As New DataTable
        Dim dv_WorkSpaceNode As New DataView

        Try
           


            Me.ViewState(VS_toolTip) = e.Node.ToolTip.ToString()

            If e.Node.Checked = True Then
                e.Node.Checked = False
                Me.lblNode.Text = e.Node.Value

                CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
                Str = CType(Me.ViewState(VS_toolTip), String).Split(",")
                ActivityId = Str(0)
                NodeId = Str(1)
                ViewState(VS_NodeId) = NodeId
                DocId = Str(2)
                Doc = Str(3)
                SubjectMedEx = Str(4)
                Period = Str(5)
                SubjectWiseFlag = Str(6)
                Act = Str(7)

                dt_WorkSpaceNode = CType(Me.ViewState(VS_DtWorkSpaceNode), DataTable)

                '*********Checking Activity is Subject specific or not***************
                'dv_WorkSpaceNode = dt_WorkSpaceNode.DefaultView
                'dv_WorkSpaceNode.RowFilter = "iNodeId = " + NodeId + " And cSubjectWiseFlag = 'Y'"

                Me.btnSubDocs.Visible = False
                If SubjectWiseFlag.ToString.ToUpper() = "Y" Then
                    Me.btnSubDocs.Visible = True
                End If
                'If dv_WorkSpaceNode.ToTable().Rows.Count > 0 Then
                '    Me.btnSubDocs.Visible = True
                'End If
                '*******************************************************

                'dv_WorkSpaceNode = dt_WorkSpaceNode.DefaultView
                'dv_WorkSpaceNode.RowFilter = "iNodeId = " + NodeId + " And cSubjectWiseFlag = 'N'"

                'If dv_WorkSpaceNode.ToTable().Rows.Count < 1 Then
                '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "Show", "ShowElement();", True)
                '    Exit Sub
                'End If

                wStr = "vWorkSpaceId = '" + Me.ViewState(VS_WorkSpaceId).ToString() + "'" + _
                        " And vActivityId = '" + ActivityId + "'" + _
                        " And cActiveFlag <> 'N' And cStatusIndi <> 'D'" + _
                        " And iPeriod = " + Period + " And iNodeId = " + NodeId

                If Not Me.objHelp.View_MedExInfoHdrDtl_Edit(wStr, "vMedExDesc,vDefaultValue", ds_MedExInfoHdrDtl, eStr) Then
                    Me.objCommon.ShowAlert("Error While Getting Data From View_MedExInfoHdrDtl_Edit : " + eStr, Me.Page)
                    Exit Sub
                End If

                Me.gvw_MedExDetail.DataSource = ds_MedExInfoHdrDtl.Tables(0)
                Me.gvw_MedExDetail.DataBind()

                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Show", "ShowElement();", True)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "Button Click Events"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Me.ViewState(VS_WorkSpaceId) = Me.HProjectId.Value.ToString()
        BindTree()
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Hide", "HideElement('" + divact.ClientID + "');", True)
        Me.gvw_MedExDetail.DataSource = Nothing
        Me.gvw_MedExDetail.DataBind()
        Me.lblNode.Text = ""
    End Sub

    Protected Sub btnDocs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDocs.Click
        Me.redirectPage(frmDocumentManagementProject.Docs)
    End Sub

    Protected Sub btnSubDocs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubDocs.Click

        Dim wStr As String = ""
        Dim eStr As String = ""
        Dim ds_WorkspaceSubjectMst As New DataSet

        Dim SubjectWiseFlag As Char
        Dim Str() As String

        Try

            Str = CType(Me.ViewState(VS_toolTip), String).Split(",")
            SubjectWiseFlag = Str(6)

            If SubjectWiseFlag.ToString.ToUpper() = "N" Then
                Exit Sub
            End If


            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            wStr = "vWorkspaceId = '" + Me.ViewState(VS_WorkSpaceId).ToString() + "'"
            wStr += " And cStatusIndi <> 'D'"

            If Not Me.objHelp.GetWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_WorkspaceSubjectMst, eStr) Then
                Me.ShowErrorMessage("Error While Getting Data From WorkspaceSubjectMst :", eStr)
                Exit Sub
            End If

            If ds_WorkspaceSubjectMst.Tables(0).Rows.Count < 1 Then
                Me.objCommon.ShowAlert("No Subject Is Assigned To This Project", Me.Page)
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Show", "ShowElement();", True)
                Exit Sub
            End If

            Me.redirectPage(frmDocumentManagementProject.SubDocs)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

    Protected Sub btnSubsDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubsDetail.Click
        Dim wStr As String = ""
        Dim eStr As String = ""
        Dim ds_WorkspaceSubjectMst As New DataSet
        Dim SubjectWiseFlag As Char
        Dim Str() As String

        Try

            Str = CType(Me.ViewState(VS_toolTip), String).Split(",")
            SubjectWiseFlag = Str(6)

            If SubjectWiseFlag.ToString.ToUpper() = "N" Then
                Me.redirectPage(frmDocumentManagementProject.SubsDetail)
                Exit Sub
            End If

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            wStr = "vWorkspaceId = '" + Me.ViewState(VS_WorkSpaceId) + "'"
            wStr += " And cStatusIndi <> 'D'"

            If Not Me.objHelp.GetWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_WorkspaceSubjectMst, eStr) Then
                Me.ShowErrorMessage("Error While Getting Data From WorkspaceSubjectMst :", eStr)
                Exit Sub
            End If

            If ds_WorkspaceSubjectMst.Tables(0).Rows.Count < 1 Then
                Me.objCommon.ShowAlert("No Subject Is Assigned To This Project", Me.Page)
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Show", "ShowElement();", True)
                Exit Sub
            End If

            Me.redirectPage(frmDocumentManagementProject.SubsDetail)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnTalk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTalk.Click
        Me.redirectPage(frmDocumentManagementProject.Talk)
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

#Region "Redirect Page()"

    Protected Sub redirectPage(ByVal param As String)
        Dim ActivityId As String = ""
        Dim NodeId As String = ""
        Dim DocId As String = ""
        Dim Doc As String = ""
        Dim Act As String = ""
        Dim Period As String = ""
        Dim SubjectMedEx As String = ""
        Dim SubjectWiseFlag As Char
        Dim RedirectStr As String = ""
        Dim Str() As String
        ''added by vishal for release button
        'Dim ds_grid1 As New Data.DataSet
        'Dim dv_grid1 As New Data.DataView
        'Dim WorkSpaceId As String = ""
        '' Dim NodeId As String = ""
        'Dim wstr As String = ""
        'Dim estr As String = String.Empty

        Try
            'WorkSpaceId = Me.ViewState(VS_WorkSpaceId).ToString()
            'NodeId = ViewState(VS_NodeId)

            'wstr = "vWorkSpaceId='" + WorkSpaceId + "' and iNodeId='" + NodeId + "'"
            'objHelp.getWorkspaceNodeHistory(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_grid1, estr)

            'If ds_grid1.Tables(0).Rows.Count > 0 Then
            '    dv_grid1 = New Data.DataView
            '    dv_grid1 = ds_grid1.Tables(0).DefaultView
            '    dv_grid1 = ds_grid1.Tables(0).DefaultView
            '    dv_grid1.RowFilter = "cRequiredFlag='P' "
            '    dv_grid1.ToTable().AcceptChanges()
            '    If dv_grid1.ToTable.Rows.Count > 0 Then
            '        btnRelease.Visible = True
            '    End If
            'End If


            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            Str = CType(Me.ViewState(VS_toolTip), String).Split(",")
            ActivityId = Str(0)
            NodeId = Str(1)
            DocId = Str(2)
            Doc = Str(3)
            SubjectMedEx = Str(4)
            Period = Str(5)
            SubjectWiseFlag = Str(6)
            Act = Str(7)

            If param = frmDocumentManagementProject.Docs Then

                Me.Response.Redirect("frmDocumentDetail_New.aspx?WorkSpaceId=" + Me.ViewState(VS_WorkSpaceId).ToString() & "&NodeId=" & NodeId & "&Mode=1" & "&Page=frmMainPage&Page2=frmDocumentManagementProject&Type=OPERATIONAL&QC=N")

            ElseIf param = frmDocumentManagementProject.SubDocs Then

                Response.Redirect("frmSubjectDetail.aspx?workspaceid=" + Me.ViewState(VS_WorkSpaceId).ToString() & "&NodeId=" & NodeId & "&DCId=" & DocId & "&DC=" & Doc & "&ActId=" & ActivityId & "&Act=" & Act & "&Mode=1" & "&Page=frmMainPage&Page2=frmDocumentManagementProject&Type=OPERATIONAL&QC=N")

            ElseIf param = frmDocumentManagementProject.SubsDetail Then

                If ActivityId = Act_Attendance Then
                    Response.Redirect("frmWorkspaceSubjectMst.aspx?mode=1&workspaceid=" + Me.ViewState(VS_WorkSpaceId).ToString() & "&Page=frmMainPage&Page2=frmDocumentManagementProject&Type=OPERATIONAL&PeriodId=" & Period)
                End If

                If SubjectWiseFlag = "Y" Then
                    Response.Redirect("frmWorkspaceSubjectMedExInfo.aspx?mode=1&workspaceid=" + Me.ViewState(VS_WorkSpaceId).ToString() & "&nodeId=" & NodeId & "&ActivityId=" & ActivityId & "&PeriodId=" & Period & "&Page=frmMainPage&Page2=frmDocumentManagementProject&Type=OPERATIONAL")
                Else
                    RedirectStr = "window.open(""" + "frmMedExInfoHdrDtl.aspx?WorkSpaceId=" + Me.ViewState(VS_WorkSpaceId).ToString().Trim() + _
                                      "&ActivityId=" + ActivityId + "&NodeId=" + NodeId + _
                                      "&PeriodId=" + Period + "&SubjectId=0000" + _
                                      "&MySubjectNo=0000"")"
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)
                End If

            ElseIf param = frmDocumentManagementProject.Talk Then

                Response.Redirect("frmProjectTalk.aspx?mode=1&workspaceid=" + Me.ViewState(VS_WorkSpaceId).ToString() & "&nodeId=" & NodeId & "&Page=frmMainPage&Page2=frmDocumentManagementProject&Type=OPERATIONAL")

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

#End Region

    Protected Sub btnRelease_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRelease.Click
        ' added by vishal For Release Mechanism

        Dim ds_project As New Data.DataSet
        Dim Wstr As String = String.Empty
        Dim ds_activitySubDetail As New Data.DataSet
        Dim ds_DocumentRelease As New DataSet
        Dim WorkSpaceId As String = ""
        Dim NodeId As String = ""
        Dim estr As String = String.Empty
        txtProjNo.Text = String.Empty
        Dim VS_iTranNos As String = ""

        MdlRelease.Show()


        Try
            WorkSpaceId = Me.ViewState(VS_WorkSpaceId).ToString()
            NodeId = ViewState(VS_NodeId)

          
            'added by vishal for Proposed Docs ,Effectivedocs ,Audit Trails Starts
            Wstr = "vWorkSpaceId='" + WorkSpaceId + "' and iNodeId='" + NodeId + "'"
            objHelp.GetViewWorkSpaceNodeHistory(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_project, estr)

            If Not objHelp.GetFieldsOfTable("Workspacenodehistory", "isNull(Max(iTranNo),1) as iTranNo", Wstr, ds_project, estr) Then

            End If
            VS_iTranNos = ds_project.Copy.Tables(0).Rows(0).Item("iTranNo")
            'objHelp.getWorkspaceNodeHistory("vWorkSpaceId='" & WorkSpaceId & "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_project, estr)

            Wstr = "vWorkSpaceId='" + WorkSpaceId + "' and iNodeId='" + NodeId + "' and iTranNo='" + VS_iTranNos + "'"
            objHelp.getWorkspaceNodeHistory(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_project, estr)

            objHelp.GetViewProjectActivityCurrAttr("vWorkSpaceID='" & WorkSpaceId & "' and iNodeId=" & NodeId, ds_activitySubDetail, estr)
            'Me.txtProjNo.Text = ds_project.Tables(0).Rows(0).Item("vProjectNo")
            Me.txtActivityName.Text = ds_activitySubDetail.Tables(0).Rows(0).Item("vActivityName")
            Me.HProject.Value = ds_project.Tables(0).Rows(0).Item("nWorkSpaceNodeHistoryNo")

        Catch ex As Exception
            objCommon.ShowAlert("Error in Releasing a document", Me.Page)
        End Try



    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim ds_Save As New DataSet
        Dim dt_Save As New DataTable
        Dim estr As String = ""
        Dim WorkspaceId As String = ""

        Dim ds_Grid As New DataSet
        Dim dv_Grid As New DataView
        Try
            AssignValues()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub
    Private Function AssignValues() As Boolean 'added by vishal to Assign Value to Document Release Details

        Dim dr As DataRow
        Dim Wstr As String = String.Empty
        Dim ds_activitySubDetail As New DataSet
        Dim ds_Save As New DataSet
        Dim DtCopyOfNeCahngedDocumentRelease As DataTable
        Dim estr As String = String.Empty
        Dim dt_ReleaseDocumentDetails As New DataTable
        Try
            Wstr = "1=2"
            If Not objHelp.getDocumentReleaseDetails(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_activitySubDetail, estr) Then
                Me.objCommon.ShowAlert("Error while getting data from DocumentReleaseDetails", Me.Page)
                Exit Function

            End If


            dt_ReleaseDocumentDetails = ds_activitySubDetail.Tables(0)
            dr = dt_ReleaseDocumentDetails.NewRow()
            ' dr("nDocReleaseNo") = 123
            dr("nWorkSpaceNodeHistoryNo") = Me.HProject.Value
            dr("vWorkSpaceId") = Me.ViewState(VS_WorkSpaceId)
            dr("vRemarks") = Me.txtRemarks.Text.Trim()
            ' dr("iTranNo") =
            dr("iModifyBy") = Me.Session(S_UserID)
            dr("cStatusIndi") = "N"

            dt_ReleaseDocumentDetails.Rows.Add(dr)
            Me.ViewState(VS_dtReleaseDocumenTDetail) = dt_ReleaseDocumentDetails
            DtCopyOfNeCahngedDocumentRelease = dt_ReleaseDocumentDetails.Copy()
            ds_Save.Tables.Add(DtCopyOfNeCahngedDocumentRelease)

            If Not objLambda.Save_DocumentReleaseDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Save, Me.Session(S_UserID), estr) Then
                objCommon.ShowAlert("Error While Saving ReleaseDocumenTDetails ", Me.Page)
                Exit Function
            Else
                objCommon.ShowAlert("Record Saved SuccessFully", Me.Page)
                ' ResetPage()
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function
End Class
