
Partial Class frmChildWorkspaceSite
    Inherits System.Web.UI.Page


#Region " Variable Declaration"

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Dim eStr As String

    Private Const VS_DtChildProtocolDtlMatrix As String = "Dt_ChildProtocolDtlMatrix"
    Private Const VS_WorkspaceDesc As String = "WorkspaceDesc"
    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtWSFullDetail As String = "WsFullDetail"
    Private Const Vs_LocationCode As String = "LocationCode"
    Private Const VS_LocationInit As String = "LocationInitiate"
    Private Const Vs_SelectedIndex_ID As String = "Vs_SelectedIndex_ID"
    Private Const Vs_SelectedIndex_CD As String = "Vs_SelectedIndex_CD"

    Private Const Vs_CDIndexADD As String = "Vs_CDIndexADD"
    Private Const Vs_IDIndexADD As String = "Vs_IDIndexADD"

    Private Const GvwCont_MatrixNo As Integer = 11
    Private Const GvwInv_MatrixNo As Integer = 11
    Private Const GvwCont_Edit As Integer = 10
    Private Const GvwInv_Edit As Integer = 10
    Private Const GvwContType As Integer = 0
    Private Const GvwInvType As Integer = 0


#End Region

#Region " Load Events "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            AutoCompleteExtender1.ContextKey = "isnull(vParentWorkspaceId,'') = ''"
            If Not Me.GenCall() Then
                Exit Sub
            End If

        End If
    End Sub

#End Region

#Region "GenCall "

    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try


            Choice = Me.Request.QueryString("Mode")
            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtChildProtocolDtlMatrix) = ds.Tables("workspaceprotocoldetailmatrix")

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If



            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall")

        Finally

        End Try

    End Function
#End Region

#Region "GenCall_Data "

    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean

        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing

        Try
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            End If

            If Not objHelp.getPortocolInfo(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            If ds.Tables(0).Rows.Count <= 0 And _
                                    Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found For Selected Operation")
            End If

            ds_DWR_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..GenCall_Data")
        End Try

    End Function

#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean

        Dim ds_Protocol As DataSet = Nothing
        Dim BaseWorkFolder As String = System.Configuration.ConfigurationManager.AppSettings("BaseWorkFolder")
        Try
            Page.Title = " :: Child/Site Project ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Child/Site Project"

            lblProject.Text = "Parent Project :"
            If Me.RblListCreateAndEditChildProject.SelectedIndex <> 0 Then
                lblProject.Text = " Child/Site Project :"
            End If
            ''Commented by Aaditya and added same logic client side
            ''txtRetentionPeriod.Attributes.Add("onblur", "return chkNumeric('Period');")
            ''txtnoofprjct.Attributes.Add("onblur", "return chkNumeric('Subjects');")

            If Not FillDropDown() Then
                Exit Function
            End If

            If Not CreateTable() Then 'Create Table for All WorkSpaceDetail
                Exit Function
            End If



            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "........GenCall_ShowUI")
        End Try

    End Function

#End Region

#Region "FillDropDown"

    Private Function FillDropDown() As Boolean

        Dim ds_User As New Data.DataSet
        Dim ds_Location As New Data.DataSet
        Dim ds_WorktypeMst As New Data.DataSet
        Dim dv_User As New DataView
        Dim dv_Location As New DataView
        Dim dv_WorktypeMst As New DataView
        Dim wStr As String = String.Empty

        Try


            wStr = " cStatusIndi<>'D' And cLocationType = 'L'"
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                wStr = " cStatusIndi<>'D' And cLocationType <> 'L'"
                'ElseIf Me.Session(S_ScopeNo) = Scope_SAll Or Me.Session(S_ScopeNo) = Scope_SAdmin Then
                '    wStr = " cStatusIndi<>'D'"
            End If

            objHelp.getLocationMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                     ds_Location, eStr)

            objHelp.GetViewUserMst("1=1  And cStatusindi<>'D' order by vUserName", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_User, eStr)

            dv_Location = ds_Location.Tables(0).DefaultView
            dv_Location.Sort = "vLocationName"
            Me.ddlLocation.DataSource = dv_Location
            Me.ddlLocation.DataValueField = "vLocationCode"
            Me.ddlLocation.DataTextField = "vLocationName"
            Me.ddlLocation.DataBind()
            Me.ddlLocation.Items.Insert(0, New ListItem("Select Site/Location", ""))
            '=========added on 08-12-09=========
            objHelp.getWorkTypeMst("cReplicaFlag<>'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                ds_WorktypeMst, eStr)

            dv_WorktypeMst = ds_WorktypeMst.Tables(0).DefaultView
            dv_WorktypeMst.Sort = "vWorkTypeDesc"
            Me.ddlStudytype.DataSource = dv_WorktypeMst
            Me.ddlStudytype.DataTextField = "vWorkTypeDesc"
            Me.ddlStudytype.DataValueField = "nWorkTypeNo"
            Me.ddlStudytype.DataBind()
            Me.ddlStudytype.Items.Insert(0, New ListItem("Select Study Type", ""))

            '===============
            If Not ds_User.Tables(0).Rows.Count <= 0 Then

                'dv_User = ds_User.Tables(0).DefaultView
                'dv_User.Sort = "vUserName"

                'For CntOfDv_user As Integer = 0 To dv_User.ToTable.Rows.Count - 1
                '    dv_User.Table.Rows(CntOfDv_user).Item("vUserName") = dv_User.Table.Rows(CntOfDv_user).Item("vUserName").ToString() + "     (" + dv_User.ToTable.Rows(CntOfDv_user).Item("vuserTypeName").ToString() + ")"
                'Next CntOfDv_user
                'dv_User.ToTable().AcceptChanges()

                Me.ddlmanager.DataSource = ds_User.Tables(0)
                Me.ddlmanager.DataValueField = "iUserId"
                Me.ddlmanager.DataTextField = "UserNameWithProfile"
                Me.ddlmanager.DataBind()
                Me.ddlmanager.Items.Insert(0, New ListItem("Select Manager", ""))

            End If

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillDropDown")
            Return False
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

#Region "Button Events"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click

        Dim ds_GetselectedChild As DataSet
        Dim setChildValue As String = String.Empty
        Dim GetChildVersion As String = String.Empty
        Dim SetChildReqID As String = String.Empty
        Dim GetChildReqId As String = String.Empty
        Dim ContChildWorkSpaceId As String = String.Empty
        Dim wstr As String = String.Empty
        Dim Ds_ChildMatrix As New DataSet
        Dim DT_CD As New DataTable
        Dim DT_ID As New DataTable
        Dim ds_GetManagerParentProject As New DataSet

        Try
            Me.fldChild.Style.Add("display", "")
            Me.fContactDetail.Style.Add("display", "")
            Me.fInvestigator.Style.Add("display", "")
            Me.tdSave.Style.Add("display", "")
            Me.tblDataEntry.Style.Add("display", "")
            'Me.tblDataEntry.Attributes.Add("Style", "display:block")
            ViewState(Vs_CDIndexADD) = 0
            ' GenCall()

            ds_GetManagerParentProject = Me.objHelp.GetResultSet("select iProjectManagerId,cIsTestSite from WorkSpaceProtocolDetail where vWorkSpaceId='" + Me.HProjectId.Value.Trim() + "'", "WorkSpaceProtocolDetail")       ' Added cIsTestSite to workspaceprotocoldetail

            If ds_GetManagerParentProject.Tables(0).Rows(0).Item("iProjectManagerId").ToString() <> "0" And ds_GetManagerParentProject.Tables(0).Rows(0).Item("iProjectManagerId").ToString() <> "" Then
                ddlmanager.SelectedValue = ds_GetManagerParentProject.Tables(0).Rows(0).Item("iProjectManagerId").ToString()
            End If

            If Not IsDBNull(ds_GetManagerParentProject.Tables(0).Rows(0).Item("cIsTestSite")) = True Then
                If ds_GetManagerParentProject.Tables(0).Rows(0).Item("cIsTestSite") = "Y" Then     ' Added By Mani For EDit Mode      
                    Me.chkTestSite.Checked = True
                Else
                    Me.chkTestSite.Checked = False
                End If
            End If

            If RblListCreateAndEditChildProject.SelectedIndex = 0 Then

                ds_GetselectedChild = Me.objHelp.GetResultSet("Select vWorkspaceId,vRequestId,vProjectNo,vParentWorkspaceId,vWorkSpaceDesc  From View_getWorkspaceDetailForHdr where vParentWorkspaceId ='" + Me.HProjectId.Value.Trim() + "' order by vWorkspaceId desc", "View_getWorkspaceDetailForHdr ")

            Else

                ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                ds_GetselectedChild = Me.objHelp.GetResultSet("Select vWorkspaceId,vRequestId,vProjectNo,vParentWorkspaceId,vWorkSpaceDesc  From View_getWorkspaceDetailForHdr where vWorkspaceId ='" + Me.HProjectId.Value.Trim() + "' order by vWorkspaceId desc", "View_getWorkspaceDetailForHdr ")
                ContChildWorkSpaceId = HProjectId.Value.ToString()

                '' ********************* Added By Dharmesh H.Salla On 22-Apr-2011 *************************** ''
                wstr = "vWorkSpaceId='" & ContChildWorkSpaceId & "'"
                If Not objHelp.getPortocolInfo(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_ChildMatrix, eStr) Then
                    Me.ObjCommon.ShowAlert("Error While Getting Data From Workspaceprotocoldetailsmatrix ", Me.Page)
                    Exit Sub
                End If

                ''''for setting  preSaved values
                ddlLocation.SelectedValue = Ds_ChildMatrix.Tables("workspacemst").Rows(0).Item("vLocationCode").ToString()

                If Ds_ChildMatrix.Tables("workspacemst").Rows(0).Item("cFastingFed").ToString() = "FS" Then
                    Me.chkFastfedYes.Checked = True
                ElseIf Ds_ChildMatrix.Tables("workspacemst").Rows(0).Item("cFastingFed").ToString() = "FD" Then
                    Me.chkFastfedNo.Checked = True
                End If

                If Not Ds_ChildMatrix.Tables("workspacemst").Rows(0).Item("nWorktypeno").ToString() = "" Then
                    If Ds_ChildMatrix.Tables("workspacemst").Rows(0).Item("nWorktypeno") = 1 Then
                        ddlStudytype.SelectedIndex = 1
                    ElseIf Ds_ChildMatrix.Tables("workspacemst").Rows(0).Item("nWorktypeno") = 2 Then
                        ddlStudytype.SelectedIndex = 2
                    End If
                End If
                If Not Ds_ChildMatrix.Tables("workspacemst").Rows(0).Item("nRetaintionPeriod").ToString().Trim() = "" Then
                    txtRetentionPeriod.Text = Convert.ToInt32(Ds_ChildMatrix.Tables("workspacemst").Rows(0).Item("nRetaintionPeriod").ToString().Trim.Substring(0, Convert.ToString(Ds_ChildMatrix.Tables("workspacemst").Rows(0).Item("nRetaintionPeriod").ToString()).Trim.IndexOf(".")))
                End If

                txtBiznetProjectNo.Text = Ds_ChildMatrix.Tables("workspacemst").Rows(0).Item("vExternalProjectNo").ToString()
                txtnoofprjct.Text = Ds_ChildMatrix.Tables("workspaceprotocoldetail").Rows(0).Item("iNoOfSubjects").ToString()

                '======================

                ViewState(VS_DtChildProtocolDtlMatrix) = Ds_ChildMatrix.Tables("workspaceprotocoldetailmatrix")
                Dim dv_ChildMatrix As DataView
                dv_ChildMatrix = Ds_ChildMatrix.Tables("workspaceprotocoldetailmatrix").DefaultView
                dv_ChildMatrix.RowFilter = "vType='CD' and cStatusIndi<>'D'"
                DT_CD = dv_ChildMatrix.ToTable.Copy
                dv_ChildMatrix.RowFilter = "vType='ID'and cStatusIndi<>'D'"
                DT_ID = dv_ChildMatrix.ToTable.Copy

                GvwContactDtl.DataSource = DT_CD
                GvwContactDtl.DataBind()
                GvwContactDtl.Visible = True
                GvwInvestDtl.DataSource = DT_ID
                GvwInvestDtl.DataBind()
                GvwInvestDtl.Visible = True

            End If

            If ds_GetselectedChild.Tables(0).Rows.Count > 0 Then ' its not for first time

                If RblListCreateAndEditChildProject.SelectedIndex = 1 Then

                    Me.txtChildReqID.Text = ds_GetselectedChild.Tables(0).Rows(0)("vRequestId").ToString()
                    Me.txtGetChildVersion.Text = ds_GetselectedChild.Tables(0).Rows(0)("vProjectNo").ToString()

                    Me.ViewState(VS_WorkspaceDesc) = ds_GetselectedChild.Tables(0).Rows(0)("vWorkSpaceDesc").ToString()

                Else

                    SetChildReqID = ds_GetselectedChild.Tables(0).Rows(0)("vRequestId").ToString()
                    GetChildReqId = SetChildReqID.Substring(0, SetChildReqID.LastIndexOf("-") + 1)
                    SetChildReqID = SetChildReqID.Substring(SetChildReqID.LastIndexOf("-") + 1)
                    SetChildReqID = Convert.ToString(IIf(SetChildReqID = "", 0, Integer.Parse(SetChildReqID) + 1))
                    SetChildReqID = "0" + SetChildReqID.ToString()
                    GetChildReqId = GetChildReqId + SetChildReqID
                    Me.txtChildReqID.Text = GetChildReqId

                    setChildValue = ds_GetselectedChild.Tables(0).Rows(0)("vProjectNo").ToString()
                    GetChildVersion = setChildValue.Substring(0, setChildValue.LastIndexOf("-") + 1)
                    GetChildVersion = GetChildVersion + SetChildReqID
                    Me.txtGetChildVersion.Text = GetChildVersion
                    Me.ViewState(VS_WorkspaceDesc) = ds_GetselectedChild.Tables(0).Rows(0)("vWorkSpaceDesc").ToString()

                End If

            Else ' for first time only

                ds_GetselectedChild = Me.objHelp.GetResultSet("Select vWorkspaceId,vRequestId,vProjectNo,vParentWorkspaceId,vWorkSpaceDesc  From View_getWorkspaceDetailForHdr where vWorkspaceId ='" + Me.HProjectId.Value.Trim() + "'", "View_getWorkspaceDetailForHdr ")
                If ds_GetselectedChild.Tables(0).Rows(0)("vParentWorkspaceId").ToString() Is DBNull.Value Or _
                    ds_GetselectedChild.Tables(0).Rows(0)("vParentWorkspaceId").ToString() = "" Then

                    setChildValue = ds_GetselectedChild.Tables(0).Rows(0)("vProjectNo").ToString() + "-01"
                    Me.txtGetChildVersion.Text = setChildValue

                    SetChildReqID = ds_GetselectedChild.Tables(0).Rows(0)("vRequestId").ToString() + "-01"
                    Me.txtChildReqID.Text = SetChildReqID

                    Me.ViewState(VS_WorkspaceDesc) = ds_GetselectedChild.Tables(0).Rows(0)("vWorkSpaceDesc").ToString()
                End If

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...........btnSetProject_Click")
        End Try

    End Sub

    Protected Sub btnContAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContAdd.Click

        Dim dv_Continfo As New DataView

        Try

            If Me.btnContAdd.Text = "Add" Then

                Me.AssignValueforGrid("CD")
                dv_Continfo = CType(Me.ViewState(VS_DtChildProtocolDtlMatrix), DataTable).DefaultView
                dv_Continfo.RowFilter = "vType = 'CD' and cStatusIndi<>'D'"
                Me.GvwContactDtl.DataSource = dv_Continfo.ToTable
                GvwContactDtl.Visible = True
                Me.GvwContactDtl.DataBind()
                Me.ResetContactControls()

            ElseIf Me.btnContAdd.Text = "Update" Then

                Dim dr As DataRow
                Dim Dt_CD As New DataTable
                Dim Ds_CD As New DataSet
                Dim Dv_CD As DataView
                Dim index As Integer = Me.ViewState(Vs_SelectedIndex_CD)
                Dt_CD = CType(Me.ViewState(VS_DtChildProtocolDtlMatrix), DataTable)

                For Each dr In Dt_CD.Rows

                    If RblListCreateAndEditChildProject.SelectedIndex = 0 Then

                        If dr.Item("vType") = "CD" Then
                            dr("vName") = Me.txtContName.Text
                            dr("vAddress1") = Me.txtContAdds1.Text
                            dr("vAddress2") = Me.txtContAdds2.Text
                            dr("vAddress3") = Me.txtContAdds3.Text
                            dr("vTeleNo") = Me.txtContTelno.Text
                            dr("vExtNo") = Me.txtContExtNo.Text
                            dr("vDesignation") = Me.txtContDesig.Text
                            dr("vQualification") = Me.txtContQuali.Text
                            dr("cStatusIndi") = "E"
                            Dt_CD.AcceptChanges()
                        End If

                        ''for edit mode
                    ElseIf dr.Item("nWorkspaceProtocolDetailMatrixNo") = Me.GvwContactDtl.Rows(index).Cells(GvwCont_MatrixNo).Text.ToString() Then

                        dr("vName") = Me.txtContName.Text
                        dr("vAddress1") = Me.txtContAdds1.Text
                        dr("vAddress2") = Me.txtContAdds2.Text
                        dr("vAddress3") = Me.txtContAdds3.Text
                        dr("vTeleNo") = Me.txtContTelno.Text
                        dr("vExtNo") = Me.txtContExtNo.Text
                        dr("vDesignation") = Me.txtContDesig.Text
                        dr("vQualification") = Me.txtContQuali.Text
                        dr("cStatusIndi") = "E"
                        Dt_CD.AcceptChanges()

                    End If

                Next

                Ds_CD.Tables.Add(Dt_CD)
                Ds_CD.Tables(0).TableName = "workspaceProtocolDetailMatrix"

                ViewState(VS_DtChildProtocolDtlMatrix) = Dt_CD

                ''''for the grid view
                Dv_CD = Dt_CD.DefaultView()
                Dv_CD.RowFilter = "vType = 'CD' and cStatusIndi<>'D'"
                Dt_CD = Dv_CD.ToTable()

                Me.ResetContactControls()
                Me.GvwContactDtl.DataSource = Dt_CD
                Me.GvwContactDtl.DataBind()
                GvwContactDtl.Visible = True
                Me.btnContAdd.Text = "Add"

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

    Protected Sub btnInvesAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInvesAdd.Click

        Dim dv_Investinfo As New DataView

        Try

            If Me.btnInvesAdd.Text = "Add" Then

                Me.AssignValueforGrid("ID")
                dv_Investinfo = CType(Me.ViewState(VS_DtChildProtocolDtlMatrix), DataTable).DefaultView
                dv_Investinfo.RowFilter = "vType = 'ID' and cStatusIndi<>'D'"
                Me.GvwInvestDtl.DataSource = dv_Investinfo.ToTable
                Me.GvwInvestDtl.DataBind()
                Me.GvwInvestDtl.Visible = True
                Me.ResetInvestControls()

            ElseIf Me.btnInvesAdd.Text = "Update" Then

                Dim Dt_ID As New DataTable
                Dim Ds_ID As New DataSet
                Dim Dv_ID As DataView
                Dim index As Integer = Me.ViewState(Vs_SelectedIndex_ID)
                Dt_ID = CType(Me.ViewState(VS_DtChildProtocolDtlMatrix), DataTable)

                For Each dr In Dt_ID.Rows

                    If RblListCreateAndEditChildProject.SelectedIndex = 0 Then

                        If dr.Item("vType") = "ID" Then
                            dr("vName") = Me.txtInvsName.Text
                            dr("vAddress1") = Me.txtInvsAdds1.Text
                            dr("vAddress2") = Me.TxtInvsAdds2.Text
                            dr("vAddress3") = Me.txtInvsAdds3.Text
                            dr("vTeleNo") = Me.txtInvstele.Text
                            dr("vExtNo") = Me.txtInvsExtes.Text
                            dr("vDesignation") = Me.txtInvsDesig.Text
                            dr("vQualification") = Me.TxtInvsqalific.Text
                            dr("cStatusIndi") = "E"
                            Dt_ID.AcceptChanges()
                        End If

                        ''for edit mode
                    ElseIf dr.Item("nWorkspaceProtocolDetailMatrixNo") = Me.GvwInvestDtl.Rows(index).Cells(GvwInv_MatrixNo).Text.ToString() Then

                        dr("vName") = Me.txtInvsName.Text
                        dr("vAddress1") = Me.txtInvsAdds1.Text
                        dr("vAddress2") = Me.TxtInvsAdds2.Text
                        dr("vAddress3") = Me.txtInvsAdds3.Text
                        dr("vTeleNo") = Me.txtInvstele.Text
                        dr("vExtNo") = Me.txtInvsExtes.Text
                        dr("vDesignation") = Me.txtInvsDesig.Text
                        dr("vQualification") = Me.TxtInvsqalific.Text
                        dr("cStatusIndi") = "E"
                        Dt_ID.AcceptChanges()

                    End If

                Next

                ViewState(VS_DtChildProtocolDtlMatrix) = Dt_ID
                'for the grid
                Dv_ID = Dt_ID.DefaultView()
                Dv_ID.RowFilter = "vType = 'ID' and cStatusIndi<>'D'"
                Dt_ID = Dv_ID.ToTable()

                Ds_ID.Tables.Add(Dt_ID)
                Ds_ID.Tables(0).TableName = "workspaceProtocolDetailMatrix"
                Me.ResetInvestControls()
                GvwInvestDtl.DataSource = Dt_ID
                GvwInvestDtl.DataBind()
                GvwInvestDtl.Visible = True
                Me.btnInvesAdd.Text = "Add"

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "........btnContAdd_Click")
        End Try

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim ds_Save As New DataSet
        Dim dt_SaveDtinfo As New DataTable
        Dim Dt_WSAll As New DataTable

        Try
            If Me.RblListCreateAndEditChildProject.SelectedIndex = 0 Then

                Me.AssignValue("ADD")

            ElseIf Me.RblListCreateAndEditChildProject.SelectedIndex = 1 Then

                Me.AssignValue("EDIT")

            End If

            ds_Save = New DataSet
            Dt_WSAll = CType(Me.ViewState(VS_DtWSFullDetail), DataTable)
            Dt_WSAll.TableName = "Proc_ParentToChild"
            ds_Save.Tables.Add(Dt_WSAll)

            dt_SaveDtinfo = CType(Me.ViewState(VS_DtChildProtocolDtlMatrix), DataTable)
            dt_SaveDtinfo.TableName = "workspaceprotocoldetailmatrix"
            ds_Save.Tables.Add(dt_SaveDtinfo)

            If Not objLambda.Save_ChildWorkSpaceMstSite(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_ChildWorkSpaceMstSite, ds_Save, Me.Session(S_UserID), eStr) Then
                ObjCommon.ShowAlert("Error While Saving Site Project", Me.Page)
                Exit Sub
            Else
                ObjCommon.ShowAlert("Child/Site Project Saved Successfully", Me.Page)
                Me.ResetPage()
                dt_SaveDtinfo.Rows.Clear()
                Dt_WSAll.Rows.Clear()

            End If

        Catch ex As Exception
            Me.ResetPage()
            Me.ShowErrorMessage(ex.Message, ".........btnSave_Click")
        End Try

    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmmainPage.aspx", False)
    End Sub

#End Region

#Region "Create Table"

    Private Function CreateTable() As Boolean
        Dim Dt_WSFullDtl As New DataTable
        Dim dc As DataColumn
        Try

            dc = New DataColumn
            dc.ColumnName = "vParentWorkspaceId"
            dc.DataType = GetType(String)
            Dt_WSFullDtl.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "vWorkSpaceDesc"
            dc.DataType = GetType(String)
            Dt_WSFullDtl.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "cWorkspaceType"
            dc.DataType = GetType(String)
            Dt_WSFullDtl.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "vProjectNo"
            dc.DataType = GetType(String)
            Dt_WSFullDtl.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "vRequestId"
            dc.DataType = GetType(String)
            Dt_WSFullDtl.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "vLocationCode"
            dc.DataType = GetType(String)
            Dt_WSFullDtl.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "iProjectManagerId"
            dc.DataType = GetType(Integer)
            Dt_WSFullDtl.Columns.Add(dc)

            'nWorkTypeNo,cFastingFed,nRetaintionPeriod
            dc = New DataColumn
            dc.ColumnName = "nWorkTypeNo"
            dc.DataType = GetType(Integer)
            Dt_WSFullDtl.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "cFastingFed"
            dc.DataType = GetType(String)
            Dt_WSFullDtl.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "nRetaintionPeriod"
            dc.DataType = GetType(Integer)
            Dt_WSFullDtl.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "iNoOfSubjects"
            dc.DataType = GetType(Integer)
            Dt_WSFullDtl.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "iModifyBy"
            dc.DataType = GetType(Integer)
            Dt_WSFullDtl.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "vWorkspaceId"
            dc.DataType = GetType(String)
            Dt_WSFullDtl.Columns.Add(dc)

            dc = New DataColumn                'Added By Mani For New Column in WorkSpaceProtocoldetail
            dc.ColumnName = "cIsTestSite"
            dc.DataType = GetType(String)
            Dt_WSFullDtl.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "vExternalProjectNo"
            dc.DataType = GetType(String)
            Dt_WSFullDtl.Columns.Add(dc)

            Me.ViewState(VS_DtWSFullDetail) = Dt_WSFullDtl
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "..........CreateTable")
            Return False
        End Try
    End Function

#End Region

#Region "AssignValue"

    Protected Sub AssignValue(ByVal Type As String)

        Dim dr As DataRow
        Dim dt_WorkspaceAll As New DataTable

        Try

            Me.SetWorkspaceDesc()
            Me.ViewState(Vs_LocationCode) = Me.ddlLocation.SelectedItem.Value.Trim()

            dt_WorkspaceAll = CType(Me.ViewState(VS_DtWSFullDetail), DataTable)
            dr = dt_WorkspaceAll.NewRow()
            dr("vParentWorkspaceId") = Me.HProjectId.Value.Trim()
            dr("vWorkSpaceDesc") = Me.ViewState(VS_WorkspaceDesc)
            dr("cWorkspaceType") = "C"
            dr("vProjectNo") = Me.txtGetChildVersion.Text.Trim()
            dr("vRequestId") = Me.txtChildReqID.Text.Trim()
            dr("vLocationCode") = Me.ViewState(Vs_LocationCode)
            dr("iProjectManagerId") = Me.ddlmanager.SelectedItem.Value.Trim()
            'nWorkTypeNo,cFastingFed,nRetaintionPeriod
            dr("nWorkTypeNo") = Me.ddlStudytype.SelectedValue.Trim()
            If Me.chkFastfedYes.Checked = True Then
                dr("cFastingFed") = "FS"
            ElseIf Me.chkFastfedNo.Checked = True Then
                dr("cFastingFed") = "FD"
            End If

            If Me.txtRetentionPeriod.Text.Trim() <> "" Then
                'dr("nRetaintionPeriod") = Convert.ToInt32(txtRetentionPeriod.Text.Trim.Substring(0, Convert.ToString(txtRetentionPeriod.Text).Trim.IndexOf(".")))
                dr("nRetaintionPeriod") = Convert.ToInt32(txtRetentionPeriod.Text)
            End If

            If Me.txtnoofprjct.Text.Trim() <> "" Then
                dr("iNoOfSubjects") = Me.txtnoofprjct.Text.Trim()
            End If

            dr("iModifyBy") = Me.Session(S_UserID)
            If Me.chkTestSite.Checked = True Then       ' Added By Mani to indicate Test Site..for Child Project 
                dr("cIsTestSite") = "Y"
            Else
                dr("cIsTestSite") = "N"
            End If


            If Type = "EDIT" Then
                txtBiznetProjectNo.Enabled = True
                dr("vworkspaceid") = Me.HProjectId.Value.Trim()
                dr("vExternalProjectNo") = Me.txtBiznetProjectNo.Text.Trim()
            End If

            dt_WorkspaceAll.Rows.Add(dr)
            Me.ViewState(VS_DtWSFullDetail) = dt_WorkspaceAll

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......AssignValue")
        End Try

    End Sub

    Private Sub SetWorkspaceDesc() 'Replace WorkspaceDesc with current Locationinitiate
        Dim selectedWsDesc As String = String.Empty
        Try
            selectedWsDesc = Me.ViewState(VS_WorkspaceDesc)
            selectedWsDesc = selectedWsDesc.Substring(0, selectedWsDesc.LastIndexOf("-"))
            selectedWsDesc = selectedWsDesc + "-" + Me.ViewState(VS_LocationInit)
            Me.ViewState(VS_WorkspaceDesc) = selectedWsDesc
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".........SetWorkspaceDesc")
        End Try
    End Sub

    Private Sub AssignValueforGrid(ByVal Type As String)

        Dim dr As DataRow
        Dim dt_Info As New DataTable
        dt_Info = CType(Me.ViewState(VS_DtChildProtocolDtlMatrix), DataTable)
        dr = dt_Info.NewRow()

        Try
            If Type = "CD" Then

                dr("vType") = "CD"
                dr("vName") = Me.txtContName.Text.Trim()
                dr("vAddress1") = Me.txtContAdds1.Text.Trim()
                dr("vAddress2") = Me.txtContAdds2.Text.Trim()
                dr("vAddress3") = Me.txtContAdds3.Text.Trim()
                dr("vTeleNo") = Me.txtContTelno.Text.Trim()
                dr("vExtNo") = Me.txtContExtNo.Text.Trim()
                dr("vFaxNo") = "NA"
                dr("vDesignation") = Me.txtContDesig.Text.Trim()
                dr("vQualification") = Me.txtContQuali.Text.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dr("nWorkspaceProtocolDetailMatrixNo") = Convert.ToInt32(ViewState(Vs_CDIndexADD))
                ViewState(Vs_CDIndexADD) += 1

            ElseIf Type = "ID" Then

                dr("vType") = "ID"
                dr("vName") = Me.txtInvsName.Text.Trim()
                dr("vAddress1") = Me.txtInvsAdds1.Text.Trim()
                dr("vAddress2") = Me.TxtInvsAdds2.Text.Trim()
                dr("vAddress3") = Me.txtInvsAdds3.Text.Trim()
                dr("vTeleNo") = Me.txtInvstele.Text.Trim()
                dr("vExtNo") = Me.txtInvsExtes.Text.Trim()
                dr("vFaxNo") = "NA"
                dr("vDesignation") = Me.txtInvsDesig.Text.Trim()
                dr("vQualification") = Me.TxtInvsqalific.Text.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dr("nWorkspaceProtocolDetailMatrixNo") = Convert.ToInt32(ViewState(Vs_IDIndexADD))
                ViewState(Vs_IDIndexADD) += 1

            End If

            dt_Info.Rows.Add(dr)
            Me.ViewState(VS_DtChildProtocolDtlMatrix) = dt_Info

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......AssignValueforGrid")
        End Try

    End Sub

#End Region

#Region "Reset Controls"

    Private Sub ResetContactControls()

        Me.txtContName.Text = ""
        Me.txtContAdds1.Text = ""
        Me.txtContAdds2.Text = ""
        Me.txtContAdds3.Text = ""
        Me.txtContDesig.Text = ""
        Me.txtContExtNo.Text = ""
        Me.txtContQuali.Text = ""
        Me.txtContTelno.Text = ""

    End Sub

    Private Sub ResetInvestControls()

        Me.txtInvsName.Text = ""
        Me.txtInvsAdds1.Text = ""
        Me.TxtInvsAdds2.Text = ""
        Me.txtInvsAdds3.Text = ""
        Me.txtInvsDesig.Text = ""
        Me.txtInvsExtes.Text = ""
        Me.TxtInvsqalific.Text = ""
        Me.txtInvstele.Text = ""

    End Sub

#End Region

#Region "Contact Gridview Events"

    Protected Sub GvwContactDtl_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvwContactDtl.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or _
            e.Row.RowType = DataControlRowType.Header Or _
            e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GvwCont_MatrixNo).Visible = False
            e.Row.Cells(GvwContType).Visible = False

        End If
    End Sub

    Protected Sub GvwContactDtl_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim index As Int32
        Dim IndexDelete As Int32
        Dim DTDELETE As New DataTable
        Dim DVDELETE As New DataView
        Dim dvEdit As DataView

        Try
            index = CInt(e.CommandArgument)
            ViewState(Vs_SelectedIndex_CD) = index

            If e.CommandName.ToUpper = "ADELETE" Then

                DTDELETE = CType(Me.ViewState(VS_DtChildProtocolDtlMatrix), DataTable)

                For IndexDelete = 0 To DTDELETE.Rows.Count - 1

                    If DTDELETE.Rows(IndexDelete).Item("vType") = "CD" And _
                        Convert.ToString(DTDELETE.Rows(IndexDelete).Item("vName")).Trim = Replace(GvwContactDtl.Rows(index).Cells(1).Text, "&nbsp;", " ").Trim And _
                        Convert.ToString(DTDELETE.Rows(IndexDelete).Item("vAddress1")).Trim = GvwContactDtl.Rows(index).Cells(2).Text.Replace("&nbsp;", " ").Trim.Replace("&amp;", "&").Trim And _
                        Convert.ToString(DTDELETE.Rows(IndexDelete).Item("vAddress2")).Trim = GvwContactDtl.Rows(index).Cells(3).Text.Replace("&nbsp;", " ").Trim.Replace("&amp;", "&").Trim And _
                        Convert.ToString(DTDELETE.Rows(IndexDelete).Item("vAddress3")).Trim = GvwContactDtl.Rows(index).Cells(4).Text.Replace("&nbsp;", " ").Trim.Replace("&amp;", "&").Trim Then

                        'DTDELETE.Rows(IndexDelete).Item("vName") = Replace(GvwContactDtl.Rows(index).Cells(1).Text, "&nbsp;", " ").Trim And _
                        'DTDELETE.Rows(IndexDelete).Item("vAddress1") = Replace(GvwContactDtl.Rows(index).Cells(2).Text, "&nbsp;", " ").Trim And _
                        'DTDELETE.Rows(IndexDelete).Item("vAddress2") = Replace(GvwContactDtl.Rows(index).Cells(3).Text, "&nbsp;", " ").Trim And _
                        'DTDELETE.Rows(IndexDelete).Item("vAddress3") = Replace(GvwContactDtl.Rows(index).Cells(4).Text, "&nbsp;", " ").Trim Then

                        If DTDELETE.Rows(IndexDelete).Item("vWorkSpaceId") Is System.DBNull.Value Then
                            DTDELETE.Rows(IndexDelete).Delete()
                            DTDELETE.AcceptChanges()
                            Exit For
                        Else
                            If DTDELETE.Rows(IndexDelete).Item("vWorkSpaceId") = "" Then
                                DTDELETE.Rows(IndexDelete).Delete()
                                DTDELETE.AcceptChanges()
                                Exit For
                            Else
                                DTDELETE.Rows(IndexDelete).Item("vWorkSpaceId") = System.DBNull.Value
                                DTDELETE.Rows(IndexDelete).Item("cStatusIndi") = "D"
                                Exit For
                            End If
                        End If

                    End If

                Next IndexDelete

                Me.ViewState(VS_DtChildProtocolDtlMatrix) = DTDELETE
                DVDELETE = CType(Me.ViewState(VS_DtChildProtocolDtlMatrix), DataTable).DefaultView
                DVDELETE.RowFilter = "vType = 'CD' and cStatusIndi<>'D'"
                Me.GvwContactDtl.DataSource = DVDELETE.ToTable
                Me.GvwContactDtl.DataBind()

            ElseIf e.CommandName.ToUpper = "EDITCD" Then

                Dim Dt_CD As New DataTable
                dvEdit = CType(Me.ViewState(VS_DtChildProtocolDtlMatrix), DataTable).DefaultView()
                dvEdit.RowFilter = "vType = 'CD' and cStatusIndi<>'D'"
                Dt_CD = dvEdit.ToTable()
                txtContName.Text = IIf(Dt_CD.Rows(index).Item("vName") Is System.DBNull.Value, "", Dt_CD.Rows(index).Item("vName"))
                txtContAdds1.Text = IIf(Dt_CD.Rows(index).Item("vAddress1") Is System.DBNull.Value, "", Dt_CD.Rows(index).Item("vAddress1"))
                txtContAdds2.Text = IIf(Dt_CD.Rows(index).Item("vAddress2") Is System.DBNull.Value, "", Dt_CD.Rows(index).Item("vAddress2"))
                txtContAdds3.Text = IIf(Dt_CD.Rows(index).Item("vAddress3") Is System.DBNull.Value, "", Dt_CD.Rows(index).Item("vAddress3"))
                txtContTelno.Text = IIf(Dt_CD.Rows(index).Item("vTeleNo") Is System.DBNull.Value, "", Dt_CD.Rows(index).Item("vTeleNo"))
                txtContExtNo.Text = IIf(Dt_CD.Rows(index).Item("vExtNo") Is System.DBNull.Value, "", Dt_CD.Rows(index).Item("vExtNo"))
                txtContDesig.Text = IIf(Dt_CD.Rows(index).Item("vDesignation") Is System.DBNull.Value, "", Dt_CD.Rows(index).Item("vDesignation"))
                txtContQuali.Text = IIf(Dt_CD.Rows(index).Item("vQualification") Is System.DBNull.Value, "", Dt_CD.Rows(index).Item("vQualification"))
                btnContAdd.Text = "Update"

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".......GvwContactDtl_RowCommand")
        End Try

    End Sub

    Protected Sub GvwContactDtl_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("ImbADelete"), ImageButton).CommandName = "ADelete"
            CType(e.Row.FindControl("ImbADelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("LnkBtnEditForCD"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("LnkBtnEditForCD"), ImageButton).CommandName = "EditCD"
            CType(e.Row.FindControl("ImbADelete"), ImageButton).Attributes.Add("OnClick", "return show_confirm()")
        End If
    End Sub

    Protected Sub GvwContactDtl_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)

    End Sub

#End Region

#Region "Investigation Grid Details"

    Protected Sub GvwInvestDtl_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvwInvestDtl.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or _
           e.Row.RowType = DataControlRowType.Header Or _
           e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GvwInv_MatrixNo).Visible = False
            e.Row.Cells(GvwInvType).Visible = False

        End If
    End Sub

    Protected Sub GvwInvestDtl_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim dv_Invinfo As DataView
        Dim Index As Int32
        Dim IndexDelete As Int32
        Dim DTDELETE As New DataTable
        Dim DVDELETE As New DataView

        Try
            Index = CInt(e.CommandArgument)
            ViewState(Vs_SelectedIndex_ID) = Index

            If e.CommandName.ToUpper = "MDELETE" Then

                DTDELETE = CType(Me.ViewState(VS_DtChildProtocolDtlMatrix), DataTable)
                For IndexDelete = 0 To DTDELETE.Rows.Count - 1
                    If DTDELETE.Rows(IndexDelete).Item("vType") = "ID" And _
                       Convert.ToString(DTDELETE.Rows(IndexDelete).Item("vName")).Trim = Replace(GvwInvestDtl.Rows(Index).Cells(1).Text, "&nbsp;", " ").Trim And _
                        Convert.ToString(DTDELETE.Rows(IndexDelete).Item("vAddress1")).Trim = GvwInvestDtl.Rows(Index).Cells(2).Text.Replace("&nbsp;", " ").Trim.Replace("&amp;", "&").Trim And _
                        Convert.ToString(DTDELETE.Rows(IndexDelete).Item("vAddress2")).Trim = GvwInvestDtl.Rows(Index).Cells(3).Text.Replace("&nbsp;", " ").Trim.Replace("&amp;", "&").Trim And _
                        Convert.ToString(DTDELETE.Rows(IndexDelete).Item("vAddress3")).Trim = GvwInvestDtl.Rows(Index).Cells(4).Text.Replace("&nbsp;", " ").Trim.Replace("&amp;", "&").Trim Then

                        'DTDELETE.Rows(IndexDelete).Item("vName") = Replace(GvwInvestDtl.Rows(Index).Cells(1).Text, "&nbsp;", " ").Trim And _
                        'DTDELETE.Rows(IndexDelete).Item("vAddress1") = Replace(GvwInvestDtl.Rows(Index).Cells(2).Text, "&nbsp;", " ").Trim And _
                        'DTDELETE.Rows(IndexDelete).Item("vAddress2") = Replace(GvwInvestDtl.Rows(Index).Cells(3).Text, "&nbsp;", " ").Trim And _
                        'DTDELETE.Rows(IndexDelete).Item("vAddress3") = Replace(GvwInvestDtl.Rows(Index).Cells(4).Text, "&nbsp;", " ").Trim Then

                        If DTDELETE.Rows(IndexDelete).Item("vWorkSpaceId") Is System.DBNull.Value Then
                            DTDELETE.Rows(IndexDelete).Delete()
                            DTDELETE.AcceptChanges()
                            Exit For
                        Else
                            If DTDELETE.Rows(IndexDelete).Item("vWorkSpaceId") = "" Then
                                DTDELETE.Rows(IndexDelete).Delete()
                                DTDELETE.AcceptChanges()
                                Exit For
                            Else
                                DTDELETE.Rows(IndexDelete).Item("vWorkSpaceId") = System.DBNull.Value
                                DTDELETE.Rows(IndexDelete).Item("cStatusIndi") = "D"
                                Exit For
                            End If
                        End If

                    End If

                Next IndexDelete

                Me.ViewState(VS_DtChildProtocolDtlMatrix) = DTDELETE
                DVDELETE = CType(Me.ViewState(VS_DtChildProtocolDtlMatrix), DataTable).DefaultView
                DVDELETE.RowFilter = "vType = 'ID' and cStatusIndi<>'D'"
                Me.GvwInvestDtl.DataSource = DVDELETE.ToTable
                Me.GvwInvestDtl.DataBind()

            ElseIf e.CommandName.ToUpper = "EDITID" Then

                dv_Invinfo = CType(Me.ViewState(VS_DtChildProtocolDtlMatrix), DataTable).DefaultView
                dv_Invinfo.RowFilter = "vType = 'ID' and cStatusIndi<>'D'"

                Dim Dt_ID As New DataTable
                Dt_ID = dv_Invinfo.ToTable()
                txtInvsName.Text = Dt_ID.Rows(Index).Item("vName")
                txtInvsAdds1.Text = Dt_ID.Rows(Index).Item("vAddress1")
                TxtInvsAdds2.Text = Dt_ID.Rows(Index).Item("vAddress2")
                txtInvsAdds3.Text = Dt_ID.Rows(Index).Item("vAddress3")
                txtInvstele.Text = Dt_ID.Rows(Index).Item("vTeleNo")
                txtInvsExtes.Text = Dt_ID.Rows(Index).Item("vExtNo")
                txtInvsDesig.Text = Dt_ID.Rows(Index).Item("vDesignation")
                TxtInvsqalific.Text = Dt_ID.Rows(Index).Item("vQualification")
                btnInvesAdd.Text = "Update"
                btnInvesAdd.ToolTip = "Update"

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......GvwInvestDtl_RowCommand")
        End Try

    End Sub

    Protected Sub GvwInvestDtl_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("ImbMDelete"), ImageButton).CommandName = "MDelete"
            CType(e.Row.FindControl("ImbMDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("LnkBtnEditForID"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("LnkBtnEditForID"), ImageButton).CommandName = "EditID"
            CType(e.Row.FindControl("ImbMDelete"), ImageButton).Attributes.Add("OnClick", "return show_confirm()")
        End If
    End Sub

    Protected Sub GvwInvestDtl_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)

    End Sub

#End Region

#Region "Reset Page "

    Private Sub ResetPage()

        Me.GvwContactDtl.DataSource = Nothing
        Me.GvwContactDtl.DataBind()
        Me.GvwInvestDtl.DataSource = Nothing
        Me.GvwInvestDtl.DataBind()
        Me.ddlmanager.SelectedIndex = -1
        Me.ddlLocation.SelectedIndex = -1
        Me.txtnoofprjct.Text = ""
        Me.txtChildReqID.Text = ""
        Me.txtGetChildVersion.Text = ""
        Me.txtProject.Text = ""
        txtRetentionPeriod.Text = ""
        Me.ViewState(Vs_LocationCode) = Nothing
        Me.HProjectId.Value = ""
        Me.ViewState(VS_WorkspaceDesc) = Nothing
        Me.ViewState(VS_LocationInit) = Nothing
        Me.chkTestSite.Checked = False
        Me.chkFastfedYes.Checked = False
        Me.chkFastfedNo.Checked = False
        Me.txtBiznetProjectNo.Text = "" 'add by vishal mameriya
        Me.ddlStudytype.SelectedIndex = -1
        GenCall()
        ResetContactControls()
        ResetInvestControls()

    End Sub

#End Region

#Region "Dropdown Selection Change"

    Protected Sub ddlLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds_LocationInitial As New DataSet

        Try

            If Me.ddlLocation.SelectedIndex > 0 Then

                Me.ViewState(Vs_LocationCode) = Me.ddlLocation.SelectedItem.Value.Trim()
                ds_LocationInitial = Me.objHelp.GetResultSet("select vLocationInitiate from LocationMst where vLocationCode = '" + Me.ViewState(Vs_LocationCode) + "'", "LocationMst ")
                If ds_LocationInitial.Tables(0).Rows.Count > 0 Then
                    Me.ViewState(VS_LocationInit) = ds_LocationInitial.Tables(0).Rows(0)("vLocationInitiate").ToString()
                End If

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "........ddlLocation_SelectedIndexChanged")
        End Try

    End Sub

#End Region

#Region "RadioButton Change"

    Protected Sub RblListCreateAndEditChildProject_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RblListCreateAndEditChildProject.SelectedIndexChanged

        Try

            If RblListCreateAndEditChildProject.SelectedIndex = 0 Then
                AutoCompleteExtender1.ContextKey = "isnull(vParentWorkspaceId,'') = ''"
                'txtProject.Text = ""
                btnContAdd.Text = "Add"
                btnInvesAdd.Text = "Add"
                ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
                lblProject.Text = "Parent Project :"
            Else
                AutoCompleteExtender1.ContextKey = "isnull(vParentWorkspaceId,'') <> ''"
                txtProject.Text = ""
                Me.HProjectId.Value = ""
                ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                lblProject.Text = " Child/Site Project :"
            End If
            Me.ResetPage()
            ResetContactControls()
            ResetInvestControls()
            GvwContactDtl.Visible = False
            GvwInvestDtl.Visible = False
            ddlLocation.SelectedIndex = -1
            ddlmanager.SelectedIndex = -1
            ddlStudytype.SelectedIndex = -1
            txtChildReqID.Text = ""
            txtGetChildVersion.Text = ""
            txtnoofprjct.Text = ""
            txtRetentionPeriod.Text = ""

            Me.tblDataEntry.Style.Add("display", "none")

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".......RblListCreateAndEditChildProject_SelectedIndexChanged")
        End Try

    End Sub

#End Region

#Region "CheckDuplicateSite"
    <Services.WebMethod()> _
    Public Shared Function CheckDuplicateSite(ByVal vProjectNo As String, ByVal vWorkspaceId As String) As String
        Dim Ds_Site As DataSet = Nothing
        Dim ErrorMsg As String = String.Empty
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()

        wStr = " vProjectNo = '" & vProjectNo.ToString() & "' And vworkspaceId <> '" & vWorkspaceId.ToString() & "' And cStatusIndi <> 'D'"

        If Not objHelp.GetWorkspaceProtocolDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_Site, eStr) Then
            Return eStr.ToString()
        End If

        If Ds_Site.Tables(0).Rows.Count > 0 Then
            ErrorMsg = "The Site Name Already Exists"
        End If
        Return ErrorMsg
    End Function

#End Region

    'Add by shivani pandya for Project lock
#Region "LockImpact"
    <Services.WebMethod()> _
    Public Shared Function LockImpact(ByVal WorkspaceID As String) As String
        Dim ds_Check As DataSet = Nothing
        Dim dtProjectStatus As New DataTable
        Dim ProjectStatus As String = String.Empty
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim iTran As String = String.Empty
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()

        wStr = "vWorkspaceId = '" + WorkspaceID.ToString() + "' And cStatusIndi <> 'D'"

        If Not objHelp.GetCRFLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_Check, eStr) Then
            Throw New Exception(eStr)
        End If

        If ds_Check.Tables(0).Rows.Count > 0 Then

            dtProjectStatus = ds_Check.Tables(0)
            iTran = dtProjectStatus.Compute("Max(iTranNo)", String.Empty)
            dtProjectStatus.DefaultView.RowFilter = "iTranNo =" + iTran.ToString()

            If dtProjectStatus.DefaultView.ToTable.Rows.Count > 0 Then
                ProjectStatus = dtProjectStatus.DefaultView.ToTable().Rows(0)("cLockFlag").ToString()
            End If
        Else
            ProjectStatus = "U"
        End If

        Return ProjectStatus
    End Function
#End Region

End Class
