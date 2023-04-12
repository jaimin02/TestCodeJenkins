
Partial Class frmIPAdministration
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION"

    Private ObjCommon As New clsCommon
    Private ObjHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private ObjLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Private Const VS_dtViewDosingDetail As String = "View_DosingDetail"
    Private Const VS_DtSave As String = "Dt_Save"
    Private Const VS_DtViewWorkSpaceSubjectMst As String = "View_WorkSpaceSubjectMst"
    Private Const VS_DtView_MedExWorkSpaceDtl As String = "View_MedExWorkSpaceDtl"
    Private Const VS_DtView_WorkSpaceNodeDetail As String = "View_WorkSpaceNodeDetail"
    Private Const VS_DtCRFHdr As String = "DtCRFHdr"
    Private Const VS_DtCRFDtl As String = "DtCRFDtl"
    Private Const VS_DtCRFSubDtl As String = "DtCRFSubDtl"
    Private Const VS_CrfSubDtl As String = "Ds_CrfSubDtl"
    Private Const VS_TempCrfSubDtl As String = "ds_Temp"
    Private Const VS_TempDosedOn As String = "ds_Tempd"



    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_DosingBarCode As Integer = 1
    Private Const GVC_iMySubjectNo As Integer = 2
    Private Const GVC_MySubjectNo As Integer = 3
    Private Const GVC_SubjectID As Integer = 4
    Private Const GVC_FullName As Integer = 5
    Private Const GVC_DosedOn As Integer = 6
    Private Const GVC_DosedBy As Integer = 7
    Private Const GVC_DoserName As Integer = 8
    Private Const GVC_DosingSupervisor As Integer = 9
    Private Const GVC_SupervisorName As Integer = 10
    Private Const GVC_WaterAdministered As Integer = 11
    Private Const GVC_Remarks As Integer = 12
    Private Const GVC_DosingDetailNo As Integer = 13
    Private Const GVC_MouthCheckDone As Integer = 14
    Private Const GVC_Edit As Integer = 16

    Private Const GVCSub_Select As Integer = 0
    Private Const GVCSub_WorkspaceId As Integer = 1
    Private Const GVCSub_MySubjectNo As Integer = 2
    Private Const GVCSub_vMySubjectNo As Integer = 3
    Private Const GVCSub_SubjectID As Integer = 4
    Private Const GVCSub_FullName As Integer = 5
    Dim ScanText As String = String.Empty

    Private Const CPMA_DeptCode As String = "0002"
    Private Const BABE_DeptCode As String = "0004"

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Not IsPostBack Then
            fsBarcode.Visible = False
            If Not GenCall_ShowUI() Then
                Exit Sub
            End If
        End If

    End Sub

#End Region

#Region "GenCall_Data()"

    Private Function GenCall_Data() As Boolean

        Dim Ds As New DataSet
        Dim wStr As String = String.Empty
        Dim estr As String = String.Empty

        Try




            wStr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'" _
                    + " And vActivityId = '" + Act_IPAdmin + "'" _
                    + " And iNodeId = " + Me.ddlActivity.SelectedValue() _
                    + " And vMedexType <> 'Import' And cStatusIndi <> 'D' "

            If Not Me.ObjHelp.GetViewMedExWorkSpaceDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                Ds, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From View_MedExWorkSpaceDtl : " + estr, Me.Page)
                Exit Function
            End If

            Me.ViewState(VS_DtView_MedExWorkSpaceDtl) = Nothing
            If Not Ds Is Nothing Then
                If Ds.Tables(0).Rows.Count < 1 Then
                    Me.ObjCommon.ShowAlert("No Attribute Template Is Attached With IP Administration. ", Me.Page)
                    Exit Function
                End If
                Me.ViewState(VS_DtView_MedExWorkSpaceDtl) = Ds.Tables(0)
            End If

            wStr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'" _
                    + " And iPeriod = " + Me.ddlPeriod.SelectedValue.Trim() _
                    + " And vActivityId = '" + Act_IPAdmin + "'" _
                    + " And iNodeId = " + Me.ddlActivity.SelectedValue.ToString.Trim() + " And cStatusIndi <> 'D'"
            Ds = Nothing
            Ds = New DataSet

            If Not Me.ObjHelp.GetViewWorkSpaceNodeDetail(wStr, Ds, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From WorkSpaceNodeDetail : " + estr, Me.Page)
                Exit Function
            End If
            Me.ViewState(VS_DtView_WorkSpaceNodeDetail) = Nothing
            If Not Ds Is Nothing Then
                If Ds.Tables(0).Rows.Count < 1 Then
                    Me.ObjCommon.ShowAlert("IP Administration Activity Is Not Attached Properly In Project. ", Me.Page)
                    Exit Function
                End If
                Me.ViewState(VS_DtView_WorkSpaceNodeDetail) = Ds.Tables(0)
            End If


            Ds = Nothing
            Ds = New DataSet
            If Not Me.ObjHelp.GetCRFHdr("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, Ds, estr) Then
                Throw New Exception(estr)
            End If
            Me.ViewState(VS_DtCRFHdr) = Nothing
            If Not Ds Is Nothing Then
                Me.ViewState(VS_DtCRFHdr) = Ds.Tables(0)
            End If


            Ds = Nothing
            Ds = New DataSet
            If Not Me.ObjHelp.GetCRFDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, Ds, estr) Then
                Throw New Exception(estr)
            End If
            Me.ViewState(VS_DtCRFDtl) = Nothing
            If Not Ds Is Nothing Then
                Me.ViewState(VS_DtCRFDtl) = Ds.Tables(0)
            End If


            Ds = Nothing
            Ds = New DataSet
            If Not Me.ObjHelp.GetCRFSubDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, Ds, estr) Then
                Throw New Exception(estr)
            End If
            Me.ViewState(VS_DtCRFSubDtl) = Nothing
            If Not Ds Is Nothing Then
                Me.ViewState(VS_DtCRFSubDtl) = Ds.Tables(0)
            End If


            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_Data")
        End Try
    End Function

#End Region

#Region "GenCall_showUI()"

    Private Function GenCall_ShowUI() As Boolean
        Dim sender As New Object
        Dim e As New EventArgs
        Try

            Page.Title = " :: IMP Administration ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "IMP Administration"
            CType(Me.Master.FindControl("form1"), HtmlForm).DefaultButton = Me.btndefault.UniqueID

            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
                GenCall_ShowUI = True

            End If

            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)

            If Not FillddlDosingSupervisor() Then
                Exit Function
            End If

            If Not FillDoserName() Then
                Exit Function
            End If

            '' Me.ddlDosingSupervisor.SelectedValue = Me.Session(S_UserID)
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideIMPDetails", "HideIMPDetails(); ", True)
            End If
            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_ShowUI")
        End Try
    End Function

#End Region

#Region "Grid Events"

    Protected Sub gvwSubjectSample_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvwSubjectSample.RowCancelingEdit

    End Sub

    Protected Sub gvwSubjectSample_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwSubjectSample.RowCreated
        If e.Row.RowType = DataControlRowType.Header Or _
                    e.Row.RowType = DataControlRowType.DataRow Or _
                    e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GVC_iMySubjectNo).Visible = False
            e.Row.Cells(GVC_FullName).Visible = False
            e.Row.Cells(GVC_Remarks).Visible = False
            e.Row.Cells(GVC_DosedBy).Visible = False
            e.Row.Cells(GVC_DosingSupervisor).Visible = False
            e.Row.Cells(GVC_DosingDetailNo).Visible = False
            e.Row.Cells(GVC_MouthCheckDone).Visible = False
        End If
    End Sub

    Protected Sub gvwSubjectSample_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwSubjectSample.RowCommand

        Dim index As Integer = CType(e.CommandArgument, Integer)
        Dim dtDosingDetail As New DataTable
        Dim dsDosingDetail As New DataSet
        Dim dvDosingDetail As New DataView
        Dim eStr As String = String.Empty
        Dim Retu_vSampleBarCode As String = String.Empty
        Dim Retu_nSampleBarCode As String = String.Empty
        Dim strModifyOn As String = String.Empty


        If e.CommandName.ToUpper.Trim() = "REPLACE" Then

            'If Not Me.gvwSubjectSample.Rows(index).Cells(GVC_DosedOn).Text = "" Then
            '    Me.ObjCommon.ShowAlert("IP Label cannot be Replaced ,Already been Collected.", Me.Page)
            '    Exit Sub
            'End If

            'Me.divReplacement.Visible = True
            'Me.pnlReplace.Visible = True
            Me.txtreplaceCode.Text = ""
            MPEReplacement.Show()
            Me.lblReplaceCode.Text = Me.gvwSubjectSample.Rows(index).Cells(GVC_DosingBarCode).Text.Trim()

            Me.txtReplaceRemark.Text = ""
            Me.lbReplaceWith.Text = ""
            Me.txtreplaceCode.Focus()

            'Me.lblReplaceCode.Text = Me.gvwSubjectSample.Rows(index).Cells(GVC_vSampleId).Text.Trim()
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + _
            '  Me.divReplacement.ClientID.ToString.Trim() + "');", True)
            Me.btnSubjectMgmt.Enabled = False
            fsBarcode.Visible = True

        End If

        If e.CommandName.ToUpper = "EDIT" Then

            If gvwSubjectSample.Rows(index).Cells(GVC_DosedOn).Text.Trim <> "&nbsp;" Then
                CType(gvwSubjectSample.Rows(index).FindControl("ImgSave"), ImageButton).Visible = True
                CType(gvwSubjectSample.Rows(index).FindControl("ImgCancel"), ImageButton).Visible = True
                CType(gvwSubjectSample.Rows(index).FindControl("ImgEdit"), ImageButton).Visible = False
                CType(gvwSubjectSample.Rows(index).FindControl("txtRemark"), TextBox).Enabled = True
                fsBarcode.Visible = True
            End If

        ElseIf e.CommandName.ToUpper = "UPDATE" Then

            Me.ViewState(VS_TempDosedOn) = ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName))
            Me.ViewState(VS_TempCrfSubDtl) = ObjCommon.GetCurDatetimeWithOffSet("India Standard Time")

            If Me.ViewState(VS_TempCrfSubDtl).ToString().Contains("+") Then
                Me.ViewState(VS_TempCrfSubDtl) = CDate(Me.ViewState(VS_TempCrfSubDtl).ToString().Split("+")(0).ToString()).ToString("dd-MMM-yyyy HH:mm").Trim()
            ElseIf Me.ViewState(VS_TempCrfSubDtl).ToString().Contains("-") Then
                Me.ViewState(VS_TempCrfSubDtl) = CDate(Me.ViewState(VS_TempCrfSubDtl).ToString().Split("-")(0).ToString()).ToString("dd-MMM-yyyy HH:mm").Trim()
            End If

            dtDosingDetail = CType(Me.ViewState(VS_dtViewDosingDetail), DataTable).Copy()
            dvDosingDetail = dtDosingDetail.DefaultView
            dvDosingDetail.RowFilter = " nDosingDetailNo = '" + Me.gvwSubjectSample.Rows(index).Cells(GVC_DosingDetailNo).Text + "'"
            'dtDosingDetail.Dispose()
            dsDosingDetail.Tables.Add(dvDosingDetail.ToTable)
            dsDosingDetail.Tables(0).Rows(0)("vRemarks") = CType(Me.gvwSubjectSample.Rows(index).FindControl("txtRemark"), TextBox).Text.Trim()
            Me.HdFieldSubjectID.Value = Me.gvwSubjectSample.Rows(index).Cells(GVC_SubjectID).Text
            dsDosingDetail.Tables(0).Rows(0)("iModifyBy") = Me.Session(S_UserID)
            strModifyOn = Me.ViewState(VS_TempCrfSubDtl).ToString
            dsDosingDetail.Tables(0).Rows(0)("dModifyOn") = strModifyOn.ToString()
            dsDosingDetail.Tables(0).AcceptChanges()
            dsDosingDetail.Tables(0).TableName = "DosingDetail"

            If Not ObjLambda.Save_DosingDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, dsDosingDetail, Me.Session(S_UserID), Retu_nSampleBarCode, Retu_vSampleBarCode) Then
                Me.ShowErrorMessage("Error While Saving Data In DosingDetail", eStr)
                Me.resetpage()
                Exit Sub
            End If

            Dim Wstr As String = String.Empty
            Dim Ds_CRFHdr As New DataSet
            Dim Ds_CRFDtl As New DataSet
            Dim Ds_CRFSubDtl As New DataSet

            Wstr = "vWorkSpaceId='" & HProjectId.Value.Trim() & "' and  iNodeId = ' " & ddlActivity.SelectedValue & "'  and vActivityId='" & Act_IPAdmin & "' and iPeriod='" & ddlPeriod.SelectedValue.ToString() & "' and cStatusIndi<> 'D'"
            If Not ObjHelp.GetCRFHdr(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_CRFHdr, eStr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From CRF Hdr", Me.Page)
                Exit Sub
            End If

            Wstr = "nCRFHdrNo='" & Ds_CRFHdr.Tables(0).Rows(0).Item("nCRFHdrNo").ToString() & "' and iRepeatNo='1' and cStatusIndi<> 'D'"
            If Not ObjHelp.GetCRFDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_CRFDtl, eStr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From CRF Dtl", Me.Page)
                Exit Sub
            End If

            Dim Dv_FilterCRFDtl As New DataView
            Dv_FilterCRFDtl = Ds_CRFDtl.Tables(0).DefaultView()
            Dv_FilterCRFDtl.RowFilter = "vSubjectId='" & Me.gvwSubjectSample.Rows(index).Cells(GVC_SubjectID).Text & "'"
            Dv_FilterCRFDtl.ToTable().AcceptChanges()

            Wstr = "nCRFDtlNo='" & Dv_FilterCRFDtl.ToTable().Rows(0).Item("nCRFDtlNo".ToString()) & "' and vMedExDesc='REMARKS'"
            If Not ObjHelp.GetCRFSubDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_CRFSubDtl, eStr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From CRF Dtl", Me.Page)
                Exit Sub
            End If

            Dim Dv_CRFSubDtl As New DataView
            Dim MaxTranNo As Integer
            Dim Dt_MaxTranNoCRFSubDtl As New DataTable
            Dv_CRFSubDtl = Ds_CRFSubDtl.Tables(0).DefaultView()
            MaxTranNo = Dv_CRFSubDtl.ToTable().Compute("max(iTranNo)", "1=1")
            Dv_CRFSubDtl.RowFilter = "iTranNo='" & MaxTranNo & "'"
            Dv_CRFSubDtl.ToTable().Rows(0).Item("vMedExResult") = 1
            Dv_CRFSubDtl.ToTable().AcceptChanges()
            Dt_MaxTranNoCRFSubDtl = Dv_CRFSubDtl.ToTable()
            Dt_MaxTranNoCRFSubDtl.Rows(0).Item("vMedExResult") = CType(Me.gvwSubjectSample.Rows(index).FindControl("txtRemark"), TextBox).Text.Trim()

            Dim Ds_ChangesCRFSubDtl As New DataSet

            Ds_ChangesCRFSubDtl.Tables.Add(Dt_MaxTranNoCRFSubDtl)

            If Not ObjLambda.Save_CRFSubDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, Ds_ChangesCRFSubDtl, Me.Session(S_UserID), eStr) Then
                Me.ObjCommon.ShowAlert("Error While Sving Data To CRFSubDtl", Me.Page)
                Exit Sub
            End If

            CType(gvwSubjectSample.Rows(index).FindControl("ImgSave"), ImageButton).Visible = True
            CType(gvwSubjectSample.Rows(index).FindControl("ImgCancel"), ImageButton).Visible = True
            CType(gvwSubjectSample.Rows(index).FindControl("ImgEdit"), ImageButton).Visible = True
            CType(gvwSubjectSample.Rows(index).FindControl("txtRemark"), TextBox).Enabled = False

        ElseIf e.CommandName.ToUpper = "CANCEL" Then

            CType(gvwSubjectSample.Rows(index).FindControl("ImgSave"), ImageButton).Visible = False
            CType(gvwSubjectSample.Rows(index).FindControl("ImgCancel"), ImageButton).Visible = False
            CType(gvwSubjectSample.Rows(index).FindControl("ImgEdit"), ImageButton).Visible = True
            CType(gvwSubjectSample.Rows(index).FindControl("txtRemark"), TextBox).Enabled = False
            If Not Me.FillGrid() Then
                Exit Sub
            End If

        End If

    End Sub

    Protected Sub gvwSubjectSample_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwSubjectSample.RowDataBound
        Dim RowCount As String = String.Empty
        Dim LockStatus As String = String.Empty
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then

                RowCount = e.Row.RowIndex + 1

                CType(e.Row.FindControl("lnkReplace"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkReplace"), ImageButton).CommandName = "REPLACE"
                CType(e.Row.FindControl("lnkReplace"), ImageButton).Attributes.Add("ReplaceRow", "yes")

                CType(e.Row.FindControl("ImgEdit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgEdit"), ImageButton).CommandName = "EDIT"
                CType(e.Row.FindControl("ImgEdit"), ImageButton).Attributes.Add("EditRow", "" + RowCount + "")

                CType(e.Row.FindControl("ImgSave"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgSave"), ImageButton).CommandName = "UPDATE"
                CType(e.Row.FindControl("ImgSave"), ImageButton).Attributes.Add("UpdateRow", "yes")

                CType(e.Row.FindControl("ImgCancel"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgCancel"), ImageButton).CommandName = "CANCEL"
                CType(e.Row.FindControl("ImgCancel"), ImageButton).Attributes.Add("CancelRow", "yes")

                e.Row.Cells(GVC_DosedOn).Attributes.Add("DosingTime" + RowCount.ToString() + "", "no")
                e.Row.Cells(GVC_DoserName).Attributes.Add("DosingName" + RowCount.ToString() + "", "no")
                e.Row.Cells(GVC_DosingSupervisor).Attributes.Add("DosingSypervisor" + RowCount.ToString() + "", "no")

                e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + 1 + (Me.gvwSubjectSample.PageSize * Me.gvwSubjectSample.PageIndex)
                'e.Row.Cells(GVC_CollectionDateTime).Text = Replace(e.Row.Cells(GVC_CollectionDateTime).Text.Trim(), "&nbsp;", "")

                If e.Row.Cells(GVC_Remarks).Text.Trim() = "&nbsp;" Then
                    e.Row.Cells(GVC_Remarks).Text = ""
                End If

                ''Add by shivani pandya for lock project
                If Me.hndLockStatus.Value.Trim() = "Lock" Then
                    CType(e.Row.FindControl("lnkReplace"), ImageButton).Attributes.Add("Disabled", "Disabled")
                    CType(e.Row.FindControl("ImgEdit"), ImageButton).Attributes.Add("Disabled", "Disabled")
                    CType(e.Row.FindControl("ImgSave"), ImageButton).Attributes.Add("Disabled", "Disabled")
                    CType(e.Row.FindControl("ImgCancel"), ImageButton).Attributes.Add("Disabled", "Disabled")
                    Me.txtScan.Attributes.Add("Disabled", "Disabled")
                End If
                If Me.hndLockStatus.Value.Trim() = "UnLock" Then
                    CType(e.Row.FindControl("lnkReplace"), ImageButton).Attributes.Remove("Disabled")
                    CType(e.Row.FindControl("ImgEdit"), ImageButton).Attributes.Remove("Disabled")
                    CType(e.Row.FindControl("ImgSave"), ImageButton).Attributes.Remove("Disabled")
                    CType(e.Row.FindControl("ImgCancel"), ImageButton).Attributes.Remove("Disabled")
                    Me.txtScan.Attributes.Remove("Disabled")
                End If

                CType(e.Row.FindControl("txtRemark"), TextBox).Text = e.Row.Cells(GVC_Remarks).Text.Trim()

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub gvwSubjectSample_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvwSubjectSample.RowEditing
        e.Cancel = True
    End Sub

    Protected Sub gvwSubjectSample_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvwSubjectSample.RowUpdating

    End Sub

    Protected Sub gvwSubjectSample_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvwSubjectSample.RowDeleting

    End Sub

#End Region

#Region "Text change event"

    Protected Sub txtScan_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtScan.TextChanged
        Dim ScanText As String = String.Empty
        Dim DtDosingDetail As DataTable
        Dim DvDosingDetail As DataView
        Dim DvViewDosingDetail As DataView

        Try

            Me.hfTextChnaged.Value = "BARCODE"

            If Me.HdFieldValidationTrueOrFalse.Value = "false" Then

            Else

                If Me.txtScan.Text = "" Then

                    Me.ObjCommon.ShowAlert("Please Enter Subjct Id", Me.Page)
                    txtScan.Focus()
                    Exit Sub

                Else

                    ScanText = Me.txtScan.Text.Trim
                    If (ScanText.Contains("-")) Or (ScanText.Length < 8) Then

                        If lblSubject.Text = "" Then

                            lblSubject.Text = txtScan.Text.Trim()
                            Me.MPESubjectCompliant.Show()
                            Me.rbllstCompiance.Items(0).Selected = False
                            Me.rbllstCompiance.Items(1).Selected = False
                            DtDosingDetail = CType(Me.ViewState(VS_dtViewDosingDetail), DataTable)
                            DvDosingDetail = DtDosingDetail.DefaultView
                            DvDosingDetail.RowFilter = "vSubjectId = '" + txtScan.Text.Trim() + "'"
                            If DvDosingDetail.ToTable().Rows.Count > 0 Then
                                lblMySubject.Text = DvDosingDetail.ToTable().Rows(0)("vMySubjectNo").ToString()
                            End If
                            rbllstCompiance.ClearSelection()
                            Me.rbllstCompiance.Focus()
                            UpControls.Update()
                            Exit Sub

                        Else

                            If txtScan.Text.Trim() <> lblSubject.Text.Trim() Then
                                Me.ObjCommon.ShowAlert("Subject ID Does Not Match", Me.Page)
                                txtScan.Text = ""
                                txtScan.Focus()
                                Exit Sub
                            Else
                                If lblIPLabelID.Text = "" Then
                                    Me.ObjCommon.ShowAlert("You Have Assigned This SubjectId Just Now,So Now Enter IP LabelId", Me.Page)
                                    txtScan.Text = ""
                                    txtScan.Focus()
                                    Exit Sub
                                Else
                                    If lblSubject.Text.Trim() = txtScan.Text.Trim() Then
                                        If Not TempSubjectSampleGridBind() Then '' Added By dipen shah on 31-Dec-2014.
                                            Me.ShowErrorMessage("error While Temp..", "")
                                        End If
                                        MpeMouthCheckDone.Show()
                                        Me.BtnOkWhileMouthCheckDone.Focus()
                                    Else
                                        Me.ObjCommon.ShowAlert("Subject ID Does Not Match", Me.Page)
                                        txtScan.Text = ""
                                        txtScan.Focus()
                                        Exit Sub
                                    End If
                                End If
                            End If

                        End If

                        ''--------------------------------------- subject id portion completed ------------------''

                    Else

                        DtDosingDetail = CType(Me.ViewState(VS_dtViewDosingDetail), DataTable)
                        DvDosingDetail = DtDosingDetail.DefaultView
                        DvDosingDetail.RowFilter = "vDosingBarCode='" & txtScan.Text & "'"
                        If DvDosingDetail.ToTable().Rows.Count > 0 Then
                            ''Commented and added by Aaditya on 19-May-2015 for Add On Changes
                            ''If CDate(DvDosingDetail.ToTable().Rows(0).Item("dExpiryDate").ToString()) < CDate(ObjCommon.GetCurDatetime(Session(S_TimeZoneName))) Then
                            If CDate(DvDosingDetail.ToTable().Rows(0).Item("dExpiryDate").ToString()) < CDate(ObjCommon.GetCurDatetime(Session(S_TimeZoneName))).ToString("M/dd/yyyy") Then
                                Me.ObjCommon.ShowAlert("The Drug is expired, You can not dose the Subject. ", Me.Page)
                                Me.BtnCancelDosing_Click(sender, e)
                                Exit Sub
                            End If
                        End If

                        ' ------------------------------------ IP Label id portion started ------------------''

                        If lblSubject.Text = "" Then
                            Me.ObjCommon.ShowAlert("Please Scan Subject ID Card First, Then Scan IP Label Of The Subject", Me.Page)
                            txtScan.Text = ""
                            txtScan.Focus()
                            Exit Sub
                        Else
                            DvViewDosingDetail = CType(Me.ViewState(VS_dtViewDosingDetail), DataTable).DefaultView
                            DvViewDosingDetail.RowFilter = "vDosingBarCode = '" + txtScan.Text + "' And vSubjectId = '" + Me.lblSubject.Text.Trim() + "'"
                            If Not DvViewDosingDetail.ToTable().Rows.Count > 0 Then
                                ObjCommon.ShowAlert("Sample Is Not Valid For Selected Subject", Me.Page)
                                txtScan.Text = ""
                                txtScan.Focus()
                                Exit Sub
                            End If

                            lblIPLabelID.Text = txtScan.Text
                            txtScan.Text = ""
                            txtScan.Focus()
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Focus", "document.getElementById('" + txtScan.ClientID + "').focus();", True)
                            Exit Sub
                        End If

                    End If

                End If

            End If

            txtScan.Focus()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...txtScan_TextChanged")

        Finally
            fsBarcode.Visible = True

        End Try
    End Sub

    Protected Sub txtreplaceCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtreplaceCode.TextChanged
        Dim ScanText As String = String.Empty
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds As New DataSet
        Dim StrQry As String = String.Empty
        Dim dsReplaceData As New DataSet

        Try


            ScanText = Me.txtreplaceCode.Text.Trim

            If (ScanText.Contains("-")) Or (ScanText.Length < 8) Then

                ObjCommon.ShowAlert("Please Select Only IPLabel For Replacement", Me.Page)
                Me.resetpage()
                Exit Sub

            End If

            wStr = "vDosingBarCode = '" + ScanText.Trim() + "'"
            If Not ObjHelp.View_DosingDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr) Then
                Me.ShowErrorMessage("Error While Getting Data From SampleDetail", eStr)
                Me.resetpage()
                Exit Sub
            End If

            If ds.Tables(0).Rows.Count < 1 Then
                ObjCommon.ShowAlert("IPSample Is Not Valid", Me.Page)
                Me.resetpage()
                Exit Sub
            End If

            If ds.Tables(0).Rows(0)("cReplaceFlag").ToString.Trim() = "Y" Then
                ObjCommon.ShowAlert("Barcode Is Already Assigned To The Subject.", Me.Page)
                Me.resetpage()
                Exit Sub
            End If

            If (Not ds.Tables(0).Rows(0)("vSubjectID") Is DBNull.Value AndAlso _
                    ds.Tables(0).Rows(0)("vSubjectID").ToString.Trim() <> "") And _
                (Not ds.Tables(0).Rows(0)("dDosedOn") Is DBNull.Value AndAlso _
                    ds.Tables(0).Rows(0)("dDosedOn").ToString.Trim() <> "") Then

                ObjCommon.ShowAlert("IPSample Is Already Assigned To The Subject", Me.Page)
                Me.resetpage()
                Exit Sub

            End If

            ''Added by Aaditya on 19-May-2015 for checking condition of reference type of Add on changes
            StrQry = "vDosingBarCode = '" + Me.lblReplaceCode.Text.Trim() + "'"

            If Not ObjHelp.View_DosingDetail(StrQry, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsReplaceData, eStr) Then
                Me.ShowErrorMessage("Error While Getting Data From SampleDetail", eStr)
                Me.resetpage()
                Exit Sub
            End If

            If IsNothing(dsReplaceData.Tables(0)) Or dsReplaceData.Tables(0).Rows.Count < 1 Then
                ObjCommon.ShowAlert("Existing IPLabelId Is Not Correct.", Me.Page)
                Me.resetpage()
                Exit Sub
            End If

            If (Not dsReplaceData.Tables(0).Rows(0)("vSubjectID") Is DBNull.Value AndAlso _
                    dsReplaceData.Tables(0).Rows(0)("vSubjectID").ToString.Trim() <> "") And _
                (Not dsReplaceData.Tables(0).Rows(0)("dDosedOn") Is DBNull.Value AndAlso _
                    dsReplaceData.Tables(0).Rows(0)("dDosedOn").ToString.Trim() <> "") Then

                ObjCommon.ShowAlert("Dosing Already Done For Selected Subject. You Can't Replace BarCode.", Me.Page)
                Me.resetpage()
                Exit Sub
            End If

            If ds.Tables(0).Rows(0).Item("vProductType").ToString.Trim() <> dsReplaceData.Tables(0).Rows(0).Item("vProductType").ToString.Trim() Or _
                CInt(ds.Tables(0).Rows(0).Item("iPeriod").ToString.Trim()) <> CInt(dsReplaceData.Tables(0).Rows(0).Item("iPeriod").ToString.Trim()) Or _
                CInt(ds.Tables(0).Rows(0).Item("iDoseNo").ToString.Trim()) <> CInt(dsReplaceData.Tables(0).Rows(0).Item("iDoseNo").ToString.Trim()) Or _
                CInt(ds.Tables(0).Rows(0).Item("iDayNo").ToString.Trim()) <> CInt(dsReplaceData.Tables(0).Rows(0).Item("iDayNo").ToString.Trim()) Then
                ObjCommon.ShowAlert("Day and Dose combination, Period No.and Product type of IplabelID should match with that of blank barcode..", Me.Page)
                Me.resetpage()
                Exit Sub
            End If
            ''Ended by Aaditya

            Me.ViewState(VS_DtSave) = ds.Tables(0)
            ds = Nothing
            Me.lbReplaceWith.Text = ScanText.Trim

            Me.txtreplaceCode.Text = ""
            Me.txtReplaceRemark.Focus()

            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + _
            '                                    Me.divReplacement.ClientID.ToString.Trim() + "');", True)

            Me.btnSubjectMgmt.Enabled = False
            'to show div
            MPEReplacement.Show()

        Catch ex As Exception
            Me.ShowErrorMessage(".txtreplaceCode_TextChanged", ex.Message)

        Finally
            fsBarcode.Visible = True

        End Try



    End Sub


#End Region

#Region "Fill Function"

    Private Function CreateSubjectTable(ByRef Dt As DataTable) As Boolean

        Dt = Nothing
        Dt = New DataTable()

        Dt.Columns.Add(New DataColumn("iUserId", GetType(Integer)))
        Dt.Columns.Add(New DataColumn("vWorkspaceId", GetType(String)))
        Dt.Columns.Add(New DataColumn("vSubjectId", GetType(String)))
        Dt.Columns.Add(New DataColumn("iMySubjectNo", GetType(String)))
        Dt.AcceptChanges()
        Return True

    End Function

    Private Function FillddlPeriod() As Boolean
        Dim ds_WorkSpaceNodeDetail As New Data.DataSet
        Dim View_WorkSpaceNodeDetail As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' "
            ds_WorkSpaceNodeDetail = Nothing

            If Not ObjHelp.GetViewWorkSpaceNodeDetail(wstr, ds_WorkSpaceNodeDetail, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data from WorkSpaceModeDetail:" + estr, Me.Page)
                Return False
            End If

            View_WorkSpaceNodeDetail = ds_WorkSpaceNodeDetail.Tables(0).DefaultView
            View_WorkSpaceNodeDetail.Sort = "iPeriod"

            Me.ddlPeriod.DataSource = ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable(True, "iPeriod".ToString())
            Me.ddlPeriod.DataValueField = "iPeriod"
            Me.ddlPeriod.DataTextField = "iPeriod"
            Me.ddlPeriod.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ThemeSelection();", "ThemeSelection();", True)
            Me.ddlPeriod.Items.Insert(0, New ListItem("--Select Period--", 0))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillddlPeriod")
            Return False
        End Try
    End Function

    Private Function FillddlActivity() As Boolean
        Dim ds_Activity As New Data.DataSet
        Dim dt_Activity As New Data.DataTable
        Dim wStr As String = String.Empty
        Dim estr As String = String.Empty
        Try


            ds_Activity = Nothing

            'added on 25-10-10===
            wStr = " vWorkspaceID = '" + Me.HProjectId.Value.Trim() + _
                   "' AND iPeriod = '" + Me.ddlPeriod.SelectedItem.Value.Trim() + _
                   "' And vActivityId ='1100'" '' Added by dipen shah on 9-jan-2015 for only imp activities shows in dropdown. 

            If Not ObjHelp.GetViewWorkSpaceNodeDetail(wStr, ds_Activity, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From View_WorkspaceNodeDetail:" + estr, Me.Page)
                Return False
            End If

            dt_Activity = ds_Activity.Tables(0)
            '=====================

            Me.ddlActivity.DataSource = dt_Activity.DefaultView.ToTable(True, "iNodeId,vNodeDisplayName".Split(","))
            Me.ddlActivity.DataValueField = "iNodeId"
            Me.ddlActivity.DataTextField = "vNodeDisplayName"
            Me.ddlActivity.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ThemeSelection();", "ThemeSelection();", True)
            Me.ddlActivity.Items.Insert(0, New ListItem("--Select Activity--", 0))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......FillddlActivity")
            Return False
        End Try

    End Function

    Private Function FillddlDosingSupervisor() As Boolean
        Dim ds_User As New Data.DataSet
        Dim dt_User As New DataTable
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty

        Try

            'Commented and added by Parth Modi on dated 18-August-2014
            'Reason: Add Location filter on User list
            'wstr = "nScopeNo = " & Me.Session(S_ScopeNo) & " And cStatusIndi <> 'D' And vUserTypeCode = '" + Me.Session(S_UserType) + "'"
            wstr = "nScopeNo = " & Me.Session(S_ScopeNo) & " And cStatusIndi <> 'D' And vUserTypeCode = '" + Me.Session(S_UserType) + "' AND vLocationCode = '" + Me.Session(S_LocationCode).ToString + "'"
            'END
            wstr += " Order by vUserName"

            If Not ObjHelp.GetViewUserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        ds_User, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data from View_UserMst:" + estr, Me.Page)
                Return False
            End If

            For CntOfDs_User As Integer = 0 To ds_User.Tables(0).Rows.Count - 1
                ds_User.Tables(0).Rows(CntOfDs_User).Item("vUserName") = ds_User.Tables(0).Rows(CntOfDs_User).Item("vUserName").ToString() + "   " + "(" + ds_User.Tables(0).Rows(CntOfDs_User).Item("vUserTypeName").ToString() + ")"
            Next CntOfDs_User

            ds_User.Tables(0).AcceptChanges()
            dt_User = ds_User.Tables(0).DefaultView.ToTable(True, "iUserId,vUserName".Split(","))

            Me.ddlDosingSupervisor.DataSource = dt_User
            Me.ddlDosingSupervisor.DataValueField = "iUserId"
            Me.ddlDosingSupervisor.DataTextField = "vUserName"
            Me.ddlDosingSupervisor.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ThemeSelection();", "ThemeSelection();", True)
            Me.ddlDosingSupervisor.Items.Insert(0, New ListItem("--Select Supervisor--", 0))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillddlDosingSupervisor")
            Return False
        End Try

    End Function

    Private Function FillDosingDay() As Boolean
        Dim ds_DosingDay As New Data.DataSet
        Dim View_DosingDay As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = "1=1"
        Dim iDosingDayNo As Integer = 0
        Dim indexDosingDay As Integer = 0
        Try


            Me.ddlDosingDay.Items.Clear()

            wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' And iPeriod='" & ddlPeriod.SelectedItem.ToString() & "' and cStatusIndi <> 'D' AND inodeid = " & Me.ddlActivity.SelectedValue.ToString & " order by iDayNo desc"
            ds_DosingDay = Nothing
            'If Not ObjHelp.GetDosingDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DosingDay, estr) Then
            '    Me.ObjCommon.ShowAlert("Error While Getting Data From Dosing Day Detail:" + estr, Me.Page)
            '    Return False
            'End If
            'If Not ObjHelp.getWorkSpaceNodeDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DosingDay, estr) Then
            '    Me.ObjCommon.ShowAlert("Error While Getting Data of Day:" + estr, Me.Page)
            '    Return False
            'End If

            If Not ObjHelp.GetViewWorkSpaceNodeDetail(wstr, ds_DosingDay, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data of Day:" + estr, Me.Page)
                Return False
            End If

            'If ds_DosingDay.Tables(0).Rows.Count > 0 Then
            '    iDosingDayNo = CInt(ds_DosingDay.Tables(0).Rows(0)("iDayNo"))
            '    For indexDosingDay = 1 To iDosingDayNo Step 1
            ddlDosingDay.Items.Add(CInt(ds_DosingDay.Tables(0).Rows(0)("iDayNo")))
            '    Next
            'End If

            Me.ddlDosingDay.Items.Insert(0, New ListItem("--Select Day No --", 0))
            FillDosingDay = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillDosingDay")
            FillDosingDay = False
        End Try
    End Function

    Private Function FillDoseNo() As Boolean
        Dim ds_DoseNo As New Data.DataSet
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim iDoseNo As Integer = 0
        Dim indexDosingNo As Integer = 0

        Try


            Me.ddlDosingNo.Items.Clear()

            'wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' And iPeriod='" & ddlPeriod.SelectedItem.ToString() & "' order by iDoseNo desc"
            'ds_DoseNo = Nothing
            'If Not ObjHelp.GetDosingDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DoseNo, estr) Then
            '    Me.ObjCommon.ShowAlert("Error While Getting Data From Dosing  No Detail:" + estr, Me.Page)
            '    Return False
            'End If

            wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' And iPeriod='" & ddlPeriod.SelectedItem.ToString() & "' and cStatusIndi <> 'D' AND inodeid = " & Me.ddlActivity.SelectedValue.ToString & " order by iDoseNo desc"
            ds_DoseNo = Nothing
            'If Not ObjHelp.getWorkSpaceNodeDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DoseNo, estr) Then
            '    Me.ObjCommon.ShowAlert("Error While Getting Data of Dose:" + estr, Me.Page)
            '    Return False
            'End If


            If Not ObjHelp.GetViewWorkSpaceNodeDetail(wstr, ds_DoseNo, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data of Day:" + estr, Me.Page)
                Return False
            End If

            'If ds_DoseNo.Tables(0).Rows.Count > 0 Then
            '    iDoseNo = CInt(ds_DoseNo.Tables(0).Rows(0)("iDoseNo"))
            '    For indexDosingNo = 1 To iDoseNo Step 1
            ddlDosingNo.Items.Add(CInt(ds_DoseNo.Tables(0).Rows(0)("iDoseNo")))
            '    Next
            'End If

            Me.ddlDosingNo.Items.Insert(0, New ListItem("--Select Dose No --", 0))
            FillDoseNo = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillDoseNo")
            FillDoseNo = False
        End Try
    End Function

    Private Function FillGrid() As Boolean
        Dim ds_DosingDetail As New Data.DataSet
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim Subjects As String = String.Empty
        Dim dtUserSubject As New Data.DataTable
        Dim dvUserSubject As New Data.DataView
        Dim dDosedOnDatetime As DataRow = Nothing
        Try

            If IsNothing(Me.Session("UserSubjectDtl")) Then

                Me.ObjCommon.ShowAlert("Please, First Do Subject Management", Me.Page())
                Exit Function

            Else
                dvUserSubject = CType(Me.Session("UserSubjectDtl"), DataTable).Copy().DefaultView()

                dvUserSubject.RowFilter = "vWorkspaceId = '" + Me.HProjectId.Value + "' And iUserId=" + _
                                                     Me.Session(S_UserID)

                dtUserSubject = dvUserSubject.ToTable()

                If dtUserSubject.Rows.Count <= 0 Then

                    Me.ObjCommon.ShowAlert("Please, First Do Subject Management", Me.Page())
                    Exit Function

                End If

            End If

            Wstr = "vWorkspaceId = '" + Me.HProjectId.Value + "' And cStatusIndi <> 'D'"

            Wstr += " And iDoseNo = " + Me.ddlDosingNo.SelectedItem.Value.Trim() + _
                    " And iDayNo = " + Me.ddlDosingDay.SelectedItem.Value.Trim() + " And iPeriod = " + _
                    Me.ddlPeriod.SelectedItem.Value.Trim()

            dtUserSubject = CType(Me.Session("UserSubjectDtl"), DataTable)
            dvUserSubject = dtUserSubject.DefaultView
            dvUserSubject.RowFilter = "vWorkspaceId = '" + Me.HProjectId.Value + "'"
            dtUserSubject = dvUserSubject.ToTable()

            For Each dr As DataRow In dtUserSubject.Rows()
                Subjects += IIf(Subjects = "", "'" & dr("iMySubjectNo"), "','" & dr("iMySubjectNo"))
            Next

            Subjects += "'"

            Wstr += " And iMySubjectNo in (" & Subjects & ") order by iMySubjectNo"

            If Not ObjHelp.View_DosingDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_DosingDetail, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From View_SampleDetail : ", estr)
                Exit Function
            End If

            ds_DosingDetail.AcceptChanges()


            Me.ViewState(VS_dtViewDosingDetail) = ds_DosingDetail.Tables(0)
            ds_DosingDetail.Tables(0).Columns.Add("dDosedOnDatetime")
            If ds_DosingDetail.Tables(0).Rows.Count > 0 Then
                For Each dr As DataRow In ds_DosingDetail.Tables(0).Rows()
                    If Not dr("dDosedOn").ToString = "" Then
                        dr("dDosedOnDatetime") = CType(dr("dDosedOn"), DateTime).ToString("dd-MMM-yyyy HH:mm")
                        'dr("dDosedOnDatetime") = dr("dDosedOn").ToString + strServerOffset 'Added By Parth Pandya for Change in time format
                    Else
                        dr("dDosedOnDatetime") = ""
                    End If
                Next
                ds_DosingDetail.AcceptChanges()
                Me.txtScan.Enabled = True 'Added on 11-Sep-2009

                Me.gvwSubjectSample.DataSource = ds_DosingDetail.Tables(0)
                Me.gvwSubjectSample.DataBind()
                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fsBarcode", "fsBarcode(); ", True)
                fsBarcode.Visible = True
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ThemeSelection();", "ThemeSelection();", True)
                Return True

            End If

            Me.gvwSubjectSample.DataSource = Nothing
            Me.gvwSubjectSample.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ThemeSelection();", "ThemeSelection();", True)
            Me.ObjCommon.ShowAlert("Records Not Found.", Me.Page)

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillGrid")
            Return False
        End Try
    End Function

    Private Function FillSubjectGrid() As Boolean
        Dim ds_Subjects As New Data.DataSet
        Dim dtOldSubjects As New Data.DataTable
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty

        Try


            Wstr = "vWorkspaceId = '" + Me.HProjectId.Value + "' And cStatusIndi <> 'D' "
            Wstr += " And iPeriod = " + Me.ddlPeriod.SelectedItem.Value.Trim() + " And cRejectionFlag<>'Y' And iMySubjectNo >0 "
            Wstr += " order by vMySubjectNo"

            If Not ObjHelp.GetViewWorkspaceSubjectMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                       ds_Subjects, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From View_SampleDetail : ", estr)
                Exit Function
            End If

            If ds_Subjects.Tables(0).Rows.Count > 0 Then

                Me.ViewState(VS_DtViewWorkSpaceSubjectMst) = ds_Subjects.Tables(0)
                Me.gvwSubjects.DataSource = ds_Subjects.Tables(0)
                Me.gvwSubjects.DataBind()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ThemeSelection();", "ThemeSelection();", True)
                If Not IsNothing(Me.Session("UserSubjectDtl")) Then

                    dtOldSubjects = CType(Me.Session("UserSubjectDtl"), DataTable)

                    For index As Integer = 0 To Me.gvwSubjects.Rows.Count - 1

                        For Each DrSubjects As DataRow In dtOldSubjects.Rows

                            If DrSubjects("iUserId") = Me.Session(S_UserID) AndAlso _
                                DrSubjects("vWorkspaceId") = Me.gvwSubjects.Rows(index).Cells(GVCSub_WorkspaceId).Text AndAlso _
                                DrSubjects("vSubjectId") = Me.gvwSubjects.Rows(index).Cells(GVCSub_SubjectID).Text Then

                                CType(Me.gvwSubjects.Rows(index).FindControl("ChkMove"), CheckBox).Checked = True

                                ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "CheckSelected", "CheckSelected();", True)

                            End If

                        Next DrSubjects

                    Next index

                End If

                Return True
            End If

            Me.gvwSubjects.DataSource = Nothing
            Me.gvwSubjects.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ThemeSelection();", "ThemeSelection();", True)
            Me.ObjCommon.ShowAlert("Records Not Found.", Me.Page)

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillSubjectGrid")
            Return False
        End Try
    End Function

#End Region

#Region "AssignValues1"

    Private Function AssignValues1() As Boolean
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_ViewMedExWorkSpaceDtl As New DataSet
        Dim dr As DataRow
        Dim ds_CRFHdr As New DataSet
        Dim ds_CRFDtl As New DataSet
        Dim ds_CRFSUBDTl As New DataSet
        Dim ds_CRFHdrDtlSubdtl As New DataSet
        Dim drHdr As DataRow
        Dim drDtl As DataRow
        Dim drSubDtl As DataRow
        Dim Retu_TranNo As Integer = 0
        Dim iMySubjectNo As String = "0"
        Dim iMySubjectNoNew As String = "0"
        Dim iNodeIndex As Integer = 0
        Dim SubjectID As String = String.Empty
        Dim DtDosingDetail As New DataTable
        Dim ds As New DataSet
        Dim ds_Save As New DataSet
        Dim Retu_nSampleBarCode As String = String.Empty
        Dim pendingActivity As Array
        Dim str_PendingActivity As Array
        Dim strtime As String = String.Empty

        Dim dtTemp As DataTable
        Dim dvTemp As DataView
        Dim ds_ActivityDeviation As DataSet = Nothing
        Dim ds_ViewCRFDtl As New DataSet


        Try

            wStr = "vDosingBarCode = '" + Me.lblIPLabelID.Text.Trim() + "'"
            dtTemp = CType(Me.ViewState(VS_dtViewDosingDetail), DataTable)
            dvTemp = dtTemp.DefaultView
            dvTemp.RowFilter = wStr
            ds_Save.Tables.Add(dvTemp.ToTable.Copy())
            ds_Save.AcceptChanges()

            'wStr = "vWorkSpaceId = '" + ds_Save.Tables(0).Rows(0).Item("vWorkSpaceId").ToString + "' And"
            wStr = "vSubjectID = '" + Me.lblSubject.Text.Trim + "' And cRejectionFlag <> 'Y'"
            dtTemp = Nothing
            dvTemp = Nothing
            dtTemp = CType(Me.ViewState(VS_DtViewWorkSpaceSubjectMst), DataTable)
            dvTemp = dtTemp.DefaultView
            dvTemp.RowFilter = wStr
            ds.Tables.Add(dvTemp.ToTable.Copy())
            ds.AcceptChanges()
            If ds.Tables(0).Rows.Count < 1 Then
                ObjCommon.ShowAlert("Subject Is Not Valid For This Project", Me.Page)
                Me.resetpage()
                Exit Function
            End If

            'To Validate Subject
            If Not IsNothing(ds_Save.Tables(0).Columns.Contains("vSubjectID")) AndAlso _
             Not (ds_Save.Tables(0).Rows(0).Item("vSubjectID") Is System.DBNull.Value) AndAlso _
             ds_Save.Tables(0).Rows(0).Item("vSubjectID").ToString.Trim() <> "" Then

                SubjectID = Convert.ToString(ds.Tables(0).Rows(0)("vSubjectID"))

                If SubjectID <> Convert.ToString(ds_Save.Tables(0).Rows(0)("vSubjectID")) Then
                    ObjCommon.ShowAlert("Subject Is Not Valid For This Sample. This Sample Is For '" + _
                                        ds_Save.Tables(0).Rows(0)("vSubjectID").ToString.Trim() + "'", Me.Page)
                    Me.resetpage()
                    Exit Function
                End If

            End If
            '***********************************************


            '''''Commented by dharmesh H.Salla on 04-May-2011'''
            '' changed 'iMySubjectNo' to 'iMySubjectNoNew'

            If ds.Tables(0).Columns.Contains("iMySubjectNo") Then
                iMySubjectNo = Convert.ToString(ds.Tables(0).Rows(0)("iMySubjectNoNew"))
                iMySubjectNoNew = Convert.ToString(ds.Tables(0).Rows(0)("iMySubjectNo"))

                If iMySubjectNo <> Convert.ToString(ds_Save.Tables(0).Rows(0)("iMySubjectNo")) Then
                    ObjCommon.ShowAlert("SubjectNo Is Not Valid For This Sample", Me.Page)
                    Me.resetpage()
                    Exit Function
                End If

            End If


            'Added on 04-June-2011 to not to allow to save if its entry is in Dynamic page
            wStr = " vWorkspaceID = '" + Me.HProjectId.Value + "' and iNodeId = " + Me.ddlActivity.SelectedValue + " and vSubjectId = '" + Me.lblSubject.Text.Trim + "' and cStatusIndiDtl <>'D'"
            If Not ObjHelp.View_CRFDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ViewCRFDtl, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_ViewCRFDtl.Tables(0).Rows.Count > 0 Then
                ObjCommon.ShowAlert("Sample For Selected Subject Is Already Collected From Dynamic Page.", Me.Page)
                Me.resetpage()
                Exit Function
            End If
            '=================================================================================
            For Each dr In ds_Save.Tables(0).Rows

                dr("iDosedBy") = Me.Session(S_UserID)
                dr("iDosingSupervisor") = Me.ddlDosingSupervisor.SelectedItem.Value.Trim()
                dr("dDosedOn") = Me.ViewState(VS_TempDosedOn)
                dr("vWaterAdministered") = Me.txtWaterQuantity.Text.Trim()
                dr("cStatusIndi") = "E"
                dr("dModifyOn") = Me.ViewState(VS_TempCrfSubDtl)
                dr.AcceptChanges()

            Next

            'Added Code For Making Entry In "CRFHdr" ,"CrfDTl" and "CRFSubdtl" Table.

            ds_ViewMedExWorkSpaceDtl.Tables.Add(CType(Me.ViewState(VS_DtView_MedExWorkSpaceDtl), DataTable).Copy())
            ds_ViewMedExWorkSpaceDtl.AcceptChanges()

            ds.Tables.Clear()
            ds.AcceptChanges()
            ds.Tables.Add(CType(Me.ViewState(VS_DtView_WorkSpaceNodeDetail), DataTable).Copy())
            ds.AcceptChanges()
            If ds.Tables(0).Rows.Count > 0 Then
                iNodeIndex = ds.Tables(0).Rows(0)("iNodeIndex")
            End If
            '------------------------------------------------------

            ds_CRFHdr.Tables.Add(CType(Me.ViewState(VS_DtCRFHdr), DataTable).Copy())
            ds_CRFHdr.Tables(0).Rows.Clear()
            ds_CRFHdr.AcceptChanges()

            'for CRf HDR   
            drHdr = ds_CRFHdr.Tables(0).NewRow()
            drHdr("nCRFHdrNo") = 1
            drHdr("vWorkSpaceId") = ds_Save.Tables(0).Rows(0)("vWorkSpaceId").ToString()
            drHdr("dStartDate") = CType(ObjCommon.GetCurDatetime(Session(S_TimeZoneName)), DateTime)
            drHdr("iPeriod") = ds_Save.Tables(0).Rows(0)("iPeriod").ToString()
            drHdr("iNodeId") = Me.ddlActivity.SelectedValue.Trim()
            drHdr("iNodeIndex") = iNodeIndex
            drHdr("vActivityId") = "1100"
            drHdr("cLockStatus") = "U" 'cLockStatus
            drHdr("iModifyBy") = Me.Session(S_UserID).ToString()
            drHdr("cStatusIndi") = "N"
            drHdr("dModifyon") = Me.ViewState(VS_TempCrfSubDtl)

            ds_CRFHdr.Tables(0).Rows.Add(drHdr)
            ds_CRFHdr.Tables(0).AcceptChanges()

            'for CRFDtl
            ds_CRFDtl.Tables.Add(CType(Me.ViewState(VS_DtCRFDtl), DataTable).Copy())
            ds_CRFDtl.Tables(0).Rows.Clear()
            ds_CRFDtl.AcceptChanges()

            drDtl = ds_CRFDtl.Tables(0).NewRow()
            drDtl("dRepeatationDate") = CType(ObjCommon.GetCurDatetime(Session(S_TimeZoneName)), DateTime)
            drDtl("vSubjectId") = ds_Save.Tables(0).Rows(0)("vSubjectID").ToString()


            '' changed by dharmesh H.Salla on 06-May-2011''''
            'drDtl("iMySubjectNo") = iMySubjectNo
            drDtl("iMySubjectNo") = iMySubjectNoNew
            '' ******************************************************** '''''
            drDtl("cLockStatus") = "U"
            drDtl("iWorkFlowstageId") = Me.Session(S_WorkFlowStageId)
            drDtl("iModifyBy") = Me.Session(S_UserID).ToString()
            drDtl("cStatusIndi") = "N"
            drDtl("dModifyon") = Me.ViewState(VS_TempCrfSubDtl)

            'If ds_ViewMedExWorkSpaceDtl.Tables(0).Rows.Count > 8 Then
            '    drDtl("cDataStatus") = "B"
            'Else
            drDtl("cDataStatus") = CRF_Review
            'End If

            ds_CRFDtl.Tables(0).Rows.Add(drDtl)
            ds_CRFDtl.Tables(0).AcceptChanges()

            'for CRfSubDtl
            ds_CRFSUBDTl.Tables.Add(CType(Me.ViewState(VS_DtCRFSubDtl), DataTable).Copy())
            ds_CRFSUBDTl.Tables(0).Rows.Clear()
            ds_CRFSUBDTl.AcceptChanges()


            '---------For saving Remark subjectWise added by Megha Shah-----'
            If Me.HPendingNode.Value.Contains(Me.lblSubject.Text.Trim) Then
                str_PendingActivity = Regex.Split(Me.HPendingNode.Value, Me.lblSubject.Text.Trim() + "@@")
                pendingActivity = Regex.Split(str_PendingActivity(1).ToString(), "##")

                If Not ObjHelp.GetData("WorkSpaceActivitySequenceDeviation", "*", "", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_ActivityDeviation, eStr) Then
                    Throw New Exception("Error While Saving Data For ActivitySeuenceDeviation")
                End If
                Dim drActivity As DataRow
                drActivity = ds_ActivityDeviation.Tables(0).NewRow()
                drActivity("vWorkspaceId") = Me.HProjectId.Value.Trim()
                drActivity("iPeriod") = Me.ddlPeriod.SelectedValue()
                drActivity("vSubjectId") = Me.lblSubject.Text.Trim
                drActivity("iNodeId") = Me.ddlActivity.SelectedValue.Trim()
                drActivity("vPendingNodes") = pendingActivity(0).ToString()
                drActivity("vRemarks") = Me.hremark.Value
                drActivity("iUserId") = Me.Session(S_UserID).ToString()
                drActivity("dModifiedDate") = CType(ObjCommon.GetCurDatetime(Session(S_TimeZoneName)), DateTime)



                ds_ActivityDeviation.Tables(0).Rows.Add(drActivity)
                ds_ActivityDeviation.Tables(0).AcceptChanges()

                If Not ObjLambda.Insert_WorkspaceActivitySequenceDeviation(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_ActivityDeviation, Me.Session(S_UserID).ToString(), eStr) Then
                    Throw New Exception("Error While Saving Data For ActivitySeuenceDeviation")
                End If
            End If
            '-------------------------'

            For Each dr In ds_ViewMedExWorkSpaceDtl.Tables(0).Rows

                drSubDtl = ds_CRFSUBDTl.Tables(0).NewRow()
                drSubDtl("vMedexCode") = dr("vMedExCode").ToString()
                drSubDtl("iModifyBy") = Me.Session(S_UserID).ToString()
                drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim().ToUpper()
                drSubDtl("cStatusIndi") = "N"
                drSubDtl("dModifyOn") = Me.ViewState(VS_TempCrfSubDtl)

                If dr("vMedExCode").ToString().Trim().ToUpper() = "01281" Then '"LABEL:" 

                    drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim().ToUpper()
                    drSubDtl("vMedExResult") = Me.lblIPLabelID.Text.Trim()

                ElseIf dr("vMedExCode").ToString().Trim().ToUpper() = "10525" Then '"IS SUBJECT COMPLIANT TO ALL PREDOSE REQUIREMENTS AS PER PROTOCOL?"

                    drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim().ToUpper()
                    drSubDtl("vMedExResult") = rbllstCompiance.SelectedItem.ToString()

                ElseIf dr("vMedExCode").ToString().Trim().ToUpper() = "00649" Then 'Date
                    drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim().ToUpper()

                    If Me.ViewState(VS_TempDosedOn).ToString().Contains("+") Then
                        drSubDtl("vMedexResult") = CDate(Me.ViewState(VS_TempDosedOn).ToString()).ToString("dd-MMM-yyyy").Trim()
                    ElseIf Me.ViewState(VS_TempDosedOn).ToString().Contains("-") Then
                        drSubDtl("vMedexResult") = CDate(Me.ViewState(VS_TempDosedOn).ToString()).ToString("dd-MMM-yyyy").Trim()
                    End If

                ElseIf dr("vMedExCode").ToString().Trim().ToUpper() = "00650" Then 'TIME

                    drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim().ToUpper()

                    If Me.ViewState(VS_TempDosedOn).ToString().Contains("+") Then
                        strtime = CDate(Me.ViewState(VS_TempDosedOn).ToString().Split("+")(0).ToString()).ToString("HH:mm")
                    ElseIf Me.ViewState(VS_TempDosedOn).ToString().Contains("-") Then
                        strtime = CDate(Me.ViewState(VS_TempDosedOn).ToString().Split("-")(0).ToString()).ToString("HH:mm")
                    End If
                    drSubDtl("vMedExResult") = strtime

                ElseIf dr("vMedExCode").ToString().Trim().ToUpper() = "00653" Then '"DOSING SUPERVISION DONE BY" 

                    drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim().ToUpper()
                    drSubDtl("vMedExResult") = Me.ddlDosingSupervisor.SelectedItem.Text.Trim()

                ElseIf dr("vMedExCode").ToString().Trim().ToUpper() = "00651" Then '"MOUTH CHECK DONE" 

                    drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim().ToUpper()
                    drSubDtl("vMedExResult") = Me.RblMouthCheckDone.SelectedItem.Text.Trim()

                ElseIf dr("vMedExCode").ToString().Trim().ToUpper() = "00654" Then '"ML OF WATER ADMINISTERED WITH IP" 

                    drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim().ToUpper()
                    drSubDtl("vMedExResult") = Me.txtWaterQuantity.Text.Trim()

                ElseIf dr("vMedExCode").ToString().Trim().ToUpper() = "00655" Then 'Remarks

                    drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim().ToUpper()
                    drSubDtl("vMedExResult") = Me.HdFieldRemarks.Value.Trim()
                    ''Me.HdFieldRemarks.Value = ""

                    'ElseIf dr("vMedExCode").ToString().Trim().ToUpper() = "18596" Then ' Type/Code added by dipen shah  ' 18596 is used for BiznetValid

                ElseIf dr("vMedExCode").ToString().Trim().ToUpper() = "19327" Then ' Type/Code added by Dipen shah for (10.0.0.70)Biznet
                    wStr = "vDosingBarCode = '" + lblIPLabelID.Text.Trim() + "'"
                    If Not ObjHelp.View_DosingDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr) Then
                        Me.ShowErrorMessage("Error While Getting Data From SampleDetail", eStr)
                        Exit Function
                    End If

                    If (ds.Tables(0).Rows(0)("vRandomizationcode").ToString() <> "") Then
                        drSubDtl("vmedexdesc") = dr("vmedexdesc").ToString().Trim().ToUpper()
                        drSubDtl("vmedexresult") = ds.Tables(0).Rows(0)("vrandomizationcode")
                    Else '' added by dipen shah . 
                        drSubDtl("vMedExDesc") = dr("vMedExDesc").ToString().Trim().ToUpper()
                        drSubDtl("vMedExResult") = ds.Tables(0).Rows(0)("vProductType")
                    End If

                ElseIf dr("vMedExCode").ToString().Trim().ToUpper() = ConfigurationManager.AppSettings.Item("DoserName").Trim() Then         'Code Added by Rahul Rupareliya For Doser Name 
                    drSubDtl("vMedExResult") = ddlDoserName.SelectedItem.Text
                End If

                ds_CRFSUBDTl.Tables(0).Rows.Add(drSubDtl)
                ds_CRFSUBDTl.Tables(0).AcceptChanges()

            Next dr

            ds_CRFHdrDtlSubdtl.Tables.Add(ds_CRFHdr.Tables(0).Copy())
            ds_CRFHdrDtlSubdtl.Tables(0).TableName = "CRFHdr"

            ds_CRFHdrDtlSubdtl.Tables.Add(ds_CRFDtl.Tables(0).Copy())
            ds_CRFHdrDtlSubdtl.Tables(1).TableName = "CRFDtl"

            ds_CRFHdrDtlSubdtl.Tables.Add(ds_CRFSUBDTl.Tables(0).Copy())

            ds_CRFHdrDtlSubdtl.Tables(2).TableName = "CRFSubDtl"
            ds_CRFHdrDtlSubdtl.AcceptChanges()

            If Not AssignValues2(ds_Save, DtDosingDetail) Then
                Me.ShowErrorMessage("Error While Assigning Data", eStr)
                Me.resetpage()
                Exit Function
            End If

            ds_CRFHdrDtlSubdtl.Tables.Add(DtDosingDetail.Copy())
            ds_CRFHdrDtlSubdtl.Tables(3).TableName = "DosingDetail"
            ds_CRFHdrDtlSubdtl.AcceptChanges()
            If Not ObjLambda.Save_DosingDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_CRFHdrDtlSubdtl, Me.Session(S_UserID), Retu_nSampleBarCode, eStr) Then
                If Retu_nSampleBarCode = "0" Then
                    ObjCommon.ShowAlert(eStr, Me.Page)
                Else
                    Me.ShowErrorMessage("Error While Saving Data In DosingDetail. ", eStr)
                    Me.resetpage()
                    Exit Function
                End If
            End If

            ds_CRFHdr.Dispose()
            ds_CRFDtl.Dispose()
            ds_CRFSUBDTl.Dispose()
            ds_CRFHdrDtlSubdtl.Dispose()
            ds.Dispose()

            Me.ViewState(VS_dtViewDosingDetail) = Nothing
            Me.HdFieldRemarks.Value = ""
            If Not FillGrid() Then
                ObjCommon.ShowAlert("Error While Filling Grid", Me.Page)
            End If
            Me.ViewState(VS_TempCrfSubDtl) = Nothing
            Me.resetpage()
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..AssignValues1")
            Return False

        Finally
            fsBarcode.Visible = True
        End Try

    End Function

#End Region

#Region "AssignValues2"

    Private Function AssignValues2(ByVal ds_Save As DataSet, ByRef Dt_DosingDetail As DataTable) As Boolean
        Dim ds_DosingDetail As New DataSet
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty
        Try


            Wstr = "nDosingDetailNo=" & ds_Save.Tables(0).Rows(0).Item("nDosingDetailNo").ToString.Trim()

            If Not Me.ObjHelp.GetDosingDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    ds_DosingDetail, estr) Then
                Return False
            End If

            Dt_DosingDetail = ds_DosingDetail.Tables(0).Copy()

            For Each dr As DataRow In Dt_DosingDetail.Rows
                ' dr("iDosedBy") = Me.Session(S_UserID)
                dr("iDosedBy") = IIf(ddlDoserName.SelectedIndex = 0, Me.Session(S_UserID), ddlDoserName.SelectedValue.Trim())
                dr("iDosingSupervisor") = Me.ddlDosingSupervisor.SelectedItem.Value.Trim()
                dr("dDosedOn") = Me.ViewState(VS_TempDosedOn)
                dr("vWaterAdministered") = Me.txtWaterQuantity.Text.Trim()
                dr("cStatusIndi") = "E"
                dr("vRemarks") = Me.HdFieldRemarks.Value.ToString()
                dr("dModifyOn") = Me.ViewState(VS_TempCrfSubDtl)
                dr.AcceptChanges()

            Next dr
            Dt_DosingDetail.AcceptChanges()

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "Button Events"

    Protected Sub btnCloseMouthCheckDone_Click(sender As Object, e As EventArgs) Handles btnCloseMouthCheckDone.Click
        Try
            If Not GenCall_Data() Then
                Throw New Exception("GenCall_Data()")
            End If
            If Not FillGrid() Then
                Throw New Exception("Error While Fill Grid()")
            End If
            Me.lblIPLabelID.Text = ""
            Me.lblSubject.Text = ""
            Me.lblMySubject.Text = ""
            Me.txtScan.Text = ""
        Catch ex As Exception
            Throw New Exception("Error While btnCloseMouthCheckDone_Click()")
        End Try
    End Sub
    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim ds_CrfVersionDetail As DataSet = Nothing
        Dim VersionNo As Integer = 0
        Dim VersionDate As Date
        Dim wstr As String = String.Empty
        Dim eStr_Retu As String = String.Empty


        'add by ronak s. khilosiya
        Me.ddlPeriod.Items.Clear()
        Me.ddlActivity.Items.Clear()
        ''Me.ddlDosingSupervisor.SelectedValue = Me.Session(S_UserID)
        txtWaterQuantity.Text = ""
        Me.ddlDosingDay.Items.Clear()
        Me.ddlDosingNo.Items.Clear()
        Me.ddlActivity.Items.Clear()
        Me.gvwSubjectSample.DataSource = Nothing
        Me.gvwSubjectSample.DataBind()
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ThemeSelection();", "ThemeSelection();", True)
        UpGridSubjectSample.Update()
        Me.Session.Add("UserSubjectDtl", Nothing)
        fsBarcode.Visible = False
        'end add

        Try

            ''====== CRFVersion Control==================================

            wstr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'"

            If Not ObjHelp.GetData("View_CRFVersionForDataEntryControl", "*", wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CrfVersionDetail, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            If ds_CrfVersionDetail.Tables(0).Rows.Count > 0 Then
                VersionNo = ds_CrfVersionDetail.Tables(0).Rows(0)("nVersionNo")
                VersionDate = ds_CrfVersionDetail.Tables(0).Rows(0)("dVersiondate").ToString
                Me.VersionNo.Text = VersionNo.ToString
                Me.VersionDate.Text = VersionDate.ToString("dd-MMM-yyyy")
                Me.VersionDtl.Attributes.Add("style", " ")
                If ds_CrfVersionDetail.Tables(0).Rows(0)("cFreezeStatus").ToString.Trim.ToUpper = "U" Then
                    ImageLockUnlock.Attributes.Add("src", "images/UnFreeze.jpg")
                    ObjCommon.ShowAlert("Project Is In UnFreeze State, First Freeze It Then Proceed", Me.Page)
                    Exit Sub
                ElseIf ds_CrfVersionDetail.Tables(0).Rows(0)("cFreezeStatus").ToString.Trim.ToUpper = "F" Then
                    ImageLockUnlock.Attributes.Add("src", "images/Freeze.jpg")
                End If
            Else
                Me.VersionDtl.Attributes.Add("style", "display:none;")
            End If
            ''==========================================================

            If Not FillddlPeriod() Then
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click

    End Sub

    Protected Sub btnSaveSubject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveSubject.Click
        Try
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                Me.btnOk_Click(sender, e)
                Exit Sub
            End If
            If Not CheckSequence(Me.HProjectId.Value.Trim(), Me.ddlActivity.SelectedValue, Me.HsubjectId.Value, Me.ddlPeriod.SelectedValue) Then
                Me.MPEActivitySequence.Hide()
                Exit Sub
            End If
            If Me.HPendingNode.Value = "" Then
                Me.btnOk_Click(sender, e)
                Exit Sub
            End If
            Me.txtContent.Text = ""
            Me.MPEActivitySequence.Show()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......btnSaveSubject_Click")
        End Try
    End Sub

    Protected Sub btnSubjectMgmt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubjectMgmt.Click

        If Me.hfTextChnaged.Value.ToUpper.Trim() = "BARCODE" Then
            Me.hfTextChnaged.Value = ""
            Exit Sub
        End If

        If Not FillSubjectGrid() Then
            Exit Sub
        End If
        Me.MPESubjectManagement.Show()
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        If Me.hfTextChnaged.Value.ToUpper.Trim() = "BARCODE" Then
            Me.hfTextChnaged.Value = ""
            Exit Sub
        End If

        If Not GenCall_Data() Then
            Exit Sub
        End If

        If Not FillSubjectGrid() Then    '' Sequence Change For ViewState Data Nothig   by  Aaditya Chaubey
            Exit Sub
        End If


        If Not FillGrid() Then
            Exit Sub
        End If



        Me.txtScan.Focus()
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnReplaceOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReplaceOK.Click
        Dim ds_DosingDetail As New DataSet
        Dim dv_DosingDetail As New DataView
        Dim dt_DosingDetail As New DataTable
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim tempSubjectNo As String = String.Empty
        Dim Retu_nDosingBarCode As String = String.Empty
        'Me.divReplacement.Visible = False
        'Me.pnlReplace.Visible = False
        Me.MPEReplacement.Hide()

        Me.ViewState(VS_TempDosedOn) = ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName))
        Me.ViewState(VS_TempCrfSubDtl) = ObjCommon.GetCurDatetimeWithOffSet("India Standard Time")

        If Me.ViewState(VS_TempCrfSubDtl).ToString().Contains("+") Then
            Me.ViewState(VS_TempCrfSubDtl) = CDate(Me.ViewState(VS_TempCrfSubDtl).ToString().Split("+")(0).ToString()).ToString("dd-MMM-yyyy HH:mm").Trim()
        ElseIf Me.ViewState(VS_TempCrfSubDtl).ToString().Contains("-") Then
            Me.ViewState(VS_TempCrfSubDtl) = CDate(Me.ViewState(VS_TempCrfSubDtl).ToString().Split("-")(0).ToString()).ToString("dd-MMM-yyyy HH:mm").Trim()
        End If

        'Write Code for Replacement 01-Oct-2009
        wstr = "vDosingBarcode in ('" & Me.lblReplaceCode.Text.Trim() & "','" & Me.lbReplaceWith.Text.Trim() & "')"

        If Not Me.ObjHelp.GetDosingDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_DosingDetail, estr) Then

            Me.ObjCommon.ShowAlert("Error While Getting Data", Me.Page())
            Exit Sub

        End If

        If ds_DosingDetail.Tables(0).Rows.Count > 1 Then
            dt_DosingDetail = ds_DosingDetail.Tables(0).Copy()
            dv_DosingDetail = dt_DosingDetail.DefaultView
            dv_DosingDetail.RowFilter = "vDosingBarcode = '" & Me.lbReplaceWith.Text.Trim & "'"
            If dv_DosingDetail.ToTable.Rows.Count > 0 AndAlso Not dv_DosingDetail.ToTable.Rows(0)("iMySubjectNo") Is System.DBNull.Value _
            AndAlso dv_DosingDetail.ToTable.Rows(0)("iMySubjectNo") <> 0 Then
                ObjCommon.ShowAlert("Only Blank Labels Can Be Used For Replace", Me)
                Exit Sub
            End If

            tempSubjectNo = ds_DosingDetail.Tables(0).Rows(0)("iMySubjectNo").ToString.Trim()
            ds_DosingDetail.Tables(0).Rows(0)("iMySubjectNo") = ds_DosingDetail.Tables(0).Rows(1)("iMySubjectNo")
            ds_DosingDetail.Tables(0).Rows(1)("iMySubjectNo") = tempSubjectNo

            ds_DosingDetail.Tables(0).Rows(0)("vRemarks") = Me.txtReplaceRemark.Text.Trim()
            ds_DosingDetail.Tables(0).Rows(1)("vRemarks") = Me.txtReplaceRemark.Text.Trim()
            ds_DosingDetail.Tables(0).Rows(0)("cReplaceFlag") = "Y"
            ds_DosingDetail.Tables(0).Rows(0)("dModifyOn") = Me.ViewState(VS_TempCrfSubDtl)

            ds_DosingDetail.Tables(0).AcceptChanges()

            If Not ObjLambda.Save_DosingDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_DosingDetail, _
                                        Me.Session(S_UserID), Retu_nDosingBarCode, estr) Then

                Me.ShowErrorMessage("Error While Saving Data In SampleDetail", estr)
                Me.resetpage()
                Exit Sub

            End If

        End If

        'kept to fill grid at replace click
        If Not FillGrid() Then
            Exit Sub
        End If

        btnSearch_Click(sender, e)
        Me.btnSubjectMgmt.Enabled = True

    End Sub

    Protected Sub btnReplaceCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReplaceCancel.Click
        'Me.divReplacement.Visible = False
        'Me.pnlReplace.Visible = False
        Me.MPEReplacement.Hide()
        Me.btnSubjectMgmt.Enabled = True
        lbReplaceWith.Text = ""
        Me.txtReplaceRemark.Text = ""
    End Sub

    'Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    '    Me.divDeviation.Visible = False
    '    Me.PnlDeviation.Visible = False
    'End Sub

    'Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
    '    Try

    '        If Not AssignValues1() Then
    '            resetpage()
    '            Exit Sub
    '        End If

    'Me.divDeviation.Visible = False
    'Me.PnlDeviation.Visible = False

    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.Message, "")
    '    End Try
    'End Sub

    Protected Sub BtnSaveWhenYesInCompliance_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSaveWhenYesInCompliance.Click
        SaveIPTime()
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "RefreshScan", "RefreshScan(); ", True)
    End Sub

    Protected Sub BtnSaveAfterGridFromJavaScript_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSaveAfterGridFromJavaScript.Click
        Dim index As Integer = HdFieldFoundRow.Value
        Dim Subjectid As String = HdFieldSubjectID.Value
        Dim dtDosingDetail As New DataTable
        Dim dsDosingDetail As New DataSet
        Dim dvDosingDetail As New DataView
        Dim eStr As String = String.Empty
        Dim Retu_vSampleBarCode As String = String.Empty
        Dim Retu_nSampleBarCode As String = String.Empty

        Me.ViewState(VS_TempCrfSubDtl) = ObjCommon.GetCurDatetimeWithOffSet("India Standard Time")

        If Me.ViewState(VS_TempCrfSubDtl).ToString().Contains("+") Then
            Me.ViewState(VS_TempCrfSubDtl) = CDate(Me.ViewState(VS_TempCrfSubDtl).ToString().Split("+")(0).ToString()).ToString("dd-MMM-yyyy HH:mm").Trim()
        ElseIf Me.ViewState(VS_TempCrfSubDtl).ToString().Contains("-") Then
            Me.ViewState(VS_TempCrfSubDtl) = CDate(Me.ViewState(VS_TempCrfSubDtl).ToString().Split("-")(0).ToString()).ToString("dd-MMM-yyyy HH:mm").Trim()
        End If

        dtDosingDetail = CType(Me.ViewState(VS_dtViewDosingDetail), DataTable).Copy()
        dvDosingDetail = dtDosingDetail.DefaultView
        dvDosingDetail.RowFilter = "vSubjectId='" & Subjectid & "'"
        'dtDosingDetail.Dispose()
        dsDosingDetail.Tables.Add(dvDosingDetail.ToTable)
        txtScan.Text = ""
        dsDosingDetail.Tables(0).Rows(0)("vRemarks") = HdFieldRemarks.Value.ToString()
        dsDosingDetail.Tables(0).Rows(0)("iModifyBy") = Me.Session(S_UserID)
        dsDosingDetail.Tables(0).Rows(0)("dModifyOn") = Me.ViewState(VS_TempCrfSubDtl)
        dsDosingDetail.Tables(0).AcceptChanges()
        dsDosingDetail.Tables(0).TableName = "DosingDetail"
        txtScan.Focus()
        If Not ObjLambda.Save_DosingDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, dsDosingDetail, Me.Session(S_UserID), Retu_nSampleBarCode, Retu_vSampleBarCode) Then
            Me.ShowErrorMessage("Error While Saving Data in DosingDetail", eStr)
            Me.resetpage()
            Exit Sub
        End If
        'txtScan.Text = ""
        If Not FillGrid() Then
            Exit Sub
        End If

    End Sub

    Protected Sub BtnAfterMouthCheckDoneRadioSelected_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAfterMouthCheckDoneRadioSelected.Click
        SaveIPTime()
        txtScan.Text = ""
        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub

    Protected Sub BtnCancelDosing_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancelDosing.Click

        lblSubject.Text = ""
        lblIPLabelID.Text = ""
        lblMySubject.Text = ""
        Me.txtScan.Text = ""

        If Me.hfTextChnaged.Value.ToUpper.Trim() = "BARCODE" Then
            Me.hfTextChnaged.Value = ""
        End If
        Me.txtScan.Focus()
        fsBarcode.Visible = True


    End Sub

    Protected Sub btnReplace_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReplace.Click

    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim DtSubjects As New DataTable
        Dim DrSubjects As DataRow
        Dim index As Integer = 0
        If IsNothing(Me.Session("UserSubjectDtl")) Then

            If Not CreateSubjectTable(DtSubjects) Then
                Exit Sub
            End If

        Else

            DtSubjects = CType(Me.Session("UserSubjectDtl"), DataTable)

        End If

        DtSubjects.Clear()

        For index = 0 To Me.gvwSubjects.Rows.Count - 1

            If CType(Me.gvwSubjects.Rows(index).FindControl("ChkMove"), CheckBox).Checked = True Then

                DrSubjects = DtSubjects.NewRow
                DrSubjects("iUserId") = Me.Session(S_UserID)
                DrSubjects("vWorkspaceId") = Me.gvwSubjects.Rows(index).Cells(GVCSub_WorkspaceId).Text
                DrSubjects("vSubjectId") = Me.gvwSubjects.Rows(index).Cells(GVCSub_SubjectID).Text

                '' ******************************Changed On 21-May-2011 (Dharmesh Salla) ******************'
                '=========================================================================================='

                If Me.gvwSubjects.Rows(index).Cells(GVCSub_MySubjectNo).Text.Length > 4 Then
                    DrSubjects("iMySubjectNo") = Me.gvwSubjects.Rows(index).Cells(GVCSub_MySubjectNo).Text.ToString().Remove(4, (Me.gvwSubjects.Rows(index).Cells(GVCSub_MySubjectNo).Text.ToString().Length) - 4)
                Else
                    DrSubjects("iMySubjectNo") = Me.gvwSubjects.Rows(index).Cells(GVCSub_MySubjectNo).Text.ToString()
                End If
                '=========================================================================================='
                '' ***************************************************************************************** ''


                'DrSubjects("iMySubjectNo") = Me.gvwSubjects.Rows(index).Cells(GVCSub_MySubjectNo).Text
                DtSubjects.Rows.Add(DrSubjects)
                DtSubjects.AcceptChanges()

            End If

        Next index

        If IsNothing(Me.Session("UserSubjectDtl")) Then
            Me.Session.Add("UserSubjectDtl", DtSubjects)
        Else
            Me.Session("UserSubjectDtl") = DtSubjects
        End If

        btnClose_Click(sender, e)
        btnSearch_Click(sender, e)
        Me.UpControls.Update()
        Me.UpGridSubjectSample.Update()
        gvwSubjectSample.Visible = True
    End Sub

#End Region

#Region "Selected Index Change Event"

    Protected Sub ddlPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPeriod.SelectedIndexChanged

        gvwSubjectSample.DataSource = Nothing
        gvwSubjectSample.DataBind()
        gvwSubjects.DataSource = Nothing
        gvwSubjects.DataBind()
        Me.ddlDosingDay.Items.Clear()
        Me.ddlDosingNo.Items.Clear()
        UpGridSubjectSample.Update()
        UpControls.Update()
        btnSubjectMgmt.Enabled = False
        Me.txtScan.Enabled = False
        ViewState(VS_DtSave) = Nothing
        ViewState(VS_dtViewDosingDetail) = Nothing
        ViewState(VS_DtViewWorkSpaceSubjectMst) = Nothing
        If Not FillddlActivity() Then
            Exit Sub
        End If
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ThemeSelection();", "ThemeSelection();", True)
    End Sub

    Protected Sub ddlActivity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlActivity.SelectedIndexChanged

        Try
            If Me.ddlActivity.SelectedIndex > 0 Then
                Me.btnSubjectMgmt.Enabled = True
            End If
            If Not FillDosingDay() Then
                Exit Sub
            End If

            If Not FillDoseNo() Then
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......ddlActivity_SelectedIndexChanged")
        End Try

    End Sub

#End Region

#Region "gvwSubject Event"

    Protected Sub gvwSubjects_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwSubjects.RowCreated
        e.Row.Cells(GVCSub_WorkspaceId).Visible = False
        e.Row.Cells(GVCSub_MySubjectNo).Visible = False
    End Sub

    Protected Sub gvwSubjects_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwSubjects.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.Cells(GVCSub_Select).FindControl("ChkMove"), CheckBox).Attributes.Add("Onclick", "CheckSelected();")
        End If
    End Sub
#End Region

#Region "SaveIP Time"

    Private Function SaveIPTime()

        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds As New DataSet
        Dim ds_Save As New DataSet
        Dim SubjectID As String = String.Empty
        Dim ValidSubject As Boolean = False
        Dim DtViewWorkSpaceSubjectMst As DataTable
        Dim DvViewWorkSpaceSubjectMst As DataView
        Dim DvViewDosingDetail As DataView

        Try

            Me.hfTextChnaged.Value = "BARCODE"


            ScanText = Me.txtScan.Text.Trim

            If (ScanText.Contains("-")) Or (ScanText.Length < 8) Then

                If Not rbllstCompiance.SelectedIndex = 1 Then
                    Me.lblSubject.Text = ScanText.ToString()
                End If

                ValidSubject = False
                DvViewDosingDetail = CType(Me.ViewState(VS_dtViewDosingDetail), DataTable).DefaultView
                DvViewDosingDetail.RowFilter = "vSubjectId = '" + ScanText.ToString().Trim() + "'"
                If DvViewDosingDetail.ToTable().Rows.Count > 0 Then
                    ValidSubject = True
                End If
                DvViewDosingDetail = Nothing

                If ValidSubject = False Then
                    ObjCommon.ShowAlert("Subject Is Not Valid For You", Me.Page)
                    Me.resetpage()
                    Exit Try

                End If

                If Not Me.ViewState(VS_DtSave) Is Nothing Then

                    If ds_Save.Tables.Count < 1 Then
                        ds_Save.Tables.Add(CType(Me.ViewState(VS_DtSave), DataTable).Copy())
                        'Checking whether the subject belongs to same workspace or not
                    End If

                    'To Validate Subject
                    If Not IsNothing(ds_Save.Tables(0).Columns.Contains("vSubjectID")) AndAlso _
                        Not (ds_Save.Tables(0).Rows(0).Item("vSubjectID") Is System.DBNull.Value) AndAlso _
                        ds_Save.Tables(0).Rows(0).Item("vSubjectID").ToString.Trim() <> "" Then

                        SubjectID = ScanText.ToString()

                        If SubjectID <> Convert.ToString(ds_Save.Tables(0).Rows(0)("vSubjectID")) Then
                            ObjCommon.ShowAlert("Subject Is Not Valid For This Sample. This Sample Is For " + _
                                                ds_Save.Tables(0).Rows(0)("vSubjectID").ToString.Trim(), Me.Page)
                            Me.resetpage()
                            Exit Function
                        End If

                    End If
                    '***********************************************

                    If ds_Save.Tables(0).Rows(0).Item("vWorkSpaceId").ToString = Pro_Screening Then

                        wStr = " vSubjectID = '" + Me.lblSubject.Text.Trim + "'"
                        wStr += " And cRejectionFlag <> 'Y'"
                        If Not ObjHelp.GetView_SubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr) Then
                            Me.ShowErrorMessage("Error While Getting Data From View_SubjectMaster", eStr)
                            Me.resetpage()
                            Exit Function
                        End If

                    Else

                        'wStr = "vWorkSpaceId = '" + ds_Save.Tables(0).Rows(0).Item("vWorkSpaceId").ToString + "' And"
                        wStr = "vSubjectID = '" + Me.lblSubject.Text.Trim + "' And cRejectionFlag <> 'Y'"

                        DtViewWorkSpaceSubjectMst = CType(Me.ViewState(VS_DtViewWorkSpaceSubjectMst), DataTable)
                        DvViewWorkSpaceSubjectMst = DtViewWorkSpaceSubjectMst.DefaultView()
                        DvViewWorkSpaceSubjectMst.RowFilter = wStr
                        ds = Nothing
                        ds = New DataSet
                        ds.Tables.Add(DvViewWorkSpaceSubjectMst.ToTable().Copy())

                        If Not rbllstCompiance.SelectedIndex = 1 Then
                            If Not ds.Tables(0).Rows.Count < 1 Then
                                Me.lblMySubject.Text = ds.Tables(0).Rows(0)("vMySubjectNo").ToString()
                            End If
                        End If

                    End If

                Else

                    '==added on 29-jan-10= by Deepak Singh to add a filter of SubjectID =
                    'Added For "MySubjectNo" on 27-Jun-2009
                    'wStr = "vWorkSpaceId = '" + Me.HProjectId.Value.ToString + "' And"
                    If Me.ViewState(VS_DtViewWorkSpaceSubjectMst) Is Nothing Then
                        Me.ObjCommon.ShowAlert("Please Do Subject Management", Me.Page)
                        Me.txtScan.Text = ""
                        Me.txtScan.Focus()
                        Exit Function
                    End If
                    wStr = "vSubjectID = '" + Me.lblSubject.Text.Trim + "' And cRejectionFlag <> 'Y'"
                    DtViewWorkSpaceSubjectMst = CType(Me.ViewState(VS_DtViewWorkSpaceSubjectMst), DataTable)
                    DvViewWorkSpaceSubjectMst = DtViewWorkSpaceSubjectMst.DefaultView()
                    DvViewWorkSpaceSubjectMst.RowFilter = wStr
                    ds = Nothing
                    ds = New DataSet
                    ds.Tables.Add(DvViewWorkSpaceSubjectMst.ToTable().Copy())

                    If Not rbllstCompiance.SelectedIndex = 1 Then
                        If ds.Tables(0).Rows.Count > 0 Then

                            If Not ds.Tables(0).Rows(0).Item("vWorkSpaceId").ToString() Is Nothing Or _
                                        ds.Tables(0).Rows(0).Item("vWorkSpaceId").ToString() = "" Then

                                Me.lblMySubject.Text = ds.Tables(0).Rows(0)("vMySubjectNo").ToString()

                            End If

                        Else

                            wStr = " vSubjectID = '" + Me.lblSubject.Text.Trim + "'"
                            wStr += " And cRejectionFlag <> 'Y'"
                            ds = Nothing
                            ds = New DataSet

                            If Not ObjHelp.GetView_SubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr) Then
                                Me.ShowErrorMessage("Error While Getting Data From View_SubjectMaster", eStr)
                                Me.resetpage()
                                Exit Function
                            End If

                            If Not ds.Tables(0).Rows(0).Item("vWorkSpaceId").ToString() Is Nothing Or _
                                        ds.Tables(0).Rows(0).Item("vWorkSpaceId").ToString() = "" Then

                                Me.lblMySubject.Text = "Not Assigned"
                            End If

                        End If

                    End If

                    '**************************************************
                End If

                If ds.Tables(0).Rows.Count < 1 Then
                    ObjCommon.ShowAlert("Subject Is Not Valid", Me.Page)
                    Me.resetpage()
                    Exit Function
                End If
                ds = Nothing

            Else

                ValidSubject = False
                DvViewDosingDetail = CType(Me.ViewState(VS_dtViewDosingDetail), DataTable).DefaultView
                DvViewDosingDetail.RowFilter = "vDosingBarCode = '" + ScanText.ToString().Trim() + "'"
                If DvViewDosingDetail.ToTable().Rows.Count > 0 Then
                    ValidSubject = True
                End If
                DvViewDosingDetail = Nothing

                If ValidSubject = False Then
                    ObjCommon.ShowAlert("Sample Is Not Valid For You", Me.Page)
                    Me.resetpage()
                    Exit Function
                End If

                wStr = "vDosingBarCode = '" + ScanText.Trim() + "'"
                DvViewDosingDetail = CType(Me.ViewState(VS_dtViewDosingDetail), DataTable).DefaultView
                DvViewDosingDetail.RowFilter = wStr
                ds = Nothing
                ds = New DataSet
                ds.Tables.Add(DvViewDosingDetail.ToTable().Copy())

                If ds.Tables(0).Rows.Count < 1 Then
                    ObjCommon.ShowAlert("Sample Is Not Valid", Me.Page)
                    Me.resetpage()
                    Exit Function
                End If

                If (Not ds.Tables(0).Rows(0)("vSubjectID") Is DBNull.Value AndAlso _
                        ds.Tables(0).Rows(0)("vSubjectID").ToString.Trim() <> "") And _
                    (Not ds.Tables(0).Rows(0)("dDosedOn") Is DBNull.Value AndAlso _
                        ds.Tables(0).Rows(0)("dDosedOn").ToString.Trim() <> "") Then

                    ObjCommon.ShowAlert("Sample Is Already Assigned To The Subject", Me.Page)
                    Me.resetpage()
                    Exit Function

                End If

                Me.ViewState(VS_DtSave) = ds.Tables(0)
                ds = Nothing
                Me.lblIPLabelID.Text = ScanText.Trim

            End If

            Me.txtScan.Text = ""
            Me.txtScan.Focus()

            If Me.lblIPLabelID.Text <> "" And Me.lblSubject.Text <> "" Then

                'Added code for checking deviation on 09-09-2009

                'ValidSubject = False
                'DvViewDosingDetail = CType(Me.ViewState(VS_dtViewDosingDetail), DataTable).DefaultView
                'DvViewDosingDetail.RowFilter = "vDosingBarCode = '" + Me.lblIPLabelID.Text.Trim() + "' And vSubjectId = '" + Me.lblSubject.Text.Trim() + "'"
                'If DvViewDosingDetail.ToTable().Rows.Count > 0 Then
                '    ValidSubject = True
                'End If
                'DvViewDosingDetail = Nothing

                'If ValidSubject = False Then
                '    ObjCommon.ShowAlert("Sample Is Not Valid For Selected Subject", Me.Page)
                '    Me.resetpage()
                '    Exit Function
                'End If

                If Me.lblIPLabel.Text <> "" And Me.lblSubject.Text <> "" Then
                    If Not AssignValues1() Then
                        resetpage()
                        Exit Function
                    End If
                End If

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....SaveIPTime")

        Finally
            fsBarcode.Visible = True

        End Try

    End Function

#End Region

#Region "Temp_SubjectSampleGridBind" '' Added by dipen shah on 30-dec-2014 for third gun on Ip administration activity.. 
    Private Function TempSubjectSampleGridBind() As Boolean
        Dim wstr As String = String.Empty
        Dim dtUserSubject As DataTable
        Dim dvTemp As DataView
        Dim dvUserSubject As DataView
        Dim ds_Save As New DataSet
        Dim Subjects As String = String.Empty
        Dim ds_DosingDetail As New DataSet
        Dim estr As String = String.Empty
        Dim Dt_DosingDetail As New DataTable
        Try
            wstr = "vDosingBarCode = '" + Me.lblIPLabelID.Text.Trim() + "'"
            dvTemp = CType(Me.ViewState(VS_dtViewDosingDetail), DataTable).DefaultView
            dvTemp.RowFilter = wstr
            ds_Save.Tables.Add(dvTemp.ToTable.Copy())
            ds_Save.AcceptChanges()

            wstr = "nDosingDetailNo=" & ds_Save.Tables(0).Rows(0).Item("nDosingDetailNo").ToString.Trim()

            If Not Me.ObjHelp.GetDosingDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    ds_DosingDetail, estr) Then

            End If

            Dt_DosingDetail = ds_DosingDetail.Tables(0).Copy()

            Me.ViewState(VS_TempDosedOn) = ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName))
            Me.ViewState(VS_TempCrfSubDtl) = ObjCommon.GetCurDatetimeWithOffSet("India Standard Time")

            If Me.ViewState(VS_TempCrfSubDtl).ToString().Contains("+") Then
                Me.ViewState(VS_TempCrfSubDtl) = CDate(Me.ViewState(VS_TempCrfSubDtl).ToString().Split("+")(0).ToString()).ToString("dd-MMM-yyyy HH:mm").Trim()
            ElseIf Me.ViewState(VS_TempCrfSubDtl).ToString().Contains("-") Then
                Me.ViewState(VS_TempCrfSubDtl) = CDate(Me.ViewState(VS_TempCrfSubDtl).ToString().Split("-")(0).ToString()).ToString("dd-MMM-yyyy HH:mm").Trim()
            End If

            For Each dr_dosing As DataRow In Dt_DosingDetail.Rows
                dr_dosing("iDosedBy") = Me.Session(S_UserID)
                dr_dosing("iDosingSupervisor") = Me.ddlDosingSupervisor.SelectedItem.Value.Trim()
                dr_dosing("dDosedOn") = Me.ViewState(VS_TempDosedOn)
                dr_dosing("vWaterAdministered") = Me.txtWaterQuantity.Text.Trim()
                dr_dosing("cStatusIndi") = "E"
                dr_dosing.AcceptChanges()

            Next dr_dosing
            Dt_DosingDetail.AcceptChanges()

            wstr = "vWorkspaceId = '" + Me.HProjectId.Value + "' And cStatusIndi <> 'D'"

            wstr += " And iDoseNo = " + Me.ddlDosingNo.SelectedItem.Value.Trim() + _
                    " And iDayNo = " + Me.ddlDosingDay.SelectedItem.Value.Trim() + " And iPeriod = " + _
                    Me.ddlPeriod.SelectedItem.Value.Trim()

            dtUserSubject = CType(Me.Session("UserSubjectDtl"), DataTable)
            dvUserSubject = dtUserSubject.DefaultView
            dvUserSubject.RowFilter = "vWorkspaceId = '" + Me.HProjectId.Value + "'"
            dtUserSubject = dvUserSubject.ToTable()

            For Each dr1 As DataRow In dtUserSubject.Rows
                Subjects += IIf(Subjects = "", "'" & dr1("iMySubjectNo"), "','" & dr1("iMySubjectNo"))
            Next

            Subjects += "'"
            wstr += " And iMySubjectNo in (" & Subjects & ") order by iMySubjectNo"
            If Not ObjHelp.View_DosingDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_DosingDetail, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From View_SampleDetail : ", estr)
                Return False
            End If
            ds_DosingDetail.Tables(0).Columns.Add("dDosedOnDatetime")
            If Not ds_DosingDetail Is Nothing AndAlso ds_DosingDetail.Tables(0).Rows.Count > 0 Then
                For Each dr In ds_DosingDetail.Tables(0).Rows
                    If dr("nDosingDetailNo") = ds_Save.Tables(0).Rows(0).Item("nDosingDetailNo").ToString.Trim() Then
                        dr("iDosedBy") = Dt_DosingDetail.Rows(0)("iDosedBy").ToString()
                        dr("iDosingSupervisor") = Me.ddlDosingSupervisor.SelectedItem.Value.Trim()
                        ''dr("vSupervisorName") = CType(Master.FindControl("lblUserName"), Label).Text
                        dr("vSupervisorName") = ddlDosingSupervisor.SelectedItem.Text.ToString()
                        dr("dDosedOn") = Me.ViewState(VS_TempDosedOn)
                        dr("vWaterAdministered") = Dt_DosingDetail.Rows(0)("vWaterAdministered").ToString()
                        dr("cStatusIndi") = Dt_DosingDetail.Rows(0)("cStatusIndi").ToString()
                        ''dr("vDoserName") = ddlDosingSupervisor.SelectedItem.Text.ToString()
                        dr("vDoserName") = CType(Master.FindControl("lblUserName"), Label).Text
                        dr("vRemarks") = Me.HdFieldRemarks.Value.ToString()
                        dr("dModifyOn") = Me.ViewState(VS_TempCrfSubDtl)
                        ds_DosingDetail.Tables(0).AcceptChanges()
                    End If
                Next
                For Each dr In ds_DosingDetail.Tables(0).Rows
                    If Not dr("dDosedOn").ToString() = "" Then
                        dr("dDosedOnDatetime") = CType(dr("dDosedOn"), DateTime).ToString("dd-MMM-yyyy HH:mm")
                    Else
                        dr("dDosedOnDatetime") = ""
                    End If
                    ds_DosingDetail.Tables(0).AcceptChanges()
                Next
                If Not ds_DosingDetail Is Nothing AndAlso ds_DosingDetail.Tables(0).Rows.Count > 0 Then
                    Me.gvwSubjectSample.DataSource = ds_DosingDetail
                    Me.gvwSubjectSample.DataBind()
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ThemeSelection();", "ThemeSelection();", True)
                    Me.UpGridSubjectSample.Update()
                Else
                    ObjCommon.ShowAlert("No data found.", Me.Page)
                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message + "Error while Temp()..", "")
        End Try
        Return True
    End Function
#End Region

#Region "Reset Page"

    Protected Sub resetpage()
        Me.lblIPLabelID.Text = ""
        Me.lblSubject.Text = ""
        Me.txtScan.Text = ""
        Me.txtScan.Focus()
        Me.lblMySubject.Text = ""
        Me.ViewState(VS_DtSave) = Nothing
        'Me.txtRemarks.Text = ""
        'Me.divDeviation.Visible = False
        'Me.PnlDeviation.Visible = False
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

#Region "Method For Check Activity Deviation"

    Private Function CheckSequence(ByVal vWorkspaceId As String, _
                                         ByVal NodeId As Integer, _
                                         ByVal SubjectId As String, _
                                         ByVal Period As Integer) As Boolean

        Dim eStr As String = String.Empty
        Dim Param As String = String.Empty
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Dim Ds_Structure As DataSet = Nothing
        Dim SeqNo As Integer
        Dim Str As String = String.Empty

        Dim PendingNode As String = String.Empty
        Dim i As Integer
        Dim temp As Array

        Dim lbl As Label
        Dim gridView As GridView



        Try
            Param = vWorkspaceId + "##" + SubjectId + "##" + Period.ToString()
            If Not objHelp.Proc_GetStructure(Param, Ds_Structure, eStr) Then
                Return False
            End If

            If Ds_Structure.Tables(0).Rows.Count > 0 Then

                temp = Regex.Split(SubjectId, ",")
                SeqNo = Ds_Structure.Tables(0).Select(" iNodeId = " & NodeId.ToString)(0).Item("SeqNo")

                For i = 0 To temp.Length - 1
                    If Ds_Structure.Tables(0).Select(" iNodeId = " & NodeId.ToString & " And vSubjectID = '" & temp(i).ToString & "'")(0).Item("ActivityStatus") <> CRF_DataEntryPending Then
                        Continue For
                    End If

                    Ds_Structure.Tables(0).DefaultView.RowFilter = " SeqNo < " & SeqNo.ToString & " And ActivityStatus = '" & CRF_DataEntryPending & "'" & " And vSubjectID = '" & temp(i).ToString & "'"

                    If Ds_Structure.Tables(0).DefaultView.ToTable.Rows.Count > 0 Then

                        For Each dr As DataRow In Ds_Structure.Tables(0).DefaultView.ToTable.Rows
                            If PendingNode.ToString = "" Then
                                PendingNode = dr.Item("iNodeId").ToString()
                            Else
                                PendingNode = PendingNode.ToString().Trim + "," + dr.Item("iNodeId").ToString()
                            End If

                        Next dr

                        Str += temp(i).ToString() + "@@" + PendingNode.ToString() + "##"
                        PendingNode = String.Empty
                        lbl = New Label
                        lbl.ID = "lblDeviation" & i
                        lbl.CssClass = "Label"
                        lbl.Text = Ds_Structure.Tables(0).DefaultView.ToTable().Rows(0)("vMySubjectNo")

                        gridView = New GridView
                        gridView.ID = "GvDeviation" & i
                        gridView.AutoGenerateColumns = False
                        gridView.SkinID = "grdViewSmlAutoSize"
                        gridView.Attributes.Add("style", "width:100%")
                        'gridView.CellSpacing = "25"

                        Dim bf As BoundField = New BoundField
                        Dim dc As DataColumn
                        dc = New DataColumn(Ds_Structure.Tables(0).DefaultView.ToTable().Columns("vNodeDisplayName").ColumnName)

                        bf.DataField = dc.ColumnName
                        bf.HeaderText = "Pending Activity"
                        gridView.Columns.Add(bf)
                        gridView.DataSource = Ds_Structure.Tables(0).DefaultView.ToTable()
                        gridView.DataBind()
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ThemeSelection();", "ThemeSelection();", True)
                        Me.PlaceDeviation.Controls.Add(lbl)
                        Me.PlaceDeviation.Controls.Add(gridView)
                    End If
                Next i
                Me.HPendingNode.Value = Str.ToString()
            Else
                Me.HPendingNode.Value = ""
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "...CheckSequence")
            Return False
        End Try
    End Function
#End Region

    Private Function FillDoserName() As Boolean
        Dim ds_User As New Data.DataSet
        Dim dt_User As New DataTable
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty

        Try


            wstr = "vLocationCode = '" + Me.Session(S_LocationCode).ToString + "' AND vUserTypeName = 'Dosing' AND cStatusIndi <> 'D'"
            wstr += " Order by vUserName"

            If Not ObjHelp.GetViewUserMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        ds_User, estr) Then
                Throw New Exception("Error While Getting Data From View_UserMst:" + estr)
            End If

            For CntOfDs_User As Integer = 0 To ds_User.Tables(0).Rows.Count - 1
                ds_User.Tables(0).Rows(CntOfDs_User).Item("vUserName") = ds_User.Tables(0).Rows(CntOfDs_User).Item("vUserName").ToString() + "   " + "(" + ds_User.Tables(0).Rows(CntOfDs_User).Item("vUserTypeName").ToString() + ")"
            Next CntOfDs_User

            ds_User.Tables(0).AcceptChanges()
            dt_User = ds_User.Tables(0).DefaultView.ToTable(True, "iUserId,vUserName".Split(","))

            Me.ddlDoserName.DataSource = dt_User
            Me.ddlDoserName.DataValueField = "iUserId"
            Me.ddlDoserName.DataTextField = "vUserName"
            Me.ddlDoserName.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ThemeSelection();", "ThemeSelection();", True)
            Me.ddlDoserName.Items.Insert(0, New ListItem("--Select Doser Names--", 0))


            If (CType(Session(S_UserType), Integer) = 16) Then
                ddlDoserName.SelectedValue = Session(S_UserID)
            Else
                ddlDoserName.SelectedIndex = 0
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillDoserName", ex)
            Return False
        End Try

    End Function

    Protected Sub btnDisplayNone_Click(sender As Object, e As EventArgs)
        lblSubject.Text = ""
        lblMySubject.Text = ""
        txtScan.Text = ""
        txtScan.Focus()
    End Sub
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
End Class
