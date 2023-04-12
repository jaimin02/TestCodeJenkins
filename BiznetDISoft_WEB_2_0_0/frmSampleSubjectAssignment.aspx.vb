
Partial Class frmSampleSubjectAssignment
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION"

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtSampleDetailGrid As String = "SampleDetailGrid"
    Private Const VS_DtSampleTypeDetail As String = "SampleTypeDetail"
    Private Const Vs_dsSampleTypeDetail As String = "View_SampleTypeDetail"
    Private Const VS_DtSave As String = "Dt_Save"

    Private Const GVC_SampleTypeNo As Integer = 0
    Private Const GVC_SampleBarCode As Integer = 1
    Private Const GVC_SubjectId As Integer = 2
    Private Const GVC_SubjectNo As Integer = 3
    Private Const GVC_iMySubjectNo As Integer = 4
    Private Const GVC_SubjectName As Integer = 5
    Private Const GVC_ProjectName As Integer = 6
    Private Const GVC_Node As Integer = 7
    Private Const GVC_Period As Integer = 8
    Private Const GVC_CollectionTime As Integer = 9
    Private Const GVC_ModifyOn As Integer = 10
    Private Const GVC_CollectionBy As Integer = 11
    Private Const GVC_DeptCode As Integer = 12
    Private Const Str_CPMADept As String = "0002"
    Private Const Str_BABEDept As String = "0004"
    Private Const strProjectType_Housing As String = "0016"

#End Region

#Region "Form Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Not IsPostBack Then
                GenCall()
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....Page_Load")
        End Try
    End Sub

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try

            Choice = "2"
            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

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

#Region "GenCall_Data"

    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean

        Dim eStr_Retu As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing

        Try

            Choice = Me.ViewState(VS_Choice)

            If Not objHelp.getSampleTypeDetail("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                      ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            Me.ViewState(VS_DtSampleTypeDetail) = ds.Tables(0)


            ds_DWR_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_Data")
        Finally
        End Try

    End Function

#End Region

#Region "GenCall_showUI"

    Private Function GenCall_ShowUI() As Boolean
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim sender As New Object
        Dim e As New EventArgs

        Try
            Page.Title = ":: Sample Collection  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Safety Sample Collection"
            Choice = Me.ViewState(VS_Choice)

            Me.txtScan.Focus()
            ddlType_SelectedIndexChanged(sender, e)

            If Not FillgvwSubjectSample() Then
                Exit Function
            End If


            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....GenCall_ShowUI")
        Finally
        End Try
    End Function

#End Region

#Region "FillGrid"

    Private Function FillgvwSubjectSample() As Boolean
        Dim ds_gvwSampleDetail As New Data.DataSet
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim Wstr_Scope As String = String.Empty
        Dim dv_gvwSampleDetail As New DataView
        Try

            Me.gvwSubjectSample.DataSource = Nothing
            Me.gvwSubjectSample.DataBind()

            'To Get Where condition of ScopeVales( Project Type )
            If Not ObjCommon.GetScopeValueWithCondition(Wstr_Scope) Then
                Return False
            End If

            If Me.RBLProjecttype.SelectedValue <> "0000000000" Then

                wstr = " vWorkspaceid = '" + Me.HProjectId.Value.Trim + "' And iNodeID= '" + Me.ddlActivity.SelectedValue.ToString + "'"
                wstr += " AND dCollectionDateTime is not NULL "
                wstr += " And cStatusIndi <> 'D' And (vSubjectId is Not NULL or vSubjectId <> '') And " + Wstr_Scope
            ElseIf Me.RBLProjecttype.SelectedValue = "0000000000" Then
                wstr = "Convert(varchar(11), dCollectionDateTime, 113) = Convert(varchar(11), getDate(), 113)"
                wstr += "And vWorkspaceid = '0000000000' "
                wstr += " And cStatusIndi <> 'D' And (vSubjectId Is Not NULL Or vSubjectId <> '') And " + Wstr_Scope
            End If

            wstr += " And vLocationCode = '" + Me.Session(S_LocationCode) + "'"
            wstr += " AND  vSampleBarCodeId NOT LIKE '%P%'  "
            If Not objHelp.View_SampleTypeDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_gvwSampleDetail, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From View_SampleDetail", estr)
                Return False
            End If

            If ds_gvwSampleDetail.Tables(0).Rows.Count > 0 Then
                dv_gvwSampleDetail = ds_gvwSampleDetail.Tables(0).DefaultView
                dv_gvwSampleDetail.Sort = "dCollectionDateTime desc"

                Me.gvwSubjectSample.DataSource = dv_gvwSampleDetail
                Me.gvwSubjectSample.DataBind()
                Me.ViewState(VS_DtSampleDetailGrid) = ds_gvwSampleDetail.Tables(0)

                Me.lblCount.Text = "No. Of Samples Collected : " + Me.gvwSubjectSample.Rows.Count.ToString()
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillgvwSubjectSample")
            Return False
        End Try
    End Function

#End Region

#Region "Fill Dropdown"
    Private Sub FillddlSubjectID()
        Dim ds_ddlSubjectID As New DataSet
        Dim ds_SampleTypeDetail As New DataSet
        Dim dv_ddlSubjectID1 As New DataView
        Dim wstr As String = String.Empty
        Dim dv_ddlSubjectId As New DataView
        Dim dt_ddlSubjectID As New DataTable
        Dim eStr_Retu As String = String.Empty

        Try

            ddlSubjectID.Items.Clear()
            If Me.RBLProjecttype.SelectedValue = "0000000000" Then
                wstr = "vWorkspaceid = '0000000000' "
            ElseIf Me.RBLProjecttype.SelectedValue <> "0000000000" Then
                wstr = "vWorkspaceid = '" + Me.HProjectId.Value + "' "
                wstr += " And iNodeId = " + Me.ddlActivity.SelectedValue
            End If



            'wstr += " And convert(varchar(11),dModifyOn,113) = Convert(varchar(11), getDate(), 113)"
            wstr += " AND  vSampleBarCodeId NOT LIKE '%P%'  "
            wstr += " And vLocationCode = '" & Me.Session(S_LocationCode) & "' ANd dCollectionDateTime is NULL order by iMySubjectNo"

            If Not objHelp.View_SampleTypeDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                 ds_ddlSubjectID, eStr_Retu) Then
                ObjCommon.ShowAlert("Error While Getting Data", Me)
                Exit Sub
            End If

            If ds_ddlSubjectID.Tables(0).Rows.Count <= 0 Then
                ddlType.SelectedValue = "1"
                ObjCommon.ShowAlert("No Sample Collection Is Done Today", Me)
                Exit Sub
            End If
            ''dv_ddlSubjectId = ds_ddlSubjectID.Tables(0).DefaultView
            ''dv_ddlSubjectId.RowFilter = "dCollectionDateTime is NULL"

            ''dv_ddlSubjectId.ToTable()

            ''ds_ddlSubjectID = New DataSet
            ''ds_ddlSubjectID.Tables.Add(dv_ddlSubjectId.ToTable().Copy())

            ViewState(Vs_dsSampleTypeDetail) = ds_ddlSubjectID
            dv_ddlSubjectId = ds_ddlSubjectID.Tables(0).DefaultView
            dt_ddlSubjectID = dv_ddlSubjectId.ToTable(True, "vSubjectId,vMySubjectNo".Split(",")).Copy

            ddlSubjectID.Items.Insert(0, New ListItem("--Select Subject--"))

            For Index As Integer = 1 To dt_ddlSubjectID.Rows.Count

                ddlSubjectID.Items.Insert(Index, New ListItem(Convert.ToString(dt_ddlSubjectID.Rows(Index - 1)("vSubjectID")) _
                & "(" & dt_ddlSubjectID.Rows(Index - 1)("vMySubjectNo") & ")", Convert.ToString(dt_ddlSubjectID.Rows(Index - 1)("vSubjectID"))))

            Next

            ddlSubjectID.SelectedIndex = 0
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillddlSubjectID")
        End Try

    End Sub

    Private Sub fillChklSampleId()
        Dim ds_ddlsampleId As New DataSet
        Dim dv_ddlsampleId As New DataView
        Try
            ds_ddlsampleId = CType(ViewState(Vs_dsSampleTypeDetail), DataSet)
            dv_ddlsampleId = ds_ddlsampleId.Tables(0).DefaultView
            dv_ddlsampleId.RowFilter = "vSubjectId='" & Me.ddlSubjectID.SelectedValue.ToString & "' and cStatusIndi <> 'D'"
            ChklSampleId.DataSource = dv_ddlsampleId.ToTable
            ChklSampleId.DataTextField = "vSampleBarCode"
            ChklSampleId.DataValueField = "nSampleId"
            ChklSampleId.DataBind()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....fillChklSampleId")
        End Try
    End Sub

    Private Function FillddlActivity() As Boolean
        Dim wstr As String = String.Empty
        Dim Ds_FillActivity As New DataSet
        Dim Dv_FillActivity As New DataView
        Dim Dt_FillActivity As New DataTable
        Dim eStr_Retu As String = String.Empty
        Try

            wstr = " vWorkspaceId = '" & Me.HProjectId.Value.Trim() & "'"
            wstr += " And cStatusindi<>'D'"


            If Not objHelp.Get_ViewSampleDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                               Ds_FillActivity, eStr_Retu) Then

                Throw New Exception(eStr_Retu)

            End If

            'wstr += "' And (vActivityId is Not NULL or vActivityId<>'') and cstatusindi<>'D' order by vActivityName"

            'If Not objHelp.GetViewWorkSpaceNodeDetail(wstr, Ds_FillActivity, eStr_Retu) Then
            '    Throw New Exception(eStr_Retu)
            'End If

            Dv_FillActivity = Ds_FillActivity.Tables(0).DefaultView
            Dt_FillActivity = Dv_FillActivity.ToTable(True, "iNodeId,vNodeDisplayName".Split(","))

            If Dt_FillActivity.Rows.Count <= 0 Then
                ObjCommon.ShowAlert("No Safety Sample Is Created For Selected Project.", Me)
                Return True
                Exit Function
            End If

            Me.ddlActivity.DataSource = Dt_FillActivity
            Me.ddlActivity.DataTextField = "vNodeDisplayName"
            Me.ddlActivity.DataValueField = "iNodeid"
            Me.ddlActivity.DataBind()


            Me.ddlActivity.Items.Insert(0, New ListItem("Select Activity", 0))


            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getData", "getData();", True)

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillddlActivity")
            Return False
        End Try
    End Function

#End Region

#Region "LockUnloCkCheckBox"
    Function LockUnlock() As Boolean
        If Me.hndLockStatus.Value.Trim() = "Lock" Then
            ddlType.Enabled = False
        Else
            ddlType.Enabled = True
        End If
        Return True
    End Function
#End Region

#Region "Selected Index Change Event"

    Protected Sub ddlSubjectID_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSubjectID.SelectedIndexChanged

        fillChklSampleId()
        Me.pnlSubject.Attributes.Add("style", "''")

        Me.BtncollectSample.Enabled = True

        If (Me.ddlSubjectID.SelectedIndex.Equals(0)) Then
            Me.pnlSubject.Attributes.Add("style", "display:none")
            Me.BtncollectSample.Enabled = False
        End If

    End Sub

    Protected Sub RBLProjecttype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLProjecttype.SelectedIndexChanged
        If Me.RBLProjecttype.SelectedValue = "0000000000" Then
            lblCount.Text = ""
            Me.trProject.Attributes.Add("style", "display:none")
            Me.trActivity.Attributes.Add("style", "display:none")
            Me.trWoutScanner.Attributes.Add("style", "display:none")
            Me.trWoutScanner1.Attributes.Add("style", "display:none")
            Me.ddlSubjectID.Items.Clear()
            Me.ChklSampleId.Items.Clear()
            Me.BtncollectSample.Visible = True
            'Me.btnManualCollect.Visible = False
            ddlType.SelectedValue = "1"


            If Me.RBLProjecttype.SelectedValue = "0000000000" Then
                Me.BtncollectSample.Visible = True
            ElseIf Me.RBLProjecttype.SelectedValue <> "0000000000" Then
                'Me.btnManualCollect.Visible = True
            End If

            If Not FillgvwSubjectSample() Then
                Exit Sub
            End If

        Else
            lblCount.Text = ""
            Me.trProject.Attributes.Add("style", "''")
            Me.trActivity.Attributes.Add("style", "''")
            ddlType.SelectedValue = "1"
            Me.BtncollectSample.Visible = True
            Me.chkwithoutscanner.Checked = False
            'Me.btnManualCollect.Visible = True
            Me.ddlSubjectID.Items.Clear()
            Me.ChklSampleId.Items.Clear()



            If Me.RBLProjecttype.SelectedValue = "0000000000" Then
                Me.BtncollectSample.Visible = True
            ElseIf Me.RBLProjecttype.SelectedValue <> "0000000000" Then
                'Me.btnManualCollect.Visible = True
            End If


            If Me.HProjectId.Value <> "" AndAlso Me.ddlActivity.SelectedValue <> 0 Then

                If Not FillgvwSubjectSample() Then
                    Exit Sub
                End If
            Else
                Me.gvwSubjectSample.DataSource = Nothing
                Me.gvwSubjectSample.DataBind()

            End If

            LockUnlock()
        End If
    End Sub

    Protected Sub ddlActivity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlActivity.SelectedIndexChanged
        LockUnlock()
        FillgvwSubjectSample()
    End Sub

#End Region

#Region "TxtScan TextChange Event"

    Protected Sub txtScan_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtScan.TextChanged
        'Handles txtScan.TextChanged
        Dim ScanText As String = String.Empty
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds As New DataSet
        Dim ds_Save As New DataSet
        Dim dr As DataRow
        Dim Retu_nSampleBarCode As String = String.Empty
        Dim Retu_vSampleBarCode As String = String.Empty
        Dim dt_SubjectSample As New DataTable
        Dim dv_gvwSampleDetail As New DataView
        Dim ds_ViewMedExWorkSpaceDtl As New DataSet
        Dim ds_MedExInfoHdrDtl As New DataSet
        Dim ds_MedExInfoHdr As New DataSet
        Dim ds_MedExInfoDtl As New DataSet
        Dim Retu_TranNo As Integer = 0
        Dim iMySubjectNo As String = "0"
        Dim iNodeIndex As Integer = 0
        Dim DtSampleTypeDetail As New DataTable
        Dim DtSampleDetail As New DataTable
        Dim SubjectID As String = String.Empty

        Try


            If ddlType.SelectedValue = "2" Then
                ScanText = Me.ddlSubjectID.SelectedValue.ToString
            ElseIf ddlType.SelectedValue = "1" Then
                ScanText = Me.txtScan.Text.Trim
            End If
            '=========
            If ((ScanText.Contains("-")) Or (ScanText.Length > 9)) AndAlso Me.txtScan.Enabled = True Then

                Me.lblSubject.Text = ScanText.ToString()

                'If Not Me.ViewState(VS_DtSave) Is Nothing Then

                '    If ds_Save.Tables.Count < 1 Then
                '        ds_Save.Tables.Add(CType(Me.ViewState(VS_DtSave), DataTable).Copy())
                '        'Checking whether the subject belongs to same workspace or not
                '    End If


                '    'To Validate Subject
                '    If Not IsNothing(ds_Save.Tables(0).Columns.Contains("vSubjectID")) AndAlso _
                '        Not (ds_Save.Tables(0).Rows(0).Item("vSubjectID") Is System.DBNull.Value) AndAlso _
                '        ds_Save.Tables(0).Rows(0).Item("vSubjectID").ToString.Trim() <> "" Then

                '        SubjectID = ScanText.ToString()

                '        If SubjectID.ToUpper.Trim() <> Convert.ToString(ds_Save.Tables(0).Rows(0)("vSubjectID")).ToUpper.Trim() Then
                '            ObjCommon.ShowAlert("Subject Is Not Valid For This Sample. This Sample Is For " + _
                '                                ds_Save.Tables(0).Rows(0)("vSubjectID").ToString.Trim(), Me.Page)
                '            Me.resetpage()
                '            Exit Sub
                '        End If

                '    End If
                '***********************************************

                'If ds_Save.Tables(0).Rows(0).Item("vWorkSpaceId").ToString = Pro_Screening Then
                If Me.RBLProjecttype.SelectedValue <> Pro_Screening Then

                    wStr = " vSubjectID = '" + Me.lblSubject.Text.Trim + "'"
                    wStr += " And vLocationCode = '" + Me.Session(S_LocationCode) + "'"
                    wStr += " AND vWorkSpaceId = '" + Me.HProjectId.Value + "'"

                    If Not objHelp.GetViewWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr) Then
                        Me.ShowErrorMessage("Error While Getting Data From ViewWorkSpaceSubjectMst", eStr)
                        Me.resetpage()
                        Exit Sub
                    End If

                    'If Not objHelp.View_SampleTypeDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr) Then
                    '    Me.ShowErrorMessage("Error While Getting Data From SampleDetail", eStr)
                    '    Me.resetpage()
                    '    Exit Sub
                    'End If

                    If Not ds.Tables(0).Rows.Count < 1 Then
                        Me.lblMySubject.Text = ds.Tables(0).Rows(0)("vMySubjectNo").ToString()
                        Me.ViewState(VS_DtSave) = ds.Tables(0)
                        ds = Nothing
                    End If
                Else
                    wStr = " vSubjectID = '" + Me.lblSubject.Text.Trim + "'"
                    wStr += " And vLocationCode = '" + Me.Session(S_LocationCode) + "'"

                    If Not objHelp.GetView_SubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr) Then
                        Me.ShowErrorMessage("Error While Getting Data From View_SubjectMaster", eStr)
                        Me.resetpage()
                        Exit Sub
                    End If
                    If Not ds.Tables(0).Rows.Count < 1 Then
                        Me.lblMySubject.Text = ds.Tables(0).Rows(0)("vMySubjectNo").ToString()
                        Me.ViewState(VS_DtSave) = ds.Tables(0)
                        ds = Nothing
                    End If
                End If

                ds = Nothing

            Else
                '=======added on 26-dec-09
                'If Not Me.ViewState(VS_DtSave) Is Nothing Then

                'If ds_Save.Tables.Count < 1 Then
                '    ds_Save.Tables.Add(CType(Me.ViewState(VS_DtSave), DataTable).Copy())
                'End If


                If Me.RBLProjecttype.SelectedValue = "0000000000" Then
                    wStr = "vWorkspaceid = '0000000000' "
                ElseIf Me.RBLProjecttype.SelectedValue <> "0000000000" Then
                    wStr = "vWorkspaceid = '" + Me.HProjectId.Value + "' "
                    wStr += " And iNodeId = " + Me.ddlActivity.SelectedValue.ToString
                End If

                If ddlType.SelectedValue = "2" Then
                    wStr += " And vSampleBarCode = '" + Me.lblSample.Text.ToString + "'"
                ElseIf ddlType.SelectedValue = "1" Then

                    wStr += " And vSampleBarCode = '" + ScanText.Trim() + "'"
                End If

                wStr += " And vLocationCode = '" + Me.Session(S_LocationCode) + "'"
                wStr += " AND  vSampleBarCodeId NOT LIKE '%P%'  "

                '===========
                If Not objHelp.View_SampleTypeDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Save, eStr) Then
                    Me.ShowErrorMessage("Error While Getting Data From SampleDetail", eStr)
                    Me.resetpage()
                    Exit Sub
                End If
                If ds_Save.Tables(0).Rows.Count > 0 Then


                    If Not IsNothing(ds_Save.Tables(0).Columns.Contains("vSubjectId")) AndAlso _
                        Not (ds_Save.Tables(0).Rows(0).Item("vSubjectId") Is System.DBNull.Value) AndAlso _
                        ds_Save.Tables(0).Rows(0).Item("vSubjectId").ToString.Trim() <> "" Then

                        SubjectID = ScanText.ToString()
                        Dim dv As DataView
                        Dim dt As DataTable
                        dt = ds_Save.Tables(0)
                        dv = dt.DefaultView

                        If ddlType.SelectedValue = "1" Then
                            dv.RowFilter = "vSampleBarCode = '" + SubjectID.ToString() + "'"
                            ds_Save.Tables(0).DefaultView.RowFilter = "vSampleBarCode ='" + SubjectID.ToString() + "'"
                        ElseIf ddlType.SelectedValue = "2" Then
                            dv.RowFilter = "vSubjectID = '" + SubjectID.ToString() + "'"
                            ds_Save.Tables(0).DefaultView.RowFilter = "vSubjectID ='" + SubjectID.ToString() + "'"
                        End If



                        ds_Save = New DataSet
                        ds_Save.Tables.Add(dv.ToTable().DefaultView.Table())

                        If Not ds_Save Is Nothing AndAlso ds_Save.Tables(0).Rows.Count > 0 Then

                            If ddlType.SelectedValue = "2" Then
                                If SubjectID.ToUpper.Trim() <> Convert.ToString(ds_Save.Tables(0).Rows(0)("vSubjectID")).ToUpper.Trim() Then
                                    ObjCommon.ShowAlert("Subject Is Not Valid For This Sample. This Sample Is For " + _
                                                        ds_Save.Tables(0).Rows(0)("vSubjectID").ToString.Trim(), Me.Page)
                                    Me.resetpage()
                                    Exit Sub
                                End If
                            ElseIf ddlType.SelectedValue = "1" Then
                                If SubjectID.ToUpper.Trim() <> Convert.ToString(ds_Save.Tables(0).Rows(0)("vSampleBarCode")).ToUpper.Trim() Then
                                    ObjCommon.ShowAlert("Subject Is Not Valid For This Sample. This Sample Is For " + _
                                                        ds_Save.Tables(0).Rows(0)("vSubjectID").ToString.Trim(), Me.Page)
                                    Me.resetpage()
                                    Exit Sub
                                End If
                            End If


                        Else
                            ObjCommon.ShowAlert("Subject Is Not Valid For This Sample. This Sample Is For " + _
                                                SubjectID, Me.Page)
                            Me.resetpage()
                            Exit Sub
                        End If
                    End If
                End If
                If ds_Save.Tables(0).Rows.Count < 1 Then

                    If Me.RBLProjecttype.SelectedValue = "0000000000" Then
                        ObjCommon.ShowAlert("Sample Is Not Valid", Me.Page)
                    ElseIf Me.RBLProjecttype.SelectedValue <> "0000000000" Then
                        ObjCommon.ShowAlert("Sample Is Not Valid For Selected Project And Selected Activity", Me.Page)
                    End If
                    Me.resetpage()
                    Exit Sub
                End If

                Me.ViewState(VS_DtSave) = ds_Save.Tables(0)
                'ds_Save = Nothing
                '========added on 26-dec-09
                If ddlType.SelectedValue = "1" Then
                    Me.lblSample.Text = ScanText.Trim
                End If
                'Else
                ''ObjCommon.ShowAlert("Please Gunned Wrist Band First", Me.Page)
                'Me.resetpage()
                'Me.ViewState(VS_DtSave) = Nothing
                'ds = Nothing
                'Exit Sub

                'End If
            End If
            '===added on 26-dec-09
            If ddlType.SelectedValue = "1" Then
                Me.txtScan.Text = ""
                Me.txtScan.Focus()
            End If
            '=======

            If Me.lblSample.Text <> "" And Me.lblSubject.Text <> "" Then

                If ds_Save.Tables.Count < 1 Then
                    ds_Save.Tables.Add(CType(Me.ViewState(VS_DtSave), DataTable).Copy())
                    'Checking whether the subject belongs to same workspace or not
                End If

                If ds_Save.Tables(0).Rows(0).Item("vWorkSpaceId").ToString = Pro_Screening Then

                    wStr = " vSubjectID = '" + Me.lblSubject.Text.Trim + "' And cRejectionFlag <> 'Y' "

                    If Not objHelp.GetView_SubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr) Then
                        Me.ShowErrorMessage("Error While Getting Data From View_SubjectMaster", eStr)
                        Me.resetpage()
                        Exit Sub
                    End If

                Else
                    wStr = "vWorkSpaceId = '" + ds_Save.Tables(0).Rows(0).Item("vWorkSpaceId").ToString + "' And"
                    wStr += " vSubjectID = '" + Me.lblSubject.Text.Trim + "'"
                    wStr += " And  cStatusIndi <> 'D' "
                    'wStr += " And cRejectionFlag <> 'Y'"

                    If Not objHelp.GetViewWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr) Then
                        Me.ShowErrorMessage("Error While Getting Data From ViewWorkSpaceSubjectMst", eStr)
                        Me.resetpage()
                        Exit Sub
                    End If
                    ''*************************************************

                End If

                If Me.chkProjectScreening.Checked = False Then
                    If ds.Tables(0).Rows.Count < 1 Then
                        ObjCommon.ShowAlert("Subject Is Not Valid.Subject Is Rejected.", Me.Page)
                        Me.resetpage()
                        Exit Sub
                    End If

                    'To Validate Subject
                    If Not IsNothing(ds_Save.Tables(0).Columns.Contains("vSubjectID")) AndAlso _
                     Not (ds_Save.Tables(0).Rows(0).Item("vSubjectID") Is System.DBNull.Value) AndAlso _
                     ds_Save.Tables(0).Rows(0).Item("vSubjectID").ToString.Trim() <> "" Then

                        SubjectID = Convert.ToString(ds.Tables(0).Rows(0)("vSubjectID")).ToUpper.Trim

                        If SubjectID <> Convert.ToString(ds_Save.Tables(0).Rows(0)("vSubjectID")).ToUpper.Trim Then
                            ObjCommon.ShowAlert("Subject Is Not Valid For This Sample. This Sample Is For '" + _
                                                ds_Save.Tables(0).Rows(0)("vSubjectID").ToString.Trim() + "'", Me.Page)
                            Me.resetpage()
                            Exit Sub
                        End If

                    End If
                    '***********************************************

                    If ds.Tables(0).Columns.Contains("iMySubjectNo") Then
                        If ds.Tables(0).Rows(0)("vProjectTypeCode").ToString = strProjectType_Housing Then 'InCase of In-House Project Only
                            iMySubjectNo = Convert.ToString(ds.Tables(0).Select("", "iPeriod Desc")(0)("iMySubjectNo"))
                        Else
                            iMySubjectNo = Convert.ToString(ds.Tables(0).Rows(0)("iMySubjectNo"))
                        End If
                        'iMySubjectNo = Convert.ToString(ds.Tables(0).Select("", "iPeriod Desc")(0)("iMySubjectNo"))

                        If iMySubjectNo <> Convert.ToString(ds_Save.Tables(0).Rows(0)("iMySubjectNo")) Then
                            ObjCommon.ShowAlert("SubjectNo Is Not Valid For This Sample", Me.Page)
                            Me.resetpage()
                            Exit Sub
                        End If
                    End If
                End If

                For Each dr In ds_Save.Tables(0).Rows

                    dr("vSubjectID") = Me.lblSubject.Text.Trim
                    dr("vLocationCode") = Me.Session(S_LocationCode).ToString()
                    dr("dCollectionDateTime") = System.DateTime.Now.ToString()
                    'If Me.txtCollectionTime.Text <> "" Then
                    '    dr("dCollectionDateTime") = CType(Me.txtCollectionDate.Text.Trim() + " " + Me.txtCollectionTime.Text.Trim(), DateTime)
                    'End If
                    dr("cStatusIndi") = "E"
                    dr.AcceptChanges()

                Next


                'Changed on 18-Jul-2009
                If Not AssignValues(ds_Save, DtSampleTypeDetail, DtSampleDetail) Then
                    Me.ShowErrorMessage("Error While Assigning Data", eStr)
                    Me.resetpage()
                    Exit Sub
                End If

                'For SampleTypeDetail
                ds_Save = Nothing
                ds_Save = New DataSet

                ds_Save.Tables.Add(DtSampleTypeDetail.Copy())
                If Not objLambda.Save_SampleTypeDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                        ds_Save, Me.Session(S_UserID), eStr) Then
                    Me.ShowErrorMessage("Error While Saving Data In SampleTypeDetail", eStr)
                    Me.resetpage()
                    Exit Sub
                End If

                'For SampleDetail
                ds_Save = Nothing
                ds_Save = New DataSet

                ds_Save.Tables.Add(DtSampleDetail.Copy())
                If Not objLambda.Save_SampleDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_Save, Me.Session(S_UserID), Retu_nSampleBarCode, Retu_vSampleBarCode, eStr) Then
                    Me.ShowErrorMessage("Error While Saving Data In SampleDetail", eStr)
                    Me.resetpage()
                    Exit Sub
                End If

                '****************************************

                Me.ViewState(VS_DtSampleDetailGrid) = Nothing

                If Not FillgvwSubjectSample() Then
                    ObjCommon.ShowAlert("Error While Filling Grid", Me.Page)
                End If

                'Me.divSearch.Attributes.Add("style", "display:none")

                ObjCommon.ShowAlert("Sample Collected Successfully. Details Of Sample Collected Will Displayed If Collection Date Is  Today's Date", Me)

                Me.resetpage()
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....txtScan_TextChanged")
        End Try

    End Sub

#End Region

#Region "AssignValues"

    Private Function AssignValues(ByVal ds_Save As DataSet, ByRef DtSampleTypeDetail As DataTable, ByRef DtSampleDetail As DataTable) As Boolean
        Dim DsSampleDetail As New DataSet
        Dim Dr_New As DataRow
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty


        Dim ds_ViewMedExWorkSpaceDtl As New DataSet

        Dim ds_CRFHdr As New DataSet
        Dim ds_CRFDtl As New DataSet
        Dim ds_CRFSUBDTl As New DataSet
        Dim ds_CRFHdrDtlSubdtl As New DataSet
        Dim Retu_TranNo As Integer = 0
        Dim iMySubjectNo As String = "0"
        Dim iNodeIndex As Integer = 0
        Dim SubjectID As String = ""
        Dim ds As New DataSet
        Dim index As Integer = 0

        Try

            DtSampleTypeDetail = CType(Me.ViewState(VS_DtSampleTypeDetail), DataTable).Copy()
            DtSampleTypeDetail.Clear()

            Wstr = "nSampleId=" & ds_Save.Tables(0).Rows(0).Item("nSampleId").ToString.Trim()

            If Not Me.objHelp.GetSampleDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    DsSampleDetail, estr) Then
                Return False
            End If

            'For SampleTypeDetail 
            For Each drSampletype As DataRow In ds_Save.Tables(0).Rows
                Dr_New = DtSampleTypeDetail.NewRow

                Dr_New("nSampleTypeDetailNo") = drSampletype("nSampleTypeDetailNo")
                Dr_New("nSampleId") = drSampletype("nSampleId")
                Dr_New("cSampleTypeCode") = drSampletype("cSampleTypeCode")
                Dr_New("vSampleBarCode") = drSampletype("vSampleBarCode")
                Dr_New("dCollectionDateTime") = System.DateTime.Now.ToString()
                'Dr_New("dCollectionDateTime") = System.DBNull.Value
                'If Me.txtCollectionTime.Text <> "" Then
                '    Dr_New("dCollectionDateTime") = CType(Me.txtCollectionDate.Text.Trim() + " " + Me.txtCollectionTime.Text.Trim(), DateTime)
                'End If

                Dr_New("iCollectionBy") = Me.Session(S_UserID)
                Dr_New("iModifyBy") = Me.Session(S_UserID)
                Dr_New("cStatusIndi") = "E"

                DtSampleTypeDetail.Rows.Add(Dr_New)
            Next drSampletype

            'For SampleDetail 
            DtSampleDetail = DsSampleDetail.Tables(0).Copy()

            For Each drSample As DataRow In DtSampleDetail.Rows

                drSample("vSubjectID") = Me.lblSubject.Text.Trim
                drSample("vLocationCode") = Me.Session(S_LocationCode).ToString()
                drSample("dCollectionDateTime") = System.DateTime.Now.ToString()
                'drSample("dCollectionDateTime") = System.DBNull.Value
                'If Me.txtCollectionTime.Text <> "" Then
                '    drSample("dCollectionDateTime") = CType(Me.txtCollectionDate.Text.Trim() + " " + Me.txtCollectionTime.Text.Trim(), DateTime)
                'End If

                drSample("iCollectionBy") = Me.Session(S_UserID)
                drSample("iModifyBy") = Me.Session(S_UserID)
                drSample("cStatusIndi") = "E"
                drSample.AcceptChanges()
            Next
            DtSampleDetail.AcceptChanges()
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......AssignValues")
            Return False
        End Try
    End Function

#End Region

#Region "Grid Events"

    Protected Sub gvwSubjectSample_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwSubjectSample.RowCreated
        e.Row.Cells(GVC_SampleTypeNo).Visible = False
        e.Row.Cells(GVC_iMySubjectNo).Visible = False
        e.Row.Cells(GVC_ModifyOn).Visible = False
        e.Row.Cells(GVC_DeptCode).Visible = False

    End Sub

    Protected Sub gvwSubjectSample_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwSubjectSample.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            'If e.Row.Cells(GVC_SubjectNo).Text.Trim() = 0 Then
            '    e.Row.Cells(GVC_SubjectNo).Text = ""
            'End If

            If e.Row.Cells(GVC_Period).Text.Trim() = 0 Then
                e.Row.Cells(GVC_Period).Text = ""
            End If

            'If e.Row.Cells(GVC_Period).Text.Trim() = 0 Then
            '    e.Row.Cells(GVC_Period).Text = ""
            'End If

            e.Row.Cells(GVC_CollectionTime).Text = Replace(e.Row.Cells(GVC_CollectionTime).Text.Trim(), "&nbsp;", "")
            e.Row.Cells(GVC_ModifyOn).Text = Replace(e.Row.Cells(GVC_ModifyOn).Text.Trim(), "&nbsp;", "")

            If Not Convert.ToString(e.Row.Cells(GVC_CollectionTime).Text).Trim = "" Then

                e.Row.Cells(GVC_CollectionTime).Text = Convert.ToString(CDate(e.Row.Cells(GVC_CollectionTime).Text.Trim()).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                e.Row.Cells(GVC_ModifyOn).Text = Convert.ToString(CDate(e.Row.Cells(GVC_ModifyOn).Text.Trim()).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                'e.Row.Cells(GVC_CollectionTime).Text = CType(e.Row.Cells(GVC_CollectionTime).Text.Trim(), Date).GetDateTimeFormats()(23).Trim()
                'e.Row.Cells(GVC_ModifyOn).Text = CType(e.Row.Cells(GVC_ModifyOn).Text.Trim(), Date).GetDateTimeFormats()(23).Trim()

            ElseIf Convert.ToString(e.Row.Cells(GVC_CollectionTime).Text).Trim = "" Then
                e.Row.Cells(GVC_ModifyOn).Text = ""
            End If


            If Convert.ToString(e.Row.Cells(GVC_DeptCode).Text).Trim <> Str_CPMADept AndAlso Convert.ToString(e.Row.Cells(GVC_DeptCode).Text).Trim <> Str_BABEDept Then
                e.Row.BackColor = Drawing.Color.Red
            End If

            If Me.hndLockStatus.Value.Trim() = "Lock" Then
                Me.txtScan.Attributes.Add("Disabled", "Disabled")
                Me.BtncollectSample.Attributes.Add("Disabled", "Disabled")
            End If

            'e.Row.Cells(GVC_CollectionTime).Text = CType(e.Row.Cells(GVC_CollectionTime).Text, DateTime).ToString("dd-MMM-yyyy HH:mm:ss")
            'e.Row.Cells(GVC_ModifyOn).Text = CType(e.Row.Cells(GVC_ModifyOn).Text, DateTime).ToString("dd-MMM-yyyy HH:mm:ss")

        End If

    End Sub

#End Region

#Region "Button Events"

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    'Protected Sub chkwithoutscanner_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkwithoutscanner.CheckedChanged
    '    If chkwithoutscanner.Checked = True Then
    '        Me.trLbl.Attributes.Add("Style", "display:none")
    '        Me.trWoutScanner.Attributes.Add("Style", "''")
    '        Me.ddlSubjectID.Visible = True
    '        Me.pnlSubject.Visible = True
    '        'Me.ddlSamplId.Visible = True

    '        'If Me.RBLProjecttype.SelectedValue = "0000000000" Then
    '        '    Me.BtncollectSample.Visible = True
    '        '    Me.btnManualCollect.Visible = False
    '        'ElseIf Me.RBLProjecttype.SelectedValue <> "0000000000" Then
    '        '    Me.btnManualCollect.Visible = True
    '        '    Me.BtncollectSample.Visible = True
    '        'End If

    '        Me.txtScan.Enabled = False
    '        Me.lblSubject.Text = ""
    '        Me.lblSample.Text = ""

    '        FillddlSubjectID()
    '    ElseIf chkwithoutscanner.Checked = False Then
    '        Me.trLbl.Attributes.Add("Style", "''")
    '        Me.trWoutScanner.Attributes.Add("Style", "display:none")
    '        Me.ddlSubjectID.Visible = False
    '        'Me.ddlSamplId.Visible = False
    '        Me.BtncollectSample.Visible = True
    '        'Me.btnManualCollect.Visible = False

    '        Me.txtScan.Enabled = True
    '        Me.lblSample.Text = ""
    '        Me.lblSubject.Text = ""
    '    End If
    'End Sub

    Protected Sub BtncollectSample_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtncollectSample.Click


        For Each it As ListItem In ChklSampleId.Items
            If it.Selected = True Then
                Me.lblSubject.Text = Me.ddlSubjectID.SelectedValue.ToString
                Me.lblSample.Text = it.Text.ToString
                txtScan_TextChanged(txtScan, e)

            End If

        Next
        'Me.txtCollectionTime.Text = ""
        If ddlType.SelectedValue = "2" Then
            Me.ddlSubjectID.DataSource = Nothing
            Me.ddlSubjectID.Items.Clear()

            FillddlSubjectID()
            'Me.ddlSubjectID.SelectedIndex = 0
            Me.ChklSampleId.Items.Clear()
            Me.ChklSampleId.DataSource = Nothing
            Me.ChklSampleId.DataBind()


            Me.BtncollectSample.Enabled = False
            Me.pnlSubject.Attributes.Add("style", "display:none")
        End If

        'txtScan_TextChanged(txtScan, e)
    End Sub

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click

        Try

            If Not FillddlActivity() Then
                Exit Sub
            End If


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......btnSetProject_Click")
        End Try
    End Sub

    'Protected Sub btnManualCollect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnManualCollect.Click
    '    Me.txtCollectionDate.Text = ""
    '    Me.txtCollectionTime.Text = ""

    '    Me.divSearch.Attributes.Add("style", "display:block")
    '    txtCollectionDate.Text = Date.Today.ToString("dd-MMM-yyyy").Trim()

    'End Sub

    'Protected Sub btnDivOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDivOK.Click
    '    Dim S As New Object
    '    Dim ev As New System.EventArgs

    '    If (CType(Me.txtCollectionDate.Text.Trim() + " " + Me.txtCollectionTime.Text.Trim(), DateTime) > DateTime.Now) Then
    '        ObjCommon.ShowAlert("Collection Time should not be greater then current Date Time ", Me)
    '        Me.txtCollectionDate.Text = ""
    '        Me.txtCollectionTime.Text = ""
    '        Exit Sub
    '    End If

    '    BtncollectSample_Click(S, ev)


    'End Sub

    'Protected Sub btnDivCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDivCancel.Click

    '    Me.divSearch.Attributes.Add("style", "display:none")

    'End Sub

#End Region

#Region "ResetPage"

    Protected Sub resetpage()
        Me.lblSample.Text = ""
        Me.lblSubject.Text = ""
        Me.txtScan.Text = ""
        Me.txtScan.Focus()
        Me.lblMySubject.Text = ""
        Me.ViewState(VS_DtSave) = Nothing
        'Me.txtCollectionTime.Text = ""
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
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()

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

    Protected Sub ddlType_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim selectedvalue As String = RBLProjecttype.SelectedValue


        If ddlType.SelectedValue = "2" Then
            If (selectedvalue <> "0000000000") Then
                If HProjectId.Value = "" Then
                    ddlType.SelectedValue = "1"
                    ObjCommon.ShowAlert("Please Enter Project First!", Me)
                    Exit Sub
                End If
            End If
            

            Me.trLbl.Attributes.Add("Style", "display:none")
            Me.trWoutScanner.Attributes.Add("Style", "display:''")
            Me.trWoutScanner1.Attributes.Add("Style", "display:''")
            Me.ddlSubjectID.Visible = True
            Me.pnlSubject.Visible = True
            pnlSubject.Attributes.Add("style", "display:''")
            Me.txtScan.Enabled = False
            Me.lblSubject.Text = ""
            Me.lblSample.Text = ""
            rwBarcodeId.Visible = False

            FillddlSubjectID()
        ElseIf ddlType.SelectedValue = "1" Then
            Me.trLbl.Attributes.Add("Style", "''")
            Me.trWoutScanner.Attributes.Add("Style", "display:none")
            Me.trWoutScanner1.Attributes.Add("Style", "display:none")
            Me.ddlSubjectID.Visible = False
            'Me.ddlSamplId.Visible = False
            Me.BtncollectSample.Visible = True
            'Me.btnManualCollect.Visible = False
            rwBarcodeId.Visible = True
            Me.txtScan.Enabled = True
            Me.lblSample.Text = ""
            Me.lblSubject.Text = ""
        End If
    End Sub

End Class
