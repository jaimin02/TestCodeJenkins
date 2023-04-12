
Partial Class frmArchiveLabReportReview
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION"

    Private ObjCommon As New clsCommon
    Private ObjHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private ObjLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_chkedit As Integer = 1
    Private Const GVC_vMedexDesc As Integer = 2
    Private Const GVC_vMedexResult As Integer = 3
    Private Const GVC_vLowRange As Integer = 4
    Private Const GVC_vHighRange As Integer = 5
    Private Const GVC_vUOM As Integer = 6
    Private Const GVC_ChkNormal As Integer = 7
    Private Const GVC_ChkAbnormal As Integer = 8
    Private Const GVC_ChkClSignificant As Integer = 9
    Private Const GVC_Remarks As Integer = 10
    Private Const GVC_AuditTrail As Integer = 11
    Private Const GVC_SampleId As Integer = 12
    Private Const GVC_tranno As Integer = 13
    Private Const GVC_Normalflag As Integer = 14
    Private Const GVC_Abnormalflag As Integer = 15
    Private Const GVC_ClinicallySignflag As Integer = 16
    Private Const GVC_GeneralRemark As Integer = 17
    Private Const GVC_Medexcode As Integer = 18
    Private Const VS_dsSublabRpt As String = "ds_viewSublabRpt"
    Private Const VS_dsSubLabReportDtl As String = "ds_SubLabReportDtl"
    Private Const VS_dtSubjectLabReportDtl As String = "dt_SubjectLabReportDtl"
    Private Const VS_dtMedExScreeningHdrDtl As String = "dt_MedExScreeningHdrDtlAuditTrail"
    Private Const VS_dsLabRptLockUnlockDtl As String = "ds_LabRptLockUnlockDtl"
    'Private Const VS_Choice As String = "Choice"
    Private Const VS_SubjectID As String = "SubjectId"
    Private Const VS_ViewLabLockUnlockDtl As String = "ds_ViewLabLockUnlockDtl"
    Private Const VS_ScrDate As String = "ScrDate"
    Private Const GVCHist_Test As Integer = 0
    Private Const GVCHist_Abnormal As Integer = 1
    Private Const GVCHist_ClinicallySig As Integer = 2
    Private Const GVCHist_Remarks As Integer = 3
    Private Const GVCHist_UserName As Integer = 5
    Private Const GVCHist_SampleId As Integer = 6
    Private Const GVCHist_MedexCode As Integer = 7
    Private Const GVCHist_TranNo As Integer = 8
    Private Const GVCAudit_vUserName As Integer = 0
    Private Const GVCAudit_dReviewedOn As Integer = 1
    Private Const GVCAudit_vRemarks As Integer = 2
    Private Const GVCAudit_nSampleId As Integer = 3
    Private Const GVCAudit_iReviewedBy As Integer = 4
    Private Const GVCAudit_iTranNo As Integer = 5
    Private Const GVCTRF_SrNo As Integer = 0
    Private Const GVCTRF_nSampleId As Integer = 3
    Private Const GVSampletype_SrNo As Integer = 0
    Private Const GVSampletype_nSampleId As Integer = 3
    Private Const Str_Chemistry As String = "CHEMISTRY"
    Private Const Str_Hematology As String = "HEMATOLOGY"
    Private Const Str_Immunology As String = "IMMUNOLOGY"
    Private Const Str_Urianalysis As String = "URIANALYSIS"
    Private Const Str_Coagulation As String = "COAGULATION"
    Private Const Str_STOOLEXAMINATION As String = "STOOLEXAMINATION"
    Private Const Str_HIV1 As String = "WESTERN BLOT FOR HIV -I"
    Private Const Str_PAPSmear As String = "PAP SMEAR REPORT"
    Public Const MedexGrp_PAPSmear As String = "00105"
    Private Str_Rev As String = String.Empty
    Private TestPerform As String = String.Empty
    Dim iperiodNo As Integer

#End Region

#Region "FORM LOAD"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try
    End Sub

#End Region

#Region "GenCall() "

    Private Function GenCall() As Boolean
        Dim eStr As String = ""
        Dim wStr As String = ""
        Dim ds As New DataSet
        'Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Me.txtproject.Enabled = True
            Me.chkInProject.Enabled = True


            'If Not IsNothing(Me.Request.QueryString("mode")) Then
            '    Me.ViewState(VS_Choice) = CType(Me.Request.QueryString("mode"), WS_Lambda.DataObjOpenSaveModeEnum)
            'End If


            If Not IsNothing(Me.Request.QueryString("SubjectId")) Then
                Me.ViewState(VS_SubjectID) = Me.Request.QueryString("SubjectId")
            End If

            If Not IsNothing(Me.Request.QueryString("ScrDateNo")) Then

                Me.ViewState(VS_ScrDate) = Me.Request.QueryString("ScrDateNo")
                Me.txtproject.Enabled = False
                Me.chkInProject.Enabled = False
            End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function

#End Region

#Region " GenCall_Data "

    Private Function GenCall_Data(ByRef dt_Dist_Retu As DataSet) As Boolean


        Dim ds_viewSublabRpt As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_SubLabReportDtl As New DataSet
        Dim ds_LabRptLockUnlockDtl As New DataSet


        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            'wStr = "nSampleid =1693" 'done for testing purpose(to be changed afterward)

            'If Not ObjHelp.View_SubjectLabRptDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '                        ds_viewSublabRpt, eStr) Then

            '    Throw New Exception(eStr)

            'End If

            'ViewState(VS_dsSublabRpt) = ds_viewSublabRpt


            If Not ObjHelp.GetLabRptLockUnlockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                               ds_LabRptLockUnlockDtl, eStr) Then

                Throw New Exception(eStr)

            End If
            ViewState(VS_dsLabRptLockUnlockDtl) = ds_LabRptLockUnlockDtl


            If Not ObjHelp.GetSubjectLabReportDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                      ds_SubLabReportDtl, eStr) Then

                Throw New Exception(eStr)

            End If
            ViewState(VS_dsSubLabReportDtl) = ds_SubLabReportDtl



            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            GenCall_Data = False
        Finally
        End Try
    End Function

#End Region

#Region " GenCall_showUI() "

    Private Function GenCall_ShowUI() As Boolean
        Dim sender As New Object
        Dim e As New EventArgs
        Dim dv_SubLabrpt As New DataView
        Dim dt_SubLabrpt As New DataTable
        Dim ds_viewSublabRpt As DataSet = CType(ViewState(VS_dsSublabRpt), DataSet)
        Dim Str_FirstGrp As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = " :: Archive Lab Report Review ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Lab Result"

            Me.AutoCompleteExtender1.ContextKey = Session(S_LocationCode)
            '==added on 11-Nov-2011 by Mrunal Parekh to show project according to user
            Me.AutoCompleteExtender2.ContextKey = "iUserId = " & Me.Session(S_UserID)
            '========
            Me.HschemaId.Value = Me.Request.QueryString("SchemaId").ToString()
            'If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
            'If (Not Me.Request.QueryString("vWorkSpaceId") Is Nothing) AndAlso (Not Me.Request.QueryString("Type") Is Nothing) Then
            If (Not Me.Request.QueryString("Type") Is Nothing) Then
                Me.RBLProjecttype.Items(0).Enabled = False
                Me.RBLProjecttype.SelectedValue = 1
                RBLProjecttype_SelectedIndexChanged(sender, e)
                Me.HProjectId.Value = Request.QueryString("vWorkSpaceId").ToString()
                Me.txtproject.Text = Request.QueryString("ProjectNo").ToString()
                Me.txtproject.Enabled = False
                Me.btnSetProject_Click(sender, e)
                Me.chkInProject.Checked = True
                Me.chkInProject_CheckedChanged(sender, e)
                Me.trScreeningDate.Style.Add("display", "none")
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowDiv", "document.getElementById('ctl00_CPHLAMBDA_divSubjectSelection').style.display='';", True)
                Me.chkReviewed.Visible = False
                'Me.btnSave.Enabled = False
                'Me.btnCancel.Enabled = False
                'HideMenu()
                'Exit Function
                'End If
            End If

            trShowHide()

            'Choice = Me.Request.QueryString("mode")
            'If (Choice.Equals(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View)) Then


            Me.txtSubject.Text = Me.Request.QueryString("SubjectId")

            'End If
            'If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then


            If (Not Me.ViewState(VS_ScrDate) Is Nothing) Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowDiv", "document.getElementById('ctl00_CPHLAMBDA_divSubjectSelection').style.display='';", True)

                Me.RBLProjecttype.Items(1).Enabled = False
                Me.trSearchSub.Style.Add("display", "none")
                Me.btnSubject_Click(sender, e)
                Me.rblScreeningDate.Items(0).Selected = True
                Me.rblScreeningDate_SelectedIndexChanged(sender, e)
                'Me.btnCancel.Enabled = False
                'Me.btnSave.Enabled = False
                Me.chkReviewed.Visible = False
                HideMenu()
                Exit Function
            End If
            'End If
            'If Not IsNothing(ViewState(VS_Choice)) Then
            '    Choice = CType(ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum)

            '    If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
            btnSubject_Click(sender, e)
            Me.txtSubject.Enabled = False
            'Me.chkReviewed.Visible = False
            'Me.btnSave.Enabled = False
            Me.btnExit.Attributes.Add("onclick", "return closewindow();")

            'HideMenu()

            '    ElseIf Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None Then
            'Me.chkReviewed.Visible = False
            'Me.btnSave.Enabled = False
            '    End If


            'End If

            If Me.gvwtstChemistry.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ChangedDisabled", "ChangedDisabled();", True)
            End If


            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage("Error while Gencall_ShowUI", ex.Message)
            GenCall_ShowUI = False
        Finally
        End Try
    End Function

#End Region

#Region "HideMenu"

    Private Sub HideMenu()
        Dim tmpMenu As Menu
        Dim tmpddlprofile As DropDownList

        'lblProfile()
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

#Region "FillGrid gvwtstChemistry"

    Private Function FillGridview(ByVal TstGroup As String) As Boolean

        Dim sender As New Object
        Dim e As New EventArgs
        Dim dtSubLabrpt As DataTable = Nothing
        Dim dvSubLabrpt As New DataView
        Dim ds_SublabRpt As New DataSet
        Dim wStr As String = String.Empty

        Try

            If Not GetViewSubjectLabRptDtl() Then
                ObjCommon.ShowAlert("Error while filling grid", Me)
                Exit Function
            End If
            ds_SublabRpt = CType(ViewState(VS_dsSublabRpt), DataSet)



            Me.btnFnlRmkAudit.Visible = True
            'fill gvwChemistry
            If TstGroup.ToUpper.Trim = Str_Chemistry Then

                wStr = GeneralModule.MedexGrp_Chemistry

            ElseIf TstGroup.ToUpper.Trim = Str_Hematology Then

                wStr = GeneralModule.MedexGrp_HEMATOLOGY

            ElseIf TstGroup.ToUpper.Trim = Str_Immunology Then

                wStr = GeneralModule.MedexGrp_IMMUNOLOGY

            ElseIf TstGroup.ToUpper.Trim = Str_Urianalysis Then

                wStr = GeneralModule.MedexGrp_URIANALYSIS

            ElseIf TstGroup.ToUpper.Trim = Str_Coagulation Then

                wStr = GeneralModule.MedexGrp_COAGULATION

            ElseIf TstGroup.ToUpper.Trim = Str_STOOLEXAMINATION Then

                wStr = GeneralModule.MedexGrp_STOOL

            ElseIf TstGroup.ToUpper.Trim = Str_HIV1 Then

                wStr = GeneralModule.MedexGrp_HIV1

            ElseIf TstGroup.ToUpper.Trim = Str_PAPSmear Then

                wStr = MedexGrp_PAPSmear

            End If

            If ds_SublabRpt.Tables(0).Rows.Count > 0 Then
                dvSubLabrpt = ds_SublabRpt.Tables(0).DefaultView
                dvSubLabrpt.RowFilter = "vMedexGroupCode = '" + wStr + "'"
                dvSubLabrpt.Sort = "iseqNo"
                dtSubLabrpt = dvSubLabrpt.ToTable

                Me.gvwtstChemistry.DataSource = dtSubLabrpt
                Me.gvwtstChemistry.DataBind()

                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ChangedDisabled", "ChangedDisabled();", True)

                Me.trRptRemarks.Attributes.Add("style", "display:none")
                Me.txtRptRemarks.Value = ""
                If GetRptRemarks(ds_SublabRpt, wStr) Then

                    Me.trRptRemarks.Attributes.Add("style", "display:block")
                    Me.txtRptRemarks.Value = Str_Rev.Trim()

                End If
            End If
            Return True

        Catch ex As Exception
            ShowErrorMessage("", ex.Message)
            Return False
        End Try

    End Function
#End Region

#Region "FillGrid GVHistoryDtl"

    Private Sub FillGrid(ByVal Sampleid As String, ByVal medexcode As String)
        Dim wstr As String = String.Empty
        Dim ds_SUbLabRtpAudit As New DataSet
        Dim eStr As String = String.Empty
        Try

            wstr = Me.HschemaId.Value.Trim() + "##" + Sampleid + "##" + medexcode + "##"

            'If Not ObjHelp.Proc_SubjectLabReportDtl_Audit(wstr, ds_SUbLabRtpAudit, eStr) Then

            '    Throw New Exception(eStr)
            'End If

            If Not ds_SUbLabRtpAudit.Tables(0).Rows.Count <= 0 Then
                Me.GVHistoryDtl.DataSource = ds_SUbLabRtpAudit
                Me.GVHistoryDtl.DataBind()
            End If


        Catch ex As Exception
            Me.ShowErrorMessage("Error while Filling getting Audit Trail", ex.Message)
        End Try



    End Sub
#End Region

#Region "FillGrid GvwTRF"

    Private Function FillgvwTRF() As Boolean
        Dim ds_TRF As New DataSet
        Dim eStr As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim dv_TRF As DataView
        Dim dt_TRF As New DataTable
        Try

            Wstr = " nSampleID = " + Me.rblSampleid.SelectedValue.Trim + " Order By vTemplateCode"
            If Not ObjHelp.View_RptTRFInfo(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_TRF, eStr) Then

                Throw New Exception(eStr)
            End If

            If ds_TRF.Tables(0).Rows.Count > 0 Then
                gvwTRF.DataSource = ds_TRF
                gvwTRF.DataBind()

                Me.lblTRF.Text = ds_TRF.Tables(0).Rows(0)("vSampleID")
                Me.lblSampleCollectedOn.Text = CType(ds_TRF.Tables(0).Rows(0)("dCollectionDateTime"), Date).GetDateTimeFormats()(23)
                Me.lblCollectedby.Text = ds_TRF.Tables(0).Rows(0)("Collectedby")
                Me.lblSampleReceivedOn.Text = CType(ds_TRF.Tables(0).Rows(0)("dReceiveDateTime"), Date).GetDateTimeFormats()(23)
                Me.lblReceivedby.Text = ds_TRF.Tables(0).Rows(0)("ReceivedBy")

                dv_TRF = ds_TRF.Tables(0).DefaultView
                dt_TRF = dv_TRF.ToTable(True, "nSampleId,vSampleTypeDesc,fVolume".Split(","))
                gvwSampletype.DataSource = dt_TRF
                gvwSampletype.DataBind()
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error while filling TRFgrid", ex.Message)
            Return False
        End Try


    End Function
#End Region

#Region "LnkButton Events"

    Protected Sub lnkChemistry_Click(ByVal sender As Object, ByVal e As System.EventArgs)


        Me.lnkChemistry.ForeColor = Drawing.Color.HotPink
        Me.lnkCOAGULATION.ForeColor = Drawing.Color.White
        Me.lnkHEMATOLOGY.ForeColor = Drawing.Color.White
        Me.LnkIMMUNOLOGY.ForeColor = Drawing.Color.White
        Me.lnkURIANALYSIS.ForeColor = Drawing.Color.White
        Me.lnkSTOOL.ForeColor = Drawing.Color.White
        Me.lnkHIV1.ForeColor = Drawing.Color.White
        Me.lnkPAP.ForeColor = Drawing.Color.White

        If Not FillGridview(Str_Chemistry) Then
            Exit Sub
        End If


    End Sub

    Protected Sub lnkCOAGULATION_Click(ByVal sender As Object, ByVal e As System.EventArgs)


        Me.lnkChemistry.ForeColor = Drawing.Color.White
        Me.lnkCOAGULATION.ForeColor = Drawing.Color.HotPink
        Me.lnkHEMATOLOGY.ForeColor = Drawing.Color.White
        Me.LnkIMMUNOLOGY.ForeColor = Drawing.Color.White
        Me.lnkURIANALYSIS.ForeColor = Drawing.Color.White
        Me.lnkSTOOL.ForeColor = Drawing.Color.White
        Me.lnkHIV1.ForeColor = Drawing.Color.White
        Me.lnkPAP.ForeColor = Drawing.Color.White

        If Not FillGridview(Str_Coagulation) Then
            Exit Sub
        End If

    End Sub

    Protected Sub LnkIMMUNOLOGY_Click(ByVal sender As Object, ByVal e As System.EventArgs)


        Me.lnkChemistry.ForeColor = Drawing.Color.White
        Me.lnkCOAGULATION.ForeColor = Drawing.Color.White
        Me.lnkHEMATOLOGY.ForeColor = Drawing.Color.White
        Me.LnkIMMUNOLOGY.ForeColor = Drawing.Color.HotPink
        Me.lnkURIANALYSIS.ForeColor = Drawing.Color.White
        Me.lnkSTOOL.ForeColor = Drawing.Color.White
        Me.lnkHIV1.ForeColor = Drawing.Color.White
        Me.lnkPAP.ForeColor = Drawing.Color.White

        If Not FillGridview(Str_Immunology) Then
            Exit Sub
        End If

    End Sub

    Protected Sub lnkHEMATOLOGY_Click(ByVal sender As Object, ByVal e As System.EventArgs)


        Me.lnkChemistry.ForeColor = Drawing.Color.White
        Me.lnkCOAGULATION.ForeColor = Drawing.Color.White
        Me.lnkHEMATOLOGY.ForeColor = Drawing.Color.HotPink
        Me.LnkIMMUNOLOGY.ForeColor = Drawing.Color.White
        Me.lnkURIANALYSIS.ForeColor = Drawing.Color.White
        Me.lnkSTOOL.ForeColor = Drawing.Color.White
        Me.lnkHIV1.ForeColor = Drawing.Color.White
        Me.lnkPAP.ForeColor = Drawing.Color.White

        If Not FillGridview(Str_Hematology) Then
            Exit Sub
        End If

    End Sub

    Protected Sub lnkURIANALYSIS_Click(ByVal sender As Object, ByVal e As System.EventArgs)


        Me.lnkChemistry.ForeColor = Drawing.Color.White
        Me.lnkCOAGULATION.ForeColor = Drawing.Color.White
        Me.lnkHEMATOLOGY.ForeColor = Drawing.Color.White
        Me.LnkIMMUNOLOGY.ForeColor = Drawing.Color.White
        Me.lnkURIANALYSIS.ForeColor = Drawing.Color.HotPink
        Me.lnkSTOOL.ForeColor = Drawing.Color.White
        Me.lnkHIV1.ForeColor = Drawing.Color.White
        Me.lnkPAP.ForeColor = Drawing.Color.White

        If Not FillGridview(Str_Urianalysis) Then
            Exit Sub
        End If

    End Sub

    Protected Sub lnkSTOOL_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.lnkChemistry.ForeColor = Drawing.Color.White
        Me.lnkCOAGULATION.ForeColor = Drawing.Color.White
        Me.lnkHEMATOLOGY.ForeColor = Drawing.Color.White
        Me.LnkIMMUNOLOGY.ForeColor = Drawing.Color.White
        Me.lnkURIANALYSIS.ForeColor = Drawing.Color.White
        Me.lnkSTOOL.ForeColor = Drawing.Color.HotPink
        Me.lnkHIV1.ForeColor = Drawing.Color.White
        Me.lnkPAP.ForeColor = Drawing.Color.White

        If Not FillGridview(Str_STOOLEXAMINATION) Then
            Exit Sub
        End If
    End Sub

    Protected Sub lnkHIV1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.lnkChemistry.ForeColor = Drawing.Color.White
        Me.lnkCOAGULATION.ForeColor = Drawing.Color.White
        Me.lnkHEMATOLOGY.ForeColor = Drawing.Color.White
        Me.LnkIMMUNOLOGY.ForeColor = Drawing.Color.White
        Me.lnkURIANALYSIS.ForeColor = Drawing.Color.White
        Me.lnkSTOOL.ForeColor = Drawing.Color.White
        Me.lnkHIV1.ForeColor = Drawing.Color.HotPink
        Me.lnkPAP.ForeColor = Drawing.Color.White

        If Not FillGridview(Str_HIV1) Then
            Exit Sub
        End If
    End Sub

    Protected Sub lnkPAP_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Me.lnkChemistry.ForeColor = Drawing.Color.White
        Me.lnkCOAGULATION.ForeColor = Drawing.Color.White
        Me.lnkHEMATOLOGY.ForeColor = Drawing.Color.White
        Me.LnkIMMUNOLOGY.ForeColor = Drawing.Color.White
        Me.lnkURIANALYSIS.ForeColor = Drawing.Color.White
        Me.lnkSTOOL.ForeColor = Drawing.Color.White
        Me.lnkHIV1.ForeColor = Drawing.Color.White
        Me.lnkPAP.ForeColor = Drawing.Color.HotPink

        If Not FillGridview(Str_PAPSmear) Then
            Exit Sub
        End If
    End Sub

#End Region

#Region "Grid gvwtstChemistry Events"

    Protected Sub gvwtstChemistry_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwtstChemistry.RowCommand
        Dim index As Integer = CType(e.CommandArgument, Integer)
        Dim Sampleid As String = String.Empty
        Dim Medexcode As String = String.Empty
        Try

            If e.CommandName.ToUpper = "AUDIT" Then

                Sampleid = Me.gvwtstChemistry.Rows(index).Cells(GVC_SampleId).Text.ToString.Trim()
                Medexcode = Me.gvwtstChemistry.Rows(index).Cells(GVC_Medexcode).Text.ToString.Trim()
                Me.lblMedexDescription.Text = Me.gvwtstChemistry.Rows(index).Cells(GVC_vMedexDesc).Text.Trim()


                FillGrid(Sampleid, Medexcode)
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "DivShowHide", "DivShowHide('S');", True)

                'divHistoryDtl.Attributes.Add("style", "display:block")
                'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('divHistoryDtl');", True)

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

    Protected Sub gvwtstChemistry_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwtstChemistry.RowCreated
        If e.Row.RowType = DataControlRowType.Header Or _
                    e.Row.RowType = DataControlRowType.DataRow Or _
                    e.Row.RowType = DataControlRowType.Footer Then


            e.Row.Cells(GVC_SampleId).Visible = False
            e.Row.Cells(GVC_tranno).Visible = False
            e.Row.Cells(GVC_Normalflag).Visible = False
            e.Row.Cells(GVC_Abnormalflag).Visible = False
            e.Row.Cells(GVC_ClinicallySignflag).Visible = False
            e.Row.Cells(GVC_GeneralRemark).Visible = False
            'e.Row.Cells(GVC_ChkNormal).Enabled = False
            e.Row.Cells(GVC_Medexcode).Visible = False

            'If Not (Me.Request.QueryString("Mode") Is Nothing) Then
            '    If Me.Request.QueryString("Mode") = 4 Or Me.Request.QueryString("Mode") = 0 Then
            e.Row.Cells(GVC_chkedit).Visible = False
            e.Row.Cells(GVC_ChkAbnormal).Visible = False
            e.Row.Cells(GVC_ChkClSignificant).Visible = False
            e.Row.Cells(GVC_Remarks).Visible = False
            ' End If
            'End If


        End If
    End Sub

    Protected Sub gvwtstChemistry_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwtstChemistry.RowDataBound

        Try
            If e.Row.RowType = DataControlRowType.DataRow Then

                e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + 1 + (Me.gvwtstChemistry.PageSize * Me.gvwtstChemistry.PageIndex)
                CType(e.Row.FindControl("ImgAudit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgAudit"), ImageButton).CommandName = "AUDIT"


                'If e.Row.Cells(GVC_GeneralRemark).Text.Trim() = "&nbsp;" Then
                '    e.Row.Cells(GVC_GeneralRemark).Text = ""
                'End If 'End if of e.Row.Cells(GVC_GeneralRemark).Text

                'CType(e.Row.FindControl("txtRemark"), TextBox).Text = e.Row.Cells(GVC_GeneralRemark).Text.Trim()

                If e.Row.Cells(GVC_Normalflag).Text.Trim() = "Y" Then
                    CType(e.Row.FindControl("ChkNormal"), CheckBox).BorderWidth = 1
                    CType(e.Row.FindControl("ChkNormal"), CheckBox).BorderColor = Drawing.Color.Red
                    CType(e.Row.FindControl("ChkNormal"), CheckBox).Checked = True
                End If 'End if of e.Row.Cells(GVC_Normalflag).Text

                If e.Row.Cells(GVC_Abnormalflag).Text.Trim() = "Y" Then

                    CType(e.Row.FindControl("ChkAbnormal"), CheckBox).BorderWidth = 1
                    CType(e.Row.FindControl("ChkAbnormal"), CheckBox).BorderColor = Drawing.Color.Red
                    CType(e.Row.FindControl("ChkAbnormal"), CheckBox).Checked = True
                End If 'End if of e.Row.Cells(GVC_Abnormalflag).Text

                If e.Row.Cells(GVC_ClinicallySignflag).Text.Trim() = "Y" Then
                    CType(e.Row.FindControl("ChkClSignificant"), CheckBox).BorderWidth = 1
                    CType(e.Row.FindControl("ChkClSignificant"), CheckBox).BorderColor = Drawing.Color.Red
                    CType(e.Row.FindControl("ChkClSignificant"), CheckBox).Checked = True
                End If 'End if of e.Row.Cells(GVC_ClinicallySignflag).Text

                If e.Row.Cells(GVC_tranno).Text.Trim() <> "0" Then
                    CType(e.Row.FindControl("ImgAudit"), ImageButton).Visible = True
                End If 'End if of e.Row.Cells(GVC_tranno).Text

                If (e.Row.Cells(GVC_vMedexResult).Text.ToString.ToUpper = "NEGATIVE") Or _
                   (e.Row.Cells(GVC_vMedexResult).Text.ToString.ToUpper = "ABSENT") Or _
                   (e.Row.Cells(GVC_vMedexResult).Text.ToString.ToUpper = "NON-REACTIVE") Then

                    CType(e.Row.FindControl("ChkNormal"), CheckBox).Checked = True
                    'or if e.Row.Cells(GVC_vMedexResult).Text.ToString.ToUpper="ABSENT" or if e.Row.Cells(GVC_vMedexResult).Text.ToString.ToUpper="NON-REACTIVE" then
                End If

            End If 'End if e.Row.RowType = DataControlRowType.DataRow

        Catch ex As Exception
            Me.ShowErrorMessage("Error while binding rows", ex.Message)
        End Try

    End Sub

#End Region

#Region "Button Events"

    'Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim ds_SaveLabrpt As New DataSet
    '    Dim eStr As String = String.Empty
    '    Dim ds As New DataSet
    '    Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
    '    Try
    '        AssignValue()

    '        ds_SaveLabrpt = CType(ViewState(VS_dsSubLabReportDtl), DataSet)

    '        If Not ObjLambda.Save_SubjectLabReportDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
    '                           ds_SaveLabrpt, Me.Session(S_UserID), eStr) Then

    '            Throw New Exception(eStr)

    '        End If

    '        ObjCommon.ShowAlert("Lab Report saved Successfully", Me)
    '        If Not GenCall_Data(ds) Then ' For Data Retrieval
    '            Exit Sub
    '        End If

    '        If Not GenCall_ShowUI() Then 'For Displaying Data
    '            Exit Sub
    '        End If

    '    Catch ex As Exception
    '        Me.ShowErrorMessage("Error while Saving SubjectLabReportDetail", ex.Message)
    '    Finally
    '    End Try

    'End Sub

    Protected Sub btnSubject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubject.Click
        'RefreshUpdatePanel2()
        fillScreeningDates()


    End Sub

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim Wstr As String = ""
        Dim estr As String = ""
        Dim ds_Subject As New DataSet
        Dim dt_Subject As New DataTable
        Dim dv_Subject As New DataView
        Dim index As Integer = 0
        Dim inHouseProjectCode As String = "0016"
        Dim ds_period As New DataSet

        Dim ds_Check As New DataSet
        Dim dv_Check As New DataView
        Dim ds_Subjects As New DataSet
        Dim ds_CreatedBy As New DataSet
        Dim dv_CreatedBy As New DataView
        Try

            'Wstr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D' Order by iTranNo desc"

            'If Not Me.ObjHelp.GetCRFLockDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '                    ds_Check, estr) Then
            '    Throw New Exception(estr)
            'End If

            'If Not ds_Check Is Nothing Then

            '    dv_Check = ds_Check.Tables(0).DefaultView
            '    'dv_Check.Sort = "iTranNo desc"
            '    If dv_Check.ToTable().Rows.Count > 0 Then

            '        If Convert.ToString(dv_Check.ToTable().Rows(0)("cLockFlag")).Trim.ToUpper() = "L" Then
            '            Me.ObjCommon.ShowAlert("Project/Site is Locked.", Me.Page)
            '            Me.txtproject.Text = ""
            '            Me.HProjectId.Value = ""
            '            Exit Sub

            '        End If

            '    End If

            'End If


            Wstr = "vWorkspaceId='" & Me.HProjectId.Value.ToString.Trim()
            Wstr += "' and cStatusIndi <> 'D' "
            If Not Me.ObjHelp.getworkspacemst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_period, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting View_Workspaceprotocol details", Me.Page)
                Exit Sub
            End If

            If ds_period.Tables.Count > 0 Then
                If ds_period.Tables(0).Rows(0).Item("vprojecttypecode") = inHouseProjectCode Then
                    trperiod.Visible = True
                    FillPeriodDropDown()

                ElseIf ds_period.Tables(0).Rows(0).Item("vprojecttypecode") <> inHouseProjectCode Then
                    trperiod.Visible = False
                    fillrblSubject()

                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try


    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Response.Redirect("frmReportReview.aspx")
    End Sub

    Protected Sub btnDivOK_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr_LabRpt As DataRow
        Dim eStr As String = String.Empty
        Dim ds_LabRptLockUnlockDtl As New DataSet


        Try

            ds_LabRptLockUnlockDtl = CType(ViewState(VS_dsLabRptLockUnlockDtl), DataSet)
            ds_LabRptLockUnlockDtl.Tables(0).Rows.Clear()
            dr_LabRpt = ds_LabRptLockUnlockDtl.Tables(0).NewRow
            'datarow
            dr_LabRpt("nLabRptLockUnlockDtlno") = 0
            dr_LabRpt("iTranNo") = 0
            dr_LabRpt("nSampleId") = Me.rblSampleid.SelectedValue
            dr_LabRpt("iReviewedBy") = Me.Session(S_UserID)
            dr_LabRpt("vRemarks") = Me.txtLockRemark.Text
            dr_LabRpt("cStatusIndi") = "N"
            'dr_LabRpt.AcceptChanges()
            ds_LabRptLockUnlockDtl.Tables(0).Rows.Add(dr_LabRpt)
            ds_LabRptLockUnlockDtl.Tables(0).AcceptChanges()


            If Not ObjLambda.Save_LabRptLockUnlockDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                           ds_LabRptLockUnlockDtl, Session(S_UserID), eStr) Then

                Throw New Exception(eStr)
            End If

            ObjCommon.ShowAlert("Lab Report saved successfully with Review Details", Me)

            If Me.gvwtstChemistry.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ChangedDisabled", "ChangedDisabled();", True)
            End If

        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)
        End Try
    End Sub

    Protected Sub btnDivCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.gvwtstChemistry.Rows.Count > 0 Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ChangedDisabled", "ChangedDisabled();", True)
        End If
    End Sub

    Protected Sub btnFnlRmkAudit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dt_ViewLabLockUnlockDtl As New DataTable


        If Not Me.ViewState(VS_ViewLabLockUnlockDtl) Is Nothing Then

            Me.lblSampleID.Text = Me.rblSampleid.SelectedItem.ToString
            dt_ViewLabLockUnlockDtl = CType(Me.ViewState(VS_ViewLabLockUnlockDtl), DataTable)
            GVAuditFnlRmk.DataSource = dt_ViewLabLockUnlockDtl
            GVAuditFnlRmk.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "DivAuditShowHide", "DivAuditShowHide('S');", True)

        ElseIf Me.ViewState(VS_ViewLabLockUnlockDtl) Is Nothing Then

            ObjCommon.ShowAlert("Report is not reviewed!", Me)

        End If

    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnTRF_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not FillgvwTRF() Then
            ObjCommon.ShowAlert("Error while filling TRF Grid", Me)
        End If
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "DivShowHideTRF", "DivShowHideTRF('S');", True)


    End Sub

#End Region

    '#Region "Assign Values"

    '    Private Sub AssignValue()
    '        Dim dt_SubjectLabReportDtl As New DataTable
    '        Dim dr_SubjectLabReportDtl As DataRow

    '        Try
    '            dt_SubjectLabReportDtl = CType(ViewState(VS_dsSubLabReportDtl), DataSet).Tables(0)

    '            For indexrow As Integer = 0 To Me.gvwtstChemistry.Rows.Count - 1

    '                If CType(Me.gvwtstChemistry.Rows(indexrow).FindControl("ChkEdit"), HtmlInputCheckBox).Checked = True Then

    '                    dr_SubjectLabReportDtl = dt_SubjectLabReportDtl.NewRow
    '                    dr_SubjectLabReportDtl("nSubjectLabReportNo") = 0
    '                    dr_SubjectLabReportDtl("nSampleId") = Me.gvwtstChemistry.Rows(indexrow).Cells(GVC_SampleId).Text.Trim
    '                    dr_SubjectLabReportDtl("vMedexCode") = Me.gvwtstChemistry.Rows(indexrow).Cells(GVC_Medexcode).Text.Trim
    '                    dr_SubjectLabReportDtl("itranNo") = Me.gvwtstChemistry.Rows(indexrow).Cells(GVC_tranno).Text.Trim

    '                    dr_SubjectLabReportDtl("cAbnormalflag") = "N"
    '                    If CType(Me.gvwtstChemistry.Rows(indexrow).FindControl("ChkAbnormal"), CheckBox).Checked = True Then
    '                        dr_SubjectLabReportDtl("cAbnormalflag") = "Y"
    '                    End If 'End if of CType(Me.gvwtstChemistry.Rows(indexrow).FindControl("ChkAbnormal"), CheckBox)

    '                    dr_SubjectLabReportDtl("cClinicallySignflag") = "N"
    '                    If CType(Me.gvwtstChemistry.Rows(indexrow).FindControl("ChkClSignificant"), CheckBox).Checked = True Then
    '                        dr_SubjectLabReportDtl("cClinicallySignflag") = "Y"
    '                    End If 'end if of CType(Me.gvwtstChemistry.Rows(indexrow).FindControl("ChkClSignificant"), CheckBox)

    '                    dr_SubjectLabReportDtl("vGeneralRemark") = CType(Me.gvwtstChemistry.Rows(indexrow).FindControl("txtRemark"), TextBox).Text.ToString.Trim
    '                    dr_SubjectLabReportDtl("iModifyBy") = Me.Session(S_UserID)
    '                    dr_SubjectLabReportDtl("cStatusIndi") = "N"
    '                    dt_SubjectLabReportDtl.Rows.Add(dr_SubjectLabReportDtl)
    '                    dt_SubjectLabReportDtl.AcceptChanges()

    '                End If 'end if of CType(Me.gvwtstChemistry.Rows(indexrow).FindControl("ChkEdit"), CheckBox).Checked 

    '            Next indexrow

    '        Catch ex As Exception
    '            ShowErrorMessage("Error while Assigning Value", ex.Message)
    '        Finally
    '        End Try
    '    End Sub

    '#End Region

#Region "GvHistoryDtl Events"

    Protected Sub GVHistoryDtl_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVHistoryDtl.RowCreated
        If e.Row.RowType = DataControlRowType.Header Or _
                e.Row.RowType = DataControlRowType.DataRow Or _
                e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVCHist_SampleId).Visible = False
            e.Row.Cells(GVCHist_MedexCode).Visible = False
            e.Row.Cells(GVCHist_TranNo).Visible = False

        End If
    End Sub

#End Region

#Region "GVAuditFnlRmkEvent"

    Protected Sub GVAuditFnlRmk_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVAuditFnlRmk.RowCreated
        If e.Row.RowType = DataControlRowType.Header Or _
            e.Row.RowType = DataControlRowType.DataRow Or _
            e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVCAudit_nSampleId).Visible = False
            e.Row.Cells(GVCAudit_iReviewedBy).Visible = False
            e.Row.Cells(GVCAudit_iTranNo).Visible = False

        End If
    End Sub

#End Region

#Region "FillCheckBoxList"

    Private Sub fillrblSubject()

        Dim Wstr As String = ""
        Dim estr As String = ""
        Dim ds_Subject As New DataSet
        Dim dt_Subject As New DataTable
        Dim dv_Subject As New DataView
        Dim index As Integer = 0

        Try

            'Wstr = "cStatusIndi <> 'D'"
            'RBLSubject.DataSource = Nothing
            'RBLSubject.DataBind()


            'Wstr += "vWorkspaceId='" & Me.HProjectId.Value.ToString.Trim() & _
            '         "' and iPeriod =1 and cStatusIndi <> 'D' and cRejectionflag<>'Y' and iMySubjectNo > 0 order by vMySubjectNo"

            'Wstr += "vWorkspaceId='" & Me.HProjectId.Value.ToString.Trim() & _
            '         "' and iPeriod =1 and cStatusIndi <> 'D'  and iMySubjectNo >= 0 order by dReportingDate"


            'added by vishal 
            Wstr = "vWorkspaceId='" & Me.HProjectId.Value.ToString.Trim()

            If trperiod.Visible = True Then

                Wstr += "' and iPeriod = " & ddlPeriods.SelectedValue
            Else

                Wstr += "' and iPeriod =1"
            End If

            Wstr += " and  cStatusIndi <> 'D' And  ((iMySubjectNo = 0 And cRejectionFlag <> 'Y') Or "
            Wstr += "(iMySubjectNo > 0 And cRejectionFlag = 'Y') or (iMySubjectNo > 0 And cRejectionFlag <> 'Y')) order by dReportingDate"


            If Not Me.ObjHelp.GetViewWorkspaceSubjectMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_Subject, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting View_WorkspaceSubjectMst", Me.Page)
                Exit Sub
            End If

            If ds_Subject.Tables(0).Rows.Count < 1 Then
                Me.pnlSubject.Visible = False

                Exit Sub
            End If

            dt_Subject = ds_Subject.Tables(0)

            'dt_Subject = New DataTable
            'dv_Subject = ds_Subject.Tables(0).DefaultView
            'dv_Subject.RowFilter = "iMySubjectNo > 0"
            'If dv_Subject.ToTable().Rows.Count > 0 Then
            'dv_Subject.Sort = "iMySubjectNo"
            'End If
            'dt_Subject = dv_Subject.ToTable()
            Me.RBLSubject.Items.Clear()



            For Each dr As DataRow In dt_Subject.Rows
                Me.RBLSubject.Items.Add(New ListItem(dr("vMySubjectNo") + "-" + dr("FullName"), dr("vSubjectId")))
                ' dt_Subject.Rows(index).Item("SubjectJoin") = dr("vMySubjectNo") + "-" + dr("FullName")
                index += 1

                If dr("cRejectionflag").ToString.ToUpper = "Y" Then
                    Me.RBLSubject.Items(index - 1).Attributes.Add("style", "color:red")
                End If

            Next dr

            'Me.RBLSubject.DataSource = dt_Subject
            'Me.RBLSubject.DataTextField = "SubjectJoin"
            'Me.RBLSubject.DataBind()
            Me.RBLSubject.Visible = True
            Me.pnlSubject.Visible = True



        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "SelectedIndexChange Event"

    Protected Sub RBLProjecttype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)


        refreshupSubjectSelection()
        refreshUpScreeningDtl()
        RefreshUpdatePanel2()
        trShowHide()

    End Sub

    Protected Sub rblScreeningDate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblScreeningDate.SelectedIndexChanged
        RefreshUpdatePanel2()
        Me.trbtnTRF.Attributes.Add("Style", "display:none")
        If Not fillSampleId() Then
            Exit Sub
        End If
        HideTr()

        'ds = ObjHelp.GetResultSet("Select distinct From ","")
    End Sub

    Protected Sub rblSampleid_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblSampleid.SelectedIndexChanged


        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim dv_SubLabrpt As New DataView
        Dim dt_SubLabrpt As New DataTable
        Dim ds_viewSublabRpt As New DataSet
        Dim Str_FirstGrp As String = String.Empty
        'Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            'Me.lbTestPerformed.Text = "Total No. of Test Requested Vs Total No of Test performed"

            RefreshUpdatePanel2()

            'btnTRF.Visible = True


            If Not GetViewSubjectLabRptDtl() Then

                ObjCommon.ShowAlert("Error while getting data from SubjectLabRptDtl", Me)
                Exit Sub
            End If

            ds_viewSublabRpt = CType(ViewState(VS_dsSublabRpt), DataSet)

            If Not ds_viewSublabRpt.Tables(0).Rows.Count > 0 Then
                Me.ObjCommon.ShowAlert("Lab report is not released from Lab.", Me.Page)
                Exit Sub
            End If




            TestPerform = ds_viewSublabRpt.Tables(0).Rows.Count

            'fillTestPerformDtl(Me.rblSampleid.SelectedValue.ToString)

            'to show/hide linkbutton if present in Sample
            dv_SubLabrpt = ds_viewSublabRpt.Tables(0).DefaultView
            dt_SubLabrpt = dv_SubLabrpt.ToTable(True, "vMedexGroupCode")

            For index As Integer = 0 To dt_SubLabrpt.Rows.Count - 1

                If dt_SubLabrpt.Rows(index)("vMedexGroupCode") = GeneralModule.MedexGrp_Chemistry Then
                    Me.lnkChemistry.Attributes.Add("style", "Display:block")
                    If Str_FirstGrp.Trim = String.Empty Then
                        Str_FirstGrp = Str_Chemistry

                    End If

                ElseIf dt_SubLabrpt.Rows(index)("vMedexGroupCode") = GeneralModule.MedexGrp_IMMUNOLOGY Then
                    Me.LnkIMMUNOLOGY.Attributes.Add("style", "Display:block")

                    If Str_FirstGrp.Trim = String.Empty Then
                        Str_FirstGrp = Str_Immunology
                    End If

                ElseIf dt_SubLabrpt.Rows(index)("vMedexGroupCode") = GeneralModule.MedexGrp_HEMATOLOGY Then
                    Me.lnkHEMATOLOGY.Attributes.Add("style", "Display:block")

                    If Str_FirstGrp.Trim = String.Empty Then
                        Str_FirstGrp = Str_Hematology
                    End If
                ElseIf dt_SubLabrpt.Rows(index)("vMedexGroupCode") = GeneralModule.MedexGrp_URIANALYSIS Then
                    Me.lnkURIANALYSIS.Attributes.Add("style", "Display:block")

                    If Str_FirstGrp.Trim = String.Empty Then
                        Str_FirstGrp = Str_Urianalysis
                    End If

                ElseIf dt_SubLabrpt.Rows(index)("vMedexGroupCode") = GeneralModule.MedexGrp_COAGULATION Then
                    Me.lnkCOAGULATION.Attributes.Add("style", "Display:block")

                    If Str_FirstGrp.Trim = String.Empty Then
                        Str_FirstGrp = Str_Coagulation
                    End If
                ElseIf dt_SubLabrpt.Rows(index)("vMedexGroupCode") = GeneralModule.MedexGrp_STOOL Then
                    Me.lnkSTOOL.Attributes.Add("style", "Display:block")

                    If Str_FirstGrp.Trim = String.Empty Then
                        Str_FirstGrp = Str_STOOLEXAMINATION
                    End If
                ElseIf dt_SubLabrpt.Rows(index)("vMedexGroupCode") = GeneralModule.MedexGrp_HIV1 Then
                    Me.lnkHIV1.Attributes.Add("style", "Display:block")

                    If Str_FirstGrp.Trim = String.Empty Then
                        Str_FirstGrp = Str_HIV1
                    End If

                ElseIf dt_SubLabrpt.Rows(index)("vMedexGroupCode") = MedexGrp_PAPSmear Then
                    Me.lnkPAP.Attributes.Add("style", "Display:block")

                    If Str_FirstGrp.Trim = String.Empty Then
                        Str_FirstGrp = Str_PAPSmear
                    End If


                End If 'end if of "dt_SubLabrpt.Rows(index)("vMedexGroupCode")" 

                'If Str_FirstGrp.Trim = String.Empty Then
                '    Str_FirstGrp = dt_SubLabrpt.Rows(index)("vMedexGroupCode")
                'End If 'end if of Str_FirstGrp.Trim = String.Empty

            Next index




            If Not FillGridview(Str_FirstGrp) Then
                Me.ObjCommon.ShowAlert("Error while Filling Grid", Me)
            End If


            lblReviedby.Text = ""
            Me.ViewState(VS_ViewLabLockUnlockDtl) = Nothing
            Me.chkReviewed.Checked = False

            If Not ds_viewSublabRpt.Tables(0).Rows.Count <= 0 Then
                If Not GetRevierName(Me.rblSampleid.SelectedValue.ToString) Then
                    Me.ObjCommon.ShowAlert("Error while Getting Reviewer Name", Me)
                End If
                Me.trReviewed.Attributes.Add("Style", "display:block")
            End If
            Me.trbtnTRF.Attributes.Add("Style", "display:block")





        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)
        End Try



    End Sub

    Protected Sub chkInProject_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Me.chkInProject.Checked = True Then
                If Not FillActivity() Then
                    Throw New Exception
                End If
                Me.trActivity.Attributes.Add("style", "display:block")
                Me.rblScreeningDate.Enabled = False

            ElseIf Me.chkInProject.Checked = False Then
                Me.ddlActivity.Items.Clear()
                Me.ddlActivity.DataSource = Nothing
                Me.ddlActivity.DataBind()
                Me.trActivity.Attributes.Add("style", "display:none")
                Me.rblScreeningDate.Enabled = True
            End If

            fillrblSubject()

        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)
        End Try

    End Sub

    Protected Sub RBLSubject_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'RefreshUpdatePanel2()
        If Me.chkInProject.Checked = True Then
            fillSampleId()
        ElseIf Me.chkInProject.Checked = False Then
            fillScreeningDates()
        End If

        RefreshUpdatePanel2()

    End Sub

#End Region

#Region "Fill Function"

    Private Sub FillPeriodDropDown()
        Dim wStr As String = ""
        Dim dsPeriod As New DataSet
        Dim iPeriodNumbers As Integer = 0
        Dim eStr As String = String.Empty

        wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
        If Not ObjHelp.GetWorkspaceProtocolDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                        dsPeriod, eStr) Then
            Me.ShowErrorMessage(eStr, "")
            Exit Sub
        End If


        Me.ddlPeriods.Items.Clear()
        Me.ddlPeriods.Items.Insert(0, New ListItem("Select Period", "0"))
        If dsPeriod.Tables(0).Rows.Count > 0 Then
            iPeriodNumbers = dsPeriod.Tables(0).Rows(0).Item("iNoOfPeriods")
        End If
        If iPeriodNumbers > 0 Then
            For count As Integer = 1 To dsPeriod.Tables(0).Rows(0).Item("iNoOfPeriods")
                Me.ddlPeriods.Items.Add(count.ToString())
            Next

            Me.ddlPeriods.SelectedValue = iPeriodNumbers

        End If


        'If Not IsNothing(Me.Request.QueryString("PeriodId")) Then
        '    Me.ddlPeriods.SelectedValue = Me.Request.QueryString("PeriodId").Trim()
        '    Me.ddlPeriods.Enabled = False
        'End If

    End Sub

    Private Function GetRptRemarks(ByVal ds_LabDtl As DataSet, ByVal medexgrp As String) As Boolean

        Dim Str_remark As String
        Dim dv_LabDtl As New DataView
        Dim dr_LabDtl As DataRow
        Dim Ch_Remark As Char
        Dim iAsciiValue As Integer
        Try

            GetRptRemarks = False

            dv_LabDtl = ds_LabDtl.Tables(0).DefaultView
            dv_LabDtl.RowFilter = "vmedexgroupcode = '" + medexgrp + "'"
            dr_LabDtl = dv_LabDtl.ToTable.Rows(0)

            If Not dr_LabDtl("vRemarkValue") Is System.DBNull.Value Then
                If Not dr_LabDtl("vRemarkValue") = "" Then
                    Str_remark = HTMLTOText(dr_LabDtl("vRemarkValue"))
                    If Not IsNothing(Str_remark) Then
                        For Each Ch_Remark In Str_remark
                            iAsciiValue = Asc(Ch_Remark)
                            If iAsciiValue = 13 Then
                                Ch_Remark = " "
                            End If
                            Str_Rev += Ch_Remark
                        Next

                        GetRptRemarks = True
                    End If 'IsNothing(Str_remark)
                End If 'end if of dr_LabDtl("vRemarkValue") = ""
            End If '
        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)
            GetRptRemarks = False
        End Try


    End Function

    Private Function HTMLTOText(ByVal source As String) As String
        Try
            Dim result As String

            ' Remove HTML Development formatting
            ' Replace line breaks with space
            ' because browsers inserts space
            result = source.Replace(vbCr, " ")
            ' Replace line breaks with space
            ' because browsers inserts space
            result = result.Replace(vbLf, " ")
            ' Remove step-formatting
            result = result.Replace(vbTab, String.Empty)
            ' Remove repeating spaces because browsers ignore them
            result = System.Text.RegularExpressions.Regex.Replace(result, "( )+", " ")

            ' Remove the header (prepare first by clearing attributes)
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*head([^>])*>", "<head>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "(<( )*(/)( )*head( )*>)", "</head>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "(<head>).*(</head>)", String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase)

            ' remove all scripts (prepare first by clearing attributes)
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*script([^>])*>", "<script>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "(<( )*(/)( )*script( )*>)", "</script>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            'result = System.Text.RegularExpressions.Regex.Replace(result,
            '         @"(<script>)([^(<script>\.</script>)])*(</script>)",
            '         string.Empty,
            '         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "(<script>).*(</script>)", String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase)

            ' remove all styles (prepare first by clearing attributes)
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*style([^>])*>", "<style>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "(<( )*(/)( )*style( )*>)", "</style>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "(<style>).*(</style>)", String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase)

            ' insert tabs in spaces of <td> tags
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*td([^>])*>", vbTab, System.Text.RegularExpressions.RegexOptions.IgnoreCase)

            ' insert line breaks in places of <BR> and <LI> tags
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*br( )*>", vbCr, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*li( )*>", vbCr, System.Text.RegularExpressions.RegexOptions.IgnoreCase)

            ' insert line paragraphs (double line breaks) in place
            ' if <P>, <DIV> and <TR> tags
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*div([^>])*>", vbCr & vbCr, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*tr([^>])*>", vbCr & vbCr, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*p([^>])*>", vbCr & vbCr, System.Text.RegularExpressions.RegexOptions.IgnoreCase)

            ' Remove remaining tags like <a>, links, images,
            ' comments etc - anything that's enclosed inside < >
            result = System.Text.RegularExpressions.Regex.Replace(result, "<[^>]*>", String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase)

            ' replace special characters:
            result = System.Text.RegularExpressions.Regex.Replace(result, " ", " ", System.Text.RegularExpressions.RegexOptions.IgnoreCase)

            result = System.Text.RegularExpressions.Regex.Replace(result, "&bull;", " * ", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "&lsaquo;", "<", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "&rsaquo;", ">", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "&trade;", "(tm)", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "&frasl;", "/", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "&lt;", "<", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "&gt;", ">", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "&copy;", "(c)", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "&reg;", "(r)", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ' Remove all others. More can be added, see
            ' http://hotwired.lycos.com/webmonkey/reference/special_characters/
            result = System.Text.RegularExpressions.Regex.Replace(result, "&(.{2,6});", String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase)

            ' for testing
            'System.Text.RegularExpressions.Regex.Replace(result,
            '       this.txtRegex.Text,string.Empty,
            '       System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            ' make line breaking consistent
            result = result.Replace(vbLf, vbCr)

            ' Remove extra line breaks and tabs:
            ' replace over 2 breaks with 2 and over 4 tabs with 4.
            ' Prepare first to remove any whitespaces in between
            ' the escaped characters and remove redundant tabs in between line breaks
            result = System.Text.RegularExpressions.Regex.Replace(result, "(" & vbCr & ")( )+(" & vbCr & ")", vbCr & vbCr, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "(" & vbTab & ")( )+(" & vbTab & ")", vbTab & vbTab, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "(" & vbTab & ")( )+(" & vbCr & ")", vbTab & vbCr, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "(" & vbCr & ")( )+(" & vbTab & ")", vbCr & vbTab, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ' Remove redundant tabs
            result = System.Text.RegularExpressions.Regex.Replace(result, "(" & vbCr & ")(" & vbTab & ")+(" & vbCr & ")", vbCr & vbCr, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ' Remove multiple tabs following a line break with just one tab
            result = System.Text.RegularExpressions.Regex.Replace(result, "(" & vbCr & ")(" & vbTab & ")+", vbCr & vbTab, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ' Initial replacement target string for line breaks
            Dim breaks As String = vbCr & vbCr & vbCr
            ' Initial replacement target string for tabs
            Dim tabs As String = vbTab & vbTab & vbTab & vbTab & vbTab
            For index As Integer = 0 To result.Length - 1
                result = result.Replace(breaks, vbCr & vbCr)
                result = result.Replace(tabs, vbTab & vbTab & vbTab & vbTab)
                breaks = breaks & vbCr
                tabs = tabs & vbTab
            Next
            result = Replace(result, vbCrLf, " ")
            Return result
        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)
            Return source
        End Try
    End Function

    Private Function fillScreeningDates() As Boolean
        Dim ds_AuditTrail As New DataSet
        Dim dv_AuditTrail As New DataView
        Dim Wstr As String = ""
        Dim estr As String = ""
        Try
            Me.rblScreeningDate.Items.Clear()
            'To remove all the previous filled records :Start
            Me.rblSampleid.Items.Clear()
            HideTr()
            RefreshUpdatePanel2()
            'To remove all the previous filled records :End
            If Not (Me.ViewState(VS_ScrDate) Is Nothing Or Me.ViewState(VS_ScrDate) = "") Then
                Wstr = "nMedExScreeningHdrNo = " + CType(ViewState(VS_ScrDate), String)
            ElseIf Not IsNothing(Me.Request.QueryString("SubjectId")) Then
                Wstr = "vSubjectId='" & CType(ViewState(VS_SubjectID), String).Trim & "'"
            Else
                If RBLProjecttype.SelectedValue = "0000000000" Then
                    Wstr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "'"
                Else
                    Wstr = "vSubjectId='" + Me.RBLSubject.SelectedValue + "'"
                End If 'end if of RBLProjecttype.SelectedValue = "0000000000"

            End If 'end if of IsNothing(Me.Request.QueryString("SubjectId"))
            If Not Me.ObjHelp.View_MedExScreeningHdrDtlAuditTrail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                             ds_AuditTrail, estr) Then
                Exit Function
            End If
            dv_AuditTrail = ds_AuditTrail.Tables(0).DefaultView().ToTable(True, "dScreenDate,vReviewBy,nMedExScreeningHdrNo".Split(",")).DefaultView()
            dv_AuditTrail.Sort = "dScreenDate desc"
            ViewState(VS_dtMedExScreeningHdrDtl) = dv_AuditTrail.ToTable
            Me.rblScreeningDate.Items.Clear()
            For Each dr As DataRow In dv_AuditTrail.ToTable().Rows

                'If Not Me.ObjHelp.ChkLockedScreenDate(dr("nMedExScreeningHdrNo")) AndAlso CType(ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

                '    Me.rblScreeningDate.Items.Add(New ListItem(CDate(dr("dScreenDate")).ToString("dd-MMM-yyyy") + _
                '                                                 "(Locked)", dr("dScreenDate"), False))
                'Else

                Me.rblScreeningDate.Items.Add(New ListItem(CDate(dr("dScreenDate")).ToString("dd-MMM-yyyy"), dr("nMedExScreeningHdrNo")))
                'End If
            Next dr
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function fillSampleId() As Boolean
        Dim wstr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_Sampledetail As New DataSet

        Try


            Me.rblSampleid.Items.Clear()

            If Me.chkInProject.Checked = True Then
                wstr = Me.HschemaId.Value.Trim() + "##" + Me.HProjectId.Value + "##" + Me.ddlActivity.SelectedValue + "##" + Me.RBLSubject.SelectedValue.ToString + "##" + "0" + "##"


            ElseIf Me.chkInProject.Checked = False Then

                wstr = Me.HschemaId.Value.Trim() + "##" + "" + "##" + "0" + "##" + "" + "##" + Me.rblScreeningDate.SelectedValue + "##"
            End If


            If Not ObjHelp.Proc_SampleDetail(wstr, ds_Sampledetail, eStr) Then

                Throw New Exception(eStr)

            End If


            Me.rblSampleid.Items.Clear()

            For Each drSampledetail As DataRow In ds_Sampledetail.Tables(0).Rows

                'Me.rblSampleid.Items.Add(New ListItem(drSampledetail("vSampleID").ToString, drSampledetail("nSampleID").ToString))
                Me.rblSampleid.Items.Add(New ListItem(drSampledetail("vSampleID").ToString _
                + "(Collected on:" + CDate(drSampledetail("dCollectionDateTime")).ToString("dd-MMM-yyyy") + ")" _
                     + IIf(drSampledetail("cFollowUp") = "N", "", "(Follow UP)"), drSampledetail("nSampleID").ToString))

            Next drSampledetail
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)
            Return False
        End Try

    End Function

    Private Function GetRevierName(ByVal SampleId As String) As Boolean

        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_ViewLabLockUnlockDtl As New DataSet

        Try

            wStr = "nSampleID = " + SampleId + " And cStatusindi <>'D' order by iTranNo Desc"

            If Not ObjHelp.View_LabRptLockUnlockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                          ds_ViewLabLockUnlockDtl, eStr) Then

                Throw New Exception(eStr)

            End If



            If Not ds_ViewLabLockUnlockDtl.Tables(0).Rows.Count <= 0 Then

                Me.ViewState(VS_ViewLabLockUnlockDtl) = ds_ViewLabLockUnlockDtl.Tables(0)
                lblReviedby.Text = "Last Reviewed By : " + ds_ViewLabLockUnlockDtl.Tables(0).Rows(0)("vUserName").ToString

            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)
            Return False
        End Try

    End Function

    'Private Function fillTestPerformDtl(ByVal SampleId As String) As Boolean
    '    Dim eStr As String = String.Empty
    '    Dim wStr As String = String.Empty
    '    Dim ds_SampleMedexDtl As New DataSet
    '    Dim dt_SamMedexDtl As New DataTable
    '    'Dim dv_SampleMedexDtl As New DataView
    '    Try
    '        Me.lbTestPerformed.Text = "Total No. of Test Requested Vs Total No of Test performed"
    '        wStr = "nSampleid = " + SampleId + " and iTranNo=1  and iRepeatationNo =1"
    '        If Not ObjHelp.Get_ViewSampleMedExDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                    ds_SampleMedexDtl, eStr) Then
    '            Throw New Exception(eStr)
    '        End If
    '        dt_SamMedexDtl = ds_SampleMedexDtl.Tables(0).Copy
    '        'dv_SampleMedexDtl = ds_SampleMedexDtl.Tables(0).DefaultView
    '        'dv_SampleMedexDtl.RowFilter = " cApprovedFlag ='Y'"


    '        Me.lbTestPerformed.Text = "Total No. of Test Requested = " & dt_SamMedexDtl.Rows.Count.ToString + _
    '            " Vs Total No of Test perfomed = " + TestPerform.ToString


    '    Catch ex As Exception
    '        Me.ShowErrorMessage("", ex.Message)
    '    End Try


    'End Function

    Private Function FillActivity() As Boolean
        Dim Str As String = ""
        Dim Ds_FillActivity As New DataSet
        Dim dv_FillActivity As New DataView
        Dim dt_FillActivity As New DataTable
        Dim eStr_Retu As String = ""
        Try



            ' Str = " Select distinct(iNodeID),vNodeDisplayName from View_SubjectLabRptDtl where vWorkspaceId = '" & Me.HProjectId.Value.Trim() + "'"
            Str = Me.HschemaId.Value.Trim() + "##" + "0" + "##" + Me.HProjectId.Value + "##"
            If Not ObjHelp.Proc_SubjectLabRptDtl(Str, Ds_FillActivity, eStr_Retu) Then
                Throw New Exception
            End If

            If Ds_FillActivity.Tables(0).Rows.Count <= 0 Then
                ObjCommon.ShowAlert("No Activity found on which Lab Report is attached", Me)
                Return True
                Exit Function
            End If
            dv_FillActivity = Ds_FillActivity.Tables(0).DefaultView
            dt_FillActivity = dv_FillActivity.ToTable(True, "iNodeID,vNodeDisplayName".Split(","))
            Me.ddlActivity.DataSource = dt_FillActivity
            Me.ddlActivity.DataValueField = "iNodeid"
            Me.ddlActivity.DataTextField = "vNodeDisplayName"
            Me.ddlActivity.DataBind()


            Me.ddlActivity.Items.Insert(0, New ListItem("Select Activity", 0))


            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function GetViewSubjectLabRptDtl() As Boolean
        Dim wstr As String = String.Empty
        Dim ds_viewSublabRpt As New DataSet
        Dim eStr As String = String.Empty

        Try
            wstr = Me.HschemaId.Value.Trim() + "##" + Me.rblSampleid.SelectedValue.ToString + "##" + "" + "##"

            If Not ObjHelp.Proc_SubjectLabRptDtl(wstr, ds_viewSublabRpt, eStr) Then

                Throw New Exception(eStr)

            End If
            ViewState(VS_dsSublabRpt) = ds_viewSublabRpt
            If Not FillScreeningDtl(ds_viewSublabRpt) Then
                Return False
                Exit Function
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "Refresh UpdatePanels"

    Private Sub trShowHide()

        If Me.RBLProjecttype.SelectedValue = "0000000000" Then
            Me.trSearchSub.Attributes.Add("style", "display:block")
            Me.trSubList.Attributes.Add("style", "display:none")
            Me.trProject.Attributes.Add("style", "display:none")

        ElseIf Me.RBLProjecttype.SelectedValue = "1" Then
            Me.trSubList.Attributes.Add("style", "display:block")
            Me.trProject.Attributes.Add("style", "display:block")
            Me.trSearchSub.Attributes.Add("style", "display:none")

        End If

    End Sub

    Private Function RefreshUpdatePanel2() As Boolean
        'lbTestPerformed.Text = ""
        lnkChemistry.Attributes.Add("style", "display:none")
        lnkCOAGULATION.Attributes.Add("style", "display:none")
        lnkHEMATOLOGY.Attributes.Add("style", "display:none")
        lnkHIV1.Attributes.Add("style", "display:none")
        LnkIMMUNOLOGY.Attributes.Add("style", "display:none")
        lnkPAP.Attributes.Add("style", "display:none")
        lnkSTOOL.Attributes.Add("style", "display:none")
        lnkURIANALYSIS.Attributes.Add("style", "display:none")
        trRptRemarks.Attributes.Add("style", "display:none")
        txtRptRemarks.Value = ""
        lblReviedby.Text = ""
        trReviewed.Attributes.Add("style", "display:none")
        Me.gvwtstChemistry.DataSource = Nothing
        Me.gvwtstChemistry.DataBind()
    End Function

    Private Function refreshUpScreeningDtl() As Boolean
        Me.rblScreeningDate.Items.Clear()
        Me.rblSampleid.Items.Clear()
        Me.rblScreeningDate.Enabled = True
        Me.trbtnTRF.Attributes.Add("Style", "display:none")
    End Function

    Private Function refreshupSubjectSelection() As Boolean
        Me.txtSubject.Text = ""
        Me.HSubjectId.Value = ""
        Me.txtproject.Text = ""
        Me.HProjectId.Value = ""
        Me.chkInProject.Checked = False
        Me.ddlActivity.DataSource = Nothing
        Me.ddlActivity.DataBind()
        Me.trActivity.Attributes.Add("style", "display:none")
        Me.RBLSubject.Items.Clear()

        'for refreshing colour
        Me.lnkChemistry.ForeColor = Drawing.Color.White
        Me.lnkCOAGULATION.ForeColor = Drawing.Color.White
        Me.lnkHEMATOLOGY.ForeColor = Drawing.Color.White
        Me.LnkIMMUNOLOGY.ForeColor = Drawing.Color.White
        Me.lnkURIANALYSIS.ForeColor = Drawing.Color.White
        Me.lnkSTOOL.ForeColor = Drawing.Color.White
        Me.lnkHIV1.ForeColor = Drawing.Color.White
        Me.lnkPAP.ForeColor = Drawing.Color.White

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

#Region "GVWTRF Events"

    Protected Sub gvwTRF_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwTRF.RowCreated
        If e.Row.RowType = DataControlRowType.Header Or _
                    e.Row.RowType = DataControlRowType.DataRow Or _
                    e.Row.RowType = DataControlRowType.Footer Then


            e.Row.Cells(GVCTRF_nSampleId).Visible = False
        End If
    End Sub

    Protected Sub gvwTRF_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwTRF.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVCTRF_SrNo).Text = e.Row.RowIndex + 1 + (Me.gvwTRF.PageSize * Me.gvwTRF.PageIndex)
        End If
    End Sub

#End Region

#Region "gvwSampletype Event"
    Protected Sub gvwSampletype_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwSampletype.RowCreated
        If e.Row.RowType = DataControlRowType.Header Or _
            e.Row.RowType = DataControlRowType.DataRow Or _
            e.Row.RowType = DataControlRowType.Footer Then


            e.Row.Cells(GVSampletype_nSampleId).Visible = False
        End If
    End Sub

    Protected Sub gvwSampletype_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwSampletype.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVSampletype_SrNo).Text = e.Row.RowIndex + 1 + (Me.gvwSampletype.PageSize * Me.gvwSampletype.PageIndex)
        End If
    End Sub
#End Region

#Region "FillScreeningDtl"
    ' to show or hide tr : Start
    Public Function FillScreeningDtl(ByVal ds_viewSublabRpt As DataSet) As Boolean

        If Not ds_viewSublabRpt.Tables(0).Rows.Count > 0 Then
            HideTr()
            Me.ObjCommon.ShowAlert("Lab report is not released from lab.", Me.Page)
            Return False
            Exit Function
        End If
        ShowTr()
        Me.LblScrNo.Text = IIf(ds_viewSublabRpt.Tables(0).Rows(0)("SubjectName") Is DBNull.Value, "", ds_viewSublabRpt.Tables(0).Rows(0)("SubjectName"))
        Me.LblSampleClDt.Text = IIf(ds_viewSublabRpt.Tables(0).Rows(0)("dSampleReceivedDateTime") Is DBNull.Value, "", ds_viewSublabRpt.Tables(0).Rows(0)("dSampleReceivedDateTime")).GetDateTimeFormats()(23)
        Me.LblSub.Text = IIf(ds_viewSublabRpt.Tables(0).Rows(0)("vInitials") Is DBNull.Value, "", ds_viewSublabRpt.Tables(0).Rows(0)("vInitials"))
        Dim Sex As String
        Sex = IIf(ds_viewSublabRpt.Tables(0).Rows(0)("cSex") Is DBNull.Value, "", ds_viewSublabRpt.Tables(0).Rows(0)("cSex"))
        Me.LblSex.Text = ""
        If Sex = "M" Then
            Me.LblSex.Text = "Male"
        ElseIf Sex = "F" Then
            Me.LblSex.Text = "Female"
        End If
        Me.LblSampleClAt.Text = IIf(ds_viewSublabRpt.Tables(0).Rows(0)("vLocationName") Is DBNull.Value, "", ds_viewSublabRpt.Tables(0).Rows(0)("vLocationName"))
        Me.LblSampleRcvDt.Text = IIf(ds_viewSublabRpt.Tables(0).Rows(0)("dReceivedOnDate") Is DBNull.Value, "", ds_viewSublabRpt.Tables(0).Rows(0)("dReceivedOnDate")).GetDateTimeFormats()(23)
        Me.LblLabID.Text = IIf(ds_viewSublabRpt.Tables(0).Rows(0)("vSampleId") Is DBNull.Value, "", ds_viewSublabRpt.Tables(0).Rows(0)("vSampleId"))
        Me.LblRptDt.Text = IIf(ds_viewSublabRpt.Tables(0).Rows(0)("dModifyOn") Is DBNull.Value, "", ds_viewSublabRpt.Tables(0).Rows(0)("dModifyOn")).GetDateTimeFormats()(23)
        Me.LblBDate.Text = IIf(ds_viewSublabRpt.Tables(0).Rows(0)("dBirthDate") Is DBNull.Value, "", ds_viewSublabRpt.Tables(0).Rows(0)("dBirthDate"))
        Me.LblPrj.Text = IIf(ds_viewSublabRpt.Tables(0).Rows(0)("vProjectNo") Is DBNull.Value, "", ds_viewSublabRpt.Tables(0).Rows(0)("vProjectNo"))
        Me.LblRfrdBy.Text = IIf(ds_viewSublabRpt.Tables(0).Rows(0)("vReferredBy") Is DBNull.Value, "", ds_viewSublabRpt.Tables(0).Rows(0)("vReferredBy"))
        Me.LblAuthdBy.Text = IIf(ds_viewSublabRpt.Tables(0).Rows(0)("iMedexApprovedBy") Is DBNull.Value, "", ds_viewSublabRpt.Tables(0).Rows(0)("iMedexApprovedBy"))
        Me.lblSubNo.Text = IIf(ds_viewSublabRpt.Tables(0).Rows(0)("vMySubjectNo") Is DBNull.Value, "", ds_viewSublabRpt.Tables(0).Rows(0)("vMySubjectNo"))
        Me.LblRtpAutDate.Text = IIf(ds_viewSublabRpt.Tables(0).Rows(0)("MedexGroupApprovedDate") Is DBNull.Value, "", ds_viewSublabRpt.Tables(0).Rows(0)("MedexGroupApprovedDate")).GetDateTimeFormats()(23)

        Return True

    End Function

    Private Sub ShowTr()
        Me.TrTop.Visible = True
        Me.TrScreening.Visible = True
        Me.Tr1.Visible = True
        Me.Tr2.Visible = True
        Me.Tr3.Visible = True
        Me.Tr4.Visible = True
        Me.TrTopLine.Visible = True
        Me.TrBotomTopLine.Visible = True
        Me.TrFooter.Visible = True
        Me.TrFooterLine.Visible = True
    End Sub

    Private Sub HideTr()
        Me.TrTop.Visible = False
        Me.TrScreening.Visible = False
        Me.Tr1.Visible = False
        Me.Tr2.Visible = False
        Me.Tr3.Visible = False
        Me.Tr4.Visible = False
        Me.TrTopLine.Visible = False
        Me.TrBotomTopLine.Visible = False
        Me.TrFooter.Visible = False
        Me.TrFooterLine.Visible = False
    End Sub
    ' to show or hide tr : End
#End Region

    Protected Sub ddlPeriods_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPeriods.SelectedIndexChanged

        fillrblSubject()
    End Sub
End Class
